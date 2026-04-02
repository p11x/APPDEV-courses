---
title: "Rollup Bundle Configuration for Bootstrap"
topic: "Build Tools"
difficulty: 3
duration: "45 minutes"
prerequisites: ["ES Modules", "Rollup basics", "Sass compilation"]
tags: ["rollup", "bootstrap", "tree-shaking", "esm", "build-tools"]
---

## Overview

Rollup is a module bundler focused on ES module standards, producing lean, tree-shakeable output ideal for library distribution and production applications. When bundling Bootstrap 5 with Rollup, you benefit from efficient dead code elimination, clean ES module output, and minimal runtime overhead. Rollup excels at producing small bundles because it statically analyzes `import`/`export` statements, eliminating unused Bootstrap JavaScript components and CSS utility classes when properly configured.

Rollup requires explicit plugin configuration for SCSS processing, CSS extraction, and HTML generation — unlike Parcel or Vite. This makes setup more involved but provides granular control over the build pipeline. The primary use case for Rollup with Bootstrap is creating distributable component libraries or applications where bundle size is a critical concern.

## Basic Implementation

Install dependencies:

```bash
npm install bootstrap@5 @popperjs/core
npm install --save-dev rollup @rollup/plugin-node-resolve @rollup/plugin-commonjs
npm install --save-dev rollup-plugin-scss rollup-plugin-postcss sass
npm install --save-dev rollup-plugin-terser @rollup/plugin-html autoprefixer cssnano
```

Create `rollup.config.mjs`:

```js
import resolve from '@rollup/plugin-node-resolve';
import commonjs from '@rollup/plugin-commonjs';
import scss from 'rollup-plugin-scss';
import postcss from 'rollup-plugin-postcss';
import { terser } from 'rollup-plugin-terser';
import autoprefixer from 'autoprefixer';
import cssnano from 'cssnano';

const isProd = process.env.NODE_ENV === 'production';

export default {
  input: 'src/js/main.js',
  output: {
    dir: 'dist',
    format: 'es',
    entryFileNames: isProd ? 'js/[name].[hash].js' : 'js/[name].js',
    chunkFileNames: isProd ? 'js/[name].[hash].js' : 'js/[name].js',
    sourcemap: !isProd,
  },
  plugins: [
    resolve({
      browser: true,
      extensions: ['.js', '.mjs'],
    }),
    commonjs(),
    postcss({
      extract: 'css/bundle.css',
      modules: false,
      plugins: [autoprefixer(), ...(isProd ? [cssnano()] : [])],
    }),
    scss({
      fileName: 'css/custom.css',
      sourceMap: !isProd,
      sassOptions: {
        silenceDeprecations: ['import', 'global-builtin'],
      },
    }),
    ...(isProd ? [terser({ compress: { drop_console: true } })] : []),
  ],
  treeshake: {
    moduleSideEffects: (id) => {
      // Bootstrap SCSS files have side effects (CSS output)
      if (id.endsWith('.scss')) return true;
      return false;
    },
  },
};
```

Entry point `src/js/main.js`:

```js
import '../scss/custom.scss';

import Alert from 'bootstrap/js/dist/alert';
import Collapse from 'bootstrap/js/dist/collapse';
import Dropdown from 'bootstrap/js/dist/dropdown';
import Modal from 'bootstrap/js/dist/modal';
import Offcanvas from 'bootstrap/js/dist/offcanvas';

// Initialize modals
document.querySelectorAll('[data-bs-toggle="modal"]').forEach(el => {
  el.addEventListener('click', () => {
    const target = document.querySelector(el.dataset.bsTarget);
    if (target) new Modal(target).show();
  });
});
```

SCSS entry `src/scss/custom.scss`:

```scss
$primary: #7c3aed;
$secondary: #475569;

@import 'bootstrap/scss/bootstrap';
```

Build scripts in `package.json`:

```json
{
  "scripts": {
    "dev": "rollup -c --watch",
    "build": "NODE_ENV=production rollup -c"
  }
}
```

## Advanced Variations

### Multi-Output Library Build

For publishing a Bootstrap component library as both ESM and CJS:

```js
import resolve from '@rollup/plugin-node-resolve';
import commonjs from '@rollup/plugin-commonjs';
import postcss from 'rollup-plugin-postcss';
import { terser } from 'rollup-plugin-terser';
import { readFileSync } from 'fs';

const pkg = JSON.parse(readFileSync('./package.json', 'utf8'));

export default {
  input: 'src/index.js',
  external: ['bootstrap', '@popperjs/core'],
  output: [
    {
      file: pkg.main,
      format: 'cjs',
      exports: 'named',
      sourcemap: true,
    },
    {
      file: pkg.module,
      format: 'es',
      sourcemap: true,
    },
  ],
  plugins: [
    resolve(),
    commonjs(),
    postcss({
      extract: 'dist/styles.css',
      minimize: true,
    }),
  ],
  treeshake: {
    preset: 'smallest',
  },
};
```

```json
{
  "name": "@myorg/bootstrap-components",
  "main": "dist/index.cjs.js",
  "module": "dist/index.esm.js",
  "exports": {
    ".": {
      "import": "./dist/index.esm.js",
      "require": "./dist/index.cjs.js"
    },
    "./styles": "./dist/styles.css"
  }
}
```

