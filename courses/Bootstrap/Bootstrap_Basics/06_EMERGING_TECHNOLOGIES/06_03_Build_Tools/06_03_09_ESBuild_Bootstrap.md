---
title: "esbuild + Bootstrap SCSS"
slug: "esbuild-bootstrap"
difficulty: 2
tags: ["bootstrap", "esbuild", "scss", "build", "bundler"]
prerequisites:
  - "06_03_01_Vite_Setup"
  - "06_03_02_Webpack_Setup"
related:
  - "06_03_10_Turbopack_Setup"
  - "06_03_11_CDN_Bundlers"
duration: "30 minutes"
---

# esbuild + Bootstrap SCSS

## Overview

esbuild is an extremely fast JavaScript and CSS bundler written in Go. It compiles Bootstrap SCSS, bundles JavaScript, and produces optimized production builds in milliseconds rather than seconds. While esbuild does not natively process SCSS, it integrates with the `sass` package via plugins or preprocessing. This combination delivers near-instant rebuilds during development and lightning-fast production builds, making it ideal for Bootstrap projects that prioritize build speed.

## Basic Implementation

Configure esbuild to compile Bootstrap SCSS and bundle application JavaScript.

```bash
npm install bootstrap bootstrap-icons esbuild sass --save-dev
```

```javascript
// build.mjs
import * as esbuild from 'esbuild';
import { execSync } from 'child_process';

// Compile SCSS with sass first
execSync('sass src/scss/main.scss dist/css/main.css --style=compressed', { stdio: 'inherit' });

await esbuild.build({
  entryPoints: ['src/js/main.js'],
  bundle: true,
  minify: true,
  sourcemap: true,
  outdir: 'dist/js',
  target: ['es2020'],
  loader: {
    '.js': 'jsx',
  },
});
```

```scss
// src/scss/main.scss
@import "bootstrap/scss/bootstrap";
@import "bootstrap-icons/font/bootstrap-icons.css";

// Custom overrides
$primary: #6366f1;
$enable-rounded: true;
$enable-shadows: true;
```

```html
<!-- index.html -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link rel="stylesheet" href="dist/css/main.css">
</head>
<body>
  <div class="container mt-4">
    <h1 class="text-primary">esbuild + Bootstrap</h1>
    <button class="btn btn-primary" id="demoBtn">Click Me</button>
  </div>
  <script src="dist/js/main.js"></script>
</body>
</html>
```

```json
// package.json scripts
{
  "scripts": {
    "build": "node build.mjs",
    "dev": "node build.mjs --watch"
  }
}
```

## Advanced Variations

### esbuild Plugin for SCSS

Use an esbuild plugin to handle SCSS compilation inline during the build.

```javascript
// build-with-plugin.mjs
import * as esbuild from 'esbuild';
import * as sass from 'sass';
import { dirname } from 'path';

const sassPlugin = {
  name: 'sass',
  setup(build) {
    build.onLoad({ filter: /\.scss$/ }, async (args) => {
      const result = sass.compile(args.path, {
        loadPaths: [dirname(args.path), 'node_modules'],
        style: 'compressed',
      });
      return {
        contents: result.css.toString(),
        loader: 'css',
      };
    });
  },
};

await esbuild.build({
  entryPoints: ['src/js/main.js'],
  bundle: true,
  minify: true,
  outdir: 'dist/js',
  plugins: [sassPlugin],
  loader: {
    '.woff2': 'file',
    '.woff': 'file',
    '.ttf': 'file',
    '.eot': 'file',
    '.svg': 'file',
  },
});
```

### Watch Mode with Live Rebuild

Development server with esbuild watch mode and automatic SCSS recompilation.

