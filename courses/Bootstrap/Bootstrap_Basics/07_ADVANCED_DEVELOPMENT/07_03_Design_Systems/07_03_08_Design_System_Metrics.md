---
title: "Design System Metrics"
difficulty: 2
category: "Advanced Development"
subcategory: "Design Systems"
prerequisites:
  - Analytics Tracking
  - Build-time Analysis
  - Compliance Scoring
---

## Overview

Design system metrics track adoption, usage patterns, and compliance across consuming projects. Metrics answer critical questions: which components are most used, which are ignored, how many projects use the latest version, and whether teams are following design system guidelines. This data drives prioritization, identifies training needs, and demonstrates the design system's value to leadership.

Metrics collection happens at build time (analyzing imports and component usage in source code), at runtime (tracking component rendering in production), and through periodic audits (comparing project implementations against design system standards).

## Basic Implementation

```js
// scripts/analyze-usage.js
const fs = require('fs');
const path = require('path');
const glob = require('glob');

function analyzeUsage(projectDir) {
  const files = glob.sync(`${projectDir}/**/*.{html,js,jsx,ts,tsx,vue}`);
  const components = {};

  // Define known components
  const componentPatterns = [
    { name: 'Card', pattern: /class="card"/g },
    { name: 'Modal', pattern: /data-bs-toggle="modal"/g },
    { name: 'Alert', pattern: /class="alert/g },
    { name: 'Button', pattern: /class="btn\s/g },
    { name: 'Navbar', pattern: /class="navbar/g },
    { name: 'Table', pattern: /class="table/g }
  ];

  files.forEach(file => {
    const content = fs.readFileSync(file, 'utf8');
    componentPatterns.forEach(({ name, pattern }) => {
      const matches = content.match(pattern);
      if (matches) {
        components[name] = (components[name] || 0) + matches.length;
      }
    });
  });

  return {
    project: path.basename(projectDir),
    filesScanned: files.length,
    components,
    totalUsages: Object.values(components).reduce((a, b) => a + b, 0)
  };
}

// Generate report
const projects = ['app-one', 'app-two', 'app-three'];
const report = projects.map(p => analyzeUsage(`../${p}`));

console.table(report.map(r => ({
  Project: r.project,
  Files: r.filesScanned,
  Components: Object.keys(r.components).length,
  Usages: r.totalUsages
})));
```

```json
// metrics-output.json
{
  "generated": "2025-01-15T10:00:00Z",
  "summary": {
    "totalProjects": 12,
    "adoptionRate": "87%",
    "latestVersionUsage": "72%",
    "avgComponentsPerProject": 15
  },
  "componentUsage": {
    "Button": { "count": 2340, "projects": 12 },
    "Card": { "count": 1560, "projects": 11 },
    "Modal": { "count": 430, "projects": 9 },
    "DataTable": { "count": 180, "projects": 6 },
    "Alert": { "count": 890, "projects": 12 }
  },
  "versionDistribution": {
    "2.3.0": 8,
    "2.2.1": 3,
    "2.1.0": 1
  }
}
```

## Best Practices

1. **Track adoption rate** - Measure what percentage of projects use the design system.
2. **Monitor version distribution** - Know how many projects are on outdated versions.
3. **Measure component popularity** - Identify most and least used components for prioritization.
4. **Automate collection** - Build metrics collection into CI/CD pipelines.
5. **Create dashboards** - Visualize metrics for leadership and team visibility.
6. **Track compliance** - Measure adherence to design system guidelines.
7. **Report regularly** - Share metrics monthly or quarterly with stakeholders.
8. **Act on data** - Deprecate unused components, improve unpopular ones.
9. **Respect privacy** - Don't collect user-identifiable data in metrics.
10. **Benchmark over time** - Track trends, not just point-in-time snapshots.

## Common Pitfalls

1. **Collecting without acting** - Metrics without follow-up actions are wasted effort.
2. **Over-instrumentation** - Too many runtime metrics impacts performance.
3. **Gaming metrics** - Teams inflating usage numbers to appear compliant.
4. **Ignoring context** - Low usage doesn't mean a component is bad; it may be niche.
5. **Privacy violations** - Tracking too much about how users interact with components.

## Accessibility Considerations

Track accessibility compliance as a metric: what percentage of components pass axe audits, how many projects use proper ARIA patterns.

## Responsive Behavior

Track responsive usage: what percentage of projects use responsive grid classes, which breakpoints are most common.
