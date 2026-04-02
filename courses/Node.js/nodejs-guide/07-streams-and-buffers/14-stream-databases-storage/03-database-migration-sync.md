# Stream-Based Database Operations (Migration, Export, Sync)

## What You'll Learn

- Building source → transform → target pipelines for database migration
- Exporting query results as streaming CSV and JSON files
- Importing CSV/JSON files into databases using streaming parsers
- Constructing ETL pipelines for data warehousing with stream stages
- Capturing database changes as continuous event streams (CDC)
- Synchronizing data between databases with stream-based diffing
- Executing zero-downtime migrations using dual-write and stream replay

## Stream-Based Database Migration

A migration pipeline connects three stages: a source Readable (cursor), a Transform (data mapping), and a target Writable (batch inserter). Streams ensure memory stays bounded regardless of data volume.

```js
import { MongoClient } from 'mongodb';
import pg from 'pg';
import { Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool } = pg;

// --- Source: MongoDB cursor ---
const mongo = new MongoClient('mongodb://localhost:27017');
await mongo.connect();
const source = mongo.db('legacy').collection('orders');

// --- Target: PostgreSQL pool ---
const pgPool = new Pool({ connectionString: 'postgresql://localhost:5432/modern' });

// --- Transform: map MongoDB doc to PostgreSQL row ---
const transform = new Transform({
  objectMode: true,
  transform(doc, _enc, cb) {
    cb(null, {
      order_id: doc.orderId,
      customer_email: doc.customer.email,
      total: doc.totalAmount,
      currency: doc.currency,
      status: doc.status,
      created_at: doc.createdAt,
    });
  },
});

// --- Writable: batch INSERT into PostgreSQL ---
const BATCH_SIZE = 2000;
let batch = [];
let totalInserted = 0;

const target = new Writable({
  objectMode: true,
  async write(row, _enc, cb) {
    batch.push(row);
    if (batch.length >= BATCH_SIZE) {
      try {
        await flush();
        cb();
      } catch (err) { cb(err); }
    } else { cb(); }
  },
  async final(cb) {
    try {
      await flush();
      console.log(`Migration complete: ${totalInserted} rows`);
      cb();
    } catch (err) { cb(err); }
  },
});

async function flush() {
  if (batch.length === 0) return;
  const rows = batch;
  batch = [];
  const vals = [];
  const ph = [];
  let i = 1;
  for (const r of rows) {
    ph.push(`($${i},$${i+1},$${i+2},$${i+3},$${i+4},$${i+5})`);
    vals.push(r.order_id, r.customer_email, r.total, r.currency, r.status, r.created_at);
    i += 6;
  }
  await pgPool.query(
    `INSERT INTO orders (order_id, customer_email, total, currency, status, created_at)
     VALUES ${ph.join(',')}
     ON CONFLICT (order_id) DO UPDATE SET status = EXCLUDED.status, total = EXCLUDED.total`,
    vals
  );
  totalInserted += rows.length;
  console.log(`Migrated ${totalInserted} orders...`);
}

// Execute
const cursor = source.find({}).sort({ createdAt: 1 }).batchSize(500);

try {
  await pipeline(cursor, transform, target);
} finally {
  await mongo.close();
  await pgPool.end();
}
```

### Generic Migration Adapter

```js
import { Readable, Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

/**
 * Generic migration pipeline
 * @param {Readable} source       - Cursor or reader stream
 * @param {Transform} transform   - Data mapping transform
 * @param {Writable} target       - Batch writer
 * @param {object} options        - { highWaterMark }
 */
async function migrate(source, transform, target, options = {}) {
  const startTime = Date.now();

  const counter = new Transform({
    objectMode: true,
    transform(chunk, _enc, cb) {
      this._count = (this._count || 0) + 1;
      if (this._count % 10000 === 0) {
        const elapsed = ((Date.now() - startTime) / 1000).toFixed(1);
        console.log(`Progress: ${this._count} rows (${elapsed}s)`);
      }
      this.push(chunk);
      cb();
    },
  });

  await pipeline(source, transform, counter, target, options);
  return counter._count || 0;
}
```

