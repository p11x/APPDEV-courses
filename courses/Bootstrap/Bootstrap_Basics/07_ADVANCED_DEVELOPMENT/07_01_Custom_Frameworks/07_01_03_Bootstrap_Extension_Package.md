---
title: "Bootstrap Extension Package"
difficulty: 3
category: "Advanced Development"
subcategory: "Custom Frameworks"
prerequisites:
  - npm Package Development
  - SCSS Module System
  - Bootstrap 5 Plugin Architecture
---

## Overview

Building an npm package that extends Bootstrap 5 involves creating reusable SCSS mixins, custom JavaScript plugins, and utility classes that integrate seamlessly with Bootstrap's existing architecture. The package follows Bootstrap's conventions while adding specialized functionality not found in the core framework.

An extension package has three primary layers: SCSS extensions that add new mixins, variables, and component styles; JavaScript plugin add-ons that extend or complement Bootstrap's existing plugins; and utility integrations that register custom utilities through Bootstrap's utility API. Each layer builds on Bootstrap's infrastructure rather than replacing it.

The package should be installable via npm, importable through both SCSS and JavaScript entry points, and compatible with Bootstrap 5.x's build system. It must handle tree-shaking for JavaScript, support SCSS variable overrides before import, and provide TypeScript definitions for IDE support.

## Basic Implementation

A standard extension package structure separates SCSS, JavaScript, and configuration files with clear entry points.

```json
{
  "name": "bootstrap-extended",
  "version": "1.0.0",
  "main": "dist/js/bootstrap-extended.cjs.js",
  "module": "dist/js/bootstrap-extended.esm.js",
  "style": "scss/bootstrap-extended.scss",
  "sass": "scss/bootstrap-extended.scss",
  "files": [
    "dist/",
    "scss/",
    "types/"
  ],
  "peerDependencies": {
    "bootstrap": "^5.3.0"
  },
  "exports": {
    ".": {
      "import": "./dist/js/bootstrap-extended.esm.js",
      "require": "./dist/js/bootstrap-extended.cjs.js",
      "types": "./types/index.d.ts"
    },
    "./scss/*": "./scss/*",
    "./css/*": "./dist/css/*"
  }
}
```

```scss
// scss/bootstrap-extended.scss
@import 'bootstrap/scss/functions';
@import 'bootstrap/scss/variables';
@import 'bootstrap/scss/mixins';

// Extension variables
$be-toast-max-width: 350px !default;
$be-toast-zindex: 1090 !default;
$be-skeleton-base-color: $gray-300 !default;
$be-skeleton-shine-color: $white !default;

// Custom utilities
$utilities: map-merge(
  $utilities,
  (
    'object-fit': (
      property: object-fit,
      class: object-fit,
      values: contain cover fill none scale-down
    ),
    'cursor': (
      property: cursor,
      class: cursor,
      values: pointer default not-allowed
    )
  )
);

@import 'bootstrap/scss/utilities';
@import 'bootstrap/scss/utilities/api';

// Extension components
@import 'components/toast-stacked';
@import 'components/skeleton';
@import 'components/stepper';
```

```js
// src/js/index.js
import { StackedToast } from './components/stacked-toast';
import { Skeleton } from './components/skeleton';
import { Stepper } from './components/stepper';

class BootstrapExtended {
  static Toast = StackedToast;
  static Skeleton = Skeleton;
  static Stepper = Stepper;

  static init() {
    // Auto-init components from data attributes
    document.querySelectorAll('[data-be-stepper]').forEach(el => {
      Stepper.getOrCreateInstance(el);
    });
  }
}

// Auto-initialize on DOMContentLoaded
if (typeof window !== 'undefined') {
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', BootstrapExtended.init);
  } else {
    BootstrapExtended.init();
  }
}

export { BootstrapExtended, StackedToast, Skeleton, Stepper };
export default BootstrapExtended;
```

## Advanced Variations

Advanced extension packages implement TypeScript generics, plugin registration systems, and SCSS configuration layers.

