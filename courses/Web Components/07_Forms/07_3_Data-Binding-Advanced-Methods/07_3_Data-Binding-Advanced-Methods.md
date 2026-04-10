# Data Binding Advanced Methods

## OVERVIEW

Advanced data binding methods connect form elements with component state. This guide covers bidirectional binding, computed properties, and complex form structures.

## IMPLEMENTATION DETAILS

### Bidirectional Binding

```javascript
class BidirectionalInput extends HTMLElement {
  #internals = null;
  #value = '';
  #bound = false;
  
  static get formAssociated() { return true; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#internals = this.attachInternals();
    this.render();
    this.bindInput();
  }
  
  bindInput() {
    const input = this.shadowRoot.querySelector('input');
    if (input && !this.#bound) {
      this.#bound = true;
      input.addEventListener('input', (e) => {
        this.#value = e.target.value;
        this.#internals.setFormValue(this.#value);
        this.dispatchEvent(new Event('input', { bubbles: true }));
      });
    }
  }
  
  get value() { return this.#value; }
  set value(val) {
    this.#value = val;
    const input = this.shadowRoot.querySelector('input');
    if (input && input.value !== val) input.value = val;
    this.#internals?.setFormValue(val);
  }
  
  render() {
    this.shadowRoot.innerHTML = '<input type="text" />';
  }
}
```

## NEXT STEPS

Proceed to **07_Forms/07_4_Accessibility-in-Forms**.