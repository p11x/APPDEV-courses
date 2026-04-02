---
tags: [bootstrap, history, versions, evolution, roadmap, deprecation]
category: Bootstrap Fundamentals
difficulty: 1
estimated_time: 20 minutes
---

# Version History Overview

## Overview

Bootstrap's evolution from an internal Twitter tool to the world's most popular CSS framework spans over a decade of active development. Each major version introduced significant architectural changes, reflecting shifts in web standards, browser capabilities, and frontend development practices.

Bootstrap began as **Twitter Blueprint** in 2010, created by Mark Otto and Jacob Thornton as an internal tool to standardize UI development at Twitter. It was open-sourced as **Bootstrap** in August 2011 and quickly became the most popular project on GitHub.

Understanding Bootstrap's version history helps developers make informed decisions about migration, compatibility, and long-term maintenance. Each version's lifecycle includes active development, security patches, and eventual deprecation.

The following table summarizes Bootstrap's major versions, their release dates, and key defining features:

| Version | Release | Key Features | Status |
|---------|---------|-------------|--------|
| 1.0 | Aug 2011 | Basic grid, responsive CSS, jQuery plugins | Deprecated |
| 2.0 | Jan 2012 | Responsive design, icon font, fluid grid | Deprecated |
| 3.0 | Aug 2013 | Flat design, mobile-first, Glyphicons | Deprecated |
| 4.0 | Jan 2018 | Flexbox grid, Sass, cards, utility API | Deprecated |
| 5.0 | May 2021 | No jQuery, RTL, CSS custom properties, xxl breakpoint | Active |
| 5.3 | Sep 2023 | CSS Grid, color modes, Sass color modes | Active |

```html
<!-- Bootstrap 1.0 (2011) - jQuery era begins -->
<link rel="stylesheet" href="bootstrap-1.0.0.css">
<script src="jquery.js"></script>
<script src="bootstrap-1.0.0.js"></script>

<!-- Bootstrap 3.0 (2013) - Mobile-first revolution -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
<div class="container-fluid">
  <div class="row">
    <div class="col-md-4 col-sm-6 col-xs-12">Mobile-first grid</div>
  </div>
</div>

<!-- Bootstrap 5.3 (2023+) - Modern, no jQuery -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
```

## Basic Implementation

### Bootstrap 1.x and 2.x (2011-2012)

The original Bootstrap introduced the concept of a comprehensive CSS framework with pre-built components. Bootstrap 1 provided a basic 12-column grid, styled buttons, navigation, forms, and jQuery plugins for modals, dropdowns, and tooltips.

Bootstrap 2 added responsive design with a fluid grid system, the Glyphicons icon font, and improved form styling. The responsive grid used percentage-based widths and was the first step toward mobile compatibility.

```html
<!-- Bootstrap 2.x grid (2012) -->
<div class="container-fluid">
  <div class="row-fluid">
    <div class="span4">Column 1</div>
    <div class="span4">Column 2</div>
    <div class="span4">Column 3</div>
  </div>
</div>
```

### Bootstrap 3.x (2013-2017)

Bootstrap 3 was a landmark release that embraced **mobile-first design**. It replaced the fixed-width grid with a fluid, mobile-first responsive system using `col-xs-*`, `col-sm-*`, `col-md-*`, and `col-lg-*` classes. The flat design movement influenced its visual refresh, dropping gradients and shadows from previous versions.

```html
<!-- Bootstrap 3.x mobile-first grid (2013) -->
<div class="container">
  <div class="row">
    <div class="col-xs-12 col-sm-6 col-md-4 col-lg-3">
      Responsive column
    </div>
  </div>
</div>

<!-- Bootstrap 3.x button groups -->
<div class="btn-group" role="group">
  <button type="button" class="btn btn-default">Left</button>
  <button type="button" class="btn btn-primary">Middle</button>
  <button type="button" class="btn btn-default">Right</button>
</div>
```

### Bootstrap 4.x (2018-2020)

