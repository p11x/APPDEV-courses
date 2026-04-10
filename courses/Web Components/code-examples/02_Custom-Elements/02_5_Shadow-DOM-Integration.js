/**
 * Shadow DOM Integration - Implementation Guide
 * @description Complete Shadow DOM implementation examples
 * @module custom-elements/shadow-dom
 * @version 1.0.0
 */

// ============================================
// Shadow DOM Basic Integration
// ============================================

/**
 * ShadowDOMElement - Basic Shadow DOM implementation
 */
class ShadowDOMElement extends HTMLElement {
  constructor() {
    super();
    
    // Mode options: 'open' - accessible via element.shadowRoot
    //              'closed' - shadowRoot returns null
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
  }

  render() {
    // All content goes into shadow root - isolated from main DOM
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
        }
        
        /* :host targets the custom element itself */
        :host([highlight]) {
          background: #f0f0f0;
        }
        
        /* :host() conditional styling */
        :host([variant="primary"]) {
          --accent-color: #667eea;
        }
        
        :host([variant="secondary"]) {
          --accent-color: #28a745;
        }
        
        .container {
          background: white;
          border-radius: 8px;
          padding: 20px;
          box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        
        h2 {
          margin: 0 0 10px;
          color: var(--accent-color, #333);
        }
        
        /* ::slotted() styles for distributed content */
        ::slotted(*) {
          color: #555;
        }
      </style>
      
      <div class="container">
        <h2><slot name="title">Default Title</slot></h2>
        <div class="content">
          <slot></slot>
        </div>
        <div class="footer">
          <slot name="footer"></slot>
        </div>
      </div>
    `;
  }
}

customElements.define('shadow-element', ShadowDOMElement);

// ============================================
// Open vs Closed Mode
// ============================================

/**
 * OpenShadowElement - Shadow root accessible
 */
class OpenShadowElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });  // Accessible
  }

  connectedCallback() {
    this.shadowRoot.innerHTML = '<div>Open Mode</div>';
    // Can access: element.shadowRoot.querySelector(...)
  }
}

/**
 * ClosedShadowElement - Shadow root not accessible
 */
class ClosedShadowElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'closed' });  // Not accessible
  }

  connectedCallback() {
    const shadow = this.attachShadow({ mode: 'closed' });
    shadow.innerHTML = '<div>Closed Mode</div>';
    // this.shadowRoot === null from outside
  }
}

// ============================================
// Slot Distribution
// ============================================

/**
 * CardWithSlots - Demonstrates named slots
 */
class CardWithSlots extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host { display: block; }
        
        .card {
          background: white;
          border-radius: 12px;
          overflow: hidden;
          box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        
        .header {
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          color: white;
          padding: 16px 20px;
        }
        
        .body {
          padding: 20px;
        }
        
        .footer {
          padding: 12px 20px;
          background: #f8f9fa;
          border-top: 1px solid #eee;
        }
        
        /* Fallback content when slot is empty */
        slot[name="header"]::slotted(*),
        slot[name="footer"]::slotted(*) {
          color: inherit;
        }
      </style>
      
      <div class="card">
        <div class="header">
          <slot name="header">Default Header</slot>
        </div>
        
        <div class="body">
          <slot></slot>
        </div>
        
        <div class="footer">
          <slot name="footer"></slot>
        </div>
      </div>
    `;
  }
}

customElements.define('card-with-slots', CardWithSlots);

// Usage:
// <card-with-slots>
//   <span slot="header">Custom Header</span>
//   <div>Main content goes here</div>
//   <button slot="footer">Action</button>
// </card-with-slots>

// ============================================
// Shadow DOM Event Handling
// ============================================

/**
 * ShadowEventElement - Events with composed path
 */
class ShadowEventElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
    this._bindEvents();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <button id="btn">Click Me</button>
    `;
  }

  _bindEvents() {
    const btn = this.shadowRoot.getElementById('btn');
    btn.addEventListener('click', (e) => {
      // By default, event.target is retargeted to host
      
      // To cross shadow boundary, dispatch new event with composed: true
      this.dispatchEvent(new CustomEvent('internal-click', {
        bubbles: true,
        composed: true,  // Allow crossing shadow boundary
        detail: {
          timestamp: Date.now(),
          composedPath: e.composedPath()
        }
      }));
    });
  }
}

customElements.define('shadow-event', ShadowEventElement);

export { ShadowDOMElement, OpenShadowElement, ClosedShadowElement, CardWithSlots, ShadowEventElement };