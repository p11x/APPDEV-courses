---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: ElastiCache
Purpose: Understanding Amazon ElastiCache for in-memory caching
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_ElastiCache.md, 03_Practical_ElastiCache.md
UseCase: Accelerating database queries and application performance
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Amazon ElastiCache is a managed in-memory caching service that dramatically improves application performance. Understanding ElastiCache is essential because caching is a fundamental technique for reducing latency and database load.

### Why ElastiCache Matters

- **Performance**: Microsecond latency vs. millisecond for databases
- **Cost Reduction**: Reduce database costs by decreasing read loads
- **Scalability**: Auto-scale cache nodes based on demand
- **Managed Service**: Eliminates operational overhead of Redis/Memcached
- **High Availability**: Multi-AZ deployments with automatic failover
- **Data Persistence**: Optional persistence with Redis RDB snapshots

### Industry Statistics

- Caching can reduce database load by 90%+
- Memory access is 100,000x faster than disk
- 80/20 rule: 80% of requests hit 20% of data
- Popular for session storage, leaderboards, real-time analytics

### When NOT to Use ElastiCache

- Static data accessed once: Use S3
- Persistent relational data: Use RDS
- Large datasets exceeding memory: Use DynamoDB DAX
- Complex queries: Use Athena

## WHAT

### ElastiCache Core Concepts

**Cache Engine**: ElastiCache supports two engines:

| Engine | Type | Use Case |
|--------|------|----------|
| Redis | In-memory, persistent | Advanced data structures, pub/sub, clustering |
| Memcached | In-memory, pure cache | Simple key-value, multi-node scaling |

**Node**: Single cache instance (similar to EC2 instance).

**Cluster**: Collection of one or more nodes running the same engine.

**Replication Group**: Primary node + read replicas for high availability.

**Shard**: In Redis Cluster, a subset of data (contains primary + replicas).

### Architecture Diagram

```
                    ELASTICACHE ARCHITECTURE
                    ========================

    ┌──────────────────────────────────────────────────────┐
    │                   APPLICATION                       │
    └──────────────────────────────────────────────────────┘
                            │
                            ▼
    ┌──────────────────────────────────────────────────────┐
    │              ELASTICACHE CLIENT                     │
    │  - Connection pooling                               │
    │  - Node discovery                                   │
    │  - Read/Write routing                               │
    └──────────────────────────────────────────────────────┘
                            │
          ┌─────────────────┼─────────────────┐
          ▼                 ▼                 ▼
    ┌──────────┐     ┌──────────┐     ┌──────────┐
    │ Primary  │     │  Read    │     │  Read    │
    │  Node    │────▶│ Replica  │────▶│ Replica  │
    │ (Writer) │     │ (Reader) │     │ (Reader) │
    └──────────┘     └──────────┘     └──────────┘
          │                 │                 │
          └─────────────────┴─────────────────┘
                            │
    ┌──────────────────────────────────────────────────────┐
    │              IN-MEMORY DATA STORE                   │
    │  - Keys/Values                                      │
    │  - Data structures                                  │
    │  - Pub/Sub messages                                 │
    └──────────────────────────────────────────────────────┘
```

### Key Features

**Redis vs Memcached**:

| Feature | Redis | Memcached |
|---------|-------|-----------|
| Data Structures | Strings, Lists, Sets, Sorted Sets, Hashes, Bitmaps, HyperLogLog | Strings only |
| Persistence | Yes (RDB, AOF) | No |
| Clustering | Yes (Redis Cluster) | Yes (Auto Discovery) |
| Pub/Sub | Yes | No |
| Lua Scripts | Yes | No |
| Transactions | Yes (WATCH/MULTI/EXEC) | No |

## HOW

### Example 1: Create Redis Cluster

```bash
# Create a Redis cluster (single node)
aws elasticache create-cache-cluster \
    --cache-cluster-id my-redis-cluster \
    --cache-node-type cache.m5.large \
    --engine redis \
    --engine-version 7.0 \
    --num-cache-nodes 1 \
    --cache-subnet-group-name my-subnet-group \
    --vpc-security-group-ids sg-12345678

# Create a Redis cluster with read replica
aws elasticache create-replication-group \
    --replication-group-id my-redis-group \
    --replication-group-description "Primary with read replicas" \
    --cache-node-type cache.m5.large \
    --engine redis \
    --engine-version 7.0 \
    --num-cache-clusters 3 \
    --automatic-failover-enabled \
    --multi-az-enabled \
    --cache-subnet-group-name my-subnet-group

# Expected output:
# {
#     "ReplicationGroup": {
#         "ReplicationGroupId": "my-redis-group",
#         "Status": "creating",
#         ...
#     }
# }
```

