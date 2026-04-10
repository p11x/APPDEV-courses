---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudTrail Audit
Purpose: Understanding AWS CloudTrail for audit logging and governance
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_CloudTrail.md, 03_Practical_CloudTrail.md
UseCase: Tracking API activity and security auditing
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## 💡 WHY

AWS CloudTrail provides audit logging of API calls across AWS services. It's essential for security monitoring, compliance, and troubleshooting.

### Why CloudTrail Matters

- **Compliance**: Required for many frameworks
- **Security**: Detect unauthorized access
- **Troubleshooting**: Find root cause of issues
- **Governance**: Track changes over time
- **Forensics**: Investigate security incidents

### Industry Use Cases

- SOC 2 compliance
- PCI-DSS auditing
- Security incident investigation
- Change management

## 📖 WHAT

### CloudTrail Core Concepts

**Trail**: Configuration for log delivery

**Event**: API call record (management, data, insight)

**Log File**: Encrypted S3 object with events

**Event History**: Last 90 days of events

### Event Types

| Type | Description |
|------|-------------|
| Management | Create, modify, delete resources |
| Data | S3 object-level operations |
| Insights | Unusual API activity |

### Architecture Diagram

```
CloudTrail Architecture
========================

API Calls          CloudTrail           S3
┌─────────┐       ┌──────────┐        ┌────────┐
│  User   │──────►│  Cloud   │───────►│  S3    │
│  App    │       │  Trail   │        │  Bucket│
│ Service │       └──────────┘        └────────┘
└─────────┘              │
                         ▼
                  ┌──────────────┐
                  │ CloudWatch   │
                  │ Logs          │
                  └──────────────┘
```

## 🔧 HOW

### Example 1: Create Trail

```bash
# Create S3 bucket for logs
aws s3 mb s3://my-cloudtrail-logs

# Create trail
aws cloudtrail create-trail \
    --name my-trail \
    --s3-bucket-name my-cloudtrail-logs \
    --is-multi-region-trail \
    --include-global-service-events

# Start logging
aws cloudtrail start-logging \
    --name my-trail

# Create organization trail (if using AWS Org)
aws cloudtrail create-trail \
    --name org-trail \
    --s3-bucket-name my-cloudtrail-logs \
    --is-organization-trail
```

### Example 2: Lookup Events

```bash
# Lookup events by time range
aws cloudtrail lookup-events \
    --start-time 2024-01-15T00:00:00Z \
    --end-time 2024-01-15T23:59:59Z

# Lookup by attribute
aws cloudtrail lookup-events \
    --lookup-attributes '[
        {"AttributeKey": "EventSource", "AttributeValue": "ec2.amazonaws.com"},
        {"AttributeKey": "EventName", "AttributeValue": "RunInstances"}
    ]'

# Get specific user activity
aws cloudtrail lookup-events \
    --lookup-attributes '[
        {"AttributeKey": "Username", "AttributeValue": "admin"}
    ]'
```

### Example 3: CloudWatch Integration

```bash
# Create CloudWatch log group
aws logs create-log-group \
    --log-group-name cloudtrail-events

# Update trail to send to CloudWatch
aws cloudtrail update-trail \
    --name my-trail \
    --cloud-watch-logs-log-group-arn arn:aws:logs:us-east-1:123456789:log-group:cloudtrail-events \
    --cloud-watch-logs-role-arn arn:aws:iam::123456789:role/CloudTrailRole

# Create IAM role for CloudWatch (if needed)
aws iam create-role \
    --role-name CloudTrailRole \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"Service": "cloudtrail.amazonaws.com"},
            "Action": "sts:AssumeRole"
        }]
    }'

aws iam put-role-policy \
    --role-name CloudTrailRole \
    --policy-name CloudTrailPolicy \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": ["logs:CreateLogStream", "logs:PutLogEvents"],
            "Resource": "arn:aws:logs:us-east-1:123456789:log-group:cloudtrail-events:*"
        }]
    }'
```

### Example 4: SNS Notification

```bash
# Create SNS topic
aws sns create-topic \
    --name cloudtrail-alerts

# Update trail with SNS
aws cloudtrail update-trail \
    --name my-trail \
    --sns-topic-arn arn:aws:sns:us-east-1:123456789:cloudtrail-alerts
```

## ⚠️ COMMON ISSUES

### 1. Missing Events

**Problem**: Not seeing all events.

**Solution**: Ensure trail covers all regions and includes global services.

### 2. Log Delivery Delays

**Problem**: Logs delayed in S3.

**Solution**: Check S3 bucket permissions, verify trail is active.

### 3. High Costs

**Problem**: Unexpected CloudTrail costs.

**Solution**: Use organization trails, filter events, manage log retention.

### 4. Encryption Issues

**Problem**: Cannot decrypt logs.

**Solution**: Verify KMS key permissions, check key is active.

## 🏃 PERFORMANCE

### Limits

| Feature | Default |
|---------|---------|
| Trails per region | 5 |
| Event history | 90 days |
| S3 object size | 10MB |

## 🌐 COMPATIBILITY

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Audit Logging | CloudTrail | Activity Log | Cloud Audit Logs |
| S3 Storage | Yes | Yes | Yes |
| CloudWatch | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

**Related**: CloudWatch, Config, IAM, GuardDuty

**Prerequisite**: IAM basics

**Next**: AWS Config for resource configuration tracking

## ✅ EXAM TIPS

- CloudTrail records API calls
- Management events: default on
- Data events: S3/lambda additional cost
- Insights: anomalous activity detection
- Organization trail applies to all accounts