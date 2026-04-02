---
title: "Standards Enforcement for Bootstrap Projects"
module: "Team Workflow"
difficulty: 2
estimated_time: 20
tags: ["linting", "CI", "pre-commit", "automation"]
prerequisites: ["ESLint", "Stylelint", "Git hooks"]
---

## Overview

Automated standards enforcement catches Bootstrap-related issues before they reach production. Linting rules validate SCSS structure and class naming, pre-commit hooks prevent non-compliant code from being committed, and CI checks ensure every pull request meets quality standards. This guide covers the tools and configurations needed to enforce Bootstrap coding standards automatically across a team.

## Basic Implementation

**Stylelint Configuration for SCSS**

Enforce SCSS coding standards and Bootstrap-specific patterns.

```json
// .stylelintrc.json
{
  "extends": [
    "stylelint-config-standard-scss",
    "stylelint-config-recess-order"
  ],
  "rules": {
    "selector-class-pattern": "^[a-z][a-z0-9]*(-[a-z0-9]+)*$",
    "scss/at-rule-no-unknown": [
      true,
      {
        "ignoreAtRules": ["tailwind", "apply"]
      }
    ],
    "declaration-no-important": true,
    "max-nesting-depth": 3,
    "color-no-invalid-hex": true,
    "shorthand-property-no-redundant-values": true
  }
}
```

**ESLint for Bootstrap JavaScript**

```json
// .eslintrc.json
{
  "extends": ["eslint:recommended"],
  "rules": {
    "no-unused-vars": "error",
    "no-console": "warn",
    "prefer-const": "error",
    "no-var": "error"
  },
  "overrides": [
    {
      "files": ["src/js/components/**/*.js"],
      "rules": {
        "no-console": "error"
      }
    }
  ]
}
```

**Pre-Commit Hooks with Husky**

```json
// package.json
{
  "devDependencies": {
    "husky": "^9.0.0",
    "lint-staged": "^15.0.0"
  },
  "lint-staged": {
    "*.scss": [
      "stylelint --fix",
      "git add"
    ],
    "*.js": [
      "eslint --fix",
      "git add"
    ],
    "*.html": [
      "htmlhint",
      "git add"
    ]
  }
}
```

```bash
# .husky/pre-commit
npx lint-staged
npm run build:css
```

## Advanced Variations

**CI Pipeline Configuration**

```yaml
# .github/workflows/ci.yml
name: CI
on: [push, pull_request]

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 20
      - run: npm ci
      - run: npm run lint:css
      - run: npm run lint:js
      - run: npm run lint:html

  build:
    needs: lint
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 20
      - run: npm ci
      - run: npm run build
      - name: Check bundle size
        run: |
          SIZE=$(wc -c < dist/css/app.css)
          MAX=200000
          if [ $SIZE -gt $MAX ]; then
            echo "CSS exceeds $MAX bytes: $SIZE"
            exit 1
          fi

  accessibility:
    needs: build
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: npm ci
      - run: npm run test:a11y
```

**Custom Stylelint Rules**

Create project-specific lint rules for Bootstrap conventions.

```javascript
// .stylelint/plugins/bootstrap-class-order.js
const stylelint = require('stylelint');

const ruleName = 'bootstrap/class-order';

stylelint.createPlugin(ruleName, (primaryOption) => {
  return (root, result) => {
    root.walkRules((rule) => {
      const classes = rule.selector.split(/\s+/);
      // Ensure Bootstrap utility classes come after component classes
      const utilIndex = classes.findIndex(c => c.startsWith('.d-') || c.startsWith('.flex-'));
      const compIndex = classes.findIndex(c => c.startsWith('.card') || c.startsWith('.btn'));
      if (utilIndex !== -1 && compIndex !== -1 && utilIndex < compIndex) {
        stylelint.utils.report({
          message: 'Bootstrap utilities should follow component classes',
          node: rule,
          result,
          ruleName
        });
      }
    });
  };
});
```

## Best Practices

1. **Enforce style ordering** with `stylelint-config-recess-order`
2. **Ban `!important` declarations** - use specificity instead
3. **Validate class naming conventions** with regex patterns
4. **Run linters in CI** - every PR must pass automated checks
5. **Use pre-commit hooks** - catch issues before they reach the repository
6. **Set maximum nesting depth** - prevent overly specific SCSS selectors
7. **Enforce mobile-first breakpoint order** in SCSS files
8. **Check bundle size in CI** - fail builds that exceed size thresholds
9. **Lint HTML templates** for Bootstrap markup validity
10. **Auto-fix where possible** - reduce manual correction overhead
11. **Document lint rule rationale** - explain why each rule exists
12. **Use editor integrations** - show lint errors in real-time

## Common Pitfalls

1. **No linting at all** - inconsistent code quality across the team
2. **Overly strict rules** - developers spend more time fighting linters than coding
3. **No auto-fix** - requiring manual fixes for formatting issues
4. **Inconsistent CI and local rules** - code passes locally but fails in CI
5. **Ignoring lint warnings** - warnings should be errors in CI
6. **No pre-commit hooks** - non-compliant code reaches the repository
7. **Banning `!important` without alternatives** - some Bootstrap overrides require it
8. **Not updating lint rules** - rules become outdated as project evolves
9. **No bundle size monitoring** - CSS grows without bounds
10. **Missing HTML linting** - Bootstrap markup errors go undetected

## Accessibility Considerations

Include accessibility lint rules in the enforcement pipeline. Use `eslint-plugin-jsx-a11y` or equivalent for template accessibility checks. Configure CI to run automated accessibility audits (axe-core) on built pages. Set minimum accessibility scores that must be met before merging.

## Responsive Behavior

Enforce mobile-first breakpoint ordering in SCSS with custom lint rules. Validate that responsive utility classes follow the correct sequence. Include responsive testing in CI by checking layouts at multiple viewport widths.