```scss
// scss/components/_stepper.scss
$be-stepper-icon-size: 2.5rem !default;
$be-stepper-line-color: $gray-300 !default;
$be-stepper-active-color: $primary !default;
$be-stepper-completed-color: $success !default;

.be-stepper {
  display: flex;
  flex-direction: column;
  counter-reset: step;

  &__step {
    position: relative;
    display: flex;
    align-items: flex-start;
    padding-bottom: $spacer;

    &::before {
      content: counter(step);
      counter-increment: step;
      display: flex;
      align-items: center;
      justify-content: center;
      width: $be-stepper-icon-size;
      height: $be-stepper-icon-size;
      border-radius: 50%;
      border: 2px solid $be-stepper-line-color;
      font-weight: $font-weight-bold;
      flex-shrink: 0;
      transition: all 0.3s ease;
    }

    &::after {
      content: '';
      position: absolute;
      left: calc(#{$be-stepper-icon-size} / 2 - 1px);
      top: $be-stepper-icon-size;
      width: 2px;
      height: calc(100% - #{$be-stepper-icon-size});
      background: $be-stepper-line-color;
    }

    &:last-child::after { display: none; }

    &--active::before {
      border-color: $be-stepper-active-color;
      background: $be-stepper-active-color;
      color: $white;
    }

    &--completed::before {
      border-color: $be-stepper-completed-color;
      background: $be-stepper-completed-color;
      color: $white;
      content: '\2713';
    }
  }

  &__content {
    margin-left: calc(#{$be-stepper-icon-size} + #{$spacer});
    padding-top: 0.25rem;
  }
}
```

```js
// src/js/components/stepper.js
export class Stepper {
  static NAME = 'be-stepper';
  static INSTANCES = new WeakMap();

  constructor(element, config = {}) {
    this._element = typeof element === 'string' ? document.querySelector(element) : element;
    this._config = { ...Stepper.DEFAULTS, ...config };
    this._steps = [...this._element.querySelectorAll('.be-stepper__step')];
    this._activeIndex = this._config.initialStep;

    this._init();
    Stepper.INSTANCES.set(this._element, this);
  }

  static DEFAULTS = {
    initialStep: 0,
    linear: true,
    onStepChange: null
  };

  static getOrCreateInstance(element, config = {}) {
    return Stepper.INSTANCES.get(element) || new Stepper(element, config);
  }

  _init() {
    this._render();
    this._bindEvents();
  }

  _render() {
    this._steps.forEach((step, index) => {
      step.classList.remove('be-stepper__step--active', 'be-stepper__step--completed');
      if (index < this._activeIndex) step.classList.add('be-stepper__step--completed');
      if (index === this._activeIndex) step.classList.add('be-stepper__step--active');
    });
  }

  _bindEvents() {
    if (!this._config.linear) {
      this._steps.forEach((step, index) => {
        step.style.cursor = 'pointer';
        step.addEventListener('click', () => this.goToStep(index));
      });
    }
  }

  next() {
    if (this._activeIndex < this._steps.length - 1) {
      this.goToStep(this._activeIndex + 1);
    }
  }

  previous() {
    if (this._activeIndex > 0) {
      this.goToStep(this._activeIndex - 1);
    }
  }

  goToStep(index) {
    const previousIndex = this._activeIndex;
    this._activeIndex = index;
    this._render();
    this._config.onStepChange?.({ from: previousIndex, to: index, stepper: this });

    this._element.dispatchEvent(new CustomEvent('step-change.bs.stepper', {
      detail: { from: previousIndex, to: index }
    }));
  }

  get activeStep() { return this._activeIndex; }
  get totalSteps() { return this._steps.length; }

  dispose() {
    Stepper.INSTANCES.delete(this._element);
    this._steps.forEach(step => step.replaceWith(step.cloneNode(true)));
  }
}
```

```scss
// scss/components/_skeleton.scss
$be-skeleton-animation: shimmer !default;
$be-skeleton-base-color: $gray-200 !default;
$be-skeleton-highlight-color: $gray-100 !default;

@keyframes be-skeleton-shimmer {
  0% { background-position: -200% 0; }
  100% { background-position: 200% 0; }
}

.be-skeleton {
  background: linear-gradient(
    90deg,
    $be-skeleton-base-color 25%,
    $be-skeleton-highlight-color 50%,
    $be-skeleton-base-color 75%
  );
  background-size: 200% 100%;
  animation: be-skeleton-shimmer 1.5s ease-in-out infinite;
  border-radius: $border-radius;

  &--text {
    height: 1em;
    margin-bottom: 0.5em;
    width: 100%;

    &:last-child { width: 80%; }
  }

  &--circle {
    border-radius: 50%;
    width: 48px;
    height: 48px;
  }

  &--rect {
    width: 100%;
    height: 200px;
  }
}
```

