/**
 * Form Validation Framework - Comprehensive form validation with rules, messages, and async validation
 * @module real-world/11_3_Form-Validation-Framework
 * @version 1.0.0
 * @example <validated-form></validated-form>
 */

class ValidationRule {
  constructor(name, validator, message) {
    this.name = name;
    this.validator = validator;
    this.message = message;
  }

  validate(value, formData) {
    return this.validator(value, formData);
  }
}

class FormValidator {
  constructor() {
    this.rules = {};
    this.errors = {};
    this.asyncValidators = [];
  }

  static get builtInRules() {
    return {
      required: new ValidationRule(
        'required',
        (value) => value !== null && value !== undefined && value !== '',
        'This field is required'
      ),

      email: new ValidationRule(
        'email',
        (value) => !value || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value),
        'Please enter a valid email address'
      ),

      minLength: (length) => new ValidationRule(
        'minLength',
        (value) => !value || value.length >= length,
        `Minimum ${length} characters required`
      ),

      maxLength: (length) => new ValidationRule(
        'maxLength',
        (value) => !value || value.length <= length,
        `Maximum ${length} characters allowed`
      ),

      min: (min) => new ValidationRule(
        'min',
        (value) => !value || parseFloat(value) >= min,
        `Minimum value is ${min}`
      ),

      max: (max) => new ValidationRule(
        'max',
        (value) => !value || parseFloat(value) <= max,
        `Maximum value is ${max}`
      ),

      number: new ValidationRule(
        'number',
        (value) => !value || !isNaN(parseFloat(value)),
        'Please enter a valid number'
      ),

      integer: new ValidationRule(
        'integer',
        (value) => !value || Number.isInteger(parseFloat(value)),
        'Please enter a whole number'
      ),

      phone: new ValidationRule(
        'phone',
        (value) => !value || /^[6-9]\d{9}$/.test(value.replace(/\D/g, '')),
        'Please enter a valid 10-digit Indian mobile number'
      ),

      pincode: new ValidationRule(
        'pincode',
        (value) => !value || /^[1-9]\d{5}$/.test(value),
        'Please enter a valid 6-digit PIN code'
      ),

      aadhaar: new ValidationRule(
        'aadhaar',
        (value) => !value || /^[2-9]\d{11}$/.test(value.replace(/\D/g, '')),
        'Please enter a valid 12-digit Aadhaar number'
      ),

      pan: new ValidationRule(
        'pan',
        (value) => !value || /^[A-Z]{5}\d{4}[A-Z]$/.test(value.toUpperCase()),
        'Please enter a valid PAN number (e.g., ABCDE1234F)'
      ),

      ifsc: new ValidationRule(
        'ifsc',
        (value) => !value || /^[A-Z]{4}\d{7}$/.test(value.toUpperCase()),
        'Please enter a valid 11-character IFSC code'
      ),

      pattern: (regex, message) => new ValidationRule(
        'pattern',
        (value) => !value || regex.test(value),
        message || 'Invalid format'
      ),

      match: (field, message) => new ValidationRule(
        'match',
        (value, formData) => !value || value === formData[field],
        message || `Must match ${field}`
      ),

      oneOf: (values, message) => new ValidationRule(
        'oneOf',
        (value) => !value || values.includes(value),
        message || `Please select one of: ${values.join(', ')}`
      ),

      url: new ValidationRule(
        'url',
        (value) => !value || /^https?:\/\/.+/.test(value),
        'Please enter a valid URL'
      ),

      date: new ValidationRule(
        'date',
        (value) => !value || !isNaN(Date.parse(value)),
        'Please enter a valid date'
      ),

      futureDate: new ValidationRule(
        'futureDate',
        (value) => !value || new Date(value) > new Date(),
        'Date must be in the future'
      ),

      pastDate: new ValidationRule(
        'pastDate',
        (value) => !value || new Date(value) < new Date(),
        'Date must be in the past'
      ),
    };
  }

  addRule(fieldName, rule) {
    if (!this.rules[fieldName]) {
      this.rules[fieldName] = [];
    }
    this.rules[fieldName].push(rule);
    return this;
  }

  addAsyncValidator(fieldName, validator) {
    this.asyncValidators.push({ field: fieldName, validator });
    return this;
  }

  async validate(formData) {
    this.errors = {};
    let isValid = true;

    for (const fieldName in this.rules) {
      const rules = this.rules[fieldName];
      const value = formData[fieldName];

      for (const rule of rules) {
        const result = await rule.validate(value, formData);
        if (!result) {
          this.errors[fieldName] = rule.message;
          isValid = false;
          break;
        }
      }
    }

    for (const asyncValidator of this.asyncValidators) {
      const result = await asyncValidator.validator(formData[asyncValidator.field], formData);
      if (!result.valid) {
        this.errors[asyncValidator.field] = result.message;
        isValid = false;
      }
    }

    return { isValid, errors: this.errors };
  }

  validateField(fieldName, value, formData) {
    if (!this.rules[fieldName]) return { isValid: true };

    const rules = this.rules[fieldName];
    for (const rule of rules) {
      const result = rule.validate(value, formData);
      if (!result) {
        return { isValid: false, message: rule.message };
      }
    }
    return { isValid: true };
  }

  getErrors() {
    return this.errors;
  }

  clearErrors() {
    this.errors = {};
  }
}

