# Container Storage, Networking & Scaling

## What You'll Learn

- Container networking with CNI plugins and network policies
- Persistent storage with PV, PVC, and StorageClasses
- StatefulSets for stateful applications
- Advanced health checks and probes
- Resource management with requests, limits, and quotas
- Horizontal, Vertical, and Cluster autoscaling
- Pod Disruption Budgets and topology constraints

## Container Networking Architecture

```
┌──────────────────────────────────────────────────────────────────┐
│                       Cluster Network (L3/L4)                     │
│  Pod CIDR: 10.244.0.0/16          Service CIDR: 10.96.0.0/12     │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  Node 1 (192.168.1.10)          Node 2 (192.168.1.11)            │
│  ┌─────────────────────┐        ┌─────────────────────┐          │
│  │  ┌──────┐ ┌──────┐  │        │  ┌──────┐ ┌──────┐  │          │
│  │  │Pod A │ │Pod B │  │        │  │Pod C │ │Pod D │  │          │
│  │  │veth0 │ │veth1 │  │        │  │veth2 │ │veth3 │  │          │
│  │  └──┬───┘ └──┬───┘  │        │  └──┬───┘ └──┬───┘  │          │
│  │     │        │      │        │     │        │      │          │
│  │  ┌──┴────────┴──┐   │        │  ┌──┴────────┴──┐   │          │
│  │  │  cni0 bridge │   │        │  │  cni0 bridge │   │          │
│  │  └──────┬───────┘   │        │  └──────┬───────┘   │          │
│  │         │ VXLAN     │        │         │ VXLAN     │          │
│  │  ┌──────┴───────┐   │        │  ┌──────┴───────┐   │          │
│  │  │  flannel.1   │◄──┼────────┼──┤  flannel.1   │   │          │
│  │  └──────────────┘   │        │  └──────────────┘   │          │
│  └─────────────────────┘        └─────────────────────┘          │
│                                                                   │
│  ┌─────────────────────────────────────────────────────────┐     │
│  │  kube-proxy (iptables/IPVS mode)                        │     │
│  │  Service: node-app-service → 10.96.45.12 → Pod A,B,C,D │     │
│  └─────────────────────────────────────────────────────────┘     │
└──────────────────────────────────────────────────────────────────┘
```

## CNI Plugin Configuration

### Calico Network Policy

```yaml
# k8s/calico-installation.yaml
apiVersion: operator.tigera.io/v1
kind: Installation
metadata:
  name: default
spec:
  calicoNetwork:
    ipPools:
      - blockSize: 26
        cidr: 10.244.0.0/16
        encapsulation: VXLANCrossSubnet
        natOutgoing: Enabled
        nodeSelector: all()
    nodeAddressAutodetectionV4:
      interface: eth0
---
# k8s/network-policy.yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: node-app-netpol
  namespace: my-app
spec:
  podSelector:
    matchLabels:
      app: node-app
  policyTypes:
    - Ingress
    - Egress
  ingress:
    # Allow traffic from frontend
    - from:
        - namespaceSelector:
            matchLabels:
              name: frontend
          podSelector:
            matchLabels:
              role: web
      ports:
        - protocol: TCP
          port: 3000
    # Allow monitoring
    - from:
        - namespaceSelector:
            matchLabels:
              name: monitoring
      ports:
        - protocol: TCP
          port: 9090
  egress:
    # Allow DNS
    - to:
        - namespaceSelector: {}
          podSelector:
            matchLabels:
              k8s-app: kube-dns
      ports:
        - protocol: UDP
          port: 53
        - protocol: TCP
          port: 53
    # Allow database access
    - to:
        - podSelector:
            matchLabels:
              app: postgres
      ports:
        - protocol: TCP
          port: 5432
    # Allow Redis access
    - to:
        - podSelector:
            matchLabels:
              app: redis
      ports:
        - protocol: TCP
          port: 6379
```

### Service Mesh Network Policy with Istio

