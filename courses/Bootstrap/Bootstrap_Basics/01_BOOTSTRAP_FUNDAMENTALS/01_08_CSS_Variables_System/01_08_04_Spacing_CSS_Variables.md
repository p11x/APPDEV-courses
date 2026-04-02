---
title: Spacing CSS Variables
category: Bootstrap Fundamentals
difficulty: 2
time: 20 min
tags: bootstrap5, css-variables, spacing, gap, spacers, utilities
---

## Overview

Bootstrap 5's spacing system is built on CSS custom properties that define a consistent scale for margins, padding, and gaps. The base variable `--bs-spacer` is set to `1rem`, and individual spacer variables (`--bs-spacer-0` through `--bs-spacer-5`) provide multipliers of this base value. Gap variables (`--bs-gap-x`, `--bs-gap-y`) control spacing in grid and flex layouts. These variables enable you to customize the entire spacing scale of your project from a single source, ensuring proportional spacing across all components and utility classes.

## Basic Implementation

The spacing scale variables define Bootstrap's spacing system.

```html
<style>
  /* Default spacer values */
  /* --bs-spacer-0: 0;        */
  /* --bs-spacer-1: 0.25rem;  */
  /* --bs-spacer-2: 0.5rem;   */
  /* --bs-spacer-3: 1rem;     */
  /* --bs-spacer-4: 1.5rem;   */
  /* --bs-spacer-5: 3rem;     */
</style>

<!-- Using spacer variables in custom styles -->
<style>
  .custom-spaced {
    margin-bottom: var(--bs-spacer-3);
    padding: var(--bs-spacer-4);
    gap: var(--bs-spacer-2);
  }
</style>

<div class="custom-spaced bg-light">
  Element using CSS variable spacing
</div>
```

Gap variables control spacing in grid and flex layouts.

```html
<!-- Grid with CSS variable gaps -->
<style>
  .custom-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: var(--bs-gap-x, 1rem);
  }
</style>

<div class="custom-grid">
  <div class="bg-light p-3">Item 1</div>
  <div class="bg-light p-3">Item 2</div>
  <div class="bg-light p-3">Item 3</div>
</div>
```

## Advanced Variations

Override the spacing scale to create tighter or looser layouts.

```html
<!-- Custom compact spacing scale -->
<style>
  .compact-layout {
    --bs-spacer-1: 0.125rem;
    --bs-spacer-2: 0.25rem;
    --bs-spacer-3: 0.5rem;
    --bs-spacer-4: 0.75rem;
    --bs-spacer-5: 1rem;
  }
</style>

<div class="compact-layout">
  <div class="p-3 mb-2 bg-light">Compact spacing</div>
  <div class="p-3 mb-2 bg-light">Tighter layout</div>
  <div class="p-3 bg-light">More content density</div>
</div>

<!-- Custom generous spacing scale -->
<style>
  .generous-layout {
    --bs-spacer-1: 0.5rem;
    --bs-spacer-2: 1rem;
    --bs-spacer-3: 2rem;
    --bs-spacer-4: 3rem;
    --bs-spacer-5: 5rem;
  }
</style>

<div class="generous-layout">
  <div class="p-3 mb-2 bg-light">Generous spacing</div>
  <div class="p-3 mb-2 bg-light">More breathing room</div>
</div>
```

Combining spacing variables with component-specific overrides.

```html
<!-- Component-level spacing overrides -->
<style>
  .custom-card {
    --bs-card-spacer-y: var(--bs-spacer-4);
    --bs-card-spacer-x: var(--bs-spacer-4);
  }

  .tight-card {
    --bs-card-spacer-y: var(--bs-spacer-2);
    --bs-card-spacer-x: var(--bs-spacer-2);
  }
</style>

<div class="card custom-card mb-3">
  <div class="card-body">Card with generous internal spacing</div>
</div>

<div class="card tight-card">
  <div class="card-body">Card with compact internal spacing</div>
</div>
```

## Best Practices

1. **Use spacer variables for custom components** - Reference `--bs-spacer-*` variables in custom component CSS for consistency with Bootstrap's spacing scale.
2. **Override at the root for global changes** - Modify `--bs-spacer-*` on `:root` to scale the entire spacing system proportionally.
3. **Maintain proportional relationships** - When adjusting spacer values, maintain the proportional scale to preserve visual hierarchy.
4. **Use gap variables in CSS grid** - Apply `--bs-gap-x` and `--bs-gap-y` in custom grid layouts to align with Bootstrap's gutter system.
5. **Combine with utility classes** - Spacing variables power Bootstrap's `m-*`, `p-*`, and `g-*` utilities. Customizing variables automatically updates utilities.
6. **Consider vertical rhythm** - Consistent vertical spacing variables help maintain rhythm in text-heavy layouts.
7. **Test with different font sizes** - Spacing in `rem` scales with the root font size. Verify spacing appears correct at different base font sizes.
8. **Document spacing overrides** - Record any custom spacer values in your design system documentation.
9. **Use component-specific spacing** - Many Bootstrap components have their own spacing variables (e.g., `--bs-nav-link-padding-x`). Override these for targeted adjustments.
10. **Respect whitespace** - Adequate spacing improves readability. Avoid setting spacer values too low.

## Common Pitfalls

1. **Mixing units** - Bootstrap uses `rem` for spacing. Mixing `px`, `em`, and `rem` creates inconsistent scaling across devices.
2. **Overriding without cascade understanding** - Spacing variable changes only affect elements declared after the override. Ensure overrides are at the top of your CSS.
3. **Ignoring component-specific variables** - Components like cards and modals have dedicated spacing variables. Overriding the global spacers without updating component variables leads to mismatched spacing.
4. **Breaking responsive spacing** - Custom spacer values that work on desktop may cause overflow or excessive whitespace on mobile.
5. **Not testing with real content** - Spacing that looks good with placeholder text may be too tight or loose with actual content lengths.

## Accessibility Considerations

Spacing directly affects readability and interaction. Adequate spacing between interactive elements prevents accidental clicks, benefiting users with motor impairments. Sufficient paragraph and line spacing improves readability for users with cognitive disabilities and dyslexia. WCAG guidelines recommend adequate spacing that does not rely solely on the user's ability to customize it. When adjusting spacing variables, ensure that interactive targets maintain minimum sizes and that text remains comfortably readable at all zoom levels.

## Responsive Behavior

Bootstrap's spacing utilities support responsive prefixes (e.g., `p-md-3`), but the underlying CSS variables do not change by default. To create responsive spacing scales, define variable overrides within media queries. For example, increase `--bs-spacer-3` on larger screens for more generous whitespace. This approach allows the entire spacing system to scale responsively while maintaining Bootstrap's utility class system for individual element adjustments.
