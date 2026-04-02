# Stream Piping, Chaining, and Composition

## What You'll Learn

- `pipe()` vs `pipeline()` comparison and migration guide
- `pipeline()` with multiple stages and centralized error handling
- `stream.compose()` for declarative pipeline construction
- Unpipe and re-pipe patterns for dynamic routing
- Stream branching (tee) and merging patterns
- Conditional piping based on data content
- Pipeline with `AbortController` and timeout

## `pipe()` vs `pipeline()` Comparison

| Feature | `pipe()` | `pipeline()` |
|---------|----------|--------------|
| Error propagation | Manual — each stream needs `.on('error')` | Automatic — single callback/Promise |
| Cleanup on error | Must call `destroy()` manually | Destroys all streams automatically |
| Multiple destinations | Not supported natively | Supported via composition |
| Source auto-close | No | Yes |
| Recommendation | Legacy code only | Always use `pipeline()` |

### Migration from `pipe()` to `pipeline()`

```javascript
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip } from 'node:zlib';
import { pipeline } from 'node:stream/promises';

// BEFORE: pipe() — error-prone, no cleanup
const source = createReadStream('input.txt');
const gzip = createGzip();
const dest = createWriteStream('output.txt.gz');

source.on('error', () => { gzip.destroy(); dest.destroy(); });
gzip.on('error', () => { source.destroy(); dest.destroy(); });
dest.on('error', () => { source.destroy(); gzip.destroy(); });
source.pipe(gzip).pipe(dest);

// AFTER: pipeline() — automatic error handling and cleanup
try {
  await pipeline(createReadStream('input.txt'), createGzip(), createWriteStream('output.txt.gz'));
} catch (err) { console.error('Pipeline failed:', err); }
```

## `pipeline()` with Multiple Stages

```javascript
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createGzip } from 'node:zlib';
import { Transform } from 'node:stream';

async function processLogFile(inputPath, outputPath) {
  const lineSplitter = new Transform({
    transform(chunk, enc, cb) {
      for (const line of chunk.toString().split('\n')) {
        if (line.trim()) this.push(line);
      }
      cb();
    },
  });

  const errorFilter = new Transform({
    objectMode: true,
    transform(line, enc, cb) {
      if (line.includes('ERROR') || line.includes('WARN')) this.push(line + '\n');
      cb();
    },
  });

  try {
    await pipeline(
      createReadStream(inputPath), lineSplitter, errorFilter,
      createGzip(), createWriteStream(outputPath),
    );
    return { success: true };
  } catch (err) { return { success: false, error: err.message }; }
}
```

## Pipeline with AbortController and Timeout

```javascript
import { pipeline } from 'node:stream/promises';

async function compressWithTimeout(input, output, timeoutMs) {
  const controller = new AbortController();
  const timeout = setTimeout(() => controller.abort(), timeoutMs);

  try {
    await pipeline(
      createReadStream(input), createGzip(), createWriteStream(output),
      { signal: controller.signal },
    );
    return { success: true };
  } catch (err) {
    if (err.code === 'ABORT_ERR') return { success: false, error: 'Timed out' };
    throw err;
  } finally { clearTimeout(timeout); }
}

const result = await compressWithTimeout('large.dat', 'large.gz', 60_000);
```

## `stream.compose()` for Declarative Pipelines

`compose()` returns a single Duplex from multiple streams, making pipelines reusable.

```javascript
import { compose, Transform } from 'node:stream';
import { createReadStream, createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';

function createLogProcessor() {
  return compose(
    new Transform({
      transform(chunk, enc, cb) {
        for (const line of chunk.toString().split('\n')) {
          if (line.trim()) this.push(line);
        }
        cb();
      },
    }),
    new Transform({
      objectMode: true,
      transform(line, enc, cb) { try { cb(null, JSON.parse(line)); } catch { cb(); } },
    }),
    new Transform({
      objectMode: true,
      transform(entry, enc, cb) {
        if (entry.level === 'error' || entry.level === 'warn') cb(null, entry);
        else cb();
      },
    }),
    new Transform({
      objectMode: true,
      transform(entry, enc, cb) { cb(null, JSON.stringify(entry) + '\n'); },
    })
  );
}

await pipeline(
  createReadStream('/var/log/app.log'), createLogProcessor(),
  createWriteStream('/var/log/errors.log'),
);
```

## Unpipe and Re-Pipe Patterns

Dynamically disconnect and reconnect streams for failover.

```javascript
import { createReadStream, createWriteStream } from 'node:fs';
import { Writable } from 'node:stream';

// Rotating file writer
function createRotatingWriter(basePath, maxBytes) {
  let currentSize = 0, fileIndex = 0, currentDest = null;
  return new Writable({
    write(chunk, encoding, callback) {
      if (!currentDest || currentSize >= maxBytes) {
        if (currentDest) currentDest.end();
        currentDest = createWriteStream(`${basePath}.${++fileIndex}`);
        currentSize = 0;
      }
      currentSize += chunk.length;
      currentDest.write(chunk, callback);
    },
    final(callback) { if (currentDest) currentDest.end(callback); else callback(); },
  });
}

// Failover via unpipe/re-pipe
const source = createReadStream('data.jsonl');
const primary = createWriteStream('/primary/store.jsonl');
const backup = createWriteStream('/backup/store.jsonl');

source.pipe(primary);
primary.on('error', (err) => {
  console.error('Primary failed, switching:', err.message);
  source.unpipe(primary);
  source.pipe(backup);
});
```

## Stream Branching (Tee) Pattern

Split a single source into multiple parallel destinations.

