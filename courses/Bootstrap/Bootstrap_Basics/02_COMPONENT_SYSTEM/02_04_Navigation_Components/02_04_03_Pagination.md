---
title: Pagination
category: Component System
difficulty: 1
time: 15 min
tags: bootstrap5, pagination, navigation, paging, icons
---

## Overview

Bootstrap pagination provides a set of page navigation links for multi-page content such as tables, article listings, and search results. The component uses an unordered list with `pagination` on the wrapper and `page-item` / `page-link` on each entry. Pagination supports active and disabled states, sizing variants, alignment options, and icon-based previous/next links. It is purely presentational — the actual page logic must be handled by your application backend or JavaScript.

## Basic Implementation

A simple pagination component with numbered pages and an active state:

```html
<nav aria-label="Page navigation example">
  <ul class="pagination">
    <li class="page-item"><a class="page-link" href="#">Previous</a></li>
    <li class="page-item"><a class="page-link" href="#">1</a></li>
    <li class="page-item"><a class="page-link" href="#">2</a></li>
    <li class="page-item"><a class="page-link" href="#">3</a></li>
    <li class="page-item"><a class="page-link" href="#">Next</a></li>
  </ul>
</nav>
```

The `<nav>` wrapper with `aria-label` is required for accessibility — it distinguishes this pagination from other navigation landmarks on the page.

## Advanced Variations

Pagination with active and disabled states, plus icon-based previous/next links:

```html
<nav aria-label="Search results pagination">
  <ul class="pagination justify-content-center">
    <li class="page-item disabled">
      <a class="page-link" href="#" tabindex="-1" aria-disabled="true">
        <span aria-hidden="true">&laquo;</span> Prev
      </a>
    </li>
    <li class="page-item active" aria-current="page">
      <a class="page-link" href="#">1</a>
    </li>
    <li class="page-item"><a class="page-link" href="#">2</a></li>
    <li class="page-item"><a class="page-link" href="#">3</a></li>
    <li class="page-item"><a class="page-link" href="#">4</a></li>
    <li class="page-item"><a class="page-link" href="#">5</a></li>
    <li class="page-item">
      <a class="page-link" href="#">Next <span aria-hidden="true">&raquo;</span></a>
    </li>
  </ul>
</nav>
```

Large-sized pagination with `justify-content-end` alignment:

```html
<nav aria-label="Admin panel pagination">
  <ul class="pagination pagination-lg justify-content-end">
    <li class="page-item">
      <a class="page-link" href="#" aria-label="Previous">
        <span aria-hidden="true">&laquo;</span>
      </a>
    </li>
    <li class="page-item active" aria-current="page">
      <a class="page-link" href="#">1</a>
    </li>
    <li class="page-item"><a class="page-link" href="#">2</a></li>
    <li class="page-item"><a class="page-link" href="#">3</a></li>
    <li class="page-item">
      <a class="page-link" href="#" aria-label="Next">
        <span aria-hidden="true">&raquo;</span>
      </a>
    </li>
  </ul>
</nav>
```

Small pagination for compact interfaces or embedded tables:

```html
<nav aria-label="Compact pagination">
  <ul class="pagination pagination-sm">
    <li class="page-item"><a class="page-link" href="#">&laquo;</a></li>
    <li class="page-item active" aria-current="page"><a class="page-link" href="#">1</a></li>
    <li class="page-item"><a class="page-link" href="#">2</a></li>
    <li class="page-item"><a class="page-link" href="#">3</a></li>
    <li class="page-item"><a class="page-link" href="#">&raquo;</a></li>
  </ul>
</nav>
```

## Best Practices

1. Always wrap pagination in a `<nav>` element with a descriptive `aria-label` to identify its purpose.
2. Use `aria-current="page"` on the active item so screen readers announce the current page.
3. Use `aria-disabled="true"` and `tabindex="-1"` on disabled items to prevent keyboard focus.
4. Use `justify-content-center`, `justify-content-end`, or `justify-content-start` for alignment within the `<nav>`.
5. Use `pagination-lg` or `pagination-sm` to match the size context of your layout.
6. Use `&laquo;` / `&raquo;` or Bootstrap Icons for previous/next navigation indicators.
7. Keep the visible page range reasonable — show 5-7 pages with ellipsis for large datasets.
8. Place pagination at the bottom of the content it controls, not at the top.
9. Ensure `href` values are meaningful (e.g., `?page=2`) for progressive enhancement when JavaScript is disabled.
10. Use consistent pagination styling across your application for a predictable UX.

## Common Pitfalls

1. **Missing `<nav>` wrapper** — Without `<nav>`, screen readers cannot identify the pagination as a navigation landmark.
2. **No `aria-label` on `<nav>`** — If the page has multiple `<nav>` elements, assistive technology cannot distinguish them.
3. **Active item not marked with `aria-current="page"`** — Screen readers will not announce which page is current.
4. **Disabled link still focusable** — Forgetting `tabindex="-1"` allows keyboard users to tab into a disabled link.
5. **Using `<button>` instead of `<a>` for page links** — Page links should be `<a>` tags with `href` for proper semantics and right-click/open-in-new-tab behavior.
6. **Too many page numbers** — Displaying 50 page links overwhelms users. Use a windowed approach with first, last, and surrounding pages.
7. **Pagination not wrapping on mobile** — Long pagination overflows on small screens. Use `flex-wrap` or reduce visible page count responsively.
8. **Inconsistent active styling** — Overriding Bootstrap's active styles without maintaining sufficient contrast.

## Accessibility Considerations

- The `<nav>` element provides a navigation landmark. Use `aria-label="Page navigation"` to describe it.
- `aria-current="page"` on the active link tells assistive technology which page the user is on.
- Disabled items need `aria-disabled="true"` to communicate non-interactivity.
- Icon-only previous/next links should have `aria-label="Previous page"` and `aria-label="Next page"` for screen reader context.
- Keyboard navigation works by default — each `page-link` is a focusable anchor. Arrow key navigation is not required for pagination.

## Responsive Behavior

Pagination is built with flexbox and naturally wraps. On small screens:

- Long pagination lists can overflow horizontally. Add `flex-wrap` by wrapping the `<ul>` in a flex container or reduce the number of visible page links.
- `pagination-sm` is ideal for compact mobile layouts.
- `justify-content-center` keeps pagination centered regardless of width.
- Consider showing only Previous/Next with a page indicator (e.g., "Page 3 of 20") on very small screens, replacing the full numbered list.
- Bootstrap does not provide built-in truncation (ellipsis) — implement it with a `page-item disabled` containing `...` or a non-link span.
