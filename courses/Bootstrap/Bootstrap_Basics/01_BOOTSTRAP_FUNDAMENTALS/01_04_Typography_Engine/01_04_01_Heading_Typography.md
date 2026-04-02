---
title: "Heading Typography in Bootstrap 5"
subtitle: "Mastering h1-h6, display headings, custom heading styles, and headings with secondary text"
category: "Bootstrap Basics"
subcategory: "Typography Engine"
difficulty: 1
duration: "25 minutes"
prerequisites:
  - "Basic HTML knowledge"
  - "Bootstrap 5 setup"
learning_objectives:
  - "Understand Bootstrap's default heading styles and sizing"
  - "Use display heading classes for prominent text"
  - "Create custom heading styles with utility classes"
  - "Implement headings with secondary text patterns"
keywords:
  - "bootstrap headings"
  - "display headings"
  - "typography"
  - "h1 h2 h3"
  - "display-1"
---

# Heading Typography in Bootstrap 5

## Overview

Heading typography is the foundation of visual hierarchy in web design. In Bootstrap 5, headings serve as the primary structural elements that guide users through content, establishing importance, grouping related information, and creating scannable page layouts. The framework provides a comprehensive set of heading utilities that extend beyond standard HTML heading elements, giving developers fine-grained control over visual prominence without sacrificing semantic correctness.

Bootstrap 5 ships with default styles for all six HTML heading elements (`<h1>` through `<h6>`). Each heading level carries a specific font size, font weight, and bottom margin that decreases progressively. The `<h1>` element renders at `2.5rem` (40px) with a font weight of 500, while `<h6>` renders at `1rem` (16px) with the same weight. These values are defined through Bootstrap's CSS custom properties, making them straightforward to override at the theme level or per-component level.

Beyond standard headings, Bootstrap introduces **display headings** — classes (`display-1` through `display-6`) designed for large, prominent text that sits outside the normal heading hierarchy. Display headings are ideal for hero sections, landing pages, marketing banners, and any context where text needs to command immediate attention. They range from `5rem` (80px) for `display-1` down to `1.75rem` (28px)` for `display-6`, providing significantly more visual weight than standard headings.

The framework also supports **secondary text within headings**, allowing developers to append subtitles, captions, or supplementary information directly within heading elements. This is achieved through the `<small>` element combined with utility classes, creating a visually subordinate but contextually linked text fragment.

Understanding the interplay between semantic HTML, visual presentation, and Bootstrap's utility classes is essential for building accessible, maintainable heading structures. This module covers all aspects of heading typography, from basic implementation to advanced patterns involving responsive behavior and accessibility best practices.

## Basic Implementation

### Standard HTML Headings

Bootstrap 5 automatically styles all standard HTML heading elements. The framework applies consistent sizing, weight, and spacing across all six levels:

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <title>Heading Typography</title>
</head>
<body>
  <div class="container py-5">
    <h1>Heading 1 — Primary Page Title</h1>
    <h2>Heading 2 — Section Title</h2>
    <h3>Heading 3 — Subsection Title</h3>
    <h4>Heading 4 — Minor Section</h4>
    <h5>Heading 5 — Small Heading</h5>
    <h6>Heading 6 — Smallest Heading</h6>
  </div>
</body>
</html>
```

Each heading level reduces in size and visual prominence. The default Bootstrap heading sizes are:

| Element | Size | Font Weight |
|---------|------|-------------|
| `<h1>` | 2.5rem (40px) | 500 |
| `<h2>` | 2rem (32px) | 500 |
| `<h3>` | 1.75rem (28px) | 500 |
| `<h4>` | 1.5rem (24px) | 500 |
| `<h5>` | 1.25rem (20px) | 500 |
| `<h6>` | 1rem (16px) | 500 |

### Display Headings

Display headings are Bootstrap's solution for extra-large, attention-grabbing text. They are applied as classes to any heading element:

```html
<div class="container py-5">
  <h1 class="display-1">Display 1</h1>
  <h1 class="display-2">Display 2</h1>
  <h1 class="display-3">Display 3</h1>
  <h1 class="display-4">Display 4</h1>
  <h1 class="display-5">Display 5</h1>
  <h1 class="display-6">Display 6</h1>
</div>
```

