---
Category: AWS Cloud Practitioner
Subcategory: Cloud Fundamentals
Concept: Deployment Models - Practical
Purpose: Practical migration implementation including enterprise patterns and operational procedures
Difficulty: practical
Prerequisites: 01_Basic_Deployment_Models.md, 02_Advanced_Deployment_Models.md
RelatedFiles: 01_Basic_Deployment_Models.md, 02_Advanced_Deployment_Models.md
UseCase: Enterprise migration project execution
CertificationExam: AWS Solutions Architect Associate
LastUpdated: 2025
---

## WHY

Practical migration implementation requires hands-on knowledge of AWS tools, operational procedures, and common patterns for successful cloud migration. This knowledge is essential for executing real-world migration projects.

### Why Practical Implementation Matters

- **Executable Plans**: Moving from theory to action
- **Risk Mitigation**: Having rollback procedures ready
- **Operational Excellence**: Monitoring migration progress
- **Business Continuity**: Minimizing downtime

### Common Production Scenarios

- **Web application migration**: Moving to EC2/Lambda
- **Database migration**: Moving to RDS or Aurora
- **Legacy VMware migration**: Preserving virtualized workloads
- **Container migration**: Moving to EKS

## WHAT

### Real-World Architecture

```
                 MIGRATION ARCHITECTURE
                 ===================

     ┌──────────────────────────────────┐
     │      ON-PREMISES ENVIRONMENT     │
     └──────────────┬───────────────────┘
                   │
     ┌─────────────┴──────────────────┐
     │     DATA TRANSFER LAYER        │
     │  ┌──────────┐   ┌──────────┐ │
     │  │ Direct   │   │ Snowball │ │
     │  │ Connect  │   │ Edge    │ │
     │  └──────────┘   └──────────┘ │
     └─────────────┬──────────────────┘
                   │
     ┌─────────────┴──────────────────┐
     │      AWS ENVIRONMENT           │
     │  ┌──────────┐   ┌──────────┐ │
     │  │ Migration│   │ Staging  │ │
     │  │ Hub      │   │ Target   │ │
     │  └──────────┘   └──────────┘ │
     └────────────────────────────────┘
```

### Migration Workflow

| Phase | Activities | Duration |
|-------|------------|----------|
| Assess | Discovery, dependency mapping | 2-4 weeks |
| Plan | Wave planning, resource allocation | 1-2 weeks |
| Migrate | VM replication, cutover | Per wave |
| Validate | Testing, user acceptance | 1 week |
| Decommission | Decommission old systems | 1-2 weeks |

## HOW

### Example 1: Web Application Migration (Replatforming)

```python
# Step 1: Create infrastructure as code
import boto3
import json

ec2 = boto3.resource('ec2')
rds = boto3.client('rds')
asg = boto3.client('autoscaling')

def create_infrastructure():
    # Create VPC
    vpc = ec2.create_vpc(cidr_block='10.0.0.0/16')
    
    # Create subnets
    subnet1 = vpc.create_subnet(
        cidr_block='10.0.1.0/24',
        availability_zone='us-east-1a'
    )
    subnet2 = vpc.create_subnet(
        cidr_block='10.0.2.0/24',
        availability_zone='us-east-1b'
    )
    
    # Create RDS instance
    db_instance = rds.create_db_instance(
        DBInstanceIdentifier='app-db',
        DBInstanceClass='db.t3.micro',
        Engine='mysql',
        MasterUsername='admin',
        MasterUserPassword='SecurePass123!',
        AllocatedStorage=20
    )
    
    # Create Auto Scaling group
    asg.create_auto_scaling_group(
        AutoScalingGroupName='web-asg',
        LaunchTemplate={
            'LaunchTemplateName': 'web-lt'
        },
        MinSize=2,
        MaxSize=10,
        DesiredCapacity=2,
        VPCZoneIdentifier=f'{subnet1.id},{subnet2.id}'
    )
    
    return {'vpc': vpc.id, 'db': db_instance['DBInstanceArn']}

def migrate_application():
    # Use VM Import for server migration
    response = ec2.import_image(
        Description='Web server migration',
        DiskContainers=[{
            'Format': 'vmdk',
            'Url': 's3://migration-bucket/web-server.vmdk'
        }]
    )
    return response['ImportTaskId']
```

