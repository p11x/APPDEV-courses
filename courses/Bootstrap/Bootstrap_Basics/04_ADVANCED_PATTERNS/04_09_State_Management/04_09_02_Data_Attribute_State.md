---
title: "Data Attribute State Management"
description: "Using data-* attributes for component state, reading and writing state via JavaScript with Bootstrap"
difficulty: 2
tags: ["state-management", "data-attributes", "dom", "bootstrap"]
prerequisites: ["04_09_01_Component_State_CSS"]
---

## Overview

Data attributes (`data-*`) store component state directly in the DOM, making state visible, queryable, and styleable. Bootstrap already uses this pattern extensively — `data-bs-toggle`, `data-bs-target`, and `data-bs-dismiss` drive plugin behavior. Extending this pattern with custom `data-*` attributes creates a consistent state management approach that works with CSS selectors, JavaScript queries, and server-rendered HTML.

Data attribute state is ideal for simple UI state: active tabs, filter selections, sort order, expanded sections, and component configuration. It avoids the complexity of external state stores for state that belongs to a single component.

## Basic Implementation

```html
<!-- Data attribute state for a filter toolbar -->
<div class="btn-group" role="group" data-filter-state="all">
  <input type="radio" class="btn-check" name="filter" id="filterAll"
         data-filter="all" checked>
  <label class="btn btn-outline-primary" for="filterAll">All</label>

  <input type="radio" class="btn-check" name="filter" id="filterActive"
         data-filter="active">
  <label class="btn btn-outline-primary" for="filterActive">Active</label>

  <input type="radio" class="btn-check" name="filter" id="filterArchived"
         data-filter="archived">
  <label class="btn btn-outline-primary" for="filterArchived">Archived</label>
</div>

<div class="row g-3 mt-3">
  <div class="col-md-4" data-status="active">
    <div class="card"><div class="card-body">Active Item</div></div>
  </div>
  <div class="col-md-4" data-status="archived">
    <div class="card"><div class="card-body">Archived Item</div></div>
  </div>
  <div class="col-md-4" data-status="active">
    <div class="card"><div class="card-body">Active Item</div></div>
  </div>
</div>
```

```js
// Reading and writing data attribute state
class DataState {
  static get(element, key) {
    return element.dataset[key];
  }

  static set(element, key, value) {
    element.dataset[key] = value;
    element.dispatchEvent(new CustomEvent('statechange', {
      detail: { key, value },
      bubbles: true
    }));
  }

  static getAll(container, selector) {
    return Array.from(container.querySelectorAll(selector)).map(el => ({
      element: el,
      state: { ...el.dataset }
    }));
  }
}

// Filter implementation using data attributes
document.querySelectorAll('[data-filter]').forEach(radio => {
  radio.addEventListener('change', () => {
    const filter = radio.dataset.filter;
    const container = document.querySelector('[data-filter-state]');
    container.dataset.filterState = filter;

    document.querySelectorAll('[data-status]').forEach(card => {
      if (filter === 'all' || card.dataset.status === filter) {
        card.classList.remove('d-none');
      } else {
        card.classList.add('d-none');
      }
    });
  });
});

// React to state changes
document.addEventListener('statechange', (e) => {
  console.log(`State changed: ${e.detail.key} = ${e.detail.value}`);
});
```

## Advanced Variations

```html
<!-- Complex state: multi-select with data attributes -->
<div class="card" data-component="tag-selector" data-state='{"selected":[],"max":5}'>
  <div class="card-body">
    <h6>Select up to 5 tags:</h6>
    <div class="d-flex flex-wrap gap-2">
      <button class="btn btn-sm btn-outline-secondary" data-tag="javascript">JavaScript</button>
      <button class="btn btn-sm btn-outline-secondary" data-tag="css">CSS</button>
      <button class="btn btn-sm btn-outline-secondary" data-tag="bootstrap">Bootstrap</button>
      <button class="btn btn-sm btn-outline-secondary" data-tag="react">React</button>
      <button class="btn btn-sm btn-outline-secondary" data-tag="node">Node.js</button>
      <button class="btn btn-sm btn-outline-secondary" data-tag="typescript">TypeScript</button>
    </div>
    <div class="mt-2">
      <small class="text-muted" data-role="selection-count">0 of 5 selected</small>
    </div>
  </div>
</div>
```

