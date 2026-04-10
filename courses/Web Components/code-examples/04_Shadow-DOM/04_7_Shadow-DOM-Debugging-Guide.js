/**
 * Debug Tools Component - Shadow DOM debugging guide
 * @module shadow-dom/04_7_Shadow-DOM-Debugging-Guide
 * @version 1.0.0
 * @example <debug-panel></debug-panel>
 */

class DebugPanel extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._logs = [];
    this._maxLogs = 100;
    this._logLevel = 'all';
    this._isExpanded = false;
    this._filterTimer = null;
    this._debugMode = false;
    this._stateHistory = [];
    this._maxHistory = 20;
  }

  static get observedAttributes() {
    return ['log-level', 'expanded', 'debug', 'theme'];
  }

  connectedCallback() {
    this._initDebugTools();
    this._render();
    this._attachEventListeners();
    this._setupChromeDebugging();
  }

  disconnectedCallback() {
    this._cleanup();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._handleAttributeChange(name, newValue);
    }
  }

  _initDebugTools() {
    this._debugAPI = {
      log: this._log.bind(this),
      warn: this._warn.bind(this),
      error: this._error.bind(this),
      info: this._info.bind(this),
      group: this._groupStart.bind(this),
      groupEnd: this._groupEnd.bind(this),
      time: this._timeStart.bind(this),
      timeEnd: this._timeEnd.bind(this),
      assert: this._assert.bind(this),
      trace: this._trace.bind(this),
      clear: this._clear.bind(this),
      table: this._table.bind(this)
    };

    window.__DEBUG_PANEL__ = this._debugAPI;

    this._interceptConsole();
    this._interceptErrors();
  }

  _interceptConsole() {
    this._originalConsole = { ...console };
    const levels = ['log', 'warn', 'error', 'info', 'debug', 'table'];

    levels.forEach(level => {
      const original = console[level];
      console[level] = (...args) => {
        original.apply(console, args);
        if (this._shouldLog(level)) {
          this._addLog({
            type: level,
            message: this._formatMessage(args),
            timestamp: Date.now(),
            source: 'console'
          });
        }
      };
    });
  }

  _interceptErrors() {
    this._errorHandler = (event) => {
      this._addLog({
        type: 'error',
        message: event.message,
        timestamp: Date.now(),
        source: 'error',
        filename: event.filename,
        lineno: event.lineno
      });
    };

    window.addEventListener('error', this._errorHandler);
    window.addEventListener('unhandledrejection', (event) => {
      this._addLog({
        type: 'error',
        message: `Unhandled Promise: ${event.reason}`,
        timestamp: Date.now(),
        source: 'promise'
      });
    });
  }

  _setupChromeDebugging() {
    this._debugElement = this;
    this._debugElement.__debugComponent = this;

    if (typeof Node !== 'undefined') {
      const originalInsertBefore = Node.prototype.insertBefore;
      Node.prototype.insertBefore = function(newNode, referenceNode) {
        const result = originalInsertBefore.call(this, newNode, referenceNode);
        if (this === document.body && newNode.nodeType === Node.ELEMENT_NODE) {
          DebugPanel._notifyElementInserted(newNode);
        }
        return result;
      };
    }
  }

  static _notifyElementInserted(element) {
    const debugPanel = document.querySelector('debug-panel');
    if (debugPanel && debugPanel._debugElement) {
      debugPanel._addLog({
        type: 'info',
        message: `Element inserted: <${element.tagName.toLowerCase()}>`,
        timestamp: Date.now(),
        source: 'dom'
      });
    }
  }

  _render() {
    const style = this._getStyles();
    const template = this._getTemplate();
    this.shadowRoot.innerHTML = `${style}${template}`;
    this._cacheElements();
    this._applyTheme();
  }

  _getStyles() {
    return `
      <style>
        :host {
          display: block;
          --debug-bg: #1e1e1e;
          --debug-text: #d4d4d4;
          --debug-border: #3c3c3c;
          --debug-accent: #0e639c;
          --log-error: #f14c4c;
          --log-warn: #cca700;
          --log-info: #3794ff;
          --log-debug: #b5cea8;
          --log-log: #d4d4d4;
          font-family: 'Consolas', 'Monaco', monospace;
          font-size: 12px;
          position: fixed;
          bottom: 0;
          right: 0;
          z-index: 999999;
          max-width: 100%;
        }

        :host([hidden]) {
          display: none;
        }

        .debug-panel {
          background: var(--debug-bg);
          border: 1px solid var(--debug-border);
          border-radius: 4px 4px 0 0;
          overflow: hidden;
          transition: height 0.2s ease;
        }

        .debug-header {
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 8px 12px;
          background: #2d2d2d;
          cursor: pointer;
          user-select: none;
        }

        .debug-title {
          color: var(--debug-text);
          font-weight: 600;
          display: flex;
          align-items: center;
          gap: 8px;
        }

        .debug-badge {
          background: var(--debug-accent);
          color: white;
          padding: 2px 6px;
          border-radius: 3px;
          font-size: 10px;
        }

        .debug-controls {
          display: flex;
          gap: 8px;
          align-items: center;
        }

        .control-btn {
          background: transparent;
          border: 1px solid var(--debug-border);
          color: var(--debug-text);
          padding: 4px 8px;
          border-radius: 3px;
          cursor: pointer;
          font-size: 11px;
        }

        .control-btn:hover {
          background: var(--debug-border);
        }

        .debug-content {
          display: none;
          max-height: 300px;
          overflow-y: auto;
        }

        .debug-panel.expanded .debug-content {
          display: block;
        }

        .debug-panel.collapsed {
          max-height: 36px;
        }

        .log-filter {
          display: flex;
          padding: 8px;
          border-bottom: 1px solid var(--debug-border);
          gap: 8px;
        }

        .filter-input {
          flex: 1;
          background: #3c3c3c;
          border: 1px solid var(--debug-border);
          color: var(--debug-text);
          padding: 4px 8px;
          border-radius: 3px;
          font-family: inherit;
          font-size: 11px;
        }

        .filter-select {
          background: #3c3c3c;
          border: 1px solid var(--debug-border);
          color: var(--debug-text);
          padding: 4px;
          border-radius: 3px;
          font-family: inherit;
          font-size: 11px;
        }

        .log-list {
          list-style: none;
          margin: 0;
          padding: 0;
        }

        .log-item {
          padding: 4px 8px;
          border-bottom: 1px solid #2d2d2d;
          display: flex;
          gap: 8px;
          align-items: flex-start;
        }

        .log-item:hover {
          background: #2d2d2d;
        }

        .log-time {
          color: #888;
          flex-shrink: 0;
        }

        .log-type {
          flex-shrink: 0;
          width: 50px;
          text-transform: uppercase;
          font-size: 10px;
          font-weight: 600;
        }

        .log-type.error { color: var(--log-error); }
        .log-type.warn { color: var(--log-warn); }
        .log-type.info { color: var(--log-info); }
        .log-type.debug { color: var(--log-debug); }
        .log-type.log { color: var(--log-log); }

        .log-message {
          flex: 1;
          word-break: break-word;
          white-space: pre-wrap;
        }

        .log-source {
          color: #888;
          font-size: 10px;
        }

        .debug-actions {
          display: flex;
          padding: 8px;
          border-top: 1px solid var(--debug-border);
          gap: 8px;
        }

        .action-btn {
          flex: 1;
          background: #3c3c3c;
          border: none;
          color: var(--debug-text);
          padding: 6px;
          border-radius: 3px;
          cursor: pointer;
          font-family: inherit;
          font-size: 11px;
        }

        .action-btn:hover {
          background: #4c4c4c;
        }

        .action-btn.danger {
          background: #5c1e1e;
        }

        .action-btn.danger:hover {
          background: #7a2828;
        }

        .inspector {
          display: none;
          padding: 8px;
          border-bottom: 1px solid var(--debug-border);
          background: #252526;
        }

        .debug-panel.expanded .inspector {
          display: block;
        }

        .inspector-info {
          display: grid;
          grid-template-columns: 1fr 1fr;
          gap: 4px;
          color: var(--debug-text);
        }

        .inspector-label {
          color: #888;
        }
      </style>
    `;
  }

  _getTemplate() {
    return `
      <div class="debug-panel" id="panel">
        <div class="debug-header" id="header">
          <div class="debug-title">
            <span>Debug Panel</span>
            <span class="debug-badge" id="log-count">0</span>
          </div>
          <div class="debug-controls">
            <button class="control-btn" id="pin-btn">Pin</button>
            <button class="control-btn" id="clear-btn">Clear</button>
            <button class="control-btn" id="export-btn">Export</button>
          </div>
        </div>
        <div class="inspector">
          <div class="inspector-info">
            <span class="inspector-label">Shadow Root:</span>
            <span id="shadow-root-info">-</span>
            <span class="inspector-label">Custom Elements:</span>
            <span id="custom-elements-info">-</span>
            <span class="inspector-label">Memory:</span>
            <span id="memory-info">-</span>
            <span class="inspector-label">Logs:</span>
            <span id="total-logs">-</span>
          </div>
        </div>
        <div class="log-filter">
          <input type="text" class="filter-input" id="filter-input" placeholder="Filter logs...">
          <select class="filter-select" id="level-select">
            <option value="all">All</option>
            <option value="error">Error</option>
            <option value="warn">Warning</option>
            <option value="info">Info</option>
            <option value="debug">Debug</option>
            <option value="log">Log</option>
          </select>
        </div>
        <div class="debug-content">
          <ul class="log-list" id="log-list"></ul>
        </div>
        <div class="debug-actions">
          <button class="action-btn" id="inspect-dom">Inspect DOM</button>
          <button class="action-btn" id="snapshot">Snapshot</button>
          <button class="action-btn danger" id="clear-all">Clear All</button>
        </div>
      </div>
    `;
  }

  _cacheElements() {
    this._panel = this.shadowRoot.getElementById('panel');
    this._header = this.shadowRoot.getElementById('header');
    this._logList = this.shadowRoot.getElementById('log-list');
    this._logCount = this.shadowRoot.getElementById('log-count');
    this._filterInput = this.shadowRoot.getElementById('filter-input');
    this._levelSelect = this.shadowRoot.getElementById('level-select');
    this._shadowRootInfo = this.shadowRoot.getElementById('shadow-root-info');
    this._customElementsInfo = this.shadowRoot.getElementById('custom-elements-info');
    this._memoryInfo = this.shadowRoot.getElementById('memory-info');
    this._totalLogs = this.shadowRoot.getElementById('total-logs');
  }

  _attachEventListeners() {
    this._header.addEventListener('click', () => this._toggle());
    this._filterInput.addEventListener('input', this._debounce(() => this._filterLogs(), 300));
    this._levelSelect.addEventListener('change', (e) => this._setLogLevel(e.target.value));
    this.shadowRoot.getElementById('clear-btn').addEventListener('click', () => this.clear());
    this.shadowRoot.getElementById('clear-all').addEventListener('click', () => this.clearAll());
    this.shadowRoot.getElementById('export-btn').addEventListener('click', () => this._exportLogs());
    this.shadowRoot.getElementById('inspect-dom').addEventListener('click', () => this._inspectDOM());
    this.shadowRoot.getElementById('snapshot').addEventListener('click', () => this._takeSnapshot());
  }

  _handleAttributeChange(name, value) {
    switch (name) {
      case 'log-level':
        this._setLogLevel(value);
        break;
      case 'expanded':
        this._setExpanded(value === 'true');
        break;
      case 'debug':
        this._debugMode = value === 'true';
        break;
      case 'theme':
        this._applyTheme(value);
        break;
    }
  }

  _setLogLevel(level) {
    this._logLevel = level;
    this._renderLogs();
  }

  _setExpanded(expanded) {
    this._isExpanded = expanded;
    this._panel.classList.toggle('expanded', expanded);
    this._panel.classList.toggle('collapsed', !expanded);
  }

  _toggle() {
    this._setExpanded(!this._isExpanded);
  }

  _formatMessage(args) {
    return args.map(arg => {
      if (typeof arg === 'object') {
        try {
          return JSON.stringify(arg, null, 2);
        } catch (e) {
          return String(arg);
        }
      }
      return String(arg);
    }).join(' ');
  }

  _shouldLog(level) {
    const levels = ['error', 'warn', 'info', 'debug', 'log'];
    const levelIndex = levels.indexOf(level);
    const currentIndex = levels.indexOf(this._logLevel);
    return levelIndex >= currentIndex;
  }

  _addLog(log) {
    this._logs.push(log);
    this._saveState();

    if (this._logs.length > this._maxLogs) {
      this._logs.shift();
    }

    this._updateUI();
    this._renderLogs();
  }

  _log(...args) { this._addLog({ type: 'log', message: this._formatMessage(args), timestamp: Date.now(), source: 'api' }); }
  _warn(...args) { this._addLog({ type: 'warn', message: this._formatMessage(args), timestamp: Date.now(), source: 'api' }); }
  _error(...args) { this._addLog({ type: 'error', message: this._formatMessage(args), timestamp: Date.now(), source: 'api' }); }
  _info(...args) { this._addLog({ type: 'info', message: this._formatMessage(args), timestamp: Date.now(), source: 'api' }); }

  _groupStart(label) { this._addLog({ type: 'log', message: `▼ ${label}`, timestamp: Date.now(), source: 'api', group: true }); }
  _groupEnd() { this._addLog({ type: 'log', message: '▲', timestamp: Date.now(), source: 'api', groupEnd: true }); }

  _timeStart(label) { this._addLog({ type: 'info', message: `Timer started: ${label}`, timestamp: Date.now(), source: 'api', timer: label }); }
  _timeEnd(label) { this._addLog({ type: 'info', message: `Timer ended: ${label}`, timestamp: Date.now(), source: 'api', timerEnd: label }); }

  _assert(condition, message) {
    if (!condition) {
      this._addLog({ type: 'error', message: `Assertion failed: ${message}`, timestamp: Date.now(), source: 'api' });
    }
  }

  _trace() {
    const stack = new Error().stack;
    this._addLog({ type: 'debug', message: stack, timestamp: Date.now(), source: 'api' });
  }

  _table(data) {
    this._addLog({ type: 'debug', message: this._formatMessage([data]), timestamp: Date.now(), source: 'api', table: true });
  }

  clear() {
    this._logs = [];
    this._renderLogs();
    this._updateUI();
  }

  clearAll() {
    this._logs = [];
    this._stateHistory = [];
    this._renderLogs();
    this._updateUI();
  }

  _filterLogs() {
    const filter = this._filterInput.value.toLowerCase();
    this._renderLogs(filter);
  }

  _renderLogs(filter = '') {
    const filtered = this._logs.filter(log => {
      const matchesLevel = this._shouldLog(log.type);
      const matchesFilter = !filter || log.message.toLowerCase().includes(filter);
      return matchesLevel && matchesFilter;
    });

    this._logList.innerHTML = filtered.map(log => `
      <li class="log-item">
        <span class="log-time">${this._formatTime(log.timestamp)}</span>
        <span class="log-type ${log.type}">${log.type}</span>
        <span class="log-message">${this._escapeHtml(log.message)}</span>
        ${log.source !== 'api' ? `<span class="log-source">[${log.source}]</span>` : ''}
      </li>
    `).join('');

    this._logList.scrollTop = this._logList.scrollHeight;
  }

  _formatTime(timestamp) {
    const date = new Date(timestamp);
    return date.toLocaleTimeString('en-US', { hour12: false }) + '.' + String(date.getMilliseconds()).padStart(3, '0');
  }

  _escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }

  _updateUI() {
    this._logCount.textContent = this._logs.length;
    this._updateInspector();
  }

  _updateInspector() {
    const customElements = customElements ? customElements.size : 0;
    this._shadowRootInfo.textContent = this.shadowRoot ? 'Present' : 'None';
    this._customElementsInfo.textContent = customElements;
    this._totalLogs.textContent = this._logs.length;

    if (performance.memory) {
      const used = (performance.memory.usedJSHeapSize / 1048576).toFixed(1);
      this._memoryInfo.textContent = `${used} MB`;
    }
  }

  _applyTheme(theme) {
    const style = this.style || {};
    if (theme === 'light') {
      style.setProperty('--debug-bg', '#f5f5f5');
      style.setProperty('--debug-text', '#333');
      style.setProperty('--debug-border', '#ccc');
    }
  }

  _exportLogs() {
    const data = {
      exportTime: new Date().toISOString(),
      logs: this._logs,
      metadata: {
        total: this._logs.length,
        debugMode: this._debugMode
      }
    };

    const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `debug-logs-${Date.now()}.json`;
    a.click();
    URL.revokeObjectURL(url);
  }

  _inspectDOM() {
    console.log('=== Shadow DOM Debug Info ===');
    console.log('Shadow Root:', this.shadowRoot);
    console.log('Host Element:', this);
    console.log('Logs:', this._logs);
    console.log('Performance:', this._getPerformanceInfo());
    console.log('============================');
  }

  _takeSnapshot() {
    this._saveState();
    this._addLog({
      type: 'info',
      message: `Snapshot taken: ${this._logs.length} logs`,
      timestamp: Date.now(),
      source: 'snapshot'
    });
  }

  _saveState() {
    this._stateHistory.push([...this._logs]);
    if (this._stateHistory.length > this._maxHistory) {
      this._stateHistory.shift();
    }
  }

  _getPerformanceInfo() {
    return {
      logs: this._logs.length,
      historySize: this._stateHistory.length,
      memory: performance.memory ? {
        used: (performance.memory.usedJSHeapSize / 1048576).toFixed(1) + ' MB',
        total: (performance.memory.totalJSHeapSize / 1048576).toFixed(1) + ' MB'
      } : 'N/A'
    };
  }

  debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
      const later = () => {
        clearTimeout(timeout);
        func(...args);
      };
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
    };
  }

  _cleanup() {
    if (this._originalConsole) {
      Object.assign(console, this._originalConsole);
    }
    window.removeEventListener('error', this._errorHandler);
    delete window.__DEBUG_PANEL__;
  }
}

customElements.define('debug-panel', DebugPanel);

export { DebugPanel };