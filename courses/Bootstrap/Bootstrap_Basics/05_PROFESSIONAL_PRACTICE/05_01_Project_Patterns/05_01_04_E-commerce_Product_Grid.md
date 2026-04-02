---
title: E-commerce Product Grid
category: Professional Practice
difficulty: 3
time: 60 min
tags: bootstrap5, e-commerce, product-grid, cards, filter, cart, responsive
---

## Overview

An e-commerce product grid displays merchandise in a filterable, sortable layout with product cards, a sidebar filter panel, sorting controls, and cart integration. Bootstrap 5's grid, card, dropdown, offcanvas, and utility classes make it straightforward to build a production-ready shop interface.

## Basic Implementation

### Product Card

```html
<div class="card h-100 border-0 shadow-sm">
  <div class="position-relative">
    <img src="product-1.jpg" class="card-img-top" alt="Wireless Headphones">
    <span class="badge bg-danger position-absolute top-0 end-0 m-2">Sale</span>
  </div>
  <div class="card-body d-flex flex-column">
    <p class="text-muted small mb-1">Audio</p>
    <h6 class="card-title">Wireless Headphones</h6>
    <div class="mb-2">
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star text-warning"></i>
      <small class="text-muted">(128)</small>
    </div>
    <div class="mt-auto">
      <span class="fw-bold fs-5">$79.99</span>
      <span class="text-muted text-decoration-line-through ms-2">$99.99</span>
    </div>
  </div>
  <div class="card-footer bg-transparent border-0">
    <button class="btn btn-primary w-100"><i class="bi bi-cart-plus me-1"></i> Add to Cart</button>
  </div>
</div>
```

### Filter Sidebar and Sorting Controls

```html
<div class="container-fluid py-4">
  <div class="row g-4">
    <!-- Desktop Filter Sidebar -->
    <aside class="col-lg-3 d-none d-lg-block">
      <div class="card border-0 shadow-sm mb-4">
        <div class="card-body">
          <h5 class="card-title">Filters</h5>
          <h6 class="mt-3">Category</h6>
          <div class="form-check"><input class="form-check-input" type="checkbox" id="cat1" checked><label class="form-check-label" for="cat1">Electronics</label></div>
          <div class="form-check"><input class="form-check-input" type="checkbox" id="cat2"><label class="form-check-label" for="cat2">Clothing</label></div>
          <div class="form-check"><input class="form-check-input" type="checkbox" id="cat3"><label class="form-check-label" for="cat3">Home & Garden</label></div>
          <h6 class="mt-3">Price Range</h6>
          <div class="d-flex gap-2">
            <input type="number" class="form-control form-control-sm" placeholder="Min">
            <input type="number" class="form-control form-control-sm" placeholder="Max">
          </div>
          <h6 class="mt-3">Rating</h6>
          <div class="form-check"><input class="form-check-input" type="radio" name="rating" id="r4" checked><label class="form-check-label" for="r4">4+ Stars</label></div>
          <div class="form-check"><input class="form-check-input" type="radio" name="rating" id="r3"><label class="form-check-label" for="r3">3+ Stars</label></div>
          <button class="btn btn-primary w-100 mt-3">Apply Filters</button>
        </div>
      </div>
    </aside>

    <!-- Mobile Filter Toggle -->
    <div class="col-12 d-lg-none">
      <button class="btn btn-outline-secondary" data-bs-toggle="offcanvas" data-bs-target="#filterOffcanvas">
        <i class="bi bi-funnel me-1"></i> Filters
      </button>
    </div>

    <div class="col-lg-9">
      <div class="d-flex justify-content-between align-items-center mb-3">
        <p class="mb-0 text-muted">Showing 1-12 of 48 products</p>
        <select class="form-select form-select-sm w-auto">
          <option>Sort by: Featured</option>
          <option>Price: Low to High</option>
          <option>Price: High to Low</option>
          <option>Newest First</option>
        </select>
      </div>
      <div class="row g-3">
        <div class="col-sm-6 col-xl-4">
          <!-- Product Card -->
        </div>
        <!-- Repeat for more products -->
      </div>
    </div>
  </div>
</div>
```

### Mobile Filter Offcanvas

