---
title: "Visual Regression Testing Bootstrap"
slug: "visual-regression-testing"
difficulty: 3
duration: "60 minutes"
prerequisites:
  - "Bootstrap 5 Styling"
  - "CI/CD Pipeline Basics"
  - "Git Version Control"
topics:
  - "Testing"
  - "Visual Regression"
  - "Percy"
  - "BackstopJS"
  - "Screenshot Comparison"
tools:
  - "Percy"
  - "BackstopJS"
  - "jest-image-snapshot"
learning_objectives:
  - "Set up Percy for automated visual regression testing"
  - "Configure BackstopJS reference screenshots and comparison"
  - "Detect unintended CSS changes across Bootstrap components"
  - "Test responsive visual layouts at multiple breakpoints"
---

## Overview

Visual regression testing captures pixel-level screenshots of Bootstrap components and compares them against approved baselines to detect unintended visual changes. This catches CSS regressions that functional tests miss - spacing shifts, color changes, font rendering differences, and layout breaks.

Percy (cloud-based) and BackstopJS (local) are the primary tools. Percy integrates with CI pipelines and provides collaborative review workflows. BackstopJS runs entirely locally with configurable comparison engines. Both support responsive viewport testing across multiple breakpoints.

## Basic Implementation

### BackstopJS Setup

```bash
npm install --save-dev backstopjs
npx backstop init
```

```js
// backstop.config.js
module.exports = {
  id: "bootstrap_project",
  viewports: [
    { label: "phone", width: 375, height: 667 },
    { label: "tablet", width: 768, height: 1024 },
    { label: "desktop", width: 1280, height: 720 },
  ],
  scenarios: [
    {
      label: "Alert Component",
      url: "http://localhost:3000/components/alerts.html",
      selectors: [".alert"],
      misMatchThreshold: 0.1,
    },
    {
      label: "Card Grid",
      url: "http://localhost:3000/components/cards.html",
      selectors: [".card-group"],
      misMatchThreshold: 0.2,
    },
  ],
  paths: {
    bitmaps_reference: "backstop_data/bitmaps_reference",
    bitmaps_test: "backstop_data/bitmaps_test",
    engine_scripts: "backstop_data/engine_scripts",
    html_report: "backstop_data/html_report",
    ci_report: "backstop_data/ci_report",
  },
  engine: "puppeteer",
  engineOptions: {
    args: ["--no-sandbox"],
  },
  report: ["browser", "CI"],
  debug: false,
};
```

```bash
# Generate reference screenshots
npx backstop reference

# Run visual regression tests
npx backstop test

# Approve changes as new baseline
npx backstop approve
```

### Percy Integration with Cypress

```bash
npm install --save-dev @percy/cypress @percy/cli
```

```js
// cypress/e2e/visual.cy.js
import '@percy/cypress';

describe('Visual regression', () => {
  it('renders alert components correctly', () => {
    cy.visit('/components/alerts.html');
    cy.percySnapshot('Alert Components');
  });

  it('renders modal at different states', () => {
    cy.visit('/components/modal.html');
    cy.percySnapshot('Modal - Closed');
    cy.get('[data-bs-toggle="modal"]').click();
    cy.percySnapshot('Modal - Open');
  });
});
```

```bash
# Run Percy tests
npx percy exec -- cypress run
```

## Advanced Variations

### Testing Component Variants with BackstopJS

```js
// backstop.config.js - advanced scenarios
scenarios: [
  {
    label: "Button Variants",
    url: "http://localhost:3000/buttons.html",
    selectors: [".btn-group-test"],
    hoverSelector: ".btn-primary",
    postInteractionWait: 500,
    misMatchThreshold: 0.1,
  },
  {
    label: "Dark Mode Cards",
    url: "http://localhost:3000/cards-dark.html",
    selectors: [".card"],
    misMatchThreshold: 0.15,
    requireSameDimensions: false,
  },
  {
    label: "Tooltip on Hover",
    url: "http://localhost:3000/tooltips.html",
    selectors: [".tooltip-test-area"],
    hoverSelector: "[data-bs-toggle='tooltip']",
    postInteractionWait: 1000,
    misMatchThreshold: 0.3,
  },
],
```

### Custom Puppeteer Script for State Capture

```js
// backstop_data/engine_scripts/onReady.js
module.exports = async (page, scenario) => {
  const hoverSelector = scenario.hoverSelector;
  if (hoverSelector) {
    await page.hover(hoverSelector);
  }

  if (scenario.label.includes('Scroll')) {
    await page.evaluate(() => window.scrollBy(0, 300));
  }

  await page.waitForTimeout(scenario.postInteractionWait || 0);
};
```

