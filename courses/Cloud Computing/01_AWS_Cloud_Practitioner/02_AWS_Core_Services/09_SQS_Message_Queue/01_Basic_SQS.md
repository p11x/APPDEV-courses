---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: SQS Message Queue
Purpose: Understanding Amazon SQS for message queuing and application decoupling
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_SQS.md, 03_Practical_SQS.md
UseCase: Decoupling applications with reliable message delivery
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## 💡 WHY

Amazon SQS is AWS's fully managed message queue service that enables decoupling of microservices and distributed systems. It's essential for building scalable, fault-tolerant applications.

### Why SQS Matters

- **Decoupling**: Components can scale independently
- **Reliability**: Messages persisted until processed
- **Managed**: No server infrastructure to manage
- **Scalability**: Handle millions of messages
- **Cost-effective**: Pay only for what you use

### Industry Use Cases

- Order processing systems
- Background job processing
- Distributed task queues
- Event-driven architectures

## 📖 WHAT

### SQS Core Concepts

**Queue**: Named buffer storing messages

**Message**: Data payload (up to 256KB)

**Producer**: Sends messages to queue

**Consumer**: Reads and processes messages

**Visibility Timeout**: Time before message becomes visible again

### Queue Types

| Type | Use Case | Delivery |
|------|----------|----------|
| Standard | High throughput, at-least-once | Best effort |
| FIFO | Order preserved, exactly-once | Guaranteed |

### Architecture Diagram

```
SQS Architecture
================

Producer          SQS Queue          Consumer
   │                  │                  │
   │─── Message ─────►│                  │
   │                  │                  │
   │                  │─── Message ─────►│
   │                  │                  │ (Processing)
   │                  │                  │
   │                  │(Hidden during    │
   │                  │ visibility       │
   │                  │ timeout)         │
   │                  │                  │
   │                  │─── Message ─────►│ (Complete)
   │                  │                  │
   │                  │(Delete)          │
```

## 🔧 HOW

### Example 1: Create and Use Queue

```bash
# Create standard queue
aws sqs create-queue \
    --queue-name my-queue \
    --attributes '{
        "VisibilityTimeout": "300",
        "MessageRetentionPeriod": "86400"
    }'

# Get queue URL
QUEUE_URL=$(aws sqs get-queue-url \
    --queue-name my-queue \
    --query 'QueueUrl' \
    --output text)
echo "Queue URL: $QUEUE_URL"

# Send message
aws sqs send-message \
    --queue-url $QUEUE_URL \
    --message-body '{"orderId": "12345", "amount": 99.99}'

# Receive message
aws sqs receive-message \
    --queue-url $QUEUE_URL \
    --max-number-of-messages 10

# Delete message
aws sqs delete-message \
    --queue-url $QUEUE_URL \
    --receipt-handle "<receipt-handle>"
```

### Example 2: FIFO Queue

```bash
# Create FIFO queue
aws sqs create-queue \
    --queue-name orders.fifo \
    --attributes '{
        "FifoQueue": "true",
        "ContentBasedDeduplication": "true"
    }'

# Send with message group ID
aws sqs send-message \
    --queue-url https://sqs.us-east-1.amazonaws.com/123456789/orders.fifo \
    --message-body '{"orderId": "ORD-001", "items": ["item1", "item2"]}' \
    --message-group-id order-processing
```

### Example 3: Dead Letter Queue

```bash
# Create DLQ
aws sqs create-queue \
    --queue-name my-queue-dlq

# Create main queue with DLQ
aws sqs create-queue \
    --queue-name my-queue \
    --attributes '{
        "RedrivePolicy": "{\"deadLetterTargetArn\":\"arn:aws:sqs:us-east-1:123456789:my-queue-dlq\",\"maxReceiveCount\":\"5\"}"
    }'
```

### Example 4: SQS with Lambda

```yaml
# SAM template for SQS trigger
AWSTemplateFormatVersion: '2010-09-09'
Transform: AWS::Serverless-2016-10-31
Resources:
  ProcessFunction:
    Type: AWS::Serverless::Function
    Properties:
      Handler: index.handler
      Runtime: nodejs18.x
      Events:
        SQSEvent:
          Type: SQS
          Properties:
            Queue: !GetAtt MyQueue.Arn
            BatchSize: 10
  MyQueue:
    Type: AWS::SQS::Queue
```

## ⚠️ COMMON ISSUES

### 1. Message Not Being Deleted

**Problem**: Messages remain in queue after processing.

**Solution**: Always delete message after processing using receipt handle.

### 2. Messages Going to DLQ

**Problem**: Messages exceeding maxReceiveCount.

**Solution**: Check consumer logic, increase maxReceiveCount, or fix application bugs.

### 3. Standard Queue Ordering

**Problem**: Messages processed out of order.

**Solution**: Use FIFO queue for strict ordering requirements.

### 4. Large Message Payloads

**Problem**: Messages exceed 256KB.

**Solution**: Use S3 for large payloads, store reference in SQS message.

## 🏃 PERFORMANCE

### Queue Limits

| Metric | Limit |
|--------|-------|
| Message Size | 256KB |
| Queue Retention | 14 days max |
| Visibility Timeout | 12 hours max |
| Throughput | Unlimited (standard) |
| FIFO | 300 TPS |

### Cost Model

- Requests: $0.40 per 1M requests
- Data transfer: Standard rates apply

## 🌐 COMPATIBILITY

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Queue Type | Standard, FIFO | Service Bus | Cloud Pub/Sub |
| Max Message | 256KB | 256KB | 10MB |
| Retention | 14 days | 7 days | 7 days |
| DLQ | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

**Related**: SNS, Lambda, EventBridge, DynamoDB Streams

**Prerequisite**: Basic cloud concepts

**Next**: SNS for pub/sub, EventBridge for event routing

## ✅ EXAM TIPS

- SQS is pull-based (consumer polls)
- Standard queue = at-least-once delivery
- FIFO queue = exactly-once processing
- DLQ holds failed messages
- Visibility timeout prevents duplicate processing