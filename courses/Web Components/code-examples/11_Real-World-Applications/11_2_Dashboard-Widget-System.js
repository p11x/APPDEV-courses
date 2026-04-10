/**
 * Dashboard Widget System - Analytics dashboard widgets with charts, metrics, and data visualization
 * @module real-world/11_2_Dashboard-Widget-System
 * @version 1.0.0
 * @example <metric-card></metric-card>
 */

class MetricCard extends HTMLElement {
  constructor() {
    super();
    this.value = 0;
    this.label = 'Metric';
    this.change = 0;
    this.prefix = '';
    this.suffix = '';
  }

  static get observedAttributes() {
    return ['value', 'label', 'change', 'prefix', 'suffix'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .metric-card {
        background: #fff;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
        transition: transform 0.3s, box-shadow 0.3s;
        position: relative;
        overflow: hidden;
      }
      .metric-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 8px 30px rgba(0, 0, 0, 0.12);
      }
      .metric-card::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        width: 4px;
        height: 100%;
        background: linear-gradient(180deg, #667eea 0%, #764ba2 100%);
      }
      .metric-icon {
        width: 48px;
        height: 48px;
        border-radius: 12px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.5rem;
        margin-bottom: 16px;
        background: linear-gradient(135deg, rgba(102, 126, 234, 0.1) 0%, rgba(118, 75, 162, 0.1) 100%);
      }
      .metric-label {
        font-size: 0.875rem;
        color: #6c757d;
        font-weight: 500;
        text-transform: uppercase;
        letter-spacing: 0.5px;
        margin-bottom: 8px;
      }
      .metric-value {
        font-size: 2rem;
        font-weight: 700;
        color: #212529;
        margin-bottom: 8px;
      }
      .metric-change {
        display: inline-flex;
        align-items: center;
        gap: 4px;
        padding: 4px 10px;
        border-radius: 20px;
        font-size: 0.75rem;
        font-weight: 600;
      }
      .metric-change.positive {
        background: rgba(40, 167, 69, 0.1);
        color: #28a745;
      }
      .metric-change.negative {
        background: rgba(220, 53, 69, 0.1);
        color: #dc3545;
      }
      .metric-period {
        font-size: 0.75rem;
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
      switch (name) {
        case 'value':
        case 'change':
          this[name] = parseFloat(newValue);
          break;
        case 'label':
        case 'prefix':
        case 'suffix':
          this[name] = newValue;
          break;
      }
      this.render();
    }
  }

  formatValue(value) {
    if (value >= 10000000) {
      return (value / 10000000).toFixed(1) + 'Cr';
    } else if (value >= 100000) {
      return (value / 100000).toFixed(1) + 'L';
    } else if (value >= 1000) {
      return (value / 1000).toFixed(1) + 'K';
    }
    return value.toString();
  }

  render() {
    const changeClass = this.change >= 0 ? 'positive' : 'negative';
    const changeIcon = this.change >= 0 ? '↑' : '↓';
    const icon = this.getAttribute('icon') || '📊';

    this.shadowRoot.innerHTML = `
      <style>${MetricCard.styles}</style>
      <div class="metric-card">
        <div class="metric-icon">${icon}</div>
        <div class="metric-label">${this.label}</div>
        <div class="metric-value">${this.prefix}${this.formatValue(this.value)}${this.suffix}</div>
        <div class="metric-change ${changeClass}">
          ${changeIcon} ${Math.abs(this.change)}%
        </div>
        <div class="metric-period">vs last month</div>
      </div>
    `;
  }
}

class ChartWidget extends HTMLElement {
  constructor() {
    super();
    this.type = 'line';
    this.data = [];
    this.title = 'Chart';
    this.height = 300;
  }

