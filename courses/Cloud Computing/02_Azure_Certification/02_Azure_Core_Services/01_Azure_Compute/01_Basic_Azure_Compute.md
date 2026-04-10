---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Compute
Purpose: Understanding Azure virtual machines, VM scale sets, and Azure Compute services
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Compute.md, 03_Practical_Azure_Compute.md
UseCase: Running compute workloads in Azure
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure Compute provides various options for running applications in the cloud. Understanding these options helps choose the right compute solution.

## 📖 WHAT

### Azure Compute Options

| Service | Description | Use Case |
|---------|-------------|----------|
| Virtual Machines | IaaS virtual servers | Full control |
| VM Scale Sets | Auto-scaling VMs | High availability |
| App Service | PaaS web apps | Web hosting |
| Azure Functions | Serverless | Event-driven |
| AKS | Managed Kubernetes | Containers |
| Container Instances | Containers | Quick containers |

### VM Sizes

| Category | Series | Use Case |
|----------|--------|----------|
| General Purpose | B, D, A | Development, testing |
| Compute Optimized | F, H | HPC, batch processing |
| Memory Optimized | E, M | In-memory databases |
| Storage Optimized | L | Big data, log processing |
| GPU | NC, ND, NV | ML, graphics |

## 🔧 HOW

### Example 1: Create VM

```bash
# Create resource group
az group create --name myrg --location eastus

# Create VM
az vm create \
    --name myvm \
    --resource-group myrg \
    --image UbuntuLTS \
    --size Standard_DS1_v2 \
    --admin-username azureuser \
    --generate-ssh-keys

# Get VM details
az vm show --name myvm --resource-group myrg
```

### Example 2: VM Scale Sets

```bash
# Create VM scale set
az vmss create \
    --name myvmss \
    --resource-group myrg \
    --image UbuntuLTS \
    --vm-sku Standard_DS1_v2 \
    --instance-count 2 \
    --upgrade-mode Automatic

# Update instance count
az vmss update \
    --name myvmss \
    --resource-group myrg \
    --set sku.capacity=5
```

### Example 3: Azure Functions

```bash
# Create function app
az functionapp create \
    --name myfuncapp \
    --resource-group myrg \
    --storage-account mystorage \
    --consumption-plan-location eastus

# Deploy function code
az functionapp deployment source config-local-git \
    --name myfuncapp \
    --resource-group myrg
```

## ⚠️ COMMON ISSUES

- VM size not available in region
- Quota limits exceeded
- SSH key issues

## 🔗 CROSS-REFERENCES

**Related**: Azure Networking, Azure Storage

## ✅ EXAM TIPS

- VMs = IaaS (you manage OS)
- App Service = PaaS (managed)
- Scale sets for auto-scaling
- Functions for serverless