```javascript
import { PassThrough, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

function tee(...destinations) {
  const pt = new PassThrough({ objectMode: true });
  for (const dest of destinations) pt.pipe(dest);
  return pt;
}

const jsonParser = new Transform({
  transform(chunk, enc, cb) { try { cb(null, JSON.parse(chunk)); } catch { cb(); } },
});

await pipeline(
  createReadStream('events.jsonl'), jsonParser,
  tee(
    new Transform({
      objectMode: true,
      transform(chunk, enc, cb) { cb(null, JSON.stringify(chunk) + '\n'); },
    }),
    new Transform({
      objectMode: true,
      transform(event, enc, cb) { analyticsService.track(event); cb(null, event); },
    }),
  ),
  createWriteStream('processed.jsonl'),
);
```

## Stream Merging Pattern

Combine multiple sources into a single destination stream.

```javascript
import { PassThrough } from 'node:stream';
import { createReadStream } from 'node:fs';

function merge(...sources) {
  const merged = new PassThrough();
  let pending = sources.length;
  for (const source of sources) {
    source.on('end', () => { if (--pending === 0) merged.end(); });
    source.on('error', (err) => merged.destroy(err));
    source.pipe(merged, { end: false });
  }
  return merged;
}

const combined = merge(
  createReadStream('/logs/app.log.1'),
  createReadStream('/logs/app.log.2'),
  createReadStream('/logs/app.log.3'),
);
combined.on('data', (chunk) => processChunk(chunk));
```

## Conditional Piping Based on Content

Route data to different destinations based on content.

```javascript
import { Transform } from 'node:stream';
import { createWriteStream } from 'node:fs';

class ConditionalRouter extends Transform {
  constructor(routes, options = {}) {
    super({ objectMode: true });
    this.routes = routes;
    this.defaultDest = options.default || null;
  }

  _transform(chunk, encoding, callback) {
    let routed = false;
    for (const { predicate, destination } of this.routes) {
      if (predicate(chunk)) { destination.write(chunk); routed = true; break; }
    }
    if (!routed && this.defaultDest) this.defaultDest.write(chunk);
    this.push(chunk);
    callback();
  }

  _flush(callback) {
    for (const { destination } of this.routes) destination.end();
    if (this.defaultDest) this.defaultDest.end();
    callback();
  }
}

const router = new ConditionalRouter(
  [
    { predicate: (e) => e.level === 'error', destination: createWriteStream('errors.log') },
    { predicate: (e) => e.level === 'warn', destination: createWriteStream('warnings.log') },
  ],
  { default: createWriteStream('info.log') }
);

await pipeline(
  createReadStream('events.jsonl'),
  new Transform({ transform(chunk, enc, cb) { try { cb(null, JSON.parse(chunk)); } catch { cb(); } } }),
  router, createWriteStream('all-events.jsonl'),
);
```

## Multi-Destination Writer with Acknowledgment

Write each record to multiple destinations and wait for all to acknowledge.

```javascript
import { Writable } from 'node:stream';

class MultiDestWriter extends Writable {
  constructor(destinations, options = {}) {
    super({ objectMode: true });
    this.destinations = destinations;
    this.waitForAll = options.waitForAll !== false;
    this.stats = { total: 0, success: 0, failed: 0 };
  }

  async _write(chunk, encoding, callback) {
    this.stats.total++;
    const writes = this.destinations.map(
      (dest) => new Promise((resolve, reject) => dest.write(chunk, (e) => e ? reject(e) : resolve()))
    );
    try {
      if (this.waitForAll) await Promise.all(writes);
      else await Promise.allSettled(writes);
      this.stats.success++;
      callback();
    } catch (err) {
      this.stats.failed++;
      this.emit('write-error', { record: chunk, error: err });
      callback(this.waitForAll ? err : null);
    }
  }

  _final(callback) {
    let pending = this.destinations.length;
    for (const dest of this.destinations) {
      dest.end(() => { if (--pending === 0) callback(); });
    }
  }
}

const writer = new MultiDestWriter([
  createWriteStream('primary.db'),
  createWriteStream('replica.db'),
  createWriteStream('audit.log'),
]);
writer.on('write-error', ({ record, error }) => console.error('Write failed:', record.id, error.message));
```

## Best Practices Checklist

- [ ] Always use `pipeline()` instead of `pipe()` for error handling and cleanup
- [ ] Use `stream.compose()` to build reusable multi-stream components
- [ ] Handle pipeline errors centrally — log, retry, or alert as needed
- [ ] Use `AbortController` for timeout and cancellation on long-running pipelines
- [ ] Implement proper unpipe/re-pipe logic for failover scenarios
- [ ] Use `PassThrough` as a tee point for stream branching
- [ ] Track write statistics for observability in production
- [ ] Avoid unbounded buffering in merge operations — respect backpressure
- [ ] Use conditional routing for data-driven stream splitting
- [ ] Always end destination streams in `_final()` for clean shutdown

## Cross-References

- [Custom Readable and Writable Streams](./01-custom-readable-writable.md) for building data sources and sinks
- [Custom Transform and Duplex Streams](./02-custom-transform-duplex.md) for data transformation patterns
- [Stream Error Handling](../07-stream-error-handling/01-error-patterns.md) for error propagation and recovery
- [HTTP Streaming](../05-stream-network-operations/01-http-streaming.md) for network pipeline patterns

## Next Steps

- Build production HTTP streaming pipelines with Express/Fastify
- Implement distributed stream processing across multiple services
- Explore Worker Threads for CPU-bound pipeline stages
- Apply backpressure monitoring and adaptive buffering strategies
