---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Licensing
Purpose: Practical licensing implementation and optimization
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Licensing.md
RelatedFiles: 01_Basic_Azure_Licensing.md, 02_Advanced_Azure_Licensing.md
UseCase: License optimization, compliance management
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Practical licensing implementation helps organizations optimize costs while maintaining compliance, using the right licensing programs for each workload type and ensuring proper license tracking.

## 📖 WHAT

### Optimization Workflow

| Step | Action | Tool |
|------|--------|------|
| 1 | Inventory | Azure Migrate |
| 2 | Assess | Azure Advisor |
| 3 | Optimize | Hybrid Benefit |
| 4 | Monitor | Cost Management |

### Licensing Optimization Matrix

| Workload | Recommended | Alternative |
|----------|-------------|--------------|
| Windows VMs | Hybrid Benefit | Reserved |
| Linux VMs | Reserved | Savings Plan |
| SQL Server | Hybrid + RI | Pay-as-you-go |
| Dev/Test | Dev/Test pricing | MSDN credits |

## 🔧 HOW

### Example 1: Enable Windows Hybrid Benefit

```bash
# Create VM with hybrid benefit
az vm create \
    --name winvm \
    --resource-group myrg \
    --image Win2019Datacenter \
    --license-type Windows_Server \
    --size Standard_D2s_v3

# Enable on existing VM
az vm update \
    --name winvm \
    --resource-group myrg \
    --set licenseType='Windows_Server'
```

### Example 2: Verify SQL License Mobility

```bash
# Create SQL VM with license mobility
az vm create \
    --name sqlvm \
    --resource-group myrg \
    --image SQL2017EnterpriseWindows2019 \
    --license-type Windows_Server \
    --sku Enterprise

# Check existing
az vm show \
    --name sqlvm \
    --resource-group myrg \
    --query '[name, licenseType]'
```

### Example 3: Track License Compliance

```bash
# Export license report
# Portal: License Management > Compliance
# Export all licenses
# Check against purchased

# API usage
az billing list-licenses \
    --account-name xxx
```

## ⚠️ COMMON ISSUES

- **Missing SA**: Cannot use mobility
- **Expired licenses**: Check expiration
- **Not all VMs eligible**: Check eligibility

## 🏃 PERFORMANCE

- No performance impact

## 🌐 COMPATIBILITY

- Azure Arc for hybrid
- Azure Stack HCI

## 🔗 CROSS-REFERENCES

- **Azure Advisor**: Recommendations
- **Cost Management**: Tracking

## ✅ EXAM TIPS

- Enable hybrid on all Windows
- Track SQL licenses separately
- Review license compliance