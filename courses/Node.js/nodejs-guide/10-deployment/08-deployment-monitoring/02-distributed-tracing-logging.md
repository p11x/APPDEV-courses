# Distributed Tracing and Log Aggregation

## What You'll Learn

- OpenTelemetry SDK setup with auto-instrumentation and manual spans
- Context propagation across services
- Trace backends: Jaeger, Zipkin, Grafana Tempo
- ELK stack and Loki log aggregation
- Fluentd/Fluent Bit container log collection
- Structured logging with Pino (correlation IDs, trace context)
- Log parsing, enrichment, routing, and retention
- Sentry error tracking and alerting

## OpenTelemetry SDK Setup

Install the core dependencies:

```bash
npm install @opentelemetry/sdk-node \
  @opentelemetry/api \
  @opentelemetry/auto-instrumentations-node \
  @opentelemetry/exporter-trace-otlp-http \
  @opentelemetry/exporter-metrics-otlp-http \
  @opentelemetry/resources \
  @opentelemetry/semantic-conventions \
  @opentelemetry/sdk-trace-node
```

### Full SDK Initialization

```javascript
// src/tracing.js — must be imported BEFORE any other module
import { NodeSDK } from '@opentelemetry/sdk-node';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { Resource } from '@opentelemetry/resources';
import {
  ATTR_SERVICE_NAME,
  ATTR_SERVICE_VERSION,
  ATTR_DEPLOYMENT_ENVIRONMENT_NAME,
} from '@opentelemetry/semantic-conventions';
import { BatchSpanProcessor } from '@opentelemetry/sdk-trace-node';
import { diag, DiagConsoleLogger, DiagLogLevel } from '@opentelemetry/api';

// Enable debug logging in development
if (process.env.OTEL_DEBUG === 'true') {
  diag.setLogger(new DiagConsoleLogger(), DiagLogLevel.DEBUG);
}

const traceExporter = new OTLPTraceExporter({
  url: process.env.OTEL_EXPORTER_OTLP_TRACES_ENDPOINT
    || 'http://localhost:4318/v1/traces',
  headers: {},
  timeoutMillis: 15000,
});

const metricExporter = new OTLPMetricExporter({
  url: process.env.OTEL_EXPORTER_OTLP_METRICS_ENDPOINT
    || 'http://localhost:4318/v1/metrics',
});

const metricReader = new PeriodicExportingMetricReader({
  exporter: metricExporter,
  exportIntervalMillis: 30000,
});

const sdk = new NodeSDK({
  resource: new Resource({
    [ATTR_SERVICE_NAME]: process.env.OTEL_SERVICE_NAME || 'node-app',
    [ATTR_SERVICE_VERSION]: process.env.APP_VERSION || '1.0.0',
    [ATTR_DEPLOYMENT_ENVIRONMENT_NAME]: process.env.NODE_ENV || 'development',
    'service.namespace': 'ecommerce',
    'service.instance.id': process.env.HOSTNAME || 'local',
  }),
  traceExporter,
  spanProcessor: new BatchSpanProcessor(traceExporter, {
    maxQueueSize: 2048,
    maxExportBatchSize: 512,
    scheduledDelayMillis: 5000,
    exportTimeoutMillis: 30000,
  }),
  metricReader,
  instrumentations: [
    getNodeAutoInstrumentations({
      '@opentelemetry/instrumentation-http': {
        ignoreIncomingRequestHook: (req) =>
          req.url === '/health' || req.url === '/metrics',
        requestHook: (span, request) => {
          span.setAttribute('http.request_id', request.headers['x-request-id']);
        },
      },
      '@opentelemetry/instrumentation-express': { enabled: true },
      '@opentelemetry/instrumentation-pg': { enabled: true },
      '@opentelemetry/instrumentation-redis': { enabled: true },
      '@opentelemetry/instrumentation-fs': { enabled: false },
    }),
  ],
});

sdk.start();

process.on('SIGTERM', async () => {
  await sdk.shutdown();
  process.exit(0);
});

export default sdk;
```

