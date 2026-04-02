# Deployment Tracking and Production Dashboards

## What You'll Learn

- Deployment markers and annotations in Grafana
- Canary analysis automation
- RED and USE metric dashboards for Node.js
- Infrastructure monitoring (node-exporter, cAdvisor, kube-state-metrics)
- PagerDuty / OpsGenie alerting with escalation policies
- SLI/SLO/SLA definitions and monitoring
- Synthetic monitoring and Real User Monitoring (RUM)
- On-call runbook automation
- Performance baseline establishment

## Deployment Markers and Annotations

### Grafana Annotations API

```javascript
// src/deployment/annotations.js
export class GrafanaAnnotations {
  constructor(grafanaUrl, apiToken) {
    this.baseUrl = `${grafanaUrl}/api/annotations`;
    this.headers = {
      Authorization: `Bearer ${apiToken}`,
      'Content-Type': 'application/json',
    };
  }

  async markDeployment({ version, environment, tags = [], text }) {
    const annotation = {
      dashboardUID: process.env.GRAFANA_DASHBOARD_UID,
      time: Date.now(),
      timeEnd: 0,
      tags: ['deployment', environment, ...tags],
      text: text || `Deployed ${version} to ${environment}`,
    };

    const res = await fetch(this.baseUrl, {
      method: 'POST',
      headers: this.headers,
      body: JSON.stringify(annotation),
    });

    return res.json();
  }

  async markRollback({ version, environment, reason }) {
    return this.markDeployment({
      version,
      environment,
      tags: ['rollback'],
      text: `ROLLBACK: ${version} on ${environment} — ${reason}`,
    });
  }

  async getDeploymentAnnotations(environment, from, to) {
    const params = new URLSearchParams({
      tags: `deployment,${environment}`,
      from: from.getTime().toString(),
      to: to.getTime().toString(),
    });

    const res = await fetch(`${this.baseUrl}?${params}`, {
      headers: this.headers,
    });

    return res.json();
  }
}
```

### Automated Canary Analysis

```javascript
// src/deployment/canary-analyzer.js
import { getPrometheusClient } from '../metrics/prometheus.js';

export class CanaryAnalyzer {
  constructor({ grafanaUrl, apiToken, prometheusUrl }) {
    this.grafana = new GrafanaAnnotations(grafanaUrl, apiToken);
    this.prometheus = getPrometheusClient(prometheusUrl);
  }

  async analyze({ baselineVersion, canaryVersion, duration = 300, threshold = 0.05 }) {
    const queries = {
      errorRate: {
        baseline: `rate(http_requests_total{version="${baselineVersion}",status=~"5.."}[5m])
                    / rate(http_requests_total{version="${baselineVersion}"}[5m])`,
        canary: `rate(http_requests_total{version="${canaryVersion}",status=~"5.."}[5m])
                 / rate(http_requests_total{version="${canaryVersion}"}[5m])`,
      },
      latencyP99: {
        baseline: `histogram_quantile(0.99, rate(http_request_duration_seconds_bucket{version="${baselineVersion}"}[5m]))`,
        canary: `histogram_quantile(0.99, rate(http_request_duration_seconds_bucket{version="${canaryVersion}"}[5m]))`,
      },
      latencyP50: {
        baseline: `histogram_quantile(0.50, rate(http_request_duration_seconds_bucket{version="${baselineVersion}"}[5m]))`,
        canary: `histogram_quantile(0.50, rate(http_request_duration_seconds_bucket{version="${canaryVersion}"}[5m]))`,
      },
    };

    const results = {};
    let passed = true;

    for (const [metric, pair] of Object.entries(queries)) {
      const baselineVal = await this.prometheus.query(pair.baseline);
      const canaryVal = await this.prometheus.query(pair.canary);
      const diff = Math.abs(canaryVal - baselineVal) / baselineVal;

      results[metric] = {
        baseline: baselineVal,
        canary: canaryVal,
        delta: diff,
        pass: diff <= threshold,
      };

      if (diff > threshold) passed = false;
    }

    await this.grafana.markDeployment({
      version: canaryVersion,
      environment: 'canary',
      tags: [passed ? 'canary-passed' : 'canary-failed'],
      text: `Canary ${canaryVersion}: ${passed ? 'PASSED' : 'FAILED'}`,
    });

    return { passed, results, threshold, duration };
  }
}
```

