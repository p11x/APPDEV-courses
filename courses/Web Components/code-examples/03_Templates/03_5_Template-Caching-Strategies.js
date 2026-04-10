/**
 * Template Caching Strategies
 * @description Performance optimization through intelligent template caching and pre-compilation
 * @module templates/caching
 * @version 1.0.0
 * @example <template-cache-manager></template-cache-manager>
 */

// ============================================
// Template Cache Manager
// ============================================

class LRUCache {
  /**
   * Creates an LRU Cache instance
   * @param {number} maxSize - Maximum number of items to store
   */
  constructor(maxSize = 50) {
    this.maxSize = maxSize;
    this.cache = new Map();
    this.hits = 0;
    this.misses = 0;
  }

  /**
   * Gets a value from cache
   * @param {string} key - Cache key
   * @returns {*} Cached value or undefined
   */
  get(key) {
    if (!this.cache.has(key)) {
      this.misses++;
      return undefined;
    }
    const value = this.cache.get(key);
    this.cache.delete(key);
    this.cache.set(key, value);
    this.hits++;
    return value;
  }

  /**
   * Sets a value in cache
   * @param {string} key - Cache key
   * @param {*} value - Value to cache
   */
  set(key, value) {
    if (this.cache.has(key)) {
      this.cache.delete(key);
    } else if (this.cache.size >= this.maxSize) {
      const firstKey = this.cache.keys().next().value;
      this.cache.delete(firstKey);
    }
    this.cache.set(key, value);
  }

  /**
   * Checks if key exists in cache
   * @param {string} key - Cache key
   * @returns {boolean} True if key exists
   */
  has(key) {
    return this.cache.has(key);
  }

  /**
   * Removes a key from cache
   * @param {string} key - Cache key
   * @returns {boolean} True if key was removed
   */
  delete(key) {
    return this.cache.delete(key);
  }

  /**
   * Clears all cached items
   */
  clear() {
    this.cache.clear();
    this.hits = 0;
    this.misses = 0;
  }

  /**
   * Gets current cache size
   * @returns {number} Number of items in cache
   */
  size() {
    return this.cache.size;
  }

  /**
   * Gets cache statistics
   * @returns {Object} Hit/miss statistics
   */
  getStats() {
    const total = this.hits + this.misses;
    return {
      hits: this.hits,
      misses: this.misses,
      size: this.cache.size,
      maxSize: this.maxSize,
      hitRate: total > 0 ? (this.hits / total) * 100 : 0
    };
  }
}

class TemplateCacheManager extends HTMLElement {
  static get observedAttributes() {
    return ['max-cache-size', 'enable-precompilation', 'cache-timeout', 'max-memory-mb'];
  }

  static get template() {
    return `
      <style>
        :host {
          display: block;
          font-family: system-ui, -apple-system, sans-serif;
        }
        .cache-container {
          padding: 16px;
          background: #f5f5f5;
          border-radius: 8px;
        }
        .stats-panel {
          display: grid;
          grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
          gap: 12px;
          margin-bottom: 16px;
        }
        .stat-item {
          background: white;
          padding: 12px;
          border-radius: 6px;
          box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        .stat-label {
          font-size: 12px;
          color: #666;
          text-transform: uppercase;
        }
        .stat-value {
          font-size: 24px;
          font-weight: bold;
          color: #333;
        }
        .cache-list {
          background: white;
          border-radius: 6px;
          overflow: hidden;
        }
        .cache-entry {
          display: flex;
          justify-content: space-between;
          padding: 10px 12px;
          border-bottom: 1px solid #eee;
        }
        .cache-entry:last-child {
          border-bottom: none;
        }
        .cache-key {
          font-family: monospace;
          font-size: 13px;
          color: #333;
        }
        .cache-size {
          font-size: 12px;
          color: #888;
        }
        .controls {
          display: flex;
          gap: 8px;
          margin-top: 12px;
        }
        button {
          padding: 8px 16px;
          border: none;
          border-radius: 4px;
          cursor: pointer;
          font-size: 13px;
          transition: background 0.2s;
        }
        button.primary {
          background: #2196F3;
          color: white;
        }
        button.primary:hover {
          background: #1976D2;
        }
        button.secondary {
          background: #e0e0e0;
          color: #333;
        }
        button.secondary:hover {
          background: #bdbdbd;
        }
      </style>
      <div class="cache-container">
        <div class="stats-panel">
          <div class="stat-item">
            <div class="stat-label">Cache Size</div>
            <div class="stat-value" id="size">0</div>
          </div>
          <div class="stat-item">
            <div class="stat-label">Hit Rate</div>
            <div class="stat-value" id="hit-rate">0%</div>
          </div>
          <div class="stat-item">
            <div class="stat-label">Hits</div>
            <div class="stat-value" id="hits">0</div>
          </div>
          <div class="stat-item">
            <div class="stat-label">Memory</div>
            <div class="stat-value" id="memory">0KB</div>
          </div>
        </div>
        <div class="cache-list" id="cache-list"></div>
        <div class="controls">
          <button class="primary" id="clear-cache">Clear Cache</button>
          <button class="secondary" id="precompile">Pre-compile</button>
        </div>
      </div>
    `;
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._cache = null;
    this._compiledTemplates = new Map();
    this._attributeObserver = null;
    this._maxCacheSize = 50;
    this._enablePrecompilation = true;
    this._cacheTimeout = 300000;
    this._maxMemoryMB = 10;
    this._memoryUsage = 0;
    this._cacheTimestamps = new Map();
    this._precompileQueue = [];
    this._isPrecompiling = false;
    this._statsUpdateInterval = null;
  }

