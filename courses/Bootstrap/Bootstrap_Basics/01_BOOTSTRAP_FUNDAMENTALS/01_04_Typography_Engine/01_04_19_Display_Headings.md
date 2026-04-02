---
title: "Display Headings"
topic: "Typography Engine"
subtopic: "Display Headings"
difficulty: 1
duration: "20 minutes"
prerequisites: ["Heading Typography", "Font Weight Style"]
learning_objectives:
  - Apply display-1 through display-6 classes for prominent headings
  - Understand when to use display vs standard heading classes
  - Combine display headings with responsive breakpoints
---

## Overview

Bootstrap's display heading classes (`display-1` through `display-6`) create extra-large, lightweight headings ideal for hero sections, landing pages, and feature callouts. Unlike standard heading tags (`<h1>`-`<h6>`), display classes decouple visual size from semantic heading level, allowing a `<h3>` to appear as a large display heading while maintaining proper document hierarchy for accessibility.

## Basic Implementation

Display heading classes with increasing sizes:

```html
<h1 class="display-1">Display 1</h1>
<h1 class="display-2">Display 2</h1>
<h1 class="display-3">Display 3</h1>
<h1 class="display-4">Display 4</h1>
<h1 class="display-5">Display 5</h1>
<h1 class="display-6">Display 6</h1>
```

Display heading in a hero section:

```html
<div class="bg-primary text-white text-center py-5">
  <h1 class="display-4">Welcome to Our Site</h1>
  <p class="lead">Build responsive layouts with Bootstrap 5.</p>
  <button class="btn btn-light btn-lg mt-3">Get Started</button>
</div>
```

Display heading with a lower semantic level:

```html
<h3 class="display-3">
  Semantic h3 styled as a large display heading
</h3>
<p>
  This maintains proper heading hierarchy (h3 under h2) while appearing
  visually large. Screen readers see h3, users see a prominent display.
</p>
```

## Advanced Variations

Responsive display headings that change size at breakpoints:

```html
<h1 class="display-4 display-md-3 display-lg-1">
  Responsive Display Heading
</h1>
<p class="text-muted">
  Shrinks on mobile (display-4), grows on tablet (display-3),
  and reaches maximum size on desktop (display-1).
</p>
```

Display heading with custom font weight:

```html
<style>
  .display-bold {
    font-weight: 800;
    letter-spacing: -0.02em;
  }
</style>
<h1 class="display-3 display-bold">
  Bold Display Heading
</h1>
```

Display heading combined with text utilities:

```html
<div class="bg-dark text-white p-5 rounded">
  <h1 class="display-5 text-warning mb-3">Feature Highlight</h1>
  <p class="display-6 fw-light text-body-secondary mb-0">
    Sub-display text for secondary emphasis.
  </p>
</div>
```

Display heading in a split layout:

```html
<div class="container">
  <div class="row align-items-center py-5">
    <div class="col-lg-6">
      <h1 class="display-4 fw-bold mb-4">Product Name</h1>
      <p class="lead text-muted mb-4">
        A compelling description of the product that goes alongside
        the display heading for visual balance.
      </p>
      <button class="btn btn-primary btn-lg">Learn More</button>
    </div>
    <div class="col-lg-6">
      <div class="bg-light rounded" style="height: 300px;">
        <div class="d-flex align-items-center justify-content-center h-100">
          Image Placeholder
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use display headings for hero sections, landing page headlines, and prominent feature titles.
2. Combine display classes with appropriate semantic heading tags (`<h1>`-`<h6>`) for accessibility.
3. Use `display-1` or `display-2` sparingly — they are very large and suit only primary headlines.
4. Apply `display-4` through `display-6` for section-level prominence without overwhelming the page.
5. Use responsive display classes (`display-md-3`) to scale headings for mobile readability.
6. Pair display headings with `lead` class paragraphs for visual hierarchy.
7. Use `fw-bold` or `fw-light` to adjust display heading weight beyond the default.
8. Limit each page to 1-2 display headings — overuse reduces their visual impact.
9. Test display headings at mobile widths — `display-1` may be too large for narrow screens.
10. Maintain clear visual hierarchy — display headings should be the largest text on the page.

## Common Pitfalls

- **Using display-1 on mobile**: `display-1` (5rem / 80px) overflows or dominates mobile screens. Use responsive classes.
- **Ignoring semantic hierarchy**: Using `<h1 class="display-1">` everywhere breaks heading level structure.
- **Overusing display headings**: Multiple display headings on a page reduce the impact of each one.
- **Missing line-height adjustment**: Display headings with default line-height may have too much or too little space between lines.
- **Combining with conflicting utilities**: `display-4` with `fs-1` creates conflicting font-size rules.
- **Not testing with real content**: Display headings look different with long text that wraps to multiple lines.
- **Contrast on backgrounds**: Display headings on colored backgrounds need sufficient contrast verification.

## Accessibility Considerations

- Use semantic heading tags (`<h1>`-`<h6>`) with display classes, not `<div>` or `<span>`.
- Maintain proper heading hierarchy — don't skip from `<h1>` to `<h3>` for visual reasons.
- Screen readers ignore display classes and use the semantic heading level — this is correct behavior.
- Ensure display heading text contrasts 4.5:1 with its background (3:1 for text 18px+ or 14px+ bold).
- Provide alternative text descriptions for display headings that use decorative or custom fonts.
- Test with screen readers to verify heading navigation (H key) works correctly with display-styled headings.

## Responsive Behavior

Display headings support responsive breakpoint variants. Use `display-{breakpoint}-{size}` to change the display class at specific viewports:

```html
<div class="container">
  <h1 class="display-6 display-sm-5 display-md-4 display-lg-3 display-xl-2">
    Progressive Display Heading
  </h1>
  <p class="lead">
    Scales up at each breakpoint: display-6 on mobile, display-5 on sm,
    display-4 on md, display-3 on lg, and display-2 on xl screens.
  </p>
</div>
```

This creates a heading that starts compact on mobile and grows progressively as viewport width increases, ensuring readability at every size while maintaining visual prominence on larger screens.
