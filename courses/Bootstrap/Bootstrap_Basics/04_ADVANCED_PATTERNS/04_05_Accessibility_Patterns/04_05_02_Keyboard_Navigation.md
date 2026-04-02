---
title: "Keyboard Navigation in Bootstrap"
module: "04_ADVANCED_PATTERNS"
lesson: "04_05_Accessibility_Patterns"
file: "04_05_02_Keyboard_Navigation.md"
difficulty: 2
description: "Tab order, focus management, Enter/Space activation, Escape dismissal, arrow key navigation in tabs/dropdowns/carousels"
---

## Overview

Keyboard navigation is fundamental to web accessibility. Users who cannot use a mouse, including those with motor disabilities and screen reader users, rely entirely on keyboard input. Bootstrap provides built-in keyboard support for its interactive components, but understanding the conventions allows you to maintain accessibility when customizing or building new components.

Core keyboard interaction patterns:

| Key | Expected Behavior | Common Components |
|-----|------------------|-------------------|
| `Tab` | Move focus to next interactive element | All focusable elements |
| `Shift+Tab` | Move focus to previous interactive element | All focusable elements |
| `Enter` / `Space` | Activate buttons, links, checkboxes | Buttons, links, checkboxes |
| `Escape` | Dismiss modals, dropdowns, popovers | Modal, dropdown, popover |
| `Arrow Up/Down` | Navigate within lists, tabs, menus | Tabs, dropdowns, accordion |
| `Arrow Left/Right` | Navigate horizontal tabs, carousel slides | Tabs, carousel |
| `Home` / `End` | Jump to first/last item in group | Tabs, list groups |

## Basic Implementation

### Dropdown Keyboard Navigation

Bootstrap's dropdown supports `Enter`, `Space`, `Escape`, and arrow keys out of the box. No additional code is needed for standard dropdowns:

```html
<div class="dropdown">
  <button class="btn btn-secondary dropdown-toggle" type="button"
          data-bs-toggle="dropdown" aria-expanded="false">
    Keyboard Dropdown
  </button>
  <ul class="dropdown-menu">
    <li><a class="dropdown-item" href="#">First option</a></li>
    <li><a class="dropdown-item" href="#">Second option</a></li>
    <li><a class="dropdown-item" href="#">Third option</a></li>
    <li><hr class="dropdown-divider"></li>
    <li><a class="dropdown-item" href="#">Separated option</a></li>
  </ul>
</div>
```

Navigation: `Enter`/`Space` opens the menu, `Arrow Up/Down` moves between items, `Enter` selects, `Escape` closes.

### Button Activation with Enter and Space

```html
<!-- Buttons respond to both Enter and Space natively -->
<button class="btn btn-primary" type="button" onclick="submitForm()">Submit</button>

<!-- Custom interactive element needs manual key handling -->
<div class="custom-switch" role="switch" aria-checked="false" tabindex="0"
     onclick="toggleSwitch(this)" onkeydown="handleSwitchKey(event, this)">
  <span class="switch-track"></span>
  <span class="switch-thumb"></span>
  <span class="visually-hidden">Enable notifications</span>
</div>

<script>
function handleSwitchKey(event, el) {
  if (event.key === 'Enter' || event.key === ' ') {
    event.preventDefault();
    toggleSwitch(el);
  }
}
</script>
```

```css
.custom-switch:focus-visible {
  outline: 2px solid var(--bs-primary);
  outline-offset: 2px;
  border-radius: 1rem;
}
```

### Escape Key to Dismiss Alerts

```html
<div class="alert alert-info alert-dismissible" role="alert" id="dismissibleAlert">
  Press Escape to dismiss this alert, or use the close button.
  <button type="button" class="btn-close" data-bs-dismiss="alert"
          aria-label="Close"></button>
</div>

<script>
document.addEventListener('keydown', function(event) {
  if (event.key === 'Escape') {
    const alert = document.getElementById('dismissibleAlert');
    if (alert) {
      const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
      bsAlert.close();
    }
  }
});
</script>
```

## Advanced Variations

### Tab Component with Arrow Key Navigation

Bootstrap tabs handle arrow key navigation automatically when structured correctly:

```html
<ul class="nav nav-pills" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="home-tab" data-bs-toggle="pill"
            data-bs-target="#home" type="button" role="tab"
            aria-controls="home" aria-selected="true">
      Home
    </button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="profile-tab" data-bs-toggle="pill"
            data-bs-target="#profile" type="button" role="tab"
            aria-controls="profile" aria-selected="false" tabindex="-1">
      Profile
    </button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="contact-tab" data-bs-toggle="pill"
            data-bs-target="#contact" type="button" role="tab"
            aria-controls="contact" aria-selected="false" tabindex="-1">
      Contact
    </button>
  </li>
</ul>
<div class="tab-content">
  <div class="tab-pane fade show active" id="home" role="tabpanel" tabindex="0">
    Home content
  </div>
  <div class="tab-pane fade" id="profile" role="tabpanel" tabindex="0">
    Profile content
  </div>
  <div class="tab-pane fade" id="contact" role="tabpanel" tabindex="0">
    Contact content
  </div>
</div>
```

### Custom Keyboard Navigation for List Group

```html
<div class="list-group" role="listbox" aria-label="Select a color"
     id="colorList" tabindex="0">
  <button type="button" class="list-group-item list-group-item-action active"
          role="option" aria-selected="true">Red</button>
  <button type="button" class="list-group-item list-group-item-action"
          role="option" aria-selected="false">Blue</button>
  <button type="button" class="list-group-item list-group-item-action"
          role="option" aria-selected="false">Green</button>
  <button type="button" class="list-group-item list-group-item-action"
          role="option" aria-selected="false">Yellow</button>
</div>

<script>
const list = document.getElementById('colorList');
const items = [...list.querySelectorAll('[role="option"]')];

list.addEventListener('keydown', (e) => {
  const currentIndex = items.findIndex(el => el === document.activeElement);
  let newIndex;

  switch (e.key) {
    case 'ArrowDown':
      e.preventDefault();
      newIndex = currentIndex < items.length - 1 ? currentIndex + 1 : 0;
      items[newIndex].focus();
      break;
    case 'ArrowUp':
      e.preventDefault();
      newIndex = currentIndex > 0 ? currentIndex - 1 : items.length - 1;
      items[newIndex].focus();
      break;
    case 'Home':
      e.preventDefault();
      items[0].focus();
      break;
    case 'End':
      e.preventDefault();
      items[items.length - 1].focus();
      break;
  }
});
</script>
```

### Carousel with Keyboard Support

```html
<div id="carouselExample" class="carousel slide" data-bs-ride="carousel"
     data-bs-keyboard="true">
  <div class="carousel-inner">
    <div class="carousel-item active">
      <div class="d-flex align-items-center justify-content-center bg-light"
           style="height: 200px;">Slide 1</div>
    </div>
    <div class="carousel-item">
      <div class="d-flex align-items-center justify-content-center bg-secondary text-white"
           style="height: 200px;">Slide 2</div>
    </div>
  </div>
  <button class="carousel-control-prev" type="button"
          data-bs-target="#carouselExample" data-bs-slide="prev">
    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Previous</span>
  </button>
  <button class="carousel-control-next" type="button"
          data-bs-target="#carouselExample" data-bs-slide="next">
    <span class="carousel-control-next-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Next</span>
  </button>
</div>
```

## Best Practices

1. **Never remove focus outlines without providing an alternative** - If you remove `outline: none` on `:focus`, replace it with a visible `:focus-visible` style that meets WCAG 2.1 focus indicator requirements (minimum 3:1 contrast, 2px minimum thickness).
2. **Use `tabindex="0"` for custom interactive elements** - This places elements in the natural tab order. Never use positive `tabindex` values as they create confusing tab sequences.
3. **Use `tabindex="-1"` to remove elements from tab order** - Useful for tab panel content that should only be reached via tab panel activation.
4. **Support both Enter and Space for button activation** - Custom buttons (`role="button"`) must handle `keydown` for both keys, not just one.
5. **Trap focus inside modals** - When a modal opens, `Tab` and `Shift+Tab` should cycle through focusable elements within the modal only.
6. **Restore focus after modal closes** - Return focus to the element that triggered the modal, not to the document body.
7. **Support arrow keys in composite widgets** - Tabs, toolbars, menus, and listboxes should support `ArrowUp`, `ArrowDown`, `ArrowLeft`, `ArrowRight` for internal navigation.
8. **Ensure Home and End keys work in long lists** - Allow quick navigation to the first and last items in tabs, listboxes, and menus.
9. **Make all interactive elements focusable** - Ensure every element that responds to clicks also responds to keyboard input.
10. **Test keyboard navigation in both directions** - Verify that `Shift+Tab` reverses through the same elements as `Tab` in forward order.
11. **Avoid keyboard traps** - Users must be able to navigate away from any component using `Tab` or `Escape`.
12. **Document custom keyboard shortcuts** - If your application uses non-standard keyboard interactions, provide a help dialog listing them.

