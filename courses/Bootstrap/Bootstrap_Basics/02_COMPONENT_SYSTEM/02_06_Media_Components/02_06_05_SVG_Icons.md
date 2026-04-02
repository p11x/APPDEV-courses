---
title: "SVG Icons"
course: "Bootstrap Basics"
module: "02_COMPONENT_SYSTEM"
lesson: "02_06_Media_Components"
file: "02_06_05_SVG_Icons"
difficulty: 2
framework_version: "Bootstrap 5.3"
tags: [svg, icons, bootstrap-icons, icon-button, sprite, inline-svg, sizing, colors]
prerequisites:
  - "02_01_Buttons"
  - "02_06_01_Responsive_Images"
description: "Integrate SVG icons using Bootstrap Icons, inline SVG, SVG sprites, icon sizing, coloring, and icon button patterns."
---

## Overview

SVG icons are resolution-independent, styleable with CSS, and accessible when implemented correctly. Bootstrap Icons is the official icon library for Bootstrap 5, providing over 2,000 icons that can be used via a web font, SVG sprite, or individual SVG files. Inline SVG gives you the most control over styling, animation, and accessibility.

There are three primary integration methods: (1) the Bootstrap Icons web font via a CSS class (e.g., `bi-alarm`), (2) inline SVG markup copied from the icon library, and (3) SVG sprite references. Each method has trade-offs in terms of flexibility, performance, and ease of maintenance.

## Basic Implementation

Using the Bootstrap Icons web font is the simplest approach. Include the CDN link and use icon classes:

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">

<i class="bi bi-house"></i>
<i class="bi bi-search"></i>
<i class="bi bi-person-circle"></i>
```

Inline SVG provides full CSS control:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-alarm" viewBox="0 0 16 16">
  <path d="M8.5 5.5a.5.5 0 0 0-1 0v3.362l-1.429 2.38a.5.5 0 1 0 .858.515l1.5-2.5A.5.5 0 0 0 8.5 9V5.5z"/>
  <path d="M6.5 0a.5.5 0 0 0 0 1H7v1.07a7.001 7.001 0 0 0-3.273 12.474l-.602.602a.5.5 0 0 0 .707.708l.746-.746A6.97 6.97 0 0 0 8 16a6.97 6.97 0 0 0 3.422-.892l.746.746a.5.5 0 0 0 .707-.708l-.601-.602A7.001 7.001 0 0 0 9 2.07V1h.5a.5.5 0 0 0 0-1h-3zM9 16A6 6 0 1 1 9 4a6 6 0 0 1 0 12z"/>
</svg>
```

## Advanced Variations

Size icons using Bootstrap's font-size utilities or explicit `width`/`height` attributes:

```html
<i class="bi bi-alarm fs-1"></i>
<i class="bi bi-alarm fs-3"></i>
<i class="bi bi-alarm fs-6"></i>

<!-- Inline SVG sizing -->
<svg class="bi" width="32" height="32" fill="currentColor" viewBox="0 0 16 16">
  <path d="M8.5 5.5a.5.5 0 0 0-1 0v3.362l-1.429 2.38a.5.5 0 1 0 .858.515l1.5-2.5A.5.5 0 0 0 8.5 9V5.5z"/>
</svg>
```

Color icons using text color utilities:

```html
<i class="bi bi-heart-fill text-danger fs-4"></i>
<i class="bi bi-check-circle-fill text-success fs-4"></i>
<i class="bi bi-exclamation-triangle-fill text-warning fs-4"></i>
<i class="bi bi-info-circle-fill text-primary fs-4"></i>
```

Create icon buttons by placing icons inside button elements:

```html
<button type="button" class="btn btn-primary">
  <i class="bi bi-plus-circle me-1"></i> Add Item
</button>

<button type="button" class="btn btn-outline-danger">
  <i class="bi bi-trash"></i>
</button>

<button type="button" class="btn btn-light btn-sm">
  <i class="bi bi-pencil-square"></i>
</button>
```

Use SVG sprite references for a single-file icon system:

