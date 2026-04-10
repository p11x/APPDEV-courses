---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudTrail Audit
Purpose: Hands-on CloudTrail implementation including compliance monitoring and security automation
Difficulty: intermediate
Prerequisites: 01_Basic_CloudTrail.md, 02_Advanced_CloudTrail.md
RelatedFiles: 01_Basic_CloudTrail.md, 02_Advanced_CloudTrail.md
UseCase: Production audit and compliance deployment
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## 💡 WHY

Hands-on CloudTrail implementation provides practical experience building audit, compliance, and security automation systems.

## 📖 WHAT

### Lab: Security Monitoring Pipeline

```
CloudTrail Events → EventBridge → Lambda → Security Team
                                           ↓
                                    SNS Alert
                                           ↓
                                    Ticket System
```

## 🔧 HOW

### Module 1: Configure Enterprise Trail

```bash
#!/bin/bash
# CloudTrail Security Setup

# Create S3 bucket for CloudTrail
aws s3 mb s3://enterprise-trail-logs-123456789

# Enable bucket versioning
aws s3api put-bucket-versioning \
    --bucket enterprise-trail-logs-123456789 \
    --versioning-configuration Status=Enabled

# Enable encryption
aws s3api put-bucket-encryption \
    --bucket enterprise-trail-logs-123456789 \
    --server-side-encryption-configuration '{
        "Rules": [{
            "ApplyServerSideEncryptionByDefault": {
                "SSEAlgorithm": "AES256"
            }
        }]
    }'

# Create organizational trail
aws cloudtrail create-trail \
    --name enterprise-trail \
    --s3-bucket-name enterprise-trail-logs-123456789 \
    --s3-key-prefix "security-audit" \
    --is-organization-trail \
    --include-global-service-events \
    --is-multi-region-trail
```

### Module 2: Security Event Rules

```bash
# Create EventBridge rule for IAM changes
aws events put-rule \
    --name "iam-security-changes" \
    --event-pattern '{
        "source": ["aws.iam"],
        "detail-type": ["AWS API Call via CloudTrail"],
        "detail": {
            "eventSource": ["iam.amazonaws.com"],
            "eventName": ["CreateUser", "DeleteUser", "CreateGroup", "AttachUserPolicy", "PutUserPolicy"]
        }
    }'

# Create rule for console logins
aws events put-rule \
    --name "console-login-alerts" \
    --event-pattern '{
        "source": ["aws.signin"],
        "detail-type": ["AWS Console Sign In via CloudTrail"],
        "detail": {
            "userIdentity": {
                "type": ["Root", "IAMUser"]
            }
        }
    }'

# Create rule for root account usage
aws events put-rule \
    --name "root-account-usage" \
    --event-pattern '{
        "source": ["aws.signin"],
        "detail-type": ["AWS API Call via CloudTrail"],
        "detail": {
            "userIdentity": {
                "type": ["Root"]
            }
        }
    }'
```

### Module 3: Alert Processing Lambda

```javascript
// index.js - Security Alert Handler
const AWS = require('aws-sdk');
const sns = new AWS.SNS();

exports.handler = async (event) => {
    console.log('Received event:', JSON.stringify(event));
    
    const detail = event.detail;
    const alertMessage = `Security Alert: ${detail.eventName} by ${detail.userIdentity.arn}`;
    
    // Send alert to security team
    await sns.publish({
        TopicArn: 'arn:aws:sns:us-east-1:123456789:security-alerts',
        Subject: `AWS Security Alert: ${detail.eventName}`,
        Message: JSON.stringify({
            Event: detail.eventName,
            User: detail.userIdentity.arn,
            Time: detail.eventTime,
            SourceIP: detail.sourceIPAddress,
            Account: detail.recipientAccountId
        }, null, 2)
    }).promise();
    
    return { statusCode: 200 };
};
```

```bash
# Create Lambda function
aws lambda create-function \
    --function-name security-alert-handler \
    --runtime nodejs18.x \
    --handler index.handler \
    --zip-file fileb://alert-handler.zip \
    --role arn:aws:iam::123456789:role/lambda-security-role

# Add permissions
aws lambda add-permission \
    --function-name security-alert-handler \
    --source-arn arn:aws:events:us-east-1:123456789:rule/iam-security-changes \
    --principal events.amazonaws.com \
    --action lambda:InvokeFunction \
    --statement-id events

# Add targets to rules
aws events put-targets \
    --rule "iam-security-changes" \
    --targets '[{"Id": "lambda", "Arn": "arn:aws:lambda:us-east-1:123456789:function:security-alert-handler"}]'
```

## VERIFICATION

```bash
# Check trail status
aws cloudtrail get-trail-status --name enterprise-trail

# Look up recent events
aws cloudtrail lookup-events \
    --lookup-attributes '[{"AttributeKey": "EventSource", "AttributeValue": "iam.amazonaws.com"}]'

# Check EventBridge rules
aws events list-rules --name-prefix "security"
```

## CLEANUP

```bash
aws cloudtrail delete-trail --name enterprise-trail
aws events delete-rule --name "iam-security-changes" --force
aws lambda delete-function --function-name security-alert-handler
```

## 🔗 CROSS-REFERENCES

**Related**: CloudWatch, IAM, SNS