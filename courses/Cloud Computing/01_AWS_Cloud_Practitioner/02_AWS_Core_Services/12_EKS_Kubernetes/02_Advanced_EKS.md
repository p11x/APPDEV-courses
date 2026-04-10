---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: EKS Containers
Purpose: Advanced EKS configuration including Fargate, security, and cluster management
Difficulty: advanced
Prerequisites: 01_Basic_EKS.md
RelatedFiles: 01_Basic_EKS.md, 03_Practical_EKS.md
UseCase: Enterprise Kubernetes deployment
CertificationExam: AWS Developer Associate, CKA
LastUpdated: 2025
---

## 💡 WHY

Advanced EKS features enable secure, scalable container orchestration with serverless options and enterprise-grade security.

## 📖 WHAT

### Advanced Features

**Fargate**: Serverless containers without node management

**Cluster Auto Scaler**: Dynamic node scaling

**IRSA**: IAM Roles for Service Accounts

**EKS Add-ons**: Managed Kubernetes components

**Windows Containers**: Windows node support

### Cross-Platform Comparison

| Feature | AWS EKS | Azure AKS | GCP GKE | Open-Source |
|---------|---------|-----------|---------|--------------|
| Managed Control Plane | Yes | Yes | Yes | No |
| Serverless Containers | Fargate | Virtual Nodes | Autopilot | No |
| Windows Support | Yes | Yes | No | Yes |
| GPU Nodes | Yes | Yes | Yes | Yes |
| Native Networking | VPC CNI | Azure CNI | VPC-native | Various |
| Istio Integration | Yes | Yes | Yes | Yes |

## 🔧 HOW

### Example 1: Fargate Profile

```bash
# Create Fargate profile
aws eks create-fargate-profile \
    --cluster-name production-cluster \
    --fargate-profile-name production-apps \
    --pod-execution-role-arn arn:aws:iam::123456789:role/eks-fargate-role \
    --subnets subnet-12345 subnet-67890 \
    --selectors '[
        {"namespace": "production"},
        {"namespace": "monitoring", "labels": {"env": "prod"}}
    ]'
```

### Example 2: IRSA (IAM Roles for Service Accounts)

```bash
# Create OIDC provider for cluster
aws eks describe-cluster \
    --name production-cluster \
    --query 'cluster.identity.oidc.issuer'

# Create service account with IAM role
kubectl create serviceaccount my-app-sa -n production

# Create IAM role with trust policy
aws iam create-role \
    --role-name my-app-s3-role \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {
                "Federated": "arn:aws:iam::123456789:oidc-provider/oidc.eks.us-east-1.amazonaws.com/id/ABC123"
            },
            "Action": "sts:AssumeRoleWithWebIdentity",
            "Condition": {
                "StringEquals": {
                    "oidc.eks.us-east-1.amazonaws.com/id/ABC123:sub": "system:serviceaccount:production:my-app-sa"
                }
            }
        }]
    }'

# Attach policy
aws iam attach-role-policy \
    --role-name my-app-s3-role \
    --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess
```

### Example 3: EKS Add-ons

```bash
# Add VPC CNI
aws eks create-addon \
    --cluster-name production-cluster \
    --addon-name vpc-cni \
    --addon-version v1.14.1-eksbuild.2

# Add CoreDNS
aws eks create-addon \
    --cluster-name production-cluster \
    --addon-name coredns \
    --addon-version v1.10.1-eksbuild.1

# Add kube-proxy
aws eks create-addon \
    --cluster-name production-cluster \
    --addon-name kube-proxy \
    --addon-version v1.27.0-eksbuild.1
```

## ⚠️ COMMON ISSUES

### 1. Nodes Not Joining Cluster

**Problem**: Worker nodes fail to join cluster

**Solution**: Verify node role has required policies, check security groups, verify kubelet can reach API server

### 2. Fargate Pods Stuck

**Problem**: Fargate pods not being scheduled

**Solution**: Ensure Fargate profile exists for the namespace, check pod execution role

### 3. IRSA Not Working

**Problem**: Service account cannot access AWS resources

**Solution**: Verify OIDC provider is configured, check trust relationship

## 🏃 PERFORMANCE

### Limits

| Resource | Limit |
|----------|-------|
| Clusters per account | 100 |
| Nodes per cluster | 100 |
| Pods per node | 110 |
| Namespaces | Unlimited |

## 🔗 CROSS-REFERENCES

**Related**: ECS, Fargate, IAM, VPC, ALB

**Prerequisite**: Basic EKS understanding

## ✅ EXAM TIPS

- EKS managed control plane, you manage worker nodes
- Fargate: serverless, no node management
- IRSA: secure IAM access from pods
- EKS Add-ons: managed components
- Cluster Auto Scaler scales nodes based on pod needs