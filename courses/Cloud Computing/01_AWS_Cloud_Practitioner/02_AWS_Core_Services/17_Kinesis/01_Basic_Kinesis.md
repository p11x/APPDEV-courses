---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Kinesis
Purpose: Understanding AWS Kinesis for real-time data streaming
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Kinesis.md, 03_Practical_Kinesis.md
UseCase: Real-time data streaming and processing
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

AWS Kinesis is a family of services for real-time data streaming and processing. Understanding Kinesis is essential for building applications that need to process data in real-time, such as IoT analytics, log processing, and event-driven architectures.

### Why Kinesis Matters

- **Real-Time**: Process data within milliseconds
- **Scalability**: Handle gigabytes per second
- **Durability**: Multi-AZ data replication
- **Low Latency**: Sub-second processing
- **Integration**: Works with 50+ AWS services
- **Managed**: No servers to manage

### Industry Statistics

- Processes trillions of records per day
- Used by 80% of Fortune 500 for analytics
- 99.999999999% durability
- Supports millions of producers/consumers

### When NOT to Use Kinesis

- Batch processing: Use Glue/Batch
- Simple queues: Use SQS
- File storage: Use S3
- Near-real-time acceptable: Use S3 + Lambda

## WHAT

### Kinesis Family

| Service | Use Case | Data Type |
|---------|----------|-----------|
| Kinesis Data Streams | Custom streaming | Any data |
| Kinesis Data Firehose | Load to destinations | Structured data |
| Kinesis Data Analytics | SQL real-time processing | Structured data |
| Kinesis Video Streams | Video streaming | Video |

### Core Concepts

**Shard**: Base throughput unit - 1MB/s write, 2MB/s read.

**Partition Key**: Routes data to specific shard.

**Sequence Number**: Unique ID for each record.

**Consumer**: Application reading from stream.

### Architecture Diagram

```
                    KINESIS ARCHITECTURE
                    =====================

    ┌──────────────┐    ┌──────────────┐    ┌──────────────┐
    │  Producer 1 │    │  Producer 2 │    │  Producer N │
    │  - IoT     │    │  - Logs     │    │  - App     │
    └──────┬──────┘    └──────┬──────┘    └──────┬──────┘
           │                   │                   │
           ▼                   ▼                   ▼
    ┌─────────────────────────────────────────────────────┐
    │              KINESIS DATA STREAM                    │
    │  ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐   │
    │  │ Shard 0 │ │ Shard 1 │ │ Shard 2 │ │ Shard N │   │
    │  │ hash    │ │ hash    │ │ hash    │ │ hash    │   │
    │  │ key     │ │ key     │ │ key     │ │ key     │   │
    │  └─────────┘ └─────────┘ └─────────┘ └─────────┘   │
    └─────────────────────────────────────────────────────┘
           │                   │                   │
           ▼                   ▼                   ▼
    ┌──────────────┐    ┌──────────────┐    ┌──────────────┐
    │  Consumer 1 │    │  Consumer 2 │    │  Consumer 3 │
    │  - Lambda   │    │  - Kinesis  │    │  - Custom   │
    │             │    │    Analytics│    │             │
    └──────────────┘    └──────────────┘    └──────────────┘
```

### Throughput

| Shard | Write (MB/s) | Read (MB/s) | Records/s |
|-------|--------------|-------------|-----------|
| 1 | 1 | 2 | 1000 |
| 10 | 10 | 20 | 10,000 |
| 100 | 100 | 200 | 100,000 |

## HOW

### Example 1: Create Kinesis Data Stream

```bash
# Create data stream
aws kinesis create-stream \
    --stream-name my-stream \
    --shard-count 1 \
    --stream-mode-details 'StreamMode=PROVISIONED'

# Create stream with on-demand mode
aws kinesis create-stream \
    --stream-name my-on-demand-stream \
    --stream-mode-details 'StreamMode=ON_DEMAND'

# Describe stream
aws kinesis describe-stream \
    --stream-name my-stream

# Output includes:
# "StreamDescription": {
#     "StreamName": "my-stream",
#     "StreamARN": "arn:aws:kinesis:us-east-1:123456789012:stream/my-stream",
#     "Shards": [...],
#     "StreamStatus": "ACTIVE"
# }

# List streams
aws kinesis list-streams

# Add shards to scale
aws kinesis update-shard-count \
    --stream-name my-stream \
    --target-shard-count 4 \
    --scaling-type UNIFORM_SCALING
```

### Example 2: Produce Data

```bash
# Put record
aws kinesis put-record \
    --stream-name my-stream \
    --partition-key "user-123" \
    --data "Hello Kinesis" \
    --cli-binary-format raw-in-base64-out

# Put base64 encoded data
aws kinesis put-record \
    --stream-name my-stream \
    --partition-key "user-123" \
    --data $(echo -n '{"event": "click", "user": "123"}' | base64)

# Put multiple records
aws kinesis put-records \
    --stream-name my-stream \
    --records '[
        {"Data": "Record1", "PartitionKey": "key1"},
        {"Data": "Record2", "PartitionKey": "key2"},
        {"Data": "Record3", "PartitionKey": "key3"}
    ]'

# Output includes:
# "FailedRecordCount": 0,
# "Records": [
#     {"SequenceNumber": "49630987899225047315889182017084672..."}
# ]
```

### Example 3: Consume with Lambda

