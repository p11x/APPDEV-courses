---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Networking
Purpose: Practical hands-on labs for enterprise network deployment and security
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Networking.md, 02_Advanced_Azure_Networking.md
RelatedFiles: 01_Basic_Azure_Networking.md, 02_Advanced_Azure_Networking.md
UseCase: Deploying and managing enterprise networks
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Hands-on labs demonstrate enterprise network patterns with hub-spoke topology, VPN connectivity, and security boundaries.

## 📖 WHAT

### Lab Overview

Deploy hub-spoke network topology with VPN connectivity, Azure Firewall, and private endpoints.

### Architecture

```
                    ┌────────────────────────────────────┐
                    │         On-Premises              │
                    │    ┌─────────────────────┐    │
                    │    │   Corporate VPN    │    │
                    │    └──────────┬────────┘    │
                    └────────────────┼────────────┘
                                     │ VPN Tunnel
                    ┌────────────────┼────────────┐
                    │         Hub VNet │            │
                    │  ┌──────────────┼──────────┐│
                    │  │ Azure Firewall│         ││
                    │  └──────────────┴──────────┘│
                    │          /    |    \          │
        ┌────────────┼────────-+----+----+─────────┼──────────┐
        │           │         |    |    |         │          │
   ┌────▼────┐ ┌──▼─────┐ ┌──▼────┐ ┌───▼────┐ ┌───▼────┐
   │ Prod VNet│ │ Dev    │ │ Shared│ │ Vault │ │ API   │
   └─────────┘ └───────┘ └───────┘ └───────┘ └───────┘
```

## 🔧 HOW

### Module 1: Hub VNet with Firewall

```bash
# Create resource group
az group create \
    --name enterprise-net-rg \
    --location eastus

# Create hub VNet
az network vnet create \
    --name hub-vnet \
    --resource-group enterprise-net-rg \
    --address-prefix 10.0.0.0/16

# Create subnets
az network vnet subnet create \
    --name GatewaySubnet \
    --resource-group enterprise-net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.0.0/27

az network vnet subnet create \
    --name AzureFirewallSubnet \
    --resource-group enterprise-net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.1.0/24

az network vnet subnet create \
    --name management \
    --resource-group enterprise-net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.2.0/24

az network vnet subnet create \
    --name shared-services \
    --resource-group enterprise-net-rg \
    --vnet-name hub-vnet \
    --address-prefix 10.0.3.0/24

# Create Azure Firewall
az network public-ip create \
    --name azurefw-ip \
    --resource-group enterprise-net-rg \
    --sku Standard

az network firewall create \
    --name azure-fw \
    --resource-group enterprise-net-rg \
    --location eastus \
    --vnet-name hub-vnet \
    --public-ip azurefw-ip
```

### Module 2: Spoke Networks

```bash
# Create spoke VNets
az network vnet create \
    --name prod-vnet \
    --resource-group enterprise-net-rg \
    --address-prefix 10.1.0.0/16

az network vnet create \
    --name dev-vnet \
    --resource-group enterprise-net-rg \
    --address-prefix 10.2.0.0/16

az network vnet create \
    --name workload-vnet \
    --resource-group enterprise-net-rg \
    --address-prefix 10.3.0.0/16

# Create subnets
for subnet in app-tier data-tier web-tier; do
    az network vnet subnet create \
        --name $subnet \
        --resource-group enterprise-net-rg \
        --vnet-name prod-vnet \
        --address-prefix 10.1.$(echo $subnet | grep -oe '[0-9]' | head -1)0.0/24
done
```

### Module 3: Network Security Groups

```bash
# Create NSG for prod
az network nsg create \
    --name prod-nsg \
    --resource-group enterprise-net-rg \
    --location eastus

# Add security rules
az network nsg rule create \
    --name allow-https \
    --resource-group enterprise-net-rg \
    --nsg-name prod-nsg \
    --priority 100 \
    --direction Inbound \
    --protocol tcp \
    --source-address-prefixes 10.0.0.0/16 \
    --destination-address-prefixes 10.1.0.0/16 \
    --destination-port-ranges 443

az network nsg rule create \
    --name allow-sql \
    --resource-group enterprise-net-rg \
    --nsg-name prod-nsg \
    --priority 110 \
    --direction Inbound \
    --protocol tcp \
    --source-app-service-prefix ProdApp \
    --destination-address-prefixes 10.1.2.0/24 \
    --destination-port-ranges 1433

az network nsg rule create \
    --name deny-all-inbound \
    --resource-group enterprise-net-rg \
    --nsg-name prod-nsg \
    --priority 1000 \
    --direction Inbound \
    --protocol "*" \
    --source-address-prefixes "*" \
    --destination-address-prefixes "*" \
    --access Deny

# Associate NSG with subnet
az network vnet subnet update \
    --name data-tier \
    --resource-group enterprise-net-rg \
    --vnet-name prod-vnet \
    --network-security-group prod-nsg
```

### Module 4: VPN Gateway

