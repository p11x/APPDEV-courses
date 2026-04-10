---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Policy
Purpose: Advanced policy features including initiatives, exemptions, and remediation
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Policy.md
RelatedFiles: 01_Basic_Azure_Policy.md, 03_Practical_Azure_Policy.md
UseCase: Enterprise governance, multi-subscription policy management
CertificationExam: AZ-305 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Azure Policy features enable enterprise-scale governance with initiatives, remediation tasks, and sophisticated compliance automation across multiple subscriptions and management groups.

## 📖 WHAT

### Policy Initiatives

| Feature | Description | Use Case |
|---------|-------------|----------|
| Initiative | Group of related policies | Compliance package |
| Initiative Assignment | Apply to scope | Organization-wide |
| Initiative Parameters | Shared values | Reduce duplication |
| Built-in Initiatives | Azure provided | Security Center baselines |

### Remediation Tasks

| Type | Trigger | Purpose |
|------|---------|---------|
| DeployIfNotExists | Resource creation | Auto-create resources |
| Modify | Non-compliant state | Fix missing properties |
| Async Execution | Post-creation | Long-running fixes |

### Policy Exemptions

| Element | Purpose |
|---------|---------|
| Exemption Scope | Specific resource/scope |
| Exemption Category | Waiver ormitigation |
| Expiration | Time-limited exceptions |
| Justification | Business reason |

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| Policy Engine | Azure Policy | AWS Config | Organization Policy |
| Rules Language | JSON | JSON | ConstraintTemplates |
| Built-in Controls | Security Center | Config Rules | Organization Policy |
| Remediation | Tasks | Conformance Packs | Custom Constraints |
| Exemptions | Supported | Partial | Limited |

## 🔧 HOW

### Example 1: Create Policy Initiative

```bash
# Create initiative with multiple policies
az policy set-definition create \
    --name 'corp-compliance' \
    --display-name 'Corporate Compliance Initiative' \
    --description 'Required compliance policies' \
    --definitions '[
        {
            "policyDefinitionId": "/providers/Microsoft.Authorization/policyDefinitions/1f3cfdf2-2f5a-4e5a-8b4a-7e5a6b4a7e5a",
            "parameters": {
                "listOfAllowedLocations": {"value": ["eastus", "westus"]}
            }
        },
        {
            "policyDefinitionId": "/providers/Microsoft.Authorization/policyDefinitions/4f2f3cf-2f5a-4e5a-8b4a-7e5a6b4a7e5a"
        }
    ]'
```

### Example 2: Create Remediation Task

```bash
# Trigger remediation for deploy-if-not-exists
az policy remediation create \
    --name 'remediate-storage-config' \
    --policy-assignment 'storage-encryption' \
    --resource-group production-rg

# List remediation tasks
az policy remediation list --resource-group production-rg

# Get remediation status
az policy remediation show \
    --name 'remediate-storage-config' \
    --resource-group production-rg
```

### Example 3: Create Policy Exemption

```bash
# Create exemption for specific resource
az policy exemption create \
    --name 'dev-exemption' \
    --exemption-assignment-name 'allowed-locations' \
    --scope '/subscriptions/xxx/resourceGroups/dev-rg' \
    --exemption-category Waiver \
    --expires-on '2025-12-31' \
    --description 'Development sandbox exemption'

# List exemptions
az policy exemption list --scope '/subscriptions/xxx'
```

## ⚠️ COMMON ISSUES

### Initiative Issues

- **Parameter conflicts**: Same param name in different policies
- **Duplicate evaluation**: Initiative + direct policy assignment
- **Versioning**: Initiative updates don't auto-apply

### Remediation Issues

- **Permissions**: Managed identity required
- **Async timing**: Long-running remediation
- **Resource locks**: Blocked remediation

## 🏃 PERFORMANCE

### Optimization Strategies

| Strategy | Impact |
|----------|--------|
| Exclusions | Narrow scope reduces evaluation |
| Alias targeting | Specific fields vs all |
| Batch remediation | Process multiple resources |
| Scheduled tasks | Off-peak remediation |

### Enterprise Scale

| Scale | Recommendation |
|-------|--------------|
| <50 policies | Direct assignment |
| 50-200 | Initiatives |
| Multi-sub | Management group |

## 🌐 COMPATIBILITY

### Azure Policy Service Limits

| Limit | Value |
|-------|-------|
| Policy definitions | 500 per subscription |
| Initiative definitions | 200 per subscription |
| Assignments | 100 per scope |
| Exemptions | 10,000 per tenant |

## 🔗 CROSS-REFERENCES

- **Management Groups**: Hierarchical policy assignment
- **Azure Blueprints**: Policy in blueprint definition
- **Azure Security Center**: Security initiative integration

## ✅ EXAM TIPS

- Initiatives simplify management of related policies
- Use exemptions sparingly and track with expiration
- Remediation requires managed identity permissions
- Policy evaluations are real-time for new resources