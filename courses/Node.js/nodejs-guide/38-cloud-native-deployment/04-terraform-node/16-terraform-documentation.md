# Terraform Documentation

## What You'll Learn

- Documenting Terraform modules
- Generating documentation
- README best practices
- Version documentation

---

## Layer 1: Documentation

### Module Documentation

```markdown
# Lambda Module

## Requirements
- Terraform >= 1.0
- aws provider >= 5.0

## Usage
```hcl
module "lambda" {
  source = "./modules/lambda"
  
  function_name = "my-function"
  runtime       = "nodejs18.x"
}
```

## Inputs
| Name | Type | Description |
|------|------|-------------|
| function_name | string | Function name |
| runtime | string | Lambda runtime |
```