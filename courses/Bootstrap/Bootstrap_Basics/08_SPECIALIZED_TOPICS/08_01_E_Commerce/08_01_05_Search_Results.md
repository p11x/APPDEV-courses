---
title: "Search Results"
module: "E-Commerce"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["02_01_Grid_System", "04_01_Card_Component", "05_03_Pagination"]
---

## Overview

Search results pages help users find products quickly from a query. They combine a results grid, faceted filters, pagination, and a no-results state. Bootstrap 5 provides the grid, offcanvas, pagination, badge, and form components to build effective search result pages that handle both successful queries and empty states gracefully.

## Basic Implementation

### Search Results with Header and Pagination

```html
<div class="container py-4">
  <!-- Search Header -->
  <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
    <div>
      <h2 class="h4 mb-1">Results for "wireless headphones"</h2>
      <p class="text-muted mb-0">Showing 1-24 of 156 results</p>
    </div>
    <div class="d-flex gap-2 mt-2 mt-sm-0">
      <button class="btn btn-outline-secondary d-lg-none" data-bs-toggle="offcanvas" data-bs-target="#filterPanel">
        <i class="bi bi-funnel me-1"></i>Filters
      </button>
      <select class="form-select form-select-sm" style="width:auto">
        <option>Relevance</option>
        <option>Price: Low to High</option>
        <option>Price: High to Low</option>
        <option>Newest</option>
        <option>Best Selling</option>
      </select>
    </div>
  </div>

  <div class="row">
    <!-- Faceted Filters -->
    <div class="col-lg-3 d-none d-lg-block">
      <div class="mb-4">
        <h6 class="fw-bold d-flex justify-content-between align-items-center">
          Category
          <i class="bi bi-chevron-down small"></i>
        </h6>
        <div class="form-check"><input class="form-check-input" type="checkbox" checked><label class="form-check-label">Over-Ear (45)</label></div>
        <div class="form-check"><input class="form-check-input" type="checkbox"><label class="form-check-label">In-Ear (72)</label></div>
        <div class="form-check"><input class="form-check-input" type="checkbox"><label class="form-check-label">On-Ear (39)</label></div>
      </div>
      <div class="mb-4">
        <h6 class="fw-bold">Price</h6>
        <div class="form-check"><input class="form-check-input" type="radio" name="price"><label class="form-check-label">Under $50</label></div>
        <div class="form-check"><input class="form-check-input" type="radio" name="price"><label class="form-check-label">$50 - $100</label></div>
        <div class="form-check"><input class="form-check-input" type="radio" name="price"><label class="form-check-label">$100 - $200</label></div>
        <div class="form-check"><input class="form-check-input" type="radio" name="price"><label class="form-check-label">$200+</label></div>
      </div>
      <div class="mb-4">
        <h6 class="fw-bold">Brand</h6>
        <div class="form-check"><input class="form-check-input" type="checkbox"><label class="form-check-label">SoundMax (18)</label></div>
        <div class="form-check"><input class="form-check-input" type="checkbox"><label class="form-check-label">AudioPro (24)</label></div>
      </div>
      <div class="mb-4">
        <h6 class="fw-bold">Rating</h6>
        <div class="form-check">
          <input class="form-check-input" type="checkbox">
          <label class="form-check-label text-warning">
            <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i> & Up
          </label>
        </div>
      </div>
    </div>

    <!-- Results Grid -->
    <div class="col-lg-9">
      <!-- Active Filters -->
      <div class="mb-3">
        <span class="badge bg-light text-dark border me-2 py-2 px-3">
          Over-Ear <button class="btn-close btn-close-sm ms-1" style="font-size:0.6em"></button>
        </span>
        <a href="#" class="small">Clear all filters</a>
      </div>

      <div class="row row-cols-2 row-cols-md-3 g-4">
        <div class="col">
          <div class="card h-100">
            <img src="product.jpg" class="card-img-top" alt="Wireless Headphones" loading="lazy">
            <div class="card-body">
              <h6 class="card-title">SoundMax Pro X1</h6>
              <div class="text-warning small mb-2"><i class="bi bi-star-fill"></i>4.5 (128)</div>
              <span class="fw-bold">$129.99</span>
            </div>
          </div>
        </div>
        <!-- More result cards -->
      </div>

      <!-- Pagination -->
      <nav class="mt-5 d-flex justify-content-center" aria-label="Search results pages">
        <ul class="pagination">
          <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
          <li class="page-item active"><a class="page-link" href="#">1</a></li>
          <li class="page-item"><a class="page-link" href="#">2</a></li>
          <li class="page-item"><a class="page-link" href="#">3</a></li>
          <li class="page-item"><a class="page-link" href="#">...</a></li>
          <li class="page-item"><a class="page-link" href="#">7</a></li>
          <li class="page-item"><a class="page-link" href="#">Next</a></li>
        </ul>
      </nav>
    </div>
  </div>
</div>
```

