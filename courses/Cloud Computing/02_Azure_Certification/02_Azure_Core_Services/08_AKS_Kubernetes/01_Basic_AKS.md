---
title: "Azure Kubernetes Service (AKS) - Basic"
category: Azure Certification
subcategory: Azure Core Services
concept: AKS Kubernetes
purpose: Understanding Azure Kubernetes Service for container orchestration
difficulty: beginner
prerequisites:
  - 01_Basic_Azure_Compute.md
relatedFiles:
  - 02_Advanced_AKS.md
  - 03_Practical_AKS.md
useCase: Container orchestration
certificationExam: AZ-900 Azure Fundamentals
lastUpdated: 2025
---

# Azure Kubernetes Service (AKS) - Basic Concepts

## Overview

Azure Kubernetes Service (AKS) is a managed Kubernetes service that simplifies deploying, managing, and scaling containerized applications using Kubernetes on Azure. As a fully managed Kubernetes orchestrator, AKS handles the complexity of managing Kubernetes infrastructure, allowing developers to focus on application development rather than cluster management.

## What is Kubernetes?

Kubernetes is an open-source container orchestration platform originally developed by Google and now maintained by the Cloud Native Computing Foundation (CNCF). It automates the deployment, scaling, and management of containerized applications across clusters of hosts.

### Key Kubernetes Concepts

- **Pod**: The smallest deployable unit in Kubernetes, representing a single instance of an application
- **Node**: A worker machine in Kubernetes cluster (virtual or physical)
- **Cluster**: A set of nodes that run containerized applications
- **Service**: An abstraction defining a logical set of pods and a policy to access them
- **Deployment**: A declarative way to manage pod replicas
- **Ingress**: Manages external access to services, typically HTTP/HTTPS

## AKS Cluster Architecture

### Control Plane

The AKS control plane is fully managed by Azure and includes:

1. **kube-apiserver**: The API server that exposes Kubernetes API
2. **etcd**: Key-value store for cluster state
3. **kube-scheduler**: Schedules pods to nodes
4. **kube-controller-manager**: Runs controller loops
5. **cloud-controller-manager**: Integrates with Azure cloud APIs

### Node Pools

AKS uses node pools to group VMs with similar configurations:

- **System node pools**: Host system pods (DNS, ingress controllers)
- **User node pools**: Host application workloads
- Each node pool can have different VM sizes and scaling settings

### Node Architecture

Nodes in AKS run:
- Kubelet: Agent that manages containers on the node
- kube-proxy: Network proxy for service communication
- Container Runtime: Typically containerd or Docker

## Getting Started with AKS

### Prerequisites

- Azure subscription
- Azure CLI installed
- kubectl installed
- Basic understanding of containers and Docker

### Installing Required Tools

```bash
# Install Azure CLI
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# Install kubectl
az aks install-cli

# Verify installation
kubectl version --client
az --version
```

### Creating Your First AKS Cluster

```bash
# Set subscription
az account set --subscription <your-subscription-id>

# Create resource group
az group create --name myResourceGroup --location eastus

# Create AKS cluster
az aks create \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --node-count 2 \
  --generate-ssh-keys \
  --load-balancer-sku standard

# Get cluster credentials
az aks get-credentials \
  --resource-group myResourceGroup \
  --name myAKSCluster

# Verify connection
kubectl get nodes
```

### Basic kubectl Commands

```bash
# Get cluster information
kubectl cluster-info

# List all pods in default namespace
kubectl get pods

# List all namespaces
kubectl get namespaces

# List all nodes
kubectl get nodes

# Get pod details
kubectl describe pod <pod-name>

# Get events
kubectl get events

# View logs
kubectl logs <pod-name>

# Execute command in pod
kubectl exec -it <pod-name> -- /bin/bash
```

### Working with Deployments

```bash
# Create a deployment
kubectl create deployment nginx --image=nginx

# Scale a deployment
kubectl scale deployment nginx --replicas=3

# Update deployment image
kubectl set image deployment/nginx nginx=nginx:1.19

# Check rollout status
kubectl rollout status deployment/nginx

# Rollback to previous revision
kubectl rollout undo deployment/nginx
```

### Working with Services

```bash
# Expose deployment as LoadBalancer service
kubectl expose deployment nginx --port=80 --type=LoadBalancer

# List services
kubectl get services

# Describe service
kubectl describe service <service-name>
```

### Working with ConfigMaps and Secrets