## RED Metrics Dashboard

```json
{
  "dashboard": {
    "title": "Node.js App — RED Metrics",
    "uid": "nodejs-red-metrics",
    "tags": ["nodejs", "red", "production"],
    "timezone": "browser",
    "refresh": "30s",
    "panels": [
      {
        "title": "Request Rate (req/s)",
        "type": "timeseries",
        "gridPos": { "h": 8, "w": 12, "x": 0, "y": 0 },
        "targets": [{
          "expr": "sum(rate(http_requests_total[5m])) by (method)",
          "legendFormat": "{{method}}"
        }],
        "fieldConfig": {
          "defaults": {
            "unit": "reqps",
            "custom": { "lineWidth": 2 }
          }
        }
      },
      {
        "title": "Error Rate (%)",
        "type": "timeseries",
        "gridPos": { "h": 8, "w": 12, "x": 12, "y": 0 },
        "targets": [{
          "expr": "sum(rate(http_requests_total{status=~\"5..\"}[5m])) / sum(rate(http_requests_total[5m])) * 100",
          "legendFormat": "Error %"
        }],
        "fieldConfig": {
          "defaults": {
            "unit": "percent",
            "thresholds": {
              "mode": "absolute",
              "steps": [
                { "value": null, "color": "green" },
                { "value": 1, "color": "yellow" },
                { "value": 5, "color": "red" }
              ]
            },
            "custom": { "lineWidth": 2 }
          }
        }
      },
      {
        "title": "Duration — P50 / P95 / P99",
        "type": "timeseries",
        "gridPos": { "h": 8, "w": 12, "x": 0, "y": 8 },
        "targets": [
          {
            "expr": "histogram_quantile(0.50, sum(rate(http_request_duration_seconds_bucket[5m])) by (le))",
            "legendFormat": "p50"
          },
          {
            "expr": "histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket[5m])) by (le))",
            "legendFormat": "p95"
          },
          {
            "expr": "histogram_quantile(0.99, sum(rate(http_request_duration_seconds_bucket[5m])) by (le))",
            "legendFormat": "p99"
          }
        ],
        "fieldConfig": {
          "defaults": {
            "unit": "s",
            "custom": { "lineWidth": 2 }
          }
        }
      },
      {
        "title": "Request Latency Heatmap",
        "type": "heatmap",
        "gridPos": { "h": 8, "w": 12, "x": 12, "y": 8 },
        "targets": [{
          "expr": "sum(increase(http_request_duration_seconds_bucket[1m])) by (le)",
          "legendFormat": "{{le}}",
          "format": "heatmap"
        }],
        "options": {
          "calculate": true,
          "calculation": { "xBuckets": { "mode": "count", "value": 30 } }
        }
      },
      {
        "title": "Slowest Endpoints",
        "type": "table",
        "gridPos": { "h": 8, "w": 12, "x": 0, "y": 16 },
        "targets": [{
          "expr": "topk(10, histogram_quantile(0.99, sum(rate(http_request_duration_seconds_bucket[5m])) by (le, path)))",
          "legendFormat": "{{path}}",
          "instant": true,
          "format": "table"
        }]
      },
      {
        "title": "Status Code Distribution",
        "type": "piechart",
        "gridPos": { "h": 8, "w": 12, "x": 12, "y": 16 },
        "targets": [{
          "expr": "sum(increase(http_requests_total[1h])) by (status)",
          "legendFormat": "{{status}}",
          "instant": true
        }]
      }
    ],
    "time": { "from": "now-6h", "to": "now" },
    "templating": {
      "list": [
        {
          "name": "instance",
          "type": "query",
          "datasource": "Prometheus",
          "query": "label_values(http_requests_total, instance)",
          "multi": true,
          "includeAll": true
        }
      ]
    }
  }
}
```

