---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: SQS Messaging
Purpose: Hands-on SQS implementation including production message processing, Lambda integration, and monitoring
Difficulty: intermediate
Prerequisites: 01_Basic_SQS.md, 02_Advanced_SQS.md
RelatedFiles: 01_Basic_SQS.md, 02_Advanced_SQS.md
UseCase: Production message queue deployment
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## 💡 WHY

Hands-on SQS implementation provides practical experience building event-driven architectures with reliable message processing.

## 📖 WHAT

### Lab Architecture

```
┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐
│ Producer│───►│   SQS   │───►│ Lambda  │───►│ Process │
└─────────┘    │  Queue  │    │ Consumer│    │  Result │
               └─────────┘    └─────────┘    └─────────┘
                    │              │
                    └──────────────┘
                         │
                    ┌────┴────┐
                    │   DLQ  │
                    └─────────┘
```

## 🔧 HOW

### Module 1: Create Production Queue

```bash
#!/bin/bash
# SQS Production Setup

# Create dead letter queue
aws sqs create-queue \
    --queue-name prod-dlq.fifo \
    --attributes '{
        "FifoQueue": "true",
        "ContentBasedDeduplication": "true"
    }'

# Create main queue with DLQ
DLQ_ARN=$(aws sqs get-queue-attributes \
    --queue-url https://sqs.us-east-1.amazonaws.com/123456789/prod-dlq.fifo \
    --attribute-names QueueArn \
    --query 'Attributes.QueueArn')

aws sqs create-queue \
    --queue-name orders-prod.fifo \
    --attributes '{
        "FifoQueue": "true",
        "ContentBasedDeduplication": "true",
        "VisibilityTimeout": "300",
        "MessageRetentionPeriod": "604800",
        "RedrivePolicy": "{\"deadLetterTargetArn\":\"'$DLQ_ARN'\",\"maxReceiveCount\":\"3\"}"
    }'
```

### Module 2: Lambda Consumer

```javascript
// index.js - Lambda message processor
const AWS = require('aws-sdk');
const sqs = new AWS.SQS();

exports.handler = async (event) => {
    console.log('Received', event.Records.length, 'messages');
    
    for (const record of event.Records) {
        try {
            const message = JSON.parse(record.body);
            console.log('Processing order:', message.orderId);
            
            // Process the message
            await processOrder(message);
            
            console.log('Successfully processed order:', message.orderId);
        } catch (error) {
            console.error('Error processing message:', error.message);
            throw error; // Will trigger Lambda retry and then DLQ
        }
    }
    
    return { processed: event.Records.length };
};

async function processOrder(order) {
    // Simulate order processing
    await new Promise(resolve => setTimeout(resolve, 100));
    return true;
}
```

```bash
# Create Lambda function
aws lambda create-function \
    --function-name order-processor \
    --runtime nodejs18.x \
    --role arn:aws:iam::123456789:role/lambda-sqs-role \
    --handler index.handler \
    --zip-file fileb://function.zip

# Create event source mapping
aws lambda create-event-source-mapping \
    --function-name order-processor \
    --event-source-arn arn:aws:sqs:us-east-1:123456789:orders-prod.fifo \
    --batch-size 10
```

### Module 3: Monitoring

```bash
# Create CloudWatch alarm for queue depth
aws cloudwatch put-metric-alarm \
    --alarm-name "orders-queue-depth-high" \
    --metric-name ApproximateNumberOfMessagesVisible \
    --namespace AWS/SQS \
    --statistic Maximum \
    --period 300 \
    --threshold 1000 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 2 \
    --alarm-actions arn:aws:sns:us-east-1:123456789:alerts
```

## VERIFICATION

```bash
# Check queue metrics
aws cloudwatch get-metric-statistics \
    --namespace AWS/SQS \
    --metric-name ApproximateNumberOfMessagesVisible \
    --start-time 2024-01-01T00:00:00Z \
    --end-time 2024-01-01T01:00:00Z \
    --period 300 \
    --statistics Maximum

# Check Lambda invocations
aws logs filter-log-events \
    --log-group-name /aws/lambda/order-processor \
    --filter-pattern "Successfully processed"
```

## CLEANUP

```bash
# Delete queue
aws sqs delete-queue \
    --queue-url https://sqs.us-east-1.amazonaws.com/123456789/orders-prod.fifo
```

## 🔗 CROSS-REFERENCES

**Related**: SNS, Lambda, CloudWatch