---
title: Transition Utilities
category: Advanced Patterns
difficulty: 2
time: 20 min
tags: bootstrap5, animation, transitions, css-transitions, utilities
---

## Overview

CSS transitions provide smooth property changes between states. Bootstrap 5 includes built-in transition utilities and leverages CSS transitions across its interactive components. Understanding how to control `transition-property`, `transition-duration`, `transition-timing-function`, and the shorthand syntax gives you precise control over every animated interaction in your UI.

Bootstrap applies transitions by default to components like modals, dropdowns, collapses, and carousels. You can customize these or add transitions to custom elements using Bootstrap's utility classes and your own CSS.

## Basic Implementation

Bootstrap provides a `.transition` utility class that can be applied to any element. The framework also uses the `$enable-transitions` Sass variable (default: `true`) to include transition styles in compiled CSS.

```html
<div class="card transition" style="transition: transform 0.3s ease-in-out;">
  <div class="card-body">
    <h5 class="card-title">Hover Me</h5>
    <p class="card-text">This card scales on hover.</p>
  </div>
</div>

<style>
  .transition:hover {
    transform: scale(1.03);
  }
</style>
```

Bootstrap's dropdown menus use `transition: opacity 0.15s linear` for fade effects. You can override the default timing:

```css
.dropdown-menu {
  transition: opacity 0.3s ease, transform 0.3s ease;
}
```

The `transition` shorthand accepts: `property duration timing-function delay`. Apply multiple transitions by comma-separating them.

```css
.navbar {
  transition: background-color 0.4s ease-in, box-shadow 0.3s ease-out;
}
```

## Advanced Variations

Combine transitions with Bootstrap's utility classes for reusable patterns. Use `transition-property: all` sparingly; instead, target specific properties for better performance.

```html
<button class="btn btn-primary transition-all"
        style="transition-duration: 0.25s; transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);">
  Custom Easing
</button>
```

Stagger transitions across list items for a choreographed entrance effect:

```css
.list-group-item {
  opacity: 0;
  transform: translateX(-20px);
  transition: opacity 0.4s ease, transform 0.4s ease;
}

.list-group-item.visible {
  opacity: 1;
  transform: translateX(0);
}

.list-group-item:nth-child(1) { transition-delay: 0s; }
.list-group-item:nth-child(2) { transition-delay: 0.1s; }
.list-group-item:nth-child(3) { transition-delay: 0.2s; }
.list-group-item:nth-child(4) { transition-delay: 0.3s; }
```

Override Bootstrap's `$transition-base`, `$transition-fade`, and `$transition-collapse` Sass variables to change defaults globally.

## Best Practices

1. Prefer `transform` and `opacity` transitions over `width`, `height`, or `top`/`left` — they are GPU-accelerated and avoid layout reflow.
2. Keep transition durations between 150ms and 400ms for UI interactions; longer durations feel sluggish.
3. Use `ease-out` for entrances and `ease-in` for exits to match natural motion expectations.
4. Target specific properties in `transition-property` instead of `all` to avoid unintended animated side effects.
5. Always test transitions on lower-end devices to ensure 60fps performance.
6. Use CSS custom properties for transition timing to maintain consistency across a design system.
7. Leverage Bootstrap's Sass variables (`$transition-base`, `$transition-fade`) for global customization.
8. Group related property transitions into a single `transition` declaration for cleaner code.
9. Use `will-change` sparingly and only on elements that will actually animate.
10. Set explicit `transition-duration` to `0s` to immediately disable a transition when needed.

## Common Pitfalls

1. **Animating layout properties**: Transitioning `margin`, `padding`, or `height` triggers expensive layout recalculations. Use `transform` and `opacity` instead.
2. **Transitioning `display: none` to `display: block`**: CSS cannot transition the `display` property. Use `opacity` combined with `visibility` as a workaround.
3. **Missing vendor prefixes**: While modern browsers handle transitions well, older Safari versions may need `-webkit-transition`.
4. **Overriding Bootstrap defaults without specificity**: Bootstrap's transitions use compound selectors. Ensure your override has sufficient specificity or use `!important` as a last resort.
5. **Forgetting `transition-delay` on hover-off**: Without a delay on the exit state, elements snap back instantly, creating visual jarring.

## Accessibility Considerations

Respect the `prefers-reduced-motion` media query. Users who enable reduced motion in their OS settings should not see transitions.

```css
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    transition-duration: 0.01ms !important;
    transition-delay: 0ms !important;
  }
}
```

Bootstrap includes this media query by default via its `_reboot.scss`. When writing custom transitions, always include this override. Avoid transitions that trigger vestibular disorders — large-scale transforms and rapid position changes are especially problematic.

## Responsive Behavior

Transition performance can vary across devices. On mobile, complex transitions may drop frames. Use media queries to reduce or disable transitions on smaller viewports:

```css
@media (max-width: 576px) {
  .card {
    transition: none;
  }
}
```

Bootstrap's responsive utilities work with transitions — combine `.d-none`, `.d-sm-block` with opacity transitions for smooth content reveal across breakpoints. Test on real devices, not just browser DevTools, to catch actual performance bottlenecks.
