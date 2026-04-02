# Kubernetes Deployment for Node.js

## What You'll Learn

- How to create Kubernetes deployment manifests for Node.js applications
- How to manage application replicas and scaling
- How to implement rolling updates and rollbacks
- How to configure health checks and readiness probes

---

## Layer 1: Academic Foundation

### Deployment Strategies

Kubernetes supports multiple deployment strategies that determine how application updates are rolled out:

1. **Recreate**: All instances terminated and new ones started. Results in downtime but simple.
2. **Rolling Update**: Gradually replace pods with new versions. Zero downtime.
3. **Blue-Green**: Two identical environments, switch traffic at load balancer level.
4. **Canary**: Deploy to subset of users, gradually increase traffic.

The rolling update strategy uses the following flow:

```
v1 Pods: [P1] [P2] [P3]
         ↓ replace 1/3
v1 Pods: [P1] [P2] [P3]    v2 Pods: [P4]
         ↓ replace 2/3
v1 Pods: [P1] [P2] [P3]    v2 Pods: [P4] [P5]
         ↓ replace 3/3
v1 Pods: [P1] [P2] [P3]    v2 Pods: [P4] [P5] [P6]
         ↓ complete
v2 Pods: [P4] [P5] [P6]
```

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Basic Deployment Manifest

```yaml
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-api-deployment
  labels:
    app: node-api
spec:
  replicas: 3
  selector:
    matchLabels:
      app: node-api
  template:
    metadata:
      labels:
        app: node-api
    spec:
      containers:
        - name: node-api
          image: myregistry/node-api:latest
          ports:
            - containerPort: 3000
          resources:
            requests:
              memory: "256Mi"
              cpu: "250m"
            limits:
              memory: "512Mi"
              cpu: "500m"
```

### Paradigm 2 — Deployment with Liveness/Readiness Probes

```yaml
# deployment-with-probes.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-api-deployment
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  selector:
    matchLabels:
      app: node-api
  template:
    metadata:
      labels:
        app: node-api
    spec:
      containers:
        - name: node-api
          image: myregistry/node-api:latest
          ports:
            - containerPort: 3000
          livenessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 30
            periodSeconds: 10
            timeoutSeconds: 5
            failureThreshold: 3
          readinessProbe:
            httpGet:
              path: /ready
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 5
            timeoutSeconds: 3
            failureThreshold: 3
          env:
            - name: NODE_ENV
              value: "production"
            - name: PORT
              value: "3000"
```

### Paradigm 3 — Imperative Deployment Commands

```bash
# Create deployment
kubectl create deployment node-api --image=myregistry/node-api:latest --replicas=3

# Scale deployment
kubectl scale deployment node-api --replicas=5

# Update image
kubectl set image deployment/node-api node-api=myregistry/node-api:v2.0.0

# Rollback
kubectl rollout undo deployment/node-api

# Check rollout status
kubectl rollout status deployment/node-api
```

### Paradigm 4 — Helm Chart for Node.js

```yaml
# Chart.yaml
apiVersion: v2
name: node-api
description: A Node.js API Helm chart
version: 1.0.0
appVersion: "1.0.0"

# values.yaml
replicaCount: 3

image:
  repository: myregistry/node-api
  tag: latest
  pullPolicy: IfNotPresent

service:
  type: ClusterIP
  port: 3000

ingress:
  enabled: true
  className: nginx
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
  hosts:
    - host: api.example.com
      paths:
        - path: /
          pathType: Prefix

resources:
  limits:
    cpu: 500m
    memory: 512Mi
  requests:
    cpu: 250m
    memory: 256Mi

autoscaling:
  enabled: true
  minReplicas: 2
  maxReplicas: 10
  targetCPUUtilizationPercentage: 70
```

---

## Layer 3: Performance Engineering Lab

### Rolling Update Performance

| Strategy | Downtime | Rollback Time | Resource Usage |
|----------|----------|---------------|----------------|
| Recreate | Yes | Fast | Low |
| RollingUpdate | No | Gradual | Medium |
| Blue-Green | No | Instant | High |
| Canary | No | Gradual | Medium |

### Profiling Container Performance

```typescript
// metrics.ts - Expose Prometheus metrics from Node.js
import client from 'prom-client';

const register = new client.Registry();

register.setDefaultLabels({
  app: 'node-api'
});

client.collectDefaultMetrics({ register });

const httpRequestDuration = new client.Histogram({
  name: 'http_request_duration_seconds',
  help: 'Duration of HTTP requests in seconds',
  labelNames: ['method', 'route', 'status_code'],
  buckets: [0.01, 0.05, 0.1, 0.5, 1, 5]
});

register.registerMetric(httpRequestDuration);

export { register, httpRequestDuration };
```

