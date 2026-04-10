/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 04_Tracing_Types Purpose: Distributed tracing type definitions Difficulty: advanced UseCase: web, backend, microservices Version: TS 5.0+ Compatibility: Node.js 16+, OpenTelemetry Performance: low-overhead Security: N/A */

declare namespace TracingTypes {
  interface Span {
    name: string;
    kind: SpanKind;
    status: SpanStatus;
    parentId?: string;
    startTime: number;
    endTime?: number;
    attributes: Attributes;
    events: SpanEvent[];
    links: SpanLink[];
  }

  type SpanKind = 'internal' | 'server' | 'client' | 'producer' | 'consumer';

  interface SpanStatus {
    code: StatusCode;
    message?: string;
  }

  type StatusCode = 'OK' | 'ERROR' | 'UNSET';

  interface Attributes extends Record<string, AttributeValue> {}

  type AttributeValue = string | boolean | number | string[] | boolean[] | number[];

  interface SpanEvent {
    name: string;
    time: number;
    attributes?: Attributes;
  }

  interface SpanLink {
    traceId: string;
    spanId: string;
    attributes?: Attributes;
  }

  interface SpanContext {
    traceId: string;
    spanId: string;
    traceFlags?: number;
    traceState?: TraceState;
  }

  interface TraceState {
    get(key: string): string | undefined;
    set(key: string, value: string): TraceState;
    delete(key: string): TraceState;
    forEach(fn: (key: string, value: string) => void): void;
    toString(): string;
  }

  interface Trace {
    name: string;
    context: SpanContext;
    kind: SpanKind;
    startTime: number;
    endTime?: number;
    status: SpanStatus;
    attributes: Attributes;
    links: SpanLink[];
    events: SpanEvent[];
    span: Span;
  }

  interface Tracer {
    startSpan(name: string, options?: SpanOptions): Span;
    withSpan<T>(span: Span, fn: (span: Span) => T): T;
    bind<T>(span: Span | undefined, target: T): T;
  }

  interface SpanOptions {
    kind?: SpanKind;
    parent?: SpanContext | Span;
    links?: SpanLink[];
    startTime?: number;
    attributes?: Attributes;
  }

  interface SpanExporter {
    export(batch: Span[], result: (error?: Error) => void): void;
    shutdown(): Promise<void>;
  }

  interface BatchSpanProcessor {
    new (exporter: SpanExporter): BatchSpanProcessor;
  }

  interface SimpleSpanProcessor {
    new (exporter: SpanExporter): SimpleSpanProcessor;
  }

  interface NodeTracerConfig {
    serviceName: string;
    logger?: Logger;
    logLevel?: LogLevel;
    resource?: Resource;
    spanProcessor?: SpanProcessor;
    sampler?: Sampler;
    maxAttributesPerSpan?: number;
    maxEventsPerSpan?: number;
    maxLinks?: number;
  }

  interface SpanProcessor {
    onStart(span: Span): void;
    onEnd(span: Span): void;
    async shutdown(): Promise<void>;
  }

  interface BatchSpanProcessorOptions {
    maxQueueSize?: number;
    maxExportBatchSize?: number;
    exportTimeoutMillis?: number;
    maxTimeoutMillis?: number;
  }

  interface ParentBasedSampler {
    new (config: ParentBasedSamplerConfig): Sampler;
  }

  interface ParentBasedSamplerConfig {
    root?: Sampler;
    remoteParentSampled?: Sampler;
    remoteParentNotSampled?: Sampler;
    localParentSampled?: Sampler;
    localParentNotSampled?: Sampler;
  }

  interface Sampler {
    shouldSample(context: SamplingContext): SamplingResult;
    toString(): string;
  }

  interface SamplingContext {
    traceId: string;
    spanName: string;
    spanKind: SpanKind;
    attributes: Attributes;
    links: SpanLink[];
  }

  interface SamplingResult {
    decision: SamplingDecision;
    attributes?: Attributes;
  }

  type SamplingDecision = 'RECORD_AND_SAMPLE' | 'RECORD_ONLY' | 'NOT_RECORD';

  interface Resource {
    detect(config?: ResourceConfig): Promise<Resource>;
    merge(other: Resource): Resource;
    attributes: Attributes;
  }

  interface ResourceConfig {
    serviceName?: string;
    attributes?: Attributes;
  }

  interface Logger {
    debug(message: string, ...args: unknown[]): void;
    info(message: string, ...args: unknown[]): void;
    warn(message: string, ...args: unknown[]): void;
    error(message: string, ...args: unknown[]): void;
  }

  type LogLevel = 'DEBUG' | 'INFO' | 'WARN' | 'ERROR';

  interface Context {
    active(): Context;
    withValue(key: symbol, value: unknown, context: Context): Context;
    getValue(key: symbol): unknown;
  }

  interface EntryType {}

  interface TelemetrySDK {
    version: string;
  }
}

import trace, { context, SpanKind, SpanStatusCode } from '@opentelemetry/api';

describe('Tracing Types', () => {
  const tracer = trace.getTracer('my-service');

  describe('Basic Tracing', () => {
    it('should create span', () => {
      const span = tracer.startSpan('my-span');
      span.end();
    });
  });

  describe('Span Attributes', () => {
    it('should set attributes', () => {
      const span = tracer.startSpan('my-span');
      span.setAttribute('key', 'value');
      span.setAttributes({ key1: 'value1', key2: 'value2' });
      span.end();
    });
  });

  describe('Span Events', () => {
    it('should add events', () => {
      const span = tracer.startSpan('my-span');
      span.addEvent('event-name');
      span.addEvent('event-name', { 'event.attr': 'value' });
      span.end();
    });
  });

  describe('Nested Spans', () => {
    it('should create nested spans', () => {
      const parentSpan = tracer.startSpan('parent-span');
      const childSpan = tracer.startSpan('child-span', { parent: parentSpan });
      childSpan.end();
      parentSpan.end();
    });
  });

  describe('Span Status', () => {
    it('should set status', () => {
      const span = tracer.startSpan('my-span');
      span.setStatus({ code: SpanStatusCode.OK, message: 'Success' });
      span.end();
    });

    it('should set error status', () => {
      const span = tracer.startSpan('my-span');
      span.setStatus({ code: SpanStatusCode.ERROR, message: 'Error occurred' });
      span.end();
    });
  });
});

console.log('\n=== Tracing Types Complete ===');
console.log('Next: 05_OpenTelemetry.ts');