## Streaming CSV Export from Database Queries

Exporting large query results to CSV without buffering the entire dataset in memory.

```js
import pg from 'pg';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createWriteStream } from 'node:fs';

const { Pool } = pg;
const pool = new Pool({ connectionString: 'postgresql://localhost:5432/analytics' });

class CsvTransform extends Transform {
  constructor(columns, options = {}) {
    super({ objectMode: true, ...options });
    this._columns = columns;
    this._headerWritten = false;
  }

  _transform(row, _enc, cb) {
    if (!this._headerWritten) {
      this.push(this._columns.join(',') + '\n');
      this._headerWritten = true;
    }

    const values = this._columns.map((col) => {
      const val = row[col];
      if (val === null || val === undefined) return '';
      const str = String(val);
      // Escape fields containing commas, quotes, or newlines
      if (str.includes(',') || str.includes('"') || str.includes('\n')) {
        return `"${str.replace(/"/g, '""')}"`;
      }
      return str;
    });

    this.push(values.join(',') + '\n');
    cb();
  }
}

// pg-query-stream: streams rows one at a time from a server-side cursor
import QueryStream from 'pg-query-stream';

async function exportCsv(query, params, columns, outputPath) {
  const client = await pool.connect();
  try {
    const qs = new QueryStream(query, params, { batchSize: 500 });
    const rows = client.query(qs);
    const csv = new CsvTransform(columns);
    const output = createWriteStream(outputPath);

    await pipeline(rows, csv, output);
    console.log(`Exported to ${outputPath}`);
  } finally {
    client.release();
  }
}

await exportCsv(
  'SELECT id, email, plan, created_at FROM users WHERE active = $1 ORDER BY id',
  [true],
  ['id', 'email', 'plan', 'created_at'],
  './active_users.csv'
);

await pool.end();
```

## Streaming JSON Export from Database Queries

```js
import pg from 'pg';
import QueryStream from 'pg-query-stream';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createWriteStream } from 'node:fs';

const { Pool } = pg;
const pool = new Pool({ connectionString: 'postgresql://localhost:5432/analytics' });

class JsonLinesTransform extends Transform {
  constructor(options = {}) {
    super({ objectMode: true, ...options });
  }

  _transform(row, _enc, cb) {
    this.push(JSON.stringify(row) + '\n');
    cb();
  }
}

class JsonArrayTransform extends Transform {
  constructor(options = {}) {
    super({ objectMode: true, ...options });
    this._first = true;
  }

  _transform(row, _enc, cb) {
    if (this._first) {
      this.push('[\n  ');
      this._first = false;
    } else {
      this.push(',\n  ');
    }
    this.push(JSON.stringify(row));
    cb();
  }

  _flush(cb) {
    if (this._first) {
      this.push('[\n]');
    } else {
      this.push('\n]');
    }
    cb();
  }
}

// Export as JSON Lines (newline-delimited JSON)
async function exportJsonl(query, params, outputPath) {
  const client = await pool.connect();
  try {
    const qs = new QueryStream(query, params, { batchSize: 1000 });
    const rows = client.query(qs);
    const output = createWriteStream(outputPath);

    await pipeline(rows, new JsonLinesTransform(), output);
    console.log(`JSONL export: ${outputPath}`);
  } finally {
    client.release();
  }
}

// Export as formatted JSON array
async function exportJsonArray(query, params, outputPath) {
  const client = await pool.connect();
  try {
    const qs = new QueryStream(query, params, { batchSize: 1000 });
    const rows = client.query(qs);
    const output = createWriteStream(outputPath);

    await pipeline(rows, new JsonArrayTransform(), output);
    console.log(`JSON export: ${outputPath}`);
  } finally {
    client.release();
  }
}

await exportJsonl(
  'SELECT * FROM orders WHERE created_at > NOW() - INTERVAL \'30 days\'',
  [],
  './recent_orders.jsonl'
);

