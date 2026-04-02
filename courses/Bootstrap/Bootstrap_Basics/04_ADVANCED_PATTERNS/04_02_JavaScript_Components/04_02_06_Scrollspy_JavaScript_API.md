---
title: "Scrollspy JavaScript API"
module: "JavaScript Components"
lesson: "04_02_06"
difficulty: 3
estimated_time: 25 minutes
bootstrap_version: 5.3
prerequisites:
  - Bootstrap 5 navigation components
  - Understanding of scroll events
  - CSS positioning (relative, fixed, sticky)
learning_objectives:
  - Initialize Scrollspy programmatically with the JavaScript API
  - Configure target, offset, and rootMargin options
  - Use the refresh method after dynamic content changes
  - Handle Scrollspy activation events
---

# Scrollspy JavaScript API

## Overview

Bootstrap Scrollspy automatically updates navigation links based on scroll position, highlighting the section currently visible in the viewport. The `bootstrap.Scrollspy` class provides programmatic initialization, fine-tuned offset configuration, and a `refresh` method for dynamic content.

Scrollspy is commonly used for documentation sidebars, single-page application navigation, and long-form content with a table of contents. The JavaScript API enables precise control that `data-bs-spy="scroll"` attributes alone cannot provide.

```js
const scrollspy = new bootstrap.Scrollspy(document.body, {
  target: '#navbar-example',
  offset: 100
});
```

## Basic Implementation

### HTML Setup

```html
<nav id="navbar-example" class="navbar navbar-light bg-light px-3 sticky-top">
  <a class="navbar-brand" href="#">Documentation</a>
  <ul class="nav nav-pills">
    <li class="nav-item">
      <a class="nav-link" href="#section-intro">Introduction</a>
    </li>
    <li class="nav-item">
      <a class="nav-link" href="#section-setup">Setup</a>
    </li>
    <li class="nav-item">
      <a class="nav-link" href="#section-usage">Usage</a>
    </li>
    <li class="nav-item">
      <a class="nav-link" href="#section-api">API</a>
    </li>
  </ul>
</nav>
<div data-bs-spy="scroll" data-bs-target="#navbar-example" data-bs-offset="0"
     class="scrollspy-example" tabindex="0" style="height: 400px; overflow-y: scroll;">
  <h4 id="section-intro">Introduction</h4>
  <p>Introduction content...</p>
  <h4 id="section-setup">Setup</h4>
  <p>Setup content...</p>
  <h4 id="section-usage">Usage</h4>
  <p>Usage content...</p>
  <h4 id="section-api">API</h4>
  <p>API content...</p>
</div>
```

### JavaScript Initialization

```js
const scrollEl = document.querySelector('.scrollspy-example');
const spy = new bootstrap.Scrollspy(scrollEl, {
  target: '#navbar-example',
  offset: 10
});

// Refresh after content changes
spy.refresh();
```

### Declarative vs Programmatic

Declarative (data attributes):
```html
<div data-bs-spy="scroll" data-bs-target="#navbar-example" data-bs-offset="50">
```

Programmatic (JavaScript):
```js
const spy = new bootstrap.Scrollspy(element, {
  target: '#navbar-example',
  offset: 50
});
```

## Advanced Variations

### Configuration Options

```js
const spy = new bootstrap.Scrollspy(document.body, {
  target: '#sidebar-nav',     // Selector for the nav element
  offset: 100,                // Pixel offset from top for activation
  rootMargin: '0px 0px -25%' // IntersectionObserver rootMargin
});
```

- `target` — required. Selector for the navigation element containing links to sections.
- `offset` — pixels to offset from the top of the viewport when determining which section is active.
- `rootMargin` — CSS `rootMargin` string passed to the IntersectionObserver for fine-grained control over activation thresholds.

### Activation Events

```js
document.querySelector('#navbar-example').addEventListener('activate.bs.scrollspy', (event) {
  // event.relatedTarget is the newly activated nav link
  const activeLink = event.relatedTarget;
  console.log('Activated:', activeLink.textContent);
  console.log('Target section:', activeLink.getAttribute('href'));

  // Update a breadcrumb or progress indicator
  document.getElementById('currentSection').textContent = activeLink.textContent;
});
```

### Refresh After Dynamic Content

When sections are added, removed, or resized dynamically, call `refresh()` to recalculate spy positions:

```js
const spy = bootstrap.Scrollspy.getInstance(document.body);

// Load new content dynamically
fetch('/api/sections')
  .then(res => res.json())
  .then(sections => {
    const container = document.getElementById('content');
    sections.forEach(section => {
      const div = document.createElement('div');
      div.id = section.id;
      div.innerHTML = `<h4>${section.title}</h4><p>${section.body}</p>`;
      container.appendChild(div);
    });

    // Update nav links
    const nav = document.getElementById('sidebar-nav');
    sections.forEach(section => {
      const li = document.createElement('li');
      li.className = 'nav-item';
      li.innerHTML = `<a class="nav-link" href="#${section.id}">${section.title}</a>`;
      nav.appendChild(li);
    });

    // Recalculate spy
    spy.refresh();
  });
```

### Instance Retrieval

