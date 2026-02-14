############################
# Data
############################

data "aws_availability_zones" "available" {}

data "aws_region" "current" {}

############################
# VPC
############################

module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "~> 5.0"

  name = "ssm-demo-${var.environment}"
  cidr = local.vpc_cidr

  azs = local.azs
  public_subnets = [
    cidrsubnet(
      local.vpc_cidr,
      local.subnet.newbits,
    local.subnet.public_offset),
    cidrsubnet(
      local.vpc_cidr,
      local.subnet.newbits,
    local.subnet.public_offset + 1)
  ]
  private_subnets = [
    cidrsubnet(
      local.vpc_cidr,
      local.subnet.newbits,
    local.subnet.private_offset),
    cidrsubnet(
      local.vpc_cidr,
      local.subnet.newbits,
    local.subnet.private_offset + 1)
  ]

  enable_dns_support   = true
  enable_dns_hostnames = true

  create_igw = true

  # NAT Gateways
  enable_nat_gateway     = local.create_nat
  single_nat_gateway     = false
  one_nat_gateway_per_az = true

  enable_ipv6 = false

  tags = local.tags
}

############################
# Security Groups
############################

resource "aws_security_group" "alb" {
  name   = "alb-sg"
  vpc_id = module.vpc.vpc_id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = local.tags
}

resource "aws_security_group" "ec2" {
  name   = "ec2-sg"
  vpc_id = module.vpc.vpc_id

  ingress {
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    security_groups = [aws_security_group.alb.id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = local.tags
}