await exportJsonArray(
  'SELECT id, name, revenue FROM products ORDER BY revenue DESC LIMIT 1000',
  [],
  './top_products.json'
);

await pool.end();
```

## Streaming Data Import from Files to Database

Ingest CSV and JSONL files row-by-row into a database without loading the full file.

```js
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';
import pg from 'pg';
import { Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool } = pg;
const pool = new Pool({ connectionString: 'postgresql://localhost:5432/warehouse' });

// CSV line parser (handles quoted fields)
function parseCsvLine(line) {
  const fields = [];
  let current = '';
  let inQuotes = false;

  for (let i = 0; i < line.length; i++) {
    const ch = line[i];
    if (inQuotes) {
      if (ch === '"' && line[i + 1] === '"') {
        current += '"';
        i++;
      } else if (ch === '"') {
        inQuotes = false;
      } else {
        current += ch;
      }
    } else if (ch === '"') {
      inQuotes = true;
    } else if (ch === ',') {
      fields.push(current);
      current = '';
    } else {
      current += ch;
    }
  }
  fields.push(current);
  return fields;
}

class CsvRowParser extends Transform {
  constructor(columns, options = {}) {
    super({ objectMode: true, ...options });
    this._columns = columns;
    this._headerSkipped = false;
  }

  _transform(line, _enc, cb) {
    if (!this._headerSkipped) {
      this._headerSkipped = true;
      cb();
      return;
    }

    const values = parseCsvLine(line);
    const row = {};
    this._columns.forEach((col, i) => {
      row[col] = values[i] || null;
    });
    this.push(row);
    cb();
  }
}

class BatchInserter extends Writable {
  constructor(pool, table, columns, batchSize = 2000) {
    super({ objectMode: true });
    this._pool = pool;
    this._table = table;
    this._columns = columns;
    this._batchSize = batchSize;
    this._batch = [];
    this._count = 0;
  }

  async _write(row, _enc, cb) {
    this._batch.push(row);
    if (this._batch.length >= this._batchSize) {
      try { await this._flush(); cb(); }
      catch (err) { cb(err); }
    } else { cb(); }
  }

  async _final(cb) {
    try { await this._flush(); cb(); }
    catch (err) { cb(err); }
  }

  async _flush() {
    if (this._batch.length === 0) return;
    const rows = this._batch;
    this._batch = [];
    const vals = [];
    const ph = [];
    let idx = 1;
    for (const r of rows) {
      const placeholders = this._columns.map((_, i) => `$${idx + i}`);
      ph.push(`(${placeholders.join(',')})`);
      vals.push(...this._columns.map((c) => r[c]));
      idx += this._columns.length;
    }
    await this._pool.query(
      `INSERT INTO ${this._table} (${this._columns.join(',')}) VALUES ${ph.join(',')}`,
      vals
    );
    this._count += rows.length;
  }

  get count() { return this._count; }
}

// Import CSV file
async function importCsv(filePath, table, columns) {
  const input = createReadStream(filePath);
  const lines = createInterface({ input, crlfDelay: Infinity });

  const parser = new CsvRowParser(columns);
  const inserter = new BatchInserter(pool, table, columns, 3000);

  // Adapt readline to stream
  const lineStream = new Transform({
    objectMode: true,
    transform(line, _enc, cb) {
      if (line.trim()) this.push(line);
      cb();
    },
  });

  // Connect readline to transform
  lines.on('line', (line) => lineStream.write(line));
  lines.on('close', () => lineStream.end());

  await pipeline(lineStream, parser, inserter);
  console.log(`Imported ${inserter.count} rows into ${table}`);
}

