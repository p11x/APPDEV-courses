---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Role-Based Access Control
Purpose: Advanced RBAC features including custom roles, PIM, and enterprise patterns
Difficulty: advanced
Prerequisites: 01_Basic_RBAC.md
RelatedFiles: 01_Basic_RBAC.md, 03_Practical_RBAC.md
UseCase: Enterprise access management, just-in-time access, custom role design
CertificationExam: AZ-305 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced RBAC enables enterprise-grade access management with custom roles tailored to specific workloads, Privileged Identity Management (PIM) for just-in-time access, and comprehensive access reviews for compliance.

## 📖 WHAT

### Custom Role Design

| Element | Purpose | Example |
|---------|---------|---------|
| AssignableScopes | Where role can be assigned | /subscriptions/xxx |
| Permissions | Allowed actions | Microsoft.Compute/* |
| Description | Human-readable purpose | Manage VMs only |

### Role Permissions Matrix

| Action | Description | Wildcard Support |
|--------|-------------|------------------|
| */read | Read any resource | Yes |
| Microsoft.Compute/virtualMachines/* | Full VM access | Yes |
| Microsoft.Sql/servers/databases/read | DB read-only | No |

### Custom Role JSON Structure

```json
{
  "Name": "Custom VM Operator",
  "Description": "Can start/stop VMs but not delete",
  "Actions": [
    "Microsoft.Compute/virtualMachines/start/action",
    "Microsoft.Compute/virtualMachines/restart/action",
    "Microsoft.Compute/virtualMachines/read"
  ],
  "NotActions": [],
  "DataActions": [],
  "AssignableScopes": ["/subscriptions/xxx"]
}
```

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| Built-in Roles | 100+ | IAM roles | Primitive roles |
| Custom Roles | Yes | Yes | Yes |
| Permission Model | Allow only | Allow + deny | Allow only |
| Just-in-Time | PIM | IAM access analyzer | IAP |
| Access Reviews | Yes | Access analyzer | Access Transparency |

## 🔧 HOW

### Example 1: Create Custom Role

```bash
# Create custom role definition
az role definition create \
    --role-definition '{
        "Name": "VM Operator",
        "Description": "Can start and stop VMs",
        "Actions": [
            "Microsoft.Compute/virtualMachines/start/action",
            "Microsoft.Compute/virtualMachines/restart/action",
            "Microsoft.Compute/virtualMachines/read"
        ],
        "AssignableScopes": ["/subscriptions/xxx"]
    }'
```

### Example 2: PIM Role Activation

```bash
# Check eligible roles
az role eligibility list \
    --scope '/subscriptions/xxx' \
    --assignee 'user@domain.com'

# Request role activation
az role assignment approve \
    --name 'eligibility-assignment-id' \
    --scope '/subscriptions/xxx'

# Activate role (via portal or API)
# Requires MFA and justification
```

### Example 3: Access Review Workflow

```bash
# Create access review
az monitor access-review create \
    --name 'quarterly-review' \
    --review-type 'Principal' \
    --start-date '2025-01-01' \
    --end-date '2025-03-31' \
    --scope '/subscriptions/xxx' \
    --reviewers 'reviewer@domain.com'

# List access reviews
az monitor access-review list \
    --scope '/subscriptions/xxx'
```

## ⚠️ COMMON ISSUES

### Custom Role Issues

- **Scope too broad**: Avoid at tenant root
- **Permission typos**: Use exact action strings
- **Role conflicts**: Test before production
- **Lifecycle management**: Version roles

### PIM Issues

- **MFA requirement**: Ensure MFA enrolled
- **Approval workflows**: Configure approvers
- **Notification settings**: Avoid alert fatigue

## 🏃 PERFORMANCE

### Enterprise RBAC Design

| Pattern | Use Case | Scale |
|---------|----------|-------|
| Central team | All admin actions | <500 users |
| Decentralized | Team-based | 500-5000 |
| Federated | Business unit | >5000 users |

### Role Assignment Optimization

- Use groups for bulk assignment
- Avoid nesting where possible
- Regular access reviews clean up

## 🌐 COMPATIBILITY

### RBAC Limits

| Limit | Value |
|-------|-------|
| Role assignments | 2000 per scope |
| Custom roles | 500 per tenant |
| Role definitions | 100 per scope |

## 🔗 CROSS-REFERENCES

- **Azure AD Admin Roles**: Directory-level roles
- **Azure Privileged Identity Management**: JIT access
- **Azure AD Access Reviews**: Periodic certification

## ✅ EXAM TIPS

- Custom roles should have minimal assignable scopes
- PIM requires eligible assignment first
- Access reviews mandatory for compliance
- Use naming conventions for role clarity