# File Format Streaming and Processing

## What You'll Learn

- Streaming JSON parser (`JSON.parse` alternative for large files)
- NDJSON/JSONL streaming read and write
- Streaming XML parser using transform streams
- Streaming YAML parser patterns
- Multi-format file detection and routing stream
- Streaming file conversion pipeline (CSV→JSON→XML)
- Real-world: streaming ETL pipeline for data warehouse ingestion

---

## 1. Streaming JSON Parser

`JSON.parse()` loads the entire document. For multi-GB files, use `stream-json`:

```bash
npm install stream-json
```

```js
import { createReadStream } from 'node:fs';
import { parser } from 'stream-json';
import { streamArray } from 'stream-json/streamers/StreamArray';

const stream = createReadStream('./large-dataset.json')
  .pipe(parser())
  .pipe(streamArray());

let count = 0;
stream.on('data', ({ value }) => {
  if (++count % 100_000 === 0) console.log(`Processed ${count}...`);
});
stream.on('end', () => console.log(`Done: ${count} records`));
```

### Custom Streaming JSON Parser

```js
import { Transform } from 'node:stream';

class JsonStreamParser extends Transform {
  constructor() {
    super({ readableObjectMode: true });
    this._current = ''; this._depth = 0; this._inStr = false; this._esc = false;
  }

  _transform(chunk, _, cb) {
    for (const ch of chunk.toString()) {
      this._current += ch;
      if (this._esc) { this._esc = false; continue; }
      if (ch === '\\' && this._inStr) { this._esc = true; continue; }
      if (ch === '"') { this._inStr = !this._inStr; continue; }
      if (this._inStr) continue;
      if (ch === '{' || ch === '[') this._depth++;
      if ((ch === '}' || ch === ']') && --this._depth === 0) {
        this.push(JSON.parse(this._current));
        this._current = '';
      }
    }
    cb();
  }

  _flush(cb) { if (this._current.trim()) this.push(JSON.parse(this._current)); cb(); }
}

// Filter active records from a JSON stream
import { pipeline } from 'node:stream/promises';
await pipeline(
  createReadStream('./records.json'), new JsonStreamParser(),
  async function* (src) { for await (const r of src) if (r.active) yield JSON.stringify(r) + '\n'; },
  createWriteStream('./active.jsonl'),
);
```

---

## 2. NDJSON / JSONL Streaming

Each line is a self-contained JSON object — ideal for streaming.

```js
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';

async function* readNdjson(path) {
  const rl = createInterface({ input: createReadStream(path), crlfDelay: Infinity });
  for await (const line of rl) if (line.trim()) yield JSON.parse(line);
}

for await (const e of readNdjson('./events.jsonl')) console.log(e.type, e.ts);
```

### Write

```js
import { createWriteStream } from 'node:fs';

function ndjsonWriter(path) {
  const ws = createWriteStream(path);
  return { write(o) { ws.write(JSON.stringify(o) + '\n'); }, end() { return new Promise((r) => ws.end(r)); } };
}

const w = ndjsonWriter('./out.jsonl');
for (let i = 0; i < 1_000_000; i++) w.write({ id: i, ts: Date.now() });
await w.end();
```

### NDJSON Transform Pipeline

```js
import { Transform, pipeline } from 'node:stream';
import { createReadStream, createWriteStream } from 'node:fs';

const parseNdjson = new Transform({
  objectMode: true,
  transform(chunk, _, cb) {
    for (const line of chunk.toString().split('\n')) {
      if (line.trim()) { cb(null, JSON.parse(line)); return; }
    } cb();
  },
});

const enrich = new Transform({
  objectMode: true,
  transform(r, _, cb) { cb(null, { ...r, processedAt: new Date().toISOString() }); },
});

const serialize = new Transform({
  objectMode: true,
  transform(r, _, cb) { cb(null, JSON.stringify(r) + '\n'); },
});

await pipeline(createReadStream('./raw.jsonl'), parseNdjson, enrich, serialize, createWriteStream('./enriched.jsonl'));
```

---

## 3. Streaming XML Parser

