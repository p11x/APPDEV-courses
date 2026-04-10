---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Management Groups
Purpose: Advanced hierarchy design and enterprise organization patterns
Difficulty: advanced
Prerequisites: 01_Basic_Management_Groups.md
RelatedFiles: 01_Basic_Management_Groups.md, 03_Practical_Management_Groups.md
UseCase: Enterprise hierarchy, cross-tenant management
CertificationExam: AZ-305 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Management Group patterns enable sophisticated enterprise governance with multi-tenant support, hierarchical inheritance, and complex organizational structures.

## 📖 WHAT

### Enterprise Hierarchy Design

| Pattern | Structure | Use Case |
|---------|-----------|----------|
| Functional | By workload type | Shared services |
| Geographic | By region | Data residency |
| Departmental | By business unit | Cost attribution |
| Environment | Dev/Test/Prod | Lifecycle separation |

### Hierarchy Best Practices

- Limit depth to 3-4 levels
- Use naming conventions
- Document ownership
- Regular review cadence

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| Organization | Management Groups | Organizations | Organizations |
| Root | Tenant root | Organization root | Organization |
| Units | Management groups | OUs | Folders |
| Policy Inheritance | Yes | Yes | Yes |

## 🔧 HOW

### Example 1: Enterprise Hierarchy

```bash
# Create parent groups
az account management-group create \
    --name 'corp' \
    --display-name 'Corporate'

az account management-group create \
    --name 'dev' \
    --display-name 'Development' \
    --parent 'corp'

# Create environment subgroups
az account management-group create \
    --name 'dev-sandbox' \
    --display-name 'Sandbox' \
    --parent 'dev'
```

### Example 2: Policy at Hierarchy Level

```bash
# Create policy at parent level
az policy definition create \
    --name 'global-location' \
    --display-name 'Allowed Locations' \
    --rules '{"if":{"not":{"field":"location","in":["eastus","westus"]}},"then":{"effect":"Deny"}}'

# Assign at management group
az policy assignment create \
    --name 'global-location' \
    --policy 'global-location' \
    --scope '/providers/Microsoft.Management/managementGroups/corp'
```

### Example 3: RBAC at Management Group

```bash
# Assign admin to management group
az role assignment create \
    --assignee 'team@domain.com' \
    --role 'Owner' \
    --scope '/providers/Microsoft.Management/managementGroups/dev'
```

## ⚠️ COMMON ISSUES

- **Migration complexity**: Moving subscriptions between hierarchies
- **Policy conflicts**: Inherited vs direct assignment
- **Billing implications**: Costs at subscription level

## 🏃 PERFORMANCE

- Hierarchy operations scale to 10K+ subscriptions
- Policy evaluation uses cache

## 🌐 COMPATIBILITY

- Azure Lighthouse supports cross-tenant
- Azure Stack HCI integration

## 🔗 CROSS-REFERENCES

- **Azure Policy**: Initiative inheritance
- **Azure Cost Management**: Consolidated billing
- **Azure Security Center**: Security recommendations

## ✅ EXAM TIPS

- Plan hierarchy before scaling
- Use descriptive names
- Assign ownership at each level