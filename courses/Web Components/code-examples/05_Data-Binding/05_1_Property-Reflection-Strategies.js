/**
 * Property Observers and Reflection Strategies
 * Implements property change detection, reflection between properties and attributes
 * @module data-binding/05_1_Property-Reflection-Strategies
 * @version 1.0.0
 * @example <property-observer-element id="observer" message="Hello World" count="42"></property-observer-element>
 */

const OBSERVER_CONFIG = {
  observedAttributes: ['message', 'count', 'disabled', 'theme'],
  attributeReflection: {
    message: 'message',
    count: 'count',
    disabled: 'disabled',
    theme: 'theme'
  },
  propertyTypes: {
    message: String,
    count: Number,
    disabled: Boolean,
    theme: String
  }
};

class PropertyReflectionError extends Error {
  constructor(message, code = 'PROPERTY_ERROR') {
    super(message);
    this.name = 'PropertyReflectionError';
    this.code = code;
  }
}

class PropertyObserverElement extends HTMLElement {
  static get observedAttributes() {
    return OBSERVER_CONFIG.observedAttributes;
  }

  static get observedProperties() {
    return Object.keys(OBSERVER_CONFIG.propertyTypes);
  }

  constructor() {
    super();
    this._properties = new Map();
    this._attributeHistory = [];
    this._propertyObservers = new Map();
    this._pendingUpdates = new Map();
    this._reflectionLock = false;
    this._observerId = `prop-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
    
    this._initProperties();
    this._initShadowDOM();
  }

  _initProperties() {
    const defaults = {
      message: 'Default message',
      count: 0,
      disabled: false,
      theme: 'light'
    };

    for (const [prop, type] of Object.entries(OBSERVER_CONFIG.propertyTypes)) {
      const defaultValue = defaults[prop];
      this._properties.set(prop, {
        value: defaultValue,
        type: type,
        previousValue: defaultValue,
        dirty: false
      });
    }
  }

  _initShadowDOM() {
    const shadow = this.attachShadow({ mode: 'open' });
    
    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
        padding: 16px;
        border: 2px solid var(--theme-border, #ccc);
        border-radius: 8px;
        background: var(--theme-bg, #fff);
        font-family: system-ui, sans-serif;
      }
      
      :host([disabled]) {
        opacity: 0.6;
        pointer-events: none;
      }

      .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 12px;
        padding-bottom: 12px;
        border-bottom: 1px solid #eee;
      }

      .title {
        font-size: 14px;
        font-weight: 600;
        color: #333;
      }

      .badge {
        font-size: 11px;
        padding: 2px 8px;
        background: #e0e0e0;
        border-radius: 12px;
        color: #666;
      }

      .content {
        margin-bottom: 12px;
      }

      .property-row {
        display: flex;
        justify-content: space-between;
        padding: 8px 0;
        border-bottom: 1px solid #f0f0f0;
      }

      .property-name {
        font-weight: 500;
        color: #555;
      }

      .property-value {
        font-family: monospace;
        color: #007acc;
      }

      .controls {
        display: flex;
        gap: 8px;
        margin-top: 12px;
      }

      button {
        flex: 1;
        padding: 8px 16px;
        border: 1px solid #ccc;
        border-radius: 4px;
        background: #fff;
        cursor: pointer;
        font-size: 13px;
        transition: all 0.2s;
      }

      button:hover {
        background: #f5f5f5;
        border-color: #999;
      }

      button:active {
        background: #eee;
      }

      .history {
        margin-top: 12px;
        padding: 12px;
        background: #f9f9f9;
        border-radius: 4px;
        max-height: 150px;
        overflow-y: auto;
      }

      .history-title {
        font-size: 12px;
        font-weight: 600;
        margin-bottom: 8px;
        color: #666;
      }

      .history-item {
        font-size: 11px;
        padding: 4px 0;
        border-bottom: 1px solid #eee;
        font-family: monospace;
      }

      .error-message {
        color: #d32f2f;
        font-size: 12px;
        margin-top: 8px;
        padding: 8px;
        background: #ffebee;
        border-radius: 4px;
      }

      @keyframes pulse {
        0%, 100% { opacity: 1; }
        50% { opacity: 0.7; }
      }

      .updating {
        animation: pulse 0.3s ease-in-out;
      }
    `;
    
    const container = document.createElement('div');
    container.innerHTML = `
      <div class="header">
        <span class="title">Property Observer</span>
        <span class="badge" id="badge">ID: ${this._observerId}</span>
      </div>
      <div class="content" id="content">
        <div class="property-row">
          <span class="property-name">message</span>
          <span class="property-value" id="msg-value">-</span>
        </div>
        <div class="property-row">
          <span class="property-name">count</span>
          <span class="property-value" id="count-value">-</span>
        </div>
        <div class="property-row">
          <span class="property-name">disabled</span>
          <span class="property-value" id="disabled-value">-</span>
        </div>
        <div class="property-row">
          <span class="property-name">theme</span>
          <span class="property-value" id="theme-value">-</span>
        </div>
      </div>
      <div class="controls">
        <button id="btn-increment">Increment</button>
        <button id="btn-toggle">Toggle</button>
        <button id="btn-reset">Reset</button>
      </div>
      <div class="history">
        <div class="history-title">Change History</div>
        <div id="history-list"></div>
      </div>
      <div id="error-container"></div>
    `;
    
    shadow.appendChild(style);
    shadow.appendChild(container);
  }

