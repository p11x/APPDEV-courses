# Accessibility in Forms

## OVERVIEW

Form accessibility ensures components work with assistive technologies. This guide covers ARIA in forms, keyboard navigation, screen reader support, and error announcements.

## IMPLEMENTATION DETAILS

### Accessible Form Elements

```javascript
class AccessibleFormElement extends HTMLElement {
  static get formAssociated() { return true; }
  
  #internals = null;
  #errorId = '';
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.#errorId = `error-${Math.random().toString(36).substr(2, 9)}`;
  }
  
  connectedCallback() {
    this.#internals = this.attachInternals();
    this.render();
    this.setupValidation();
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        .error { color: red; font-size: 12px; }
        input[aria-invalid="true"] { border-color: red; }
      </style>
      <label>
        <span>Enter value:</span>
        <input 
          type="text" 
          aria-describedby="${this.#errorId}"
          aria-invalid="false" 
        />
      </label>
      <div id="${this.#errorId}" class="error" aria-live="polite"></div>
    `;
  }
  
  showError(message) {
    const error = this.shadowRoot.getElementById(this.#errorId);
    const input = this.shadowRoot.querySelector('input');
    
    if (message) {
      error.textContent = message;
      input.setAttribute('aria-invalid', 'true');
    } else {
      error.textContent = '';
      input.setAttribute('aria-invalid', 'false');
    }
  }
}
```

## NEXT STEPS

Proceed to **07_Forms/07_5_Custom-Input-Element-Development**.