---
title: "Test-Driven Development with Bootstrap"
slug: "test-driven-bootstrap"
difficulty: 3
duration: "75 minutes"
prerequisites:
  - "Bootstrap 5 Components"
  - "TDD Fundamentals"
  - "Jest / Testing Library"
topics:
  - "Testing"
  - "TDD"
  - "Component Development"
  - "Test Coverage"
  - "Red-Green-Refactor"
tools:
  - "Jest"
  - "@testing-library/dom"
  - "Cypress"
  - "Playwright"
learning_objectives:
  - "Apply the TDD red-green-refactor cycle to Bootstrap component development"
  - "Write failing tests first, then implement Bootstrap components to pass them"
  - "Define and measure test coverage goals for Bootstrap projects"
  - "Build reusable component test patterns for common Bootstrap patterns"
---

## Overview

Test-Driven Development (TDD) flips the traditional workflow: write the test before the code. For Bootstrap components, this means defining expected classes, attributes, and behaviors in tests first, then implementing the HTML and JavaScript to satisfy those tests. The red-green-refactor cycle - write a failing test (red), make it pass (green), clean up (refactor) - produces well-tested, maintainable Bootstrap components with high coverage from the start.

TDD with Bootstrap focuses on class application, ARIA attribute correctness, interaction behaviors, and responsive class logic rather than testing Bootstrap's own internals.

## Basic Implementation

### TDD Cycle: Building an Alert Component

**Step 1 - RED: Write a failing test**

```js
// __tests__/alert.test.js
const { JSDOM } = require('jsdom');

test('renders a dismissible alert with correct classes', () => {
  const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
  const document = dom.window.document;

  // This will fail - element doesn't exist yet
  const alert = document.querySelector('.alert');
  expect(alert).toBeTruthy();
  expect(alert.classList.contains('alert-primary')).toBe(true);
  expect(alert.classList.contains('alert-dismissible')).toBe(true);
  expect(alert.classList.contains('fade')).toBe(true);
  expect(alert.classList.contains('show')).toBe(true);
});
```

**Step 2 - GREEN: Make it pass**

```html
<!-- components/alert.html -->
<div class="alert alert-primary alert-dismissible fade show" role="alert">
  <strong>Notice!</strong> This is a primary alert.
  <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
</div>
```

```js
// Updated test with DOM setup
test('renders a dismissible alert with correct classes', () => {
  const dom = new JSDOM(`
    <!DOCTYPE html><html><body>
      <div class="alert alert-primary alert-dismissible fade show" role="alert">
        <strong>Notice!</strong> This is a primary alert.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>
    </body></html>
  `);
  const document = dom.window.document;

  const alert = document.querySelector('.alert');
  expect(alert).toBeTruthy();
  expect(alert.classList.contains('alert-primary')).toBe(true);
  expect(alert.classList.contains('alert-dismissible')).toBe(true);
  expect(alert.classList.contains('fade')).toBe(true);
  expect(alert.classList.contains('show')).toBe(true);
});
```

**Step 3 - REFACTOR: Clean up**

```js
// __tests__/alert.test.js - refactored with helpers
function createAlert(variant, dismissible = true) {
  const dom = new JSDOM(`<!DOCTYPE html><html><body></body></html>`);
  const document = dom.window.document;

  const alert = document.createElement('div');
  alert.className = `alert alert-${variant}${dismissible ? ' alert-dismissible fade show' : ''}`;
  alert.setAttribute('role', 'alert');
  alert.innerHTML = `Alert message <button type="button" class="btn-close" aria-label="Close"></button>`;
  document.body.appendChild(alert);

  return { alert, document };
}

describe('Alert component', () => {
  test.each(['primary', 'success', 'danger', 'warning'])(
    'renders alert-%s with correct variant class',
    (variant) => {
      const { alert } = createAlert(variant);
      expect(alert.classList.contains(`alert-${variant}`)).toBe(true);
    }
  );

  test('dismissible alert has close button', () => {
    const { alert } = createAlert('primary', true);
    expect(alert.querySelector('.btn-close')).toBeTruthy();
    expect(alert.classList.contains('alert-dismissible')).toBe(true);
  });
});
```