Bootstrap 4 migrated from `float`-based grid to **Flexbox**, enabling powerful alignment and distribution utilities. It introduced the **card** component (replacing panels, wells, and thumbnails), the utility API, and Sass as the primary CSS preprocessor. The `rem` unit replaced `px` for scalable spacing and typography.

```html
<!-- Bootstrap 4.x Flexbox grid (2018) -->
<div class="container">
  <div class="row justify-content-between align-items-center">
    <div class="col-md-4">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">Card Component</h5>
          <p class="card-text">New in Bootstrap 4.</p>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="d-flex justify-content-center">
        Flexbox utilities
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Bootstrap 5.x (2021-Present)

Bootstrap 5 was a ground-up rewrite that removed jQuery, introduced CSS custom properties, added RTL support, and expanded the utility system. Key architectural changes:

```javascript
// Bootstrap 4 - jQuery plugin API
$('#myModal').modal('show');
$('#myTooltip').tooltip();
$('[data-toggle="popover"]').popover();

// Bootstrap 5 - Vanilla JS class-based API
const modal = new bootstrap.Modal(document.getElementById('myModal'));
modal.show();

const tooltip = new bootstrap.Tooltip(document.getElementById('myTooltip'));

const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
const popoverList = [...popoverTriggerList].map(
  el => new bootstrap.Popover(el)
);
```

```css
/* Bootstrap 5 - CSS Custom Properties for runtime theming */
:root {
  --bs-primary: #0d6efd;
  --bs-primary-rgb: 13, 110, 253;
}

[data-bs-theme="dark"] {
  --bs-body-bg: #212529;
  --bs-body-color: #dee2e6;
}
```

### Bootstrap 5.3 (2023)

Bootstrap 5.3 introduced **color modes** (light/dark/auto) via the `data-bs-theme` attribute, **CSS Grid support** alongside the existing Flexbox grid, and improved Sass color functions.

```html
<!-- Bootstrap 5.3 color modes -->
<html lang="en" data-bs-theme="light">
<body>
  <button onclick="document.documentElement.setAttribute('data-bs-theme', 
    document.documentElement.getAttribute('data-bs-theme') === 'dark' ? 'light' : 'dark')">
    Toggle Theme
  </button>
</body>
</html>

<!-- Bootstrap 5.3 CSS Grid -->
<div class="grid gap-3">
  <div class="g-col-6">CSS Grid Column</div>
  <div class="g-col-6">CSS Grid Column</div>
