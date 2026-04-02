---
title: "Semantic HTML Review for Bootstrap Projects"
module: "Code Review"
difficulty: 2
estimated_time: 20
tags: ["semantic-html", "accessibility", "markup", "landmarks"]
prerequisites: ["HTML5 semantics", "Bootstrap 5 structure"]
---

## Overview

Semantic HTML provides meaning to page structure beyond visual presentation. In Bootstrap projects, the abundance of utility classes can tempt developers into "div soup" - using generic `<div>` elements everywhere. This guide teaches how to review Bootstrap markup for proper semantic structure, landmark elements, and meaningful document hierarchy that benefits accessibility, SEO, and maintainability.

## Basic Implementation

**Landmark Elements**

Replace generic `<div>` wrappers with semantic landmark elements that assistive technologies use for navigation.

```html
<!-- INCORRECT: Div soup -->
<div class="container">
  <div class="row">
    <div class="col">
      <div class="navbar">...</div>
    </div>
  </div>
  <div class="row">
    <div class="col-8">...</div>
    <div class="col-4">...</div>
  </div>
  <div class="row">
    <div class="col">...</div>
  </div>
</div>

<!-- CORRECT: Semantic landmarks -->
<header class="container">
  <nav class="navbar">...</nav>
</header>
<main class="container">
  <div class="row">
    <article class="col-8">...</article>
    <aside class="col-4">...</aside>
  </div>
</main>
<footer class="container">...</footer>
```

**Heading Hierarchy**

Verify that headings follow a logical h1-h6 sequence without skipping levels.

```html
<!-- CORRECT: Sequential heading hierarchy -->
<h1 class="mb-4">Product Catalog</h1>
<section>
  <h2 class="h4">Featured Items</h2>
  <div class="row">
    <div class="col-md-4">
      <h3 class="h6">Product Name</h3>
    </div>
  </div>
</section>
```

**List Semantics**

Use `<ul>`/`<ol>` for navigation and grouped items instead of styled `<div>` collections.

```html
<!-- INCORRECT: Div-based list -->
<div class="d-flex flex-column gap-2">
  <div class="list-item">First</div>
  <div class="list-item">Second</div>
</div>

<!-- CORRECT: Semantic list -->
<ul class="list-unstyled d-flex flex-column gap-2">
  <li>First</li>
  <li>Second</li>
</ul>
```

## Advanced Variations

**Sectioning with `<section>` and `<article>`**

Distinguish between thematic groupings and self-contained content units.

```html
<main class="container py-5">
  <section aria-labelledby="blog-heading">
    <h2 id="blog-heading">Latest Posts</h2>
    <article class="card mb-3">
      <div class="card-body">
        <h3 class="card-title h5">Post Title</h3>
        <p class="card-text">Excerpt text...</p>
      </div>
    </article>
    <article class="card mb-3">
      <div class="card-body">
        <h3 class="card-title h5">Another Post</h3>
        <p class="card-text">Excerpt text...</p>
      </div>
    </article>
  </section>
</main>
```

**Figure Elements for Media**

Use `<figure>` and `<figcaption>` for images with captions instead of `<div>` wrappers.

```html
<!-- Semantic image with caption -->
<figure class="figure text-center">
  <img src="chart.png" class="figure-img img-fluid rounded" alt="Q3 revenue chart showing 15% growth">
  <figcaption class="figure-caption text-center">Q3 2024 Revenue Growth</figcaption>
</figure>
```

**Form Structure Review**

Verify forms use `<fieldset>` and `<legend>` for grouped inputs.

```html
<form class="needs-validation" novalidate>
  <fieldset class="mb-4">
    <legend class="fs-6 fw-bold">Shipping Address</legend>
    <div class="row g-3">
      <div class="col-12">
        <label for="street" class="form-label">Street</label>
        <input type="text" class="form-control" id="street" required>
      </div>
    </div>
  </fieldset>
  <fieldset class="mb-4">
    <legend class="fs-6 fw-bold">Payment Method</legend>
    <div class="form-check">
      <input class="form-check-input" type="radio" name="payment" id="credit">
      <label class="form-check-label" for="credit">Credit Card</label>
    </div>
  </fieldset>
</form>
```

## Best Practices

1. **Use `<header>`, `<main>`, `<nav>`, `<aside>`, `<footer>`** for page-level landmarks
2. **Wrap `<nav>` with `aria-label`** when multiple navigation elements exist on a page
3. **Maintain sequential heading hierarchy** - never skip from h2 to h4
4. **Use `<button>` for actions and `<a>` for navigation** - never mix their purposes
5. **Apply `<section>` with `aria-labelledby`** for distinct content areas
6. **Use `<figure>`/`<figcaption>`** for images, charts, and code blocks with captions
7. **Prefer semantic list elements** (`<ul>`, `<ol>`, `<dl>`) for grouped content
8. **Use `<blockquote>` with `<cite>`** for quotations instead of styled `<div>` elements
9. **Wrap form groups in `<fieldset>`/`<legend>`** for related input collections
10. **Use `<time datetime>`** for date/time values to enable machine readability
11. **Avoid `<div>` for interactive elements** - use `<button>`, `<a>`, or `<input>`
12. **Use `<mark>` for highlighted text** instead of `<span class="bg-warning">`

## Common Pitfalls

1. **Using `<div>` for navigation** - Screen readers cannot identify the element as a nav landmark
2. **Non-sequential heading order** - Jumping from h1 to h3 breaks document outline expectations
3. **Using `<span>` or `<div>` as buttons** - Missing keyboard support, focus management, and ARIA roles
4. **Wrapping `<a>` around block elements** in HTML5 - While valid, it creates confusing semantics
5. **Missing `<label>` associations** - Inputs without labels are inaccessible to screen readers
6. **Using `<b>` and `<i>` for semantic meaning** - Prefer `<strong>` and `<em>` for emphasis
7. **Placing `<section>` without headings** - Sections should always contain a heading child
8. **Overusing `<main>`** - Only one `<main>` element should exist per page
9. **Using `<article>` for non-independent content** - Articles should make sense standalone
10. **Forgetting `<caption>` on data tables** - Tables need captions or `aria-label` for context

## Accessibility Considerations

Semantic elements create the accessibility tree that screen readers navigate. Landmark elements (`<nav>`, `<main>`, `<aside>`) allow users to jump between page sections. Heading hierarchy enables document outline navigation. Form `<fieldset>` and `<legend>` provide context for grouped inputs. Always verify that semantic choices align with the actual content meaning, not just visual structure. Run automated tools like axe-core and supplement with manual screen reader testing.

## Responsive Behavior

Semantic structure remains consistent across breakpoints. When Bootstrap responsive classes change visual layout (e.g., stacking columns on mobile), the underlying semantic structure should still make logical sense. Review that `<article>` and `<aside>` content order is meaningful when columns stack vertically. Ensure `<nav>` elements remain accessible in collapsed mobile menus and that landmark navigation is preserved in offcanvas implementations.
