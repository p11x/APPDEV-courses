---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Networking
Purpose: Understanding Azure virtual networks, subnets, and network security groups
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Networking.md, 03_Practical_Azure_Networking.md
UseCase: Creating isolated network environments in Azure
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure Networking provides the foundation for cloud network connectivity. Understanding virtual networks and security is essential.

## 📖 WHAT

### Azure Networking Components

**Virtual Network (VNet)**: Isolated network in Azure

**Subnet**: Subdivision of VNet

**NSG (Network Security Group)**: Firewall rules

**Load Balancer**: Traffic distribution

**Application Gateway**: Layer 7 load balancer with WAF

**VPN Gateway**: Site-to-site VPN

## 🔧 HOW

### Example 1: Create Virtual Network

```bash
# Create VNet
az network vnet create \
    --name myvnet \
    --resource-group myrg \
    --location eastus \
    --address-prefixes 10.0.0.0/16 \
    --subnet-name default \
    --subnet-prefixes 10.0.0.0/24

# Create subnet
az network vnet subnet create \
    --name appsubnet \
    --vnet-name myvnet \
    --resource-group myrg \
    --address-prefixes 10.0.1.0/24
```

### Example 2: NSG Configuration

```bash
# Create NSG
az network nsg create \
    --name mynsg \
    --resource-group myrg

# Add rule
az network nsg rule create \
    --name allow-http \
    --nsg-name mynsg \
    --resource-group myrg \
    --protocol tcp \
    --direction Inbound \
    --priority 100 \
    --source-address-prefix '*' \
    --source-port-range '*' \
    --destination-address-prefix '*' \
    --destination-port-range 80 \
    --access Allow
```

## ✅ EXAM TIPS

- VNet is region-scoped
- NSG filters traffic at subnet or NIC level
- Load Balancer for layer 4
- Application Gateway for layer 7