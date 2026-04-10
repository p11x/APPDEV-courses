---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Licensing
Purpose: Advanced licensing decisions for enterprise optimization
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Licensing.md
RelatedFiles: 01_Basic_Azure_Licensing.md, 03_Practical_Azure_Licensing.md
UseCase: Enterprise licensing strategy, license mobility, compliance
CertificationExam: AZ-305 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Azure licensing strategies enable organizations to maximize savings through proper licensing programs, compliance management, and enterprise agreement optimization.

## 📖 WHAT

### Enterprise Agreement

| Element | Description |
|---------|-------------|
| Enrollment | Master agreement |
| Account | Billing account |
| Department | Cost center |
| Subscription | Services |

### License Mobility

| Product | Mobility | Requirements |
|---------|----------|--------------|
| SQL Server | Yes | SA required |
| Windows Server | Azure Hybrid | License + SA |
| Visual Studio | Azure credits | Active subscription |

### Licensing Comparison

| Option | Savings | Flexibility | Commitment |
|--------|---------|-------------|------------|
| Pay-as-you-go | 0% | Highest | None |
| Reserved Instance | 20-40% | Low | 1-3 years |
| Savings Plan | 20-35% | Medium | 1-3 years |
| Azure Hybrid | 40%+ | High | License |

## 🔧 HOW

### Example 1: EA Enrollment Management

```bash
# View EA enrollment
az billing enrollment-account show

# List departments
az billing department list \
    --enrollment-account-number xxx

# Move subscription to department
az billing subscription move \
    --destination-department-id xxx \
    --subscription-id xxx
```

### Example 2: License Mobility Verification

```bash
# Check existing licenses
az vm get-instance-view \
    --name myvm \
    --resource-group myrg \
    --query 'osProfile.licenseType'

# Apply Windows hybrid benefit
az vm update \
    --name myvm \
    --resource-group myrg \
    --set licenseType='Windows_Server'
```

### Example 3: Azure Credits Management

```bash
# Check credit balance
az account show \
    --query '[name, [account]'

# View credit usage
# Portal: Cost Management > Credits
```

## ⚠️ COMMON ISSUES

- **License disputes**: Keep license records
- **EA reconciliation**: Check invoices
- **Audit compliance**: License proof

## 🏃 PERFORMANCE

- No performance impact

## 🌐 COMPATIBILITY

- EA has full features
- CSP limited

## 🔗 CROSS-REFERENCES

- **Hybrid Benefit**: Windows/ SQL
- **Reservations**: Compute commitments

## ✅ EXAM TIPS

- Maximize license mobility
- Track SA expiration
- Use Azure credits for dev