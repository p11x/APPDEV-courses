---
title: "Code Documentation"
category: "Maintenance Patterns"
difficulty: 2
estimated_time: "25 minutes"
prerequisites: ["Bootstrap 5 fundamentals", "Component customization"]
tags: ["bootstrap", "documentation", "storybook", "style-guide", "living-docs"]
---

# Code Documentation

## Overview

Well-documented Bootstrap components reduce onboarding time, prevent inconsistent implementations, and serve as a single source of truth for design decisions. Documentation patterns for Bootstrap projects include **component usage examples**, **property tables**, **variant showcases**, and **accessibility notes**. Tools like **Storybook** enable interactive component exploration, while style guide generators produce browsable documentation from your codebase. Living documentation stays synchronized with your actual components, eliminating stale or outdated references.

## Basic Implementation

**Documenting a custom Bootstrap component:**

```html
<!--
  Alert Component
  Usage: Display contextual feedback messages.

  Variants: .alert-primary, .alert-success, .alert-danger, .alert-warning
  Dismissible: Add .alert-dismissible and a .btn-close

  Example:
  <div class="alert alert-success alert-dismissible" role="alert">
    Operation completed successfully.
    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
  </div>
-->
```

**Using JSDoc for Bootstrap JavaScript interactions:**

```javascript
/**
 * Initializes a Bootstrap modal with custom configuration.
 * @param {string} selector - CSS selector for the modal element.
 * @param {object} options - Bootstrap modal options.
 * @returns {bootstrap.Modal} The modal instance.
 * @example
 * const modal = initModal('#confirmDialog', { backdrop: 'static' });
 * modal.show();
 */
function initModal(selector, options = {}) {
  const element = document.querySelector(selector);
  return new bootstrap.Modal(element, options);
}
```

**Markdown component documentation:**

```markdown
## Custom Card Component

**Class:** `.custom-card`
**Extends:** Bootstrap `.card`

| Prop | Type | Default | Description |
|------|------|---------|-------------|
| variant | string | default | Color theme (primary, success, danger) |
| elevated | boolean | false | Adds shadow on hover |
| bordered | boolean | true | Shows card border |
```

## Advanced Variations

**Storybook integration with Bootstrap:**

```javascript
// stories/Button.stories.js
export default {
  title: 'Components/Button',
  argTypes: {
    variant: {
      control: 'select',
      options: ['primary', 'secondary', 'success', 'danger', 'warning'],
    },
    size: {
      control: 'select',
      options: ['sm', 'md', 'lg'],
    },
    disabled: { control: 'boolean' },
  },
};

export const Primary = ({ variant, size, disabled }) => `
  <button class="btn btn-${variant} btn-${size}"
    ${disabled ? 'disabled' : ''}>
    Click Me
  </button>
`;
Primary.args = { variant: 'primary', size: 'md', disabled: false };
```

**Living style guide with CSS custom properties:**

```css
/* Document your design tokens */
:root {
  /* @name Primary Color
     @description Main brand color used for primary buttons and links
     @example .btn-primary, .text-primary */
  --bs-primary: #0d6efd;

  /* @name Border Radius
     @description Default border radius for rounded components
     @example .rounded, .card, .btn */
  --bs-border-radius: 0.375rem;
}
```

**Auto-generating component docs from source:**

```bash
# Using SassDoc for SCSS documentation
npx sassdoc src/scss --dest docs/sass

# Using JSDoc for JavaScript utilities
npx jsdoc src/js -d docs/js
```

## Best Practices

1. **Document every custom component** with usage examples, props, and variants.
2. **Use Storybook** to create an interactive component library.
3. **Include accessibility notes** — document ARIA attributes and keyboard interactions.
4. **Show both correct and incorrect usage** — antipatterns are as important as examples.
5. **Keep documentation in the same repository** as the code it describes.
6. **Generate docs automatically** from source comments using JSDoc or SassDoc.
7. **Version your documentation** alongside your Bootstrap version.
8. **Include responsive examples** — show components at multiple breakpoints.
9. **Document color and typography tokens** — reference CSS custom properties.
10. **Add visual regression tests** to documentation examples.
11. **Use code snippets that can be copy-pasted** directly into projects.
12. **Review documentation in pull requests** — treat docs as first-class code.

## Common Pitfalls

1. **Stale documentation** — examples that no longer match the actual component output.
2. **Missing accessibility documentation** — not recording keyboard and screen reader behavior.
3. **Over-documenting defaults** — restating what Bootstrap's official docs already cover.
4. **No interactive examples** — static screenshots don't demonstrate behavior.
5. **Documentation in separate repository** — leads to drift between code and docs.
6. **Ignoring mobile experience** — only documenting desktop layouts.

## Accessibility Considerations

Documentation should include **accessibility specifications** for every component. Record expected **ARIA roles**, **keyboard shortcuts**, and **screen reader announcements**. Storybook's accessibility addon (`@storybook/addon-a11y`) automatically checks components for WCAG violations during development. Include testing instructions for assistive technologies in your component docs.

## Responsive Behavior

Component documentation must showcase **responsive behavior** at multiple breakpoints. Use Storybook's viewport addon to display components at mobile (375px), tablet (768px), and desktop (1280px) widths. Document responsive class variants (e.g., `.col-md-6 .col-lg-4`) with grid examples. Include notes on how components adapt when space is constrained.
