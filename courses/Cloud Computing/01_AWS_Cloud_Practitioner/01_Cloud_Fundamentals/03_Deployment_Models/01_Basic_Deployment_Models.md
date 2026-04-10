---
Category: AWS Cloud Practitioner
Subcategory: Cloud Fundamentals
Concept: Deployment Models
Purpose: Understand cloud deployment approaches including cloud-native, lift-and-shift, and rehosting strategies
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Models.md
RelatedFiles: 02_Advanced_Deployment_Models.md, 03_Practical_Deployment_Models.md
UseCase: Planning application migration to cloud
CertificationExam: AWS Certified Cloud Practitioner - Migration Concepts
LastUpdated: 2025
---

## WHY

Choosing the right migration strategy determines project success, cost, timeline, and post-migration performance. An incorrect strategy can double migration costs or leave applications unable to leverage cloud benefits.

### Strategy Impact

- **Cost Impact**: Re-architecting costs 3-5x more than lift-and-shift but delivers 50%+ ongoing savings for appropriate workloads
- **Timeline**: Cloud-native migrations take longer but deliver faster time-to-value
- **Risk**: Re-architecting introduces more risk than rehosting
- **Operations**: Each strategy requires different operational support

### Industry Migration Patterns

- 80% of enterprises use lift-and-shift as initial migration, then optimize (RightScale Survey)
- Most organizations run hybrid for 3-5 years during migration
- 70% of legacy applications never get fully re-architected

### When to Use Each Strategy

- **Rehosting**: Rapid migration needed, no code changes available
- **Replatforming**: Quick cloud benefits, minor optimizations
- **Refactoring**: Leverage cloud-native features, longer-term investment
- **Repurchasing**: SaaS replaces custom development
- **Retiring**: Application no longer provides value

## WHAT

### Cloud Migration Strategies Defined

**Rehosting ("Lift and Shift")**: Moving applications to cloud without modifications.

- Process: Image existing servers, provision cloud resources, migrate
- Benefits: Fastest migration, lowest risk
- Drawbacks: Doesn't leverage cloud features
- Use when: Speed is priority, no development capacity

**Replatforming ("Lift, Tinker, and Shift")**: Minor optimizations during migration.

- Process: Same as rehosting but tune for cloud environment
- Benefits: Some cloud benefits, moderate effort
- Drawbacks: Still running as if on-premises
- Use when: Small improvements acceptable

**Refactoring ("Re-architecting")**: Redesigning for cloud-native.

- Process: Rewrite application using cloud services
- Benefits: Full cloud benefits, modern architecture
- Drawbacks: Highest cost and risk
- Use when: Long-term cloud investment planned

**Repurchasing ("SaaS")**: Replacing with commercial SaaS.

- Process: Subscribe to SaaS, migrate data
- Benefits: No infrastructure management
- Drawbacks: Feature limitations, subscription cost
- Use when: Commercial solution available

**Retiring**: Decommissioning applications.

- Process: Identify unused apps, validate, shut down
- Benefits: Cost savings, reduced attack surface
- Drawbacks: Potential business impact
- Use when: Application provides no value

### Architecture Diagram: Migration Strategies

```
                    CLOUD MIGRATION STRATEGIES
                    =======================

    REHOSTING              REPLATFORMING          REFACTORING
    ────────              ────────────          ───────────
    ┌─────────┐            ┌─────────┐            ┌─────────┐
    ��� On-Prem │ ───────►   │ On-Prem │ ───────►   │ On-Prem │
    │ App     │ Image     │ App     │ + Tuning │ App     │
    └─────────┘            └─────────┘            └─────────┘
           │                      │                      │
           ▼                      ▼                      ▼
    ┌─────────┐            ┌─────────┐            ┌─────────┐
    │   EC2   │            │   EC2   │            │ Lambda  │
    │ Original│            │With OPT │            │ +DynaDB │
    │ Config  │            │        │            │ +S3    │
    └─────────┘            └─────────┘            └─────────┘

    Timeline: Fast         Timeline: Med        Timeline: Slow
    Cost: Low            Cost: Med            Cost: High
    Cloud: 0%            Cloud: 30%           Cloud: 100%
```

### Comparison Table

