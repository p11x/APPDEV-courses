---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: SNS Message Pub/Sub
Purpose: Understanding Amazon SNS for pub/sub messaging and notifications
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_SNS.md, 03_Practical_SNS.md
UseCase: Building event-driven notification systems
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## 💡 WHY

Amazon SNS is AWS's pub/sub messaging service for event-driven architectures. It enables scalable notification systems and application decoupling through the publish-subscribe pattern.

### Why SNS Matters

- **Push-based**: No polling required
- **Fan-out**: One message to multiple subscribers
- **Topics**: Organize messages by category
- **Protocols**: Email, SMS, HTTP, Lambda, SQS
- **Managed**: No infrastructure to manage

### Industry Use Cases

- Application alerts and notifications
- Mobile push notifications
- Email and SMS notifications
- Event-driven microservices
- Real-time streaming

## 📖 WHAT

### SNS Core Concepts

**Topic**: Named channel for publishing messages

**Publisher**: Sends message to topic

**Subscriber**: Receives messages from topic

**Subscription**: Connection between topic and endpoint

### Supported Protocols

| Protocol | Description |
|----------|-------------|
| HTTP/HTTPS | Webhook delivery |
| Email | JSON or plain text |
| SMS | Text messages |
| SQS | Queue integration |
| Lambda | Function invocation |
| Platform | Mobile push (APNS, FCM) |

### Architecture Diagram

```
SNS Pub/Sub Architecture
=========================

Publisher 1 ──┐
             │
Publisher 2 ──┼──► SNS Topic ◄── Subscription 1 ──► Lambda
             │               ├── Subscription 2 ──► SQS
Publisher 3 ──┘               ├── Subscription 3 ──► Email
                             └── Subscription 4 ──► SMS
```

## 🔧 HOW

### Example 1: Create Topic and Subscribe

```bash
# Create topic
TOPIC_ARN=$(aws sns create-topic \
    --name my-notifications \
    --query 'TopicArn' \
    --output text)
echo "Topic ARN: $TOPIC_ARN"

# Subscribe email endpoint
aws sns subscribe \
    --topic-arn $TOPIC_ARN \
    --protocol email \
    --notification-endpoint admin@example.com

# Subscribe Lambda function
aws sns subscribe \
    --topic-arn $TOPIC_ARN \
    --protocol lambda \
    --notification-endpoint arn:aws:lambda:us-east-1:123456789:function:my-function

# Publish message
aws sns publish \
    --topic-arn $TOPIC_ARN \
    --subject "Alert: High CPU Usage" \
    --message '{"instanceId": "i-123456789", "cpu": 95}'
```

### Example 2: SNS to SQS Fan-out

```bash
# Create SQS queues
QUEUE1_URL=$(aws sqs create-queue --queue-name queue1 --query 'QueueUrl' --output text)
QUEUE2_URL=$(aws sqs create-queue --queue-name queue2 --query 'QueueUrl' --output text)

# Create SNS topic
TOPIC_ARN=$(aws sns create-topic --name fanout-topic --query 'TopicArn' --output text)

# Get queue ARNs
QUEUE1_ARN=$(aws sqs get-queue-attributes \
    --queue-url $QUEUE1_URL \
    --attribute-names QueueArn \
    --query 'Attributes.QueueArn' \
    --output text)
QUEUE2_ARN=$(aws sqs get-queue-attributes \
    --queue-url $QUEUE2_URL \
    --attribute-names QueueArn \
    --query 'Attributes.QueueArn' \
    --output text)

# Subscribe queues to topic
aws sns subscribe \
    --topic-arn $TOPIC_ARN \
    --protocol sqs \
    --notification-endpoint $QUEUE1_ARN
aws sns subscribe \
    --topic-arn $TOPIC_ARN \
    --protocol sqs \
    --notification-endpoint $QUEUE2_ARN
```

### Example 3: SMS Notifications

```bash
# Set topic to support SMS
aws sns publish \
    --topic-arn $TOPIC_ARN \
    --message "System alert: Production server down" \
    --target-arn arn:aws:sns:us-east-1:123456789:endpoint/SMS/my-endpoint

# Direct SMS (without topic)
aws sns publish \
    --phone-number "+1234567890" \
    --message "Your verification code is 123456"
```

### Example 4: SNS with Filtering

```bash
# Create topic with filtering
TOPIC_ARN=$(aws sns create-topic \
    --name filtered-topic \
    --attributes '{
        "SubscriptionPolicy": "{\"filterPolicy\": {\"eventType\": [\"order_placed\", \"order_shipped\"]}}"
    }' \
    --query 'TopicArn' \
    --output text)

# Publish with attributes
aws sns publish \
    --topic-arn $TOPIC_ARN \
    --message '{"orderId": "12345", "status": "shipped"}' \
    --message-attributes '{
        "eventType": {
            "DataType": "String",
            "StringValue": "order_shipped"
        }
    }'
```

## ⚠️ COMMON ISSUES

### 1. Messages Not Delivered

**Problem**: Subscribers not receiving messages.

**Solution**: Check subscription confirmation, verify endpoint is reachable.

### 2. Email Going to Spam

**Problem**: Email notifications marked as spam.

**Solution**: Use proper email headers, verify sender reputation.

### 3. Message Size Limit

**Problem**: Message exceeds 256KB.

**Solution**: Use S3 for large payloads, reference in SNS message.

### 4. FIFO Topic Limits

**Problem**: FIFO topic throughput limited.

**Solution**: Batch messages, use partitioning for scaling.

## 🏃 PERFORMANCE

### Limits

| Metric | Standard | FIFO |
|--------|----------|------|
| Publish TPS | 12,000/sec | 150/sec |
| Subscriptions | 12M/topic | 100/topic |
| Message Size | 256KB | 256KB |

### Cost Model

- Publish: $0.50 per 1M
- SQS delivery: $0.40 per 1M
- Email: $0.50 per 1K
- SMS: varies by country

## 🌐 COMPATIBILITY

| Feature | AWS SNS | Azure Event Grid | GCP Pub/Sub |
|---------|---------|------------------|--------------|
| Push delivery | Yes | Yes | Yes |
| FIFO | Yes | Yes | Yes |
| Filtering | Yes | Yes | Yes |
| SMS | Yes | No | No |

## 🔗 CROSS-REFERENCES

**Related**: SQS, Lambda, EventBridge, CloudWatch

**Prerequisite**: Basic cloud concepts

**Next**: EventBridge for advanced event routing

## ✅ EXAM TIPS

- SNS is push-based (publisher to subscribers)
- Fan-out pattern: one publisher, multiple subscribers
- FIFO for ordered message processing
- Combine with SQS for guaranteed delivery
- Message attributes enable filtering