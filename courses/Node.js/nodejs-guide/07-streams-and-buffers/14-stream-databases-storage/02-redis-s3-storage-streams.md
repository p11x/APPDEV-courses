# Redis, S3, and Cloud Storage Streaming

## What You'll Learn

- Scanning Redis keys and hashes with SCAN/HSCAN as Readable streams
- Batching Redis commands using pipeline and stream operations
- Using Redis Streams (XADD/XREAD/XREADGROUP) for persistent event streaming
- Uploading to AWS S3 with `Upload` from `@aws-sdk/lib-storage`
- Downloading S3 objects as Readable streams via `GetObject`
- Implementing multipart uploads through streams for large files
- Streaming data to/from Google Cloud Storage
- Streaming data to/from Azure Blob Storage
- Building a real-world compressed and encrypted S3 backup pipeline

## Redis SCAN/HSCAN as Readable Stream

Scanning large keyspaces with `SCAN` requires cursor-based iteration. A Readable stream wraps the pattern, yielding keys without blocking.

```js
import { createClient } from 'redis';
import { Readable } from 'node:stream';

const redis = createClient({ url: 'redis://localhost:6379' });
await redis.connect();

class RedisScanStream extends Readable {
  constructor(client, pattern = '*', count = 100) {
    super({ objectMode: true });
    this._client = client;
    this._pattern = pattern;
    this._count = count;
    this._cursor = '0';
  }

  async _read() {
    if (this._cursor === null) return; // already done

    try {
      const [cursor, keys] = await this._client.scan(this._cursor, {
        MATCH: this._pattern,
        COUNT: this._count,
      });

      for (const key of keys) {
        if (!this.push(key)) {
          // consumer signaled backpressure — pause by returning
          // we store cursor so next _read picks up
          this._cursor = cursor;
          if (cursor === '0') this._cursor = null;
          return;
        }
      }

      if (cursor === '0') {
        this._cursor = null;
        this.push(null); // end
      } else {
        this._cursor = cursor;
      }
    } catch (err) {
      this.destroy(err);
    }
  }
}

// Usage: find all user session keys
const scanStream = new RedisScanStream(redis, 'session:*', 500);

scanStream.on('data', (key) => {
  console.log(key);
});

scanStream.on('end', () => {
  console.log('Scan complete');
  redis.disconnect();
});
```

### HSCAN Stream for Hash Fields

```js
import { Readable } from 'node:stream';

class RedisHScanStream extends Readable {
  constructor(client, key, count = 100) {
    super({ objectMode: true });
    this._client = client;
    this._key = key;
    this._count = count;
    this._cursor = '0';
  }

  async _read() {
    if (this._cursor === null) return;

    try {
      const [cursor, tuples] = await this._client.hScan(this._key, this._cursor, {
        COUNT: this._count,
      });

      for (let i = 0; i < tuples.length; i++) {
        const entry = { field: tuples[i].field, value: tuples[i].value };
        if (!this.push(entry)) {
          this._cursor = cursor === '0' ? null : cursor;
          return;
        }
      }

      if (cursor === '0') {
        this._cursor = null;
        this.push(null);
      } else {
        this._cursor = cursor;
      }
    } catch (err) {
      this.destroy(err);
    }
  }
}

// Usage: scan a large hash
const hscanStream = new RedisHScanStream(redis, 'user:1001:preferences', 200);
hscanStream.on('data', ({ field, value }) => {
  console.log(`${field} = ${value}`);
});
```

## Redis Pipeline and Stream Operations

Pipelines batch commands to reduce round-trips. Combined with streams, they enable high-throughput bulk operations.

