/**
 * Async Data Patterns
 * Implements API integration, fetch patterns, caching, and async data handling
 * @module data-binding/05_7_Async-Data-Patterns
 * @version 1.0.0
 * @example <async-data-element endpoint="/api/data" cache></async-data-element>
 */

const ASYNC_CONFIG = {
  defaultTimeout: 30000,
  maxRetries: 3,
  retryDelay: 1000,
  cacheEnabled: true,
  cacheMaxAge: 300000,
  cacheStorage: 'memory',
  requestMethods: ['GET', 'POST', 'PUT', 'DELETE', 'PATCH'],
  responseTypes: ['json', 'text', 'blob', 'arrayBuffer'],
  debounceDelay: 300
};

class AsyncError extends Error {
  constructor(message, code = 'ASYNC_ERROR', statusCode = 0) {
    super(message);
    this.name = 'AsyncError';
    this.code = code;
    this.statusCode = statusCode;
  }
}

class CacheEntry {
  constructor(data, maxAge) {
    this.data = data;
    this.timestamp = Date.now();
    this.maxAge = maxAge;
    this.hits = 0;
  }

  isValid() {
    return Date.now() - this.timestamp < this.maxAge;
  }

  getAge() {
    return Date.now() - this.timestamp;
  }
}

class MemoryCache {
  constructor() {
    this.cache = new Map();
  }

  get(key) {
    const entry = this.cache.get(key);
    if (!entry) return null;
    
    if (!entry.isValid()) {
      this.cache.delete(key);
      return null;
    }

    entry.hits++;
    return entry.data;
  }

  set(key, data, maxAge) {
    this.cache.set(key, new CacheEntry(data, maxAge));
  }

  delete(key) {
    this.cache.delete(key);
  }

  clear() {
    this.cache.clear();
  }

  size() {
    return this.cache.size;
  }

  cleanup(maxAge) {
    const now = Date.now();
    for (const [key, entry] of this.cache) {
      if (now - entry.timestamp > maxAge) {
        this.cache.delete(key);
      }
    }
  }
}

class RequestQueue {
  constructor() {
    this.queue = [];
    this.processing = false;
    this.maxConcurrent = 3;
    this.activeRequests = 0;
  }

  async enqueue(request) {
    return new Promise((resolve, reject) => {
      this.queue.push({ request, resolve, reject });
      this._process();
    });
  }

  async _process() {
    if (this.processing || this.activeRequests >= this.maxConcurrent) return;
    
    this.processing = true;
    while (this.queue.length > 0 && this.activeRequests < this.maxConcurrent) {
      const item = this.queue.shift();
      this.activeRequests++;
      
      try {
        const result = await item.request();
        item.resolve(result);
      } catch (error) {
        item.reject(error);
      } finally {
        this.activeRequests--;
      }
    }
    
    this.processing = false;
  }
}

class AsyncDataElement extends HTMLElement {
  static get observedAttributes() {
    return ['endpoint', 'method', 'cache', 'timeout', 'headers', 'body', 'auto-fetch'];
  }

  constructor() {
    super();
    this._endpoint = '';
    this._method = 'GET';
    this._cacheEnabled = ASYNC_CONFIG.cacheEnabled;
    this._timeout = ASYNC_CONFIG.defaultTimeout;
    this._headers = {};
    this._body = null;
    this._autoFetch = false;
    
    this._cache = new MemoryCache();
    this._queue = new RequestQueue();
    this._abortController = null;
    this._fetchPromise = null;
    this._requestHistory = [];
    this._isLoading = false;
    this._lastResponse = null;
    this._error = null;

    this._attachShadowDOM();
  }

