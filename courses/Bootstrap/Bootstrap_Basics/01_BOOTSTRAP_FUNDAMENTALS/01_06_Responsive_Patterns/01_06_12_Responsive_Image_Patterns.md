---
title: "Responsive Image Patterns"
lesson: "01_06_12"
difficulty: "2"
topics: ["images", "picture-element", "srcset", "art-direction", "lazy-loading"]
estimated_time: "30 minutes"
---

# Responsive Image Patterns

## Overview

Responsive images serve appropriately sized files to different devices, reducing bandwidth on mobile and providing crisp images on high-DPI displays. Bootstrap provides `.img-fluid` for CSS-based responsive scaling, while HTML5's `<picture>` element, `srcset`, and `sizes` attributes enable server-side image selection. Art direction uses different image crops at different breakpoints, and `loading="lazy"` defers offscreen image downloads. Combining these techniques creates fast, visually appropriate image experiences across all devices.

Bootstrap's `.img-fluid` class sets `max-width: 100%` and `height: auto`, preventing overflow while maintaining aspect ratio. This CSS-only approach does not reduce file size - the browser still downloads the full-resolution image.

## Basic Implementation

### Basic Fluid Image

```html
<!-- Scales to container width, maintains aspect ratio -->
<img src="large-photo.jpg" class="img-fluid" alt="Responsive image">
```

### Thumbnail Variant

```html
<!-- Adds a border and padding -->
<img src="photo.jpg" class="img-thumbnail" alt="Thumbnail image">
```

### Figure with Caption

```html
<figure class="figure">
  <img src="photo.jpg" class="figure-img img-fluid rounded" alt="Descriptive alt text">
  <figcaption class="figure-caption text-center">Image description</figcaption>
</figure>
```

## Advanced Variations

### srcset with Size Descriptors

```html
<!-- Different resolutions for different DPI displays -->
<img
  src="photo-800.jpg"
  srcset="photo-400.jpg 400w,
          photo-800.jpg 800w,
          photo-1200.jpg 1200w,
          photo-1600.jpg 1600w"
  sizes="(max-width: 576px) 100vw,
         (max-width: 992px) 50vw,
         33vw"
  class="img-fluid"
  alt="Responsive image with srcset">
```

### Picture Element for Art Direction

```html
<!-- Different crops for different viewports -->
<picture>
  <!-- Mobile: square crop -->
  <source media="(max-width: 575px)" srcset="hero-square.jpg">
  <!-- Tablet: 16:9 crop -->
  <source media="(max-width: 991px)" srcset="hero-16x9.jpg">
  <!-- Desktop: ultrawide crop -->
  <source media="(min-width: 992px)" srcset="hero-ultrawide.jpg">
  <!-- Fallback -->
  <img src="hero-16x9.jpg" class="img-fluid" alt="Hero image">
</picture>
```

### Format Fearing with Picture

```html
<!-- Serve WebP with JPEG fallback -->
<picture>
  <source type="image/webp" srcset="photo.webp">
  <source type="image/jpeg" srcset="photo.jpg">
  <img src="photo.jpg" class="img-fluid" alt="Photo">
</picture>
```

### Lazy Loading

```html
<!-- Defer loading of offscreen images -->
<img src="below-fold.jpg" class="img-fluid" alt="Below fold content" loading="lazy" width="800" height="600">

<!-- Eager loading for above-fold (default) -->
<img src="hero.jpg" class="img-fluid" alt="Hero" loading="eager">

<!-- Native lazy with decode hint -->
<img src="article.jpg" class="img-fluid" alt="Article image" loading="lazy" decoding="async">
```

### Responsive Images in Bootstrap Grid

```html
<div class="row">
  <div class="col-12 col-md-6 col-lg-4">
    <img src="photo.jpg" class="img-fluid rounded" alt="Gallery image 1"
         srcset="photo-400.jpg 400w, photo-800.jpg 800w"
         sizes="(max-width: 768px) 100vw, (max-width: 992px) 50vw, 33vw"
         loading="lazy">
  </div>
  <div class="col-12 col-md-6 col-lg-4">
    <img src="photo2.jpg" class="img-fluid rounded" alt="Gallery image 2"
         loading="lazy">
  </div>
</div>
```

## Best Practices

1. **Always set `alt` text** - Essential for screen readers and SEO.
2. **Use `.img-fluid` for CSS-responsive images** - Prevents container overflow.
3. **Use `srcset` for resolution switching** - Serves smaller files to smaller screens.
4. **Use `<picture>` for art direction** - Different crops at different breakpoints.
5. **Set `width` and `height` attributes** - Prevents Cumulative Layout Shift (CLS).
6. **Use `loading="lazy"` for below-fold images** - Defers download until near viewport.
7. **Use `loading="eager"` (or omit) for above-fold images** - Hero images should load immediately.
8. **Specify `sizes` attribute with `srcset`** - Tells the browser which image to request before layout is known.
9. **Serve modern formats (WebP/AVIF) with fallbacks** - Use `<picture>` with multiple `<source>` elements.
10. **Compress images before uploading** - Tools like Sharp, Squoosh, or ImageOptim reduce file size.
11. **Use descriptive, unique `alt` text** - "Photo of a sunset over the ocean" not "image1.jpg".
12. **Test with network throttling** - Ensure images load acceptably on 3G connections.

## Common Pitfalls

1. **Using `.img-fluid` without `width`/`height`** - Causes layout shift as images load.
2. **Relying on CSS `max-width` alone without `srcset`** - Mobile downloads the full desktop image.
3. **Forgetting `sizes` attribute with `srcset`** - Browser guesses and may choose wrong image.
4. **Using `loading="lazy"` on hero images** - Delays Largest Contentful Paint (LCP).
5. **Not providing fallback `<img>` inside `<picture>`** - Breaks in older browsers.

## Accessibility Considerations

Every `<img>` must have an `alt` attribute. Decorative images should use `alt=""` and `role="presentation"`. Art-directed images via `<picture>` need the same `alt` text across all sources. When using responsive images to show different text crops, ensure the `alt` text remains accurate for all variants. SVG images need `role="img"` and `<title>` elements for screen readers.

## Responsive Behavior

`.img-fluid` handles CSS-level responsiveness: images scale down with their container but never exceed their natural width. `srcset` handles server-level responsiveness: the browser selects the appropriate image file based on viewport width and device pixel ratio. `<picture>` handles art-direction: entirely different images load at different breakpoints. Combining all three creates the optimal responsive image experience - correct crop, correct size, correct format for every device.
