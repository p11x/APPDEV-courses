---
Category: AWS Cloud Practitioner
Subcategory: AWS Billing
Concept: Pricing Models - Practical
Purpose: Apply AWS pricing knowledge to real-world scenarios
Difficulty: practical
Prerequisites: 01_Basic_Pricing_Models.md, 02_Advanced_Pricing_Models.md
RelatedFiles: 01_Basic_Pricing_Models.md, 02_Advanced_Pricing_Models.md
UseCase: Cost optimization for production workloads
CertificationExam: AWS Certified Cloud Practitioner
LastUpdated: 2025
---

## WHY

Practical pricing knowledge transforms theoretical savings into actual cost reductions. Organizations that apply AWS pricing models correctly can achieve 40-72% savings compared to on-demand pricing, translating to thousands of dollars in monthly savings for production workloads.

### Why Practical Application Matters

- **Real Savings**: Theory vs. actual implementation gaps cost money
- **Workload Matching**: Correct pricing model selection maximizes savings
- **Operational Efficiency**: Automated purchasing and management
- **Budget Predictability**: Reserved capacity planning enables forecasting

### Business Impact

| Scenario | Potential Savings | Annual Impact |
|----------|-------------------|---------------|
| Production API (steady-state) | 60-72% | $15,000-21,000 |
| Development Environment | 72-90% | $8,000-12,000 |
| Batch Processing Jobs | 85-90% | $25,000-35,000 |
| Mixed Workload (prod + dev) | 50-70% | $20,000-30,000 |

## WHAT

### Practical Pricing Architecture

```yaml
# Multi-tier Pricing Architecture
Architecture:
  Production_Tier:
    Pricing: Reserved Instance / Savings Plan
    Coverage: 80-100% of baseline
    Use_Case: Critical workloads

  Burst_Tier:
    Pricing: Savings Plan + On-Demand
    Coverage: 20% baseline + burst
    Use_Case: Variable workloads

  Batch_Tier:
    Pricing: Spot Instances
    Coverage: Flexible capacity
    Use_Case: Batch processing

  Dev_Test_Tier:
    Pricing: Spot + On-Demand
    Coverage: Intermittent
    Use_Case: Development environments
```

### Workload-to-Pricing Model Mapping

| Workload Type | Recommended Model | Justification |
|---------------|-------------------|----------------|
| Always-on API | 3-YR All-Upfront RI | Maximum savings, predictable |
| Daytime web app | Compute Savings Plan | Flexible baseline + burst |
| Nightly batch | Spot Fleet | 90% savings, fault-tolerant |
| Dev environment | Spot + On-Demand mix | Cost-effective, available |
| DR workload | 1-YR RI | Moderate savings, recoverable |
| Stateless microservice | EC2 Savings Plan | Instance-specific savings |

## HOW

### Example 1: Production Workload Reserved Instance Purchase

```bash
# Step 1: Analyze current usage patterns
aws ce get-dimension-values \
    --dimension INSTANCE_TYPE \
    --time-period Start=2024-01-01,End=2024-03-31

# Step 2: Get pricing recommendations
aws ce get-reserved-instances-recommendations \
    --account-id 123456789012 \
    --offering-type "MediumUtilization"

# Step 3: Describe available offerings
aws ec2 describe-reserved-instances-offerings \
    --instance-type m5.large \
    --product-description "Linux/UNIX" \
    --offering-type "All Upfront" \
    --filters '[
        {"Name": "duration", "Values": ["31536000"]},
        {"Name": "tenancy", "Values": ["default"]}
    ]'

# Step 4: Purchase Reserved Instance
aws ec2 purchase-reserved-instances-offering \
    --reserved-instances-offering-id 123456789abcdef0123456789 \
    --instance-count 4

# Step 5: Verify purchase and get details
aws ec2 describe-reserved-instances \
    --filters '[
        {"Name": "state", "Values": ["active"]},
        {"Name": "instance-type", "Values": ["m5.large"]}
    ]'

# Step 6: Set up utilization monitoring
aws ce get-reserved-instances-utilization \
    --time-period Start=2024-01-01,End=2024-01-31
```

**Architecture Pattern:**