Display headings can be applied to any element, not just `<h1>`. This flexibility is useful when you want the visual impact of a large heading while maintaining correct semantic structure:

```html
<!-- Semantic h2 with display-1 visual treatment -->
<h2 class="display-1">Visually Large Section Heading</h2>

<!-- Div with display heading for hero sections -->
<div class="display-3 fw-bold text-center">Welcome to Our Platform</div>
```

### Heading with Secondary Text

Bootstrap supports secondary text within headings using the `<small>` element. This pattern is commonly used for subtitles, dates, or supplementary context:

```html
<div class="container py-5">
  <h1>
    Dashboard
    <small class="text-body-secondary">Analytics Overview</small>
  </h1>

  <h2>
    Monthly Report
    <small class="text-body-secondary">March 2025</small>
  </h2>

  <h3>
    User Activity
    <small class="text-body-secondary">Last updated 5 minutes ago</small>
  </h3>
</div>
```

The `<small>` element within a heading inherits the parent's styling but renders at 70% of the parent's font size, creating a natural visual hierarchy between the primary heading and its secondary text.

## Advanced Variations

### Custom Heading Styles with Utility Classes

Bootstrap's utility classes allow you to create custom heading styles without writing custom CSS. You can combine font weight, color, spacing, and other utilities to build unique heading treatments:

```html
<div class="container py-5">
  <!-- Large bold heading with custom spacing -->
  <h1 class="fw-bold mb-4 pb-2 border-bottom border-primary border-3">
    Section Title with Custom Border
  </h1>

  <!-- Uppercase heading with letter spacing -->
  <h2 class="text-uppercase fw-light" style="letter-spacing: 0.15em;">
    Spaced Uppercase Heading
  </h2>

  <!-- Italic heading for emphasis -->
  <h3 class="fst-italic text-body-secondary">
    Emphasized Subtitle in Italic
  </h3>
</div>
```

### Combining Display Headings with Color Utilities

Display headings become significantly more impactful when paired with color utilities and background treatments:

```html
<div class="bg-dark text-white p-5 rounded-3 mb-4">
  <h1 class="display-2 fw-bold text-warning">Premium Features</h1>
  <p class="lead text-white-50">Unlock the full potential of your application.</p>
</div>

<div class="bg-primary text-white p-5 rounded-3 mb-4">
  <h2 class="display-4 fw-semibold">Get Started Today</h2>
  <p class="fs-5 opacity-75">Join thousands of developers building with Bootstrap.</p>
</div>
```

### Heading with Badges and Icons

Headings often include badges for status indicators or icons for visual reinforcement:

```html
<div class="container py-4">
  <h2 class="d-flex align-items-center gap-2">
    Notifications
    <span class="badge bg-danger fs-6">12</span>
  </h2>

  <h3 class="d-flex align-items-center gap-2">
    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16" class="text-success">
      <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
    </svg>
    Verification Complete
  </h3>
</div>
```

### Responsive Display Headings

While Bootstrap does not provide responsive display heading classes by default, you can create them using media queries:

```css
@media (max-width: 576px) {
  .display-responsive {
    font-size: 2rem;
  }
}
@media (min-width: 577px) and (max-width: 991px) {
  .display-responsive {
    font-size: 3rem;
  }
}
@media (min-width: 992px) {
  .display-responsive {
    font-size: 4.5rem;
  }
}
```

```html
<h1 class="display-responsive fw-bold">Responsive Heading</h1>
```

### Heading with Subtitle Pattern

For complex heading structures with a primary title and a descriptive subtitle:

```html
<div class="container py-4">
  <div class="mb-4">
    <h2 class="fw-bold mb-1">Account Settings</h2>
    <p class="text-body-secondary fs-5 mb-0">
      Manage your profile, preferences, and security options.
    </p>
  </div>

  <div class="mb-4">
    <h3 class="fw-semibold mb-1">Billing Information</h3>
    <p class="text-body-secondary mb-0">
      Update payment methods and view invoice history.
    </p>
  </div>
</div>
```

