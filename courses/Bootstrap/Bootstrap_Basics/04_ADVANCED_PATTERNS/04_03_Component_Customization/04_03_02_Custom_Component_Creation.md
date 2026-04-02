---
title: "Custom Component Creation"
module: "Component Customization"
difficulty: 3
duration: "35 minutes"
prerequisites: ["Bootstrap utilities", "CSS architecture", "BEM methodology"]
tags: ["components", "css", "bem", "architecture"]
---

# Custom Component Creation

## Overview

Building custom components on top of Bootstrap's utility foundation allows you to create reusable UI patterns that integrate seamlessly with Bootstrap's design system. This approach combines Bootstrap's utilities with custom CSS, leveraging the framework's existing spacing, color, and typography systems while introducing domain-specific components.

## Basic Implementation

Create a custom card variant using Bootstrap utilities as a foundation:

```html
<div class="feature-card p-4 rounded-3 shadow-sm bg-white">
  <div class="feature-card__icon mb-3 text-primary">
    <svg width="32" height="32"><!-- icon --></svg>
  </div>
  <h3 class="feature-card__title h5 mb-2">Feature Title</h3>
  <p class="feature-card__description text-muted mb-3">
    Feature description text here.
  </p>
  <a href="#" class="feature-card__link btn btn-outline-primary btn-sm">
    Learn More
  </a>
</div>
```

Style the component using BEM naming with Bootstrap variable integration:

```css
/* feature-card.css */
.feature-card {
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  border: 1px solid var(--bs-border-color);
}

.feature-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.1);
}

.feature-card__icon {
  width: 48px;
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  background-color: var(--bs-primary-bg-subtle);
}

.feature-card__title {
  color: var(--bs-heading-color);
}

.feature-card__description {
  font-size: 0.875rem;
  line-height: 1.5;
}

.feature-card--highlighted {
  border-color: var(--bs-primary);
  background: linear-gradient(135deg, var(--bs-primary-bg-subtle), white);
}
```

## Advanced Variations

Build CSS-only interactive components using Bootstrap's custom properties:

```html
<!-- CSS-only accordion using Bootstrap variables -->
<div class="custom-accordion">
  <details class="custom-accordion__item" open>
    <summary class="custom-accordion__header p-3 d-flex justify-content-between
                    align-items-center bg-body-secondary rounded">
      <span class="fw-semibold">Section Title</span>
      <span class="custom-accordion__icon">▼</span>
    </summary>
    <div class="custom-accordion__content p-3">
      Content goes here with Bootstrap utility classes.
    </div>
  </details>
</div>
```

```css
.custom-accordion__content {
  border: 1px solid var(--bs-border-color);
  border-top: none;
  border-radius: 0 0 var(--bs-border-radius) var(--bs-border-radius);
}

.custom-accordion__item[open] .custom-accordion__icon {
  transform: rotate(180deg);
}

.custom-accordion__icon {
  transition: transform 0.2s ease;
}
```

Extend button patterns with compound components:

```scss
// _button-groups-custom.scss
.btn-icon-text {
  display: inline-flex;
  align-items: center;
  gap: map-get($spacers, 2);

  .btn-icon {
    flex-shrink: 0;
    width: 1.25em;
    height: 1.25em;
  }
}

// BEM + Bootstrap hybrid approach
.btn-social {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  padding: 0;

  &__label {
    @extend .visually-hidden;
  }

  &--github {
    @extend .btn-dark;
  }

  &--twitter {
    @extend .btn-info;
  }
}
```

Create a complex notification component:

```js
// Initialize custom component with JS enhancement
class NotificationToast {
  constructor(element, options = {}) {
    this.element = element;
    this.duration = options.duration || 5000;
    this.init();
  }

  init() {
    this.element.classList.add(
      'notification-toast', 'p-3', 'rounded-2',
      'shadow-lg', 'position-fixed', 'bottom-0', 'end-0', 'm-3'
    );
    this.element.setAttribute('role', 'alert');
    this.scheduleDismiss();
  }

  scheduleDismiss() {
    setTimeout(() => {
      this.element.classList.add('notification-toast--dismissing');
      setTimeout(() => this.element.remove(), 300);
    }, this.duration);
  }
}
```

## Best Practices

1. Use Bootstrap utilities for layout and spacing before writing custom CSS
2. Follow BEM naming for custom component classes
3. Extend Bootstrap's CSS custom properties in your components
4. Keep component CSS scoped and modular
5. Use CSS custom properties for component-level theming
6. Document component variants and states
7. Provide both CSS-only and JS-enhanced versions when possible
8. Test components with Bootstrap's responsive utilities
9. Avoid overriding Bootstrap's core classes directly
10. Use data attributes for component configuration
11. Maintain consistent naming conventions across components
12. Create component documentation with live examples

## Common Pitfalls

1. Overriding Bootstrap's core classes instead of creating new ones
2. Creating overly specific selectors that conflict with Bootstrap
3. Not leveraging Bootstrap's existing utility classes
4. Building components that break at different breakpoints
5. Ignoring Bootstrap's CSS custom property system
6. Creating components without proper ARIA attributes
7. Over-engineering simple UI patterns
8. Not testing with Bootstrap's dark mode

## Accessibility Considerations

- Include proper ARIA roles and attributes on custom components
- Ensure keyboard navigation works for interactive components
- Maintain focus management in dynamic components
- Provide screen reader text using Bootstrap's `.visually-hidden` class
- Test with assistive technologies

## Responsive Behavior

- Use Bootstrap's responsive utility classes for mobile adaptations
- Ensure custom components stack properly on small screens
- Test touch interactions on mobile devices
- Adjust component sizing for different viewport widths