  connectedCallback() {
    this._parseAttributes();
    this._initCache();
    this._render();
    this._bindEvents();
    this._startStatsUpdate();
    this._processPrecompileQueue();
  }

  disconnectedCallback() {
    this._stopStatsUpdate();
    if (this._attributeObserver) {
      this._attributeObserver.disconnect();
    }
    this._clearCache();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    this._parseAttribute(name, newValue);
    this._updateUI();
  }

  _parseAttributes() {
    this._parseAttribute('max-cache-size', this.getAttribute('max-cache-size'));
    this._parseAttribute('enable-precompilation', this.getAttribute('enable-precompilation'));
    this._parseAttribute('cache-timeout', this.getAttribute('cache-timeout'));
    this._parseAttribute('max-memory-mb', this.getAttribute('max-memory-mb'));
  }

  _parseAttribute(name, value) {
    switch (name) {
      case 'max-cache-size':
        this._maxCacheSize = parseInt(value, 10) || 50;
        break;
      case 'enable-precompilation':
        this._enablePrecompilation = value !== 'false';
        break;
      case 'cache-timeout':
        this._cacheTimeout = parseInt(value, 10) || 300000;
        break;
      case 'max-memory-mb':
        this._maxMemoryMB = parseInt(value, 10) || 10;
        break;
    }
  }

  _initCache() {
    this._cache = new LRUCache(this._maxCacheSize);
    this._compileTemplate('default', '<slot></slot>');
  }

  _render() {
    const template = document.createElement('template');
    template.innerHTML = TemplateCacheManager.template;
    this.shadowRoot.appendChild(template.content.cloneNode(true));
    this._updateUI();
  }

  _bindEvents() {
    const clearBtn = this.shadowRoot.getElementById('clear-cache');
    const precompileBtn = this.shadowRoot.getElementById('precompile');

    clearBtn.addEventListener('click', () => this.clearCache());
    precompileBtn.addEventListener('click', () => this.precompileAll());
  }

  _startStatsUpdate() {
    this._statsUpdateInterval = setInterval(() => this._updateUI(), 1000);
  }

  _stopStatsUpdate() {
    if (this._statsUpdateInterval) {
      clearInterval(this._statsUpdateInterval);
      this._statsUpdateInterval = null;
    }
  }

  _updateUI() {
    if (!this.shadowRoot) return;

    const stats = this._cache.getStats();
    const sizeEl = this.shadowRoot.getElementById('size');
    const hitRateEl = this.shadowRoot.getElementById('hit-rate');
    const hitsEl = this.shadowRoot.getElementById('hits');
    const memoryEl = this.shadowRoot.getElementById('memory');
    const cacheListEl = this.shadowRoot.getElementById('cache-list');

    if (sizeEl) sizeEl.textContent = stats.size;
    if (hitRateEl) hitRateEl.textContent = `${stats.hitRate.toFixed(1)}%`;
    if (hitsEl) hitsEl.textContent = stats.hits;
    if (memoryEl) memoryEl.textContent = this._formatBytes(this._memoryUsage);

    if (cacheListEl) {
      cacheListEl.innerHTML = '';
      const entries = Array.from(this._cache.cache.entries()).slice(0, 10);
      entries.forEach(([key, value]) => {
        const entry = document.createElement('div');
        entry.className = 'cache-entry';
        entry.innerHTML = `
          <span class="cache-key">${this._escapeHtml(key)}</span>
          <span class="cache-size">${this._estimateTemplateSize(value)}</span>
        `;
        cacheListEl.appendChild(entry);
      });
    }
  }

