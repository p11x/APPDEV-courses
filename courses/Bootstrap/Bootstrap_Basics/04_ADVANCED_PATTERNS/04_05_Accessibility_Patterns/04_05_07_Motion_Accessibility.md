---
title: "Motion Accessibility in Bootstrap"
module: "04_ADVANCED_PATTERNS"
lesson: "04_05_Accessibility_Patterns"
file: "04_05_07_Motion_Accessibility.md"
difficulty: 2
description: "prefers-reduced-motion media query, Bootstrap's $enable-reduced-motion, disabling animations safely, respecting user preferences"
---

## Overview

Motion and animation can trigger vestibular disorders, migraines, and motion sickness in some users. WCAG 2.3.3 (Level AAA) requires providing mechanisms to disable motion, and WCAG 2.3.1 (Level AA) forbids motion that cannot be disabled unless it is essential. Bootstrap provides built-in support for the `prefers-reduced-motion` media query through its `$enable-reduced-motion` Sass variable.

The `prefers-reduced-motion` media query detects the user's OS-level motion preference:

- **macOS**: System Preferences > Accessibility > Display > Reduce motion
- **Windows**: Settings > Ease of Access > Display > Show animations
- **iOS**: Settings > Accessibility > Motion > Reduce Motion
- **Android**: Settings > Accessibility > Remove animations

## Basic Implementation

### Bootstrap's Built-in Reduced Motion

Bootstrap automatically respects reduced motion preferences when `$enable-reduced-motion: true` (the default). This generates CSS that disables transitions and animations:

```scss
// In Bootstrap's _reboot.scss (automatically included)
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
    scroll-behavior: auto !important;
  }
}
```

This means all Bootstrap components (modals, carousels, collapses, toasts) automatically respect user preferences without additional code.

### Manual Reduced Motion Styles

```css
/* Custom animation that respects user preference */
.hero-animation {
  animation: fadeSlideIn 0.6s ease-out forwards;
}

@media (prefers-reduced-motion: reduce) {
  .hero-animation {
    animation: none;
    opacity: 1;
    transform: none;
  }
}
```

### Disabling Specific Animations

```css
/* Keep modal fade but disable bounce effects */
.modal.fade {
  transition: opacity 0.15s linear;
}

@media (prefers-reduced-motion: reduce) {
  .modal.fade .modal-dialog {
    transition: none;
    transform: none;
  }
}

/* Carousel slide animation */
.carousel-item {
  transition: transform 0.6s ease-in-out;
}

@media (prefers-reduced-motion: reduce) {
  .carousel-item {
    transition: none;
  }
}
```

### Toggle Button for Motion Preference

```html
<button class="btn btn-outline-secondary" id="motionToggle"
        aria-pressed="false" onclick="toggleMotion()">
  <span id="motionIcon" aria-hidden="true">🎬</span>
  <span id="motionLabel">Disable animations</span>
</button>

<script>
function toggleMotion() {
  const btn = document.getElementById('motionToggle');
  const isReduced = document.documentElement.classList.toggle('reduce-motion');
  btn.setAttribute('aria-pressed', isReduced);
  document.getElementById('motionLabel').textContent =
    isReduced ? 'Enable animations' : 'Disable animations';
  localStorage.setItem('reduceMotion', isReduced);
}

// Restore preference on load
if (localStorage.getItem('reduceMotion') === 'true') {
  document.documentElement.classList.add('reduce-motion');
}
</script>
```

```css
/* CSS controlled by toggle */
.reduce-motion *,
.reduce-motion *::before,
.reduce-motion *::after {
  animation-duration: 0.01ms !important;
  animation-iteration-count: 1 !important;
  transition-duration: 0.01ms !important;
}
```

## Advanced Variations

### Conditional Animation Based on Preference

```css
/* Subtle animation for normal users, none for reduced motion */
.card-hover {
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.card-hover:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
}

@media (prefers-reduced-motion: reduce) {
  .card-hover {
    transition: none;
  }
  .card-hover:hover {
    transform: none;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  }
}
```

### Parallax and Scroll Effects

```css
.parallax-section {
  background-attachment: fixed;
  background-size: cover;
  transition: background-position 0.1s linear;
}

@media (prefers-reduced-motion: reduce) {
  .parallax-section {
    background-attachment: scroll;
    background-position: center center !important;
  }
}
```

### Skeleton Loading with Reduced Motion

```css
.skeleton {
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite;
}

@media (prefers-reduced-motion: reduce) {
  .skeleton {
    animation: none;
    background: #e0e0e0;
  }
}

@keyframes shimmer {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
```

### JavaScript Detection

```javascript
// Check user's motion preference
const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');

function handleMotionPreference(event) {
  if (event.matches) {
    // User prefers reduced motion
    disableCustomAnimations();
  } else {
    // User is OK with motion
    enableCustomAnimations();
  }
}

// Listen for changes
prefersReducedMotion.addEventListener('change', handleMotionPreference);

// Check current state
handleMotionPreference(prefersReducedMotion);

function disableCustomAnimations() {
  document.querySelectorAll('[data-animate]').forEach(el => {
    el.style.animation = 'none';
    el.style.transition = 'none';
  });
}
```

### Sass Configuration

```scss
// _variables.scss - Override Bootstrap defaults
$enable-reduced-motion: true;  // Default, generates reduced-motion CSS

// Custom reduced motion transition overrides
$transition-base: all 0.2s ease-in-out;
$transition-fade: opacity 0.15s linear;

@media (prefers-reduced-motion: reduce) {
  // Override specific transitions
  .btn {
    transition: none;
  }

  .collapse {
    transition: none;
  }

  .fade {
    transition: none;
  }
}
```

