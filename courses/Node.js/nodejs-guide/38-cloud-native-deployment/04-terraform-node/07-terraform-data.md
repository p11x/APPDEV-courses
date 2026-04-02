# Terraform Data Sources

## What You'll Learn

- How to use data sources to read existing infrastructure
- How to query AWS resources
- How to fetch remote state data
- How to implement data source filtering

---

## Layer 1: Data Sources

### AWS Data Sources

```hcl
# Fetch current AWS account info
data "aws_caller_identity" "current" {}

# Fetch VPC information
data "aws_vpc" "existing" {
  filter {
    name   = "tag:Name"
    values = ["main-vpc"]
  }
}

# Fetch AMI
data "aws_ami" "amazon_linux" {
  most_recent = true
  owners      = ["amazon"]
  
  filter {
    name   = "name"
    values = ["amzn2-ami-hvm-*-x86_64-gp2"]
  }
}

# Fetch available AZs
data "aws_availability_zones" "available" {
  state = "available"
}

# Use data in resources
resource "aws_subnet" "private" {
  vpc_id            = data.aws_vpc.existing.id
  availability_zone = data.aws_availability_zones.available.names[0]
  cidr_block        = "10.0.1.0/24"
}
```

---

## Next Steps

Continue to [Terraform Outputs](./08-terraform-outputs.md)