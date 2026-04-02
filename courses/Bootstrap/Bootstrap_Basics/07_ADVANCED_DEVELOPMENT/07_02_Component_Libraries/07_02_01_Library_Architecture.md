---
title: "Library Architecture"
difficulty: 3
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - ES Module System
  - npm Package Structure
  - Bootstrap 5 Module Architecture
---

## Overview

Component library architecture defines how Bootstrap-based components are organized, exported, and consumed across projects. A well-structured library uses barrel exports for clean imports, modular package organization for tree-shaking support, and clear separation between core utilities, individual components, and theme layers.

The architecture follows a layered approach: foundation layer (variables, mixins, tokens), component layer (individual UI components), composition layer (components assembled into patterns), and application layer (page templates and layouts). Each layer depends only on layers below it, preventing circular dependencies and enabling selective consumption.

Package organization determines how teams import the library. A monorepo with individual component packages offers maximum tree-shaking but higher complexity. A single package with subpath exports provides a balanced approach. The choice depends on library size, team count, and build tool capabilities.

## Basic Implementation

A component library uses a clear directory structure with barrel exports at each level.

```
bootstrap-library/
├── package.json
├── scss/
│   ├── _index.scss              # Main SCSS entry
│   ├── _variables.scss          # Library-wide variables
│   ├── _mixins.scss             # Shared mixins
│   ├── components/
│   │   ├── _index.scss          # Barrel export for all components
│   │   ├── _button.scss
│   │   ├── _card.scss
│   │   ├── _modal.scss
│   │   └── _data-table.scss
│   ├── utilities/
│   │   ├── _index.scss
│   │   └── _responsive.scss
│   └── themes/
│       ├── _index.scss
│       ├── _light.scss
│       └── _dark.scss
├── js/
│   ├── index.js                 # Main JS entry
│   ├── components/
│   │   ├── index.js             # Barrel export
│   │   ├── Modal.js
│   │   ├── DataTable.js
│   │   └── Notification.js
│   └── utils/
│       ├── index.js
│       ├── dom.js
│       └── events.js
└── types/
    └── index.d.ts
```

```scss
// scss/_index.scss
@import 'bootstrap/scss/functions';
@import 'bootstrap/scss/variables';
@import 'bootstrap/scss/mixins';

// Library variables (override Bootstrap defaults)
@import 'variables';

// Bootstrap core
@import 'bootstrap/scss/bootstrap';

// Library components
@import 'components/index';

// Library utilities
@import 'utilities/index';
```

```scss
// scss/components/_index.scss
@import 'button';
@import 'card';
@import 'modal';
@import 'data-table';
```

```js
// js/index.js - Main barrel export
export { default as DataTable } from './components/DataTable';
export { default as Notification } from './components/Notification';
export { default as Modal } from './components/Modal';

// Utilities
export { debounce, throttle } from './utils/dom';
export { EventEmitter } from './utils/events';

// Library metadata
export const version = '1.0.0';
export const name = '@company/bootstrap-library';

// Auto-init all components
export function initAll(context = document) {
  const components = [DataTable, Notification, Modal];
  components.forEach(Component => {
    if (Component.selector) {
      context.querySelectorAll(Component.selector).forEach(el => {
        Component.getOrCreateInstance(el);
      });
    }
  });
}
```

```json
// package.json with subpath exports
{
  "name": "@company/bootstrap-library",
  "version": "1.0.0",
  "exports": {
    ".": {
      "import": "./js/index.js",
      "require": "./js/index.cjs.js",
      "types": "./types/index.d.ts"
    },
    "./scss": "./scss/_index.scss",
    "./css": "./dist/css/library.min.css",
    "./components/*": {
      "import": "./js/components/*.js",
      "types": "./types/components/*.d.ts"
    },
    "./themes/*": "./scss/themes/*.scss"
  },
  "sideEffects": [
    "**/*.css",
    "**/*.scss"
  ],
  "peerDependencies": {
    "bootstrap": "^5.3.0"
  }
}
```

## Advanced Variations

