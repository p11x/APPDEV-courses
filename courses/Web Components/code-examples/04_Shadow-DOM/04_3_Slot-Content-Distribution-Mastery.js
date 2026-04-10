/**
 * Slot-Content-Distribution-Mastery - Content projection with slots
 * @module 04_Shadow-DOM/Slot-Content-Distribution-Mastery
 * @version 1.0.0
 * @example <slot-content-distribution-mastery></slot-content-distribution-mastery>
 */

class SlotContentDistributionMastery extends HTMLElement {
  /**
   * Creates an instance of SlotContentDistributionMastery.
   * @constructor
   */
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._slotConfig = {
      defaultSlotName: 'content',
      fallbackEnabled: true,
      dynamicSlots: true,
      slotChangeDetection: true,
    };
    this._slots = new Map();
    this._slotObservers = [];
    this._distributedNodes = new Map();
    this._fallbackContent = new Map();
    this._activeSlot = 'default';
  }

  /**
   * Lifecycle callback when the element is added to the DOM.
   * @method connectedCallback
   * @returns {void}
   */
  connectedCallback() {
    try {
      this._setupTemplate();
      this._initializeSlots();
      this._setupSlotObservers();
      this._render();
      this._bindEvents();
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
    this._cleanupSlotObservers();
    this._cleanupEventListeners();
  }

  /**
   * Lifecycle callback when an attribute changes.
   * @method observedAttributes
   * @returns {string[]} Array of observed attribute names.
   */
  static get observedAttributes() {
    return ['mode', 'layout', 'show-fallback', 'slot-count'];
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
    
    switch (name) {
      case 'show-fallback':
        this._updateFallbackVisibility(newValue === 'true');
        break;
      case 'mode':
        this._updateMode(newValue);
        break;
      case 'layout':
        this._updateLayout(newValue);
        break;
    }
  }

  /**
   * Sets up the component template.
   * @method _setupTemplate
   * @private
   * @returns {void}
   */
  _setupTemplate() {
    const style = document.createElement('style');
    style.textContent = this._getStyles();
    this.shadowRoot.appendChild(style);
    
    const container = document.createElement('div');
    container.className = 'slot-container';
    this.shadowRoot.appendChild(container);
  }

  /**
   * Gets the component styles.
   * @method _getStyles
   * @private
   * @returns {string} The CSS styles.
   */
  _getStyles() {
    return `
      :host {
        display: block;
        --slot-bg: #ffffff;
        --slot-border: #e0e0e0;
        --slot-text: #333333;
        --slot-accent: #6200ee;
        --slot-secondary: #03dac6;
        --slot-error: #b00020;
        --slot-radius: 8px;
        --slot-padding: 16px;
        --slot-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      }

      :host([mode="dark"]) {
        --slot-bg: #1a1a1a;
        --slot-border: #333333;
        --slot-text: #ffffff;
        --slot-accent: #bb86fc;
      }

      .slot-container {
        background: var(--slot-bg);
        border: 1px solid var(--slot-border);
        border-radius: var(--slot-radius);
        padding: var(--slot-padding);
        color: var(--slot-text);
      }

      .slot-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 12px;
        border-bottom: 1px solid var(--slot-border);
        margin: calc(-1 * var(--slot-padding));
        margin-bottom: var(--slot-padding);
      }

      .slot-title {
        font-size: 18px;
        font-weight: 600;
        margin: 0;
      }

      .slot-controls {
        display: flex;
        gap: 8px;
      }

      .control-button {
        padding: 6px 12px;
        background: var(--slot-bg);
        border: 1px solid var(--slot-border);
        border-radius: 4px;
        color: var(--slot-text);
        font-size: 12px;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .control-button:hover {
        background: var(--slot-accent);
        color: white;
        border-color: var(--slot-accent);
      }

      .control-button.active {
        background: var(--slot-accent);
        color: white;
        border-color: var(--slot-accent);
      }

      .slot-content-area {
        position: relative;
        min-height: 100px;
        padding: 16px;
      }

      .slot-panel {
        display: none;
        animation: fadeIn 0.3s ease;
      }

      .slot-panel.active {
        display: block;
      }

      @keyframes fadeIn {
        from { opacity: 0; transform: translateY(10px); }
        to { opacity: 1; transform: translateY(0); }
      }

      ::slotted(*) {
        margin: 8px 0;
      }

      ::slotted(.highlight) {
        background: var(--slot-accent);
        color: white;
        padding: 4px 8px;
        border-radius: 4px;
      }

      ::slotted([slot="header"]) {
        font-weight: 600;
        font-size: 16px;
      }

      ::sloted([slot="footer"]) {
        font-size: 12px;
        color: #666;
      }

      ::slotted(img) {
        max-width: 100%;
        border-radius: 4px;
      }

      ::slotted(a) {
        color: var(--slot-accent);
        text-decoration: none;
      }

      ::slotted(a:hover) {
        text-decoration: underline;
      }

      .fallback-content {
        padding: 20px;
        text-align: center;
        color: #999;
        background: var(--slot-bg);
        border: 2px dashed var(--slot-border);
        border-radius: var(--slot-radius);
      }

      .fallback-content.hidden {
        display: none;
      }

      .slot-info {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 12px;
        background: var(--slot-bg);
        border-radius: 4px;
        margin: 12px 0;
      }

      .slot-indicator {
        width: 10px;
        height: 10px;
        border-radius: 50%;
        background: var(--slot-accent);
      }

      .slot-info-text {
        font-size: 13px;
      }

      .slot-selector {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin-bottom: 16px;
      }

      .selector-item {
        padding: 8px 16px;
        background: var(--slot-bg);
        border: 1px solid var(--slot-border);
        border-radius: 20px;
        font-size: 13px;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .selector-item:hover {
        border-color: var(--slot-accent);
      }

      .selector-item.active {
        background: var(--slot-accent);
        color: white;
        border-color: var(--slot-accent);
      }

      .nested-slot-wrapper {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 16px;
        margin: 16px 0;
      }

      .nested-panel {
        padding: 16px;
        background: var(--slot-bg);
        border: 1px solid var(--slot-border);
        border-radius: var(--slot-radius);
      }

      .nested-title {
        font-size: 14px;
        font-weight: 600;
        margin-bottom: 8px;
        padding-bottom: 8px;
        border-bottom: 1px solid var(--slot-border);
      }

      .slot-status {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 8px 12px;
        background: rgba(0, 0, 0, 0.05);
        border-radius: 4px;
        font-size: 12px;
      }

      .status-label {
        font-weight: 500;
        color: #666;
      }

      .status-value {
        font-family: monospace;
      }
    `;
  }

  /**
   * Initializes slot elements.
   * @method _initializeSlots
   * @private
   * @returns {void}
   */
  _initializeSlots() {
    const container = this.shadowRoot.querySelector('.slot-container');
    if (!container) return;

    const slotDefinitions = [
      { name: 'default', fallback: 'Default content goes here...' },
      { name: 'header', fallback: '<h3>No header provided</h3>' },
      { name: 'body', fallback: '<p>No body content provided</p>' },
      { name: 'footer', fallback: '<small>No footer provided</small>' },
      { name: 'actions', fallback: '<button>Default Action</button>' },
      { name: 'media', fallback: '<div>No media slot</div>' },
    ];

    slotDefinitions.forEach(({ name, fallback }) => {
      if (name === 'default') {
        const defaultSlot = document.createElement('slot');
        defaultSlot.className = 'default-slot';
        container.appendChild(defaultSlot);
        this._slots.set('default', defaultSlot);
      } else {
        const namedSlot = document.createElement('slot');
        namedSlot.setAttribute('name', name);
        namedSlot.className = `${name}-slot`;
        container.appendChild(namedSlot);
        this._slots.set(name, namedSlot);
      }
      
      this._fallbackContent.set(name, fallback);
      this._distributedNodes.set(name, []);
    });
  }

  /**
   * Sets up slot change observers.
   * @method _setupSlotObservers
   * @private
   * @returns {void}
   */
  _setupSlotObservers() {
    if (!this._slotConfig.slotChangeDetection) return;

    this._slots.forEach((slot, name) => {
      const observer = new MutationObserver((mutations) => {
        mutations.forEach((mutation) => {
          if (mutation.type === 'childList') {
            this._handleSlotChange(name, mutation.addedNodes, mutation.removedNodes);
          }
        });
      });

      observer.observe(slot, { childList: true });
      this._slotObservers.push({ slot, observer });
    });
  }

  /**
   * Handles slot content changes.
   * @method _handleSlotChange
   * @private
   * @param {string} slotName - The slot name.
   * @param {NodeList} addedNodes - Added nodes.
   * @param {NodeList} removedNodes - Removed nodes.
   * @returns {void}
   */
  _handleSlotChange(slotName, addedNodes, removedNodes) {
    const nodes = Array.from(this.children).filter(
      (node) => node.getAttribute?.('slot') === slotName
    );
    
    this._distributedNodes.set(slotName, nodes);
    
    this.dispatchEvent(
      new CustomEvent('slot-change', {
        bubbles: true,
        composed: true,
        detail: {
          slotName,
          hasContent: nodes.length > 0,
          nodeCount: nodes.length,
        },
      })
    );
  }

  /**
   * Cleans up slot observers.
   * @method _cleanupSlotObservers
   * @private
   * @returns {void}
   */
  _cleanupSlotObservers() {
    this._slotObservers.forEach(({ observer }) => observer.disconnect());
    this._slotObservers = [];
  }

  /**
   * Cleans up event listeners.
   * @method _cleanupEventListeners
   * @private
   * @returns {void}
   */
  _cleanupEventListeners() {
    this._clickHandlers?.forEach((handler, target) => {
      target.removeEventListener('click', handler);
    });
  }

  /**
   * Updates fallback content visibility.
   * @method _updateFallbackVisibility
   * @private
   * @param {boolean} visible - Whether to show fallback.
   * @returns {void}
   */
  _updateFallbackVisibility(visible) {
    const fallbacks = this.shadowRoot.querySelectorAll('.fallback-content');
    fallbacks.forEach((el) => {
      el.classList.toggle('hidden', !visible);
    });
  }

  /**
   * Updates display mode.
   * @method _updateMode
   * @private
   * @param {string} mode - The mode value.
   * @returns {void}
   */
  _updateMode(mode) {
    if (mode === 'dark') {
      this.setAttribute('mode', 'dark');
    } else {
      this.removeAttribute('mode');
    }
  }

  /**
   * Updates layout.
   * @method _updateLayout
   * @private
   * @param {string} layout - The layout type.
   * @returns {void}
   */
  _updateLayout(layout) {
    const container = this.shadowRoot.querySelector('.slot-container');
    if (container) {
      container.setAttribute('data-layout', layout);
    }
  }

  /**
   * Renders the component.
   * @method _render
   * @private
   * @returns {void}
   */
  _render() {
    const container = this.shadowRoot.querySelector('.slot-container');
    if (!container) return;

    const content = `
      <div class="slot-header">
        <h2 class="slot-title">Content Distribution</h2>
        <div class="slot-controls">
          <button class="control-button" data-slot="default">Default</button>
          <button class="control-button" data-slot="header">Header</button>
          <button class="control-button" data-slot="body">Body</button>
          <button class="control-button" data-slot="footer">Footer</button>
        </div>
      </div>

      <div class="slot-content-area">
        <div class="slot-selector">
          <button class="selector-item active" data-panel="default">Default Slot</button>
          <button class="selector-item" data-panel="named">Named Slots</button>
          <button class="selector-item" data-panel="nested">Nested</button>
          <button class="selector-item" data-panel="fallback">Fallback</button>
        </div>

        <div class="slot-panel active" data-panel="default">
          <slot name="default"></slot>
          <div class="slot-info">
            <span class="slot-indicator"></span>
            <span class="slot-info-text">Default slot - content without slot attribute</span>
          </div>
        </div>

        <div class="slot-panel" data-panel="named">
          <div class="nested-slot-wrapper">
            <div class="nested-panel">
              <div class="nested-title">Header Slot</div>
              <slot name="header"></slot>
            </div>
            <div class="nested-panel">
              <div class="nested-title">Body Slot</div>
              <slot name="body"></slot>
            </div>
            <div class="nested-panel">
              <div class="nested-title">Footer Slot</div>
              <slot name="footer"></slot>
            </div>
          </div>
        </div>

        <div class="slot-panel" data-panel="nested">
          <div class="nested-slot-wrapper">
            <div class="nested-panel">
              <div class="nested-title">Actions</div>
              <slot name="actions"></slot>
              <div class="slot-status">
                <span class="status-label">Available</span>
                <span class="status-value">slot="actions"</span>
              </div>
            </div>
            <div class="nested-panel">
              <div class="nested-title">Media</div>
              <slot name="media"></slot>
              <div class="slot-status">
                <span class="status-label">Available</span>
                <span class="status-value">slot="media"</span>
              </div>
            </div>
          </div>
        </div>

        <div class="slot-panel" data-panel="fallback">
          <div class="fallback-content hidden" data-slot="default">
            <slot name="default">
              <p>No content provided - showing fallback</p>
            </slot>
          </div>
          <div class="fallback-content">
            <p>This is fallback content when no content is distributed to the slot</p>
          </div>
        </div>
      </div>
    `;

    container.innerHTML = content;
  }

  /**
   * Binds event listeners.
   * @method _bindEvents
   * @private
   * @returns {void}
   */
  _bindEvents() {
    this._clickHandlers = new Map();
    
    const handleSlotClick = (event) => {
      const target = event.target;
      
      if (target.classList.contains('control-button')) {
        const slot = target.getAttribute('data-slot');
        this._dispatchSlotSelect(slot);
      }
      
      if (target.classList.contains('selector-item')) {
        const panel = target.getAttribute('data-panel');
        this._switchPanel(panel);
      }
    };

    this.addEventListener('click', handleSlotClick);
    this._clickHandlers.set(this, handleSlotClick);
  }

  /**
   * Switches the active panel.
   * @method _switchPanel
   * @private
   * @param {string} panel - The panel name.
   * @returns {void}
   */
  _switchPanel(panel) {
    const items = this.shadowRoot.querySelectorAll('.selector-item');
    items.forEach((item) => {
      item.classList.toggle('active', item.getAttribute('data-panel') === panel);
    });

    const panels = this.shadowRoot.querySelectorAll('.slot-panel');
    panels.forEach((p) => {
      p.classList.toggle('active', p.getAttribute('data-panel') === panel);
    });
  }

  /**
   * Dispatch slot selection event.
   * @method _dispatchSlotSelect
   * @private
   * @param {string} slotName - The slot name.
   * @returns {void}
   */
  _dispatchSlotSelect(slotName) {
    this.dispatchEvent(
      new CustomEvent('slot-select', {
        bubbles: true,
        composed: true,
        detail: { slotName },
      })
    );
  }

  /**
   * Gets slot content.
   * @method getSlotContent
   * @public
   * @param {string} slotName - The slot name.
   * @returns {NodeList|null} The distributed nodes.
   */
  getSlotContent(slotName) {
    const slot = this._slots.get(slotName);
    return slot ? slot.assignedNodes() : null;
  }

  /**
   * Checks if slot has content.
   * @method hasSlotContent
   * @public
   * @param {string} slotName - The slot name.
   * @returns {boolean} Whether the slot has content.
   */
  hasSlotContent(slotName) {
    const nodes = this.getSlotContent(slotName);
    return nodes !== null && nodes.length > 0;
  }

  /**
   * Gets all slots.
   * @method getSlots
   * @public
   * @returns {Map<string, HTMLSlotElement>} Map of slot elements.
   */
  getSlots() {
    return new Map(this._slots);
  }

  /**
   * Manually assigns nodes to a slot.
   * @method assignNodes
   * @public
   * @param {string} slotName - The slot name.
   * @param {Node[]} nodes - The nodes to assign.
   * @returns {void}
   */
  assignNodes(slotName, nodes) {
    const slot = this._slots.get(slotName);
    if (slot) {
      slot.assign(nodes);
    }
  }

  /**
   * Handles errors.
   * @method _handleError
   * @private
   * @param {string} source - The error source.
   * @param {Error} error - The error.
   * @returns {void}
   */
  _handleError(source, error) {
    console.error(`[SlotContentDistributionMastery] ${source}:`, error);
  }
}

customElements.define('slot-content-distribution-mastery', SlotContentDistributionMastery);

export { SlotContentDistributionMastery };