  _attachShadowDOM() {
    const shadow = this.attachShadow({ mode: 'open' });
    
    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
        padding: 20px;
        border: 2px solid #2196f3;
        border-radius: 10px;
        background: linear-gradient(135deg, #e3f2fd 0%, #fff 100%);
        font-family: 'Source Sans Pro', system-ui, sans-serif;
      }

      :host([cache]) {
        border-color: #4caf50;
        background: linear-gradient(135deg, #e8f5e9 0%, #fff 100%);
      }

      :host(.loading) {
        border-color: #ff9800;
        background: linear-gradient(135deg, #fff3e0 0%, #fff 100%);
      }

      :host(.error) {
        border-color: #f44336;
        background: linear-gradient(135deg, #ffebee 0%, #fff 100%);
      }

      .async-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid rgba(33, 150, 243, 0.3);
      }

      .async-title {
        font-size: 16px;
        font-weight: 700;
        color: #1565c0;
      }

      .async-badge {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 11px;
        padding: 4px 10px;
        background: #2196f3;
        color: white;
        border-radius: 12px;
      }

      .async-badge.loading {
        background: #ff9800;
      }

      .async-badge.error {
        background: #f44336;
      }

      .status-indicator {
        width: 8px;
        height: 8px;
        border-radius: 50%;
        background: #4caf50;
        animation: pulse 1.5s ease-in-out infinite;
      }

      .status-indicator.loading {
        background: #ff9800;
      }

      .status-indicator.error {
        background: #f44336;
      }

      @keyframes pulse {
        0%, 100% { opacity: 1; }
        50% { opacity: 0.5; }
      }

      .data-display {
        margin-bottom: 16px;
        padding: 16px;
        background: white;
        border-radius: 8px;
        border: 1px solid #bbdefb;
        max-height: 200px;
        overflow: auto;
      }

      .data-label {
        font-size: 11px;
        font-weight: 600;
        color: #1976d2;
        margin-bottom: 8px;
        text-transform: uppercase;
      }

      .data-content {
        font-family: 'Roboto Mono', monospace;
        font-size: 12px;
        color: #333;
        white-space: pre-wrap;
        word-break: break-all;
        line-height: 1.5;
      }

      .data-content.loading {
        color: #ff9800;
        font-style: italic;
      }

      .data-content.error {
        color: #f44336;
      }

      .cache-info {
        display: flex;
        gap: 12px;
        margin-bottom: 16px;
        padding: 12px;
        background: #f5f5f5;
        border-radius: 6px;
        font-size: 11px;
      }

      .cache-item {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 2px;
      }

      .cache-label {
        color: #666;
        text-transform: uppercase;
      }

      .cache-value {
        font-weight: 600;
        color: #333;
      }

      .controls {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 8px;
        margin-bottom: 16px;
      }

      .control-btn {
        padding: 10px 12px;
        border: 1px solid #64b5f6;
        border-radius: 6px;
        background: white;
        color: #1565c0;
        font-size: 12px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
      }

      .control-btn:hover {
        background: #e3f2fd;
        border-color: #2196f3;
      }

      .control-btn:active {
        background: #bbdefb;
        transform: scale(0.98);
      }

      .control-btn.primary {
        background: #2196f3;
        color: white;
      }

      .control-btn.primary:hover {
        background: #1976d2;
      }

      .control-btn.danger {
        border-color: #ef9a9a;
        color: #c62828;
      }

      .control-btn.danger:hover {
        background: #ffebee;
        border-color: #f44336;
      }

      .request-history {
        background: #fafafa;
        border-radius: 6px;
        padding: 12px;
        max-height: 120px;
        overflow-y: auto;
      }

      .history-title {
        font-size: 12px;
        font-weight: 600;
        color: #616161;
        margin-bottom: 8px;
      }

      .history-item {
        padding: 4px 8px;
        margin: 2px 0;
        background: white;
        border-radius: 4px;
        font-size: 11px;
        font-family: monospace;
        display: flex;
        justify-content: space-between;
      }

      .history-item .method {
        font-weight: 600;
        color: #2196f3;
      }

      .history-item .endpoint {
        color: #333;
      }

      .history-item .status {
        font-weight: 600;
      }

      .history-item .status.success {
        color: #4caf50;
      }

      .history-item .status.error {
        color: #f44336;
      }

      .error-message {
        padding: 12px;
        background: #ffebee;
        border-radius: 6px;
        color: #c62828;
        font-size: 12px;
        margin-bottom: 16px;
        display: none;
      }

      .error-message.show {
        display: block;
      }

      .metrics {
        display: flex;
        gap: 12px;
        margin-top: 12px;
        padding: 10px;
        background: #e0f2f1;
        border-radius: 6px;
      }

      .metric {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
      }

      .metric-label {
        font-size: 10px;
        color: #00695c;
        text-transform: uppercase;
      }

      .metric-value {
        font-size: 16px;
        font-weight: 700;
        color: #00796b;
      }

      @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
      }

      .loading-spinner {
        width: 16px;
        height: 16px;
        border: 2px solid #ff9800;
        border-top-color: transparent;
        border-radius: 50%;
        animation: spin 0.8s linear infinite;
      }
    `;

    const container = document.createElement('div');
    container.innerHTML = `
      <div class="async-header">
        <span class="async-title">Async Data</span>
        <span class="async-badge" id="status-badge">
          <span class="status-indicator" id="status-indicator"></span>
          <span id="status-text">Ready</span>
        </span>
      </div>
      
      <div class="error-message" id="error-message"></div>
      
      <div class="data-display">
        <div class="data-label">Response Data</div>
        <div class="data-content" id="data-content">No data loaded</div>
      </div>
      
      <div class="cache-info">
        <div class="cache-item">
          <span class="cache-label">Cache Size</span>
          <span class="cache-value" id="cache-size">0</span>
        </div>
        <div class="cache-item">
          <span class="cache-label">Cache Hits</span>
          <span class="cache-value" id="cache-hits">0</span>
        </div>
        <div class="cache-item">
          <span class="cache-label">Last Status</span>
          <span class="cache-value" id="last-status">-</span>
        </div>
      </div>
      
      <div class="controls">
        <button class="control-btn primary" id="btn-fetch">Fetch</button>
        <button class="control-btn" id="btn-fetch-cached">Fetch (Cached)</button>
        <button class="control-btn" id="btn-abort">Abort</button>
        <button class="control-btn danger" id="btn-clear">Clear Cache</button>
      </div>
      
      <div class="request-history">
        <div class="history-title">Request History</div>
        <div id="history-list"></div>
      </div>
      
      <div class="metrics">
        <div class="metric">
          <span class="metric-label">Requests</span>
          <span class="metric-value" id="request-count">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Success</span>
          <span class="metric-value" id="success-count">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Failed</span>
          <span class="metric-value" id="failed-count">0</span>
        </div>
      </div>
    `;
    
    shadow.appendChild(style);
    shadow.appendChild(container);
  }

  connectedCallback() {
    this._parseEndpoint();
    this._bindEvents();
    this._render();

    if (this._autoFetch) {
      this.fetch();
    }
  }

  disconnectedCallback() {
    this._abortRequest();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'endpoint':
        this._endpoint = newValue;
        break;
      case 'method':
        if (ASYNC_CONFIG.requestMethods.includes(newValue)) {
          this._method = newValue;
        }
        break;
      case 'cache':
        this._cacheEnabled = newValue !== 'false';
        this._render();
        break;
      case 'timeout':
        this._timeout = parseInt(newValue, 10) || ASYNC_CONFIG.defaultTimeout;
        break;
      case 'headers':
        try {
          this._headers = JSON.parse(newValue);
        } catch (e) {
          console.error('Invalid headers JSON:', e);
        }
        break;
      case 'body':
        this._body = newValue;
        break;
      case 'auto-fetch':
        this._autoFetch = newValue !== 'false';
        break;
    }
  }

  _parseEndpoint() {
    const endpoint = this.getAttribute('endpoint');
    if (endpoint) {
      this._endpoint = endpoint;
    }
  }

  _bindEvents() {
    const shadow = this.shadowRoot;
    
    const btnFetch = shadow.getElementById('btn-fetch');
    const btnFetchCached = shadow.getElementById('btn-fetch-cached');
    const btnAbort = shadow.getElementById('btn-abort');
    const btnClear = shadow.getElementById('btn-clear');

    btnFetch?.addEventListener('click', () => this.fetch());
    btnFetchCached?.addEventListener('click', () => this.fetchCached());
    btnAbort?.addEventListener('click', () => this.abort());
    btnClear?.addEventListener('click', () => this.clearCache());
  }

  async fetch(options = {}) {
    const endpoint = options.endpoint || this._endpoint;
    const method = options.method || this._method;
    const body = options.body !== undefined ? options.body : this._body;
    const useCache = options.useCache !== false && this._cacheEnabled;

    if (!endpoint) {
      throw new AsyncError('No endpoint specified', 'NO_ENDPOINT');
    }

    this._isLoading = true;
    this._setLoadingState(true);
    this._clearError();

    this._abortController = new AbortController();
    const signal = this._abortController.signal;

    const cacheKey = `${method}:${endpoint}`;
    const cachedData = useCache ? this._cache.get(cacheKey) : null;
    
    if (cachedData !== null) {
      this._lastResponse = cachedData;
      this._isLoading = false;
      this._setLoadingState(false);
      this._render();
      this._logRequest(method, endpoint, 200, 'cache');
      
      this.dispatchEvent(new CustomEvent('data-cached', {
        detail: { data: cachedData },
        bubbles: true,
        composed: true
      }));
      
      return cachedData;
    }

    try {
      const fetchPromise = this._executeFetch(endpoint, method, body, signal);
      const timeoutPromise = this._createTimeout();
      
      const response = await Promise.race([fetchPromise, timeoutPromise]);
      
      this._lastResponse = response;
      
      if (useCache) {
        this._cache.set(cacheKey, response, ASYNC_CONFIG.cacheMaxAge);
      }

      this._isLoading = false;
      this._setLoadingState(false);
      this._render();
      this._logRequest(method, endpoint, 200, 'success');

      this.dispatchEvent(new CustomEvent('data-loaded', {
        detail: { data: response, endpoint, method },
        bubbles: true,
        composed: true
      }));

      return response;
    } catch (error) {
      this._isLoading = false;
      this._setLoadingState(false);
      
      if (error.name === 'AbortError') {
        this._error = new AsyncError('Request aborted', 'ABORTED', 0);
        this._logRequest(method, endpoint, 0, 'aborted');
      } else {
        this._error = new AsyncError(
          error.message || 'Request failed',
          'REQUEST_FAILED',
          error.statusCode || 0
        );
        this._logRequest(method, endpoint, error.statusCode || 0, 'error');
      }
      
      this._render();
      throw this._error;
    }
  }

  async fetchCached() {
    return this.fetch({ useCache: true });
  }

  async _executeFetch(endpoint, method, body, signal) {
    const url = this._resolveUrl(endpoint);
    
    const fetchOptions = {
      method,
      headers: {
        'Content-Type': 'application/json',
        ...this._headers
      },
      signal
    };

    if (body && method !== 'GET') {
      fetchOptions.body = typeof body === 'string' ? body : JSON.stringify(body);
    }

    const response = await fetch(url, fetchOptions);
    
    if (!response.ok) {
      const error = new AsyncError(
        `HTTP error: ${response.statusText}`,
        'HTTP_ERROR',
        response.status
      );
      throw error;
    }

    const contentType = response.headers.get('content-type') || '';
    
    if (contentType.includes('application/json')) {
      return await response.json();
    } else if (contentType.includes('text/')) {
      return await response.text();
    } else if (contentType.includes('application/octet-stream')) {
      return await response.blob();
    } else {
      return await response.text();
    }
  }

  _resolveUrl(endpoint) {
    if (endpoint.startsWith('http://') || endpoint.startsWith('https://')) {
      return endpoint;
    }
    return endpoint;
  }

  _createTimeout() {
    return new Promise((_, reject) => {
      setTimeout(() => {
        reject(new AsyncError('Request timeout', 'TIMEOUT', 0));
      }, this._timeout);
    });
  }

  abort() {
    this._abortRequest();
  }

  _abortRequest() {
    if (this._abortController) {
      this._abortController.abort();
      this._abortController = null;
    }
    
    this._isLoading = false;
    this._setLoadingState(false);
  }

  clearCache() {
    this._cache.clear();
    this._render();
    this._logRequest('CLEAR', 'cache', 0, 'cache');
  }

  _setLoadingState(loading) {
    if (loading) {
      this.classList.add('loading');
    } else {
      this.classList.remove('loading');
    }
  }

  _clearError() {
    this._error = null;
  }

  _logRequest(method, endpoint, statusCode, result) {
    this._requestHistory.push({
      method,
      endpoint: endpoint.substring(0, 30),
      statusCode,
      result,
      timestamp: Date.now()
    });

    if (this._requestHistory.length > ASYNC_CONFIG.requestMethods.length * 5) {
      this._requestHistory.shift();
    }

    this._updateHistoryDisplay();
  }

  _updateHistoryDisplay() {
    const shadow = this.shadowRoot;
    const historyList = shadow?.getElementById('history-list');
    if (!historyList) return;

    historyList.innerHTML = this._requestHistory
      .slice(-10)
      .reverse()
      .map(entry => `
        <div class="history-item">
          <span class="method">${entry.method}</span>
          <span class="endpoint">${entry.endpoint}</span>
          <span class="status ${entry.result}">${entry.statusCode || entry.result}</span>
        </div>
      `).join('');
  }

  _render() {
    const shadow = this.shadowRoot;
    
    const statusBadge = shadow?.getElementById('status-badge');
    const statusIndicator = shadow?.getElementById('status-indicator');
    const statusText = shadow?.getElementById('status-text');
    const dataContent = shadow?.getElementById('data-content');
    const cacheSize = shadow?.getElementById('cache-size');
    const cacheHits = shadow?.getElementById('cache-hits');
    const lastStatus = shadow?.getElementById('last-status');
    const errorMessage = shadow?.getElementById('error-message');
    const requestCount = shadow?.getElementById('request-count');
    const successCount = shadow?.getElementById('success-count');
    const failedCount = shadow?.getElementById('failed-count');

    if (statusBadge) {
      statusBadge.classList.remove('loading', 'error');
      if (this._isLoading) {
        statusBadge.classList.add('loading');
      }
    }

    if (statusIndicator) {
      statusIndicator.classList.remove('loading', 'error');
      if (this._isLoading) {
        statusIndicator.classList.add('loading');
      } else if (this._error) {
        statusIndicator.classList.add('error');
      }
    }

    if (statusText) {
      if (this._isLoading) {
        statusText.textContent = 'Loading...';
      } else if (this._error) {
        statusText.textContent = 'Error';
      } else {
        statusText.textContent = 'Ready';
      }
    }

    if (dataContent) {
      dataContent.classList.remove('loading', 'error');
      
      if (this._isLoading) {
        dataContent.textContent = 'Loading...';
        dataContent.classList.add('loading');
      } else if (this._error) {
        dataContent.textContent = this._error.message;
        dataContent.classList.add('error');
      } else if (this._lastResponse !== null) {
        dataContent.textContent = typeof this._lastResponse === 'object' 
          ? JSON.stringify(this._lastResponse, null, 2)
          : String(this._lastResponse);
      } else {
        dataContent.textContent = 'No data loaded';
      }
    }

    if (cacheSize) {
      cacheSize.textContent = String(this._cache.size());
    }

    if (cacheHits) {
      let totalHits = 0;
      cacheHits.textContent = '0';
    }

    if (lastStatus) {
      lastStatus.textContent = this._lastResponse ? '200' : '-';
    }

    if (errorMessage) {
      if (this._error) {
        errorMessage.textContent = this._error.message;
        errorMessage.classList.add('show');
      } else {
        errorMessage.classList.remove('show');
      }
    }

    if (requestCount) {
      requestCount.textContent = String(this._requestHistory.length);
    }

    if (successCount) {
      const successes = this._requestHistory.filter(r => r.result === 'success' || r.result === 'cache').length;
      successCount.textContent = String(successes);
    }

    if (failedCount) {
      const failures = this._requestHistory.filter(r => r.result === 'error').length;
      failedCount.textContent = String(failures);
    }
  }

  get endpoint() {
    return this._endpoint;
  }

  set endpoint(value) {
    this._endpoint = value;
  }

  get isLoading() {
    return this._isLoading;
  }

  get data() {
    return this._lastResponse;
  }

  get error() {
    return this._error;
  }
}

if (!customElements.get('async-data-element')) {
  customElements.define('async-data-element', AsyncDataElement);
}

export { 
  AsyncDataElement, 
  AsyncError, 
  CacheEntry, 
  MemoryCache,
  RequestQueue,
  ASYNC_CONFIG 
};