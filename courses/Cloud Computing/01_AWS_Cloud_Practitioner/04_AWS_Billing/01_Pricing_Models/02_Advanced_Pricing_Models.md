---
Category: AWS Cloud Practitioner
Subcategory: AWS Billing
Concept: Pricing Models - Advanced
Purpose: Advanced AWS pricing strategies including Reserved Instances, Savings Plans, and Spot planning
Difficulty: advanced
Prerequisites: 01_Basic_Pricing_Models.md
RelatedFiles: 01_Basic_Pricing_Models.md, 03_Practical_Pricing_Models.md
UseCase: Enterprise cost optimization with pricing models
CertificationExam: AWS Certified Cloud Practitioner - Domain 4
LastUpdated: 2025
---

## WHY

Advanced pricing optimization requires understanding of Reserved Instances, Savings Plans, and Spot instances for significant cost savings. Enterprise deployments can achieve 40-70% savings with proper planning.

### Why Advanced Pricing Matters

- **Significant Savings**: 40-70% cost reduction
- **Capacity Planning**: Predictable costs
- **Risk Management**: Balance saving vs. flexibility
- **Budget Accuracy**: Reserved capacity

### Advanced Use Cases

- **Reserved Capacity**: Production workloads
- **Savings Plans**: Flexible compute
- **Spot Integration**: Batch processing
- **Multi-tier**: Different pricing for dev/prod

## WHAT

### Pricing Model Comparison

| Model | Savings | Flexibility | Commitment |
|-------|---------|-------------|------------|
| On-Demand | 0% | Full | None |
| Reserved Instance | 40-60% | 1-3 years | Partial |
| Savings Plans | 66% | Variable | $ per hour |
| Spot Instances | 90% | Interruption | None |

### Cross-Platform Comparison

| Pricing | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Reserved | Reserved Instance | Reserved VM | Comitted Use |
| Savings | Savings Plans | Azure savings | Comitted Use |
| Spot | Spot Instances | Spot VMs | Preemptible |
| Flexible | Compute Savings | Hybrid Benefit | Sole Tenant |

### Reserved Instance Types

| Type | Characteristics | Best For |
|------|-----------------|----------|
| Standard RI | Fixed term, fixed AZ | Predictable |
| Convertible RI | Can exchange | Future changes |
| Scheduled RI | Specified schedule | Batch processing |

## HOW

### Example 1: Reserved Instance Purchase

```bash
# Purchase Reserved Instance
aws ec2 purchase-reserved-instances-offering \
    --reserved-instances-offering-id 123456789abcdef \
    --instance-count 5

# Get available offerings
aws ec2 describe-reserved-instances-offerings \
    --filters '[
        {"Name": "instance-type", "Values": ["t3.medium"]},
        {"Name": "tenancy", "Values": ["default"]},
        {"Name": "offering-type", "Values": ["NoUpfront", "PartialUpfront", "AllUpfront"]}
    ]'

# Get recommendations
aws ce get-reserved-instances-recommendations \
    --account-id 123456789

# List reserved instances
aws ec2 describe-reserved-instances \
    --filters '[
        {"Name": "state", "Values": ["active"]}
    ]'
```

### Example 2: Savings Plans Configuration

```bash
# Purchase Compute Savings Plan
aws ce purchase-savings-plans \
    --savings-plan-offering-id '123456789' \
    --commitment '1000.0' \
    --purchase-timeframe '{
        "Start": "2024-01-01T00:00:00Z",
        "End": "2024-12-31T23:59:59Z"
    }'

# Get Savings Plans offerings
aws ce get-savings-plans-offerings \
    --filters '[
        {"Type": "region", "Values": ["us-east-1"]},
        {"Type": "instance-family", "Values": ["t3", "m5"]}
    ]'

# Get utilization
aws ce get-savings-plans-utilization \
    --time-period Start=2024-01-01,End=2024-01-31

# Get coverage
aws ce get-savings-plans-coverage \
    --time-period Start=2024-01-01,End=2024-01-31
```

### Example 3: Spot Fleet Strategy

