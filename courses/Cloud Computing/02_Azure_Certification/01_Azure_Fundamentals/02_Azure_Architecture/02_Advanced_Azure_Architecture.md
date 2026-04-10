---
Category: Azure Certification
Subcategory: Azure Fundamentals
Concept: Azure Architecture - Advanced
Purpose: Advanced Azure architecture patterns including availability zones, load balancing, and disaster recovery
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Architecture.md
RelatedFiles: 01_Basic_Azure_Architecture.md, 03_Practical_Azure_Architecture.md
UseCase: Enterprise Azure deployment
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## WHY

Advanced Azure architecture enables high availability and disaster recovery for production workloads requiring 99.9%+ uptime.

### Enterprise Requirements

- **Availability**: Multi-zone deployments
- **Scalability**: Auto-scaling resources
- **Resilience**: Failover capability
- **DR**: Disaster recovery strategies

## WHAT

### Azure Availability Features

| Feature | Availability | Use Case |
|---------|-------------|---------|
| Availability Zones | 99.99% | Critical workloads |
| Availability Sets | 99.95% | VMs |
| Load Balancer | 99.99% | Traffic distribution |
| Traffic Manager | 99.99% | DNS routing |

### Cross-Platform HA Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| Zones | Yes | Yes | Yes |
| Sets | No | Yes | No |
| Load Balancer | Yes | Yes | Yes |
| DNS Failover | Yes | Yes | Yes |

### Azure HA Services

| Service | HA Mechanism | SLA |
|---------|-------------|-----|
| VM | Availability Set/Zone | 99.95%/99.99% |
| VMSS | Auto-scale | 99.95% |
| App Gateway | Multi-instance | 99.95% |
| SQL DB | Geo-replication | 99.99% |

## HOW

### Example 1: Availability Zone Deployment

```bash
# Create VM in availability zone
az vm create \
    --name vm-zone1 \
    --resource-group rg \
    --image UbuntuLTS \
    --zone 1

# Create zone-redundant VM scale set
az vmss create \
    --name vmss-ha \
    --resource-group rg \
    --image UbuntuLTS \
    --zones 1 2 3 \
    --instance-count 3 \
    --upgrade-mode Automatic

# Create public IP zone-redundant
az network public-ip create \
    --name pip-ha \
    --resource-group rg \
    --sku Standard \
    --zone 1 2 3
```

### Example 2: Load Balancer Configuration

```bash
# Create load balancer
az network lb create \
    --name app-lb \
    --resource-group rg \
    --sku Standard \
    --frontend-ip-name frontend \
    --backend-pool-name backend

# Create health probe
az network lb probe create \
    --lb-name app-lb \
    --name healthprobe \
    --protocol http \
    --port 80 \
    --path /health

# Create load balancing rule
az network lb rule create \
    --lb-name app-lb \
    --name rule1 \
    --protocol tcp \
    --frontend-port 80 \
    --backend-port 80 \
    --probe-name healthprobe \
    --backend-pool-name backend
```

### Example 3: Traffic Manager

```bash
# Create Traffic Manager profile
az network traffic-manager profile create \
    --name app-tm \
    --routing-method Performance \
    --unique-dns-name app-tm

# Create endpoints
az network traffic-manager endpoint create \
    --profile-name app-tm \
    --name primary \
    --type AzureEndpoints \
    --target-resource-id /subscriptions/id/resourceGroups/rg/providers/Microsoft.Network/trafficManagerProfiles/app-tm

# Endpoint for secondary region
az network traffic-manager endpoint create \
    --profile-name app-tm \
    --name secondary \
    --type AzureEndpoints \
    --target-resource-id /subscriptions/id/resourceGroups/rg2/providers/Microsoft.Network/trafficManagerProfiles/app-tm
```

### Example 4: Azure Backup Configuration

```bash
# Create Recovery Services vault
az backup vault create \
    --name backup-vault \
    --resource-group rg \
    --location eastus

# Enable Azure Backup
az backup protection enable-for-vm \
    --vault-name backup-vault \
    --vm-name vm-name \
    --policy-name DefaultPolicy

# Configure backup policy
az backup policy create \
    --vault-name backup-vault \
    --name daily-policy \
    --backup-management-type AzureIaasVM \
    --daily-retention 30
```

## COMMON ISSUES

### 1. Availability Zone Not Available

**Problem**: Zone not supported.

**Solution**:
```bash
# List available zones
az account list-locations --query "[].{Name:name,.Zones:physicalLocation}" -o table
```

### 2. LB Backend Not Responding

**Problem**: Health check failing.

**Solution**:
- Check health probe path
- Verify security groups
- Check instance health

### 3. Traffic Manager Not Failing Over

**Problem**: Endpoint not failing.

**Solution**:
- Check endpoint status
- Review routing method
- Verify health check

### 4. VM Not in Set/Zone

**Problem**: No HA capability.

**Solution**:
- Recreate with zone/set
- UseARM templates for deployment

### 5. Backup Not Working

**Problem**: Backup failing.

**Solution**:
- Check agent
- Verify network
- Review exclusion settings

## PERFORMANCE

### HA Performance Metrics

| Component | SLA | Typical Recovery |
|-----------|-----|-----------------|
| Availability Zone | 99.99% | < 1 minute |
| Availability Set | 99.95% | < 5 minutes |
| Load Balancer | 99.99% | Immediate |
| Traffic Manager | 99.99% | 30-60 seconds |

### Scale Limits

| Resource | Maximum | Burst |
|----------|---------|-------|
| VM per Set | 200 | Yes |
| VMSS instances | 1000 | Yes |
| LB backends | 100 | Yes |

## COMPATIBILITY

### Azure VM HA Options

| VM Type | HA Option | Notes |
|---------|-----------|-------|
| VMs | Availability Set | Legacy |
| VMs | Availability Zone | Current |
| VMSS | Automatic | Scale |
| VMs | Planned | Migration |

### Zone Redundancy

| Service | Zone Redundant |
|---------|-------------|
| Managed Disk | Yes |
| Public IP | Yes |
| Load Balancer | Yes |
| Storage | Yes |

## CROSS-REFERENCES

### Prerequisites

- Azure basic architecture
- Azure networking

### What to Study Next

1. Practical Azure Architecture
2. Disaster Recovery
3. Backup strategies

## EXAM TIPS

### Key Exam Facts

- Availability Zones = 99.99% SLA
- Availability Sets = 99.95% SLA
- Multi-AZ for VM = deployment across zones
- Traffic Manager for DNS failover

### Exam Questions

- **Question**: "3-nines availability" = Availability Set
- **Question**: "4-nines availability" = Availability Zone
- **Question**: "Global failover" = Traffic Manager
- **Question**: "Instant healing" = Load Balancer probe