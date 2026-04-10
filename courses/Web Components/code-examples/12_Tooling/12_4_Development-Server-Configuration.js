/**
 * Development Server Configuration - Local dev server setup and proxy configuration
 * @module tooling/12_4_Development-Server-Configuration
 * @version 1.0.0
 */

class DevelopmentServerConfiguration extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._serverConfig = {
      port: 3000,
      host: 'localhost',
      https: false,
      hot: true,
      open: true,
      proxy: [],
      middleware: [],
    };
    this._isRunning = false;
    this._requestLog = [];
    this._activeConnections = 0;
  }

  static get observedAttributes() {
    return ['port', 'hot', 'https', 'open'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updateConfig(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
    this._initializeServer();
  }

  _updateConfig(name, value) {
    if (name === 'port') {
      this._serverConfig.port = parseInt(value, 10) || 3000;
    } else if (name === 'hot') {
      this._serverConfig.hot = value === 'true';
    } else if (name === 'https') {
      this._serverConfig.https = value === 'true';
    } else if (name === 'open') {
      this._serverConfig.open = value === 'true';
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
        .status-dot.running {
          background: #4ec9b0;
          box-shadow: 0 0 8px #4ec9b0;
        }
        .status-text {
          font-size: 14px;
        }
        .status-url {
          font-family: 'Consolas', monospace;
          font-size: 14px;
          color: #4ec9b0;
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
        .proxy-list {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 120px;
          overflow-y: auto;
        }
        .proxy-item {
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 8px 12px;
          border-bottom: 1px solid #3c3c3c;
          font-size: 13px;
        }
        .proxy-item:last-child {
          border-bottom: none;
        }
        .proxy-source {
          color: #ce9178;
        }
        .proxy-target {
          color: #4ec9b0;
        }
        .request-log {
          background: #1e1e1e;
          border-radius: 4px;
          max-height: 250px;
          overflow-y: auto;
        }
        .request-item {
          display: flex;
          align-items: center;
          padding: 8px 12px;
          border-bottom: 1px solid #3c3c3c;
          font-size: 13px;
        }
        .request-item:last-child {
          border-bottom: none;
        }
        .method {
          font-weight: 600;
          width: 60px;
          padding: 2px 6px;
          border-radius: 3px;
          text-align: center;
          font-size: 11px;
          margin-right: 12px;
        }
        .method-get {
          background: #0e639c;
          color: #fff;
        }
        .method-post {
          background: #6a9955;
          color: #fff;
        }
        .method-put {
          background: #cca700;
          color: #000;
        }
        .method-delete {
          background: #f44747;
          color: #fff;
        }
        .path {
          flex: 1;
          font-family: 'Consolas', monospace;
        }
        .status {
          margin-left: 12px;
        }
        .status-200 {
          color: #4ec9b0;
        }
        .status-201 {
          color: #4ec9b0;
        }
        .status-400 {
          color: #cca700;
        }
        .status-404 {
          color: #f44747;
        }
        .status-500 {
          color: #f44747;
        }
        .duration {
          color: #808080;
          font-size: 12px;
          margin-left: 12px;
        }
        .connections {
          display: flex;
          align-items: center;
          gap: 8px;
          margin-top: 8px;
          font-size: 12px;
          color: #808080;
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
        .btn-success {
          background: #4ec9b0;
        }
        .btn-success:hover {
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
          <span class="title">Development Server</span>
          <span class="badge">v1.0.0</span>
        </div>

        <div class="status-bar">
          <div class="status-indicator">
            <div class="status-dot" id="status-dot"></div>
            <span class="status-text" id="status-text">Server stopped</span>
          </div>
          <span class="status-url" id="status-url">http://localhost:${this._serverConfig.port}</span>
        </div>

        <div class="config-section">
          <div class="section-title">Server Configuration</div>
          <div class="config-grid">
            <div class="config-item">
              <span class="config-label">Port</span>
              <input class="input" type="number" id="port-input" value="${this._serverConfig.port}" />
            </div>
            <div class="config-item">
              <span class="config-label">Host</span>
              <input class="input" type="text" id="host-input" value="${this._serverConfig.host}" />
            </div>
            <div class="config-item">
              <span class="config-label">Hot Reload</span>
              <div class="toggle active" id="hot-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">Auto Open Browser</span>
              <div class="toggle active" id="open-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">HTTPS</span>
              <div class="toggle" id="https-toggle"></div>
            </div>
            <div class="config-item">
              <span class="config-label">CORS Enabled</span>
              <div class="toggle active" id="cors-toggle"></div>
            </div>
          </div>
        </div>

        <div class="config-section">
          <div class="section-title">Proxy Configuration</div>
          <div class="proxy-list" id="proxy-list">
            <div class="proxy-item">
              <span class="proxy-source">/api</span>
              <span>→</span>
              <span class="proxy-target">http://localhost:8080/api</span>
            </div>
          </div>
        </div>

        <div class="config-section">
          <div class="section-title">Request Log</div>
          <div class="request-log" id="request-log">
            <div class="request-item" style="color: #808080;">
              Waiting for requests...
            </div>
          </div>
          <div class="connections">
            <span>Active connections:</span>
            <span id="active-connections">0</span>
          </div>
        </div>

        <div class="actions">
          <button id="start-btn" class="btn-success">Start Server</button>
          <button id="stop-btn" class="btn-danger" disabled>Stop Server</button>
          <button id="restart-btn" disabled>Restart</button>
        </div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const startBtn = this.shadowRoot.getElementById('start-btn');
    const stopBtn = this.shadowRoot.getElementById('stop-btn');
    const restartBtn = this.shadowRoot.getElementById('restart-btn');

    startBtn?.addEventListener('click', () => this._startServer());
    stopBtn?.addEventListener('click', () => this._stopServer());
    restartBtn?.addEventListener('click', () => this._restartServer());

    const hotToggle = this.shadowRoot.getElementById('hot-toggle');
    const openToggle = this.shadowRoot.getElementById('open-toggle');
    const httpsToggle = this.shadowRoot.getElementById('https-toggle');

    hotToggle?.addEventListener('click', () => {
      hotToggle.classList.toggle('active');
      this._serverConfig.hot = hotToggle.classList.contains('active');
    });

    openToggle?.addEventListener('click', () => {
      openToggle.classList.toggle('active');
      this._serverConfig.open = openToggle.classList.contains('active');
    });

    httpsToggle?.addEventListener('click', () => {
      httpsToggle.classList.toggle('active');
      this._serverConfig.https = httpsToggle.classList.contains('active');
      this._updateStatusUrl();
    });
  }

  _initializeServer() {
    this._isRunning = false;
    this._requestLog = [];
  }

  _updateStatusUrl() {
    const statusUrl = this.shadowRoot.getElementById('status-url');
    const protocol = this._serverConfig.https ? 'https' : 'http';
    if (statusUrl) {
      statusUrl.textContent = `${protocol}://${this._serverConfig.host}:${this._serverConfig.port}`;
    }
  }

  async _startServer() {
    const startBtn = this.shadowRoot.getElementById('start-btn');
    const stopBtn = this.shadowRoot.getElementById('stop-btn');
    const restartBtn = this.shadowRoot.getElementById('restart-btn');
    const statusDot = this.shadowRoot.getElementById('status-dot');
    const statusText = this.shadowRoot.getElementById('status-text');

    startBtn.disabled = true;
    stopBtn.disabled = false;
    restartBtn.disabled = false;

    statusDot.classList.add('running');
    statusText.textContent = 'Server running';

    this._isRunning = true;
    this._updateStatusUrl();

    this._simulateRequests();
  }

  async _stopServer() {
    const startBtn = this.shadowRoot.getElementById('start-btn');
    const stopBtn = this.shadowRoot.getElementById('stop-btn');
    const restartBtn = this.shadowRoot.getElementById('restart-btn');
    const statusDot = this.shadowRoot.getElementById('status-dot');
    const statusText = this.shadowRoot.getElementById('status-text');

    startBtn.disabled = false;
    stopBtn.disabled = true;
    restartBtn.disabled = true;

    statusDot.classList.remove('running');
    statusText.textContent = 'Server stopped';

    this._isRunning = false;
  }

  _restartServer() {
    this._stopServer();
    setTimeout(() => this._startServer(), 500);
  }

  _simulateRequests() {
    const endpoints = [
      { path: '/', method: 'GET', status: 200, minDuration: 10, maxDuration: 50 },
      { path: '/index.html', method: 'GET', status: 200, minDuration: 5, maxDuration: 30 },
      { path: '/bundle.js', method: 'GET', status: 200, minDuration: 20, maxDuration: 100 },
      { path: '/styles.css', method: 'GET', status: 200, minDuration: 10, maxDuration: 40 },
      { path: '/api/users', method: 'GET', status: 200, minDuration: 50, maxDuration: 150 },
      { path: '/api/users', method: 'POST', status: 201, minDuration: 100, maxDuration: 300 },
      { path: '/api/data', method: 'GET', status: 200, minDuration: 30, maxDuration: 80 },
      { path: '/api/data', method: 'PUT', status: 200, minDuration: 80, maxDuration: 200 },
      { path: '/api/unknown', method: 'GET', status: 404, minDuration: 10, maxDuration: 30 },
    ];

    const requestInterval = setInterval(() => {
      if (!this._isRunning) {
        clearInterval(requestInterval);
        return;
      }

      const endpoint = endpoints[Math.floor(Math.random() * endpoints.length)];
      const duration = Math.floor(Math.random() * (endpoint.maxDuration - endpoint.minDuration)) + endpoint.minDuration;
      
      this._activeConnections++;
      this._updateActiveConnections();

      setTimeout(() => {
        this._activeConnections--;
        this._addRequestLog(endpoint, duration);
        this._updateActiveConnections();
      }, duration);
    }, 800);
  }

  _addRequestLog(endpoint, duration) {
    const requestLog = this.shadowRoot.getElementById('request-log');
    
    if (requestLog.children.length > 0 && requestLog.children[0].textContent === 'Waiting for requests...') {
      requestLog.innerHTML = '';
    }

    const item = document.createElement('div');
    item.className = 'request-item';
    
    let statusClass = 'status-200';
    if (endpoint.status === 201) statusClass = 'status-201';
    else if (endpoint.status >= 400 && endpoint.status < 500) statusClass = 'status-400';
    else if (endpoint.status >= 500) statusClass = 'status-500';

    let methodClass = 'method-get';
    if (endpoint.method === 'POST') methodClass = 'method-post';
    else if (endpoint.method === 'PUT') methodClass = 'method-put';
    else if (endpoint.method === 'DELETE') methodClass = 'method-delete';

    item.innerHTML = `
      <span class="method ${methodClass}">${endpoint.method}</span>
      <span class="path">${endpoint.path}</span>
      <span class="status ${statusClass}">${endpoint.status}</span>
      <span class="duration">${duration}ms</span>
    `;
    
    requestLog.insertBefore(item, requestLog.firstChild);
    
    if (requestLog.children.length > 20) {
      requestLog.removeChild(requestLog.lastChild);
    }
  }

  _updateActiveConnections() {
    const activeConnections = this.shadowRoot.getElementById('active-connections');
    if (activeConnections) {
      activeConnections.textContent = this._activeConnections;
    }
  }

  get serverConfig() {
    return { ...this._serverConfig };
  }

  set serverConfig(value) {
    if (value && typeof value === 'object') {
      Object.assign(this._serverConfig, value);
    }
  }

  get isRunning() {
    return this._isRunning;
  }

  get requestLog() {
    return [...this._requestLog];
  }
}

customElements.define('development-server-configuration', DevelopmentServerConfiguration);

export { DevelopmentServerConfiguration };