# ETL Patterns and Implementation

## What You'll Learn

- ETL pipeline architecture
- Extract, Transform, Load stages
- Error handling in ETL
- Incremental ETL patterns
- Parallel ETL processing

## ETL Pipeline Framework

```javascript
import { pipeline } from 'node:stream/promises';
import { Transform, Readable } from 'node:stream';

class ETLPipeline {
    constructor(options = {}) {
        this.batchSize = options.batchSize || 1000;
        this.maxConcurrency = options.maxConcurrency || 5;
        this.errorHandler = options.errorHandler || console.error;
    }

    async extract(source) {
        return source.read();
    }

    async transform(records, transformers) {
        let result = records;
        for (const transformer of transformers) {
            result = await transformer(result);
        }
        return result;
    }

    async load(records, destination) {
        return destination.write(records);
    }

    async run(source, transformers, destination) {
        let totalProcessed = 0;
        let totalErrors = 0;

        const records = await this.extract(source);

        for (let i = 0; i < records.length; i += this.batchSize) {
            const batch = records.slice(i, i + this.batchSize);

            try {
                const transformed = await this.transform(batch, transformers);
                await this.load(transformed, destination);
                totalProcessed += transformed.length;
            } catch (err) {
                totalErrors += batch.length;
                this.errorHandler(err, batch);
            }
        }

        return { totalProcessed, totalErrors };
    }
}
```

## Database ETL Source

```javascript
class DatabaseETLSource {
    constructor(pool, options = {}) {
        this.pool = pool;
        this.table = options.table;
        this.query = options.query;
        this.chunkSize = options.chunkSize || 5000;
        this.orderColumn = options.orderColumn || 'id';
    }

    async *read() {
        let lastValue = 0;
        let hasMore = true;

        while (hasMore) {
            const sql = this.query ||
                `SELECT * FROM ${this.table} WHERE ${this.orderColumn} > $1 ORDER BY ${this.orderColumn} LIMIT $2`;

            const { rows } = await this.pool.query(sql, [lastValue, this.chunkSize]);

            if (rows.length === 0) {
                hasMore = false;
                break;
            }

            yield rows;
            lastValue = rows[rows.length - 1][this.orderColumn];
        }
    }
}
```

## Streaming ETL with Backpressure

```javascript
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

function createETLStream(options = {}) {
    const batchSize = options.batchSize || 100;

    // Extract → batch
    const batcher = new Transform({
        objectMode: true,
        transform(record, encoding, callback) {
            if (!this.buffer) this.buffer = [];
            this.buffer.push(record);

            if (this.buffer.length >= batchSize) {
                this.push(this.buffer);
                this.buffer = [];
            }
            callback();
        },
        flush(callback) {
            if (this.buffer?.length > 0) {
                this.push(this.buffer);
            }
            callback();
        },
    });

    // Transform
    const transformer = new Transform({
        objectMode: true,
        async transform(batch, encoding, callback) {
            try {
                const transformed = batch.map(record => ({
                    ...record,
                    processedAt: new Date().toISOString(),
                    hash: createHash('md5').update(JSON.stringify(record)).digest('hex'),
                }));
                callback(null, transformed);
            } catch (err) {
                callback(err);
            }
        },
    });

    // Load
    const loader = new Transform({
        objectMode: true,
        async transform(batch, encoding, callback) {
            try {
                await this.destination.write(batch);
                this.push({ loaded: batch.length });
                callback();
            } catch (err) {
                callback(err);
            }
        },
    });

    return { batcher, transformer, loader };
}
```

## Incremental ETL

```javascript
class IncrementalETL {
    constructor(sourcePool, targetPool, options = {}) {
        this.source = sourcePool;
        this.target = targetPool;
        this.watermarkTable = options.watermarkTable || 'etl_watermarks';
    }

    async init() {
        await this.target.query(`
            CREATE TABLE IF NOT EXISTS ${this.watermarkTable} (
                pipeline_name VARCHAR(255) PRIMARY KEY,
                last_value TEXT,
                last_run TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        `);
    }

    async getWatermark(pipelineName) {
        const { rows } = await this.target.query(
            `SELECT last_value FROM ${this.watermarkTable} WHERE pipeline_name = $1`,
            [pipelineName]
        );
        return rows[0]?.last_value || '0';
    }

    async setWatermark(pipelineName, value) {
        await this.target.query(
            `INSERT INTO ${this.watermarkTable} (pipeline_name, last_value, last_run)
             VALUES ($1, $2, NOW())
             ON CONFLICT (pipeline_name) DO UPDATE SET last_value = $2, last_run = NOW()`,
            [pipelineName, value]
        );
    }

    async run(pipelineName, extractSQL, transformFn, loadFn) {
        const watermark = await this.getWatermark(pipelineName);
        let totalProcessed = 0;

        const { rows } = await this.source.query(extractSQL, [watermark]);

        if (rows.length === 0) return { processed: 0 };

        const transformed = transformFn ? await transformFn(rows) : rows;
        await loadFn(transformed);

        const newWatermark = rows[rows.length - 1].id || rows[rows.length - 1].updated_at;
        await this.setWatermark(pipelineName, String(newWatermark));

        return { processed: transformed.length, watermark: newWatermark };
    }
}
```

## Best Practices Checklist

- [ ] Use streaming for large datasets
- [ ] Implement incremental ETL with watermarks
- [ ] Handle errors without stopping the pipeline
- [ ] Use batch operations for database writes
- [ ] Log ETL metrics (processed, errors, duration)
- [ ] Implement idempotent ETL operations
- [ ] Use transactions for atomic batch loads

## Cross-References

- See [Streaming Data](./01-streaming-data.md) for stream processing
- See [Bulk Operations](../02-database-performance-optimization/04-batch-operations-bulk.md) for batch ops
- See [Data Validation](./03-data-validation.md) for validation patterns

## Next Steps

Continue to [Data Validation](./03-data-validation.md) for data quality patterns.