class ValidatedField extends HTMLElement {
  constructor() {
    super();
    this.fieldName = '';
    this.label = '';
    this.type = 'text';
    this.value = '';
    this.disabled = false;
    this.validator = null;
  }

  static get observedAttributes() {
    return ['name', 'label', 'type', 'value', 'disabled', 'rules'];
  }

  static get styles() {
    return `
      :host {
        display: block;
        margin-bottom: 20px;
      }
      .field-container {
        display: flex;
        flex-direction: column;
        gap: 6px;
      }
      .field-label {
        font-size: 0.875rem;
        font-weight: 500;
        color: #495057;
      }
      .field-label.required::after {
        content: ' *';
        color: #dc3545;
      }
      .field-input {
        padding: 12px 16px;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        font-size: 0.875rem;
        transition: border-color 0.2s, box-shadow 0.2s;
        background: white;
      }
      .field-input:focus {
        outline: none;
        border-color: #667eea;
        box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
      }
      .field-input.error {
        border-color: #dc3545;
      }
      .field-input.error:focus {
        box-shadow: 0 0 0 3px rgba(220, 53, 69, 0.1);
      }
      .field-input.valid {
        border-color: #28a745;
      }
      .field-textarea {
        min-height: 100px;
        resize: vertical;
      }
      .field-select {
        appearance: none;
        background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 12 12'%3E%3Cpath fill='%236c757d' d='M6 8L1 3h10z'/%3E%3C/svg%3E");
        background-repeat: no-repeat;
        background-position: right 12px center;
        padding-right: 36px;
      }
      .field-hint {
        font-size: 0.75rem;
        color: #6c757d;
      }
      .field-error {
        font-size: 0.75rem;
        color: #dc3545;
        display: flex;
        align-items: center;
        gap: 4px;
      }
      .field-error-icon {
        font-size: 0.875rem;
      }
      .field-char-count {
        font-size: 0.75rem;
        color: #6c757d;
        text-align: right;
      }
      .otp-input-group {
        display: flex;
        gap: 8px;
      }
      .otp-input {
        width: 48px;
        height: 48px;
        text-align: center;
        font-size: 1.25rem;
        font-weight: 600;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.setupValidator();
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = newValue;
      if (name === 'value') {
        this.render();
      }
    }
  }

  setupValidator() {
    this.validator = new FormValidator();
    const rules = JSON.parse(this.getAttribute('rules') || '[]');
    rules.forEach(rule => {
      if (rule.type === 'required') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.required);
      } else if (rule.type === 'email') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.email);
      } else if (rule.type === 'phone') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.phone);
      } else if (rule.type === 'pincode') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.pincode);
      } else if (rule.type === 'minLength') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.minLength(rule.value));
      } else if (rule.type === 'maxLength') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.maxLength(rule.value));
      } else if (rule.type === 'aadhaar') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.aadhaar);
      } else if (rule.type === 'pan') {
        this.validator.addRule(this.fieldName, FormValidator.builtInRules.pan);
      }
    });
  }

  validate(value) {
    if (!this.validator) return { isValid: true };
    return this.validator.validateField(this.name, value, {});
  }

  render() {
    const inputClass = this.type === 'textarea' ? 'field-input field-textarea' : 
                      this.type === 'select' ? 'field-input field-select' : 'field-input';
    const isRequired = this.getAttribute('rules')?.includes('required');

    this.shadowRoot.innerHTML = `
      <style>${ValidatedField.styles}</style>
      <div class="field-container">
        <label class="field-label ${isRequired ? 'required' : ''}">${this.label}</label>
        
        ${this.type === 'select' ? `
          <select class="${inputClass}" name="${this.fieldName}" ${this.disabled ? 'disabled' : ''}>
            <option value="">Select ${this.label}</option>
            <slot name="options"></slot>
          </select>
        ` : this.type === 'textarea' ? `
          <textarea class="${inputClass}" name="${this.fieldName}" ${this.disabled ? 'disabled' : ''}
            placeholder="Enter ${this.label.toLowerCase()}">${this.value}</textarea>
        ` : this.type === 'otp' ? `
          <div class="otp-input-group">
            <input type="text" class="${inputClass} otp-input" maxlength="1" data-index="0">
            <input type="text" class="${inputClass} otp-input" maxlength="1" data-index="1">
            <input type="text" class="${inputClass} otp-input" maxlength="1" data-index="2">
            <input type="text" class="${inputClass} otp-input" maxlength="1" data-index="3">
            <input type="text" class="${inputClass} otp-input" maxlength="1" data-index="4">
            <input type="text" class="${inputClass} otp-input" maxlength="1" data-index="5">
          </div>
        ` : `
          <input type="${this.type}" class="${inputClass}" name="${this.fieldName}" value="${this.value}"
            ${this.disabled ? 'disabled' : ''} placeholder="Enter ${this.label.toLowerCase()}">
        `}
        
        ${this.getAttribute('hint') ? `<div class="field-hint">${this.getAttribute('hint')}</div>` : ''}
        
        <div class="field-error" style="display: none;"></div>
        ${this.getAttribute('maxlength') ? `<div class="field-char-count">0/${this.getAttribute('maxlength')}</div>` : ''}
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const input = this.shadowRoot.querySelector(`[name="${this.fieldName}"]`);
    input?.addEventListener('blur', () => {
      const result = this.validate(input.value);
      this.showError(result);
    });

    input?.addEventListener('input', () => {
      if (this.getAttribute('maxlength')) {
        const counter = this.shadowRoot.querySelector('.field-char-count');
        if (counter) {
          counter.textContent = `${input.value.length}/${this.getAttribute('maxlength')}`;
        }
      }
    });

    const otpInputs = this.shadowRoot.querySelectorAll('.otp-input');
    otpInputs.forEach((otpInput, index) => {
      otpInput.addEventListener('input', (e) => {
        if (e.target.value && index < otpInputs.length - 1) {
          otpInputs[index + 1].focus();
        }
      });

      otpInput.addEventListener('keydown', (e) => {
        if (e.key === 'Backspace' && !e.target.value && index > 0) {
          otpInputs[index - 1].focus();
        }
      });
    });
  }

