---
title: "Integration Testing Bootstrap Components"
slug: "integration-testing"
difficulty: 3
duration: "60 minutes"
prerequisites:
  - "Bootstrap 5 Components"
  - "Cypress or Playwright"
  - "DOM Testing Library"
topics:
  - "Testing"
  - "Integration Tests"
  - "Cypress"
  - "Playwright"
  - "User Interactions"
tools:
  - "Cypress"
  - "Playwright"
  - "@testing-library/dom"
learning_objectives:
  - "Test Bootstrap modal open/close workflows end-to-end"
  - "Validate dropdown, tooltip, and collapse interactions"
  - "Configure Cypress and Playwright for Bootstrap projects"
  - "Write integration tests that simulate real user behavior"
---

## Overview

Integration testing validates that Bootstrap components work correctly with real user interactions - clicks, keyboard navigation, form submissions, and DOM state changes. Unlike unit tests, integration tests exercise multiple components together and verify that Bootstrap's JavaScript plugins (modals, dropdowns, tooltips) behave as expected in a live or simulated browser environment.

Cypress and Playwright are the two dominant tools for this purpose. Both provide real browser execution, automatic waiting, and rich APIs for interacting with Bootstrap's interactive components.

## Basic Implementation

### Cypress Setup for Bootstrap

```bash
npm install --save-dev cypress
npx cypress open
```

```js
// cypress/e2e/modal.cy.js
describe('Bootstrap Modal', () => {
  beforeEach(() => {
    cy.visit('/components/modal.html');
  });

  it('opens modal when trigger button is clicked', () => {
    cy.get('[data-bs-toggle="modal"]').click();
    cy.get('.modal').should('have.class', 'show');
    cy.get('.modal').should('be.visible');
  });

  it('closes modal when close button is clicked', () => {
    cy.get('[data-bs-toggle="modal"]').click();
    cy.get('.modal').should('have.class', 'show');
    cy.get('.btn-close').click();
    cy.get('.modal').should('not.have.class', 'show');
  });

  it('closes modal on Escape key', () => {
    cy.get('[data-bs-toggle="modal"]').click();
    cy.get('.modal').should('have.class', 'show');
    cy.get('.modal').type('{esc}');
    cy.get('.modal').should('not.have.class', 'show');
  });
});
```

### Playwright Setup for Bootstrap

```bash
npm install --save-dev @playwright/test
npx playwright test
```

```js
// tests/dropdown.spec.js
const { test, expect } = require('@playwright/test');

test.describe('Bootstrap Dropdown', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/components/dropdown.html');
  });

  test('opens dropdown on click', async ({ page }) => {
    await page.click('[data-bs-toggle="dropdown"]');
    await expect(page.locator('.dropdown-menu')).toHaveClass(/show/);
  });

  test('closes dropdown when clicking outside', async ({ page }) => {
    await page.click('[data-bs-toggle="dropdown"]');
    await expect(page.locator('.dropdown-menu')).toHaveClass(/show/);
    await page.click('body', { position: { x: 10, y: 10 } });
    await expect(page.locator('.dropdown-menu')).not.toHaveClass(/show/);
  });

  test('selects dropdown item', async ({ page }) => {
    await page.click('[data-bs-toggle="dropdown"]');
    await page.click('.dropdown-item:nth-child(2)');
    const selected = await page.textContent('[data-bs-toggle="dropdown"]');
    expect(selected).toBeTruthy();
  });
});
```

## Advanced Variations

### Testing Bootstrap Collapse with State Transitions

```js
// cypress/e2e/collapse.cy.js
describe('Bootstrap Collapse', () => {
  beforeEach(() => {
    cy.visit('/components/collapse.html');
  });

  it('toggles content visibility on button click', () => {
    cy.get('#collapseExample').should('not.have.class', 'show');
    cy.get('[data-bs-toggle="collapse"]').click();
    cy.get('#collapseExample')
      .should('have.class', 'collapsing')
      .then($el => {
        cy.wrap($el).should('have.class', 'show');
      });
  });

  it('accordion expands one panel at a time', () => {
    cy.get('.accordion-button').first().click();
    cy.get('.accordion-collapse').first().should('have.class', 'show');
    cy.get('.accordion-button').last().click();
    cy.get('.accordion-collapse').last().should('have.class', 'show');
    cy.get('.accordion-collapse').first().should('not.have.class', 'show');
  });
});
```

### Testing Bootstrap Toast Notifications

```js
// tests/toast.spec.js
const { test, expect } = require('@playwright/test');

test('toast appears and auto-hides', async ({ page }) => {
  await page.goto('/components/toast.html');
  await page.click('#showToastBtn');
  const toast = page.locator('.toast');
  await expect(toast).toHaveClass(/show/);
  await expect(toast).not.toBeVisible({ timeout: 6000 });
});

test('toast can be manually dismissed', async ({ page }) => {
  await page.goto('/components/toast.html');
  await page.click('#showToastBtn');
  await page.click('.toast .btn-close');
  await expect(page.locator('.toast')).not.toHaveClass(/show/);
});
```