This pattern uses a separate paragraph element for the subtitle rather than the `<small>` element, providing better control over sizing and spacing.

## Best Practices

1. **Always use semantic heading levels in order.** Never skip from `<h2>` to `<h4>`. Screen readers and search engines rely on proper heading hierarchy to understand page structure. If the visual size doesn't match the semantic level, use utility classes to adjust the appearance.

2. **Use only one `<h1>` per page.** The `<h1>` should represent the primary topic or title of the page. All other sections should use `<h2>` through `<h6>` in a logical hierarchy.

3. **Apply display headings only for visual emphasis, not structural hierarchy.** Display heading classes (`display-1` through `display-6`) are purely visual. Pair them with the correct semantic heading element for accessibility.

4. **Use `<small>` for secondary text within headings.** The `<small>` element semantically indicates side comments or fine print, making it appropriate for subtitles and supplementary heading information.

5. **Combine font weight utilities with headings for custom emphasis.** Use `fw-bold`, `fw-semibold`, or `fw-light` to adjust heading weight without changing the semantic element.

6. **Maintain consistent heading styles across your project.** Define heading styles in a central stylesheet or use Bootstrap's CSS custom properties to ensure all headings follow the same design language.

7. **Use `text-body-secondary` for secondary heading text.** This utility class provides a muted color that visually subordinates secondary text while maintaining sufficient contrast for accessibility.

8. **Avoid using display headings for body-level content.** Display headings are designed for hero sections and prominent callouts. Using them within content areas creates visual noise and disrupts reading flow.

9. **Test heading hierarchy with accessibility tools.** Use browser extensions like axe or Lighthouse to verify that your heading structure is logical and properly nested.

10. **Use responsive utility classes for heading alignment.** Combine heading elements with `text-center`, `text-md-start`, and similar classes to control heading alignment across breakpoints.

11. **Keep heading text concise.** Long headings break layout consistency and reduce scannability. Aim for headings that are 8 words or fewer, using subtitles for additional context.

12. **Use Bootstrap's CSS custom properties for global heading customization.** Override `--bs-heading-font-weight` and related variables to change heading styles project-wide rather than targeting individual elements.

## Common Pitfalls

### Skipping Heading Levels for Visual Effect

One of the most common mistakes is choosing heading elements based on their default visual size rather than their semantic meaning. Using `<h4>` because it "looks right" while skipping `<h2>` and `<h3>` breaks document outline and confuses screen reader users.

```html
<!-- WRONG: Skipping levels for visual reasons -->
<h1>Page Title</h1>
<h4>Section Title</h4>  <!-- Should be h2 -->

<!-- CORRECT: Proper hierarchy with visual adjustment -->
<h1>Page Title</h1>
<h2 class="fs-4">Section Title</h2>
```

### Using Display Headings Everywhere

Overusing display headings dilutes their impact and creates an unbalanced page layout. Display headings should be reserved for moments of high visual emphasis.

```html
<!-- WRONG: Display headings on every section -->
<h1 class="display-3">Welcome</h1>
<h2 class="display-4">About Us</h2>
<h2 class="display-4">Services</h2>
<h2 class="display-4">Contact</h2>

<!-- CORRECT: Display heading for hero, standard headings for sections -->
<h1 class="display-3">Welcome</h1>
<h2>About Us</h2>
<h2>Services</h2>
<h2>Contact</h2>
```

### Relying on Heading Elements for Styling Only

Using heading elements purely for their visual appearance (larger, bolder text) when the content does not represent a heading creates accessibility problems:

```html
<!-- WRONG: h2 used for visual emphasis, not structural heading -->
<h2 class="text-muted">Last updated: March 2025</h2>

<!-- CORRECT: Use appropriate element with utility classes -->
<p class="text-body-secondary fw-semibold small">Last updated: March 2025</p>
```

### Ignoring Heading Hierarchy in Components

Components like modals, cards, and sidebars often have their own heading hierarchies. Failing to account for these in the overall page structure can create multiple `<h1>` elements or broken heading levels:

```html
<!-- WRONG: Multiple h1 elements -->
<h1>Page Title</h1>
<div class="modal">
  <h1>Modal Title</h1>  <!-- Should be h2 or h3 -->
</div>

<!-- CORRECT: Nested hierarchy within components -->
<h1>Page Title</h1>
<div class="modal">
  <h2>Modal Title</h2>
</div>
```

### Hardcoding Heading Sizes Instead of Using Bootstrap Classes

Writing custom CSS for heading sizes when Bootstrap utilities can achieve the same result creates maintenance overhead and inconsistency:

```html
<!-- WRONG: Custom CSS for heading size -->
<style> .custom-heading { font-size: 1.65rem; } </style>
<h2 class="custom-heading">Title</h2>

<!-- CORRECT: Use Bootstrap utility classes -->
<h2 class="fs-3">Title</h2>
```

### Not Providing Context for Secondary Heading Text

Using `<small>` within headings without proper context or visual distinction can confuse users about which text is the primary heading:

```html
<!-- WRONG: Unclear which text is the heading -->
<h1>Dashboard <small class="text-dark">Your complete overview</small></h1>

<!-- CORRECT: Clear visual distinction -->
<h1>Dashboard <small class="text-body-secondary fw-normal">Your complete overview</small></h1>
```

## Accessibility Considerations

Heading accessibility is critical for users who rely on screen readers and keyboard navigation. Screen readers use heading levels to build a page outline, allowing users to jump between sections efficiently. Incorrect heading structure makes this navigation unreliable.

Always maintain a logical heading hierarchy. Start with `<h1>` for the page title and nest subsequent sections with `<h2>` through `<h6>`. Never skip levels, even if the visual design suggests a different hierarchy. Use CSS utility classes to adjust visual appearance independently of semantic structure.

When using display heading classes, ensure they are applied to semantically correct heading elements. A `<div class="display-1">` provides no semantic meaning to screen readers, while `<h1 class="display-1">` provides both visual impact and structural information.

```html
<!-- Inaccessible: No semantic meaning -->
<div class="display-1">Welcome</div>

<!-- Accessible: Semantic heading with visual treatment -->
<h1 class="display-1">Welcome</h1>
```

Secondary text within headings should use the `<small>` element rather than `<span>`, as `<small>` carries semantic meaning indicating reduced importance. Screen readers may treat `<small>` content as less critical, which is appropriate for subtitles and supplementary information.

Ensure sufficient color contrast for all heading text, including secondary text. Bootstrap's `text-body-secondary` class provides a color that meets WCAG AA contrast requirements against white backgrounds. Custom colors should be tested using a contrast checker tool.

For headings that include icons or badges, ensure the icon is decorative (`aria-hidden="true"`) or provide an accessible label:

```html
<h2>
  <svg aria-hidden="true" class="bi bi-check-circle">...</svg>
  Verification Complete
</h2>
```

## Responsive Behavior

Bootstrap's heading sizes use `rem` units, which scale relative to the root font size. This provides inherent responsiveness, but large headings can still cause layout issues on small screens.

Standard headings (`<h1>` through `<h6>`) generally work well across all breakpoints due to their moderate sizes. However, display headings (`display-1` through `display-4`) can overflow on mobile devices and should be used with responsive considerations.

Use Bootstrap's responsive text alignment classes to adjust heading alignment at different breakpoints:

```html
<h1 class="text-center text-md-start">Centered on Mobile, Left-aligned on Desktop</h1>
```

For headings that need to change size responsively, combine Bootstrap's font size utilities with responsive display:

```html
<h1 class="fs-2 fs-md-1 fs-lg-display-6">Responsive Heading</h1>
```

When display headings are essential for mobile layouts, create custom responsive classes:

```css
@media (max-width: 575.98px) {
  .display-1-responsive { font-size: 2.5rem; }
  .display-2-responsive { font-size: 2rem; }
  .display-3-responsive { font-size: 1.75rem; }
}
```

Consider text wrapping for long headings on mobile. Use `text-wrap: balance` (supported in modern browsers) or manual `<br>` tags to control line breaks:

```html
<h1 class="fs-3 fs-md-1" style="text-wrap: balance;">
  Building Modern Web Applications with Bootstrap 5
</h1>
```

This ensures headings remain readable and aesthetically balanced regardless of viewport width.