```bash
# Create Spot Fleet request
aws ec2 request-spot-fleet \
    --spot-fleet-request-config '{
        "IamFleetRole": "arn:aws:iam::123456789:role/spot-fleet-role",
        "targetCapacity": 100,
        "type": "maintain",
        "validFrom": "2024-01-01T00:00:00Z",
        "validUntil": "2024-12-31T23:59:59Z",
        "launchSpecifications": [
            {
                "imageId": "ami-0c55b159cbfafe1f0",
                "instanceType": "t3.medium",
                " subnetId": "subnet-0123456789",
                "weightedCapacity": 10
            }
        ],
        "allocationStrategy": "lowest-price",
        "instancePoolsToUseCount": 3
    }'

# Create Spot block
aws ec2 create-spot-datafeed-preference \
    --bucket my-spot-logs \
    --prefix spot-fleet

# Track Spot pricing history
aws ec2 describe-spot-price-history \
    --start-time 2024-01-01 \
    --end-time 2024-01-02 \
    --instance-types "t3.medium" \
    --product-descriptions "Linux/UNIX"
```

## COMMON ISSUES

### 1. Reserved Instance Not Utilized

**Problem**: RIs sitting idle.

**Solution**:
```bash
# Get utilization report
aws ec2 describe-reserved-instances-utilization \
    --start-time 2024-01-01 \
    --end-time 2024-01-31

# Modify instance tenancy
aws ec2 modify-reserved-instances \
    --reserved-instances-id ri-123456789 \
    --target-configurations '[
        {"AvailabilityZone": "us-east-1a", "Platform": "Linux/UNIX", "InstanceCount": 5}'
```

### 2. Savings Plan Overcommitment

**Problem**: Paying for unused plan.

**Solution**:
- Monitor coverage metrics
- Right-size reservations
- Use partial upfront

### 3. Spot Interruption

**Problem**: Instances terminated unexpectedly.

**Solution**:
```bash
# Handle interruption via instance metadata
curl http://169.254.169.254/latest/meta-data/spot/instance-action

# Create interruption notice handler
aws events put-rule \
    --name spot-interruption \
    --event-pattern '{
        "source": ["aws.ec2"],
        "detail-type": ["EC2 Spot Instance Interruption Warning"]
    }'
```

### 4. Platform Mismatch

**Problem**: RI not applying.

**Solution**:
- Verify platform (Linux/UNIX vs. Windows)
- Check instance type matching
- Review AZ matching

### 5. Convertible Exchange

**Problem**: Cannot convert RI.

**Solution**:
- Check exchange eligibility
- Use same family for easier exchange
- Plan for future needs

## PERFORMANCE

### Savings Benchmarks

| Model | Potential Savings | Average Savings |
|-------|-----------------|----------------|
| Reserved Instance | 40-60% | 45% |
| Compute Savings | 66% | 55% |
| Spot | 90% | 70% |
| All Combined | 70-90% | 60% |

### Optimization Metrics

| Metric | Target | Measurement |
|--------|--------|------------|
| Coverage | > 80% | RI / On-Demand |
| Utilization | > 90% | Used / Purchased |
| Savings | > 50% | vs. On-Demand |

## COMPATIBILITY

### Service Pricing Support

| Service | RI | Savings Plan | Spot |
|---------|-----|-------------|------|
| EC2 | Yes | Yes | Yes |
| RDS | Yes | No | Yes |
| Lambda | No | Yes | No |
| Fargate | No | Yes | Yes |

### Region Support

| Pricing Type | US Regions | EU Regions | All |
|-------------|-----------|-----------|-----|
| Reserved | Yes | Yes | Yes |
| Savings Plans | Yes | Limited | Regional |
| Spot | Yes | Yes | Yes |

## CROSS-REFERENCES

### Related Concepts

- Cost Explorer: Usage analysis
- Budgets: Spending limits
- Cost Categories: Organization

### Prerequisites

- Basic Pricing Models
- Usage understanding

### What to Study Next

1. Practical Pricing: Implementation
2. Cost Management: Budgets
3. Billing: Invoices

## EXAM TIPS

### Key Exam Facts

- RIs: 40-60% savings on 1-3 year term
- Savings Plans: 66% savings, flexible compute
- Spot: 90% savings, can be interrupted
- All Upfront: Maximum savings

### Exam Questions

- **Question**: "Production database" = Reserved Instance
- **Question**: "Flexible workload" = Savings Plan
- **Question**: "Batch processing" = Spot Instances
- **Question**: "Maximum savings" = All Upfront RI