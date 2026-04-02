---
title: "Component Catalog"
difficulty: 2
category: "Advanced Development"
subcategory: "Design Systems"
prerequisites:
  - Storybook or Docusaurus
  - Bootstrap 5 Components
  - Content Management for Docs
---

## Overview

A living component catalog serves as the definitive reference for all UI components in a design system. It provides interactive previews, API documentation, usage guidelines, and do/don't examples that show correct and incorrect patterns. The catalog stays in sync with the codebase because it generates documentation from component source code and metadata.

The catalog has four sections per component: overview (what it is and when to use it), API reference (props, events, slots), examples (interactive demos in all states), and guidelines (do/don't illustrations with rationale). This structure helps designers understand intent, developers understand implementation, and QA teams understand edge cases.

## Basic Implementation

```mdx
<!-- docs/components/card.mdx -->
---
title: Card
description: A flexible container for grouping related content and actions.
category: Layout
---

import { CardDemo, CardWithImage, CardVariants } from './demos/Card';

## Overview

Cards are versatile containers that group related content including headings, body text, images, and actions. Use cards to present discrete pieces of information in a scannable grid layout.

<CardDemo />

## When to Use

- Display a collection of similar items (products, articles, team members)
- Group related form fields or settings
- Present summary information with a link to details

## When Not to Use

- Displaying sequential content (use a list instead)
- Simple text content without actions (use a basic container)
- Full-page layouts (use grid directly)

## API

| Prop | Type | Default | Description |
|------|------|---------|-------------|
| `title` | `string` | `''` | Card heading text |
| `body` | `string` | `''` | Main content area |
| `image` | `{src, alt}` | `null` | Header image |
| `variant` | `'primary' \| 'success' \| 'danger'` | `''` | Color variant |
| `bordered` | `boolean` | `true` | Show card border |
| `actions` | `Action[]` | `[]` | Footer action buttons |

## Do's and Don'ts

### Do
- Use consistent card sizes within a grid
- Include meaningful alt text for card images
- Limit card actions to 2-3 buttons

### Don't
- Nest cards inside other cards
- Use cards for single-line content
- Overload cards with too many actions
```

```html
<!-- Interactive component demo -->
<div class="catalog-demo">
  <h4>Default Card</h4>
  <div class="card" style="width: 18rem;">
    <div class="card-body">
      <h5 class="card-title">Card Title</h5>
      <p class="card-text">Quick example text to build on the card title.</p>
      <a href="#" class="btn btn-primary">Action</a>
    </div>
  </div>
</div>

<div class="catalog-demo catalog-demo--dont">
  <h4>Don't: Nest Cards</h4>
  <div class="card">
    <div class="card-body">
      <div class="card">
        <div class="card-body">
          <p class="text-danger">Nested cards create visual confusion and depth ambiguity.</p>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. **Generate docs from code** - Use JSDoc, propType descriptions, or TypeScript to auto-generate API tables.
2. **Include interactive demos** - Static screenshots don't show hover states, focus rings, or animations.
3. **Show all component states** - Default, loading, error, empty, disabled, and focused states.
4. **Provide copy-paste examples** - Every example should be directly copyable into a project.
5. **Document do's and don'ts** - Visual examples of correct and incorrect usage with rationale.
6. **Version the catalog** - Documentation should match the library version being used.
7. **Search across components** - Enable searching by component name, prop, and description.
8. **Include accessibility notes** - Document keyboard shortcuts, ARIA behavior, and screen reader output.
9. **Show responsive behavior** - Include viewport toggles to preview mobile/tablet/desktop layouts.
10. **Link to source code** - Every documented component should link to its source file on GitHub.

## Common Pitfalls

1. **Outdated documentation** - Docs that don't reflect current code mislead developers.
2. **Missing usage context** - Documenting props without explaining when and why to use the component.
3. **No visual examples** - Text-only documentation is hard to understand for visual components.
4. **Incomplete state coverage** - Only showing default state misses edge case behavior.
5. **Missing version indicators** - Not marking which version introduced or deprecated features.

## Accessibility Considerations

Document keyboard interaction patterns, ARIA roles, and expected screen reader announcements for every component.

## Responsive Behavior

Show how components adapt across breakpoints with viewport toggle controls in the catalog.
