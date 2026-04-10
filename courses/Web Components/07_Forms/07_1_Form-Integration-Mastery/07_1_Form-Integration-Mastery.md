# Form Integration Mastery

## OVERVIEW

Form integration enables Web Components to participate in HTML forms. This guide covers form-associated custom elements, validation, and submission handling.

## TECHNICAL SPECIFICATIONS

### Form-Associated Custom Elements

| Feature | Description |
|---------|-------------|
| formAssociated | Static property enabling form integration |
| attachInternals() | Attaches ElementInternals for form control |
| setFormValue() | Sets the value submitted with form |
| validation API | Integrates with browser validation |

## IMPLEMENTATION DETAILS

### Basic Form Element

```javascript
class FormInputElement extends HTMLElement {
  // REQUIRED: Enable form association
  static get formAssociated() { return true; }
  
  #internals = null;
  #input = null;
  #value = '';
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    // Initialize form internals
    this.#internals = this.attachInternals();
    this.render();
    this.setupListeners();
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host { display: block; }
        input {
          padding: 8px;
          border: 1px solid #ccc;
          border-radius: 4px;
          width: 100%;
        }
      </style>
      <input type="text" id="input" aria-label="Custom input" />
    `;
    this.#input = this.shadowRoot.getElementById('input');
  }
  
  setupListeners() {
    this.#input?.addEventListener('input', (e) => {
      this.#value = e.target.value;
      this.#internals.setFormValue(this.#value);
      this.#validate();
    });
  }
  
  #validate() {
    const valid = this.#input?.checkValidity() ?? false;
    this.#internals.setValidity(
      { valueMissing: !this.#value },
      valid ? '' : 'Value is required'
    );
  }
  
  // Public API
  get value() { return this.#value; }
  set value(val) {
    this.#value = val;
    if (this.#input) this.#input.value = val;
    this.#internals?.setFormValue(val);
  }
  
  checkValidity() {
    return this.#internals?.checkValidity() ?? false;
  }
  
  reportValidity() {
    return this.#internals?.reportValidity() ?? false;
  }
}
customElements.define('form-input-element', FormInputElement);
```

### Using in Forms

```html
<form>
  <form-input-element name="username" required></form-input-element>
  <form-input-element name="email" type="email" required></form-input-element>
  <button type="submit">Submit</button>
</form>
```

## NEXT STEPS

Proceed to **07_Forms/07_2_Validation-Framework-Integration**.