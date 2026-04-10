/**
 * Runtime Performance Techniques - Optimization strategies for web components
 * @module performance/09_2_Runtime-Performance-Techniques
 * @version 1.0.0
 * @example <performance-optimizer></performance-optimizer>
 */

class RuntimePerformanceOptimizer extends HTMLElement {
  constructor() {
    super();
    this._frameRequestId = null;
    this._lastFrameTime = 0;
    this._frameTimes = [];
    this._maxSamples = 60;
    this._shouldOptimize = true;
    this._rafCallback = this._onFrame.bind(this);
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this._render();
    this._startMonitoring();
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
          font-family: system-ui, sans-serif;
        }
        .stats {
          display: grid;
          grid-template-columns: repeat(3, 1fr);
          gap: 8px;
          margin-top: 12px;
        }
        .stat {
          padding: 12px;
          background: #f5f5f5;
          border-radius: 4px;
          text-align: center;
        }
        .stat-value {
          font-size: 24px;
          font-weight: bold;
          color: #2196f3;
        }
        .stat-label {
          font-size: 12px;
          color: #666;
        }
        canvas {
          width: 100%;
          height: 100px;
          margin-top: 12px;
        }
      </style>
      <div class="container">
        <h3>Runtime Performance Optimizer</h3>
        <canvas id="fps-chart"></canvas>
        <div class="stats">
          <div class="stat">
            <div class="stat-value" id="fps">60</div>
            <div class="stat-label">FPS</div>
          </div>
          <div class="stat">
            <div class="stat-value" id="frame-time">16</div>
            <div class="stat-label">Frame Time (ms)</div>
          </div>
          <div class="stat">
            <div class="stat-value" id="dropped">0</div>
            <div class="stat-label">Dropped Frames</div>
          </div>
        </div>
      </div>
    `;
  }

  _startMonitoring() {
    requestAnimationFrame(this._rafCallback);
  }

  _onFrame(timestamp) {
    if (!this._shouldOptimize) {
      this._frameRequestId = requestAnimationFrame(this._rafCallback);
      return;
    }

    const delta = timestamp - this._lastFrameTime;
    this._lastFrameTime = timestamp;

    if (this._frameTimes.length >= this._maxSamples) {
      this._frameTimes.shift();
    }
    this._frameTimes.push(delta);

    this._updateStats();
    this._renderChart();

    this._frameRequestId = requestAnimationFrame(this._rafCallback);
  }

  _updateStats() {
    const avgFrameTime = this._frameTimes.reduce((a, b) => a + b, 0) / this._frameTimes.length;
    const fps = Math.round(1000 / avgFrameTime);
    const droppedFrames = this._frameTimes.filter(t => t > 16.67).length;

    const fpsEl = this.shadowRoot.getElementById('fps');
    const frameTimeEl = this.shadowRoot.getElementById('frame-time');
    const droppedEl = this.shadowRoot.getElementById('dropped');

    if (fpsEl) fpsEl.textContent = fps;
    if (frameTimeEl) frameTimeEl.textContent = avgFrameTime.toFixed(1);
    if (droppedEl) droppedEl.textContent = droppedFrames;
  }

  _renderChart() {
    const canvas = this.shadowRoot.getElementById('fps-chart');
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    const width = canvas.width = canvas.offsetWidth * 2;
    const height = canvas.height = 200;

    ctx.clearRect(0, 0, width, height);

    ctx.strokeStyle = '#2196f3';
    ctx.lineWidth = 2;
    ctx.beginPath();

    this._frameTimes.forEach((frameTime, i) => {
      const x = (i / this._maxSamples) * width;
      const y = height - (frameTime / 33.33) * height;

      if (i === 0) {
        ctx.moveTo(x, y);
      } else {
        ctx.lineTo(x, y);
      }
    });

    ctx.stroke();

    ctx.strokeStyle = '#ff9800';
    ctx.setLineDash([5, 5]);
    ctx.beginPath();
    ctx.moveTo(0, height / 2);
    ctx.lineTo(width, height / 2);
    ctx.stroke();
    ctx.setLineDash([]);
  }

  pause() {
    this._shouldOptimize = false;
  }

  resume() {
    this._shouldOptimize = true;
    this._lastFrameTime = performance.now();
  }

  getMetrics() {
    const avgFrameTime = this._frameTimes.reduce((a, b) => a + b, 0) / this._frameTimes.length;
    const fps = Math.round(1000 / avgFrameTime);
    const droppedFrames = this._frameTimes.filter(t => t > 16.67).length;

    return {
      fps,
      avgFrameTime,
      droppedFrames,
      samples: this._frameTimes.length
    };
  }

  disconnectCallback() {
    if (this._frameRequestId) {
      cancelAnimationFrame(this._frameRequestId);
    }
    this._frameTimes = [];
  }
}

export { RuntimePerformanceOptimizer };