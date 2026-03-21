# Dynamic Provisioning

## Overview

Dynamic provisioning automates the entire storage lifecycle - when a user creates a PersistentVolumeClaim, the StorageClass provisioner automatically creates the underlying storage resource in the cloud provider or storage system. This eliminates manual PV creation and ensures storage is available exactly when needed.

## Prerequisites

- Understanding of StorageClasses
- Knowledge of PVC and PV concepts
- CSI driver installed (for most provisioners)

## Core Concepts

### How Dynamic Provisioning Works

1. User creates PVC with storageClassName
2. Kubernetes finds StorageClass matching the name
3. Provisioner (CSI driver) creates storage in backend
4. Provisioner creates PV pointing to the new storage
5. PV is bound to the PVC
6. User mounts PVC in pod

### Benefits

- **No manual PV creation**: Storage created on-demand
- **Efficient**: Only creates what's needed
- **Portable**: Same YAML works across clusters
- **Self-service**: Developers don't need admin access

### VolumeBindingMode Impact

| Mode | When Bound | Use Case |
|------|-----------|----------|
| Immediate | When PVC created | Fast binding, may cause zone issues |
| WaitForFirstConsumer | When pod scheduled | Better placement, slightly slower |

## Step-by-Step Examples

### End-to-End Example

#### Step 1: Verify StorageClass Exists

```bash
# Check available StorageClasses
kubectl get storageclass

# Output:
# NAME                 PROVISIONER              RECLAIMPOLICY   VOLUMEBINDINGMODE
# standard             kubernetes.io/gce-pd     Delete          Immediate
# fast                 ebs.csi.aws.com         Delete          WaitForFirstConsumer

# If no StorageClass exists, you may need to install one
# Example: Install CSI driver for AWS EBS
kubectl apply -k "github.com/kubernetes-sigs/aws-ebs-csi-driver/deploy/kubernetes/overlays/stable/?ref=release-1.0"
```

#### Step 2: Create PVC

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: app-data-pvc
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 10Gi
  storageClassName: fast             # Must match StorageClass
```

```bash
# Apply PVC
kubectl apply -f pvc.yaml

# Immediately check status
# With Immediate binding: may already be Bound
# With WaitForFirstConsumer: will be Pending until pod scheduled
kubectl get pvc app-data-pvc -w
```

#### Step 3: Create Pod Using PVC

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: app-pod
spec:
  containers:
  - name: app
    image: nginx:1.25-alpine
    volumeMounts:
    - name: data
      mountPath: /usr/share/nginx/html
  volumes:
  - name: data
    persistentVolumeClaim:
      claimName: app-data-pvc
```

```bash
# Apply pod
kubectl apply -f pod.yaml

# Now check PVC status
# Should transition from Pending to Bound
kubectl get pvc app-data-pvc

# Check the auto-created PV
kubectl get pv
# New PV with name like pvc-abc123-def456 created
```

#### Step 4: Verify Complete Setup

```bash
# Verify PV details
kubectl get pv -o wide
# Shows: CAPACITY, ACCESS MODES, RECLAIM POLICY, STORAGECLASS

# Describe PV for full details
kubectl describe pv pvc-abc123-def456
# Shows: claim, provisioner, creation time

# Verify pod has volume
kubectl describe pod app-pod | grep -A10 Volumes
# Shows: Volume Type, PVC Name, Mount Path
```

### Volume Expansion Example

With allowVolumeExpansion: true in StorageClass:

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: expandable-pvc
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 10Gi
  storageClassName: fast
```

```bash
# After creating PVC and pod, expand capacity
kubectl patch pvc expandable-pvc -p '{"spec":{"resources":{"requests":{"storage":"20Gi"}}}}'

# Check status
# Kubernetes updates PV and storage backend
kubectl get pvc expandable-pvc
# Shows: 20Gi

# Note: Some storage backends require pod restart
```

### Complete StorageClass + PVC + Pod Example

```yaml
# storageclass.yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: ssd-storage
provisioner: ebs.csi.aws.com
parameters:
  type: gp3
  iops: "3000"
reclaimPolicy: Delete
volumeBindingMode: WaitForFirstConsumer
allowVolumeExpansion: true
---
# pvc.yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: database-pvc
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 50Gi
  storageClassName: ssd-storage
---
# pod.yaml
apiVersion: v1
kind: Pod
metadata:
  name: database
spec:
  containers:
  - name: database
    image: postgres:16-alpine
    env:
    - name: POSTGRES_PASSWORD
      valueFrom:
        secretKeyRef:
          name: db-secret
          key: password
    volumeMounts:
    - name: db-data
      mountPath: /var/lib/postgresql/data
  volumes:
  - name: db-data
    persistentVolumeClaim:
      claimName: database-pvc
```

```bash
# Apply all at once
kubectl apply -f storageclass.yaml -f pvc.yaml -f pod.yaml

# Verify everything
kubectl get pvc,pv,pod | grep database
```

## Managing Dynamic Volumes

```bash
# Check PVC binding events
# Shows provisioner logs and any errors
kubectl describe pvc app-data-pvc

# Check provisioner logs
# Useful for debugging provisioning issues
kubectl logs -n kube-system -l app=ebs-csi-driver

# Delete in order: Pod → PVC → (PV if reclaim=Delete)
kubectl delete pod app-pod
kubectl delete pvc app-data-pvc

# With reclaimPolicy: Delete, PV is automatically deleted
# With reclaimPolicy: Retain, PV remains with data
```

## Gotchas for Docker Users

- **Automatic creation**: Unlike Docker where you create volumes first, PVC triggers creation
- **Cloud-specific parameters**: Each provider has different tuning options
- **Not instant**: Provisioning takes seconds to minutes depending on backend
- **CSI driver required**: Most dynamic provisioning requires CSI driver installation
- **Cost implications**: Dynamic volumes incur cloud provider costs immediately

## Common Mistakes

- **No StorageClass**: Dynamic provisioning fails without proper StorageClass
- **Wrong provisioner**: Using wrong provisioner for your cloud provider
- **Deleting in wrong order**: Delete pod first, then PVC
- **Forgetting reclaim policy**: Data loss if Delete policy
- **Zone mismatches**: Pod and PVC must be in same zone for some backends

## Quick Reference

| Provisioning Step | Command |
|------------------|---------|
| Create StorageClass | kubectl apply -f storageclass.yaml |
| Create PVC | kubectl apply -f pvc.yaml |
| Check binding | kubectl get pvc,pv |
| Verify pod mount | kubectl describe pod |

| Troubleshooting | Command |
|-----------------|---------|
| Check PVC events | kubectl describe pvc |
| Check provisioner | kubectl logs -n kube-system -l app=PROVISIONER |
| Check PV | kubectl describe pv |

## What's Next

Continue to [Volume Snapshots](./../../advanced-storage/01-volume-snapshots.md) to learn about backing up persistent volumes.
