---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: GKE Kubernetes
Purpose: Hands-on exercises for GKE cluster deployment and management
Difficulty: advanced
Prerequisites: 01_Basic_GKE.md, 02_Advanced_GKE.md
RelatedFiles: 01_Basic_GKE.md, 02_Advanced_GKE.md
UseCase: Production Kubernetes deployments, enterprise workloads
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with GKE is essential for managing production Kubernetes clusters and deploying containerized applications at scale.

### Lab Goals

- Deploy GKE clusters
- Configure security
- Implement autoscaling

## 📖 WHAT

### Exercise Overview

1. **Cluster Deployment**: Standard and Autopilot
2. **Security Configuration**: Workload Identity
3. **Scaling**: HPA, VPA, Cluster Autoscaler

## 🔧 HOW

### Exercise 1: Deploy Production GKE Cluster

```bash
#!/bin/bash
# Deploy production GKE cluster

PROJECT_ID="my-project-id"
CLUSTER_NAME="production-cluster"

gcloud config set project $PROJECT_ID

# Create Standard cluster with best practices
gcloud container clusters create $CLUSTER_NAME \
    --location=us-central1 \
    --node-locations=us-central1-a,us-central1-b \
    --num-nodes=3 \
    --machine-type=e2-standard-4 \
    --enable-autoscaling \
    --min-nodes=2 \
    --max-nodes=10 \
    --enable-private-nodes \
    --master-ipv4-cidr=172.16.0.0/28 \
    --enable-shielded-nodes \
    --enable-intra-node-visibility \
    --enable-network-policy \
    --enable-vertical-pod-autoscaling \
    --enable-dashboard \
    --workload-pool=$PROJECT_ID.svc.id.goog

# Get cluster credentials
gcloud container clusters get-credentials $CLUSTER_NAME \
    --location=us-central1

# Verify cluster
kubectl cluster-info

echo "Production cluster deployed!"
```

### Exercise 2: Configure Workload Identity

```bash
#!/bin/bash
# Configure Workload Identity for secure access

PROJECT_ID="my-project-id"
CLUSTER_NAME="production-cluster"

# Enable Workload Identity
gcloud container clusters update $CLUSTER_NAME \
    --location=us-central1 \
    --enable-workload-identity

# Create Kubernetes service account
kubectl create serviceaccount app-sa --namespace=default

# Create GCP service account
gcloud iam service-accounts create app-gsa \
    --display-name="App Service Account"

# Grant permissions to GCP SA
gcloud projects add-iam-policy-binding $PROJECT_ID \
    --member="serviceAccount:app-gsa@$PROJECT_ID.iam.gserviceaccount.com" \
    --role="roles/storage.objectViewer"

# Bind KSA to GCP SA
gcloud iam service-accounts add-iam-policy-binding \
    app-gsa@$PROJECT_ID.iam.gserviceaccount.com \
    --member="system:serviceaccount:default:app-sa" \
    --role="roles/iam.workloadIdentityUser"

# Annotate KSA
kubectl annotate serviceaccount app-sa \
    iam.gke.io/workload-identity-user=project.svc.id.goog[$PROJECT_ID/default] \
    --namespace=default

# Deploy application using Workload Identity
kubectl apply -f - << 'EOF'
apiVersion: v1
kind: Pod
metadata:
  name: app-pod
spec:
  serviceAccountName: app-sa
  containers:
  - name: app
    image: gcr.io/google-samples/hello-app:2.0
EOF

echo "Workload Identity configured!"
```

### Exercise 3: Configure Autoscaling

```bash
#!/bin/bash
# Configure autoscaling for GKE workloads

PROJECT_ID="my-project-id"
CLUSTER_NAME="production-cluster"

# Get credentials
gcloud container clusters get-credentials $CLUSTER_NAME \
    --location=us-central1

# Horizontal Pod Autoscaler
kubectl autoscale deployment myapp \
    --min=2 \
    --max=20 \
    --cpu-percent=70

# Vertical Pod Autoscaling
kubectl apply -f - << 'EOF'
apiVersion: autoscaling.k8s.io/v1
kind: VerticalPodAutoscaler
metadata:
  name: myapp-vpa
spec:
  targetRef:
    apiVersion: "apps/v1"
    kind: Deployment
    name: myapp
  updatePolicy:
    updateMode: "Auto"
EOF

# Cluster Autoscaler (enabled on node pool)
gcloud container clusters update $CLUSTER_NAME \
    --location=us-central1 \
    --enable-autoscaling \
    --min-nodes=2 \
    --max-nodes=20

# Node pool autoscaling
gcloud container node-pools update my-pool \
    --cluster=$CLUSTER_NAME \
    --location=us-central1 \
    --enable-autoscaling \
    --min-nodes=2 \
    --max-nodes=10

# Check autoscaling status
kubectl get hpa
kubectl get vpa

echo "Autoscaling configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Pods not starting | Check quotas |
| Image pull errors | Check IAM |
| Node issues | Check node pool |

### Validation

```bash
# Check cluster status
gcloud container clusters describe production-cluster --location=us-central1

# Check node pools
gcloud container node-pools list --cluster=production-cluster --location=us-central1

# View pod status
kubectl get pods -o wide
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Load Balancing
- Cloud Monitoring
- Cloud Storage

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Load Balancing
- Cloud Monitoring
- Cloud Run

### Next Steps

- Set up Cloud Armor
- Configure Ingress
- Implement service mesh

## ✅ EXAM TIPS

- Know cluster configuration options
- Practice kubectl commands
- Understand autoscaling types
- Remember Workload Identity
