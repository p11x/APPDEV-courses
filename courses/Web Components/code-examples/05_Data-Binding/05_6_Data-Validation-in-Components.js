/**
 * Data Validation in Components
 * Implements form validation, input constraints, and validation rules
 * @module data-binding/05_6_Data-Validation-in-Components
 * @version 1.0.0
 * @example <validated-form-element rules='{"email":"required|email"}'></validated-form-element>
 */

const VALIDATION_CONFIG = {
  builtInValidators: {
    required: (value) => value !== null && value !== undefined && value !== '',
    email: (value) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value),
    minLength: (value, min) => value && value.length >= min,
    maxLength: (value, max) => !value || value.length <= max,
    min: (value, min) => value >= min,
    max: (value, max) => value <= max,
    pattern: (value, pattern) => new RegExp(pattern).test(value),
    numeric: (value) => !isNaN(value) && !isNaN(parseFloat(value)),
    integer: (value) => Number.isInteger(Number(value)),
    url: (value) => {
      try {
        new URL(value);
        return true;
      } catch {
        return false;
      }
    },
    alpha: (value) => /^[a-zA-Z]+$/.test(value),
    alphaNumeric: (value) => /^[a-zA-Z0-9]+$/.test(value),
    custom: () => true
  },
  validationMode: 'onChange',
  showAllErrors: true,
  delayTime: 300
};

class ValidationError extends Error {
  constructor(message, code = 'VALIDATION_ERROR', errors = []) {
    super(message);
    this.name = 'ValidationError';
    this.code = code;
    this.errors = errors;
  }
}

class ValidationRule {
  constructor(validator, message, params = []) {
    this.validator = validator;
    this.message = message;
    this.params = params;
  }

  validate(value) {
    return this.validator(value, ...this.params);
  }
}

class FieldValidator {
  constructor(fieldName, rules = {}) {
    this.fieldName = fieldName;
    this.rules = new Map();
    this.errors = [];
    this.touched = false;
    this.dirty = false;
    
    this._parseRules(rules);
  }

  _parseRules(rules) {
    for (const [ruleName, ruleConfig] of Object.entries(rules)) {
      if (typeof ruleConfig === 'boolean' && ruleConfig) {
        const validator = VALIDATION_CONFIG.builtInValidators[ruleName];
        if (validator) {
          this.rules.set(ruleName, new ValidationRule(
            validator,
            this._getDefaultMessage(ruleName),
            []
          ));
        }
      } else if (typeof ruleConfig === 'string') {
        const parts = ruleConfig.split(':');
        const name = parts[0];
        const params = parts[1] ? parts[1].split(',') : [];
        
        const validator = VALIDATION_CONFIG.builtInValidators[name];
        if (validator) {
          this.rules.set(name, new ValidationRule(
            validator,
            this._getDefaultMessage(name),
            params
          ));
        }
      } else if (typeof ruleConfig === 'object' && ruleConfig.validator) {
        this.rules.set(ruleName, new ValidationRule(
          ruleConfig.validator,
          ruleConfig.message || this._getDefaultMessage(ruleName),
          ruleConfig.params || []
        ));
      }
    }
  }

  _getDefaultMessage(ruleName) {
    const messages = {
      required: 'This field is required',
      email: 'Please enter a valid email address',
      minLength: 'Value is too short',
      maxLength: 'Value is too long',
      min: 'Value is too small',
      max: 'Value is too large',
      pattern: 'Invalid format',
      numeric: 'Please enter a number',
      integer: 'Please enter an integer',
      url: 'Please enter a valid URL',
      alpha: 'Only letters are allowed',
      alphaNumeric: 'Only letters and numbers are allowed'
    };
    return messages[ruleName] || 'Invalid value';
  }

  validate(value) {
    this.errors = [];
    
    for (const [ruleName, rule] of this.rules) {
      if (!rule.validate(value)) {
        this.errors.push({
          rule: ruleName,
          message: rule.message
        });
      }
    }

    return this.errors.length === 0;
  }

  isValid() {
    return this.errors.length === 0;
  }

