# Kubernetes Deployment Patterns

## What You'll Learn

- Kubernetes deployment configurations
- Service mesh implementation
- Auto-scaling strategies
- Persistent storage management
- Health checks and probes

## Complete Kubernetes Setup

```yaml
# k8s/namespace.yaml
apiVersion: v1
kind: Namespace
metadata:
  name: my-app
  labels:
    name: my-app
    istio-injection: enabled
---
# k8s/configmap.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
  namespace: my-app
data:
  NODE_ENV: production
  LOG_LEVEL: info
  CACHE_TTL: "300"
---
# k8s/secret.yaml
apiVersion: v1
kind: Secret
metadata:
  name: app-secrets
  namespace: my-app
type: Opaque
stringData:
  DATABASE_URL: postgresql://user:pass@db:5432/myapp
  JWT_SECRET: your-secret-here
  REDIS_URL: redis://redis:6379
---
# k8s/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  namespace: my-app
  labels:
    app: node-app
    version: v1
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
        version: v1
    spec:
      terminationGracePeriodSeconds: 30
      containers:
        - name: node-app
          image: registry.example.com/my-app:v1.2.3
          imagePullPolicy: Always
          ports:
            - name: http
              containerPort: 3000
              protocol: TCP
          envFrom:
            - configMapRef:
                name: app-config
            - secretRef:
                name: app-secrets
          resources:
            requests:
              cpu: 100m
              memory: 128Mi
            limits:
              cpu: 500m
              memory: 256Mi
          livenessProbe:
            httpGet:
              path: /health/live
              port: http
            initialDelaySeconds: 15
            periodSeconds: 20
            timeoutSeconds: 5
            failureThreshold: 3
          readinessProbe:
            httpGet:
              path: /health/ready
              port: http
            initialDelaySeconds: 5
            periodSeconds: 10
            timeoutSeconds: 3
            failureThreshold: 3
          startupProbe:
            httpGet:
              path: /health/startup
              port: http
            initialDelaySeconds: 10
            periodSeconds: 5
            failureThreshold: 30
          lifecycle:
            preStop:
              exec:
                command: ["sh", "-c", "sleep 5"]
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
---
# k8s/service.yaml
apiVersion: v1
kind: Service
metadata:
  name: node-app-service
  namespace: my-app
spec:
  selector:
    app: node-app
  ports:
    - name: http
      port: 80
      targetPort: http
      protocol: TCP
  type: ClusterIP
---
# k8s/ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: node-app-ingress
  namespace: my-app
  annotations:
    kubernetes.io/ingress.class: nginx
    cert-manager.io/cluster-issuer: letsencrypt-prod
    nginx.ingress.kubernetes.io/rate-limit: "100"
    nginx.ingress.kubernetes.io/rate-limit-window: "1m"
spec:
  tls:
    - hosts:
        - api.example.com
      secretName: api-tls
  rules:
    - host: api.example.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: node-app-service
                port:
                  name: http
---
# k8s/hpa.yaml
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
    - type: Pods
      pods:
        metric:
          name: http_requests_per_second
        target:
          type: AverageValue
          averageValue: "1000"
  behavior:
    scaleUp:
      stabilizationWindowSeconds: 30
      policies:
        - type: Pods
          value: 4
          periodSeconds: 60
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
        - type: Pods
          value: 1
          periodSeconds: 60
```

## Health Check Endpoints

```javascript
// src/health.js
import { Router } from 'express';
import { Pool } from 'pg';
import { createClient } from 'redis';

const healthRouter = Router();

let dbPool;
let redisClient;
let isReady = false;

export function initHealthChecks(db, redis) {
    dbPool = db;
    redisClient = redis;
    isReady = true;
}

// Liveness: Is the process alive?
healthRouter.get('/health/live', (req, res) => {
    res.json({ status: 'alive', uptime: process.uptime() });
});

// Readiness: Can it accept traffic?
healthRouter.get('/health/ready', async (req, res) => {
    if (!isReady) {
        return res.status(503).json({ status: 'not ready' });
    }

    const checks = {};

    try {
        await dbPool.query('SELECT 1');
        checks.database = 'ok';
    } catch {
        checks.database = 'error';
    }

    try {
        await redisClient.ping();
        checks.redis = 'ok';
    } catch {
        checks.redis = 'error';
    }

    const healthy = Object.values(checks).every(v => v === 'ok');
    res.status(healthy ? 200 : 503).json({
        status: healthy ? 'ready' : 'not ready',
        checks,
    });
});

// Startup: Has it finished initializing?
healthRouter.get('/health/startup', (req, res) => {
    res.json({ started: true, pid: process.pid });
});

// Detailed health
healthRouter.get('/health/detailed', async (req, res) => {
    res.json({
        status: 'ok',
        uptime: process.uptime(),
        memory: process.memoryUsage(),
        pid: process.pid,
        version: process.version,
        timestamp: new Date().toISOString(),
    });
});

export default healthRouter;
```

## Graceful Shutdown

```javascript
// src/server.js
import express from 'express';

const app = express();
let server;

export function startServer(port) {
    server = app.listen(port, () => {
        console.log(`Server running on port ${port}`);
    });

    return server;
}

// Graceful shutdown handler
const SHUTDOWN_TIMEOUT = 30000; // 30 seconds

async function gracefulShutdown(signal) {
    console.log(`Received ${signal}, starting graceful shutdown...`);

    // Stop accepting new connections
    server.close(() => {
        console.log('HTTP server closed');
    });

    // Set a timeout for forced shutdown
    const forceTimeout = setTimeout(() => {
        console.error('Forced shutdown after timeout');
        process.exit(1);
    }, SHUTDOWN_TIMEOUT);

    try {
        // Close database connections
        await dbPool?.end();
        console.log('Database connections closed');

        // Close Redis connections
        await redisClient?.quit();
        console.log('Redis connections closed');

        clearTimeout(forceTimeout);
        console.log('Graceful shutdown complete');
        process.exit(0);
    } catch (err) {
        console.error('Error during shutdown:', err);
        process.exit(1);
    }
}

process.on('SIGTERM', () => gracefulShutdown('SIGTERM'));
process.on('SIGINT', () => gracefulShutdown('SIGINT'));
```

## Best Practices Checklist

- [ ] Use RollingUpdate strategy for zero-downtime deployments
- [ ] Implement liveness, readiness, and startup probes
- [ ] Set resource requests and limits
- [ ] Use pod anti-affinity for high availability
- [ ] Implement graceful shutdown (SIGTERM handling)
- [ ] Use ConfigMaps and Secrets for configuration
- [ ] Set up Horizontal Pod Autoscaler
- [ ] Use pod disruption budgets

## Cross-References

- See [Docker](../docker/01-dockerfile.md) for container setup
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for patterns
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability
- See [Security](../07-container-security/01-image-scanning.md) for hardening

## Next Steps

Continue to [Serverless Deployment](../04-serverless-deployment/01-lambda-patterns.md).
