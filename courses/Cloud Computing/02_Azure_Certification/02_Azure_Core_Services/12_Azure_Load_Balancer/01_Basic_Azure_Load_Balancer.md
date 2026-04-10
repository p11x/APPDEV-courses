---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Load Balancer
Purpose: Understanding Azure Load Balancer for network traffic distribution
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Azure_Load_Balancer.md, 03_Practical_Azure_Load_Balancer.md
UseCase: Distributing traffic across Azure resources
CertificationExam: Microsoft Azure Fundamentals - AZ-900
LastUpdated: 2025
---

## WHY

Azure Load Balancer is a layer 4 (TCP/UDP) load balancer that distributes incoming traffic across healthy virtual machines. Understanding Load Balancer is essential for building highly available applications.

### Why Load Balancer Matters

- **High Availability**: Distribute traffic across instances
- **Scalability**: Add/remove instances seamlessly
- **Health Checks**: Automatic instance monitoring
- **Low Latency**: Layer 4 forwarding
- **Automatic Failover**: Route around failed instances
- **SSL Termination**: Offload SSL processing

### Industry Statistics

- Supports millions of requests per second
- 99.99% availability SLA
- Up to 100 Gbps throughput
- Built-in DDoS protection

### When NOT to Use Load Balancer

- HTTP/HTTPS traffic: Use Application Gateway
- Global routing: Use Traffic Manager
- SSL offload with advanced features: Use Application Gateway
- DNS-based routing: Use Traffic Manager

## WHAT

### Load Balancer Types

| Type | Layer | Use Case |
|------|-------|----------|
| Public | Public IP | Internet-facing |
| Internal | Private IP | Internal apps |

### SKU Comparison

| Feature | Basic | Standard |
|---------|-------|----------|
| Backend pools | 100 VMs | 1000 VMs |
| Health probes | Limited | Advanced |
| SLA | 99.9% | 99.99% |
| Availability Zones | No | Yes |
| HTTPS | No | Yes |
| DDoS | Basic | Standard |

### Architecture Diagram

```
                AZURE LOAD BALANCER
                ===================

    ┌──────────────┐
    │  Internet   │
    └──────┬───────┘
           │
           ▼
    ┌────────────────────────────────┐
    │     PUBLIC IP (Frontend)      │
    │      52.x.x.x                 │
    └────────────────────────────────┘
           │
           ▼
    ┌────────────────────────────────┐
    │    LOAD BALANCER RULES        │
    │    Port 80 -> Pool:80         │
    │    Port 443 -> Pool:443       │
    └────────────────────────────────┘
           │
    ┌───────┴───────┐
    │               │
    ▼               ▼
┌────────┐    ┌────────┐
│ Health │    │ Health │
│ Probe  │    │ Probe  │
└────────┘    └────────┘
    │               │
    ▼               ▼
┌────────┐    ┌────────┐
│  VM 1  │    │  VM 2  │
│ Zone 1 │    │ Zone 2 │
└────────┘    └────────┘
    │               │
    └───────┬───────┘
            │
    ┌───────▼───────┐
    │  Backend Pool│
    └──────────────┘
```

### Key Components

| Component | Description |
|-----------|-------------|
| Frontend IP | Public/private IP |
| Backend Pool | VM collection |
| Health Probe | Instance health check |
| LB Rule | Traffic distribution rule |
| NAT Rule | Port forwarding |

## HOW

### Example 1: Create Public Load Balancer

```bash
# Create resource group
az group create --name my-rg --location eastus

# Create public IP
az network public-ip create \
    --resource-group my-rg \
    --name my-pip \
    --sku Standard

# Create load balancer
az network lb create \
    --resource-group my-rg \
    --name my-lb \
    --sku Standard \
    --public-ip-address my-pip \
    --frontend-ip-name my-frontend \
    --backend-pool-name my-pool

# Create health probe
az network lb probe create \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-healthprobe \
    --protocol tcp \
    --port 80

# Create LB rule
az network lb rule create \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-rule \
    --frontend-ip-name my-frontend \
    --backend-pool-name my-pool \
    --protocol tcp \
    --port 80 \
    --backend-port 80
```

### Example 2: Add VMs to Backend Pool

```bash
# Create NSG and rule
az network nsg create \
    --resource-group my-rg \
    --name my-nsg

az network nsg rule create \
    --resource-group my-rg \
    --nsg-name my-nsg \
    --name allow-http \
    --protocol tcp \
    --direction inbound \
    --priority 1000 \
    --source-address-prefix Internet \
    --source-port-range '*' \
    --destination-address-prefix '*' \
    --destination-port-range 80

# Create VNet
az network vnet create \
    --resource-group my-rg \
    --name my-vnet \
    --address-prefix 10.0.0.0/16

# Create subnet
az network vnet subnet create \
    --resource-group my-rg \
    --vnet-name my-vnet \
    --name my-subnet \
    --address-prefix 10.0.0.0/24

# Create NIC for VM1
az network nic create \
    --resource-group my-rg \
    --name my-nic1 \
    --vnet-name my-vnet \
    --subnet my-subnet \
    --network-security-group my-nsg

# Create NIC for VM2
az network nic create \
    --resource-group my-rg \
    --name my-nic2 \
    --vnet-name my-vnet \
    --subnet my-subnet \
    --network-security-group my-nsg

# Add NICs to backend pool
az network nic ip-config address-pool add \
    --resource-group my-rg \
    --nic-name my-nic1 \
    --ip-config-name ipconfig1 \
    --lb-name my-lb \
    --backend-pool-name my-pool

az network nic ip-config address-pool add \
    --resource-group my-rg \
    --nic-name my-nic2 \
    --ip-config-name ipconfig1 \
    --lb-name my-lb \
    --backend-pool-name my-pool
```

