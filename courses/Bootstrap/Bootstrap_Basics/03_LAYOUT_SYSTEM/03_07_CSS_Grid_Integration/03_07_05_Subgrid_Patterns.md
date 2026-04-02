---
title: "Subgrid Patterns"
description: "Use CSS subgrid to align nested content across grid boundaries in Bootstrap-based layouts"
difficulty: 3
tags: [css-grid, subgrid, nested-grid, alignment, advanced]
prerequisites:
  - "CSS Grid area placement"
  - "CSS Grid basic layouts"
  - "Bootstrap card components"
---

## Overview

CSS subgrid allows nested grid items to inherit track definitions from their parent grid. This solves the long-standing problem of aligning nested content across sibling grid items - such as card titles, body content, and footers in a card grid. Subgrid is supported in all modern browsers (Chrome 117+, Firefox 71+, Safari 16+). When combined with Bootstrap components, subgrid enables perfectly aligned card layouts without JavaScript height calculations or flex hacks.

## Basic Implementation

### Subgrid Column Alignment

Use `grid-template-columns: subgrid` to let child items inherit parent column tracks.

```html
<div class="parent-grid">
  <div class="card-grid-item">
    <div class="card h-100">
      <img src="https://via.placeholder.com/300x150" class="card-img-top" alt="">
      <div class="card-body">
        <h5 class="card-title">Short Title</h5>
        <p class="card-text">Brief description.</p>
      </div>
      <div class="card-footer">
        <a href="#" class="btn btn-primary">Read More</a>
      </div>
    </div>
  </div>
  <div class="card-grid-item">
    <div class="card h-100">
      <img src="https://via.placeholder.com/300x150" class="card-img-top" alt="">
      <div class="card-body">
        <h5 class="card-title">A Much Longer Title That Spans Multiple Lines</h5>
        <p class="card-text">Longer description text that may push subsequent content down.</p>
      </div>
      <div class="card-footer">
        <a href="#" class="btn btn-primary">Read More</a>
      </div>
    </div>
  </div>
</div>

<style>
  .parent-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 1.5rem;
  }
  .card-grid-item {
    display: grid;
    grid-template-rows: subgrid;
    grid-row: span 3;
  }
</style>
```

### Subgrid Row Alignment

Align card sections (image, body, footer) across all cards in a row.

```html
<div class="aligned-cards">
  <article class="card-wrapper">
    <img src="https://via.placeholder.com/300x180" class="card-img-top" alt="">
    <h5 class="p-3 mb-0">Product Alpha</h5>
    <p class="px-3 pb-3">Short description of the product.</p>
    <div class="mt-auto p-3 border-top">
      <button class="btn btn-success w-100">Buy Now</button>
    </div>
  </article>
  <article class="card-wrapper">
    <img src="https://via.placeholder.com/300x180" class="card-img-top" alt="">
    <h5 class="p-3 mb-0">Product Beta Extended Edition</h5>
    <p class="px-3 pb-3">A much longer product description that spans several lines and tests alignment behavior.</p>
    <div class="mt-auto p-3 border-top">
      <button class="btn btn-success w-100">Buy Now</button>
    </div>
  </article>
  <article class="card-wrapper">
    <img src="https://via.placeholder.com/300x180" class="card-img-top" alt="">
    <h5 class="p-3 mb-0">Product Gamma</h5>
    <p class="px-3 pb-3">Medium length description.</p>
    <div class="mt-auto p-3 border-top">
      <button class="btn btn-success w-100">Buy Now</button>
    </div>
  </article>
</div>

<style>
  .aligned-cards {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    grid-auto-rows: auto;
    gap: 1.5rem;
  }
  .card-wrapper {
    display: grid;
    grid-template-rows: subgrid;
    grid-row: span 4;
    border: 1px solid var(--bs-border-color);
    border-radius: var(--bs-border-radius);
    overflow: hidden;
  }
</style>
```

## Advanced Variations

### Data Table with Subgrid Alignment

Align table cells across rows using subgrid for consistent column widths.

