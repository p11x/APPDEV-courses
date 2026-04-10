---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: EC2 Compute
Purpose: Practical EC2 deployment and management labs
Difficulty: intermediate
Prerequisites: 01_Basic_EC2.md, 02_Advanced_EC2.md
RelatedFiles: 01_Basic_EC2.md, 02_Advanced_EC2.md
UseCase: Production EC2 deployment and operations
CertificationExam: AWS SysOps Administrator
LastUpdated: 2025
---

## WHY

Hands-on EC2 labs solidify understanding through practical implementation.

## WHAT

### Lab: Production Web Server

Deploy scalable web infrastructure using Auto Scaling and ALB.

## HOW

### Module 1: Create VPC

```bash
# Create VPC with subnets
aws ec2 create-vpc --cidr-block 10.0.0.0/16

# Create subnets
aws ec2 create-subnet --vpc-id vpc-123 \
    --cidr-block 10.0.1.0/24 --availability-zone us-east-1a
```

### Module 2: Launch Template

```bash
# Create launch template
aws ec2 create-launch-template \
    --launch-template-name web-template \
    --launch-template-data '{
        "ImageId": "ami-0c55b159cbfafe1f0",
        "InstanceType": "t3.micro"
    }'
```

### Module 3: Auto Scaling Group

```bash
# Create ASG
aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name web-asg \
    --launch-template LaunchTemplateName=web-template \
    --min-size 2 \
    --max-size 10 \
    --desired-capacity 2
```

## VERIFICATION

```bash
# Check instances
aws ec2 describe-instances --filters "Name=instance-state-name,Values=running"

# Check ASG
aws autoscaling describe-auto-scaling-groups \
    --auto-scaling-group-names web-asg
```

## CLEANUP

```bash
# Delete ASG
aws autoscaling delete-auto-scaling-group --auto-scaling-group-name web-asg --force-delete
```

## CROSS-REFERENCES

### Prerequisites

- VPC networking concepts