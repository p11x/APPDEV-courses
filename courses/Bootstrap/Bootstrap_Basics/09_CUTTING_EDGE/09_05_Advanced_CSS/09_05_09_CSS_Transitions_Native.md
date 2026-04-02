---
title: "Native CSS Transitions with Bootstrap"
category: "Cutting Edge"
difficulty: 2
time: "20 min"
tags: bootstrap5, css, transitions, animation, performance
prerequisites: ["04_01_01_Transition_Utilities"]
---

## Overview

Native CSS transitions combined with Bootstrap utilities create smooth, performant animations without JavaScript. By leveraging CSS transitions on Bootstrap classes, components like cards, buttons, and modals gain polished micro-interactions that enhance user experience while maintaining zero JavaScript overhead for the animation logic itself.

## Basic Implementation

### Hover Card Transitions

```html
<div class="card transition-card" style="transition: transform 0.3s ease, box-shadow 0.3s ease;">
  <img src="image.jpg" class="card-img-top" alt="Card image">
  <div class="card-body">
    <h5 class="card-title">Hover Effect Card</h5>
    <p class="card-text">This card lifts and gains shadow on hover.</p>
  </div>
</div>

<style>
.transition-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 12px 24px rgba(0,0,0,0.15);
}
</style>
```

### Button Ripple Effect

```html
<button class="btn btn-primary position-relative overflow-hidden">
  <span class="position-relative z-1">Click Me</span>
  <span class="ripple position-absolute top-50 start-50 translate-middle rounded-circle"></span>
</button>

<style>
.btn-primary:hover .ripple {
  animation: ripple-effect 0.6s ease-out;
}
@keyframes ripple-effect {
  from { width: 0; height: 0; opacity: 0.5; }
  to { width: 200px; height: 200px; opacity: 0; }
}
</style>
```

## Advanced Variations

### Staggered List Animation

```html
<ul class="list-group stagger-list">
  <li class="list-group-item" style="--delay: 0">First item</li>
  <li class="list-group-item" style="--delay: 1">Second item</li>
  <li class="list-group-item" style="--delay: 2">Third item</li>
  <li class="list-group-item" style="--delay: 3">Fourth item</li>
</ul>

<style>
.stagger-list .list-group-item {
  opacity: 0;
  transform: translateX(-20px);
  animation: slide-in 0.4s ease forwards;
  animation-delay: calc(var(--delay) * 0.1s);
}
@keyframes slide-in {
  to { opacity: 1; transform: translateX(0); }
}
</style>
```

### Modal Scale Transition

```html
<div class="modal fade" id="scaleModal" style="--bs-modal-transition: transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);">
  <div class="modal-dialog" style="transform: scale(0.8); opacity: 0;
    transition: transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1), opacity 0.3s ease;">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Animated Modal</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">Content with bouncy scale animation.</div>
    </div>
  </div>
</div>

<style>
.modal.show .modal-dialog {
  transform: scale(1);
  opacity: 1;
}
</style>
```

## Best Practices

- **Use transform and opacity** - These properties are GPU-accelerated
- **Keep durations under 300ms** - Faster transitions feel more responsive
- **Use ease-out for entrances** - Elements arriving should decelerate
- **Use ease-in for exits** - Elements leaving should accelerate
- **Apply will-change sparingly** - Only for elements that will animate
- **Respect prefers-reduced-motion** - Disable animations for sensitive users
- **Test on lower-end devices** - Transition performance varies
- **Combine with Bootstrap utilities** - Use existing classes for initial states
- **Use CSS custom properties** - Enable easy theming of transition values
- **Document animation purposes** - Explain why each transition exists

## Common Pitfalls

- **Animating layout properties** - width/height/margin cause reflow
- **Too many simultaneous transitions** - Overwhelms rendering pipeline
- **Missing vendor prefixes** - Some older browsers need -webkit-
- **Ignoring reduced motion** - Not providing static alternatives
- **Overusing transitions** - Every element doesn't need animation
- **Inconsistent timing** - Different durations feel jarring
- **Not testing iOS Safari** - Different transition behavior
- **Forgetting transitionend event** - For chaining animations

## Accessibility Considerations

Always include `@media (prefers-reduced-motion: reduce) { * { transition: none !important; } }`. Ensure transitions don't hide critical content. Provide focus indicators that transition smoothly. Animated content must be pauseable. Screen readers should not be affected by visual transitions.

## Responsive Behavior

Transition durations should be shorter on mobile for snappier feel. Hover-based transitions don't work on touch devices - use :active or JavaScript. Reduce transition complexity on smaller screens. Ensure transitioning elements don't overflow viewport. Touch gestures should have immediate visual feedback.
