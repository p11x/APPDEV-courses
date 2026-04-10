---
Category: AWS Cloud Practitioner
Subcategory: AWS Architecture
Concept: Disaster Recovery - Advanced
Purpose: Advanced DR strategies including multi-region active-active, continuous backup replication, and automated failover
Difficulty: advanced
Prerequisites: 01_Basic_Disaster_Recovery.md
RelatedFiles: 01_Basic_Disaster_Recovery.md, 03_Practical_Disaster_Recovery.md
UseCase: Mission-critical DR implementation
CertificationExam: AWS Solutions Architect Professional
LastUpdated: 2025
---

## WHY

Advanced DR requires sophisticated replication, automated failover, and validated recovery procedures for mission-critical workloads requiring minimal RTO and RPO.

### Why Advanced DR Matters

- **Zero Downtime**: Automated failover for critical systems
- **Data Protection**: Continuous replication
- **Validation**: Regular DR testing
- **Compliance**: Regulatory requirements

## WHAT

### DR Comparison Matrix

| Strategy | RTO | RPO | Infrastructure | Cost |
|----------|-----|-----|----------------|------|
| Backup/restore | Hours | Hours | 0% | Low |
| Pilot light | Minutes | Minutes | 50% | Medium |
| Warm standby | Minutes | Seconds | 100% | High |
| Multi-region | Seconds | Seconds | 200% | Very High |

### Cross-Platform Comparison

| Technique | AWS | Azure | GCP |
|-----------|-----|-------|-----|
| Backup | Backup | Backup | Backup |
| Replication | S3, RDS | Storage | Storage |
| Failover DNS | Route 53 | DNS | Cloud DNS |
| Auto-failover | ASG + LB | VMSS + LB | MIG + LB |

### AWS Service DR Support

| Service | Native DR | Cross-Region | Self-Service |
|---------|----------|--------------|--------------|
| S3 | Yes | Cross-region | Replication |
| RDS | Multi-AZ | Read replica | Backup |
| DynamoDB | Global Tables | Yes | On-demand |
| EFS | Yes | Replication | Backup |
| FSx | Yes | Backup | Snapshot |

## HOW

### Example 1: Multi-Region Active-Active

```bash
# Create primary stack
aws cloudformation create-stack \
    --stack-name primary-app \
    --template-body file://template.yaml \
    --parameters ParameterKey=Region,ParameterValue=us-east-1

# Create secondary stack
aws cloudformation create-stack \
    --stack-name secondary-app \
    --template-body file://template.yaml \
    --parameters ParameterKey=Region,ParameterValue=us-west-2

# Configure Route 53 weighted routing
aws route53 change-resource-record-sets \
    --hosted-zone-id zone-id \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "SetIdentifier": "primary",
                "Weight": 100,
                "TTL": 60,
                "ResourceRecords": [{"Value": "1.2.3.4"}]
            }
        }]
    }'

# Enable S3 replication
aws s3 put-bucket-replication \
    --bucket primary-bucket \
    --replication-configuration '{
        "Role": "arn:aws:iam::123456789:role/replication",
        "Rules": [{
            "ID": "replicate",
            "Status": "Enabled",
            "Priority": 1,
            "DeleteMarkerReplication": {"Status": "Enabled"},
            "Destination": {
                "Bucket": "arn:aws:s3:::secondary-bucket",
                "Region": "us-west-2"
            }
        }]
    }'
```

### Example 2: Cross-Region Database

```bash
# Create cross-region read replica
aws rds create-db-instance-read-replica \
    --db-instance-identifier primary-db \
    --source-db-instance-identifier primary-db \
    --region us-west-2 \
    --db-instance-class db.r5.large

# Promote to primary
aws rds promote-read-replica \
    --db-instance-identifier read-replica \
    --backup-retention-period 7 \
    --preferred-maintenance-window "mon:04:00-mon:05:00"

# Configure DMS for ongoing replication
aws dms create-endpoint \
    --endpoint-identifier source \
    --endpoint-type source \
    --engine-name postgres \
    --server-name source.123456789.us-east-1.rds.amazonaws.com \
    --port 5432 \
    --database-name admin

aws dms create-endpoint \
    --endpoint-identifier target \
    --endpoint-type target \
    --engine-name postgres \
    --server-name target.us-west-2.rds.amazonaws.com \
    --port 5432 \
    --database-name admin

aws dms create-replication-task \
    --replication-task-id task-001 \
    --replication-instance-arn instance-arn \
    --source-endpoint-arn source-arn \
    --target-endpoint-arn target-arn \
    --migration-type full-load-and-cdc \
    --table-mappings file://mappings.json
```