## USE Metrics Dashboard

```json
{
  "dashboard": {
    "title": "Infrastructure — USE Metrics",
    "uid": "infra-use-metrics",
    "panels": [
      {
        "title": "CPU Utilization (%)",
        "type": "gauge",
        "gridPos": { "h": 6, "w": 6, "x": 0, "y": 0 },
        "targets": [{
          "expr": "100 - (avg(rate(node_cpu_seconds_total{mode=\"idle\"}[5m])) * 100)",
          "instant": true
        }],
        "fieldConfig": {
          "defaults": {
            "unit": "percent",
            "min": 0, "max": 100,
            "thresholds": {
              "steps": [
                { "value": null, "color": "green" },
                { "value": 70, "color": "yellow" },
                { "value": 90, "color": "red" }
              ]
            }
          }
        }
      },
      {
        "title": "Memory Utilization (%)",
        "type": "gauge",
        "gridPos": { "h": 6, "w": 6, "x": 6, "y": 0 },
        "targets": [{
          "expr": "(1 - node_memory_MemAvailable_bytes / node_memory_MemTotal_bytes) * 100",
          "instant": true
        }],
        "fieldConfig": {
          "defaults": {
            "unit": "percent",
            "min": 0, "max": 100,
            "thresholds": {
              "steps": [
                { "value": null, "color": "green" },
                { "value": 80, "color": "yellow" },
                { "value": 95, "color": "red" }
              ]
            }
          }
        }
      },
      {
        "title": "Disk I/O Saturation",
        "type": "timeseries",
        "gridPos": { "h": 6, "w": 6, "x": 12, "y": 0 },
        "targets": [
          { "expr": "rate(node_disk_io_time_seconds_total[5m])", "legendFormat": "{{device}}" },
          { "expr": "rate(node_disk_io_time_weighted_seconds_total[5m])", "legendFormat": "{{device}} weighted" }
        ],
        "fieldConfig": { "defaults": { "unit": "percentunit" } }
      },
      {
        "title": "Network Saturation",
        "type": "timeseries",
        "gridPos": { "h": 6, "w": 6, "x": 18, "y": 0 },
        "targets": [
          { "expr": "rate(node_network_receive_bytes_total[5m]) * 8", "legendFormat": "{{device}} rx" },
          { "expr": "rate(node_network_transmit_bytes_total[5m]) * 8", "legendFormat": "{{device}} tx" }
        ],
        "fieldConfig": { "defaults": { "unit": "bps" } }
      },
      {
        "title": "Node.js Heap Usage",
        "type": "timeseries",
        "gridPos": { "h": 8, "w": 12, "x": 0, "y": 6 },
        "targets": [
          { "expr": "nodejs_heap_size_used_bytes", "legendFormat": "Used" },
          { "expr": "nodejs_heap_size_total_bytes", "legendFormat": "Total" },
          { "expr": "nodejs_external_memory_bytes", "legendFormat": "External" }
        ],
        "fieldConfig": { "defaults": { "unit": "bytes" } }
      },
      {
        "title": "Event Loop Lag",
        "type": "timeseries",
        "gridPos": { "h": 8, "w": 12, "x": 12, "y": 6 },
        "targets": [
          { "expr": "nodejs_eventloop_lag_p50_seconds", "legendFormat": "p50" },
          { "expr": "nodejs_eventloop_lag_p99_seconds", "legendFormat": "p99" }
        ],
        "fieldConfig": { "defaults": { "unit": "s" } }
      }
    ],
    "time": { "from": "now-6h", "to": "now" }
  }
}
```

## Infrastructure Monitoring Components

### docker-compose.infrastructure.yml

