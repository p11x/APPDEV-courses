# Terraform Best Practices

## What You'll Learn

- Production-ready Terraform patterns
- Code organization strategies
- Security and compliance
- State management best practices

---

## Layer 1: Best Practices

### Code Organization

```hcl
# Separate files by purpose
# providers.tf - Provider configuration
# variables.tf - Input variables
# main.tf - Resources
# outputs.tf - Output values
# versions.tf - Version constraints
```

### Security Checklist

- [ ] Use remote backend with state locking
- [ ] Enable encryption at rest
- [ ] Use least privilege IAM roles
- [ ] Store secrets in Secrets Manager
- [ ] Enable audit logging
- [ ] Use workspaces for environments
- [ ] Implement state file versioning