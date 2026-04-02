---
title: "Figures"
course: "Bootstrap Basics"
module: "02_COMPONENT_SYSTEM"
lesson: "02_06_Media_Components"
file: "02_06_03_Figures"
difficulty: 1
framework_version: "Bootstrap 5.3"
tags: [figures, figure-caption, image-caption, semantic-html, alignment]
prerequisites:
  - "02_06_01_Responsive_Images"
  - "02_06_02_Image_Shapes"
description: "Learn to use Bootstrap's figure component for displaying images with captions, including text alignment, responsive patterns, and semantic HTML best practices."
---

## Overview

The `<figure>` element is a semantic HTML5 tag designed to wrap self-contained content like images, illustrations, diagrams, or code snippets, along with an optional `<figcaption>`. Bootstrap enhances this with the `.figure` container class, `.figure-img` for consistent image sizing, and `.figure-caption` for styled captions. This component is particularly useful in documentation, blog posts, portfolios, and editorial layouts where images need descriptive text directly beneath or beside them.

Using figures instead of a plain `<img>` with a `<p>` tag below it provides better semantics for assistive technologies and search engines, and ensures consistent styling across your project.

## Basic Implementation

The simplest figure consists of an image and a caption:

```html
<figure class="figure">
  <img src="images/chart-quarterly.png" class="figure-img img-fluid rounded" alt="Quarterly revenue chart">
  <figcaption class="figure-caption">Figure 1: Quarterly revenue growth for FY2025.</figcaption>
</figure>
```

The `.figure-img` class ensures the image maintains reasonable sizing, and `.figure-caption` applies muted text styling.

Align the caption to the right or center using text utilities:

```html
<figure class="figure">
  <img src="images/diagram-architecture.png" class="figure-img img-fluid rounded" alt="System architecture diagram">
  <figcaption class="figure-caption text-center">System architecture overview.</figcaption>
</figure>
```

## Advanced Variations

Use `.text-end` for right-aligned captions:

```html
<figure class="figure">
  <img src="images/landscape.jpg" class="figure-img img-fluid rounded" alt="Mountain landscape at sunset">
  <figcaption class="figure-caption text-end">Photo by Jane Doe — Unsplash</figcaption>
</figure>
```

Create a responsive figure grid using Bootstrap's grid system:

```html
<div class="row g-4">
  <div class="col-md-6">
    <figure class="figure">
      <img src="images/before.jpg" class="figure-img img-fluid rounded" alt="Before renovation">
      <figcaption class="figure-caption text-center">Before renovation</figcaption>
    </figure>
  </div>
  <div class="col-md-6">
    <figure class="figure">
      <img src="images/after.jpg" class="figure-img img-fluid rounded" alt="After renovation">
      <figcaption class="figure-caption text-center">After renovation</figcaption>
    </figure>
  </div>
</div>
```

Combine figures with cards for a polished gallery layout:

```html
<div class="card">
  <figure class="figure mb-0">
    <img src="images/project-hero.jpg" class="figure-img img-fluid rounded-top" alt="Project showcase">
    <figcaption class="figure-caption p-3">Project Alpha — Completed March 2025. This initiative reduced processing time by 40%.</figcaption>
  </figure>
</div>
```

Use figures with code blocks for technical documentation:

```html
<figure class="figure">
  <pre class="bg-dark text-light p-3 rounded"><code>const result = await fetch('/api/data');
const json = await result.json();</code></pre>
  <figcaption class="figure-caption">Code sample: Fetching data from the API.</figcaption>
</figure>
```

## Best Practices

1. **Use `<figure>` for images that have captions.** If there is no caption, a regular `<img>` is sufficient.
2. **Always include `.figure-img img-fluid`** together to ensure responsive sizing within the figure container.
3. **Use `.figure-caption`** instead of a plain `<p>` tag for consistent muted styling.
4. **Use `text-center` or `text-end`** on the figcaption for caption alignment rather than custom CSS.
5. **Wrap figures in grid columns** for multi-figure layouts to maintain responsive behavior.
6. **Keep captions concise.** Long descriptions belong in body text, not in `<figcaption>`.
7. **Use semantic numbering** (e.g., "Figure 1:") in captions for documents with cross-references.
8. **Apply `rounded` to `.figure-img`** for visual consistency with Bootstrap's design language.
9. **Use `.mb-0` on the figure** when placing it inside a card to remove default bottom margin.
10. **Combine with `loading="lazy"`** on figure images that appear below the fold.
11. **Avoid nesting interactive elements** (buttons, links) inside `<figcaption>` for accessibility clarity.

## Common Pitfalls

1. **Using `.figure` without `.figure-img`.** The image may not size correctly and can overflow the container.
2. **Placing non-image content as the direct child of `<figure>`** without appropriate classes, causing alignment issues.
3. **Forgetting `img-fluid`** on the image inside a figure, breaking responsive behavior on mobile.
4. **Using `<figure>` for images without any caption.** This adds unnecessary markup; a plain `<img>` with appropriate classes is sufficient.
5. **Overriding `.figure-caption` color** with dark text on dark backgrounds, reducing readability.
6. **Not using grid columns** when placing multiple figures side by side, resulting in poor stacking on mobile.

## Accessibility Considerations

The `<figure>` and `<figcaption>` pairing provides an implicit association between the image and its description, which assistive technologies can leverage. Screen readers announce the caption as associated with the image. For complex images (charts, diagrams), supplement with `aria-describedby` pointing to a hidden `<div>` containing a detailed text alternative. Ensure caption text has sufficient color contrast — Bootstrap's default `.figure-caption` uses `--bs-secondary-color`, which should meet WCAG AA standards in your theme. If the figure contains a decorative image, use `alt=""` and provide the informational content only in the caption.

## Responsive Behavior

Figures inherit responsive behavior from the grid system and the `img-fluid` class. Within a `.col-md-6`, a figure occupies half the row on medium+ screens and full width on smaller screens. The `.figure-caption` wraps naturally as the container narrows. For multi-figure layouts, use responsive grid classes (`col-12 col-md-6 col-lg-4`) so figures reflow appropriately. On very narrow screens, ensure captions remain readable by avoiding overly long text and using `text-center` for balanced visual weight.