// Import JSONL file
async function importJsonl(filePath, table, columns) {
  const input = createReadStream(filePath);
  const lines = createInterface({ input, crlfDelay: Infinity });

  const parser = new Transform({
    objectMode: true,
    transform(line, _enc, cb) {
      if (!line.trim()) return cb();
      try {
        const doc = JSON.parse(line);
        const row = {};
        columns.forEach((c) => { row[c] = doc[c]; });
        this.push(row);
        cb();
      } catch (err) { cb(err); }
    },
  });

  const inserter = new BatchInserter(pool, table, columns, 3000);

  const lineStream = new Transform({
    objectMode: true,
    transform(line, _enc, cb) {
      if (line.trim()) this.push(line);
      cb();
    },
  });

  lines.on('line', (line) => lineStream.write(line));
  lines.on('close', () => lineStream.end());

  await pipeline(lineStream, parser, inserter);
  console.log(`Imported ${inserter.count} rows into ${table}`);
}

await importCsv('./users.csv', 'users', ['id', 'name', 'email', 'plan']);
await importJsonl('./events.jsonl', 'events', ['event_id', 'user_id', 'type', 'timestamp']);

await pool.end();
```

## Stream-Based ETL Pipeline for Data Warehousing

An ETL (Extract, Transform, Load) pipeline chains extraction, transformation, and loading as stream stages.

```js
import pg from 'pg';
import QueryStream from 'pg-query-stream';
import { Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool } = pg;

// Source database (OLTP)
const sourcePool = new Pool({ connectionString: 'postgresql://localhost:5432/oltp' });

// Target database (OLAP / warehouse)
const targetPool = new Pool({ connectionString: 'postgresql://localhost:5432/warehouse' });

// === EXTRACT: stream rows from source ===
async function* extractSource(query, params = [], batchSize = 1000) {
  const client = await sourcePool.connect();
  try {
    const cursor = client.query(new QueryStream(query, params, { batchSize }));
    for await (const row of cursor) {
      yield row;
    }
  } finally {
    client.release();
  }
}

// === TRANSFORM: dimension lookup and fact computation ===
class DimensionLookupTransform extends Transform {
  constructor(dimensionCache) {
    super({ objectMode: true });
    this._cache = dimensionCache;
  }

  async _transform(factRow, _enc, cb) {
    try {
      // Lookup dim keys (cached)
      const customerKey = this._cache.customers.get(factRow.customer_email) || -1;
      const productKey = this._cache.products.get(factRow.product_sku) || -1;

      const transformed = {
        order_date: factRow.order_date,
        customer_key: customerKey,
        product_key: productKey,
        quantity: factRow.quantity,
        unit_price: factRow.unit_price,
        total_amount: factRow.quantity * factRow.unit_price,
        discount: factRow.discount || 0,
        region: factRow.region || 'UNKNOWN',
      };

      this.push(transformed);
      cb();
    } catch (err) {
      cb(err);
    }
  }
}

// === LOAD: batch insert into target ===
class WarehouseLoader extends Writable {
  constructor(pool, batchSize = 5000) {
    super({ objectMode: true });
    this._pool = pool;
    this._batchSize = batchSize;
    this._batch = [];
    this._loaded = 0;
  }

  async _write(row, _enc, cb) {
    this._batch.push(row);
    if (this._batch.length >= this._batchSize) {
      try { await this._flush(); cb(); }
      catch (err) { cb(err); }
    } else { cb(); }
  }

  async _final(cb) {
    try {
      await this._flush();
      console.log(`Warehouse load complete: ${this._loaded} facts`);
      cb();
    } catch (err) { cb(err); }
  }

  async _flush() {
    if (this._batch.length === 0) return;
    const rows = this._batch;
    this._batch = [];

    const vals = [];
    const ph = [];
    let i = 1;
    for (const r of rows) {
      ph.push(`($${i},$${i+1},$${i+2},$${i+3},$${i+4},$${i+5},$${i+6},$${i+7})`);
      vals.push(r.order_date, r.customer_key, r.product_key, r.quantity,
                 r.unit_price, r.total_amount, r.discount, r.region);
      i += 8;
    }

    await this._pool.query(
      `INSERT INTO fact_sales (order_date, customer_key, product_key, quantity,
         unit_price, total_amount, discount, region)
       VALUES ${ph.join(',')}`,
      vals
    );
    this._loaded += rows.length;
  }
}

