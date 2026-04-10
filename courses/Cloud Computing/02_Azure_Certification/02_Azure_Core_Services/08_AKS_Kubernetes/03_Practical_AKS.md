---
title: "Azure Kubernetes Service (AKS) - Practical"
category: Azure Certification
subcategory: Azure Core Services
concept: AKS Kubernetes
purpose: Practical AKS management and operations
difficulty: intermediate
prerequisites:
  - 01_Basic_AKS.md
  - 02_Advanced_AKS.md
relatedFiles:
  - 01_Basic_AKS.md
  - 02_Advanced_AKS.md
useCase: Managing Kubernetes
certificationExam: AZ-104 Azure Administrator
lastUpdated: 2025
---

# Azure Kubernetes Service (AKS) - Practical Guide

## Overview

This practical guide covers hands-on tasks for managing AKS clusters, including creation, deployment, scaling, monitoring, and day-to-day operational tasks that Azure Administrators perform.

## Creating an AKS Cluster

### Step-by-Step Cluster Creation

```bash
# 1. Verify Azure CLI and login
az --version
az login

# 2. Set subscription
az account set --subscription <subscription-name-or-id>
az account show

# 3. Create resource group
az group create \
  --name aks-rg \
  --location eastus

# 4. Verify available VM sizes
az vm list-sizes \
  --location eastus \
  | grep -i standard_d

# 5. Create AKS cluster with standard settings
az aks create \
  --resource-group aks-rg \
  --name myakscluster \
  --node-count 3 \
  --generate-ssh-keys \
  --load-balancer-sku standard \
  --vm-set-type VirtualMachineScaleSets \
  --kubernetes-version 1.27

# 6. Verify cluster creation
az aks show \
  --resource-group aks-rg \
  --name myakscluster

# 7. Install kubectl
az aks install-cli

# 8. Get cluster credentials
az aks get-credentials \
  --resource-group aks-rg \
  --name myakscluster \
  --overwrite-existing

# 9. Verify cluster connectivity
kubectl get nodes
kubectl cluster-info
```

### Creating Cluster with Monitoring

```bash
# Enable Azure Monitor during cluster creation
az aks create \
  --resource-group aks-rg \
  --name myakscluster \
  --node-count 3 \
  --generate-ssh-keys \
  --enable-addons monitoring \
  --workspace-resource-group monitoring-rg
```

### Creating Multi-node Pool Cluster

```bash
# Create cluster with system node pool
az aks create \
  --resource-group aks-rg \
  --name myakscluster \
  --node-count 3 \
  --vm-set-type VirtualMachineScaleSets \
  --kubernetes-version 1.27

# Add Windows node pool
az aks nodepool add \
  --resource-group aks-rg \
  --cluster-name myakscluster \
  --name winpool \
  --os-type Windows \
  --node-count 2 \
  --node-vm-size Standard_D4s_v3

# Add GPU node pool
az aks nodepool add \
  --resource-group aks-rg \
  --cluster-name myakscluster \
  --name gpu-pool \
  --node-vm-size Standard_NC4s_v3 \
  --node-count 1

# List all node pools
az aks nodepool list \
  --resource-group aks-rg \
  --cluster-name myakscluster -o table
```

## Deploying Applications

### Preparing Application for Deployment

