/**
 * Web Component Analytics - Performance metrics for web components
 * @module performance/09_6_Web-Component-Analytics
 * @version 1.0.0
 * @example <component-analytics></component-analytics>
 */

class ComponentAnalytics extends HTMLElement {
  constructor() {
    super();
    this._componentMetrics = new Map();
    this._eventQueue = [];
    this._sessionStart = Date.now();
    this._customDimensions = new Map();
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this._setupAnalytics();
    this._render();
  }

  _setupAnalytics() {
    this._trackLifecycle();
    this._trackInteractions();
    this._trackErrors();
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
          font-family: system-ui, sans-serif;
        }
        .analytics-card {
          background: white;
          border-radius: 8px;
          padding: 16px;
          margin: 8px 0;
          box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .metric-header {
          font-size: 14px;
          color: #666;
          margin-bottom: 8px;
        }
        .metric-value {
          font-size: 32px;
          font-weight: bold;
          color: #2196f3;
        }
        .metric-grid {
          display: grid;
          grid-template-columns: repeat(2, 1fr);
          gap: 12px;
        }
        .timeline {
          margin-top: 16px;
          position: relative;
          padding-left: 24px;
        }
        .timeline-item {
          position: relative;
          padding: 8px 0;
          border-left: 2px solid #e0e0e0;
          padding-left: 12px;
          margin-left: -14px;
        }
        .timeline-item::before {
          content: '';
          position: absolute;
          left: -6px;
          top: 12px;
          width: 10px;
          height: 10px;
          border-radius: 50%;
          background: #2196f3;
        }
        button {
          padding: 8px 16px;
          margin: 4px;
          border: 1px solid #ccc;
          border-radius: 4px;
          background: white;
          cursor: pointer;
        }
      </style>
      <div class="container">
        <h3>Web Component Analytics</h3>
        <div class="metric-grid">
          <div class="analytics-card">
            <div class="metric-header">Total Views</div>
            <div class="metric-value" id="views">0</div>
          </div>
          <div class="analytics-card">
            <div class="metric-header">Interactions</div>
            <div class="metric-value" id="interactions">0</div>
          </div>
          <div class="analytics-card">
            <div class="metric-header">Errors</div>
            <div class="metric-value" id="errors">0</div>
          </div>
          <div class="analytics-card">
            <div class="metric-header">Avg. Render Time</div>
            <div class="metric-value" id="render-time">0ms</div>
          </div>
        </div>
        <div class="analytics-card">
          <div class="metric-header">Event Timeline</div>
          <div class="timeline" id="timeline"></div>
        </div>
        <div style="margin-top: 12px;">
          <button id="track-event">Track Event</button>
          <button id="export-data">Export Data</button>
        </div>
      </div>
    `;
  }

  _trackLifecycle() {
    const connectedAt = Date.now();
    this._recordMetric('lifecycle:connected', connectedAt);
  }

  _trackInteractions() {
    this.addEventListener('click', (e) => this._handleInteraction(e));
    this.addEventListener('input', (e) => this._handleInteraction(e));
    this.addEventListener('focus', (e) => this._handleInteraction(e));
  }

  _trackErrors() {
    window.addEventListener('error', (e) => {
      this._recordError({
        type: 'error',
        message: e.message,
        filename: e.filename,
        lineno: e.lineno
      });
    });

    window.addEventListener('unhandledrejection', (e) => {
      this._recordError({
        type: 'unhandledrejection',
        reason: e.reason?.toString() || 'Unknown'
      });
    });
  }

  _handleInteraction(event) {
    const interactionType = event.type;
    const target = event.target?.tagName || 'unknown';
    const timestamp = Date.now();

    this._recordMetric('interaction', {
      type: interactionType,
      target,
      timestamp,
      sessionDuration: timestamp - this._sessionStart
    });

    this._queueEvent({
      type: 'interaction',
      data: { interactionType, target },
      timestamp
    });
  }

  _recordMetric(name, value) {
    const metric = {
      name,
      value,
      timestamp: Date.now(),
      component: this.tagName.toLowerCase()
    };

    if (!this._componentMetrics.has(name)) {
      this._componentMetrics.set(name, []);
    }
    this._componentMetrics.get(name).push(metric);
  }

  _recordError(error) {
    this._recordMetric('error', error);
    
    this._queueEvent({
      type: 'error',
      data: error,
      timestamp: Date.now()
    });
  }

  _queueEvent(event) {
    this._eventQueue.push(event);
    
    if (this._eventQueue.length > 100) {
      this._eventQueue.shift();
    }
  }

  trackCustomEvent(eventName, attributes = {}) {
    this._recordMetric(`custom:${eventName}`, {
      ...attributes,
      timestamp: Date.now()
    });

    this._queueEvent({
      type: 'custom',
      name: eventName,
      data: attributes,
      timestamp: Date.now()
    });
  }

  setCustomDimension(key, value) {
    this._customDimensions.set(key, value);
  }

  getAnalytics() {
    const totalViews = this._componentMetrics.size;
    const totalInteractions = this._getMetricCount('interaction');
    const totalErrors = this._getMetricCount('error');
    const renderTimes = this._getRenderTimes();
    
    const avgRenderTime = renderTimes.length > 0
      ? renderTimes.reduce((a, b) => a + b, 0) / renderTimes.length
      : 0;

    return {
      component: this.tagName.toLowerCase(),
      sessionId: this._sessionId,
      totalViews,
      totalInteractions,
      totalErrors,
      avgRenderTime,
      events: this._eventQueue,
      customDimensions: Object.fromEntries(this._customDimensions)
    };
  }

  _getMetricCount(type) {
    const metrics = this._componentMetrics.get(type);
    return metrics ? metrics.length : 0;
  }

  _getRenderTimes() {
    const connected = this._componentMetrics.get('lifecycle:connected');
    if (!connected) return [];
    
    return connected.map(m => {
      return m.value - this._sessionStart;
    });
  }

  get _sessionId() {
    return `session_${this._sessionStart}`;
  }

  disconnectCallback() {
    this._recordMetric('lifecycle:disconnected', Date.now());
  }
}

export { ComponentAnalytics };