---
title: "Collapse JavaScript API"
module: "JavaScript Components"
lesson: "04_02_05"
difficulty: 2
estimated_time: 20 minutes
bootstrap_version: 5.3
prerequisites:
  - Bootstrap 5 collapse markup
  - JavaScript fundamentals
learning_objectives:
  - Initialize collapse components programmatically
  - Control collapse with show/hide/toggle methods
  - Handle collapse lifecycle events
  - Build accordion and multi-target collapse patterns
---

# Collapse JavaScript API

## Overview

The Bootstrap Collapse API provides programmatic control over expandable content sections. The `bootstrap.Collapse` class manages the animation, state, and event lifecycle for collapsible elements. It powers both standalone collapsible sections and accordion groups.

Collapse is commonly used for FAQ sections, filter panels, mobile navigation menus, and accordion-style content organization. The JavaScript API extends these use cases with dynamic control and event-driven behavior.

```js
const collapseEl = document.getElementById('myCollapse');
const collapse = new bootstrap.Collapse(collapseEl, {
  toggle: false  // Don't auto-toggle on init
});
```

## Basic Implementation

### Basic Collapsible Section

```html
<p>
  <button class="btn btn-primary" type="button" id="toggleBtn"
          data-bs-toggle="collapse" data-bs-target="#contentArea">
    Toggle Content
  </button>
</p>
<div class="collapse" id="contentArea">
  <div class="card card-body">
    This content can be collapsed and expanded.
  </div>
</div>
```

```js
const collapseEl = document.getElementById('contentArea');
const collapse = new bootstrap.Collapse(collapseEl, {
  toggle: false // Start collapsed, don't auto-show
});

// Show
collapse.show();

// Hide
collapse.hide();

// Toggle
collapse.toggle();
```

### Instance Retrieval

```js
const existing = bootstrap.Collapse.getInstance(collapseEl);
const instance = bootstrap.Collapse.getOrCreateInstance(collapseEl);
```

## Advanced Variations

### Accordion with Parent Grouping

Bootstrap accordion uses collapse with the `data-bs-parent` attribute to enforce single-open behavior:

```html
<div class="accordion" id="faqAccordion">
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button" type="button" data-bs-toggle="collapse"
              data-bs-target="#faq1">Question 1</button>
    </h2>
    <div id="faq1" class="accordion-collapse collapse show" data-bs-parent="#faqAccordion">
      <div class="accordion-body">Answer 1</div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
              data-bs-target="#faq2">Question 2</button>
    </h2>
    <div id="faq2" class="accordion-collapse collapse" data-bs-parent="#faqAccordion">
      <div class="accordion-body">Answer 2</div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
              data-bs-target="#faq3">Question 3</button>
    </h2>
    <div id="faq3" class="accordion-collapse collapse" data-bs-parent="#faqAccordion">
      <div class="accordion-body">Answer 3</div>
    </div>
  </div>
</div>
```

```js
// Programmatically open the second accordion item
const faq2 = new bootstrap.Collapse(document.getElementById('faq2'));
faq2.show();
```

### Lifecycle Events

```js
const collapseEl = document.getElementById('contentArea');

collapseEl.addEventListener('show.bs.collapse', () => {
  console.log('Collapse is expanding...');
  // Good place to start loading content
});

collapseEl.addEventListener('shown.bs.collapse', () => {
  console.log('Collapse is fully expanded');
  // Initialize components that need visible container
});

collapseEl.addEventListener('hide.bs.collapse', () => {
  console.log('Collapse is collapsing...');
});

collapseEl.addEventListener('hidden.bs.collapse', () => {
  console.log('Collapse is fully collapsed');
  // Cleanup resources
});
```

### Multi-Target Collapse

A single button can toggle multiple collapse targets by using a shared class selector:

```html
<button class="btn btn-outline-danger" type="button" id="collapseAll"
        data-bs-toggle="collapse" data-bs-target=".multi-collapse">
  Collapse All Sections
</button>

<div class="collapse multi-collapse show" id="section1">
  <div class="card card-body mt-2">Section 1 content</div>
</div>
<div class="collapse multi-collapse show" id="section2">
  <div class="card card-body mt-2">Section 2 content</div>
</div>
<div class="collapse multi-collapse show" id="section3">
  <div class="card card-body mt-2">Section 3 content</div>
</div>
```

