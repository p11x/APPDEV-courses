# CSI Drivers

## Overview

The Container Storage Interface (CSI) is a standard for exposing arbitrary storage systems to containerized workloads on Kubernetes. CSI drivers replace the legacy in-tree storage drivers, providing a vendor-neutral interface for storage operations. Understanding CSI is essential for working with modern Kubernetes storage.

## Prerequisites

- Understanding of storage backends
- Familiarity with PersistentVolumes

## Core Concepts

### Why CSI?

Legacy in-tree drivers were:
- Part of Kubernetes core code
- Required Kubernetes updates to add new drivers
- Hard for vendors to maintain

CSI provides:
- Vendor-developed drivers (not in Kubernetes core)
- Standardized interface for all storage operations
- Easier driver development and maintenance

### CSI Driver Components

CSI drivers typically include:
- **Plugin**: Runs as DaemonSet (node plugin)
- **External Attacher**: Handles volume attach/detach
- **External Provisioner**: Handles dynamic provisioning
- **Plugin Registrar**: Registers driver with kubelet

### Driver Capabilities

| Capability | Description |
|------------|-------------|
| CREATE_DELETE_VOLUME | Dynamic provisioning |
| EXPAND_VOLUME | Volume expansion |
| SNAPSHOT | Volume snapshots |
| CLONE_VOLUME | Volume cloning |
| ATTACH_DETACH | Attach/detach management |

## Listing CSI Drivers

```bash
# List installed CSI drivers
kubectl get csidrivers

# Output example:
# NAME                  ATTACHREQUIRED   PODINFOONMOUNT   STORETYPE   CONTROLLERPLUGIN
# ebs.csi.aws.com       true            true             false       true

# List CSINodes (nodes with CSI drivers)
kubectl get csinodes

# Shows: driver name, node names
# NAME         DRIVERS
# node-1       ebs.csi.aws.com

# Get CSI driver details
kubectl describe csidriver ebs.csi.aws.com
```

## Common CSI Drivers

### Cloud Provider Drivers

| Driver | Provider | Use |
|--------|----------|-----|
| ebs.csi.aws.com | AWS EBS | AWS block storage |
| pd.csi.storage.gke.io | GCP PD | Google Cloud PD |
| disk.csi.azure.com | Azure Disk | Azure managed disks |

### Network Storage

| Driver | Storage |
|--------|---------|
| nfs.csi.k8s.io | NFS |
| cephfs.csi.ceph.com | CephFS |
| rbd.csi.ceph.com | Ceph RBD |

### Others

| Driver | Use |
|--------|-----|
| hostpath.csi.local | Local storage |
| iscsi.csi.open-e.com | iSCSI |

## Installing CSI Drivers

```bash
# Most drivers are installed via Helm or operator
# Example: AWS EBS CSI Driver
helm repo add aws-ebs-csi-driver https://kubernetes-sigs.github.io/aws-ebs-csi-driver
helm install aws-ebs-csi-driver aws-ebs-csi-driver/aws-ebs-csi-driver \
  --namespace kube-system

# Or via kubectl
kubectl apply -k "github.com/kubernetes-sigs/aws-ebs-csi-driver/deploy/kubernetes/overlays/stable/?ref=release-1.0"
```

## Verifying CSI Driver

```bash
# Check driver pods running
kubectl get pods -n kube-system -l app.kubernetes.io/name=ebs-csi-driver

# Check driver plugin on nodes
# Should see driver on each node
kubectl get csinodes -o jsonpath='{range .items[*]}{.metadata.name}: {.spec.drivers[*].name}{"\n"}{end}'

# Test driver by creating storage
kubectl apply -f storageclass.yaml
```

## Gotchas for Docker Users

- **Plugin-based**: Not built into Kubernetes like Docker volume plugins
- **Node-level**: Unlike Docker volumes, CSI works at cluster level with node components
- **Driver-specific features**: Not all drivers support all CSI features

## Quick Reference

| Command | Description |
|---------|-------------|
| `kubectl get csidrivers` | List CSI drivers |
| `kubectl get csinodes` | Nodes with CSI |
| `kubectl describe csidriver NAME` | Driver details |

| Capability | Feature |
|------------|---------|
| CREATE_DELETE_VOLUME | Provisioning |
| EXPAND_VOLUME | Resize |
| SNAPSHOT | Snapshots |
| CLONE_VOLUME | Cloning |

## What's Next

Continue to [Stateful App Example](./03-stateful-app-example.md) for a complete PostgreSQL deployment.