### Manual Span Creation and Context Propagation

```javascript
// src/services/order-service.js
import { trace, context, SpanStatusCode, propagation } from '@opentelemetry/api';

const tracer = trace.getTracer('order-service', '1.0.0');

export class OrderService {
  constructor(paymentClient, inventoryClient) {
    this.paymentClient = paymentClient;
    this.inventoryClient = inventoryClient;
  }

  async createOrder(req, orderData) {
    return tracer.startActiveSpan('createOrder', async (span) => {
      try {
        span.setAttribute('order.customer_id', orderData.customerId);
        span.setAttribute('order.item_count', orderData.items.length);

        // Child span: validate inventory
        const availability = await tracer.startActiveSpan(
          'checkInventory',
          async (invSpan) => {
            try {
              const result = await this.inventoryClient.check(
                orderData.items,
              );
              invSpan.setAttribute('inventory.available', result.available);
              return result;
            } finally {
              invSpan.end();
            }
          },
        );

        if (!availability.available) {
          span.setStatus({
            code: SpanStatusCode.ERROR,
            message: 'Items out of stock',
          });
          span.setAttribute('order.status', 'rejected');
          throw new Error('Items out of stock');
        }

        // Child span: process payment with propagated context
        const payment = await tracer.startActiveSpan(
          'processPayment',
          { attributes: { 'payment.amount': orderData.totalAmount } },
          async (paySpan) => {
            try {
              // Inject trace context into outgoing HTTP headers
              const headers = {};
              propagation.inject(context.active(), headers);

              const result = await this.paymentClient.charge({
                amount: orderData.totalAmount,
                currency: orderData.currency,
                customerId: orderData.customerId,
                traceHeaders: headers,
              });

              paySpan.setAttribute('payment.id', result.transactionId);
              return result;
            } catch (err) {
              paySpan.recordException(err);
              paySpan.setStatus({ code: SpanStatusCode.ERROR, message: err.message });
              throw err;
            } finally {
              paySpan.end();
            }
          },
        );

        span.setAttribute('order.id', payment.orderId);
        span.setAttribute('order.status', 'confirmed');
        span.setStatus({ code: SpanStatusCode.OK });

        return { orderId: payment.orderId, status: 'confirmed' };
      } catch (err) {
        span.recordException(err);
        span.setStatus({ code: SpanStatusCode.ERROR, message: err.message });
        throw err;
      } finally {
        span.end();
      }
    });
  }
}

// Express middleware: extract incoming trace context
import { context as otelContext } from '@opentelemetry/api';

export function traceContextMiddleware(req, res, next) {
  // Propagation is handled automatically by HTTP instrumentation,
  // but for custom headers or messaging:
  const parentContext = propagation.extract(
    otelContext.active(),
    req.headers,
  );

  otelContext.with(parentContext, () => {
    const span = trace.getActiveSpan();
    if (span) {
      span.setAttribute('http.request_id', req.headers['x-request-id']);
      span.setAttribute('user.id', req.user?.id || 'anonymous');
    }
    next();
  });
}
```

## Trace Collection Backends

### Jaeger (All-in-One for Development)

```yaml
# docker-compose.jaeger.yml
services:
  jaeger:
    image: jaegertracing/all-in-one:1.52
    ports:
      - "16686:16686"   # Jaeger UI
      - "4317:4317"     # OTLP gRPC
      - "4318:4318"     # OTLP HTTP
    environment:
      COLLECTOR_OTLP_ENABLED: "true"
    volumes:
      - jaeger-data:/badger

volumes:
  jaeger-data:
```

### Jaeger Production (Kubernetes)

