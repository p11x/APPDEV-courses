---
title: "Azure Kubernetes Service (AKS) - Advanced"
category: Azure Certification
subcategory: Azure Core Services
concept: AKS Kubernetes
purpose: Advanced AKS concepts for enterprise workloads
difficulty: advanced
prerequisites:
  - 01_Basic_AKS.md
relatedFiles:
  - 01_Basic_AKS.md
  - 03_Practical_AKS.md
useCase: Enterprise Kubernetes
certificationExam: AZ-305 Azure Solutions Architect
lastUpdated: 2025
---

# Azure Kubernetes Service (AKS) - Advanced Concepts

## Overview

This guide covers advanced AKS topics essential for enterprise Kubernetes deployments, including networking, autoscaling, security integration, and architectural considerations for production workloads.

## Azure CNI Networking

### Understanding Azure CNI

Azure Container Networking Interface (CNI) provides advanced networking capabilities for AKS clusters:

- Pods get IP addresses from the Azure virtual network subnet
- Direct communication between pods without NAT
- Integration with Azure network security groups
- Support for network policies

### Configuring Azure CNI

```bash
# Create VNet first
az network vnet create \
  --resource-group myResourceGroup \
  --name myVNet \
  --address-prefixes 10.0.0.0/8 \
  --subnet-name mySubnet \
  --subnet-prefixes 10.1.0.0/16

# Create AKS with Azure CNI
az aks create \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --network-plugin azure \
  --vnet-subnet-id /subscriptions/<sub-id>/resourceGroups/myResourceGroup/providers/Microsoft.Network/virtualNetworks/myVNet/subnets/mySubnet \
  --service-cidr 10.2.0.0/16 \
  --dns-service-ip 10.2.0.10 \
  --docker-bridge-address 172.17.0.1/16
```

### Azure CNI Dynamic Allocation

```bash
# Enable Azure CNI with dynamic IP allocation
az aks create \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --network-plugin azure \
  --pod-cidr 10.244.0.0/16 \
  --service-cidr 10.2.0.0/16 \
  --dns-service-ip 10.2.0.10 \
  --enable-managed-identity
```

### Network Policy Configuration

```yaml
# Network Policy - Deny all traffic by default
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: deny-all
spec:
  podSelector: {}
  policyTypes:
  - Ingress
  - Egress
---
# Network Policy - Allow specific traffic
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: allow-app
spec:
  podSelector:
    matchLabels:
      app: allowed-app
  policyTypes:
  - Ingress
  ingress:
  - from:
    - podSelector:
        matchLabels:
          app: frontend
    ports:
    - protocol: TCP
      port: 80
```

## Cluster Autoscaling

### Cluster Autoscaler Setup

```bash
# Enable cluster autoscaler on node pools
az aks nodepool update \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster \
  --name nodepool1 \
  --enable-cluster-autoscaler \
  --min-nodes 1 \
  --max-nodes 10
```

### Autoscaler Configuration

```yaml
# ClusterAutoscaler configuration
apiVersion: autoscaling.k8s.io/v1
kind: ClusterAutoscaler
metadata:
  name: cluster-autoscaler
spec:
  scaleDown:
    enabled: true
    delayAfterAdd: 10m
    delayAfterDelete: 10m
    delayAfterFailure: 3m
  expander: priority
  maxNodeProvisionTime: 15m
  scanInterval: 10s
```

### Vertical Pod Autoscaler

```bash
# Install Vertical Pod Autoscaler
git clone https://github.com/kubernetes/autoscaler.git
cd autoscaler/vertical-pod-autoscaler

# Deploy VPA components
kubectl apply -f deployments/vpa-components.yaml
```

```yaml
# Vertical Pod Autoscaler recommendation
apiVersion: "autoscaling.k8s.io/v1"
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
      controlledResources: ["CPU", "Memory"]
```

### KEDA Integration for Event-Driven Scaling

