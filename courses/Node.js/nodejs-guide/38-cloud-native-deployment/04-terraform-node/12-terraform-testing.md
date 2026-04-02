# Terraform Testing

## What You'll Learn

- Unit testing Terraform modules
- Integration testing with Terratest
- Policy as code with Sentinel/OPA
- Test-driven development

---

## Layer 1: Testing

### Module Testing

```hcl
# tests/main.tftest.hcl
mock_provider "aws" {}

mock_data "aws_ami" "amazon_linux" {
  defaults {
    id = "ami-12345678"
  }
}

run "test" {
  command = plan
  
  assert {
    condition     = aws_instance.web.instance_type == "t3.micro"
    error_message = "Wrong instance type"
  }
}
```

---

## Next Steps

Continue to [Terraform Performance](./13-terraform-performance.md)