```
┌─────────────────────────────────────────────────┐
│         Production Workload Architecture       │
├─────────────────────────────────────────────────┤
│                                                 │
│  ┌──────────────┐     ┌──────────────────┐     │
│  │ Load Balancer│────▶│  Auto Scaling    │     │
│  └──────────────┘     │     Group        │     │
│                       └──────────────────┘     │
│                              │                 │
│              ┌───────────────┼───────────────┐ │
│              ▼               ▼               ▼ │
│       ┌───────────┐   ┌───────────┐   ┌───────────┐
│       │ Reserved  │   │ Reserved  │   │ Reserved  │
│       │ Instance  │   │ Instance  │   │ Instance  │
│       │  (m5.lg)  │   │  (m5.lg)  │   │  (m5.lg)  │
│       └───────────┘   └───────────┘   └───────────┘
│                                                 │
│  Savings: 60-72% vs. On-Demand                   │
└─────────────────────────────────────────────────┘
```

### Example 2: Variable Workload with Compute Savings Plan

```bash
# Step 1: Analyze baseline usage
aws ce get-usage-forecast \
    --granularity MONTHLY \
    --metric UNBLENDED_COST \
    --service AmazonEC2 \
    --time-period Start=2024-01-01,End=2024-03-31

# Step 2: Query available Savings Plans
aws ce get-savings-plans-offerings \
    --filters '[
        {"Type": "region", "Values": ["us-east-1"]},
        {"Type": "instance-family", "Values": ["m5", "c5"]},
        {"Type": "offering-type", "Values": ["Compute"]}
    ]' \
    --max-results 10

# Step 3: Purchase Compute Savings Plan (via console - no CLI)
# Console路径: AWS Cost Console → Savings Plans → Purchase

# Step 4: Monitor Savings Plan coverage
aws ce get-savings-plans-coverage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --granularity MONTHLY

# Step 5: Get coverage rates by instance family
aws ce get-savings-rate-coverage \
    --time-period Start=2024-01-01,End=2024-01-31

# Step 6: Monitor utilization
aws ce get-savings-plans-utilization \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --granularity DAILY
```

**Architecture Pattern:**

```
┌─────────────────────────────────────────────────┐
│      Variable Workload Architecture            │
├─────────────────────────────────────────────────┤
│                                                 │
│  Baseline (80% usage) ──────────────────────────▶│
│         │                                       │
│         │ Compute Savings Plan Applied          │
│         ▼                                       │
│  ┌─────────────────────────────────────┐        │
│  │     Auto Scaling Group (min: 2)     │        │
│  └─────────────────────────────────────┘        │
│              │                                  │
│              ▼ (burst up to 10 instances)       │
│  ┌─────────────────────────────────────┐        │
│  │     Auto Scaling Group (max: 10)    │        │
│  └─────────────────────────────────────┘        │
│              │                                  │
│              ▼ On-Demand (excess capacity)      │
│                                                 │
│  Savings: 66% on baseline, on-demand for burst   │
└─────────────────────────────────────────────────┘
```

### Example 3: Batch Processing with Spot Fleet

