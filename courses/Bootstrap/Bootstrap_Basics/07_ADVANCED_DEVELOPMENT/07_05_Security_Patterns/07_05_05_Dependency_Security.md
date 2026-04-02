---
title: "Dependency Security"
difficulty: 2
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - npm audit
  - Snyk / Dependabot
  - Vulnerability Scanning
---

## Overview

Dependency security for Bootstrap applications involves scanning npm packages for known vulnerabilities, keeping Bootstrap and its dependencies updated, and automating security checks in CI/CD pipelines. Bootstrap 5 depends on Popper.js (for tooltips and dropdowns), and both must be kept current to address security patches.

The npm ecosystem has tools for every stage: `npm audit` for quick vulnerability checks, Dependabot or Renovate for automated update PRs, and Snyk or Socket for deep dependency analysis including transitive dependencies and supply chain risks.

## Basic Implementation

```bash
# Check for vulnerabilities
npm audit

# Fix automatically
npm audit fix

# Fix with breaking changes (major version updates)
npm audit fix --force

# Check specific package
npm ls bootstrap
npm ls @popperjs/core
```

```yaml
# GitHub Dependabot configuration
# .github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "npm"
    directory: "/"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 10
    reviewers:
      - "security-team"
    labels:
      - "dependencies"
      - "security"
    allow:
      - dependency-name: "bootstrap"
      - dependency-name: "@popperjs/core"
    groups:
      bootstrap:
        patterns:
          - "bootstrap"
          - "@popperjs/core"
```

```yaml
# CI security scanning
# .github/workflows/security.yml
name: Security Scan
on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  schedule:
    - cron: '0 6 * * 1' # Weekly Monday scan

jobs:
  audit:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
      - run: npm ci
      - run: npm audit --audit-level=high
      - name: Snyk Security Scan
        uses: snyk/actions/node@master
        with:
          args: --severity-threshold=high
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}
```

## Advanced Variations

```json
// package.json with security scripts
{
  "scripts": {
    "security:audit": "npm audit --audit-level=moderate",
    "security:check": "npx better-npm-audit audit",
    "security:outdated": "npm outdated",
    "security:license": "license-checker --onlyAllow 'MIT;ISC;BSD-2-Clause;BSD-3-Clause;Apache-2.0'"
  }
}
```

```js
// Custom dependency check script
// scripts/check-deps.js
const { execSync } = require('child_process');
const semver = require('semver');

function checkDependencyHealth() {
  const outdated = JSON.parse(execSync('npm outdated --json 2>/dev/null || echo "{}"').toString());
  const critical = [];

  Object.entries(outdated).forEach(([pkg, info]) => {
    if (semver.diff(info.current, info.latest) === 'major') {
      critical.push({ package: pkg, ...info });
    }
  });

  if (critical.length) {
    console.warn('Major version updates available:');
    critical.forEach(c => console.warn(`  ${c.package}: ${c.current} → ${c.latest}`));
  }

  const audit = JSON.parse(execSync('npm audit --json 2>/dev/null').toString());
  const highVulns = audit.metadata?.vulnerabilities?.high || 0;
  const criticalVulns = audit.metadata?.vulnerabilities?.critical || 0;

  if (highVulns > 0 || criticalVulns > 0) {
    console.error(`Found ${criticalVulns} critical and ${highVulns} high vulnerabilities`);
    process.exit(1);
  }
}
```

## Best Practices

1. **Run npm audit in CI** - Block merges with high/critical vulnerabilities.
2. **Use Dependabot or Renovate** - Automated PRs for dependency updates.
3. **Pin Bootstrap version** - Use exact version or tight semver range for predictability.
4. **Monitor transitive dependencies** - Bootstrap's dependencies have their own vulnerabilities.
5. **Review lock file changes** - `package-lock.json` changes can introduce supply chain attacks.
6. **Use npm ci in CI** - `npm ci` uses lock file exactly; `npm install` can update it.
7. **Check license compliance** - Ensure dependencies have compatible licenses.
8. **Enable 2FA on npm** - Protect the npm account that publishes your packages.
9. **Audit after major updates** - Run full audit after Bootstrap major version upgrades.
10. **Use Socket or Snyk** - Deep analysis beyond what npm audit provides.

## Common Pitfalls

1. **Ignoring npm audit warnings** - Low-severity issues compound into real vulnerabilities.
2. **Auto-merging Dependabot PRs** - Breaking changes from major updates can break the application.
3. **Not updating lock file** - Lock file drift from package.json causes inconsistent builds.
4. **Missing transitive dependency scanning** - Direct dependencies are checked, but their dependencies are not.
5. **Using deprecated packages** - Bootstrap 4's jQuery dependency is a known issue; always use Bootstrap 5.

## Accessibility Considerations

Dependency updates should not break accessibility. Include accessibility tests in CI to catch regressions from dependency updates.

## Responsive Behavior

Dependency security is independent of responsive design considerations.