### Example 2: Database Migration (Replication)

```bash
#!/bin/bash
# Database migration script using DMS

# Configuration
SOURCE_DB="oracle.onprem.local"
TARGET_DB="app-db.xxxx.rds.us-east-1.amazonaws.com"
REPLICATION_INSTANCE="dms-replication"

# Step 1: Create replication instance
aws dms create-replication-instance \
    --replication-instance-identifier $REPLICATION_INSTANCE \
    --replication-instance-class dms.r5.large \
    --allocated-storage 100 \
    --vpc-security-group-ids sg-0123456789

# Step 2: Create endpoints
aws dms create-endpoint \
    --endpoint-identifier source-db \
    --endpoint-type source \
    --engine-name oracle \
    --server-name $SOURCE_DB \
    --port 1521 \
    --database-name production \
    --username dms_user \
    --password 'DMSPassword123!'

aws dms create-endpoint \
    --endpoint-identifier target-db \
    --endpoint-type target \
    --engine-name mysql \
    --rds-identifier app-db

# Step 3: Create and run full load task
aws dms create-replication-task \
    --replication-task-identifier full-load-task \
    --replication-instance-arn $REPLICATION_ARN \
    --source-endpoint-arn $SOURCE_ARN \
    --target-endpoint-arn $TARGET_ARN \
    --table-mappings '{
        "rules": [{
            "rule-type": "selection",
            "schema-name": "PRODUCTION",
            "table-name": "%",
            "rule-action": "include"
        }]
    }'

# Step 4: Monitor progress
aws dms describe-replication-tasks \
    --filters "Name=replication-task-arn,Values=$TASK_ARN"

# Step 5: Create CDC task for ongoing sync
aws dms create-replication-task \
    --replication-task-identifier cdc-task \
    --replication-instance-arn $REPLICATION_ARN \
    --source-endpoint-arn $SOURCE_ARN \
    --target-endpoint-arn $TARGET_ARN \
    --migration-type cdc \
    --table-mappings '{
        "rules": [{
            "rule-type": "selection",
            "schema-name": "PRODUCTION",
            "table-name": "%",
            "rule-action": "include"
        }]
    }'
```

### Example 3: Complete Migration Runbook

```yaml
# migration-runbook.yaml
name: Production Migration Runbook
version: 1.0

pre_migration:
  - name: Final backup
    command: |
      # Database full export
      mysqldump -h $DB_HOST -u root -p database > backup.sql
      # Upload to S3
      aws s3 cp backup.sql s3://migration-backups/
    duration: 4-8 hours
    
  - name: DNS TTL reduction
    command: |
      aws route53 change-resource-record-sets \
        --hosted-zone-id Z1234567890ABC \
        --changes '...'
    duration: 48 hours before cutover

migration_window:
  - name: Stop application
    command: |
      # Gracefully stop application
      systemctl stop app-server
    duration: 30 minutes
    
  - name: Final sync
    command: |
      # Run DMS CDC for final sync
      aws dms start-replication-task \
        --task-arn $CDC_TASK_ARN \
        --start-time now
    duration: 1-2 hours
    
  - name: DNS cutover
    command: |
      # Update DNS to point to AWS
      aws route53 change-resource-record-sets \
        --hosted-zone-id Z1234567890ABC \
        --changes '...'
    duration: 1 hour

post_migration:
  - name: Verification
    commands:
      - curl https://app.example.com/health
      - psql "connection string" -c "SELECT COUNT(*) FROM users"
    duration: 2 hours
    
  - name: Monitoring
    commands:
      - aws cloudwatch get-metric-statistics
      - Check application logs
    duration: 24-48 hours

rollback:
  command: |
    # Revert DNS
    aws route53 change-resource-record-sets ...
    # Point back to on-premises
  duration: 1 hour
```

