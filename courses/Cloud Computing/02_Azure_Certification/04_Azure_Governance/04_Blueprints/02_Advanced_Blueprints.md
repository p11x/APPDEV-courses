---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Blueprints
Purpose: Enterprise blueprint patterns, security baselines, CI/CD integration
Difficulty: advanced
Prerequisites: 01_Basic_Blueprints.md
RelatedFiles: 01_Basic_Blueprints.md, 03_Practical_Blueprints.md
UseCase: Enterprise deployments, security compliance, automated pipelines
CertificationExam: AZ-305 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Blueprint patterns enable enterprise-scale deployments with integrated security baselines, CI/CD automation, and multi-environment management with versioning and rollback capabilities.

## 📖 WHAT

### Advanced Features

| Feature | Use Case | Benefit |
|---------|----------|---------|
| Security Baseline | CIS, NIST compliance | Pre-configured security |
| Version Management | Environment promotion | Change tracking |
| Locking | Production protection | Prevent modifications |
| Parameters | Environment flexibility | Single blueprint |

### Security Baselines

| Standard | Policies | RBAC |
|-----------|----------|------|
| CIS Azure | 50+ | Configured |
| NIST 800-53 | 100+ | Configured |
| ISO 27001 | 40+ | Configured |

### Artifact Sequencing

| Sequence | Artifact | Purpose |
|----------|---------|---------|
| 1 | Resource Groups | Create containers |
| 2 | ARM Templates | Deploy resources |
| 3 | Policy | Apply governance |
| 4 | RBAC | Set access |

## 🔧 HOW

### Example 1: Enterprise Blueprint with Sec Baseline

```bash
# Create enterprise blueprint
az blueprint create \
    --name 'enterprise-secure' \
    --display-name 'Enterprise Security' \
    --description 'CIS Azure Foundation'

# Add policy assignment
az blueprint artifact create \
    --blueprint-name 'enterprise-secure' \
    --name 'security-baseline' \
    --type policyAssignment \
    --display-name 'Enable Security Center' \
    --policy '/providers/Microsoft.Authorization/policySetDefinitions/SecurityCenter' \
    --parameters '{\"enabledApiForAzureSecurityCenter\":{\"value\":true}}'
```

### Example 2: Parameterized Blueprint

```bash
# Create with parameters
az blueprint create \
    --name 'parameterized-app' \
    --display-name 'Parameterized App' \
    --parameters '{
        "appName": {"type": "string"},
        "env": {"type": "string", "default": "dev"},
        "tier": {"type": "string", "default": "standard"}
    }'

# Use in ARM template
# "parameters": {
#   "appName": {"value": "[parameters('appName')]"}
# }
```

### Example 3: Blueprint Versioning

```bash
# Release v1.0
az blueprint publish \
    --blueprint-name 'standard-app' \
    --version '1.0' \
    --changes 'Initial release'

# Update and release v1.1
az blueprint publish \
    --blueprint-name 'standard-app' \
    --version '1.1' \
    --changes 'Added monitoring'

# Assign specific version
az blueprint assignment create \
    --blueprint-name 'standard-app' \
    --name 'prod-app' \
    --version '1.0' \
    --location eastus \
    --subscription xxx
```

## ⚠️ COMMON ISSUES

- **Circular dependencies**: Order artifacts carefully
- **Lock conflicts**: Cannot modify locked assignments
- **Version drift**: Specify versions explicitly

## 🏃 PERFORMANCE

- Parallel artifact deployment
- Cached templates

## 🌐 COMPATIBILITY

| Integration | Support |
|--------------|----------|
| Azure DevOps | Full |
| GitHub Actions | Full |
| Terraform | Partial |

## 🔗 CROSS-REFERENCES

- **Azure Policy**: Security initiatives
- **ARM Templates**: Underlying IaC

## ✅ EXAM TIPS

- Use semantic versioning
- Test at each environment
- Lock production assignments