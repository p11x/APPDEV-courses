---
title: "SaaS Landing Page"
module: "SaaS Applications"
difficulty: 2
estimated_time: "30 min"
prerequisites: ["02_01_Grid_System", "04_01_Card_Component", "05_01_Accordion"]
---

## Overview

A SaaS landing page converts visitors into users by communicating value, building trust, and driving action. Bootstrap 5 provides hero sections with display typography, card grids for features and pricing, accordion for FAQs, and responsive utilities to build high-converting landing pages that work across all devices.

## Basic Implementation

### Hero Section with CTA

```html
<section class="bg-primary text-white py-5">
  <div class="container py-5">
    <div class="row align-items-center">
      <div class="col-lg-6">
        <h1 class="display-4 fw-bold mb-4">Build Better Products, Faster</h1>
        <p class="lead mb-4 opacity-75">The all-in-one platform for teams to plan, build, and ship software that customers love.</p>
        <div class="d-flex gap-3 flex-wrap">
          <a href="#" class="btn btn-light btn-lg px-4">Start Free Trial</a>
          <a href="#" class="btn btn-outline-light btn-lg px-4">
            <i class="bi bi-play-circle me-2"></i>Watch Demo
          </a>
        </div>
        <p class="mt-3 small opacity-75">No credit card required. 14-day free trial.</p>
      </div>
      <div class="col-lg-6 mt-4 mt-lg-0">
        <img src="hero-dashboard.png" alt="Dashboard preview" class="img-fluid rounded shadow">
      </div>
    </div>
  </div>
</section>
```

### Feature Comparison Table

```html
<section class="py-5">
  <div class="container">
    <h2 class="text-center mb-2">Compare Plans</h2>
    <p class="text-center text-muted mb-5">Choose the plan that fits your team</p>
    <div class="table-responsive">
      <table class="table table-bordered text-center">
        <thead class="table-light">
          <tr>
            <th class="text-start">Feature</th>
            <th>Starter</th>
            <th class="table-primary">Pro</th>
            <th>Enterprise</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td class="text-start">Projects</td>
            <td>5</td>
            <td>Unlimited</td>
            <td>Unlimited</td>
          </tr>
          <tr>
            <td class="text-start">Team Members</td>
            <td>3</td>
            <td>15</td>
            <td>Unlimited</td>
          </tr>
          <tr>
            <td class="text-start">API Access</td>
            <td><i class="bi bi-x-circle text-muted"></i></td>
            <td><i class="bi bi-check-circle text-success"></i></td>
            <td><i class="bi bi-check-circle text-success"></i></td>
          </tr>
          <tr>
            <td class="text-start">SSO / SAML</td>
            <td><i class="bi bi-x-circle text-muted"></i></td>
            <td><i class="bi bi-x-circle text-muted"></i></td>
            <td><i class="bi bi-check-circle text-success"></i></td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</section>
```

## Advanced Variations

### Pricing Section with Toggle

```html
<section class="py-5 bg-light">
  <div class="container">
    <h2 class="text-center mb-2">Simple, Transparent Pricing</h2>
    <div class="text-center mb-5">
      <div class="btn-group" role="group">
        <input type="radio" class="btn-check" name="billing" id="monthly" checked>
        <label class="btn btn-outline-primary" for="monthly">Monthly</label>
        <input type="radio" class="btn-check" name="billing" id="annual">
        <label class="btn btn-outline-primary" for="annual">Annual <span class="badge bg-success">Save 20%</span></label>
      </div>
    </div>
    <div class="row g-4 justify-content-center">
      <div class="col-md-4">
        <div class="card h-100">
          <div class="card-body text-center p-4">
            <h5 class="card-title">Starter</h5>
            <div class="my-4">
              <span class="display-5 fw-bold">$9</span>
              <span class="text-muted">/month</span>
            </div>
            <ul class="list-unstyled text-start mb-4">
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>5 Projects</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>3 Team Members</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>5GB Storage</li>
              <li class="mb-2 text-muted"><i class="bi bi-x text-muted me-2"></i>API Access</li>
            </ul>
            <a href="#" class="btn btn-outline-primary w-100">Get Started</a>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card h-100 border-primary shadow">
          <div class="card-header bg-primary text-white text-center py-2">Most Popular</div>
          <div class="card-body text-center p-4">
            <h5 class="card-title">Pro</h5>
            <div class="my-4">
              <span class="display-5 fw-bold">$29</span>
              <span class="text-muted">/month</span>
            </div>
            <ul class="list-unstyled text-start mb-4">
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>Unlimited Projects</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>15 Team Members</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>50GB Storage</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>API Access</li>
            </ul>
            <a href="#" class="btn btn-primary w-100">Get Started</a>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card h-100">
          <div class="card-body text-center p-4">
            <h5 class="card-title">Enterprise</h5>
            <div class="my-4">
              <span class="display-5 fw-bold">$99</span>
              <span class="text-muted">/month</span>
            </div>
            <ul class="list-unstyled text-start mb-4">
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>Unlimited Everything</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>Priority Support</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>SSO / SAML</li>
              <li class="mb-2"><i class="bi bi-check text-success me-2"></i>Dedicated Account Manager</li>
            </ul>
            <a href="#" class="btn btn-outline-primary w-100">Contact Sales</a>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>
```