```bash
# Step 1: Analyze spot pricing history
aws ec2 describe-spot-price-history \
    --instance-types "m5.large" "c5.large" \
    --product-descriptions "Linux/UNIX" \
    --start-time 2024-01-01T00:00:00Z \
    --end-time 2024-01-07T00:00:00Z \
    --availability-zone us-east-1

# Step 2: Create Spot Fleet with diversification
aws ec2 request-spot-fleet \
    --spot-fleet-request-config '{
        "IamFleetRole": "arn:aws:iam::123456789:role/SpotFleetRole",
        "TargetCapacity": 20,
        "AllocationStrategy": "lowest-price",
        "InstancePoolsToUseCount": 3,
        "Type": "maintain",
        "ValidFrom": "2024-01-01T00:00:00Z",
        "ValidUntil": "2024-12-31T23:59:59Z",
        "LaunchSpecifications": [
            {
                "ImageId": "ami-0c55b159cbfafe1f0",
                "InstanceType": "m5.large",
                "SubnetId": "subnet-0123456789abcdef0",
                "WeightedCapacity": 1,
                "SpotPlacement": {
                    "AvailabilityZone": "us-east-1a"
                }
            },
            {
                "ImageId": "ami-0c55b159cbfafe1f0",
                "InstanceType": "c5.large",
                "SubnetId": "subnet-0123456789abcdef1",
                "WeightedCapacity": 1,
                "SpotPlacement": {
                    "AvailabilityZone": "us-east-1b"
                }
            },
            {
                "ImageId": "ami-0c55b159cbfafe1f0",
                "InstanceType": "m5.xlarge",
                "SubnetId": "subnet-0123456789abcdef2",
                "WeightedCapacity": 2,
                "SpotPlacement": {
                    "AvailabilityZone": "us-east-1c"
                }
            }
        ]
    }'

# Step 3: Monitor Spot Fleet status
aws ec2 describe-spot-fleet-requests \
    --spot-fleet-request-ids sfr-123456789abcdef0

# Step 4: Configure interruption handling
aws events put-rule \
    --name spot-interruption-handler \
    --event-pattern '{
        "source": ["aws.ec2"],
        "detail-type": ["EC2 Spot Instance Interruption Warning"],
        "detail": {
            "instance-id": ["*"]
        }
    }'

# Step 5: Create termination handler Lambda function
# Note: Handle 2-minute warning for graceful shutdown
aws lambda create-function \
    --function-name spot-termination-handler \
    --runtime python3.11 \
    --role arn:aws:iam::123456789:function/lambda-role \
    --handler handler.lambda_handler \
    --zip-file fileb://handler.zip

# Step 6: Track cost savings
aws ce get-cost-and-usage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --granularity MONTHLY \
    --metrics UNBLENDED_COST \
    --group-by '[
        {"Type": "DIMENSION", "Key": "INSTANCE_TYPE"}
    ]'
```

**Architecture Pattern:**

```
┌─────────────────────────────────────────────────┐
│         Batch Processing Architecture           │
├─────────────────────────────────────────────────┤
│                                                 │
│  ┌────────────────────────────────────────────┐ │
│  │         SQS Queue (Job Queue)               │ │
│  └────────────────────────────────────────────┘ │
│                       │                          │
│                       ▼                          │
│  ┌────────────────────────────────────────────┐ │
│  │    Spot Fleet Auto Scaling Group           │ │
│  │  - Allocation: lowest-price                │ │
│  │  - Diversification: 3 instance pools      │ │
│  │  - Type: maintain (replaces interrupted)   │ │
│  └────────────────────────────────────────────┘ │
│                       │                          │
│    ┌─────────┬─────────┬────���────┐               │
│    ▼         ▼         ▼                         │
│ ┌──────┐ ┌──────┐ ┌──────┐     Instances       │
│ │ Spot │ │ Spot │ │ Spot │     launched       │
│ │ m5.lg│ │ c5.lg│ │m5.xlg│     across       │
│ └──────┘ └──────┘ └──────┘     AZs            │
│                                                 │
│  Savings: 70-90% vs. On-Demand                  │
│  Cost per batch job: ~$0.003 vs. $0.030         │
└─────────────────────────────────────────────────┘
```

## COMMON ISSUES

### 1. Reserved Instance Coverage Gaps

**Problem**: Purchasing wrong instance size or AZ results in unused RIs

```bash
# Check utilization by instance
aws ce get-reserved-instances-utilization \
    --time-period Start=2024-01-01,End=2024-01-31

# Modify RI configuration
aws ec2 modify-reserved-instances \
    --reserved-instances-ids ri-123456789abcdef0 \
    --target-configurations '[
        {
            "AvailabilityZone": "us-east-1a",
            "InstanceType": "m5.large",
            "InstanceCount": 2
        }
    ]'

# Solution: Match purchase to actual usage patterns
# Use m5.xlarge instead of m5.large if needed
# Purchase across multiple AZs if needed
```

### 2. Spot Fleet Interruption

**Problem**: Batch jobs fail when Spot instances interrupted

```bash
# Monitor for interruption warnings
aws ec2 describe-spot-instance-requests \
    --filters '[
        {"Name": "state", "Values": ["active"]}
    ]'

# Create SNS topic for notifications
aws sns create-topic --name spot-interruptions

# Add CloudWatch event rule
aws events put-rule \
    --name spot-interruption-warning \
    --event-pattern '{
        "source": ["aws.ec2"],
        "detail-type": ["EC2 Spot Instance Interruption Warning"]
    }'

# Solution: Design for fault tolerance
# Use spot-fleet with type=maintain
# Implement job checkpoints
# Use multiple instance pools
```

