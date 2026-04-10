---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: GCP Compliance
Purpose: Hands-on exercises for GCP compliance implementation
Difficulty: advanced
Prerequisites: 01_Basic_GCP_Compliance.md, 02_Advanced_GCP_Compliance.md
RelatedFiles: 01_Basic_GCP_Compliance.md, 02_Advanced_GCP_Compliance.md
UseCase: Enterprise compliance implementation, regulatory configuration
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with GCP compliance is essential for implementing enterprise security controls, meeting regulatory requirements, and ensuring data protection.

### Lab Goals

- Configure access controls
- Implement VPC Service Controls
- Set up DLP scanning

## 📖 WHAT

### Exercise Overview

1. **Access Transparency**: Enable and review
2. **VPC Service Controls**: Configure perimeter
3. **DLP**: Sensitive data protection

## 🔧 HOW

### Exercise 1: Configure Access Controls

```bash
#!/bin/bash
# Configure access controls for compliance

ORG_ID="123456789012"

# Enable Access Transparency
gcloud organization policies enable-access-transparency \
    --organization=$ORG_ID

# Enable Access Approval
gcloud services enable accessapproval.googleapis.com

# Configure Access Approval settings
gcloud access-approval settings update \
    --organization=$ORG_ID \
    --enrolled_services=bigquery.googleapis.com,storage.googleapis.com

# Enable Audit Logs
gcloud logging sinks create audit-sink \
    --storage-location=us-central1 \
    --log-filter='protoPayload.serviceName="cloudresourcemanager.googleapis.com"'

# Create access policy
gcloud access-context-manager policies create \
    --organization=$ORG_ID \
    --title="Corporate Access Policy"

# Create access level for trusted users
gcloud access-context-manager access-levels create trusted-users \
    --policy=my-policy \
    --title="Trusted Users" \
    --ip-subnetwork="10.0.0.0/8" \
    --device-policy=require-verified-access

echo "Access controls configured!"
```

### Exercise 2: Configure VPC Service Controls

```bash
#!/bin/bash
# Configure VPC Service Controls

PROJECT_ID="my-project-id"
PERIMETER_NAME="production-perimeter"

gcloud config set project $PROJECT_ID

# Create access context manager policy
gcloud access-context-manager policies create \
    --organization=$ORG_ID \
    --title="Production Policy"

# Create service perimeter
gcloud access-context-manager perimeters create $PERIMETER_NAME \
    --title="Production Data Perimeter" \
    --resource=projects/$PROJECT_ID \
    --restricted-services=bigquery.googleapis.com,storage.googleapis.com,sqladmin.googleapis.com \
    --perimeter-type=PERIMETER

# Configure ingress policy
gcloud access-context-manager perimeters update $PERIMETER_NAME \
    --add-ingress-policies='[
        {
            "ingressFrom": {
                "accessLevels": ["accessPolicies/PLATFORM/accessLevels/trusted"],
                "identities": ["serviceAccount:dev@my-project.iam.gserviceaccount.com"]
            },
            "ingressTo": {
                "operations": [{"serviceName": "bigquery.googleapis.com"}],
                "resources": ["projects/*"]
            }
        }
    ]'

# Configure egress policy
gcloud access-context-manager perimeters update $PERIMETER_NAME \
    --add-egress-policies='[
        {
            "egressFrom": {
                "identities": ["serviceAccount:app@my-project.iam.gserviceaccount.com"]
            },
            "egressTo": {
                "operations": [{"serviceName": "storage.googleapis.com"}],
                "resources": ["projects/*"]
            }
        }
    ]'

# List perimeters
gcloud access-context-manager perimeters list

echo "VPC Service Controls configured!"
```

### Exercise 3: Configure DLP

```bash
#!/bin/bash
# Configure Data Loss Prevention

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create inspect template
gcloud dlp inspect-templates create pii-template \
    --location=us-central1 \
    --type="infotype" \
    --info-types=PASSWORD,CREDIT_CARD_NUMBER,US_SOCIAL_SECURITY_NUMBER,EMAIL_ADDRESS \
    --min-likelihood=LIKELY

# Create de-identification template
gcloud dlp deidentify-templates create deidentify-template \
    --location=us-central1 \
    --type="deidentification" \
    --replace-with-infotype

# Scan bucket for sensitive data
gcloud dlp jobs create inspect \
    --location=us-central1 \
    --storage-config='{"cloudStorageOptions":{"fileSet":{"url":"gs://my-bucket/sensitive-data/"}}}' \
    --inspect-config='{"infoTypes":[{"name":"EMAIL_ADDRESS"}],"includeQuote":true}'

# Schedule recurring DLP job
gcloud dlp jobs create trigger daily-scan \
    --location=us-central1 \
    --schedule='{"period":"24h"}' \
    --storage-config='{"bigQueryOptions":{"tableReference":{"projectId":"my-project","datasetId":"users","tableId":"data"}}}' \
    --inspect-config='{"infoTypes":[{"name":"PASSWORD"}]}'

# Check job status
gcloud dlp jobs list --location=us-central1

echo "DLP configuration complete!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Perimeter errors | Check service permissions |
| DLP quota | Request increase |
| Access denied | Verify access levels |

### Validation

```bash
# Check perimeter status
gcloud access-context-manager perimeters describe $PERIMETER_NAME

# Check DLP jobs
gcloud dlp jobs list --location=us-central1
```

## 🌐 COMPATIBILITY

### Integration

- Cloud IAM
- Cloud Logging
- Cloud KMS

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud IAM
- VPC Service Controls
- Cloud DLP

### Next Steps

- Set up monitoring
- Configure alerts
- Implement encryption

## ✅ EXAM TIPS

- Practice VPC SC configuration
- Know DLP info types
- Understand access transparency
- Monitor compliance status