  _escapeHtml(str) {
    const div = document.createElement('div');
    div.textContent = str;
    return div.innerHTML;
  }

  _formatBytes(bytes) {
    if (bytes < 1024) return `${bytes}B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)}KB`;
    return `${(bytes / (1024 * 1024)).toFixed(2)}MB`;
  }

  _estimateTemplateSize(template) {
    if (!template) return '0B';
    const size = new Blob([template]).size;
    return this._formatBytes(size);
  }

  _updateMemoryUsage(size) {
    this._memoryUsage += size;
    if (this._memoryUsage > this._maxMemoryMB * 1024 * 1024) {
      this._evictOldEntries();
    }
  }

  _evictOldEntries() {
    const entries = Array.from(this._cacheTimestamps.entries());
    entries.sort((a, b) => a[1] - b[1]);
    const toRemove = Math.floor(entries.length * 0.2);
    for (let i = 0; i < toRemove; i++) {
      this._cache.delete(entries[i][0]);
      this._cacheTimestamps.delete(entries[i][0]);
    }
    this._memoryUsage = this._calculateMemoryUsage();
  }

  _calculateMemoryUsage() {
    let total = 0;
    this._cache.cache.forEach((value) => {
      total += new Blob([value]).size;
    });
    return total;
  }

  getTemplate(key) {
    if (!key) {
      throw new Error('Template key is required');
    }

    const cached = this._cache.get(key);
    if (cached) {
      this._cacheTimestamps.set(key, Date.now());
      return cached;
    }

    return null;
  }

  /**
   * Gets a cached template by key
   * @param {string} key - Template identifier
   * @returns {string|null} Cached template or null
   */
  getCachedTemplate(key) {
    return this.getTemplate(key);
  }

  /**
   * Stores a template in cache
   * @param {string} key - Template identifier
   * @param {string} template - Template content
   * @returns {boolean} Success status
   */
  setTemplate(key, template) {
    if (!key || typeof key !== 'string') {
      console.error('Invalid key provided');
      return false;
    }

    if (!template || typeof template !== 'string') {
      console.error('Invalid template provided');
      return false;
    }

    try {
      const size = new Blob([template]).size;
      if (this._memoryUsage + size > this._maxMemoryMB * 1024 * 1024) {
        this._evictOldEntries();
      }

      this._cache.set(key, template);
      this._cacheTimestamps.set(key, Date.now());
      this._updateMemoryUsage(size);
      this._updateUI();
      return true;
    } catch (error) {
      console.error('Failed to cache template:', error);
      return false;
    }
  }

  _compileTemplate(key, template) {
    try {
      const compiled = this._compileTemplateContent(template);
      this._compiledTemplates.set(key, compiled);
      return compiled;
    } catch (error) {
      console.error('Template compilation failed:', error);
      return null;
    }
  }

  _compileTemplateContent(template) {
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = template;

    const scripts = tempDiv.querySelectorAll('script');
    scripts.forEach((script) => {
      script.remove();
    });

    const styles = tempDiv.querySelectorAll('style');
    const styleContent = Array.from(styles).map(s => s.textContent).join('\n');

    return {
      html: tempDiv.innerHTML,
      styles: styleContent,
      timestamp: Date.now()
    };
  }

  /**
   * Caches a template with pre-compilation
   * @param {string} key - Template identifier
   * @param {string} template - Template content
   * @param {boolean} precompile - Whether to pre-compile
   * @returns {boolean} Success status
   */
  cacheTemplate(key, template, precompile = true) {
    if (!key || !template) {
      return false;
    }

    this.setTemplate(key, template);

    if (precompile && this._enablePrecompilation) {
      this._compileTemplate(key, template);
    }

    return true;
  }

