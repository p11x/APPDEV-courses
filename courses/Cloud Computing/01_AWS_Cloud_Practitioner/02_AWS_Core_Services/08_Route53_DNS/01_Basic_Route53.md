---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Route53 DNS
Purpose: Understanding Amazon Route 53 DNS service, routing policies, and health checks
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Route53.md, 03_Practical_Route53.md
UseCase: DNS management and global traffic routing
CertificationExam: AWS Certified Cloud Practitioner - Domain 3
LastUpdated: 2025
---

## WHY

Route 53 is AWS's highly available and scalable DNS service. It's essential for cloud applications because DNS is often the first point of contact users have with your application.

### Why Route53 Matters

- **High Availability**: 100% SLA design
- **Global Infrastructure**: 200+ Route 53 edge locations
- **Integration**: Works seamlessly with other AWS services
- **Health Checks**: Automatic failover

### Key Statistics

- Processes billions of queries daily
- 100% availability SLA
- Supports all DNS record types

## WHAT

### Route53 Core Components

**Hosted Zone**: Container for DNS records.

**Record Sets**: Individual DNS records.

**Health Checks**: Automated health monitoring.

**Routing Policies**: How DNS responds to queries.

### Routing Policies

| Policy | Use Case | Behavior |
|--------|----------|----------|
| Simple | Single record | Returns IP |
| Weighted | Traffic distribution | % to each |
| Latency | Performance | Lowest latency |
| Failover | DR scenarios | Backup on failure |
| Geolocation | Geographic routing | Based on location |

### Record Types

- A, AAAA (IP addresses)
- CNAME (aliases)
- MX (mail)
- TXT (text records)
- NS (name servers)

## HOW

### Example 1: Create Hosted Zone

```bash
# Create hosted zone
aws route53 create-hosted-zone \
    --name example.com \
    --caller-reference "unique-identifier"

# Get nameserver information
aws route53 get-hosted-zone \
    --id /hostedzone/Z1234567890ABC

# Output shows nameservers to configure
```

### Example 2: Create Records

```bash
# Create A record
aws route53 change-resource-record-sets \
    --hosted-zone-id Z1234567890ABC \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "www.example.com",
                "Type": "A",
                "TTL": 300,
                "ResourceRecords": [
                    {"Value": "1.2.3.4"}
                ]
            }
        }]
    }'

# Create CNAME record (for ALB)
aws route53 change-resource-record-sets \
    --hosted-zone-id Z1234567890ABC \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "CNAME",
                "TTL": 300,
                "ResourceRecords": [
                    {"Value": "myalb.us-east-1.elb.amazonaws.com"}
                ]
            }
        }]
    }'
```

### Example 3: Failover Configuration

```bash
# Create health check
HEALTH_CHECK=$(aws route53 create-health-check \
    --caller-reference "unique-check" \
    --health-check-config '{
        "Type": "HTTPS",
        "FullyQualifiedDomainName": "primary.example.com",
        "Port": 443,
        "ResourcePath": "/health",
        "RequestInterval": 10,
        "FailureThreshold": 3
    }' \
    --query 'HealthCheck.Id' \
    --output text)

# Create primary record with health check
aws route53 change-resource-record-sets \
    --hosted-zone-id Z1234567890ABC \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "example.com",
                "Type": "A",
                "Failover": "PRIMARY",
                "SetIdentifier": "primary",
                "HealthCheckId": "'$HEALTH_CHECK'",
                "TTL": 60,
                "ResourceRecords": [{"Value": "1.2.3.4"}]
            }
        }]
    }'

# Create secondary record
aws route53 change-resource-record-sets \
    --hosted-zone-id Z1234567890ABC \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "example.com",
                "Type": "A",
                "Failover": "SECONDARY",
                "SetIdentifier": "secondary",
                "TTL": 60,
                "ResourceRecords": [{"Value": "5.6.7.8"}]
            }
        }]
    }'
```

## PRICING

### Route53 Pricing (per million queries)

| Queries | Price (USD) |
|---------|-------------|
| First 1B | $0.400 |
| Next 1B | $0.200 |
| Next 10B | $0.100 |
| Over 12B | $0.080 |

## CROSS-REFERENCES

### Related Services

- CloudFront: Uses Route53 for DNS
- ELB: Common CNAME target
- Health Checks: Integrate with DNS failover