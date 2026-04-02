---
title: "Line Height and Spacing"
subtitle: "Controlling line height, letter spacing, and word spacing for optimal typographic rhythm in Bootstrap 5"
category: "Bootstrap Basics"
subcategory: "Typography Engine"
difficulty: 2
duration: "25 minutes"
prerequisites: ["01_04_01_Heading_Typography", "01_04_03_Font_Weight_Style"]
learning_objectives:
  - "Apply Bootstrap 5 line height utilities lh-1, lh-sm, lh-base, and lh-lg"
  - "Control letter spacing with custom CSS integrated into Bootstrap projects"
  - "Implement word spacing adjustments for readability optimization"
  - "Understand the relationship between line height and font size for legibility"
  - "Build consistent vertical rhythm using line height and spacing utilities"
keywords:
  - "line height bootstrap"
  - "lh-1 lh-sm lh-base lh-lg"
  - "letter spacing CSS"
  - "word spacing"
  - "typographic rhythm"
  - "vertical rhythm CSS"
---

# Line Height and Spacing

## Overview

Line height and letter spacing are the invisible forces that determine whether text is comfortable to read or visually exhausting. Line height — the CSS `line-height` property — controls the vertical space between lines of text. Letter spacing — the CSS `letter-spacing` property — controls the horizontal space between individual characters. Word spacing — the CSS `word-spacing` property — controls the space between words. Together, these three properties form the spatial foundation of typography.

Bootstrap 5 provides four line height utilities: `lh-1` (a unitless value of 1, tightly packed), `lh-sm` (small, slightly more space than `lh-1`), `lh-base` (the default line height for the element, typically 1.5 for paragraphs), and `lh-lg` (large, generous vertical spacing). These utilities apply to the element they are placed on and affect all lines within that element.

The line height value directly impacts readability. A line height of 1.0 means each line's height equals the font size, resulting in no extra space between lines. This creates dense, hard-to-read paragraphs. A line height of 1.5 (Bootstrap's default for body text) provides comfortable breathing room. A line height of 2.0 or higher creates loose, airy text suitable for quotes or decorative layouts.

Bootstrap 5 does not include built-in letter spacing or word spacing utilities. These must be implemented with custom CSS. However, Bootstrap's CSS variable system and utility class conventions make it straightforward to add these as project-specific utilities that integrate seamlessly with the framework.

Understanding the interplay between line height, letter spacing, word spacing, and font size is essential for creating typography that feels polished and professional. Tight letter spacing with a large font size creates a commanding display style. Loose letter spacing with a small font size creates an airy, premium feel. The right combination depends on the content type, the font family, and the design intent.

## Basic Implementation

### Line Height Utilities

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <title>Line Height and Spacing</title>
</head>
<body>
  <div class="container py-5">
    <h2 class="mb-4">Line Height Utilities</h2>

    <div class="row g-4">
      <div class="col-md-6">
        <p class="fw-semibold mb-1">lh-1 (Tight)</p>
        <p class="lh-1 border p-3">
          Line height of 1 creates tightly packed text. Each line's height
          matches the font size exactly. This is suitable for display headings
          and compact UI elements but difficult to read for body text.
        </p>
      </div>

      <div class="col-md-6">
        <p class="fw-semibold mb-1">lh-sm (Small)</p>
        <p class="lh-sm border p-3">
          Line height small provides slightly more breathing room than lh-1.
          The vertical space between lines is minimal but present, creating
          text that feels dense yet somewhat readable.
        </p>
      </div>

      <div class="col-md-6">
        <p class="fw-semibold mb-1">lh-base (Default)</p>
        <p class="lh-base border p-3">
          Line height base is the standard for body text. It provides the
          default line height defined by the element's CSS, typically 1.5
          for paragraphs. This is the optimal line height for sustained reading.
        </p>
      </div>

      <div class="col-md-6">
        <p class="fw-semibold mb-1">lh-lg (Large)</p>
        <p class="lh-lg border p-3">
          Line height large creates generous vertical spacing between lines.
          This is useful for quotes, introductory text, or any content where
          visual airiness enhances the reading experience.
        </p>
      </div>
    </div>
  </div>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

