/**
 * HTML Template Tag Deep Dive - Template-based Rendering
 * @description Deep dive into HTML template elements, their parsing behavior, and efficient rendering patterns
 * @module templates/template-tag
 * @version 1.0.0
 * @example <template-renderer></template-renderer>
 */

// ============================================
// Template Renderer Component
// ============================================

/**
 * TemplateRenderer - Efficient template rendering with caching
 * 
 * Features:
 * - Template element caching
 * - Clone optimization
 * - Data binding support
 * - Conditional rendering
 * - List rendering
 * 
 * Props:
 * - templateId: string - ID of template element to render
 * - data: object - Data to bind to template
 * - cache: boolean - Enable template caching (default: true)
 * 
 * Events:
 * - render: Fired after rendering completes
 * - error: Fired on render error
 */
class TemplateRenderer extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._templateCache = new Map();
    this._boundData = null;
    this._templateId = null;
    this._cacheEnabled = true;
  }

  static get observedAttributes() {
    return ['template-id', 'cache'];
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._templateCache = new Map();
    this._boundData = null;
  }

  static get observedAttributes() {
    return ['template-id', 'cache', 'data'];
  }

  connectedCallback() {
    this._cacheEnabled = this.hasAttribute('cache') ? this.getAttribute('cache') !== 'false' : true;
    this._templateId = this.getAttribute('template-id');
    if (this.hasAttribute('data')) {
      try {
        this._boundData = JSON.parse(this.getAttribute('data'));
      } catch (e) {
        console.error('[TemplateRenderer] Failed to parse data attribute:', e);
      }
    }
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    
    if (name === 'template-id') {
      this._templateId = newValue;
    } else if (name === 'cache') {
      this._cacheEnabled = newValue !== 'false';
    } else if (name === 'data') {
      try {
        this._boundData = JSON.parse(newValue);
      } catch (e) {
        console.error('[TemplateRenderer] Failed to parse data:', e);
      }
    }
    this.render();
  }

  get data() {
    return this._boundData;
  }

  set data(value) {
    this._boundData = value;
    this.setAttribute('data', JSON.stringify(value));
    this.render();
  }

  /**
   * Get template from cache or DOM
   * @private
   */
  _getTemplate() {
    if (!this._templateId) return null;
    
    if (this._cacheEnabled && this._templateCache.has(this._templateId)) {
      return this._templateCache.get(this._templateId);
    }
    
    const template = document.getElementById(this._templateId);
    if (!template || template.tagName !== 'TEMPLATE') {
      console.error('[TemplateRenderer] Template not found:', this._templateId);
      return null;
    }
    
    if (this._cacheEnabled) {
      this._templateCache.set(this._templateId, template);
    }
    
    return template;
  }

  /**
   * Bind data to template variables
   * @private
   */
  _bindData(template, data) {
    if (!data) return template;
    
    const clone = template.content.cloneNode(true);
    const walker = document.createTreeWalker(clone, NodeFilter.SHOW_TEXT | NodeFilter.SHOW_ELEMENT);
    
    const regex = /\{\{(\w+)(?:\.(\w+))?\}\}/g;
    const nodesToReplace = [];
    
    let node;
    while (node = walker.nextNode()) {
      if (node.nodeType === Node.TEXT_NODE) {
        if (regex.test(node.textContent)) {
          nodesToReplace.push({ node, text: node.textContent });
        }
      }
    }
    
    nodesToReplace.forEach(({ node, text }) => {
      const newText = text.replace(regex, (match, key, subkey) => {
        if (subkey && data[key]) {
          return data[key][subkey] ?? match;
        }
        return data[key] ?? match;
      });
      node.textContent = newText;
    });
    
    return clone;
  }

  render() {
    const template = this._getTemplate();
    if (!template) {
      this.shadowRoot.innerHTML = '<slot></slot>';
      return;
    }
    
    const content = this._bindData(template, this._boundData);
    this.shadowRoot.innerHTML = '';
    this.shadowRoot.appendChild(content);
    
    this.dispatchEvent(new CustomEvent('render', { bubbles: true, composed: true }));
  }

  /**
   * Render a list of items
   * @param {string} templateId - Template ID
   * @param {Array} items - Array of data objects
   * @param {string} keyField - Field to use as key
   */
  renderList(templateId, items, keyField = 'id') {
    const template = document.getElementById(templateId);
    if (!template) return;
    
    const container = document.createDocumentFragment();
    
    items.forEach((item, index) => {
      const clone = template.content.cloneNode(true);
      this._bindDataInClone(clone, item);
      
      if (keyField && item[keyField]) {
        clone.querySelectorAll('*').forEach(el => {
          el.dataset[keyField] = item[keyField];
        });
      }
      
      container.appendChild(clone);
    });
    
    this.shadowRoot.innerHTML = '';
    this.shadowRoot.appendChild(container);
  }

  /**
   * Bind data in cloned template
   * @private
   */
  _bindDataInClone(clone, data) {
    const walker = document.createTreeWalker(clone, NodeFilter.SHOW_ELEMENT);
    let node;
    
    while (node = walker.nextNode()) {
      Object.keys(data).forEach(key => {
        if (node.hasAttribute(`:${key}`)) {
          node.textContent = data[key];
        }
        if (node.hasAttribute(`@${key}`)) {
          node.setAttribute(node.getAttribute(`@${key}`), data[key]);
        }
      });
    }
    
    return clone;
  }

  /**
   * Clear template cache
   */
  clearCache() {
    this._templateCache.clear();
  }

  disconnectedCallback() {
    if (!this._cacheEnabled) {
      this.clearCache();
    }
  }
}