  static get observedAttributes() {
    return ['type', 'data', 'title', 'height'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .chart-widget {
        background: #fff;
        border-radius: 16px;
        padding: 24px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .chart-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
      }
      .chart-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
      }
      .chart-actions {
        display: flex;
        gap: 8px;
      }
      .chart-action {
        padding: 6px 12px;
        border: 1px solid #dee2e6;
        border-radius: 6px;
        background: #fff;
        cursor: pointer;
        font-size: 0.75rem;
        color: #6c757d;
        transition: all 0.2s;
      }
      .chart-action:hover,
      .chart-action.active {
        background: #667eea;
        color: white;
        border-color: #667eea;
      }
      .chart-container {
        position: relative;
        height: ${this.height}px;
      }
      .chart-canvas {
        width: 100%;
        height: 100%;
      }
      .chart-legend {
        display: flex;
        justify-content: center;
        gap: 24px;
        margin-top: 16px;
      }
      .legend-item {
        display: flex;
        align-items: center;
        gap: 8px;
        font-size: 0.875rem;
        color: #495057;
      }
      .legend-dot {
        width: 12px;
        height: 12px;
        border-radius: 50%;
      }
      .chart-tooltip {
        position: absolute;
        background: #212529;
        color: white;
        padding: 8px 12px;
        border-radius: 8px;
        font-size: 0.75rem;
        pointer-events: none;
        opacity: 0;
        transition: opacity 0.2s;
        z-index: 100;
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
      switch (name) {
        case 'data':
          this.parseData();
          break;
        case 'type':
        case 'title':
        case 'height':
          this[name] = newValue;
          break;
      }
      this.render();
    }
  }

  parseData() {
    try {
      this.data = JSON.parse(this.getAttribute('data') || '[]');
    } catch (e) {
      this.data = [];
    }
  }

  render() {
    const canvasId = `chart-${Math.random().toString(36).substr(2, 9)}`;

    this.shadowRoot.innerHTML = `
      <style>${ChartWidget.styles}</style>
      <div class="chart-widget">
        <div class="chart-header">
          <div class="chart-title">${this.title}</div>
          <div class="chart-actions">
            <button class="chart-action active" data-period="7d">7D</button>
            <button class="chart-action" data-period="30d">30D</button>
            <button class="chart-action" data-period="90d">90D</button>
          </div>
        </div>
        <div class="chart-container">
          <canvas id="${canvasId}" class="chart-canvas"></canvas>
        </div>
        <div class="chart-legend">
          <div class="legend-item">
            <div class="legend-dot" style="background: #667eea;"></div>
            <span>Revenue</span>
          </div>
          <div class="legend-item">
            <div class="legend-dot" style="background: #764ba2;"></div>
            <span>Orders</span>
          </div>
        </div>
      </div>
    `;

    this.drawChart(canvasId);
    this.setupEventListeners();
  }

  drawChart(canvasId) {
    const canvas = this.shadowRoot.getElementById(canvasId);
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    const container = canvas.parentElement;
    canvas.width = container.clientWidth;
    canvas.height = container.clientHeight;

    const padding = 40;
    const chartWidth = canvas.width - padding * 2;
    const chartHeight = canvas.height - padding * 2;

    // Sample data points
    const labels = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'];
    const revenueData = [30000, 45000, 35000, 52000, 48000, 61000, 55000];
    const ordersData = [120, 180, 140, 210, 190, 250, 220];

    const maxRevenue = Math.max(...revenueData);
    const maxOrders = Math.max(...ordersData);

    // Draw grid lines
    ctx.strokeStyle = '#e9ecef';
    ctx.lineWidth = 1;
    for (let i = 0; i <= 4; i++) {
      const y = padding + (chartHeight / 4) * i;
      ctx.beginPath();
      ctx.moveTo(padding, y);
      ctx.lineTo(canvas.width - padding, y);
      ctx.stroke();
    }

    // Draw revenue line
    ctx.strokeStyle = '#667eea';
    ctx.lineWidth = 3;
    ctx.beginPath();
    revenueData.forEach((val, i) => {
      const x = padding + (chartWidth / (revenueData.length - 1)) * i;
      const y = padding + chartHeight - (val / maxRevenue) * chartHeight;
      if (i === 0) ctx.moveTo(x, y);
      else ctx.lineTo(x, y);
    });
    ctx.stroke();

    // Draw revenue points
    revenueData.forEach((val, i) => {
      const x = padding + (chartWidth / (revenueData.length - 1)) * i;
      const y = padding + chartHeight - (val / maxRevenue) * chartHeight;
      ctx.beginPath();
      ctx.arc(x, y, 6, 0, Math.PI * 2);
      ctx.fillStyle = '#667eea';
      ctx.fill();
      ctx.strokeStyle = '#fff';
      ctx.lineWidth = 2;
      ctx.stroke();
    });

    // Draw orders line
    ctx.strokeStyle = '#764ba2';
    ctx.lineWidth = 3;
    ctx.beginPath();
    ordersData.forEach((val, i) => {
      const x = padding + (chartWidth / (ordersData.length - 1)) * i;
      const y = padding + chartHeight - (val / maxOrders) * chartHeight;
      if (i === 0) ctx.moveTo(x, y);
      else ctx.lineTo(x, y);
    });
    ctx.stroke();

    // Draw orders points
    ordersData.forEach((val, i) => {
      const x = padding + (chartWidth / (ordersData.length - 1)) * i;
      const y = padding + chartHeight - (val / maxOrders) * chartHeight;
      ctx.beginPath();
      ctx.arc(x, y, 6, 0, Math.PI * 2);
      ctx.fillStyle = '#764ba2';
      ctx.fill();
      ctx.strokeStyle = '#fff';
      ctx.lineWidth = 2;
      ctx.stroke();
    });

    // Draw x-axis labels
    ctx.fillStyle = '#6c757d';
    ctx.font = '12px Poppins';
    ctx.textAlign = 'center';
    labels.forEach((label, i) => {
      const x = padding + (chartWidth / (labels.length - 1)) * i;
      ctx.fillText(label, x, canvas.height - 10);
    });
  }

