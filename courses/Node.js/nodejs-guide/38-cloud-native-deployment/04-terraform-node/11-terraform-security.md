# Terraform Security

## What You'll Learn

- Securing Terraform configurations
- Managing secrets safely
- Implementing RBAC for Terraform
- Audit and compliance

---

## Layer 1: Security

### Secret Management

```hcl
# Use environment variables for secrets
# Never commit secrets to version control

variable "db_password" {
  description = "Database password"
  type        = string
  sensitive   = true
}

# Use AWS Secrets Manager
data "aws_secretsmanager_secret" "db" {
  name = "prod/database"
}

data "aws_secretsmanager_secret_version" "db" {
  secret_id = data.aws_secretsmanager_secret.db.id
}
```

---

## Next Steps

Continue to [Terraform Testing](./12-terraform-testing.md)