## Best Practices

1. **Keep `$enable-reduced-motion: true` in Bootstrap** - Disabling this variable removes all reduced-motion CSS and violates WCAG 2.3.3. Never set it to `false` without providing an equivalent mechanism.
2. **Provide non-animated alternatives for all content** - If an animation conveys information (e.g., a spinner indicating loading), ensure the same information is available as text or static content.
3. **Use `prefers-reduced-motion` for all custom animations** - Every CSS animation and transition you write should include a reduced-motion media query override.
4. **Set `animation-duration: 0.01ms` instead of `animation: none`** - This is the recommended approach because it allows the animation to complete instantly rather than being removed entirely, which can cause issues with `animation-fill-mode: forwards`.
5. **Offer a manual toggle in addition to OS preference** - Some users cannot change OS settings. Providing an in-app toggle ensures everyone can control motion.
6. **Store the user's motion preference** - Use `localStorage` to persist the toggle choice across sessions and pages.
7. **Test with reduced motion enabled** - Enable reduced motion in your OS and verify all animations stop while content remains accessible.
8. **Avoid infinite animations** - Infinite looping animations (`animation-iteration-count: infinite`) are particularly problematic. Always disable them in reduced-motion mode.
9. **Keep essential transitions** - Some transitions convey meaning (e.g., modal fade indicating an overlay). In reduced-motion mode, use instant display instead of animated transitions.
10. **Respect scroll-behavior preferences** - Smooth scrolling can trigger motion sensitivity. Bootstrap's reduced-motion CSS sets `scroll-behavior: auto` automatically.
11. **Use `prefers-reduced-motion` in JavaScript** - Check the media query before triggering programmatic animations, not just in CSS.
12. **Document your motion design decisions** - In your design system, note which animations are decorative (safe to remove) and which convey meaning (need alternatives).

## Common Pitfalls

1. **Setting `$enable-reduced-motion: false`** - This removes Bootstrap's built-in reduced-motion CSS entirely, making your site inaccessible to users with vestibular disorders.
2. **Forgetting `prefers-reduced-motion` on custom animations** - Bootstrap components are covered automatically, but any custom CSS animations you add need manual reduced-motion handling.
3. **Using `animation: none` with `animation-fill-mode: forwards`** - Removing the animation entirely can cause the element to revert to its initial state if `forwards` was keeping it in the final state. Use `animation-duration: 0.01ms` instead.
4. **Infinite scroll animations not disabled** - Loading spinners and skeleton screens that animate indefinitely must be disabled or replaced with static alternatives in reduced-motion mode.
5. **Parallax effects causing nausea** - Fixed background attachment and scroll-linked animations are among the most problematic for motion-sensitive users. Always disable them in reduced-motion mode.
6. **JavaScript animations ignoring the preference** - Libraries like GSAP, Framer Motion, or custom JavaScript animations may not respect `prefers-reduced-motion` by default. You must check the preference and disable them manually.
7. **Motion toggle not persisted** - If users toggle motion off but the preference resets on page reload, they must re-toggle on every page, creating a frustrating experience.
8. **Removing essential transitions entirely** - Some transitions help users understand state changes (e.g., accordion expansion). In reduced-motion mode, show the final state instantly rather than removing the interaction entirely.

## Accessibility Considerations

### WCAG Requirements

| Criterion | Level | Requirement |
|-----------|-------|-------------|
| 2.3.1 Three Flashes | A | No content flashes more than 3 times per second |
| 2.3.2 Three Flashes (repeated) | AAA | No content flashes more than 3 times per second, with general flash/red flash thresholds |
| 2.3.3 Motion from Interactions | AAA | Provide mechanism to disable motion triggered by interaction, except for essential animation |

### Vestibular Disorder Considerations

Users with vestibular disorders may experience:
- Nausea and dizziness from parallax scrolling
- Vertigo from zoom animations
- Headaches from continuous animations
- Disorientation from page transitions

Always provide alternatives and respect the `prefers-reduced-motion` setting.

### Testing Reduced Motion

```javascript
// DevTools snippet to toggle reduced motion simulation
// In Chrome DevTools: Rendering tab > Emulate CSS media feature prefers-reduced-motion

// Or programmatically:
const mq = window.matchMedia('(prefers-reduced-motion: reduce)');
console.log('Prefers reduced motion:', mq.matches);

mq.addEventListener('change', (e) => {
  console.log('Motion preference changed:', e.matches ? 'reduce' : 'no-preference');
});
```

## Responsive Behavior

Motion accessibility is consistent across viewport sizes, but mobile-specific considerations exist:

- **Mobile OS settings** - iOS and Android have separate animation settings. iOS "Reduce Motion" also disables the parallax effect on home screen icons and the zoom animation for app transitions. These settings carry into web browsers.
- **Touch interactions** - Swipe gestures on carousels and sliders should still work with reduced motion; only the animated transition is disabled, not the interaction.
- **Mobile performance** - Animations on mobile devices can drain battery. Users who disable motion for accessibility reasons also benefit from improved battery life and performance.
- **Viewport-specific animations** - Some animations only trigger on desktop (e.g., hover effects). Ensure these are also covered by `prefers-reduced-motion`.

```css
/* Responsive animation that respects reduced motion */
@media (min-width: 768px) {
  .feature-card {
    transition: transform 0.3s ease, box-shadow 0.3s ease;
  }
  .feature-card:hover {
    transform: scale(1.02);
  }
}

@media (prefers-reduced-motion: reduce) {
  .feature-card {
    transition: none;
  }
  .feature-card:hover {
    transform: none;
  }
}
```