```yaml
# jaeger-production.yaml — uses Elasticsearch as storage
apiVersion: jaegertracing.io/v1
kind: Jaeger
metadata:
  name: jaeger-production
spec:
  strategy: production
  storage:
    type: elasticsearch
    options:
      es:
        server-urls: http://elasticsearch:9200
        index-prefix: jaeger
  collector:
    resources:
      requests:
        memory: 512Mi
        cpu: 250m
      limits:
        memory: 1Gi
        cpu: 500m
```

### Zipkin

```yaml
# docker-compose.zipkin.yml
services:
  zipkin:
    image: openzipkin/zipkin:3
    ports:
      - "9411:9411"
    environment:
      STORAGE_TYPE: elasticsearch
      ES_HOSTS: http://elasticsearch:9200
```

```javascript
// Zipkin exporter configuration
import { ZipkinExporter } from '@opentelemetry/exporter-zipkin';

const zipkinExporter = new ZipkinExporter({
  url: process.env.ZIPKIN_URL || 'http://localhost:9411/api/v2/spans',
  serviceName: 'node-app',
});
```

### Grafana Tempo (Cloud-Native Tracing)

```yaml
# tempo.yaml — Grafana Tempo local config
server:
  http_listen_port: 3200

distributor:
  receivers:
    otlp:
      protocols:
        http:
          endpoint: "0.0.0.0:4318"
        grpc:
          endpoint: "0.0.0.0:4317"

storage:
  trace:
    backend: s3
    s3:
      bucket: tempo-traces
      endpoint: s3.us-east-1.amazonaws.com
    wal:
      path: /var/tempo/wal
    local:
      path: /var/tempo/blocks

compactor:
  compaction:
    block_retention: 720h  # 30 days
```

```yaml
# docker-compose.tempo.yml
services:
  tempo:
    image: grafana/tempo:2.3.1
    ports:
      - "3200:3200"
      - "4317:4317"
      - "4318:4318"
    volumes:
      - ./tempo.yaml:/etc/tempo.yaml
      - tempo-data:/var/tempo
    command: ["-config.file=/etc/tempo.yaml"]

volumes:
  tempo-data:
```

## Log Aggregation

### ELK Stack (Elasticsearch, Logstash, Kibana)

```yaml
# docker-compose.elk.yml
services:
  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:8.12.0
    environment:
      - discovery.type=single-node
      - xpack.security.enabled=false
      - "ES_JAVA_OPTS=-Xms1g -Xmx1g"
    ports:
      - "9200:9200"
    volumes:
      - es-data:/usr/share/elasticsearch/data

  logstash:
    image: docker.elastic.co/logstash/logstash:8.12.0
    ports:
      - "5044:5044"
    volumes:
      - ./logstash/pipeline:/usr/share/logstash/pipeline
    depends_on:
      - elasticsearch

  kibana:
    image: docker.elastic.co/kibana/kibana:8.12.0
    ports:
      - "5601:5601"
    environment:
      ELASTICSEARCH_HOSTS: http://elasticsearch:9200
    depends_on:
      - elasticsearch

volumes:
  es-data:
```

```ruby
# logstash/pipeline/node-app.conf
input {
  tcp {
    port => 5044
    codec => json_lines
  }
}

filter {
  # Parse Pino JSON logs
  if [type] == "node-app" {
    mutate {
      rename => { "msg" => "message" }
    }
    date {
      match => ["time", "ISO8601"]
      target => "@timestamp"
    }
    if [err] {
      mutate {
        add_field => { "error_message" => "%{[err][message]}" }
        add_field => { "error_stack" => "%{[err][stack]}" }
      }
    }
    # GeoIP enrichment for request logs
    if [req][ip] {
      geoip {
        source => "[req][ip]"
        target => "geoip"
      }
    }
  }
}

output {
  elasticsearch {
    hosts => ["http://elasticsearch:9200"]
    index => "node-app-%{+YYYY.MM.dd}"
  }
}
```

### Loki + Grafana (Lightweight Alternative)