```bash
npm install sax
```

```js
import { createReadStream } from 'node:fs';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import sax from 'sax';

class XmlStreamParser extends Transform {
  constructor(tag) {
    super({ objectMode: true });
    this._parser = sax.createStream(true);
    this._current = null;
    this._parser.on('opentag', (n) => { if (n.name === tag) this._current = { $: n.attributes, _t: '' }; });
    this._parser.on('text', (t) => { if (this._current) this._current._t += t; });
    this._parser.on('closetag', (name) => {
      if (name === tag && this._current) { this.push(this._current); this._current = null; }
    });
    this._parser.on('error', (err) => this.destroy(err));
  }

  _transform(c, _, cb) { this._parser.write(c.toString()); cb(); }
  _flush(cb) { this._parser.close(); cb(); }
}

await pipeline(
  createReadStream('./catalog.xml'), new XmlStreamParser('record'),
  async function* (src) { for await (const r of src) console.log(r.$.id, r._t.trim()); },
);
```

---

## 4. Streaming YAML Parser

Process multi-document YAML by splitting on `---`:

```bash
npm install yaml
```

```js
import { createReadStream } from 'node:fs';
import { createInterface } from 'node:readline';
import { parse } from 'yaml';

async function* streamYaml(path) {
  const rl = createInterface({ input: createReadStream(path) });
  let lines = [];
  for await (const line of rl) {
    if (line.trim() === '---') { if (lines.length) yield parse(lines.join('\n')); lines = []; }
    else lines.push(line);
  }
  if (lines.length) yield parse(lines.join('\n'));
}

for await (const cfg of streamYaml('./configs.yml')) console.log(cfg.name, cfg.version);
```

### YAML→JSON Converter

```js
import { Transform, pipeline } from 'node:stream';
import { parse } from 'yaml';

const yaml2json = new Transform({
  transform(chunk, _, cb) {
    for (const doc of chunk.toString().split('---').filter((d) => d.trim()))
      this.push(JSON.stringify(parse(doc)) + '\n');
    cb();
  },
});

await pipeline(createReadStream('./multi.yml'), yaml2json, createWriteStream('./converted.jsonl'));
```

---

## 5. Multi-Format Detection and Routing

```js
import { Transform, PassThrough } from 'node:stream';

function detectFormat(header) {
  const s = header.toString('utf8', 0, 20).trimStart();
  if (s.startsWith('{') || s.startsWith('[')) return 'json';
  if (s.startsWith('<?xml') || s.startsWith('<')) return 'xml';
  if (/^\w+:/.test(s)) return 'yaml';
  return 'unknown';
}

function createFormatRouter() {
  const out = { json: new PassThrough(), xml: new PassThrough(), yaml: new PassThrough() };
  const router = new Transform({
    transform(chunk, _, cb) {
      if (!this._detected) {
        this._detected = true;
        out[detectFormat(chunk)]?.write(chunk);
      }
      cb();
    },
  });
  return { router, ...out };
}
```

---

## 6. Streaming Conversion: CSV → JSON → XML

```js
import { createReadStream, createWriteStream } from 'node:fs';
import { Transform, pipeline } from 'node:stream';

class CsvToJson extends Transform {
  constructor() {
    super({ objectMode: true });
    this._headers = null; this._buf = '';
  }

  _transform(chunk, _, cb) {
    this._buf += chunk.toString();
    const lines = this._buf.split('\n');
    this._buf = lines.pop();
    for (const line of lines) {
      if (!line.trim()) continue;
      const f = line.split(',').map((s) => s.trim());
      if (!this._headers) { this._headers = f; }
      else {
        const obj = {};
        this._headers.forEach((h, i) => (obj[h] = f[i]));
        this.push(obj);
      }
    } cb();
  }

  _flush(cb) {
    if (this._buf.trim() && this._headers) {
      const f = this._buf.split(',').map((s) => s.trim());
      const obj = {};
      this._headers.forEach((h, i) => (obj[h] = f[i]));
      this.push(obj);
    } cb();
  }
}

class JsonToXml extends Transform {
  constructor(root = 'records', item = 'record') {
    super({ objectMode: true });
    this.root = root; this.item = item; this._first = true;
  }

  _transform(obj, _, cb) {
    if (this._first) { this.push(`<?xml version="1.0"?>\n<${this.root}>\n`); this._first = false; }
    const inner = Object.entries(obj).map(([k, v]) => `<${k}>${String(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')}</${k}>`).join('');
    this.push(`  <${this.item}>${inner}</${this.item}>\n`);
    cb();
  }

  _flush(cb) { if (!this._first) this.push(`</${this.root}>\n`); cb(); }
}

