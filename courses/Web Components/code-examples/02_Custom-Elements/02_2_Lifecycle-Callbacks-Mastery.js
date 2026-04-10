/**
 * Lifecycle Callbacks Mastery - Complex State Management
 * @description Deep dive into lifecycle callbacks with state management
 * @module custom-elements/lifecycle
 * @version 1.0.0
 */

// ============================================
// Complete Lifecycle Implementation
// ============================================

/**
 * FullLifecycleComponent - Demonstrates all lifecycle callbacks
 */
class FullLifecycleComponent extends HTMLElement {
  // ============================================
  // Static Configuration
  // ============================================
  
  static get observedAttributes() {
    return ['data-value', 'disabled', 'mode'];
  }

  static get formAssociated() {
    return true;
  }

  // ============================================
  // Private State
  // ============================================
  
  #state = {
    value: null,
    disabled: false,
    mode: 'view'
  };
  
  #internals = null;
  #observer = null;
  #timers = [];
  #listeners = new Map();
  #initialized = false;

  // ============================================
  // CONSTRUCTOR
  // ============================================
  
  constructor() {
    super();
    
    this.attachShadow({ mode: 'open' });
    
    // Pre-bind event handlers for proper cleanup
    this._boundClick = this._handleClick.bind(this);
    this._boundKeydown = this._handleKeydown.bind(this);
    this._boundInput = this._handleInput.bind(this);
    
    console.log('[Lifecycle] Constructor');
  }

  // ============================================
  // CONNECTED CALLBACK
  // ============================================
  
  connectedCallback() {
    console.log('[Lifecycle] connectedCallback');
    
    // Check for re-connection (element moved in DOM)
    if (this.#initialized) {
      console.log('[Lifecycle] Re-connected (already initialized)');
      this.render(); // May need to re-render after move
      return;
    }
    
    try {
      // Initialize form internals
      this.#internals = this.attachInternals();
      
      // Parse initial attributes
      this._parseAttributes();
      
      // Setup observers
      this._setupMutationObserver();
      
      // Bind events
      this._bindEvents();
      
      // Initial render
      this.render();
      
      this.#initialized = true;
      
      // Dispatch ready event
      this.dispatchEvent(new CustomEvent('component-ready', {
        bubbles: true,
        composed: true
      }));
      
      console.log('[Lifecycle] Initialization complete');
    } catch (error) {
      console.error('[Lifecycle] Initialization failed:', error);
      this._renderError(error);
    }
  }

  // ============================================
  // DISCONNECTED CALLBACK
  // ============================================
  
  disconnectedCallback() {
    console.log('[Lifecycle] disconnectedCallback');
    
    // CRITICAL: Clean up ALL resources
    
    // 1. Disconnect observers
    if (this.#observer) {
      this.#observer.disconnect();
      this.#observer = null;
    }
    
    // 2. Clear timers
    this.#timers.forEach(id => clearTimeout(id));
    this.#timers = [];
    
    // 3. Remove event listeners
    this._unbindEvents();
    
    // 4. Reset initialization flag
    this.#initialized = false;
    
    // Dispatch disposed event
    this.dispatchEvent(new CustomEvent('component-disposed', {
      bubbles: true,
      composed: true
    }));
    
    console.log('[Lifecycle] Cleanup complete');
  }

  // ============================================
  // ATTRIBUTE CHANGED CALLBACK
  // ============================================
  
  attributeChangedCallback(name, oldValue, newValue) {
    console.log(`[Lifecycle] attributeChanged: ${name} = "${newValue}" (was: "${oldValue}")`);
    
    if (oldValue === newValue) return;
    
    // Update state
    if (name === 'data-value') {
      this.#state.value = newValue;
    } else if (name === 'disabled') {
      this.#state.disabled = newValue !== null;
    } else if (name === 'mode') {
      this.#state.mode = newValue || 'view';
    }
    
    // Re-render if connected
    if (this.isConnected) {
      this.render();
    }
  }

  // ============================================
  // ADOPTED CALLBACK
  // ============================================
  
  adoptedCallback() {
    console.log('[Lifecycle] adoptedCallback - Element moved to new document');
    // Re-render in new document context
    if (this.#initialized) {
      this.render();
    }
  }

  // ============================================
  // Private Methods
  // ============================================
  
  _parseAttributes() {
    this.#state = {
      value: this.getAttribute('data-value'),
      disabled: this.hasAttribute('disabled'),
      mode: this.getAttribute('mode') || 'view'
    };
  }

  _setupMutationObserver() {
    this.#observer = new MutationObserver((mutations) => {
      mutations.forEach(mutation => {
        if (mutation.type === 'childList') {
          console.log('[Lifecycle] Children changed:', mutation.addedNodes.length, 'added');
        }
      });
    });
    
    this.#observer.observe(this, {
      childList: true,
      subtree: true
    });
  }

  _bindEvents() {
    const container = this.shadowRoot.querySelector('.container');
    if (container) {
      container.addEventListener('click', this._boundClick);
      container.addEventListener('keydown', this._boundKeydown);
      
      this.#listeners.set('click', this._boundClick);
      this.#listeners.set('keydown', this._boundKeydown);
    }
    
    const input = this.shadowRoot.querySelector('input');
    if (input) {
      input.addEventListener('input', this._boundInput);
      this.#listeners.set('input', this._boundInput);
    }
  }

  _unbindEvents() {
    this.#listeners.forEach((handler, event) => {
      const element = this.shadowRoot.querySelector('.container') || this.shadowRoot.querySelector('input');
      element?.removeEventListener(event, handler);
    });
    this.#listeners.clear();
  }

  _handleClick(event) {
    if (this.#state.disabled) return;
    
    this.dispatchEvent(new CustomEvent('element-click', {
      bubbles: true,
      composed: true,
      detail: { timestamp: Date.now() }
    }));
  }

  _handleKeydown(event) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this._handleClick(event);
    }
  }

  _handleInput(event) {
    const value = event.target.value;
    this.#internals?.setFormValue(value);
    
    this.dispatchEvent(new CustomEvent('value-change', {
      bubbles: true,
      composed: true,
      detail: { value }
    }));
  }

  // ============================================
  // Render
  // ============================================
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>${this.styles}</style>
      <div class="container ${this.#state.disabled ? 'disabled' : ''}" role="button" tabindex="0">
        <div class="mode">Mode: ${this.#state.mode}</div>
        <div class="value">Value: ${this.#state.value || 'None'}</div>
        <input type="text" placeholder="Enter value" ?disabled="${this.#state.disabled}" />
      </div>
    `;
  }

  _renderError(error) {
    this.shadowRoot.innerHTML = `
      <div class="error">
        <h3>Component Error</h3>
        <p>${error.message}</p>
      </div>
    `;
  }

  get styles() {
    return `
      <style>
        :host { display: block; }
        .container {
          padding: 20px;
          background: white;
          border-radius: 8px;
          box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        .container.disabled { opacity: 0.5; }
        .mode, .value { margin: 8px 0; }
        input { width: 100%; padding: 8px; margin-top: 10px; }
        .error { color: red; padding: 20px; }
      </style>
    `;
  }

  // ============================================
  // Public API
  // ============================================
  
  get value() { return this.#state.value; }
  set value(val) { this.setAttribute('data-value', val); }
  
  get mode() { return this.#state.mode; }
  set mode(val) { this.setAttribute('mode', val); }

  reset() {
    this.#state.value = null;
    this.removeAttribute('data-value');
    this.render();
  }
}

customElements.define('full-lifecycle', FullLifecycleComponent);

export { FullLifecycleComponent };