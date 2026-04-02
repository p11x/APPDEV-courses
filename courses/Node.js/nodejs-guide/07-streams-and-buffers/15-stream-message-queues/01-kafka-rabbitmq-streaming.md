# Kafka and RabbitMQ Stream Processing

## What You'll Learn

- How to wrap Kafka consumers as Node.js Readable streams
- How to wrap Kafka producers as Writable streams
- Consumer group stream processing for horizontal scaling
- Exactly-once semantics with idempotent Kafka streams
- RabbitMQ consumer/producer stream adapters using `amqplib`
- Exchange routing patterns with stream-based message handling
- Building an event-driven order processing pipeline with Kafka streams
- Handling backpressure in message queue stream integrations

## Kafka Consumer as Readable Stream

Wrap a `kafkajs` consumer so downstream code can use the standard stream API with backpressure support.

```js
// kafka-readable-stream.mjs
import { Readable } from 'node:stream';
import { Kafka } from 'kafkajs';

class KafkaConsumerStream extends Readable {
  #consumer;
  #topic;
  #fromBeginning;
  #processing = false;

  constructor({ brokers, groupId, topic, fromBeginning = false }) {
    super({ objectMode: true });
    const kafka = new Kafka({ clientId: 'stream-consumer', brokers });
    this.#consumer = kafka.consumer({ groupId });
    this.#topic = topic;
    this.#fromBeginning = fromBeginning;
  }

  async _construct(callback) {
    try {
      await this.#consumer.connect();
      await this.#consumer.subscribe({
        topic: this.#topic,
        fromBeginning: this.#fromBeginning,
      });

      await this.#consumer.run({
        eachMessage: async ({ topic, partition, message }) => {
          const record = {
            topic,
            partition,
            offset: message.offset,
            key: message.key?.toString(),
            value: JSON.parse(message.value.toString()),
            headers: Object.fromEntries(
              Object.entries(message.headers).map(([k, v]) => [k, v.toString()])
            ),
            timestamp: message.timestamp,
          };

          // Respect backpressure: pause until _read drains the buffer
          const canContinue = this.push(record);
          if (!canContinue) {
            await this.#consumer.pause([{ topic: this.#topic }]);
            await new Promise((resolve) => {
              this.once('resume-consumer', resolve);
            });
            await this.#consumer.resume([{ topic: this.#topic }]);
          }
        },
      });

      callback();
    } catch (err) {
      callback(err);
    }
  }

  _read() {
    // Signal that we can accept more data
    this.emit('resume-consumer');
  }

  async _destroy(err, callback) {
    try {
      await this.#consumer.disconnect();
    } catch {
      // ignore disconnect errors
    }
    callback(err);
  }
}

// Usage
const consumerStream = new KafkaConsumerStream({
  brokers: ['localhost:9092'],
  groupId: 'order-processing-group',
  topic: 'orders',
  fromBeginning: true,
});

consumerStream.on('data', (record) => {
  console.log('Received order:', record.value);
});

consumerStream.on('error', (err) => {
  console.error('Kafka stream error:', err);
});
```

## Kafka Producer as Writable Stream

Collect records from any Readable/Transform stream and publish them to Kafka in batches.

