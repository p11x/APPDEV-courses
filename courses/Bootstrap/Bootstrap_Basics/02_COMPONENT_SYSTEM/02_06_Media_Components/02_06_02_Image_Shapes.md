---
title: "Image Shapes"
course: "Bootstrap Basics"
module: "02_COMPONENT_SYSTEM"
lesson: "02_06_Media_Components"
file: "02_06_02_Image_Shapes"
difficulty: 1
framework_version: "Bootstrap 5.3"
tags: [images, shapes, rounded, circle, pill, thumbnail, float, alignment]
prerequisites:
  - "02_06_01_Responsive_Images"
description: "Master Bootstrap's image shape utilities including rounded corners, circles, pills, thumbnails, and image float alignment techniques."
---

## Overview

Bootstrap provides a set of utility classes for controlling image shape and alignment. The `rounded` family of classes applies border-radius values ranging from small corners to fully circular shapes. `img-thumbnail` adds a light border for a polished look suitable for gallery previews. Float utilities align images to the left or right, allowing text to wrap around them, a classic pattern in editorial and blog layouts.

These utilities are not limited to images; they work on any element. However, applying them to images is the most common use case and is essential for building avatar components, product thumbnails, and visually balanced page layouts.

## Basic Implementation

Apply rounded corners to any image with the `rounded` class:

```html
<img src="images/avatar.jpg" class="img-fluid rounded" alt="User avatar">
```

Create a perfect circle using `rounded-circle`, ideal for user avatars:

```html
<img src="images/profile.jpg" class="img-fluid rounded-circle" alt="Profile picture" width="80" height="80">
```

The `img-thumbnail` class adds a subtle border and padding:

```html
<img src="images/product-thumb.jpg" class="img-thumbnail" alt="Product thumbnail">
```

Align images to the left or right with float utilities so surrounding text wraps naturally:

```html
<img src="images/author.jpg" class="rounded float-start me-3 mb-2" width="120" alt="Author photo">
<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
```

## Advanced Variations

Use `rounded-pill` for a pill-shaped border radius (border-radius: 50rem), which works well for banner images or badge-style elements:

```html
<img src="images/banner-badge.jpg" class="img-fluid rounded-pill" alt="Promotional banner">
```

Combine size-specific rounded utilities for fine-grained control:

```html
<div class="d-flex gap-3 align-items-center">
  <img src="images/icon-sm.jpg" class="rounded-1" width="32" height="32" alt="Small icon">
  <img src="images/icon-md.jpg" class="rounded-2" width="48" height="48" alt="Medium icon">
  <img src="images/icon-lg.jpg" class="rounded-3" width="64" height="64" alt="Large icon">
  <img src="images/icon-xl.jpg" class="rounded-4" width="80" height="80" alt="Extra large icon">
</div>
```

Float an image to the right with text on the left:

```html
<img src="images/feature.jpg" class="img-fluid rounded float-end ms-3 mb-2" width="200" alt="Feature highlight">
<p class="clearfix">Our latest feature includes advanced analytics, real-time collaboration, and seamless integrations with your existing tools. The dashboard has been redesigned for clarity and speed.</p>
```

Build a responsive avatar group using `rounded-circle` with flexbox:

```html
<div class="d-flex">
  <img src="images/user1.jpg" class="rounded-circle border border-white" width="40" height="40" alt="Team member one" style="margin-right: -10px;">
  <img src="images/user2.jpg" class="rounded-circle border border-white" width="40" height="40" alt="Team member two" style="margin-right: -10px;">
  <img src="images/user3.jpg" class="rounded-circle border border-white" width="40" height="40" alt="Team member three" style="margin-right: -10px;">
  <span class="rounded-circle bg-secondary text-white d-flex align-items-center justify-content-center" style="width: 40px; height: 40px; font-size: 0.75rem;">+5</span>
</div>
```

## Best Practices

1. **Use `rounded-circle` only on square images.** Non-square images will become ellipses rather than circles.
2. **Always set `width` and `height`** on rounded-circle avatars to maintain consistent sizing across browsers.
3. **Use `me-3` or `ms-3`** (Bootstrap spacing utilities) with floated images to add breathing room between image and text.
4. **Add `clearfix`** to the parent container after floated elements to prevent layout collapse.
5. **Combine `img-fluid` with shape classes** so images remain responsive while retaining their shape.
6. **Use `object-fit: cover`** via inline styles or utility classes on circle images to prevent distortion when source aspect ratio differs from display size.
7. **Apply `rounded-3` or `rounded-4`** for card images to match Bootstrap's default card border radius.
8. **Avoid applying `rounded-circle` to images with important edge details**, as cropping will hide them.
9. **Use `img-thumbnail` for interactive gallery items** where the border signals clickability.
10. **Test floated images at multiple breakpoints** to ensure text wrapping looks natural on all screen sizes.
11. **Use `d-block mx-auto`** combined with `rounded` for centered images that should not float.
12. **Prefer CSS `border-radius` utilities** over inline styles for consistency and overridability.

## Common Pitfalls

1. **Using `rounded-circle` on rectangular images.** The result is an oval, not a circle. Always crop or set equal width/height.
2. **Forgetting `clearfix` after float-end or float-start.** Subsequent content may overlap or misalign.
3. **Applying `img-fluid` without a width constraint.** The image may still exceed the intended display area in edge cases.
4. **Using `rounded-pill` on small images.** The pill shape is barely visible and looks like a regular rounded corner.
5. **Not adding margin utilities with floats.** Text crowds the image, reducing readability.
6. **Mixing float and flexbox utilities** on the same element, causing unpredictable layout behavior.
7. **Relying solely on shape classes for responsive behavior.** Always pair with `img-fluid` or explicit dimensions.
8. **Applying `img-thumbnail` to large images** without constraining width, resulting in oversized bordered images.

## Accessibility Considerations

When using images with shape utilities, the `alt` attribute remains the primary accessibility concern. For avatar groups, each image should have an `alt` describing the user (e.g., `alt="Jane Smith profile photo"`). If a floated image is purely decorative, use `alt=""`. When images convey meaning (e.g., a product photo in a description), ensure the `alt` text is descriptive. Rounded shapes do not affect accessibility directly, but verify that overlaid text on images (captions, badges) maintains a contrast ratio of at least 4.5:1 against the image background. Use `aria-label` on containers when grouping related images.

## Responsive Behavior

Shape classes like `rounded`, `rounded-circle`, and `rounded-pill` are not breakpoint-responsive by default in Bootstrap 5. They apply uniformly across all viewport sizes. To control float behavior responsively, use responsive float utilities: `float-md-start` only floats the image on medium+ screens, stacking it above text on mobile. Combine with responsive width utilities (`w-25`, `w-md-auto`) to adjust image size at different breakpoints. On small screens, consider hiding floated images or switching to a stacked layout using `float-none` to prevent cramped text wrapping.