### Custom Letter Spacing Utilities

Bootstrap does not include letter spacing utilities by default. Adding them follows Bootstrap's utility pattern:

```html
<head>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <style>
    .ls-tight { letter-spacing: -0.02em; }
    .ls-normal { letter-spacing: 0; }
    .ls-wide { letter-spacing: 0.05em; }
    .ls-wider { letter-spacing: 0.1em; }
    .ls-widest { letter-spacing: 0.2em; }
  </style>
</head>
<body>
  <div class="container py-5">
    <p class="ls-tight fs-4 fw-bold">Tight letter spacing for display text</p>
    <p class="ls-normal">Normal letter spacing for body text.</p>
    <p class="ls-wide text-uppercase small fw-semibold">Wide spacing for labels</p>
    <p class="ls-wider text-uppercase small">Wider spacing for uppercase</p>
    <p class="ls-widest text-uppercase small text-muted">Widest spacing for subtle effect</p>
  </div>
</body>
```

### Custom Word Spacing Utilities

```html
<style>
  .ws-tight { word-spacing: -0.1em; }
  .ws-normal { word-spacing: 0; }
  .ws-wide { word-spacing: 0.25em; }
  .ws-wider { word-spacing: 0.5em; }
</style>

<div class="container py-4">
  <p class="ws-tight">Tight word spacing compresses the gaps between words.</p>
  <p class="ws-normal">Normal word spacing is the default browser behavior.</p>
  <p class="ws-wide">Wide word spacing adds breathing room between words.</p>
  <p class="ws-wider">Wider word spacing creates an airy, spacious feel.</p>
</div>
```

### Line Height on Headings

```html
<div class="container py-4">
  <h1 class="lh-1">Display heading with tight line height for a compact, impactful look</h1>

  <h2 class="lh-base">Section heading with base line height for standard spacing</h2>

  <h3 class="lh-lg">Subsection heading with generous line height for visual openness</h3>
</div>
```

## Advanced Variations

### Vertical Rhythm System

Vertical rhythm ensures that all elements on a page align to a consistent vertical grid, creating visual harmony:

```html
<style>
  :root {
    --rhythm: 1.5rem;
  }

  .rhythm-1 { margin-bottom: var(--rhythm); }
  .rhythm-2 { margin-bottom: calc(var(--rhythm) * 2); }

  p {
    line-height: var(--rhythm);
    margin-bottom: var(--rhythm);
  }

  h2 {
    line-height: calc(var(--rhythm) * 1.5);
    margin-bottom: var(--rhythm);
  }
</style>

<div class="container py-4">
  <h2 class="rhythm-1">Consistent Vertical Rhythm</h2>
  <p>
    This paragraph maintains a vertical rhythm based on the --rhythm variable.
    The line height and margins both align to the same base unit, creating
    visual consistency throughout the document.
  </p>
  <p>
    Second paragraph continues the rhythm. Notice how the spacing between
    elements feels balanced and intentional, not arbitrary.
  </p>
</div>
```

### Responsive Letter Spacing

Letter spacing can be adjusted for different screen sizes:

```html
<style>
  .ls-responsive {
    letter-spacing: 0.15em;
  }
  @media (max-width: 576px) {
    .ls-responsive {
      letter-spacing: 0.05em;
    }
  }
</style>

<h2 class="ls-responsive text-uppercase fw-bold">
  Responsive Letter Spacing
</h2>
```

### Combining Line Height with Typography Utilities