```bash
# Add KEDA to AKS
kubectl delete namespace keda
kubectl apply -f https://kedacore/scalesystems/keda-2.15.1-core.yaml

# Create ScaledObject for KEDA
kubectl apply -f - <<EOF
apiVersion: keda.sh/v1alpha1
kind: ScaledObject
metadata:
  name: myapp-scaledobject
  namespace: default
spec:
  scaleTargetRef:
    name: myapp
  pollingInterval: 30
  cooldownPeriod: 300
  minReplicaCount: 0
  maxReplicaCount: 100
  triggers:
  - type: azure-queue
    metadata:
      connectionFromEnv: AzureWebJobsStorage
      queueName: myqueue
      queueLength: "5"
EOF
```

## Azure AD Integration

### Configure Azure AD for AKS

```bash
# Create Azure AD application
az ad app create \
  --display-name myAKSCluster

# Create server application
SERVER_APP_ID=$(az ad app create \
  --display-name myAKSServer \
  --identifier-uris https://myakscluster \
  --query appId -o tsv)

# Create client application
CLIENT_APP_ID=$(az ad app create \
  --display-name myAKSClient \
  --reply-urls https://myakscluster/oauth2callback \
  --query appId -o tsv)

# Set required resource access
az ad app permission add \
  --id $SERVER_APP_ID \
  --api 00000003-0000-0000-c000-000000000000 \
  --api-permissions e1fe6dd8-ba31-4d61-89ae-1d0d9a5fb395=Scope

# Grant permissions
az ad app permission grant \
  --id $CLIENT_APP_ID \
  --api $SERVER_APP_ID

# Make AKS cluster use Azure AD
az aks create \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --enable-aad \
  --aad-admin-group-object-ids <group-object-id> \
  --aad-tenant-id <tenant-id>
```

### Azure AD Authentication with kubectl

```bash
# Install kubelogin for Azure AD authentication
az aks install-cli

# Login with Azure AD
kubelogin convert-kubeconfig -l azurecli

# Or use managed identity
kubelogin convert-kubeconfig -l managedidentity
```

### RBAC with Azure AD

```yaml
# Kubernetes RoleBinding with Azure AD group
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: app-developers
  namespace: default
subjects:
- kind: Group
  name: app-developers@mytenant.onmicrosoft.com
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole
  name: edit
  apiGroup: rbac.authorization.k8s.io
---
# ClusterRoleBinding for admins
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: aks-admins
subjects:
- kind: Group
  name: aks-admins@mytenant.onmicrosoft.com
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole
  name: cluster-admin
  apiGroup: rbac.authorization.k8s.io
```

## Enterprise Security

### Pod Security Standards

```yaml
# Pod Security Standards - Baseline
apiVersion: apps/v1
kind: Deployment
metadata:
  name: secure-app
spec:
  template:
    metadata:
      labels:
        app: secure-app
    spec:
      securityContext:
        runAsNonRoot: true
        runAsUser: 1000
        fsGroup: 2000
      containers:
      - name: app
        image: myapp:latest
        securityContext:
          allowPrivilegeEscalation: false
          readOnlyRootFilesystem: true
          runAsNonRoot: true
          capabilities:
            drop:
            - ALL
```

### Azure Key Vault Integration

```bash
# Enable secrets store CSI driver
az aks enable-addons \
  --addons azure-keyvault-secrets-provider \
  --resource-group myResourceGroup \
  --name myAKSCluster

# Check if addon is enabled
az aks show \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --query addonProfiles.azureKeyvaultSecretsProvider
```

