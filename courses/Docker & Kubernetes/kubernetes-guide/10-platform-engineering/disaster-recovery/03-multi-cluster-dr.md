# Multi-Cluster Disaster Recovery

## Overview

Multi-cluster disaster recovery uses multiple Kubernetes clusters to ensure high availability. If one cluster fails, workloads fail over to another cluster.

## Prerequisites

- Multiple Kubernetes clusters
- Network connectivity between clusters

## Core Concepts

### DR Patterns

| Pattern | Description | RTO |
|---------|-------------|-----|
| Active-Passive | Standby cluster | Minutes |
| Active-Active | Multiple active | Near zero |
| Pilot | Subset failover | Minutes |

### Key Components

- **Cluster federation**: Manage multiple clusters
- **Service mesh**: Cross-cluster networking
- **Global DNS**: Traffic routing

## Step-by-Step Examples

### Sync Backups to Secondary Cluster

```yaml
# Velero schedule with cross-cluster sync
apiVersion: velero.io/v1
kind: Schedule
metadata:
  name: dr-backup
spec:
  schedule: "0 * * * *"  # Hourly
  template:
    includedNamespaces:
    - production
    storageLocation: default
    volumeSnapshotLocations:
    - default
---
# BackupStorageLocation to secondary cluster
apiVersion: velero.io/v1
kind: BackupStorageLocation
metadata:
  name: secondary
spec:
  provider: aws
  objectStorage:
    bucket: dr-backups
  config:
    region: us-west-2
    prefix: secondary-cluster
```

### Cross-Cluster Service Discovery

```yaml
# Federation control plane
apiVersion: core.k8s.io/v1alpha1
kind: KubeFedConfig
metadata:
  name: kubefed
  namespace: kube-federation-system
spec:
  scope: Namespaced
  featureGates:
  - name: PushReconciler
    configuration: Enabled
  - name: SchedulerPreferences
    configuration: Enabled
```

### DNS-Based Failover

```yaml
# ExternalDNS for cross-cluster
apiVersion: externaldns.k8s.io/v1alpha1
kind: DNSEndpoint
metadata:
  name: myapp-global
spec:
  endpoints:
  - dnsName: myapp.example.com
    recordTTL: 300
    recordType: A
    targets:
    - 10.0.0.1  # Primary cluster IP
    - 10.0.0.2  # Secondary cluster IP
```

### GitOps-Based Recovery

```yaml
# ArgoCD application for DR
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: production-dr
  namespace: argocd
spec:
  destination:
    server: https://secondary-cluster.example.com
    namespace: production
  source:
    repoURL: https://github.com/org/manifests
    path: production
    targetRevision: HEAD
  syncPolicy:
    automated:
      prune: true
      selfHeal: true
```

## Gotchas for Docker Users

- **No Docker equivalent**: Multi-cluster is Kubernetes-specific
- **Network latency**: Cross-cluster traffic adds latency
- **Data sync**: Persistent data needs replication

## Common Mistakes

- **No testing**: DR never tested
- **Split brain**: Data inconsistency
- **Single point**: DNS or network failure

## Quick Reference

| DR Pattern | Complexity | Cost |
|------------|------------|------|
| Active-Passive | Low | Medium |
| Active-Active | High | High |
| Pilot | Medium | Medium |

## What's Next

The Kubernetes guide is complete! Check out the final status summary.
