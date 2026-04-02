---
title: "Product Detail Page"
module: "E-Commerce"
difficulty: 2
estimated_time: "30 min"
prerequisites: ["04_01_Card_Component", "04_05_Forms", "06_01_Carousel"]
---

## Overview

The product detail page (PDP) is where purchase decisions are made. It showcases product images, descriptions, variants, pricing, reviews, and related items. Bootstrap 5 provides carousel for image galleries, form controls for variant selectors, tabs for organizing content, and cards for related products. A well-designed PDP answers every customer question and reduces support inquiries.

## Basic Implementation

### Product Image Gallery with Carousel

```html
<div class="row g-5">
  <div class="col-lg-6">
    <div id="productCarousel" class="carousel slide" data-bs-ride="false">
      <div class="carousel-inner rounded">
        <div class="carousel-item active">
          <img src="product-1.jpg" class="d-block w-100" alt="Product front view" style="object-fit:cover;max-height:500px">
        </div>
        <div class="carousel-item">
          <img src="product-2.jpg" class="d-block w-100" alt="Product side view" style="object-fit:cover;max-height:500px">
        </div>
        <div class="carousel-item">
          <img src="product-3.jpg" class="d-block w-100" alt="Product detail view" style="object-fit:cover;max-height:500px">
        </div>
      </div>
      <button class="carousel-control-prev" type="button" data-bs-target="#productCarousel" data-bs-slide="prev">
        <span class="carousel-control-prev-icon"></span>
      </button>
      <button class="carousel-control-next" type="button" data-bs-target="#productCarousel" data-bs-slide="next">
        <span class="carousel-control-next-icon"></span>
      </button>
    </div>
    <!-- Thumbnails -->
    <div class="d-flex gap-2 mt-3">
      <img src="product-1-thumb.jpg" class="rounded border border-primary cursor-pointer" width="80" height="80" style="object-fit:cover" data-bs-target="#productCarousel" data-bs-slide-to="0">
      <img src="product-2-thumb.jpg" class="rounded border cursor-pointer" width="80" height="80" style="object-fit:cover" data-bs-target="#productCarousel" data-bs-slide-to="1">
      <img src="product-3-thumb.jpg" class="rounded border cursor-pointer" width="80" height="80" style="object-fit:cover" data-bs-target="#productCarousel" data-bs-slide-to="2">
    </div>
  </div>

  <!-- Product Info -->
  <div class="col-lg-6">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item"><a href="#">Home</a></li>
        <li class="breadcrumb-item"><a href="#">Headphones</a></li>
        <li class="breadcrumb-item active">Wireless Pro</li>
      </ol>
    </nav>
    <h1 class="h2">Wireless Pro Headphones</h1>
    <div class="mb-3">
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-fill text-warning"></i>
      <i class="bi bi-star-half text-warning"></i>
      <a href="#reviews" class="ms-2">4.5 (238 reviews)</a>
    </div>
    <div class="mb-4">
      <span class="text-decoration-line-through text-muted fs-5 me-2">$179.99</span>
      <span class="text-danger fw-bold fs-3">$129.99</span>
      <span class="badge bg-danger ms-2">Save 28%</span>
    </div>
    <p class="text-muted mb-4">Premium wireless headphones with active noise cancellation, 30-hour battery life, and premium comfort padding.</p>

    <!-- Variant Selectors -->
    <div class="mb-3">
      <label class="form-label fw-bold">Color</label>
      <div class="d-flex gap-2">
        <input type="radio" class="btn-check" name="color" id="colorBlack" checked>
        <label class="btn btn-outline-dark rounded-circle p-3" for="colorBlack" title="Black"></label>
        <input type="radio" class="btn-check" name="color" id="colorWhite">
        <label class="btn btn-outline-secondary rounded-circle p-3" for="colorWhite" title="White"></label>
        <input type="radio" class="btn-check" name="color" id="colorBlue">
        <label class="btn btn-outline-primary rounded-circle p-3" for="colorBlue" title="Blue"></label>
      </div>
    </div>
    <div class="mb-4">
      <label for="size" class="form-label fw-bold">Size</label>
      <select class="form-select w-auto" id="size">
        <option>One Size Fits All</option>
      </select>
    </div>

    <!-- Quantity and Add to Cart -->
    <div class="d-flex gap-3 mb-4">
      <div class="input-group" style="width:130px">
        <button class="btn btn-outline-secondary" type="button">-</button>
        <input type="number" class="form-control text-center" value="1" min="1">
        <button class="btn btn-outline-secondary" type="button">+</button>
      </div>
      <button class="btn btn-primary flex-grow-1">
        <i class="bi bi-cart-plus me-2"></i>Add to Cart
      </button>
      <button class="btn btn-outline-danger" title="Add to Wishlist">
        <i class="bi bi-heart"></i>
      </button>
    </div>

    <!-- Trust Signals -->
    <div class="row g-3 text-center">
      <div class="col-4">
        <i class="bi bi-truck fs-4 text-primary"></i>
        <div class="small mt-1">Free Shipping</div>
      </div>
      <div class="col-4">
        <i class="bi bi-arrow-return-left fs-4 text-primary"></i>
        <div class="small mt-1">30-Day Returns</div>
      </div>
      <div class="col-4">
        <i class="bi bi-shield-check fs-4 text-primary"></i>
        <div class="small mt-1">2-Year Warranty</div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Product Details Tabs

```html
<ul class="nav nav-tabs mt-5" role="tablist">
  <li class="nav-item"><button class="nav-link active" data-bs-toggle="tab" data-bs-target="#descTab">Description</button></li>
  <li class="nav-item"><button class="nav-link" data-bs-toggle="tab" data-bs-target="#specsTab">Specifications</button></li>
  <li class="nav-item"><button class="nav-link" data-bs-toggle="tab" data-bs-target="#reviewsTab">Reviews (238)</button></li>
