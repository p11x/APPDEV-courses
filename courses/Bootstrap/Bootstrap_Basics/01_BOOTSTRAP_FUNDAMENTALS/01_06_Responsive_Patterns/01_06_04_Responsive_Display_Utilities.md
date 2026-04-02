---
title: "Responsive Display Utilities"
module: "Responsive Patterns"
lesson: "01_06_04"
difficulty: 2
estimated_time: "20 minutes"
tags: [display, d-none, d-block, visibility, show-hide, responsive]
prerequisites:
  - "01_06_01_Breakpoint_System"
  - "01_05_01_Utility_Fundamentals"
---

# Responsive Display Utilities

## Overview

Bootstrap's responsive display utilities control the CSS `display` property at specific breakpoints. The `d-{breakpoint}-{value}` pattern lets you show, hide, or change the display mode of elements depending on the viewport width. This is one of the most frequently used responsive patterns in Bootstrap, enabling conditional visibility and layout switching across device sizes.

The syntax follows Bootstrap's mobile-first convention: `d-{value}` sets the display at all sizes, while `d-{breakpoint}-{value}` activates only at the specified breakpoint and above. The available display values include `none`, `inline`, `inline-block`, `block`, `grid`, `flex`, `inline-flex`, and `table`.

Responsive visibility is not simply about showing and hiding elements. It involves choosing the correct display value so that hidden elements are properly removed from the layout flow, and shown elements participate correctly in their parent's formatting context. A `d-none` element is completely removed from layout — it takes no space and is invisible to both visual users and assistive technologies (unless overridden).

Understanding display utilities at each breakpoint is essential for building responsive navigation, toggling between mobile and desktop components, and implementing progressive disclosure patterns.

---

## Basic Implementation

The most common pattern uses `d-none` and `d-{breakpoint}-block` to hide an element on mobile and show it on larger screens.

**Example 1: Show on desktop, hide on mobile**

```html
<div class="d-none d-md-block">
  <p>This paragraph is hidden below 768px and visible from md up.</p>
</div>
```

`d-none` hides the element at all sizes. `d-md-block` overrides `d-none` at 768px and above by setting `display: block`. Below 768px, `d-none` is active and the element takes no space in the layout.

**Example 2: Show on mobile, hide on desktop**

```html
<div class="d-block d-md-none">
  <button class="btn btn-primary w-100">
    Call Now — Tap to Dial
  </button>
</div>
```

`d-block` shows the element at all sizes. `d-md-none` hides it at `md` and above. This pattern is ideal for mobile-only elements like tap-to-call buttons or mobile-specific CTAs that are unnecessary on desktop.

**Example 3: Toggle between two different elements by breakpoint**

```html
<!-- Mobile version -->
<div class="d-md-none">
  <nav class="navbar navbar-dark bg-dark">
    <button class="navbar-toggler" data-bs-toggle="collapse"
            data-bs-target="#mobileMenu">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="mobileMenu">
      <ul class="navbar-nav">
        <li class="nav-item"><a class="nav-link" href="#">Home</a></li>
        <li class="nav-item"><a class="nav-link" href="#">About</a></li>
      </ul>
    </div>
  </nav>
</div>

<!-- Desktop version -->
<div class="d-none d-md-flex justify-content-between align-items-center p-3 bg-dark">
  <span class="text-white fw-bold">Site Name</span>
  <nav>
    <a href="#" class="text-white me-3">Home</a>
    <a href="#" class="text-white me-3">About</a>
    <a href="#" class="text-white">Contact</a>
    <a href="#" class="text-white">Services</a>
  </nav>
</div>
```

This pattern renders two entirely different navigation components. On mobile, a collapsible hamburger menu appears. On `md` and above, a horizontal flex navigation bar replaces it. Each component is hidden at the breakpoints where the other is shown. Both exist in the DOM simultaneously; only one is visible at any viewport size.

---

## Advanced Variations

**Example 4: Responsive flex display with direction changes**

```html
<div class="d-flex flex-column d-md-flex flex-md-row align-items-md-center">
  <div class="p-2 bg-light border">
    <strong>Label</strong>
  </div>
  <div class="p-2 bg-light border flex-grow-1">
    <input type="text" class="form-control" placeholder="Search...">
  </div>
  <div class="p-2 bg-light border">
    <button class="btn btn-primary">Go</button>
  </div>
</div>
```

On mobile, `d-flex flex-column` stacks the label, input, and button vertically. At `md`, `d-md-flex flex-md-row` switches to a horizontal flex layout, and `align-items-md-center` vertically centers all items. The `flex-grow-1` on the input makes it consume remaining space in the row layout.