## Best Practices

1. **Wait for Bootstrap transitions** - Use Cypress `should('have.class', 'show')` instead of `cy.wait()` for animation-dependent assertions.
2. **Test with real data-bs attributes** - Always use `data-bs-toggle`, `data-bs-target` selectors to test actual Bootstrap markup.
3. **Use `data-testid` for stable selectors** - Add test IDs to avoid brittle CSS selectors that break with markup changes.
4. **Test keyboard navigation** - Verify Tab, Escape, Enter, and Arrow keys work for accessible components.
5. **Reset state between tests** - Use `beforeEach` to ensure a clean page state.
6. **Test both happy and error paths** - Verify modals open (success) and handle invalid triggers (failure).
7. **Run tests in headed mode during development** - Use `--headed` or `--headed --slow-mo 500` to debug visually.
8. **Parallelize test suites** - Split component tests into separate files for parallel execution.
9. **Mock external dependencies** - Intercept API calls that Bootstrap components might trigger.
10. **Test focus management** - Verify focus moves to modal on open and returns to trigger on close.
11. **Assert ARIA attributes** - Check `aria-expanded`, `aria-hidden`, `aria-modal` after interactions.
12. **Use custom commands** - Create reusable Cypress commands like `cy.openModal()` for repeated workflows.

## Common Pitfalls

1. **Not waiting for animations** - Clicking close before modal animation completes causes flaky tests; use proper assertions.
2. **Using `cy.wait(1000)` for timing** - Arbitrary waits are brittle; use Cypress retry-ability with `should()`.
3. **Ignoring Bootstrap's backdrop** - Clicking through a modal backdrop tests the wrong interaction; target the backdrop element.
4. **Hard-coding selectors to Bootstrap internals** - `.modal-dialog` may change; prefer `data-testid` or semantic selectors.
5. **Not testing escape key behavior** - Modal accessibility requires Escape key dismissal; many tests skip this.
6. **Assuming dropdown closes on blur** - Bootstrap dropdowns require click-outside; test the actual interaction.
7. **Running tests without Bootstrap JS loaded** - Integration tests fail silently if Bootstrap's JS bundle is missing.
8. **Forgetting to test tooltip cleanup** - Orphaned tooltips cause memory leaks; verify they're removed from DOM on hide.
9. **Not testing mobile touch events** - Hover-dependent tooltips fail on touch devices; test `focus` and `click` triggers.
10. **Ignoring z-index conflicts** - Custom CSS may hide modals behind other elements; assert visibility, not just class presence.

## Accessibility Considerations

Integration tests must verify Bootstrap's accessibility features work in practice:

```js
// cypress/e2e/accessibility.cy.js
describe('Modal accessibility', () => {
  it('focuses modal on open', () => {
    cy.get('[data-bs-toggle="modal"]').click();
    cy.focused().should('have.class', 'modal-content').or
      .should('have.attr', 'role', 'dialog');
  });

  it('sets aria-modal on open', () => {
    cy.get('[data-bs-toggle="modal"]').click();
    cy.get('.modal').should('have.attr', 'aria-modal', 'true');
  });

  it('traps focus inside modal', () => {
    cy.get('[data-bs-toggle="modal"]').click();
    cy.get('.modal').tab();
    cy.focused().should('be.within', cy.get('.modal'));
  });
});
```

```js
// Playwright accessibility test
test('dropdown has correct ARIA attributes', async ({ page }) => {
  await page.click('[data-bs-toggle="dropdown"]');
  const trigger = page.locator('[data-bs-toggle="dropdown"]');
  await expect(trigger).toHaveAttribute('aria-expanded', 'true');
  await expect(trigger).toHaveAttribute('aria-haspopup', 'true');
});
```

## Responsive Behavior

Test Bootstrap components across viewport sizes:

```js
// cypress/e2e/responsive-integration.cy.js
describe('Responsive navbar', () => {
  it('shows hamburger on mobile', () => {
    cy.viewport(375, 667);
    cy.visit('/components/navbar.html');
    cy.get('.navbar-toggler').should('be.visible');
    cy.get('.navbar-collapse').should('not.be.visible');
  });

  it('shows full nav on desktop', () => {
    cy.viewport(1280, 720);
    cy.visit('/components/navbar.html');
    cy.get('.navbar-toggler').should('not.be.visible');
    cy.get('.navbar-collapse').should('be.visible');
  });

  it('toggles nav menu on mobile click', () => {
    cy.viewport(375, 667);
    cy.visit('/components/navbar.html');
    cy.get('.navbar-toggler').click();
    cy.get('.navbar-collapse').should('have.class', 'show');
  });
});
```

```js
// Playwright viewport testing
test.use({ viewport: { width: 375, height: 667 } });
test('offcanvas opens on mobile', async ({ page }) => {
  await page.goto('/components/offcanvas.html');
  await page.click('[data-bs-toggle="offcanvas"]');
  await expect(page.locator('.offcanvas')).toHaveClass(/show/);
});
```
