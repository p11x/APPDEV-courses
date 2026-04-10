/**
 * Event-Bubbling-and-Targeting - Event handling in Shadow DOM
 * @module 04_Shadow-DOM/Event-Bubbling-and-Targeting
 * @version 1.0.0
 * @example <event-bubbling-and-targeting></event-bubbling-and-targeting>
 */

class EventBubblingAndTargeting extends HTMLElement {
  /**
   * Creates an instance of EventBubblingAndTargeting.
   * @constructor
   */
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._eventConfig = {
      bubbles: true,
      composed: true,
      cancelable: false,
      capturePhase: false,
      eventPrefix: 'wc',
      useCustomEvents: true,
      eventLogger: true,
    };
    this._registeredHandlers = new Map();
    this._eventHistory = [];
    this._maxHistorySize = 50;
    this._eventTargetMap = new Map();
    this._delegationEnabled = true;
    this._pendingEvents = [];
  }

  /**
   * Lifecycle callback when the element is added to the DOM.
   * @method connectedCallback
   * @returns {void}
   */
  connectedCallback() {
    try {
      this._render();
      this._setupEventDelegation();
      this._registerInternalHandlers();
      this._logEvent('connected', { timestamp: Date.now() });
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
    this._cleanupEventHandlers();
    this._logEvent('disconnected', { timestamp: Date.now() });
  }

  /**
   * Lifecycle callback when an attribute changes.
   * @method observedAttributes
   * @returns {string[]} Array of observed attribute names.
   */
  static get observedAttributes() {
    return ['bubbles', 'verbose', 'track-targets'];
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
      case 'verbose':
        this._eventConfig.eventLogger = newValue === 'true';
        break;
      case 'track-targets':
        this._configureTargetTracking(newValue === 'true');
        break;
    }
  }

  /**
   * Renders the component template.
   * @method _render
   * @private
   * @returns {void}
   */
  _render() {
    const template = document.createElement('template');
    template.innerHTML = `
      <style>
        :host {
          display: block;
          --event-bg: #ffffff;
          --event-border: #e0e0e0;
          --event-text: #333333;
          --event-accent: #6200ee;
          --event-success: #4caf50;
          --event-error: #f44336;
          --event-warning: #ff9800;
          --event-info: #2196f3;
          --event-radius: 8px;
          --event-padding: 16px;
        }

        :host([bubbles="false"]) {
          --event-accent: #9e9e9e;
        }

        .event-container {
          background: var(--event-bg);
          border: 1px solid var(--event-border);
          border-radius: var(--event-radius);
          padding: var(--event-padding);
          color: var(--event-text);
        }

        .event-header {
          display: flex;
          align-items: center;
          justify-content: space-between;
          margin-bottom: 16px;
          padding-bottom: 12px;
          border-bottom: 1px solid var(--event-border);
        }

        .event-title {
          font-size: 18px;
          font-weight: 600;
          margin: 0;
        }

        .config-badge {
          display: inline-flex;
          align-items: center;
          gap: 6px;
          padding: 4px 10px;
          background: rgba(0, 0, 0, 0.08);
          border-radius: 12px;
          font-size: 11px;
          font-weight: 500;
          text-transform: uppercase;
        }

        .config-badge.active {
          background: var(--event-success);
          color: white;
        }

        .target-area {
          position: relative;
          min-height: 200px;
          padding: 12px;
          background: rgba(0, 0, 0, 0.03);
          border-radius: 4px;
        }

        .target-layer {
          position: relative;
          padding: 16px;
          margin: 8px 0;
          border: 2px dashed;
          border-radius: 4px;
          transition: all 0.2s ease;
        }

        .target-layer:hover {
          transform: translateX(4px);
        }

        .layer-outer {
          border-color: var(--event-error);
          background: rgba(244, 67, 54, 0.05);
        }

        .layer-middle {
          border-color: var(--event-warning);
          background: rgba(255, 152, 0, 0.05);
        }

        .layer-inner {
          border-color: var(--event-success);
          background: rgba(76, 175, 80, 0.05);
        }

        .layer-core {
          border-color: var(--event-accent);
          background: rgba(98, 0, 238, 0.1);
        }

        .layer-label {
          display: inline-flex;
          align-items: center;
          gap: 8px;
          padding: 4px 12px;
          background: white;
          border-radius: 4px;
          font-size: 12px;
          font-weight: 600;
        }

        .trigger-button {
          display: inline-flex;
          align-items: center;
          justify-content: center;
          padding: 10px 20px;
          background: var(--event-accent);
          color: white;
          border: none;
          border-radius: 4px;
          font-size: 14px;
          font-weight: 500;
          cursor: pointer;
          margin: 8px 4px;
          transition: all 0.2s ease;
        }

        .trigger-button:hover {
          filter: brightness(1.1);
          transform: translateY(-1px);
        }

        .trigger-button:active {
          transform: translateY(0);
        }

        .trigger-button.secondary {
          background: transparent;
          border: 2px solid var(--event-accent);
          color: var(--event-accent);
        }

        .trigger-button.danger {
          background: var(--event-error);
        }

        .event-log {
          max-height: 250px;
          overflow-y: auto;
          margin-top: 16px;
          border: 1px solid var(--event-border);
          border-radius: 4px;
        }

        .log-header {
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 12px;
          background: rgba(0, 0, 0, 0.05);
          font-weight: 600;
          font-size: 13px;
          position: sticky;
          top: 0;
        }

        .log-entry {
          display: flex;
          align-items: flex-start;
          padding: 8px 12px;
          border-bottom: 1px solid var(--event-border);
          font-size: 12px;
          animation: slideIn 0.2s ease;
        }

        .log-entry:last-child {
          border-bottom: none;
        }

        @keyframes slideIn {
          from { opacity: 0; transform: translateX(-10px); }
          to { opacity: 1; transform: translateX(0); }
        }

        .log-timestamp {
          width: 80px;
          flex-shrink: 0;
          font-family: monospace;
          color: #666;
        }

        .log-type {
          width: 100px;
          flex-shrink: 0;
          font-weight: 600;
          text-transform: uppercase;
        }

        .log-type.click { color: var(--event-accent); }
        .log-type.bubbles { color: var(--event-success); }
        .log-type.composed { color: var(--event-info); }
        .log-type.error { color: var(--event-error); }

        .log-detail {
          flex: 1;
          color: #666;
          word-break: break-word;
        }

        .event-path {
          display: flex;
          flex-wrap: wrap;
          gap: 4px;
          margin: 12px 0;
          padding: 8px;
          background: rgba(0, 0, 0, 0.03);
          border-radius: 4px;
        }

        .path-node {
          display: inline-flex;
          padding: 4px 8px;
          background: var(--event-accent);
          color: white;
          border-radius: 4px;
          font-size: 11px;
          font-family: monospace;
        }

        .clear-button {
          padding: 6px 12px;
          background: transparent;
          border: 1px solid var(--event-border);
          border-radius: 4px;
          font-size: 12px;
          cursor: pointer;
          transition: all 0.2s ease;
        }

        .clear-button:hover {
          background: var(--event-error);
          color: white;
          border-color: var(--event-error);
        }
      </style>

      <div class="event-container">
        <div class="event-header">
          <h2 class="event-title">Event Bubbling & Targeting</h2>
          <span class="config-badge active">Composed: true</span>
        </div>

        <div class="event-controls" style="margin-bottom: 16px;">
          <button class="trigger-button" data-action="bubble">Fire Bubble Event</button>
          <button class="trigger-button secondary" data-action="direct">Fire Direct Event</button>
          <button class="trigger-button" data-action="composed">Fire Composed</button>
        </div>

        <div class="target-area">
          <div class="target-layer layer-outer">
            <span class="layer-label">Outer Layer</span>
            <div class="target-layer layer-middle">
              <span class="layer-label">Middle Layer</span>
              <div class="target-layer layer-inner">
                <span class="layer-label">Inner Layer</span>
                <div class="target-layer layer-core">
                  <span class="layer-label">Core Layer</span>
                  <button class="trigger-button" data-action="click">Click Me</button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="event-path" id="eventPath"></div>

        <div class="event-log">
          <div class="log-header">
            <span>Event Log</span>
            <button class="clear-button" id="clearLog">Clear</button>
          </div>
          <div id="logEntries"></div>
        </div>
      </div>
    `;

    this.shadowRoot.appendChild(template.content.cloneNode(true));
  }

  /**
   * Sets up event delegation.
   * @method _setupEventDelegation
   * @private
   * @returns {void}
   */
  _setupEventDelegation() {
    this._delegationHandler = this._handleDelegatedEvent.bind(this);
    this.addEventListener('click', this._delegationHandler);
    this.addEventListener('focus', this._delegationHandler);
    this.addEventListener('blur', this._delegationHandler);
  }

  /**
   * Registers internal event handlers.
   * @method _registerInternalHandlers
   * @private
   * @returns {void}
   */
  _registerInternalHandlers() {
    this._clearButton = this.shadowRoot.getElementById('clearLog');
    if (this._clearButton) {
      this._clearButton.addEventListener('click', () => this._clearEventLog());
    }

    const buttons = this.shadowRoot.querySelectorAll('.trigger-button');
    buttons.forEach((button) => {
      const handler = () => this._handleTriggerButton(button.dataset.action);
      button.addEventListener('click', handler);
      this._registeredHandlers.set(button, handler);
    });
  }

  /**
   * Handles delegated events.
   * @method _handleDelegatedEvent
   * @private
   * @param {Event} event - The event object.
   * @returns {void}
   */
  _handleDelegatedEvent(event) {
    if (!this._delegationEnabled) return;

    const composed = event.composed && event.composedPath ? event.composedPath() : [event.target];
    this._logEvent(event.type, {
      target: event.target.tagName,
      bubbles: event.bubbles,
      composed: event.composed,
      path: composed.map((n) => n.tagName || n.nodeName).join(' > '),
    });

    this._updateEventPath(event);
  }

  /**
   * Handles trigger button clicks.
   * @method _handleTriggerButton
   * @private
   * @param {string} action - The action type.
   * @returns {void}
   */
  _handleTriggerButton(action) {
    switch (action) {
      case 'bubble':
        this._dispatchCustomEvent('bubble-test', { bubbles: true, composed: true });
        break;
      case 'direct':
        this._dispatchCustomEvent('direct-test', { bubbles: false, composed: false });
        break;
      case 'composed':
        this._fireComposedEvent();
        break;
      case 'click':
        this._logEvent('element-click', { action: 'button clicked', bubbles: true });
        break;
    }
  }

  /**
   * Fires a composed event through shadow DOM.
   * @method _fireComposedEvent
   * @private
   * @returns {void}
   */
  _fireComposedEvent() {
    const eventInit = {
      bubbles: true,
      composed: true,
      cancelable: true,
      detail: { source: 'shadow-dom', timestamp: Date.now() },
    };

    const event = new CustomEvent('composed-test', eventInit);
    this.dispatchEvent(event);

    this._logEvent('composed', {
      type: 'CustomEvent',
      bubbles: eventInit.bubbles,
      composed: eventInit.composed,
    });
  }

  /**
   * Dispatches a custom event.
   * @method _dispatchCustomEvent
   * @private
   * @param {string} eventName - The event name.
   * @param {Object} options - Event options.
   * @returns {void}
   */
  _dispatchCustomEvent(eventName, options) {
    const eventInit = {
      bubbles: options.bubbles !== false,
      composed: options.composed !== false,
      cancelable: options.cancelable || false,
      detail: options.detail || {},
    };

    const event = new CustomEvent(`${this._eventConfig.eventPrefix}-${eventName}`, eventInit);
    this.dispatchEvent(event);

    this._logEvent(eventName, {
      bubbles: eventInit.bubbles,
      composed: eventInit.composed,
    });
  }

  /**
   * Updates the event path display.
   * @method _updateEventPath
   * @private
   * @param {Event} event - The event.
   * @returns {void}
   */
  _updateEventPath(event) {
    const pathContainer = this.shadowRoot.getElementById('eventPath');
    if (!pathContainer) return;

    const path = event.composedPath ? event.composedPath() : [];
    pathContainer.innerHTML = path
      .map((node) => `<span class="path-node">${node.tagName || node.nodeName}</span>`)
      .join('');
  }

  /**
   * Logs an event.
   * @method _logEvent
   * @private
   * @param {string} type - Event type.
   * @param {Object} detail - Event detail.
   * @returns {void}
   */
  _logEvent(type, detail) {
    const entry = {
      id: Date.now(),
      type,
      detail,
      timestamp: new Date().toLocaleTimeString(),
    };

    this._eventHistory.push(entry);

    if (this._eventHistory.length > this._maxHistorySize) {
      this._eventHistory.shift();
    }

    if (this._eventConfig.eventLogger) {
      this._renderLogEntry(entry);
    }

    this._dispatchLogEvent(entry);
  }

  /**
   * Renders a log entry.
   * @method _renderLogEntry
   * @private
   * @param {Object} entry - The log entry.
   * @returns {void}
   */
  _renderLogEntry(entry) {
    const container = this.shadowRoot.getElementById('logEntries');
    if (!container) return;

    const div = document.createElement('div');
    div.className = 'log-entry';
    div.innerHTML = `
      <span class="log-timestamp">${entry.timestamp}</span>
      <span class="log-type ${entry.type}">${entry.type}</span>
      <span class="log-detail">${this._formatDetail(entry.detail)}</span>
    `;

    container.insertBefore(div, container.firstChild);
  }

  /**
   * Formats detail for display.
   * @method _formatDetail
   * @private
   * @param {Object} detail - The detail object.
   * @returns {string} Formatted string.
   */
  _formatDetail(detail) {
    if (!detail) return '';
    
    return Object.entries(detail)
      .map(([key, value]) => `${key}: ${value}`)
      .join(', ');
  }

  /**
   * Clears the event log.
   * @method _clearEventLog
   * @private
   * @returns {void}
   */
  _clearEventLog() {
    this._eventHistory = [];
    const container = this.shadowRoot.getElementById('logEntries');
    if (container) {
      container.innerHTML = '';
    }
  }

  /**
   * Dispatches log event.
   * @method _dispatchLogEvent
   * @private
   * @param {Object} entry - The log entry.
   * @returns {void}
   */
  _dispatchLogEvent(entry) {
    this.dispatchEvent(
      new CustomEvent('event-logged', {
        bubbles: true,
        composed: true,
        detail: entry,
      })
    );
  }

  /**
   * Configures target tracking.
   * @method _configureTargetTracking
   * @private
   * @param {boolean} enabled - Whether tracking is enabled.
   * @returns {void}
   */
  _configureTargetTracking(enabled) {
    this._eventTargetMap.clear();
  }

  /**
   * Cleans up event handlers.
   * @method _cleanupEventHandlers
   * @private
   * @returns {void}
   */
  _cleanupEventHandlers() {
    this._registeredHandlers.forEach((handler, target) => {
      target.removeEventListener('click', handler);
    });
    this._registeredHandlers.clear();

    if (this._delegationHandler) {
      this.removeEventListener('click', this._delegationHandler);
      this.removeEventListener('focus', this._delegationHandler);
      this.removeEventListener('blur', this._delegationHandler);
    }
  }

  /**
   * Handles errors.
   * @method _handleError
   * @private
   * @param {string} source - Error source.
   * @param {Error} error - The error.
   * @returns {void}
   */
  _handleError(source, error) {
    console.error(`[EventBubblingAndTargeting] ${source}:`, error);
    this._logEvent('error', { source, message: error.message });
  }

  /**
   * Gets event history.
   * @method getEventHistory
   * @public
   * @returns {Object[]} Array of event entries.
   */
  getEventHistory() {
    return [...this._eventHistory];
  }

  /**
   * Clears event history.
   * @method clearHistory
   * @public
   * @returns {void}
   */
  clearHistory() {
    this._clearEventLog();
  }

  /**
   * Creates and dispatches an event.
   * @method dispatchEvent
   * @public
   * @param {string} eventName - Event name.
   * @param {Object} options - Event options.
   * @returns {void}
   */
  dispatchNamedEvent(eventName, options) {
    this._dispatchCustomEvent(eventName, options);
  }
}

customElements.define('event-bubbling-and-targeting', EventBubblingAndTargeting);

export { EventBubblingAndTargeting };