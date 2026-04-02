---
title: "CSS Container Queries Advanced"
description: "Named containers, style queries, and container query units for component-level responsive design with Bootstrap 5"
difficulty: 3
tags: [container-queries, responsive, named-containers, style-queries, advanced]
prerequisites:
  - "CSS media queries"
  - "Bootstrap 5 responsive utilities"
  - "CSS custom properties"
---

## Overview

Container queries enable components to respond to their container's size rather than the viewport, solving the component portability problem. CSS container queries are fully supported in modern browsers (Chrome 105+, Firefox 110+, Safari 16+). Named containers allow multiple independent query targets. Style queries enable conditional styling based on CSS custom property values. Container query units (`cqw`, `cqh`) provide fluid sizing relative to the container.

## Basic Implementation

### Basic Container Query

```html
<style>
  .card-container {
    container-type: inline-size;
    container-name: card;
  }

  /* Default: stack layout */
  .responsive-card .card-body {
    display: flex;
    flex-direction: column;
  }

  /* When container is wider than 400px: horizontal layout */
  @container card (min-width: 400px) {
    .responsive-card .card-body {
      flex-direction: row;
      gap: 1rem;
    }
    .responsive-card img {
      max-width: 150px;
    }
  }

  /* When container is wider than 600px: show sidebar */
  @container card (min-width: 600px) {
    .responsive-card .card-body {
      gap: 2rem;
    }
    .responsive-card img {
      max-width: 200px;
    }
  }
</style>

<div class="card-container">
  <div class="card responsive-card">
    <div class="card-body">
      <img src="https://via.placeholder.com/200x150" class="img-fluid rounded" alt="">
      <div>
        <h5 class="card-title">Container-Responsive Card</h5>
        <p class="card-text">This card adapts to its container width, not the viewport.</p>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Named Containers

```html
<style>
  .sidebar {
    container-type: inline-size;
    container-name: sidebar;
  }
  .main-content {
    container-type: inline-size;
    container-name: main;
  }

  /* Navigation adapts to sidebar width */
  @container sidebar (max-width: 200px) {
    .nav-label { display: none; }
    .nav-icon { font-size: 1.25rem; }
  }

  /* Content adapts to main area width */
  @container main (min-width: 600px) {
    .grid-2col { grid-template-columns: 1fr 1fr; }
  }
</style>

<div class="d-flex">
  <div class="sidebar bg-light p-3" style="width: 250px;">
    <ul class="nav flex-column">
      <li class="nav-item">
        <a class="nav-link" href="#">
          <i class="bi bi-house nav-icon me-2"></i>
          <span class="nav-label">Dashboard</span>
        </a>
      </li>
    </ul>
  </div>
  <div class="main-content flex-grow-1 p-4">
    <div class="grid-2col" style="display: grid; gap: 1rem;">
      <div class="card"><div class="card-body">Card 1</div></div>
      <div class="card"><div class="card-body">Card 2</div></div>
    </div>
  </div>
</div>
```

### Container Query Units

Use `cqw` (container query width) and `cqh` (container query height) for fluid sizing.

```html
<style>
  .hero-container {
    container-type: inline-size;
    container-name: hero;
  }

  .hero-title {
    font-size: clamp(1.5rem, 5cqw, 3rem);
  }

  .hero-subtitle {
    font-size: clamp(0.875rem, 2cqw, 1.25rem);
  }

  @container hero (min-width: 500px) {
    .hero-section {
      padding: 3rem;
      text-align: center;
    }
  }
</style>

<div class="hero-container">
  <div class="hero-section bg-primary text-white rounded-3 p-4">
    <h1 class="hero-title">Welcome to Our Platform</h1>
    <p class="hero-subtitle">Build responsive components that work anywhere.</p>
    <button class="btn btn-light">Get Started</button>
  </div>
