# Event-Driven Stream Architecture Patterns

## What You'll Learn

- How to bridge EventEmitter to Readable streams for unified event handling
- Implementing event sourcing with append-only stream storage
- Building CQRS with separate read and write streams
- Orchestrating sagas using stream-based coordination
- Creating a stream-based pub/sub system
- Implementing dead letter queue streams for failed events
- Replaying events from stream history to rebuild state
- Building an event-sourced inventory system with stream processing

## EventEmitter to Readable Stream Bridge

Convert EventEmitter events into a consumable Readable stream with backpressure and lifecycle management.

```js
// event-bridge.mjs
import { Readable } from 'node:stream';
import { EventEmitter } from 'node:events';

class EventEmitterStream extends Readable {
  #emitter;
  #eventName;
  #buffer = [];
  #listening = true;
  #maxBuffer;

  constructor(emitter, eventName, { maxBuffer = 1000 } = {}) {
    super({ objectMode: true, highWaterMark: Math.min(maxBuffer, 16) });
    this.#emitter = emitter;
    this.#eventName = eventName;
    this.#maxBuffer = maxBuffer;

    this.#emitter.on(eventName, this.#onEvent);
    this.#emitter.on('error', this.#onError);

    // Stop listening when stream ends
    this.on('close', () => {
      if (this.#listening) {
        this.#emitter.removeListener(eventName, this.#onEvent);
        this.#emitter.removeListener('error', this.#onError);
        this.#listening = false;
      }
    });
  }

  #onEvent = (data) => {
    if (this.#buffer.length >= this.#maxBuffer) {
      this.#buffer.shift(); // drop oldest
      this.emit('warning', {
        code: 'BUFFER_OVERFLOW',
        message: `Event buffer overflow, oldest event dropped`,
      });
    }

    if (this.#listening) {
      this.#buffer.push(data);
      this._read();
    }
  };

  #onError = (err) => {
    this.destroy(err);
  };

  _read() {
    while (this.#buffer.length > 0) {
      const item = this.#buffer.shift();
      if (!this.push(item)) return; // backpressure
    }
  }

  _destroy(err, callback) {
    this.#listening = false;
    this.#emitter.removeListener(this.#eventName, this.#onEvent);
    this.#emitter.removeListener('error', this.#onError);
    callback(err);
  }
}

// Usage
const sensorBus = new EventEmitter();

// Simulate sensor data
setInterval(() => {
  sensorBus.emit('reading', {
    sensorId: 'temp-01',
    value: 20 + Math.random() * 10,
    unit: 'celsius',
    ts: Date.now(),
  });
}, 100);

const sensorStream = new EventEmitterStream(sensorBus, 'reading', {
  maxBuffer: 500,
});

sensorStream.on('data', (reading) => {
  console.log(`${reading.sensorId}: ${reading.value.toFixed(1)}°${reading.unit}`);
});

sensorStream.on('warning', (w) => {
  console.warn('Stream warning:', w.message);
});
```

## Event Sourcing with Append-Only Stream Storage

Store domain events as an append-only log and replay them to reconstruct aggregate state.

