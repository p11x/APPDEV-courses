---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Storage
Purpose: Hands-on exercises for Cloud Storage configurations and management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_Storage.md, 02_Advanced_Cloud_Storage.md
RelatedFiles: 01_Basic_Cloud_Storage.md, 02_Advanced_Cloud_Storage.md
UseCase: Enterprise data management, lifecycle automation, secure storage
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cloud Storage is essential for building data pipelines, implementing compliance requirements, and managing enterprise storage.

### Lab Goals

- Configure lifecycle policies
- Implement versioning and holds
- Create secure access patterns

## 📖 WHAT

### Exercise Overview

1. **Data Lifecycle**: Automated transitions
2. **Compliance**: Object holds, retention
3. **Security**: Signed URLs, encryption

## 🔧 HOW

### Exercise 1: Configure Data Lifecycle

```bash
#!/bin/bash
# Configure lifecycle management for data lake

PROJECT_ID="my-project-id"
BUCKET_NAME="my-datalake-bucket"

gsutil mb -l us-central1 gs://$BUCKET_NAME

# Configure lifecycle policy
cat > lifecycle.json << 'EOF'
{
  "rule": [
    {
      "action": {"type": "SetStorageClass", "storageClass": "NEARLINE"},
      "condition": {"age": 30, "matchesPrefix": ["raw/"]}
    },
    {
      "action": {"type": "SetStorageClass", "storageClass": "COLDLINE"},
      "condition": {"age": 90, "matchesPrefix": ["processed/"]}
    },
    {
      "action": {"type": "SetStorageClass", "storageClass": "ARCHIVE"},
      "condition": {"age": 180, "matchesPrefix": ["archived/"]}
    },
    {
      "action": {"type": "Delete"},
      "condition": {"age": 365, "isLive": false}
    }
  ]
}
EOF

gsutil lifecycle set lifecycle.json gs://$BUCKET_NAME/

# Set up versioning
gsutil versioning set on gs://$BUCKET_NAME/

# Verify configuration
echo "=== Bucket Configuration ==="
gsutil lifecycle get gs://$BUCKET_NAME/
gsutil versioning get gs://$BUCKET_NAME/

echo "Lifecycle configured successfully!"
```

### Exercise 2: Implement Compliance Controls

```bash
#!/bin/bash
# Compliance configuration for regulated data

BUCKET_NAME="my-compliant-bucket"

# Create bucket with retention policy
gsutil mb -l us-central1 gs://$BUCKET_NAME

# Set retention policy (365 days)
gsutil retention set 365 gs://$BUCKET_NAME/

# Verify retention
gsutil retention get gs://$BUCKET_NAME/

# Enable uniform bucket-level access
gsutil uniformbucketlevel access set gs://$BUCKET_NAME/

# Apply legal hold to specific objects
echo "Sensitive data" > sensitive.txt
gsutil cp sensitive.txt gs://$BUCKET_NAME/

# Set legal hold
gsutil retention hold set gs://$BUCKET_NAME/sensitive.txt

# Verify hold
gsutil retention hold get gs://$BUCKET_NAME/sensitive.txt

# Enable public access prevention
gsutil publicaccessprevention set enforced gs://$BUCKET_NAME/

echo "Compliance controls configured!"
echo "Retention: 365 days"
echo "Legal hold: enabled"
```

### Exercise 3: Secure Access Configuration

```bash
#!/bin/bash
# Secure access with signed URLs and encryption

BUCKET_NAME="my-secure-bucket"

# Create bucket
gsutil mb -l us-central1 gs://$BUCKET_NAME

# Generate signed URL for download
gsutil signurl -d 3600 /path/to/service-account.json gs://$BUCKET_NAME/private.txt

# Generate signed URL for upload
gsutil signurl -m PUT -d 3600 /path/to/service-account.json gs://$BUCKET_NAME/upload/

# Configure CORS for web app
cat > cors.json << 'EOF'
[
  {
    "origin": ["https://myapp.example.com"],
    "method": ["GET", "PUT", "DELETE"],
    "responseHeader": ["Authorization", "Content-Type"],
    "maxAgeSeconds": 3600
  }
]
EOF

gsutil cors set cors.json gs://$BUCKET_NAME/

# Use customer-managed encryption keys
# (requires Cloud KMS)
gcloud kms keys create storage-key \
    --location=global \
    --keyring=storage-keyring \
    --purpose=encryption

gsutil encryption set -k projects/my-project/locations/global/keyRings/storage-keyring/cryptoKeys/storage-key gs://$BUCKET_NAME/

# Verify encryption
gsutil encryption get gs://$BUCKET_NAME/

echo "Secure access configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Lifecycle not working | Check JSON syntax |
| Access denied | Verify IAM, signed URLs |
| High costs | Review lifecycle policies |

### Validation

```bash
# Check bucket configuration
gsutil ls -L gs://my-bucket/

# Verify lifecycle
gsutil lifecycle get gs://my-bucket/

# List object versions
gsutil ls -a gs://my-bucket/
```

## 🌐 COMPATIBILITY

### Integration

- BigQuery: Native support
- Cloud Composer: Data pipeline integration
- Transfer Service: Online/offline transfers

## 🔗 CROSS-REFERENCES

### Related Labs

- BigQuery Import/Export
- Cloud Composer
- Transfer Service

### Next Steps

- Set up Cloud Logging
- Configure Cloud Monitoring
- Implement data retention

## ✅ EXAM TIPS

- Know lifecycle action types
- Understand retention policies
- Practice signed URL generation
- Remember to verify configurations
