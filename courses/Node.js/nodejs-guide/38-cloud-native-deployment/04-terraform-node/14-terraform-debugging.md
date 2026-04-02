# Terraform Debugging

## What You'll Learn

- Debugging Terraform execution
- Using terraform console
- Reading logs and traces
- Common issues and fixes

---

## Layer 1: Debugging

### Debug Mode

```bash
export TF_LOG=TRACE
export TF_LOG_PATH=terraform.log
terraform apply
```

### Console

```bash
terraform console
> aws_vpc.main.id
> var.environment
```

---

## Next Steps

Continue to [Terraform Migration](./15-terraform-migration.md)