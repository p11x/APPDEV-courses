/**
 * Data Flow Architecture
 * Implements unidirectional data flow, data pipeline, and reactive data streams
 * @module data-binding/05_3_Data-Flow-Architecture
 * @version 1.0.0
 * @example <data-pipeline-element source="api" mode="stream"></data-pipeline-element>
 */

const DATA_FLOW_CONFIG = {
  flowDirections: ['单向', '双向', '上游', '下游'],
  pipelineStages: ['source', 'transform', 'filter', 'validate', 'output'],
  defaultBufferSize: 10,
  maxPipelineDepth: 100,
  enableBackpressure: true,
  retryAttempts: 3,
  retryDelay: 1000,
  batchOptions: {
    enabled: true,
    maxBatchSize: 5,
    batchTimeout: 100
  }
};

class DataFlowError extends Error {
  constructor(message, code = 'DATA_FLOW_ERROR') {
    super(message);
    this.name = 'DataFlowError';
    this.code = code;
  }
}

class PipelineNode {
  constructor(config) {
    this.id = config.id || `node-${Date.now()}`;
    this.type = config.type;
    this.transform = config.transform || ((data) => data);
    this.filter = config.filter || (() => true);
    this.next = null;
    this.previous = null;
    this.dataBuffer = [];
    this.processing = false;
    this.errorHandler = config.errorHandler || console.error;
  }

  connect(node) {
    if (this.next) {
      this.next.previous = null;
    }
    this.next = node;
    node.previous = this;
    return node;
  }

  async process(data) {
    if (this.processing) {
      throw new DataFlowError(`Node ${this.id} is already processing`, 'NODE_BUSY');
    }

    this.processing = true;

    try {
      const filtered = await this._applyFilter(data);
      if (!filtered) {
        return null;
      }

      const transformed = await this._applyTransform(data);
      return transformed;
    } catch (error) {
      this.errorHandler(error);
      throw error;
    } finally {
      this.processing = false;
    }
  }

  async _applyFilter(data) {
    if (typeof this.filter === 'function') {
      return await this.filter(data);
    }
    return true;
  }

  async _applyTransform(data) {
    if (typeof this.transform === 'function') {
      return await this.transform(data);
    }
    return data;
  }

  push(data) {
    if (this.dataBuffer.length >= DATA_FLOW_CONFIG.defaultBufferSize) {
      if (DATA_FLOW_CONFIG.enableBackpressure) {
        throw new DataFlowError('Backpressure: buffer full', 'BACKPRESSURE');
      }
      this.dataBuffer.shift();
    }
    this.dataBuffer.push(data);
  }

  pop() {
    return this.dataBuffer.shift();
  }

  flush() {
    const data = [...this.dataBuffer];
    this.dataBuffer = [];
    return data;
  }
}

class SourceNode extends PipelineNode {
  constructor(config) {
    super({ ...config, type: 'source' });
    this.fetchFn = config.fetchFn;
    this.pollingInterval = null;
    this.pollingEnabled = false;
  }

  startPolling(interval = 5000) {
    if (this.pollingInterval) {
      return;
    }

    this.pollingEnabled = true;
    this.pollingInterval = setInterval(async () => {
      if (this.pollingEnabled) {
        await this.fetch();
      }
    }, interval);
  }

  stopPolling() {
    this.pollingEnabled = false;
    if (this.pollingInterval) {
      clearInterval(this.pollingInterval);
      this.pollingInterval = null;
    }
  }

  async fetch() {
    if (this.fetchFn) {
      const data = await this.fetchFn();
      this.push(data);
    }
  }
}

class TransformNode extends PipelineNode {
  constructor(config) {
    super({ ...config, type: 'transform' });
    this.transformations = [config.transform].filter(Boolean);
  }

  addTransformation(fn) {
    this.transformations.push(fn);
  }

  async _applyTransform(data) {
    let result = data;
    for (const transform of this.transformations) {
      result = await transform(result);
    }
    return result;
  }
}

class FilterNode extends PipelineNode {
  constructor(config) {
    super({ ...config, type: 'filter' });
    this.filters = [config.filter].filter(Boolean);
  }

  addFilter(fn) {
    this.filters.push(fn);
  }

  async _applyFilter(data) {
    for (const filter of this.filters) {
      if (!(await filter(data))) {
        return false;
      }
    }
    return true;
  }
}

class DataPipelineElement extends HTMLElement {
  static get observedAttributes() {
    return ['source', 'mode', 'buffer-size', 'auto-connect'];
  }