### TDD for Form Validation States

```js
// __tests__/form-validation.test.js
describe('Form validation states', () => {
  test('valid input has is-valid class', () => {
    const dom = new JSDOM(`<!DOCTYPE html><html><body></body></html>`);
    const input = dom.window.document.createElement('input');
    input.className = 'form-control is-valid';
    input.setAttribute('aria-invalid', 'false');

    expect(input.classList.contains('is-valid')).toBe(true);
    expect(input.getAttribute('aria-invalid')).toBe('false');
  });

  test('invalid input has is-invalid class and feedback', () => {
    const dom = new JSDOM(`<!DOCTYPE html><html><body></body></html>`);
    const document = dom.window.document;

    const wrapper = document.createElement('div');
    wrapper.innerHTML = `
      <input class="form-control is-invalid" aria-invalid="true" aria-describedby="error-feedback">
      <div id="error-feedback" class="invalid-feedback">This field is required.</div>
    `;

    const input = wrapper.querySelector('input');
    const feedback = wrapper.querySelector('.invalid-feedback');

    expect(input.classList.contains('is-invalid')).toBe(true);
    expect(input.getAttribute('aria-invalid')).toBe(true);
    expect(input.getAttribute('aria-describedby')).toBe('error-feedback');
    expect(feedback.textContent).toBe('This field is required.');
  });
});
```

## Advanced Variations

### TDD for Modal Focus Management

```js
// __tests__/modal-focus.test.js
describe('Modal focus trap (TDD)', () => {
  test('initial failing test - focus trap', () => {
    const dom = new JSDOM(`<!DOCTYPE html><html><body></body></html>`);
    const document = dom.window.document;

    // RED: Test expects focus trap behavior we haven't implemented
    const modal = document.createElement('div');
    modal.className = 'modal fade';
    modal.setAttribute('role', 'dialog');
    modal.setAttribute('aria-modal', 'true');
    modal.innerHTML = `
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Title</h5>
            <button class="btn-close" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <input type="text" class="form-control" id="modalInput">
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary">Cancel</button>
            <button class="btn btn-primary">Save</button>
          </div>
        </div>
      </div>
    `);

    document.body.appendChild(modal);

    // GREEN: After implementing Bootstrap markup
    expect(modal.getAttribute('role')).toBe('dialog');
    expect(modal.getAttribute('aria-modal')).toBe('true');

    const focusable = modal.querySelectorAll(
      'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
    );
    expect(focusable.length).toBeGreaterThanOrEqual(3);
  });
});
```

### TDD for Responsive Component Behavior

```js
// __tests__/responsive-component.test.js
function getColumnClasses(width) {
  if (width < 576) return ['col-12'];
  if (width < 768) return ['col-sm-6'];
  if (width < 992) return ['col-md-4'];
  return ['col-lg-3'];
}

describe('Responsive card grid (TDD)', () => {
  // RED: Define expected behavior before implementation
  test('single column on mobile', () => {
    expect(getColumnClasses(375)).toContain('col-12');
  });

  test('two columns on small tablet', () => {
    expect(getColumnClasses(640)).toContain('col-sm-6');
  });

  test('three columns on tablet', () => {
    expect(getColumnClasses(800)).toContain('col-md-4');
  });

  test('four columns on desktop', () => {
    expect(getColumnClasses(1200)).toContain('col-lg-3');
  });

  test('row contains correct number of cards', () => {
    const dom = new JSDOM(`<!DOCTYPE html><html><body></body></html>`);
    const document = dom.window.document;

    const row = document.createElement('div');
    row.className = 'row g-4';

    for (let i = 0; i < 4; i++) {
      const col = document.createElement('div');
      col.className = 'col-lg-3 col-md-4 col-sm-6 col-12';
      col.innerHTML = `<div class="card"><div class="card-body">Card ${i + 1}</div></div>`;
      row.appendChild(col);
    }

    document.body.appendChild(row);
    expect(row.querySelectorAll('.card')).toHaveLength(4);
    expect(row.querySelectorAll('[class*="col-"]')).toHaveLength(4);
  });
});
```

