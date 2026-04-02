---
title: "Inline Code Comments for Bootstrap Projects"
module: "Documentation"
difficulty: 1
estimated_time: 15
tags: ["comments", "documentation", "SCSS", "JavaScript"]
prerequisites: ["Bootstrap customization", "SCSS basics"]
---

## Overview

Inline code comments explain the reasoning behind non-obvious decisions in Bootstrap SCSS and JavaScript code. Good comments answer "why" questions that the code itself cannot convey. Poor comments restate what the code does. This guide establishes standards for when to comment, what to document, and how to format comments in SCSS, JavaScript, and HTML templates within Bootstrap projects.

## Basic Implementation

**SCSS Comment Standards**

Use `//` for developer comments (stripped in compilation) and `/* */` for comments that must appear in output CSS.

```scss
// Override Bootstrap's default border-radius for brand consistency
// Design spec: https://figma.com/file/abc123
$border-radius: 0.5rem;

// Primary color updated from #3b82f6 to match new brand guidelines
// Approved by design team on 2024-02-15
$primary: #2563eb;

/* This comment will appear in compiled CSS */
// This comment will NOT appear in compiled CSS
```

**Variable Documentation**

Document the purpose and constraints of custom variables.

```scss
// Spacing scale multiplier - changing this affects ALL spacing utilities
// Valid range: 0.25rem - 2rem
// Current value matches design system's 4px base grid
$spacer: 1rem;

// Grid gutter width - used by .gx-* and .gy-* classes
// Note: Bootstrap 5.3+ uses CSS gap, this var is for fallback
$gutter-width: 1.5rem;
```

**JavaScript Comment Patterns**

Use JSDoc format for function documentation and inline comments for logic explanation.

```javascript
/**
 * Initializes Bootstrap tooltips on dynamically loaded content.
 * Must be called after AJAX content is inserted into the DOM.
 * @param {string} containerSelector - CSS selector for the parent container
 * @returns {bootstrap.Tooltip[]} Array of initialized tooltip instances
 */
function initDynamicTooltips(containerSelector) {
  const container = document.querySelector(containerSelector);
  if (!container) return [];

  // Bootstrap's Tooltip constructor auto-binds to data-bs-toggle="tooltip"
  // We manually initialize here because elements were added after page load
  const elements = container.querySelectorAll('[data-bs-toggle="tooltip"]');
  return Array.from(elements).map(el => new bootstrap.Tooltip(el));
}
```

## Advanced Variations

**SCSS Section Organization**

Use comment banners to organize large SCSS files into logical sections.

```scss
// ============================================
// BOOTSTRAP VARIABLE OVERRIDES
// ============================================
// These variables override Bootstrap defaults before import.
// Do NOT modify Bootstrap source files directly.

$primary: #2563eb;
$font-family-base: 'Inter', sans-serif;

// ============================================
// BOOTSTRAP IMPORT
// ============================================
@import "bootstrap/scss/bootstrap";

// ============================================
// CUSTOM COMPONENT STYLES
// ============================================
// Project-specific components that extend Bootstrap's library.
// Follow BEM naming with project prefix (app-).

.app-feature-card {
  // Use CSS custom property for theming support
  // Allows runtime theme switching without recompilation
  background: var(--app-card-bg, #{$light});
  border-radius: $border-radius;
}
```

**Complex Logic Comments**

Explain non-obvious calculations and conditionals.

```javascript
// Calculate modal backdrop opacity based on stacking depth
// Each nested modal adds 0.1 opacity, capped at 0.8
// This prevents backdrop from becoming completely opaque
function updateBackdropOpacity(modalCount) {
  const baseOpacity = 0.5;
  const increment = 0.1;
  const maxOpacity = 0.8;
  const opacity = Math.min(baseOpacity + (modalCount - 1) * increment, maxOpacity);

  document.querySelector('.modal-backdrop').style.opacity = opacity;
}
```

**HTML Template Comments**

Use comments sparingly in HTML, focusing on structural landmarks.

```html
<!-- Main navigation - collapses to offcanvas on mobile -->
<nav class="navbar navbar-expand-lg">
  ...
</nav>

<!-- Content grid: 3-col desktop, 2-col tablet, 1-col mobile -->
<div class="row g-4">
  <div class="col-12 col-md-6 col-lg-4">...</div>
  ...
</div>

<!-- End content grid -->
```

## Best Practices

1. **Comment the "why" not the "what"** - code shows what, comments explain why
2. **Document design decision sources** - link to Figma, Jira tickets, or spec docs
3. **Date significant changes** - `// Updated 2024-03-15 per design review`
4. **Use JSDoc for all public functions** - IDE support depends on JSDoc
5. **Mark TODO/FIXME with context** - include assignee and target date
6. **Remove commented-out code** - use version control for history
7. **Keep comments up to date** - stale comments are worse than no comments
8. **Use section banners** in files longer than 100 lines
9. **Document Bootstrap version** in variable override files
10. **Explain magic numbers** - `z-index: 1072` needs an explanation
11. **Avoid obvious comments** - `// increment counter` is noise
12. **Use consistent comment style** across the project

## Common Pitfalls

1. **Commenting every line** - excessive comments obscure the code
2. **Stale comments** - comments that contradict current code behavior
3. **Commented-out code blocks** - should use git history instead
4. **No JSDoc on public APIs** - IDE autocomplete cannot provide hints
5. **Missing "why" context** - "changed color to blue" without reason
6. **TODO without ownership** - anonymous TODOs never get resolved
7. **Inconsistent comment formats** - mixing `//`, `/* */`, and `#`
8. **HTML comments for logic** - putting business rules in HTML templates
9. **No version context** - comments that reference "recently changed"
10. **Over-documenting Bootstrap internals** - commenting well-known Bootstrap behavior

## Accessibility Considerations

Comment ARIA attribute decisions to explain why specific accessibility choices were made. Document screen reader testing results as inline comments. Note any accessibility trade-offs where visual design conflicts with best practices. Reference WCAG success criteria for non-obvious accessibility implementations.

## Responsive Behavior

Comment breakpoint decisions and why specific column distributions were chosen. Document any mobile-specific workarounds and their necessity. Explain responsive font sizing decisions that deviate from Bootstrap defaults. Note any cross-browser issues found at specific viewport widths.
