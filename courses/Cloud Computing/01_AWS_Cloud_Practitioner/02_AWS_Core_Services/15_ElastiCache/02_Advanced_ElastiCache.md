---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: ElastiCache Advanced
Purpose: Advanced ElastiCache configurations, clustering, and optimization
Difficulty: advanced
Prerequisites: 01_Basic_ElastiCache.md
RelatedFiles: 01_Basic_ElastiCache.md, 03_Practical_ElastiCache.md
UseCase: Production caching with high availability and performance
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Advanced ElastiCache configurations enable production-grade caching solutions with high availability, scalability, and performance optimization. Understanding these concepts is essential for building resilient applications.

### Why Advanced Configuration Matters

- **High Availability**: Multi-AZ deployments prevent single points of failure
- **Scalability**: Cluster mode scales horizontally
- **Performance Tuning**: Optimize for specific workloads
- **Data Persistence**: Protect against data loss
- **Security**: Encryption, authentication, VPC isolation

### Advanced Use Cases

- **Gaming**: Leaderboards, session stores, real-time updates
- **E-commerce**: Shopping cart, product catalogs, sessions
- **Analytics**: Real-time dashboards, counters, caching query results
- **Machine Learning**: Feature store, model inference caching
- **IoT**: Time-series data, message queuing

## WHAT

### Redis Cluster Mode

**Cluster Mode Disabled**: Single shard, up to 6 nodes (1 primary + 5 replicas).

**Cluster Mode Enabled**: Up to 500 shards, automatic sharding, higher scalability.

```
    CLUSTER MODE ENABLED ARCHITECTURE
    ===============================

           Application
                │
                ▼
    ┌─────────────────────────┐
    │    Redis Cluster       │
    │   (Auto Discovery)     │
    └─────────────────────────┘
                │
    ┌───────────┼───────────┐
    │           │           │
    ▼           ▼           ▼
┌───────┐  ┌───────┐  ┌───────┐
│Shard 1│  │Shard 2│  │Shard N│
│  ◉    │  │  ◉    │  │  ◉    │
│ /|\   │  │ /|\   │  │ /|\   │
│/ | \  │  │/ | \  │  │/ | \  │
└───────┘  └───────┘  └───────┘
```

### High Availability Features

| Feature | Description | Benefit |
|---------|-------------|---------|
| Automatic Failover | Primary fails, replica promotes | Zero downtime |
| Multi-AZ | Nodes across AZs | AZ failure resilience |
| Read Replicas | Async replication | Read scalability |
| Backup/Restore | RDB/AOF snapshots | Disaster recovery |
| Software Patching | Managed updates | Security compliance |

### Encryption Options

| Type | Description | Use Case |
|------|-------------|----------|
| In-Transit | TLS encryption | All traffic |
| At-Rest | AES-256 encryption | Sensitive data |
| Auth | Redis AUTH password | Access control |

## HOW

### Example 1: Create Redis Cluster Mode Enabled

```bash
# Create Redis cluster with cluster mode enabled
aws elasticache create-replication-group \
    --replication-group-id my-clustered-redis \
    --replication-group-description "Redis cluster mode enabled" \
    --cache-node-type cache.m5.large \
    --engine redis \
    --engine-version 7.0 \
    --num-node-groups 3 \
    --replicas-per-node-group 2 \
    --automatic-failover-enabled \
    --multi-az-enabled \
    --cache-subnet-group-name my-subnet-group \
    --node-group-configuration '[
        {"PrimaryAvailabilityZone": "us-east-1a", "ReplicaAvailabilityZones": ["us-east-1b", "us-east-1c"], "Slots": "0-5460"},
        {"PrimaryAvailabilityZone": "us-east-1b", "ReplicaAvailabilityZones": ["us-east-1c", "us-east-1a"], "Slots": "5461-10922"},
        {"PrimaryAvailabilityZone": "us-east-1c", "ReplicaAvailabilityZones": ["us-east-1a", "us-east-1b"], "Slots": "10923-16383"}
    ]'

# Enable encryption
aws elasticache create-replication-group \
    --replication-group-id my-encrypted-redis \
    --replication-group-description "Encrypted Redis cluster" \
    --cache-node-type cache.m5.large \
    --engine redis \
    --engine-version 7.0 \
    --num-cache-clusters 3 \
    --automatic-failover-enabled \
    --at-rest-encryption-enabled \
    --transit-encryption-enabled \
    --auth-token-enabled \
    --cache-subnet-group-name my-subnet-group
```

