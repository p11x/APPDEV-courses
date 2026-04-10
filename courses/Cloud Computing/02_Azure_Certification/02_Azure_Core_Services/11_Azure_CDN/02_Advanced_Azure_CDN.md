---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure CDN Advanced
Purpose: Advanced Azure CDN configurations, rules engine, and optimization
Difficulty: advanced
Prerequisites: 01_Basic_Azure_CDN.md
RelatedFiles: 01_Basic_Azure_CDN.md, 03_Practical_Azure_CDN.md
UseCase: Production CDN with custom configurations
CertificationExam: Microsoft Azure Fundamentals - AZ-900
LastUpdated: 2025
---

## WHY

Advanced Azure CDN configurations enable production-grade content delivery with custom routing, optimization, and security features. Understanding these concepts is essential for enterprise deployments.

### Why Advanced Configuration Matters

- **Performance**: Optimize for specific content types
- **Security**: Custom rules and protection
- **Cost**: Reduce bandwidth costs
- **Reliability**: Failover and redundancy
- **Control**: Fine-grained caching

### Advanced Use Cases

- **Multi-origin**: Load balancing
- **Geo-filtering**: Regional restrictions
- **Token Auth**: Secure content
- **A/B Testing**: Traffic splitting

## WHAT

### Rules Engine

```
    RULES ENGINE ARCHITECTURE
    =========================

    Request ──▶ Global Rules
               │
               ▼
        URL Rewrite
        Redirect
        Cache Rules
        Header Mod
               │
               ▼
        Origin Request
               │
               ▼
        Response Rules
        Compression
        Cache Update
               │
               ▼
         CDN Response
```

### Advanced Features

| Feature | Description | Use |
|---------|-------------|-----|
| Geo-filtering | Restrict by region | Licensing |
| Token auth | Signed URLs | Paid content |
| Origin shielding | Additional cache layer | High traffic |
| Query string | Cache variations | Search results |

## HOW

### Example 1: Geo-Filtering

```bash
# Create geo-filtering rule
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --rule-name "BlockNonUS" \
    --match-variable "RemoteAddress" \
    --operator "GeoMatch" \
    --match-values "US" \
    --action-name "AllowRequest" \
    --order 1

# Block specific countries
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --rule-name "BlockGeo" \
    --match-variable "RemoteAddress" \
    --operator "GeoMatch" \
    --match-values "CN RU IN" \
    --action-name "DenyRequest" \
    --order 2

# Allow specific countries only
# Create allow rule first (order 1), then deny all (order 2)
```

### Example 2: URL Rewriting and Redirects

```bash
# URL rewrite rule
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --rule-name "RewriteURL" \
    --match-variable "UrlPath" \
    --operator "Contains" \
    --match-values "/old/" \
    --action-name "UrlRewrite" \
    --source-pattern "/old/(.*)" \
    --destination "/new/$1" \
    --preserve-unmatched false

# Redirect rule
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --rule-name "RedirectHTTP" \
    --match-variable "Scheme" \
    --operator "Equal" \
    --match-values "HTTP" \
    --action-name "UrlRedirect" \
    --redirect-type Found 302 \
    --destination-protocol Https
```

### Example 3: Token Authentication

```bash
# Enable token authentication (via portal or ARM)
# Create token validation rule
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --rule-name "TokenAuth" \
    --match-variable "QueryString" \
    --operator "Contains" \
    --match-values "token=" \
    --action-name "ValidateTokenAuth" \
    --token-auth-rules '{
        "authType": "Standard",
        "tokens": ["token"],
        "paramName": "token",
        "paramIndex": "QueryString"
    }'

# Generate token (server-side)
import hmac
import hashlib
import base64
import time

def generate_token(path, secret, expiry_hours=1):
    expires = int(time.time()) + (expiry_hours * 3600)
    token = f"{path}={expires}"
    signature = hmac.new(
        secret.encode(),
        token.encode(),
        hashlib.sha256
    ).digest()
    
    encoded = base64.urlsafe_b64encode(signature).decode()
    return f"{token}={encoded}"
```

### Example 4: Cache Optimization

```bash
# Set cache key based on query string
az cdn endpoint update \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --query-string-caching-behavior "UniqueQueryStrings"

# Create cache rule for specific content
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --rule-name "LongCache" \
    --match-variable "UrlPath" \
    --operator "EndsWith" \
    --match-values ".jpg .png .css .js" \
    --action-name "CacheExpiration" \
    --cache-behavior "Override" \
    --cache-duration 30d

# Bypass cache for dynamic content
az cdn endpoint rule add \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --rule-name "NoCache" \
    --match-variable "UrlPath" \
    --operator "Contains" \
    --match-values "/api/" \
    --action-name "CacheExpiration" \
    --cache-behavior "BypassCache"
```

## COMMON ISSUES

### 1. Cache Invalidation Not Working

**Problem**: Purged content still served.

**Solution**:
```bash
# Verify purge completed
az cdn endpoint show \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --query "provisioningState"

# Use wildcard purge for complete clear
az cdn endpoint purge \
    --resource-group my-rg \
    --profile-name my-cdn-profile \
    --endpoint-name my-cdn-endpoint \
    --content-paths "/*"
```

### 2. Origin Shield Not Working

**Problem**: Shield not reducing origin traffic.

**Solution**:
- Enable origin shield in portal
- Shield POP should be close to origin
- Wait for propagation

### 3. Rules Not Matching

**Problem**: Rules engine not applying.

**Solution**:
- Check rule order
- Verify conditions match
- Use "Any" for multiple conditions

## PERFORMANCE

### Performance Optimization

| Setting | Recommended | Impact |
|---------|-------------|--------|
| Compression | Enable (gzip/brotli) | 60% size |
| HTTP/2 | Enable | Faster |
| Origin shield | Enable | Reduce origin |
| Query caching | Use-cache | Results cache |

### Monitoring

| Metric | Description |
|--------|-------------|
| Request count | Total requests |
| Hit ratio | Cache hit % |
| Latency | Response time |
| Bandwidth | Data transfer |

## COMPATIBILITY

### Cross-Platform Comparison

| Feature | Azure CDN | AWS CloudFront | GCP Cloud CDN |
|---------|-----------|----------------|---------------|
| Global POPs | 150+ | 200+ | 100+ |
| SSL | Free | Free | Free |
| Rules engine | Yes | Yes | Limited |
| Origin shield | Yes | Yes | Yes |

### Supported File Types

- Images: jpg, png, gif, webp, svg
- Video: mp4, webm, hls
- Documents: pdf, doc, css
- Application: js, wasm

## CROSS-REFERENCES

### Related Services

- Azure Front Door: Dynamic content
- Azure Storage: Origin
- Azure WAF: Security
- Azure Media: Video

### Prerequisites

- Basic CDN concepts
- Azure subscription
- Storage or web origin

### What to Study Next

1. Practical CDN: Implementation
2. Front Door: Hybrid content
3. Media Services: Video streaming

## EXAM TIPS

### Key Exam Facts

- Geo-filtering: Restrict by country
- Origin shield: Additional caching layer
- Token auth: Signed URLs
- Rules engine: Custom routing

### Exam Questions

- **Question**: "Block by country" = Geo-filtering
- **Question**: "Reduce origin calls" = Origin shield
- **Question**: "Secure content" = Token authentication