```bash
# Create project directory structure
mkdir -p myapp/{k8s,docker}
cd myapp

# Create Dockerfile
cat > docker/Dockerfile <<EOF
FROM node:18-alpine
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
USER node
EXPOSE 3000
CMD ["node", "server.js"]
EOF

# Create Kubernetes manifests
cat > k8s/deployment.yaml <<EOF
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
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  template:
    metadata:
      labels:
        app: myapp
    spec:
      containers:
      - name: myapp
        image: mycontainerregistry.azurecr.io/myapp:v1
        imagePullPolicy: Always
        ports:
        - containerPort: 3000
          name: http
        env:
        - name: NODE_ENV
          value: "production"
        - name: LOG_LEVEL
          valueFrom:
            configMapKeyRef:
              name: myapp-config
              key: log-level
        - name: DB_PASSWORD
          valueFrom:
            secretKeyRef:
              name: myapp-secrets
              key: password
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health
            port: 3000
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /ready
            port: 3000
          initialDelaySeconds: 5
          periodSeconds: 5
EOF

cat > k8s/service.yaml <<EOF
apiVersion: v1
kind: Service
metadata:
  name: myapp
  labels:
    app: myapp
spec:
  type: LoadBalancer
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 3000
    protocol: TCP
    name: http
EOF

cat > k8s/configmap.yaml <<EOF
apiVersion: v1
kind: ConfigMap
metadata:
  name: myapp-config
data:
  log-level: "info"
  app-name: "myapp"
EOF

cat > k8s/secret.yaml <<EOF
apiVersion: v1
kind: Secret
metadata:
  name: myapp-secrets
type: Opaque
stringData:
  username: admin
  password: changeme
EOF
```

### Building and Pushing Docker Image

```bash
# Build Docker image
az acr build \
  --registry mycontainerregistry \
  --image myapp:v1 \
  --file docker/Dockerfile .

# Verify image
az acr repository show \
  --name mycontainerregistry \
  --image myapp:v1
```

### Deploying to AKS

```bash
# Create registry secret for pulling images
kubectl create secret docker-registry acr-secret \
  --docker-server mycontainerregistry.azurecr.io \
  --docker-username $(az acr credential show -n mycontainerregistry --query username -o tsv) \
  --docker-password $(az acr credential show -n mycontainerregistry --query passwords[0].value -o tsv)

# Apply configurations
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml

# Verify deployment
kubectl get deployments
kubectl get pods
kubectl get services

# Check pod logs
kubectl logs -l app=myapp

# Describe deployment
kubectl describe deployment myapp

# View service external IP
kubectl get service myapp -w
```

### Implementing Blue-Green Deployment

```yaml
# Blue deployment (v1)
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp-blue
spec:
  replicas: 3
  selector:
    matchLabels:
      app: myapp
      version: v1
  template:
    metadata:
      labels:
        app: myapp
        version: v1
    spec:
      containers:
      - name: myapp
        image: mycontainerregistry.azurecr.io/myapp:v1
---
# Green deployment (v2)
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp-green
spec:
  replicas: 3
  selector:
    matchLabels:
      app: myapp
      version: v2
  template:
    metadata:
      labels:
        app: myapp
        version: v2
    spec:
      replicas: 0
      containers:
      - name: myapp
        image: mycontainerregistry.azurecr.io/myapp:v2
```

```bash
# Switch traffic to green
kubectl patch deployment myapp-green -p '{"spec":{"replicas":3}}'

# Update service selector to green
kubectl patch service myapp -p '{"spec":{"selector":{"version":"v2"}}}'
```

## Configuring Scaling

### Horizontal Pod Autoscaler

```bash
# Create HPA manifest
cat > k8s/hpa.yaml <<EOF
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: myapp-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: myapp
  minReplicas: 2
  maxReplicas: 20
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
  behavior:
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
      - type: Percent
        value: 10
        periodSeconds: 60
    scaleUp:
      stabilizationWindowSeconds: 0
      policies:
      - type: Percent
        value: 100
        periodSeconds: 15
      - type: Pods
        value: 4
        periodSeconds: 15
      selectPolicy: Max
EOF

kubectl apply -f k8s/hpa.yaml
kubectl get hpa
kubectl describe hpa myapp-hpa
```

### Cluster Autoscaler

```bash
# Enable cluster autoscaler on nodepool
az aks nodepool update \
  --resource-group aks-rg \
  --cluster-name myakscluster \
  --name nodepool1 \
  --enable-cluster-autoscaler \
  --min-nodes 1 \
  --max-nodes 10

# Verify autoscaler status
az aks show \
  --resource-group aks-rg \
  --cluster-name myakscluster \
  --query agentPoolProfiles
```

### Vertical Pod Autoscaler