</div>
```

### Deprecation Timeline

```text
Bootstrap 1.x: Released Aug 2011, deprecated when 2.0 released (Jan 2012)
Bootstrap 2.x: Released Jan 2012, deprecated when 3.0 released (Aug 2013)
Bootstrap 3.x: Released Aug 2013, EOL July 2019 (security patches only)
Bootstrap 4.x: Released Jan 2018, EOL January 2023 (no further patches)
Bootstrap 5.x: Released May 2021, actively maintained (current version: 5.3.3)
```

## Best Practices

- **Use Bootstrap 5.x for all new projects** — Bootstrap 4 reached end-of-life in January 2023 and no longer receives security patches.
- **Monitor the Bootstrap blog and GitHub releases** for deprecation notices, security updates, and migration guides before upgrading.
- **Test thoroughly after major version upgrades** — even minor releases can introduce subtle CSS changes that affect layout.
- **Keep dependencies updated** — Bootstrap's Popper.js dependency receives frequent updates for positioning accuracy and bug fixes.
- **Document your Bootstrap version** in `package.json` and your project's README for team awareness and reproducibility.
- **Plan migration budgets** — major version upgrades (3→4, 4→5) typically require 2-5 days of developer time depending on project complexity.
- **Use Bootstrap's official migration guides** available on the documentation site for systematic class-by-class updates.
- **Avoid mixing Bootstrap versions** — using Bootstrap 4 CSS with Bootstrap 5 JavaScript (or vice versa) causes unpredictable behavior.
- **Consider long-term support** — Bootstrap 5 is the only actively supported version; building on deprecated versions creates technical debt.
- **Follow Bootstrap's release channels** — the GitHub repository, Twitter account, and blog announce breaking changes before they land in stable releases.

## Common Pitfalls

- **Starting new projects on Bootstrap 4** — it reached EOL in January 2023 and lacks security patches, RTL support, and CSS custom properties.
- **Ignoring minor version changes** — Bootstrap 5.2 → 5.3 introduced color modes that can break existing dark mode implementations if custom CSS conflicts with the new system.
- **Assuming backward compatibility between major versions** — Bootstrap 4→5 changed over 100 class names; automated migration tools help but require manual review.
- **Not updating Popper.js alongside Bootstrap** — version mismatches between Bootstrap and Popper.js cause positioning failures in dropdowns and tooltips.
- **Missing the xxl breakpoint** when migrating from Bootstrap 4 — the new 1400px breakpoint affects container widths and grid column behavior on large screens.
- **Forgetting to migrate `data-toggle` to `data-bs-toggle`** — this single attribute change affects every interactive Bootstrap plugin and is the most common migration failure point.

## Accessibility Considerations

Each Bootstrap version improved accessibility progressively:

- **Bootstrap 3**: Added ARIA landmark roles and basic screen reader support.
- **Bootstrap 4**: Improved form validation feedback and keyboard navigation for dropdowns.
- **Bootstrap 5**: Comprehensive ARIA attribute support, focus trapping in modals/offcanvas, `aria-expanded` state management, reduced motion support, and semantic HTML requirements.

When migrating between versions, verify that ARIA attributes are updated alongside markup changes. Bootstrap 5 enforces stricter accessibility patterns — modals require `aria-labelledby` and `aria-hidden`, close buttons require `aria-label`, and dropdowns manage `aria-expanded` programmatically.

```html
<!-- Accessibility evolution across versions -->

<!-- Bootstrap 3: Basic ARIA -->
<div class="modal" role="dialog" aria-labelledby="myModalLabel">
  <button class="close" data-dismiss="modal">&times;</button>
</div>

<!-- Bootstrap 4: Improved patterns -->
<div class="modal" role="dialog" aria-labelledby="myModalLabel" aria-modal="true">
  <button class="close" data-dismiss="modal" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>

<!-- Bootstrap 5: Full ARIA compliance -->
<div class="modal fade" tabindex="-1" aria-labelledby="myModalLabel" 
     aria-hidden="true" data-bs-theme="light">
  <button type="button" class="btn-close" data-bs-dismiss="modal" 
          aria-label="Close"></button>
</div>
```

## Responsive Behavior

Bootstrap's responsive system evolved significantly across versions:

```html
<!-- Bootstrap 2: Fluid percentage grid -->
<div class="row-fluid">
  <div class="span6">50%</div>
  <div class="span6">50%</div>
</div>

<!-- Bootstrap 3: Mobile-first with 4 breakpoints -->
<div class="row">
  <div class="col-xs-12 col-sm-6 col-md-4 col-lg-3">4 breakpoints</div>
</div>

<!-- Bootstrap 4: Flexbox grid with 5 breakpoints -->
<div class="row">
  <div class="col-12 col-sm-6 col-md-4 col-lg-3 col-xl-2">5 breakpoints</div>
</div>

<!-- Bootstrap 5: Flexbox + CSS Grid with 6 breakpoints -->
<div class="row">
  <div class="col-12 col-sm-6 col-md-4 col-lg-3 col-xl-2 col-xxl-1">6 breakpoints</div>
</div>

<!-- Bootstrap 5.3: CSS Grid alternative -->
<div class="grid">
  <div class="g-col-12 g-col-sm-6 g-col-md-4">CSS Grid responsive</div>
</div>
```

The addition of the `xxl` breakpoint (≥1400px) in Bootstrap 5 and CSS Grid support in Bootstrap 5.3 represent the most significant responsive layout changes in recent versions. Container max-widths also increased: Bootstrap 4's largest container was 1140px, while Bootstrap 5's xxl container reaches 1320px.
