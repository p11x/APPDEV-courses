---
title: Island Architecture
category: [Future Bootstrap, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, island-architecture, partial-hydration, selective-js, performance
---

## Overview

Island architecture hydrates only interactive components ("islands") while keeping the rest of the page as static HTML. Applied to Bootstrap, this means loading JavaScript only for modals, dropdowns, and carousels that need interactivity, dramatically reducing bundle size and improving initial page load.

## Basic Implementation

Structuring a page with interactive islands and static Bootstrap markup.

```html
<body>
  <!-- Static: No JS needed -->
  <nav class="navbar navbar-expand-lg bg-body-tertiary">
    <div class="container">
      <a class="navbar-brand" href="/">MyApp</a>
      <div class="collapse navbar-collapse">
        <ul class="navbar-nav">
          <li class="nav-item"><a class="nav-link" href="/">Home</a></li>
          <li class="nav-item"><a class="nav-link" href="/about">About</a></li>
        </ul>
      </div>
    </div>
  </nav>

  <!-- Static content: pure HTML, zero JS -->
  <main class="container py-5">
    <div class="row">
      <div class="col-lg-8">
        <h1>Article Title</h1>
        <p class="lead">Static content that needs no JavaScript.</p>
      </div>
      <div class="col-lg-4">
        <!-- Island: Only this component gets JS -->
        <div data-island="dropdown" data-hydrate="visible">
          <div class="dropdown">
            <button class="btn btn-outline-primary dropdown-toggle w-100"
                    data-bs-toggle="dropdown">
              Filter by Category
            </button>
            <ul class="dropdown-menu w-100">
              <li><a class="dropdown-item" href="#">Technology</a></li>
              <li><a class="dropdown-item" href="#">Design</a></li>
              <li><a class="dropdown-item" href="#">Business</a></li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </main>

  <!-- Another island: modal only loads when triggered -->
  <div data-island="modal" data-hydrate="interaction">
    <button class="btn btn-primary" data-bs-toggle="modal" data-bs-contactModal>
      Contact Us
    </button>
    <div class="modal fade" id="contactModal" tabindex="-1">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Contact</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <form>
              <div class="mb-3">
                <label class="form-label">Email</label>
                <input type="email" class="form-control">
              </div>
              <button class="btn btn-primary" type="submit">Send</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</body>
```

## Advanced Variations

Implementing an island loader that hydrates components based on visibility or interaction.

```html
<script>
class IslandLoader {
  constructor() {
    this.loaded = new Set();
    this._init();
  }

  _init() {
    const islands = document.querySelectorAll('[data-island]');

    islands.forEach(island => {
      const strategy = island.dataset.hydrate || 'immediate';

      if (strategy === 'visible') {
        this._hydrateOnVisible(island);
      } else if (strategy === 'interaction') {
        this._hydrateOnInteraction(island);
      } else {
        this._hydrate(island);
      }
    });
  }

  _hydrateOnVisible(island) {
    const observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting) {
        this._hydrate(island);
        observer.disconnect();
      }
    }, { rootMargin: '200px' });
    observer.observe(island);
  }

  _hydrateOnInteraction(island) {
    island.addEventListener('pointerenter', () => this._hydrate(island), { once: true });
    island.addEventListener('focusin', () => this._hydrate(island), { once: true });
  }

  async _hydrate(island) {
    const type = island.dataset.island;
    if (this.loaded.has(type)) return;

    if (type === 'modal') {
      await import('./bootstrap-modal.js');
    } else if (type === 'dropdown') {
      await import('./bootstrap-dropdown.js');
    } else if (type === 'carousel') {
      await import('./bootstrap-carousel.js');
    }

    this.loaded.add(type);
  }
}

new IslandLoader();
</script>
```

Selective carousel loading with progressive enhancement.

```html
<div data-island="carousel" data-hydrate="visible">
  <div id="heroCarousel" class="carousel slide">
    <div class="carousel-inner">
      <div class="carousel-item active">
        <div class="bg-primary text-white p-5 text-center">
          <h2>Slide 1</h2>
        </div>
      </div>
      <div class="carousel-item">
        <div class="bg-success text-white p-5 text-center">
          <h2>Slide 2</h2>
        </div>
      </div>
    </div>
    <!-- Fallback: static prev/next that work without JS -->
    <a href="#heroCarousel" class="carousel-control-prev" role="button">
      <span class="carousel-control-prev-icon"></span>
    </a>
    <a href="#heroCarousel" class="carousel-control-next" role="button">
      <span class="carousel-control-next-icon"></span>
    </a>
  </div>
</div>
```

## Best Practices

1. Use `data-island` attributes to mark interactive component boundaries
2. Implement hydration strategies: `immediate`, `visible`, `interaction`, `idle`
3. Use `IntersectionObserver` for viewport-based lazy hydration
4. Load Bootstrap JS modules dynamically with `import()` for code splitting
5. Provide static HTML fallbacks for all interactive components
6. Keep the majority of the page as pure HTML without JS dependency
7. Use `<link rel="modulepreload">` for likely-needed island scripts
8. Track loaded islands to prevent duplicate hydration
9. Defer non-critical islands until `requestIdleCallback`
10. Measure and report island hydration timing for performance budgets

## Common Pitfalls

1. **Flash of static content** — Users see unstyled components before hydration completes
2. **Hydration race conditions** — Multiple triggers hydrating the same island simultaneously
3. **Missing fallbacks** — Static HTML without JS leaves users with broken interactive elements
4. **Over-islanding** — Too many small islands increase HTTP request overhead
5. **Event listener duplication** — Hydrating an already-initialized component binds events twice
6. **No SSR support** — Pure client-side island loading defeats server rendering benefits
7. **Bundle splitting complexity** — Dynamic imports require careful chunk management
8. **CLS from hydration** — Layout shifts when JS modifies static component dimensions

## Accessibility Considerations

Ensure static fallback HTML is keyboard navigable before hydration. Provide visible focus indicators on fallback controls. Use `aria-live` to announce when interactive components become available. Maintain semantic HTML structure in island boundaries so screen readers navigate correctly even before JS loads.

## Responsive Behavior

Island components should render acceptably without JavaScript at all viewport sizes. Use Bootstrap's responsive grid to lay out island containers. Consider different hydration strategies per breakpoint — `visible` for above-fold, `idle` for below-fold on mobile with slower connections.
