# Template Cloning and Instantiation

## OVERVIEW

Template cloning is the process of creating deep copies of template content for use in components. Understanding the nuances of template cloning and instantiation enables you to create efficient component rendering systems that minimize DOM operations and maximize performance.

This guide covers all aspects of template cloning, from basic cloneNode usage to advanced patterns for dynamic content, partial updates, and efficient re-rendering strategies.

## IMPLEMENTATION DETAILS

### Basic Cloning

```javascript
class CloneElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    // Get template
    const template = document.getElementById('my-template');
    // Clone content
    const clone = template.content.cloneNode(true);
    // Append to shadow DOM
    this.shadowRoot.appendChild(clone);
  }
}
```

### DocumentFragment Usage

```javascript
class FragmentElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    // Create DocumentFragment
    const fragment = document.createDocumentFragment();
    
    // Build content in fragment
    for (let i = 0; i < 5; i++) {
      const div = document.createElement('div');
      div.className = 'item';
      div.textContent = `Item ${i}`;
      fragment.appendChild(div);
    }
    
    // Single DOM operation
    this.shadowRoot.appendChild(fragment);
  }
}
```

## CODE EXAMPLES

### Clone with Modifications

```javascript
class ModifiedCloneElement extends HTMLElement {
  static get observedAttributes() {
    return ['variant', 'size'];
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
  }
  
  attributeChangedCallback(name, oldVal, newVal) {
    if (oldVal !== newVal) {
      this.render();
    }
  }
  
  render() {
    const template = document.getElementById('styled-template');
    const clone = template.content.cloneNode(true);
    
    // Modify cloned content
    const variant = this.getAttribute('variant') || 'default';
    const size = this.getAttribute('size') || 'medium';
    
    clone.querySelector('.container').classList.add(`variant-${variant}`);
    clone.querySelector('.container').classList.add(`size-${size}`);
    
    this.shadowRoot.innerHTML = '';
    this.shadowRoot.appendChild(clone);
  }
}
```

### Partial Template Cloning

```javascript
class PartialCloneElement extends HTMLElement {
  #templateParts = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.#extractParts();
  }
  
  #extractParts() {
    const template = document.getElementById('partial-template');
    this.#templateParts = {
      header: template.content.querySelector('.header').cloneNode(true),
      body: template.content.querySelector('.body').cloneNode(true),
      footer: template.content.querySelector('.footer').cloneNode(true)
    };
  }
  
  render(mode = 'full') {
    const fragment = document.createDocumentFragment();
    
    if (mode !== 'footer-only') {
      fragment.appendChild(this.#templateParts.header.cloneNode(true));
    }
    
    if (mode !== 'header-only') {
      fragment.appendChild(this.#templateParts.body.cloneNode(true));
    }
    
    if (mode !== 'body-only') {
      fragment.appendChild(this.#templateParts.footer.cloneNode(true));
    }
    
    this.shadowRoot.innerHTML = '';
    this.shadowRoot.appendChild(fragment);
  }
}
```

## BEST PRACTICES

```javascript
// Pre-parse templates for performance
const TemplateCache = {
  templates: new Map(),
  
  get(id) {
    if (!this.templates.has(id)) {
      const el = document.getElementById(id);
      this.templates.set(id, el.content.cloneNode(true));
    }
    return this.templates.get(id).cloneNode(true);
  }
};
```

## NEXT STEPS

Proceed to **03_Templates/03_3_Safe-HTML-Parsing-Methods** for security considerations.