| Strategy | Timeline | Cost | Cloud Benefits | Risk |
|----------|----------|------|---------------|------|
| Rehosting | 1-4 weeks | Low | 10-20% | Low |
| Replatforming | 4-8 weeks | Medium | 30-40% | Medium |
| Refactoring | 2-6 months | High | 80-100% | High |
| Repurchasing | 1-4 weeks | Varies | 100% | Low |
| Retiring | 1-2 weeks | Low | N/A | Low |

## HOW

### Example 1: Rehosting Migration

```bash
# Step 1: Assess current environment
aws application-discovery describe-agents

# Step 2: Create server mapping
aws application-discovery create-application \
    --name "production-webapp" \
    --description "Web application to migrate"

# Step 3: Export server configuration
aws application-discovery export-configurations \
    --output-file /tmp/migration-config.zip

# Step 4: Create VM images (using AWS VM Import/Export)
# Note: Requires VM Import export first, then:
aws ec2 register-image \
    --name my-server-image \
    --image-location s3://my-bucket/server.vmdk

# Step 5: Launch in cloud (rehost)
aws ec2 run-instances \
    --image-id ami-0c55b159cbfafe1f0 \
    --instance-type t3.micro \
    --key-name my-key-pair \
    --security-group-ids sg-0123456789abcdef0 \
    --subnet-id subnet-0123456789abcdef0

# Step 6: Configure DNS cutover
aws route53 change-resource-record-sets \
    --hosted-zone-id Z1234567890ABC \
    --change-batch '{
        "Changes": [{
            "Action": "UPSERT",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "TTL": 300,
                "ResourceRecords": [{"Value": "13.233.45.1"}]
            }
        }]
    }'
```

### Example 2: Replatforming Implementation

```bash
# Replatforming uses same process but adds optimizations:

# 1. Enable performance monitoring
aws ec2 put-metric-alarm \
    --alarm-name High-CPU \
    --metric-name CPUUtilization \
    --namespace AWS/EC2 \
    --statistic Average \
    --period 300 \
    --threshold 80 \
    --comparison-operator GreaterThanThreshold

# 2. Configure Auto Scaling for unexpected load
aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name web-asg \
    --launch-template "LaunchTemplateName=web-lt" \
    --min-size 2 \
    --max-size 10 \
    --desired-capacity 2 \
    --vpc-zone-identifier subnet-0123456789abcdef0

# 3. Add RDS for managed database (instead of self-managed)
aws rds create-db-instance \
    --db-instance-identifier app-db \
    --db-instance-class db.t3.micro \
    --engine mysql \
    --allocated-storage 20 \
    --master-username admin \
    --master-user-password 'SecurePass123!'

# 4. Configure S3 for static assets
aws s3 mb s3://app-assets
aws s3 website --index-document index.html
```

### Example 3: Refactoring Approach

```bash
# Refactoring = Cloud-native redesign

# 1. Create Lambda function (serverless)
aws lambda create-function \
    --function-name web-handler \
    --runtime nodejs18.x \
    --role arn:aws:iam::123456789:role/lambda-exec \
    --handler index.handler \
    --zip-file fileb://function.zip \
    --timeout 30 \
    --memory-size 256

# 2. Create DynamoDB table
aws dynamodb create-table \
    --table-name app-data \
    --attribute-definitions '[
        {"AttributeName":"id","AttributeType":"S"}
    ]' \
    --key-schema '[
        {"AttributeName":"id","KeyType":"HASH"}
    ]' \
    --billing-mode PAY_PER_REQUEST

# 3. Create API Gateway
aws apigatewayv2 create-api \
    --name web-api \
    --protocol-type HTTP

# 4. Add integration with Lambda
aws apigatewayv2 create-integration \
    --api-id $API_ID \
    --integration-type LAMBDA_PROXY \
    --integration-uri arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-1:123456789:function:web-handler/invocations

# 5. Configure static S3 for assets
aws s3 mb s3://static-assets
aws cloudfront create-distribution \
    --origin-domain-name static-assets.s3.amazonaws.com \
    --default-root-object index.html
```

### Example 4: Application Migration Discovery

