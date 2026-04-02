# Terraform Variables

## What You'll Learn

- How to define and use input variables
- How to implement variable types and validation
- How to manage environment-specific configurations
- How to use variable precedence

---

## Layer 1: Variable Definitions

### Input Variables

```hcl
# variables.tf
variable "environment" {
  description = "Environment name"
  type        = string
  default     = "dev"
  validation {
    condition     = contains(["dev", "staging", "prod"], var.environment)
    error_message = "Environment must be dev, staging, or prod"
  }
}

variable "region" {
  description = "AWS region"
  type        = string
  default     = "us-east-1"
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
  default     = "t3.micro"
}

variable "vpc_cidr" {
  description = "VPC CIDR block"
  type        = string
  default     = "10.0.0.0/16"
}

variable "tags" {
  description = "Resource tags"
  type        = map(string)
  default     = {}
}

variable "lambda_settings" {
  description = "Lambda function settings"
  type = object({
    runtime       = string
    memory_size   = number
    timeout       = number
    reserved_concurrency = number
  })
  default = {
    runtime       = "nodejs18.x"
    memory_size   = 256
    timeout       = 30
    reserved_concurrency = null
  }
}
```

---

## Layer 2: Variable Usage

### In Resources

```hcl
# main.tf
resource "aws_vpc" "main" {
  cidr_block           = var.vpc_cidr
  enable_dns_hostnames = true
  enable_dns_support   = true
  
  tags = merge(var.tags, {
    Name = "vpc-${var.environment}"
  })
}

resource "aws_instance" "web" {
  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = var.instance_type
  
  tags = merge(var.tags, {
    Name = "web-${var.environment}"
  })
}
```

---

## Next Steps

Continue to [Terraform Modules](./03-terraform-modules.md)