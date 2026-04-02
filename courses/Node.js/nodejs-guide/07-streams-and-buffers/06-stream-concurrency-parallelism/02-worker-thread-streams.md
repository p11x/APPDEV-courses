# Worker Thread Stream Processing

## What You'll Learn

- Building a `WorkerThreadTransform` that offloads CPU-intensive work to worker threads
- Implementing a worker pool for concurrent stream processing
- Using `SharedArrayBuffer` for zero-copy data transfer between main thread and workers
- Coordinating backpressure between the main thread and worker pool
- Collecting ordered vs unordered results from workers
- Real-world: parallel image resize stream using worker threads

## Why Worker Threads for Streams?

Node.js streams run on the main thread by default. CPU-heavy transforms (encryption, image processing) block the event loop. Worker threads distribute work across threads while preserving stream semantics.

## WorkerThreadTransform

### Main Thread Transform

```js
// worker-thread-transform.mjs
import { Transform } from 'node:stream';
import { Worker } from 'node:worker_threads';
import { randomUUID } from 'node:crypto';
import { resolve } from 'node:path';

class WorkerThreadTransform extends Transform {
  #worker;
  #pending = new Map();

  constructor(workerPath, options = {}) {
    super({ objectMode: true, ...options });
    this.#worker = new Worker(resolve(workerPath));
    this.#worker.on('message', ({ id, value, error }) => {
      const entry = this.#pending.get(id);
      if (!entry) return;
      this.#pending.delete(id);
      error ? entry.reject(new Error(error)) : entry.resolve(value);
    });
    this.#worker.on('error', (err) => this.destroy(err));
  }

  async _transform(chunk, encoding, callback) {
    try {
      const id = randomUUID();
      const result = await new Promise((resolve, reject) => {
        this.#pending.set(id, { resolve, reject });
        this.#worker.postMessage({ id, chunk });
      });
      this.push(result);
      callback();
    } catch (err) { callback(err); }
  }

  _flush(callback) {
    this.#worker.terminate().then(() => callback()).catch(callback);
  }
}

export { WorkerThreadTransform };
```

### Worker Script

```js
// workers/cpu-worker.mjs
import { parentPort } from 'node:worker_threads';
import { createHash } from 'node:crypto';

parentPort.on('message', ({ id, chunk }) => {
  try {
    const hash = createHash('sha256').update(JSON.stringify(chunk)).digest('hex');
    parentPort.postMessage({ id, value: { ...chunk, hash, processedAt: Date.now() } });
  } catch (error) { parentPort.postMessage({ id, error: error.message }); }
});
```

### Usage

```js
import { pipeline } from 'node:stream/promises';
import { Readable } from 'node:stream';
import { WorkerThreadTransform } from './worker-thread-transform.mjs';

const source = Readable.from(
  Array.from({ length: 100 }, (_, i) => ({ id: i, payload: `record-${i}` }))
);

await pipeline(source, new WorkerThreadTransform('./workers/cpu-worker.mjs'),
  async function* (src) {
    for await (const r of src) console.log(`Processed: ${r.id} hash=${r.hash.slice(0, 12)}...`);
  }
);
```

## Worker Pool Implementation

A single worker per transform is wasteful. A pool distributes chunks across N workers.

```js
// worker-pool-transform.mjs
import { Transform } from 'node:stream';
import { Worker } from 'node:worker_threads';
import { randomUUID } from 'node:crypto';
import { resolve } from 'node:path';

class WorkerPool {
  #workers = [];
  #free = [];
  #pending = new Map();
  #queue = [];

  constructor(size, workerPath) {
    for (let i = 0; i < size; i++) {
      const worker = new Worker(resolve(workerPath));
      worker.on('message', (msg) => this.#onMessage(i, msg));
      worker.on('error', () => this.#workers.forEach((w) => w.terminate()));
      this.#workers.push(worker);
      this.#free.push(i);
    }
  }

  #onMessage(workerIdx, { id, value, error }) {
    this.#free.push(workerIdx);
    const entry = this.#pending.get(id);
    if (!entry) return;
    this.#pending.delete(id);
    error ? entry.reject(new Error(error)) : entry.resolve(value);
    this.#flush();
  }

  #flush() {
    while (this.#free.length && this.#queue.length) {
      const idx = this.#free.shift();
      const { id, chunk, resolve, reject } = this.#queue.shift();
      this.#pending.set(id, { resolve, reject });
      this.#workers[idx].postMessage({ id, chunk });
    }
  }

  exec(chunk) {
    return new Promise((resolve, reject) => {
      const id = randomUUID();
      this.#queue.push({ id, chunk, resolve, reject });
      this.#flush();
    });
  }

  get pending() { return this.#pending.size + this.#queue.length; }
  async terminate() { await Promise.all(this.#workers.map((w) => w.terminate())); }
}

class PooledWorkerTransform extends Transform {
  #pool;
  constructor(poolSize, workerPath, options = {}) {
    super({ objectMode: true, ...options });
    this.#pool = new WorkerPool(poolSize, workerPath);
  }
  async _transform(chunk, enc, cb) {
    try { this.push(await this.#pool.exec(chunk)); cb(); }
    catch (err) { cb(err); }
  }
  async _flush(cb) { await this.#pool.terminate(); cb(); }
}

export { PooledWorkerTransform, WorkerPool };
```

