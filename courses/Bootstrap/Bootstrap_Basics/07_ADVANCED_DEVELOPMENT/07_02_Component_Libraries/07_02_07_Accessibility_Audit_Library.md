---
title: "Accessibility Audit Library"
difficulty: 2
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - axe-core
  - WCAG 2.1 Guidelines
  - CI/CD Pipeline Integration
---

## Overview

Automating accessibility audits for a component library ensures every component meets WCAG compliance before release. The audit pipeline integrates axe-core for automated testing, manual testing protocols for complex interactions, and CI enforcement that blocks releases containing accessibility violations.

The testing pyramid for accessibility includes automated checks (axe-core, HTML validators), assisted manual testing (keyboard navigation, screen reader spot-checks), and comprehensive audits (full WCAG 2.1 AA conformance reports). Each layer catches issues the previous layer misses.

## Basic Implementation

```js
// tests/a11y/axe-setup.js
const { configureAxe } = require('jest-axe');

const axe = configureAxe({
  rules: {
    'color-contrast': { enabled: true },
    'keyboard-navigation': { enabled: true },
    'aria-roles': { enabled: true },
    'aria-required-attr': { enabled: true }
  },
  runOnly: {
    type: 'tag',
    values: ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa']
  }
});

module.exports = axe;
```

```js
// tests/a11y/components.a11y.test.js
const axe = require('./axe-setup');

describe('Component Accessibility Audit', () => {
  const components = [
    { name: 'Card', markup: '<div class="card"><div class="card-body"><h5 class="card-title">Title</h5><p class="card-text">Text</p></div></div>' },
    { name: 'Alert', markup: '<div class="alert alert-info" role="alert">Info message</div>' },
    { name: 'Modal', markup: '<div class="modal" role="dialog" aria-labelledby="title" aria-modal="true"><div class="modal-dialog"><div class="modal-content"><h5 class="modal-title" id="title">Title</h5><div class="modal-body">Body</div></div></div></div>' },
    { name: 'Form', markup: '<form><div class="mb-3"><label for="email" class="form-label">Email</label><input type="email" class="form-control" id="email" required></div></form>' },
    { name: 'Nav', markup: '<nav aria-label="Main"><ul class="nav"><li class="nav-item"><a class="nav-link active" href="#">Home</a></li></ul></nav>' }
  ];

  components.forEach(({ name, markup }) => {
    test(`${name} passes axe accessibility audit`, async () => {
      document.body.innerHTML = markup;
      const results = await axe(document.body);
      expect(results.violations).toEqual([]);
    });
  });
});
```

```yaml
# .github/workflows/a11y-audit.yml
name: Accessibility Audit
on: [pull_request]
jobs:
  a11y:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
      - run: npm ci
      - run: npm run test:a11y
      - name: Upload audit report
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: a11y-report
          path: reports/a11y/
```

## Advanced Variations

```js
// Comprehensive audit with reporting
const axe = require('axe-core');
const fs = require('fs');
const path = require('path');

async function auditComponent(name, markup) {
  const container = document.createElement('div');
  container.innerHTML = markup;
  document.body.appendChild(container);

  const results = await axe.run(container, {
    runOnly: ['wcag2a', 'wcag2aa', 'wcag21aa'],
    resultTypes: ['violations', 'incomplete', 'passes']
  });

  document.body.removeChild(container);

  return {
    component: name,
    violations: results.violations.map(v => ({
      id: v.id,
      impact: v.impact,
      description: v.description,
      help: v.help,
      helpUrl: v.helpUrl,
      nodes: v.nodes.length
    })),
    passes: results.passes.length,
    incomplete: results.incomplete.length
  };
}

async function runFullAudit(components) {
  const report = [];
  for (const comp of components) {
    report.push(await auditComponent(comp.name, comp.markup));
  }

  const summary = {
    total: report.length,
    passing: report.filter(r => r.violations.length === 0).length,
    failing: report.filter(r => r.violations.length > 0).length,
    totalViolations: report.reduce((sum, r) => sum + r.violations.length, 0)
  };

  fs.writeFileSync(
    path.join('reports', 'a11y-audit.json'),
    JSON.stringify({ summary, details: report }, null, 2)
  );

  return { summary, details: report };
}
```

## Best Practices

1. **Run axe on every PR** - Integrate into CI to block merges with accessibility violations.
2. **Test all component states** - Audit default, loading, error, empty, and disabled states.
3. **Include keyboard tests** - Verify Tab, Enter, Escape, and Arrow key navigation.
4. **Test with real screen readers** - Automated tools catch ~30% of issues; manual testing catches the rest.
5. **Maintain an a11y test matrix** - Track which components have been audited and at what WCAG level.
6. **Fix violations immediately** - Accessibility regressions are harder to fix the longer they persist.
7. **Document known exceptions** - If a violation can't be fixed, document the reason and workaround.
8. **Use semantic HTML first** - Automated tests are more reliable with proper semantic markup.
9. **Test color contrast** - Verify contrast ratios programmatically for all theme variants.
10. **Audit dark mode separately** - WCAG contrast requirements apply to both light and dark themes.

## Common Pitfalls

1. **Only testing default state** - Loading and error states often have worse accessibility than default.
2. **Ignoring `incomplete` results** - Axe's "needs review" items frequently contain real violations.
3. **False confidence** - Passing axe doesn't mean fully accessible; manual testing is still required.
4. **Not testing after updates** - Bootstrap updates can change accessibility behavior.
5. **Missing ARIA in dynamic content** - JavaScript-rendered content often lacks proper ARIA attributes.

## Accessibility Considerations

This entire topic is about accessibility. The audit pipeline should be the gatekeeper for all component releases.

## Responsive Behavior

Accessibility audits should test at multiple viewport sizes. Touch targets need larger hit areas on mobile, and content reflow must maintain logical reading order.

```js
const viewports = [
  { width: 375, height: 667, name: 'mobile' },
  { width: 768, height: 1024, name: 'tablet' },
  { width: 1280, height: 800, name: 'desktop' }
];

viewports.forEach(vp => {
  test(`components accessible at ${vp.name}`, async () => {
    await page.setViewportSize(vp);
    const results = await axe.run();
    expect(results.violations).toEqual([]);
  });
});
```
