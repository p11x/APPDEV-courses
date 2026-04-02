# Terraform State

## What You'll Learn

- How Terraform manages state
- State storage backends
- State locking and consistency
- State file security

---

## Layer 1: State Management

### Local vs Remote State

```hcl
# Local state (default)
terraform {
  # State stored in terraform.tfstate file
}

# Remote state with S3
terraform {
  backend "s3" {
    bucket         = "my-terraform-state"
    key            = "prod/terraform.tfstate"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "terraform-locks"
  }
}
```

---

## Next Steps

Continue to [Terraform Backend](./05-terraform-backend.md)