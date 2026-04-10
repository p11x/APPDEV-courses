---
Category: Azure Certification
Subcategory: Azure Governance
Concept: Azure Role-Based Access Control
Purpose: Practical RBAC implementation patterns for real-world scenarios
Difficulty: intermediate
Prerequisites: 01_Basic_RBAC.md
RelatedFiles: 01_Basic_RBAC.md, 02_Advanced_RBAC.md
UseCase: Team access management, devOps pipelines, compliance access
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Practical RBAC patterns enable organizations to implement secure access management that balances security with operational efficiency, supporting DevOps teams, automated pipelines, and compliance requirements.

## 📖 WHAT

### Common Assignment Patterns

| Scenario | Role | Scope | Duration |
|----------|------|-------|----------|
| Dev Team | Contributor | Resource group | Permanent |
| Auditor | Reader | Subscription | Temporary |
| On-call Engineer | Custom | Subscription | JIT |
| Pipeline | Service Principal | Resource group | Permanent |

### Service Principal Patterns

| Pattern | Use Case | Authentication |
|---------|----------|---------------|
| Managed Identity | Azure resources | System-assigned |
| Service Principal | External tools | Client secret/cert |
| Workload Identity | AKS/Kubernetes | Federated token |

## 🔧 HOW

### Example 1: DevOps Pipeline Access

```bash
# Create service principal for pipeline
appId=$(az ad sp create-for-rbac \
    --name 'pipeline-sp' \
    --role Contributor \
    --scopes /subscriptions/xxx/resourceGroups/prod-rg \
    --query appId -o tsc)

# Get secrets (one time only)
az ad sp credential reset \
    --id $appId

# Pipeline uses AZURE_CLIENT_ID and AZURE_CLIENT_SECRET
```

### Example 2: Managed Identity for VM

```bash
# Enable managed identity on VM
az vm identity assign \
    --name myvm \
    --resource-group myrg \
    --system-assigned

# Grant access to key vault
az key vault set-policy \
    --name mykv \
    --object-id $(az vm show -n myvm -g myrg --query identity.principalId -o tsc) \
    --secret-permissions get list

# VM can now access KV without credentials
```

### Example 3: Break-Glass Emergency Access

```bash
# Create emergency access group
az ad group create \
    --display-name 'Emergency Access'

# Add break-glass accounts
userId=$(az ad user show --id breakglass@domain.com --query objectId -o tsc)
az ad group member add \
    --group 'Emergency Access' \
    --member-id $userId

# Assign owner temporarily
az role assignment create \
    --assignee $(az ad group show -g 'Emergency Access' --query objectId -o tsc) \
    --role Owner \
    --scope /subscriptions/xxx
```

## ⚠️ COMMON ISSUES

### Operational Issues

| Issue | Solution |
|-------|----------|
| Orphaned SPNs | Use managed identity |
| Expired secrets | Certificate auth |
| Over-permissioned | Custom roles |
| No audit trail | Enable logging |

### Security Hardening

- Enable MFA for all user accounts
- Use Privileged Identity Management
- Implement access reviews quarterly
- Monitor sign-in logs

## 🏃 PERFORMANCE

### Access Patterns

| Pattern | Performance | Scale |
|---------|-------------|-------|
| Direct assignment | Fast | <100 |
| Group-based | Fast | 100-1000 |
| RBAC to AD group | Slight delay | >1000 |

### Monitoring

```bash
# Get role assignment changes
az monitor activity-log list \
    --resource-group myrg \
    --query '[?contains(operationName.value, "roleAssignment")]'

# Enable audit logging
az monitor diagnostic-settings create \
    --name auditlogs \
    --resource /subscriptions/xxx \
    --logs '[{"category": "AuthorizationAuditLogs", "enabled": true}]'
```

## 🌐 COMPATIBILITY

### Service Integration

| Service | RBAC Support |
|---------|-------------|
| Azure DevOps | Service connections |
| GitHub Actions | Azure login action |
| Terraform | Azure provider |
| Ansible | Azure module |

## 🔗 CROSS-REFERENCES

- **Azure Key Vault**: Secrets and certificates
- **Azure Monitor**: Activity logging
- **Azure Sentinel**: Security analytics

## ✅ EXAM TIPS

- Use managed identity over service principals
- Document break-glass procedures
- Regular access reviews required
- Enable just-in-time for sensitive roles