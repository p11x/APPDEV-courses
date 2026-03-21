# PersistentVolumes and PersistentVolumeClaims

## Overview

PersistentVolumes (PV) and PersistentVolumeClaims (PVC) provide a way to store data beyond the lifetime of individual pods. While containers are ephemeral, PVs enable stateful applications to persist data. This separation allows administrators to provision storage while developers simply claim what they need.

## Prerequisites

- Understanding of Kubernetes pods
- Basic knowledge of storage concepts
- Familiarity with volumes in Docker

## Core Concepts

### The Problem with Ephemeral Storage

Container filesystems are temporary - when a container restarts, all data is lost. For databases, file stores, and stateful applications, you need persistent storage.

### PV vs PVC

- **PersistentVolume (PV)**: Cluster-wide storage resource provisioned by admin
- **PersistentVolumeClaim (PVC)**: Request for storage by a user

The separation allows:
- **Admin control**: PVs are managed separately from applications
- **Dynamic provisioning**: Storage can be created on-demand
- **Portability**: PVCs work regardless of underlying storage type

### Access Modes

| Mode | Abbreviation | Description |
|------|--------------|-------------|
| ReadWriteOnce | RWO | Single node read-write |
| ReadOnlyMany | ROX | Multiple nodes read-only |
| ReadWriteMany | RWX | Multiple nodes read-write |

Note: Not all storage backends support all modes.

### Reclaim Policy

| Policy | Behavior |
|--------|----------|
| Retain | Data preserved after PVC deleted |
| Delete | Storage is deleted when PVC deleted |
| Recycle | Data is scrubbed, PV becomes available again (deprecated) |

## Step-by-Step Examples

### Creating a PersistentVolume

```yaml
apiVersion: v1
kind: PersistentVolume
metadata:
  name: my-pv                       # Unique name across cluster
  labels:
    type: local
spec:
  capacity:
    storage: 10Gi                  # Size of the volume
  accessModes:
    - ReadWriteOnce               # Single node read-write
  reclaimPolicy: Retain           # Keep data after PVC deletion
  storageClassName: standard       # Optional: for dynamic provisioning
  hostPath:
    path: /mnt/data/myapp          # Path on the node (for local storage)
```

### Creating a PersistentVolumeClaim

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: my-pvc                     # Name used by pods
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 5Gi                # Can be less than or equal to PV
  storageClassName: standard      # Must match or use default
  selector:                       # Optional: select specific PV by label
    matchLabels:
      type: local
```

### Using PVC in a Pod

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: myapp-pod
spec:
  containers:
  - name: myapp
    image: nginx:1.25-alpine
    ports:
    - containerPort: 80
      name: http
    volumeMounts:
    - name: my-storage            # Must match volume name
      mountPath: /usr/share/nginx/html
  volumes:
  - name: my-storage
    persistentVolumeClaim:
      claimName: my-pvc           # Reference to the PVC
```

### Complete Example

```bash
# Create PV first
kubectl apply -f pv.yaml

# Verify PV is available
# STATUS should be Available
kubectl get pv my-pv

# Create PVC
kubectl apply -f pvc.yaml

# Check PVC binding status
# STATUS should be Bound to my-pv
kubectl get pvc my-pvc

# Shows: NAME     STATUS   VOLUME   CAPACITY   ACCESS MODES
#         my-pvc   Bound    my-pv    10Gi       RWO

# Create Pod using PVC
kubectl apply -f pod.yaml

# Verify pod is running with volume
kubectl describe pod myapp-pod | grep -A5 Volumes
```

### Static Provisioning Example

```yaml
# For cloud storage (e.g., AWS EBS) instead of hostPath
apiVersion: v1
kind: PersistentVolume
metadata:
  name: ebs-pv
spec:
  capacity:
    storage: 20Gi
  accessModes:
    - ReadWriteOnce
  awsElasticBlockStore:
    volumeId: vol-0abc123def456789a  # Actual AWS EBS volume ID
    fsType: ext4
  storageClassName: gp2
```

### Managing PVs and PVCs

```bash
# List all PVs
kubectl get pv -o wide

# Describe PV for details
# Shows: capacity, access modes, reclaim policy, status
kubectl describe pv my-pv

# List PVCs in namespace
kubectl get pvc

# Check PVC status
kubectl get pvc my-pvc -o jsonpath='{.status.phase}'

# Check what PV a PVC is bound to
kubectl get pvc my-pvc -o jsonpath='{.spec.volumeName}'

# Release a PVC (retain data)
kubectl delete pvc my-pc

# Delete PV after data is backed up
kubectl delete pv my-pv

# Manually reclaim a released PV
kubectl patch pv my-pv -p '{"spec":{"claimRef":null}}'
```

## Binding Lifecycle

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Pending  │────►│   Bound     │────►│  Released  │
└─────────────┘     └─────────────┘     └─────────────┘
      │                   │                   │
      │ PVC created       │ PVC deleted       │ PV deleted
      │ no matching PV    │ reclaim=Retain   │
```

## Gotchas for Docker Users

- **Separation of concerns**: Unlike Docker volumes bound to containers, Kubernetes separates provisioning (PV) from consumption (PVC)
- **Namespace isolation**: PVCs are namespace-scoped, PVs are cluster-scoped
- **Binding is one-way**: Once bound, PVC is tied to that specific PV
- **Access modes matter**: Not all storage supports all modes - RWX requires special storage
- **Cleanup responsibility**: Admin must clean up data when PV is deleted

## Common Mistakes

- **Wrong storage class**: PVC and PV storageClassName must match
- **Capacity too large**: PVC request can't exceed PV capacity
- **Access mode mismatch**: PV must support the access mode requested by PVC
- **Forgetting reclaim policy**: Data loss if Delete policy and PVC deleted
- **Not waiting for binding**: Creating pod before PVC is bound fails

## Quick Reference

| Command | Description |
|---------|-------------|
| `kubectl get pv` | List PersistentVolumes |
| `kubectl get pvc` | List PersistentVolumeClaims |
| `kubectl describe pv NAME` | PV details |
| `kubectl describe pvc NAME` | PVC details |

| PV Field | Purpose |
|----------|---------|
| capacity.storage | Size |
| accessModes | How the volume can be mounted |
| reclaimPolicy | Retain/Delete |
| storageClassName | Class for dynamic provisioning |

| PVC Field | Purpose |
|-----------|---------|
| resources.requests.storage | Size requested |
| accessModes | Must match PV |
| storageClassName | Links to StorageClass |

## What's Next

Continue to [StorageClasses](./02-storage-classes.md) to learn about dynamic storage provisioning.
