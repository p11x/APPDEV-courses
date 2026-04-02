---
title: "Responsive Images & Media"
module: "Responsive Patterns"
lesson: "01_06_06"
difficulty: 2
estimated_time: "20 minutes"
tags: [images, media, img-fluid, picture, srcset, video, art-direction, responsive]
prerequisites:
  - "01_06_01_Breakpoint_System"
  - "01_06_03_Responsive_Grid_Columns"
---

# Responsive Images & Media

## Overview

Responsive images and media adapt their dimensions, resolution, and sometimes content to the viewport and device capabilities. Bootstrap provides utility classes like `img-fluid` for basic responsive behavior, but building truly responsive media requires combining Bootstrap utilities with native HTML features such as the `<picture>` element, `srcset` attributes, and responsive embed utilities.

On mobile devices, loading a 2000px-wide hero image wastes bandwidth and slows page rendering. On retina displays, a 750px image looks blurry. Responsive image strategies solve both problems: they serve appropriately sized images to each device and art-direct different crops or compositions for different viewport contexts.

Bootstrap's approach to responsive images is pragmatic. The `img-fluid` class applies `max-width: 100%` and `height: auto`, ensuring images never overflow their containers while maintaining aspect ratio. For responsive video and iframe embeds, Bootstrap provides ratio wrapper classes. For art direction and resolution switching, you combine Bootstrap with HTML5's native `<picture>` and `srcset` features.

---

## Basic Implementation

The `img-fluid` class is the simplest responsive image pattern. It constrains the image to its container's width and scales its height proportionally.

**Example 1: Basic fluid image**

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-8">
      <img src="hero.jpg" alt="Hero image"
           class="img-fluid w-100 rounded">
    </div>
    <div class="col-12 col-md-4">
      <p>Image scales with its column. On mobile, it fills
         the full width. On md+, it occupies col-md-8.</p>
    </div>
  </div>
</div>
```

The `img-fluid` class ensures the image never exceeds its column width. Combined with `w-100`, it forces the image to fill the column completely. Without `w-100`, the image stays at its natural size up to the column width.

**Example 2: Responsive video embed with ratio wrapper**

```html
<!-- 16:9 video -->
<div class="ratio ratio-16x9">
  <iframe src="https://www.youtube.com/embed/dQw4w9WgXcQ"
          title="Video player"
          allowfullscreen>
  </iframe>
</div>

<!-- 4:3 video -->
<div class="ratio ratio-4x3">
  <iframe src="https://www.youtube.com/embed/dQw4w9WgXcQ"
          title="Video player"
          allowfullscreen>
  </iframe>
</div>
```

The `ratio` class with `ratio-16x9` creates a responsive container that maintains the 16:9 aspect ratio at any width. The `<iframe>` inside uses `position: absolute` and fills the container. This eliminates the need for padding-bottom hacks that were common in older responsive frameworks.

**Example 3: Responsive figure with caption**

```html
<figure class="figure">
  <img src="chart.png" alt="Sales chart for Q4 2025"
       class="figure-img img-fluid rounded">
  <figcaption class="figure-caption text-center">
    Q4 2025 sales data across all regions.
    Source: Internal analytics.
  </figcaption>
</figure>
```

Bootstrap's `figure` and `figure-caption` classes style semantic `<figure>` elements. The `img-fluid` class keeps the image responsive, while `figure-caption` applies muted text styling. The `<figure>` element is the correct semantic container for images with captions.

---

## Advanced Variations

**Example 4: Art direction with the `<picture>` element**

```html
<picture>
  <!-- Large desktop: wide landscape crop -->
  <source media="(min-width: 1200px)"
          srcset="hero-desktop-wide.webp"
          type="image/webp">
  <source media="(min-width: 1200px)"
          srcset="hero-desktop-wide.jpg">

  <!-- Tablet: medium crop -->
  <source media="(min-width: 768px)"
          srcset="hero-tablet.webp"
          type="image/webp">
  <source media="(min-width: 768px)"
          srcset="hero-tablet.jpg">

  <!-- Mobile: square crop, focused subject -->
  <source srcset="hero-mobile.webp"
          type="image/webp">
  <img src="hero-mobile.jpg"
       alt="Product showcase"
       class="img-fluid w-100">
