/**
 * Dynamic Template Generation
 * @description Building templates programmatically based on runtime data and configuration
 * @module templates/dynamic-generation
 * @version 1.0.0
 * @example <dynamic-template-builder></dynamic-template-builder>
 */

// ============================================
// Utility Functions
// ============================================

/**
 * Safely escapes HTML special characters to prevent XSS attacks.
 * @param {string} value - The value to escape
 * @returns {string} The escaped string safe for HTML insertion
 */
function escapeHtml(value) {
  if (value == null) return '';
  const str = String(value);
  const escapeMap = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '"': '&quot;',
    "'": '&#39;'
  };
  return str.replace(/[&<>"']/g, char => escapeMap[char]);
}

/**
 * Deep clones an object, handling circular references
 * @param {*} obj - The object to clone
 * @param {WeakMap} [seen] - Internal tracking for circular refs
 * @returns {*} The cloned object
 */
function deepClone(obj, seen = new WeakMap()) {
  if (obj === null || typeof obj !== 'object') return obj;
  if (seen.has(obj)) return seen.get(obj);

  if (Array.isArray(obj)) {
    const arrCopy = [];
    seen.set(obj, arrCopy);
    return obj.map(item => deepClone(item, seen));
  }

  const objCopy = {};
  seen.set(obj, objCopy);

  for (const key in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, key)) {
      objCopy[key] = deepClone(obj[key], seen);
    }
  }

  return objCopy;
}

/**
 * Merges multiple objects deeply
 * @param {...object} objects - Objects to merge
 * @returns {object} The merged object
 */
function deepMerge(...objects) {
  const result = {};

  for (const obj of objects) {
    if (!obj || typeof obj !== 'object') continue;

    for (const key in obj) {
      if (!Object.prototype.hasOwnProperty.call(obj, key)) continue;

      const sourceValue = obj[key];
      const targetValue = result[key];

      if (
        sourceValue &&
        typeof sourceValue === 'object' &&
        !Array.isArray(sourceValue) &&
        targetValue &&
        typeof targetValue === 'object' &&
        !Array.isArray(targetValue)
      ) {
        result[key] = deepMerge(targetValue, sourceValue);
      } else {
        result[key] = deepClone(sourceValue);
      }
    }
  }

  return result;
}

/**
 * Validates that a value is a plain object
 * @param {*} value - The value to check
 * @returns {boolean} True if value is a plain object
 */
function isPlainObject(value) {
  if (!value || typeof value !== 'object') return false;
  const proto = Object.getPrototypeOf(value);
  return proto === Object.prototype || proto === null;
}

/**
 * Generates a unique ID for template elements
 * @param {string} [prefix='el'] - Optional prefix for the ID
 * @returns {string} Unique ID string
 */