## SharedArrayBuffer for Zero-Copy Transfer

For large binary payloads, `SharedArrayBuffer` avoids serialization overhead.

```js
// shared-worker.mjs
import { parentPort } from 'node:worker_threads';

parentPort.on('message', ({ id, sharedBuffer, byteOffset, byteLength }) => {
  const view = new Uint8Array(sharedBuffer, byteOffset, byteLength);
  for (let i = 0; i < view.length; i++) view[i] = 255 - view[i];
  parentPort.postMessage({ id, bytesProcessed: byteLength });
});
```

```js
// shared-transform.mjs
import { Transform } from 'node:stream';
import { Worker } from 'node:worker_threads';
import { randomUUID } from 'node:crypto';

class SharedBufferTransform extends Transform {
  #worker;
  #sharedBuffer;
  #pending = new Map();

  constructor() {
    super();
    this.#sharedBuffer = new SharedArrayBuffer(1024 * 1024);
    this.#worker = new Worker('./shared-worker.mjs');
    this.#worker.on('message', ({ id, bytesProcessed }) => {
      const entry = this.#pending.get(id);
      if (!entry) return;
      this.#pending.delete(id);
      entry.resolve(Buffer.from(this.#sharedBuffer, 0, bytesProcessed));
    });
    this.#worker.on('error', (err) => this.destroy(err));
  }

  _transform(chunk, enc, cb) {
    const id = randomUUID();
    chunk.copy(new Uint8Array(this.#sharedBuffer, 0, chunk.length));
    new Promise((resolve, reject) => {
      this.#pending.set(id, { resolve, reject });
      this.#worker.postMessage({ id, sharedBuffer: this.#sharedBuffer, byteOffset: 0, byteLength: chunk.length });
    }).then((r) => { this.push(r); cb(); }).catch(cb);
  }

  _flush(cb) { this.#worker.terminate().then(() => cb()).catch(cb); }
}

export { SharedBufferTransform };
```

## Backpressure Coordination

Track in-flight tasks to block the upstream when workers are saturated.

```js
import { Transform } from 'node:stream';
import { WorkerPool } from './worker-pool-transform.mjs';

class BackpressurePoolTransform extends Transform {
  #pool;
  #highWaterMark;
  #inFlight = 0;
  #drainCallback = null;

  constructor(poolSize, workerPath, options = {}) {
    super({ objectMode: true, highWaterMark: 16, ...options });
    this.#pool = new WorkerPool(poolSize, workerPath);
    this.#highWaterMark = options.highWaterMark ?? 16;
  }

  _transform(chunk, enc, callback) {
    this.#inFlight++;
    this.#pool.exec(chunk).then((result) => {
      this.#inFlight--;
      this.push(result);
      if (this.#drainCallback && this.#inFlight < this.#highWaterMark) {
        const cb = this.#drainCallback;
        this.#drainCallback = null;
        cb();
      }
    }).catch((err) => this.destroy(err));

    if (this.#inFlight >= this.#highWaterMark) this.#drainCallback = callback;
    else callback();
  }

  async _flush(cb) {
    const check = () => {
      if (this.#inFlight === 0) this.#pool.terminate().then(cb).catch(cb);
      else setTimeout(check, 10);
    };
    check();
  }
}

export { BackpressurePoolTransform };
```

## Ordered vs Unordered Results

Workers finish at different times. Choose between preserving order or emitting immediately.