### Code-Splitting with Dynamic Imports

```js
// src/js/router.js
export async function loadPage(page) {
  switch (page) {
    case 'dashboard':
      const { initDashboard } = await import('./pages/dashboard.js');
      initDashboard();
      break;
    case 'settings':
      const { initSettings } = await import('./pages/settings.js');
      initSettings();
      break;
  }
}
```

```js
// rollup.config.mjs — enable code splitting
export default {
  input: ['src/js/main.js', 'src/js/router.js'],
  output: {
    dir: 'dist',
    format: 'es',
    manualChunks: {
      bootstrap: ['bootstrap/js/dist/modal', 'bootstrap/js/dist/dropdown'],
      popper: ['@popperjs/core'],
    },
  },
};
```

### Custom Rollup Plugin for Bootstrap Purging

```js
// rollup-plugin-purge-bootstrap.mjs
import { createFilter } from '@rollup/pluginutils';

export default function purgeBootstrap(options = {}) {
  const filter = createFilter(options.include || ['**/*.css']);

  return {
    name: 'purge-bootstrap',
    generateBundle(_, bundle) {
      for (const [fileName, chunk] of Object.entries(bundle)) {
        if (chunk.type === 'asset' && fileName.endsWith('.css') && filter(fileName)) {
          let css = typeof chunk.source === 'string' ? chunk.source : chunk.source.toString();
          // Remove unused responsive utility classes
          const usedBreakpoints = options.breakpoints || ['sm', 'md', 'lg'];
          const allBreakpoints = ['sm', 'md', 'lg', 'xl', 'xxl'];
          allBreakpoints.forEach(bp => {
            if (!usedBreakpoints.includes(bp)) {
              const regex = new RegExp(`\\.[\\w-]+-${bp}-[\\w-]+[^{]*\\{[^}]*\\}`, 'g');
              css = css.replace(regex, '');
            }
          });
          chunk.source = css;
        }
      }
    },
  };
}
```

## Best Practices

1. **Use `format: 'es'`** for output to leverage native tree-shaking and produce smaller bundles.
2. **Mark `bootstrap` and `@popperjs/core` as `external`** when building libraries to avoid bundling peer dependencies.
3. **Configure `treeshake.moduleSideEffects`** correctly — Bootstrap SCSS files must be treated as having side effects.
4. **Use `rollup-plugin-postcss`** with `extract: true` to separate CSS from JavaScript bundles.
5. **Import individual Bootstrap JS modules** (`bootstrap/js/dist/modal`) rather than the full bundle.
6. **Set `output.manualChunks`** to split Bootstrap into a separate vendor chunk for caching.
7. **Use `.mjs` extension** for Rollup config to ensure ES module resolution without `"type": "module"` in package.json.
8. **Enable `sourcemap: true`** in development and `terser` in production only.
9. **Use `@rollup/plugin-node-resolve`** with `browser: true` to prefer browser-compatible module variants.
10. **Define `external` dependencies** to prevent bundling packages that consumers will provide.
11. **Use `rollup-plugin-terser`** (not standalone `terser`) for Rollup-compatible minification in the plugin pipeline.

## Common Pitfalls

1. **Missing `@rollup/plugin-commonjs`** causes errors when Rollup encounters CommonJS modules in Bootstrap's dependency tree.
2. **Incorrect `treeshake.moduleSideEffects`** configuration can strip Bootstrap's CSS output entirely if SCSS files are marked as side-effect free.
3. **Not setting `external`** for library builds results in bundled copies of Bootstrap in every consumer package.
4. **Using `output.file` with code-splitting** — code splitting requires `output.dir` instead.
5. **Forgetting `autoprefixer`** means Bootstrap's CSS vendor prefixes are missing, breaking layout in older browsers.
6. **Config file using `require()`** fails if Rollup config is `.mjs` or `"type": "module"` is set. Use `import` syntax.

## Accessibility Considerations

Rollup's tree-shaking analyzes JavaScript imports statically, so Bootstrap's ARIA-related initialization code is preserved when imported modules reference it. Ensure that tree-shaking does not remove Bootstrap's keyboard navigation handlers for modals, dropdowns, and offcanvas components. When using `manualChunks`, verify that focus-trapping code for modals remains in the correct chunk. PostCSS processing with cssnano preserves accessibility media queries like `prefers-reduced-motion` and `prefers-contrast` by default. Test the production bundle to confirm that Bootstrap's `aria-*` attributes in the HTML are not affected by any custom Rollup plugins.

## Responsive Behavior

Rollup's SCSS plugin processes Bootstrap's responsive breakpoint mixins identically to a standalone Sass compiler. All five breakpoints (`sm`, `md`, `lg`, `xl`, `xxl`) and their associated utility classes are generated during compilation. The `postcss` plugin with `autoprefixer` adds vendor prefixes to any CSS features used in Bootstrap's responsive utilities. When using code-splitting, ensure that responsive utility CSS is included in the shared/common chunk rather than page-specific chunks, since responsive classes like `.d-md-flex` and `.col-lg-6` are used across routes. The `treeshake` configuration should exclude `.scss` files from side-effect analysis to prevent removal of responsive utility classes that may appear unused in static analysis.