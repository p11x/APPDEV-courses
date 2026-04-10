---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Blueprints
Purpose: Hands-on blueprint implementation for common scenarios
Difficulty: intermediate
Prerequisites: 01_Basic_Blueprints.md
RelatedFiles: 01_Basic_Blueprints.md, 02_Advanced_Blueprints.md
UseCase: Standard environments, enterprise templates, compliance automation
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Practical Blueprint implementations accelerate deployment while ensuring compliance, enabling organizations to consistently deploy standardized environments across dev, test, and production.

## 📖 WHAT

### Common Patterns

| Environment | Components | Use Case |
|-------------|-------------|----------|
| Dev | Basic resources | Development |
| Test | + monitoring | Testing |
| Prod | + security + locking | Production |

### Deployment Workflow

```
Blueprint → Publish → Assign → Deploy → Verify
```

## 🔧 HOW

### Example 1: Standard Web App Blueprint

```json
// storage.json artifact
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "storageName": {"type": "string"},
    "storageSku": {"type": "string", "defaultValue": "Standard_LRS"}
  },
  "resources": [{
    "type": "Microsoft.Storage/storageAccounts",
    "apiVersion": "2021-04-01",
    "name": "[parameters('storageName')]",
    "location": "[parameters('location')]",
    "sku": {"name": "[parameters('storageSku')]"},
    "kind": "StorageV2"
  }]
}
```

```bash
# Create blueprint with artifact
az blueprint create --name 'webapp' --display-name 'Web App'

# Add storage artifact
az blueprint artifact create \
    --blueprint-name 'webapp' \
    --name 'storage' \
    --type template \
    --display-name 'Storage Account' \
    --template-file storage.json

# Publish
az blueprint publish --blueprint-name 'webapp' --version '1.0'

# Assign
az blueprint assignment create \
    --blueprint-name 'webapp' \
    --name 'webapp-dev' \
    --location eastus \
    --subscription xxx \
    --parameters '{\"storageName\":{\"value\":\"devstore123\"}}'
```

### Example 2: Complete Environment Blueprint

```bash
# Create with resource group
az blueprint create --name 'complete-app'

# Create resource group artifact
az blueprint artifact create \
    --blueprint-name 'complete-app' \
    --name 'app-rg' \
    --type resourceGroup \
    --display-name 'Application Resource Group' \
    --dependsOn '[]'

# Add VNet template
az blueprint artifact create \
    --blueprint-name 'complete-app' \
    --name 'vnet' \
    --type template \
    --display-name 'Virtual Network' \
    --template-file vnet.json

# Add policy assignment
az blueprint artifact create \
    --blueprint-name 'complete-app' \
    --name 'enc-policy' \
    --type policyAssignment \
    --display-name 'Storage Encryption'
```

### Example 3: Blueprints with CI/CD

```yaml
# Azure Pipelines template
- stage: DeployBlueprint
  jobs:
    - job: Deploy
      steps:
        - task: AzureCLI@2
          inputs:
            azureSubscription: '$(serviceConnection)'
            scriptType: 'bash'
            scriptLocation: 'inlineScript'
            inlineScript: |
              az blueprint publish \
                --blueprint-name '$(blueprintName)' \
                --version '$(Build.BuildNumber)'
              az blueprint assignment create \
                --blueprint-name '$(blueprintName)' \
                --name '$(environment)' \
                --version '$(Build.BuildNumber)' \
                --location eastus \
                --subscription '$(subscription)' \
                --parameters '$(blueprintParams)'
```

## ⚠️ COMMON ISSUES

- **Template errors**: Validate before publishing
- **Parameter mismatches**: Match exactly
- **Assignment failures**: Check permissions

## 🏃 PERFORMANCE

- Parallel deployment
- Cached templates

## 🌐 COMPATIBILITY

- Azure CLI 2.0+
- PowerShell Az module
- REST API

## 🔗 CROSS-REFERENCES

- **ARM Templates**: Underlying IaC
- **Azure DevOps**: CI/CD integration

## ✅ EXAM TIPS

- Use consistent naming
- Version every change
- Test before publishing