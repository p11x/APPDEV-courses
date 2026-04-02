---
title: Scroll Animations
category: Advanced Patterns
difficulty: 3
time: 30 min
tags: bootstrap5, animation, scroll, intersection-observer, parallax, reveal
---

## Overview

Scroll animations trigger visual changes as elements enter, exit, or move through the viewport. Bootstrap 5 does not include a built-in scroll animation library, but its utility classes and CSS custom properties integrate seamlessly with the Intersection Observer API. This pattern lets you reveal elements on scroll, create progress indicators driven by scroll position, and build parallax-like depth effects without heavy JavaScript libraries.

The Intersection Observer API monitors target elements relative to a root element (typically the viewport) and fires callbacks when specified thresholds are crossed. This is far more performant than listening to the `scroll` event, which fires dozens of times per second and triggers layout thrashing.

## Basic Implementation

Reveal elements as they scroll into view by toggling Bootstrap utility classes:

```html
<div class="row g-4">
  <div class="col-md-4 reveal-on-scroll" data-reveal-delay="0">
    <div class="card h-100">
      <div class="card-body">
        <h5>Feature One</h5>
        <p>This card fades in when scrolled into view.</p>
      </div>
    </div>
  </div>
  <div class="col-md-4 reveal-on-scroll" data-reveal-delay="100">
    <div class="card h-100">
      <div class="card-body">
        <h5>Feature Two</h5>
        <p>This card appears 100ms after the first.</p>
      </div>
    </div>
  </div>
  <div class="col-md-4 reveal-on-scroll" data-reveal-delay="200">
    <div class="card h-100">
      <div class="card-body">
        <h5>Feature Three</h5>
        <p>This card appears 200ms after the first.</p>
      </div>
    </div>
  </div>
</div>
```

```css
.reveal-on-scroll {
  opacity: 0;
  transform: translateY(30px);
  transition: opacity 0.6s ease-out, transform 0.6s ease-out;
}

.reveal-on-scroll.revealed {
  opacity: 1;
  transform: translateY(0);
}
```

```js
const observer = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      const delay = parseInt(entry.target.dataset.revealDelay) || 0;
      setTimeout(() => {
        entry.target.classList.add('revealed');
      }, delay);
      observer.unobserve(entry.target);
    }
  });
}, { threshold: 0.15 });

document.querySelectorAll('.reveal-on-scroll').forEach(el => {
  observer.observe(el);
});
```

## Advanced Variations

Create a scroll-driven progress bar that fills as the user scrolls the page:

```html
<div class="progress scroll-progress" aria-hidden="true">
  <div class="progress-bar bg-primary" id="scrollProgressBar"></div>
</div>
```

```css
.scroll-progress {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 4px;
  z-index: 2000;
  border-radius: 0;
}

#scrollProgressBar {
  width: 0%;
  transition: width 50ms linear;
}
```

```js
window.addEventListener('scroll', () => {
  const scrollTop = window.scrollY;
  const docHeight = document.documentElement.scrollHeight - window.innerHeight;
  const progress = (scrollTop / docHeight) * 100;
  document.getElementById('scrollProgressBar').style.width = `${progress}%`;
}, { passive: true });
```

Parallax-like depth using `transform: translateY` driven by scroll offset:

```css
.parallax-layer {
  transition: transform 0.1s linear;
  will-change: transform;
}
```

```js
window.addEventListener('scroll', () => {
  const scrollY = window.scrollY;
  document.querySelectorAll('.parallax-layer').forEach(layer => {
    const speed = parseFloat(layer.dataset.parallaxSpeed) || 0.3;
    layer.style.transform = `translateY(${scrollY * speed}px)`;
  });
}, { passive: true });
```

Use Intersection Observer with multiple thresholds for granular scroll-driven effects:

```js
const progressObserver = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    const ratio = Math.round(entry.intersectionRatio * 100);
    entry.target.style.setProperty('--visible-ratio', entry.intersectionRatio);
    entry.target.style.opacity = 0.3 + (entry.intersectionRatio * 0.7);
  });
}, { threshold: Array.from({ length: 10 }, (_, i) => i / 10) });

document.querySelectorAll('.fade-on-proximity').forEach(el => {
  progressObserver.observe(el);
});
```

## Best Practices

1. Use Intersection Observer instead of `scroll` event listeners for reveal animations — it is significantly more performant.
2. Set `rootMargin` on the observer to trigger animations slightly before elements enter the viewport for a smoother experience.
3. Use `observer.unobserve(entry.target)` after a reveal fires to prevent re-triggering and reduce observer overhead.
4. Apply `will-change: transform` only to actively animating elements; remove it after animation completes.
5. Mark scroll event listeners as `{ passive: true }` to prevent blocking the main thread.
6. Stagger reveal delays across grid items using `data-reveal-delay` attributes for choreographed entrances.
7. Combine Bootstrap's `.fade` and `.show` classes with scroll triggers for consistency with component animations.
8. Use `transform` and `opacity` exclusively — avoid animating `margin`, `padding`, or `position` on scroll.
9. Provide a CSS-only fallback for users with JavaScript disabled: set elements to visible by default and hide them only when JS is available.
10. Keep parallax effects subtle — large translate values on scroll cause motion sickness in sensitive users.

## Common Pitfalls

1. **Observing elements inside `display: none` containers**: Intersection Observer does not fire for hidden elements. Collapse or tab-hidden content must be re-observed when shown.
2. **Not cleaning up observers**: Leaving observers attached to removed DOM nodes causes memory leaks. Always disconnect or unobserve.
3. **Using `scroll` events without throttling**: Unthrottled scroll handlers fire hundreds of times per second, causing jank. Intersection Observer avoids this entirely.
4. **Forgetting `threshold` configuration**: Without setting `threshold`, the observer fires only at 0 and 1 — you miss intermediate visibility states.
5. **Parallax on mobile without performance testing**: Complex parallax calculations drop frames on mobile GPUs. Test on real devices and reduce complexity if frames drop below 30fps.

## Accessibility Considerations

Respect `prefers-reduced-motion` by disabling scroll animations for users who opt out:

```css
@media (prefers-reduced-motion: reduce) {
  .reveal-on-scroll {
    opacity: 1;
    transform: none;
    transition: none;
  }

  .parallax-layer {
    transform: none !important;
  }
}
```

Ensure scroll-triggered content remains accessible to screen readers regardless of animation state. Hidden-by-default content that only reveals on scroll can be missed by assistive technology — use `aria-live` regions for dynamically appearing critical content. Avoid auto-scrolling or scroll-jacking, which disorients keyboard and screen reader users.

## Responsive Behavior

Intersection Observer thresholds may behave differently on small screens where elements fill more of the viewport. Test reveal triggers at various breakpoints. Reduce or disable parallax intensity on mobile:

```js
const isMobile = window.innerWidth < 768;
const parallaxSpeed = isMobile ? 0.1 : 0.3;
```

Use Bootstrap's responsive grid to adjust the number of reveal-animated items per row. On mobile, stagger delays should be shorter (50ms vs 100ms) since fewer items appear simultaneously. Consider disabling complex scroll animations entirely on devices with limited GPU capability by checking `navigator.hardwareConcurrency` or `navigator.deviceMemory`.