```yaml
# AzureKeyVaultSecret
apiVersion: secrets-store.csi.x-k8s.io/v1
kind: AzureKeyVaultSecret
metadata:
  name: myapp-secrets
  namespace: default
spec:
  vaultUri: https://myvault.vault.azure.net/
  objects:
  - objectName: db-password
    objectType: secret
    secret VERSION: latest
  - objectName: connection-string
    objectType: key
    secret VERSION: latest
  authSecret:
    name: kv-auth-secret
    namespace: default
---
# Pod using Key Vault secrets
apiVersion: v1
kind: Pod
metadata:
  name: myapp
spec:
  containers:
  - name: myapp
    image: myapp:latest
    volumeMounts:
    - name: secrets
      mountPath: /mnt/secrets
      readOnly: true
  volumes:
  - name: secrets
    csi:
      driver: secrets-store.csi.x-k8s.io
      readOnly: true
      volumeAttributes:
        secretProviderClass: azure-kv
```

### Network Policies for Security

```yaml
# Restrict ingress to specific namespaces
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: ingress-egress-policy
  namespace: production
spec:
  podSelector:
    matchLabels:
      app: production-app
  policyTypes:
  - Ingress
  - Egress
  ingress:
  - from:
    - namespaceSelector:
        matchLabels:
          name: production
    - podSelector:
        matchLabels:
          app: ingress-controller
    ports:
    - protocol: TCP
      port: 80
  egress:
  - to:
    - namespaceSelector:
        matchLabels:
          name: production
    ports:
    - protocol: TCP
      port: 5432
  - to:
    - namespaceSelector: {}
      ports:
      - protocol: TCP
        port: 53
      - protocol: UDP
        port: 53
```

### Azure Policies for AKS

```bash
# Enable Azure Policy add-on
az aks enable-addons \
  --addons azure-policy \
  --resource-group myResourceGroup \
  --name myAKSCluster

# Assign built-in policy
az policy assignment create \
  --name "Kubernetes cluster containers should only use allowed images" \
  --scope /subscriptions/<sub-id>/resourceGroups/myResourceGroup \
  --policy "6c1e3d76-0781-4b1a-b5a4-a4f1f26ccd78"
```

## High Availability

### Availability Zones

```bash
# Create AKS with availability zones
az aks create \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --node-count 6 \
  --zones 1 2 3 \
  --vm-set-type VirtualMachineScaleSets
```

### Pod Disruption Budgets

```yaml
# PodDisruptionBudget
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: myapp-pdb
spec:
  minAvailable: 2
  selector:
    matchLabels:
      app: myapp
---
# Example with maxUnavailable
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: myapp-pdb
spec:
  maxUnavailable: 1
  selector:
    matchLabels:
      app: myapp
```

### Planning for Planned Maintenance

```yaml
# Node Maintenance Window
apiVersion: v1
kind: ConfigMap
metadata:
  name: node-maintenance-config
  namespace: kube-system
data:
  # This will be ignored - maintenance windows configured via Azure Portal or API
```

## Advanced Storage

### Azure Disk Storage Class

```yaml
# StorageClass for Azure Disk
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: managed-premium-retain
provisioner: kubernetes.io/azure-disk
parameters:
  storageAccountType: Premium_LRS
  kind: Managed
reclaimPolicy: Retain
volumeBindingMode: WaitForFirstConsumer
allowVolumeExpansion: true
```

### Azure File Storage Class

```yaml
# StorageClass for Azure Files
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: azurefile-premium
provisioner: kubernetes.io/azure-file
parameters:
  skuName: Premium_LRS
  kind: Managed
  storageAccount: mystorageaccount
mountOptions:
  - dir_mode=0777
  - file_mode=0777
  - uid=1000
  - gid=1000
reclaimPolicy: Retain
volumeBindingMode: WaitForFirstConsumer
```

### Using Azure Files with Persistent Volume Claim

```yaml
# PVC using Azure Files
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: myapp-pvc-azurefile
spec:
  accessModes:
    - ReadWriteMany
  storageClassName: azurefile-premium
  resources:
    requests:
      storage: 5Gi
```

### StatefulSet with Persistent Storage

