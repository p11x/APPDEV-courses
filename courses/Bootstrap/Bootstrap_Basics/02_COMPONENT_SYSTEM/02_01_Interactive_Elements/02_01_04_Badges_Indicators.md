---
tags: [bootstrap, badges, indicators, components]
category: Interactive Elements
difficulty: 1
time: 20 minutes
---

# Badges and Indicators

## Overview

Badges are compact visual indicators used to highlight counts, statuses, labels, and categories within a user interface. Bootstrap 5's badge component is built on a minimal inline element that automatically scales to match the font size of its parent context. Badges serve as notification counters in navigation, status labels in data tables, category tags in content listings, and inline indicators within headings and paragraphs.

The badge system leverages Bootstrap's background color utilities (`bg-*`) and text color utilities (`text-*`) for full visual customization. Pill-shaped badges with fully rounded corners provide a modern aesthetic, while standard badges use smaller border-radius values consistent with other Bootstrap components. Positioned badges extend the pattern to overlay counts on icons and avatars, creating the ubiquitous notification dot or counter seen in modern application interfaces.

Badges are intentionally lightweight. They contain no padding beyond what is necessary for readability and no interactive behavior by default. When badges need to function as links or buttons, they must be wrapped in or combined with interactive elements. This separation of concerns keeps the badge purely presentational while allowing composition with interactive components.

## Basic Implementation

Badges use the `.badge` class with a background color utility. By default, badges are inline elements that size to their content:

```html
<h1>Example heading <span class="badge bg-secondary">New</span></h1>
<h2>Example heading <span class="badge bg-secondary">New</span></h2>
<h3>Example heading <span class="badge bg-secondary">New</span></h3>
<h4>Example heading <span class="badge bg-secondary">New</span></h4>
<h5>Example heading <span class="badge bg-secondary">New</span></h5>
<h6>Example heading <span class="badge bg-secondary">New</span></h6>
```

Badges automatically scale their font size and padding relative to the parent heading, maintaining proportional visual weight across different heading levels.

Contextual color variants use the same `bg-*` utilities available throughout Bootstrap:

```html
<span class="badge bg-primary">Primary</span>
<span class="badge bg-secondary">Secondary</span>
<span class="badge bg-success">Success</span>
<span class="badge bg-danger">Danger</span>
<span class="badge bg-warning text-dark">Warning</span>
<span class="badge bg-info text-dark">Info</span>
<span class="badge bg-light text-dark">Light</span>
<span class="badge bg-dark">Dark</span>
```

Note that `bg-warning` and `bg-info` require `text-dark` for sufficient contrast against their light backgrounds.

Pill badges use `.rounded-pill` for fully rounded corners:

```html
<span class="badge rounded-pill bg-primary">Primary</span>
<span class="badge rounded-pill bg-secondary">Secondary</span>
<span class="badge rounded-pill bg-success">Success</span>
<span class="badge rounded-pill bg-danger">Danger</span>
<span class="badge rounded-pill bg-warning text-dark">Warning</span>
<span class="badge rounded-pill bg-info text-dark">Info</span>
<span class="badge rounded-pill bg-light text-dark">Light</span>
<span class="badge rounded-pill bg-dark">Dark</span>
```

## Advanced Variations

Badges inside buttons create notification counts on action buttons:

```html
<button type="button" class="btn btn-primary">
  Notifications <span class="badge bg-light text-dark">4</span>
</button>
<button type="button" class="btn btn-danger">
  Alerts <span class="badge bg-light text-dark">9+</span>
</button>
```

Positioned badges use `position-relative` on the parent and `position-absolute` on the badge with translate utilities for alignment:

```html
<button type="button" class="btn btn-primary position-relative">
  Inbox
  <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
    99+
    <span class="visually-hidden">unread messages</span>
  </span>
</button>
```

The `translate-middle` class centers the badge on the edge of the parent element. Adjust with `translate-middle-x` or `translate-middle-y` for single-axis centering.

Dot indicators remove text content and reduce the badge to a small circle. Use padding utilities to control size:

```html
<button type="button" class="btn btn-primary position-relative">
  Profile
  <span class="position-absolute top-0 start-100 p-2 bg-danger border border-light rounded-circle">
    <span class="visually-hidden">New alerts</span>
  </span>
</button>
```

Badges with icons provide visual shorthand:

```html
<span class="badge bg-success">
  <i class="bi bi-check-circle me-1"></i>Approved
</span>
<span class="badge bg-danger">
  <i class="bi bi-x-circle me-1"></i>Rejected
</span>
<span class="badge bg-warning text-dark">
  <i class="bi bi-clock me-1"></i>Pending
</span>
```

Badges inside list groups provide inline status indicators:

```html
<ul class="list-group">
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Inbox
    <span class="badge bg-primary rounded-pill">14</span>
  </li>
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Spam
    <span class="badge bg-primary rounded-pill">2</span>
  </li>
  <li class="list-group-item d-flex justify-content-between align-items-center">
    Trash
    <span class="badge bg-primary rounded-pill">1</span>
  </li>
</ul>
```

## Best Practices

1. **Use badges for counts and statuses, not for primary content.** Badges are supplementary indicators. If the information is the main content, use a proper label or paragraph element instead.

2. **Always include `text-dark` on light-colored badge backgrounds.** `bg-warning`, `bg-info`, and `bg-light` produce backgrounds with insufficient contrast against default text color. Explicitly set `text-dark` to meet WCAG AA standards.

3. **Include visually hidden text for notification counts.** Screen readers need context for numeric badges. Use `.visually-hidden` to describe what the number represents, such as "unread messages" or "pending items."

4. **Keep badge text concise.** Badges are designed for short labels and counts. Long text breaks layouts and overflows containers. Limit badge content to one or two words or a number.

5. **Use `rounded-pill` for notification-style badges.** Pill-shaped badges are visually distinct from standard badges, making them more recognizable as notification counters in navigation and buttons.

6. **Position badges with Bootstrap utilities, not custom CSS.** The `position-relative`, `position-absolute`, and `translate-middle` utilities provide consistent positioning that responds correctly to RTL layouts and text direction changes.

7. **Use consistent badge colors across the application.** Define a color vocabulary: blue for informational, green for success, red for danger or high count, yellow for warning. Consistency reduces cognitive load.

8. **Avoid interactive badges without wrapper elements.** Badges are not interactive components. Wrap them in `<a>` or `<button>` elements when click behavior is needed, rather than adding click handlers directly to badge elements.

9. **Scale badge notification thresholds.** Display "99+" instead of large numbers that break badge sizing. Establish a threshold (e.g., 99) and append "+" to indicate overflow.

10. **Test badges at different parent font sizes.** Because badges inherit scaling from their parent, verify appearance inside various heading levels, buttons, and table cells to ensure consistent readability.

## Common Pitfalls

1. **Using badges for content that should be a button or tag component.** Badges are not buttons. If the element needs click behavior and visible affordance, use a button with a badge inside it, not a standalone badge.

2. **Forgetting `text-dark` on light backgrounds.** This is the most common accessibility failure with badges. Yellow, cyan, and light backgrounds produce unreadable text without explicit dark text color.

3. **Positioning badges without `position-relative` on the parent.** Absolute positioning requires a positioned ancestor. Without `position-relative` on the parent, the badge positions relative to the nearest positioned ancestor, which may be the viewport or an unexpected container.

4. **Using `badge` class without a background color.** The `.badge` class alone provides no background color. The badge renders with transparent background and may be invisible against page backgrounds.

5. **Placing badges inside heading elements without semantic consideration.** A `<span>` badge inside an `<h1>` is read as part of the heading text by screen readers. If the badge content is not part of the heading meaning, move it outside the heading or use `aria-hidden="true"`.

6. **Not handling large notification counts.** Displaying "14582" in a tiny badge creates overflow and readability issues. Implement a threshold pattern that caps the displayed number.

7. **Using `btn-close` or interactive elements inside badges.** Badges have minimal padding and are not sized to contain interactive controls. Nest close buttons or dropdowns inside badges produces cramped, inaccessible interfaces.

## Accessibility Considerations

Badges used as notification counters require supplementary text for screen readers. The visual number "3" is meaningless without context. Use `.visually-hidden` to provide that context:

```html
<span class="badge bg-danger">
  3
  <span class="visually-hidden">unread notifications</span>
</span>
```

When badges are used inside links or buttons, the interactive element's accessible name must include the badge information. If the badge is the only content in a link, the badge text becomes the link's accessible name.

Badges with `aria-hidden="true"` are hidden from screen readers. Use this when the badge is purely decorative and the surrounding text already communicates the same information.

Color-only badges (dot indicators without text) must include visually hidden text to convey their meaning. A red dot alone provides no information to users who cannot perceive color.

## Responsive Behavior

Badges are inline elements that adapt to their container naturally. They do not have built-in responsive behavior because their size is determined by content and parent font size.

For responsive notification counts in navigation, combine badges with Bootstrap's display utilities to hide or show badges at specific breakpoints:

```html
<button type="button" class="btn btn-primary position-relative">
  Messages
  <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger d-none d-sm-inline">
    5
    <span class="visually-hidden">new messages</span>
  </span>
</button>
```

The `d-none d-sm-inline` classes hide the badge on extra-small screens and display it on small and above. This prevents badge overflow on very narrow viewports where space is limited.

For list group badges in responsive layouts, ensure the `d-flex justify-content-between` pattern maintains alignment across breakpoints. If the badge text grows long, use `text-truncate` on the list item text to prevent layout shifts.
