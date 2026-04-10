# State Management Integration

## OVERVIEW

State management integration connects Web Components with external state systems. This guide covers patterns for Redux, MobX, and custom state solutions.

## IMPLEMENTATION DETAILS

### Redux Integration

```javascript
class ReduxConnected extends HTMLElement {
  #unsubscribe = null;
  #store = null;
  
  set store(store) {
    this.#store = store;
    this.#connect();
  }
  
  #connect() {
    this.#unsubscribe = this.#store.subscribe(() => {
      this.render();
    });
    this.render();
  }
  
  disconnectedCallback() {
    this.#unsubscribe?.();
  }
  
  getState() {
    return this.#store?.getState();
  }
  
  dispatch(action) {
    this.#store?.dispatch(action);
  }
  
  render() {
    const state = this.getState();
    this.shadowRoot.innerHTML = `<div>State: ${JSON.stringify(state)}</div>`;
  }
}
```

### Context Integration

```javascript
class ContextConsumer extends HTMLElement {
  #context = null;
  #unsubscribe = null;
  
  static get observedAttributes() { return ['context-name']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    const name = this.getAttribute('context-name');
    this.#context = window.getContext(name);
    if (this.#context) {
      this.#unsubscribe = this.#context.subscribe(v => this.render(v));
      this.render(this.#context.value);
    }
  }
  
  disconnectedCallback() {
    this.#unsubscribe?.();
  }
  
  render(value) {
    this.shadowRoot.innerHTML = `<div>Context: ${JSON.stringify(value)}</div>`;
  }
}
```

## NEXT STEPS

Proceed to **06_Styling/06_1_CSS-Custom-Properties-in-Components**.