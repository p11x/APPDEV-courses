# Kubernetes Setup for Node.js

## What You'll Learn

- How to set up a local Kubernetes environment
- How to install and configure kubectl
- How to use Minikube or kind for local development
- How to verify your Kubernetes installation

## Layer 1: Academic Foundation

### Container Orchestration Fundamentals

Kubernetes (K8s) is an open-source container orchestration platform that automates deployment, scaling, and management of containerized applications. It was originally designed by Google based on their internal Borg system and now maintained by the Cloud Native Computing Foundation (CNCF).

The core architectural components include:

- **Control Plane**: Manages the cluster state and scheduling
- **Worker Nodes**: Run containerized workloads
- **etcd**: Distributed key-value store for cluster state
- **kube-apiserver**: REST API for cluster communication
- **kube-controller-manager**: Runs controller loops
- **kube-scheduler**: Assigns pods to nodes
- **kubelet**: Node agent that manages containers

### Mathematical Foundations

Container scheduling involves optimization problems. The scheduler uses a scoring algorithm to rank nodes based on resources:

```
Score(node) = (cpuScore * cpuWeight) + (memoryScore * memoryWeight) + (diskScore * diskWeight)
```

Where each resource score is calculated as:

```
resourceScore = (capacity - requested) / capacity * 100
```

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Local Development with kind

kind (Kubernetes IN Docker) creates Kubernetes clusters using Docker containers as nodes:

```bash
# Install kind
curl -Lo ./kind https://kind.sigs.k8s.io/dl/v0.20.0/kind-linux-amd64
chmod +x ./kind
sudo mv ./kind /usr/local/bin/kind

# Create a cluster
kind create cluster --name node-app

# Verify cluster
kubectl cluster-info
kubectl get nodes
```

### Paradigm 2 — Minikube Setup

Minikube runs a single-node Kubernetes cluster locally:

```bash
# Install minikube
curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
sudo install minikube-linux-amd64 /usr/local/bin/minikube

# Start cluster
minikube start --driver=docker

# Enable addons
minikube addons enable ingress
minikube addons enable dashboard
```

### Paradigm 3 — Production-Grade with k3s

k3s is a lightweight Kubernetes distribution ideal for edge and resource-constrained environments:

```bash
# Install k3s
curl -sfL https://get.k3s.io | sh -

# Verify installation
kubectl get nodes
sudo k3s server &
```

### Paradigm 4 — Cloud-Based Clusters

For cloud-managed Kubernetes:

```bash
# EKS (AWS)
eksctl create cluster --name node-cluster --node-type t3.medium

# GKE (Google Cloud)
gcloud container clusters create node-cluster --zone us-central1-a

# AKS (Azure)
az aks create --resource-group myResourceGroup --name node-cluster
```

---

## Layer 3: Performance Engineering Lab

### Benchmark Suite

| Cluster Type | CPU | Memory | Startup Time | Max Pods |
|--------------|-----|--------|--------------|----------|
| kind | 2 cores | 4GB | ~30s | 100 |
| Minikube | 2 cores | 4GB | ~60s | 110 |
| k3s | 1 core | 1GB | ~45s | 50 |
| EKS | Variable | Variable | ~5min | 100+ |

### Resource Planning

```yaml
# resource-quota.yaml
apiVersion: v1
kind: ResourceQuota
metadata:
  name: node-app-quota
spec:
  hard:
    requests.cpu: "4"
    requests.memory: 8Gi
    limits.cpu: "8"
    limits.memory: 16Gi
    pods: "50"
    services: "10"
```

---

## Layer 4: Zero-Trust Security Architecture

### Threat Model (STRIDE)

| Threat | Description | Mitigation |
|--------|-------------|------------|
| Spoofing | Impersonating cluster components | TLS between components, node identity |
| Tampering | Modifying cluster state | RBAC, pod security policies |
| Repudiation | No audit trail | Audit logging enabled |
| Information Disclosure | Exposing sensitive data | Secrets encryption, network policies |
| Denial of Service | Resource exhaustion | Resource limits, quotas |
| Elevation | Gaining privileges | Least privilege RBAC, pod security |

### Security Checklist

- [ ] Enable RBAC authorization
- [ ] Use network policies to restrict pod communication
- [ ] Encrypt etcd data at rest
- [ ] Enable audit logging
- [ ] Use pod security standards/admission controllers
- [ ] Implement secrets management (HashiCorp Vault, AWS Secrets Manager)

---

## Layer 5: AI-Enhanced Testing Ecosystem

### Cluster Health Tests

```typescript
// k8s-health-test.ts
import { k8s } from './kubernetes-client';

interface ClusterHealth {
  controlPlane: boolean;
  nodeStatus: Map<string, 'Ready' | 'NotReady'>;
  podCount: number;
  failedPods: Pod[];
}

async function checkClusterHealth(): Promise<ClusterHealth> {
  const nodes = await k8s.core.listNode();
  const nodeStatus = new Map<string, 'Ready' | 'NotReady'>();
  
  for (const node of nodes.items) {
    const conditions = node.status.conditions;
    const readyCondition = conditions.find(c => c.type === 'Ready');
    nodeStatus.set(node.metadata.name, readyCondition?.status === 'True' ? 'Ready' : 'NotReady');
  }

  const pods = await k8s.core.listPod('default');
  const failedPods = pods.items.filter(p => p.status.phase === 'Failed');

  return {
    controlPlane: nodeStatus.size > 0,
    nodeStatus,
    podCount: pods.items.length,
    failedPods
  };
}
```

---

## Layer 6: DevOps & SRE Operations Center

### SLI/SLO Definitions

| Metric | SLI | SLO |
|--------|-----|-----|
| API Server availability | Request success rate | 99.9% |
| Node health | Ready node percentage | 99.5% |
| Pod scheduling | Successful scheduling rate | 99% |
| etcd latency | P99 API call latency | 100ms |

### Monitoring Stack

```yaml
# prometheus-values.yaml
prometheus:
  prometheusSpec:
    retention: 15d
    storageSpec:
      volumeClaimTemplate:
        spec:
          resources:
            requests:
              storage: 50Gi

prometheus-node-exporter:
  tolerations:
    - effect: NoSchedule
      operator: Exists

kube-state-metrics:
  collectors:
    - deployments
    - pods
    - services
```

---

## Layer 7: Advanced Learning Analytics

### Knowledge Graph

- **Prerequisites**: Docker fundamentals, container networking
- **Related Topics**: Service mesh, ingress controllers, Helm
- **Career Mapping**: Cloud Engineer, DevOps Engineer, Platform Engineer

### Hands-On Challenges

1. **Easy**: Set up local cluster and deploy a Node.js app
2. **Medium**: Configure RBAC for multi-team environment
3. **Hard**: Set up cluster autoscaler with HPA

---

## Layer 8: Enterprise Integration Framework

### System Integration Patterns

- **GitOps**: ArgoCD or Flux for declarative deployments
- **Service Catalog**: Backstage for developer self-service
- **Multi-cluster**: Federation for cross-cluster workloads

---

## Diagnostic Center

### Troubleshooting Flowchart

```
Cluster not responding?
├── Check kubectl connectivity
│   └── Verify kubeconfig
├── Check control plane pods
│   └── kubectl get pods -n kube-system
├── Check etcd health
│   └── kubectl get --raw=/healthz
└── Check node status
    └── kubectl get nodes
```

---

## Next Steps

Continue to [Kubernetes Deployment](./02-k8s-deployment.md) to learn how to deploy Node.js applications to Kubernetes.