// ============================================
// Template Registry
// ============================================

/**
 * TemplateRegistry - Manages template caching and retrieval
 */
class TemplateRegistry {
  constructor() {
    this._templates = new Map();
    this._maxSize = 50;
  }

  /**
   * Register a template
   * @param {string} id - Template ID
   * @param {HTMLTemplateElement} template - Template element
   */
  register(id, template) {
    if (this._templates.size >= this._maxSize) {
      const firstKey = this._templates.keys().next().value;
      this._templates.delete(firstKey);
    }
    this._templates.set(id, template);
  }

  /**
   * Get a template by ID
   * @param {string} id - Template ID
   * @returns {HTMLTemplateElement|null}
   */
  get(id) {
    return this._templates.get(id) || null;
  }

  /**
   * Check if template exists
   * @param {string} id - Template ID
   * @returns {boolean}
   */
  has(id) {
    return this._templates.has(id);
  }

  /**
   * Clear all templates
   */
  clear() {
    this._templates.clear();
  }

  /**
   * Get all template IDs
   * @returns {string[]}
   */
  keys() {
    return Array.from(this._templates.keys());
  }

  /**
   * Get registry size
   * @returns {number}
   */
  get size() {
    return this._templates.size;
  }
}

// Global registry instance
const globalTemplateRegistry = new TemplateRegistry();

// ============================================
// Template Utilities
// ============================================

/**
 * Template utility functions
 */
const TemplateUtils = {
  /**
   * Create template from string
   * @param {string} html - HTML string
   * @returns {HTMLTemplateElement}
   */
  createTemplate(html) {
    const template = document.createElement('template');
    template.innerHTML = html;
    return template;
  },

  /**
   * Clone template with data
   * @param {HTMLTemplateElement} template - Template to clone
   * @param {object} data - Data to bind
   * @returns {DocumentFragment}
   */
  cloneWithData(template, data) {
    const clone = template.content.cloneNode(true);
    if (!data) return clone;
    
    const walker = document.createTreeWalker(clone, NodeFilter.SHOW_TEXT);
    let node;
    const regex = /\{\{(\w+)\}\}/g;
    
    while (node = walker.nextNode()) {
      node.textContent = node.textContent.replace(regex, (match, key) => {
        return data[key] ?? match;
      });
    }
    
    return clone;
  },

  /**
   * Pre-compile templates for performance
   * @param {string[]} ids - Array of template IDs
   */
  precompile(ids) {
    const templates = [];
    ids.forEach(id => {
      const template = document.getElementById(id);
      if (template) {
        templates.push({ id, content: template.innerHTML });
      }
    });
    return templates;
  },

  /**
   * Get template content as string
   * @param {HTMLTemplateElement} template - Template element
   * @returns {string}
   */
  getContent(template) {
    return template ? template.innerHTML : '';
  }
};

// ============================================
// Export
// ============================================

export { TemplateRenderer, TemplateRegistry, TemplateUtils };
export { globalTemplateRegistry };