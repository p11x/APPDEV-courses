/**
 * Custom Input Element Development - Provides reusable custom input elements
 * with built-in validation, formatting, and accessibility
 * @module forms/Custom-Input-Element-Development
 * @version 1.0.0
 * @example <custom-input type="currency" name="amount"></custom-input>
 */

const TEMPLATE = document.createElement('template');
TEMPLATE.innerHTML = `
  <style>
    :host {
      display: inline-block;
      font-family: system-ui, -apple-system, sans-serif;
    }
    :host([hidden]) {
      display: none;
    }
    :host([disabled]) {
      opacity: 0.5;
      pointer-events: none;
    }
    :host([readonly]) .input-wrapper {
      cursor: default;
    }
    .input-wrapper {
      display: flex;
      flex-direction: column;
      gap: 0.25rem;
    }
    .label {
      font-weight: 600;
      font-size: 0.875rem;
      color: #333;
    }
    .label.required::after {
      content: " *";
      color: #d32f2f;
    }
    .input-container {
      display: flex;
      align-items: stretch;
      border: 2px solid #ccc;
      border-radius: 4px;
      overflow: hidden;
      transition: border-color 0.2s, box-shadow 0.2s;
    }
    .input-container:focus-within {
      border-color: #1976d2;
      box-shadow: 0 0 0 3px rgba(25, 118, 210, 0.2);
    }
    :host([invalid]) .input-container {
      border-color: #d32f2f;
    }
    :host([invalid]) .input-container:focus-within {
      box-shadow: 0 0 0 3px rgba(211, 47, 47, 0.2);
    }
    .prefix, .suffix {
      display: flex;
      align-items: center;
      padding: 0 0.75rem;
      background: #f5f5f5;
      color: #666;
      font-size: 1rem;
      font-weight: 500;
    }
    :host([disabled]) .prefix,
    :host([disabled]) .suffix {
      background: #e0e0e0;
    }
    input {
      flex: 1;
      border: none;
      padding: 0.75rem;
      font-size: 1rem;
      font-family: inherit;
      min-width: 0;
      outline: none;
      background: transparent;
    }
    input:disabled {
      background: transparent;
    }
    input::placeholder {
      color: #999;
    }
    input::-webkit-outer-spin-button,
    input::-webkit-inner-spin-button {
      -webkit-appearance: none;
      margin: 0;
    }
    input[type="number"] {
      -moz-appearance: textfield;
    }
    .hint {
      font-size: 0.75rem;
      color: #666;
    }
    .error {
      font-size: 0.75rem;
      color: #d32f2f;
      display: flex;
      align-items: center;
      gap: 0.25rem;
    }
    .counter {
      font-size: 0.75rem;
      color: #666;
      text-align: right;
    }
    .counter.warning {
      color: #f57c00;
    }
    .counter.danger {
      color: #d32f2f;
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
  </style>
  <div class="input-wrapper">
    <label class="label" id="label"></label>
    <div class="input-container">
      <span class="prefix" id="prefix" hidden></span>
      <input id="input" type="text" aria-describedby="hint error">
      <span class="suffix" id="suffix" hidden></span>
    </div>
    <div id="hint" class="hint" hidden></div>
    <div id="error" class="error" role="alert" hidden></div>
    <div id="counter" class="counter" hidden></div>
  </div>
`;

class CustomInput extends HTMLElement {
  #shadowRoot;
  #input;
  #label;
  #prefix;
  #suffix;
  #hint;
  #error;
  #counter;