### TDD for Dynamic Toggle Component

```js
// __tests__/toggle.test.js
class ToggleButton {
  constructor(document, label, initialState = false) {
    this.el = document.createElement('button');
    this.el.className = 'btn btn-outline-primary';
    this.el.textContent = label;
    this.el.setAttribute('aria-pressed', String(initialState));
    this.active = initialState;
    if (initialState) this.el.classList.add('active');

    this.el.addEventListener('click', () => this.toggle());
  }

  toggle() {
    this.active = !this.active;
    this.el.classList.toggle('active', this.active);
    this.el.setAttribute('aria-pressed', String(this.active));
    return this.active;
  }
}

describe('Toggle button (TDD)', () => {
  let document;

  beforeEach(() => {
    const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
    document = dom.window.document;
  });

  test('starts inactive by default', () => {
    const btn = new ToggleButton(document, 'Toggle');
    expect(btn.active).toBe(false);
    expect(btn.el.classList.contains('active')).toBe(false);
    expect(btn.el.getAttribute('aria-pressed')).toBe('false');
  });

  test('starts active when initialState is true', () => {
    const btn = new ToggleButton(document, 'Toggle', true);
    expect(btn.active).toBe(true);
    expect(btn.el.classList.contains('active')).toBe(true);
    expect(btn.el.getAttribute('aria-pressed')).toBe('true');
  });

  test('click toggles state', () => {
    const btn = new ToggleButton(document, 'Toggle');
    btn.el.click();
    expect(btn.active).toBe(true);
    expect(btn.el.classList.contains('active')).toBe(true);
    expect(btn.el.getAttribute('aria-pressed')).toBe('true');

    btn.el.click();
    expect(btn.active).toBe(false);
    expect(btn.el.classList.contains('active')).toBe(false);
  });
});
```

## Best Practices

1. **Write the test first, always** - Resist the urge to write HTML before the test; TDD discipline produces better coverage.
2. **Test behavior, not implementation** - Assert "modal opens on click" not "classList.add('show')" when possible.
3. **Use the smallest possible test** - One assertion per test when feasible; compound assertions hide root causes.
4. **Start with the simplest case** - Test a single Bootstrap alert before building an alert system with auto-dismiss.
5. **Refactor ruthlessly after green** - Clean up test and component code; extract helpers, remove duplication.
6. **Aim for 80%+ coverage on custom code** - Bootstrap's own code doesn't need coverage; focus on your integration layer.
7. **Test edge cases early** - Empty strings, null values, invalid class names should be in the first test round.
8. **Use test doubles for Bootstrap JS** - Mock `bootstrap.Modal`, `bootstrap.Dropdown` when testing component wrappers.
9. **Run tests on every save** - Use `jest --watch` or `vitest --watch` for instant feedback during TDD.
10. **Write integration tests after unit tests** - TDD unit tests build confidence; then verify with browser-based integration tests.
11. **Document TDD decisions** - Use test names as living documentation of component requirements.
12. **Measure and track coverage** - Use `jest --coverage` and set minimum thresholds in CI.

## Common Pitfalls

