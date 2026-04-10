---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Networking
Purpose: Advanced networking concepts including VNet peering, ExpressRoute, VPN gateways, and network security
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Networking.md
RelatedFiles: 01_Basic_Azure_Networking.md, 03_Practical_Azure_Networking.md
UseCase: Enterprise network architecture and hybrid connectivity
CertificationExam: AZ-304 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Azure networking enables complex enterprise architectures with hybrid connectivity, global reach, and enhanced security.

## 📖 WHAT

### VNet Peering Types

| Type | Traffic | Use Case |
|------|---------|----------|
| VNet Peering | Within region | Cross-VNet communication |
| Global Peering | Cross-region | Multi-region architecture |
| Hub-Spoke | Central network | Shared services |
| Transit Gateway | Multi-hub | Global transit |

### ExpressRoute

| SKU | Bandwidth | Circuits | Location |
|-----|----------|----------|-----------|
| Standard | 50Mbps-10Gbps | 1 | Any |
| Premium | + Global | 2 | Extended |
| Local | 1Gbps-10Gbps | 1 | Metropolitan |

### VPN Gateway Types

| Type | Tunnel | Throughput |
|------|--------|-----------|
| Route-based | 30+ | 1Gbps |
| Policy-based | 1-10 | 100Mbps-1Gbps |
| Active-Active | 4+ | 2Gbps |
| ExpressRoute | Encrypted | 500Mbps-10Gbps |

### Network Security Options

- **NSG**: Layer 3-4 filtering
- **ASG**: Application security groups
- **Azure Firewall**: Layer 7, NAT, threat intelligence
- **WAF**: Web application protection

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| VNet | Virtual Network | VPC | VPC Network |
| Peering | VNet Peering | VPC Peering | VPC Peering |
| Transit | Transit Gateway | Transit Gateway | Cloud Router |
| VPN | VPN Gateway | Site-to-Site | Cloud VPN |
| Private Link | Private Link | PrivateLink | Private Google Access |
| CDN | Azure CDN | CloudFront | Cloud CDN |

## 🔧 HOW

### Example 1: Hub-Spoke Network Architecture

```bash
# Create hub VNet
az network vnet create \
    --name hub-vnet \
    --resource-group net-rg \
    --address-prefix 10.0.0.0/16 \
    --subnet-name GatewaySubnet \
    --subnet-prefix 10.0.0.0/27

az network vnet subnet create \
    --name management \
    --resource-group net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.1.0/24

az network vnet subnet create \
    --name shared-services \
    --resource-group net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.2.0/24

# Create spoke VNets
az network vnet create \
    --name spoke-prod-vnet \
    --resource-group net-rg \
    --address-prefix 10.10.0.0/16

az network vnet create \
    --name spoke-dev-vnet \
    --resource-group net-rg \
    --address-prefix 10.20.0.0/16

# Create hub peerings
az network vnet peering create \
    --name hub-to-spoke-prod \
    --resource-group net-rg \
    --vnet-name hub-vnet \
    --remote-vnet spoke-prod-vnet \
    --allow-virtual-network-access \
    --allow-forwarded-traffic

az network vnet peering create \
    --name spoke-prod-to-hub \
    --resource-group net-rg \
    --vnet-name spoke-prod-vnet \
    --remote-vnet hub-vnet \
    --allow-virtual-network-access \
    --allow-forwarded-traffic
```

### Example 2: Site-to-Site VPN

```bash
# Create VPN gateway
az network vnet subnet create \
    --name GatewaySubnet \
    --resource-group net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.0.0/27

az network public-ip create \
    --name vpn-gateway-ip \
    --resource-group net-rg \
    --location eastus \
    --sku Standard

az network vnet-gateway create \
    --name vpn-gateway \
    --resource-group net-rg \
    --vnet-name hub-vnet \
    --location eastus \
    --gateway-type Vpn \
    --vpn-type Routebased \
    --sku VpnGw1 \
    --public-ip-address vpn-gateway-ip \
    --bgp-peering-address 10.0.0.10 \
    --asn 65001

# Create local network gateway
az network local-gateway create \
    --name onprem-gateway \
    --resource-group net-rg \
    --location eastus \
    --gateway-ip-address 203.0.113.1 \
    --address-space 192.168.0.0/16

# Create VPN connection
az network vnet-gateway vpn-connection create \
    --name site-to-site \
    --resource-group net-rg \
    --vnet-gateway-name vpn-gateway \
    --local-gateway onprem-gateway \
    --shared-key "YourSecureKey123"
```

### Example 3: Azure Firewall with DNAT