```js
// kafka-writable-stream.mjs
import { Writable } from 'node:stream';
import { Kafka } from 'kafkajs';

class KafkaProducerStream extends Writable {
  #producer;
  #topic;
  #queue = [];
  #flushThreshold;

  constructor({ brokers, topic, flushThreshold = 100 }) {
    super({ objectMode: true, highWaterMark: flushThreshold });
    const kafka = new Kafka({ clientId: 'stream-producer', brokers });
    this.#producer = kafka.producer({
      idempotent: true,
      maxInFlightRequests: 5,
    });
    this.#topic = topic;
    this.#flushThreshold = flushThreshold;
  }

  async _construct(callback) {
    try {
      await this.#producer.connect();
      callback();
    } catch (err) {
      callback(err);
    }
  }

  async _write(chunk, _encoding, callback) {
    try {
      const message = {
        key: chunk.key ?? null,
        value: JSON.stringify(chunk.value ?? chunk),
        headers: chunk.headers ?? {},
      };

      this.#queue.push(message);

      if (this.#queue.length >= this.#flushThreshold) {
        await this.#flush();
      }

      callback();
    } catch (err) {
      callback(err);
    }
  }

  async _final(callback) {
    try {
      await this.#flush();
      callback();
    } catch (err) {
      callback(err);
    }
  }

  async #flush() {
    if (this.#queue.length === 0) return;

    const messages = this.#queue.splice(0);
    await this.#producer.send({
      topic: this.#topic,
      messages,
      acks: -1, // all replicas
    });
  }

  async _destroy(err, callback) {
    try {
      await this.#producer.disconnect();
    } catch {
      // ignore
    }
    callback(err);
  }
}

// Usage: pipe data from any source into Kafka
const producerStream = new KafkaProducerStream({
  brokers: ['localhost:9092'],
  topic: 'processed-orders',
  flushThreshold: 50,
});

// Example: feed data manually
producerStream.write({ key: 'order-1', value: { id: 1, status: 'confirmed' } });
producerStream.end(() => console.log('All messages flushed to Kafka'));
```

## Kafka Consumer Group Stream Processing

Distribute partition assignment across multiple consumer instances for parallel stream processing.

```js
// kafka-consumer-group.mjs
import { Transform, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { Kafka } from 'kafkajs';

function createConsumerGroupStream({ brokers, groupId, topics, instanceId }) {
  const kafka = new Kafka({ clientId: `consumer-${instanceId}`, brokers });
  const consumer = kafka.consumer({
    groupId,
    sessionTimeout: 30000,
    heartbeatInterval: 3000,
  });

  const output = new PassThrough({ objectMode: true });

  const start = async () => {
    await consumer.connect();

    for (const topic of topics) {
      await consumer.subscribe({ topic, fromBeginning: false });
    }

    await consumer.run({
      eachBatchAutoResolve: true,
      eachBatch: async ({
        batch,
        resolveOffset,
        heartbeat,
        isRunning,
        isStale,
      }) => {
        for (const message of batch.messages) {
          if (!isRunning() || isStale()) break;

          const record = {
            topic: batch.topic,
            partition: batch.partition,
            offset: message.offset,
            key: message.key?.toString(),
            value: JSON.parse(message.value.toString()),
          };

          const canWrite = output.write(record);
          if (!canWrite) {
            await new Promise((resolve) => output.once('drain', resolve));
          }

          resolveOffset(message.offset);
          await heartbeat();
        }
      },
    });
  };

  return {
    stream: output,
    start,
    shutdown: async () => {
      await consumer.disconnect();
      output.end();
    },
  };
}

// Processor transform stream
function createOrderProcessor(instanceId) {
  return new Transform({
    objectMode: true,
    transform(record, _encoding, callback) {
      const processed = {
        ...record,
        processedBy: instanceId,
        processedAt: new Date().toISOString(),
        value: {
          ...record.value,
          total: record.value.items?.reduce((sum, i) => sum + i.price * i.qty, 0) ?? 0,
        },
      };
      callback(null, processed);
    },
  });
}

// Deploy multiple instances
async function runGroup(instanceCount = 3) {
  const instances = [];

  for (let i = 0; i < instanceCount; i++) {
    const group = createConsumerGroupStream({
      brokers: ['localhost:9092'],
      groupId: 'order-processors',
      topics: ['orders'],
      instanceId: `instance-${i}`,
    });

    await group.start();

    await pipeline(
      group.stream,
      createOrderProcessor(`instance-${i}`),
      async function* (source) {
        for await (const record of source) {
          console.log(`[${record.processedBy}] Partition ${record.partition}:`, record.value);
        }
      }
    );

    instances.push(group);
  }

  return instances;
}
```

## Exactly-Once Stream Processing

Ensure each message is processed exactly once using Kafka transactions and an idempotency store.

