---
title: "Render Performance"
difficulty: 3
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - Browser Rendering Pipeline
  - CSS Containment
  - requestAnimationFrame
---

## Overview

Render performance optimization focuses on minimizing layout thrashing, reducing paint operations, and leveraging composite layers in Bootstrap-based applications. The browser rendering pipeline has four stages: style calculation, layout, paint, and composite. Expensive operations at any stage cause jank, particularly during animations, scrolling, and dynamic content updates.

Bootstrap components like carousels, modals, and dropdowns trigger reflows when not implemented carefully. Layout thrashing occurs when JavaScript reads layout properties (offsetHeight, getBoundingClientRect) and then writes styles in a loop, forcing the browser to recalculate layout repeatedly. Understanding and avoiding these patterns is critical for smooth 60fps interactions.

## Basic Implementation

```js
// BAD: Layout thrashing
function resizeCards() {
  const cards = document.querySelectorAll('.card');
  cards.forEach(card => {
    const height = card.offsetHeight; // READ (forces layout)
    card.style.minHeight = (height + 20) + 'px'; // WRITE (invalidates layout)
  });
}

// GOOD: Batched reads and writes
function resizeCardsOptimized() {
  const cards = document.querySelectorAll('.card');

  // Batch all reads
  const heights = Array.from(cards).map(card => card.offsetHeight);

  // Batch all writes
  requestAnimationFrame(() => {
    cards.forEach((card, i) => {
      card.style.minHeight = (heights[i] + 20) + 'px';
    });
  });
}
```

```scss
// CSS containment for Bootstrap cards
.card {
  // Tell browser this element is self-contained
  contain: layout style paint;
  // Or use stricter containment
  // contain: strict; /* layout style paint size */
}

// GPU-accelerated animations
.modal.fade .modal-dialog {
  // Use transform instead of margin-top for slide animation
  transform: translateY(-50px);
  transition: transform 0.3s ease-out;
  will-change: transform; /* hint to promote to GPU layer */
}

.modal.show .modal-dialog {
  transform: translateY(0);
}

// Only animate compositor-friendly properties
.carousel-item {
  transition: transform 0.6s ease-in-out; /* composite only */
  // Avoid: transition: left 0.6s; /* triggers layout + paint */
}
```

```js
// Efficient scroll handler with requestAnimationFrame
class PerformantScroller {
  constructor(element) {
    this.element = element;
    this.ticking = false;
    this.lastScrollY = 0;

    window.addEventListener('scroll', () => this.onScroll(), { passive: true });
  }

  onScroll() {
    this.lastScrollY = window.scrollY;

    if (!this.ticking) {
      requestAnimationFrame(() => {
        this.update(this.lastScrollY);
        this.ticking = false;
      });
      this.ticking = true;
    }
  }

  update(scrollY) {
    // Only use transform and opacity here
    // These are composite-only properties
    this.element.style.transform = `translateY(${scrollY * 0.5}px)`;
  }
}
```

## Advanced Variations

```js
// Virtual scrolling for large lists in Bootstrap table
class VirtualTable {
  constructor(container, options = {}) {
    this.container = container;
    this.rowHeight = options.rowHeight || 48;
    this.buffer = options.buffer || 5;
    this.data = [];

    this.viewport = document.createElement('div');
    this.viewport.style.overflow = 'auto';
    this.viewport.style.height = options.height || '400px';
    this.viewport.addEventListener('scroll', () => this.onScroll(), { passive: true });

    this.content = document.createElement('div');
    this.content.style.position = 'relative';

    this.viewport.appendChild(this.content);
    container.appendChild(this.viewport);
  }

  setData(data) {
    this.data = data;
    this.content.style.height = `${data.length * this.rowHeight}px`;
    this.render();
  }

  onScroll() {
    if (!this._scrollFrame) {
      this._scrollFrame = requestAnimationFrame(() => {
        this.render();
        this._scrollFrame = null;
      });
    }
  }

  render() {
    const scrollTop = this.viewport.scrollTop;
    const viewportHeight = this.viewport.clientHeight;

    const startIndex = Math.max(0, Math.floor(scrollTop / this.rowHeight) - this.buffer);
    const endIndex = Math.min(this.data.length, Math.ceil((scrollTop + viewportHeight) / this.rowHeight) + this.buffer);

    const visibleData = this.data.slice(startIndex, endIndex);

    this.content.innerHTML = `
      <table class="table table-hover mb-0">
        <thead class="table-light" style="position: sticky; top: 0; z-index: 1;">
          <tr><th>Name</th><th>Email</th><th>Status</th></tr>
        </thead>
        <tbody>
          ${visibleData.map((row, i) => `
            <tr style="transform: translateY(${(startIndex + i) * this.rowHeight}px);">
              <td>${row.name}</td>
              <td>${row.email}</td>
              <td><span class="badge bg-${row.status === 'active' ? 'success' : 'secondary'}">${row.status}</span></td>
            </tr>
          `).join('')}
        </tbody>
      </table>
    `;
  }
}
```

## Best Practices

1. **Batch DOM reads and writes** - Read all layout properties first, then write all style changes.
2. **Use requestAnimationFrame** - Schedule visual updates in the next frame to avoid layout thrashing.
3. **Prefer transform and opacity** - These are the only properties that only trigger composite, not layout or paint.
4. **Apply CSS containment** - Use `contain: layout paint style` on self-contained components.
5. **Use will-change sparingly** - Only on elements that will animate; overuse wastes GPU memory.
6. **Passive event listeners** - Use `{ passive: true }` on scroll and touch handlers.
7. **Debounce resize handlers** - Don't recalculate on every resize event.
8. **Use Intersection Observer** - Replace scroll-based visibility checks with Intersection Observer.
9. **Avoid forced synchronous layouts** - Never read layout properties after writing styles in the same frame.
10. **Profile with DevTools** - Use the Performance tab to identify actual bottlenecks.

## Common Pitfalls

1. **Animating layout properties** - Animating width, height, margin, padding triggers layout on every frame.
2. **Reading layout in loops** - `offsetHeight` inside a loop forces repeated layouts.
3. **Missing passive listeners** - Synchronous scroll handlers block scrolling on mobile.
4. **Overusing will-change** - Promoting too many elements to GPU layers consumes memory.
5. **Not using contain** - Without containment, one card change can reflow the entire page.

## Accessibility Considerations

Animations must respect `prefers-reduced-motion`. Composite-only animations are acceptable but layout-triggering animations should be disabled.

```scss
@media (prefers-reduced-motion: reduce) {
  .modal.fade .modal-dialog {
    transition: none;
    transform: none;
  }
  .carousel-item {
    transition: none;
  }
}
```

## Responsive Behavior

Virtual scrolling row heights should adapt to viewport size. Mobile viewports need larger touch targets, increasing effective row height.

```js
const rowHeight = window.matchMedia('(min-width: 768px)').matches ? 48 : 64;
```
