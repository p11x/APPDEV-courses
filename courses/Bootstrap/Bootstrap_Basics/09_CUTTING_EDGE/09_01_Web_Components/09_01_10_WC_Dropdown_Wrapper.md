---
title: Web Component Dropdown Wrapper
category: [Web Components, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, web-components, custom-elements, dropdown, slots
---

## Overview

A dropdown web component encapsulates Bootstrap's dropdown behavior inside a custom element with a declarative API and slot-based item composition. This enables framework-agnostic dropdowns with clean markup, keyboard navigation, and event-driven integration.

## Basic Implementation

```html
<bs-dropdown label="Actions" variant="primary">
  <bs-dropdown-item value="edit">Edit</bs-dropdown-item>
  <bs-dropdown-item value="duplicate">Duplicate</bs-dropdown-item>
  <bs-dropdown-divider></bs-dropdown-divider>
  <bs-dropdown-item value="delete" danger>Delete</bs-dropdown-item>
</bs-dropdown>

<script>
class BsDropdown extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._open = false;
  }

  connectedCallback() {
    this.render();
    this._setupListeners();
  }

  render() {
    const label = this.getAttribute('label') || 'Dropdown';
    const variant = this.getAttribute('variant') || 'primary';

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <div class="dropdown">
        <button class="btn btn-${variant} dropdown-toggle"
                data-bs-toggle="dropdown" aria-expanded="false">
          ${label}
        </button>
        <ul class="dropdown-menu" role="menu">
          <slot></slot>
        </ul>
      </div>
    `;
  }

  _setupListeners() {
    const btn = this.shadowRoot.querySelector('button');
    const menu = this.shadowRoot.querySelector('.dropdown-menu');

    btn.addEventListener('click', () => {
      this._open = !this._open;
      menu.classList.toggle('show', this._open);
      btn.setAttribute('aria-expanded', this._open);
    });

    document.addEventListener('click', (e) => {
      if (!this.contains(e.target)) {
        this._open = false;
        menu.classList.remove('show');
        btn.setAttribute('aria-expanded', 'false');
      }
    });
  }
}
customElements.define('bs-dropdown', BsDropdown);

class BsDropdownItem extends HTMLElement {
  connectedCallback() {
    const danger = this.hasAttribute('danger');
    this.setAttribute('role', 'menuitem');
    this.style.display = 'block';
    this.classList.add('dropdown-item');
    if (danger) this.classList.add('text-danger');
    this.addEventListener('click', () => {
      this.closest('bs-dropdown').dispatchEvent(
        new CustomEvent('bs-select', {
          bubbles: true, composed: true,
          detail: { value: this.getAttribute('value') }
        })
      );
    });
  }
}
customElements.define('bs-dropdown-item', BsDropdownItem);

class BsDropdownDivider extends HTMLElement {
  connectedCallback() {
    this.classList.add('dropdown-divider');
    this.setAttribute('role', 'separator');
  }
}
customElements.define('bs-dropdown-divider', BsDropdownDivider);
</script>
```

## Advanced Variations

Adding a declarative API with reactive attributes and programmatic control.

```html
<bs-dropdown label="Filter" variant="outline-secondary" position="end" open>
  <bs-dropdown-item value="all">All Items</bs-dropdown-item>
  <bs-dropdown-item value="active">Active</bs-dropdown-item>
  <bs-dropdown-item value="archived">Archived</bs-dropdown-item>
</bs-dropdown>

<script>
class BsDropdown extends HTMLElement {
  static get observedAttributes() {
    return ['label', 'variant', 'position', 'open', 'disabled'];
  }

  get open() { return this.hasAttribute('open'); }
  set open(v) { v ? this.setAttribute('open', '') : this.removeAttribute('open'); }

  attributeChangedCallback(name, old, val) {
    if (old !== val && this.shadowRoot) this._update();
  }

  _update() {
    const menu = this.shadowRoot.querySelector('.dropdown-menu');
    const btn = this.shadowRoot.querySelector('button');
    if (!menu || !btn) return;

    const position = this.getAttribute('position') || 'start';
    menu.classList.toggle('dropdown-menu-end', position === 'end');
    menu.classList.toggle('show', this.open);
    btn.setAttribute('aria-expanded', this.open);
    btn.disabled = this.hasAttribute('disabled');
  }
}
</script>
```

Keyboard navigation within the dropdown menu.

```html
<script>
class BsDropdown extends HTMLElement {
  _setupKeyboard() {
    const menu = this.shadowRoot.querySelector('.dropdown-menu');
    const slot = this.shadowRoot.querySelector('slot');
    const getItems = () =>
      slot.assignedElements().filter(el => el.tagName === 'BS-DROPDOWN-ITEM');

    this.shadowRoot.querySelector('button').addEventListener('keydown', (e) => {
      if (e.key === 'ArrowDown') {
        e.preventDefault();
        this.open = true;
        getItems()[0]?.focus();
      }
    });

    menu.addEventListener('keydown', (e) => {
      const items = getItems();
      const idx = items.indexOf(document.activeElement);
      if (e.key === 'ArrowDown') {
        e.preventDefault();
        items[(idx + 1) % items.length]?.focus();
      } else if (e.key === 'ArrowUp') {
        e.preventDefault();
        items[(idx - 1 + items.length) % items.length]?.focus();
      } else if (e.key === 'Escape') {
        this.open = false;
        this.shadowRoot.querySelector('button').focus();
      }
    });
  }
}
</script>
```

## Best Practices

1. Use `<slot>` for item composition so consumers control dropdown content declaratively
2. Dispatch `bs-select` events with the selected item's `value` in `detail`
3. Support keyboard navigation with Arrow Up/Down, Escape, and Enter keys
4. Close dropdown when clicking outside by listening on `document`
5. Reflect `open` attribute for programmatic control and CSS state styling
6. Use `aria-expanded` and `role="menu"` for screen reader compatibility
7. Separate item, divider, and header as distinct custom elements for clean composition
8. Support `position="end"` attribute for right-aligned dropdowns
9. Prevent event propagation from items to avoid double-close behavior
10. Use `composed: true` on events to cross Shadow DOM boundaries
11. Provide `disabled` attribute support on both host and items
12. Clean up document-level listeners in `disconnectedCallback`

## Common Pitfalls

1. **Slot content not styled** — Slotted items need Bootstrap CSS applied within Shadow DOM or via `:host` context
2. **Missing outside-click handler** — Dropdown stays open when clicking elsewhere
3. **No keyboard support** — Dropdown becomes inaccessible to keyboard-only users
4. **Event not composed** — Consumer cannot listen for `bs-select` outside shadow root
5. **Items not focusable** — Without `tabindex` on items, keyboard nav fails
6. **Multiple dropdowns interfering** — Document click handlers close all dropdowns, not just the target
7. **Not handling `disabled` items** — Click events fire on disabled items
8. **Re-render losing state** — Shadow DOM `innerHTML` replacement destroys open state

## Accessibility Considerations

Apply `role="menu"` to the dropdown list and `role="menuitem"` to items. Use `aria-expanded` on the trigger button. Support `aria-haspopup="true"` on the button. Ensure items are focusable with `tabindex="-1"` and managed via arrow key navigation. Announce the dropdown label to screen readers using `aria-labelledby`.

## Responsive Behavior

Use the `position` attribute to switch alignment at different screen sizes. On mobile, consider rendering dropdowns as full-width bottom sheets using a `mobile-sheet` attribute. Apply Bootstrap's `d-grid` and `w-100` classes via the host element for full-width dropdowns on small viewports.
