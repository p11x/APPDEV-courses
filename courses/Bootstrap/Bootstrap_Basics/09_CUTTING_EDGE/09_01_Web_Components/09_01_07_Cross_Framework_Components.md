---
title: "Cross-Framework Components"
description: "Building framework-agnostic Bootstrap web components that interoperate with React, Vue, Angular, and Svelte"
difficulty: 3
tags: [cross-framework, interop, web-components, react, vue, angular, svelte]
prerequisites:
  - 09_01_05_Web_Component_Design_System
---

## Overview

The primary value proposition of web components is framework independence. A `<bs-toggle>` custom element works identically in a React app, Vue template, Angular component, or plain HTML. However, each framework handles custom elements differently — React's synthetic event system doesn't bubble custom events from shadow DOM, Vue's `v-model` needs two-way binding adapters, and Angular's template-driven forms expect `ControlValueAccessor`.

Interop patterns bridge these gaps: wrapping custom elements in thin framework-specific wrappers, using event name mapping for React, and implementing value accessor interfaces for Angular. The goal is zero framework-specific code in the component itself, with optional thin adapter layers for each consumer framework.

## Basic Implementation

```html
<!-- Works in any framework or plain HTML -->
<bs-toggle
  label="Dark Mode"
  checked
  variant="primary"
></bs-toggle>
```

```js
class BsToggle extends HTMLElement {
  static get observedAttributes() { return ['checked', 'disabled', 'label', 'variant']; }
  static formAssociated = true;

  constructor() {
    super();
    this._internals = this.attachInternals();
    this.attachShadow({ mode: 'open' });
    this.shadowRoot.innerHTML = `
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
      <div class="form-check form-switch" part="container">
        <input class="form-check-input" type="checkbox" role="switch" part="toggle" id="toggle">
        <label class="form-check-label" part="label" for="toggle"><slot></slot></label>
      </div>
    `;
    this._input = this.shadowRoot.querySelector('input');
    this._input.addEventListener('change', () => {
      this._internals.setFormValue(this._input.checked ? 'on' : null);
      this.dispatchEvent(new CustomEvent('bs:change', {
        detail: { checked: this._input.checked },
        bubbles: true,
        composed: true
      }));
    });
  }

  get checked() { return this._input.checked; }
  set checked(val) { this._input.checked = val; }

  get value() { return this._input.checked ? 'on' : ''; }
}

customElements.define('bs-toggle', BsToggle);
```

```jsx
// React wrapper
import React, { useRef, useEffect } from 'react';

export function BsToggle({ label, checked, onChange, ...props }) {
  const ref = useRef();

  useEffect(() => {
    const el = ref.current;
    const handler = (e) => onChange?.(e.detail.checked);
    el.addEventListener('bs:change', handler);
    return () => el.removeEventListener('bs:change', handler);
  }, [onChange]);

  return <bs-toggle ref={ref} checked={checked} {...props}>{label}</bs-toggle>;
}
```

```js
// Vue 3 wrapper
// BsToggle.vue
export default {
  props: ['modelValue', 'label'],
  emits: ['update:modelValue'],
  mounted() {
    this.$el.addEventListener('bs:change', (e) => {
      this.$emit('update:modelValue', e.detail.checked);
    });
  },
  template: `<bs-toggle :checked="modelValue"><slot /></bs-toggle>`
};
```

## Advanced Variations

Angular adapter using `ControlValueAccessor`:

```ts
@Component({
  selector: 'app-bs-toggle',
  template: `<bs-toggle [attr.checked]="value ? '' : null"><ng-content></ng-content></bs-toggle>`,
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: BsToggleAdapter, multi: true }]
})
class BsToggleAdapter implements ControlValueAccessor {
  value = false;
  onChange = () => {};

  @HostListener('bs:change', ['$event.detail.checked'])
  onToggle(checked: boolean) {
    this.value = checked;
    this.onChange(checked);
  }

  writeValue(val: boolean) { this.value = val; }
  registerOnChange(fn: any) { this.onChange = fn; }
  registerOnTouched() {}
}
```

## Best Practices

1. Use `composed: true` on custom events so they cross shadow DOM boundaries.
2. Use `bubbles: true` so events reach framework parent listeners.
3. Implement `ElementInternals` (`attachInternals()`) for native form participation.
4. Set `static formAssociated = true` for form elements to work with `<form>`.
5. Provide both attribute and property APIs — attributes for HTML, properties for framework bindings.
6. Use `dispatchEvent(new CustomEvent(...))` instead of `onclick` for clean event interfaces.
7. Provide TypeScript type declarations (`.d.ts`) for framework consumers.
8. Document the event detail shape and attribute types in the component API.
9. Test in each target framework; don't assume one framework's behavior applies to all.
10. Use `composedPath()` debugging to understand event flow across shadow boundaries.
11. Avoid framework-specific patterns (e.g., React's `className`) in the component itself.
12. Ship as an ES module; frameworks can `import` and use `<script type="module">`.

## Common Pitfalls

1. **React event retargeting** — React doesn't recognize custom events from shadow DOM; use `addEventListener` in `useEffect`.
2. **Vue `v-model` incompatibility** — Custom elements need explicit `modelValue` prop + `update:modelValue` emit.
3. **Angular template binding** — Angular binds to properties, not attributes; reflect attributes to properties.
4. **Event not bubbling** — Without `composed: true`, events stop at the shadow boundary.
5. **Form association missing** — Without `formAssociated`, the element doesn't participate in `<form>` submission.
6. **Svelte `bind:checked`** — Svelte's two-way binding may not detect property changes on custom elements; use event listeners.

## Accessibility Considerations

Implement native form semantics with `ElementInternals`: `this._internals.ariaChecked`, `this._internals.role = 'switch'`. This works across all frameworks because it's native, not framework-dependent.

## Responsive Behavior

Custom elements are responsive by default (they're block-level or inline elements in the flow). Use Bootstrap utility classes in the shadow template and CSS custom properties for responsive theming. No framework-specific responsive handling needed.