```yaml
# k8s/istio-network-policy.yaml
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: deny-all-default
  namespace: my-app
spec:
  {} # Empty spec denies all traffic by default
---
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: allow-frontend-to-api
  namespace: my-app
spec:
  selector:
    matchLabels:
      app: node-app
  action: ALLOW
  rules:
    - from:
        - source:
            principals:
              - "cluster.local/ns/frontend/sa/frontend-sa"
            namespaces: ["frontend"]
      to:
        - operation:
            methods: ["GET", "POST"]
            paths: ["/api/v1/*"]
      when:
        - key: request.headers[x-request-id]
          notEquals: ""
```

## Container Storage

### PersistentVolumes and PersistentVolumeClaims

```yaml
# k8s/persistent-volume.yaml
apiVersion: v1
kind: PersistentVolume
metadata:
  name: postgres-pv
  labels:
    type: local
    app: postgres
spec:
  capacity:
    storage: 20Gi
  accessModes:
    - ReadWriteOnce
  persistentVolumeReclaimPolicy: Retain
  storageClassName: fast-ssd
  hostPath:
    path: /mnt/data/postgres
  nodeAffinity:
    required:
      nodeSelectorTerms:
        - matchExpressions:
            - key: kubernetes.io/hostname
              operator: In
              values: ["node3"]
---
# k8s/persistent-volume-claim.yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: postgres-pvc
  namespace: my-app
spec:
  accessModes:
    - ReadWriteOnce
  storageClassName: fast-ssd
  resources:
    requests:
      storage: 20Gi
  selector:
    matchLabels:
      app: postgres
```

### StorageClass with CSI Drivers

```yaml
# k8s/storage-class.yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: fast-ssd
  annotations:
    storageclass.kubernetes.io/is-default-class: "false"
provisioner: ebs.csi.aws.com
parameters:
  type: gp3
  iops: "4000"
  throughput: "250"
  encrypted: "true"
  kmsKeyId: "arn:aws:kms:us-east-1:123456:key/abcd"
volumeBindingMode: WaitForFirstConsumer
allowVolumeExpansion: true
reclaimPolicy: Retain
mountOptions:
  - noatime
  - nodiratime
---
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: shared-nfs
provisioner: nfs.csi.k8s.io
parameters:
  server: nfs-server.example.com
  share: /exports/shared
volumeBindingMode: Immediate
allowVolumeExpansion: false
reclaimPolicy: Delete
---
# GCP pd-ssd example
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: premium-rwo
provisioner: pd.csi.storage.gke.io
parameters:
  type: pd-ssd
  replication-type: regional-pd
volumeBindingMode: WaitForFirstConsumer
allowVolumeExpansion: true
reclaimPolicy: Retain
```

## StatefulSets for Stateful Applications

### PostgreSQL StatefulSet

```yaml
# k8s/postgres-statefulset.yaml
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: postgres
  namespace: my-app
spec:
  serviceName: postgres-headless
  replicas: 3
  selector:
    matchLabels:
      app: postgres
  template:
    metadata:
      labels:
        app: postgres
    spec:
      terminationGracePeriodSeconds: 60
      containers:
        - name: postgres
          image: postgres:16-alpine
          ports:
            - containerPort: 5432
              name: postgres
          env:
            - name: POSTGRES_DB
              value: myapp
            - name: POSTGRES_USER
              valueFrom:
                secretKeyRef:
                  name: postgres-secret
                  key: username
            - name: POSTGRES_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: postgres-secret
                  key: password
            - name: PGDATA
              value: /var/lib/postgresql/data/pgdata
          volumeMounts:
            - name: postgres-data
              mountPath: /var/lib/postgresql/data
            - name: init-scripts
              mountPath: /docker-entrypoint-initdb.d
          resources:
            requests:
              cpu: 500m
              memory: 1Gi
            limits:
              cpu: 2000m
              memory: 4Gi
          livenessProbe:
            exec:
              command:
                - pg_isready
                - -U
                - postgres
            initialDelaySeconds: 30
            periodSeconds: 10
            timeoutSeconds: 5
            failureThreshold: 6
          readinessProbe:
            exec:
              command:
                - pg_isready
                - -U
                - postgres
                - -d
                - myapp
            initialDelaySeconds: 5
            periodSeconds: 5
            timeoutSeconds: 3
            failureThreshold: 3
      volumes:
        - name: init-scripts
          configMap:
            name: postgres-init-scripts
  volumeClaimTemplates:
    - metadata:
        name: postgres-data
        labels:
          app: postgres
      spec:
        accessModes: ["ReadWriteOnce"]
        storageClassName: fast-ssd
        resources:
          requests:
            storage: 20Gi
---
# k8s/postgres-headless-service.yaml
apiVersion: v1
kind: Service
metadata:
  name: postgres-headless
  namespace: my-app
spec:
  selector:
    app: postgres
  clusterIP: None
  ports:
    - port: 5432
      targetPort: 5432
---
# k8s/postgres-service.yaml
apiVersion: v1
kind: Service
metadata:
  name: postgres
  namespace: my-app
spec:
  selector:
    app: postgres
  ports:
    - port: 5432
      targetPort: 5432
  type: ClusterIP
```

