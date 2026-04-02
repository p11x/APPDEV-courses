---
title: "Bootstrap in Monorepo Architecture"
topic: "Build Tools"
difficulty: 3
duration: "50 minutes"
prerequisites: ["Monorepo concepts (npm/yarn/pnpm workspaces)", "Bootstrap theming", "Package publishing"]
tags: ["monorepo", "workspaces", "bootstrap", "shared-theme", "build-tools"]
---

## Overview

A monorepo consolidates multiple related packages into a single repository, enabling shared code, consistent tooling, and atomic cross-package changes. For Bootstrap 5 projects, a monorepo architecture allows you to extract a shared Bootstrap theme package, component libraries, and application-specific customizations into separate, independently versioned packages while maintaining a single source of truth for design tokens and styling conventions.

The typical Bootstrap monorepo structure separates concerns into: a `@org/bootstrap-theme` package containing SCSS variables, overrides, and compiled CSS; a `@org/ui-components` package with reusable Bootstrap-based components; and application packages (`@org/web-app`, `@org/admin-panel`) that consume the theme. Tools like npm workspaces, Yarn workspaces, pnpm workspaces, or Nx/Turborepo manage inter-package dependencies and build orchestration.

## Basic Implementation

Set up a monorepo with npm workspaces:

```json
// root package.json
{
  "name": "@myorg/bootstrap-monorepo",
  "private": true,
  "workspaces": [
    "packages/*",
    "apps/*"
  ],
  "scripts": {
    "build": "npm run build --workspaces",
    "build:theme": "npm run build -w packages/bootstrap-theme",
    "dev:web": "npm run dev -w apps/web",
    "dev:admin": "npm run dev -w apps/admin",
    "lint": "npm run lint --workspaces"
  },
  "devDependencies": {
    "sass": "^1.77.0",
    "prettier": "^3.3.0"
  }
}
```

Create the directory structure:

```
bootstrap-monorepo/
├── packages/
│   ├── bootstrap-theme/
│   │   ├── package.json
│   │   ├── src/
│   │   │   ├── _variables.scss
│   │   │   ├── _overrides.scss
│   │   │   ├── _components.scss
│   │   │   └── index.scss
│   │   └── dist/
│   │       ├── theme.css
│   │       └── theme.css.map
│   └── ui-components/
│       ├── package.json
│       └── src/
├── apps/
│   ├── web/
│   │   ├── package.json
│   │   └── src/
│   └── admin/
│       ├── package.json
│       └── src/
└── package.json
```

### Theme Package

```json
// packages/bootstrap-theme/package.json
{
  "name": "@myorg/bootstrap-theme",
  "version": "1.0.0",
  "main": "dist/theme.css",
  "sass": "src/index.scss",
  "exports": {
    ".": {
      "sass": "./src/index.scss",
      "style": "./dist/theme.css"
    },
    "./scss": "./src/index.scss"
  },
  "files": ["src", "dist"],
  "scripts": {
    "build": "sass src/index.scss dist/theme.css --style compressed --source-map",
    "watch": "sass --watch src:dist"
  },
  "dependencies": {
    "bootstrap": "^5.3.0"
  },
  "peerDependencies": {
    "sass": ">=1.70.0"
  }
}
```

```scss
// packages/bootstrap-theme/src/_variables.scss
// Design tokens shared across all applications
$primary: #6366f1;
$secondary: #64748b;
$success: #059669;
$danger: #dc2626;
$warning: #f59e0b;
$info: #0891b2;

$font-family-sans-serif: 'Inter', system-ui, sans-serif;
$font-size-base: 1rem;
$line-height-base: 1.6;

$border-radius: 0.5rem;
$border-radius-lg: 0.75rem;
$border-radius-sm: 0.375rem;

$spacer: 1rem;
$enable-rounded: true;
$enable-shadows: true;
$enable-gradients: false;

$container-max-widths: (
  sm: 540px,
  md: 720px,
  lg: 960px,
  xl: 1140px,
  xxl: 1320px,
);

$theme-colors: (
  'primary': $primary,
  'secondary': $secondary,
  'brand': #f59e0b,
  'neutral': #64748b,
);
```

```scss
// packages/bootstrap-theme/src/_overrides.scss
// Component-level overrides
.btn {
  font-weight: 500;
  letter-spacing: 0.025em;
}

.card {
  border: none;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.navbar {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}
```

```scss
// packages/bootstrap-theme/src/_components.scss
// Custom components
@import 'bootstrap/scss/functions';
@import 'variables';
@import 'bootstrap/scss/mixins';

.badge-soft {
  &-primary {
    color: $primary;
    background-color: rgba($primary, 0.1);
  }
  &-success {
    color: $success;
    background-color: rgba($success, 0.1);
  }
  &-danger {
    color: $danger;
    background-color: rgba($danger, 0.1);
  }
}
```

```scss
// packages/bootstrap-theme/src/index.scss
@import 'variables';
@import 'bootstrap/scss/bootstrap';
@import 'overrides';
@import 'components';
```

### Application Package

