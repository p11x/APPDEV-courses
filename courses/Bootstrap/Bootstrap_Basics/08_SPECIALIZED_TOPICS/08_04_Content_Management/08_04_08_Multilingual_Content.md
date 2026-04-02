---
title: "Multilingual Content"
module: "Content Management"
difficulty: 3
estimated_time: "35 min"
prerequisites: ["04_05_Forms", "04_06_Nav_And_Tabs", "04_09_Badges"]
---

## Overview

Multilingual content interfaces manage translations across multiple languages and locales. Bootstrap 5 tabs for language switching, form controls for translation fields, badges for translation status, and dropdowns for locale selection create comprehensive internationalization management tools for CMS platforms.

## Basic Implementation

### Language Switcher Tabs

```html
<div class="card">
  <div class="card-header bg-white">
    <ul class="nav nav-tabs card-header-tabs">
      <li class="nav-item">
        <a class="nav-link active d-flex align-items-center gap-2" href="#">
          <span class="fi fi-us"></span> English
          <span class="badge bg-success">Complete</span>
        </a>
      </li>
      <li class="nav-item">
        <a class="nav-link d-flex align-items-center gap-2" href="#">
          <span class="fi fi-es"></span> Spanish
          <span class="badge bg-warning text-dark">Partial</span>
        </a>
      </li>
      <li class="nav-item">
        <a class="nav-link d-flex align-items-center gap-2" href="#">
          <span class="fi fi-fr"></span> French
          <span class="badge bg-danger">Missing</span>
        </a>
      </li>
      <li class="nav-item">
        <a class="nav-link d-flex align-items-center gap-2" href="#">
          <span class="fi fi-de"></span> German
          <span class="badge bg-success">Complete</span>
        </a>
      </li>
      <li class="nav-item">
        <a class="nav-link d-flex align-items-center gap-2" href="#">
          <span class="fi fi-jp"></span> Japanese
          <span class="badge bg-danger">Missing</span>
        </a>
      </li>
    </ul>
  </div>
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <div>
        <span class="badge bg-success">English</span>
        <span class="text-muted small ms-2">Source language &bull; Last updated Mar 15, 2024</span>
      </div>
      <button class="btn btn-outline-primary btn-sm">
        <i class="bi bi-translate me-1"></i>Auto-translate Missing
      </button>
    </div>
    <form>
      <div class="mb-3">
        <label for="enTitle" class="form-label">Title</label>
        <input type="text" class="form-control" id="enTitle" value="Getting Started with Bootstrap 5">
      </div>
      <div class="mb-3">
        <label for="enDesc" class="form-label">Description</label>
        <textarea class="form-control" id="enDesc" rows="3">Learn how to build responsive, accessible websites using Bootstrap 5's powerful component library.</textarea>
      </div>
      <div class="mb-3">
        <label for="enContent" class="form-label">Content</label>
        <div class="border rounded p-4 bg-light" style="min-height:200px">
          <p>Content editor area for English translation...</p>
        </div>
      </div>
      <button class="btn btn-primary">Save English</button>
    </form>
  </div>
</div>
```

### Side-by-Side Translation View

