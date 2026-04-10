---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Compliance Programs
Purpose: Understanding AWS compliance programs, certifications, and regulatory frameworks
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Compliance.md, 03_Practical_Compliance.md
UseCase: Meeting compliance requirements in AWS
CertificationExam: AWS Certified Cloud Practitioner - Domain 2
LastUpdated: 2025
---

## WHY

Compliance is essential for regulated industries. Understanding AWS compliance programs helps ensure your applications meet necessary requirements.

### AWS Compliance Programs

- **SOC 1/2/3**: Security and availability
- **ISO 27001**: Information security
- **FedRAMP**: US government
- **HIPAA**: Healthcare
- **PCI DSS**: Payment card industry

### Compliance Responsibility Matrix

| Responsibility | AWS | Customer |
|----------------|-----|----------|
| Physical Security | AWS | N/A |
| Network Security | AWS | N/A |
| Instance Patching | Shared | Config Management |
| Data Encryption | Shared | Key management |
| Access Control | Shared | IAM policies |
| Application Security | Customer | Customer |

## HOW

### Example 1: Enable AWS Config

```bash
# Enable AWS Config
aws configservice put-configuration-recorder \
    --configuration-recorder '{
        "name": "default",
        "roleARN": "arn:aws:iam::123456789:role/config-role"
    }'

# Setup delivery channel
aws configservice put-delivery-channel \
    --delivery-channel '{
        "name": "default",
        "s3BucketName": "my-config-bucket"
    }'
```

### Example 2: Use AWS Artifact

```bash
# Download AWS compliance reports
# Via AWS Artifact console or website

# Accept agreement for HIPAA
aws marketplace-agreements search \
    --filters 'MarketplaceType=AWS_Marketplace'

# Get SOC reports
# Via AWS Artifact
```

### Example 3: Enable CloudTrail

```bash
# Create trail for compliance
aws cloudtrail create-trail \
    --name compliance-trail \
    --s3-bucket-name my-logs-bucket \
    --is-multi-region-trail \
    --include-global-service-events

# Start logging
aws cloudtrail start-logging \
    --name compliance-trail
```

## PRICING

| Service | What You Pay |
|--------|--------------|
| AWS Config | Per rule evaluated |
| CloudTrail | Per event |
| Artifact | Free for agreements |

## CROSS-REFERENCES

### Related Services

- AWS Config: Compliance monitoring
- CloudTrail: Audit logging
- Artifact: Compliance reports
- IAM: Access control