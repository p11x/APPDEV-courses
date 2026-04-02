---
title: "Typography Dark Mode"
topic: "Typography Engine"
subtopic: "Typography Dark Mode"
difficulty: 2
duration: "25 minutes"
prerequisites: ["Text Colors", "Font Stack Customization"]
learning_objectives:
  - Apply text colors that work in dark mode themes
  - Use emphasis and heading colors for dark backgrounds
  - Implement Bootstrap 5.3 color mode typography
---

## Overview

Bootstrap 5.3 introduced color modes (light/dark) via the `data-bs-theme` attribute. Typography in dark mode requires careful color selection — default dark text (`#212529`) becomes invisible on dark backgrounds, so Bootstrap remaps text colors automatically. Understanding how heading colors, emphasis colors, and muted text behave in dark mode ensures readable, accessible typography in both themes.

## Basic Implementation

Dark mode typography using `data-bs-theme="dark"`:

```html
<div data-bs-theme="dark" class="bg-dark p-4 rounded">
  <h2>Dark Mode Heading</h2>
  <p>Body text in dark mode uses light colors automatically.</p>
  <p class="text-muted">Muted text adjusts for dark backgrounds.</p>
</div>
```

Heading colors that adapt to color mode:

```html
<div data-bs-theme="dark" class="bg-dark p-4 rounded">
  <h1 class="text-body">Adaptive Heading</h1>
  <h2 class="text-body-secondary">Secondary Heading</h2>
  <h3 class="text-body-emphasis">Emphasis Heading</h3>
</div>
```

Mixed theme sections on the same page:

```html
<div class="bg-white p-4 rounded mb-3" data-bs-theme="light">
  <h3>Light Section</h3>
  <p class="text-body-secondary">Light mode secondary text.</p>
</div>
<div class="bg-dark p-4 rounded" data-bs-theme="dark">
  <h3>Dark Section</h3>
  <p class="text-body-secondary">Dark mode secondary text.</p>
</div>
```

## Advanced Variations

Emphasis and display typography in dark mode:

```html
<div data-bs-theme="dark" class="bg-dark p-5 rounded">
  <h1 class="display-4 text-body-emphasis mb-3">Dark Display Heading</h1>
  <p class="fs-5 text-body">
    Primary body text in dark mode uses <code>--bs-body-color</code> which
    maps to a light value automatically.
  </p>
  <p class="text-body-secondary">
    Secondary text uses <code>--bs-secondary-color</code> for reduced emphasis.
  </p>
  <p class="text-body-tertiary">
    Tertiary text for minimal visual weight in dark mode.
  </p>
</div>
```

Code blocks and monospace in dark mode:

```html
<div data-bs-theme="dark" class="bg-dark p-4 rounded">
  <h4 class="text-body-emphasis">Dark Mode Code</h4>
  <pre class="bg-black text-light p-3 rounded"><code>const theme = document.documentElement
  .getAttribute('data-bs-theme');
console.log(`Current theme: ${theme}`);</code></pre>
  <p>Inline code: <code class="text-info">const x = 42;</code></p>
</div>
```

Blockquotes with dark mode styling:

```html
<div data-bs-theme="dark" class="bg-dark p-4 rounded">
  <blockquote class="blockquote border-start border-4 border-secondary ps-3">
    <p class="text-body">"Typography in dark mode requires careful attention to contrast."</p>
    <footer class="blockquote-footer text-body-secondary">Design Guide</footer>
  </blockquote>
</div>
```

## Best Practices

1. Use `text-body`, `text-body-secondary`, and `text-body-tertiary` instead of hardcoded colors like `text-dark` or `text-light`.
2. Apply `text-body-emphasis` for headings and important text that should have maximum contrast.
3. Use `data-bs-theme="dark"` on specific sections rather than the entire page for mixed-mode layouts.
4. Test all typography colors with both `data-bs-theme="light"` and `data-bs-theme="dark"`.
5. Use `bg-body` and `bg-body-secondary` for backgrounds that adapt to the current color mode.
6. Avoid `text-white` and `text-black` — use theme-aware color utilities instead.
7. Apply `border-secondary` for borders that adapt to dark/light themes.
8. Ensure code blocks use appropriate dark-on-light or light-on-dark backgrounds for the active theme.
9. Use CSS custom properties (`--bs-body-color`, `--bs-emphasis-color`) for custom typography elements.
10. Test dark mode typography with users who have light sensitivity or visual impairments.

## Common Pitfalls

- **Hardcoded text colors**: Using `color: #333` or `text-dark` in dark mode makes text invisible on dark backgrounds.
- **Forgetting `data-bs-theme`**: Without the attribute, dark mode classes don't activate.
- **Missing secondary/tertiary variants**: Using only `text-body` loses the ability to de-emphasize text.
- **Code block contrast**: `<pre>` with `bg-light` in dark mode creates jarring light blocks on dark backgrounds.
- **Image and icon contrast**: SVG icons and images may need inverse colors in dark mode.
- **Not testing both modes**: Typography that looks good in light mode may have contrast issues in dark mode.
- **CSS specificity conflicts**: Custom color CSS with high specificity can override Bootstrap's dark mode remapping.

## Accessibility Considerations

- Ensure text contrast ratios meet WCAG 4.5:1 in both light and dark modes.
- Use `text-body-emphasis` for important content that needs maximum readability.
- Provide a visible theme toggle so users can choose their preferred mode.
- Respect `prefers-color-scheme` media query for automatic theme detection.
- Test with high contrast mode enabled in the operating system.
- Ensure focus indicators remain visible in both color modes.
- Verify that screen reader content is unaffected by visual color mode changes.

## Responsive Behavior

Color mode applies globally and doesn't change at breakpoints. Typography sizing and color mode work independently:

```html
<div data-bs-theme="dark" class="bg-dark p-4 rounded">
  <h1 class="display-4 display-md-3 text-body-emphasis">
    Responsive Dark Mode Heading
  </h1>
  <p class="fs-5 fs-md-4 text-body">
    Font size responds to breakpoints while color mode provides appropriate
    contrast at all viewport sizes.
  </p>
  <div class="row mt-3">
    <div class="col-12 col-md-6">
      <p class="text-body-secondary">Column text adapts to dark mode.</p>
    </div>
    <div class="col-12 col-md-6">
      <p class="text-body-secondary">Second column with same adaptive colors.</p>
    </div>
  </div>
</div>
```

Combine responsive font utilities with theme-aware text classes to maintain readability across all viewport sizes and color modes.
