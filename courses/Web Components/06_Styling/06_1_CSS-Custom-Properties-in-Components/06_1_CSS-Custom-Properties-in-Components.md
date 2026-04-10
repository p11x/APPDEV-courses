# CSS Custom Properties in Components

## OVERVIEW

CSS Custom Properties (CSS Variables) enable theming and customization in Web Components while maintaining encapsulation. This guide covers using, exposing, and managing CSS custom properties.

## TECHNICAL SPECIFICATIONS

### CSS Custom Property Features

| Feature | Description |
|---------|-------------|
| Inheritance | Properties inherit to children |
| Override | Can be set externally |
| Fallback | Default values when not set |
| Runtime | Can be changed dynamically |

## IMPLEMENTATION DETAILS

### Basic Custom Properties

```javascript
class CustomPropsElement extends HTMLElement {
  get styles() {
    return `
      <style>
        :host {
          /* Define customizable properties with defaults */
          --primary-color: #007bff;
          --secondary-color: #6c757d;
          --border-radius: 4px;
          --padding: 16px;
          --font-size: 14px;
        }
        
        .container {
          background: var(--primary-color);
          border-radius: var(--border-radius);
          padding: var(--padding);
          color: white;
          font-size: var(--font-size);
        }
      </style>
    `;
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = this.styles + '<div class="container"><slot></slot></div>';
  }
}
```

```css
/* External customization */
custom-props-element {
  --primary-color: #28a745;
  --border-radius: 8px;
  --padding: 24px;
}
```

### Exposed Properties API

```javascript
class ThemedElement extends HTMLElement {
  #properties = [
    '--bg-color',
    '--text-color', 
    '--accent-color',
    '--spacing',
    '--border-width'
  ];
  
  static get observedAttributes() { return this.#properties; }
  
  attributeChangedCallback(name, oldVal, newVal) {
    if (this.#properties.includes(name)) {
      this.style.setProperty(name, newVal);
    }
  }
  
  // Expose property for external setting
  connectedCallback() {
    this.#properties.forEach(prop => {
      const value = this.getAttribute(`style:${prop}`);
      if (value) {
        this.style.setProperty(prop, value);
      }
    });
  }
}
```

## NEXT STEPS

Proceed to **06_Styling/06_2_Scoped-Styling-Techniques**.