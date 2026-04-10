/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 02_Logging_Types Purpose: Logging type definitions for structured logging Difficulty: intermediate UseCase: web, backend, enterprise Version: TS 5.0+ Compatibility: Node.js 16+, Browsers Performance: low-overhead Security: sanitization */

declare namespace LoggingTypes {
  type LogLevel = 'fatal' | 'error' | 'warn' | 'info' | 'debug' | 'trace' | 'silent';

  interface LogEntry {
    level: LogLevel;
    message: string;
    timestamp: string;
    service?: string;
    environment?: string;
    version?: string;
    release?: string;
    runtime?: string;
    pid?: number;
    hostname?: string;
    traceId?: string;
    spanId?: string;
    context?: Record<string, unknown>;
    metadata?: Record<string, unknown>;
    error?: ErrorSerialization;
  }

  interface ErrorSerialization {
    name: string;
    message: string;
    stack?: string;
    cause?: ErrorSerialization;
    code?: string;
    signal?: string;
  }

  interface Logger {
    fatal(message: string, context?: Record<string, unknown>): void;
    error(message: string, context?: Record<string, unknown>): void;
    warn(message: string, context?: Record<string, unknown>): void;
    info(message: string, context?: Record<string, unknown>): void;
    debug(message: string, context?: Record<string, unknown>): void;
    trace(message: string, context?: Record<string, unknown>): void;
    child(bindings: Record<string, unknown>): Logger;
    level: LogLevel;
    levels: LogLevel[];
    silent(): void;
  }

  interface LoggerOptions {
    name?: string;
    level?: LogLevel;
    levels?: LogLevel[];
    transport?: Transport;
    defaultMeta?: Record<string, unknown>;
    formatters?: Formatters;
    serializers?: Serializers;
    hooks?: Hooks;
    redact?: RedactOptions;
    base?: Record<string, unknown>;
    sync?: boolean;
  }

  interface Transport {
    write(data: string): void;
    close(): Promise<void>;
  }

  interface StreamingTransport extends Transport {
    write(data: string): void;
    flush(): Promise<void>;
    end(): Promise<void>;
  }

  interface Formatters {
    format?(entry: LogEntry): string;
    level?(level: LogLevel): string;
    time?(timestamp: Date): string;
    merge?: (entry: LogEntry, levels: LogLevel[]) => LogEntry;
  }

  interface Serializers {
    err?: (error: Error) => ErrorSerialization;
    req?: (request: IncomingMessage) => Record<string, unknown>;
    res?: (response: ServerResponse) => Record<string, unknown>;
  }

  interface Hooks {
    level?: (entry: LogEntry) => void;
    write?: (entry: LogEntry) => void;
    err?: (error: Error) => void;
  }

  interface RedactOptions {
    paths?: string[];
    censor?: (value: unknown, path: string) => unknown;
    ignore?: (value: unknown, path: string) => boolean;
    remove?: boolean;
  }

  interface createLogger {
    (options?: LoggerOptions): Logger;
    levels: Record<LogLevel, number>;
  }

  type Destination =
    | ConsoleDestination
    | FileDestination
    | StreamDestination
    | HTTPEndpointDestination
    | SyslogDestination
    | LambdaDestination
    | CloudWatchDestination
    | DataDogDestination
    | GenericDestination;

  interface ConsoleDestination {
    type: 'console';
    options?: {
      colorize?: boolean | ((entry: LogEntry) => boolean);
      translateTime?: string | boolean;
      ignore?: string[];
    };
  }

  interface FileDestination {
    type: 'file';
    destination: string;
    options?: {
      maxSize?: number;
      maxFiles?: number;
      mkdir?: boolean;
      compress?: boolean;
      timestampPattern?: string;
      eol?: string;
    };
  }

  interface StreamDestination {
    type: 'stream';
    stream: NodeJS.WritableStream;
    options?: Record<string, unknown>;
  }

  interface HTTPEndpointDestination {
    type: 'httpEndpoint';
    url: string;
    headers?: Record<string, string>;
    method?: string;
    maxConcurrentRequests?: number;
    timeout?: number;
    retries?: number;
    retryDelay?: number;
    batch?: boolean;
    batchTimeout?: number;
    batchLimit?: number;
  }

  interface SyslogDestination {
    type: 'syslog';
    address?: string;
    port?: number;
    protocol?: 'tcp' | 'udp';
    app?: string;
    facility?: string;
    localhost?: string;
    rfc?: 'rfc5424' | 'rfc3164';
    eol?: string;
  }

  interface LambdaDestination {
    type: 'lambda';
    functionName: string;
    payload?: Record<string, unknown>;
    timeout?: number;
  }

