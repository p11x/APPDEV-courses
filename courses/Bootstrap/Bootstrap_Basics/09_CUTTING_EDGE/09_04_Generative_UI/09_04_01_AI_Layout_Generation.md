---
title: "AI Layout Generation"
description: "Prompting AI for Bootstrap layouts, layout description patterns, and AI-assisted grid generation"
difficulty: 2
tags: [ai, layout, prompting, grid-generation, copilot]
prerequisites:
  - 02_01_Grid_System
---

## Overview

AI layout generation uses natural language prompts to produce Bootstrap grid layouts. Instead of manually writing `.row` + `.col-*` structures, developers describe the layout ("a three-column dashboard with sidebar, main content, and widget panel"), and AI generates the corresponding Bootstrap HTML. The quality of output depends heavily on prompt specificity and the layout description patterns used.

Effective prompts include viewport behavior ("stacks on mobile"), content types ("cards with images"), spacing preferences ("gutter of 2rem"), and Bootstrap-specific terminology. A layout description DSL (domain-specific language) can standardize prompts for consistent AI output.

## Basic Implementation

```
# Prompt template for AI layout generation
Generate a Bootstrap 5 responsive layout:
- 3 columns on desktop (sidebar 25%, main 50%, aside 25%)
- 2 columns on tablet (main 66%, sidebar 33%, aside below)
- 1 column on mobile (stacked: main, sidebar, aside)
- Use .container-fluid
- Include sticky sidebar on desktop
- Add navbar at top with brand and 3 nav links
```

```html
<!-- AI-generated output -->
<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Brand</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#nav">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="nav">
      <ul class="navbar-nav">
        <li class="nav-item"><a class="nav-link active" href="#">Home</a></li>
        <li class="nav-item"><a class="nav-link" href="#">Features</a></li>
        <li class="nav-item"><a class="nav-link" href="#">Pricing</a></li>
      </ul>
    </div>
  </div>
</nav>

<div class="container-fluid">
  <div class="row">
    <aside class="col-lg-3 col-md-4 order-md-1 order-3 sticky-top" style="height: fit-content;">
      Sidebar
    </aside>
    <main class="col-lg-6 col-md-8 order-1">
      Main Content
    </main>
    <aside class="col-lg-3 order-md-3 order-2">
      Widgets
    </aside>
  </div>
</div>
```

```js
// Prompt refinement loop
const promptTemplates = {
  dashboard: `Create a Bootstrap 5 dashboard layout with:
    - Top navbar with brand and user dropdown
    - Left sidebar with navigation menu (collapses on mobile)
    - Main content area with {numWidgets} widget cards
    - Cards should be responsive grid (3 cols desktop, 2 tablet, 1 mobile)
    - Use Bootstrap utilities: shadow, rounded, padding`,

  landing: `Create a Bootstrap 5 landing page with:
    - Hero section with centered text and CTA button (full viewport height)
    - Features section: 3 columns with icons
    - Testimonials carousel
    - Footer with 4 columns of links`,

  form: `Create a Bootstrap 5 form layout with:
    - 2-column grid for name/email fields
    - Full-width textarea
    - Inline radio buttons
    - Submit button aligned right`
};

// Use template
const prompt = promptTemplates.dashboard.replace('{numWidgets}', '6');
```

## Advanced Variations

Structured prompt for complex layouts:

```json
{
  "layout": "dashboard",
  "responsive": {
    "mobile": "single-column-stack",
    "tablet": "two-column-sidebar-main",
    "desktop": "three-column-sidebar-main-aside"
  },
  "components": ["navbar", "sidebar", "card-grid", "footer"],
  "theme": "light",
  "bootstrap_version": "5.3"
}
```

## Best Practices

1. Include Bootstrap version in the prompt for version-accurate output.
2. Specify responsive behavior for each breakpoint explicitly.
3. Mention Bootstrap class names when you need specific utilities.
4. Use structured prompt templates for repeatable layouts.
5. Review AI output for semantic HTML correctness.
6. Validate generated Bootstrap classes against the actual Bootstrap version.
7. Provide context about content types (text, images, forms) in the prompt.
8. Ask for accessibility attributes in the prompt.
9. Iterate on prompts — refine based on output quality.
10. Use AI for initial scaffolding, then refine manually.
11. Test generated layouts at all Bootstrap breakpoints.
12. Document successful prompt patterns in a team prompt library.

## Common Pitfalls

1. **Outdated classes** — AI may generate Bootstrap 4 classes (`mr-3` instead of `me-3`).
2. **Non-semantic HTML** — AI may use `<div>` where `<section>`, `<nav>`, or `<main>` is appropriate.
3. **Missing responsive classes** — AI may forget mobile-first breakpoints.
4. **Incorrect nesting** — Bootstrap requires `.container > .row > .col` nesting; AI may skip levels.
5. **Hallucinated classes** — AI may invent Bootstrap classes that don't exist.
6. **Accessibility gaps** — AI output often lacks ARIA attributes, alt text, and focus management.

## Accessibility Considerations

Always add `aria-label`, `role`, and semantic landmarks to AI-generated layouts. Prompt AI to include accessibility attributes. Review output with accessibility auditing tools.

## Responsive Behavior

Specify responsive requirements in the prompt. AI can generate breakpoint-specific classes (`col-md-6 col-lg-4`) but may miss edge cases. Always test AI output at all Bootstrap breakpoints.
