# Monitoring and Observability for NodeMark

## What You'll Build In This File

Structured logging, application metrics, error tracking, and monitoring dashboards for production NodeMark.

## Structured Logging with Pino

```javascript
// src/services/logger.js — Production logging
import pino from 'pino';
import { config } from '../config/index.js';

export const logger = pino({
    level: config.nodeEnv === 'production' ? 'info' : 'debug',
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
                userId: req.user?.userId,
                ip: req.ip,
            };
        },
        res(res) {
            return { statusCode: res.statusCode };
        },
        err: pino.stdSerializers.err,
    },
    transport: config.nodeEnv !== 'production'
        ? { target: 'pino-pretty', options: { colorize: true } }
        : undefined,
});

// Request logging middleware
export function requestLogger(req, res, next) {
    const start = Date.now();

    res.on('finish', () => {
        const duration = Date.now() - start;
        const logData = {
            req,
            res,
            duration,
            type: 'request',
        };

        if (res.statusCode >= 500) {
            logger.error(logData);
        } else if (res.statusCode >= 400) {
            logger.warn(logData);
        } else {
            logger.info(logData);
        }
    });

    next();
}
```

## Application Metrics

```javascript
// src/services/metrics.js — Prometheus metrics
import { Counter, Histogram, Gauge, collectDefaultMetrics } from 'prom-client';

collectDefaultMetrics({ prefix: 'nodemark_' });

export const httpRequestsTotal = new Counter({
    name: 'http_requests_total',
    help: 'Total HTTP requests',
    labelNames: ['method', 'path', 'status'],
});

export const httpRequestDuration = new Histogram({
    name: 'http_request_duration_seconds',
    help: 'Request duration in seconds',
    labelNames: ['method', 'path'],
    buckets: [0.01, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5],
});

export const activeConnections = new Gauge({
    name: 'active_connections',
    help: 'Active connections',
});

export const dbQueryDuration = new Histogram({
    name: 'db_query_duration_seconds',
    help: 'Database query duration',
    labelNames: ['operation'],
    buckets: [0.001, 0.005, 0.01, 0.05, 0.1, 0.5],
});

export const bookmarksTotal = new Gauge({
    name: 'bookmarks_total',
    help: 'Total bookmarks in system',
});

// Metrics middleware
export function metricsMiddleware(req, res, next) {
    const start = process.hrtime.bigint();
    activeConnections.inc();

    res.on('finish', () => {
        const duration = Number(process.hrtime.bigint() - start) / 1e9;
        const route = req.route?.path || req.path;

        httpRequestsTotal.inc({ method: req.method, path: route, status: res.statusCode });
        httpRequestDuration.observe({ method: req.method, path: route }, duration);
        activeConnections.dec();
    });

    next();
}

// Metrics endpoint
import { register } from 'prom-client';
app.get('/metrics', async (req, res) => {
    res.set('Content-Type', register.contentType);
    res.end(await register.metrics());
});
```

## Error Tracking

```javascript
// src/services/error-tracker.js — Error tracking service
import { logger } from './logger.js';

class ErrorTracker {
    constructor() {
        this.errors = [];
        this.maxErrors = 1000;
    }

    capture(error, context = {}) {
        const errorEntry = {
            id: crypto.randomUUID(),
            message: error.message,
            stack: error.stack,
            type: error.name,
            context: {
                path: context.path,
                method: context.method,
                userId: context.userId,
                ip: context.ip,
            },
            timestamp: new Date().toISOString(),
        };

        this.errors.push(errorEntry);
        if (this.errors.length > this.maxErrors) {
            this.errors.shift();
        }

        logger.error({ err: error, ...context, type: 'error' });

        // In production, send to Sentry/LogRocket/etc.
    }

    getRecent(limit = 50) {
        return this.errors.slice(-limit);
    }

    getStats(sinceMs = 3600000) {
        const since = Date.now() - sinceMs;
        const recent = this.errors.filter(e => new Date(e.timestamp).getTime() > since);

        const byType = {};
        for (const error of recent) {
            byType[error.type] = (byType[error.type] || 0) + 1;
        }

        return { total: recent.length, byType };
    }
}

export const errorTracker = new ErrorTracker();

// Global error handler integration
app.use((err, req, res, next) => {
    errorTracker.capture(err, {
        path: req.path,
        method: req.method,
        userId: req.user?.userId,
        ip: req.ip,
    });
    // ... send error response
});
```

## Monitoring Dashboard Data

```javascript
// src/routes/admin.js — Admin monitoring endpoints
import { Router } from 'express';
import { authenticate } from '../middleware/auth.js';
import { authorize } from '../middleware/rbac.js';
import { getPoolStats } from '../db/index.js';
import { errorTracker } from '../services/error-tracker.js';
import { register } from 'prom-client';

const router = Router();
router.use(authenticate, authorize('system:manage'));

router.get('/admin/health', async (req, res) => {
    res.json({
        uptime: process.uptime(),
        memory: process.memoryUsage(),
        pool: getPoolStats(),
        errors: errorTracker.getStats(),
        timestamp: new Date().toISOString(),
    });
});

router.get('/admin/errors', (req, res) => {
    res.json(errorTracker.getRecent(50));
});

export { router as adminRouter };
```

## How It Connects

- Logging follows [21-logging-monitoring](../../../21-logging-monitoring/)
- Metrics follow [10-deployment/08-deployment-monitoring/](../../../10-deployment/08-deployment-monitoring/)
- Error tracking follows [09-testing/09-security-performance/](../../../09-testing/09-security-performance/)

## Common Mistakes

- Logging sensitive data (passwords, tokens)
- Not using structured logging
- Missing error context in logs
- Not setting up alerts for error rate spikes

## Try It Yourself

### Exercise 1: View Metrics
Start the app and visit `/metrics` to see Prometheus output.

### Exercise 2: Error Tracking
Trigger an error and check it appears in `/admin/errors`.

### Exercise 3: Set Up Grafana
Create a dashboard showing request rate and error rate.

## Next Steps

Continue to [21-documentation-quality/01-api-documentation.md](../21-documentation-quality/01-api-documentation.md).
