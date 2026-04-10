/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 05_OpenTelemetry Purpose: OpenTelemetry integration and configuration Difficulty: advanced UseCase: web, backend, microservices Version: TS 5.0+ Compatibility: Node.js 16+, OpenTelemetry Performance: low Security: N/A */

declare namespace OpenTelemetryTypes {
  interface SDKRegistration {
    enable(): void;
    disable(): void;
  }

  interface NodeSDK {
    new (options?: NodeSDKOptions): NodeSDK;
    start(): Promise<void>;
    shutdown(): Promise<void>;
    addSpanProcessor(spanProcessor: SpanProcessor): this;
    addInstrumentation(instrumentation: Instrumentation): this;
    addResourceDetector(detector: ResourceDetector): this;
  }

  interface NodeSDKOptions {
    serviceName: string;
    autoDetectResources?: boolean;
    resourceDetectors?: ResourceDetector[];
    sampler?: Sampler;
    spanProcessor?: SpanProcessor;
    instrumentations?: Instrumentation[];
    traceExporter?: SpanExporter;
    metricReader?: MetricReader;
    logRecordExporter?: LogRecordExporter;
    logLevel?: number;
  }

  interface SpanProcessor {
    onStart(span: Span): void;
    onEnd(span: Span): void;
    forceFlush(): Promise<void>;
    shutdown(): Promise<void>;
  }

  interface ConsoleSpanExporter extends SpanExporter {}

  interface OTLPTraceExporter extends SpanExporter {
    new (config?: OTLPExporterConfig): OTLPTraceExporter;
  }

  interface OTLPExporterConfig {
    url?: string;
    headers?: Record<string, string>;
    keepAlive?: boolean;
    timeout?: number;
  }

  interface BatchSpanProcessor {
    new (exporter: SpanExporter, options?: BatchSpanProcessorOptions): BatchSpanProcessor;
  }

  interface BatchSpanProcessorOptions {
    maxQueueSize?: number;
    maxExportBatchSize?: number;
    exportTimeoutMillis?: number;
    maxTimeoutMillis?: number;
    maxQueueSize?: number;
  }

  interface Instrumentation {
    new (options?: InstrumentationConfig): Instrumentation;
  }

  interface InstrumentationConfig {
    enabled?: boolean;
    method?: string;
  }

  interface HttpInstrumentation extends Instrumentation {
    new (config?: HttpInstrumentationConfig): HttpInstrumentation;
  }

  interface HttpInstrumentationConfig {
    enabled?: boolean;
    applyCustomAttributesOnSpan?: (span: Span, request: IncomingMessage, response: ServerResponse) => void;
    headersToCapture?: string[];
    ignoreIncomingRequest?: (request: IncomingMessage) => boolean;
    ignoreOutgoingRequest?: (request: unknown, options?: RequestOptions) => boolean;
    serverName?: string;
    requestHook?: (span: Span, request: IncomingMessage) => void;
    responseHook?: (span: Span, response: ServerResponse) => void;
  }

  interface MySQL2Instrumentation extends Instrumentation {
    new (config?: DatabaseInstrumentationConfig): MySQL2Instrumentation;
  }

  interface PGInstrumentation extends Instrumentation {
    new (config?: DatabaseInstrumentationConfig): PGInstrumentation;
  }

  interface DatabaseInstrumentationConfig {
    enabled?: boolean;
    enableDatabaseStatementSanitizer?: boolean;
  }

  interface ResourceDetector {
    detect(config?: ResourceDetectorConfig): Promise<Resource>;
  }

  interface ResourceDetectorConfig {
    timeout?: number;
  }

  interface Resource {
    merge(other: Resource): Resource;
    attributes: ResourceAttributes;
    schemaUrl?: string;
  }

  interface ResourceAttributes extends Record<string, AttributeValue> {}

  type AttributeValue = string | boolean | number;

  interface MetricReader {
    selectableMetric?: string;
    onInitializedReader: () => void;
    createMetricCollector(): Promise<MetricCollector>;
  }

