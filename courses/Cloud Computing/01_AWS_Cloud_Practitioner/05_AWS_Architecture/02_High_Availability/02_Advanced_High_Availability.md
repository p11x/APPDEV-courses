---
Category: AWS Cloud Practitioner
Subcategory: AWS Architecture
Concept: High Availability - Advanced
Purpose: Advanced HA patterns including global failover, multi-region strategies, and automated healing
Difficulty: advanced
Prerequisites: 01_Basic_High_Availability.md
RelatedFiles: 01_Basic_High_Availability.md, 03_Practical_High_Availability.md
UseCase: Mission-critical deployments
CertificationExam: AWS Solutions Architect Professional
LastUpdated: 2025
---

## WHY

Advanced HA designs require geographic distribution, automated healing, and systematic failure handling for mission-critical applications requiring 99.99%+ availability.

### Why Advanced HA Matters

- **Global Distribution**: Multi-region active/active
- **Automated Recovery**: Self-healing infrastructure
- **Zero Downtime**: Deployments without impact
- **Failure Isolation**: Contain blast radius

## WHAT

### HA Architecture Patterns

| Pattern | Availability | Recovery | Cost |
|---------|--------------|----------|------|
| Single AZ | 99.5% | Manual | Low |
| Multi-AZ | 99.99% | Automatic | Medium |
| Active-Active | 99.999% | Real-time | High |
| Global Accelerator | 99.99% | Real-time | Medium |

### Cross-Platform Comparison

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Multi-AZ | Yes | Yes | Yes |
| Load Balancer | ALB/ELB | Load Balancer | Cloud Load Balancer |
| Auto Healing | ASG | VMSS | MIG |
| Global Routing | Route 53 | Traffic Manager | Cloud DNS |

### Advanced HA Components

| Component | Purpose | Implementation |
|-----------|---------|----------------|
| Global Accelerator | Edge routing | Anycast |
| Route 53 Health Checks | Failover logic | DNS failover |
| Amazon CloudWatch | Health monitoring | Metrics/alarms |
| AWS Fault Injection | Chaos testing | FI Simulator |

## HOW

### Example 1: Global Accelerator HA

```bash
# Create endpoint group
aws globalaccelerator create-endpoint-group \
    --listener-arn listener-arn \
    --endpoint-group-region us-east-1 \
    --traffic-dial-percentage 50

# Add endpoints
aws globalaccelerator register-endpoints \
    --endpoint-group-arn group-arn \
    --endpoints '[
        {
            "EndpointId": "arn:aws:elasticloadbalancing:us-east-1:123456789:loadbalancer/app/my-alb",
            "Weight": 128
        },
        {
            "EndpointId": "arn:aws:elasticloadbalancing:us-west-2:123456789:loadbalancer/app/my-alb",
            "Weight": 128
        }
    ]'

# Configure health check
aws globalaccelerator update-endpoint-group \
    --endpoint-group-arn group-arn \
    --health-check-interval-seconds 10 \
    --health-check-path /health \
    --threshold-count 3 \
    --traffic-dial-percentage 100
```

### Example 2: Route 53 Failover

```bash
# Create health check
aws route53 create-health-check \
    --caller-reference "health-$(date +%s)" \
    --health-check-config '{
        "Type": "HTTPS",
        "FullyQualifiedDomainName": "app.example.com",
        "Port": 443,
        "ResourcePath": "/health",
        "RequestInterval": 10,
        "FailureThreshold": 3
    }'

# Create failover records
aws route53 change-resource-record-sets \
    --hosted-zone-id Z123456789 \
    --change-batch '{
        "Changes": [
            {
                "Action": "CREATE",
                "ResourceRecordSet": {
                    "Name": "app.example.com",
                    "Type": "A",
                    "SetIdentifier": "primary",
                    "Failover": "PRIMARY",
                    "TTL": 60,
                    "ResourceRecords": [{"Value": "1.2.3.4"}]
                }
            },
            {
                "Action": "CREATE",
                "ResourceRecordSet": {
                    "Name": "app.example.com",
                    "Type": "A",
                    "SetIdentifier": "secondary",
                    "Failover": "SECONDARY",
                    "TTL": 60,
                    "ResourceRecords": [{"Value": "5.6.7.8"}]
                }
            }
        ]
    }'
```