### 3. Savings Plan Not Applying

**Problem**: Savings Plan coverage is 0% or very low

```bash
# Check Savings Plan coverage
aws ce get-savings-plans-coverage \
    --time-period Start=2024-01-01,End=2024-01-31

# Verify Savings Plan type matches usage
aws ce get-savings-plan-offerings \
    --filters '[
        {"Type": "region", "Values": ["us-east-1"]},
        {"Type": "instance-family", "Values": ["m5"]}
    ]'

# Check coverage by instance family
aws ce get-savings-rate-coverage \
    --time-period Start=2024-01-01,End=2024-01-31

# Solution: Use correct Savings Plan type
# Compute Savings Plans: more flexible
# EC2 Instance Savings Plans: specific to instance type
```

### 4. Budget Overruns

**Problem**: Monthly costs exceed budget despite savings plans

```bash
# Check current spend vs. budget
aws budgets describe-budget \
    --account-id 123456789012 \
    --budget-name "monthly-production"

# Get cost forecast
aws ce get-cost-forecast \
    --metric UNBLENDED_COST \
    --granularity MONTHLY \
    --time-period Start=2024-01-01,End=2024-01-31

# Create budget alert
aws budgets create-notification \
    --account-id 123456789012 \
    --budget-name "monthly-production" \
    --notification '{
        "Threshold": 80,
        "ComparisonOperator": "GREATER_THAN",
        "NotificationType": "ACTUAL"
    }' \
    --subscribers '[{
        "Type": "SNS",
        "Address": "arn:aws:sns:us-east-1:123456789:cost-alerts"
    }]'

# Solution: Implement budget controls
# Set alerts at 50%, 75%, 90% thresholds
# Use Savings Plans with commitment limits
# Implement auto-scaling limits
```

## PERFORMANCE

### Cost Optimization Benchmarks

| Metric | On-Demand | Reserved | Savings Plan | Spot |
|--------|-----------|----------|-------------|------|
| Hourly Cost | $0.20 | $0.08 | $0.07 | $0.02 |
| Monthly Cost | $144 | $58 | $50 | $14 |
| Savings % | 0% | 60% | 65% | 90% |
| Availability | 100% | 100% | 100% | 95% |

### Optimization Targets

| Workload Type | Target Savings | Target Coverage | Target Utilization |
|--------------|----------------|-----------------|---------------------|
| Production | 60-72% | >90% | >85% |
| Development | 72-90% | >50% | >70% |
| Batch | 85-90% | N/A | 100% |
| Mixed | 50-70% | >75% | >80% |

### Performance Monitoring Commands

```bash
# Monitor Reserved Instance coverage
aws ce get-reserved-instances-coverage \
    --time-period Start=2024-01-01,End=2024-01-31

# Monitor Savings Plan coverage
aws ce get-savings-plans-coverage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --granularity MONTHLY

# Get cost by pricing model
aws ce get-cost-and-usage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --granularity MONTHLY \
    --metrics UNBLENDED_COST \
    --group-by '[{"Type": "DIMENSION", "Key": "PRICING_MODEL"}]'

# Generate savings report
aws ce get-savings-plans-savings-totals \
    --time-period Start=2024-01-01,End=2024-01-31
```

## COMPATIBILITY

### Pricing Model Compatibility by Service

| Service | Reserved Instance | Savings Plan | Spot | On-Demand |
|---------|-------------------|--------------|------|----------|
| EC2 | Yes | Yes | Yes | Yes |
| RDS | Yes | No | Yes* | Yes |
| Lambda | No | Yes | No | Yes |
| Fargate | No | Yes | Yes | Yes |
| EMR | Yes | Yes | Yes | Yes |
| EKS | No | Yes | Yes | Yes |
| SageMaker | No | Yes | No | Yes |

* RDS Spot only for Read Replicas

### Cross-Account Compatibility

```bash
# Share Reserved Instances across accounts
aws ec2 describe-reserved-instances \
    --filters '[
        {"Name": "owner-id", "Values": ["123456789012"]}
    ]'

# Enable Cost Explorer across accounts
aws ce get-cost-and-usage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --granularity MONTHLY \
    --account-ids ["123456789012", "987654321098"]

# Use Consolidated Billing for volume discounts
aws organizations list-accounts
```