```bash
# Create VPN gateway
az network public-ip create \
    --name vpn-ip \
    --resource-group enterprise-net-rg \
    --location eastus \
    --sku Standard

az network vnet-gateway create \
    --name enterprise-vpn \
    --resource-group enterprise-net-rg \
    --vnet-name hub-vnet \
    --location eastus \
    --gateway-type Vpn \
    --vpn-type Routebased \
    --sku VpnGw1 \
    --public-ip-address vpn-ip \
    --bgp-peering-address 10.0.0.5 \
    --asn 65001

# Create local gateway
az network local-gateway create \
    --name corporate-hq \
    --resource-group enterprise-net-rg \
    --gateway-ip-address 203.0.113.1 \
    --location eastus \
    --address-space 192.168.0.0/16 \
    --bgp-peering-address 192.168.0.1 \
    --asn 65010

# Create VPN connection
az network vnet-gateway vpn-connection create \
    --name corp-vpn \
    --resource-group enterprise-net-rg \
    --vnet-gateway-name enterprise-vpn \
    --local-gateway corporate-hq \
    --shared-key "EnterpriseVPNKey2025!"
```

### Module 5: Private Endpoints

```bash
# Create Key Vault with Private Endpoint
az keyvault create \
    --name enterprise-kv \
    --resource-group enterprise-net-rg \
    --location eastus \
    --enable-soft-delete \
    --enable-purge-protection

# Create private endpoint
az network private-endpoint create \
    --name kv-pe \
    --resource-group enterprise-net-rg \
    --location eastus \
    --vnet-name workload-vnet \
    --subnet data-tier \
    --type Microsoft.KeyVault/vaults \
    --private-connection-resource-id $(az keyvault show --name enterprise-kv --query id -o tsv) \
    --connection-name kv-connection

# Create DNS zone for Key Vault
az network private-dns zone create \
    --name vault.azure.net \
    --resource-group enterprise-net-rg

# Link to hub VNet
az network private-dns link vnet create \
    --name kv-dns-link \
    --resource-group enterprise-net-rg \
    --zone-name vault.azure.net \
    --virtual-network hub-vnet \
    --registration-enabled false
```

## ⚠️ TROUBLESHOOTING

### Connectivity Tests

```bash
# Test VNet peering
az network vnet-check-ip-addressavailability \
    --resource-group enterprise-net-rg \
    --vnet-name prod-vnet \
    --ip-address 10.1.0.100

# Check VPN status
az network vnet-gateway show \
    --name enterprise-vpn \
    --resource-group enterprise-net-rg \
    --query "vpnConnections"

# Verify firewall logs
az network firewall show \
    --name azure-fw \
    --resource-group enterprise-net-rg \
    --query "ipConfigurations"
```

### Network Watcher

```bash
# Enable Network Watcher if not enabled
az network watcher configure \
    --resource-group enterprise-net-rg \
    --locations eastus \
    --enabled true

# Run connection troubleshoot
az network watcher run-connectivity-test \
    --resource-group enterprise-net-rg \
    --source-resource-id $(az vm show -n testvm -g enterprise-net-rg --query id -o tsv) \
    --dest-resource-id $(az vm show -n testvm2 -g enterprise-net-rg --query id -o tsv)

# Capture NSG flow logs
az network watcher flow-log create \
    --resource-group enterprise-net-rg \
    --nsg prod-nsg \
    --location eastus \
    --storage-account "$(az storage account list -g enterprise-net-rg --query [0].name -o tsv)"
```

## ✅ VERIFICATION

### Test Connectivity

```bash
# Get hub IP
HUB_FW_IP=$(az network firewall show \
    --name azure-fw \
    --resource-group enterprise-net-rg \
    --query "ipConfigurations[0].privateIPAddress" -o tsv)

# Test from spoke
az network traffic-profile subscription-quota \
    --location eastus

# View effective routes
az network route-table show-effective-routes \
    --resource-group enterprise-net-rg \
    --name prod-routes
```

### Test VPN

```bash
# Get VPN gateway config
az network vnet-gateway show \
    --name enterprise-vpn \
    --resource-group enterprise-net-rg

# Check connection status
az network vnet-gateway vpn-connection show \
    --name corp-vpn \
    --resource-group enterprise-net-rg \
    --vnet-gateway-name enterprise-vpn \
    --query "connectionStatus"
```

## 🧹 CLEANUP

```bash
# Delete VPN connection
az network vnet-gateway vpn-connection delete \
    --name corp-vpn \
    --resource-group enterprise-net-rg \
    --vnet-gateway-name enterprise-vpn

# Delete VPN gateway
az network vnet-gateway delete \
    --name enterprise-vpn \
    --resource-group enterprise-net-rg

# Delete firewall
az network firewall delete \
    --name azure-fw \
    --resource-group enterprise-net-rg

# Delete VNets
for vnet in hub-vnet prod-vnet dev-vnet workload-vnet; do
    az network vnet delete \
        --name $vnet \
        --resource-group enterprise-net-rg
done

# Delete resource group
az group delete \
    --name enterprise-net-rg \
    --yes
```

## 🔗 CROSS-REFERENCES

### Related Labs

- Azure Compute: VM network integration
- Azure Storage: Private endpoint access
- Azure Key Vault: Secret storage
- Azure AD: Conditional access