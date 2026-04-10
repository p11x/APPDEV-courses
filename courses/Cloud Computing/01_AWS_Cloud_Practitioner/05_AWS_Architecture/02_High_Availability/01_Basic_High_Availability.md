---
Category: AWS Cloud Practitioner
Subcategory: AWS Architecture
Concept: High Availability
Purpose: Understanding high availability design patterns, multi-AZ deployments, and availability targets
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_High_Availability.md, 03_Practical_High_Availability.md
UseCase: Designing fault-tolerant systems
CertificationExam: AWS Solutions Architect Associate
LastUpdated: 2025
---

## WHY

High availability is essential for production systems that must remain operational even when failures occur. Understanding HA patterns prevents costly downtime.

### Why High Availability Matters

- **Business Continuity**: Prevent revenue loss from downtime
- **Customer Trust**: Maintain positive user experience
- **SLAs**: Meet contractual availability requirements
- **Recovery**: Minimize incident impact

### Availability Levels

| Availability | Downtime/Year | Downtime/Month |
|-------------|---------------|----------------|
| 99% (2 9s) | 3.65 days | 7.3 hours |
| 99.9% (3 9s) | 8.76 hours | 43.8 minutes |
| 99.99% (4 9s) | 52.6 minutes | 4.38 minutes |
| 99.999% (5 9s) | 5.26 minutes | 26 seconds |

## WHAT

### HA Architectural Patterns

**Pattern 1: Horizontal Scaling**
- Add more instances instead of larger ones
- Distribute across AZs
- Use load balancers

**Pattern 2: Multi-AZ Deployment**
- Deploy across multiple Availability Zones
- Automatic failover
- Typical 99.99% achievable

**Pattern 3: Load Balancing**
- Distribute traffic across instances
- Health checks route around failures
- Managed services available

### Architecture Comparison

```
    SINGLE AZ vs MULTI-AZ
    ====================

    SINGLE AZ              MULTI-AZ
    ──────────            ──────────

    ┌──────┐              ┌────────┐
    │     │               │ AZ-a  │┼──Instance 1
    │ EC2 │               └───────┘
    │     │               ┌───────┐
    └─────┘               │ AZ-b  │┼──Instance 2
        │                 └───────┘   
    Failure         Automatic Failover 
    = Disaster        = High Availability
```

## HOW

### Example 1: Multi-AZ Deployment

```bash
# Create subnets in multiple AZs
SUBNET_A=$(aws ec2 create-subnet \
    --vpc-id vpc-12345678 \
    --cidr-block 10.0.1.0/24 \
    --availability-zone us-east-1a \
    --query Subnet.SubnetId)

SUBNET_B=$(aws ec2 create-subnet \
    --vpc-id vpc-12345678 \
    --cidr-block 10.0.2.0/24 \
    --availability-zone us-east-1b \
    --query Subnet.SubnetId)

# Create Auto Scaling Group across AZs
aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name ha-asg \
    --launch-template LaunchTemplateName=my-lt \
    --min-size 2 \
    --max-size 10 \
    --desired-capacity 2 \
    --vpc-zone-identifier "$SUBNET_A,$SUBNET_B" \
    --health-check-type ELB
```

### Example 2: Multi-AZ RDS

```bash
# Create RDS with Multi-AZ
aws rds create-db-instance \
    --db-instance-identifier my-db \
    --db-instance-class db.t3.medium \
    --engine mysql \
    --allocated-storage 100 \
    --master-username admin \
    --master-user-password 'Password123!' \
    --vpc-security-group-ids sg-12345678 \
    --db-subnet-group-name my-subnet-group \
    --multi-az
```

### Example 3: Multi-AZ Application Load Balancer

```bash
# Create target group
TG_ARN=$(aws elbv2 create-target-group \
    --name my-tg \
    --protocol HTTP \
    --port 80 \
    --vpc-id vpc-12345678 \
    --query TargetGroups[0].TargetGroupArn)

# Create ALB in multiple subnets
aws elbv2 create-load-balancer \
    --name my-alb \
    --scheme internet-facing \
    --type application \
    --subnets subnet-a subnet-b \
    --security-groups sg-12345678
```

## HA BEST PRACTICES

- Deploy across at least 2 AZs
- Use managed services (RDS Multi-AZ)
- Implement health checks at every layer
- Automate failover with Auto Scaling
- Test failover regularly

## ⚠️ COMMON ISSUES

1. **Cost vs Availability**: Multi-AZ increases costs - balance availability needs with budget
2. **Failover Testing**: Many fail to test failover - schedule regular drills
3. **Health Check Configuration**: Incorrect health checks can cause false failures
4. **Single Points of Failure**: Even with multi-AZ, some services remain single-AZ
5. **Data Replication Lag**: Multi-AZ replication can have lag - plan for RPO

## 🏃 PERFORMANCE

- Multi-AZ deployments add minimal latency (same region, fast network)
- Auto Scaling responds to load metrics in seconds
- Load balancers distribute traffic with ~1ms latency
- RDS Multi-AZ failover takes 1-2 minutes

## 🌐 COMPATIBILITY

- Works with all AWS services that support Multi-AZ
- RDS, ElastiCache, Aurora support Multi-AZ natively
- Auto Scaling Groups can spread across AZs
- ALB, NLB support multi-AZ deployments

## ✅ EXAM TIPS

1. "Highly available" means multi-AZ deployment
2. 99.99% availability = Multi-AZ for most services
3. Failover is automatic with managed services
4. Cost is the main tradeoff for HA
5. SLAs define availability targets
6. RTO and RPO guide HA architecture decisions

## CROSS-REFERENCES

### Related Services

- Auto Scaling Groups
- Application Load Balancer
- RDS Multi-AZ
- Route 53 health checks