  interface MetricCollector {
    collect(): Promise<ResourceMetrics>;
  }

  interface ResourceMetrics {
    resource: Resource;
    scopeMetrics: ScopeMetrics[];
  }

  interface ScopeMetrics {
    scope: InstrumentationScope;
    metrics: Metric[];
    schemaUrl?: string;
  }

  interface InstrumentationScope {
    name: string;
    version?: string;
    schemaUrl?: string;
  }

  interface Metric {
    name: string;
    description?: string;
    unit?: string;
    data: MetricData;
  }

  type MetricData = 
    | GaugeMetricData 
    | SumMetricData 
    | HistogramMetricData 
    | ExponentialHistogramMetricData;

  interface GaugeMetricData {
    dataPoints: NumberDataPoint[];
    isMonotonic: boolean;
  }

  interface SumMetricData {
    dataPoints: NumberDataPoint[];
    aggregationTemporality: AggregationTemporality;
    isMonotonic: boolean;
  }

  interface HistogramMetricData {
    dataPoints: HistogramDataPoint[];
    aggregationTemporality: AggregationTemporality;
  }

  interface ExponentialHistogramMetricData {
    dataPoints: ExponentialHistogramDataPoint[];
    aggregationTemporality: AggregationTemporality;
  }

  interface NumberDataPoint {
    attributes: Record<string, AttributeValue>;
    value: number;
    time: number;
  }

  interface HistogramDataPoint extends NumberDataPoint {
    count: number;
    sum: number;
    buckets: Bucket[];
  }

  interface Bucket {
    count: number;
    max?: number;
  }

  interface ExponentialHistogramDataPoint extends NumberDataPoint {
    count: number;
    sum: number;
    scale: number;
    zeroCount: number;
    positive: ExponentialBucket;
    negative: ExponentialBucket;
  }

  interface ExponentialBucket {
    offset: number;
    count: number;
  }

  type AggregationTemporality = 'CUMULATIVE' | 'DELTA';

  interface LogRecordExporter {
    export(logRecords: LogRecord[], result: (error?: Error) => void): void;
    shutdown(): Promise<void>;
  }

  interface LogRecord {
    attributes: Record<string, AttributeValue>;
    timeUnixNano: number;
    observedTimeUnixNano: number;
    severityNumber: SeverityNumber;
    severityText: string;
    body: Body;
    spanId?: string;
    traceId?: string;
  }

  type SeverityNumber = number;

  type Body = string;

  interface LoggerProvider {
    getLogger(name: string, version?: string, schemaUrl?: string): Logger;
  }

  interface Logger {
    logRecordBuilder(): LogRecordBuilder;
  }

  interface LogRecordBuilder {
    setAttributes(attributes: Record<string, AttributeValue>): this;
    setSeverityNumber(severityNumber: SeverityNumber): this;
    setSeverityText(severityText: string): this;
    setBody(body: Body): this;
    setSpanContext(spanContext: SpanContext): this;
    emit(): void;
  }

  interface SpanContext extends Serializable {}

  interface Serializable {}

  interface IncomingMessage {}
  interface ServerResponse {}
  interface RequestOptions {}
}

import { NodeSDK } from '@opentelemetry/sdk-node';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';

const sdk = new NodeSDK({
  serviceName: 'my-service',
  traceExporter: new OTLPTraceExporter({
    url: 'http://localhost:4318/v1/traces',
  }),
  instrumentations: [getNodeAutoInstrumentations()],
});

sdk.start();

describe('OpenTelemetry', () => {
  describe('Basic Configuration', () => {
    it('should initialize SDK', async () => {
      expect(sdk).toBeDefined();
    });
  });

  describe('Instrumentations', () => {
    it('should add HTTP instrumentation', () => {
      const instrumentation = new OpenTelemetryTypes.HttpInstrumentation();
      expect(instrumentation).toBeDefined();
    });
  });
});

console.log('\n=== OpenTelemetry Complete ===');
console.log('Next: 06_Prometheus_Integration.ts');