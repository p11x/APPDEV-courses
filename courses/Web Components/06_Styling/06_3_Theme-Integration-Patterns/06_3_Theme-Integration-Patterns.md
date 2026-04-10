# Theme Integration Patterns

## OVERVIEW

Theme integration enables consistent styling across components and applications. This guide covers theme systems, dark mode, and dynamic theming.

## IMPLEMENTATION DETAILS

### Theme System

```javascript
class ThemedComponent extends HTMLElement {
  #themes = {
    light: {
      '--bg': '#ffffff',
      '--text': '#333333',
      '--border': '#cccccc'
    },
    dark: {
      '--bg': '#222222',
      '--text': '#ffffff',
      '--border': '#444444'
    }
  };
  
  static get observedAttributes() { return ['theme']; }
  
  attributeChangedCallback(name, oldVal, newVal) {
    if (name === 'theme') {
      this.#applyTheme(newVal);
    }
  }
  
  #applyTheme(theme) {
    const vars = this.#themes[theme] || this.#themes.light;
    Object.entries(vars).forEach(([key, value]) => {
      this.style.setProperty(key, value);
    });
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#applyTheme(this.getAttribute('theme') || 'light');
    this.render();
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          background: var(--bg);
          color: var(--text);
          border: 1px solid var(--border);
        }
      </style>
      <slot></slot>
    `;
  }
}
```

### CSS Custom Properties Inheritance

```javascript
// In main CSS
:root {
  --theme-primary: #007bff;
  --theme-secondary: #6c757d;
  --theme-success: #28a745;
  --theme-danger: #dc3545;
}

/* Component automatically inherits */
class InheritThemeElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          color: var(--theme-primary);
        }
      </style>
      <div>Uses inherited theme</div>
    `;
  }
}
```

## NEXT STEPS

Proceed to **06_Styling/06_4_Dynamic-Styling-Methods**.