## Common Pitfalls

1. **Removing focus outlines with `outline: none`** - This is the most common accessibility violation. It makes keyboard navigation completely invisible for sighted keyboard users.
2. **Using `<div>` or `<span>` as buttons without keyboard support** - Click handlers alone do not provide `Enter`/`Space` activation. Always add `tabindex`, `role`, and `keydown` handlers, or simply use a `<button>`.
3. **Positive tabindex values** - Setting `tabindex="1"` or higher creates unpredictable tab order that changes as the page is updated. Always use `0` or `-1`.
4. **Not trapping focus in modals** - When a modal is open, keyboard users can still tab to elements behind the modal, breaking the expected interaction pattern.
5. **Forgetting to restore focus after dismissals** - After closing a modal or dropdown, focus must return to the triggering element. Without this, focus jumps to the top of the page.
6. **Keyboard-invisible custom controls** - Custom checkboxes, radio buttons, and switches built with `<div>` elements are not focusable by default. Use `role` and `tabindex="0"`, or better yet, visually hide the native input and style its label.
7. **Missing Escape key support** - Users expect `Escape` to close popups, modals, dropdowns, and overlays. Omitting this creates a keyboard trap.
8. **Arrow keys not working in expected contexts** - In tab panels, listboxes, and menus, arrow keys are expected to navigate between items. If they don't work, users must `Tab` through every item, which is inefficient.

## Accessibility Considerations

### Focus Indicator Styles

Bootstrap 5 includes `:focus-visible` support. Customize focus indicators to match your design while maintaining visibility:

```css
/* Enhanced focus indicator with better contrast */
.form-control:focus-visible,
.form-select:focus-visible,
.btn:focus-visible {
  outline: 3px solid var(--bs-primary);
  outline-offset: 2px;
  box-shadow: none;
}

/* Ensure focus is visible on dark backgrounds */
.btn-outline-light:focus-visible {
  outline-color: #fff;
  outline-offset: 2px;
}
```

### Skip Navigation Link

```html
<body>
  <a href="#main-content" class="visually-hidden-focusable">Skip to main content</a>
  <nav class="navbar navbar-expand-lg">...</nav>
  <main id="main-content" tabindex="-1">...</main>
</body>

<style>
/* Make skip link visible on focus */
.visually-hidden-focusable:not(:focus):not(:focus-within) {
  position: absolute !important;
  width: 1px !important;
  height: 1px !important;
  padding: 0 !important;
  margin: -1px !important;
  overflow: hidden !important;
  clip: rect(0, 0, 0, 0) !important;
  white-space: nowrap !important;
  border: 0 !important;
}
</style>
```

## Responsive Behavior

Keyboard navigation patterns remain consistent across viewport sizes, but the interaction context changes on touch devices:

- **Touch devices** do not have traditional tab navigation; however, external keyboards connected to tablets still require proper tab order and focus indicators.
- **Collapsible navigation** - When the navbar collapses into a hamburger menu on mobile, keyboard users need `Tab` to reach the toggle, `Enter`/`Space` to open it, and `Escape` to close it. Bootstrap handles this automatically.
- **Offcanvas panels** - On mobile, offcanvas replaces the traditional sidebar. Bootstrap traps focus within the offcanvas when open and restores it on close.
- **Responsive tables** - Horizontally scrolling tables need `tabindex="0"` on the scroll container to allow keyboard scrolling with arrow keys.

```html
<div class="table-responsive" tabindex="0" role="region" aria-label="Scrollable data table">
  <table class="table">...</table>
</div>
```

Test keyboard navigation at multiple viewport widths to ensure collapsing layouts do not create hidden focusable elements or break tab order.
