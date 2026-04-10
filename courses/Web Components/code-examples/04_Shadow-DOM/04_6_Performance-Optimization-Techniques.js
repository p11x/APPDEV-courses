/**
 * Performance Optimization Component - Techniques for Shadow DOM
 * @module shadow-dom/04_6_Performance-Optimization-Techniques
 * @version 1.0.0
 * @example <perf-chart></perf-chart>
 */

class PerformanceChart extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._data = [];
    this._cachedDimensions = null;
    this._animationFrameId = null;
    this._resizeObserver = null;
    this._dataObserver = null;
    this._renderQueue = null;
    this._isConnected = false;
    this._lazyElements = new Map();
  }

  static get observedAttributes() {
    return ['data-src', 'max-items', 'animation', 'theme'];
  }

  connectedCallback() {
    this._isConnected = true;
    this._initRenderQueue();
    this._initPerformanceMonitoring();
    this._render();
    this._setupResizeObserver();
    this._loadData();
  }

  disconnectedCallback() {
    this._isConnected = false;
    this._cleanupResources();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue && this._isConnected) {
      this._queueRender(() => this._handleAttributeChange(name, newValue));
    }
  }

  _initRenderQueue() {
    this._renderQueue = [];
    this._isProcessing = false;

    const processQueue = () => {
      if (this._renderQueue.length > 0 && !this._isProcessing) {
        this._isProcessing = true;
        const task = this._renderQueue.shift();
        task();
        requestAnimationFrame(() => {
          this._isProcessing = false;
          processQueue();
        });
      }
    };
    this._processQueue = processQueue;
  }

  _queueRender(task) {
    this._renderQueue.push(task);
    this._processQueue();
  }

  _initPerformanceMonitoring() {
    this._performance = {
      renderCount: 0,
      lastRenderTime: 0,
      totalRenderTime: 0,
      memoryUsage: 0
    };

    if (typeof PerformanceObserver !== 'undefined') {
      try {
        this._perfObserver = new PerformanceObserver(this._handlePerformanceEntry.bind(this));
        this._perfObserver.observe({ entryTypes: ['measure', 'paint'] });
      } catch (e) {
        console.warn('Performance observer not supported');
      }
    }
  }

  _handlePerformanceEntry(entry) {
    if (entry.entryType === 'measure') {
      console.log(`[PerfChart] ${entry.name}: ${entry.duration.toFixed(2)}ms`);
    }
  }

  _render() {
    const startTime = performance.now();
    const style = this._getStyles();
    const template = this._getTemplate();
    this.shadowRoot.innerHTML = `${style}${template}`;
    this._cacheElements();
    this._attachEventListeners();
    this._renderChart();

    const endTime = performance.now();
    this._updatePerformanceMetrics(endTime - startTime);
  }

  _getStyles() {
    return `
      <style>
        :host {
          display: block;
          --chart-bg: #fafafa;
          --chart-border: 1px solid #e0e0e0;
          --chart-bar-color: #4caf50;
          --chart-bar-hover: #45a049;
          --chart-text: #666;
          --chart-font: system-ui, -apple-system, sans-serif;
        }

        :host([hidden]) {
          display: none;
        }

        .chart-container {
          background: var(--chart-bg);
          border: var(--chart-border);
          border-radius: 8px;
          padding: 20px;
          font-family: var(--chart-font);
          min-height: 200px;
        }

        .chart-header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 16px;
        }

        .chart-title {
          font-size: 1rem;
          font-weight: 600;
          color: #333;
          margin: 0;
        }

        .perf-badge {
          font-size: 0.75rem;
          color: var(--chart-text);
          padding: 4px 8px;
          background: #f0f0f0;
          border-radius: 4px;
        }

        .chart-area {
          position: relative;
          height: 150px;
          display: flex;
          align-items: flex-end;
          gap: 8px;
          padding: 10px 0;
        }

        .chart-bar {
          flex: 1;
          background: var(--chart-bar-color);
          border-radius: 4px 4px 0 0;
          min-height: 4px;
          transition: height 0.3s ease, background 0.15s ease;
          cursor: pointer;
          position: relative;
        }

        .chart-bar:hover {
          background: var(--chart-bar-hover);
        }

        .chart-bar[data-animated="false"] {
          transition: none;
        }

        .chart-bar-label {
          position: absolute;
          bottom: -20px;
          left: 50%;
          transform: translateX(-50%);
          font-size: 0.625rem;
          color: var(--chart-text);
          white-space: nowrap;
        }

        .chart-tooltip {
          position: absolute;
          background: #333;
          color: white;
          padding: 8px 12px;
          border-radius: 4px;
          font-size: 0.75rem;
          pointer-events: none;
          opacity: 0;
          transition: opacity 0.15s ease;
          z-index: 10;
          white-space: nowrap;
        }

        .chart-tooltip.visible {
          opacity: 1;
        }

        .chart-tooltip::after {
          content: '';
          position: absolute;
          top: 100%;
          left: 50%;
          transform: translateX(-50%);
          border: 6px solid transparent;
          border-top-color: #333;
        }

        .chart-legend {
          display: flex;
          gap: 16px;
          margin-top: 24px;
          font-size: 0.75rem;
          color: var(--chart-text);
        }

        .legend-item {
          display: flex;
          align-items: center;
          gap: 6px;
        }

        .legend-color {
          width: 12px;
          height: 12px;
          border-radius: 2px;
        }

        .loading-skeleton {
          background: linear-gradient(90deg, #f0f0f0 25%, #e8e8e8 50%, #f0f0f0 75%);
          background-size: 200% 100%;
          animation: skeleton-loading 1.5s infinite;
        }

        @keyframes skeleton-loading {
          0% { background-position: 200% 0; }
          100% { background-position: -200% 0; }
        }

        .lazy-content {
          opacity: 0;
          transition: opacity 0.3s ease;
        }

        .lazy-content.loaded {
          opacity: 1;
        }
      </style>
    `;
  }

  _getTemplate() {
    return `
      <div class="chart-container">
        <div class="chart-header">
          <h3 class="chart-title">Performance Data</h3>
          <span class="perf-badge">Render: <span id="render-time">0</span>ms</span>
        </div>
        <div class="chart-area" id="chart-area"></div>
        <div class="chart-legend">
          <div class="legend-item">
            <span class="legend-color" style="background: var(--chart-bar-color)"></span>
            <span>Value</span>
          </div>
        </div>
        <div class="chart-tooltip" id="tooltip"></div>
      </div>
    `;
  }

  _cacheElements() {
    this._chartArea = this.shadowRoot.getElementById('chart-area');
    this._renderTimeDisplay = this.shadowRoot.getElementById('render-time');
    this._tooltip = this.shadowRoot.getElementById('tooltip');
  }

  _attachEventListeners() {
    this._chartArea.addEventListener('mouseenter', this._handleMouseEnter.bind(this));
    this._chartArea.addEventListener('mouseleave', this._handleMouseLeave.bind(this));
    this._chartArea.addEventListener('mousemove', this._handleMouseMove.bind(this));
  }

  _setupResizeObserver() {
    if (typeof ResizeObserver !== 'undefined') {
      this._resizeObserver = new ResizeObserver(this._handleResize.bind(this));
      this._resizeObserver.observe(this);
    }
  }

  _handleResize(entries) {
    const entry = entries[0];
    const newDimensions = {
      width: entry.contentRect.width,
      height: entry.contentRect.height
    };

    if (!this._cachedDimensions || 
        this._cachedDimensions.width !== newDimensions.width ||
        this._cachedDimensions.height !== newDimensions.height) {
      this._cachedDimensions = newDimensions;
      this._debounceRender(() => this._renderChart());
    }
  }

  _debounceRender(callback) {
    if (this._animationFrameId) {
      cancelAnimationFrame(this._animationFrameId);
    }
    this._animationFrameId = requestAnimationFrame(callback);
  }

  _loadData() {
    const dataSrc = this.getAttribute('data-src');
    if (dataSrc) {
      this._fetchData(dataSrc);
    } else {
      this._data = this._generateSampleData();
    }
  }

  async _fetchData(url) {
    try {
      this._showSkeleton();
      const response = await fetch(url);
      if (!response.ok) throw new Error('Failed to fetch data');
      const json = await response.json();
      this._data = json.data || json;
      this._queueRender(() => this._renderChart());
    } catch (error) {
      console.error('[PerfChart] Data load error:', error);
      this._data = this._generateSampleData();
      this._renderChart();
    }
  }

  _generateSampleData() {
    const labels = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    return labels.map(label => ({
      label,
      value: Math.floor(Math.random() * 100) + 10
    }));
  }

  _showSkeleton() {
    if (this._chartArea) {
      this._chartArea.innerHTML = '';
      for (let i = 0; i < 7; i++) {
        const bar = document.createElement('div');
        bar.className = 'chart-bar loading-skeleton';
        bar.style.height = `${Math.random() * 80 + 20}%`;
        this._chartArea.appendChild(bar);
      }
    }
  }

  _renderChart() {
    if (!this._chartArea || !this._data.length) return;

    const maxItems = parseInt(this.getAttribute('max-items')) || 7;
    const animate = this.getAttribute('animation') !== 'false';

    const displayData = this._data.slice(0, maxItems);
    const maxValue = Math.max(...displayData.map(d => d.value));

    this._chartArea.innerHTML = '';

    displayData.forEach((item, index) => {
      const bar = document.createElement('div');
      bar.className = 'chart-bar';
      bar.dataset.index = index;
      bar.dataset.value = item.value;
      bar.dataset.label = item.label;
      bar.dataset.animated = animate ? 'true' : 'false';

      const heightPercent = (item.value / maxValue) * 100;
      bar.style.height = animate ? '0%' : `${heightPercent}%`;

      const label = document.createElement('span');
      label.className = 'chart-bar-label';
      label.textContent = item.label;
      bar.appendChild(label);

      this._chartArea.appendChild(bar);

      if (animate) {
        requestAnimationFrame(() => {
          bar.style.height = `${heightPercent}%`;
        });
      }
    });
  }

  _handleMouseEnter(event) {
    const bar = event.target.closest('.chart-bar');
    if (bar) {
      this._showTooltip(bar);
    }
  }

  _handleMouseLeave() {
    this._hideTooltip();
  }

  _handleMouseMove(event) {
    const bar = event.target.closest('.chart-bar');
    if (bar) {
      this._updateTooltipPosition(event);
    }
  }

  _showTooltip(bar) {
    const label = bar.dataset.label;
    const value = bar.dataset.value;
    this._tooltip.textContent = `${label}: ${value}`;
    this._tooltip.classList.add('visible');
  }

  _hideTooltip() {
    this._tooltip.classList.remove('visible');
  }

  _updateTooltipPosition(event) {
    const rect = this._chartArea.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;

    this._tooltip.style.left = `${x}px`;
    this._tooltip.style.top = `${y - 40}px`;
  }

  _updatePerformanceMetrics(renderTime) {
    this._performance.renderCount++;
    this._performance.lastRenderTime = renderTime;
    this._performance.totalRenderTime += renderTime;

    const avgTime = this._performance.totalRenderTime / this._performance.renderCount;

    if (this._renderTimeDisplay) {
      this._renderTimeDisplay.textContent = renderTime.toFixed(1);
    }

    if (renderTime > 16.67) {
      console.warn(`[PerfChart] Frame dropped: ${renderTime.toFixed(2)}ms (> 16.67ms)`);
    }
  }

  _handleAttributeChange(name, value) {
    switch (name) {
      case 'data-src':
        this._loadData();
        break;
      case 'max-items':
      case 'animation':
        this._renderChart();
        break;
      case 'theme':
        this._applyTheme(value);
        break;
    }
  }

  _applyTheme(theme) {
    const styles = this.style || {};
    switch (theme) {
      case 'dark':
        this.style.setProperty('--chart-bg', '#1a1a1a');
        this.style.setProperty('--chart-border', '#333');
        this.style.setProperty('--chart-text', '#aaa');
        break;
      case 'light':
      default:
        this.style.setProperty('--chart-bg', '#fafafa');
        this.style.setProperty('--chart-border', '#e0e0e0');
        this.style.setProperty('--chart-text', '#666');
    }
  }

  setData(data) {
    this._data = data;
    this._queueRender(() => this._renderChart());
  }

  addDataPoint(point) {
    this._data.push(point);
    if (this._data.length > 50) {
      this._data.shift();
    }
    this._queueRender(() => this._renderChart());
  }

  _cleanupResources() {
    if (this._animationFrameId) {
      cancelAnimationFrame(this._animationFrameId);
    }
    if (this._resizeObserver) {
      this._resizeObserver.disconnect();
    }
    if (this._perfObserver) {
      this._perfObserver.disconnect();
    }
    this._lazyElements.clear();
    this._renderQueue = [];
  }

  getPerformanceMetrics() {
    return {
      ...this._performance,
      averageRenderTime: this._performance.totalRenderTime / this._performance.renderCount
    };
  }
}

customElements.define('perf-chart', PerformanceChart);

export { PerformanceChart };