```js
import { createClient } from 'redis';
import { pipeline } from 'node:stream/promises';
import { Transform, Writable } from 'node:stream';
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';

const redis = createClient({ url: 'redis://localhost:6379' });
await redis.connect();

// Stream JSON lines from a file and pipeline into Redis hashes
const BATCH_SIZE = 500;

class RedisPipelineWriter extends Writable {
  constructor(client, batchSize = 500) {
    super({ objectMode: true });
    this._client = client;
    this._batchSize = batchSize;
    this._batch = [];
    this._count = 0;
  }

  async _write(record, _enc, cb) {
    this._batch.push(record);
    if (this._batch.length >= this._batchSize) {
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
    if (this._batch.length === 0) return;
    const pipe = this._client.multi();
    for (const rec of this._batch) {
      pipe.hSet(`user:${rec.id}`, {
        name: rec.name,
        email: rec.email,
        status: rec.status,
      });
      pipe.expire(`user:${rec.id}`, 86400);
    }
    await pipe.exec();
    this._count += this._batch.length;
    this._batch = [];
  }

  async _final(cb) {
    try {
      await this._flush();
      console.log(`Pipeline wrote ${this._count} records`);
      cb();
    } catch (err) {
      cb(err);
    }
  }
}

const input = createReadStream('./users.jsonl');
const lines = createInterface({ input, crlfDelay: Infinity });

const parser = new Transform({
  objectMode: true,
  transform(line, _enc, cb) {
    if (line.trim()) {
      try {
        cb(null, JSON.parse(line));
      } catch (err) {
        cb(err);
      }
    } else {
      cb();
    }
  },
});

const writer = new RedisPipelineWriter(redis, BATCH_SIZE);

// Node.js readline doesn't produce a stream directly, so adapt it
const lineReader = new Transform({
  objectMode: true,
  transform(_chunk, _enc, cb) {
    // placeholder — lines come from async iterator
    cb();
  },
});

// Use async iteration approach instead
async function processFile() {
  const writer = new RedisPipelineWriter(redis, BATCH_SIZE);
  for await (const line of lines) {
    if (!line.trim()) continue;
    const record = JSON.parse(line);
    if (!writer.write(record)) {
      await new Promise((resolve) => writer.once('drain', resolve));
    }
  }
  return new Promise((resolve, reject) => {
    writer.end((err) => err ? reject(err) : resolve());
  });
}

await processFile();
await redis.disconnect();
```

## Redis Streams (XADD/XREAD/XREADGROUP)

Redis Streams provide append-only log semantics similar to Kafka. Node.js can produce and consume events with consumer groups.

```js
import { createClient } from 'redis';

const redis = createClient({ url: 'redis://localhost:6379' });
await redis.connect();

// === Producer: add events to a stream ===
await redis.xAdd('orders:stream', '*', {
  orderId: 'ORD-1001',
  userId: 'USR-42',
  amount: '99.99',
  currency: 'USD',
});

// === Consumer: read new events in a loop ===
class RedisStreamReader {
  constructor(client, stream, group, consumer, { count = 10, block = 5000 } = {}) {
    this.client = client;
    this.stream = stream;
    this.group = group;
    this.consumer = consumer;
    this.count = count;
    this.block = block;
  }

  // Ensure consumer group exists
  async init() {
    try {
      await this.client.xGroupCreate(this.stream, this.group, '0', {
        MKSTREAM: true,
      });
    } catch (err) {
      if (!err.message.includes('BUSYGROUP')) throw err;
    }
  }

  // Read events as an async iterable
  async *[Symbol.asyncIterator]() {
    while (true) {
      const results = await this.client.xReadGroup(
        this.group,
        this.consumer,
        { key: this.stream, id: '>' },
        { COUNT: this.count, BLOCK: this.block }
      );

      if (!results) continue;

      for (const stream of results) {
        for (const message of stream.messages) {
          yield { id: message.id, data: message.message };
          // Acknowledge after processing
          await this.client.xAck(this.stream, this.group, message.id);
        }
      }
    }
  }
}

// Usage: consume order events
const reader = new RedisStreamReader(redis, 'orders:stream', 'order-processors', 'worker-1');
await reader.init();

let processed = 0;
for await (const event of reader) {
  console.log(`Processing ${event.data.orderId} ($${event.data.amount})`);
  processed++;
  if (processed >= 100) break; // stop after 100 events
}

await redis.disconnect();
```

