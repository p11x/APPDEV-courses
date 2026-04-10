---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Policy
Purpose: Hands-on policy implementation for real-world compliance scenarios
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Policy.md
RelatedFiles: 01_Basic_Azure_Policy.md, 02_Advanced_Azure_Policy.md
UseCase: Enforce enterprise compliance, audit security, automate governance
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Practical Azure Policy implementation enables organizations to automate compliance, reduce manual review overhead, and maintain consistent governance across all resources deployed in Azure.

## 📖 WHAT

### Common Compliance Scenarios

| Scenario | Policy Approach | Expected Outcome |
|----------|-----------------|------------------|
| Tag enforcement | Append on creation | Consistent taxonomy |
| Storage security | Audit then Deny | Secure by default |
| Network restrictions | Deny with exemptions | Zero-trust network |
| Cost management | Deny oversized | Optimize spend |
| Regulatory | Audit + Remediation | Continuous compliance |

### Policy Patterns

| Pattern | Use Case | Approach |
|---------|----------|----------|
| Check-then-create | Existing resources | Audit effect |
| Prevent-then-approve | Restricted resources | Deny + exemption |
| Auto-tag | Missing metadata | Append effect |
| Deploy-config | Required resources | DeployIfNotExists |

## 🔧 HOW

### Example 1: Enforce Storage Encryption

```bash
# Create policy to audit unencrypted storage
az policy definition create \
    --name 'audit-storage-encryption' \
    --display-name 'Audit Unencrypted Storage' \
    --description 'Audit if Storage uses encryption' \
    --mode Indexed \
    --rules '{
        "if": {
            "allOf": [
                {
                    "field": "type",
                    "equals": "Microsoft.Storage/storageAccounts"
                },
                {
                    "not": {
                        "field": "Microsoft.Storage/storageAccounts/enableBlobEncryption",
                        "equals": "true"
                    }
                }
            ]
        },
        "then": {
            "effect": "Audit"
        }
    }'

# Assign to subscription
az policy assignment create \
    --name 'audit-storage-encryption' \
    --policy 'audit-storage-encryption' \
    --scope '/subscriptions/xxx'
```

### Example 2: Virtual Network Requirements

```bash
# Create policy requiring vnet
az policy definition create \
    --name 'require-vnet' \
    --display-name 'Require Virtual Network' \
    --description 'Ensures VMs are in VNets' \
    --mode Indexed \
    --rules '{
        "if": {
            "allOf": [
                {
                    "field": "type",
                    "in": ["Microsoft.Compute/virtualMachines", "Microsoft.Compute/virtualMachineScaleSets"]
                },
                {
                    "not": {
                        "field": "Microsoft.Compute/virtualMachines/networkInterfaceConfigurations[*].ipConfigurations[*].subnet.id",
                        "exists": true
                    }
                }
            ]
        },
        "then": {
            "effect": "Deny"
        }
    }'

# Assign with scope
az policy assignment create \
    --name 'require-vnet' \
    --policy 'require-vnet' \
    --resource-group production-rg
```

### Example 3: Cost Tagging Policy

```bash
# Create cost center policy
az policy definition create \
    --name 'require-costcenter' \
    --display-name 'Require Cost Center Tag' \
    --description 'Mandates CostCenter tag for billing' \
    --mode All \
    --rules '{
        "if": {
            "allOf": [
                {
                    "field": "type",
                    "in": [
                        "Microsoft.Compute/virtualMachines",
                        "Microsoft.Storage/storageAccounts",
                        "Microsoft.Sql/servers/databases"
                    ]
                },
                {
                    "field": "tags[CostCenter]",
                    "exists": false
                }
            ]
        },
        "then": {
            "effect": "Deny"
        }
    }'

# Assign and set parameters
az policy assignment create \
    --name 'require-costcenter' \
    --policy 'require-costcenter' \
    --subscription xxx \
    --params '{
        "CostCenter": {"value": "REQUIRED"}
    }'
```

## ⚠️ COMMON ISSUES

### Operational Issues

| Issue | Solution |
|-------|----------|
| Locked resources | Use exemption or lock removal |
| False positives | Refine condition logic |
| Compliance drift | Regular remediation runs |
| Notification delays | Enable alerts |

### Troubleshooting Steps

1. Check policy assignment scope
2. Verify effect type (Audit vs Deny)
3. Review compliance state in portal
4. Test policy rule in what-if mode
5. Check exemption list

## 🏃 PERFORMANCE

### Compliance Automation

| Automation | Frequency | Scope |
|------------|-----------|-------|
| On-change | Real-time | Individual |
| Scheduled | Daily | Subscription |
| On-demand | Manual | Management group |

### Workflow Integration

- **CI/CD**: Policy check in deployment pipeline
- **Azure DevOps**: Policy as code in ARM/Terraform
- **ServiceNow**: Compliance tickets

## 🌐 COMPATIBILITY

### Policy SDK Support

| SDK | Policy Operations |
|-----|-------------------|
| Azure CLI | Full support |
| PowerShell | Full support |
| Python SDK | Full support |
| REST API | Full support |
| Terraform | Azure Provider |

## 🔗 CROSS-REFERENCES

- **ARM Templates**: Policy in template resources
- **Azure Cost Management**: Tag-based cost attribution
- **Azure Security Center**: Regulatory compliance

## ✅ EXAM TIPS

- Start with Audit, move to Deny after testing
- Use What-If to preview policy impact
- Document exemptions for audit trails
- Integrate policy in CI/CD pipeline