```js
// event-store.mjs
import { createReadStream, createWriteStream, appendFile, readFile } from 'node:fs/promises';
import { Transform, Readable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createInterface } from 'node:readline';
import { randomUUID } from 'node:crypto';

class EventStore {
  #storagePath;
  #snapshots = new Map();

  constructor(storagePath) {
    this.#storagePath = storagePath;
  }

  async append(aggregateId, eventType, payload, metadata = {}) {
    const event = {
      id: randomUUID(),
      aggregateId,
      type: eventType,
      payload,
      metadata,
      timestamp: new Date().toISOString(),
      version: await this.#getNextVersion(aggregateId),
    };

    const line = JSON.stringify(event) + '\n';
    await appendFile(this.#storagePath, line, 'utf-8');

    return event;
  }

  async #getNextVersion(aggregateId) {
    let version = 0;
    for await (const event of this.readAll()) {
      if (event.aggregateId === aggregateId) version++;
    }
    return version + 1;
  }

  readAll() {
    const rl = createInterface({
      input: createReadStream(this.#storagePath, { encoding: 'utf-8' }),
      crlfDelay: Infinity,
    });

    const output = new Readable({ objectMode: true, read() {} });

    rl.on('line', (line) => {
      if (line.trim()) {
        try {
          output.push(JSON.parse(line));
        } catch {
          // skip malformed lines
        }
      }
    });

    rl.on('close', () => output.push(null));
    rl.on('error', (err) => output.destroy(err));

    return output;
  }

  readAggregate(aggregateId) {
    return new Transform({
      objectMode: true,
      transform(event, _encoding, callback) {
        if (event.aggregateId === aggregateId) {
          callback(null, event);
        } else {
          callback();
        }
      },
    });
  }

  async getSnapshot(aggregateId) {
    return this.#snapshots.get(aggregateId) ?? null;
  }

  async saveSnapshot(aggregateId, state, version) {
    this.#snapshots.set(aggregateId, { state, version, savedAt: new Date().toISOString() });
  }

  async replayAggregate(aggregateId, applyFn, initialState = {}) {
    const snapshot = await this.getSnapshot(aggregateId);
    let state = snapshot?.state ?? initialState;
    let fromVersion = snapshot?.version ?? 0;

    await pipeline(
      this.readAll(),
      this.readAggregate(aggregateId),
      async function* (source) {
        for await (const event of source) {
          if (event.version > fromVersion) {
            state = applyFn(state, event);
          }
        }
        yield state;
      }
    );

    return state;
  }
}

// Aggregate: apply events to build state
function applyOrderEvent(state, event) {
  switch (event.type) {
    case 'OrderCreated':
      return {
        id: event.aggregateId,
        items: event.payload.items,
        status: 'created',
        total: event.payload.total,
        createdAt: event.timestamp,
      };
    case 'OrderConfirmed':
      return { ...state, status: 'confirmed', confirmedAt: event.timestamp };
    case 'OrderShipped':
      return { ...state, status: 'shipped', trackingNo: event.payload.trackingNo, shippedAt: event.timestamp };
    case 'OrderCancelled':
      return { ...state, status: 'cancelled', reason: event.payload.reason, cancelledAt: event.timestamp };
    default:
      return state;
  }
}

// Usage
const store = new EventStore('./events.log');

await store.append('order-001', 'OrderCreated', { items: [{ sku: 'A', qty: 2 }], total: 59.98 });
await store.append('order-001', 'OrderConfirmed', {});
await store.append('order-001', 'OrderShipped', { trackingNo: 'TRK-12345' });

const orderState = await store.replayAggregate('order-001', applyOrderEvent);
console.log('Reconstructed order:', orderState);
```

## CQRS Pattern with Separate Read/Write Streams

Separate command (write) and query (read) paths using dedicated streams with eventual consistency.

