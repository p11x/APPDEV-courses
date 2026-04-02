# Advanced File System Stream Operations

## What You'll Learn

- `fs.opendir()` streaming directory entries with recursive walking
- Chokidar-style file watcher as a Readable stream
- Atomic file writes using temp file + rename with streams
- File locking patterns for concurrent access
- Log rotation stream patterns (size-based, time-based)
- Streaming file search and grep implementation
- Real-world: streaming backup system for directory trees

---

## 1. Streaming Directory Traversal with `fs.opendir()`

`fs.opendir()` returns an `AsyncIterable` `Dir` that streams entries without loading all at once.

```js
import { opendir, stat } from 'node:fs/promises';
import { join } from 'node:path';

async function* walkDir(dirPath) {
  const dir = await opendir(dirPath);
  for await (const dirent of dir) {
    const fullPath = join(dirPath, dirent.name);
    if (dirent.isDirectory()) yield* walkDir(fullPath);
    else yield { path: fullPath, size: (await stat(fullPath)).size };
  }
}

let totalSize = 0;
for await (const f of walkDir('./nodejs-guide')) totalSize += f.size;
console.log(`Total: ${(totalSize / 1024).toFixed(1)} KB`);
```

---

## 2. File Watcher as Readable Stream

Wrap `fs.watch` in a `Readable` for backpressure-aware change events.

```js
import { Readable, Transform } from 'node:stream';
import { watch } from 'node:fs';
import { join } from 'node:path';

class FileWatcher extends Readable {
  constructor(dirPath, { recursive = true } = {}) {
    super({ objectMode: true });
    this.dirPath = dirPath;
    this.recursive = recursive;
    this._watcher = null;
  }

  _construct(cb) {
    this._watcher = watch(this.dirPath, { recursive }, (type, name) => {
      if (name) this.push({ type, path: join(this.dirPath, name), ts: Date.now() });
    });
    this._watcher.on('error', (err) => this.destroy(err));
    cb();
  }

  _read() {}
  _destroy(err, cb) { this._watcher?.close(); cb(err); }
}

const fmt = new Transform({
  objectMode: true,
  transform(e, _, cb) { cb(null, `[${new Date(e.ts).toISOString()}] ${e.type}: ${e.path}\n`); },
});
new FileWatcher('./src').pipe(fmt).pipe(process.stdout);
```

---

## 3. Atomic File Writes (Temp + Rename)

Write to a temp file, flush, then `rename()` — atomic on most filesystems.

```js
import { createWriteStream } from 'node:fs';
import { rename, unlink } from 'node:fs/promises';
import { join, dirname, basename } from 'node:path';
import { randomUUID } from 'node:crypto';
import { pipeline } from 'node:stream/promises';

async function atomicWrite(dest, dataOrStream) {
  const tmp = join(dirname(dest), `.${basename(dest)}.${randomUUID()}.tmp`);
  try {
    const ws = createWriteStream(tmp, { flush: true });
    if (typeof dataOrStream === 'string' || Buffer.isBuffer(dataOrStream)) {
      ws.end(dataOrStream);
      await new Promise((r, j) => { ws.on('finish', r); ws.on('error', j); });
    } else {
      await pipeline(dataOrStream, ws);
    }
    await rename(tmp, dest);
  } catch (err) {
    await unlink(tmp).catch(() => {});
    throw err;
  }
}

// String write
await atomicWrite('./config.json', JSON.stringify({ v: 2 }, null, 2));

// Stream write from fetch
const resp = await fetch('https://example.com/data.csv');
await atomicWrite('./data.csv', Readable.fromWeb(resp.body));
```

---

## 4. File Locking for Concurrent Access

Serialize writes to shared files with a lock file.

```js
import { open, unlink } from 'node:fs/promises';
import { join } from 'node:path';

class FileLock {
  constructor(dir) { this.dir = dir; }
  async acquire(name, timeout = 5000) {
    const p = join(this.dir, `${name}.lock`);
    const t0 = Date.now();
    while (Date.now() - t0 < timeout) {
      try {
        const h = await open(p, 'wx');
        await h.writeFile(String(process.pid));
        await h.close();
        return p;
      } catch { await new Promise((r) => setTimeout(r, 50)); }
    }
    throw new Error(`Lock timeout: ${name}`);
  }
  async release(p) { await unlink(p).catch(() => {}); }
}

const lock = new FileLock('./locks');
const lp = await lock.acquire('app-log');
try {
  const h = await open('./app.log', 'a');
  const ws = h.createWriteStream();
  ws.write(`[${new Date().toISOString()}] PID ${process.pid}\n`);
  ws.end();
  await new Promise((r) => ws.on('finish', r));
  await h.close();
} finally {
  await lock.release(lp);
}
```

---

## 5. Log Rotation Stream Patterns

### Size-Based

