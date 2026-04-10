---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: SNS Notifications
Purpose: Advanced SNS configuration including fanout patterns, message filtering, and cross-region delivery
Difficulty: advanced
Prerequisites: 01_Basic_SNS.md
RelatedFiles: 01_Basic_SNS.md, 03_Practical_SNS.md
UseCase: Enterprise notification architectures
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## 💡 WHY

Advanced SNS features enable sophisticated pub/sub architectures with message filtering, cross-region replication, and enterprise integrations.

## 📖 WHAT

### Advanced Features

**Message Filtering**: Filter messages based on attributes

**Cross-Region Publishing**: Replicate topics across regions

**FIFO Topics**: Preserve message ordering

**Message Transformations**: Raw message delivery to Lambda

**Encryption**: Server-side encryption with KMS

### Cross-Platform Comparison

| Feature | AWS SNS | Azure Event Grid | GCP Pub/Sub | Apache Kafka |
|---------|---------|------------------|-------------|--------------|
| Pub/Sub | Yes | Yes | Yes | Yes |
| Filtering | Yes | Yes | Yes | Yes |
| FIFO | Yes | Yes | Yes | Yes |
| Message Ordering | Best effort | Yes | Yes | Yes |
| HTTP Push | Yes | Yes | Yes | No |
| Message Size | 256KB | 1MB | 10MB | 1MB |
| Retention | 13 months | 7 days | 7 days | Configurable |

## 🔧 HOW

### Example 1: Message Filtering

```bash
# Create topic with filtering
aws sns create-topic \
    --name filtered-orders \
    --attributes '{
        "FifoTopic": "false"
    }'

# Subscribe with filter policy
aws sns subscribe \
    --topic-arn arn:aws:sns:us-east-1:123456789:filtered-orders \
    --protocol lambda \
    --notification-endpoint arn:aws:lambda:us-east-1:123456789:function:order-processor \
    --filter-policy '{
        "eventType": ["order_placed", "order_shipped"],
        "region": ["us-east-1", "us-west-2"]
    }'
```

### Example 2: Fanout to Multiple SQS Queues

```bash
# Create SQS queues for different consumers
QUEUE1_URL=$(aws sqs create-queue --queue-name analytics-queue --query 'QueueUrl')
QUEUE2_URL=$(aws sqs create-queue --queue-name fulfillment-queue --query 'QueueUrl')
QUEUE3_URL=$(aws sqs create-queue --queue-name notification-queue --query 'QueueUrl')

# Create SNS topic
TOPIC_ARN=$(aws sns create-topic --name orders-fanout --query 'TopicArn')

# Get queue ARNs
QUEUE1_ARN=$(aws sqs get-queue-attributes --queue-url $QUEUE1_URL --attribute-names QueueArn --query 'Attributes.QueueArn')
QUEUE2_ARN=$(aws sqs get-queue-attributes --queue-url $QUEUE2_URL --attribute-names QueueArn --query 'Attributes.QueueArn')
QUEUE3_ARN=$(aws sqs get-queue-attributes --queue-url $QUEUE3_URL --attribute-names QueueArn --query 'Attributes.QueueArn')

# Subscribe all queues
aws sns subscribe --topic-arn $TOPIC_ARN --protocol sqs --notification-endpoint $QUEUE1_ARN
aws sns subscribe --topic-arn $TOPIC_ARN --protocol sqs --notification-endpoint $QUEUE2_ARN
aws sns subscribe --topic-arn $TOPIC_ARN --protocol sqs --notification-endpoint $QUEUE3_ARN
```

### Example 3: Cross-Region Delivery

```bash
# Enable cross-region subscription (via console or different architecture)
# Create topic in primary region
aws sns create-topic --name global-alerts

# Use AWS EventBridge for cross-region event routing
aws events put-rule \
    --name "replicate-sns-to-other-region" \
    --event-pattern '{"source":["aws.sns"]}' \
    --state ENABLED

# Add target in another region
aws events put-targets \
    --rule "replicate-sns-to-other-region" \
    --targets '[{"Id":"target-1","Arn":"arn:aws:events:us-west-2:123456789:event-bus/default"}]'
```

## ⚠️ COMMON ISSUES

### 1. Subscription Not Receiving Messages

**Problem**: Messages not being delivered to endpoint

**Solution**: Check subscription confirmation, verify endpoint is reachable, check filter policy

### 2. Message Filtering Not Working

**Problem**: Filtered messages still arriving

**Solution**: Ensure subscription has matching filter policy, check JSON syntax

### 3. FIFO Topic Limitations

**Problem**: Cannot create FIFO topic

**Solution**: Ensure name ends with .fifo, specify ContentBasedDeduplication

## 🏃 PERFORMANCE

### Limits

| Feature | Limit |
|---------|-------|
| Topics per account | 100,000 |
| Subscriptions per topic | 12,000,000 |
| Publish TPS | 10,000 |
| Message size | 256KB |

## 🔗 CROSS-REFERENCES

**Related**: SQS, EventBridge, Lambda, CloudWatch

**Prerequisite**: Basic SNS understanding

## ✅ EXAM TIPS

- Fanout pattern: one topic, multiple SQS subscriptions
- Message filtering at subscription level
- FIFO topics require message group ID
- Cross-region uses EventBridge or SNS regional isolation