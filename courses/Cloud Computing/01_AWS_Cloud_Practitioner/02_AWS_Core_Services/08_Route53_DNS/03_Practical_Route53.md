---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Route53 DNS
Purpose: Practical Route53 labs including DNS failover and complex routing scenarios
Difficulty: intermediate
Prerequisites: 01_Basic_Route53.md, 02_Advanced_Route53.md
RelatedFiles: 01_Basic_Route53.md, 02_Advanced_Route53.md
UseCase: Production DNS management
CertificationExam: AWS Solutions Architect
LastUpdated: 2025
---

## WHY

Hands-on Route53 labs provide practical DNS management experience.

## WHAT

### Lab: DNS Failover Architecture

Implement automated failover with health checks.

## HOW

### Module 1: Create Health Checks

```bash
# Create health check for primary
aws route53 create-health-check \
    --caller-reference "health-$(date +%s)" \
    --health-check-config '{
        "Type": "HTTPS",
        "FullyQualifiedDomainName": "primary.example.com",
        "Port": 443,
        "ResourcePath": "/health",
        "RequestInterval": 10,
        "FailureThreshold": 3
    }'
```

### Module 2: Create Records

```bash
# Primary record
aws route53 change-resource-record-sets \
    --hosted-zone-id Z123456789 \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "Failover": "PRIMARY",
                "SetIdentifier": "primary",
                "HealthCheckId": "health-check-id",
                "TTL": 60,
                "ResourceRecords": [{"Value": "1.2.3.4"}]
            }
        }]
    }'
```

### Module 3: Secondary Record

```bash
# Secondary (failover) record
aws route53 change-resource-record-sets \
    --hosted-zone-id Z123456789 \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "Failover": "SECONDARY",
                "SetIdentifier": "secondary",
                "TTL": 60,
                "ResourceRecords": [{"Value": "5.6.7.8"}]
            }
        }]
    }'
```

## VERIFICATION

```bash
# Test health check
aws route53 get-health-check --health-check-id health-check-id
```

## CROSS-REFERENCES

### Prerequisites

- Basic Route53 knowledge