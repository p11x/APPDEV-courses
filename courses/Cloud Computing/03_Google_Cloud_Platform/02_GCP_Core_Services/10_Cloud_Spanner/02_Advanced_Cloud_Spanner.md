---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Spanner
Purpose: Advanced understanding of Cloud Spanner features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_Spanner.md
RelatedFiles: 01_Basic_Cloud_Spanner.md, 03_Practical_Cloud_Spanner.md
UseCase: Enterprise global databases, high-scale applications
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Spanner knowledge enables building globally distributed databases with optimal performance, proper schema design, and cost-effective configurations.

### Why Advanced Spanner

- **Instance Configs**: Regional vs multi-region
- **Query Optimization**: Execution plans
- **Interleaved Tables**: Performance optimization
- **Backup Strategies**: Point-in-time recovery

## 📖 WHAT

### Instance Configurations

| Config | Nodes | Availability | Use Case |
|--------|-------|--------------|----------|
| Regional | 1+ | 99.99% | Single region |
| Multi-region | 3+ replicas | 99.999% | Global apps |

### Advanced Features

**Interleaved Tables**:
- Parent-child in same node
- Faster joins
- Cascade delete

**Query Modes**:
- Partitioned: Large reads
- Batch: Background jobs
- Read-only: Stale data acceptable

## 🔧 HOW

### Example 1: Multi-region Configuration

```bash
# Create multi-region instance
gcloud spanner instances create global-instance \
    --config=nam-eur-asia1 \
    --description="Global Instance" \
    --nodes=3

# Create database with optimal schema
gcloud spanner databases ddl update global-db \
    --instance=global-instance \
    --ddl='CREATE TABLE Users (
        UserId STRING(36) NOT NULL,
        Email STRING(256),
        CreatedAt TIMESTAMP NOT NULL OPTIONS (allow_commit_timestamp=true)
    ) PRIMARY KEY (UserId)'

# Create interleaved table
gcloud spanner databases ddl update global-db \
    --instance=global-instance \
    --ddl='CREATE TABLE Orders (
        OrderId STRING(36) NOT NULL,
        UserId STRING(36) NOT NULL,
        Total DECIMAL,
        CreatedAt TIMESTAMP NOT NULL OPTIONS (allow_commit_timestamp=true)
    ) PRIMARY KEY (UserId, OrderId),
    INTERLEAVE IN PARENT Users ON DELETE CASCADE'
```

### Example 2: Backup Configuration

```bash
# Create on-demand backup
gcloud spanner backups create my-backup \
    --instance=global-instance \
    --database=global-db \
    --retention-days=30

# Schedule automated backups
gcloud spanner backups create daily-backup \
    --instance=global-instance \
    --database=global-db \
    --retention-period=168h

# Restore from backup
gcloud spanner databases restore \
    --backup=my-backup \
    --destination-instance=global-instance \
    --destination-database=restored-db

# Create point-in-time recovery backup
gcloud spanner databases create pitr-db \
    --instance=global-instance \
    --enable-point-in-time-recovery
```

### Example 3: Query Optimization

```bash
# Analyze query plan
gcloud spanner databases execute-sql global-db \
    --instance=global-instance \
    --query='SELECT * FROM Orders WHERE UserId = @userId' \
    --params='{"userId": "123"}' \
    --query-mode=PROFILE

# Create index
gcloud spanner databases ddl update global-db \
    --instance=global-instance \
    --ddl='CREATE INDEX idx_orders_created ON Orders(CreatedAt)'

# Use query hints
gcloud spanner databases execute-sql global-db \
    --instance=global-instance \
    --read-only \
    --query='SELECT /*+ ORDER BY */ * FROM Users'
```

## ⚠️ COMMON ISSUES

### Troubleshooting Spanner Issues

| Issue | Solution |
|-------|----------|
| Slow queries | Check execution plan, add indexes |
| High costs | Right-size nodes |
| Unavailable | Use multi-region |

### Cost Optimization

- Use regional for non-critical
- Right-size node count
- Use query limits

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP Spanner | AWS Aurora Global | Azure Cosmos DB |
|---------|-------------|-------------------|-----------------|
| Global SQL | Yes | Yes | Limited |
| ACID | Yes | Yes | Eventual |
| Horizontal Scale | Yes | Read replicas | Yes |
| 99.999% SLA | Yes | No | No |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud SQL (relational)
- BigQuery (analytics)
- Cloud Storage (backup)

### Study Resources

- Spanner documentation
- Schema design best practices

## ✅ EXAM TIPS

- Interleaved tables for performance
- Multi-region for 99.999% availability
- Node count determines capacity
- Point-in-time recovery available
