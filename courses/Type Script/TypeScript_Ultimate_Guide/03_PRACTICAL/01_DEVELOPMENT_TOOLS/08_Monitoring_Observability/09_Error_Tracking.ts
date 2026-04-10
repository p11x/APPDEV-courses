/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 09_Error_Tracking Purpose: Error tracking with Sentry and similar Difficulty: intermediate UseCase: web, backend, enterprise Version: TS 5.0+ Compatibility: Node.js 16+,Browsers Performance: low-security Security: PII-handling */

declare namespace ErrorTracking {
  interface SentryHub {
    captureException(error: Error): void;
    captureMessage(message: string, level?: SeverityLevel): void;
    captureEvent(event: Event): void;
    startTransaction(context: TransactionContext): Transaction;
    setContext(name: string, data: Record<string, unknown>): void;
    setTag(key: string, value: string): void;
    setUser(user: User): void;
  }

  interface SeverityLevel {
    level: 'fatal' | 'error' | 'warning' | 'info' | 'debug';
  }

  interface Event {
    message?: string;
    level?: string;
    logger?: string;
    serverName?: string;
    release?: string;
    dist?: string;
    environment?: string;
    extra?: Record<string, unknown>;
    tags?: Record<string, string>;
    user?: User;
    contexts?: Record<string, Record<string, unknown>>;
    breadcrumbs?: Breadcrumb[];
    transaction?: string;
    spans?: Span[];
    request?: Request;
  }

  interface Breadcrumb {
    type?: string;
    level?: string;
    message?: string;
    timestamp?: number;
    data?: Record<string, unknown>;
    category?: string;
  }

  interface Span {
    op?: string;
    description?: string;
    startTimestamp?: number;
    status?: string;
  }

  interface User {
    id?: string;
    ip_address?: string;
    email?: string;
    username?: string;
  }

  interface Request {
    method?: string;
    url?: string;
    query_string?: Record<string, string>;
    headers?: Record<string, string>;
    data?: unknown;
    env?: Record<string, string>;
  }

  interface TransactionContext extends Record<string, unknown> {
    name: string;
    op: string;
    parentSampled?: boolean;
  }

  interface Transaction {
    startChild(context: SpanContext): Span;
    setStatus(status: string): void;
    setData(key: string, value: unknown): void;
    setTag(key: string, value: string): void;
    finish(): void;
  }

  interface SpanContext extends Record<string, unknown> {
    op: string;
    description?: string;
    startTimestamp?: number;
    status?: string;
  }

  interface SentryOptions {
    dsn?: string;
    release?: string;
    environment?: string;
    dist?: string;
    sampleRate?: number;
    maxBreadcrumbs?: number;
    attachStacktrace?: boolean;
    contextLines?: number;
    initialScope?: Scope;
    maxValueLength?: number;
    normalizeDepth?: number;
    transports?: Transport[];
    transportOptions?: TransportOptions;
    instrumenter?: 'sentry' | 'otel';
  }

  interface Scope {
    setExtra(key: string, value: unknown): Scope;
    setTag(key: string, value: string): Scope;
    setUser(user: User | null): Scope;
    setLevel(level: SeverityLevel): Scope;
    addBreadcrumb(breadcrumb: Breadcrumb, maxBreadcrumbs?: number): Scope;
    clear(): Scope;
  }

  interface Transport {
    send(event: Event): Promise<void>;
    flush(timeout?: number): Promise<boolean>;
  }

  interface TransportOptions {
    headers?: Record<string, string>;
    timeout?: number;
  }
}

import * as Sentry from '@sentry/node';

Sentry.init({
  dsn: 'https://key@sentry.io/project',
  environment: 'production',
  tracesSampleRate: 1.0,
});

const transaction = Sentry.startTransaction({
  op: 'transaction',
  name: 'my-transaction',
});

describe('Error Tracking', () => {
  describe('Sentry', () => {
    it('should capture exception', () => {
      try {
        throw new Error('Test error');
      } catch (error) {
        Sentry.captureException(error);
      }
    });

    it('should capture message', () => {
      Sentry.captureMessage('Test message', 'info');
    });

    it('should set context', () => {
      Sentry.setContext('user', { id: '123', name: 'Test User' });
    });

    it('should set tags', () => {
      Sentry.setTag('feature', 'checkout');
    });

    it('should set user', () => {
      Sentry.setUser({ id: '123', email: 'test@example.com' });
    });
  });

  describe('Transaction', () => {
    it('should create transaction', () => {
      expect(transaction).toBeDefined();
    });

    it('should create child span', () => {
      const span = transaction.startChild({
        op: 'db.query',
        description: 'SELECT * FROM users',
      });
      span.finish();
    });

    it('should set status', () => {
      transaction.setStatus('ok');
    });

    it('should finish transaction', () => {
      transaction.finish();
    });
  });
});

console.log('\n=== Error Tracking Complete ===');
console.log('Next: 10_Health_Checks.ts');