---
title: "Bootstrap 6 Preview"
description: "Expected changes in Bootstrap 6, roadmap speculation, and preparation strategies for migration"
difficulty: 2
tags: [bootstrap-6, roadmap, migration, future, css-first]
prerequisites:
  - 01_01_Bootstrap_Setup
---

## Overview

Bootstrap 6 is expected to continue the shift from Sass-dependent to CSS-native architecture. Based on GitHub discussions, RFC threads, and the Bootstrap 5.3 trajectory, anticipated changes include dropping Sass entirely in favor of CSS custom properties, adopting CSS nesting, container queries, and `@layer`, and removing remaining jQuery-era patterns. The JavaScript layer may shift to ES modules-first with optional tree-shaking.

Migration preparation involves adopting CSS custom properties today (Bootstrap 5.3 already uses them), reducing Sass variable overrides, and building component libraries against the CSS custom property API rather than Sass maps. Teams that invest in CSS-only theming now will have a smoother migration path.

## Basic Implementation

```css
/* Bootstrap 5.3 — already preparing for v6 */
:root {
  --bs-primary: #0d6efd;
  --bs-primary-rgb: 13, 110, 253;
}

/* Bootstrap 6 expected — full CSS custom property theming */
:root {
  --bs-primary: oklch(55% 0.2 250);
  --bs-btn-primary-bg: var(--bs-primary);
  --bs-btn-primary-border: var(--bs-primary);
  --bs-btn-primary-hover-bg: color-mix(in oklch, var(--bs-primary), black 15%);
}

/* Today: prepare by using custom properties instead of Sass */
.custom-theme {
  background: var(--bs-primary); /* ✅ portable to v6 */
  /* background: $primary; */     /* ❌ Sass-only, breaks in v6 */
}
```

```html
<!-- Expected Bootstrap 6 HTML — minimal changes -->
<link rel="stylesheet" href="bootstrap.css">
<!-- No Sass compilation needed; pure CSS file -->

<div class="container">
  <div class="row">
    <div class="col-md-6">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">Future-proof Card</h5>
          <p class="card-text">Same classes, CSS-first internals.</p>
        </div>
      </div>
    </div>
  </div>
</div>
```

```js
// Expected Bootstrap 6 JS — ES module imports
import { Modal } from 'bootstrap';
// or individual imports
import Modal from 'bootstrap/js/modal.js';

const modal = new Modal(document.getElementById('myModal'));
modal.show();
```

## Advanced Variations

Prepare for v6 by adopting a CSS custom property layer:

```css
/* Create an abstraction layer today */
:root {
  --my-primary: var(--bs-primary);
  --my-spacing-sm: var(--bs-spacer-2, 0.5rem);
  --my-spacing-md: var(--bs-spacer-3, 1rem);
  --my-border-radius: var(--bs-border-radius, 0.375rem);
}

/* Use your abstraction layer everywhere */
.btn-custom {
  background: var(--my-primary);
  padding: var(--my-spacing-sm) var(--my-spacing-md);
  border-radius: var(--my-border-radius);
}
```

## Best Practices

1. Use CSS custom properties for all theme values today (Bootstrap 5.3+).
2. Avoid Sass-only variables (`$primary`, `$spacer`) in new code.
3. Build component styles against `--bs-*` custom properties.
4. Use native CSS nesting instead of Sass nesting for new styles.
5. Adopt `@layer` for cascade management now.
6. Use ES module imports for Bootstrap JavaScript.
7. Test with Bootstrap's CDN CSS-only build (no Sass).
8. Monitor the Bootstrap GitHub `v6` milestone for roadmap updates.
9. Reduce dependency on Bootstrap's Sass mixins; replace with CSS utilities.
10. Document Sass overrides that will need migration.
11. Use `@supports` for progressive enhancement of new CSS features.
12. Build a design token system that maps cleanly to CSS custom properties.

## Common Pitfalls

1. **Sass lock-in** — Heavy Sass customization makes v6 migration harder.
2. **jQuery remnants** — Some plugins still use jQuery-era patterns; migrate to Bootstrap 5 JS APIs.
3. **Breaking class names** — v6 may rename or remove utility classes; audit usage.
4. **Build tool dependency** — Teams relying on Sass compilation will need build changes.
5. **Third-party Bootstrap themes** — Theme compatibility with v6 is uncertain.
6. **Premature migration** — v6 is not released; don't build against unreleased APIs.

## Accessibility Considerations

Bootstrap 6 is expected to improve default accessibility (see 09_03_07). Current best practices (ARIA labels, focus management) should continue to work. Adopt accessible patterns now that are likely to become defaults.

## Responsive Behavior

Bootstrap 6 will likely expand container query support and may shift from viewport-based to container-based responsive utilities. Prepare by using container queries (see 09_02_01) where appropriate.