### Example 3: Auto Healing with ASG

```bash
# Create ASG with lifecycle hooks
aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name ha-app-asg \
    --vpc-zone-identifier "subnet-1,subnet-2" \
    --launch-template LaunchTemplateName=app-lt \
    --min-size 2 \
    --max-size 10 \
    --desired-capacity 2 \
    --health-check-type ELB \
    --health-check-grace-period 60

# Add lifecycle hook
aws autoscaling put-lifecycle-hook \
    --lifecycle-hook-name app-launching \
    --auto-scaling-group-name ha-app-asg \
    --lifecycle-transition autoscaling:EC2_INSTANCE_LAUNCHING \
    --notification-target-arn arn:aws:sns:us-east-1:123456789:topic \
    --default-result CONTINUE

# Scheduled actions
aws autoscaling put-scheduled-update-group-action \
    --scheduled-action-name scale-out \
    --auto-scaling-group-name ha-app-asg \
    --recurrence "0 9 * * MON-FRI" \
    --min-size 2 \
    --desired-capacity 4
```

### Example 4: Fault Injection Testing

```bash
# Enable FIS
aws fis create-experiment-template \
    --experiment-template-id template-test \
    --description "HA test experiment" \
    --targets '{
        "Instances": {
            "resourceType": "aws:ec2:instance",
            "resourceArns": ["arn:aws:ec2:us-east-1:123456789:instance/i-12345678"],
            "selectionMode": "ALL"
        }
    }' \
    --actions '{
        "StopInstances": {
            "target": "Instances",
            "action": "aws:ec2:stop-instances"
        }
    }'

# Start experiment
aws fis start-experiment \
    --experiment-template-id template-test

# Get experiment status
aws fis get-experiment \
    --id experiment-id
```

## COMMON ISSUES

### 1. Health Check False Positives

**Problem**: Failing health checks incorrectly.

**Solution**:
- Increase threshold count
- Adjust health check path
- Check security groups

### 2. ASG Not Scaling

**Problem**: Scaling not triggering.

**Solution**:
- Check CloudWatch metrics
- Verify health check type
- Review ASG metrics

### 3. Global Accelerator Issues

**Problem**: Traffic not routing correctly.

**Solution**:
- Verify endpoint status
- Check listener configuration
- Review traffic dial

### 4. Failover Not Working

**Problem**: DNS not switching.

**Solution**:
```bash
# Test health check
aws route53 get-health-check-status \
    --health-check-id health-check-id
```

### 5. Cost Overruns

**Problem**: HA costs too high.

**Solution**:
- Right-size instances
- Use S3/CloudFront caching
- Configure lifecycle hooks

## PERFORMANCE

### Availability vs Cost

| Design | Availability | Relative Cost |
|--------|--------------|----------------|
| Single-AZ | 99.5% | 1x |
| Multi-AZ | 99.99% | 2x |
| Active-Active | 99.999% | 3-4x |

### Recovery Metrics

| Pattern | RTO | RPO |
|---------|-----|-----|
| Single-AZ | Hours | Hours |
| Multi-AZ | Minutes | Minutes |
| Active-Active | Seconds | Seconds |

## COMPATIBILITY

### Service HA Support

| Service | Multi-AZ | Auto Healing | Global |
|---------|----------|--------------|--------|
| EC2 | Yes | ASG | No |
| RDS | Yes | Yes | Cross-region |
| ELB | Yes | Yes | Global Accel |
| Lambda | Yes | Yes | Edge |

### Region Pairs

| Primary | Paired Region |
|---------|---------------|
| us-east-1 | us-east-2 |
| us-west-2 | us-west-1 |
| eu-west-1 | eu-west-2 |
| ap-northeast-1 | ap-northeast-2 |

## CROSS-REFERENCES

### Prerequisites

- High Availability basics
- AWS networking
- Auto Scaling

### What to Study Next

1. Practical HA: Implementation
2. Disaster Recovery
3. Chaos Engineering

## EXAM TIPS

### Key Exam Facts

- Multi-AZ = 99.99% availability
- Active-active = 99.999% availability
- Health checks required for failover
- ASG with ELB health = automated healing