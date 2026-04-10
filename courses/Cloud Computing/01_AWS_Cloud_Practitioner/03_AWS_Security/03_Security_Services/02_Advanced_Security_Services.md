---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Security Services - Advanced
Purpose: Advanced AWS security services including GuardDuty, Security Hub, and WAF configuration
Difficulty: advanced
Prerequisites: 01_Basic_Security_Services.md
RelatedFiles: 01_Basic_Security_Services.md, 03_Practical_Security_Services.md
UseCase: Enterprise security monitoring and threat detection
CertificationExam: AWS Certified Cloud Practitioner - Domain 2
LastUpdated: 2025
---

## WHY

Enterprise security requires advanced threat detection, centralized security management, and automated response capabilities. Understanding advanced security services enables organizations to build comprehensive security architectures.

### Why Advanced Security Matters

- **Threat Detection**: Identify sophisticated attacks
- **Centralized Management**: Security Hub aggregation
- **Automated Response**: Event-driven remediation
- **Continuous Monitoring**: Real-time alerts

### Advanced Use Cases

- **Multi-account Security**: Organization-wide security
- **Threat Intelligence**: Custom threat feeds
- **SIEM Integration**: Third-party SIEM
- **Compliance Reporting**: Automated audits

## WHAT

### Advanced Security Services

| Service | Function | Use Case |
|---------|---------|---------|
| GuardDuty | Threat detection | Malware, reconnaissance |
| Security Hub | Centralized view | Multi-service aggregation |
| WAF | Web application firewall | SQL injection, XSS |
| Shield | DDoS protection | Layer 3/4/7 attacks |
| Detective | Investigation | Security analysis |

### Cross-Platform Comparison

| Capability | AWS | Azure | GCP |
|-----------|-----|-------|-----|
| Threat Detection | GuardDuty | Defender | Chronicle |
| WAF | WAF | Application Gateway | Cloud Armor |
| DDoS Protection | Shield | DDoS Protection | Cloud Armor |
| Centralized Hub | Security Hub | Security Center | Security Command Center |

### GuardDuty Findings

| Finding Type | Severity | Description |
|------------|----------|------------|
| Backdoor:EC2 | High | Compromised instance |
| CryptoMiner | High | Cryptocurrency mining |
| Exfiltration | Medium | Data exfiltration |
| Recon | Low | Reconnaissance |

## HOW

### Example 1: GuardDuty Configuration

```bash
# Enable GuardDuty
aws guardduty create-detector \
    --finding-publishing-frequency "ONE_HOUR" \
    --data-sources '{
        "cloudTrail": {"status": "ENABLED"},
        "dnsLogs": {"status": "ENABLED"},
        "vpcFlowLogs": {"status": "ENABLED"},
        "s3Logs": {"status": "ENABLED"}
    }'

# Get detector
DETECTOR_ID=$(aws guardduty list-detectors --query 'DetectorIds[0]' --output text)

# Enable additional data sources
aws guardduty update-detector \
    --detector-id $DETECTOR_ID \
    --finding-publishing-frequency "FIFTEEN_MINUTES" \
    --enable

# Create trusted IP list
aws guardduty create-ip-set \
    --detector-id $DETECTOR_ID \
    --name "trusted-ips" \
    --format TXT \
    --location s3://my-bucket/trusted-ips.txt

# Create threat intel set
aws guardduty create-threat-intel-set \
    --detector-id $DETECTOR_ID \
    --name "custom-threats" \
    --format TXT \
    --location s3://my-bucket/threats.txt \
    --activate

# Get findings
aws guardduty list-findings \
    --detector-id $DETECTOR_ID \
    --finding-criteria '{
        "severity": [{"eq": ["HIGH"]}]
    }'
```

### Example 2: Security Hub Integration

```bash
# Enable Security Hub
aws securityhub enable-security-hub \
    --enable-default-standards

# Enable standards
aws securityhub update-standards-control \
    --security-hub-arn arn:aws:securityhub:us-east-1:123456789:standards/aws-foundational-security-best-practices/1.0.0 \
    --control-id "IAM.1" \
    --control-status ENABLED

# Enable automatic findings
aws securityhub enable-import-findings-for-product \
    --product-arn arn:aws:securityhub:us-east-1:123456789:product/aws/config

# Enable AWS GuardDuty integration
aws securityhub enable-import-findings-for-product \
    --product-arn arn:aws:securityhub:us-east-1:123456789:product/aws/guardduty

# Create custom action
aws securityhub create-action-target \
    --name "Escalate-To-Security-Team" \
    --description "Escalate to security team"

# Get findings with filters
aws securityhub get-findings \
    --filters '{
        "SeverityLabel": [{"eq": ["CRITICAL", "HIGH"]}],
        "RecordState": [{"eq": ["ACTIVE"]}]
    }'
```

### Example 3: WAF Advanced Configuration

