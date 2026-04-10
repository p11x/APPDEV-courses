# Form Validation Framework

## OVERVIEW

Form validation frameworks provide reusable validation logic. This guide covers validation rules, error display, and form integration.

## IMPLEMENTATION DETAILS

### Validation Framework

```javascript
class ValidatedForm extends HTMLElement {
  #validators = new Map();
  #errors = new Map();
  #fields = new Map();
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
    this.#setupFields();
  }
  
  #setupFields() {
    const inputs = this.querySelectorAll('[name]');
    inputs.forEach(input => {
      const name = input.getAttribute('name');
      this.#fields.set(name, input);
    });
  }
  
  addValidator(field, validators) {
    this.#validators.set(field, validators);
  }
  
  async validate() {
    let valid = true;
    
    for (const [field, validators] of this.#validators) {
      const input = this.#fields.get(field);
      if (!input) continue;
      
      const value = input.value;
      let error = '';
      
      for (const v of validators) {
        const result = v(value);
        if (!result.valid) {
          error = result.message;
          valid = false;
          break;
        }
      }
      
      this.#errors.set(field, error);
    }
    
    this.#renderErrors();
    return valid;
  }
  
  #renderErrors() {
    const errorContainer = this.shadowRoot.querySelector('.errors');
    errorContainer.innerHTML = [...this.#errors.values()]
      .filter(e => e)
      .map(e => `<div class="error">${e}</div>`)
      .join('');
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        .error { color: red; font-size: 12px; }
      </style>
      <form class="form">
        <slot></slot>
        <div class="errors"></div>
        <button type="submit">Submit</button>
      </form>
    `;
    
    this.shadowRoot.querySelector('form').addEventListener('submit', (e) => {
      e.preventDefault();
      this.validate();
    });
  }
}
customElements.define('validated-form', ValidatedForm);
```

### Built-in Validators

```javascript
const Validators = {
  required: (value) => ({
    valid: value && value.trim().length > 0,
    message: 'This field is required'
  }),
  
  email: (value) => ({
    valid: /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value),
    message: 'Invalid email format'
  }),
  
  minLength: (min) => (value) => ({
    valid: value.length >= min,
    message: `Must be at least ${min} characters`
  }),
  
  pattern: (regex, message) => (value) => ({
    valid: regex.test(value),
    message
  })
};
```

## NEXT STEPS

Proceed to **11_Real-World-Applications/11_4_Design-System-Implementation**.