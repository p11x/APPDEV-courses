/**
 * Analytics Dashboard Components - Business analytics, charts, KPIs, and data visualization for dashboards
 * @module real-world/11_8_Analytics-Dashboard-Components
 * @version 1.0.0
 * @example <kpi-card></kpi-card>
 */

class KPICard extends HTMLElement {
  constructor() {
    super();
    this.title = '';
    this.value = 0;
    this.previousValue = 0;
    this.format = 'number';
    this.trend = 'up';
  }

  static get observedAttributes() {
    return ['title', 'value', 'previous-value', 'format', 'trend', 'goal'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .kpi-card {
        background: white;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
        transition: transform 0.3s, box-shadow 0.3s;
      }
      .kpi-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 8px 30px rgba(0, 0, 0, 0.12);
      }
      .kpi-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 16px;
      }
      .kpi-title {
        font-size: 0.875rem;
        color: #6c757d;
        font-weight: 500;
        text-transform: uppercase;
        letter-spacing: 0.5px;
      }
      .kpi-icon {
        width: 40px;
        height: 40px;
        border-radius: 10px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.25rem;
        background: linear-gradient(135deg, #e0e7ff 0%, #d8b4fe 100%);
      }
      .kpi-value {
        font-size: 2rem;
        font-weight: 700;
        color: #212529;
        margin-bottom: 8px;
      }
      .kpi-footer {
        display: flex;
        align-items: center;
        gap: 8px;
      }
      .kpi-trend {
        display: inline-flex;
        align-items: center;
        gap: 4px;
        padding: 4px 8px;
        border-radius: 12px;
        font-size: 0.75rem;
        font-weight: 600;
      }
      .kpi-trend.up {
        background: rgba(40, 167, 69, 0.1);
        color: #28a745;
      }
      .kpi-trend.down {
        background: rgba(220, 53, 69, 0.1);
        color: #dc3545;
      }
      .kpi-comparison {
        font-size: 0.75rem;
        color: #6c757d;
      }
      .kpi-goal {
        margin-top: 12px;
        padding-top: 12px;
        border-top: 1px solid #e9ecef;
      }
      .kpi-goal-label {
        font-size: 0.75rem;
        color: #6c757d;
        margin-bottom: 4px;
      }
      .kpi-goal-bar {
        height: 6px;
        background: #e9ecef;
        border-radius: 3px;
        overflow: hidden;
      }
      .kpi-goal-fill {
        height: 100%;
        background: linear-gradient(90deg, #667eea 0%, #764ba2 100%);
        border-radius: 3px;
        transition: width 0.5s;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'value' || name === 'previous-value') {
        this[name === 'value' ? 'value' : 'previousValue'] = parseFloat(newValue);
      } else if (name === 'goal') {
        this.goal = parseFloat(newValue);
      } else {
        this[name] = newValue;
      }
      this.render();
    }
  }

  formatValue(value) {
    switch (this.format) {
      case 'currency':
        return new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(value);
      case 'percentage':
        return `${value}%`;
      case 'time':
        return this.formatDuration(value);
      default:
        return new Intl.NumberFormat('en-IN').format(value);
    }
  }

  formatDuration(minutes) {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return hours > 0 ? `${hours}h ${mins}m` : `${mins}m`;
  }

  calculateChange() {
    if (!this.previousValue) return 0;
    return ((this.value - this.previousValue) / this.previousValue * 100).toFixed(1);
  }

  render() {
    const change = this.calculateChange();
    const isPositive = change >= 0;
    const goalPercent = this.goal ? Math.min(100, (this.value / this.goal) * 100) : null;
    const icon = this.getAttribute('icon') || '📊';

    this.shadowRoot.innerHTML = `
      <style>${KPICard.styles}</style>
      <div class="kpi-card">
        <div class="kpi-header">
          <div class="kpi-title">${this.title}</div>
          <div class="kpi-icon">${icon}</div>
        </div>
        <div class="kpi-value">${this.formatValue(this.value)}</div>
        <div class="kpi-footer">
          <span class="kpi-trend ${this.trend}">
            ${isPositive ? '↑' : '↓'} ${Math.abs(change)}%
          </span>
          <span class="kpi-comparison">vs last period</span>
        </div>
        ${goalPercent !== null ? `
          <div class="kpi-goal">
            <div class="kpi-goal-label">Goal: ${this.formatValue(this.goal)}</div>
            <div class="kpi-goal-bar">
              <div class="kpi-goal-fill" style="width: ${goalPercent}%"></div>
            </div>
          </div>
        ` : ''}
      </div>
    `;
  }
}