## COMMON ISSUES

### 1. Migration Window Exceeded

**Problem**: Cutover window not long enough.

**Solution**:
```bash
# Step 1: Monitor progress
aws dms describe-replication-tasks \
    --filters "Name=status,Values=running"

# Step 2: Stop and resume if needed
aws dms stop-replication-task --task-arn $TASK_ARN

# Step 3: Plan larger window next time
```

### 2. Data Validation Failures

**Problem**: Row counts don't match after migration.

**Solution**:
- Create validation queries
- Compare checksums
- Run in stages with validation between

### 3. Application Connection Issues

**Problem**: Application can't connect to database.

**Solution**:
- Verify security group rules
- Check VPC routing
- Verify connection string
- Test with standalone connection

### 4. Performance Degradation

**Problem**: Application runs slower after migration.

**Solution**:
- Check instance sizes
- Review database indexes
- Enable caching (ElastiCache)
- Review CloudWatch metrics

### 5. User Acceptance Testing Failures

**Problem**: UAT reveals issues not found in testing.

**Solution**:
- Include actual users in testing
- Test with production-like data
- Allow sufficient UAT time

## PERFORMANCE

### Real-World Timing

| Migration Type | Average Duration | Best Practices |
|---------------|----------------|---------------|
| Small web app (1-5 VMs) | 2-3 days | Replatforming |
| Medium web app (5-10 VMs) | 1-2 weeks | Phased approach |
| Database (100GB) | 2-4 hours | DMS full load |
| Database (1TB) | 8-24 hours | DMS with CDC |

### Performance Monitoring

| Metric | Target | Alert |
|--------|--------|-------|
| Replication lag | < 5 minutes | > 30 minutes |
| IOPS | < 80% provisioned | > 90% |
| Latency | < 100ms | > 500ms |

## COMPATIBILITY

### Production Environment Compatibility

| Component | Support | Notes |
|-----------|---------|-------|
| Windows Server 2019 | Full | Requires VM Import |
| Windows Server 2022 | Full | UEFI supported |
| RHEL 7+ | Full | Cloud-init required |
| Ubuntu 20.04+ | Full | Cloud-init required |
| Oracle Linux | Full | KVM hypervisor |

### Network Connectivity

| Connectivity | Required | Bandwidth |
|-------------|----------|----------|
| Direct Connect | Recommended | 1+ Gbps |
| Site-to-Site VPN | Required | 100 Mbps+ |
| Internet | Minimum | 25 Mbps+ |

## CROSS-REFERENCES

### Related Patterns

- Auto Scaling: Post-migration scaling
- RDS: Database targets
- CloudFront: Static asset delivery
- ElastiCache: Performance optimization

### Integration Tools

- CI/CD: CodeDeploy for application deployment
- Monitoring: CloudWatch dashboards
- Backup: AWS Backup service

### What to Study Next

1. High Availability: Multi-AZ deployment
2. Disaster Recovery: Backup strategies
3. Cost Optimization: Savings plans

## EXAM TIPS

### Key Exam Facts

- Migration Hub is central tracking tool
- DMS supports Oracle, SQL Server, MySQL, PostgreSQL
- VM Import requires VMDK, VHD, or RAW formats
- Migration waves help manage dependencies

### Problem Scenarios

- **Scenario**: "Zero-downtime migration" = Run parallel, use DNS
- **Scenario**: "QuickOracle to Aurora" = DMS homogeneous migration
- **Scenario**: "5TB file transfer" = Snowball Edge
- **Scenario**: "Rollback needed" = Keep old environment running