```yaml
# docker-compose.loki.yml
services:
  loki:
    image: grafana/loki:2.9.4
    ports:
      - "3100:3100"
    volumes:
      - ./loki-config.yaml:/etc/loki/local-config.yaml
      - loki-data:/loki
    command: ["-config.file=/etc/loki/local-config.yaml"]

  grafana:
    image: grafana/grafana:10.3.1
    ports:
      - "3000:3000"
    environment:
      GF_INSTALL_PLUGINS: ""
    volumes:
      - grafana-data:/var/lib/grafana

volumes:
  loki-data:
  grafana-data:
```

```yaml
# loki-config.yaml
auth_enabled: false

server:
  http_listen_port: 3100

common:
  path_prefix: /loki
  storage:
    filesystem:
      chunks_directory: /loki/chunks
      rules_directory: /loki/rules
  replication_factor: 1
  ring:
    kvstore:
      store: inmemory

schema_config:
  configs:
    - from: 2024-01-01
      store: tsdb
      object_config:
        filesystem: {}
      schema: v13
      index:
        prefix: index_
        period: 24h

limits_config:
  retention_period: 720h
  max_query_series: 5000

compactor:
  working_directory: /loki/compactor
  compaction_interval: 10m
  retention_enabled: true
  retention_delete_delay: 2h
  retention_delete_worker_count: 150
```

## Fluentd / Fluent Bit for Container Logs

### Fluent Bit (Lightweight DaemonSet)

```yaml
# fluent-bit-config.yaml
[SERVICE]
    Flush         5
    Daemon        off
    Log_Level     info
    Parsers_File  parsers.conf

[INPUT]
    Name              tail
    Path              /var/log/containers/*.log
    Parser            cri
    Tag               kube.*
    Refresh_Interval  5
    Mem_Buf_Limit     5MB
    Skip_Long_Lines   On

[FILTER]
    Name                kubernetes
    Match               kube.*
    Kube_URL            https://kubernetes.default.svc:443
    Kube_CA_File        /var/run/secrets/kubernetes.io/serviceaccount/ca.crt
    Kube_Token_File     /var/run/secrets/kubernetes.io/serviceaccount/token
    Merge_Log           On
    Keep_Log            Off
    K8S-Logging.Parser  On
    K8S-Logging.Exclude On

[FILTER]
    Name    rewrite_tag
    Match   kube.*
    Rule    $kubernetes['namespace'] ^production$  production.app.*
    Rule    $kubernetes['namespace'] ^staging$     staging.app.*

[OUTPUT]
    Name            loki
    Match           production.app.*
    Url             http://loki.monitoring.svc:3100/loki/api/v1/push
    Labels          {job="kubernetes", namespace="$kubernetes_namespace", app="$kubernetes_container_name"}
    BatchWait       1
    BatchSize       1048576

[OUTPUT]
    Name            es
    Match           staging.app.*
    Host            elasticsearch.monitoring.svc
    Port            9200
    Index           staging-logs
    Type            _doc

[PARSER]
    Name        cri
    Format      regex
    Regex       ^(?<time>[^ ]+) (?<stream>stdout|stderr) (?<logtag>[^ ]*) (?<log>.*)$
    Time_Key    time
    Time_Format %Y-%m-%dT%H:%M:%S.%L%z
```

### Kubernetes DaemonSet

```yaml
# fluent-bit-daemonset.yaml
apiVersion: apps/v1
kind: DaemonSet
metadata:
  name: fluent-bit
  namespace: logging
spec:
  selector:
    matchLabels:
      app: fluent-bit
  template:
    metadata:
      labels:
        app: fluent-bit
    spec:
      serviceAccountName: fluent-bit
      containers:
        - name: fluent-bit
          image: fluent/fluent-bit:2.2.0
          ports:
            - containerPort: 2020
          volumeMounts:
            - name: varlog
              mountPath: /var/log
              readOnly: true
            - name: containers
              mountPath: /var/log/containers
              readOnly: true
            - name: config
              mountPath: /fluent-bit/etc/
          resources:
            requests:
              cpu: 100m
              memory: 128Mi
            limits:
              cpu: 500m
              memory: 256Mi
      volumes:
        - name: varlog
          hostPath:
            path: /var/log
        - name: containers
          hostPath:
            path: /var/log/containers
        - name: config
          configMap:
            name: fluent-bit-config
```

