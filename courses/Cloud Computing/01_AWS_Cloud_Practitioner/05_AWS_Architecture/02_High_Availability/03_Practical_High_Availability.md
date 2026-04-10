---
Category: AWS Cloud Practitioner
Subcategory: AWS Architecture
Concept: High Availability - Practical
Purpose: Implementing production HA systems with multi-AZ, auto healing, and global failover
Difficulty: practical
Prerequisites: 01_Basic_High_Availability.md, 02_Advanced_High_Availability.md
RelatedFiles: 01_Basic_High_Availability.md, 02_Advanced_High_Availability.md
UseCase: Production HA deployment
CertificationExam: AWS Solutions Architect Professional
LastUpdated: 2025
---

## WHY

Practical HA implementation ensures applications remain available despite failures through automated detection and recovery.

## WHAT

### Production HA Architecture

```
Multi-Region HA Architecture
==========================

                 ┌────────────────────────┐
                 │   Global Accelerator  │
                 │   (Edge Locations)    │
                 └──────────┬─────────────┘
                            │
           ┌───────────────┴───────────────┐
           │                             │
    ┌──────▼──────┐              ┌───────▼──────┐
    │ us-east-1  │              │  us-west-2  │
    │ Primary   │◄────────────►│ Secondary  │
    └──────┬──────┘              └──────┬──────┘
           │                             │
    ┌──────▼──────┐              ┌───────▼──────┐
    │ ALB        │              │ ALB         │
    │ (Multi-AZ)│              │ (Multi-AZ)  │
    └──────┬──────┘              └──────┬──────┘
           │                             │
    ┌──────▼──────┐              ┌───────▼──────┐
    │ ASG        │              │ ASG         │
    │ (2+ AZs)   │              │ (2+ AZs)    │
    └───────────┘              └─────────────┘
```

## HOW

### Lab 1: Multi-AZ Application

```bash
# Create VPC with subnets
aws ec2 create-vpc --cidr-block 10.0.0.0/16

aws ec2 create-subnet \
    --vpc-id vpc-id \
    --cidr-block 10.0.1.0/24 \
    --availability-zone us-east-1a

aws ec2 create-subnet \
    --vpc-id vpc-id \
    --cidr-block 10.0.2.0/24 \
    --availability-zone us-east-1b

# Create ASG
aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name prod-asg \
    --vpc-zone-identifier "subnet-id-a,subnet-id-b" \
    --launch-template LaunchTemplateName=app-lt \
    --min-size 2 \
    --max-size 6 \
    --desired-capacity 2 \
    --health-check-type ELB \
    --health-check-grace-period 60
```

### Lab 2: Multi-AZ Database

```bash
# Create RDS Multi-AZ
aws rds create-db-instance \
    --db-instance-identifier production-db \
    --db-instance-class db.r5.large \
    --engine postgres \
    --allocated-storage 500 \
    --master-username dbadmin \
    --master-user-password 'SecurePass123!' \
    --vpc-security-group-ids sg-id \
    --db-subnet-group-name db-subnet-group \
    --multi-az \
    --backup-retention-period 30 \
    --deletion-protection

# Configure read replica
aws rds create-db-instance-read-replica \
    --db-instance-identifier read-replica \
    --source-db-instance-identifier production-db \
    --db-instance-class db.r5.large \
    --availability-zone us-east-1b
```

### Lab 3: Global Failover

```bash
# Create primary record with health check
aws route53 create-health-check \
    --caller-reference "primary-$(date +%s)" \
    --health-check-config '{
        "Type": "HTTPS",
        "FullyQualifiedDomainName": "primary.example.com",
        "Port": 443,
        "ResourcePath": "/health"
    }'

aws route53 change-resource-record-sets \
    --hosted-zone-id zone-id \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "Failover": "PRIMARY",
                "TTL": 60,
                "ResourceRecords": [{"Value": "1.2.3.4"}]
            }
        }]
    }'

# Create secondary record
aws route53 change-resource-record-sets \
    --hosted-zone-id zone-id \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "Failover": "SECONDARY",
                "TTL": 60,
                "ResourceRecords": [{"Value": "5.6.7.8"}]
            }
        }]
    }'
```

### Lab 4: HA Monitoring

```bash
# Create CloudWatch dashboard
aws cloudwatch put-dashboard \
    --dashboard-name "ha-monitor" \
    --dashboard-body '{
        "widgets": [{
            "type": "metric",
            "properties": {
                "title": "Instance Health",
                "metrics": [
                    ["AWS/EC2", "StatusCheckFailed", "InstanceId", "i-12345678"],
                    [".", "InstanceState", ".", "."]
                ]
            }
        }, {
            "type": "alarm",
            "properties": {
                "title": "ALB Targets",
                "alarms": ["alarm-1"]
            }
        }]
    }'

# Create recovery alarm
aws cloudwatch put-metric-alarm \
    --alarm-name "Recovery" \
    --metric-name HealthyHostCount \
    --namespace AWS/ApplicationELB \
    --threshold 1 \
    --comparison-operator LESS_THAN_THRESHOLD \
    --evaluation-periods 2 \
    --alarm-actions arn:aws:sns:region:account:topic
```

## COMMON ISSUES

### 1. AZ Capacity Issues

**Problem**: Cannot provision instances.

**Solution**:
- Use different instance types
- Request limit increase
- Spread across AZs

### 2. Health Check Failures

**Problem**: Instances marked unhealthy.

**Solution**:
- Check health check path
- Verify security groups
- Review target group config

### 3. Failover Delays

**Problem**: Slow failover.

**Solution**:
- Reduce TTL
- Improve health check interval
- Check Route 53 latency

### 4. Cost Overruns

**Problem**: Too many instances.

**Solution**:
- Configure scaling policies
- Set instance limits
- Use spot instances

### 5. Database Failover

**Problem**: DB not failing over.

**Solution**:
- Check primary status
- Verify multi-az enabled
- Review maintenance window

## PERFORMANCE

### HA Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| Availability | > 99.99% | Uptime |
| RTO | < 5 minutes | Recovery test |
| Recovery | < 1 minute | Health check |
| Failover | < 60 seconds | DNS TTL |

### Performance Benchmarks

| Operation | Average Time |
|-----------|--------------|
| AZ failover | 60-120 seconds |
| Region failover | 30-60 seconds |
| DNS failover | TTL dependent |
| Auto healing | 2-5 minutes |

## COMPATIBILITY

### Multi-Region Services

| Service | Cross-Region | Failover |
|--------|--------------|----------|
| EC2 | No | Manual |
| RDS | Read replica | Manual |
| S3 | Yes | Automatic |
| Route 53 | Yes | DNS |

### Integration Matrix

| Service | ASG | ELB | Route 53 |
|---------|-----|-----|----------|
| EC2 | Yes | Yes | Yes |
| RDS | No | Yes | Yes |
| Lambda | N/A | Yes | Yes |
| ECS | Yes | Yes | Yes |

## CROSS-REFERENCES

### Prerequisites

- High availability basics
- AWS CLI experience

### Next Steps

1. Disaster recovery testing
2. Chaos engineering
3. RTO/RPO validation

## EXAM TIPS

### Production Patterns

- Multi-AZ for 99.99%
- Auto healing with ASG
- Health check-driven failover
- Regular DR testing