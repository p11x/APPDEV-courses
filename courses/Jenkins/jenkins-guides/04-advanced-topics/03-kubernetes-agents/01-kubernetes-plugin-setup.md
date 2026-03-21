# Kubernetes Plugin Setup

## What this covers

This guide explains how to install and configure the Kubernetes Plugin for Jenkins to dynamically provision Jenkins agents as Kubernetes pods. You'll learn about configuring the Kubernetes cloud, pod templates, and why Kubernetes agents are ephemeral.

## Prerequisites

- Kubernetes cluster (self-hosted, EKS, GKE, AKS, etc.)
- Access to configure Kubernetes
- Jenkins admin access

## Why Kubernetes Agents?

### Traditional Setup

```
┌─────────────────────────────────────────────────────────────┐
│  Jenkins Master                                              │
│                                                              │
│  Agent 1 (always running) ─► Waiting for work               │
│  Agent 2 (always running) ─► Waiting for work               │
│  Agent 3 (always running) ─► Waiting for work               │
│                                                              │
│  Problem: Agents sit idle, wasting resources!               │
└─────────────────────────────────────────────────────────────┘
```

### Kubernetes Setup

```
┌─────────────────────────────────────────────────────────────┐
│  Jenkins Master                                              │
│                                                              │
│  ┌─────────────────────────────────────────────────────┐   │
│  │ When build starts:                                  │   │
│  │ - Create pod with agent container                  │   │
│  │ - Run build                                         │   │
│  │ - Delete pod when done                              │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                              │
│  Benefit: Only use resources when needed!                   │
└─────────────────────────────────────────────────────────────┘
```

---

## Installing Kubernetes Plugin

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Search for "Kubernetes"
3. Install **Kubernetes** plugin
4. Restart Jenkins if required

---

## Configuring Kubernetes Cloud

### Step 1: Add Kubernetes Cloud

1. Go to **Manage Jenkins** → **Manage Nodes and Clouds**
2. Click **Configure Clouds**
3. Click **Add a new cloud** → **Kubernetes**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Kubernetes Cloud Details                                          │
│                                                                     │
│  Name:  [ kubernetes ]                                              │
│       ↓                                                             │
│  Kubernetes URL:  [ https://kubernetes.default.svc ]              │
│       ↓                                                             │
│  Kubernetes Namespace:  [ jenkins ]                                │
│       ↓                                                             │
│  Credentials:  [ Add Kubernetes Token ▼ ]                          │
│       ↓                                                             │
│  Jenkins URL:  [ http://jenkins:8080 ]                             │
│       ↓                                                             │
│  Jenkins tunnel:  [ jenkins-agent:50000 ]                          │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Configure Credentials

Add a Kubernetes service account token:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Add Credentials                                                   │
│                                                                     │
│  Kind:  [ Kubernetes service account ]                             │
│       ↓                                                             │
│  OR:                                                               │
│                                                                     │
│  Kind:  [ Secret text ]                                           │
│  Secret:  [ <paste service account token> ]                       │
│  ID:  [ kubernetes-credentials ]                                  │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 3: Configure Pod Templates

Add default pod template:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Pod Templates                                                     │
│  [+ Add Pod Template]                                             │
│                                                                     │
│  Name:  [ jenkins-agent ]                                         │
│  Namespace:  [ jenkins ]                                           │
│  Labels:  [ kubernetes ]                                           │
│       ↓                                                             │
│  Containers:                                                        │
│  [+ Add Container]                                                 │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Name:  [ jnlp ]                                             │ │
│  │ Docker Image:  [ jenkins/inbound-agent:latest ]            │ │
│  │ Working Directory:  [ /home/jenkins/agent ]                 │ │
│  │ Command to run:  [ ]                                        │ │
│  │ Arguments to pass to command:  [ ]                          │ │
│  │ TTY:  [ ] ✓                                                 │ │
│  └───────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Complete Configuration

```yaml
# Example Kubernetes service account for Jenkins
apiVersion: v1
kind: ServiceAccount
metadata:
  name: jenkins
  namespace: jenkins
---
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: jenkins
  namespace: jenkins
rules:
- apiGroups: [""]
  resources: ["pods", "pods/log", "secrets"]
  verbs: ["get", "list", "watch", "create", "delete", "patch"]
- apiGroups: [""]
  resources: ["pods/exec"]
  verbs: ["create"]
---
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: jenkins
  namespace: jenkins
subjects:
- kind: ServiceAccount
  name: jenkins
  namespace: jenkins
roleRef:
  kind: Role
  name: jenkins
  apiGroup: rbac.authorization.k8s.io
```

---

## Testing the Connection

1. After configuring, click **Test Connection**
2. You should see:

```
Connected to Kubernetes v1.28.0
```

---

## Why Ephemeral Agents?

| Traditional Agents | Kubernetes Agents |
|-------------------|-------------------|
| Always running | Created on-demand |
| Persistent workspace | Fresh workspace each build |
| Fixed resources | Scalable resources |
| Pre-provisioned | Dynamic provisioning |

---

## Best Practices

### 1. Use Namespaces

```groovy
agent {
    kubernetes {
        namespace 'jenkins-builds'
        defaultContainer 'jnlp'
    }
}
```

### 2. Set Resource Limits

```yaml
resources:
  limits:
    cpu: "1"
    memory: "1Gi"
  requests:
    cpu: "500m"
    memory: "512Mi"
```

### 3. Use Pod Templates in Pipeline

```groovy
agent {
    kubernetes {
        label 'my-app'
        defaultContainer 'builder'
        yaml '''
            apiVersion: v1
            kind: Pod
            spec:
              containers:
              - name: builder
                image: maven:3.9-eclipse-temurin-17
                command:
                - cat
                tty: true
'''
    }
}
```

---

## Common Configuration Issues

### Issue: Connection Failed

```
Failed to connect to http://kubernetes.default.svc:443
```

**Solution**: Check service account token and RBAC permissions

### Issue: Pod Not Starting

```
Pod jenkins-agent-xxx is Pending
```

**Solution**: Check cluster resources (CPU/memory quotas)

---

## Next Steps

- **[Pod Templates](02-pod-templates.md)** - Define custom pod specs
- **[Pipeline with K8s Agent](03-pipeline-with-k8s-agent.md)** - Use K8s in pipelines
- **[Security Setup](04-security-and-rbac/01-security-realm-setup.md)** - Secure Jenkins
