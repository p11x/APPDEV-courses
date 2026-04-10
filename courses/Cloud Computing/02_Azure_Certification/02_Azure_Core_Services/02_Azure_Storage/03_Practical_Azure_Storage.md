---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Storage
Purpose: Practical labs for Azure storage management, data protection, and optimization
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Storage.md, 02_Advanced_Azure_Storage.md
RelatedFiles: 01_Basic_Azure_Storage.md, 02_Advanced_Azure_Storage.md
UseCase: Enterprise storage deployment and management
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Hands-on labs demonstrate real-world storage management including backup, disaster recovery, and cost optimization.

## 📖 WHAT

### Lab Overview

Deploy enterprise storage with lifecycle management, versioning, and disaster recovery capabilities.

### Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Storage Account                      │
│  ┌──────────┐  ┌──────────┐  ┌──────────────────────┐ │
│  │ Hot Tier │→│ Cool Tier│→│ Archive Tier          │ │
│  └──────────┘  └──────────┘  └──────────────────────┘ │
│  ┌──────────────────────────────────────────┐        │
│  │ Versioning + Soft Delete                  │        │
│  └──────────────────────────────────────────┘        │
└─────────────────────────────────────────────────────────┘
```

## 🔧 HOW

### Module 1: Storage Account with Data Protection

```bash
# Create storage account with data protection
az storage account create \
    --name enterprisestorage \
    --resource-group storage-rg \
    --location eastus \
    --sku Standard_GZRS \
    --enable-hierarchical-namespace true \
    --allow-blob-public-access false \
    --min-tls-version TLS1_2

# Enable blob soft delete and versioning
az storage account blob-service-properties update \
    --account-name enterprisestorage \
    --resource-group storage-rg \
    --delete-retention-days 30 \
    --container-delete-retention-days 30 \
    --enable-versioning true \
    --enable-delete-retention true

# Enable static website (for this lab)
az storage blob service-properties update \
    --account-name enterprisestorage \
    --resource-group storage-rg \
    --static-website \
    --index-document index.html \
    --error-document error.html
```

### Module 2: Blob Storage Operations

```bash
# Create containers
az storage container create \
    --name raw-data \
    --account-name enterprisestorage \
    --public-access off

az storage container create \
    --name processed \
    --account-name enterprisestorage \
    --public-access off

az storage container create \
    --name archive \
    --account-name enterprisestorage \
    --public-access off

# Upload sample data
echo "sample data 1" > sample1.txt
echo "sample data 2" > sample2.txt

az storage blob upload \
    --file sample1.txt \
    --container-name raw-data \
    --name data1.txt \
    --account-name enterprisestorage

az storage blob upload \
    --file sample2.txt \
    --container-name raw-data \
    --name data2.txt \
    --account-name enterprisestorage

# Start copy operation
az storage blob copy start \
    --source-container raw-data \
    --source-blob data1.txt \
    --destination-container processed \
    --destination-blob data1_copy.txt \
    --account-name enterprisestorage
```

### Module 3: Azure Files SMB Deployment

```bash
# Create file share
az storage share create \
    --name companyfiles \
    --account-name enterprisestorage \
    --quota 100

# Generate storage account key
STORAGE_KEY=$(az storage account keys list \
    --account-name enterprisestorage \
    --resource-group storage-rg \
    --query "[0].value" -o tsv)

# Mount on Windows
cmdkey /add:enterprisestorage.file.core.windows.net /user:enterprisestorage /pass:$STORAGE_KEY
net use Z: \\enterprisestorage.file.core.windows.net\companyfiles

# Mount on Linux (requires cifs-utils)
# mount -t cifs //enterprisestorage.file.core.windows.net/companyfiles /mnt/files \
#   -o credentials=/etc/smbcredentials,file_mode=0777,dir_mode=0777
```

### Module 4: Lifecycle Policy Configuration

```bash
# Create lifecycle policy
cat > lifecycle.json << 'EOF'
{
  "rules": [
    {
      "name": "processRawData",
      "enabled": true,
      "type": "Lifecycle",
      "definition": {
        "filters": {
          "blobTypes": ["blockBlob"],
          "prefixMatch": ["raw-data/"]
        },
        "actions": {
          "baseBlob": {
            "tierToCool": {"daysAfterModificationGreaterThan": 30},
            "tierToArchive": {"daysAfterModificationGreaterThan": 90},
            "delete": {"daysAfterModificationGreaterThan": 365}
          }
        }
      }
    },
    {
      "name": "processProcessed",
      "enabled": true,
      "type": "Lifecycle",
      "definition": {
        "filters": {
          "blobTypes": ["blockBlob"],
          "prefixMatch": ["processed/"]
        },
        "actions": {
          "baseBlob": {
            "tierToArchive": {"daysAfterModificationGreaterThan": 60},
            "delete": {"daysAfterModificationGreaterThan": 180}
          }
        }
      }
    }
  ]
}
EOF

