/**
 * Testing Framework Setup - Jest, Vitest, and Web Test Runner configuration
 * @module tooling/12_2_Testing-Framework-Setup
 * @version 1.0.0
 */

class TestingFrameworkSetup extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._testFramework = 'vitest';
    this._testFiles = [];
    this._testResults = [];
    this._coverage = {
      statements: 0,
      branches: 0,
      functions: 0,
      lines: 0,
    };
    this._config = {
      framework: 'vitest',
      watch: false,
      coverage: true,
      updateSnapshots: false,
      parallel: true,
      maxWorkers: 4,
    };
  }

  static get observedAttributes() {
    return ['framework', 'watch', 'coverage', 'update-snapshots'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updateConfig(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
    this._initializeTestFramework();
  }

  disconnectedCallback() {
    this._cleanup();
  }

  _updateConfig(name, value) {
    if (name === 'framework') {
      this._testFramework = value || 'vitest';
      this._config.framework = this._testFramework;
    } else if (name === 'watch') {
      this._config.watch = value === 'true';
    } else if (name === 'coverage') {
      this._config.coverage = value === 'true';
    } else if (name === 'update-snapshots') {
      this._config.updateSnapshots = value === 'true';
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
          background: #6a9955;
          color: #fff;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
        }
        .framework-selector {
          display: flex;
          gap: 8px;
          margin-bottom: 16px;
        }
        .framework-btn {
          padding: 8px 16px;
          border: 2px solid #3c3c3c;
          background: #2d2d2d;
          color: #d4d4d4;
          border-radius: 4px;
          cursor: pointer;
          transition: all 0.2s;
        }
        .framework-btn.active {
          border-color: #4ec9b0;
          background: #3a3a3a;
        }
        .framework-btn:hover {
          border-color: #569cd6;
        }
        .stats-grid {
          display: grid;
          grid-template-columns: repeat(4, 1fr);
          gap: 12px;
          margin-bottom: 16px;
        }
        .stat-card {
          background: #2d2d2d;
          padding: 16px;
          border-radius: 8px;
          text-align: center;
        }
        .stat-value {
          font-size: 24px;
          font-weight: 600;
          color: #4ec9b0;
        }
        .stat-label {
          font-size: 12px;
          color: #808080;
          margin-top: 4px;
        }
        .test-list {
          max-height: 300px;
          overflow-y: auto;
          background: #2d2d2d;
          border-radius: 4px;
          padding: 12px;
        }
        .test-item {
          display: flex;
          align-items: center;
          padding: 8px;
          border-bottom: 1px solid #3c3c3c;
        }
        .test-item:last-child {
          border-bottom: none;
        }
        .test-status {
          width: 8px;
          height: 8px;
          border-radius: 50%;
          margin-right: 12px;
        }
        .test-pass {
          background: #4ec9b0;
        }
        .test-fail {
          background: #f44747;
        }
        .test-pending {
          background: #cca700;
        }
        .test-name {
          flex: 1;
          font-size: 14px;
        }
        .test-duration {
          font-size: 12px;
          color: #808080;
        }
        .actions {
          margin-top: 16px;
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
        .btn-secondary {
          background: #3c3c3c;
        }
        .btn-secondary:hover {
          background: #4a4a4a;
        }
        .coverage-bar {
          height: 8px;
          background: #3c3c3c;
          border-radius: 4px;
          margin-top: 8px;
          overflow: hidden;
        }
        .coverage-fill {
          height: 100%;
          background: #4ec9b0;
          transition: width 0.3s;
        }
        .coverage-fill.low {
          background: #f44747;
        }
        .coverage-fill.medium {
          background: #cca700;
        }
      </style>
      <div class="container">
        <div class="header">
          <span class="title">Testing Framework Setup</span>
          <span class="badge">v1.0.0</span>
        </div>

        <div class="framework-selector">
          <button class="framework-btn active" data-framework="jest">Jest</button>
          <button class="framework-btn" data-framework="vitest">Vitest</button>
          <button class="framework-btn" data-framework="web-test-runner">Web Test Runner</button>
        </div>

        <div class="stats-grid">
          <div class="stat-card">
            <div class="stat-value" id="total-tests">0</div>
            <div class="stat-label">Total Tests</div>
          </div>
          <div class="stat-card">
            <div class="stat-value" id="passed-tests">0</div>
            <div class="stat-label">Passed</div>
          </div>
          <div class="stat-card">
            <div class="stat-value" id="failed-tests">0</div>
            <div class="stat-label">Failed</div>
          </div>
          <div class="stat-card">
            <div class="stat-value" id="test-duration">0ms</div>
            <div class="stat-label">Duration</div>
          </div>
        </div>

        <div class="test-list" id="test-list">
          <div class="test-item">
            <div class="test-status test-pending"></div>
            <span class="test-name">Loading tests...</span>
          </div>
        </div>

        <div class="actions">
          <button id="run-tests-btn">Run Tests</button>
          <button id="watch-btn" class="btn-secondary">Watch Mode</button>
          <button id="coverage-btn" class="btn-secondary">Coverage</button>
        </div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const runTestsBtn = this.shadowRoot.getElementById('run-tests-btn');
    const watchBtn = this.shadowRoot.getElementById('watch-btn');
    const coverageBtn = this.shadowRoot.getElementById('coverage-btn');
    const frameworkBtns = this.shadowRoot.querySelectorAll('.framework-btn');

    runTestsBtn?.addEventListener('click', () => this._runTests());
    watchBtn?.addEventListener('click', () => this._toggleWatchMode());
    coverageBtn?.addEventListener('click', () => this._toggleCoverage());

    frameworkBtns.forEach(btn => {
      btn.addEventListener('click', (e) => {
        const framework = e.target.dataset.framework;
        this._selectFramework(framework);
      });
    });
  }

  _initializeTestFramework() {
    this._testFiles = [
      { name: 'component.spec.js', tests: ['should render', 'should update', 'should handle events'] },
      { name: 'lifecycle.spec.js', tests: ['connectedCallback', 'disconnectedCallback', 'attributeChangedCallback'] },
      { name: 'shadow-dom.spec.js', tests: ['should create shadow root', 'should attach shadow DOM'] },
      { name: 'events.spec.js', tests: ['should dispatch custom events', 'should listen to events'] },
      { name: 'styles.spec.js', tests: ['should apply styles', 'should handle CSS variables'] },
    ];
  }

  _selectFramework(framework) {
    const frameworkBtns = this.shadowRoot.querySelectorAll('.framework-btn');
    frameworkBtns.forEach(btn => {
      btn.classList.toggle('active', btn.dataset.framework === framework);
    });
    this._testFramework = framework;
    this._config.framework = framework;
  }

  async _runTests() {
    const runTestsBtn = this.shadowRoot.getElementById('run-tests-btn');
    runTestsBtn.disabled = true;

    this._testResults = [];
    const testList = this.shadowRoot.getElementById('test-list');
    
    testList.innerHTML = '';

    const totalTests = this._testFiles.reduce((acc, f) => acc + f.tests.length, 0);
    let passed = 0;
    let failed = 0;

    for (const file of this._testFiles) {
      for (const testName of file.tests) {
        await this._simulateDelay(100);
        
        const isPass = Math.random() > 0.15;
        const result = {
          name: `${file.name} > ${testName}`,
          status: isPass ? 'pass' : 'fail',
        };
        this._testResults.push(result);

        if (isPass) passed++;
        else failed++;

        this._addTestItem(result);
      }
    }

    const duration = totalTests * 50 + Math.random() * 500;
    const totalTestsEl = this.shadowRoot.getElementById('total-tests');
    const passedTestsEl = this.shadowRoot.getElementById('passed-tests');
    const failedTestsEl = this.shadowRoot.getElementById('failed-tests');
    const durationEl = this.shadowRoot.getElementById('test-duration');

    if (totalTestsEl) totalTestsEl.textContent = totalTests;
    if (passedTestsEl) passedTestsEl.textContent = passed;
    if (failedTestsEl) failedTestsEl.textContent = failed;
    if (durationEl) durationEl.textContent = `${Math.round(duration)}ms`;

    if (this._config.coverage) {
      this._generateCoverage();
    }

    runTestsBtn.disabled = false;
  }

  _addTestItem(result) {
    const testList = this.shadowRoot.getElementById('test-list');
    const item = document.createElement('div');
    item.className = 'test-item';
    item.innerHTML = `
      <div class="test-status ${result.status === 'pass' ? 'test-pass' : 'test-fail'}"></div>
      <span class="test-name">${result.name}</span>
      <span class="test-duration">${Math.floor(Math.random() * 50)}ms</span>
    `;
    testList.appendChild(item);
    testList.scrollTop = testList.scrollHeight;
  }

  _generateCoverage() {
    this._coverage = {
      statements: 85 + Math.random() * 15,
      branches: 75 + Math.random() * 20,
      functions: 90 + Math.random() * 10,
      lines: 82 + Math.random() * 18,
    };
  }

  async _toggleWatchMode() {
    const watchBtn = this.shadowRoot.getElementById('watch-btn');
    this._config.watch = !this._config.watch;
    
    if (this._config.watch) {
      watchBtn.textContent = 'Stop Watch';
      this._startWatchLoop();
    } else {
      watchBtn.textContent = 'Watch Mode';
    }
  }

  _startWatchLoop() {
    const checkInterval = setInterval(async () => {
      if (!this._config.watch) {
        clearInterval(checkInterval);
        return;
      }

      const hasChanges = Math.random() > 0.7;
      if (hasChanges) {
        await this._runTests();
      }
    }, 2000);
  }

  _toggleCoverage() {
    this._config.coverage = !this._config.coverage;
  }

  _simulateDelay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  _cleanup() {
    this._config.watch = false;
    this._testResults = [];
  }

  get framework() {
    return this._testFramework;
  }

  set framework(value) {
    this._testFramework = value;
    this._config.framework = value;
    this._initializeTestFramework();
  }

  get testResults() {
    return [...this._testResults];
  }

  get config() {
    return { ...this._config };
  }
}

customElements.define('testing-framework-setup', TestingFrameworkSetup);

export { TestingFrameworkSetup };