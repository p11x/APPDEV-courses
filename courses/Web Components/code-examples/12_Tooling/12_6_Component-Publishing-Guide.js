/**
 * Component Publishing Guide - How to publish Web Components to npm
 * @module tooling/12_6_Component-Publishing-Guide
 * @version 1.0.0
 */

class ComponentPublishingGuide extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._packageInfo = {
      name: 'my-web-components',
      version: '1.0.0',
      description: 'Reusable Web Components',
      author: 'Your Name',
      license: 'MIT',
      keywords: ['web-components', 'custom-elements', 'ui-components'],
      homepage: 'https://github.com/username/my-web-components',
      repository: 'https://github.com/username/my-web-components',
    };
    this._publishConfig = {
      access: 'public',
      tag: 'latest',
      registry: 'https://registry.npmjs.org',
      twoFactorAuth: false,
    };
    this._isPublished = false;
    this._versions = [];
  }

  static get observedAttributes() {
    return ['name', 'version', 'access'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updatePackageInfo(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
  }

  _updatePackageInfo(key, value) {
    if (key === 'name') {
      this._packageInfo.name = value;
    } else if (key === 'version') {
      this._packageInfo.version = value;
    } else if (key === 'access') {
      this._publishConfig.access = value;
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
          background: #b5cea8;
          color: #1e1e1e;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
        }
        .package-header {
          background: #2d2d2d;
          border-radius: 8px;
          padding: 16px;
          margin-bottom: 16px;
        }
        .package-name {
          font-size: 24px;
          font-weight: 600;
          color: #4ec9b0;
          margin-bottom: 4px;
        }
        .package-version {
          font-size: 14px;
          color: #9cdcfe;
          display: flex;
          align-items: center;
          gap: 8px;
        }
        .version-badge {
          background: #569cd6;
          color: #fff;
          padding: 2px 6px;
          border-radius: 3px;
          font-size: 12px;
        }
        .package-description {
          margin-top: 12px;
          font-size: 14px;
          color: #d4d4d4;
        }
        .info-grid {
          display: grid;
          grid-template-columns: repeat(2, 1fr);
          gap: 12px;
          margin-top: 16px;
        }
        .info-item {
          display: flex;
          flex-direction: column;
          gap: 4px;
        }
        .info-label {
          font-size: 12px;
          color: #808080;
        }
        .info-value {
          font-size: 14px;
          color: #d4d4d4;
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
        .input {
          background: #3c3c3c;
          color: #d4d4d4;
          border: none;
          padding: 8px 12px;
          border-radius: 4px;
          width: 100%;
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
        .keywords {
          display: flex;
          flex-wrap: wrap;
          gap: 8px;
        }
        .keyword {
          background: #0e639c;
          color: #fff;
          padding: 4px 10px;
          border-radius: 12px;
          font-size: 12px;
        }
        .version-history {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 150px;
          overflow-y: auto;
        }
        .version-item {
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 8px 12px;
          border-bottom: 1px solid #3c3c3c;
        }
        .version-item:last-child {
          border-bottom: none;
        }
        .version-number {
          font-weight: 600;
          color: #9cdcfe;
        }
        .version-date {
          font-size: 12px;
          color: #808080;
        }
        .version-tag {
          font-size: 11px;
          padding: 2px 6px;
          border-radius: 3px;
          background: #808080;
          color: #1e1e1e;
        }
        .version-tag.latest {
          background: #4ec9b0;
        }
        .version-tag.beta {
          background: #cca700;
        }
        .publish-status {
          background: #2d2d2d;
          border-radius: 8px;
          padding: 16px;
          margin-bottom: 16px;
        }
        .status-indicator {
          display: flex;
          align-items: center;
          gap: 8px;
          margin-bottom: 8px;
        }
        .status-dot {
          width: 12px;
          height: 12px;
          border-radius: 50%;
          background: #808080;
        }
        .status-dot.published {
          background: #4ec9b0;
        }
        .status-text {
          font-size: 14px;
        }
        .publish-url {
          font-family: 'Consolas', monospace;
          font-size: 13px;
          color: #4ec9b0;
          word-break: break-all;
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
        .btn-publish {
          background: #b5cea8;
          color: #1e1e1e;
        }
        .btn-publish:hover {
          background: #c5deb8;
        }
      </style>
      <div class="container">
        <div class="header">
          <span class="title">Component Publishing</span>
          <span class="badge">v1.0.0</span>
        </div>

        <div class="package-header">
          <div class="package-name">@${this._packageInfo.name}</div>
          <div class="package-version">
            <span class="version-badge">v${this._packageInfo.version}</span>
            <span>Last updated: ${new Date().toLocaleDateString()}</span>
          </div>
          <div class="package-description">${this._packageInfo.description}</div>
          
          <div class="info-grid">
            <div class="info-item">
              <span class="info-label">Author</span>
              <span class="info-value">${this._packageInfo.author}</span>
            </div>
            <div class="info-item">
              <span class="info-label">License</span>
              <span class="info-value">${this._packageInfo.license}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Repository</span>
              <span class="info-value">${this._packageInfo.repository}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Homepage</span>
              <span class="info-value">${this._packageInfo.homepage}</span>
            </div>
          </div>

          <div style="margin-top: 12px;">
            <span class="info-label" style="margin-bottom: 8px; display: block;">Keywords</span>
            <div class="keywords">
              ${this._packageInfo.keywords.map(k => `<span class="keyword">${k}</span>`).join('')}
            </div>
          </div>
        </div>

        <div class="publish-status">
          <div class="status-indicator">
            <div class="status-dot" id="publish-status-dot"></div>
            <span class="status-text" id="publish-status-text">Not published</span>
          </div>
          <div class="publish-url" id="publish-url"></div>
        </div>

        <div class="section">
          <div class="section-title">Publish Configuration</div>
          <div class="config-grid">
            <div class="config-item">
              <span class="config-label">Registry</span>
              <select class="select" id="registry-select">
                <option value="npmjs">npmjs.org</option>
                <option value="vernpm">vernpm.dev</option>
                <option value="private">Private Registry</option>
              </select>
            </div>
            <div class="config-item">
              <span class="config-label">Access</span>
              <select class="select" id="access-select">
                <option value="public">Public</option>
                <option value="restricted">Restricted</option>
              </select>
            </div>
            <div class="config-item">
              <span class="config-label">Dist Tag</span>
              <select class="select" id="tag-select">
                <option value="latest">latest</option>
                <option value="beta">beta</option>
                <option value="next">next</option>
              </select>
            </div>
            <div class="config-item">
              <span class="config-label">2FA Required</span>
              <div class="toggle" id="2fa-toggle"></div>
            </div>
          </div>
        </div>

        <div class="section">
          <div class="section-title">Version History</div>
          <div class="version-history" id="version-history">
            <div class="version-item">
              <span class="version-number">v1.0.0</span>
              <span class="version-date">${new Date().toLocaleDateString()}</span>
              <span class="version-tag latest">latest</span>
            </div>
            <div class="version-item">
              <span class="version-number">v0.9.0</span>
              <span class="version-date">Jan 15, 2024</span>
              <span class="version-tag">deprecated</span>
            </div>
            <div class="version-item">
              <span class="version-number">v0.8.0-beta</span>
              <span class="version-date">Jan 1, 2024</span>
              <span class="version-tag beta">beta</span>
            </div>
          </div>
        </div>

        <div class="section">
          <div class="section-title">Files to Publish</div>
          <div class="version-history">
            <div class="version-item">
              <span>/dist</span>
              <span>3 files</span>
            </div>
            <div class="version-item">
              <span>/src</span>
              <span>12 files</span>
            </div>
            <div class="version-item">
              <span>README.md</span>
              <span>1 file</span>
            </div>
            <div class="version-item">
              <span>LICENSE</span>
              <span>1 file</span>
            </div>
          </div>
        </div>

        <div class="actions">
          <button id="login-btn">npm login</button>
          <button id="publish-btn" class="btn-publish">npm publish</button>
          <button id="tag-btn">npm dist-tag</button>
        </div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const loginBtn = this.shadowRoot.getElementById('login-btn');
    const publishBtn = this.shadowRoot.getElementById('publish-btn');
    const tagBtn = this.shadowRoot.getElementById('tag-btn');

    loginBtn?.addEventListener('click', () => this._npmLogin());
    publishBtn?.addEventListener('click', () => this._npmPublish());
    tagBtn?.addEventListener('click', () => this._manageTags());
  }

  _npmLogin() {
    const statusDot = this.shadowRoot.getElementById('publish-status-dot');
    const statusText = this.shadowRoot.getElementById('publish-status-text');

    statusDot.classList.add('published');
    statusText.textContent = 'Logged in';
  }

  async _npmPublish() {
    const publishBtn = this.shadowRoot.getElementById('publish-btn');
    const statusDot = this.shadowRoot.getElementById('publish-status-dot');
    const statusText = this.shadowRoot.getElementById('publish-status-text');
    const publishUrl = this.shadowRoot.getElementById('publish-url');

    publishBtn.disabled = true;
    statusText.textContent = 'Publishing...';

    await this._simulateDelay(1500);

    this._isPublished = true;
    statusDot.classList.add('published');
    statusText.textContent = 'Successfully published!';
    publishUrl.textContent = `https://www.npmjs.com/package/${this._packageInfo.name}`;

    publishBtn.disabled = false;
  }

  _manageTags() {
    const statusText = this.shadowRoot.getElementById('publish-status-text');
    statusText.textContent = 'Managing dist-tags...';

    setTimeout(() => {
      statusText.textContent = this._isPublished ? 'Published' : 'Ready to publish';
    }, 500);
  }

  _simulateDelay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  get packageInfo() {
    return { ...this._packageInfo };
  }

  get publishConfig() {
    return { ...this._publishConfig };
  }

  get isPublished() {
    return this._isPublished;
  }

  set packageInfo(value) {
    if (value && typeof value === 'object') {
      Object.assign(this._packageInfo, value);
    }
  }
}

customElements.define('component-publishing-guide', ComponentPublishingGuide);

export { ComponentPublishingGuide };