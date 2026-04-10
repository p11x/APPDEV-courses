---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Load Balancer Practical
Purpose: Practical Azure Load Balancer implementation and best practices
Difficulty: practical
Prerequisites: 01_Basic_Azure_Load_Balancer.md, 02_Advanced_Azure_Load_Balancer.md
RelatedFiles: 01_Basic_Azure_Load_Balancer.md, 02_Advanced_Azure_Load_Balancer.md
UseCase: Production load balancer implementation
CertificationExam: Microsoft Azure Fundamentals - AZ-900
LastUpdated: 2025
---

## WHY

Practical Load Balancer implementation involves building production-ready configurations with health monitoring, auto-scaling, and operational best practices. This knowledge is essential for deploying enterprise applications.

### Why Practical Implementation Matters

- **Reliability**: Production-grade setup
- **Observability**: Health monitoring
- **Scalability**: Auto-scaling support
- **Security**: Proper NSG rules
- **Automation**: Repeatable deployments

### Common Production Use Cases

- **Web Applications**: Multi-tier architecture
- **Microservices**: Container balancing
- **Database**: Read replicas
- **API**: REST endpoints

## WHAT

### Architecture Patterns

| Pattern | Description | Components |
|---------|-------------|------------|
| Web tier | HTTP balancing | Public LB + VMs |
| App tier | Internal LB | Private LB + VMs |
| Data tier | DB balancing | Internal LB + DB |

### Best Practices

1. **Health probes**: Configure proper intervals
2. **Availability Zones**: Deploy across zones
3. **NSG**: Allow LB traffic
4. **Monitoring**: Track metrics

## HOW

### Example 1: Multi-tier Application

```bash
# Create NSG for web tier
az network nsg create \
    --resource-group my-rg \
    --name web-nsg

# Allow HTTP/HTTPS from LB
az network nsg rule create \
    --resource-group my-rg \
    --nsg-name web-nsg \
    --name allow-lb \
    --protocol tcp \
    --direction inbound \
    --priority 100 \
    --source-address-prefix AzureLoadBalancer \
    --source-port-range '*' \
    --destination-address-prefix '*' \
    --destination-port-range 80,443

# Allow SSH from bastion
az network nsg rule create \
    --resource-group my-rg \
    --nsg-name web-nsg \
    --name allow-ssh \
    --protocol tcp \
    --direction inbound \
    --priority 200 \
    --source-address-prefix 10.0.1.0/24 \
    --source-port-range '*' \
    --destination-address-prefix '*' \
    --destination-port-range 22

# Create web tier LB
az network lb create \
    --resource-group my-rg \
    --name web-lb \
    --sku Standard \
    --public-ip-address my-web-pip

# Add rules for HTTP/HTTPS
az network lb rule create \
    --resource-group my-rg \
    --lb-name web-lb \
    --name http-rule \
    --frontend-ip-name my-frontend \
    --backend-pool-name web-pool \
    --protocol tcp \
    --port 80 \
    --backend-port 80

az network lb rule create \
    --resource-group my-rg \
    --lb-name web-lb \
    --name https-rule \
    --frontend-ip-name my-frontend \
    --backend-pool-name web-pool \
    --protocol tcp \
    --port 443 \
    --backend-port 443

# Create app tier LB (internal)
az network lb create \
    --resource-group my-rg \
    --name app-lb \
    --sku Standard \
    --vnet-name app-vnet \
    --subnet app-subnet

# Create health probe for app tier
az network lb probe create \
    --resource-group my-rg \
    --lb-name app-lb \
    --name app-probe \
    --protocol http \
    --port 8080 \
    --path /health
```

### Example 2: VM Scale Sets Integration

```bash
# Create VMSS with LB
az vmss create \
    --resource-group my-rg \
    --name my-vmss \
    --image UbuntuLTS \
    --lb my-lb \
    --backend-pool-name my-pool \
    --admin-username azureuser \
    --generate-ssh-keys

# Configure VMSS for auto-scaling
az monitor autoscale create \
    --resource-group my-rg \
    --resource my-vmss \
    --resource-type Microsoft.Compute/virtualMachineScaleSets \
    --name my-autoscale \
    --min-count 2 \
    --max-count 10 \
    --cpu-threshold 70

# Add scaling rule
az monitor autoscale rule create \
    --resource-group my-rg \
    --autoscale-name my-autoscale \
    --condition "Percentage CPU > 70 avg 5m" \
    --scale out 1

az monitor autoscale rule create \
    --resource-group my-rg \
    --autoscale-name my-autoscale \
    --condition "Percentage CPU < 30 avg 5m" \
    --scale in 1
```

### Example 3: Monitoring and Alerting