  getFirstError() {
    return this.errors[0]?.message;
  }

  getAllErrors() {
    return this.errors.map(e => e.message);
  }
}

class FormValidator {
  constructor(fields = {}) {
    this.fields = new Map();
    this.errors = new Map();
    this.valid = true;
    this.submitted = false;
    
    for (const [fieldName, rules] of Object.entries(fields)) {
      this.fields.set(fieldName, new FieldValidator(fieldName, rules));
    }
  }

  validate(data) {
    this.valid = true;
    this.errors.clear();

    for (const [fieldName, validator] of this.fields) {
      const value = data[fieldName];
      validator.validate(value);
      
      if (!validator.isValid()) {
        this.valid = false;
        this.errors.set(fieldName, validator.getAllErrors());
      }
    }

    return {
      valid: this.valid,
      errors: Object.fromEntries(this.errors)
    };
  }

  validateField(fieldName, value) {
    const validator = this.fields.get(fieldName);
    if (!validator) return { valid: true, errors: [] };

    validator.validate(value);
    
    if (!validator.isValid()) {
      this.errors.set(fieldName, validator.getAllErrors());
      return { valid: false, errors: validator.getAllErrors() };
    }

    this.errors.delete(fieldName);
    return { valid: true, errors: [] };
  }

  getFieldError(fieldName) {
    return this.errors.get(fieldName);
  }

  getErrors() {
    return Object.fromEntries(this.errors);
  }

  reset() {
    for (const validator of this.fields.values()) {
      validator.errors = [];
    }
    this.errors.clear();
    this.valid = true;
  }
}

class ValidatedFormElement extends HTMLElement {
  static get observedAttributes() {
    return ['rules', 'validation-mode', 'show-all-errors', 'disabled'];
  }

  constructor() {
    super();
    this._validator = null;
    this._formData = {};
    this._fieldElements = new Map();
    this._validationMode = VALIDATION_CONFIG.validationMode;
    this._showAllErrors = VALIDATION_CONFIG.showAllErrors;
    this._isDisabled = false;
    this._submitAttempts = 0;
    
    this._attachShadowDOM();
    this._parseRules();
  }

  _attachShadowDOM() {
    const shadow = this.attachShadow({ mode: 'open' });
    
    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
        padding: 20px;
        border: 2px solid #9c27b0;
        border-radius: 10px;
        background: linear-gradient(135deg, #f3e5f5 0%, #fff 100%);
        font-family: 'Open Sans', system-ui, sans-serif;
      }

      :host([disabled]) {
        opacity: 0.6;
        pointer-events: none;
      }

      :host(.invalid) {
        border-color: #f44336;
      }

      .form-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid rgba(156, 39, 176, 0.3);
      }

      .form-title {
        font-size: 16px;
        font-weight: 700;
        color: #6a1b9a;
      }

      .form-badge {
        font-size: 11px;
        padding: 4px 10px;
        background: #9c27b0;
        color: white;
        border-radius: 12px;
        font-weight: 500;
      }

      .form-fields {
        display: flex;
        flex-direction: column;
        gap: 16px;
        margin-bottom: 16px;
      }

      .field-group {
        display: flex;
        flex-direction: column;
        gap: 6px;
      }

      .field-label {
        font-size: 12px;
        font-weight: 600;
        color: #6a1b9a;
      }

      .field-label .required {
        color: #f44336;
        margin-left: 2px;
      }

      .field-input {
        padding: 10px 14px;
        border: 2px solid #ce93d8;
        border-radius: 6px;
        font-size: 14px;
        transition: all 0.2s;
        background: white;
      }

      .field-input:focus {
        outline: none;
        border-color: #9c27b0;
        box-shadow: 0 0 0 3px rgba(156, 39, 176, 0.1);
      }

      .field-input.error {
        border-color: #f44336;
        background: #ffebee;
      }

      .field-input.valid {
        border-color: #4caf50;
      }

      .field-error {
        font-size: 11px;
        color: #d32f2f;
        padding: 4px 8px;
        background: #ffebee;
        border-radius: 4px;
        display: none;
      }

