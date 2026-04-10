---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Architecture - Practical
Purpose: Production HA deployment with availability zones, load balancing, and backup
Difficulty: practical
Prerequisites: 01_Basic_Azure_Architecture.md, 02_Advanced_Azure_Architecture.md
RelatedFiles: 01_Basic_Azure_Architecture.md, 02_Advanced_Azure_Architecture.md
UseCase: Production Azure HA implementation
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## WHY

Production HA implementation ensures applications remain available despite failures.

## WHAT

### Production HA Architecture

```
Azure HA Architecture
====================

                 ┌───────────────────┐
                 │   Traffic Manager │ (Global)
                 └─────────┬─────────┘
                          │
         ┌────────────────┼────────────────┐
         │                │                │
    ┌────▼────┐     ┌────▼────┐     ┌────▼────┐
    │ App GW  │     │ App GW  │     │ App GW  │
    │ EastUS │     │ EastUS2│     │ WestUS │
    └────┬────┘     └────┬────┘     └────┬────┘
         │                │                │
    ┌────▼────┐     ┌────▼────┐     ┌────▼────┐
    │ LB     │     │ LB     │     │ LB     │
    │ Zone   │     │ Zone   │     │ Zone   │
    └────┬────┘     └────┬────┘     └────┬────┘
         │                │                │
    ┌────▼──────────────▼─────────────────▼────┐
    │         VM Scale Set (3 zones)           │
    │         └──┬──┬──┘                      │
    │        Zones 1, 2, 3                    │
    └───────────────────────────────────────────┘
```

## HOW

### Lab 1: Multi-Zone VM Scale Set

```bash
# Create zone-redundant scale set
az vmss create \
    --name ha-vmss \
    --resource-group rg \
    --image UbuntuLTS \
    --zones 1 2 3 \
    --instance-count 3 \
    --upgrade-mode Automatic \
    --admin-username azureuser \
    --generate-ssh-keys

# Configure health extension
az vmss extension set \
    --vmss-name ha-vmss \
    --name healthExtension \
    --publisher Microsoft.ManagedService \
    --type Configuration \
    --version 1.0

# Update instance count
az vmss update \
    --name ha-vmss \
    --resource-group rg \
    --set sku.capacity=5
```

### Lab 2: Application Gateway HA

```bash
# Create public IP
az network public-ip create \
    --name app-gw-pip \
    --resource-group rg \
    --sku Standard \
    --zone 1 2 3

# Create application gateway
az network application-gateway create \
    --name app-gw \
    --resource-group rg \
    --sku Standard_v2 \
    --waf-configuration_ENABLED \
    --front-end-pip app-gw-pip

# Add backend pool
az network application-gateway address-pool update \
    --gateway-name app-gw \
    --name pool1 \
    --resource-group rg \
    --servers 10.0.1.4 10.0.2.4

# Configure health probe
az network application-gateway probe create \
    --gateway-name app-gw \
    --name healthprobe \
    --resource-group rg \
    --protocol http \
    --path /health
```

### Lab 3: SQL Database HA

```bash
# Create with zone redundancy
az sql db create \
    --name app-db \
    --resource-group rg \
    --server appserver \
    --edition GeneralPurpose \
    --zone-redundant True \
    --ha-enabled True \
    --backup-retention 30

# Configure active geo-replication
az sql dbReplica create \
    --name app-db-replica \
    --resource-group rg \
    --server appserver \
    --source-server appserver \
    --location eastus2

# Failover command
az sql dbReplica set-primary \
    --name app-db-replica \
    --resource-group rg \
    --server appserver \
    --allow-data-loss True
```

### Lab 4: Backup Configuration

```bash
# Enable backup on VM
az backup protection enable-for-vm \
    --vault-name backup-vault \
    --vm-name production-vm \
    --policy-name EnhancedPolicy

# Configure backup schedule
az backup policy create \
    --vault-name backup-vault \
    --name daily-policy \
    --backup-management-type AzureIaasVM \
    --daily-retention-days 30 \
    --weekly-retention-weeks 4 \
    --yearly-retention-year 1

# Restore VM
az backup recovery show-logical-chain \
    --vault-name backup-vault \
    --restore-mode OriginalLocation \
    --container-name production-vm \
    --item-name production-vm
```

## COMMON ISSUES

### 1. VMSS Not Scaling

**Problem**: No automatic scale.

**Solution**:
```bash
# Configure autoscale
az monitor autoscale create \
    --name vmss-autoscale \
    --resource-group rg \
    --resource vmss-ha \
    --min-count 2 \
    --max-count 10

# Add rule
az monitor autoscale rule create \
    --name scaleout \
    --resource-group rg \
    --autoscale-name vmss-autoscale \
    --condition "Percentage CPU > 80 avg 5m" \
    --scale-action type IncreaseCountBy 2 duration PT5M
```

### 2. Gateway SSL Issues

**Problem**: SSL errors.

**Solution**:
- Upload certificate to Key Vault
- Configure Key Vault integration
- Validate certificate format

### 3. Backup Not Completing

**Problem**: Job fails.

**Solution**:
- Verify network connectivity
- Check storage account
- Review exclusions

## PERFORMANCE

### HA Performance

| Component | RTO | RPO |
|-----------|-----|-----|
| VMSS | 2-5 min | N/A |
| App Gateway | Immediate | N/A |
| SQL DB | < 30 sec | < 1 sec |
| Backup | Minutes | User-defined |

### Performance Benchmarks

| Operation | Average |
|-----------|---------|
| Zone creation | 10-15 seconds |
| Gateway setup | 3-5 minutes |
| DB failover | < 30 seconds |

## COMPATIBILITY

### Zone Support by Region

| Region | Zones | SLA |
|--------|------|-----|
| East US | 3 | 99.99% |
| West US 2 | 3 | 99.99% |
| Central US | 3 | 99.99% |

### Services by Zone

| Service | Zone Support |
|---------|-------------|
| VM | Yes |
| VMSS | Yes |
| Disk | Yes |
| Public IP | Yes |

## CROSS-REFERENCES

### Prerequisites

- Azure architecture basics
- Azure CLI

### Next Steps

1. Disaster Recovery testing
2. Backup automation
3. Monitoring

## EXAM TIPS

### Production Patterns

- Multi-zone for production
- Use ARM templates
- Test failover regularly
- Monitor health metrics