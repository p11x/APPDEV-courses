---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: GKE Kubernetes
Purpose: Understanding GCP GKE managed Kubernetes service
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_GKE.md, 03_Practical_GKE.md
UseCase: Container orchestration on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

GKE (Google Kubernetes Engine) provides managed Kubernetes on GCP. Understanding GKE is essential for containerized applications.

## 📖 WHAT

### GKE Features

- **Autopilot**: Fully managed Kubernetes
- **Standard**: Self-managed clusters
- **Auto-scaling**: Node and pod scaling
- **Load Balancing**: Integrated L7 balancing
- **Private Cluster**: VPC-native isolation

## 🔧 HOW

### Example: Create Cluster

```bash
# Create cluster
gcloud container clusters create my-cluster \
    --zone us-central1-a \
    --node-pool-size 3

# Get credentials
gcloud container clusters get-credentials my-cluster

# Deploy application
kubectl create deployment myapp --image=nginx
```

## ✅ EXAM TIPS

- GKE = managed Kubernetes
- Autopilot = fully managed
- Kubernetes is foundation of GCP services