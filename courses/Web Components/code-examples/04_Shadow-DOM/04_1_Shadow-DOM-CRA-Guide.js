/**
 * Shadow-DOM-CRA-Guide - Component development workflow with Shadow DOM setup
 * @module 04_Shadow-DOM/Shadow-DOM-CRA-Guide
 * @version 1.0.0
 * @example <shadow CRA-Guide></shadow CRA-Guide>
 */

class ShadowDOMCRAGuide extends HTMLElement {
  /**
   * Creates an instance of ShadowDOMCRAGuide.
   * @constructor
   */
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._initialState = {
      isReady: false,
      isMounted: false,
      errorCount: 0,
      renderCount: 0,
      lastRenderTime: null,
      stylesLoaded: false,
      dependenciesResolved: false,
    };
    this._state = { ...this._initialState };
    this._styles = null;
    this._slot = null;
    this._observers = [];
    this._eventListeners = new Map();
    this._timeoutIds = [];
    this._animationFrameIds = [];
  }

  /**
   * Lifecycle callback when the element is added to the DOM.
   * @method connectedCallback
   * @returns {void}
   */
  connectedCallback() {
    if (this._state.isMounted) return;
    
    try {
      this._initializeComponent();
      this._setupEventListeners();
      this._setupMutationObserver();
      this._setupIntersectionObserver();
      this._render();
      this._state.isMounted = true;
      this._dispatchLifecycleEvent('connected');
    } catch (error) {
      this._handleError('connectedCallback', error);
    }
  }

  /**
   * Lifecycle callback when the element is removed from the DOM.
   * @method disconnectedCallback
   * @returns {void}
   */
  disconnectedCallback() {
    try {
      this._cleanupEventListeners();
      this._cleanupObservers();
      this._cleanupTimers();
      this._state.isMounted = false;
      this._dispatchLifecycleEvent('disconnected');
    } catch (error) {
      this._handleError('disconnectedCallback', error);
    }
  }

  /**
   * Lifecycle callback when an attribute changes.
   * @method observedAttributes
   * @returns {string[]} Array of observed attribute names.
   */
  static get observedAttributes() {
    return ['theme', 'disabled', 'size', 'variant', 'label', 'placeholder'];
  }

  /**
   * Lifecycle callback when an attribute changes.
   * @method attributeChangedCallback
   * @param {string} name - The attribute name that changed.
   * @param {string} oldValue - The old value of the attribute.
   * @param {string} newValue - The new value of the attribute.
   * @returns {void}
   */
  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    
    try {
      this._handleAttributeChange(name, oldValue, newValue);
      this._render();
    } catch (error) {
      this._handleError('attributeChangedCallback', error);
    }
  }

  /**
   * Lifecycle callback when the element moves to a new document.
   * @method adoptedCallback
   * @returns {void}
   */
  adoptedCallback() {
    this._dispatchLifecycleEvent('adopted');
  }

  /**
   * Initializes the component with default styles and structure.
   * @method _initializeComponent
   * @private
   * @returns {void}
   */
  _initializeComponent() {
    this._styles = this._createStyles();
    this._slot = this._createSlot();
    this._applyStyles();
    this._state.isReady = true;
  }

  /**
   * Creates the CSS styles for the component.
   * @method _createStyles
   * @private
   * @returns {string} The CSS styles.
   */
  _createStyles() {
    return `
      :host {
        display: block;
        --component-bg: #ffffff;
        --component-border: #e0e0e0;
        --component-text: #333333;
        --component-accent: #2196f3;
        --component-error: #f44336;
        --component-success: #4caf50;
        --component-shadow: rgba(0, 0, 0, 0.1);
        --component-radius: 8px;
        --component-padding: 16px;
        --component-font-size: 14px;
        --component-transition: all 0.3s ease;
      }

      :host([theme="dark"]) {
        --component-bg: #1a1a1a;
        --component-border: #333333;
        --component-text: #ffffff;
        --component-accent: #64b5f6;
      }

      :host([disabled]) {
        pointer-events: none;
        opacity: 0.5;
      }

      .component-container {
        background: var(--component-bg);
        border: 1px solid var(--component-border);
        border-radius: var(--component-radius);
        padding: var(--component-padding);
        transition: var(--component-transition);
        position: relative;
      }

      .component-container:hover {
        box-shadow: 0 4px 12px var(--component-shadow);
      }

      .component-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: 12px;
        padding-bottom: 12px;
        border-bottom: 1px solid var(--component-border);
      }

      .component-title {
        font-size: calc(var(--component-font-size) + 4px);
        font-weight: 600;
        color: var(--component-text);
        margin: 0;
      }

      .component-badge {
        display: inline-flex;
        align-items: center;
        padding: 4px 8px;
        border-radius: 4px;
        background: var(--component-accent);
        color: white;
        font-size: 12px;
        font-weight: 500;
      }

      .component-content {
        color: var(--component-text);
        font-size: var(--component-font-size);
        line-height: 1.6;
      }

      .component-footer {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-top: 12px;
        padding-top: 12px;
        border-top: 1px solid var(--component-border);
      }

      .component-button {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        padding: 8px 16px;
        border: none;
        border-radius: 4px;
        background: var(--component-accent);
        color: white;
        font-size: var(--component-font-size);
        font-weight: 500;
        cursor: pointer;
        transition: var(--component-transition);
      }

      .component-button:hover {
        filter: brightness(1.1);
        transform: translateY(-1px);
      }

      .component-button:active {
        transform: translateY(0);
      }

      .component-input {
        width: 100%;
        padding: 10px 12px;
        border: 1px solid var(--component-border);
        border-radius: 4px;
        font-size: var(--component-font-size);
        background: var(--component-bg);
        color: var(--component-text);
        transition: var(--component-transition);
        box-sizing: border-box;
      }

      .component-input:focus {
        outline: none;
        border-color: var(--component-accent);
        box-shadow: 0 0 0 2px rgba(33, 150, 243, 0.2);
      }

      .component-input::placeholder {
        color: #999;
      }

      .component-error {
        color: var(--component-error);
        font-size: 12px;
        margin-top: 4px;
      }

      .status-indicator {
        display: inline-flex;
        align-items: center;
        gap: 6px;
        font-size: 12px;
        color: #666;
      }

      .status-dot {
        width: 8px;
        height: 8px;
        border-radius: 50%;
        background: var(--component-success);
        animation: pulse 2s ease-in-out infinite;
      }

      @keyframes pulse {
        0%, 100% { opacity: 1; }
        50% { opacity: 0.5; }
      }

      .state-grid {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 8px;
        margin-top: 12px;
      }

      .state-item {
        display: flex;
        justify-content: space-between;
        padding: 8px;
        background: rgba(0, 0, 0, 0.05);
        border-radius: 4px;
        font-size: 12px;
      }

      .state-label {
        font-weight: 500;
        color: #666;
      }

      .state-value {
        font-family: monospace;
        color: var(--component-text);
      }
    `;
  }

  /**
   * Creates the slot element for content distribution.
   * @method _createSlot
   * @private
   * @returns {HTMLSlotElement} The slot element.
   */
  _createSlot() {
    const slot = document.createElement('slot');
    slot.setAttribute('name', 'content');
    return slot;
  }

  /**
   * Applies styles to the shadow DOM.
   * @method _applyStyles
   * @private
   * @returns {void}
   */
  _applyStyles() {
    const styleSheet = document.createElement('style');
    styleSheet.textContent = this._styles;
    this.shadowRoot.appendChild(styleSheet);
    this._state.stylesLoaded = true;
  }

  /**
   * Sets up event listeners for the component.
   * @method _setupEventListeners
   * @private
   * @returns {void}
   */
  _setupEventListeners() {
    this._handleClick = this._handleClick.bind(this);
    this._handleInput = this._handleInput.bind(this);
    this._handleSubmit = this._handleSubmit.bind(this);
    this._handleKeyDown = this._handleKeyDown.bind(this);
    this._handleFocus = this._handleFocus.bind(this);
    this._handleBlur = this._handleBlur.bind(this);

    this.addEventListener('click', this._handleClick);
    this.addEventListener('input', this._handleInput);
    this.addEventListener('submit', this._handleSubmit);
    this.addEventListener('keydown', this._handleKeyDown);
    this.addEventListener('focus', this._handleFocus);
    this.addEventListener('blur', this._handleBlur);

    this._eventListeners.set('click', this._handleClick);
    this._eventListeners.set('input', this._handleInput);
    this._eventListeners.set('submit', this._handleSubmit);
    this._eventListeners.set('keydown', this._handleKeyDown);
    this._eventListeners.set('focus', this._handleFocus);
    this._eventListeners.set('blur', this._handleBlur);
  }

  /**
   * Cleans up event listeners.
   * @method _cleanupEventListeners
   * @private
   * @returns {void}
   */
  _cleanupEventListeners() {
    this.removeEventListener('click', this._handleClick);
    this.removeEventListener('input', this._handleInput);
    this.removeEventListener('submit', this._handleSubmit);
    this.removeEventListener('keydown', this._handleKeyDown);
    this.removeEventListener('focus', this._handleFocus);
    this.removeEventListener('blur', this._handleBlur);
    this._eventListeners.clear();
  }

  /**
   * Sets up mutation observer for attribute and child changes.
   * @method _setupMutationObserver
   * @private
   * @returns {void}
   */
  _setupMutationObserver() {
    const observer = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.type === 'attributes') {
          this._handleAttributeChange(
            mutation.attributeName,
            mutation.oldValue,
            this.getAttribute(mutation.attributeName)
          );
        }
      });
    });

    observer.observe(this, {
      attributes: true,
      attributeOldValue: true,
      subtree: false,
    });

    this._observers.push(observer);
  }

  /**
   * Sets up intersection observer for visibility detection.
   * @method _setupIntersectionObserver
   * @private
   * @returns {void}
   */
  _setupIntersectionObserver() {
    if (!('IntersectionObserver' in window)) return;

    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          this._dispatchCustomEvent('visibility-change', {
            isVisible: entry.isIntersecting,
            intersectionRatio: entry.intersectionRatio,
          });
        });
      },
      { threshold: [0, 0.25, 0.5, 0.75, 1] }
    );

    observer.observe(this);
    this._observers.push(observer);
  }

  /**
   * Cleans up all observers.
   * @method _cleanupObservers
   * @private
   * @returns {void}
   */
  _cleanupObservers() {
    this._observers.forEach((observer) => {
      observer.disconnect();
    });
    this._observers = [];
  }

  /**
   * Cleans up all timers and animation frames.
   * @method _cleanupTimers
   * @private
   * @returns {void}
   */
  _cleanupTimers() {
    this._timeoutIds.forEach((id) => clearTimeout(id));
    this._animationFrameIds.forEach((id) => cancelAnimationFrame(id));
    this._timeoutIds = [];
    this._animationFrameIds = [];
  }

  /**
   * Handles click events.
   * @method _handleClick
   * @private
   * @param {MouseEvent} event - The click event.
   * @returns {void}
   */
  _handleClick(event) {
    const target = event.target;
    if (target.classList.contains('component-button')) {
      this._handleButtonClick(event);
    }
  }

  /**
   * Handles button click.
   * @method _handleButtonClick
   * @private
   * @param {MouseEvent} event - The click event.
   * @returns {void}
   */
  _handleButtonClick(event) {
    this._dispatchCustomEvent('button-click', { event });
  }

  /**
   * Handles input events.
   * @method _handleInput
   * @private
   * @param {InputEvent} event - The input event.
   * @returns {void}
   */
  _handleInput(event) {
    const target = event.target;
    if (target.classList.contains('component-input')) {
      this._dispatchCustomEvent('input-change', {
        value: target.value,
        event,
      });
    }
  }

  /**
   * Handles submit events.
   * @method _handleSubmit
   * @private
   * @param {Event} event - The submit event.
   * @returns {void}
   */
  _handleSubmit(event) {
    event.preventDefault();
    this._dispatchCustomEvent('form-submit', { event });
  }

  /**
   * Handles keydown events.
   * @method _handleKeyDown
   * @private
   * @param {KeyboardEvent} event - The keydown event.
   * @returns {void}
   */
  _handleKeyDown(event) {
    if (event.key === 'Enter') {
      this._dispatchCustomEvent('enter-key', { event });
    }
  }

  /**
   * Handles focus events.
   * @method _handleFocus
   * @private
   * @param {FocusEvent} event - The focus event.
   * @returns {void}
   */
  _handleFocus(event) {
    this._dispatchCustomEvent('component-focus', { event });
  }

  /**
   * Handles blur events.
   * @method _handleBlur
   * @private
   * @param {FocusEvent} event - The blur event.
   * @returns {void}
   */
  _handleBlur(event) {
    this._dispatchCustomEvent('component-blur', { event });
  }

  /**
   * Handles attribute changes.
   * @method _handleAttributeChange
   * @private
   * @param {string} name - The attribute name.
   * @param {string|null} oldValue - The old value.
   * @param {string|null} newValue - The new value.
   * @returns {void}
   */
  _handleAttributeChange(name, oldValue, newValue) {
    this._dispatchCustomEvent('attribute-change', {
      attribute: name,
      oldValue,
      newValue,
    });
  }

  /**
   * Renders the component UI.
   * @method _render
   * @private
   * @returns {void}
   */
  _render() {
    this._state.renderCount++;
    this._state.lastRenderTime = new Date().toISOString();
    
    const label = this.getAttribute('label') || 'Shadow Component';
    const placeholder = this.getAttribute('placeholder') || 'Enter text...';
    const size = this.getAttribute('size') || 'medium';
    const variant = this.getAttribute('variant') || 'default';
    const disabled = this.hasAttribute('disabled');

    const template = document.createElement('template');
    template.innerHTML = `
      <div class="component-container" data-variant="${variant}" data-size="${size}">
        <div class="component-header">
          <h2 class="component-title">${label}</h2>
          <span class="component-badge">v1.0.0</span>
        </div>
        
        <div class="component-content">
          <input 
            type="text" 
            class="component-input" 
            placeholder="${placeholder}"
            ${disabled ? 'disabled' : ''}
            aria-label="${label}"
          />
          
          <div class="state-grid">
            <div class="state-item">
              <span class="state-label">Ready:</span>
              <span class="state-value">${this._state.isReady}</span>
            </div>
            <div class="state-item">
              <span class="state-label">Mounted:</span>
              <span class="state-value">${this._state.isMounted}</span>
            </div>
            <div class="state-item">
              <span class="state-label">Render Count:</span>
              <span class="state-value">${this._state.renderCount}</span>
            </div>
            <div class="state-item">
              <span class="state-label">Error Count:</span>
              <span class="state-value">${this._state.errorCount}</span>
            </div>
          </div>
        </div>
        
        <div class="component-footer">
          <div class="status-indicator">
            <span class="status-dot"></span>
            <span>Active</span>
          </div>
          <button class="component-button" type="button">Action</button>
        </div>
      </div>
    `;

    this.shadowRoot.appendChild(template.content.cloneNode(true));
  }

  /**
   * Dispatches a custom lifecycle event.
   * @method _dispatchLifecycleEvent
   * @private
   * @param {string} phase - The lifecycle phase.
   * @returns {void}
   */
  _dispatchLifecycleEvent(phase) {
    this.dispatchEvent(
      new CustomEvent(`component-${phase}`, {
        bubbles: true,
        composed: true,
        detail: { component: this, timestamp: Date.now() },
      })
    );
  }

  /**
   * Dispatches a custom event with detail.
   * @method _dispatchCustomEvent
   * @private
   * @param {string} eventName - The event name.
   * @param {Object} detail - The event detail.
   * @returns {void}
   */
  _dispatchCustomEvent(eventName, detail) {
    this.dispatchEvent(
      new CustomEvent(eventName, {
        bubbles: true,
        composed: true,
        detail: { ...detail, component: this, timestamp: Date.now() },
      })
    );
  }

  /**
   * Handles errors with error boundary.
   * @method _handleError
   * @private
   * @param {string} source - The error source.
   * @param {Error} error - The error object.
   * @returns {void}
   */
  _handleError(source, error) {
    this._state.errorCount++;
    console.error(`[ShadowDOMCRAGuide] Error in ${source}:`, error);
    this._dispatchCustomEvent('component-error', {
      source,
      error: error.message,
      stack: error.stack,
      errorCount: this._state.errorCount,
    });
  }

  /**
   * Gets the current state of the component.
   * @method getState
   * @public
   * @returns {Object} The component state.
   */
  getState() {
    return { ...this._state };
  }

  /**
   * Gets the value of a specified attribute.
   * @method getProperty
   * @public
   * @param {string} name - The property name.
   * @returns {*} The property value.
   */
  getProperty(name) {
    return this._state[name];
  }

  /**
   * Sets the value of a specified property.
   * @method setProperty
   * @public
   * @param {string} name - The property name.
   * @param {*} value - The property value.
   * @returns {void}
   */
  setProperty(name, value) {
    if (this._state.hasOwnProperty(name)) {
      this._state[name] = value;
      this._render();
    }
  }

  /**
   * Forces a re-render of the component.
   * @method forceUpdate
   * @public
   * @returns {void}
   */
  forceUpdate() {
    this.shadowRoot.innerHTML = '';
    this._initializeComponent();
    this._render();
  }

  /**
   * Destroys the component and cleans up resources.
   * @method destroy
   * @public
   * @returns {void}
   */
  destroy() {
    this._cleanupEventListeners();
    this._cleanupObservers();
    this._cleanupTimers();
    this._state = { ...this._initialState };
    this.shadowRoot.innerHTML = '';
  }
}

customElements.define('shadow-cra-guide', ShadowDOMCRAGuide);

export { ShadowDOMCRAGuide };