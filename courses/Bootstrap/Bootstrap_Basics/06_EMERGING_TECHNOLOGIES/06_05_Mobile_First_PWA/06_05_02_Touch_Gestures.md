---
title: "Touch Gestures with Bootstrap"
topic: "Mobile First PWA"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Touch Events API", "Bootstrap 5 basics", "JavaScript fundamentals"]
tags: ["touch", "gestures", "swipe", "mobile", "bootstrap"]
---

## Overview

Touch gestures enhance mobile Bootstrap applications with swipe navigation, pull-to-refresh, pinch-to-zoom, and drag interactions. The browser's Touch Events API (`touchstart`, `touchmove`, `touchend`) provides raw touch data, while libraries like Hammer.js or custom gesture handlers translate touch sequences into semantic gestures. Bootstrap's carousel, offcanvas, and modal components are primary candidates for touch gesture integration.

Bootstrap 5's carousel natively supports swipe for slide transitions. Offcanvas panels can be dismissed by swiping. Custom gestures — like pull-to-refresh on a card list or swipe-to-delete on table rows — combine Bootstrap components with touch event handling to create native-app-like interactions on mobile web.

## Basic Implementation

### Swipe Detection Utility

```js
// utils/touch.js
export function createSwipeDetector(element, callbacks, threshold = 50) {
  let startX = 0;
  let startY = 0;
  let startTime = 0;

  element.addEventListener('touchstart', (e) => {
    const touch = e.touches[0];
    startX = touch.clientX;
    startY = touch.clientY;
    startTime = Date.now();
  }, { passive: true });

  element.addEventListener('touchend', (e) => {
    const touch = e.changedTouches[0];
    const dx = touch.clientX - startX;
    const dy = touch.clientY - startY;
    const dt = Date.now() - startTime;

    if (dt > 500) return; // ignore slow swipes

    if (Math.abs(dx) > Math.abs(dy) && Math.abs(dx) > threshold) {
      if (dx > 0) callbacks.onRight?.();
      else callbacks.onLeft?.();
    } else if (Math.abs(dy) > Math.abs(dx) && Math.abs(dy) > threshold) {
      if (dy > 0) callbacks.onDown?.();
      else callbacks.onUp?.();
    }
  }, { passive: true });
}
```

### Swipe-Enabled Carousel Enhancement

```html
<div id="swipeCarousel" class="carousel slide" data-bs-ride="carousel">
  <div class="carousel-inner">
    <div class="carousel-item active">
      <div class="d-flex align-items-center justify-content-center bg-primary text-white"
           style="height: 300px">
        <h3>Slide 1</h3>
      </div>
    </div>
    <div class="carousel-item">
      <div class="d-flex align-items-center justify-content-center bg-success text-white"
           style="height: 300px">
        <h3>Slide 2</h3>
      </div>
    </div>
    <div class="carousel-item">
      <div class="d-flex align-items-center justify-content-center bg-danger text-white"
           style="height: 300px">
        <h3>Slide 3</h3>
      </div>
    </div>
  </div>
  <button class="carousel-control-prev" data-bs-target="#swipeCarousel" data-bs-slide="prev">
    <span class="carousel-control-prev-icon"></span>
  </button>
  <button class="carousel-control-next" data-bs-target="#swipeCarousel" data-bs-slide="next">
    <span class="carousel-control-next-icon"></span>
  </button>
</div>

<script type="module">
  import { createSwipeDetector } from './utils/touch.js';

  const carouselEl = document.getElementById('swipeCarousel');
  const carousel = new bootstrap.Carousel(carouselEl);

  createSwipeDetector(carouselEl, {
    onLeft: () => carousel.next(),
    onRight: () => carousel.prev(),
  });
</script>
```

## Advanced Variations

### Pull-to-Refresh

```js
// components/pullToRefresh.js
export function initPullToRefresh(container, onRefresh) {
  let startY = 0;
  let pulling = false;
  const threshold = 80;

  const indicator = document.createElement('div');
  indicator.className = 'text-center py-3 d-none';
  indicator.innerHTML = `
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Refreshing...</span>
    </div>
    <p class="text-muted small mt-2 mb-0">Pull to refresh</p>
  `;
  container.prepend(indicator);

  container.addEventListener('touchstart', (e) => {
    if (container.scrollTop === 0) {
      startY = e.touches[0].clientY;
      pulling = true;
    }
  }, { passive: true });

  container.addEventListener('touchmove', (e) => {
    if (!pulling) return;
    const dy = e.touches[0].clientY - startY;
    if (dy > 0 && dy < threshold * 1.5) {
      indicator.classList.remove('d-none');
      indicator.style.transform = `translateY(${dy * 0.5}px)`;
      indicator.querySelector('p').textContent =
        dy > threshold ? 'Release to refresh' : 'Pull to refresh';
    }
  }, { passive: true });

  container.addEventListener('touchend', async () => {
    if (!pulling) return;
    pulling = false;
    const finalY = parseFloat(indicator.style.transform?.match(/[\d.]+/)?.[0] || 0);

    if (finalY > threshold * 0.5) {
      indicator.querySelector('p').textContent = 'Refreshing...';
      await onRefresh();
    }

    indicator.classList.add('d-none');
    indicator.style.transform = '';
  }, { passive: true });
}
```