```js
// cqrs-streams.mjs
import { Transform, Writable, Readable, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { EventEmitter } from 'node:events';

class CommandBus extends EventEmitter {
  #writeStore = [];

  dispatch(command) {
    this.emit('command', command);
    this.#writeStore.push({ ...command, dispatchedAt: Date.now() });
  }

  createCommandStream() {
    const self = this;
    return new Readable({
      objectMode: true,
      read() {
        // Push commands as they arrive
      },
      construct(cb) {
        self.on('command', (cmd) => this.push(cmd));
        cb();
      },
    });
  }
}

class QueryStore {
  #data = new Map();
  #changeLog = new PassThrough({ objectMode: true });

  applyProjection(event) {
    switch (event.type) {
      case 'InventoryAdded': {
        const current = this.#data.get(event.sku) ?? { sku: event.sku, quantity: 0 };
        current.quantity += event.quantity;
        this.#data.set(event.sku, current);
        this.#changeLog.write({ type: 'projection-updated', sku: event.sku, data: current });
        break;
      }
      case 'InventoryRemoved': {
        const current = this.#data.get(event.sku) ?? { sku: event.sku, quantity: 0 };
        current.quantity -= event.quantity;
        this.#data.set(event.sku, current);
        this.#changeLog.write({ type: 'projection-updated', sku: event.sku, data: current });
        break;
      }
    }
  }

  query(sku) {
    return this.#data.get(sku) ?? null;
  }

  queryAll() {
    return Array.from(this.#data.values());
  }

  get changeStream() {
    return this.#changeLog;
  }
}

class CQRSProcessor {
  #commandBus;
  #queryStore;
  #events = new PassThrough({ objectMode: true });

  constructor() {
    this.#commandBus = new CommandBus();
    this.#queryStore = new QueryStore();
  }

  // Command handler: validates and emits domain events
  #createCommandHandler() {
    return new Transform({
      objectMode: true,
      transform(command, _encoding, callback) {
        // Validation
        if (!command.type || !command.aggregateId) {
          return callback(new Error('Invalid command'));
        }

        // Convert command to domain event
        let event;
        switch (command.type) {
          case 'AddInventory':
            event = {
              type: 'InventoryAdded',
              sku: command.sku,
              quantity: command.quantity,
              commandId: command.id,
              timestamp: new Date().toISOString(),
            };
            break;
          case 'RemoveInventory':
            event = {
              type: 'InventoryRemoved',
              sku: command.sku,
              quantity: command.quantity,
              commandId: command.id,
              timestamp: new Date().toISOString(),
            };
            break;
          default:
            return callback(new Error(`Unknown command: ${command.type}`));
        }

        callback(null, event);
      },
    });
  }

  // Projection: updates read store from events
  #createProjector() {
    return new Writable({
      objectMode: true,
      write(event, _encoding, callback) {
        this.#queryStore.applyProjection(event);
        callback();
      }.bind(this),
    });
  }

  async start() {
    // Wire command -> event -> projection pipeline
    const commandStream = this.#commandBus.createCommandStream();

    await pipeline(
      commandStream,
      this.#createCommandHandler(),
      this.#events,
      this.#createProjector(),
      (err) => {
        if (err) console.error('CQRS pipeline error:', err);
      }
    );
  }

  get commandBus() { return this.#commandBus; }
  get queryStore() { return this.#queryStore; }
}

// Usage
const cqrs = new CQRSProcessor();
await cqrs.start();

cqrs.commandBus.dispatch({ id: 'cmd-1', type: 'AddInventory', sku: 'WIDGET-01', quantity: 100 });
cqrs.commandBus.dispatch({ id: 'cmd-2', type: 'RemoveInventory', sku: 'WIDGET-01', quantity: 25 });

// Query side (eventually consistent)
setTimeout(() => {
  console.log('Inventory:', cqrs.queryStore.query('WIDGET-01'));
  // { sku: 'WIDGET-01', quantity: 75 }
}, 100);
```

## Saga Pattern with Stream-Based Orchestration

Coordinate multi-step transactions across services using streams as the saga execution engine.

