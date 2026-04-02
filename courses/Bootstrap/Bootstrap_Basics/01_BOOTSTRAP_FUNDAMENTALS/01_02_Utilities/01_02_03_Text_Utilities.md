---
tags:
  - bootstrap
  - utilities
  - text
  - typography
  - alignment
category: Bootstrap Fundamentals
difficulty: 1
estimated_time: 25 minutes
---

# Text Utilities

## Overview

Bootstrap 5 text utilities provide a comprehensive set of classes for controlling text alignment, wrapping, breaking, weight, style, decoration, transformation, and line height. These utilities work on any HTML element and follow Bootstrap's consistent naming pattern with responsive breakpoint support.

Text utilities cover the most common typography adjustments without requiring custom CSS. They are designed to be composable — you can combine multiple text classes on a single element to achieve precise typographic control.

Key categories include:

- **Alignment**: `text-start`, `text-center`, `text-end`, `text-sm-center`, etc.
- **Wrapping and breaking**: `text-wrap`, `text-nowrap`, `text-break`
- **Weight and style**: `fw-bold`, `fw-normal`, `fst-italic`, `fst-normal`
- **Transformation**: `text-lowercase`, `text-uppercase`, `text-capitalize`
- **Decoration**: `text-decoration-underline`, `text-decoration-line-through`, `text-decoration-none`
- **Line height**: `lh-1`, `lh-sm`, `lh-base`, `lh-lg`
- **Font family**: `font-monospace`

## Basic Implementation

**Text alignment:**

```html
<p class="text-start">Left-aligned text (default).</p>
<p class="text-center">Center-aligned text.</p>
<p class="text-end">Right-aligned text.</p>
```

**Font weight:**

```html
<p class="fw-bold">Bold text</p>
<p class="fw-bolder">Bolder text (relative to parent)</p>
<p class="fw-semibold">Semibold text</p>
<p class="fw-medium">Medium weight text</p>
<p class="fw-normal">Normal weight text</p>
<p class="fw-light">Light weight text</p>
<p class="fw-lighter">Lighter text (relative to parent)</p>
```

**Font style:**

```html
<p class="fst-italic">Italic text</p>
<p class="fst-normal">Normal (non-italic) text</p>
```

**Text transformation:**

```html
<p class="text-lowercase">LOWERCASED TEXT</p>
<p class="text-uppercase">uppercased text</p>
<p class="text-capitalize">capitalized text — each word starts uppercase</p>
```

**Text decoration:**

```html
<p class="text-decoration-underline">Underlined text</p>
<p class="text-decoration-line-through">Strikethrough text</p>
<p class="text-decoration-none">No decoration (removes default link underline)</p>
```

**Line height:**

```html
<p class="lh-1">Tight line height (1). Good for compact UIs.</p>
<p class="lh-sm">Small line height.</p>
<p class="lh-base">Default line height (1.5).</p>
<p class="lh-lg">Large line height (2). Good for body copy readability.</p>
```

**Monospace font:**

```html
<code class="font-monospace">Monospace text for code or technical content.</code>
<p class="font-monospace">Any text can use monospace.</p>
```

## Advanced Variations

**Responsive text alignment:**

```html
<p class="text-start text-md-center text-lg-end">
  Left on mobile, centered on tablet, right on desktop.
</p>
```

**Text wrapping and overflow:**

```html
<!-- Prevent wrapping -->
<div class="text-nowrap bg-light p-2" style="width: 200px;">
  This long text will not wrap to the next line and will overflow.
</div>

<!-- Force wrapping on normally non-wrapping content -->
<div class="text-wrap" style="width: 150px;">
  superlongwordthatwouldnotnormallybreak
</div>

<!-- Break long words to prevent overflow -->
<div class="text-break" style="width: 150px;">
  antidisestablishmentarianism will break mid-word if needed.
</div>
```

**Combining text utilities:**

```html
<h2 class="text-uppercase fw-bold text-center lh-1">
  Heading with combined utilities
</h2>

<p class="fst-italic text-muted fw-light lh-lg">
  Muted, light, italic paragraph with generous line height for readability.
</p>

<code class="font-monospace text-danger fw-semibold">
  Error code with monospace, bold, and red color.
</code>
```

**Resetting inherited text styles:**

```html
<a href="#" class="text-decoration-none fw-normal text-body">
  Link that looks like plain text.
</a>

<h3 class="fw-normal fst-normal text-start">
  Heading with normal weight and style — looks like a paragraph
  but maintains semantic heading structure.
</h3>
```

**Text utilities with display elements:**

```html
<div class="d-flex justify-content-center">
  <p class="text-center fw-semibold mb-0">
    Centered text inside a flex container.
  </p>
</div>

<span class="d-inline-block text-truncate" style="max-width: 200px;">
  This long text will be truncated with an ellipsis.
</span>
```

## Best Practices

1. **Use `text-start` for LTR content alignment.** In Bootstrap 5, `text-left` is replaced by `text-start` for RTL compatibility. Always use `start`/`end` rather than `left`/`right`.

2. **Combine `text-break` with constrained widths** to prevent long words (URLs, technical terms) from overflowing containers on mobile devices.

3. **Use `fw-bold` instead of `<b>` or `<strong>` when the boldness is purely visual.** Reserve `<strong>` for semantically important content that screen readers should emphasize.

4. **Apply `text-decoration-none` to links** inside navigation components that already indicate interactivity through other visual cues (background changes, borders, icons).

5. **Use `lh-lg` for body text** to improve readability, especially for longer paragraphs. WCAG recommends at least 1.5x line height for body copy.

