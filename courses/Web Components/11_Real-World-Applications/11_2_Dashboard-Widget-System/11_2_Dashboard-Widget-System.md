# Dashboard Widget System

## OVERVIEW

Dashboard widget systems enable customizable user interfaces. This guide covers widget containers, data binding, and real-time updates.

## IMPLEMENTATION DETAILS

### Widget Container

```javascript
class DashboardWidget extends HTMLElement {
  #config = {};
  #data = null;
  
  static get observedAttributes() { return ['type', 'title', 'refresh']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#init();
  }
  
  async #init() {
    this.#config = {
      type: this.getAttribute('type'),
      title: this.getAttribute('title') || 'Widget',
      refresh: parseInt(this.getAttribute('refresh') || '0')
    };
    
    this.render();
    if (this.#config.refresh > 0) {
      this.#startRefresh();
    }
  }
  
  async #startRefresh() {
    setInterval(() => this.#fetchData(), this.#config.refresh * 1000);
  }
  
  async #fetchData() {
    // Fetch based on type
    this.render();
  }
  
  get template() {
    return `
      <style>
        :host { display: block; }
        .widget {
          background: white;
          border-radius: 8px;
          box-shadow: 0 2px 4px rgba(0,0,0,0.1);
          padding: 16px;
        }
        .header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 16px;
        }
        .title { font-size: 18px; font-weight: 600; }
        .content { min-height: 100px; }
      </style>
      <div class="widget">
        <div class="header">
          <span class="title">${this.#config.title}</span>
          <button class="refresh">⟳</button>
        </div>
        <div class="content">${this.getWidgetContent()}</div>
      </div>
    `;
  }
  
  getWidgetContent() {
    return '<p>Loading...</p>';
  }
  
  render() {
    this.shadowRoot.innerHTML = this.template;
    this.shadowRoot.querySelector('.refresh').addEventListener('click', () => this.#fetchData());
  }
}
customElements.define('dashboard-widget', DashboardWidget);
```

### Chart Widget

```javascript
class ChartWidget extends DashboardWidget {
  getWidgetContent() {
    return '<div class="chart">Chart visualization</div>';
  }
}
customElements.define('chart-widget', ChartWidget);
```

## NEXT STEPS

Proceed to **11_Real-World-Applications/11_3_Form-Validation-Framework**.