```js
// saga-stream.mjs
import { Transform, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { EventEmitter } from 'node:events';

class SagaOrchestrator extends EventEmitter {
  #steps;
  #compensations;
  #state;

  constructor(steps) {
    super();
    this.#steps = steps;
    this.#compensations = [];
    this.#state = { status: 'pending', currentStep: 0, data: {}, errors: [] };
  }

  createExecutionStream() {
    const steps = this.#steps;
    const compensations = this.#compensations;
    const state = this.#state;
    const emitter = this;

    return new Transform({
      objectMode: true,
      async transform(input, _encoding, callback) {
        state.data = { ...input };
        state.status = 'running';

        for (let i = 0; i < steps.length; i++) {
          const step = steps[i];
          state.currentStep = i;

          emitter.emit('step:start', { step: step.name, index: i });

          try {
            const result = await step.execute(state.data);
            state.data = { ...state.data, ...result };

            if (step.compensate) {
              compensations.unshift({ name: step.name, compensate: step.compensate });
            }

            emitter.emit('step:complete', { step: step.name, index: i, result });
          } catch (err) {
            state.status = 'compensating';
            state.errors.push({ step: step.name, error: err.message });

            emitter.emit('step:failed', { step: step.name, index: i, error: err });

            // Run compensations in reverse order
            for (const comp of compensations) {
              try {
                await comp.compensate(state.data);
                emitter.emit('compensation:complete', { step: comp.name });
              } catch (compErr) {
                emitter.emit('compensation:failed', {
                  step: comp.name,
                  error: compErr,
                });
              }
            }

            state.status = 'failed';
            emitter.emit('saga:failed', state);
            return callback(err);
          }
        }

        state.status = 'completed';
        state.currentStep = steps.length;
        emitter.emit('saga:complete', state);

        callback(null, state);
      },
    });
  }

  get state() {
    return { ...this.#state };
  }
}

// Define saga steps for order fulfillment
const orderFulfillmentSaga = new SagaOrchestrator([
  {
    name: 'validate-order',
    execute: async (data) => {
      if (!data.orderId || !data.items?.length) throw new Error('Invalid order');
      return { validatedAt: new Date().toISOString() };
    },
  },
  {
    name: 'reserve-inventory',
    execute: async (data) => {
      // Simulate inventory reservation
      return { reservationId: `res-${data.orderId}`, reservedAt: new Date().toISOString() };
    },
    compensate: async (data) => {
      console.log(`Releasing reservation: ${data.reservationId}`);
    },
  },
  {
    name: 'charge-payment',
    execute: async (data) => {
      // Simulate payment processing
      if (data.total > 10000) throw new Error('Payment limit exceeded');
      return { paymentId: `pay-${data.orderId}`, chargedAt: new Date().toISOString() };
    },
    compensate: async (data) => {
      console.log(`Refunding payment: ${data.paymentId}`);
    },
  },
  {
    name: 'create-shipment',
    execute: async (data) => {
      return { shipmentId: `ship-${data.orderId}`, createdAt: new Date().toISOString() };
    },
    compensate: async (data) => {
      console.log(`Cancelling shipment: ${data.shipmentId}`);
    },
  },
]);

orderFulfillmentSaga.on('step:complete', ({ step, result }) => {
  console.log(`[Saga] ${step} completed:`, Object.keys(result));
});

orderFulfillmentSaga.on('saga:complete', (state) => {
  console.log('[Saga] Order fulfilled:', state.data.orderId);
});

orderFulfillmentSaga.on('saga:failed', (state) => {
  console.error('[Saga] Failed:', state.errors);
});

// Execute saga
const executionStream = orderFulfillmentSaga.createExecutionStream();

executionStream.write({
  orderId: 'ORD-001',
  customerId: 'CUST-001',
  items: [{ sku: 'WIDGET', qty: 2, price: 29.99 }],
  total: 59.98,
});
```

## Stream-Based Pub/Sub Implementation

A lightweight publish/subscribe system backed by Node.js streams with topic filtering.

