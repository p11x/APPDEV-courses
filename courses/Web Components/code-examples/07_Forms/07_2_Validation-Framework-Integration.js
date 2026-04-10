/**
 * Validation Framework Integration - Integrates HTML5 Constraint Validation API
 * with custom web components for form validation
 * @module forms/Validation-Framework-Integration
 * @version 1.0.0
 * @example <validation-form></validation-form>
 */

const TEMPLATE = document.createElement('template');
TEMPLATE.innerHTML = `
  <style>
    :host {
      display: block;
      font-family: system-ui, -apple-system, sans-serif;
    }
    :host([hidden]) {
      display: none;
    }
    .form-group {
      margin-bottom: 1rem;
    }
    label {
      display: block;
      font-weight: 600;
      margin-bottom: 0.5rem;
      color: #333;
    }
    input, select, textarea {
      width: 100%;
      padding: 0.75rem;
      border: 2px solid #ccc;
      border-radius: 4px;
      font-size: 1rem;
      transition: border-color 0.2s, box-shadow 0.2s;
    }
    input:focus, select:focus, textarea:focus {
      outline: none;
      border-color: #0066cc;
      box-shadow: 0 0 0 3px rgba(0, 102, 204, 0.2);
    }
    input:invalid, select:invalid, textarea:invalid {
      border-color: #cc0000;
    }
    input.valid, select.valid, textarea.valid {
      border-color: #00aa00;
    }
    .error-message {
      color: #cc0000;
      font-size: 0.875rem;
      margin-top: 0.25rem;
      min-height: 1.25rem;
    }
    .validity-indicator {
      display: inline-block;
      width: 8px;
      height: 8px;
      border-radius: 50%;
      margin-left: 0.5rem;
      vertical-align: middle;
    }
    .validity-indicator.valid {
      background-color: #00aa00;
    }
    .validity-indicator.invalid {
      background-color: #cc0000;
    }
    .validity-indicator.warning {
      background-color: #ffaa00;
    }
    button {
      padding: 0.75rem 1.5rem;
      border: none;
      border-radius: 4px;
      font-size: 1rem;
      font-weight: 600;
      cursor: pointer;
      transition: background-color 0.2s;
    }
    button.primary {
      background-color: #0066cc;
      color: white;
    }
    button.primary:hover {
      background-color: #0055aa;
    }
    button.primary:disabled {
      background-color: #ccc;
      cursor: not-allowed;
    }
    button.secondary {
      background-color: #f0f0f0;
      color: #333;
      margin-left: 0.5rem;
    }
    button.secondary:hover {
      background-color: #e0e0e0;
    }
    .field-wrapper {
      display: flex;
      align-items: flex-start;
    }
    .field-wrapper > *:first-child {
      flex: 1;
    }
  </style>
  <form id="validationForm" novalidate>
    <div class="form-group">
      <label for="email">Email Address</label>
      <div class="field-wrapper">
        <input 
          type="email" 
          id="email" 
          name="email" 
          required 
          pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$"
          autocomplete="email"
          aria-describedby="email-error"
        >
        <span class="validity-indicator" aria-hidden="true"></span>
      </div>
      <div id="email-error" class="error-message" role="alert"></div>
    </div>
    
    <div class="form-group">
      <label for="password">Password</label>
      <div class="field-wrapper">
        <input 
          type="password" 
          id="password" 
          name="password" 
          required 
          minlength="8"
          maxlength="128"
          autocomplete="new-password"
          aria-describedby="password-error password-hint"
        >
        <span class="validity-indicator" aria-hidden="true"></span>
      </div>
      <div id="password-hint" class="error-message">Must be 8-128 characters</div>
      <div id="password-error" class="error-message" role="alert"></div>
    </div>
    
    <div class="form-group">
      <label for="confirm-password">Confirm Password</label>
      <div class="field-wrapper">
        <input 
          type="password" 
          id="confirm-password" 
          name="confirmPassword" 
          required
          autocomplete="new-password"
          aria-describedby="confirm-error"
        >
        <span class="validity-indicator" aria-hidden="true"></span>
      </div>
      <div id="confirm-error" class="error-message" role="alert"></div>
    </div>
    
    <div class="form-group">
      <label for="url">Website URL</label>
      <div class="field-wrapper">
        <input 
          type="url" 
          id="url" 
          name="url"
          pattern="https?://.+"
          autocomplete="url"
          aria-describedby="url-error"
        >
        <span class="validity-indicator" aria-hidden="true"></span>
      </div>
      <div id="url-error" class="error-message" role="alert"></div>
    </div>
    
    <div class="form-group">
      <label for="age">Age</label>
      <div class="field-wrapper">
        <input 
          type="number" 
          id="age" 
          name="age" 
          min="13"
          max="120"
          step="1"
          aria-describedby="age-error age-hint"
        >
        <span class="validity-indicator" aria-hidden="true"></span>
      </div>
      <div id="age-hint" class="error-message">Must be 13 or older</div>
      <div id="age-error" class="error-message" role="alert"></div>
    </div>
    
    <div class="form-group">
      <label for="terms">Terms Acceptance</label>
      <div>
        <input 
          type="checkbox" 
          id="terms" 
          name="terms" 
          required
          aria-describedby="terms-error"
        >
        <label for="terms" style="display:inline;font-weight:normal;">I accept the terms and conditions</label>
      </div>
      <div id="terms-error" class="error-message" role="alert"></div>
    </div>
    
    <div class="form-actions">
      <button type="submit" class="primary">Submit</button>
      <button type="reset" class="secondary">Reset</button>
    </div>
  </form>
`;