### Example 3: Create Internal Load Balancer

```bash
# Create VNet for internal LB
az network vnet create \
    --resource-group my-rg \
    --name my-internal-vnet \
    --address-prefix 10.1.0.0/16

az network vnet subnet create \
    --resource-group my-rg \
    --vnet-name my-internal-vnet \
    --name my-internal-subnet \
    --address-prefix 10.1.0.0/24

# Create internal LB
az network lb create \
    --resource-group my-rg \
    --name my-internal-lb \
    --sku Standard \
    --vnet-name my-internal-vnet \
    --subnet my-internal-subnet \
    --frontend-ip-name my-internal-frontend \
    --backend-pool-name my-internal-pool

# Create health probe
az network lb probe create \
    --resource-group my-rg \
    --lb-name my-internal-lb \
    --name my-internal-probe \
    --protocol http \
    --port 80 \
    --path /

# Create rule
az network lb rule create \
    --resource-group my-rg \
    --lb-name my-internal-lb \
    --name my-internal-rule \
    --frontend-ip-name my-internal-frontend \
    --backend-pool-name my-internal-pool \
    --protocol tcp \
    --port 80 \
    --backend-port 80

# Access internal LB at: 10.1.0.100
```

### Example 4: Configure Port Forwarding

```bash
# Create NAT rule for SSH
az network lb inbound-nat-rule create \
    --resource-group my-rg \
    --lb-name my-lb \
    --name ssh-nat-rule \
    --frontend-ip-name my-frontend \
    --protocol tcp \
    --frontend-port 2222 \
    --backend-port 22

# Associate with VM NIC
az network nic ip-config inbound-nat-rule add \
    --resource-group my-rg \
    --nic-name my-nic1 \
    --ip-config-name ipconfig1 \
    --inbound-nat-rule ssh-nat-rule

# SSH to VM via LB
# ssh -p 2222 user@52.x.x.x
```

## COMMON ISSUES

### 1. Health Probe Failing

**Problem**: VMs marked unhealthy.

**Solution**:
```bash
# Check health probe status
az network lb probe show \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-healthprobe

# Update probe settings
az network lb probe update \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-healthprobe \
    --interval 5 \
    --probe-threshold 3
```

### 2. Traffic Not Distributed

**Problem**: All traffic to one VM.

**Solution**:
- Check distribution mode (Source IP)
- Verify health probes
- Check connection limits

### 3. High Latency

**Problem**: Slow response times.

**Solution**:
- Deploy across Availability Zones
- Enable health probes with fast detection
- Use Standard SKU for better performance

## PERFORMANCE

### Performance Characteristics

| Metric | Standard | Basic |
|--------|----------|-------|
| Max throughput | 100 Gbps | 10 Gbps |
| Max connections | 100K | 10K |
| Max rules | 2500 | 250 |
| Idle timeout | 30 min | 4 min |

### Cost Optimization

| Strategy | Savings |
|----------|---------|
| Basic SKU | Free (with VMs) |
| Right-size rules | Reduce complexity |
| HA ports | Single rule for all |

## COMPATIBILITY

### Region Availability

- All Azure regions
- Sovereign clouds available

### Integration

| Service | Use Case |
|---------|----------|
| VM Scale Sets | Auto-scaling |
| App Service | Web apps |
| AKS | Kubernetes |
| Virtual WAN | Branch connectivity |

## CROSS-REFERENCES

### Related Services

- Application Gateway: Layer 7 load balancing
- Traffic Manager: DNS-based routing
- NAT Gateway: Outbound NAT
- Network Security Groups: Traffic filtering

### Alternatives

| Need | Use |
|------|-----|
| HTTP/HTTPS | Application Gateway |
| Global routing | Traffic Manager |
| SSL termination | Application Gateway |
| DNS-based | Traffic Manager |

### What to Study Next

1. Advanced Load Balancer: HA, zones
2. Application Gateway: Layer 7
3. Traffic Manager: Global routing

## EXAM TIPS

### Key Exam Facts

- Load Balancer: Layer 4 (TCP/UDP)
- Public LB: Internet-facing
- Internal LB: Private IP
- Health probe: Monitors backend
- NAT: Port forwarding

### Exam Questions

- **Question**: "Layer 4 balancing" = Load Balancer
- **Question**: "HTTP traffic" = Application Gateway
- **Question**: "SSL offload" = Application Gateway
