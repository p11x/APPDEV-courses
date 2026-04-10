---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Management Groups
Purpose: Practical management group implementation for enterprise scenarios
Difficulty: intermediate
Prerequisites: 01_Basic_Management_Groups.md
RelatedFiles: 01_Basic_Management_Groups.md, 02_Advanced_Management_Groups.md
UseCase: Multi-subscription deployment, governance automation
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Practical Management Group implementations enable organizations to scale governance across hundreds of subscriptions with consistent policy, access control, and organizational structure.

## 📖 WHAT

### Implementation Patterns

| Scenario | Approach | Benefits |
|----------|----------|----------|
| New tenant | Pre-created hierarchy | Start organized |
| Existing tenant | Phased migration | Minimize disruption |
| Multi-tenant | Separate roots | Full isolation |

### Common Hierarchies

```
Tenant Root
├── Production
│   ├── Prod-EastUS
│   └── Prod-WestEU
├── Development
│   ├── Dev-Sandbox
│   └── Dev-Shared
└── Corporate
    ├── IT
    └── Finance
```

## 🔧 HOW

### Example 1: Production Hierarchy

```bash
# Create production structure
az account management-group create --name 'prod' --display-name 'Production'
az account management-group create --name 'prod-app1' --display-name 'App 1' --parent 'prod'
az account management-group create --name 'prod-app2' --display-name 'App 2' --parent 'prod'

# Move subscriptions
az account management-group subscription move \
    --name 'prod-app1' \
    --subscription-id sub-app1
```

### Example 2: Sandbox Policy Enforcement

```bash
# Create sandbox policy
az policy definition create \
    --name 'sandbox-limits' \
    --display-name 'Sandbox Resource Limits' \
    --mode All \
    --rules '{
        "if": {
            "allOf": [
                {"field": "type", "equals": "Microsoft.Compute/virtualMachines"},
                {"field": "sku.name", "in": ["Standard_D32s_v3", "Standard_D64s_v3"]}
            ]
        },
        "then": {"effect": "Deny"}
    }'

# Assign to sandbox group
az policy assignment create \
    --name 'sandbox-limits' \
    --policy 'sandbox-limits' \
    --scope '/providers/Microsoft.Management/managementGroups/dev-sandbox'
```

### Example 3: Audit and Reporting

```bash
# Get subscription list per group
az account management-group list \
    --query '[?contains(name, "prod")].[{Name:name, Subscriptions:subscriptions}]'

# Export hierarchy to JSON
az account management-group list \
    --expand \
    > hierarchy-export.json
```

## ⚠️ COMMON ISSUES

- **Moving subscriptions**: Keep billing in sync
- **Policy conflicts**: Review inheritance
- **Deletion**: Remove all children first

## 🏃 PERFORMANCE

- Use automation for bulk operations
- Script policy deployment

## 🌐 COMPATIBILITY

- Terraform provider support
- Azure CLI full support

## 🔗 CROSS-REFERENCES

- **Azure Budgets**: Cost by group
- **Azure Policy**: Compliance by group

## ✅ EXAM TIPS

- Document hierarchy design
- Regular review cadence
- Automate with scripts