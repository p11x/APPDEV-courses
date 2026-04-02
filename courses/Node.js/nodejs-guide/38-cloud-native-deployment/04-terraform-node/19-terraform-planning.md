# Terraform Planning

## What You'll Learn

- terraform plan command
- Execution plans
- Plan output options
- Plan in CI/CD

---

## Layer 1: Planning

### Plan Command

```bash
# Standard plan
terraform plan

# Save plan to file
terraform plan -out=plan.tfplan

# Plan with variables
terraform plan -var="environment=prod"

# Detailed plan
terraform plan -detailed-exitcode

# Use saved plan
terraform apply plan.tfplan
```