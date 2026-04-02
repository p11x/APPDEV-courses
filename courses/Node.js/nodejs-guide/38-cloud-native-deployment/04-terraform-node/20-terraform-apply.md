# Terraform Apply

## What You'll Learn

- terraform apply command
- Apply options and flags
- Auto-approve and targets
- Destroy and rollback

---

## Layer 1: Apply

### Apply Command

```bash
# Interactive apply
terraform apply

# Non-interactive with auto-approve
terraform apply -auto-approve

# Apply saved plan
terraform apply plan.tfplan

# Target specific resource
terraform apply -target=aws_instance.web

# Replace specific resource
terraform apply -replace=aws_instance.web

# Set variables
terraform apply -var="environment=prod" -var="region=us-west-2"
```