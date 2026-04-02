# Deployment Monitoring and Observability

## What You'll Learn

- Application Performance Monitoring (APM)
- Infrastructure monitoring with Prometheus
- Log aggregation strategies
- Distributed tracing
- Deployment success tracking

## APM with Prometheus and Grafana

```javascript
// src/metrics.js
import { collectDefaultMetrics, Counter, Histogram, Gauge } from 'prom-client';

collectDefaultMetrics({ prefix: 'node_app_' });

export const httpRequestsTotal = new Counter({
    name: 'http_requests_total',
    help: 'Total HTTP requests',
    labelNames: ['method', 'path', 'status'],
});

export const httpRequestDuration = new Histogram({
    name: 'http_request_duration_seconds',
    help: 'HTTP request duration in seconds',
    labelNames: ['method', 'path'],
    buckets: [0.01, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5, 10],
});

export const activeConnections = new Gauge({
    name: 'active_connections',
    help: 'Number of active connections',
});

export const dbQueryDuration = new Histogram({
    name: 'db_query_duration_seconds',
    help: 'Database query duration',
    labelNames: ['operation', 'table'],
    buckets: [0.001, 0.005, 0.01, 0.05, 0.1, 0.5, 1],
});

// Metrics middleware
export function metricsMiddleware(req, res, next) {
    const start = process.hrtime.bigint();
    activeConnections.inc();

    res.on('finish', () => {
        const duration = Number(process.hrtime.bigint() - start) / 1e9;
        const route = req.route?.path || req.path;

        httpRequestsTotal.inc({
            method: req.method,
            path: route,
            status: res.statusCode,
        });

        httpRequestDuration.observe(
            { method: req.method, path: route },
            duration
        );

        activeConnections.dec();
    });

    next();
}

// Metrics endpoint
app.get('/metrics', async (req, res) => {
    const { register } = await import('prom-client');
    res.set('Content-Type', register.contentType);
    res.end(await register.metrics());
});
```

## Structured Logging

```javascript
// src/logger.js
import pino from 'pino';

export const logger = pino({
    level: process.env.LOG_LEVEL || 'info',
    formatters: {
        level(label) {
            return { level: label };
        },
    },
    timestamp: pino.stdTimeFunctions.isoTime,
    serializers: {
        req(req) {
            return {
                method: req.method,
                url: req.url,
                id: req.id,
                userId: req.user?.id,
            };
        },
        res(res) {
            return {
                statusCode: res.statusCode,
            };
        },
    },
    transport: process.env.NODE_ENV !== 'production'
        ? { target: 'pino-pretty', options: { colorize: true } }
        : undefined,
});

// Request logging middleware
export function requestLogger(req, res, next) {
    const start = Date.now();

    res.on('finish', () => {
        const duration = Date.now() - start;

        logger.info({
            req,
            res,
            duration,
            type: 'request',
        });

        if (res.statusCode >= 500) {
            logger.error({
                req,
                res,
                duration,
                type: 'error_response',
            });
        }
    });

    next();
}
```

## Distributed Tracing

```javascript
// src/tracing.js
import { NodeSDK } from '@opentelemetry/sdk-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { HttpInstrumentation } from '@opentelemetry/instrumentation-http';
import { PgInstrumentation } from '@opentelemetry/instrumentation-pg';

const sdk = new NodeSDK({
    traceExporter: new OTLPTraceExporter({
        url: process.env.OTEL_EXPORTER_OTLP_ENDPOINT || 'http://localhost:4318/v1/traces',
    }),
    instrumentations: [
        new HttpInstrumentation(),
        new PgInstrumentation(),
    ],
    serviceName: 'node-app',
});

sdk.start();

// Custom spans
import { trace } from '@opentelemetry/api';

const tracer = trace.getTracer('node-app');

export async function processOrder(order) {
    return tracer.startActiveSpan('processOrder', async (span) => {
        try {
            span.setAttribute('order.id', order.id);
            span.setAttribute('order.total', order.total);

            const items = await tracer.startActiveSpan('validateItems', async (itemSpan) => {
                const result = await validateOrderItems(order.items);
                itemSpan.setAttribute('item.count', result.length);
                itemSpan.end();
                return result;
            });

            const payment = await tracer.startActiveSpan('processPayment', async (paySpan) => {
                const result = await chargePayment(order.total);
                paySpan.setAttribute('payment.id', result.id);
                paySpan.end();
                return result;
            });

            span.setAttribute('success', true);
            return { items, payment };
        } catch (err) {
            span.recordException(err);
            span.setStatus({ code: 2, message: err.message });
            throw err;
        } finally {
            span.end();
        }
    });
}
```

## Deployment Tracking

```javascript
// Track deployment events
class DeploymentTracker {
    constructor(redis, slackWebhook) {
        this.redis = redis;
        this.slackWebhook = slackWebhook;
    }

    async recordDeployment(deployment) {
        const record = {
            id: crypto.randomUUID(),
            version: deployment.version,
            environment: deployment.environment,
            deployedBy: deployment.user,
            timestamp: new Date().toISOString(),
            status: 'started',
        };

        await this.redis.lPush('deployments', JSON.stringify(record));
        await this.redis.lTrim('deployments', 0, 99);

        await this.notifySlack(record);
        return record;
    }

    async markSuccess(deploymentId) {
        await this.updateStatus(deploymentId, 'success');
    }

    async markFailed(deploymentId, error) {
        await this.updateStatus(deploymentId, 'failed', error);
        await this.notifySlack({ status: 'failed', error });
    }

    async updateStatus(id, status, error) {
        // Update deployment record
        const deployments = await this.redis.lRange('deployments', 0, -1);
        for (let i = 0; i < deployments.length; i++) {
            const record = JSON.parse(deployments[i]);
            if (record.id === id) {
                record.status = status;
                record.completedAt = new Date().toISOString();
                if (error) record.error = error;
                await this.redis.lSet('deployments', i, JSON.stringify(record));
                break;
            }
        }
    }

    async notifySlack(deployment) {
        if (!this.slackWebhook) return;

        await fetch(this.slackWebhook, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                text: `Deployment ${deployment.status}: ${deployment.version} to ${deployment.environment}`,
            }),
        });
    }
}
```

## Best Practices Checklist

- [ ] Expose /metrics endpoint for Prometheus
- [ ] Use structured JSON logging (pino)
- [ ] Implement distributed tracing (OpenTelemetry)
- [ ] Set up Grafana dashboards for key metrics
- [ ] Alert on error rate, latency, and availability
- [ ] Track deployment success/failure rates
- [ ] Log request/response with correlation IDs
- [ ] Monitor resource usage (CPU, memory, disk)

## Cross-References

- See [Health Checks](../monitoring/01-health-checks.md) for health endpoints
- See [Kubernetes](../03-container-orchestration/01-kubernetes-patterns.md) for K8s monitoring
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for deployment tracking
- See [Performance](../10-performance-optimization/01-performance-optimization.md) for optimization

## Next Steps

Continue to [Deployment Security](../09-deployment-security/01-security-scanning.md).
