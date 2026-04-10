# Dynamic Template Generation

## OVERVIEW

Dynamic template generation enables components to create and modify template content programmatically. This guide covers techniques for generating templates based on configuration, dynamic data, and runtime conditions.

## IMPLEMENTATION DETAILS

### Programmatic Template Creation

```javascript
class DynamicTemplateElement extends HTMLElement {
  #config = {};
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  set config(value) {
    this.#config = value;
    this.render();
  }
  
  render() {
    const template = this.#generateTemplate(this.#config);
    const clone = template.content.cloneNode(true);
    this.shadowRoot.innerHTML = '';
    this.shadowRoot.appendChild(clone);
  }
  
  #generateTemplate(config) {
    const template = document.createElement('template');
    
    let html = '<style>';
    html += config.styles || '';
    html += '</style>';
    html += '<div class="container">';
    
    if (config.showHeader) {
      html += `<h1>${config.title || 'Default'}</h1>`;
    }
    
    html += '<slot></slot>';
    html += '</div>';
    
    template.innerHTML = html;
    return template;
  }
}
```

### Conditional Template Generation

```javascript
class ConditionalTemplate extends HTMLElement {
  static get observedAttributes() {
    return ['mode', 'theme'];
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  attributeChangedCallback() {
    this.render();
  }
  
  render() {
    const mode = this.getAttribute('mode') || 'view';
    const theme = this.getAttribute('theme') || 'light';
    
    let template;
    if (mode === 'edit') {
      template = this.#getEditTemplate(theme);
    } else {
      template = this.#getViewTemplate(theme);
    }
    
    const clone = template.content.cloneNode(true);
    this.shadowRoot.innerHTML = '';
    this.shadowRoot.appendChild(clone);
  }
  
  #getEditTemplate(theme) {
    const t = document.createElement('template');
    t.innerHTML = `
      <style>:host { display: block; }</style>
      <input type="text" class="edit-mode theme-${theme}" />
    `;
    return t;
  }
  
  #getViewTemplate(theme) {
    const t = document.createElement('template');
    t.innerHTML = `
      <style>:host { display: block; }</style>
      <div class="view-mode theme-${theme}"><slot></slot></div>
    `;
    return t;
  }
}
```

## NEXT STEPS

Proceed to **03_Templates/03_5_Template-Caching-Strategies** for optimization.