  setupEventListeners() {
    const actions = this.shadowRoot.querySelectorAll('.chart-action');
    actions.forEach(action => {
      action.addEventListener('click', () => {
        actions.forEach(a => a.classList.remove('active'));
        action.classList.add('active');
      });
    });
  }
}

class DataTable extends HTMLElement {
  constructor() {
    super();
    this.columns = [];
    this.data = [];
    this.pageSize = 10;
    this.currentPage = 1;
  }

  static get observedAttributes() {
    return ['columns', 'data', 'page-size'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .data-table {
        background: #fff;
        border-radius: 16px;
        overflow: hidden;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
      }
      .table-header {
        padding: 20px 24px;
        border-bottom: 1px solid #dee2e6;
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
        width: 250px;
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
        border-bottom: 2px solid #dee2e6;
      }
      td {
        padding: 16px 20px;
        font-size: 0.875rem;
        color: #212529;
        border-bottom: 1px solid #f0f0f0;
      }
      tr:hover td {
        background: #f8f9fa;
      }
      .status-badge {
        display: inline-flex;
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 0.75rem;
        font-weight: 600;
      }
      .status-badge.completed {
        background: rgba(40, 167, 69, 0.1);
        color: #28a745;
      }
      .status-badge.pending {
        background: rgba(255, 193, 7, 0.1);
        color: #ffc107;
      }
      .status-badge.cancelled {
        background: rgba(220, 53, 69, 0.1);
        color: #dc3545;
      }
      .status-badge.processing {
        background: rgba(102, 126, 234, 0.1);
        color: #667eea;
      }
      .action-btn {
        padding: 6px 12px;
        border: 1px solid #dee2e6;
        border-radius: 6px;
        background: #fff;
        cursor: pointer;
        font-size: 0.75rem;
        color: #495057;
        transition: all 0.2s;
        margin-right: 4px;
      }
      .action-btn:hover {
        border-color: #667eea;
        color: #667eea;
      }
      .table-pagination {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 16px 24px;
        border-top: 1px solid #dee2e6;
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
        background: #fff;
        cursor: pointer;
        font-size: 0.875rem;
        transition: all 0.2s;
      }
      .pagination-btn:hover:not(:disabled) {
        background: #667eea;
        color: white;
        border-color: #667eea;
      }
      .pagination-btn:disabled {
        opacity: 0.5;
        cursor: not-allowed;
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
      if (name === 'data' || name === 'columns') {
        this.parseData();
      } else if (name === 'page-size') {
        this.pageSize = parseInt(newValue);
      }
      this.render();
    }
  }

  parseData() {
    try {
      this.data = JSON.parse(this.getAttribute('data') || '[]');
      this.columns = JSON.parse(this.getAttribute('columns') || '[]');
    } catch (e) {
      this.data = [];
      this.columns = [];
    }
  }

  render() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    const pageData = this.data.slice(startIndex, endIndex);
    const totalPages = Math.ceil(this.data.length / this.pageSize);

    this.shadowRoot.innerHTML = `
      <style>${DataTable.styles}</style>
      <div class="data-table">
        <div class="table-header">
          <div class="table-title">📋 Recent Orders</div>
          <input type="text" class="table-search" placeholder="Search orders...">
        </div>
        <div class="table-wrapper">
          <table>
            <thead>
              <tr>
                ${this.columns.map(col => `<th>${col.label}</th>`).join('')}
              </tr>
            </thead>
            <tbody>
              ${pageData.map(row => `
                <tr>
                  ${this.columns.map(col => {
                    if (col.key === 'status') {
                      return `<td><span class="status-badge ${row[col.key]}">${row[col.key]}</span></td>`;
                    }
                    return `<td>${row[col.key] || '-'}</td>`;
                  }).join('')}
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
        <div class="table-pagination">
          <div class="pagination-info">
            Showing ${startIndex + 1} to ${Math.min(endIndex, this.data.length)} of ${this.data.length} entries
          </div>
          <div class="pagination-buttons">
            <button class="pagination-btn" ${this.currentPage === 1 ? 'disabled' : ''} data-action="prev">← Previous</button>
            <button class="pagination-btn" ${this.currentPage === totalPages ? 'disabled' : ''} data-action="next">Next →</button>
          </div>
        </div>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const prevBtn = this.shadowRoot.querySelector('[data-action="prev"]');
    const nextBtn = this.shadowRoot.querySelector('[data-action="next"]');

    prevBtn?.addEventListener('click', () => {
      if (this.currentPage > 1) {
        this.currentPage--;
        this.render();
      }
    });

    nextBtn?.addEventListener('click', () => {
      const totalPages = Math.ceil(this.data.length / this.pageSize);
      if (this.currentPage < totalPages) {
        this.currentPage++;
        this.render();
      }
    });
  }
}