// Build dimension caches
async function loadDimensionCaches() {
  const customerRows = await sourcePool.query(
    'SELECT DISTINCT customer_email, customer_id FROM orders'
  );
  const productRows = await sourcePool.query(
    'SELECT DISTINCT sku, product_id FROM products'
  );

  return {
    customers: new Map(customerRows.rows.map((r) => [r.customer_email, r.customer_id])),
    products: new Map(productRows.rows.map((r) => [r.sku, r.product_id])),
  };
}

// Run the ETL
const caches = await loadDimensionCaches();

const extract = extractSource(
  'SELECT order_date, customer_email, product_sku, quantity, unit_price, discount, region FROM order_items oi JOIN orders o ON oi.order_id = o.id WHERE o.order_date >= $1',
  ['2025-01-01'],
  2000
);

const transform = new DimensionLookupTransform(caches);
const loader = new WarehouseLoader(targetPool, 5000);

// Convert async generator to a Readable stream
const extractStream = new (await import('node:stream')).Readable({
  objectMode: true,
  read() {},
});

(async () => {
  for await (const row of extract) {
    if (!extractStream.push(row)) {
      // wait for drain
      await new Promise((resolve) => extractStream.once('drain', resolve));
    }
  }
  extractStream.push(null);
})();

await pipeline(extractStream, transform, loader);

await sourcePool.end();
await targetPool.end();
```

## Database Change Data Capture (CDC) as Stream

CDC captures row-level changes (inserts, updates, deletes) from a database's write-ahead log and emits them as a stream.

```js
import pg from 'pg';
import { Readable, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool, Client } = pg;

// PostgreSQL logical replication / pg_logical_emit_message approach
// Using a trigger-based CDC simulation with LISTEN/NOTIFY

class PgCdcStream extends Readable {
  constructor(connectionString, slotName) {
    super({ objectMode: true });
    this._connectionString = connectionString;
    this._slotName = slotName;
    this._client = null;
    this._started = false;
  }

  async _read() {
    if (this._started) return;
    this._started = true;

    this._client = new Client(this._connectionString);
    await this._client.connect();

    // Create a replication slot-like notification channel
    await this._client.query(`LISTEN ${this._slotName}`);

    this._client.on('notification', (msg) => {
      if (!msg.payload) return;
      try {
        const event = JSON.parse(msg.payload);
        this.push(event);
      } catch {
        // ignore malformed payloads
      }
    });

    // Keep-alive to prevent connection timeout
    this._keepAlive = setInterval(async () => {
      try {
        await this._client.query('SELECT 1');
      } catch {
        this.destroy(new Error('CDC connection lost'));
      }
    }, 30000);
  }

  async _destroy(err, cb) {
    clearInterval(this._keepAlive);
    if (this._client) {
      await this._client.query(`UNLISTEN ${this._slotName}`).catch(() => {});
      await this._client.end().catch(() => {});
    }
    cb(err);
  }
}

// CDC consumer: apply changes to a target database
class CdcApplier extends Writable {
  constructor(targetPool, table) {
    super({ objectMode: true });
    this._pool = targetPool;
    this._table = table;
    this._stats = { inserts: 0, updates: 0, deletes: 0 };
  }

  async _write(event, _enc, cb) {
    try {
      switch (event.operation) {
        case 'INSERT':
          await this._pool.query(
            `INSERT INTO ${this._table} (${Object.keys(event.data).join(',')}) VALUES (${Object.keys(event.data).map((_, i) => `$${i + 1}`).join(',')}) ON CONFLICT DO NOTHING`,
            Object.values(event.data)
          );
          this._stats.inserts++;
          break;

        case 'UPDATE':
          const sets = Object.keys(event.data)
            .map((k, i) => `${k} = $${i + 1}`)
            .join(', ');
          await this._pool.query(
            `UPDATE ${this._table} SET ${sets} WHERE id = $${Object.keys(event.data).length + 1}`,
            [...Object.values(event.data), event.data.id]
          );
          this._stats.updates++;
          break;

        case 'DELETE':
          await this._pool.query(
            `DELETE FROM ${this._table} WHERE id = $1`,
            [event.data.id]
          );
          this._stats.deletes++;
          break;
      }
      cb();
    } catch (err) {
      cb(err);
    }
  }