class ChartComponent extends HTMLElement {
  constructor() {
    super();
    this.type = 'line';
    this.data = [];
    this.labels = [];
  }

  static get observedAttributes() {
    return ['type', 'data', 'labels'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .chart-container {
        background: white;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .chart-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 24px;
      }
      .chart-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
      }
      .chart-controls {
        display: flex;
        gap: 8px;
      }
      .chart-control {
        padding: 6px 12px;
        border: 1px solid #dee2e6;
        border-radius: 6px;
        background: white;
        cursor: pointer;
        font-size: 0.75rem;
        color: #6c757d;
        transition: all 0.2s;
      }
      .chart-control:hover,
      .chart-control.active {
        background: #667eea;
        color: white;
        border-color: #667eea;
      }
      canvas {
        width: 100%;
        height: 300px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.parseData();
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'data' || name === 'labels') {
        this.parseData();
      } else {
        this[name] = newValue;
      }
      this.render();
    }
  }

  parseData() {
    try {
      const dataAttr = this.getAttribute('data');
      const labelsAttr = this.getAttribute('labels');
      this.data = dataAttr ? JSON.parse(dataAttr) : [];
      this.labels = labelsAttr ? JSON.parse(labelsAttr) : [];
    } catch (e) {
      this.data = [];
      this.labels = [];
    }
  }

  render() {
    const canvasId = `chart-${Math.random().toString(36).substr(2, 9)}`;

    this.shadowRoot.innerHTML = `
      <style>${ChartComponent.styles}</style>
      <div class="chart-container">
        <div class="chart-header">
          <div class="chart-title">${this.getAttribute('title') || 'Chart'}</div>
          <div class="chart-controls">
            <button class="chart-control active">7D</button>
            <button class="chart-control">30D</button>
            <button class="chart-control">90D</button>
          </div>
        </div>
        <canvas id="${canvasId}"></canvas>
      </div>
    `;

    this.drawChart(canvasId);
    this.setupEventListeners();
  }

  drawChart(canvasId) {
    const canvas = this.shadowRoot.getElementById(canvasId);
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    canvas.width = canvas.parentElement.clientWidth;
    canvas.height = 300;

    const data = [12, 19, 15, 25, 22, 30, 28];
    const labels = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    const max = Math.max(...data);

    const padding = 40;
    const chartWidth = canvas.width - padding * 2;
    const chartHeight = canvas.height - padding * 2;
    const barWidth = chartWidth / data.length - 16;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    ctx.strokeStyle = '#e9ecef';
    ctx.lineWidth = 1;
    for (let i = 0; i <= 4; i++) {
      const y = padding + (chartHeight / 4) * i;
      ctx.beginPath();
      ctx.moveTo(padding, y);
      ctx.lineTo(canvas.width - padding, y);
      ctx.stroke();
    }

    const gradient = ctx.createLinearGradient(0, padding, 0, canvas.height - padding);
    gradient.addColorStop(0, 'rgba(102, 126, 234, 0.3)');
    gradient.addColorStop(1, 'rgba(102, 126, 234, 0)');

    if (this.type === 'line') {
      ctx.beginPath();
      data.forEach((val, i) => {
        const x = padding + (chartWidth / (data.length - 1)) * i;
        const y = canvas.height - padding - (val / max) * chartHeight;
        if (i === 0) ctx.moveTo(x, y);
        else ctx.lineTo(x, y);
      });
      ctx.lineTo(canvas.width - padding, canvas.height - padding);
      ctx.lineTo(padding, canvas.height - padding);
      ctx.closePath();
      ctx.fillStyle = gradient;
      ctx.fill();

      ctx.beginPath();
      data.forEach((val, i) => {
        const x = padding + (chartWidth / (data.length - 1)) * i;
        const y = canvas.height - padding - (val / max) * chartHeight;
        if (i === 0) ctx.moveTo(x, y);
        else ctx.lineTo(x, y);
      });
      ctx.strokeStyle = '#667eea';
      ctx.lineWidth = 3;
      ctx.stroke();

      data.forEach((val, i) => {
        const x = padding + (chartWidth / (data.length - 1)) * i;
        const y = canvas.height - padding - (val / max) * chartHeight;
        ctx.beginPath();
        ctx.arc(x, y, 6, 0, Math.PI * 2);
        ctx.fillStyle = '#667eea';
        ctx.fill();
        ctx.strokeStyle = '#fff';
        ctx.lineWidth = 2;
        ctx.stroke();
      });
    } else {
      data.forEach((val, i) => {
        const x = padding + (chartWidth / data.length) * i + 8;
        const height = (val / max) * chartHeight;
        const y = canvas.height - padding - height;

        const barGradient = ctx.createLinearGradient(x, y, x, canvas.height - padding);
        barGradient.addColorStop(0, '#667eea');
        barGradient.addColorStop(1, '#764ba2');

        ctx.fillStyle = barGradient;
        ctx.beginPath();
        ctx.roundRect(x, y, barWidth, height, [8, 8, 0, 0]);
        ctx.fill();
      });
    }

    ctx.fillStyle = '#6c757d';
    ctx.font = '12px sans-serif';
    ctx.textAlign = 'center';
    labels.forEach((label, i) => {
      const x = padding + (chartWidth / labels.length) * i + barWidth / 2 + 8;
      ctx.fillText(label, x, canvas.height - 10);
    });
  }

  setupEventListeners() {
    const controls = this.shadowRoot.querySelectorAll('.chart-control');
    controls.forEach(control => {
      control.addEventListener('click', () => {
        controls.forEach(c => c.classList.remove('active'));
        control.classList.add('active');
      });
    });
  }
}

