############################
# Data
############################

data "aws_ami" "amazon_linux" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["al2023-ami-2023.10.20260202.2-kernel-6*-x86_64"]
  }

  filter {
    name   = "root-device-type"
    values = ["ebs"]
  }
}

############################
# EC2 instances
############################

resource "aws_instance" "web" {
  count                  = 2
  ami                    = data.aws_ami.amazon_linux.id
  instance_type          = "t2.micro"
  subnet_id              = module.vpc.private_subnets[count.index]
  vpc_security_group_ids = [aws_security_group.ec2.id]
  user_data              = local.user_data

  root_block_device {
    volume_size = 8
    volume_type = "gp3"
    encrypted   = true
  }

  iam_instance_profile = aws_iam_instance_profile.ec2_ssm.name

  tags = merge(local.tags, {
    Name = "web-${count.index + 1}"
  })
}

############################
# ALB
############################

resource "aws_lb" "this" {
  name               = "ssm-demo-alb-${var.environment}"
  internal           = false
  load_balancer_type = "application"
  subnets            = module.vpc.public_subnets
  security_groups    = [aws_security_group.alb.id]

  tags = local.tags
}

resource "aws_lb_listener" "http" {
  load_balancer_arn = aws_lb.this.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.this.arn
  }

  tags = local.tags
}

resource "aws_lb_target_group" "this" {
  name     = "ssm-demo-tg-${var.environment}"
  port     = 80
  protocol = "HTTP"
  vpc_id   = module.vpc.vpc_id

  health_check {
    enabled             = true
    healthy_threshold   = 2
    interval            = 30
    matcher             = "200"
    path                = "/health"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 2
  }

  tags = local.tags
}

resource "aws_lb_target_group_attachment" "this" {
  count            = 2
  target_group_arn = aws_lb_target_group.this.arn
  target_id        = aws_instance.web[count.index].id
  port             = 80
}