```yaml
# StatefulSet
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: mysql
spec:
  serviceName: mysql
  replicas: 3
  selector:
    matchLabels:
      app: mysql
  template:
    metadata:
      labels:
        app: mysql
    spec:
      containers:
      - name: mysql
        image: mysql:8.0
        ports:
        - containerPort: 3306
          name: mysql
        volumeMounts:
        - name: data
          mountPath: /var/lib/mysql
  volumeClaimTemplates:
  - metadata:
      name: data
    spec:
      accessModes: ["ReadWriteOnce"]
      storageClassName: managed-premium
      resources:
        requests:
          storage: 10Gi
```

## Advanced Networking

### Ingress Controller with TLS

```bash
# Install NGINX ingress controller
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.9.4/deploy/static/provider/azure/deploy.yaml

# Add Helm repo
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

# Install with Helm
helm install ingress-nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  --create-namespace \
  --set controller.replicaCount=2 \
  --set controller.service.annotations.service\.beta\.kubernetes\.io/azure-load-balancer-health-probe-request-path=/healthz
```

### TLS Certificate with Let's Encrypt

```yaml
# ClusterIssuer for Let's Encrypt
apiVersion: cert-manager.io/v1
kind: ClusterIssuer
metadata:
  name: letsencrypt-prod
spec:
  acme:
    server: https://acme-v02.api.letsencrypt.org/directory
    email: admin@example.com
    privateKeySecretRef:
      name: letsencrypt-prod
    solvers:
    - http01:
        ingress:
          class: nginx
---
# Ingress with TLS
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: myapp-ingress
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
spec:
  tls:
  - hosts:
    - myapp.example.com
    secretName: myapp-tls
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
```

### Service Mesh with Istio

```bash
# Install Istio
curl -L https://istio.io/downloadIstio | sh -
export PATH=$PATH:$HOME/istio-1.24/bin

istioctl install --set profile=default

# Enable Istio injection
kubectl label namespace default istio-injection=enabled
```

```yaml
# Istio VirtualService
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: myapp
spec:
  hosts:
  - myapp
  gateways:
  - myapp-gateway
  http:
  - match:
    - headers:
        version:
          exact: v2
    route:
    - destination:
        host: myapp-v2
        port:
          number: 80
  - route:
    - destination:
        host: myapp-v1
        port:
          number: 80
---
# DestinationRule
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: myapp
spec:
  host: myapp
  trafficPolicy:
    connectionPool:
      tcp:
        maxConnections: 100
      http:
        h2UpgradePolicy: UPGRADE
        http2MaxRequests: 1000
    loadBalancer:
      simple: ROUND_ROBIN
    outlierDetection:
      consecutive5xxErrors: 5
      interval: 30s
      baseEjectionTime: 30s
```

## GitOps with AKS

### ArgoCD Setup

```bash
# Install ArgoCD
kubectl create namespace argocd
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

# Get initial password
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d

# Access ArgoCD UI
kubectl port-forward -n argocd svc/argocd-server 8080:443
```

```yaml
# ArgoCD Application
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: myapp
  namespace: argocd
spec:
  project: default
  source:
    repoURL: https://github.com/myorg/myapp.git
    targetRevision: HEAD
    path: k8s
  destination:
    server: https://kubernetes.default.svc
    namespace: default
  syncPolicy:
    automated:
      prune: true
      selfHeal: true
```

### Flux CD Setup

```bash
# Install Flux
flux install --components=source-controller,helm-controller,kustomize-controller

# Create GitRepository
flux create source git myapp \
  --url=https://github.com/myorg/myapp \
  --branch=main

# Create Kustomization
flux create kustomization myapp \
  --source=myapp \
  --path=./k8s \
  --prune=true \
  --interval=10m
```

## AKS vs EKS vs GKE Comparison