## Structured Logging with Pino

```javascript
// src/logger.js
import pino from 'pino';
import { trace, context } from '@opentelemetry/api';
import { randomUUID } from 'node:crypto';

function getTraceContext() {
  const span = trace.getActiveSpan();
  if (!span) return {};

  const spanContext = span.spanContext();
  return {
    'trace.id': spanContext.traceId,
    'span.id': spanContext.spanId,
    'trace.flags': spanContext.traceFlags.toString(16),
  };
}

export const logger = pino({
  level: process.env.LOG_LEVEL || 'info',
  base: {
    service: process.env.OTEL_SERVICE_NAME || 'node-app',
    version: process.env.APP_VERSION,
    env: process.env.NODE_ENV,
    hostname: process.env.HOSTNAME,
  },
  formatters: {
    level(label) {
      return { level: label };
    },
    bindings(bindings) {
      return { pid: bindings.pid, host: bindings.hostname };
    },
  },
  timestamp: pino.stdTimeFunctions.isoTime,
  serializers: {
    req(req) {
      return {
        method: req.method,
        url: req.url,
        id: req.id,
        remoteAddress: req.socket?.remoteAddress,
        userAgent: req.headers?.['user-agent'],
      };
    },
    res(res) {
      return {
        statusCode: res.statusCode,
        contentLength: res.getHeader?.('content-length'),
      };
    },
    err: pino.stdSerializers.err,
  },
  redact: {
    paths: [
      'req.headers.authorization',
      'req.headers.cookie',
      'password',
      'creditCard',
      '*.token',
    ],
    censor: '[REDACTED]',
  },
  transport:
    process.env.NODE_ENV !== 'production'
      ? {
          target: 'pino-pretty',
          options: { colorize: true, translateTime: 'SYS:standard' },
        }
      : undefined,
});

// Child logger with trace context
export function getLogger(bindings = {}) {
  return logger.child({
    ...bindings,
    ...getTraceContext(),
  });
}
```

### Correlation ID Middleware

```javascript
// src/middleware/correlation.js
import { randomUUID } from 'node:crypto';
import { getLogger } from '../logger.js';

export function correlationMiddleware(req, res, next) {
  // Use existing correlation ID or generate a new one
  const correlationId =
    req.headers['x-correlation-id'] || req.headers['x-request-id'] || randomUUID();

  req.id = correlationId;
  res.setHeader('x-correlation-id', correlationId);

  // Attach enriched logger to request
  req.log = getLogger({
    correlationId,
    requestId: correlationId,
  });

  const start = process.hrtime.bigint();

  res.on('finish', () => {
    const durationMs = Number(process.hrtime.bigint() - start) / 1e6;

    req.log.info({
      type: 'http_request',
      req,
      res,
      durationMs: Math.round(durationMs * 100) / 100,
    });
  });

  next();
}
```

### Error Logging with Trace Context

```javascript
// src/middleware/error-handler.js
import { SpanStatusCode } from '@opentelemetry/api';
import { trace } from '@opentelemetry/api';

export function errorHandler(err, req, res, next) {
  const span = trace.getActiveSpan();

  if (span) {
    span.recordException(err);
    span.setStatus({ code: SpanStatusCode.ERROR, message: err.message });
    span.setAttribute('error.type', err.constructor.name);
  }

  req.log.error({
    err,
    type: 'unhandled_error',
    correlationId: req.id,
    path: req.path,
    method: req.method,
    statusCode: err.statusCode || 500,
  });

  res.status(err.statusCode || 500).json({
    error: {
      message: process.env.NODE_ENV === 'production'
        ? 'Internal server error'
        : err.message,
      correlationId: req.id,
    },
  });
}
```

