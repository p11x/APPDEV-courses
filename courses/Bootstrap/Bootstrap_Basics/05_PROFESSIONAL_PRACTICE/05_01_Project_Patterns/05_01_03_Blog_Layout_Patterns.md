---
title: Blog Layout Patterns
category: Professional Practice
difficulty: 2
time: 50 min
tags: bootstrap5, blog, cards, sidebar, grid, layout, responsive
---

## Overview

Blog layouts require flexible card grids, sidebar widgets, and well-structured single post pages. Bootstrap 5's grid system, card component, and utility classes provide everything needed to build a professional blog with category filtering, author cards, and tag pages.

## Basic Implementation

### Blog Post Card Grid

Use `card` components inside a responsive `row` to create the main post feed.

```html
<div class="container py-5">
  <div class="row g-4">
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 border-0 shadow-sm">
        <img src="post-thumb.jpg" class="card-img-top" alt="Post thumbnail">
        <div class="card-body d-flex flex-column">
          <span class="badge bg-primary mb-2 align-self-start">Bootstrap</span>
          <h5 class="card-title">Getting Started with Bootstrap 5</h5>
          <p class="card-text text-muted flex-grow-1">Learn the fundamentals of Bootstrap 5 grid system, utilities, and components.</p>
          <div class="d-flex align-items-center mt-3">
            <img src="author.jpg" alt="Author" class="rounded-circle me-2" width="32" height="32">
            <small class="text-muted">John Doe &middot; Mar 15, 2026</small>
          </div>
        </div>
        <div class="card-footer bg-transparent border-0">
          <a href="#" class="btn btn-outline-primary btn-sm">Read More</a>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 border-0 shadow-sm">
        <img src="post-thumb-2.jpg" class="card-img-top" alt="Post thumbnail">
        <div class="card-body d-flex flex-column">
          <span class="badge bg-success mb-2 align-self-start">CSS</span>
          <h5 class="card-title">Mastering Bootstrap Utilities</h5>
          <p class="card-text text-muted flex-grow-1">Deep dive into spacing, display, flex, and color utility classes.</p>
          <div class="d-flex align-items-center mt-3">
            <img src="author-2.jpg" alt="Author" class="rounded-circle me-2" width="32" height="32">
            <small class="text-muted">Jane Smith &middot; Mar 12, 2026</small>
          </div>
        </div>
        <div class="card-footer bg-transparent border-0">
          <a href="#" class="btn btn-outline-primary btn-sm">Read More</a>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 border-0 shadow-sm">
        <img src="post-thumb-3.jpg" class="card-img-top" alt="Post thumbnail">
        <div class="card-body d-flex flex-column">
          <span class="badge bg-warning text-dark mb-2 align-self-start">JavaScript</span>
          <h5 class="card-title">Bootstrap JavaScript Components</h5>
          <p class="card-text text-muted flex-grow-1">Explore modals, dropdowns, tooltips, and other interactive components.</p>
          <div class="d-flex align-items-center mt-3">
            <img src="author-3.jpg" alt="Author" class="rounded-circle me-2" width="32" height="32">
            <small class="text-muted">Alex Lee &middot; Mar 10, 2026</small>
          </div>
        </div>
        <div class="card-footer bg-transparent border-0">
          <a href="#" class="btn btn-outline-primary btn-sm">Read More</a>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Sidebar with Widgets

Position a sidebar alongside the main content using `col-lg-8` and `col-lg-4`.

```html
<div class="container py-5">
  <div class="row g-4">
    <main class="col-lg-8">
      <!-- Blog post cards go here -->
    </main>
    <aside class="col-lg-4">
      <div class="card border-0 shadow-sm mb-4">
        <div class="card-body">
          <h5 class="card-title">Search</h5>
          <div class="input-group">
            <input type="search" class="form-control" placeholder="Search posts...">
            <button class="btn btn-primary"><i class="bi bi-search"></i></button>
          </div>
        </div>
      </div>
      <div class="card border-0 shadow-sm mb-4">
        <div class="card-body">
          <h5 class="card-title">Categories</h5>
          <ul class="list-group list-group-flush">
            <li class="list-group-item d-flex justify-content-between align-items-center border-0 px-0">
              Bootstrap <span class="badge bg-primary rounded-pill">12</span>
            </li>
            <li class="list-group-item d-flex justify-content-between align-items-center border-0 px-0">
              CSS <span class="badge bg-primary rounded-pill">8</span>
            </li>
            <li class="list-group-item d-flex justify-content-between align-items-center border-0 px-0">
              JavaScript <span class="badge bg-primary rounded-pill">15</span>
            </li>
          </ul>
        </div>
      </div>
      <div class="card border-0 shadow-sm">
        <div class="card-body">
          <h5 class="card-title">Recent Posts</h5>
          <div class="d-flex mb-3">
            <img src="recent-1.jpg" alt="" class="rounded me-3" width="64" height="64" style="object-fit:cover">
            <div>
              <a href="#" class="text-decoration-none fw-semibold">Responsive Design Tips</a>
              <p class="text-muted small mb-0">Mar 8, 2026</p>
            </div>
          </div>
          <div class="d-flex">
            <img src="recent-2.jpg" alt="" class="rounded me-3" width="64" height="64" style="object-fit:cover">
            <div>
              <a href="#" class="text-decoration-none fw-semibold">Customizing Bootstrap</a>
              <p class="text-muted small mb-0">Mar 5, 2026</p>
            </div>
          </div>
        </div>
      </div>
    </aside>
  </div>