### Example 2: Configure Redis Parameters

```bash
# Create parameter group for Redis tuning
aws elasticache create-cache-parameter-group \
    --cache-parameter-group-name production-redis-params \
    --cache-parameter-group-family redis7 \
    --description "Production Redis parameters"

# Set performance parameters
aws elasticache modify-cache-parameter-group \
    --cache-parameter-group-name production-redis-params \
    --parameter-name-values '[
        {"ParameterName": "maxmemory", "ParameterValue": "12gb"},
        {"ParameterName": "maxmemory-policy", "ParameterValue": "allkeys-lru"},
        {"ParameterName": "timeout", "ParameterValue": "300"},
        {"ParameterName": "tcp-keepalive", "ParameterValue": "300"},
        {"ParameterName": "hash-max-ziplist-entries", "ParameterValue": "512"},
        {"ParameterName": "hash-max-ziplist-value", "ParameterValue": "64"}
    ]'

# Apply to replication group
aws elasticache modify-replication-group \
    --replication-group-id my-redis-group \
    --cache-parameter-group-name production-redis-params
```

### Example 3: Backup and Restore

```bash
# Create manual backup
aws elasticache create-snapshot \
    --cache-cluster-id my-redis-cluster \
    --snapshot-name my-backup-$(date +%Y%m%d)

# Create backup of replication group
aws elasticache create-snapshot \
    --replication-group-id my-redis-group \
    --snapshot-name my-replica-backup-$(date +%Y%m%d)

# Copy snapshot to another region
aws elasticache copy-snapshot \
    --source-snapshot-name my-backup \
    --target-snapshot-name my-backup-us-west-2 \
    --source-snapshot-region us-east-1

# Restore from snapshot
aws elasticache create-replication-group-from-snapshot \
    --replication-group-id restored-redis \
    --replication-group-description "Restored from backup" \
    --snapshot-name my-backup-20250101 \
    --cache-node-type cache.m5.large
```

### Example 4: Scaling Operations

```bash
# Scale up node type
aws elasticache modify-replication-group \
    --replication-group-id my-redis-group \
    --cache-node-type cache.m5.2xlarge \
    --apply-immediately

# Add read replica
aws elasticache increase-replicas-in-replication-group \
    --replication-group-id my-redis-group \
    --new-replica-count 3

# Remove read replica
aws elasticache decrease-replicas-in-replication-group \
    --replication-group-id my-redis-group \
    --replica-configuration '[
        {"NodeGroupId": "0001", "NewReplicaCount": 2}
    ]'

# Scale Redis cluster (add shards)
aws elasticache modify-replication-group \
    --replication-group-id my-clustered-redis \
    --num-node-groups 6
```

## COMMON ISSUES

### 1. Split-Brain Scenario

**Problem**: Network partition causes multiple primaries.

**Solution**:
```bash
# Ensure proper network configuration
# Use same-region multi-AZ
# Configure proper security groups

# Check cluster status
aws elasticache describe-replication-groups \
    --replication-group-id my-group \
    --query 'ReplicationGroup[].MemberClusters'
```

### 2. Replication Lag

**Problem**: Read replicas out of sync.

