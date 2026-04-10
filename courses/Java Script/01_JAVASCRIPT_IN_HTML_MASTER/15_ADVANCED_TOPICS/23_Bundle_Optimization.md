# 📦 Bundle Optimization

## 📋 Overview

Optimizing JavaScript bundles is crucial for fast load times. This guide covers techniques to reduce bundle size and improve performance.

---

## 🎯 Bundle Analysis

### Analyzing Bundle

```javascript
// Use webpack-bundle-analyzer
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer')
    .BundleAnalyzerPlugin;

module.exports = {
    plugins: [
        new BundleAnalyzerPlugin()
    ]
};
```

### Common Size Bloaters

```javascript
// 1. Large libraries
import _ from 'lodash'; // Full lodash - 70kb+
// ✅ Better: import only what you need
import debounce from 'lodash/debounce';

// 2. Duplicate code
// Problem: Multiple copies of same package
// Solution: Deduplication in bundler

// 3. Unused code
// Use tree-shaking to remove
import { button, input } from '@my-ui';
// If 'input' not used, tree-shake removes it
```

---

## 🎯 Optimization Techniques

### Code Splitting

```javascript
// Dynamic imports - load on demand
const Modal = () => import('./Modal.js');

// Route-based splitting
const routes = [
    { path: '/', component: () => import('./Home.js') },
    { path: '/about', component: () => import('./About.js') },
    { path: '/dashboard', component: () => import('./Dashboard.js') }
];
```

### Tree Shaking

```javascript
// package.json - enable sideEffects
{
    "sideEffects": false
}

// ES6 imports - tree-shakeable
import { add, multiply } from './math.js';

// Unused 'multiply' will be removed
```

### Compression

```javascript
// Gzip compression (server-side)
const compression = require('compression');
app.use(compression());

// Brotli - better than Gzip
// Modern browsers support
```

---

## 🔗 Related Topics

- [22_Memory_Management.md](./22_Memory_Management.md)
- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)

---

**Next: [Web Workers Master](./25_Web_Workers_Master.md)**