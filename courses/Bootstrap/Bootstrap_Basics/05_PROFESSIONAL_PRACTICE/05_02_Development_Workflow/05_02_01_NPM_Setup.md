---
title: "NPM Setup for Bootstrap 5"
module: "Development Workflow"
difficulty: 1
estimated_time: 20 min
prerequisites:
  - Node.js and npm installed
  - Basic terminal knowledge
tags:
  - npm
  - bootstrap
  - package-management
  - setup
---

# NPM Setup for Bootstrap 5

## Overview

NPM (Node Package Manager) is the standard package manager for installing Bootstrap 5 in modern web projects. Using npm provides version-controlled dependency management, simplifies updates, and integrates Bootstrap into your build pipeline. Unlike CDN-based approaches, npm installation gives you full access to Bootstrap's source SCSS files, JavaScript source, and enables tree-shaking to reduce bundle size. This guide covers installing Bootstrap via npm, configuring your `package.json`, importing CSS and JavaScript from `node_modules`, and managing versions effectively across your team.

## Basic Installation

Run the following command to install Bootstrap 5 and its peer dependency Popper.js:

```bash
npm install bootstrap @popperjs/core
```

Your `package.json` will be updated automatically with Bootstrap listed under `dependencies`. Verify the installation by checking `node_modules/bootstrap/dist/` for compiled CSS and JS files.

Import Bootstrap's compiled CSS in your main stylesheet or JavaScript entry point:

```html
<link rel="stylesheet" href="node_modules/bootstrap/dist/css/bootstrap.min.css">
<script src="node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
```

For JavaScript module imports, reference Bootstrap from your entry file:

```json
{
  "dependencies": {
    "bootstrap": "^5.3.3",
    "@popperjs/core": "^2.11.8"
  }
}
```

## Advanced Variations

Import only specific Bootstrap components to reduce bundle size. Use individual module imports instead of the full bundle:

```bash
npm install bootstrap@5.3.3 --save-exact
```

The `--save-exact` flag locks the exact version in `package.json`, preventing unexpected updates. You can also install Bootstrap as a devDependency if you only need it during development:

```bash
npm install bootstrap --save-dev
```

For projects requiring legacy Bootstrap versions alongside newer releases, use npm aliases:

```bash
npm install bootstrap5@npm:bootstrap@5.3.3
```

This installs Bootstrap 5 under the alias `bootstrap5`, allowing parallel installations of different major versions.

## Best Practices

1. **Always pin your Bootstrap version** in production projects using `--save-exact` to avoid breaking changes from minor updates.
2. **Use `package-lock.json`** — never delete it; it ensures reproducible installs across all team members and CI environments.
3. **Separate runtime from dev dependencies** — use `--save` for production and `--save-dev` for build-only tools.
4. **Run `npm audit` regularly** to identify known vulnerabilities in Bootstrap or its transitive dependencies.
5. **Use `.npmrc`** in your project root to enforce consistent npm behavior (e.g., `engine-strict=true`).
6. **Document required Node.js and npm versions** in your README to prevent environment mismatches.
7. **Avoid installing Bootstrap globally** — project-local installs prevent version conflicts between projects.
8. **Use `npm ci` in CI/CD pipelines** instead of `npm install` for faster, deterministic builds.
9. **Keep `node_modules` out of version control** via `.gitignore`.
10. **Run `npm outdated` periodically** to check for available Bootstrap updates and plan upgrades intentionally.
11. **Use `npm ls bootstrap`** to verify the installed version and check for dependency conflicts.

## Common Pitfalls

1. **Forgetting to install `@popperjs/core`** — Bootstrap's JavaScript dropdowns and tooltips require Popper.js. Without it, these components silently fail.
2. **Mixing npm and CDN imports** — loading Bootstrap from both sources causes duplicate styles and JavaScript conflicts. Pick one method.
3. **Not adding `node_modules` paths correctly** — relative paths to `node_modules/` break when your HTML file is nested in subdirectories.
4. **Ignoring `package-lock.json` conflicts** — merge conflicts in this file must be resolved by running `npm install` after resolving, not by manual editing.
5. **Using `npm install` without a version** in production — this installs the latest version, which may introduce breaking changes to your layout.
6. **Assuming Bootstrap SCSS is available** — the default `npm install bootstrap` includes compiled CSS. For SCSS access, import from `bootstrap/scss/` in your build pipeline.
7. **Circular dependency issues** — importing Bootstrap JS modules incorrectly can cause circular references; always import from `bootstrap/js/dist/` for individual components.

## Accessibility Considerations

When setting up Bootstrap via npm, ensure your build process does not strip accessibility attributes. Bootstrap's compiled JavaScript includes ARIA attribute management for modals, dropdowns, and alerts. Import the full `bootstrap.bundle.min.js` to retain these features. If using individual JS imports, verify each component's ARIA behavior is preserved. Always test with screen readers after your build completes to confirm interactive elements announce correctly.

## Responsive Behavior

Bootstrap's npm distribution includes all responsive breakpoint utilities by default. The compiled CSS at `node_modules/bootstrap/dist/css/bootstrap.min.css` contains the full responsive grid system, responsive typography, and display utilities. No additional configuration is needed for responsive behavior — it is baked into the compiled output. If you later customize via Sass (covered in the next module), you can selectively include breakpoint-specific utilities to reduce CSS size.
