---
title: "Responsive Testing Bootstrap Layouts"
slug: "responsive-testing"
difficulty: 2
duration: "45 minutes"
prerequisites:
  - "Bootstrap 5 Grid System"
  - "CSS Breakpoints"
  - "Browser DevTools"
topics:
  - "Testing"
  - "Responsive Design"
  - "Viewport Testing"
  - "Device Simulation"
  - "Breakpoints"
tools:
  - "Chrome DevTools"
  - "Playwright"
  - "Cypress"
  - "Responsively App"
learning_objectives:
  - "Test Bootstrap layouts across all five breakpoint tiers"
  - "Simulate devices with Playwright and Cypress viewport APIs"
  - "Build a responsive testing checklist for Bootstrap projects"
  - "Detect layout breaks, overflow, and hidden content at each viewport"
---

## Overview

Responsive testing validates that Bootstrap's grid system, utility classes, and components render correctly across viewport sizes. Bootstrap defines five breakpoints: sm (576px), md (768px), lg (992px), xl (1200px), and xxl (1400px). Each breakpoint can trigger different column widths, visibility states, and layout behaviors.

Testing must verify that content remains accessible, readable, and functional at every viewport - from 320px mobile screens to 4K displays. Automated viewport testing with Playwright and Cypress catches responsive regressions that manual testing misses.

## Basic Implementation

### Cypress Viewport Testing

```js
// cypress/e2e/responsive-grid.cy.js
describe('Responsive grid behavior', () => {
  beforeEach(() => {
    cy.visit('/layout/grid.html');
  });

  it('stacks columns on mobile (xs)', () => {
    cy.viewport(375, 667);
    cy.get('.col-md-6').should($cols => {
      $cols.each((_, col) => {
        const rect = col.getBoundingClientRect();
        expect(rect.width).to.be.closeTo(343, 10);
      });
    });
  });

  it('splits columns on tablet (md)', () => {
    cy.viewport(768, 1024);
    cy.get('.col-md-6').should($cols => {
      const widths = [...$cols].map(c => c.getBoundingClientRect().width);
      expect(widths[0]).to.be.closeTo(widths[1], 5);
    });
  });

  it('renders three columns on desktop (lg)', () => {
    cy.viewport(992, 768);
    cy.get('.col-lg-4').should('have.length', 3);
    cy.get('.col-lg-4').first()
      .invoke('width')
      .should('be.greaterThan', 300);
  });
});
```

### Playwright Viewport Testing

```js
// tests/responsive.spec.js
const { test, expect } = require('@playwright/test');

const viewports = [
  { name: 'xs', width: 375, height: 667 },
  { name: 'sm', width: 576, height: 768 },
  { name: 'md', width: 768, height: 1024 },
  { name: 'lg', width: 992, height: 768 },
  { name: 'xl', width: 1200, height: 800 },
  { name: 'xxl', width: 1400, height: 900 },
];

for (const vp of viewports) {
  test(`grid renders correctly at ${vp.name} (${vp.width}px)`, async ({ page }) => {
    await page.setViewportSize({ width: vp.width, height: vp.height });
    await page.goto('/layout/grid.html');

    const container = page.locator('.container');
    const box = await container.boundingBox();

    if (vp.width < 576) {
      expect(box.width).toBe(vp.width - 24);
    } else {
      expect(box.width).toBeLessThanOrEqual(vp.width);
    }
  });
}
```

### Manual Device Simulation Checklist

```bash
# Using Chrome DevTools device simulation
# 1. Open DevTools (F12)
# 2. Toggle device toolbar (Ctrl+Shift+M)
# 3. Test these presets:
#    - iPhone SE (375x667)
#    - iPhone 12 Pro (390x844)
#    - iPad (768x1024)
#    - iPad Pro (1024x1366)
#    - Desktop (1280x720)
#    - Large Desktop (1920x1080)
```

## Advanced Variations

### Testing Navbar Collapse Behavior

```js
// tests/navbar-responsive.spec.js
const { test, expect } = require('@playwright/test');

test.describe('Navbar responsive behavior', () => {
  test('shows toggler and hides links on mobile', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    await page.goto('/components/navbar.html');

    await expect(page.locator('.navbar-toggler')).toBeVisible();
    await expect(page.locator('.navbar-collapse')).not.toBeVisible();
  });

  test('shows links and hides toggler on desktop', async ({ page }) => {
    await page.setViewportSize({ width: 1200, height: 800 });
    await page.goto('/components/navbar.html');

    await expect(page.locator('.navbar-toggler')).not.toBeVisible();
    await expect(page.locator('.navbar-collapse')).toBeVisible();
  });

  test('toggler opens collapse on mobile', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    await page.goto('/components/navbar.html');

    await page.click('.navbar-toggler');
    await expect(page.locator('.navbar-collapse')).toBeVisible();
    await expect(page.locator('.navbar-collapse')).toHaveClass(/show/);
  });
});
```

### Testing Offcanvas at Different Breakpoints