```bash
# Create Log Analytics workspace
az monitor log-analytics workspace create \
    --resource-group my-rg \
    --name my-workspace

# Enable diagnostics for LB
az monitor diagnostic-settings create \
    --name lb-diagnostics \
    --resource /subscriptions/sub-id/resourceGroups/my-rg/providers/Microsoft.Network/loadBalancers/my-lb \
    --workspace /subscriptions/sub-id/resourceGroups/my-rg/providers/Microsoft.OperationalInsights/workspaces/my-workspace \
    --logs '[
        {"category": "LoadBalancerAlertEvent", "enabled": true},
        {"category": "LoadBalancerProbeHealthStatus", "enabled": true}
    ]' \
    --metrics '[
        {"category": "AllMetrics", "enabled": true}
    ]'

# Create alert for high CPU
az monitor metrics alert create \
    --name high-cpu-alert \
    --resource-group my-rg \
    --condition "CpuPercentage > 80" \
    --description "High CPU on backend VMs" \
    --action-group my-action-group

# Query health status
# In Log Analytics:
# AzureDiagnostics
# | where ResourceType == "LOADBALANCERS"
# | where Category == "LoadBalancerProbeHealthStatus"
# | project TimeGenerated, BackendIPAddress, BackendPort, Health
```

### Example 4: Disaster Recovery Setup

```bash
# Create secondary region LB (DR)
az network lb create \
    --resource-group dr-rg \
    --location westus2 \
    --name dr-lb \
    --sku Standard \
    --public-ip-address dr-pip

# Configure identical rules
az network lb rule create \
    --resource-group dr-rg \
    --lb-name dr-lb \
    --name http-rule \
    --frontend-ip-name my-frontend \
    --backend-pool-name dr-pool \
    --protocol tcp \
    --port 80 \
    --backend-port 80

# Use Traffic Manager for failover
az network traffic-manager profile create \
    --resource-group my-rg \
    --name my-tm-profile \
    --routing-method failover \
    --unique-dns-name myapp

# Add endpoints
az network traffic-manager endpoint create \
    --resource-group my-rg \
    --profile-name my-tm-profile \
    --name primary \
    --type AzureEndpoints \
    --target-resource-id /subscriptions/sub-id/resourceGroups/my-rg/providers/Microsoft.Network/loadBalancers/my-lb

az network traffic-manager endpoint create \
    --resource-group my-rg \
    --profile-name my-tm-profile \
    --name secondary \
    --type AzureEndpoints \
    --target-resource-id /subscriptions/sub-id/resourceGroups/dr-rg/providers/Microsoft.Network/loadBalancers/dr-lb
```

## COMMON ISSUES

### 1. Health Check Failures

**Problem**: Backend VMs marked unhealthy.

**Solution**:
```bash
# Check probe configuration
az network lb probe show \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-probe

# Test health endpoint on VM
curl http://localhost:80/health

# Update probe interval
az network lb probe update \
    --resource-group my-rg \
    --lb-name my-lb \
    --name my-probe \
    --interval 10 \
    --probe-threshold 3
```

### 2. High Latency

**Problem**: Slow response times.

**Solution**:
- Deploy VMs in same zone as LB
- Enable health probes with fast detection
- Use Availability Zones
- Check network latency

### 3. SNAT Exhaustion

**Problem**: Cannot make outbound connections.

**Solution**:
```bash
# Increase outbound ports
az network lb outbound-rule update \
    --resource-group my-rg \
    --lb-name my-lb \
    --outbound-ports 64000

# Add more public IPs
```

## PERFORMANCE

### Performance Benchmarks

| Metric | Typical | Target |
|--------|---------|--------|
| Latency | <5ms | <2ms |
| Throughput | 10 Gbps | 50+ Gbps |
| Max connections | 50K | 100K |
| Availability | 99.9% | 99.99% |

### Optimization Tips

1. **Enable HA ports**: Single rule
2. **Use zones**: Same-zone routing
3. **Connection reuse**: HTTP keepalive
4. **Health probes**: Fast detection

## COMPATIBILITY

### SDK Support

| Language | Library |
|----------|---------|
| CLI | az |
| Python | azure-mgmt-network |
| .NET | Microsoft.Azure.Management.Network |
| Go | azure-sdk-for-go |

### Integration

| Service | Integration |
|---------|-------------|
| VMSS | Auto-scaling |
| AKS | Service load balancing |
| App Service | Integration |
| Azure Firewall | Security |

## CROSS-REFERENCES

### Related Patterns

- Multi-tier: Web/App/Data
- DR: Cross-region
- Scale: VMSS integration

### What to Study Next

1. Application Gateway: Layer 7
2. Traffic Manager: DNS routing
3. AKS: Kubernetes networking

## EXAM TIPS

### Key Exam Facts

- Health probes: Monitor backend health
- NAT rules: Port forwarding
- Outbound: SNAT configuration
- Availability zones: Zone redundancy

### Exam Questions

- **Question**: "Add VM to pool" = Add NIC to backend pool
- **Question**: "Health check" = Configure probe
- **Question**: "DR setup" = Traffic Manager