```js
// pubsub-stream.mjs
import { Transform, PassThrough } from 'node:stream';

class PubSub {
  #topics = new Map();

  createTopic(topicName) {
    if (this.#topics.has(topicName)) return this.#topics.get(topicName);

    const stream = new PassThrough({ objectMode: true });
    this.#topics.set(topicName, stream);
    return stream;
  }

  publish(topic, message) {
    const stream = this.#topics.get(topic);
    if (!stream) return false;

    return stream.write({
      topic,
      payload: message,
      publishedAt: Date.now(),
      id: crypto.randomUUID(),
    });
  }

  subscribe(topic, handler) {
    const source = this.#topics.get(topic);
    if (!source) throw new Error(`Topic "${topic}" does not exist`);

    const subscription = new Transform({
      objectMode: true,
      transform(msg, _encoding, callback) {
        try {
          handler(msg);
          callback(null, msg); // pass through for chaining
        } catch (err) {
          callback(err);
        }
      },
    });

    source.pipe(subscription);

    return {
      stream: subscription,
      unsubscribe: () => {
        source.unpipe(subscription);
        subscription.destroy();
      },
    };
  }

  // Pattern-based subscription (e.g., "orders.*", "orders.created")
  subscribePattern(pattern, handler) {
    const regex = new RegExp('^' + pattern.replace(/\*/g, '[^.]+') + '$');
    const combined = new PassThrough({ objectMode: true });

    const subscriptions = [];

    for (const [topic, stream] of this.#topics) {
      if (regex.test(topic)) {
        const sub = this.subscribe(topic, handler);
        subscriptions.push(sub);
      }
    }

    return {
      stream: combined,
      unsubscribe: () => {
        subscriptions.forEach((s) => s.unsubscribe());
      },
    };
  }

  listTopics() {
    return Array.from(this.#topics.keys());
  }
}

// Usage
const pubsub = new PubSub();

pubsub.createTopic('orders.created');
pubsub.createTopic('orders.shipped');
pubsub.createTopic('inventory.updated');

const orderSub = pubsub.subscribe('orders.created', (msg) => {
  console.log('New order:', msg.payload);
});

const inventorySub = pubsub.subscribe('inventory.updated', (msg) => {
  console.log('Inventory change:', msg.payload);
});

pubsub.publish('orders.created', { orderId: '123', total: 99.99 });
pubsub.publish('inventory.updated', { sku: 'WIDGET', delta: -1 });

// Cleanup
orderSub.unsubscribe();
inventorySub.unsubscribe();
```

## Dead Letter Queue Stream Pattern

Route failed messages to a dead letter stream for inspection, retry, or manual intervention.

```js
// dead-letter-stream.mjs
import { Transform, PassThrough, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { appendFile } from 'node:fs/promises';

class DeadLetterQueue {
  #dlq = new PassThrough({ objectMode: true });
  #deadLetters = [];
  #maxRetries;
  #retryDelay;

  constructor({ maxRetries = 3, retryDelay = 1000, persistencePath = './dead-letters.log' }) {
    this.#maxRetries = maxRetries;
    this.#retryDelay = retryDelay;
    this.#persistencePath = persistencePath;

    // Persist dead letters to disk
    this.#dlq.pipe(new Writable({
      objectMode: true,
      write: async (record, _encoding, callback) => {
        this.#deadLetters.push(record);
        try {
          await appendFile(this.#persistencePath, JSON.stringify(record) + '\n');
        } catch (err) {
          console.error('Failed to persist dead letter:', err);
        }
        callback();
      },
    }));
  }

  createProcessorStream(processFn) {
    const dlq = this.#dlq;
    const maxRetries = this.#maxRetries;
    const retryDelay = this.#retryDelay;

    return new Transform({
      objectMode: true,
      async transform(record, _encoding, callback) {
        let lastError;

        for (let attempt = 1; attempt <= maxRetries; attempt++) {
          try {
            const result = await processFn(record);
            callback(null, { ...record, result, attempts: attempt });
            return;
          } catch (err) {
            lastError = err;
            if (attempt < maxRetries) {
              await new Promise((r) => setTimeout(r, retryDelay * attempt));
            }
          }
        }

        // All retries exhausted - send to DLQ
        const deadLetter = {
          originalMessage: record,
          error: lastError.message,
          failedAt: new Date().toISOString(),
          attempts: maxRetries,
          stack: lastError.stack,
        };

        dlq.write(deadLetter);
        callback(); // don't propagate to downstream - message consumed
      },
    });
  }

  get deadLetterStream() {
    return this.#dlq;
  }

  getDeadLetters() {
    return [...this.#deadLetters];
  }

  async replayDeadLetter(index, processFn) {
    const record = this.#deadLetters[index];
    if (!record) throw new Error(`No dead letter at index ${index}`);

    try {
      const result = await processFn(record.originalMessage);
      this.#deadLetters.splice(index, 1);
      return { success: true, result };
    } catch (err) {
      return { success: false, error: err.message };
    }
  }
}

// Usage
const dlq = new DeadLetterQueue({
  maxRetries: 3,
  retryDelay: 500,
  persistencePath: './dead-letters.log',
});

// Main processing stream with DLQ
const orderProcessor = dlq.createProcessorStream(async (order) => {
  if (order.total > 9999) throw new Error('Amount exceeds limit');
  // Simulate processing
  return { processed: true, orderId: order.id };
});

// Monitor dead letters
dlq.deadLetterStream.on('data', (deadLetter) => {
  console.error('[DLQ] Message failed:', deadLetter.error);
});

// Feed messages
const source = new PassThrough({ objectMode: true });
source.pipe(orderProcessor);

source.write({ id: '1', total: 100 });   // succeeds
source.write({ id: '2', total: 99999 }); // fails -> DLQ
source.write({ id: '3', total: 50 });   // succeeds
source.end();

setTimeout(() => {
  console.log('Dead letters:', dlq.getDeadLetters().length);
}, 2000);
```

