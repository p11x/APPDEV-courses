---
title: Object Fit Utilities
category: Bootstrap Fundamentals
difficulty: 1
time: 15 min
tags: bootstrap5, object-fit, object-position, images, media, utilities
---

## Overview

Bootstrap 5 object-fit utilities control how replaced content like images and videos fit within their container. The CSS `object-fit` property determines whether the content should be resized to fill, contain, cover, or scale down within the element's box. Combined with `object-position` utilities, you can precisely control the focal point of media content. These utilities are essential for creating responsive image galleries, hero sections, and card layouts where images must conform to specific dimensions.

## Basic Implementation

Object-fit utilities mirror the CSS `object-fit` property values directly.

```html
<!-- object-fit-contain: fits within bounds, may have letterboxing -->
<div style="width: 300px; height: 200px;">
  <img src="photo.jpg" class="w-100 h-100 object-fit-contain" alt="Contained image">
</div>

<!-- object-fit-cover: fills bounds, may crop edges -->
<div style="width: 300px; height: 200px;">
  <img src="photo.jpg" class="w-100 h-100 object-fit-cover" alt="Covered image">
</div>

<!-- object-fit-fill: stretches to exact dimensions -->
<div style="width: 300px; height: 200px;">
  <img src="photo.jpg" class="w-100 h-100 object-fit-fill" alt="Filled image">
</div>

<!-- object-fit-scale-down: like contain but never scales up -->
<div style="width: 300px; height: 200px;">
  <img src="photo.jpg" class="w-100 h-100 object-fit-scale-down" alt="Scaled down image">
</div>

<!-- object-fit-none: original size, no resizing -->
<div style="width: 300px; height: 200px; overflow: hidden;">
  <img src="photo.jpg" class="object-fit-none" alt="Original size image">
</div>
```

The `object-fit-contain` and `object-fit-cover` classes are the most commonly used in responsive layouts.

```html
<!-- Card with consistently sized images -->
<div class="row">
  <div class="col-4">
    <div class="card">
      <div style="height: 200px;">
        <img src="photo1.jpg" class="w-100 h-100 object-fit-cover" alt="Card image">
      </div>
      <div class="card-body">
        <p class="card-text">Image fills the area uniformly.</p>
      </div>
    </div>
  </div>
  <div class="col-4">
    <div class="card">
      <div style="height: 200px;">
        <img src="photo2.jpg" class="w-100 h-100 object-fit-cover" alt="Card image">
      </div>
      <div class="card-body">
        <p class="card-text">Same height regardless of source.</p>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

Object-position utilities control the focal point when using `object-fit-cover` or `object-fit-none`.

```html
<!-- Object position: control the focal point -->
<div style="width: 300px; height: 200px;">
  <img src="wide-photo.jpg" class="w-100 h-100 object-fit-cover object-position-top" alt="Top aligned">
</div>

<div style="width: 300px; height: 200px;">
  <img src="wide-photo.jpg" class="w-100 h-100 object-fit-cover object-position-center" alt="Center aligned">
</div>

<div style="width: 300px; height: 200px;">
  <img src="wide-photo.jpg" class="w-100 h-100 object-fit-cover object-position-bottom" alt="Bottom aligned">
</div>
```

Responsive object-fit can be applied conditionally at different breakpoints.

```html
<!-- Video with object-fit -->
<div style="width: 100%; height: 400px;">
  <video class="w-100 h-100 object-fit-cover" autoplay muted loop>
    <source src="background.mp4" type="video/mp4">
  </video>
</div>

<!-- Hero section with positioned background image -->
<div class="position-relative" style="height: 60vh;">
  <img src="hero.jpg" class="w-100 h-100 object-fit-cover object-position-center" alt="Hero">
  <div class="position-absolute top-50 start-50 translate-middle text-white text-center">
    <h1>Hero Content</h1>
  </div>
</div>
```

## Best Practices

1. **Always set both width and height** - Object-fit requires explicit dimensions. Pair with `w-100 h-100` and a sized parent.
2. **Use `object-fit-cover` for cards and galleries** - Cover ensures images fill their container uniformly without distortion.
3. **Use `object-fit-contain` for logos and icons** - Contain preserves aspect ratio without cropping, ideal for brand assets.
4. **Set overflow hidden on the parent** - When using `object-fit-none`, hide overflow to prevent content from spilling out.
5. **Combine with aspect ratio utilities** - Use Bootstrap's `ratio` classes with object-fit for responsive media containers.
6. **Use object-position for focal points** - Direct attention to the important part of an image when using cover mode.
7. **Apply to video elements** - Object-fit works on `<video>` elements for full-width background videos.
8. **Provide alt text** - Ensure images with object-fit still have meaningful alt attributes for accessibility.
9. **Test with various aspect ratios** - Verify that portrait, landscape, and square images all render correctly with your chosen object-fit.
10. **Consider lazy loading** - Use `loading="lazy"` on images with object-fit to improve initial page load performance.

## Common Pitfalls

1. **No effect without dimensions** - Object-fit has no visible effect if the image and container are the same size. The container must have explicit dimensions.
2. **Forgetting `overflow: hidden`** - `object-fit-none` can cause images to overflow their containers without overflow control.
3. **Using object-fit on non-replaced elements** - Object-fit only works on replaced elements like `<img>`, `<video>`, and `<canvas>`, not on `<div>` elements.
4. **Distorted images with `object-fit-fill`** - Fill stretches images to fit, which distorts the aspect ratio. Use only when distortion is acceptable.
5. **Confusing object-fit with background-size** - Object-fit applies to `<img>` elements. For background images, use `background-size: cover` instead.

## Accessibility Considerations

Object-fit utilities are visual presentation tools and do not alter the semantic meaning of images. Always provide descriptive `alt` text for images regardless of how they are visually cropped or positioned. When using `object-fit-cover` on hero images, ensure that critical content is not in the cropped area. If an image conveys important information, consider whether cropping with cover mode removes essential visual details. Provide alternative text descriptions for any content that may be hidden by cropping.

## Responsive Behavior

Object-fit utilities in Bootstrap 5 do not include responsive breakpoint variants by default. The utility applies uniformly at all screen sizes. To achieve responsive object-fit behavior, use custom CSS with media queries or conditionally apply classes with JavaScript. Consider that mobile viewports may crop images differently with `object-fit-cover`, so testing across devices is important. For responsive image containers, combine object-fit with Bootstrap's grid system and aspect ratio utilities to maintain proper proportions across breakpoints.
