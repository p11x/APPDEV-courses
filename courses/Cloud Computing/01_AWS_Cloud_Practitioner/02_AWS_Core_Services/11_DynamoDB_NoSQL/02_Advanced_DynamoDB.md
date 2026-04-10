---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: DynamoDB NoSQL
Purpose: Advanced DynamoDB concepts including global tables, TTL, streams, and capacity modes
Difficulty: advanced
Prerequisites: 01_Basic_DynamoDB.md
RelatedFiles: 01_Basic_DynamoDB.md, 03_Practical_DynamoDB.md
UseCase: Enterprise NoSQL database design
CertificationExam: AWS Database Specialty
LastUpdated: 2025
---

## 💡 WHY

Advanced DynamoDB features enable global-scale, real-time applications with sophisticated data patterns and cost optimization.

## 📖 WHAT

### Advanced Features

**Global Tables**: Multi-region active-active replication

**DynamoDB Streams**: Change data capture for event-driven architectures

**TTL (Time to Live)**: Automatic data expiration

**On-Demand Capacity**: Pay-per-request billing

**DAX (DynamoDB Accelerator)**: In-memory cache

### Cross-Platform Comparison

| Feature | AWS DynamoDB | Azure Cosmos DB | GCP Firestore | MongoDB Atlas |
|---------|--------------|------------------|---------------|---------------|
| Global Tables | Yes | Native | Yes | Yes |
| In-Memory Cache | DAX | Optional | Datastore mode | Atlas Cache |
| Event Streams | Streams | Change Feed | Listeners | Change Streams |
| Auto-Scaling | Yes | Yes | Yes | Yes |
| Max Item Size | 400KB | 2MB | 1MB | 16MB |
| Free Tier | 25 GB | 50 GB | 5 GB | 512 MB |
| Max Write Units | Unlimited | Unlimited | Unlimited | Sharded |

## 🔧 HOW

### Example 1: Global Tables

```bash
# Create table in first region
aws dynamodb create-table \
    --table-name global-users \
    --attribute-definitions '[
        {"AttributeName": "userId", "AttributeType": "S"}
    ]' \
    --key-schema '[
        {"AttributeName": "userId", "KeyType": "HASH"}
    ]' \
    --billing-mode PAY_PER_REQUEST \
    --stream-specification '{
        "StreamEnabled": true,
        "StreamViewType": "NEW_AND_OLD_IMAGES"
    }'

# Enable global tables (via console or API in each region)
aws dynamodb update-table \
    --table-name global-users \
    --global-table-version "2024.1.0" \
    --replica-settings '[
        {"RegionName": "us-east-1"},
        {"RegionName": "us-west-2"},
        {"RegionName": "eu-west-1"}
    ]'
```

### Example 2: TTL for Auto-Expiration

```bash
# Enable TTL on table
aws dynamodb update-time-to-live \
    --table-name session-data \
    --time-to-live-specification '{
        "Enabled": true,
        "AttributeName": "expiresAt"
    }'

# Items will automatically expire when expiresAt timestamp passes
# No additional cost for TTL deletion

# Example item with TTL
aws dynamodb put-item \
    --table-name session-data \
    --item '{
        "sessionId": {"S": "sess-123"},
        "userId": {"S": "user-456"},
        "data": {"S": "session data"},
        "expiresAt": {"N": "1706784000"}
    }'
```

### Example 3: DynamoDB Streams with Lambda

```bash
# Create Lambda function to process stream
aws lambda create-function \
    --function-name stream-processor \
    --runtime nodejs18.x \
    --handler index.handler \
    --zip-file fileb://stream-handler.zip \
    --role arn:aws:iam::123456789:role/lambda-dynamodb-role

# Add event source mapping
aws lambda create-event-source-mapping \
    --function-name stream-processor \
    --event-source-arn arn:aws:dynamodb:us-east-1:123456789:table/orders/stream/2024-01-01T00:00:00Z \
    --batch-size 100 \
    --starting-position TRIM_HORIZON
```

## ⚠️ COMMON ISSUES

### 1. Hot Partition

**Problem**: Uneven access causing throttling

**Solution**: Use partition keys with high cardinality, enable adaptive capacity

### 2. Wrong Capacity Mode

**Problem**: Over-provisioned or under-provisioned

**Solution**: Use on-demand for variable, provisioned for predictable

### 3. Scan Instead of Query

**Problem**: Expensive full table scans

**Solution**: Always use partition key queries, create GSIs for access patterns

## 🏃 PERFORMANCE

### Limits

| Feature | Limit |
|---------|-------|
| Item Size | 400KB |
| Table Size | Unlimited |
| Partition Size | 10GB |
| Max RCU/WCU | 40,000 per table (can request increase) |

## 🔗 CROSS-REFERENCES

**Related**: Lambda, Kinesis, ElastiCache, Data Pipeline

**Prerequisite**: Basic DynamoDB understanding

## ✅ EXAM TIPS

- Global Tables provide multi-region replication
- Streams enable event-driven patterns
- TTL automatically deletes old data at no cost
- On-demand mode: pay-per-request
- DAX provides millisecond caching