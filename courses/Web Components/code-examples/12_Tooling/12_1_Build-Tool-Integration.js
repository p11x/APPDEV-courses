/**
 * Build Tool Integration - Webpack and Vite configuration for Web Components
 * @module tooling/12_1_Build-Tool-Integration
 * @version 1.0.0
 */

class BuildToolIntegration extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._config = {
      buildSystem: 'webpack',
      watch: false,
      minify: true,
      sourceMap: false,
      target: 'es2020',
      moduleFormat: 'es',
    };
    this._webpackConfig = null;
    this._viteConfig = null;
    this._bundle = null;
    this._compilationResults = [];
  }

  static get observedAttributes() {
    return ['build-system', 'watch', 'minify', 'source-map', 'target'];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._updateConfig(name, newValue);
    }
  }

  connectedCallback() {
    this._render();
    this._initializeBuildSystem();
  }

  disconnectedCallback() {
    this._cleanup();
  }

  _updateConfig(key, value) {
    const configKey = key.replace(/-([a-z])/g, (g) => g[1].toUpperCase());
    if (key === 'build-system') {
      this._config.buildSystem = value || 'webpack';
    } else if (key === 'watch') {
      this._config.watch = value === 'true';
    } else if (key === 'minify') {
      this._config.minify = value === 'true';
    } else if (key === 'source-map') {
      this._config.sourceMap = value === 'true';
    } else if (key === 'target') {
      this._config.target = value || 'es2020';
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
          background: #0e639c;
          color: #fff;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
        }
        .config-section {
          margin-bottom: 16px;
        }
        .config-title {
          font-size: 14px;
          font-weight: 500;
          color: #9cdcfe;
          margin-bottom: 8px;
        }
        .config-item {
          display: flex;
          justify-content: space-between;
          padding: 8px;
          background: #2d2d2d;
          border-radius: 4px;
          margin-bottom: 4px;
        }
        .config-key {
          color: #9cdcfe;
        }
        .config-value {
          color: #ce9178;
        }
        .build-status {
          padding: 12px;
          border-radius: 4px;
          margin-top: 16px;
        }
        .status-idle {
          background: #2d2d2d;
          color: #808080;
        }
        .status-building {
          background: #3a3a00;
          color: #ffe600;
        }
        .status-success {
          background: #003300;
          color: #4ec9b0;
        }
        .status-error {
          background: #3a0000;
          color: #f44747;
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
        .logs {
          max-height: 200px;
          overflow-y: auto;
          background: #1e1e1e;
          border-radius: 4px;
          padding: 12px;
          font-family: 'Consolas', 'Courier New', monospace;
          font-size: 12px;
          white-space: pre-wrap;
        }
        .log-entry {
          margin-bottom: 4px;
        }
        .log-info {
          color: #d4d4d4;
        }
        .log-warn {
          color: #cca700;
        }
        .log-error {
          color: #f44747;
        }
        .log-success {
          color: #4ec9b0;
        }
      </style>
      <div class="container">
        <div class="header">
          <span class="title">Build Tool Integration</span>
          <span class="badge">v1.0.0</span>
        </div>
        
        <div class="config-section">
          <div class="config-title">Build Configuration</div>
          <div class="config-item">
            <span class="config-key">Build System</span>
            <span class="config-value" id="build-system">${this._config.buildSystem}</span>
          </div>
          <div class="config-item">
            <span class="config-key">Target</span>
            <span class="config-value" id="target">${this._config.target}</span>
          </div>
          <div class="config-item">
            <span class="config-key">Watch Mode</span>
            <span class="config-value">${this._config.watch ? 'Enabled' : 'Disabled'}</span>
          </div>
          <div class="config-item">
            <span class="config-key">Minify</span>
            <span class="config-value">${this._config.minify ? 'Yes' : 'No'}</span>
          </div>
          <div class="config-item">
            <span class="config-key">Source Map</span>
            <span class="config-value">${this._config.sourceMap ? 'Yes' : 'No'}</span>
          </div>
        </div>

        <div class="build-status status-idle" id="status">
          Ready to build
        </div>

        <div style="margin-top: 16px;">
          <button id="build-btn">Run Build</button>
          <button id="watch-btn" style="margin-left: 8px;">Start Watch</button>
        </div>

        <div class="logs" id="logs"></div>
      </div>
    `;

    this._setupEventListeners();
  }

  _setupEventListeners() {
    const buildBtn = this.shadowRoot.getElementById('build-btn');
    const watchBtn = this.shadowRoot.getElementById('watch-btn');

    if (buildBtn) {
      buildBtn.addEventListener('click', () => this._runBuild());
    }

    if (watchBtn) {
      watchBtn.addEventListener('click', () => this._toggleWatchMode());
    }
  }

  _initializeBuildSystem() {
    this._log('info', `Initializing ${this._config.buildSystem} build system...`);
    
    if (this._config.buildSystem === 'webpack') {
      this._initializeWebpack();
    } else if (this._config.buildSystem === 'vite') {
      this._initializeVite();
    }
  }

  _initializeWebpack() {
    this._webpackConfig = {
      mode: 'production',
      entry: './src/index.js',
      output: {
        path: this._resolvePath('dist'),
        filename: '[name].bundle.js',
        library: {
          name: 'WebComponents',
          type: 'umd',
        },
        globalObject: 'this',
      },
      resolve: {
        extensions: ['.js', '.json'],
      },
      module: {
        rules: [
          {
            test: /\.js$/,
            exclude: /node_modules/,
            use: {
              loader: 'babel-loader',
              options: {
                presets: [
                  ['@babel/preset-env', { targets: this._config.target }],
                ],
              },
            },
          },
        ],
      },
      optimization: {
        minimize: this._config.minify,
        sourceMap: this._config.sourceMap,
      },
      devtool: this._config.sourceMap ? 'source-map' : false,
    };

    this._log('info', 'Webpack configuration initialized');
  }

  _initializeVite() {
    this._viteConfig = {
      build: {
        target: this._config.target,
        minify: this._config.minify ? 'terser' : false,
        sourcemap: this._config.sourceMap,
        lib: {
          entry: './src/index.js',
          name: 'WebComponents',
          formats: ['es', 'umd'],
          fileName: (format) => `web-components.${format}.js`,
        },
      },
      resolve: {
        extensions: ['.js', '.json'],
      },
    };

    this._log('info', 'Vite configuration initialized');
  }

  async _runBuild() {
    const status = this.shadowRoot.getElementById('status');
    const buildBtn = this.shadowRoot.getElementById('build-btn');
    
    status.className = 'build-status status-building';
    status.textContent = 'Building...';
    buildBtn.disabled = true;

    this._log('info', 'Starting build process...');
    this._log('info', `Using ${this._config.buildSystem}...`);

    try {
      if (this._config.buildSystem === 'webpack') {
        await this._runWebpackBuild();
      } else if (this._config.buildSystem === 'vite') {
        await this._runViteBuild();
      }

      status.className = 'build-status status-success';
      status.textContent = 'Build successful!';
      this._log('success', 'Build completed successfully');
    } catch (error) {
      status.className = 'build-status status-error';
      status.textContent = 'Build failed';
      this._log('error', `Build failed: ${error.message}`);
    } finally {
      buildBtn.disabled = false;
    }
  }

  async _runWebpackBuild() {
    this._log('info', 'Webpack: Compiling modules...');
    await this._simulateDelay(500);
    this._compilationResults.push({ type: 'module', count: 42 });

    this._log('info', 'Webpack: Generating bundle...');
    await this._simulateDelay(300);
    this._compilationResults.push({ type: 'chunk', count: 5 });

    this._log('info', 'Webpack: Optimizing output...');
    await this._simulateDelay(200);

    this._bundle = {
      size: Math.floor(Math.random() * 50000) + 10000,
      files: this._compilationResults.reduce((acc, r) => acc + r.count, 0),
    };

    this._log('info', `Bundle size: ${this._formatSize(this._bundle.size)}`);
  }

  async _runViteBuild() {
    this._log('info', 'Vite: Scanning dependencies...');
    await this._simulateDelay(400);
    this._compilationResults.push({ type: 'deps', count: 28 });

    this._log('info', 'Vite: Building modules...');
    await this._simulateDelay(350);
    this._compilationResults.push({ type: 'module', count: 42 });

    this._log('info', 'Vite: Creating chunk bundles...');
    await this._simulateDelay(250);

    this._bundle = {
      size: Math.floor(Math.random() * 45000) + 8000,
      files: this._compilationResults.reduce((acc, r) => acc + r.count, 0),
    };

    this._log('info', `Bundle size: ${this._formatSize(this._bundle.size)}`);
  }

  async _toggleWatchMode() {
    const watchBtn = this.shadowRoot.getElementById('watch-btn');
    const status = this.shadowRoot.getElementById('status');

    if (!this._config.watch) {
      this._config.watch = true;
      watchBtn.textContent = 'Stop Watch';
      status.className = 'build-status status-building';
      status.textContent = 'Watching for changes...';
      this._log('info', 'Watch mode enabled');
      
      this._startWatchLoop();
    } else {
      this._config.watch = false;
      watchBtn.textContent = 'Start Watch';
      status.className = 'build-status status-idle';
      status.textContent = 'Watch mode stopped';
      this._log('info', 'Watch mode disabled');
    }
  }

  _startWatchLoop() {
    let changeCount = 0;
    const interval = setInterval(() => {
      if (!this._config.watch) {
        clearInterval(interval);
        return;
      }

      changeCount++;
      this._log('info', `File change detected: src/component-${changeCount}.js`);
      
      const status = this.shadowRoot.getElementById('status');
      status.textContent = `Rebuilding (change #${changeCount})...`;

      setTimeout(() => {
        if (this._config.watch) {
          status.className = 'build-status status-success';
          status.textContent = 'Rebuild successful!';
        }
      }, 500);
    }, 3000);
  }

  _simulateDelay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  _resolvePath(relativePath) {
    return relativePath;
  }

  _formatSize(bytes) {
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(2)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(2)} MB`;
  }

  _log(type, message) {
    const logs = this.shadowRoot.getElementById('logs');
    if (logs) {
      const entry = document.createElement('div');
      entry.className = `log-entry log-${type}`;
      entry.textContent = `[${new Date().toLocaleTimeString()}] ${message}`;
      logs.appendChild(entry);
      logs.scrollTop = logs.scrollHeight;
    }
  }

  _cleanup() {
    this._config.watch = false;
    this._bundle = null;
    this._compilationResults = [];
  }

  get buildSystem() {
    return this._config.buildSystem;
  }

  set buildSystem(value) {
    this._config.buildSystem = value;
    this._initializeBuildSystem();
  }

  get config() {
    return { ...this._config };
  }

  set config(value) {
    if (value && typeof value === 'object') {
      Object.assign(this._config, value);
      this._initializeBuildSystem();
    }
  }
}

customElements.define('build-tool-integration', BuildToolIntegration);

export { BuildToolIntegration };