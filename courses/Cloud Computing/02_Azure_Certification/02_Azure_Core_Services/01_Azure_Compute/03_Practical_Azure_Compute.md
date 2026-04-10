---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Compute
Purpose: Practical hands-on labs for Azure compute deployment and management
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Compute.md, 02_Advanced_Azure_Compute.md
RelatedFiles: 01_Basic_Azure_Compute.md, 02_Advanced_Azure_Compute.md
UseCase: Deploying and managing production compute workloads
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Hands-on labs reinforce Azure compute concepts through practical deployment, configuration, and management exercises.

## 📖 WHAT

### Lab Overview

This lab covers deploying a scalable web infrastructure using VM scale sets, load balancing, and auto-scaling.

### Architecture

```
                    ┌─────────────────┐
                    │  Load Balancer   │
                    │ (Zone Redundant)│
                    └────────┬────────┘
                             │
                    ┌────────▼────────┐
                    │  VM Scale Set   │
                    │  (Min 2, Max 10) │
                    └─────────────────┘
```

## 🔧 HOW

### Module 1: Resource Group and Network

```bash
# Create resource group
az group create \
    --name webapp-rg \
    --location eastus

# Create virtual network
az network vnet create \
    --name webapp-vnet \
    --resource-group webapp-rg \
    --address-prefix 10.0.0.0/16

# Create subnets
az network subnet create \
    --name webapp-subnet \
    --resource-group webapp-rg \
    --vnet-name webapp-vnet \
    --address-prefix 10.0.1.0/24
```

### Module 2: VM Scale Set with Web Server

```bash
# Create scale set with custom script
az vmss create \
    --name webapp-vmss \
    --resource-group webapp-rg \
    --image UbuntuLTS \
    --vm-sku Standard_B2s \
    --instance-count 2 \
    --upgrade-mode Manual \
    --admin-username azureuser \
    --generate-ssh-keys \
    --vnet-name webapp-vnet \
    --subnet webapp-subnet

# Add custom script extension for nginx
az vmss extension set \
    --name CustomScript \
    --vmss-name webapp-vmss \
    --resource-group webapp-rg \
    --publisher Microsoft.Azure.Extensions \
    --type CustomScript \
    --settings '{"commandToExecute":"apt-get update && apt-get install -y nginx && echo hello > /var/www/html/index.html"}'
```

### Module 3: Load Balancer Configuration

```bash
# Create public IP
az network public-ip create \
    --name webapp-ip \
    --resource-group webapp-rg \
    --sku Standard \
    --allocation-method Static

# Create load balancer
az network lb create \
    --name webapp-lb \
    --resource-group webapp-rg \
    --sku Standard \
    --frontend-ip-name webapp-fe \
    --backend-pool-name webapp-backend

# Create health probe
az network lb probe create \
    --name webapp-health \
    --resource-group webapp-rg \
    --lb-name webapp-lb \
    --protocol tcp \
    --port 80 \
    --interval 15 \
    --probe-count 2

# Create load balancing rule
az network lb rule create \
    --name webapp-rule \
    --resource-group webapp-rg \
    --lb-name webapp-lb \
    --protocol tcp \
    --front-end-port 80 \
    --back-end-port 80 \
    --probe-name webapp-health \
    --backend-pool-name webapp-backend
```

### Module 4: Auto-scaling Rules

