---
title: Width and Height Utilities
category: Bootstrap Fundamentals
difficulty: 1
time: 20 min
tags: bootstrap5, width, height, sizing, utilities, layout
---

## Overview

Bootstrap 5 provides width and height utility classes to control element dimensions without custom CSS. These utilities include percentage-based sizing (`w-25`, `w-50`, `w-75`, `w-100`), viewport-relative sizing (`vw-100`, `vh-100`), and maximum constraint utilities (`mw-100`, `mh-100`). They are invaluable for creating responsive layouts, constraining images, and building full-viewport sections.

These utilities work with any element and respect Bootstrap's `border-box` model, meaning padding and borders are included in the declared dimensions.

## Basic Implementation

Width utilities set element width as a percentage of the parent container.

```html
<!-- Percentage-based width utilities -->
<div class="w-25 bg-light p-2 mb-2">w-25: 25% width</div>
<div class="w-50 bg-light p-2 mb-2">w-50: 50% width</div>
<div class="w-75 bg-light p-2 mb-2">w-75: 75% width</div>
<div class="w-100 bg-light p-2 mb-2">w-100: 100% width</div>
<div class="w-auto bg-light p-2">w-auto: auto width</div>
```

Height utilities work identically but measure against the parent's height.

```html
<!-- Height utilities with fixed-height container -->
<div class="d-flex" style="height: 200px;">
  <div class="h-25 bg-primary text-white p-2">h-25</div>
  <div class="h-50 bg-secondary text-white p-2">h-50</div>
  <div class="h-75 bg-success text-white p-2">h-75</div>
  <div class="h-100 bg-danger text-white p-2">h-100</div>
</div>
```

Viewport-relative utilities fill the entire browser viewport.

```html
<!-- Viewport-relative sizing -->
<div class="vw-100 bg-warning p-3">
  Full viewport width element
</div>

<div class="vh-100 bg-info d-flex align-items-center justify-content-center">
  Full viewport height section
</div>

<div class="min-vh-100 bg-light">
  Minimum full viewport height (grows with content)
</div>
```

## Advanced Variations

Maximum width and height constraints prevent elements from exceeding their natural size or container boundaries.

```html
<!-- Maximum constraints -->
<img src="large-image.jpg" class="mw-100 mh-100" alt="Constrained image">
<!-- Image will not exceed its container's width or height -->

<!-- Combining width with max-width -->
<div class="w-75 mw-100 bg-light p-3">
  75% wide but never exceeding parent width
</div>
```

Combining width utilities with responsive breakpoints creates adaptive sizing.

```html
<!-- Responsive width -->
<div class="w-100 w-md-75 w-lg-50 mx-auto bg-light p-4">
  Full width on mobile, 75% on tablet, 50% on desktop, centered
</div>

<!-- Full-width image gallery with constrained height -->
<div class="row">
  <div class="col-4">
    <img src="photo.jpg" class="w-100 h-100 object-fit-cover" alt="Gallery image">
  </div>
  <div class="col-4">
    <img src="photo.jpg" class="w-100 h-100 object-fit-cover" alt="Gallery image">
  </div>
  <div class="col-4">
    <img src="photo.jpg" class="w-100 h-100 object-fit-cover" alt="Gallery image">
  </div>
</div>
```

## Best Practices

1. **Use `w-100` for responsive images** - Apply `w-100` to images to make them scale with their container while maintaining aspect ratio.
2. **Combine `mw-100` with `w-auto`** - Prevent large images from overflowing while keeping natural sizing on smaller images.
3. **Use `vh-100` for hero sections** - Create full-viewport hero banners with `vh-100` and flex centering.
4. **Prefer percentage widths over fixed pixels** - Percentage-based utilities like `w-50` adapt to different screen sizes.
5. **Use `min-vh-100` for page layouts** - Ensure the body or main content area fills at least the full viewport height.
6. **Constrain text container widths** - Use `w-75 w-lg-50 mx-auto` on text-heavy sections for optimal line length readability.
7. **Combine with flex utilities** - Pair `h-100` with `d-flex align-items-center` for vertically centered content in fixed-height containers.
8. **Apply `mw-100` to tables** - Prevent wide tables from breaking layouts by constraining their maximum width.
9. **Use viewport units sparingly** - Prefer `min-vh-100` over `vh-100` to allow content to extend beyond the viewport.
10. **Test with real content** - Verify that width/height utilities do not cause overflow or truncation with actual content lengths.

## Common Pitfalls

1. **Forgetting `mw-100` on images** - Without `mw-100`, large images can overflow their containers on small screens.
2. **Using `vh-100` on scrollable content** - `vh-100` fixes the element to viewport height, clipping overflow. Use `min-vh-100` instead.
3. **Height utilities require parent height** - Percentage-based height utilities only work when the parent has an explicit height. Without it, the utility has no effect.
4. **Mixing width utilities with grid columns** - Applying `w-100` to a grid child can conflict with the column width. Use the grid system's sizing instead.
5. **Ignoring box-sizing** - Padding and borders add to the total width in `border-box` mode. A `w-100` element with `p-3` will overflow its container.

## Accessibility Considerations

Width and height utilities primarily affect layout and do not directly impact accessibility. However, ensure that constraining dimensions does not cause text truncation that hides important information. Use `mw-100` to prevent content from extending beyond the viewport, which would require horizontal scrolling. Horizontal scrolling is a significant accessibility barrier, especially for users with motor impairments. Always verify that constrained elements remain fully readable and interactive at all viewport sizes.

## Responsive Behavior

Bootstrap width and height utilities support responsive prefixes. The pattern `w-{breakpoint}-{size}` applies the width utility starting at that breakpoint. For example, `w-100 w-md-50` makes an element full width below the `md` breakpoint and half width at `md` and above. Viewport utilities (`vw-100`, `vh-100`) are inherently responsive since they reference the current viewport dimensions. Combine responsive width utilities with container and grid classes for adaptive layouts that maintain proper proportions across all device sizes.
