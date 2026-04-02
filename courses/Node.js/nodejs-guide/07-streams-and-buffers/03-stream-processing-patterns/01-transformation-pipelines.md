# Stream Transformation Pipelines

## What You'll Learn

- Building data transformation pipelines
- Chaining multiple transforms
- Pipeline error recovery
- Pipeline monitoring and metrics
- Real-world pipeline patterns

## Pipeline Architecture

```javascript
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

// CSV → Parse → Validate → Transform → JSON
const csvParser = new Transform({
    objectMode: true,
    transform(chunk, encoding, callback) {
        const lines = chunk.toString().split('\n');
        const headers = this.headers;
        
        for (const line of lines) {
            if (!headers) {
                this.headers = line.split(',').map(h => h.trim());
                continue;
            }
            if (!line.trim()) continue;
            
            const values = line.split(',');
            const record = {};
            headers.forEach((h, i) => record[h] = values[i]?.trim());
            this.push(record);
        }
        callback();
    }
});

const validator = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        if (!record.email || !record.email.includes('@')) {
            this.emit('invalid', record);
            return callback();
        }
        callback(null, record);
    }
});

const enricher = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        callback(null, {
            ...record,
            processedAt: new Date().toISOString(),
            emailLower: record.email.toLowerCase(),
        });
    }
});

const jsonSerializer = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        callback(null, JSON.stringify(record) + '\n');
    }
});

validator.on('invalid', (record) => {
    console.warn('Invalid record:', record);
});

await pipeline(
    createReadStream('users.csv'),
    csvParser,
    validator,
    enricher,
    jsonSerializer,
    createWriteStream('users.jsonl')
);
```

## Pipeline with Error Recovery

```javascript
class RecoverablePipeline {
    constructor(stages) {
        this.stages = stages;
        this.errorCount = 0;
        this.maxErrors = 100;
    }

    async run(input, output) {
        const { Transform } = await import('node:stream');
        const { pipeline } = await import('node:stream/promises');

        // Wrap each stage with error handling
        const wrappedStages = this.stages.map(stage => {
            return new Transform({
                objectMode: true,
                transform(chunk, encoding, callback) {
                    try {
                        const result = stage(chunk);
                        if (result !== undefined) callback(null, result);
                        else callback();
                    } catch (err) {
                        this.errorCount++;
                        if (this.errorCount > this.maxErrors) {
                            callback(new Error(`Max errors (${this.maxErrors}) exceeded`));
                        } else {
                            console.error('Stage error:', err.message);
                            callback(); // Skip failed record
                        }
                    }
                }
            });
        });

        await pipeline(input, ...wrappedStages, output);
        return { errors: this.errorCount };
    }
}
```

## Streaming File Processor

```javascript
import { createReadStream } from 'node:fs';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createGzip } from 'node:zlib';

// Process and compress large log files
const logParser = new Transform({
    transform(chunk, encoding, callback) {
        const lines = chunk.toString().split('\n');
        for (const line of lines) {
            const match = line.match(/(\d{4}-\d{2}-\d{2}) (.+?) (.+)/);
            if (match) {
                this.push(JSON.stringify({
                    date: match[1],
                    level: match[2],
                    message: match[3],
                }) + '\n');
            }
        }
        callback();
    }
});

await pipeline(
    createReadStream('server.log'),
    logParser,
    createGzip(),
    createWriteStream('parsed-logs.jsonl.gz')
);
```

## Best Practices Checklist

- [ ] Use `pipeline()` with async error handling
- [ ] Implement error recovery in long-running pipelines
- [ ] Monitor throughput and memory usage
- [ ] Use objectMode for structured data pipelines
- [ ] Log pipeline metrics for debugging
- [ ] Test pipelines with realistic data volumes

## Cross-References

- See [Duplex and Pipeline](../01-streams-architecture/01-duplex-passthrough-pipeline.md) for pipeline basics
- See [Transform Streams](../streams/04-transform-streams.md) for transform patterns
- See [File Processing](./02-file-processing-streams.md) for file pipelines

## Next Steps

Continue to [File Processing Streams](./02-file-processing-streams.md).
