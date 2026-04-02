# Bundle Analysis

## What You'll Learn

- How to analyze your dependency bundle size
- Tools for visualizing bundle composition
- How to identify bloated dependencies
- How to reduce bundle size

## Why Analyze?

Understanding bundle size helps you:
- Identify bloated dependencies
- Find duplicate packages
- Optimize load times
- Reduce hosting costs

## Tools

### webpack-bundle-analyzer

```bash
npm install -D webpack-bundle-analyzer
```

```js
// webpack.config.js
import { BundleAnalyzerPlugin } from 'webpack-bundle-analyzer';

export default {
  plugins: [
    new BundleAnalyzerPlugin({
      analyzerMode: 'static',
      reportFilename: 'bundle-report.html',
    }),
  ],
};
```

### source-map-explorer

```bash
npm install -g source-map-explorer

# Analyze a built file
source-map-explorer dist/bundle.js
```

### bundlephobia (Online)

Visit [bundlephobia.com](https://bundlephobia.com) to check package sizes before installing:

```
https://bundlephobia.com/package/express@4.18.0
https://bundlephobia.com/package/lodash@4.17.21
```

## Analyzing with Node.js

```js
// analyze.js — List all dependencies and their sizes

import { readdirSync, statSync } from 'node:fs';
import { join } from 'node:path';

function getDirSize(dir) {
  let size = 0;
  try {
    for (const entry of readdirSync(dir, { withFileTypes: true })) {
      const fullPath = join(dir, entry.name);
      if (entry.isDirectory()) {
        size += getDirSize(fullPath);
      } else {
        size += statSync(fullPath).size;
      }
    }
  } catch {}
  return size;
}

const nodeModules = './node_modules';
const packages = readdirSync(nodeModules).filter((name) => !name.startsWith('.'));

const sizes = packages.map((name) => ({
  name,
  sizeMB: (getDirSize(join(nodeModules, name)) / 1024 / 1024).toFixed(2),
})).sort((a, b) => parseFloat(b.sizeMB) - parseFloat(a.sizeMB));

console.log('Top 20 largest packages:');
sizes.slice(0, 20).forEach((p, i) => {
  console.log(`  ${i + 1}. ${p.name}: ${p.sizeMB}MB`);
});
```

## Common Bloat Sources

| Package | Size | Lighter Alternative |
|---------|------|-------------------|
| moment | 300KB | date-fns (modular) or dayjs (2KB) |
| lodash | 70KB | lodash-es (tree-shakable) or native |
| axios | 30KB | native fetch or undici |
| express | 200KB | fastify or hono |

## Next Steps

For tree shaking, continue to [Tree Shaking](./02-tree-shaking.md).
