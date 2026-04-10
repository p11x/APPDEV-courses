---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Hybrid Benefit
Purpose: Use existing licenses for Azure cost savings
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_Azure_Hybrid_Benefit.md, 03_Practical_Azure_Hybrid_Benefit.md
UseCase: Cost savings with existing Windows/SQL licenses
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure Hybrid Benefit allows you to use existing Windows Server and SQL Server licenses with Software Assurance to save up to 40% on Azure virtual machines, making cloud migration more cost-effective.

## 📖 WHAT

### Hybrid Benefit Types

| License | Savings | Requirement |
|---------|---------|-------------|
| Windows Server | 40% | SA |
| SQL Server | 40%+ | SA |
| Windows Client | VDA | Modern license |

### Eligible Resources

| Resource | Discount |
|----------|----------|
| VMs (Windows) | ~40% |
| VM Scale Sets | ~40% |
| SQL Database | 40%+ |
| SQL Managed Instance | 40%+ |

### How It Works

```
Standard VM: $X/hour
Hybrid VM: $X - (license cost) = Y/hour
Savings: ~40%
```

## 🔧 HOW

### Example 1: Verify Benefit Availability

```bash
# Check if VM can use hybrid
# Portal: VM > Configuration > License

# CLI not available
# Via Azure Portal only
```

### Example 2: Enable on New VM

```bash
# Create VM with hybrid benefit
az vm create \
    --name winhvm \
    --resource-group myrg \
    --image Win2019Datacenter \
    --license-type Windows_Server \
    --size Standard_D2s_v3

# Creates VM at discounted rate
```

### Example 3: Enable on Existing VM

```bash
# Enable hybrid benefit
az vm update \
    --name winhvm \
    --resource-group myrg \
    --set licenseType='Windows_Server'

# Verify
az vm get-instance-view \
    --name winhvm \
    --resource-group myrg \
    --query 'licenseType'
```

## ⚠️ COMMON ISSUES

- **SA required**: Must have active SA
- **Not all images**: Windows images only
- **Region restrictions**: Not all regions

## 🏃 PERFORMANCE

- No performance impact
- Same VM performance

## 🌐 COMPATIBILITY

| Service | Support |
|----------|----------|
| VM | Yes |
| VMSS | Yes |
| SQL | Yes |
| Azure Stack | Limited |

## 🔗 CROSS-REFERENCES

- **Reservations**: Combined savings
- **Licensing**: License mobility

## ✅ EXAM TIPS

- Use hybrid ON all Windows VMs
- Requires Software Assurance
- Check eligibility before