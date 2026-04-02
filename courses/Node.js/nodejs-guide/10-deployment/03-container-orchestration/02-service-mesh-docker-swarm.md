# Service Mesh and Docker Swarm

## What You'll Learn

- Istio service mesh installation and configuration
- Traffic management with VirtualService and DestinationRule
- Linkerd lightweight service mesh
- Docker Swarm orchestration
- Sidecar, ambassador, and adapter patterns
- Service discovery mechanisms

## Service Mesh Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Service Mesh (Istio)                  │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐    ┌──────────┐    ┌──────────┐          │
│  │ Service A│    │ Service B│    │ Service C│          │
│  │ ┌──────┐ │    │ ┌──────┐ │    │ ┌──────┐ │          │
│  │ │Node.js│ │    │ │Node.js│ │    │ │Node.js│ │          │
│  │ └──┬───┘ │    │ └──┬───┘ │    │ └──┬───┘ │          │
│  │ ┌──┴───┐ │    │ ┌──┴───┐ │    │ ┌──┴───┐ │          │
│  │ │Envoy │◄├────┤►│Envoy │◄├────┤►│Envoy │ │          │
│  │ │Proxy │ │    │ │Proxy │ │    │ │Proxy │ │          │
│  │ └──────┘ │    │ └──────┘ │    │ └──────┘ │          │
│  └──────────┘    └──────────┘    └──────────┘          │
│        ▲               ▲               ▲               │
│        └───────────────┼───────────────┘               │
│                   mTLS Encrypted                        │
│                                                         │
│  ┌─────────────────────────────────────────────────┐   │
│  │           Istiod (Control Plane)                 │   │
│  │  • Certificate Authority  • Config Distribution  │   │
│  │  • Service Discovery      • Policy Enforcement   │   │
│  └─────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

## Istio Installation

### Installing Istio with istioctl

```bash
# Download Istio
curl -L https://istio.io/downloadIstio | sh -
cd istio-*
export PATH=$PWD/bin:$PATH

# Verify installation
istioctl version

# Install Istio with production profile
istioctl install --set profile=default -y

# Verify installation
istioctl verify-install

# Enable sidecar injection for a namespace
kubectl label namespace my-app istio-injection=enabled

# Verify sidecar injection
kubectl get namespace -L istio-injection
```

### Istio Operator Configuration

```yaml
# istio/operator.yaml
apiVersion: install.istio.io/v1alpha1
kind: IstioOperator
metadata:
  name: istio-control-plane
spec:
  profile: default
  meshConfig:
    accessLogFile: /dev/stdout
    accessLogEncoding: JSON
    enableTracing: true
    defaultConfig:
      tracing:
        sampling: 100
      holdApplicationUntilProxyStarts: true
    outboundTrafficPolicy:
      mode: REGISTRY_ONLY
  components:
    pilot:
      k8s:
        resources:
          requests:
            cpu: 500m
            memory: 2Gi
          limits:
            cpu: 1000m
            memory: 4Gi
        hpaSpec:
          minReplicas: 2
          maxReplicas: 5
    ingressGateways:
      - name: istio-ingressgateway
        enabled: true
        k8s:
          resources:
            requests:
              cpu: 200m
              memory: 256Mi
          service:
            type: LoadBalancer
            ports:
              - name: http2
                port: 80
                targetPort: 8080
              - name: https
                port: 443
                targetPort: 8443
    egressGateways:
      - name: istio-egressgateway
        enabled: true
  values:
    global:
      proxy:
        resources:
          requests:
            cpu: 100m
            memory: 128Mi
          limits:
            cpu: 500m
            memory: 256Mi
```

### Deploy with Operator

```bash
# Apply operator configuration
istioctl install -f istio/operator.yaml

# Verify all pods are running
kubectl get pods -n istio-system
```

## Istio VirtualService and DestinationRule

### VirtualService for Traffic Management

