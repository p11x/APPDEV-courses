# Cross-Shadow DOM Communication

## OVERVIEW

Communication between components inside and outside Shadow DOM requires specific patterns due to encapsulation boundaries. This guide covers methods for crossing the shadow boundary safely and effectively.

## IMPLEMENTATION DETAILS

### Event-Based Communication

```javascript
class CrossShadowElement extends HTMLElement {
  connectedCallback() {
    this.render();
    this.setupListeners();
  }
  
  setupListeners() {
    // Listen to internal events and re-dispatch
    const button = this.shadowRoot.querySelector('button');
    button.addEventListener('click', (e) => {
      // Cross the shadow boundary with composed: true
      this.dispatchEvent(new CustomEvent('element-action', {
        bubbles: true,
        composed: true,  // Important: allows crossing shadow boundary
        detail: { action: 'click' }
      }));
    });
  }
}
```

### Property-Based Communication

```javascript
class ParentElement extends HTMLElement {
  #child = null;
  
  connectedCallback() {
    this.render();
    this.#child = this.querySelector('child-element');
  }
  
  communicateWithChild(data) {
    if (this.#child) {
      this.#child.receiveData(data);
    }
  }
}
```

## NEXT STEPS

Proceed to `05_Data-Binding/05_6_Data-Validation-in-Components.md`.