  connectedCallback() {
    this._setupObservers();
    this._bindEvents();
    this._render();
    this._logHistory('connected', 'Component connected to DOM');
  }

  disconnectedCallback() {
    this._cleanupObservers();
    this._logHistory('disconnected', 'Component disconnected from DOM');
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    
    const reflection = OBSERVER_CONFIG.attributeReflection[name];
    if (!reflection) return;

    this._logHistory('attribute', `${name}: ${oldValue} -> ${newValue}`);
    
    this._queueUpdate(reflection, newValue);
  }

  _setupObservers() {
    this._propertyObservers.set('message', new Set());
    this._propertyObservers.set('count', new Set());
    this._propertyObservers.set('disabled', new Set());
    this._propertyObservers.set('theme', new Set());
  }

  _cleanupObservers() {
    for (const observers of this._propertyObservers.values()) {
      observers.clear();
    }
  }

  _bindEvents() {
    const shadow = this.shadowRoot;
    
    const btnIncrement = shadow.getElementById('btn-increment');
    const btnToggle = shadow.getElementById('btn-toggle');
    const btnReset = shadow.getElementById('btn-reset');

    btnIncrement?.addEventListener('click', () => {
      const current = this.count;
      this.count = current + 1;
    });

    btnToggle?.addEventListener('click', () => {
      this.disabled = !this.disabled;
    });

    btnReset?.addEventListener('click', () => {
      this.message = 'Reset message';
      this.count = 0;
      this.disabled = false;
      this.theme = 'light';
    });
  }

  _queueUpdate(property, value) {
    if (this._reflectionLock) {
      this._pendingUpdates.set(property, value);
      return;
    }

    this._performPropertyUpdate(property, value);
  }

  _performPropertyUpdate(property, value) {
    this._reflectionLock = true;
    
    try {
      const propData = this._properties.get(property);
      if (!propData) return;

      const previousValue = propData.value;
      propData.previousValue = previousValue;
      
      const type = propData.type;
      let convertedValue;

      if (value === null || value === undefined) {
        convertedValue = type === Boolean ? false : type();
      } else if (type === Boolean) {
        convertedValue = value !== 'false' && value !== '0';
      } else if (type === Number) {
        convertedValue = Number(value) || 0;
      } else {
        convertedValue = value;
      }

      propData.value = convertedValue;
      propData.dirty = true;

      this._notifyPropertyObservers(property, convertedValue, previousValue);
      this._reflectToAttribute(property, convertedValue);
      this._render();
    } catch (error) {
      this._handleError(error, 'PROPERTY_UPDATE');
    } finally {
      this._reflectionLock = false;
      this._processPendingUpdates();
    }
  }

  _processPendingUpdates() {
    if (this._pendingUpdates.size === 0) return;

    const pending = new Map(this._pendingUpdates);
    this._pendingUpdates.clear();

    for (const [property, value] of pending) {
      this._performPropertyUpdate(property, value);
    }
  }