```yaml
services:
  prometheus:
    image: prom/prometheus:v2.49.1
    ports:
      - "9090:9090"
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
      - ./alert-rules.yml:/etc/prometheus/alert-rules.yml
      - prometheus-data:/prometheus

  node-exporter:
    image: prom/node-exporter:v1.7.0
    ports:
      - "9100:9100"
    volumes:
      - /proc:/host/proc:ro
      - /sys:/host/sys:ro
      - /:/rootfs:ro
    command:
      - '--path.procfs=/host/proc'
      - '--path.sysfs=/host/sys'

  cadvisor:
    image: gcr.io/cadvisor/cadvisor:v0.49.1
    ports:
      - "8080:8080"
    volumes:
      - /:/rootfs:ro
      - /var/run:/var/run:ro
      - /sys:/sys:ro
      - /var/lib/docker:/var/lib/docker:ro

  kube-state-metrics:
    image: registry.k8s.io/kube-state-metrics/kube-state-metrics:v2.10.0
    ports:
      - "8081:8080"

  alertmanager:
    image: prom/alertmanager:v0.27.0
    ports:
      - "9093:9093"
    volumes:
      - ./alertmanager.yml:/etc/alertmanager/alertmanager.yml

  grafana:
    image: grafana/grafana:10.3.1
    ports:
      - "3000:3000"
    environment:
      GF_SECURITY_ADMIN_PASSWORD: ${GRAFANA_ADMIN_PASSWORD:-admin}
    volumes:
      - grafana-data:/var/lib/grafana

volumes:
  prometheus-data:
  grafana-data:
```

### prometheus.yml

```yaml
global:
  scrape_interval: 15s
  evaluation_interval: 15s

rule_files:
  - alert-rules.yml

alerting:
  alertmanagers:
    - static_configs:
        - targets: ['alertmanager:9093']

scrape_configs:
  - job_name: 'node-app'
    metrics_path: /metrics
    static_configs:
      - targets: ['host.docker.internal:3000']
        labels:
          app: 'node-app'
          env: 'production'

  - job_name: 'node-exporter'
    static_configs:
      - targets: ['node-exporter:9100']

  - job_name: 'cadvisor'
    static_configs:
      - targets: ['cadvisor:8080']

  - job_name: 'kube-state-metrics'
    static_configs:
      - targets: ['kube-state-metrics:8080']
```

## Alert Rules

```yaml
# alert-rules.yml
groups:
  - name: node-app-alerts
    rules:
      - alert: HighErrorRate
        expr: >
          sum(rate(http_requests_total{status=~"5.."}[5m]))
          / sum(rate(http_requests_total[5m])) > 0.05
        for: 5m
        labels:
          severity: critical
          team: backend
        annotations:
          summary: "High error rate: {{ $value | humanizePercentage }}"
          runbook: "https://wiki.internal/runbooks/high-error-rate"

      - alert: HighLatencyP99
        expr: >
          histogram_quantile(0.99, sum(rate(http_request_duration_seconds_bucket[5m])) by (le)) > 2
        for: 10m
        labels:
          severity: warning
        annotations:
          summary: "P99 latency above 2s: {{ $value }}s"

      - alert: HighMemoryUsage
        expr: >
          (1 - node_memory_MemAvailable_bytes / node_memory_MemTotal_bytes) * 100 > 90
        for: 5m
        labels:
          severity: critical
        annotations:
          summary: "Memory usage above 90%: {{ $value }}%"

      - alert: HighCPUUsage
        expr: >
          100 - (avg(rate(node_cpu_seconds_total{mode="idle"}[5m])) * 100) > 85
        for: 10m
        labels:
          severity: warning
        annotations:
          summary: "CPU usage above 85%: {{ $value }}%"

      - alert: EventLoopLagHigh
        expr: nodejs_eventloop_lag_p99_seconds > 0.5
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "Event loop P99 lag above 500ms"

      - alert: PodCrashLooping
        expr: increase(kube_pod_container_status_restarts_total[1h]) > 5
        for: 10m
        labels:
          severity: critical
        annotations:
          summary: "Pod {{ $labels.pod }} crash-looping"

      - alert: DeploymentFailed
        expr: >
          kube_deployment_status_replicas_unavailable > 0
          and kube_deployment_status_replicas_available < kube_deployment_spec_replicas
        for: 15m
        labels:
          severity: critical
        annotations:
          summary: "Deployment {{ $labels.deployment }} has unavailable replicas"
```