  showError(result) {
    const errorDiv = this.shadowRoot.querySelector('.field-error');
    const input = this.shadowRoot.querySelector('.field-input');

    if (!result.isValid) {
      errorDiv.style.display = 'flex';
      errorDiv.innerHTML = `<span class="field-error-icon">⚠️</span> ${result.message}`;
      input.classList.add('error');
      input.classList.remove('valid');
    } else {
      errorDiv.style.display = 'none';
      input.classList.remove('error');
      if (input.value) {
        input.classList.add('valid');
      }
    }
  }

  setError(message) {
    this.showError({ isValid: false, message });
  }

  clearError() {
    this.showError({ isValid: true });
  }

  getValue() {
    const input = this.shadowRoot.querySelector(`[name="${this.fieldName}"]`);
    return input?.value || '';
  }

  setValue(value) {
    const input = this.shadowRoot.querySelector(`[name="${this.fieldName}"]`);
    if (input) {
      input.value = value;
    }
  }
}

class ValidatedForm extends HTMLElement {
  constructor() {
    super();
    this.validator = new FormValidator();
    this.fieldValidators = {};
  }

  static get styles() {
    return `
      :host {
        display: block;
        font-family: 'Poppins', -apple-system, BlinkMacSystemFont, sans-serif;
      }
      .validated-form {
        background: #fff;
        border-radius: 16px;
        padding: 32px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .form-title {
        font-size: 1.5rem;
        font-weight: 700;
        color: #212529;
        margin-bottom: 8px;
      }
      .form-subtitle {
        font-size: 0.875rem;
        color: #6c757d;
        margin-bottom: 24px;
      }
      .form-fields {
        display: grid;
        gap: 20px;
      }
      .form-row {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 20px;
      }
      .form-actions {
        display: flex;
        gap: 12px;
        margin-top: 24px;
      }
      .submit-btn {
        flex: 1;
        padding: 14px 24px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        transition: transform 0.2s, box-shadow 0.2s;
      }
      .submit-btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
      }
      .submit-btn:disabled {
        background: #dee2e6;
        cursor: not-allowed;
        transform: none;
        box-shadow: none;
      }
      .reset-btn {
        padding: 14px 24px;
        background: #fff;
        color: #495057;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
      }
      .reset-btn:hover {
        border-color: #667eea;
        color: #667eea;
      }
      .form-error {
        padding: 16px;
        background: rgba(220, 53, 69, 0.1);
        border: 1px solid #dc3545;
        border-radius: 8px;
        color: #dc3545;
        margin-bottom: 20px;
        display: none;
      }
      .form-error.show {
        display: block;
      }
      @media (max-width: 768px) {
        .form-row {
          grid-template-columns: 1fr;
        }
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.prepareForm();
    this.render();
  }

  prepareForm() {
    const fields = this.querySelectorAll('validated-field');
    fields.forEach(field => {
      const rules = JSON.parse(field.getAttribute('rules') || '[]');
      rules.forEach(rule => {
        if (rule.type === 'required') {
          this.validator.addRule(field.fieldName, FormValidator.builtInRules.required);
        } else if (rule.type === 'email') {
          this.validator.addRule(field.fieldName, FormValidator.builtInRules.email);
        } else if (rule.type === 'phone') {
          this.validator.addRule(field.fieldName, FormValidator.builtInRules.phone);
        } else if (rule.type === 'pincode') {
          this.validator.addRule(field.fieldName, FormValidator.builtInRules.pincode);
        } else if (rule.type === 'match') {
          this.validator.addRule(field.fieldName, FormValidator.builtInRules.match(rule.field, rule.message));
        }
      });
      this.fieldValidators[field.fieldName] = field;
    });
  }

  render() {
    const title = this.getAttribute('title') || 'Form';
    const subtitle = this.getAttribute('subtitle') || '';

    this.shadowRoot.innerHTML = `
      <style>${ValidatedForm.styles}</style>
      <form class="validated-form" novalidate>
        <div class="form-title">${title}</div>
        ${subtitle ? `<div class="form-subtitle">${subtitle}</div>` : ''}
        
        <div class="form-error"></div>
        
        <div class="form-fields">
          ${this.innerHTML}
        </div>
        
        <div class="form-actions">
          <button type="reset" class="reset-btn">Reset</button>
          <button type="submit" class="submit-btn">Submit</button>
        </div>
      </form>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const form = this.shadowRoot.querySelector('form');
    
    form?.addEventListener('submit', async (e) => {
      e.preventDefault();
      await this.handleSubmit();
    });

    form?.addEventListener('reset', () => {
      this.handleReset();
    });
  }

