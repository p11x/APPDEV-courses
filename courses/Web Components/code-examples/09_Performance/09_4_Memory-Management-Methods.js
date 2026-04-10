/**
 * Memory Management Methods - Memory patterns for web components
 * @module performance/09_4_Memory-Management-Methods
 * @version 1.0.0
 * @example <memory-manager></memory-manager>
 */

class MemoryManager extends HTMLElement {
  constructor() {
    super();
    this._cache = new LRUCache(50);
    this._eventListeners = [];
    this._subscriptions = [];
    this._observers = [];
    this._buffers = [];
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this._render();
    this._startMemoryMonitoring();
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
          font-family: system-ui, sans-serif;
        }
        .memory-bar {
          height: 24px;
          background: #e0e0e0;
          border-radius: 12px;
          overflow: hidden;
          margin: 8px 0;
        }
        .memory-bar-fill {
          height: 100%;
          transition: width 0.3s, background-color 0.3s;
          border-radius: 12px;
        }
        .memory-bar-fill.normal {
          background: #4caf50;
        }
        .memory-bar-fill.warning {
          background: #ff9800;
        }
        .memory-bar-fill.critical {
          background: #f44336;
        }
        .stats-grid {
          display: grid;
          grid-template-columns: repeat(2, 1fr);
          gap: 12px;
          margin-top: 16px;
        }
        .stat-card {
          padding: 12px;
          background: #f5f5f5;
          border-radius: 8px;
        }
        .stat-value {
          font-size: 20px;
          font-weight: bold;
        }
        .stat-label {
          font-size: 12px;
          color: #666;
        }
        button {
          padding: 8px 16px;
          margin: 4px;
          border: 1px solid #ccc;
          border-radius: 4px;
          background: white;
          cursor: pointer;
        }
        button:hover {
          background: #f0f0f0;
        }
      </style>
      <div class="container">
        <h3>Memory Management</h3>
        <div class="memory-bar">
          <div class="memory-bar-fill normal" id="memory-fill" style="width: 0%"></div>
        </div>
        <div id="memory-text">0 / 0 MB</div>
        <div class="controls" style="margin-top: 12px;">
          <button id="force-gc">Force GC</button>
          <button id="clear-cache">Clear Cache</button>
          <button id="dump-memory">Dump Info</button>
        </div>
        <div class="stats-grid">
          <div class="stat-card">
            <div class="stat-value" id="cache-size">0</div>
            <div class="stat-label">Cache Items</div>
          </div>
          <div class="stat-card">
            <div class="stat-value" id="listener-count">0</div>
            <div class="stat-label">Event Listeners</div>
          </div>
          <div class="stat-card">
            <div class="stat-value" id="observer-count">0</div>
            <div class="stat-label">Observers</div>
          </div>
          <div class="stat-card">
            <div class="stat-value" id="buffer-count">0</div>
            <div class="stat-label">Buffers</div>
          </div>
        </div>
      </div>
    `;
  }

  _startMemoryMonitoring() {
    this._setupControls();
    this._updateMemoryDisplay();
    this._memoryInterval = setInterval(() => this._updateMemoryDisplay(), 1000);
  }

  _setupControls() {
    this.shadowRoot.getElementById('force-gc')?.addEventListener('click', () => {
      if (window.gc) window.gc();
    });

    this.shadowRoot.getElementById('clear-cache')?.addEventListener('click', () => {
      this._cache.clear();
      this._updateMemoryDisplay();
    });

    this.shadowRoot.getElementById('dump-memory')?.addEventListener('click', () => {
      console.log(this.getMemoryInfo());
    });
  }

  _updateMemoryDisplay() {
    const performance = window.performance || { memory: null };
    const memory = performance.memory;

    if (!memory) return;

    const usedMB = memory.usedJSHeapSize / 1048576;
    const totalMB = memory.jsHeapSizeLimit / 1048576;
    const percentage = (used MB / total MB) * 100;

    const fill = this.shadowRoot.getElementById('memory-fill');
    const text = this.shadowRoot.getElementById('memory-text');

    if (fill) {
      fill.style.width = `${Math.min(percentage, 100)}%`;
      fill.className = 'memory-bar-fill';
      if (percentage > 90) {
        fill.classList.add('critical');
      } else if (percentage > 70) {
        fill.classList.add('warning');
      } else {
        fill.classList.add('normal');
      }
    }

    if (text) {
      text.textContent = `${usedMB.toFixed(1)} / ${totalMB.toFixed(1)} MB (${percentage.toFixed(1)}%)`;
    }

    this.shadowRoot.getElementById('cache-size').textContent = this._cache.size();
    this.shadowRoot.getElementById('listener-count').textContent = this._eventListeners.length;
    this.shadowRoot.getElementById('observer-count').textContent = this._observers.length;
    this.shadowRoot.getElementById('buffer-count').textContent = this._buffers.length;
  }

  cache(key, value, ttl = 300000) {
    this._cache.set(key, { value, expires: Date.now() + ttl });
  }

  getCached(key) {
    const item = this._cache.get(key);
    if (!item) return null;

    if (Date.now() > item.expires) {
      this._cache.delete(key);
      return null;
    }

    return item.value;
  }

  registerEventListener(target, type, listener, options) {
    target.addEventListener(type, listener, options);
    this._eventListeners.push({ target, type, listener, options });
  }

  cleanupEventListeners() {
    this._eventListeners.forEach(({ target, type, listener, options }) => {
      target.removeEventListener(type, listener, options);
    });
    this._eventListeners = [];
  }

  registerObserver(observer) {
    this._observers.push(observer);
  }

  cleanupObservers() {
    this._observers.forEach(observer => observer.disconnect());
    this._observers = [];
  }

  allocateBuffer(size, type = 'Float32') {
    const buffer = new Float32Array(size);
    this._buffers.push(buffer);
    return buffer;
  }

  cleanupBuffers() {
    this._buffers = [];
  }

  getMemoryInfo() {
    const performance = window.performance || { memory: null };
    const memory = performance.memory || {};

    return {
      usedJSHeapSize: memory.usedJSHeapSize || 0,
      totalJSHeapSize: memory.totalJSHeapSize || 0,
      jsHeapSizeLimit: memory.jsHeapSizeLimit || 0,
      cacheSize: this._cache.size(),
      eventListeners: this._eventListeners.length,
      observers: this._observers.length,
      buffers: this._buffers.length
    };
  }

  disconnectCallback() {
    if (this._memoryInterval) {
      clearInterval(this._memoryInterval);
    }
    this.cleanupEventListeners();
    this.cleanupObservers();
    this.cleanupBuffers();
    this._cache.clear();
  }
}

class LRUCache {
  constructor(capacity) {
    this.capacity = capacity;
    this.cache = new Map();
  }

  get(key) {
    if (!this.cache.has(key)) return null;

    const value = this.cache.get(key);
    this.cache.delete(key);
    this.cache.set(key, value);
    return value;
  }

  set(key, value) {
    if (this.cache.has(key)) {
      this.cache.delete(key);
    } else if (this.cache.size >= this.capacity) {
      const firstKey = this.cache.keys().next().value;
      this.cache.delete(firstKey);
    }
    this.cache.set(key, value);
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
}

export { MemoryManager, LRUCache };