### Example 2: Connect to ElastiCache

```bash
# Get cluster endpoints
aws elasticache describe-replication-groups \
    --replication-group-id my-redis-group \
    --output json

# Output includes:
# "PrimaryEndpoint": {"Address": "master.my-redis-group.xxxx.use1.cache.amazonaws.com", "Port": 6379}
# "ReadEndpoint": {"Address": "my-redis-group.xxxx.use1.cache.amazonaws.com", "Port": 6379}

# Connect using redis-cli
redis-cli -h master.my-redis-group.xxxx.use1.cache.amazonaws.com -p 6379

# In Redis CLI:
# > SET mykey "Hello Redis"
# > GET mykey
# "Hello Redis"
# > SET counter 100
# > INCR counter
# (integer) 101
```

### Example 3: Memcached Cluster with Auto Discovery

```bash
# Create Memcached cluster
aws elasticache create-cache-cluster \
    --cache-cluster-id my-memcached-cluster \
    --cache-node-type cache.m5.large \
    --engine memcached \
    --engine-version 1.6.12 \
    --num-cache-nodes 3 \
    --cache-subnet-group-name my-subnet-group \
    --vpc-security-group-ids sg-12345678

# Configure Auto Discovery in application
# Python example with pymemcache:
from pymemcache.client.hash import HashClient

# Nodes will be discovered automatically
nodes = [
    'my-memcached-cluster.xxxx.use1.cache.amazonaws.com:11211'
]
client = HashClient(nodes)
client.set('key', 'value')
value = client.get('key')
```

### Example 4: Configure Parameters and Security

```bash
# Create custom parameter group
aws elasticache create-cache-parameter-group \
    --cache-parameter-group-name my-redis-params \
    --cache-parameter-group-family redis7 \
    --description "Custom Redis parameters"

# Modify parameters
aws elasticache modify-cache-parameter-group \
    --cache-parameter-group-name my-redis-params \
    --parameter-name-values "[{\"ParameterName\": \"maxmemory-policy\", \"ParameterValue\": \"allkeys-lru\"}]"

# Apply to cluster
aws elasticache modify-cache-cluster \
    --cache-cluster-id my-redis-cluster \
    --cache-parameter-group-name my-redis-params

# Create security group rule
aws ec2 authorize-security-group-ingress \
    --group-id sg-12345678 \
    --protocol tcp \
    --port 6379 \
    --cidr 10.0.0.0/16
```

## COMMON ISSUES

### 1. Connection Timeout

**Problem**: Application cannot connect to ElastiCache.

**Solution**:
```bash
# Check security group allows traffic
aws ec2 describe-security-groups \
    --group-id sg-12345678 \
    --query 'SecurityGroups[0].IpPermissions'

# Verify subnet configuration
aws elasticache describe-cache-subnet-groups \
    --cache-subnet-group-name my-subnet-group

# Check if cluster is available
aws elasticache describe-cache-clusters \
    --cache-cluster-id my-redis-cluster \
    --show-cache-node-info
```

### 2. Data Loss on Node Failure

**Problem**: Data lost when Redis node fails.

**Solution**:
```bash
# Enable automatic failover
aws elasticache create-replication-group \
    --replication-group-id my-group \
    --automatic-failover-enabled \
    --multi-az-enabled \
    --cache-node-type cache.m5.large \
    --engine redis

# Enable Redis persistence
aws elasticache describe-replication-groups \
    --replication-group-id my-group \
    --query 'ReplicationGroup[].AtRestEncryptionEnabled'
```

### 3. Out of Memory

**Problem**: Redis node runs out of memory.

