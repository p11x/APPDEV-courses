# MongoDB and PostgreSQL Stream Operations

## What You'll Learn

- Streaming query results from MongoDB cursors using Readable streams
- Writing data to MongoDB in bulk via Writable streams with automatic batching
- Subscribing to MongoDB change streams with Readable stream adapters
- Cursor-based streaming from PostgreSQL using `pg-cursor`
- Leveraging PostgreSQL COPY TO/COPY FROM for high-throughput data transfer
- Listening to PostgreSQL LISTEN/NOTIFY as a continuous event stream
- Integrating connection pools with Node.js streams for optimal resource usage
- Building a real-world data migration pipeline between MongoDB and PostgreSQL

## MongoDB Cursor as Readable Stream

A MongoDB cursor is natively a Readable stream. Calling `find().stream()` returns documents one at a time, preventing full result sets from loading into memory.

```js
import { MongoClient } from 'mongodb';
import { createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';

const client = new MongoClient('mongodb://localhost:27017');
await client.connect();

const db = client.db('analytics');
const collection = db.collection('events');

// find() cursor is a Readable stream
const cursor = collection
  .find({ processed: false })
  .sort({ timestamp: 1 })
  .batchSize(200);

const stringify = new Transform({
  objectMode: true,
  transform(doc, _enc, cb) {
    this.push(JSON.stringify(doc) + '\n');
    cb();
  },
});

const output = createWriteStream('./events-export.jsonl');

// stream documents directly to a file without loading all into memory
await pipeline(cursor, stringify, output);

console.log('Export complete');
await client.close();
```

### Controlling Backpressure on Cursors

```js
import { Readable } from 'node:stream';

// Wrap a cursor in a custom Readable for fine-grained control
class MongoCursorStream extends Readable {
  constructor(cursor, options) {
    super({ objectMode: true, ...options });
    this.cursor = cursor;
  }

  async _read() {
    try {
      const doc = await this.cursor.next();
      if (doc === null) {
        this.push(null); // end of data
      } else {
        this.push(doc);
      }
    } catch (err) {
      this.destroy(err);
    }
  }

  _destroy(err, cb) {
    this.cursor.close().then(() => cb(err)).catch(cb);
  }
}

const cursor = collection.find({}).batchSize(100);
const stream = new MongoCursorStream(cursor);

stream.on('data', (doc) => {
  // process each document
  console.log(doc._id);
});
```

## MongoDB Bulk Write via Writable Stream

Writing large volumes to MongoDB efficiently requires batching. A Writable stream collects documents in memory and flushes them using `bulkWrite` when the batch size is reached.

```js
import { Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { MongoClient } from 'mongodb';

class MongoBulkWriteStream extends Writable {
  constructor(collection, { batchSize = 1000, ordered = false } = {}) {
    super({ objectMode: true });
    this.collection = collection;
    this.batchSize = batchSize;
    this.ordered = ordered;
    this.batch = [];
    this.stats = { inserted: 0, batches: 0 };
  }

  async _write(doc, _enc, cb) {
    this.batch.push({ insertOne: { document: doc } });

    if (this.batch.length >= this.batchSize) {
      try {
        await this._flush();
        cb();
      } catch (err) {
        cb(err);
      }
    } else {
      cb();
    }
  }

  async _final(cb) {
    if (this.batch.length > 0) {
      try {
        await this._flush();
        cb();
      } catch (err) {
        cb(err);
      }
    } else {
      cb();
    }
  }

  async _flush() {
    const ops = this.batch;
    this.batch = [];
    const result = await this.collection.bulkWrite(ops, { ordered: this.ordered });
    this.stats.inserted += result.insertedCount;
    this.stats.batches += 1;
  }
}

// Usage: stream documents from a file into MongoDB
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';

const client = new MongoClient('mongodb://localhost:27017');
await client.connect();
const collection = client.db('warehouse').collection('products');

const input = createReadStream('./products.jsonl');
const lines = createInterface({ input, crlfDelay: Infinity });

const bulkWriter = new MongoBulkWriteStream(collection, {
  batchSize: 5000,
  ordered: false,
});

// Create an async iterator that parses each line
async function* parseDocs(source) {
  for await (const line of source) {
    if (line.trim()) yield JSON.parse(line);
  }
}

import { Writable as NodeWritable } from 'node:stream';

// Adapter: async iterable → Writable consumer
const docReader = new NodeWritable({
  objectMode: true,
  write(doc, _enc, cb) {
    bulkWriter.write(doc, cb);
  },
  final(cb) {
    bulkWriter.end(cb);
  },
});

for await (const doc of parseDocs(lines)) {
  if (!docReader.write(doc)) {
    await new Promise((resolve) => docReader.once('drain', resolve));
  }
}

docReader.end();

console.log('Bulk write stats:', bulkWriter.stats);
await client.close();
```

