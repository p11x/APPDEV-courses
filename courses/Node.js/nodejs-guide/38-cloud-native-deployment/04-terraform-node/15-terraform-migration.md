# Terraform Migration

## What You'll Learn

- Migrating from other IaC tools
- Importing existing resources
- State migration strategies
- Best practices for migrations

---

## Layer 1: Migration

### Import

```bash
# Import existing resources
terraform import aws_vpc.main vpc-12345678
terraform import aws_instance.web i-12345678

# Import with ID mapping
terraform import 'aws_security_group.rule[0]' sg-12345678
```