---
title: Scrollspy
category: Component System
difficulty: 2
time: 20 min
tags: bootstrap5, scrollspy, scroll-navigation, data-attributes, navbar
---

## Overview

Scrollspy is a Bootstrap plugin that automatically updates navigation links based on scroll position. As the user scrolls through sections of content, the corresponding nav link receives the `active` class. It works with navbars, list groups, and standalone `nav` components. Scrollspy is configured via `data-bs-spy="scroll"` on a scrollable element and `data-bs-target` pointing to the navigation container. The offset can be customized with `data-bs-offset"` to account for fixed headers.

## Basic Implementation

A navbar scrollspy with section targets:

```html
<nav id="navbarScrollspy" class="navbar navbar-light bg-light px-3 sticky-top">
  <a class="navbar-brand" href="#">Scrollspy Demo</a>
  <ul class="nav nav-pills">
    <li class="nav-item">
      <a class="nav-link" href="#section-one">One</a>
    </li>
    <li class="nav-item">
      <a class="nav-link" href="#section-two">Two</a>
    </li>
    <li class="nav-item">
      <a class="nav-link" href="#section-three">Three</a>
    </li>
  </ul>
</nav>
<div data-bs-spy="scroll" data-bs-target="#navbarScrollspy" data-bs-offset="80"
     tabindex="0" style="overflow-y: scroll; height: 400px;">
  <h4 id="section-one">Section One</h4>
  <p>Content for section one. Scroll down to see the nav update.</p>
  <h4 id="section-two">Section Two</h4>
  <p>Content for section two. The active pill shifts automatically.</p>
  <h4 id="section-three">Section Three</h4>
  <p>Content for section three. Scroll back up to see it reverse.</p>
</div>
```

The `data-bs-offset="80"` accounts for the sticky navbar height so the active state triggers at the right scroll position.

## Advanced Variations

Scrollspy on the `<body>` element with a fixed navbar — the most common real-world pattern:

```html
<body data-bs-spy="scroll" data-bs-target="#navbarFixed" data-bs-offset="100"
      tabindex="0">
  <nav id="navbarFixed" class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
    <div class="container-fluid">
      <a class="navbar-brand" href="#">Portfolio</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
              data-bs-target="#navContent" aria-controls="navContent"
              aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navContent">
        <ul class="navbar-nav">
          <li class="nav-item"><a class="nav-link" href="#about">About</a></li>
          <li class="nav-item"><a class="nav-link" href="#skills">Skills</a></li>
          <li class="nav-item"><a class="nav-link" href="#projects">Projects</a></li>
          <li class="nav-item"><a class="nav-link" href="#contact">Contact</a></li>
        </ul>
      </div>
    </div>
  </nav>

  <div class="container">
    <section id="about" class="py-5">
      <h2>About</h2>
      <p>About section content...</p>
    </section>
    <section id="skills" class="py-5">
      <h2>Skills</h2>
      <p>Skills section content...</p>
    </section>
    <section id="projects" class="py-5">
      <h2>Projects</h2>
      <p>Projects section content...</p>
    </section>
    <section id="contact" class="py-5">
      <h2>Contact</h2>
      <p>Contact section content...</p>
    </section>
  </div>
</body>
```

Nested scrollspy with list group navigation:

```html
<div class="row">
  <div class="col-4">
    <div id="listGroupSpy" class="list-group">
      <a class="list-group-item list-group-item-action" href="#list-item-1">Item 1</a>
      <a class="list-group-item list-group-item-action" href="#list-item-2">Item 2</a>
      <a class="list-group-item list-group-item-action" href="#list-item-3">Item 3</a>
    </div>
  </div>
  <div class="col-8">
    <div data-bs-spy="scroll" data-bs-target="#listGroupSpy" data-bs-offset="0"
         class="scrollspy-example" tabindex="0"
         style="height: 300px; overflow-y: scroll;">
      <h4 id="list-item-1">Item 1</h4>
      <p>First item content with enough text to create scrollable area.</p>
      <h4 id="list-item-2">Item 2</h4>
      <p>Second item content with enough text to create scrollable area.</p>
      <h4 id="list-item-3">Item 3</h4>
      <p>Third item content with enough text to create scrollable area.</p>
    </div>
  </div>
</div>
```

