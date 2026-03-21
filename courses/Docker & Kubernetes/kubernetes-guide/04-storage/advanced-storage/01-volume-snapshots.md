# Volume Snapshots

## Overview

Volume Snapshots provide point-in-time copies of PersistentVolumes, enabling backup and restore functionality for stateful applications. Like photography captures a moment, snapshots preserve the state of data at a specific time. They're essential for disaster recovery, testing, and data migration.

## Prerequisites

- Understanding of PVCs and PVs
- CSI driver with snapshot support
- VolumeSnapshot CRDs installed

## Core Concepts

### Snapshot Components

- **VolumeSnapshot**: User's request for a snapshot
- **VolumeSnapshotContent**: Actual snapshot resource in storage backend
- **VolumeSnapshotClass**: Parameters for snapshot creation

### How It Works

1. User creates VolumeSnapshot referencing a PVC
2. CSI driver creates snapshot of underlying storage
3. VolumeSnapshotContent created linking to the snapshot
4. User can restore to new PVC

### CSI Feature

Volume Snapshots are provided via the Container Storage Interface (CSI). Not all CSI drivers support snapshots - check compatibility.

## Step-by-Step Examples

### Check Snapshot CRDs

```bash
# Check if VolumeSnapshot CRDs exist
kubectl get crd | grep snapshot

# Output should show:
# volumesnapshotclasses.snapshot.storage.k8s.io
# volumesnapshotcontents.snapshot.storage.k8s.io
# volumesnapshots.snapshot.storage.k8s.io

# If not installed, install VolumeSnapshot CRDs
kubectl apply -f https://raw.githubusercontent.com/kubernetes-csi/external-snapshotter/master/client/config/crd/snapshot.storage.k8s.io_volumesnapshots.yaml
kubectl apply -f https://raw.githubusercontent.com/kubernetes-csi/external-snapshotter/master/client/config/crd/snapshot.storage.k8s.io_volumesnapshotcontents.yaml
```

### Create VolumeSnapshotClass

```yaml
apiVersion: snapshot.storage.k8s.io/v1
kind: VolumeSnapshotClass
metadata:
  name: snapshot-default
driver: ebs.csi.aws.com                   # Must match CSI driver for your storage
deletionPolicy: Delete                    # Delete snapshot when VolumeSnapshot deleted
parameters:
  # Driver-specific parameters
  encrypted: "true"
```

```bash
kubectl apply -f volumesnapshotclass.yaml
```

### Taking a Snapshot

```yaml
apiVersion: snapshot.storage.k8s.io/v1
kind: VolumeSnapshot
metadata:
  name: database-backup
  namespace: production
spec:
  volumeSnapshotClassName: snapshot-default
  source:
    persistentVolumeClaimName: database-pvc   # PVC to snapshot
```

```bash
# Apply snapshot
kubectl apply -f snapshot.yaml

# Check status
# READY should show True when complete
kubectl get volumesnapshot database-backup -n production

# Describe for details
# Shows: creation time, size, status
kubectl describe volumesnapshot database-backup -n production
```

### Restoring from Snapshot

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: database-pvc-restored
  namespace: production
spec:
  accessModes:
    - ReadWriteOnce
  dataSource:
    kind: VolumeSnapshot
    name: database-backup
    apiGroup: snapshot.storage.k8s.io
  resources:
    requests:
      storage: 50Gi          # Must be >= source snapshot size
  storageClassName: ebs-sc   # Same as original or compatible
```

```bash
# Create PVC from snapshot
kubectl apply -f restore-pvc.yaml

# Wait for binding
kubectl get pvc database-pvc-restored -n production -w
# STATUS: Pending → Bound

# Use restored PVC in pod
kubectl apply -f restored-pod.yaml
```

### Complete Backup/Restore Example

```yaml
# Full workflow YAML
---
# 1. Create snapshot
apiVersion: snapshot.storage.k8s.io/v1
kind: VolumeSnapshot
metadata:
  name: app-backup
spec:
  volumeSnapshotClassName: snapshot-default
  source:
    persistentVolumeClaimName: app-data
---
# 2. Restore to new PVC
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: app-data-restored
spec:
  accessModes:
    - ReadWriteOnce
  dataSource:
    kind: VolumeSnapshot
    name: app-backup
    apiGroup: snapshot.storage.k8s.io
  resources:
    requests:
      storage: 10Gi
  storageClassName: standard
---
# 3. Use restored PVC in new pod
apiVersion: v1
kind: Pod
metadata:
  name: app-restored
spec:
  containers:
  - name: app
    image: myapp:v1
    volumeMounts:
    - name: data
      mountPath: /data
  volumes:
  - name: data
    persistentVolumeClaim:
      claimName: app-data-restored
```

### Managing Snapshots

```bash
# List all snapshots in namespace
kubectl get volumesnapshot -n production

# Get snapshot content
kubectl get volumesnapshotcontent

# Check snapshot status
kubectl get volumesnapshot database-backup -n production \
  -o jsonpath='{.status}'

# Delete snapshot (if deletionPolicy: Delete, storage snapshot removed)
kubectl delete volumesnapshot database-backup -n production

# Keep snapshot (if deletionPolicy: Retain, storage snapshot remains)
```

## Snapshot Lifecycle

```
┌──────────┐    Create    ┌─────────────┐   Ready   ┌──────────┐
│  New     │────────────►│  Pending    │──────────►│  Ready   │
└──────────┘              └─────────────┘           └──────────┘
       │                        │                        │
       │                        │ Error                  │ Delete
       ▼                        ▼                        ▼
  Being Created           Creating              Deleting
```

## Gotchas for Docker Users

- **Point-in-time, not point-in-space**: Snapshot copies data at that moment, but doesn't clone to new location
- **Storage support required**: Not all storage backends support snapshots
- **CSI requirement**: Requires CSI driver with snapshot capability
- **Size considerations**: Snapshots store deltas, not full copies initially

## Common Mistakes

- **No CSI support**: Using driver without snapshot capability
- **Wrong storageClassName**: Restored PVC must use compatible storage class
- **Size too small**: Restored PVC must be >= snapshot size
- **Forgetting dataSource**: Restoring requires dataSource in PVC spec

## Quick Reference

| Resource | Purpose |
|----------|---------|
| VolumeSnapshot | Request for snapshot |
| VolumeSnapshotContent | Actual snapshot on storage |
| VolumeSnapshotClass | Snapshot parameters |

| Command | Description |
|---------|-------------|
| `kubectl get volumesnapshot` | List snapshots |
| `kubectl describe volumesnapshot` | Snapshot details |
| `kubectl get volumesnapshotcontent` | Backend snapshots |

## What's Next

Continue to [CSI Drivers](./02-csi-drivers.md) to understand the Container Storage Interface.