**Example 5: Responsive table visibility for mobile-friendly data**

```html
<!-- Desktop table -->
<div class="d-none d-lg-block">
  <table class="table table-striped">
    <thead>
      <tr>
        <th>Name</th>
        <th>Email</th>
        <th>Role</th>
        <th>Status</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Jane Smith</td>
        <td>jane@example.com</td>
        <td>Admin</td>
        <td><span class="badge bg-success">Active</span></td>
        <td><button class="btn btn-sm btn-outline-primary">Edit</button></td>
      </tr>
    </tbody>
  </table>
</div>

<!-- Mobile cards -->
<div class="d-lg-none">
  <div class="card mb-2">
    <div class="card-body">
      <h6 class="card-title">Jane Smith</h6>
      <p class="card-text small mb-1">jane@example.com</p>
      <p class="card-text small mb-1">Role: Admin</p>
      <span class="badge bg-success">Active</span>
      <button class="btn btn-sm btn-outline-primary mt-2">Edit</button>
    </div>
  </div>
</div>
```

Tables with many columns do not fit on mobile screens. This pattern shows a full table on `lg` and above, and replaces it with stacked cards on smaller viewports. Each card contains the same data as a table row, reformatted for vertical consumption. Both versions are in the DOM; the display utilities control which is visible.

**Example 6: Responsive grid display mode**

```html
<div class="d-block d-md-grid d-lg-flex"
     style="grid-template-columns: repeat(3, 1fr); gap: 1rem;">
  <div class="p-3 bg-light border">Item 1</div>
  <div class="p-3 bg-light border">Item 2</div>
  <div class="p-3 bg-light border">Item 3</div>
  <div class="p-3 bg-light border">Item 4</div>
  <div class="p-3 bg-light border">Item 5</div>
  <div class="p-3 bg-light border">Item 6</div>
</div>
```

This element switches its display mode across three breakpoints. On mobile, `d-block` renders items as stacked blocks. At `md`, `d-grid` activates CSS Grid with the defined `grid-template-columns`. At `lg`, `d-flex` switches to a flex layout. Each display mode changes how child elements are arranged.

**Example 7: Conditional sidebar with responsive display**

```html
<div class="row">
  <main class="col-12 col-lg-9 order-lg-2">
    <h1>Article Title</h1>
    <p>Main content goes here...</p>
  </main>

  <!-- Sidebar: hidden on mobile/tablet, shown on lg+ -->
  <aside class="col-lg-3 order-lg-1 d-none d-lg-block">
    <div class="list-group">
      <a href="#" class="list-group-item list-group-item-action active">Overview</a>
      <a href="#" class="list-group-item list-group-item-action">Installation</a>
      <a href="#" class="list-group-item list-group-item-action">Configuration</a>
      <a href="#" class="list-group-item list-group-item-action">Deployment</a>
    </div>
  </aside>
</div>
```

The sidebar is hidden below `lg` using `d-none`. At `lg`, `d-lg-block` shows it. The `order-lg-1` and `order-lg-2` classes rearrange the visual order so the sidebar appears on the left on desktop, while main content appears first on mobile. This is a common documentation site pattern.

---

## Best Practices

1. **Use `d-none d-{breakpoint}-block` to show only at larger breakpoints.** This is the most common visibility pattern. The `d-none` hides at all sizes, and the suffixed class overrides it at the target breakpoint.

2. **Use `d-{breakpoint}-none` to hide only at larger breakpoints.** `d-block d-md-none` shows on mobile and hides on tablet and above. The base `d-block` keeps the element visible at smaller sizes.

3. **Choose the correct display value when showing elements.** Use `d-{bp}-block` for block-level elements, `d-{bp}-flex` for flex containers, `d-{bp}-inline` for inline elements. Using `d-{bp}-block` on a flex container breaks its flex behavior.

4. **Prefer `visually-hidden` over `d-none` for screen-reader-accessible hidden content.** `d-none` removes the element from both visual layout and accessibility tree. `visually-hidden` hides visually but keeps the element accessible.

5. **Do not use display utilities to create animation effects.** Toggling `d-none` causes instant show/hide with no transition. Use opacity and visibility CSS properties with transitions for animated reveals.

6. **Avoid hiding critical content on mobile.** If an element is `d-none d-md-block`, its content is invisible to mobile users. Ensure that mobile users are not missing essential information or functionality.

7. **Use `d-print-block` to show content only when printing.** Bootstrap includes print display utilities. `d-none d-print-block` hides on screen but shows in print, useful for URLs, QR codes, or reference information.

