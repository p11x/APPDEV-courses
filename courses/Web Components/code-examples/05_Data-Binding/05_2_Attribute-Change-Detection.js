/**
 * Attribute Change Detection
 * Implements attribute monitoring, mutation observers, and change tracking
 * @module data-binding/05_2_Attribute-Change-Detection
 * @version 1.0.0
 * @example <attribute-detector-element data-config='{"key":"value"}'></attribute-detector-element>
 */

const ATTRIBUTE_CONFIG = {
  monitoredAttributes: ['data-config', 'data-state', 'data-mode', 'data-source', 'data-options'],
  debounceDelay: 150,
  maxHistorySize: 100,
  enableMutationObserver: true,
  attributeParser: {
    'data-config': JSON.parse,
    'data-state': JSON.parse,
    'data-options': JSON.parse,
    'data-mode': (val) => val,
    'data-source': (val) => val
  },
  attributeSerializer: {
    'data-config': JSON.stringify,
    'data-state': JSON.stringify,
    'data-options': JSON.stringify,
    'data-mode': (val) => val,
    'data-source': (val) => val
  }
};

class AttributeChangeError extends Error {
  constructor(message, code = 'ATTRIBUTE_ERROR') {
    super(message);
    this.name = 'AttributeChangeError';
    this.code = code;
  }
}

class AttributeDetectorElement extends HTMLElement {
  static get observedAttributes() {
    return ATTRIBUTE_CONFIG.monitoredAttributes;
  }

  constructor() {
    super();
    this._attributes = new Map();
    this._changeHistory = [];
    this._mutationObserver = null;
    this._attributeObservers = new Map();
    this._debounceTimers = new Map();
    this._pendingChanges = new Set();
    this._lockChanges = false;
    
    this._initializeAttributes();
    this._attachShadowDOM();
  }

  _initializeAttributes() {
    const defaults = {
      'data-config': { theme: 'light', compact: false },
      'data-state': { active: true, loading: false },
      'data-mode': 'view',
      'data-source': '',
      'data-options': { autoSave: true, retry: 3 }
    };

    for (const attr of ATTRIBUTE_CONFIG.monitoredAttributes) {
      const defaultValue = defaults[attr] || null;
      this._attributes.set(attr, {
        value: defaultValue,
        rawValue: defaultValue !== null ? JSON.stringify(defaultValue) : null,
        previousValue: defaultValue,
        parse: ATTRIBUTE_CONFIG.attributeParser[attr] || JSON.parse,
        serialize: ATTRIBUTE_CONFIG.attributeSerializer[attr] || JSON.stringify,
        version: 0
      });
    }
  }

  _attachShadowDOM() {
    const shadow = this.attachShadow({ mode: 'open' });
    
    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
        padding: 20px;
        border: 2px solid #ddd;
        border-radius: 8px;
        background: #fff;
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
      }

