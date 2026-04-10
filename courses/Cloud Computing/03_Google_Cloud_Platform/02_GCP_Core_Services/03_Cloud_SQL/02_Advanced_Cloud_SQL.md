---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud SQL
Purpose: Advanced understanding of Cloud SQL features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_SQL.md
RelatedFiles: 01_Basic_Cloud_SQL.md, 03_Practical_Cloud_SQL.md
UseCase: Production databases, high availability, scaling strategies
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Cloud SQL knowledge enables building resilient database architectures, implementing proper scaling, and managing production database workloads.

### Why Advanced Cloud SQL

- **High Availability**: Multi-zone failover, automated backups
- **Read Replicas**: Horizontal scaling for read-heavy workloads
- **Connection Pooling**: Efficient connection management
- **Point-in-Time Recovery**: Granular recovery options

## 📖 WHAT

### HA Configuration Options

| Option | Description | RPO | RTO |
|--------|-------------|-----|-----|
| Single Zone | Development only | N/A | N/A |
| Multi-zone (HA) | Primary + standby | <1 min | <1 min |
| Cross-region | DR region | <5 min | <15 min |

### Replication Types

- **Read Replicas**: Async, same/different region
- **Cross-region**: Disaster recovery
- **External Replica**: MySQL replication

### Connection Management

- **Cloud SQL Proxy**: Secure connections
- **Connection Pooling**: App Engine, Cloud Run
- **Private IP**: VPC networking

## 🔧 HOW

### Example 1: HA Configuration

```bash
# Create HA-enabled Cloud SQL instance
gcloud sql instances create prod-instance \
    --database-version=POSTGRES_15 \
    --tier=db-custom-2-4096 \
    --zone=us-central1-a \
    --enable-high-availability \
    --maintenance-window-day=SUNDAY \
    --maintenance-window-hour=2 \
    --backup-start-time="03:00" \
    --enable-point-in-time-recovery \
    --retained-backups-count=30 \
    --storage-auto-increase \
    --storage-type=SSD

# Configure backup schedule
gcloud sql instances patch prod-instance \
    --backup-start-time="03:00" \
    --enabled-backup-recovery
```

### Example 2: Read Replica Configuration

```bash
# Create read replica in same region
gcloud sql instances create read-replica-1 \
    --database-version=POSTGRES_15 \
    --tier=db-custom-2-4096 \
    --zone=us-central1-b \
    --source-instance=prod-instance \
    --replica-type=READ_REPLICA

# Create cross-region read replica
gcloud sql instances create read-replica-dr \
    --database-version=POSTGRES_15 \
    --tier=db-custom-2-4096 \
    --zone=us-east1-a \
    --source-instance=prod-instance \
    --replica-type=READ_REPLICA

# Configure replica settings
gcloud sql instances update prod-instance \
    --read-replica-config-replication-mode=ASYNC

# Promote read replica (for DR testing)
gcloud sql instances promote-replica read-replica-dr
```

### Example 3: VPC and Connection Configuration

```bash
# Create instance with private IP only
gcloud sql instances create vpc-instance \
    --database-version=POSTGRES_15 \
    --tier=db-custom-2-4096 \
    --network=projects/my-project/global/networks/vpc-name \
    --enable-private-ip \
    --no-public-ip

# Authorize network
gcloud sql instances authorize-public-access vpc-instance \
    --network=0.0.0.0/0

# Set up Cloud SQL Auth proxy
# Download proxy
curl -o cloud-sql-proxy https://storage.googleapis.com/cloud-sql-connectors/cloud-sql-proxy/v2.8.0/cloud-sql-proxy.linux.amd64

# Start proxy
./cloud-sql-proxy --port 5432 my-project:us-central1:prod-instance

# Connect via proxy
psql -h 127.0.0.1 -U postgres -d mydb
```

## ⚠️ COMMON ISSUES

### Troubleshooting Database Issues

| Issue | Solution |
|-------|----------|
| Connection timeouts | Check firewall, private IP |
| Slow queries | Enable query insights |
| HA failover | Verify health check |
| Backup failures | Check storage quota |

### Performance Tuning

- Use SSD storage type
- Right-size instance tiers
- Enable query insights
- Use connection pooling

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP | AWS | Azure |
|---------|-----|-----|-------|
| Managed DB | Yes | RDS | SQL Database |
| HA | Yes (multi-zone) | Yes (multi-AZ) | Yes |
| Read Replicas | Yes | Yes | Yes |
| Point-in-Time | Yes | Yes | Yes |
| Private Link | Yes | VPC Peering | Private Link |

## 🔗 CROSS-REFERENCES

### Related Topics

- VPC Networking
- Cloud Run (connection)
- Cloud Functions (data)

### Study Resources

- Cloud SQL documentation
- Best practices for Cloud SQL

## ✅ EXAM TIPS

- HA = automatic failover
- Read replicas for read scaling
- Point-in-time recovery enabled by default
- Use Cloud SQL Proxy for secure connections
