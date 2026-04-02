---
title: Custom CSS Animations
category: Advanced Patterns
difficulty: 3
time: 30 min
tags: bootstrap5, animation, keyframes, pulse, spin, bounce, prefers-reduced-motion
---

## Overview

CSS `@keyframes` animations go beyond transitions by defining multi-step sequences with precise intermediate states. While Bootstrap 5 uses CSS transitions for most of its components, `@keyframes` unlock patterns like infinite pulsing, spinning loaders, bounce effects, and entrance choreographies. Bootstrap's utility system and CSS custom properties make it straightforward to build reusable animation utilities that integrate with the existing class-based workflow.

Unlike transitions, which require a state change (hover, class toggle), `@keyframes` animations run automatically when applied and can loop infinitely, reverse, alternate, and pause on demand.

## Basic Implementation

Define a pulse animation that scales an element in and out continuously:

```css
@keyframes pulse {
  0%, 100% {
    transform: scale(1);
    opacity: 1;
  }
  50% {
    transform: scale(1.08);
    opacity: 0.85;
  }
}

.animate-pulse {
  animation: pulse 2s ease-in-out infinite;
}
```

```html
<button class="btn btn-success animate-pulse">
  Notification (3)
</button>
```

A spin animation for loading indicators:

```css
@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

.animate-spin {
  animation: spin 1s linear infinite;
}
```

```html
<div class="d-flex align-items-center gap-2">
  <span class="animate-spin d-inline-block" style="width: 20px; height: 20px;
        border: 3px solid #dee2e6; border-top-color: #0d6efd; border-radius: 50%;"
        role="status" aria-label="Loading"></span>
  <span>Loading...</span>
</div>
```

A bounce animation for attention-grabbing elements:

```css
@keyframes bounce {
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-20px);
  }
  60% {
    transform: translateY(-10px);
  }
}

.animate-bounce {
  animation: bounce 1.5s ease infinite;
}
```

## Advanced Variations

Combine `animation-delay` and `animation-iteration-count` for staggered, finite entrance animations:

```css
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-fade-in-up {
  animation: fadeInUp 0.6s ease-out both;
}

.animate-fade-in-up:nth-child(1) { animation-delay: 0s; }
.animate-fade-in-up:nth-child(2) { animation-delay: 0.15s; }
.animate-fade-in-up:nth-child(3) { animation-delay: 0.3s; }
.animate-fade-in-up:nth-child(4) { animation-delay: 0.45s; }
```

```html
<div class="row g-3">
  <div class="col-6 col-md-3 animate-fade-in-up">
    <div class="card text-center p-3">Item 1</div>
  </div>
  <div class="col-6 col-md-3 animate-fade-in-up">
    <div class="card text-center p-3">Item 2</div>
  </div>
  <div class="col-6 col-md-3 animate-fade-in-up">
    <div class="card text-center p-3">Item 3</div>
  </div>
  <div class="col-6 col-md-3 animate-fade-in-up">
    <div class="card text-center p-3">Item 4</div>
  </div>
</div>
```

Build a reusable animation utility system using CSS custom properties:

```css
:root {
  --animation-duration: 0.3s;
  --animation-timing: ease-in-out;
  --animation-delay: 0s;
  --animation-iteration: 1;
  --animation-direction: normal;
  --animation-fill: both;
}

[class*="animate-"] {
  animation-duration: var(--animation-duration);
  animation-timing-function: var(--animation-timing);
  animation-delay: var(--animation-delay);
  animation-iteration-count: var(--animation-iteration);
  animation-direction: var(--animation-direction);
  animation-fill-mode: var(--animation-fill);
}
```

Pause any animation on demand:

```css
.animate-paused {
  animation-play-state: paused;
}
```

## Best Practices

1. Always include `prefers-reduced-motion` fallbacks for every `@keyframes` animation you define.
2. Use `animation-fill-mode: both` so the element takes the first keyframe's styles during the delay and the last keyframe's styles after completion.
3. Limit `animation-iteration-count: infinite` to decorative elements (loaders, status indicators) — infinite animations on content elements distract users.
4. Prefer `transform` and `opacity` keyframe properties for GPU-accelerated performance.
5. Use `animation-play-state: paused` to pause animations without removing the class, enabling easy toggle behavior.
6. Set `animation-delay` using CSS custom properties or `:nth-child()` selectors for staggered group animations.
7. Name keyframes descriptively (e.g., `fadeInUp`, `slideInLeft`, `pulseGlow`) to keep stylesheets maintainable.
8. Keep animation durations under 500ms for UI interactions; use longer durations only for ambient decorative effects.
9. Test animations in both light and dark Bootstrap themes to ensure visual consistency.
10. Use `backface-visibility: hidden` on animated elements to prevent flickering during rapid transforms.

## Common Pitfalls

1. **No `prefers-reduced-motion` override**: Failing to disable animations for users with motion sensitivity causes accessibility violations and discomfort.
2. **Animating `width`, `height`, or `top`/`left` in keyframes**: These trigger layout and paint cycles. Use `transform: scale()` and `transform: translate()` instead.
3. **Overlapping animations**: Applying multiple `animation` declarations to the same property causes conflicts. Use comma-separated values in a single `animation` shorthand instead.
4. **Forgetting `animation-fill-mode: both`**: Elements snap back to their original state after the animation ends, creating visual glitches.
5. **Infinite animations on scrollable content**: Continuous motion inside scrollable areas can cause vestibular discomfort and distract from content consumption.

## Accessibility Considerations

The `prefers-reduced-motion` media query is critical. Users with vestibular disorders, motion sensitivity, or cognitive impairments can experience nausea, dizziness, or distraction from animated content. Always provide a no-motion alternative:

```css
@media (prefers-reduced-motion: reduce) {
  .animate-pulse,
  .animate-spin,
  .animate-bounce,
  .animate-fade-in-up {
    animation: none !important;
  }

  .animate-fade-in-up {
    opacity: 1;
    transform: none;
  }
}
```

Bootstrap includes this media query globally via `_reboot.scss`, but your custom `@keyframes` animations are not automatically covered. Every animation you define must include its own reduced-motion override. Use `aria-live` regions for content that appears via animation to ensure screen reader users receive equivalent information.

## Responsive Behavior

Adjust animation intensity and complexity for different viewport sizes. Mobile devices benefit from simpler, shorter animations:

```css
@media (max-width: 576px) {
  .animate-pulse {
    animation-duration: 1.5s;
  }

  .animate-fade-in-up {
    animation-duration: 0.4s;
  }
}
```

Use `prefers-reduced-motion` in combination with device capability detection. On low-powered devices, reduce the number of simultaneously animated elements. Test on real mobile hardware — browser DevTools CPU throttling does not fully replicate GPU limitations. Consider disabling decorative infinite animations on mobile entirely to preserve battery life and scroll performance.
