# Terraform Destroy

## What You'll Learn

- terraform destroy command
- Safe infrastructure removal
- Destroy options and targets
- Confirmation handling

---

## Layer 1: Destroy

### Destroy Command

```bash
# Interactive destroy
terraform destroy

# Non-interactive destroy
terraform destroy -auto-approve

# Target specific resource
terraform destroy -target=aws_instance.web

# Preserve resources
terraform destroy -target=null_resource.example
```