class DataTableWidget extends HTMLElement {
  constructor() {
    super();
    this.columns = [];
    this.rows = [];
  }

  static get observedAttributes() {
    return ['columns', 'rows'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .table-widget {
        background: white;
        border-radius: 16px;
        overflow: hidden;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .table-header {
        padding: 20px 24px;
        border-bottom: 1px solid #e9ecef;
        display: flex;
        justify-content: space-between;
        align-items: center;
      }
      .table-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
      }
      .table-search {
        padding: 8px 16px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 0.875rem;
        width: 200px;
      }
      .table-wrapper {
        overflow-x: auto;
      }
      table {
        width: 100%;
        border-collapse: collapse;
      }
      th {
        background: #f8f9fa;
        padding: 14px 20px;
        text-align: left;
        font-size: 0.75rem;
        font-weight: 600;
        color: #6c757d;
        text-transform: uppercase;
        letter-spacing: 0.5px;
      }
      td {
        padding: 16px 20px;
        font-size: 0.875rem;
        color: #212529;
        border-bottom: 1px solid #f0f0f0;
      }
      tr:hover td {
        background: #f8f9fa;
        cursor: pointer;
      }
      .status-badge {
        display: inline-flex;
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 0.75rem;
        font-weight: 600;
      }
      .status-badge.success {
        background: rgba(40, 167, 69, 0.1);
        color: #28a745;
      }
      .status-badge.warning {
        background: rgba(255, 193, 7, 0.1);
        color: #d39e00;
      }
      .status-badge.error {
        background: rgba(220, 53, 69, 0.1);
        color: #dc3545;
      }
      .status-badge.info {
        background: rgba(102, 126, 234, 0.1);
        color: #667eea;
      }
      .table-pagination {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 16px 24px;
        border-top: 1px solid #e9ecef;
      }
      .pagination-info {
        font-size: 0.875rem;
        color: #6c757d;
      }
      .pagination-buttons {
        display: flex;
        gap: 8px;
      }
      .pagination-btn {
        padding: 8px 14px;
        border: 1px solid #dee2e6;
        border-radius: 6px;
        background: white;
        cursor: pointer;
        font-size: 0.875rem;
        transition: all 0.2s;
      }
      .pagination-btn:hover:not(:disabled) {
        background: #667eea;
        color: white;
        border-color: #667eea;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.parseData();
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this.parseData();
      this.render();
    }
  }

  parseData() {
    try {
      const columnsAttr = this.getAttribute('columns');
      const rowsAttr = this.getAttribute('rows');
      this.columns = columnsAttr ? JSON.parse(columnsAttr) : [];
      this.rows = rowsAttr ? JSON.parse(rowsAttr) : [];
    } catch (e) {
      this.columns = [];
      this.rows = [];
    }
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${DataTableWidget.styles}</style>
      <div class="table-widget">
        <div class="table-header">
          <div class="table-title">${this.getAttribute('title') || 'Data'}</div>
          <input type="text" class="table-search" placeholder="Search...">
        </div>
        <div class="table-wrapper">
          <table>
            <thead>
              <tr>
                ${this.columns.map(col => `<th>${col}</th>`).join('')}
              </tr>
            </thead>
            <tbody>
              ${this.rows.length === 0 ? `
                <tr>
                  <td colspan="${this.columns.length}" style="text-align: center; color: #6c757d;">
                    No data available
                  </td>
                </tr>
              ` : this.rows.map(row => `
                <tr>
                  ${row.map(cell => `
                    <td>${typeof cell === 'object' ? `
                      <span class="status-badge ${cell.status}">${cell.value}</span>
                    ` : cell}</td>
                  `).join('')}
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
        <div class="table-pagination">
          <div class="pagination-info">Showing 1 to ${this.rows.length} of ${this.rows.length} entries</div>
          <div class="pagination-buttons">
            <button class="pagination-btn" disabled>← Previous</button>
            <button class="pagination-btn" disabled>Next →</button>
          </div>
        </div>
      </div>
    `;
  }
}

class FunnelChart extends HTMLElement {
  constructor() {
    super();
    this.stages = [];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .funnel-container {
        background: white;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .funnel-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
        margin-bottom: 24px;
      }
      .funnel-stage {
        display: flex;
        align-items: center;
        margin-bottom: 8px;
      }
      .stage-bar {
        flex: 1;
        height: 40px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        border-radius: 8px;
        display: flex;
        align-items: center;
        padding: 0 16px;
        transition: all 0.3s;
        position: relative;
        overflow: hidden;
      }
      .stage-bar::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: linear-gradient(90deg, transparent 0%, rgba(255,255,255,0.2) 50%, transparent 100%);
        transform: translateX(-100%);
        animation: shimmer 2s infinite;
      }
      @keyframes shimmer {
        100% { transform: translateX(100%); }
      }
      .stage-label {
        color: white;
        font-weight: 600;
        font-size: 0.875rem;
        z-index: 1;
      }
      .stage-count {
        margin-left: auto;
        color: white;
        font-weight: 600;
        z-index: 1;
      }
      .stage-percentage {
        width: 60px;
        text-align: right;
        font-size: 0.875rem;
        color: #6c757d;
        margin-left: 12px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.parseStages();
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue && name === 'stages') {
      this.parseStages();
      this.render();
    }
  }

  parseStages() {
    try {
      const stagesAttr = this.getAttribute('stages');
      this.stages = stagesAttr ? JSON.parse(stagesAttr) : [
        { name: 'Visitors', value: 10000 },
        { name: 'Sign Ups', value: 3500 },
        { name: 'Activated', value: 2800 },
        { name: 'Subscribers', value: 1200 },
        { name: ' Paying', value: 450 },
      ];
    } catch (e) {
      this.stages = [];
    }
  }

  render() {
    const max = Math.max(...this.stages.map(s => s.value));

    this.shadowRoot.innerHTML = `
      <style>${FunnelChart.styles}</style>
      <div class="funnel-container">
        <div class="funnel-title">📈 Conversion Funnel</div>
        ${this.stages.map(stage => `
          <div class="funnel-stage">
            <div class="stage-bar" style="width: ${(stage.value / max) * 100}%">
              <span class="stage-label">${stage.name}</span>
              <span class="stage-count">${stage.value.toLocaleString()}</span>
            </div>
            <span class="stage-percentage">${((stage.value / max) * 100).toFixed(0)}%</span>
          </div>
        `).join('')}
      </div>
    `;
  }
}

class HeatmapChart extends HTMLElement {
  constructor() {
    super();
    this.data = [];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .heatmap-container {
        background: white;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .heatmap-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
        margin-bottom: 24px;
      }
      .heatmap-grid {
        display: grid;
        grid-template-columns: repeat(24, 1fr);
        gap: 4px;
      }
      .heatmap-cell {
        aspect-ratio: 1;
        border-radius: 4px;
        background: #e9ecef;
        cursor: pointer;
        transition: all 0.2s;
        position: relative;
      }
      .heatmap-cell:hover {
        transform: scale(1.2);
        z-index: 10;
      }
      .heatmap-cell.level-1 { background: #dbeafe; }
      .heatmap-cell.level-2 { background: #bfdbfe; }
      .heatmap-cell.level-3 { background: #93c5fd; }
      .heatmap-cell.level-4 { background: #60a5fa; }
      .heatmap-cell.level-5 { background: #3b82f6; }
      .heatmap-cell.level-6 { background: #2563eb; }
      .heatmap-cell.level-7 { background: #1d4ed8; }
      .heatmap-cell.level-8 { background: #1e40af; }
      .heatmap-tooltip {
        position: absolute;
        background: #212529;
        color: white;
        padding: 6px 12px;
        border-radius: 6px;
        font-size: 0.75rem;
        pointer-events: none;
        opacity: 0;
        transition: opacity 0.2s;
        z-index: 100;
        white-space: nowrap;
        bottom: 100%;
        left: 50%;
        transform: translateX(-50%);
        margin-bottom: 8px;
      }
      .heatmap-cell:hover .heatmap-tooltip {
        opacity: 1;
      }
      .heatmap-legend {
        display: flex;
        align-items: center;
        justify-content: flex-end;
        gap: 4px;
        margin-top: 16px;
        font-size: 0.75rem;
        color: #6c757d;
      }
      .legend-cell {
        width: 16px;
        height: 16px;
        border-radius: 4px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.generateData();
    this.render();
  }

  generateData() {
    this.data = Array.from({ length: 168 }, () => Math.floor(Math.random() * 9));
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${HeatmapChart.styles}</style>
      <div class="heatmap-container">
        <div class="heatmap-title">📊 Activity Heatmap</div>
        <div class="heatmap-grid">
          ${this.data.map((value, i) => `
            <div class="heatmap-cell level-${value + 1}" title="${value}">
              <div class="heatmap-tooltip">${value} events</div>
            </div>
          `).join('')}
        </div>
        <div class="heatmap-legend">
          <span>Less</span>
          ${[1,2,3,4,5,6,7,8,9].map(level => `
            <div class="legend-cell level-${level}"></div>
          `).join('')}
          <span>More</span>
        </div>
      </div>
    `;
  }
}

class GaugeChart extends HTMLElement {
  constructor() {
    super();
    this.value = 0;
    this.max = 100;
    this.label = 'Progress';
  }

  static get observedAttributes() {
    return ['value', 'max', 'label'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .gauge-container {
        background: white;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
        display: flex;
        flex-direction: column;
        align-items: center;
      }
      .gauge-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
        margin-bottom: 16px;
      }
      .gauge-wrapper {
        position: relative;
        width: 200px;
        height: 100px;
        overflow: hidden;
      }
      .gauge-bg {
        position: absolute;
        width: 200px;
        height: 200px;
        border-radius: 50%;
        background: #e9ecef;
      }
      .gauge-fill {
        position: absolute;
        width: 200px;
        height: 200px;
        border-radius: 50%;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        transform: rotate(0deg);
        clip-path: polygon(0 0, 100% 0, 100% 50%, 0 50%);
      }
      .gauge-value {
        position: absolute;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%);
        font-size: 2rem;
        font-weight: 700;
        color: #212529;
      }
      .gauge-label {
        font-size: 0.875rem;
        color: #6c757d;
        margin-top: 8px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'value' || name === 'max') {
        this[name] = parseFloat(newValue);
      } else {
        this[name] = newValue;
      }
      this.render();
    }
  }

  render() {
    const percent = (this.value / this.max) * 100;
    const rotation = Math.min(180, (percent / 100) * 180);

    this.shadowRoot.innerHTML = `
      <style>${GaugeChart.styles}</style>
      <div class="gauge-container">
        <div class="gauge-title">${this.label}</div>
        <div class="gauge-wrapper">
          <div class="gauge-bg"></div>
          <div class="gauge-value">${percent.toFixed(0)}%</div>
        </div>
        <div class="gauge-label">${this.value} / ${this.max}</div>
      </div>
    `;
  }
}

class AnomalyDetector extends HTMLElement {
  constructor() {
    super();
    this.threshold = 2;
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .anomaly-container {
        background: white;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .anomaly-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
        margin-bottom: 16px;
      }
      .anomaly-list {
        display: flex;
        flex-direction: column;
        gap: 12px;
      }
      .anomaly-item {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 12px;
        border-radius: 8px;
        background: #fff5f5;
        border-left: 4px solid #dc3545;
      }
      .anomaly-item.warning {
        background: #fffbeb;
        border-left-color: #d97706;
      }
      .anomaly-icon {
        font-size: 1.25rem;
      }
      .anomaly-content {
        flex: 1;
      }
      .anomaly-metric {
        font-weight: 600;
        color: #212529;
        font-size: 0.875rem;
      }
      .anomaly-detail {
        font-size: 0.75rem;
        color: #6c757d;
      }
      .anomaly-badge {
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 0.75rem;
        font-weight: 600;
      }
      .anomaly-badge.critical {
        background: rgba(220, 53, 69, 0.1);
        color: #dc3545;
      }
      .anomaly-badge.warning {
        background: rgba(217, 119, 6, 0.1);
        color: #d97706;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  render() {
    const anomalies = [
      { metric: 'Cart Abandonment', value: '67%', expected: '45%', severity: 'critical' },
      { metric: 'Page Load Time', value: '4.2s', expected: '<2s', severity: 'warning' },
      { metric: 'Failed Transactions', value: '12', expected: '<5', severity: 'critical' },
    ];

    this.shadowRoot.innerHTML = `
      <style>${AnomalyDetector.styles}</style>
      <div class="anomaly-container">
        <div class="anomaly-title">⚠️ Anomalies Detected</div>
        <div class="anomaly-list">
          ${anomalies.map(a => `
            <div class="anomaly-item ${a.severity}">
              <span class="anomaly-icon">⚠️</span>
              <div class="anomaly-content">
                <div class="anomaly-metric">${a.metric}</div>
                <div class="anomaly-detail">Current: ${a.value} | Expected: ${a.expected}</div>
              </div>
              <span class="anomaly-badge ${a.severity}">${a.severity}</span>
            </div>
          `).join('')}
        </div>
      </div>
    `;
  }
}

class PredictiveAnalytics extends HTMLElement {
  constructor() {
    super();
    this.predictions = [];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .predictions-container {
        background: white;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .predictions-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
        margin-bottom: 16px;
      }
      .prediction-item {
        padding: 12px;
        border-radius: 8px;
        background: #f8f9fa;
        margin-bottom: 8px;
      }
      .prediction-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 4px;
      }
      .prediction-metric {
        font-weight: 600;
        color: #212529;
      }
      .prediction-confidence {
        font-size: 0.75rem;
        color: #28a745;
      }
      .prediction-bar {
        height: 6px;
        background: #e9ecef;
        border-radius: 3px;
        overflow: hidden;
      }
      .prediction-fill {
        height: 100%;
        background: linear-gradient(90deg, #667eea 0%, #28a745 100%);
        border-radius: 3px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  render() {
    const predictions = [
      { metric: 'Revenue (Next 7 days)', value: '₹2.4L', confidence: 78, trend: 'up' },
      { metric: 'Churn Rate (Next 30 days)', value: '12%', confidence: 65, trend: 'down' },
      { metric: 'Active Users (Next 7 days)', value: '4,500', confidence: 82, trend: 'up' },
    ];

    this.shadowRoot.innerHTML = `
      <style>${PredictiveAnalytics.styles}</style>
      <div class="predictions-container">
        <div class="predictions-title">🔮 AI Predictions</div>
        ${predictions.map(p => `
          <div class="prediction-item">
            <div class="prediction-header">
              <span class="prediction-metric">${p.metric}</span>
              <span class="prediction-confidence">${p.confidence}% confidence</span>
            </div>
            <div style="color: #6c757d; font-size: 0.875rem; margin-bottom: 8px;">
              Predicted: ${p.value}
            </div>
            <div class="prediction-bar">
              <div class="prediction-fill" style="width: ${p.confidence}%"></div>
            </div>
          </div>
        `).join('')}
      </div>
    `;
  }
}

export { KPICard, ChartComponent, DataTableWidget, FunnelChart, HeatmapChart, GaugeChart, AnomalyDetector, PredictiveAnalytics };