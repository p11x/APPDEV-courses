---
title: Breakpoint CSS Variables
category: Bootstrap Fundamentals
difficulty: 2
time: 15 min
tags: bootstrap5, css-variables, breakpoints, responsive, media-queries
---

## Overview

Bootstrap 5 exposes its responsive breakpoints as CSS custom properties, making them accessible for use in custom CSS media queries without hardcoding pixel values. The breakpoint variables (`--bs-breakpoint-sm`, `--bs-breakpoint-md`, `--bs-breakpoint-lg`, `--bs-breakpoint-xl`, `--bs-breakpoint-xxl`) maintain consistency between Bootstrap's responsive utilities and custom styles. This ensures that when breakpoints are customized in Sass, the CSS variables update automatically, and custom media queries remain synchronized with Bootstrap's grid and utility system.

## Basic Implementation

Use breakpoint variables in custom media queries for responsive styles.

```html
<style>
  /* Using Bootstrap breakpoint variables */
  .responsive-text {
    font-size: 0.875rem;
  }

  @media (min-width: var(--bs-breakpoint-md)) {
    .responsive-text {
      font-size: 1rem;
    }
  }

  @media (min-width: var(--bs-breakpoint-lg)) {
    .responsive-text {
      font-size: 1.125rem;
    }
  }
</style>

<p class="responsive-text">
  This text scales up at md and lg breakpoints using CSS variables.
</p>
```

Accessing breakpoint values with JavaScript.

```html
<!-- Reading breakpoint variables -->
<script>
  const rootStyles = getComputedStyle(document.documentElement);

  const breakpoints = {
    sm: rootStyles.getPropertyValue('--bs-breakpoint-sm').trim(),
    md: rootStyles.getPropertyValue('--bs-breakpoint-md').trim(),
    lg: rootStyles.getPropertyValue('--bs-breakpoint-lg').trim(),
    xl: rootStyles.getPropertyValue('--bs-breakpoint-xl').trim(),
    xxl: rootStyles.getPropertyValue('--bs-breakpoint-xxl').trim(),
  };

  console.log(breakpoints);
  // Output: { sm: '576px', md: '768px', lg: '992px', xl: '1200px', xxl: '1400px' }
</script>
```

## Advanced Variations

Building responsive custom layouts with breakpoint variables.

```html
<style>
  .dashboard-grid {
    display: grid;
    grid-template-columns: 1fr;
    gap: var(--bs-spacer-3);
  }

  @media (min-width: var(--bs-breakpoint-md)) {
    .dashboard-grid {
      grid-template-columns: repeat(2, 1fr);
    }
  }

  @media (min-width: var(--bs-breakpoint-lg)) {
    .dashboard-grid {
      grid-template-columns: repeat(3, 1fr);
    }
  }

  @media (min-width: var(--bs-breakpoint-xl)) {
    .dashboard-grid {
      grid-template-columns: repeat(4, 1fr);
    }
  }
</style>

<div class="dashboard-grid">
  <div class="bg-light p-3">Widget 1</div>
  <div class="bg-light p-3">Widget 2</div>
  <div class="bg-light p-3">Widget 3</div>
  <div class="bg-light p-3">Widget 4</div>
</div>
```

Combining breakpoint variables with container queries for modern responsive design.

```html
<style>
  /* Custom responsive component using breakpoint variables */
  .sidebar-layout {
    display: flex;
    flex-direction: column;
  }

  .sidebar-layout .sidebar {
    width: 100%;
    order: 2;
  }

  .sidebar-layout .content {
    width: 100%;
    order: 1;
  }

  @media (min-width: var(--bs-breakpoint-md)) {
    .sidebar-layout {
      flex-direction: row;
    }

    .sidebar-layout .sidebar {
      width: 250px;
      order: 0;
    }

    .sidebar-layout .content {
      flex: 1;
      order: 0;
    }
  }

  @media (min-width: var(--bs-breakpoint-xl)) {
    .sidebar-layout .sidebar {
      width: 300px;
    }
  }
</style>

<div class="sidebar-layout">
  <aside class="sidebar bg-light p-3">Sidebar</aside>
  <main class="content p-3">Main content area</main>
</div>
```

## Best Practices

1. **Use breakpoint variables instead of hardcoded values** - Reference `--bs-breakpoint-md` instead of `768px` to stay synchronized with Bootstrap.
2. **Maintain mobile-first approach** - Use `min-width` queries with breakpoint variables to follow Bootstrap's mobile-first methodology.
3. **Combine with CSS custom properties** - Use breakpoint variables alongside other CSS variables for fully dynamic responsive styles.
4. **Keep custom media queries aligned** - Only use breakpoints that Bootstrap defines. Adding custom breakpoints creates maintenance issues.
5. **Test at exact breakpoint values** - Verify layouts at each breakpoint's exact pixel value to catch edge-case issues.
6. **Use variables in container queries** - Modern browsers support container queries. Use breakpoint variables as fallback values.
7. **Document responsive behavior** - Clearly document which custom responsive rules use breakpoint variables and their intended behavior.
8. **Avoid max-width queries** - Stick to `min-width` (mobile-first) for consistency with Bootstrap's approach.
9. **Layer custom styles carefully** - Ensure custom responsive CSS does not conflict with Bootstrap's responsive utilities.
10. **Use JavaScript breakpoint detection** - For JS-based responsive behavior, read breakpoint variables instead of duplicating pixel values.

## Common Pitfalls

1. **Hardcoding pixel values** - Writing `@media (min-width: 768px)` instead of using `--bs-breakpoint-md` creates maintenance issues if breakpoints change.
2. **Using max-width inconsistently** - Mixing `min-width` and `max-width` queries creates confusing and conflicting responsive behavior.
3. **Forgetting the px unit in variable values** - Breakpoint variables include the `px` unit. Don't add another unit when using them.
4. **Mismatched breakpoints** - Custom CSS using different breakpoints than Bootstrap's utilities creates inconsistent responsive behavior.
5. **Not testing on real devices** - Browser resize testing misses device-specific behaviors like viewport scaling and orientation changes.

## Accessibility Considerations

Responsive breakpoints affect content layout and readability. Ensure that custom responsive rules maintain adequate text size and line length (45-75 characters per line is optimal). When layouts change at breakpoints, verify that reading order remains logical for screen readers. Provide sufficient spacing at all viewport sizes for users with motor impairments. Test responsive designs at 200% browser zoom to verify that content reflows properly without horizontal scrolling, per WCAG requirements.

## Responsive Behavior

Breakpoint CSS variables are the foundation of responsive design in Bootstrap. The standard breakpoints are: `sm` (576px), `md` (768px), `lg` (992px), `xl` (1200px), and `xxl` (1400px). Custom media queries using these variables automatically stay aligned with Bootstrap's responsive grid, utility classes, and component behavior. When customizing breakpoints in Bootstrap's Sass, the CSS variables update accordingly, ensuring custom responsive styles remain synchronized without manual updates.
