/**
 * Web Component Best Practices - Production-Ready Patterns
 * @description Comprehensive best practices for building production Web Components
 * @module basics/best-practices
 * @version 1.0.0
 */

// ============================================
// Production-Ready Component Template
// ============================================

/**
 * BestPracticeComponent - Production-ready component template
 * @description Demonstrates all best practices in one component
 */
class BestPracticeComponent extends HTMLElement {
  // ============================================
  // Static Configuration
  // ============================================
  
  static get is() { return 'best-practice-component'; }
  
  static get observedAttributes() {
    return ['title', 'variant', 'disabled', 'loading'];
  }
  
  // Enable form integration
  static get formAssociated() {
    return true;
  }
  
  // ============================================
  // Private Fields (True Encapsulation)
  // ============================================
  
  #internals = null;
  #rendered = false;
  #cleanupFns = [];
  #state = { title: '', variant: 'default', disabled: false, loading: false };

  // ============================================
  // Constructor
  // ============================================
  
  constructor() {
    super();
    
    // Always call super() first
    this.attachShadow({ mode: 'open' });
    
    // Bind methods to preserve 'this' context
    this._handleClick = this._handleClick.bind(this);
    this._handleKeydown = this._handleKeydown.bind(this);
  }

  // ============================================
  // Lifecycle Callbacks
  // ============================================
  
  connectedCallback() {
    // Avoid double initialization
    if (this.#rendered) return;
    
    this._parseAttributes();
    this._initFormInternals();
    this._bindEvents();
    this._setupObservers();
    
    this.render();
    this.#rendered = true;
    
    this.dispatchEvent(new CustomEvent('component-ready', {
      bubbles: true,
      composed: true
    }));
    
    console.log(`[${BestPracticeComponent.is}] Component mounted`);
  }

  disconnectedCallback() {
    // CRITICAL: Clean up all resources
    this._unbindEvents();
    this._cleanupObservers();
    this._clearTimers();
    
    this.dispatchEvent(new CustomEvent('component-disposed', {
      bubbles: true,
      composed: true
    }));
    
    console.log(`[${BestPracticeComponent.is}] Component unmounted`);
  }

  // ============================================
  // Attribute Observation
  // ============================================
  
  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    
    console.log(`[${BestPracticeComponent.is}] ${name}: ${oldValue} → ${newValue}`);
    
    this.#state[name] = newValue;
    this._onAttributeChange(name, oldValue, newValue);
  }

  // ============================================
  // Public Properties (with reflection)
  // ============================================
  
  get title() { return this.#state.title; }
  set title(value) {
    this.setAttribute('title', value);
    this.#state.title = value;
  }

  get variant() { return this.#state.variant; }
  set variant(value) {
    this.setAttribute('variant', value);
    this.#state.variant = value;
  }

  get disabled() { return this.#state.disabled; }
  set disabled(value) {
    value ? this.setAttribute('disabled', '') : this.removeAttribute('disabled');
    this.#state.disabled = value;
  }

  get loading() { return this.#state.loading; }
  set loading(value) {
    value ? this.setAttribute('loading', '') : this.removeAttribute('loading');
    this.#state.loading = value;
  }

  // Form Integration API
  get form() { return this.#internals?.form; }
  get validity() { return this.#internals?.validity; }
  checkValidity() { return this.#internals?.checkValidity() ?? true; }

  // ============================================
  // Private Methods
  // ============================================
  
  _parseAttributes() {
    this.#state = {
      title: this.getAttribute('title') || '',
      variant: this.getAttribute('variant') || 'default',
      disabled: this.hasAttribute('disabled'),
      loading: this.hasAttribute('loading')
    };
  }

  _initFormInternals() {
    if (this.attachInternals) {
      this.#internals = this.attachInternals();
    }
  }

  _bindEvents() {
    const button = this.shadowRoot.querySelector('.button');
    if (button) {
      button.addEventListener('click', this._handleClick);
      button.addEventListener('keydown', this._handleKeydown);
      
      // Store for cleanup
      this.#cleanupFns.push(() => {
        button.removeEventListener('click', this._handleClick);
        button.removeEventListener('keydown', this._handleKeydown);
      });
    }
  }

  _unbindEvents() {
    this.#cleanupFns.forEach(fn => fn());
    this.#cleanupFns = [];
  }

  _setupObservers() {
    // Setup MutationObserver if needed
  }

  _cleanupObservers() {
    // Cleanup observers
  }

  _clearTimers() {
    // Clear any timers/intervals
  }

  _onAttributeChange(name, oldValue, newValue) {
    // Handle specific attribute changes
  }

  _handleClick(event) {
    if (this.#state.disabled || this.#state.loading) return;
    
    this.dispatchEvent(new CustomEvent('button-click', {
      bubbles: true,
      composed: true,
      detail: { originalEvent: event }
    }));
  }

  _handleKeydown(event) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this._handleClick(event);
    }
  }

  // ============================================
  // Render Method
  // ============================================
  
  render() {
    const { title, variant, disabled, loading } = this.#state;
    
    this.shadowRoot.innerHTML = `
      <style>${this.styles}</style>
      <button 
        class="button ${variant}"
        ?disabled="${disabled || loading}"
        aria-label="${title}"
        aria-busy="${loading}"
        role="button"
        tabindex="0"
      >
        ${loading ? '<span class="spinner"></span>' : ''}
        <slot>${title}</slot>
      </button>
    `;
  }

  get styles() {
    return `
      <style>
        :host {
          display: inline-block;
          --primary: #667eea;
          --success: #28a745;
          --danger: #dc3545;
          --disabled: #6c757d;
        }
        
        .button {
          display: inline-flex;
          align-items: center;
          justify-content: center;
          gap: 8px;
          padding: 12px 24px;
          border: none;
          border-radius: 6px;
          font-family: inherit;
          font-size: 14px;
          font-weight: 600;
          cursor: pointer;
          transition: all 0.2s ease;
        }
        
        .button:hover:not(:disabled) {
          transform: translateY(-2px);
          box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        }
        
        .button:focus-visible {
          outline: 2px solid var(--primary);
          outline-offset: 2px;
        }
        
        .default { background: var(--primary); color: white; }
        .success { background: var(--success); color: white; }
        .danger { background: var(--danger); color: white; }
        
        .button:disabled {
          background: var(--disabled);
          opacity: 0.6;
          cursor: not-allowed;
        }
        
        .spinner {
          width: 16px;
          height: 16px;
          border: 2px solid transparent;
          border-top-color: white;
          border-radius: 50%;
          animation: spin 0.8s linear infinite;
        }
        
        @keyframes spin {
          to { transform: rotate(360deg); }
        }
      </style>
    `;
  }
}

// Register component
customElements.define(BestPracticeComponent.is, BestPracticeComponent);

export { BestPracticeComponent };