  async handleSubmit() {
    const formData = {};
    for (const name in this.fieldValidators) {
      formData[name] = this.fieldValidators[name].getValue();
    }

    const { isValid, errors } = await this.validator.validate(formData);

    if (!isValid) {
      for (const fieldName in errors) {
        const field = this.fieldValidators[fieldName];
        if (field) {
          field.setError(errors[fieldName]);
        }
      }
      this.showFormError('Please fix the errors above');
    } else {
      this.clearFormError();
      this.dispatchEvent(new CustomEvent('form-submit', {
        detail: { formData, isValid },
        bubbles: true,
        composed: true,
      }));
    }
  }

  handleReset() {
    for (const name in this.fieldValidators) {
      this.fieldValidators[name].clearError();
      this.fieldValidators[name].setValue('');
    }
    this.clearFormError();
  }

  showFormError(message) {
    const errorDiv = this.shadowRoot.querySelector('.form-error');
    if (errorDiv) {
      errorDiv.textContent = message;
      errorDiv.classList.add('show');
    }
  }

  clearFormError() {
    const errorDiv = this.shadowRoot.querySelector('.form-error');
    if (errorDiv) {
      errorDiv.classList.remove('show');
    }
  }
}

class AsyncValidator {
  static async checkEmailExists(email) {
    await new Promise(resolve => setTimeout(resolve, 500));
    const existingEmails = ['test@example.com', 'admin@example.com'];
    return {
      valid: !existingEmails.includes(email),
      message: 'This email is already registered'
    };
  }

  static async checkUsernameExists(username) {
    await new Promise(resolve => setTimeout(resolve, 500));
    const existingUsers = ['admin', 'user', 'test'];
    return {
      valid: !existingUsers.includes(username.toLowerCase()),
      message: 'This username is already taken'
    };
  }

  static async checkPincode(pincode) {
    await new Promise(resolve => setTimeout(resolve, 300));
    const validPincodes = {
      '560034': { city: 'Bangalore', state: 'Karnataka' },
      '110001': { city: 'New Delhi', state: 'Delhi' },
      '400001': { city: 'Mumbai', state: 'Maharashtra' },
    };
    const data = validPincodes[pincode];
    return {
      valid: !!data,
      message: 'Invalid PIN code',
      data
    };
  }

  static async checkIFSC(ifsc) {
    await new Promise(resolve => setTimeout(resolve, 300));
    const validIFSCs = {
      'SBIN0001234': { bank: 'State Bank of India', branch: 'Bangalore Main' },
      'HDFC0001234': { bank: 'HDFC Bank', branch: 'Bangalore' },
    };
    const data = validIFSCs[ifsc];
    return {
      valid: !!data,
      message: 'Invalid IFSC code',
      data
    };
  }
}

export { FormValidator, ValidationRule, ValidatedField, ValidatedForm, AsyncValidator };