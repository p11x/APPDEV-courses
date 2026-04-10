---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: SNS Notifications
Purpose: Hands-on SNS implementation including event-driven architectures and notification systems
Difficulty: intermediate
Prerequisites: 01_Basic_SNS.md, 02_Advanced_SNS.md
RelatedFiles: 01_Basic_SNS.md, 02_Advanced_SNS.md
UseCase: Production notification architecture deployment
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## 💡 WHY

Hands-on SNS implementation provides practical experience building event-driven notification systems.

## 📖 WHAT

### Lab: Order Processing Pipeline

```
┌──────────┐    ┌──────────┐    ┌──────────┐    ┌──────────┐
│   API    │───►│   SNS    │───►│ Lambda   │───►│ Process  │
└──────────┘    │  Topic   │    │ Multiple │    │ Results  │
                └──────────┘    └──────────┘    └──────────┘
                      │
         ┌────────────┼────────────┐
         ▼            ▼            ▼
    ┌─────────┐  ┌─────────┐  ┌─────────┐
    │ SQS     │  │ Email   │  │ SMS    │
    │ Queue   │  │        │  │        │
    └─────────┘  └─────────┘  └─────────┘
```

## 🔧 HOW

### Module 1: Create SNS Topic Architecture

```bash
#!/bin/bash
# SNS Order Processing Setup

# Create topic
TOPIC_ARN=$(aws sns create-topic \
    --name order-events \
    --attributes '{
        "DisplayName": "Order Events",
        "TopicArn": "arn:aws:sns:us-east-1:123456789:order-events"
    }' \
    --query 'TopicArn' \
    --output text)

echo "Topic created: $TOPIC_ARN"

# Create Lambda functions for different handlers
aws lambda create-function \
    --function-name order-processor \
    --runtime nodejs18.x \
    --handler index.handler \
    --zip-file fileb://processor.zip \
    --role arn:aws:iam::123456789:role/lambda-exec

aws lambda create-function \
    --function-name order-notifier \
    --runtime nodejs18.x \
    --handler index.handler \
    --zip-file fileb://notifier.zip \
    --role arn:aws:iam::123456789:role/lambda-exec
```

### Module 2: Subscribe Multiple Endpoints

```bash
# Subscribe SQS queue for order processing
SQS_ARN=$(aws sqs get-queue-attributes \
    --queue-url https://sqs.us-east-1.amazonaws.com/123456789/orders-queue \
    --attribute-names QueueArn \
    --query 'Attributes.QueueArn')

aws sns subscribe \
    --topic-arn $TOPIC_ARN \
    --protocol sqs \
    --notification-endpoint $SQS_ARN \
    --attributes '{
        "FilterPolicy": "{\"orderType\": [\"standard\", \"express\"]}"
    }'

# Subscribe Lambda for notifications
aws sns subscribe \
    --topic-arn $TOPIC_ARN \
    --protocol lambda \
    --notification-endpoint arn:aws:lambda:us-east-1:123456789:function:order-processor

# Subscribe email for high-value orders
aws sns subscribe \
    --topic-arn $TOPIC_ARN \
    --protocol email \
    --notification-endpoint admin@example.com
```

### Module 3: Publish Events

```bash
# Publish standard order
aws sns publish \
    --topic-arn $TOPIC_ARN \
    --subject "New Order Received" \
    --message '{"orderId": "ORD-001", "customerId": "CUST-123", "orderType": "standard", "total": 99.99}' \
    --message-attributes '{
        "orderType": {"DataType": "String", "StringValue": "standard"},
        "total": {"DataType": "Number", "StringValue": "99.99"}
    }'

# Publish express order (filtered to different handler)
aws sns publish \
    --topic-arn $TOPIC_ARN \
    --subject "Express Order" \
    --message '{"orderId": "ORD-002", "customerId": "CUST-456", "orderType": "express", "total": 299.99}' \
    --message-attributes '{
        "orderType": {"DataType": "String", "StringValue": "express"}
    }'
```

## VERIFICATION

```bash
# Check subscription status
aws sns list-subscriptions-by-topic \
    --topic-arn $TOPIC_ARN

# Check CloudWatch metrics
aws cloudwatch get-metric-statistics \
    --namespace AWS/SNS \
    --metric-name NumberOfMessagesPublished \
    --start-time 2024-01-01T00:00:00Z \
    --end-time 2024-01-01T01:00:00Z \
    --period 3600 \
    --statistics Sum
```

## CLEANUP

```bash
# Delete topic
aws sns delete-topic --topic-arn $TOPIC_ARN

# Delete subscriptions
aws sns unsubscribe --subscription-arn <subscription-arn>
```

## 🔗 CROSS-REFERENCES

**Related**: SQS, Lambda, EventBridge