```bash
# Install VPA components
git clone https://github.com/kubernetes/autoscaler.git
cd autoscaler/vertical-pod-autoscaler
kubectl apply -f deployments/vpa-components.yaml

# Create VPA recommendation
cat > k8s/vpa.yaml <<EOF
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
  resourcePolicy:
    containerPolicies:
    - containerName: myapp
      minAllowed:
        cpu: 100m
        memory: 128Mi
      maxAllowed:
        cpu: 2
        memory: 2Gi
EOF

kubectl apply -f k8s/vpa.yaml
kubectl get vpa
```

### Manual Scaling

```bash
# Scale deployment manually
kubectl scale deployment myapp --replicas=10

# Scale statefulset
kubectl scale statefulset myapp --replicas=3

# Scale via label
kubectl scale deployment -l app=myapp --replicas=5
```

## Monitoring with Azure Monitor

### Setting Up Azure Monitor

```bash
# Enable Azure Monitor addon
az aks enable-addons \
  --addons monitoring \
  --resource-group aks-rg \
  --cluster-name myakscluster

# Get Log Analytics workspace
LOG_ANALYTICS_WORKSPACE=$(az aks show \
  --resource-group aks-rg \
  --name myakscluster \
  --query addonProfiles['omsagent'].config.logAnalyticsWorkspaceResourceID -o tsv)

echo "Log Analytics Workspace: $LOG_ANALYTICS_WORKSPACE"

# View Metrics in Azure Portal
# Navigate to: Azure Monitor > Metrics
```

### Querying Logs

```bash
# Run using Azure CLI
az monitor log-analytics query \
  --workspace $(echo $LOG_ANALYTICS_WORKSPACE | rev | cut -d'/' -f1 | rev) \
  --analytics-queries 'KubePodInventory | where TimeGenerated > ago(1h) | where Namespace == "default" | project TimeGenerated, PodName, ContainerStatus | order by TimeGenerated desc'
```

### Sample Log Queries

```sql
-- Container CPU usage
ContainerID | where TimeGenerated > ago(1h) 
| summarize avg(CPU) by bin(TimeGenerated, 5m), ContainerName
| render timechart

-- Failed pods
KubePodInventory
| where TimeGenerated > ago(24h)
| where ContainerStatusTerminated == "Failed"
| project TimeGenerated, PodName, ContainerStatusReason, ContainerExitCode

-- Memory usage
ContainerID | where TimeGenerated > ago(1h) 
| summarize avg(UsedMemory) by bin(TimeGenerated, 5m), ContainerName

-- Network in/out
KubeNetworkIngress | where TimeGenerated > ago(1h)
| summarize sum(SentBytes), sum(ReceivedBytes) by bin(TimeGenerated, 5m)

-- Pod restart count
KubePodInventory | where TimeGenerated > ago(24h) | summarize sum(ContainerRestartCount) by PodName
```

### Creating Alerts

```bash
# Create alert for pod restart
az monitor metrics alert create \
  --name "PodRestartsAlert" \
  --resource-group aks-rg \
  --description "Alert when pods restart frequently" \
  --condition "avg(KubePodInventory | where TimeGenerated > ago(10m) | summarize RestartCount = sum(ContainerRestartCount) by bin(TimeGenerated, 5m) | where RestartCount > 5)" \
  --evaluation-frequency 15m \
  --window-size 15m
```

### Azure Dashboard Configuration

```bash
# Create dashboard via Azure CLI
az portal dashboard create \
  --name "AKSDashboard" \
  --resource-group aks-rg \
  --location eastus \
  --tags prod=true \
  --input-json '{
    "lenses": {
      "0": {
        "order": 0,
        "title": "AKS Overview",
        "parts": {
          "0": {
            "position": {"rowSpan": 2, "colSpan": 12},
            "metadata": {"inputs": [{"name": "query", "value": "KubePodInventory | where TimeGenerated > ago(1h) | summarize avg(CPU) by bin(TimeGenerated, 5m) | render timechart"}], "type": "Extension/AppInsights"}
          }
        }
      }
    }
  }'
```