## Event Replay from Stream History

Replay historical events to rebuild projections, catch up lagging consumers, or debug issues.

```js
// event-replay.mjs
import { Readable, Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createReadStream, appendFile } from 'node:fs/promises';
import { createInterface } from 'node:readline';

class EventReplayEngine {
  #logPath;
  #projections = new Map();

  constructor(logPath) {
    this.#logPath = logPath;
  }

  createEventStream({ fromTimestamp, toTimestamp, eventTypes, aggregateId } = {}) {
    const rl = createInterface({
      input: createReadStream(this.#logPath, { encoding: 'utf-8' }),
      crlfDelay: Infinity,
    });

    const output = new Readable({ objectMode: true, read() {} });

    rl.on('line', (line) => {
      if (!line.trim()) return;

      try {
        const event = JSON.parse(line);

        // Apply filters
        if (aggregateId && event.aggregateId !== aggregateId) return;
        if (eventTypes && !eventTypes.includes(event.type)) return;
        if (fromTimestamp && new Date(event.timestamp) < new Date(fromTimestamp)) return;
        if (toTimestamp && new Date(event.timestamp) > new Date(toTimestamp)) return;

        output.push(event);
      } catch {
        // skip malformed
      }
    });

    rl.on('close', () => output.push(null));
    rl.on('error', (err) => output.destroy(err));

    return output;
  }

  registerProjection(name, initialState, applyFn) {
    this.#projections.set(name, { state: initialState, apply: applyFn });
  }

  async rebuildProjection(name, options = {}) {
    const projection = this.#projections.get(name);
    if (!projection) throw new Error(`Projection "${name}" not found`);

    projection.state = {}; // reset

    const eventStream = this.createEventStream(options);

    const applyStream = new Transform({
      objectMode: true,
      transform(event, _encoding, callback) {
        projection.state = projection.apply(projection.state, event);
        callback(null, event);
      },
    });

    const counter = new Writable({
      objectMode: true,
      write(_chunk, _encoding, callback) {
        this.count = (this.count ?? 0) + 1;
        callback();
      },
    });

    await pipeline(eventStream, applyStream, counter);

    console.log(`[Replay] ${name}: processed ${counter.count} events`);
    return projection.state;
  }

  getProjectionState(name) {
    return this.#projections.get(name)?.state ?? null;
  }
}

// Usage
const replay = new EventReplayEngine('./events.log');

// Register projection
replay.registerProjection('inventory', {}, (state, event) => {
  switch (event.type) {
    case 'InventoryAdded':
      return { ...state, [event.sku]: (state[event.sku] ?? 0) + event.quantity };
    case 'InventoryRemoved':
      return { ...state, [event.sku]: (state[event.sku] ?? 0) - event.quantity };
    default:
      return state;
  }
});

// Rebuild from full history
const inventory = await replay.rebuildProjection('inventory');
console.log('Inventory state:', inventory);

// Replay only recent events for a specific aggregate
const recentEvents = await replay.rebuildProjection('inventory', {
  fromTimestamp: '2026-01-01T00:00:00Z',
  aggregateId: 'warehouse-east',
});
```

## Real-World: Event-Sourced Inventory System

