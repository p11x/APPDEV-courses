# Batch Operations and Bulk Updates

## What You'll Learn

- Bulk insert strategies across databases
- Batch update patterns
- Upsert operations
- Stream-based bulk processing
- Performance optimization for bulk operations

## PostgreSQL Bulk Operations

```javascript
import { Pool } from 'pg';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';
import copyFrom from 'pg-copy-streams';

const pool = new Pool();

// Multi-row INSERT
async function bulkInsert(pool, table, records, columns) {
    if (records.length === 0) return { inserted: 0 };

    const colList = columns.join(', ');
    const placeholders = records.map((_, rowIdx) => {
        const start = rowIdx * columns.length + 1;
        const params = columns.map((_, colIdx) => `$${start + colIdx}`);
        return `(${params.join(', ')})`;
    }).join(', ');

    const values = records.flatMap(record => columns.map(col => record[col]));

    const { rowCount } = await pool.query(
        `INSERT INTO ${table} (${colList}) VALUES ${placeholders}`,
        values
    );

    return { inserted: rowCount };
}

// Batch processing with configurable batch size
async function bulkInsertBatched(pool, table, records, columns, batchSize = 1000) {
    let totalInserted = 0;

    for (let i = 0; i < records.length; i += batchSize) {
        const batch = records.slice(i, i + batchSize);
        const result = await bulkInsert(pool, table, batch, columns);
        totalInserted += result.inserted;
        console.log(`Inserted batch: ${totalInserted}/${records.length}`);
    }

    return { inserted: totalInserted };
}

// COPY FROM (fastest for PostgreSQL)
async function copyFromCSV(pool, table, csvStream) {
    const client = await pool.connect();
    try {
        const copyStream = client.query(
            copyFrom.from(`COPY ${table} FROM STDIN WITH CSV HEADER`)
        );
        await pipeline(csvStream, copyStream);
    } finally {
        client.release();
    }
}
```

## Bulk Update Patterns

```javascript
// UPDATE with VALUES (PostgreSQL)
async function bulkUpdate(pool, table, records, keyColumn, updateColumns) {
    const colList = updateColumns.join(', ');
    const allCols = [keyColumn, ...updateColumns];

    const values = records.map((_, rowIdx) => {
        const start = rowIdx * allCols.length + 1;
        return `(${allCols.map((_, ci) => `$${start + ci}`).join(', ')})`;
    }).join(', ');

    const setClause = updateColumns.map(col => `${col} = v.${col}`).join(', ');
    const params = records.flatMap(r => allCols.map(c => r[c]));

    const { rowCount } = await pool.query(
        `UPDATE ${table} SET ${setClause}
         FROM (VALUES ${values}) AS v(${allCols.join(', ')})
         WHERE ${table}.${keyColumn} = v.${keyColumn}`,
        params
    );

    return { updated: rowCount };
}

// Batch updates with individual queries (transaction)
async function batchUpdateInTransaction(pool, updates, buildQuery) {
    const client = await pool.connect();
    let updated = 0;

    try {
        await client.query('BEGIN');

        for (const update of updates) {
            const { query, params } = buildQuery(update);
            const { rowCount } = await client.query(query, params);
            updated += rowCount;
        }

        await client.query('COMMIT');
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }

    return { updated };
}
```

## MongoDB Bulk Operations

```javascript
import mongoose from 'mongoose';

const User = mongoose.model('User', new mongoose.Schema({ /* ... */ }));

// bulkWrite with mixed operations
async function mongoBulkOperations(operations) {
    const result = await User.bulkWrite([
        { insertOne: { document: { name: 'Alice', email: 'alice@example.com' } } },
        { insertOne: { document: { name: 'Bob', email: 'bob@example.com' } } },
        { updateOne: { filter: { email: 'charlie@example.com' }, update: { $set: { role: 'admin' } } } },
        { updateMany: { filter: { role: 'user' }, update: { $inc: { loginCount: 1 } } } },
        { deleteOne: { filter: { email: 'delete@example.com' } } },
        { replaceOne: { filter: { email: 'replace@example.com' }, replacement: { name: 'New Name' } } },
    ], { ordered: false }); // Continue on error

    return {
        insertedCount: result.insertedCount,
        matchedCount: result.matchedCount,
        modifiedCount: result.modifiedCount,
        deletedCount: result.deletedCount,
    };
}

// Stream-based bulk insert
async function mongoStreamInsert(records) {
    const BATCH_SIZE = 1000;
    let batch = [];
    let totalInserted = 0;

    for (const record of records) {
        batch.push(record);
        if (batch.length >= BATCH_SIZE) {
            await User.insertMany(batch, { ordered: false });
            totalInserted += batch.length;
            batch = [];
        }
    }

    if (batch.length > 0) {
        await User.insertMany(batch, { ordered: false });
        totalInserted += batch.length;
    }

    return { inserted: totalInserted };
}
```

