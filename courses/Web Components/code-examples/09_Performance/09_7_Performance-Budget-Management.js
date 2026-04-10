/**
 * Performance Budget Management - Budget tracking for web components
 * @module performance/09_7_Performance-Budget-Management
 * @version 1.0.0
 * @example <budget-manager></budget-manager>
 */

class BudgetManager extends HTMLElement {
  constructor() {
    super();
    this._budgets = new Map();
    this._violations = [];
    this._history = [];
    this._maxHistory = 100;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this._initBudgets();
    this._render();
    this._startMonitoring();
  }

  _initBudgets() {
    this._budgets.set('bundleSize', new PerformanceBudget('bundleSize', 100, 'KB', 50));
    this._budgets.set('firstContentfulPaint', new PerformanceBudget('firstContentfulPaint', 1500, 'ms', 1000));
    this._budgets.set('timeToInteractive', new PerformanceBudget('timeToInteractive', 3000, 'ms', 2000));
    this._budgets.set('largestContentfulPaint', new PerformanceBudget('largestContentfulPaint', 2500, 'ms', 1500));
    this._budgets.set('cumulativeLayoutShift', new PerformanceBudget('cumulativeLayoutShift', 0.1, '', 0.05));
    this._budgets.set('firstInputDelay', new PerformanceBudget('firstInputDelay', 100, 'ms', 50));
    this._budgets.set('totalBlockingTime', new PerformanceBudget('totalBlockingTime', 200, 'ms', 100));
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
          font-family: system-ui, sans-serif;
        }
        .budget-item {
          display: flex;
          justify-content: space-between;
          align-items: center;
          padding: 12px;
          margin: 8px 0;
          background: #f5f5f5;
          border-radius: 8px;
        }
        .budget-info {
          flex: 1;
        }
        .budget-name {
          font-weight: 500;
          margin-bottom: 4px;
        }
        .budget-limits {
          font-size: 12px;
          color: #666;
        }
        .budget-bar {
          width: 120px;
          height: 8px;
          background: #e0e0e0;
          border-radius: 4px;
          overflow: hidden;
          margin-left: 12px;
        }
        .budget-bar-fill {
          height: 100%;
          border-radius: 4px;
          transition: width 0.3s, background-color 0.3s;
        }
        .violation {
          padding: 12px;
          margin: 8px 0;
          background: #ffebee;
          border-radius: 8px;
          border-left: 4px solid #f44336;
        }
        .violation.over {
          background: #f44336;
          color: white;
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
        .status-badge {
          display: inline-block;
          padding: 2px 8px;
          border-radius: 4px;
          font-size: 12px;
          font-weight: 500;
        }
        .status-badge.over-budget {
          background: #f44336;
          color: white;
        }
        .status-badge.near-budget {
          background: #ff9800;
          color: white;
        }
        .status-badge.on-track {
          background: #4caf50;
          color: white;
        }
      </style>
      <div class="container">
        <h3>Performance Budget Manager</h3>
        <div class="budgets-list" id="budgets-list"></div>
        <div class="violations-section" id="violations-section"></div>
        <div style="margin-top: 12px;">
          <button id="check-budgets">Check Budgets</button>
          <button id="reset-violations">Reset Violations</button>
          <button id="export-budgets">Export Budgets</button>
        </div>
      </div>
    `;
  }

  _startMonitoring() {
    this._setupControls();
    this._checkBudgets();
  }

  _setupControls() {
    this.shadowRoot.getElementById('check-budgets')?.addEventListener('click', () => {
      this._checkBudgets();
    });

    this.shadowRoot.getElementById('reset-violations')?.addEventListener('click', () => {
      this._violations = [];
      this._updateDisplay();
    });

    this.shadowRoot.getElementById('export-budgets')?.addEventListener('click', () => {
      const data = this.getBudgetData();
      console.log('Budget Data:', JSON.stringify(data, null, 2));
    });
  }

  _checkBudgets() {
    this._budgets.forEach((budget, name) => {
      const currentValue = this._getCurrentValue(name);
      budget.check(currentValue);
      
      if (budget.isOver()) {
        this._recordViolation(name, currentValue, budget);
      }
    });

    this._updateDisplay();
  }

  _getCurrentValue(metricName) {
    const metrics = {
      bundleSize: this._getBundleSize(),
      firstContentfulPaint: this._getPerformanceMetric('paint', 'first-contentful-paint'),
      timeToInteractive: this._getInteractionMetric('TTI'),
      largestContentfulPaint: this._getPerformanceMetric('paint', 'largest-contentful-paint'),
      cumulativeLayoutShift: this._getPerformanceMetric('layout-shift'),
      firstInputDelay: this._getPerformanceMetric('event', 'first-input'),
      totalBlockingTime: this._getPerformanceMetric('longtask')
    };

    return metrics[metricName] || 0;
  }

  _getBundleSize() {
    const scripts = Array.from(document.scripts);
    return scripts.reduce((total, script) => {
      return total + (script.src ? 100 : 10);
    }, 0);
  }

  _getPerformanceMetric(entryType, name) {
    try {
      const entries = performance.getEntriesByType(entryType);
      const entry = entries.find(e => e.name.includes(name));
      return entry?.startTime || 0;
    } catch {
      return Math.random() * 1000;
    }
  }

  _getInteractionMetric(metric) {
    try {
      return performance.getEntriesByType('navigation')[0]?.[metric.toLowerCase()] || 0;
    } catch {
      return Math.random() * 2000 + 1000;
    }
  }

  _recordViolation(name, value, budget) {
    const violation = {
      name,
      value,
      limit: budget.fixedLimit,
      unit: budget.unit,
      timestamp: Date.now()
    };

    this._violations.push(violation);
    this._history.push({ ...violation, type: 'violation' });

    if (this._history.length > this._maxHistory) {
      this._history.shift();
    }
  }

  _updateDisplay() {
    const budgetsList = this.shadowRoot.getElementById('budgets-list');
    const violationsSection = this.shadowRoot.getElementById('violations-section');

    if (!budgetsList || !violationsSection) return;

    budgetsList.innerHTML = '';
    this._budgets.forEach((budget, name) => {
      const item = document.createElement('div');
      item.className = 'budget-item';

      const percentage = (budget.currentValue / budget.fixedLimit) * 100;
      const statusClass = budget.isOver() ? 'over-budget' : percentage > 80 ? 'near-budget' : 'on-track';
      const statusText = budget.isOver() ? 'Over' : percentage > 80 ? 'Near' : 'On Track';

      item.innerHTML = `
        <div class="budget-info">
          <div class="budget-name">${budget.name}</div>
          <div class="budget-limits">Limit: ${budget.fixedLimit}${budget.unit} | Target: ${budget.targetLimit}${budget.unit}</div>
        </div>
        <span class="status-badge ${statusClass}">${statusText}</span>
        <div class="budget-bar">
          <div class="budget-bar-fill" style="width: ${Math.min(percentage, 100)}%; background-color: ${budget.isOver() ? '#f44336' : percentage > 80 ? '#ff9800' : '#4caf50'}"></div>
        </div>
      `;
      budgetsList.appendChild(item);
    });

    violationsSection.innerHTML = '<h4>Violations</h4>';
    this._violations.slice(-5).reverse().forEach(v => {
      const el = document.createElement('div');
      el.className = 'violation';
      el.textContent = `${v.name}: ${v.value}${v.unit} exceeded limit of ${v.limit}${v.unit}`;
      violationsSection.appendChild(el);
    });
  }

  setBudget(name, limit, target) {
    const budget = this._budgets.get(name);
    if (budget) {
      budget.fixedLimit = limit;
      budget.targetLimit = target;
    }
  }

  getBudgetData() {
    return {
      budgets: Array.from(this._budgets.entries()).map(([name, budget]) => ({
        name,
        current: budget.currentValue,
        limit: budget.fixedLimit,
        target: budget.targetLimit,
        unit: budget.unit,
        status: budget.isOver() ? 'over' : budget.isNear() ? 'near' : 'ok'
      })),
      violations: this._violations,
      history: this._history
    };
  }

  disconnectCallback() {
    this._violations = [];
  }
}

class PerformanceBudget {
  constructor(name, fixedLimit, unit, targetLimit = 0) {
    this.name = name;
    this.fixedLimit = fixedLimit;
    this.unit = unit;
    this.targetLimit = targetLimit || fixedLimit * 0.8;
    this.currentValue = 0;
  }

  check(value) {
    this.currentValue = value;
  }

  isOver() {
    return this.currentValue > this.fixedLimit;
  }

  isNear() {
    return this.currentValue > this.targetLimit && !this.isOver();
  }
}

export { BudgetManager, PerformanceBudget };