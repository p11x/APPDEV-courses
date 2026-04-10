---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: DynamoDB NoSQL
Purpose: Understanding Amazon DynamoDB fully managed NoSQL database
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_DynamoDB.md, 03_Practical_DynamoDB.md
UseCase: High-performance NoSQL database for applications
CertificationExam: AWS Database Specialty
LastUpdated: 2025
---

## 💡 WHY

Amazon DynamoDB is AWS's fully managed NoSQL database service that provides single-digit millisecond performance at any scale. It's essential for applications requiring flexible data models and automatic scaling.

### Why DynamoDB Matters

- **Performance**: Single-digit millisecond latency
- **Scaling**: Automatic partitioning
- **Managed**: No server management
- **Serverless**: Pay-per-request option
- **Security**: Built-in encryption and access control

### Industry Use Cases

- Mobile backends
- Gaming leaderboards
- IoT device data
- Real-time bidding systems
- Session stores

## 📖 WHAT

### DynamoDB Core Concepts

**Table**: Collection of items

**Item**: Single record (like row)

**Attribute**: Key-value pair (like column)

**Partition Key**: Determines data distribution

**Sort Key**: Enables hierarchical queries

### Key-Value and Document Data

| Feature | Description |
|---------|-------------|
| Key-Value | Simple access patterns |
| Document | JSON, nested structures |
| No fixed schema | Flexible attributes |

### Architecture Diagram

```
DynamoDB Architecture
======================

Client ─────► DynamoDB Table
                    │
         ┌─────────┴─────────┐
         │   Partition Key    │
         │   (PK)            │
         └─────────┬─────────┘
                   │
         ┌─────────┴─────────┐
         │   Data stored in  │
         │   partitions      │
         │                   │
         │  ┌────┐ ┌────┐    │
         │  │ Pt1│ │ Pt2│    │
         │  └────┘ └────┘    │
         └──────────────────┘
```

## 🔧 HOW

### Example 1: Create Table

```bash
# Create table with partition key
aws dynamodb create-table \
    --table-name Users \
    --attribute-definitions '[
        {"AttributeName": "userId", "AttributeType": "S"}
    ]' \
    --key-schema '[
        {"AttributeName": "userId", "KeyType": "HASH"}
    ]' \
    --billing-mode PAY_PER_REQUEST \
    --tags 'Key=Environment,Value=Production'

# Create table with partition and sort key
aws dynamodb create-table \
    --table-name Orders \
    --attribute-definitions '[
        {"AttributeName": "customerId", "AttributeType": "S"},
        {"AttributeName": "orderId", "AttributeType": "S"},
        {"AttributeName": "timestamp", "AttributeType": "S"}
    ]' \
    --key-schema '[
        {"AttributeName": "customerId", "KeyType": "HASH"},
        {"AttributeName": "orderId", "KeyType": "RANGE"}
    ]' \
    --billing-mode PAY_PER_REQUEST
```

### Example 2: CRUD Operations

```bash
# Put item
aws dynamodb put-item \
    --table-name Users \
    --item '{
        "userId": {"S": "user123"},
        "email": {"S": "user@example.com"},
        "name": {"S": "John Doe"},
        "age": {"N": "30"}
    }'

# Get item
aws dynamodb get-item \
    --table-name Users \
    --key '{"userId": {"S": "user123"}}'

# Update item
aws dynamodb update-item \
    --table-name Users \
    --key '{"userId": {"S": "user123"}}' \
    --update-expression "SET age = :newAge" \
    --expression-attribute-values '{
        ":newAge": {"N": "31"}
    }'

# Delete item
aws dynamodb delete-item \
    --table-name Users \
    --key '{"userId": {"S": "user123"}}'
```

### Example 3: Queries and Scans

```bash
# Query by partition key
aws dynamodb query \
    --table-name Orders \
    --key-condition-expression "customerId = :cid" \
    --expression-attribute-values '{
        ":cid": {"S": "customer123"}
    }'

# Query with sort key condition
aws dynamodb query \
    --table-name Orders \
    --key-condition-expression "customerId = :cid AND orderId BETWEEN :start AND :end" \
    --expression-attribute-values '{
        ":cid": {"S": "customer123"},
        ":start": {"S": "ORD-001"},
        ":end": {"S": "ORD-010"}
    }'

# Scan (full table)
aws dynamodb scan \
    --table-name Users \
    --filter-expression "age > :age" \
    --expression-attribute-values '{
        ":age": {"N": "25"}
    }'
```

### Example 4: Using GSI and LSI

```bash
# Create table with GSI
aws dynamodb create-table \
    --table-name Products \
    --attribute-definitions '[
        {"AttributeName": "category", "AttributeType": "S"},
        {"AttributeName": "productId", "AttributeType": "S"}
    ]' \
    --key-schema '[
        {"AttributeName": "productId", "KeyType": "HASH"}
    ]' \
    --global-secondary-indexes '[
        {
            "IndexName": "CategoryIndex",
            "KeySchema": [{"AttributeName": "category", "KeyType": "HASH"}],
            "Projection": {"ProjectionType": "ALL"}
        }
    ]' \
    --billing-mode PAY_PER_REQUEST
```

## ⚠️ COMMON ISSUES

### 1. Hot Partitions

**Problem**: Uneven access patterns cause throttling.

**Solution**: Use distributed partition keys, enable adaptive capacity.

### 2. Scan Instead of Query

**Problem**: Full table scans are expensive.

**Solution**: Use partition key queries, create GSIs.

### 3. Wrong Capacity Mode

**Problem**: Over or under-provisioned capacity.

**Solution**: Use on-demand for variable workloads, provisioned for predictable.

### 4. Missing Backups

**Problem**: Data loss from accidental deletes.

**Solution**: Enable Point-in-time recovery, use on-demand backups.

## 🏃 PERFORMANCE

### Limits

| Feature | Limit |
|---------|-------|
| Item Size | 400KB |
| Attributes per item | Unlimited (400KB limit) |
| Table size | Unlimited |
| Partition size | 10GB |

### Modes

| Mode | Use Case |
|------|----------|
| On-Demand | Variable, unpredictable |
| Provisioned | Predictable, steady |

## 🌐 COMPATIBILITY

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| NoSQL | DynamoDB | Cosmos DB | Firestore |
| Key-value | Yes | Yes | Yes |
| Serverless | Yes | Yes | Yes |
| Global Tables | Yes | Yes | Datastore |

## 🔗 CROSS-REFERENCES

**Related**: Lambda, SQS, ElastiCache, RDS

**Prerequisite**: Basic cloud concepts

**Next**: DynamoDB Streams for change data capture

## ✅ EXAM TIPS

- Partition key = HASH, sort key = RANGE
- GSI for alternative access patterns
- LSI for different sort order on same partition
- On-demand mode: pay-per-request
- Provisioned mode: pay for capacity units
- Avoid hot partitions by distributing keys