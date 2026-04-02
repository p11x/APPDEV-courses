---
tags:
  - bootstrap
  - utilities
  - display
  - visibility
  - layout
category: Bootstrap Fundamentals
difficulty: 1
estimated_time: 25 minutes
---

# Display Utilities

## Overview

Bootstrap 5 display utilities map directly to CSS `display` property values, allowing you to control how elements participate in layout flow. These classes eliminate the need to write custom `display` declarations in your stylesheet and follow Bootstrap's consistent naming convention with responsive breakpoint support.

The utility classes mirror the full range of CSS display values: `none`, `inline`, `block`, `inline-block`, `flex`, `inline-flex`, `grid`, `inline-grid`, `table`, `table-row`, `table-cell`, `contents`, and `flow-root`.

The naming convention is `{d}-{value}` or `{d}-{breakpoint}-{value}`:

- `d-none` — hides the element entirely
- `d-block` — renders as a block-level element
- `d-inline` — renders as an inline element
- `d-inline-block` — inline flow with block-level box model control
- `d-flex` — enables flexbox layout
- `d-grid` — enables CSS Grid layout
- `d-table` — renders as a table

Display utilities are fundamental to responsive design — they let you show, hide, or change the layout model of elements at different screen sizes without writing media queries.

## Basic Implementation

**Hiding elements:**

```html
<div class="d-none">This element is hidden on all screen sizes.</div>
<div class="d-block">This element is always visible as a block.</div>
```

**Inline and inline-block:**

```html
<span class="d-inline">Inline (default for spans)</span>
<span class="d-block">Now block-level — takes full width</span>
<div class="d-inline-block" style="width: 200px;">
  Inline-block: inline flow, but you can set width/height.
</div>
```

**Enabling flex layout:**

```html
<div class="d-flex">
  <div>Flex item 1</div>
  <div>Flex item 2</div>
  <div>Flex item 3</div>
</div>
```

**Enabling grid layout:**

```html
<div class="d-grid gap-3">
  <div>Grid item 1</div>
  <div>Grid item 2</div>
</div>
```

**Table display:**

```html
<div class="d-table w-100">
  <div class="d-table-row">
    <div class="d-table-cell p-2">Cell 1</div>
    <div class="d-table-cell p-2">Cell 2</div>
  </div>
</div>
```

**Inline flex and inline grid:**

```html
<div class="d-inline-flex gap-2">
  <span>Inline flex item</span>
  <span>Inline flex item</span>
</div>

<div class="d-inline-grid gap-2">
  <span>Inline grid item</span>
  <span>Inline grid item</span>
</div>
```

## Advanced Variations

**Responsive display toggling:**

The most powerful feature of display utilities is breakpoint-conditional display. You can show an element on mobile and hide it on desktop, or change the display model at different breakpoints.

```html
<!-- Visible only on small screens -->
<div class="d-block d-md-none">Mobile-only content</div>

<!-- Visible only on medium and larger screens -->
<div class="d-none d-md-block">Desktop-only content</div>

<!-- Visible on all screens -->
<div class="d-block">Always visible</div>
```

**Changing layout model per breakpoint:**

```html
<!-- Block on mobile, flex on medium+ -->
<div class="d-block d-md-flex gap-3">
  <div>Stacked on mobile, side-by-side on desktop</div>
  <div>Second item</div>
</div>

<!-- Flex on mobile, grid on large -->
<div class="d-flex d-lg-grid gap-3">
  <div>Layout model changes at lg breakpoint</div>
  <div>Second item</div>
</div>
```

**Flow-root for BFC containment:**

```html
<div class="d-flow-root">
  This element creates a new block formatting context,
  containing floated children without needing a clearfix hack.
  <div class="float-start">Floated left</div>
</div>
```

**Contents display for wrapper elimination:**

```html
<div class="d-contents">
  The element itself doesn't generate a box.
  Children participate directly in the parent's layout context.
</div>
```

**Combining with other utilities:**

```html
<div class="d-flex d-none d-sm-flex justify-content-between align-items-center p-3 bg-light rounded">
  <span>Hidden on xs, flex on sm+ with alignment</span>
  <button class="btn btn-primary">Action</button>
</div>

<div class="d-grid d-md-none">
  <button class="btn btn-outline-primary">Stacked button on mobile</button>
  <button class="btn btn-outline-secondary">Another button</button>
</div>
```

## Best Practices

1. **Use `d-none` and `d-block` for responsive visibility** rather than JavaScript-based show/hide logic. CSS-based hiding is faster and more accessible.

2. **Combine `d-none` with a breakpoint prefix** to create mobile-first show/hide patterns: `d-none d-md-block` hides on mobile, shows on desktop.

3. **Use `d-flex` instead of `float` for layout.** Flexbox provides predictable alignment and distribution without the clearfix problems of floats.

4. **Apply `d-grid` to parent containers** when you need two-dimensional layout control. Pair it with `grid-template-columns` utilities or inline styles.

5. **Avoid `d-none` on interactive elements** that screen readers might still need to access. Use `visually-hidden` instead if the element should be announced but not visible.

6. **Use `d-inline-block` for inline elements that need box model control** — width, height, padding, and margin all sides.

7. **Prefer `d-flex` with gap over manual margin on children** for consistent spacing between flex items.

8. **Use `d-flow-root` to contain floats** instead of the outdated clearfix hack. It creates a new block formatting context cleanly.

9. **Test responsive display changes at every breakpoint.** The `d-none d-md-block` pattern is easy to break if another utility class overrides at a different breakpoint.

