---
title: "Accessibility Testing Bootstrap Components"
slug: "accessibility-testing"
difficulty: 2
duration: "45 minutes"
prerequisites:
  - "Bootstrap 5 Accessibility Features"
  - "WCAG 2.1 Basics"
  - "npm/Node.js"
topics:
  - "Testing"
  - "Accessibility"
  - "axe-core"
  - "pa11y"
  - "Lighthouse"
  - "WCAG"
tools:
  - "axe-core"
  - "pa11y"
  - "Lighthouse"
  - "@axe-core/playwright"
learning_objectives:
  - "Integrate axe-core into automated test suites for Bootstrap components"
  - "Run pa11y accessibility audits against Bootstrap pages"
  - "Configure Lighthouse CI for continuous accessibility monitoring"
  - "Interpret and fix common WCAG violations in Bootstrap markup"
---

## Overview

Accessibility testing ensures Bootstrap components are usable by people with disabilities - those using screen readers, keyboard navigation, or assistive technologies. Automated tools like axe-core, pa11y, and Lighthouse detect WCAG 2.1 violations programmatically, catching issues like missing alt text, insufficient color contrast, absent ARIA labels, and broken keyboard navigation.

Bootstrap 5 includes strong accessibility defaults, but customizations, dynamic content, and incorrect markup can introduce violations. Integrating accessibility testing into your CI pipeline catches these regressions before they reach production.

## Basic Implementation

### axe-core with Jest and jsdom

```bash
npm install --save-dev jest @axe-core/jaxe-core
```

```js
// __tests__/a11y-alert.test.js
const { JSDOM } = require('jsdom');
const { axe, toHaveNoViolations } = require('jest-axe');

expect.extend(toHaveNoViolations);

test('Bootstrap alert has no accessibility violations', async () => {
  const dom = new JSDOM(`
    <!DOCTYPE html>
    <html lang="en">
      <body>
        <div class="alert alert-success" role="alert">
          A simple success alert with a
          <a href="#" class="alert-link">link</a>.
        </div>
      </body>
    </html>
  `);

  const results = await axe(dom.window.document.body);
  expect(results).toHaveNoViolations();
});

test('Bootstrap form has no accessibility violations', async () => {
  const dom = new JSDOM(`
    <!DOCTYPE html>
    <html lang="en">
      <body>
        <form>
          <div class="mb-3">
            <label for="email" class="form-label">Email address</label>
            <input type="email" class="form-control" id="email" aria-describedby="emailHelp">
            <div id="emailHelp" class="form-text">We'll never share your email.</div>
          </div>
          <button type="submit" class="btn btn-primary">Submit</button>
        </form>
      </body>
    </html>
  `);

  const results = await axe(dom.window.document.body);
  expect(results).toHaveNoViolations();
});
```

### pa11y CLI Testing

```bash
npm install --save-dev pa11y pa11y-ci
```

```bash
# Test a single page
npx pa11y http://localhost:3000/components/modal.html

# Run with WCAG2AA standard
npx pa11y --standard WCAG2AA http://localhost:3000/index.html

# Generate JSON report
npx pa11y --reporter json http://localhost:3000/index.html > a11y-report.json
```

```json
// .pa11yci.json
{
  "defaults": {
    "standard": "WCAG2AA",
    "runners": ["axe", "htmlcs"],
    "chromeLaunchConfig": {
      "args": ["--no-sandbox"]
    }
  },
  "urls": [
    "http://localhost:3000/index.html",
    "http://localhost:3000/components/modal.html",
    "http://localhost:3000/components/dropdown.html",
    "http://localhost:3000/components/navbar.html"
  ]
}
```

```bash
npx pa11y-ci
```

## Advanced Variations

### axe-core with Playwright

```bash
npm install --save-dev @axe-core/playwright
```

```js
// tests/a11y-modal.spec.js
const { test, expect } = require('@playwright/test');
const AxeBuilder = require('@axe-core/playwright').default;

test.describe('Modal accessibility', () => {
  test('open modal has no violations', async ({ page }) => {
    await page.goto('/components/modal.html');
    await page.click('[data-bs-toggle="modal"]');
    await page.waitForSelector('.modal.show');

    const results = await new AxeBuilder({ page })
      .include('.modal')
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a'])
      .analyze();

    expect(results.violations).toEqual([]);
  });

  test('full page accessibility audit', async ({ page }) => {
    await page.goto('/index.html');
    const results = await new AxeBuilder({ page }).analyze();
    expect(results.violations).toEqual([]);
  });
});
```

### Lighthouse CI Configuration

```bash
npm install --save-dev @lhci/cli
```

```json
// lighthouserc.json
{
  "ci": {
    "collect": {
      "url": [
        "http://localhost:3000/index.html",
        "http://localhost:3000/components/modal.html",
        "http://localhost:3000/components/navbar.html"
      ],
      "numberOfRuns": 3,
      "settings": {
        "onlyCategories": ["accessibility"]
      }
    },
    "assert": {
      "assertions": {
        "categories:accessibility": ["error", { "minScore": 0.95 }]
      }
    },
    "upload": {
      "target": "temporary-public-storage"
    }
  }
}
```

