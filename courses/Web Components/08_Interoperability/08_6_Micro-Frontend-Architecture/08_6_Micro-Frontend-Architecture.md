# Micro-Frontend Architecture

## OVERVIEW

Micro-frontend architecture uses Web Components as integration boundaries between independent deployments. This guide covers component distribution, lazy loading, and cross-app communication.

## IMPLEMENTATION DETAILS

### Component Distribution

```javascript
// Component registry for micro-frontends
class ComponentRegistry {
  #components = new Map();
  
  register(name, loader) {
    this.#components.set(name, loader);
  }
  
  async get(name) {
    if (!this.#components.has(name)) {
      throw new Error(`Component ${name} not registered`);
    }
    
    const loader = this.#components.get(name);
    const module = await loader();
    return module.default || module;
  }
  
  define(name) {
    return this.get(name).then(ComponentClass => {
      customElements.define(name, ComponentClass);
    });
  }
}

export const registry = new ComponentRegistry();
```

### Lazy Loading

```javascript
class LazyLoader extends HTMLElement {
  static get observedAttributes() { return ['src', 'component']; }
  
  #ComponentClass = null;
  
  async attributeChangedCallback() {
    await this.#loadComponent();
  }
  
  async #loadComponent() {
    const src = this.getAttribute('src');
    if (!src) return;
    
    const module = await import(src);
    this.#ComponentClass = module.default || module;
    
    if (this.isConnected) {
      this.render();
    }
  }
  
  render() {
    if (!this.#ComponentClass) return;
    
    this.innerHTML = '';
    const instance = new this.#ComponentClass();
    this.appendChild(instance);
  }
}
```

## NEXT STEPS

Proceed to **09_Performance/09_1_Bundle-Size-Optimization**.