```html
<div id="refresh-list" class="list-group" style="height: 400px; overflow-y: auto;">
  <div class="list-group-item">Item 1</div>
  <div class="list-group-item">Item 2</div>
  <div class="list-group-item">Item 3</div>
</div>

<script type="module">
  import { initPullToRefresh } from './components/pullToRefresh.js';

  const list = document.getElementById('refresh-list');
  initPullToRefresh(list, async () => {
    await new Promise(r => setTimeout(r, 1500));
    const newItem = document.createElement('div');
    newItem.className = 'list-group-item list-group-item-success';
    newItem.textContent = `New item at ${new Date().toLocaleTimeString()}`;
    list.appendChild(newItem);
  });
</script>
```

### Swipe-to-Delete on List Items

```js
// components/swipeToDelete.js
export function enableSwipeToDelete(listSelector) {
  const items = document.querySelectorAll(`${listSelector} .swipe-item`);

  items.forEach(item => {
    let startX = 0;
    const content = item.querySelector('.swipe-content');
    const action = item.querySelector('.swipe-action');

    item.addEventListener('touchstart', (e) => {
      startX = e.touches[0].clientX;
    }, { passive: true });

    item.addEventListener('touchmove', (e) => {
      const dx = e.touches[0].clientX - startX;
      if (dx < 0 && dx > -100) {
        content.style.transform = `translateX(${dx}px)`;
      }
    }, { passive: true });

    item.addEventListener('touchend', () => {
      const current = parseFloat(content.style.transform?.match(/-?[\d.]+/)?.[0] || 0);
      if (current < -50) {
        content.style.transform = 'translateX(-80px)';
      } else {
        content.style.transform = 'translateX(0)';
      }
    }, { passive: true });
  });
}
```

```html
<div id="task-list" class="list-group">
  <div class="list-group-item swipe-item position-relative overflow-hidden">
    <div class="swipe-content position-relative bg-white p-3"
         style="transition: transform 0.2s; z-index: 1;">
      Task 1
    </div>
    <div class="swipe-action position-absolute top-0 end-0 h-100 d-flex align-items-center px-3 bg-danger text-white"
         style="z-index: 0;">
      Delete
    </div>
  </div>
</div>
```

## Best Practices

1. **Use `{ passive: true }`** on `touchstart` and `touchmove` listeners to avoid blocking scroll performance.
2. **Set a threshold** (50-80px) for swipe detection to avoid accidental gesture triggers.
3. **Distinguish horizontal vs vertical** swipes by comparing `|dx|` vs `|dy|` to prevent conflicts with scrolling.
4. **Use CSS `transform`** for gesture animations instead of `left`/`top` for 60fps performance.
5. **Add `touch-action: pan-y`** in CSS to allow vertical scrolling while capturing horizontal swipes.
6. **Debounce rapid gesture events** to prevent multiple triggers during fast swipes.
7. **Provide visual feedback** (pull indicator, delete reveal) so users understand available gestures.
8. **Include a non-gesture fallback** (button) for accessibility — not all users can perform gestures.
9. **Test on real devices** — touch behavior differs between simulated and actual touchscreens.
10. **Use `changedTouches`** in `touchend` (not `touches`) since active touches are removed on release.

## Common Pitfalls

1. **Missing `{ passive: true }`** on touch listeners causes Chrome to warn about scroll performance.
2. **Not distinguishing horizontal from vertical** swipes causes page scroll conflicts.
3. **Using `touchstart` coordinates in `touchend`** — use `changedTouches` for the final position.
4. **Setting `touch-action: none`** prevents all scrolling — use `pan-y` or `pan-x` instead.
5. **Forgetting to clean up** event listeners when components unmount causes memory leaks.

## Accessibility Considerations

Touch gestures should supplement, not replace, keyboard and screen reader interactions. Always provide button alternatives for swipe-to-delete, pull-to-refresh, and carousel navigation. Use `aria-live="polite"` on containers that update via gestures to announce changes. The `role="button"` attribute should accompany tappable elements. Consider `aria-label` on gesture-enabled elements to describe available interactions for screen reader users.

## Responsive Behavior

Touch gestures are inherently mobile-focused. Use `window.matchMedia('(pointer: coarse)')` to detect touch devices and conditionally enable gesture handlers. On desktop (pointer: fine), fall back to mouse-based interactions or standard Bootstrap component behavior. Swipe thresholds should account for different screen densities. Bootstrap's responsive grid ensures gesture-enabled components (carousels, offcanvas) reflow correctly at all breakpoints.