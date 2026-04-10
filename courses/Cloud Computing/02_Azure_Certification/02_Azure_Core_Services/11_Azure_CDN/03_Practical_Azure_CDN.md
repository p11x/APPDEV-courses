---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure CDN Practical
Purpose: Practical Azure CDN implementation and best practices
Difficulty: practical
Prerequisites: 01_Basic_Azure_CDN.md, 02_Advanced_Azure_CDN.md
RelatedFiles: 01_Basic_Azure_CDN.md, 02_Advanced_Azure_CDN.md
UseCase: Production CDN implementation
CertificationExam: Microsoft Azure Fundamentals - AZ-900
LastUpdated: 2025
---

## WHY

Practical Azure CDN implementation involves building real-world content delivery solutions, optimization patterns, and operational best practices. This knowledge is essential for deploying production CDN solutions.

### Why Practical Implementation Matters

- **Reliability**: Production-grade configurations
- **Performance**: Optimized for real workloads
- **Cost**: Bandwidth cost optimization
- **Security**: Protected content delivery
- **Monitoring**: Observable operations

### Common Production Use Cases

- **Static Website**: CSS, JS, images
- **Media Streaming**: Video delivery
- **Software Downloads**: Application updates
- **E-commerce**: Product images

## WHAT

### Implementation Patterns

| Pattern | Description | Use |
|---------|-------------|-----|
| Static assets | Cache everything | Images, JS |
| Dynamic cache | Partial caching | News, posts |
| Secure | Authenticated | Paid content |
| Multi-origin | Failover | High availability |

### Best Practices

1. **Enable compression**: Reduce size
2. **Long cache for static**: 30+ days
3. **Cache busting**: Version URLs
4. **Monitor**: Track hit ratio

## HOW

### Example 1: Static Website CDN

```bash
# Create storage account
az storage account create \
    --name mystorageaccount \
    --resource-group my-rg \
    --sku Standard_LRS

# Enable static website
az storage blob service-properties update \
    --account-name mystorageaccount \
    --static-website \
    --index-document index.html \
    --error-document 404.html

# Upload website files
az storage blob upload-batch \
    --source ./website \
    --destination '$web' \
    --account-name mystorageaccount

# Create CDN profile
az cdn profile create \
    --name my-cdn \
    --resource-group my-rg \
    --sku Standard_Microsoft

# Create endpoint
az cdn endpoint create \
    --name my-website-cdn \
    --profile-name my-cdn \
    --resource-group my-rg \
    --origin-name mystorageaccount.z33.web.core.windows.net \
    --origin-host-header mystorageaccount.z33.web.core.windows.net \
    --origin-type web

# Configure caching
az cdn endpoint update \
    --name my-website-cdn \
    --profile-name my-cdn \
    --resource-group my-rg \
    --cache-duration 30d \
    --is-compression-enabled true

# Get CDN URL
# https://my-website-cdn.azureedge.net
```

### Example 2: Media Streaming CDN

```bash
# Configure for video streaming
az cdn endpoint update \
    --name my-video-cdn \
    --profile-name my-cdn \
    --resource-group my-rg \
    --origin media.example.com \
    --origin-host-header media.example.com \
    --cache-duration 7d

# Add query string caching for video variants
az cdn endpoint update \
    --name my-video-cdn \
    --profile-name my-cdn \
    --resource-group my-rg \
    --query-string-caching-behavior "UniqueQueryStrings"

# Create rules for HLS/DASH
az cdn endpoint rule add \
    --name my-video-cdn \
    --profile-name my-cdn \
    --resource-group my-rg \
    --rule-name "VideoCache" \
    --match-variable "UrlPath" \
    --operator "Contains" \
    --match-values ".m3u8 .mpd .ts" \
    --action-name "CacheExpiration" \
    --cache-behavior "Override" \
    --cache-duration 1d
```

### Example 3: Secure Content Delivery