```yaml
# k8s/virtualservice.yaml
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: node-app-vs
  namespace: my-app
spec:
  hosts:
    - node-app-service
    - api.example.com
  gateways:
    - node-app-gateway
    - mesh
  http:
    # Canary routing: send 10% to v2
    - match:
        - headers:
            x-canary:
              exact: "true"
      route:
        - destination:
            host: node-app-service
            subset: v2
    # Header-based routing
    - match:
        - headers:
            user-type:
              exact: "premium"
      route:
        - destination:
            host: node-app-service
            subset: v2
            port:
              number: 80
    # Main traffic split
    - route:
        - destination:
            host: node-app-service
            subset: v1
          weight: 90
        - destination:
            host: node-app-service
            subset: v2
          weight: 10
      retries:
        attempts: 3
        perTryTimeout: 2s
        retryOn: 5xx,reset,connect-failure
      timeout: 10s
      fault:
        delay:
          percentage:
            value: 5
          fixedDelay: 3s
---
# k8s/gateway.yaml
apiVersion: networking.istio.io/v1beta1
kind: Gateway
metadata:
  name: node-app-gateway
  namespace: my-app
spec:
  selector:
    istio: ingressgateway
  servers:
    - port:
        number: 443
        name: https
        protocol: HTTPS
      tls:
        mode: SIMPLE
        credentialName: api-tls-secret
      hosts:
        - api.example.com
    - port:
        number: 80
        name: http
        protocol: HTTP
      tls:
        httpsRedirect: true
      hosts:
        - api.example.com
```

### DestinationRule with Circuit Breaking

```yaml
# k8s/destinationrule.yaml
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: node-app-dr
  namespace: my-app
spec:
  host: node-app-service
  trafficPolicy:
    connectionPool:
      tcp:
        maxConnections: 100
        connectTimeout: 30ms
        tcpKeepalive:
          time: 7200s
          interval: 75s
      http:
        h2UpgradePolicy: DEFAULT
        http1MaxPendingRequests: 100
        http2MaxRequests: 1000
        maxRequestsPerConnection: 100
        maxRetries: 3
    outlierDetection:
      consecutive5xxErrors: 5
      interval: 30s
      baseEjectionTime: 30s
      maxEjectionPercent: 50
      minHealthPercent: 30
    loadBalancer:
      simple: LEAST_REQUEST
    tls:
      mode: ISTIO_MUTUAL
  subsets:
    - name: v1
      labels:
        version: v1
      trafficPolicy:
        connectionPool:
          http:
            http1MaxPendingRequests: 50
    - name: v2
      labels:
        version: v2
      trafficPolicy:
        connectionPool:
          http:
            http1MaxPendingRequests: 200
```

### Mutual TLS Enforcement

```yaml
# k8s/peer-authentication.yaml
apiVersion: security.istio.io/v1beta1
kind: PeerAuthentication
metadata:
  name: strict-mtls
  namespace: my-app
spec:
  mtls:
    mode: STRICT
---
# k8s/authorization-policy.yaml
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: node-app-authz
  namespace: my-app
spec:
  selector:
    matchLabels:
      app: node-app
  rules:
    - from:
        - source:
            principals:
              - "cluster.local/ns/my-app/sa/frontend-sa"
      to:
        - operation:
            methods: ["GET", "POST", "PUT", "DELETE"]
            paths: ["/api/*"]
    - from:
        - source:
            namespaces: ["monitoring"]
      to:
        - operation:
            methods: ["GET"]
            paths: ["/health/*", "/metrics"]
```

## Linkerd Service Mesh

### Linkerd Installation

```bash
# Install Linkerd CLI
curl --proto '=https' -sL https://run.linkerd.io/install | sh
export PATH=$PATH:$HOME/.linkerd2/bin

# Check cluster readiness
linkerd check --pre

# Install Linkerd control plane
linkerd install | kubectl apply -f -

# Verify installation
linkerd check

# Install Linkerd Viz dashboard
linkerd viz install | kubectl apply -f -

# Inject sidecar into deployment
kubectl get deploy -n my-app -o yaml | linkerd inject - | kubectl apply -f -
```

