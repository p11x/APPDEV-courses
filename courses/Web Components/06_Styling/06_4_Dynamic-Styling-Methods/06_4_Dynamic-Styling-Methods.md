# Dynamic Styling Methods

## OVERVIEW

Dynamic styling allows runtime style changes based on state or user interaction. This guide covers JavaScript-based styling, CSS variable manipulation, and runtime theme switching.

## IMPLEMENTATION DETAILS

### Runtime Style Changes

```javascript
class DynamicStyleElement extends HTMLElement {
  #state = { variant: 'default', size: 'medium' };
  
  static get observedAttributes() { return ['variant', 'size']; }
  
  attributeChangedCallback(name, oldVal, newVal) {
    this.#state[name] = newVal;
    this.#updateStyles();
  }
  
  #updateStyles() {
    const { variant, size } = this.#state;
    
    // Update CSS variables based on state
    const colors = {
      default: '#333',
      primary: '#007bff',
      success: '#28a745',
      danger: '#dc3545'
    };
    
    const sizes = {
      small: '12px',
      medium: '14px',
      large: '18px'
    };
    
    this.style.setProperty('--color', colors[variant]);
    this.style.setProperty('--font-size', sizes[size]);
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
    this.#updateStyles();
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          color: var(--color, #333);
          font-size: var(--font-size, 14px);
        }
      </style>
      <div><slot></slot></div>
    `;
  }
}
```

### Dynamic Style Injection

```javascript
class DynamicInjectElement extends HTMLElement {
  #sheet = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  #injectStyles(css) {
    const style = document.createElement('style');
    style.textContent = css;
    this.shadowRoot.appendChild(style);
  }
  
  #updateRules(updates) {
    if (!this.#sheet) {
      this.#sheet = new CSSStyleSheet();
      this.shadowRoot.adoptedStyleSheets = [this.#sheet];
    }
    
    // Update specific rules
    try {
      this.#sheet.replace(updates);
    } catch (e) {
      console.error('Invalid CSS:', e);
    }
  }
}
```

## NEXT STEPS

Proceed to **06_Styling/06_5_Performance-Optimized-Styling**.