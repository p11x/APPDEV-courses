/**
 * Dependency Management - Package management and version control best practices
 * @module tooling/12_7_Dependency-Management
 * @version 1.0.0
 */

class DependencyManagement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._dependencies = [];
    this._devDependencies = [];
    this._updateAvailable = [];
    this._config = {
      autoUpdate: false,
      exactVersions: false,
      audit: true,
      fund: false,
    };
    this._auditResults = { vulnerabilities: 0, warnings: 0 };
  }

  static get observedAttributes() {
    return ['auto-update', 'audit', 'exact-versions'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updateConfig(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
    this._loadDependencies();
  }

  _updateConfig(name, value) {
    if (name === 'auto-update') {
      this._config.autoUpdate = value === 'true';
    } else if (name === 'audit') {
      this._config.audit = value === 'true';
    } else if (name === 'exact-versions') {
      this._config.exactVersions = value === 'true';
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
          background: #ce9178;
          color: #fff;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
        }
        .summary-bar {
          display: flex;
          gap: 16px;
          background: #2d2d2d;
          padding: 16px;
          border-radius: 8px;
          margin-bottom: 16px;
        }
        .summary-item {
          flex: 1;
          text-align: center;
        }
        .summary-value {
          font-size: 24px;
          font-weight: 600;
          color: #4ec9b0;
        }
        .summary-label {
          font-size: 12px;
          color: #808080;
        }
        .summary-value.warning {
          color: #cca700;
        }
        .summary-value.danger {
          color: #f44747;
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
          display: flex;
          align-items: center;
          justify-content: space-between;
        }
        .section-count {
          background: #3c3c3c;
          padding: 2px 8px;
          border-radius: 10px;
          font-size: 12px;
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
        .package-list {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 250px;
          overflow-y: auto;
        }
        .package-item {
          display: flex;
          align-items: center;
          padding: 10px 12px;
          border-bottom: 1px solid #3c3c3c;
        }
        .package-item:last-child {
          border-bottom: none;
        }
        .package-name {
          flex: 1;
          font-weight: 500;
          color: #9cdcfe;
        }
        .package-version {
          color: #ce9178;
          margin-right: 12px;
          font-family: 'Consolas', monospace;
        }
        .package-update {
          font-size: 12px;
          color: #4ec9b0;
        }
        .package-outdated {
          color: #cca700;
        }
        .package-deprecated {
          color: #f44747;
          text-decoration: line-through;
        }
        .audit-results {
          background: #1e1e1e;
          border-radius: 4px;
          padding: 12px;
        }
        .audit-item {
          display: flex;
          align-items: center;
          gap: 12px;
          padding: 8px 0;
          border-bottom: 1px solid #3c3c3c;
        }
        .audit-item:last-child {
          border-bottom: none;
        }
        .audit-severity {
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 11px;
          font-weight: 600;
        }
        .severity-critical {
          background: #f44747;
          color: #fff;
        }
        .severity-high {
          background: #ff6b6b;
          color: #fff;
        }
        .severity-moderate {
          background: #cca700;
          color: #000;
        }
        .audit-message {
          flex: 1;
          font-size: 13px;
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
        .btn-update {
          background: #4ec9b0;
          color: #1e1e1e;
        }
        .btn-update:hover {
          background: #5dd9c0;
        }
        .btn-audit {
          background: #cca700;
          color: #000;
        }
      </style>
      <div class="container">
        <div class="header">
          <span class="title">Dependency Management</span>
          <span class="badge">v1.0.0</span>
        </div>

        <div class="summary-bar">
          <div class="summary-item">
            <div class="summary-value" id="dep-count">8</div>
            <div class="summary-label">Dependencies</div>
          </div>
          <div class="summary-item">
            <div class="summary-value" id="dev-dep-count">15</div>
            <div class="summary-label">Dev Dependencies</div>
          </div>
          <div class="summary-item">
            <div class="summary-value warning" id="update-count">3</div>
            <div class="summary-label">Updates Available</div>
          </div>
          <div class="summary-item">
            <div class="summary-value danger" id="vulnerability-count">0</div>
            <div class="summary-label">Vulnerabilities</div>
          </div>
        </div>

        <div class="section">
          <div class="section-title">
            <span>Dependencies</span>
            <span class="section-count">8</span>
          </div>
          <div class="package-list" id="dep-list">
          </div>
        </div>

        <div class="section">
          <div class="section-title">
            <span>Dev Dependencies</span>
            <span class="section-count">15</span>
          </div>
          <div class="package-list" id="dev-dep-list">
          </div>
        </div>

        <div class="section">
          <div class="section-title">Configuration</div>
          <div class="config-grid">
            <div class="config-item">
              <span class="config-label">Auto Update</span>
              <div class="toggle" id="auto-update-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Exact Versions</span>
              <div class="toggle" id="exact-versions-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Audit on Install</span>
              <div class="toggle active" id="audit-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">npm fund</span>
              <div class="toggle" id="fund-toggle"></div>
            </div>
          </div>
        </div>

        <div class="section">
          <div class="section-title">
            <span>Security Audit</span>
            <span class="section-count" id="audit-count">0 issues</span>
          </div>
          <div class="audit-results" id="audit-results">
            <div class="audit-item">
              <span style="color: #4ec9b0;">No vulnerabilities found</span>
            </div>
          </div>
        </div>

        <div class="actions">
          <button id="install-btn">npm install</button>
          <button id="update-btn" class="btn-update">Update All</button>
          <button id="audit-btn" class="btn-audit">npm audit</button>
          <button id="outdated-btn">npm outdated</button>
        </div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const installBtn = this.shadowRoot.getElementById('install-btn');
    const updateBtn = this.shadowRoot.getElementById('update-btn');
    const auditBtn = this.shadowRoot.getElementById('audit-btn');
    const outdatedBtn = this.shadowRoot.getElementById('outdated-btn');

    installBtn?.addEventListener('click', () => this._npmInstall());
    updateBtn?.addEventListener('click', () => this._updateDependencies());
    auditBtn?.addEventListener('click', () => this._runAudit());
    outdatedBtn?.addEventListener('click', () => this._checkOutdated());

    const autoUpdateToggle = this.shadowRoot.getElementById('auto-update-toggle');
    const exactVersionsToggle = this.shadowRoot.getElementById('exact-versions-toggle');
    const auditToggle = this.shadowRoot.getElementById('audit-toggle');
    const fundToggle = this.shadowRoot.getElementById('fund-toggle');

    autoUpdateToggle?.addEventListener('click', () => {
      autoUpdateToggle.classList.toggle('active');
      this._config.autoUpdate = autoUpdateToggle.classList.contains('active');
    });

    exactVersionsToggle?.addEventListener('click', () => {
      exactVersionsToggle.classList.toggle('active');
      this._config.exactVersions = exactVersionsToggle.classList.contains('active');
    });

    auditToggle?.addEventListener('click', () => {
      auditToggle.classList.toggle('active');
      this._config.audit = auditToggle.classList.contains('active');
    });
  }

  _loadDependencies() {
    this._dependencies = [
      { name: 'lit', version: '2.4.0', latest: '3.1.0', status: 'outdated' },
      { name: '@lit/task', version: '0.2.0', latest: '0.2.0', status: 'ok' },
      { name: 'chart.js', version: '4.2.0', latest: '4.4.0', status: 'outdated' },
      { name: 'd3', version: '7.8.2', latest: '7.8.5', status: 'outdated' },
    ];

    this._devDependencies = [
      { name: 'typescript', version: '5.0.0', latest: '5.3.0', status: 'outdated' },
      { name: 'esbuild', version: '0.17.0', latest: '0.19.0', status: 'outdated' },
      { name: 'vite', version: '4.2.0', latest: '5.0.0', status: 'outdated' },
      { name: 'eslint', version: '8.50.0', latest: '8.56.0', status: 'outdated' },
    ];

    this._renderDependencyList();
  }

  _renderDependencyList() {
    const depList = this.shadowRoot.getElementById('dep-list');
    const devDepList = this.shadowRoot.getElementById('dev-dep-list');

    depList.innerHTML = '';
    devDepList.innerHTML = '';

    for (const dep of this._dependencies) {
      const item = document.createElement('div');
      item.className = 'package-item';
      let versionClass = '';
      let updateText = '';
      
      if (dep.status === 'outdated') {
        versionClass = 'package-outdated';
        updateText = `→ ${dep.latest}`;
      }

      item.innerHTML = `
        <span class="package-name">${dep.name}</span>
        <span class="package-version ${versionClass}">${dep.version}</span>
        <span class="package-update">${updateText}</span>
      `;
      depList.appendChild(item);
    }

    for (const dep of this._devDependencies) {
      const item = document.createElement('div');
      item.className = 'package-item';
      let versionClass = '';
      let updateText = '';
      
      if (dep.status === 'outdated') {
        versionClass = 'package-outdated';
        updateText = `→ ${dep.latest}`;
      }

      item.innerHTML = `
        <span class="package-name">${dep.name}</span>
        <span class="package-version ${versionClass}">${dep.version}</span>
        <span class="package-update">${updateText}</span>
      `;
      devDepList.appendChild(item);
    }

    const updateCount = this.shadowRoot.getElementById('update-count');
    const totalOutdated = [...this._dependencies, ...this._devDependencies].filter(d => d.status === 'outdated').length;
    if (updateCount) updateCount.textContent = totalOutdated;
  }

  _npmInstall() {
    const installBtn = this.shadowRoot.getElementById('install-btn');
    installBtn.disabled = true;
    installBtn.textContent = 'Installing...';

    setTimeout(() => {
      installBtn.disabled = false;
      installBtn.textContent = 'npm install';
    }, 1000);
  }

  async _updateDependencies() {
    const updateBtn = this.shadowRoot.getElementById('update-btn');
    updateBtn.disabled = true;
    updateBtn.textContent = 'Updating...';

    await this._simulateDelay(1500);

    for (const dep of this._dependencies) {
      dep.version = dep.latest;
      dep.status = 'ok';
    }

    for (const dep of this._devDependencies) {
      dep.version = dep.latest;
      dep.status = 'ok';
    }

    this._renderDependencyList();

    const updateCount = this.shadowRoot.getElementById('update-count');
    if (updateCount) updateCount.textContent = '0';

    updateBtn.disabled = false;
    updateBtn.textContent = 'Update All';
  }

  _runAudit() {
    const auditResults = this.shadowRoot.getElementById('audit-results');
    const auditCount = this.shadowRoot.getElementById('audit-count');
    const vulnerabilityCount = this.shadowRoot.getElementById('vulnerability-count');

    const issues = [
      { severity: 'moderate', message: 'Prototype Pollution in minimist' },
      { severity: 'high', message: 'Command Injection in lodash' },
    ];

    auditResults.innerHTML = '';
    
    for (const issue of issues) {
      const item = document.createElement('div');
      item.className = 'audit-item';
      item.innerHTML = `
        <span class="audit-severity severity-${issue.severity}">${issue.severity}</span>
        <span class="audit-message">${issue.message}</span>
      `;
      auditResults.appendChild(item);
    }

    this._auditResults.vulnerabilities = issues.length;
    if (auditCount) auditCount.textContent = `${issues.length} issues`;
    if (vulnerabilityCount) vulnerabilityCount.textContent = issues.length;
  }

  _checkOutdated() {
    this._renderDependencyList();
  }

  _simulateDelay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  get dependencies() {
    return [...this._dependencies];
  }

  get devDependencies() {
    return [...this._devDependencies];
  }

  set config(value) {
    if (value && typeof value === 'object') {
      Object.assign(this._config, value);
    }
  }
}

customElements.define('dependency-management', DependencyManagement);

export { DependencyManagement };