---
title: Breadcrumbs
category: Component System
difficulty: 1
time: 10 min
tags: bootstrap5, breadcrumbs, navigation, hierarchy, divider
---

## Overview

Breadcrumbs are a secondary navigation aid that shows the user's location within a site's hierarchy. Bootstrap provides the `breadcrumb` class for the container and `breadcrumb-item` for each entry. The current page is marked with the `active` class. Breadcrumbs are commonly placed at the top of page content, below the primary navbar. Bootstrap 5.2+ supports CSS variable customization of the divider character, and Bootstrap 5.3+ adds `breadcrumb-divider` utilities for built-in divider options.

## Basic Implementation

A standard breadcrumb with three levels:

```html
<nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><a href="#">Home</a></li>
    <li class="breadcrumb-item"><a href="#">Library</a></li>
    <li class="breadcrumb-item active" aria-current="page">Data</li>
  </ol>
</nav>
```

The `active` item does not contain a link — it represents the current page. `aria-current="page"` is essential for screen reader context.

## Advanced Variations

Breadcrumb with custom divider using CSS variables:

```html
<nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>';">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><a href="#">Home</a></li>
    <li class="breadcrumb-item"><a href="#">Products</a></li>
    <li class="breadcrumb-item"><a href="#">Electronics</a></li>
    <li class="breadcrumb-item active" aria-current="page">Laptops</li>
  </ol>
</nav>
```

You can also use an SVG or Unicode character as the divider:

```html
<nav aria-label="breadcrumb"
     style="--bs-breadcrumb-divider: url(&#34;data:image/svg+xml,...&#34;);">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><a href="#">Dashboard</a></li>
    <li class="breadcrumb-item"><a href="#">Settings</a></li>
    <li class="breadcrumb-item active" aria-current="page">Profile</li>
  </ol>
</nav>
```

To remove the divider entirely, set it to an empty string:

```html
<nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '';">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><a href="#">Home</a></li>
    <li class="breadcrumb-item"><a href="#">Docs</a></li>
    <li class="breadcrumb-item active" aria-current="page">Components</li>
  </ol>
</nav>
```

Breadcrumb with Bootstrap Icons alongside text:

```html
<nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item">
      <a href="#"><i class="bi bi-house-door me-1"></i>Home</a>
    </li>
    <li class="breadcrumb-item">
      <a href="#"><i class="bi bi-collection me-1"></i>Library</a>
    </li>
    <li class="breadcrumb-item active" aria-current="page">
      <i class="bi bi-file-earmark-text me-1"></i>Documentation
    </li>
  </ol>
</nav>
```

Responsive breadcrumb that collapses intermediate items on small screens:

```html
<nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><a href="#">Home</a></li>
    <li class="breadcrumb-item d-none d-sm-inline"><a href="#">Category</a></li>
    <li class="breadcrumb-item d-none d-md-inline"><a href="#">Subcategory</a></li>
    <li class="breadcrumb-item active" aria-current="page">Current Page</li>
  </ol>
</nav>
```

Using `d-none d-sm-inline` hides intermediate breadcrumb items on the smallest screens and reveals them as the viewport grows.

## Best Practices

1. Always use `<nav aria-label="breadcrumb">` as the wrapper for semantic and accessibility correctness.
2. Use an ordered list `<ol>` — breadcrumbs represent a hierarchical sequence, making `<ol>` more appropriate than `<ul>`.
3. Mark the current page with `active` class and `aria-current="page"`.
4. Do not link the last item — it is the current page and linking to itself is confusing.
5. Keep breadcrumb labels short — use concise category names, not full sentences.
6. Place breadcrumbs above the main page content, below the primary navigation bar.
7. Customize dividers with `--bs-breadcrumb-divider` CSS variable rather than modifying HTML structure.
8. Use icons sparingly — they enhance recognition but should not replace text labels.
9. Remove the divider with `--bs-breadcrumb-divider: ''` when a flat label-style breadcrumb is desired.
10. For deeply nested hierarchies, consider collapsing intermediate items on mobile with responsive display utilities.

## Common Pitfalls

1. **Using `<ul>` instead of `<ol>`** — Semantically, breadcrumbs are an ordered hierarchy. `<ul>` conveys no sequence.
2. **Linking the active/current item** — The current page breadcrumb should be plain text, not an anchor.
3. **Missing `aria-label="breadcrumb"` on `<nav>`** — Screen readers cannot identify the purpose of the navigation.
4. **Missing `aria-current="page"` on the active item** — Assistive technology will not announce which crumb is the current page.
5. **Too many levels** — Breadcrumbs beyond 4-5 levels become hard to read. Consider flattening the hierarchy or showing only the first, parent, and current.
6. **Using breadcrumb for non-hierarchical navigation** — Breadcrumbs are for site hierarchy, not for tracking user actions or linear flows.
7. **Custom divider not escaping HTML entities** — When using SVG in `--bs-breadcrumb-divider`, URL-encoded entities must be used inside `url()`.
8. **Breadcrumb hidden on all screen sizes** — Using `d-none` without a responsive breakpoint class hides the breadcrumb entirely.

## Accessibility Considerations

- The `<nav>` landmark with `aria-label="breadcrumb"` allows screen readers to announce "breadcrumb navigation."
- `aria-current="page"` on the active item tells assistive technology which page the user is on.
- Ordered list semantics (`<ol>`) communicate the hierarchical sequence to screen readers.
- Icon-only breadcrumb items need visually hidden text (`<span class="visually-hidden">`) or `aria-label` for context.
- Ensure sufficient color contrast between breadcrumb text and background, particularly for the non-linked active item.

## Responsive Behavior

Bootstrap breadcrumbs are fluid by default and wrap to the next line if they exceed the container width. Strategies for responsive breadcrumbs:

- **Hide intermediate items**: Use `d-none d-sm-inline` or `d-none d-md-inline` on breadcrumb items to show fewer crumbs on smaller screens.
- **Shorten labels**: Use abbreviated text on mobile via responsive utility classes or JavaScript.
- **Wrap behavior**: Breadcrumbs wrap naturally with flexbox. If wrapping is undesirable, add `flex-nowrap overflow-auto` for horizontal scrolling.
- **Divider customization**: Dividers are CSS-only and scale with font size, so they adapt to all breakpoints without adjustment.
- For very long hierarchies, consider a "..." middle crumb that expands on click, revealing hidden intermediate items.