```bash
# Create ConfigMap from file
kubectl create configmap app-config \
  --from-file=config.json

# Create ConfigMap from literal values
kubectl create configmap app-settings \
  --from-literal=DEBUG=true \
  --from-literal=LOG_LEVEL=info

# Create Secret
kubectl create secret generic db-credentials \
  --from-literal=username=admin \
  --from-literal=password=secret
```

## Azure CLI for AKS

### Cluster Management

```bash
# List AKS clusters
az aks list --resource-group myResourceGroup
az aks list -o table

# Show cluster details
az aks show \
  --resource-group myResourceGroup \
  --name myAKSCluster

# Update cluster
az aks update \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --node-count 3

# Upgrade cluster Kubernetes version
az aks upgrade \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --kubernetes-version 1.28

# Stop/start cluster
az aks stop \
  --resource-group myResourceGroup \
  --name myAKSCluster

az aks start \
  --resource-group myResourceGroup \
  --name myAKSCluster
```

### Node Pool Management

```bash
# Add node pool
az aks nodepool add \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster \
  --name pool2 \
  --node-count 2 \
  --vm-size Standard_DS2_v2

# List node pools
az aks nodepool list \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster

# Scale node pool
az aks nodepool scale \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster \
  --name pool2 \
  --node-count 4

# Delete node pool
az aks nodepool delete \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster \
  --name pool2
```

### Monitoring and Diagnostics

```bash
# Enable monitoring
az aks enable-addons \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster \
  --addons monitoring

# Get logs
az aks show-ads \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster
```

## Deploying Applications to AKS

### Sample Application Deployment

```yaml
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp-deployment
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
        image: nginx:1.25
        ports:
        - containerPort: 80
        resources:
          requests:
            memory: "64Mi"
            cpu: "250m"
          limits:
            memory: "128Mi"
            cpu: "500m"
```

```yaml
# service.yaml
apiVersion: v1
kind: Service
metadata:
  name: myapp-service
spec:
  selector:
    app: myapp
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
  type: LoadBalancer
```

```bash
# Apply configurations
kubectl apply -f deployment.yaml
kubectl apply -f service.yaml

# Check deployment status
kubectl get deployments
kubectl get pods
kubectl get services

# Scale deployment
kubectl scale deployment myapp-deployment --replicas=5

# View application logs
kubectl logs -l app=myapp

# Access the application
kubectl get service myapp-service
```

## Resource Management

### Namespaces

```bash
# Create namespace
kubectl create namespace dev

# Set default namespace
kubectl config set-context --current --namespace=dev

# List resources in namespace
kubectl get all -n dev
```

### Resource Quotas

```yaml
# resource-quota.yaml
apiVersion: v1
kind: ResourceQuota
metadata:
  name: compute-quota
spec:
  hard:
    requests.cpu: "4"
    requests.memory: "8Gi"
    limits.cpu: "8"
    limits.memory: "16Gi"
    pods: "20"
```

### LimitRanges

```yaml
# limit-range.yaml
apiVersion: v1
kind: LimitRange
metadata:
  name: default-limits
spec:
  limits:
  - default:
      memory: "256Mi"
      cpu: "500m"
    defaultRequest:
      memory: "128Mi"
      cpu: "250m"
    type: Container
```

## Networking Basics

### Kubernetes Networking Model

- All pods can communicate with each other without NAT
- All nodes can communicate with all pods without NAT
- Pod IP addresses are unique across the cluster

### Service Types

```yaml
# ClusterIP (default)
apiVersion: v1
kind: Service
metadata:
  name: my-service
spec:
  type: ClusterIP
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 80

# NodePort
apiVersion: v1
kind: Service
metadata:
  name: my-service
spec:
  type: NodePort
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 80
    nodePort: 30080

# LoadBalancer
apiVersion: v1
kind: Service
metadata:
  name: my-service
spec:
  type: LoadBalancer
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 80
```

### DNS in AKS

AKS provides internal DNS for service discovery:
- Services get DNS names in format: `<service-name>.<namespace>.svc.cluster.local`
- Headless services for pod discovery: `<service-name>.<namespace>.svc.cluster.local`

## Storage Basics

### Volumes in Kubernetes

```yaml
# EmptyDir volume
apiVersion: v1
kind: Pod
metadata:
  name: test-pod
spec:
  containers:
  - name: test-container
    image: nginx
    volumeMounts:
    - name: cache-volume
      mountPath: /app/cache
  volumes:
  - name: cache-volume
    emptyDir: {}
```

