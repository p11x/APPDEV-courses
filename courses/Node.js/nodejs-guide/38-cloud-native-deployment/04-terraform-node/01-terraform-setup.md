# Terraform Setup for Node.js Projects

## What You'll Learn

- How to install and configure Terraform
- How to initialize a Terraform project
- How to create and structure Terraform configuration files
- How to manage provider dependencies

---

## Layer 1: Academic Foundation

### Infrastructure as Code with Terraform

Terraform is an open-source Infrastructure as Code (IaC) tool by HashiCorp that enables declarative provisioning of cloud and on-premises resources.

**Core Concepts:**
- **Provider**: Plugin for interacting with cloud APIs
- **Resource**: Infrastructure component to manage
- **Data Source**: Read-only data from infrastructure
- **Variable**: Parameterized configuration
- **Output**: Expose values from configuration

---

## Layer 2: Installation and Configuration

### Installation

```bash
# macOS
brew install terraform

# Linux
wget -O- https://apt.releases.hashicorp.com/gpg | gpg --dearmor | sudo tee /usr/share/keyrings/hashicorp-archive-keyring.gpg
echo "deb [signed-by=/usr/share/keyrings/hashicorp-archive-keyring.gpg] https://apt.releases.hashicorp.com $(lsb_release -cs) main" | sudo tee /etc/apt/sources.list.d/hashicorp.list
sudo apt update && sudo apt install terraform

# Windows
choco install terraform
```

### Project Structure

```
terraform/
├── main.tf
├── variables.tf
├── outputs.tf
├── providers.tf
└── modules/
    └── lambda/
        ├── main.tf
        ├── variables.tf
        └── outputs.tf
```

---

## Layer 3: Provider Configuration

### AWS Provider

```hcl
# providers.tf
terraform {
  required_version = ">= 1.0"
  
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
  
  backend "s3" {
    bucket = "my-terraform-state"
    key    = "prod/terraform.tfstate"
    region = "us-east-1"
  }
}

provider "aws" {
  region = "us-east-1"
  
  default_tags {
    tags = {
      Environment = "production"
      Project     = "nodejs-api"
    }
  }
}
```

---

## Next Steps

Continue to [Terraform Variables](./02-terraform-variables.md)