# Performance Optimized Styling

## OVERVIEW

Optimizing styling improves rendering performance. This guide covers efficient CSS patterns, style caching, and performance best practices.

## IMPLEMENTATION DETAILS

### CSS Containment

```javascript
class ContainedElement extends HTMLElement {
  get styles() {
    return `
      <style>
        :host {
          /* content - only children's DOM affects layout */
          contain: content;
        }
        
        :host {
          /* layout - layout contained */
          contain: layout;
        }
        
        :host {
          /* style - style properties contained */
          contain: style;
        }
      </style>
      <slot></slot>
    `;
  }
}
```

### Efficient Updates

```javascript
class OptimizedStyleElement extends HTMLElement {
  #container = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <style>:host { display: block; }</style>
      <div class="container"></div>
    `;
    this.#container = this.shadowRoot.querySelector('.container');
  }
  
  updateContent(items) {
    // Use DocumentFragment for efficient updates
    const fragment = document.createDocumentFragment();
    
    items.forEach(item => {
      const div = document.createElement('div');
      div.textContent = item;
      fragment.appendChild(div);
    });
    
    this.#container.innerHTML = '';
    this.#container.appendChild(fragment);
  }
}
```

## NEXT STEPS

Proceed to **07_Forms/07_1_Form-Integration-Mastery**.