### PersistentVolumes

```yaml
# PersistentVolume
apiVersion: v1
kind: PersistentVolume
metadata:
  name: my-pv
spec:
  capacity:
    storage: 10Gi
  accessModes:
    - ReadWriteOnce
  storageClassName: managed-premium
  azureDisk:
    kind: Managed
    diskName: my-disk
    diskURI: /subscriptions/{sub}/resourcegroups/{rg}/providers.microsoft.compute/disks/my-disk
```

### PersistentVolumeClaims

```yaml
# PersistentVolumeClaim
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: my-pvc
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 5Gi
  storageClassName: managed-premium
```

## Security Basics

### RBAC (Role-Based Access Control)

```yaml
# Role
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  namespace: default
  name: pod-reader
rules:
- apiGroups: [""]
  resources: ["pods"]
  verbs: ["get", "list", "watch"]

# RoleBinding
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: read-pods
  namespace: default
subjects:
- kind: User
  name: jane
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: Role
  name: pod-reader
  apiGroup: rbac.authorization.k8s.io
```

### Kubernetes Secrets

```bash
# Create generic secret
kubectl create secret generic db-creds \
  --from-literal=username=admin \
  --from-literal=password='password123'

# Create TLS secret
kubectl create secret tls tls-secret \
  --cert=path/to/cert.crt \
  --key=path/to/cert.key

# Use secret in pod
kubectl apply -f - <<EOF
apiVersion: v1
kind: Pod
metadata:
  name: secret-pod
spec:
  containers:
  - name: test
    image: nginx
    env:
    - name: DB_USER
      valueFrom:
        secretKeyRef:
          name: db-creds
          key: username
    - name: DB_PASSWORD
      valueFrom:
        secretKeyRef:
          name: db-creds
          key: password
EOF
```

## Monitoring and Observability

### Health Checks

```yaml
# Liveness Probe
apiVersion: v1
kind: Pod
metadata:
  name: myapp
spec:
  containers:
  - name: myapp
    image: myapp:latest
    livenessProbe:
      httpGet:
        path: /healthz
        port: 8080
      initialDelaySeconds: 15
      periodSeconds: 20

# Readiness Probe
    readinessProbe:
      httpGet:
        path: /ready
        port: 8080
      initialDelaySeconds: 5
      periodSeconds: 10
```

### Resource Monitoring

```bash
# View node resources
kubectl top nodes

# View pod resources
kubectl top pods

# Describe pod for resource info
kubectl describe pod <pod-name>
```

## Scaling Applications

### Manual Scaling

```bash
# Scale deployment
kubectl scale deployment myapp --replicas=10

# Scale statefulset
kubectl scale statefulset mystatefulset --replicas=5
```

### Horizontal Pod Autoscaler

```yaml
# HorizontalPodAutoscaler
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: myapp-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: myapp
  minReplicas: 3
  maxReplicas: 20
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

## Best Practices

### Deployment Best Practices

1. Use ready-to-run images
2. Specify resource requests and limits
3. Use health probes
4. Implement proper logging
5. Use labels for organization

### Security Best Practices

1. Use RBAC to restrict access
2. Use secrets for sensitive data
3. Run containers as non-root users
4. Keep Kubernetes updated
5. Use network policies

### Performance Best Practices

1. Right-size resource requests
2. Use pod disruption budgets
3. Implement proper caching
4. Use appropriate storage classes

## Clean Up

```bash
# Delete deployment
kubectl delete deployment myapp-deployment

# Delete service
kubectl delete service myapp-service

# Delete cluster
az aks delete \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --yes

# Delete resource group
az group delete --name myResourceGroup --yes
```

## Summary

In this guide, you learned:

- Basic Kubernetes concepts (pods, nodes, services, deployments)
- AKS cluster architecture (control plane, node pools)
- Basic kubectl commands for interacting with clusters
- Azure CLI commands for AKS management
- Deploying applications to AKS
- Working with namespaces, volumes, and secrets
- Basic security with RBAC
- Monitoring with health probes
- Scaling applications manually and with HPA

Next steps:
- Explore advanced AKS topics in 02_Advanced_AKS.md
- Learn practical management in 03_Practical_AKS.md
- Practice deploying real applications to AKS
- Study for AZ-900 Azure Fundamentals exam