---
title: "Product Reviews"
description: "Build interactive product review components with star ratings, review cards, helpful votes, filtering, and photo reviews using Bootstrap 5."
difficulty: 2
estimated_time: "35 minutes"
prerequisites:
  - "Bootstrap 5 Cards"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Badges"
---

## Overview

Product reviews are essential e-commerce components that build trust and drive conversions. Bootstrap 5 provides the foundational utilities to create star rating displays, review cards with user information, helpful vote mechanisms, photo review galleries, and review filtering systems. These components work together to present social proof effectively while maintaining clean, accessible markup.

The review system typically includes aggregate ratings, individual review cards, filtering controls, and interactive elements like helpful votes. Bootstrap's grid, card, badge, and form components combine to build a comprehensive review experience.

## Basic Implementation

### Star Rating Display

```html
<div class="d-flex align-items-center mb-2">
  <div class="text-warning me-2">
    <i class="bi bi-star-fill"></i>
    <i class="bi bi-star-fill"></i>
    <i class="bi bi-star-fill"></i>
    <i class="bi bi-star-fill"></i>
    <i class="bi bi-star-half"></i>
  </div>
  <span class="fw-bold">4.5</span>
  <span class="text-muted ms-2">(238 reviews)</span>
</div>
```

### Review Card

```html
<div class="card mb-3">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-start mb-2">
      <div class="d-flex align-items-center">
        <img src="avatar.jpg" class="rounded-circle me-2" width="40" height="40" alt="User avatar">
        <div>
          <h6 class="mb-0">Jane Cooper</h6>
          <small class="text-muted">Verified Purchase</small>
        </div>
      </div>
      <div class="text-warning">
        <i class="bi bi-star-fill"></i>
        <i class="bi bi-star-fill"></i>
        <i class="bi bi-star-fill"></i>
        <i class="bi bi-star-fill"></i>
        <i class="bi bi-star"></i>
      </div>
    </div>
    <h6 class="card-title">Great product, highly recommend!</h6>
    <p class="card-text">This exceeded my expectations. The build quality is excellent and it arrived earlier than expected.</p>
    <div class="d-flex justify-content-between align-items-center">
      <small class="text-muted">Posted on March 15, 2026</small>
      <div>
        <button class="btn btn-sm btn-outline-secondary me-1">
          <i class="bi bi-hand-thumbs-up"></i> Helpful (12)
        </button>
        <button class="btn btn-sm btn-outline-secondary">
          <i class="bi bi-hand-thumbs-down"></i> (2)
        </button>
      </div>
    </div>
  </div>
</div>
```

### Interactive Star Rating Input

```html
<div class="mb-3">
  <label class="form-label">Your Rating</label>
  <div class="rating-input d-flex gap-1" role="radiogroup" aria-label="Product rating">
    <input type="radio" class="btn-check" name="rating" id="star1" value="1">
    <label class="btn btn-outline-warning" for="star1"><i class="bi bi-star-fill"></i></label>
    <input type="radio" class="btn-check" name="rating" id="star2" value="2">
    <label class="btn btn-outline-warning" for="star2"><i class="bi bi-star-fill"></i></label>
    <input type="radio" class="btn-check" name="rating" id="star3" value="3">
    <label class="btn btn-outline-warning" for="star3"><i class="bi bi-star-fill"></i></label>
    <input type="radio" class="btn-check" name="rating" id="star4" value="4">
    <label class="btn btn-outline-warning" for="star4"><i class="bi bi-star-fill"></i></label>
    <input type="radio" class="btn-check" name="rating" id="star5" value="5">
    <label class="btn btn-outline-warning" for="star5"><i class="bi bi-star-fill"></i></label>
  </div>
</div>
```

## Advanced Variations

### Review Filters

```html
<div class="d-flex flex-wrap gap-2 mb-4">
  <button class="btn btn-primary btn-sm active" data-filter="all">All Reviews</button>
  <button class="btn btn-outline-secondary btn-sm" data-filter="5">5 Stars (142)</button>
  <button class="btn btn-outline-secondary btn-sm" data-filter="4">4 Stars (64)</button>
  <button class="btn btn-outline-secondary btn-sm" data-filter="3">3 Stars (20)</button>
  <button class="btn btn-outline-secondary btn-sm" data-filter="photo">
    <i class="bi bi-camera"></i> With Photos (38)
  </button>
</div>
```

### Photo Review Gallery

