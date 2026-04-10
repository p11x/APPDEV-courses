# JavaScript Form Validation: Comprehensive Guide

**Table of Contents**
1. [Introduction](#introduction)
2. [Constraint Validation API](#constraint-validation-api)
3. [Custom Validation Techniques](#custom-validation-techniques)
4. [Error Handling and Messaging](#error-handling-and-messaging)
5. [UX Best Practices](#ux-best-practices)
6. [Code Examples](#code-examples)
7. [Security Considerations](#security-considerations)
8. [Performance Implications](#performance-implications)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pitfalls)

---

## Introduction

While HTML5 provides native form validation through attributes, real-world applications often require more sophisticated validation logic that considers complex rules, cross-field dependencies, and business logic. JavaScript form validation bridges the gap between basic HTML5 validation and the nuanced requirements of production applications.

The Constraint Validation API, combined with custom JavaScript validation, provides developers with powerful tools to create robust, user-friendly forms. This guide explores both native browser capabilities and custom implementation patterns that work across all browsers.

Understanding these techniques is essential for building forms that not only validate data but provide excellent user experiences through clear feedback and intuitive interfaces.

---

## Constraint Validation API

The Constraint Validation API is a native browser API that provides programmatic access to form validation. It allows developers to check validation status, set custom validity messages, and control form submission programmatically.

### API Methods and Properties

#### `checkValidity()`

Checks if an element passes validation:

```javascript
const input = document.getElementById('email');

if (input.checkValidity()) {
  console.log('Valid');
} else {
  console.log(input.validationMessage);
}
```

#### `reportValidity()`

Shows validation errors to the user:

```javascript
const form = document.getElementById('myForm');

if (form.checkValidity()) {
  // Form is valid, submit
  form.submit();
} else {
  // Show errors
  form.reportValidity();
}
```

#### `setCustomValidity()`

Sets a custom validity message:

```javascript
const password = document.getElementById('password');
const confirmPassword = document.getElementById('confirm-password');

function validatePasswordMatch() {
  if (password.value !== confirmPassword.value) {
    confirmPassword.setCustomValidity('Passwords do not match');
  } else {
    confirmPassword.setCustomValidity('');
  }
}

confirmPassword.addEventListener('input', validatePasswordMatch);
```

#### Validation Properties

Each input element exposes these read-only properties:

| Property | Description |
|---------|------------|
| `validity` | ValidityState object with validation result flags |
| `validationMessage` | Custom or browser-generated error message |
| `willValidate` | Whether the element will be validated |

#### ValidityState Flags

The `validity` property contains multiple boolean flags:

```javascript
const input = document.getElementById('email');
const validity = input.validity;

console.log({
  valueMissing: validity.valueMissing,
  typeMismatch: validity.typeMismatch,
  patternMismatch: validity.patternMismatch,
  tooLong: validity.tooLong,
  tooShort: validity.tooShort,
  rangeUnderflow: validity.rangeUnderflow,
  rangeOverflow: validity.rangeOverflow,
  stepMismatch: validity.stepMismatch,
  badInput: validity.badInput,
  customError: validity.customError,
  valid: validity.valid
});
```

---

## Custom Validation Techniques

### Real-Time Validation

Validate fields as users type for immediate feedback:

```javascript
class FormValidator {
  constructor(formId) {
    this.form = document.getElementById(formId);
    this.fields = this.form.querySelectorAll('[data-validate]');
    this.init();
  }
  
  init() {
    this.fields.forEach(field => {
      field.addEventListener('input', () => this.validateField(field));
      field.addEventListener('blur', () => this.validateField(field));
    });
    
    this.form.addEventListener('submit', (e) => this.handleSubmit(e));
  }
  
  validateField(field) {
    const rules = this.getFieldRules(field.dataset.validate);
    let error = null;
    
    for (const rule of rules) {
      const result = rule.validator(field.value, field);
      if (result !== true) {
        error = result;
        break;
      }
    }
    
    this.setFieldError(field, error);
    return error === null;
  }
  
  getFieldRules(validationType) {
    const rules = {
      required: [
        {
          validator: (value) => value.trim() !== '' || 'This field is required'
        }
      ],
      email: [
        {
          validator: (value) => value === '' || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value) || 'Invalid email address'
        }
      ],
      password: [
        {
          validator: (value) => value.length >= 8 || 'Password must be at least 8 characters'
        },
        {
          validator: (value) => /[A-Z]/.test(value) || 'Password must contain an uppercase letter'
        },
        {
          validator: (value) => /[0-9]/.test(value) || 'Password must contain a number'
        }
      ]
    };
    
    return rules[validationType] || [];
  }
  
  setFieldError(field, error) {
    const errorElement = field.parentElement.querySelector('.error-message');
    
    if (error) {
      field.setAttribute('aria-invalid', 'true');
      if (errorElement) {
        errorElement.textContent = error;
      }
    } else {
      field.removeAttribute('aria-invalid');
      if (errorElement) {
        errorElement.textContent = '';
      }
    }
  }
  
  handleSubmit(e) {
    let isValid = true;
    
    this.fields.forEach(field => {
      if (!this.validateField(field)) {
        isValid = false;
      }
    });
    
    if (!isValid) {
      e.preventDefault();
      this.form.querySelector('[aria-invalid="true"]')?.focus();
    }
  }
}

// Usage
new FormValidator('registrationForm');
```

### Cross-Field Validation

Validate fields that depend on each other:

```javascript
class CrossFieldValidator {
  constructor(formId) {
    this.form = document.getElementById(formId);
    this.password = this.form.querySelector('#password');
    this.confirmPassword = this.form.querySelector('#confirmPassword');
    this.terms = this.form.querySelector('#terms');
    this.init();
  }
  
  init() {
    this.password.addEventListener('input', () => this.validatePasswordMatch());
    this.confirmPassword.addEventListener('input', () => this.validatePasswordMatch());
    this.terms.addEventListener('change', () => this.validateTerms());
  }
  
  validatePasswordMatch() {
    const isValid = this.password.value === this.confirmPassword.value;
    
    if (this.confirmPassword.value && !isValid) {
      this.confirmPassword.setCustomValidity('Passwords do not match');
    } else {
      this.confirmPassword.setCustomValidity('');
    }
    
    return isValid;
  }
  
  validateTerms() {
    if (!this.terms.checked) {
      this.terms.setCustomValidity('You must accept the terms');
      return false;
    }
    this.terms.setCustomValidity('');
    return true;
  }
  
  validateAll() {
    return this.validatePasswordMatch() && this.validateTerms();
  }
}
```

### Async Validation

Validate fields with server-side checks:

```javascript
class AsyncValidator {
  constructor(formId) {
    this.form = document.getElementById(formId);
    this.username = this.form.querySelector('#username');
    this.init();
  }
  
  init() {
    let debounceTimer;
    
    this.username.addEventListener('input', (e) => {
      clearTimeout(debounceTimer);
      this.clearError();
      
      if (e.target.value.length < 3) return;
      
      this.setLoading(true);
      
      debounceTimer = setTimeout(async () => {
        try {
          const isAvailable = await this.checkUsernameAvailability(e.target.value);
          
          if (!isAvailable) {
            this.setError('Username is already taken');
          }
        } catch (error) {
          this.setError('Could not verify username. Please try again.');
        } finally {
          this.setLoading(false);
        }
      }, 500);
    });
  }
  
  async checkUsernameAvailability(username) {
    const response = await fetch(`/api/check-username?username=${encodeURIComponent(username)}`);
    const data = await response.json();
    return data.available;
  }
  
  setError(message) {
    this.username.setCustomValidity(message);
    this.showError(message);
  }
  
  clearError() {
    this.username.setCustomValidity('');
    this.hideError();
  }
  
  showError(message) {
    const errorEl = this.form.querySelector('#username-error');
    if (errorEl) errorEl.textContent = message;
  }
  
  hideError() {
    const errorEl = this.form.querySelector('#username-error');
    if (errorEl) errorEl.textContent = '';
  }
  
  setLoading(loading) {
    this.username.classList.toggle('loading', loading);
  }
}
```

---

## Error Handling and Messaging

### Custom Error Display

Replace browser default errors with custom styling:

```javascript
class CustomErrorHandler {
  constructor(formId) {
    this.form = document.getElementById(formId);
    this.init();
  }
  
  init() {
    const inputs = this.form.querySelectorAll('input, select, textarea');
    
    inputs.forEach(input => {
      input.addEventListener('invalid', (e) => {
        e.preventDefault();
        this.showError(input);
      });
      
      input.addEventListener('input', () => {
        if (input.validity.valid) {
          this.clearError(input);
        }
      });
    });
    
    this.form.addEventListener('submit', (e) => {
      if (!this.form.checkValidity()) {
        e.preventDefault();
        this.handleInvalidSubmit();
      }
    });
  }
  
  showError(input) {
    const errorEl = input.parentElement.querySelector('.error-message');
    const errorMessage = input.validationMessage;
    
    input.classList.add('error');
    input.setAttribute('aria-invalid', 'true');
    
    if (errorEl) {
      errorEl.textContent = errorMessage;
      errorEl.setAttribute('role', 'alert');
    }
  }
  
  clearError(input) {
    const errorEl = input.parentElement.querySelector('.error-message');
    
    input.classList.remove('error');
    input.removeAttribute('aria-invalid');
    
    if (errorEl) {
      errorEl.textContent = '';
    }
  }
  
  handleInvalidSubmit() {
    const firstError = this.form.querySelector('input:invalid, select:invalid, textarea:invalid');
    firstError?.focus();
    firstError?.scrollIntoView({ behavior: 'smooth', block: 'center' });
  }
}
```

### Live Error Regions

Use ARIA live regions for dynamic error messages:

```html
<form id="myForm" aria-live="polite">
  <div class="error-summary" role="alert" aria-hidden="true">
    <!-- Summary errors appear here -->
  </div>
  
  <div class="form-group">
    <label for="email">Email</label>
    <input type="email" id="email" name="email" required>
    <span class="error-message" role="alert"></span>
  </div>
  
  <button type="submit">Submit</button>
</form>
```

```javascript
class LiveErrorHandler {
  constructor(formId) {
    this.form = document.getElementById(formId);
    this.errorSummary = this.form.querySelector('.error-summary');
    this.init();
  }
  
  init() {
    this.form.addEventListener('submit', (e) => {
      if (!this.form.checkValidity()) {
        e.preventDefault();
        this.showErrors();
      }
    });
  }
  
  showErrors() {
    const errors = [];
    const inputs = this.form.querySelectorAll('input, select, textarea');
    
    inputs.forEach(input => {
      if (!input.validity.valid) {
        const error = {
          field: input.name,
          message: input.validationMessage
        };
        errors.push(error);
        
        input.classList.add('error');
      }
    });
    
    if (errors.length > 0) {
      this.errorSummary.innerHTML = this.formatErrors(errors);
      this.errorSummary.setAttribute('aria-hidden', 'false');
      this.errorSummary.focus();
    }
  }
  
  formatErrors(errors) {
    return `
      <h2>Please correct the following errors:</h2>
      <ul>
        ${errors.map(e => `<li>${e.field}: ${e.message}</li>`).join('')}
      </ul>
    `;
  }
}
```

### First Error Focus

Focus on the first invalid field after submission:

```javascript
function validateAndFocus(form) {
  const isValid = form.checkValidity();
  
  if (!isValid) {
    const firstInvalid = form.querySelector(':invalid');
    
    firstInvalid.focus();
    
    firstInvalid.scrollIntoView({
      behavior: 'smooth',
      block: 'center'
    });
    
    return false;
  }
  
  return true;
}

document.getElementById('myForm').addEventListener('submit', function(e) {
  if (!validateAndFocus(this)) {
    e.preventDefault();
  }
});
```

---

## UX Best Practices

### Debounced Validation

Avoid validating on every keystroke:

```javascript
function createDebouncedValidator(input, validator, delay = 300) {
  let timeout;
  
  function clear() {
    clearTimeout(timeout);
  }
  
  function handleInput() {
    clear();
    
    timeout = setTimeout(() => {
      validator(input.value, input);
    }, delay);
  }
  
  function handleBlur() {
    clear();
    validator(input.value, input);
  }
  
  input.addEventListener('input', handleInput);
  input.addEventListener('blur', handleBlur);
  
  return { clear, handleInput, handleBlur };
}
```

### Validation States

Visual feedback for different states:

```javascript
const ValidationState = {
  idling: 'idling',
  validating: 'validating',
  valid: 'valid',
  invalid: 'invalid'
};

function updateFieldState(input, state, message = '') {
  const field = input.closest('.form-field') || input.parentElement;
  
  field.classList.remove(
    ValidationState.validating,
    ValidationState.valid,
    ValidationState.invalid
  );
  
  if (state !== ValidationState.idling && state !== ValidationState.validating) {
    field.classList.add(state);
  }
  
  const errorEl = field.querySelector('.error-message');
  if (errorEl) {
    errorEl.textContent = message;
  }
  
  if (state === ValidationState.valid) {
    input.setAttribute('aria-invalid', 'false');
  } else if (state === ValidationState.invalid) {
    input.setAttribute('aria-invalid', 'true');
  }
}
```

### Show/Hide Password Toggle

Improve password field UX:

```javascript
function createPasswordToggle(passwordInput, toggleButton) {
  toggleButton.addEventListener('click', () => {
    const isPassword = passwordInput.type === 'password';
    
    passwordInput.type = isPassword ? 'text' : 'password';
    toggleButton.textContent = isPassword ? 'Hide' : 'Show';
    toggleButton.setAttribute('aria-label', isPassword ? 'Hide password' : 'Show password');
  });
}
```

```html
<div class="password-field">
  <label for="password">Password</label>
  <input type="password" id="password" name="password" required>
  <button type="button" aria-label="Show password">Show</button>
</div>
```

---

## Code Examples

### Example 1: Complete Form Validation Class

```javascript
// File: js/form-validator.js

class FormValidator {
  constructor(formSelector, config = {}) {
    this.form = document.querySelector(formSelector);
    this.config = {
      validateOnBlur: true,
      validateOnInput: false,
      showErrorsOnSubmit: true,
      customValidators: {},
      ...config
    };
    
    this.errors = new Map();
    this.init();
  }
  
  init() {
    this.form.noValidate = true;
    
    const inputs = this.form.querySelectorAll('input, select, textarea');
    inputs.forEach(input => this.attachValidation(input));
    
    this.form.addEventListener('submit', (e) => this.handleSubmit(e));
  }
  
  attachValidation(input) {
    const validateOnBlur = input.dataset.validateOnBlur !== 'false';
    const validateOnInput = input.dataset.validateOnInput === 'true';
    
    if (validateOnBlur) {
      input.addEventListener('blur', () => this.validateInput(input));
    }
    
    if (validateOnInput) {
      let debounceTimer;
      input.addEventListener('input', () => {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => {
          this.validateInput(input);
        }, 300);
      });
    }
  }
  
  validateInput(input) {
    const isValid = input.checkValidity();
    const error = isValid ? '' : input.validationMessage;
    
    this.errors.set(input.name, error);
    this.updateFieldUI(input, isValid, error);
    
    return isValid;
  }
  
  validateForm() {
    const inputs = this.form.querySelectorAll('input, select, textarea');
    let isValid = true;
    
    inputs.forEach(input => {
      if (!this.validateInput(input)) {
        isValid = false;
      }
    });
    
    return isValid;
  }
  
  updateFieldUI(input, isValid, error) {
    const field = input.closest('.form-field');
    if (!field) return;
    
    const errorEl = field.querySelector('.error-message');
    
    if (isValid) {
      field.classList.remove('invalid');
      field.classList.add('valid');
      input.setAttribute('aria-invalid', 'false');
    } else {
      field.classList.remove('valid');
      field.classList.add('invalid');
      input.setAttribute('aria-invalid', 'true');
    }
    
    if (errorEl) {
      errorEl.textContent = error;
    }
  }
  
  handleSubmit(e) {
    if (!this.validateForm()) {
      e.preventDefault();
      
      const firstError = this.form.querySelector('.form-field.invalid input, .form-field.invalid select, .form-field.invalid textarea');
      firstError?.focus();
      firstError?.scrollIntoView({ behavior: 'smooth', block: 'center' });
      
      if (this.config.showErrorsOnSubmit) {
        this.showErrorSummary();
      }
    }
  }
  
  showErrorSummary() {
    let summary = this.form.querySelector('.error-summary');
    
    if (!summary) {
      summary = document.createElement('div');
      summary.className = 'error-summary';
      this.form.insertBefore(summary, this.form.firstChild);
    }
    
    const errorList = Array.from(this.errors.entries())
      .filter(([_, error]) => error)
      .map(([name, error]) => `<li>${name}: ${error}</li>`)
      .join('');
    
    summary.innerHTML = errorList ? `<ul>${errorList}</ul>` : '';
    summary.hidden = !errorList;
  }
}

export default FormValidator;
```

### Example 2: Custom Validation Rules

```javascript
// File: js/validation-rules.js

class ValidationRules {
  static required(value, field) {
    if (!value || value.trim() === '') {
      return 'This field is required';
    }
    return true;
  }
  
  static email(value) {
    if (!value) return true;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(value)) {
      return 'Please enter a valid email address';
    }
    return true;
  }
  
  static minLength(min) {
    return (value) => {
      if (!value) return true;
      if (value.length < min) {
        return `Must be at least ${min} characters`;
      }
      return true;
    };
  }
  
  static maxLength(max) {
    return (value) => {
      if (!value) return true;
      if (value.length > max) {
        return `Must be no more than ${max} characters`;
      }
      return true;
    };
  }
  
  static pattern(regex, message) {
    return (value) => {
      if (!value) return true;
      if (!regex.test(value)) {
        return message;
      }
      return true;
    };
  }
  
  static match(fieldSelector, message = 'Fields do not match') {
    return (value, field) => {
      const target = document.querySelector(fieldSelector);
      if (!target) return true;
      if (value !== target.value) {
        return message;
      }
      return true;
    };
  }
  
  static username(value) {
    if (!value) return true;
    if (value.length < 3 || value.length > 20) {
      return 'Username must be 3-20 characters';
    }
    if (!/^[a-zA-Z0-9_]+$/.test(value)) {
      return 'Username can only contain letters, numbers, and underscores';
    }
    if (!/[a-zA-Z]/.test(value)) {
      return 'Username must contain at least one letter';
    }
    return true;
  }
  
  static password(value) {
    if (!value) return true;
    const errors = [];
    
    if (value.length < 8) {
      errors.push('at least 8 characters');
    }
    if (!/[A-Z]/.test(value)) {
      errors.push('an uppercase letter');
    }
    if (!/[a-z]/.test(value)) {
      errors.push('a lowercase letter');
    }
    if (!/[0-9]/.test(value)) {
      errors.push('a number');
    }
    if (!/[^a-zA-Z0-9]/.test(value)) {
      errors.push('a special character');
    }
    
    if (errors.length > 0) {
      return `Password must contain ${errors.join(', ')}`;
    }
    return true;
  }
  
  static creditCard(value) {
    if (!value) return true;
    
    const sanitized = value.replace(/\s/g, '');
    
    if (!/^\d{13,19}$/.test(sanitized)) {
      return 'Please enter a valid card number';
    }
    
    let sum = 0;
    let isEven = false;
    
    for (let i = sanitized.length - 1; i >= 0; i--) {
      let digit = parseInt(sanitized[i], 10);
      
      if (isEven) {
        digit *= 2;
        if (digit > 9) {
          digit -= 9;
        }
      }
      
      sum += digit;
      isEven = !isEven;
    }
    
    if (sum % 10 !== 0) {
      return 'Invalid card number';
    }
    
    return true;
  }
  
  static unique(asyncValidator, errorMessage = 'This value is already taken') {
    return async (value) => {
      if (!value) return true;
      
      try {
        const isUnique = await asyncValidator(value);
        if (!isUnique) {
          return errorMessage;
        }
        return true;
      } catch (e) {
        return true;
      }
    };
  }
}

export default ValidationRules;
```

### Example 3: Dynamic Form Validation

```javascript
// File: js/dynamic-form-validation.js

class DynamicFormValidator {
  constructor(containerSelector) {
    this.container = document.querySelector(containerSelector);
    this.init();
  }
  
  init() {
    this.container.addEventListener('click', (e) => {
      if (e.target.matches('.add-field-btn')) {
        this.addField(e.target);
      }
      if (e.target.matches('.remove-field-btn')) {
        this.removeField(e.target);
      }
    });
    
    this.container.addEventListener('input', (e) => {
      if (e.target.classList.contains('dynamic-input')) {
        this.validateDynamicField(e.target);
      }
    });
  }
  
  addField(button) {
    const template = button.dataset.template;
    const container = button.closest('.dynamic-field-container');
    const fieldsContainer = container.querySelector('.fields-container');
    
    const newField = document.createElement('div');
    newField.className = 'dynamic-field';
    newField.innerHTML = template;
    
    newField.querySelector('input')?.classList.add('dynamic-input');
    
    fieldsContainer.appendChild(newField);
  }
  
  removeField(button) {
    const field = button.closest('.dynamic-field');
    field.remove();
  }
  
  validateDynamicField(input) {
    const isValid = input.checkValidity();
    this.updateFieldError(input, isValid ? '' : input.validationMessage);
    return isValid;
  }
  
  updateFieldError(input, error) {
    const field = input.closest('.dynamic-field');
    const errorEl = field?.querySelector('.error-message');
    if (errorEl) {
      errorEl.textContent = error;
    }
  }
  
  validateAll() {
    const inputs = this.container.querySelectorAll('.dynamic-input');
    let valid = true;
    
    inputs.forEach(input => {
      if (!this.validateDynamicField(input)) {
        valid = false;
      }
    });
    
    return valid;
  }
}
```

### Example 4: Multi-Step Form Validation

```javascript
// File: js/multi-step-validator.js

class MultiStepValidator {
  constructor(formId, steps) {
    this.form = document.getElementById(formId);
    this.steps = steps;
    this.currentStep = 0;
    this.stepCache = new Map();
    
    this.init();
  }
  
  init() {
    this.showStep(0);
    
    this.form.querySelector('.next-btn')?.addEventListener('click', () => this.next());
    this.form.querySelector('.prev-btn')?.addEventListener('click', () => this.prev());
    this.form.querySelector('.submit-btn')?.addEventListener('click', () => this.submit());
  }
  
  showStep(index) {
    this.steps.forEach((step, i) => {
      const stepEl = document.getElementById(step.id);
      if (stepEl) {
        stepEl.hidden = i !== index;
        
        if (i === index) {
          const firstInput = stepEl.querySelector('input, select, textarea');
          firstInput?.focus();
        }
      }
    });
    
    this.currentStep = index;
    this.updateNavigation();
  }
  
  validateCurrentStep() {
    const step = this.steps[this.currentStep];
    const stepEl = document.getElementById(step.id);
    
    if (!stepEl) return true;
    
    const inputs = stepEl.querySelectorAll('input, select, textarea');
    let isValid = true;
    
    inputs.forEach(input => {
      if (!input.checkValidity()) {
        input.reportValidity();
        isValid = false;
      }
    });
    
    return isValid;
  }
  
  next() {
    if (!this.validateCurrentStep()) return;
    
    this.cacheCurrentStep();
    
    if (this.currentStep < this.steps.length - 1) {
      this.showStep(this.currentStep + 1);
    }
  }
  
  prev() {
    if (this.currentStep > 0) {
      this.showStep(this.currentStep - 1);
    }
  }
  
  submit() {
    if (!this.validateCurrentStep()) return;
    
    if (this.form.checkValidity()) {
      this.form.submit();
    } else {
      this.form.reportValidity();
    }
  }
  
  cacheCurrentStep() {
    const step = this.steps[this.currentStep];
    const stepEl = document.getElementById(step.id);
    
    if (stepEl) {
      const data = new FormData(stepEl);
      this.stepCache.set(this.currentStep, Object.fromEntries(data));
    }
  }
  
  updateNavigation() {
    const progress = ((this.currentStep + 1) / this.steps.length) * 100;
    const progressBar = this.form.querySelector('.progress-bar');
    if (progressBar) {
      progressBar.style.width = `${progress}%`;
    }
    
    const prevBtn = this.form.querySelector('.prev-btn');
    const nextBtn = this.form.querySelector('.next-btn');
    const submitBtn = this.form.querySelector('.submit-btn');
    
    if (prevBtn) prevBtn.hidden = this.currentStep === 0;
    if (nextBtn) nextBtn.hidden = this.currentStep === this.steps.length - 1;
    if (submitBtn) submitBtn.hidden = this.currentStep < this.steps.length - 1;
  }
}

// Usage
const validator = new MultiStepValidator('registrationForm', [
  { id: 'step-account', title: 'Account' },
  { id: 'step-profile', title: 'Profile' },
  { id: 'step-confirm', title: 'Confirmation' }
]);
```

### Example 5: File Upload Validation

```javascript
// File: js/file-upload-validator.js

class FileUploadValidator {
  constructor(inputId, options = {}) {
    this.input = document.getElementById(inputId);
    this.options = {
      maxSize: 5 * 1024 * 1024,
      allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'application/pdf'],
      allowedExtensions: ['.jpg', '.jpeg', '.png', '.gif', '.pdf'],
      maxFiles: 5,
      ...options
    };
    
    this.init();
  }
  
  init() {
    this.input.addEventListener('change', (e) => this.handleChange(e));
  }
  
  handleChange(e) {
    const files = Array.from(e.target.files);
    this.clearErrors();
    
    if (files.length === 0) return;
    
    for (const file of files) {
      const errors = this.validateFile(file);
      
      for (const error of errors) {
        this.showError(error);
      }
    }
  }
  
  validateFile(file) {
    const errors = [];
    
    if (!this.options.allowedTypes.includes(file.type)) {
      errors.push(`File type ${file.type} is not allowed`);
    }
    
    if (file.size > this.options.maxSize) {
      const maxMB = (this.options.maxSize / (1024 * 1024)).toFixed(1);
      errors.push(`File size exceeds ${maxMB}MB limit`);
    }
    
    const extension = '.' + file.name.split('.').pop().toLowerCase();
    if (!this.options.allowedExtensions.includes(extension)) {
      errors.push(`File extension ${extension} is not allowed`);
    }
    
    return errors;
  }
  
  showError(message) {
    const errorContainer = this.input.closest('.file-upload-container')
      .querySelector('.error-messages');
    
    if (errorContainer) {
      const li = document.createElement('li');
      li.textContent = message;
      errorContainer.appendChild(li);
    }
  }
  
  clearErrors() {
    const errorContainer = this.input.closest('.file-upload-container')
      .querySelector('.error-messages');
    
    if (errorContainer) {
      errorContainer.innerHTML = '';
    }
  }
  
  getValidFiles() {
    const files = Array.from(this.input.files);
    return files.filter(file => this.validateFile(file).length === 0);
  }
}
```

---

## Security Considerations

### Client-Side vs Server-Side Validation

Always validate on both client and server:

```javascript
// Client-side validation provides UX, not security
// Server-side validation is REQUIRED for security

async function handleFormSubmit(formData) {
  // Client-side validation (for UX)
  if (!clientValidate(formData)) {
    return { success: false, message: 'Please fix the errors' };
  }
  
  // Server-side validation (for security)
  const serverErrors = await serverValidate(formData);
  
  if (serverErrors.length > 0) {
    return { success: false, errors: serverErrors };
  }
  
  return { success: true };
}

async function serverValidate(formData) {
  const errors = [];
  
  // Validate each field server-side
  const email = formData.get('email');
  if (!isValidEmail(email)) {
    errors.push({ field: 'email', message: 'Invalid email format' });
  }
  
  const password = formData.get('password');
  if (!meetsPasswordRequirements(password)) {
    errors.push({ field: 'password', message: 'Password too weak' });
  }
  
  return errors;
}
```

### Input Sanitization

Sanitize input to prevent injection attacks:

```javascript
function sanitizeInput(input) {
  if (typeof input !== 'string') return '';
  
  return input
    .replace(/[<>]/g, '')
    .replace(/javascript:/gi, '')
    .replace(/on\w+=/gi, '')
    .trim();
}

function validateAndSanitize(formData) {
  const sanitized = new FormData();
  
  for (const [key, value] of formData.entries()) {
    if (typeof value === 'string') {
      sanitized.append(key, sanitizeInput(value));
    } else {
      sanitized.append(key, value);
    }
  }
  
  return sanitized;
}
```

---

## Performance Implications

### Validation Debouncing

Avoid expensive validation on every keystroke:

```javascript
function createDebouncedValidator(validator, delay = 300) {
  let timeout;
  
  return function(value, ...args) {
    clearTimeout(timeout);
    
    return new Promise((resolve) => {
      timeout = setTimeout(() => {
        resolve(validator(value, ...args));
      }, delay);
    });
  };
}

const debouncedAsyncValidator = createDebouncedValidator(async (value) => {
  const response = await fetch(`/validate?value=${encodeURIComponent(value)}`);
  return response.json();
});
```

### Batch Validation

Validate all fields efficiently:

```javascript
function batchValidate(fields) {
  const results = [];
  
  for (const field of fields) {
    const errors = validateField(field);
    results.push({ field, errors });
  }
  
  const hasErrors = results.some(r => r.errors.length > 0);
  
  return { valid: !hasErrors, results };
}
```

---

## Key Takeaways

1. **Use Constraint Validation API**: Leverage native `checkValidity()`, `setCustomValidity()`, and `validity` properties for programmatic validation control.

2. **Combine with HTML5 Validation**: Start with HTML5 attributes, enhance with JavaScript for complex validation logic.

3. **Provide Clear Feedback**: Show errors near their respective fields and use ARIA attributes for accessibility.

4. **Validate Cross-Fields**: Implement cross-field validation for dependent fields like password confirmation.

5. **Security First**: Always validate on the server-side as the final line of defense.

---

## Common Pitfalls

1. **Relying Only on Client-Side Validation**: Client-side validation is for UX only. Always validate on the server.

2. **Not Clearing Custom Validity**: Call `setCustomValidity('')` after fixing errors to reset validation state.

3. **Blocking User Input**: Avoid aggressive validation that interrupts typing. Use debouncing.

4. **Ignoring Accessibility**: Don't rely solely on color for error states. Use text and ARIA.

5. **Poor Error Messages**: Native validation messages vary across browsers. Implement custom error handling for consistency.

---

## Cross-Reference

- Previous: [HTML5 Forms API](./01_HTML5_FORMS_API.md)
- Next: [Form Data Handling](./03_FORM_DATA_HANDLING.md)
- Related: [Advanced Form Patterns](./04_ADVANCED_FORM_PATTERNS.md)