# Apply lifecycle policy
az storage account management-policy create \
    --policy @lifecycle.json \
    --account-name enterprisestorage \
    --resource-group storage-rg
```

### Module 5: Disaster Recovery Setup

```bash
# Create geo-redundant storage in secondary region
az storage account create \
    --name disasterstorage \
    --resource-group storage-rg \
    --location westus2 \
    --sku Standard_GRS

# Enable object replication
az storage account or-policy create \
    --account-name enterprisestorage \
    --resource-group storage-rg \
    --source-container raw-data \
    --destination-container raw-data \
    --destination-account disasterstorage

# Create sync to secondary
az storage account blob-inventory-policy create \
    --account-name enterprisestorage \
    --resource-group storage-rg \
    --schedule daily \
    --include-blob-properties true \
    --include-blob-tags true
```

## ⚠️ TROUBLESHOOTING

### Common Issues

```bash
# Check blob tier
az storage blob show \
    --container-name raw-data \
    --name data1.txt \
    --account-name enterprisestorage \
    --query "properties.blobTier"

# Check copy status
az storage blob show \
    --container-name processed \
    --name data1_copy.txt \
    --account-name enterprisestorage \
    --query "properties.copy"

# Verify replication status
az storage account show \
    --name enterprisestorage \
    --resource-group storage-rg \
    --query "geoPrimaryStats"

# Check lifecycle policy
az storage account management-policy show \
    --account-name enterprisestorage \
    --resource-group storage-rg
```

### Performance Testing

```bash
# Get storage metrics
az monitor metrics list \
    --resource enterprisestorage \
    --metric "Transactions" \
    --aggregation Total \
    --interval 1h

# Check egress
az monitor metrics list \
    --resource enterprisestorage \
    --metric "Egress" \
    --aggregation Total \
    --interval 1h
```

## ✅ VERIFICATION

### Test Storage Account

```bash
# Get account keys
az storage account keys list \
    --account-name enterprisestorage \
    --resource-group storage-rg

# Get connection string
az storage account show-connection-string \
    --account-name enterprisestorage \
    --resource-group storage-rg

# List containers
az storage container list \
    --account-name enterprisestorage

# List blobs in container
az storage blob list \
    --container-name raw-data \
    --account-name enterprisestorage
```

### Test Data Access

```bash
# Download blob
az storage blob download \
    --container-name raw-data \
    --name data1.txt \
    --file ./download.txt \
    --account-name enterprisestorage

# Generate SAS token
END=$(date -u -d "1 hour" "+%Y-%m-%dT%H:%M:%SZ")
SAS=$(az storage blob generate-sas \
    --container-name raw-data \
    --name data1.txt \
    --account-name enterprisestorage \
    --permissions r \
    --expiry $END -o tsv)

# Access with SAS
az storage blob show \
    --container-name raw-data \
    --name data1.txt \
    --account-name enterprisestorage \
    --sas-token $SAS
```

## 🧹 CLEANUP

```bash
# Delete lifecycle policies
az storage account management-policy delete \
    --account-name enterprisestorage \
    --resource-group storage-rg

# Delete containers
az storage container delete \
    --name raw-data \
    --account-name enterprisestorage

az storage container delete \
    --name processed \
    --account-name enterprisestorage

az storage container delete \
    --name archive \
    --account-name enterprisestorage

# Delete storage accounts
az storage account delete \
    --name enterprisestorage \
    --resource-group storage-rg \
    --yes

az storage account delete \
    --name disasterstorage \
    --resource-group storage-rg \
    --yes

# Delete resource group
az group delete \
    --name storage-rg \
    --yes
```

## 🔗 CROSS-REFERENCES

### Related Labs

- Azure Compute: VM storage integration
- Azure Key Vault: CMK encryption
- Azure Monitor: Storage metrics
- Azure Backup: VM backup