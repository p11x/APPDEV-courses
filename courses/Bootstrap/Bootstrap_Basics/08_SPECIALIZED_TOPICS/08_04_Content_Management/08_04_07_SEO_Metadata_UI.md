---
title: "SEO Metadata UI"
module: "Content Management"
difficulty: 1
estimated_time: "15 min"
prerequisites: ["04_05_Forms", "04_01_Card_Component"]
---

## Overview

SEO metadata interfaces let content editors manage search engine visibility by configuring titles, descriptions, and social sharing previews. Bootstrap 5 form controls, cards for preview panels, and alert components for validation feedback create intuitive SEO management tools within CMS admin panels.

## Basic Implementation

### Meta Title and Description Fields

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">SEO Settings</h5></div>
  <div class="card-body">
    <div class="mb-4">
      <label for="metaTitle" class="form-label">
        Meta Title
        <span class="text-muted small">(50-60 characters recommended)</span>
      </label>
      <input type="text" class="form-control" id="metaTitle"
             value="Getting Started with Bootstrap 5 - Complete Guide"
             maxlength="70">
      <div class="d-flex justify-content-between mt-1">
        <small class="text-muted">Appears as the clickable headline in search results</small>
        <small class="text-success">52/60 characters</small>
      </div>
    </div>
    <div class="mb-4">
      <label for="metaDesc" class="form-label">
        Meta Description
        <span class="text-muted small">(150-160 characters recommended)</span>
      </label>
      <textarea class="form-control" id="metaDesc" rows="3" maxlength="200">Learn how to build responsive, modern websites with Bootstrap 5. This comprehensive guide covers grid system, components, utilities, and best practices.</textarea>
      <div class="d-flex justify-content-between mt-1">
        <small class="text-muted">Appears below the title in search results</small>
        <small class="text-success">148/160 characters</small>
      </div>
    </div>
    <div class="mb-4">
      <label for="metaSlug" class="form-label">URL Slug</label>
      <div class="input-group">
        <span class="input-group-text">https://example.com/blog/</span>
        <input type="text" class="form-control" id="metaSlug" value="getting-started-bootstrap-5">
      </div>
      <small class="text-muted">Use lowercase, hyphens instead of spaces</small>
    </div>
    <div class="mb-3">
      <label for="metaKeywords" class="form-label">
        Focus Keywords
        <span class="text-muted small">(comma separated)</span>
      </label>
      <input type="text" class="form-control" id="metaKeywords" value="bootstrap 5, responsive design, web development, CSS framework">
    </div>
  </div>
</div>
```

### Search Engine Preview

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Search Preview</h5></div>
  <div class="card-body">
    <div class="border rounded p-3 bg-white">
      <div class="d-flex align-items-center mb-1">
        <div class="bg-light rounded-circle me-2" style="width:24px;height:24px"></div>
        <small class="text-muted">example.com > blog</small>
      </div>
      <a href="#" class="text-decoration-none">
        <h5 class="text-primary mb-1" style="font-size:1.25rem">Getting Started with Bootstrap 5 - Complete Guide</h5>
      </a>
      <p class="text-muted small mb-0" style="line-height:1.4">Learn how to build responsive, modern websites with Bootstrap 5. This comprehensive guide covers grid system, components, utilities, and best practices.</p>
    </div>
    <div class="mt-3">
      <div class="d-flex align-items-center mb-2">
        <span class="badge bg-success me-2">Good</span>
        <small>Title length is optimal (52 characters)</small>
      </div>
      <div class="d-flex align-items-center mb-2">
        <span class="badge bg-success me-2">Good</span>
        <small>Description length is optimal (148 characters)</small>
      </div>
      <div class="d-flex align-items-center">
        <span class="badge bg-warning text-dark me-2">Warning</span>
        <small>Consider adding your primary keyword earlier in the title</small>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Open Graph Settings

```html
<div class="card mt-4">
  <div class="card-header bg-white"><h5 class="mb-0">Social Sharing (Open Graph)</h5></div>
  <div class="card-body">
    <div class="row g-4">
      <div class="col-md-6">
        <label class="form-label">OG Title</label>
        <input type="text" class="form-control" value="Getting Started with Bootstrap 5" placeholder="Social media title">
        <small class="text-muted">Title shown when shared on social media</small>
      </div>
      <div class="col-md-6">
        <label class="form-label">OG Description</label>
        <textarea class="form-control" rows="2" placeholder="Social media description">A complete beginner's guide to building responsive websites.</textarea>
      </div>
      <div class="col-12">
        <label class="form-label">OG Image</label>
        <div class="d-flex align-items-start gap-3">
          <div class="border rounded bg-light d-flex align-items-center justify-content-center" style="width:200px;height:120px">
            <div class="text-center text-muted">
              <i class="bi bi-image fs-3 d-block"></i>
              <small>No image</small>
            </div>
          </div>
          <div>
            <button class="btn btn-outline-primary btn-sm">Choose Image</button>
            <p class="text-muted small mt-2 mb-0">Recommended: 1200x630px for optimal display on Facebook and Twitter.</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Social Preview Card -->
    <div class="mt-4">
      <label class="form-label fw-semibold">Facebook Preview</label>
      <div class="border rounded overflow-hidden" style="max-width:500px">
        <div class="bg-light d-flex align-items-center justify-content-center" style="height:200px">
          <i class="bi bi-image text-muted display-4"></i>
        </div>
        <div class="p-3 bg-light border-top">
          <div class="text-uppercase small text-muted mb-1">example.com</div>
          <div class="fw-semibold">Getting Started with Bootstrap 5</div>
          <div class="small text-muted">A complete beginner's guide to building responsive websites.</div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Show character counters on meta title (50-60) and description (150-160)
2. Provide a live search engine preview that updates as users type
3. Validate meta fields and show warnings for suboptimal lengths
4. Include Open Graph title, description, and image fields
5. Show social media preview cards (Facebook, Twitter) that match actual rendering
6. Recommend OG image dimensions (1200x630px)
7. Auto-generate slug from the page title
8. Include focus keyword input for SEO analysis
9. Use color-coded badges for validation status (green=good, yellow=warning, red=error)
10. Show the canonical URL for the page

## Common Pitfalls

1. **No character limits** - Titles and descriptions get truncated in search results. Enforce limits with counters.
2. **No search preview** - Editors can't visualize how their page appears in Google results.
3. **Missing OG image** - Social shares without images get poor engagement. Always prompt for an image.
4. **Slug not auto-generated** - Manually creating slugs leads to inconsistency. Auto-generate from the title.
5. **No validation feedback** - Editors don't know if their SEO is good. Provide real-time feedback.
6. **Duplicate meta descriptions** - Using the same description across pages hurts SEO. Flag duplicates.

## Accessibility Considerations

- Associate character counters with their inputs using `aria-describedby`
- Use `aria-live="polite"` for real-time character count updates
- Label all SEO fields with clear, descriptive labels
- Mark the search preview with `aria-label="Search engine results preview"`

## Responsive Behavior

SEO forms stack vertically on **mobile** with full-width inputs. The search preview remains centered. Social preview cards scale to fit the container. On **tablet and desktop**, OG settings use a 2-column layout with side-by-side fields.
