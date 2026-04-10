---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud Networking
Concept: Hybrid Connectivity
Difficulty: beginner
Prerequisites: Basic Cloud Computing, Cloud Networking Basics
RelatedFiles: 02_Advanced_Hybrid_Connectivity.md, 03_Practical_Hybrid_Connectivity.md
UseCase: Implementing hybrid cloud connectivity
CertificationExam: AWS Solutions Architect / Professional
LastUpdated: 2025
---

## WHY

Hybrid cloud connectivity is the foundation for multi-cloud architectures. Understanding connectivity options enables secure, high-performance communication between clouds and on-premises infrastructure.

### Why Hybrid Connectivity Matters

- **Data Migration**: Move workloads between clouds and on-prem
- **Burst Capacity**: Extend on-prem to cloud during peaks
- **Disaster Recovery**: Replicate data for DR scenarios
- **Compliance**: Keep sensitive data on-prem while using cloud

### Connectivity Models

| Model | Use Case | Latency | Cost |
|-------|----------|---------|------|
| VPN | Small workloads | 20-50ms | Low |
| Direct Connect | Large data transfer | 5-10ms | Medium |
| SD-WAN | Branch offices | 10-30ms | Medium |
| Transit Hub | Multi-cloud | Variable | Variable |

## WHAT

### Cloud Connectivity Options

**AWS Connectivity**
- Direct Connect: 1Gbps-100Gbps dedicated
- VPN: IPSec tunnel over public internet
- Transit Gateway: Hub for VPC-to-VPC

**Azure Connectivity**
- ExpressRoute: 50Mbps-100Gbps private
- VPN Gateway: Site-to-site IPSec
- Virtual WAN: Global transit hub

**GCP Connectivity**
- Cloud Interconnect: 10Gbps-100Gbps
- Cloud VPN: IPSec tunnels
- Cloud Router: BGP peering

### Connectivity Architecture

```
HYBRID CONNECTIVITY
===================

    ┌──────────────────────────────────────────────────┐
    │                 ON-PREMISES                        │
    │  ┌──────────┐  ┌──────────┐  ┌──────────┐       │
    │  │   App    │  │   Data   │  │   DB     │       │
    │  └──────────┘  └──────────┘  └──────────┘       │
    └────────────────────┬─────────────────────────────┘
                         │
    ┌────────────────────┼────────────────────────────┐
    │              EDGE ROUTER                          │
    │         (Direct Connect / VPN)                    │
    └────────────────────┬────────────────────────────┘
                         │
    ┌────────────────────┼────────────────────────────┐
    │              CLOUD CONNECTIVITY                   │
    │  ┌──────────┐  ┌──────────┐  ┌──────────┐       │
    │  │AWS Direct│  │Azure Expr│  │GCP Inter │       │
    │  │ Connect │  │ Route    │  │ connect  │       │
    │  └──────────┘  └──────────┘  └──────────┘       │
    └──────────────────────────────────────────────────┘
```

## HOW

### Example 1: AWS Direct Connect

```hcl
# AWS Direct Connect setup
resource "aws_dx_connection" "onprem" {
  name    = "onprem-connection"
  port    = "1Gbps"
  location = "EqDC2"
}

resource "aws_dx_lag" "main" {
  name            = "main-lag"
  location        = "EqDC2"
  number_of_connections = 2
  provider_name   = "AWS"
}

resource "aws_dx_gateway" "main" {
  name = "main-gateway"
  amazon_asn = 65001
}

resource "aws_dx_gateway_association" "main" {
  dx_gateway_id = aws_dx_gateway.main.id
  vpn_gateway_id = aws_vpc.main.vpn_gateway_id
}

resource "aws_vpn_connection" "main" {
  type                = "ipsec.1"
  customer_gateway_id = aws_customer_gateway.onprem.id
  vpn_gateway_id      = aws_vpn_gateway.main.id
}
```

### Example 2: Azure ExpressRoute

```hcl
# Azure ExpressRoute setup
resource "azurerm_express_route_circuit" "main" {
  name                  = "main-circuit"
  resource_group_name   = "network-rg"
  location              = "eastus"
  
  sku {
    tier   = "Standard"
    family = "MeteredData"
  }
  
  service_provider_properties {
    service_provider_name = "Equinix"
    peering_location     = "Washington DC"
    bandwidth_in_mbps    = 1000
  }
  
  authorization_key = "authorization-key"
}

resource "azurerm_express_route_gateway" "main" {
  name                = "main-gateway"
  resource_group_name = "network-rg"
  location            = "eastus"
  virtual_hub_id      = azurerm_virtual_hub.main.id
  sku                 = "Standard"
}

resource "azurerm_virtual_hub" "main" {
  name                = "main-hub"
  resource_group_name = "network-rg"
  location            = "eastus"
  address_prefix      = "10.0.0.0/16"
}
```

### Example 3: GCP Cloud Interconnect

```hcl
# GCP Cloud Interconnect setup
resource "google_compute_interconnect_attachment" "onprem" {
  name                = "onprem-attachment"
  region              = "us-central1"
  router              = google_compute_router.main.name
  interconnect        = "interconnect-1"
  ipsec_internal_addresses = ["10.0.0.0/24"]
}

resource "google_compute_router" "main" {
  name    = "main-router"
  network = google_compute_network.main.name
  region  = "us-central1"
  
  bgp {
    asn = 65001
  }
}

resource "google_compute_router_interface" "main" {
  name       = "main-interface"
  router     = google_compute_router.main.name
  region     = "us-central1"
  ip_range   = "169.254.0.1/30"
  linked_interconnect_attachment = google_compute_interconnect_attachment.onprem.name
}

resource "google_compute_network" "main" {
  name                    = "main-network"
  auto_create_subnetworks = false
}
```

## COMMON ISSUES

### 1. Latency

- Cross-cloud traffic adds latency
- Solution: Deploy in same region

### 2. Bandwidth Limits

- Direct Connect has port limits
- Solution: Use multiple connections

### 3. BGP Configuration

- Routing complexity
- Solution: Use transit gateways

## CROSS-REFERENCES

### Prerequisites

- Cloud networking basics
- VPN concepts
- BGP basics

### What to Study Next

1. Service Mesh
2. DNS Multi-Cloud
3. Multi-Cloud Security

## EXAM TIPS

- Know connectivity options for each provider
- Understand use cases for each connection type
- Be able to recommend connectivity based on requirements