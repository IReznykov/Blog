variable "environment" {
  description = "Deployment environment (dev, uat, prod)"
  type        = string
  default     = "dev"

  validation {
    condition     = contains(["dev", "uat", "prod"], var.environment)
    error_message = "environment must be one of: dev, uat, prod"
  }
}

variable "restore_runbook" {
  description = "SSM Runbook to add NAT Gateways and start EC2 instances"
  type        = string
  default     = "Restore-Environment"
}

variable "reduce_runbook" {
  description = "SSM Runbook to remove NAT Gateways and stop EC2 instances"
  type        = string
  default     = "Reduce-Environment"
}
