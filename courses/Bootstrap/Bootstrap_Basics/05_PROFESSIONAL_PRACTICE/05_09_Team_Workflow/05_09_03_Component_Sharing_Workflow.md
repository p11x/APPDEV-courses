---
title: "Component Sharing Workflow Across Teams"
module: "Team Workflow"
difficulty: 3
estimated_time: 30
tags: ["components", "sharing", "packages", "monorepo"]
prerequisites: ["npm packaging", "Bootstrap customization", "Build tools"]
---

## Overview

Sharing Bootstrap components across teams requires a structured workflow that handles versioning, distribution, documentation, and updates. Whether using an internal npm registry, a monorepo, or a component library, teams need clear processes for publishing, consuming, and maintaining shared components. This guide covers practical approaches to component sharing at different organizational scales.

## Basic Implementation

**Internal npm Package Setup**

Package reusable Bootstrap components as an installable npm module.

```json
// packages/ui-components/package.json
{
  "name": "@company/ui-components",
  "version": "2.1.0",
  "main": "dist/index.js",
  "style": "dist/ui-components.css",
  "peerDependencies": {
    "bootstrap": "^5.3.0"
  },
  "files": [
    "dist/",
    "src/",
    "README.md"
  ],
  "publishConfig": {
    "registry": "https://npm.company.com"
  }
}
```

**Package Structure**

```
packages/ui-components/
  src/
    scss/
      _index.scss
      _feature-card.scss
      _data-table.scss
      _alert-banner.scss
    js/
      index.js
      feature-card.js
      data-table.js
    components/
      FeatureCard.vue
      DataTable.vue
  dist/                  # Built output
  docs/                  # Component documentation
  package.json
  README.md
  CHANGELOG.md
```

**Consuming Shared Components**

```bash
# Install from internal registry
npm install @company/ui-components@latest

# Or in a monorepo workspace
npm install @company/ui-components@workspace:*
```

```scss
// Consumer project's main.scss
@import "bootstrap/scss/bootstrap";
@import "@company/ui-components/scss/index";
```

## Advanced Variations

**Monorepo Component Management**

Use a monorepo tool to manage shared components alongside applications.

```json
// Root package.json
{
  "workspaces": [
    "packages/*",
    "apps/*"
  ]
}
```

```
monorepo/
  packages/
    ui-components/     # Shared Bootstrap components
    design-tokens/     # SCSS variables and tokens
    eslint-config/     # Shared linting rules
  apps/
    dashboard/         # Consumer app 1
    marketing/         # Consumer app 2
  package.json
  turbo.json           # Turborepo configuration
```

```json
// turbo.json
{
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    }
  }
}
```

**Versioning Strategy**

Use semantic versioning with automated release management.

```json
// .changeset/config.json
{
  "changelog": "@changesets/cli/changelog",
  "commit": false,
  "fixed": [],
  "linked": [],
  "access": "restricted",
  "baseBranch": "main",
  "updateInternalDependencies": "patch"
}
```

```bash
# Create a changeset for component changes
npx changeset
# Select packages that changed
# Describe the change
# Choose version bump (patch/minor/major)

# Apply changesets and publish
npx changeset version
npm run build
npm publish --access restricted
```

**Component Registry with Documentation**

```javascript
// scripts/generate-registry.js
const fs = require('fs');
const path = require('path');

const componentsDir = path.join(__dirname, '../src/components');
const components = fs.readdirSync(componentsDir);

const registry = components.map(name => {
  const docPath = path.join(componentsDir, name, 'README.md');
  const hasDocs = fs.existsSync(docPath);
  return {
    name,
    path: `src/components/${name}`,
    hasDocs,
    version: require('../package.json').version
  };
});

fs.writeFileSync(
  path.join(__dirname, '../dist/registry.json'),
  JSON.stringify(registry, null, 2)
);
```

## Best Practices

1. **Use scoped npm packages** (`@company/`) to avoid naming conflicts
2. **Pin Bootstrap as a peer dependency** - consumers provide their own Bootstrap
3. **Version components independently** when they have different change frequencies
4. **Maintain a changelog per package** for clear upgrade paths
5. **Provide TypeScript definitions** even for vanilla JS components
6. **Include comprehensive README** with installation and usage instructions
7. **Automate publishing** with CI/CD pipelines
8. **Run visual regression tests** across consuming applications
9. **Document breaking changes** with migration guides
10. **Use internal npm registry** (Verdaccio, Artifactory) for private packages
11. **Establish a component review process** before publishing
12. **Tag stable versions** and maintain an LTS version when needed

## Common Pitfalls

1. **Duplicating Bootstrap's own components** - wrapping basic cards and buttons unnecessarily
2. **No versioning strategy** - breaking changes deployed without warning
3. **Missing peer dependencies** - Bootstrap is bundled multiple times
4. **No documentation** - consuming teams cannot discover or use components
5. **Tight coupling to specific Bootstrap versions** - upgrades break shared components
6. **No testing in consuming apps** - components work in isolation but break in integration
7. **Monolithic packages** - one package contains everything, forcing unnecessary imports
8. **No ownership model** - no team responsible for maintaining shared components
9. **Ignoring tree-shaking** - consumers cannot import individual components
10. **No automated publishing** - manual releases are error-prone and delayed

## Accessibility Considerations

Shared components must have accessibility baked in by default. Include accessibility testing in the component publishing pipeline. Document ARIA requirements that consuming teams must fulfill. Provide accessibility-focused examples showing correct implementation patterns. Run automated accessibility audits before every release.

## Responsive Behavior

Shared components must work responsively out of the box. Document responsive behavior in component specs. Include breakpoint configuration options where components need different behavior on mobile vs. desktop. Test shared components in all consuming applications at multiple viewport widths.