### Linkerd Traffic Splitting

```yaml
# k8s/linkerd-traffic-split.yaml
apiVersion: split.smi-spec.io/v1alpha2
kind: TrafficSplit
metadata:
  name: node-app-split
  namespace: my-app
spec:
  service: node-app-service
  backends:
    - service: node-app-stable
      weight: 900
    - service: node-app-canary
      weight: 100
---
# Stable service
apiVersion: v1
kind: Service
metadata:
  name: node-app-stable
  namespace: my-app
spec:
  selector:
    app: node-app
    version: v1
  ports:
    - port: 80
      targetPort: 3000
---
# Canary service
apiVersion: v1
kind: Service
metadata:
  name: node-app-canary
  namespace: my-app
spec:
  selector:
    app: node-app
    version: v2
  ports:
    - port: 80
      targetPort: 3000
```

### Linkerd Observability with Service Profiles

```yaml
# k8s/service-profile.yaml
apiVersion: linkerd.io/v1alpha2
kind: ServiceProfile
metadata:
  name: node-app-service.my-app.svc.cluster.local
  namespace: my-app
spec:
  routes:
    - name: GET /api/users
      condition:
        method: GET
        pathRegex: /api/users
      timeout: 300ms
      isRetryable: true
    - name: POST /api/orders
      condition:
        method: POST
        pathRegex: /api/orders
      timeout: 500ms
    - name: GET /health/ready
      condition:
        method: GET
        pathRegex: /health/ready
      timeout: 100ms
  retryBudget:
    retryRatio: 0.2
    minRetriesPerSecond: 10
    ttl: 10s
```

## Docker Swarm Deployment

### Swarm Initialization and Node Management

```bash
# Initialize swarm on manager node
docker swarm init --advertise-addr 192.168.1.10

# Get join tokens
docker swarm join-token worker
docker swarm join-token manager

# Join worker nodes (run on each worker)
docker swarm join --token SWMTKN-1-xxxxx 192.168.1.10:2377

# List nodes
docker node ls

# Label nodes for placement constraints
docker node update --label-add zone=us-east node1
docker node update --label-add zone=us-west node2
docker node update --label-add role=database node3
```

### Docker Stack File

```yaml
# docker-stack.yml
version: "3.8"

services:
  api:
    image: registry.example.com/my-app:latest
    deploy:
      replicas: 3
      update_config:
        parallelism: 1
        delay: 30s
        failure_action: rollback
        order: start-first
      rollback_config:
        parallelism: 1
        delay: 10s
      restart_policy:
        condition: on-failure
        delay: 5s
        max_attempts: 3
        window: 120s
      placement:
        constraints:
          - node.role == worker
        preferences:
          - spread: node.id
      resources:
        limits:
          cpus: "0.50"
          memory: 256M
        reservations:
          cpus: "0.10"
          memory: 128M
    ports:
      - "80:3000"
    environment:
      - NODE_ENV=production
      - DATABASE_URL_FILE=/run/secrets/db_url
    secrets:
      - db_url
      - jwt_secret
    networks:
      - app-network
    healthcheck:
      test: ["CMD", "node", "-e", "require('http').get('http://localhost:3000/health/live', r => { process.exit(r.statusCode === 200 ? 0 : 1) })"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s

  redis:
    image: redis:7-alpine
    deploy:
      replicas: 1
      placement:
        constraints:
          - node.labels.role == cache
    volumes:
      - redis-data:/data
    command: redis-server --appendonly yes --maxmemory 128mb
    networks:
      - app-network

  nginx:
    image: nginx:alpine
    deploy:
      replicas: 2
      update_config:
        parallelism: 1
        delay: 10s
    ports:
      - "443:443"
    configs:
      - source: nginx_conf
        target: /etc/nginx/nginx.conf
    secrets:
      - source: tls_cert
        target: /etc/nginx/ssl/cert.pem
      - source: tls_key
        target: /etc/nginx/ssl/key.pem
    networks:
      - app-network

volumes:
  redis-data:
    driver: local

networks:
  app-network:
    driver: overlay
    attachable: true
    driver_opts:
      encrypted: "true"

secrets:
  db_url:
    external: true
  jwt_secret:
    external: true
  tls_cert:
    file: ./certs/cert.pem
  tls_key:
    file: ./certs/key.pem

configs:
  nginx_conf:
    file: ./nginx/nginx.conf
```

