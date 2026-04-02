---
title: "Wishlist Interface"
module: "E-Commerce"
difficulty: 1
estimated_time: "20 min"
prerequisites: ["04_01_Card_Component", "04_09_Badges"]
---

## Overview

A wishlist lets customers save products for later, increasing return visits and conversion opportunities. Bootstrap 5 cards, badges, and grid components make it straightforward to build a wishlist page with product cards, move-to-cart actions, share functionality, and an empty state that encourages browsing.

## Basic Implementation

### Wishlist Grid

```html
<div class="container py-5">
  <div class="d-flex justify-content-between align-items-center mb-4">
    <h2>My Wishlist <span class="badge bg-secondary">5</span></h2>
    <div class="d-flex gap-2">
      <button class="btn btn-outline-primary btn-sm">
        <i class="bi bi-share me-1"></i>Share Wishlist
      </button>
      <button class="btn btn-outline-danger btn-sm">
        <i class="bi bi-trash me-1"></i>Clear All
      </button>
    </div>
  </div>
  <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4 g-4">
    <div class="col">
      <div class="card h-100">
        <div class="position-relative">
          <img src="product.jpg" class="card-img-top" alt="Wireless Headphones" loading="lazy">
          <button class="btn btn-sm btn-danger position-absolute top-0 end-0 m-2 rounded-circle" style="width:36px;height:36px" aria-label="Remove from wishlist">
            <i class="bi bi-heart-fill"></i>
          </button>
        </div>
        <div class="card-body d-flex flex-column">
          <h6 class="card-title">Wireless Headphones</h6>
          <div class="text-warning small mb-2">
            <i class="bi bi-star-fill"></i> 4.5 <span class="text-muted">(128)</span>
          </div>
          <p class="fw-bold text-primary mb-3">$129.99</p>
          <div class="mt-auto">
            <button class="btn btn-primary w-100 mb-2">
              <i class="bi bi-cart-plus me-1"></i>Add to Cart
            </button>
            <small class="text-muted d-block text-center">
              <i class="bi bi-truck me-1"></i>Free shipping
            </small>
          </div>
        </div>
      </div>
    </div>
    <!-- Additional wishlist items -->
  </div>
</div>
```

## Advanced Variations

### Share Wishlist Modal

```html
<div class="modal fade" id="shareWishlistModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Share Your Wishlist</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <p class="text-muted">Share your wishlist with friends and family.</p>
        <div class="input-group mb-3">
          <input type="text" class="form-control" value="https://shop.example.com/wishlist/abc123" readonly id="shareLink">
          <button class="btn btn-outline-primary" onclick="navigator.clipboard.writeText(document.getElementById('shareLink').value)">
            <i class="bi bi-clipboard"></i>
          </button>
        </div>
        <div class="d-flex justify-content-center gap-3">
          <button class="btn btn-outline-primary rounded-circle" style="width:48px;height:48px" aria-label="Share on Facebook">
            <i class="bi bi-facebook"></i>
          </button>
          <button class="btn btn-outline-info rounded-circle" style="width:48px;height:48px" aria-label="Share on Twitter">
            <i class="bi bi-twitter-x"></i>
          </button>
          <button class="btn btn-outline-success rounded-circle" style="width:48px;height:48px" aria-label="Share via Email">
            <i class="bi bi-envelope"></i>
          </button>
          <button class="btn btn-outline-secondary rounded-circle" style="width:48px;height:48px" aria-label="Share via WhatsApp">
            <i class="bi bi-whatsapp"></i>
          </button>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Empty Wishlist State

```html
<div class="text-center py-5">
  <i class="bi bi-heart display-1 text-muted mb-4"></i>
  <h3>Your wishlist is empty</h3>
  <p class="text-muted mb-4">Save items you love by clicking the heart icon on any product.</p>
  <a href="catalog.html" class="btn btn-primary">
    <i class="bi bi-bag me-2"></i>Start Shopping
  </a>
</div>
```

### Out of Stock Indicator

```html
<div class="card h-100">
  <div class="position-relative">
    <img src="product.jpg" class="card-img-top opacity-75" alt="Out of stock product">
    <div class="position-absolute top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center" style="background:rgba(0,0,0,0.3)">
      <span class="badge bg-dark fs-6">Out of Stock</span>
    </div>
  </div>
  <div class="card-body d-flex flex-column">
    <h6 class="card-title">Limited Edition Speaker</h6>
    <p class="fw-bold mb-3">$249.99</p>
    <div class="mt-auto">
      <button class="btn btn-outline-primary w-100 mb-2" disabled>Out of Stock</button>
      <button class="btn btn-outline-secondary w-100 btn-sm">
        <i class="bi bi-bell me-1"></i>Notify When Available
      </button>
    </div>
  </div>
</div>
```

## Best Practices

1. Display the wishlist item count in the page header
2. Use a filled heart icon (`bi-heart-fill`) to indicate saved items
3. Provide "Add to Cart" as the primary action on each card
4. Include "Remove" as a quick, non-destructive action
5. Show product availability status (in stock, low stock, out of stock)
6. Disable add-to-cart for out-of-stock items with a "Notify Me" alternative
7. Offer a "Share Wishlist" feature to drive social engagement
8. Use `loading="lazy"` on product images for performance
9. Provide a "Clear All" option with confirmation
10. Persist wishlist across sessions using localStorage or user accounts

## Common Pitfalls

1. **No empty state** - A blank wishlist page with no guidance frustrates users. Show a friendly prompt to browse.
2. **Removing items without feedback** - Use a toast notification to confirm removal with an undo option.
3. **Stale prices** - Prices may change after items are added. Fetch current prices and show a "Price changed" indicator.
4. **No stock status** - Users add out-of-stock items to cart and encounter errors. Show status upfront.
5. **Missing share feature** - Wishlists are a conversion driver when shared. Make sharing easy.
6. **Not responsive** - A 4-column grid breaks on phones. Use `row-cols-1` on mobile.

## Accessibility Considerations

- Use `aria-label="Remove Wireless Headphones from wishlist"` on remove buttons
- Announce item count changes with `aria-live="polite"`
- Mark out-of-stock overlays with `aria-disabled="true"` on the card
- Ensure all buttons are keyboard-accessible with visible focus states
- Provide `aria-label` on share buttons (e.g., "Share on Facebook")

## Responsive Behavior

On **mobile**, cards display in a single column with full-width buttons. On **small tablets**, a 2-column grid works well. On **desktop**, a 4-column grid maximizes visible products. The share modal remains centered at all breakpoints. Action buttons stack vertically on small screens and sit side by side on larger ones.