## Advanced Health Checks

### Node.js Health Check Implementation

```javascript
// src/health.js
import { Router } from 'express';
import { Pool } from 'pg';
import { createClient } from 'redis';
import { promisify } from 'util';
import os from 'os';

const healthRouter = Router();

let dbPool;
let redisClient;
let startupComplete = false;
let ready = false;

// Track application state
const state = {
  startTime: Date.now(),
  requestCount: 0,
  errorCount: 0,
  lastError: null,
};

export function initHealthChecks(db, redis) {
  dbPool = db;
  redisClient = redis;

  // Simulate initialization delay
  setTimeout(() => {
    startupComplete = true;
  }, 5000);
}

// Startup probe: blocks readiness until initialization completes
healthRouter.get('/health/startup', (req, res) => {
  if (!startupComplete) {
    return res.status(503).json({
      status: 'starting',
      uptime: process.uptime(),
    });
  }
  res.json({ status: 'started', pid: process.pid });
});

// Liveness probe: detects deadlocks and unresponsive states
healthRouter.get('/health/live', async (req, res) => {
  const checks = {};
  let healthy = true;

  // Check event loop lag
  const start = process.hrtime.bigint();
  await new Promise((resolve) => setImmediate(resolve));
  const lagMs = Number(process.hrtime.bigint() - start) / 1e6;
  checks.eventLoopLag = { value: `${lagMs.toFixed(2)}ms`, ok: lagMs < 100 };
  if (!checks.eventLoopLag.ok) healthy = false;

  // Check heap usage
  const heapUsed = process.memoryUsage().heapUsed;
  const heapTotal = process.memoryUsage().heapTotal;
  const heapRatio = heapUsed / heapTotal;
  checks.heapUsage = {
    value: `${(heapRatio * 100).toFixed(1)}%`,
    ok: heapRatio < 0.9,
  };
  if (!checks.heapUsage.ok) healthy = false;

  const statusCode = healthy ? 200 : 503;
  res.status(statusCode).json({
    status: healthy ? 'alive' : 'unhealthy',
    checks,
    uptime: process.uptime(),
  });
});

// Readiness probe: checks all dependencies
healthRouter.get('/health/ready', async (req, res) => {
  if (!startupComplete) {
    return res.status(503).json({ status: 'not ready', reason: 'starting' });
  }

  const checks = {};

  // Database check
  try {
    const dbStart = Date.now();
    const result = await dbPool.query('SELECT 1 AS health');
    checks.database = {
      status: 'ok',
      latency: `${Date.now() - dbStart}ms`,
      connections: dbPool.totalCount,
    };
  } catch (err) {
    checks.database = { status: 'error', error: err.message };
  }

  // Redis check
  try {
    const redisStart = Date.now();
    await redisClient.ping();
    checks.redis = {
      status: 'ok',
      latency: `${Date.now() - redisStart}ms`,
    };
  } catch (err) {
    checks.redis = { status: 'error', error: err.message };
  }

  // External dependency check
  try {
    const apiStart = Date.now();
    const controller = new AbortController();
    const timeout = setTimeout(() => controller.abort(), 2000);
    const resp = await fetch('https://api.external.com/health', {
      signal: controller.signal,
    });
    clearTimeout(timeout);
    checks.externalApi = {
      status: resp.ok ? 'ok' : 'degraded',
      latency: `${Date.now() - apiStart}ms`,
    };
  } catch (err) {
    checks.externalApi = { status: 'degraded', error: 'unreachable' };
  }

  const allCriticalOk = checks.database?.status === 'ok'
    && checks.redis?.status === 'ok';

  res.status(allCriticalOk ? 200 : 503).json({
    status: allCriticalOk ? 'ready' : 'not ready',
    checks,
  });
});

// Detailed metrics endpoint
healthRouter.get('/health/detailed', (req, res) => {
  const mem = process.memoryUsage();
  res.json({
    status: 'ok',
    uptime: process.uptime(),
    pid: process.pid,
    version: process.version,
    platform: process.platform,
    memory: {
      rss: `${(mem.rss / 1024 / 1024).toFixed(1)} MB`,
      heapTotal: `${(mem.heapTotal / 1024 / 1024).toFixed(1)} MB`,
      heapUsed: `${(mem.heapUsed / 1024 / 1024).toFixed(1)} MB`,
      external: `${(mem.external / 1024 / 1024).toFixed(1)} MB`,
    },
    cpu: {
      cores: os.cpus().length,
      loadAvg: os.loadavg(),
    },
    requests: state.requestCount,
    errors: state.errorCount,
    lastError: state.lastError,
    timestamp: new Date().toISOString(),
  });
});

export default healthRouter;
```

