---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud IAM
Purpose: Advanced understanding of GCP IAM features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_IAM.md
RelatedFiles: 01_Basic_Cloud_IAM.md, 03_Practical_Cloud_IAM.md
UseCase: Enterprise access management, security hardening
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced IAM knowledge enables implementing fine-grained access control, managing enterprise identities, and securing GCP resources according to best practices.

### Why Advanced IAM

- **Custom Roles**: Tailored permissions
- **Conditions**: Attribute-based access
- **Policy Troubleshooter**: Debug access
- **Organization Policies**: Resource constraints

## 📖 WHAT

### Advanced Role Management

**Role Lifecycle**:
1. Create custom role
2. Test permissions
3. Grant to principals
4. Audit and update

**Role Recommendations**:
- IAM Recommender suggests roles
- Shows unused permissions
- Recommends minimal roles

### Policy Analysis

- **Policy Troubleshooter**: Why access was denied
- **Policy Analyzer**: What can a principal do
- **IAM Explain**: Understand effective permissions

### Organization Policies

| Policy | Purpose |
|--------|---------|
| constraints/iam.disableServiceAccountKeyUpload | Prevent key export |
| constraints/iam.requireServiceAccount | Require SA creation |
| constraints/resourcemanager.folderChildConstraint | Resource placement |

## 🔧 HOW

### Example 1: Custom Role with Conditions

```bash
# Create custom role
gcloud iam roles create storage.admin.custom \
    --project=my-project \
    --title="Storage Admin Custom" \
    --description="Custom storage admin with conditions" \
    --permissions=storage.buckets.get,storage.buckets.list,storage.objects.get,storage.objects.list \
    --stage=GA

# Grant with condition
gcloud projects add-iam-policy-binding my-project \
    --member=user:dev@example.com \
    --role=projects/my-project/roles/storage.admin.custom \
    --condition="expression=resource.name.startsWith('projects/_/buckets/dev-'),title=Dev Bucket Access"

# Create condition using Resource Manager
gcloud beta resource-manager conditions create \
    --resource=projects/my-project \
    --expression='resource.name.startsWith("projects/_/buckets/dev-")' \
    --title='Dev Environment Access'
```

### Example 2: Using IAM Recommender

```bash
# Get role recommendations
gcloud beta recommender insights list \
    --location=global \
    --recommender=google.iam.policy.Recommender

# Get detailed recommendation
gcloud beta recommender insights describe INSIGHT_ID \
    --location=global \
    --recommender=google.iam.policy.Recommender

# Apply recommendation
gcloud beta recommender insights mark ACCEPTED INSIGHT_ID \
    --location=global \
    --recommender=google.iam.policy.Recommender

# Generate custom role from usage
gcloud iam roles explain roles/storage.objectAdmin \
    --project=my-project
```

### Example 3: Service Account Management

```bash
# Create service account with conditions
gcloud iam service-accounts create app-sa \
    --display-name="App Service Account" \
    --description="Service account for application"

# Create service account key (avoid in production)
gcloud iam service-accounts keys create key.json \
    --iam-account=app-sa@my-project.iam.gserviceaccount.com

# Enable Domain-Wide Delegation
gcloud iam service-accounts update app-sa@my-project.iam.gserviceaccount.com \
    --enable-domain-wide-delegation

# Set up workload identity (recommended)
gcloud iam service-accounts add-iam-policy-binding app-sa@my-project.iam.gserviceaccount.com \
    --member="system:serviceaccount:default:app-sa" \
    --role="roles/iam.workloadIdentityUser"

# Audit service account usage
gcloud logging read "protoPayload.serviceName=iamcredentials.googleapis.com" \
    --limit=50
```

## ⚠️ COMMON ISSUES

### Troubleshooting IAM Issues

| Issue | Solution |
|-------|----------|
| Access denied | Use Policy Troubleshooter |
| Missing permissions | Check role definition |
| Overprivileged | Use Recommender |

### Security Best Practices

- Use Service Accounts over user accounts
- Enable Workload Identity
- Audit with Cloud Logging
- Use Organization Policies

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP IAM | AWS IAM | Azure AD |
|---------|---------|---------|----------|
| Custom Roles | Yes | Yes | Yes |
| Conditions | Yes | Yes | Yes |
| Policy Analyzer | Yes | IAM Access Analyzer | Yes |
| Service Accounts | Yes | IAM Roles | Managed Identities |

## 🔗 CROSS-REFERENCES

### Related Topics

- VPC Service Controls
- Organization Policies
- Cloud Audit Logs

### Study Resources

- IAM documentation
- Best practices for IAM

## ✅ EXAM TIPS

- Custom roles = specific permissions
- Conditions = attribute-based access
- Workload Identity = secure SA access
- Policy Troubleshooter = debug access
