---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure CDN
Purpose: Understanding Azure Content Delivery Network for content acceleration
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Azure_CDN.md, 03_Practical_Azure_CDN.md
UseCase: Accelerating content delivery globally
CertificationExam: Microsoft Azure Fundamentals - AZ-900
LastUpdated: 2025
---

## WHY

Azure Content Delivery Network (CDN) is a global CDN solution for delivering high-bandwidth content. Understanding Azure CDN is essential for serving content to users worldwide with low latency.

### Why Azure CDN Matters

- **Performance**: Reduce latency by up to 90%
- **Global Presence**: 150+ edge locations worldwide
- **Scalability**: Handle millions of requests
- **Security**: Built-in DDoS protection
- **Cost**: Reduce origin server load
- **Reliability**: 99.9% availability

### Industry Statistics

- 150+ POPs globally
- 90%+ latency reduction typical
- Petabytes of content delivered
- 99.9% availability SLA

### When NOT to Use Azure CDN

- Dynamic content: Use Azure Front Door
- Real-time data: Use SignalR
- Private content: Use Private Link
- Simple static files: Use Azure Static Web Apps

## WHAT

### Azure CDN Products

| Product | Description | Use Case |
|---------|-------------|----------|
| Azure CDN (Microsoft) | Microsoft-managed | General purpose |
| Azure CDN (Akamai) | Akamai-powered | Enterprise |
| Azure CDN Premium | Verizon-powered | Premium features |

### Core Concepts

**POP (Point of Presence)**: Edge server locations.

**Origin**: Source of content (Storage, Web App, etc.).

**Endpoint**: CDN endpoint URL.

**Rules Engine**: Custom content handling.

### Architecture Diagram

```
                    AZURE CDN ARCHITECTURE
                    ====================

    ┌──────────────┐    ┌──────────────┐    ┌──────────────┐
    │   US User   │    │  EU User    │    │  Asia User  │
    └──────┬───────┘    └──────┬───────┘    └──────┬───────┘
           │                   │                   │
           ▼                   ▼                   ▼
    ┌──────────┐         ┌──────────┐         ┌──────────┐
    │ US Edge  │         │  EU Edge │         │ Asia Edge│
    │   POP   │         │   POP    │         │   POP   │
    └──────────┘         └──────────┘         └──────────┘
           │                   │                   │
           └───────────────────┼───────────────────┘
                               │
                               ▼
    ┌──────────────────────────────────────────────────────┐
    │              ORIGIN SERVER                           │
    │  - Azure Blob Storage                               │
    │  - Web Apps                                         │
    │  - VMs                                             │
    └──────────────────────────────────────────────────────┘
```

### Caching Behavior

| Header | Effect |
|--------|--------|
| Cache-Control | Standard HTTP caching |
| Expires | Alternative expiration |
| ETag | Content version |
| Vary | Multiple versions |

## HOW

### Example 1: Create CDN Profile and Endpoint

```bash
# Create CDN profile
az cdn profile create \
    --resource-group my-rg \
    --name my-cdn-profile \
    --sku Standard_Microsoft

# Create CDN endpoint
az cdn endpoint create \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --name my-cdn-endpoint \
    --origin www.example.com \
    --origin-host-header www.example.com

# Or use Azure Storage as origin
az cdn endpoint create \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --name my-storage-cdn \
    --origin-name mystorageaccount.blob.core.windows.net \
    --origin-host-header mystorageaccount.blob.core.windows.net

# List CDN endpoints
az cdn endpoint list \
    --resource-group my-rg \
    --profile-name my-cdn-profile

# Get CDN URL
# https://my-cdn-endpoint.azureedge.net
```

### Example 2: Configure Caching Rules

```bash
# Update cache settings
az cdn endpoint update \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --name my-cdn-endpoint \
    --cache-duration 7d \
    --is-compression-enabled true \
    --query-string-caching-behavior ignore-query-string

# Configure query string caching
# Options: ignore-query-string, bypass-caching, use-query-string, not-specified

# Set compression
az cdn endpoint update \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --name my-cdn-endpoint \
    --is-compression-enabled true \
    --content-types-to-compress "text/html text/plain text/css application/javascript application/json"

# Create custom rule
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --order 1 \
    --rule-name "CacheStatic" \
    --match-variable "RequestUri" \
    --operator "Contains" \
    --match-values "/images/" \
    --action-name "Cache" \
    --duration 30d
```