### Deploy and Manage Stack

```bash
# Deploy stack
docker stack deploy -c docker-stack.yml myapp

# List services
docker stack services myapp

# List tasks (containers)
docker stack ps myapp

# Scale a service
docker service scale myapp_api=5

# Update service image
docker service update --image registry.example.com/my-app:v2 myapp_api

# View service logs
docker service logs -f myapp_api

# Remove stack
docker stack rm myapp
```

### Rolling Update with CLI

```bash
# Create service with rolling update
docker service create \
  --name node-app \
  --replicas 3 \
  --update-parallelism 1 \
  --update-delay 30s \
  --update-failure-action rollback \
  --update-order start-first \
  --rollback-parallelism 1 \
  --rollback-delay 10s \
  --limit-cpu 0.5 \
  --limit-memory 256M \
  --reserve-cpu 0.1 \
  --reserve-memory 128M \
  --publish published=80,target=3000 \
  --secret db_url \
  --network app-network \
  registry.example.com/my-app:latest

# Perform rolling update
docker service update --image registry.example.com/my-app:v2 node-app

# Rollback to previous version
docker service rollback node-app
```

## Multi-Container Deployment Patterns

### Sidecar Pattern

```yaml
# k8s/sidecar-pattern.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app-with-sidecars
  namespace: my-app
spec:
  replicas: 3
  selector:
    matchLabels:
      app: node-app
  template:
    metadata:
      labels:
        app: node-app
    spec:
      containers:
        # Main application container
        - name: node-app
          image: registry.example.com/my-app:v1
          ports:
            - containerPort: 3000
          volumeMounts:
            - name: shared-logs
              mountPath: /app/logs
          resources:
            requests:
              cpu: 200m
              memory: 256Mi
            limits:
              cpu: 500m
              memory: 512Mi

        # Sidecar: Log shipper (Filebeat/Fluentd)
        - name: log-shipper
          image: fluent/fluent-bit:2.2
          volumeMounts:
            - name: shared-logs
              mountPath: /app/logs
              readOnly: true
            - name: fluent-config
              mountPath: /fluent-bit/etc
          resources:
            requests:
              cpu: 50m
              memory: 64Mi
            limits:
              cpu: 100m
              memory: 128Mi

        # Sidecar: Metrics exporter
        - name: metrics-exporter
          image: prom/node-exporter:v1.7
          ports:
            - containerPort: 9100
              name: metrics
          resources:
            requests:
              cpu: 25m
              memory: 32Mi
            limits:
              cpu: 50m
              memory: 64Mi

      volumes:
        - name: shared-logs
          emptyDir: {}
        - name: fluent-config
          configMap:
            name: fluent-bit-config
```

### Ambassador Pattern

```javascript
// ambassador/proxy.js
// Ambassador proxy: handles external API communication
import express from 'express';
import { createProxyMiddleware } from 'http-proxy-middleware';
import circuitBreaker from 'opossum';

const app = express();

const externalApiOptions = {
  timeout: 3000,
  errorThresholdPercentage: 50,
  resetTimeout: 30000,
};

const breaker = new circuitBreaker(
  (req, res) => {
    return fetch(`https://external-api.example.com${req.path}`, {
      method: req.method,
      headers: { 'Content-Type': 'application/json' },
      body: req.method !== 'GET' ? JSON.stringify(req.body) : undefined,
    });
  },
  externalApiOptions
);