```js
import { createWriteStream } from 'node:fs';
import { rename } from 'node:fs/promises';
import { Writable } from 'node:stream';

class RotatingLog extends Writable {
  constructor(path, { maxBytes = 1_048_576, maxFiles = 5 } = {}) {
    super();
    this.path = path; this.maxBytes = maxBytes; this.maxFiles = maxFiles;
    this._size = 0; this._stream = null;
  }

  _construct(cb) { this._stream = createWriteStream(this.path, { flags: 'a' }); cb(); }

  async _write(chunk, enc, cb) {
    this._size += chunk.length;
    if (this._size >= this.maxBytes) {
      this._stream.end();
      for (let i = this.maxFiles - 1; i > 0; i--)
        await rename(`${this.path}.${i}`, `${this.path}.${i + 1}`).catch(() => {});
      await rename(this.path, `${this.path}.1`);
      this._stream = createWriteStream(this.path);
      this._size = 0;
    }
    this._stream.write(chunk, enc, cb);
  }

  _final(cb) { this._stream.end(cb); }
}

const log = new RotatingLog('./app.log', { maxBytes: 10240, maxFiles: 3 });
for (let i = 0; i < 200; i++) log.write(`Entry ${i}: payload\n`);
log.end();
```

### Time-Based

```js
class TimeRotatingLog extends Writable {
  constructor(path, { intervalMs = 86_400_000 } = {}) {
    super();
    this.path = path; this.intervalMs = intervalMs;
    this._stream = null; this._timer = null;
  }
  _construct(cb) {
    this._open();
    this._timer = setInterval(() => { this._stream.end(); this._open(); }, this.intervalMs);
    this._timer.unref(); cb();
  }
  _open() { this._stream = createWriteStream(`${this.path}.${new Date().toISOString().slice(0, 10)}`, { flags: 'a' }); }
  _write(c, e, cb) { this._stream.write(c, e, cb); }
  _final(cb) { clearInterval(this._timer); this._stream.end(cb); }
}
```

---

## 6. Streaming File Search and Grep

```js
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';
import { glob } from 'node:fs/promises';

async function* grepFiles(pattern, paths) {
  for (const p of paths) {
    const rl = createInterface({ input: createReadStream(p) });
    let n = 0;
    for await (const line of rl) {
      n++;
      if (pattern.test(line)) yield { file: p, line: n, match: line.trim() };
    }
  }
}

const files = [];
for await (const f of glob('**/*.md')) files.push(f);
for await (const hit of grepFiles(/stream/i, files))
  console.log(`${hit.file}:${hit.line}: ${hit.match.slice(0, 80)}`);
```

---

## 7. Real-World: Streaming Backup System

Archive a directory tree into a compressed stream:

```js
import { createReadStream, createWriteStream } from 'node:fs';
import { opendir } from 'node:fs/promises';
import { join, relative } from 'node:path';
import { createGzip } from 'node:zlib';
import { Readable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

class DirEntryStream extends Readable {
  constructor(root) {
    super({ objectMode: true });
    this.root = root;
    this._queue = [root];
  }

  async _read() {
    if (!this._queue.length) { this.push(null); return; }
    const dir = await opendir(this._queue.shift());
    for await (const d of dir) {
      const full = join(this.root, d.name);
      const rel = relative(this.root, full);
      if (d.isDirectory()) {
        this._queue.push(full);
        this.push(JSON.stringify({ t: 'd', p: rel }) + '\n');
      } else {
        const chunks = [];
        for await (const c of createReadStream(full)) chunks.push(c);
        this.push(JSON.stringify({ t: 'f', p: rel, d: Buffer.concat(chunks).toString('base64') }) + '\n');
      }
    }
  }
}

const progress = new Transform({ transform(c, _, cb) { process.stderr.write('.'); cb(null, c); } });

await pipeline(new DirEntryStream('./nodejs-guide'), progress, createGzip(), createWriteStream('./backup.gz'));
console.log('\nBackup complete');
```

---

## Best Practices Checklist

- [ ] Use `fs.opendir()` over `fs.readdir()` for large directories
- [ ] Use `pipeline()` for automatic error propagation and cleanup
- [ ] Atomic writes with temp files + `rename()` to prevent corruption
- [ ] File locks with timeouts to avoid deadlocks
- [ ] Set `highWaterMark` on file read streams to control memory
- [ ] Handle `ENOENT` when watching/walking mutable directories
- [ ] Rotate logs by both size and time in production
- [ ] Use `gzip`/`brotli` transforms for streaming backups
- [ ] Progress trackers for long-running file operations
- [ ] `Readable.fromWeb()` to adapt fetch responses into Node.js streams

---

## Cross-References

- [01-stream-fundamentals.md](./01-stream-fundamentals.md) — Core stream types and `pipeline()`
- [03-file-format-streaming.md](./03-file-format-streaming.md) — Streaming JSON, CSV, XML processing
- [02-tcp-udp-socket-streams.md](../05-stream-network-operations/02-tcp-udp-socket-streams.md) — Network stream patterns

---

## Next Steps

- Explore [03-file-format-streaming.md](./03-file-format-streaming.md) for format conversion pipelines
- Move to [05-stream-network-operations](../05-stream-network-operations/) to combine file streams with TCP/HTTP/2
- Benchmark `opendir()` vs `readdir()` on 100K+ entry directories
- Implement TLS-wrapped file watchers that stream changes over the network
