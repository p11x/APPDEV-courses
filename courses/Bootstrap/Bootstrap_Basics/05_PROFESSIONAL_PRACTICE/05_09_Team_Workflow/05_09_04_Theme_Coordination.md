---
title: "Multi-Team Theme Coordination"
module: "Team Workflow"
difficulty: 2
estimated_time: 20
tags: ["themes", "coordination", "tokens", "versioning"]
prerequisites: ["Bootstrap theming", "SCSS variables", "CSS custom properties"]
---

## Overview

When multiple teams share a Bootstrap-based design system, theme coordination ensures visual consistency across all products. This involves managing a central theme package, versioning theme changes, communicating updates, and providing migration paths for breaking changes. Without coordination, teams diverge into incompatible visual styles that fragment the user experience.

## Basic Implementation

**Central Theme Package**

Create a single source of truth for theme variables and components.

```scss
// packages/theme/_variables.scss
// Central theme variables - DO NOT override locally

// Brand colors
$primary: #2563eb;
$secondary: #64748b;
$accent: #7c3aed;

// Typography
$font-family-base: 'Inter', system-ui, sans-serif;
$headings-font-weight: 600;

// Spacing
$spacer: 1rem;
$container-max-widths: (
  sm: 540px,
  md: 720px,
  lg: 960px,
  xl: 1140px,
  xxl: 1320px
);

// Export as CSS custom properties for runtime theming
:root {
  --bs-primary: #{$primary};
  --bs-secondary: #{$secondary};
  --bs-font-family-base: #{$font-family-base};
}
```

**Theme Distribution**

```json
// packages/theme/package.json
{
  "name": "@company/bootstrap-theme",
  "version": "3.2.0",
  "main": "dist/theme.css",
  "sass": "_index.scss",
  "peerDependencies": {
    "bootstrap": "^5.3.0"
  }
}
```

```scss
// Consumer project imports
@import "@company/bootstrap-theme/scss/variables";
@import "bootstrap/scss/bootstrap";
@import "@company/bootstrap-theme/scss/components";
```

**Theme Version Communication**

```markdown
# Theme Update Notification

## @company/bootstrap-theme v3.2.0
**Released:** 2024-03-15
**Requires action:** Yes

### Changes
- Primary color changed from `#3b82f6` to `#2563eb`
- New `--bs-accent` CSS custom property added
- Card border-radius updated to match design system v2.1

### Migration
1. Update package: `npm update @company/bootstrap-theme`
2. No SCSS changes required
3. Visual regression test recommended

### Deadline
All teams must update by 2024-03-30
```

## Advanced Variations

**Multi-Brand Theme Support**

Support multiple brands from a single theme package using SCSS maps and mixins.

```scss
// packages/theme/_brands.scss
$brands: (
  "enterprise": (
    primary: #1e40af,
    font-family: 'Inter',
    border-radius: 0.25rem,
  ),
  "consumer": (
    primary: #7c3aed,
    font-family: 'Poppins',
    border-radius: 1rem,
  ),
  "partner": (
    primary: #059669,
    font-family: 'DM Sans',
    border-radius: 0.5rem,
  )
);

@mixin brand-theme($brand-name) {
  $brand: map-get($brands, $brand-name);
  --bs-primary: #{map-get($brand, primary)};
  --bs-font-family-base: #{map-get($brand, font-family)};
  --bs-border-radius: #{map-get($brand, border-radius)};
}
```

```scss
// Consumer selects brand theme
.app-enterprise {
  @include brand-theme("enterprise");
}

.app-consumer {
  @include brand-theme("consumer");
}
```

**Theme Change Request Process**

```markdown
## Theme Change Request Template

### Requestor
- Team:
- Contact:

### Change Description
What variable(s) need to change and why?

### Impact Assessment
- [ ] Affects all teams
- [ ] Affects specific teams: ___
- [ ] Requires visual regression testing
- [ ] Breaking change

### Proposed Timeline
- PR submitted: ___
- Review period: ___
- Target release: ___
- Migration deadline: ___
```

## Best Practices

1. **Maintain a single theme package** - never duplicate theme variables across repos
2. **Use CSS custom properties** for runtime theming capabilities
3. **Version theme packages semantically** - breaking changes require major version bump
4. **Announce theme changes** at least 2 weeks before required adoption
5. **Provide migration guides** for every breaking theme change
6. **Run visual regression tests** across all consuming applications
7. **Use design tokens** as the shared language between design and development
8. **Maintain a theme changelog** with clear before/after examples
9. **Test themes at all breakpoints** before releasing
10. **Include dark mode variants** in the central theme package
11. **Automate theme builds** and publishing through CI/CD
12. **Establish a theme review board** for cross-team change coordination

## Common Pitfalls

1. **Local theme overrides** - teams override central variables, creating inconsistency
2. **No versioning** - theme changes break consumers without warning
3. **Missing dark mode** - theme only supports light mode
4. **No communication process** - teams discover changes through broken builds
5. **Hard-coded values** - using hex codes instead of SCSS variables
6. **No visual regression testing** - subtle changes go unnoticed
7. **Monolithic theme file** - changes to one brand affect all brands
8. **Ignoring RTL support** - themes break in right-to-left languages
9. **No fallback values** - CSS custom properties without defaults
10. **Delayed updates** - teams skip versions and accumulate breaking changes

## Accessibility Considerations

Ensure all theme color combinations meet WCAG contrast requirements. Document contrast ratios for every color pairing in the theme. Include high-contrast theme variants for accessibility compliance. Test themes with forced-colors mode (Windows High Contrast).

## Responsive Behavior

Theme values may need to adapt at different breakpoints (e.g., smaller border-radius on mobile, adjusted spacing). Document responsive theme behavior. Test theme consistency across all breakpoints in all consuming applications.