```js
import { Transform } from 'node:stream';
import { WorkerPool } from './worker-pool-transform.mjs';

class OrderedWorkerTransform extends Transform {
  #pool;
  #buffer = new Map();
  #nextSeq = 0;
  #currentSeq = 0;

  constructor(poolSize, workerPath, options = {}) {
    super({ objectMode: true, ...options });
    this.#pool = new WorkerPool(poolSize, workerPath);
  }

  _transform(chunk, enc, cb) {
    const seq = this.#currentSeq++;
    this.#pool.exec({ seq, data: chunk }).then((result) => {
      this.#buffer.set(seq, result);
      while (this.#buffer.has(this.#nextSeq)) {
        this.push(this.#buffer.get(this.#nextSeq));
        this.#buffer.delete(this.#nextSeq++);
      }
    }).catch((err) => this.destroy(err));
    cb();
  }

  async _flush(cb) { await this.#pool.terminate(); cb(); }
}

class UnorderedWorkerTransform extends Transform {
  #pool;
  constructor(poolSize, workerPath, options = {}) {
    super({ objectMode: true, ...options });
    this.#pool = new WorkerPool(poolSize, workerPath);
  }
  _transform(chunk, enc, cb) {
    this.#pool.exec(chunk).then((r) => this.push(r)).catch((e) => this.destroy(e));
    cb();
  }
  async _flush(cb) { await this.#pool.terminate(); cb(); }
}

export { OrderedWorkerTransform, UnorderedWorkerTransform };
```

## Real-World: Parallel Image Resize Stream

```js
// image-resize-worker.mjs
import { parentPort } from 'node:worker_threads';
import { createCanvas, loadImage } from '@napi-rs/canvas';

parentPort.on('message', async ({ id, buffer, width, height, format }) => {
  try {
    const img = await loadImage(buffer);
    const canvas = createCanvas(width, height);
    canvas.getContext('2d').drawImage(img, 0, 0, width, height);
    parentPort.postMessage({
      id,
      buffer: canvas.toBuffer({ jpeg: 'image/jpeg', png: 'image/png' }[format] ?? 'image/jpeg'),
    });
  } catch (error) { parentPort.postMessage({ id, error: error.message }); }
});
```

```js
// resize-pipeline.mjs
import { readdir, readFile, writeFile } from 'node:fs/promises';
import { join, parse } from 'node:path';
import { pipeline } from 'node:stream/promises';
import { PooledWorkerTransform } from './worker-pool-transform.mjs';

class ImageResizeTransform extends PooledWorkerTransform {
  #width; #height; #format;
  constructor(poolSize, { width = 800, height = 600, format = 'jpeg' } = {}) {
    super(poolSize, './image-resize-worker.mjs', { objectMode: true });
    this.#width = width; this.#height = height; this.#format = format;
  }
}

async function* imageSource(dir) {
  for (const f of await readdir(dir)) {
    if (!/\.(jpg|jpeg|png|webp)$/i.test(f)) continue;
    yield { filename: f, buffer: await readFile(join(dir, f)) };
  }
}

function imageSink(outDir) {
  return async function* (source) {
    for await (const { filename, buffer } of source) {
      await writeFile(join(outDir, `${parse(filename).name}-resized.jpg`), buffer);
      console.log(`${filename} resized (${buffer.length} bytes)`);
    }
  };
}

await pipeline(imageSource('./images'),
  new ImageResizeTransform(4, { width: 400, height: 300 }),
  imageSink('./resized')
);
```

## Best Practices Checklist

- [ ] Set pool size to `navigator.hardwareConcurrency - 1` or a sensible default
- [ ] Always handle worker `error` events to prevent silent failures
- [ ] Terminate workers in `_flush` to prevent resource leaks
- [ ] Use `SharedArrayBuffer` only for large binary payloads (>64KB)
- [ ] Implement backpressure tracking with in-flight counters
- [ ] Choose ordered collection only when sequence matters (higher memory cost)
- [ ] Set appropriate `highWaterMark` on transforms to bound memory usage
- [ ] Monitor worker pool queue depth in production with metrics

## Cross-References

- [Stream Concurrency and Parallel Processing](./01-parallel-processing.md) — foundational parallel patterns
- [Stream Backpressure](../03-readable-streams/03-backpressure.md) — backpressure mechanics
- [Error Handling in Streams](../07-stream-error-handling/01-error-patterns.md) — worker error handling

## Next Steps

- [Stream Partitioning, Batching, and Windowing](./03-stream-partitioning-batching.md) — partitioning work by key
- Benchmark pool size vs throughput to find optimal worker count