```bash
# Create firewall subnet
az network vnet subnet create \
    --name AzureFirewallSubnet \
    --resource-group net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.3.0/24

# Create public IP for firewall
az network public-ip create \
    --name fw-public-ip \
    --resource-group net-rg \
    --location eastus \
    --sku Standard

# Create firewall
az network firewall create \
    --name.azurefw \
    --resource-group net-rg \
    --location eastus \
    --vnet-name hub-vnet \
    --public-ip fw-public-ip

# Create NAT rule (allow RDP)
az network firewall nat-rule create \
    --name allow-rdp \
    --resource-group net-rg \
    --azure-firewall-name azurefw \
    --priority 100 \
    --action Dnat \
    --rule-type Transparent \
    --source-addresses "*" \
    --destination-addresses "10.0.3.4" \
    --translated-address 10.0.1.10 \
    --translated-port 3389 \
    --name match-allow-rdp \
    --protocols TCP

# Create network rule (allow HTTP/HTTPS)
az network firewall network-rule create \
    --name allow-web \
    --resource-group net-rg \
    --azure-firewall-name azurefw \
    --priority 200 \
    --action Allow \
    --rule-type Transparent \
    --source-addresses 10.0.1.0/24 \
    --destination-addresses "*" \
    --destination-ports 80 443 \
    --protocols TCP
```

### Example 4: Private Endpoint

```bash
# Create storage account with Private Endpoint
az storage account create \
    --name privatestore \
    --resource-group net-rg \
    --location eastus \
    --sku Standard_LRS

# Create VNets
az network vnet create \
    --name app-vnet \
    --resource-group net-rg \
    --address-prefix 10.100.0.0/24

az network vnet subnet create \
    --name endpoints \
    --resource-group net-rg \
    --vnet-name app-vnet \
    --address-prefix 10.100.1.0/24

# Create Private Endpoint
az network private-endpoint create \
    --name storage-pe \
    --resource-group net-rg \
    --location eastus \
    --vnet-name app-vnet \
    --subnet endpoints \
    --type Microsoft.Storage/storageAccounts \
    --private-connection-resource-id $(az storage account show \
        --name privatestore \
        --query id -o tsv) \
    --connection-name storage-connection

# Create DNS zone
az network private-dns zone create \
    --name privatestore.blob.core.windows.net \
    --resource-group net-rg

# Link DNS zone to VNet
az network private-dns link vnet create \
    --name storage-dns-link \
    --resource-group net-rg \
    --zone-name privatestore.blob.core.windows.net \
    --virtual-network app-vnet \
    --registration-enabled false

# Add DNS A record
az network private-dns record-set a create \
    --name privatestore \
    --zone-name privatestore.blob.core.windows.net \
    --resource-group net-rg

# Get PE IP
PE_IP=$(az network private-endpoint show \
    --name storage-pe \
    --resource-group net-rg \
    --query "customDnsConfigs[0].ipAddresses[0]" -o tsv)

az network private-dns record-set a add-record \
    --record-set-name privatestore \
    --zone-name privatestore.blob.core.windows.net \
    --resource-group net-rg \
    -a $PE_IP
```

## ⚠️ COMMON ISSUES

### Peering Issues

- **Address space overlap**: Cannot peer overlapping VNets
- **DNS resolution**: Requires private DNS zone
- **Transitive peering**: Not automatic in hub-spoke

### VPN Issues

- **IPSec phase mismatch**: Ensure compatible encryption
- **BGP AS number conflict**: Must be different
- **MTU issues**: Fragmentation with ICMP

### Firewall Issues

- **SNAT for outbound**: Required for internet access
- **Application rules**: Require FQDN configuration
- **Inspection bypass**: Allowed traffic bypasses firewall

## 🏃 PERFORMANCE

### Network Optimization

| Optimization | Impact |
|---------------|--------|
| Accelerated Networking | 2-3x throughput |
| ExpressRoute | <1ms latency |
| Private Link | No internet hop |
| CDNs | Global edge |

### Bandwidth Planning

| Service | Typical Usage |
|---------|---------------|
| VNet Peering | Unlimited |
| VPN Gateway | 100Mbps-3Gbps |
| ExpressRoute | 50Mbps-10Gbps |

## 🌐 COMPATIBILITY

### VPN Device Compatibility

- Cisco ASA, ASR
- Juniper SRX, SSG
- Palo Alto PA-Series
- Fortinet FortiGate
- Microsoft RRAS

### Routing Protocols

- BGP (required for ExpressRoute)
- OSPF (optional)
- Static routes

## 🔗 CROSS-REFERENCES

### Related Services

- **Azure Load Balancer**: Traffic distribution
- **Application Gateway**: L7 routing
- **Azure CDN**: Content delivery
- **ExpressRoute**: Private connectivity

### Related Concepts

- **Private Link**: Private endpoint access
- **Service Endpoints**: VNet access to services
- **Network Watcher**: Diagnostics

## ✅ EXAM TIPS

### Key Differences

- **VNet Peering**: Non-transitive, direct routing
- **VPN Gateway**: Encrypted, route-based vs policy-based
- **ExpressRoute**: Private, dedicated circuit

### Cost Optimization

- Use VNet peering (free)
- Use Standard SKU for basic needs
- Consider Local for metropolitan connectivity

### Security

- Azure Firewall for centralized rule
- NSG for microsegmentation
- Private Link for service access