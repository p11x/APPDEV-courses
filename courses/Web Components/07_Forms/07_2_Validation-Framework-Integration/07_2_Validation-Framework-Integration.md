# Validation Framework Integration

## OVERVIEW

Validation framework integration connects Web Components with form validation systems. This guide covers built-in validation, custom validators, and framework validation patterns.

## IMPLEMENTATION DETAILS

### Custom Validation

```javascript
class ValidatedInput extends HTMLElement {
  static get formAssociated() { return true; }
  
  #internals = null;
  #validators = [];
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#internals = this.attachInternals();
    this.render();
  }
  
  addValidator(fn, message) {
    this.#validators.push({ fn, message });
  }
  
  validate() {
    const value = this.value;
    let valid = true;
    let message = '';
    
    for (const validator of this.#validators) {
      const result = validator.fn(value);
      if (!result.valid) {
        valid = false;
        message = validator.message;
        break;
      }
    }
    
    this.#internals.setValidity(
      { customError: !valid },
      valid ? '' : message
    );
    
    return valid;
  }
  
  get value() {
    return this.shadowRoot.querySelector('input')?.value || '';
  }
}
```

## NEXT STEPS

Proceed to **07_Forms/07_3_Data-Binding-Advanced-Methods**.