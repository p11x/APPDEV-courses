---
title: Runtime Theming with JavaScript
category: Bootstrap Fundamentals
difficulty: 2
time: 25 min
tags: bootstrap5, css-variables, runtime, javascript, theme-switcher, localStorage
---

## Overview

CSS custom properties can be modified at runtime using JavaScript, enabling dynamic theme switching without page reloads or CSS recompilation. Bootstrap 5's CSS variable architecture makes it straightforward to change colors, spacing, typography, and component styling in real-time. Combined with `localStorage` for persistence, you can build theme switchers that remember user preferences across sessions. This approach is used for implementing light/dark mode toggles, brand customization interfaces, and accessibility-focused contrast adjustments.

## Basic Implementation

Modify CSS variables with JavaScript using `setProperty()` on the document element.

```html
<!-- Theme toggle button -->
<button class="btn btn-outline-primary" id="themeToggle">Toggle Dark Mode</button>

<script>
  const toggle = document.getElementById('themeToggle');

  toggle.addEventListener('click', () => {
    const root = document.documentElement;
    const currentBg = getComputedStyle(root).getPropertyValue('--bs-body-bg').trim();

    if (currentBg === '#fff' || currentBg === '') {
      // Switch to dark theme
      root.style.setProperty('--bs-body-bg', '#212529');
      root.style.setProperty('--bs-body-color', '#dee2e6');
      root.style.setProperty('--bs-emphasis-color', '#ffffff');
      root.style.setProperty('--bs-secondary-color', 'rgba(222, 226, 230, 0.75)');
      root.style.setProperty('--bs-secondary-bg', '#343a40');
      root.style.setProperty('--bs-border-color', '#495057');
    } else {
      // Switch to light theme
      root.style.setProperty('--bs-body-bg', '#fff');
      root.style.setProperty('--bs-body-color', '#212529');
      root.style.setProperty('--bs-emphasis-color', '#000');
      root.style.setProperty('--bs-secondary-color', 'rgba(33, 37, 41, 0.75)');
      root.style.setProperty('--bs-secondary-bg', '#e9ecef');
      root.style.setProperty('--bs-border-color', '#dee2e6');
    }
  });
</script>
```

## Advanced Variations

A complete theme switcher with multiple themes and localStorage persistence.

```html
<!-- Theme selector UI -->
<div class="btn-group" role="group" aria-label="Theme selector">
  <button class="btn btn-outline-secondary" data-theme="light">Light</button>
  <button class="btn btn-outline-secondary" data-theme="dark">Dark</button>
  <button class="btn btn-outline-secondary" data-theme="high-contrast">High Contrast</button>
</div>

<script>
  const themes = {
    light: {
      '--bs-body-bg': '#ffffff',
      '--bs-body-color': '#212529',
      '--bs-emphasis-color': '#000000',
      '--bs-secondary-color': 'rgba(33, 37, 41, 0.75)',
      '--bs-secondary-bg': '#e9ecef',
      '--bs-border-color': '#dee2e6',
      '--bs-primary': '#0d6efd',
      '--bs-primary-rgb': '13, 110, 253',
    },
    dark: {
      '--bs-body-bg': '#121212',
      '--bs-body-color': '#e0e0e0',
      '--bs-emphasis-color': '#ffffff',
      '--bs-secondary-color': 'rgba(224, 224, 224, 0.7)',
      '--bs-secondary-bg': '#1e1e1e',
      '--bs-border-color': '#333333',
      '--bs-primary': '#6ea8fe',
      '--bs-primary-rgb': '110, 168, 254',
    },
    'high-contrast': {
      '--bs-body-bg': '#000000',
      '--bs-body-color': '#ffffff',
      '--bs-emphasis-color': '#ffffff',
      '--bs-secondary-color': 'rgba(255, 255, 255, 0.7)',
      '--bs-secondary-bg': '#1a1a1a',
      '--bs-border-color': '#ffffff',
      '--bs-primary': '#ffff00',
      '--bs-primary-rgb': '255, 255, 0',
    },
  };

  function applyTheme(themeName) {
    const root = document.documentElement;
    const theme = themes[themeName];

    Object.entries(theme).forEach(([variable, value]) => {
      root.style.setProperty(variable, value);
    });

    localStorage.setItem('selectedTheme', themeName);
    document.body.setAttribute('data-theme', themeName);
  }

  // Initialize from localStorage
  const savedTheme = localStorage.getItem('selectedTheme') || 'light';
  applyTheme(savedTheme);

  // Bind theme buttons
  document.querySelectorAll('[data-theme]').forEach(button => {
    button.addEventListener('click', () => {
      applyTheme(button.dataset.theme);
    });
  });
</script>
```