```bash
# Create WAF Web ACL
aws wafv2 create-web-acl \
    --name "production-web-acl" \
    --scope REGIONAL \
    --visibility-config '{
        "sampledRequestsEnabled": true,
        "cloudWatchMetricsEnabled": true,
        "metricName": "production-metrics"
    }' \
    --default-action '{
        "allow": {}
    }'

WEB_ACL_ID=$(aws wafv2 list-web-acls --scope REGIONAL --query 'WebACLs[0].Id' --output text)
WEB_ACL_TOKEN=$(aws wafv2 list-web-acls --scope REGIONAL --query 'WebACLs[0].Token' --output text)

# Add rule for SQL injection
aws wafv2.create_rule \
    --name "sql-injection-rule" \
    --scope REGIONAL \
    --visibility-config '{
        "sampledRequestsEnabled": true,
        "cloudWatchMetricsEnabled": true,
        "metricName": "sql-metrics"
    }'

aws wafv2 put-rule \
    --rule '{
        "Name": "sql-injection",
        "Id": "sql-id",
        "Statement": {
            "SqliMatchStatement": {
                "FieldToMatch": {"Body": {}},
                "TextTransformations": [{"Priority": 0, "Type": "NONE"}]
            }
        },
        "VisibilityConfig": {"SampledRequestsEnabled": true}
    }'

# Add rule for IP rate limiting
aws wafv2 put-rule \
    --rule '{
        "Name": "rate-limit",
        "Statement": {
            "RateBasedStatement": {
                "Key": "IP",
                "Limit": 100
            }
        }
    }'

# Attach rules to Web ACL
aws wafv2 put-web-acl-rulings \
    --web-acl-arn $WEB_ACL_ARN \
    --rules '[
        {
            "Name": "sql-injection",
            "Priority": 1,
            "Statement": {"SqliMatchStatement": {...}},
            "Action": {"Block": {}},
            "VisibilityConfig": {...}
        }
    ]'
```

## COMMON ISSUES

### 1. Too Many GuardDuty Findings

**Problem**: High volume of findings.

**Solution**:
- Filter by severity
- Use auto-suppression
- Create trusted IP lists

### 2. WAF Rules Blocking Legitimate Traffic

**Problem**: False positives.

**Solution**:
- Use CloudWatch to review requests
- Add exceptions for known patterns
- Adjust rule sensitivity

### 3. Security Hub Not Aggregating

**Problem**: Missing findings.

**Solution**:
```bash
# Verify integration enabled
aws securityhub list-enabled-products-for-import

# Re-enable integration
aws securityhub enable-import-findings-for-product \
    --product-arn ProductArn
```

### 4. GuardDuty Findings Not Appearing

**Problem**: No findings.

**Solution**:
- Verify data sources enabled
- Check CloudTrail status
- Wait for evaluation cycle

### 5. WAF Cost Creep

**Problem**: High WAF costs.

**Solution**:
- Use rate-based rules
- Filter CloudWatch metrics
- Enable only needed rules

## PERFORMANCE

### Performance Metrics

| Service | Latency | Cost per Million |
|--------|--------|---------------|
| GuardDuty | ~15 min | $0.002-0.004 |
| WAF | Real-time | $0.006-0.010 |
| Shield | Real-time | Flat + usage |

### Optimization

| Technique | Savings |
|-----------|---------|
| Rate limiting | 90% false positive reduction |
| Trusted IPs | 60% fewer findings |
| Custom rules | 40% cost reduction |

## COMPATIBILITY

### Integration Matrix

| Service | CloudWatch | EventBridge | Lambda |
|---------|-----------|------------|--------|
| GuardDuty | Yes | Yes | Yes |
| Security Hub | Yes | Yes | Yes |
| WAF | Yes | Yes | Yes |
| Shield | Yes | Yes | N/A |

### Supported Regions

| Region | GuardDuty | Security Hub | WAF |
|--------|----------|------------|-----|
| us-east-1 | Yes | Yes | Yes |
| us-west-2 | Yes | Yes | Yes |
| eu-west-1 | Yes | Yes | Yes |

## CROSS-REFERENCES

### Related Services

- CloudWatch: Monitoring
- EventBridge: Automation
- IAM: Access control

### Prerequisites

- Basic Security Services
- Networking fundamentals

### What to Study Next

1. Practical Security Services: Implementation
2. Compliance Programs: Audit
3. Cost Management: Security costs

## EXAM TIPS

### Key Exam Facts

- GuardDuty: Automated threat detection
- Security Hub: Central aggregation
- WAF: Layer 7 protection
- Shield: DDoS protection

### Exam Questions

- **Question**: "Detect compromised EC2" = GuardDuty
- **Question**: "Block SQL injection" = WAF rule
- **Question**: "Aggregate security findings" = Security Hub
- **Question**: "DDoS protection" = Shield