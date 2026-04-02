---
title: Intersection Observer with Bootstrap
category: Emerging Technologies
difficulty: 2
time: 25 min
tags: bootstrap5, intersection-observer, scroll-animations, lazy-loading, infinite-scroll
---

## Overview

The Intersection Observer API enables performant scroll-driven behaviors without the performance cost of scroll event listeners. Combined with Bootstrap's utility classes and components, it powers scroll-triggered animations, lazy-loaded images, infinite scroll pagination, and sticky element activation. The API observes when elements enter or exit the viewport and triggers callbacks accordingly.

## Basic Implementation

Scroll-triggered fade-in animations using Intersection Observer with Bootstrap's visibility utilities.

```js
// Scroll-triggered fade-in animation
const observerOptions = {
  root: null,
  rootMargin: '0px',
  threshold: 0.15
};

const fadeInObserver = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.classList.add('animate__animated', 'animate__fadeInUp');
      entry.target.classList.remove('opacity-0');
      fadeInObserver.unobserve(entry.target); // animate once
    }
  });
}, observerOptions);

// Observe all elements with data-animate attribute
document.querySelectorAll('[data-animate]').forEach(el => {
  el.classList.add('opacity-0');
  fadeInObserver.observe(el);
});
```

```html
<!-- Cards that animate on scroll -->
<div class="container py-5">
  <div class="row g-4">
    <div class="col-md-4" data-animate>
      <div class="card shadow-sm h-100">
        <div class="card-body">
          <h5 class="card-title">Feature One</h5>
          <p class="card-text">This card fades in when scrolled into view.</p>
        </div>
      </div>
    </div>
    <div class="col-md-4" data-animate>
      <div class="card shadow-sm h-100">
        <div class="card-body">
          <h5 class="card-title">Feature Two</h5>
          <p class="card-text">Each card animates independently.</p>
        </div>
      </div>
    </div>
    <div class="col-md-4" data-animate>
      <div class="card shadow-sm h-100">
        <div class="card-body">
          <h5 class="card-title">Feature Three</h5>
          <p class="card-text">Animation triggers only once.</p>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

Infinite scroll with Bootstrap pagination and lazy image loading for data-heavy pages.

```js
// Infinite scroll implementation
class InfiniteScroll {
  constructor(containerSelector, loadMoreCallback, options = {}) {
    this.container = document.querySelector(containerSelector);
    this.loadMore = loadMoreCallback;
    this.loading = false;
    this.page = 1;
    this.hasMore = true;

    this.sentinel = document.createElement('div');
    this.sentinel.className = 'infinite-scroll-sentinel';
    this.container.after(this.sentinel);

    this.observer = new IntersectionObserver(
      (entries) => this.handleIntersection(entries),
      { rootMargin: options.rootMargin || '200px', threshold: 0 }
    );
    this.observer.observe(this.sentinel);
  }

  async handleIntersection(entries) {
    if (!entries[0].isIntersecting || this.loading || !this.hasMore) return;

    this.loading = true;
    this.showSpinner();

    try {
      const result = await this.loadMore(++this.page);
      this.hasMore = result.hasMore;
      this.appendItems(result.items);
    } catch (error) {
      this.showError(error);
    } finally {
      this.loading = false;
      this.hideSpinner();
    }
  }

  showSpinner() {
    const spinner = document.createElement('div');
    spinner.id = 'scrollSpinner';
    spinner.className = 'text-center py-3';
    spinner.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading more...</span></div>';
    this.sentinel.before(spinner);
  }

  hideSpinner() {
    document.getElementById('scrollSpinner')?.remove();
  }

  appendItems(items) {
    const html = items.map(item => `
      <div class="col">
        <div class="card h-100 shadow-sm">
          <div class="card-body">
            <h5 class="card-title">${item.title}</h5>
            <p class="card-text">${item.description}</p>
          </div>
        </div>
      </div>
    `).join('');
    this.container.querySelector('.row').insertAdjacentHTML('beforeend', html);
  }
}

// Usage
const scroller = new InfiniteScroll('#itemContainer', async (page) => {
  const res = await fetch(`/api/items?page=${page}&limit=12`);
  return res.json();
});
```

```js
// Lazy image loading with placeholder
const lazyImageObserver = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      const img = entry.target;
      img.src = img.dataset.src;
      img.classList.remove('placeholder-glow');
      img.addEventListener('load', () => {
        img.classList.add('img-fluid');
        img.classList.remove('placeholder');
      });
      lazyImageObserver.unobserve(img);
    }
  });
}, { rootMargin: '100px' });

document.querySelectorAll('img[data-src]').forEach(img => {
  img.classList.add('placeholder', 'placeholder-glow');
  lazyImageObserver.observe(img);
});
```

```html
<!-- Lazy-loaded image grid -->
<div class="row g-3">
  <div class="col-6 col-md-4">
    <img data-src="photo-1.jpg" class="img-fluid rounded" alt="Photo 1"
         width="400" height="300">
  </div>
  <div class="col-6 col-md-4">
    <img data-src="photo-2.jpg" class="img-fluid rounded" alt="Photo 2"
         width="400" height="300">
  </div>
</div>
```

## Best Practices

1. Use `threshold: 0.15` for animation triggers to avoid premature firing
2. Set `rootMargin` to preload content before it enters the viewport
3. Unobserve elements after one-time animations to save resources
4. Use a single observer instance for multiple similar elements
5. Implement loading spinners with Bootstrap's `.spinner-border` class
6. Add `data-src` attributes for lazy image loading patterns
7. Handle the case where IntersectionObserver is not supported with a polyfill
8. Use `root: scrollContainer` for observers within scrollable containers, not just the viewport
9. Debounce rapid observer callbacks for expensive operations
10. Clean up observers with `disconnect()` when components unmount

## Common Pitfalls

1. **Observer never fires** - Elements already in the viewport won't trigger with `threshold: 0`. Use `threshold: 0` for initial detection.
2. **Memory leaks** - Observers not disconnected on page navigation accumulate. Always clean up.
3. **Race conditions in infinite scroll** - Multiple rapid triggers load duplicate content. Use a loading flag to prevent concurrent requests.
4. **Animation re-triggering** - Without `unobserve()`, animations replay on every scroll. Animate once and disconnect.
5. **Root margin units** - `rootMargin` uses CSS margin syntax (`'100px'` not `100`). Use string values with units.

## Accessibility Considerations

Scroll animations should respect `prefers-reduced-motion` media query. Disable or simplify animations for users who prefer reduced motion. Lazy-loaded content should have meaningful `alt` text on placeholder elements, infinite scroll should maintain keyboard navigability, and dynamically loaded content must be announced to screen readers via `aria-live` regions.

```js
// Respect reduced motion preference
const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');

if (!prefersReducedMotion.matches) {
  // Only set up animation observers if motion is allowed
  document.querySelectorAll('[data-animate]').forEach(el => {
    fadeInObserver.observe(el);
  });
}
```

## Responsive Behavior

Intersection Observer behavior is viewport-independent, but root margins and thresholds may need adjustment for different screen sizes. Use `window.matchMedia` to configure different observer options per breakpoint. For mobile, increase `rootMargin` to compensate for slower scrolling and network conditions, ensuring content loads before users reach it.
