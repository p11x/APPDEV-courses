/**
 * Form Integration Mastery - Comprehensive form controls integration
 * @module forms/07_1_Form-Integration-Mastery
 * @version 1.0.0
 * @example <form-integration-mastery></form-integration-mastery>
 */

class FormIntegrationMastery extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._form = null;
    this._controls = new Map();
    this._boundHandlers = {};
  }

  static get observedAttributes() {
    return ['action', 'method', 'enctype', 'novalidate', 'disabled'];
  }

  static get formAssociated() {
    return true;
  }

  static get template() {
    return `
      <style>
        :host {
          display: block;
          font-family: system-ui, -apple-system, sans-serif;
        }
        :host([disabled]) {
          opacity: 0.6;
          pointer-events: none;
        }
        .form-container {
          padding: 20px;
          border: 1px solid #ddd;
          border-radius: 8px;
          background: #fff;
        }
        .form-field {
          margin-bottom: 16px;
        }
        label {
          display: block;
          margin-bottom: 4px;
          font-weight: 500;
          color: #333;
        }
        input, select, textarea {
          width: 100%;
          padding: 8px 12px;
          border: 1px solid #ccc;
          border-radius: 4px;
          font-size: 14px;
          box-sizing: border-box;
          transition: border-color 0.2s, box-shadow 0.2s;
        }
        input:focus, select:focus, textarea:focus {
          outline: none;
          border-color: #0066cc;
          box-shadow: 0 0 0 3px rgba(0, 102, 204, 0.1);
        }
        input:invalid, select:invalid, textarea:invalid {
          border-color: #dc3545;
        }
        .error-message {
          color: #dc3545;
          font-size: 12px;
          margin-top: 4px;
        }
        .form-actions {
          display: flex;
          gap: 12px;
          margin-top: 20px;
        }
        button {
          padding: 10px 20px;
          border: none;
          border-radius: 4px;
          font-size: 14px;
          cursor: pointer;
          transition: background-color 0.2s;
        }
        button[type="submit"] {
          background: #0066cc;
          color: white;
        }
        button[type="submit"]:hover {
          background: #0052a3;
        }
        button[type="reset"] {
          background: #6c757d;
          color: white;
        }
        button[type="reset"]:hover {
          background: #5a6268;
        }
        .progress-indicator {
          height: 4px;
          background: #e9ecef;
          border-radius: 2px;
          margin-bottom: 20px;
          overflow: hidden;
        }
        .progress-bar {
          height: 100%;
          background: #28a745;
          transition: width 0.3s ease;
        }
      </style>
      <div class="form-container">
        <div class="progress-indicator">
          <div class="progress-bar" style="width: 0%"></div>
        </div>
        <form id="masterForm" novalidate>
          <div class="form-field">
            <label for="username">Username</label>
            <input 
              type="text" 
              id="username" 
              name="username" 
              required 
              minlength="3"
              maxlength="20"
              pattern="^[a-zA-Z0-9_]+$"
              autocomplete="username"
            >
            <div class="error-message" id="usernameError"></div>
          </div>
          
          <div class="form-field">
            <label for="email">Email Address</label>
            <input 
              type="email" 
              id="email" 
              name="email" 
              required
              autocomplete="email"
            >
            <div class="error-message" id="emailError"></div>
          </div>
          
          <div class="form-field">
            <label for="password">Password</label>
            <input 
              type="password" 
              id="password" 
              name="password" 
              required
              minlength="8"
              autocomplete="new-password"
            >
            <div class="error-message" id="passwordError"></div>
          </div>
          
          <div class="form-field">
            <label for="confirmPassword">Confirm Password</label>
            <input 
              type="password" 
              id="confirmPassword" 
              name="confirmPassword" 
              required
              autocomplete="new-password"
            >
            <div class="error-message" id="confirmPasswordError"></div>
          </div>
          
          <div class="form-field">
            <label for="country">Country</label>
            <select id="country" name="country" required>
              <option value="">Select a country</option>
              <option value="us">United States</option>
              <option value="uk">United Kingdom</option>
              <option value="ca">Canada</option>
              <option value="au">Australia</option>
              <option value="de">Germany</option>
              <option value="fr">France</option>
              <option value="jp">Japan</option>
              <option value="in">India</option>
            </select>
            <div class="error-message" id="countryError"></div>
          </div>
          
          <div class="form-field">
            <label for="bio">Bio</label>
            <textarea 
              id="bio" 
              name="bio" 
              rows="4" 
              maxlength="500"
              placeholder="Tell us about yourself..."
            ></textarea>
            <div class="error-message" id="bioError"></div>
          </div>
          
          <div class="form-field">
            <label>
              <input type="checkbox" id="terms" name="terms" required>
              I agree to the Terms and Conditions
            </label>
            <div class="error-message" id="termsError"></div>
          </div>
          
          <div class="form-actions">
            <button type="submit">Submit</button>
            <button type="reset">Reset</button>
          </div>
        </form>
      </div>
    `;
  }

  connectedCallback() {
    this.render();
    this._initializeForm();
    this._attachEventListeners();
  }

  disconnectedCallback() {
    this._removeEventListeners();
    if (this._form) {
      this._form.removeEventListener('submit', this._boundHandlers.submit);
      this._form.removeEventListener('reset', this._boundHandlers.reset);
      this._form.removeEventListener('input', this._boundHandlers.input);
      this._form.removeEventListener('change', this._boundHandlers.change);
      this._form.removeEventListener('blur', this._boundHandlers.blur);
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._handleAttributeChange(name, newValue);
    }
  }

  render() {
    this.shadowRoot.innerHTML = this.constructor.template;
  }

  _initializeForm() {
    this._form = this.shadowRoot.getElementById('masterForm');
    
    if (!this._form) {
      console.error('Form element not found');
      return;
    }

    const controls = this._form.elements;
    for (let i = 0; i < controls.length; i++) {
      const control = controls[i];
      if (control.name) {
        this._controls.set(control.name, {
          element: control,
          value: control.value,
          valid: true,
          touched: false,
          dirty: false
        });
      }
    }

    if (this.form) {
      this.form.append(this);
    }
  }

  _attachEventListeners() {
    this._boundHandlers.submit = this._handleSubmit.bind(this);
    this._boundHandlers.reset = this._handleReset.bind(this);
    this._boundHandlers.input = this._handleInput.bind(this);
    this._boundHandlers.change = this._handleChange.bind(this);
    this._boundHandlers.blur = this._handleBlur.bind(this);
    this._boundHandlers.invalid = this._handleInvalid.bind(this);

    this._form.addEventListener('submit', this._boundHandlers.submit);
    this._form.addEventListener('reset', this._boundHandlers.reset);
    this._form.addEventListener('input', this._boundHandlers.input);
    this._form.addEventListener('change', this._boundHandlers.change);
    this._form.addEventListener('blur', this._boundHandlers.blur, true);

    const controls = this._form.elements;
    for (let i = 0; i < controls.length; i++) {
      controls[i].addEventListener('invalid', this._boundHandlers.invalid);
    }
  }

  _removeEventListeners() {
    if (!this._form) return;
    
    const controls = this._form.elements;
    for (let i = 0; i < controls.length; i++) {
      controls[i].removeEventListener('invalid', this._boundHandlers.invalid);
    }
  }

  _handleSubmit(event) {
    event.preventDefault();
    
    if (!this._validateAll()) {
      this._dispatchEvent('form-error', { message: 'Please fix the validation errors' });
      return;
    }

    const formData = this._getFormData();
    
    this._dispatchEvent('form-submit', { data: formData });
    
    console.log('Form submitted:', formData);
  }

  _handleReset(event) {
    this._controls.forEach((control, name) => {
      control.value = control.element.value;
      control.valid = true;
      control.touched = false;
      control.dirty = false;
      this._clearError(name);
    });
    
    this._updateProgress();
    this._dispatchEvent('form-reset', {});
  }

  _handleInput(event) {
    const control = this._controls.get(event.target.name);
    if (control) {
      control.dirty = true;
      control.value = event.target.value;
      
      if (control.touched) {
        this._validateField(event.target.name);
      }
    }
    
    this._updateProgress();
    this._dispatchEvent('form-input', { 
      name: event.target.name, 
      value: event.target.value 
    });
  }

  _handleChange(event) {
    const control = this._controls.get(event.target.name);
    if (control) {
      control.value = event.target.value;
    }
    
    this._dispatchEvent('form-change', { 
      name: event.target.name, 
      value: event.target.value 
    });
  }

  _handleBlur(event) {
    const control = this._controls.get(event.target.name);
    if (control) {
      control.touched = true;
      this._validateField(event.target.name);
    }
  }

  _handleInvalid(event) {
    event.preventDefault();
    const name = event.target.name;
    const control = this._controls.get(name);
    
    if (control) {
      control.valid = false;
      control.touched = true;
    }
    
    this._setError(name, event.target.validationMessage);
    this._dispatchEvent('form-invalid', { 
      name: name, 
      message: event.target.validationMessage 
    });
  }

  _validateAll() {
    let isValid = true;
    
    this._controls.forEach((control, name) => {
      control.touched = true;
      if (!this._validateField(name)) {
        isValid = false;
      }
    });
    
    return isValid;
  }

  _validateField(name) {
    const control = this._controls.get(name);
    if (!control) return true;

    const element = control.element;
    const isValid = element.checkValidity();
    
    control.valid = isValid;
    
    if (!isValid) {
      this._setError(name, element.validationMessage);
    } else {
      if (name === 'confirmPassword') {
        const password = this._controls.get('password');
        if (password && control.value !== password.value) {
          this._setError(name, 'Passwords do not match');
          control.valid = false;
          return false;
        }
      }
      this._clearError(name);
    }
    
    return control.valid;
  }

  _setError(name, message) {
    const errorElement = this.shadowRoot.getElementById(`${name}Error`);
    if (errorElement) {
      errorElement.textContent = message;
    }
    
    const control = this._controls.get(name);
    if (control) {
      control.element.setAttribute('aria-invalid', 'true');
    }
  }

  _clearError(name) {
    const errorElement = this.shadowRoot.getElementById(`${name}Error`);
    if (errorElement) {
      errorElement.textContent = '';
    }
    
    const control = this._controls.get(name);
    if (control) {
      control.element.removeAttribute('aria-invalid');
    }
  }

  _updateProgress() {
    let filledCount = 0;
    let totalRequired = 0;
    
    this._controls.forEach((control, name) => {
      const element = control.element;
      if (element.hasAttribute('required') || element.required) {
        totalRequired++;
        if (control.value && control.value.length > 0) {
          filledCount++;
        }
      }
    });
    
    const progress = totalRequired > 0 ? (filledCount / totalRequired) * 100 : 0;
    const progressBar = this.shadowRoot.querySelector('.progress-bar');
    if (progressBar) {
      progressBar.style.width = `${progress}%`;
    }
  }

  _getFormData() {
    const data = {};
    this._controls.forEach((control, name) => {
      const element = control.element;
      if (element.type === 'checkbox') {
        data[name] = element.checked;
      } else {
        data[name] = control.value;
      }
    });
    return data;
  }

  _handleAttributeChange(name, value) {
    switch (name) {
      case 'disabled':
        if (this._form) {
          this._form.disabled = value !== null;
        }
        break;
      case 'novalidate':
        if (this._form) {
          this._form.noValidate = value !== null;
        }
        break;
    }
  }

  _dispatchEvent(name, detail) {
    const event = new CustomEvent(name, {
      bubbles: true,
      composed: true,
      detail
    });
    this.dispatchEvent(event);
  }

  get form() {
    return this._form;
  }

  getFormData() {
    return this._getFormData();
  }

  reset() {
    if (this._form) {
      this._form.reset();
    }
  }

  validate() {
    return this._validateAll();
  }

  setValue(name, value) {
    const control = this._controls.get(name);
    if (control) {
      control.element.value = value;
      control.value = value;
      this._updateProgress();
    }
  }

  getValue(name) {
    const control = this._controls.get(name);
    return control ? control.value : null;
  }

  get validity() {
    const validity = {};
    this._controls.forEach((control, name) => {
      validity[name] = control.valid;
    });
    return validity;
  }

  get touched() {
    const touched = {};
    this._controls.forEach((control, name) => {
      touched[name] = control.touched;
    });
    return touched;
  }

  get dirty() {
    const dirty = {};
    this._controls.forEach((control, name) => {
      dirty[name] = control.dirty;
    });
    return dirty;
  }
}

customElements.define('form-integration-mastery', FormIntegrationMastery);

export { FormIntegrationMastery };