      .field-error.show {
        display: block;
      }

      .field-error-list {
        display: flex;
        flex-direction: column;
        gap: 2px;
      }

      .field-error-item {
        font-size: 11px;
        color: #c62828;
      }

      .form-controls {
        display: flex;
        gap: 8px;
        margin-bottom: 16px;
      }

      .control-btn {
        flex: 1;
        padding: 12px 16px;
        border: 2px solid #ab47bc;
        border-radius: 6px;
        background: white;
        color: #6a1b9a;
        font-size: 13px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
      }

      .control-btn:hover {
        background: #f3e5f5;
        border-color: #9c27b0;
      }

      .control-btn:active {
        background: #e1bee7;
        transform: scale(0.98);
      }

      .control-btn.primary {
        background: #9c27b0;
        color: white;
      }

      .control-btn.primary:hover {
        background: #7b1fa2;
      }

      .validation-summary {
        padding: 12px;
        background: #ffebee;
        border-radius: 6px;
        margin-bottom: 16px;
        display: none;
      }

      .validation-summary.show {
        display: block;
      }

      .summary-title {
        font-size: 12px;
        font-weight: 600;
        color: #c62828;
        margin-bottom: 8px;
      }

      .summary-item {
        font-size: 11px;
        color: #b71c1c;
        padding: 2px 0;
      }

      .form-metrics {
        display: flex;
        gap: 12px;
        padding: 12px;
        background: #e8eaf6;
        border-radius: 6px;
      }

      .metric {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
      }

      .metric-label {
        font-size: 10px;
        color: #303f9f;
        text-transform: uppercase;
      }

      .metric-value {
        font-size: 16px;
        font-weight: 700;
        color: #3f51b5;
      }

      @keyframes shake {
        0%, 100% { transform: translateX(0); }
        25% { transform: translateX(-5px); }
        75% { transform: translateX(5px); }
      }

      .shake {
        animation: shake 0.3s ease-in-out;
      }

      @keyframes success {
        0% { transform: scale(1); }
        50% { transform: scale(1.05); }
        100% { transform: scale(1); }
      }

      .success {
        animation: success 0.3s ease-in-out;
      }
    `;

    const container = document.createElement('div');
    container.innerHTML = `
      <div class="form-header">
        <span class="form-title">Validated Form</span>
        <span class="form-badge">VALIDATION</span>
      </div>
      
      <div class="form-fields" id="form-fields">
        <div class="field-group">
          <label class="field-label">
            Username <span class="required">*</span>
          </label>
          <input class="field-input" id="field-username" placeholder="Enter username" data-field="username" />
          <div class="field-error" id="error-username"></div>
        </div>
        
        <div class="field-group">
          <label class="field-label">
            Email <span class="required">*</span>
          </label>
          <input class="field-input" id="field-email" type="email" placeholder="Enter email" data-field="email" />
          <div class="field-error" id="error-email"></div>
        </div>
        
        <div class="field-group">
          <label class="field-label">
            Password <span class="required">*</span>
          </label>
          <input class="field-input" id="field-password" type="password" placeholder="Enter password" data-field="password" />
          <div class="field-error" id="error-password"></div>
        </div>
        
        <div class="field-group">
          <label class="field-label">
            Age
          </label>
          <input class="field-input" id="field-age" type="number" placeholder="Enter age" data-field="age" />
          <div class="field-error" id="error-age"></div>
        </div>
        
        <div class="field-group">
          <label class="field-label">
            Website
          </label>
          <input class="field-input" id="field-website" type="url" placeholder="https://example.com" data-field="website" />
          <div class="field-error" id="error-website"></div>
        </div>
      </div>
      
      <div class="validation-summary" id="validation-summary">
        <div class="summary-title">Validation Errors</div>
        <div id="summary-list"></div>
      </div>
      
      <div class="form-controls">
        <button class="control-btn" id="btn-validate">Validate</button>
        <button class="control-btn primary" id="btn-submit">Submit</button>
        <button class="control-btn" id="btn-reset">Reset</button>
      </div>
      
