---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Compute
Purpose: Advanced Azure compute options including VM scale sets, availability zones, batching, and specialized workloads
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Compute.md
RelatedFiles: 01_Basic_Azure_Compute.md, 03_Practical_Azure_Compute.md
UseCase: Enterprise compute workloads, high availability, and performance optimization
CertificationExam: AZ-304 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Azure compute features enable enterprise-grade architectures with high availability, auto-scaling, and specialized workloads like GPU computing and batch processing.

## 📖 WHAT

### VM Scale Sets Advanced

| Feature | Description | Benefit |
|---------|-------------|---------|
| Automatic Scaling | Scale based on metrics | Cost optimization |
| Zone Redundancy | Cross-AZ deployment | High availability |
| Custom Script Extension | Post-provision scripts | Automation |
| Managed Identity | System-managed credentials | Security |

### Availability Options

- **Availability Sets**:  Update domains (5) + Fault domains (2-3)
- **Availability Zones**: Separate physical locations within region
- **Availability Sets + Zones**: Maximum redundancy

### Specialized Compute

| Type | Series | Workload |
|------|--------|----------|
| GPU | NC, ND, NV | ML, HPC |
| Memory Optimized | Esv3, M | In-memory DB |
| Storage Optimized | Ls | Big data |
| HPC | H | Batch processing |

### Azure Batch

- Job scheduling
- Task parallelization
- Multi-node compute
- Application packaging

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| VM Scale Sets | VM Scale Sets | Auto Scaling Groups | Instance Groups |
| GPU VMs | NC/ND/NV series | P/G instances | A100/TPU |
| Batch Processing | Azure Batch | AWS Batch | Cloud Batch |
| Container Service | AKS | EKS | GKE |
| Serverless Containers | Container Instances | Fargate | Cloud Run |

## 🔧 HOW

### Example 1: VM Scale Set with Auto-scaling

```bash
# Create VM scale set with autoscale
az vmss create \
    --name myvmss \
    --resource-group myrg \
    --image UbuntuLTS \
    --vm-sku Standard_DS2_v2 \
    --instance-count 2 \
    --upgrade-mode Automatic \
    --admin-username azureuser \
    --generate-ssh-keys

# Define autoscale rules
az monitor autoscale create \
    --name myvmss-autoscale \
    --resource-group myrg \
    --resource "/subscriptions/xxx/resourceGroups/myrg/providers/Microsoft.Compute/virtualMachineScaleSets/myvmss" \
    --min-count 2 \
    --max-count 10 \
    --count 2

# Add CPU-based scale-out rule
az monitor autoscale rule create \
    --name cpu-scale-out \
    --resource-group myrg \
    --autoscale-name myvmss-autoscale \
    --condition "Percentage CPU > 70" \
    --scale-out-increasing-by 1
```

### Example 2: Availability Zones Deployment

```bash
# Create public IP with zone
az network public-ip create \
    --name myip \
    --resource-group myrg \
    --location eastus \
    --zone 1 2 3

# Create load balancer with zone redundancy
az network lb create \
    --name mylb \
    --resource-group myrg \
    --location eastus \
    --sku Standard \
    --frontend-ip-name myfe \
    --backend-pool-name mypool

# Create VM in specific availability zone
az vm create \
    --name myvm \
    --resource-group myrg \
    --image UbuntuLTS \
    --zone 1 \
    --availability-set myavset
```

### Example 3: Azure Batch Configuration

```bash
# Create Batch account
az batch account create \
    --name mybatch \
    --resource-group myrg \
    --location eastus \
    --storage-account mystorage

# Create pool
az batch pool create \
    --pool-id mypool \
    --account-name mybatch \
    --resource-group myrg \
    --vm-size Standard_D2_v2 \
    --target-dedicated-nodes 2 \
    --image-reference publisher:Canonical offer:UbuntuServer sku:18.04-lts

# Create job
az batch job create \
    --job-id myjob \
    --account-name mybatch \
    --resource-group myrg \
    --pool-id mypool

# Add task
az batch task create \
    --task-id mytask \
    --job-id myjob \
    --account-name mybatch \
    --resource-group myrg \
    --command-line "echo hello"
```

## ⚠️ COMMON ISSUES

### VM Scale Set Issues

- **Upgrade policy mismatch**: Choose Manual, Automatic, or Rolling
- **Capacity limits**: Subscription quotas restrict scale
- **Health extension failures**: Ensure custom script is valid
- **Load balancer backend ports**: Must match across instances

### Availability Issues

- **Zone not supported**: Not all regions support zones
- **Cross-zone costs**: Zone-redundant services cost more
- **Zonal deployments**: Explicit zone = single point of failure

### Azure Batch Issues

- **Pool allocation**: Long wait times for dedicated nodes
- **Task failures**: Check exit codes and stdout
- **Quota limits**: Increase account quotas proactively

## 🏃 PERFORMANCE

### VM Performance Optimization

| Action | Impact |
|--------|--------|
| Accelerated Networking | 2-3x throughput |
| Premium SSD | 50K IOPS vs 500 |
| Proper VM sizing | Cost/performance balance |
| Deploy in proximity | Minimize latency |

### Auto-scaling Best Practices

- Set appropriate cooldown periods (5-15 minutes)
- Use multiple metrics, not just CPU
- Configure scale-in policies to avoid thrashing
- Test scaling regularly with expected loads

### Batch Performance

- Use smaller, more tasks (not few large tasks)
- Leverage auto-pool for burst workloads
- Use low-priority VMs for cost savings
- Parallel task execution maximizes throughput

## 🌐 COMPATIBILITY

### OS Support

| OS | Full Support | Extended Support |
|----|-------------|----------------|
| Windows Server 2022 | Yes | 2026+ |
| Windows Server 2019 | Yes | 2025 |
| Ubuntu 18.04/20.04 LTS | Yes | 2025+ |
| RHEL 7.9, 8.x | Yes | 2024+ |
| CentOS 7.9, 8 | Yes | No |

### SDK Compatibility

- .NET, Python, Node.js, Java, Go
- REST API for all operations
- Azure CLI and PowerShell support

### Region Support

- Most compute options available in all regions
- Some GPU SKUs limited to specific regions
- Batch has limited region support

## 🔗 CROSS-REFERENCES

### Related Services

- **Azure Storage**: For VM disks and data
- **Azure Networking**: For connectivity
- **Azure Monitor**: For metrics and alerts
- **Azure Backup**: For VM recovery

### Related Concepts

- **ARM Templates**: IaC for compute resources
- **VM Extensions**: Configuration automation
- **Azure Site Recovery**: Disaster recovery

## ✅ EXAM TIPS

### Key Differences

- **Availability Sets**: Protects against hardware faults within DC
- **Availability Zones**: Protects against entire DC failure
- **VMSS**: Auto-scaling + high availability combined

### Cost Optimization

- Use VMSS with auto-scale
- Spot VMs for fault-tolerant workloads
- Reserved Instances for predictable loads

### Enterprise Patterns

- Standard Load Balancer for zone-redundant
- Application Gateway for layer 7
- Azure Batch for parallel processing