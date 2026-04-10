# Design System Implementation

## OVERVIEW

Design system implementation creates consistent UI across applications. This guide covers design tokens, component primitives, and theming systems.

## IMPLEMENTATION DETAILS

### Design Tokens

```javascript
const DesignTokens = {
  colors: {
    primary: '#007bff',
    primaryHover: '#0056b3',
    secondary: '#6c757d',
    success: '#28a745',
    danger: '#dc3545',
    warning: '#ffc107',
    info: '#17a2b8',
    light: '#f8f9fa',
    dark: '#343a40',
    background: '#ffffff',
    text: '#212529',
    textMuted: '#6c757d',
    border: '#dee2e6'
  },
  spacing: {
    xs: '4px',
    sm: '8px',
    md: '16px',
    lg: '24px',
    xl: '32px',
    xxl: '48px'
  },
  borderRadius: {
    sm: '2px',
    md: '4px',
    lg: '8px',
    round: '50%'
  },
  typography: {
    fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif',
    fontSize: {
      xs: '10px',
      sm: '12px',
      md: '14px',
      lg: '16px',
      xl: '20px',
      xxl: '24px'
    }
  },
  shadows: {
    sm: '0 1px 2px rgba(0,0,0,0.05)',
    md: '0 4px 6px rgba(0,0,0,0.1)',
    lg: '0 10px 20px rgba(0,0,0,0.15)'
  }
};
```

### Base Components

```javascript
class Button extends HTMLElement {
  static get observedAttributes() { return ['variant', 'size', 'disabled']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() { this.render(); }
  attributeChangedCallback() { this.render(); }
  
  get template() {
    const variant = this.getAttribute('variant') || 'primary';
    const size = this.getAttribute('size') || 'md';
    const colors = DesignTokens.colors;
    const colors = DesignTokens.colors;
    
    return `
      <style>
        :host { display: inline-block; }
        button {
          font-family: ${DesignTokens.typography.fontFamily};
          font-size: ${DesignTokens.typography.fontSize[size === 'sm' ? 'sm' : 'md']};
          padding: ${size === 'sm' ? '4px 12px' : '8px 16px'};
          border: none;
          border-radius: ${DesignTokens.borderRadius.md};
          cursor: pointer;
          background: ${colors[variant] || colors.primary};
          color: white;
        }
        button:disabled { opacity: 0.5; cursor: not-allowed; }
      </style>
      <button ?disabled="${this.hasAttribute('disabled')}">
        <slot></slot>
      </button>
    `;
  }
  
  render() { this.shadowRoot.innerHTML = this.template; }
}
customElements.define('ds-button', Button);
```

### Input Component

```javascript
class Input extends HTMLElement {
  static get formAssociated() { return true; }
  
  #internals = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#internals = this.attachInternals();
    this.render();
  }
  
  get template() {
    return `
      <style>
        :host { display: block; }
        input {
          width: 100%;
          padding: ${DesignTokens.spacing.sm} ${DesignTokens.spacing.md};
          border: 1px solid ${DesignTokens.colors.border};
          border-radius: ${DesignTokens.borderRadius.md};
          font-family: ${DesignTokens.typography.fontFamily};
          font-size: ${DesignTokens.typography.fontSize.md};
          box-sizing: border-box;
        }
        input:focus {
          outline: none;
          border-color: ${DesignTokens.colors.primary};
          box-shadow: 0 0 0 2px rgba(0,123,255,0.25);
        }
      </style>
      <input type="${this.getAttribute('type') || 'text'}" />
    `;
  }
  
  render() {
    this.shadowRoot.innerHTML = this.template;
    const input = this.shadowRoot.querySelector('input');
    input?.addEventListener('input', () => {
      this.#internals.setFormValue(input.value);
    });
  }
}
customElements.define('ds-input', Input);
```

## NEXT STEPS

Proceed to **11_Real-World-Applications/11_5_Micro-Frontend-Implementation**.