Use the JavaScript API for dynamic initialization or when data attributes are not sufficient:

```html
<script>
  const scrollElement = document.querySelector('[data-bs-spy="scroll"]');
  const bsScrollSpy = bootstrap.ScrollSpy.getOrCreateInstance(scrollElement, {
    target: '#navbarScrollspy',
    offset: 100,
    smoothScroll: true
  });
</script>
```

## Best Practices

1. Set `data-bs-spy="scroll"` on the scrollable container (or `<body>` for full-page scroll).
2. Set `data-bs-target` to the `id` of the navigation component that receives `active` updates.
3. Use `data-bs-offset` to compensate for fixed headers — set it to the navbar height in pixels.
4. Add `tabindex="0"` to the scrollable element so it can receive keyboard focus.
5. Ensure every `href` in the nav matches an `id` in the scrollable content exactly.
6. Use `smoothScroll: true` in the JS API or add `scroll-behavior: smooth` in CSS for animated scrolling.
7. Refresh scrollspy with `bootstrap.ScrollSpy.getInstance(el).refresh()` after dynamically adding/removing content.
8. Use scrollspy with `nav-pills` for the clearest active state visual feedback.
9. Keep section IDs unique across the page to prevent scrollspy targeting the wrong element.
10. Use `position: sticky` on the nav container so it stays visible as the user scrolls.

## Common Pitfalls

1. **Missing `data-bs-offset` with fixed navbar** — Without it, the active state triggers too early because scrollspy does not account for the fixed header height.
2. **`id` mismatch** — The `href="#section"` in the nav must exactly match `id="section"` on the target element. A typo breaks the highlight.
3. **No `tabindex="0"` on scrollable div** — Keyboard users cannot focus the scrollable area, making scrollspy inaccessible.
4. **Scrollspy not refreshing after DOM changes** — Adding sections dynamically requires calling `.refresh()` on the ScrollSpy instance.
5. **`data-bs-spy` on a non-scrollable element** — If the element has no overflow, scrollspy has nothing to observe. Ensure `overflow-y: scroll` or `overflow-y: auto` is set, or use `<body>`.
6. **Smooth scroll conflicts** — Third-party smooth scroll libraries can interfere with Bootstrap's scrollspy offset calculations.
7. **Multiple scrollspy instances on the same element** — This causes unpredictable behavior. Use one scrollspy per scrollable container.
8. **Bootstrap JS not loaded** — Scrollspy requires the Bootstrap JavaScript bundle. CSS-only inclusion will not activate the plugin.

## Accessibility Considerations

- `tabindex="0"` on the scrollable container allows keyboard users to focus and scroll through content.
- Scrollspy automatically manages `aria-current` or class changes, but ensure the active link has sufficient visual contrast.
- The navigation component should use semantic markup (`<nav>`, `<ul>`, `<li>`) as covered in navbar and nav documentation.
- Smooth scrolling can cause disorientation for users with vestibular disorders. Respect `prefers-reduced-motion` by disabling smooth scroll when the user preference is set:
  ```css
  @media (prefers-reduced-motion: reduce) {
    html { scroll-behavior: auto; }
  }
  ```
- Ensure that all sections are reachable via keyboard — each `id` target must be focusable or contain focusable content.
- Consider adding `aria-label` to the nav to describe its scrollspy purpose.

## Responsive Behavior

- Scrollspy works at all breakpoints — the `active` class is applied based on scroll position, not viewport size.
- On mobile, scrollspy with a collapsible navbar means the active link is only visible when the hamburger menu is open. Consider a fixed sidebar or bottom navigation for better mobile scrollspy UX.
- The `data-bs-offset` value may need adjustment at different breakpoints if the fixed navbar height changes (e.g., when the navbar wraps on mobile). Use JavaScript to dynamically calculate and update the offset.
- For single-page applications with route changes, scrollspy may need manual refresh when new content mounts.
- Scrollspy on a contained `div` with fixed height works well in dashboard layouts. On full-page scroll, use `<body>` as the scrollspy target.