**Solution**:
```bash
# Monitor memory usage
aws elasticache describe-cache-nodes \
    --cache-cluster-id my-cluster \
    --query 'CacheNodes[].CacheNodeMetrics'

# Set eviction policy
aws elasticache modify-cache-parameter-group \
    --cache-parameter-group-name my-params \
    --parameter-name-values "[{\"ParameterName\": \"maxmemory-policy\", \"ParameterValue\": \"allkeys-lru\"}]"

# Scale up node type
aws elasticache modify-cache-cluster \
    --cache-cluster-id my-cluster \
    --cache-node-type cache.m5.xlarge
```

### 4. High CPU Usage

**Problem**: Memcached node CPU at 100%.

**Solution**:
```bash
# Add more nodes
aws elasticache modify-cache-cluster \
    --cache-cluster-id my-memcached-cluster \
    --num-cache-nodes 5

# Or scale to larger node type
aws elasticache modify-cache-cluster \
    --cache-cluster-id my-cluster \
    --cache-node-type cache.m5.2xlarge
```

### 5. Slow Queries

**Problem**: Redis slow log queries.

**Solution**:
```bash
# Enable slow log
aws elasticache modify-cache-parameter-group \
    --cache-parameter-group-name my-params \
    --parameter-name-values "[{\"ParameterName\": \"slowlog-log-slower-than\", \"ParameterValue\": \"10000\"}]"

# View slow log via Redis CLI
redis-cli SLOWLOG GET 10
```

## PERFORMANCE

### Performance Characteristics

| Metric | Redis | Memcached |
|--------|-------|-----------|
| Latency | <1ms | <1ms |
| Max Connections | 10,000+ | 10,000+ |
| Max Memory | Depends on node | Depends on node |
| Throughput | High | Very High |
| Data Size Limit | 512MB per key | 1MB per key |

### Best Practices

1. **Use Connection Pooling**: Reuse connections
2. **Enable Multi-AZ**: Automatic failover
3. **Use Read Replicas**: Offload read traffic
4. **Configure Eviction Policy**: allkeys-lru for general use
5. **Use Pipeline**: Batch commands
6. **Enable Compression**: For large values

### Cost Optimization

| Strategy | Savings |
|----------|---------|
| Use smaller node + more nodes | 30-50% |
| Reserved Nodes | Up to 60% |
| Right-size nodes | Match workload |
| Memcached vs Redis | 20-30% (simpler use cases) |

## COMPATIBILITY

### Region Availability

- All commercial AWS Regions
- GovCloud (US) regions available
- China regions (requires account)

### Supported Engines

| Engine | Version |
|--------|---------|
| Redis | 7.0, 6.2, 6.0, 5.0.6, 5.0.5, 5.0.4, 5.0.3, 5.0, 4.0.10, 4.0.10 |
| Memcached | 1.6.12, 1.6.6, 1.6.5, 1.5.16, 1.5.10 |

### SDK Support

- AWS SDK for Python (boto3)
- AWS SDK for Java
- AWS SDK for Node.js
- AWS SDK for .NET
- AWS SDK for Go
- AWS SDK for Ruby

### Integration

- EC2: Direct access
- Lambda: VPC-based access
- RDS: Cache for database
- CloudWatch: Metrics monitoring

## CROSS-REFERENCES

### Related Services

- RDS: Source database for caching
- DynamoDB Accelerator (DAX): DynamoDB caching
- CloudFront: Edge caching
- ElastiCache Serverless: New serverless option

### Alternatives

| Need | Use |
|------|-----|
| DynamoDB caching | DAX |
| Session storage | DynamoDB DAX or ElastiCache |
| Simple key-value | Memcached |
| Real-time analytics | Redis with data structures |
| Pub/Sub messaging | Redis |

### What to Study Next

1. Advanced ElastiCache: Clustering, tuning
2. Practical ElastiCache: Best practices, use cases
3. RDS: Database concepts for caching

## EXAM TIPS

### Key Exam Facts

- ElastiCache = Managed Redis or Memcached
- Two engines: Redis (persistent) and Memcached (pure cache)
- Redis supports data structures, Memcached does not
- Multi-AZ with automatic failover available
- Read replicas for scaling reads

### Exam Questions

- **Question**: "Sub-millisecond latency" = ElastiCache
- **Question**: "Session storage" = ElastiCache (Redis or Memcached)
- **Question**: "Data structures" = Redis required
- **Question**: "No persistence needed" = Memcached preferred