const VALIDATION_MESSAGES = {
  valueMissing: 'This field is required',
  typeMismatch: {
    email: 'Please enter a valid email address',
    url: 'Please enter a valid URL (starting with http:// or https://)'
  },
  patternMismatch: 'Please match the required format',
  rangeUnderflow: 'Value is too low',
  rangeOverflow: 'Value is too high',
  stepMismatch: 'Please enter a valid value',
  tooLong: 'This field has too many characters',
  tooShort: 'This field needs more characters',
  customError: 'Invalid value'
};

class ValidationForm extends HTMLElement {
  #shadowRoot;
  #form;
  #customValidators;
  #validationHooks;

  static get observedAttributes() {
    return ['disabled', 'novalidate'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#shadowRoot.appendChild(TEMPLATE.content.cloneNode(true));
    
    this.#form = this.#shadowRoot.getElementById('validationForm');
    this.#customValidators = new Map();
    this.#validationHooks = new Map();
  }

  connectedCallback() {
    this.#form.addEventListener('submit', this.#handleSubmit.bind(this));
    this.#form.addEventListener('reset', this.#handleReset.bind(this));
    this.#form.addEventListener('input', this.#handleInput.bind(this));
    this.#form.addEventListener('blur', this.#handleBlur.bind(this), true);
    
    this.#form.noValidate = this.hasAttribute('novalidate') === false;
    
    this.#setupCustomValidators();
    this.#setupValidationHooks();
    
    this.#form.querySelectorAll('input, select, textarea').forEach(field => {
      this.#updateFieldValidation(field);
    });
  }

  disconnectedCallback() {
    this.#form.removeEventListener('submit', this.#handleSubmit.bind(this));
    this.#form.removeEventListener('reset', this.#handleReset.bind(this));
    this.#form.removeEventListener('input', this.#handleInput.bind(this));
    this.#form.removeEventListener('blur', this.#handleBlur.bind(this), true);
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (name === 'novalidate') {
      this.#form.noValidate = newValue !== null;
    }
  }

  #setupCustomValidators() {
    const confirmPassword = this.#shadowRoot.getElementById('confirm-password');
    const password = this.#shadowRoot.getElementById('password');
    
    this.#customValidators.set('confirmPassword', {
      validate: () => {
        if (confirmPassword.value !== password.value) {
          return { valid: false, message: 'Passwords do not match' };
        }
        return { valid: true };
      },
      fields: [confirmPassword, password]
    });
    
    const terms = this.#shadowRoot.getElementById('terms');
    this.#customValidators.set('terms', {
      validate: () => {
        if (!terms.checked) {
          return { valid: false, message: 'You must accept the terms and conditions' };
        }
        return { valid: true };
      },
      fields: [terms]
    });
  }

  #setupValidationHooks() {
    const fields = this.#form.querySelectorAll('input, select, textarea');
    fields.forEach(field => {
      this.#validationHooks.set(field, {
        onInput: this.#debounce(() => this.#updateFieldValidation(field), 300),
        onChange: () => this.#updateFieldValidation(field)
      });
    });
  }

  #handleSubmit(event) {
    event.preventDefault();
    
    const isValid = this.#validateAllFields();
    
    if (isValid) {
      const formData = new FormData(this.#form);
      const data = Object.fromEntries(formData.entries());
      
      this.dispatchEvent(new CustomEvent('form-valid', {
        detail: { data, formData },
        bubbles: true,
        composed: true
      }));
      
      console.log('Form submitted:', data);
    } else {
      this.dispatchEvent(new CustomEvent('form-invalid', {
        bubbles: true,
        composed: true
      }));
      
      this.#focusFirstInvalidField();
    }
  }

  #handleReset(event) {
    const fields = this.#form.querySelectorAll('input, select, textarea');
    fields.forEach(field => {
      this.#clearFieldValidation(field);
    });
    
    this.dispatchEvent(new CustomEvent('form-reset', {
      bubbles: true,
      composed: true
    }));
  }

  #handleInput(event) {
    const field = event.target;
    const hook = this.#validationHooks.get(field);
    
    if (hook && hook.onInput) {
      hook.onInput();
    }
  }

  #handleBlur(event) {
    const field = event.target;
    this.#updateFieldValidation(field);
  }

  #validateAllFields() {
    let isValid = true;
    const fields = this.#form.querySelectorAll('input, select, textarea');
    
    fields.forEach(field => {
      if (!this.#updateFieldValidation(field)) {
        isValid = false;
      }
    });
    
    this.#customValidators.forEach((validator, name) => {
      const result = validator.validate();
      const field = validator.fields[0];
      
      if (!result.valid) {
        this.#setFieldError(field, result.message);
        isValid = false;
      } else {
        this.#clearFieldError(field);
      }
    });
    
    return isValid;
  }

  #updateFieldValidation(field) {
    const validity = field.validity;
    const isValid = validity.valid;
    const errorId = field.getAttribute('aria-describedby');
    
    if (!isValid) {
      const message = this.#getValidationMessage(field, validity);
      this.#setFieldError(field, message, errorId);
      this.#setFieldClasses(field, false);
      return false;
    }
    
    let customValid = true;
    this.#customValidators.forEach((validator, name) => {
      if (validator.fields.includes(field)) {
        const result = validator.validate();
        if (!result.valid) {
          this.#setFieldError(field, result.message, errorId);
          customValid = false;
        }
      }
    });
    
    if (customValid) {
      this.#clearFieldError(field);
      this.#setFieldClasses(field, true);
    }
    
    return customValid;
  }

  #getValidationMessage(field, validity) {
    for (const key in VALIDATION_MESSAGES) {
      if (validity[key]) {
        const message = VALIDATION_MESSAGES[key];
        if (typeof message === 'object') {
          const fieldType = field.getAttribute('type');
          return message[fieldType] || message.valueMissing || 'Invalid value';
        }
        return message;
      }
    }
    
    const customError = field.getCustomValidity();
    if (customError) {
      return customError;
    }
    
    return 'Invalid value';
  }

  #setFieldError(field, message, errorId) {
    field.setCustomValidity(message);
    
    if (errorId) {
      const errorElement = this.#shadowRoot.getElementById(errorId);
      if (errorElement) {
        errorElement.textContent = message;
        errorElement.setAttribute('aria-live', 'polite');
      }
    } else {
      const parent = field.closest('.form-group');
      if (parent) {
        let errorElement = parent.querySelector('.error-message[role="alert"]');
        if (!errorElement) {
          errorElement = document.createElement('div');
          errorElement.className = 'error-message';
          errorElement.setAttribute('role', 'alert');
          parent.appendChild(errorElement);
        }
        errorElement.textContent = message;
      }
    }
    
    this.#announceToScreenReader(field, message);
  }

  #clearFieldError(field) {
    field.setCustomValidity('');
    
    const parent = field.closest('.form-group');
    if (parent) {
      const errorElement = parent.querySelector('.error-message[role="alert"]');
      if (errorElement) {
        errorElement.textContent = '';
      }
    }
  }

  #setFieldClasses(field, valid) {
    const wrapper = field.closest('.field-wrapper') || field;
    const indicator = wrapper.querySelector('.validity-indicator');
    
    if (indicator) {
      indicator.classList.remove('valid', 'invalid', 'warning');
      indicator.classList.add(valid ? 'valid' : 'invalid');
    }
    
    field.classList.remove('valid', 'invalid');
    field.classList.add(valid ? 'valid' : 'invalid');
  }

  #clearFieldValidation(field) {
    this.#clearFieldError(field);
    this.#setFieldClasses(field, false);
    field.classList.remove('valid', 'invalid');
  }

  #focusFirstInvalidField() {
    const firstInvalid = this.#form.querySelector(':invalid');
    if (firstInvalid) {
      firstInvalid.focus();
      firstInvalid.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
  }

  #announceToScreenReader(field, message) {
    const announcement = document.createElement('div');
    announcement.setAttribute('role', 'alert');
    announcement.setAttribute('aria-live', 'assertive');
    announcement.setAttribute('aria-atomic', 'true');
    announcement.style.cssText = 'position:absolute;width:1px;height:1px;overflow:hidden;clip:rect(0,0,0,0);';
    announcement.textContent = message;
    
    this.#shadowRoot.appendChild(announcement);
    
    setTimeout(() => {
      this.#shadowRoot.removeChild(announcement);
    }, 1000);
  }

  #debounce(func, wait) {
    let timeout;
    return (...args) => {
      clearTimeout(timeout);
      timeout = setTimeout(() => func.apply(this, args), wait);
    };
  }

  get form() {
    return this.#form;
  }

  validate() {
    return this.#validateAllFields();
  }

  reset() {
    this.#form.reset();
  }

  getData() {
    return Object.fromEntries(new FormData(this.#form).entries());
  }

  setFieldError(fieldName, message) {
    const field = this.#form.querySelector(`[name="${fieldName}"]`);
    if (field) {
      const errorId = field.getAttribute('aria-describedby');
      this.#setFieldError(field, message, errorId);
      this.#setFieldClasses(field, false);
    }
  }

  clearFieldError(fieldName) {
    const field = this.#form.querySelector(`[name="${fieldName}"]`);
    if (field) {
      this.#clearFieldError(field);
      this.#setFieldClasses(field, true);
    }
  }

  addCustomValidator(name, validateFn, fields) {
    this.#customValidators.set(name, {
      validate: validateFn,
      fields: fields.map(f => this.#form.querySelector(f))
    });
  }

  removeCustomValidator(name) {
    this.#customValidators.delete(name);
  }
}

customElements.define('validation-form', ValidationForm);

export { ValidationForm };