```bash
# Run Lighthouse CI
npx lhci autorun
```

## Best Practices

1. **Test at WCAG2AA minimum** - This is the standard most regulations require; test against `wcag2a` and `wcag2aa` tags.
2. **Include axe-core in unit tests** - Run automated checks on every component render in your test suite.
3. **Use pa11y-ci in pre-commit hooks** - Catch accessibility violations before code is committed.
4. **Set Lighthouse CI thresholds** - Enforce a minimum accessibility score (e.g., 95) in CI pipelines.
5. **Test dynamic content** - Re-run axe after modals open, dropdowns expand, or toasts appear.
6. **Audit all user flows** - Test login forms, multi-step wizards, and error states, not just static pages.
7. **Combine automated and manual testing** - Automated tools catch ~30% of issues; manual keyboard testing catches the rest.
8. **Test with screen readers** - Use NVDA (Windows), VoiceOver (macOS), or TalkBack (Android) for manual verification.
9. **Check color contrast ratios** - Bootstrap's default colors meet WCAG AA, but custom themes may not.
10. **Test RTL layouts** - Verify Bootstrap's RTL support maintains accessibility in right-to-left languages.
11. **Version-pin axe-core** - New rules can introduce violations in previously passing code.
12. **Review axe results in PRs** - Add accessibility reports to CI output for developer visibility.

## Common Pitfalls

1. **Ignoring low-severity axe violations** - "Needs review" items often indicate real issues; don't dismiss them.
2. **Testing only static pages** - Modal, dropdown, and toast accessibility requires interaction-based testing.
3. **Using `role` without required ARIA attributes** - `role="tablist"` requires `aria-selected` on tabs; axe catches this.
4. **Missing `lang` attribute on `<html>`** - Screen readers can't determine page language without it.
5. **Custom components without keyboard support** - Bootstrap's JS handles keyboard events; custom wrappers may lose this.
6. **Relying solely on automated tools** - axe-core cannot test logical tab order or screen reader announcements.
7. **Forgetting focus management on route changes** - Single-page apps must move focus on navigation.
8. **Not testing error states** - Form validation messages must be announced to screen readers via `aria-live`.
9. **Overriding Bootstrap's focus styles** - Custom focus outlines that are too subtle violate WCAG 2.4.7.
10. **Color-only information** - Using only color to convey status (red = error) fails WCAG 1.4.1; add icons or text.

## Accessibility Considerations

This section covers the accessibility patterns that testing should verify:

- **ARIA landmarks** - Ensure `<nav>`, `<main>`, `<aside>` regions are present and labeled.
- **Skip navigation links** - Verify skip-to-content links work and are visible on focus.
- **Focus trap in modals** - Test that Tab cycles within open modals without escaping to background.
- **Live regions** - Verify `aria-live="polite"` announcements for dynamic content updates.
- **Form labels** - Every input must have an associated `<label>` or `aria-label`.
- **Image alt text** - All `<img>` elements need descriptive `alt` attributes or `alt=""` for decorative images.

```js
// Verify focus trap in modal
test('focus stays within open modal', async ({ page }) => {
  await page.goto('/components/modal.html');
  await page.click('[data-bs-toggle="modal"]');
  await page.waitForSelector('.modal.show');

  const focusableInModal = await page.$$eval(
    '.modal.show button, .modal.show input, .modal.show a',
    els => els.length
  );

  expect(focusableInModal).toBeGreaterThan(0);

  // Tab through all focusable elements
  for (let i = 0; i <= focusableInModal; i++) {
    await page.keyboard.press('Tab');
  }

  const focusedSelector = await page.evaluate(() =>
    document.activeElement.closest('.modal') ? 'inside' : 'outside'
  );
  expect(focusedSelector).toBe('inside');
});
```

## Responsive Behavior

Accessibility testing must cover responsive layouts:

```js
// Test mobile navbar accessibility
test('mobile navbar toggle is accessible', async ({ page }) => {
  await page.setViewportSize({ width: 375, height: 667 });
  await page.goto('/components/navbar.html');

  const toggler = page.locator('.navbar-toggler');
  await expect(toggler).toHaveAttribute('aria-controls');
  await expect(toggler).toHaveAttribute('aria-expanded', 'false');

  await toggler.click();
  await expect(toggler).toHaveAttribute('aria-expanded', 'true');

  const results = await new AxeBuilder({ page }).analyze();
  expect(results.violations).toEqual([]);
});
```

```bash
# pa11y with custom viewport
npx pa11y --viewport-width 375 --viewport-height 667 http://localhost:3000/navbar.html
```

Test that responsive breakpoints don't hide focusable elements without updating `aria-hidden` or `tabindex` accordingly.
