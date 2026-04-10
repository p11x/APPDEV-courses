---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Architecture
Purpose: Understanding Azure architecture patterns, availability zones, and regions
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Architecture.md, 03_Practical_Azure_Architecture.md
UseCase: Designing highly available Azure architectures
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure architecture patterns enable building resilient, scalable applications. Understanding Azure's architectural components is essential for cloud deployments.

## 📖 WHAT

### Azure Architecture Components

**Availability Zones**: Physically isolated data centers within a region

**Availability Sets**: Logical grouping for redundancy

**Azure Regions**: Geographic deployment areas

**Region Pairs**: Secondary region for disaster recovery

### Architecture Diagram

```
Azure Architecture
==================

┌─────────────────────────────────────┐
│          Azure Region                 │
│  ┌─────────┐ ┌─────────┐            │
│  │  Zone A │ │ Zone B │            │
│  │ VM-1    │ │ VM-2    │            │
│  └─────────┘ └─────────┘            │
│          │       │                   │
│          ▼       ▼                   │
│     ┌─────────┴─────────┐          │
│     │   Load Balancer    │          │
│     └───────────────────┘          │
└─────────────────────────────────────┘
```

## 🔧 HOW

### Example 1: Create VM with Availability Zone

```bash
# Create VM in specific availability zone
az vm create \
    --name myvm \
    --resource-group myrg \
    --location eastus \
    --image UbuntuLTS \
    --size Standard_DS1_v2 \
    --zone 1 \
    --admin-username azureuser

# Create VM with availability set
az vm availability-set create \
    --name my-avset \
    --resource-group myrg

az vm create \
    --name myvm \
    --resource-group myrg \
    --availability-set my-avset \
    --image UbuntuLTS
```

### Example 2: Region Pair Configuration

```bash
# Deploy to paired region
# East US paired with East US 2
# West US paired with East US

# Create resource in secondary region
az vm create \
    --name dr-vm \
    --resource-group dr-rg \
    --location eastus2 \
    --image UbuntuLTS
```

## 🔗 CROSS-REFERENCES

**Related**: Azure Compute, Azure Networking

**Next**: Azure Management Tools