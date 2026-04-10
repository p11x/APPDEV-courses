---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Core Concepts
Purpose: Understanding Azure's core cloud platform concepts, regions, and resource model
Difficulty: beginner
Prerequisites: Basic Cloud Computing
RelatedFiles: 02_Advanced_Azure_Core.md, 03_Practical_Azure_Core.md
UseCase: Deploying and managing resources in Azure cloud
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## WHY

Microsoft Azure is the #2 cloud provider with extensive enterprise adoption. Understanding Azure concepts is essential for enterprise cloud professionals and for Microsoft's certification pathway.

### Why Azure Matters

- **Enterprise Adoption**: 95% of Fortune 500 use Azure
- **Hybrid Cloud**: Strongest hybrid cloud capabilities
- **Compliance**: Extensive compliance certifications
- **Integration**: Best Microsoft product integration

### Azure Quick Facts

- 60+ regions globally
- 200+ services
- Windows, Linux, Containers, Serverless

## WHAT

### Azure Core Concepts

**Resource**: Deployable cloud service (VM, database, storage).

**Resource Group**: Container for related resources.

**Subscription**: Billing boundary and access control.

**Azure Region**: Geographic deployment area.

**Availability Zone**: Isolated data centers within region.

### Azure Regions vs AWS Regions

| Azure | AWS | Geographic |
|-------|-----|-----------|
| East US | us-east-1 | Virginia |
| West US | us-west-2 | Oregon |
| UK South | eu-west-1 | London |
| Southeast Asia | ap-southeast-1 | Singapore |

### Azure Resource Model

```
    AZURE RESOURCE HIERARCHY
    ======================

  ┌─────────────────────────┐
  │    Tenant (Org)       │
  └──────────┬────────────┘
             │
    ┌────────┴────────┐
    │  Management     │
    │  Groups        │
    └───────┬────────┘
           │
    ┌──────┴──────┐
    │ Subscription│
    └─────┬──────┘
          │
    ┌────┴────┐
    │Resource │
    │ Group   │
    └────┬────┘
         │
  ┌─────┴─────┐
  │ Resources │
  │ VM, S3, DB│
  └──────────┘
```

## HOW

### Example 1: Create Resources in Azure

```bash
# Login to Azure
az login

# Set default subscription
az account set --subscription "My Subscription"

# Create resource group
az group create \
    --name my-resource-group \
    --location eastus

# Create virtual machine
az vm create \
    --resource-group my-resource-group \
    --name my-vm \
    --image UbuntuLTS \
    --size Standard_DS1_v2 \
    --admin-username azureuser \
    --generate-ssh-keys

# Create storage account
az storage account create \
    --resource-group my-resource-group \
    --name mystorageaccount \
    --sku Standard_LRS
```

### Example 2: Azure Resource Management

```bash
# List all resources
az resource list

# List resources by group
az resource list \
    --resource-group my-resource-group

# Show specific resource
az vm show \
    --resource-group my-resource-group \
    --name my-vm

# Stop VM
az vm stop \
    --resource-group my-resource-group \
    --name my-vm

# Delete resource
az group delete \
    --name my-resource-group
```

### Example 3: Azure with Terraform

```hcl
# main.tf for Azure
provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "main" {
  name     = "my-resource-group"
  location = "eastus"
}

resource "azurerm_virtual_network" "main" {
  name                = "my-vnet"
  resource_group_name = azurerm_resource_group.main.name
  address_space      = ["10.0.0.0/16"]
  location           = azurerm_resource_group.main.location
}

resource "azurerm_subnet" "main" {
  name                 = "internal"
  resource_group_name  = azurerm_resource_group.main.name
  virtual_network_name = azurerm_virtual_network.main.name
  address_prefixes    = ["10.0.1.0/24"]
}
```

## AZURE SERVICE EQUIVALENTS

| Service | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Compute | EC2 | Virtual Machines | Compute Engine |
| Storage | S3 | Blob Storage | Cloud Storage |
| Database | RDS | SQL Database | Cloud SQL |
| Serverless | Lambda | Functions | Cloud Functions |
| CDN | CloudFront | Azure CDN | Cloud CDN |
| DNS | Route 53 | Azure DNS | Cloud DNS |

## CROSS-REFERENCES

### Prerequisites

- Basic cloud concepts
- Azure account setup

### What to Study Next

1. Azure Core Services
2. Azure Security
3. Azure Management Tools