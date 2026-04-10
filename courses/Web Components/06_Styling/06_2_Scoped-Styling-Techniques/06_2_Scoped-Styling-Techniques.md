# Scoped Styling Techniques

## OVERVIEW

Scoped styling ensures component styles don't affect external elements and external styles don't break components. This guide covers Shadow DOM styling and best practices.

## IMPLEMENTATION DETAILS

### Shadow DOM Scoping

```javascript
class ScopedStyleElement extends HTMLElement {
  get template() {
    const t = document.createElement('template');
    t.innerHTML = `
      <style>
        /* These styles are isolated to this component */
        :host {
          display: block;
          padding: 16px;
        }
        
        .inner {
          color: #333;
          font-size: 14px;
        }
        
        /* :host targets the custom element */
        :host([theme="dark"]) {
          background: #222;
        }
        
        :host([theme="dark"]) .inner {
          color: #fff;
        }
        
        /* ::slotted styles affect distributed content */
        ::slotted(*) {
          color: inherit;
        }
      </style>
      <div class="inner"><slot></slot></div>
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

### External Style Blocking

```javascript
class BlockedStyleElement extends HTMLElement {
  // External styles cannot reach inside
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    // Using :host prevents external styles
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          /* This always wins over external styles */
          display: block !important;
        }
      </style>
      <div class="content"><slot></slot></div>
    `;
  }
}
```

## NEXT STEPS

Proceed to **06_Styling/06_3_Theme-Integration-Patterns**.