## MongoDB Change Streams as Readable Stream Adapter

Change streams let you watch for real-time data changes. Wrapping them in a Readable stream integrates them into pipeline processing.

```js
import { Readable } from 'node:stream';
import { MongoClient } from 'mongodb';
import { pipeline } from 'node:stream/promises';

class ChangeStreamAdapter extends Readable {
  constructor(collection, pipeline = [], options = {}) {
    super({ objectMode: true });
    this._collection = collection;
    this._pipeline = pipeline;
    this._options = { fullDocument: 'updateLookup', ...options };
    this._changeStream = null;
  }

  async _read() {
    if (!this._changeStream) {
      this._changeStream = this._collection.watch(
        this._pipeline,
        this._options
      );

      this._changeStream.on('change', (change) => {
        if (!this.push(change)) {
          this._changeStream.pause();
        }
      });

      this._changeStream.on('error', (err) => this.destroy(err));
      this._changeStream.on('end', () => this.push(null));
    } else {
      this._changeStream.resume();
    }
  }

  async _destroy(err, cb) {
    if (this._changeStream) {
      await this._changeStream.close();
    }
    cb(err);
  }
}

// Usage: watch for new orders and process them
const client = new MongoClient('mongodb://localhost:27017');
await client.connect();
const orders = client.db('ecommerce').collection('orders');

const changes = new ChangeStreamAdapter(orders, [
  { $match: { 'operationType': { $in: ['insert', 'update'] } } },
]);

const processor = new Writable({
  objectMode: true,
  write(change, _enc, cb) {
    console.log(`Change: ${change.operationType} on doc ${change.documentKey._id}`);
    // Process change: update cache, send notification, etc.
    cb();
  },
});

try {
  await pipeline(changes, processor);
} finally {
  await client.close();
}
```

## PostgreSQL Cursor Streaming with pg-cursor

The `pg-cursor` library enables server-side cursors that stream rows incrementally instead of fetching the entire result set.

```js
import pg from 'pg';
import Cursor from 'pg-cursor';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';

const { Pool } = pg;
const pool = new Pool({
  connectionString: 'postgresql://user:pass@localhost:5432/analytics',
  max: 10,
});

// Create a stream from a server-side cursor
class PgCursorStream extends Transform {
  constructor(pool, query, params = [], batchSize = 200) {
    super({ objectMode: true });
    this.pool = pool;
    this.query = query;
    this.params = params;
    this.batchSize = batchSize;
  }

  async _transform(_chunk, _enc, cb) {
    // This transform is used as a passthrough entry point
    cb();
  }

  async _flush(cb) {
    const client = await this.pool.connect();
    try {
      const cursor = client.query(new Cursor(this.query, this.params));

      const readBatch = () => {
        cursor.read(this.batchSize, (err, rows) => {
          if (err) {
            cb(err);
            return;
          }
          if (rows.length === 0) {
            cursor.close(() => {
              client.release();
              cb();
            });
            return;
          }
          for (const row of rows) {
            this.push(row);
          }
          readBatch();
        });
      };

      readBatch();
    } catch (err) {
      client.release();
      cb(err);
    }
  }
}

// Simpler approach: use cursor directly with async iteration
async function* streamRows(pool, query, params = [], batchSize = 500) {
  const client = await pool.connect();
  try {
    const cursor = client.query(new Cursor(query, params));
    while (true) {
      const rows = await new Promise((resolve, reject) => {
        cursor.read(batchSize, (err, rows) => {
          if (err) reject(err);
          else resolve(rows);
        });
      });
      if (rows.length === 0) break;
      yield* rows;
    }
    await new Promise((resolve) => cursor.close(resolve));
  } finally {
    client.release();
  }
}

// Usage
const rowStream = streamRows(
  pool,
  'SELECT id, name, email, created_at FROM users WHERE active = $1 ORDER BY id',
  [true],
  1000
);

for await (const row of rowStream) {
  console.log(row.id, row.email);
}

await pool.end();
```