### Testimonials Carousel

```html
<section class="py-5">
  <div class="container">
    <h2 class="text-center mb-5">Loved by Teams Worldwide</h2>
    <div id="testimonialCarousel" class="carousel slide" data-bs-ride="carousel">
      <div class="carousel-inner">
        <div class="carousel-item active">
          <div class="row justify-content-center">
            <div class="col-lg-8 text-center">
              <blockquote class="blockquote">
                <p class="fs-4">"This platform transformed how our team ships software. We're 3x faster."</p>
              </blockquote>
              <div class="d-flex align-items-center justify-content-center mt-3">
                <img src="avatar-1.jpg" class="rounded-circle me-3" width="48" height="48" alt="Sarah Chen">
                <div class="text-start">
                  <strong>Sarah Chen</strong>
                  <div class="text-muted small">CTO at TechFlow</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <button class="carousel-control-prev" data-bs-target="#testimonialCarousel" data-bs-slide="prev">
        <span class="carousel-control-prev-icon"></span>
      </button>
      <button class="carousel-control-next" data-bs-target="#testimonialCarousel" data-bs-slide="next">
        <span class="carousel-control-next-icon"></span>
      </button>
    </div>
  </div>
</section>
```

### FAQ Accordion

```html
<section class="py-5 bg-light">
  <div class="container">
    <h2 class="text-center mb-5">Frequently Asked Questions</h2>
    <div class="row justify-content-center">
      <div class="col-lg-8">
        <div class="accordion" id="faqAccordion">
          <div class="accordion-item">
            <h3 class="accordion-header">
              <button class="accordion-button" data-bs-toggle="collapse" data-bs-target="#faq1">
                How does the free trial work?
              </button>
            </h3>
            <div id="faq1" class="accordion-collapse collapse show" data-bs-parent="#faqAccordion">
              <div class="accordion-body">You get full access to all Pro features for 14 days. No credit card required.</div>
            </div>
          </div>
          <div class="accordion-item">
            <h3 class="accordion-header">
              <button class="accordion-button collapsed" data-bs-toggle="collapse" data-bs-target="#faq2">
                Can I change plans later?
              </button>
            </h3>
            <div id="faq2" class="accordion-collapse collapse" data-bs-parent="#faqAccordion">
              <div class="accordion-body">Yes, you can upgrade or downgrade at any time. Changes take effect immediately.</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>
```

## Best Practices

1. Lead with a clear value proposition in the hero heading
2. Use a single primary CTA above the fold; secondary actions should be outline buttons
3. Show a product screenshot or demo video in the hero
4. Highlight the "Most Popular" pricing card with a border and shadow
5. Use the accordion component for FAQs to keep the page compact
6. Include trust signals: company logos, user count, testimonials
7. Provide monthly/annual pricing toggle to encourage annual plans
8. Use `btn-group` with radio inputs for the billing toggle
9. Keep feature comparison tables simple with check/cross icons
10. Include social proof with real names, titles, and company logos
11. Add "No credit card required" near trial CTAs to reduce friction

## Common Pitfalls

1. **Cluttered hero** - Too much text or too many CTAs dilutes the message. Keep the hero focused.
2. **No social proof** - Visitors need trust signals. Include testimonials, logos, or user counts.
3. **Hidden pricing** - Opaque pricing frustrates users. Show clear, comparable plans.
4. **No mobile hero optimization** - Hero images that are too large slow mobile loading. Use responsive images.
5. **Accordion items all closed** - Open the first FAQ item by default so users see the pattern.
6. **Missing trial disclaimer** - Always clarify what happens after the trial ends.

## Accessibility Considerations

- Use `<h1>` for the hero heading and maintain proper heading hierarchy
- Provide `alt` text for all product screenshots and avatar images
- Use `aria-label` on carousel controls
- Ensure pricing cards have sufficient text contrast
- Mark the FAQ accordion with `aria-labelledby` on collapse panels
- Use `aria-pressed` or `aria-selected` on the billing toggle buttons

## Responsive Behavior

On **mobile**, the hero text stacks above the image. Pricing cards stack vertically in a single column. The feature comparison table wraps horizontally with `table-responsive`. On **tablet**, the hero uses a side-by-side layout with smaller text. Pricing cards can use a 2-column grid. On **desktop**, the full side-by-side hero, 3-column pricing, and horizontal feature table display naturally.