### Advanced Probe Configurations

```yaml
# k8s/advanced-probes.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
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
      terminationGracePeriodSeconds: 60
      initContainers:
        - name: wait-for-db
          image: busybox:1.36
          command:
            - sh
            - -c
            - |
              until nc -z postgres-0.postgres-headless 5432; do
                echo "Waiting for database..."
                sleep 2
              done
        - name: run-migrations
          image: registry.example.com/my-app:v1
          command: ["npm", "run", "migrate"]
          envFrom:
            - secretRef:
                name: app-secrets
      containers:
        - name: node-app
          image: registry.example.com/my-app:v1
          ports:
            - containerPort: 3000
          startupProbe:
            httpGet:
              path: /health/startup
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 5
            failureThreshold: 30
            timeoutSeconds: 3
          livenessProbe:
            httpGet:
              path: /health/live
              port: 3000
            initialDelaySeconds: 0
            periodSeconds: 15
            timeoutSeconds: 5
            failureThreshold: 3
            successThreshold: 1
          readinessProbe:
            httpGet:
              path: /health/ready
              port: 3000
            initialDelaySeconds: 0
            periodSeconds: 5
            timeoutSeconds: 3
            failureThreshold: 3
            successThreshold: 1
          lifecycle:
            preStop:
              exec:
                command:
                  - sh
                  - -c
                  - "sleep 10"
          resources:
            requests:
              cpu: 200m
              memory: 256Mi
            limits:
              cpu: 1000m
              memory: 512Mi
```

## Resource Management

### LimitRange

```yaml
# k8s/limit-range.yaml
apiVersion: v1
kind: LimitRange
metadata:
  name: app-limits
  namespace: my-app
spec:
  limits:
    - type: Container
      default:
        cpu: 500m
        memory: 512Mi
      defaultRequest:
        cpu: 100m
        memory: 128Mi
      min:
        cpu: 50m
        memory: 64Mi
      max:
        cpu: 2000m
        memory: 4Gi
      maxLimitRequestRatio:
        cpu: "10"
        memory: "8"
    - type: Pod
      max:
        cpu: 4000m
        memory: 8Gi
      min:
        cpu: 100m
        memory: 128Mi
    - type: PersistentVolumeClaim
      min:
        storage: 1Gi
      max:
        storage: 100Gi
```

### ResourceQuota

```yaml
# k8s/resource-quota.yaml
apiVersion: v1
kind: ResourceQuota
metadata:
  name: app-quota
  namespace: my-app
spec:
  hard:
    requests.cpu: "8"
    requests.memory: 16Gi
    limits.cpu: "16"
    limits.memory: 32Gi
    pods: "30"
    services: "10"
    persistentvolumeclaims: "10"
    secrets: "20"
    configmaps: "20"
  scopeSelector:
    matchExpressions:
      - scopeName: PriorityClass
        operator: In
        values: ["high"]
---
# k8s/priority-class.yaml
apiVersion: scheduling.k8s.io/v1
kind: PriorityClass
metadata:
  name: high
value: 1000
globalDefault: false
description: "High priority class for critical workloads"
```

