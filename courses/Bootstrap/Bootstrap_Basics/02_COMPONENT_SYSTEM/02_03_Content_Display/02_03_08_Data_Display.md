---
title: Data Display
category: Component System
difficulty: 1
time: 20 min
tags: bootstrap5, description-list, dl-dt-dd, key-value, content-display
---

## Overview

Bootstrap styles HTML description lists (`<dl>`, `<dt>`, `<dd>`) for displaying key-value pairs, metadata, glossary terms, and structured data. A description list is semantically appropriate when each term has one or more associated descriptions. Bootstrap adds typography styling and a horizontal layout option that aligns terms and definitions side by side, making it ideal for profile details, specification tables, configuration panels, and API documentation.

## Basic Implementation

A vertical description list stacks each term above its definitions.

```html
<dl>
  <dt>Term</dt>
  <dd>Description for the term goes here.</dd>
  <dt>Another Term</dt>
  <dd>Additional description for this term.</dd>
</dl>
```

Apply Bootstrap's typography utilities to improve readability and spacing.

```html
<dl class="row">
  <dt class="col-sm-3">Name</dt>
  <dd class="col-sm-9">John Doe</dd>

  <dt class="col-sm-3">Email</dt>
  <dd class="col-sm-9">john@example.com</dd>

  <dt class="col-sm-3">Role</dt>
  <dd class="col-sm-9">Administrator</dd>
</dl>
```

A single term can have multiple definitions listed below it.

```html
<dl>
  <dt>Programming Languages</dt>
  <dd>JavaScript</dd>
  <dd>Python</dd>
  <dd>Rust</dd>
</dl>
```

## Advanced Variations

Horizontal description lists use `.row` on `<dl>` and `.col-sm-*` classes on `<dt>` and `<dd>` for aligned columns.

```html
<dl class="row">
  <dt class="col-sm-3">Full Name</dt>
  <dd class="col-sm-9">Jane Elizabeth Smith</dd>

  <dt class="col-sm-3">Date of Birth</dt>
  <dd class="col-sm-9">January 15, 1990</dd>

  <dt class="col-sm-3">Address</dt>
  <dd class="col-sm-9">123 Main Street, Springfield, IL 62704</dd>
</dl>
```

Combine description lists with utility classes for styled data cards.

```html
<div class="card">
  <div class="card-header">Server Configuration</div>
  <div class="card-body">
    <dl class="row mb-0">
      <dt class="col-sm-4">CPU</dt>
      <dd class="col-sm-8">8 cores, 3.6 GHz</dd>

      <dt class="col-sm-4">Memory</dt>
      <dd class="col-sm-8">32 GB DDR5</dd>

      <dt class="col-sm-4">Storage</dt>
      <dd class="col-sm-8">1 TB NVMe SSD</dd>

      <dt class="col-sm-4">OS</dt>
      <dd class="col-sm-8">Ubuntu 24.04 LTS</dd>
    </dl>
  </div>
</div>
```

Nest description lists for hierarchical data structures.

```html
<dl class="row">
  <dt class="col-sm-3">Project</dt>
  <dd class="col-sm-9">
    Website Redesign
    <dl class="row mt-2">
      <dt class="col-sm-4">Start Date</dt>
      <dd class="col-sm-8">March 1, 2026</dd>
      <dt class="col-sm-4">Status</dt>
      <dd class="col-sm-8">In Progress</dd>
    </dl>
  </dd>
</dl>
```

Use text truncation for long values that should not wrap.

```html
<dl class="row">
  <dt class="col-sm-3">API Endpoint</dt>
  <dd class="col-sm-9 text-truncate">
    https://api.example.com/v2/resources/items?filter=active&sort=created_at&limit=100
  </dd>
</dl>
```

## Best Practices

1. Use `<dl>` for genuine key-value or term-definition data, not for layout purposes.
2. Apply `.row` to `<dl>` and `.col-sm-*` to `<dt>` and `<dd>` for horizontal alignment.
3. Keep `<dt>` text concise; terms that are too long push descriptions off screen on mobile.
4. Use `mb-0` on the `<dl>` when nesting inside cards to remove extra bottom margin.
5. Apply `text-truncate` on `<dd>` elements with long URLs or strings to prevent overflow.
6. Group related terms under a common heading rather than creating excessively long description lists.
7. Use `<dd>` tags even for single-value terms to maintain consistent markup structure.
8. Avoid using `<table>` for simple key-value data; description lists are semantically correct.
9. Combine with Bootstrap utility classes (`text-muted`, `fw-bold`) for visual hierarchy.
10. Test horizontal description lists at the `sm` breakpoint to verify column behavior on tablets.

## Common Pitfalls

- Using `<table>` markup for simple key-value pairs instead of `<dl>` introduces unnecessary overhead and less semantic structure.
- Forgetting `.row` on the `<dl>` element means the `.col-sm-*` classes on `<dt>` and `<dd>` have no effect.
- Setting the breakpoint too high (e.g., `.col-lg-*`) causes the horizontal layout to only activate on large screens, leaving cramped mobile layouts.
- Placing `<dd>` directly after `<dl>` without `<dt>` causes invalid HTML and unpredictable rendering.
- Nesting description lists too deeply creates confusing indentation and readability issues.
- Not truncating long values in `<dd>` causes layout overflow on narrow screens.

## Accessibility Considerations

Description lists convey semantic relationships between terms and their definitions. Screen readers announce each term followed by its associated definition, making the data easy to navigate. When nesting description lists, ensure the hierarchy is logical. Use `aria-label` on the `<dl>` if the list needs additional context, such as labeling a section as "User Profile Details."

## Responsive Behavior

Horizontal description lists switch to a stacked layout on screens narrower than the specified breakpoint (typically `sm`). Terms and definitions are displayed vertically below the breakpoint and horizontally above it. On very long values, use `text-truncate` or `word-break` utilities to prevent overflow. Vertical description lists are inherently responsive and require no breakpoint configuration.
