# Performance Optimization Techniques

## OVERVIEW

Optimizing Shadow DOM performance involves efficient template caching, minimal DOM operations, and proper cleanup. This guide covers techniques for building high-performance components.

## IMPLEMENTATION DETAILS

### Template Caching

```javascript
class PerfComponent {
  // Static template - parsed once
  static #template = null;
  
  static get template() {
    if (!PerfComponent.#template) {
      const t = document.createElement('template');
      t.innerHTML = '<div>Content</div>';
      PerfComponent.#template = t;
    }
    return PerfComponent.#template;
  }
  
  connectedCallback() {
    // Clone is fast - no re-parsing
    const clone = PerfComponent.template.content.cloneNode(true);
    this.shadowRoot.appendChild(clone);
  }
}
```

### DocumentFragment Usage

```javascript
class FragmentComponent extends HTMLElement {
  connectedCallback() {
    // Use DocumentFragment to minimize reflows
    const fragment = document.createDocumentFragment();
    
    for (let i = 0; i < 100; i++) {
      const div = document.createElement('div');
      div.textContent = `Item ${i}`;
      fragment.appendChild(div);
    }
    
    this.shadowRoot.appendChild(fragment);
  }
}
```

### Lazy Rendering

```javascript
class LazyComponent extends HTMLElement {
  #ready = false;
  
  connectedCallback() {
    if (!this.#ready) {
      requestAnimationFrame(() => {
        this.render();
        this.#ready = true;
      });
    }
  }
  
  render() {
    this.attachShadow({ mode: 'open' });
    this.shadowRoot.innerHTML = '<div>Content</div>';
  }
}
```

### CSS Containment

```javascript
class ContainedComponent extends HTMLElement {
  get styles() {
    return `
      <style>
        :host {
          /* Containment improves rendering performance */
          contain: content;
        }
      </style>
      <slot></slot>
    `;
  }
}
```

## NEXT STEPS

Proceed to **05_Data-Binding/05_1_Property-Reflection-Strategies** for data binding.