---
title: "Carousel"
course: "Bootstrap Basics"
module: "02_COMPONENT_SYSTEM"
lesson: "02_06_Media_Components"
file: "02_06_06_Carousel"
difficulty: 2
framework_version: "Bootstrap 5.3"
tags: [carousel, slider, slideshow, crossfade, indicators, controls, autoplay, interval]
prerequisites:
  - "02_01_Buttons"
  - "02_06_01_Responsive_Images"
description: "Build accessible, responsive carousels with indicators, controls, captions, crossfade transitions, autoplay behavior, and interval configuration."
---

## Overview

The Bootstrap carousel is a slideshow component for cycling through image slides, content panels, or mixed media. It includes previous/next navigation controls, slide indicators, optional captions, and supports both slide and crossfade transitions. The carousel is fully keyboard-navigable and screen-reader friendly when implemented with proper ARIA attributes.

Key components include `.carousel` (the wrapper), `.carousel-inner` (slide container), `.carousel-item` (individual slides), `.carousel-indicators` (dot navigation), `.carousel-control-prev` / `.carousel-control-next` (arrow buttons), and `.carousel-caption` (overlay text). Data attributes control behavior like interval timing, pause-on-hover, and cycling.

## Basic Implementation

A minimal carousel with three image slides:

```html
<div id="heroCarousel" class="carousel slide" data-bs-ride="carousel">
  <div class="carousel-inner">
    <div class="carousel-item active">
      <img src="images/slide-1.jpg" class="d-block w-100" alt="First slide">
    </div>
    <div class="carousel-item">
      <img src="images/slide-2.jpg" class="d-block w-100" alt="Second slide">
    </div>
    <div class="carousel-item">
      <img src="images/slide-3.jpg" class="d-block w-100" alt="Third slide">
    </div>
  </div>
  <button class="carousel-control-prev" type="button" data-bs-target="#heroCarousel" data-bs-slide="prev">
    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Previous</span>
  </button>
  <button class="carousel-control-next" type="button" data-bs-target="#heroCarousel" data-bs-slide="next">
    <span class="carousel-control-next-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Next</span>
  </button>
</div>
```

## Advanced Variations

Add slide indicators for direct slide access:

```html
<div id="carouselWithIndicators" class="carousel slide" data-bs-ride="carousel">
  <div class="carousel-indicators">
    <button type="button" data-bs-target="#carouselWithIndicators" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
    <button type="button" data-bs-target="#carouselWithIndicators" data-bs-slide-to="1" aria-label="Slide 2"></button>
    <button type="button" data-bs-target="#carouselWithIndicators" data-bs-slide-to="2" aria-label="Slide 3"></button>
  </div>
  <div class="carousel-inner">
    <div class="carousel-item active">
      <img src="images/slide-1.jpg" class="d-block w-100" alt="First slide">
      <div class="carousel-caption d-none d-md-block">
        <h5>Welcome to Our Platform</h5>
        <p>Discover powerful tools designed for modern teams.</p>
      </div>
    </div>
    <div class="carousel-item">
      <img src="images/slide-2.jpg" class="d-block w-100" alt="Second slide">
      <div class="carousel-caption d-none d-md-block">
        <h5>Real-Time Collaboration</h5>
        <p>Work together seamlessly across time zones.</p>
      </div>
    </div>
    <div class="carousel-item">
      <img src="images/slide-3.jpg" class="d-block w-100" alt="Third slide">
      <div class="carousel-caption d-none d-md-block">
        <h5>Enterprise Security</h5>
        <p>Bank-grade encryption protects your data.</p>
      </div>
    </div>
  </div>
  <button class="carousel-control-prev" type="button" data-bs-target="#carouselWithIndicators" data-bs-slide="prev">
    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Previous</span>
  </button>
  <button class="carousel-control-next" type="button" data-bs-target="#carouselWithIndicators" data-bs-slide="next">
    <span class="carousel-control-next-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Next</span>
  </button>
</div>
```

Enable crossfade transitions instead of sliding:

```html
<div id="crossfadeCarousel" class="carousel slide carousel-fade" data-bs-ride="carousel" data-bs-interval="4000">
  <div class="carousel-inner">
    <div class="carousel-item active">
      <img src="images/fade-1.jpg" class="d-block w-100" alt="Fade slide one">
    </div>
    <div class="carousel-item">
      <img src="images/fade-2.jpg" class="d-block w-100" alt="Fade slide two">
    </div>
  </div>
</div>
```

Build a dark-themed carousel with custom interval and pause-on-hover disabled:

```html
<div id="autoCarousel" class="carousel slide carousel-dark" data-bs-ride="carousel" data-bs-interval="3000" data-bs-pause="false">
  <div class="carousel-inner">
    <div class="carousel-item active" data-bs-interval="5000">
      <img src="images/dark-1.jpg" class="d-block w-100" alt="Extended slide">
    </div>
    <div class="carousel-item">
      <img src="images/dark-2.jpg" class="d-block w-100" alt="Standard slide">
    </div>
    <div class="carousel-item">
      <img src="images/dark-3.jpg" class="d-block w-100" alt="Standard slide">
    </div>
  </div>
  <button class="carousel-control-prev" type="button" data-bs-target="#autoCarousel" data-bs-slide="prev">
    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Previous</span>
  </button>
  <button class="carousel-control-next" type="button" data-bs-target="#autoCarousel" data-bs-slide="next">
    <span class="carousel-control-next-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Next</span>
  </button>
</div>
```

Use JavaScript API for programmatic control:

```html
<script>
  const myCarousel = document.querySelector('#heroCarousel');
  const carousel = new bootstrap.Carousel(myCarousel, {
    interval: 5000,
    wrap: true,
    keyboard: true
  });

  document.getElementById('pauseBtn').addEventListener('click', () => carousel.pause());
  document.getElementById('playBtn').addEventListener('click', () => carousel.cycle());
</script>
```

## Best Practices

1. **Always mark one `.carousel-item` as `active`.** Without it, no slide is visible and the carousel appears broken.
2. **Use `d-block w-100`** on all carousel images to override default inline image behavior and ensure full-width display.
3. **Include `data-bs-ride="carousel"`** to enable automatic cycling on page load.
4. **Add `aria-current="true"`** on the active indicator button and `aria-label` on all indicator buttons.
5. **Use `visually-hidden`** text inside control buttons so screen readers announce their purpose.
6. **Set `data-bs-interval`** per `.carousel-item` for varied slide durations (e.g., longer for text-heavy slides).
7. **Apply `carousel-fade`** for crossfade transitions, which feel smoother than horizontal slides.
8. **Hide captions on mobile** with `d-none d-md-block` to prevent text overlap on small images.
9. **Use `carousel-dark`** when slides have light backgrounds to ensure control visibility.
10. **Pause autoplay on `mouseenter`** by default (Bootstrap's default `data-bs-pause="hover"`) to improve usability.
11. **Set explicit heights** on carousel images or use `min-height` on `.carousel-item` to prevent layout shift.
12. **Use `object-fit: cover`** on images to maintain aspect ratio while filling the carousel area.

## Common Pitfalls

1. **Forgetting the `active` class** on the first carousel item, resulting in a blank carousel.
2. **Not using `d-block w-100`** on images, causing them to stack vertically instead of sliding.
3. **Omitting `data-bs-ride="carousel"`**, which prevents autoplay initialization.
4. **Missing `aria-label`** on control buttons and indicators, failing WCAG accessibility audits.
5. **Setting `data-bs-interval="0"`** unintentionally, which disables autoplay entirely.
6. **Using different image aspect ratios** across slides, causing the carousel height to jump between transitions.
7. **Placing carousels inside modals** without reinitializing Bootstrap JS, as the modal's DOM may not be ready.
8. **Forgetting to include Bootstrap JS** (not just CSS), leaving all `data-bs-*` attributes non-functional.

## Accessibility Considerations

Each `.carousel-control-prev` and `.carousel-control-next` button must include `<span class="visually-hidden">Previous</span>` (or "Next") so screen readers announce their function. Indicator buttons require `aria-label="Slide 1"` (and so on) plus `aria-current="true"` on the active indicator. Use `aria-roledescription="carousel"` on the `.carousel` wrapper and `aria-roledescription="slide"` on each `.carousel-item` to provide semantic context. Carousel captions should not contain essential information that is not available elsewhere on the page, as captions may be hidden on mobile. Ensure keyboard users can navigate slides using Tab to reach controls and Enter/Space to activate them. Consider providing a pause button for users with vestibular disorders.

## Responsive Behavior

The carousel is inherently responsive; images with `d-block w-100` scale to the full width of their parent container. Captions overlay the image and should be hidden on small screens (`d-none d-md-block`) to prevent text from obscuring the image. Use responsive height utilities (`min-vh-25 min-vh-md-50`) or aspect-ratio containers to control carousel height across breakpoints. On mobile, ensure touch swipe gestures work (enabled by default in Bootstrap 5). Test that indicator dots and control arrows remain tappable (minimum 44x44px touch target) on small screens.
