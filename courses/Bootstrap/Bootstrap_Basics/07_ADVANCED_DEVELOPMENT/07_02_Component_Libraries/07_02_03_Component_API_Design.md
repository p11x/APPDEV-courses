---
title: "Component API Design"
difficulty: 3
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - Bootstrap 5 Component Options
  - Event-Driven Architecture
  - Slot/Template Patterns
---

## Overview

Component API design defines how developers interact with Bootstrap-based components through props, events, and slots. A well-designed API is intuitive, consistent across all components, and provides clear mental models for configuration. The API surface includes property interfaces for configuration, event systems for communication, and content projection patterns for flexibility.

For vanilla JavaScript Bootstrap components, the API translates to configuration objects, CustomEvent dispatch, and DOM query patterns. The goal is to make component usage predictable: every component accepts a config object, emits standard lifecycle events, and provides named slots for content injection.

Consistency is the primary design principle. If one component uses `onDismiss`, all dismissal callbacks should use `onDismiss`. If one component supports `data-bs-*` attributes, all should. This consistency reduces cognitive load and documentation needs.

## Basic Implementation

A standardized component API uses consistent property, event, and slot patterns.

```js
// Standard component API pattern
class ComponentBase {
  static Default = {};

  constructor(element, config = {}) {
    this._element = element;
    this._config = { ...this.constructor.Default, ...config };
    this._slots = this._parseSlots();
    this._state = {};
  }

  // Props: get/set with reactivity
  get(key) {
    return this._config[key];
  }

  set(key, value) {
    const oldValue = this._config[key];
    this._config[key] = value;
    this._emit('change', { key, value, oldValue });
    this._update();
  }

  // Slots: named content areas
  _parseSlots() {
    const slots = {};
    this._element.querySelectorAll('[slot]').forEach(el => {
      slots[el.getAttribute('slot')] = el;
    });
    return slots;
  }

  slot(name) {
    return this._slots[name] || null;
  }

  // Events: consistent event API
  _emit(eventName, detail = {}) {
    this._element.dispatchEvent(new CustomEvent(
      `${this.constructor.EVENT_KEY}${eventName}`,
      { bubbles: true, detail: { ...detail, component: this } }
    ));
  }

  on(eventName, handler) {
    this._element.addEventListener(
      `${this.constructor.EVENT_KEY}${eventName}`,
      handler
    );
    return this;
  }

  off(eventName, handler) {
    this._element.removeEventListener(
      `${this.constructor.EVENT_KEY}${eventName}`,
      handler
    );
    return this;
  }
}
```

```html
<!-- Slot-based component usage -->
<div data-component="modal" data-bs-title="Confirm">
  <template slot="header">
    <h5 class="modal-title">Custom Header</h5>
  </template>
  <template slot="body">
    <p>Are you sure you want to proceed?</p>
  </template>
  <template slot="footer">
    <button class="btn btn-secondary">Cancel</button>
    <button class="btn btn-primary">Confirm</button>
  </template>
</div>
```

```js
// Event-driven component API
const modal = Modal.getOrCreateInstance('#confirmModal');

modal
  .on('show', (e) => {
    console.log('Modal is about to show');
  })
  .on('shown', (e) => {
    console.log('Modal is fully visible');
  })
  .on('hide', (e) => {
    console.log('Modal is about to hide');
  })
  .on('hidden', (e) => {
    console.log('Modal is fully hidden');
  });

// Programmatic control
modal.show();
modal.set('title', 'Updated Title');
modal.set('backdrop', 'static');
```

## Advanced Variations

