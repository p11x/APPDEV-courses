# Terraform Validation

## What You'll Learn

- terraform validate command
- Syntax checking
- Module validation
- CI/CD validation

---

## Layer 1: Validation

### Validate Command

```bash
# Validate all files
terraform validate

# Validate with providers
terraform init
terraform validate

# JSON output for CI/CD
terraform validate -json
```