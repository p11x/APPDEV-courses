# AWS CDK vs Terraform

## What You'll Learn

- Comparison between AWS CDK and Terraform
- Decision criteria for choosing the right tool
- Migration strategies between tools
- Hybrid approaches

---

## Layer 1: Tool Comparison

### Feature Comparison

| Feature | AWS CDK | Terraform |
|---------|---------|-----------|
| Language | TypeScript, Python, Java, Go | HCL, JSON |
| State | CloudFormation | Local/Remote |
| Planning | `cdk diff` | `terraform plan` |
| Testing | Jest/Pytest | Terratest |
| Multi-cloud | AWS-first | Multi-cloud |
| Learning curve | Medium | Low |
| Community | Growing | Mature |

### When to Use Each

**Use CDK when:**
- AWS-only infrastructure
- Strong programming language skills
- Need for abstraction and reuse
- Integration with CI/CD pipelines

**Use Terraform when:**
- Multi-cloud infrastructure
- Existing Terraform codebase
- Need for state management
- Large community support

---

## Next Steps

Continue to [Azure Functions Setup](../03-azure-functions/01-azure-functions-setup.md)