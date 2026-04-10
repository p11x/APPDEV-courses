/**
 * DOM Manipulation Mastery - Advanced DOM Operations
 * @description Advanced DOM manipulation patterns for Web Components
 * @module basics/dom-manipulation
 * @version 1.0.0
 */

// ============================================
// DocumentFragment Operations
// ============================================

/**
 * EfficientListComponent - Demonstrates DocumentFragment usage
 */
export class EfficientListComponent extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._items = [];
  }

  set items(value) {
    this._items = Array.isArray(value) ? value : [];
    this.render();
  }

  get items() {
    return this._items;
  }

  connectedCallback() {
    this.render();
  }

  /**
   * Efficient rendering using DocumentFragment
   * Batch DOM operations to minimize reflows
   */
  render() {
    const fragment = document.createDocumentFragment();
    
    // Create style element
    const style = document.createElement('style');
    style.textContent = this.styles;
    fragment.appendChild(style);
    
    // Create list container
    const list = document.createElement('ul');
    list.className = 'list';
    
    // Batch append children
    for (const item of this._items) {
      const li = document.createElement('li');
      li.className = 'item';
      li.dataset.id = item.id;
      li.textContent = item.label;
      fragment.appendChild(li);
    }
    
    list.appendChild(fragment);
    
    // Single DOM operation
    this.shadowRoot.innerHTML = '';
    this.shadowRoot.appendChild(list);
  }

  get styles() {
    return `
      ul { list-style: none; padding: 0; margin: 0; }
      li { 
        padding: 12px 16px; 
        border-bottom: 1px solid #eee; 
        transition: background 0.2s;
      }
      li:hover { background: #f5f5f5; }
    `;
  }

  /**
   * Update specific item without full re-render
   */
  updateItem(id, newData) {
    const item = this.shadowRoot.querySelector(`[data-id="${id}"]`);
    if (item) {
      item.textContent = newData.label;
      Object.assign(item.dataset, newData);
    }
  }

  /**
   * Add item efficiently
   */
  addItem(item) {
    this._items.push(item);
    
    const list = this.shadowRoot.querySelector('ul');
    const li = document.createElement('li');
    li.className = 'item';
    li.dataset.id = item.id;
    li.textContent = item.label;
    
    // Insert without full re-render
    list.appendChild(li);
  }

  /**
   * Remove item efficiently
   */
  removeItem(id) {
    const item = this.shadowRoot.querySelector(`[data-id="${id}"]`);
    if (item) {
      item.remove();
    }
    this._items = this._items.filter(i => i.id !== id);
  }
}

customElements.define('efficient-list', EfficientListComponent);

// ============================================
// Query Methods
// ============================================

/**
 * QueryHelper - Utility for querying within component
 */
export class QueryHelper {
  constructor(shadowRoot) {
    this._root = shadowRoot;
  }

  $(selector) {
    return this._root.querySelector(selector);
  }

  $$(selector) {
    return Array.from(this._root.querySelectorAll(selector));
  }

  $id(id) {
    return this._root.getElementById(id);
  }

  $closest(element, selector) {
    return element.closest(selector);
  }

  $matches(element, selector) {
    return element.matches(selector);
  }
}

// ============================================
// Mutation Observation
// ============================================

/**
 * ObservableComponent - Component with mutation observation
 */
export class ObservableComponent extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._observer = null;
  }

  connectedCallback() {
    this.setupObserver();
    this.render();
  }

  disconnectedCallback() {
    if (this._observer) {
      this._observer.disconnect();
    }
  }

  setupObserver() {
    this._observer = new MutationObserver((mutations) => {
      mutations.forEach(mutation => {
        if (mutation.type === 'childList') {
          console.log('[Observable] Children changed:', {
            added: mutation.addedNodes.length,
            removed: mutation.removedNodes.length
          });
        } else if (mutation.type === 'attributes') {
          console.log('[Observable] Attribute changed:', mutation.attributeName);
        }
      });
    });

    this._observer.observe(this, {
      childList: true,
      subtree: true,
      attributes: true,
      attributeOldValue: true
    });
  }

  render() {
    this.shadowRoot.innerHTML = `
      <div class="content"><slot></slot></div>
      <style>
        .content { padding: 16px; }
      </style>
    `;
  }
}

customElements.define('observable-component', ObservableComponent);