# StorageClasses

## Overview

StorageClasses enable dynamic provisioning of PersistentVolumes, eliminating the need for administrators to manually create PVs for each PVC. When a PVC requests storage, the StorageClass's provisioner automatically creates the appropriate storage resource in the underlying infrastructure (cloud provider, network storage system, or local storage).

## Prerequisites

- Understanding of PersistentVolumes and PVCs
- Knowledge of your cluster's storage backend
- Familiarity with Kubernetes storage concepts

## Core Concepts

### What is a StorageClass?

A StorageClass defines:
- **Provisioner**: The plugin that creates the storage
- **Parameters**: Backend-specific configuration
- **Reclaim Policy**: What happens when PVC is deleted
- **Volume Binding Mode**: When binding occurs

### Components

- **Provisioner**: External plugin (AWS EBS, GCE PD, Ceph, NFS, etc.)
- **Parameters**: Backend-specific settings (size, IOPS, type)
- **Annotations**: Additional metadata

### Volume Binding Modes

| Mode | Behavior |
|------|----------|
| Immediate | PVC bound to PV as soon as created |
| WaitForFirstConsumer | Binding delayed until pod using PVC is scheduled |

WaitForFirstConsumer is recommended as it ensures PV is created in the same zone where the pod will run.

## Step-by-Step Examples

### Creating a StorageClass

```yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: fast-storage                  # Unique name in cluster
  annotations:
    storageclass.kubernetes.io/is-default-class: "false"   # Not default
provisioner: kubernetes.io/gce-pd      # GCE Persistent Disk provisioner
parameters:
  type: pd-ssd                       # SSD-backed disk
  replication-type: regional-pd       # Multi-zone replication
reclaimPolicy: Delete                # Delete when PVC is deleted
volumeBindingMode: WaitForFirstConsumer  # Bind when pod scheduled
allowVolumeExpansion: true           # Allow PVC size increase
```

### Default StorageClass

```yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: standard
  annotations:
    storageclass.kubernetes.io/is-default-class: "true"   # Default class
provisioner: kubernetes.io/gce-pd
parameters:
  type: pd-standard                  # Standard HDD disk
reclaimPolicy: Delete
volumeBindingMode: Immediate
```

### Using StorageClass in PVC

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: my-database-pvc
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 50Gi
  storageClassName: fast-storage    # Must match StorageClass name
```

### Cloud Provider Examples

#### AWS EBS

```yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: ebs-sc
provisioner: ebs.csi.aws.com         # Use CSI driver (recommended)
parameters:
  type: gp3                         # General Purpose SSD
  iops: "3000"
  throughput: "125"
  encrypted: "true"
reclaimPolicy: Delete
volumeBindingMode: WaitForFirstConsumer
allowVolumeExpansion: true
```

#### Azure Disk

```yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: azure-disk
provisioner: disk.csi.azure.com
parameters:
  storageAccountType: Premium_LRS   # Premium SSD
  kind: Managed                     # Managed disks
reclaimPolicy: Delete
volumeBindingMode: WaitForFirstConsumer
```

#### NFS

```yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: nfs-storage
provisioner: nfs.csi.k8s.io         # NFS CSI provisioner
parameters:
  server: nfs-server.example.com     # NFS server address
  share: /exports/shared             # NFS export path
reclaimPolicy: Retain
volumeBindingMode: Immediate
```

### Managing StorageClasses

```bash
# List StorageClasses
# Shows provisioner and default status
kubectl get storageclass

# Output:
# NAME                 PROVISIONER              RECLAIMPOLICY   VOLUMEBINDINGMODE
# standard             kubernetes.io/gce-pd     Delete          Immediate
# fast-storage         kubernetes.io/gce-pd     Delete          WaitForFirstConsumer
# ebs-sc               ebs.csi.aws.com         Delete          WaitForFirstConsumer

# Describe StorageClass for details
# Shows: provisioner, parameters, mount options
kubectl describe storageclass fast-storage

# Set default StorageClass
kubectl annotate storageclass standard \
  storageclass.kubernetes.io/is-default-class=true

# Remove default
kubectl annotate storageclass standard \
  storageclass.kubernetes.io/is-default-class-

# Verify default is set
kubectl get storageclass -o jsonpath='{.items[?(@.metadata.annotations.storageclass\.kubernetes\.io/is-default-class=="true")].metadata.name}'
```

### Checking Dynamic Provisioning

```bash
# Create PVC with StorageClass
kubectl apply -f pvc.yaml

# Watch for PV creation
# When volumeBindingMode=WaitForFirstConsumer,
# PV is created only when pod is scheduled
kubectl get pv -w

# Check PVC status
kubectl get pvc my-database-pvc
# Shows: STATUS = Bound, VOLUME = pvc-abc123

# Check the dynamically created PV
kubectl get pv pvc-abc123 -o yaml
# Shows: storageClassName: fast-storage, size: 50Gi
```

## Default StorageClass Behavior

Without explicit storageClassName:
1. If default StorageClass exists → uses it
2. If no default → requires explicit storageClassName
3. Empty string ("") → no StorageClass (use static PVs)

## Gotchas for Docker Users

- **Not manual provisioning**: Unlike Docker volumes, storage is created automatically
- **Backend-specific**: Parameters vary by cloud provider/storage system
- **CSI vs in-tree**: Modern clusters use CSI drivers, not built-in provisioners
- **Binding delay**: WaitForFirstConsumer adds latency but ensures better placement
- **Expansion requires allowVolumeExpansion**: Must be true in StorageClass

## Common Mistakes

- **Wrong provisioner**: Using wrong provisioner for your storage backend
- **Case sensitivity**: StorageClass names are case-sensitive in PVC
- **Missing CSI driver**: CSI driver must be installed before using
- **Not setting default**: Without default, all PVCs need explicit storageClassName
- **Zone constraints**: Some provisioners only work in specific zones/regions

## Quick Reference

| Field | Description |
|-------|-------------|
| provisioner | Storage plugin (aws EBS, gce-pd, etc.) |
| parameters | Backend-specific settings |
| reclaimPolicy | Delete/Retain |
| volumeBindingMode | Immediate/WaitForFirstConsumer |
| allowVolumeExpansion | true/false |

| Common Provisioners | Storage Backend |
|--------------------|-----------------|
| kubernetes.io/gce-pd | Google Cloud PD |
| kubernetes.io/aws-ebs | AWS EBS |
| kubernetes.io/azure-disk | Azure Disk |
| ebs.csi.aws.com | AWS EBS (CSI) |
| disk.csi.azure.com | Azure Disk (CSI) |
| pd.csi.storage.gke.io | GCE PD (CSI) |
| nfs.csi.k8s.io | NFS |

## What's Next

Continue to [Dynamic Provisioning](./03-dynamic-provisioning.md) to see complete end-to-end dynamic provisioning workflow.
