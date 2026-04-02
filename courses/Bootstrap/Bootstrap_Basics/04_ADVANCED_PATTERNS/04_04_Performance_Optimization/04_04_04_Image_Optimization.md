---
title: "Image Optimization"
module: "Performance Optimization"
difficulty: 2
duration: "25 minutes"
prerequisites: ["HTML images", "Bootstrap responsive utilities"]
tags: ["images", "responsive", "lazy-loading", "webp", "performance"]
---

# Image Optimization

## Overview

Images often account for the largest portion of page weight. Optimizing images with responsive sizing, modern formats (WebP/AVIF), lazy loading, and proper Bootstrap integration can reduce image payloads by 50-80%, dramatically improving page load performance and Core Web Vitals scores.

## Basic Implementation

Use Bootstrap's responsive image classes with lazy loading:

```html
<!-- Responsive image with lazy loading -->
<img
  src="photo-800.jpg"
  srcset="photo-400.jpg 400w,
          photo-800.jpg 800w,
          photo-1200.jpg 1200w"
  sizes="(max-width: 576px) 100vw,
         (max-width: 992px) 50vw,
         33vw"
  alt="Descriptive alt text"
  class="img-fluid rounded"
  loading="lazy"
  decoding="async"
  width="800"
  height="600"
>
```

Use `<picture>` for format fallbacks:

```html
<picture>
  <source
    srcset="photo.avif"
    type="image/avif"
  >
  <source
    srcset="photo.webp"
    type="image/webp"
  >
  <img
    src="photo.jpg"
    alt="Fallback format"
    class="img-fluid"
    loading="lazy"
    width="800"
    height="600"
  >
</picture>
```

```css
/* Ensure images maintain aspect ratio */
.img-fluid {
  max-width: 100%;
  height: auto;
}

/* Prevent layout shift */
img[width][height] {
  aspect-ratio: attr(width) / attr(height);
}
```

## Advanced Variations

Combine responsive images with Bootstrap grid:

```html
<div class="row g-4">
  <div class="col-md-6 col-lg-4">
    <div class="card h-100">
      <picture>
        <source srcset="thumb-1.avif" type="image/avif">
        <source srcset="thumb-1.webp" type="image/webp">
        <img
          src="thumb-1.jpg"
          alt="Card image"
          class="card-img-top"
          loading="lazy"
          width="400"
          height="300"
        >
      </picture>
      <div class="card-body">
        <h5 class="card-title">Card Title</h5>
      </div>
    </div>
  </div>
</div>
```

Implement hero image with priority loading:

```html
<!-- Hero: above-fold = eager loading + preload -->
<link rel="preload" as="image" href="hero-1200.webp"
      imagesrcset="hero-800.webp 800w, hero-1200.webp 1200w"
      imagesizes="100vw">

<section class="hero-section position-relative">
  <picture>
    <source srcset="hero-1200.avif" type="image/avif">
    <source srcset="hero-1200.webp" type="image/webp">
    <img
      src="hero-1200.jpg"
      alt="Hero background"
      class="w-100"
      style="object-fit: cover; height: 60vh;"
      fetchpriority="high"
      decoding="async"
      width="1200"
      height="675"
    >
  </picture>
</section>
```

Integrate with image CDN for automatic optimization:

```html
<!-- Cloudinary integration -->
<img
  src="https://res.cloudinary.com/demo/image/upload/w_800,q_auto,f_auto/sample.jpg"
  srcset="https://res.cloudinary.com/demo/image/upload/w_400,q_auto,f_auto/sample.jpg 400w,
          https://res.cloudinary.com/demo/image/upload/w_800,q_auto,f_auto/sample.jpg 800w,
          https://res.cloudinary.com/demo/image/upload/w_1200,q_auto,f_auto/sample.jpg 1200w"
  sizes="(max-width: 576px) 100vw, 50vw"
  alt="CDN optimized image"
  class="img-fluid"
  loading="lazy"
>

<!-- imgix integration -->
<img
  src="https://example.imgix.net/photo.jpg?w=800&auto=format&fit=max"
  alt="imgix optimized"
  class="img-fluid rounded"
  loading="lazy"
>
```

## Best Practices

1. Always specify `width` and `height` attributes to prevent layout shift
2. Use `loading="lazy"` for below-the-fold images
3. Use `fetchpriority="high"` for above-the-fold hero images
4. Provide WebP/AVIF formats with JPG fallback via `<picture>`
5. Set appropriate `srcset` and `sizes` for responsive images
6. Use `img-fluid` class for responsive behavior
7. Compress images before uploading (TinyPNG, Squoosh)
8. Preload critical above-the-fold images
9. Use descriptive alt text for accessibility
10. Implement image CDN for automatic format/quality optimization
11. Use CSS `object-fit` for proper image cropping
12. Monitor image performance with Lighthouse

## Common Pitfalls

1. Missing `width`/`height` attributes causing Cumulative Layout Shift
2. Lazy loading above-the-fold images (delays LCP)
3. Using `srcset` without `sizes` (browser can't choose correctly)
4. Serving oversized images to small viewports
5. Not providing fallback `<img>` inside `<picture>`
6. Forgetting `alt` text on images
7. Using images for text content instead of CSS/HTML
8. Not preloading hero images

## Accessibility Considerations

- Always include descriptive `alt` text for meaningful images
- Use `alt=""` for decorative images
- Ensure sufficient color contrast in image overlays
- Provide text alternatives for infographics
- Test with screen readers for proper image descriptions

## Responsive Behavior

- Verify images scale correctly at all breakpoints
- Test `srcset` image selection with DevTools
- Ensure images don't overflow containers on mobile
- Validate `sizes` attribute matches actual layout
