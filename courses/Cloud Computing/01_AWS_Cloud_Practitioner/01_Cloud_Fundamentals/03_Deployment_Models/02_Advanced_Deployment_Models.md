---
Category: AWS Cloud Practitioner
Subcategory: Cloud Fundamentals
Concept: Deployment Models - Advanced
Purpose: Advanced migration strategies, hybrid architectures, and multi-phase migration planning
Difficulty: advanced
Prerequisites: 01_Basic_Deployment_Models.md
RelatedFiles: 01_Basic_Deployment_Models.md, 03_Practical_Deployment_Models.md
UseCase: Enterprise-scale migration planning and execution
CertificationExam: AWS Solutions Architect Associate
LastUpdated: 2025
---

## WHY

Enterprise migrations require sophisticated planning that balances business continuity with cloud transformation goals. Understanding advanced deployment strategies enables organizations to migrate complex workloads while maintaining operational continuity and optimizing for cloud-native benefits.

### Why Advanced Knowledge Matters

- **Hybrid Operations**: Most enterprises run hybrid environments during migration
- **Migration Waves**: Large migrations require phased approaches
- **Dependency Management**: Complex application dependencies require careful planning
- **Risk Mitigation**: Enterprise migrations need rollback strategies

### Advanced Use Cases

- **Multi-phase migration**: Migrating in waves based on dependency analysis
- **Database migration**: Moving databases with minimal downtime
- **Large-scale VM migration**: Migrating hundreds of VMs
- **Application modernization**: Modernizing during migration

## WHAT

### Migration Architecture Patterns

| Pattern | Description | Best For |
|---------|------------|---------|
| Big Bang | Migrate everything at once | Small projects |
| Phased | Migrate in phases | Medium projects |
| Parallel Run | Run both in parallel | Zero-downtime needs |
| Hybrid | Keep some on-premises | Long-term transition |

### Cross-Platform Comparison

| Capability | AWS | Azure | GCP |
|-----------|-----|-------|-----|
| VM Import/Export | VM Import/Export | Azure Site Recovery | Migrate for Compute Engine |
| Database Migration | DMS | Azure Database Migration | Database Migration Service |
| Assessment Tools | Application Discovery | Azure Migrate | Migrate |
| Agent-based Discovery | Yes | Yes | Yes |
| Wave Planning | Migration Hub | Hub | Migration Hub |
| Cutover Validation | Yes | Yes | Yes |

### Database-Specific Migrations

| Source Database | Target AWS | Target Azure | Target GCP |
|---------------|------------|-------------|------------|
| Oracle | RDS Oracle, Aurora | Azure SQL, Oracle DB | Cloud SQL, Spanner |
| SQL Server | RDS SQL, Aurora | Azure SQL | Cloud SQL |
| MySQL | RDS MySQL, Aurora | Azure DB for MySQL | Cloud SQL |
| PostgreSQL | RDS PostgreSQL | Azure DB for PostgreSQL | Cloud SQL |
| MongoDB | DocumentDB | Cosmos DB | MongoDB Atlas |

## HOW

### Example 1: Phased Migration with Wave Planning

```bash
# Create Migration Hub workspace
aws mgh create-progress-update-stream \
    --progress-update-stream-name enterprise-migration

# Register source servers
aws mgh list-source-servers \
    --filters '{"logicalDirectConnectIdFilters": []}'

# Group servers into waves
aws mgh create-wave \
    --wave-name "wave-1-web-tier" \
    --wave-description "Web tier servers"

# Add servers to wave
aws mgh tag-resource \
    --resource-arn arn:aws:mgh:us-east-1:123456789:source-server/server-123 \
    --tags Key=Wave,Value=wave-1-web-tier

# Create replication job for wave
aws m gn create-replication-job \
    --source-server-id server-123 \
    --launch-configuration '{
        "launchInstanceThrough Ec2": {
            "instanceType": "t3.medium"
        }
    }'
```

### Example 2: Database Migration with DMS

```bash
# Create DMS replication instance
aws dms create-replication-instance \
    --replication-instance-identifier prod-replication \
    --replication-instance-class dms.r5.large \
    --allocated-storage 100 \
    --vpc-security-group-ids sg-0123456789

# Create source endpoint (on-premises Oracle)
aws dms create-endpoint \
    --endpoint-identifier source-oracle \
    --endpoint-type source \
    --engine-name oracle \
    --server-name oracle.onprem.local \
    --port 1521 \
    --database-name mydb \
    --username admin \
    --password 'SecurePass123!'

# Create target endpoint (RDS Oracle)
aws dms create-endpoint \
    --endpoint-identifier target-oracle \
    --endpoint-type target \
    --engine-name oracle \
    --rds-identifier oracle-prod

# Create migration task
aws dms create-migration-task \
    --migration-task-identifier full-load-migration \
    --replication-task-arn arn:aws:dms:us-east-1:123456789:task/abc123 \
    --source-endpoint-arn arn:aws:dms:us-east-1:123456789:endpoint/source-oracle \
    --target-endpoint-arn arn:aws:dms:us-east-1:123456789:endpoint/target-oracle

# Start task
aws dms start-replication-task \
    --task-arn arn:aws:dms:us-east-1:123456789:task/abc123 \
    --start-time 2024-01-01T00:00:00Z
```

### Example 3: Hybrid DNS Cutover

