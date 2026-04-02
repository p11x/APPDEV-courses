---
title: "ResizeObserver with Bootstrap"
slug: "resize-observer-bootstrap"
difficulty: 2
tags: ["bootstrap", "javascript", "resize-observer", "responsive", "container-queries"]
prerequisites:
  - "02_01_Grid_System"
  - "06_02_01_Intersection_Observer"
related:
  - "06_02_10_Performance_Observer"
  - "06_02_12_Broadcast_Channel"
duration: "25 minutes"
---

# ResizeObserver with Bootstrap

## Overview

ResizeObserver provides element-level size monitoring that goes beyond window resize events. While Bootstrap's grid handles viewport-based responsive design, ResizeObserver enables components to adapt based on their actual container size. This creates truly portable components that respond to their context rather than the page width. Use cases include dynamic chart resizing, adaptive navigation collapse, container-aware typography, and sidebar content that adjusts when panels resize.

## Basic Implementation

Monitor a Bootstrap card's dimensions and adjust its content layout accordingly.

```html
<div class="container mt-4">
  <div class="row">
    <div class="col-md-8">
      <div class="card" id="observedCard">
        <div class="card-body">
          <div class="d-flex align-items-center card-header-content">
            <i class="bi bi-bar-chart fs-4 me-2"></i>
            <h5 class="mb-0">Sales Overview</h5>
          </div>
          <p class="card-text mt-2" id="cardDescription">
            Detailed sales metrics for the current quarter with trend analysis.
          </p>
          <div id="cardSize" class="text-muted small"></div>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <button class="btn btn-outline-secondary mb-2" id="toggleSidebar">Toggle Sidebar</button>
    </div>
  </div>
</div>

<script>
const card = document.getElementById('observedCard');
const sizeDisplay = document.getElementById('cardSize');

const observer = new ResizeObserver(entries => {
  for (const entry of entries) {
    const { width, height } = entry.contentRect;
    sizeDisplay.textContent = `${Math.round(width)}px × ${Math.round(height)}px`;

    card.classList.remove('card-narrow', 'card-medium', 'card-wide');
    if (width < 300) {
      card.classList.add('card-narrow');
    } else if (width < 500) {
      card.classList.add('card-medium');
    } else {
      card.classList.add('card-wide');
    }
  }
});

observer.observe(card);
</script>

<style>
.card-narrow .card-header-content { flex-direction: column; text-align: center; }
.card-narrow .card-header-content i { margin-right: 0 !important; margin-bottom: 0.5rem; }
.card-wide #cardDescription { font-size: 1.1rem; }
</style>
```

## Advanced Variations

### Container-Aware Navigation

Automatically collapse navigation items based on available container width rather than viewport width.

```html
<div class="card">
  <div class="card-header p-0">
    <nav class="navbar navbar-expand" id="containerNav">
      <div class="container-fluid">
        <a class="navbar-brand" href="#">Brand</a>
        <ul class="navbar-nav" id="navItems">
          <li class="nav-item"><a class="nav-link active" href="#">Dashboard</a></li>
          <li class="nav-item"><a class="nav-link" href="#">Analytics</a></li>
          <li class="nav-item"><a class="nav-link" href="#">Reports</a></li>
          <li class="nav-item"><a class="nav-link" href="#">Settings</a></li>
          <li class="nav-item"><a class="nav-link" href="#">Users</a></li>
          <li class="nav-item"><a class="nav-link" href="#">Billing</a></li>
        </ul>
        <div class="dropdown d-none" id="overflowMenu">
          <button class="btn btn-sm btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
            More
          </button>
          <ul class="dropdown-menu dropdown-menu-end" id="overflowItems"></ul>
        </div>
      </div>
    </nav>
  </div>
</div>

<script>
const nav = document.getElementById('containerNav');
const navItems = document.getElementById('navItems');
const overflowMenu = document.getElementById('overflowMenu');
const overflowItems = document.getElementById('overflowItems');

const navObserver = new ResizeObserver(entries => {
  for (const entry of entries) {
    const navWidth = entry.contentRect.width;
    const brandWidth = nav.querySelector('.navbar-brand').offsetWidth;
    const available = navWidth - brandWidth - 100;
    const itemWidth = 100;
    const maxVisible = Math.max(1, Math.floor(available / itemWidth));

    const allItems = Array.from(navItems.querySelectorAll('.nav-item'));
    allItems.forEach((item, i) => {
      if (i < maxVisible) {
        item.classList.remove('d-none');
      } else {
        item.classList.add('d-none');
      }
    });

    const hidden = allItems.slice(maxVisible);
    if (hidden.length > 0) {
      overflowMenu.classList.remove('d-none');
      overflowItems.innerHTML = hidden.map(item =>
        `<li>${item.innerHTML.replace('nav-link', 'dropdown-item')}</li>`
      ).join('');
    } else {
      overflowMenu.classList.add('d-none');
    }
  }
});

navObserver.observe(nav);
</script>
```

