---
title: "Lazy Loading Components"
module: "Performance Optimization"
difficulty: 3
duration: "35 minutes"
prerequisites: ["JavaScript modules", "Intersection Observer", "Dynamic imports"]
tags: ["lazy-loading", "dynamic-imports", "intersection-observer", "performance"]
---

# Lazy Loading Components

## Overview

Lazy loading defers the initialization of non-critical Bootstrap components until they're needed, reducing initial JavaScript execution time and improving Time to Interactive. Using Intersection Observer, dynamic imports, and progressive loading strategies, you can load modals, carousels, and offcanvas components only when users are about to interact with them.

## Basic Implementation

Lazy load modals using dynamic imports:

```html
<!-- Modal trigger - modal HTML exists, JS loads on demand -->
<button type="button"
        class="btn btn-primary"
        data-bs-toggle="modal"
        data-bs-target="#exampleModal"
        data-lazy-modal>
  Launch Modal
</button>

<div class="modal fade" id="exampleModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Lazy Loaded Modal</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <p>This modal's JavaScript loads only when opened.</p>
      </div>
    </div>
  </div>
</div>
```

```js
// lazy-modals.js
document.querySelectorAll('[data-lazy-modal]').forEach(trigger => {
  trigger.addEventListener('click', async () => {
    const { Modal } = await import('bootstrap/js/dist/modal');
    const target = document.querySelector(trigger.dataset.bsTarget);
    new Modal(target).show();
  }, { once: true });
});
```

## Advanced Variations

Use Intersection Observer for viewport-based component loading:

```js
// lazy-carousel.js
const lazyComponents = new Map([
  ['.carousel', () => import('bootstrap/js/dist/carousel')],
  ['[data-bs-spy="scroll"]', () => import('bootstrap/js/dist/scrollspy')],
  ['.accordion', () => import('bootstrap/js/dist/collapse')]
]);

const observer = new IntersectionObserver((entries) => {
  entries.forEach(async (entry) => {
    if (entry.isIntersecting) {
      const element = entry.target;
      const selector = [...lazyComponents.keys()].find(
        s => element.matches(s)
      );

      if (selector) {
        const loader = lazyComponents.get(selector);
        const module = await loader();
        const ComponentClass = Object.values(module)[0];
        new ComponentClass(element);
        observer.unobserve(element);
      }
    }
  });
}, { rootMargin: '100px' }); // Load 100px before visible

document.querySelectorAll([...lazyComponents.keys()].join(',')).forEach(el => {
  observer.observe(el);
});
```

Implement progressive component loading with placeholders:

```html
<!-- Carousel with skeleton placeholder -->
<div class="carousel-container" data-lazy-component="carousel">
  <div class="skeleton-placeholder">
    <div class="skeleton-slide bg-light rounded"
         style="height: 400px; animation: pulse 1.5s infinite;">
    </div>
  </div>

  <div class="carousel visually-hidden" id="heroCarousel">
    <div class="carousel-inner">
      <div class="carousel-item active">
        <img src="slide1.jpg" class="d-block w-100" alt="Slide 1" loading="lazy">
      </div>
      <div class="carousel-item">
        <img src="slide2.jpg" class="d-block w-100" alt="Slide 2" loading="lazy">
      </div>
    </div>
    <button class="carousel-control-prev" data-bs-target="#heroCarousel"
            data-bs-slide="prev">
      <span class="carousel-control-prev-icon"></span>
    </button>
  </div>
</div>
```

```css
@keyframes pulse {
  0%, 100% { opacity: 0.6; }
  50% { opacity: 1; }
}

.skeleton-placeholder {
  transition: opacity 0.3s ease;
}

.carousel-container[data-loaded] .skeleton-placeholder {
  display: none;
}

.carousel-container[data-loaded] .carousel {
  visibility: visible;
}
```

```js
// Progressive loader with skeleton replacement
class ComponentLoader {
  constructor() {
    this.observer = new IntersectionObserver(this.handleIntersection.bind(this), {
      rootMargin: '200px'
    });
  }

  observe(elements) {
    elements.forEach(el => this.observer.observe(el));
  }

  async handleIntersection(entries) {
    for (const entry of entries) {
      if (!entry.isIntersecting) continue;

      const el = entry.target;
      const type = el.dataset.lazyComponent;

      await this.loadComponent(type, el);
      this.observer.unobserve(el);
    }
  }

  async loadComponent(type, container) {
    const loaders = {
      carousel: () => import('bootstrap/js/dist/carousel'),
      offcanvas: () => import('bootstrap/js/dist/offcanvas'),
      tooltip: () => import('bootstrap/js/dist/tooltip'),
      popover: () => import('bootstrap/js/dist/popover')
    };

    if (loaders[type]) {
      await loaders[type]();
      container.dataset.loaded = 'true';
      container.querySelector('.carousel')?.classList.remove('visually-hidden');
    }
  }
}

const loader = new ComponentLoader();
loader.observe(document.querySelectorAll('[data-lazy-component]'));
```

Create a dynamic route-based loader:

```js
// route-loader.js - Load components per page/route
const routeComponents = {
  '/dashboard': () => import('./components/dashboard-components'),
  '/products': () => import('./components/product-components'),
  '/checkout': () => import('./components/checkout-components')
};

async function loadRouteComponents(path) {
  const loader = routeComponents[path];
  if (loader) {
    const module = await loader();
    module.init(); // Initialize route-specific components
  }
}

// Usage with SPA router
window.addEventListener('popstate', () => {
  loadRouteComponents(window.location.pathname);
});

// Initial load
loadRouteComponents(window.location.pathname);
```

## Best Practices

1. Lazy load components below the fold first
2. Use Intersection Observer with appropriate `rootMargin`
3. Provide skeleton placeholders during loading
4. Use `{ once: true }` for single-fire lazy load triggers
5. Preload components on user intent (hover, focus)
6. Bundle lazy-loaded chunks separately
7. Test lazy loading with slow network throttling
8. Provide fallback content when JavaScript fails
9. Monitor chunk sizes for lazy-loaded components
10. Use `requestIdleCallback` for non-critical components
11. Cache lazy-loaded modules in service worker
12. Track lazy loading performance metrics

## Common Pitfalls

1. Lazy loading above-the-fold components (delays interaction)
2. Missing skeleton placeholder causing layout shift
3. Not handling network failures for lazy imports
4. Creating too many small chunks (request overhead)
5. Not testing with JavaScript disabled
6. Lazy loading components with required initial state
7. Intersection Observer not triggering on scroll containers
8. Memory leaks from observers not being disconnected

## Accessibility Considerations

- Announce component loading state to screen readers
- Ensure skeleton placeholders have proper ARIA attributes
- Maintain focus order after lazy-loaded component renders
- Test keyboard navigation with dynamically loaded components
- Provide loading announcements via `aria-live` regions

## Responsive Behavior

- Test lazy loading at all viewport sizes
- Adjust `rootMargin` for different screen heights
- Verify mobile touch events trigger lazy loading
- Ensure lazy-loaded components render correctly on small screens
