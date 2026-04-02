---
title: Modal Animations
category: Advanced Patterns
difficulty: 2
time: 25 min
tags: bootstrap5, animation, modal, offcanvas, fade, scale, transitions
---

## Overview

Bootstrap 5's modal and offcanvas components use CSS transitions for entrance and exit animations. Modals default to a fade effect that transitions `opacity` while the `.modal-dialog` scales in slightly with `transform: translateY(-50px)` to `translateY(0)`. Offcanvas slides in from a designated side using `transform: translateX`. Understanding these animation layers lets you customize entrance effects, disable animations for testing, or create entirely new transition behaviors.

The modal plugin toggles `.show` on the `.modal` element and manages a `.modal-backdrop` with its own opacity transition. Both modal and offcanvas dispatch lifecycle events (`show.bs.*`, `shown.bs.*`, `hide.bs.*`, `hidden.bs.*`) you can hook into for choreographed animations.

## Basic Implementation

A standard modal with the default fade animation:

```html
<button type="button" class="btn btn-primary" data-bs-toggle="modal"
        data-bs-target="#demoModal">
  Open Modal
</button>

<div class="modal fade" id="demoModal" tabindex="-1" aria-labelledby="demoModalLabel">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="demoModalLabel">Default Fade Modal</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <p>This modal fades in and the dialog slides down from above.</p>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
        <button class="btn btn-primary">Save</button>
      </div>
    </div>
  </div>
</div>
```

The `.modal-backdrop` fades from `0` to `0.5` opacity over 150ms. The modal itself transitions `opacity` from `0` to `1`, and the dialog applies `transform: scale(0.95)` to `scale(1)` during entrance.

## Advanced Variations

Create a custom scale-in animation by overriding the modal dialog transition:

```css
.modal.fade .modal-dialog {
  transition: transform 0.35s cubic-bezier(0.34, 1.56, 0.64, 1),
              opacity 0.3s ease;
  transform: scale(0.8) translateY(-30px);
  opacity: 0;
}

.modal.show .modal-dialog {
  transform: scale(1) translateY(0);
  opacity: 1;
}
```

For a slide-up modal (common in mobile UIs), change the transform origin:

```css
.modal.fade .modal-dialog {
  transition: transform 0.4s ease-out;
  transform: translateY(100%);
}

.modal.show .modal-dialog {
  transform: translateY(0);
}

.modal-dialog {
  margin-top: auto;
  margin-bottom: 0;
}

.modal-content {
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
}
```

Offcanvas uses a similar system with directional transforms:

```html
<button class="btn btn-dark" data-bs-toggle="offcanvas" data-bs-target="#sidePanel">
  Open Panel
</button>

<div class="offcanvas offcanvas-end" tabindex="-1" id="sidePanel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Settings</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <p>This offcanvas slides from the right.</p>
  </div>
</div>
```

Disable modal animations entirely by removing the `.fade` class:

```html
<div class="modal" id="instantModal" tabindex="-1">
  <!-- Modal content appears/disappears instantly without transition -->
</div>
```

## Best Practices

1. Keep modal transitions under 400ms — longer durations feel unresponsive for critical interactions like confirmations.
2. Use `.fade` for all modals unless you have a specific reason for instant appearance (e.g., loading indicators).
3. Customize `$modal-transition` and `$offcanvas-transition` Sass variables for project-wide animation changes.
4. Hook into `shown.bs.modal` event to focus the first interactive element after animation completes.
5. Use `data-bs-backdrop="static"` to prevent closing on backdrop click when the modal requires explicit user action.
6. Combine modal transitions with content animations — stagger inner elements using transition-delay on `shown.bs.modal`.
7. Test modal animations with long content that causes scroll — the backdrop and dialog should remain visually correct.
8. Use offcanvas for navigation patterns and modals for focused tasks — each has appropriate animation defaults.
9. Ensure the backdrop opacity transition matches the dialog transition timing for visual cohesion.
10. Use `data-bs-keyboard="false"` for modals that should not close on Escape press.

## Common Pitfalls

1. **Removing `.fade` but expecting animation**: Without `.fade`, the modal toggles `.show` immediately with no transition. Add `.fade` back if you want animation.
2. **Overriding `transform` without accounting for Bootstrap's scale**: Bootstrap applies `transform: scale()` on `.modal-dialog`. Overriding only part of the transform can break the animation.
3. **Z-index conflicts with nested modals**: Stacking multiple modals requires careful z-index management. Bootstrap supports this but backdrop stacking can get messy.
4. **Forgetting `tabindex="-1"`**: The modal element needs `tabindex="-1"` for focus management. Without it, keyboard focus does not trap inside the modal.
5. **Animating offcanvas with custom CSS while Bootstrap's JS also manages transforms**: This causes conflicting transforms. Either disable Bootstrap's JS-managed classes or override carefully.

## Accessibility Considerations

Modals must trap focus within their content when open — Bootstrap handles this automatically. Use `aria-labelledby` pointing to the modal title and `aria-describedby` for the body content. The backdrop transition should complete before allowing user interaction. Screen readers announce modal content when it appears — ensure the modal title is descriptive and sufficient to convey context. For offcanvas, use `aria-label` on the container and `aria-controls` on the trigger.

## Responsive Behavior

Bootstrap modals adapt to viewport size via `.modal-fullscreen` variants and responsive breakpoints:

```html
<div class="modal-dialog modal-fullscreen-sm-down">
  <!-- Fullscreen on small screens, standard modal on larger -->
</div>
```

Offcanvas supports `responsive` variants like `offcanvas-lg-start`, which displays inline on large screens and slides in on smaller ones:

```html
<div class="offcanvas-lg offcanvas-start" id="responsiveOffcanvas">
  <!-- Inline on lg+, offcanvas on smaller viewports -->
</div>
```

Test modal animations on mobile — slow transitions on low-powered devices can cause visible stutter. Consider shortening durations or disabling fade on mobile via media queries.
