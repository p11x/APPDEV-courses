/**
 * Performance Monitoring Tools - Analytics for web components
 * @module performance/09_5_Performance-Monitoring-Tools
 * @version 1.0.0
 * @example <performance-monitor></performance-monitor>
 */

class PerformanceMonitor extends HTMLElement {
  constructor() {
    super();
    this._metrics = new Map();
    this._markers = [];
    this._measures = [];
    this._observers = [];
    this._sessionId = this._generateSessionId();
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this._render();
    this._setupPerformanceObserver();
    this._startReporting();
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
          font-family: system-ui, sans-serif;
        }
        .metric-row {
          display: flex;
          justify-content: space-between;
          padding: 8px 12px;
          margin: 4px 0;
          background: #f5f5f5;
          border-radius: 4px;
        }
        .metric-name {
          font-weight: 500;
        }
        .metric-value {
          color: #2196f3;
        }
        .metric-value.bad {
          color: #f44336;
        }
        .metric-value.warning {
          color: #ff9800;
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
        .status-indicator {
          display: inline-block;
          width: 8px;
          height: 8px;
          border-radius: 50%;
          margin-right: 8px;
        }
        .status-indicator.active {
          background: #4caf50;
        }
        .status-indicator.inactive {
          background: #f44336;
        }
      </style>
      <div class="container">
        <h3>
          <span class="status-indicator active" id="status"></span>
          Performance Monitor
        </h3>
        <div class="controls">
          <button id="start-record">Start Recording</button>
          <button id="stop-record">Stop Recording</button>
          <button id="get-metrics">Get Metrics</button>
        </div>
        <div class="metrics-list" id="metrics-list"></div>
      </div>
    `;
  }

  _setupPerformanceObserver() {
    if (!('PerformanceObserver' in window)) return;

    const paintObserver = new PerformanceObserver((entries) => {
      entries.getEntries().forEach(entry => {
        this._recordMetric(`paint:${entry.name}`, entry.startTime);
      });
    });
    paintObserver.observe({ entryTypes: ['paint'] });

    const layoutObserver = new PerformanceObserver((entries) => {
      entries.getEntries().forEach(entry => {
        this._recordMetric(`layout:${entry.name}`, entry.value);
      });
    });
    layoutObserver.observe({ entryTypes: ['layout-shift'] });

    const resourceObserver = new PerformanceObserver((entries) => {
      entries.getEntries().forEach(entry => {
        this._recordMetric(`resource:${entry.name}`, entry.duration);
      });
    });
    resourceObserver.observe({ entryTypes: ['resource'] });

    this._observers.push(paintObserver, layoutObserver, resourceObserver);
  }

  _startReporting() {
    this._setupControls();
    this._updateMetricsDisplay();
    this._updateInterval = setInterval(() => this._updateMetricsDisplay(), 2000);
  }

  _setupControls() {
    this.shadowRoot.getElementById('start-record')?.addEventListener('click', () => {
      this.startRecording();
    });

    this.shadowRoot.getElementById('stop-record')?.addEventListener('click', () => {
      this.stopRecording();
    });

    this.shadowRoot.getElementById('get-metrics')?.addEventListener('click', () => {
      const metrics = this.getMetrics();
      console.log('Performance Metrics:', JSON.stringify(metrics, null, 2));
    });
  }

  _updateMetricsDisplay() {
    const container = this.shadowRoot.getElementById('metrics-list');
    if (!container) return;

    container.innerHTML = '';

    const metricGroups = this._getMetricGroups();
    
    Object.entries(metricGroups).forEach(([group, metrics]) => {
      const groupEl = document.createElement('div');
      groupEl.innerHTML = `<strong>${group}</strong>`;
      container.appendChild(groupEl);

      Object.entries(metrics).forEach(([name, value]) => {
        const row = document.createElement('div');
        row.className = 'metric-row';
        
        const valueClass = this._getValueClass(name, value);
        row.innerHTML = `
          <span class="metric-name">${name}</span>
          <span class="metric-value ${valueClass}">${value}</span>
        `;
        container.appendChild(row);
      });
    });

    const status = this.shadowRoot.getElementById('status');
    if (status) {
      status.className = `status-indicator ${this._isRecording ? 'active' : 'inactive'}`;
    }
  }

  _getMetricGroups() {
    return {
      Timing: {
        'FCP': this._getMetric('paint:first-contentful-paint')?.toFixed(2) + 'ms' || 'N/A',
        'LCP': this._getMetric('paint:largest-contentful-paint')?.toFixed(2) + 'ms' || 'N/A',
        'TTI': this._getMetric('interaction:TTI')?.toFixed(2) + 'ms' || 'N/A',
        'TTFB': this._getMetric('resource:first-byte')?.toFixed(2) + 'ms' || 'N/A'
      },
      Layout: {
        'CLS': this._getMetric('layout:cumulative-layout-shift')?.toFixed(3) || '0',
        'SL': this._getMetric('layout:longest-layout-shift')?.toFixed(3) || '0'
      },
      Memory: {
        'JS Heap': this._formatBytes(performance?.memory?.usedJSHeapSize || 0),
        'Total Heap': this._formatBytes(performance?.memory?.jsHeapSizeLimit || 0)
      }
    };
  }

  _getValueClass(name, value) {
    const numValue = parseFloat(value);
    
    if (name === 'FCP' || name === 'LCP') {
      if (numValue > 2500) return 'bad';
      if (numValue > 1500) return 'warning';
    }
    if (name === 'CLS' && numValue > 0.1) return 'bad';
    
    return '';
  }

  _formatBytes(bytes) {
    if (bytes === 0) return '0 B';
    const k = 1024;
    const sizes = ['B', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + ' ' + sizes[i];
  }

  _generateSessionId() {
    return `session_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
  }

  _recordMetric(name, value, timestamp = Date.now()) {
    const key = `${name}:${this._sessionId}`;
    this._metrics.set(key, { name, value, timestamp, sessionId: this._sessionId });
  }

  _getMetric(name) {
    const key = `${name}:${this._sessionId}`;
    return this._metrics.get(key)?.value;
  }

  startRecording() {
    this._isRecording = true;
    this._sessionId = this._generateSessionId();
    this._recordMetric('recording:start', Date.now());
  }

  stopRecording() {
    this._isRecording = false;
    this._recordMetric('recording:stop', Date.now());
  }

  mark(label) {
    this._markers.push({ label, timestamp: performance.now() });
  }

  measure(name, startMark, endMark) {
    const start = this._markers.find(m => m.label === startMark);
    const end = this._markers.find(m => m.label === endMark);
    
    if (start && end) {
      const duration = end.timestamp - start.timestamp;
      this._measures.push({ name, duration });
      this._recordMetric(`measure:${name}`, duration);
    }
  }

  getMetrics() {
    return {
      sessionId: this._sessionId,
      metrics: Array.from(this._metrics.values()),
      markers: this._markers,
      measures: this._measures
    };
  }

  _getMetricGroups() {
    const timing = {};
    const perf = performance || {};
    
    if (perf.timing) {
      timing['Load Time'] = (perf.timing.loadEventEnd - perf.timing.navigationStart)?.toFixed(0) + 'ms' || 'N/A';
    }

    return {
      Timing: timing,
      Custom: {
        'Marks': this._markers.length,
        'Measures': this._measures.length,
        'Recording': this._isRecording ? 'Active' : 'Inactive'
      }
    };
  }

  disconnectCallback() {
    if (this._updateInterval) {
      clearInterval(this._updateInterval);
    }
    this._observers.forEach(obs => obs.disconnect());
    this._observers = [];
  }
}

export { PerformanceMonitor };