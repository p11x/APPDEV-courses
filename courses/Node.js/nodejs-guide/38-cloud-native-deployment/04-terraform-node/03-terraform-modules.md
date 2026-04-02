# Terraform Modules

## What You'll Learn

- How to create reusable Terraform modules
- How to structure module files
- How to publish and version modules
- How to compose modules for complex infrastructure

---

## Layer 1: Module Structure

### Module Files

```hcl
# modules/lambda/main.tf
variable "function_name" {
  description = "Name of the Lambda function"
  type        = string
}

variable "runtime" {
  description = "Lambda runtime"
  type        = string
  default     = "nodejs18.x"
}

variable "handler" {
  description = "Function handler"
  type        = string
}

variable "memory_size" {
  description = "Memory allocation in MB"
  type        = number
  default     = 256
}

variable "timeout" {
  description = "Function timeout in seconds"
  type        = number
  default     = 30
}

variable "environment_variables" {
  description = "Environment variables"
  type        = map(string)
  default     = {}
}

resource "aws_lambda_function" "this" {
  function_name    = var.function_name
  runtime          = var.runtime
  handler          = var.handler
  filename         = "${path.module}/../../lambda.zip"
  source_code_hash = filebase64sha256("${path.module}/../../lambda.zip")
  memory_size      = var.memory_size
  timeout          = var.timeout
  
  environment {
    variables = var.environment_variables
  }
  
  lifecycle {
    create_before_destroy = true
  }
}

output "function_arn" {
  value = aws_lambda_function.this.arn
}
```

---

## Layer 2: Module Usage

### Calling Module

```hcl
# main.tf
module "api_lambda" {
  source = "./modules/lambda"
  
  function_name        = "nodejs-api"
  runtime              = "nodejs18.x"
  handler              = "index.handler"
  memory_size          = 512
  timeout              = 30
  environment_variables = {
    NODE_ENV  = "production"
    LOG_LEVEL = "info"
  }
}
```

---

## Next Steps

Continue to [Terraform State](./04-terraform-state.md)