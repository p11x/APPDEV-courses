---
title: "Style Guide Creation for Bootstrap Projects"
module: "Documentation"
difficulty: 2
estimated_time: 25
tokens: ["style-guide", "design-tokens", "pattern-library"]
prerequisites: ["Bootstrap customization", "SCSS fundamentals"]
---

## Overview

A style guide documents the visual language of a Bootstrap project, including colors, typography, spacing, and reusable patterns. It serves as the authoritative reference for designers and developers, ensuring visual consistency across all pages and features. A well-maintained style guide reduces design drift, accelerates development, and provides a single source of truth for the project's design system.

## Basic Implementation

**Color Token Documentation**

Document all theme colors with their Bootstrap variable mappings and usage contexts.

```markdown
## Color Palette

### Primary Colors
| Name | Variable | Value | Usage |
|------|----------|-------|-------|
| Primary | `$primary` | `#2563eb` | CTAs, active states, links |
| Secondary | `$secondary` | `#64748b` | Subdued elements, borders |
| Success | `$success` | `#16a34a` | Positive feedback, confirmations |
| Danger | `$danger` | `#dc2626` | Errors, destructive actions |
| Warning | `$warning` | `#f59e0b` | Caution, pending states |
| Info | `$info` | `#0ea5e9` | Informational notices |

### Neutral Colors
| Name | Variable | Value |
|------|----------|-------|
| Dark | `$dark` | `#1e293b` |
| Light | `$light` | `#f8fafc` |
| Body | `$body-bg` | `#ffffff` |
```

**Typography Scale**

Document the type scale with Bootstrap variable mappings.

```markdown
## Typography

### Font Family
- **Base:** `$font-family-base` - `'Inter', system-ui, sans-serif`
- **Monospace:** `$font-family-monospace` - `'Fira Code', monospace`

### Scale
| Size | Variable | Value | Line Height | Use |
|------|----------|-------|-------------|-----|
| xs | `$font-size-sm` | 0.875rem | 1.25 | Captions, labels |
| sm | `$font-size-base` | 1rem | 1.5 | Body text |
| lg | `$font-size-lg` | 1.25rem | 1.5 | Subheadings |
| xl | `$h4-font-size` | 1.5rem | 1.3 | Section headings |
```

**Spacing System**

Document spacing tokens and their application.

```markdown
## Spacing Scale

Based on `$spacer: 1rem` with multiplier system.

| Class | Value | Use |
|-------|-------|-----|
| `p-1` / `m-1` | 0.25rem | Tight spacing, inline elements |
| `p-2` / `m-2` | 0.5rem | Compact components |
| `p-3` / `m-3` | 1rem | Default component spacing |
| `p-4` / `m-4` | 1.5rem | Section spacing |
| `p-5` / `m-5` | 3rem | Major section breaks |
```

## Advanced Variations

**Component Pattern Library**

Document reusable UI patterns with their HTML structure and variations.

```markdown
## Pattern: Content Card

### Base Structure
```html
<article class="card">
  <img class="card-img-top" src="" alt="">
  <div class="card-body">
    <h5 class="card-title"></h5>
    <p class="card-text"></p>
  </div>
  <div class="card-footer"></div>
</article>
```

### Variants
- **Horizontal:** Add `.flex-row` on card, `.col-*` on img and body
- **Overlay:** Use `.card-img-overlay` instead of `.card-body`
- **List group:** Replace `.card-body` with `.list-group-flush`
```

**SCSS Variable Overrides Table**

Document all customized Bootstrap variables and their project-specific values.

```scss
// _variables.scss - Documented overrides
$primary: #2563eb;           // Project brand blue
$border-radius: 0.5rem;     // Softer corners than Bootstrap default
$spacer: 1rem;              // Base spacing unit
$grid-breakpoints: (        // Unchanged from Bootstrap default
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1400px
);
```

## Best Practices

1. **Start with Bootstrap's defaults** and document only project-specific overrides
2. **Use design tokens** (CSS custom properties) for values that may change
3. **Include visual swatches** for colors - hex values alone are insufficient
4. **Show real usage examples** for every documented pattern
5. **Version the style guide** alongside the application
6. **Make it searchable** - organize with clear headings and a table of contents
7. **Include code snippets** that can be copied directly into projects
8. **Document the "why" not just the "what"** - explain design decisions
9. **Review and update quarterly** to prevent drift from actual implementation
10. **Link to live component demos** when possible
11. **Document responsive behavior** for each pattern
12. **Include accessibility requirements** as part of each pattern's spec

## Common Pitfalls

1. **Documenting without enforcement** - style guide exists but is not referenced
2. **Too much theory, not enough examples** - abstract descriptions without code
3. **Ignoring dark mode** - not documenting color adaptations for dark theme
4. **Outdated tokens** - documented values don't match compiled CSS
5. **Missing breakpoints** - only documenting desktop appearance
6. **No ownership** - no one is responsible for maintaining the guide
7. **Duplicate documentation** - same info in style guide and component docs
8. **Over-documenting obvious things** - restating Bootstrap's official documentation
9. **Ignoring version changes** - style guide for Bootstrap 4 when project uses Bootstrap 5
10. **No visual examples** - relying on text descriptions for visual concepts

## Accessibility Considerations

Document color contrast ratios for every color combination. Include minimum font sizes and line heights that meet WCAG requirements. Note focus indicator styles and their visibility requirements. Document how custom components handle keyboard navigation and screen reader announcements.

## Responsive Behavior

Document how the style guide patterns adapt across breakpoints. Include the grid configuration (number of columns, gutters, container widths) for each breakpoint. Show how typography scales on smaller screens. Note which patterns have mobile-specific variants and their differences from desktop.