```json
// apps/web/package.json
{
  "name": "@myorg/web",
  "version": "1.0.0",
  "private": true,
  "scripts": {
    "dev": "vite",
    "build": "vite build"
  },
  "dependencies": {
    "@myorg/bootstrap-theme": "*",
    "@myorg/ui-components": "*"
  },
  "devDependencies": {
    "sass": "^1.77.0",
    "vite": "^5.4.0"
  }
}
```

```scss
// apps/web/src/styles/app.scss
// Import the shared theme (exposes all Bootstrap + overrides)
@use '@myorg/bootstrap-theme/scss' as theme;

// App-specific overrides
.page-header {
  background: linear-gradient(135deg, theme.$primary, darken(theme.$primary, 15%));
  color: white;
}
```

## Advanced Variations

### Nx Monorepo Configuration

```json
// nx.json
{
  "targetDefaults": {
    "build": {
      "dependsOn": ["^build"],
      "cache": true
    },
    "sass": {
      "dependsOn": ["^build"],
      "cache": true,
      "inputs": ["default", "{projectRoot}/src/**/*.scss"]
    }
  }
}
```

```json
// packages/bootstrap-theme/project.json
{
  "name": "bootstrap-theme",
  "targets": {
    "build": {
      "executor": "@nrwl/workspace:run-commands",
      "options": {
        "command": "sass src/index.scss:dist/theme.css --style compressed --source-map"
      },
      "outputs": ["{projectRoot}/dist"]
    },
    "watch": {
      "executor": "@nrwl/workspace:run-commands",
      "options": {
        "command": "sass --watch src:dist"
      }
    }
  }
}
```

### pnpm Workspaces

```yaml
# pnpm-workspace.yaml
packages:
  - 'packages/*'
  - 'apps/*'
```

```json
// packages/bootstrap-theme/package.json (pnpm)
{
  "name": "@myorg/bootstrap-theme",
  "exports": {
    ".": {
      "sass": "./src/index.scss",
      "style": "./dist/theme.css"
    },
    "./scss/*": "./src/*"
  }
}
```

### Shared Component Library

```json
// packages/ui-components/package.json
{
  "name": "@myorg/ui-components",
  "exports": {
    ".": "./src/index.js",
    "./components/*": "./src/components/*"
  },
  "dependencies": {
    "@myorg/bootstrap-theme": "*"
  }
}
```

```js
// packages/ui-components/src/index.js
export { Alert } from './components/Alert.js';
export { Modal } from './components/Modal.js';
export { DataTable } from './components/DataTable.js';
export { Pagination } from './components/Pagination.js';
```

### Turborepo Configuration

```json
// turbo.json
{
  "$schema": "https://turbo.build/schema.json",
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    },
    "lint": {}
  }
}
```

## Best Practices

1. **Use the `sass` export field** in the theme package's `package.json` so consuming packages can resolve SCSS imports.
2. **Keep `bootstrap` as a dependency** of the theme package, not the root — this ensures correct version resolution per package.
3. **Use `peerDependencies` for `sass`** in the theme package so consumers provide their own Sass compiler.
4. **Export both compiled CSS (`style`) and SCSS source (`sass`)** for flexibility in how downstream packages consume the theme.
5. **Version the theme package independently** using changesets or semantic-release for independent package versioning.
6. **Use workspace protocol** (`"bootstrap-theme": "workspace:*"`) in pnpm for explicit workspace linking.
7. **Set `"private": true`** on application packages to prevent accidental publishing to npm.
8. **Run `npm run build` with `--workspaces`** or Turborepo to build dependencies before dependents.
9. **Store design tokens in a dedicated `_variables.scss`** that both the theme and custom code reference.
10. **Avoid circular dependencies** — the theme should not import from applications or component libraries.

## Common Pitfalls

1. **Circular dependency between theme and components** causes infinite Sass compilation loops or bundler errors.
2. **Missing `sass` export in `package.json`** causes `@use '@myorg/theme/scss'` to fail in consuming packages.
3. **Bootstrap installed in multiple packages** at different versions leads to CSS duplication and style conflicts.
4. **Not building the theme before dependent packages** results in missing `dist/` artifacts during application builds.
5. **Using `*` version ranges** without lock file discipline causes inconsistent builds across environments.

## Accessibility Considerations

A shared theme package ensures accessibility standards propagate across all applications. Define ARIA-aware component defaults in the theme's `_overrides.scss` (e.g., ensuring all `.btn` elements have visible focus states). Maintain a shared set of accessible color contrast ratios in `_variables.scss` that all packages inherit. Document accessibility requirements in the theme package's README so downstream consumers understand the ARIA patterns and keyboard navigation behaviors built into shared components.

## Responsive Behavior

The shared theme package defines the grid breakpoint map (`$grid-breakpoints`) that all applications inherit. This ensures consistent responsive behavior across `@myorg/web`, `@myorg/admin`, and other applications. If a new breakpoint is added (e.g., `xxl`), updating the theme package propagates the change to all workspaces. The `container-max-widths` variable in `_variables.scss` controls maximum container widths at each breakpoint, applied consistently across the monorepo. Custom responsive utilities generated in the theme package are available to all downstream SCSS files via the `@use` import path.