## PostgreSQL COPY Command Streaming

COPY TO and COPY FROM provide the fastest way to transfer bulk data to/from PostgreSQL. They bypass the query executor and stream raw data.

```js
import pg from 'pg';
import { pipeline } from 'node:stream/promises';
import { createWriteStream, createReadStream } from 'node:fs';
import { Transform, PassThrough } from 'node:stream';

const { Pool } = pg;
const pool = new Pool({ connectionString: 'postgresql://localhost:5432/sales' });

// COPY TO: export table to a file stream
async function copyToStream(query, outputPath) {
  const client = await pool.connect();
  try {
    const copyStream = client.query(
      pg.CopyTo("COPY (SELECT * FROM orders WHERE year = 2025) TO STDOUT WITH (FORMAT CSV, HEADER)")
    );
    const output = createWriteStream(outputPath);

    await pipeline(copyStream, output);
    console.log('COPY TO complete');
  } finally {
    client.release();
  }
}

// COPY FROM: import from a file stream into a table
async function copyFromStream(inputPath, table) {
  const client = await pool.connect();
  try {
    const copyStream = client.query(
      pg.CopyFrom(`COPY ${table} FROM STDIN WITH (FORMAT CSV, HEADER)`)
    );
    const input = createReadStream(inputPath);

    await pipeline(input, copyStream);
    console.log('COPY FROM complete');
  } finally {
    client.release();
  }
}

// COPY TO with transformation: export as JSON instead of CSV
async function copyToJson(query, outputPath) {
  const client = await pool.connect();
  try {
    const copyStream = client.query(
      pg.CopyTo("COPY (SELECT * FROM products) TO STDOUT WITH (FORMAT CSV, HEADER)")
    );
    const output = createWriteStream(outputPath);

    let isFirst = true;
    const headerFields = [];

    const csvToJson = new Transform({
      transform(chunk, _enc, cb) {
        const lines = chunk.toString().split('\n');
        for (const line of lines) {
          if (!line.trim()) continue;
          const fields = line.split(',');
          if (isFirst) {
            headerFields.push(...fields);
            isFirst = false;
            this.push('[\n');
            continue;
          }
          const obj = {};
          headerFields.forEach((h, i) => { obj[h] = fields[i]; });
          const prefix = this._firstDoc ? ',\n' : '';
          this._firstDoc = false;
          this.push(prefix + JSON.stringify(obj));
        }
        cb();
      },
      flush(cb) {
        this.push('\n]');
        cb();
      },
    });
    csvToJson._firstDoc = true;

    await pipeline(copyStream, csvToJson, output);
    console.log('JSON export complete');
  } finally {
    client.release();
  }
}

await copyToStream('./orders_2025.csv');
await copyFromStream('./new_orders.csv', 'orders');
await pool.end();
```

## PostgreSQL LISTEN/NOTIFY as Event Stream

PostgreSQL NOTIFY provides pub/sub messaging. Wrapping it in a Readable stream turns notifications into a consumable event pipeline.