```js
// cypress/e2e/offcanvas-responsive.cy.js
describe('Offcanvas responsive', () => {
  it('renders as offcanvas on mobile', () => {
    cy.viewport(375, 667);
    cy.visit('/components/offcanvas.html');
    cy.get('.offcanvas').should('not.have.class', 'show');
    cy.get('[data-bs-toggle="offcanvas"]').click();
    cy.get('.offcanvas').should('have.class', 'show');
  });

  it('can use responsive offcanvas-backdrop', () => {
    cy.viewport(375, 667);
    cy.visit('/components/offcanvas.html');
    cy.get('[data-bs-toggle="offcanvas"]').click();
    cy.get('.offcanvas-backdrop').should('exist');
  });
});
```

## Best Practices

1. **Test all six Bootstrap breakpoints** - xs, sm, md, lg, xl, xxl - each can behave differently.
2. **Include real device dimensions** - iPhone (390px), iPad (810px), common desktop (1440px) alongside breakpoint values.
3. **Test both orientations** - Portrait and landscape on mobile/tablet viewports.
4. **Check for horizontal scroll** - No element should cause `overflow-x` at any viewport.
5. **Verify touch targets are 44x44px minimum** - WCAG 2.5.5 requires adequate target size on mobile.
6. **Test image responsiveness** - Verify `img-fluid` images scale without overflow or distortion.
7. **Validate text readability** - Font sizes should not be smaller than 16px on mobile for body text.
8. **Test intermediate widths** - Test values between breakpoints (e.g., 650px between sm and md).
9. **Use `cy.viewport()` before navigation** - Set viewport before `cy.visit()` for accurate initial render.
10. **Automate responsive tests in CI** - Run viewport tests across Chromium, Firefox, and WebKit.
11. **Check table responsiveness** - Verify `table-responsive` wrapper activates horizontal scroll on small screens.
12. **Test sticky elements** - Verify `sticky-top` and fixed-positioned elements don't overlap content on mobile.

## Common Pitfalls

1. **Testing only breakpoint boundary values** - 576px is the boundary; test 575px and 577px to confirm behavior.
2. **Ignoring scrollbar width** - A 1280px viewport with scrollbar gives ~1263px content width; this can affect layout.
3. **Not resetting viewport between tests** - Residual viewport size from a prior test affects subsequent assertions.
4. **Forgetting about Bootstrap containers** - Containers have max-width at each breakpoint; test container behavior specifically.
5. **Missing `meta viewport` tag** - Without `<meta name="viewport" content="width=device-width, initial-scale=1">`, mobile renders at desktop width.
6. **Not testing with browser chrome** - Mobile browsers have address bars and navigation bars that reduce available height.
7. **Assuming CSS-only responsive** - Some Bootstrap JS plugins (tooltip, popover) need repositioning on resize.
8. **Ignoring font size on zoom** - iOS Safari increases font sizes in landscape; test with default zoom.
9. **Hard-coding pixel assertions** - Use ranges (`greaterThan`, `lessThan`) instead of exact pixel values.
10. **Not testing image aspect ratios** - Images with `img-fluid` in flex containers can distort if `align-self` isn't set.

## Accessibility Considerations

Responsive testing intersects with accessibility:

- **Focus visibility on mobile** - Touch devices may not show focus rings; verify `:focus-visible` styles work.
- **Screen reader navigation order** - DOM order must match visual order at all breakpoints.
- **ARIA hidden on collapsed content** - Elements hidden with `d-none` at certain breakpoints should have `aria-hidden="true"`.
- **Zoom behavior** - Test at 200% browser zoom (WCAG 1.4.4) to ensure content reflows without horizontal scroll.

```js
test('content reflows at 200% zoom', async ({ page }) => {
  await page.setViewportSize({ width: 1280, height: 720 });
  await page.goto('/layout/grid.html');
  await page.evaluate(() => {
    document.documentElement.style.fontSize = '200%';
  });

  const body = await page.evaluate(() => document.body.scrollWidth);
  expect(body).toBeLessThanOrEqual(1280);
});
```

## Responsive Behavior

Testing responsive components systematically:

```js
// Comprehensive responsive test matrix
const components = [
  { path: '/components/navbar.html', name: 'Navbar' },
  { path: '/components/modal.html', name: 'Modal' },
  { path: '/components/table.html', name: 'Table' },
  { path: '/layout/grid.html', name: 'Grid' },
];

const viewports = [
  { name: 'mobile', width: 375 },
  { name: 'tablet', width: 768 },
  { name: 'desktop', width: 1280 },
];

for (const comp of components) {
  for (const vp of viewports) {
    test(`${comp.name} at ${vp.name}`, async ({ page }) => {
      await page.setViewportSize({ width: vp.width, height: 800 });
      await page.goto(comp.path);

      const hasOverflow = await page.evaluate(() => {
        return document.body.scrollWidth > window.innerWidth;
      });
      expect(hasOverflow).toBe(false);
    });
  }
}
```

```bash
# Automated responsive check script
for width in 375 576 768 992 1200 1400; do
  echo "Testing at ${width}px"
  npx playwright test --config=playwright.config.js --grep "responsive" --viewport=$width
done
```