  _notifyPropertyObservers(property, newValue, oldValue) {
    const observers = this._propertyObservers.get(property);
    if (!observers) return;

    const change = { property, newValue, oldValue, timestamp: Date.now() };
    
    observers.forEach(callback => {
      try {
        callback(change);
      } catch (error) {
        console.error(`Observer error for ${property}:`, error);
      }
    });
  }

  _reflectToAttribute(property, value) {
    if (this._reflectionLock) return;

    const attrName = property;
    const type = OBSERVER_CONFIG.propertyTypes[property];
    
    let attrValue;
    if (type === Boolean) {
      attrValue = value ? '' : null;
    } else if (value === null || value === undefined) {
      attrValue = null;
    } else {
      attrValue = String(value);
    }

    if (attrValue === null) {
      this.removeAttribute(attrName);
    } else if (this.getAttribute(attrName) !== attrValue) {
      this.setAttribute(attrName, attrValue);
    }
  }

  observeProperty(property, callback) {
    if (!OBSERVER_CONFIG.propertyTypes.hasOwnProperty(property)) {
      throw new PropertyReflectionError(
        `Unknown property: ${property}`,
        'UNKNOWN_PROPERTY'
      );
    }

    const observers = this._propertyObservers.get(property);
    if (!observers) {
      throw new PropertyReflectionError(
        `Failed to get observers for: ${property}`,
        'OBSERVER_SETUP'
      );
    }

    observers.add(callback);

    return () => {
      observers.delete(callback);
    };
  }

  getPropertyDescriptor(property) {
    const type = OBSERVER_CONFIG.propertyTypes[property];
    if (!type) return undefined;

    return {
      enumerable: true,
      configurable: true,
      get: () => this._properties.get(property)?.value,
      set: (value) => this._queueUpdate(property, value)
    };
  }

  _render() {
    const shadow = this.shadowRoot;
    if (!shadow) return;

    const msgValue = shadow.getElementById('msg-value');
    const countValue = shadow.getElementById('count-value');
    const disabledValue = shadow.getElementById('disabled-value');
    const themeValue = shadow.getElementById('theme-value');

    const message = this.message;
    const count = this.count;
    const disabled = this.disabled;
    const theme = this.theme;

    if (msgValue) msgValue.textContent = JSON.stringify(message);
    if (countValue) countValue.textContent = String(count);
    if (disabledValue) disabledValue.textContent = String(disabled);
    if (themeValue) themeValue.textContent = JSON.stringify(theme);

    const content = shadow.getElementById('content');
    if (content) {
      content.classList.add('updating');
      setTimeout(() => content.classList.remove('updating'), 300);
    }
  }

  _logHistory(type, message) {
    const entry = { type, message, timestamp: Date.now() };
    this._attributeHistory.push(entry);
    
    if (this._attributeHistory.length > 50) {
      this._attributeHistory.shift();
    }

    this._updateHistoryDisplay();
  }

  _updateHistoryDisplay() {
    const shadow = this.shadowRoot;
    const historyList = shadow?.getElementById('history-list');
    if (!historyList) return;

    historyList.innerHTML = this._attributeHistory
      .slice(-10)
      .reverse()
      .map(entry => `
        <div class="history-item">
          [${entry.type}] ${entry.message}
        </div>
      `).join('');
  }

  _handleError(error, code) {
    console.error(`PropertyReflectionError [${code}]:`, error);
    
    const shadow = this.shadowRoot;
    const errorContainer = shadow?.getElementById('error-container');
    if (errorContainer) {
      errorContainer.innerHTML = `
        <div class="error-message">
          Error [${code}]: ${error.message}
        </div>
      `;
    }
  }

  get message() {
    return this._properties.get('message')?.value;
  }

  set message(value) {
    this._queueUpdate('message', value);
  }

  get count() {
    return this._properties.get('count')?.value;
  }

  set count(value) {
    this._queueUpdate('count', value);
  }

  get disabled() {
    return this._properties.get('disabled')?.value;
  }

  set disabled(value) {
    this._queueUpdate('disabled', value);
  }

  get theme() {
    return this._properties.get('theme')?.value;
  }

  set theme(value) {
    this._queueUpdate('theme', value);
  }
}

if (!customElements.get('property-observer-element')) {
  customElements.define('property-observer-element', PropertyObserverElement);
}

export { PropertyObserverElement, OBSERVER_CONFIG, PropertyReflectionError };