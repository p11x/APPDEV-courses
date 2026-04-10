/**
 * Debugging Tools and Techniques - Browser devtools, logging, and debugging workflows
 * @module tooling/12_8_Debugging-Tools-and-Techniques
 * @version 1.0.0
 */

class DebuggingToolsAndTechniques extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._breakpoints = [];
    this._callStack = [];
    this._logs = [];
    this._config = {
      logLevel: 'info',
      console: true,
      network: true,
      elements: true,
    };
  }

  static get observedAttributes() {
    return ['log-level', 'console', 'network'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updateConfig(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
    this._initDebugger();
  }

  _updateConfig(name, value) {
    if (name === 'log-level') {
      this._config.logLevel = value;
    } else if (name === 'console') {
      this._config.console = value === 'true';
    } else if (name === 'network') {
      this._config.network = value === 'true';
    }
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        }
        .container {
          padding: 16px;
          background: #1e1e1e;
          border-radius: 8px;
          color: #d4d4d4;
        }
        .header {
          display: flex;
          align-items: center;
          justify-content: space-between;
          margin-bottom: 16px;
        }
        .title {
          font-size: 18px;
          font-weight: 600;
          color: #fff;
        }
        .badge {
          background: #569cd6;
          color: #fff;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
        }
        .toolbar {
          display: flex;
          gap: 8px;
          margin-bottom: 16px;
        }
        .toolbar-btn {
          flex: 1;
          padding: 10px;
          background: #2d2d2d;
          border: 2px solid #3c3c3c;
          border-radius: 4px;
          color: #d4d4d4;
          cursor: pointer;
          text-align: center;
          font-size: 13px;
          transition: all 0.2s;
        }
        .toolbar-btn.active {
          border-color: #569cd6;
          background: #3a3a3a;
        }
        .toolbar-btn:hover {
          border-color: #569cd6;
        }
        .toolbar-icon {
          font-size: 18px;
          margin-bottom: 4px;
        }
        .section {
          background: #2d2d2d;
          border-radius: 8px;
          padding: 16px;
          margin-bottom: 16px;
        }
        .section-title {
          font-size: 14px;
          font-weight: 500;
          color: #9cdcfe;
          margin-bottom: 12px;
        }
        .log-panel {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 200px;
          overflow-y: auto;
          font-family: 'Consolas', monospace;
          font-size: 12px;
        }
        .log-entry {
          display: flex;
          align-items: flex-start;
          padding: 6px 12px;
          border-bottom: 1px solid #3c3c3c;
        }
        .log-entry:last-child {
          border-bottom: none;
        }
        .log-time {
          color: #808080;
          margin-right: 12px;
          min-width: 70px;
        }
        .log-type {
          width: 60px;
          margin-right: 12px;
          font-size: 11px;
          text-transform: uppercase;
        }
        .log-type.log {
          color: #d4d4d4;
        }
        .log-type.info {
          color: #569cd6;
        }
        .log-type.warn {
          color: #cca700;
        }
        .log-type.error {
          color: #f44747;
        }
        .log-message {
          flex: 1;
          word-break: break-word;
        }
        .breakpoint-list {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 150px;
          overflow-y: auto;
        }
        .breakpoint-item {
          display: flex;
          align-items: center;
          padding: 8px 12px;
          border-bottom: 1px solid #3c3c3c;
          font-size: 13px;
        }
        .breakpoint-item:last-child {
          border-bottom: none;
        }
        .breakpoint-icon {
          width: 16px;
          height: 16px;
          background: #f44747;
          border-radius: 2px;
          margin-right: 12px;
        }
        .breakpoint-file {
          flex: 1;
          color: #9cdcfe;
        }
        .breakpoint-line {
          color: #808080;
        }
        .call-stack {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 150px;
          overflow-y: auto;
        }
        .stack-item {
          display: flex;
          align-items: center;
          padding: 8px 12px;
          border-bottom: 1px solid #3c3c3c;
          font-size: 13px;
        }
        .stack-item:last-child {
          border-bottom: none;
        }
        .stack-index {
          width: 24px;
          color: #808080;
          font-size: 12px;
        }
        .stack-function {
          flex: 1;
          color: #9cdcfe;
        }
        .stack-file {
          color: #808080;
        }
        .network-log {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 150px;
          overflow-y: auto;
        }
        .network-item {
          display: flex;
          align-items: center;
          padding: 8px 12px;
          border-bottom: 1px solid #3c3c3c;
          font-size: 13px;
        }
        .network-item:last-child {
          border-bottom: none;
        }
        .network-method {
          font-weight: 600;
          width: 50px;
        }
        .method-get {
          color: #4ec9b0;
        }
        .method-post {
          color: #ce9178;
        }
        .network-url {
          flex: 1;
          color: #d4d4d4;
        }
        .network-status {
          width: 40px;
        }
        .status-200 {
          color: #4ec9b0;
        }
        .status-400 {
          color: #cca700;
        }
        .status-500 {
          color: #f44747;
        }
        .network-duration {
          color: #808080;
          width: 50px;
          text-align: right;
        }
        .variable-inspector {
          background: #1e1e1e;
          border-radius: 4px;
          padding: 12px;
        }
        .variable-item {
          display: flex;
          padding: 6px 0;
          font-size: 13px;
        }
        .variable-name {
          color: #9cdcfe;
          min-width: 120px;
        }
        .variable-value {
          color: #ce9178;
        }
        .variable-type {
          color: #569cd6;
        }
        .console-output {
          background: #1e1e1e;
          border-radius: 4px;
          padding: 12px;
          font-family: 'Consolas', monospace;
          font-size: 12px;
          min-height: 80px;
        }
        .console-line {
          padding: 2px 0;
        }
        .actions {
          display: flex;
          gap: 8px;
        }
        button {
          background: #0e639c;
          color: #fff;
          border: none;
          padding: 10px 20px;
          border-radius: 4px;
          cursor: pointer;
          font-size: 14px;
          transition: background 0.2s;
        }
        button:hover {
          background: #1177bb;
        }
        button:disabled {
          background: #404040;
          cursor: not-allowed;
        }
        .btn-primary {
          background: #569cd6;
        }
        .btn-primary:hover {
          background: #67abf5;
        }
        .btn-success {
          background: #4ec9b0;
          color: #1e1e1e;
        }
        .config-section {
          background: #2d2d2d;
          border-radius: 8px;
          padding: 16px;
          margin-bottom: 16px;
        }
        .config-grid {
          display: grid;
          grid-template-columns: repeat(2, 1fr);
          gap: 12px;
        }
        .config-item {
          display: flex;
          flex-direction: column;
          gap: 4px;
        }
        .config-label {
          font-size: 12px;
          color: #808080;
        }
        .select {
          background: #3c3c3c;
          color: #d4d4d4;
          border: none;
          padding: 8px 12px;
          border-radius: 4px;
          cursor: pointer;
        }
        .toggle {
          position: relative;
          width: 48px;
          height: 24px;
          background: #3c3c3c;
          border-radius: 12px;
          cursor: pointer;
        }
        .toggle.active {
          background: #4ec9b0;
        }
        .toggle::after {
          content: '';
          position: absolute;
          top: 2px;
          left: 2px;
          width: 20px;
          height: 20px;
          background: #fff;
          border-radius: 50%;
          transition: transform 0.2s;
        }
        .toggle.active::after {
          transform: translateX(24px);
        }
      </style>
      <div class="container">
        <div class="header">
          <span class="title">Debugging Tools</span>
          <span class="badge">v1.0.0</span>
        </div>

        <div class="toolbar">
          <div class="toolbar-btn active" data-panel="console">
            <div class="toolbar-icon">📋</div>
            <div>Console</div>
          </div>
          <div class="toolbar-btn" data-panel="sources">
            <div class="toolbar-icon">📝</div>
            <div>Sources</div>
          </div>
          <div class="toolbar-btn" data-panel="network">
            <div class="toolbar-icon">🌐</div>
            <div>Network</div>
          </div>
          <div class="toolbar-btn" data-panel="elements">
            <div class="toolbar-icon">🔍</div>
            <div>Elements</div>
          </div>
        </div>

        <div class="config-section">
          <div class="section-title">Debugger Settings</div>
          <div class="config-grid">
            <div class="config-item">
              <span class="config-label">Log Level</span>
              <select class="select" id="log-level-select">
                <option value="debug">Debug</option>
                <option value="info" selected>Info</option>
                <option value="warn">Warn</option>
                <option value="error">Error</option>
              </select>
            </div>
            <div class="config-item">
              <span class="config-label">Console Logging</span>
              <div class="toggle active" id="console-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Network Monitoring</span>
              <div class="toggle active" id="network-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Auto-pause on Error</span>
              <div class="toggle" id="pause-toggle"></div>
            </div>
          </div>
        </div>

        <div class="section" id="console-panel">
          <div class="section-title">Console Output</div>
          <div class="console-output" id="console-output">
            <div class="console-line" style="color: #569cd6;">> Web Components debugger initialized</div>
          </div>
        </div>

        <div class="section" id="sources-panel" style="display: none;">
          <div class="section-title">Breakpoints</div>
          <div class="breakpoint-list" id="breakpoint-list">
            <div class="breakpoint-item">
              <div class="breakpoint-icon"></div>
              <span class="breakpoint-file">component.ts</span>
              <span class="breakpoint-line">Line 42</span>
            </div>
            <div class="breakpoint-item">
              <div class="breakpoint-icon"></div>
              <span class="breakpoint-file">renderer.ts</span>
              <span class="breakpoint-line">Line 78</span>
            </div>
          </div>

          <div class="section-title" style="margin-top: 16px;">Call Stack</div>
          <div class="call-stack" id="call-stack">
            <div class="stack-item">
              <span class="stack-index">#0</span>
              <span class="stack-function">render</span>
              <span class="stack-file">component.ts:42</span>
            </div>
            <div class="stack-item">
              <span class="stack-index">#1</span>
              <span class="stack-function">updateComplete</span>
              <span class="stack-file">component.ts:115</span>
            </div>
            <div class="stack-item">
              <span class="stack-index">#2</span>
              <span class="stack-function">firstUpdated</span>
              <span class="stack-file">renderer.ts:78</span>
            </div>
          </div>

          <div class="section-title" style="margin-top: 16px;">Scope Variables</div>
          <div class="variable-inspector" id="variable-inspector">
            <div class="variable-item">
              <span class="variable-name">this.shadowRoot</span>
              <span class="variable-value">ShadowRoot</span>
              <span class="variable-type">object</span>
            </div>
            <div class="variable-item">
              <span class="variable-name">this._data</span>
              <span class="variable-value">[{...}]</span>
              <span class="variable-type">array[5]</span>
            </div>
            <div class="variable-item">
              <span class="variable-name">this._config</span>
              <span class="variable-value">{mode: 'open'}</span>
              <span class="variable-type">object</span>
            </div>
            <div class="variable-item">
              <span class="variable-name">this.isConnected</span>
              <span class="variable-value">true</span>
              <span class="variable-type">boolean</span>
            </div>
          </div>
        </div>

        <div class="section" id="network-panel" style="display: none;">
          <div class="section-title">Network Requests</div>
          <div class="network-log" id="network-log">
            <div class="network-item">
              <span class="network-method method-get">GET</span>
              <span class="network-url">/api/components</span>
              <span class="network-status status-200">200</span>
              <span class="network-duration">45ms</span>
            </div>
            <div class="network-item">
              <span class="network-method method-post">POST</span>
              <span class="network-url">/api/render</span>
              <span class="network-status status-200">200</span>
              <span class="network-duration">128ms</span>
            </div>
            <div class="network-item">
              <span class="network-method method-get">GET</span>
              <span class="network-url">/api/styles</span>
              <span class="network-status status-200">200</span>
              <span class="network-duration">12ms</span>
            </div>
          </div>
        </div>

        <div class="section" id="elements-panel" style="display: none;">
          <div class="section-title">DOM Inspector</div>
          <div class="variable-inspector">
            <div class="variable-item">
              <span class="variable-name">tagName</span>
              <span class="variable-value">MY-COMPONENT</span>
            </div>
            <div class="variable-item">
              <span class="variable-name">classList</span>
              <span class="variable-value">["container", "active"]</span>
            </div>
            <div class="variable-item">
              <span class="variable-name">attributes</span>
              <span class="variable-value">[mode="open"]</span>
            </div>
            <div class="variable-item">
              <span class="variable-name">childElementCount</span>
              <span class="variable-value">3</span>
            </div>
          </div>
        </div>

        <div class="actions">
          <button id="step-btn">Step Over</button>
          <button id="step-in-btn">Step Into</button>
          <button id="step-out-btn">Step Out</button>
          <button id="continue-btn" class="btn-success">Continue</button>
          <button id="add-breakpoint-btn" class="btn-primary">Add Breakpoint</button>
        </div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const stepBtn = this.shadowRoot.getElementById('step-btn');
    const stepInBtn = this.shadowRoot.getElementById('step-in-btn');
    const stepOutBtn = this.shadowRoot.getElementById('step-out-btn');
    const continueBtn = this.shadowRoot.getElementById('continue-btn');
    const addBreakpointBtn = this.shadowRoot.getElementById('add-breakpoint-btn');
    const toolbarBtns = this.shadowRoot.querySelectorAll('.toolbar-btn');

    stepBtn?.addEventListener('click', () => this._stepOver());
    stepInBtn?.addEventListener('click', () => this._stepInto());
    stepOutBtn?.addEventListener('click', () => this._stepOut());
    continueBtn?.addEventListener('click', () => this._continue());
    addBreakpointBtn?.addEventListener('click', () => this._addBreakpoint());

    toolbarBtns.forEach(btn => {
      btn.addEventListener('click', (e) => {
        const panel = e.currentTarget.dataset.panel;
        this._showPanel(panel);
      });
    });
  }

  _initDebugger() {
    this._addLog('info', 'Debugger initialized');
    this._addLog('info', 'Breakpoints: 2 active');
    this._addLog('warn', 'Consider using shadowRoot when debugging Web Components');
  }

  _showPanel(panelName) {
    const toolbarBtns = this.shadowRoot.querySelectorAll('.toolbar-btn');
    toolbarBtns.forEach(btn => {
      btn.classList.toggle('active', btn.dataset.panel === panelName);
    });

    const panels = ['console', 'sources', 'network', 'elements'];
    panels.forEach(p => {
      const el = this.shadowRoot.getElementById(`${p}-panel`);
      if (el) el.style.display = p === panelName ? 'block' : 'none';
    });
  }

  _stepOver() {
    this._addLog('info', 'Stepped over line 42 → 43');
    this._addLog('info', 'Evaluating: this.shadowRoot.innerHTML');
    this._addLog('info', 'Result: "<div>...</div>"');
  }

  _stepInto() {
    this._addLog('info', 'Stepping into function');
    this._addLog('info', '→ render() at component.ts:42');
    this._addLog('info', 'Paused at inline template evaluation');
  }

  _stepOut() {
    this._addLog('info', 'Stepping out of render()');
    this._addLog('info', 'Returning to updateComplete() at component.ts:115');
  }

  _continue() {
    this._addLog('success', '▶ Continuing execution...');
    setTimeout(() => {
      this._addLog('info', 'Breakpoint hit at component.ts:78');
    }, 500);
  }

  _addBreakpoint() {
    const list = this.shadowRoot.getElementById('breakpoint-list');
    const newBp = document.createElement('div');
    newBp.className = 'breakpoint-item';
    newBp.innerHTML = `
      <div class="breakpoint-icon"></div>
      <span class="breakpoint-file">component.ts</span>
      <span class="breakpoint-line">Line ${Math.floor(Math.random() * 100) + 50}</span>
    `;
    list.appendChild(newBp);
    this._addLog('info', 'Breakpoint added at component.ts:${Math.floor(Math.random() * 100) + 50}');
  }

  _addLog(type, message) {
    const consoleOutput = this.shadowRoot.getElementById('console-output');
    if (consoleOutput) {
      const line = document.createElement('div');
      line.className = `console-line log-${type}`;
      line.textContent = `> ${message}`;
      consoleOutput.appendChild(line);
      consoleOutput.scrollTop = consoleOutput.scrollHeight;
    }

    this._logs.push({ type, message });
  }

  get breakpoints() {
    return [...this._breakpoints];
  }

  get logs() {
    return [...this._logs];
  }

  get config() {
    return { ...this._config };
  }
}

customElements.define('debugging-tools-and-techniques', DebuggingToolsAndTechniques);

export { DebuggingToolsAndTechniques };