8. **Combine display utilities with other responsive utilities.** `d-none d-md-flex justify-content-between align-items-center` creates a hidden-on-mobile, flex-centered element on desktop. Stacking utilities produces compound responsive behavior.

9. **Keep duplicated DOM elements to a minimum.** The mobile/desktop toggle pattern (two separate elements, one hidden per breakpoint) increases DOM size. For simple content differences, use CSS-only approaches like responsive text or responsive spacing instead.

10. **Test all breakpoints to verify visibility logic.** A `d-sm-none d-lg-block` pattern hides from `sm` through `md` and shows at `lg`. Verify that intermediate breakpoints behave as expected.

11. **Use `d-{breakpoint}-table-row` and `d-{breakpoint}-table-cell` for responsive table elements.** Generic `d-block` on `<tr>` or `<td>` elements breaks table semantics. Bootstrap provides table-specific display values.

12. **Avoid nesting `d-none` on parent and child elements unnecessarily.** If a parent is `d-none`, children inherit the hidden state. Applying `d-none` to both is redundant and can cause confusion when the parent's display changes but the child's does not.

---

## Common Pitfalls

**Pitfall 1: Using `d-none` without a corresponding show class.**
`d-hide` is not a Bootstrap class. `d-none` hides permanently unless a suffixed display class overrides it. Forgetting the override class keeps the element hidden at all sizes.

**Pitfall 2: Applying `d-block` to a flex parent.**
If a container uses `d-flex`, applying `d-block` to a child overrides its flex participation. Inside a flex container, children should not have their display property changed unless intentionally removing them from the flex context.

**Pitfall 3: Expecting `d-none` to preserve layout space.**
`display: none` removes the element from layout entirely. Elements around it collapse into the vacated space. Use `visibility: hidden` if you need to hide an element while preserving its space.

**Pitfall 4: Using `d-sm-block` as the base class.**
`d-sm-block` only activates at `sm` and above. Below `sm`, the element uses its default display (usually `block` for `<div>`). If the intent is to hide below `sm`, `d-none` must be the base class.

**Pitfall 5: Forgetting that `d-none` affects accessibility.**
Elements with `d-none` are removed from the accessibility tree in most screen readers. Critical content that should remain accessible to screen readers must use `visually-hidden` instead.

**Pitfall 6: Mixing display utilities with conflicting CSS.**
Custom CSS that sets `display: flex !important` overrides Bootstrap's `d-none`, breaking responsive visibility. Avoid `!important` on display properties when using Bootstrap's display utilities.

**Pitfall 7: Using `d-none` on `<tr>` elements inappropriately.**
Hiding table rows with `d-none` works, but `d-table-row` must be used to show them again. Using `d-block` on a `<tr>` breaks table semantics.

---

## Accessibility Considerations

When hiding elements with `d-none`, screen readers exclude them from the accessibility tree. If content is hidden on mobile but critical for understanding, use `visually-hidden` so screen readers still announce it. The `visually-hidden` class positions the element off-screen without removing it from the accessibility tree.

When toggling between two elements by breakpoint (mobile element vs. desktop element), ensure that both contain equivalent information and functionality. Screen readers may encounter both elements in the DOM and announce them sequentially. To prevent duplication, use `aria-hidden="true"` on the element that is currently `d-none`, though be aware this affects assistive technology users regardless of viewport size.

Keyboard focus should not land on `d-none` elements. Most browsers exclude `display: none` elements from the tab order, but some edge cases exist with CSS transitions and JavaScript focus management. Test tab navigation across breakpoints to verify that hidden elements do not trap focus.

Interactive elements that are shown conditionally (e.g., a mobile-only menu button) must be keyboard-accessible when visible. Ensure that `tabindex`, focus management, and ARIA attributes are correctly applied on all conditional elements.

---

## Responsive Behavior

Responsive display utilities follow the mobile-first activation pattern. `d-none` applies at all sizes. `d-md-block` activates only at `md` and above, overriding `d-none` in that range.

When multiple breakpoint display classes are used, the smallest active breakpoint wins. `d-none d-sm-block d-lg-flex` hides on `xs`, shows as `block` from `sm` to `md`, and shows as `flex` from `lg` upward. Each class overrides the previous at its breakpoint threshold.

Display utilities cascade like all CSS properties. A `d-flex` on a parent does not affect children's display values unless explicitly set. Children inside a `d-flex` container participate in flex layout regardless of their own display value (except `d-none`, which removes them from the layout entirely).

The breakpoint-suffixed display classes only override the `display` property. They do not affect other CSS properties like `visibility`, `opacity`, or `position`. An element that is `d-none d-md-block` becomes visible at `md`, but any `visibility: hidden` applied by other CSS remains in effect.