```html
<div class="card">
  <div class="card-header bg-white">
    <h5 class="mb-0">Translate: Spanish</h5>
  </div>
  <div class="card-body p-0">
    <div class="row g-0">
      <div class="col-md-6 border-end">
        <div class="p-3 bg-light border-bottom">
          <span class="badge bg-secondary">English (Source)</span>
        </div>
        <div class="p-3">
          <div class="mb-3">
            <label class="form-label small text-muted">Title</label>
            <div class="form-control-plaintext">Getting Started with Bootstrap 5</div>
          </div>
          <div class="mb-3">
            <label class="form-label small text-muted">Description</label>
            <div class="form-control-plaintext">Learn how to build responsive, accessible websites using Bootstrap 5.</div>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="p-3 bg-primary bg-opacity-10 border-bottom">
          <span class="badge bg-primary">Spanish (Translation)</span>
        </div>
        <div class="p-3">
          <div class="mb-3">
            <label for="esTitle" class="form-label small text-muted">Title</label>
            <input type="text" class="form-control" id="esTitle" value="Comenzando con Bootstrap 5">
          </div>
          <div class="mb-3">
            <label for="esDesc" class="form-label small text-muted">Description</label>
            <textarea class="form-control" id="esDesc" rows="2">Aprende a crear sitios web responsivos y accesibles usando Bootstrap 5.</textarea>
          </div>
          <div class="mb-3">
            <label for="esContent" class="form-label small text-muted">Content</label>
            <div class="border rounded p-4 bg-light text-center" style="min-height:100px">
              <i class="bi bi-translate text-muted fs-4"></i>
              <p class="text-muted small mb-0">Translation needed</p>
            </div>
          </div>
          <button class="btn btn-primary btn-sm">Save Spanish Translation</button>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Locale-Aware Formatting Preview

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Locale Formatting Preview</h5></div>
  <div class="card-body">
    <div class="table-responsive">
      <table class="table table-sm align-middle mb-0">
        <thead class="table-light">
          <tr>
            <th>Format</th>
            <th>English (US)</th>
            <th>Spanish (ES)</th>
            <th>German (DE)</th>
            <th>Japanese (JP)</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Date</td>
            <td>03/15/2024</td>
            <td>15/03/2024</td>
            <td>15.03.2024</td>
            <td>2024/03/15</td>
          </tr>
          <tr>
            <td>Currency</td>
            <td>$1,299.99</td>
            <td>1.299,99 &euro;</td>
            <td>1.299,99 &euro;</td>
            <td>&yen;129,999</td>
          </tr>
          <tr>
            <td>Number</td>
            <td>1,234,567.89</td>
            <td>1.234.567,89</td>
            <td>1.234.567,89</td>
            <td>1,234,567.89</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</div>
```

### Translation Progress Dashboard

```html
<div class="row g-4 mb-4">
  <div class="col-md-4">
    <div class="card h-100">
      <div class="card-body text-center">
        <h3 class="text-success">98%</h3>
        <p class="text-muted mb-0">English</p>
        <div class="progress mt-2" style="height:4px">
          <div class="progress-bar bg-success" style="width:98%"></div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card h-100">
      <div class="card-body text-center">
        <h3 class="text-warning">65%</h3>
        <p class="text-muted mb-0">Spanish</p>
        <div class="progress mt-2" style="height:4px">
          <div class="progress-bar bg-warning" style="width:65%"></div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card h-100">
      <div class="card-body text-center">
        <h3 class="text-danger">12%</h3>
        <p class="text-muted mb-0">French</p>
        <div class="progress mt-2" style="height:4px">
          <div class="progress-bar bg-danger" style="width:12%"></div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Show translation status badges: Complete (green), Partial (yellow), Missing (red)
2. Provide side-by-side source and translation views
3. Include locale-aware formatting previews (dates, currency, numbers)
4. Offer auto-translate as a starting point with manual review
5. Track translation progress per language with progress bars
6. Use language tabs with flag icons for quick switching
7. Show last-updated timestamps per translation
8. Allow fallback to source language for missing translations
9. Provide translation memory suggestions from previous translations
10. Support RTL languages with `dir="rtl"` on content fields

## Common Pitfalls

1. **No translation status** - Editors can't see which languages need work. Use badges and progress bars.
2. **Missing source reference** - Translators need to see the source text. Provide side-by-side views.
3. **Hardcoded locale formats** - Dates and currencies must adapt to locale. Use `Intl` APIs.
4. **No auto-translate option** - Starting from scratch is slow. Provide machine translation as a draft.
5. **RTL not supported** - Arabic and Hebrew need right-to-left text direction support.
6. **No translation progress tracking** - Managers need to see completion rates across languages.

## Accessibility Considerations

- Use `hreflang` attributes on language switcher links
- Mark the active language tab with `aria-current="page"`
- Use `lang` attribute on translation fields (e.g., `lang="es"`)
- Provide `aria-label` on translation progress indicators
- Announce translation save status with `aria-live="polite"`

## Responsive Behavior

On **mobile**, language tabs become a horizontal scrollable row. Side-by-side translation views stack vertically with source above translation. On **tablet**, the side-by-side view works with reduced padding. On **desktop**, the full side-by-side layout with source on the left and translation on the right displays comfortably.