```js
// State manager for data attribute components
class ComponentStateManager {
  constructor(element) {
    this.element = element;
    this.state = JSON.parse(element.dataset.state || '{}');
    this.listeners = new Map();
  }

  get(key) {
    return this.state[key];
  }

  set(key, value) {
    this.state[key] = value;
    this.element.dataset.state = JSON.stringify(this.state);
    this.emit(key, value);
  }

  on(key, callback) {
    if (!this.listeners.has(key)) this.listeners.set(key, []);
    this.listeners.get(key).push(callback);
  }

  emit(key, value) {
    (this.listeners.get(key) || []).forEach(cb => cb(value));
    this.element.dispatchEvent(new CustomEvent('datastate', {
      detail: { key, value, state: this.state },
      bubbles: true
    }));
  }
}

// Tag selector implementation
const tagSelector = document.querySelector('[data-component="tag-selector"]');
const manager = new ComponentStateManager(tagSelector);

tagSelector.querySelectorAll('[data-tag]').forEach(btn => {
  btn.addEventListener('click', () => {
    const tag = btn.dataset.tag;
    const selected = manager.get('selected');
    const max = manager.get('max');

    if (selected.includes(tag)) {
      manager.set('selected', selected.filter(t => t !== tag));
      btn.classList.remove('btn-primary');
      btn.classList.add('btn-outline-secondary');
    } else if (selected.length < max) {
      manager.set('selected', [...selected, tag]);
      btn.classList.remove('btn-outline-secondary');
      btn.classList.add('btn-primary');
    }
  });
});

manager.on('selected', (selected) => {
  const countEl = tagSelector.querySelector('[data-role="selection-count"]');
  countEl.textContent = `${selected.length} of ${manager.get('max')} selected`;
});
```

## Best Practices

1. Use `data-*` attributes for state that belongs to a single DOM element
2. Use `dataset` API (`element.dataset.key`) to read/write state in JavaScript
3. Dispatch custom events on state change so other components can react
4. Store complex state as JSON in a single `data-state` attribute
5. Use data attributes for initial state rendered by the server
6. Query state with CSS attribute selectors: `[data-status="active"]`
7. Keep state attribute names descriptive: `data-sort-order` not `data-so`
8. Use `data-state` consistently across components for predictability
9. Validate data attribute values before using them in logic
10. Document the expected data attributes for each component type

## Common Pitfalls

1. **Storing large data in attributes** — Data attributes are strings; large JSON objects become unwieldy and impact DOM size
2. **Not escaping JSON values** — Special characters in JSON strings break HTML parsing; use proper escaping
3. **State not synced with UI** — Changing `data-state` without updating visual appearance creates inconsistency
4. **Over-reliance on dataset** — Complex state with many interdependencies belongs in a state object, not scattered attributes
5. **Missing state change events** — Silent state changes prevent other components from reacting
6. **CSS selector specificity issues** — Attribute selectors `[data-state="active"]` have low specificity and can be overridden

## Accessibility Considerations

Data attribute state should be reflected in ARIA attributes. When `data-active="true"` marks an active item, also set `aria-selected="true"`. Use data attributes to store configuration but ensure ARIA state is the source of truth for assistive technology. Avoid using data attributes as the sole indicator of interactive state.

## Responsive Behavior

Data attribute state is viewport-independent by nature. However, the UI representation of state may need responsive adjustments. A `data-sort="desc"` attribute works the same on mobile and desktop, but the sort indicator icon may need different sizing. Use Bootstrap's responsive utilities alongside data attributes for viewport-specific display.