```html
<div class="card mb-3">
  <div class="card-body">
    <div class="d-flex align-items-center mb-3">
      <img src="avatar.jpg" class="rounded-circle me-2" width="36" height="36" alt="">
      <div>
        <strong>Mike Johnson</strong>
        <div class="text-warning">
          <i class="bi bi-star-fill"></i>
          <i class="bi bi-star-fill"></i>
          <i class="bi bi-star-fill"></i>
          <i class="bi bi-star-fill"></i>
          <i class="bi bi-star-fill"></i>
        </div>
      </div>
    </div>
    <p class="card-text">Amazing quality! Here are some photos of the product in use.</p>
    <div class="row g-2 mb-3">
      <div class="col-3">
        <img src="review1.jpg" class="img-fluid rounded" alt="Review photo 1" data-bs-toggle="modal" data-bs-target="#photoModal">
      </div>
      <div class="col-3">
        <img src="review2.jpg" class="img-fluid rounded" alt="Review photo 2" data-bs-toggle="modal" data-bs-target="#photoModal">
      </div>
      <div class="col-3">
        <img src="review3.jpg" class="img-fluid rounded" alt="Review photo 3" data-bs-toggle="modal" data-bs-target="#photoModal">
      </div>
    </div>
    <button class="btn btn-sm btn-outline-primary">
      <i class="bi bi-hand-thumbs-up"></i> Helpful (24)
    </button>
  </div>
</div>
```

### Rating Distribution Bar

```html
<div class="mb-4">
  <div class="d-flex align-items-center mb-1">
    <span class="me-2" style="width: 50px;">5 star</span>
    <div class="progress flex-grow-1" style="height: 12px;">
      <div class="progress-bar bg-warning" style="width: 60%"></div>
    </div>
    <span class="ms-2 text-muted" style="width: 40px;">60%</span>
  </div>
  <div class="d-flex align-items-center mb-1">
    <span class="me-2" style="width: 50px;">4 star</span>
    <div class="progress flex-grow-1" style="height: 12px;">
      <div class="progress-bar bg-warning" style="width: 27%"></div>
    </div>
    <span class="ms-2 text-muted" style="width: 40px;">27%</span>
  </div>
  <div class="d-flex align-items-center mb-1">
    <span class="me-2" style="width: 50px;">3 star</span>
    <div class="progress flex-grow-1" style="height: 12px;">
      <div class="progress-bar bg-warning" style="width: 8%"></div>
    </div>
    <span class="ms-2 text-muted" style="width: 40px;">8%</span>
  </div>
</div>
```

## Best Practices

1. Always use `aria-label` on star rating groups for screen readers
2. Mark verified purchases with a visible badge to build trust
3. Display the total review count alongside the aggregate rating
4. Use `lazy loading` on review photos to improve page performance
5. Implement pagination or infinite scroll for large review sets
6. Show the most recent and most helpful reviews by default
7. Use semantic heading levels for review titles to maintain document hierarchy
8. Include a "Write a Review" call-to-action prominently above the fold
9. Cache review data to reduce server load on high-traffic product pages
10. Use `btn-check` with radio inputs for accessible star rating selection
11. Display reviewer display names rather than full real names for privacy
12. Allow sorting by newest, highest rated, lowest rated, and most helpful
13. Use Bootstrap's `object-fit` utilities for consistent review photo thumbnails
14. Implement rate limiting on review submission to prevent spam

## Common Pitfalls

1. **Missing ARIA labels on star ratings**: Screen readers cannot interpret visual star icons without `role="img"` and `aria-label` attributes describing the rating value.
2. **Not sanitizing user review text**: Failing to escape HTML in review content opens XSS vulnerabilities. Always sanitize or use `textContent` instead of `innerHTML`.
3. **Loading all reviews at once**: Rendering hundreds of review cards simultaneously causes significant performance degradation. Implement virtual scrolling or pagination.
4. **Inconsistent star rendering**: Mixing Font Awesome and Bootstrap Icons for stars leads to visual inconsistencies. Standardize on one icon library.
5. **Missing verified purchase indicators**: Without verification badges, fake reviews are indistinguishable from legitimate ones, eroding user trust.
6. **Hardcoded rating values**: Using static HTML for ratings instead of dynamic data binding means ratings never update without page reloads.
7. **Ignoring mobile layout for photo grids**: Review photo grids that use fixed column counts break on small screens. Always use responsive `col-*` classes.

## Accessibility Considerations

- Use `role="img"` with descriptive `aria-label` on star rating containers (e.g., "4.5 out of 5 stars")
- Ensure keyboard users can navigate through review filters using proper `button` or `tab` elements
- Provide text alternatives for review photos using meaningful `alt` attributes
- Use `aria-live="polite"` regions to announce when reviews are filtered or loaded
- Ensure sufficient color contrast for star icons against their background (WCAG AA 4.5:1 minimum)
- Make helpful vote buttons accessible with clear `aria-label` text including the current count
- Support keyboard interaction for the star rating input using arrow keys within the `radiogroup`

## Responsive Behavior

On mobile devices, review cards should stack vertically with full-width layout. Photo grids should reduce from 4 columns to 2 columns using `col-6` on small screens. Filter buttons should wrap naturally using `d-flex flex-wrap`. The helpful vote buttons can collapse into icon-only versions using `d-none d-md-inline` to show text labels only on larger screens. Rating distribution bars should maintain readable proportions by setting minimum widths on labels and adjusting progress bar heights for touch targets (minimum 44px tap area).
