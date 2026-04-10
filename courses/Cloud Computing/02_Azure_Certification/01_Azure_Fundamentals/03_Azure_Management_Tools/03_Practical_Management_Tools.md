---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Management Tools - Practical
Purpose: Production automation with Bicep templates, scripts, and Azure DevOps
Difficulty: practical
Prerequisites: 01_Basic_Management_Tools.md, 02_Advanced_Management_Tools.md
RelatedFiles: 01_Basic_Management_Tools.md, 02_Advanced_Management_Tools.md
UseCase: Enterprise automation implementation
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## WHY

Practical automation ensures consistent, repeatable deployments and enables CI/CD practices.

## WHAT

### Automation Pipeline

```
Azure CI/CD Pipeline
==================

┌──────────┐    ┌──────────┐    ┌──────────┐    ┌──────────┐
│ Code     │───►│ Build   │───►│ Test    │───►│ Deploy  │
│ Commit   │    │         │    │         │    │         │
└──────────┘    └──────────┘    └──────────┘    └──────────┘
     │                                                   │
     │   ┌──────────────────────────────────────────────────┘
     ▼   ▼
┌──────────┐
│ Azure    │
│ Resource │
└──────────┘
```

## HOW

### Lab 1: Multi-Environment Deploy

```bash
# Deploy to dev
az deployment group create \
    --name dev-deploy \
    --resource-group rg-dev \
    --template-file main.bicep \
    --parameters environment=dev

# Deploy to prod
az deployment group create \
    --name prod-deploy \
    --resource-group rg-prod \
    --template-file main.bicep \
    --parameters environment=prod
```

### Lab 2: Bicep with Modules

```bicep
// main.bicep
module network 'network.bicep' = {
  name: 'network-module'
  params: {
    vnetName: vnetName
    location: location
  }
}

module app 'app.bicep' = {
  name: 'app-module'
  params: {
    appServicePlanName: asp.name
    location: location
  }
  dependsOn: [network]
}
```

### Lab 3: GitHub Actions Integration

```yaml
name: Azure Deploy

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      
      - uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}
      
      - name: Deploy
        run: |
          az deployment group create \
            --resource-group ${{ env.RG_NAME }} \
            --template-file main.bicep
```

### Lab 4: Script Automation

```bash
#!/bin/bash
# Deploy application with full lifecycle

ENV=$1

# Validate template
echo "Validating template..."
az deployment group validate \
    --resource-group rg-${ENV} \
    --template-file main.bicep \
    --parameters env=${ENV}

# Deploy
echo "Deploying to ${ENV}..."
az deployment group create \
    --name app-deploy-${ENV} \
    --resource-group rg-${ENV} \
    --template-file main.bicep \
    --parameters env=${ENV}

# Verify
echo "Verifying deployment..."
STATUS=$(az webapp show \
    --name app-${ENV} \
    --resource-group rg-${ENV} \
    --query state)

if [ "$STATUS" == "Running" ]; then
    echo "Deployment successful"
else
    echo "Deployment failed"
    exit 1
fi
```

## COMMON ISSUES

### 1. Deployment Failures

**Problem**: Resources not deploying.

**Solution**:
- Check template syntax
- Review parameters
- Check permissions

### 2. Module Resolution

**Problem**: Cannot find modules.

**Solution**:
- Use relative paths
- Check storage account
- Use mcr.microsoft.com bicep registry

### 3. Secrets Management

**Problem**: Secrets exposed.

**Solution**:
- Use Key Vault references
- Never commit secrets
- Use Azure Key Vault task

### 4. Pipeline Errors

**Problem**: Build fails.

**Solution**:
- Check service principal
- Verify RBAC
- Review logs

### 5. State Management

**Problem**: State conflicts.

**Solution**:
- Use remote state
- Clean up failed deployments
- Check for resource locks

## PERFORMANCE

### Deployment Benchmarks

| Operation | Duration |
|-----------|----------|
| Template validation | 10-30 seconds |
| Single resource | 30-60 seconds |
| Multi-resource | 2-5 minutes |
| CI/CD pipeline | 5-10 minutes |

### Troubleshooting Performance

| Action | Typical Time |
|--------|-------------|
| Validation debug | 2-5 minutes |
| RBAC check | 1-2 minutes |
| Resource diagnostics | 5-10 minutes |

## COMPATIBILITY

### Bicep CLI Versions

| Feature | v0.3 | v0.4+ |
|---------|-------|-------|
| Modules | Yes | Yes |
| User-defined types | No | Yes |
| Test framework | No | Yes |
| Side-by-side | Yes | Yes |

### Azure DevOps Tasks

| Task | Version | Use |
|------|--------|-----|
| AzureResourceManager | 4.* | Deploy |
| AzureCLI | 2.* | Scripts |
| Bicep | 1.* | Build |

## CROSS-REFERENCES

### Prerequisites

- Azure CLI basics
- Git basics

### Next Steps

1. Azure DevOps pipelines
2. GitHub Actions
3. Terraform integration

## EXAM TIPS

### Production Patterns

- Use validation step
- Implement gates
- Automated testing
- Approval flows