      .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid #f0f0f0;
      }

      .title {
        font-size: 16px;
        font-weight: 600;
        color: #222;
      }

      .status {
        display: flex;
        align-items: center;
        gap: 8px;
      }

      .status-dot {
        width: 8px;
        height: 8px;
        border-radius: 50%;
        background: #4caf50;
      }

      .status-dot.inactive {
        background: #f44336;
      }

      .status-text {
        font-size: 12px;
        color: #666;
      }

      .attribute-grid {
        display: grid;
        gap: 12px;
        margin-bottom: 16px;
      }

      .attribute-card {
        padding: 12px;
        background: #fafafa;
        border-radius: 6px;
        border: 1px solid #eee;
      }

      .attribute-name {
        font-size: 11px;
        text-transform: uppercase;
        letter-spacing: 0.5px;
        color: #888;
        margin-bottom: 4px;
      }

      .attribute-value {
        font-family: 'Fira Code', monospace;
        font-size: 13px;
        color: #d63384;
        word-break: break-all;
      }

      .controls {
        display: flex;
        gap: 8px;
        margin-bottom: 16px;
      }

      .control-button {
        flex: 1;
        padding: 10px 16px;
        border: 1px solid #ddd;
        border-radius: 6px;
        background: #fff;
        font-size: 13px;
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s;
      }

      .control-button:hover {
        background: #f5f5f5;
        border-color: #ccc;
      }

      .control-button:active {
        background: #eee;
        transform: scale(0.98);
      }

      .history-panel {
        background: #f5f5f5;
        border-radius: 6px;
        padding: 12px;
        max-height: 200px;
        overflow-y: auto;
      }

      .history-title {
        font-size: 12px;
        font-weight: 600;
        color: #666;
        margin-bottom: 8px;
      }

      .history-entry {
        font-size: 11px;
        padding: 4px 0;
        border-bottom: 1px solid #e0e0e0;
        font-family: monospace;
        display: flex;
        justify-content: space-between;
      }

      .history-entry .attr {
        color: #007ad4;
      }

      .history-entry .change {
        color: #333;
      }

      .history-entry .time {
        color: #999;
      }

      .detection-info {
        display: flex;
        gap: 16px;
        margin-top: 12px;
        padding: 12px;
        background: #e8f5e9;
        border-radius: 6px;
      }

      .info-item {
        display: flex;
        flex-direction: column;
        gap: 2px;
      }

      .info-label {
        font-size: 10px;
        color: #666;
        text-transform: uppercase;
      }

      .info-value {
        font-size: 14px;
        font-weight: 600;
        color: #2e7d32;
      }

      @keyframes highlight {
        0% { background-color: rgba(0, 122, 204, 0.2); }
        100% { background-color: transparent; }
      }

      .updated {
        animation: highlight 0.5s ease-out;
      }
    `;

    const container = document.createElement('div');
    container.innerHTML = `
      <div class="header">
        <span class="title">Attribute Change Detector</span>
        <div class="status">
          <div class="status-dot" id="status-dot"></div>
          <span class="status-text" id="status-text">Observing</span>
        </div>
      </div>
      
      <div class="attribute-grid" id="attribute-grid">
        ${ATTRIBUTE_CONFIG.monitoredAttributes.map(attr => `
          <div class="attribute-card" data-attr="${attr}">
            <div class="attribute-name">${attr}</div>
            <div class="attribute-value" id="value-${attr.replace('.', '-')}">-</div>
          </div>
        `).join('')}
      </div>
      
      <div class="controls">
        <button class="control-button" id="btn-update-config">Update Config</button>
        <button class="control-button" id="btn-toggle-state">Toggle State</button>
        <button class="control-button" id="btn-cycle-mode">Cycle Mode</button>
      </div>
      
      <div class="history-panel">
        <div class="history-title">Change Detection Log</div>
        <div id="history-log"></div>
      </div>
      
      <div class="detection-info">
        <div class="info-item">
          <span class="info-label">Total Changes</span>
          <span class="info-value" id="total-changes">0</span>
        </div>
        <div class="info-item">
          <span class="info-label">Observers</span>
          <span class="info-value" id="observer-count">0</span>
        </div>
        <div class="info-item">
          <span class="info-label">Last Detect</span>
          <span class="info-value" id="last-detect">-</span>
        </div>
      </div>
    `;
    
    shadow.appendChild(style);
    shadow.appendChild(container);
  }

  connectedCallback() {
    this._setupMutationObserver();
    this._bindControlEvents();
    this._registerDefaultObservers();
    this._render();
    this._recordChange('system', 'initialized', 'Component connected');
  }

  disconnectedCallback() {
    this._teardownMutationObserver();
    this._recordChange('system', 'shutdown', 'Component disconnected');
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    if (!ATTRIBUTE_CONFIG.monitoredAttributes.includes(name)) return;

    this._debouncedHandleChange(name, oldValue, newValue);
  }

  _setupMutationObserver() {
    if (!ATTRIBUTE_CONFIG.enableMutationObserver) return;
    if (!this._mutationObserver) {
      this._mutationObserver = new MutationObserver((mutations) => {
        this._handleMutations(mutations);
      });
    }

    this._mutationObserver.observe(this, {
      attributes: true,
      attributeOldValue: true,
      attributeFilter: ATTRIBUTE_CONFIG.monitoredAttributes
    });
  }

  _teardownMutationObserver() {
    if (this._mutationObserver) {
      this._mutationObserver.disconnect();
      this._mutationObserver = null;
    }
  }

  _handleMutations(mutations) {
    for (const mutation of mutations) {
      if (mutation.type === 'attributes' && mutation.attributeName) {
        const oldValue = mutation.oldValue;
        const newValue = this.getAttribute(mutation.attributeName);
        
        this._recordChange(
          mutation.attributeName,
          oldValue,
          newValue,
          'mutation'
        );
      }
    }
  }

  _debouncedHandleChange(name, oldValue, newValue) {
    const existingTimer = this._debounceTimers.get(name);
    if (existingTimer) {
      clearTimeout(existingTimer);
    }

    const timer = setTimeout(() => {
      this._processAttributeChange(name, oldValue, newValue);
      this._debounceTimers.delete(name);
    }, ATTRIBUTE_CONFIG.debounceDelay);

    this._debounceTimers.set(name, timer);
  }

  _processAttributeChange(name, oldValue, newValue) {
    if (this._lockChanges) {
      this._pendingChanges.add(JSON.stringify({ name, oldValue, newValue }));
      return;
    }

    const attrData = this._attributes.get(name);
    if (!attrData) return;

    try {
      const parsedOld = oldValue ? attrData.parse(oldValue) : attrData.previousValue;
      const parsedNew = newValue ? attrData.parse(newValue) : null;

      attrData.previousValue = parsedOld;
      attrData.value = parsedNew;
      attrData.rawValue = newValue;
      attrData.version++;

      this._recordChange(name, parsedOld, parsedNew, 'attribute');
      this._notifyObservers(name, parsedNew, parsedOld);
      this._render();
    } catch (error) {
      this._handleError(error, 'PARSE_ERROR');
    }
  }

  _notifyObservers(attributeName, newValue, oldValue) {
    const observers = this._attributeObservers.get(attributeName);
    if (!observers) return;

    const changeEvent = {
      attribute: attributeName,
      newValue,
      oldValue,
      version: this._attributes.get(attributeName)?.version || 0,
      timestamp: Date.now()
    };

    observers.forEach(callback => {
      try {
        callback(changeEvent);
      } catch (error) {
        console.error(`Observer error for ${attributeName}:`, error);
      }
    });
  }

  _recordChange(attribute, oldValue, newValue, source = 'direct') {
    const entry = {
      id: `${Date.now()}-${Math.random().toString(36).substr(2, 6)}`,
      attribute,
      oldValue: this._truncateValue(oldValue),
      newValue: this._truncateValue(newValue),
      source,
      timestamp: new Date().toISOString()
    };

    this._changeHistory.push(entry);

    if (this._changeHistory.length > ATTRIBUTE_CONFIG.maxHistorySize) {
      this._changeHistory.shift();
    }

    this._updateHistoryDisplay();
  }

  _truncateValue(value, maxLength = 50) {
    if (value === null || value === undefined) return 'null';
    const str = typeof value === 'string' ? value : JSON.stringify(value);
    if (str.length > maxLength) {
      return str.substring(0, maxLength) + '...';
    }
    return str;
  }

  _bindControlEvents() {
    const shadow = this.shadowRoot;
    
    const btnUpdateConfig = shadow.getElementById('btn-update-config');
    const btnToggleState = shadow.getElementById('btn-toggle-state');
    const btnCycleMode = shadow.getElementById('btn-cycle-mode');

    btnUpdateConfig?.addEventListener('click', () => {
      const current = this.getAttribute('data-config');
      const config = current ? JSON.parse(current) : {};
      config.theme = config.theme === 'light' ? 'dark' : 'light';
      config.compact = !config.compact;
      this.setAttribute('data-config', JSON.stringify(config));
    });

    btnToggleState?.addEventListener('click', () => {
      const current = this.getAttribute('data-state');
      const state = current ? JSON.parse(current) : { active: true, loading: false };
      state.active = !state.active;
      state.loading = !state.loading;
      this.setAttribute('data-state', JSON.stringify(state));
    });

    btnCycleMode?.addEventListener('click', () => {
      const modes = ['view', 'edit', 'preview', 'debug'];
      const current = this.getAttribute('data-mode') || 'view';
      const currentIndex = modes.indexOf(current);
      const nextIndex = (currentIndex + 1) % modes.length;
      this.setAttribute('data-mode', modes[nextIndex]);
    });
  }

  _registerDefaultObservers() {
    this.observeAttribute('data-config', (change) => {
      console.log('Config changed:', change);
    });

    this.observeAttribute('data-state', (change) => {
      console.log('State changed:', change);
    });

    this.observeAttribute('data-mode', (change) => {
      console.log('Mode changed:', change);
    });
  }

  observeAttribute(attributeName, callback) {
    if (!ATTRIBUTE_CONFIG.monitoredAttributes.includes(attributeName)) {
      throw new AttributeChangeError(
        `Attribute not monitored: ${attributeName}`,
        'INVALID_ATTRIBUTE'
      );
    }

    if (!this._attributeObservers.has(attributeName)) {
      this._attributeObservers.set(attributeName, new Set());
    }

    this._attributeObservers.get(attributeName).add(callback);
    this._updateObserverCount();

    return () => {
      const observers = this._attributeObservers.get(attributeName);
      if (observers) {
        observers.delete(callback);
        this._updateObserverCount();
      }
    };
  }

  getAttributeData(attributeName) {
    const data = this._attributes.get(attributeName);
    if (!data) return null;

    return {
      value: data.value,
      previousValue: data.previousValue,
      rawValue: data.rawValue,
      version: data.version
    };
  }

  _render() {
    const shadow = this.shadowRoot;
    if (!shadow) return;

    for (const attr of ATTRIBUTE_CONFIG.monitoredAttributes) {
      const valueEl = shadow.getElementById(`value-${attr.replace('.', '-')}`);
      const attrData = this._attributes.get(attr);
      
      if (valueEl && attrData) {
        valueEl.textContent = attrData.rawValue || '(not set)';
        
        const card = valueEl.closest('.attribute-card');
        if (card) {
          card.classList.remove('updated');
          void card.offsetWidth;
          card.classList.add('updated');
        }
      }
    }

    const totalChanges = shadow.getElementById('total-changes');
    const lastDetect = shadow.getElementById('last-detect');
    
    if (totalChanges) totalChanges.textContent = String(this._changeHistory.length);
    if (lastDetect && this._changeHistory.length > 0) {
      const last = this._changeHistory[this._changeHistory.length - 1];
      lastDetect.textContent = new Date(last.timestamp).toLocaleTimeString();
    }
  }

  _updateHistoryDisplay() {
    const shadow = this.shadowRoot;
    const historyLog = shadow?.getElementById('history-log');
    if (!historyLog) return;

    historyLog.innerHTML = this._changeHistory
      .slice(-15)
      .reverse()
      .map(entry => `
        <div class="history-entry">
          <span>
            <span class="attr">${entry.attribute}</span>
            <span class="change">: ${entry.oldValue} → ${entry.newValue}</span>
          </span>
          <span class="time">${new Date(entry.timestamp).toLocaleTimeString()}</span>
        </div>
      `).join('');
  }

  _updateObserverCount() {
    const shadow = this.shadowRoot;
    const observerCount = shadow?.getElementById('observer-count');
    
    if (observerCount) {
      let total = 0;
      this._attributeObservers.forEach(observers => {
        total += observers.size;
      });
      observerCount.textContent = String(total);
    }
  }

  _handleError(error, code) {
    console.error(`AttributeChangeError [${code}]:`, error);
  }

  get dataConfig() {
    const data = this._attributes.get('data-config');
    return data?.value;
  }

  set dataConfig(value) {
    this.setAttribute('data-config', JSON.stringify(value));
  }

  get dataState() {
    const data = this._attributes.get('data-state');
    return data?.value;
  }

  set dataState(value) {
    this.setAttribute('data-state', JSON.stringify(value));
  }

  get dataMode() {
    return this.getAttribute('data-mode');
  }

  set dataMode(value) {
    this.setAttribute('data-mode', value);
  }

  get dataSource() {
    return this.getAttribute('data-source');
  }

  set dataSource(value) {
    this.setAttribute('data-source', value);
  }

  get dataOptions() {
    const data = this._attributes.get('data-options');
    return data?.value;
  }

  set dataOptions(value) {
    this.setAttribute('data-options', JSON.stringify(value));
  }
}

if (!customElements.get('attribute-detector-element')) {
  customElements.define('attribute-detector-element', AttributeDetectorElement);
}

export { AttributeDetectorElement, ATTRIBUTE_CONFIG, AttributeChangeError };