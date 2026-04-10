---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Secret Manager - Basic
Purpose: Securely store and manage sensitive data like API keys, passwords, and certificates
Difficulty: beginner
Prerequisites: []
RelatedFiles: [02_Advanced_Secret_Manager.md], [03_Practical_Secret_Manager.md]
UseCase: Secrets management, API keys
CertificationExam: GCP Professional Cloud Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Secret Manager is essential for security-sensitive data that applications need at runtime. Hardcoding secrets in application code or configuration files is a major security risk - if an attacker gains access to your codebase, they have access to all your secrets. Secret Manager provides a centralized, secure repository with encryption at rest, access control through IAM, and audit logging. It integrates natively with Google Cloud services, enabling automatic secret rotation and access without embedding credentials in code.

# WHAT

Secret Manager features:

1. **Secure Storage**: Encrypted at rest using Google Cloud's default encryption (AES-256)
2. **Versioning**: Each secret can have multiple versions for rotation support
3. **IAM Integration**: Fine-grained access control at project or secret level
4. **Audit Logging**: All access is logged in Cloud Logging
5. **Regional Replication**: Secrets replicated across regions for availability
6. **Lifetime Management**: Automatic secret expiration and rotation reminders

# HOW

## Example 1: Create and Access a Secret

```bash
# Create a new secret
echo -n "my-secret-api-key" | gcloud secrets create api-key-prod \
    --replication-policy=automatic \
    --data-file=- \
    --description "Production API key"

# Grant access to a service account
gcloud secrets add-iam-policy-binding api-key-prod \
    --member="serviceAccount:my-service@project.iam.gserviceaccount.com" \
    --role="roles/secretmanager.secretAccessor"

# Access the secret in application
gcloud secrets versions access latest --secret=api-key-prod
```

## Example 2: Create a Secret with Automatic Replication

```bash
# Create secret with user-managed replication (specific regions)
gcloud secrets create db-password \
    --replication-policy=user-managed \
    --locations="us-central1,europe-west1" \
    --data-file="./password.txt" \
    --description "Database password"
```

## Example 3: Version Management

```bash
# Add new version of secret
echo -n "new-password-v2" | gcloud secrets versions add password-secret \
    --data-file=-

# List all versions
gcloud secret-manager list-versions password-secret

# Disable old version
gcloud secret-manager disable-version password-secret/versions/1

# Destroy disabled version
gcloud secret-manager destroy-version password-secret/versions=1
```

# COMMON ISSUES

1. **Access Denied**: Ensure service account has secretmanager.secretAccessor role
2. **Secret Not Found**: Check correct project and secret name
3. **Permission Denied**: IAM policies may take 60 seconds to propagate
4. **Version Not Found**: Using wrong version ID or secret was destroyed
5. **Cost**: Secret Manager has per-access pricing; use caching
6. **Replication Policy**: Check if manual replication required

# PERFORMANCE

Secret access latency is typically 50-100ms. For high-traffic applications:
- Implement client-side caching with TTL
- Use Secret Manager integration with workload identity
- Consider regional proximity for latency-sensitive workloads

# COMPATIBILITY

Secret Manager integrates with:
- Cloud Run with managed secret mounts
- GKE with workload identity
- Compute Engine with service accounts
- Cloud Functions
- App Engine standard environment
- Anthos for hybrid/multi-cloud

Not supported in:
- App Engine flexible (requires manual configuration)

# CROSS-REFERENCES

- **02_Advanced.yaml**: Secret rotation, labels, and automation
- **03_Practical.yaml**: Production patterns and integrations
- **Cloud KMS**: Complementary encryption key management
- **IAM**: Access control documentation
- **Workload Identity**: GKE integration

# EXAM TIPS

1. Secrets are encrypted with AES-256 at rest
2. IAM roles: secretmanager.secretAccessor (read), secretmanager.secretVersionAdder (add versions)
3. Automatic replication uses Google-managed keys
4. User-managed replication allows specific regions
5. Disabled versions can be re-enabled
6. Destroyed versions cannot be recovered
7. Access is logged in Cloud Logging
8. Lifetime labels help track expiration