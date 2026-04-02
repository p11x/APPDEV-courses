---
title: "Typography Accessibility"
topic: "Typography Engine"
subtopic: "Typography Accessibility"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Desktop Mobile Typography", "Font Stack Customization"]
learning_objectives:
  - Set readable font sizes that meet WCAG guidelines
  - Ensure sufficient contrast ratios for all text
  - Apply dyslexia-friendly font and spacing techniques
---

## Overview

Accessible typography ensures all users — including those with visual impairments, dyslexia, or cognitive disabilities — can read and understand content. Key factors include minimum font sizes (16px for body text), sufficient color contrast (4.5:1 for normal text, 3:1 for large text), adequate line height (1.5+ for body text), and proper letter spacing. Bootstrap provides utilities and variables that support these requirements out of the box.

## Basic Implementation

Body text with accessible defaults:

```html
<div class="container">
  <h1>Accessible Page Title</h1>
  <p class="fs-5">
    Body text at 16px (1rem) minimum ensures readability on all devices.
    Bootstrap's default body font size meets WCAG SC 1.4.4 requirements.
  </p>
  <p class="lead">
    The lead paragraph uses a larger font size for introductory content,
    improving readability for all users.
  </p>
</div>
```

High contrast text with sufficient ratios:

```html
<div class="bg-dark text-white p-4 rounded">
  <h2>High Contrast Section</h2>
  <p>White text on dark background provides excellent contrast (21:1).</p>
  <p class="text-warning">Warning text on dark background (5.5:1).</p>
  <p class="text-info">Info text on dark background (7.2:1).</p>
</div>
```

Readable line height and spacing:

```html
<div class="container">
  <p style="line-height: 1.75; letter-spacing: 0.02em; max-width: 65ch;">
    This paragraph uses an increased line-height of 1.75 and slight
    letter-spacing for improved readability. The maximum width is
    constrained to 65 characters per line, which is optimal for
    comfortable reading across all users.
  </p>
</div>
```

## Advanced Variations

Dyslexia-friendly typography overrides:

```html
<style>
  .dyslexia-friendly {
    font-family: 'OpenDyslexic', 'Comic Sans MS', sans-serif;
    line-height: 1.8;
    letter-spacing: 0.05em;
    word-spacing: 0.16em;
    max-width: 60ch;
  }
  .dyslexia-friendly p {
    margin-bottom: 1.5em;
  }
</style>
<div class="dyslexia-friendly">
  <h2>Dyslexia-Friendly Typography</h2>
  <p>
    Increased spacing, wider word gaps, and a dyslexia-specific font
    family improve readability for users with dyslexia.
  </p>
  <p>
    Paragraphs have extra margin between them, and line-height is
    increased to 1.8 for comfortable line tracking.
  </p>
</div>
```

Accessible heading hierarchy:

```html
<div class="container">
  <h1>Page Title (h1)</h1>
  <p>Introduction paragraph.</p>
  <h2>Section Heading (h2)</h2>
  <p>Section content.</p>
  <h3>Subsection Heading (h3)</h3>
  <p>Subsection content.</p>
  <h4>Detail Heading (h4)</h4>
  <p>Detail content. Never skip heading levels.</p>
</div>
```

Respecting user preferences with `prefers-contrast` and `prefers-reduced-motion`:

```html
<style>
  @media (prefers-contrast: more) {
    .high-contrast-text {
      color: #000 !important;
      background: #fff !important;
      font-weight: 600;
    }
  }
  @media (prefers-reduced-motion: reduce) {
    * {
      transition: none !important;
      animation: none !important;
    }
  }
</style>
<p class="high-contrast-text bg-light p-3">
  This text respects the user's high contrast preference setting.
</p>
```

## Best Practices

1. Set body font size to minimum 16px (1rem) — never go below 14px for any text.
2. Maintain 4.5:1 contrast ratio for normal text and 3:1 for large text (18px+ or 14px+ bold).
3. Use `line-height: 1.5` or greater for body text to aid line tracking.
4. Constrain text width to 60-80 characters per line with `max-width: 65ch` or similar.
5. Maintain a clear heading hierarchy (h1 → h2 → h3) without skipping levels.
6. Use `rem` units for font sizes so they respect user browser font settings.
7. Provide sufficient paragraph spacing (`margin-bottom: 1.5em`) for visual separation.
8. Test typography with screen readers, browser zoom (200-400%), and high contrast modes.
9. Use `prefers-contrast` media query to enhance contrast for users who request it.
10. Offer a typography customization option (font size toggle) for users with low vision.
11. Avoid justified text alignment — left-aligned text has more consistent word spacing.

## Common Pitfalls

- **Small font sizes**: Using 12px or 14px body text violates WCAG SC 1.4.4.
- **Low contrast text**: Light gray text on white backgrounds fails contrast requirements.
- **Skipping heading levels**: Jumping from h1 to h3 confuses screen reader navigation.
- **Fixed pixel sizes**: `font-size: 14px` ignores user browser font preferences.
- **Justified text**: Creates uneven word spacing that is harder to read for many users.
- **Tiny clickable text**: Links or buttons smaller than 44x44px are hard to tap on mobile.
- **Ignoring user preferences**: Not respecting `prefers-reduced-motion` or `prefers-contrast`.
- **Line-height too tight**: `line-height: 1.0` or `1.2` for body text reduces readability significantly.

## Accessibility Considerations

- Follow WCAG 2.1 SC 1.4.4 (Resize Text) — text must be resizable to 200% without loss of content.
- Follow SC 1.4.8 (Visual Presentation) for blocks of text — line height, spacing, and width.
- Follow SC 1.4.3 (Contrast Minimum) — 4.5:1 for normal text, 3:1 for large text.
- Ensure focus indicators on interactive text (links, buttons) have 3:1 contrast against adjacent colors.
- Use `aria-label` on truncated text to provide the full content to screen readers.
- Test with assistive technologies: NVDA, JAWS, VoiceOver, and browser zoom tools.
- Provide a skip-to-content link for users navigating with screen readers.
- Use `role="heading"` and `aria-level` when visual headings lack semantic HTML heading elements.

## Responsive Behavior

Typography must remain accessible at all viewport sizes. Use responsive font utilities and ensure contrast is maintained:

```html
<div class="container">
  <h1 class="fs-3 fs-md-2 fs-lg-1">Responsive Accessible Heading</h1>
  <p class="fs-6 fs-md-5">
    Text that maintains minimum 16px at all breakpoints while scaling
    up on larger viewports for improved readability.
  </p>
  <div class="row">
    <div class="col-12 col-md-6">
      <p style="max-width: 65ch; line-height: 1.6;">
        Constrained paragraph width with accessible line height
        that adapts to column width at each breakpoint.
      </p>
    </div>
  </div>
</div>
```

Test at 320px, 768px, and 1440px widths to verify text remains readable and contrast ratios are maintained at all sizes.