```bash
# Create Route 53 private hosted zone
aws route53 create-hosted-zone \
    --name internal.example.com \
    --caller-reference "migration-$(date +%s)" \
    --hosted-zone-config '{
        "comment": "Internal DNS for hybrid migration",
        "privateZone": true
    }' \
    --vpc '{
        "vpcRegion": "us-east-1",
        "vpcId": "vpc-0123456789abcdef0"
    }'

# Create weighted record sets
aws route53 change-resource-record-sets \
    --hosted-zone-id Z1234567890ABC \
    --change-batch '{
        "Changes": [
            {
                "Action": "CREATE",
                "ResourceRecordSet": {
                    "Name": "app.internal.example.com",
                    "Type": "A",
                    "SetIdentifier": "on-premises",
                    "HealthCheckId": "abc123",
                    "TTL": 300,
                    "ResourceRecords": [{"Value": "10.0.1.50"}]
                }
            },
            {
                "Action": "CREATE",
                "ResourceRecordSet": {
                    "Name": "app.internal.example.com",
                    "Type": "A",
                    "SetIdentifier": "aws",
                    "Weight": 0,
                    "TTL": 300,
                    "ResourceRecords": [{"Value": "10.0.2.50"}]
                }
            }
        ]
    }'

# Gradually shift traffic
aws route53 change-resource-record-sets \
    --hosted-zone-id Z1234567890ABC \
    --change-batch '{
        "Changes": [{
            "Action": "UPSERT",
            "ResourceRecordSet": {
                "Name": "app.internal.example.com",
                "Type": "A",
                "SetIdentifier": "aws",
                "Weight": 100,
                "TTL": 300,
                "ResourceRecords": [{"Value": "10.0.2.50"}]
            }
        }]
    }'
```

## COMMON ISSUES

### 1. Database Migration Performance

**Problem**: DMS full load is too slow for large databases.

**Solution**:
```bash
# Use DMS Change Data Capture (CDC)
aws dms describe-replication-tasks \
    --filters "Name=replication-task-identifier,Values=my-task"

# Enable CDC for ongoing sync
aws dms start-replication-task \
    --task-arn my-task-arn \
    --start-time now
```

### 2. Application Dependencies

**Problem**: Applications depend on on-premises services.

**Solution**:
- Use AWS Direct Connect for private connectivity
- Implement DNS split-brain for gradual cutover
- Create VPN for temporary access

### 3. Large Data Transfer

**Problem**: Moving terabytes of data takes too long.

**Solution**:
```bash
# Use Snowball Edge
aws snowball create-job \
    --job-type EXPORT \
    --storage-unit TB \
    --address-id address-id

# Enable S3 Transfer Acceleration
aws s3 cp large-data/ s3://my-bucket/ \
    --transfer-acceleration
```

### 4. VM Compatibility

**Problem**: VM doesn't boot in AWS.

**Solution**:
- Check hypervisor compatibility (VMware to XEN)
- Convert disk format (VMDK to VHD for Windows)
- Re-import with corrected drivers

### 5. DNS Propagation Delay

**Problem**: DNS changes take time to propagate.

**Solution**:
- Use Route 53 weighted routing
- Reduce TTL before migration
- Implement application-level routing

## PERFORMANCE

### Migration Timing Benchmarks

| Workload Type | Typical Duration | Key Factors |
|--------------|----------------|------------|
| Single VM | 1-4 hours | Data size, network |
| 10 VMs | 1-3 days | Dependencies |
| 100 VMs | 2-6 weeks | Wave planning |
| Database (1TB) | 4-24 hours | DMS configuration |
| Large Database | Days | CDC required |

### Performance Optimization

| Technique | Speed Improvement |
|-----------|----------------|
| Snowball Edge | 10-100x faster |
| Transfer Acceleration | 2-3x faster |
| Multi-threaded DMS | 2-4x faster |
| Parallel VM Import | 3-5x faster |

## COMPATIBILITY

### VM Migration Compatibility

| Source | AWS Compatible | Notes |
|--------|---------------|-------|
| VMware | Yes | Direct import |
| Hyper-V | Yes | Import to XEN |
| Physical | Yes | Via agent |
| AWS to Azure | Yes | Cross-cloud |
| Azure to AWS | Limited | Some formats |

### Network Requirements

| Item | Requirement |
|------|-------------|
| Internet | 25 Mbps minimum |
| Direct Connect | 1 Gbps recommended |
| VPN | Site-to-site |
| DNS | Outbound access |

## CROSS-REFERENCES

### Related AWS Services

- AWS Migration Hub: Central tracking
- AWS Application Discovery: Assessment
- AWS Server Migration Service: VM migration
- AWS Database Migration Service: Database migration
- AWS VM Import/Export: Legacy VM import

### Prerequisites

- Basic Deployment Models
- Understanding of cloud services

### What to Study Next

1. Practical Deployment: Hands-on implementation
2. Cost Management: Migration costs
3. Architecture: Design patterns

## EXAM TIPS

### Key Exam Facts

- Migration strategies: Rehost, Replatform, Refactor, Repurchase, Retire
- DMS supports homogeneous and heterogeneous migrations
- Migration Hub provides central tracking
- Snowball Family for bulk data transfer

### Exam Questions

- **Question**: "Migrate Oracle to RDS Oracle" = DMS homogeneous
- **Question**: "100 VMs, phased approach" = Wave planning
- **Question**: "5TB data transfer" = Snowball Edge
- **Question**: "Hybrid DNS" = Weighted routing