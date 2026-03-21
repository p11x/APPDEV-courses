# StatefulSets

## Overview

StatefulSets are Kubernetes resources designed for stateful applications that require stable network identities, stable storage, and ordered deployment/scaling. Unlike Deployments which treat pods as interchangeable, StatefulSets maintain a persistent identity for each pod, making them ideal for databases, message queues, and other applications that need stable hostnames and persistent storage.

## Prerequisites

- Understanding of Kubernetes Pods and Deployments
- Basic knowledge of persistent storage concepts
- Familiarity with Services and networking

## Core Concepts

### What Makes StatefulSets Different

Unlike Deployments, StatefulSets provide:

1. **Stable Network Identity**: Each pod gets a deterministic name (e.g., `web-0`, `web-1`, `web-2`) that persists across rescheduling
2. **Stable Storage**: PersistentVolumeClaims are tied to each pod identity, preserving data across rescheduling
3. **Ordered Deployment and Scaling**: Pods are created, scaled, and deleted in a predictable order (0 to N-1)
4. **Ordered Rolling Updates**: Updates proceed in reverse ordinal order

### Headless Services Required

StatefulSets require a headless Service to control the network identity of the pods. The headless service provides DNS entries for each pod:

```
web-0.web-service.default.svc.cluster.local
web-1.web-service.default.svc.cluster.local
web-2.web-service.default.svc.cluster.local
```

### When to Use StatefulSets

- Databases (PostgreSQL, MySQL, MongoDB)
- Message queues (Kafka, RabbitMQ)
- Distributed storage systems (etcd, Cassandra)
- Any application requiring stable network identity and persistent storage

## Step-by-Step Examples

### Creating a StatefulSet

```yaml
apiVersion: apps/v1                        # API group for StatefulSet (apps)
kind: StatefulSet                          # Resource type for stateful workloads
metadata:
  name: web                                # Unique name within namespace
spec:
  serviceName: web-service                 # Must match headless Service name
  replicas: 3                              # Number of pods to maintain
  minReadySeconds: 10                     # New in k8s 1.30 - minimum seconds before pod is ready
  selector:
    matchLabels:
      app: web                             # Must match pod template labels
  template:
    metadata:
      labels:
        app: web                           # Pod labels
    spec:
      containers:
      - name: nginx
        image: nginx:1.25-alpine
        ports:
        - containerPort: 80
          name: http
        volumeMounts:
        - name: www
          mountPath: /usr/share/nginx/html
  volumeClaimTemplates:                    # Templates for per-pod PVCs
  - metadata:
      name: www                            # Name referenced in volumeMounts
    spec:
      accessModes: ["ReadWriteOnce"]      # RWO: single node read-write
      storageClassName: standard           # StorageClass for dynamic provisioning
      resources:
        requests:
          storage: 1Gi                    # Size of each PVC
---
apiVersion: v1
kind: Service
metadata:
  name: web-service                       # Must match StatefulSet serviceName
spec:
  clusterIP: None                        # Headless service - no cluster IP
  selector:
    app: web
  ports:
  - port: 80
    targetPort: 80
```

### Managing StatefulSets

```bash
# Create the StatefulSet and Service
kubectl apply -f statefulset.yaml

# Get StatefulSet status
# Shows ready replicas, current version, and conditions
kubectl get statefulset web -o wide

# Describe for detailed information
# Includes: service name, selectors, volume claims, update strategy
kubectl describe statefulset web

# Scale the StatefulSet
# Pods are added in order (web-0, web-1, etc.)
kubectl scale statefulset web --replicas=5

# Delete with graceful termination
# Pods terminated in reverse order (web-4, web-3, etc.)
kubectl delete statefulset web

# View pods (note stable naming)
kubectl get pods -l app=web
# Output: web-0, web-1, web-2 (stable names)

# Connect to specific pod
kubectl exec -it web-0 -- /bin/sh
```

### Updating a StatefulSet

```yaml
# Update strategy in spec
spec:
  updateStrategy:
    type: RollingUpdate                   # Default: update in reverse order
    rollingUpdate:
      partition: 2                        # Only update pods with ordinal >= 2
```

```bash
# Check rollout status
# StatefulSet rollouts are tracked via .status.updateRevision
kubectl rollout status statefulset/web

# Rollback to previous revision
kubectl rollout undo statefulset/web
```

## Gotchas for Docker Users

- **No interchangeable pods**: Unlike Docker containers, StatefulSet pods have persistent identities - you can't just restart any pod
- **Storage persistence**: Data survives pod restarts due to PVCs, unlike ephemeral Docker container filesystem
- **Ordered operations**: Scaling and updates happen sequentially, unlike parallel Docker Compose scaling
- **Manual cleanup**: Deleting the StatefulSet doesn't delete PVCs - you must delete them manually
- **No automatic scaling**: StatefulSets don't work well with HPA due to stable identities requirement

## Common Mistakes

- **Forgetting headless Service**: StatefulSet will not work without a corresponding headless Service
- **Using wrong storage**: Not specifying storageClassName can result in wrong storage type
- **Deleting pods manually**: Never delete individual StatefulSet pods directly - let the controller manage them
- **Not understanding ordinals**: Pod names include ordinal numbers that persist across updates
- **Ignoring PVC cleanup**: Orphaned PVCs consume storage and must be manually cleaned up

## Quick Reference

| Field | Description |
|-------|-------------|
| serviceName | Name of headless Service |
| replicas | Number of pods |
| volumeClaimTemplates | PVC templates for each pod |
| minReadySeconds | Seconds before pod is ready (k8s 1.30+) |
| ordinals | start and template for pod naming |

| Command | Description |
|---------|-------------|
| `kubectl get statefulset` | List StatefulSets |
| `kubectl describe statefulset` | Detailed info |
| `kubectl scale statefulset --replicas=N` | Scale |
| `kubectl delete statefulset` | Delete (PVCs remain) |

## What's Next

Continue to [DaemonSets](./02-daemonsets.md) to learn about running one pod per node.
