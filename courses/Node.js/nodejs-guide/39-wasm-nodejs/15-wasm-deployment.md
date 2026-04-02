# WebAssembly Deployment

## What You'll Learn

- Deploying WASM modules to production
- CDN and edge deployment strategies
- Version management
- Rollback strategies

---

## Layer 1: Deployment Strategies

### Node.js Deployment

```dockerfile
# Dockerfile
FROM node:20-alpine AS builder

# Install Rust
RUN apk add --no-cache rust cargo wasm-pack

WORKDIR /app
COPY . .
RUN npm run build:wasm

# Production image
FROM node:20-alpine
WORKDIR /app
COPY --from=builder /app/pkg ./pkg
COPY --from=builder /app/dist ./dist
COPY package*.json ./

RUN npm ci --production

CMD ["node", "dist/index.js"]
```

### Edge Deployment

```typescript
// edge/wasm-handler.ts
export default {
  async fetch(request: Request): Promise<Response> {
    const wasm = await loadWasmModule();
    const result = wasm.processRequest(await request.text());
    return new Response(JSON.stringify(result));
  }
};
```

---

## Layer 2: Version Management

### Semantic Versioning

```bash
# Build with version
wasm-pack build --version 1.2.3 --out-dir pkg
npm publish
```

---

## Next Steps

Continue to [WASM Case Studies](./16-wasm-case-studies.md)