---
title: "Design Token Architecture"
difficulty: 3
category: "Advanced Development"
subcategory: "Design Systems"
prerequisites:
  - CSS Custom Properties
  - SCSS Map Architecture
  - Design Token Format (DTCG)
---

## Overview

Design tokens are the atomic building blocks of a design system, representing design decisions as platform-agnostic data. In a Bootstrap-based design system, tokens bridge the gap between design tools and code by defining colors, typography, spacing, and other visual properties in a structured format that transforms into SCSS variables, CSS custom properties, and platform-specific outputs.

The token hierarchy follows three levels: global tokens (raw values like `#0d6efd`), semantic tokens (purpose-based aliases like `--color-primary`), and component tokens (scoped values like `--btn-primary-bg`). This hierarchy enables theming at every level while maintaining a single source of truth for design decisions.

A token transformation pipeline reads tokens in a standard format (JSON or YAML) and outputs platform-specific formats: SCSS variables for preprocessor consumers, CSS custom properties for runtime theming, iOS color sets, Android XML resources, and Tailwind config values.

## Basic Implementation

```json
// tokens/global.json
{
  "color": {
    "blue": {
      "100": { "value": "#cfe2ff", "type": "color" },
      "500": { "value": "#0d6efd", "type": "color" },
      "900": { "value": "#031e44", "type": "color" }
    },
    "gray": {
      "100": { "value": "#f8f9fa", "type": "color" },
      "500": { "value": "#6c757d", "type": "color" },
      "900": { "value": "#212529", "type": "color" }
    }
  },
  "spacing": {
    "1": { "value": "0.25rem", "type": "dimension" },
    "2": { "value": "0.5rem", "type": "dimension" },
    "3": { "value": "1rem", "type": "dimension" },
    "4": { "value": "1.5rem", "type": "dimension" },
    "5": { "value": "3rem", "type": "dimension" }
  },
  "font": {
    "family": {
      "base": { "value": "'Inter', sans-serif", "type": "fontFamily" },
      "heading": { "value": "'Inter', sans-serif", "type": "fontFamily" },
      "mono": { "value": "'JetBrains Mono', monospace", "type": "fontFamily" }
    },
    "size": {
      "sm": { "value": "0.875rem", "type": "dimension" },
      "base": { "value": "1rem", "type": "dimension" },
      "lg": { "value": "1.25rem", "type": "dimension" },
      "xl": { "value": "1.5rem", "type": "dimension" }
    }
  }
}
```

```json
// tokens/semantic.json
{
  "color": {
    "brand": {
      "primary": { "value": "{color.blue.500}", "type": "color" },
      "secondary": { "value": "{color.gray.500}", "type": "color" }
    },
    "text": {
      "primary": { "value": "{color.gray.900}", "type": "color" },
      "muted": { "value": "{color.gray.500}", "type": "color" },
      "inverse": { "value": "{color.gray.100}", "type": "color" }
    },
    "background": {
      "primary": { "value": "#ffffff", "type": "color" },
      "secondary": { "value": "{color.gray.100}", "type": "color" }
    }
  }
}
```

```scss
// Generated output: scss/_tokens.scss
// Auto-generated - do not edit manually

// Global tokens
$color-blue-100: #cfe2ff;
$color-blue-500: #0d6efd;
$color-blue-900: #031e44;
$color-gray-100: #f8f9fa;
$color-gray-500: #6c757d;
$color-gray-900: #212529;

$spacing-1: 0.25rem;
$spacing-2: 0.5rem;
$spacing-3: 1rem;
$spacing-4: 1.5rem;
$spacing-5: 3rem;

// Semantic tokens (references resolved)
$color-brand-primary: $color-blue-500;
$color-brand-secondary: $color-gray-500;
$color-text-primary: $color-gray-900;
$color-text-muted: $color-gray-500;
$color-background-primary: #ffffff;
$color-background-secondary: $color-gray-100;

// Bootstrap variable mapping
$primary: $color-brand-primary;
$secondary: $color-brand-secondary;
$body-color: $color-text-primary;
$body-bg: $color-background-primary;
```

```scss
// Generated output: css/_tokens.css
:root {
  --color-blue-100: #cfe2ff;
  --color-blue-500: #0d6efd;
  --color-blue-900: #031e44;
  --color-gray-100: #f8f9fa;
  --color-gray-500: #6c757d;
  --color-gray-900: #212529;
  --color-brand-primary: var(--color-blue-500);
  --color-brand-secondary: var(--color-gray-500);
  --color-text-primary: var(--color-gray-900);
  --color-text-muted: var(--color-gray-500);
  --spacing-1: 0.25rem;
  --spacing-2: 0.5rem;
  --spacing-3: 1rem;
}
```

