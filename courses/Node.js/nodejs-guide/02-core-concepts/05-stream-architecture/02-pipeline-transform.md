# Stream Pipelines and Transform Streams

## What You'll Learn

- Pipeline chaining with error handling
- Creating custom transform streams
- Composing stream processing chains
- Error recovery in pipelines

## Stream Pipeline

```javascript
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip, createGunzip } from 'node:zlib';

// Simple pipeline with error handling
await pipeline(
    createReadStream('input.txt'),
    createGzip(),
    createWriteStream('input.txt.gz'),
);
console.log('Compression complete');

// Pipeline with error handling
try {
    await pipeline(
        createReadStream('input.txt'),
        createGzip(),
        createWriteStream('output.gz'),
    );
} catch (err) {
    console.error('Pipeline failed:', err.message);
}

// Multi-stage pipeline
await pipeline(
    createReadStream('data.csv'),
    csvParser(),        // Transform: CSV → objects
    dataEnricher(),     // Transform: enrich with API data
    jsonFormatter(),    // Transform: objects → JSON lines
    createGzip(),       // Transform: compress
    createWriteStream('output.json.gz'),
);
```

## Transform Streams

```javascript
import { Transform } from 'node:stream';

// Simple transform: uppercase
const toUpperCase = new Transform({
    transform(chunk, encoding, callback) {
        this.push(chunk.toString().toUpperCase());
        callback();
    }
});

// Object-mode transform: parse JSON lines
const jsonParser = new Transform({
    objectMode: true,
    transform(chunk, encoding, callback) {
        try {
            const lines = chunk.toString().split('\n').filter(Boolean);
            for (const line of lines) {
                this.push(JSON.parse(line));
            }
            callback();
        } catch (err) {
            callback(err);
        }
    }
});

// Transform with state
class LineCounter extends Transform {
    constructor() {
        super({ encoding: 'utf-8' });
        this.lineCount = 0;
    }

    _transform(chunk, encoding, callback) {
        const lines = chunk.toString().split('\n');
        this.lineCount += lines.length - 1;
        this.push(chunk);
        callback();
    }

    _flush(callback) {
        this.push(`\nTotal lines: ${this.lineCount}\n`);
        callback();
    }
}

// Usage
await pipeline(
    createReadStream('./server.log'),
    new LineCounter(),
    createWriteStream('./line-counted.log'),
);
```

## Practical ETL Pipeline

```javascript
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';
import { createReadStream, createWriteStream } from 'node:fs';

// Parse CSV rows
const csvParser = () => new Transform({
    objectMode: true,
    transform(chunk, encoding, callback) {
        const lines = chunk.toString().split('\n');
        for (const line of lines) {
            if (!line.trim()) continue;
            const [name, email, age] = line.split(',');
            this.push({ name, email, age: parseInt(age) });
        }
        callback();
    }
});

// Validate and filter
const validate = () => new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        if (record.name && record.email?.includes('@')) {
            callback(null, record);
        } else {
            callback(); // Skip invalid records
        }
    }
});

// Enrich with computed fields
const enrich = () => new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        callback(null, {
            ...record,
            id: Math.random().toString(36).slice(2),
            createdAt: new Date().toISOString(),
            isAdult: record.age >= 18,
        });
    }
});

// Format as JSON lines
const toJsonLines = () => new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        callback(null, JSON.stringify(record) + '\n');
    }
});

// Run the pipeline
await pipeline(
    createReadStream('users.csv'),
    csvParser(),
    validate(),
    enrich(),
    toJsonLines(),
    createWriteStream('users.jsonl'),
);
console.log('ETL pipeline complete');
```

## Best Practices Checklist

- [ ] Always use `pipeline()` instead of `.pipe()` for error handling
- [ ] Handle errors in transform `_transform` callbacks
- [ ] Use `objectMode` for non-binary data processing
- [ ] Implement `_flush()` for cleanup in transforms
- [ ] Test stream pipelines with large data sets

## Cross-References

- See [Readable/Writable](./01-readable-writable-streams.md) for stream basics
- See [Advanced Patterns](./03-stream-advanced-patterns.md) for complex patterns
- See [Error Handling](../11-error-handling/01-error-propagation.md) for error patterns

## Next Steps

Continue to [Advanced Stream Patterns](./03-stream-advanced-patterns.md) for complex streaming.