### Consumer Group with Parallel Workers

```js
import { Worker, isMainThread, parentPort, workerData } from 'node:worker_threads';

if (isMainThread) {
  // Spawn multiple consumer workers
  const workerCount = 4;
  const workers = [];

  for (let i = 0; i < workerCount; i++) {
    const worker = new Worker(new URL(import.meta.url), {
      workerData: { consumerName: `worker-${i}` },
    });
    worker.on('message', (msg) => console.log(msg));
    workers.push(worker);
  }

  // Graceful shutdown
  process.on('SIGINT', () => {
    workers.forEach((w) => w.terminate());
  });
} else {
  const redis = createClient({ url: 'redis://localhost:6379' });
  await redis.connect();

  const reader = new RedisStreamReader(
    redis,
    'orders:stream',
    'order-group',
    workerData.consumerName,
    { count: 5, block: 3000 }
  );
  await reader.init();

  for await (const event of reader) {
    parentPort.postMessage(
      `[${workerData.consumerName}] Processed ${event.data.orderId}`
    );
  }
}
```

## AWS S3 Upload Stream

The `@aws-sdk/lib-storage` `Upload` class accepts a Readable stream and handles multipart uploads automatically.

```js
import { S3Client } from '@aws-sdk/client-s3';
import { Upload } from '@aws-sdk/lib-storage';
import { createReadStream } from 'node:fs';
import { Readable } from 'node:stream';

const s3 = new S3Client({ region: 'us-east-1' });

// Upload a file from disk
async function uploadFile(filePath, bucket, key) {
  const fileStream = createReadStream(filePath);

  const upload = new Upload({
    client: s3,
    params: {
      Bucket: bucket,
      Key: key,
      Body: fileStream,
      ContentType: 'application/octet-stream',
    },
    queueSize: 4,     // concurrent multipart uploads
    partSize: 10 * 1024 * 1024, // 10 MB per part
    leavePartsOnError: false,
  });

  upload.on('httpUploadProgress', (progress) => {
    console.log(`Uploaded ${progress.loaded} bytes → ${progress.key}`);
  });

  const result = await upload.done();
  console.log('Upload complete:', result.Location);
  return result;
}

// Upload from an in-memory stream (e.g., generated data)
async function uploadStream(readable, bucket, key) {
  const upload = new Upload({
    client: s3,
    params: {
      Bucket: bucket,
      Key: key,
      Body: readable,
      ContentType: 'text/csv',
    },
  });

  await upload.done();
}

await uploadFile('./data/export.csv', 'my-bucket', 'exports/2025/export.csv');
```

## AWS S3 Download Stream

`GetObjectCommand` returns a `Body` property that is a Node.js Readable stream.

```js
import { S3Client, GetObjectCommand } from '@aws-sdk/client-s3';
import { createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';

const s3 = new S3Client({ region: 'us-east-1' });

async function downloadToStream(bucket, key, outputPath) {
  const command = new GetObjectCommand({ Bucket: bucket, Key: key });
  const response = await s3.send(command);

  const output = createWriteStream(outputPath);
  await pipeline(response.Body, output);
  console.log('Download complete:', outputPath);
}

// Download and process in-stream
async function processObject(bucket, key) {
  const command = new GetObjectCommand({ Bucket: bucket, Key: key });
  const response = await s3.send(command);

  // response.Body is a Readable stream
  let byteCount = 0;
  response.Body.on('data', (chunk) => {
    byteCount += chunk.length;
  });

  await new Promise((resolve, reject) => {
    response.Body.on('end', resolve);
    response.Body.on('error', reject);
  });

  console.log(`Processed ${byteCount} bytes from s3://${bucket}/${key}`);
}