  static get observedAttributes() {
    return [
      'type', 'name', 'value', 'label', 'placeholder',
      'required', 'disabled', 'readonly', 'min', 'max',
      'minlength', 'maxlength', 'pattern', 'prefix', 'suffix',
      'hint', 'invalid', 'autocomplete'
    ];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#shadowRoot.appendChild(TEMPLATE.content.cloneNode(true));
    
    this.#input = this.#shadowRoot.getElementById('input');
    this.#label = this.#shadowRoot.getElementById('label');
    this.#prefix = this.#shadowRoot.getElementById('prefix');
    this.#suffix = this.#shadowRoot.getElementById('suffix');
    this.#hint = this.#shadowRoot.getElementById('hint');
    this.#error = this.#shadowRoot.getElementById('error');
    this.#counter = this.#shadowRoot.getElementById('counter');
  }

  connectedCallback() {
    this.#setupAttributes();
    this.#setupEventListeners();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    
    switch (name) {
      case 'label':
        this.#label.textContent = newValue;
        this.#label.classList.toggle('required', this.hasAttribute('required'));
        this.#input.setAttribute('aria-label', newValue);
        break;
      case 'placeholder':
        this.#input.placeholder = newValue;
        break;
      case 'required':
        this.#input.required = newValue !== null;
        this.#label.classList.toggle('required', newValue !== null);
        this.#input.setAttribute('aria-required', newValue !== null);
        break;
      case 'disabled':
        this.#input.disabled = newValue !== null;
        break;
      case 'readonly':
        this.#input.readOnly = newValue !== null;
        break;
      case 'min':
      case 'max':
        this.#input[name] = newValue;
        break;
      case 'minlength':
      case 'maxlength':
        this.#input[name] = newValue ? parseInt(newValue) : undefined;
        this.#updateCounter();
        break;
      case 'pattern':
        this.#input.pattern = newValue;
        break;
      case 'prefix':
        if (newValue) {
          this.#prefix.textContent = newValue;
          this.#prefix.hidden = false;
        } else {
          this.#prefix.hidden = true;
        }
        break;
      case 'suffix':
        if (newValue) {
          this.#suffix.textContent = newValue;
          this.#suffix.hidden = false;
        } else {
          this.#suffix.hidden = true;
        }
        break;
      case 'hint':
        if (newValue) {
          this.#hint.textContent = newValue;
          this.#hint.hidden = false;
        } else {
          this.#hint.hidden = true;
        }
        break;
      case 'invalid':
        this.#handleInvalidAttribute(newValue);
        break;
      case 'autocomplete':
        this.#input.autocomplete = newValue;
        break;
      case 'type':
        this.#handleTypeChange(newValue);
        break;
    }
  }

  #setupAttributes() {
    this.#input.type = this.getAttribute('type') || 'text';
    this.#input.name = this.getAttribute('name') || '';
    this.#input.id = this.getAttribute('id') || `input-${Math.random().toString(36).substr(2, 9)}`;
    
    if (this.hasAttribute('label')) {
      this.#label.textContent = this.getAttribute('label');
      this.#input.setAttribute('aria-label', this.getAttribute('label'));
    }
    
    if (this.hasAttribute('placeholder')) {
      this.#input.placeholder = this.getAttribute('placeholder');
    }
    
    if (this.hasAttribute('value')) {
      this.#input.value = this.getAttribute('value');
    }
    
    if (this.hasAttribute('required')) {
      this.#input.required = true;
      this.#label.classList.add('required');
      this.#input.setAttribute('aria-required', 'true');
    }
    
    if (this.hasAttribute('disabled')) {
      this.#input.disabled = true;
    }
    
    if (this.hasAttribute('readonly')) {
      this.#input.readOnly = true;
    }
    
    if (this.hasAttribute('min')) {
      this.#input.min = this.getAttribute('min');
    }
    
    if (this.hasAttribute('max')) {
      this.#input.max = this.getAttribute('max');
    }
    
    if (this.hasAttribute('minlength')) {
      this.#input.minLength = parseInt(this.getAttribute('minlength'));
    }
    
    if (this.hasAttribute('maxlength')) {
      this.#input.maxLength = parseInt(this.getAttribute('maxlength'));
      this.#counter.hidden = false;
    }
    