      <div class="form-metrics">
        <div class="metric">
          <span class="metric-label">Valid</span>
          <span class="metric-value" id="valid-count">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Errors</span>
          <span class="metric-value" id="error-count">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Attempts</span>
          <span class="metric-value" id="attempt-count">0</span>
        </div>
      </div>
    `;
    
    shadow.appendChild(style);
    shadow.appendChild(container);
  }

  connectedCallback() {
    this._bindEvents();
    this._setupFieldListeners();
    this._render();
  }

  disconnectedCallback() {
    this._cleanupFieldListeners();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'rules':
        this._parseRules();
        break;
      case 'validation-mode':
        this._validationMode = newValue;
        this._setupFieldListeners();
        break;
      case 'show-all-errors':
        this._showAllErrors = newValue !== 'false';
        break;
      case 'disabled':
        this._isDisabled = newValue !== null;
        break;
    }

    this._render();
  }

  _parseRules() {
    const rulesAttr = this.getAttribute('rules');
    if (!rulesAttr) {
      this._validator = new FormValidator({
        username: { required: true, minLength: '3', maxLength: '20', alphaNumeric: true },
        email: { required: true, email: true },
        password: { required: true, minLength: '8' },
        age: { numeric: true, min: '18', max: '120' },
        website: { url: true }
      });
      return;
    }

    try {
      const rules = JSON.parse(rulesAttr);
      this._validator = new FormValidator(rules);
    } catch (e) {
      console.error('Failed to parse rules:', e);
      this._validator = new FormValidator({});
    }
  }

  _bindEvents() {
    const shadow = this.shadowRoot;
    
    const btnValidate = shadow.getElementById('btn-validate');
    const btnSubmit = shadow.getElementById('btn-submit');
    const btnReset = shadow.getElementById('btn-reset');

    btnValidate?.addEventListener('click', () => this.validate());
    btnSubmit?.addEventListener('click', () => this._handleSubmit());
    btnReset?.addEventListener('click', () => this.reset());
  }

  _setupFieldListeners() {
    const shadow = this.shadowRoot;
    const inputs = shadow?.querySelectorAll('.field-input');
    
    inputs?.forEach(input => {
      const fieldName = input.dataset.field;
      if (fieldName) {
        this._fieldElements.set(fieldName, input);
        
        const eventType = this._validationMode === 'onBlur' ? 'blur' : 'input';
        input.addEventListener(eventType, () => this._handleFieldChange(fieldName));
        input.addEventListener('blur', () => this._handleFieldBlur(fieldName));
      }
    });
  }

  _cleanupFieldListeners() {
    const shadow = this.shadowRoot;
    const inputs = shadow?.querySelectorAll('.field-input');
    
    inputs?.forEach(input => {
      input.replaceWith(input.cloneNode(true));
    });
  }

  _handleFieldChange(fieldName) {
    if (this._validationMode !== 'onChange') return;
    
    const input = this._fieldElements.get(fieldName);
    const value = input?.value;
    this._formData[fieldName] = value;
    
    this._validateField(fieldName, value);
  }

  _handleFieldBlur(fieldName) {
    const input = this._fieldElements.get(fieldName);
    const value = input?.value;
    this._formData[fieldName] = value;
    
    this._validateField(fieldName, value);
  }

  _validateField(fieldName, value) {
    if (!this._validator) return;
    
    this._validator.validateField(fieldName, value);
    this._updateFieldErrorDisplay(fieldName);
    this._render();
  }

  _updateFieldErrorDisplay(fieldName) {
    const shadow = this.shadowRoot;
    const input = this._fieldElements.get(fieldName);
    const errorContainer = shadow?.querySelector(`[id="error-${fieldName}"]`);
    
    if (!input || !errorContainer) return;

    const errors = this._validator.getFieldError(fieldName);
    
    if (errors && errors.length > 0) {
      input.classList.add('error');
      input.classList.remove('valid');
      errorContainer.classList.add('show');
      
      if (this._showAllErrors) {
        errorContainer.innerHTML = `
          <div class="field-error-list">
            ${errors.map(e => `<div class="field-error-item">${e}</div>`).join('')}
          </div>
        `;
      } else {
        errorContainer.innerHTML = errors[0];
      }
    } else if (input.value) {
      input.classList.remove('error');
      input.classList.add('valid');
      errorContainer.classList.remove('show');
    }
  }

  validate() {
    if (!this._validator) return { valid: false, errors: {} };
    
    this._collectFormData();
    const result = this._validator.validate(this._formData);
    
    for (const fieldName of this._fieldElements.keys()) {
      this._updateFieldErrorDisplay(fieldName);
    }
    
    this._updateValidationSummary();
    this._render();
    
    return result;
  }

  _handleSubmit() {
    this._submitAttempts++;
    const result = this.validate();
    
    if (result.valid) {
      this._handleSuccess();
    } else {
      this._handleValidationError();
    }
  }

  _handleSuccess() {
    const shadow = this.shadowRoot;
    const formFields = shadow?.getElementById('form-fields');
    
    if (formFields) {
      formFields.classList.add('success');
      setTimeout(() => formFields.classList.remove('success'), 300);
    }
    
    this.dispatchEvent(new CustomEvent('form-valid', {
      detail: { data: this._formData },
      bubbles: true,
      composed: true
    }));
  }

  _handleValidationError() {
    const shadow = this.shadowRoot;
    const formFields = shadow?.getElementById('form-fields');
    
    if (formFields) {
      formFields.classList.add('shake');
      setTimeout(() => formFields.classList.remove('shake'), 300);
    }
  }

  _updateValidationSummary() {
    const shadow = this.shadowRoot;
    const summary = shadow?.getElementById('validation-summary');
    const summaryList = shadow?.getElementById('summary-list');
    
    const errors = this._validator.getErrors();
    const errorCount = Object.keys(errors).length;
    
    if (errorCount > 0 && summary && summaryList) {
      summary.classList.add('show');
      summaryList.innerHTML = Object.entries(errors)
        .map(([field, errorMsgs]) => `
          <div class="summary-item">
            ${field}: ${errorMsgs.join(', ')}
          </div>
        `).join('');
    } else if (summary) {
      summary.classList.remove('show');
    }
  }

  reset() {
    this._formData = {};
    this._validator?.reset();
    
    for (const input of this._fieldElements.values()) {
      input.value = '';
      input.classList.remove('error', 'valid');
    }
    
    this._updateValidationSummary();
    this._render();
  }

  _collectFormData() {
    for (const [fieldName, input] of this._fieldElements) {
      const type = input.type;
      if (type === 'checkbox') {
        this._formData[fieldName] = input.checked;
      } else {
        this._formData[fieldName] = input.value;
      }
    }
  }

  _render() {
    const shadow = this.shadowRoot;
    
    const validCount = shadow?.getElementById('valid-count');
    const errorCount = shadow?.getElementById('error-count');
    const attemptCount = shadow?.getElementById('attempt-count');

    if (validCount && this._validator) {
      const fields = this._validator.fields;
      let validFields = 0;
      let errorFields = 0;
      
      for (const [fieldName, validator] of fields) {
        if (validator.isValid()) {
          validFields++;
        } else {
          errorFields++;
        }
      }
      
      validCount.textContent = String(validFields);
      if (errorCount) errorCount.textContent = String(errorFields);
    }

    if (attemptCount) {
      attemptCount.textContent = String(this._submitAttempts);
    }
  }

  getFormData() {
    this._collectFormData();
    return { ...this._formData };
  }

  setFormData(data) {
    this._formData = { ...data };
    
    for (const [fieldName, value] of Object.entries(data)) {
      const input = this._fieldElements.get(fieldName);
      if (input) {
        input.value = value;
      }
    }
    
    this._render();
  }
}

if (!customElements.get('validated-form-element')) {
  customElements.define('validated-form-element', ValidatedFormElement);
}

export { 
  ValidatedFormElement, 
  FormValidator, 
  FieldValidator, 
  ValidationRule,
  VALIDATION_CONFIG,
  ValidationError 
};