```html
<div class="offcanvas offcanvas-start" tabindex="-1" id="filterOffcanvas">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Filters</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <!-- Same filter controls as desktop sidebar -->
  </div>
</div>
```

### Cart Summary Dropdown

```html
<div class="dropdown">
  <button class="btn btn-outline-dark position-relative" data-bs-toggle="dropdown">
    <i class="bi bi-cart3"></i>
    <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">3</span>
  </button>
  <div class="dropdown-menu dropdown-menu-end p-3" style="width:320px">
    <h6 class="dropdown-header">Cart Summary</h6>
    <div class="d-flex align-items-center mb-2">
      <img src="product-sm.jpg" alt="" class="rounded me-2" width="48" height="48" style="object-fit:cover">
      <div class="flex-grow-1">
        <p class="mb-0 small fw-semibold">Wireless Headphones</p>
        <p class="mb-0 text-muted small">Qty: 1 &mdash; $79.99</p>
      </div>
      <button class="btn btn-sm btn-outline-danger"><i class="bi bi-x"></i></button>
    </div>
    <hr>
    <div class="d-flex justify-content-between fw-bold">
      <span>Total</span><span>$79.99</span>
    </div>
    <a href="#" class="btn btn-primary w-100 mt-2">Checkout</a>
  </div>
</div>
```

## Advanced Variations

- **Quick View Modal:** Trigger a Bootstrap modal with product details, gallery, and add-to-cart button without page navigation.
- **Wishlist Toggle:** Use a heart icon button that toggles between `bi-heart` and `bi-heart-fill` with JavaScript.
- **Image Lazy Loading:** Add `loading="lazy"` to product images for deferred loading.
- **Price Range Slider:** Integrate `noUiSlider` or a native `<input type="range">` for dual-handle price filtering.
- **AJAX Pagination:** Replace product cards with fetched results using `fetch()` and `innerHTML` on page button clicks.

## Best Practices

1. Use `h-100` and `d-flex flex-column` on product cards for uniform height.
2. Apply `position-absolute` on sale badges with `top-0 end-0 m-2` placement.
3. Use `object-fit: cover` with fixed dimensions on all product images.
4. Keep card footers outside the `card-body` with `bg-transparent border-0`.
5. Use `form-select-sm w-auto` for compact sorting dropdowns.
6. Provide an offcanvas version of filters for mobile instead of a separate page.
7. Use `translate-middle` on the cart badge for precise positioning.
8. Apply `dropdown-menu-end` on the cart dropdown to align it to the right edge.
9. Limit product grid to `col-sm-6 col-xl-4` for balanced 2/3 column layouts.
10. Use `gap-3` or `g-3` gutters for consistent card spacing.
11. Display product count text (`Showing 1-12 of 48`) for context.
12. Use `text-decoration-line-through` on original prices to indicate discounts.

## Common Pitfalls

1. **Missing `h-100` on cards:** Creates uneven rows when descriptions vary in length.
2. **No `object-fit` on images:** Product images stretch or distort without it.
3. **Desktop-only filters:** Hiding the offcanvas filter entirely on mobile makes filtering impossible.
4. **Cart badge overflow:** Without `position-relative` on the parent, the badge position breaks.
5. **Too many filter checkboxes:** Group filters into collapsible accordion sections for long lists.
6. **Inaccessible star ratings:** Add `aria-label="4 out of 5 stars"` to the rating container.
7. **Hardcoded product card count:** Always use the grid to auto-wrap; never set a fixed column count.

## Accessibility Considerations

- Add `aria-label="Add Wireless Headphones to cart"` to each add-to-cart button.
- Use `role="search"` on the filter form and `aria-label="Product filters"`.
- Apply `aria-live="polite"` on the product count text so screen readers announce filter changes.
- Ensure all form inputs have associated `<label>` elements or `aria-label`.
- Use `aria-expanded` on the cart dropdown trigger.

## Responsive Behavior

| Breakpoint | Product Grid | Filters | Sorting |
|------------|-------------|---------|---------|
| `<576px` | 1 column | Offcanvas | Full width |
| `≥576px` | 2 columns | Offcanvas | Row |
| `≥768px` | 2 columns | Offcanvas | Row |
| `≥992px` | 2 columns | 3-col sidebar | Row |
| `≥1200px` | 3 columns | 3-col sidebar | Row |