## Best Practices

1. **Set appropriate misMatchThreshold** - 0.1% for precise components, 0.3% for cross-browser-rendered elements.
2. **Test at all Bootstrap breakpoints** - xs (576px), sm (768px), md (992px), lg (1200px), xl (1400px).
3. **Isolate selectors** - Test specific components, not entire pages, to reduce noise.
4. **Freeze animations** - Add `* { animation: none !important; transition: none !important; }` to prevent flaky screenshots.
5. **Use consistent viewport heights** - Ensure elements render above the fold for deterministic captures.
6. **Run in headless mode in CI** - Headed mode introduces rendering variations.
7. **Version control reference images** - Store baselines in the repo for reproducibility.
8. **Review diffs in PRs** - Percy provides per-commit visual diffs; require approval before merging.
9. **Test both light and dark themes** - If using Bootstrap's dark mode, capture both variants.
10. **Capture component states** - Test hover, focus, active, and disabled states separately.
11. **Use `readySelector`** - Wait for a specific element before capturing to avoid partial renders.
12. **Exclude dynamic content** - Mask timestamps, user avatars, or ad banners that change every run.

## Common Pitfalls

1. **Font rendering differences across OS** - macOS and Windows render fonts differently; use Docker for consistent baselines.
2. **Anti-aliasing variations** - Slight pixel differences in anti-aliasing cause false positives; increase threshold.
3. **Not waiting for web fonts** - Screenshots before fonts load show fallback fonts; wait for `document.fonts.ready`.
4. **Ignoring scrollbar widths** - Scrollbars affect layout; use `overflow: hidden` on captured containers.
5. **Testing entire pages** - Large screenshots have more noise; target specific component selectors.
6. **Hard-coded pixel dimensions** - Bootstrap's responsive behavior means components change size; use `requireSameDimensions: false`.
7. **Not freezing CSS animations** - Transitioning elements produce different screenshots on each run.
8. **Forgetting backdrop elements** - Modal backdrops render as overlays; test with and without backdrop.
9. **Baseline drift** - Incremental approved changes compound; periodically reset and re-baseline.
10. **Browser version mismatches** - Puppeteer/Playwright updates change rendering; pin browser versions.

## Accessibility Considerations

Visual regression tests can catch accessibility regressions:

- Test focus ring visibility on interactive elements (buttons, links, form controls).
- Verify `:focus-visible` styles render correctly across browsers.
- Capture high-contrast mode rendering for Windows users.
- Ensure text scaling (150%, 200%) doesn't break layouts.

```js
// backstop.config.js - accessibility scenarios
{
  label: "Focus States",
  url: "http://localhost:3000/buttons.html",
  selectors: [".btn-focus-test"],
  clickSelector: ".btn-primary",
  postInteractionWait: 500,
  misMatchThreshold: 0.1,
},
```

```js
// Test text scaling
{
  label: "Large Text Mode",
  url: "http://localhost:3000/typography.html",
  selectors: ["body"],
  onReadyScript: "setLargeText.js",
  misMatchThreshold: 0.5,
}
```

```js
// backstop_data/engine_scripts/setLargeText.js
module.exports = async (page) => {
  await page.evaluate(() => {
    document.documentElement.style.fontSize = '150%';
  });
  await page.waitForTimeout(500);
};
```

## Responsive Behavior

Comprehensive responsive visual testing requires capturing all breakpoints:

```js
// backstop.config.js
viewports: [
  { label: "xs", width: 375, height: 667 },
  { label: "sm", width: 576, height: 768 },
  { label: "md", width: 768, height: 1024 },
  { label: "lg", width: 992, height: 768 },
  { label: "xl", width: 1200, height: 800 },
  { label: "xxl", width: 1400, height: 900 },
],
scenarios: [
  {
    label: "Responsive Grid",
    url: "http://localhost:3000/grid.html",
    selectors: [".container"],
    viewports: [
      { label: "xs", width: 375, height: 667 },
      { label: "md", width: 768, height: 1024 },
      { label: "xxl", width: 1400, height: 900 },
    ],
  },
],
```

```js
// Percy responsive snapshots
cy.percySnapshot('Navbar - Mobile', { widths: [375] });
cy.percySnapshot('Navbar - Tablet', { widths: [768] });
cy.percySnapshot('Navbar - Desktop', { widths: [1280] });
cy.percySnapshot('Navbar - All Sizes', { widths: [375, 768, 1280] });
```
