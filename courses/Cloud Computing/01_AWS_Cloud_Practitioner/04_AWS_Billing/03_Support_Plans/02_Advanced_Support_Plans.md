---
Category: AWS Cloud Practitioner
Subcategory: AWS Billing
Concept: Support Plans - Advanced
Purpose: Advanced support plan features, Trusted Advisor integration, and TAM engagement
Difficulty: advanced
Prerequisites: 01_Basic_Support_Plans.md
RelatedFiles: 01_Basic_Support_Plans.md, 03_Practical_Support_Plans.md
UseCase: Enterprise support strategy
CertificationExam: AWS Certified Solutions Architect Professional
LastUpdated: 2025
---

## WHY

Enterprise support requires advanced features including proactive guidance, architectural reviews, and dedicated resources to optimize AWS usage and resolve complex issues.

### Enterprise Support Requirements

- **Proactive Prevention**: TAM identifies issues before they occur
- **Technical Depth**: Complex troubleshooting capabilities
- **Strategic Planning**: Architecture reviews and optimization
- **Escalation Path**: Direct access to AWS engineering

## WHAT

### Advanced Support Components

| Component | Business | Enterprise | Description |
|-----------|----------|-----------|------------|
| Trusted Advisor | 7 checks | All checks | Infrastructure guidance |
| TAM | No | Yes | Dedicated resource |
| Architecture Reviews | No | Quarterly | Deep dive sessions |
| Concierge | No | Yes | Billing/account focus |
| Personal Health | No | Yes | Custom alerts |

### Cross-Platform Support Comparison

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Basic Support | Free | Free | Free |
| Technical Support | Paid | Paid | Paid |
| TAM | Enterprise | Premier | Enhanced |
| 24/7 Phone | Business | Standard | Premium |
| Architecture Review | Enterprise | Premier | Premium |

### Trusted Advisor Checks

| Check | Category | Business | Enterprise |
|-------|----------|----------|-----------|
| Cost Optimizations | Cost | Yes | Yes |
| Security Groups | Security | Yes | Yes |
| IAM Password | Security | Yes | Yes |
| MFA Root | Security | Yes | Yes |
| EBS Public | Security | Yes | Yes |
| RDS Public | Security | Yes | Yes |
| Service Limits | Performance | Yes | Yes |
| S3 Bucket Permissions | Security | Yes | Yes |

## HOW

### Example 1: Trusted Advisor API Integration

```bash
# Get all checks
aws support describe-trusted-advisor-checks \
    --language en

# Get specific check result
aws support describe-trusted-advisor-check-result \
    --check-id "1e王国21b3-5b78-4d1b-9乙醇1c6e29e" \
    --language en

# Get check summaries
aws support describe-trusted-advisor-check-summaries \
    --check-ids '["1e王国21b3-5b78-4d1b-9乙醇1c6e29e"]'

# Refresh check
aws support refresh-trusted-advisor-check \
    --check-id "1e王国21b3-5b78-4d1b-9乙醇1c6e29e"
```

### Example 2: Advanced Case Management

```bash
# Create escalated case
aws support create-case \
    --subject "Production Down - Urgent" \
    --service-code amazon-elastic-compute-cloud \
    --category-code instance \
    --severity urgent \
    --description "EC2 instances not responding in us-east-1" \
    --language en

# Add file attachment
aws support add-attachments-to-set \
    --attachments-set-id "att-12345678" \
    --attachments '[{
        "fileName": "error-logs.txt",
        "data": "VGhpcyBpcyBlcnJvciBsb2dz..."
    }]'

# Link case to related resources
aws support describe-links \
    --case-id "case-12345678"

# Request API rate limit increase
aws support create-case \
    --subject "API Rate Limit Increase Request" \
    --service-code amazon-api-gateway \
    --category-code rate-limits \
    --severity normal \
    --description "Requesting increased rate limits" \
    --language en
```

### Example 3: TAM Engagement

