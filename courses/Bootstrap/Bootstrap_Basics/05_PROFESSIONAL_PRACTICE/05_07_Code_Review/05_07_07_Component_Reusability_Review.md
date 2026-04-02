---
title: "Component Reusability Review"
module: "Code Review"
difficulty: 2
estimated_time: 20
tags: ["DRY", "components", "patterns", "reusability"]
prerequisites: ["Bootstrap components", "HTML templating"]
---

## Overview

Component reusability review identifies duplicated markup patterns and opportunities for extraction into reusable components. Applying the DRY (Don't Repeat Yourself) principle to Bootstrap components reduces code duplication, simplifies maintenance, and ensures visual consistency. This guide teaches how to spot reusable patterns, extract them into components, and document their interfaces for team-wide adoption.

## Basic Implementation

**Identifying Duplicate Patterns**

Look for repeated markup structures across templates with only data differences.

```html
<!-- Pattern found in 5+ files: card with icon, title, description -->
<div class="card h-100">
  <div class="card-body text-center">
    <div class="fs-1 text-primary mb-3"><i class="bi bi-shield-check"></i></div>
    <h5 class="card-title">Security</h5>
    <p class="card-text text-muted">Enterprise protection for your data.</p>
  </div>
</div>
```

**Extracting a Reusable Component**

Convert the identified pattern into a parameterized component template.

```html
<!-- Reusable feature-card component (Handlebars/ERB/Vue/etc.) -->
<!-- feature-card.hbs -->
<div class="card h-100">
  <div class="card-body text-center">
    <div class="fs-1 text-{{color}} mb-3"><i class="bi bi-{{icon}}"></i></div>
    <h5 class="card-title">{{title}}</h5>
    <p class="card-text text-muted">{{description}}</p>
  </div>
</div>

<!-- Usage -->
{{> feature-card icon="shield-check" color="primary" title="Security" description="Enterprise protection."}}
{{> feature-card icon="lightning" color="warning" title="Performance" description="Blazing fast response."}}
```

**Partial Extraction for Server-Side Templates**

For projects using server-side rendering, extract Bootstrap patterns into includes.

```erb
<!-- _alert.html.erb -->
<div class="alert alert-<%= type %> alert-dismissible fade show" role="alert">
  <%= message %>
  <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
</div>

<!-- Usage -->
<%= render 'alert', type: 'success', message: 'Record saved successfully.' %>
<%= render 'alert', type: 'danger', message: 'Validation failed.' %>
```

## Advanced Variations

**Component Composition Patterns**

Build complex components by composing simpler reusable pieces.

```html
<!-- Base modal component -->
<div class="modal fade" id="{{id}}" tabindex="-1" aria-labelledby="{{id}}Title">
  <div class="modal-dialog {{size}}">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="{{id}}Title">{{title}}</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">{{> @partial-block}}</div>
      <div class="modal-footer">
        {{#each actions}}
          <button class="btn {{class}}" {{attributes}}>{{label}}</button>
        {{/each}}
      </div>
    </div>
  </div>
</div>
```

**Slot-Based Components**

Use slot patterns to allow flexible content injection into reusable components.

```html
<!-- Vue/React-style slot component -->
<template>
  <div class="card">
    <div class="card-header" v-if="$slots.header">
      <slot name="header"></slot>
    </div>
    <div class="card-body">
      <slot></slot>
    </div>
    <div class="card-footer" v-if="$slots.footer">
      <slot name="footer"></slot>
    </div>
  </div>
</template>
```

**Data Table Component**

Extract the common data table pattern with sorting, pagination, and empty states.

```html
<!-- Reusable table component -->
<div class="table-responsive">
  <table class="table table-hover align-middle">
    <thead class="table-light">
      <tr>
        {{#each columns}}
          <th scope="col" class="sortable" data-sort="{{key}}">{{label}}</th>
        {{/each}}
      </tr>
    </thead>
    <tbody>
      {{#each rows}}
        <tr>{{> table-row .}}</tr>
      {{else}}
        <tr><td colspan="{{../columns.length}}" class="text-center text-muted py-4">{{emptyMessage}}</td></tr>
      {{/each}}
    </tbody>
  </table>
</div>
```

## Best Practices

1. **Identify patterns appearing 3+ times** - this threshold justifies extraction overhead
2. **Parameterize variable content** - icons, colors, labels, and data should be inputs
3. **Keep components focused** - one component should do one thing well
4. **Document component props/parameters** in a clear table format
5. **Use consistent naming** - `feature-card`, `data-table`, `alert-banner`
6. **Provide sensible defaults** - components should work without all parameters specified
7. **Version components** when breaking changes are introduced
8. **Create a component registry** - central catalog of all reusable components
9. **Write usage examples** for every extracted component
10. **Test components independently** before integrating into pages
11. **Avoid over-abstraction** - not every repeated pattern needs a component
12. **Consider CSS-only solutions** before reaching for JavaScript components

## Common Pitfalls

1. **Over-engineering simple patterns** - extracting a component for a one-off variation
2. **Creating tightly coupled components** - components that depend on specific parent structures
3. **Parameter explosion** - components with 10+ parameters become hard to use
4. **Ignoring variant differences** - forcing two similar patterns into one component with flags
5. **Breaking Bootstrap's own component API** - wrapping Bootstrap components in ways that break their JS
6. **Not documenting component interfaces** - team members cannot discover or reuse components
7. **Duplicating Bootstrap's built-in components** - rebuilding cards or modals from scratch
8. **Missing accessibility in extracted components** - ARIA attributes lost during extraction
9. **Inconsistent styling across instances** - parameterized colors applied inconsistently
10. **Extracting too early** - creating abstractions before understanding the full pattern space

## Accessibility Considerations

When extracting components, preserve all ARIA attributes and semantic structure. Create component templates that enforce accessibility by default - for example, modals should always require `aria-labelledby` as a parameter. Include accessibility documentation in component specs. Test extracted components with screen readers to ensure that parameterized content (dynamic labels, descriptions) is announced correctly.

## Responsive Behavior

Reusable components must be responsive by default. Include breakpoint-aware parameter options where needed (e.g., different layouts for mobile vs. desktop). Test extracted components at all breakpoints in isolation and within page contexts. Document responsive behavior as part of the component specification so consumers know how the component adapts to different viewport sizes.