```html
<div class="data-grid">
  <div class="row-item header">
    <div>Name</div>
    <div>Status</div>
    <div>Role</div>
    <div>Actions</div>
  </div>
  <div class="row-item">
    <div>Alice Johnson</div>
    <div><span class="badge bg-success">Active</span></div>
    <div>Administrator with full access permissions</div>
    <div><button class="btn btn-sm btn-outline-primary">Edit</button></div>
  </div>
  <div class="row-item">
    <div>Bob Smith</div>
    <div><span class="badge bg-warning text-dark">Pending</span></div>
    <div>Editor</div>
    <div><button class="btn btn-sm btn-outline-primary">Edit</button></div>
  </div>
</div>

<style>
  .data-grid {
    display: grid;
    grid-template-columns: 150px 100px 1fr 100px;
    gap: 0;
    border: 1px solid var(--bs-border-color);
    border-radius: var(--bs-border-radius);
    overflow: hidden;
  }
  .row-item {
    display: grid;
    grid-template-columns: subgrid;
    grid-column: 1 / -1;
    border-bottom: 1px solid var(--bs-border-color);
  }
  .row-item > div {
    padding: 0.75rem 1rem;
    border-right: 1px solid var(--bs-border-color);
  }
  .row-item > div:last-child { border-right: none; }
  .row-item.header {
    background: var(--bs-tertiary-bg);
    font-weight: 600;
  }
</style>
```

### Form Grid with Subgrid Labels

```html
<div class="form-grid">
  <label class="form-label-cell" for="subName">Full Name</label>
  <input type="text" class="form-control" id="subName" placeholder="John Doe">

  <label class="form-label-cell" for="subEmail">Email Address</label>
  <input type="email" class="form-control" id="subEmail" placeholder="john@example.com">

  <label class="form-label-cell" for="subRole">Role Description</label>
  <select class="form-select" id="subRole">
    <option>Admin</option>
    <option>Editor</option>
  </select>
</div>

<style>
  .form-grid {
    display: grid;
    grid-template-columns: 150px 1fr;
    gap: 0.75rem;
    align-items: center;
  }
</style>
```

## Best Practices

1. **Use `grid-row: span N`** to tell subgrid how many parent rows to inherit.
2. **Apply subgrid to the row axis** for card layouts where content height varies.
3. **Ensure parent grid defines explicit rows** so subgrid has tracks to inherit.
4. **Use subgrid for card layouts** where title, body, and footer must align across cards.
5. **Combine with Bootstrap utilities** for padding, colors, and typography within subgrid items.
6. **Test subgrid in all target browsers** - it requires Chrome 117+, Firefox 71+, Safari 16+.
7. **Provide fallbacks** using `@supports not (grid-template-rows: subgrid)` for older browsers.
8. **Keep subgrid nesting shallow** - deep nesting reduces readability and performance.
9. **Use semantic HTML** inside subgrid items (article, section) for accessibility.
10. **Apply `overflow: hidden`** on subgrid items that may contain images or long text.
11. **Use CSS custom properties** for consistent gap values between parent and subgrid.
12. **Document subgrid dependencies** in component documentation for team awareness.

## Common Pitfalls

1. **Forgetting `grid-row: span N`** causes subgrid items to only span one parent row.
2. **Parent grid missing explicit row definitions** leaves subgrid with no tracks to inherit.
3. **Using subgrid for columns** when rows are the alignment problem - subgrid is most useful for rows.
4. **Browser support gaps** - subgrid doesn't work in Chrome < 117 or older Safari versions.
5. **Overriding subgrid with explicit heights** defeats the purpose of dynamic alignment.
6. **Mixing subgrid with `height: 100%`** on child elements causes unexpected sizing.
7. **Not updating `grid-row: span`** when adding or removing card sections.
8. **Using subgrid for simple layouts** where flexbox or regular grid would suffice.

## Accessibility Considerations

- Subgrid is purely visual - DOM order still determines reading and tab order.
- Use semantic elements (`<article>`, `<section>`) for subgrid card wrappers.
- Ensure aligned content maintains logical reading flow from top to bottom.
- Screen readers do not perceive subgrid structure - provide proper heading hierarchy.
- Test with keyboard navigation to verify focus order matches visual layout.

## Responsive Behavior

- Subgrid inherits parent responsive changes automatically when parent grid adjusts.
- Override `grid-template-columns` on subgrid items at breakpoints to disable subgrid on mobile.
- Use `grid-column: 1 / -1` to make subgrid items span full width on small screens.
- Consider disabling subgrid on mobile where single-column layout makes alignment unnecessary.
- Test responsive subgrid behavior with `auto-fit` parent grids as track count changes.
