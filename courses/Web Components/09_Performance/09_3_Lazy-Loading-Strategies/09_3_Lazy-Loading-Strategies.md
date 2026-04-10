# Lazy Loading Strategies

## OVERVIEW

Lazy loading defers component loading until needed. This guide covers intersection observers, dynamic imports, and progressive loading.

## IMPLEMENTATION DETAILS

### Intersection Observer Loading

```javascript
class LazyLoadElement extends HTMLElement {
  #loaded = false;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    if (this.#loaded) return;
    
    const observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting) {
        this.#load();
        observer.disconnect();
      }
    });
    
    observer.observe(this);
  }
  
  async #load() {
    const module = await import('./HeavyComponent.js');
    const ComponentClass = module.default;
    this.appendChild(new ComponentClass());
    this.#loaded = true;
  }
}
```

### Module Preloading

```html
<!-- Preload components -->
<link rel="modulepreload" href="./components/button.js">
<link rel="modulepreload" href="./components/card.js">
```

## NEXT STEPS

Proceed to **09_Performance/09_4_Memory-Management-Methods**.