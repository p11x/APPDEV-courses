---
title: "Focus Management in Bootstrap"
module: "04_ADVANCED_PATTERNS"
lesson: "04_05_Accessibility_Patterns"
file: "04_05_04_Focus_Management.md"
difficulty: 3
description: "Focus trap in modals, focus-visible vs focus, outline styling, focus restoration after modal close, roving tabindex"
---

## Overview

Focus management is one of the most challenging aspects of web accessibility. It determines which element receives keyboard input and how focus moves through interactive components. Bootstrap handles focus management for modals and offcanvas components automatically, but custom components and edge cases require manual implementation.

Key focus management concepts:

| Concept | Description | Bootstrap Relevance |
|---------|-------------|-------------------|
| Focus trap | Restricting focus to a container | Modal, offcanvas |
| Focus restoration | Returning focus to trigger after close | Modal, dropdown, popover |
| `:focus-visible` | Showing focus only for keyboard users | All interactive elements |
| Roving tabindex | Moving tabindex between items in a group | Tabs, toolbars, menus |
| `focus()` method | Programmatically setting focus | Custom components |

## Basic Implementation

### Focus-visible Styling

Bootstrap 5 uses `:focus-visible` to show focus indicators only for keyboard navigation, not mouse clicks:

```css
/* Default Bootstrap focus-visible behavior */
.form-control:focus-visible {
  outline: 0;
  box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
}

/* Custom enhanced focus indicator */
.btn:focus-visible {
  outline: 3px solid var(--bs-primary);
  outline-offset: 2px;
}

/* Remove default focus ring and provide custom */
.form-check-input:focus-visible {
  outline: none;
  box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
}
```

### Focus Restoration After Bootstrap Modal

Bootstrap manages this automatically. Understanding the mechanism helps when building custom overlays:

```html
<!-- Trigger button -->
<button class="btn btn-primary" id="openModalBtn" data-bs-toggle="modal"
        data-bs-target="#myModal">
  Open Settings
</button>

<!-- Modal -->
<div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalTitle"
     aria-modal="true" role="dialog">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="modalTitle">Settings</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"
                aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <label for="settingInput" class="form-label">Setting value</label>
        <input type="text" class="form-control" id="settingInput">
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary">Save</button>
      </div>
    </div>
  </div>
</div>

<!-- Bootstrap automatically:
     1. Moves focus to the modal on open
     2. Traps Tab/Shift+Tab inside the modal
     3. Returns focus to #openModalBtn on close -->
```

### Focus Trap Implementation for Custom Overlay

```html
<div class="custom-overlay" id="customOverlay" role="dialog" aria-modal="true"
     aria-labelledby="overlayTitle" style="display:none">
  <h2 id="overlayTitle">Custom Panel</h2>
  <input type="text" placeholder="Search..." id="overlayInput">
  <button id="overlayClose">Close</button>
</div>

<script>
const overlay = document.getElementById('customOverlay');
const trigger = document.getElementById('openOverlayBtn');
let previousFocus;

function openOverlay() {
  previousFocus = document.activeElement;
  overlay.style.display = 'block';
  document.getElementById('overlayInput').focus();
  document.addEventListener('keydown', trapFocus);
}

function closeOverlay() {
  overlay.style.display = 'none';
  document.removeEventListener('keydown', trapFocus);
  previousFocus?.focus();
}

function trapFocus(e) {
  if (e.key !== 'Tab') {
    if (e.key === 'Escape') closeOverlay();
    return;
  }

  const focusable = overlay.querySelectorAll(
    'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
  );
  const first = focusable[0];
  const last = focusable[focusable.length - 1];

  if (e.shiftKey && document.activeElement === first) {
    e.preventDefault();
    last.focus();
  } else if (!e.shiftKey && document.activeElement === last) {
    e.preventDefault();
    first.focus();
  }
}
</script>
```

## Advanced Variations

### Roving Tabindex for Tab Component

Roving tabindex allows a group of items to share a single tab stop, with arrow keys navigating between them:

```html
<div class="btn-toolbar" role="toolbar" aria-label="Text formatting">
  <div class="btn-group me-2" role="group" aria-label="Text style">
    <button type="button" class="btn btn-outline-secondary active"
            aria-pressed="true" tabindex="0" data-group="style">
      <strong>B</strong>
    </button>
    <button type="button" class="btn btn-outline-secondary"
            aria-pressed="false" tabindex="-1" data-group="style">
      <em>I</em>
    </button>
    <button type="button" class="btn btn-outline-secondary"
            aria-pressed="false" tabindex="-1" data-group="style">
      <u>U</u>
    </button>
  </div>
</div>

<script>
document.querySelectorAll('[data-group="style"]').forEach(group => {
  const buttons = [...document.querySelectorAll(`[data-group="${group.dataset.group}"]`)];

  buttons.forEach(btn => {
    btn.addEventListener('keydown', (e) => {
      const currentIndex = buttons.indexOf(btn);
      let newIndex;

      switch (e.key) {
        case 'ArrowRight':
        case 'ArrowDown':
          e.preventDefault();
          newIndex = (currentIndex + 1) % buttons.length;
          break;
        case 'ArrowLeft':
        case 'ArrowUp':
          e.preventDefault();
          newIndex = (currentIndex - 1 + buttons.length) % buttons.length;
          break;
        case 'Home':
          e.preventDefault();
          newIndex = 0;
          break;
        case 'End':
          e.preventDefault();
          newIndex = buttons.length - 1;
          break;
        default:
          return;
      }

      buttons[currentIndex].setAttribute('tabindex', '-1');
      buttons[newIndex].setAttribute('tabindex', '0');
      buttons[newIndex].focus();
    });
  });
});
</script>
```

### Focus Management in SPA Navigation

```html
<!-- Announce route changes and manage focus -->
<main id="mainContent" tabindex="-1">
  <h1 id="pageTitle">Dashboard</h1>
  <!-- Page content -->
</main>

<script>
function navigateTo(url, title) {
  // Fetch content and update DOM
  fetch(url)
    .then(res => res.text())
    .then(html => {
      document.getElementById('mainContent').innerHTML = html;
      document.getElementById('pageTitle').textContent = title;
      document.title = title;

      // Move focus to main content area
      document.getElementById('mainContent').focus();

      // Update URL without reload
      history.pushState({}, title, url);
    });
}
</script>
```

### Nested Focus Management

```html
<!-- Main modal with nested dropdown -->
<div class="modal fade" id="complexModal" tabindex="-1" aria-modal="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Complex Form</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <!-- Dropdown inside modal - focus stays within modal -->
        <div class="dropdown mb-3">
          <button class="btn btn-secondary dropdown-toggle" type="button"
                  data-bs-toggle="dropdown">
            Select category
          </button>
          <ul class="dropdown-menu">
            <li><a class="dropdown-item" href="#">Category A</a></li>
            <li><a class="dropdown-item" href="#">Category B</a></li>
          </ul>
        </div>
        <input type="text" class="form-control" placeholder="Description">
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. **Always provide a visible focus indicator** - Every focusable element must have a clearly visible focus style that meets WCAG 2.1 minimum requirements (3:1 contrast ratio against adjacent colors).
2. **Use `:focus-visible` instead of `:focus` for styling** - This prevents focus outlines from appearing on mouse clicks while preserving them for keyboard navigation.
3. **Restore focus to the triggering element** - After closing a modal, dropdown, or popover, return focus to the element that opened it.
4. **Trap focus inside modals and dialogs** - When a modal is open, `Tab` and `Shift+Tab` must cycle only through elements within the modal.
5. **Use roving tabindex for groups of controls** - Toolbars, tab lists, and menu bars should use a single tab stop with arrow key navigation between items.
6. **Set initial focus on the first interactive element** - When opening a modal or panel, move focus to the first input, button, or interactive element within it.
7. **Never use `outline: none` without an alternative** - If you remove the default outline, provide an equally visible focus indicator using `box-shadow`, `border`, or `outline`.
8. **Use `tabindex="-1"` for programmatic focus** - Elements that need to receive focus via JavaScript but not via `Tab` should have `tabindex="-1"`.
9. **Test focus order matches visual order** - Focus should move through elements in a logical sequence that matches the visual layout.
10. **Manage focus on single-page app navigation** - When content changes without a page reload, move focus to the new content area or heading.
11. **Handle focus for dynamically inserted content** - When adding elements to the DOM, consider whether focus should move to the new element.
12. **Support both keyboard and mouse focus indicators** - While `:focus-visible` is preferred, ensure elements remain usable when focused via any input method.

## Common Pitfalls

1. **Removing all focus outlines with `*:focus { outline: none }`** - This blanket rule destroys keyboard accessibility across the entire site. Always target specific elements and provide alternatives.
2. **Not trapping focus in modals** - Users can tab behind the modal to interact with background content, breaking the modal's purpose and confusing the interaction model.
3. **Forgetting to restore focus on close** - Without focus restoration, keyboard users lose their position after closing a modal, and focus falls to the document body or top of the page.
4. **Using positive tabindex values** - `tabindex="1"` or higher creates a separate tab order that overrides natural document flow. This causes unpredictable navigation, especially as content changes.
5. **Focus indicator invisible on certain backgrounds** - A blue outline on a blue background fails contrast requirements. Test focus indicators on all background colors.
6. **Roving tabindex without keyboard handlers** - Setting `tabindex="-1"` on items without adding arrow key navigation makes those items unreachable by keyboard.
7. **Focus jumping on page load** - Auto-focusing an element on page load (e.g., `input.focus()`) can disorient screen reader users who expect to start from the top of the document.
8. **Inconsistent focus-visible support** - Older browsers may not support `:focus-visible`. Provide a fallback or use a polyfill.

## Accessibility Considerations

### WCAG Focus Requirements

WCAG 2.1 defines specific requirements for focus:

- **2.4.7 Focus Visible** (Level AA) - Any keyboard operable UI must have a mode of operation where the keyboard focus indicator is visible.
- **2.4.3 Focus Order** (Level A) - Focus order must preserve meaning and operability.
- **2.4.11 Focus Not Obscured (Minimum)** (Level AA) - When a UI component receives focus, it is not entirely hidden by author-created content.
- **1.4.11 Non-text Contrast** (Level AA) - Focus indicators must have at least 3:1 contrast against adjacent colors.

### Focus Indicator Styles

```css
/* Minimum accessible focus style */
:focus-visible {
  outline: 2px solid #0d6efd;
  outline-offset: 2px;
}

