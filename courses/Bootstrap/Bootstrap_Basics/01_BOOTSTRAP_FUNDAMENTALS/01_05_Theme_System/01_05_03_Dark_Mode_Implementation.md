---
tags:
  - bootstrap
  - dark-mode
  - theming
  - css-custom-properties
  - accessibility
category: Bootstrap Fundamentals
difficulty: 2
time: 45 minutes
---

# Dark Mode Implementation

## Overview

Bootstrap 5.3 introduced first-class dark mode support through the `data-bs-theme` attribute. This system allows developers to toggle between light and dark themes at the page, section, or component level without writing separate CSS for each mode.

The dark mode system operates on CSS custom properties. When `data-bs-theme="dark"` is applied, Bootstrap redefines its color variables (`--bs-body-bg`, `--bs-body-color`, `--bs-primary-rgb`, etc.) to dark-mode-appropriate values. Every Bootstrap component and utility that references these variables adapts automatically.

Three modes of operation exist: **always light**, **always dark**, and **auto**. The `auto` mode respects the user's operating system preference via `prefers-color-scheme`, providing an out-of-the-box experience that aligns with user expectations without requiring JavaScript.

Dark mode can be applied at three levels. The root level (`<html data-bs-theme="dark">`) sets the default for the entire page. Section-level application (`<div data-bs-theme="dark">`) creates dark zones within a light page. Component-level application overrides individual elements. This granularity enables creative layouts where dark and light surfaces coexist.

The system integrates with Bootstrap's entire color palette. Backgrounds, text colors, borders, alerts, cards, modals, dropdowns, and form controls all respond to the theme attribute. Custom components that rely on Bootstrap's CSS custom properties inherit dark mode behavior automatically.

## Basic Implementation

The simplest dark mode implementation sets the theme on the root element:

```html
<!DOCTYPE html>
<html lang="en" data-bs-theme="dark">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Dark Mode Page</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
  <div class="container py-5">
    <h1>Dark Mode Active</h1>
    <p>All components render with dark backgrounds and light text.</p>
    <button class="btn btn-primary">Primary Button</button>
    <div class="card mt-3">
      <div class="card-body">
        <h5 class="card-title">Dark Card</h5>
        <p class="card-text">Card adapts to dark theme automatically.</p>
      </div>
    </div>
  </div>
</body>
</html>
```

To respect the user's OS preference, use the `auto` value:

```html
<html lang="en" data-bs-theme="auto">
```

Bootstrap checks `prefers-color-scheme: dark` and applies dark mode accordingly. No JavaScript is required.

A theme toggle button switches between modes at runtime:

```html
<button class="btn btn-outline-secondary" id="themeToggle">
  Toggle Dark Mode
</button>

<script>
  const toggle = document.getElementById('themeToggle');
  const html = document.documentElement;

  toggle.addEventListener('click', () => {
    const current = html.getAttribute('data-bs-theme');
    const next = current === 'dark' ? 'light' : 'dark';
    html.setAttribute('data-bs-theme', next);
    localStorage.setItem('bs-theme', next);
  });

  // Restore saved preference
  const saved = localStorage.getItem('bs-theme');
  if (saved) {
    html.setAttribute('data-bs-theme', saved);
  }
</script>
```

Section-level dark mode creates contrast zones:

```html
<div class="container py-5">
  <h2>Light Section</h2>
  <p>This content uses the default light theme.</p>

  <div data-bs-theme="dark" class="p-4 rounded mt-4">
    <h2>Dark Section</h2>
    <p>Only this container switches to dark mode.</p>
    <button class="btn btn-primary">Dark Button</button>
  </div>

  <h2 class="mt-4">Light Section Resumes</h2>
  <p>Content returns to light theme below the dark container.</p>
</div>
```

## Advanced Variations

Component-level theming overrides the section theme for specific elements:

```html
<div data-bs-theme="dark" class="p-4">
  <h3>Dark Section</h3>

  <!-- This card stays light inside the dark section -->
  <div data-bs-theme="light" class="card">
    <div class="card-body">
      <h5 class="card-title">Light Card in Dark Section</h5>
      <p class="card-text">This card overrides the parent theme.</p>
    </div>
  </div>
</div>
```

