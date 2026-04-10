/**
 * Inheritance and Composition Patterns - Component Hierarchy
 * @description Demonstrates inheritance and composition for component reusability
 * @module custom-elements/inheritance
 * @version 1.0.0
 */

// ============================================
// Base Component Class
// ============================================

/**
 * BaseComponent - Abstract base class for all components
 */
class BaseComponent extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._initialized = false;
    this._boundHandlers = new Map();
  }

  connectedCallback() {
    if (!this._initialized) {
      this.onInit();
      this._initialized = true;
    }
    this.onConnect();
  }

  disconnectedCallback() {
    this.onDisconnect();
    this._cleanup();
  }

  // Hooks to override
  onInit() {
    this.render();
  }

  onConnect() {}

  onDisconnect() {}

  // Cleanup
  _cleanup() {
    this._boundHandlers.forEach((handler, target) => {
      target.removeEventListener(...handler);
    });
    this._boundHandlers.clear();
  }

  // Utility methods
  $(selector) {
    return this.shadowRoot.querySelector(selector);
  }

  $$(selector) {
    return Array.from(this.shadowRoot.querySelectorAll(selector));
  }

  dispatch(name, detail = {}) {
    this.dispatchEvent(new CustomEvent(name, {
      bubbles: true,
      composed: true,
      detail
    }));
  }

  // Abstract methods to override
  render() {
    throw new Error('render() must be implemented');
  }
}

// ============================================
// Inheritance Pattern
// ============================================

/**
 * ButtonComponent - Extends BaseComponent
 */
class ButtonComponent extends BaseComponent {
  static get observedAttributes() {
    return ['variant', 'size', 'disabled'];
  }

  onInit() {
    this._variant = 'primary';
    this._size = 'medium';
    this.render();
  }

  attributeChangedCallback(name, oldVal, newVal) {
    if (name === 'variant') this._variant = newVal;
    if (name === 'size') this._size = newVal;
    this.render();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>
        button {
          padding: ${this._size === 'small' ? '6px 12px' : this._size === 'large' ? '14px 28px' : '10px 20px'};
          border: none;
          border-radius: 6px;
          background: ${this._variant === 'primary' ? '#667eea' : this._variant === 'success' ? '#28a745' : '#6c757d'};
          color: white;
          cursor: pointer;
        }
      </style>
      <button><slot></slot></button>
    `;
  }
}

// ============================================
// Mixin Pattern
// ============================================

/**
 * ReactiveMixin - Adds reactive state management
 */
function ReactiveMixin(Base) {
  return class extends Base {
    constructor() {
      super();
      this._state = {};
    }

    setState(updates) {
      const oldState = { ...this._state };
      Object.assign(this._state, updates);
      this.onStateChange(this._state, oldState);
      this.render();
    }

    getState() {
      return { ...this._state };
    }

    onStateChange(newState, oldState) {}
  };
}

/**
 * ValidationMixin - Adds validation capabilities
 */
function ValidationMixin(Base) {
  return class extends Base {
    constructor() {
      super();
      this._validators = [];
    }

    addValidator(validator) {
      this._validators.push(validator);
    }

    validate(value) {
      for (const validator of this._validators) {
        const result = validator(value);
        if (!result.valid) return result;
      }
      return { valid: true };
    }
  };
}

/**
 * FormMixin - Adds form integration
 */
function FormMixin(Base) {
  return class extends Base {
    static get formAssociated() { return true; }

    constructor() {
      super();
      this._internals = null;
    }

    connectedCallback() {
      super.connectedCallback?.();
      if (this.attachInternals) {
        this._internals = this.attachInternals();
      }
    }

    setFormValue(value) {
      this._internals?.setFormValue(value);
    }
  };
}

// ============================================
// Composition Pattern
// ============================================

/**
 * ComposedComponent - Uses composition over inheritance
 */
class ComposedComponent extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._children = new Map();
  }

  connectedCallback() {
    this.render();
    this._composeChildren();
  }

  _composeChildren() {
    // Find and initialize child elements
    const slots = this.shadowRoot.querySelectorAll('slot');
    slots.forEach(slot => {
      const name = slot.name || 'default';
      const nodes = slot.assignedNodes();
      this._children.set(name, nodes);
    });
  }

  addChild(name, element) {
    this._children.set(name, element);
    this.render();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <div class="header"><slot name="header"></slot></div>
      <div class="body"><slot></slot></div>
      <div class="footer"><slot name="footer"></slot></div>
    `;
  }
}

// ============================================
// Usage Examples
// ============================================

// Using inheritance
class MyButton extends ButtonComponent {}
customElements.define('my-button', MyButton);

// Using mixins (composition)
class FormInput extends FormMixin(ValidationMixin(ReactiveMixin(BaseComponent))) {
  render() {
    this.shadowRoot.innerHTML = '<input type="text" />';
  }
}
customElements.define('form-input', FormInput);

// Using composition
class Card extends ComposedComponent {}
customElements.define('my-card', Card);

export { BaseComponent, ButtonComponent, ReactiveMixin, ValidationMixin, FormMixin, ComposedComponent };