  interface CloudWatchDestination {
    type: 'cloudwatch';
    logGroupName: string;
    logStreamName?: string;
    awsRegion?: string;
    accessKeyId?: string;
    secretAccessKey?: string;
    batchSize?: number;
  }

  interface DataDogDestination {
    type: 'datadog';
    region?: 'us' | 'eu';
    apiKey?: string;
    service?: string;
    source?: string;
    ddsource?: string;
  }

  interface GenericDestination {
    type: string;
    [key: string]: unknown;
  }

  interface PinoConfig {
    name?: string;
    level?: LogLevel;
    levelVal?: number;
    useOnlyCrosss?: boolean;
    customLevels?: Record<string, number>;
    useCustomOnly?: boolean;
    customLevelsEnabled?: boolean;
    formatters?: {
      level?: (entry: LogEntry) => { level: number };
      bindings?: (bindings: Record<string, unknown>) => Record<string, unknown;
      log?: (entry: LogEntry) => Record<string, unknown>;
    };
    serializers?: Record<string, (value: unknown) => unknown>;
    hooks?: Record<string, (entry: LogEntry) => void>;
    transport?: string | string[] | TransportOptions;
    sinks?: Record<string, Destination>;
    levelKey?: string;
    levelStringKeys?: string;
    nameKey?: string;
    nestedKey?: string;
    base?: Record<string, unknown>;
    cronkat?: boolean;
    timestamp?: () => string;
    zlib?: Record<string, unknown>;
  }

  interface TransportOptions {
    target?: string;
    options?: Record<string, unknown>;
  }

  interface IncomingMessage {
    method?: string;
    url?: string;
    path?: string;
    headers?: Record<string, string | string[] | undefined>;
    query?: Record<string, string | string[]>;
    params?: Record<string, string>;
    ip?: string;
    ips?: string[];
    protocol?: string;
    secure?: boolean;
    hostname?: string;
    host?: string;
    subdomain?: string;
    methodOriginal?: string;
    route?: string;
  }

  interface ServerResponse {
    statusCode?: number;
    statusMessage?: string;
    headers?: Record<string, string | string[]>;
  }

  interface Stream {
    pipe<T extends NodeJS.WritableStream>(dest: T): T;
    unpipe(dest?: NodeJS.WritableStream): void;
    on(event: 'data', listener: (chunk: Buffer | string) => void): this;
    on(event: 'end', listener: () => void): this;
    on(event: 'error', listener: (err: Error) => void): this;
  }

  interface WritableStream {
    write(buffer: Buffer | string): boolean;
    destroy(err?: Error): this;
    on(event: 'drain', listener: () => void): this;
    on(event: 'error', listener: (err: Error) => void): this;
    on(event: 'close', listener: () => void): this;
  }

  type NodeJS_Stream = Stream;

  type IncomingMessage = IncomingMessage;
  type ServerResponse = ServerResponse;
}

declare const pino: LoggingTypes.createLogger;

import pino from 'pino';

const logger = pino({
  name: 'my-service',
  level: 'info',
  transport: {
    target: 'pino-pretty',
    options: {
      colorize: true,
    },
  },
});

const childLogger = logger.child({ module: 'api' });

describe('Logging Types', () => {
  describe('Basic Logging', () => {
    it('should log fatal message', () => {
      logger.fatal({ err: new Error('Critical error') }, 'Fatal error occurred');
    });

    it('should log error message', () => {
      logger.error({ err: new Error('Error occurred') }, 'An error happened');
    });

    it('should log warn message', () => {
      logger.warn('Warning: deprecated API used');
    });

    it('should log info message', () => {
      logger.info({ userId: '123' }, 'User logged in');
    });

    it('should log debug message', () => {
      logger.debug({ query: 'SELECT * FROM users' }, 'Executing query');
    });

    it('should log trace message', () => {
      logger.trace({ stack: new Error().stack }, 'Trace information');
    });
  });

  describe('Structured Logging', () => {
    it('should log with context', () => {
      logger.info({
        requestId: 'abc-123',
        userId: 'user-456',
        action: 'update',
        resource: 'profile',
      }, 'Operation completed');
    });
  });

  describe('Child Loggers', () => {
    it('should create child logger', () => {
      childLogger.info('Child logger message');
      expect(childLogger).toBeDefined();
    });
  });

  describe('Log Levels', () => {
    it('should set log level', () => {
      logger.level = 'warn';
      logger.info('This should not log');
      logger.warn('This should log');
    });

    it('should get log level', () => {
      const level = logger.level;
      expect(level).toBeDefined();
    });
  });

  describe('Error Logging', () => {
    it('should serialize error', () => {
      const error = new Error('Test error');
      error.stack = 'Error stack trace';
      logger.error({ err: error }, 'Error with stack');
    });
  });
});

console.log('\n=== Logging Types Complete ===');
console.log('Next: 03_Metrics_Types.ts');