---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud Networking
Concept: DNS Multi-Cloud
Difficulty: beginner
Prerequisites: Basic Cloud Computing, DNS Basics, Cloud Networking
RelatedFiles: 02_Advanced_DNS_Multi_Cloud.md, 03_Practical_DNS_Multi_Cloud.md
UseCase: Implementing DNS for multi-cloud environments
CertificationExam: AWS Solutions Architect / Professional
LastUpdated: 2025
---

## WHY

DNS is critical for multi-cloud architectures, enabling traffic routing, failover, and geographic distribution across cloud providers.

### Why DNS Matters

- **Traffic Routing**: Direct users to appropriate endpoints
- **Failover**: Automatic switching on failures
- **Geographic Routing**: Route based on user location
- **Load Balancing**: Distribute traffic across endpoints
- **Service Discovery**: Find services in dynamic environments

### DNS Benefits

| Benefit | Description | Implementation |
|---------|-------------|----------------|
| Global Routing | Route by geography | Latency-based |
| Health Checking | Monitor endpoints | Active checks |
| Failover | Automatic switching | DNS failover |
| Load Balancing | Distribute traffic | Weighted routing |

## WHAT

### Cloud DNS Services

**AWS Route 53**
- Globally distributed
- Health checks
- Routing policies: Simple, Weighted, Latency, Geo, Failover
- DNSSEC support

**Azure DNS**
- High availability
- Private DNS zones
- Alias records
- Traffic Manager integration

**GCP Cloud DNS**
- Low latency
- DNSSEC
- Managed zones
- Cloud Load Balancing integration

### DNS Architecture

```
MULTI-CLOUD DNS ARCHITECTURE
============================

┌─────────────────────────────────────────────────────────────┐
│                       DNS QUERIES                           │
│                    (User地理位置)                          │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────┼────────────────────────────────────┐
│                   DNS RESOLUTION                            │
│  ┌──────────┐   ┌──────────┐   ┌──────────┐               │
│  │Route 53  │   │Azure DNS │   │Cloud DNS │               │
│  │(Global)  │   │ (Global) │   │ (Global) │               │
│  └──────────┘   └──────────┘   └──────────┘               │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────┼────────────────────────────────────┐
│                 ENDPOINT HEALTH                             │
│  ┌──────────┐   ┌──────────┐   ┌──────────┐               │
│  │AWS Health│   │Azure Mon │   │GCP Health│               │
│  │ Check    │   │ itor     │   │ Check    │               │
│  └──────────┘   └──────────┘   └──────────┘               │
└─────────────────────────────────────────────────────────────┘
                         │
┌────────────────────────┼────────────────────────────────────┐
│                 BACKEND SERVICES                            │
│  ┌──────────┐   ┌──────────┐   ┌──────────┐               │
│  │ AWS EKS  │   │ Azure AKS│   │ GCP GKE  │               │
│  └──────────┘   └──────────┘   └──────────┘               │
└─────────────────────────────────────────────────────────────┘
```

## HOW

### Example 1: AWS Route 53 Configuration

```hcl
# AWS Route 53 hosted zone
resource "aws_route53_zone" "main" {
  name = "example.com"
  
  tags = {
    Environment = "production"
  }
}

# Simple record
resource "aws_route53_record" "www" {
  zone_id = aws_route53_zone.main.zone_id
  name    = "www"
  type    = "A"
  ttl     = 300
  
  records = [aws_instance.web.public_ip]
}

# Weighted record
resource "aws_route53_record" "weighted" {
  zone_id = aws_route53_zone.main.zone_id
  name    = "api"
  type    = "A"
  set_identifier = "primary"
  
  weighted_routing_policy {
    weight = 90
  }
  
  ttl  = 60
  records = [aws_instance.primary.private_ip]
}

resource "aws_route53_record" "weighted_secondary" {
  zone_id = aws_route53_zone.main.zone_id
  name    = "api"
  type    = "A"
  set_identifier = "secondary"
  
  weighted_routing_policy {
    weight = 10
  }
  
  ttl  = 60
  records = [aws_instance.secondary.private_ip]
}

# Health check
resource "aws_route53_health_check" "main" {
  type              = "HTTPS"
  fqdn              = "api.example.com"
  port              = 443
  resource_path     = "/health"
  failure_threshold = 3
  request_interval  = 10
}

# Failover record
resource "aws_route53_record" "failover_primary" {
  zone_id = aws_route53_zone.main.zone_id
  name    = " failover"
  type    = "A"
  set_identifier = "primary"
  
  failover_routing_policy {
    type = "PRIMARY"
  }
  
  ttl   = 60
  records = [aws_instance.primary.private_ip]
  
  health_check_id = aws_route53_health_check.main.id
}
```

### Example 2: Azure DNS Configuration

```hcl
# Azure DNS zone
resource "azurerm_dns_zone" "main" {
  name                = "example.com"
  resource_group_name = "network-rg"
  
  tags = {
    Environment = "production"
  }
}

# A record
resource "azurerm_dns_a_record" "www" {
  name                = "www"
  zone_name           = azurerm_dns_zone.main.name
  resource_group_name = "network-rg"
  ttl                 = 300
  
  records = [azurerm_public_ip.web.ip_address]
}

# CNAME record
resource "azurerm_dns_cname_record" "api" {
  name                = "api"
  zone_name           = azurerm_dns_zone.main.name
  resource_group_name = "network-rg"
  ttl                 = 300
  
  record {
    cname = "api.example.azure.com"
  }
}

# Alias record
resource "azurerm_dns_alias_record" "app" {
  name                = "app"
  zone_name           = azurerm_dns_zone.main.name
  resource_group_name = "network-rg"
  
  target_resource_id = azurerm_lb.main.id
}
```

### Example 3: GCP Cloud DNS Configuration

```hcl
# GCP managed zone
resource "google_dns_managed_zone" "main" {
  name     = "example-com"
  dns_name = "example.com."
  description = "Multi-cloud DNS zone"
  
  dnssec {
    state = "on"
  }
}

# A record
resource "google_dns_record_set" "www" {
  name = "www.example.com."
  type = "A"
  ttl  = 300
  
  managed_zone = google_dns_managed_zone.main.name
  
  rrdatas = [
    google_compute_instance.web.network_ip
  ]
}

# CNAME record
resource "google_dns_record_set" "api" {
  name = "api.example.com."
  type = "CNAME"
  ttl  = 300
  
  managed_zone = google_dns_managed_zone.main.name
  
  rrdatas = ["api.example.gcp.com."]
}

# AAAA record (IPv6)
resource "google_dns_record_set" "www_ipv6" {
  name = "www.example.com."
  type = "AAAA"
  ttl  = 300
  
  managed_zone = google_dns_managed_zone.main.name
  
  rrdatas = [google_compute_instance.web.network_ip]
}
```

## COMMON ISSUES

### 1. TTL Settings

- Caching issues during updates
- Solution: Use appropriate TTL values

### 2. DNSSEC

- Configuration complexity
- Solution: Enable gradually

### 3. Propagation Delays

- DNS changes not immediate
- Solution: Plan ahead, use lower TTL during changes

## CROSS-REFERENCES

### Prerequisites

- DNS fundamentals
- Cloud networking basics
- Load balancing concepts

### What to Study Next

1. Service Mesh
2. Multi-Cloud Security
3. Multi-Cloud DevOps

## EXAM TIPS

- Know DNS routing policies
- Understand health check configuration
- Be able to design DNS for multi-cloud