```js
// exactly-once-stream.mjs
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { Kafka } from 'kafkajs';

class ExactlyOnceProcessor {
  #producer;
  #consumer;
  #processedOffsets = new Map(); // topic-partition -> Set<offset>

  constructor({ brokers, groupId }) {
    const kafka = new Kafka({ clientId: 'exactly-once', brokers });
    this.#consumer = kafka.consumer({ groupId });
    this.#producer = kafka.producer({
      idempotent: true,
      transactionalId: `txn-${groupId}`,
      maxInFlightRequests: 5,
    });
  }

  async start(inputTopic, outputTopic, processFn) {
    await this.#consumer.connect();
    await this.#producer.connect();

    await this.#consumer.subscribe({ topic: inputTopic });

    await this.#consumer.run({
      eachBatchAutoResolve: false,
      eachBatch: async ({ batch, resolveOffset, heartbeat }) => {
        const txn = await this.#producer.transaction();

        try {
          const processedMessages = [];

          for (const message of batch.messages) {
            const partitionKey = `${batch.topic}-${batch.partition}`;

            // Idempotency check
            if (!this.#processedOffsets.has(partitionKey)) {
              this.#processedOffsets.set(partitionKey, new Set());
            }

            const offsetSet = this.#processedOffsets.get(partitionKey);
            if (offsetSet.has(message.offset)) {
              continue; // already processed
            }

            const input = JSON.parse(message.value.toString());
            const output = await processFn(input);

            processedMessages.push({
              key: message.key,
              value: JSON.stringify(output),
              headers: {
                'x-source-topic': batch.topic,
                'x-source-offset': message.offset,
                'x-processed-at': Date.now().toString(),
              },
            });

            offsetSet.add(message.offset);
            resolveOffset(message.offset);
          }

          if (processedMessages.length > 0) {
            await txn.send({
              topic: outputTopic,
              messages: processedMessages,
            });
          }

          await txn.sendOffsets({
            consumerGroupId: batch.consumerGroupId,
            topics: [{
              topic: batch.topic,
              partitions: [{
                partition: batch.partition,
                offset: (BigInt(batch.firstOffset()) + BigInt(batch.messages.length)).toString(),
              }],
            }],
          });

          await txn.commit();
          await heartbeat();
        } catch (err) {
          await txn.abort();
          throw err;
        }
      },
    });
  }

  async shutdown() {
    await this.#consumer.disconnect();
    await this.#producer.disconnect();
  }
}

// Usage
const processor = new ExactlyOnceProcessor({
  brokers: ['localhost:9092'],
  groupId: 'exactly-once-group',
});

await processor.start('raw-orders', 'validated-orders', async (order) => {
  // Idempotent processing logic
  return {
    ...order,
    validated: true,
    validationId: `val-${order.id}`,
    validatedAt: new Date().toISOString(),
  };
});
```

## RabbitMQ Consumer as Readable Stream

Wrap an `amqplib` consumer into a Node.js Readable stream with acknowledgement support.

```js
// rabbitmq-readable-stream.mjs
import { Readable } from 'node:stream';
import amqp from 'amqplib';

class RabbitMQConsumerStream extends Readable {
  #connection;
  #channel;
  #queue;
  #exchange;
  #routingKey;
  #prefetch;

  constructor({
    url = 'amqp://localhost',
    queue,
    exchange = '',
    routingKey = '',
    prefetch = 10,
  }) {
    super({ objectMode: true, highWaterMark: prefetch });
    this.#queue = queue;
    this.#exchange = exchange;
    this.#routingKey = routingKey;
    this.#prefetch = prefetch;
    this.#url = url;
    this.#connection = null;
    this.#channel = null;
  }

  async _construct(callback) {
    try {
      this.#connection = await amqp.connect(this.#url);
      this.#channel = await this.#connection.createChannel();

      await this.#channel.assertQueue(this.#queue, {
        durable: true,
        arguments: { 'x-queue-type': 'quorum' },
      });

      await this.#channel.prefetch(this.#prefetch);

      this.#channel.consume(this.#queue, (msg) => {
        if (msg === null) {
          this.push(null); // consumer cancelled
          return;
        }

        const record = {
          content: JSON.parse(msg.content.toString()),
          properties: msg.properties,
          fields: msg.fields,
          ack: () => this.#channel.ack(msg),
          nack: (requeue = false) => this.#channel.nack(msg, false, requeue),
        };

        this.push(record);
      });

      callback();
    } catch (err) {
      callback(err);
    }
  }

  _read() {
    // amqplib handles flow control via prefetch
  }

  async _destroy(err, callback) {
    try {
      await this.#channel?.close();
      await this.#connection?.close();
    } catch {
      // ignore
    }
    callback(err);
  }
}

// Usage
const mqStream = new RabbitMQConsumerStream({
  url: 'amqp://localhost',
  queue: 'order-events',
  prefetch: 25,
});

mqStream.on('data', async (record) => {
  try {
    console.log('Processing:', record.content);
    record.ack();
  } catch (err) {
    record.nack(true); // requeue on failure
  }
});
```

