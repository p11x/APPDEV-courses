# Worker Thread Deployment and Operations

## What You'll Learn

- Production worker deployment configuration
- Worker monitoring and alerting
- Worker scaling strategies
- Worker health checks
- Operational runbooks

## Production Worker Configuration

```js
// config/workers.js — Production worker configuration
export default {
    // Worker pool configuration
    pool: {
        // Auto-detect CPU count
        size: parseInt(process.env.WORKER_POOL_SIZE) || 
              Math.min(require('node:os').availableParallelism(), 8),
        
        // Resource limits per worker
        resourceLimits: {
            maxOldGenerationSizeMb: parseInt(process.env.WORKER_MAX_HEAP) || 256,
            maxYoungGenerationSizeMb: 64,
            codeRangeSizeMb: 16,
        },
        
        // Restart policy
        maxRestarts: parseInt(process.env.WORKER_MAX_RESTARTS) || 5,
        restartDelay: parseInt(process.env.WORKER_RESTART_DELAY) || 1000,
        
        // Timeouts
        taskTimeout: parseInt(process.env.WORKER_TASK_TIMEOUT) || 30000,
        startupTimeout: parseInt(process.env.WORKER_STARTUP_TIMEOUT) || 10000,
    },
    
    // Monitoring
    monitoring: {
        enabled: process.env.WORKER_MONITORING !== 'false',
        metricsInterval: parseInt(process.env.WORKER_METRICS_INTERVAL) || 10000,
        alertOnCrash: true,
        alertOnHighMemory: true,
        memoryThresholdMB: parseInt(process.env.WORKER_MEMORY_ALERT_MB) || 200,
    },
};
```

## Worker Health Checks

```js
// health-check.js — Worker health check endpoint
import { createServer } from 'node:http';
import { WorkerPool } from './lib/worker-pool.js';

const pool = new WorkerPool('./workers/compute.js');
await pool.start();

const healthServer = createServer((req, res) => {
    if (req.url === '/health/workers') {
        const stats = pool.getStats();
        const healthy = stats.activeWorkers > 0 && stats.queuedTasks < 100;

        res.writeHead(healthy ? 200 : 503, {
            'Content-Type': 'application/json',
        });

        res.end(JSON.stringify({
            status: healthy ? 'healthy' : 'degraded',
            workers: {
                total: stats.poolSize,
                active: stats.activeWorkers,
                queued: stats.queuedTasks,
                completed: stats.completed,
                failed: stats.failed,
            },
            timestamp: new Date().toISOString(),
        }));
    }
});

healthServer.listen(9999);
```

## Kubernetes Worker Deployment

```yaml
# k8s/worker-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
spec:
  replicas: 3
  template:
    spec:
      containers:
        - name: app
          image: my-app:latest
          env:
            - name: WORKER_POOL_SIZE
              value: "4"
            - name: WORKER_MAX_HEAP
              value: "256"
          resources:
            requests:
              memory: "512Mi"
              cpu: "500m"
            limits:
              memory: "1Gi"
              cpu: "2000m"
          livenessProbe:
            httpGet:
              path: /health/workers
              port: 9999
            initialDelaySeconds: 15
            periodSeconds: 30
```

## Operational Runbook

```
Worker Thread Operations Runbook:
─────────────────────────────────────────────

Worker Crash Loop:
1. Check logs: docker logs <container> | grep -i worker
2. Check memory: /health/workers endpoint
3. Reduce pool size: WORKER_POOL_SIZE=2
4. Increase heap: WORKER_MAX_HEAP=512
5. If persists: disable workers temporarily

High Memory Usage:
1. Check worker count: /health/workers
2. Reduce pool: WORKER_POOL_SIZE=2
3. Reduce heap per worker: WORKER_MAX_HEAP=128
4. Check for memory leaks: --inspect workers

Slow Performance:
1. Check pool utilization: /health/workers
2. Increase pool size: WORKER_POOL_SIZE=8
3. Check queue depth (should be < 10)
4. Profile with --prof flag
```

## Common Mistakes

- Not setting resource limits (OOM in production)
- Not implementing health checks for workers
- Not monitoring worker crash rates
- Using development pool sizes in production

## Try It Yourself

### Exercise 1: Health Check
Implement the health check endpoint and test with curl.

### Exercise 2: Docker Deploy
Deploy a worker-enabled app with Docker and verify pool size.

### Exercise 3: Load Test
Load test with different pool sizes and find optimal configuration.

## Next Steps

Continue to [Future Trends](../12-future-trends/01-webassembly-workers.md).