**Solution**:
```bash
# Monitor lag
aws elasticache describe-replication-groups \
    --replication-group-id my-group \
    --query 'ReplicationGroup[].ReplicationGroupLocalities'

# Reduce network traffic
# Use smaller key sizes
# Disable debug logging

# Scale up network
```

### 3. Cluster Rebalancing

**Problem**: Uneven data distribution.

**Solution**:
```bash
# Check key distribution
redis-cli CLUSTER SLOTS

# Use hash tags for related keys
# Example: user:1000:profile, user:1000:session

# Reshard the cluster
aws elasticache modify-replication-group \
    --replication-group-id my-clustered-redis \
    --num-node-groups 6
```

### 4. OOM Errors

**Problem**: Out of memory errors.

**Solution**:
```bash
# Monitor memory
aws cloudwatch get-metric-statistics \
    --namespace AWS/ElastiCache \
    --metric-name DatabaseMemoryUsagePercentage \
    --start-time 2025-01-01 \
    --end-time 2025-01-02 \
    --period 3600

# Add eviction policy
aws elasticache modify-cache-parameter-group \
    --cache-parameter-group-name my-params \
    --parameter-name-values '[{"ParameterName": "maxmemory-policy", "ParameterValue": "allkeys-lru"}]'

# Scale up or scale out
```

## PERFORMANCE

### Performance Optimization

| Parameter | Recommended Value | Effect |
|-----------|-------------------|--------|
| maxmemory-policy | allkeys-lru | Evict old keys |
| timeout | 300 | Close idle connections |
| tcp-keepalive | 300 | Connection health |
| slowlog-log-slower-than | 10000 | Log slow queries |

### Monitoring Metrics

| Metric | Description | Alert Threshold |
|--------|-------------|-----------------|
| CPUUtilization | CPU usage | >80% |
| MemoryUsage | Memory usage | >80% |
| Evictions | Keys evicted | >0 |
| ReplicationLag | Replica lag | >30s |
| CurrConnections | Active connections | >10000 |

### Scaling Strategies

1. **Vertical Scaling**: Larger node types
2. **Horizontal Scaling**: More replicas/shards
3. **Read Replicas**: Add replicas for reads
4. **Cluster Mode**: Enable for sharding

## COMPATIBILITY

### Cross-Platform Comparison

| Feature | AWS ElastiCache | Azure Cache | GCP Memorystore |
|---------|-----------------|--------------|-----------------|
| Redis | Yes | Yes | Yes |
| Memcached | Yes | No | No |
| Cluster Mode | Yes | Yes (Premium) | Yes |
| Multi-AZ | Yes | Yes (Premium) | Yes |
| Auto Failover | Yes | Yes | Yes |
| Encryption | Yes | Yes | Yes |
| Serverless | Yes | No | No |

### Supported Instance Types

| Family | vCPU | Memory |
|--------|------|--------|
| cache.m5 | 2-64 | 4-256GB |
| cache.m6i | 2-64 | 4-256GB |
| cache.r5 | 2-64 | 4-256GB |
| cache.r6i | 2-64 | 4-256GB |
| cache.t3 | 2-4 | 1-16GB |

## CROSS-REFERENCES

### Related Services

- RDS: Source database
- DynamoDB: Alternative NoSQL
- DAX: DynamoDB caching
- CloudWatch: Monitoring

### Prerequisites

- Basic ElastiCache concepts
- VPC networking
- Security groups

### What to Study Next

1. Practical ElastiCache: Implementation
2. Serverless Caching: ElastiCache Serverless
3. Multi-tier Caching: CloudFront + ElastiCache

## EXAM TIPS

### Key Exam Facts

- Redis Cluster: Up to 500 shards
- Multi-AZ: Automatic failover
- Encryption: At-rest and in-transit
- Scaling: Vertical and horizontal

### Exam Questions

- **Question**: "Global caching across regions" = Global Datastore
- **Question**: "Serverless Redis" = ElastiCache Serverless
- **Question**: "Encryption at rest" = KMS key required
