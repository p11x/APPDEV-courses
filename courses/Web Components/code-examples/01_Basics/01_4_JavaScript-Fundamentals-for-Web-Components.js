/**
 * JavaScript Fundamentals for Web Components - ES Module Patterns
 * @description Modern ES Module patterns for component architecture
 * @module basics/js-fundamentals
 * @version 1.0.0
 */

// ============================================
// ES Module Export Patterns
// ============================================

/**
 * ButtonComponent - Demonstrates ES module patterns
 * @export
 */
export class ButtonComponent extends HTMLElement {
  static get is() { return 'wc-button'; }
  
  static get observedAttributes() {
    return ['variant', 'size', 'disabled'];
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._variant = 'primary';
    this._size = 'medium';
  }

  connectedCallback() {
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[`_${name}`] = newValue;
      this.render();
    }
  }

  get variant() { return this._variant; }
  set variant(value) { this.setAttribute('variant', value); }

  get size() { return this._size; }
  set size(value) { this.setAttribute('size', value); }

  get disabled() { return this.hasAttribute('disabled'); }
  set disabled(value) { value ? this.setAttribute('disabled', '') : this.removeAttribute('disabled'); }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${this.styles}</style>
      <button class="btn ${this._variant} ${this._size}" ?disabled="${this.disabled}">
        <slot></slot>
      </button>
    `;
  }

  get styles() {
    return `
      <style>
        .btn {
          font-family: inherit;
          border: none;
          border-radius: 6px;
          cursor: pointer;
          transition: all 0.2s ease;
        }
        .btn:hover { transform: translateY(-1px); }
        .btn:active { transform: translateY(0); }
        
        .primary { background: #667eea; color: white; }
        .secondary { background: #6c757d; color: white; }
        .success { background: #28a745; color: white; }
        .danger { background: #dc3545; color: white; }
        
        .small { padding: 6px 12px; font-size: 12px; }
        .medium { padding: 10px 20px; font-size: 14px; }
        .large { padding: 14px 28px; font-size: 16px; }
        
        .btn:disabled {
          opacity: 0.5;
          cursor: not-allowed;
        }
      </style>
    `;
  }
}

// Named export
export { ButtonComponent as Button };

// Default export
export default class DefaultButton extends ButtonComponent {
  connectedCallback() {
    super.connectedCallback();
    console.log('[DefaultButton] Initialized');
  }
}

// Register element
customElements.define(ButtonComponent.is, ButtonComponent);

// ============================================
// Component Utilities
// ============================================

/**
 * Create component factory
 * @param {string} tagName - Custom element tag name
 * @param {Function} ComponentClass - Component class
 * @returns {Promise<Function>}
 */
export function createComponent(tagName, ComponentClass) {
  return new Promise((resolve, reject) => {
    if (customElements.get(tagName)) {
      resolve(ComponentClass);
      return;
    }
    
    customElements.define(tagName, ComponentClass);
    
    customElements.whenDefined(tagName)
      .then(() => resolve(ComponentClass))
      .catch(reject);
  });
}

/**
 * Component registry
 */
export class ComponentRegistry {
  constructor() {
    this._components = new Map();
  }

  register(name, module) {
    this._components.set(name, module);
  }

  get(name) {
    return this._components.get(name);
  }

  async load(name, url) {
    const module = await import(url);
    this.register(name, module);
    return module;
  }
}

export const registry = new ComponentRegistry();