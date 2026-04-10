# Component Communication Patterns

## OVERVIEW

Component communication patterns define how Web Components interact with each other and with the broader application. This guide covers events, properties, and shared state patterns.

## IMPLEMENTATION DETAILS

### Event-Based Communication

```javascript
class ParentComponent extends HTMLElement {
  connectedCallback() {
    this.addEventListener('child-action', this.#handleChildAction);
  }
  
  #handleChildAction(e) {
    console.log('Received:', e.detail);
    // Process and potentially respond
  }
}

class ChildComponent extends HTMLElement {
  #dispatch(action, data) {
    this.dispatchEvent(new CustomEvent(action, {
      bubbles: true,
      composed: true,
      detail: data
    }));
  }
}
```

### Property-Based Communication

```javascript
class DataSourceComponent extends HTMLElement {
  #data = [];
  
  setData(data) {
    this.#data = data;
    this.render();
    // Notify dependents
    this.dispatchEvent(new CustomEvent('data-changed', {
      detail: { data: this.#data },
      bubbles: true,
      composed: true
    }));
  }
  
  getData() {
    return this.#data;
  }
}
```

## NEXT STEPS

Proceed to `08_Interoperability/08_8_State-Management-Cross-Framework.md`.