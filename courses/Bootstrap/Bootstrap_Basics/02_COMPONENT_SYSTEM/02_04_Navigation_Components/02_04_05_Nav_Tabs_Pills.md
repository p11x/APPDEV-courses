---
title: Nav Tabs and Pills
category: Component System
difficulty: 2
time: 25 min
tags: bootstrap5, nav, tabs, pills, tab-content, fade, vertical
---

## Overview

Bootstrap's `nav` component provides tabbed and pill-style navigation. The `nav-tabs` class creates underline-style tabs, while `nav-pills` creates rounded button-style pills. Both use the same `nav` base class and support `nav-fill` for equal-width items, `nav-justified` for full-width justification, dropdowns, and vertical layouts. Tabs can be paired with tab content panels using `tab-content` and `tab-pane` for show/hide panel switching, optionally with fade transitions.

## Basic Implementation

Underline-style tabs:

```html
<ul class="nav nav-tabs" id="myTab" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="home-tab" data-bs-toggle="tab"
            data-bs-target="#home-pane" type="button" role="tab"
            aria-controls="home-pane" aria-selected="true">
      Home
    </button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="profile-tab" data-bs-toggle="tab"
            data-bs-target="#profile-pane" type="button" role="tab"
            aria-controls="profile-pane" aria-selected="false">
      Profile
    </button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="contact-tab" data-bs-toggle="tab"
            data-bs-target="#contact-pane" type="button" role="tab"
            aria-controls="contact-pane" aria-selected="false">
      Contact
    </button>
  </li>
</ul>
<div class="tab-content" id="myTabContent">
  <div class="tab-pane fade show active" id="home-pane" role="tabpanel"
       aria-labelledby="home-tab" tabindex="0">
    <p class="p-3">Home tab content goes here.</p>
  </div>
  <div class="tab-pane fade" id="profile-pane" role="tabpanel"
       aria-labelledby="profile-tab" tabindex="0">
    <p class="p-3">Profile tab content goes here.</p>
  </div>
  <div class="tab-pane fade" id="contact-pane" role="tabpanel"
       aria-labelledby="contact-tab" tabindex="0">
    <p class="p-3">Contact tab content goes here.</p>
  </div>
</div>
```

## Advanced Variations

Pills with a dropdown and `nav-fill` for equal-width items:

```html
<ul class="nav nav-pills nav-fill">
  <li class="nav-item">
    <a class="nav-link active" aria-current="page" href="#">Active</a>
  </li>
  <li class="nav-item dropdown">
    <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#"
       role="button" aria-expanded="false">Dropdown</a>
    <ul class="dropdown-menu">
      <li><a class="dropdown-item" href="#">Action</a></li>
      <li><a class="dropdown-item" href="#">Another action</a></li>
      <li><hr class="dropdown-divider"></li>
      <li><a class="dropdown-item" href="#">Separated link</a></li>
    </ul>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#">Link</a>
  </li>
  <li class="nav-item">
    <a class="nav-link disabled" aria-disabled="true" href="#">Disabled</a>
  </li>
</ul>
```

Vertical pills layout with tab content panels:

```html
<div class="d-flex align-items-start">
  <div class="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist"
       aria-orientation="vertical">
    <button class="nav-link active" id="v-pills-home-tab" data-bs-toggle="pill"
            data-bs-target="#v-pills-home" type="button" role="tab"
            aria-controls="v-pills-home" aria-selected="true">
      Home
    </button>
    <button class="nav-link" id="v-pills-settings-tab" data-bs-toggle="pill"
            data-bs-target="#v-pills-settings" type="button" role="tab"
            aria-controls="v-pills-settings" aria-selected="false">
      Settings
    </button>
    <button class="nav-link" id="v-pills-profile-tab" data-bs-toggle="pill"
            data-bs-target="#v-pills-profile" type="button" role="tab"
            aria-controls="v-pills-profile" aria-selected="false">
      Profile
    </button>
  </div>
  <div class="tab-content" id="v-pills-tabContent">
    <div class="tab-pane fade show active" id="v-pills-home" role="tabpanel"
         aria-labelledby="v-pills-home-tab" tabindex="0">
      <p>Home panel content.</p>
    </div>
    <div class="tab-pane fade" id="v-pills-settings" role="tabpanel"
         aria-labelledby="v-pills-settings-tab" tabindex="0">
      <p>Settings panel content.</p>
    </div>
    <div class="tab-pane fade" id="v-pills-profile" role="tabpanel"
         aria-labelledby="v-pills-profile-tab" tabindex="0">
      <p>Profile panel content.</p>
    </div>
  </div>
</div>
```