Custom CSS variables can extend the dark theme with brand-specific values:

```css
[data-bs-theme="dark"] {
  --bs-body-bg: #0a0e17;
  --bs-body-color: #e0e0e0;
  --bs-primary: #6ea8fe;
  --bs-primary-rgb: 110, 168, 254;
  --my-brand-accent: #ff6b6b;
}

[data-bs-theme="light"] {
  --bs-body-bg: #ffffff;
  --bs-body-color: #212529;
  --bs-primary: #0d6efd;
  --bs-primary-rgb: 13, 110, 253;
  --my-brand-accent: #e74c3c;
}
```

Synchronize a toggle icon with the current theme:

```html
<button class="btn btn-outline-secondary" id="themeToggle">
  <i class="bi bi-sun-fill" id="themeIcon"></i>
</button>

<script>
  const updateIcon = () => {
    const isDark = document.documentElement.getAttribute('data-bs-theme') === 'dark';
    document.getElementById('themeIcon').className =
      isDark ? 'bi bi-moon-fill' : 'bi bi-sun-fill';
  };

  document.getElementById('themeToggle').addEventListener('click', () => {
    const current = document.documentElement.getAttribute('data-bs-theme');
    const next = current === 'dark' ? 'light' : 'dark';
    document.documentElement.setAttribute('data-bs-theme', next);
    localStorage.setItem('bs-theme', next);
    updateIcon();
  });

  updateIcon();
</script>
```

Media query-based auto-detection without JavaScript:

```html
<html lang="en" data-bs-theme="auto">
```

This uses Bootstrap's built-in CSS that reads `prefers-color-scheme`. You can also detect and apply it with JavaScript for more control:

```javascript
const applySystemTheme = () => {
  const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
  document.documentElement.setAttribute(
    'data-bs-theme',
    prefersDark ? 'dark' : 'light'
  );
};

applySystemTheme();

window.matchMedia('(prefers-color-scheme: dark)')
  .addEventListener('change', applySystemTheme);
```

## Best Practices

1. **Use `data-bs-theme="auto"` as the default.** This respects the user's OS preference and provides a good experience without requiring explicit theme selection.

2. **Persist the user's theme choice in localStorage.** If you provide a manual toggle, save the preference so it survives page reloads. Restore it before the page renders to prevent a flash of the wrong theme.

3. **Apply dark mode at the root level, not per-component.** Section and component-level dark mode are useful for contrast zones but should not be the primary theming strategy. Root-level theming is simpler and more maintainable.

4. **Test all components in both themes.** Cards, modals, dropdowns, forms, and tables must render correctly in both light and dark modes. Visual regressions in dark mode are common when components use hardcoded colors.

5. **Define both light and dark values for custom CSS properties.** If you add a `--my-custom-color` variable, define it under both `[data-bs-theme="light"]` and `[data-bs-theme="dark"]` selectors.

6. **Avoid hardcoded color values in component CSS.** Use Bootstrap's CSS custom properties (`var(--bs-primary)`, `var(--bs-body-bg)`) so components respond to theme changes.