```js
// Toggle all sections programmatically
document.querySelectorAll('.multi-collapse').forEach(el => {
  const instance = bootstrap.Collapse.getOrCreateInstance(el);
  instance.toggle();
});

// Expand all
document.querySelectorAll('.multi-collapse').forEach(el => {
  bootstrap.Collapse.getOrCreateInstance(el).show();
});

// Collapse all
document.querySelectorAll('.multi-collapse').forEach(el => {
  bootstrap.Collapse.getOrCreateInstance(el).hide();
});
```

## Best Practices

1. **Use `toggle: false`** when initializing to prevent the collapse from auto-toggling on page load.
2. **Always pair with a toggle button** that has `data-bs-toggle="collapse"` and `data-bs-target="#id"`.
3. **Use `data-bs-parent`** on accordion children to enforce single-open behavior.
4. **Listen for `shown.bs.collapse`** before manipulating content inside the expanded area.
5. **Use `.collapsing` class awareness** — during animation, the element has this class. Avoid interfering with its styles.
6. **Dispose instances** when removing collapsible elements from the DOM to prevent memory leaks.
7. **Use `getOrCreateInstance`** to avoid creating duplicate instances on the same element.
8. **Keep collapse content semantic** — use headings, lists, and appropriate HTML inside the collapsed region.
9. **Use vertical padding** (`py-3`) on collapse bodies rather than margins for smoother animations.
10. **Avoid setting `height` or `overflow`** on collapse elements directly; Bootstrap manages these during transitions.
11. **Use `collapse-horizontal`** class for horizontal collapse animations instead of vertical.

## Common Pitfalls

1. **Missing `data-bs-target`** on the toggle button — the button won't control any collapse element.
2. **Using `id` selectors incorrectly** — `data-bs-target` must include the `#` prefix (e.g., `#myCollapse`).
3. **Interfering with `.collapsing` styles** — overriding `transition` or `height` on `.collapsing` breaks animations.
4. **Not using `data-bs-parent`** in accordions — all panels can be open simultaneously without it.
5. **Calling `show()` on an already-visible collapse** — causes unexpected behavior. Check the element's state first.
6. **Placing focusable elements inside collapsed content without managing focus** — keyboard users may not know the content appeared.
7. **Using collapse for content that should always be accessible** — hiding essential content behind collapse hurts usability and SEO.

## Accessibility Considerations

- Toggle buttons should have `aria-expanded` that reflects the current state (Bootstrap manages this automatically).
- The collapse region should be referenced by `aria-controls` on the toggle button.
- When using accordion, each header button should have `aria-expanded` and `aria-controls` pointing to the corresponding panel.
- Content inside the collapse should be keyboard-accessible and follow logical tab order.
- Use `aria-labelledby` on collapse panels to reference their controlling header buttons.

```html
<button class="btn btn-primary" type="button"
        data-bs-toggle="collapse" data-bs-target="#accessiblePanel"
        aria-expanded="false" aria-controls="accessiblePanel">
  Show Details
</button>
<div class="collapse" id="accessiblePanel">
  <div class="card card-body">
    <p>Accessible content that screen readers can navigate.</p>
    <a href="#" class="btn btn-sm btn-link">Learn more</a>
  </div>
</div>
```

```js
// Update ARIA state manually if needed
const btn = document.getElementById('myToggle');
const collapseEl = document.getElementById('myCollapse');

collapseEl.addEventListener('shown.bs.collapse', () => {
  btn.setAttribute('aria-expanded', 'true');
});

collapseEl.addEventListener('hidden.bs.collapse', () => {
  btn.setAttribute('aria-expanded', 'false');
});
```

## Responsive Behavior

Collapse components work across all screen sizes, but consider these patterns:

- **Mobile navigation**: Use collapse for responsive navbar toggling. The navbar collapse is a standard Bootstrap pattern:

```html
<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Brand</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
            data-bs-target="#navbarContent">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarContent">
      <ul class="navbar-nav">
        <li class="nav-item"><a class="nav-link" href="#">Home</a></li>
        <li class="nav-item"><a class="nav-link" href="#">About</a></li>
      </ul>
    </div>
  </div>
</nav>
```

- **Filter panels on mobile**: Auto-collapse filter sections on small screens:

```js
function handleResponsiveFilters() {
  const filterCollapse = document.getElementById('filterPanel');
  const instance = bootstrap.Collapse.getOrCreateInstance(filterCollapse);
  if (window.innerWidth < 768) {
    instance.hide();
  } else {
    instance.show();
  }
}
window.addEventListener('resize', handleResponsiveFilters);
```

- **Horizontal collapse** for side panels on wide screens using `.collapse-horizontal` with a fixed width.
