---
title: List Groups
category: Component System
difficulty: 1
time: 25 min
tags: bootstrap5, list-group, content-display, badges, custom-content
---

## Overview

Bootstrap list groups provide a flexible component for displaying a series of content in a vertical arrangement. They support simple text lists, links, buttons, badges, and fully custom content. Built on the `.list-group` and `.list-group-item` classes, they adapt to various use cases including navigation menus, sidebar filters, settings panels, and data listings.

List groups are responsive by default and support active/disabled states, contextual color variants, horizontal orientation, and JavaScript-driven behavior via Bootstrap's tab plugin.

## Basic Implementation

A minimal list group uses the unordered list element with `.list-group` and `.list-group-item` classes.

```html
<ul class="list-group">
  <li class="list-group-item">Cras justo odio</li>
  <li class="list-group-item">Dapibus ac facilisis in</li>
  <li class="list-group-item">Morbi leo risus</li>
  <li class="list-group-item">Porta ac consectetur ac</li>
  <li class="list-group-item">Vestibulum at eros</li>
</ul>
```

Use `.active` to highlight the current selection and `.disabled` to indicate non-interactive items.

```html
<ul class="list-group">
  <li class="list-group-item active">Active item</li>
  <li class="list-group-item">Normal item</li>
  <li class="list-group-item disabled" aria-disabled="true">Disabled item</li>
</ul>
```

Convert list groups to linked or button-based items for interactive use.

```html
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action active">Current link</a>
  <a href="#" class="list-group-item list-group-item-action">Second link</a>
  <a href="#" class="list-group-item list-group-item-action">Third link</a>
  <button type="button" class="list-group-item list-group-item-action">Button item</button>
</div>
```

Contextual color classes apply background and text color to items.

```html
<ul class="list-group">
  <li class="list-group-item list-group-item-primary">Primary</li>
  <li class="list-group-item list-group-item-success">Success</li>
  <li class="list-group-item list-group-item-danger">Danger</li>
  <li class="list-group-item list-group-item-warning">Warning</li>
</ul>
```

## Advanced Variations

Add badges to list items for counts or status indicators.

```html
<ul class="list-group">
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Inbox
    <span class="badge bg-primary rounded-pill">14</span>
  </li>
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Sent
    <span class="badge bg-success rounded-pill">3</span>
  </li>
</ul>
```

Numbered list groups use the `<ol>` element and `.list-group-numbered` class.

```html
<ol class="list-group list-group-numbered">
  <li class="list-group-item">First item</li>
  <li class="list-group-item">Second item</li>
  <li class="list-group-item">Third item</li>
</ol>
```

Horizontal list groups use `.list-group-horizontal` with optional responsive breakpoint suffixes.

```html
<ul class="list-group list-group-horizontal-md">
  <li class="list-group-item">Item 1</li>
  <li class="list-group-item">Item 2</li>
  <li class="list-group-item">Item 3</li>
</ul>
```

Custom content allows rich layouts inside each item.

```html
<div class="list-group">
  <a href="#" class="list-group-item list-group-item-action active">
    <div class="d-flex w-100 justify-content-between">
      <h5 class="mb-1">List group heading</h5>
      <small>3 days ago</small>
    </div>
    <p class="mb-1">Some placeholder content in a paragraph.</p>
    <small>Additional detail text.</small>
  </a>
  <a href="#" class="list-group-item list-group-item-action">
    <div class="d-flex w-100 justify-content-between">
      <h5 class="mb-1">Another heading</h5>
      <small class="text-muted">Just now</small>
    </div>
    <p class="mb-1">More content here for this item.</p>
  </a>
</div>
```

Flush list groups remove borders and rounded corners for edge-to-edge placement inside cards or other containers.

```html
<ul class="list-group list-group-flush">
  <li class="list-group-item">Flush item 1</li>
  <li class="list-group-item">Flush item 2</li>
</ul>
```

## Best Practices

1. Use `<ul>` and `<li>` for non-interactive lists; use `<div>` with `<a>` or `<button>` for interactive ones.
2. Always pair `.list-group-item-action` with links or buttons to enable hover and focus styles.
3. Apply `aria-current="true"` on the active item for screen reader context.
4. Use `.disabled` along with `aria-disabled="true"` on non-interactive items.
5. Prefer contextual color classes over custom CSS for consistent theming.
6. Use `list-group-flush` inside cards to avoid double borders.
7. Leverage responsive horizontal variants (`list-group-horizontal-sm`, `list-group-horizontal-md`) for adaptive layouts.
8. Keep custom content items structured with consistent heading levels.
9. Use `.rounded-pill` on badges for a polished pill shape.
10. Numbered lists work well for ranked or sequenced content; use `<ol>` semantically.
11. Avoid nesting interactive elements inside `.list-group-item-action` items.
12. Maintain adequate contrast between text and contextual background colors.

## Common Pitfalls

- Mixing `<ul>/<li>` with `<div>` wrappers inside the same list group causes inconsistent rendering and spacing.
- Forgetting `.list-group-item-action` on `<a>` or `<button>` items results in missing hover and focus styles.
- Using `.disabled` on a link without `href="#"` or `tabindex="-1"` does not truly prevent focus navigation.
- Placing non-list content directly inside `.list-group` without proper item classes breaks layout and spacing.
- Overriding padding or margins with custom CSS can break alignment of badges and icons.
- Using horizontal list groups without a breakpoint suffix causes poor mobile rendering on very small screens.
- Applying contextual colors without considering contrast ratios for accessibility compliance.

## Accessibility Considerations

List groups used for navigation should use `<nav>` or `aria-label` to describe their purpose. Active items should include `aria-current="true"` so screen readers can identify the selected state. Disabled items need `aria-disabled="true"` in addition to the `.disabled` class. When list groups function as interactive controls, ensure keyboard navigation works properly and focus indicators are visible.

## Responsive Behavior

List groups are fully responsive by default. The `.list-group-horizontal-{breakpoint}` classes switch from vertical to horizontal layout at the specified breakpoint (sm, md, lg, xl, xxl). Without a breakpoint suffix, the list stays horizontal at all screen sizes, which can cause overflow on small devices. Always pair horizontal variants with a breakpoint to ensure mobile-friendly stacking.
