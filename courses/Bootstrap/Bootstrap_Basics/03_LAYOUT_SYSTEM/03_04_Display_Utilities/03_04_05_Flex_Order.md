---
title: "Flex Order Utilities"
description: "Control visual order of flex items independently from DOM order using Bootstrap 5 order utilities"
difficulty: 1
estimated_time: "12 minutes"
tags: ["flexbox", "order", "visual-ordering", "responsive"]
---

# Flex Order Utilities

## Overview

Bootstrap 5's `order` utilities change the visual order of flex items without modifying the DOM structure. Available classes include `order-first` (maps to -1), `order-0` through `order-5`, and `order-last` (maps to 6). Items with lower order values appear first in the flex container.

These utilities are valuable for responsive layouts where content order should differ between mobile and desktop viewports. For example, a sidebar can appear after the main content on mobile but beside it on desktop, all without duplicating HTML or using JavaScript.

## Basic Implementation

### Order Classes

Reorder flex items visually using numeric order values:

```html
<div class="d-flex">
  <div class="p-3 bg-primary text-white order-3">Third visually</div>
  <div class="p-3 bg-success text-white order-1">First visually</div>
  <div class="p-3 bg-danger text-white order-2">Second visually</div>
</div>
```

### Order First and Last

Use `order-first` and `order-last` to push items to the extremes:

```html
<div class="d-flex">
  <div class="p-3 bg-primary text-white">Middle</div>
  <div class="p-3 bg-success text-white order-first">Always first</div>
  <div class="p-3 bg-danger text-white order-last">Always last</div>
  <div class="p-3 bg-warning text-dark">Middle</div>
</div>
```

### Default Order

Items without an `order` class have an implicit order of 0. They appear in DOM order relative to each other:

```html
<div class="d-flex">
  <div class="p-3 bg-primary text-white">Default order (0)</div>
  <div class="p-3 bg-success text-white order-1">After default</div>
  <div class="p-3 bg-danger text-white">Also default (0)</div>
</div>
```

## Advanced Variations

### Responsive Ordering

Change item order at specific breakpoints. This is the primary use case for order utilities:

```html
<!-- Content first on mobile, sidebar first on desktop -->
<div class="d-flex flex-column flex-md-row">
  <main class="p-3 bg-light order-2 order-md-1 flex-grow-1">
    Main content appears first on mobile, second on desktop
  </main>
  <aside class="p-3 bg-secondary text-white order-1 order-md-2" style="width: 250px;">
    Sidebar appears second on mobile, first on desktop
  </aside>
</div>

<!-- Navigation reorder -->
<div class="d-flex flex-column flex-lg-row align-items-center gap-2 p-3 bg-light">
  <img src="logo.png" alt="Logo" class="order-lg-1 order-2">
  <nav class="d-flex gap-3 order-lg-2 order-1">
    <a href="#">Home</a>
    <a href="#">About</a>
  </nav>
  <button class="btn btn-primary order-3">CTA</button>
</div>
```

### Ordering with Grid and Card Layouts

```html
<div class="d-flex flex-wrap">
  <div class="card order-2" style="width: 33%;">Featured 1</div>
  <div class="card order-1" style="width: 33%;">Highlight (appears first)</div>
  <div class="card order-3" style="width: 33%;">Featured 2</div>
</div>
```

### Combining with Align Self

Order and alignment work independently, allowing items to be reordered while maintaining individual alignment:

```html
<div class="d-flex align-items-center" style="height: 150px;">
  <div class="p-2 bg-primary text-white align-self-start order-3">Last, top-aligned</div>
  <div class="p-2 bg-success text-white align-self-center order-1">First, centered</div>
  <div class="p-2 bg-danger text-white align-self-end order-2">Second, bottom-aligned</div>
</div>
```

## Best Practices

1. **Use responsive order utilities** (`order-md-1`, `order-lg-last`) as the primary mechanism for reordering content across breakpoints. This avoids duplicating HTML.

2. **Keep DOM order logical.** Since screen readers and keyboard navigation follow DOM order, the source sequence should make sense independently of visual ordering.

3. **Use `order-first`** for elements like call-to-action buttons or logos that should visually lead regardless of their position in the markup.

4. **Limit order range.** Sticking to `order-0` through `order-5` keeps the ordering scheme manageable. Avoid creating complex order hierarchies.

5. **Pair with flex direction changes.** When switching from `flex-column` to `flex-row` responsively, order utilities can rearrange items appropriately for each layout.

6. **Document order intent.** When multiple items have custom order values, add comments explaining the intended visual sequence for future maintainers.

7. **Avoid order on interactive elements.** Reordering buttons or links visually while tab order follows DOM creates confusion for keyboard users.

8. **Test with screen readers.** VoiceOver, NVDA, and JAWS announce content in DOM order. Verify that reordered content still makes logical sense when read aloud.

9. **Use `order-0`** to explicitly reset an item to default ordering when it might otherwise inherit an order value from a smaller breakpoint.

10. **Combine with Bootstrap's grid system** for complex responsive layouts. Grid column classes plus order utilities provide powerful layout control.

## Common Pitfalls

### Screen reader mismatch
Visual order diverging from DOM order is the primary accessibility concern. Screen reader users hear content in source order, which may contradict the visual arrangement.

### Overriding order unintentionally at larger breakpoints
Setting `order-2` without a responsive prefix applies it at all sizes. If you set `order-md-1` on another element but forget to scope `order-2` to mobile, the desktop layout may be incorrect.

### Complex order hierarchies becoming unmaintainable
When many items have different order values, tracking the visual sequence becomes difficult. Limit reordering to 2-3 items per container.

### Order not applying outside flex containers
`order` utilities only work within flex or grid containers. Applying them to children of a `d-block` element has no effect.

### Forgetting that `order-last` equals 6
`order-last` is equivalent to `order-6`. Items with `order-5` still appear before `order-last`. This distinction matters when combining with `order-first` (-1) and numeric values.

## Accessibility Considerations

Order utilities present a significant accessibility responsibility. The WCAG 2.1 Success Criterion 1.3.2 (Meaningful Sequence) requires that content order makes sense when linearized. When using `order` to rearrange visual layout, ensure the DOM order provides a coherent reading experience.

Never use `order` to move skip-navigation links or focus-trapping elements. These must remain at predictable DOM positions for assistive technology.

For responsive layouts where order changes at breakpoints, test both mobile and desktop layouts with a screen reader. The content should tell a complete story in both arrangements.

## Responsive Behavior

Order utilities support all Bootstrap breakpoints. The mobile-first approach means un-prefixed order classes apply at all sizes, while prefixed classes override from that breakpoint upward:

```html
<!-- Default order on mobile, custom order on medium+ -->
<div class="d-flex flex-column flex-md-row">
  <div class="p-3 order-2 order-md-1 bg-primary text-white">Main content</div>
  <div class="p-3 order-1 order-md-2 bg-success text-white">Sidebar</div>
</div>

<!-- Complex responsive reordering -->
<div class="d-flex flex-column flex-xl-row">
  <header class="order-3 order-xl-1 bg-dark text-white p-3">Header (bottom mobile, left desktop)</header>
  <main class="order-1 order-xl-2 bg-light p-3 flex-grow-1">Main (top mobile, center desktop)</main>
  <footer class="order-2 order-xl-3 bg-secondary text-white p-3">Footer (middle mobile, right desktop)</footer>
</div>
```

This responsive ordering capability is one of flexbox's most powerful features for creating adaptive layouts without duplicating markup.
