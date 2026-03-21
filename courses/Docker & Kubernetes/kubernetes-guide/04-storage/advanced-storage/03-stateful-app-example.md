# Stateful App Example

## Overview

This guide walks through deploying a complete PostgreSQL database on Kubernetes using StatefulSet, demonstrating persistent storage, headless services, and proper security configuration. This is a practical example of running a stateful workload in production.

## Prerequisites

- Kubernetes cluster with PVC support
- kubectl configured
- Understanding of StatefulSet, PVC, Services

## Step-by-Step Example

### Step 1: Create Namespace

```bash
kubectl create namespace production
```

### Step 2: Create StorageClass

```yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: postgres-storage
provisioner: kubernetes.io/gce-pd  # Use your cluster's provisioner
reclaimPolicy: Retain
volumeBindingMode: WaitForFirstConsumer
```

```bash
kubectl apply -f storageclass.yaml
```

### Step 3: Create Secret for Password

```bash
# Create secret from literal
kubectl create secret generic postgres-secret \
  --from-literal=password=SecurePassword123! \
  -n production
```

```bash
# Verify secret
kubectl get secret postgres-secret -n production
```

### Step 4: Create Headless Service

```yaml
apiVersion: v1
kind: Service
metadata:
  name: postgres-headless
  namespace: production
spec:
  clusterIP: None
  selector:
    app: postgres
  ports:
  - name: postgres
    port: 5432
    targetPort: 5432
```

```bash
kubectl apply -f service.yaml
```

### Step 5: Create StatefulSet

```yaml
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: postgres
  namespace: production
spec:
  serviceName: postgres-headless
  replicas: 1
  selector:
    matchLabels:
      app: postgres
  template:
    metadata:
      labels:
        app: postgres
    spec:
      containers:
      - name: postgres
        image: postgres:16-alpine
        ports:
        - containerPort: 5432
          name: postgres
        env:
        - name: POSTGRES_PASSWORD
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: password
        - name: POSTGRES_DB
          value: "mydb"
        volumeMounts:
        - name: postgres-data
          mountPath: /var/lib/postgresql/data
  volumeClaimTemplates:
  - metadata:
      name: postgres-data
    spec:
      accessModes: ["ReadWriteOnce"]
      storageClassName: postgres-storage
      resources:
        requests:
          storage: 10Gi
```

```bash
kubectl apply -f statefulset.yaml

# Watch pods
kubectl get pods -n production -w
# Wait for postgres-0 to be Running
```

### Step 6: Verify Deployment

```bash
# Check StatefulSet status
kubectl get statefulset postgres -n production

# Check PVC
kubectl get pvc -n production
# Shows: postgres-data-postgres-0   Bound   10Gi

# Check service
kubectl get svc postgres-headless -n production
```

### Step 7: Connect to PostgreSQL

```bash
# Exec into the pod
kubectl exec -it postgres-0 -n production -- /bin/bash

# Connect to PostgreSQL
psql -U postgres -d mydb

# Inside psql:
# CREATE TABLE test (id SERIAL PRIMARY KEY, name TEXT);
# INSERT INTO test (name) VALUES ('hello');
# SELECT * FROM test;
# \q
# exit
```

### Step 8: Connect from Another Pod

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: postgres-client
  namespace: production
spec:
  containers:
  - name: postgres-client
    image: postgres:16-alpine
    command: ["sleep", "infinity"]
```

```bash
kubectl apply -f client.yaml

# Connect using DNS name
# Full DNS: postgres-headless.production.svc.cluster.local
kubectl exec -it postgres-client -n production -- \
  psql -h postgres-headless.production.svc.cluster.local -U postgres -d mydb
```

## Complete YAML Files

```yaml
# postgres-complete.yaml - All-in-one file
---
apiVersion: v1
kind: Secret
metadata:
  name: postgres-secret
  namespace: production
type: Opaque
stringData:
  password: SecurePassword123!
---
apiVersion: v1
kind: Service
metadata:
  name: postgres-headless
  namespace: production
spec:
  clusterIP: None
  selector:
    app: postgres
  ports:
  - name: postgres
    port: 5432
---
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: postgres
  namespace: production
spec:
  serviceName: postgres-headless
  replicas: 1
  selector:
    matchLabels:
      app: postgres
  template:
    metadata:
      labels:
        app: postgres
    spec:
      containers:
      - name: postgres
        image: postgres:16-alpine
        ports:
        - containerPort: 5432
          name: postgres
        env:
        - name: POSTGRES_PASSWORD
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: password
        - name: POSTGRES_DB
          value: "mydb"
        volumeMounts:
        - name: postgres-data
          mountPath: /var/lib/postgresql/data
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        readinessProbe:
          exec:
            command: ["pg_isready", "-U", "postgres"]
          initialDelaySeconds: 10
          periodSeconds: 5
        livenessProbe:
          exec:
            command: ["pg_isready", "-U", "postgres"]
          initialDelaySeconds: 30
          periodSeconds: 10
  volumeClaimTemplates:
  - metadata:
      name: postgres-data
    spec:
      accessModes: ["ReadWriteOnce"]
      storageClassName: postgres-storage
      resources:
        requests:
          storage: 10Gi
```

## Management Commands

```bash
# Scale StatefulSet
kubectl scale statefulset postgres --replicas=2 -n production

# Delete pod (StatefulSet recreates it)
kubectl delete pod postgres-0 -n production

# Update image
kubectl set image statefulset/postgres postgres=postgres:16 -n production

# Check rollout status
kubectl rollout status statefulset/postgres -n production

# Get PVC
kubectl get pvc -l app=postgres -n production
```

## Cleanup

```bash
# Delete StatefulSet (PVC remains with Retain policy)
kubectl delete statefulset postgres -n production

# Delete PVC manually
kubectl delete pvc postgres-data-postgres-0 -n production

# Delete Service
kubectl delete service postgres-headless -n production

# Delete Secret
kubectl delete secret postgres-secret -n production
```

## Gotchas for Docker Users

- **Pod identity**: Unlike Docker Compose, PostgreSQL pods have stable names (postgres-0)
- **Storage persistence**: Data survives pod restarts unlike ephemeral Docker volumes
- **Connection string**: Uses DNS service name not localhost
- **Secrets**: Securely pass passwords via Secrets not environment variables

## Quick Reference

| Component | Purpose |
|-----------|---------|
| StatefulSet | Manages PostgreSQL pods |
| Headless Service | DNS for pod discovery |
| PVC | Persistent storage |
| Secret | Database password |
| StorageClass | Storage provisioning |

| Command | Description |
|---------|-------------|
| `kubectl exec -it postgres-0 -n production -- psql` | Connect to database |
| `kubectl scale statefulset postgres --replicas=2` | Scale |
| `kubectl delete pvc NAME` | Delete storage |

## What's Next

Continue to [Manual Scaling](../scaling/01-manual-scaling.md) for scaling workloads.
