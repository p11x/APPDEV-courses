/**
 * Defining Custom Elements - Registry Patterns
 * @description Advanced patterns for custom element registration
 * @module custom-elements/defining
 * @version 1.0.0
 */

// ============================================
// Basic Registration
// ============================================

class BasicElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  connectedCallback() {
    this.shadowRoot.innerHTML = '<div>Basic Element</div>';
  }
}

// Simple registration
customElements.define('basic-element', BasicElement);

// ============================================
// Advanced Registration with Options
// ============================================

/**
 * ExtendedButton - Customized built-in element
 */
class ExtendedButton extends HTMLButtonElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
  }

  render() {
    const variant = this.getAttribute('variant') || 'primary';
    this.shadowRoot.innerHTML = `
      <style>
        button {
          padding: 10px 20px;
          border: none;
          border-radius: 6px;
          background: ${variant === 'primary' ? '#667eea' : '#28a745'};
          color: white;
          cursor: pointer;
        }
      </style>
      <button><slot></slot></button>
    `;
  }
}

// Register with extends option for customized built-in elements
customElements.define('extended-button', ExtendedButton, { extends: 'button' });

// ============================================
// Custom Element Registry Patterns
// ============================================

/**
 * ComponentRegistry - Manages multiple component registrations
 */
class ComponentRegistry {
  constructor() {
    this._components = new Map();
    this._loading = new Map();
  }

  /**
   * Register a component with validation
   * @param {string} name - Element name (must contain hyphen)
   * @param {Function} classDef - Component class
   * @param {Object} options - Registration options
   */
  register(name, classDef, options = {}) {
    // Validate name
    if (!name.includes('-')) {
      throw new Error(`Invalid element name: "${name}". Must contain a hyphen.`);
    }

    if (customElements.get(name)) {
      console.warn(`Element "${name}" already defined. Skipping.`);
      return;
    }

    try {
      customElements.define(name, classDef, options);
      this._components.set(name, { classDef, options });
      console.log(`[Registry] Registered: ${name}`);
    } catch (error) {
      console.error(`[Registry] Failed to register ${name}:`, error);
      throw error;
    }
  }

  /**
   * Async registration with loading
   */
  async registerAsync(name, loader) {
    if (this._loading.has(name)) {
      return this._loading.get(name);
    }

    const loadPromise = (async () => {
      const module = await loader();
      const ComponentClass = module.default || module;
      this.register(name, ComponentClass);
      return ComponentClass;
    })();

    this._loading.set(name, loadPromise);
    return loadPromise;
  }

  /**
   * Get registered component
   */
  get(name) {
    return this._components.get(name);
  }

  /**
   * Check if element is defined
   */
  isDefined(name) {
    return customElements.get(name) !== undefined;
  }

  /**
   * Wait for element to be defined
   */
  whenDefined(name) {
    return customElements.whenDefined(name);
  }
}

// Create global registry
export const registry = new ComponentRegistry();

// ============================================
// Factory Pattern
// ============================================

/**
 * Create component with default setup
 */
function createComponent(name, renderFn, options = {}) {
  class FactoryComponent extends HTMLElement {
    constructor() {
      super();
      this.attachShadow({ mode: options.shadowMode || 'open' });
    }

    connectedCallback() {
      renderFn.call(this, this.shadowRoot);
    }
  }

  // Add observed attributes if provided
  if (options.attributes) {
    FactoryComponent.observedAttributes = options.attributes;
  }

  // Add form support if needed
  if (options.formAssociated) {
    FactoryComponent.formAssociated = true;
  }

  // Register
  customElements.define(name, FactoryComponent);
  
  return FactoryComponent;
}

// Usage
createComponent(
  'factory-button',
  (shadow) => {
    shadow.innerHTML = '<button><slot></slot></button>';
  },
  { attributes: ['variant', 'disabled'], shadowMode: 'open' }
);

// ============================================
// Lazy Registration
// ============================================

/**
 * LazyDefine - Register only when needed
 */
class LazyDefine {
  static observe(element, componentMap) {
    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          const tagName = entry.target.tagName.toLowerCase();
          const ComponentClass = componentMap[tagName];
          
          if (ComponentClass && !customElements.get(tagName)) {
            customElements.define(tagName, ComponentClass);
            console.log(`[LazyDefine] Registered: ${tagName}`);
          }
          
          observer.unobserve(entry.target);
        }
      });
    });

    observer.observe(element);
    return observer;
  }
}

export { ComponentRegistry, createComponent, LazyDefine };