---
title: "Token Toolchain"
difficulty: 3
category: "Advanced Development"
subcategory: "Design Systems"
prerequisites:
  - Style Dictionary
  - Node.js Scripting
  - JSON/YAML Token Formats
---

## Overview

Style Dictionary is the industry-standard tool for transforming design tokens from a source format into platform-specific outputs. In a Bootstrap context, the token toolchain reads design tokens from JSON or YAML files and generates SCSS variables that override Bootstrap's defaults, CSS custom properties for runtime theming, and potentially TypeScript constants for JavaScript consumption.

The pipeline handles reference resolution (semantic tokens referencing global tokens), value transformation (converting pixel values to rem, color formats), and format-specific output (SCSS maps, CSS variables, JSON for JavaScript). Custom transforms and formats extend Style Dictionary to handle Bootstrap-specific needs like mapping tokens to Bootstrap's variable naming conventions.

## Basic Implementation

```js
// style-dictionary.config.js
module.exports = {
  source: ['tokens/**/*.json'],
  platforms: {
    scss: {
      transformGroup: 'scss',
      buildPath: 'scss/',
      files: [{
        destination: '_tokens.scss',
        format: 'scss/variables',
        filter: (token) => token.type === 'color' || token.type === 'dimension'
      }]
    },
    css: {
      transformGroup: 'css',
      buildPath: 'css/',
      files: [{
        destination: 'tokens.css',
        format: 'css/variables',
        options: {
          outputReferences: true
        }
      }]
    },
    js: {
      transformGroup: 'js',
      buildPath: 'js/',
      files: [{
        destination: 'tokens.js',
        format: 'javascript/es6'
      }]
    }
  }
};
```

```json
// tokens/colors.json
{
  "color": {
    "brand": {
      "primary": { "value": "#0d6efd", "type": "color" },
      "secondary": { "value": "#6c757d", "type": "color" }
    },
    "semantic": {
      "success": { "value": "#198754", "type": "color" },
      "danger": { "value": "#dc3545", "type": "color" },
      "warning": { "value": "#ffc107", "type": "color" }
    }
  }
}
```

```scss
// Generated: scss/_tokens.scss
$color-brand-primary: #0d6efd;
$color-brand-secondary: #6c757d;
$color-semantic-success: #198754;
$color-semantic-danger: #dc3545;
$color-semantic-warning: #ffc107;

// Bootstrap mapping
$primary: $color-brand-primary;
$secondary: $color-brand-secondary;
$success: $color-semantic-success;
$danger: $color-semantic-danger;
$warning: $color-semantic-warning;
```

## Advanced Variations

```js
// Custom Style Dictionary transform for Bootstrap
const StyleDictionary = require('style-dictionary');

StyleDictionary.registerTransform({
  name: 'size/rem',
  type: 'value',
  matcher: (token) => token.type === 'dimension' && token.value !== 0,
  transformer: (token) => `${token.value / 16}rem`
});

StyleDictionary.registerTransform({
  name: 'name/bootstrap',
  type: 'name',
  transformer: (token) => {
    // Convert token path to Bootstrap variable name
    // color.brand.primary → $primary
    const bootstrapMap = {
      'color.brand.primary': 'primary',
      'color.brand.secondary': 'secondary',
      'color.semantic.success': 'success',
      'color.semantic.danger': 'danger',
      'color.semantic.warning': 'warning'
    };
    const path = token.path.join('.');
    return bootstrapMap[path] || path.replace(/\./g, '-');
  }
});

StyleDictionary.registerFormat({
  name: 'scss/bootstrap',
  formatter: ({ dictionary }) => {
    return dictionary.allTokens
      .map(token => `$${token.name}: ${token.value};`)
      .join('\n');
  }
});
```

## Best Practices

1. **Use Style Dictionary** - It's the standard tool with extensive format and transform support.
2. **Single source of truth** - One token set generates all platform outputs.
3. **Reference resolution** - Use `{ref.path}` syntax for semantic tokens referencing global tokens.
4. **Custom transforms for Bootstrap** - Map token names to Bootstrap's variable naming conventions.
5. **Validate before build** - Check token structure, required fields, and value formats.
6. **Version the token schema** - Track which token format version your pipeline supports.
7. **Automate builds** - Generate tokens on every commit via CI.
8. **Test generated output** - Verify SCSS compiles, CSS is valid, and JS exports correctly.
9. **Document custom transforms** - Explain what each custom transform does and why.
10. **Keep tokens in version control** - Track token changes with git history.

## Common Pitfalls

1. **Circular references** - Token A references B which references A; resolve with topological sort.
2. **Unit mismatches** - Tokens in pixels but SCSS expects rem; add unit transform.
3. **Stale generated files** - Not rebuilding after token changes causes out-of-sync code.
4. **Over-complex transforms** - Transform logic too complex makes debugging difficult.
5. **Missing output references** - Not using `outputReferences: true` loses the alias chain in CSS.

## Accessibility Considerations

Include contrast ratio metadata in tokens and generate accessibility reports showing which color combinations meet WCAG requirements.

## Responsive Behavior

Include breakpoint-specific tokens that transform into responsive SCSS mixins or CSS media queries.

```json
{
  "spacing": {
    "section": {
      "mobile": { "value": "2rem", "type": "dimension" },
      "desktop": { "value": "4rem", "type": "dimension" }
    }
  }
}
```
