/**
 * Complex Form State Management - Handles complex form state including
 * conditional fields, dependent validation, undo/redo, form history,
 * and multi-step form persistence
 * @module forms/Form-State-Management
 * @version 1.0.0
 * @example <complex-state-form></complex-state-form>
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
    .form-panel {
      background: #fff;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      overflow: hidden;
    }
    .panel-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1rem 1.5rem;
      background: #f5f5f5;
      border-bottom: 1px solid #e0e0e0;
    }
    .panel-title {
      font-size: 1.25rem;
      font-weight: 700;
      color: #333;
      margin: 0;
    }
    .panel-actions {
      display: flex;
      gap: 0.5rem;
    }
    .action-btn {
      padding: 0.5rem;
      border: none;
      background: transparent;
      color: #666;
      cursor: pointer;
      border-radius: 4px;
      transition: background-color 0.2s, color 0.2s;
    }
    .action-btn:hover {
      background: #e0e0e0;
      color: #333;
    }
    .action-btn:disabled {
      opacity: 0.4;
      cursor: not-allowed;
    }
    .action-btn svg {
      width: 20px;
      height: 20px;
    }
    .panel-content {
      padding: 1.5rem;
    }
    .form-section {
      margin-bottom: 1.5rem;
    }
    .section-title {
      font-size: 1rem;
      font-weight: 600;
      color: #333;
      margin-bottom: 1rem;
      padding-bottom: 0.5rem;
      border-bottom: 2px solid #e0e0e0;
    }
    .form-row {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 1rem;
    }
    .form-group {
      margin-bottom: 1rem;
    }
    .form-group.full-width {
      grid-column: span 2;
    }
    label {
      display: block;
      font-weight: 600;
      font-size: 0.875rem;
      color: #333;
      margin-bottom: 0.25rem;
    }
    input, select, textarea {
      width: 100%;
      padding: 0.625rem;
      border: 2px solid #ddd;
      border-radius: 4px;
      font-size: 0.9375rem;
      transition: border-color 0.2s, box-shadow 0.2s;
    }
    input:focus, select:focus, textarea:focus {
      outline: none;
      border-color: #1976d2;
      box-shadow: 0 0 0 3px rgba(25, 118, 210, 0.2);
    }
    input[disabled], select[disabled], textarea[disabled] {
      background: #f5f5f5;
      cursor: not-allowed;
    }
    input.hidden, select.hidden, textarea.hidden {
      display: none;
    }
    .checkbox-group, .radio-group {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }
    .checkbox-item, .radio-item {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }
    .checkbox-item label, .radio-item label {
      display: inline;
      font-weight: normal;
      margin-bottom: 0;
      cursor: pointer;
    }
    .state-indicator {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.5rem 0.75rem;
      background: #f5f5f5;
      border-radius: 4px;
      font-size: 0.75rem;
      color: #666;
    }
    .state-dot {
      width: 8px;
      height: 8px;
      border-radius: 50%;
    }
    .state-dot.modified {
      background: #ff9800;
    }
    .state-dot.saved {
      background: #4caf50;
    }
    .state-dot.error {
      background: #f44336;
    }
    .history-list {
      max-height: 200px;
      overflow-y: auto;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
    }
    .history-item {
      padding: 0.5rem;
      border-bottom: 1px solid #eee;
      font-size: 0.75rem;
      cursor: pointer;
    }
    .history-item:hover {
      background: #f5f5f5;
    }
    .history-item.current {
      background: #e3f2fd;
      font-weight: 600;
    }
    .history-item:last-child {
      border-bottom: none;
    }
    .history-timestamp {
      color: #999;
      margin-right: 0.5rem;
    }
    .conditional-section {
      padding: 1rem;
      background: #fafafa;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      margin-top: 0.5rem;
    }
    .conditional-section.hidden {
      display: none;
    }
    .panel-footer {
      display: flex;
      justify-content: space-between;
      padding: 1rem 1.5rem;
      background: #f9f9f9;
      border-top: 1px solid #e0e0e0;
    }
    .footer-info {
      font-size: 0.75rem;
      color: #666;
    }
    .button-group {
      display: flex;
      gap: 0.5rem;
    }
    button {
      padding: 0.625rem 1.25rem;
      border: none;
      border-radius: 4px;
      font-size: 0.875rem;
      font-weight: 600;
      cursor: pointer;
      transition: background-color 0.2s;
    }
    button.primary {
      background: #1976d2;
      color: white;
    }
    button.primary:hover {
      background: #1565c0;
    }
    button.secondary {
      background: #f5f5f5;
      color: #333;
    }
    button.secondary:hover {
      background: #e0e0e0;
    }
    button.danger {
      background: #f44336;
      color: white;
    }
    button.danger:hover {
      background: #d32f2f;
    }
  </style>
  
  <div class="form-panel">
    <div class="panel-header">
      <h2 class="panel-title">Complex Registration</h2>
      <div class="panel-actions">
        <button class="action-btn" id="undoBtn" title="Undo" disabled>
          <svg viewBox="0 0 24 24" fill="currentColor"><path d="M12.5 8c-2.65 0-5.05.99-6.9 2.6L2 7v9h9l-3.62-3.62c1.39-1.16 3.16-1.88 5.12-1.88 3.54 0 6.55 2.31 7.6 5.5l2.37-.78C21.08 11.03 17.15 8 12.5 8z"/></svg>
        </button>
        <button class="action-btn" id="redoBtn" title="Redo" disabled>
          <svg viewBox="0 0 24 24" fill="currentColor"><path d="M18.4 10.6C16.55 8.99 14.15 8 12 8c-4.65 0-8.58 3.03-9.96 7.22L3.9 16c1.05-3.19 4.05-5.5 7.6-5.5 1.95 0 3.73.72 5.12 1.88L13 16h9V7l-3.6 3.6z"/></svg>
        </button>
        <button class="action-btn" id="historyBtn" title="History">
          <svg viewBox="0 0 24 24" fill="currentColor"><path d="M13 3a9 9 0 0 0-9 9H1l3.89 3.89.07.14L9 12H6c0-3.87 3.13-7 7-7s7 3.13 7 7-3.13 7-7 7c-1.93 0-3.68-.79-4.94-2.06l-1.42 1.42A8.954 8.954 0 0 0 13 21a9 9 0 0 0 0-18zm-1 5v5l4.28 2.54.72-1.21-3.5-2.08V8H12z"/></svg>
        </button>
        <button class="action-btn" id="saveBtn" title="Save">
          <svg viewBox="0 0 24 24" fill="currentColor"><path d="M17 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.1 0 2-.9 2-2V7l-4-4zm-5 16c-1.66 0-3-1.34-3-3s1.34-3 3-3 3 1.34 3 3-1.34 3-3 3zm3-10H5V5h10v4z"/></svg>
        </button>
        <button class="action-btn" id="resetBtn" title="Reset">
          <svg viewBox="0 0 24 24" fill="currentColor"><path d="M12 5V1L7 6l5 5V7c3.31 0 6 2.69 6 6s-2.69 6-6 6-6-2.69-6-6H4c0 4.42 3.58 8 8 8s8-3.58 8-8-3.58-8-8-8z"/></svg>
        </button>
      </div>
    </div>
    
    <div class="panel-content">
      <div class="form-section">
        <h3 class="section-title">Account Type</h3>
        <div class="form-group">
          <label>Select Account Type</label>
          <div class="radio-group" role="radiogroup" aria-required="true">
            <div class="radio-item">
              <input type="radio" id="typePersonal" name="accountType" value="personal" checked data-triggers="personal">
              <label for="typePersonal">Personal</label>
            </div>
            <div class="radio-item">
              <input type="radio" id="typeBusiness" name="accountType" value="business" data-triggers="business">
              <label for="typeBusiness">Business</label>
            </div>
            <div class="radio-item">
              <input type="radio" id="typeEnterprise" name="accountType" value="enterprise" data-triggers="enterprise">
              <label for="typeEnterprise">Enterprise</label>
            </div>
          </div>
        </div>
      </div>
      
      <div class="form-section" id="personalSection">
        <h3 class="section-title">Personal Information</h3>
        <div class="form-row">
          <div class="form-group">
            <label for="firstName">First Name *</label>
            <input type="text" id="firstName" name="firstName" required autocomplete="given-name">
          </div>
          <div class="form-group">
            <label for="lastName">Last Name *</label>
            <input type="text" id="lastName" name="lastName" required autocomplete="family-name">
          </div>
          <div class="form-group full-width">
            <label for="personalEmail">Personal Email *</label>
            <input type="email" id="personalEmail" name="personalEmail" required autocomplete="email">
          </div>
        </div>
      </div>
      
      <div class="form-section hidden" id="businessSection">
        <h3 class="section-title">Business Information</h3>
        <div class="form-row">
          <div class="form-group">
            <label for="companyName">Company Name *</label>
            <input type="text" id="companyName" name="companyName">
          </div>
          <div class="form-group">
            <label for="businessEmail">Business Email *</label>
            <input type="email" id="businessEmail" name="businessEmail">
          </div>
          <div class="form-group">
            <label for="companySize">Company Size</label>
            <select id="companySize" name="companySize">
              <option value="">Select size</option>
              <option value="1-10">1-10 employees</option>
              <option value="11-50">11-50 employees</option>
              <option value="51-200">51-200 employees</option>
              <option value="200+">200+ employees</option>
            </select>
          </div>
          <div class="form-group">
            <label for="industry">Industry</label>
            <select id="industry" name="industry">
              <option value="">Select industry</option>
              <option value="tech">Technology</option>
              <option value="finance">Finance</option>
              <option value="healthcare">Healthcare</option>
              <option value="retail">Retail</option>
              <option value="other">Other</option>
            </select>
          </div>
        </div>
      </div>
      
      <div class="form-section hidden" id="enterpriseSection">
        <h3 class="section-title">Enterprise Information</h3>
        <div class="form-row">
          <div class="form-group full-width">
            <label for="orgName">Organization Name *</label>
            <input type="text" id="orgName" name="orgName">
          </div>
          <div class="form-group">
            <label for="department">Department *</label>
            <input type="text" id="department" name="department">
          </div>
          <div class="form-group">
            <label for="role">Job Role *</label>
            <input type="text" id="role" name="role">
          </div>
          <div class="form-group full-width">
            <label for="workEmail">Work Email *</label>
            <input type="email" id="workEmail" name="workEmail">
          </div>
        </div>
        
        <div class="conditional-section hidden" id="ssorequirement">
          <div class="form-group">
            <label for="idpUrl">Identity Provider URL</label>
            <input type="url" id="idpUrl" name="idpUrl" placeholder="https://your-idp.example.com">
          </div>
          <div class="form-group">
            <div class="checkbox-item">
              <input type="checkbox" id="requireSso" name="requireSso">
              <label for="requireSso">Require SSO for all users</label>
            </div>
          </div>
        </div>
      </div>
      
      <div class="form-section">
        <h3 class="section-title">Preferences</h3>
        <div class="form-group">
          <label for="language">Language Preference</label>
          <select id="language" name="language">
            <option value="en">English</option>
            <option value="es">Spanish</option>
            <option value="fr">French</option>
            <option value="de">German</option>
          </select>
        </div>
        <div class="form-group">
          <div class="checkbox-group">
            <div class="checkbox-item">
              <input type="checkbox" id="newsletter" name="newsletter">
              <label for="newsletter">Subscribe to newsletter</label>
            </div>
            <div class="checkbox-item">
              <input type="checkbox" id="offers" name="offers">
              <label for="offers">Receive special offers</label>
            </div>
          </div>
        </div>
      </div>
      
      <div class="history-panel hidden" id="historyPanel">
        <h3 class="section-title">History</h3>
        <div class="history-list" id="historyList" role="listbox"></div>
      </div>
    </div>
    
    <div class="panel-footer">
      <div class="state-indicator">
        <span class="state-dot saved" id="stateDot"></span>
        <span id="stateText">Saved</span>
      </div>
      <div class="button-group">
        <button class="secondary" id="cancelBtn">Cancel</button>
        <button class="primary" id="submitBtn">Submit</button>
      </div>
    </div>
  </div>
`;

class ComplexStateForm extends HTMLElement {
  #shadowRoot;
  #form;
  #stateHistory;
  #currentStateIndex;
  #undoStack;
  #redoStack;
  #isModified;
  #lastSavedState;

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#shadowRoot.appendChild(TEMPLATE.content.cloneNode(true));
    
    this.#stateHistory = [];
    this.#currentStateIndex = -1;
    this.#undoStack = [];
    this.#redoStack = [];
    this.#isModified = false;
    this.#lastSavedState = null;
  }

  connectedCallback() {
    this.#form = this.#shadowRoot.querySelector('form') || document.createElement('form');
    this.#setupEventListeners();
    this.#setupConditionalFields();
    this.#saveState();
  }

  #setupEventListeners() {
    const undoBtn = this.#shadowRoot.getElementById('undoBtn');
    const redoBtn = this.#shadowRoot.getElementById('redoBtn');
    const historyBtn = this.#shadowRoot.getElementById('historyBtn');
    const saveBtn = this.#shadowRoot.getElementById('saveBtn');
    const resetBtn = this.#shadowRoot.getElementById('resetBtn');
    const submitBtn = this.#shadowRoot.getElementById('submitBtn');
    const cancelBtn = this.#shadowRoot.getElementById('cancelBtn');
    
    undoBtn.addEventListener('click', this.#handleUndo.bind(this));
    redoBtn.addEventListener('click', this.#handleRedo.bind(this));
    historyBtn.addEventListener('click', this.#toggleHistory.bind(this));
    saveBtn.addEventListener('click', this.#handleSave.bind(this));
    resetBtn.addEventListener('click', this.#handleReset.bind(this));
    submitBtn.addEventListener('click', this.#handleSubmit.bind(this));
    cancelBtn.addEventListener('click', this.#handleCancel.bind(this));
    
    const allInputs = this.#shadowRoot.querySelectorAll('input, select, textarea');
    allInputs.forEach(input => {
      input.addEventListener('change', this.#handleFieldChange.bind(this));
      input.addEventListener('input', this.#handleFieldInput.bind(this));
    });
    
    const accountTypes = this.#shadowRoot.querySelectorAll('input[name="accountType"]');
    accountTypes.forEach(input => {
      input.addEventListener('change', this.#handleAccountTypeChange.bind(this));
    });
  }

  #setupConditionalFields() {
    const personalSection = this.#shadowRoot.getElementById('personalSection');
    const businessSection = this.#shadowRoot.getElementById('businessSection');
    const enterpriseSection = this.#shadowRoot.getElementById('enterpriseSection');
    
    this.#conditionalFields = {
      personal: personalSection,
      business: businessSection,
      enterprise: enterpriseSection
    };
  }

  #handleFieldChange(event) {
    this.#markAsModified();
    this.#saveToUndoStack();
  }

  #handleFieldInput(event) {
    this.#markAsModified();
  }

  #handleAccountTypeChange(event) {
    const selectedType = event.target.value;
    
    Object.keys(this.#conditionalFields).forEach(type => {
      const section = this.#conditionalFields[type];
      if (section) {
        if (type === selectedType) {
          section.classList.remove('hidden');
          this.#enableFields(section, true);
        } else {
          section.classList.add('hidden');
          this.#enableFields(section, false);
        }
      }
    });
    
    if (selectedType === 'enterprise') {
      const ssoSection = this.#shadowRoot.getElementById('ssorequirement');
      ssoSection.classList.remove('hidden');
    } else {
      const ssoSection = this.#shadowRoot.getElementById('ssorequirement');
      ssoSection.classList.add('hidden');
    }
    
    this.#saveState();
  }

  #enableFields(section, enabled) {
    const fields = section.querySelectorAll('input, select, textarea');
    fields.forEach(field => {
      field.disabled = !enabled;
    });
  }

  #handleUndo() {
    if (this.#undoStack.length === 0) return;
    
    const currentState = this.#captureState();
    this.#redoStack.push(currentState);
    
    const previousState = this.#undoStack.pop();
    this.#restoreState(previousState);
    
    this.#updateUndoRedoButtons();
    this.#updateHistoryUI();
  }

  #handleRedo() {
    if (this.#redoStack.length === 0) return;
    
    const currentState = this.#captureState();
    this.#undoStack.push(currentState);
    
    const nextState = this.#redoStack.pop();
    this.#restoreState(nextState);
    
    this.#updateUndoRedoButtons();
    this.#updateHistoryUI();
  }

  #toggleHistory() {
    const historyPanel = this.#shadowRoot.getElementById('historyPanel');
    historyPanel.classList.toggle('hidden');
    
    if (!historyPanel.classList.contains('hidden')) {
      this.#updateHistoryUI();
    }
  }

  #handleSave() {
    const state = this.#captureState();
    this.#lastSavedState = JSON.stringify(state);
    this.#saveState();
    this.#markAsSaved();
    
    this.dispatchEvent(new CustomEvent('form-save', {
      detail: { state },
      bubbles: true,
      composed: true
    }));
  }

  #handleReset() {
    if (this.#lastSavedState) {
      this.#restoreState(JSON.parse(this.#lastSavedState));
    } else {
      const inputs = this.#shadowRoot.querySelectorAll('input, select, textarea');
      inputs.forEach(input => {
        if (input.type === 'checkbox' || input.type === 'radio') {
          input.checked = input.defaultChecked;
        } else {
          input.value = input.defaultValue;
        }
      });
    }
    
    this.#clearUndoHistory();
    this.#markAsSaved();
  }

  #handleSubmit() {
    if (!this.#validateForm()) {
      return;
    }
    
    const state = this.#captureState();
    
    this.dispatchEvent(new CustomEvent('form-submit', {
      detail: { state },
      bubbles: true,
      composed: true
    }));
    
    console.log('Form submitted:', state);
  }

  #handleCancel() {
    if (this.#isModified) {
      if (!confirm('You have unsaved changes. Are you sure you want to cancel?')) {
        return;
      }
    }
    
    if (this.#lastSavedState) {
      this.#restoreState(JSON.parse(this.#lastSavedState));
    } else {
      this.#handleReset();
    }
    
    this.dispatchEvent(new CustomEvent('form-cancel', {
      bubbles: true,
      composed: true
    }));
  }

  #captureState() {
    const state = {};
    
    const inputs = this.#shadowRoot.querySelectorAll('input, select, textarea');
    inputs.forEach(input => {
      if (input.type === 'checkbox' || input.type === 'radio') {
        state[input.name] = input.checked;
      } else {
        state[input.name] = input.value;
      }
    });
    
    state.timestamp = Date.now();
    return state;
  }

  #restoreState(state) {
    const stateObj = typeof state === 'string' ? JSON.parse(state) : state;
    
    Object.keys(stateObj).forEach(key => {
      if (key === 'timestamp') return;
      
      const input = this.#shadowRoot.querySelector(`[name="${key}"]`) || 
                   this.#shadowRoot.querySelector(`#${key}`);
      
      if (input) {
        if (input.type === 'checkbox' || input.type === 'radio') {
          input.checked = stateObj[key];
          input.dispatchEvent(new Event('change'));
        } else {
          input.value = stateObj[key];
        }
      }
    });
  }

  #saveState() {
    const state = this.#captureState();
    this.#stateHistory.push(state);
    this.#currentStateIndex = this.#stateHistory.length - 1;
    
    if (this.#stateHistory.length > 50) {
      this.#stateHistory.shift();
      this.#currentStateIndex--;
    }
    
    this.#updateHistoryUI();
  }

  #saveToUndoStack() {
    const currentState = this.#captureState();
    this.#undoStack.push(currentState);
    this.#redoStack = [];
    
    if (this.#undoStack.length > 50) {
      this.#undoStack.shift();
    }
    
    this.#updateUndoRedoButtons();
  }

  #validateForm() {
    const requiredFields = this.#shadowRoot.querySelectorAll('[required]');
    let isValid = true;
    
    requiredFields.forEach(field => {
      if (!field.value && !field.disabled) {
        field.setAttribute('aria-invalid', 'true');
        isValid = false;
        
        field.addEventListener('input', function handler() {
          field.removeAttribute('aria-invalid');
          field.removeEventListener('input', handler);
        });
      }
    });
    
    return isValid;
  }

  #markAsModified() {
    this.#isModified = true;
    const stateDot = this.#shadowRoot.getElementById('stateDot');
    const stateText = this.#shadowRoot.getElementById('stateText');
    
    stateDot.classList.remove('saved', 'error');
    stateDot.classList.add('modified');
    stateText.textContent = 'Modified';
  }

  #markAsSaved() {
    this.#isModified = false;
    const stateDot = this.#shadowRoot.getElementById('stateDot');
    const stateText = this.#shadowRoot.getElementById('stateText');
    
    stateDot.classList.remove('modified', 'error');
    stateDot.classList.add('saved');
    stateText.textContent = 'Saved';
  }

  #clearUndoHistory() {
    this.#undoStack = [];
    this.#redoStack = [];
    this.#updateUndoRedoButtons();
  }

  #updateUndoRedoButtons() {
    const undoBtn = this.#shadowRoot.getElementById('undoBtn');
    const redoBtn = this.#shadowRoot.getElementById('redoBtn');
    
    undoBtn.disabled = this.#undoStack.length === 0;
    redoBtn.disabled = this.#redoStack.length === 0;
  }

  #updateHistoryUI() {
    const historyList = this.#shadowRoot.getElementById('historyList');
    historyList.innerHTML = '';
    
    this.#stateHistory.forEach((state, index) => {
      const item = document.createElement('div');
      item.className = 'history-item';
      if (index === this.#currentStateIndex) {
        item.classList.add('current');
      }
      
      const timestamp = new Date(state.timestamp);
      item.innerHTML = `<span class="history-timestamp">${timestamp.toLocaleTimeString()}</span>State ${index + 1}`;
      
      item.addEventListener('click', () => {
        this.#restoreState(state);
        this.#currentStateIndex = index;
        this.#updateHistoryUI();
      });
      
      historyList.appendChild(item);
    });
  }

  getFormData() {
    return this.#captureState();
  }

  setFormData(data) {
    this.#restoreState(data);
  }

  resetForm() {
    this.#handleReset();
  }
}

customElements.define('complex-state-form', ComplexStateForm);

export { ComplexStateForm };