# Terraform Performance

## What You'll Learn

- Optimizing Terraform execution
- Reducing plan and apply time
- Parallelism and caching
- Large infrastructure patterns

---

## Layer 1: Optimization

### Parallelism

```bash
terraform apply -parallelism=10
terraform plan -parallelism=20

# Configure in terraform block
terraform {
  parallelisme = 10
}
```

---

## Next Steps

Continue to [Terraform Debugging](./14-terraform-debugging.md)