### Example 3: Automated Backup and Restore

```bash
# Configure S3 bucket lifecycle
aws s3 put-bucket-lifecycle-configuration \
    --bucket backup-bucket \
    --lifecycle-configuration '{
        "Rules": [{
            "ID": "archive",
            "Status": "Enabled",
            "Transitions": [{
                "Days": 30,
                "StorageClass": "GLACIER"
            }]
        }]
    }'

# Create cross-region backup
aws s3 sync local-backup/ s3://backup-bucket/

# Automate with EventBridge
aws events put-rule \
    --name daily-backup \
    --schedule-expression "rate(1 day)"

aws events put-targets \
    --rule daily-backup \
    --targets '[{
        "Id": "backup",
        "Arn": "arn:aws:lambda:region:account:function:backup-function"
    }]'
```

### Example 4: DR Automation

```bash
# Lambda function for failover
import boto3

def failover_handler(event, context):
    asg = boto3.client('autoscaling')
    elb = boto3.client('elbv2')
    
    # Detach from primary ASG
    asg.update-auto-scaling-group(
        AutoScalingGroupName='primary-asg',
        MinSize=0,
        DesiredCapacity=0
    )
    
    # Scale up secondary
    asg.update-auto-scaling-group(
        AutoScalingGroupName='secondary-asg',
        MinSize=2,
        DesiredCapacity=2
    )
    
    # Failover DNS to secondary
    route53 = boto3.client('route53')
    route53.change_resource_record_sets(
        HostedZoneId='zone-id',
        ChangeBatch={
            'Changes': [{
                'Action': 'UPSERT',
                'ResourceRecordSet': {
                    'Name': 'app.example.com',
                    'Type': 'A',
                    'TTL': 60,
                    'ResourceRecords': [{'Value': '5.6.7.8'}]
                }
            }]
        }
    )

# Enable trigger on alarm
aws events put-rule \
    --name dr-failover \
    --event-pattern '{
        "source": ["aws.cloudwatch"],
        "detail-type": ["EC2 Instance State-change Notification"]
    }'
```

## COMMON ISSUES

### 1. Replication Lag

**Problem**: High lag in read replica.

**Solution**:
- Upgrade instance size
- Check network throughput
- Use Multi-AZ

### 2. Failover Not Triggering

**Problem**: Automated failover not working.

**Solution**:
- Check IAM permissions
- Verify health check configuration
- Review event triggers

### 3. Data Loss

**Problem**: Data loss after failover.

**Solution**:
- Reduce RPO with more frequent backup
- Use CDC replication
- Test regularly

### 4. Cost Overruns

**Problem**: DR costs too high.

**Solution**:
- Use on-demand replication
- Right-size DR infrastructure
- Clean up unused resources

### 5. Validation Failures

**Problem**: DR test failures.

**Solution**:
- Document recovery steps
- Test in non-production
- Update runbooks

## PERFORMANCE

### DR Metrics by Strategy

| Strategy | RTO | RPO | Cost/hr |
|----------|-----|-----|---------|
| Backup | Hours | Hours | $0.05 |
| Pilot | Minutes | Minutes | $0.50 |
| Warm | Minutes | Seconds | $2.00 |
| Active | Seconds | Seconds | $5.00 |

### Test Frequency Recommendation

| Tier | Test Frequency | Team |
|------|---------------|------|
| Critical | Monthly | DR team |
| Important | Quarterly | Ops |
| Standard | Semi-annual | IT |

## COMPATIBILITY

### AWS DR Services

| Service | Backup | Replication | Failover |
|---------|-------|--------------|----------|
| EC2 | AMI | AMI copy | ASG |
| RDS | Automated | Read replica | Promote |
| S3 | Versioning | Replication | None |
| DynamoDB | On-demand | Global tables | None |
| EFS | Backup | Replication | Restore |

### Region Support

| Feature | Available Regions | Limitations |
|---------|------------------|--------------|
| S3 Replication | All | None |
| Read Replica | Limited pairs | Same engine |
| Global Tables | Limited | DynamoDB only |

## CROSS-REFERENCES

### Prerequisites

- DR basics
- High availability

### What to Study Next

1. Practical DR: Implementation
2. DR testing procedures
3. Runbook documentation

## EXAM TIPS

### Key Exam Facts

- Active-active = lowest RTO
- RPO = backup frequency
- Pilot light = minimal infrastructure
- Test DR regularly

### Exam Questions

- **Question**: "Lowest RTO" = Active-active
- **Question**: "Cross-region backup" = S3 replication
- **Question**: "Automated database DR" = Read replica
- **Question**: "Test strategy" = Regular testing