```bash
# Create Lambda function
aws lambda create-function \
    --function-name kinesis-processor \
    --runtime python3.11 \
    --role arn:aws:iam::123456789012:role/lambda-role \
    --handler index.handler \
    --zip-file fileb://function.zip

# Add event source mapping
aws lambda create-event-source-mapping \
    --function-name kinesis-processor \
    --event-source-arn arn:aws:kinesis:us-east-1:123456789012:stream/my-stream \
    --batch-size 100 \
    --bisect-batch-on-function-error \
    --starting-position TRIM_HORIZON

# Lambda handler for Kinesis
# def handler(event, context):
#     for record in event['Records']:
#         payload = base64.b64decode(record['kinesis']['data'])
#         print(f"Processed: {payload}")
```

### Example 4: Kinesis Data Firehose

```bash
# Create Firehose delivery stream
aws firehose create-delivery-stream \
    --delivery-stream-name my-firehose \
    --delivery-stream-type DirectPut \
    --s3-destination-configuration '{
        "RoleARN": "arn:aws:iam::123456789012:role/firehose-role",
        "BucketARN": "arn:aws:s3:::my-bucket",
        "Prefix": "data/",
        "BufferingHints": {
            "SizeInMBs": 128,
            "IntervalInSeconds": 300
        },
        "CompressionFormat": "GZIP"
    }'

# Create with data transformation
aws firehose create-delivery-stream \
    --delivery-stream-name my-transform-stream \
    --delivery-stream-type DirectPut \
    --processing-configuration '{
        "Enabled": true,
        "Processors": [{
            "Type": "Lambda",
            "Parameters": [{
                "ParameterName": "LambdaArn",
                "ParameterValue": "arn:aws:lambda:us-east-1:123456789012:function:transform"
            }]
        }]
    }' \
    --s3-destination-configuration '{...}'

# Put record (direct put)
aws firehose put-record \
    --delivery-stream-name my-firehose \
    --record '{"Data": "Hello Firehose"}'

# Put records batch
aws firehose put-record-batch \
    --delivery-stream-name my-firehose \
    --records '[{"Data": "Record1"}, {"Data": "Record2"}]'
```

## COMMON ISSUES

### 1. Provisioned Throughput Exceeded

**Problem**: Getting ProvisionedThroughputExceededException.

**Solution**:
```bash
# Check shard metrics
aws cloudwatch get-metric-statistics \
    --namespace AWS/Kinesis \
    --metric-name PutRecords.Bytes \
    --start-time 2025-01-01 \
    --end-time 2025-01-02 \
    --period 3600 \
    --statistics Average

# Add more shards
aws kinesis update-shard-count \
    --stream-name my-stream \
    --target-shard-count 10 \
    --scaling-type UNIFORM_SCALING

# Use better partition keys for distribution
# Avoid "hot" keys
```

### 2. Record Delivery Failure

**Problem**: Records not being delivered to consumers.

**Solution**:
```bash
# Check iterator age
aws kinesis describe-stream \
    --stream-name my-stream \
    --query 'StreamDescription.Shards[].SequenceNumberRange'

# Increase consumer processing capacity
aws lambda update-event-source-mapping \
    --uuid <uuid> \
    --batch-size 500

# Check Lambda dead letter queue
```

### 3. Data Ordering Issues

**Problem**: Records out of order.

**Solution**:
# Use same partition key for related records
# Add sequence numbers for ordering
# Implement ordering in consumer application

### 4. High Costs

**Problem**: Kinesis costs too much.

**Solution**:
```bash
# Use on-demand mode for variable workloads
aws kinesis update-stream-mode \
    --stream-name my-stream \
    --stream-mode-details 'StreamMode=ON_DEMAND'

# Reduce shard count when not needed
aws kinesis update-shard-count \
    --stream-name my-stream \
    --target-shard-count 1 \
    --scaling-type UNIFORM_SCALING
```

## PERFORMANCE

### Performance Characteristics

| Metric | Value |
|--------|-------|
| Latency | <1 second |
| Retention | 1-7 days (default 24h) |
| Max record size | 1 MB |
| Max partition key | 256 bytes |
| Shards per stream | 500 (default) |

### Cost Optimization

| Strategy | Savings |
|----------|---------|
| On-demand mode | 50% for variable |
| Extended retention | $0.02/GB/day |
| Firehose batching | Reduce PUT costs |
| Compression | Reduce data size |

## COMPATIBILITY

### Region Availability

- All commercial AWS Regions
- GovCloud available
- China requires account

### Supported Data Formats

| Format | Description |
|--------|-------------|
| JSON | Structured data |
| CSV | Tabular data |
| PARQUET | Columnar (Firehose) |
| AVRO | Schema evolution |

### Integration

| Service | Use Case |
|---------|----------|
| Lambda | Process records |
| Kinesis Analytics | SQL processing |
| S3 | Data lake |
| Redshift | Data warehouse |

## CROSS-REFERENCES

### Related Services

- SQS: Message queuing
- SNS: Pub/Sub messaging
- EventBridge: Event routing
- MSK: Kafka managed service

### Alternatives

| Need | Use |
|------|-----|
| Simple queuing | SQS |
| Kafka | MSK |
| Pub/Sub | SNS |
| Event routing | EventBridge |

### What to Study Next

1. Advanced Kinesis: Analytics, scaling
2. Practical Kinesis: ETL patterns
3. Event Processing: Lambda, EventBridge

## EXAM TIPS

### Key Exam Facts

- Kinesis Data Streams: Custom consumers
- Kinesis Data Firehose: Auto-load to destinations
- Shard: 1MB/s write, 2MB/s read
- Partition key: Routes to shard
- Retention: 1-7 days

### Exam Questions

- **Question**: "Load to S3" = Firehose
- **Question**: "Custom processing" = Data Streams
- **Question**: "SQL analytics" = Kinesis Analytics
