---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Management Groups
Purpose: Organize and manage Azure resources at scale
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_Management_Groups.md, 03_Practical_Management_Groups.md
UseCase: Multi-subscription governance, policy inheritance
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Management Groups provide hierarchical organization for Azure resources, enabling consistent policy and access management across multiple subscriptions. They are essential for enterprise-scale Azure deployments.

## 📖 WHAT

### Hierarchy Structure

| Level | Purpose |
|-------|---------|
| Tenant Root | All management groups |
| Management Group | Subscription grouping |
| Subscription | Billing/isolation |
| Resource Group | Resource grouping |
| Resource | Deployable unit |

### Default Hierarchy

- Tenants have one root management group
- Subscriptions default to root
- Can create up to 6 levels deep

### Key Properties

| Property | Description |
|-----------|-------------|
| Display Name | Human-readable name |
| Management Group ID | Unique identifier |
| Parent | Inherit settings |
| Subscriptions | Child subscriptions |

## 🔧 HOW

### Example 1: Create Management Group

```bash
# Create management group
az account management-group create \
    --name 'mg-production' \
    --display-name 'Production'

# Move subscription to group
az account subscription move \
    --source-group root \
    --target-group 'mg-production' \
    --subscription-id xxx
```

### Example 2: List Hierarchy

```bash
# List all management groups
az account management-group list \
    --output table

# Show group details
az account management-group show \
    --name 'mg-production'
```

### Example 3: Move Subscriptions

```bash
# Move subscription between groups
az account management-group subscription move \
    --name 'mg-production' \
    --subscription-id xxx

# Remove subscription
az account management-group subscription remove \
    --name 'mg-production' \
    --subscription-id xxx
```

## ⚠️ COMMON ISSUES

- **Root permissions**: Must have AA at root to manage
- **Moving restrictions**: Can only move one sub at a time
- **Deletion**: Cannot delete with children

## 🏃 PERFORMANCE

- Hierarchy updates propagate in minutes
- Policies inherit from parent

## 🌐 COMPATIBILITY

- Supports up to 10,000 subscriptions
- Azure Lighthouse compatible

## 🔗 CROSS-REFERENCES

- **Azure Policy**: Apply at mgmt group
- **RBAC**: Assign roles at mgmt group

## ✅ EXAM TIPS

- Create logical grouping by workload
- Use management groups not sub-folders
- Policies flow down automatically