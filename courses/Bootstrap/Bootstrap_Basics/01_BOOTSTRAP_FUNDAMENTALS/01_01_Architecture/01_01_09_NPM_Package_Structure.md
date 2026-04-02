---
title: "NPM Package Structure"
lesson: "01_01_09"
difficulty: "2"
topics: ["npm", "package", "dist", "src", "peer-dependencies", "imports"]
estimated_time: "25 minutes"
---

# NPM Package Structure

## Overview

Bootstrap is distributed as an npm package (`bootstrap`) that contains precompiled assets, source files, and JavaScript modules. Understanding the package structure helps you choose the right files for your project and avoid bloated bundles. The npm package is the recommended way to include Bootstrap in modern build workflows, offering full access to source SCSS files, compiled CSS, JavaScript modules, and all component plugins.

When you install Bootstrap via `npm install bootstrap`, you receive a comprehensive directory structure with everything needed for both simple and advanced usage scenarios. The package ships with both compiled distributions and raw source code, giving developers flexibility in how they integrate the framework.

## Basic Implementation

### Installing Bootstrap via npm

```bash
npm install bootstrap
```

After installation, the package lives in `node_modules/bootstrap/`. The key directories and files are:

```
node_modules/bootstrap/
├── dist/
│   ├── css/
│   │   ├── bootstrap.css
│   │   ├── bootstrap.min.css
│   │   ├── bootstrap-grid.css
│   │   ├── bootstrap-reboot.css
│   │   └── bootstrap-utilities.css
│   └── js/
│       ├── bootstrap.bundle.js      (includes Popper.js)
│       ├── bootstrap.bundle.min.js
│       ├── bootstrap.esm.js         (ES module format)
│       ├── bootstrap.esm.min.js
│       ├── bootstrap.js             (standalone, no Popper)
│       └── bootstrap.min.js
├── scss/
│   ├── bootstrap.scss
│   ├── _variables.scss
│   ├── _functions.scss
│   ├── _mixins.scss
│   └── ...all source partials
└── js/
    └── src/
        ├── dom/
        ├── base-component.js
        ├── alert.js
        ├── button.js
        ├── carousel.js
        ├── collapse.js
        ├── dropdown.js
        └── ...
```

### Importing Compiled CSS

```html
<!-- From dist folder -->
<link href="node_modules/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
```

### Importing Compiled JS

```html
<!-- Bootstrap JS with Popper (bundle) -->
<script src="node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"></script>

<!-- Or standalone Bootstrap JS (requires separate Popper.js) -->
<script src="https://unpkg.com/@popperjs/core@2"></script>
<script src="node_modules/bootstrap/dist/js/bootstrap.min.js"></script>
```

## Advanced Variations

### ES Module Imports (Recommended for Bundlers)

```javascript
// Import individual components via ES modules
import 'bootstrap/js/dist/base-component.js';
import 'bootstrap/js/dist/dropdown.js';
import 'bootstrap/js/dist/modal.js';

// Or import all bootstrap JS
import * as bootstrap from 'bootstrap';
```

### Importing SCSS Source

```scss
// Import full Bootstrap from source
@import "node_modules/bootstrap/scss/bootstrap";

// Or selective imports for better performance
@import "node_modules/bootstrap/scss/functions";
@import "node_modules/bootstrap/scss/variables";
@import "node_modules/bootstrap/scss/mixins";
@import "node_modules/bootstrap/scss/root";
@import "node_modules/bootstrap/scss/reboot";
@import "node_modules/bootstrap/scss/grid";
```

### CSS Bundle Splitting

```html
<!-- Only the grid system -->
<link href="node_modules/bootstrap/dist/css/bootstrap-grid.min.css" rel="stylesheet">

<!-- Only the reboot/normalize -->
<link href="node_modules/bootstrap/dist/css/bootstrap-reboot.min.css" rel="stylesheet">

<!-- Only utility classes -->
<link href="node_modules/bootstrap/dist/css/bootstrap-utilities.min.css" rel="stylesheet">
```

## Best Practices

1. **Use `bootstrap.bundle.js` if you need dropdowns, tooltips, or popovers** - It includes Popper.js already.
2. **Use `bootstrap.bundle.min.js` in production** - Minified for smaller file size.
3. **Use `bootstrap.esm.js` when working with module bundlers** - Tree-shaking support.
4. **Import only the SCSS partials you need** - Reduces CSS bundle size significantly.
5. **Pin your Bootstrap version in package.json** - Avoid unexpected breaking changes.
6. **Check peer dependency warnings** - Bootstrap requires `@popperjs/core` for JS plugins.
7. **Use `dist/` for quick prototyping** - No build step required.
8. **Use `scss/` source for production customizations** - Full control over variables.
9. **Review package.json exports field** - Understand which entry points are exposed.
10. **Keep Bootstrap updated** - Security patches and bug fixes arrive regularly.
11. **Use a lock file** - Ensure consistent installs across environments.
12. **Audit your bundle size** - Tools like `webpack-bundle-analyzer` help identify bloat.

## Common Pitfalls

1. **Importing the entire JS bundle when only using one component** - Use individual ESM imports instead.
2. **Missing Popper.js peer dependency** - Dropdowns and tooltips silently fail without it. Use the bundle build.
3. **Importing both `bootstrap.css` and `bootstrap-utilities.css`** - Duplicates styles, increasing bundle size.
4. **Using compiled `dist/` CSS but wanting to customize SCSS variables** - You must use the source SCSS for variable overrides.
5. **Forgetting to include the SCSS functions and variables partials** - Partial imports require dependency order.
6. **Using `src/js/` directly without a bundler** - The `src/` directory contains uncompiled ES modules that need transpilation.

## Accessibility Considerations

The npm package includes all necessary ARIA attributes and accessibility hooks built into the JavaScript components. When importing individual modules, ensure you include all required JS dependencies for components that manage focus or keyboard navigation. The compiled bundle guarantees all accessibility features are included. Using partial CSS imports like `bootstrap-reboot.css` alone does not include accessible component styles.

## Responsive Behavior

Bootstrap's npm package includes all responsive CSS across all five breakpoints (xs through xxl) regardless of which dist file you import. There is no way to strip individual breakpoints from the compiled CSS. For granular control over responsive output, you must use SCSS source files and modify the `$grid-breakpoints` variable before compiling.