await downloadToStream('my-bucket', 'data/logs.jsonl', './local-logs.jsonl');
```

## S3 Multipart Upload with Streams

For very large or dynamically generated data, manually control multipart upload stages.

```js
import {
  S3Client,
  CreateMultipartUploadCommand,
  UploadPartCommand,
  CompleteMultipartUploadCommand,
  AbortMultipartUploadCommand,
} from '@aws-sdk/client-s3';
import { Readable } from 'node:stream';

const s3 = new S3Client({ region: 'us-east-1' });

async function multipartUploadFromStream(readable, bucket, key) {
  // Step 1: Initiate
  const { UploadId } = await s3.send(
    new CreateMultipartUploadCommand({
      Bucket: bucket,
      Key: key,
      ContentType: 'application/octet-stream',
    })
  );

  const PART_SIZE = 50 * 1024 * 1024; // 50 MB
  const parts = [];
  let partNumber = 1;

  try {
    let buffer = Buffer.alloc(0);

    for await (const chunk of readable) {
      buffer = Buffer.concat([buffer, chunk]);

      while (buffer.length >= PART_SIZE) {
        const partData = buffer.subarray(0, PART_SIZE);
        buffer = buffer.subarray(PART_SIZE);

        const { ETag } = await s3.send(
          new UploadPartCommand({
            Bucket: bucket,
            Key: key,
            UploadId,
            PartNumber: partNumber,
            Body: partData,
          })
        );

        parts.push({ ETag, PartNumber: partNumber });
        console.log(`Part ${partNumber} uploaded (${partData.length} bytes)`);
        partNumber++;
      }
    }

    // Upload remaining data
    if (buffer.length > 0) {
      const { ETag } = await s3.send(
        new UploadPartCommand({
          Bucket: bucket,
          Key: key,
          UploadId,
          PartNumber: partNumber,
          Body: buffer,
        })
      );
      parts.push({ ETag, PartNumber: partNumber });
      console.log(`Part ${partNumber} uploaded (${buffer.length} bytes)`);
    }

    // Step 3: Complete
    await s3.send(
      new CompleteMultipartUploadCommand({
        Bucket: bucket,
        Key: key,
        UploadId,
        MultipartUpload: { Parts: parts },
      })
    );

    console.log(`Multipart upload complete: ${parts.length} parts`);
  } catch (err) {
    await s3.send(
      new AbortMultipartUploadCommand({
        Bucket: bucket,
        Key: key,
        UploadId,
      })
    );
    throw err;
  }
}
```

## Google Cloud Storage Stream Operations

```js
import { Storage } from '@google-cloud/storage';
import { createReadStream, createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';

const storage = new Storage({ projectId: 'my-project' });
const bucket = storage.bucket('my-gcs-bucket');

// Upload from a Readable stream
async function uploadToGCS(localPath, destPath) {
  const file = bucket.file(destPath);
  const writeStream = file.createWriteStream({
    resumable: true,
    chunkSize: 8 * 1024 * 1024, // 8 MB chunks
    metadata: { contentType: 'application/octet-stream' },
  });

  const readStream = createReadStream(localPath);
  await pipeline(readStream, writeStream);
  console.log('GCS upload complete:', destPath);
}

// Download as a Readable stream
async function downloadFromGCS(srcPath, localPath) {
  const file = bucket.file(srcPath);
  const readStream = file.createReadStream({ validation: 'crc32c' });
  const writeStream = createWriteStream(localPath);

  await pipeline(readStream, writeStream);
  console.log('GCS download complete:', localPath);
}

// Stream a GCS file directly to S3 (no local file)
async function gcToS3(gcsPath, s3Client, s3Bucket, s3Key) {
  const { Upload } = await import('@aws-sdk/lib-storage');
  const file = bucket.file(gcsPath);
  const readStream = file.createReadStream();

  const upload = new Upload({
    client: s3Client,
    params: {
      Bucket: s3Bucket,
      Key: s3Key,
      Body: readStream,
    },
  });

  await upload.done();
  console.log(`Copied gs://${bucket.name}/${gcsPath} → s3://${s3Bucket}/${s3Key}`);
}

await uploadToGCS('./big-data.csv', 'uploads/big-data.csv');
await downloadFromGCS('uploads/big-data.csv', './local-big-data.csv');
```

## Azure Blob Storage Stream Operations

```js
import {
  BlobServiceClient,
  StorageSharedKeyCredential,
} from '@azure/storage-blob';
import { createReadStream, createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';

const account = 'myaccount';
const sharedKeyCredential = new StorageSharedKeyCredential(
  account,
  process.env.AZURE_STORAGE_KEY
);

const blobServiceClient = new BlobServiceClient(
  `https://${account}.blob.core.windows.net`,
  sharedKeyCredential
);

const containerClient = blobServiceClient.getContainerClient('data');

// Upload from a Readable stream
async function uploadToAzure(localPath, blobName) {
  const blockBlobClient = containerClient.getBlockBlobClient(blobName);

  const readStream = createReadStream(localPath, {
    highWaterMark: 4 * 1024 * 1024, // 4 MB buffer
  });

  await blockBlobClient.uploadStream(readStream, 4 * 1024 * 1024, 5, {
    blobHTTPHeaders: { blobContentType: 'application/octet-stream' },
  });

  console.log('Azure upload complete:', blobName);
}

// Download as a Readable stream
async function downloadFromAzure(blobName, localPath) {
  const blockBlobClient = containerClient.getBlockBlobClient(blobName);
  const downloadResponse = await blockBlobClient.download();

  const writeStream = createWriteStream(localPath);
  await pipeline(downloadResponse.readableStreamBody, writeStream);
  console.log('Azure download complete:', localPath);
}

await uploadToAzure('./data.bin', 'backups/data.bin');
await downloadFromAzure('backups/data.bin', './restored-data.bin');
```

## Real-World: Streaming Backup to S3 with Compression and Encryption

This pipeline reads a Redis dataset, compresses it, encrypts it, and streams the result to S3 — all without writing intermediate files.

```js
import { createClient } from 'redis';
import { S3Client } from '@aws-sdk/client-s3';
import { Upload } from '@aws-sdk/lib-storage';
import { createGzip } from 'node:zlib';
import { createCipheriv, randomBytes } from 'node:crypto';
import { Readable, Transform, PassThrough } from 'node:stream';
import { pipeline } from 'node:stream/promises';

const redis = createClient({ url: 'redis://localhost:6379' });
await redis.connect();

const s3 = new S3Client({ region: 'us-east-1' });

const ENCRYPTION_KEY = randomBytes(32); // In production, use a managed key
const IV = randomBytes(16);

// Readable: iterate all keys and dump their values
class RedisDumpStream extends Readable {
  constructor(client, pattern = '*', count = 200) {
    super({ objectMode: true });
    this._client = client;
    this._pattern = pattern;
    this._count = count;
    this._cursor = '0';
  }

  async _read() {
    if (this._cursor === null) return;

    try {
      const [cursor, keys] = await this._client.scan(this._cursor, {
        MATCH: this._pattern,
        COUNT: this._count,
      });

      for (const key of keys) {
        const type = await this._client.type(key);
        let value;

        switch (type) {
          case 'string':
            value = await this._client.get(key);
            break;
          case 'hash':
            value = await this._client.hGetAll(key);
            break;
          case 'list':
            value = await this._client.lRange(key, 0, -1);
            break;
          case 'set':
            value = await this._client.sMembers(key);
            break;
          default:
            continue; // skip unsupported types
        }

        if (!this.push(JSON.stringify({ key, type, value }) + '\n')) {
          this._cursor = cursor === '0' ? null : cursor;
          return;
        }
      }

      if (cursor === '0') {
        this._cursor = null;
        this.push(null);
      } else {
        this._cursor = cursor;
      }
    } catch (err) {
      this.destroy(err);
    }
  }
}

// Encrypt transform: AES-256-CBC
class AesEncryptStream extends Transform {
  constructor(key, iv) {
    super();
    this._cipher = createCipheriv('aes-256-cbc', key, iv);
  }

  _transform(chunk, enc, cb) {
    const encrypted = this._cipher.update(chunk);
    if (encrypted.length > 0) this.push(encrypted);
    cb();
  }

  _flush(cb) {
    const final = this._cipher.final();
    if (final.length > 0) this.push(final);
    cb();
  }
}

// Build the pipeline
const dump = new RedisDumpStream(redis, '*', 500);
const gzip = createGzip({ level: 6 });
const encrypt = new AesEncryptStream(ENCRYPTION_KEY, IV);

// PassThrough allows Upload to consume the final stream
const output = new PassThrough();

const upload = new Upload({
  client: s3,
  params: {
    Bucket: 'backups-bucket',
    Key: `redis-dump-${Date.now()}.enc.gz`,
    Body: output,
    Metadata: {
      'encryption-algorithm': 'aes-256-cbc',
      'iv': IV.toString('base64'),
      'source': 'redis',
    },
  },
  partSize: 25 * 1024 * 1024, // 25 MB parts
  queueSize: 3,
});

// Monitor progress
upload.on('httpUploadProgress', (progress) => {
  if (progress.loaded) {
    console.log(`Uploaded ${(progress.loaded / 1024 / 1024).toFixed(1)} MB`);
  }
});

try {
  await Promise.all([
    pipeline(dump, gzip, encrypt, output),
    upload.done(),
  ]);

  console.log('Backup complete: encrypted, compressed, uploaded to S3');
  console.log(`Encryption key (store securely): ${ENCRYPTION_KEY.toString('base64')}`);
} catch (err) {
  console.error('Backup failed:', err);
  throw err;
} finally {
  await redis.disconnect();
}
```

## Best Practices Checklist

- [ ] Always use SCAN instead of KEYS for iterating Redis keyspaces to avoid blocking the server
- [ ] Set appropriate `COUNT` values for Redis SCAN to balance latency and throughput
- [ ] Use `multi()`/`exec()` pipelines in Redis to batch writes and reduce round-trips
- [ ] Configure `queueSize` and `partSize` for S3 multipart uploads based on network bandwidth
- [ ] Use `xReadGroup` with acknowledgments (`xAck`) for reliable Redis Stream consumption
- [ ] Create consumer groups before starting consumers to avoid race conditions
- [ ] Set `leavePartsOnError: false` in S3 uploads to clean up failed multipart uploads
- [ ] Use CRC32C validation for Google Cloud Storage downloads
- [ ] Compress data before uploading to cloud storage to reduce transfer time and cost
- [ ] Encrypt sensitive data before streaming to cloud storage; store keys separately
- [ ] Monitor `httpUploadProgress` events for S3 uploads to implement progress reporting
- [ ] Use resumable uploads for GCS and Azure when handling very large files
- [ ] Implement graceful shutdown by terminating Redis streams and S3 uploads on SIGINT
- [ ] Test cloud storage operations against local emulators (LocalStack, Azurite, fake-gcs-server)

## Cross-References

- [Stream Architecture](../01-streams-architecture/01-nodejs-stream-architecture.md) — Readable, Writable, and Transform stream fundamentals
- [Stream Error Handling](../07-stream-error-handling/) — retry and recovery strategies for cloud storage failures
- [Stream Performance](../08-stream-performance-optimization/) — tuning buffer sizes and concurrency for throughput
- [Stream Security](../09-stream-security/) — encryption patterns for data at rest and in transit
- [Modern Stream Technologies](../10-modern-stream-technologies/) — Web Streams API and async iterators

## Next Steps

- Explore **Stream-Based Database Operations** (`03-database-migration-sync.md`) for ETL and CDC patterns
- Study **Stream Error Handling** (`../07-stream-error-handling/`) for building resilient backup pipelines
- Review **Stream Performance** (`../08-stream-performance-optimization/`) for optimizing upload throughput