/* Enhanced focus for buttons */
.btn:focus-visible {
  outline: 3px solid rgba(13, 110, 253, 0.5);
  outline-offset: 2px;
}

/* Focus on dark backgrounds */
.btn-dark:focus-visible {
  outline-color: #fff;
}

/* High contrast mode support */
@media (forced-colors: active) {
  :focus-visible {
    outline: 2px solid LinkText;
  }
}
```

### Focus Debugging

```javascript
// Log focus changes for debugging
document.addEventListener('focusin', (e) => {
  console.log('Focus moved to:', e.target.tagName, e.target.className, e.target.id);
});

// Verify focus trap in modals
document.addEventListener('keydown', (e) => {
  if (e.key === 'Tab') {
    const modal = document.querySelector('.modal.show');
    if (modal) {
      const focusable = modal.querySelectorAll(
        'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
      );
      console.log('Focusable in modal:', focusable.length);
    }
  }
});
```

## Responsive Behavior

Focus management remains consistent across viewport sizes, but responsive layouts introduce specific challenges:

- **Hamburger menus** - When navigation collapses, the hamburger button becomes a critical focus target. Ensure it has clear focus styling and proper `aria-expanded` state.
- **Offcanvas panels** - Bootstrap's offcanvas component traps focus similarly to modals. On mobile, ensure the close button is immediately focusable after opening.
- **Responsive tables** - When tables require horizontal scrolling, add `tabindex="0"` to the scroll container so keyboard users can scroll with arrow keys.
- **Touch vs keyboard** - On mobile devices with external keyboards, focus management should work identically to desktop. Touch-only devices do not need focus indicators but do need proper ARIA states.
- **Viewport resizing** - When a responsive layout collapses columns, ensure the focus order still follows a logical reading sequence from top to bottom.

```css
/* Focus styles for responsive offcanvas */
.offcanvas:focus-visible {
  outline: none;
}

.offcanvas .btn-close:focus-visible {
  outline: 3px solid rgba(13, 110, 253, 0.5);
  outline-offset: 2px;
}

/* Ensure scrollable regions are keyboard accessible */
.table-responsive:focus-visible {
  outline: 2px solid var(--bs-primary);
  outline-offset: -2px;
}
```
