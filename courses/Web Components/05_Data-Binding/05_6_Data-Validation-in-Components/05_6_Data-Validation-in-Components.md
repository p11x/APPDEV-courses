# Data Validation in Components

## OVERVIEW

Data validation in Web Components ensures data integrity and provides user feedback. This guide covers validation patterns, custom validators, and integration with browser validation APIs.

## IMPLEMENTATION DETAILS

### Built-in Validation API

```javascript
class ValidatedComponent extends HTMLElement {
  static get formAssociated() { return true; }
  
  #internals = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#internals = this.attachInternals();
    this.render();
  }
  
  validate() {
    const value = this.getValue();
    let valid = true;
    let message = '';
    
    // Custom validation logic
    if (!value) {
      valid = false;
      message = 'Value is required';
    } else if (value.length < 3) {
      valid = false;
      message = 'Value must be at least 3 characters';
    }
    
    // Set validity state
    this.#internals.setValidity(
      valid ? {} : { customError: true },
      message,
      this.shadowRoot.querySelector('input')
    );
    
    return valid;
  }
  
  getValue() {
    return this.shadowRoot.querySelector('input')?.value || '';
  }
}
```

### Custom Validators

```javascript
class ValidatorRegistry {
  static validators = {
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
      message: `Minimum ${min} characters required`
    }),
    
    pattern: (regex, message) => (value) => ({
      valid: regex.test(value),
      message
    }),
    
    custom: (fn, message) => (value) => {
      const result = fn(value);
      return { valid: result, message: result ? '' : message };
    }
  };
  
  static addValidator(name, validator) {
    this.validators[name] = validator;
  }
}
```

## NEXT STEPS

Proceed to `05_Data-Binding/05_7_Async-Data-Patterns.md`.