  constructor() {
    super();
    this._pipeline = null;
    this._source = null;
    this._nodes = new Map();
    this._flowHistory = [];
    this._subscribers = new Set();
    this._mode = 'stream';
    this._bufferSize = DATA_FLOW_CONFIG.defaultBufferSize;
    this._autoConnect = true;
    this._processing = false;
    this._currentData = null;
    
    this._initPipeline();
    this._attachShadowDOM();
  }

  _initPipeline() {
    this._source = new SourceNode({
      id: 'source',
      fetchFn: () => this._generateData()
    });

    const transform = new TransformNode({
      id: 'transform',
      transform: (data) => this._processTransform(data)
    });

    const filter = new FilterNode({
      id: 'filter',
      filter: (data) => this._processFilter(data)
    });

    const output = new PipelineNode({
      id: 'output',
      type: 'output',
      transform: (data) => this._processOutput(data)
    });

    this._source.connect(transform).connect(filter).connect(output);

    this._nodes.set('source', this._source);
    this._nodes.set('transform', transform);
    this._nodes.set('filter', filter);
    this._nodes.set('output', output);

    this._pipeline = this._source;
  }

  _attachShadowDOM() {
    const shadow = this.attachShadow({ mode: 'open' });
    
    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
        padding: 20px;
        border: 2px solid #9c27b0;
        border-radius: 10px;
        background: linear-gradient(135deg, #fff 0%, #f3e5f5 100%);
        font-family: 'Segoe UI', system-ui, sans-serif;
      }

      :host([mode="batch"]) {
        border-color: #ff9800;
        background: linear-gradient(135deg, #fff 0%, #fff3e0 100%);
      }

      :host([mode="stream"]) {
        border-color: #2196f3;
        background: linear-gradient(135deg, #fff 0%, #e3f2fd 100%);
      }

      .pipeline-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid rgba(0,0,0,0.1);
      }

      .pipeline-title {
        font-size: 16px;
        font-weight: 700;
        color: #333;
      }

      .pipeline-badge {
        font-size: 11px;
        padding: 4px 10px;
        background: #9c27b0;
        color: white;
        border-radius: 12px;
        font-weight: 500;
      }

      .pipeline-visual {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 16px;
        padding: 16px;
        background: rgba(255,255,255,0.7);
        border-radius: 8px;
        overflow-x: auto;
      }

      .node {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 4px;
        padding: 12px 16px;
        background: white;
        border: 2px solid #ddd;
        border-radius: 8px;
        min-width: 80px;
        transition: all 0.3s;
      }

      .node.active {
        border-color: #4caf50;
        background: #e8f5e9;
      }

      .node.processing {
        border-color: #2196f3;
        background: #e3f2fd;
        box-shadow: 0 0 8px rgba(33, 150, 243, 0.4);
      }

      .node.error {
        border-color: #f44336;
        background: #ffebee;
      }

      .node-type {
        font-size: 10px;
        text-transform: uppercase;
        color: #888;
        letter-spacing: 0.5px;
      }

      .node-name {
        font-size: 13px;
        font-weight: 600;
        color: #333;
      }

      .node-arrow {
        font-size: 20px;
        color: #bbb;
        flex-shrink: 0;
      }

      .data-preview {
        margin-bottom: 16px;
        padding: 12px;
        background: white;
        border-radius: 6px;
        border: 1px solid #eee;
      }

      .data-label {
        font-size: 11px;
        color: #888;
        margin-bottom: 4px;
      }

      .data-content {
        font-family: 'Fira Code', monospace;
        font-size: 12px;
        color: #333;
        white-space: pre-wrap;
        word-break: break-all;
      }

      .flow-controls {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 8px;
        margin-bottom: 16px;
      }

      .flow-btn {
        padding: 10px 12px;
        border: 1px solid #ccc;
        border-radius: 6px;
        background: white;
        font-size: 12px;
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s;
      }

      .flow-btn:hover {
        background: #f5f5f5;
        border-color: #aaa;
      }

      .flow-btn:active {
        background: #e0e0e0;
      }

      .flow-history {
        background: #fafafa;
        border-radius: 6px;
        padding: 12px;
        max-height: 150px;
        overflow-y: auto;
      }

      .history-title {
        font-size: 12px;
        font-weight: 600;
        color: #666;
        margin-bottom: 8px;
      }

      .history-item {
        padding: 4px 8px;
        margin: 2px 0;
        background: white;
        border-radius: 4px;
        font-size: 11px;
        font-family: monospace;
      }

      .history-item .type {
        color: #9c27b0;
        font-weight: 600;
      }

      .history-item .value {
        color: #333;
      }

      .subscribers-count {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-top: 12px;
        padding: 8px 12px;
        background: #e8eaf6;
        border-radius: 6px;
      }

      .subscribers-label {
        font-size: 11px;
        color: #666;
      }

      .subscribers-value {
        font-size: 14px;
        font-weight: 600;
        color: #3949ab;
      }

      @keyframes flow {
        0%, 100% { transform: translateX(0); }
        50% { transform: translateX(4px); }
      }

      .flowing .node-arrow {
        animation: flow 0.5s ease-in-out infinite;
      }
    `;

    const container = document.createElement('div');
    container.innerHTML = `
      <div class="pipeline-header">
        <span class="pipeline-title">Data Flow Pipeline</span>
        <span class="pipeline-badge" id="mode-badge">STREAM</span>
      </div>
      
      <div class="pipeline-visual" id="pipeline-visual">
        <div class="node" data-node="source">
          <span class="node-type">Source</span>
          <span class="node-name">source</span>
        </div>
        <span class="node-arrow">→</span>
        <div class="node" data-node="transform">
          <span class="node-type">Transform</span>
          <span class="node-name">transform</span>
        </div>
        <span class="node-arrow">→</span>
        <div class="node" data-node="filter">
          <span class="node-type">Filter</span>
          <span class="node-name">filter</span>
        </div>
        <span class="node-arrow">→</span>
        <div class="node" data-node="output">
          <span class="node-type">Output</span>
          <span class="node-name">output</span>
        </div>
      </div>
      
      <div class="data-preview">
        <div class="data-label">Current Data</div>
        <div class="data-content" id="current-data">No data</div>
      </div>
      
      <div class="flow-controls">
        <button class="flow-btn" id="btn-push">Push Data</button>
        <button class="flow-btn" id="btn-process">Process</button>
        <button class="flow-btn" id="btn-clear">Clear</button>
      </div>
      
      <div class="flow-history">
        <div class="history-title">Flow History</div>
        <div id="flow-history"></div>
      </div>
      
      <div class="subscribers-count">
        <span class="subscribers-label">Subscribers:</span>
        <span class="subscribers-value" id="subscriber-count">0</span>
      </div>
    `;
    
    shadow.appendChild(style);
    shadow.appendChild(container);
  }

  connectedCallback() {
    this._bindEvents();
    this._render();
    this._logFlow('connected', 'Pipeline initialized');
    
    if (this._autoConnect && this._mode === 'stream') {
      this._startProcessing();
    }
  }

  disconnectedCallback() {
    this._stopProcessing();
    this._logFlow('disconnected', 'Pipeline destroyed');
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'source':
        this._handleSourceChange(newValue);
        break;
      case 'mode':
        this._handleModeChange(newValue);
        break;
      case 'buffer-size':
        this._bufferSize = parseInt(newValue, 10) || DATA_FLOW_CONFIG.defaultBufferSize;
        break;
      case 'auto-connect':
        this._autoConnect = newValue !== 'false';
        break;
    }
  }

  _bindEvents() {
    const shadow = this.shadowRoot;
    
    const btnPush = shadow.getElementById('btn-push');
    const btnProcess = shadow.getElementById('btn-process');
    const btnClear = shadow.getElementById('btn-clear');

    btnPush?.addEventListener('click', () => this.pushData(this._generateData()));
    btnProcess?.addEventListener('click', () => this.processData());
    btnClear?.addEventListener('click', () => this.clearPipeline());
  }

  _handleSourceChange(source) {
    this._logFlow('source', `Source changed to: ${source}`);
    this._render();
  }

  _handleModeChange(mode) {
    this._mode = mode;
    this._logFlow('mode', `Mode changed to: ${mode}`);
    
    if (mode === 'stream') {
      this._startProcessing();
    } else if (mode === 'batch') {
      this._stopProcessing();
    }
    
    if (mode === 'stream') {
      this.setAttribute('mode', 'stream');
    } else if (mode === 'batch') {
      this.setAttribute('mode', 'batch');
    }
    
    this._render();
  }

  async _generateData() {
    return {
      id: `data-${Date.now()}`,
      timestamp: new Date().toISOString(),
      value: Math.floor(Math.random() * 1000),
      type: ['numeric', 'string', 'object'][Math.floor(Math.random() * 3)],
      metadata: {
        source: this._getAttribute('source') || 'default',
        priority: Math.floor(Math.random() * 5) + 1
      }
    };
  }

  _processTransform(data) {
    return {
      ...data,
      processed: true,
      transformedAt: new Date().toISOString(),
      value: typeof data.value === 'number' ? data.value * 2 : data.value
    };
  }

  _processFilter(data) {
    return data && data.value > 0;
  }

  _processOutput(data) {
    this._currentData = data;
    this._notifySubscribers(data);
    this._logFlow('output', `Processed: ${JSON.stringify(data).substring(0, 50)}`);
    this._render();
    return data;
  }

  async pushData(data) {
    if (!this._source) {
      throw new DataFlowError('No source node', 'NO_SOURCE');
    }

    this._source.push(data);
    this._logFlow('push', `Data pushed: ${data?.id || 'unknown'}`);
    
    if (this._mode === 'stream') {
      await this.processData();
    }
  }

  async processData() {
    if (this._processing) {
      return;
    }

    this._processing = true;
    this._updateNodeState('source', 'processing');

    try {
      let current = this._source;
      while (current) {
        this._updateNodeState(current.id, 'processing');
        
        const buffered = current.flush();
        for (const data of buffered) {
          const result = await current.process(data);
          if (result && current.next) {
            await current.next.process(result);
          }
        }
        
        this._updateNodeState(current.id, 'active');
        current = current.next;
      }
    } catch (error) {
      this._handleError(error, 'PROCESSING');
    } finally {
      this._processing = false;
      this._clearNodeStates();
    }
  }

  subscribe(callback) {
    this._subscribers.add(callback);
    
    const unsubscribe = () => {
      this._subscribers.delete(callback);
    };
    
    return unsubscribe;
  }

  _notifySubscribers(data) {
    this._subscribers.forEach(callback => {
      try {
        callback(data);
      } catch (error) {
        console.error('Subscriber error:', error);
      }
    });
  }

  _updateNodeState(nodeId, state) {
    const shadow = this.shadowRoot;
    const nodeEl = shadow?.querySelector(`[data-node="${nodeId}"]`);
    if (nodeEl) {
      nodeEl.classList.remove('active', 'processing', 'error');
      nodeEl.classList.add(state);
    }
  }

  _clearNodeStates() {
    const shadow = this.shadowRoot;
    const nodes = shadow?.querySelectorAll('.node');
    nodes?.forEach(node => {
      node.classList.remove('processing', 'active');
    });
  }

  _startProcessing() {
    if (this._source && typeof this._source.startPolling === 'function') {
      this._source.startPolling(3000);
    }
  }

  _stopProcessing() {
    if (this._source && typeof this._source.stopPolling === 'function') {
      this._source.stopPolling();
    }
  }

  clearPipeline() {
    let current = this._source;
    while (current) {
      current.flush();
      current = current.next;
    }
    this._currentData = null;
    this._logFlow('clear', 'Pipeline cleared');
    this._render();
  }

  _logFlow(type, message) {
    this._flowHistory.push({
      type,
      message,
      timestamp: Date.now()
    });

    if (this._flowHistory.length > 50) {
      this._flowHistory.shift();
    }

    this._updateHistoryDisplay();
  }

  _render() {
    const shadow = this.shadowRoot;
    
    const modeBadge = shadow.getElementById('mode-badge');
    const currentData = shadow.getElementById('current-data');
    const subscriberCount = shadow.getElementById('subscriber-count');

    if (modeBadge) {
      modeBadge.textContent = this._mode.toUpperCase();
    }

    if (currentData) {
      currentData.textContent = this._currentData 
        ? JSON.stringify(this._currentData, null, 2)
        : 'No data';
    }

    if (subscriberCount) {
      subscriberCount.textContent = String(this._subscribers.size);
    }
  }

  _updateHistoryDisplay() {
    const shadow = this.shadowRoot;
    const historyEl = shadow?.getElementById('flow-history');
    if (!historyEl) return;

    historyEl.innerHTML = this._flowHistory
      .slice(-10)
      .reverse()
      .map(entry => `
        <div class="history-item">
          <span class="type">[${entry.type}]</span>
          <span class="value">${entry.message}</span>
        </div>
      `).join('');
  }

  _handleError(error, code) {
    console.error(`DataFlowError [${code}]:`, error);
    this._logFlow('error', `Error: ${error.message}`);
  }

  get source() {
    return this.getAttribute('source');
  }

  set source(value) {
    this.setAttribute('source', value);
  }

  get mode() {
    return this._mode;
  }

  set mode(value) {
    this._handleModeChange(value);
  }
}

if (!customElements.get('data-pipeline-element')) {
  customElements.define('data-pipeline-element', DataPipelineElement);
}

export { 
  DataPipelineElement, 
  PipelineNode, 
  SourceNode, 
  TransformNode, 
  FilterNode,
  DATA_FLOW_CONFIG,
  DataFlowError 
};