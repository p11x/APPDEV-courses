---
title: "Dropdown JavaScript API"
module: "JavaScript Components"
lesson: "04_02_01"
difficulty: 2
estimated_time: 20 minutes
bootstrap_version: 5.3
prerequisites:
  - Basic HTML/CSS knowledge
  - Bootstrap 5 basics
  - JavaScript fundamentals
learning_objectives:
  - Create dropdowns programmatically using the JavaScript API
  - Control dropdown visibility with show/hide/toggle methods
  - Handle dropdown lifecycle events
  - Properly dispose of dropdown instances
---

# Dropdown JavaScript API

## Overview

Bootstrap 5 provides a full JavaScript API for dropdown components, enabling programmatic control beyond what `data-bs-*` attributes offer. The `bootstrap.Dropdown` class allows you to create, show, hide, toggle, and destroy dropdown instances with fine-grained control.

The JavaScript API is especially useful when you need to trigger dropdowns from custom UI elements, synchronize dropdown state with application logic, or build dynamic interfaces where dropdowns are created and destroyed at runtime.

```js
// Basic initialization
const dropdownEl = document.getElementById('myDropdown');
const dropdown = new bootstrap.Dropdown(dropdownEl);
```

Every dropdown instance exposes methods for controlling visibility and lifecycle, plus a full set of events that fire at each stage of the show/hide transition.

## Basic Implementation

### Creating a Dropdown Instance

To initialize a dropdown via JavaScript, pass a DOM element (the toggle button) to the constructor:

```html
<div class="dropdown">
  <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownToggle">
    Actions
  </button>
  <ul class="dropdown-menu" aria-labelledby="dropdownToggle">
    <li><a class="dropdown-item" href="#">Edit</a></li>
    <li><a class="dropdown-item" href="#">Duplicate</a></li>
    <li><hr class="dropdown-divider"></li>
    <li><a class="dropdown-item text-danger" href="#">Delete</a></li>
  </ul>
</div>
```

```js
const toggleBtn = document.getElementById('dropdownToggle');
const dropdown = new bootstrap.Dropdown(toggleBtn);

// Show the dropdown programmatically
dropdown.show();

// Hide it after 3 seconds
setTimeout(() => dropdown.hide(), 3000);
```

### Available Options

The constructor accepts an optional configuration object:

```js
const dropdown = new bootstrap.Dropdown(toggleBtn, {
  autoClose: true,       // 'inside', 'outside', true, false
  reference: 'toggle'    // 'toggle', 'parent', or DOM element
});
```

The `autoClose` option controls when the dropdown closes: clicking inside, outside, both, or never.

## Advanced Variations

### Listening to Lifecycle Events

Dropdowns emit four events during their show/hide cycle. These are essential for synchronizing external state:

```html
<div class="dropdown">
  <button class="btn btn-primary dropdown-toggle" type="button" id="eventDropdown">
    Select Option
  </button>
  <ul class="dropdown-menu" aria-labelledby="eventDropdown">
    <li><a class="dropdown-item" href="#">Option A</a></li>
    <li><a class="dropdown-item" href="#">Option B</a></li>
  </ul>
</div>
<div id="statusLog" class="mt-2 text-muted small"></div>
```

```js
const el = document.getElementById('eventDropdown');
const log = document.getElementById('statusLog');

el.addEventListener('show.bs.dropdown', () => {
  log.textContent = 'Dropdown is about to show...';
});

el.addEventListener('shown.bs.dropdown', () => {
  log.textContent = 'Dropdown is now visible.';
});

el.addEventListener('hide.bs.dropdown', () => {
  log.textContent = 'Dropdown is about to hide...';
});

el.addEventListener('hidden.bs.dropdown', () => {
  log.textContent = 'Dropdown is now hidden.';
});
```

### Retrieving an Existing Instance

Bootstrap stores the instance on the DOM element. Use `getInstance` or `getOrCreateInstance` to retrieve it:

```js
// Retrieve existing instance (returns null if none exists)
const existing = bootstrap.Dropdown.getInstance(el);

// Get or create
const instance = bootstrap.Dropdown.getOrCreateInstance(el);
```

