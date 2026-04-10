---
Category: Google Cloud Platform
Subcategory: GCP Fundamentals
Concept: GCP Resource Hierarchy
Purpose: Advanced understanding of GCP resource organization and policy management
Difficulty: intermediate
Prerequisites: 01_Basic_GCP_Resource_Hierarchy.md
RelatedFiles: 01_Basic_GCP_Resource_Hierarchy.md, 03_Practical_GCP_Resource_Hierarchy.md
UseCase: Enterprise resource organization, hierarchical policies, access control
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced resource hierarchy knowledge enables implementing enterprise-scale GCP organizations with proper policy inheritance, billing management, and access control structures.

### Why Advanced Hierarchy

- **Organization Policies**: Resource constraints
- **Folder Hierarchy**: Department organization
- **Project Quotas**: Resource limits
- **Billing at Scale**: Multi-project billing

## 📖 WHAT

### Organization Policies

| Policy | Purpose |
|--------|---------|
| constraints/iam.allowedPolicyMemberDomains | Restrict to specific domains |
| constraints/compute.requireOsLogin | Enable OS Login |
| constraints/sql.restrictAuthorizedNetworks | Restrict SQL access |
| constraints/iam.disableServiceAccountKeyUpload | Prevent key export |

### Folder Structure Best Practices

**Common Hierarchy**:
```
Organization (company.com)
├── Folder: Production
│   ├── Project: prod-app-1
│   └── Project: prod-app-2
├── Folder: Development
│   ├── Project: dev-app-1
│   └── Project: dev-app-2
└── Folder: Shared Services
    ├── Project: shared-monitoring
    └── Project: shared-logging
```

### Policy Inheritance

- Explicit deny overrides allow
- Policies flow down to children
- Lower-level policies can restrict
- Audit logging for compliance

## 🔧 HOW

### Example 1: Create Folder Hierarchy

```bash
# Create folder
gcloud resource-manager folders create \
    --display-name="Production" \
    --organization=ORG_ID

# Create nested folder
gcloud resource-manager folders create \
    --display-name="Production Apps" \
    --folder=folders/PARENT_FOLDER_ID

# Move project to folder
gcloud projects move PROJECT_ID \
    --folder=folders/TARGET_FOLDER_ID

# List folder contents
gcloud resource-manager folders list --folder=folders/FOLDER_ID
```

### Example 2: Set Organization Policies

```bash
# Set organization policy
gcloud resource-manager org-policies set-policy \
    --organization=ORG_ID \
    --policy-file=policy.yaml

# policy.yaml example
# constraint: constraints/iam.allowedPolicyMemberDomains
# condition: {}
# listPolicy:
#   allowedValues:
#     - DOMAIN1.com
#     - DOMAIN2.com
#   deniedValues: []

# Disable service account key upload
gcloud resource-manager org-policies set-policy \
    --organization=ORG_ID \
    --constraint='constraints/iam.disableServiceAccountKeyUpload' \
    --boolean-policy=true

# Restrict compute resources to specific zones
gcloud resource-manager org-policies set-policy \
    --organization=ORG_ID \
    --constraint='constraints/compute.restrictZones' \
    --list-policy='allowedValues: ["us-central1-a", "us-central1-b"]'
```

### Example 3: Manage Project Quotas

```bash
# List quotas
gcloud compute project-info describe --project=PROJECT_ID

# Request quota increase
gcloud compute regions update us-central1 \
    --quotas=CORE:10,SSD_TOTAL_GB:10000

# Set project labels
gcloud projects update PROJECT_ID \
    --labels=environment=production,team=platform

# Get project metadata
gcloud projects describe PROJECT_ID

# Enable APIs at organization level
gcloud services enable compute.googleapis.com \
    --organization=ORG_ID
```

## ⚠️ COMMON ISSUES

### Troubleshooting Hierarchy Issues

| Issue | Solution |
|-------|----------|
| Can't create project | Check quota, billing |
| Policies not applying | Check inheritance |
| Permission denied | Verify IAM at correct level |

### Best Practices

- Use folders for department organization
- Enable organization policy inheritance
- Use labels for cost tracking
- Implement least privilege at project level

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP | AWS | Azure |
|---------|-----|-----|-------|
| Organization | Yes | AWS Organizations | Management Groups |
| Folders | Yes | OUs | Management Groups |
| Projects | Yes | Accounts | Subscriptions |
| Policies | Yes | SCPs | Azure Policy |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud IAM (access control)
- Organization Policies
- Billing Setup

### Study Resources

- Resource Manager documentation
- Organization policy constraints

## ✅ EXAM TIPS

- Policies flow down (inherit)
- Organization > Folders > Projects
- Organization policies constrain resources
- Project is the basic billing unit
