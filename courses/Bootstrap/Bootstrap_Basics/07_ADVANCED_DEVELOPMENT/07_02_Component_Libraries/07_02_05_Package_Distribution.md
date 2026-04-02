---
title: "Package Distribution"
difficulty: 2
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - npm Publishing
  - CDN Configuration
  - ES Module Syntax
---

## Overview

Distributing a Bootstrap component library involves publishing to npm for bundled consumption, serving via CDN for direct inclusion, and ensuring tree-shakeable exports so consumers only include what they use. Distribution strategy affects build size, load times, and developer experience across different integration patterns.

npm distribution supports both CommonJS and ES module formats with subpath exports for granular imports. CDN distribution provides pre-built CSS and JS bundles for projects without build tools. Tree-shaking requires ESM exports, the `sideEffects` field in package.json, and avoiding side-effectful top-level code.

## Basic Implementation

```json
// package.json
{
  "name": "@company/bootstrap-components",
  "version": "1.0.0",
  "main": "./dist/cjs/index.js",
  "module": "./dist/esm/index.js",
  "types": "./dist/types/index.d.ts",
  "exports": {
    ".": {
      "import": "./dist/esm/index.js",
      "require": "./dist/cjs/index.js",
      "types": "./dist/types/index.d.ts"
    },
    "./css": "./dist/css/components.min.css",
    "./scss/*": "./scss/*",
    "./components/*": {
      "import": "./dist/esm/components/*.js",
      "types": "./dist/types/components/*.d.ts"
    }
  },
  "sideEffects": ["**/*.css", "**/*.scss"],
  "files": ["dist/", "scss/", "types/"]
}
```

```js
// rollup.config.js
import resolve from '@rollup/plugin-node-resolve';
import sass from 'rollup-plugin-sass';
import terser from '@rollup/plugin-terser';

export default {
  input: 'src/index.js',
  output: [
    { file: 'dist/cjs/index.js', format: 'cjs', exports: 'named' },
    { file: 'dist/esm/index.js', format: 'es' }
  ],
  external: ['bootstrap'],
  plugins: [resolve(), sass({ output: 'dist/css/components.css' })]
};
```

```html
<!-- CDN Usage -->
<!DOCTYPE html>
<html lang="en">
<head>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
  <link href="https://cdn.jsdelivr.net/npm/@company/bootstrap-components@1.0.0/dist/css/components.min.css" rel="stylesheet">
</head>
<body>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
  <script type="module">
    import { DataTable } from 'https://cdn.jsdelivr.net/npm/@company/bootstrap-components@1.0.0/dist/esm/index.js';
    DataTable.getOrCreateInstance('#myTable');
  </script>
</body>
</html>
```

## Advanced Variations

```js
// Dual CJS/ESM build with TypeScript
// tsconfig.build.json
{
  "compilerOptions": {
    "declaration": true,
    "declarationDir": "dist/types",
    "emitDeclarationOnly": true
  },
  "include": ["src"]
}
```

```js
// jsDelivr CDN configuration (auto-updates from npm)
// Users can pin versions or use latest
// https://cdn.jsdelivr.net/npm/@company/bootstrap-components@1/dist/css/components.min.css
```

## Best Practices

1. **Ship both CJS and ESM** - CJS for Node/webpack, ESM for tree-shaking.
2. **Use subpath exports** - Allow `import X from '@lib/components/X'` for better tree-shaking.
3. **Set sideEffects correctly** - Mark CSS/SCSS files as side-effectful so bundlers don't drop them.
4. **Minify production builds** - Provide `.min.css` and `.min.js` for CDN usage.
5. **Include source maps** - Ship `.map` files for debugging in production.
6. **Pin CDN versions** - Recommend exact version URLs, not `@latest`.
7. **Test with major bundlers** - Verify webpack, Vite, Rollup, and esbuild all work.
8. **Provide UMD as fallback** - For legacy systems without module support.
9. **Document all entry points** - List what each export path provides.
10. **Automate publishing** - CI/CD should publish on tag, not manual `npm publish`.

## Common Pitfalls

1. **Dual package hazard** - CJS and ESM versions of the same module loaded simultaneously create duplicate instances.
2. **Missing sideEffects** - Bundlers tree-shake entire components that have CSS side effects.
3. **CDN cache issues** - Not versioning CDN assets causes stale cache problems.
4. **Large bundle sizes** - Not splitting code by component leads to monolithic downloads.
5. **Missing types** - Not shipping `.d.ts` files breaks TypeScript consumers.

## Accessibility Considerations

CDN-served components must include minified but accessible markup. Never remove ARIA attributes during minification. Test CDN builds with screen readers.

## Responsive Behavior

Pre-built CSS bundles should include all responsive utilities. CDN users expect full Bootstrap grid support without build-time tree-shaking.

```html
<!-- CDN build includes all responsive classes -->
<div class="row">
  <div class="col-12 col-md-6 col-lg-4">
    <!-- Works at all breakpoints out of the box -->
  </div>
</div>
```
