---
title: "Bootstrap Naming Conventions"
lesson: "01_01_18"
difficulty: "1"
topics: ["class-naming", "bem", "utilities", "conventions"]
estimated_time: "20 minutes"
---

# Bootstrap Naming Conventions

## Overview

Bootstrap follows a systematic class naming convention that balances readability with conciseness. While not strictly BEM (Block Element Modifier), Bootstrap's naming is BEM-inspired: component names serve as blocks, sub-components use double hyphens as element separators, and modifiers use contextual suffixes. Utility classes follow a `property-value` pattern with responsive and state variants. Understanding these patterns helps you predict class names, write consistent custom code, and extend Bootstrap effectively.

The naming system covers three tiers: component classes (`.card`, `.btn`), modifier classes (`.btn-primary`, `.card-body`), and utility classes (`.mt-3`, `.d-flex`). Each tier follows predictable rules.

## Basic Implementation

### Component Class Pattern

```html
<!-- Block: the component name -->
<div class="card">
  <!-- Element: block--element pattern -->
  <div class="card-body">
    <h5 class="card-title">Title</h5>
    <p class="card-text">Text</p>
    <!-- Modifier: block--modifier -->
    <a href="#" class="btn btn-primary">Action</a>
  </div>
</div>
```

### Utility Class Pattern

```html
<!-- property-value: property abbreviation + scale -->
<div class="mt-3 p-4 text-center bg-light fw-bold">
  Utility classes follow a consistent pattern
</div>
```

### Responsive Prefix Pattern

```html
<!-- breakpoint:utility -->
<div class="col-12 col-md-6 col-lg-4">
  Full width on mobile, half on medium, third on large
</div>
```

## Advanced Variations

### Naming Convention Reference

```
Components:     .component-name           (card, navbar, modal)
Sub-components: .component-element        (card-body, modal-header, navbar-brand)
Modifiers:      .component-modifier       (btn-primary, alert-danger)
Sizes:          .component-size           (btn-lg, btn-sm, modal-lg)
States:         .component-state          (active, disabled, show)
Utilities:      .property-value           (mt-3, d-flex, text-primary)
Responsive:     .breakpoint-property-value (md-mt-3, lg-d-flex)
State utils:    .state-property-value     (hover-bg-primary, focus-ring)
```

### Custom Classes Following Bootstrap Conventions

```scss
// Follow Bootstrap's naming for custom components
.announcement-bar {
  // Block
  padding: $spacer;
  background: $primary;

  &-content {
    // Element: announcement-bar-content
    color: white;
  }

  &-dismiss {
    // Element: announcement-bar-dismiss
    position: absolute;
    top: 0;
    right: 0;
  }

  &-dismissible {
    // Modifier: announcement-bar-dismissible
    padding-right: 3rem;
  }
}
```

### Utility Naming in Detail

```html
<!-- Spacing: m/p (margin/padding) + side (t/b/s/e/x/y) + size (0-5) -->
<div class="mt-0 mb-3 mx-auto px-2 py-4">Spacing</div>

<!-- Display: d + breakpoint + value -->
<div class="d-none d-md-block d-lg-flex">Display</div>

<!-- Text: text + property + value -->
<p class="text-primary text-center text-truncate">Text</p>

<!-- Background: bg + color -->
<div class="bg-success-subtle bg-opacity-50">Background</div>

<!-- Border: border + side + color + radius -->
<div class="border-top border-primary rounded-pill">Border</div>
```

## Best Practices

1. **Follow Bootstrap's kebab-case naming** - Use `my-component` not `myComponent` or `MyComponent`.
2. **Use BEM-style nesting in your SCSS** - `&-element` produces `.component-element` consistently.
3. **Match utility naming to Bootstrap's abbreviations** - `m` for margin, `p` for padding, `d` for display.
4. **Use responsive prefixes consistently** - Always mobile-first: `col-12 col-md-6`, not `col-md-6 col-12`.
5. **Avoid creating classes that conflict with Bootstrap** - Prefix custom classes with your project name.
6. **Use semantic names for custom components** - `.search-form` not `.blue-box-with-input`.
7. **Keep modifier names descriptive** - `.btn-outline-danger` clearly communicates purpose.
8. **Use data attributes for JS behavior, classes for styling** - Separation of concerns.
9. **Document your custom naming conventions** - Helps team members follow patterns.
10. **Group related utilities** - Write `<div class="d-flex align-items-center gap-2">` not scattered classes.

## Common Pitfalls

1. **Mixing BEM double-underscore syntax with Bootstrap's single-hyphen** - Bootstrap uses `.card-body` not `.card__body`.
2. **Using camelCase class names** - Inconsistent with Bootstrap's convention and breaks SCSS `&-` nesting.
3. **Creating overly specific class names** - `.primary-colored-large-rounded-button` instead of `.btn btn-primary btn-lg`.
4. **Using responsive prefixes in the wrong order** - Must be smallest to largest breakpoint.
5. **Duplicating Bootstrap utilities with custom CSS** - Write `.mt-3` in HTML, not `margin-top: 1rem` in custom CSS.

## Accessibility Considerations

Bootstrap's naming conventions do not directly map to ARIA semantics, but they support accessible patterns. Classes like `.visually-hidden` (not `.sr-only` in v5) follow an accurate naming convention that reflects their behavior across assistive technologies. When creating custom classes, avoid names that imply visual-only behavior for elements that affect screen reader output. Use `.visually-hidden-focusable` over `.hidden-but-accessible`.

## Responsive Behavior

Bootstrap's responsive naming convention uses breakpoint prefixes: `.col-md-6`, `.d-lg-flex`, `.text-sm-start`. The prefix applies to the class and all larger breakpoints unless overridden. This mobile-first convention means `.d-none` hides at all sizes, while `.d-md-none` hides only from the `md` breakpoint up. Custom responsive utilities should follow the same `.breakpoint-property-value` pattern.