await pipeline(createReadStream('./products.csv'), new CsvToJson(), new JsonToXml(), createWriteStream('./products.xml'));
```

---

## 7. Real-World: Streaming ETL Pipeline

```js
import { createReadStream, createWriteStream } from 'node:fs';
import { Transform, pipeline } from 'node:stream';

class EtlTransform extends Transform {
  constructor({ requiredFields, enrichFn }) {
    super({ objectMode: true });
    this.required = requiredFields;
    this.enrichFn = enrichFn;
    this.stats = { total: 0, valid: 0, invalid: 0 };
  }

  _transform(record, _, cb) {
    this.stats.total++;
    const missing = this.required.filter((f) => !(f in record));
    if (missing.length) {
      this.stats.invalid++;
      this.emit('validation-error', { record, missing });
      return cb();
    }
    this.stats.valid++;
    try { cb(null, this.enrichFn ? this.enrichFn(record) : record); }
    catch { cb(null, record); }
  }

  _flush(cb) { this.emit('stats', this.stats); cb(); }
}

const parseNdjson = new Transform({
  objectMode: true,
  transform(c, _, cb) {
    for (const line of c.toString().split('\n'))
      if (line.trim()) { try { cb(null, JSON.parse(line)); } catch {} return; }
    cb();
  },
});

const etl = new EtlTransform({
  requiredFields: ['id', 'name', 'email'],
  enrichFn: (r) => ({ ...r, processedAt: new Date().toISOString() }),
});

etl.on('stats', (s) => console.log('ETL:', s));
etl.on('validation-error', ({ record, missing }) => console.error(`Skipped ${record.id}: [${missing}]`));

const serialize = new Transform({ objectMode: true, transform(r, _, cb) { cb(null, JSON.stringify(r) + '\n'); } });

await pipeline(createReadStream('./raw.jsonl'), parseNdjson, etl, serialize, createWriteStream('./processed.jsonl'));
```

---

## Best Practices Checklist

- [ ] Use `stream-json` for files over 100 MB instead of `JSON.parse()`
- [ ] Prefer NDJSON over JSON arrays — each line is independently parseable
- [ ] Handle parse errors in transforms to prevent pipeline crashes
- [ ] `objectMode: true` when working with parsed objects
- [ ] Buffer incomplete lines at chunk boundaries for line-delimited formats
- [ ] `pipeline()` from `node:stream/promises` for error propagation and cleanup
- [ ] Detect format from magic bytes, not extensions
- [ ] Track ETL stats (total, valid, invalid) for monitoring
- [ ] Write invalid records to a separate error log for reprocessing
- [ ] Check `write()` return value for backpressure

---

## Cross-References

- [02-directory-streaming-advanced.md](./02-directory-streaming-advanced.md) — Streaming directory walks for multi-file ETL
- [02-tcp-udp-socket-streams.md](../05-stream-network-operations/02-tcp-udp-socket-streams.md) — Streaming data over TCP
- [03-http2-grpc-streaming.md](../05-stream-network-operations/03-http2-grpc-streaming.md) — HTTP/2 and gRPC streaming

---

## Next Steps

- Combine with [02-directory-streaming-advanced.md](./02-directory-streaming-advanced.md) for directory tree processing
- Move to [05-stream-network-operations](../05-stream-network-operations/) to stream parsed data over TCP/HTTP/2
- Benchmark `stream-json` vs `JSON.parse` on varying file sizes
- Implement a streaming CSV parser handling quoted fields and multiline values
