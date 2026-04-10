---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Core Concepts - Practical
Purpose: Implementing production Azure resource organization with management groups, policies, and governance
Difficulty: practical
Prerequisites: 01_Basic_Azure_Core.md, 02_Advanced_Azure_Core.md
RelatedFiles: 01_Basic_Azure_Core.md, 02_Advanced_Azure_Core.md
UseCase: Production Azure governance
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## WHY

Practical Azure governance implements enterprise resource organization with proper policies and controls.

## WHAT

### Production Governance Architecture

```
Azure Governance Pipeline
====================

Tenant Root
    │
    ├── Management Groups (Hierarchical)
    │   ├── Production
    │   ├── Development
    │   └── Sandbox
    │
    ├── Subscriptions
    │   ├── Core Services
    │   ├── Workloads
    │   └── Shared Resources
    │
    ├── Resource Groups (Logical)
    │   ├── Network
    │   ├── Compute
    │   └── Data
    │
    └── Policies + Blueprints
        ├── Location restrictions
        ├── Tagging requirements
        └── Resource locks
```

## HOW

### Lab 1: Enterprise Organization

```bash
# Create management group structure
az account management-group create \
    --name "enterprise" \
    --display-name "Enterprise"

az account management-group create \
    --name "prod" \
    --display-name "Production" \
    --parent "enterprises/enterprise"

az account management-group create \
    --name "dev" \
    --display-name "Development" \
    --parent "enterprises/enterprise"

# Move subscription between groups
az account management-group subscription add \
    --name "prod" \
    --subscription "subscription-id"
```

### Lab 2: Resource Group Design

```bash
# Network resource group
az group create \
    --name network-rg \
    --location eastus

# Application resource groups
az group create \
    --name app-prod-rg \
    --location eastus

az group create \
    --name app-dev-rg \
    --location eastus

# Data resource group
az group create \
    --name data-rg \
    --location eastus
```

### Lab 3: Tag-Based Governance

```bash
# Add tags to resource group
az group update \
    --name app-prod-rg \
    --set tags.Environment=Production tags.Owner=team

# Query resources by tag
az resource list --tag "Environment=Production"

# Create cost tracking tag policy
az policy definition create \
    --name "cost-center-tag" \
    --display-name "Require Cost Center tag" \
    --rules '{
        "if": {
            "field": "type",
            "equals": "Microsoft.Resources/subscriptions/resourceGroups"
        },
        "then": {
            "effect": "deny",
            "details": {
                "operations": ["*"],
                "notValues": ["Microsoft.Resources/subscriptions/resourceGroups/read"],
                "conflictEffect": "deny"
            }
        }
    }'
```

### Lab 4: Lock-Based Protection

```bash
# Production lock
az lock create \
    --name "protect-prod" \
    --lock-type CanNotDelete \
    --resource-group app-prod-rg

# Verify before delete
az lock delete \
    --name "protect-prod" \
    --resource-group app-prod-rg
```

## COMMON ISSUES

### 1. Subscription Cannot Move

**Problem**: Cannot reassign subscription.

**Solution**:
- Check permissions
- Verify no budget blocks
- Clear pending bills

### 2. Policy Not Applying

**Problem**: No policy effect.

**Solution**:
- Check assignment
- Verify scope
- Wait for propagation

### 3. Resource Lock Conflict

**Problem**: Lock preventing updates.

**Solution**:
- Remove lock
- Apply changes
- Re-add lock

### 4. Tag Not Inherited

**Problem**: Tags not propagating.

**Solution**:
- Manual addition
- Policy enforcement
- Blueprints

## PERFORMANCE

### Resource Query Performance

| Query Type | Response Time |
|-----------|------------|
| Resource List | 2-3 seconds |
| Tag Query | 3-5 seconds |
| Policy Check | Immediate |

### Azure Resource Limits

| Limit | Default | Increase |
|-------|---------|---------|
| Subscriptions | 100 | Yes |
| Resource Groups | 980 | Yes |
| Tags | 50 | No |
| Locks | 200 | No |

## COMPATIBILITY

### Tag Inheritance

| Resource | Inherits Tags |
|----------|-------------|
| RG tags | Yes |
| Resource | Optional |
| Children | No |

### Policy Scope

| Scope | Children Affected |
|-------|----------------|
| Management Group | All children |
| Subscription | All RG/resources |
| Resource Group | All resources |

## CROSS-REFERENCES

### Prerequisites

- Azure CLI basics
- Azure portal

### Next Steps

1. Automation with Biceps
2. Policy as Code
3. Cost management

## EXAM TIPS

### Production Patterns

- Use management groups
- Apply policies early
- Use locks on production
- Tag everything