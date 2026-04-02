---
title: "Flex Wrap Utilities"
description: "Control how flex items wrap within containers using Bootstrap 5 flex-wrap utilities"
difficulty: 1
estimated_time: "10 minutes"
tags: ["flexbox", "flex-wrap", "wrapping", "responsive"]
---

# Flex Wrap Utilities

## Overview

Flex wrap utilities in Bootstrap 5 control whether flex items are forced onto a single line or allowed to wrap onto multiple lines within a flex container. The three options are `flex-nowrap` (default), `flex-wrap`, and `flex-wrap-reverse`.

Without wrapping, flex items shrink or overflow depending on their minimum content size. Enabling wrapping allows items to flow to the next line when the container's main axis is full, creating natural multi-row layouts for tags, buttons, cards, and galleries.

## Basic Implementation

### Default Behavior (No Wrap)

By default, flex items stay on one line and may overflow:

```html
<div class="d-flex flex-nowrap bg-light overflow-auto" style="max-width: 400px;">
  <div class="p-3 bg-primary text-white m-1" style="min-width: 150px;">Item 1</div>
  <div class="p-3 bg-success text-white m-1" style="min-width: 150px;">Item 2</div>
  <div class="p-3 bg-danger text-white m-1" style="min-width: 150px;">Item 3</div>
  <div class="p-3 bg-warning text-dark m-1" style="min-width: 150px;">Item 4</div>
</div>
```

### Enable Wrapping

Allow items to wrap to the next line when space runs out:

```html
<div class="d-flex flex-wrap bg-light p-2">
  <div class="p-3 bg-primary text-white m-1">Item 1</div>
  <div class="p-3 bg-success text-white m-1">Item 2</div>
  <div class="p-3 bg-danger text-white m-1">Item 3</div>
  <div class="p-3 bg-warning text-dark m-1">Item 4</div>
  <div class="p-3 bg-info text-white m-1">Item 5</div>
  <div class="p-3 bg-secondary text-white m-1">Item 6</div>
</div>
```

### Reverse Wrapping

Items wrap to the next line above the current row:

```html
<div class="d-flex flex-wrap-reverse bg-light p-2" style="height: 200px; align-content: start;">
  <div class="p-3 bg-primary text-white m-1">Item 1</div>
  <div class="p-3 bg-success text-white m-1">Item 2</div>
  <div class="p-3 bg-danger text-white m-1">Item 3</div>
  <div class="p-3 bg-warning text-dark m-1">Item 4</div>
</div>
```

## Advanced Variations

### Responsive Wrapping

Change wrap behavior at different breakpoints:

```html
<!-- No wrap on mobile, wrap on medium+ -->
<div class="d-flex flex-nowrap flex-md-wrap bg-light p-2">
  <div class="p-3 bg-primary text-white m-1" style="min-width: 200px;">Sidebar</div>
  <div class="p-3 bg-success text-white m-1" style="min-width: 200px;">Content</div>
  <div class="p-3 bg-danger text-white m-1" style="min-width: 200px;">Widget</div>
</div>
```

### Wrapping with Gap and Alignment

Combine wrapping with spacing and alignment utilities:

```html
<!-- Tag cloud with wrapping -->
<div class="d-flex flex-wrap gap-2 p-3 bg-light rounded">
  <span class="badge bg-primary fs-6">JavaScript</span>
  <span class="badge bg-secondary fs-6">TypeScript</span>
  <span class="badge bg-success fs-6">React</span>
  <span class="badge bg-danger fs-6">Angular</span>
  <span class="badge bg-warning text-dark fs-6">Vue</span>
  <span class="badge bg-info fs-6">Svelte</span>
  <span class="badge bg-dark fs-6">Node.js</span>
  <span class="badge bg-primary fs-6">Python</span>
  <span class="badge bg-secondary fs-6">Django</span>
</div>

<!-- Centered wrapped items -->
<div class="d-flex flex-wrap justify-content-center align-items-center gap-3 p-3 bg-light">
  <div class="card" style="width: 180px;">Card 1</div>
  <div class="card" style="width: 180px;">Card 2</div>
  <div class="card" style="width: 180px;">Card 3</div>
  <div class="card" style="width: 180px;">Card 4</div>
  <div class="card" style="width: 180px;">Card 5</div>
</div>
```

