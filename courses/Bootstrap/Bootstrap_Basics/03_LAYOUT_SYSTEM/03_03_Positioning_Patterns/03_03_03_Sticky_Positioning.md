---
title: "Sticky Positioning"
description: "Implement sticky headers, sidebars, and scroll-aware elements using Bootstrap 5 sticky positioning utilities."
difficulty: 2
estimated_time: "20 minutes"
prerequisites:
  - "Position utilities"
  - "Top/Bottom offset utilities"
tags:
  - positioning
  - sticky
  - scroll
  - layout
  - headers
---

## Overview

Sticky positioning toggles between `relative` and `fixed` based on the user's scroll position. Bootstrap 5 provides `sticky-top` and `sticky-bottom` classes, plus responsive variants like `sticky-sm-top`. An element becomes sticky when the user scrolls to its designated offset point and remains pinned until its parent container scrolls out of view.

Unlike `position-fixed`, sticky elements stay within their parent's bounds, making them ideal for table headers, section titles, and sidebar navigation that should follow the user through a specific content area.

## Basic Implementation

### Sticky Top

Pin an element to the top of its scrollable container:

```html
<div style="height: 300px; overflow-y: scroll;">
  <div class="sticky-top bg-warning p-2">Sticky Header</div>
  <p>Scroll down to see the header stay in place...</p>
  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
  <p>More scrollable content here...</p>
  <p>Even more content...</p>
</div>
```

### Sticky Bottom

Pin an element to the bottom of its scrollable container:

```html
<div style="height: 300px; overflow-y: scroll; position: relative;">
  <p>Content above the sticky footer...</p>
  <p>More content...</p>
  <div class="sticky-bottom bg-dark text-white p-2">
    Sticky Footer
  </div>
</div>
```

### Sticky Table Header

Keep table headers visible while scrolling through large datasets:

```html
<div style="height: 200px; overflow-y: scroll;">
  <table class="table">
    <thead class="sticky-top bg-white">
      <tr>
        <th>#</th>
        <th>Name</th>
        <th>Status</th>
      </tr>
    </thead>
    <tbody>
      <tr><td>1</td><td>Alpha</td><td>Active</td></tr>
      <tr><td>2</td><td>Bravo</td><td>Inactive</td></tr>
      <tr><td>3</td><td>Charlie</td><td>Active</td></tr>
      <!-- More rows -->
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Responsive Sticky Behavior

Use breakpoint modifiers to make elements sticky only above certain screen sizes:

```html
<!-- Sticky only on sm screens and above -->
<div class="sticky-sm-top bg-light p-2">
  Becomes sticky on sm+ screens, static on xs
</div>

<!-- Sticky only on md screens and above -->
<nav class="sticky-md-top bg-dark text-white p-3">
  Desktop sticky navigation
</nav>

<!-- Sticky only on lg screens and above -->
<div class="sticky-lg-top bg-primary text-white p-3">
  Large screen sticky sidebar
</div>
```

### Sticky Sidebar Navigation

```html
<div class="container">
  <div class="row">
    <div class="col-md-3">
      <nav class="sticky-top" style="top: 1rem;">
        <div class="list-group">
          <a href="#section1" class="list-group-item list-group-item-action">Section 1</a>
          <a href="#section2" class="list-group-item list-group-item-action">Section 2</a>
          <a href="#section3" class="list-group-item list-group-item-action">Section 3</a>
        </div>
      </nav>
    </div>
    <div class="col-md-9">
      <h2 id="section1">Section 1</h2>
      <p>Long content...</p>
      <h2 id="section2">Section 2</h2>
      <p>Long content...</p>
    </div>
  </div>
</div>
```

## Best Practices

1. **Set `top` offset explicitly** with inline style or utility (`style="top: 1rem;"`) to control where the element sticks.
2. **Use `sticky-top` on `<thead>`** inside scrollable table containers for persistent column headers.
3. **Apply responsive sticky classes** (`sticky-md-top`) to avoid sticky behavior on small screens where space is limited.
4. **Ensure the parent has scrollable overflow** (`overflow-y: auto` or `overflow-y: scroll`) for sticky to activate.
5. **Do not set `overflow: hidden`** on any ancestor of the sticky element; it breaks sticky positioning.
6. **Add `background-color`** to sticky elements so content beneath does not show through.
7. **Use `sticky-bottom`** for persistent footers or action bars within scrollable sections.
8. **Set appropriate `z-index`** when sticky elements need to scroll over other content.
9. **Test in all major browsers**; sticky behavior has subtle differences in Safari and Firefox.
10. **Combine with `shadow-sm`** to add depth when the element becomes pinned during scroll.
11. **Keep sticky elements compact**; large sticky headers consume valuable viewport space on mobile.
12. **Use `top-0` class** instead of inline style when the standard zero offset is sufficient.

## Common Pitfalls

1. **Ancestor with `overflow: hidden`**: Sticky will not work if any parent has `overflow: hidden`. Check all ancestors.
2. **No scrollable container**: Sticky only activates when the element's scroll container has overflow. Without scrolling, the element stays in normal flow.
3. **Missing `position-sticky`**: The `sticky-top` class sets `position: sticky; top: 0`. If you override position, it breaks.
4. **Not setting a background**: Sticky elements overlay content when pinned; without a background, text becomes unreadable.
5. **Sticky inside flexbox issues**: Some flex containers interfere with sticky. Ensure the flex parent has a defined height or scroll.
6. **Browser inconsistencies**: Safari requires `-webkit-sticky` (Bootstrap includes this, but custom CSS may not).

## Accessibility Considerations

- Sticky headers should not consume more than 20% of the viewport height on mobile to preserve content reading area.
- Ensure sticky elements do not trap keyboard focus or obscure focused elements during tab navigation.
- Provide sufficient color contrast on sticky backgrounds, especially when overlaying various content.
- Use semantic HTML for sticky elements (`<nav>`, `<thead>`, `<header>`) to maintain document structure.
- Test with screen readers to confirm sticky elements are announced at the correct point in the reading order.

## Responsive Behavior

Bootstrap's sticky utilities support responsive breakpoint suffixes out of the box:

```html
<!-- Static on mobile, sticky on md+ -->
<div class="sticky-md-top bg-white p-3 shadow-sm">
  Navigation that sticks only on tablet/desktop
</div>

<!-- Different sticky behavior per breakpoint -->
<div class="sticky-sm-top sticky-lg-bottom bg-light p-2">
  Sticky-top on sm-md, sticky-bottom on lg+
</div>

<!-- Sticky with responsive offset -->
<div class="sticky-top bg-primary text-white p-2" style="top: 0.5rem;">
  Sticky with custom offset at all breakpoints
</div>
```

This allows progressive enhancement where sticky behavior adds value on larger screens without impacting mobile layouts.
