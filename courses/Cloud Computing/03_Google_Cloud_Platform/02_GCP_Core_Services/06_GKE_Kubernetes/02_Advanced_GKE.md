---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: GKE Kubernetes
Purpose: Advanced understanding of GKE features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_GKE.md
RelatedFiles: 01_Basic_GKE.md, 03_Practical_GKE.md
UseCase: Production Kubernetes clusters, enterprise workloads
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced GKE knowledge enables building production-grade Kubernetes clusters with enterprise features like security, autoscaling, and multi-cluster management.

### Why Advanced GKE

- **Autopilot**: Fully managed Kubernetes
- **Workload Identity**: Secure IAM integration
- **Multi-cluster**: Federation andAnthos
- **Security**: RBAC, pod security, network policies

## 📖 WHAT

### GKE Deployment Modes

| Mode | Management | Pricing | Use Case |
|------|-------------|---------|----------|
| Autopilot | Full | Per-pod | Managed workloads |
| Standard | Self-managed | Per-node | Custom clusters |

### Advanced Networking

- **Network Policies**: Cluster-level traffic control
- **Ingress**: Global HTTP(S) load balancing
- **Gateway API**: Modern ingress configuration
- **Cilium**: Enhanced networking

### Security Features

- Workload Identity
- Binary Authorization
- Shielded GKE nodes
- RBAC with IAM integration

## 🔧 HOW

### Example 1: Autopilot Cluster Configuration

```bash
# Create Autopilot cluster
gcloud container clusters create-auto my-autopilot \
    --location=us-central1 \
    --enable-private-nodes \
    --enable-master-authorized-networks \
    --master-authorized-networks=10.0.0.0/8 \
    --enable-shielded-nodes \
    --enable-vertical-pod-autoscaling \
    --enable-dashboard

# Deploy workload
kubectl create deployment myapp --image=nginx

# Enable horizontal pod autoscaling
kubectl autoscale deployment myapp --min=2 --max=10 --cpu-percent=70
```

### Example 2: Workload Identity Configuration

```bash
# Enable Workload Identity
gcloud container clusters update my-cluster \
    --enable-workload-identity

# Create Kubernetes service account
kubectl create serviceaccount my-app-sa \
    --namespace=default

# Create IAM policy binding
gcloud iam service-accounts add-iam-policy-binding \
    --member="system:serviceaccount:default:my-app-sa" \
    --role="roles/storage.objectViewer" \
    --namespace=default \
    --project=my-project

# Annotate KSA
kubectl annotate serviceaccount my-app-sa \
    iam.gke.io/gcp-service-account="my-app-sa@my-project.iam.gserviceaccount.com"
```

### Example 3: Multi-cluster Management

```bash
# Register cluster to fleet
gcloud container clusters register my-cluster \
    --location=us-central1 \
    --fleet-project=my-fleet-project

# List registered clusters
gcloud container clusters list --fleet-project=my-fleet-project

# Configure Config Sync
gcloud alpha container hub config-sync apply \
    --sync-file=sync.yaml \
    --fleet-project=my-fleet-project

# Set up Multi-cluster Services
gcloud container hub multi-cluster-services enable \
    --project=my-project
```

## ⚠️ COMMON ISSUES

### Troubleshooting GKE Issues

| Issue | Solution |
|-------|----------|
| Pods pending | Check resource quotas |
| Node issues | Check node pool status |
| Network issues | Check VPC, firewall rules |
| Image pull errors | Check registry permissions |

### Best Practices

- Use Autopilot when possible
- Enable Workload Identity
- Use private clusters
- Enable vertical pod autoscaling

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GKE | EKS | AKS |
|---------|-----|-----|-----|
| Autopilot | Yes | No | No |
| Private Nodes | Yes | Yes | Yes |
| Workload Identity | Yes | IRSA | WIF |
| Multi-cluster | Yes | EKS Fargate | Arc |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud Run (serverless containers)
- Cloud Load Balancing
- Binary Authorization

### Study Resources

- GKE documentation
- Kubernetes best practices

## ✅ EXAM TIPS

- Autopilot = fully managed
- Workload Identity = secure IAM
- Private nodes = enhanced security
- Multi-cluster for high availability
