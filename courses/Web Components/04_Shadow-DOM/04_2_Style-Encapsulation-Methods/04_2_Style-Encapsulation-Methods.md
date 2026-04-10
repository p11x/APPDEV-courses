# Style Encapsulation Methods

## OVERVIEW

Style encapsulation is one of Shadow DOM's most powerful features, preventing styles from leaking into or out of components. This guide covers all styling methods including CSS custom properties, ::part, and constructable stylesheets.

## IMPLEMENTATION DETAILS

### Basic Style Encapsulation

```javascript
class EncapsulatedElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <style>
        /* These styles only apply within the component */
        :host {
          display: block;
          padding: 16px;
          background: white;
        }
        
        .inner {
          color: #333;
        }
        
        /* :host targets the custom element itself */
        :host([theme="dark"]) {
          background: #222;
          color: white;
        }
      </style>
      <div class="inner"><slot></slot></div>
    `;
  }
}
```

### CSS Custom Properties for Theming

```javascript
class ThemedElement extends HTMLElement {
  get template() {
    const t = document.createElement('template');
    t.innerHTML = `
      <style>
        :host {
          /* Use CSS custom properties with defaults */
          --bg-color: #fff;
          --text-color: #333;
          --padding: 16px;
          --border-radius: 4px;
          
          display: block;
          background: var(--bg-color);
          color: var(--text-color);
          padding: var(--padding);
          border-radius: var(--border-radius);
        }
      </style>
      <div class="content"><slot></slot></div>
    `;
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    const clone = this.template.content.cloneNode(true);
    this.shadowRoot.appendChild(clone);
  }
}
```

```css
/* External usage - override component defaults */
themed-element {
  --bg-color: #f5f5f5;
  --text-color: #666;
  --padding: 24px;
}
```

### Using ::part for External Styling

```javascript
class PartElement extends HTMLElement {
  get template() {
    const t = document.createElement('template');
    t.innerHTML = `
      <style>
        :host { display: block; }
        button {
          padding: 8px 16px;
          border: none;
          border-radius: 4px;
          cursor: pointer;
        }
      </style>
      <button part="primary">Primary</button>
      <button part="secondary">Secondary</button>
    `;
    return t;
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    const clone = this.template.content.cloneNode(true);
    this.shadowRoot.appendChild(clone);
  }
}
```

```css
/* External styling via ::part */
part-element::part(primary) {
  background: #007bff;
  color: white;
}

part-element::part(secondary) {
  background: #e9ecef;
  color: #333;
}
```

## Constructable Stylesheets

```javascript
class SheetElement extends HTMLElement {
  #sheet = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    // Create CSSStyleSheet
    this.#sheet = new CSSStyleSheet();
    this.#sheet.replace(`
      :host { display: block; }
      .content { padding: 16px; }
    `);
    
    // Add to shadow root
    this.shadowRoot.adoptedStyleSheets = [this.#sheet];
  }
}
```

## NEXT STEPS

Proceed to **04_Shadow-DOM/04_3_Slot-Content-Distribution-Mastery** for slot details.