  get stats() { return this._stats; }
}

// Source database trigger function (run once on source):
// CREATE OR REPLACE FUNCTION notify_change() RETURNS TRIGGER AS $$
// BEGIN
//   PERFORM pg_notify('cdc_slot', json_build_object(
//     'operation', TG_OP,
//     'table', TG_TABLE_NAME,
//     'data', CASE WHEN TG_OP = 'DELETE' THEN row_to_json(OLD) ELSE row_to_json(NEW) END,
//     'timestamp', extract(epoch from now())
//   )::text);
//   RETURN NEW;
// END;
// $$ LANGUAGE plpgsql;
//
// CREATE TRIGGER orders_cdc
// AFTER INSERT OR UPDATE OR DELETE ON orders
// FOR EACH ROW EXECUTE FUNCTION notify_change();

// Usage
const cdcStream = new PgCdcStream('postgresql://source:5432/app', 'cdc_slot');
const targetPool = new Pool({ connectionString: 'postgresql://target:5432/reporting' });
const applier = new CdcApplier(targetPool, 'orders_mirror');

await pipeline(cdcStream, applier);
```

## Stream-Based Data Synchronization Between Databases

Synchronize two databases by streaming from a source, comparing against the target, and applying diffs.

```js
import pg from 'pg';
import QueryStream from 'pg-query-stream';
import { Transform, Writable } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool } = pg;
const sourcePool = new Pool({ connectionString: 'postgresql://source:5432/primary' });
const targetPool = new Pool({ connectionString: 'postgresql://target:5432/replica' });

// Sync a table by streaming source rows and upserting into target
async function syncTable(table, keyColumn, { batchSize = 2000 } = {}) {
  const client = await sourcePool.connect();
  const startTime = Date.now();
  let synced = 0;

  try {
    const qs = new QueryStream(
      `SELECT * FROM ${table} ORDER BY ${keyColumn}`,
      [],
      { batchSize: 500 }
    );
    const sourceRows = client.query(qs);

    const columns = null; // resolved on first row

    const resolver = new Transform({
      objectMode: true,
      transform(row, _enc, cb) {
        if (!this._columns) {
          this._columns = Object.keys(row);
        }
        this.push(row);
        cb();
      },
    });
    resolver._columns = null;

    let batch = [];

    const upserter = new Writable({
      objectMode: true,
      async write(row, _enc, cb) {
        batch.push(row);
        if (batch.length >= batchSize) {
          try { await flush(); cb(); }
          catch (err) { cb(err); }
        } else { cb(); }
      },
      async final(cb) {
        try { await flush(); cb(); }
        catch (err) { cb(err); }
      },
    });

    async function flush() {
      if (batch.length === 0) return;
      const rows = batch;
      batch = [];

      const cols = resolver._columns;
      const vals = [];
      const ph = [];
      let i = 1;

      for (const r of rows) {
        const placeholders = cols.map((_, j) => `$${i + j}`);
        ph.push(`(${placeholders.join(',')})`);
        vals.push(...cols.map((c) => r[c]));
        i += cols.length;
      }

      const updateSets = cols
        .filter((c) => c !== keyColumn)
        .map((c) => `${c} = EXCLUDED.${c}`)
        .join(', ');

      await targetPool.query(
        `INSERT INTO ${table} (${cols.join(',')})
         VALUES ${ph.join(',')}
         ON CONFLICT (${keyColumn}) DO UPDATE SET ${updateSets}`,
        vals
      );

      synced += rows.length;
      if (synced % 10000 === 0) {
        const elapsed = ((Date.now() - startTime) / 1000).toFixed(1);
        console.log(`Synced ${synced} rows (${elapsed}s)`);
      }
    }

    await pipeline(sourceRows, resolver, upserter);

    const elapsed = ((Date.now() - startTime) / 1000).toFixed(1);
    console.log(`Sync complete: ${synced} rows in ${table} (${elapsed}s)`);
  } finally {
    client.release();
  }
}

