---
title: "Responsive Table Patterns"
lesson: "01_06_14"
difficulty: "1"
topics: ["table-responsive", "stacked-tables", "horizontal-scroll", "mobile-tables"]
estimated_time: "20 minutes"
---

# Responsive Table Patterns

## Overview

Tables present a unique responsive challenge because their columnar structure does not naturally stack on narrow screens. Bootstrap offers `.table-responsive` for horizontal scrolling on small viewports and `.table-responsive-{breakpoint}` for breakpoint-specific wrapping. For complete mobile reformatting, stacked table patterns transform rows into card-like vertical layouts. Choosing the right pattern depends on your data density, user context, and the importance of comparing columns side-by-side.

Bootstrap's table component provides consistent styling with `.table`, `.table-striped`, `.table-bordered`, and color variants that maintain readability across devices.

## Basic Implementation

### Basic Responsive Table

```html
<!-- Horizontal scroll on screens smaller than the table -->
<div class="table-responsive">
  <table class="table">
    <thead>
      <tr>
        <th>Name</th><th>Email</th><th>Role</th><th>Status</th><th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Jane Doe</td><td>jane@example.com</td><td>Admin</td>
        <td><span class="badge bg-success">Active</span></td>
        <td><button class="btn btn-sm btn-primary">Edit</button></td>
      </tr>
    </tbody>
  </table>
</div>
```

### Breakpoint-Specific Responsive

```html
<!-- Scrollable only below lg breakpoint -->
<div class="table-responsive-lg">
  <table class="table">
    <thead>
      <tr><th>ID</th><th>Name</th><th>Date</th><th>Amount</th><th>Status</th></tr>
    </thead>
    <tbody>
      <tr><td>1</td><td>Order A</td><td>2024-01-15</td><td>$120.00</td><td>Shipped</td></tr>
    </tbody>
  </table>
</div>
```

### Striped and Bordered Table

```html
<table class="table table-striped table-bordered table-hover">
  <thead class="table-dark">
    <tr><th>#</th><th>First</th><th>Last</th><th>Handle</th></tr>
  </thead>
  <tbody>
    <tr><td>1</td><td>Mark</td><td>Otto</td><td>@mdo</td></tr>
    <tr><td>2</td><td>Jacob</td><td>Thornton</td><td>@fat</td></tr>
  </tbody>
</table>
```

## Advanced Variations

### Stacked Table Pattern (Card-Style on Mobile)

```html
<!-- Desktop: normal table -->
<div class="d-none d-md-block">
  <table class="table">
    <thead><tr><th>Name</th><th>Email</th><th>Role</th></tr></thead>
    <tbody>
      <tr><td>Jane</td><td>jane@example.com</td><td>Admin</td></tr>
    </tbody>
  </table>
</div>

<!-- Mobile: stacked cards -->
<div class="d-md-none">
  <div class="card mb-3">
    <div class="card-body">
      <div class="row mb-2">
        <div class="col-4 fw-bold">Name</div>
        <div class="col-8">Jane</div>
      </div>
      <div class="row mb-2">
        <div class="col-4 fw-bold">Email</div>
        <div class="col-8">jane@example.com</div>
      </div>
      <div class="row">
        <div class="col-4 fw-bold">Role</div>
        <div class="col-8">Admin</div>
      </div>
    </div>
  </div>
</div>
```

### CSS-Only Stacked Table

```html
<style>
  @media (max-width: 767.98px) {
    .table-stackable thead { display: none; }
    .table-stackable tr { display: block; margin-bottom: 1rem; border: 1px solid #dee2e6; border-radius: 0.375rem; }
    .table-stackable td { display: flex; justify-content: space-between; border: none; padding: 0.5rem 1rem; }
    .table-stackable td::before { content: attr(data-label); font-weight: bold; }
  }
</style>

<table class="table table-stackable">
  <thead><tr><th>Name</th><th>Email</th><th>Role</th></tr></thead>
  <tbody>
    <tr>
      <td data-label="Name">Jane</td>
      <td data-label="Email">jane@example.com</td>
      <td data-label="Role">Admin</td>
    </tr>
  </tbody>
</table>
```

### Responsive Table with Sorting Hints

```html
<div class="table-responsive">
  <table class="table table-sm">
    <thead>
      <tr>
        <th scope="col" class="text-nowrap">Date <i class="bi bi-arrow-down"></i></th>
        <th scope="col">Description</th>
        <th scope="col" class="text-end">Amount</th>
      </tr>
    </thead>
    <tbody>
      <tr><td class="text-nowrap">2024-01-15</td><td>Payment received</td><td class="text-end">+$500</td></tr>
    </tbody>
  </table>
</div>
```

## Best Practices

1. **Use `.table-responsive` for data-heavy tables** - Horizontal scroll preserves all data.
2. **Use `.table-responsive-{breakpoint}` for targeted wrapping** - Only scroll when viewport is too narrow.
3. **Use stacked patterns when columns are labels + values** - Card layout is more readable on mobile.
4. **Always add `scope="col"` and `scope="row"` to `<th>` elements** - Screen readers need header associations.
5. **Keep the most important columns visible on mobile** - Hide less critical columns with `d-none d-md-table-cell`.
6. **Use `.table-striped` for better row tracking** - Alternating colors help users follow rows across columns.
7. **Use `.table-hover` on desktop** - Provides row highlighting on mouse hover.
8. **Set a `min-width` on table containers** - Prevents content from collapsing too narrow.
9. **Use `text-nowrap` on columns that should not wrap** - Dates, IDs, currency values.
10. **Test horizontal scrolling behavior on iOS** - `-webkit-overflow-scrolling: touch` affects momentum.

## Common Pitfalls

1. **Not wrapping tables in `.table-responsive`** - Table overflows container on mobile, causing horizontal page scroll.
2. **Hiding columns on mobile without providing alternative access** - Users lose data; use stacked layout instead.
3. **Using `.table-responsive` on already-small tables** - Unnecessary wrapper adds padding and scroll affordance.
4. **Forgetting `scope` attributes on `<th>`** - Screen readers cannot associate headers with data cells.
5. **Making tables too wide for mobile with too many columns** - Redesign the data presentation for small screens.

## Accessibility Considerations

Tables must use `<th scope="col">` or `<th scope="row">` for screen reader header associations. Responsive tables that hide columns must ensure the remaining data is still understandable. Stacked table patterns should maintain logical reading order. Complex tables with merged cells (`rowspan`, `colspan`) are difficult to navigate on mobile - simplify or use alternative layouts. Caption elements (`<caption>`) provide context for all users.

## Responsive Behavior

`.table-responsive` adds `overflow-x: auto` on screens below the table's natural width, enabling horizontal scroll within the container. `.table-responsive-lg` only applies scrolling below the `lg` breakpoint. Stacked table patterns use display utilities (`d-none d-md-block`) to swap between table and card layouts. Column hiding uses `d-none d-md-table-cell` to remove less important columns on mobile while preserving them on desktop.
