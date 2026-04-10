/**
 * Linting and Formatting Guide - ESLint and Prettier configuration
 * @module tooling/12_3_Linting-and-Formatting-Guide
 * @version 1.0.0
 */

class LintingAndFormattingGuide extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._lintResults = [];
    this._formatResults = [];
    this._config = {
      linter: 'eslint',
      formatter: 'prettier',
      autoFix: true,
      maxLineLength: 80,
      tabWidth: 2,
      semi: true,
      singleQuote: true,
      trailingComma: 'es5',
    };
    this._lintSettings = {
      extends: ['eslint:recommended'],
      parserOptions: {
        ecmaVersion: '2020',
        sourceType: 'module',
      },
      rules: {
        'no-unused-vars': 'warn',
        'no-console': 'off',
        'prefer-const': 'warn',
        'eqeqeq': 'error',
      },
    };
    this._prettierConfig = {
      printWidth: 80,
      tabWidth: 2,
      semi: true,
      singleQuote: true,
      trailingComma: 'es5',
    };
  }

  static get observedAttributes() {
    return ['auto-fix', 'max-line-length', 'semi', 'single-quote'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updateConfig(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
    this._initializeLintConfig();
  }

  _updateConfig(name, value) {
    if (name === 'auto-fix') {
      this._config.autoFix = value === 'true';
    } else if (name === 'max-line-length') {
      this._config.maxLineLength = parseInt(value, 10) || 80;
      this._prettierConfig.printWidth = this._config.maxLineLength;
    } else if (name === 'semi') {
      this._config.semi = value === 'true';
      this._prettierConfig.semi = this._config.semi;
    } else if (name === 'single-quote') {
      this._config.singleQuote = value === 'true';
      this._prettierConfig.singleQuote = this._config.singleQuote;
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
          background: #c586c0;
          color: #fff;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
        }
        .config-section {
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
        .toggle {
          position: relative;
          width: 48px;
          height: 24px;
          background: #3c3c3c;
          border-radius: 12px;
          cursor: pointer;
          transition: background 0.2s;
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
        .select {
          background: #3c3c3c;
          color: #d4d4d4;
          border: none;
          padding: 8px 12px;
          border-radius: 4px;
          cursor: pointer;
        }
        .input {
          background: #3c3c3c;
          color: #d4d4d4;
          border: none;
          padding: 8px 12px;
          border-radius: 4px;
          width: 100%;
        }
        .results-section {
          margin-bottom: 16px;
        }
        .results-title {
          display: flex;
          align-items: center;
          justify-content: space-between;
          font-size: 14px;
          font-weight: 500;
          margin-bottom: 8px;
        }
        .result-count {
          background: #3c3c3c;
          padding: 2px 8px;
          border-radius: 10px;
          font-size: 12px;
        }
        .result-list {
          background: #2d2d2d;
          border-radius: 4px;
          max-height: 200px;
          overflow-y: auto;
        }
        .result-item {
          display: flex;
          align-items: center;
          padding: 8px 12px;
          border-bottom: 1px solid #3c3c3c;
          font-size: 13px;
        }
        .result-item:last-child {
          border-bottom: none;
        }
        .result-icon {
          width: 16px;
          height: 16px;
          margin-right: 8px;
          border-radius: 50%;
        }
        .icon-error {
          background: #f44747;
        }
        .icon-warning {
          background: #cca700;
        }
        .icon-info {
          background: #569cd6;
        }
        .icon-success {
          background: #4ec9b0;
        }
        .result-message {
          flex: 1;
        }
        .result-location {
          color: #808080;
          font-size: 12px;
        }
        .actions {
          display: flex;
          gap: 8px;
          margin-top: 16px;
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
        .btn-secondary {
          background: #3c3c3c;
        }
        .btn-secondary:hover {
          background: #4a4a4a;
        }
      </style>
      <div class="container">
        <div class="header">
          <span class="title">Linting & Formatting</span>
          <span class="badge">v1.0.0</span>
        </div>

        <div class="config-section">
          <div class="section-title">ESLint Configuration</div>
          <div class="config-grid">
            <div class="config-item">
              <span class="config-label">Auto-fix on save</span>
              <div class="toggle active" id="auto-fix-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Max Line Length</span>
              <input class="input" type="number" id="max-line-input" value="80" />
            </div>
            <div class="config-item">
              <span class="config-label">Tab Width</span>
              <input class="input" type="number" id="tab-width-input" value="2" />
            </div>
            <div class="config-item">
              <span class="config-label">Semicolons</span>
              <div class="toggle active" id="semi-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Single Quotes</span>
              <div class="toggle active" id="single-quote-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Trailing Comma</span>
              <select class="select" id="trailing-comma-select">
                <option value="none">None</option>
                <option value="es5">ES5</option>
                <option value="all">All</option>
              </select>
            </div>
          </div>
        </div>

        <div class="results-section">
          <div class="results-title">
            <span>ESLint Results</span>
            <span class="result-count" id="lint-count">0 issues</span>
          </div>
          <div class="result-list" id="lint-results">
            <div class="result-item">
              <div class="result-icon icon-info"></div>
              <span class="result-message">Run lint to see results</span>
            </div>
          </div>
        </div>

        <div class="results-section">
          <div class="results-title">
            <span>Prettier Results</span>
            <span class="result-count" id="format-count">0 fixes</span>
          </div>
          <div class="result-list" id="format-results">
            <div class="result-item">
              <div class="result-icon icon-success"></div>
              <span class="result-message">Run format to see results</span>
            </div>
          </div>
        </div>

        <div class="actions">
          <button id="lint-btn">Run ESLint</button>
          <button id="format-btn" class="btn-secondary">Run Prettier</button>
          <button id="fix-btn" class="btn-secondary">Fix All</button>
        </div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const lintBtn = this.shadowRoot.getElementById('lint-btn');
    const formatBtn = this.shadowRoot.getElementById('format-btn');
    const fixBtn = this.shadowRoot.getElementById('fix-btn');

    const autoFixToggle = this.shadowRoot.getElementById('auto-fix-toggle');
    const semiToggle = this.shadowRoot.getElementById('semi-toggle');
    const singleQuoteToggle = this.shadowRoot.getElementById('single-quote-toggle');

    lintBtn?.addEventListener('click', () => this._runLinter());
    formatBtn?.addEventListener('click', () => this._runFormatter());
    fixBtn?.addEventListener('click', () => this._runFixAll());

    autoFixToggle?.addEventListener('click', () => {
      autoFixToggle.classList.toggle('active');
      this._config.autoFix = autoFixToggle.classList.contains('active');
    });

    semiToggle?.addEventListener('click', () => {
      semiToggle.classList.toggle('active');
      this._config.semi = semiToggle.classList.contains('active');
    });

    singleQuoteToggle?.addEventListener('click', () => {
      singleQuoteToggle.classList.toggle('active');
      this._config.singleQuote = singleQuoteToggle.classList.contains('active');
    });
  }

  _initializeLintConfig() {
    this._lintResults = [];
  }

  async _runLinter() {
    const lintBtn = this.shadowRoot.getElementById('lint-btn');
    lintBtn.disabled = true;

    const lintResults = this.shadowRoot.getElementById('lint-results');
    lintResults.innerHTML = '';

    const testIssues = [
      { severity: 'error', message: "Unexpected use of '===' in equality comparison", line: 42, column: 15 },
      { severity: 'warning', message: "'unusedVariable' is assigned a value but never used", line: 78, column: 5 },
      { severity: 'warning', message: "Unexpected console statement", line: 95, column: 3 },
      { severity: 'info', message: "Consider using 'const' instead of 'let'", line: 120, column: 3 },
    ];

    for (const issue of testIssues) {
      await this._simulateDelay(50);
      this._lintResults.push(issue);
      this._addLintResult(issue);
    }

    const lintCount = this.shadowRoot.getElementById('lint-count');
    if (lintCount) {
      lintCount.textContent = `${this._lintResults.length} issues`;
    }

    lintBtn.disabled = false;
  }

  _addLintResult(issue) {
    const lintResults = this.shadowRoot.getElementById('lint-results');
    const item = document.createElement('div');
    item.className = 'result-item';
    
    let iconClass = 'icon-info';
    if (issue.severity === 'error') iconClass = 'icon-error';
    else if (issue.severity === 'warning') iconClass = 'icon-warning';
    else if (issue.severity === 'info') iconClass = 'icon-info';

    item.innerHTML = `
      <div class="result-icon ${iconClass}"></div>
      <span class="result-message">${issue.message}</span>
      <span class="result-location">${issue.line}:${issue.column}</span>
    `;
    lintResults.appendChild(item);
  }

  async _runFormatter() {
    const formatBtn = this.shadowRoot.getElementById('format-btn');
    formatBtn.disabled = true;

    const formatResults = this.shadowRoot.getElementById('format-results');
    formatResults.innerHTML = '';

    const fixes = [
      { message: "Print width fixed (80 → 100)" },
      { message: "Trailing comma added" },
      { message: "Quote style fixed (double → single)" },
      { message: "Semicolon added" },
      { message: "Arrow function parentheses fixed" },
    ];

    for (const fix of fixes) {
      await this._simulateDelay(30);
      this._formatResults.push(fix);
      
      const item = document.createElement('div');
      item.className = 'result-item';
      item.innerHTML = `
        <div class="result-icon icon-success"></div>
        <span class="result-message">${fix.message}</span>
      `;
      formatResults.appendChild(item);
    }

    const formatCount = this.shadowRoot.getElementById('format-count');
    if (formatCount) {
      formatCount.textContent = `${fixes.length} fixes`;
    }

    formatBtn.disabled = false;
  }

  async _runFixAll() {
    const fixBtn = this.shadowRoot.getElementById('fix-btn');
    fixBtn.disabled = true;

    this._lintResults = [];
    this._formatResults = [];

    const fixEntries = [
      { message: "Auto-fixing: no-eqeq (error → 0)", type: 'lint' },
      { message: "Auto-fixing: no-unused-vars (warn → 0)", type: 'lint' },
      { message: "Auto-fixing: prefer-const (warn → 0)", type: 'lint' },
      { message: "Running prettier --write", type: 'format' },
    ];

    const lintResultsEl = this.shadowRoot.getElementById('lint-results');
    const formatResultsEl = this.shadowRoot.getElementById('format-results');
    
    lintResultsEl.innerHTML = '';
    formatResultsEl.innerHTML = '';

    for (const entry of fixEntries) {
      await this._simulateDelay(100);
      
      if (entry.type === 'lint') {
        const item = document.createElement('div');
        item.className = 'result-item';
        item.innerHTML = `
          <div class="result-icon icon-success"></div>
          <span class="result-message">${entry.message}</span>
        `;
        lintResultsEl.appendChild(item);
      } else {
        const item = document.createElement('div');
        item.className = 'result-item';
        item.innerHTML = `
          <div class="result-icon icon-success"></div>
          <span class="result-message">${entry.message}</span>
        `;
        formatResultsEl.appendChild(item);
      }
    }

    const lintCount = this.shadowRoot.getElementById('lint-count');
    const formatCount = this.shadowRoot.getElementById('format-count');
    if (lintCount) lintCount.textContent = '0 issues';
    if (formatCount) formatCount.textContent = '0 fixes';

    fixBtn.disabled = false;
  }

  _simulateDelay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  get linterConfig() {
    return { ...this._lintSettings };
  }

  get prettierConfig() {
    return { ...this._prettierConfig };
  }

  get lintResults() {
    return [...this._lintResults];
  }

  get formatResults() {
    return [...this._formatResults];
  }
}

customElements.define('linting-and-formatting-guide', LintingAndFormattingGuide);

export { LintingAndFormattingGuide };