```js
// Advanced: Computed properties and watchers
class ReactiveComponent extends ComponentBase {
  constructor(element, config) {
    super(element, config);
    this._computed = {};
    this._watchers = [];
    this._setupComputed();
  }

  // Computed properties
  defineComputed(name, fn) {
    Object.defineProperty(this, name, {
      get: () => fn(this._config, this._state),
      enumerable: true
    });
    this._computed[name] = fn;
  }

  // Property watchers
  watch(key, callback) {
    this._watchers.push({ key, callback });
    return this;
  }

  set(key, value) {
    const oldValue = this._config[key];
    if (oldValue === value) return;

    this._config[key] = value;

    // Run watchers
    this._watchers
      .filter(w => w.key === key)
      .forEach(w => w.callback(value, oldValue, this));

    this._emit('change', { key, value, oldValue });
    this._update();
  }
}

// Usage with watchers
const alert = new AlertComponent(el, { type: 'info', message: 'Hello' });

alert
  .watch('type', (newVal, oldVal) => {
    el.classList.remove(`alert-${oldVal}`);
    el.classList.add(`alert-${newVal}`);
  })
  .watch('message', (newVal) => {
    el.querySelector('.alert-text').textContent = newVal;
  });

alert.set('type', 'danger');
alert.set('message', 'Error occurred');
```

## Best Practices

1. **Use consistent prop naming** - Same names across all components: `variant`, `size`, `disabled`, `visible`.
2. **Provide sensible defaults** - Every prop should have a default that makes the component work out of the box.
3. **Emit lifecycle events** - `show`, `shown`, `hide`, `hidden` for every component with visibility states.
4. **Support data attributes** - Mirror every config option as `data-bs-*` attribute for declarative usage.
5. **Use slots for flexible content** - Named slots for header, body, footer, and actions areas.
6. **Validate prop types** - Throw descriptive errors when props receive wrong types.
7. **Document events with payloads** - Specify the `detail` shape for every CustomEvent.
8. **Make events cancelable** - Allow `preventDefault()` on `show` and `hide` events.
9. **Support method chaining** - Return `this` from state-changing methods.
10. **Provide getter/setter pairs** - Allow both `modal.title = 'X'` and `modal.set('title', 'X')`.

## Common Pitfalls

1. **Inconsistent event names** - Using `onShow` in one component and `onBeforeOpen` in another.
2. **Missing event details** - Events without relevant `detail` data force consumers to query the DOM.
3. **No prop validation** - Accepting any value leads to silent failures and hard-to-debug issues.
4. **Forgetting data attribute sync** - Props set via JS don't update `data-*` attributes, breaking serialization.
5. **Tight coupling** - Components referencing other components directly instead of using events.

## Accessibility Considerations

API design must expose accessibility-relevant configuration as first-class props, not afterthoughts.

```js
// Accessibility-first API design
class AccessibleModal extends ComponentBase {
  static Default = {
    title: '',
    ariaLabel: null,
    ariaLabelledBy: null,
    ariaDescribedBy: null,
    role: 'dialog',
    ariaModal: true,
    returnFocus: true,
    focusTrap: true
  };

  _render() {
    const id = `modal-${Date.now()}`;
    const labelledBy = this._config.ariaLabelledBy || `${id}-title`;
    const describedBy = this._config.ariaDescribedBy || `${id}-desc`;

    return `
      <div class="modal"
           role="${this._config.role}"
           aria-labelledby="${labelledBy}"
           aria-describedby="${describedBy}"
           ${this._config.ariaModal ? 'aria-modal="true"' : ''}
           tabindex="-1">
        <!-- ... -->
      </div>
    `;
  }
}
```

## Responsive Behavior

Component APIs should expose responsive configuration options that adapt behavior across breakpoints.

```js
// Responsive API pattern
class ResponsiveComponent extends ComponentBase {
  static Default = {
    layout: 'horizontal',       // default layout
    layoutSm: null,             // override at sm breakpoint
    layoutMd: null,             // override at md breakpoint
    layoutLg: null,             // override at lg breakpoint
    collapseBelow: null         // collapse below breakpoint
  };

  _getEffectiveLayout() {
    const width = window.innerWidth;
    const breakpoints = { sm: 576, md: 768, lg: 992, xl: 1200 };

    let layout = this._config.layout;
    if (width >= breakpoints.sm && this._config.layoutSm) layout = this._config.layoutSm;
    if (width >= breakpoints.md && this._config.layoutMd) layout = this._config.layoutMd;
    if (width >= breakpoints.lg && this._config.layoutLg) layout = this._config.layoutLg;

    return layout;
  }
}
```
