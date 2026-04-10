---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud IAM
Purpose: Hands-on exercises for IAM configuration and management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_IAM.md, 02_Advanced_Cloud_IAM.md
RelatedFiles: 01_Basic_Cloud_IAM.md, 02_Advanced_Cloud_IAM.md
UseCase: Enterprise access management, security implementation
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with IAM is essential for implementing proper access control, managing service accounts, and securing GCP resources.

### Lab Goals

- Create custom roles
- Configure service accounts
- Implement access policies

## 📖 WHAT

### Exercise Overview

1. **Custom Roles**: Create and manage
2. **Service Accounts**: Secure deployment
3. **Policy Analysis**: Debug access

## 🔧 HOW

### Exercise 1: Create and Manage Custom Roles

```bash
#!/bin/bash
# Create and manage custom IAM roles

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Define custom role permissions
ROLE_PERMISSIONS=(
    "storage.buckets.get"
    "storage.buckets.list"
    "storage.objects.get"
    "storage.objects.list"
    "storage.objects.create"
)

# Create custom role
gcloud iam roles create app.storage.manager \
    --project=$PROJECT_ID \
    --title="App Storage Manager" \
    --description="Custom role for app storage access" \
    --permissions="${ROLE_PERMISSIONS[*]}"

# List custom roles
gcloud iam roles list --project=$PROJECT_ID

# Update custom role with new permissions
gcloud iam roles update app.storage.manager \
    --project=$PROJECT_ID \
    --add-permissions="storage.buckets.update"

# Grant custom role
gcloud projects add-iam-policy-binding $PROJECT_ID \
    --member=user:developer@example.com \
    --role=projects/$PROJECT_ID/roles/app.storage.manager

# Audit role usage
gcloud iam roles explain app.storage.manager \
    --project=$PROJECT_ID

echo "Custom roles configured!"
```

### Exercise 2: Secure Service Account Setup

```bash
#!/bin/bash
# Set up secure service accounts

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create service account with description
gcloud iam service-accounts create app-service-sa \
    --display-name="Application Service Account" \
    --description="Service account for application access"

# Disable service account (when not in use)
gcloud iam service-accounts update \
    app-service-sa@$PROJECT_ID.iam.gserviceaccount.com \
    --disabled

# Grant minimal permissions
gcloud projects add-iam-policy-binding $PROJECT_ID \
    --member=serviceAccount:app-service-sa@$PROJECT_ID.iam.gserviceaccount.com \
    --role=roles/storage.objectViewer

gcloud projects add-iam-policy-binding $PROJECT_ID \
    --member=serviceAccount:app-service-sa@$PROJECT_ID.iam.gserviceaccount.com \
    --role=roles/bigquery.dataViewer

# Set up IAM policy conditions
gcloud projects add-iam-policy-binding $PROJECT_ID \
    --member=serviceAccount:app-service-sa@$PROJECT_ID.iam.gserviceaccount.com \
    --role=roles/storage.objectAdmin \
    --condition="expression=resource.name.startsWith('projects/_/buckets/app-data-'),title=App Data Access"

# Audit service account keys
gcloud iam service-accounts keys list \
    --iam-account=app-service-sa@$PROJECT_ID.iam.gserviceaccount.com

# Create JSON key (for external systems)
gcloud iam service-accounts keys create key.json \
    --iam-account=app-service-sa@$PROJECT_ID.iam.gserviceaccount.com

echo "Service accounts configured!"
```

### Exercise 3: Use Policy Analyzer

```bash
#!/bin/bash
# Analyze and debug IAM policies

PROJECT_ID="my-project-id"

# Analyze what a user can do
gcloud beta iam policy-analyzer analyze \
    --principal-email=user@example.com \
    --full-resource-name=//storage.googleapis.com/buckets/my-bucket

# Analyze access to a specific resource
gcloud beta iam policy-analyzer analyze \
    --access-filter="principal:user@example.com" \
    --full-resource-name=//cloudkms.googleapis.com/projects/my-project/locations/global/keyRings/my-keyring

# Check effective permissions
gcloud beta iam policy-analyzer explain \
    --principal-email=sa@$PROJECT_ID.iam.gserviceaccount.com \
    --permissions=storage.buckets.get,storage.objects.list

# Troubleshoot denied access
gcloud beta resource-manager test-iam-permissions \
    projects/$PROJECT_ID \
    --permissions=storage.buckets.get

# List all policies
gcloud projects get-iam-policy $PROJECT_ID \
    --format=json > policy.json

# View specific role bindings
jq '.bindings[] | select(.role | contains("Storage"))' policy.json

echo "Policy analysis complete!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Access denied | Use Policy Analyzer |
| Missing permissions | Check role definition |
| Overprivileged | Use Recommender |

### Validation

```bash
# Test permissions
gcloud beta resource-manager test-iam-permissions \
    projects/$PROJECT_ID --permissions=resourcemanager.projects.get

# Check service account status
gcloud iam service-accounts describe app-sa@$PROJECT_ID.iam.gserviceaccount.com
```

## 🌐 COMPATIBILITY

### Integration

- Workload Identity
- VPC Service Controls
- Cloud Audit Logs

## 🔗 CROSS-REFERENCES

### Related Labs

- Workload Identity
- VPC Networking
- Organization Policies

### Next Steps

- Set up VPC SC
- Configure Audit Logs
- Implement security scoring

## ✅ EXAM TIPS

- Practice custom role creation
- Know service account best practices
- Use Policy Analyzer
- Understand IAM conditions