6. **Use `text-capitalize` judiciously.** It capitalizes every word, which is appropriate for names and titles but incorrect for articles and prepositions in formal text.

7. **Apply `text-truncate` with a parent width constraint** for single-line text overflow. This requires `d-inline-block` or `d-block` plus a `max-width` or container width.

8. **Use responsive alignment for mobile-first design.** Center text on mobile for visual balance, then align left on desktop for readability: `text-center text-md-start`.

9. **Prefer utility classes over custom CSS for typography adjustments.** This keeps styles consistent and makes the design system easier to maintain.

10. **Use `font-monospace` for code, data tables, and technical identifiers.** Monospace fonts improve readability for content where character alignment matters.

11. **Avoid `text-uppercase` on long paragraphs.** All-caps text is harder to read in large blocks. Use it for headings, labels, and short UI strings only.

12. **Test text utilities with different languages.** Transformation utilities like `text-uppercase` may not produce correct results for all locales (e.g., Turkish dotted/dotless I).

## Common Pitfalls

**1. Using `text-break` on elements that also have `text-nowrap`.** These classes directly conflict — `text-nowrap` prevents wrapping, and `text-break` forces word breaking. Only one should be applied.

**2. Confusing `text-decoration-none` with `text-reset`.** `text-decoration-none` removes underlines. `text-reset` inherits the parent's text color. They solve different problems.

**3. Applying `text-truncate` without a width constraint.** `text-truncate` only adds an ellipsis when the text overflows its container. If the container has no width limit, there is nothing to overflow.

**4. Using `text-capitalize` for proper title casing.** `text-capitalize` capitalizes every word, including articles and prepositions ("the", "of", "and"), which is incorrect for formal titles. Use a server-side or JavaScript library for proper title case.

**5. Forgetting responsive alignment overrides.** Setting `text-center` applies to all breakpoints. If you later add `text-md-start`, the center alignment persists below `md`. Always consider the full breakpoint chain.

**6. Overriding semantic meaning with visual styling.** Applying `fw-normal` to a `<strong>` tag removes its visual boldness but the element still conveys importance to screen readers. This can confuse sighted users who rely on visual emphasis cues.

**7. Using `text-lowercase` on user input.** Lowercasing user-entered text (like names or email addresses) can destroy data. Only apply text transformations to display strings you control.

**8. Applying `lh-1` to body text.** A line height of 1 makes lines of text overlap, creating severe readability issues. Reserve `lh-1` for decorative headings or UI elements with very small text.

**9. Neglecting RTL considerations.** Using custom `text-align: left` instead of Bootstrap's `text-start` breaks layouts in right-to-left languages. Always use logical property-based classes.

**10. Relying on `font-monospace` without verifying font stack.** Bootstrap's monospace stack is `"SFMono-Regular", Menlo, Monaco, Consolas, "Liberation Mono"`. If your design specifies a different monospace font, override the CSS variable or font-family.

## Accessibility Considerations

**Text alignment and readability:** Avoid `text-center` or `text-end` on long paragraphs. Centered and right-aligned text creates uneven line starts, making it harder for users with dyslexia or cognitive disabilities to track lines. Reserve center alignment for headings and short content.

**Sufficient text size with line height:** Combine Bootstrap's `fs-*` and `lh-*` utilities to ensure body text is at least `1rem` (16px) with a line height of at least `1.5` (`lh-base` or `lh-lg`). Small text with `lh-1` is inaccessible.

**`text-decoration-none` and link identification:** Removing underlines from links eliminates the most universal visual indicator of interactivity. If you must remove underlines, ensure an alternative indicator exists (color contrast, border-bottom on hover, icon). WCAG 2.1 SC 1.4.1 requires that color is not the only means of conveying information.

**Text transformation and screen readers:** `text-uppercase`, `text-lowercase`, and `text-capitalize` are purely visual — screen readers read the text as it appears in the DOM. A CSS-uppercased word is still read in its original casing. Do not rely on these classes to convey meaning.

**Avoid justified text:** Bootstrap does not provide a `text-justify` utility by default because justified text creates uneven word spacing that reduces readability, especially for users with cognitive disabilities.

## Responsive Behavior

All text alignment utilities support responsive breakpoint prefixes, enabling mobile-first text alignment strategies.

**Responsive alignment:**

```html
<!-- Centered on mobile, left-aligned on medium+ -->
<p class="text-center text-md-start">
  This text adapts its alignment to screen size.
</p>

<!-- Right-aligned on large screens only -->
<p class="text-lg-end">
  Default alignment below lg, right-aligned on lg+.
</p>
```

**Responsive line height:**

```html
<p class="lh-sm lh-md-base lh-lg-lg">
  Tight on small screens, comfortable on medium, generous on large.
</p>
```

**Responsive font weight:**

```html
<span class="fw-normal fw-lg-bold">
  Normal weight on mobile, bold on large screens.
</span>
```

**Practical responsive text pattern:**

```html
<div class="container">
  <h1 class="text-center text-md-start fw-bold text-uppercase">
    Product Name
  </h1>
  <p class="text-center text-md-start lh-lg text-muted">
    Long product description that benefits from centered alignment
    on mobile for visual balance but switches to left alignment on
    desktop for optimal readability in wider columns.
  </p>
  <div class="text-center text-md-end">
    <button class="btn btn-primary">Buy Now</button>
  </div>
</div>
```

This pattern — centered on mobile, start-aligned on desktop — is the most common responsive text pattern in Bootstrap layouts and should be your default approach for headings and body text.
