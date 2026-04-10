---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud SQL
Purpose: Hands-on exercises for Cloud SQL deployment and management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_SQL.md, 02_Advanced_Cloud_SQL.md
RelatedFiles: 01_Basic_Cloud_SQL.md, 02_Advanced_Cloud_SQL.md
UseCase: Production database deployment, HA setup, disaster recovery
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cloud SQL is essential for managing production databases, implementing high availability, and ensuring data protection.

### Lab Goals

- Deploy production-ready Cloud SQL
- Configure high availability
- Implement disaster recovery
- Optimize performance

## 📖 WHAT

### Exercise Overview

1. **HA Deployment**: Multi-zone configuration
2. **Replication**: Read replicas setup
3. **Disaster Recovery**: Backup and restore
4. **Security**: VPC and encryption

## 🔧 HOW

### Exercise 1: Deploy Production Cloud SQL

```bash
#!/bin/bash
# Deploy production Cloud SQL with HA

PROJECT_ID="my-project-id"
INSTANCE_NAME="prod-database"

gcloud config set project $PROJECT_ID

# Create production instance with HA
gcloud sql instances create $INSTANCE_NAME \
    --database-version=POSTGRES_15 \
    --tier=db-custom-2-4096 \
    --zone=us-central1-a \
    --enable-high-availability \
    --maintenance-window-day=SUNDAY \
    --maintenance-window-hour=2 \
    --backup-start-time="03:00" \
    --backup-location=us \
    --enable-point-in-time-recovery \
    --retained-backups-count=30 \
    --retained-interval-days=7 \
    --storage-auto-increase \
    --storage-type=SSD \
    --storage-size=50GB \
    --database-flags=max_connections=100,log_connections=on

# Configure additional backups
gcloud sql instances patch $INSTANCE_NAME \
    --backup-start-time="03:00" \
    --enabled-backup-recovery \
    --backup-retained-counts=30

# Create database
gcloud sql databases create production_db --instance=$INSTANCE_NAME

# Create user
gcloud sql users create dbadmin \
    --instance=$INSTANCE_NAME \
    --password=SecurePass123

echo "Production Cloud SQL deployed!"
echo "HA: enabled"
echo "Backups: 30 days retention"
```

### Exercise 2: Configure Read Replicas

```bash
#!/bin/bash
# Configure read replicas for scaling

PROJECT_ID="my-project-id"
PRIMARY_INSTANCE="prod-database"

gcloud config set project $PROJECT_ID

# Create read replica in same region
gcloud sql instances create read-replica-us \
    --database-version=POSTGRES_15 \
    --tier=db-custom-2-4096 \
    --zone=us-central1-b \
    --source-instance=$PRIMARY_INSTANCE \
    --replica-type=READ_REPLICA

# Create cross-region read replica
gcloud sql instances create read-replica-eu \
    --database-version=POSTGRES_15 \
    --tier=db-custom-2-4096 \
    --zone=europe-west1-a \
    --source-instance=$PRIMARY_INSTANCE \
    --replica-type=READ_REPLICA

# Verify replica status
gcloud sql instances describe $PRIMARY_INSTANCE --format="value(replicaConfiguration)"

# List all replicas
gcloud sql instances list --filter="replicaOf:$PRIMARY_INSTANCE"

# Test failover (promote replica)
echo "Testing DR failover..."
gcloud sql instances promote-replica read-replica-eu

echo "Read replicas configured!"
echo "US: read scaling"
echo "EU: disaster recovery"
```

### Exercise 3: Disaster Recovery Setup

```bash
#!/bin/bash
# Configure disaster recovery

PROJECT_ID="my-project-id"
INSTANCE_NAME="prod-database"

gcloud config set project $PROJECT_ID

# Enable point-in-time recovery
gcloud sql instances patch $INSTANCE_NAME \
    --enable-point-in-time-recovery

# Create on-demand backup
gcloud sql instances restore-backup $INSTANCE_NAME \
    --backup-id=$(gcloud sql instances list-backups $INSTANCE_NAME --format="value(id)" | head -1)

# Test point-in-time recovery
gcloud sql instances restore-backup $INSTANCE_NAME \
    --point-in-time="2025-01-15T10:00:00Z" \
    --backup-id=1

# Create export to Cloud Storage
gcloud sql export sql $INSTANCE_NAME \
    gs://my-backup-bucket/backup.sql \
    --database=production_db

# Schedule automated exports
gcloud scheduler jobs create http daily-export \
    --schedule="0 4 * * *" \
    --uri="https://sqladmin.googleapis.com/v1/projects/$PROJECT_ID/instances/$INSTANCE_NAME/export" \
    --headers="Content-Type=application/json" \
    --body='{"exportContext":{"kind":"sql#exportContext","fileType":"SQL","databases":["production_db"],"uri":"gs://my-backup-bucket/daily-backup.sql"}}'

echo "DR configuration complete!"
echo "Point-in-time recovery: enabled"
echo "Backups: automated + on-demand"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Connection failures | Check firewall rules |
| Slow queries | Enable query insights |
| Replica lag | Check network latency |

### Verification Commands

```bash
# Check instance status
gcloud sql instances describe prod-database --format="value(state)"

# Check replica status
gcloud sql instances list --filter="replicaOf:prod-database"

# Verify backups
gcloud sql instances list-backups prod-database
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Run: Private connection
- GKE: Cloud SQL Proxy
- Dataflow: Direct connection

## 🔗 CROSS-REFERENCES

### Related Labs

- VPC Networking
- Cloud Run
- GKE

### Next Steps

- Set up monitoring
- Configure alerts
- Implement connection pooling

## ✅ EXAM TIPS

- Know HA configuration
- Understand replica types
- Remember backup strategies
- Practice restore procedures
