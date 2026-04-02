---
tags: [bootstrap, file-structure, dist, source-maps, rtl, build-output]
category: Bootstrap Fundamentals
difficulty: 1
estimated_time: 20 minutes
---

# Bootstrap File Structure

## Overview

Understanding Bootstrap's file structure is essential for selecting the right files for your project, debugging compiled output, and configuring build tools. The npm package contains two primary directories: `dist/` for pre-compiled production files and `scss/` for Sass source files.

The `dist/` directory contains ready-to-use CSS and JavaScript files in multiple formats: expanded (unminified) for development, minified for production, source maps for debugging, and RTL (Right-to-Left) variants for multilingual support.

The `scss/` directory contains the Sass source files organized into partials that can be selectively imported. This directory is used when customizing Bootstrap through Sass variable overrides.

JavaScript files in `dist/js/` are provided as both a single bundle (`bootstrap.bundle.js` includes Popper.js) and standalone files (`bootstrap.js` requires separate Popper.js). Each format has expanded and minified variants.

```text
bootstrap/
├── dist/
│   ├── css/
│   │   ├── bootstrap.css              # Expanded CSS (development)
│   │   ├── bootstrap.css.map          # Source map for expanded
│   │   ├── bootstrap.min.css          # Minified CSS (production)
│   │   ├── bootstrap.min.css.map      # Source map for minified
│   │   ├── bootstrap.rtl.css          # Expanded RTL CSS
│   │   ├── bootstrap.rtl.css.map      # Source map for RTL expanded
│   │   ├── bootstrap.rtl.min.css      # Minified RTL CSS
│   │   └── bootstrap.rtl.min.css.map  # Source map for RTL minified
│   └── js/
│       ├── bootstrap.esm.js           # ES module format (expanded)
│       ├── bootstrap.esm.js.map       # Source map for ESM expanded
│       ├── bootstrap.esm.min.js       # ES module format (minified)
│       ├── bootstrap.esm.min.js.map   # Source map for ESM minified
│       ├── bootstrap.js               # UMD format (expanded)
│       ├── bootstrap.js.map           # Source map for UMD expanded
│       ├── bootstrap.min.js           # UMD format (minified)
│       ├── bootstrap.min.js.map       # Source map for UMD minified
│       ├── bootstrap.bundle.js        # UMD + Popper.js (expanded)
│       ├── bootstrap.bundle.js.map    # Source map for bundle expanded
│       ├── bootstrap.bundle.min.js    # UMD + Popper.js (minified)
│       └── bootstrap.bundle.min.js.map # Source map for bundle minified
├── js/
│   ├── src/                           # JavaScript source files
│   │   ├── base-component.js
│   │   ├── dom/
│   │   ├── modal.js
│   │   ├── dropdown.js
│   │   └── ...
│   └── dist/                          # Individual compiled plugins
│       ├── modal.js
│       ├── dropdown.js
│       └── ...
├── scss/
│   ├── bootstrap.scss                 # Main entry point (imports all)
│   ├── bootstrap-grid.scss            # Grid-only build
│   ├── bootstrap-reboot.scss          # Reboot-only build
│   ├── _functions.scss                # Sass functions
│   ├── _variables.scss                # All Sass variables
│   ├── _mixins.scss                   # Sass mixins
│   ├── _root.scss                     # CSS custom properties
│   ├── _reboot.scss                   # Browser normalization
│   ├── _containers.scss
│   ├── _grid.scss
│   ├── _buttons.scss
│   ├── _card.scss
│   ├── _modal.scss
│   ├── _navbar.scss
│   ├── forms/                         # Form component partials
│   ├── helpers/                       # Helper utility partials
│   ├── mixins/                        # Mixin partials
│   └── utilities/                     # Utility class partials
└── package.json
```

## Basic Implementation

For CDN usage, choose between the CSS-only link, the full bundle, or the RTL variant depending on your needs:

```html
<!-- Option 1: CSS + JS Bundle (most common) -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" 
      rel="stylesheet" 
      integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YcnS/1TDp2EYdKj5fjr5bE/JBM2+UoKfPZh" 
      crossorigin="anonymous">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" 
        integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" 
        crossorigin="anonymous"></script>

<!-- Option 2: RTL Support -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.rtl.min.css" 
      rel="stylesheet" 
      integrity="sha384-NQy6RzTrnRB/N8z1p4gicGqfbXi6gFrf06FVL478OhkceGD9YJkPUy/rm+HkYar7" 
      crossorigin="anonymous">

<!-- Option 3: Separate Popper.js (for fine-grained control) -->
<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.min.js"></script>
```

For npm installations, the key import paths are:

```javascript
// CSS imports (in your main SCSS file)
@import 'bootstrap/scss/bootstrap';          // Full build
@import 'bootstrap/scss/bootstrap-grid';     // Grid only
@import 'bootstrap/scss/bootstrap-reboot';   // Reboot only

// JavaScript imports
import 'bootstrap';                           // All plugins (UMD)
import 'bootstrap/dist/css/bootstrap.min.css'; // Compiled CSS

// Individual JS plugin imports (tree-shakeable)
import { Modal } from 'bootstrap';
import { Tooltip } from 'bootstrap';
import { Collapse } from 'bootstrap';

// Direct source import for maximum tree-shaking
import Modal from 'bootstrap/js/src/modal';
import Tooltip from 'bootstrap/js/src/tooltip';
```

## Advanced Variations

Understanding source maps enables effective debugging of compiled CSS and JavaScript. Source maps map minified/compiled code back to original source files, allowing developers to see Sass file names and line numbers in browser DevTools.

