---
title: "Dependency Management"
category: "Maintenance Patterns"
difficulty: 2
estimated_time: "25 minutes"
prerequisites: ["npm fundamentals", "Version updates"]
tags: ["bootstrap", "dependencies", "npm-audit", "dependabot", "lock-file"]
---

# Dependency Management

## Overview

Bootstrap does not exist in isolation — it depends on **Popper.js** for tooltips and dropdowns, and your project likely includes additional Bootstrap plugins, icon libraries, and build tools. Effective dependency management ensures your project remains stable, secure, and reproducible across environments. This involves maintaining lock files, understanding peer dependencies, running security audits, and automating update notifications. Poor dependency management leads to "works on my machine" bugs, security vulnerabilities, and upgrade nightmares.

## Basic Implementation

**Understanding Bootstrap's peer dependencies:**

```json
// package.json
{
  "dependencies": {
    "bootstrap": "^5.3.3",
    "@popperjs/core": "^2.11.8"
  }
}
```

Bootstrap requires **@popperjs/core** as a peer dependency for tooltip, popover, and dropdown positioning. Install it explicitly.

**Locking your dependency tree:**

```bash
# Generate or update the lock file
npm install

# Verify lock file integrity
npm ci
```

Always commit `package-lock.json` to version control for reproducible builds.

**Running a security audit:**

```bash
# Run npm security audit
npm audit

# Auto-fix vulnerabilities where possible
npm audit fix

# Force fixes (use with caution)
npm audit fix --force
```

## Advanced Variations

**Automated dependency updates with Dependabot:**

```yaml
# .github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "npm"
    directory: "/"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 10
    reviewers:
      - "frontend-team"
    labels:
      - "dependencies"
    allow:
      - dependency-name: "bootstrap"
      - dependency-name: "@popperjs/core"
```

**Dependency health check script:**

```bash
#!/bin/bash
echo "=== Outdated Dependencies ==="
npm outdated

echo "=== Security Audit ==="
npm audit --audit-level=moderate

echo "=== Duplicate Dependencies ==="
npm dedupe --dry-run
```

**Using npm-ci for deterministic installs in CI/CD:**

```yaml
# .github/workflows/ci.yml
steps:
  - uses: actions/checkout@v4
  - uses: actions/setup-node@v4
    with:
      node-version: '20'
      cache: 'npm'
  - run: npm ci
  - run: npm test
```

## Best Practices

1. **Always commit `package-lock.json`** to ensure reproducible installs.
2. **Use `npm ci` in CI/CD** — it installs exactly what the lock file specifies.
3. **Run `npm audit` regularly** — schedule it in your CI pipeline.
4. **Pin Bootstrap's peer dependencies** explicitly in your `package.json`.
5. **Use Dependabot or Renovate** for automated update pull requests.
6. **Review dependency update PRs** before merging — don't auto-merge blindly.
7. **Keep `@popperjs/core` version aligned** with Bootstrap's requirements.
8. **Avoid duplicate dependencies** — run `npm dedupe` periodically.
9. **Use `npm ls bootstrap`** to verify the installed version and its dependency tree.
10. **Document dependency decisions** — why specific versions are pinned or ranges chosen.
11. **Set up audit-level thresholds** in CI to fail builds on high-severity vulnerabilities.
12. **Remove unused dependencies** — use `depcheck` to identify them.

## Common Pitfalls

1. **Not committing the lock file** — leads to inconsistent builds across team members and CI.
2. **Ignoring `npm audit` warnings** — unpatched vulnerabilities can be exploited in production.
3. **Mismatched Popper.js versions** — Bootstrap may behave unexpectedly if Popper.js is incompatible.
4. **Using `npm ci` with a stale lock file** — always regenerate the lock file after dependency changes.
5. **Auto-merging Dependabot PRs** — always review and test before merging automated updates.
6. **Mixing package managers** — using both `npm` and `yarn` creates conflicting lock files.
7. **Forgetting transitive dependencies** — Bootstrap's dependencies may have their own vulnerabilities.

## Accessibility Considerations

Dependency management indirectly affects accessibility. Outdated Bootstrap versions may lack **recent accessibility fixes** for screen readers or keyboard navigation. Keeping dependencies current ensures your application benefits from the latest ARIA improvements and focus management enhancements. Run accessibility audits after dependency updates to catch any regressions.

## Responsive Behavior

Dependency updates rarely directly alter responsive behavior, but version mismatches between Bootstrap and Popper.js can cause **positioning issues** in responsive dropdowns and tooltips. Ensure Popper.js is compatible with your Bootstrap version to maintain correct positioning across all viewport sizes. Test dropdown and tooltip placement at mobile, tablet, and desktop breakpoints after any dependency change.
