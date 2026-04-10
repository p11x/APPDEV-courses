/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 06_Prometheus_Integration Purpose: Prometheus integration for metrics collection Difficulty: intermediate UseCase: backend, enterprise Version: TS 5.0+ Compatibility: Node.js 16+, Prometheus Performance: lowoverhead Security: N/A */

declare namespace PrometheusIntegration {
  interface PrometheusConfig {
    defaultDuration?: number;
    defaultLabels?: Record<string, string>;
    durationOnlyLabels?: string[];
    httpDurationBuckets?: number[];
    gcDurationBuckets?: number[];
    eventLoopLagBuckets?: number[];
  }

  interface Prometheus {
    register: Registry;
    startTimer: (req: Request, options?: TimerOptions) => void;
    validateLabels: (labels: Record<string, string>) => void;
    contentType: (registry?: Registry) => string;
    buildDefaultMetrics: () => void;
    buildHistogramMetrics: (opts: HistogramOpts) => Histogram;
    httpRequestDuration: Histogram;
    httpRequestSize: Histogram;
    httpResponseSize: Histogram;
    numActiveRequests: Gauge;
    numCPUs: Gauge;
    durationInSec: Histogram;
    heapSizeBytes: Gauge;
    heapUsedBytes: Gauge;
    externalLabels: string[];
    prefix: string;
    disableDefaultMetrics: boolean;
    disableDurationMetric: boolean;
    cpuProfileDurationSec: number;
  }

  interface Request {}
  interface TimerOptions {}
  interface Gauge extends Metric {}
  interface Histogram extends Metric {}
  interface Metric {}
  interface HistogramOpts {}
  interface Registry {}

  interface collectDefaultMetrics {
    (options?: DefaultMetricsOptions): void;
  }

  interface DefaultMetricsOptions {
    register?: Registry;
    enabledHooks?: string[];
    prefix?: string;
    gcDurationBuckets?: number[];
  }

  interface pushgateway {
    push(
      job: string,
      groupKey: string | Record<string, string>,
      registry?: Registry
    ): Promise<void>;
    pushAdd(
      job: string,
      groupKey: string | Record<string, string>,
      registry?: Registry
    ): Promise<void>;
    deleteJob(job: string, groupKey?: string | Record<string, string>): Promise<void>;
  }
}

import client, { Registry, Counter, Gauge, Histogram } from 'prom-client';

const registry = new Registry();

const counter = new Counter({
  name: 'api_requests_total',
  help: 'Total API requests',
  labelNames: ['method', 'endpoint', 'status'],
  registers: [registry],
});

const gauge = new Gauge({
  name: 'server_active_connections',
  help: 'Active server connections',
  labelNames: ['server'],
  registers: [registry],
});

const histogram = new Histogram({
  name: 'request_duration_seconds',
  help: 'Request duration in seconds',
  labelNames: ['method', 'endpoint'],
  buckets: [0.001, 0.005, 0.01, 0.025, 0.05, 0.1, 0.25, 0.5, 1],
  registers: [registry],
});

describe('Prometheus Integration', () => {
  it('should increment counter', () => {
    counter.inc({ method: 'GET', endpoint: '/api/users', status: 200 });
  });

  it('should set gauge', () => {
    gauge.set({ server: 'main' }, 10);
  });

  it('should observe histogram', () => {
    histogram.observe({ method: 'GET', endpoint: '/api/users' }, 0.15);
  });

  it('should return metrics', async () => {
    const metrics = await registry.metrics();
    const contentType = registry.contentType;
    expect(metrics).toBeDefined();
  });
});

console.log('\n=== Prometheus Integration Complete ===');
console.log('Next: 07_Grafana_Integration.ts');