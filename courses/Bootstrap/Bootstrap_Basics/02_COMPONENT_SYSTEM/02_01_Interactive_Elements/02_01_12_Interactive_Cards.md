---
title: Interactive Cards
category: Component System
difficulty: 2
time: 30 min
tags: bootstrap5, cards, hover, clickable, expandable, interactive
---

## Overview

Bootstrap cards are flexible content containers that can be enhanced with interactive behaviors. By combining card classes with utility classes, CSS transitions, and lightweight JavaScript, you can create clickable cards with hover effects, expandable cards that reveal hidden content, and card action buttons. These patterns are common in dashboards, product listings, and content galleries.

## Basic Implementation

Clickable cards use anchor tags or the `stretched-link` utility to make the entire card a link.

```html
<!-- Clickable card (entire card is a link) -->
<div class="card" style="width: 18rem;">
  <img src="https://via.placeholder.com/300x180" class="card-img-top" alt="Card image">
  <div class="card-body">
    <h5 class="card-title">Clickable Card</h5>
    <p class="card-text">This entire card acts as a clickable link area.</p>
    <a href="#" class="stretched-link"></a>
  </div>
</div>
```

## Advanced Variations

```html
<!-- Card with hover effect -->
<style>
  .card-hover {
    transition: transform 0.2s ease, box-shadow 0.2s ease;
    cursor: pointer;
  }
  .card-hover:hover {
    transform: translateY(-5px);
    box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
  }
</style>

<div class="card card-hover" style="width: 18rem;">
  <img src="https://via.placeholder.com/300x180" class="card-img-top" alt="Hover card">
  <div class="card-body">
    <h5 class="card-title">Hover Effect</h5>
    <p class="card-text">Hover over this card to see the lift and shadow effect.</p>
  </div>
</div>
```

```html
<!-- Expandable card with collapse -->
<div class="card" style="width: 22rem;">
  <div class="card-header d-flex justify-content-between align-items-center">
    <span>Project Details</span>
    <button class="btn btn-sm btn-outline-primary" type="button"
            data-bs-toggle="collapse" data-bs-target="#expandableContent"
            aria-expanded="false" aria-controls="expandableContent">
      <i class="bi bi-chevron-down"></i> Expand
    </button>
  </div>
  <div class="card-body">
    <p class="card-text">Overview summary visible at all times.</p>
    <div class="collapse" id="expandableContent">
      <div class="mt-2">
        <p>Additional details revealed on expand.</p>
        <ul>
          <li>Feature one description</li>
          <li>Feature two description</li>
          <li>Feature three description</li>
        </ul>
      </div>
    </div>
  </div>
  <div class="card-footer text-muted">
    Last updated: 2 hours ago
  </div>
</div>
```

```html
<!-- Card with action buttons -->
<div class="card" style="width: 18rem;">
  <img src="https://via.placeholder.com/300x180" class="card-img-top" alt="Action card">
  <div class="card-body">
    <h5 class="card-title">Card Actions</h5>
    <p class="card-text">Cards can include action buttons in the footer.</p>
  </div>
  <div class="card-footer bg-transparent border-top-0">
    <div class="d-flex justify-content-between">
      <button class="btn btn-primary btn-sm">Edit</button>
      <button class="btn btn-outline-danger btn-sm">Delete</button>
    </div>
  </div>
</div>

<!-- Card with checkbox selection -->
<div class="card border-primary" style="width: 18rem;">
  <div class="card-body">
    <div class="form-check">
      <input class="form-check-input" type="checkbox" id="cardCheck1">
      <label class="form-check-label fw-bold" for="cardCheck1">
        Select this option
      </label>
    </div>
    <p class="card-text mt-2">Click the checkbox or the card to select this item.</p>
  </div>
</div>
```

## Best Practices

1. Use `stretched-link` to make entire cards clickable without wrapping in anchor tags.
2. Apply CSS transitions for smooth hover effects rather than JavaScript.
3. Keep hover effects subtle: translateY of -3 to -5px with a soft box-shadow.
4. Use `card-header` and `card-footer` to separate interactive areas from content.
5. Ensure clickable cards have visible focus states for keyboard navigation.
6. Use `data-bs-toggle="collapse"` for expandable card sections.
7. Place action buttons in `card-footer` with clear spacing between them.
8. Add `cursor: pointer` to cards that serve as interactive elements.
9. Use semantic HTML structure within cards for screen readers.
10. Limit expandable card content to maintain reasonable card height.
11. Combine cards with grid classes for responsive card layouts.

## Common Pitfalls

1. **Overlapping stretched links.** Using `stretched-link` on nested elements creates unpredictable click targets.
2. **No focus indicators.** Removing outline on focus makes cards inaccessible to keyboard users.
3. **Excessive hover animations.** Large transforms or rapid animations disorient users and cause motion sickness.
4. **Expandable cards without ARIA.** Missing `aria-expanded` and `aria-controls` breaks screen reader support.
5. **Unlabeled action buttons.** Icon-only buttons without `aria-label` are inaccessible.
6. **Cards as semantic containers.** Using cards for layout instead of content grouping dilutes their meaning.

## Accessibility Considerations

Clickable cards using `stretched-link` should have descriptive link text or `aria-label` attributes. Expandable cards must include `aria-expanded`, `aria-controls`, and matching `id` targets. Interactive cards must have visible focus indicators using `:focus-visible`. Action buttons within cards need accessible labels, either visible text or `aria-label` for icon buttons. Ensure color is not the sole indicator of card state; use icons, text, or borders.

## Responsive Behavior

Cards fill their parent width by default. Use grid or flex utilities to create responsive card grids: `row-cols-1 row-cols-md-2 row-cols-lg-3`. Set `w-100` or fixed `style="width: ..."` for consistent sizing. On mobile, cards stack vertically; ensure hover effects degrade to tap interactions. Expandable cards should remain scrollable when expanded on small screens. Use `card-img-top` for images that scale with card width.
