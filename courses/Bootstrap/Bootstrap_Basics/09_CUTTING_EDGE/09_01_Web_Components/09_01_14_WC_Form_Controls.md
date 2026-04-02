---
title: Web Component Form Controls
category: [Web Components, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, web-components, custom-elements, forms, validation
---

## Overview

Custom form control web components extend Bootstrap's form system with encapsulated, reusable inputs that participate in native form submission via `ElementInternals`. These components handle validation, `FormData` integration, and visual feedback while maintaining compatibility with standard HTML forms.

## Basic Implementation

```html
<form id="signupForm" novalidate>
  <bs-input name="email" label="Email address" type="email"
            required placeholder="user@example.com"></bs-input>
  <bs-input name="password" label="Password" type="password"
            required minlength="8"></bs-input>
  <button type="submit" class="btn btn-primary">Sign Up</button>
</form>

<script>
class BsInput extends HTMLElement {
  static formAssociated = true;

  static get observedAttributes() {
    return ['label', 'type', 'value', 'required', 'placeholder',
            'disabled', 'minlength', 'maxlength', 'pattern'];
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._internals = this.attachInternals();
  }

  connectedCallback() {
    this.render();
    this._setupListeners();
  }

  render() {
    const label = this.getAttribute('label') || '';
    const type = this.getAttribute('type') || 'text';
    const placeholder = this.getAttribute('placeholder') || '';
    const required = this.hasAttribute('required');
    const id = `bs-input-${Math.random().toString(36).slice(2, 8)}`;

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <div class="mb-3">
        <label for="${id}" class="form-label">${label}${required ? ' <span class="text-danger">*</span>' : ''}</label>
        <input type="${type}" id="${id}" class="form-control"
               placeholder="${placeholder}" ${required ? 'required' : ''}>
        <div class="invalid-feedback"></div>
      </div>
    `;
  }

  _setupListeners() {
    const input = this.shadowRoot.querySelector('input');
    input.addEventListener('input', () => {
      this._internals.setFormValue(input.value);
      this._validate(input);
    });
  }

  _validate(input) {
    if (!input.checkValidity()) {
      input.classList.add('is-invalid');
      this.shadowRoot.querySelector('.invalid-feedback').textContent =
        input.validationMessage;
      this._internals.setValidity(input.validity, input.validationMessage);
    } else {
      input.classList.remove('is-invalid');
      input.classList.add('is-valid');
      this._internals.setValidity({});
    }
  }

  get form() { return this._internals.form; }
  get name() { return this.getAttribute('name'); }
  get value() { return this.shadowRoot.querySelector('input')?.value || ''; }
  set value(v) {
    const input = this.shadowRoot.querySelector('input');
    if (input) input.value = v;
    this._internals.setFormValue(v);
  }
  get validity() { return this._internals.validity; }
  checkValidity() { return this._internals.checkValidity(); }
  reportValidity() { return this._internals.reportValidity(); }
}
customElements.define('bs-input', BsInput);
</script>
```

## Advanced Variations

Adding a custom select component with search and multi-select capabilities.

```html
<bs-select name="country" label="Country" required>
  <option value="">Choose...</option>
  <option value="us">United States</option>
  <option value="uk">United Kingdom</option>
  <option value="ca">Canada</option>
</bs-select>

<script>
class BsSelect extends HTMLElement {
  static formAssociated = true;

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._internals = this.attachInternals();
  }

  connectedCallback() {
    this.render();
    const select = this.shadowRoot.querySelector('select');
    const slot = this.shadowRoot.querySelector('slot');

    slot.addEventListener('slotchange', () => {
      const options = slot.assignedNodes().filter(n => n.tagName === 'OPTION');
      options.forEach(opt => select.appendChild(opt.cloneNode(true)));
    });

    select.addEventListener('change', () => {
      this._internals.setFormValue(select.value);
      this._validate(select);
    });
  }

  render() {
    const label = this.getAttribute('label') || '';
    const required = this.hasAttribute('required');
    const id = `bs-select-${Math.random().toString(36).slice(2, 8)}`;

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <div class="mb-3">
        <label for="${id}" class="form-label">${label}</label>
        <select id="${id}" class="form-select" ${required ? 'required' : ''}></select>
        <div class="invalid-feedback"></div>
      </div>
      <slot style="display:none"></slot>
    `;
  }

  _validate(select) {
    if (!select.checkValidity()) {
      select.classList.add('is-invalid');
      this._internals.setValidity(select.validity, select.validationMessage);
    } else {
      select.classList.remove('is-invalid');
      this._internals.setValidity({});
    }
  }

  get value() { return this.shadowRoot.querySelector('select')?.value || ''; }
  set value(v) {
    const select = this.shadowRoot.querySelector('select');
    if (select) select.value = v;
    this._internals.setFormValue(v);
  }
}
customElements.define('bs-select', BsSelect);
```

## Best Practices

1. Use `static formAssociated = true` and `attachInternals()` for native form participation
2. Call `_internals.setFormValue()` on every input change to update form data
3. Implement `checkValidity()` and `reportValidity()` delegating to internals
4. Reflect `validity` and `validationMessage` through `_internals.setValidity()`
5. Map HTML validation attributes (`required`, `pattern`, `minlength`) to the internal input
6. Use Bootstrap's `is-valid`/`is-invalid` classes for visual feedback
7. Generate unique IDs for label-input association within Shadow DOM
8. Provide getter/setter for `value` property for programmatic access
9. Support `disabled` attribute by disabling the internal input element
10. Use `<slot>` for passing `<option>` elements into custom select components
11. Handle form reset events via `formAssociatedCallback`
12. Expose the `form` getter via `_internals.form` for external access

## Common Pitfalls

1. **Missing `formAssociated = true`** — Component doesn't participate in form submission
2. **Not calling `setFormValue()`** — Form data is empty despite visible input
3. **Validation not synced** — Internal input valid but `_internals.validity` is not updated
4. **Shadow DOM blocks form reset** — Component doesn't clear on `form.reset()`
5. **Label association broken** — `for`/`id` mapping doesn't cross Shadow DOM boundary
6. **Options not slotted** — Custom select doesn't receive `<option>` children
7. **Missing `reportValidity()`** — Browser-native validation UI doesn't trigger
8. **`FormData` missing values** — Values not included because `setFormValue` was never called

## Accessibility Considerations

Ensure the internal input is properly associated with its label via `for`/`id`. Map ARIA attributes (`aria-describedby`, `aria-invalid`) from the host to the internal element. Announce validation errors using `aria-live` regions. Support `aria-required` alongside the `required` attribute. Maintain focus management so the internal input receives focus when the host is clicked.

## Responsive Behavior

Apply Bootstrap's responsive form classes through the component's internal markup. Use `col-12 col-md-6` wrapper classes via attributes for inline form layouts. Ensure form controls expand to full-width on small screens. Test validation message positioning at narrow viewports to prevent overflow.