class DashboardGrid extends HTMLElement {
  constructor() {
    super();
    this.layout = 'grid';
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .dashboard-grid {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 20px;
        padding: 20px;
      }
      .dashboard-grid.layout-4 {
        grid-template-columns: repeat(4, 1fr);
      }
      .dashboard-grid.layout-3 {
        grid-template-columns: repeat(3, 1fr);
      }
      .dashboard-grid.layout-2 {
        grid-template-columns: repeat(2, 1fr);
      }
      .dashboard-grid.layout-1 {
        grid-template-columns: 1fr;
      }
      @media (max-width: 1200px) {
        .dashboard-grid {
          grid-template-columns: repeat(2, 1fr);
        }
      }
      @media (max-width: 768px) {
        .dashboard-grid {
          grid-template-columns: 1fr;
        }
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue && name === 'layout') {
      this.layout = newValue;
      this.render();
    }
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${DashboardGrid.styles}</style>
      <div class="dashboard-grid layout-${this.layout}">
        <slot></slot>
      </div>
    `;
  }
}

class StatTrend extends HTMLElement {
  constructor() {
    super();
    this.value = 0;
    this.label = 'Trend';
    this.period = 'today';
  }

  static get observedAttributes() {
    return ['value', 'label', 'period'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .stat-trend {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        border-radius: 16px;
        padding: 24px;
        color: white;
      }
      .trend-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
      }
      .trend-icon {
        font-size: 2rem;
        opacity: 0.9;
      }
      .trend-period {
        background: rgba(255, 255, 255, 0.2);
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 0.75rem;
      }
      .trend-value {
        font-size: 2.5rem;
        font-weight: 700;
        margin: 16px 0 8px;
      }
      .trend-label {
        font-size: 0.875rem;
        opacity: 0.9;
      }
      .trend-chart {
        display: flex;
        align-items: flex-end;
        gap: 4px;
        height: 40px;
        margin-top: 16px;
      }
      .trend-bar {
        flex: 1;
        background: rgba(255, 255, 255, 0.3);
        border-radius: 4px 4px 0 0;
        transition: background 0.3s;
      }
      .trend-bar:hover {
        background: rgba(255, 255, 255, 0.5);
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = newValue;
      this.render();
    }
  }

  render() {
    const icon = this.getAttribute('icon') || '📈';
    const bars = Array.from({ length: 12 }, () => Math.random() * 100);

    this.shadowRoot.innerHTML = `
      <style>${StatTrend.styles}</style>
      <div class="stat-trend">
        <div class="trend-header">
          <div class="trend-icon">${icon}</div>
          <div class="trend-period">${this.period}</div>
        </div>
        <div class="trend-value">${this.value}</div>
        <div class="trend-label">${this.label}</div>
        <div class="trend-chart">
          ${bars.map(h => `<div class="trend-bar" style="height: ${h}%"></div>`).join('')}
        </div>
      </div>
    `;
  }
}

class SideNav extends HTMLElement {
  constructor() {
    super();
    this.items = [];
    this.collapsed = false;
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .side-nav {
        width: 260px;
        background: #212529;
        height: 100vh;
        position: fixed;
        left: 0;
        top: 0;
        transition: width 0.3s;
        z-index: 1000;
      }
      .side-nav.collapsed {
        width: 72px;
      }
      .nav-header {
        padding: 20px;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        display: flex;
        align-items: center;
        gap: 12px;
      }
      .nav-logo {
        font-size: 1.5rem;
      }
      .nav-brand {
        font-size: 1.25rem;
        font-weight: 700;
        color: white;
      }
      .collapsed .nav-brand {
        display: none;
      }
      .nav-items {
        padding: 12px;
      }
      .nav-item {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 14px 16px;
        border-radius: 8px;
        color: rgba(255, 255, 255, 0.7);
        cursor: pointer;
        transition: all 0.2s;
        margin-bottom: 4px;
      }
      .nav-item:hover {
        background: rgba(255, 255, 255, 0.1);
        color: white;
      }
      .nav-item.active {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
      }
      .nav-icon {
        font-size: 1.25rem;
        min-width: 24px;
        text-align: center;
      }
      .nav-label {
        font-size: 0.875rem;
        white-space: nowrap;
      }
      .collapsed .nav-label {
        display: none;
      }
      .nav-toggle {
        position: absolute;
        right: -12px;
        top: 50%;
        transform: translateY(-50%);
        width: 24px;
        height: 24px;
        border-radius: 50%;
        background: #667eea;
        border: none;
        color: white;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 0.75rem;
      }
      .collapsed .nav-toggle {
        transform: translateY(-50%) rotate(180deg);
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.items = JSON.parse(this.getAttribute('items') || '[]');
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue && name === 'items') {
      this.items = JSON.parse(newValue);
      this.render();
    }
  }

  render() {
    const defaultItems = [
      { icon: '📊', label: 'Dashboard', active: true },
      { icon: '📈', label: 'Analytics' },
      { icon: '🛒', label: 'Orders' },
      { icon: '👥', label: 'Customers' },
      { icon: '📦', label: 'Products', badge: 24 },
      { icon: '🎫', label: 'Coupons' },
      { icon: '📝', label: 'Reports' },
      { icon: '⚙️', label: 'Settings' },
    ];

    this.shadowRoot.innerHTML = `
      <style>${SideNav.styles}</style>
      <div class="side-nav ${this.collapsed ? 'collapsed' : ''}">
        <div class="nav-header">
          <div class="nav-logo">🛒</div>
          <div class="nav-brand">ShopAdmin</div>
        </div>
        <div class="nav-items">
          ${(this.items.length ? this.items : defaultItems).map((item, i) => `
            <div class="nav-item ${item.active ? 'active' : ''}" data-index="${i}">
              <div class="nav-icon">${item.icon}</div>
              <div class="nav-label">${item.label}</div>
            </div>
          `).join('')}
        </div>
        <button class="nav-toggle">◀</button>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const navItems = this.shadowRoot.querySelectorAll('.nav-item');
    navItems.forEach(item => {
      item.addEventListener('click', () => {
        navItems.forEach(i => i.classList.remove('active'));
        item.classList.add('active');
        const index = item.dataset.index;
        this.dispatchEvent(new CustomEvent('nav-click', {
          detail: { index },
          bubbles: true,
          composed: true,
        }));
      });
    });

    const toggle = this.shadowRoot.querySelector('.nav-toggle');
    toggle?.addEventListener('click', () => {
      this.collapsed = !this.collapsed;
      this.render();
    });
  }
}

export { MetricCard, ChartWidget, DataTable, DashboardGrid, StatTrend, SideNav };