## Horizontal Pod Autoscaler

### HPA with Custom Metrics

```yaml
# k8s/hpa-custom-metrics.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-app-hpa
  namespace: my-app
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: node-app
  minReplicas: 3
  maxReplicas: 30
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 65
    - type: Resource
      resource:
        name: memory
        target:
          type: Utilization
          averageUtilization: 75
    - type: Pods
      pods:
        metric:
          name: http_requests_per_second
        target:
          type: AverageValue
          averageValue: "500"
    - type: Pods
      pods:
        metric:
          name: http_request_duration_seconds_p95
        target:
          type: AverageValue
          averageValue: "500m"
    - type: Object
      object:
        describedObject:
          apiVersion: v1
          kind: Service
          name: node-app-service
        metric:
          name: requests_queue_length
        target:
          type: Value
          value: "10"
  behavior:
    scaleUp:
      stabilizationWindowSeconds: 30
      policies:
        - type: Pods
          value: 4
          periodSeconds: 60
        - type: Percent
          value: 50
          periodSeconds: 120
      selectPolicy: Max
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
        - type: Pods
          value: 2
          periodSeconds: 60
      selectPolicy: Min
```

### Prometheus Adapter for Custom Metrics

```yaml
# k8s/prometheus-adapter.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: prometheus-adapter
  namespace: monitoring
data:
  config.yaml: |
    rules:
      - seriesQuery: 'http_requests_total{namespace!="",pod!=""}'
        resources:
          overrides:
            namespace: {resource: "namespace"}
            pod: {resource: "pod"}
        name:
          matches: "^(.*)_total$"
          as: "${1}_per_second"
        metricsQuery: 'rate(<<.Series>>{<<.LabelMatchers>>}[2m])'
      - seriesQuery: 'http_request_duration_seconds_bucket{namespace!="",pod!=""}'
        resources:
          overrides:
            namespace: {resource: "namespace"}
            pod: {resource: "pod"}
        name:
          matches: "^(.*)_bucket$"
          as: "${1}_p95"
        metricsQuery: 'histogram_quantile(0.95, rate(<<.Series>>{<<.LabelMatchers>>}[5m]))'
```

### Custom Metrics with Prometheus in Node.js

```javascript
// src/metrics.js
import express from 'express';
import promClient from 'prom-client';

const register = new promClient.Registry();
promClient.collectDefaultMetrics({ register });

const httpRequestDuration = new promClient.Histogram({
  name: 'http_request_duration_seconds',
  help: 'Duration of HTTP requests in seconds',
  labelNames: ['method', 'route', 'status_code'],
  buckets: [0.01, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5],
  registers: [register],
});

const httpRequestsTotal = new promClient.Counter({
  name: 'http_requests_total',
  help: 'Total number of HTTP requests',
  labelNames: ['method', 'route', 'status_code'],
  registers: [register],
});

const httpRequestsInProgress = new promClient.Gauge({
  name: 'http_requests_in_progress',
  help: 'Number of HTTP requests in progress',
  labelNames: ['method', 'route'],
  registers: [register],
});

const httpRequestQueueLength = new promClient.Gauge({
  name: 'requests_queue_length',
  help: 'Number of requests waiting to be processed',
  registers: [register],
});

export function metricsMiddleware(req, res, next) {
  const start = process.hrtime.bigint();
  httpRequestsInProgress.inc({ method: req.method, route: req.route?.path });

  res.on('finish', () => {
    const duration = Number(process.hrtime.bigint() - start) / 1e9;
    const labels = {
      method: req.method,
      route: req.route?.path || 'unknown',
      status_code: res.statusCode,
    };
    httpRequestDuration.observe(labels, duration);
    httpRequestsTotal.inc(labels);
    httpRequestsInProgress.dec({ method: req.method, route: req.route?.path });
  });

  next();
}

const app = express();
app.use(metricsMiddleware);

app.get('/metrics', async (req, res) => {
  res.set('Content-Type', register.contentType);
  res.end(await register.metrics());
});

app.listen(3000);
```

## Vertical Pod Autoscaler

