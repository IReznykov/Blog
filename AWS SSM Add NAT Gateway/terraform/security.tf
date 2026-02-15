############################
# Automation Role
############################

resource "aws_iam_role" "automation" {
  name = "SSMDemoAutomationRole-${var.environment}"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect    = "Allow"
      Principal = { Service = "ssm.amazonaws.com" }
      Action    = "sts:AssumeRole"
    }]
  })

  tags = local.tags
}

resource "aws_iam_role_policy_attachment" "vpc_full_access" {
  role       = aws_iam_role.automation.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonVPCFullAccess"
}

resource "aws_iam_role_policy_attachment" "ec2_full_access" {
  role       = aws_iam_role.automation.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEC2FullAccess"
}

resource "aws_iam_role_policy" "automation_inline" {
  name = "automation-inline-policy"
  role = aws_iam_role.automation.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Sid      = "AllowPassSelf"
        Effect   = "Allow"
        Action   = "iam:PassRole"
        Resource = "${aws_iam_role.automation.arn}*"
      },
      {
        Sid    = "AllowSSMAutomation"
        Effect = "Allow"
        Action = [
          "ssm:StartAutomationExecution",
          "ssm:GetAutomationExecution"
        ]
        Resource = "*"
      }
    ]
  })
}

############################
# EC2 Instance Role
############################

resource "aws_iam_role" "ec2_ssm" {
  name = "SSMDemoEC2InstancesRole-${var.environment}"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Principal = {
          Service = "ec2.amazonaws.com"
        }
        Action = "sts:AssumeRole"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "ec2_ssm" {
  role       = aws_iam_role.ec2_ssm.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
}

resource "aws_iam_instance_profile" "ec2_ssm" {
  name = "ssm-demo-ec2-profile-${var.environment}"
  role = aws_iam_role.ec2_ssm.name
}
