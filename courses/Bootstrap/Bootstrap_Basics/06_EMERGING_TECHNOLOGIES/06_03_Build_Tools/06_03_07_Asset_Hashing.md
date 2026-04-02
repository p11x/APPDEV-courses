---
title: "Asset Hashing and Cache Busting for Bootstrap"
topic: "Build Tools"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Build tool basics (Webpack/Vite/Rollup)", "HTTP caching concepts"]
tags: ["asset-hashing", "cache-busting", "content-hash", "manifest", "build-tools"]
---

## Overview

Asset hashing appends a content-based hash to filenames (e.g., `custom.a1b2c3d4.css`) enabling aggressive browser caching while ensuring users receive updated files when content changes. When a file's contents change, its hash changes, producing a new filename that bypasses cached versions. For Bootstrap 5 projects, this applies to compiled CSS bundles, JavaScript chunks, fonts, and images.

The strategy is: set long-lived `Cache-Control` headers on hashed assets (`max-age=31536000, immutable`) and short-lived headers on the HTML file that references them. When a new deploy changes a CSS file, the HTML references the new hash and the browser fetches the fresh asset. Manifest files (like `manifest.json` or `asset-manifest.json`) map original filenames to their hashed counterparts for programmatic lookup.

## Basic Implementation

### Webpack Content Hashing

Webpack natively supports content hashing via filename templates:

```js
// webpack.config.js
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
  output: {
    filename: 'js/[name].[contenthash:8].js',
    chunkFilename: 'js/[name].[contenthash:8].chunk.js',
    assetModuleFilename: 'assets/[hash:8][ext][query]',
    clean: true,
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: 'css/[name].[contenthash:8].css',
      chunkFilename: 'css/[id].[contenthash:8].css',
    }),
  ],
};
```

### Vite Content Hashing

Vite applies content hashes by default in production builds:

```js
// vite.config.js
export default {
  build: {
    rollupOptions: {
      output: {
        entryFileNames: 'assets/js/[name]-[hash].js',
        chunkFileNames: 'assets/js/[name]-[hash].js',
        assetFileNames: 'assets/[ext]/[name]-[hash].[ext]',
      },
    },
  },
};
```

### Manifest Generation

Webpack generates a manifest with the `webpack-manifest-plugin`:

```bash
npm install --save-dev webpack-manifest-plugin
```

```js
// webpack.config.js
const { WebpackManifestPlugin } = require('webpack-manifest-plugin');

module.exports = {
  plugins: [
    new WebpackManifestPlugin({
      fileName: 'asset-manifest.json',
      filter: (file) => !file.name.endsWith('.map'),
      generate: (seed, files) => {
        const manifest = {};
        files.forEach(file => {
          manifest[file.name] = file.path;
        });
        return manifest;
      },
    }),
  ],
};
```

Output `asset-manifest.json`:

```json
{
  "main.js": "/js/main.a3f8b2c1.js",
  "main.css": "/css/main.d4e5f6a7.css",
  "bootstrap.js": "/js/bootstrap.e8f9a0b1.js",
  "logo.png": "/assets/images/logo.c2d3e4f5.png",
  "inter.woff2": "/assets/fonts/inter.a1b2c3d4.woff2"
}
```

## Advanced Variations

### Server-Side Manifest Integration

Use the manifest file in server-rendered applications to inject correct hashed asset URLs:

```js
// express-server.js
const express = require('express');
const { readFileSync } = require('fs');
const { resolve } = require('path');

const app = express();
const manifest = JSON.parse(
  readFileSync(resolve(__dirname, 'dist/asset-manifest.json'), 'utf8')
);

app.get('/', (req, res) => {
  res.send(`
    <!DOCTYPE html>
    <html lang="en">
    <head>
      <meta charset="UTF-8">
      <meta name="viewport" content="width=device-width, initial-scale=1.0">
      <link rel="stylesheet" href="${manifest['main.css']}">
      <title>Bootstrap App</title>
    </head>
    <body>
      <div id="app">
        <div class="container py-5">
          <h1>Server-rendered Bootstrap</h1>
        </div>
      </div>
      <script src="${manifest['main.js']}"></script>
    </body>
    </html>
  `);
});

app.use(express.static('dist', {
  maxAge: '1y',
  immutable: true,
  etag: false,
}));

app.listen(3000);
```

### Long-Term Caching with Webpack

Configure Webpack for optimal long-term caching with runtime chunk separation:

```js
// webpack.config.js
module.exports = {
  output: {
    filename: 'js/[name].[contenthash:8].js',
    clean: true,
  },
  optimization: {
    runtimeChunk: 'single',
    moduleIds: 'deterministic',
    chunkIds: 'deterministic',
    splitChunks: {
      cacheGroups: {
        bootstrap: {
          test: /[\\/]node_modules[\\/]bootstrap[\\/]/,
          name: 'bootstrap',
          chunks: 'all',
          priority: 20,
        },
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendor',
          chunks: 'all',
          priority: 10,
        },
        common: {
          minChunks: 2,
          chunks: 'all',
          name: 'common',
          priority: 5,
          reuseExistingChunk: true,
        },
      },
    },
  },
};
```

### Vite Plugin for Custom Manifest

```js
// vite-plugin-manifest.mjs
export function manifestPlugin() {
  return {
    name: 'vite-manifest',
    generateBundle(options, bundle) {
      const manifest = {};
      for (const [fileName, chunk] of Object.entries(bundle)) {
        const name = chunk.name || fileName;
        manifest[name] = {
          file: fileName,
          type: chunk.type,
          isEntry: chunk.isEntry || false,
          isDynamicEntry: chunk.isDynamicEntry || false,
        };
      }
      this.emitFile({
        type: 'asset',
        fileName: 'manifest.json',
        source: JSON.stringify(manifest, null, 2),
      });
    },
  };
}
```

### Cache-Control Headers Configuration

```nginx
# nginx.conf
location ~* \.(css|js|woff2?|png|jpe?g|gif|svg|webp|ico)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
    access_log off;
}

location = /index.html {
    expires -1;
    add_header Cache-Control "no-cache, no-store, must-revalidate";
}

location / {
    try_files $uri $uri/ /index.html;
}
```

```json
// netlify/_headers
/*
  Cache-Control: public, max-age=0, must-revalidate

/*.css
  Cache-Control: public, max-age=31536000, immutable

/*.js
  Cache-Control: public, max-age=31536000, immutable

/*.woff2
  Cache-Control: public, max-age=31536000, immutable
```

## Best Practices

1. **Use `contenthash`** (not `hash` or `chunkhash`) so each file's hash depends only on its own content, not the build session.
2. **Set `moduleIds: 'deterministic'`** in Webpack to prevent module ID changes from cascading hash changes across unrelated files.
3. **Enable `clean: true`** in output config to remove old hashed files from previous builds.
4. **Set `Cache-Control: immutable` with `max-age=31536000`** (1 year) on hashed assets since their filenames are unique.
5. **Set `Cache-Control: no-cache`** on HTML files so browsers always revalidate and pick up new asset hashes.
6. **Generate a manifest file** mapping original names to hashed names for server-side template rendering.
7. **Separate the Webpack runtime chunk** (`runtimeChunk: 'single'`) to prevent runtime changes from invalidating vendor chunk hashes.
8. **Use deterministic chunk IDs** to ensure the same source code always produces the same hashes across builds.
9. **Exclude source maps** from the manifest to keep it focused on user-facing assets.
10. **Test cache invalidation** by changing one file and verifying only its hash changes, not sibling files.

## Common Pitfalls

1. **Using `[hash]` instead of `[contenthash]`** causes all files to share the same hash, invalidating the entire cache on any change.
2. **Not enabling `clean: true`** accumulates old hashed files in `dist/`, bloating deployment size over time.
3. **Setting long `max-age` on HTML** prevents browsers from discovering new asset hashes after deployment.
4. **Missing `deterministic` module/chunk IDs** causes hash instability between builds with identical source code.
5. **Hardcoded hashed filenames in HTML** break when content changes and hashes update — always use the build tool's HTML plugin.

## Accessibility Considerations

Asset hashing does not affect accessibility — the hashed files contain identical content. However, when using manifest-driven HTML generation, ensure that dynamically injected `<link>` and `<script>` tags include proper `rel`, `type`, and `crossorigin` attributes. Font preloading (`<link rel="preload" as="font">`) must reference the hashed font filename from the manifest. Verify that assistive technologies are not disrupted by rapid asset replacement during hot updates in development.

## Responsive Behavior

Responsive CSS files retain their content hashes regardless of the number of media queries or breakpoints they contain. When Bootstrap's responsive utilities change (e.g., adding custom breakpoints), only the CSS file hash updates. JavaScript chunks containing responsive logic (e.g., resize observers, matchMedia handlers) are hashed independently. The `splitChunks` configuration ensures that Bootstrap's core responsive CSS remains in a stable vendor chunk, preserving its hash across application-level code changes and maximizing cache retention for returning visitors.