breaker.fallback(() => ({ status: 'degraded', data: null }));

breaker.on('open', () => console.log('Circuit breaker OPEN'));
breaker.on('halfOpen', () => console.log('Circuit breaker HALF-OPEN'));
breaker.on('close', () => console.log('Circuit breaker CLOSED'));

app.use('/external', async (req, res) => {
  try {
    const result = await breaker.fire(req, res);
    res.json(result);
  } catch (err) {
    res.status(503).json({ error: 'Service unavailable', retry: true });
  }
});

app.get('/health', (req, res) => {
  res.json({
    status: 'ok',
    circuitBreaker: breaker.status.stats,
  });
});

app.listen(3001);
```

```yaml
# k8s/ambassador-pattern.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app-with-ambassador
  namespace: my-app
spec:
  replicas: 2
  selector:
    matchLabels:
      app: node-app
  template:
    metadata:
      labels:
        app: node-app
    spec:
      containers:
        - name: node-app
          image: registry.example.com/my-app:v1
          ports:
            - containerPort: 3000
          env:
            - name: EXTERNAL_API_URL
              value: "http://localhost:3001"

        - name: ambassador
          image: registry.example.com/ambassador-proxy:v1
          ports:
            - containerPort: 3001
          resources:
            requests:
              cpu: 50m
              memory: 64Mi
            limits:
              cpu: 100m
              memory: 128Mi
```

### Adapter Pattern

```yaml
# k8s/adapter-pattern.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app-with-adapter
  namespace: my-app
spec:
  replicas: 2
  selector:
    matchLabels:
      app: node-app
  template:
    metadata:
      labels:
        app: node-app
    spec:
      containers:
        - name: node-app
          image: registry.example.com/my-app:v1
          ports:
            - containerPort: 3000

        # Adapter: Standardizes monitoring output
        - name: monitoring-adapter
          image: registry.example.com/metrics-adapter:v1
          ports:
            - containerPort: 9090
          env:
            - name: APP_METRICS_URL
              value: "http://localhost:3000/metrics"
            - name: OUTPUT_FORMAT
              value: "prometheus"
            - name: SCRAPE_INTERVAL
              value: "15s"
          resources:
            requests:
              cpu: 25m
              memory: 32Mi
```

## Service Discovery Comparison

```
┌──────────────────────┬────────────────────────┬────────────────────────┐
│     Mechanism        │      Kubernetes        │      Docker Swarm      │
├──────────────────────┼────────────────────────┼────────────────────────┤
│ DNS-based            │ <svc>.<ns>.svc.        │ <service> (within      │
│                      │ cluster.local          │ same network)          │
├──────────────────────┼────────────────────────┼────────────────────────┤
│ Environment vars     │ <SVC>_SERVICE_HOST     │ Not recommended        │
│                      │ <SVC>_SERVICE_PORT     │                        │
├──────────────────────┼────────────────────────┼────────────────────────┤
│ Load balancing       │ kube-proxy (iptables/  │ Internal DNS round-    │
│                      │ IPVS), service mesh    │ robin, VIP routing     │
├──────────────────────┼────────────────────────┼────────────────────────┤
│ Health checking      │ Liveness/Readiness     │ Healthcheck command    │
│                      │ Probes                 │ in service config      │
├──────────────────────┼────────────────────────┼────────────────────────┤
│ Service registration │ Label selectors +      │ Automatic via          │
│                      │ Endpoints              │ overlay network        │
└──────────────────────┴────────────────────────┴────────────────────────┘
```

## Troubleshooting Service Mesh Issues

### Istio Debugging Commands

```bash
# Check proxy status
istioctl proxy-status

