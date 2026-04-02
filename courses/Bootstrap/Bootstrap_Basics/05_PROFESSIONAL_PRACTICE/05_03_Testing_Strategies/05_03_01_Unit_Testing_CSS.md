---
title: "Unit Testing Bootstrap CSS"
slug: "unit-testing-css"
difficulty: 2
duration: "45 minutes"
prerequisites:
  - "Bootstrap 5 Basics"
  - "JavaScript Fundamentals"
  - "Jest Testing Framework"
topics:
  - "Testing"
  - "CSS Validation"
  - "Jest"
  - "jsdom"
  - "Unit Tests"
tools:
  - "Jest"
  - "jsdom"
  - "@testing-library/dom"
learning_objectives:
  - "Verify Bootstrap classes are applied correctly to DOM elements"
  - "Write unit tests for computed styles and CSS class presence"
  - "Integrate jsdom for DOM-based CSS testing"
  - "Test dynamic class toggling with Bootstrap utility classes"
---

## Overview

Unit testing Bootstrap CSS involves verifying that the correct classes are applied to elements and that computed styles match expectations. Unlike functional testing, unit tests focus on isolated DOM fragments, making them fast and deterministic. Using Jest with jsdom, you can simulate a browser environment to assert class presence, inline style overrides, and conditional class logic without a real browser.

This approach is essential when building dynamic Bootstrap components in JavaScript frameworks (React, Vue, Angular) where classes are applied programmatically.

## Basic Implementation

### Testing Class Presence with Jest + jsdom

Set up a basic test that checks if Bootstrap utility classes are applied:

```bash
npm install --save-dev jest jsdom @testing-library/jest-dom
```

```js
// __tests__/class-presence.test.js
const { JSDOM } = require('jsdom');

describe('Bootstrap class application', () => {
  let document;

  beforeEach(() => {
    const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
    document = dom.window.document;
  });

  test('button has btn and btn-primary classes', () => {
    const btn = document.createElement('button');
    btn.className = 'btn btn-primary';
    expect(btn.classList.contains('btn')).toBe(true);
    expect(btn.classList.contains('btn-primary')).toBe(true);
  });

  test('alert dismissible adds btn-close', () => {
    const alert = document.createElement('div');
    alert.className = 'alert alert-warning alert-dismissible fade show';
    const closeBtn = document.createElement('button');
    closeBtn.className = 'btn-close';
    alert.appendChild(closeBtn);

    expect(alert.classList.contains('alert-dismissible')).toBe(true);
    expect(closeBtn.classList.contains('btn-close')).toBe(true);
  });
});
```

### Testing Computed Styles

```html
<!-- component.html -->
<div class="container">
  <div class="row">
    <div class="col-md-6 bg-primary text-white p-3">
      Content
    </div>
  </div>
</div>
```

```js
// __tests__/computed-styles.test.js
const { JSDOM } = require('jsdom');

describe('Computed style assertions', () => {
  test('container has max-width set by Bootstrap', () => {
    const dom = new JSDOM(`
      <!DOCTYPE html>
      <html>
        <head><link rel="stylesheet" href="bootstrap.min.css"></head>
        <body><div class="container" id="test"></div></body>
      </html>
    `, { resources: 'usable', runScripts: 'dangerously' });

    const el = dom.window.document.getElementById('test');
    const style = dom.window.getComputedStyle(el);
    expect(style.maxWidth).not.toBe('none');
  });
});
```

## Advanced Variations

### Testing Dynamic Class Toggling

```js
// __tests__/dynamic-toggle.test.js
function toggleClass(element, className) {
  element.classList.toggle(className);
  return element.classList.contains(className);
}

describe('Dynamic Bootstrap class toggling', () => {
  let el;

  beforeEach(() => {
    el = { classList: new Set(['btn', 'btn-outline-secondary']) };
    el.classList.contains = el.classList.has.bind(el.classList);
    el.classList.toggle = (cls) => {
      el.classList.has(cls) ? el.classList.delete(cls) : el.classList.add(cls);
    };
  });

  test('toggling active class adds it when absent', () => {
    expect(toggleClass(el, 'active')).toBe(true);
    expect(el.classList.contains('active')).toBe(true);
  });

  test('toggling active class removes it when present', () => {
    el.classList.add('active');
    expect(toggleClass(el, 'active')).toBe(false);
  });
});
```

### Testing Responsive Class Logic

