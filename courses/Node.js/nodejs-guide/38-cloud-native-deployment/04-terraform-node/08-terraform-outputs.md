# Terraform Outputs

## What You'll Learn

- How to define output values
- How to expose resource attributes
- How to use outputs for cross-module communication
- How to format and sensitive outputs

---

## Layer 1: Output Definitions

### Output Values

```hcl
# outputs.tf
output "vpc_id" {
  description = "ID of the VPC"
  value       = aws_vpc.main.id
}

output "instance_public_ip" {
  description = "Public IP of the web server"
  value       = aws_instance.web.public_ip
}

output "lambda_arn" {
  description = "ARN of the Lambda function"
  value       = module.lambda.function_arn
  sensitive   = false
}

output "api_endpoint" {
  description = "API Gateway endpoint URL"
  value       = "https://${aws_api_gateway_deployment.api.id}.execute-api.${var.region}.amazonaws.com/${var.stage}"
}

output "all_security_groups" {
  description = "List of all security group IDs"
  value       = [aws_security_group.this.id, aws_security_group.database.id]
}
```

---

## Next Steps

Continue to [Terraform Best Practices](./09-terraform-best-practices.md)