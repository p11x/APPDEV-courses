/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 03_Metrics_Types Purpose: Metrics collection type definitions Difficulty: intermediate UseCase: web, backend, enterprise Version: TS 5.0+ Compatibility: Node.js 18+, Prometheus Performance: low-overhead Security: N/A */

declare namespace MetricsTypes {
  interface Metric {
    name: string;
    help: string;
    type: MetricType;
    values: MetricValue[];
  }

  type MetricType = 'counter' | 'gauge' | 'histogram' | 'summary' | 'untyped';

  interface MetricValue {
    labels: Record<string, string>;
    value: number;
    timestamp?: number;
  }

  interface Counter extends Metric {
    type: 'counter';
  }

  interface Gauge extends Metric {
    type: 'gauge';
  }

  interface Histogram extends Metric {
    type: 'histogram';
    buckets: Bucket[];
    sum: number;
    count: number;
  }

  interface Bucket {
    cumulative: boolean;
    le: number;
    count: number;
  }

  interface Summary extends Metric {
    type: 'summary';
    quantiles: Quantile[];
    sum: number;
    count: number;
  }

  interface Quantile {
    quantile: number;
    value: number;
  }

  interface Collector {
    collect(): void;
    reset(): void;
  }

  interface CounterChild {
    inc(value?: number, labelValues?: Record<string, string>): void;
  }

  interface GaugeChild {
    inc(value?: number, labelValues?: Record<string, string>): void;
    dec(value?: number, labelValues?: Record<string, string>): void;
    set(value: number, labelValues?: Record<string, string>): void;
    setToCurrentTime(labelValues?: Record<string, string>): void;
    startTimer(labelValues?: Record<string, string>): () => number;
  }

  interface HistogramChild {
    observe(value: number, labelValues?: Record<string, string>): void;
    startTimer(labelValues?: Record<string, string>): () => number;
  }

  interface SummaryChild {
    observe(value: number, labelValues?: Record<string, string>): void;
  }

  interface Registry {
    registerMetric<T extends Metric>(
      metric: T
    ): T;
    registerCollector(
      collector: Collector
    ): void;
    clear(): void;
    getMetricsAsJSON(): Metric[];
    contentType: string;
    metrics(): string;
    aggregateByName(metrics: Metric[]): Record<string, Metric>;
  }

  interface HistogramConfiguration {
    name: string;
    help: string;
    labelNames?: string[];
    buckets?: number[];
    ageBuckets?: number;
    maxAgeSeconds?: number;
    ageBucketsMaxTimer?: number;
    suppressSameFirstLoad?: boolean;
    collect?: (instance: Histogram) => void;
  }

  interface SummaryConfiguration {
    name: string;
    help: string;
    labelNames?: string[];
    percentiles?: number[];
    ageBuckets?: number;
    maxAgeSeconds?: number;
    ageBucketsMaxTimer?: number;
    compressCount?: number;
    collect?: (instance: Summary) => void;
  }

  interface CounterConfiguration {
    name: string;
    help: string;
    labelNames?: string[];
    aggregator?: 'sum' | 'omit';
    collect?: (instance: Counter) => void;
  }

  interface GaugeConfiguration {
    name: string;
    help: string;
    labelNames?: string[];
    collect?: (instance: Gauge) => void;
  }

  interface Pushgateway {
    pushAdd(
      jobName: string,
      groupingKeys?: Record<string, string>,
      registry?: Registry
    ): Promise<void>;
    push(
      jobName: string,
      groupingKeys?: Record<string, string>,
      registry?: Registry
    ): Promise<void>;
    delete(
      jobName: string,
      groupingKeys?: Record<string, string>,
      registry?: Registry
    ): Promise<void>;
  }

  interface PushgatewayOptions {
    url?: string;
    timeout?: number;
    job?: string;
    groupingKey?: Record<string, string>;
  }

  interface SummaryConfiguration {
    name: string;
    help: string;
    labelNames?: string[];
    percentiles?: number[];
  }
}

declare const client: {
  Counter: new (
    config: MetricsTypes.HistogramConfiguration
  ) => MetricsTypes.Counter;
  Gauge: new (
    config: MetricsTypes.GaugeConfiguration
  ) => MetricsTypes.Gauge;
  Histogram: new (
    config: MetricsTypes.HistogramConfiguration
  ) => MetricsTypes.Histogram;
  Summary: new (
    config: MetricsTypes.SummaryConfiguration
  ) => MetricsTypes.Summary;
  Registry: new () => MetricsTypes.Registry;
  register: MetricsTypes.Registry;
  registerables: MetricsTypes.Metric[];
  collectDefaultMetrics: (registry?: Registry) => void;
  collectDBMetrics: (registry?: Registry) => void;
};

import { Registry, Counter, Gauge, Histogram, Summary, collectDefaultMetrics } from 'prom-client';

const registry = new Registry();

const httpRequestDuration = new Histogram({
  name: 'http_request_duration_seconds',
  help: 'Duration of HTTP requests in seconds',
  labelNames: ['method', 'route', 'code', 'host'],
  buckets: [0.001, 0.005, 0.01, 0.025, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5],
});

const requestsTotal = new Counter({
  name: 'http_requests_total',
  help: 'Total number of HTTP requests',
  labelNames: ['method', 'route', 'code'],
});

const activeConnections = new Gauge({
  name: 'http_active_connections',
  help: 'Number of active HTTP connections',
  labelNames: ['host'],
});

const responseSize = new Summary({
  name: 'http_response_size_bytes',
  help: 'Response size in bytes',
  percentiles: [0.5, 0.9, 0.95, 0.99],
});

collectDefaultMetrics({ register: registry });

describe('Metrics Types', () => {
  describe('Counter', () => {
    it('should increment counter', () => {
      requestsTotal.inc({ method: 'GET', route: '/api/users', code: '200' });
    });

    it('should increment with value', () => {
      requestsTotal.inc(5, { method: 'POST', route: '/api/posts', code: '201' });
    });
  });

  describe('Gauge', () => {
    it('should set gauge value', () => {
      activeConnections.set(10, { host: 'example.com' });
    });

    it('should increment gauge value', () => {
      activeConnections.inc({ host: 'example.com' });
    });

    it('should decrement gauge value', () => {
      activeConnections.dec({ host: 'example.com' });
    });

    it('should set to current time', () => {
      activeConnections.setToCurrentTime({ host: 'example.com' });
    });
  });

  describe('Histogram', () => {
    it('should observe value', () => {
      httpRequestDuration.observe(0.15, { method: 'GET', route: '/api/users', code: '200', host: 'example.com' });
    });

    it('should use start timer', () => {
      const end = httpRequestDuration.startTimer({ method: 'GET', route: '/api/users', code: '200', host: 'example.com' });
      
      const response = await fetch('/api/users');
      end();
    });
  });

  describe('Summary', () => {
    it('should observe value', () => {
      responseSize.observe(1024, { method: 'GET', route: '/api/users', code: '200', host: 'example.com' });
    });
  });

  describe('Registry', () => {
    it('should get metrics', async () => {
      const metrics = await registry.metrics();
      expect(metrics).toBeDefined();
    });

    it('should get metrics as JSON', () => {
      const metrics = registry.getMetricsAsJSON();
      expect(metrics).toBeDefined();
    });

    it('should get content type', () => {
      const contentType = registry.contentType;
      expect(contentType).toContain('prometheus');
    });
  });
});

console.log('\n=== Metrics Types Complete ===');
console.log('Next: 04_Tracing_Types.ts');