</picture>
```

The `<picture>` element serves different image files based on media queries. Each `<source>` specifies a `media` attribute that matches Bootstrap's breakpoints. WebP sources provide smaller file sizes with a JPG fallback for older browsers. Art direction ensures the mobile crop focuses on the product, while the desktop crop shows the full scene.

**Example 5: Resolution switching with srcset**

```html
<img src="product-400.jpg"
     srcset="product-400.jpg 400w,
             product-800.jpg 800w,
             product-1200.jpg 1200w,
             product-2400.jpg 2400w"
     sizes="(max-width: 575px) 100vw,
            (max-width: 991px) 50vw,
            33vw"
     alt="Product photo"
     class="img-fluid">
```

The `srcset` attribute lists available image widths. The `sizes` attribute tells the browser how wide the image will display at each breakpoint. On mobile (`max-width: 575px`), the image is `100vw` (full viewport width), so the browser selects `product-1200.jpg` or `product-2400.jpg` for retina displays. At `md`, the image is `50vw`, so `product-800.jpg` suffices. At `lg`+, the image is `33vw`, making `product-400.jpg` adequate for standard displays.

**Example 6: Responsive background images with CSS**

```html
<style>
  .hero-responsive {
    background-image: url('hero-mobile.jpg');
    background-size: cover;
    background-position: center;
    min-height: 300px;
  }

  @media (min-width: 768px) {
    .hero-responsive {
      background-image: url('hero-tablet.jpg');
      min-height: 450px;
    }
  }

  @media (min-width: 1200px) {
    .hero-responsive {
      background-image: url('hero-desktop.jpg');
      min-height: 600px;
    }
  }
</style>

<section class="hero-responsive d-flex align-items-center">
  <div class="container">
    <h1 class="text-white display-5">Welcome</h1>
  </div>
</section>
```

CSS background images cannot use `srcset`, so media queries serve different images at each breakpoint. The mobile image loads by default (mobile-first). Larger images are requested only when the viewport matches the media query. The `min-height` increases progressively for larger backgrounds.

**Example 7: Responsive image gallery with varying layouts**

```html
<div class="row g-2 g-md-3">
  <!-- Featured image: full width on mobile, 2/3 on desktop -->
  <div class="col-12 col-lg-8">
    <img src="gallery-featured.jpg" alt="Featured photo"
         class="img-fluid w-100 rounded"
         style="object-fit: cover; height: 100%;">
  </div>

  <!-- Side thumbnails: stack on mobile, 1/3 column on desktop -->
  <div class="col-12 col-lg-4">
    <div class="row g-2 g-md-3 h-100">
      <div class="col-6 col-lg-12">
        <img src="gallery-thumb-1.jpg" alt="Thumbnail 1"
             class="img-fluid w-100 rounded"
             style="object-fit: cover; height: 200px;">
      </div>
      <div class="col-6 col-lg-12">
        <img src="gallery-thumb-2.jpg" alt="Thumbnail 2"
             class="img-fluid w-100 rounded"
             style="object-fit: cover; height: 200px;">
      </div>
    </div>
  </div>
</div>
```

This gallery uses Bootstrap's grid to create different layouts at each breakpoint. On mobile, the featured image is full-width, followed by two half-width thumbnails side by side. On `lg`, the featured image takes 8 columns and the thumbnails stack vertically in a 4-column sidebar. `object-fit: cover` ensures images fill their containers without distortion.

---

## Best Practices

1. **Always use `img-fluid` on images that should scale with their container.** This prevents images from overflowing on mobile and maintains aspect ratio during scaling.

2. **Always include `alt` text on images.** Descriptive `alt` text is essential for screen readers. Decorative images should use `alt=""` to be skipped by assistive technology.

3. **Use `<picture>` for art direction — serving different crops at different breakpoints.** The `<picture>` element is ideal when the composition or aspect ratio needs to change, not just the resolution.

4. **Use `srcset` with `sizes` for resolution switching — serving different file sizes at the same composition.** When the image content is the same but file sizes differ, `srcset` lets the browser choose the optimal file.

5. **Serve WebP with JPG/PNG fallbacks.** WebP provides 25-35% smaller files than JPEG at equivalent quality. Use `<source type="image/webp">` before the `<img>` fallback.

6. **Use `object-fit: cover` to fill fixed-height containers without distortion.** Without `object-fit`, images in fixed-height containers stretch or squish. `cover` fills the container while cropping excess.

7. **Lazy-load images below the fold with `loading="lazy"`.** The native `loading="lazy"` attribute defers loading until the image is near the viewport. This improves initial page load on mobile.

8. **Use the `ratio` class for responsive video and iframe embeds.** Bootstrap's ratio utility is cleaner and more maintainable than manual padding-bottom hacks.

9. **Define image dimensions (`width` and `height` attributes`) to prevent layout shifts.** Setting intrinsic dimensions allows the browser to reserve space before the image loads, preventing cumulative layout shift (CLS).

