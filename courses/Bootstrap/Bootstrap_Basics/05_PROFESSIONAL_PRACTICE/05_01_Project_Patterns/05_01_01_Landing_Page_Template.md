---
title: Landing Page Template
category: Professional Practice
difficulty: 2
time: 45 min
tags: bootstrap5, landing-page, hero, cta, grid, responsive
---

## Overview

A landing page is a single-page marketing site designed to convert visitors into leads or customers. Bootstrap 5 provides all the building blocks needed: responsive grid, navbar, cards, buttons, and utility classes. This lesson covers a professional landing page pattern with a hero section, features grid, call-to-action (CTA), and footer.

## Basic Implementation

### Hero Section with Background and CTA

The hero section is the first thing users see. Use Bootstrap's container, grid, and button components to create a high-impact hero.

```html
<section class="bg-primary text-white py-5">
  <div class="container">
    <div class="row align-items-center min-vh-75">
      <div class="col-lg-6">
        <h1 class="display-3 fw-bold">Build Faster with Bootstrap 5</h1>
        <p class="lead my-4">
          Create responsive, mobile-first projects with the world's most popular front-end framework.
        </p>
        <a href="#features" class="btn btn-light btn-lg me-2">Get Started</a>
        <a href="#pricing" class="btn btn-outline-light btn-lg">View Pricing</a>
      </div>
      <div class="col-lg-6 text-center">
        <img src="hero-image.svg" alt="Dashboard preview" class="img-fluid">
      </div>
    </div>
  </div>
</section>
```

### Features Grid

Use the card component inside a responsive grid to display feature highlights.

```html
<section id="features" class="py-5">
  <div class="container">
    <div class="text-center mb-5">
      <h2 class="fw-bold">Why Choose Us</h2>
      <p class="text-muted">Everything you need to build modern web applications</p>
    </div>
    <div class="row g-4">
      <div class="col-md-4">
        <div class="card h-100 border-0 shadow-sm text-center p-4">
          <div class="card-body">
            <div class="display-4 text-primary mb-3"><i class="bi bi-lightning-charge"></i></div>
            <h5 class="card-title">Lightning Fast</h5>
            <p class="card-text text-muted">Optimized build process delivers minimal CSS and JS output.</p>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card h-100 border-0 shadow-sm text-center p-4">
          <div class="card-body">
            <div class="display-4 text-primary mb-3"><i class="bi bi-phone"></i></div>
            <h5 class="card-title">Responsive</h5>
            <p class="card-text text-muted">Built mobile-first with six breakpoint tiers.</p>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card h-100 border-0 shadow-sm text-center p-4">
          <div class="card-body">
            <div class="display-4 text-primary mb-3"><i class="bi bi-shield-check"></i></div>
            <h5 class="card-title">Secure</h5>
            <p class="card-text text-muted">Enterprise-grade security for every project.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>
```

### CTA and Footer

```html
<section class="bg-dark text-white py-5 text-center">
  <div class="container">
    <h2 class="fw-bold mb-3">Ready to Get Started?</h2>
    <p class="lead mb-4">Join thousands of developers building with Bootstrap 5.</p>
    <a href="#" class="btn btn-primary btn-lg px-5">Sign Up Free</a>
  </div>
</section>

<footer class="bg-light py-4">
  <div class="container">
    <div class="row">
      <div class="col-md-6 text-center text-md-start">
        <p class="mb-0 text-muted">&copy; 2026 Your Company. All rights reserved.</p>
      </div>
      <div class="col-md-6 text-center text-md-end">
        <a href="#" class="text-muted text-decoration-none me-3">Privacy</a>
        <a href="#" class="text-muted text-decoration-none me-3">Terms</a>
        <a href="#" class="text-muted text-decoration-none">Contact</a>
      </div>
    </div>
  </div>
</footer>
```

## Advanced Variations

- **Video Background Hero:** Replace the hero image with an embedded `<video>` or use a CSS background video with `object-fit: cover`.
- **Parallax Sections:** Use `background-attachment: fixed` on section backgrounds for a parallax scroll effect.
- **Testimonials Carousel:** Insert Bootstrap's carousel component between features and CTA to display rotating client quotes.
- **Countdown Timer:** Add a JavaScript-powered countdown in the hero for product launches or events.
- **Dark Mode Toggle:** Use `data-bs-theme="dark"` on `<html>` and Bootstrap 5.3's built-in dark mode support.

## Best Practices

1. Use `container` or `container-fluid` consistently to maintain readable content width.
2. Apply `g-4` or `g-5` gutter classes on `.row` for consistent spacing between columns.
3. Use `align-items-center` with `min-vh-*` classes to vertically center hero content.
4. Keep the hero copy to one headline, one subheadline, and two CTA buttons maximum.
5. Use `img-fluid` on all images to prevent overflow on small screens.
6. Apply `fw-bold` and `display-*` utilities for hero typography instead of custom CSS.
7. Limit feature cards to 3-6 items to avoid overwhelming visitors.
8. Use `text-center` on parent containers to center content across breakpoints.
9. Apply `shadow-sm` to cards for subtle depth without heavy visual weight.
10. Place the primary CTA above the fold (visible without scrolling).
11. Use semantic `<section>` tags with unique `id` attributes for anchor navigation.
12. Leverage Bootstrap Icons for lightweight, scalable iconography.

## Common Pitfalls

1. **Too many hero CTAs:** Avoid more than two buttons in the hero; it reduces conversion clarity.
2. **Large hero images:** Unoptimized images delay page load; always compress and use `srcset`.
3. **Ignoring mobile breakpoint:** Test at `col-sm-6` and below; desktop-only layouts break on phones.
4. **Overusing shadows:** Stacking `shadow-lg` on every card creates visual noise.
5. **Missing alt text:** Every `<img>` requires descriptive `alt` for accessibility.
6. **Fixed pixel widths:** Never hardcode `width` in pixels; use Bootstrap grid percentages.
7. **Inconsistent padding:** Mixing `py-3`, `py-5`, and `mt-4` randomly creates uneven rhythm.

## Accessibility Considerations

- Add `role="banner"` to the hero `<section>` and `role="contentinfo"` to the `<footer>`.
- Ensure a skip navigation link (`<a class="visually-hidden-focusable" href="#main-content">Skip to content</a>`) is placed before the navbar.
- Maintain a minimum contrast ratio of 4.5:1 for all text against background colors.
- Use `<h1>` once in the hero, then sequential `<h2>`, `<h3>` for proper heading hierarchy.
- Add `aria-label` to icon-only buttons and links.

## Responsive Behavior

| Breakpoint | Hero Layout | Features Grid | Footer |
|------------|-------------|---------------|--------|
| `<576px` | Stacked | 1 column | Stacked |
| `≥576px` | Stacked | 2 columns | Stacked |
| `≥768px` | 2-column | 3 columns | Side-by-side |
| `≥992px` | 2-column | 3 columns | Side-by-side |
| `≥1200px` | 2-column | 3 columns | Side-by-side |

Use `order-lg-2` on the hero image column to swap visual order on desktop, and `d-none d-lg-block` to hide decorative elements on mobile.
