---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud DevOps
Concept: Terraform IaC
Difficulty: advanced
Prerequisites: Basic Cloud Computing, Basic Terraform Concepts
RelatedFiles: 01_Basic_Terraform_IaC.md, 03_Practical_Terraform_IaC.md
UseCase: Advanced Terraform for enterprise multi-cloud infrastructure
CertificationExam: AWS Solutions Architect Professional, Azure Architect Expert
LastUpdated: 2025
---

## WHY

Advanced Terraform implementation requires sophisticated patterns including modules, workspaces, remote state, and orchestration for complex multi-cloud environments.

### Strategic Requirements

- **Modular Design**: Reusable infrastructure components
- **State Management**: Remote state with proper locking
- **Orchestration**: Cross-cloud dependencies
- **Security**: Secrets management, RBAC
- **Governance**: Policy as code

### Advanced Patterns

| Pattern | Complexity | Features | Use Case |
|---------|------------|----------|----------|
| Modules | Low | Reusability | Standard patterns |
| Workspaces | Medium | Multiple environments | Dev/Stage/Prod |
| Remote State | Medium | State sharing | Team collaboration |
| Provisioners | High | Custom config | Complex setups |

## WHAT

### Advanced Terraform Features

**State Management**
- Remote state backends (S3, Azure Blob, GCS)
- State locking
- State import
- State drift detection

**Workspaces**
- Environment isolation
- Variable workspaces
- Workspace-specific configurations

**Modules**
- Module composition
- Module versioning
- Private registries

### Cross-Platform Comparison

| Feature | AWS | Azure | GCP | On-Prem |
|---------|-----|-------|-----|---------|
| State Storage | S3 | Blob Storage | GCS | Local/Consul |
| Key Management | KMS | Key Vault | Cloud KMS | Vault |
| Identity | IAM | Entra ID | Cloud IAM | AD/LDAP |
| Networking | VPC | VNet | VPC Network | Physical |

## HOW

### Example 1: Multi-Cloud Module Design

```hcl
# Multi-cloud compute module
variable "provider" {
  description = "Cloud provider: aws, azure, or gcp"
  type        = string
  default     = "aws"
}

variable "name" {
  description = "Instance name"
  type        = string
}

variable "instance_count" {
  type    = number
  default = 1
}

variable "instance_type" {
  type    = string
  default = "t3.micro"
}

# AWS EC2
resource "aws_instance" "this" {
  count = var.provider == "aws" ? var.instance_count : 0
  ami   = var.aws_ami
  instance_type = var.instance_type
  subnet_id = var.subnet_id
  tags = { Name = "${var.name}-aws" }
}

# Azure VM
resource "azurerm_virtual_machine" "this" {
  count = var.provider == "azure" ? var.instance_count : 0
  name = "${var.name}-azure"
  location = var.location
  vm_size = var.instance_type
}

# GCP Compute
resource "google_compute_instance" "this" {
  count = var.provider == "gcp" ? var.instance_count : 0
  name = "${var.name}-gcp"
  machine_type = var.instance_type
  zone = var.zone
}

output "instance_ids" {
  value = {
    aws   = aws_instance.this[*].id
    azure = azurerm_virtual_machine.this[*].id
    gcp   = google_compute_instance.this[*].id
  }
}
```

### Example 2: Remote State Configuration

```hcl
# Backend configuration
terraform {
  backend "s3" {
    bucket         = "terraform-state-multi"
    key            = "prod/terraform.tfstate"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "terraform-state-lock"
  }
}

# Provider configuration
provider "aws" {
  region = "us-east-1"
  assume_role {
    role_arn = "arn:aws:iam::${var.account_id}:role/TerraformRole"
  }
}

provider "azurerm" {
  features {}
  subscription_id = var.azure_subscription_id
  tenant_id       = var.azure_tenant_id
}

provider "google" {
  project = var.gcp_project
  region  = "us-central1"
}

# Remote state reference
data "terraform_remote_state" "network" {
  backend = "s3"
  config = {
    bucket = "terraform-state-multi"
    key    = "network/terraform.tfstate"
    region = "us-east-1"
  }
}
```

### Example 3: Policy as Code with Sentinel/OPA

```hcl
# Sentinel policy example
# policy.sentinel

import "tfplan/v2" as tfplan

# Prevent public S3 buckets
main = rule {
  all tfplan.resource_changes as _, resource {
    resource.type is "aws_s3_bucket" implies
    resource.change.after.acl is not "public-read"
  }
}

# Require tags on resources
tagging = rule {
  all tfplan.resource_changes as _, resource {
    resource.change.after.tags is not null
  }
}

# OPA Rego policy
# policy.rego
package main

deny[msg] {
  input.resource.type == "aws_instance"
  input.resource.change.after.instance_type == "t2.micro"
  msg = "t2.micro instances not allowed"
}

deny[msg] {
  input.resource.type == "aws_s3_bucket"
  not input.resource.change.after.server_side_encryption_configuration
  msg = "S3 buckets must have encryption"
}
```

## COMMON ISSUES

### 1. State Conflicts

- Multiple people editing state
- Solution: Use state locking

### 2. Large States

- Slow operations
- Solution: Use state splitting

### 3. Drift Detection

- Manual vs. Terraform state
- Solution: Regular terraform refresh

## PERFORMANCE

### Performance Optimization

| Optimization | Technique | Impact |
|--------------|-----------|--------|
| Parallelism | -parallelism flag | 50% faster |
| State Splitting | Split by component | 70% faster |
| Caching | Provider cache | 30% faster |

## COMPATIBILITY

### Backend Support

| Backend | State Storage | Locking | Use Case |
|---------|---------------|---------|----------|
| S3 | Yes | DynamoDB | AWS |
| Azure Blob | Yes | Azure Blob | Azure |
| GCS | Yes | GCS | GCP |
| Consul | Yes | Consul | On-prem |

## CROSS-REFERENCES

### Prerequisites

- Basic Terraform concepts
- Cloud provider services
- HCL syntax

### Related Topics

1. Basic Terraform
2. GitOps
3. Kubernetes Multi-Cloud

## EXAM TIPS

- Know module design patterns
- Understand state management
- Be able to design enterprise Terraform