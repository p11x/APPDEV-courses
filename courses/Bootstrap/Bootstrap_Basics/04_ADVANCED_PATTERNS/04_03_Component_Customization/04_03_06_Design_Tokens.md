---
title: "Design Tokens"
module: "Component Customization"
difficulty: 3
duration: "35 minutes"
prerequisites: ["CSS custom properties", "Design systems", "SCSS"]
tags: ["tokens", "design-system", "css-variables", "figma"]
---

# Design Tokens

## Overview

Design tokens are platform-agnostic variables that store visual design decisions—colors, spacing, typography, and more. Using CSS custom properties alongside Bootstrap creates a scalable design token system that bridges design tools and code, ensuring consistency across your entire application.

## Basic Implementation

Establish a CSS custom property token system:

```scss
:root {
  // Color tokens
  --color-primary: #6366f1;
  --color-primary-light: #818cf8;
  --color-primary-dark: #4f46e5;
  --color-secondary: #64748b;
  --color-success: #16a34a;
  --color-danger: #dc2626;

  // Spacing tokens
  --space-xs: 0.25rem;
  --space-sm: 0.5rem;
  --space-md: 1rem;
  --space-lg: 1.5rem;
  --space-xl: 2rem;
  --space-2xl: 3rem;

  // Typography tokens
  --font-family-sans: 'Inter', system-ui, sans-serif;
  --font-family-mono: 'JetBrains Mono', monospace;
  --font-size-sm: 0.875rem;
  --font-size-base: 1rem;
  --font-size-lg: 1.125rem;
  --font-weight-normal: 400;
  --font-weight-medium: 500;
  --font-weight-bold: 700;

  // Border tokens
  --border-radius-sm: 0.25rem;
  --border-radius-md: 0.375rem;
  --border-radius-lg: 0.5rem;
  --border-width: 1px;
}
```

Reference tokens in components:

```html
<div class="token-card" style="
  --card-bg: var(--color-primary);
  --card-padding: var(--space-lg);
">
  <h3 style="font-weight: var(--font-weight-bold);">Token Card</h3>
  <p style="font-size: var(--font-size-sm);">Built with design tokens.</p>
</div>
```

```css
.token-card {
  background: var(--card-bg, var(--color-primary));
  padding: var(--card-padding, var(--space-md));
  border-radius: var(--border-radius-md);
  color: white;
}
```

## Advanced Variations

Create a token hierarchy with semantic layers:

```scss
:root {
  // Primitive tokens (raw values)
  --gray-50: #f9fafb;
  --gray-100: #f3f4f6;
  --gray-200: #e5e7eb;
  --gray-800: #1f2937;
  --gray-900: #111827;

  // Semantic tokens (purpose-driven)
  --surface-primary: var(--gray-50);
  --surface-secondary: #ffffff;
  --surface-elevated: #ffffff;
  --text-primary: var(--gray-900);
  --text-secondary: var(--gray-800);
  --text-muted: var(--gray-200);
  --border-default: var(--gray-200);

  // Component tokens (component-specific)
  --card-bg: var(--surface-elevated);
  --card-border: var(--border-default);
  --card-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

// Dark theme overrides semantic tokens
[data-theme="dark"] {
  --surface-primary: var(--gray-900);
  --surface-secondary: var(--gray-800);
  --surface-elevated: #1e293b;
  --text-primary: #f9fafb;
  --text-secondary: #e5e7eb;
  --text-muted: #6b7280;
  --border-default: #374151;
}
```

Map tokens to Bootstrap variables:

```scss
// Sync design tokens with Bootstrap's variable system
$primary: var(--color-primary);
$secondary: var(--color-secondary);
$body-bg: var(--surface-primary);
$body-color: var(--text-primary);
$font-family-base: var(--font-family-sans);

@import "bootstrap/scss/bootstrap";
```

Generate token documentation from SCSS:

```scss
// Token metadata for documentation generation
$token-meta: (
  "color-primary": (
    value: #6366f1,
    category: "color",
    description: "Primary brand color",
    figma: "Brand/Primary"
  ),
  "space-md": (
    value: 1rem,
    category: "spacing",
    description: "Medium spacing unit",
    figma: "Spacing/16"
  )
);
```

## Best Practices

1. Use a three-tier token hierarchy (primitive → semantic → component)
2. Name tokens semantically, not visually (e.g., `--color-danger` not `--color-red`)
3. Sync tokens with Figma variables for design-code consistency
4. Document all tokens with descriptions and usage guidelines
5. Use CSS custom properties for runtime theming capabilities
6. Provide fallback values for every token reference
7. Keep primitive tokens in a dedicated file
8. Version token changes alongside design system updates
9. Use consistent naming conventions across all tokens
10. Validate token accessibility (contrast ratios)
11. Create token preview components for documentation
12. Automate token generation from design tool exports

## Common Pitfalls

1. Mixing primitive and semantic token naming
2. Not providing fallback values for CSS custom properties
3. Creating too many granular tokens (over-engineering)
4. Breaking token references when renaming tokens
5. Not syncing tokens between design and code
6. Ignoring browser support limitations for CSS custom properties
7. Hardcoding values instead of using tokens
8. Not documenting token usage guidelines

## Accessibility Considerations

- Ensure color tokens meet WCAG contrast requirements
- Test token-based themes with assistive technologies
- Document accessible token combinations
- Maintain focus indicator visibility across themes
- Provide high contrast token alternatives

## Responsive Behavior

- Create responsive token variants for different viewports
- Test token scaling across screen sizes
- Ensure spacing tokens adapt to mobile layouts
- Validate typography tokens remain legible at all sizes
