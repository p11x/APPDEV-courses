/**
 * Accessible Forms - Provides accessible form components with proper ARIA attributes,
 * keyboard navigation, focus management, and screen reader announcements
 * @module forms/Accessibility-in-Forms
 * @version 1.0.0
 * @example <accessible-form></accessible-form>
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
    .form-wrapper {
      background: #fff;
      border-radius: 8px;
      padding: 1.5rem;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }
    .form-title {
      font-size: 1.5rem;
      font-weight: 700;
      margin-bottom: 1.5rem;
      color: #222;
    }
    .description {
      color: #666;
      margin-bottom: 1.5rem;
    }
    .form-group {
      margin-bottom: 1.25rem;
    }
    label {
      display: block;
      font-weight: 600;
      margin-bottom: 0.5rem;
      color: #333;
    }
    label.required::after {
      content: " *";
      color: #d32f2f;
    }
    .required-indicator {
      color: #666;
      font-size: 0.875rem;
      font-weight: normal;
    }
    input[type="text"],
    input[type="email"],
    input[type="tel"],
    input[type="password"],
    input[type="number"],
    select,
    textarea {
      width: 100%;
      padding: 0.75rem;
      border: 2px solid #ddd;
      border-radius: 4px;
      font-size: 1rem;
      transition: border-color 0.2s, box-shadow 0.2s;
    }
    input:focus,
    select:focus,
    textarea:focus {
      outline: none;
      border-color: #1976d2;
      box-shadow: 0 0 0 3px rgba(25, 118, 210, 0.2);
    }
    input[aria-invalid="true"],
    select[aria-invalid="true"],
    textarea[aria-invalid="true"] {
      border-color: #d32f2f;
    }
    input[aria-invalid="true"]:focus,
    select[aria-invalid="true"]:focus,
    textarea[aria-invalid="true"]:focus {
      box-shadow: 0 0 0 3px rgba(211, 47, 47, 0.2);
    }
    .field-hint {
      font-size: 0.875rem;
      color: #666;
      margin-top: 0.25rem;
    }
    .error-message {
      color: #d32f2f;
      font-size: 0.875rem;
      margin-top: 0.25rem;
      display: flex;
      align-items: center;
      gap: 0.25rem;
    }
    .error-icon {
      width: 16px;
      height: 16px;
    }
    .radio-group,
    .checkbox-group {
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
    }
    .radio-option,
    .checkbox-option {
      display: flex;
      align-items: flex-start;
      gap: 0.5rem;
    }
    .radio-option input,
    .checkbox-option input {
      margin-top: 0.25rem;
    }
    .radio-option label,
    .checkbox-option label {
      display: inline;
      font-weight: normal;
      margin-bottom: 0;
      cursor: pointer;
    }
    .button-group {
      display: flex;
      gap: 1rem;
      margin-top: 1.5rem;
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
      background-color: #1976d2;
      color: white;
    }
    button.primary:hover {
      background-color: #1565c0;
    }
    button.primary:focus {
      outline: 2px solid #1976d2;
      outline-offset: 2px;
    }
    button.secondary {
      background-color: #f5f5f5;
      color: #333;
    }
    button.secondary:hover {
      background-color: #e0e0e0;
    }
    .skip-link {
      position: absolute;
      top: -40px;
      left: 0;
      background: #1976d2;
      color: white;
      padding: 0.5rem 1rem;
      z-index: 100;
    }
    .skip-link:focus {
      top: 0;
    }
    .visually-hidden {
      position: absolute;
      width: 1px;
      height: 1px;
      padding: 0;
      margin: -1px;
      overflow: hidden;
      clip: rect(0, 0, 0, 0);
      white-space: nowrap;
      border: 0;
    }
    *:focus-visible {
      outline: 2px solid #1976d2;
      outline-offset: 2px;
    }
    .form-region-label {
      font-size: 1.25rem;
      font-weight: 700;
      margin: 1.5rem 0 1rem;
    }
    .fieldset {
      border: none;
      padding: 0;
      margin: 0;
    }
    .fieldset-legend {
      font-weight: 600;
      margin-bottom: 0.5rem;
      display: block;
    }
  </style>
  <a href="#firstField" class="skip-link">Skip to form</a>
  
  <div class="form-wrapper" role="form" aria-labelledby="formTitle" aria-describedby="formDesc">
    <h1 id="formTitle" class="form-title">Contact Information</h1>
    <p id="formDesc" class="description">Please fill out all required fields to submit the form.</p>
    
    <form id="accessibleForm" novalidate>
      <div class="form-group">
        <label for="fullName" class="required">
          Full Name
          <span class="visually-hidden">(required)</span>
        </label>
        <input 
          type="text" 
          id="fullName" 
          name="fullName" 
          required
          aria-required="true"
          autocomplete="name"
          aria-describedby="fullNameHint fullNameError"
        >
        <p id="fullNameHint" class="field-hint">Enter your first and last name</p>
        <p id="fullNameError" class="error-message" role="alert" hidden></p>
      </div>
      
      <div class="form-group">
        <label for="email" class="required">
          Email Address
          <span class="visually-hidden">(required)</span>
        </label>
        <input 
          type="email" 
          id="email" 
          name="email" 
          required
          aria-required="true"
          autocomplete="email"
          aria-describedby="emailHint emailError"
        >
        <p id="emailHint" class="field-hint">We'll use this for verification</p>
        <p id="emailError" class="error-message" role="alert" hidden></p>
      </div>
      
      <div class="form-group">
        <label for="phone">Phone Number</label>
        <input 
          type="tel" 
          id="phone" 
          name="phone"
          autocomplete="tel"
          aria-describedby="phoneHint"
        >
        <p id="phoneHint" class="field-hint">Optional - for SMS notifications</p>
      </div>
      
      <fieldset class="fieldset">
        <legend class="fieldset-legend required">Contact Preference</legend>
        <div class="radio-group" role="radiogroup" aria-required="true">
          <div class="radio-option">
            <input type="radio" id="prefEmail" name="contactPref" value="email" checked>
            <label for="prefEmail">Email</label>
          </div>
          <div class="radio-option">
            <input type="radio" id="prefPhone" name="contactPref" value="phone">
            <label for="prefPhone">Phone</label>
          </div>
          <div class="radio-option">
            <input type="radio" id="prefMail" name="contactPref" value="mail">
            <label for="prefMail">Mail</label>
          </div>
        </div>
      </fieldset>
      
      <div class="form-group">
        <label for="inquiryType" class="required">
          Inquiry Type
          <span class="visually-hidden">(required)</span>
        </label>
        <select 
          id="inquiryType" 
          name="inquiryType" 
          required
          aria-required="true"
          aria-describedby="inquiryTypeHint inquiryTypeError"
        >
          <option value="">Select an option</option>
          <option value="support">Technical Support</option>
          <option value="sales">Sales Inquiry</option>
          <option value="billing">Billing Question</option>
          <option value="feedback">Feedback</option>
          <option value="other">Other</option>
        </select>
        <p id="inquiryTypeHint" class="field-hint">Choose the most relevant option</p>
        <p id="inquiryTypeError" class="error-message" role="alert" hidden></p>
      </div>
      
      <div class="form-group">
        <label for="message" class="required">
          Message
          <span class="visually-hidden">(required)</span>
        </label>
        <textarea 
          id="message" 
          name="message" 
          required
          aria-required="true"
          rows="5"
          aria-describedby="messageHint messageError"
        ></textarea>
        <p id="messageHint" class="field-hint">Describe your inquiry in detail (10-500 characters)</p>
        <p id="messageError" class="error-message" role="alert" hidden></p>
      </div>
      
      <div class="form-group">
        <div class="checkbox-option">
          <input type="checkbox" id="newsletter" name="newsletter">
          <label for="newsletter">Subscribe to our newsletter</label>
        </div>
      </div>
      
      <div class="button-group">
        <button type="submit" class="primary" id="firstField">Submit</button>
        <button type="reset" class="secondary">Clear Form</button>
      </div>
    </form>
    
    <div aria-live="polite" aria-atomic="true" class="visually-hidden" id="formStatus"></div>
  </div>
`;

class AccessibleForm extends HTMLElement {
  #shadowRoot;
  #form;
  #statusRegion;
  #firstInteractiveElement;

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#shadowRoot.appendChild(TEMPLATE.content.cloneNode(true));
  }

  connectedCallback() {
    this.#form = this.#shadowRoot.getElementById('accessibleForm');
    this.#statusRegion = this.#shadowRoot.getElementById('formStatus');
    this.#firstInteractiveElement = this.#shadowRoot.getElementById('firstField');
    
    this.#setupEventListeners();
    this.#setupKeyboardNavigation();
    this.#announceFormLoaded();
  }

  #setupEventListeners() {
    this.#form.addEventListener('submit', this.#handleSubmit.bind(this));
    this.#form.addEventListener('reset', this.#handleReset.bind(this));
    this.#form.addEventListener('invalid', this.#handleInvalid.bind(this), true);
    
    const fields = this.#form.querySelectorAll('input, select, textarea');
    fields.forEach(field => {
      field.addEventListener('blur', this.#handleFieldBlur.bind(this));
      field.addEventListener('input', this.#handleFieldInput.bind(this));
    });
  }

  #setupKeyboardNavigation() {
    this.#form.addEventListener('keydown', this.#handleKeyDown.bind(this));
  }

  #handleSubmit(event) {
    event.preventDefault();
    
    const invalidFields = this.#form.querySelectorAll('[aria-invalid="true"]');
    invalidFields.forEach(field => {
      field.removeAttribute('aria-invalid');
    });
    
    let isValid = true;
    const fields = this.#form.querySelectorAll('[aria-required="true"]');
    
    fields.forEach(field => {
      if (!field.value || (field.type === 'checkbox' && !field.checked)) {
        this.#setFieldError(field, 'This field is required');
        isValid = false;
      }
    });
    
    if (isValid) {
      this.#announceSuccess('Form submitted successfully');
      this.#form.reset();
      this.#announceFormLoaded();
    } else {
      this.#announceError('Form has errors. Please correct the highlighted fields.');
      const firstError = this.#form.querySelector('[aria-invalid="true"]');
      if (firstError) {
        firstError.focus();
      }
    }
  }

  #handleReset() {
    const fields = this.#form.querySelectorAll('[aria-invalid="true"]');
    fields.forEach(field => {
      field.removeAttribute('aria-invalid');
      const errorElement = this.#shadowRoot.getElementById(`${field.id}Error`);
      if (errorElement) {
        errorElement.hidden = true;
        errorElement.textContent = '';
      }
    });
    
    this.#announceFormLoaded();
  }

  #handleInvalid(event) {
    const field = event.target;
    this.#setFieldError(field, field.validationMessage);
  }

  #handleFieldBlur(event) {
    const field = event.target;
    if (field.validity.valid && !field.hasAttribute('aria-invalid')) {
      this.#announceFieldStatus(field, 'field completed');
    }
  }

  #handleFieldInput(event) {
    const field = event.target;
    if (field.validity.valid || field.hasAttribute('aria-invalid')) {
      field.removeAttribute('aria-invalid');
      const errorElement = this.#shadowRoot.getElementById(`${field.id}Error`);
      if (errorElement) {
        errorElement.hidden = true;
      }
    }
  }

  #handleKeyDown(event) {
    if (event.key === 'Enter' && event.target.matches('input:not([type="submit"]):not([type="button"])')) {
      const currentIndex = this.#getFieldIndex(event.target);
      const fields = this.#getOrderedFields();
      const nextField = fields[currentIndex + 1];
      
      if (nextField) {
        nextField.focus();
        event.preventDefault();
      }
    }
    
    if (event.key === 'Escape') {
      this.#form.reset();
      this.#announceFormLoaded();
    }
  }

  #setFieldError(field, message) {
    field.setAttribute('aria-invalid', 'true');
    
    const errorId = `${field.id}Error`;
    const errorElement = this.#shadowRoot.getElementById(errorId);
    
    if (errorElement) {
      errorElement.hidden = false;
      errorElement.textContent = message;
      errorElement.setAttribute('aria-live', 'assertive');
    }
  }

  #getFieldIndex(field) {
    const fields = Array.from(this.#form.querySelectorAll('input, select, textarea'));
    return fields.indexOf(field);
  }

  #getOrderedFields() {
    return Array.from(this.#form.querySelectorAll('input, select, textarea'))
      .filter(field => field.type !== 'hidden' && !field.disabled);
  }

  #announceSuccess(message) {
    this.#statusRegion.textContent = message;
    this.#statusRegion.setAttribute('aria-live', 'polite');
    
    this.dispatchEvent(new CustomEvent('form-success', {
      detail: { message },
      bubbles: true,
      composed: true
    }));
  }

  #announceError(message) {
    this.#statusRegion.textContent = message;
    this.#statusRegion.setAttribute('aria-live', 'assertive');
    
    this.dispatchEvent(new CustomEvent('form-error', {
      detail: { message },
      bubbles: true,
      composed: true
    }));
  }

  #announceFieldStatus(field, status) {
    const hintId = `${field.id}Hint`;
    const hintElement = this.#shadowRoot.getElementById(hintId);
    
    if (hintElement) {
      this.#statusRegion.textContent = `${field.labels[0]?.textContent}: ${status}`;
      this.#statusRegion.setAttribute('aria-live', 'polite');
    }
  }

  #announceFormLoaded() {
    this.#statusRegion.textContent = 'Contact form loaded. Required fields marked.';
    this.#statusRegion.setAttribute('aria-live', 'polite');
  }

  focus() {
    this.#firstInteractiveElement?.focus();
  }

  get form() {
    return this.#form;
  }
}

customElements.define('accessible-form', AccessibleForm);

export { AccessibleForm };