### Dynamic Chart Resizing

Resize charts when their container changes size, independent of window resize.

```html
<div class="row">
  <div class="col-md-8">
    <div class="card">
      <div class="card-body">
        <canvas id="dynamicChart"></canvas>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card">
      <div class="card-body">
        <label class="form-label">Panel Width</label>
        <input type="range" class="form-range" id="widthSlider" min="25" max="100" value="66">
        <div class="text-muted small">Drag to resize the chart panel</div>
      </div>
    </div>
  </div>
</div>

<script>
const chartCanvas = document.getElementById('dynamicChart');
let chartInstance = null;

const chartObserver = new ResizeObserver(entries => {
  for (const entry of entries) {
    const { width, height } = entry.contentRect;
    if (chartInstance) {
      chartInstance.resize(width, height);
    }
  }
});

chartObserver.observe(chartCanvas.closest('.card'));

document.getElementById('widthSlider').addEventListener('input', (e) => {
  const col = chartCanvas.closest('.col-md-8');
  col.className = `col-md-${Math.round(e.target.value / 100 * 12)}`;
});
</script>
```

### Responsive Font Sizing

Scale typography based on card container width using CSS custom properties.

```html
<div class="card" id="typographyCard">
  <div class="card-body">
    <h2 class="responsive-heading">Adaptive Heading</h2>
    <p class="responsive-body">
      This text scales based on the card container width, not the viewport.
    </p>
  </div>
</div>

<script>
const typeCard = document.getElementById('typographyCard');

const typeObserver = new ResizeObserver(entries => {
  for (const entry of entries) {
    const width = entry.contentRect.width;
    const scale = Math.max(0.8, Math.min(1.5, width / 400));
    typeCard.style.setProperty('--container-scale', scale);
  }
});

typeObserver.observe(typeCard);
</script>

<style>
.responsive-heading { font-size: calc(1.5rem * var(--container-scale, 1)); }
.responsive-body { font-size: calc(1rem * var(--container-scale, 1)); }
</style>
```

## Best Practices

1. Disconnect observers when components are removed from the DOM to prevent memory leaks
2. Use `requestAnimationFrame` inside callbacks to batch visual updates with browser repaints
3. Debounce rapid resize events if the handler performs expensive operations
4. Use a single observer for multiple elements when they share the same resize logic
5. Set `box-sizing: border-box` globally to ensure `contentRect` matches visual expectations
6. Test with browser DevTools responsive mode to verify container-based breakpoints
7. Use ResizeObserver for container queries polyfill behavior in older browsers
8. Avoid observing elements that are hidden (`display: none`) as they report zero dimensions
9. Store previous dimensions and only trigger updates when size actually changes
10. Combine with `ResizeObserver.unobserve()` for one-time size measurements
11. Use `devicePixelContentBox` for sub-pixel accurate measurements when available
12. Apply CSS transitions to prevent jarring layout changes during resize
13. Log resize frequency during development to identify performance bottlenecks
14. Prefer ResizeObserver over window resize listeners for component-level responsiveness

## Common Pitfalls

1. **Memory leaks**: Failing to call `observer.disconnect()` on component teardown
2. **Infinite loops**: Modifying element size inside the resize callback triggers another resize event
3. **Zero dimensions**: Observing elements before they are rendered or while hidden
4. **Performance**: Observing too many elements simultaneously without throttling
5. **Box model confusion**: Forgetting that `contentRect` excludes padding and borders by default
6. **Browser support**: Not providing fallback for environments that lack ResizeObserver
7. **Janky animations**: Performing heavy DOM manipulation inside resize callbacks

## Accessibility Considerations

Ensure container-adaptive layouts maintain readable font sizes (minimum 16px for body text). Preserve focus indicators when navigation items move to overflow menus during resize. Use `aria-expanded` on overflow menu toggles. Avoid removing interactive elements from the tab order when they are hidden due to container width. Announce significant layout changes with `aria-live` regions only when they fundamentally alter the page structure. Maintain consistent keyboard navigation patterns regardless of container-driven layout state.

```html
<div id="overflowMenu" aria-label="Additional navigation items" role="navigation">
  <button class="dropdown-toggle" aria-expanded="false" aria-haspopup="menu">
    More options
  </button>
</div>
```

## Responsive Behavior

ResizeObserver complements Bootstrap's viewport-based grid by providing element-level awareness. On mobile, most cards occupy full width, so ResizeObserver primarily adjusts content density. On desktop, sidebars and split panels benefit from container-aware component adjustments. Use `col-*` classes for viewport breakpoints and ResizeObserver for fine-tuning within those breakpoints. When panels are resizable (via drag handles), ResizeObserver detects the manual resize and adapts content accordingly. Combine with Bootstrap's `offcanvas` for mobile navigation that respects container changes when opened.