```js
import pg from 'pg';
import { Readable } from 'node:stream';

const { Pool } = pg;

class PgNotifyStream extends Readable {
  constructor(connectionString, channels = []) {
    super({ objectMode: true });
    this._connectionString = connectionString;
    this._channels = channels;
    this._client = null;
  }

  async _read() {
    if (this._client) return; // already listening

    this._client = new pg.Client(this._connectionString);
    await this._client.connect();

    for (const channel of this._channels) {
      await this._client.query(`LISTEN ${channel}`);
    }

    this._client.on('notification', (msg) => {
      const event = {
        channel: msg.channel,
        payload: msg.payload ? JSON.parse(msg.payload) : null,
        processId: msg.processId,
        receivedAt: new Date(),
      };
      if (!this.push(event)) {
        // pause events until consumer drains
        this._client.removeAllListeners('notification');
        this._client.on('notification', (m) => {
          // buffer and resume later
          this._pending = this._pending || [];
          this._pending.push(m);
        });
      }
    });

    this._client.on('error', (err) => this.destroy(err));
  }

  _resume() {
    if (this._pending?.length) {
      for (const msg of this._pending) {
        this.push({
          channel: msg.channel,
          payload: msg.payload ? JSON.parse(msg.payload) : null,
          processId: msg.processId,
          receivedAt: new Date(),
        });
      }
      this._pending = [];
    }
  }

  async _destroy(err, cb) {
    if (this._client) {
      for (const channel of this._channels) {
        await this._client.query(`UNLISTEN ${channel}`).catch(() => {});
      }
      await this._client.end();
    }
    cb(err);
  }
}

// Usage
import { Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const notifyStream = new PgNotifyStream(
  'postgresql://localhost:5432/myapp',
  ['order_events', 'user_events']
);

const handler = new Writable({
  objectMode: true,
  write(event, _enc, cb) {
    console.log(`[${event.channel}]`, event.payload);
    cb();
  },
});

await pipeline(notifyStream, handler);
```

## Connection Pool Integration with Streams

Proper pool management ensures streams don't exhaust connection limits under concurrent load.

```js
import pg from 'pg';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool } = pg;

const pool = new Pool({
  host: 'localhost',
  port: 5432,
  database: 'warehouse',
  max: 20,             // max connections
  idleTimeoutMillis: 30000,
  connectionTimeoutMillis: 5000,
});

// Monitor pool health during streaming
pool.on('error', (err) => {
  console.error('Unexpected pool error:', err);
});

// Stream-aware pool wrapper: leases a connection, releases on stream end
class PoolLeaseTransform extends Transform {
  constructor(pool, transformFn) {
    super({ objectMode: true });
    this._pool = pool;
    this._client = null;
    this._transformFn = transformFn;
  }

  async _transform(chunk, _enc, cb) {
    if (!this._client) {
      this._client = await this._pool.connect();
    }
    try {
      const result = await this._transformFn(this._client, chunk);
      if (result !== undefined) this.push(result);
      cb();
    } catch (err) {
      cb(err);
    }
  }

  async _flush(cb) {
    if (this._client) {
      this._client.release();
      this._client = null;
    }
    cb();
  }
}

// Usage: stream records and enrich each with a database lookup
const enricher = new PoolLeaseTransform(pool, async (client, record) => {
  const { rows } = await client.query(
    'SELECT category, price FROM products WHERE id = $1',
    [record.productId]
  );
  if (rows.length > 0) {
    return { ...record, ...rows[0] };
  }
  return record;
});

// pipeline(input, enricher, output)
```

## Real-World: Streaming Data Migration Between MongoDB and PostgreSQL

This example migrates a `users` collection from MongoDB to a PostgreSQL table, with data transformation and progress tracking.

```js
import { MongoClient } from 'mongodb';
import pg from 'pg';
import { Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool } = pg;

// Source: MongoDB
const mongoClient = new MongoClient('mongodb://localhost:27017');
await mongoClient.connect();
const mongoCol = mongoClient.db('legacy').collection('users');

// Target: PostgreSQL
const pgPool = new Pool({
  connectionString: 'postgresql://localhost:5432/modern',
});

// Ensure target table exists
await pgPool.query(`
  CREATE TABLE IF NOT EXISTS users (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    email TEXT UNIQUE NOT NULL,
    name TEXT,
    plan TEXT DEFAULT 'free',
    migrated_at TIMESTAMPTZ DEFAULT NOW()
  )