```javascript
// dev.mjs
import * as esbuild from 'esbuild';
import { createServer } from 'http';
import { readFileSync, existsSync } from 'fs';
import { exec } from 'child_process';

const ctx = await esbuild.context({
  entryPoints: ['src/js/main.js'],
  bundle: true,
  sourcemap: true,
  outdir: 'dist/js',
  target: ['es2020'],
});

await ctx.watch();

function compileSass() {
  exec('sass src/scss/main.scss dist/css/main.css', (err) => {
    if (err) console.error('SCSS compilation error:', err.message);
  });
}
compileSass();

// Simple file watcher for SCSS
import { watch } from 'fs';
watch('src/scss', { recursive: true }, () => compileSass());

createServer((req, res) => {
  let filePath = `dist${req.url === '/' ? '/index.html' : req.url}`;
  if (existsSync(filePath)) {
    res.writeHead(200, {
      'Content-Type': filePath.endsWith('.css') ? 'text/css' :
                       filePath.endsWith('.js') ? 'application/javascript' : 'text/html'
    });
    res.end(readFileSync(filePath));
  } else {
    res.writeHead(404);
    res.end('Not found');
  }
}).listen(3000, () => console.log('Dev server on http://localhost:3000'));
```

### Bootstrap Tree-Shaking

Import only the Bootstrap modules you need to reduce bundle size.

```scss
// src/scss/main.scss - Selective imports
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/root";
@import "bootstrap/scss/reboot";
@import "bootstrap/scss/type";
@import "bootstrap/scss/grid";
@import "bootstrap/scss/buttons";
@import "bootstrap/scss/card";
@import "bootstrap/scss/modal";
// Skip: carousel, accordion, offcanvas, etc.
```

```javascript
// src/js/main.js - Selective JS imports
import 'bootstrap/js/dist/util';
import 'bootstrap/js/dist/modal';
import 'bootstrap/js/dist/dropdown';
import 'bootstrap/js/dist/toast';
// Skip unused components
```

## Best Practices

1. Use `sass` (Dart Sass) as the SCSS compiler, not `node-sass` which is deprecated
2. Enable `--style=compressed` for production CSS to minimize file size
3. Use sourcemaps in development (`sourcemap: true`) but disable in production
4. Import only needed Bootstrap SCSS partials to reduce CSS bundle size
5. Use `esbuild.build()` for one-off builds and `esbuild.context().watch()` for development
6. Set the `target` option to match your browser support requirements
7. Configure `external` for packages that should not be bundled (e.g., CDN-loaded libraries)
8. Use the `define` option to replace environment variables at build time
9. Split vendor code into a separate chunk with `splitting: true` for better caching
10. Monitor build output size with `--metafile` flag for bundle analysis
11. Use `loader` configuration for non-JS assets like fonts and images
12. Keep the `node_modules` directory in `loadPaths` for SCSS imports
13. Use `npm-run-all` to parallelize SCSS and JS builds
14. Test builds in CI with `--minify --sourcemap` flags matching production

## Common Pitfalls

1. **SCSS not compiling**: Forgetting that esbuild needs a separate SCSS pipeline or plugin
2. **Font paths broken**: Bootstrap icon font paths not resolving after bundling
3. **Missing loadPaths**: SCSS imports failing because `node_modules` is not in load paths
4. **No sourcemaps**: Debugging minified output without source maps in development
5. **Watch mode not detecting SCSS**: esbuild watch only tracks JS imports, not SCSS files
6. **Large bundles**: Importing all of Bootstrap JS when only a few components are used
7. **CSS not injecting in dev**: Forgetting to link compiled CSS in HTML during development

## Accessibility Considerations

Build configuration does not directly affect accessibility, but ensure the compiled CSS preserves Bootstrap's focus styles and ARIA-related classes. Do not strip `:focus-visible` rules during CSS minification. Verify that tree-shaken CSS still includes skip-link, `.visually-hidden`, and screen reader utility classes. Test the built output with screen readers to ensure no accessibility-related styles were removed.

## Responsive Behavior

Build output is static and does not change responsive behavior. Bootstrap's responsive grid and utility classes compile correctly regardless of the bundler. Ensure the SCSS build preserves all responsive breakpoint mixins (`@media-breakpoint-up`, `@media-breakpoint-down`). Verify that `@include media-breakpoint-up(md)` generates correct media queries in the compiled output. Test the production build across breakpoints to confirm responsive utilities work identically to development.
