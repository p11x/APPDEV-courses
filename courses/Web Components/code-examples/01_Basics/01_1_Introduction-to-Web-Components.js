/**
 * Introduction to Web Components - Basic Element Creation
 * @description A simple greeting component demonstrating fundamental Web Component concepts
 * @module basics/intro
 * @version 1.0.0
 */

// ============================================
// Greeting Component - Basic Implementation
// ============================================

class GreetingElement extends HTMLElement {
  /**
   * Creates a new GreetingElement
   * @constructor
   */
  constructor() {
    // Always call super() first to establish prototype chain
    super();
    
    // Attach Shadow DOM for encapsulation
    this.attachShadow({ mode: 'open' });
    
    // Initialize private state
    this._name = 'World';
    this._greetingType = 'hello';
  }

  /**
   * Called when element is added to DOM
   * @ Lifecycle callback
   */
  connectedCallback() {
    this._parseAttributes();
    this.render();
    console.log('[GreetingElement] Component mounted');
  }

  /**
   * Called when element is removed from DOM
   * @ Lifecycle callback for cleanup
   */
  disconnectedCallback() {
    console.log('[GreetingElement] Component unmounted');
  }

  /**
   * Parse attributes on initialization
   * @private
   */
  _parseAttributes() {
    this._name = this.getAttribute('name') || 'World';
    this._greetingType = this.getAttribute('greeting-type') || 'hello';
  }

  /**
   * Static observedAttributes for reactive updates
   * @static
   * @returns {string[]} Array of attribute names to observe
   */
  static get observedAttributes() {
    return ['name', 'greeting-type'];
  }

  /**
   * Called when observed attributes change
   * @param {string} name - Attribute name
   * @param {string} oldValue - Previous value
   * @param {string} newValue - New value
   */
  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      console.log(`[GreetingElement] ${name} changed: ${oldValue} -> ${newValue}`);
      this._parseAttributes();
      this.render();
    }
  }

  /**
   * Getter for name property
   * @returns {string}
   */
  get name() {
    return this._name;
  }

  /**
   * Setter for name property with attribute reflection
   * @param {string} value
   */
  set name(value) {
    this._name = value;
    this.setAttribute('name', value);
  }

  /**
   * Getter for greetingType property
   * @returns {string}
   */
  get greetingType() {
    return this._greetingType;
  }

  /**
   * Setter for greetingType property
   * @param {string} value
   */
  set greetingType(value) {
    this._greetingType = value;
    this.setAttribute('greeting-type', value);
  }

  /**
   * Get greeting message based on type
   * @private
   * @returns {string}
   */
  _getGreeting() {
    const greetings = {
      hello: 'Hello',
      hi: 'Hi',
      hey: 'Hey',
      welcome: 'Welcome'
    };
    return greetings[this._greetingType] || 'Hello';
  }

  /**
   * Render the component template
   */
  render() {
    const greeting = this._getGreeting();
    
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: inline-block;
          font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        }
        
        .greeting {
          padding: 12px 20px;
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          color: white;
          border-radius: 8px;
          font-size: 16px;
          font-weight: 500;
          box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
          transition: transform 0.2s ease, box-shadow 0.2s ease;
        }
        
        :host([highlight]) .greeting {
          transform: scale(1.05);
          box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
        }
        
        .name {
          font-weight: 700;
        }
        
        .greeting:hover {
          transform: translateY(-2px);
          box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
        }
      </style>
      
      <div class="greeting" role="status" aria-live="polite">
        ${greeting}, <span class="name">${this._name}</span>!
      </div>
    `;
  }
}

// Define the custom element
customElements.define('greeting-element', GreetingElement);

// ============================================
// Usage Examples
// ============================================

/*
  HTML Usage:
  
  <greeting-element></greeting-element>
  <greeting-element name="Developer"></greeting-element>
  <greeting-element name="World" greeting-type="hi"></greeting-element>
  
  JavaScript Usage:
  
  const greeting = document.querySelector('greeting-element');
  greeting.name = 'New Name';
  greeting.greetingType = 'welcome';
  
  Properties:
  - name: string (default: 'World')
  - greetingType: 'hello' | 'hi' | 'hey' | 'welcome' (default: 'hello')
  
  Attributes:
  - name
  - greeting-type
  
  Events:
  - None (can be added as needed)
*/

export { GreetingElement };