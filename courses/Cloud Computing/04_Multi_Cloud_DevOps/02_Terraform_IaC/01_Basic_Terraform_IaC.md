---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud DevOps
Concept: Terraform IaC
Difficulty: beginner
Prerequisites: Basic Cloud Computing, DevOps Basics, Infrastructure as Code
RelatedFiles: 02_Advanced_Terraform_IaC.md, 03_Practical_Terraform_IaC.md
UseCase: Understanding Terraform for multi-cloud infrastructure management
CertificationExam: AWS Solutions Architect / Professional
LastUpdated: 2025
---

## WHY

Terraform is an Infrastructure as Code (IaC) tool that enables consistent, repeatable infrastructure deployment across multiple cloud providers.

### Why Terraform Matters

- **Multi-Cloud Support**: Single tool for AWS, Azure, GCP
- **Declarative**: Define desired state
- **Plan and Apply**: Preview changes before execution
- **State Management**: Track infrastructure changes
- **Reusability**: Modules for common patterns

### Terraform Benefits

| Benefit | Description | Impact |
|---------|-------------|--------|
| Consistency | Same config everywhere | Fewer errors |
| Version Control | Track changes | Auditability |
| Automation | No manual steps | Speed |
| Reusability | Modules | Efficiency |

## WHAT

### Terraform Concepts

**Providers**
- AWS, Azure, GCP providers
- Custom providers
- Plugin architecture

**Resources**
- Cloud resources
- Data sources
- Modules

**State**
- Local state
- Remote state
- State locking

### Terraform Architecture

```
TERRAFORM ARCHITECTURE
======================

┌─────────────────────────────────────────────────────────────┐
│                    CONFIGURATION                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │   *.tf       │  │  Variables   │  │  Outputs     │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────┼───────────────────────────────────┐
│                    TERRAFORM CORE                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │   Init       │  │   Plan        │  │   Apply      │       │
│  │             │  │   Preview     │  │   Execute    │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────┼───────────────────────────────────┐
│                    PROVIDERS                                 │
│  ┌────────┐    ┌────────┐    ┌────────┐                    │
│  │  AWS   │    │ Azure  │    │  GCP   │                    │
│  └────────┘    └────────┘    └────────┘                    │
└─────────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────┼───────────────────────────────────┐
│                    STATE                                     │
│  ┌──────────────┐  ┌──────────────┐                        │
│  │ Local State │  │ Remote State │                        │
│  │  (file)     │  │   (S3/Blob)  │                        │
│  └──────────────┘  └──────────────┘                        │
└─────────────────────────────────────────────────────────────┘
```

## HOW

### Example 1: AWS Terraform Configuration

```hcl
# Terraform configuration
terraform {
  required_version = ">= 1.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = "us-east-1"
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
  default     = "t3.micro"
}

variable "tags" {
  description = "Tags to apply"
  type        = map(string)
  default     = {
    Environment = "production"
    Project    = "multi-cloud"
  }
}

resource "aws_vpc" "main" {
  cidr_block           = "10.0.0.0/16"
  enable_dns_hostnames = true
  enable_dns_support   = true
  tags = var.tags
}

resource "aws_subnet" "main" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "10.0.1.0/24"
  availability_zone       = "us-east-1a"
  map_public_ip_on_launch = true
  tags = var.tags
}

resource "aws_security_group" "web" {
  name        = "web-sg"
  description = "Security group for web"
  vpc_id      = aws_vpc.main.id
  
  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = var.tags
}

resource "aws_instance" "web" {
  ami               = "ami-0c55b159cbfafe1f0"
  instance_type     = var.instance_type
  subnet_id         = aws_subnet.main.id
  security_groups   = [aws_security_group.web.id]
  tags = var.tags
}

output "instance_ip" {
  value = aws_instance.web.public_ip
}
```

### Example 2: Azure Provider Configuration

```hcl
# Azure provider
provider "azurerm" {
  features {}
  subscription_id = var.azure_subscription_id
}

resource "azurerm_resource_group" "main" {
  name     = "main-rg"
  location = "eastus"
}

resource "azurerm_virtual_network" "main" {
  name                = "main-vnet"
  resource_group_name = azurerm_resource_group.main.name
  location            = "eastus"
  address_space       = ["10.0.0.0/16"]
}

resource "azurerm_subnet" "main" {
  name                 = "main-subnet"
  resource_group_name  = azurerm_resource_group.main.name
  virtual_network_name = azurerm_virtual_network.main.name
  address_prefixes     = ["10.0.1.0/24"]
}

resource "azurerm_network_security_group" "main" {
  name                = "main-nsg"
  location            = "eastus"
  resource_group_name = azurerm_resource_group.main.name
}
```

### Example 3: GCP Provider Configuration

```hcl
# GCP provider
provider "google" {
  project = var.gcp_project
  region  = "us-central1"
}

resource "google_compute_network" "main" {
  name                    = "main-network"
  auto_create_subnetworks = false
}

resource "google_compute_subnetwork" "main" {
  name          = "main-subnet"
  region        = "us-central1"
  network       = google_compute_network.main.name
  ip_cidr_range = "10.0.0.0/24"
}

resource "google_compute_firewall" "allow-https" {
  name    = "allow-https"
  network = google_compute_network.main.name
  
  allow {
    protocol = "tcp"
    ports    = ["443"]
  }
  
  source_ranges = ["0.0.0.0/0"]
  target_tags   = ["web"]
}

resource "google_compute_instance_template" "main" {
  name        = "main-template"
  machine_type = "e2-medium"
  
  disk {
    boot_disk {
      initialize_params {
        image = "ubuntu-1804-bionic-v20230302"
      }
    }
  }
  
  network_interface {
    network = google_compute_network.main.name
  }
  
  tags = ["http-server", "https-server"]
}
```

## COMMON ISSUES

### 1. State Management

- State corruption
- Solution: Use remote state with locking

### 2. Provider Versioning

- Breaking changes
- Solution: Pin provider versions

### 3. Resource Dependencies

- Implicit dependencies
- Solution: Use explicit dependencies

## CROSS-REFERENCES

### Prerequisites

- Cloud fundamentals
- Command line basics
- JSON/HCL syntax

### What to Study Next

1. Advanced Terraform
2. Kubernetes Multi-Cloud
3. GitOps

## EXAM TIPS

- Know Terraform workflow
- Understand state management
- Be able to write basic Terraform