## Best Practices

1. **Use peer dependencies** - Declare Bootstrap as a peer dependency, not a direct dependency, to avoid version duplication in consuming projects.
2. **Follow Bootstrap naming conventions** - Prefix all custom classes and data attributes (e.g., `be-`) to prevent collisions with Bootstrap core.
3. **Provide SCSS entry points** - Allow users to import your SCSS before Bootstrap's utilities layer so the utility API integration works correctly.
4. **Support SCSS variable overrides** - Declare all customizable values with `!default` so users can override them before importing.
5. **Bundle both ESM and CJS** - Provide ES module and CommonJS builds for maximum compatibility across build tools.
6. **Include TypeScript definitions** - Provide `.d.ts` files for all JavaScript exports to enable IDE autocomplete and type checking.
7. **Document import order** - Clearly state whether SCSS should be imported before or after Bootstrap's core to avoid specificity conflicts.
8. **Version against Bootstrap ranges** - Support Bootstrap 5.x with flexible semver ranges; test against multiple minor versions.
9. **Minimize CSS output** - Use `@if` guards in SCSS to only generate CSS for components that are explicitly enabled.
10. **Provide a build script** - Include a build command that compiles SCSS, bundles JavaScript, and generates all distribution formats.

## Common Pitfalls

1. **Importing Bootstrap SCSS twice** - If your package imports Bootstrap and the consumer also imports Bootstrap, variables and mixins get redefined causing conflicts.
2. **Overriding core Bootstrap** - Modifying Bootstrap's internal variables after import breaks other packages that depend on Bootstrap.
3. **Missing tree-shaking support** - Not using ESM exports or sideEffects flag causes entire package to be bundled even when only one component is used.
4. **Breaking Bootstrap's utility API** - Incorrectly merging into `$utilities` map after Bootstrap's `utilities/api` import produces no output.
5. **Incompatible z-index values** - Using z-index values that don't respect Bootstrap's z-index scale causes stacking context issues with modals and dropdowns.

## Accessibility Considerations

Extension components must match Bootstrap's accessibility standards. Custom components like steppers need proper ARIA attributes, keyboard navigation, and screen reader announcements.

```html
<!-- Accessible stepper markup generated by the extension -->
<div class="be-stepper" role="list" aria-label="Checkout steps" data-be-stepper>
  <div class="be-stepper__step be-stepper__step--completed" role="listitem" aria-label="Step 1, completed: Shipping address">
    <div class="be-stepper__content">
      <h3>Shipping Address</h3>
      <p>123 Main Street, Apt 4B</p>
    </div>
  </div>
  <div class="be-stepper__step be-stepper__step--active" role="listitem" aria-current="step" aria-label="Step 2, current: Payment method">
    <div class="be-stepper__content">
      <h3>Payment Method</h3>
      <form><!-- fields --></form>
    </div>
  </div>
  <div class="be-stepper__step" role="listitem" aria-label="Step 3, pending: Review order">
    <div class="be-stepper__content">
      <h3>Review Order</h3>
    </div>
  </div>
</div>
```

Ensure skeleton loading states include `aria-busy="true"` and `aria-label="Loading content"` so screen readers announce loading status to users.

## Responsive Behavior

Extension components should inherit responsive behavior from Bootstrap's grid and breakpoint system. Custom components should not define their own breakpoints but instead use Bootstrap's `media-breakpoint-up` and `media-breakpoint-down` mixins.

```scss
// Responsive stepper layout
.be-stepper {
  &--horizontal {
    @include media-breakpoint-up(md) {
      flex-direction: row;

      .be-stepper__step {
        flex-direction: column;
        align-items: center;
        flex: 1;
        padding-bottom: 0;

        &::after {
          left: calc(#{$be-stepper-icon-size});
          top: calc(#{$be-stepper-icon-size} / 2 - 1px);
          width: calc(100% - #{$be-stepper-icon-size} * 2);
          height: 2px;
        }
      }

      .be-stepper__content {
        margin-left: 0;
        margin-top: $spacer;
        text-align: center;
      }
    }
  }
}
```

Use Bootstrap's responsive container queries when available and fall back to viewport-based breakpoints for component-level responsive adjustments.
