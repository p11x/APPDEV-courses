---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Policy
Purpose: Define and enforce organizational policies to ensure compliance and resource governance
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_Azure_Policy.md, 03_Practical_Azure_Policy.md
UseCase: Enforce tagging standards, restrict resource locations, ensure security baselines
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Azure Policy is essential for governance because it allows organizations to enforce rules and ensure resources comply with corporate standards without manual review processes. It provides continuous compliance assessment and remediation.

## 📖 WHAT

### Policy Definition

| Element | Description |
|---------|-------------|
| Display Name | Human-readable policy name |
| Policy ID | Unique identifier |
| Description | Detailed explanation |
| Mode | Indexed or All for resource types |
| Parameters | Dynamic values for rules |
| Policy Rule | If/then conditions |

### Built-in Policies

| Policy Name | Effect | Purpose |
|-------------|--------|---------|
| Allowed Storage Account SKUs | Deny | Restrict storage tiers |
| Allowed Virtual Machine SKUs | Deny | Limit VM types |
| Tags Required | Append | Enforce tagging |
| Allowed Locations | Deny | Geographic restrictions |
| Audit VMs with unencrypted disks | Audit | Security compliance |

### Policy Effects

| Effect | Behavior |
|--------|----------|
| Deny | Blocks non-compliant resource creation |
| Audit | Creates warning for non-compliance |
| Append | Adds properties to resources |
| DenyIfNotExists | Denies if condition not met |
| DeployIfNotExists | Creates resource if missing |

## 🔧 HOW

### Example 1: Create Basic Tag Policy

```bash
# Create policy definition for required tag
az policy definition create \
    --name 'require-tag-owner' \
    --display-name 'Require Owner Tag' \
    --description 'Ensures resource has an Owner tag' \
    --mode Indexed \
    --rules '{
        "if": {
            "field": "tags[Owner]",
            "exists": false
        },
        "then": {
            "effect": "append",
            "details": [{
                "field": "tags[Owner]",
                "value": "unassigned"
            }]
        }
    }' \
    --params '{
        "tagName": {"type": "String", "defaultValue": "Owner"}
    }'
```

### Example 2: Restrict Resource Locations

```bash
# Create allowed locations policy
az policy definition create \
    --name 'allowed-locations' \
    --display-name 'Allowed Locations' \
    --description 'Restricts resource creation to approved regions' \
    --mode Indexed \
    --rules '{
        "if": {
            "not": {
                "field": "location",
                "in": ["eastus", "westus", "westeurope"]
            }
        },
        "then": {
            "effect": "Deny"
        }
    }'
```

### Example 3: Assign Policy to Resource Group

```bash
# Assign policy to resource group
az policy assignment create \
    --name 'enforce-tagging' \
    --policy 'require-tag-owner' \
    --resource-group production-rg

# List policy assignments
az policy assignment list --resource-group production-rg
```

## ⚠️ COMMON ISSUES

### Policy Assignment Issues

- **Scope confusion**: Policy inherits from parent, verify assignment level
- **Effect ordering**: Deny takes precedence over Append
- **Exemption management**: Track exemptions separately
- **Compliance delays**: May take 15-30 minutes to reflect

## 🏃 PERFORMANCE

### Policy Evaluation

| Factor | Impact |
|--------|--------|
| Number of policies | More policies = slower evaluation |
| Complex rules | Regex and exists checks slower |
| Resource count | Large subscriptions slower |
| Cache results | Use compliance snapshot |

## 🌐 COMPATIBILITY

### Supported Resources

- All ARM resources support policy
- Some legacy resources limited
- Extension resources have specific modes

## 🔗 CROSS-REFERENCES

- **Azure Blueprints**: Policy as part of blueprint
- **Management Groups**: Apply policy at hierarchy level
- **Azure Security Center**: Security baseline policies

## ✅ EXAM TIPS

- Policy evaluates at resource creation/update
- Use exemptions for legitimate exceptions
- Built-in policies save time vs custom
- Initiative groups multiple policies