`);

// Transform: convert MongoDB document to PostgreSQL row
const transformDoc = new Transform({
  objectMode: true,
  transform(doc, _enc, cb) {
    cb(null, {
      email: doc.email,
      name: `${doc.firstName} ${doc.lastName}`.trim(),
      plan: doc.subscription?.plan || 'free',
    });
  },
});

// Writable: batch insert into PostgreSQL
const BATCH_SIZE = 2000;
let batch = [];
let totalInserted = 0;
const startTime = Date.now();

const pgWriter = new Writable({
  objectMode: true,
  async write(row, _enc, cb) {
    batch.push(row);
    if (batch.length >= BATCH_SIZE) {
      try {
        await flushBatch();
        cb();
      } catch (err) {
        cb(err);
      }
    } else {
      cb();
    }
  },
  async final(cb) {
    try {
      if (batch.length > 0) await flushBatch();
      const elapsed = ((Date.now() - startTime) / 1000).toFixed(1);
      console.log(`Migration complete: ${totalInserted} rows in ${elapsed}s`);
      cb();
    } catch (err) {
      cb(err);
    }
  },
});

async function flushBatch() {
  const rows = batch;
  batch = [];

  // Build parameterized query for batch insert
  const values = [];
  const placeholders = [];
  let idx = 1;

  for (const row of rows) {
    placeholders.push(`($${idx}, $${idx + 1}, $${idx + 2})`);
    values.push(row.email, row.name, row.plan);
    idx += 3;
  }

  const query = `
    INSERT INTO users (email, name, plan)
    VALUES ${placeholders.join(', ')}
    ON CONFLICT (email) DO UPDATE SET
      name = EXCLUDED.name,
      plan = EXCLUDED.plan,
      migrated_at = NOW()
  `;

  await pgPool.query(query, values);
  totalInserted += rows.length;

  if (totalInserted % 10000 === 0) {
    const elapsed = ((Date.now() - startTime) / 1000).toFixed(1);
    console.log(`Progress: ${totalInserted} rows (${elapsed}s)`);
  }
}

// Execute the migration pipeline
const mongoCursor = mongoCol
  .find({ email: { $exists: true } })
  .project({ email: 1, firstName: 1, lastName: 1, subscription: 1 })
  .batchSize(500);

try {
  await pipeline(mongoCursor, transformDoc, pgWriter);
} finally {
  await mongoClient.close();
  await pgPool.end();
}
```

## Best Practices Checklist

- [ ] Use `batchSize()` on MongoDB cursors to control fetch sizes and memory usage
- [ ] Always close cursors and release connections in stream `_destroy`/`_final` handlers
- [ ] Use `COPY TO/FROM` instead of row-by-row streaming for bulk PostgreSQL data transfer
- [ ] Set `ordered: false` in MongoDB bulk writes for better throughput when order doesn't matter
- [ ] Monitor connection pool utilization during long-running stream operations
- [ ] Use `ON CONFLICT` clauses to make stream-based inserts idempotent and resumable
- [ ] Implement backpressure propagation from database writers back to source readers
- [ ] Log progress periodically (every N rows) during long migrations
- [ ] Use transactions with `SAVEPOINT` for batch error recovery in PostgreSQL streams
- [ ] Test with realistic data volumes to catch memory leaks and connection exhaustion
- [ ] Handle MongoDB change stream resume tokens for fault-tolerant real-time pipelines
- [ ] Set appropriate `statement_timeout` and `idle_in_transaction_session_timeout` on PostgreSQL connections

## Cross-References

- [Stream Architecture](../01-streams-architecture/01-nodejs-stream-architecture.md) — foundational concepts of Readable, Writable, and Transform streams
- [Buffer Mastery](../02-buffer-mastery/01-binary-data-and-buffers.md) — handling binary data when dealing with COPY streams
- [Stream Error Handling](../07-stream-error-handling/) — retry strategies for database stream failures
- [Stream Concurrency](../06-stream-concurrency-parallelism/) — parallelizing database stream consumers
- [Stream Performance](../08-stream-performance-optimization/) — tuning batch sizes and buffer watermarks

## Next Steps

- Explore **Redis and Cloud Storage Streaming** (`02-redis-s3-storage-streams.md`) for caching and object storage integration
- Learn **Stream-Based Database Operations** (`03-database-migration-sync.md`) for ETL pipelines and CDC patterns
- Study **Stream Testing** (`../11-stream-testing/`) for unit testing database stream implementations
