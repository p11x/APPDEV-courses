---
tags:
  - bootstrap
  - custom-theme
  - sass
  - scss
  - theming
  - advanced
category: Bootstrap Fundamentals
difficulty: 3
time: 60 minutes
---

# Custom Theme Creation

## Overview

Bootstrap 5's default color palette is a starting point, not a constraint. Custom theme creation involves overriding Sass variables, extending color maps, and recompiling Bootstrap's CSS to produce a stylesheet that reflects your brand identity.

The `$theme-colors` Sass map is the primary customization point. It defines the contextual colors used by every Bootstrap component and utility. By modifying this map before importing Bootstrap's source, you control the entire color system with a single configuration block.

Bootstrap's architecture separates configuration from implementation. Variables are defined in `_variables.scss`, then consumed by component partials. This means your overrides must be loaded before Bootstrap's source files. The recommended approach uses a custom entry point that defines variables, then imports Bootstrap.

Beyond colors, you can customize spacing, typography, borders, shadows, and component-specific variables. The `$theme-colors-rgb` map must stay synchronized with `$theme-colors` — Bootstrap provides the `to-rgb` function to automate this. Failing to sync these maps breaks opacity utilities.

Custom theme creation requires a Sass build pipeline. You cannot override Sass variables with plain CSS alone. While CSS custom properties allow runtime theming, the Sass approach compiles a fixed set of values into the output, making it ideal for brand-specific design systems.

## Basic Implementation

The standard setup creates a custom Sass entry point that overrides variables before importing Bootstrap:

```scss
// custom.scss

// 1. Override default variables BEFORE importing Bootstrap
$primary: #6c5ce7;
$secondary: #a29bfe;
$success: #00b894;
$danger: #d63031;
$warning: #fdcb6e;
$info: #74b9ff;
$light: #f8f9fa;
$dark: #2d3436;

// 2. Import Bootstrap's source
@import "bootstrap/scss/bootstrap";
```

Compile this with the Sass CLI:

```bash
sass custom.scss dist/css/custom-bootstrap.css --style compressed
```

Or with npm scripts in `package.json`:

```json
{
  "scripts": {
    "css:compile": "sass custom.scss dist/css/custom-bootstrap.css --style compressed",
    "css:watch": "sass --watch custom.scss dist/css/custom-bootstrap.css"
  }
}
```

The compiled output includes all of Bootstrap's CSS with your custom colors applied. Every component — buttons, alerts, badges, cards — reflects the new palette.

To extend the color map with additional semantic colors:

```scss
// Add brand-specific colors to the theme map
$theme-colors: (
  "primary": #6c5ce7,
  "secondary": #a29bfe,
  "success": #00b894,
  "danger": #d63031,
  "warning": #fdcb6e,
  "info": #74b9ff,
  "light": #f8f9fa,
  "dark": #2d3436,
  "brand": #e17055,
  "accent": #00cec9,
);

@import "bootstrap/scss/bootstrap";
```

This generates new utilities like `text-brand`, `bg-brand`, `btn-brand`, `alert-brand`, and `badge-brand` automatically.

## Advanced Variations

Remove colors you do not need from the theme map:

```scss
// Remove 'info' and 'warning' from the theme
$theme-colors: map-remove($theme-colors, "info", "warning");

@import "bootstrap/scss/bootstrap";
```

Create a completely custom map without Bootstrap's defaults:

```scss
$theme-colors: (
  "primary": #1a1a2e,
  "secondary": #16213e,
  "accent": #e94560,
  "neutral": #0f3460,
  "surface": #f5f5f5,
);

// Generate RGB map for opacity utilities
$theme-colors-rgb: to-rgb($theme-colors);

@import "bootstrap/scss/bootstrap";
```

Customize component-specific variables for fine-grained control:

