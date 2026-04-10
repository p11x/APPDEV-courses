# Framework-Neutral Patterns

## OVERVIEW

Framework-neutral patterns ensure Web Components work across any JavaScript framework or vanilla JS. This guide covers creating universally compatible components.

## IMPLEMENTATION DETAILS

### Framework-Agnostic Component

```javascript
class AgnosticElement extends HTMLElement {
  #props = new Map();
  
  static get observedAttributes() { return ['value', 'disabled', 'variant']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  attributeChangedCallback(name, oldVal, newVal) {
    this.#props.set(name, newVal);
    this.render();
  }
  
  // Props work with any framework
  get value() { return this.getAttribute('value'); }
  set value(val) { this.setAttribute('value', val); }
  
  get disabled() { return this.hasAttribute('disabled'); }
  set disabled(val) { val ? this.setAttribute('disabled', '') : this.removeAttribute('disabled'); }
  
  connectedCallback() {
    this.render();
  }
  
  render() {
    const { value, disabled, variant } = Object.fromEntries(this.#props);
    this.shadowRoot.innerHTML = `
      <style>
        :host { display: block; }
        button { padding: 8px 16px; }
      </style>
      <button ?disabled="${disabled}">${value || 'Click'}</button>
    `;
  }
}
```

## NEXT STEPS

Proceed to **08_Interoperability/08_2_React-Integration-Guide**.