---

## Layer 4: Zero-Trust Security Architecture

### Pod Security Standards

```yaml
# pod-security-policy.yaml
apiVersion: policy/v1beta1
kind: PodSecurityPolicy
metadata:
  name: restricted-psp
spec:
  privileged: false
  allowPrivilegeEscalation: false
  requiredDropCapabilities:
    - ALL
  volumes:
    - configMap
    - secret
    - emptyDir
  runAsUser:
    rule: MustRunAsNonRoot
  seLinux:
    rule: RunAsAny
  supplementalGroups:
    rule: RunAsAny
  fsGroup:
    rule: RunAsAny
```

### Security Best Practices

- Never run containers as root (runAsNonRoot: true)
- Use read-only root filesystem when possible
- Implement resource limits to prevent DoS
- Use network policies to restrict traffic
- Enable RBAC for fine-grained access control

---

## Layer 5: AI-Enhanced Testing Ecosystem

### Deployment Validation Tests

```typescript
// deployment-test.ts
import { k8s } from './kubernetes-client';

interface DeploymentSpec {
  name: string;
  replicas: number;
  image: string;
}

async function validateDeployment(spec: DeploymentSpec): Promise<{
  valid: boolean;
  issues: string[];
}> {
  const issues: string[] = [];
  
  const deployment = await k8s.apps.readNamespacedDeployment(spec.name, 'default');
  
  if (deployment.spec?.replicas !== spec.replicas) {
    issues.push(`Replica count mismatch: expected ${spec.replicas}, got ${deployment.spec?.replicas}`);
  }
  
  if (!deployment.spec?.template?.spec?.containers?.[0]?.image?.includes(spec.image)) {
    issues.push(`Image mismatch: expected ${spec.image}`);
  }
  
  const pods = await k8s.core.listNamespacedPod('default', {
    labelSelector: `app=${spec.name}`
  });
  
  const readyReplicas = pods.items.filter(p => 
    p.status.conditions?.some(c => c.type === 'Ready' && c.status === 'True')
  ).length;
  
  if (readyReplicas < spec.replicas) {
    issues.push(`Not all replicas ready: ${readyReplicas}/${spec.replicas}`);
  }
  
  return {
    valid: issues.length === 0,
    issues
  };
}
```

---

## Layer 6: DevOps & SRE Operations Center

### Deployment SLIs

| Metric | Description | Target |
|--------|-------------|--------|
| Deployment Success Rate | % of successful deployments | 100% |
| Deployment Duration | Time from trigger to complete | < 5 min |
| Time to Traffic | Time to start serving traffic | < 30s |
| Rollback Duration | Time to rollback | < 2 min |

### Deployment Dashboard

```yaml
# Grafana dashboard JSON excerpt
{
  "panels": [
    {
      "title": "Deployment Success Rate",
      "type": "stat",
      "targets": [
        {
          "expr": "sum(rate(kubernetes_deployment_status_replicas_updated_total[5m])) / sum(rate(kubernetes_deployment_status_replicas_updated_total[5m]))"
        }
      ]
    },
    {
      "title": "Pod Restarts",
      "type": "graph",
      "targets": [
        {
          "expr": "sum(rate(kubernetes_pod_container_status_restarts_total[5m])) by (pod)"
        }
      ]
    }
  ]
}
```

---

## Layer 7: Advanced Learning Analytics

### Knowledge Graph

- **Prerequisites**: Kubernetes setup, Docker fundamentals
- **Related Topics**: Services, Ingress, ConfigMaps, Secrets
- **Career Mapping**: Platform Engineer, Site Reliability Engineer

### Self-Assessment Quiz

1. What's the difference between liveness and readiness probes?
2. How do you perform a rolling update with zero downtime?
3. What strategies can you use to rollback a deployment?

---

## Diagnostic Center

### Common Deployment Issues

| Issue | Symptom | Solution |
|-------|---------|----------|
| ImagePullBackOff | Pod stuck pending | Check image name, registry credentials |
| CrashLoopBackOff | Container restarting | Check logs, verify startup command |
| Evicted | Pod terminated due to resource pressure | Increase node resources, adjust limits |
| Unschedulable | Pod cannot be scheduled | Check node capacity, taints |

---

## Next Steps

Continue to [Kubernetes Service](./03-k8s-service.md) to learn about service discovery and load balancing.