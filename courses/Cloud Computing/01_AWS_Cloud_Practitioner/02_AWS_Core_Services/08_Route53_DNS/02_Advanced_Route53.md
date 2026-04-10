---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Route53 DNS
Purpose: Advanced Route53 including health checks, weighted routing, and failover configurations
Difficulty: advanced
Prerequisites: 01_Basic_Route53.md
RelatedFiles: 01_Basic_Route53.md, 03_Practical_Route53.md
UseCase: Complex DNS architectures
CertificationExam: AWS Solutions Architect
LastUpdated: 2025
---

## WHY

Advanced Route53 features enable sophisticated traffic routing and health management.

## WHAT

### Weighted Routing

Distribute traffic across multiple resources based on weight.

### Latency-Based Routing

Route to lowest latency region.

### Geolocation Routing

Route based on user location.

### Failover Routing

Automatic failover to backup resources.

## HOW

### Weighted Routing

```bash
# Create weighted record
aws route53 change-resource-record-sets \
    --hosted-zone-id Z123456789 \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "SetIdentifier": "primary",
                "Weight": 80,
                "TTL": 300,
                "ResourceRecords": [{"Value": "1.2.3.4"}]
            }
        }, {
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "SetIdentifier": "secondary",
                "Weight": 20,
                "TTL": 300,
                "ResourceRecords": [{"Value": "5.6.7.8"}]
            }
        }]
    }'
```

### Geolocation Routing

```bash
# Create geolocation record
aws route53 change-resource-record-sets \
    --hosted-zone-id Z123456789 \
    --change-batch '{
        "Changes": [{
            "Action": "CREATE",
            "ResourceRecordSet": {
                "Name": "app.example.com",
                "Type": "A",
                "GeoLocation": {"CountryCode": "US"},
                "TTL": 300,
                "ResourceRecords": [{"Value": "1.2.3.4"}]
            }
        }]
    }'
```

## CROSS-REFERENCES

### Related Services

- CloudFront: Content delivery
- ELB: Load balancing