    if (this.hasAttribute('pattern')) {
      this.#input.pattern = this.getAttribute('pattern');
    }
    
    if (this.hasAttribute('prefix')) {
      this.#prefix.textContent = this.getAttribute('prefix');
      this.#prefix.hidden = false;
    }
    
    if (this.hasAttribute('suffix')) {
      this.#suffix.textContent = this.getAttribute('suffix');
      this.#suffix.hidden = false;
    }
    
    if (this.hasAttribute('hint')) {
      this.#hint.textContent = this.getAttribute('hint');
      this.#hint.hidden = false;
    }
    
    if (this.hasAttribute('autocomplete')) {
      this.#input.autocomplete = this.getAttribute('autocomplete');
    }
    
    if (this.hasAttribute('invalid')) {
      this.#handleInvalidAttribute(this.getAttribute('invalid'));
    }
    
    this.#updateCounter();
  }

  #setupEventListeners() {
    this.#input.addEventListener('input', this.#handleInput.bind(this));
    this.#input.addEventListener('blur', this.#handleBlur.bind(this));
    this.#input.addEventListener('focus', this.#handleFocus.bind(this));
    this.#input.addEventListener('invalid', this.#handleInvalid.bind(this));
    this.#input.addEventListener('keydown', this.#handleKeyDown.bind(this));
  }

  #handleInput(event) {
    this.#updateCounter();
    this.#clearError();
    
    const formattedValue = this.#formatValue(this.#input.value);
    if (formattedValue !== this.#input.value) {
      this.#input.value = formattedValue;
    }
    
    this.dispatchEvent(new CustomEvent('input-change', {
      detail: { value: this.#input.value, originalValue: event?.target?.value },
      bubbles: true,
      composed: true
    }));
    
    this.#syncAttribute('value', this.#input.value);
  }

  #handleBlur() {
    this.#validate();
    this.dispatchEvent(new CustomEvent('input-blur', {
      detail: { value: this.#input.value },
      bubbles: true,
      composed: true
    }));
  }

  #handleFocus() {
    this.dispatchEvent(new CustomEvent('input-focus', {
      detail: { value: this.#input.value },
      bubbles: true,
      composed: true
    }));
  }

  #handleInvalid(event) {
    event.preventDefault();
    this.#setError(this.#input.validationMessage);
  }

  #handleKeyDown(event) {
    if (this.getAttribute('type') === 'number') {
      return;
    }
    
    if (event.key === 'Enter') {
      this.#validate();
      this.dispatchEvent(new CustomEvent('input-submit', {
        detail: { value: this.#input.value },
        bubbles: true,
        composed: true
      }));
    }
  }

  #handleInvalidAttribute(value) {
    if (value !== null) {
      this.#input.setAttribute('aria-invalid', 'true');
      this.#setError(value);
    } else {
      this.#input.removeAttribute('aria-invalid');
      this.#clearError();
    }
  }

  #handleTypeChange(type) {
    const inputTypes = ['text', 'password', 'email', 'tel', 'url', 'number', 'search'];
    this.#input.type = inputTypes.includes(type) ? type : 'text';
  }

  #formatValue(value) {
    const type = this.getAttribute('type');
    
    switch (type) {
      case 'number':
        return value.replace(/[^0-9.-]/g, '');
      case 'email':
        return value.toLowerCase().trim();
      case 'tel':
        return value.replace(/[^\d+()- ]/g, '');
      case 'search':
        return value.trim();
      default:
        return value;
    }
  }

  #validate() {
    if (!this.#input.value) {
      if (this.#input.required) {
        this.#setError('This field is required');
        return false;
      }
    } else if (this.#input.validity.valid) {
      this.#clearError();
      return true;
    } else {
      this.#setError(this.#input.validationMessage);
      return false;
    }
    return true;
  }

  #updateCounter() {
    if (!this.hasAttribute('maxlength')) {
      this.#counter.hidden = true;
      return;
    }
    
    const maxLength = parseInt(this.getAttribute('maxlength'));
    const currentLength = this.#input.value.length;
    const remaining = maxLength - currentLength;
    
    if (remaining < 0) {
      this.#counter.textContent = `${Math.abs(remaining)} characters over limit`;
      this.#counter.classList.add('danger');
      this.#counter.classList.remove('warning');
    } else if (remaining < 10) {
      this.#counter.textContent = `${remaining} characters remaining`;
      this.#counter.classList.add('warning');
      this.#counter.classList.remove('danger');
    } else {
      this.#counter.textContent = `${currentLength}/${maxLength}`;
      this.#counter.className = 'counter';
    }
    
    this.#counter.hidden = false;
  }

  #setError(message) {
    this.#error.textContent = message;
    this.#error.hidden = false;
    this.#input.setAttribute('aria-invalid', 'true');
    this.#input.setAttribute('aria-describedby', 'error');
    
    const describedBy = this.#input.getAttribute('aria-describedby') || '';
    if (!describedBy.includes('error')) {
      this.#input.setAttribute('aria-describedby', `${describedBy} error`.trim());
    }
  }

  #clearError() {
    this.#error.textContent = '';
    this.#error.hidden = true;
    this.#input.removeAttribute('aria-invalid');
  }

  #syncAttribute(name, value) {
    if (value) {
      this.setAttribute(name, value);
    } else {
      this.removeAttribute(name);
    }
  }

  get value() {
    return this.#input.value;
  }

  set value(val) {
    this.#input.value = val;
    this.#updateCounter();
    this.dispatchEvent(new CustomEvent('input-change', {
      detail: { value: val },
      bubbles: true,
      composed: true
    }));
  }

  get validity() {
    return this.#input.validity;
  }

  get validationMessage() {
    return this.#input.validationMessage;
  }

  checkValidity() {
    return this.#input.checkValidity();
  }

  reportValidity() {
    return this.#input.reportValidity();
  }

  focus() {
    this.#input.focus();
  }

  blur() {
    this.#input.blur();
  }

  reset() {
    this.#input.value = '';
    this.#clearError();
    this.#updateCounter();
  }

  setCustomValidity(message) {
    this.#input.setCustomValidity(message);
    if (message) {
      this.#setError(message);
      this.setAttribute('invalid', message);
    } else {
      this.#clearError();
      this.removeAttribute('invalid');
    }
  }

  get inputElement() {
    return this.#input;
  }
}