```bash
# Using AWS Application Discovery Service

# Enable discovery agent
# (on-premises VM or physical server)
# Then in AWS:

# List discovered servers
aws application-discovery list-servers \
    --filters '[{"name":"osType","values":["WINDOWS"]}]'

# Export discovered data
aws application-discovery export-configurations \
    --output-file /tmp/servers.csv

# Get server recommendations
aws application-discovery get-discovery-summary

# Create migration plan
aws mgn create-replication-template \
    --source-server-id "server-12345" \
    --launchDisposition AVAILABLE
```

## COMMON ISSUES

### 1. Underestimating Migration Complexity

**Problem**: Simple rehost takes longer than expected.

**Solution**: 
- Plan 2x time for legacy systems
- Include application dependencies
- Document custom configurations

### 2. Missing Dependencies

**Problem**: Application fails at cutover due to missing dependencies.

**Solution**: 
- Use discovery tools to map dependencies
- Create comprehensive test plan
- Include network connectivity tests

### 3. Data Transfer Bottlenecks

**Problem**: Large data migrations take too long.

**Solution**:
- Use AWS Snow Family for bulk transfer
- Pre-stage data using S3 Transfer Acceleration
- Schedule transfers during off-peak

### 4. DNS Cutover Issues

**Problem**: DNS change doesn't propagate or fails.

**Solution**:
- Use weighted routing for gradual cutover
- Test TTL reduction before migration
- Have rollback plan ready

### 5. Application Compatibility

**Problem**: Application doesn't work on cloud.

**Solution**:
- Test thoroughly in staging first
- Use re-platforming for known issues
- Consider containerizing for portability

## PERFORMANCE

### Migration Timing Benchmarks

| Application Size | Rehosting | Replatforming | Refactoring |
|------------------|-----------|--------------|--------------|
| Small (1-5 VMs) | 2-4 weeks | 3-6 weeks | 1-3 months |
| Medium (5-20 VMs) | 4-8 weeks | 6-12 weeks | 3-6 months |
| Large (20-50 VMs) | 2-3 months | 3-4 months | 6-12 months |
| Enterprise (50+ VMs) | 3-6 months | 6-9 months | 12-24 months |

### Cost Comparison

| Strategy | Migration Cost | Annual Ops Cost | 3-Year Total |
|----------|---------------|---------------|--------------|
| Rehosting | $50K | $120K | $410K |
| Replatforming | $75K | $96K | $363K |
| Refactoring | $150K | $72K | $366K |
| Repurchasing | $25K | $144K | $457K |

## COMPATIBILITY

### Supported Workloads

| Workload | Rehost | Replatform | Refactor |
|----------|-------|------------|----------|
| Windows Servers | Full | Full | Full |
| Linux Servers | Full | Full | Full |
| Oracle DB | Full | Full | Full |
| MS SQL | Full | Full | Full |
| VMware | Direct | Direct | Via Containers |
| SAP | Limited | Full | Full |

### Tools Available

- **AWS Application Discovery Service**: Assessment and planning
- **AWS VM Import/Export**: VM migration
- **AWS Server Migration Service**: Automated rehosting
- **AWS Database Migration Service**: Database replatforming
- **AWS Migration Hub**: Central tracking

## CROSS-REFERENCES

### Related Concepts

- Cloud Models (Basic): Required prerequisite
- Core AWS Services: Migration target services
- Cost Management: Migration cost planning

### Migration Tool Equivalents

| Tool | AWS | Azure | GCP |
|------|-----|-------|-----|
| Assessment | ADS | Migrate | Migrate |
| VM Import | VM Import | ASR | Migrate |
| Database | DMS | DMA | Database Migration |
| Central | Migration Hub | Mig Hub | Migration Hub |

### Prerequisites

- Basic Cloud Models required
- Review target service documentation

### What to Study Next

1. Advanced Deployment Models: Technical details
2. Practical Deployment: Hands-on migration
3. Cost Management: Migration budget

## EXAM TIPS

### Key Facts

- Six strategies: Rehost, Replatform, Refactor, Repurchase, Retire, Retain
- Most common is Rehosting (lift and shift)
- Refactoring has highest risk but best long-term benefits
- Repurchasing (SaaS) removes infrastructure entirely

### Exam Questions

- **Question**: "No modifications, fastest migration" = Rehosting
- **Question**: "Tune for cloud but don't rewrite" = Replatforming
- **Question**: "Full cloud features, rewrite required" = Refactoring
- **Question**: "Replace with SaaS" = Repurchasing