```html
<div class="container py-4">
  <div class="row g-4">
    <div class="col-md-6">
      <div class="card h-100">
        <div class="card-body">
          <h5 class="card-title lh-1 fw-bold">Tight Heading</h5>
          <p class="card-text lh-lg text-muted">
            Body text with generous line height creates contrast against the
            tight heading, drawing the eye to the title while keeping the
            description readable and airy.
          </p>
          <a href="#" class="text-uppercase ls-wide small fw-semibold text-decoration-none">
            Learn More
          </a>
        </div>
      </div>
    </div>

    <div class="col-md-6">
      <div class="card h-100">
        <div class="card-body">
          <h5 class="card-title lh-base fw-semibold">Relaxed Heading</h5>
          <p class="card-text lh-base">
            Body text with standard line height paired with a heading that
            uses base line height. Both elements share the same vertical
            rhythm, creating cohesion.
          </p>
          <a href="#" class="text-uppercase ls-wider small fw-semibold text-decoration-none">
            Learn More
          </a>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Line Height for Code Blocks

Code blocks benefit from increased line height for readability:

```html
<style>
  pre.code-spacious {
    line-height: 1.8;
    font-size: 0.875rem;
  }
</style>

<pre class="code-spacious bg-dark text-light p-4 rounded"><code>function greet(name) {
  return `Hello, ${name}!`;
}

const result = greet('World');
console.log(result);</code></pre>
```

### Letter Spacing for Display Headings

Display headings often benefit from subtle negative or positive letter spacing:

```html
<style>
  .display-tight {
    letter-spacing: -0.03em;
    line-height: 1.1;
  }
  .display-tracking {
    letter-spacing: 0.2em;
    line-height: 1.4;
  }
</style>

<h1 class="display-tight fw-bold display-4">
  Tight Display Heading
</h1>

<p class="display-tracking text-uppercase text-muted fs-6">
  Spaced Out Subtitle
</p>
```

### Paragraph with Optimal Reading Metrics

```html
<style>
  .readable-block {
    line-height: 1.7;
    letter-spacing: 0.01em;
    word-spacing: 0.1em;
    max-width: 65ch;
  }
</style>

<div class="container py-4">
  <p class="readable-block">
    This paragraph combines optimal line height, subtle letter spacing,
    and generous word spacing within a maximum width of 65 characters.
    These metrics create the most comfortable reading experience for
    sustained content consumption.
  </p>
</div>
```

## Best Practices

1. **Use `lh-base` (1.5) for body text paragraphs.** This is the widely accepted optimal line height for body text, providing enough vertical space for comfortable reading without feeling loose.

2. **Use `lh-1` for display headings only.** Tight line height works for large, short headings where lines are few and the compact appearance adds visual impact. Never use `lh-1` on body text.

3. **Use `lh-lg` for introductory text, pull quotes, and testimonials.** Generous line height creates visual airiness that suits content meant to stand apart from dense body text.

4. **Apply negative letter spacing (-0.02em to -0.03em) for large display headings.** Large text at default letter spacing can appear loosely connected. Tightening the spacing at large sizes creates a more cohesive visual word shape.

5. **Apply positive letter spacing (0.05em to 0.2em) for uppercase text.** Uppercase letters have flat tops and bottoms, which makes them appear more tightly spaced than mixed-case text. Adding letter spacing compensates for this optical effect.

6. **Limit word spacing to subtle adjustments (0.1em to 0.5em).** Excessive word spacing breaks the visual connection between words, making sentences harder to parse. Word spacing should enhance readability, not disrupt it.

7. **Use unitless line height values (like Bootstrap's defaults) rather than fixed units.** Unitless values scale proportionally with the font size. A line height of `24px` on `16px` text is fine, but the same `24px` on `32px` text creates negative space. Unitless `1.5` adapts to any font size.

8. **Maintain vertical rhythm by aligning margins and line heights to a common base unit.** If your base unit is `1.5rem`, ensure that paragraph line heights, heading margins, and section spacing are multiples of this unit.

9. **Test line height with your actual font.** Different fonts have different internal metrics (ascender height, descender depth, x-height). A line height of 1.5 may look generous with one font but tight with another.

10. **Use `letter-spacing: 0` (or `ls-normal`) for body text by default.** Additional letter spacing on body text slows reading speed and creates a disconnected feel. Body text should use the font designer's intended spacing.

11. **Create project-specific spacing utilities that follow Bootstrap's naming convention.** Classes like `ls-tight`, `ls-wide`, and `ws-normal` integrate naturally with Bootstrap's utility-first approach.

12. **Apply line height to the container, not individual spans.** The `line-height` property on a block element affects all lines within it. Setting it on inline elements within a block can create inconsistent line spacing.

## Common Pitfalls

### Using Fixed Line Height Units

Setting `line-height` in pixels creates inconsistent spacing across different font sizes:

```html
<!-- WRONG: Fixed pixel line height doesn't scale -->
<p style="line-height: 24px; font-size: 16px;">Body text at 16px with 24px line height.</p>
<h1 style="line-height: 24px; font-size: 48px;">Heading at 48px with the same 24px line height — lines overlap!</h1>

