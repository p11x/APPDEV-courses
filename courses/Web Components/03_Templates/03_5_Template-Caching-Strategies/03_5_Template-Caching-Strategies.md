# Template Caching Strategies

## OVERVIEW

Template caching improves performance by storing parsed templates and avoiding repeated parsing. This guide covers caching strategies for single templates, template registries, and component-specific caches.

## IMPLEMENTATION DETAILS

### Static Cache Pattern

```javascript
class CachedTemplateElement extends HTMLElement {
  // Static template storage
  static #template = null;
  
  static get template() {
    if (!CachedTemplateElement.#template) {
      const t = document.createElement('template');
      t.innerHTML = `
        <style>
          :host { display: block; padding: 16px; }
        </style>
        <div class="content"><slot></slot></div>
      `;
      CachedTemplateElement.#template = t;
    }
    return CachedTemplateElement.#template;
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    const clone = CachedTemplateElement.template.content.cloneNode(true);
    this.shadowRoot.appendChild(clone);
  }
}
```

### Global Template Registry

```javascript
const TemplateCache = (() => {
  const cache = new Map();
  
  function get(id) {
    if (!cache.has(id)) {
      const el = document.getElementById(id);
      if (!el) throw new Error(`Template #${id} not found`);
      cache.set(id, el.content.cloneNode(true));
    }
    return cache.get(id).cloneNode(true);
  }
  
  function preload(ids) {
    return Promise.all(ids.map(id => {
      const el = document.getElementById(id);
      if (el) cache.set(id, el.content.cloneNode(true));
    }));
  }
  
  function clear() {
    cache.clear();
  }
  
  return { get, preload, clear };
})();

// Usage
class RegistryElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    const clone = TemplateCache.get('my-template');
    this.shadowRoot.appendChild(clone);
  }
}
```

### LRU Cache for Multiple Templates

```javascript
class LRUTemplateCache {
  #maxSize = 10;
  #cache = new Map();
  
  constructor(maxSize = 10) {
    this.#maxSize = maxSize;
  }
  
  get(id) {
    if (this.#cache.has(id)) {
      // Move to end (most recently used)
      const value = this.#cache.get(id);
      this.#cache.delete(id);
      this.#cache.set(id, value);
      return value.cloneNode(true);
    }
    
    const el = document.getElementById(id);
    if (!el) return null;
    
    // Evict oldest if at capacity
    if (this.#cache.size >= this.#maxSize) {
      const oldestKey = this.#cache.keys().next().value;
      this.#cache.delete(oldestKey);
    }
    
    const clone = el.content.cloneNode(true);
    this.#cache.set(id, clone);
    return clone.cloneNode(true);
  }
}
```

## NEXT STEPS

Proceed to **04_Shadow-DOM/04_1-Shadow-DOM-CRA-Guide** for Shadow DOM integration.