```html
<!-- Source map references (automatically linked in non-minified files) -->
/*# sourceMappingURL=bootstrap.css.map */

<!-- Source maps enable debugging in DevTools:
     - Chrome: Settings → Sources → Enable JavaScript source maps
     - Firefox: Settings → Enable Source Maps
     - Shows original .scss file names and line numbers
*/
```

The Bootstrap package also includes pre-built CSS variants for specific use cases:

```scss
// bootstrap-grid.scss — Grid system only (~45KB unminified)
// Includes: containers, row, col-*, gutter utilities
// Does NOT include: reboot, components, most utilities
@import "bootstrap/scss/bootstrap-grid";

// bootstrap-reboot.scss — Reboot only (~7KB unminified)
// Includes: CSS reset/normalize, box-sizing, form defaults
// Does NOT include: grid, components, utilities
@import "bootstrap/scss/bootstrap-reboot";

// bootstrap-utilities.scss — Utilities only (~70KB unminified)
// Includes: all utility classes
// Does NOT include: reboot, grid, components
@import "bootstrap/scss/bootstrap-utilities";
```

Custom builds using npm scripts:

```json
// package.json scripts
{
  "scripts": {
    "css:compile": "sass --style expanded --source-map src/scss/custom.scss dist/css/custom.css",
    "css:compile:min": "sass --style compressed --source-map src/scss/custom.scss dist/css/custom.min.css",
    "css:rtl": "sass --style expanded --source-map src/scss/custom-rtl.scss dist/css/custom.rtl.css",
    "css:prefix": "postcss dist/css/custom.css --use autoprefixer --output dist/css/custom.css --no-map",
    "css:build": "npm run css:compile && npm run css:prefix && npm run css:compile:min",
    "watch": "sass --watch src/scss/:dist/css/"
  }
}
```

## Best Practices

- **Use `bootstrap.bundle.min.js` for most projects** — it includes Popper.js in a single file, avoiding the complexity of managing separate dependencies.
- **Enable source maps during development** for both CSS and JavaScript to trace issues back to original source files.
- **Use minified files in production** (`bootstrap.min.css`, `bootstrap.min.js`) to reduce transfer size by approximately 40-60%.
- **Use npm-installed files for customization** — CDN files cannot be customized with Sass overrides.
- **Reference `.css.map` files in your server configuration** — source maps must be served alongside their corresponding files for DevTools to find them.
- **Exclude source maps from production** if security is a concern — source maps expose your source code structure to end users.
- **Use the RTL CSS variant** (`bootstrap.rtl.min.css`) for Arabic, Hebrew, Persian, and other RTL languages rather than trying to manually mirror styles.
- **Pin the exact Bootstrap version** in your lock file to ensure `dist/` files remain consistent across environments.
- **Keep `dist/` files out of version control** when building from source — the `dist/` directory is a build artifact that can be regenerated.
- **Verify file integrity** using SRI hashes from the Bootstrap CDN documentation when using externally hosted files.

## Common Pitfalls

- **Using `bootstrap.js` without Popper.js** — dropdowns, tooltips, and popovers silently fail if Popper.js isn't loaded. Use `bootstrap.bundle.js` instead.
- **Importing `bootstrap.min.css` in a Sass file** — this bypasses all variable overrides; import from `scss/` source files for customization.
- **Missing `crossorigin="anonymous"` on CDN links** — this causes CORS errors that prevent source maps from loading and may break integrity checks.
- **Serving source maps in production unintentionally** — some hosting platforms serve `.map` files by default; disable this with server configuration if security is a concern.
- **Using outdated SRI hashes** — Bootstrap updates change SRI hashes; mismatched hashes cause the browser to block the resource entirely.
- **Mixing `dist/` compiled files with `scss/` source imports** — this results in duplicate CSS rules and unpredictable cascade behavior.

## Accessibility Considerations

Bootstrap's JavaScript plugins manage focus, ARIA states, and keyboard navigation. Using the correct JS file ensures these accessibility features are included:

```html
<!-- The bundle includes all accessibility-critical plugins -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

<!-- Individual plugin imports must include focus-management dependencies -->
<!-- Modal requires backdrop.js, focus-trap, and base-component -->
<!-- Dropdown requires manipulator.js and event handler utilities -->
```

The `_reboot.scss` partial normalizes focus outlines, form element sizing, and button behavior across browsers. Always include reboot (or its equivalent) in any Bootstrap build to maintain accessible defaults.

The `dist/css/bootstrap.css` expanded file can be inspected to verify that ARIA-related styles (`.visually-hidden`, focus ring outlines) are present. In the minified version, these are compressed but still functional.

## Responsive Behavior

The `dist/` files include all five breakpoint tiers (xs, sm, md, lg, xl, xxl) and their associated responsive classes. The grid-only build (`bootstrap-grid.css`) includes responsive column classes but not responsive display utilities or responsive spacing.

```html
<!-- All responsive classes available in full dist/css/bootstrap.min.css -->
<div class="container">
  <div class="row">
    <div class="col-12 col-sm-6 col-md-4 col-lg-3 col-xl-2 col-xxl-1">
      Responsive at every breakpoint
    </div>
  </div>
</div>

<!-- Responsive visibility (requires full build, not grid-only) -->
<div class="d-none d-md-block">Visible on md+</div>
<div class="d-block d-md-none">Visible below md</div>

<!-- Grid-only build includes: .col-*, .row, .container -->
<!-- Grid-only build does NOT include: .d-none, .d-md-block, .mt-md-3 -->
```

The RTL variant (`bootstrap.rtl.css`) mirrors directional properties automatically: `margin-left` becomes `margin-right`, `padding-left` becomes `padding-right`, `text-align: left` becomes `text-align: right`, and flex `justify-content: flex-start` becomes `justify-content: flex-end`. This ensures layouts reverse correctly for RTL languages without manual CSS overrides.
