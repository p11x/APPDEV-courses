---
title: "JavaScript Profiling for Bootstrap Plugins"
description: "Profiling Bootstrap JS plugins, identifying performance bottlenecks, and analyzing memory usage"
difficulty: 3
tags: ["performance", "profiling", "javascript", "devtools", "bootstrap"]
prerequisites: ["04_07_01_Lighthouse_Audit", "04_07_02_Core_Web_Vitals"]
---

## Overview

Bootstrap's JavaScript plugins — modal, dropdown, carousel, collapse, and toast — attach event listeners, manipulate the DOM, and manage internal state. On complex pages with many components, these plugins can cause long tasks that block the main thread and degrade interactivity. Profiling reveals which plugins consume the most CPU, trigger excessive reflows, or leak memory.

Chrome DevTools Performance and Memory panels provide flame charts, allocation timelines, and heap snapshots that expose bottlenecks in Bootstrap plugin initialization, event handling, and teardown.

## Basic Implementation

```html
<!-- Page with multiple Bootstrap components to profile -->
<body>
  <div class="container py-4">
    <!-- 20 modals trigger heavy initialization -->
    <div class="row g-3">
      <div class="col-md-4" id="card-container"></div>
    </div>

    <!-- Each modal initializes a Modal plugin instance -->
    <div id="modal-container"></div>
  </div>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
  <script>
    // Generate 20 cards with modals for profiling
    const cardContainer = document.getElementById('card-container');
    const modalContainer = document.getElementById('modal-container');

    for (let i = 0; i < 20; i++) {
      cardContainer.innerHTML += `
        <div class="card">
          <div class="card-body">
            <h5>Card ${i}</h5>
            <button class="btn btn-primary" data-bs-toggle="modal"
                    data-bs-target="#modal-${i}">Open</button>
          </div>
        </div>`;

      modalContainer.innerHTML += `
        <div class="modal fade" id="modal-${i}" tabindex="-1">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <h5>Modal ${i}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
              </div>
              <div class="modal-body">Content for modal ${i}.</div>
            </div>
          </div>
        </div>`;
    }
  </script>
</body>
```

```js
// Recording a performance profile programmatically
// 1. Open Chrome DevTools > Performance tab
// 2. Click Record (Ctrl+Shift+E)
// 3. Interact with Bootstrap components (open modals, click dropdowns)
// 4. Stop recording

// Identifying long tasks from Bootstrap initialization
const observer = new PerformanceObserver((list) => {
  for (const entry of list.getEntries()) {
    if (entry.duration > 50) {
      console.warn(`Long task: ${entry.name} (${entry.duration.toFixed(1)}ms)`);
      // Bootstrap modal initialization often creates 50ms+ tasks
    }
  }
});
observer.observe({ entryTypes: ['longtask'] });
```

## Advanced Variations

```js
// Memory profiling: Detecting leaked Bootstrap instances
function auditBootstrapInstances() {
  const instances = {
    modals: document.querySelectorAll('.modal').length,
    tooltips: document.querySelectorAll('[data-bs-toggle="tooltip"]').length,
    dropdowns: document.querySelectorAll('[data-bs-toggle="dropdown"]').length,
    carousels: document.querySelectorAll('.carousel').length
  };

  // Check for orphaned backdrop elements (memory leak indicator)
  const backdrops = document.querySelectorAll('.modal-backdrop');
  const tooltips = document.querySelectorAll('.tooltip, .popover');

  console.table({
    ...instances,
    orphanedBackdrops: backdrops.length,
    orphanedTooltips: tooltips.length
  });

  // Bootstrap stores instances on elements — check for accumulation
  const modalElements = document.querySelectorAll('.modal');
  modalElements.forEach((el, i) => {
    const bsInstance = bootstrap.Modal.getInstance(el);
    if (!bsInstance) {
      console.warn(`Modal #${i} has no Bootstrap instance — may be leaked`);
    }
  });
}

// Run after heavy component interaction
setInterval(auditBootstrapInstances, 30000);
```

```js
// Profiling Bootstrap event handler performance
function profileBootstrapEvents() {
  const events = ['show.bs.modal', 'shown.bs.modal', 'hide.bs.modal',
                  'hidden.bs.modal', 'show.bs.dropdown', 'shown.bs.dropdown'];

  events.forEach(eventName => {
    document.addEventListener(eventName, (e) => {
      performance.mark(`${eventName}-start`);

      // Use requestAnimationFrame to measure render cost
      requestAnimationFrame(() => {
        performance.mark(`${eventName}-end`);
        performance.measure(eventName, `${eventName}-start`, `${eventName}-end`);

        const measure = performance.getEntriesByName(eventName)[0];
        if (measure.duration > 16.67) {
          console.warn(`${eventName} took ${measure.duration.toFixed(2)}ms (exceeds 60fps frame budget)`);
        }
      });
    });
  });
}

profileBootstrapEvents();
```

## Best Practices

1. Profile with CPU throttling enabled (4x or 6x slowdown) to simulate mobile devices
2. Record performance profiles before and after optimization to measure improvement
3. Use the Memory panel's Allocation instrumentation to track Bootstrap instance creation
4. Check for detached DOM trees from improperly destroyed Bootstrap modals
5. Profile both initialization (page load) and interaction (opening/closing components)
6. Use `PerformanceObserver` for long task detection in production
7. Lazy-initialize Bootstrap plugins on components below the fold
8. Destroy Bootstrap instances when removing components from the DOM
9. Avoid inline event handlers that duplicate Bootstrap plugin behavior
10. Bundle and tree-shake Bootstrap JS to import only needed plugins

## Common Pitfalls

1. **Not destroying modal instances on SPA navigation** — Single-page apps that remove modal DOM without calling `.dispose()` leak event listeners and memory
2. **Initializing tooltips on hundreds of elements** — Each tooltip registers mouseenter/mouseleave listeners; batch-init or use event delegation instead
3. **Ignoring long tasks from carousel auto-play** — Carousel intervals can create long tasks during slide transitions with heavy images
4. **Profiling without throttling** — Desktop performance profiles mask mobile bottlenecks; always throttle CPU and network
5. **Stacking multiple event listeners** — Adding custom click handlers alongside Bootstrap's plugin handlers doubles the work; use Bootstrap events instead
6. **Missing Memory panel heap snapshots** — CPU profiles alone don't reveal memory leaks from abandoned Bootstrap instances

## Accessibility Considerations

Profiling can reveal accessibility-related performance issues. Bootstrap modals with many focusable elements increase focus-trap computation time. Dropdowns with long item lists cause layout thrashing during keyboard navigation. Use profiling to ensure focus management does not exceed the 16.67ms frame budget, which causes visible jank for keyboard users.

## Responsive Behavior

Bootstrap components behave differently across viewports, affecting profiling results. Mobile viewports trigger `collapse` for navbar toggles, adding JS overhead. Carousel swipe handlers add touch event listeners on mobile. Profile at both desktop (1920px) and mobile (375px) widths to capture the full performance picture across Bootstrap's responsive breakpoints.
