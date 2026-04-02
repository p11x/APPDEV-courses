# Kubernetes Service

## What You'll Learn

- How to create and manage Kubernetes services
- How to configure different service types
- How to implement service discovery
- How to set up load balancing for Node.js applications

---

## Layer 1: Academic Foundation

### Service Types in Kubernetes

| Type | Description | Use Case |
|------|-------------|----------|
| ClusterIP | Internal cluster IP | Internal service communication |
| NodePort | Exposes on each node's IP | Development, testing |
| LoadBalancer | External load balancer | Cloud provider integration |
| ExternalName | DNS CNAME alias | External service mapping |

### Network Architecture

Services in Kubernetes use iptables or IPVS for packet forwarding. The kube-proxy component watches the API server for service and endpoint changes, updating routing rules accordingly.

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — ClusterIP Service

```yaml
# service-clusterip.yaml
apiVersion: v1
kind: Service
metadata:
  name: node-api-service
spec:
  type: ClusterIP
  selector:
    app: node-api
  ports:
    - name: http
      port: 80
      targetPort: 3000
      protocol: TCP
```

### Paradigm 2 — NodePort Service

```yaml
# service-nodeport.yaml
apiVersion: v1
kind: Service
metadata:
  name: node-api-nodeport
spec:
  type: NodePort
  selector:
    app: node-api
  ports:
    - port: 80
      targetPort: 3000
      nodePort: 30080
      protocol: TCP
```

### Paradigm 3 — LoadBalancer Service

```yaml
# service-loadbalancer.yaml
apiVersion: v1
kind: Service
metadata:
  name: node-api-lb
  annotations:
    service.beta.kubernetes.io/aws-load-balancer-type: "nlb"
spec:
  type: LoadBalancer
  selector:
    app: node-api
  ports:
    - port: 80
      targetPort: 3000
      protocol: TCP
  loadBalancerSourceRanges:
    - 10.0.0.0/8
```

### Paradigm 4 — Headless Service

```yaml
# service-headless.yaml
apiVersion: v1
kind: Service
metadata:
  name: node-api-headless
spec:
  clusterIP: None
  selector:
    app: node-api
  ports:
    - port: 80
      targetPort: 3000
```

---

## Layer 3: Performance Engineering Lab

### Service Performance Metrics

| Metric | Description | Target |
|--------|-------------|--------|
| Service Latency | End-to-end request latency | < 50ms p99 |
| Connection Pooling | Backend connections | Optimized per node |
| Packet Loss | Network packet loss | < 0.1% |

---

## Layer 4: Zero-Trust Security Architecture

### Network Policies

```yaml
# network-policy.yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: node-api-policy
spec:
  podSelector:
    matchLabels:
      app: node-api
  policyTypes:
    - Ingress
    - Egress
  ingress:
    - from:
        - podSelector:
            matchLabels:
              app: frontend
      ports:
        - port: 3000
  egress:
    - to:
        - podSelector:
            matchLabels:
              app: database
      ports:
        - port: 5432
```

---

## Layer 5: Testing Ecosystem

### Service Connectivity Tests

```typescript
// service-test.ts
import { k8s } from './kubernetes-client';

async function testServiceConnectivity(serviceName: string, namespace: string) {
  const service = await k8s.core.readNamespacedService(serviceName, namespace);
  const endpoints = await k8s.core.readNamespacedEndpoints(serviceName, namespace);
  
  const readyEndpoints = endpoints.subsets?.reduce((count, subset) => {
    return count + (subset.addresses?.length || 0);
  }, 0) || 0;
  
  return {
    serviceType: service.spec?.type,
    clusterIP: service.spec?.clusterIP,
    readyEndpoints,
    ports: service.spec?.ports?.map(p => p.port)
  };
}
```

---

## Layer 6: DevOps Operations

### Service Monitoring

```yaml
# service-monitor.yaml
apiVersion: monitoring.coreos.com/v1
kind: ServiceMonitor
metadata:
  name: node-api-monitor
spec:
  selector:
    matchLabels:
      app: node-api
  endpoints:
    - port: http
      path: /metrics
```

---

## Layer 7: Learning Analytics

### Knowledge Graph

- **Prerequisites**: Kubernetes deployment basics
- **Related Topics**: Ingress, EndpointSlices, CoreDNS

---

## Next Steps

Continue to [Kubernetes Ingress](./04-k8s-ingress.md) for external access configuration.