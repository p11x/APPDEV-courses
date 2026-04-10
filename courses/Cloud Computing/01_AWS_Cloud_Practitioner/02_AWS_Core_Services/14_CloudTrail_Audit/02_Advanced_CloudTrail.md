---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudTrail Audit
Purpose: Advanced CloudTrail configuration including organizational trails, event selectors, and security automation
Difficulty: advanced
Prerequisites: 01_Basic_CloudTrail.md
RelatedFiles: 01_Basic_CloudTrail.md, 03_Practical_CloudTrail.md
UseCase: Enterprise audit and compliance
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## 💡 WHY

Advanced CloudTrail features enable enterprise-wide auditing, compliance automation, and security monitoring.

## 📖 WHAT

### Advanced Features

**Organizational Trails**: Single trail across all accounts

**Event Selectors**: Fine-grained event logging control

**Insights Events**: Detect unusual API activity

**Integration**: Security Hub, EventBridge, Lambda

### Cross-Platform Comparison

| Feature | AWS CloudTrail | Azure Activity Log | GCP Audit Logs | Splunk |
|---------|----------------|---------------------|----------------|--------|
| API Logging | Yes | Yes | Yes | Yes |
| Organization Trails | Yes | Azure Lighthouse | Org policies | Via integration |
| Insights | Yes | Anomaly detection | AI/ML | ML app |
| Event Bridge | Yes | Yes | Yes | Yes |
| Retention | 90 days (history) | 90 days | 400 days | Configurable |
| Log Encryption | Yes (SSE-KMS) | Yes | Yes | Yes |

## 🔧 HOW

### Example 1: Organizational Trail

```bash
# Enable CloudTrail in master account
aws cloudtrail create-trail \
    --name org-trail \
    --s3-bucket-name org-trail-logs \
    --is-organization-trail \
    --include-global-service-events \
    --is-multi-region-trail

aws cloudtrail start-logging \
    --name org-trail

# In member accounts, trail appears automatically
# SCP can enforce organization trail usage
```

### Example 2: Event Selectors

```bash
# Create trail with read-only and write events
aws cloudtrail create-trail \
    --name detailed-trail \
    --s3-bucket-name detailed-logs \
    --s3-prefix cloudtrail-detail

aws cloudtrail put-event-selectors \
    --trail-name detailed-trail \
    --event-selectors '[
        {
            "ReadWriteType": "ReadOnly",
            "IncludeManagementEvents": true,
            "DataResources": [
                {
                    "Type": "AWS::S3::Object",
                    "Values": ["arn:aws:s3:::my-bucket/*"]
                }
            ]
        },
        {
            "ReadWriteType": "WriteOnly",
            "IncludeManagementEvents": true
        }
    ]'
```

### Example 3: CloudTrail + EventBridge + Lambda

```bash
# Create rule to catch security group changes
aws events put-rule \
    --name "security-group-changes" \
    --event-pattern '{
        "source": ["aws.ec2"],
        "detail-type": ["AWS API Call via CloudTrail"],
        "detail": {
            "eventSource": ["ec2.amazonaws.com"],
            "eventName": ["AuthorizeSecurityGroupIngress", "AuthorizeSecurityGroupEgress", "RevokeSecurityGroupIngress", "RevokeSecurityGroupEgress"]
        }
    }'

# Add Lambda target
aws events put-targets \
    --rule "security-group-changes" \
    --targets '[{"Id": "lambda-target", "Arn": "arn:aws:lambda:us-east-1:123456789:function:security-alert"}]'

# Lambda function processes security group changes
```

## ⚠️ COMMON ISSUES

### 1. Not Receiving Events

**Problem**: CloudTrail not showing events

**Solution**: Verify trail is active, check S3 bucket permissions, ensure region matches

### 2. High Costs

**Problem**: Unexpected CloudTrail charges

**Solution**: Use event selectors to filter events, enable Insights only where needed

### 3. Cross-Account Access

**Problem**: Cannot access trails in member accounts

**Solution**: Use organizational trails, ensure proper S3 bucket policies

## 🏃 PERFORMANCE

### Limits

| Feature | Limit |
|---------|-------|
| Trails per region | 5 |
| Event selectors per trail | 5 |
| Data events per selector | 50 |
| Log file size | 10MB |

## 🔗 CROSS-REFERENCES

**Related**: CloudWatch, Security Hub, Config, GuardDuty

**Prerequisite**: Basic CloudTrail understanding

## ✅ EXAM TIPS

- Organizational trail logs across all accounts
- Event selectors filter what gets logged
- Insights detect anomalous API activity
- CloudTrail + EventBridge enables automation
- Integration with Security Hub for centralized visibility