## RabbitMQ Producer as Writable Stream

Publish messages to RabbitMQ from any stream source with exchange routing.

```js
// rabbitmq-writable-stream.mjs
import { Writable } from 'node:stream';
import amqp from 'amqplib';

class RabbitMQProducerStream extends Writable {
  #connection;
  #channel;
  #exchange;
  #defaultRoutingKey;
  #exchangeType;

  constructor({
    url = 'amqp://localhost',
    exchange = 'events',
    exchangeType = 'topic',
    defaultRoutingKey = '',
  }) {
    super({ objectMode: true });
    this.#exchange = exchange;
    this.#exchangeType = exchangeType;
    this.#defaultRoutingKey = defaultRoutingKey;
    this.#url = url;
  }

  async _construct(callback) {
    try {
      this.#connection = await amqp.connect(this.#url);
      this.#channel = await this.#connection.createChannel();

      await this.#channel.assertExchange(this.#exchange, this.#exchangeType, {
        durable: true,
      });

      callback();
    } catch (err) {
      callback(err);
    }
  }

  async _write(chunk, _encoding, callback) {
    try {
      const routingKey = chunk.routingKey ?? this.#defaultRoutingKey;
      const body = JSON.stringify(chunk.body ?? chunk);
      const headers = chunk.headers ?? {};

      const published = this.#channel.publish(
        this.#exchange,
        routingKey,
        Buffer.from(body),
        {
          persistent: true,
          contentType: 'application/json',
          headers,
          messageId: chunk.messageId ?? crypto.randomUUID(),
          timestamp: Date.now(),
        }
      );

      if (!published) {
        await new Promise((resolve) => this.#channel.once('drain', resolve));
      }

      callback();
    } catch (err) {
      callback(err);
    }
  }

  async _final(callback) {
    try {
      // Wait for all pending confirms
      await this.#channel.waitForConfirms();
      callback();
    } catch (err) {
      callback(err);
    }
  }

  async _destroy(err, callback) {
    try {
      await this.#channel?.close();
      await this.#connection?.close();
    } catch {
      // ignore
    }
    callback(err);
  }
}

// Usage
const rabbitProducer = new RabbitMQProducerStream({
  url: 'amqp://localhost',
  exchange: 'order-events',
  exchangeType: 'topic',
  defaultRoutingKey: 'order.created',
});

rabbitProducer.write({
  routingKey: 'order.created',
  body: { orderId: '12345', customerId: 'c-001', total: 99.99 },
});
```

## RabbitMQ Exchange Routing with Streams

Route messages to different queues via topic exchanges, each backed by its own consumer stream.

