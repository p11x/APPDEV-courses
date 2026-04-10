---
Category: DevOps
Subcategory: Infrastructure as Code
Concept: Terraform Basics
Purpose: Understanding Terraform for infrastructure-as-code
Difficulty: beginner
Prerequisites: Cloud Fundamentals
RelatedFiles: 02_Advanced_Terraform.md
UseCase: Cloud infrastructure provisioning
CertificationExam: Terraform Associate
LastUpdated: 2025
---

## WHY

Terraform enables infrastructure-as-code for reproducible, version-controlled cloud deployments.

## WHAT

### Terraform Concepts

**Provider**: Cloud platform plugin

**Resource**: Infrastructure component

**Data Source**: Read-only data

**Variable**: Parameterizable values

**Module**: Reusable configuration

## HOW

### Example: AWS Infrastructure

```hcl
provider "aws" {
  region = "us-east-1"
}

resource "aws_vpc" "main" {
  cidr_block = "10.0.0.0/16"
  
  tags = {
    Name = "main-vpc"
  }
}

resource "aws_subnet" "public" {
  vpc_id     = aws_vpc.main.id
  cidr_block = "10.0.1.0/24"
  
  tags = {
    Name = "public-subnet"
  }
}

resource "aws_instance" "web" {
  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = "t3.micro"
  subnet_id     = aws_subnet.public.id
  
  tags = {
    Name = "web-server"
  }
}
```

### Commands

```bash
# Initialize
terraform init

# Plan changes
terraform plan

# Apply changes
terraform apply

# Destroy
terraform destroy
```

## CROSS-REFERENCES

### Related Tools

- CloudFormation: AWS native IaC
- Pulumi: Programmatic IaC
- Ansible: Configuration management