## Sentry Integration

```bash
npm install @sentry/node @sentry/tracing
```

```javascript
// src/sentry.js
import * as Sentry from '@sentry/node';
import { nodeProfilingIntegration } from '@sentry/profiling-node';

export function initSentry(app) {
  Sentry.init({
    dsn: process.env.SENTRY_DSN,
    environment: process.env.NODE_ENV,
    release: process.env.APP_VERSION,
    integrations: [
      Sentry.httpIntegration(),
      Sentry.expressIntegration({ app }),
      Sentry.prismaIntegration(),
      nodeProfilingIntegration(),
    ],
    tracesSampleRate: process.env.NODE_ENV === 'production' ? 0.1 : 1.0,
    profilesSampleRate: 0.1,
    beforeSend(event, hint) {
      // Filter out noisy errors
      const error = hint.originalException;
      if (error?.code === 'ECONNRESET') return null;
      return event;
    },
  });
}

// Capturing errors manually
export function captureError(error, context = {}) {
  Sentry.withScope((scope) => {
    scope.setContext('additional', context);
    scope.setTag('component', context.component || 'unknown');
    if (context.userId) scope.setUser({ id: context.userId });
    Sentry.captureException(error);
  });
}
```

## Log Retention and Cost Optimization

```yaml
# Index lifecycle policy for Elasticsearch
# kibana → Stack Management → Index Lifecycle Policies
policy:
  phases:
    hot:
      actions:
        rollover:
          max_age: "1d"
          max_primary_shard_size: "50gb"
    warm:
      min_age: "7d"
      actions:
        shrink:
          number_of_shards: 1
        forcemerge:
          max_num_segments: 1
        set_priority:
          priority: 50
    cold:
      min_age: "30d"
      actions:
        set_priority:
          priority: 0
        freeze: {}
    delete:
      min_age: "90d"
      actions:
        delete: {}
```

```yaml
# Loki retention via compactor in loki-config.yaml
compactor:
  retention_enabled: true
  retention_delete_delay: 2h
  compaction_interval: 10m
limits_config:
  retention_period: 720h  # 30 days
  per_stream_rate_limit: 3MB
  max_entries_limit_per_query: 5000
```

## Best Practices Checklist

- [ ] Initialize OpenTelemetry SDK before importing application modules
- [ ] Use auto-instrumentation for HTTP, database, and cache clients
- [ ] Create manual spans for business-critical operations
- [ ] Propagate trace context across service boundaries via HTTP headers
- [ ] Inject `trace.id` and `span.id` into every log line via Pino child loggers
- [ ] Add `x-correlation-id` to every request for cross-service correlation
- [ ] Use Fluent Bit as a DaemonSet for container log collection
- [ ] Route logs to Loki or ELK based on namespace/environment
- [ ] Configure log retention policies aligned with compliance requirements
- [ ] Redact sensitive fields (tokens, passwords, PII) in structured logs
- [ ] Integrate Sentry for error aggregation with environment and release context
- [ ] Sample traces appropriately: 100% in dev, 10-20% in production

## Cross-References

- See [APM and Metrics](./01-apm-metrics.md) for Prometheus and Grafana setup
- See [Deployment Tracking](./03-deployment-tracking-dashboards.md) for dashboards and alerting
- See [Kubernetes Patterns](../03-container-orchestration/01-kubernetes-patterns.md) for K8s logging
- See [Security](../09-deployment-security/01-security-scanning.md) for log security practices

## Next Steps

Continue to [Deployment Tracking and Dashboards](./03-deployment-tracking-dashboards.md).
