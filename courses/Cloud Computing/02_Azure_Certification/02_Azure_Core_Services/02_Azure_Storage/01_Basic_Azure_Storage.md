---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Storage
Purpose: Understanding Azure Blob, File, Queue, and Disk storage services
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Storage.md, 03_Practical_Azure_Storage.md
UseCase: Storing data in Azure
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure Storage provides durable, scalable cloud storage for any type of data. Understanding storage options is essential for Azure deployments.

## 📖 WHAT

### Azure Storage Services

| Service | Type | Use Case |
|---------|------|----------|
| Blob Storage | Object | Unstructured data |
| Files | File share | SMB file shares |
| Queue | Messages | Message queuing |
| Table | NoSQL | NoSQL key-value |
| Disk | Block | VM disks |

### Storage Tiers

- **Hot**: Frequent access, higher cost
- **Cool**: Less frequent, lower cost
- **Archive**: Rarely accessed, lowest cost

## 🔧 HOW

### Example 1: Storage Account

```bash
# Create storage account
az storage account create \
    --name mystorageacct \
    --resource-group myrg \
    --location eastus \
    --sku Standard_LRS

# Get connection string
az storage account show-connection-string \
    --name mystorageacct \
    --resource-group myrg
```

### Example 2: Blob Storage

```bash
# Create container
az storage container create \
    --name mycontainer \
    --account-name mystorageacct

# Upload blob
az storage blob upload \
    --container-name mycontainer \
    --name myfile.txt \
    --file myfile.txt \
    --account-name mystorageacct

# List blobs
az storage blob list \
    --container-name mycontainer \
    --account-name mystorageacct
```

## ✅ EXAM TIPS

- Blob for unstructured data
- Files for SMB shares
- Queue for messaging
- Disk for VM storage