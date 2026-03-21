# Velero Backup and Restore

## Overview

Velero is an open-source tool for backing up and restoring Kubernetes resources and persistent volumes. It provides a unified solution for disaster recovery.

## Prerequisites

- Kubernetes cluster
- Object storage (S3, GCS, Azure Blob)

## Core Concepts

### Velero Components

| Component | Purpose |
|-----------|---------|
| Velero server | Runs on cluster, performs backups |
| Restic | Backs up persistent volumes |
| CLI | Commands for backup/restore |

### Storage Providers

| Provider | Backend |
|----------|---------|
| AWS | S3 |
| GCP | Google Cloud Storage |
| Azure | Azure Blob |
| MinIO | S3-compatible |

## Step-by-Step Examples

### Install Velero

```bash
# Install Velero CLI
brew install velero

# Install Velero with S3
velero install \
  --provider aws \
  --plugins velero/velero-plugin-for-aws:v1.8.0 \
  --bucket my-backup-bucket \
  --secret-file ./credentials-velero \
  --backup-location-config region=us-east-1 \
  --snapshot-location-config region=us-east-1
```

### Create Backup

```bash
# Backup entire namespace
velero backup create daily-backup \
  --include-namespaces production \
  --ttl 720h

# Backup with volume snapshots
velero backup create full-backup \
  --include-namespaces production \
  --snapshot-volumes \
  --ttl 168h
```

### Schedule Regular Backups

```yaml
apiVersion: velero.io/v1
kind: Schedule
metadata:
  name: daily-backup
  namespace: velero
spec:
  schedule: "0 2 * * *"  # Daily at 2 AM
  template:
    includedNamespaces:
    - production
    ttl: 168h
    snapshotVolumes: true
```

### Restore from Backup

```bash
# List backups
velero backup get

# Restore from backup
velero restore create --from-backup daily-backup

# Restore to different namespace
velero restore create restore-prod \
  --from-backup daily-backup \
  --namespace-mappings production:production-new
```

### Restore Specific Resources

```bash
# Restore only deployments
velero restore create partial-restore \
  --from-backup daily-backup \
  --include-resources deployments
```

## Gotchas for Docker Users

- **No Docker equivalent**: Kubernetes-specific backup tool
- **Volume backup**: Requires Restic or CSI snapshots
- **Storage needed**: Requires object storage bucket

## Common Mistakes

- **No test restore**: Backup never verified
- **Expired TTL**: Backups auto-deleted
- **Incomplete includes**: Missing resources

## Quick Reference

| Command | Action |
|---------|--------|
| velero backup create | Create backup |
| velero backup get | List backups |
| velero restore create | Restore |
| velero schedule create | Scheduled backup |

## What's Next

Continue to [Multi-Cluster DR](./03-multi-cluster-dr.md) for advanced disaster recovery.
