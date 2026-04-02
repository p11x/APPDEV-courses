# WebAssembly Bundling

## What You'll Learn

- Bundling WASM with JavaScript bundlers
- Vite, Webpack, Rollup WASM support
- Code splitting with WASM
- Asset handling and optimization

---

## Layer 1: Bundler Integration

### Vite Configuration

```typescript
// vite.config.ts
import { defineConfig } from 'vite';

export default defineConfig({
  optimizeDeps: {
    exclude: ['@my-org/wasm-crypto']
  },
  build: {
    target: 'esnext',
    rollupOptions: {
      output: {
        manualChunks: {
          wasm: ['@my-org/wasm-crypto']
        }
      }
    }
  }
});
```

### Webpack Configuration

```javascript
// webpack.config.js
module.exports = {
  experiments: {
    asyncWebAssembly: true,
    syncWebAssembly: true
  },
  module: {
    rules: [
      {
        test: /\.wasm$/,
        type: 'asset/resource'
      }
    ]
  }
};
```

---

## Layer 2: Asset Handling

### Loading Strategies

```typescript
// Dynamic import
const wasmModule = await import('./pkg/my_wasm');
await wasmModule.default();

// With fallback
async function loadWasm() {
  try {
    return await import('./pkg/my_wasm');
  } catch (e) {
    console.warn('WASM not supported, using JS fallback');
    return await import('./fallback/js-implementation');
  }
}
```

---

## Next Steps

Continue to [WASM Testing](./14-wasm-testing.md)