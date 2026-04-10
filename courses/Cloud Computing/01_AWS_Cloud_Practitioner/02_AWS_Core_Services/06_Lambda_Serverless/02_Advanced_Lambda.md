---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Lambda Serverless
Purpose: Advanced Lambda configuration, VPC access, and performance optimization
Difficulty: advanced
Prerequisites: 01_Basic_Lambda.md
RelatedFiles: 01_Basic_Lambda.md, 03_Practical_Lambda.md
UseCase: Enterprise serverless applications
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## WHY

Advanced Lambda features enable enterprise serverless applications with VPC access and optimization.

## WHAT

### VPC-Enabled Lambda

Lambda functions with access to VPC resources (RDS, ElastiCache, internal services).

### Provisioned Concurrency

Pre-warmed execution environments for predictable performance.

### Layers

Shared code and dependencies across functions.

## HOW

### VPC Configuration

```bash
# Create Lambda in VPC
aws lambda create-function \
    --function-name vpc-function \
    --runtime python3.9 \
    --role role-arn \
    --handler handler \
    --zip-file fileb://function.zip \
    --vpc-config '{
        "SubnetIds": ["subnet-abc", "subnet-def"],
        "SecurityGroupIds": ["sg-123"]
    }'
```

### Provisioned Concurrency

```bash
# Configure provisioned concurrency
aws lambda put-provisioned-concurrency-config \
    --function-name my-function \
    -- ProvisionedConcurrentExecutions 10
```

## CROSS-REFERENCES

### Related Services

- VPC: Network access
- X-Ray: Tracing]