```scss
// Button customization
$btn-border-radius: 0.5rem;
$btn-font-weight: 600;
$btn-padding-y: 0.75rem;
$btn-padding-x: 1.5rem;

// Card customization
$card-border-radius: 1rem;
$card-border-width: 0;
$card-box-shadow: 0 2px 15px rgba(0, 0, 0, 0.08);

// Navbar customization
$navbar-light-color: rgba(0, 0, 0, 0.7);
$navbar-light-hover-color: rgba(0, 0, 0, 0.9);
$navbar-light-active-color: $primary;

// Alert customization
$alert-border-width: 0;
$alert-border-radius: 0.75rem;

// Form controls
$input-border-radius: 0.5rem;
$input-focus-border-color: $primary;
$input-placeholder-color: rgba(0, 0, 0, 0.4);

@import "bootstrap/scss/bootstrap";
```

Create a dark variant theme using a separate build:

```scss
// dark-theme.scss
$theme-colors: (
  "primary": #bb86fc,
  "secondary": #03dac6,
  "success": #66bb6a,
  "danger": #ef5350,
  "warning": #ffa726,
  "info": #29b6f6,
  "light": #1e1e1e,
  "dark": #121212,
);

$body-bg: #121212;
$body-color: #e0e0e0;
$card-bg: #1e1e1e;
$input-bg: #2d2d2d;
$input-color: #e0e0e0;
$input-border-color: #444;

@import "bootstrap/scss/bootstrap";
```

Using Sass maps for a multi-brand system:

```scss
// Define brand palettes
$brands: (
  "acme": (
    "primary": #007bff,
    "secondary": #6c757d,
  ),
  "globex": (
    "primary": #e84393,
    "secondary": #6c5ce7,
  ),
);

// Compile each brand separately
@each $brand-name, $brand-colors in $brands {
  $theme-colors: map-merge($theme-colors, $brand-colors);

  .brand-#{$brand-name} {
    // Generate brand-specific CSS variables
    @each $color, $value in $brand-colors {
      --bs-#{$color}: #{$value};
      --bs-#{$color}-rgb: #{to-rgb($value)};
    }
  }
}

@import "bootstrap/scss/bootstrap";
```

```html
<!-- Apply brand via class -->
<div class="brand-acme">
  <button class="btn btn-primary">Acme Primary</button>
</div>

<div class="brand-globex">
  <button class="btn btn-primary">Globex Primary</button>
</div>
```

## Best Practices

1. **Override variables before importing Bootstrap's source.** Sass variable overrides are compile-time. Placing them after `@import "bootstrap/scss/bootstrap"` has no effect.

2. **Always sync `$theme-colors-rgb` with `$theme-colors`.** Use the `to-rgb()` function: `$theme-colors-rgb: to-rgb($theme-colors)`. Missing this step breaks `bg-opacity-*` and `text-opacity-*` utilities.

3. **Use the `$theme-colors` map, not individual `$primary` variables.** Changing `$primary` alone does not update the `$theme-colors` map. Bootstrap's components reference `$theme-colors`, so the map is the source of truth.

4. **Test compiled output for all components.** After compiling, verify buttons, alerts, badges, cards, modals, forms, and tables. A change that looks correct on buttons may produce unexpected results in alerts.

5. **Keep custom Sass in a separate file.** Never modify Bootstrap's `node_modules` source directly. Create `custom.scss` and import Bootstrap from within it.

6. **Use `map-merge` to extend rather than replace.** When adding colors, merge with the existing map instead of redefining it entirely:

   ```scss
   $theme-colors: map-merge($theme-colors, ("brand": #e17055));
   ```

7. **Document your color tokens.** List every color in the `$theme-colors` map with its semantic purpose. This prevents accidental duplication and helps designers reference the correct tokens.

8. **Version your theme files.** When Bootstrap updates, your overrides may conflict with new defaults. Keep theme files under version control and review changes during upgrades.

9. **Compile both light and dark variants as separate files.** Use different entry points (`light.scss`, `dark.scss`) with different variable overrides, then switch between them with `data-bs-theme`.

10. **Validate Sass compilation with `--style expanded` during development.** Compressed output is hard to debug. Use expanded output in dev and compressed in production.

11. **Use `$enable-gradients`, `$enable-shadows`, and `$enable-rounded` to match brand style.** These flags globally toggle visual features without per-component overrides.

12. **Avoid `!important` in theme overrides.** If specificity is insufficient, restructure the import order or use more specific selectors instead of forcing overrides with `!important`.

## Common Pitfalls