A complete inventory management system using event sourcing, CQRS, and stream processing.

```js
// inventory-system.mjs
import { Transform, PassThrough, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { appendFile, readFile } from 'node:fs/promises';
import { randomUUID } from 'node:crypto';

class InventorySystem {
  #eventLog = './inventory-events.log';
  #writeBus = new PassThrough({ objectMode: true });
  #readModel = new InventoryReadModel();
  #alertStream = new PassThrough({ objectMode: true });

  constructor() {
    this.#setupPipelines();
  }

  #setupPipelines() {
    // Write path: validate command -> emit event -> persist -> project
    const eventEmitter = new Transform({
      objectMode: true,
      transform(command, _encoding, callback) {
        const event = this.#commandToEvent(command);
        if (!event) return callback(new Error(`Unknown command: ${command.type}`));
        callback(null, event);
      }.bind(this),
    });

    const persister = new Transform({
      objectMode: true,
      async transform(event, _encoding, callback) {
        try {
          await appendFile(this.#eventLog, JSON.stringify(event) + '\n');
          callback(null, event);
        } catch (err) {
          callback(err);
        }
      },
    });

    const projector = new Transform({
      objectMode: true,
      transform(event, _encoding, callback) {
        this.#readModel.apply(event);

        // Emit alerts for low stock
        if (event.type === 'StockDecremented') {
          const item = this.#readModel.get(event.sku);
          if (item && item.quantity <= item.reorderPoint) {
            this.#alertStream.write({
              type: 'low-stock-alert',
              sku: event.sku,
              quantity: item.quantity,
              reorderPoint: item.reorderPoint,
              timestamp: new Date().toISOString(),
            });
          }
        }

        callback(null, event);
      }.bind(this),
    });

    pipeline(
      this.#writeBus,
      eventEmitter,
      persister,
      projector,
      new Writable({ objectMode: true, write(_c, _e, cb) { cb(); } }),
      (err) => { if (err) console.error('Pipeline error:', err); }
    );
  }

  #commandToEvent(command) {
    const base = {
      id: randomUUID(),
      aggregateId: command.sku,
      timestamp: new Date().toISOString(),
      correlationId: command.correlationId,
    };

    switch (command.type) {
      case 'RegisterItem':
        return { ...base, type: 'ItemRegistered', sku: command.sku, name: command.name, reorderPoint: command.reorderPoint, initialQuantity: command.quantity };
      case 'AddStock':
        return { ...base, type: 'StockIncremented', sku: command.sku, quantity: command.quantity, reason: command.reason ?? 'restock' };
      case 'RemoveStock':
        return { ...base, type: 'StockDecremented', sku: command.sku, quantity: command.quantity, reason: command.reason ?? 'sale', orderId: command.orderId };
      case 'AdjustStock':
        return { ...base, type: 'StockAdjusted', sku: command.sku, newQuantity: command.newQuantity, reason: command.reason };
      default:
        return null;
    }
  }

  dispatch(command) {
    this.#writeBus.write(command);
  }

  query(sku) {
    return this.#readModel.get(sku);
  }

  queryAll() {
    return this.#readModel.getAll();
  }

  get alerts() {
    return this.#alertStream;
  }

  async rebuild() {
    // Replay all events from log
    try {
      const content = await readFile(this.#eventLog, 'utf-8');
      const lines = content.trim().split('\n').filter(Boolean);

      this.#readModel = new InventoryReadModel();
      for (const line of lines) {
        const event = JSON.parse(line);
        this.#readModel.apply(event);
      }

      console.log(`[Inventory] Rebuilt from ${lines.length} events`);
    } catch (err) {
      if (err.code !== 'ENOENT') throw err;
    }
  }
}

class InventoryReadModel {
  #items = new Map();

  apply(event) {
    switch (event.type) {
      case 'ItemRegistered':
        this.#items.set(event.sku, {
          sku: event.sku,
          name: event.name,
          quantity: event.initialQuantity,
          reorderPoint: event.reorderPoint,
          lastUpdated: event.timestamp,
        });
        break;

      case 'StockIncremented': {
        const item = this.#items.get(event.sku);
        if (item) {
          item.quantity += event.quantity;
          item.lastUpdated = event.timestamp;
        }
        break;
      }

      case 'StockDecremented': {
        const item = this.#items.get(event.sku);
        if (item) {
          item.quantity -= event.quantity;
          item.lastUpdated = event.timestamp;
        }
        break;
      }

      case 'StockAdjusted': {
        const item = this.#items.get(event.sku);
        if (item) {
          item.quantity = event.newQuantity;
          item.lastUpdated = event.timestamp;
        }
        break;
      }
    }
  }

  get(sku) {
    return this.#items.get(sku) ?? null;
  }

  getAll() {
    return Array.from(this.#items.values());
  }
}

// Usage
const inventory = new InventorySystem();

// Listen for low stock alerts
inventory.alerts.on('data', (alert) => {
  console.warn(`[ALERT] Low stock: ${alert.sku} (${alert.quantity}/${alert.reorderPoint})`);
});

// Register items
inventory.dispatch({ type: 'RegisterItem', sku: 'LAPTOP-01', name: 'Laptop', quantity: 50, reorderPoint: 10 });
inventory.dispatch({ type: 'RegisterItem', sku: 'MOUSE-01', name: 'Mouse', quantity: 200, reorderPoint: 50 });

// Process sales
inventory.dispatch({ type: 'RemoveStock', sku: 'LAPTOP-01', quantity: 5, reason: 'sale', orderId: 'ORD-100' });
inventory.dispatch({ type: 'RemoveStock', sku: 'MOUSE-01', quantity: 160, reason: 'sale', orderId: 'ORD-101' });

// Query state
setTimeout(() => {
  console.log('All items:', inventory.queryAll());
  console.log('Laptop:', inventory.query('LAPTOP-01'));
}, 100);
```

