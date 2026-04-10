/**
 * ES Modules Deep Dive - Module Federation Examples
 * @description Advanced ES Module patterns for component architecture
 * @module basics/es-modules
 * @version 1.0.0
 */

// ============================================
// Module Federation Pattern
// ============================================

/**
 * ComponentLoader - Lazy loading of remote components
 */
export class ComponentLoader {
  constructor() {
    this._cache = new Map();
    this._loading = new Map();
  }

  /**
   * Load component from remote URL
   * @param {string} name - Component name
   * @param {string} url - Remote URL
   * @returns {Promise<Object>}
   */
  async load(name, url) {
    // Return cached if available
    if (this._cache.has(name)) {
      return this._cache.get(name);
    }

    // Return existing promise if already loading
    if (this._loading.has(name)) {
      return this._loading.get(name);
    }

    // Create loading promise
    const loadPromise = import(/* @vite-ignore */ url)
      .then(module => {
        const component = module.default || module;
        this._cache.set(name, component);
        this._loading.delete(name);
        return component;
      })
      .catch(error => {
        this._loading.delete(name);
        throw error;
      });

    this._loading.set(name, loadPromise);
    return loadPromise;
  }

  /**
   * Load multiple components
   * @param {Object} components - Key-value pairs of name and URL
   * @returns {Promise<Object[]>}
   */
  async loadMany(components) {
    const promises = Object.entries(components).map(
      ([name, url]) => this.load(name, url)
    );
    return Promise.all(promises);
  }

  /**
   * Check if component is cached
   * @param {string} name
   * @returns {boolean}
   */
  has(name) {
    return this._cache.has(name);
  }

  /**
   * Clear cache
   */
  clear() {
    this._cache.clear();
  }
}

export const componentLoader = new ComponentLoader();

// ============================================
// Dynamic Import Patterns
// ============================================

/**
 * LazyComponent - Demonstrates lazy loading
 */
export class LazyComponent extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._loaded = false;
  }

  connectedCallback() {
    if (!this._loaded) {
      this.renderLoading();
      this._loadContent();
    }
  }

  async _loadContent() {
    try {
      // Dynamic import for lazy loading
      const { HeavyComponent } = await import('./heavy-component.js');
      
      this._componentClass = HeavyComponent;
      this._loaded = true;
      this.render();
    } catch (error) {
      this.renderError(error);
    }
  }

  renderLoading() {
    this.shadowRoot.innerHTML = `
      <style>
        .loading {
          display: flex;
          align-items: center;
          justify-content: center;
          padding: 40px;
          color: #666;
        }
        .spinner {
          width: 24px;
          height: 24px;
          border: 3px solid #eee;
          border-top-color: #667eea;
          border-radius: 50%;
          animation: spin 1s linear infinite;
        }
        @keyframes spin {
          to { transform: rotate(360deg); }
        }
      </style>
      <div class="loading">
        <div class="spinner"></div>
        <span>Loading...</span>
      </div>
    `;
  }

  render() {
    if (!this._componentClass) return;
    
    this.shadowRoot.innerHTML = `
      <div class="content">
        <${this._componentClass.is}></${this._componentClass.is}>
      </div>
    `;
  }

  renderError(error) {
    this.shadowRoot.innerHTML = `
      <div class="error">
        <p>Failed to load component</p>
        <small>${error.message}</small>
      </div>
    `;
  }
}

customElements.define('lazy-component', LazyComponent);

// ============================================
// Re-export Pattern (Barrel Pattern)
// ============================================

// index.js - Re-export all components
export { ButtonComponent } from './button.js';
export { InputComponent } from './input.js';
export { CardComponent } from './card.js';
export { ModalComponent } from './modal.js';

export { componentLoader, ComponentLoader } from './loader.js';
export { LazyComponent } from './lazy.js';

// ============================================
// Import Maps for Clean Dependencies
// ============================================

/*
  Use import maps in HTML:
  
  <script type="importmap">
  {
    "imports": {
      "lit": "https://cdn.jsdelivr.net/npm/lit@3/+esm",
      "lit/": "https://cdn.jsdelivr.net/npm/lit@3/",
      "@components/": "./components/"
    }
  }
  </script>
  
  Then use clean imports:
  import { Button } from '@components/button.js';
*/