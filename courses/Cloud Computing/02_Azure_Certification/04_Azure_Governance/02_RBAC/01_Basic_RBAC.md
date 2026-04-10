---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Role-Based Access Control
Purpose: Understand Azure RBAC fundamentals for access management
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_RBAC.md, 03_Practical_RBAC.md
UseCase: Grant appropriate access, implement least privilege
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Azure RBAC is the primary access control mechanism in Azure, enabling fine-grained permission management through built-in and custom roles. It follows the principle of least privilege and integrates with Azure AD for identity-based access.

## 📖 WHAT

### RBAC Concepts

| Concept | Description |
|---------|-------------|
| Role Definition | Collection of permissions |
| Role Assignment | Linking role to principal |
| Principal | User, group, or service |
| Scope | Resource hierarchy level |
| Permission | Action allowed/denied |

### Built-in Roles

| Role | Scope | Use Case |
|------|-------|----------|
| Owner | Full access | Admin management |
| Contributor | Manage resources | DevOps |
| Reader | View only | Auditors |
| User Access Administrator | Manage access | RBAC admin |
| Virtual Machine Contributor | VM management | Dev team |

### Scope Hierarchy

| Scope Level | Inheritance |
|-------------|-------------|
| Management Group | Child subs inherit |
| Subscription | Child groups inherit |
| Resource Group | Child resources inherit |
| Resource | Specific item only |

## 🔧 HOW

### Example 1: Assign Reader Role

```bash
# Assign reader to user at subscription scope
az role assignment create \
    --assignee 'user@domain.com' \
    --role 'Reader' \
    --scope '/subscriptions/xxx'

# Verify assignment
az role assignment list \
    --assignee 'user@domain.com' \
    --output table
```

### Example 2: Assign Custom Role to Group

```bash
# Get group object ID
groupId=$(az ad group show --group 'Developers' --query objectId -o tsc)

# Assign VM contributor to group
az role assignment create \
    --assignee $groupId \
    --role 'Virtual Machine Contributor' \
    --resource-group dev-rg
```

### Example 3: List Role Definitions

```bash
# List all built-in roles
az role definition list \
    --output table \
    --query '[].{Name:name, Type:type, ID:id}'

# List custom roles
az role definition list \
    --custom-role-only true \
    --output table
```

## ⚠️ COMMON ISSUES

### Assignment Issues

- **Propagation delay**: Changes may take 5 minutes
- **Duplicate assignments**: RBAC allows multiple
- **Inherited roles**: Child scope inherits
- **Permission verification**: Check effective access

## 🏃 PERFORMANCE

### Access Evaluation

| Factor | Impact |
|--------|--------|
| Number of roles | Slight impact on check |
| Role assignments | More assignments = slower |
| Custom roles | Same as built-in |

## 🌐 COMPATIBILITY

### Integration Points

- Azure AD for identity
- Azure Resource Manager for scopes
- Privileged Identity Management for just-in-time

## 🔗 CROSS-REFERENCES

- **Azure AD**: Identity provider
- **Azure Policy**: Complementary controls
- **Management Groups**: Scope organization

## ✅ EXAM TIPS

- Roles assign at scope - check inheritance
- Use groups for easier management
- RBAC uses allow model only (no deny rules)
- Check effective permissions via portal