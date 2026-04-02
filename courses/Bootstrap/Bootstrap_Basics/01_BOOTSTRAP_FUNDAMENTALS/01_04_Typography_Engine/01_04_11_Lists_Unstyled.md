---
title: "Lists Unstyled"
topic: "Typography Engine"
subtopic: "Lists Unstyled"
difficulty: 1
duration: "15 minutes"
prerequisites: ["Paragraph Styles", "Text Transformation"]
learning_objectives:
  - Remove default list styling with list-unstyled
  - Create inline lists with list-inline and list-inline-item
  - Style lists for navigation and tag displays
---

## Overview

Bootstrap provides utility classes to transform standard HTML lists into unstyled or inline layouts. The `list-unstyled` class removes bullets and left padding from `<ul>` and `<ol>` elements. The `list-inline` and `list-inline-item` classes convert list items into horizontal inline displays, ideal for tag lists, breadcrumbs, or simple navigation patterns.

## Basic Implementation

Unstyled list removing all default formatting:

```html
<ul class="list-unstyled">
  <li>First item without bullet</li>
  <li>Second item without bullet</li>
  <li>Third item without bullet
    <ul class="list-unstyled">
      <li>Nested unstyled item</li>
      <li>Another nested item</li>
    </ul>
  </li>
</ul>
```

Inline list for horizontal display:

```html
<ul class="list-inline">
  <li class="list-inline-item">Home</li>
  <li class="list-inline-item">About</li>
  <li class="list-inline-item">Services</li>
  <li class="list-inline-item">Contact</li>
</ul>
```

Inline list with separators:

```html
<ul class="list-inline">
  <li class="list-inline-item">First</li>
  <li class="list-inline-item text-muted">|</li>
  <li class="list-inline-item">Second</li>
  <li class="list-inline-item text-muted">|</li>
  <li class="list-inline-item">Third</li>
</ul>
```

## Advanced Variations

Styled tag list using inline items with badges:

```html
<ul class="list-inline">
  <li class="list-inline-item"><span class="badge bg-primary">HTML</span></li>
  <li class="list-inline-item"><span class="badge bg-secondary">CSS</span></li>
  <li class="list-inline-item"><span class="badge bg-success">JavaScript</span></li>
  <li class="list-inline-item"><span class="badge bg-danger">React</span></li>
  <li class="list-inline-item"><span class="badge bg-warning text-dark">Bootstrap</span></li>
</ul>
```

Unstyled list as a navigation sidebar:

```html
<nav>
  <ul class="list-unstyled">
    <li class="mb-2">
      <a href="#" class="text-decoration-none d-block p-2 bg-light rounded">
        Dashboard
      </a>
    </li>
    <li class="mb-2">
      <a href="#" class="text-decoration-none d-block p-2 bg-light rounded">
        Projects
      </a>
    </li>
    <li class="mb-2">
      <a href="#" class="text-decoration-none d-block p-2 bg-light rounded">
        Settings
      </a>
    </li>
  </ul>
</nav>
```

Unstyled list with icons:

```html
<ul class="list-unstyled">
  <li class="d-flex align-items-center mb-2">
    <span class="bg-primary text-white rounded-circle d-inline-flex align-items-center
          justify-content-center me-2" style="width: 32px; height: 32px;">&#10003;</span>
    Feature one completed
  </li>
  <li class="d-flex align-items-center mb-2">
    <span class="bg-success text-white rounded-circle d-inline-flex align-items-center
          justify-content-center me-2" style="width: 32px; height: 32px;">&#10003;</span>
    Feature two completed
  </li>
  <li class="d-flex align-items-center">
    <span class="bg-warning text-white rounded-circle d-inline-flex align-items-center
          justify-content-center me-2" style="width: 32px; height: 32px;">!</span>
    Feature three pending
  </li>
</ul>
```

## Best Practices

1. Use `list-unstyled` when lists represent non-sequential content like navigation links or tag groups.
2. Apply `list-inline` with `list-inline-item` for horizontal tag lists, metadata displays, or footer links.
3. Always add `list-inline-item` to each `<li>` inside a `list-inline` container.
4. Use semantic `<nav>` with unstyled lists for navigation instead of non-semantic `<div>` wrappers.
5. Add `mb-*` or `me-*` spacing utilities to unstyled list items for consistent gaps.
6. Combine `list-unstyled` with `d-flex` and `gap-*` for flexbox-based list layouts.
7. Use `list-inline` for breadcrumb-like separators by adding separator elements between items.
8. Apply `text-truncate` on list items with long text to prevent overflow.
9. Keep nested unstyled lists visually distinct with indentation or background differences.
10. Test unstyled lists with screen readers to ensure they still convey list semantics.

## Common Pitfalls

- **Forgetting `list-inline-item`**: Without this class on each `<li>`, inline lists don't display horizontally.
- **Removing list semantics**: `list-unstyled` only removes visual styling — screen readers still announce list structure.
- **Overusing inline lists**: Inline lists don't wrap well with many items — use flexbox or CSS Grid instead.
- **Missing spacing**: Unstyled lists have no default spacing between items — add `mb-*` utilities.
- **Nesting without `list-unstyled`**: Nested lists inside unstyled lists still show bullets unless also unstyled.
- **Accessibility regression**: Removing all visual indicators from lists makes it harder for users to identify list boundaries.
- **Using `list-inline` for navigation**: `list-inline` lacks proper navigation semantics — use `<nav>` with unstyled lists instead.

## Accessibility Considerations

- `list-unstyled` removes visual bullets but preserves list semantics — screen readers still announce the list.
- Use `role="list"` on `list-unstyled` elements to ensure Safari retains list semantics (Safari removes list role for unstyled lists).
- Provide sufficient spacing between inline list items for touch accessibility (minimum 8px gaps).
- Use `<nav>` with `aria-label` when unstyled lists serve as navigation.
- Ensure inline list items maintain readable text size and contrast ratios.
- Add `aria-current="page"` to the active item in navigation lists.

## Responsive Behavior

Inline lists wrap naturally on narrow screens. For responsive horizontal/vertical switching, combine `list-unstyled` with flexbox utilities:

```html
<ul class="list-unstyled d-flex flex-column flex-md-row gap-2 gap-md-3">
  <li class="list-inline-item bg-light p-2 rounded">Dashboard</li>
  <li class="list-inline-item bg-light p-2 rounded">Analytics</li>
  <li class="list-inline-item bg-light p-2 rounded">Reports</li>
  <li class="list-inline-item bg-light p-2 rounded">Settings</li>
</ul>
```

This list stacks vertically on mobile (`flex-column`) and displays horizontally on medium screens and above (`flex-md-row`). The gap adjusts responsively from `gap-2` (0.5rem) to `gap-md-3` (1rem).
