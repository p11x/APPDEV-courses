# Streaming Data Processing with Node.js

## What You'll Learn

- Large dataset processing with streams
- ETL pipeline implementation
- Data transformation patterns
- Real-time data processing

## Streaming Large Datasets

```javascript
import { createReadStream } from 'node:fs';
import { Transform, pipeline } from 'node:stream';
import { createInterface } from 'node:readline';

// Process large CSV file line by line
async function processLargeFile(filePath, processLine) {
    const stream = createReadStream(filePath, { encoding: 'utf-8' });
    const rl = createInterface({ input: stream, crlfDelay: Infinity });

    let lineCount = 0;
    for await (const line of rl) {
        if (lineCount++ === 0) continue; // Skip header
        await processLine(line);
    }

    return lineCount;
}

// Usage
await processLargeFile('./data.csv', async (line) => {
    const [name, email, age] = line.split(',');
    await db.query('INSERT INTO users (name, email, age) VALUES ($1, $2, $3)',
        [name, email, parseInt(age)]);
});
```

## ETL Pipeline

```javascript
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

// Parse CSV
const csvParser = new Transform({
    objectMode: true,
    transform(chunk, encoding, callback) {
        const lines = chunk.toString().split('\n');
        for (const line of lines) {
            if (line.trim()) {
                const [name, email, age] = line.split(',');
                this.push({ name, email, age: parseInt(age) });
            }
        }
        callback();
    }
});

// Validate
const validate = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        if (record.name && record.email?.includes('@')) {
            callback(null, record);
        } else {
            callback(); // Skip invalid
        }
    }
});

// Enrich
const enrich = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        callback(null, {
            ...record,
            id: Math.random().toString(36).slice(2),
            createdAt: new Date().toISOString(),
        });
    }
});

// Run pipeline
await pipeline(
    createReadStream('./input.csv'),
    csvParser,
    validate,
    enrich,
    createWriteStream('./output.jsonl'),
);
```

## Best Practices Checklist

- [ ] Use streams for large datasets
- [ ] Process data in chunks
- [ ] Implement error handling in pipelines
- [ ] Use objectMode for non-binary data
- [ ] Monitor memory usage during processing

## Cross-References

- See [Database Performance](../02-database-performance-optimization/01-query-optimization.md) for queries
- See [Caching](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching
- See [Scalability](../05-scalability-patterns/01-load-balancing.md) for scaling

## Next Steps

Continue to [Database Security](../07-database-security-implementation/01-connection-security.md).
