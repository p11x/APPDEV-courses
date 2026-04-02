---
title: "Library Testing"
difficulty: 3
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - Jest / Vitest
  - Playwright / Testing Library
  - Visual Regression Testing
---

## Overview

Testing a Bootstrap component library requires a multi-layered strategy: unit tests for individual component logic, snapshot tests for HTML output stability, integration tests for component interactions, and visual regression tests for CSS rendering fidelity. Each layer catches different classes of bugs and operates at different speeds and confidence levels.

Unit tests verify component configuration, event emission, state management, and disposal. Snapshot tests capture rendered HTML to detect unintended markup changes. Integration tests validate that components work together (e.g., a modal containing a form). Visual regression tests compare pixel-level screenshots against baselines across browsers and viewports.

## Basic Implementation

```js
// tests/components/DataTable.test.js
import { DataTable } from '../../src/components/DataTable';

describe('DataTable', () => {
  let container;

  beforeEach(() => {
    container = document.createElement('div');
    container.dataset.component = 'data-table';
    document.body.appendChild(container);
  });

  afterEach(() => {
    container.remove();
  });

  test('creates instance from element', () => {
    const table = new DataTable(container);
    expect(table).toBeInstanceOf(DataTable);
    expect(DataTable.getInstance(container)).toBe(table);
  });

  test('renders table with columns and data', () => {
    const table = new DataTable(container, {
      columns: [
        { key: 'name', label: 'Name' },
        { key: 'email', label: 'Email' }
      ],
      data: [
        { name: 'John', email: 'john@test.com' },
        { name: 'Jane', email: 'jane@test.com' }
      ]
    });

    expect(container.querySelector('table')).toBeTruthy();
    expect(container.querySelectorAll('th')).toHaveLength(2);
    expect(container.querySelectorAll('tbody tr')).toHaveLength(2);
  });

  test('emits change event on sort', () => {
    const handler = jest.fn();
    const table = new DataTable(container, {
      columns: [{ key: 'name', label: 'Name' }],
      data: [{ name: 'John' }]
    });

    container.addEventListener('bs.datatable:sort', handler);
    container.querySelector('th').click();

    expect(handler).toHaveBeenCalled();
    expect(handler.mock.calls[0][0].detail).toHaveProperty('column', 'name');
  });

  test('disposes correctly', () => {
    const table = new DataTable(container);
    table.dispose();
    expect(DataTable.getInstance(container)).toBeNull();
  });
});
```

```js
// tests/snapshots/Card.snap.test.js
import { Card } from '../../src/components/Card';

describe('Card snapshots', () => {
  test('default card matches snapshot', () => {
    const container = document.createElement('div');
    new Card(container, {
      title: 'Test Card',
      body: 'Card content'
    });
    expect(container.innerHTML).toMatchSnapshot();
  });

  test('card with image matches snapshot', () => {
    const container = document.createElement('div');
    new Card(container, {
      title: 'Image Card',
      body: 'Content',
      image: { src: 'test.jpg', alt: 'Test' }
    });
    expect(container.innerHTML).toMatchSnapshot();
  });
});
```

## Advanced Variations

```js
// Visual regression tests with Playwright
// tests/visual/card.spec.js
const { test, expect } = require('@playwright/test');

test.describe('Card Component Visual Tests', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/storybook/iframe.html?id=components-card--default');
    await page.waitForSelector('.card');
  });

  test('default card renders correctly', async ({ page }) => {
    await expect(page.locator('.card')).toHaveScreenshot('card-default.png');
  });

  test('card renders at mobile viewport', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    await expect(page.locator('.card')).toHaveScreenshot('card-mobile.png');
  });

  test('card renders in dark mode', async ({ page }) => {
    await page.evaluate(() => {
      document.documentElement.setAttribute('data-bs-theme', 'dark');
    });
    await expect(page.locator('.card')).toHaveScreenshot('card-dark.png');
  });
});
```

```js
// Integration test: Modal with Form
test('modal contains accessible form', async () => {
  const container = document.createElement('div');
  document.body.appendChild(container);

  const modal = new Modal(container, {
    title: 'Contact Form',
    body: '<form><input type="text" aria-label="Name" required></form>'
  });

  modal.show();

  // Verify modal is visible
  expect(container.querySelector('.modal')).toHaveClass('show');

  // Verify form is accessible
  const input = container.querySelector('input');
  expect(input).toHaveAttribute('aria-label', 'Name');
  expect(input).toHaveAttribute('required');

  // Verify focus is trapped
  expect(document.activeElement).toBe(input);

  modal.dispose();
  container.remove();
});
```

## Best Practices

1. **Test component lifecycle** - Create, update, dispose in every test suite.
2. **Use Testing Library for DOM queries** - Prefer `getByRole`, `getByText` over `querySelector`.
3. **Snapshot sparingly** - Snapshot only stable HTML structures; skip dynamic content.
4. **Test accessibility programmatically** - Use `axe-core` in integration tests to catch a11y violations.
5. **Test across viewports** - Visual regression tests must cover mobile, tablet, and desktop.
6. **Test disposal** - Verify WeakMap cleanup, event listener removal, and DOM cleanup.
7. **Mock Bootstrap plugins** - When testing components that use Bootstrap's JS, mock the Bootstrap API.
8. **Test error states** - Verify components handle missing elements, invalid configs, and network errors.
9. **Run tests in CI** - Every PR should trigger the full test suite including visual regression.
10. **Update baselines intentionally** - Visual regression baselines should only update with intentional design changes.

## Common Pitfalls

1. **Flaky visual tests** - Font rendering differences between CI and local environments cause false failures.
2. **Stale snapshots** - Snapshots committed without review let unintended changes slip through.
3. **Missing disposal tests** - Memory leaks from uncleaned components go undetected.
4. **Not testing keyboard interaction** - Components only tested with mouse clicks miss accessibility bugs.
5. **Over-mocking** - Mocking too much of Bootstrap's JS hides real integration issues.

## Accessibility Considerations

Every component test suite should include accessibility assertions verifying ARIA attributes, keyboard navigation, and focus management.

```js
test('modal traps focus and restores on close', async () => {
  const trigger = document.createElement('button');
  trigger.textContent = 'Open';
  document.body.appendChild(trigger);

  const modal = new Modal(document.createElement('div'), {
    title: 'Test',
    body: '<button>Inside</button>'
  });

  trigger.focus();
  modal.show();

  const innerBtn = modal._element.querySelector('button');
  expect(document.activeElement).toBe(innerBtn);

  modal.hide();
  expect(document.activeElement).toBe(trigger);

  trigger.remove();
  modal.dispose();
});
```

## Responsive Behavior

Visual regression tests should cover Bootstrap's major breakpoints (576, 768, 992, 1200, 1400px) to catch responsive layout issues.

```js
const breakpoints = [
  { name: 'mobile', width: 375 },
  { name: 'tablet', width: 768 },
  { name: 'desktop', width: 1280 }
];

breakpoints.forEach(({ name, width }) => {
  test(`card grid renders correctly at ${name}`, async ({ page }) => {
    await page.setViewportSize({ width, height: 800 });
    await page.goto('/storybook/iframe.html?id=components-cardgrid--default');
    await expect(page.locator('.row')).toHaveScreenshot(`cardgrid-${name}.png`);
  });
});
```
