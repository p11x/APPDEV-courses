---
title: "CI/CD Pipeline for Bootstrap 5"
section: "05_04_Deployment_Guides"
file: "05_04_07_CI_CD_Pipeline.md"
difficulty: 3
tags: ["ci-cd", "github-actions", "testing", "automation", "artifacts"]
duration: "15 minutes"
prerequisites:
  - "GitHub repository configured"
  - "Production build pipeline working locally"
  - "Deployment target configured (Netlify/Vercel/AWS)"
learning_objectives:
  - "Create a GitHub Actions workflow for Bootstrap builds"
  - "Implement automated testing and deployment triggers"
  - "Manage build artifacts and environment secrets"
---

# CI/CD Pipeline for Bootstrap 5

## Overview

A CI/CD (Continuous Integration / Continuous Deployment) pipeline automates building, testing, and deploying your Bootstrap project on every code change. GitHub Actions provides a free, integrated CI/CD platform that triggers workflows on push, pull request, or schedule events.

A well-configured pipeline catches broken builds before they reach production, runs accessibility and visual regression tests, and deploys preview environments for every pull request. This eliminates manual deployment steps and ensures every release is reproducible.

---

## Basic Implementation

### Complete Build + Deploy Workflow

```yaml
# .github/workflows/deploy.yml
name: Build and Deploy

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: npm

      - run: npm ci

      - run: npm run build

      - name: Upload build artifact
        uses: actions/upload-artifact@v4
        with:
          name: dist
          path: dist/
          retention-days: 7

  deploy:
    needs: build
    if: github.ref == 'refs/heads/main' && github.event_name == 'push'
    runs-on: ubuntu-latest
    steps:
      - uses: actions/download-artifact@v4
        with:
          name: dist
          path: dist/

      - name: Deploy to Netlify
        uses: nwtgck/actions-netlify@v3
        with:
          publish-dir: dist
          production-deploy: true
        env:
          NETLIFY_AUTH_TOKEN: ${{ secrets.NETLIFY_AUTH_TOKEN }}
          NETLIFY_SITE_ID: ${{ secrets.NETLIFY_SITE_ID }}
```

---

## Advanced Variations

### Automated Testing + Lighthouse Audit

```yaml
# .github/workflows/test.yml
name: Test and Audit

on: [pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: 20, cache: npm }
      - run: npm ci
      - run: npm run lint
      - run: npm run test --if-present

  lighthouse:
    needs: test
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: 20, cache: npm }
      - run: npm ci && npm run build
      - name: Run Lighthouse
        uses: treosh/lighthouse-ci-action@v11
        with:
          uploadArtifacts: true
          configPath: .lighthouserc.json
```

```json
// .lighthouserc.json
{
  "ci": {
    "collect": {
      "staticDistDir": "./dist",
      "numberOfRuns": 3
    },
    "assert": {
      "assertions": {
        "categories:accessibility": ["error", { "minScore": 0.95 }],
        "categories:performance": ["warn", { "minScore": 0.8 }]
      }
    }
  }
}
```

### Preview Deployments on Pull Requests

```yaml
preview:
  if: github.event_name == 'pull_request'
  runs-on: ubuntu-latest
  steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-node@v4
      with: { node-version: 20, cache: npm }
    - run: npm ci && npm run build
    - name: Deploy Preview
      uses: nwtgck/actions-netlify@v3
      with:
        publish-dir: dist
        production-deploy: false
        alias: pr-${{ github.event.pull_request.number }}
      env:
        NETLIFY_AUTH_TOKEN: ${{ secrets.NETLIFY_AUTH_TOKEN }}
        NETLIFY_SITE_ID: ${{ secrets.NETLIFY_SITE_ID }}
```

---

## Best Practices

1. **Use `npm ci` not `npm install`** — faster, deterministic, fails on lock file mismatches
2. **Cache `node_modules`** with `actions/setup-node` `cache: npm` — reduces build time by 50-70%
3. **Store secrets in GitHub Secrets** — never hardcode API tokens, deploy keys, or credentials
4. **Run tests before deploy** — catch broken builds before they reach production
5. **Use `upload-artifact` / `download-artifact`** — pass build output between jobs without rebuilding
6. **Set `retention-days: 7`** on artifacts — GitHub free tier has storage limits
7. **Deploy only on `main` branch pushes** — PRs build and test but don't deploy to production
8. **Pin action versions** (`uses: actions/checkout@v4`) — prevents unexpected breaking changes
9. **Use `concurrency` groups** — cancel in-progress deploys when new commits are pushed
10. **Add Lighthouse CI assertions** — fail the build if accessibility or performance scores drop below threshold
11. **Separate build and deploy jobs** — build runs on all events, deploy only on merge to main
12. **Use `environment` protection rules** — require manual approval for production deploys

---

## Common Pitfalls

1. **Missing `npm ci` caching** — every workflow run downloads 200MB+ of node_modules from scratch
2. **Exposing secrets in logs** — `echo $SECRET` or verbose npm scripts leak credentials in workflow output
3. **Deploying on PR events** — accidentally pushing preview builds to production
4. **Not setting `if` conditions on deploy jobs** — deploys run on every push to every branch
5. **Hardcoded environment variables in workflow YAML** — use repository secrets or environment variables instead
6. **Ignoring workflow failures** — `continue-on-error: true` hides real problems; fix them instead
7. **Not using `concurrency`** — rapid pushes queue multiple deploys, wasting build minutes
8. **Forgetting `permissions` block** — default token permissions may be insufficient for deploy actions

---

## Accessibility Considerations

Integrate automated accessibility testing into the CI pipeline using `axe-core` or Lighthouse CI. Set a minimum accessibility score threshold (e.g., 95%) that fails the build if violated. This catches regressions in ARIA attributes, color contrast, and keyboard navigation before they reach production.

```yaml
- name: Accessibility Test
  run: npx pa11y-ci --config .pa11yci.json dist/index.html
```

---

## Responsive Behavior

CI pipelines should run visual regression tests at multiple viewport widths to catch responsive layout breaks. Use tools like `backstopjs` configured with Bootstrap's breakpoint widths (576, 766, 992, 1200, 1400px) to detect unintended responsive changes:

```json
// backstop.json
{
  "viewports": [
    { "label": "mobile", "width": 375, "height": 667 },
    { "label": "tablet", "width": 768, "height": 1024 },
    { "label": "desktop", "width": 1200, "height": 800 }
  ]
}
```