## MySQL Bulk Operations

```javascript
import mysql from 'mysql2/promise';

const pool = mysql.createPool({ /* config */ });

// Bulk insert with query (multiple VALUES)
async function mysqlBulkInsert(records) {
    const values = records.map(r => [r.name, r.email, r.age]);
    const [result] = await pool.query(
        'INSERT INTO users (name, email, age) VALUES ?',
        [values]
    );
    return { inserted: result.affectedRows };
}

// LOAD DATA LOCAL INFILE (fastest for MySQL)
async function mysqlLoadCSV(filePath, table) {
    const [result] = await pool.query(`
        LOAD DATA LOCAL INFILE ?
        INTO TABLE ${table}
        FIELDS TERMINATED BY ','
        ENCLOSED BY '"'
        LINES TERMINATED BY '\\n'
        IGNORE 1 ROWS
    `, [filePath]);
    return { loaded: result.affectedRows };
}

// INSERT ... ON DUPLICATE KEY UPDATE (upsert)
async function mysqlUpsert(records) {
    const values = records.map(r => [r.id, r.name, r.email, r.count]);
    const [result] = await pool.query(`
        INSERT INTO users (id, name, email, count) VALUES ?
        ON DUPLICATE KEY UPDATE 
            name = VALUES(name),
            email = VALUES(email),
            count = count + VALUES(count)
    `, [values]);
    return {
        affected: result.affectedRows,
        changed: result.changedRows,
    };
}
```

## Streaming Bulk Processor

```javascript
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createReadStream } from 'node:fs';

class BulkProcessor extends Transform {
    constructor(pool, options = {}) {
        super({ objectMode: true });
        this.pool = pool;
        this.batchSize = options.batchSize || 1000;
        this.table = options.table;
        this.columns = options.columns;
        this.batch = [];
        this.totalProcessed = 0;
    }

    _transform(record, encoding, callback) {
        this.batch.push(record);
        if (this.batch.length >= this.batchSize) {
            this.flushBatch().then(() => callback()).catch(callback);
        } else {
            callback();
        }
    }

    _flush(callback) {
        if (this.batch.length > 0) {
            this.flushBatch().then(() => callback()).catch(callback);
        } else {
            callback();
        }
    }

    async flushBatch() {
        const batch = this.batch;
        this.batch = [];

        const placeholders = batch.map((_, rowIdx) => {
            const start = rowIdx * this.columns.length + 1;
            return `(${this.columns.map((_, ci) => `$${start + ci}`).join(', ')})`;
        }).join(', ');

        const values = batch.flatMap(r => this.columns.map(c => r[c]));

        await this.pool.query(
            `INSERT INTO ${this.table} (${this.columns.join(', ')}) VALUES ${placeholders}`,
            values
        );

        this.totalProcessed += batch.length;
        this.push({ batchInserted: batch.length, totalProcessed: this.totalProcessed });
    }
}

// Usage
await pipeline(
    createReadStream('./data.jsonl')
        .pipe(new LineParser()),
    new BulkProcessor(pool, {
        table: 'users',
        columns: ['name', 'email', 'age'],
        batchSize: 500,
    }),
    new ProgressReporter(),
);
```

## Performance Benchmarks

```
Bulk Insert Performance (10,000 records):
─────────────────────────────────────────────
Method                   PostgreSQL   MySQL      MongoDB
─────────────────────────────────────────────
Individual inserts       8.5s         9.2s       6.1s
Bulk INSERT (100/batch)  1.2s         1.4s       0.9s
Bulk INSERT (1000/batch) 0.6s         0.7s       0.5s
COPY/LOAD DATA           0.3s         0.4s       N/A
insertMany               N/A          N/A        0.4s

Optimal batch sizes:
├── PostgreSQL: 1,000-5,000 rows per INSERT
├── MySQL: 500-2,000 rows per INSERT
├── MongoDB: 1,000-10,000 documents per insertMany
└── Rule of thumb: Keep batch size under 16MB
```

## Best Practices Checklist

- [ ] Use bulk INSERT instead of individual inserts
- [ ] Use COPY (PostgreSQL) or LOAD DATA (MySQL) for massive imports
- [ ] Process in batches to avoid memory exhaustion
- [ ] Use transactions for atomicity in batch operations
- [ ] Use `ordered: false` for MongoDB bulk operations
- [ ] Monitor batch size to stay under database limits
- [ ] Log progress for long-running bulk operations
- [ ] Implement retry for failed batches

## Cross-References

- See [Query Optimization](./01-query-optimization.md) for query tuning
- See [Streaming Data](../06-data-processing-transformation/01-streaming-data.md) for stream processing
- See [Transaction Management](../01-database-integration-patterns/05-transaction-management.md) for transactions

## Next Steps

Continue to [Read Replicas and Sharding](./05-read-replicas-sharding.md) for scaling database reads.