</ul>
<div class="tab-content border border-top-0 p-4">
  <div class="tab-pane fade show active" id="descTab">
    <p>Experience immersive audio with active noise cancellation technology...</p>
  </div>
  <div class="tab-pane fade" id="specsTab">
    <table class="table table-striped">
      <tbody>
        <tr><th>Driver Size</th><td>40mm</td></tr>
        <tr><th>Battery Life</th><td>30 hours</td></tr>
        <tr><th>Weight</th><td>250g</td></tr>
      </tbody>
    </table>
  </div>
  <div class="tab-pane fade" id="reviewsTab">
    <div class="d-flex mb-4">
      <div class="text-center me-4">
        <div class="display-4 fw-bold">4.5</div>
        <div class="text-warning mb-1">
          <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i>
          <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i>
          <i class="bi bi-star-half"></i>
        </div>
        <div class="text-muted small">238 reviews</div>
      </div>
      <div class="flex-grow-1">
        <div class="d-flex align-items-center mb-1">
          <span class="small me-2">5<i class="bi bi-star-fill text-warning ms-1"></i></span>
          <div class="progress flex-grow-1" style="height:8px">
            <div class="progress-bar bg-warning" style="width:68%"></div>
          </div>
          <span class="small ms-2 text-muted">162</span>
        </div>
        <div class="d-flex align-items-center mb-1">
          <span class="small me-2">4<i class="bi bi-star-fill text-warning ms-1"></i></span>
          <div class="progress flex-grow-1" style="height:8px">
            <div class="progress-bar bg-warning" style="width:20%"></div>
          </div>
          <span class="small ms-2 text-muted">48</span>
        </div>
      </div>
    </div>
    <div class="border-top pt-3">
      <div class="mb-3 pb-3 border-bottom">
        <div class="d-flex align-items-center mb-2">
          <strong>Jane D.</strong>
          <span class="badge bg-success ms-2">Verified</span>
          <span class="text-muted small ms-auto">2 days ago</span>
        </div>
        <div class="text-warning mb-1"><i class="bi bi-star-fill"></i>x5</div>
        <p>Amazing sound quality and the battery lasts forever!</p>
      </div>
    </div>
  </div>
</div>
```

### Related Products

```html
<section class="mt-5">
  <h3 class="mb-4">You May Also Like</h3>
  <div class="row row-cols-2 row-cols-md-4 g-4">
    <div class="col">
      <div class="card h-100">
        <img src="related-1.jpg" class="card-img-top" alt="Related product" loading="lazy">
        <div class="card-body">
          <h6 class="card-title">Bluetooth Speaker</h6>
          <span class="fw-bold text-primary">$49.99</span>
        </div>
      </div>
    </div>
    <!-- More related products -->
  </div>
</section>
```

## Best Practices

1. Provide multiple product images with thumbnail navigation
2. Use breadcrumb navigation for orientation within the site hierarchy
3. Display price prominently with sale/original pricing clearly distinguished
4. Include variant selectors (color, size) with visual feedback on selection
5. Show trust signals (shipping, returns, warranty) near the add-to-cart button
6. Use tabs to organize long content (description, specs, reviews)
7. Include a review summary with rating distribution bars
8. Lazy-load images below the fold for performance
9. Provide quantity controls with min/max validation
10. Show stock availability status clearly
11. Add schema.org Product structured data for SEO

## Common Pitfalls

1. **Single product image** - Customers need multiple angles. Provide at least 3-5 images.
2. **No breadcrumb** - Users lose context on deep product pages. Always show navigation hierarchy.
3. **Variant selection without visual feedback** - Users can't tell which option is selected. Use `btn-check` pattern with active states.
4. **Reviews hidden in tabs** - Social proof influences purchase. Show rating summary above the fold.
5. **No mobile image optimization** - Full-size images waste bandwidth. Serve responsive images with `srcset`.
6. **Missing structured data** - Without schema.org markup, search engines can't display rich product snippets.

## Accessibility Considerations

- Provide descriptive alt text for all product images ("Product front view in black")
- Use `aria-label` on color variant buttons (e.g., "Select Black color")
- Mark the carousel with `aria-label="Product images"`
- Use `aria-current="true"` on the active thumbnail
- Associate variant labels with their controls using `<label for="...">`
- Announce price changes when variants are selected using `aria-live`
- Ensure tab panels use proper `role="tabpanel"` and `aria-labelledby`

## Responsive Behavior

On **mobile**, the image carousel takes full width above the product info. Variant selectors use horizontal scrollable pill groups. The related products grid becomes a 2-column layout. On **tablet**, the carousel and product info sit side by side. On **desktop**, generous padding and larger images create an immersive product showcase. Tabs remain horizontal across all breakpoints.
