---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Blueprints
Purpose: Define repeatable deployment patterns for Azure resources
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_Blueprints.md, 03_Practical_Blueprints.md
UseCase: Standard environments, compliance baselines, quick deployment
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Azure Blueprints enable organizations to define and deploy repeatable, compliant infrastructure patterns. They package ARM templates, policy assignments, RBAC roles, and resource groups into a single deployable unit.

## 📖 WHAT

### Blueprint Components

| Component | Purpose | Order |
|-----------|---------|------|
| Artifacts | Resources to deploy | 1 |
| Policy Assignments | Governance rules | 2 |
| RBAC Assignments | Access control | 2 |
| Resource Groups | Container structure | 3 |

### Artifact Types

| Type | Description |
|------|-------------|
| ARM Template | JSON resource definition |
| Policy Assignment | Policy enforcement |
| Role Assignment | Access grant |

### Blueprint Properties

| Property | Description |
|-----------|-------------|
| Version | Semantic versioning |
| Description | Purpose documentation |
| Parameters | Input values |

## 🔧 HOW

### Example 1: Create Basic Blueprint

```bash
# Create blueprint
az blueprint create \
    --name 'standard-webapp' \
    --display-name 'Standard Web App' \
    --description 'Basic web app template'

# Add ARM template artifact
az blueprint artifact create \
    --blueprint-name 'standard-webapp' \
    --name 'webapp-storage' \
    --type template \
    --display-name 'Storage Account' \
    --template file://storage.json
```

### Example 2: Publish Blueprint

```bash
# Publish blueprint
az blueprint publish \
    --blueprint-name 'standard-webapp' \
    --version '1.0' \
    --changes 'Initial release'

# Assign blueprint to subscription
az blueprint assignment create \
    --blueprint-name 'standard-webapp' \
    --name 'prod-webapp' \
    --location eastus \
    --subscription xxx
```

### Example 3: List Blueprints

```bash
# List blueprints
az blueprint list \
    --output table

# Show blueprint details
az blueprint show \
    --name 'standard-webapp'
```

## ⚠️ COMMON ISSUES

- **Publishing required**: Must publish before assignment
- **Artifact limits**: 100 per blueprint
- **Parameter values**: Required at assignment

## 🏃 PERFORMANCE

- Quick deployment via blueprints
- Consistent, repeatable results

## 🌐 COMPATIBILITY

- Azure DevOps integration
- GitHub workflows support

## 🔗 CROSS-REFERENCES

- **ARM Templates**: Underlying templates
- **Policy**: Built-in enforcement

## ✅ EXAM TIPS

- Use versioning for changes
- Document all parameters
- Test before production