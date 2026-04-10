# CSS-in-JS Alternative Methods

## OVERVIEW

While Shadow DOM provides style encapsulation, there are scenarios where CSS-in-JS patterns are useful. This guide covers alternatives to traditional CSS-in-JS libraries that work well with Web Components.

## IMPLEMENTATION DETAILS

### Constructable Stylesheets

```javascript
class StyleSheetElement extends HTMLElement {
  #sheet = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    // Create CSSStyleSheet programmatically
    this.#sheet = new CSSStyleSheet();
    this.#sheet.replace(`
      :host {
        display: block;
        background: var(--bg-color, #fff);
      }
      .container {
        padding: 16px;
      }
    `);
    
    // Apply to shadow root
    this.shadowRoot.adoptedStyleSheets = [this.#sheet];
  }
  
  // Dynamic style updates
  updateVariable(name, value) {
    this.#sheet.insertRule(`:host { --${name}: ${value}; }`);
  }
}
```

### Dynamic Style Generation

```javascript
class DynamicStyleElement extends HTMLElement {
  generateStyles(theme) {
    return `
      <style>
        :host {
          --primary: ${theme.primary};
          --secondary: ${theme.secondary};
          --background: ${theme.background};
        }
        .content {
          background: var(--primary);
          color: var(--background);
        }
      </style>
    `;
  }
}
```

## NEXT STEPS

Proceed to `06_Styling/06_7_Animation-and-Transition-Patterns.md`.