</div>
```

### Style Queries

Style queries check custom property values on the container.

```html
<style>
  .themed-container {
    container-type: style;
    container-name: theme;
    --theme-variant: compact;
  }

  .themed-container.spacious {
    --theme-variant: spacious;
  }

  @container theme style(--theme-variant: compact) {
    .card { margin-bottom: 0.5rem; }
    .card-body { padding: 0.75rem; }
    .card-title { font-size: 0.9rem; }
  }

  @container theme style(--theme-variant: spacious) {
    .card { margin-bottom: 1.5rem; }
    .card-body { padding: 1.5rem; }
    .card-title { font-size: 1.25rem; }
  }
</style>

<div class="themed-container">
  <div class="card"><div class="card-body"><h5 class="card-title">Compact Card</h5></div></div>
</div>

<div class="themed-container spacious mt-4">
  <div class="card"><div class="card-body"><h5 class="card-title">Spacious Card</h5></div></div>
</div>
```

### Responsive Card Grid with Container Queries

```html
<style>
  .card-grid-container {
    container-type: inline-size;
  }

  .auto-grid {
    display: grid;
    gap: 1rem;
    grid-template-columns: 1fr;
  }

  @container (min-width: 400px) {
    .auto-grid { grid-template-columns: repeat(2, 1fr); }
  }

  @container (min-width: 700px) {
    .auto-grid { grid-template-columns: repeat(3, 1fr); }
  }

  @container (min-width: 1000px) {
    .auto-grid { grid-template-columns: repeat(4, 1fr); }
  }
</style>

<div class="card-grid-container">
  <div class="auto-grid">
    <div class="card"><div class="card-body">1</div></div>
    <div class="card"><div class="card-body">2</div></div>
    <div class="card"><div class="card-body">3</div></div>
    <div class="card"><div class="card-body">4</div></div>
  </div>
</div>
```

## Best Practices

1. **Use `container-type: inline-size`** for horizontal responsiveness (most common use case).
2. **Name containers** with `container-name` for explicit, readable query targets.
3. **Use container query units** (`cqw`, `cqh`) for fluid sizing within containers.
4. **Combine with Bootstrap grid** - container queries handle component internals, Bootstrap handles page layout.
5. **Use `container` shorthand** for `container-type` and `container-name` together.
6. **Apply container queries to reusable components** that may appear in different layout contexts.
7. **Use style queries** for theme variations based on custom properties.
8. **Test with multiple container sizes** to verify component behavior at all breakpoints.
9. **Use `@container (min-width: Npx)`** for mobile-first container sizing.
10. **Combine container queries** with media queries - containers for components, media for page layout.
11. **Set `container-type` on direct parents** of the components being queried.
12. **Use `clamp()` with container query units** for fluid typography within containers.

## Common Pitfalls

1. **Forgetting `container-type`** - the element won't be queryable without it.
2. **Using container queries for page layout** - use media queries for viewport-level changes.
3. **Container queries on the wrong element** - query the container, not the component itself.
4. **Style queries require `container-type: style`** - different from size queries.
5. **Browser support** - older browsers don't support container queries (need fallbacks).
6. **Mixing `container-name` and unnamed containers** in the same stylesheet.
7. **Not considering container nesting** - inner containers may have different dimensions.
8. **Forgetting `inline-size`** defaults to both inline and block - usually only need inline.

## Accessibility Considerations

- Container queries don't affect accessibility - they're purely responsive layout tools.
- Ensure container-responsive font sizes remain readable at all container widths.
- Maintain sufficient contrast when container queries change background colors.
- Focus management should work regardless of container size-based layout changes.
- Test with zoom to ensure container queries work when user zooms to 200%.

## Responsive Behavior

- Container queries respond to container width, not viewport width.
- Components become truly portable - they adapt to any layout context.
- Combine with Bootstrap's responsive grid for page-level responsiveness.
- Container query units provide fluid sizing relative to the container.
- Nest containers carefully - each container queries independently.
