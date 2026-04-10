---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Management Tools - Advanced
Purpose: Advanced Azure management with automation scripts, Azure Policy, and Bicep templates
Difficulty: advanced
Prerequisites: 01_Basic_Management_Tools.md
RelatedFiles: 01_Basic_Management_Tools.md, 03_Practical_Management_Tools.md
UseCase: Enterprise automation
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## WHY

Advanced Azure management uses declarative templates and automation for infrastructure as code and governance at scale.

### Enterprise Automation Needs

- **Reproducibility**: Consistent deployments
- **Version Control**: Infrastructure as code
- **Policy as Code**: Compliance enforcement
- **Automation**: Reduce manual work

## WHAT

### Azure IaC Tools

| Tool | Type | Use Case |
|------|-----|----------|
| ARM Templates | Declarative | Azure deployments |
| Bicep | Declarative | Simplified ARM |
| Terraform | Declarative | Multi-cloud |
| Ansible | Imperative | Configuration |

### Cross-Platform Comparison

| Tool | Azure | AWS | GCP |
|------|------|-----|-----|
| Template | ARM | CloudFormation | Deployment Manager |
| CLI | Az | AWS CLI | gcloud |
| SDK | Multiple | Boto3 | Client libraries |
| State | REST API | S3 | Cloud Storage |

### Azure Automation Services

| Service | Purpose | API |
|---------|---------|-----|
| Automation | Runbooks | REST |
| Functions | Serverless | HTTP |
| Logic Apps | Workflow | Designer |
| DevOps | Pipelines | YAML |

## HOW

### Example 1: Bicep Template

```bash
# Deploy bicep template
az deployment group create \
    --name app-deployment \
    --resource-group rg \
    --template-file main.bicep

# Validate first
az deployment group validate \
    --resource-group rg \
    --template-file main.bicep
```

```bicep
// main.bicep
param appName string = 'webapp'
param location string = resourceGroup().location

var appServicePlanName = '${appName}-asp'
var webAppName = '${appName}-web'

resource asp 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'B1'
    capacity: 1
  }
}

resource webapp 'Microsoft.Web/sites@2020-06-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: asp.id
  }
}
```

### Example 2: Azure Policy as Code

```bash
# Create policy definition
az policy definition create \
    --name "allowed-storage-sku" \
    --display-name "Allowed storage SKUs" \
    --rules '{
        "if": {
            "field": "type",
            "equals": "Microsoft.Storage/storageAccounts/sku.name"
        },
        "then": {
            "effect": "deny",
            "details": {
                "notValues": ["Standard_LRS", "Standard_GRS"]
            }
        }
    }'

# Assign policy
az policy assignment create \
    --name "storage-sku-assignment" \
    --display-name "Allowed Storage SKUs" \
    --policy "allowed-storage-sku" \
    --scope /subscriptions/id/resourceGroups/rg
```

### Example 3: Automation Runbook

```powershell
# Azure Automation runbook
param(
    [string]$ResourceGroupName,
    [string]$VMName
)

$context = Get-AzContext
$vm = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName

if ($vm.PowerState -eq "VM running") {
    Write-Output "VM is already running"
} else {
    Start-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName
    Write-Output "VM started successfully"
}
```

### Example 4: Azure CLI Automation

```bash
# Script to deploy multi-tier app
#!/bin/bash

# Create resource groups
az group create --name network-rg --location eastus
az group create --name app-rg --location eastus
az group create --name data-rg --location eastus

# Create network
az network vnet create \
    --name app-vnet \
    --resource-group network-rg \
    --address-prefixes 10.0.0.0/16

# Create VM with tagging
az vm create \
    --name app-vm \
    --resource-group app-rg \
    --image UbuntuLTS \
    --tags Environment=Production Team=App

# Get inventory
az resource list --tag Environment=Production
```

## COMMON ISSUES

### 1. Template Validation Failing

**Problem**: Deployment validation fails.

**Solution**:
```bash
# Review error message
az deployment group validate \
    --resource-group rg \
    --template-file template.json \
    --debug

# Check syntax
az bicep build --file template.bicep
```

### 2. Policy Not Working

**Problem**: Policy not applying.

**Solution**:
- Check assignment scope
- Wait for propagation
- Verify definition

### 3. Runbook Not Executing

**Problem**: Runbook errors.

**Solution**:
```powershell
# Test in sandbox
Connect-AzAccount
Get-AzContext

# Check job status
Get-AzAutomationJob -AutomationAccountName account -ResourceGroupName rg
```

### 4. Rate Limiting

**Problem**: Too many API calls.

**Solution**:
- Implement retry logic
- Use batching
- Check throttling limits

## PERFORMANCE

### Deployment Performance

| Template Type | Validation | Deployment |
|--------------|-------------|------------|
| ARM | 10-30 seconds | 1-3 minutes |
| Bicep | 5-15 seconds | 1-3 minutes |
| Terraform | 10-20 seconds | 1-3 minutes |

### API Limits

| Operation | Limit | Throttle |
|-----------|-------|---------|
| Read | 120/min | Exponential |
| Write | 60/min | Exponential |
| Delete | 60/min | Exponential |

## COMPATIBILITY

### Bicep Version Support

| Version | Azure CLI | VS Code |
|---------|---------|----------|
| 0.4+ | Yes | Yes |
| 0.3+ | Yes | Yes |
| 0.2+ | Yes | Yes |

### Template Exports

| Source | Format | Export |
|--------|--------|--------|
| Portal | ARM JSON | Manual |
| CLI | ARM JSON | az export |
| VS Code | Bicep | Extension |

## CROSS-REFERENCES

### Prerequisites

- Azure CLI basics
- Basic scripting

### What to Study Next

1. Practical Management Tools
2. IaC practices
3. CI/CD pipelines

## EXAM TIPS

### Key Exam Facts

- Bicep vs ARM: Bicep is simplified ARM
- Policy applies during deployment
- Runbooks for automation
- Templates for reproducibility

### Exam Questions

- **Question**: "Infrastructure as code" = ARM/Bicep
- **Question**: "Compliance enforcement" = Azure Policy
- **Question**: "Deployment templates" = ARM/Bicep
- **Question**: "Automation runbook" = Azure Automation