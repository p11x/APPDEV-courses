---
title: "Component State Management with CSS"
description: "Using CSS :checked, :target, and :focus-within for state management in Bootstrap components"
difficulty: 2
tags: ["state-management", "css", "selectors", "progressive-enhancement", "bootstrap"]
prerequisites: ["02_06_Navbar", "04_08_05_Graceful_Degradation"]
---

## Overview

CSS pseudo-classes like `:checked`, `:target`, and `:focus-within` can manage component state without JavaScript. These selectors react to user interactions natively, enabling toggles, tabs, offcanvas panels, and collapsible sections using pure CSS. Bootstrap leverages these patterns internally — the offcanvas component uses `:target` for hash-based activation, and custom checkboxes use `:checked` for visual state.

Using CSS for state management reduces JavaScript dependency, improves performance, and provides progressive enhancement — the state works even if scripts fail to load.

## Basic Implementation

```html
<!-- CSS-only toggle switch with :checked -->
<style>
  .toggle-content {
    max-height: 0;
    overflow: hidden;
    transition: max-height 0.3s ease;
  }

  #toggleSwitch:checked ~ .toggle-content {
    max-height: 500px;
  }

  #toggleSwitch:checked ~ .toggle-label .toggle-icon {
    transform: rotate(180deg);
  }
</style>

<div class="card">
  <div class="card-header">
    <input type="checkbox" class="btn-check" id="toggleSwitch" autocomplete="off">
    <label class="btn btn-outline-primary toggle-label" for="toggleSwitch">
      Toggle Details <i class="bi bi-chevron-down toggle-icon"></i>
    </label>
  </div>
  <div class="toggle-content">
    <div class="card-body">
      <p>This content is shown/hidden using pure CSS :checked state.</p>
      <p>No JavaScript required — works even if scripts fail to load.</p>
    </div>
  </div>
</div>
```

```html
<!-- CSS-only tabs with :checked -->
<style>
  .css-tabs input[type="radio"] { display: none; }

  .css-tabs .tab-panel {
    display: none;
    padding: 1rem;
    border: 1px solid var(--bs-border-color);
    border-top: none;
  }

  #tab1:checked ~ .tab-panels .panel-1,
  #tab2:checked ~ .tab-panels .panel-2,
  #tab3:checked ~ .tab-panels .panel-3 {
    display: block;
  }

  .css-tabs .tab-label {
    padding: 0.5rem 1rem;
    cursor: pointer;
    border: 1px solid transparent;
    border-bottom: none;
    margin-bottom: -1px;
  }

  #tab1:checked ~ .tab-labels label[for="tab1"],
  #tab2:checked ~ .tab-labels label[for="tab2"],
  #tab3:checked ~ .tab-labels label[for="tab3"] {
    border-color: var(--bs-border-color);
    background: var(--bs-body-bg);
    border-bottom-color: var(--bs-body-bg);
  }
</style>

<div class="css-tabs">
  <input type="radio" name="tabs" id="tab1" checked>
  <input type="radio" name="tabs" id="tab2">
  <input type="radio" name="tabs" id="tab3">

  <div class="tab-labels d-flex">
    <label class="tab-label" for="tab1">Overview</label>
    <label class="tab-label" for="tab2">Details</label>
    <label class="tab-label" for="tab3">Reviews</label>
  </div>

  <div class="tab-panels">
    <div class="tab-panel panel-1">Overview content managed by CSS.</div>
    <div class="tab-panel panel-2">Details content managed by CSS.</div>
    <div class="tab-panel panel-3">Reviews content managed by CSS.</div>
  </div>
</div>
```

## Advanced Variations

```html
<!-- :target-based offcanvas without Bootstrap JS -->
<style>
  .css-offcanvas {
    position: fixed;
    top: 0;
    right: -300px;
    width: 300px;
    height: 100%;
    background: var(--bs-body-bg);
    box-shadow: -2px 0 8px rgba(0,0,0,0.2);
    transition: right 0.3s ease;
    z-index: 1050;
    padding: 1rem;
  }

  .css-offcanvas:target {
    right: 0;
  }

  .css-offcanvas-backdrop {
    display: none;
    position: fixed;
    inset: 0;
    background: rgba(0,0,0,0.5);
    z-index: 1040;
  }

  .css-offcanvas:target ~ .css-offcanvas-backdrop {
    display: block;
  }
</style>

<a href="#myPanel" class="btn btn-primary">Open Panel</a>

<div id="myPanel" class="css-offcanvas">
  <a href="#" class="btn-close position-absolute top-0 end-0 m-3" aria-label="Close"></a>
  <h5>Offcanvas Panel</h5>
  <p>Managed entirely by CSS :target selector.</p>
</div>
<a href="#" class="css-offcanvas-backdrop" tabindex="-1" aria-hidden="true"></a>
```

```html
<!-- :focus-within for form validation state -->
<style>
  .focus-group {
    padding: 0.75rem;
    border: 2px solid transparent;
    border-radius: 0.375rem;
    transition: border-color 0.2s ease;
  }

  .focus-group:focus-within {
    border-color: var(--bs-primary);
    background: rgba(13, 110, 253, 0.05);
  }

  .focus-group:focus-within .field-label {
    color: var(--bs-primary);
    font-weight: 600;
  }
</style>

<div class="focus-group mb-3">
  <label class="form-label field-label">Email address</label>
  <input type="email" class="form-control" placeholder="name@example.com">
  <div class="form-text">We'll never share your email.</div>
</div>
```

## Best Practices

1. Use `:checked` with hidden radio/checkbox inputs for CSS-only toggle state
2. Use `:target` for hash-based navigation to offcanvas and modal-like panels
3. Use `:focus-within` to highlight entire form groups during input focus
4. Combine CSS state with Bootstrap's `.btn-check` for toggle buttons
5. Provide smooth transitions with `transition` property on state changes
6. Ensure CSS-only state degrades gracefully when JavaScript enhances it
7. Use `aria-expanded` alongside `:checked` state for accessibility
8. Keep CSS state selectors specific to avoid unintended style leaks
9. Test CSS state patterns without JavaScript enabled
10. Document which components use CSS-only state vs JS-enhanced state

## Common Pitfalls

1. **Sibling selector limitations** — `~` and `+` require the input and content to be siblings; deeply nested structures break the pattern
2. **Hash change pollution** — `:target` pushes to browser history; rapid toggling creates a back-button maze
3. **No ARIA updates** — CSS state changes don't update `aria-expanded`; screen readers miss the state change
4. **Overriding Bootstrap JS** — CSS-only patterns can conflict with Bootstrap's JavaScript plugin behavior
5. **Transition jank** — Animating `max-height` from 0 to a large value (e.g., 1000px) causes delayed transitions
6. **Focus management gaps** — `:target` panels don't automatically move focus into the opened content

## Accessibility Considerations

CSS-only state management lacks programmatic state announcements. When using `:checked` for toggles, add `aria-expanded` via a small enhancement script. `:target` panels should contain a close link as the first focusable element. Use `:focus-within` to provide clear visual feedback for keyboard users navigating form groups.

## Responsive Behavior

CSS state patterns work across all viewport widths by default. Offcanvas panels using `:target` should be full-width on mobile (`width: 100%`). Toggle content should respect the responsive grid — `:checked` expansions should not break column layouts on narrow viewports. Test tab patterns at each breakpoint since label text may wrap differently.
