---
title: Typography CSS Variables
category: Bootstrap Fundamentals
difficulty: 2
time: 20 min
tags: bootstrap5, css-variables, typography, font, font-size, line-height
---

## Overview

Bootstrap 5's typography system is controlled through CSS custom properties that define font family, base font size, font weight, and line height. Key variables include `--bs-body-font-family` (default typeface), `--bs-body-font-size` (root text size), `--bs-body-font-weight` (default weight), and `--bs-body-line-height` (line spacing). These variables cascade to all text-rendering components, making them the central control point for typography customization. Modifying these variables at the root level transforms the typographic appearance of the entire site.

## Basic Implementation

The core typography variables set the foundation for all text on the page.

```html
<style>
  /* Default Bootstrap typography variables */
  :root {
    /* --bs-body-font-family: system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", ...; */
    /* --bs-body-font-size: 1rem; */
    /* --bs-body-font-weight: 400; */
    /* --bs-body-line-height: 1.5; */
  }
</style>

<!-- Elements inheriting typography variables -->
<p>This paragraph inherits all typography settings from the root variables.</p>
<small>This small text also inherits the font family and line height.</small>
```

Customizing typography for the entire application.

```html
<style>
  :root {
    --bs-body-font-family: 'Inter', 'Segoe UI', system-ui, sans-serif;
    --bs-body-font-size: 1rem;
    --bs-body-font-weight: 400;
    --bs-body-line-height: 1.6;
  }
</style>

<h3>Heading with Custom Typography</h3>
<p>Body text now uses the Inter font with increased line height for improved readability. All text on this page inherits these settings.</p>
```

## Advanced Variations

Setting up a comprehensive type scale with CSS variables.

```html
<style>
  :root {
    --bs-body-font-family: 'Georgia', 'Times New Roman', serif;
    --bs-body-line-height: 1.7;

    /* Custom heading font family */
    --bs-headings-font-family: 'Helvetica Neue', 'Arial', sans-serif;
    --bs-headings-font-weight: 700;
    --bs-headings-line-height: 1.2;
  }

  h1, h2, h3, h4, h5, h6 {
    font-family: var(--bs-headings-font-family);
    font-weight: var(--bs-headings-font-weight);
    line-height: var(--bs-headings-line-height);
  }
</style>

<h1>Sans-serif Heading</h1>
<p>This body text uses Georgia, a serif font, while headings use Helvetica Neue for a clean, modern appearance. The contrast creates visual hierarchy.</p>
```

Using Google Fonts with Bootstrap's typography variables.

```html
<!-- Load Google Fonts -->
<link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&family=Roboto+Slab:wght@400;700&display=swap" rel="stylesheet">

<style>
  :root {
    --bs-body-font-family: 'Roboto', system-ui, sans-serif;
    --bs-body-font-size: 1rem;
    --bs-body-line-height: 1.5;
  }

  h1, h2, h3 {
    font-family: 'Roboto Slab', serif;
    font-weight: 700;
  }
</style>

<h1>Roboto Slab Heading</h1>
<p>Body text in Roboto with headings in Roboto Slab for a professional editorial look.</p>
```

## Best Practices

1. **Use system font stacks for performance** - Bootstrap's default system font stack loads instantly without web font downloads.
2. **Load web fonts efficiently** - Use `font-display: swap` and preload critical fonts to minimize layout shift.
3. **Set line-height appropriately** - Body text benefits from 1.5-1.7 line-height; headings work better with 1.1-1.3.
4. **Maintain readable line lengths** - Combine font size variables with max-width constraints for 45-75 characters per line.
5. **Use `rem` for font sizes** - Ensure font sizes scale relative to the root for consistent zoom behavior.
6. **Provide font fallbacks** - Always include generic font families (`sans-serif`, `serif`, `monospace`) as fallbacks.
7. **Limit font families** - Use at most 2-3 font families to maintain visual cohesion and reduce load times.
8. **Adjust font weight variables** - Set `--bs-body-font-weight` to match your brand's typographic tone.
9. **Test across operating systems** - System fonts render differently on Windows, macOS, and Linux. Verify on all target platforms.
10. **Consider variable fonts** - Variable font files offer multiple weights in a single file, reducing HTTP requests.

## Common Pitfalls

1. **Not preloading web fonts** - Loading fonts without preload causes FOUT (Flash of Unstyled Text) or FOIT (Flash of Invisible Text).
2. **Incorrect font fallback order** - Listing generic fallbacks before specific fonts causes inconsistent rendering across platforms.
3. **Ignoring font licensing** - Commercial fonts require proper licensing for web use. Verify license terms before embedding.
4. **Too many font weights** - Loading 6+ font weights significantly increases page load time. Limit to essential weights.
5. **Not testing with real text** - Typography that looks good with Latin placeholder text may break with accented characters, CJK scripts, or special symbols.

## Accessibility Considerations

Typography choices directly impact readability and accessibility. Ensure body text is at least 16px (1rem) for comfortable reading. Line height should be at least 1.5 for body text to support users with cognitive disabilities. Provide sufficient contrast between text and background (4.5:1 minimum). Allow text to scale to 200% without breaking layout. Avoid fixed font sizes in pixels; use `rem` and `em` units so users can adjust text size through browser settings. Test with screen readers to verify that font changes do not affect pronunciation or reading order.

## Responsive Behavior

Typography CSS variables can be overridden within media queries to create responsive type scales. Increase `--bs-body-font-size` at larger breakpoints for more readable text on wide screens. Adjust `--bs-body-line-height` based on viewport width to maintain optimal line length readability. Combine responsive typography with Bootstrap's responsive utility classes for a complete adaptive text system. For example, body text might be 14px on mobile and scale to 18px on desktop through variable overrides in media queries.