```bash
# Request architecture review
aws support create-case \
    --subject "Quarterly Architecture Review" \
    --service-code general-information \
    --category-code architecture \
    --severity normal \
    --description "Schedule QBR for account review" \
    --language en

# Request well-architected review
aws support create-case \
    --subject "WA Framework Review" \
    --service-code general-information \
    --category-code well-architected \
    --severity normal \
    --description "Complete WA review for workloads" \
    --language en

# Request cost optimization workshop
aws support create-case \
    --subject "Cost Optimization Workshop" \
    --service-code billing \
    --category-code cost-optimization \
    --severity normal \
    --description "Schedule cost optimization session" \
    --language en
```

### Example 4: AWS Health Events

```bash
# Describe health events
aws health describe-events \
    --filter '{
        "region": "us-east-1",
        "startDate": "2024-01-01T00:00:00Z",
        "endDate": "2024-01-31T23:59:59Z"
    }'

# Describe entity aggregates
aws health describe-entity-aggregates \
    --event-arn "arn:aws:health:us-east-1::event/amazon-ec2/事件-ID"

# Describe affected entities
aws health describe-affected-entities \
    --filter '{
        "eventArn": "arn:aws:health:us-east-1::event/amazon-ec2/事件-ID",
        "entityStatus": "IMPAIRED"
    }'

# Enable health dashboard
aws health enable-health-event \
    --event-type-codes '["AWS_EC2_INSTANCE_RETIREMENT_IMITATED"]'
```

## COMMON ISSUES

### 1. Trusted Advisor Not Updating

**Problem**: Stale check results.

**Solution**:
```bash
# Refresh individual check
aws support refresh-trusted-advisor-check \
    --check-id "check-id"

# Or wait for auto-refresh (24-72 hours)
```

### 2. Case Not Escalating

**Problem**: No response within SLA.

**Solution**:
- Escalate through TAM
- Use severity appropriately
- Provide detailed information

### 3. TAM Not Engaged

**Problem**: No TAM assigned.

**Solution**:
- Verify Enterprise plan
- Request TAM engagement via case
- Set up regular meetings

### 4. Service Limits Issue

**Problem**: Cannot increase limits.

**Solution**:
- Request via support case
- Provide justification
- Allow 24-48 hours

### 5. Trusted Advisor Access

**Problem**: Cannot see all checks.

**Solution**:
- Upgrade to Business
- All checks require Enterprise
- Verify correct account

## PERFORMANCE

### Response Time Metrics

| Severity | Enterprise | Business | Developer |
|----------|------------|----------|-----------|
| Urgent | 15 min | 1 hour | N/A |
| High | 1 hour | 4 hours | 24 hours |
| Normal | 4 hours | 12 hours | 48 hours |
| Low | 24 hours | 72 hours | 72 hours |

### Trusted Advisor Refresh

| Check Type | Auto Refresh | Manual |
|------------|-------------|--------|
| Cost | Daily | Yes |
| Security | Daily | Yes |
| Performance | Daily | Yes |
| Fault | Real-time | N/A |

## COMPATIBILITY

### Support API Availability

| API | Business | Enterprise |
|-----|----------|-----------|
| create-case | Yes | Yes |
| describe-cases | Yes | Yes |
| Trusted Advisor | 7 checks | All |
| Health events | Yes | Yes |
| TAM access | No | Yes |

### Region Support

| Feature | All Regions | Limited |
|---------|-----------|---------|
| Support API | Yes | No |
| Trusted Advisor | Yes | No |
| Health | Yes | Some |
| TAM | Yes | N/A |

## CROSS-REFERENCES

### Related Services

- AWS Health: Event notifications
- Trusted Advisor: Optimization
- AWS Organizations: Multi-account

### Prerequisites

- Support plans basics
- AWS account setup

### What to Study Next

1. Practical Support: Implementation
2. Trusted Advisor automation
3. Enterprise engagement

## EXAM TIPS

### Key Exam Facts

- Business plan required for Trusted Advisor 7 checks
- Enterprise includes all checks + TAM
- 15-minute response for urgent on Enterprise
- TAM provides proactive guidance

### Exam Questions

- **Question**: "Full guidance" = Enterprise + Trusted Advisor
- **Question**: "Quarterly reviews" = Enterprise TAM
- **Question**: "Account optimization" = Concierge
- **Question**: "Always-on alerts" = AWS Health Dashboard