---
title: "Design-Developer Handoff for Bootstrap Projects"
module: "Team Workflow"
difficulty: 2
estimated_time: 20
tags: ["design", "handoff", "figma", "workflow"]
prerequisites: ["Bootstrap grid system", "Figma basics"]
---

## Overview

The design-developer handoff is the process of translating design specifications into Bootstrap implementations. A smooth handoff reduces miscommunication, prevents pixel-pushing iterations, and ensures that the final product matches the intended design. This guide covers workflows for converting Figma designs into Bootstrap components, managing design tokens, and establishing shared language between designers and developers.

## Basic Implementation

**Figma to Bootstrap Mapping**

Establish a shared vocabulary between design files and Bootstrap classes.

```markdown
## Design-to-Code Mapping

| Figma Property | Bootstrap Equivalent |
|----------------|---------------------|
| Auto Layout (horizontal) | `d-flex flex-row` |
| Auto Layout (vertical) | `d-flex flex-column` |
| Gap | `gap-{1-5}` |
| Padding | `p-{1-5}`, `px-{1-5}`, `py-{1-5}` |
| Corner radius 4px | `rounded` |
| Corner radius 8px | `rounded-2` |
| Corner radius 16px | `rounded-4` |
| Drop shadow (small) | `shadow-sm` |
| Drop shadow (medium) | `shadow` |
| Fill (primary) | `bg-primary` |
| Text (primary) | `text-primary` |
```

**Design Spec Extraction**

Document the information developers need from design files.

```markdown
## Component Spec: Feature Card

### Dimensions
- Max width: 360px
- Padding: 24px (`p-4` in Bootstrap)
- Border radius: 8px (`rounded-2`)

### Typography
- Title: 1.25rem / 600 weight / `text-dark`
- Description: 0.875rem / 400 weight / `text-muted`

### Spacing
- Title to description: 8px (`mb-2`)
- Description to button: 16px (`mt-3`)
- Card to card (grid): 24px (`g-4`)

### States
- Default: `shadow-sm`
- Hover: `shadow` + slight translateY
- Focus: `border-primary` outline

### Responsive
- Mobile: Full width (`col-12`)
- Tablet: Half width (`col-md-6`)
- Desktop: Third width (`col-lg-4`)
```

**Design Token Sync**

Keep design tokens synchronized between Figma and SCSS variables.

```scss
// _design-tokens.scss - Synced with Figma design system
// Last sync: 2024-03-15

$primary: #2563eb;          // Figma: Brand/Primary/500
$secondary: #64748b;        // Figma: Brand/Secondary/500
$font-size-base: 1rem;      // Figma: Typography/Body/Regular
$spacer: 1rem;              // Figma: Spacing/4
$border-radius: 0.5rem;     // Figma: Radius/Medium
$card-border-radius: 0.5rem; // Figma: Components/Card/Radius
```

## Advanced Variations

**Redline Process**

Systematic comparison between design and implementation.

```markdown
## Redline Checklist

### Layout
- [ ] Column widths match grid spec
- [ ] Gutters match design (g-3 = 16px)
- [ ] Content alignment (left, center, right)
- [ ] Vertical rhythm spacing

### Typography
- [ ] Font family matches
- [ ] Font sizes match scale
- [ ] Line heights match
- [ ] Font weights match

### Colors
- [ ] Background colors match
- [ ] Text colors match
- [ ] Border colors match
- [ ] Hover/focus states match

### Spacing
- [ ] Internal padding matches
- [ ] External margins match
- [ ] Gap between elements matches

### Responsive
- [ ] Breakpoint behavior matches
- [ ] Content reflow matches
- [ ] Touch targets adequate on mobile
```

**Design Review Sync**

```markdown
## Weekly Design-Dev Sync Agenda

1. **New designs review** (15 min)
   - Designer presents upcoming designs
   - Developers identify Bootstrap component matches
   - Flag any components that need custom development

2. **Implementation review** (15 min)
   - Developers present completed components
   - Designer verifies against spec
   - Redline items discussed and assigned

3. **Token updates** (10 min)
   - Any design token changes
   - Sync status between Figma and SCSS

4. **Open questions** (10 min)
   - Unresolved design-dev discrepancies
   - Browser compatibility concerns
```

## Best Practices

1. **Use a shared design system** in Figma with Bootstrap-mapped components
2. **Export design tokens** as SCSS variables automatically when possible
3. **Include responsive specs** in every design handoff
4. **Document interaction states** (hover, focus, active, disabled)
5. **Use Bootstrap's spacing scale** in design tools for alignment
6. **Schedule regular design-dev syncs** to catch discrepancies early
7. **Maintain a design-to-code mapping document** as a shared reference
8. **Include accessibility annotations** in design files
9. **Version design files** alongside code releases
10. **Use Figma's dev mode** for auto-generated CSS properties
11. **Document edge cases** in design specs (long text, empty states)
12. **Provide assets in required formats** (SVG for icons, optimized PNG for images)

## Common Pitfalls

1. **Designing without Bootstrap constraints** - creating layouts that Bootstrap's grid cannot produce
2. **Missing responsive specs** - developers guess at mobile behavior
3. **No interaction states** - hover and focus states are unspecified
4. **Ignoring design tokens** - developers hard-code values instead of using variables
5. **Outdated design files** - Figma doesn't match deployed application
6. **No edge case designs** - empty states and error states are unspecified
7. **Pixel-perfect obsession** - spending hours on sub-pixel differences
8. **No shared vocabulary** - designers and developers use different terms for the same concepts
9. **Missing asset exports** - developers screenshot images from Figma
10. **Skipping the redline process** - differences are discovered by users in production

## Accessibility Considerations

Designers should annotate accessibility requirements in design files: focus order, ARIA labels, color contrast ratios, and text alternatives for images. Developers should verify that Bootstrap's built-in accessibility features meet the design requirements. Establish a shared understanding of WCAG requirements and how they affect design decisions.

## Responsive Behavior

Every design handoff should include specs at mobile (375px), tablet (768px), and desktop (1440px) viewports. Document how components restructure at each breakpoint - which elements hide, stack, or resize. Use Bootstrap's responsive utility classes as the shared language for responsive behavior specification.