```yaml
# k8s/vpa.yaml
apiVersion: autoscaling.k8s.io/v1
kind: VerticalPodAutoscaler
metadata:
  name: node-app-vpa
  namespace: my-app
spec:
  targetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: node-app
  updatePolicy:
    updateMode: "Auto"
    minReplicas: 2
  resourcePolicy:
    containerPolicies:
      - containerName: node-app
        minAllowed:
          cpu: 100m
          memory: 128Mi
        maxAllowed:
          cpu: 4000m
          memory: 8Gi
        controlledResources: ["cpu", "memory"]
        controlledValues: RequestsAndLimits
      - containerName: log-shipper
        mode: "Off"
```

## Cluster Autoscaler

```yaml
# k8s/cluster-autoscaler.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: cluster-autoscaler
  namespace: kube-system
spec:
  replicas: 1
  selector:
    matchLabels:
      app: cluster-autoscaler
  template:
    metadata:
      labels:
        app: cluster-autoscaler
    spec:
      serviceAccountName: cluster-autoscaler
      priorityClassName: system-cluster-critical
      containers:
        - name: cluster-autoscaler
          image: registry.k8s.io/autoscaling/cluster-autoscaler:v1.29
          command:
            - ./cluster-autoscaler
            - --v=4
            - --stderrthreshold=info
            - --cloud-provider=aws
            - --skip-nodes-with-local-storage=false
            - --expander=least-waste
            - --node-group-auto-discovery=asg:tag=k8s.io/cluster-autoscaler/enabled,k8s.io/cluster-autoscaler/my-cluster
            - --balance-similar-node-groups
            - --scale-down-enabled=true
            - --scale-down-delay-after-add=10m
            - --scale-down-unneeded-time=5m
            - --scale-down-utilization-threshold=0.5
            - --max-graceful-termination-sec=600
            - --max-node-provision-time=15m
          resources:
            requests:
              cpu: 100m
              memory: 300Mi
            limits:
              cpu: 200m
              memory: 600Mi
```

## Pod Disruption Budgets

```yaml
# k8s/pdb.yaml
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: node-app-pdb
  namespace: my-app
spec:
  minAvailable: 2
  selector:
    matchLabels:
      app: node-app
  unhealthyPodEvictionPolicy: IfHealthyBudget
---
# Alternative: use maxUnavailable
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: postgres-pdb
  namespace: my-app
spec:
  maxUnavailable: 1
  selector:
    matchLabels:
      app: postgres
  unhealthyPodEvictionPolicy: AlwaysAllow
```

## Topology Spread Constraints

```yaml
# k8s/topology-spread.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  namespace: my-app
spec:
  replicas: 6
  selector:
    matchLabels:
      app: node-app
  template:
    metadata:
      labels:
        app: node-app
    spec:
      topologySpreadConstraints:
        - maxSkew: 1
          topologyKey: topology.kubernetes.io/zone
          whenUnsatisfiable: DoNotSchedule
          labelSelector:
            matchLabels:
              app: node-app
          matchLabelKeys:
            - pod-template-hash
        - maxSkew: 1
          topologyKey: kubernetes.io/hostname
          whenUnsatisfiable: ScheduleAnyway
          labelSelector:
            matchLabels:
              app: node-app
      containers:
        - name: node-app
          image: registry.example.com/my-app:v1
          resources:
            requests:
              cpu: 200m
              memory: 256Mi
```

```
┌──────────────────────────────────────────────────────────────┐
│         Topology Spread: maxSkew=1 per Zone                  │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  Zone A (us-east-1a)     Zone B (us-east-1b)                │
│  ┌─────────────────┐     ┌─────────────────┐                │
│  │ Node1  Node2    │     │ Node3  Node4    │                │
│  │ ┌───┐  ┌───┐   │     │ ┌───┐  ┌───┐   │                │
│  │ │Pod│  │Pod│   │     │ │Pod│  │Pod│   │                │
│  │ │ 1 │  │ 2 │   │     │ │ 3 │  │ 4 │   │                │
│  │ └───┘  └───┘   │     │ └───┘  └───┘   │                │
│  │         ┌───┐  │     │         ┌───┐  │                │
│  │         │Pod│  │     │         │Pod│  │                │
│  │         │ 5 │  │     │         │ 6 │  │                │
│  │         └───┘  │     │         └───┘  │                │
│  └─────────────────┘     └─────────────────┘                │
│  Pods: 3                   Pods: 3                          │
│  (maxSkew = 0)             (maxSkew = 0)                    │
└──────────────────────────────────────────────────────────────┘
```

