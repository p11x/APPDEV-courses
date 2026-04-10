---
Category: Google Cloud Platform
Subcategory: GCP Fundamentals
Concept: GCP Resource Hierarchy
Purpose: Hands-on exercises for GCP resource organization and management
Difficulty: advanced
Prerequisites: 01_Basic_GCP_Resource_Hierarchy.md, 02_Advanced_GCP_Resource_Hierarchy.md
RelatedFiles: 01_Basic_GCP_Resource_Hierarchy.md, 02_Advanced_GCP_Resource_Hierarchy.md
UseCase: Enterprise resource organization, hierarchical policies, billing management
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with GCP resource hierarchy is essential for organizing enterprise GCP deployments, implementing proper access control, and managing multi-project environments.

### Lab Goals

- Create folder structure
- Configure organization policies
- Manage project quotas

## 📖 WHAT

### Exercise Overview

1. **Folder Setup**: Department organization
2. **Policy Configuration**: Resource constraints
3. **Project Management**: Quotas and billing

## 🔧 HOW

### Exercise 1: Create Folder Structure

```bash
#!/bin/bash
# Create GCP folder hierarchy

ORG_ID="123456789012"

# Create top-level folders
gcloud resource-manager folders create \
    --display-name="Production" \
    --organization=$ORG_ID

gcloud resource-manager folders create \
    --display-name="Development" \
    --organization=$ORG_ID

gcloud resource-manager folders create \
    --display-name="Shared Services" \
    --organization=$ORG_ID

# Create nested folders
FOLDERS=$(gcloud resource-manager folders list --organization=$ORG_ID --format="value(name)")

for folder in $FOLDERS; do
    echo "Created: $folder"
done

# List all folders
gcloud resource-manager folders list --organization=$ORG_ID

echo "Folder structure created!"
```

### Exercise 2: Configure Organization Policies

```bash
#!/bin/bash
# Configure organization policies

ORG_ID="123456789012"

# Disable service account key export
cat > disable_key_upload.yaml << 'EOF'
constraint: constraints/iam.disableServiceAccountKeyUpload
booleanPolicy:
  enforced: true
EOF

gcloud resource-manager org-policies set-policy \
    --organization=$ORG_ID \
    --policy-file=disable_key_upload.yaml

# Require OS Login
cat > require_oslogin.yaml << 'EOF'
constraint: constraints/compute.requireOsLogin
booleanPolicy:
  enforced: true
EOF

gcloud resource-manager org-policies set-policy \
    --organization=$ORG_ID \
    --policy-file=require_oslogin.yaml

# Restrict to specific zones
cat > restrict_zones.yaml << 'EOF'
constraint: constraints/compute.restrictZones
listPolicy:
  allowedValues:
    - us-central1
    - us-east1
EOF

gcloud resource-manager org-policies set-policy \
    --organization=$ORG_ID \
    --policy-file=restrict_zones.yaml

# List organization policies
gcloud resource-manager org-policies list --organization=$ORG_ID

echo "Organization policies configured!"
```

### Exercise 3: Project Management

```bash
#!/bin/bash
# Manage projects and quotas

# Create new project
gcloud projects create my-new-project \
    --name="New Project" \
    --labels=environment=production

# Link to billing account
gcloud billing projects link my-new-project \
    --billing-account=XXXXXX-XXXXXX-XXXXXX

# Set project labels
gcloud projects update my-new-project \
    --labels=environment=production,team=platform, cost-center=engineering

# Check project quotas
gcloud compute project-info describe --project=my-new-project

# Request quota increase
gcloud compute regions update us-central1 \
    --project=my-new-project \
    --quotas=CORE:20

# Enable APIs
gcloud services enable compute.googleapis.com \
    --project=my-new-project

gcloud services enable storage.googleapis.com \
    --project=my-new-project

# List projects
gcloud projects list

echo "Projects managed successfully!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Can't create project | Check billing |
| Quota exceeded | Request increase |
| Policy not applied | Check hierarchy |

### Validation

```bash
# Check folder permissions
gcloud resource-manager folders get-iam-policy folders/FOLDER_ID

# Check project IAM
gcloud projects get-iam-policy PROJECT_ID

# List organization policies
gcloud resource-manager org-policies list --organization=ORG_ID
```

## 🌐 COMPATIBILITY

### Integration

- Cloud IAM
- Cloud Billing
- Cloud Logging

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud IAM
- Organization Policies
- Billing Setup

### Next Steps

- Set up folder hierarchy
- Configure access control
- Implement billing alerts

## ✅ EXAM TIPS

- Practice folder creation
- Know organization policies
- Understand resource hierarchy
- Manage project quotas
