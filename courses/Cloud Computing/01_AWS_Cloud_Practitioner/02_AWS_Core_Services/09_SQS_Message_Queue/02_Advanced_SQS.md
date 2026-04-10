---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: SQS Messaging
Purpose: Advanced SQS configuration including dead-letter queues, FIFO ordering, and message batching
Difficulty: advanced
Prerequisites: 01_Basic_SQS.md
RelatedFiles: 01_Basic_SQS.md, 03_Practical_SQS.md
UseCase: Enterprise message processing architectures
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## 💡 WHY

Advanced SQS features enable enterprise-grade message processing with guaranteed delivery, ordering, and failure handling. Understanding these patterns is essential for production systems.

## 📖 WHAT

### Advanced Features

**FIFO Queues**: Guaranteed ordering and exactly-once processing

**Dead Letter Queues**: Handle failed messages after max receives

**Message Batching**: Reduce API calls with batch operations

**Delay Queues**: Postpone message delivery

**Long Polling**: Reduce costs by waiting for messages

### Cross-Platform Comparison

| Feature | AWS SQS | Azure Service Bus | GCP Pub/Sub | RabbitMQ |
|---------|---------|-------------------|--------------|----------|
| Queue Type | FIFO + Standard | FIFO | FIFO | FIFO |
| Max Message | 256KB | 256KB | 10MB | 512KB |
| DLQ Support | Yes | Yes | Yes | Yes |
| Dead Letter | Yes | Yes | Yes | Yes |
| Batching | Yes | Yes | Yes | Yes |
| Delay Queues | Yes | Yes | Yes | Yes |
| Priority Queues | No | Yes | No | Yes |
| Message Timings | 14 days | 7 days | 7 days | Configurable |

## 🔧 HOW

### Example 1: FIFO Queue with Deduplication

```bash
# Create FIFO queue with deduplication
aws sqs create-queue \
    --queue-name orders.fifo \
    --attributes '{
        "FifoQueue": "true",
        "ContentBasedDeduplication": "true",
        "VisibilityTimeout": "300",
        "MessageRetentionPeriod": "86400"
    }'

# Send message with deduplication ID
aws sqs send-message \
    --queue-url https://sqs.us-east-1.amazonaws.com/123456789/orders.fifo \
    --message-body '{"orderId": "ORD-001", "amount": 99.99}' \
    --message-deduplication-id order-001-12345
```

### Example 2: Dead Letter Queue Configuration

```bash
# Create main queue with DLQ
aws sqs create-queue \
    --queue-name main-queue

# Create DLQ
aws sqs create-queue \
    --queue-name main-queue-dlq

# Get DLQ ARN
DLQ_ARN=$(aws sqs get-queue-attributes \
    --queue-url https://sqs.us-east-1.amazonaws.com/123456789/main-queue-dlq \
    --attribute-names QueueArn \
    --query 'Attributes.QueueArn')

# Configure main queue to use DLQ
aws sqs set-queue-attributes \
    --queue-url https://sqs.us-east-1.amazonaws.com/123456789/main-queue \
    --attributes '{
        "RedrivePolicy": "{\"deadLetterTargetArn\":\"'$DLQ_ARN'\",\"maxReceiveCount\":\"5\"}"
    }'
```

### Example 3: Lambda Event Source Mapping

```bash
# Create Lambda event source mapping
aws lambda create-event-source-mapping \
    --function-name order-processor \
    --event-source-arn arn:aws:sqs:us-east-1:123456789:orders.fifo \
    --batch-size 10 \
    --maximum-batching-window-in-seconds 300 \
    --destination-config '{"OnFailure": {"Destination": "arn:aws:sqs:us-east-1:123456789:orders-dlq"}}'

# Configure bisect batch on function errors
aws lambda update-function-configuration \
    --function-name order-processor \
    --bisect-batch-on-function-errors
```

## ⚠️ COMMON ISSUES

### 1. Message Not Reaching DLQ

**Problem**: Messages exceeding maxReceiveCount not moving to DLQ

**Solution**: Verify redrive policy JSON format is correct, ensure DLQ exists

### 2. Duplicate Messages

**Problem**: Duplicate messages in FIFO queue

**Solution**: Enable ContentBasedDeduplication or provide MessageDeduplicationId

### 3. Message Order Not Guaranteed

**Problem**: Messages processed out of order

**Solution**: Use FIFO queue, ensure same message group ID for related messages

## 🏃 PERFORMANCE

### Limits

| Parameter | Standard | FIFO |
|-----------|----------|------|
| TPS (per queue) | Unlimited | 300 |
| Message size | 256KB | 256KB |
| Batch size | 10 messages | 10 messages |

## 🔗 CROSS-REFERENCES

**Related**: SNS for pub/sub, EventBridge for event routing, Lambda for processing

**Prerequisite**: Basic SQS understanding

## ✅ EXAM TIPS

- FIFO queues guarantee delivery order
- ContentBasedDeduplication auto-generates ID from content hash
- DLQ holds messages that fail processing
- Long polling reduces costs vs short polling