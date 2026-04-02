# Terraform Backend

## What You'll Learn

- How to configure Terraform backends
- S3 backend configuration
- State locking with DynamoDB
- Backend types and trade-offs

---

## Layer 1: Backend Configuration

### S3 Backend with State Locking

```hcl
terraform {
  backend "s3" {
    bucket         = "my-org-terraform-state"
    key            = "environments/prod/terraform.tfstate"
    region         = "us-east-1"
    encrypt        = true
    kms_key_id     = "alias/terraform-master-key"
    dynamodb_table = "terraform-state-locks"
    profile        = "terraform-user"
  }
}
```

---

## Next Steps

Continue to [Terraform Provisioners](./06-terraform-provisioners.md)