  /**
   * Clears all cached templates
   */
  clearCache() {
    this._cache.clear();
    this._compiledTemplates.clear();
    this._cacheTimestamps.clear();
    this._memoryUsage = 0;
    this._compileTemplate('default', '<slot></slot>');
    this._updateUI();
  }

  _clearCache() {
    this.clearCache();
  }

  /**
   * Pre-compiles all cached templates
   */
  precompileAll() {
    if (this._isPrecompiling) return;

    this._isPrecompiling = true;
    const keys = Array.from(this._cache.cache.keys());

    keys.forEach((key) => {
      const template = this._cache.get(key);
      if (template) {
        this._compileTemplate(key, template);
      }
    });

    this._isPrecompiling = false;
    this._updateUI();
  }

  /**
   * Queues a template for pre-compilation
   * @param {string} key - Template identifier
   * @param {string} template - Template content
   */
  queuePrecompile(key, template) {
    this._precompileQueue.push({ key, template });
    this._processPrecompileQueue();
  }

  async _processPrecompileQueue() {
    if (this._precompileQueue.length === 0) return;

    const item = this._precompileQueue.shift();
    await new Promise(resolve => setTimeout(resolve, 0));
    this._compileTemplate(item.key, item.template);
    this._processPrecompileQueue();
  }

  /**
   * Invalidates a specific template cache
   * @param {string} key - Template identifier
   * @returns {boolean} Success status
   */
  invalidate(key) {
    if (!key) {
      return false;
    }

    const result = this._cache.delete(key);
    this._compiledTemplates.delete(key);
    this._cacheTimestamps.delete(key);
    this._updateUI();
    return result;
  }

  /**
   * Checks if a template is cached
   * @param {string} key - Template identifier
   * @returns {boolean} True if cached
   */
  hasTemplate(key) {
    return this._cache.has(key);
  }

  /**
   * Gets cache statistics
   * @returns {Object} Cache statistics
   */
  getStats() {
    const basicStats = this._cache.getStats();
    return {
      ...basicStats,
      memoryUsage: this._memoryUsage,
      maxMemory: this._maxMemoryMB * 1024 * 1024,
      compiledCount: this._compiledTemplates.size,
      cacheTimeout: this._cacheTimeout,
      precompileEnabled: this._enablePrecompilation
    };
  }

  /**
   * Gets all cached template keys
   * @returns {string[]} Array of cache keys
   */
  getCachedKeys() {
    return Array.from(this._cache.cache.keys());
  }

  /**
   * Sets cache timeout
   * @param {number} timeout - Timeout in milliseconds
   */
  setCacheTimeout(timeout) {
    this._cacheTimeout = timeout;
    this._startCleanupInterval();
  }

  _startCleanupInterval() {
    setInterval(() => this._cleanupExpired(), this._cacheTimeout);
  }

  _cleanupExpired() {
    const now = Date.now();
    const keysToRemove = [];

    this._cacheTimestamps.forEach((timestamp, key) => {
      if (now - timestamp > this._cacheTimeout) {
        keysToRemove.push(key);
      }
    });

    keysToRemove.forEach(key => this.invalidate(key));
  }

  /**
   * Sets maximum cache size
   * @param {number} size - Maximum number of items
   */
  setMaxCacheSize(size) {
    this._maxCacheSize = size;
    this._cache = new LRUCache(size);
    this._updateUI();
  }

  /**
   * Sets maximum memory in MB
   * @param {number} mb - Maximum memory in megabytes
   */
  setMaxMemory(mb) {
    this._maxMemoryMB = mb;
    if (this._memoryUsage > mb * 1024 * 1024) {
      this._evictOldEntries();
    }
  }

  /**
   * Warms up cache with templates
   * @param {Object} templates - Key-value pairs of templates
   */
  warmUp(templates) {
    if (!templates || typeof templates !== 'object') {
      return;
    }

    Object.entries(templates).forEach(([key, template]) => {
      this.cacheTemplate(key, template, this._enablePrecompilation);
    });
  }

  /**
   * Batch gets multiple templates
   * @param {string[]} keys - Array of template keys
   * @returns {Object} Key-value pairs of templates
   */
  getMultiple(keys) {
    const result = {};
    keys.forEach(key => {
      const template = this.getTemplate(key);
      if (template) {
        result[key] = template;
      }
    });
    return result;
  }