## Alertmanager Configuration

```yaml
# alertmanager.yml
global:
  resolve_timeout: 5m
  slack_api_url: ${SLACK_WEBHOOK_URL}

route:
  receiver: 'default'
  group_by: ['alertname', 'severity']
  group_wait: 30s
  group_interval: 5m
  repeat_interval: 4h
  routes:
    - match:
        severity: critical
      receiver: 'pagerduty-critical'
      continue: true
    - match:
        severity: critical
      receiver: 'slack-critical'
    - match:
        severity: warning
      receiver: 'slack-warnings'

receivers:
  - name: 'default'
    slack_configs:
      - channel: '#alerts'
        title: '{{ .GroupLabels.alertname }}'
        text: '{{ range .Alerts }}{{ .Annotations.summary }}{{ end }}'

  - name: 'slack-critical'
    slack_configs:
      - channel: '#alerts-critical'
        title: 'CRITICAL: {{ .GroupLabels.alertname }}'
        color: 'danger'

  - name: 'slack-warnings'
    slack_configs:
      - channel: '#alerts-warnings'
        title: 'WARNING: {{ .GroupLabels.alertname }}'
        color: 'warning'

  - name: 'pagerduty-critical'
    pagerduty_configs:
      - service_key: ${PAGERDUTY_SERVICE_KEY}
        severity: 'critical'
        description: '{{ .GroupLabels.alertname }}'

inhibit_rules:
  - source_match:
      severity: 'critical'
    target_match:
      severity: 'warning'
    equal: ['alertname']
```

## SLI / SLO / SLA Monitoring

```javascript
// src/slo/sli-monitor.js
import { Counter, Histogram } from 'prom-client';

// SLI: Service Level Indicators
export const sliAvailability = new Counter({
  name: 'sli_availability_total',
  help: 'Total requests for availability SLI',
  labelNames: ['result'], // 'success' or 'failure'
});

export const sliLatency = new Histogram({
  name: 'sli_latency_seconds',
  help: 'Request latency for latency SLI',
  labelNames: ['endpoint'],
  buckets: [0.1, 0.25, 0.5, 1, 2.5, 5],
});

// SLO: 99.9% availability over 30 days
// SLO: 99th percentile latency under 500ms
export const SLO_DEFINITIONS = {
  availability: {
    target: 0.999,  // 99.9%
    window: '30d',
    description: '99.9% of requests return non-5xx responses',
  },
  latency: {
    target: 0.99,   // 99% of requests
    threshold: 0.5, // under 500ms
    window: '30d',
    description: '99% of requests complete within 500ms',
  },
  errorBudget: {
    // If SLO is 99.9%, error budget = 0.1% = ~43 min/month
    remaining: (currentErrors, totalRequests) => {
      const budget = totalRequests * 0.001;
      return Math.max(0, budget - currentErrors);
    },
  },
};

// Middleware to record SLI
export function sliMiddleware(req, res, next) {
  const start = process.hrtime.bigint();

  res.on('finish', () => {
    const success = res.statusCode < 500;
    sliAvailability.inc({ result: success ? 'success' : 'failure' });

    const duration = Number(process.hrtime.bigint() - start) / 1e9;
    sliLatency.observe({ endpoint: req.route?.path || req.path }, duration);
  });

  next();
}
```

### SLO Burn Rate Alerts

