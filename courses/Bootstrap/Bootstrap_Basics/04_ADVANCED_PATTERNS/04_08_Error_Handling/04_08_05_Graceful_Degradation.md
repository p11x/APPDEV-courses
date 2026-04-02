---
title: "Graceful Degradation with Bootstrap"
description: "Fallback UI when JavaScript fails, noscript patterns, and progressive enhancement strategies"
difficulty: 2
tags: ["error-handling", "progressive-enhancement", "noscript", "bootstrap"]
prerequisites: ["01_01_Basic_Template", "04_08_03_Network_Error_UI"]
---

## Overview

Graceful degradation ensures your Bootstrap site remains functional when JavaScript fails to load, execute, or is disabled. Progressive enhancement starts with a working HTML/CSS baseline and layers JavaScript enhancements on top. Bootstrap's CSS-only components (grid, utilities, typography, forms) work without JavaScript, but interactive components (modals, dropdowns, carousels) require fallback strategies.

The `<noscript>` tag provides content for no-JS environments, while feature detection with `@supports` and `classList` ensures enhancements degrade cleanly when capabilities are missing.

## Basic Implementation

```html
<!-- Noscript fallback for JS-dependent Bootstrap page -->
<noscript>
  <div class="container py-5">
    <div class="alert alert-warning" role="alert">
      <h4 class="alert-heading"><i class="bi bi-exclamation-triangle me-2"></i>JavaScript Required</h4>
      <p>This application requires JavaScript to function. Please enable JavaScript in your browser settings.</p>
      <hr>
      <p class="mb-0">
        <a href="/sitemap" class="alert-link">View the site map</a> for a text-based navigation alternative.
      </p>
    </div>
  </div>
</noscript>
```

```html
<!-- Progressive enhancement: Modal that works as a link without JS -->
<!-- Without JS: link navigates to dedicated page -->
<!-- With JS: link opens as a modal -->
<a href="/terms-page" class="btn btn-outline-secondary"
   data-bs-toggle="modal" data-bs-target="#termsModal">
  View Terms
</a>

<!-- Modal content (also available at /terms-page) -->
<div class="modal fade" id="termsModal" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Terms of Service</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <p>Terms content loaded inline or fetched from /terms-page...</p>
      </div>
    </div>
  </div>
</div>
```

```js
// Feature detection before enhancing
function enhancePage() {
  // Check for required APIs
  if (!window.fetch || !window.Promise) {
    console.log('Modern JS features not available — using fallback');
    return;
  }

  // Enhance forms with AJAX submission
  document.querySelectorAll('form[data-enhance]').forEach(form => {
    form.addEventListener('submit', async (e) => {
      e.preventDefault();
      const data = new FormData(form);
      const response = await fetch(form.action, { method: 'POST', body: data });

      if (response.ok) {
        // Show success toast
        showToast('Saved successfully', 'success');
      }
    });
  });

  // Enhance navigation with client-side routing
  document.querySelectorAll('a[data-enhance]').forEach(link => {
    link.addEventListener('click', (e) => {
      e.preventDefault();
      loadPage(link.href);
    });
  });
}

document.addEventListener('DOMContentLoaded', enhancePage);
```

## Advanced Variations

```html
<!-- CSS-only fallback for Bootstrap dropdown -->
<style>
  /* Dropdown works on hover/focus without JS */
  .dropdown:hover > .dropdown-menu,
  .dropdown:focus-within > .dropdown-menu {
    display: block;
    margin-top: 0;
  }
</style>

<div class="dropdown">
  <button class="btn btn-secondary dropdown-toggle" type="button"
          data-bs-toggle="dropdown" aria-expanded="false">
    Account
  </button>
  <ul class="dropdown-menu">
    <li><a class="dropdown-item" href="/profile">Profile</a></li>
    <li><a class="dropdown-item" href="/settings">Settings</a></li>
    <li><hr class="dropdown-divider"></li>
    <li><a class="dropdown-item" href="/logout">Logout</a></li>
  </ul>
</div>
```

```html
<!-- Progressive enhancement: Accordion that works without JS -->
<div class="accordion" id="faqAccordion">
  <div class="accordion-item">
    <!-- Works as details/summary without JS -->
    <details>
      <summary class="accordion-header">
        <button class="accordion-button" type="button"
                data-bs-toggle="collapse" data-bs-target="#faq1"
                aria-expanded="true">
          What is Bootstrap?
        </button>
      </summary>
      <div id="faq1" class="accordion-collapse collapse show"
           data-bs-parent="#faqAccordion">
        <div class="accordion-body">
          Bootstrap is a CSS framework for responsive web design.
        </div>
      </div>
    </details>
  </div>
</div>
```

```js
// Detect and handle JS failures gracefully
window.addEventListener('error', (event) => {
  if (event.target.tagName === 'SCRIPT') {
    console.warn('Script failed to load:', event.target.src);

    // Show fallback for failed component
    const fallback = document.querySelector(`[data-fallback-for="${event.target.dataset.name}"]`);
    if (fallback) {
      fallback.classList.remove('d-none');
    }
  }
}, true);
```

## Best Practices

1. Use `<noscript>` to display a meaningful message when JavaScript is disabled
2. Build HTML/CSS first, then layer JavaScript enhancements on top
3. Use `data-*` attributes to mark elements for progressive enhancement
4. Provide CSS-only alternatives for dropdowns using `:hover` and `:focus-within`
5. Use `<details>`/`<summary>` as fallback for accordion components
6. Ensure forms submit via standard HTML action when JS fails
7. Use feature detection (`if (window.fetch)`) not browser detection
8. Test your site with JavaScript disabled in browser settings
9. Provide navigation links alongside JS-driven menus for fallback
10. Use `link` navigation for modal content as a non-JS fallback page

## Common Pitfalls

1. **No `<noscript>` content** — Blank page with JS disabled is the worst experience; always provide a message
2. **Breaking form submission** — AJAX-only forms that prevent default submit become unusable without JS
3. **Hiding content with JS-dependent CSS** — Using `display: none` in JS-loaded stylesheets hides content from no-JS users
4. **Assuming Bootstrap JS is loaded** — CDN failures break all interactive components; provide fallback behavior
5. **Not testing with slow connections** — JS takes time to load on 3G; HTML-only experience during load must be acceptable
6. **Using JS for content that CSS can handle** — Dropdowns and tooltips can work with pure CSS; avoid unnecessary JS dependency

## Accessibility Considerations

Graceful degradation directly benefits accessibility. Users with restrictive Content Security Policies, older assistive technologies, or corporate proxy limitations may not execute JavaScript. Semantic HTML fallbacks — standard links instead of JS-driven modals, `<details>` instead of JS accordions — ensure assistive technology can parse and navigate your content regardless of JavaScript status.

## Responsive Behavior

CSS-only fallbacks should respect Bootstrap's responsive grid. Hover-based dropdown fallbacks fail on touch devices, so ensure the `<a href>` fallback navigates to a dedicated page on mobile. Noscript messages should be readable at all viewport widths using Bootstrap's `container` and responsive typography classes.