<!-- RIGHT: Unitless line height scales with font size -->
<p style="line-height: 1.5; font-size: 16px;">Body text at 16px with 1.5 line height (24px).</p>
<h1 style="line-height: 1.2; font-size: 48px;">Heading at 48px with 1.2 line height (57.6px).</h1>
```

### Excessive Line Height on Body Text

Line heights above 2.0 for body text create so much vertical space that readers lose their place between lines:

```html
<!-- WRONG: Line height of 3.0 is too loose for paragraphs -->
<p class="lh-1" style="line-height: 3;">
  This paragraph has an extremely high line height that makes it very
  difficult to track from one line to the next. The reader's eye must
  travel a large vertical distance between lines.
</p>

<!-- RIGHT: Line height of 1.5 to 1.7 is optimal for body text -->
<p style="line-height: 1.6;">
  This paragraph has a comfortable line height that provides adequate
  breathing room without losing the connection between lines.
</p>
```

### Letter Spacing on Body Text

Adding letter spacing to paragraphs reduces reading speed and creates a disconnected visual texture:

```html
<!-- WRONG: Letter spacing on body text -->
<p style="letter-spacing: 0.05em;">
  This paragraph has extra letter spacing that makes each word feel
  disconnected. Reading speed decreases and the text feels unnatural.
</p>

<!-- RIGHT: Letter spacing reserved for uppercase and display text -->
<p>No letter spacing on body text for natural reading.</p>
<p class="text-uppercase" style="letter-spacing: 0.1em;">UPPERCASE WITH SPACING</p>
```

### Inconsistent Line Heights Across Components

Setting different line heights on different elements without a system creates visual inconsistency:

```html
<!-- WRONG: Arbitrary line heights scattered across components -->
<p style="line-height: 1.6;">Paragraph with 1.6</p>
<div style="line-height: 1.4;">Card text with 1.4</div>
<span style="line-height: 1.8;">Span text with 1.8</span>

<!-- RIGHT: Consistent line height system -->
<p class="lh-base">Paragraph with standard base line height.</p>
<div class="lh-base">Card text with standard base line height.</div>
<p class="lh-lg">Special text with large line height.</p>
```

### Forgetting That Line Height is Inherited

The `line-height` property inherits from parent elements. Setting a tight line height on a container affects all child elements:

```html
<!-- WRONG: Tight line height on container affects all children -->
<div style="line-height: 1;">
  <p>This paragraph inherits the tight line height of 1.</p>
  <h2>This heading also inherits line height 1.</h2>
  <small>This small text is nearly unreadable at line height 1.</small>
</div>

<!-- RIGHT: Set line height at the element level or use unitless values -->
<div>
  <p style="line-height: 1.6;">Paragraph with appropriate line height.</p>
  <h2 style="line-height: 1.2;">Heading with appropriate line height.</h2>
</div>
```

### Ignoring Font Metrics

Different fonts have different built-in spacing. A line height that works for one font may not work for another:

```html
<!-- WRONG: Same line height for different fonts -->
<p style="font-family: Arial; line-height: 1.5;">Arial text — comfortable at 1.5.</p>
<p style="font-family: 'Courier New'; line-height: 1.5;">
  Monospace text — feels too tight at 1.5 because monospace fonts have
  wider characters and different internal metrics.
</p>

