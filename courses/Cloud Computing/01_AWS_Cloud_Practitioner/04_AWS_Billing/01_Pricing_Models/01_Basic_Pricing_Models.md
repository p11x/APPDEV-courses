---
Category: AWS Cloud Practitioner
Subcategory: AWS Billing
Concept: Pricing Models
Purpose: Understanding AWS pricing models, on-demand vs reserved vs savings plans
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Pricing_Models.md, 03_Practical_Pricing_Models.md
UseCase: Optimizing cloud costs through pricing model selection
CertificationExam: AWS Certified Cloud Practitioner - Domain 4: Billing and Pricing
LastUpdated: 2025
---

## WHY

Understanding AWS pricing models is essential for cost optimization. The right pricing model can reduce costs by 50-90%, while the wrong choice can significantly increase spending without benefits.

### Why Pricing Models Matter

- **Cost Impact**: Pricing models differ by 0-90% in potential savings
- **Planning**: Budget forecasting requires understanding models
- **Compliance**: Some models require upfront commitment
- **Flexibility**: Some models offer more flexibility than others

### Cost Reduction Examples

- **On-Demand** to **Reserved**: Up to 72% savings
- **On-Demand** to **Spot**: Up to 90% savings
- **Combined**: Combine savings plans + spot for maximum savings

## WHAT

### Pricing Options by Service

**EC2 Pricing Models**:

| Model | Discount | Commitment | Use Case |
|-------|---------|------------|---------|
| On-Demand | 0% | None | Unknown/variable |
| Savings Plans | 72% | 1-3 years | Predictable |
| Reserved Instances | 72% | 1-3 years | Steady-state |
| Spot Instances | 90% | Can interrupt | Fault-tolerant |

### On-Demand Pricing

- Pay by the hour/second
- No commitment required
- Highest cost option
- Best for: Unknown usage, short-term, testing

### Reserved Instances (RI)

- 1-year or 3-year commitment
- Up to 72% savings
- Types: Standard, Convertible
- Best for: Steady-state production workloads

### Savings Plans

- Similar to RIs but more flexible
- Compute Savings Plans: More flexible
- EC2 Instance Savings Plans: Less flexible
- Up to 72% savings
- Best for: Predictable baseline + burst capability

### Spot Pricing

- Bid for spare capacity
- Up to 90% discount
- Interruptible with 2-minute notice
- Best for: batch jobs, stateless workloads, dev/test

### Other Services Pricing

| Service | Model | Notes |
|---------|-------|-------|
| S3 | Per GB/month | Tiered pricing |
| RDS | Similar to EC2 | Multi-AZ extra |
| Lambda | Request + duration | Free tier available |
| CloudFront | Requests + transfer | Out > In pricing |

### Pricing Comparison Table

| 1 Year RI | 3 Year RI | 1 Year SP | Spot |
|----------|----------|-----------|------|
| Up to 40% | Up to 60% | Up to 72% | Up to 90% |
| 100% upfront | Partial upfront | Partial upfront | No commitment |
| Convertible | Standard | Flexible | Interruptible |

## HOW

### Example 1: Purchasing Reserved Instance

```bash
# Describe available Reserved Instance offerings
aws ec2 describe-reserved-instances-offerings \
    --instance-type t3.micro \
    --availability-zone us-east-1a \
    --offering-type "Reserved"

# Purchase Reserved Instance
aws ec2 purchase-reserved-instances-offering \
    --reserved-instances-offering-id offering-id \
    --instance-count 1

# After purchase, can modify AZ and size within offering
aws ec2 modify-reserved-instances \
    --reserved-instances-ids ri-1234567890abcdef0 \
    --target-configurations '{
        "AvailabilityZone": "us-east-1b",
        "InstanceType": "t3.small",
        "InstanceCount": 1
    }'
```

### Example 2: Savings Plans

```bash
# Create Compute Savings Plan via console or APIs
# Note: No direct CLI for Savings Plans; use console

# To view Savings Plans via CLI
aws savingsplans describe-savings-plans \
    --savings-plan-ids sp-1234567890

# Query savings plan coverage
aws savingsplans describe-savings-plan-offerings \
    --savings-plan-type COMPUTE

# Query coverage rates
aws savingsplans get-savings-rate-coverage \
    --time-period Start=2024-01-01,End=2024-01-31
```

### Example 3: Purchasing Reserved Instance (Terraform)

```hcl
# Terraform for Reserved Instance
resource "aws_ec2_reserved_instance" "example" {
  availability_zone = "us-east-1a"
  instance_type     = "t3.micro"
  offering_type     = "All Upfront"
  instance_count   = 1

  tags = {
    Name = "my-reserved-instance"
  }
}

# EC2 instance using RI
resource "aws_instance" "example" {
  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = "t3.micro"

  # RI is automatically applied to account
}
```

### Example 4: Using Spot Instances

```bash
# Spot instances require request configuration
aws ec2 request-spot-instances \
    --spot-price "0.0100" \
    --instance-count 1 \
    --type "one-time" \
    --launch-specification '{
        "ImageId": "ami-0c55b159cbfafe1f0",
        "InstanceType": "t3.micro"
    }'

# Request persistent spot instance (relaunches if interrupted)
aws ec2 request-spot-instances \
    --spot-price "0.0100" \
    --instance-count 1 \
    --type "persistent" \
    --launch-specification '{
        "ImageId": "ami-0c55b159cbfafe1f0",
        "InstanceType": "t3.micro"
    }'

# Spot Fleet for multiple instances
aws ec2 create-spot-fleet-request \
    --spot-fleet-request-config '{
        "TargetCapacity": 10,
        "SpotPrice": "0.0100",
        "LaunchSpecifications": [{
            "ImageId": "ami-0c55b159cbfafe1f0",
            "InstanceType": "t3.micro"
        }]
    }'
```

## PRICING CALCULATIONS

### Cost Comparison Example

| Configuration | On-Demand | 1-YR RI | 3-YR RI All Upfront |
|--------------|-----------|----------|-----------|-------------------|
| t3.micro hourly | $0.0104 | $0.0062 | $0.0042 |
| Monthly cost | $7.70 | $4.58 | $3.11 |
| Monthly savings | - | $3.12 | $4.59 |
| Annual savings of 3 | - | $112 | $166 |

### Total Cost of Ownership

- On-Demand: ~$7.70/hr = $2,332/yr
- Reserved: ~$1,095/yr (3-year all upfront)
- Savings: ~40-72% less than On-Demand

## COMMON ISSUES

### 1. Purchasing Wrong RI Type

**Problem**: Convertible vs Standard confusion

**Solution**:
- Standard: Up to 72% savings, less flexible
- Convertible: Up to 54% savings, can exchange

### 2. Forgotten RIs

**Problem**: RIs expire or unused

**Solution**:
- Set up Budgets alerts
- Use Cost Explorer to track
- Purchase RI coverage matching needs

### 3. Not Using All Reserved

**Problem**: Purchase too large RIs

**Solution**:
- Consider partial upfront options
- Use Savings Plans for flexibility

## CROSS-REFERENCES

### Related Services

- Cost Explorer: Usage analysis
- Budgets: Alerts and forecasts
- Cur: Detailed billing

### Prerequisites

- Cloud Concepts

### What to Study Next

1. Advanced Pricing: Regional variations
2. Cost Management: Optimization strategies
3. Billing: Cost allocation