```js
// js/components/DataTable.js - Full component implementation
import { EventEmitter } from '../utils/events';

export default class DataTable extends EventEmitter {
  static selector = '[data-component="data-table"]';
  static INSTANCES = new WeakMap();

  static Default = {
    sortable: true,
    filterable: false,
    paginate: true,
    pageSize: 10,
    responsive: true
  };

  constructor(element, config = {}) {
    super();
    this._element = element;
    this._config = { ...DataTable.Default, ...config };
    this._data = [];
    this._sortColumn = null;
    this._sortDirection = 'asc';
    this._currentPage = 1;

    this._init();
    DataTable.INSTANCES.set(this._element, this);
  }

  static getOrCreateInstance(element, config = {}) {
    return DataTable.INSTANCES.get(element) || new DataTable(element, config);
  }

  _init() {
    this._parseExistingTable();
    if (this._config.sortable) this._initSorting();
    if (this._config.paginate) this._initPagination();
  }

  setData(data, columns) {
    this._data = data;
    this._columns = columns;
    this._render();
    this.emit('dataChange', { data, columns });
  }

  _render() {
    const thead = this._renderHeader();
    const tbody = this._renderBody();
    const pagination = this._config.paginate ? this._renderPagination() : '';

    this._element.innerHTML = `
      <div class="table-responsive">
        <table class="table table-hover">
          ${thead}
          ${tbody}
        </table>
      </div>
      ${pagination}
    `;

    this._bindEvents();
  }

  dispose() {
    DataTable.INSTANCES.delete(this._element);
    this.removeAllListeners();
    this._element.innerHTML = '';
  }
}
```

```json
// Monorepo structure with pnpm workspaces
{
  "name": "@company/design-system",
  "private": true,
  "workspaces": [
    "packages/*"
  ]
}

// packages/core/package.json
{
  "name": "@company/ds-core",
  "exports": {
    ".": "./index.js",
    "./scss": "./scss/_index.scss"
  }
}

// packages/data-table/package.json
{
  "name": "@company/ds-data-table",
  "exports": {
    ".": "./index.js",
    "./scss": "./scss/_index.scss"
  },
  "dependencies": {
    "@company/ds-core": "workspace:*"
  }
}
```

## Best Practices

1. **Use barrel exports** - Provide `index.js` and `_index.scss` files at each directory level for clean imports.
2. **Enable tree-shaking** - Use ESM exports and the `sideEffects` field in package.json to allow unused component elimination.
3. **Separate SCSS and JS entry points** - Let consumers import styles and scripts independently based on their needs.
4. **Provide subpath exports** - Use package.json `exports` map to allow `import { DataTable } from '@lib/components/DataTable'`.
5. **Version independently from Bootstrap** - Your library version should reflect your component API, not Bootstrap's version.
6. **Document the import order** - SCSS must be imported before Bootstrap utilities; JS requires Bootstrap's bundle as a peer dependency.
7. **Use TypeScript** - Provide type definitions for all exports to enable IDE autocomplete and compile-time checking.
8. **Keep components atomic** - Each component should work independently without requiring other library components.
9. **Provide an `initAll` function** - Convenience function that initializes all components in a given DOM context.
10. **Include a CSS-only fallback** - Components should degrade gracefully without JavaScript where possible.

## Common Pitfalls

1. **Circular dependencies** - Component A importing from Component B which imports from Component A causes bundler errors.
2. **Missing sideEffects flag** - Without it, bundlers can't tree-shake CSS imports, bloating the final bundle.
3. **Hardcoded paths in SCSS** - Using `@import 'bootstrap/scss/bootstrap'` instead of `@import 'bootstrap/scss/functions'` imports everything.
4. **No disposal pattern** - Components that don't clean up create memory leaks in single-page applications.
5. **Breaking barrel exports** - Exporting the same symbol from multiple barrel files causes ambiguous import errors.

## Accessibility Considerations

Every component in the library must meet WCAG 2.1 AA standards. Include accessibility as a first-class concern in the component API.

```js
// Accessible component pattern
export default class Notification extends EventEmitter {
  static Default = {
    role: 'status',
    ariaLive: 'polite',
    ariaAtomic: true,
    autoDismiss: 5000
  };

  _render() {
    const el = document.createElement('div');
    el.className = 'notification';
    el.setAttribute('role', this._config.role);
    el.setAttribute('aria-live', this._config.ariaLive);
    el.setAttribute('aria-atomic', String(this._config.ariaAtomic));
    return el;
  }
}
```

## Responsive Behavior

Components should use Bootstrap's responsive utilities and grid system for responsive behavior. Document responsive configuration options clearly.

```scss
// Responsive data table
.data-table {
  @include media-breakpoint-down(md) {
    .table {
      font-size: $small-font-size;

      th, td {
        padding: map-get($spacers, 2);
      }
    }
  }
}
```