1. **Testing Bootstrap's internals** - Trust that `btn-primary` applies the correct color; test your code's usage.
2. **Writing too many tests before running them** - TDD means one failing test, then make it pass, then repeat.
3. **Skipping the refactor step** - Green tests with messy code accumulate technical debt quickly.
4. **Over-testing trivial markup** - Don't test that `<div class="container">` has the container class; that's Bootstrap's job.
5. **Ignoring test isolation** - Each test must work independently; shared DOM state causes flaky failures.
6. **Not resetting mocks** - Forgetting `jest.clearAllMocks()` between tests leads to false positives.
7. **Writing tests that are too coupled to markup** - Use `data-testid` or semantic selectors instead of structural CSS selectors.
8. **Achieving 100% coverage at the expense of quality** - Some code paths (error handlers, edge cases) may not justify the cost.
9. **TDD without design** - Have a clear component API in mind before writing tests; tests without design lead to awkward APIs.
10. **Forgetting negative tests** - Test what should NOT happen: missing classes, invalid states, null elements.

## Accessibility Considerations

TDD naturally improves accessibility because tests must pass before implementation:

```js
// TDD: Accessibility-first test for modal
test('modal has accessible attributes before rendering', () => {
  const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
  const modal = dom.window.document.createElement('div');

  // TDD forces us to define these requirements upfront
  modal.className = 'modal fade';
  modal.setAttribute('role', 'dialog');
  modal.setAttribute('aria-modal', 'true');
  modal.setAttribute('aria-labelledby', 'modalTitle');
  modal.setAttribute('aria-describedby', 'modalDesc');
  modal.setAttribute('tabindex', '-1');

  expect(modal.getAttribute('role')).toBe('dialog');
  expect(modal.getAttribute('aria-modal')).toBe('true');
  expect(modal.getAttribute('aria-labelledby')).toBe('modalTitle');
  expect(modal.getAttribute('aria-describedby')).toBe('modalDesc');
  expect(modal.getAttribute('tabindex')).toBe('-1');
});
```

```js
// TDD: Aria-live region for toast notifications
test('toast container has aria-live for announcements', () => {
  const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
  const container = dom.window.document.createElement('div');
  container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
  container.setAttribute('aria-live', 'polite');
  container.setAttribute('aria-atomic', 'true');

  expect(container.getAttribute('aria-live')).toBe('polite');
  expect(container.getAttribute('aria-atomic')).toBe('true');
});
```

## Responsive Behavior

TDD for responsive components means defining viewport behavior in tests first:

```js
// TDD: Define responsive navbar behavior before implementing
describe('Responsive navbar (TDD)', () => {
  test('toggler is defined for mobile viewport', () => {
    const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
    const nav = dom.window.document.createElement('nav');
    nav.className = 'navbar navbar-expand-lg';
    nav.innerHTML = `
      <div class="container-fluid">
        <a class="navbar-brand" href="#">Brand</a>
        <button class="navbar-toggler" type="button" aria-controls="navContent" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navContent">
          <ul class="navbar-nav">
            <li class="nav-item"><a class="nav-link active" href="#">Home</a></li>
          </ul>
        </div>
      </div>
    `;

    const toggler = nav.querySelector('.navbar-toggler');
    const collapse = nav.querySelector('.navbar-collapse');

    expect(toggler.getAttribute('aria-controls')).toBe('navContent');
    expect(toggler.getAttribute('aria-expanded')).toBe('false');
    expect(collapse.id).toBe('navContent');
    expect(collapse.classList.contains('collapse')).toBe(true);
  });

  test('active nav link has aria-current', () => {
    const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
    const link = dom.window.document.createElement('a');
    link.className = 'nav-link active';
    link.setAttribute('aria-current', 'page');
    link.href = '#';
    link.textContent = 'Home';

    expect(link.classList.contains('active')).toBe(true);
    expect(link.getAttribute('aria-current')).toBe('page');
  });
});
```

Test coverage goals for a TDD Bootstrap project:
- **Unit tests**: 80%+ of custom component logic
- **Integration tests**: All user-facing interactive flows
- **Accessibility tests**: 100% of interactive components
- **Visual regression**: All component variants at all breakpoints
