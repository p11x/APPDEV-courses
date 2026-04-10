---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Load Balancer Advanced
Purpose: Advanced Azure Load Balancer configurations, HA, and optimization
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Load_Balancer.md
RelatedFiles: 01_Basic_Azure_Load_Balancer.md, 03_Practical_Azure_Load_Balancer.md
UseCase: Production load balancing with high availability
CertificationExam: Microsoft Azure Fundamentals - AZ-900
LastUpdated: 2025
---

## WHY

Advanced Load Balancer configurations enable production-grade high availability, disaster recovery, and performance optimization. Understanding these concepts is essential for enterprise deployments.

### Why Advanced Configuration Matters

- **Availability Zones**: Zone-redundant deployment
- **HA Ports**: Multiple port balancing
- **Floating IP**: Direct server return
- **Outbound NAT**: SNAT optimization
- **Session Persistence**: Client affinity

### Advanced Use Cases

- **Multi-tier Applications**: Web, app, data layers
- **Disaster Recovery**: Cross-region failover
- **SSL Offload**: Terminate at LB
- **Port Translation**: Complex NAT

## WHAT

### Availability Zones

```
    AVAILABILITY ZONES ARCHITECTURE
    ==============================

           Load Balancer
    ┌─────────────────────────────┐
    │      zonal-frontend        │
    └─────────────────────────────┘
              │     │     │
              ▼     ▼     ▼
         ┌────┐ ┌────┐ ┌────┐
         │VM 1│ │VM 2│ │VM 3│
         │ Z1 │ │ Z2 │ │ Z3 │
         └────┘ └────┘ └────┘

    Zone 1: Primary zone
    Zone 2: Secondary zone  
    Zone 3: Tertiary zone
```

### Distribution Modes

| Mode | Description | Use Case |
|------|-------------|----------|
| Default (5-tuple) | Source+dest IP/port | Load distribution |
| Source IP | Client IP only | IP persistence |
| Source IP + Protocol | IP + protocol | Better persistence |

### HA Ports

- Single rule for all TCP/UDP ports
- Automatically scales with backend
- No need for multiple rules
- Ideal for VM Scale Sets

## HOW

### Example 1: Availability Zones Deployment

```bash
# Create Standard SKU public IP with zone
az network public-ip create \
    --resource-group my-rg \
    --name my-zonal-pip \
    --sku Standard \
    --zone 1

# Create zone-redundant public IP
az network public-ip create \
    --resource-group my-rg \
    --name my-zone-redundant-pip \
    --sku Standard

# Create zonal LB frontend
az network lb create \
    --resource-group my-rg \
    --name my-zonal-lb \
    --sku Standard \
    --frontend-ip-name my-frontend \
    --public-ip-address my-zonal-pip \
    --availability-zone Zone1

# Create zone-redundant LB
az network lb create \
    --resource-group my-rg \
    --name my-zr-lb \
    --sku Standard \
    --frontend-ip-name my-frontend \
    --public-ip-address my-zone-redundant-pip

# Verify zones
az network lb show \
    --resource-group my-rg \
    --name my-zr-lb \
    --query frontendIPConfigurations
```

### Example 2: HA Ports Configuration

```bash
# Create HA ports rule
az network lb rule create \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-ha-ports-rule \
    --frontend-ip-name my-frontend \
    --backend-pool-name my-pool \
    --protocol all \
    --frontend-port 0 \
    --backend-port 0 \
    --enable-floating-ip

# This rule handles ALL ports (1-65535)
# Useful for:
# - VM Scale Sets
# - Application clusters
# - Any protocol

# View all rules
az network lb rule list \
    --resource-group my-rg \
    --lb-name my-lb \
    --output table
```

### Example 3: Configure Session Persistence

```bash
# Create rule with Source IP persistence
az network lb rule create \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-session-rule \
    --frontend-ip-name my-frontend \
    --backend-pool-name my-pool \
    --protocol tcp \
    --port 80 \
    --backend-port 80 \
    --load-distribution SourceIP

# Alternative: Source IP and protocol
az network lb rule create \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-session-rule-2 \
    --frontend-ip-name my-frontend \
    --backend-pool-name my-pool \
    --protocol all \
    --frontend-port 0 \
    --backend-port 0 \
    --load-distribution SourceIPProtocol
```

### Example 4: Outbound NAT Configuration

```bash
# Create outbound rule for SNAT
az network lb outbound-rule create \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-outbound-rule \
    --frontend-ip-configs my-frontend \
    --protocol tcp \
    --idle-timeout 15 \
    --outbound-ports 1024

# Create outbound IP pool
az network public-ip create \
    --resource-group my-rg \
    --name my-outbound-pip1 \
    --sku Standard

az network public-ip create \
    --resource-group my-rg \
    --name my-outbound-pip2 \
    --sku Standard

# Associate with outbound rule
az network lb frontend-ip update \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-frontend \
    --public-ip-addresses my-outbound-pip1 my-outbound-pip2

# Test outbound connection
# VMs behind LB can now make outbound connections
```

## COMMON ISSUES

### 1. SNAT Port Exhaustion

**Problem**: Outbound connections fail.

**Solution**:
```bash
# Increase outbound ports
az network lb outbound-rule update \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-outbound-rule \
    --outbound-ports 64000

# Use multiple public IPs
# Add more outbound IPs to reduce port contention

# Enable TCP keepalive
```

### 2. Zone Failure

**Problem**: Zone goes down, traffic stops.

**Solution**:
- Use zone-redundant LB
- Distribute VMs across zones
- Enable health checks with multiple zones
- Test failover regularly

### 3. Double NAT

**Problem**: Both frontend and backend have public IPs.

**Solution**:
- Use floating IP on LB
- Disable SNAT on rules
- Use instance-level public IPs

## PERFORMANCE

### Performance Metrics

| Metric | Value |
|--------|-------|
| Max backends | 1000 |
| Max rules | 2500 |
| Max frontends | 100 |
| Throughput | Up to 100 Gbps |

### Optimization

| Setting | Effect |
|---------|--------|
| Enable floating IP | Direct return |
| HA ports | Single rule |
| TCP keepalive | Connection reuse |
| Outbound rules | SNAT control |

## COMPATIBILITY

### Cross-Platform Comparison

| Feature | Azure LB | AWS ALB | GCP LB |
|---------|----------|---------|--------|
| Layer 4 | Yes | No | Yes |
| Layer 7 | No (AG) | Yes | Yes |
| Availability Zones | Yes | Yes | Yes |
| SSL termination | No (AG) | Yes | Yes |
| Outbound NAT | Yes | Yes | Yes |

### Integration

| Service | Integration |
|---------|-------------|
| VMSS | Auto-scaling |
| AKS | Load balancing |
| VPN | Gateway |

## CROSS-REFERENCES

### Related Services

- Application Gateway: Layer 7
- NAT Gateway: Outbound
- Traffic Manager: DNS routing
- VPN Gateway: Hybrid

### Prerequisites

- Basic Load Balancer
- VNet concepts
- NSG basics

### What to Study Next

1. Application Gateway: Layer 7
2. Traffic Manager: Global routing
3. Azure Firewall: Advanced networking

## EXAM TIPS

### Key Exam Facts

- HA ports: All ports balancing
- Source IP: Session persistence
- Floating IP: Direct server return
- Outbound NAT: SNAT for VMs

### Exam Questions

- **Question**: "All ports" = HA ports rule
- **Question**: "Session persistence" = Source IP mode
- **Question**: "Cross-zone" = Availability zones