## Troubleshooting Common Issues

### Pod Issues

```bash
# Check pod status
kubectl get pods
kubectl describe pod <pod-name>

# Check pod logs
kubectl logs <pod-name>
kubectl logs <pod-name> --previous
kubectl logs <pod-name> --tail=100

# Execute in pod
kubectl exec -it <pod-name> -- /bin/sh
kubectl exec -it <pod-name> -- /bin/bash

# Port forward for debugging
kubectl port-forward <pod-name> 8080:3000
```

### Network Issues

```bash
# Check service endpoints
kubectl get endpoints

# Check DNS resolution
kubectl exec -it <pod-name> -- nslookup myapp.default.svc.cluster.local

# Test connectivity
kubectl exec -it <pod-name> -- curl -v http://myapp:80

# Check network policies
kubectl get networkpolicies
kubectl describe networkpolicy <policy-name>
```

### Node Issues

```bash
# List nodes with conditions
kubectl get nodes -o wide
kubectl describe node <node-name>

# Check node logs
kubectl get events --field-selector involvedObject.name=<node-name>

# Drain node before maintenance
kubectl drain <node-name> --ignore-daemonsets

# Cordon node (mark unschedulable)
kubectl cordon <node-name>

# Uncordon node
kubectl uncordon <node-name>
```

### Cluster Issues

```bash
# Check cluster components
kubectl get componentstatuses
kubectl get cs

# Check API server
kubectl get --raw /healthz

# Get all events
kubectl get events --sort-by='.lastTimestamp'

# Check resource usage
kubectl top nodes
kubectl top pods
```

### Debugging Tips

```bash
# Watch pod status in real-time
kubectl get pods -w

# Watch all resources
watch -n 1 kubectl get all

# Check resource YAML
kubectl get deployment myapp -o yaml

# Check rollout status
kubectl rollout status deployment/myapp

# Check revision history
kubectl rollout history deployment myapp
```

## Upgrading the Cluster

```bash
# Check available Kubernetes versions
az aks get-upgrades \
  --resource-group aks-rg \
  --name myakscluster

# Upgrade cluster
az aks upgrade \
  --resource-group aks-rg \
  --name myakscluster \
  --kubernetes-version 1.28

# Upgrade node pool
az aks nodepool upgrade \
  --resource-group aks-rg \
  --cluster-name myakscluster \
  --name nodepool1 \
  --kubernetes-version 1.28

# Verify upgrade
kubectl get nodes
```

## Backing Up and Restoring

### Velero Setup for Backup

```bash
# Install Velero
brew install velero

# Create storage account
az storage account create \
  --name velero-backups \
  --resource-group aks-rg \
  --sku Standard_LRS

# Get storage key
AZURE_STORAGE_ACCOUNT_KEY=$(az storage account keys list \
  --resource-group aks-rg \
  --account-name velero-backups \
  --query [0].value -o tsv)

# Create credentials file
cat > credentials-velero <<EOF
AZURE_STORAGE_ACCOUNT_ACCESS_KEY=${AZURE_STORAGE_ACCOUNT_KEY}
AZURE_BLOB_CONTAINER_NAME=velero
EOF

# Install Velero on AKS
velero install \
  --provider azure \
  --azure-storage-account-container-name velero \
  --azure-backupstoragelocation-name default \
  --backup-location-config resourceGroup=aks-rg,storageAccount=velero-backups \
  --snapshot-location-config resourceGroup=aks-rs \
  --secret-file ./credentials-velero \
  --plugins velero/velero-plugin-for-microsoft-azure:v1.7.0 \
  --use-volume-snapshots=false
```

### Backup Operations

```bash
# Create backup
velero backup create myapp-backup \
  --include-namespaces default \
  --wait

# List backups
velero backup get

# Create scheduled backup
velero schedule create daily-backup \
  --schedule="0 6 * * *" \
  --include-namespaces default
```

### Restore Operations

