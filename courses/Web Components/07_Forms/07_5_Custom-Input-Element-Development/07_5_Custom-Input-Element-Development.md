# Custom Input Element Development

## OVERVIEW

Custom input elements extend form capabilities beyond standard inputs. This guide covers text inputs, selects, checkboxes, and complex custom inputs.

## IMPLEMENTATION DETAILS

### Custom Text Input

```javascript
class CustomTextInput extends HTMLElement {
  static get formAssociated() { return true; }
  
  #internals = null;
  #type = 'text';
  
  static get observedAttributes() { return ['type', 'placeholder', 'disabled']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#internals = this.attachInternals();
    this.#type = this.getAttribute('type') || 'text';
    this.render();
  }
  
  attributeChangedCallback() { this.render(); }
  
  get value() {
    return this.shadowRoot.querySelector('input')?.value || '';
  }
  
  set value(val) {
    const input = this.shadowRoot.querySelector('input');
    if (input) input.value = val;
    this.#internals?.setFormValue(val);
  }
  
  render() {
    const type = this.#type;
    const placeholder = this.getAttribute('placeholder') || '';
    const disabled = this.hasAttribute('disabled');
    
    this.shadowRoot.innerHTML = `
      <style>
        :host { display: block; }
        input { 
          padding: 8px; 
          border: 1px solid #ccc; 
          border-radius: 4px; 
          width: 100%;
        }
        input:disabled { opacity: 0.5; }
      </style>
      <input 
        type="${type}" 
        placeholder="${placeholder}"
        ?disabled="${disabled}"
      />
    `;
  }
}
```

## NEXT STEPS

Proceed to **08_Interoperability/08_1_Framework-Neutral-Patterns**.