### Region and Availability Zone Support

| Pricing Type | All Regions | Specific Regions | AZ Required |
|--------------|-------------|------------------|--------------|
| Reserved Instance | Yes | Yes | Optional |
| Compute Savings Plan | Yes | Limited | No |
| EC2 Savings Plan | No | Yes | No |
| Spot | Yes | Yes | Optional |

## CROSS-REFERENCES

### AWS Services Integration

| Service | Pricing Integration | CLI Support |
|---------|--------------------|-------------|
| Cost Explorer | Savings analysis | Yes |
| Budgets | Spending alerts | Yes |
| CloudWatch | Usage monitoring | Yes |
| SNS | Cost notifications | Yes |
| Lambda | Automated responses | Yes |
| Systems Manager | Instance management | Yes |

### Prerequisites Completion

- [x] 01_Basic_Pricing_Models.md: Understanding pricing types
- [x] 02_Advanced_Pricing_Models.md: Advanced optimization
- [ ] Cost Explorer Basics: Usage analysis
- [ ] Budget Configuration: Spending limits

### Related Documentation

- [AWS Pricing Calculator](https://calculator.aws)
- [Reserved Instance Best Practices](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/ri-concepts-basics.html)
- [Savings Plans User Guide](https://docs.aws.amazon.com/savingsplans/latest/userguide/)
- [Spot Instance Best Practices](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/spot-best-practices.html)

### Next Learning Steps

1. Cost Management: Budgets and alerts configuration
2. CloudWatch: Usage monitoring and alerting
3. Cost Explorer: Advanced analysis and reporting
4. Organizations: Multi-account billing strategies

## EXAM TIPS

### Key Exam Facts

| Pricing Model | Savings | Best For | On Exam Look For |
|---------------|----------|----------|------------------|
| Reserved Instance | 40-72% | Steady-state production | "24/7 production app" |
| Savings Plans | 66% | Flexible compute | "variable workload" |
| Spot | 70-90% | Fault-tolerant batch | "batch processing" |
| On-Demand | 0% | Unknown/variable | "testing" or "unknown" |

### Exam Question Patterns

**Pattern 1**: "Always-on production database"
- **Answer**: Reserved Instance (1 or 3 year)
- **Rationale**: Predictable workload, maximum savings

**Pattern 2**: "Flexible web application with peak hours"
- **Answer**: Compute Savings Plan
- **Rationale**: Baseline + burst capability

**Pattern 3**: "Nightly batch processing"
- **Answer**: Spot Instances
- **Rationale**: Fault-tolerant, maximum savings

**Pattern 4**: "Development and testing"
- **Answer**: Spot or On-Demand mix
- **Rationale**: Flexibility, lower priority

**Pattern 5**: "Maximum savings with some risk acceptable"
- **Answer**: 3-year All Upfront Reserved Instance
- **Rationale**: Highest discount tier

### Exam Commands to Know

```bash
# Get RI recommendations
aws ce get-reserved-instances-recommendations

# Get Savings Plan coverage
aws ce get-savings-plans-coverage

# Describe RI offerings
aws ec2 describe-reserved-instances-offerings

# Get cost by pricing model
aws ce get-cost-and-usage --group-by PRICING_MODEL
```

### Common Exam Scenarios

1. **Workload**: "E-commerce site with steady traffic"
   - **Model**: Reserved Instance
   - **Savings**: 60-72%

2. **Workload**: "Machine learning training jobs"
   - **Model**: Spot Fleet
   - **Savings**: 70-90%

3. **Workload**: "API with daytime peaks"
   - **Model**: Compute Savings Plan
   - **Savings**: 66% on baseline

4. **Workload**: "Dev/test environment"
   - **Model**: Spot + On-Demand
   - **Savings**: 50-80%

### Final Exam Review Points

- On-Demand = highest cost, most flexibility
- Reserved = commitment换取 savings
- Savings Plans = flexibility换取 savings
- Spot = 最高savings, interruption risk
- 3-year All Upfront = maximum savings
- Convertible RIs = exchange capability
- Savings Plans apply to future usage, not past

(End of file - total lines: 410)