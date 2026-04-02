# Bun vs Node.js

## What You'll Learn

- Performance benchmarks between Bun and Node.js
- API compatibility differences
- When to choose Bun over Node.js
- Migration considerations

## Performance Benchmarks

### HTTP Server

```ts
// Node.js (with node:http)
import { createServer } from 'node:http';
createServer((req, res) => {
  res.writeHead(200);
  res.end('Hello');
}).listen(3000);

// Bun (with Bun.serve)
Bun.serve({
  port: 3000,
  fetch() {
    return new Response('Hello');
  },
});
```

| Metric | Node.js 22 | Bun 1.1 |
|--------|-----------|---------|
| Requests/sec | 55,000 | 110,000 |
| Latency (p50) | 1.8ms | 0.9ms |
| Latency (p99) | 8ms | 3ms |
| Startup time | 40ms | 6ms |
| Memory (idle) | 30MB | 18MB |

### Package Installation

| Package count | npm | bun |
|--------------|-----|-----|
| 0 → 100 deps | 8s | 0.5s |
| 0 → 500 deps | 25s | 2s |
| 0 → 1000 deps | 60s | 4s |

### File I/O

```ts
// Read a 10MB file

// Node.js
import { readFile } from 'node:fs/promises';
const data = await readFile('./big-file.txt');  // ~15ms

// Bun
const file = Bun.file('./big-file.txt');
const data = await file.text();  // ~5ms
```

## API Differences

| Feature | Node.js | Bun |
|---------|---------|-----|
| Package manager | `npm` / `yarn` / `pnpm` | `bun install` |
| TypeScript | `tsc` + `ts-node` or `tsx` | Built-in |
| .env loading | `dotenv` package | Built-in |
| SQLite | `better-sqlite3` (native) | `bun:sqlite` (built-in) |
| WebSocket server | `ws` package | `Bun.serve` (built-in) |
| Bundler | webpack/esbuild/rollup | `bun build` (built-in) |
| Test runner | `node:test` | `bun:test` |
| Hot reload | `node --watch` | `bun --hot` |

## When to Choose Bun

```
Is your project new?
├── Yes → Does it need maximum performance?
│   ├── Yes → Bun is a strong choice
│   └── No → Both work; Bun is simpler to set up
└── No → Does it use native C++ addons?
    ├── Yes → Test compatibility first; stay on Node.js if issues
    └── No → Try Bun; most npm packages work
```

## When to Stay on Node.js

- **Native addons** that don't compile for Bun (e.g., some `sharp` versions, `canvas`)
- **Enterprise environments** requiring LTS guarantees (Node.js LTS is more established)
- **Production stability** — Node.js has 15+ years of production hardening
- **Ecosystem maturity** — some libraries have edge-case bugs on Bun
- **Deno Deploy / Cloudflare Workers** — neither uses Bun or Node.js

## Migration Strategy

```bash
# 1. Install Bun
curl -fsSL https://bun.sh/install | bash

# 2. Install dependencies with Bun
bun install  # Reads package.json, creates bun.lockb

# 3. Run your app
bun run server.ts  # Runs TypeScript directly

# 4. Run tests
bun test

# 5. If issues, fall back to Node.js
node server.ts  # Still works if you used standard APIs
```

## Common Mistakes

### Mistake 1: Blind Migration Without Testing

```bash
# WRONG — migrate and deploy without testing
bun install && bun run build && bun run deploy

# CORRECT — test thoroughly before production
bun install
bun test              # Run full test suite
bun run build         # Verify build works
bun run server.ts     # Test locally
# Deploy only after verification
```

### Mistake 2: Using Node.js-Specific Globals

```ts
// WRONG — __dirname and __filename don't exist in Bun (like ESM in Node.js)
console.log(__dirname);  // ReferenceError

// CORRECT — use import.meta
import { fileURLToPath } from 'node:url';
import { dirname } from 'node:path';
const __dirname = dirname(fileURLToPath(import.meta.url));
```

## Next Steps

For Bun as a package manager, continue to [Bun Package Manager](./03-bun-package-manager.md).
