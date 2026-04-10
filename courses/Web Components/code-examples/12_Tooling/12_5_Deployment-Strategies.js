/**
 * Deployment Strategies - Build, optimize, and deploy Web Components to production
 * @module tooling/12_5_Deployment-Strategies
 * @version 1.0.0
 */

class DeploymentStrategies extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._deploymentTarget = 'cdn';
    this._isDeploying = false;
    this._deploymentLog = [];
    this._config = {
      target: 'cdn',
      environment: 'production',
      optimize: true,
      minify: true,
      treeShake: true,
      compress: true,
      cacheBust: true,
      cdnProvider: 'unpkg',
    };
    this._buildArtifacts = [];
  }

  static get observedAttributes() {
    return ['target', 'environment', 'optimize'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updateConfig(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
    this._initializeDeployment();
  }

  _updateConfig(name, value) {
    if (name === 'target') {
      this._deploymentTarget = value || 'cdn';
      this._config.target = this._deploymentTarget;
    } else if (name === 'environment') {
      this._config.environment = value || 'production';
    } else if (name === 'optimize') {
      this._config.optimize = value === 'true';
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
        .target-selector {
          display: flex;
          gap: 8px;
          margin-bottom: 16px;
        }
        .target-btn {
          flex: 1;
          padding: 12px;
          border: 2px solid #3c3c3c;
          background: #2d2d2d;
          color: #d4d4d4;
          border-radius: 4px;
          cursor: pointer;
          text-align: center;
          transition: all 0.2s;
        }
        .target-btn.active {
          border-color: #c586c0;
          background: #3a3a3a;
        }
        .target-btn:hover {
          border-color: #569cd6;
        }
        .target-icon {
          font-size: 24px;
          margin-bottom: 4px;
        }
        .target-label {
          font-size: 12px;
        }
        .status-bar {
          display: flex;
          align-items: center;
          justify-content: space-between;
          background: #2d2d2d;
          padding: 12px 16px;
          border-radius: 8px;
          margin-bottom: 16px;
        }
        .status-indicator {
          display: flex;
          align-items: center;
          gap: 8px;
        }
        .status-dot {
          width: 12px;
          height: 12px;
          border-radius: 50%;
          background: #808080;
        }
        .status-dot.active {
          background: #cca700;
          animation: pulse 1s infinite;
        }
        .status-dot.success {
          background: #4ec9b0;
        }
        .status-dot.error {
          background: #f44747;
        }
        @keyframes pulse {
          0%, 100% { opacity: 1; }
          50% { opacity: 0.5; }
        }
        .status-text {
          font-size: 14px;
        }
        .progress-bar {
          width: 200px;
          height: 6px;
          background: #3c3c3c;
          border-radius: 3px;
          overflow: hidden;
          display: none;
        }
        .progress-bar.active {
          display: block;
        }
        .progress-fill {
          height: 100%;
          background: linear-gradient(90deg, #4ec9b0, #c586c0);
          width: 0%;
          transition: width 0.3s;
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
        .deployment-log {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 200px;
          overflow-y: auto;
          font-family: 'Consolas', monospace;
          font-size: 12px;
        }
        .log-entry {
          padding: 6px 12px;
          border-bottom: 1px solid #3c3c3c;
        }
        .log-entry:last-child {
          border-bottom: none;
        }
        .log-time {
          color: #808080;
          margin-right: 8px;
        }
        .log-info {
          color: #569cd6;
        }
        .log-success {
          color: #4ec9b0;
        }
        .log-warning {
          color: #cca700;
        }
        .log-error {
          color: #f44747;
        }
        .build-artifacts {
          background: #1e1e1e;
          border-radius: 4px;
          padding: 12px;
        }
        .artifact-item {
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 8px;
          background: #2d2d2d;
          border-radius: 4px;
          margin-bottom: 8px;
        }
        .artifact-item:last-child {
          margin-bottom: 0;
        }
        .artifact-name {
          font-family: 'Consolas', monospace;
          font-size: 13px;
        }
        .artifact-size {
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
        .btn-deploy {
          background: #4ec9b0;
          color: #1e1e1e;
        }
        .btn-deploy:hover {
          background: #5dd9c0;
        }
        .btn-danger {
          background: #f44747;
        }
        .btn-danger:hover {
          background: #f55757;
        }
      </style>
      <div class="container">
        <div class="header">
          <span class="title">Deployment Strategies</span>
          <span class="badge">v1.0.0</span>
        </div>

        <div class="target-selector">
          <div class="target-btn" data-target="cdn">
            <div class="target-icon">☁️</div>
            <div class="target-label">CDN/npm</div>
          </div>
          <div class="target-btn" data-target="static">
            <div class="target-icon">📁</div>
            <div class="target-label">Static Hosting</div>
          </div>
          <div class="target-btn" data-target="docker">
            <div class="target-icon">🐳</div>
            <div class="target-label">Docker</div>
          </div>
          <div class="target-btn" data-target="serverless">
            <div class="target-icon">⚡</div>
            <div class="target-label">Serverless</div>
          </div>
        </div>

        <div class="status-bar">
          <div class="status-indicator">
            <div class="status-dot" id="status-dot"></div>
            <span class="status-text" id="status-text">Ready to deploy</span>
          </div>
          <div class="progress-bar" id="progress-bar">
            <div class="progress-fill" id="progress-fill"></div>
          </div>
        </div>

        <div class="config-section">
          <div class="section-title">Build Configuration</div>
          <div class="config-grid">
            <div class="config-item">
              <span class="config-label">Minify Output</span>
              <div class="toggle active" id="minify-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Tree Shaking</span>
              <div class="toggle active" id="tree-shake-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Gzip Compression</span>
              <div class="toggle active" id="compress-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Cache Busting</span>
              <div class="toggle active" id="cache-bust-toggle"></div>
            </div>
          </div>
        </div>

        <div class="config-section">
          <div class="section-title">Build Artifacts</div>
          <div class="build-artifacts" id="build-artifacts">
            <div class="artifact-item">
              <span class="artifact-name">web-components.js</span>
              <span class="artifact-size">--</span>
            </div>
            <div class="artifact-item">
              <span class="artifact-name">web-components.min.js</span>
              <span class="artifact-size">--</span>
            </div>
            <div class="artifact-item">
              <span class="artifact-name">web-components.d.ts</span>
              <span class="artifact-size">--</span>
            </div>
          </div>
        </div>

        <div class="config-section">
          <div class="section-title">Deployment Log</div>
          <div class="deployment-log" id="deployment-log">
            <div class="log-entry">
              <span class="log-time">--:--:--</span>
              <span class="log-info">Ready to deploy</span>
            </div>
          </div>
        </div>

        <div class="actions">
          <button id="build-btn">Build</button>
          <button id="deploy-btn" class="btn-deploy">Deploy</button>
          <button id="rollback-btn" class="btn-danger" disabled>Rollback</button>
        </div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const buildBtn = this.shadowRoot.getElementById('build-btn');
    const deployBtn = this.shadowRoot.getElementById('deploy-btn');
    const rollbackBtn = this.shadowRoot.getElementById('rollback-btn');
    const targetBtns = this.shadowRoot.querySelectorAll('.target-btn');

    buildBtn?.addEventListener('click', () => this._runBuild());
    deployBtn?.addEventListener('click', () => this._deploy());
    rollbackBtn?.addEventListener('click', () => this._rollback());

    targetBtns.forEach(btn => {
      btn.addEventListener('click', (e) => {
        const target = e.currentTarget.dataset.target;
        this._selectTarget(target);
      });
    });
  }

  _initializeDeployment() {
    this._deploymentLog = [];
    this._buildArtifacts = [];
  }

  _selectTarget(target) {
    const targetBtns = this.shadowRoot.querySelectorAll('.target-btn');
    targetBtns.forEach(btn => {
      btn.classList.toggle('active', btn.dataset.target === target);
    });
    this._deploymentTarget = target;
    this._config.target = target;
  }

  async _runBuild() {
    const buildBtn = this.shadowRoot.getElementById('build-btn');
    const statusDot = this.shadowRoot.getElementById('status-dot');
    const statusText = this.shadowRoot.getElementById('status-text');
    const progressBar = this.shadowRoot.getElementById('progress-bar');
    const progressFill = this.shadowRoot.getElementById('progress-fill');

    buildBtn.disabled = true;
    statusDot.classList.add('active');
    statusText.textContent = 'Building...';
    progressBar.classList.add('active');

    this._log('info', 'Starting build process...');
    this._log('info', `Target: ${this._deploymentTarget}`);

    for (let i = 0; i <= 100; i += 20) {
      progressFill.style.width = `${i}%`;
      await this._simulateDelay(200);
      
      if (i === 20) this._log('info', 'Optimizing modules...');
      if (i === 40) this._log('info', 'Tree-shaking unused exports...');
      if (i === 60) this._log('info', 'Minifying JavaScript...');
      if (i === 80) this._log('info', 'Generating TypeScript definitions...');
    }

    this._log('success', 'Build completed successfully');
    this._log('info', 'Output: dist/');

    this._updateBuildArtifacts([
      { name: 'web-components.js', size: '45.2 KB' },
      { name: 'web-components.min.js', size: '18.7 KB' },
      { name: 'web-components.d.ts', size: '12.4 KB' },
    ]);

    statusDot.classList.remove('active');
    statusDot.classList.add('success');
    statusText.textContent = 'Build complete';
    buildBtn.disabled = false;
  }

  async _deploy() {
    const deployBtn = this.shadowRoot.getElementById('deploy-btn');
    const rollbackBtn = this.shadowRoot.getElementById('rollback-btn');
    const statusDot = this.shadowRoot.getElementById('status-dot');
    const statusText = this.shadowRoot.getElementById('status-text');
    const progressBar = this.shadowRoot.getElementById('progress-bar');
    const progressFill = this.shadowRoot.getElementById('progress-fill');

    deployBtn.disabled = true;
    statusDot.classList.add('active');
    statusText.textContent = 'Deploying...';
    progressBar.classList.add('active');
    progressFill.style.width = '0%';

    this._log('info', `Deploying to ${this._deploymentTarget}...`);
    await this._simulateDelay(300);
    this._log('info', 'Uploading artifacts...');

    for (let i = 0; i <= 100; i += 25) {
      progressFill.style.width = `${i}%`;
      await this._simulateDelay(300);
      
      if (i === 25) this._log('info', 'Uploading web-components.js...');
      if (i === 50) this._log('info', 'Uploading web-components.min.js...');
      if (i === 75) this._log('info', 'Uploading TypeScript definitions...');
    }

    this._log('success', 'Deployment successful!');
    this._log('info', `URL: https://cdn.example.com/web-components/v1.0.0.js`);

    statusDot.classList.remove('active');
    statusDot.classList.add('success');
    statusText.textContent = 'Deployed';
    rollbackBtn.disabled = false;
  }

  _rollback() {
    const rollbackBtn = this.shadowRoot.getElementById('rollback-btn');
    const statusDot = this.shadowRoot.getElementById('status-dot');
    const statusText = this.shadowRoot.getElementById('status-text');

    this._log('warning', 'Rolling back to previous version...');
    
    setTimeout(() => {
      this._log('success', 'Rollback complete');
      statusDot.classList.remove('success');
      statusText.textContent = 'Ready to deploy';
      rollbackBtn.disabled = true;
    }, 1000);
  }

  _updateBuildArtifacts(artifacts) {
    const artifactsList = this.shadowRoot.getElementById('build-artifacts');
    artifactsList.innerHTML = '';

    for (const artifact of artifacts) {
      const item = document.createElement('div');
      item.className = 'artifact-item';
      item.innerHTML = `
        <span class="artifact-name">${artifact.name}</span>
        <span class="artifact-size">${artifact.size}</span>
      `;
      artifactsList.appendChild(item);
    }
  }

  _log(type, message) {
    const log = this.shadowRoot.getElementById('deployment-log');
    const now = new Date();
    const time = `${now.getHours().toString().padStart(2, '0')}:${now.getMinutes().toString().padStart(2, '0')}:${now.getSeconds().toString().padStart(2, '0')}`;

    const entry = document.createElement('div');
    entry.className = 'log-entry';
    entry.innerHTML = `
      <span class="log-time">${time}</span>
      <span class="log-${type}">${message}</span>
    `;
    log.appendChild(entry);
    log.scrollTop = log.scrollHeight;
  }

  _simulateDelay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  get config() {
    return { ...this._config };
  }

  set config(value) {
    if (value && typeof value === 'object') {
      Object.assign(this._config, value);
    }
  }

  get deploymentTarget() {
    return this._deploymentTarget;
  }

  get deploymentLog() {
    return [...this._deploymentLog];
  }
}

customElements.define('deployment-strategies', DeploymentStrategies);

export { DeploymentStrategies };