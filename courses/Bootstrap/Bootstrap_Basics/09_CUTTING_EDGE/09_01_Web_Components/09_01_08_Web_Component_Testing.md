---
title: "Web Component Testing"
description: "Testing custom elements with shadow DOM assertions, browser testing strategies, and Bootstrap component validation"
difficulty: 3
tags: [testing, web-components, shadow-dom, vitest, playwright, bootstrap]
prerequisites:
  - 09_01_05_Web_Component_Design_System
---

## Overview

Testing web components requires navigating shadow DOM boundaries, custom element lifecycle, and asynchronous upgrades. Standard DOM testing libraries (JSDOM) lack full shadow DOM support, so you need either a real browser environment (Playwright, Cypress) or a JSDOM alternative with shadow DOM patches (Happy DOM, `@open-wc/testing`).

Three testing layers are essential: unit tests for individual component logic, integration tests for component composition, and end-to-end tests for real browser rendering. Bootstrap adds complexity because its CSS and JavaScript must load inside shadow roots for accurate visual testing.

## Basic Implementation

```js
// bs-button.test.js — using Vitest + Happy DOM
import { describe, it, expect, beforeEach } from 'vitest';
import './bs-button.js';

describe('BsButton', () => {
  let el;

  beforeEach(async () => {
    el = document.createElement('bs-button');
    el.setAttribute('variant', 'primary');
    el.textContent = 'Click Me';
    document.body.appendChild(el);
    await el.updateComplete; // wait for shadow DOM render
  });

  afterEach(() => {
    el.remove();
  });

  it('should register as custom element', () => {
    expect(customElements.get('bs-button')).toBeDefined();
  });

  it('should render shadow DOM', () => {
    expect(el.shadowRoot).toBeTruthy();
    expect(el.shadowRoot.querySelector('button')).toBeTruthy();
  });

  it('should apply variant class', () => {
    const btn = el.shadowRoot.querySelector('button');
    expect(btn.classList.contains('btn-primary')).toBe(true);
  });

  it('should reflect disabled attribute', () => {
    el.setAttribute('disabled', '');
    const btn = el.shadowRoot.querySelector('button');
    expect(btn.disabled).toBe(true);
  });

  it('should project slot content', () => {
    const slot = el.shadowRoot.querySelector('slot');
    const assigned = slot.assignedNodes();
    expect(assigned[0].textContent).toBe('Click Me');
  });
});
```

```js
// Playwright E2E test
import { test, expect } from '@playwright/test';

test('bs-button renders in browser with Bootstrap styles', async ({ page }) => {
  await page.setContent(`
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
    <script type="module" src="./bs-button.js"></script>
    <bs-button variant="danger">Delete</bs-button>
  `);

  const button = page.locator('bs-button');
  await expect(button).toBeVisible();

  const shadowBtn = button.locator('button');
  await expect(shadowBtn).toHaveClass(/btn-danger/);
  await expect(shadowBtn).toHaveCSS('background-color', 'rgb(220, 53, 69)');
});

test('custom event fires on click', async ({ page }) => {
  await page.setContent(`
    <script type="module" src="./bs-button.js"></script>
    <bs-button>Test</bs-button>
    <script>
      document.querySelector('bs-button').addEventListener('bs:click', (e) => {
        window.__clicked = e.detail;
      });
    </script>
  `);

  await page.locator('bs-button').click();
  const detail = await page.evaluate(() => window.__clicked);
  expect(detail).toBeTruthy();
});
```

```css
/* Visual regression test setup with Playwright screenshot comparison */
/* No specific CSS needed — tests compare screenshot diffs */
```

## Advanced Variations

Test shadow DOM accessibility with `axe-core`:

```js
import axe from 'axe-core';

it('should have no accessibility violations', async () => {
  const results = await axe.run(el.shadowRoot, {
    rules: { 'color-contrast': { enabled: true } }
  });
  expect(results.violations).toHaveLength(0);
});
```

## Best Practices

1. Use Happy DOM or Playwright instead of JSDOM for shadow DOM support.
2. Test shadow root existence before querying shadow DOM elements.
3. Use `el.shadowRoot.querySelector()` to reach inside shadow boundaries.
4. Wait for component readiness with `el.updateComplete` or a custom ready promise.
5. Use Playwright for visual/CSS testing; unit test frameworks can't verify rendered styles.
6. Test attribute reflection: set attribute, check property; set property, check attribute.
7. Test custom events with `addEventListener` spy in the test.
8. Test slot projection by checking `slot.assignedNodes()`.
9. Use `axe-core` for automated accessibility testing inside shadow DOM.
10. Clean up with `el.remove()` in `afterEach` to prevent test pollution.
11. Test with `mode: 'closed'` shadow roots separately — they block `shadowRoot` access.
12. Mock external resources (Bootstrap CDN) in unit tests; use real URLs in E2E.

## Common Pitfalls

1. **JSDOM no shadow DOM** — JSDOM returns `null` for `el.shadowRoot`; use Happy DOM or browser.
2. **Async timing** — Shadow DOM rendering may be async; test assertions fire before render completes.
3. **Closed shadow roots** — Tests can't access `shadowRoot` when `mode: 'closed'`; use test hooks.
4. **CSS not loaded** — Unit test environments don't fetch external CSS; class presence tests pass but visual tests fail.
5. **Event retargeting** — `event.target` in test listeners shows the host, not the inner element.
6. **Multiple test registrations** — `customElements.define()` throws if called twice; guard with `if (!customElements.get(...))`.

## Accessibility Considerations

Test with `axe-core` for WCAG violations. Verify `role`, `aria-*`, and keyboard navigation programmatically. Use Playwright to test actual screen reader output with `page.accessibility.snapshot()`.

## Responsive Behavior

Use Playwright's `page.setViewportSize()` to test responsive behavior at different breakpoints. Screenshot comparisons catch visual regressions in responsive layouts.