```js
// rabbitmq-routing.mjs
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import amqp from 'amqplib';

class ExchangeRouter {
  #connection;
  #channel;
  #exchange;
  #bindings = new Map();

  constructor({ url, exchange, type = 'topic' }) {
    this.#url = url;
    this.#exchange = exchange;
    this.#type = type;
  }

  async connect() {
    this.#connection = await amqp.connect(this.#url);
    this.#channel = await this.#connection.createChannel();
    await this.#channel.assertExchange(this.#exchange, this.#type, {
      durable: true,
    });
  }

  async bindQueue(queue, pattern) {
    await this.#channel.assertQueue(queue, { durable: true });
    await this.#channel.bindQueue(queue, this.#exchange, pattern);
    this.#bindings.set(queue, pattern);
  }

  createConsumerStream(queue, prefetch = 10) {
    const self = this;
    return new Readable({
      objectMode: true,
      async construct(cb) {
        try {
          await self.#channel.prefetch(prefetch);
          cb();
        } catch (e) { cb(e); }
      },
      read() {
        self.#channel.consume(queue, (msg) => {
          if (!msg) { this.push(null); return; }
          this.push({
            queue,
            routingKey: msg.fields.routingKey,
            body: JSON.parse(msg.content.toString()),
            ack: () => self.#channel.ack(msg),
            nack: (requeue) => self.#channel.nack(msg, false, requeue),
          });
        });
      },
    });
  }

  async shutdown() {
    await this.#channel?.close();
    await this.#connection?.close();
  }
}

// Setup routing for order events
const router = new ExchangeRouter({
  url: 'amqp://localhost',
  exchange: 'order-events',
  type: 'topic',
});

await router.connect();
await router.bindQueue('order-confirmed', 'order.confirmed.#');
await router.bindQueue('order-shipped', 'order.shipped.#');
await router.bindQueue('order-failed', 'order.failed.#');

// Each queue gets its own stream pipeline
const confirmedStream = router.createConsumerStream('order-confirmed');
const shippedStream = router.createConsumerStream('order-shipped');

// Process confirmed orders
pipeline(confirmedStream, new Transform({
  objectMode: true,
  transform(msg, _enc, cb) {
    console.log('Confirmed:', msg.body);
    msg.ack();
    cb();
  },
}), (err) => {
  if (err) console.error('Confirmed pipeline error:', err);
});

// Process shipped orders
pipeline(shippedStream, new Transform({
  objectMode: true,
  transform(msg, _enc, cb) {
    console.log('Shipped:', msg.body);
    msg.ack();
    cb();
  },
}), (err) => {
  if (err) console.error('Shipped pipeline error:', err);
});
```

## Real-World: Event-Driven Order Processing Pipeline

A complete order processing system using Kafka streams for ingestion, validation, enrichment, and notification.

```js
// order-pipeline.mjs
import { Transform, PassThrough, pipeline } from 'node:stream';
import { promisify } from 'node:util';
import { Kafka } from 'kafkajs';

const pipelineAsync = promisify(pipeline);

class OrderProcessingPipeline {
  #kafka;
  #consumer;
  #producer;
  #metrics = { received: 0, validated: 0, enriched: 0, published: 0, failed: 0 };

  constructor({ brokers }) {
    this.#kafka = new Kafka({ clientId: 'order-pipeline', brokers });
    this.#consumer = this.#kafka.consumer({ groupId: 'order-pipeline-group' });
    this.#producer = this.#kafka.producer({ idempotent: true });
  }

  #createValidationStream() {
    return new Transform({
      objectMode: true,
      transform(record, _encoding, callback) {
        this.#metrics.received++;
        const order = record.value;

        if (!order.id || !order.items?.length || !order.customerId) {
          this.#metrics.failed++;
          return callback(new Error(`Invalid order: ${JSON.stringify(order)}`));
        }

        this.#metrics.validated++;
        callback(null, { ...record, value: { ...order, validated: true } });
      }.bind(this),
    });
  }

  #createEnrichmentStream() {
    return new Transform({
      objectMode: true,
      async transform(record, _encoding, callback) {
        try {
          const order = record.value;

          // Simulate enrichment: calculate totals, apply tax
          const subtotal = order.items.reduce(
            (sum, item) => sum + item.price * item.quantity, 0
          );
          const tax = subtotal * 0.08;
          const shipping = subtotal > 100 ? 0 : 9.99;

          const enriched = {
            ...order,
            subtotal,
            tax: Math.round(tax * 100) / 100,
            shipping,
            total: Math.round((subtotal + tax + shipping) * 100) / 100,
            enrichedAt: new Date().toISOString(),
          };

          this.#metrics.enriched++;
          callback(null, { ...record, value: enriched });
        } catch (err) {
          callback(err);
        }
      }.bind(this),
    });
  }

  #createPublishStream() {
    return new Transform({
      objectMode: true,
      async transform(record, _encoding, callback) {
        try {
          const order = record.value;
          const topic = order.total > 500 ? 'high-value-orders' : 'standard-orders';

          await this.#producer.send({
            topic,
            messages: [{
              key: order.id,
              value: JSON.stringify(order),
              headers: {
                'x-pipeline-version': '2.0',
                'x-processed-at': new Date().toISOString(),
              },
            }],
          });

          this.#metrics.published++;
          callback(null, record);
        } catch (err) {
          callback(err);
        }
      }.bind(this),
    });
  }

  async start() {
    await this.#consumer.connect();
    await this.#producer.connect();

    await this.#consumer.subscribe({ topic: 'raw-orders' });

    // Metrics reporter
    setInterval(() => {
      console.log('[Pipeline Metrics]', this.#metrics);
    }, 10000);

    await this.#consumer.run({
      eachMessage: async ({ message }) => {
        const input = new PassThrough({ objectMode: true });
        input.write({
          key: message.key?.toString(),
          value: JSON.parse(message.value.toString()),
          partition: message.partition,
          offset: message.offset,
        });
        input.end();

        try {
          await pipelineAsync(
            input,
            this.#createValidationStream(),
            this.#createEnrichmentStream(),
            this.#createPublishStream(),
            async function* (source) {
              for await (const record of source) {
                console.log(`Order ${record.value.id} processed: $${record.value.total}`);
              }
            }
          );
        } catch (err) {
          this.#metrics.failed++;
          console.error('Pipeline error for message:', err.message);
          // In production: send to dead letter topic
        }
      },
    });
  }

  getMetrics() {
    return { ...this.#metrics };
  }

  async shutdown() {
    await this.#consumer.disconnect();
    await this.#producer.disconnect();
  }
}

// Start the pipeline
const pipeline = new OrderProcessingPipeline({
  brokers: ['localhost:9092'],
});

await pipeline.start();

// Graceful shutdown
process.on('SIGTERM', async () => {
  console.log('Shutting down...', pipeline.getMetrics());
  await pipeline.shutdown();
  process.exit(0);
});
```