## Best Practices Checklist

- [ ] Use append-only logs for event storage to ensure immutability and replayability
- [ ] Keep event payloads small and self-describing with clear type identifiers
- [ ] Implement event versioning to handle schema evolution gracefully
- [ ] Use snapshots to avoid replaying entire event history for frequently accessed aggregates
- [ ] Separate read and write paths (CQRS) for independent scaling and optimization
- [ ] Implement compensation logic for every saga step that has side effects
- [ ] Use dead letter queues to capture and inspect failed events without data loss
- [ ] Add correlation IDs to all events for distributed tracing across service boundaries
- [ ] Set buffer limits on EventEmitter-to-Stream bridges to prevent memory exhaustion
- [ ] Persist events before projecting them to ensure no events are lost on crash
- [ ] Use `objectMode: true` consistently when streams carry structured event objects
- [ ] Implement idempotent event handlers to safely replay events without side effects
- [ ] Monitor event processing lag to detect when projections fall behind
- [ ] Test saga compensation paths as thoroughly as happy paths
- [ ] Use timestamps and sequence numbers for deterministic event ordering

## Cross-References

- [01 - Streams Architecture](../01-streams-architecture) - Stream fundamentals (Readable, Writable, Transform)
- [02 - Buffer Mastery](../02-buffer-mastery) - Buffer handling for binary event payloads
- [03 - Stream Processing Patterns](../03-stream-processing-patterns) - Transform and pipeline patterns
- [07 - Stream Error Handling](../07-stream-error-handling) - Error recovery in event pipelines
- [09 - Stream Security](../09-stream-security) - Securing event data in transit and at rest
- [01 - Kafka & RabbitMQ Streaming](./01-kafka-rabbitmq-streaming) - Message queue integration used by event stores
- [03 - Microservices Stream Communication](./03-microservices-stream-communication) - Distributing events across services

## Next Steps

Proceed to [03 - Microservices Stream Communication](./03-microservices-stream-communication) to learn how to apply these event-driven patterns across service boundaries. You will implement stream-based RPC, handle backpressure propagation between services, add circuit breakers for resilience, and build a complete microservices data pipeline with distributed tracing.
