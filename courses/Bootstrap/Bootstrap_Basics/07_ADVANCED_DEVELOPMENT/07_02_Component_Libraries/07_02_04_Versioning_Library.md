---
title: "Versioning Library"
difficulty: 2
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - Semantic Versioning
  - Conventional Commits
  - Changelog Generation
---

## Overview

Semantic versioning for component libraries follows the MAJOR.MINOR.PATCH convention with specific rules for what constitutes a breaking change in a Bootstrap-based component library. Breaking changes include removing components, changing prop names, altering HTML output structure, or modifying default behaviors. Minor versions add new components or non-breaking enhancements. Patch versions fix bugs without changing the API surface.

A robust versioning strategy includes automated changelog generation from conventional commits, pre-release channels for beta testing, migration guides for major versions, and compatibility matrices showing which library versions work with which Bootstrap versions. Tools like Changesets, semantic-release, or standard-version automate the release process.

## Basic Implementation

```json
// package.json
{
  "name": "@company/bootstrap-components",
  "version": "2.3.1",
  "repository": {
    "type": "git",
    "url": "https://github.com/company/bootstrap-components"
  }
}
```

```js
// .changeset/config.json
{
  "$schema": "https://unpkg.com/@changesets/config@3.0.0/schema.json",
  "changelog": "@changesets/cli/changelog",
  "commit": false,
  "fixed": [],
  "linked": [],
  "access": "restricted",
  "baseBranch": "main",
  "updateInternalDependencies": "patch",
  "ignore": []
}
```

```markdown
<!-- .changeset/add-data-table-sorting.md -->
---
"@company/bootstrap-components": minor
---

Added sortable columns feature to DataTable component. New `sortable` prop enables
click-to-sort on column headers with ascending/descending indicators.
```

```js
// scripts/validate-compatibility.js
const pkg = require('../package.json');
const semver = require('semver');

const BOOTSTRAP_COMPAT = {
  '1.x': '^5.2.0',
  '2.x': '^5.3.0',
  '3.x': '^5.3.0'
};

const peerRange = pkg.peerDependencies.bootstrap;
const libMajor = semver.major(pkg.version);

Object.entries(BOOTSTRAP_COMPAT).forEach(([range, bsRange]) => {
  const [min] = range.split('.');
  if (libMajor === parseInt(min)) {
    if (peerRange !== bsRange) {
      console.error(`Version ${pkg.version} requires bootstrap ${bsRange}, got ${peerRange}`);
      process.exit(1);
    }
  }
});
```

## Advanced Variations

```js
// Automated release with semantic-release
// .releaserc.js
module.exports = {
  branches: ['main', { name: 'beta', prerelease: true }],
  plugins: [
    '@semantic-release/commit-analyzer',
    '@semantic-release/release-notes-generator',
    ['@semantic-release/changelog', { changelogFile: 'CHANGELOG.md' }],
    ['@semantic-release/npm', { npmPublish: true }],
    ['@semantic-release/github', {
      successComment: false,
      failTitle: false
    }],
    ['@semantic-release/git', {
      assets: ['CHANGELOG.md', 'package.json'],
      message: 'chore(release): ${nextRelease.version} [skip ci]'
    }]
  ]
};
```

```js
// Breaking change detection
// scripts/check-breaking-changes.js
const fs = require('fs');
const { execSync } = require('child_process');

function detectBreakingChanges() {
  const mainApi = JSON.parse(fs.readFileSync('api-snapshot-main.json', 'utf8'));
  const currentApi = extractCurrentApi();

  const breaking = {
    removed: mainApi.filter(m => !currentApi.find(c => c.name === m.name)),
    changed: currentApi.filter(c => {
      const main = mainApi.find(m => m.name === c.name);
      return main && JSON.stringify(main.props) !== JSON.stringify(c.props);
    })
  };

  if (breaking.removed.length || breaking.changed.length) {
    console.error('Breaking changes detected:');
    breaking.removed.forEach(m => console.error(`  Removed: ${m.name}`));
    breaking.changed.forEach(m => console.error(`  Changed: ${m.name}`));

    const version = JSON.parse(fs.readFileSync('package.json')).version;
    if (!semver.major(version)) {
      process.exit(1);
    }
  }
}
```

## Best Practices

1. **Follow semver strictly** - MAJOR for breaking, MINOR for features, PATCH for fixes.
2. **Use Changesets** - Require changesets in PRs to automate changelog generation.
3. **Maintain a compatibility matrix** - Document which library version works with which Bootstrap version.
4. **Create migration guides** - Every major version needs a step-by-step migration document.
5. **Deprecate before removing** - Mark deprecated APIs with console warnings for at least one minor version before removal.
6. **Automate releases** - Use semantic-release or Changesets to publish from CI/CD.
7. **Tag pre-releases** - Use `-beta.1`, `-rc.1` suffixes for testing before stable releases.
8. **Version SCSS and JS together** - Never release mismatched versions of CSS and JS components.
9. **Maintain a changelog** - Human-readable CHANGELOG.md alongside automated release notes.
10. **Communicate breaking changes** - Post announcements before major version releases.

## Common Pitfalls

1. **Silent breaking changes** - Changing HTML output structure without bumping major version breaks consumers' CSS overrides.
2. **Incompatible peer dependencies** - Not updating `peerDependencies` when Bootstrap API changes.
3. **Missing changelogs** - Releasing without documenting what changed makes upgrades risky.
4. **Version drift** - Different teams on different major versions increases maintenance burden.
5. **Pre-release in production** - Publishing beta versions to the `latest` npm tag.

## Accessibility Considerations

Breaking accessibility improvements should be treated as patch releases if they fix WCAG violations, even if they change HTML output. Document these as "Accessibility Fixes" in changelogs.

## Responsive Behavior

Changes to responsive behavior (breakpoints, mobile layouts) are breaking changes if they alter visual output at existing breakpoints. Document responsive changes clearly in release notes.
