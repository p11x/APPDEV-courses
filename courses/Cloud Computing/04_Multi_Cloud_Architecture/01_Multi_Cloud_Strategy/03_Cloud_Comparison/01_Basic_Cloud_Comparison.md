---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud Strategy
Concept: Cloud Comparison
Difficulty: beginner
Prerequisites: Basic Cloud Computing, AWS Fundamentals, Azure Fundamentals, GCP Fundamentals
RelatedFiles: 02_Advanced_Cloud_Comparison.md, 03_Practical_Cloud_Comparison.md
UseCase: Comparing cloud providers for multi-cloud strategy
CertificationExam: AWS Solutions Architect / Professional
LastUpdated: 2025
---

## WHY

Understanding cloud provider differences is essential for multi-cloud architecture. Each provider has unique strengths, services, and pricing models.

### Why Cloud Comparison Matters

- **Service Selection**: Choose best service for each workload
- **Cost Optimization**: Leverage pricing differences
- **Architecture Decisions**: Select appropriate provider for use cases
- **Risk Management**: Understand provider capabilities and limitations

### Market Position

| Provider | Market Share | Strengths |
|----------|--------------|-----------|
| AWS | 32% | Most services, mature ecosystem |
| Azure | 23% | Enterprise integration, hybrid |
| GCP | 11% | Data/AI, networking |
| Others | 34% | Niche services, regions |

## WHAT

### Core Service Comparison

**Compute Services**
- AWS: EC2, Lambda, ECS, EKS
- Azure: Virtual Machines, Functions, Container Instances, AKS
- GCP: Compute Engine, Cloud Functions, Cloud Run, GKE

**Storage Services**
- AWS: S3, EBS, EFS, Glacier
- Azure: Blob Storage, Managed Disks, Files, Archive
- GCP: Cloud Storage, Persistent Disk, Filestore

**Database Services**
- AWS: RDS, DynamoDB, ElastiCache, Redshift
- Azure: Azure SQL, Cosmos DB, Redis Cache, Synapse
- GCP: Cloud SQL, Firestore, Memorystore, BigQuery

### Service Mapping

```
CLOUD SERVICE COMPARISON
========================

┌────────────────┬─────────────┬─────────────┬─────────────┐
│    Category    │     AWS     │    Azure   │     GCP     │
├────────────────┼─────────────┼─────────────┼─────────────┤
│   Compute     │    EC2      │  VM         │ Compute Eng │
│   Serverless  │   Lambda    │  Functions  │Cloud Func   │
│   Container   │   EKS/ECS   │    AKS      │    GKE      │
│   Storage     │     S3      │   Blob      │Cloud Storage│
│   Database    │    RDS      │ Azure SQL   │  Cloud SQL  │
│   NoSQL       │ DynamoDB   │ Cosmos DB   │  Firestore  │
│   Analytics   │   Redshift  │  Synapse    │   BigQuery  │
│   AI/ML       │ SageMaker  │ Azure ML    │  Vertex AI  │
└────────────────┴─────────────┴─────────────┴─────────────┘
```

## HOW

### Example 1: AWS Service Configuration

```hcl
# AWS infrastructure
provider "aws" {
  region = "us-east-1"
}

# EC2 instance
resource "aws_instance" "web" {
  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = "t3.medium"
  
  tags = {
    Name = "web-server"
  }
}

# S3 bucket
resource "aws_s3_bucket" "data" {
  bucket = "my-data-bucket"
  
  versioning {
    enabled = true
  }
}

# RDS instance
resource "aws_db_instance" "main" {
  identifier     = "main-db"
  engine         = "postgres"
  engine_version = "14.7"
  instance_class = "db.t3.medium"
  
  backup_retention_period = 7
}
```

### Example 2: Azure Service Configuration

```hcl
# Azure infrastructure
provider "azurerm" {
  features {}
}

# Virtual machine
resource "azurerm_virtual_machine" "web" {
  name                  = "web-vm"
  location              = "eastus"
  resource_group_name   = "my-rg"
  vm_size               = "Standard_D2s_v3"
  
  storage_image_reference {
    publisher = "Canonical"
    offer     = "UbuntuServer"
    sku       = "18.04-LTS"
    version   = "latest"
  }
}

# Blob storage
resource "azurerm_storage_account" "data" {
  name                     = "mydatastore"
  resource_group_name      = "my-rg"
  location                 = "eastus"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

# Azure SQL
resource "azurerm_sql_database" "main" {
  name                = "main-db"
  resource_group_name = "my-rg"
  server_name         = "myserver"
  edition             = "GeneralPurpose"
  collation           = "SQL_Latin1_General_CP1_CI_AS"
}
```

### Example 3: GCP Service Configuration

```hcl
# GCP infrastructure
provider "google" {
  project = "my-project"
  region  = "us-central1"
}

# Compute Engine instance
resource "google_compute_instance" "web" {
  name         = "web-instance"
  machine_type = "e2-medium"
  zone         = "us-central1-a"
  
  boot_disk {
    initialize_params {
      image = "ubuntu-1804-bionic-v20230302"
      size  = 20
    }
  }
  
  network_interface {
    network = "default"
  }
}

# Cloud Storage bucket
resource "google_storage_bucket" "data" {
  name          = "my-data-bucket"
  location      = "US"
  storage_class = "STANDARD"
  
  versioning {
    enabled = true
  }
}

# Cloud SQL instance
resource "google_sql_database_instance" "main" {
  name             = "main-db"
  database_version = "POSTGRES_14"
  region           = "us-central1"
  
  settings {
    tier = "db-f1-micro"
  }
}
```

## COMMON ISSUES

### 1. Service Feature Gaps

- Not all services equivalent across providers
- Solution: Map requirements to available services

### 2. Naming Conventions

- Different naming schemes
- Solution: Use abstraction layers

### 3. API Differences

- Varying authentication and response formats
- Solution: Use SDKs and abstraction libraries

## CROSS-REFERENCES

### Prerequisites

- AWS Fundamentals
- Azure Fundamentals
- GCP Fundamentals
- Cloud Computing Basics

### What to Study Next

1. Vendor Lock-In Strategies
2. Multi-Cloud Networking
3. Multi-Cloud Security

## EXAM TIPS

- Know service equivalents across providers
- Understand provider strengths
- Be able to recommend provider based on requirements