10. **Keep display utilities early in the class list** since they fundamentally change the layout model. Other layout utilities depend on the correct display value being set first.

11. **Use `d-contents` sparingly** — browser support is solid in modern browsers but older environments may not handle it correctly.

12. **Combine display utilities with visibility utilities** (`visible`, `invisible`) when you need to preserve layout space but hide content visually.

## Common Pitfalls

**1. Using `d-none` on focusable elements.** If a button or link has `d-none`, it cannot receive keyboard focus. Screen readers may skip it entirely. Use `visually-hidden` when the element needs to remain in the accessibility tree.

**2. Forgetting that `d-block` overrides inline behavior.** Applying `d-block` to a `<span>` makes it take the full width of its parent, which can break inline text flow. Use `d-inline-block` if you need inline flow with block-level sizing.

**3. Overlapping responsive display classes.** Writing `d-none d-block d-md-none d-lg-flex` creates conflicting rules that are hard to debug. Simplify by only specifying the changes at each breakpoint.

**4. Not setting `d-flex` or `d-grid` before using flex/grid utilities.** Flex and grid utilities like `justify-content-between` or `gap-3` require the parent to have `display: flex` or `display: grid` set first.

**5. Hiding content without considering screen readers.** `d-none` removes content from both visual display and the accessibility tree. If the content should be announced by assistive technology, use `aria-live` regions or `visually-hidden` instead.

**6. Using `d-table` for complex layouts.** While Bootstrap provides table display utilities, CSS Grid or Flexbox are better suited for most modern layout needs. `d-table` is primarily useful for email templates or legacy browser support.

**7. Forgetting breakpoint specificity order.** Bootstrap is mobile-first: `d-md-none` means hidden from `md` up, not hidden below `md`. The class `d-none d-md-block` means hidden by default, visible from `md` upward.

**8. Confusing `d-none` with `visibility: hidden`.** `d-none` removes the element from layout flow entirely. `visibility: hidden` (via the `invisible` class) hides the element but preserves its space in the layout.

**9. Applying `d-contents` to elements with backgrounds or borders.** Since `d-contents` removes the element's box, any background, border, padding, or margin on that element will have no visible effect.

**10. Mixing display utilities with conflicting CSS.** If your custom stylesheet sets `display: flex !important` on a class, Bootstrap's `d-none` will be overridden. Avoid `!important` in custom styles to let utility classes work as expected.

## Accessibility Considerations

**Hiding content for all users:** `d-none` removes content from both visual display and assistive technology. This is appropriate for conditionally shown content (like a mobile menu) but not for content that should be available to screen readers.

**Use `visually-hidden` for accessible hiding:**

```html
<!-- Screen readers will announce this, but it's visually hidden -->
<span class="visually-hidden">Close menu</span>

<!-- This is completely hidden from everyone -->
<span class="d-none">Info only shown conditionally</span>
```

**Maintain logical tab order:** When using responsive display utilities to show/hide navigation, ensure the focus order still makes sense. A mobile menu that appears in the DOM before the main content but is `d-none` on desktop should not create confusing focus jumps.

**ARIA attributes with conditional display:** When toggling visibility of regions (like accordions or dropdowns), pair display utilities with `aria-expanded` and `aria-controls` so screen readers understand the relationship:

```html
<button aria-expanded="false" aria-controls="menuContent">Menu</button>
<div id="menuContent" class="d-none" role="region">
  Menu content here
</div>
```

**Respect `prefers-reduced-motion`:** If you animate display changes (e.g., via transition of opacity before toggling `d-none`), respect the user's motion preferences.

## Responsive Behavior

Responsive display utilities are the backbone of Bootstrap's responsive design system. They follow mobile-first conventions.

**Show on mobile only:**

```html
<div class="d-block d-md-none">
  Only visible below the md breakpoint (768px).
</div>
```

**Show on desktop only:**

```html
<div class="d-none d-md-block">
  Visible from md breakpoint (768px) and up.
</div>
```

**Show on tablet only:**

```html
<div class="d-none d-md-block d-lg-none">
  Only visible between md (768px) and lg (992px).
</div>
```

**Responsive flex direction:**

```html
<!-- Stack on mobile, row on desktop -->
<div class="d-flex flex-column flex-md-row gap-3">
  <div class="flex-fill">Column 1</div>
  <div class="flex-fill">Column 2</div>
  <div class="flex-fill">Column 3</div>
</div>
```

**Responsive grid columns:**

```html
<!-- Grid on desktop, block on mobile -->
<div class="d-block d-md-grid" style="grid-template-columns: repeat(3, 1fr); gap: 1rem;">
  <div>Item 1</div>
  <div>Item 2</div>
  <div>Item 3</div>
</div>
```

**Navbar responsive display pattern:**

```html
<nav class="navbar">
  <div class="d-none d-lg-flex">
    Desktop navigation links
  </div>
  <div class="d-flex d-lg-none">
    Mobile hamburger menu
  </div>
</nav>
```

**Sidebar responsive pattern:**

```html
<div class="row">
  <aside class="d-none d-lg-block col-lg-3">
    Sidebar — hidden below lg breakpoint.
  </aside>
  <main class="col-12 col-lg-9">
    Main content — full width on mobile, shares row on desktop.
  </main>
</div>
```

These patterns form the foundation of nearly every responsive Bootstrap layout. Mastering display utilities with breakpoints gives you complete control over what appears and how it behaves across every device size.