Automatic system theme detection with manual override support.

```html
<script>
  function getPreferredTheme() {
    const saved = localStorage.getItem('selectedTheme');
    if (saved) return saved;
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }

  // Apply on load (place in <head> to prevent flash)
  function initTheme() {
    const theme = getPreferredTheme();
    applyTheme(theme);
  }

  // Listen for system theme changes
  window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
    if (!localStorage.getItem('selectedTheme')) {
      applyTheme(e.matches ? 'dark' : 'light');
    }
  });

  initTheme();
</script>
```

## Best Practices

1. **Persist theme in localStorage** - Save the user's theme choice with `localStorage.setItem()` and restore on page load.
2. **Detect system preference** - Use `prefers-color-scheme` media query as the default theme before checking localStorage.
3. **Prevent flash of wrong theme** - Apply the theme in a blocking script in `<head>` before the page renders.
4. **Use CSS variable names consistently** - Define theme objects with all necessary variables to avoid incomplete theming.
5. **Provide a clear theme toggle** - Make the theme switcher accessible with proper ARIA labels and keyboard support.
6. **Announce theme changes** - Use ARIA live regions to inform screen readers when the theme changes.
7. **Support system preference changes** - Listen for `prefers-color-scheme` changes and update the theme automatically if no manual override is set.
8. **Allow clearing manual preference** - Provide an option to follow system preference instead of a saved manual choice.
9. **Use requestAnimationFrame** - When applying multiple variable changes, batch them in a single animation frame for smooth transitions.
10. **Test with CSS transitions** - Add `transition: background-color 0.3s, color 0.3s` to body for smooth theme switching.

## Common Pitfalls

1. **Flash of unstyled content** - Without a blocking script in `<head>`, the page loads with the default theme before switching, causing a visible flash.
2. **localStorage unavailability** - Some browsers restrict localStorage (private browsing, disabled cookies). Wrap in try/catch for graceful fallback.
3. **Forgetting to persist** - Theme changes without localStorage persistence reset on every page navigation.
4. **Incomplete theme definitions** - Missing variables in theme objects cause some components to retain the default theme while others change.
5. **Race conditions in single-page apps** - Theme changes may conflict with framework re-renders. Apply theme changes after the DOM is stable.

## Accessibility Considerations

Theme switchers must be keyboard accessible and provide clear visual indicators. Use `aria-label` on toggle buttons that only contain icons. Support the operating system's high contrast mode alongside custom themes. Ensure that all theme options maintain WCAG contrast requirements. Announce theme changes using `aria-live="polite"` regions. Provide a "follow system preference" option that respects the user's OS-level accessibility settings. Test every theme with screen readers, keyboard-only navigation, and browser zoom at 200%.

## Responsive Behavior

Theme switching itself is not viewport-dependent, but the visual impact of themes varies across screen sizes. Dark themes may be preferred on mobile devices for reduced battery usage on OLED screens. Consider detecting mobile devices and defaulting to dark themes. Responsive layouts should remain consistent across themes. Test all themes at multiple viewport sizes to ensure that color contrast and readability are maintained on small screens where ambient lighting conditions are more variable.