function generateId(prefix = 'el') {
  return `${prefix}_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
}

// ============================================
// Template Expression Engine
// ============================================

/**
 * TemplateExpressionEngine
 * @description Handles Handlebars-like template expressions with variables, conditions, and loops
 * @version 1.0.0
 */
class TemplateExpressionEngine {
  /**
   * Creates a new TemplateExpressionEngine
   * @param {object} [options={}] - Configuration options
   * @param {boolean} [options.strictMode=false] - Throw errors on undefined variables
   * @param {boolean} [options.escapeHtml=true] - Auto-escape HTML in interpolations
   * @param {string} [options.delimiterStart='{{'] - Start delimiter for expressions
   * @param {string} [options.delimiterEnd='}}'] - End delimiter for expressions
   */
  constructor(options = {}) {
    this._options = {
      strictMode: false,
      escapeHtml: true,
      delimiterStart: '{{',
      delimiterEnd: '}}',
      ...options
    };
    this._helpers = new Map();
    this._registerBuiltInHelpers();
  }

  /**
   * Registers built-in helper functions
   * @private
   */
  _registerBuiltInHelpers() {
    this.registerHelper('uppercase', str => String(str).toUpperCase());
    this.registerHelper('lowercase', str => String(str).toLowerCase());
    this.registerHelper('capitalize', str => {
      const s = String(str);
      return s.charAt(0).toUpperCase() + s.slice(1).toLowerCase();
    });
    this.registerHelper('trim', str => String(str).trim());
    this.registerHelper('length', arr => {
      if (Array.isArray(arr) || typeof arr === 'string') return arr.length;
      return 0;
    });
    this.registerHelper('json', obj => JSON.stringify(obj));
    this.registerHelper('first', arr => Array.isArray(arr) ? arr[0] : undefined);
    this.registerHelper('last', arr => Array.isArray(arr) ? arr[arr.length - 1] : undefined);
    this.registerHelper('join', (arr, separator = ', ') => {
      if (!Array.isArray(arr)) return '';
      return arr.join(separator);
    });
    this.registerHelper('formatNumber', (num, decimals = 0) => {
      const n = Number(num);
      return isNaN(n) ? '0' : n.toFixed(decimals);
    });
  }

  /**
   * Registers a custom helper function
   * @param {string} name - Helper name
   * @param {Function} fn - Helper function
   */
  registerHelper(name, fn) {
    if (typeof fn !== 'function') {
      throw new TypeError(`Helper "${name}" must be a function`);
    }
    this._helpers.set(name, fn);
  }

  /**
   * Compiles a template string into an executable function
   * @param {string} template - The template string
   * @returns {Function} Compiled render function
   */
  compile(template) {
    const delimiterStart = this._options.delimiterStart;
    const delimiterEnd = this._options.delimiterEnd;
    const escapedDelimiters = delimiterStart.replace(/[-[\](){}*+?.\\^$|]/g, '\\$&');

    const regex = new RegExp(
      `${escapedDelimiters}([\\s\\S]*?)${delimiterEnd.replace(/[-[\](){}*+?.\\^$|]/g, '\\$&')}`,
      'g'
    );

    const parts = [];
    let lastIndex = 0;
    let match;

    while ((match = regex.exec(template)) !== null) {
      const expr = match[1].trim();

      if (match.index > lastIndex) {
        parts.push(`__output += ${JSON.stringify(template.slice(lastIndex, match.index))};`);
      }

      parts.push(this._compileExpression(expr));

      lastIndex = regex.lastIndex;
    }

    if (lastIndex < template.length) {
      parts.push(`__output += ${JSON.stringify(template.slice(lastIndex))};`);
    }

    const code = `
      var __output = '';
      var __data = __context || {};
      var __helpers = __this._helpers;
      var __escape = __this._options.escapeHtml ? __escapeHtml : function(v) { return v; };
      ${parts.join('\n')}
      return __output;
    `;

    try {
      return new Function('__context', '__this', code);
    } catch (err) {
      throw new Error(`Template compilation error: ${err.message}`);
    }
  }

  /**
   * Compiles a single expression to JavaScript code
   * @param {string} expr - The expression
   * @returns {string} JavaScript code
   * @private
   */
  _compileExpression(expr) {
    const firstNonSpace = expr.search(/\S/);
    if (firstNonSpace === -1) return `__output += '';`;

    const firstChar = expr.charAt(firstNonSpace);

    if (firstChar === '#') {
      return this._compileBlock(expr);
    }

    if (firstChar === '^') {
      return this._compileInvertedSection(expr);
    }

    if (firstChar === '/') {
      return '';
    }

    return `__output += __escape(${this._evaluateExpression(expr)});`;
  }

  /**
   * Compiles a block expression (#if, #each, #with)
   * @param {string} expr - Block expression
   * @returns {string} JavaScript code
   * @private
   */
  _compileBlock(expr) {
    const match = expr.match(/^#(\w+)\s+(.+)$/);
    if (!match) return '';

    const [, type, arg] = match;

    switch (type) {
      case 'each':
        return this._compileEachBlock(arg);
      case 'if':
        return this._compileIfBlock(arg);
      case 'with':
        return this._compileWithBlock(arg);
      default:
        return '';
    }
  }

  /**
   * Compiles an #each block
   * @param {string} arg - The iteration argument
   * @returns {string} JavaScript code
   * @private
   */
  _compileEachBlock(arg) {
    const [path, alias] = arg.split(/\s+as\s+/);
    const itemVar = alias || 'item';
    const indexVar = alias ? arg.split(/\s+as\s+/)[1].match(/\(\s*(\w+)\s*,\s*(\w+)\s*\)/) : null;

    if (indexVar) {
      return `
        (function() {
          var __arr = ${this._evaluatePath(path)};
          if (Array.isArray(__arr)) {
            for (var __i = 0; __i < __arr.length; __i++) {
              __data['${indexVar[1]}'] = __i;
              __data['${indexVar[2]}'] = __arr[__i];
              __data['${itemVar}'] = __arr[__i];
        `.replace(/\n        /g, '\n');
    }

    return `
      (function() {
        var __arr = ${this._evaluatePath(path)};
        if (Array.isArray(__arr)) {
          for (var __i = 0; __i < __arr.length; __i++) {
            __data['${itemVar}'] = __arr[__i];
            __data['index'] = __i;
    `;
  }

  /**
   * Compiles an #if block
   * @param {string} arg - The condition argument
   * @returns {string} JavaScript code
   * @private
   */
  _compileIfBlock(arg) {
    return `
      if (${this._evaluateExpression(arg)}) {
    `;
  }

  /**
   * Compiles a #with block
   * @param {string} arg - The context path
   * @returns {string} JavaScript code
   * @private
   */
  _compileWithBlock(arg) {
    return `
      (function() {
        var __prev = __data;
        __data = Object.assign({}, __prev, ${this._evaluatePath(arg)});
    `;
  }

  /**
   * Compiles an inverted section (^if NOT)
   * @param {string} expr - The expression
   * @returns {string} JavaScript code
   * @private
   */
  _compileInvertedSection(expr) {
    const match = expr.match(/^\^\s*(\w+)$/);
    if (!match) return '';

    const varName = match[1];
    return `
      if (!${this._evaluatePath(varName)}) {
    `;
  }

  /**
   * Evaluates a simple expression or path
   * @param {string} expr - The expression
   * @returns {string} JavaScript code
   * @private
   */
  _evaluateExpression(expr) {
    const pathMatch = expr.match(/^(.+?)\s*\|\s*(\w+)$/);

    if (pathMatch) {
      const [, path, helperName] = pathMatch;
      return `
        (function() {
          var __val = ${this._evaluatePath(path)};
          var __helper = __helpers.get('${helperName}');
          return __helper ? __helper(__val) : __val;
        })()
      `;
    }

    return this._evaluatePath(expr);
  }

  /**
   * Evaluates a property path
   * @param {string} path - The property path (e.g., 'user.name' or 'users[0].name')
   * @returns {string} JavaScript code
   * @private
   */
  _evaluatePath(path) {
    const tokens = path.trim().split(/\./);
    let code = '__data';

    for (const token of tokens) {
      if (token === 'this') {
        continue;
      }

      const arrayMatch = token.match(/^(\w+)\[(\d+)\]$/);
      if (arrayMatch) {
        code += `[${JSON.stringify(arrayMatch[1])}][${arrayMatch[2]}]`;
      } else {
        code += `[${JSON.stringify(token)}]`;
      }
    }

    if (this._options.strictMode) {
      return `((function() { 
        var v = ${code}; 
        if (v === undefined) throw new Error('Undefined variable: ${path}'); 
        return v; 
      })())`;
    }

    return code;
  }

  /**
   * Renders a template with the given data
   * @param {string} template - The template string
   * @param {object} [data={}] - The data context
   * @returns {string} Rendered template
   */
  render(template, data = {}) {
    const renderFn = this.compile(template);

    try {
      return renderFn.call(this, data, this);
    } catch (err) {
      if (this._options.strictMode) {
        throw err;
      }
      console.error('Template render error:', err.message);
      return template;
    }
  }
}

// ============================================
// Dynamic Template Builder
// ============================================

/**
 * DynamicTemplateBuilder
 * @description Web component for building templates programmatically based on runtime data
 * @version 1.0.0
 * @example <dynamic-template-builder template-id="my-template" data='{"name": "World"}'></dynamic-template-builder>
 */
class DynamicTemplateBuilder extends HTMLElement {
  /**
   * Creates a new DynamicTemplateBuilder
   */
  constructor() {
    super();
    this._template = null;
    this._data = {};
    this._compiledTemplate = null;
    this._engine = new TemplateExpressionEngine();
    this._cache = new Map();
    this._rendered = false;
    this._observers = [];
  }

  /**
   * Called when the element is connected to the DOM
   */
  connectedCallback() {
    this._rendered = false;
    this._initialize();
    this.render();
  }

  /**
   * Called when the element is disconnected from the DOM
   */
  disconnectedCallback() {
    this._disconnectObservers();
  }

  /**
   * Initializes the component
   * @private
   */
  _initialize() {
    this._setupShadowDOM();
    this._parseAttributes();
    this._setupObservers();
  }

  /**
   * Sets up Shadow DOM encapsulation
   * @private
   */
  _setupShadowDOM() {
    if (!this.shadowRoot) {
      const shadow = this.attachShadow({ mode: 'open' });
      shadow.appendChild(document.createElement('template'));
    }
  }

  /**
   * Parses element attributes
   * @private
   */
  _parseAttributes() {
    const templateAttr = this.getAttribute('template');
    const templateIdAttr = this.getAttribute('template-id');
    const dataAttr = this.getAttribute('data');

    if (templateAttr !== null) {
      this._template = templateAttr;
    } else if (templateIdAttr !== null) {
      const stored = this._getStoredTemplate(templateIdAttr);
      if (stored) {
        this._template = stored;
      }
    }

    if (dataAttr !== null) {
      try {
        this._data = JSON.parse(dataAttr);
      } catch (err) {
        console.warn('Invalid JSON in data attribute:', err.message);
        this._data = {};
      }
    }
  }

  /**
   * Gets a stored template by ID
   * @param {string} id - Template ID
   * @returns {string|null} The stored template
   * @private
   */
  _getStoredTemplate(id) {
    const templateEl = document.getElementById(id);
    if (templateEl && templateEl.tagName === 'TEMPLATE') {
      return templateEl.innerHTML;
    }

    return null;
  }

  /**
   * Sets up attribute observers
   * @private
   */
  _setupObservers() {
    const observer = new MutationObserver(mutations => {
      let shouldRender = false;

      for (const mutation of mutations) {
        if (mutation.type === 'attributes' || mutation.type === 'childList') {
          shouldRender = true;
          break;
        }
      }

      if (shouldRender) {
        this._parseAttributes();
        this.render();
      }
    });

    observer.observe(this, {
      attributes: true,
      childList: true,
      subtree: true
    });

    this._observers.push(observer);
  }

  /**
   * Disconnects all observers
   * @private
   */
  _disconnectObservers() {
    for (const observer of this._observers) {
      observer.disconnect();
    }
    this._observers = [];
  }

  /**
   * Gets the template ID for custom elements
   * @returns {string} The template tag name
   */
  static get templateTag() {
    return 'dynamic-template-builder';
  }

  /**
   * Gets observed attributes for the custom element
   * @returns {string[]} Array of observed attribute names
   */
  static get observedAttributes() {
    return ['template', 'template-id', 'data', 'cache', 'strict-mode'];
  }

  /**
   * Called when an attribute changes
   * @param {string} name - Attribute name
   * @param {string} oldValue - Old attribute value
   * @param {string} newValue - New attribute value
   */
  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'template':
        this._template = newValue;
        this._compiledTemplate = null;
        break;
      case 'template-id':
        this._template = this._getStoredTemplate(newValue);
        this._compiledTemplate = null;
        break;
      case 'data':
        try {
          this._data = JSON.parse(newValue);
        } catch (err) {
          console.warn('Invalid JSON in data attribute:', err.message);
        }
        break;
      case 'strict-mode':
        this._engine = new TemplateExpressionEngine({
          strictMode: newValue === 'true'
        });
        this._compiledTemplate = null;
        break;
    }

    if (!this._rendered) return;
    this.render();
  }

  /**
   * Sets the template
   * @param {string} template - The template string
   * @returns {DynamicTemplateBuilder} this for chaining
   */
  setTemplate(template) {
    if (typeof template !== 'string') {
      throw new TypeError('Template must be a string');
    }

    this._template = template;
    this._compiledTemplate = null;
    this._cache.clear();

    if (this._rendered) {
      this.render();
    }

    return this;
  }

  /**
   * Gets the current template
   * @returns {string|null} The current template
   */
  getTemplate() {
    return this._template;
  }

  /**
   * Sets the data context
   * @param {object} data - The data object
   * @returns {DynamicTemplateBuilder} this for chaining
   */
  setData(data) {
    if (!isPlainObject(data)) {
      throw new TypeError('Data must be a plain object');
    }

    this._data = deepClone(data);

    if (this._rendered) {
      this.render();
    }

    return this;
  }

  /**
   * Gets the current data
   * @returns {object} The current data
   */
  getData() {
    return deepClone(this._data);
  }

  /**
   * Updates data partially
   * @param {object} updates - Partial data to merge
   * @returns {DynamicTemplateBuilder} this for chaining
   */
  updateData(updates) {
    this._data = deepMerge(this._data, updates);

    if (this._rendered) {
      this.render();
    }

    return this;
  }

  /**
   * Registers a custom helper
   * @param {string} name - Helper name
   * @param {Function} fn - Helper function
   * @returns {DynamicTemplateBuilder} this for chaining
   */
  registerHelper(name, fn) {
    this._engine.registerHelper(name, fn);
    return this;
  }

  /**
   * Compiles the template
   * @private
   * @returns {Function} Compiled render function
   */
  _compile() {
    if (this._compiledTemplate) {
      return this._compiledTemplate;
    }

    if (!this._template) {
      return () => '';
    }

    const cacheKey = this._template;
    if (this._cache.has(cacheKey)) {
      this._compiledTemplate = this._cache.get(cacheKey);
      return this._compiledTemplate;
    }

    this._compiledTemplate = this._engine.compile(this._template);
    this._cache.set(cacheKey, this._compiledTemplate);

    return this._compiledTemplate;
  }

  /**
   * Renders the template to the DOM
   * @returns {DynamicTemplateBuilder} this for chaining
   */
  render() {
    if (!this.shadowRoot) {
      this._setupShadowDOM();
    }

    const template = this.shadowRoot.querySelector('template');
    const renderFn = this._compile();

    try {
      const output = renderFn(this._data, this._engine);
      template.innerHTML = output;
      this._rendered = true;

      this.dispatchEvent(new CustomEvent('template-rendered', {
        bubbles: true,
        composed: true,
        detail: {
          template: this._template,
          data: this._data
        }
      }));
    } catch (err) {
      console.error('Template render error:', err);

      this.dispatchEvent(new CustomEvent('template-error', {
        bubbles: true,
        composed: true,
        detail: {
          error: err,
          template: this._template,
          data: this._data
        }
      }));

      if (template) {
        template.innerHTML = `<div class="error">Template error: ${escapeHtml(err.message)}</div>`;
      }
    }

    return this;
  }

  /**
   * Clears the rendered template
   * @returns {DynamicTemplateBuilder} this for chaining
   */
  clear() {
    if (this.shadowRoot) {
      const template = this.shadowRoot.querySelector('template');
      if (template) {
        template.innerHTML = '';
      }
    }
    this._rendered = false;
    return this;
  }

  /**
   * Gets the rendered output as a string
   * @returns {string} Rendered HTML
   */
  getRenderedContent() {
    const template = this.shadowRoot.querySelector('template');
    return template ? template.innerHTML : '';
  }

  /**
   * Gets the rendered DOM elements
   * @returns {HTMLCollection} Rendered elements
   */
  getRenderedElements() {
    const template = this.shadowRoot.querySelector('template');
    return template ? template.children : [];
  }
}

// ============================================
// Template Registry (for managing reusable templates)
// ============================================

/**
 * TemplateRegistry
 * @description Registry for managing and reusing template definitions
 * @version 1.0.0
 */
class TemplateRegistry {
  /**
   * Creates a new TemplateRegistry
   */
  constructor() {
    this._templates = new Map();
    this._engine = new TemplateExpressionEngine();
  }

  /**
   * Registers a template
   * @param {string} name - Template name
   * @param {string} template - Template string
   * @param {object} [options={}] - Template options
   * @returns {TemplateRegistry} this for chaining
   */
  register(name, template, options = {}) {
    if (typeof name !== 'string' || !name) {
      throw new TypeError('Template name must be a non-empty string');
    }

    if (typeof template !== 'string') {
      throw new TypeError('Template must be a string');
    }

    this._templates.set(name, {
      template,
      options: {
        cache: true,
        strictMode: false,
        ...options
      }
    });

    return this;
  }

  /**
   * Gets a registered template
   * @param {string} name - Template name
   * @returns {object|null} Template definition
   */
  get(name) {
    return this._templates.get(name) || null;
  }

  /**
   * Checks if a template is registered
   * @param {string} name - Template name
   * @returns {boolean} True if template exists
   */
  has(name) {
    return this._templates.has(name);
  }

  /**
   * Removes a template
   * @param {string} name - Template name
   * @returns {TemplateRegistry} this for chaining
   */
  remove(name) {
    this._templates.delete(name);
    return this;
  }

  /**
   * Gets all template names
   * @returns {string[]} Array of template names
   */
  getNames() {
    return Array.from(this._templates.keys());
  }

  /**
   * Renders a template
   * @param {string} name - Template name
   * @param {object} [data={}] - Data context
   * @returns {string} Rendered template
   */
  render(name, data = {}) {
    const entry = this.get(name);

    if (!entry) {
      throw new Error(`Template "${name}" not found`);
    }

    return this._engine.render(entry.template, data);
  }

  /**
   * Clears all templates
   * @returns {TemplateRegistry} this for chaining
   */
  clear() {
    this._templates.clear();
    return this;
  }
}

// ============================================
// Template List Renderer
// ============================================

/**
 * TemplateListRenderer
 * @description Renders lists of items with templates
 * @version 1.0.0
 */
class TemplateListRenderer {
  /**
   * Creates a new TemplateListRenderer
   * @param {object} [options={}] - Configuration options
   */
  constructor(options = {}) {
    this._options = {
      itemTag: 'li',
      containerTag: 'ul',
      emptyMessage: 'No items to display',
      itemClass: 'list-item',
      containerClass: 'list-container',
      ...options
    };
    this._engine = new TemplateExpressionEngine();
  }

  /**
   * Renders a list from array data
   * @param {string} template - Item template
   * @param {Array} items - Array of items
   * @param {object} [options={}] - Render options
   * @returns {string} Rendered HTML
   */
  render(template, items, options = {}) {
    if (!Array.isArray(items)) {
      throw new TypeError('Items must be an array');
    }

    if (items.length === 0) {
      return this._renderEmpty();
    }

    const mergedOptions = { ...this._options, ...options };
    const renderedItems = items.map((item, index) => this._renderItem(template, item, index, mergedOptions));

    return this._renderContainer(renderedItems, mergedOptions);
  }

  /**
   * Renders a single item
   * @private
   * @param {string} template - Item template
   * @param {object} item - Item data
   * @param {number} index - Item index
   * @param {object} options - Render options
   * @returns {string} Rendered item HTML
   */
  _renderItem(template, item, index, options) {
    const data = { ...item, index };
    const content = this._engine.render(template, data);
    return `<${options.itemTag} class="${options.itemClass}">${content}</${options.itemTag}>`;
  }

  /**
   * Renders the container
   * @private
   * @param {string[]} items - Rendered items
   * @param {object} options - Render options
   * @returns {string} Container HTML
   */
  _renderContainer(items, options) {
    const classAttr = options.containerClass ? ` class="${options.containerClass}"` : '';
    return `<${options.containerTag}${classAttr}>${items.join('')}</${options.containerTag}>`;
  }

  /**
   * Renders empty message
   * @private
   * @returns {string} Empty message HTML
   */
  _renderEmpty() {
    return `<div class="empty-message">${escapeHtml(this._options.emptyMessage)}</div>`;
  }
}

// ============================================
// Export
// ============================================

export {
  DynamicTemplateBuilder,
  TemplateExpressionEngine,
  TemplateRegistry,
  TemplateListRenderer,
  escapeHtml,
  deepClone,
  deepMerge,
  isPlainObject,
  generateId
};