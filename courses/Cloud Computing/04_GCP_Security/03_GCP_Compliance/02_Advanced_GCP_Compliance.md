---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: GCP Compliance
Purpose: Advanced understanding of GCP compliance programs and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_GCP_Compliance.md
RelatedFiles: 01_Basic_GCP_Compliance.md, 03_Practical_GCP_Compliance.md
UseCase: Enterprise compliance implementation, regulatory requirements
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced compliance knowledge enables implementing enterprise compliance requirements, configuring access controls, and ensuring regulatory compliance for sensitive data.

### Why Advanced Compliance

- **Access Transparency**: Audit access to data
- **VPC Service Controls**: Data exfiltration protection
- **Data Loss Prevention**: Sensitive data discovery
- **Customer-Managed Keys**: Encryption control

## 📖 WHAT

### Shared Responsibility Matrix

| Responsibility | GCP | Customer |
|---------------|-----|----------|
| Physical Security | ✓ | |
| Network Infrastructure | ✓ | |
| Hypervisor | ✓ | |
| OS | ✓ | |
| Data Access Control | | ✓ |
| Application Security | | ✓ |
| Data Classification | | ✓ |

### Compliance Tools

**Access Transparency**:
- Google employee access logs
- Just-in-time access
- Customer data in regulated countries

**VPC Service Controls**:
- Perimeter security
- Data exfiltration protection
- Public access prevention

## 🔧 HOW

### Example 1: Access Transparency

```bash
# Enable Access Transparency
gcloud organization policies enable-access-transparency \
    --organization=ORG_ID

# Configure Access Approval
gcloud services enable accessapproval.googleapis.com

# Request access approval
gcloud access-approval requests create \
    --organization=ORG_ID \
    --request=reason="Production emergency"

# Review access logs
gcloud logging read "protoPayload.methodName=AccessTransparency"
```

### Example 2: VPC Service Controls

```bash
# Create service perimeter
gcloud access-context-manager perimeters create production-perimeter \
    --title="Production Perimeter" \
    --resource=projects/my-project-number \
    --restricted-services=bigquery.googleapis.com,storage.googleapis.com \
    --ingress-from=accessLevels=projects/access-context-manager/accessPolicies/PLATFORM/access-levels/trusted \
    --egress-to=resources=projects/other-project

# Add resources to perimeter
gcloud access-context-manager perimeters update production-perimeter \
    --add-resources=projects/my-project-number

# Configure egress policies
gcloud access-context-manager perimeters update production-perimeter \
    --egress-policy='{
        "egressFrom": {"identities": [{"type": "serviceAccount"}]},
        "egressTo": {"operations": [{"serviceName": "bigquery.googleapis.com"}]}
    }'
```

### Example 3: Data Loss Prevention

```bash
# Enable DLP API
gcloud services enable dlp.googleapis.com

# Create DLP inspect template
gcloud dlp inspect-templates create sensitive-data-template \
    --location=us-central1 \
    --project=my-project \
    --info-types='GOOGLE_CREDENTIALS,PASSWORD,CREDIT_CARD_NUMBER' \
    --min-likelihood=LIKELY

# Create de-identification template
gcloud dlp deidentify-templates create anonymize-template \
    --location=us-central1 \
    --project=my-project \
    --replace-with-infotype-config

# Run DLP inspection job
gcloud dlp jobs create inspect-storage \
    --location=us-central1 \
    --project=my-project \
    --storage-config='{"cloudStorageOptions":{"fileSet":[{"url":"gs://my-bucket/sensitive/"}]}' \
    --inspect-config='{"infoTypes":[{"name":"PASSWORD"}]}'
```

## ⚠️ COMMON ISSUES

### Troubleshooting Compliance Issues

| Issue | Solution |
|-------|----------|
| Data exfiltration | Configure VPC SC |
| Regulatory gaps | Use DLP |
| Access audit | Enable Access Transparency |

### Implementation Best Practices

- Enable VPC Service Controls
- Use Customer-Managed Keys
- Implement DLP scanning
- Enable Audit Logs

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP | AWS | Azure |
|---------|-----|-----|-------|
| Access Transparency | Yes | No | No |
| VPC Service Controls | Yes | VPC Endpoints | VNet Service Endpoints |
| DLP | Yes | Macie | Purview |
| Compliance Programs | Extensive | Extensive | Extensive |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud KMS (encryption)
- VPC Service Controls
- Cloud DLP

### Study Resources

- GCP Compliance website
- Security best practices

## ✅ EXAM TIPS

- VPC SC prevents data exfiltration
- Access Transparency logs Google access
- DLP for sensitive data discovery
- Customer-managed keys for encryption