Tabs with a dropdown and `nav-justified`:

```html
<ul class="nav nav-tabs nav-justified" role="tablist">
  <li class="nav-item" role="presentation">
    <a class="nav-link active" aria-current="page" href="#">Overview</a>
  </li>
  <li class="nav-item" role="presentation">
    <a class="nav-link" href="#">Specs</a>
  </li>
  <li class="nav-item dropdown" role="presentation">
    <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#"
       role="button" aria-expanded="false">Reviews</a>
    <ul class="dropdown-menu">
      <li><a class="dropdown-item" href="#">Latest</a></li>
      <li><a class="dropdown-item" href="#">Top Rated</a></li>
    </ul>
  </li>
</ul>
```

## Best Practices

1. Use `<button>` elements with `data-bs-toggle="tab"` or `data-bs-toggle="pill"` instead of `<a>` tags for non-navigational tab switching.
2. Always set `role="tablist"` on the `<ul>` and `role="presentation"` on each `<li>`.
3. Set `role="tab"` on each tab trigger and `role="tabpanel"` on each content pane.
4. Use `aria-controls` on tabs pointing to the `id` of their panel, and `aria-labelledby` on panels pointing back to the tab.
5. Include `aria-selected="true"` on the active tab and `aria-selected="false"` on inactive tabs.
6. Add `tabindex="0"` to active tab panels so they receive keyboard focus when navigated to.
7. Use `nav-fill` for equal-width tabs that scale with the container.
8. Use `nav-justified` when tabs must fill the entire width without scaling text.
9. Add `fade` and `show active` classes on the initial pane for smooth transitions.
10. Use `flex-column` for vertical pill layouts within a flex container.
11. Mark disabled tabs with both `disabled` class and `aria-disabled="true"`.

## Common Pitfalls

1. **Missing `data-bs-target` / `id` pairing** — The tab's `data-bs-target` must match the panel's `id`, or the panel will not show.
2. **Using `<a>` without `href="#"` for JS-driven tabs** — This can cause page jumps. Use `<button>` instead.
3. **Forgetting `tab-pane` class** — Without it, the panel will not be hidden/shown by Bootstrap's tab plugin.
4. **`fade` class without `show`** — The initial pane needs both `show active` to be visible on page load. Other panes need only `fade`.
5. **Dropdown inside tabs not closing siblings** — Bootstrap handles this, but custom JS interference can break it.
6. **Vertical pills missing `aria-orientation="vertical"`** — Screen readers will not announce the correct orientation.
7. **Overriding `nav-fill` and `nav-justified` simultaneously** — These classes conflict; use only one.
8. **Tab panels not inside `tab-content`** — Panels outside the `tab-content` wrapper will not function correctly with Bootstrap's JS.

## Accessibility Considerations

- `role="tablist"`, `role="tab"`, and `role="tabpanel"` create the ARIA tab pattern that screen readers understand.
- `aria-controls` and `aria-labelledby` create the bidirectional association between tabs and panels.
- `aria-selected` communicates the active tab to assistive technology.
- Keyboard navigation: `Arrow` keys move between tabs, `Tab` moves focus to the panel content. Bootstrap's tab plugin handles `Arrow` key navigation automatically.
- `tabindex="0"` on panels ensures keyboard users can tab into the active panel content after navigating the tab list.
- Disabled tabs should have `aria-disabled="true"` in addition to the `disabled` CSS class.

## Responsive Behavior

- **Tabs**: `nav-tabs` can overflow on small screens. Add `flex-nowrap overflow-auto` for horizontal scrolling, or switch to pills with `nav-pills` at a breakpoint using JavaScript.
- **`nav-fill`**: Distributes equal width to each tab. On very small screens, text may truncate. Consider shorter labels or icons.
- **`nav-justified`**: Forces full-width tabs. On mobile, this can produce very narrow tabs with wrapping text.
- **Vertical pills**: Stack well on mobile naturally. Use `flex-column` inside a responsive container to switch between horizontal on desktop and vertical on mobile.
- **Tab content**: Panels are always full-width and responsive by default. Content inside panels should use Bootstrap's grid for internal layout.
- Consider collapsing tabs into a dropdown or accordion pattern on very small screens for better mobile UX.