```bash
# Restore from backup
velero restore create myapp-restore \
  --from-backup myapp-backup

# List restores
velero restore get

# Check restore status
velero restore describe myapp-restore
```

## Managing Secrets and ConfigMaps

```bash
# Update ConfigMap
kubectl apply -f k8s/configmap.yaml

# Update Secret
kubectl apply -f k8s/secret.yaml

# Edit ConfigMap in place
kubectl edit configmap myapp-config

# Edit Secret in place
kubectl edit secret myapp-secrets

# Verify changes in pod
kubectl exec <pod-name> -- cat /etc/config/log-level
```

## Managing Ingress

```bash
# Install NGINX ingress controller
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.9.4/deploy/static/provider/azure/deploy.yaml

# Create Ingress resource
cat > k8s/ingress.yaml <<EOF
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: myapp-ingress
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /
    nginx.ingress.kubernetes.io/ssl-redirect: "true"
spec:
  ingressClassName: nginx
  rules:
  - host: myapp.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: myapp
            port:
              number: 80
  tls:
  - hosts:
    - myapp.example.com
    secretName: myapp-tls
EOF

kubectl apply -f k8s/ingress.yaml
kubectl get ingress
```

## Cleanup and Maintenance

```bash
# Delete deployment
kubectl delete -f k8s/deployment.yaml

# Delete service
kubectl delete -f k8s/service.yaml

# Scale to zero (for temporary downtime)
kubectl scale deployment myapp --replicas=0

# Delete cluster when no longer needed
az aks delete \
  --resource-group aks-rg \
  --name myakscluster \
  --yes

# Delete resource group
az group delete --name aks-rg --yes
```

## Daily Operational Tasks

### Health Checks

```bash
# 1. Check node health
kubectl get nodes

# 2. Check pod health
kubectl get pods -A

# 3. Check system pods
kubectl get pods -n kube-system

# 4. Check events
kubectl get events --sort-by='.lastTimestamp' | tail -20

# 5. Check component health
kubectl get cs
```

### Monitoring Commands

```bash
# 1. View pod resource usage
kubectl top pods

# 2. View node resource usage
kubectl top nodes

# 3. Check service health
kubectl get endpoints

# 4. View HPA status
kubectl get hpa
```

### Logs and Diagnostics

```bash
# 1. Get application logs
kubectl logs -l app=myapp --tail=100

# 2. Stream logs in real-time
kubectl logs -f -l app=myapp

# 3. Get previous container logs
kubectl logs <pod-name> --previous

# 4. Export logs to file
kubectl logs -l app=myapp > app-logs.txt
```

## Best Practices Checklist

### Deployment Checklist
- [ ] Use appropriate resource limits
- [ ] Configure liveness and readiness probes
- [ ] Use rolling update strategy
- [ ] Set maxSurge and maxUnavailable
- [ ] Use labels for organization

### Security Checklist
- [ ] Use Kubernetes Secrets for sensitive data
- [ ] Configure RBAC properly
- [ ] Use network policies
- [ ] Run containers as non-root
- [ ] Enable Azure AD integration

### Monitoring Checklist
- [ ] Configure Azure Monitor
- [ ] Set up alerts for critical issues
- [ ] Create dashboards
- [ ] Enable log retention

### Backup Checklist
- [ ] Install Velero
- [ ] Configure scheduled backups
- [ ] Test restore procedures
- [ ] Store backups in separate region

## Summary

This practical guide covered:

- Creating AKS clusters with Azure CLI
- Deploying applications using Kubernetes manifests
- Configuring horizontal, vertical, and cluster autoscaling
- Monitoring with Azure Monitor and Log Analytics
- Troubleshooting common AKS issues
- Upgrading Kubernetes clusters
- Backing up and restoring with Velero
- Managing ingress controllers
- Daily operational tasks and health checks
- Best practices for production

These skills prepare you for:
- AZ-104 Azure Administrator exam
- Day-to-day AKS cluster management
- Production deployment and operations
- Troubleshooting and debugging skills

For more advanced topics, refer to 02_Advanced_AKS.md.