```yaml
# Add to alert-rules.yml
groups:
  - name: slo-alerts
    rules:
      - alert: SLOBurnRateHigh
        expr: >
          (
            sum(rate(http_requests_total{status=~"5.."}[1h]))
            / sum(rate(http_requests_total[1h]))
          ) > (14.4 * 0.001)
        for: 2m
        labels:
          severity: critical
          slo: availability
        annotations:
          summary: "1h burn rate consuming error budget 14.4x faster than sustainable"
          description: "At this rate the 30-day SLO will be exhausted in 2 days"

      - alert: SLOBurnRateModerate
        expr: >
          (
            sum(rate(http_requests_total{status=~"5.."}[6h]))
            / sum(rate(http_requests_total[6h]))
          ) > (6 * 0.001)
        for: 15m
        labels:
          severity: warning
          slo: availability
        annotations:
          summary: "6h burn rate at 6x sustainable level"
```

## Synthetic Monitoring

```javascript
// src/synthetic/checks.js
import { chromium } from 'playwright';

export class SyntheticMonitor {
  constructor(grafanaUrl, apiKey) {
    this.grafanaUrl = grafanaUrl;
    this.apiKey = apiKey;
  }

  async checkHomepage() {
    const browser = await chromium.launch();
    const page = await browser.newPage();
    const start = Date.now();

    try {
      await page.goto('https://app.example.com', { waitUntil: 'networkidle' });
      const title = await page.title();
      const loadTime = Date.now() - start;

      // Record metric
      await this.recordMetric('synthetic_homepage_load_ms', loadTime, {
        status: loadTime < 3000 ? 'pass' : 'fail',
      });

      return { success: true, loadTime, title };
    } catch (err) {
      await this.recordMetric('synthetic_homepage_load_ms', -1, { status: 'error' });
      throw err;
    } finally {
      await browser.close();
    }
  }

  async checkAPIHealth() {
    const start = Date.now();
    const res = await fetch('https://api.example.com/health');
    const duration = Date.now() - start;

    return {
      status: res.status,
      duration,
      healthy: res.status === 200 && duration < 500,
    };
  }

  async recordMetric(name, value, labels) {
    // Push to Prometheus Pushgateway or Grafana Cloud Metrics
    const metricData = `${name}{${Object.entries(labels).map(([k, v]) => `${k}="${v}"`).join(',')}} ${value}`;
    // Implementation depends on your metrics backend
  }
}
```

## On-Call Runbook Automation

```yaml
# runbook-templates/runbook-high-error-rate.md
name: High Error Rate Response
triggers:
  - alert: HighErrorRate
steps:
  - name: Assess Impact
    actions:
      - query: |
          sum(rate(http_requests_total{status=~"5.."}[5m])) by (path)
    note: "Identify which endpoints are failing"

  - name: Check Recent Deployments
    actions:
      - command: kubectl rollout history deployment/node-app
      - query: |
          grafana_annotations{tags=~".*deployment.*"}

  - name: Rollback if Needed
    condition: "Error rate started after recent deployment"
    actions:
      - command: kubectl rollout undo deployment/node-app
      - command: kubectl rollout status deployment/node-app --timeout=300s

  - name: Escalate
    condition: "Rollback does not resolve within 10 minutes"
    actions:
      - pagerduty:
          severity: critical
          assign: backend-oncall
```

```javascript
// src/runbook/automator.js
export class RunbookAutomator {
  constructor(k8sClient, grafanaClient, pagerdutyClient) {
    this.k8s = k8sClient;
    this.grafana = grafanaClient;
    this.pagerduty = pagerdutyClient;
  }

  async executeRunbook(alertName, context) {
    switch (alertName) {
      case 'HighErrorRate':
        return this.handleHighErrorRate(context);
      case 'HighMemoryUsage':
        return this.handleHighMemory(context);
      case 'PodCrashLooping':
        return this.handleCrashLoop(context);
      default:
        return { action: 'escalate', reason: 'No automated runbook' };
    }
  }

  async handleHighErrorRate(context) {
    // Step 1: Check if triggered by deployment
    const recentDeployments = await this.grafana.getDeploymentAnnotations(
      'production',
      new Date(Date.now() - 3600000),
      new Date(),
    );

    if (recentDeployments.length > 0) {
      // Auto-rollback
      await this.k8s.rollback('node-app', 'production');
      return { action: 'rollback', deployment: recentDeployments[0] };
    }

    // Step 2: Escalate
    await this.pagerduty.createIncident({
      summary: 'High error rate — no recent deployment to rollback',
      severity: 'critical',
    });

    return { action: 'escalated' };
  }

  async handleHighMemory(context) {
    // Auto-scale up
    await this.k8s.scale('node-app', 'production', {
      replicas: context.currentReplicas + 2,
    });

    return { action: 'scaled_up', newReplicas: context.currentReplicas + 2 };
  }

  async handleCrashLoop(context) {
    const podLogs = await this.k8s.getPodLogs(context.podName, { tail: 100 });
    const reason = this.analyzeLogs(podLogs);

    if (reason === 'OOMKilled') {
      await this.k8s.patchDeployment('node-app', {
        spec: { template: { spec: { containers: [{ resources: { limits: { memory: '2Gi' } } }] } } },
      });
      return { action: 'increased_memory_limit' };
    }

    return { action: 'escalated', logs: podLogs.slice(0, 20) };
  }
}
```