// Sync multiple tables
const tables = [
  { name: 'users', key: 'id' },
  { name: 'products', key: 'sku' },
  { name: 'orders', key: 'order_id' },
];

for (const { name, key } of tables) {
  await syncTable(name, key);
}

await sourcePool.end();
await targetPool.end();
```

## Real-World: Zero-Downtime Database Migration Using Streams

A production migration that shifts traffic from an old schema to a new one without downtime, using dual-write and stream replay.

```js
import pg from 'pg';
import { MongoClient } from 'mongodb';
import QueryStream from 'pg-query-stream';
import { Transform, Writable, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const { Pool } = pg;

// === Phase 1: Schema setup on new database ===
const newDb = new Pool({ connectionString: 'postgresql://new-host:5432/app' });
await newDb.query(`
  CREATE TABLE IF NOT EXISTS orders_v2 (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    legacy_id TEXT UNIQUE,
    customer_id BIGINT NOT NULL,
    total NUMERIC(12,2) NOT NULL DEFAULT 0,
    currency TEXT NOT NULL DEFAULT 'USD',
    status TEXT NOT NULL DEFAULT 'pending',
    items JSONB DEFAULT '[]',
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    migrated_at TIMESTAMPTZ DEFAULT NOW()
  )
`);

// === Phase 2: Stream full backfill from old database ===
const oldDb = new Pool({ connectionString: 'postgresql://old-host:5432/legacy' });

async function backfill() {
  console.log('Phase 2: Backfill started');
  const client = await oldDb.connect();
  const startTime = Date.now();
  let count = 0;

  try {
    const qs = new QueryStream(
      'SELECT id, customer_id, total, currency, status, created_at FROM orders ORDER BY id',
      [],
      { batchSize: 3000 }
    );
    const rows = client.query(qs);

    const transform = new Transform({
      objectMode: true,
      transform(row, _enc, cb) {
        count++;
        cb(null, {
          legacy_id: String(row.id),
          customer_id: row.customer_id,
          total: row.total,
          currency: row.currency,
          status: row.status,
          created_at: row.created_at,
        });
      },
    });

    let batch = [];

    const writer = new Writable({
      objectMode: true,
      async write(row, _enc, cb) {
        batch.push(row);
        if (batch.length >= 3000) {
          try { await flush(); cb(); }
          catch (err) { cb(err); }
        } else { cb(); }
      },
      async final(cb) {
        try { await flush(); cb(); }
        catch (err) { cb(err); }
      },
    });

    async function flush() {
      if (batch.length === 0) return;
      const rows = batch;
      batch = [];
      const vals = [];
      const ph = [];
      let i = 1;
      for (const r of rows) {
        ph.push(`($${i},$${i+1},$${i+2},$${i+3},$${i+4},$${i+5})`);
        vals.push(r.legacy_id, r.customer_id, r.total, r.currency, r.status, r.created_at);
        i += 6;
      }
      await newDb.query(
        `INSERT INTO orders_v2 (legacy_id, customer_id, total, currency, status, created_at)
         VALUES ${ph.join(',')}
         ON CONFLICT (legacy_id) DO NOTHING`,
        vals
      );
    }

    await pipeline(rows, transform, writer);
    const elapsed = ((Date.now() - startTime) / 1000).toFixed(1);
    console.log(`Backfill complete: ${count} orders (${elapsed}s)`);
  } finally {
    client.release();
  }
}

// === Phase 3: Replay missed changes from a change log ===
async function replayChanges(sinceTimestamp) {
  console.log(`Phase 3: Replaying changes since ${sinceTimestamp}`);
  const client = await oldDb.connect();

  try {
    const qs = new QueryStream(
      'SELECT * FROM order_changes WHERE changed_at > $1 ORDER BY changed_at',
      [sinceTimestamp],
      { batchSize: 1000 }
    );
    const changes = client.query(qs);

    const applier = new Writable({
      objectMode: true,
      async write(change, _enc, cb) {
        try {
          const row = change.new_data || change.old_data;
          if (change.op === 'DELETE') {
            await newDb.query('DELETE FROM orders_v2 WHERE legacy_id = $1', [String(change.order_id)]);
          } else {
            await newDb.query(
              `INSERT INTO orders_v2 (legacy_id, customer_id, total, currency, status, created_at)
               VALUES ($1,$2,$3,$4,$5,$6)
               ON CONFLICT (legacy_id) DO UPDATE SET
                 total = EXCLUDED.total, status = EXCLUDED.status`,
              [String(change.order_id), row.customer_id, row.total, row.currency, row.status, row.created_at]
            );
          }
          cb();
        } catch (err) { cb(err); }
      },
    });

    await pipeline(changes, applier);
    console.log('Replay complete');
  } finally {
    client.release();
  }
}

// === Phase 4: Dual-write proxy (application layer) ===
// In production, this would be middleware that writes to both databases
async function dualWrite(queryFn) {
  // Write to old database first (source of truth during migration)
  const oldResult = await queryFn(oldDb);

  // Async write to new database (best-effort, errors logged)
  queryFn(newDb).catch((err) => {
    console.error('Dual-write secondary failed:', err.message);
    // Record for replay
  });

  return oldResult;
}

// === Execute migration ===
const migrationStart = new Date();

// Step 1: Backfill existing data
await backfill();

// Step 2: Replay changes that happened during backfill
await replayChanges(migrationStart);

// Step 3: Application switches to dual-write (handled by middleware)
// Step 4: After validation, cut over reads to new database
// Step 5: Stop dual-write, decommission old database

console.log('Migration phases 1-2 complete. Switch application to dual-write mode.');

await oldDb.end();
await newDb.end();
```

## Best Practices Checklist

- [ ] Use `ON CONFLICT DO UPDATE` (upsert) for idempotent stream-based inserts
- [ ] Log row count and elapsed time periodically during long migrations
- [ ] Set `batchSize` on cursors and query streams to balance memory and round-trips
- [ ] Use `ON CONFLICT DO NOTHING` during initial backfill to skip duplicates safely
- [ ] Implement a change log table for replay-based catch-up after backfill
- [ ] Apply dual-write only after backfill completes and replay catches up
- [ ] Validate row counts between source and target after migration
- [ ] Handle connection failures with automatic retry and exponential backoff
- [ ] Use transactions or `SAVEPOINT` for per-batch error isolation
- [ ] Keep migration scripts resumable by tracking high-water marks
- [ ] Test migrations with production-scale data volumes before running in production
- [ ] Monitor connection pool metrics during ETL to detect exhaustion early
- [ ] Use `COPY TO/FROM` for bulk data transfer when row-level transforms aren't needed
- [ ] Drain all streams and close connections in `finally` blocks to prevent leaks

## Cross-References

- [MongoDB and PostgreSQL Streaming](01-mongodb-postgresql-streaming.md) — cursor streaming, bulk writes, COPY commands
- [Redis and Cloud Storage Streaming](02-redis-s3-storage-streams.md) — caching and backup pipelines
- [Stream Error Handling](../07-stream-error-handling/) — retry, circuit-breaker, and dead-letter patterns
- [Stream Concurrency](../06-stream-concurrency-parallelism/) — parallelizing migration and ETL workers
- [Stream Performance](../08-stream-performance-optimization/) — tuning batch sizes and watermarks
- [Stream Testing](../11-stream-testing/) — unit testing ETL pipeline stages

## Next Steps

- Review **Stream Error Handling** (`../07-stream-error-handling/`) for building fault-tolerant migration pipelines
- Study **Stream Concurrency** (`../06-stream-concurrency-parallelism/`) for parallelizing ETL workers
- Explore **Stream Testing** (`../11-stream-testing/`) for testing each pipeline stage in isolation