# Analyze configuration issues
istioctl analyze -n my-app

# View proxy configuration
istioctl proxy-config routes <pod-name> -n my-app

# Check proxy logs
kubectl logs <pod-name> -c istio-proxy -n my-app

# Verify mTLS status
istioctl authn tls-check <pod-name> -n my-app

# Debug traffic flow
istioctl x describe pod <pod-name> -n my-app

# Check certificate status
istioctl proxy-config secret <pod-name> -n my-app
```

### Common Issues and Fixes

| Issue | Symptom | Fix |
|-------|---------|-----|
| Sidecar not injected | No istio-proxy container | Label namespace: `istio-injection=enabled` |
| 503 errors after deploy | Requests during pod termination | Add `preStop` sleep + increase `terminationGracePeriodSeconds` |
| Connection refused | Service mesh blocks non-mTLS | Set `PeerAuthentication` mode to `PERMISSIVE` during migration |
| High latency | Retries amplifying load | Reduce `retryOn` conditions, set `perTryTimeout` |
| Circuit breaker opens | Cascading failures | Tune `consecutive5xxErrors`, `baseEjectionTime` |
| TLS handshake failure | Certificate mismatch | Run `istioctl verify-install`, check `DestinationRule` TLS config |

### Linkerd Debugging Commands

```bash
# Check Linkerd health
linkerd check

# View traffic stats
linkerd stat deploy -n my-app

# View live traffic
linkerd tap deploy/node-app -n my-app

# View route metrics
linkerd routes deploy/node-app -n my-app

# Diagnose latency
linkerd diagnostics proxy-metrics -n my-app deploy/node-app
```

## Docker Swarm vs Kubernetes

```
┌──────────────────────┬─────────────────────────┬─────────────────────────┐
│    Feature           │     Docker Swarm        │      Kubernetes         │
├──────────────────────┼─────────────────────────┼─────────────────────────┤
│ Complexity           │ Low                     │ High                    │
│ Learning curve       │ Gentle                  │ Steep                   │
│ Cluster size         │ Small-medium            │ Any size                │
│ Auto-scaling         │ Manual                  │ HPA, VPA, Cluster       │
│ Service mesh         │ Third-party (Traefik)   │ Istio, Linkerd, Cilium │
│ Networking           │ Overlay (VXLAN)         │ CNI plugins             │
│ Storage              │ Volume drivers           │ PV/PVC, CSI             │
│ Rolling updates      │ Built-in                │ Built-in + advanced     │
│ Secrets management   │ Encrypted at rest       │ etcd encrypted          │
│ Community            │ Smaller                 │ Massive ecosystem       │
│ Use case             │ Dev/small prod          │ Enterprise production   │
└──────────────────────┴─────────────────────────┴─────────────────────────┘
```

## Best Practices Checklist

- [ ] Enable mTLS for all service-to-service communication
- [ ] Define VirtualService retry and timeout policies
- [ ] Use DestinationRule circuit breakers for resilience
- [ ] Implement sidecar patterns for logging and monitoring
- [ ] Configure rolling update strategies with rollback
- [ ] Use Docker secrets for sensitive configuration
- [ ] Set resource limits on all containers including sidecars
- [ ] Enable distributed tracing in the service mesh
- [ ] Test fault injection in staging before production
- [ ] Monitor proxy resource consumption separately

## Cross-References

- See [Kubernetes Patterns](./01-kubernetes-patterns.md) for core K8s deployments
- See [Container Storage & Networking](./03-container-storage-networking-scaling.md) for storage and scaling
- See [Docker](../docker/01-dockerfile.md) for container image creation
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability setup
- See [Security](../07-container-security/01-image-scanning.md) for container hardening
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for automated deployment pipelines

## Next Steps

Continue to [Container Storage, Networking & Scaling](./03-container-storage-networking-scaling.md).