## Best Practices

1. **Use `getOrCreateInstance`** to prevent duplicate instances on the same element.
2. **Always set `aria-labelledby`** on the dropdown menu linking it to the toggle button.
3. **Dispose of instances** when removing dropdown elements from the DOM to avoid memory leaks.
4. **Prefer `data-bs-*` attributes** for simple cases; use the JS API only when you need programmatic control.
5. **Listen for `hidden.bs.dropdown`** before performing DOM changes inside the dropdown to avoid layout shifts.
6. **Use `autoClose: false`** when building complex menus that contain forms or interactive content inside the dropdown.
7. **Keep dropdown menus keyboard-navigable** by ensuring focusable items use proper markup.
8. **Store the instance in a variable** for repeated access rather than calling `getInstance` on every operation.
9. **Use `reference` option** when the dropdown menu needs to be positioned relative to a different element than the toggle.
10. **Avoid nesting dropdowns** inside other dropdowns; use submenus with proper ARIA roles instead.
11. **Validate the toggle element** exists before passing it to the constructor to prevent runtime errors.

## Common Pitfalls

1. **Not disposing instances before removing elements** — causes memory leaks and orphaned event listeners.
2. **Calling methods on uninitialized elements** — always check that the instance exists before invoking `show()` or `hide()`.
3. **Confusing the toggle button with the menu** — the constructor takes the toggle button, not the `.dropdown-menu` element.
4. **Ignoring `autoClose` behavior** — users expect dropdowns to close on outside clicks; setting `autoClose: false` without visible UI cues confuses users.
5. **Missing `aria` attributes** — without `aria-expanded` and `aria-labelledby`, screen readers cannot interpret the dropdown relationship.
6. **Using `display: none` CSS overrides** — Bootstrap manages visibility via classes; overriding with `display: none` breaks animations and events.
7. **Creating duplicate instances** — calling `new bootstrap.Dropdown(el)` twice on the same element overwrites the first instance without disposing it.

## Accessibility Considerations

- The toggle button must have `aria-haspopup="true"` and `aria-expanded` that updates dynamically (Bootstrap handles this automatically when using the API).
- Each menu item should be a focusable element (`<a>` or `<button>`) inside an `<li>` with `role="menuitem"`.
- The dropdown menu should have `role="menu"` and use `aria-labelledby` to reference the toggle.
- Escape key should close the dropdown — Bootstrap handles this by default.
- Focus management: when the dropdown opens, focus should move to the first menu item; when it closes, focus should return to the toggle button.

```html
<button class="btn btn-secondary dropdown-toggle"
        type="button"
        id="accessibleDropdown"
        aria-haspopup="true"
        aria-expanded="false">
  Menu
</button>
<ul class="dropdown-menu" role="menu" aria-labelledby="accessibleDropdown">
  <li role="menuitem"><a class="dropdown-item" href="#">Action</a></li>
  <li role="menuitem"><a class="dropdown-item" href="#">Another action</a></li>
</ul>
```

## Responsive Behavior

Dropdowns are responsive by default — they adapt to viewport size automatically. However, consider these patterns:

- On **mobile devices**, dropdowns that open near screen edges may overflow. Use `autoClose: 'outside'` so tapping the overlay area closes the menu.
- For **touch interfaces**, ensure menu items have sufficient tap targets (minimum 44x44px). Add `py-2` or `py-3` classes to `.dropdown-item` elements.
- In **responsive navigation**, dropdowns inside collapsed navbars behave differently. Use the JS API to programmatically close dropdowns when the navbar collapses:

```js
const navbarCollapse = document.querySelector('.navbar-collapse');
navbarCollapse.addEventListener('hidden.bs.collapse', () => {
  const openDropdowns = document.querySelectorAll('.dropdown-menu.show');
  openDropdowns.forEach(menu => {
    const toggle = menu.previousElementSibling;
    const instance = bootstrap.Dropdown.getInstance(toggle);
    if (instance) instance.hide();
  });
});
```