7. **Use sufficient contrast in dark mode.** Dark mode text must meet the same WCAG contrast ratios as light mode. Pure white (#fff) on pure black (#000) causes eye strain — use off-white (#e0e0e0) on near-black (#121212) instead.

8. **Test dark mode with images and media.** Bright images can be jarring against dark backgrounds. Consider applying a subtle CSS filter or using different images per theme.

9. **Provide a visible toggle with clear iconography.** Use a sun/moon icon pair and `aria-label` to communicate the toggle's purpose to all users.

10. **Do not auto-detect without providing manual override.** Some users prefer light mode on a dark-themed OS. Always let the user override the auto-detected theme.

11. **Use semantic HTML for the toggle button.** A `<button>` element is focusable and keyboard-accessible. Do not use a `<div>` or `<span>` for the theme toggle.

12. **Animate theme transitions subtly.** A brief CSS transition on `background-color` and `color` smooths the theme switch. Keep transitions under 300ms to avoid sluggishness.

## Common Pitfalls

1. **Forgetting to set `data-bs-theme` on the `<html>` element.** Placing it on `<body>` or a `<div>` only affects descendants. The root element ensures all components inherit the theme.

2. **Not restoring the saved theme before page render.** Reading localStorage after the page loads causes a flash of light theme before switching to dark. Inline the restoration script in `<head>` to run before styles apply.

3. **Hardcoding background colors in custom CSS.** Writing `background: #ffffff` on a custom class means it stays white in dark mode. Use `var(--bs-body-bg)` instead.

4. **Using `color-scheme` without `data-bs-theme`.** Setting `color-scheme: dark` in CSS alone does not update Bootstrap's custom properties. The `data-bs-theme` attribute is required.

5. **Ignoring contrast in dark mode.** Light gray text on a dark gray background may fail WCAG contrast requirements. Always test dark mode contrast with the same rigor as light mode.

6. **Applying dark mode only via CSS class.** Bootstrap uses the `data-bs-theme` attribute, not a `.dark` class. Custom class-based toggles will not activate Bootstrap's dark mode styles.

7. **Not listening for OS theme changes in auto mode.** If the user changes their OS theme while the page is open, the page should update. Attach a `change` listener to `prefers-color-scheme`.

8. **Forgetting form elements in dark mode testing.** Inputs, selects, and textareas have explicit background colors in Bootstrap. Verify they render correctly in dark mode.

## Accessibility Considerations

Dark mode is an accessibility feature. Many users with light sensitivity, migraines, or visual impairments prefer or require dark interfaces. Implementing dark mode correctly improves usability for these users.

Bootstrap's dark mode respects the `prefers-color-scheme` media query, which is the standard mechanism for OS-level theme preference. Using `data-bs-theme="auto"` aligns your interface with the user's system settings automatically.

Ensure focus indicators remain visible in dark mode. Bootstrap's default focus ring uses `--bs-primary`, which changes value in dark mode. If you customize `--bs-primary`, verify that the focus ring remains visible against dark backgrounds.

```html
<!-- Verify focus visibility -->
<button class="btn btn-primary">Focus me with Tab</button>
<input type="text" class="form-control" placeholder="Focus this input">
```

Theme toggle buttons must be accessible via keyboard and screen reader. Include `aria-label`, use a `<button>` element, and announce the current state:

```html
<button
  class="btn btn-outline-secondary"
  id="themeToggle"
  aria-label="Switch to dark mode"
>
  <i class="bi bi-sun-fill" aria-hidden="true"></i>
</button>

<script>
  const btn = document.getElementById('themeToggle');
  btn.addEventListener('click', () => {
    const isDark = document.documentElement.getAttribute('data-bs-theme') === 'dark';
    btn.setAttribute('aria-label', isDark ? 'Switch to light mode' : 'Switch to dark mode');
  });
</script>
```

Do not rely on animations during theme transitions for users who have `prefers-reduced-motion` enabled. Add a `transition: none` rule under that media query.

## Responsive Behavior

Dark mode operates independently of viewport size. The theme attribute does not change at breakpoints, and the visual effect is consistent across all screen sizes.

However, layout changes at different breakpoints can affect how dark mode surfaces appear. A dark sidebar on desktop that collapses to a hamburger menu on mobile still renders dark, but its reduced footprint changes the visual balance of the page.

```html
<div class="row">
  <nav class="col-md-3 col-lg-2 d-md-block bg-dark text-white sidebar"
       data-bs-theme="dark">
    <div class="position-sticky pt-3">
      <ul class="nav flex-column">
        <li class="nav-item">
          <a class="nav-link text-white" href="#">Dashboard</a>
        </li>
      </ul>
    </div>
  </nav>
  <main class="col-md-9 ms-sm-auto col-lg-10 px-md-4">
    <!-- Main content uses the default theme -->
  </main>
</div>
```

For responsive theme toggling (e.g., dark mode only on mobile), combine `data-bs-theme` with JavaScript viewport detection:

```javascript
const applyResponsiveTheme = () => {
  const isMobile = window.innerWidth < 768;
  document.documentElement.setAttribute(
    'data-bs-theme',
    isMobile ? 'dark' : 'light'
  );
};

window.addEventListener('resize', applyResponsiveTheme);
applyResponsiveTheme();
```

This pattern is uncommon and should be used sparingly — users generally expect a consistent theme regardless of device size.