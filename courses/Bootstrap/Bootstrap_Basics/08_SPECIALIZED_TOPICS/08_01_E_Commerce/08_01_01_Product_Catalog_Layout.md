---
title: "Product Catalog Layout"
module: "E-Commerce"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["01_01_Introduction", "02_01_Grid_System", "04_01_Card_Component"]
---

## Overview

A product catalog layout is the backbone of any e-commerce storefront. It presents merchandise in an organized, scannable grid that adapts across viewports. Bootstrap 5 provides the grid system, cards, dropdowns, and offcanvas components needed to build a fully functional catalog with filter sidebar, sort controls, and responsive product cards. This pattern balances visual appeal with usability, ensuring customers can browse, filter, and sort products efficiently on any device.

## Basic Implementation

### Product Grid with Cards

The simplest catalog uses Bootstrap's card component inside a responsive grid:

```html
<div class="container py-4">
  <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
    <div class="col">
      <div class="card h-100">
        <img src="product-1.jpg" class="card-img-top" alt="Wireless Headphones">
        <div class="card-body">
          <h5 class="card-title">Wireless Headphones</h5>
          <p class="card-text text-muted">Premium sound quality with noise cancellation.</p>
          <div class="d-flex justify-content-between align-items-center">
            <span class="fw-bold text-primary">$129.99</span>
            <button class="btn btn-sm btn-outline-primary">Add to Cart</button>
          </div>
        </div>
      </div>
    </div>
    <!-- Repeat for additional products -->
  </div>
</div>
```

### Sort Controls

Add a dropdown to let users reorder products:

```html
<div class="d-flex justify-content-between align-items-center mb-4">
  <span class="text-muted">Showing 1-12 of 48 products</span>
  <div class="dropdown">
    <button class="btn btn-outline-secondary dropdown-toggle" type="button"
            data-bs-toggle="dropdown" aria-expanded="false">
      Sort by: Featured
    </button>
    <ul class="dropdown-menu dropdown-menu-end">
      <li><a class="dropdown-item active" href="#">Featured</a></li>
      <li><a class="dropdown-item" href="#">Price: Low to High</a></li>
      <li><a class="dropdown-item" href="#">Price: High to Low</a></li>
      <li><a class="dropdown-item" href="#">Newest Arrivals</a></li>
      <li><a class="dropdown-item" href="#">Best Rating</a></li>
    </ul>
  </div>
</div>
```

## Advanced Variations

### Filter Sidebar with Offcanvas

For mobile-friendly filtering, use the offcanvas component:

```html
<!-- Trigger Button (visible on mobile) -->
<button class="btn btn-outline-primary d-lg-none mb-3" type="button"
        data-bs-toggle="offcanvas" data-bs-target="#filterSidebar">
  <i class="bi bi-funnel me-2"></i>Filters
</button>

<div class="row">
  <!-- Filter Sidebar -->
  <div class="col-lg-3 d-none d-lg-block">
    <div class="offcanvas-lg offcanvas-start show position-static" id="filterSidebar">
      <div class="offcanvas-header d-lg-none">
        <h5 class="offcanvas-title">Filters</h5>
        <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
      </div>
      <div class="offcanvas-body p-0">
        <!-- Category Filter -->
        <div class="mb-4">
          <h6 class="fw-bold mb-3">Category</h6>
          <div class="form-check mb-2">
            <input class="form-check-input" type="checkbox" id="catElectronics">
            <label class="form-check-label" for="catElectronics">Electronics</label>
          </div>
          <div class="form-check mb-2">
            <input class="form-check-input" type="checkbox" id="catClothing">
            <label class="form-check-label" for="catClothing">Clothing</label>
          </div>
        </div>
        <!-- Price Range -->
        <div class="mb-4">
          <h6 class="fw-bold mb-3">Price Range</h6>
          <div class="row g-2">
            <div class="col-6">
              <input type="number" class="form-control form-control-sm" placeholder="Min">
            </div>
            <div class="col-6">
              <input type="number" class="form-control form-control-sm" placeholder="Max">
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Product Grid -->
  <div class="col-lg-9">
    <!-- Products here -->
  </div>
</div>
```

### Product Card with Badge and Rating

```html
<div class="card h-100 position-relative">
  <span class="badge bg-danger position-absolute top-0 start-0 m-2">Sale</span>
  <img src="product.jpg" class="card-img-top" alt="Product Name">
  <div class="card-body d-flex flex-column">
    <h6 class="card-title">Premium Sneakers</h6>
    <div class="mb-2">
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-half text-warning"></i>
      <small class="text-muted">(128)</small>
    </div>
    <div class="mt-auto">
      <span class="text-decoration-line-through text-muted me-2">$149.99</span>
      <span class="fw-bold text-danger">$99.99</span>
    </div>
  </div>
</div>
```

## Best Practices

1. Use `row-cols-*` classes to control column count per breakpoint instead of hardcoding `col-md-4`
2. Apply `h-100` to cards to maintain equal height within a row
3. Use `g-4` gutter utility for consistent spacing between cards
4. Implement offcanvas for filters on mobile to save screen real estate
5. Lazy-load product images with `loading="lazy"` attribute
6. Use semantic heading levels (`h5`/`h6`) inside cards for screen reader hierarchy
7. Include `aria-label` on sort dropdowns and filter controls
8. Provide skeleton loading states while products fetch
9. Use `object-fit: cover` on card images for consistent aspect ratios
10. Display product count to orient users within the catalog
11. Keep filter sidebar sticky on scroll for long product lists
12. Use badge components for sale, new, or featured indicators
13. Ensure tap targets are at least 44x44px for mobile usability

## Common Pitfalls

1. **Fixed column counts across all breakpoints** - Using only `col-md-3` forces 4 columns even on tablets. Always define breakpoints for `row-cols-1`, `row-cols-sm-2`, `row-cols-md-3`.
2. **Missing image alt text** - Product images without alt attributes break accessibility and SEO. Always describe the product.
3. **Overloading filter sidebar on mobile** - Showing a full sidebar on small screens pushes content below the fold. Use offcanvas or a modal for filters on mobile.
4. **No loading or empty state** - If products fail to load or filters return zero results, users see a blank page. Always provide feedback.
5. **Inconsistent card heights** - Cards with varying content lengths create a jagged grid. Use `h-100` and flexbox to equalize.
6. **Ignoring keyboard navigation** - Dropdowns and filter controls must be keyboard-accessible. Test with Tab and Enter keys.
7. **Cluttered sort/filter UI** - Too many options overwhelm users. Group related filters with clear headings and collapsible sections.

## Accessibility Considerations

- Add `role="region" aria-label="Product catalog"` to the main catalog container
- Use `aria-live="polite"` on the product count area so screen readers announce filter results
- Ensure all interactive elements (buttons, links, checkboxes) are reachable via Tab key
- Provide visible focus indicators using Bootstrap's `focus-ring` utility
- Use `aria-sort="ascending"` on sortable column headers if using a table layout alternative
- Label all filter inputs with associated `<label>` elements or `aria-label`
- Announce sort changes to assistive technology with a live region

## Responsive Behavior

On **extra-small screens** (<576px), products display in a single column with full-width cards. The filter button triggers an offcanvas panel that slides in from the left. On **small screens** (576px+), the grid shifts to 2 columns. On **medium screens** (768px+), the filter sidebar appears inline as a 3-column sidebar with the product grid occupying 9 columns. On **large screens** (992px+), the grid can expand to 4 columns with wider cards. On **extra-large screens** (1200px+), consider max-width containers to prevent cards from stretching too wide, maintaining readability and image quality.