customElements.define('custom-input', CustomInput);

class CurrencyInput extends CustomInput {
  static get observedAttributes() {
    return [...super.observedAttributes, 'currency', 'locale'];
  }

  constructor() {
    super();
    this.setAttribute('type', 'number');
    this.setAttribute('prefix', '$');
    this.setAttribute('min', '0');
    this.setAttribute('step', '0.01');
  }

  attributeChangedCallback(name, oldValue, newValue) {
    super.attributeChangedCallback(name, oldValue, newValue);
    
    if (name === 'currency' && newValue) {
      const currencySymbols = { USD: '$', EUR: '€', GBP: '£', JPY: '¥' };
      const symbol = currencySymbols[newValue] || newValue;
      this.setAttribute('prefix', symbol);
    }
  }
}

customElements.define('currency-input', CurrencyInput);

class PercentageInput extends CustomInput {
  constructor() {
    super();
    this.setAttribute('type', 'number');
    this.setAttribute('suffix', '%');
    this.setAttribute('min', '0');
    this.setAttribute('max', '100');
    this.setAttribute('step', '1');
  }

  #handleInput(event) {
    let value = parseFloat(event.target.value);
    
    if (value > 100) {
      event.target.value = 100;
    } else if (value < 0) {
      event.target.value = 0;
    }
    
    super.#handleInput(event);
  }
}

customElements.define('percentage-input', PercentageInput);

export { CustomInput, CurrencyInput, PercentageInput };