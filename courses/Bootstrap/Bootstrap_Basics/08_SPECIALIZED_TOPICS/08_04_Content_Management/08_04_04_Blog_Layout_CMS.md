---
title: "Blog Layout CMS"
module: "Content Management"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["02_01_Grid_System", "04_01_Card_Component", "05_03_Pagination"]
---

## Overview

Blog layouts within a CMS present articles in organized, browsable formats with sidebar widgets, author pages, and category/tag navigation. Bootstrap 5 cards for article grids, sidebar layout with sticky positioning, badge components for tags, and pagination for article lists create professional blog interfaces.

## Basic Implementation

### Article Grid

```html
<div class="container py-5">
  <div class="row">
    <!-- Main Content -->
    <div class="col-lg-8">
      <h2 class="mb-4">Latest Articles</h2>
      <div class="row g-4">
        <!-- Featured Article -->
        <div class="col-12">
          <div class="card overflow-hidden">
            <div class="row g-0">
              <div class="col-md-5">
                <img src="blog-featured.jpg" class="img-fluid h-100" alt="Featured article" style="object-fit:cover">
              </div>
              <div class="col-md-7">
                <div class="card-body d-flex flex-column h-100">
                  <span class="badge bg-primary mb-2 align-self-start">Featured</span>
                  <h4 class="card-title">Building Accessible Web Applications with Bootstrap 5</h4>
                  <p class="card-text text-muted">Learn how to create inclusive web experiences using Bootstrap 5's built-in accessibility features and ARIA support.</p>
                  <div class="mt-auto d-flex align-items-center">
                    <img src="author-1.jpg" class="rounded-circle me-2" width="32" height="32" alt="Author">
                    <div>
                      <small class="fw-semibold d-block">John Doe</small>
                      <small class="text-muted">Mar 15, 2024 &bull; 8 min read</small>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Article Cards -->
        <div class="col-md-6">
          <div class="card h-100">
            <img src="blog-1.jpg" class="card-img-top" alt="Article thumbnail" loading="lazy" style="height:180px;object-fit:cover">
            <div class="card-body d-flex flex-column">
              <div class="mb-2">
                <span class="badge bg-light text-dark">Tutorial</span>
              </div>
              <h5 class="card-title">Getting Started with Bootstrap Grid</h5>
              <p class="card-text text-muted small flex-grow-1">A comprehensive guide to Bootstrap's 12-column grid system and responsive breakpoints.</p>
              <div class="d-flex align-items-center mt-3">
                <img src="author-2.jpg" class="rounded-circle me-2" width="28" height="28" alt="Author">
                <small class="text-muted">Alice Smith &bull; Mar 12, 2024</small>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="card h-100">
            <img src="blog-2.jpg" class="card-img-top" alt="Article thumbnail" loading="lazy" style="height:180px;object-fit:cover">
            <div class="card-body d-flex flex-column">
              <div class="mb-2">
                <span class="badge bg-light text-dark">Best Practices</span>
              </div>
              <h5 class="card-title">10 CSS Tips for Better UI Design</h5>
              <p class="card-text text-muted small flex-grow-1">Practical CSS techniques to improve your user interface design skills.</p>
              <div class="d-flex align-items-center mt-3">
                <img src="author-3.jpg" class="rounded-circle me-2" width="28" height="28" alt="Author">
                <small class="text-muted">Bob Johnson &bull; Mar 10, 2024</small>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Pagination -->
      <nav class="mt-5 d-flex justify-content-center" aria-label="Blog pagination">
        <ul class="pagination">
          <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
          <li class="page-item active"><a class="page-link" href="#">1</a></li>
          <li class="page-item"><a class="page-link" href="#">2</a></li>
          <li class="page-item"><a class="page-link" href="#">3</a></li>
          <li class="page-item"><a class="page-link" href="#">Next</a></li>
        </ul>
      </nav>
    </div>

    <!-- Sidebar -->
    <div class="col-lg-4">
      <div class="sticky-top" style="top:20px">
        <!-- Search -->
        <div class="card mb-4">
          <div class="card-body">
            <div class="input-group">
              <input type="search" class="form-control" placeholder="Search articles...">
              <button class="btn btn-primary"><i class="bi bi-search"></i></button>
            </div>
          </div>
        </div>

        <!-- Categories -->
        <div class="card mb-4">
          <div class="card-header bg-white"><h6 class="mb-0">Categories</h6></div>
          <ul class="list-group list-group-flush">
            <li class="list-group-item d-flex justify-content-between">
              <a href="#" class="text-decoration-none">Tutorials</a>
              <span class="badge bg-secondary rounded-pill">12</span>
            </li>
            <li class="list-group-item d-flex justify-content-between">
              <a href="#" class="text-decoration-none">Best Practices</a>
              <span class="badge bg-secondary rounded-pill">8</span>
            </li>
            <li class="list-group-item d-flex justify-content-between">
              <a href="#" class="text-decoration-none">News</a>
              <span class="badge bg-secondary rounded-pill">15</span>
            </li>
          </ul>
        </div>

        <!-- Tags -->
        <div class="card mb-4">
          <div class="card-header bg-white"><h6 class="mb-0">Tags</h6></div>
          <div class="card-body">
            <a href="#" class="badge bg-light text-dark text-decoration-none me-1 mb-1 py-2 px-3">Bootstrap</a>
            <a href="#" class="badge bg-light text-dark text-decoration-none me-1 mb-1 py-2 px-3">CSS</a>
            <a href="#" class="badge bg-light text-dark text-decoration-none me-1 mb-1 py-2 px-3">JavaScript</a>
            <a href="#" class="badge bg-light text-dark text-decoration-none me-1 mb-1 py-2 px-3">Accessibility</a>
            <a href="#" class="badge bg-light text-dark text-decoration-none me-1 mb-1 py-2 px-3">Responsive</a>
          </div>
        </div>

        <!-- Newsletter -->
        <div class="card bg-primary text-white">
          <div class="card-body">
            <h6>Subscribe to Newsletter</h6>
            <p class="small opacity-75">Get the latest articles delivered to your inbox.</p>
            <div class="input-group">
              <input type="email" class="form-control" placeholder="your@email.com">
              <button class="btn btn-light">Subscribe</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use a featured article with a larger card layout at the top
2. Maintain a consistent card structure: thumbnail, category badge, title, excerpt, author
3. Use `sticky-top` on the sidebar to keep widgets visible while scrolling
4. Provide category counts with badge pills
5. Include a search widget in the sidebar
6. Show author avatars with each article card
7. Use `loading="lazy"` on article thumbnails
8. Display "X min read" estimates for articles
9. Include a newsletter signup in the sidebar
10. Use tag clouds with `badge bg-light text-dark` styling

## Common Pitfalls

1. **No featured article** - All articles look the same. Highlight important content.
2. **Sidebar not sticky** - Users lose context when scrolling past content.
3. **Missing author attribution** - Readers want to know who wrote the article.
4. **No reading time** - Users decide whether to read based on length. Show estimates.
5. **Tags not clickable** - Tags should link to filtered article lists.
6. **Pagination only at bottom** - Include pagination at the top too for long lists.

## Accessibility Considerations

- Use `aria-label="Featured article"` on the featured card
- Mark the sidebar with `role="complementary" aria-label="Blog sidebar"`
- Label the pagination nav with `aria-label="Blog pagination"`
- Use `aria-current="page"` on the active pagination item
- Provide descriptive alt text on all article images

## Responsive Behavior

On **mobile**, articles stack in a single column. The sidebar moves below the main content. On **tablet**, articles display in a 2-column grid. On **desktop**, the full 8+4 column layout with sticky sidebar works.
