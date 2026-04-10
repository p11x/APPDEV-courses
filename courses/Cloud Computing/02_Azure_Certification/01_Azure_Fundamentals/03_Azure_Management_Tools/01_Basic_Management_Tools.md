---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Management Tools
Purpose: Understanding Azure management tools including Portal, CLI, PowerShell, and ARM templates
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Management_Tools.md, 03_Practical_Management_Tools.md
UseCase: Managing Azure resources efficiently
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure management tools enable efficient deployment, configuration, and management of Azure resources. Understanding these tools is essential for cloud administrators.

## 📖 WHAT

### Azure Management Options

| Tool | Description | Use Case |
|------|-------------|----------|
| Azure Portal | Web-based GUI | Visual management |
| Azure CLI | Command-line interface | Scripting |
| Azure PowerShell | PowerShell cmdlets | Automation |
| ARM Templates | Infrastructure as Code | Reproducible deployments |
| Azure SDKs | Programmatic access | Custom applications |

## 🔧 HOW

### Example 1: Azure CLI

```bash
# Login
az login

# List resources
az resource list

# Create resource group
az group create --name myrg --location eastus

# Create VM
az vm create \
    --name myvm \
    --resource-group myrg \
    --image UbuntuLTS \
    --generate-ssh-keys
```

### Example 2: Azure PowerShell

```powershell
# Connect
Connect-AzAccount

# Create resource group
New-AzResourceGroup -Name myrg -Location eastus

# Create VM
New-AzVm -ResourceGroupName myrg -Name myvm -Image UbuntuLTS
```

## ✅ EXAM TIPS

- Portal = visual management
- CLI/PowerShell = scripting
- ARM = declarative IaC