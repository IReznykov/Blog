terraform {
  required_version = ">= 1.5.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = "eu-central-1"
}

data "aws_availability_zones" "available" {}

locals {
  tags = {
    "ManagedBy"   = "Terraform",
    "Project"     = "Blog"
    "Environment" = var.environment
  }

  vpc_cidr = "172.18.0.0/18"

  subnet = {
    newbits        = 4
    public_offset  = 0
    private_offset = 10
  }

  azs = slice(data.aws_availability_zones.available.names, 0, 2)

  # NAT Gateways are NOT created in dev
  create_nat = var.environment != "dev"
}

############################
# VPC
############################

module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "~> 5.0"

  name = "ssm-demo-${var.environment}"
  cidr = local.vpc_cidr

  azs             = local.azs
  public_subnets  = [
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
  enable_nat_gateway = local.create_nat
  single_nat_gateway = false
  one_nat_gateway_per_az = true

  enable_ipv6 = false

  tags = local.tags
}

############################
# Outputs
############################

output "environment" {
  value = var.environment
}

output "vpc_id" {
  value = module.vpc.vpc_id
}

output "public_subnet_ids" {
  value = module.vpc.public_subnets
}

output "private_subnet_ids" {
  value = module.vpc.private_subnets
}

output "nat_gateway_ids" {
  value       = try(module.vpc.nat_gateway_ids, [])
  description = "NAT Gateway Ids, empty in dev"
}
