---
title: "Multi-Brand Theming"
difficulty: 3
category: "Advanced Development"
subcategory: "Design Systems"
prerequisites:
  - Bootstrap 5 Color Modes
  - CSS Custom Properties
  - SCSS Maps and Loops
---

## Overview

Multi-brand theming extends Bootstrap's color mode system to support multiple brand identities within a single codebase. White-label architecture allows applications to switch between brands dynamically, with each brand defining its own color palette, typography, spacing, and component-level overrides.

The architecture uses CSS custom properties scoped to brand-specific selectors or data attributes. Bootstrap 5.3's color mode system provides the foundation; multi-brand theming extends it from two modes (light/dark) to N brands, each with light and dark variants.

## Basic Implementation

```scss
// scss/themes/_brands.scss
$brands: (
  'brand-a': (
    'primary': #2563eb,
    'secondary': #7c3aed,
    'accent': #f59e0b,
    'bg': #ffffff,
    'text': #1e293b,
    'font-heading': "'Inter', sans-serif",
    'radius': 0.5rem
  ),
  'brand-b': (
    'primary': #059669,
    'secondary': #0891b2,
    'accent': #e11d48,
    'bg': #fafafa,
    'text': #111827,
    'font-heading': "'Poppins', sans-serif",
    'radius': 0.75rem
  ),
  'brand-c': (
    'primary': #7c2d12,
    'secondary': #92400e,
    'accent': #15803d,
    'bg': #fffbeb,
    'text': #292524,
    'font-heading': "'Playfair Display', serif",
    'radius': 0.25rem
  )
);

// Generate brand CSS custom properties
@each $brand, $properties in $brands {
  [data-brand="#{$brand}"] {
    @each $key, $value in $properties {
      --brand-#{$key}: #{$value};
    }

    // Map to Bootstrap variables
    --bs-primary: var(--brand-primary);
    --bs-secondary: var(--brand-secondary);
    --bs-body-bg: var(--brand-bg);
    --bs-body-color: var(--brand-text);
    --bs-border-radius: var(--brand-radius);

    // Brand-specific font
    --bs-heading-font: var(--brand-font-heading);
  }
}
```

```html
<!-- Brand switching -->
<html data-brand="brand-a" data-bs-theme="light">
<head>
  <link rel="stylesheet" href="dist/css/brands.css">
</head>
<body>
  <nav class="navbar bg-primary" data-bs-theme="dark">
    <div class="container">
      <a class="navbar-brand" href="#">Logo</a>
      <select id="brandSwitcher" class="form-select" style="width: auto;">
        <option value="brand-a">Brand A</option>
        <option value="brand-b">Brand B</option>
        <option value="brand-c">Brand C</option>
      </select>
    </div>
  </nav>

  <script>
    document.getElementById('brandSwitcher').addEventListener('change', (e) => {
      document.documentElement.dataset.brand = e.target.value;
      localStorage.setItem('preferred-brand', e.target.value);
    });

    // Restore saved brand
    const saved = localStorage.getItem('preferred-brand');
    if (saved) document.documentElement.dataset.brand = saved;
  </script>
</body>
</html>
```

```scss
// Brand-aware component styles
.card {
  border-radius: var(--brand-radius, $border-radius);
  border: 1px solid rgba(var(--bs-primary-rgb), 0.1);

  .card-title {
    font-family: var(--bs-heading-font, inherit);
    color: var(--bs-primary);
  }
}

.btn-primary {
  border-radius: var(--brand-radius, $border-radius);
  font-family: var(--bs-heading-font, inherit);
}
```

## Advanced Variations

```js
// Brand configuration as JSON for white-label builds
// brands/brand-a.json
{
  "name": "Brand A",
  "colors": {
    "primary": "#2563eb",
    "secondary": "#7c3aed"
  },
  "typography": {
    "heading": "Inter",
    "body": "Inter"
  },
  "assets": {
    "logo": "./assets/brand-a/logo.svg",
    "favicon": "./assets/brand-a/favicon.ico"
  },
  "build": {
    "outputDir": "./dist/brand-a",
    "htmlTitle": "Brand A Application"
  }
}

// Build script for multi-brand
// scripts/build-brands.js
const sass = require('sass');
const fs = require('fs-extra');
const path = require('path');

async function buildBrand(brandConfig) {
  const scss = `
    @import 'bootstrap/scss/functions';
    @import 'bootstrap/scss/variables';
    $primary: ${brandConfig.colors.primary};
    $secondary: ${brandConfig.colors.secondary};
    @import 'bootstrap/scss/bootstrap';
  `;

  const result = sass.compileString(scss, {
    loadPaths: ['node_modules']
  });

  const outDir = brandConfig.build.outputDir;
  await fs.ensureDir(outDir);
  await fs.writeFile(path.join(outDir, 'main.css'), result.css);
  console.log(`Built ${brandConfig.name} → ${outDir}/main.css`);
}

// Build all brands
const brandFiles = fs.readdirSync('brands').filter(f => f.endsWith('.json'));
brandFiles.forEach(file => {
  const config = JSON.parse(fs.readFileSync(path.join('brands', file), 'utf8'));
  buildBrand(config);
});
```

## Best Practices

1. **Use data attributes for brand switching** - `[data-brand="x"]` enables runtime switching without page reload.
2. **Scope brands to CSS custom properties** - All brand-specific values should be CSS variables.
3. **Support light and dark per brand** - Each brand should define both light and dark mode values.
4. **Store brand preference** - Persist the selected brand in localStorage or cookies.
5. **Provide brand fallbacks** - CSS custom properties should have sensible fallbacks.
6. **Build separate brand bundles** - For white-label deployments, compile brand-specific CSS files.
7. **Test all brands equally** - Every brand should have the same test coverage.
8. **Document brand configuration** - Provide a clear schema for brand JSON files.
9. **Keep component logic brand-agnostic** - JavaScript should never reference brand-specific values.
10. **Use brand-specific assets** - Logos, favicons, and images should be part of the brand configuration.

## Common Pitfalls

1. **Hardcoded brand colors** - Using brand-specific hex values in component SCSS breaks brand switching.
2. **Missing brand fallbacks** - CSS variables without fallbacks render incorrectly when no brand is set.
3. **Not testing all brands** - Layout issues in one brand go unnoticed if only the default brand is tested.
4. **Complexity explosion** - Supporting too many brands increases maintenance cost exponentially.
5. **Flash of unstyled content** - Brand switching via JS after page load causes visual flashing.

## Accessibility Considerations

Each brand must independently meet WCAG contrast requirements. Dark mode per brand requires separate contrast validation.

## Responsive Behavior

Brands should maintain consistent identity across all breakpoints. Typography scaling and spacing adjustments should be proportional per brand.

```scss
[data-brand="brand-a"] {
  --brand-heading-scale-mobile: 1;
  --brand-heading-scale-desktop: 1.25;
}

[data-brand="brand-b"] {
  --brand-heading-scale-mobile: 1.1;
  --brand-heading-scale-desktop: 1.5;
}
```
