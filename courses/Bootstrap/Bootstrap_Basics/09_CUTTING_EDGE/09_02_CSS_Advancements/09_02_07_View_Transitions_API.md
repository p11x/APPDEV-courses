---
title: "View Transitions API"
description: "Page transitions with Bootstrap using view-transition-name for SPA navigation animations"
difficulty: 3
tags: [view-transitions, page-transitions, spa, animation, bootstrap]
prerequisites:
  - 08_02_CSS_Transitions
  - 09_02_03_CSS_Scroll_Driven_Animations
---

## Overview

The View Transitions API enables smooth animated transitions between DOM states. In SPAs, it animates between page views (old DOM replaced by new DOM) with a cross-fade and shared element animations. Individual elements with `view-transition-name` animate from their old position to their new position, creating seamless morphing effects.

Bootstrap components benefit from view transitions when navigating between list views and detail views — a card in a grid morphs into a full detail page. The browser captures the old state as a snapshot, replaces the DOM, captures the new state, and animates between them. CSS pseudo-elements (`::view-transition-old()`, `::view-transition-new()`) control the animation.

## Basic Implementation

```js
// SPA navigation with view transition
async function navigate(url) {
  const response = await fetch(url);
  const html = await response.text();
  const newDoc = new DOMParser().parseFromString(html, 'text/html');

  if (!document.startViewTransition) {
    document.body.replaceWith(newDoc.body);
    return;
  }

  document.startViewTransition(() => {
    document.body.replaceWith(newDoc.body);
  });
}
```

```css
/* Named view transitions for specific elements */
.card-hero-image {
  view-transition-name: hero-image;
}

.detail-hero-image {
  view-transition-name: hero-image; /* same name = shared element animation */
}

/* Customize the transition */
::view-transition-old(hero-image) {
  animation: fade-out 0.3s ease-in;
}

::view-transition-new(hero-image) {
  animation: fade-in 0.3s ease-out;
}

/* Global transition */
::view-transition-old(root) {
  animation: slide-out 0.3s ease-in;
}

::view-transition-new(root) {
  animation: slide-in 0.3s ease-out;
}

@keyframes slide-out {
  to { transform: translateX(-30px); opacity: 0; }
}

@keyframes slide-in {
  from { transform: translateX(30px); opacity: 0; }
}
```

```html
<!-- List view -->
<div class="row">
  <div class="col-md-4">
    <div class="card" onclick="navigate('/detail/1')">
      <img src="photo.jpg" class="card-img-top card-hero-image" alt="...">
      <div class="card-body">
        <h5 class="card-title">Product Name</h5>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

Different transitions per navigation direction:

```css
::view-transition-old(root) {
  animation: slide-to-left 0.3s ease-in;
}

::view-transition-new(root) {
  animation: slide-from-right 0.3s ease-out;
}

/* Reverse for back navigation */
.back-navigation::view-transition-old(root) {
  animation: slide-to-right 0.3s ease-in;
}

.back-navigation::view-transition-new(root) {
  animation: slide-from-left 0.3s ease-out;
}
```

Multiple named elements:

```css
.card-title { view-transition-name: title; }
.card-image { view-transition-name: image; }
.card-price { view-transition-name: price; }
```

## Best Practices

1. Use `view-transition-name` on elements that should animate between views.
2. Each `view-transition-name` must be unique at any moment; remove it when the element is hidden.
3. Use `document.startViewTransition(callback)` to trigger transitions in SPAs.
4. Customize animations with `::view-transition-old()` and `::view-transition-new()` pseudo-elements.
5. Provide a no-transition fallback for browsers without View Transitions API support.
6. Keep transitions under 500ms for perceived performance.
7. Use shared element transitions (same name on old and new elements) for morph effects.
8. Test with reduced motion preferences — disable transitions for `prefers-reduced-motion`.
9. Avoid applying `view-transition-name` to many elements; each one creates a separate snapshot.
10. Use `contain: layout` on transition containers for performance.
11. Clear `view-transition-name` from elements that disappear after transition.
12. Use view transitions for navigation, not hover effects.

## Common Pitfalls

1. **Duplicate names** — Two elements with the same `view-transition-name` at the same time causes errors.
2. **Browser support** — Chrome 111+, no Firefox or Safari support as of 2024.
3. **SPA only (server-side)** — Multi-page view transitions are experimental; stick to SPA patterns.
4. **Memory** — Capturing snapshots for many named elements consumes memory.
5. **`z-index` during transition** — Old and new snapshots have default stacking; use `z-index` to control layering.
6. **Animation interruption** — Navigating again before a transition completes may cause visual glitches.

## Accessibility Considerations

```css
@media (prefers-reduced-motion: reduce) {
  ::view-transition-group(*),
  ::view-transition-old(*),
  ::view-transition-new(*) {
    animation: none !important;
  }
}
```

Ensure focus management during transitions — move focus to the new page's main content after transition completes.

## Responsive Behavior

View transitions work at all viewport sizes. Adjust transition duration or direction based on screen size using media queries within the `::view-transition` pseudo-elements.
