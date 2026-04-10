# Async Data Patterns

## OVERVIEW

Handling asynchronous data in Web Components requires patterns for loading states, error handling, and data fetching. This guide covers async/await patterns, data fetching, and state management for async operations.

## IMPLEMENTATION DETAILS

### Async Connected Callback

```javascript
class AsyncDataElement extends HTMLElement {
  #loading = false;
  #error = null;
  #data = null;
  
  static get observedAttributes() { return ['src']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.loadData();
  }
  
  async loadData() {
    const src = this.getAttribute('src');
    if (!src) return;
    
    this.#loading = true;
    this.render();
    
    try {
      const response = await fetch(src);
      this.#data = await response.json();
      this.#error = null;
    } catch (e) {
      this.#error = e.message;
    } finally {
      this.#loading = false;
      this.render();
    }
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      ${this.#loading ? '<div class="loading">Loading...</div>' : ''}
      ${this.#error ? `<div class="error">${this.#error}</div>` : ''}
      ${this.#data ? `<div class="data">${JSON.stringify(this.#data)}</div>` : ''}
    `;
  }
}
```

### Loading States

```javascript
class LoadingStateElement extends HTMLElement {
  #state = 'idle'; // idle, loading, success, error
  #data = null;
  #error = null;
  
  setState(state, data = null, error = null) {
    this.#state = state;
    this.#data = data;
    this.#error = error;
    this.render();
  }
}
```

## NEXT STEPS

Proceed to `06_Styling/06_6_CSS-in-JS-Alternative-Methods.md`.