  /**
   * Gets a compiled template
   * @param {string} key - Template identifier
   * @returns {Object|null} Compiled template or null
   */
  getCompiled(key) {
    return this._compiledTemplates.get(key) || null;
  }

  /**
   * Exports cache to JSON
   * @returns {string} JSON string of cache
   */
  exportCache() {
    const data = {
      templates: {},
      stats: this.getStats(),
      exportedAt: new Date().toISOString()
    };

    this._cache.cache.forEach((value, key) => {
      data.templates[key] = value;
    });

    return JSON.stringify(data);
  }

  /**
   * Imports cache from JSON
   * @param {string} json - JSON string
   * @returns {boolean} Success status
   */
  importCache(json) {
    try {
      const data = JSON.parse(json);
      if (data.templates && typeof data.templates === 'object') {
        Object.entries(data.templates).forEach(([key, template]) => {
          this.cacheTemplate(key, template, false);
        });
        return true;
      }
      return false;
    } catch (error) {
      console.error('Import failed:', error);
      return false;
    }
  }
}

// ============================================
// Cache Statistics
// ============================================

class CacheStatistics {
  /**
   * Creates a CacheStatistics instance
   * @param {TemplateCacheManager} cacheManager - Cache manager instance
   */
  constructor(cacheManager) {
    this.cacheManager = cacheManager;
    this.history = [];
    this.maxHistorySize = 100;
    this._startTracking();
  }

  _startTracking() {
    setInterval(() => this._record(), 5000);
  }

  _record() {
    const stats = this.cacheManager.getStats();
    const record = {
      timestamp: Date.now(),
      ...stats
    };

    this.history.push(record);
    if (this.history.length > this.maxHistorySize) {
      this.history.shift();
    }
  }

  /**
   * Gets current statistics
   * @returns {Object} Current statistics
   */
  getCurrent() {
    return this.cacheManager.getStats();
  }

  /**
   * Gets historical statistics
   * @param {number} duration - Duration in milliseconds
   * @returns {Object[]} Historical records
   */
  getHistory(duration = 60000) {
    const cutoff = Date.now() - duration;
    return this.history.filter(r => r.timestamp >= cutoff);
  }

  /**
   * Gets average hit rate
   * @returns {number} Average hit rate percentage
   */
  getAverageHitRate() {
    if (this.history.length === 0) return 0;
    const sum = this.history.reduce((acc, r) => acc + r.hitRate, 0);
    return sum / this.history.length;
  }

  /**
   * Gets peak memory usage
   * @returns {number} Peak memory in bytes
   */
  getPeakMemory() {
    if (this.history.length === 0) return 0;
    return Math.max(...this.history.map(r => r.memoryUsage));
  }

  /**
   * Gets total cache misses
   * @returns {number} Total misses
   */
  getTotalMisses() {
    if (this.history.length === 0) return 0;
    return this.history[this.history.length - 1].misses;
  }

  /**
   * Gets total cache hits
   * @returns {number} Total hits
   */
  getTotalHits() {
    if (this.history.length === 0) return 0;
    return this.history[this.history.length - 1].hits;
  }

  /**
   * Generates statistics report
   * @returns {Object} Statistics report
   */
  generateReport() {
    return {
      current: this.getCurrent(),
      averageHitRate: this.getAverageHitRate(),
      peakMemory: this.getPeakMemory(),
      totalHits: this.getTotalHits(),
      totalMisses: this.getTotalMisses(),
      historySize: this.history.length,
      generatedAt: new Date().toISOString()
    };
  }

  /**
   * Clears historical data
   */
  clearHistory() {
    this.history = [];
  }

  /**
   * Exports statistics to JSON
   * @returns {string} JSON string
   */
  export() {
    return JSON.stringify(this.generateReport(), null, 2);
  }

  /**
   * Gets performance score (0-100)
   * @returns {number} Performance score
   */
  getPerformanceScore() {
    const hitRate = this.getAverageHitRate();
    const memoryUsage = this.getCurrent().memoryUsage;
    const maxMemory = this.getCurrent().maxMemory;

    const hitRateScore = hitRate * 0.5;
    const memoryScore = Math.max(0, (1 - memoryUsage / maxMemory)) * 50;

    return Math.round(hitRateScore + memoryScore);
  }
}

// ============================================
// Export
// ============================================

export { TemplateCacheManager, CacheStatistics, LRUCache };