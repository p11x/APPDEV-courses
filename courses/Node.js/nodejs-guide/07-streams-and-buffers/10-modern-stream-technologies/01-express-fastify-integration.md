# Modern Stream Technologies and Integration

## What You'll Learn

- Express and Fastify stream integration
- Database streaming (PostgreSQL, MongoDB)
- Redis stream integration
- Message queue streaming
- Cloud storage streaming (S3)

## Express Streaming

```javascript
import express from 'express';
import { createReadStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';

const app = express();

// Stream large JSON responses
app.get('/api/users/stream', async (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.write('[');

    let first = true;
    const cursor = db.collection('users').find().stream();

    cursor.on('data', (user) => {
        if (!first) res.write(',');
        res.write(JSON.stringify(user));
        first = false;
    });

    cursor.on('end', () => {
        res.end(']');
    });

    cursor.on('error', (err) => {
        if (!res.headersSent) {
            res.status(500).json({ error: err.message });
        }
    });
});

// Stream file download with progress
app.get('/download/:file', async (req, res) => {
    const filePath = `./files/${req.params.file}`;
    const { statSync } = await import('node:fs');
    const stats = statSync(filePath);

    res.setHeader('Content-Length', stats.size);
    res.setHeader('Content-Type', 'application/octet-stream');

    const progress = new Transform({
        transform(chunk, encoding, callback) {
            this.transferred = (this.transferred || 0) + chunk.length;
            callback(null, chunk);
        }
    });

    await pipeline(
        createReadStream(filePath),
        progress,
        res
    );
});
```

## PostgreSQL Streaming

```javascript
import { Pool } from 'pg';
import { Readable } from 'node:stream';

const pool = new Pool();

// Stream query results
function streamQuery(sql, params = []) {
    return new Readable({
        objectMode: true,
        async read() {
            if (!this.client) {
                this.client = await pool.connect();
                this.cursor = this.client.query(
                    new (await import('pg-cursor')).default(sql, params)
                );
            }

            const rows = await this.cursor.read(100); // Batch of 100
            if (rows.length === 0) {
                this.push(null);
                this.client.release();
            } else {
                for (const row of rows) {
                    this.push(row);
                }
            }
        }
    });
}

// Process millions of rows with constant memory
async function processAllUsers() {
    const stream = streamQuery('SELECT * FROM users ORDER BY id');

    for await (const user of stream) {
        await processUser(user);
    }
}
```

## MongoDB Streaming

```javascript
import { MongoClient } from 'mongodb';

const client = new MongoClient(process.env.MONGODB_URL);
const db = client.db('myapp');

// MongoDB cursor as stream
function mongoStream(collectionName, query = {}) {
    const collection = db.collection(collectionName);
    const cursor = collection.find(query).stream();

    return cursor; // Already a readable stream
}

// Usage
const users = mongoStream('users', { active: true });

for await (const user of users) {
    await processUser(user);
}
```

## Redis Stream Integration

```javascript
import { createClient } from 'redis';

const redis = createClient();
await redis.connect();

// Read from Redis Stream
async function* readRedisStream(streamName, lastId = '0') {
    while (true) {
        const result = await redis.xRead(
            { key: streamName, id: lastId },
            { COUNT: 100, BLOCK: 5000 }
        );

        if (!result || result.length === 0) continue;

        for (const message of result[0].messages) {
            lastId = message.id;
            yield { id: message.id, data: message.message };
        }
    }
}

// Write to Redis Stream
async function writeToRedisStream(streamName, data) {
    await redis.xAdd(streamName, '*', data);
}

// Stream processing loop
for await (const { id, data } of readRedisStream('events')) {
    console.log(`Processing event ${id}:`, data);
    await processEvent(data);
}
```

## S3 Streaming

```javascript
import { S3Client, GetObjectCommand, PutObjectCommand } from '@aws-sdk/client-s3';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip, createGunzip } from 'node:zlib';

const s3 = new S3Client({ region: 'us-east-1' });

// Download from S3 with streaming
async function downloadFromS3(bucket, key, localPath) {
    const command = new GetObjectCommand({ Bucket: bucket, Key: key });
    const response = await s3.send(command);

    await pipeline(
        response.Body, // S3 response body is a readable stream
        createGunzip(),
        createWriteStream(localPath)
    );
}

// Upload to S3 with streaming
async function uploadToS3(localPath, bucket, key) {
    const { statSync } = await import('node:fs');
    const stats = statSync(localPath);

    const command = new PutObjectCommand({
        Bucket: bucket,
        Key: key,
        Body: createReadStream(localPath).pipe(createGzip()),
        ContentLength: stats.size,
    });

    await s3.send(command);
}
```

## Best Practices Checklist

- [ ] Use streaming for large database result sets
- [ ] Set appropriate batch sizes for database cursors
- [ ] Use `pipeline()` for cloud storage operations
- [ ] Handle backpressure in cross-service streams
- [ ] Monitor stream throughput across integrations
- [ ] Implement reconnection for long-running streams

## Cross-References

- See [HTTP Streaming](../05-stream-network-operations/01-http-streaming.md) for HTTP patterns
- See [Pipeline](../01-streams-architecture/01-duplex-passthrough-pipeline.md) for pipeline basics
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for error patterns

## Next Steps

Continue to [Stream Testing](../11-stream-testing/01-unit-testing.md).
