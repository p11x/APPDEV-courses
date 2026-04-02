---
title: Quotations and Blockquotes
category: Component System
difficulty: 1
time: 15 min
tags: bootstrap5, blockquote, quotations, typography, content-display
---

## Overview

Bootstrap provides typography classes for displaying quotations and blockquotes consistently across your application. The `.blockquote` class styles a `<blockquote>` element with appropriate spacing and typography, while `.blockquote-footer` identifies the quotation source. These classes integrate with Bootstrap's utility system for alignment, text styling, and responsive layout adjustments.

Blockquotes are essential for testimonials, article citations, highlighted text excerpts, and any content that references or emphasizes quoted material.

## Basic Implementation

A standard blockquote pairs the `.blockquote` class with a source footer.

```html
<figure>
  <blockquote class="blockquote">
    <p>A well-known quote, contained in a blockquote element.</p>
  </blockquote>
  <figcaption class="blockquote-footer">
    Someone famous in <cite title="Source Title">Source Title</cite>
  </figcaption>
</figure>
```

The `<cite>` element inside the footer provides a semantic link to the source work.

```html
<blockquote class="blockquote">
  <p>Technology is best when it brings people together.</p>
</blockquote>
<p class="blockquote-footer">Matt Mullenweg in <cite title="Blog Post">WordPress Blog</cite></p>
```

Use text alignment utilities to position the blockquote content.

```html
<blockquote class="blockquote text-center">
  <p>Design is not just what it looks like. Design is how it works.</p>
  <footer class="blockquote-footer">Steve Jobs</footer>
</blockquote>
```

## Advanced Variations

Right-aligned blockquotes use `text-end` on the container.

```html
<div class="text-end">
  <blockquote class="blockquote">
    <p>The best way to predict the future is to invent it.</p>
    <footer class="blockquote-footer">Alan Kay</footer>
  </blockquote>
</div>
```

Add borders and styling with utility classes for visual emphasis.

```html
<blockquote class="blockquote border-start border-4 border-primary ps-3">
  <p class="mb-0">Simplicity is the ultimate sophistication.</p>
  <footer class="blockquote-footer mt-2">Leonardo da Vinci</footer>
</blockquote>
```

Combine blockquotes with card components for styled quote cards.

```html
<div class="card text-center">
  <div class="card-body">
    <figure>
      <blockquote class="blockquote">
        <p>In the middle of difficulty lies opportunity.</p>
      </blockquote>
      <figcaption class="blockquote-footer">
        Albert Einstein in <cite title="Book Title">The World as I See It</cite>
      </figcaption>
    </figure>
  </div>
</div>
```

Display quotation marks explicitly with inline styles when needed.

```html
<blockquote class="blockquote">
  <p class="mb-0">"The only way to do great work is to love what you do."</p>
</blockquote>
<p class="blockquote-footer">Steve Jobs</p>
```

## Best Practices

1. Use `<blockquote>` as the semantic element for quotations, not a styled `<div>`.
2. Wrap blockquote content in `<p>` tags for proper paragraph spacing.
3. Include `.blockquote-footer` inside `<footer>` or `<figcaption>` for source attribution.
4. Use `<cite title="...">` to provide a machine-readable reference to the source.
5. Apply `text-center` or `text-end` at the container level for alignment, not on the blockquote itself.
6. Pair border utilities (`border-start`, `border-4`) with blockquotes for visual emphasis.
7. Use blockquotes inside cards for testimonial or quote card designs.
8. Keep blockquote text concise; move lengthy citations to dedicated content sections.
9. Maintain consistent spacing with margin utilities (`mb-0`, `mt-2`) when adjusting layout.
10. Avoid using blockquotes for indentation purposes; they carry semantic meaning for quoted content.

## Common Pitfalls

- Using a styled `<div>` instead of `<blockquote>` loses semantic meaning and screen reader context.
- Placing the footer text directly inside the blockquote element rather than in a `<footer>` or `<figcaption>` breaks the source association.
- Forgetting the `title` attribute on `<cite>` removes the accessible reference to the source work.
- Applying alignment classes directly to `<blockquote>` instead of a wrapper can conflict with block-level display behavior.
- Using blockquotes purely for visual indentation misleads assistive technology about the content's purpose.
- Nesting blockquotes inside other blockquotes creates confusing hierarchy and is not recommended by HTML specifications.

## Accessibility Considerations

Screen readers identify `<blockquote>` elements as quoted content and may announce them differently from regular text. The `<footer>` inside a blockquote helps associate the quote with its source. The `<cite>` element provides a reference to the original work. Avoid using blockquotes for non-quoted decorative content, as this misrepresents the content type to assistive technology users.

## Responsive Behavior

Blockquotes are inherently responsive. They fill the width of their container and wrap text naturally. On narrow screens, text alignment classes continue to work correctly. When placing blockquotes inside cards or grid columns, the content adjusts to available space. Avoid fixed widths on blockquotes that could cause horizontal scrolling on small devices.