```bash
# Enable autoscale
az monitor autoscale create \
    --name webapp-autoscale \
    --resource-group webapp-rg \
    --resource "/subscriptions/$(az account show --query id -o tsv)/resourceGroups/webapp-rg/providers/Microsoft.Compute/virtualMachineScaleSets/webapp-vmss" \
    --min-count 2 \
    --max-count 10 \
    --count 2

# Scale-out rule (CPU > 70%)
az monitor autoscale rule create \
    --name scale-out \
    --resource-group webapp-rg \
    --autoscale-name webapp-autoscale \
    --condition "Percentage CPU > 70" \
    --scale-action type ChangeCount \
    --direction Increase \
    --scale-value 1 \
    --cooldown 5

# Scale-in rule (CPU < 30%)
az monitor autoscale rule create \
    --name scale-in \
    --resource-group webapp-rg \
    --autoscale-name webapp-autoscale \
    --condition "Percentage CPU < 30" \
    --scale-action type ChangeCount \
    --direction Decrease \
    --scale-value 1 \
    --cooldown 5

# Add memory-based rule
az monitor autoscale rule create \
    --name mem-scale-out \
    --resource-group webapp-rg \
    --autoscale-name webapp-autoscale \
    --condition "Available Memory < 1" \
    --scale-action type ChangeCount \
    --direction Increase \
    --scale-value 1 \
    --cooldown 5
```

### Module 5: Enable Boot Diagnostics

```bash
# Enable boot diagnostics
az vm boot-diagnostics enable \
    --name webapp-vmss_0 \
    --resource-group webapp-rg \
    --storage "https://webappdiag.blob.core.windows.net/"

az vm boot-diagnostics enable \
    --name webapp-vmss_1 \
    --resource-group webapp-rg \
    --storage "https://webappdiag.blob.core.windows.net/"

# Get console screenshot
az vm get-instance-view \
    --name webapp-vmss_0 \
    --resource-group webapp-rg \
    --query "instanceView.bootDiagnostics.consoleScreenshotUri"
```

## ⚠️ TROUBLESHOOTING

### Common Issues

```bash
# Check scale set instances
az vmss list-instances \
    --name webapp-vmss \
    --resource-group webapp-rg

# Check instance health
az vmss get-instance-view \
    --name webapp-vmss \
    --resource-group webapp-rg

# View extension status
az vmss extension list \
    --vmss-name webapp-vmss \
    --resource-group webapp-rg

# Check autoscale history
az monitor autoscale-show-settings \
    --name webapp-autoscale \
    --resource-group webapp-rg
```

### Debug Commands

```bash
# Get load balancer status
az network lb show \
    --name webapp-lb \
    --resource-group webapp-rg

# Check backend health
az network lb address-list show-backend-health \
    --lb-name webapp-lb \
    --resource-group webapp-rg

# View autoscale notifications
az monitor autoscale list-history \
    --resource-group webapp-rg \
    --autoscale-name webapp-autoscale
```

## ✅ VERIFICATION

### Test Web Application

```bash
# Get public IP
LB_IP=$(az network public-ip show \
    --name webapp-ip \
    --resource-group webapp-rg \
    --query ipAddress -o tsv)

# Test nginx response
curl http://$LB_IP

# Test from multiple IPs
for i in {1..10}; do
    curl -s http://$LB_IP | head -1
done
```

### Test Auto-scaling

```bash
# Install stress tool on instance
az vmss run-command invoke \
    --resource-group webapp-rg \
    --name webapp-vmss \
    --instance-id 0 \
    --command-id RunShellScript \
    --scripts "apt-get update && apt-get install -y stress"

# Trigger CPU load
az vmss run-command invoke \
    --resource-group webapp-rg \
    --name webapp-vmss \
    --instance-id 0 \
    --command-id RunShellScript \
    --scripts "stress --cpu 80"
```

## 🧹 CLEANUP

```bash
# Delete autoscale
az monitor autoscale delete \
    --name webapp-autoscale \
    --resource-group webapp-rg

# Delete scale set
az vmss delete \
    --name webapp-vmss \
    --resource-group webapp-rg \
    --yes

# Delete load balancer
az network lb delete \
    --name webapp-lb \
    --resource-group webapp-rg

# Delete resources
az group delete \
    --name webapp-rg \
    --yes
```

## 🔗 CROSS-REFERENCES

### Related Labs

- Azure Storage: For application data
- Azure Monitor: For observability
- Azure Backup: For disaster recovery
- Azure CDN: For content delivery

### Next Steps

- Add SSL/TLS with Application Gateway
- Add Web Application Firewall
- Configure Azure Front Door
- Add Azure AD authentication