## Performance Baseline

```javascript
// src/baselines/performance-baseline.js
import { register } from 'prom-client';

export class PerformanceBaseline {
  constructor(prometheusUrl, lookbackDays = 7) {
    this.prometheus = prometheusUrl;
    this.lookback = lookbackDays;
  }

  async establish() {
    const now = Date.now();
    const from = now - this.lookback * 86400000;

    return {
      latency: {
        p50: await this.queryPercentile(0.50, from, now),
        p95: await this.queryPercentile(0.95, from, now),
        p99: await this.queryPercentile(0.99, from, now),
      },
      throughput: {
        avg_rps: await this.queryAvgRps(from, now),
        peak_rps: await this.queryPeakRps(from, now),
      },
      errorRate: {
        avg: await this.queryAvgErrorRate(from, now),
        peak: await this.queryPeakErrorRate(from, now),
      },
      saturation: {
        eventLoopLagP99: await this.queryMetric(
          'nodejs_eventloop_lag_p99_seconds',
          from, now, 'avg',
        ),
        heapUsagePercent: await this.queryMetric(
          'nodejs_heap_size_used_bytes / nodejs_heap_size_total_bytes',
          from, now, 'avg',
        ),
      },
    };
  }

  async queryPercentile(percentile, from, to) {
    const query = `histogram_quantile(${percentile}, sum(rate(http_request_duration_seconds_bucket[${this.lookback}d])) by (le))`;
    return this.promQuery(query, from, to);
  }

  async promQuery(query, from, to) {
    const params = new URLSearchParams({ query, start: from / 1000, end: to / 1000, step: '3600' });
    const res = await fetch(`${this.prometheus}/api/v1/query_range?${params}`);
    const data = await res.json();
    return data.data.result[0]?.values;
  }
}
```

## Best Practices Checklist

- [ ] Annotate every deployment in Grafana for visual correlation
- [ ] Automate canary analysis with error rate and latency comparisons
- [ ] Build separate dashboards for RED (service) and USE (infrastructure) metrics
- [ ] Configure node-exporter, cAdvisor, and kube-state-metrics for full stack visibility
- [ ] Define alerting rules with appropriate severity and `for` durations
- [ ] Route critical alerts to PagerDuty, warnings to Slack
- [ ] Establish SLIs (availability, latency) and SLOs with error budget tracking
- [ ] Run synthetic checks for user-facing paths at regular intervals
- [ ] Maintain runbooks and automate common remediation steps
- [ ] Establish performance baselines after each stable release and alert on deviations

## Cross-References

- See [APM and Metrics](./01-apm-metrics.md) for Prometheus client setup
- See [Distributed Tracing and Logging](./02-distributed-tracing-logging.md) for OpenTelemetry and Pino
- See [Kubernetes Patterns](../03-container-orchestration/01-kubernetes-patterns.md) for K8s monitoring
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for deployment pipeline integration
- See [Performance Optimization](../10-performance-optimization/01-performance-optimization.md) for profiling
