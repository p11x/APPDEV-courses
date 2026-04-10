---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Core Concepts - Advanced
Purpose: Advanced Azure resource management, subscription governance, and enterprise deployment patterns
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 01_Basic_Azure_Core.md, 03_Practical_Azure_Core.md
UseCase: Enterprise Azure deployment
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## WHY

Advanced Azure knowledge enables enterprise-scale deployments with proper governance, resource organization, and cross-subscription management.

### Enterprise Azure Requirements

- **Multi-Subscription**: Management group hierarchy
- **Resource Governance**: Tagging, policies
- **Cost Management**: Budgets, alerts
- **Compliance**: RBAC, blueprints

## WHAT

### Azure Resource Organization

| Level | Purpose | Management |
|-------|---------|------------|
| Tenant | AD root | Global admin |
| Management Group | Policy | Governance |
| Subscription | Billing | Budget |
| Resource Group | Grouping | ACL |
| Resource | Deployed service | Individual |

### Cross-Platform Comparison

| Concept | Azure | AWS | GCP |
|---------|-------|-----|-----|
| Root | Tenant | Account | Organization |
| Grouping | Management Group | Organization | Folder |
| Billing | Subscription | Account | Billing Account |
| Container | Resource Group | None | Project |

### Advanced Features

| Feature | Purpose | Implementation |
|---------|---------|---------------|
| Management Groups | Hierarchical governance | Policies at scale |
| Resource Tags | Cost allocation | Metadata |
| Azure Policy | Compliance | Enforcement |
| Blueprints | Standardization | Templates |

## HOW

### Example 1: Management Group Hierarchy

```bash
# Create management groups
az account management-group create \
    --name "organization-id" \
    --display-name "Organization"

az account management-group create \
    --name "product-line" \
    --display-name "Product Line" \
    --parent "organizations/organization-id"

# Assign subscription to management group
az account management-group subscription show \
    --name "product-line" \
    --subscription "subscription-id"

# List management groups
az account management-group list
```

### Example 2: Resource Tagging Policies

```bash
# Create tagging policy
az policy definition create \
    --name "require-tags" \
    --display-name "Require tags on resources" \
    --description "Enforces required tags" \
    --rules '{
        "if": {
            "allOf": [
                {"field": "type", "equals": "Microsoft.Resources/subscriptions/resourceGroups"},
                {"not": {"field": "tags[Environment]", "exists": false}]
            }
        },
        "then": {
            "effect": "deny"
        }
    }'

# Assign policy to management group
az policy assignment create \
    --name "require-tags-assignment" \
    --display-name "Require Tags" \
    --policy "require-tags" \
    --scope "/providers/Microsoft.Management/managementGroups/product-line"
```

### Example 3: Resource Locks

```bash
# Create read-only lock on resource group
az lock create \
    --name "production-lock" \
    --lock-type ReadOnly \
    --resource-group production-rg \
    --notes "Protects production resources"

# Create delete lock
az lock create \
    --name "cannot-delete" \
    --lock-type CanNotDelete \
    --resource-group production-rg

# List locks
az lock list --resource-group production-rg

# Delete lock
az lock delete --name "cannot-delete" \
    --resource-group production-rg
```

### Example 4: Azure Blueprints

```bash
# Create blueprint
az blueprint create \
    --name "standard-webapp" \
    --display-name "Standard Web App" \
    --description "Standard web application template"

# Add webapp artifact
az blueprint artifact create \
    --blueprint-name "standard-webapp" \
    --name "webapp-template" \
    --type template \
    --path "templates/webapp.json"

# Add policy artifact
az blueprint artifact create \
    --blueprint-name "standard-webapp" \
    --name "location-policy" \
    --type policyAssignment \
    --policy "location-policy"

# Publish blueprint
az blueprint publish \
    --blueprint-name "standard-webapp" \
    --version "1.0"

# Assign blueprint to management group
az blueprint assignment create \
    --blueprint-name "standard-webapp" \
    --assignment-name "webapp-prod" \
    --location "eastus" \
    --management-group "product-line"
```

## COMMON ISSUES

### 1. Permission Issues

**Problem**: Cannot access resources.

**Solution**:
```bash
# Check role assignment
az role assignment list --resource-group rg-name

# Add contributor role
az role assignment create \
    --role Contributor \
    --assignee user@domain.com \
    --scope /subscriptions/id/rg/rg-name
```

### 2. Policy Blocking Resources

**Problem**: Deployment blocked.

**Solution**:
```bash
# Check denial details
az policy assignment list --scope /subscriptions/id

# Exempt specific resource
az policy exemption create \
    --name "exemption-1" \
    --assignment "policy-id" \
    --scope /subscriptions/id/rg/rg-name
```

### 3. Lock Issues

**Problem**: Cannot modify resources.

**Solution**:
```bash
# Identify lock
az lock list --resource-group rg-name

# Remove lock before modification
az lock delete --name lock-name \
    --resource-group rg-name
```

### 4. Tag Not Working

**Problem**: Tags not applying.

**Solution**:
- Enable inheritance
- Check policy
- Add parent tags

### 5. Blueprint Assignment Issues

**Problem**: Assignment fails.

**Solution**:
- Check permissions
- Verify parameters
- Review version

## PERFORMANCE

### Azure Limits

| Resource | Limit | Scope |
|----------|-------|-------|
| Subscriptions | 100 | Tenant |
| Resource Groups | 980 | Subscription |
| Locks | 200 | Resource |
| Tags | 50 | Resource |

### Query Performance

| Query | Response Time |
|-------|--------------|
| Resource list | 1-3 seconds |
| Graph query | 2-5 seconds |
| Policy evaluation | Immediate |

## COMPATIBILITY

### RBAC vs Azure Policy

| Feature | RBAC | Policy |
|---------|-----|--------|
| Access control | Yes | No |
| Compliance | No | Yes |
| Scope | Role | Any |
| Inheritance | Yes | Yes |

### Azure Policy Modes

| Mode | Behavior |
|------|----------|
| Indexed | Evaluates new resources |
| All | Evaluates existing |

## CROSS-REFERENCES

### Prerequisites

- Azure basics
- Azure CLI

### What to Study Next

1. Practical Azure Core
2. Azure Policies
3. Blueprints

## EXAM TIPS

### Key Exam Facts

- Management groups for organization
- Resource locks protect resources
- Policy enforces compliance
- Blueprints for standardization

### Exam Questions

- **Question**: "Organization hierarchy" = Management groups
- **Question**: "Prevent deletion" = Resource lock
- **Question**: "Compliance enforcement" = Azure Policy
- **Question**: "Standard deployment" = Blueprint