<!-- RIGHT: Adjust line height per font -->
<p style="font-family: Arial; line-height: 1.5;">Arial text.</p>
<p style="font-family: 'Courier New'; line-height: 1.8;">Monospace text with larger line height.</p>
```

## Accessibility Considerations

Line height is one of the most critical factors for text accessibility. WCAG 2.1 Success Criterion 1.4.12 (Text Spacing) specifies that content must remain readable when users override the following spacing values: line height to at least 1.5 times the font size, paragraph spacing to at least 2 times the font size, letter spacing to at least 0.12 times the font size, and word spacing to at least 0.16 times the font size.

This means your layout must not break or hide content when users apply these spacing overrides via browser extensions, user stylesheets, or reading mode tools. Test your design by temporarily applying these values globally:

```css
/* WCAG 1.4.12 test values */
* {
  line-height: 1.5 !important;
  letter-spacing: 0.12em !important;
  word-spacing: 0.16em !important;
}
```

If content overflows, becomes hidden, or loses readability with these overrides, your layout does not meet WCAG AA standards.

For users with dyslexia, increased line height (1.5 to 2.0) and slightly increased word spacing (0.16em) significantly improve reading comprehension. Consider offering a reading mode or respecting user preferences:

```css
@media (prefers-contrast: more) {
  body {
    line-height: 1.8;
    word-spacing: 0.2em;
  }
}
```

Avoid `lh-1` (line-height: 1) on any text content that needs to be read. This value creates overlapping lines when users zoom to 200% (a WCAG requirement) or apply custom text spacing. Even for headings, a minimum of 1.2 is safer.

Letter spacing below -0.02em can cause characters to overlap at large sizes and become illegible when users apply positive letter spacing overrides. Test negative letter spacing values with the WCAG text spacing requirements to ensure they do not break readability.

## Responsive Behavior

Line height should generally remain consistent across breakpoints because it relates to readability, not layout. However, there are scenarios where responsive line height adjustments improve the experience.

### Responsive Line Height for Headings

Display headings on mobile screens can feel too loose at the same line height used on desktop:

```css
@media (max-width: 576px) {
  .heading-responsive {
    line-height: 1.2;
    font-size: 1.75rem;
  }
}
@media (min-width: 577px) {
  .heading-responsive {
    line-height: 1.1;
    font-size: 3rem;
  }
}
```

```html
<h1 class="heading-responsive fw-bold">Responsive Heading</h1>
```

### Responsive Letter Spacing

Letter spacing that looks appropriate on desktop may feel excessive on mobile:

```html
<style>
  @media (max-width: 576px) {
    .ls-responsive {
      letter-spacing: 0.05em;
    }
  }
  @media (min-width: 577px) {
    .ls-responsive {
      letter-spacing: 0.15em;
    }
  }
</style>

<h2 class="ls-responsive text-uppercase fw-bold">
  Responsive Spacing
</h2>
```

### Line Height with Responsive Font Sizes

When combining responsive font sizes with line height, use unitless values to maintain proportional spacing:

```html
<style>
  .text-responsive {
    font-size: 1rem;
    line-height: 1.6;
  }
  @media (min-width: 768px) {
    .text-responsive {
      font-size: 1.125rem;
      /* line-height remains 1.6, scaling to ~1.125 * 1.6 = 1.8rem */
    }
  }
  @media (min-width: 1200px) {
    .text-responsive {
      font-size: 1.25rem;
      /* line-height remains 1.6, scaling to ~1.25 * 1.6 = 2rem */
    }
  }
</style>

<div class="container py-4">
  <p class="text-responsive">
    This paragraph's line height scales proportionally with its font size
    across breakpoints. The unitless line-height value ensures that as the
    font grows, the vertical spacing grows with it.
  </p>
</div>
```

### Responsive Vertical Rhythm

Adjust the base rhythm unit for different viewport sizes:

```css
:root {
  --rhythm: 1.25rem;
}
@media (min-width: 768px) {
  :root {
    --rhythm: 1.5rem;
  }
}
@media (min-width: 1200px) {
  :root {
    --rhythm: 1.75rem;
  }
}

p {
  line-height: var(--rhythm);
  margin-bottom: var(--rhythm);
}
```

This ensures vertical rhythm adapts to viewport dimensions while maintaining a consistent proportional system throughout the design.
