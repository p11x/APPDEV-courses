/**
 * Advanced Form Accessibility - Complex accessibility features including
 * live regions, complex validation announcements, focus management,
 * and screen reader optimization
 * @module forms/Form-Accessibility-Advanced
 * @version 1.0.0
 * @example <advanced-accessible-form></advanced-accessible-form>
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
    .form-container {
      max-width: 600px;
      margin: 0 auto;
      padding: 1.5rem;
      background: #fff;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }
    .form-header {
      margin-bottom: 1.5rem;
    }
    .form-title {
      font-size: 1.5rem;
      font-weight: 700;
      color: #222;
      margin: 0 0 0.5rem;
    }
    .form-description {
      color: #666;
      font-size: 0.875rem;
      margin: 0;
    }
    .form-steps {
      display: flex;
      gap: 0.5rem;
      margin-bottom: 1.5rem;
      padding: 0;
      list-style: none;
    }
    .step {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.5rem 0.75rem;
      background: #f5f5f5;
      border-radius: 20px;
      font-size: 0.875rem;
      color: #666;
    }
    .step.active {
      background: #1976d2;
      color: white;
    }
    .step.completed {
      background: #4caf50;
      color: white;
    }
    .step-number {
      width: 20px;
      height: 20px;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      background: currentColor;
      color: white;
      font-size: 0.75rem;
      font-weight: 700;
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
    input, select, textarea {
      width: 100%;
      padding: 0.75rem;
      border: 2px solid #ddd;
      border-radius: 4px;
      font-size: 1rem;
      transition: border-color 0.2s, box-shadow 0.2s;
    }
    input:focus, select:focus, textarea:focus {
      outline: none;
      border-color: #1976d2;
      box-shadow: 0 0 0 3px rgba(25, 118, 210, 0.2);
    }
    input[aria-invalid="true"], select[aria-invalid="true"], textarea[aria-invalid="true"] {
      border-color: #d32f2f;
    }
    .error-container {
      margin-top: 0.5rem;
    }
    .error-item {
      display: flex;
      align-items: flex-start;
      gap: 0.5rem;
      padding: 0.5rem;
      background: #ffebee;
      border-radius: 4px;
      color: #c62828;
      font-size: 0.875rem;
      margin-bottom: 0.25rem;
    }
    .error-item:last-child {
      margin-bottom: 0;
    }
    .error-icon {
      flex-shrink: 0;
      width: 16px;
      height: 16px;
    }
    .button-row {
      display: flex;
      justify-content: space-between;
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
      transition: background-color 0.2s, opacity 0.2s;
    }
    button.primary {
      background-color: #1976d2;
      color: white;
    }
    button.primary:hover {
      background-color: #1565c0;
    }
    button.secondary {
      background-color: #f5f5f5;
      color: #333;
    }
    button.secondary:hover {
      background-color: #e0e0e0;
    }
    button:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }
    .progress-indicator {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.75rem;
      background: #e3f2fd;
      border-radius: 4px;
      margin-bottom: 1.5rem;
    }
    .progress-text {
      flex: 1;
      font-size: 0.875rem;
      color: #1976d2;
    }
    .progress-bar {
      width: 100px;
      height: 8px;
      background: #bbdefb;
      border-radius: 4px;
      overflow: hidden;
    }
    .progress-fill {
      height: 100%;
      background: #1976d2;
      transition: width 0.3s ease;
    }
    .announcements {
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
    .live-region {
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
  </style>
  
  <div class="form-container">
    <div class="form-header">
      <h1 class="form-title">Account Registration</h1>
      <p class="form-description" id="formDesc">Complete all required fields to create your account. Use Tab to navigate between fields.</p>
    </div>
    
    <ul class="form-steps" role="tablist" aria-label="Form progress">
      <li class="step active" role="tab" aria-selected="true" aria-controls="step1">
        <span class="step-number">1</span>
        <span>Account</span>
      </li>
      <li class="step" role="tab" aria-selected="false" aria-controls="step2">
        <span class="step-number">2</span>
        <span>Profile</span>
      </li>
      <li class="step" role="tab" aria-selected="false" aria-controls="step3">
        <span class="step-number">3</span>
        <span>Confirm</span>
      </li>
    </ul>
    
    <div class="progress-indicator">
      <span class="progress-text" id="progressText">Step 1 of 3</span>
      <div class="progress-bar" role="progressbar" aria-valuenow="33" aria-valuemin="0" aria-valuemax="100" aria-label="Form completion">
        <div class="progress-fill" style="width: 33%"></div>
      </div>
    </div>
    
    <form id="advancedForm" aria-describedby="formDesc">
      <div id="step1" role="tabpanel" aria-labelledby="step1-label">
        <div class="form-group">
          <label for="username">
            Username
            <span aria-hidden="true" class="required-mark">*</span>
          </label>
          <input 
            type="text" 
            id="username" 
            name="username"
            required
            autocomplete="username"
            aria-required="true"
            aria-describedby="usernameHint usernameError"
            minlength="3"
            maxlength="20"
            pattern="[a-zA-Z0-9_]+"
          >
          <p id="usernameHint" class="hint">3-20 alphanumeric characters or underscore</p>
          <div id="usernameError" class="error-container" role="alert" aria-live="assertive"></div>
        </div>
        
        <div class="form-group">
          <label for="email">
            Email Address
            <span aria-hidden="true" class="required-mark">*</span>
          </label>
          <input 
            type="email" 
            id="email" 
            name="email"
            required
            autocomplete="email"
            aria-required="true"
            aria-describedby="emailHint emailError"
          >
          <p id="emailHint" class="hint">We'll send verification to this email</p>
          <div id="emailError" class="error-container" role="alert" aria-live="assertive"></div>
        </div>
        
        <div class="form-group">
          <label for="password">
            Password
            <span aria-hidden="true" class="required-mark">*</span>
          </label>
          <input 
            type="password" 
            id="password" 
            name="password"
            required
            autocomplete="new-password"
            aria-required="true"
            aria-describedby="passwordHint passwordError"
            minlength="8"
          >
          <p id="passwordHint" class="hint">Minimum 8 characters</p>
          <div id="passwordError" class="error-container" role="alert" aria-live="assertive"></div>
        </div>
      </div>
      
      <div id="step2" role="tabpanel" aria-labelledby="step2-label" hidden>
        <div class="form-group">
          <label for="fullName">
            Full Name
            <span aria-hidden="true" class="required-mark">*</span>
          </label>
          <input 
            type="text" 
            id="fullName" 
            name="fullName"
            required
            autocomplete="name"
            aria-required="true"
            aria-describedby="fullNameHint fullNameError"
          >
          <p id="fullNameHint" class="hint">As it appears on your ID</p>
          <div id="fullNameError" class="error-container" role="alert" aria-live="assertive"></div>
        </div>
        
        <div class="form-group">
          <label for="birthDate">
            Date of Birth
            <span aria-hidden="true" class="required-mark">*</span>
          </label>
          <input 
            type="date" 
            id="birthDate" 
            name="birthDate"
            required
            aria-required="true"
            aria-describedby="birthDateHint birthDateError"
          >
          <p id="birthDateHint" class="hint">You must be 18 or older</p>
          <div id="birthDateError" class="error-container" role="alert" aria-live="assertive"></div>
        </div>
      </div>
      
      <div id="step3" role="tabpanel" aria-labelledby="step3-label" hidden>
        <p>Please review your information before submitting.</p>
        <div id="summary" aria-live="polite"></div>
      </div>
      
      <div class="button-row">
        <button type="button" class="secondary" id="prevBtn" disabled>Back</button>
        <button type="button" class="primary" id="nextBtn">Next</button>
        <button type="submit" class="primary" id="submitBtn" hidden>Submit</button>
      </div>
    </form>
    
    <div id="liveRegion" class="live-region" aria-live="polite" aria-atomic="true"></div>
    <div id="assertiveRegion" class="live-region" aria-live="assertive" aria-atomic="true"></div>
  </div>
`;

class AdvancedAccessibleForm extends HTMLElement {
  #shadowRoot;
  #form;
  #currentStep;
  #totalSteps;
  #liveRegion;
  #assertiveRegion;
  #progressFill;
  #progressText;

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#shadowRoot.appendChild(TEMPLATE.content.cloneNode(true));
    
    this.#currentStep = 1;
    this.#totalSteps = 3;
  }

  connectedCallback() {
    this.#form = this.#shadowRoot.getElementById('advancedForm');
    this.#liveRegion = this.#shadowRoot.getElementById('liveRegion');
    this.#assertiveRegion = this.#shadowRoot.getElementById('assertiveRegion');
    this.#progressFill = this.#shadowRoot.querySelector('.progress-fill');
    this.#progressText = this.#shadowRoot.getElementById('progressText');
    
    this.#setupEventListeners();
    this.#announceInitialState();
  }

  #setupEventListeners() {
    const prevBtn = this.#shadowRoot.getElementById('prevBtn');
    const nextBtn = this.#shadowRoot.getElementById('nextBtn');
    const submitBtn = this.#shadowRoot.getElementById('submitBtn');
    
    prevBtn.addEventListener('click', this.#handlePrevious.bind(this));
    nextBtn.addEventListener('click', this.#handleNext.bind(this));
    this.#form.addEventListener('submit', this.#handleSubmit.bind(this));
    
    const inputs = this.#form.querySelectorAll('input');
    inputs.forEach(input => {
      input.addEventListener('invalid', this.#handleInvalid.bind(this), true);
      input.addEventListener('input', this.#handleInput.bind(this));
      input.addEventListener('blur', this.#handleBlur.bind(this));
    });
  }

  #announceInitialState() {
    this.#announce('Account registration form. Step 1 of 3: Account Information. Required fields marked.');
  }

  #handlePrevious() {
    if (this.#currentStep <= 1) return;
    
    this.#hideStep(this.#currentStep);
    this.#currentStep--;
    this.#showStep(this.#currentStep);
    this.#updateStepIndicator();
    this.#announce(`Step ${this.#currentStep} of ${this.#totalSteps}`);
  }

  #handleNext() {
    if (!this.#validateCurrentStep()) {
      this.#announceError('Please correct the errors before continuing');
      return;
    }
    
    if (this.#currentStep >= this.#totalSteps) return;
    
    this.#hideStep(this.#currentStep);
    this.#currentStep++;
    this.#showStep(this.#currentStep);
    this.#updateStepIndicator();
    
    if (this.#currentStep === this.#totalSteps) {
      this.#generateSummary();
    }
    
    this.#announce(`Step ${this.#currentStep} of ${this.#totalSteps}`);
  }

  #handleSubmit(event) {
    event.preventDefault();
    
    if (!this.#validateCurrentStep()) {
      this.#announceError('Please correct the errors before submitting');
      return;
    }
    
    this.#announce('Form submitted successfully. Redirecting...');
    
    this.dispatchEvent(new CustomEvent('form-submit', {
      bubbles: true,
      composed: true
    }));
  }

  #handleInvalid(event) {
    event.preventDefault();
    
    const field = event.target;
    const errorMessage = field.validationMessage;
    
    this.#showFieldError(field.id, errorMessage);
    
    this.#assertiveRegion.textContent = `Error: ${field.labels[0]?.textContent}. ${errorMessage}`;
  }

  #handleInput(event) {
    const field = event.target;
    this.#clearFieldError(field.id);
  }

  #handleBlur(event) {
    const field = event.target;
    if (field.validity.valid) {
      this.#announce(`${field.labels[0]?.textContent} field completed`);
    }
  }

  #validateCurrentStep() {
    const currentStepPanel = this.#shadowRoot.getElementById(`step${this.#currentStep}`);
    const fields = currentStepPanel.querySelectorAll('input[required]');
    
    let isValid = true;
    fields.forEach(field => {
      if (!field.value) {
        this.#showFieldError(field.id, 'This field is required');
        isValid = false;
      } else if (!field.validity.valid) {
        this.#showFieldError(field.id, field.validationMessage);
        isValid = false;
      }
    });
    
    return isValid;
  }

  #showFieldError(fieldId, message) {
    const errorContainer = this.#shadowRoot.getElementById(`${fieldId}Error`);
    if (!errorContainer) return;
    
    const field = this.#form.querySelector(`#${fieldId}`);
    field.setAttribute('aria-invalid', 'true');
    
    errorContainer.innerHTML = `
      <div class="error-item">
        <svg class="error-icon" viewBox="0 0 24 24" fill="currentColor">
          <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
        </svg>
        <span>${message}</span>
      </div>
    `;
    errorContainer.hidden = false;
  }

  #clearFieldError(fieldId) {
    const errorContainer = this.#shadowRoot.getElementById(`${fieldId}Error`);
    const field = this.#form.querySelector(`#${fieldId}`);
    
    if (field) {
      field.removeAttribute('aria-invalid');
    }
    
    if (errorContainer) {
      errorContainer.innerHTML = '';
      errorContainer.hidden = true;
    }
  }

  #showStep(stepNumber) {
    const stepPanel = this.#shadowRoot.getElementById(`step${stepNumber}`);
    stepPanel.hidden = false;
    
    const firstField = stepPanel.querySelector('input');
    if (firstField) {
      setTimeout(() => firstField.focus(), 100);
    }
    
    this.#updateButtons();
  }

  #hideStep(stepNumber) {
    const stepPanel = this.#shadowRoot.getElementById(`step${stepNumber}`);
    stepPanel.hidden = true;
  }

  #updateStepIndicator() {
    const steps = this.#shadowRoot.querySelectorAll('.step');
    
    steps.forEach((step, index) => {
      const stepNum = index + 1;
      
      step.classList.remove('active', 'completed');
      
      if (stepNum < this.#currentStep) {
        step.classList.add('completed');
        step.querySelector('.step-number').textContent = '✓';
      } else if (stepNum === this.#currentStep) {
        step.classList.add('active');
        step.querySelector('.step-number').textContent = stepNum;
      } else {
        step.querySelector('.step-number').textContent = stepNum;
      }
    });
    
    const progressPercent = (this.#currentStep / this.#totalSteps) * 100;
    this.#progressFill.style.width = `${progressPercent}%`;
    this.#progressText.textContent = `Step ${this.#currentStep} of ${this.#totalSteps}`;
  }

  #updateButtons() {
    const prevBtn = this.#shadowRoot.getElementById('prevBtn');
    const nextBtn = this.#shadowRoot.getElementById('nextBtn');
    const submitBtn = this.#shadowRoot.getElementById('submitBtn');
    
    prevBtn.disabled = this.#currentStep <= 1;
    
    if (this.#currentStep >= this.#totalSteps) {
      nextBtn.hidden = true;
      submitBtn.hidden = false;
    } else {
      nextBtn.hidden = false;
      submitBtn.hidden = true;
    }
  }

  #generateSummary() {
    const summary = this.#shadowRoot.getElementById('summary');
    const formData = new FormData(this.#form);
    
    summary.innerHTML = `
      <dl>
        <dt>Username:</dt><dd>${formData.get('username')}</dd>
        <dt>Email:</dt><dd>${formData.get('email')}</dd>
        <dt>Name:</dt><dd>${formData.get('fullName')}</dd>
      </dl>
    `;
    
    this.#announce('Summary generated. Review your information and click Submit.');
  }

  #announce(message) {
    this.#liveRegion.textContent = message;
  }

  #announceError(message) {
    this.#assertiveRegion.textContent = message;
  }
}

customElements.define('advanced-accessible-form', AdvancedAccessibleForm);

export { AdvancedAccessibleForm };