</div>
```

### Single Post Layout

```html
<article class="container py-5">
  <div class="row justify-content-center">
    <div class="col-lg-8">
      <span class="badge bg-primary mb-2">Bootstrap</span>
      <h1 class="display-5 fw-bold">Getting Started with Bootstrap 5</h1>
      <div class="d-flex align-items-center mb-4 text-muted">
        <img src="author.jpg" alt="John Doe" class="rounded-circle me-2" width="40" height="40">
        <span>John Doe</span>
        <span class="mx-2">&middot;</span>
        <span>Mar 15, 2026</span>
        <span class="mx-2">&middot;</span>
        <span>8 min read</span>
      </div>
      <img src="post-hero.jpg" alt="Bootstrap 5 overview" class="img-fluid rounded mb-4">
      <div class="lead mb-4">
        Bootstrap 5 is the latest major release of the world's most popular CSS framework.
      </div>
      <p>Bootstrap 5 drops jQuery as a dependency, introduces new utility classes, and adds RTL support...</p>
      <hr class="my-5">
      <h4>Author</h4>
      <div class="d-flex align-items-center">
        <img src="author.jpg" alt="John Doe" class="rounded-circle me-3" width="56" height="56">
        <div>
          <h6 class="mb-0">John Doe</h6>
          <p class="text-muted mb-0 small">Front-end developer and Bootstrap enthusiast with 10 years of experience.</p>
        </div>
      </div>
    </div>
  </div>
</article>
```

## Advanced Variations

- **Masonry Layout:** Use CSS `columns` or the Masonry.js library to create a Pinterest-style card grid.
- **Reading Progress Bar:** Add a fixed-top `<div>` whose width increases via JavaScript as the user scrolls.
- **Infinite Scroll:** Detect scroll position with `IntersectionObserver` and append new post cards via fetch.
- **Tag Cloud Sidebar:** Render tags as pill badges with varying font sizes based on post count.
- **Dark Mode Blog:** Apply `data-bs-theme="dark"` and adjust card `bg-*` and `text-*` classes.

## Best Practices

1. Use `h-100` on every card to equalize heights in a grid row.
2. Apply `flex-grow-1` on the card body paragraph so the footer aligns at the bottom.
3. Use `shadow-sm` with `border-0` for a clean card aesthetic.
4. Keep the sidebar on `col-lg-4` and stack below main content on smaller screens.
5. Use `object-fit: cover` with fixed `width`/`height` on sidebar thumbnails.
6. Apply `list-group-flush` with `border-0` and `px-0` for clean sidebar lists.
7. Use `rounded-pill` badges for category counts.
8. Limit excerpt text to 2-3 lines with CSS `line-clamp` or manual truncation.
9. Use semantic `<article>` and `<aside>` tags for blog posts and sidebars.
10. Apply `img-fluid` on all post images to prevent horizontal overflow.
11. Use `text-decoration-none` on card links to remove underlines by default.
12. Place the search widget first in the sidebar for immediate discoverability.

## Common Pitfalls

1. **Unequal card heights:** Without `h-100`, cards in the same row display at different heights.
2. **Missing `object-fit` on thumbnails:** Sidebar images stretch or squish without it.
3. **Too many sidebar widgets:** Three to five widgets is the sweet spot; more creates clutter.
4. **No mobile sidebar ordering:** Use `order-lg-2` on the sidebar to move it below content on mobile.
5. **Inaccessible images:** Every `<img>` needs a meaningful `alt` attribute, even decorative ones.
6. **Fixed excerpt length without CSS:** Use `overflow:hidden` and `text-overflow:ellipsis` for safe truncation.
7. **Over-nesting grid columns:** Keep the structure flat; avoid more than two levels of `.row` nesting.

## Accessibility Considerations

- Use `<article>` for each post card and `<aside>` for the sidebar.
- Add `aria-label="Search posts"` to the search input.
- Apply `role="navigation"` to category link groups.
- Ensure the single post `<h1>` is the only `<h1>` on the page.
- Add `aria-describedby` to read-more buttons referencing the post title.

## Responsive Behavior

| Breakpoint | Post Grid | Sidebar | Single Post |
|------------|-----------|---------|-------------|
| `<576px` | 1 column | Below content | Full width |
| `≥576px` | 2 columns | Below content | Full width |
| `≥768px` | 2 columns | Below content | Full width |
| `≥992px` | 3 columns | 4-col sidebar | 8-col centered |
| `≥1200px` | 3 columns | 4-col sidebar | 8-col centered |
