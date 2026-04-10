---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Security Services
Purpose: Understanding AWS security services including WAF, Shield, GuardDuty, and Macie
Difficulty: intermediate
Prerequisites: 01_Basic_IAM.md, 01_Basic_Shared_Responsibility.md
RelatedFiles: 02_Advanced_Security_Services.md, 03_Practical_Security_Services.md
UseCase: Implementing comprehensive security controls
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## 💡 WHY

AWS security services provide layered protection for applications. Understanding these services is essential for building secure cloud architectures.

### Why Security Services Matter

- **Protection**: Multiple layers of defense
- **Detection**: Identify threats automatically
- **Compliance**: Meet security frameworks
- **Managed**: AWS handles infrastructure

### Key Services

- **WAF**: Web application firewall
- **Shield**: DDoS protection
- **GuardDuty**: Threat detection
- **Macie**: Data classification
- **Inspector**: Vulnerability scanning

## 📖 WHAT

### Security Services Overview

| Service | Purpose | Type |
|---------|---------|------|
| WAF | Filter malicious web traffic | Preventive |
| Shield | DDoS mitigation | Preventive |
| GuardDuty | Threat detection | Detective |
| Macie | Data discovery | Detective |
| Inspector | Vulnerability scanning | Detective |
| Security Hub | Security posture | Governance |

### Architecture Diagram

```
AWS Security Services Architecture
===================================

Internet ─────► Shield (DDoS) ──► CloudFront/ALB
                                    │
                                    ▼
                              WAF (Filtering)
                                    │
                                    ▼
                              Your App

Data ─────► Macie (Classification)
Events ──► GuardDuty (Detection)
```

## 🔧 HOW

### Example 1: AWS WAF

```bash
# Create web ACL
aws wafv2 create-web-acl \
    --name my-web-acl \
    --scope CLOUDFRONT \
    --default-action '{
        "Block": {}
    }' \
    --visibility-config '{
        "SampledRequestsEnabled": true,
        "CloudWatchMetricsEnabled": true,
        "MetricName": "my-web-acl-metric"
    }'

# Add IP set
aws wafv2 create-ip-set \
    --name blocked-ips \
    --scope CLOUDFRONT \
    --addresses "192.0.2.0/24" \
    --ip-address-version IPV4

# Add rule
aws wafv2 create-rule \
    --name block-bad-ips \
    --scope CLOUDFRONT \
    --statements '{
        "ipSetReferenceStatement": {
            "ARN": "arn:aws:wafv2:us-east-1:123456789:ip-set/blocked-ips"
        }
    }' \
    --action '{
        "Block": {}
    }'

# Associate with resource
aws wafv2 associate-web-acl \
    --web-acl-arn arn:aws:wafv2:us-east-1:123456789:web-acl/my-web-acl \
    --resource-arn arn:aws:cloudfront::123456789:distribution/ABCD123456
```

### Example 2: AWS Shield

```bash
# Shield is enabled by default for CloudFront and Route 53
# Enable for EC2, ELB, etc. requires Shield Advanced

# Enable Shield Advanced (via console or support)
# Configure protected resources

# Create health-based detection
aws wafv2 create-rule-group \
    --name shield-rule-group \
    --scope REGIONAL \
    --capacity 100
```

### Example 3: GuardDuty

```bash
# Enable GuardDuty
aws guardduty enable-detector \
    --detector-id detector-id

# Get findings
aws guardduty list-findings \
    --detector-id detector-id \
    --filter '{"severityEqual": "2"}'

# Get specific finding
aws guardduty get-findings \
    --detector-id detector-id \
    --finding-ids "finding-id"

# Archive findings
aws guardduty archive-findings \
    --detector-id detector-id \
    --finding-ids "finding-id"
```

### Example 4: Macie

```bash
# Enable Macie (via Amazon Macie console)
# Create job for data discovery

# Create classification job
aws macie2 create-classification-job \
    --name "s3-data-scan" \
    --job-type SCHEDULED \
    --s3-job-definition '{
        "bucketDefinitions": [
            {
                "buckets": ["my-data-bucket"]
            }
        ]
    }'

# Get findings
aws macie2 list-findings \
    --finding-criteria '{"severity": ["High", "Medium"]}'
```

## ⚠️ COMMON ISSUES

### 1. WAF Rules Too Restrictive

**Problem**: Blocking legitimate traffic.

**Solution**: Test rules in count mode before blocking.

### 2. GuardDuty Too Many Findings

**Problem**: Alert fatigue.

**Solution**: Use filters, suppress known patterns, enable auto-remediation.

### 3. Macie Costs High

**Problem**: Large data scans cost money.

**Solution**: Limit scan scope, use exclude patterns.

### 4. Shield Not Covering All Resources

**Problem**: Some resources not protected.

**Solution**: Add resources to Shield protection, consider Shield Advanced.

## 🏃 PERFORMANCE

### Cost Considerations

| Service | Pricing |
|---------|---------|
| WAF | $5/rule/month + requests |
| Shield Advanced | $3,000/month |
| GuardDuty | $3-6 per million events |
| Macie | Scanning based |

## 🌐 COMPATIBILITY

| Service | Integration |
|---------|-------------|
| WAF | CloudFront, ALB, API Gateway |
| Shield | All AWS services |
| GuardDuty | Security Hub, EventBridge |
| Macie | S3, EventBridge |

## 🔗 CROSS-REFERENCES

**Related**: IAM, VPC Security Groups, CloudTrail

**Prerequisites**: Basic security concepts

**Next**: Security Hub for unified view

## ✅ EXAM TIPS

- WAF: Create rules for common attacks (SQLi, XSS)
- Shield Standard: Free, auto-enabled
- Shield Advanced: Additional features + cost
- GuardDuty: Machine learning based detection
- Macie: Sensitive data discovery