```html
<!-- Include sprite in your HTML (loaded once) -->
<svg xmlns="http://www.w3.org/2000/svg" style="display: none;">
  <symbol id="icon-star" viewBox="0 0 16 16">
    <path d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z"/>
  </symbol>
</svg>

<!-- Reference the symbol -->
<svg class="bi text-warning" width="24" height="24" fill="currentColor">
  <use xlink:href="#icon-star"></use>
</svg>
```

Combine icons with badges for notification indicators:

```html
<span class="position-relative d-inline-block">
  <i class="bi bi-bell fs-4"></i>
  <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
    3
    <span class="visually-hidden">unread notifications</span>
  </span>
</span>
```

## Best Practices

1. **Prefer inline SVG** when you need to animate, recolor on hover, or modify individual paths with CSS.
2. **Use the web font approach** for rapid prototyping and when icon customization is minimal.
3. **Set `fill="currentColor"`** on inline SVGs so they inherit text color from parent elements.
4. **Always include `width` and `height`** on inline SVGs to prevent layout shifts during loading.
5. **Use `viewBox="0 0 16 16"`** consistently with Bootstrap Icons to maintain correct aspect ratios.
6. **Add `aria-hidden="true"`** to decorative icons that accompany visible text labels.
7. **Provide `aria-label` or visually hidden text** when an icon is used without a visible text label (e.g., icon-only buttons).
8. **Use `text-*` utilities** for icon colors to maintain theme consistency and support dark mode.
9. **Maintain consistent icon sizing** within a UI context (e.g., all navigation icons should be the same size).
10. **Use the `me-*` or `ms-*` spacing utilities** to separate icons from adjacent text.
11. **Bundle SVG sprites** for production to reduce HTTP requests compared to individual SVG files.
12. **Test icon rendering** across browsers, as SVG rendering can vary slightly in older Safari versions.

## Common Pitfalls

1. **Not adding `aria-hidden="true"`** on decorative icons, causing screen readers to announce meaningless glyph names.
2. **Using icons without visible labels** and without accessible alternatives, creating inoperable interfaces for screen reader users.
3. **Forgetting `fill="currentColor"`** on inline SVGs, preventing CSS color inheritance and breaking dark mode support.
4. **Setting fixed `width`/`height` via CSS `px` values** instead of responsive units, causing icons to appear tiny or oversized at different DPIs.
5. **Mixing web font and inline SVG icons** inconsistently within the same component, creating visual and behavioral inconsistencies.
6. **Not including the Bootstrap Icons CSS/JS** when relying on the web font approach, resulting in empty `<i>` elements.
7. **Omitting the `viewBox` attribute** on inline SVGs, causing icons to clip or stretch incorrectly.
8. **Using deprecated `<use xlink:href>`** syntax instead of modern `<use href>` in HTML5 documents.

## Accessibility Considerations

Decorative icons (those next to visible text) should use `aria-hidden="true"` to prevent redundant announcements. Standalone icons that convey meaning (e.g., a heart for "favorite") require an accessible name via `aria-label` on the parent element or a visually hidden `<span class="visually-hidden">` with descriptive text. Icon-only buttons must have `aria-label` (e.g., `aria-label="Delete item"`). Ensure the icon color meets 3:1 contrast ratio against its background per WCAG requirements for non-text content. When using SVG, add `role="img"` and `aria-label` for meaningful graphics, or `aria-hidden="true"` for decorative ones.

## Responsive Behavior

Bootstrap Icons scale with their parent element's font size. Using `fs-1` through `fs-6` utilities controls size at all breakpoints. For responsive icon sizing, combine with breakpoint-prefixed utilities (e.g., `fs-3 fs-md-1`). Inline SVGs with `width` and `height` in `em` or `rem` units scale proportionally with the surrounding text. Avoid using `vw` units for icons as they can become unreadably small on mobile. In navigation components, switch from icon-and-text to icon-only on small screens using responsive display utilities (`d-none d-md-inline` for text, `d-md-none` for icon alone).
