---
title: Figures
category: Component System
difficulty: 1
time: 15 min
tags: bootstrap5, figure, images, captions, content-display
---

## Overview

Bootstrap's figure component provides a standardized way to display images with optional captions. The `.figure` wrapper combined with `.figure-img` and `.figure-caption` classes ensures consistent spacing, sizing, and alignment across different contexts. Figures are commonly used in articles, documentation, galleries, and any content layout where images need descriptive text beneath them.

## Basic Implementation

A minimal figure consists of an image inside a `.figure` container with a `.figure-caption`.

```html
<figure class="figure">
  <img src="https://via.placeholder.com/400x300" class="figure-img img-fluid rounded"
    alt="Placeholder image">
  <figcaption class="figure-caption">A caption for the above image.</figcaption>
</figure>
```

The `.figure-img` class applies responsive sizing and margin handling to the image element.

```html
<figure class="figure">
  <img src="https://via.placeholder.com/400x300" class="figure-img img-fluid rounded"
    alt="Responsive figure image">
  <figcaption class="figure-caption text-center">Centered caption text.</figcaption>
</figure>
```

Use utility classes on the caption for text styling and alignment.

```html
<figure class="figure">
  <img src="https://via.placeholder.com/400x300" class="figure-img img-fluid rounded"
    alt="Styled figure">
  <figcaption class="figure-caption text-end text-muted">
    Right-aligned muted caption.
  </figcaption>
</figure>
```

## Advanced Variations

Control figure width with inline styles or utility classes to fit content layouts.

```html
<figure class="figure" style="max-width: 250px;">
  <img src="https://via.placeholder.com/250x180" class="figure-img img-fluid rounded"
    alt="Constrained width figure">
  <figcaption class="figure-caption">This figure is constrained to 250px.</figcaption>
</figure>
```

Combine figures with grid columns for side-by-side image comparisons.

```html
<div class="row">
  <div class="col-md-6">
    <figure class="figure">
      <img src="https://via.placeholder.com/400x300" class="figure-img img-fluid rounded"
        alt="First comparison image">
      <figcaption class="figure-caption text-center">Before</figcaption>
    </figure>
  </div>
  <div class="col-md-6">
    <figure class="figure">
      <img src="https://via.placeholder.com/400x300" class="figure-img img-fluid rounded"
        alt="Second comparison image">
      <figcaption class="figure-caption text-center">After</figcaption>
    </figure>
  </div>
</div>
```

Responsive figures inside flex containers adapt their width automatically.

```html
<div class="d-flex justify-content-center">
  <figure class="figure text-center">
    <img src="https://via.placeholder.com/350x250" class="figure-img img-fluid rounded"
      alt="Flex-centered figure">
    <figcaption class="figure-caption">Centered within a flex container.</figcaption>
  </figure>
</div>
```

## Best Practices

1. Always include `alt` text on figure images for screen reader accessibility.
2. Use `.figure-img` on images to get proper margin spacing below the image.
3. Use `.figure-caption` on `<figcaption>` for consistent caption styling.
4. Apply `.img-fluid` alongside `.figure-img` to ensure responsive scaling.
5. Wrap figures in `.figure` to maintain semantic HTML structure with `<figure>` and `<figcaption>`.
6. Use `text-center`, `text-start`, or `text-end` on the caption for alignment control.
7. Constrain figure width with `max-width` or grid columns rather than fixed pixel widths.
8. Use `.rounded` or `.rounded-*` utilities on the image for border radius control.
9. Place multiple figures in grid rows for side-by-side layouts.
10. Keep captions concise; use short descriptive phrases rather than full paragraphs.
11. Avoid nesting figures inside other `<figure>` elements.

## Common Pitfalls

- Omitting `.img-fluid` causes images to overflow their container on small screens.
- Forgetting `.figure-img` removes the bottom margin, placing the caption directly against the image edge.
- Using `<div>` instead of `<figcaption>` loses semantic meaning and screen reader association.
- Setting a fixed width on `.figure` without `max-width` can cause horizontal overflow on mobile.
- Adding captions without the `.figure-caption` class results in unstyled text that does not match the design system.
- Nesting interactive elements like links inside captions without proper markup confuses assistive technology.

## Accessibility Considerations

The `<figure>` and `<figcaption>` elements provide semantic association between an image and its description. Screen readers announce the caption as a description of the image. Always supply meaningful `alt` text that conveys the image content, and keep captions supplementary rather than redundant. If the image is decorative, use an empty `alt=""` and rely on the caption alone.

## Responsive Behavior

Figures inherit their parent container's width by default. The `.img-fluid` class ensures the image scales proportionally within the figure. On narrow viewports, figures stack vertically if placed in a grid row with `.col-*` classes. Captions wrap naturally when the figure is constrained. Avoid fixed dimensions that prevent the figure from adapting to smaller screens.