```js
// __tests__/responsive-classes.test.js
function getResponsiveClasses(width) {
  if (width < 576) return ['col-12'];
  if (width < 768) return ['col-sm-6'];
  if (width < 992) return ['col-md-4'];
  return ['col-lg-3'];
}

describe('Responsive column calculation', () => {
  test.each([
    [375, 'col-12'],
    [640, 'col-sm-6'],
    [800, 'col-md-4'],
    [1200, 'col-lg-3'],
  ])('width %i returns class %s', (width, expected) => {
    expect(getResponsiveClasses(width)).toContain(expected);
  });
});
```

## Best Practices

1. **Isolate DOM tests** - Use fresh JSDOM instances per test to prevent state leakage.
2. **Mock Bootstrap JS only when needed** - For CSS-only tests, skip Bootstrap's JS bundle entirely.
3. **Assert class lists, not exact strings** - Use `classList.contains()` rather than `className` string matching.
4. **Group tests by component** - Mirror your component structure in test directories.
5. **Use `@testing-library/jest-dom`** - Provides `toHaveClass()`, `toHaveStyle()` matchers for cleaner assertions.
6. **Test conditional classes** - Verify that `show`/`hide` classes toggle correctly.
7. **Avoid testing Bootstrap internals** - Trust that Bootstrap's CSS works; test your integration with it.
8. **Keep tests under 100ms** - Unit tests should be blazing fast; avoid network requests.
9. **Use descriptive test names** - `"adds .active when toggled"` over `"test toggle"`.
10. **Test edge cases** - Empty class strings, duplicate classes, null elements.
11. **Version-lock Bootstrap in tests** - Pin the Bootstrap version to avoid assertion drift.
12. **Run tests in CI** - Ensure CSS unit tests pass before every merge.

## Common Pitfalls

1. **Testing Bootstrap's own CSS correctness** - Bootstrap is already tested; focus on your code's usage of it.
2. **Using `className` string comparison** - Fails with ordering differences; use `classList` instead.
3. **Forgetting to load CSS in jsdom** - Computed styles return defaults without stylesheets; load Bootstrap CSS or mock values.
4. **Not cleaning up DOM between tests** - Residual elements cause false positives in class assertions.
5. **Assuming `getComputedStyle` works like a browser** - jsdom's computed style support is incomplete for some CSS properties.
6. **Hard-coding Bootstrap breakpoint values** - If Bootstrap updates breakpoints, tests break; reference `bootstrap/scss/_variables.scss`.
7. **Testing visual appearance in unit tests** - Pixel-level rendering belongs in visual regression tests, not unit tests.
8. **Ignoring specificity conflicts** - Custom CSS overriding Bootstrap classes can cause confusing test failures.
9. **Not mocking animations** - Bootstrap transitions can cause timing issues in jsdom; mock `transitionend` events.
10. **Testing too many classes at once** - Each assertion should verify one class or one style property.

## Accessibility Considerations

When unit testing Bootstrap CSS, also verify accessibility-related class application:

- Ensure `visually-hidden` class is applied to screen-reader-only labels.
- Verify `aria-*` attributes persist alongside Bootstrap classes during dynamic updates.
- Test that focus-visible styles are correctly applied when elements gain focus.

```js
test('sr-only label is visually hidden', () => {
  const label = document.createElement('span');
  label.className = 'visually-hidden';
  label.textContent = 'Close';
  expect(label.classList.contains('visually-hidden')).toBe(true);
});

test('aria attributes survive class toggle', () => {
  const btn = document.createElement('button');
  btn.setAttribute('aria-expanded', 'false');
  btn.classList.toggle('collapsed');
  expect(btn.getAttribute('aria-expanded')).toBe('false');
});
```

## Responsive Behavior

Unit tests should verify responsive class logic:

```js
// Mock window.matchMedia for responsive tests
Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: jest.fn().mockImplementation(query => ({
    matches: query.includes('768') ? true : false,
    addEventListener: jest.fn(),
    removeEventListener: jest.fn(),
  })),
});

test('applies col-md-6 at medium breakpoint', () => {
  const el = document.createElement('div');
  if (window.matchMedia('(min-width: 768px)').matches) {
    el.classList.add('col-md-6');
  }
  expect(el.classList.contains('col-md-6')).toBe(true);
});
```

Test responsive utilities by mocking viewport dimensions and asserting the correct responsive classes are applied at each breakpoint threshold.
