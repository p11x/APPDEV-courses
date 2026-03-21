# Backup Strategies

## Overview

Disaster recovery in Kubernetes requires backing up etcd, persistent volumes, and cluster resources. A solid backup strategy ensures business continuity.

## Prerequisites

- Kubernetes cluster
- Backup tool knowledge

## Core Concepts

### What to Backup

| Component | Data | Priority |
|-----------|------|----------|
| etcd | Cluster state | Critical |
| PersistentVolumes | Application data | High |
| Secrets/ConfigMaps | Credentials | Critical |
| Custom Resources | CRDs | High |

### Backup Frequency

| Data Type | Frequency |
|-----------|-----------|
| etcd snapshots | Hourly |
| PV snapshots | Daily |
| Cluster resources | Hourly |

## Step-by-Step Examples

### Manual etcd Snapshot

```bash
# Create etcd snapshot
ETCDCTL_API=3 etcdctl snapshot save backup.db \
  --endpoints=https://127.0.0.1:2379 \
  --cacert=/etc/kubernetes/pki/etcd/ca.crt \
  --cert=/etc/kubernetes/pki/etcd/server.crt \
  --key=/etc/kubernetes/pki/etcd/server.key
```

### Restore etcd

```bash
# Stop kube-apiserver
sudo systemctl stop kube-apiserver

# Restore snapshot
ETCDCTL_API=3 etcdctl snapshot restore backup.db \
  --data-dir=/var/lib/etcd-restore

# Update etcd pod to use restored data
# Edit /etc/kubernetes/manifests/etcd.yaml
# Change --data-dir to /var/lib/etcd-restore

# Start etcd
sudo systemctl start etcd

# Start apiserver
sudo systemctl start kube-apiserver
```

### Volume Snapshot

```yaml
apiVersion: snapshot.storage.k8s.io/v1
kind: VolumeSnapshot
metadata:
  name: my-snapshot
spec:
  volumeSnapshotClassName: default
  source:
    persistentVolumeClaimName: my-pvc
```

### Schedule Regular Snapshots

```yaml
apiVersion: batch/v1
kind: CronJob
metadata:
  name: backup-cron
spec:
  schedule: "0 2 * * *"  # Daily at 2 AM
  jobTemplate:
    spec:
      template:
        spec:
          containers:
          - name: backup
            image: bitnami/kubectl:latest
            command:
            - /bin/sh
            - -c
            - |
              kubectl get pv -o yaml > /backup/pvs.yaml
              kubectl get all --all-namespaces -o yaml > /backup/resources.yaml
          restartPolicy: OnFailure
```

## Gotchas for Docker Users

- **No Docker equivalent**: Kubernetes backup is cluster-wide
- **etcd is key**: Cluster state is in etcd
- **Volume snapshots**: Require CSI driver

## Common Mistakes

- **No testing**: Restores never tested
- **Incomplete backups**: Missing critical data
- **Too infrequent**: Data loss window too large

## Quick Reference

| Backup Type | Tool |
|-------------|------|
| etcd | etcdctl |
| Resources | Velero |
| Volumes | VolumeSnapshots |

## What's Next

Continue to [Velero Backup Restore](./02-velero-backup-restore.md) for automated backup.
