---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Secret Manager - Advanced
Purpose: Advanced secret rotation, automation, and enterprise integration patterns
Difficulty: advanced
Prerequisites: [01_Basic_Secret_Manager.md]
RelatedFiles: [01_Basic_Secret_Manager.md], [03_Practical_Secret_Manager.md]
UseCase: Secrets management, API keys
CertificationExam: GCP Professional Cloud Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Secret Manager usage addresses enterprise requirements for automated secret rotation, integration with external systems, and compliance with security policies. Manual secret management does not scale and introduces security risks through human error. Automated rotation reduces the blast radius of compromised credentials and ensures compliance with rotation policies. Advanced features also enable multi-cloud secret access and integration with secrets management tools like HashiCorp Vault.

# WHAT

Advanced capabilities:

1. **Automated Rotation**: Scheduled secret updates using Cloud Functions or Cloud Scheduler
2. **External Integrations**: HashiCorp Vault, AWS Secrets Manager sync
3. **Customer-Managed Keys**: Integration with Cloud KMS for key control
4. **Pub/Sub Notifications**: Event-driven secret changes
5. **Fine-Grained Labels**: Organization and lifecycle tracking
6. **Secret References**: Direct mount in Cloud Run, GKE
7. **Replication Policies**: User-managed with specific regions
8. **Conditional IAM**: Attribute-based access control

# HOW

## Example 1: Automated Secret Rotation with Cloud Scheduler

```bash
# Create Cloud Scheduler job for rotation (cron expression)
gcloud scheduler jobs create http secret-rotation \
    --schedule="0 0 1 */ *" \
    --uri="https://secretmanager.googleapis.com/v1/projects/PROJECT/secrets/db-password:addVersion" \
    --oauth-service-account="rotation-sa@PROJECT.iam.gserviceaccount.com" \
    --headers="Content-Type=application/json" \
    --body='{"payload":{"data":"U29tZVBhc3N3b3JkMSE="}}' \
    --description "Monthly password rotation"

# Grant rotation service account access
gcloud secrets add-iam-policy-binding db-password \
    --member="serviceAccount:rotation-sa@PROJECT.iam.gserviceaccount.com" \
    --role="roles/secretmanager.secretVersionAdder"
```

## Example 2: Customer-Managed Encryption Key

```bash
# Create key ring and key in Cloud KMS
gcloud kms keyrings create secret-manager-ring --location=global
gcloud kms keys create secret-cmek \
    --keyring=secret-manager-ring \
    --location=global \
    --purpose=encryption

# Create secret with CMEK
gcloud secrets create encrypted-db-key \
    --replication-policy=automatic \
    --kms-key-name="projects/PROJECT/locations/global/keyRings/secret-manager-ring/cryptoKeys/secret-cmek" \
    --data-file="./key.txt" \
    --description "Database encryption key"
```

## Example 3: Cloud Run with Secret Mount

```yaml
# cloudrun.yaml
apiVersion: serving.knative.dev/v1
kind: Service
metadata:
  name: my-app
spec:
  template:
    spec:
      containers:
      - image: gcr.io/my-project/my-app
        env:
        - name: DB_PASSWORD
          valueFrom:
            secretKeyRef:
              name: db-password
              key: latest
        volumeMounts:
        - name: secrets
          mountPath: /secrets
          readOnly: true
      volumes:
      - name: secrets
        secret:
          secretName: api-key
```

## Example 4: AWS Secrets Manager for Hybrid (Multi-Cloud Pattern)

```bash
# AWS CLI - Create RDS secret in AWS Secrets Manager
aws secretsmanager create-secret \
    --name "prod/db-password" \
    --description "Production database password" \
    --secret-string '{"username":"admin","password":"mypassword"}' \
    --region us-east-1

# AWS CLI - Get secret value
aws secretsmanager get-secret-value \
    --secret-id prod/db-password \
    --region us-east-1

# AWS CLI - Rotate secret (enable auto-rotation)
aws secretsmanager rotate-secret \
    --secret-id prod/db-password \
    -->rotate-immediately
```

# COMMON ISSUES

1. **Rotation Failures**: Ensure rotation function has correct IAM
2. **CMEK Permissions**: Additional roles needed for CMEK usage
3. **Replication Sync**: User-managed secrets may have sync delays
4. **Quota Limits**: 1000 secrets per project default
5. **Cloud Run Mount**: Ensure correct secret version referenced

# PERFORMANCE

Caching recommendations:
- Client-side caching: 1 hour TTL for static secrets
- Redis cache for frequently accessed secrets
- Preload secrets at application startup
- Monitor access latency in Cloud Monitoring

# COMPATIBILITY

Integration with:
- Cloud Functions (event-driven rotation)
- Cloud Scheduler (scheduled rotation)
- Cloud Pub/Sub (notifications)
- GKE workload identity
- Cloud Run (secret mount)
- External load balancers
- HashiCorp Vault (via Vault plugin)
- AWS Secrets Manager (multi-cloud)

# CROSS-REFERENCES

- **01_Basic.yaml**: Fundamental concepts and basic operations
- **03_Practical.yaml**: Production deployment patterns
- **Cloud KMS**: Encryption key management
- **Cloud Scheduler**: Automation scheduling
- **Workload Identity**: GKE authentication

# EXAM TIPS

1. Use labels for rotation tracking and lifecycle
2. Customer-managed keys require Cloud KMS key usage role
3. Automatic rotation uses Cloud Functions by default
4. Pub/Sub notifications for secret changes
5. Cloud Run can mount secrets as files
6. Use latest or specific version in references
7. Rotated secrets create new versions
8. Disabled versions count toward quota