## Best Practices Checklist

- [ ] Implement backpressure handling between stream consumers and message queue clients
- [ ] Use consumer groups for horizontal scaling of Kafka stream processing
- [ ] Enable idempotent producers to prevent duplicate messages on retry
- [ ] Set appropriate `prefetch` values for RabbitMQ to control message flow rate
- [ ] Implement dead letter queues/topics for messages that fail processing
- [ ] Use transactions for exactly-once processing when output correctness is critical
- [ ] Handle graceful shutdown by draining in-flight messages before disconnecting
- [ ] Monitor consumer lag to detect processing bottlenecks early
- [ ] Use `objectMode: true` for structured message passing through streams
- [ ] Set `highWaterMark` based on expected message throughput and memory constraints
- [ ] Always ack/nack messages to prevent queue buildup from unacknowledged deliveries
- [ ] Use topic exchanges in RabbitMQ for flexible message routing patterns
- [ ] Implement health checks that verify both stream and queue connection status
- [ ] Log partition and offset information for debugging stream processing issues
- [ ] Test with realistic message volumes before deploying to production

## Cross-References

- [01 - Streams Architecture](../01-streams-architecture) - Foundational stream concepts used throughout
- [03 - Stream Processing Patterns](../03-stream-processing-patterns) - Transform and pipeline patterns applied here
- [06 - Stream Concurrency & Parallelism](../06-stream-concurrency-parallelism) - Parallel consumer group strategies
- [07 - Stream Error Handling](../07-stream-error-handling) - Error recovery for message queue streams
- [08 - Stream Performance Optimization](../08-stream-performance-optimization) - Tuning backpressure and buffer sizes
- [02 - Event-Driven Stream Architecture](./02-event-driven-stream-architecture) - Patterns built on top of queue streams
- [03 - Microservices Stream Communication](./03-microservices-stream-communication) - Inter-service messaging over streams

## Next Steps

Proceed to [02 - Event-Driven Stream Architecture](./02-event-driven-stream-architecture) to learn how to build event sourcing, CQRS, and saga patterns on top of the stream abstractions established here. You will learn to combine EventEmitter patterns with persistent stream storage, implement event replay capabilities, and design systems where streams serve as the authoritative source of truth for domain events.