1. **Overriding `$primary` without updating `$theme-colors`.** The `$primary` variable is used in some places, but `$theme-colors` drives the utility and component systems. Both must be updated.

2. **Forgetting to compile after changing variables.** Sass variables are compile-time constants. Changing them in a `.scss` file has no effect until the file is recompiled.

3. **Placing overrides after the Bootstrap import.** Sass processes files top-to-bottom. Variables defined after `@import` are too late to override Bootstrap's defaults.

4. **Not updating `$theme-colors-rgb` after changing `$theme-colors`.** The RGB map powers opacity utilities. Without it, `bg-primary bg-opacity-50` renders as fully opaque.

5. **Hardcoding values in components instead of using variables.** If a button uses `background: #6c5ce7` directly, it bypasses the theme system and will not respond to future changes.

6. **Removing a color from `$theme-colors` without checking dependencies.** Other variables (like `$component-active-color`) may reference the removed color, causing compilation errors.

7. **Mixing CSS custom property overrides with Sass compilation.** If you set `--bs-primary` in CSS and also override `$primary` in Sass, the CSS value wins at runtime. This can cause confusion about which value is active.

8. **Not testing dark mode with custom themes.** A custom palette designed for light mode may have contrast issues in dark mode. Compile and test both variants.

9. **Assuming custom colors generate all component variants automatically.** Bootstrap's component Sass iterates over `$theme-colors` to generate variants, so custom colors do produce buttons, alerts, etc. However, ensure the compiled output confirms this.

10. **Using `@import` in Sass without proper load paths.** The `@import "bootstrap/scss/bootstrap"` line requires `node_modules` in the load path. Configure your build tool with `--load-path=node_modules`.

## Accessibility Considerations

Custom themes must maintain the same accessibility standards as Bootstrap's defaults. When overriding colors, verify that all text/background combinations meet WCAG 2.1 AA contrast ratios (4.5:1 for normal text, 3:1 for large text).

Bootstrap's default colors are pre-validated for contrast. Custom colors require manual validation. Use tools like WebAIM's Contrast Checker or browser DevTools to test every color pair.

```scss
// Verify these pairs meet 4.5:1 contrast:
// $primary on white (for text-primary)
// white on $primary (for btn-primary)
// $danger on white (for text-danger)
// white on $danger (for btn-danger)
// $warning on dark (for bg-warning text)
```

When adding custom colors to `$theme-colors`, determine whether the foreground text should be white or dark:

```scss
$custom-colors: (
  "brand": #e17055,    // Orange — use white text
  "muted": #b2bec3,    // Light gray — use dark text
);

// Bootstrap auto-generates btn-brand, alert-brand, etc.
// but verify the text color contrast manually.
```

Focus indicators derive from theme colors. If your primary color is low-contrast against the background, focus outlines will be invisible. Always test keyboard navigation with custom themes.

Ensure that custom alert and badge colors provide sufficient differentiation. If `alert-brand` and `alert-warning` look too similar, users with color vision deficiencies cannot distinguish them.

## Responsive Behavior

Custom themes apply uniformly across all breakpoints. The compiled CSS contains the same values at every viewport width. However, you can create responsive theme behavior through conditional Sass compilation:

```scss
// Compile separate themes for mobile and desktop
// mobile-theme.scss
$theme-colors: (
  "primary": #007bff,
  // ... simplified palette for mobile
);
@import "bootstrap/scss/bootstrap";

// desktop-theme.scss
$theme-colors: (
  "primary": #6c5ce7,
  // ... full palette for desktop
);
@import "bootstrap/scss/bootstrap";
```

```html
<link rel="stylesheet" href="mobile-theme.css" media="(max-width: 767px)">
<link rel="stylesheet" href="desktop-theme.css" media="(min-width: 768px)">
```

This approach doubles the CSS payload and is rarely necessary. A more practical pattern uses CSS custom properties that change at breakpoints:

```css
:root {
  --bs-primary: #6c5ce7;
}

@media (max-width: 767px) {
  :root {
    --bs-primary: #007bff;
  }
}
```

Most custom themes work responsively without modification. The primary concern is ensuring that customized components (cards, navbars, forms) remain usable at all viewport sizes, which is a layout concern rather than a theming concern.