---
title: "Code Quality Tools for Bootstrap 5 Projects"
module: "Development Workflow"
difficulty: 2
estimated_time: 30 min
prerequisites:
  - NPM setup (05_02_01)
  - Basic understanding of linting concepts
tags:
  - eslint
  - stylelint
  - prettier
  - code-quality
---

# Code Quality Tools for Bootstrap 5 Projects

## Overview

Code quality tools enforce consistent formatting, catch bugs, and maintain style standards across your Bootstrap 5 project. ESLint validates JavaScript, including Bootstrap's interactive components and your custom scripts. Stylelint checks CSS/SCSS for errors, deprecated properties, and adherence to naming conventions. Prettier auto-formats code to a uniform style, eliminating debates over spacing and quotes. Together, these tools integrate into your editor and CI pipeline, catching issues before they reach production. This guide covers installing, configuring, and integrating each tool with a Bootstrap 5 project.

## Basic Implementation

Install ESLint, Stylelint, and Prettier:

```bash
npm install eslint stylelint stylelint-config-standard-scss prettier eslint-config-prettier --save-dev
```

Initialize ESLint and Stylelint configuration files:

```json
{
  "scripts": {
    "lint:js": "eslint 'src/js/**/*.js'",
    "lint:css": "stylelint 'src/scss/**/*.scss'",
    "format": "prettier --write 'src/**/*.{js,scss,html}'",
    "lint:all": "npm run lint:js && npm run lint:css"
  },
  "devDependencies": {
    "eslint": "^9.5.0",
    "stylelint": "^16.6.0",
    "stylelint-config-standard-scss": "^13.1.0",
    "prettier": "^3.3.0",
    "eslint-config-prettier": "^9.1.0"
  }
}
```

Configure Prettier in `.prettierrc`:

```json
{
  "semi": true,
  "singleQuote": true,
  "trailingComma": "es5",
  "printWidth": 100,
  "tabWidth": 2,
  "htmlWhitespaceSensitivity": "ignore"
}
```

## Advanced Variations

Configure Stylelint specifically for Bootstrap SCSS projects to enforce consistent variable naming and catch deprecated Bootstrap class usage:

```bash
npx stylelint --init
```

Create `.stylelintrc.json` with SCSS-specific rules:

```json
{
  "extends": ["stylelint-config-standard-scss"],
  "rules": {
    "selector-class-pattern": "^([a-z][a-z0-9]*)(-[a-z0-9]+)*$",
    "scss/dollar-variable-pattern": "^_?[a-z][a-z0-9-]*$",
    "max-nesting-depth": 4,
    "no-descending-specificity": null,
    "property-no-vendor-prefix": true,
    "declaration-block-no-duplicate-properties": true
  },
  "ignoreFiles": ["node_modules/**/*.scss", "dist/**/*.css"]
}
```

Set up ESLint with HTML validation for inline Bootstrap JavaScript:

```bash
npx eslint --init
```

Configure `.eslintrc.json` for browser globals used by Bootstrap:

```json
{
  "env": {
    "browser": true,
    "es2022": true
  },
  "extends": ["eslint:recommended", "prettier"],
  "rules": {
    "no-unused-vars": ["error", { "argsIgnorePattern": "^_" }],
    "no-console": "warn",
    "prefer-const": "error"
  }
}
```

Integrate HTML validation with `html-validate` for Bootstrap template structure:

```bash
npm install html-validate --save-dev
```

```json
{
  "scripts": {
    "lint:html": "html-validate 'src/**/*.html'"
  }
}
```

## Best Practices

1. **Run linting in CI/CD** — add `npm run lint:all` to your build pipeline to block merges with lint errors.
2. **Use `eslint-config-prettier`** — disables ESLint formatting rules that conflict with Prettier; avoids double-fixing.
3. **Configure editor integration** — install ESLint and Prettier VS Code extensions for real-time feedback.
4. **Use `stylelint-config-standard-scss`** — extends standard CSS rules with SCSS-specific support.
5. **Enforce Bootstrap naming conventions** — Stylelint's `selector-class-pattern` ensures custom classes follow Bootstrap's kebab-case convention.
6. **Set `htmlWhitespaceSensitivity: "ignore"`** in Prettier — prevents reformatting that breaks Bootstrap's inline spacing.
7. **Lint before commit** — use `husky` and `lint-staged` to run linters only on staged files.
8. **Use `.eslintignore` and `.stylelintignore`** — exclude `dist/`, `node_modules/`, and vendor files from linting.
9. **Pin tool versions** — lint rule changes between versions can suddenly flag new errors in existing code.
10. **Auto-fix where safe** — run `eslint --fix` and `stylelint --fix` for auto-correctable issues; review changes before committing.
11. **Document lint rules in your README** — explain non-obvious rule choices so new contributors understand the configuration.

## Common Pitfalls

1. **ESLint and Prettier conflicts** — without `eslint-config-prettier`, both tools fight over formatting rules, producing inconsistent results.
2. **Linting compiled CSS** — running Stylelint on `dist/css/` catches issues from the compiler, not your source; always lint SCSS source files.
3. **Overly strict rules** — Bootstrap's internal class names may not match your `selector-class-pattern`; exclude `node_modules` from Stylelint.
4. **Missing `env` configuration** — ESLint without `browser: true` does not recognize `document`, `window`, or Bootstrap's `bootstrap.*` global.
5. **Prettier reformatting HTML templates** — Bootstrap HTML with inline attributes can be mangled; use `htmlWhitespaceSensitivity: "ignore"`.
6. **Stylelint `no-descending-specificity` false positives** — Bootstrap's layered SCSS often triggers this rule; disable it for Bootstrap imports.
7. **Forgetting `husky` hooks** — lint-staged only runs when git hooks are configured; without husky, pre-commit hooks do not execute.

## Accessibility Considerations

HTML validators like `html-validate` can catch accessibility issues in Bootstrap templates, such as missing `alt` attributes, invalid ARIA roles, and broken landmark structures. Configure `html-validate` with the `recommended` preset to enforce WCAG-related rules. ESLint plugins like `eslint-plugin-jsx-a11y` (for React projects using React-Bootstrap) catch accessibility violations in component markup. Combine automated linting with manual screen reader testing for comprehensive accessibility coverage.

## Responsive Behavior

Code quality tools do not directly affect responsive behavior, but they ensure the CSS and SCSS that drive responsiveness is well-formed. Stylelint catches errors in media query syntax, missing breakpoints, and incorrect use of responsive utility classes. Prettier's consistent formatting makes responsive SCSS easier to review, reducing the chance of breakpoint logic errors. When linting Bootstrap SCSS overrides, verify that responsive variable changes (e.g., custom `$grid-breakpoints`) pass Stylelint validation.