## Advanced Variations

### No Results State

```html
<div class="text-center py-5">
  <i class="bi bi-search display-1 text-muted mb-4"></i>
  <h3>No results for "xyzqwerty"</h3>
  <p class="text-muted mb-4">Try checking your spelling or using different keywords.</p>
  <div class="mb-4">
    <h6 class="text-muted">Popular Searches:</h6>
    <a href="#" class="badge bg-light text-dark text-decoration-none me-1 py-2 px-3">Wireless headphones</a>
    <a href="#" class="badge bg-light text-dark text-decoration-none me-1 py-2 px-3">Bluetooth speakers</a>
    <a href="#" class="badge bg-light text-dark text-decoration-none me-1 py-2 px-3">Smart watches</a>
  </div>
  <a href="catalog.html" class="btn btn-primary">Browse All Products</a>
</div>
```

### Mobile Filter Offcanvas

```html
<div class="offcanvas offcanvas-start" tabindex="-1" id="filterPanel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Filters</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <!-- Same filter content as sidebar -->
    <div class="mb-4">
      <h6 class="fw-bold">Category</h6>
      <div class="form-check"><input class="form-check-input" type="checkbox"><label class="form-check-label">Over-Ear</label></div>
      <div class="form-check"><input class="form-check-input" type="checkbox"><label class="form-check-label">In-Ear</label></div>
    </div>
  </div>
  <div class="offcanvas-footer border-top p-3">
    <button class="btn btn-primary w-100" data-bs-dismiss="offcanvas">Show 156 Results</button>
  </div>
</div>
```

## Best Practices

1. Display result count prominently so users know the scope
2. Use active filter chips with individual remove buttons
3. Provide a "Clear all" link to reset all filters at once
4. Show filter option counts (e.g., "Over-Ear (45)") to guide selection
5. Default sort to "Relevance" for keyword searches
6. Use `loading="lazy"` on product images for performance
7. Make pagination accessible with `aria-label` on the nav element
8. Provide a friendly no-results page with alternative suggestions
9. Highlight the search term in the results header
10. Use offcanvas for filters on mobile to maximize results space
11. Show filter button with count badge on mobile ("Filters (3)")

## Common Pitfalls

1. **No empty state** - Showing a blank page for no results leaves users stranded. Provide suggestions and alternatives.
2. **Filters not shown on mobile** - Hiding filters entirely on small screens removes critical functionality. Use offcanvas.
3. **Missing result count** - Users need context. Always show "Showing X-Y of Z results."
4. **Pagination too far from results** - Placing pagination only at the bottom forces scrolling. Consider top and bottom pagination.
5. **No active filter indication** - Users forget which filters are applied. Show filter chips with remove buttons.
6. **Sort dropdown not labeled** - Screen readers need context. Use `aria-label="Sort results by"`.
7. **Filter count not updated** - After applying filters, counts should reflect the current result set.

## Accessibility Considerations

- Use `role="search"` on the search form area
- Announce result count changes with `aria-live="polite"`
- Label the pagination nav with `aria-label="Search results pagination"`
- Use `aria-current="page"` on the active pagination item
- Ensure filter checkboxes have associated labels
- Provide keyboard navigation for filter toggles
- Mark the results region with `role="region" aria-label="Search results"`

## Responsive Behavior

On **mobile**, filters move into an offcanvas panel triggered by a "Filters" button with a count badge. Results display in a 2-column grid. On **tablet**, the filter sidebar can appear inline or remain in offcanvas. Results shift to a 3-column grid. On **desktop**, the full filter sidebar displays alongside the results grid with up to 4 columns. Pagination centers below the results at all breakpoints.
