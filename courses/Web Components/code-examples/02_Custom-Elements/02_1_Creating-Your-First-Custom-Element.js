/**
 * Creating Your First Custom Element - Simple Greeting Component
 * @description Step-by-step guide to creating your first custom element
 * @module custom-elements/first-element
 * @version 1.0.0
 */

import { GreetingElement } from '../01_Basics/01_1_Introduction-to-Web-Components.js';

/**
 * Step 1: Understanding the Basic Structure
 * 
 * All custom elements must extend HTMLElement
 * 
 * class MyFirstElement extends HTMLElement {
 *   constructor() {
 *     super();  // Required!
 *   }
 * }
 */

/**
 * Step 2: Complete Implementation
 */
class MyFirstElement extends HTMLElement {
  /**
   * Step 3: Constructor - Initialize component
   */
  constructor() {
    // Always call super() first!
    super();
    
    // Create shadow DOM for encapsulation
    this.attachShadow({ mode: 'open' });
    
    console.log('[MyFirstElement] Constructor called');
  }

  /**
   * Step 4: connectedCallback - Called when element is added to DOM
   */
  connectedCallback() {
    console.log('[MyFirstElement] Added to DOM');
    this.render();
  }

  /**
   * Step 5: render() - Render the component template
   */
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 20px;
          font-family: sans-serif;
        }
        
        .container {
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          color: white;
          padding: 30px;
          border-radius: 12px;
          text-align: center;
        }
        
        h1 {
          margin: 0 0 10px;
          font-size: 28px;
        }
        
        p {
          margin: 0;
          opacity: 0.9;
        }
      </style>
      
      <div class="container">
        <h1>🎉 Hello World!</h1>
        <p>This is my first Web Component</p>
      </div>
    `;
  }
}

/**
 * Step 6: Register the custom element
 * IMPORTANT: Tag name must contain a hyphen!
 */
customElements.define('my-first-element', MyFirstElement);

/**
 * Step 7: Extending Built-in Elements
 */
class MyFirstButton extends HTMLButtonElement {
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
        button {
          padding: 12px 24px;
          background: #667eea;
          color: white;
          border: none;
          border-radius: 6px;
          cursor: pointer;
          font-size: 16px;
        }
        button:hover {
          background: #5a6fd6;
        }
      </style>
      <button><slot>Click Me</slot></button>
    `;
  }
}

// Register as customized built-in element
customElements.define('my-first-button', MyFirstButton, { extends: 'button' });

/**
 * Step 8: Usage Examples
 * 
 * HTML:
 * <my-first-element></my-first-element>
 * <button is="my-first-button">Custom Button</button>
 * 
 * JavaScript:
 * const el = document.createElement('my-first-element');
 */

export { MyFirstElement, MyFirstButton };