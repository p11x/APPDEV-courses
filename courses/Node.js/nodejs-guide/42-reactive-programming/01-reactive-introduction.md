# Reactive Programming in Node.js

## What You'll Learn

- Understanding reactive programming paradigms
- RxJS fundamentals for Node.js
- Observable patterns and operators
- Event-driven architecture with RxJS

---

## Layer 1: Academic Foundation

### What is Reactive Programming?

Reactive programming is a declarative programming paradigm where data flows as streams and changes propagate automatically to all observers.

**Core Concepts:**
- **Observable**: Data stream source
- **Observer**: Entity that subscribes to observables
- **Operator**: Functions that transform observables
- **Subscription**: Connection between observable and observer

### Comparison with Traditional Patterns

| Traditional | Reactive |
|------------|----------|
| Push-based | Pull-based |
| Synchronous | Asynchronous |
| Callback hell | Declarative operators |
| One-time values | Continuous streams |

---

## Layer 2: RxJS Node.js Implementation

### Basic Observables

```typescript
import { Observable, Subject, from, of } from 'rxjs';
import { map, filter, mergeMap, catchError } from 'rxjs/operators';

// Create observable from event
function fromEvent(emitter: EventEmitter, event: string): Observable<any> {
  return new Observable((subscriber) => {
    const handler = (data: any) => subscriber.next(data);
    emitter.on(event, handler);
    return () => emitter.off(event, handler);
  });
}

// HTTP request as observable
import { firstValueFrom } from 'rxjs';

async function fetchUser(id: string): Promise<User> {
  const response = await fetch(`/api/users/${id}`);
  return response.json();
}

const user$ = from(fetchUser('123'));
const user = await firstValueFrom(user$);
```

### Subject for Real-time Events

```typescript
import { Subject } from 'rxjs';

class EventBus {
  private message$ = new Subject<Message>();

  publish(message: Message): void {
    this.message$.next(message);
  }

  subscribe(): Observable<Message> {
    return this.message$.asObservable();
  }
}
```

### HTTP Request/Response Stream

```typescript
import { Subject, of } from 'rxjs';
import { mergeMap, retry, catchError } from 'rxjs/operators';

class RetryableHttpClient {
  private request$ = new Subject<Request>();

  constructor(private maxRetries = 3) {
    this.request$
      .pipe(
        mergeMap(
          (req) => this.execute(req),
          retry({ count: this.maxRetries, delay: 1000 }),
          catchError((error) => of({ error: error.message }))
        )
      )
      .subscribe();
  }

  private execute(req: Request): Observable<Response> {
    return new Observable((subscriber) => {
      fetch(req.url, req.options)
        .then((res) => subscriber.next(res))
        .catch((err) => subscriber.error(err))
        .finally(() => subscriber.complete());
    });
  }
}
```

---

## Layer 3: Error Handling

```typescript
import { Observable, throwError, timer } from 'rxjs';
import { catchError, retry, retryWhen, tap, delayWhen } from 'rxjs/operators';

function withRetryAndFallback<T>(
  source$: Observable<T>,
  fallback$: Observable<T>
): Observable<T> {
  return source$.pipe(
    retry({
      count: 3,
      delay: (errors) =>
        errors.pipe(
          tap(console.error),
          delayWhen((_, index) => timer(Math.pow(2, index) * 1000))
        ),
    }),
    catchError((error) => fallback$)
  );
}
```

---

## Layer 4: Backpressure Handling

```typescript
import { Observable, Subject, queue } from 'rxjs';
import { window, buffer, filter } from 'rxjs/operators';

class BackpressureProcessor {
  private input$ = new Subject<any>();
  private maxBuffer = 100;

  constructor() {
    this.input$
      .pipe(
        buffer(
          this.input$.pipe(
            filter(() => buffer.length >= this.maxBuffer)
          )
        )
      )
      .subscribe((batch) => this.processBatch(batch));
  }

  add(item: any): void {
    this.input$.next(item);
  }

  private processBatch(items: any[]): void {
    console.log(`Processing ${items.length} items`);
  }
}
```

---

## Layer 5: Testing RxJS

```typescript
import { TestScheduler } from 'rxjs/testing';

describe('EventBus', () => {
  let scheduler: TestScheduler;

  beforeEach(() => {
    scheduler = new TestScheduler((actual, expected) => {
      expect(actual).toEqual(expected);
    });
  });

  it('should publish events to subscribers', () => {
    scheduler.run(({ cold, expectObservable }) => {
      const bus = new EventBus();
      const source$ = cold('a-b-c|', { a: 'event1', b: 'event2', c: 'event3' });

      source$.subscribe((msg) => bus.publish({ type: msg }));

      expectObservable(bus.subscribe()).toBe('a-b-c|', {
        a: 'event1',
        b: 'event2',
        c: 'event3',
      });
    });
  });
});
```

---

## Next Steps

Continue to [RxJS Basics](./02-rxjs-basics.md)