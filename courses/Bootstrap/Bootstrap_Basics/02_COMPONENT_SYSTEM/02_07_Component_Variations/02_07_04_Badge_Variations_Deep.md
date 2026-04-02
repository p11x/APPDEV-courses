---
title: "Badge Variations Deep Dive"
description: "Master positioned badges, pill badges in buttons, icon badges, and notification dots in Bootstrap 5"
difficulty: 1
tags: [badges, components, variations, notifications, indicators]
prerequisites:
  - "Bootstrap 5 badge basics"
  - "CSS positioning fundamentals"
---

## Overview

Bootstrap badges are small count or label indicators that attach to other components. Variations include positioned badges that overlay corners of buttons or avatars, pill-shaped badges for rounded indicators, badges combined with icons for compact status displays, and notification dots that signal activity without showing a count. These patterns are essential for notification systems, status indicators, and category labels.

## Basic Implementation

### Standard and Pill Badges

```html
<!-- Standard badges -->
<span class="badge bg-primary">Primary</span>
<span class="badge bg-secondary">Secondary</span>
<span class="badge bg-success">Success</span>

<!-- Pill badges -->
<span class="badge rounded-pill bg-primary">14</span>
<span class="badge rounded-pill bg-danger">99+</span>
```

### Badges in Buttons

Badges inside buttons provide action context with a count indicator.

```html
<button type="button" class="btn btn-primary">
  Notifications <span class="badge text-bg-secondary">4</span>
</button>

<button type="button" class="btn btn-outline-danger">
  Messages <span class="badge text-bg-danger">12</span>
</button>
```

## Advanced Variations

### Positioned Badges

Positioned badges use absolute positioning to overlay on parent elements like buttons or avatars.

```html
<style>
  .badge-positioned { position: relative; }
</style>

<!-- Badge on button -->
<div class="badge-positioned d-inline-block">
  <button class="btn btn-outline-secondary">
    <i class="bi bi-bell fs-5"></i>
  </button>
  <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
    99+
    <span class="visually-hidden">unread messages</span>
  </span>
</div>

<!-- Badge on avatar -->
<div class="badge-positioned d-inline-block">
  <img src="https://via.placeholder.com/48" class="rounded-circle" width="48" height="48" alt="User avatar">
  <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-success">
    <span class="visually-hidden">Online status</span>
  </span>
</div>
```

### Badge with Icon

Combining icons with badges creates compact, descriptive indicators.

```html
<span class="badge bg-info text-dark">
  <i class="bi bi-star-fill me-1"></i>Featured
</span>

<span class="badge bg-warning text-dark">
  <i class="bi bi-clock me-1"></i>Pending Review
</span>

<span class="badge bg-success">
  <i class="bi bi-shield-check me-1"></i>Verified
</span>
```

### Notification Dot

A notification dot signals activity without displaying a specific count, using a minimal circular badge.

```html
<div class="badge-positioned d-inline-block">
  <button class="btn btn-outline-secondary position-relative">
    <i class="bi bi-envelope fs-5"></i>
  </button>
  <span class="position-absolute top-0 start-100 p-2 bg-danger border border-light rounded-circle">
    <span class="visually-hidden">New messages</span>
  </span>
</div>

<!-- Inline notification dot -->
<span class="position-relative d-inline-block">
  Profile
  <span class="position-absolute top-0 start-100 translate-middle p-1 bg-danger rounded-circle">
    <span class="visually-hidden">New notification</span>
  </span>
</span>
```

### Badges in List Groups

```html
<ul class="list-group">
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Inbox
    <span class="badge bg-primary rounded-pill">14</span>
  </li>
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Drafts
    <span class="badge bg-secondary rounded-pill">3</span>
  </li>
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Spam
    <span class="badge bg-danger rounded-pill">99+</span>
  </li>
</ul>
```

## Best Practices

1. **Use `rounded-pill`** for count badges to create a capsule shape.
2. **Always include `visually-hidden`** text for positioned badges to provide screen reader context.
3. **Use `text-bg-*` utilities** for badges with automatic contrast text color.
4. **Keep badge counts short** - use `99+` for counts exceeding two digits.
5. **Use `position-absolute`** with `translate-middle` for precise badge overlay positioning.
6. **Apply `border border-light`** to notification dots for visibility against various backgrounds.
7. **Use `d-inline-block`** on the parent to establish a positioning context for absolute badges.
8. **Match badge colors** to the semantic meaning of the notification type.
9. **Use `badge text-bg-*`** pattern instead of the deprecated `badge-*` color classes.
10. **Limit badge text** to 1-3 words; use tooltips for longer descriptions.
11. **Use `p-2` for notification dots** instead of explicit width/height for consistent sizing.
12. **Combine with `justify-content-between`** when badges appear in list items.

## Common Pitfalls

1. **Missing `visually-hidden`** on positioned badges makes them inaccessible.
2. **Large badge text** breaks layout in small containers like buttons.
3. **Forgetting `position-relative`** on parent when using `position-absolute` on badge.
4. **Using badges for long labels** - badges are for counts and short status text only.
5. **Overlapping badges** on small screens when multiple positioned badges compete for space.
6. **Missing `translate-middle`** causes badges to align at the corner rather than overlap it.
7. **Using deprecated `badge badge-*`** syntax from Bootstrap 4 instead of `badge bg-*`.
8. **Notification dots without `visually-hidden`** text provide no information to screen readers.

## Accessibility Considerations

- Positioned badges must include `visually-hidden` text describing their purpose.
- Badge colors alone should not convey meaning - pair with icons or descriptive text.
- Badges in interactive elements (buttons) inherit the button's accessible name.
- Notification dots should be announced via `aria-live` regions for dynamic updates.
- Badge text should be concise enough to be read quickly by screen readers.
- Do not use badges as standalone interactive elements - they lack sufficient click targets.

## Responsive Behavior

- Badges scale with their parent element's font size using `em` units by default.
- Positioned badges may need position adjustments on very small screens.
- Badge text should truncate or abbreviate on narrow viewports.
- List group badges maintain alignment across screen sizes using flex utilities.
- Notification dots remain small and unobtrusive regardless of viewport size.