## Advanced Variations

```js
// Token transformation pipeline
// scripts/build-tokens.js
const fs = require('fs');
const path = require('path');
const yaml = require('js-yaml');

class TokenTransformer {
  constructor() {
    this.tokens = {};
    this.resolved = {};
  }

  load(filePath) {
    const content = fs.readFileSync(filePath, 'utf8');
    const parsed = filePath.endsWith('.yaml')
      ? yaml.load(content)
      : JSON.parse(content);
    this.tokens = this._deepMerge(this.tokens, parsed);
    return this;
  }

  resolve() {
    this.resolved = this._resolveReferences(this.tokens);
    return this;
  }

  _resolveReferences(obj, root = obj) {
    const resolved = {};
    for (const [key, value] of Object.entries(obj)) {
      if (typeof value === 'object' && value.value) {
        resolved[key] = {
          ...value,
          value: this._resolveValue(value.value, root)
        };
      } else if (typeof value === 'object') {
        resolved[key] = this._resolveReferences(value, root);
      } else {
        resolved[key] = value;
      }
    }
    return resolved;
  }

  _resolveValue(value, root) {
    return value.replace(/\{([^}]+)\}/g, (_, ref) => {
      const path = ref.split('.');
      let current = root;
      for (const segment of path) {
        current = current[segment];
      }
      return current.value || current;
    });
  }

  toScss() {
    const lines = ['// Auto-generated tokens\n'];
    this._flatten(this.resolved, '').forEach(([path, value]) => {
      const name = path.replace(/\./g, '-').toLowerCase();
      lines.push(`$${name}: ${value};`);
    });
    return lines.join('\n');
  }

  toCss() {
    const lines = [':root {'];
    this._flatten(this.resolved, '').forEach(([path, value]) => {
      const name = `--${path.replace(/\./g, '-').toLowerCase()}`;
      lines.push(`  ${name}: ${value};`);
    });
    lines.push('}');
    return lines.join('\n');
  }

  _flatten(obj, prefix) {
    const result = [];
    for (const [key, value] of Object.entries(obj)) {
      const path = prefix ? `${prefix}.${key}` : key;
      if (value && typeof value === 'object' && value.value) {
        result.push([path, value.value]);
      } else if (typeof value === 'object' && !value.value) {
        result.push(...this._flatten(value, path));
      }
    }
    return result;
  }
}

// Usage
const transformer = new TokenTransformer();
transformer
  .load('tokens/global.json')
  .load('tokens/semantic.json')
  .load('tokens/components.json')
  .resolve();

fs.writeFileSync('scss/_tokens.scss', transformer.toScss());
fs.writeFileSync('css/_tokens.css', transformer.toCss());
```

## Best Practices

1. **Use a standard format** - Follow the DTCG (Design Token Community Group) specification for interoperability.
2. **Three-tier hierarchy** - Global (raw values), semantic (purpose aliases), component (scoped) tokens.
3. **Reference, don't duplicate** - Semantic tokens should reference global tokens, not copy values.
4. **Transform, don't hand-write** - Generated token files should never be edited manually.
5. **Version tokens with the design system** - Token changes trigger minor or major version bumps.
6. **Provide dark mode tokens** - Define a separate token set for dark mode using CSS custom properties.
7. **Use meaningful names** - `color.action.primary` is better than `color.blue.500` for semantic tokens.
8. **Include metadata** - Track token description, category, and platform availability in the source format.
9. **Lint token files** - Validate naming conventions, required fields, and value formats in CI.
10. **Document the token tree** - Provide a visual reference showing the full token hierarchy.

## Common Pitfalls

1. **Flat token structure** - No hierarchy makes tokens hard to navigate and maintain.
2. **Hardcoded values in components** - Using `#0d6efd` instead of `$primary` breaks theming.
3. **Missing fallbacks** - CSS custom properties without fallback values break in older browsers.
4. **Circular references** - Token A references token B which references token A creates infinite loops.
5. **No platform-specific transforms** - iOS needs ARGB hex, Android needs XML; raw hex doesn't work everywhere.

## Accessibility Considerations

Color tokens must include contrast ratio metadata. Token files should specify which colors are safe to pair for text/background combinations meeting WCAG AA requirements.

```json
{
  "color": {
    "text": {
      "on-primary": {
        "value": "#ffffff",
        "type": "color",
        "meta": {
          "contrast-ratio-on": "{color.brand.primary}",
          "wcag-level": "AA"
        }
      }
    }
  }
}
```

## Responsive Behavior

Spacing and typography tokens should scale across breakpoints. Define responsive token variants that adjust values for mobile and desktop viewports.

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