| Feature | Azure AKS | Amazon EKS | Google GKE |
|---------|-----------|-----------|------------|
| **Managed Control Plane** | Yes | Yes | Yes |
| **Control Plane Pricing** | Free | $72/month | Free (Standard) / $10800/month (Autopilot) |
| **Node Pricing** | Pay for VMs | Pay for VMs | Pay for VMs |
| **Free Tier** | 1 year (750 hours) | 12 months | Always Free tier |
| **Regions** | 60+ | 30+ | 40+ |
| **Kubernetes Versions** | Up to 1.28 | Up to 1.28 | Up to 1.28 |
| **Supported VM Sizes** | D, E, F, M, N, BS | M, R, C, T, M | E2, N2, N2D, C2 |
| **Node Pools** | Yes (multiple) | Yes (multiple) | Yes (multiple) |
| **Autoscaling** | Cluster Autoscaler, KEDA | Cluster Autoscaler, KEDA | Cluster Autoscaler |
| **Serverless** | Azure Container Instances | AWS Fargate | Cloud Run |
| **Private Clusters** | Yes (Private Link) | Yes (PrivateLink) | Yes (Private Cluster) |
| **Network Plugins** | Azure CNI, Kubenet | VPC CNI, Kube Router | GKE Dataplane, Kubenet |
| **Service Mesh** | Open Service Mesh, Istio | AWS App Mesh, Istio | Anthos Service Mesh, Istio |
| **Azure AD Integration** | Native | AWS IAM | Google Cloud IAM |
| **Monitoring** | Azure Monitor, Prometheus | CloudWatch, Prometheus | Cloud Monitoring, Prometheus |
| **Logging** | Azure Log Analytics | CloudWatch Logs | Cloud Logging |
| **Storage Options** | Azure Disk, Azure Files | EBS, EFS | Persistent Disk, Filestore |
| **GPU Support** | Yes (NC, ND series) | Yes (P, G series) | Yes (A100, TPU) |
| **Windows Containers** | Yes | Yes | Limited |
| **Confidential Computing** | Yes (AMD SEV) | Yes (Nitro) | Yes (c2d VMs) |
| **Compliance** | SOC, FedRAMP, HIPAA | SOC, FedRAMP, HIPAA | SOC, FedRAMP, HIPAA |
| **Uptime SLA** | 99.5% - 99.9% | 99.9% | 99.95% - 99.99% |
| **Azure Integration** | Native Azure services | AWS services | GCP services |
| **GitOps Support** | ArgoCD, Flux | ArgoCD, Flux | Config Sync, Flux |
| **Helm Support** | Yes | Yes | Yes |
| **Multi-cluster** | Azure Arc enabled | EKS Anywhere, Fargate | GKE Hub, Anthos |
| **Cost Management** | Azure Cost Management | AWS Cost Explorer | Cloud Billing |

### Decision Factors

**Choose AKS when:**
- Using other Azure services
- Need Windows container support
- Azure AD integration is required
- Using Microsoft workloads

**Choose EKS when:**
- AWS ecosystem integration needed
- AWS Fargate serverless option
- Lambda integration for functions
- AWS CloudMap for service discovery

**Choose GKE when:**
- GCP ecosystem preferred
- Need Autopilot mode
- Require zero management
- Strong ML/AI workload needs

## Disaster Recovery

### Backup and Restore with Velero

```bash
# Install Velero
velero install \
  --provider azure \
  --bucket <blob-container> \
  --secret-file ./credentials-velero \
  --backup-location-config resourceGroup=<rg>,storageAccount=<storage-account> \
  --volume-snapshot-locations-config resourceGroup=<rg>

# Create backup
velero backup create myapp-backup --include-namespaces default

# Restore backup
velero restore create --from-backup myapp-backup
```

### Multi-region Deployment

```yaml
# Frontend deployment with topology spread
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp
spec:
  replicas: 10
  template:
    spec:
      topologySpreadConstraints:
      - maxSkew: 1
        topologyKey: topology.kubernetes.io/zone
        whenUnsatisfiable: DoNotSchedule
        labelSelector:
          matchLabels:
            app: myapp
      - maxSkew: 1
        topologyKey: kubernetes.io/hostname
        whenUnsatisfiable: ScheduleAnyway
        labelSelector:
          matchLabels:
            app: myapp
```

## Performance Optimization

### Optimizing Pod Resources