## Performance Tuning

### Node.js Container Optimization

```yaml
# k8s/optimized-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app-optimized
  namespace: my-app
spec:
  replicas: 3
  selector:
    matchLabels:
      app: node-app
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  template:
    metadata:
      labels:
        app: node-app
    spec:
      terminationGracePeriodSeconds: 45
      containers:
        - name: node-app
          image: registry.example.com/my-app:v1
          ports:
            - containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
            - name: UV_THREADPOOL_SIZE
              value: "8"
            - name: NODE_OPTIONS
              value: "--max-old-space-size=384 --max-semi-space-size=16"
          resources:
            requests:
              cpu: 250m
              memory: 384Mi
            limits:
              cpu: 1000m
              memory: 512Mi
          startupProbe:
            httpGet:
              path: /health/startup
              port: 3000
            periodSeconds: 5
            failureThreshold: 30
          livenessProbe:
            httpGet:
              path: /health/live
              port: 3000
            periodSeconds: 15
            failureThreshold: 3
          readinessProbe:
            httpGet:
              path: /health/ready
              port: 3000
            periodSeconds: 5
            failureThreshold: 3
          lifecycle:
            preStop:
              exec:
                command: ["sh", "-c", "sleep 10"]
      affinity:
        podAntiAffinity:
          preferredDuringSchedulingIgnoredDuringExecution:
            - weight: 100
              podAffinityTerm:
                labelSelector:
                  matchExpressions:
                    - key: app
                      operator: In
                      values: ["node-app"]
                topologyKey: kubernetes.io/hostname
      topologySpreadConstraints:
        - maxSkew: 1
          topologyKey: topology.kubernetes.io/zone
          whenUnsatisfiable: DoNotSchedule
          labelSelector:
            matchLabels:
              app: node-app
```

### Tuning Guidelines

| Parameter | Default | Recommended | Notes |
|-----------|---------|-------------|-------|
| `UV_THREADPOOL_SIZE` | 4 | 8-16 | Match to I/O-bound workload |
| `--max-old-space-size` | ~1.5GB | 70% of limit | Leave room for overhead |
| `terminationGracePeriodSeconds` | 30 | 45-60 | Allow long requests to finish |
| `preStop` sleep | none | 10s | Drain from load balancer first |
| `initialDelaySeconds` | 0 | Use startupProbe | Prevent premature restarts |
| CPU request | none | realistic baseline | Affects scheduling priority |
| Memory request | none | realistic baseline | Affects scheduling priority |

## Best Practices Checklist

- [ ] Use `ReadWriteOnce` for databases, `ReadWriteMany` for shared assets
- [ ] Set `WaitForFirstConsumer` binding mode for multi-zone clusters
- [ ] Always set resource requests AND limits
- [ ] Use LimitRange and ResourceQuota per namespace
- [ ] Configure PodDisruptionBudgets for all critical services
- [ ] Use topology spread constraints for zone redundancy
- [ ] Implement startup probes for slow-starting applications
- [ ] Use HPA with custom metrics for traffic-based scaling
- [ ] Set `minAvailable: 2` or `maxUnavailable: 1` on PDBs
- [ ] Test autoscaling behavior under load before production
- [ ] Use StatefulSets with headless services for databases
- [ ] Configure `volumeClaimTemplates` for per-pod storage

## Cross-References

- See [Kubernetes Patterns](./01-kubernetes-patterns.md) for core deployment configs
- See [Service Mesh & Docker Swarm](./02-service-mesh-docker-swarm.md) for service mesh setup
- See [Docker](../docker/01-dockerfile.md) for container image optimization
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for Prometheus/Grafana setup
- See [Performance](../10-performance-optimization/01-nodejs-tuning.md) for Node.js tuning
- See [Security](../07-container-security/01-image-scanning.md) for container hardening

## Next Steps

Continue to [Serverless Deployment](../04-serverless-deployment/01-lambda-patterns.md).
