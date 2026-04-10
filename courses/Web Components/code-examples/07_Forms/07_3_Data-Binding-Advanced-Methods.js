/**
 * Two-Way Data Binding - Implements reactive two-way binding for form elements
 * with Web Components using property observers and change tracking
 * @module forms/Data-Binding-Advanced-Methods
 * @version 1.0.0
 * @example <data-binding-form></data-binding-form>
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
      background: #fff;
      border-radius: 8px;
      padding: 1.5rem;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }
    .section-title {
      font-size: 1.25rem;
      font-weight: 700;
      margin-bottom: 1rem;
      color: #333;
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
    input, select {
      width: 100%;
      padding: 0.75rem;
      border: 2px solid #ddd;
      border-radius: 4px;
      font-size: 1rem;
      transition: border-color 0.2s;
    }
    input:focus, select:focus {
      outline: none;
      border-color: #4a90d9;
    }
    input[type="checkbox"], input[type="radio"] {
      width: auto;
      margin-right: 0.5rem;
    }
    .checkbox-group, .radio-group {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }
    .checkbox-item, .radio-item {
      display: flex;
      align-items: center;
    }
    .checkbox-item label, .radio-item label {
      display: inline;
      font-weight: normal;
      margin-bottom: 0;
    }
    .data-preview {
      background: #f5f5f5;
      border-radius: 4px;
      padding: 1rem;
      margin-top: 1.5rem;
    }
    .data-preview h3 {
      font-size: 1rem;
      margin-bottom: 0.5rem;
    }
    .data-preview pre {
      background: #282c34;
      color: #abb2bf;
      padding: 1rem;
      border-radius: 4px;
      overflow-x: auto;
      font-size: 0.875rem;
      max-height: 200px;
      overflow-y: auto;
    }
    .dirty-indicator {
      display: inline-block;
      width: 8px;
      height: 8px;
      border-radius: 50%;
      margin-left: 0.5rem;
    }
    .dirty-indicator.dirty {
      background: #ff9800;
    }
    .dirty-indicator.pristine {
      background: #4caf50;
    }
    .change-indicator {
      background: #e3f2fd;
      padding: 0.5rem;
      border-radius: 4px;
      font-size: 0.875rem;
      margin-bottom: 1rem;
    }
    button {
      padding: 0.75rem 1.5rem;
      border: none;
      border-radius: 4px;
      font-size: 1rem;
      font-weight: 600;
      cursor: pointer;
      margin-right: 0.5rem;
      transition: background-color 0.2s;
    }
    button.primary {
      background-color: #4a90d9;
      color: white;
    }
    button.primary:hover {
      background-color: #3a7bc8;
    }
    button.secondary {
      background-color: #f5f5f5;
      color: #333;
    }
    button.secondary:hover {
      background-color: #e0e0e0;
    }
  </style>
  <div class="form-container">
    <div class="section-title">
      User Profile
      <span class="dirty-indicator pristine" title="Form state"></span>
    </div>
    <div class="change-indicator" hidden>Changes pending</div>
    
    <form id="dataForm">
      <div class="form-group">
        <label for="username">Username</label>
        <input type="text" id="username" name="username" data-bind="username">
      </div>
      
      <div class="form-group">
        <label for="email">Email</label>
        <input type="email" id="email" name="email" data-bind="email">
      </div>
      
      <div class="form-group">
        <label for="firstName">First Name</label>
        <input type="text" id="firstName" name="firstName" data-bind="firstName">
      </div>
      
      <div class="form-group">
        <label for="lastName">Last Name</label>
        <input type="text" id="lastName" name="lastName" data-bind="lastName">
      </div>
      
      <div class="form-group">
        <label for="age">Age</label>
        <input type="number" id="age" name="age" data-bind="age">
      </div>
      
      <div class="form-group">
        <label for="country">Country</label>
        <select id="country" name="country" data-bind="country">
          <option value="">Select a country</option>
          <option value="us">United States</option>
          <option value="uk">United Kingdom</option>
          <option value="ca">Canada</option>
          <option value="au">Australia</option>
          <option value="de">Germany</option>
        </select>
      </div>
      
      <div class="form-group">
        <label>Preferences</label>
        <div class="checkbox-group">
          <div class="checkbox-item">
            <input type="checkbox" id="newsletter" name="newsletter" data-bind="newsletter">
            <label for="newsletter">Subscribe to newsletter</label>
          </div>
          <div class="checkbox-item">
            <input type="checkbox" id="notifications" name="notifications" data-bind="notifications">
            <label for="notifications">Enable notifications</label>
          </div>
          <div class="checkbox-item">
            <input type="checkbox" id="publicProfile" name="publicProfile" data-bind="publicProfile">
            <label for="publicProfile">Public profile</label>
          </div>
        </div>
      </div>
      
      <div class="form-group">
        <label>Account Type</label>
        <div class="radio-group">
          <div class="radio-item">
            <input type="radio" id="free" name="accountType" value="free" data-bind="accountType">
            <label for="free">Free</label>
          </div>
          <div class="radio-item">
            <input type="radio" id="premium" name="accountType" value="premium" data-bind="accountType">
            <label for="premium">Premium</label>
          </div>
          <div class="radio-item">
            <input type="radio" id="enterprise" name="accountType" value="enterprise" data-bind="accountType">
            <label for="enterprise">Enterprise</label>
          </div>
        </div>
      </div>
    </form>
    
    <div class="form-actions">
      <button type="button" class="primary" id="saveBtn">Save Changes</button>
      <button type="button" class="secondary" id="revertBtn">Revert Changes</button>
      <button type="button" class="secondary" id="resetBtn">Reset to Defaults</button>
    </div>
    
    <div class="data-preview">
      <h3>Current Data State</h3>
      <pre id="dataPreview"></pre>
    </div>
  </div>
`;

class DataBindingForm extends HTMLElement {
  #shadowRoot;
  #form;
  #dataModel;
  #originalData;
  #boundFields;
  #dirtyFields;
  #changeCallbacks;
  #propertyObservers;

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#shadowRoot.appendChild(TEMPLATE.content.cloneNode(true));
    
    this.#dataModel = {};
    this.#originalData = {};
    this.#boundFields = new Map();
    this.#dirtyFields = new Set();
    this.#changeCallbacks = [];
    this.#propertyObservers = new Map();
  }

  connectedCallback() {
    this.#form = this.#shadowRoot.getElementById('dataForm');
    this.#setupDataBinding();
    this.#setupEventListeners();
    this.#updatePreview();
  }

  #setupDataBinding() {
    const boundElements = this.#form.querySelectorAll('[data-bind]');
    
    boundElements.forEach(element => {
      const propertyName = element.getAttribute('data-bind');
      
      this.#boundFields.set(propertyName, element);
      
      if (element.type === 'checkbox' || element.type === 'radio') {
        this.#originalData[propertyName] = element.checked ? element.value : null;
      } else {
        this.#originalData[propertyName] = element.value;
      }
    });
    
    this.#dataModel = { ...this.#originalData };
  }

  #setupEventListeners() {
    this.#form.addEventListener('input', this.#handleInput.bind(this));
    this.#form.addEventListener('change', this.#handleChange.bind(this));
    
    const saveBtn = this.#shadowRoot.getElementById('saveBtn');
    const revertBtn = this.#shadowRoot.getElementById('revertBtn');
    const resetBtn = this.#shadowRoot.getElementById('resetBtn');
    
    saveBtn.addEventListener('click', this.#handleSave.bind(this));
    revertBtn.addEventListener('click', this.#handleRevert.bind(this));
    resetBtn.addEventListener('click', this.#handleReset.bind(this));
  }

  #handleInput(event) {
    const element = event.target;
    const propertyName = element.getAttribute('data-bind');
    
    if (!propertyName) return;
    
    this.#updateProperty(propertyName, element);
    this.#trackDirtyState(propertyName);
    this.#triggerChangeCallbacks(propertyName, this.#dataModel[propertyName]);
    this.#updatePreview();
  }

  #handleChange(event) {
    const element = event.target;
    const propertyName = element.getAttribute('data-bind');
    
    if (!propertyName) return;
    
    this.#updateProperty(propertyName, element);
    this.#trackDirtyState(propertyName);
    this.#triggerChangeCallbacks(propertyName, this.#dataModel[propertyName]);
    this.#updatePreview();
  }

  #updateProperty(propertyName, element) {
    let value;
    
    if (element.type === 'checkbox') {
      value = element.checked ? element.value : false;
    } else if (element.type === 'radio') {
      value = element.checked ? element.value : null;
    } else if (element.type === 'number') {
      value = element.value ? parseFloat(element.value) : null;
    } else {
      value = element.value;
    }
    
    this.#dataModel[propertyName] = value;
    this.#notifyPropertyObserver(propertyName, value);
  }

  #trackDirtyState(propertyName) {
    const originalValue = this.#originalData[propertyName];
    const currentValue = this.#dataModel[propertyName];
    
    const isDirty = JSON.stringify(originalValue) !== JSON.stringify(currentValue);
    
    if (isDirty) {
      this.#dirtyFields.add(propertyName);
    } else {
      this.#dirtyFields.delete(propertyName);
    }
    
    this.#updateDirtyIndicator();
  }

  #updateDirtyIndicator() {
    const indicator = this.#shadowRoot.querySelector('.dirty-indicator');
    const changeIndicator = this.#shadowRoot.querySelector('.change-indicator');
    
    if (this.#dirtyFields.size > 0) {
      indicator.classList.remove('pristine');
      indicator.classList.add('dirty');
      indicator.title = `${this.#dirtyFields.size} field(s) modified`;
      changeIndicator.hidden = false;
    } else {
      indicator.classList.remove('dirty');
      indicator.classList.add('pristine');
      indicator.title = 'No changes';
      changeIndicator.hidden = true;
    }
  }

  #notifyPropertyObserver(propertyName, value) {
    const observer = this.#propertyObservers.get(propertyName);
    if (observer) {
      observer(value, this.#dataModel);
    }
  }

  #triggerChangeCallbacks(propertyName, value) {
    this.#changeCallbacks.forEach(callback => {
      try {
        callback(propertyName, value, this.#dataModel);
      } catch (error) {
        console.error('Change callback error:', error);
      }
    });
  }

  #updatePreview() {
    const preview = this.#shadowRoot.getElementById('dataPreview');
    const state = {
      data: this.#dataModel,
      dirty: Array.from(this.#dirtyFields),
      isDirty: this.#dirtyFields.size > 0
    };
    preview.textContent = JSON.stringify(state, null, 2);
  }

  #handleSave() {
    this.#originalData = { ...this.#dataModel };
    this.#dirtyFields.clear();
    this.#updateDirtyIndicator();
    
    this.dispatchEvent(new CustomEvent('data-save', {
      detail: { data: this.#dataModel, dirtyFields: Array.from(this.#dirtyFields) },
      bubbles: true,
      composed: true
    }));
    
    console.log('Data saved:', this.#dataModel);
  }

  #handleRevert() {
    this.#boundFields.forEach((element, propertyName) => {
      this.#revertField(element, propertyName);
    });
    
    this.#dirtyFields.clear();
    this.#updateDirtyIndicator();
    this.#updatePreview();
    
    this.dispatchEvent(new CustomEvent('data-revert', {
      detail: { data: this.#dataModel },
      bubbles: true,
      composed: true
    }));
  }

  #revertField(element, propertyName) {
    const originalValue = this.#originalData[propertyName];
    
    if (element.type === 'checkbox' || element.type === 'radio') {
      element.checked = element.value === originalValue;
    } else {
      element.value = originalValue || '';
    }
    
    this.#dataModel[propertyName] = originalValue;
  }

  #handleReset() {
    this.#boundFields.forEach((element, propertyName) => {
      this.#resetField(element, propertyName);
    });
    
    this.#dirtyFields.clear();
    this.#updateDirtyIndicator();
    this.#updatePreview();
    
    this.dispatchEvent(new CustomEvent('data-reset', {
      bubbles: true,
      composed: true
    }));
  }

  #resetField(element, propertyName) {
    const defaultValue = this.#getDefaultValue(propertyName);
    
    if (element.type === 'checkbox' || element.type === 'radio') {
      element.checked = element.value === defaultValue;
    } else {
      element.value = defaultValue || '';
    }
    
    this.#dataModel[propertyName] = defaultValue;
    this.#originalData[propertyName] = defaultValue;
  }

  #getDefaultValue(propertyName) {
    const defaults = {
      username: '',
      email: '',
      firstName: '',
      lastName: '',
      age: null,
      country: '',
      newsletter: false,
      notifications: false,
      publicProfile: false,
      accountType: 'free'
    };
    return defaults[propertyName];
  }

  getData() {
    return { ...this.#dataModel };
  }

  setData(data) {
    Object.keys(data).forEach(propertyName => {
      this.#setProperty(propertyName, data[propertyName]);
    });
    this.#updatePreview();
  }

  #setProperty(propertyName, value) {
    const element = this.#boundFields.get(propertyName);
    if (!element) return;
    
    if (element.type === 'checkbox' || element.type === 'radio') {
      element.checked = element.value === value;
    } else {
      element.value = value;
    }
    
    this.#dataModel[propertyName] = value;
    this.#trackDirtyState(propertyName);
  }

  getProperty(propertyName) {
    return this.#dataModel[propertyName];
  }

  setProperty(propertyName, value) {
    this.#setProperty(propertyName, value);
    this.#updatePreview();
  }

  observeProperty(propertyName, observer) {
    this.#propertyObservers.set(propertyName, observer);
  }

  unobserveProperty(propertyName) {
    this.#propertyObservers.delete(propertyName);
  }

  onChange(callback) {
    this.#changeCallbacks.push(callback);
  }

  isDirty() {
    return this.#dirtyFields.size > 0;
  }

  getDirtyFields() {
    return Array.from(this.#dirtyFields);
  }

  revert() {
    this.#handleRevert();
  }

  save() {
    this.#handleSave();
  }

  reset() {
    this.#handleReset();
  }

  validate() {
    const errors = [];
    
    Object.keys(this.#dataModel).forEach(propertyName => {
      const error = this.#validateProperty(propertyName, this.#dataModel[propertyName]);
      if (error) {
        errors.push(error);
      }
    });
    
    return errors;
  }

  #validateProperty(propertyName, value) {
    const validators = {
      username: (v) => !v ? 'Username is required' : null,
      email: (v) => !v ? 'Email is required' : !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) ? 'Invalid email' : null,
      firstName: (v) => !v ? 'First name is required' : null,
      lastName: (v) => !v ? 'Last name is required' : null,
      age: (v) => v !== null && (v < 13 || v > 120) ? 'Age must be between 13 and 120' : null,
      country: (v) => !v ? 'Country is required' : null
    };
    
    const validator = validators[propertyName];
    if (validator) {
      return validator(value);
    }
    return null;
  }
}

customElements.define('data-binding-form', DataBindingForm);

export { DataBindingForm };