### Example 3: Configure Custom Domain

```bash
# Register custom domain
az cdn custom-domain create \
    --resource-group my-rg \
    --endpoint-name my-cdn-endpoint \
    --profile-name my-cdn-profile \
    --name www.example.com \
    --hostname www.example.com

# Enable HTTPS for custom domain
az cdn custom-domain enable-https \
    --resource-group my-rg \
    --endpoint-name my-cdn-endpoint \
    --profile-name my-cdn-profile \
    --name www.example.com

# Or use own certificate
az cdn custom-domain enable-https \
    --resource-group my-rg \
    --endpoint-name my-cdn-endpoint \
    --profile-name my-cdn-profile \
    --name www.example.com \
    --certificate-type "Dedicated" \
    --minimum-tls-version "TLS12"

# Validate custom domain
az cdn custom-domain show \
    --resource-group my-rg \
    --endpoint-name my-cdn-endpoint \
    --profile-name my-cdn-profile \
    --name www.example.com
```

### Example 4: Configure Origin Settings

```bash
# Update origin settings
az cdn origin update \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --origin-name default \
    --http-port 80 \
    --https-port 443 \
    --origin-host-header "www.example.com"

# Add additional origin
az cdn origin create \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --name backup-origin \
    --hostname backup.example.com

# Configure origin groups (for failover)
az cdn origin-group create \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --name primary-group \
    --origins default

# Health probe settings
az cdn endpoint update \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --name my-cdn-endpoint \
    --probe-interval 100 \
    --probe-path /healthcheck \
    --probe-protocol Https \
    --probe-request-type HEAD
```

## COMMON ISSUES

### 1. Cache Not Working

**Problem**: Content not cached.

**Solution**:
```bash
# Check caching headers
# Ensure no Cache-Control: private or no-store

# Purge cache
az cdn endpoint purge \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --content-paths "/*"

# Verify caching is enabled
az cdn endpoint show \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint
```

### 2. Slow Performance

**Problem**: CDN slower than expected.

**Solution**:
- Verify closest edge POP is being used
- Enable compression
- Increase cache duration
- Use HTTP/2

### 3. 403 Forbidden

**Problem**: Access denied.

**Solution**:
```bash
# Check origin authentication
# If using Storage, ensure public access or proper SAS
# Check CORS settings
```

### 4. SSL Certificate Issues

**Problem**: HTTPS not working.

**Solution**:
- Azure CDN provides free certificates
- Allow 2-24 hours for provisioning
- Check domain validation

## PERFORMANCE

### Performance Characteristics

| Metric | Value |
|--------|-------|
| Latency reduction | 50-90% |
| Edge locations | 150+ |
| Max file size | 100MB (free), 1GB (premium) |
| Compression | Gzip, Brotli |

### Cost Optimization

| Strategy | Savings |
|----------|---------|
| Cache longer | Reduce origin calls |
| Compression | Reduce bandwidth |
| Tier selection | Match needs |

## COMPATIBILITY

### Region Availability

- Available in all Azure regions
- Edge locations worldwide

### Integration

| Service | Use Case |
|---------|----------|
| Blob Storage | Static assets |
| Web Apps | Web content |
| Media Services | Video streaming |
| Front Door | Dynamic + Static |

## CROSS-REFERENCES

### Related Services

- Azure Front Door: Dynamic content
- Azure Storage: Origin
- Azure Media Services: Video
- Cloudflare: Alternative CDN

### Alternatives

| Need | Use |
|------|-----|
| Dynamic content | Front Door |
| API Gateway | API Management |
| Private content | Private Link |

### What to Study Next

1. Advanced CDN: Rules, optimization
2. Front Door: Global routing
3. Media Services: Video streaming

## EXAM TIPS

### Key Exam Facts

- Azure CDN: Static content delivery
- Edge locations: Global POPs
- Caching: Reduces origin load
- Custom domains: Supported with HTTPS

### Exam Questions

- **Question**: "Static files globally" = Azure CDN
- **Question**: "Reduce origin load" = Enable caching
- **Question**: "Free HTTPS" = Azure CDN managed certs