```yaml
# Optimized Pod configuration
apiVersion: apps/v1
kind: Deployment
metadata:
  name: optimized-app
spec:
  replicas: 5
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  selector:
    matchLabels:
      app: optimized-app
  template:
    metadata:
      labels:
        app: optimized-app
    spec:
      affinity:
        podAntiAffinity:
          preferredDuringSchedulingIgnoredDuringExecution:
          - weight: 100
            podAffinityTerm:
              labelSelector:
                matchLabels:
                  app: optimized-app
              topologyKey: kubernetes.io/hostname
      topologySpreadConstraints:
      - maxSkew: 1
        topologyKey: topology.kubernetes.io/zone
        whenUnsatisfiable: DoNotSchedule
        labelSelector:
          matchLabels:
            app: optimized-app
      containers:
      - name: app
        image: myapp:latest
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /healthz
            port: 8080
          initialDelaySeconds: 10
          periodSeconds: 20
        readinessProbe:
          httpGet:
            path: /ready
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 10
```

### Profiling and Monitoring

```yaml
# Prometheus ServiceMonitor
apiVersion: monitoring.coreos.com/v1
kind: ServiceMonitor
metadata:
  name: myapp-monitor
spec:
  selector:
    matchLabels:
      app: myapp
  endpoints:
  - port: metrics
    path: /metrics
    interval: 15s
---
# PrometheusRule for alerts
apiVersion: monitoring.coreos.com/v1
kind: PrometheusRule
metadata:
  name: myapp-alerts
spec:
  groups:
  - name: myapp.rules
    rules:
    - alert: HighMemoryUsage
      expr: (container_memory_working_set_bytes / container_spec_memory_limit_bytes) > 0.9
      for: 5m
      labels:
        severity: warning
      annotations:
        summary: High memory usage detected
    - alert: HighCPUUsage
      expr: (rate(container_cpu_usage_seconds_total[5m]) > 0.8)
      for: 5m
      labels:
        severity: warning
      annotations:
        summary: High CPU usage detected
```

## Cost Optimization

### Resource Quotas and Limits

```yaml
# Namespace ResourceQuota
apiVersion: v1
kind: ResourceQuota
metadata:
  name: production-quota
  namespace: production
spec:
  hard:
    requests.cpu: "20"
    requests.memory: 40Gi
    limits.cpu: "40"
    limits.memory: 80Gi
    pods: "50"
    persistentvolumeclaims: "20"
---
# LimitRange
apiVersion: v1
kind: LimitRange
metadata:
  name: default-limits
  namespace: production
spec:
  limits:
  - default:
      memory: 512Mi
      cpu: "500m"
    defaultRequest:
      memory: 256Mi
      cpu: "250m"
    max:
      memory: 4Gi
      cpu: "4"
    min:
      memory: 128Mi
      cpu: "100m"
    type: Container
```

### Spot/Preemptible Nodes

```bash
# Create node pool with Spot VMs
az aks nodepool add \
  --resource-group myResourceGroup \
  --cluster-name myAKSCluster \
  --name spotpool \
  --enable-cluster-autoscaler \
  --min-nodes 0 \
  --max-nodes 10 \
  --eviction-policy Delete \
  --spot-max-price -1 \
  --node-vm-size Standard_D4s_v3
```

## Summary

This advanced guide covered:

- Azure CNI networking configuration
- Cluster autoscaling and VPA
- Azure AD integration for authentication
- Enterprise security (Key Vault, Network Policies)
- High availability with Availability Zones
- Advanced storage configurations
- Service mesh with Istio
- GitOps with ArgoCD and Flux
- AKS vs EKS vs GKE comparison
- Disaster recovery strategies
- Performance optimization
- Cost optimization techniques

These skills are essential for:
- AZ-305 Azure Solutions Architect exam
- Designing enterprise Kubernetes architectures
- Managing production AKS clusters
- Implementing zero-trust security models

Next steps in 03_Practical_AKS.md will cover hands-on management tasks.