10. **Compress images before serving them.** Even responsive images waste bandwidth if uncompressed. Tools like Squoosh, ImageOptim, or build-time compression plugins reduce file sizes significantly.

11. **Use `figure` and `figure-caption` for images with descriptive captions.** These semantic HTML elements are styled by Bootstrap and improve document structure.

12. **Avoid using images for text content.** Text in images is inaccessible to screen readers, not searchable, and not translatable. Use HTML text with CSS styling instead.

---

## Common Pitfalls

**Pitfall 1: Using `img-fluid` without a container constraint.**
`img-fluid` applies `max-width: 100%`, but if the parent has no width constraint, the image displays at its natural size. Ensure the image is inside a grid column or a container with a defined width.

**Pitfall 2: Forgetting the `sizes` attribute with `srcset`.**
Without `sizes`, the browser assumes the image will be `100vw`, selecting unnecessarily large files for images displayed in narrow columns. Always pair `srcset` with `sizes` for accurate file selection.

**Pitfall 3: Using `<picture>` for resolution switching only.**
The `<picture>` element is designed for art direction (different crops/compositions). For simple resolution switching (same image, different sizes), `srcset` with `sizes` is more appropriate and semantically correct.

**Pitfall 4: Setting fixed heights on responsive images without `object-fit`.**
`height: 300px` on an `img-fluid` image distorts the aspect ratio at different widths. Add `object-fit: cover` or `object-fit: contain` to maintain proportions.

**Pitfall 5: Not providing fallback formats for modern image types.**
WebP and AVIF are not supported in all browsers. Always include a JPG or PNG fallback as the last `<source>` or as the `src` attribute on `<img>`.

**Pitfall 6: Loading all images eagerly.**
Images below the fold should use `loading="lazy"` to defer loading. Without this, the browser downloads every image on page load, including images the user may never scroll to.

**Pitfall 7: Using `img-fluid` on images inside flex containers without `flex-shrink: 0`.**
In flex layouts, images can shrink below their natural size even with `img-fluid`. If the image should not shrink, add `flex-shrink: 0` or set a `min-width`.

---

## Accessibility Considerations

Every `<img>` element must have an `alt` attribute. Informative images need descriptive `alt` text that conveys the image's content and purpose. Decorative images use `alt=""` to be announced as presentational by screen readers.

Video embeds require accessible alternatives. Provide captions for hearing-impaired users and transcripts for screen reader users. The `<iframe>` should have a descriptive `title` attribute so screen readers announce its purpose before entering the embedded content.

Responsive images that serve different crops at different breakpoints should maintain the same semantic content across all breakpoints. Art direction changes the composition, not the subject. A mobile crop of a team photo should still show the team, not an unrelated image.

Images with overlaid text must have sufficient contrast between the text and the underlying image. Responsive images may change contrast ratios as they scale. Use text shadows, semi-transparent overlays, or solid background patches to maintain readability at all sizes.

---

## Responsive Behavior

`img-fluid` images scale proportionally with their container. In a `col-md-6` column, the image is 50% of the row width at `md` and above. Below `md`, it reverts to full width if the column collapses to `col-12`.

The `<picture>` element serves images based on its `<source>` media queries, not Bootstrap's breakpoints. These media queries should be aligned with Bootstrap's breakpoints for consistent behavior. Misaligned queries can serve the wrong image at a given viewport width.

Responsive embeds using the `ratio` class maintain their aspect ratio at all widths. The content inside the embed (video player, map, etc.) scales with the container. This ensures that embedded media never overflows or creates horizontal scrollbars.

Background images defined in CSS change at media query thresholds. Since CSS media queries are evaluated on the client, the browser may download multiple background images as the viewport resizes. Use `prefers-reduced-motion` and careful media query design to minimize unnecessary downloads.