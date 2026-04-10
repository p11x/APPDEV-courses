# Analytics Dashboard Components

## OVERVIEW

Analytics dashboard components visualize data and metrics in interactive dashboards. This guide covers chart components, data tables, and dashboard layouts.

## IMPLEMENTATION DETAILS

### Chart Component

```javascript
class ChartComponent extends HTMLElement {
  #chartType = 'bar';
  #data = [];
  
  static get observedAttributes() { return ['type', 'data']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
    this.drawChart();
  }
  
  drawChart() {
    const canvas = this.shadowRoot.querySelector('canvas');
    const ctx = canvas.getContext('2d');
    // Chart drawing logic
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        canvas { width: 100%; height: 300px; }
      </style>
      <canvas></canvas>
    `;
  }
}
```

### Data Table Component

```javascript
class DataTable extends HTMLElement {
  #columns = [];
  #rows = [];
  
  static get observedAttributes() { return ['columns', 'rows']; }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        table { width: 100%; border-collapse: collapse; }
        th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }
      </style>
      <table>
        <thead>
          <tr>${this.#columns.map(c => `<th>${c.label}</th>`).join('')}</tr>
        </thead>
        <tbody>
          ${this.#rows.map(row => `
            <tr>${this.#columns.map(c => `<td>${row[c.key]}</td>`).join('')}</tr>
          `).join('')}
        </tbody>
      </table>
    `;
  }
}
```

## NEXT STEPS

Proceed to `12_Tooling/12_6_Component-Publishing-Guide.md`.