### Button Groups with Wrapping

```html
<div class="d-flex flex-wrap gap-1 p-3">
  <button class="btn btn-outline-primary">Save</button>
  <button class="btn btn-outline-secondary">Edit</button>
  <button class="btn btn-outline-success">Publish</button>
  <button class="btn btn-outline-danger">Delete</button>
  <button class="btn btn-outline-warning">Archive</button>
  <button class="btn btn-outline-info">Share</button>
  <button class="btn btn-outline-dark">Export</button>
</div>
```

## Best Practices

1. **Use `flex-wrap` by default** when displaying a variable number of items. It prevents horizontal overflow and creates natural multi-row layouts.

2. **Pair with `gap` utilities** for consistent spacing between wrapped items. Margins on individual items can create uneven edge spacing.

3. **Use `flex-nowrap`** intentionally when items should remain on a single line with horizontal scrolling via `overflow-auto`.

4. **Combine `flex-wrap` with `justify-content`** to control how items in the last row are distributed. `justify-content-start` leaves them left-aligned, while `justify-content-center` centers them.

5. **Apply responsive wrapping** with `flex-md-wrap` to allow single-line display on mobile and multi-line on larger screens.

6. **Set `flex-wrap` on containers with many badges, tags, or buttons.** These UI patterns almost always benefit from wrapping behavior.

7. **Use `align-content` utilities** with `flex-wrap` when the container has a fixed height and you want to control how wrapped rows distribute vertically.

8. **Avoid `flex-wrap-reverse`** unless building a chat-like interface where new messages should appear above existing ones. It can confuse users unfamiliar with reverse flow.

9. **Test wrap behavior at various viewport widths.** Items wrapping at unexpected breakpoints indicate sizing or minimum-width issues.

10. **Set `min-width` or `flex-basis`** on flex children to control when wrapping occurs. Without these, items shrink to content size before wrapping.

## Common Pitfalls

### Items not wrapping despite flex-wrap class
If items have explicit `width` or `min-width` values larger than the container, they overflow instead of wrapping. Ensure item sizes allow wrapping within the available space.

### Uneven last row
When the last row has fewer items, they may stretch or leave gaps. Use `justify-content-start` to keep them aligned with previous rows, or `flex-grow-1` on items to fill space.

### Confusing flex-wrap-reverse behavior
`flex-wrap-reverse` wraps items upward, not backward. The first row appears at the bottom. This is counterintuitive and rarely the desired behavior outside specific use cases.

### Not using gap with wrapping
Without `gap`, items need manual margins. Margins on the last item in each row can cause slight misalignment. The `gap` utility handles inter-item spacing uniformly.

### Forgetting overflow handling
`flex-nowrap` combined with fixed-width items leads to overflow. Always apply `overflow-auto` or `overflow-x-auto` on the container when preventing wrap.

## Accessibility Considerations

Flex wrap does not change DOM order, so screen readers traverse items in source order regardless of visual wrapping. For tag clouds or button groups that wrap, ensure the logical sequence makes sense when read linearly.

Wrapped items should remain keyboard-navigable in source order. Avoid using `order` utilities combined with wrapping that would create a confusing tab flow.

When wrapping navigation links, ensure the visual grouping of wrapped rows does not mislead users about the relationship between items. Use ARIA grouping attributes if items belong to distinct categories.

## Responsive Behavior

Wrap utilities support all Bootstrap breakpoints. Use responsive prefixes to enable or disable wrapping at specific screen widths:

```html
<!-- Prevent wrapping on mobile, enable on tablet+ -->
<div class="d-flex flex-nowrap flex-md-wrap gap-2 p-2">
  <div class="p-2 bg-primary text-white" style="min-width: 180px;">Item 1</div>
  <div class="p-2 bg-success text-white" style="min-width: 180px;">Item 2</div>
  <div class="p-2 bg-danger text-white" style="min-width: 180px;">Item 3</div>
</div>
```

This approach ensures compact single-line layouts on small screens while allowing natural wrapping when space permits on larger viewports.
