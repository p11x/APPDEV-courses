---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: EKS Kubernetes
Purpose: Understanding Amazon EKS managed Kubernetes service
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_Concepts.md, Docker Basics
RelatedFiles: 02_Advanced_EKS.md, 03_Practical_EKS.md
UseCase: Running containerized applications on managed Kubernetes
CertificationExam: AWS Developer Associate, CKA
LastUpdated: 2025
---

## 💡 WHY

Amazon EKS is AWS's managed Kubernetes service that makes it easy to run Kubernetes without managing the control plane. It's essential for container orchestration in production environments.

### Why EKS Matters

- **Managed Control Plane**: No master node management
- **Kubernetes Certified**: CNCF certified
- **Multi-AZ**: High availability built-in
- **Integration**: Native AWS service integration
- **Scaling**: Auto-scaling for nodes and pods

### Industry Use Cases

- Microservices architectures
- CI/CD pipelines
- Batch processing
- Machine learning workloads

## 📖 WHAT

### EKS Core Concepts

**Cluster**: Kubernetes control plane + worker nodes

**Control Plane**: API server, etcd, scheduler

**Worker Nodes**: EC2 instances running kubelet

**Fargate**: Serverless containers on EKS

**Node Group**: Collection of EC2 instances

### Architecture Diagram

```
EKS Architecture
================

┌─────────────────────────────────────────┐
│           AWS Cloud                      │
│                                          │
│  ┌──────────────────────────────────┐   │
│  │     EKS Control Plane            │   │
│  │   (Managed by AWS)               │   │
│  │   - API Server                   │   │
│  │   - etcd                         │   │
│  │   - Scheduler                    │   │
│  └──────────────────────────────────┘   │
│                  │                       │
│         ┌────────┼────────┐              │
│         │        │        │              │
│   ┌─────┴───┐ ┌──┴──┐ ┌───┴─────┐       │
│   │Node Grp1│ │Node │ │Node Grp2│       │
│   │  (AZ-a) │ │Grp2 │ │  (AZ-b) │       │
│   └─────┬───┘ └──┬──┘ └───┬─────┘       │
│         │        │        │              │
│    ┌────┴──┐ ┌───┴───┐ ┌──┴────┐        │
│    │Pods   │ │Pods   │ │Pods   │        │
│    └───────┘ └───────┘ └───────┘        │
└─────────────────────────────────────────┘
```

## 🔧 HOW

### Example 1: Create EKS Cluster

```bash
# Create cluster
aws eks create-cluster \
    --name my-cluster \
    --region us-east-1 \
    --kubernetes-version 1.29 \
    --role-arn arn:aws:iam::123456789:role/EKSRole \
    --resources-vpc-config '{
        "subnetIds": ["subnet-12345", "subnet-67890"],
        "securityGroupIds": ["sg-12345"]
    }'

# Wait for cluster to be active
aws eks wait cluster-active \
    --name my-cluster

# Update kubeconfig
aws eks update-kubeconfig \
    --name my-cluster \
    --region us-east-1
```

### Example 2: Create Node Group

```bash
# Create node group
aws eks create-nodegroup \
    --cluster-name my-cluster \
    --nodegroup-name my-nodes \
    --scaling-config '{
        "minSize": 2,
        "maxSize": 4,
        "desiredSize": 2
    }' \
    --instance-types m5.large \
    --ami-type AL2_x86_64 \
    --subnet-ids subnet-12345 subnet-67890 \
    --node-role arn:aws:iam::123456789:role/NodeInstanceRole

# List node groups
aws eks list-nodegroups \
    --cluster-name my-cluster

# Describe node group
aws eks describe-nodegroup \
    --cluster-name my-cluster \
    --nodegroup-name my-nodes
```

### Example 3: Deploy Application

```yaml
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp
  labels:
    app: myapp
spec:
  replicas: 3
  selector:
    matchLabels:
      app: myapp
  template:
    metadata:
      labels:
        app: myapp
    spec:
      containers:
      - name: myapp
        image: nginx:latest
        ports:
        - containerPort: 80
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
---
apiVersion: v1
kind: Service
metadata:
  name: myapp-service
spec:
  type: LoadBalancer
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 80
```

```bash
# Apply deployment
kubectl apply -f deployment.yaml

# Check pods
kubectl get pods

# Check services
kubectl get svc
```

### Example 4: Helm Chart Deployment

```bash
# Add Helm repo
helm repo add aws-ebs-csi-driver https://kubernetes-sigs.github.io/aws-ebs-csi-driver
helm repo update

# Install EBS CSI Driver
helm install aws-ebs-csi-driver aws-ebs-csi-driver/aws-ebs-csi-driver \
    --namespace kube-system

# Deploy Prometheus with Helm
helm repo add prometheus https://prometheus-community.github.io/helm-charts
helm install prometheus prometheus/kube-prometheus-stack
```

## ⚠️ COMMON ISSUES

### 1. Node Group Creation Fails

**Problem**: Nodes not joining cluster.

**Solution**: Check node role has correct IAM policies, security group allows control plane communication.

### 2. Pods Stuck in Pending

**Problem**: Insufficient cluster resources.

**Solution**: Check node group size, adjust scaling configuration.

### 3. Service Not Accessible

**Problem**: Load balancer issues.

**Solution**: Check security groups, ensure subnets have correct tags for ALB.

### 4. EBS Volume Not Attaching

**Problem**: Volume issues.

**Solution**: Install EBS CSI driver, check storage class.

## 🏃 PERFORMANCE

### Limits

| Resource | Limit |
|----------|-------|
| Clusters per account | 100 |
| Nodes per node group | 100 |
| Pods per node | 110 |

### Node Types

| Type | Use Case |
|------|----------|
| Fargate | Serverless, small workloads |
| On-Demand | General purpose |
| Spot | Cost-effective, fault-tolerant |

## 🌐 COMPATIBILITY

| Feature | AWS EKS | Azure AKS | GCP GKE |
|---------|---------|-----------|---------|
| Kubernetes | Yes | Yes | Yes |
| Fargate | Yes | Virtual Nodes | Autopilot |
| Managed Nodes | Yes | Yes | Yes |
| Serverless | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

**Related**: ECS, Fargate, CloudWatch Container Insights

**Prerequisite**: Docker basics, Kubernetes fundamentals

**Next**: AWS App Mesh for service mesh

## ✅ EXAM TIPS

- EKS control plane runs in AWS-managed VPC
- Node groups span multiple AZs
- Fargate provides serverless compute
- Use eksctl for quick cluster creation
- kubectl interacts with EKS clusters
- Security: use IAM roles for service accounts (IRSA)