```bash
# Enable private content with SAS
# Generate SAS token for Azure Storage
az storage blob generate-sas \
    --account-name mystorage \
    --container-name private-content \
    --blob document.pdf \
    --permissions r \
    --expiry 2025-12-31

# Use CDN with token auth
# Generate protected token
python3 << 'EOF'
import hmac
import hashlib
import base64
import time

def generate_protected_token(path, key, hours=1):
    expiry = int(time.time()) + (hours * 3600)
    token = f"path={path}&exp={expiry}"
    sig = hmac.new(key.encode(), token.encode(), hashlib.sha256)
    encoded = base64.urlsafe_b64encode(sig.digest()).decode()
    return f"token={expiry}_{encoded}"

print(generate_protected_token("/video.mp4", "secret-key"))
EOF

# Configure token auth in CDN
# Via portal: CDN > Endpoint > Security > Token Auth
```

### Example 4: Monitoring and Analytics

```bash
# Enable log analytics
az monitor app-insights create \
    --name cdn-analytics \
    --resource-group my-rg

# Configure CDN diagnostic settings
az monitor diagnostic-settings create \
    --name cdn-diagnostics \
    --resource-group my-rg \
    --resource /subscriptions/sub-id/resourceGroups/my-rg/providers/Microsoft.Cdn/profiles/my-cdn/endpoints/my-endpoint \
    --workspace /subscriptions/sub-id/resourceGroups/my-rg/providers/Microsoft.OperationalInsights/workspaces/my-workspace \
    --logs '[
        {"category": "CoreAnalytics", "enabled": true}
    ]' \
    --metrics '[
        {"category": "AllMetrics", "enabled": true}
    ]'

# Query analytics
# In Azure Monitor, run:
# AzureDiagnostics
# | where ResourceType == "CDN"
# | summarize avg(DurationMs) by bin(TimeGenerated, 1h)
```

## COMMON ISSUES

### 1. Origin Not Accessible

**Problem**: CDN can't reach origin.

**Solution**:
```bash
# Check origin health
az cdn origin show \
    --resource-group my-rg \
    --profile-name my-cdn \
    --endpoint-name my-endpoint \
    --origin-name default

# Test from CDN POP
az network traffic-manager profile \
    --dns-name my-endpoint \
    --type routing-method geographic
```

### 2. Mixed Content Issues

**Problem**: HTTPS and HTTP mixed.

**Solution**:
- Redirect HTTP to HTTPS
- Update origin to HTTPS
- Use protocol-relative URLs

### 3. High Bandwidth Costs

**Problem**: CDN costs too much.

**Solution**:
- Increase cache duration
- Enable compression
- Use origin shielding
- Optimize with proper cache headers

## PERFORMANCE

### Performance Benchmarks

| Metric | Typical | Target |
|--------|---------|--------|
| Cache hit ratio | 80%+ | 95%+ |
| First byte | <50ms | <20ms |
| Download | <2s | <500ms |
| Availability | 99.9% | 99.99% |

### Optimization Tips

1. **Pre-populate cache**: Warm up URLs
2. **Cache busting**: Version file names
3. **HTTP/2**: Enable for multiplexing
4. **Brotli**: Better compression

## COMPATIBILITY

### SDK Support

| Language | Library |
|----------|---------|
| Python | azure-mgmt-cdn |
| JavaScript | @azure/arm-cdn |
| .NET | Microsoft.Azure.Management.Cdn |
| Go | github.com/Azure/azure-sdk-for-go |

### Integration

| Service | Integration |
|---------|-------------|
| Storage | Native |
| Media Services | Native |
| App Service | Native |
| Functions | Event-driven |

## CROSS-REFERENCES

### Related Patterns

- Static Website: SPA hosting
- Media Streaming: Video delivery
- Secure Content: Token auth

### What to Study Next

1. Front Door: Global routing
2. Media Services: Video platform
3. WAF: Web application firewall

## EXAM TIPS

### Key Exam Facts

- Static content: Cache long
- Dynamic content: Cache short or bypass
- Origin: Storage or web app
- SSL: Free with custom domain

### Exam Questions

- **Question**: "Cache static files" = Azure CDN
- **Question**: "Reduce origin load" = Cache duration
- **Question**: "Secure downloads" = Token auth