```js
const existing = bootstrap.Scrollspy.getInstance(element);
const instance = bootstrap.Scrollspy.getOrCreateInstance(element);
```

## Best Practices

1. **Set `target` to a selector string**, not a DOM element — the API expects a CSS selector.
2. **Use `offset` to account for fixed headers** — set it to the height of any `position: fixed` or `sticky` elements above the scrollable area.
3. **Call `refresh()` after adding/removing sections** — Scrollspy does not observe DOM mutations automatically.
4. **Ensure scrollable element is correct** — Scrollspy attaches to either `document.body` or a specific overflow-scrollable container.
5. **Use `rootMargin` for fine-tuning** — `'0px 0px -50%'` activates sections only when they reach the upper half of the viewport.
6. **Make the scrollable container focusable** with `tabindex="0"` for keyboard accessibility.
7. **Use `activate.bs.scrollspy` event** to trigger secondary UI updates like breadcrumbs or progress bars.
8. **Prefer `position: sticky`** for the navigation sidebar so it stays visible while scrolling.
9. **Ensure section IDs match `href` values** in nav links exactly (including case sensitivity).
10. **Dispose the instance** if the navigation or scrollable area is removed from the DOM.
11. **Test with real content lengths** — short sections may never trigger activation if offset is too large.

## Common Pitfalls

1. **Missing `target` option** — Scrollspy cannot function without knowing which navigation to update.
2. **Body vs container scroll** — if Scrollspy is on `body`, the `<body>` must have scrollable content. If on a custom container, that container needs `overflow-y: scroll` and a defined `height`.
3. **Forgetting to call `refresh()`** — after dynamic content changes, the spy uses stale position calculations.
4. **Offset too large** — if `offset` exceeds section heights, some sections may never activate.
5. **ID mismatches** — nav `href="#section-a"` must match a `<div id="section-a">` exactly. Hash mismatches cause silent failures.
6. **Fixed header interference** — without setting `offset` to the header height, active sections are hidden behind the header.
7. **Not setting `tabindex` on the scrollable container** — keyboard users cannot navigate the scrollable region.

## Accessibility Considerations

- The scrollable container should have `tabindex="0"` so keyboard users can scroll through it.
- Navigation links must have clear visual indication of the active state (Bootstrap's `.active` class).
- Use `aria-current="true"` on the active nav link for screen reader announcement.
- Section headings should follow a logical heading hierarchy (`h2` > `h3` > `h4`).
- Ensure sufficient color contrast for active link indicators.
- The scrollable region should have an `aria-label` or `aria-labelledby` describing its purpose.

```html
<nav id="doc-nav" class="nav flex-column" aria-label="Document sections">
  <a class="nav-link active" href="#intro" aria-current="true">Introduction</a>
  <a class="nav-link" href="#getting-started">Getting Started</a>
  <a class="nav-link" href="#advanced">Advanced Topics</a>
</nav>
<div data-bs-spy="scroll" data-bs-target="#doc-nav"
     class="scrollspy-example" tabindex="0"
     aria-label="Documentation content" style="height: 400px; overflow-y: auto;">
  <h2 id="intro">Introduction</h2>
  <p>Content...</p>
  <h2 id="getting-started">Getting Started</h2>
  <p>Content...</p>
  <h2 id="advanced">Advanced Topics</h2>
  <p>Content...</p>
</div>
```

```js
const spy = new bootstrap.Scrollspy(scrollEl, {
  target: '#doc-nav',
  offset: 20
});

// Update aria-current on activation
document.querySelector('#doc-nav').addEventListener('activate.bs.scrollspy', (e) {
  document.querySelectorAll('#doc-nav .nav-link').forEach(link => {
    link.removeAttribute('aria-current');
  });
  e.relatedTarget.setAttribute('aria-current', 'true');
});
```

## Responsive Behavior

- On **small screens**, side-by-side nav and scrollspy content may not fit. Use a vertical stack with a collapsible navigation:

```html
<div class="row">
  <div class="col-md-3 d-none d-md-block">
    <nav id="sidebar-nav" class="sticky-top" aria-label="Sidebar navigation">
      <ul class="nav flex-column">
        <li class="nav-item"><a class="nav-link" href="#section1">Section 1</a></li>
        <li class="nav-item"><a class="nav-link" href="#section2">Section 2</a></li>
      </ul>
    </nav>
  </div>
  <div class="col-12 col-md-9">
    <div data-bs-spy="scroll" data-bs-target="#sidebar-nav"
         class="scrollspy-example" tabindex="0"
         style="height: 80vh; overflow-y: auto;">
      <!-- Sections -->
    </div>
  </div>
</div>
```

- Adjust `offset` dynamically based on viewport width:

```js
function updateOffset() {
  const headerHeight = document.querySelector('.navbar')?.offsetHeight || 0;
  const spy = bootstrap.Scrollspy.getInstance(scrollEl);
  if (spy) {
    spy.dispose();
  }
  new bootstrap.Scrollspy(scrollEl, {
    target: '#sidebar-nav',
    offset: headerHeight + 20
  });
}
window.addEventListener('resize', updateOffset);
```

- On mobile, consider converting the scrollspy nav into a **dropdown** or **floating action button** that scrolls to sections.
