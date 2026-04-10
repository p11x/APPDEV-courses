/**
 * Web Components API Reference - Complete API documentation with examples
 * @module reference/14_1_API-Reference
 * @version 1.0.0
 */

class APIReference extends HTMLElement {
  constructor() {
    super();
    this._shadow = this.attachShadow({ mode: 'open' });
    this._lifecycleHooks = new Map();
    this._observedAttributes = [];
  }

  static get observedAttributes() {
    return ['title', 'version', 'theme', 'compact'];
  }

  static get observedProperties() {
    return ['data', 'items', 'filter', 'sortBy'];
  }

  get data() {
    return this._data || [];
  }

  set data(value) {
    this._data = value;
    this._render();
  }

  get title() {
    return this.getAttribute('title') || 'API Reference';
  }

  set title(value) {
    this.setAttribute('title', value);
  }

  get version() {
    return this.getAttribute('version') || '1.0.0';
  }

  get theme() {
    return this.getAttribute('theme') || 'light';
  }

  get compact() {
    return this.hasAttribute('compact');
  }

  connectedCallback() {
    this._lifecycleHooks.set('connected', Date.now());
    this._render();
  }

  disconnectedCallback() {
    this._lifecycleHooks.set('disconnected', Date.now());
  }

  adoptedCallback() {
    this._lifecycleHooks.set('adopted', Date.now());
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._lifecycleHooks.set(`attr_${name}`, { old: oldValue, new: newValue });
      this._render();
    }
  }

  connected() {
    return this._lifecycleHooks.get('connected');
  }

  getLifecycleHistory() {
    return Array.from(this._lifecycleHooks.entries());
  }

  getCustomElements() {
    return {
      define: (tagName, classDef, options) => customElements.define(tagName, classDef, options),
      get: (tagName) => customElements.get(tagName),
      whenDefined: (tagName) => customElements.whenDefined(tagName),
      upgrade: (root) => customElements.upgrade(root)
    };
  }

  getShadowRoot() {
    return this._shadow;
  }

  dispatchEventCustom(eventName, detail = {}) {
    const event = new CustomEvent(eventName, {
      bubbles: true,
      composed: true,
      detail
    });
    return this.dispatchEvent(event);
  }

  addEventListenerSecure(type, handler, options) {
    const wrappedHandler = (e) => {
      try {
        handler.call(this, e);
      } catch (err) {
        console.error('Event handler error:', err);
      }
    };
    this._shadow.addEventListener(type, wrappedHandler, options);
    return () => this._shadow.removeEventListener(type, wrappedHandler, options);
  }

  querySelectorAll(selector) {
    return this._shadow.querySelectorAll(selector);
  }

  querySelector(selector) {
    return this._shadow.querySelector(selector);
  }

  createTemplate(content) {
    const template = document.createElement('template');
    template.content.appendChild(document.createRange().createContextualFragment(content));
    return template;
  }

  cloneTemplate(template) {
    return template.content.cloneNode(true);
  }

  observeAttributes(attributes) {
    this._observedAttributes = attributes;
  }

  getAttributeList() {
    return {
      properties: this._observedAttributes,
      all: this.attributes ? Array.from(this.attributes).map(a => a.name) : []
    };
  }

  serializeToJSON() {
    return JSON.stringify({
      tagName: this.tagName,
      version: this.version,
      theme: this.theme,
      compact: this.compact,
      lifecycle: this.getLifecycleHistory()
    }, null, 2);
  }

  static fromJSON(json) {
    const data = JSON.parse(json);
    const el = document.createElement(data.tagName.toLowerCase());
    Object.keys(data).forEach(key => {
      if (key !== 'tagName') {
        el.setAttribute(key, data[key]);
      }
    });
    return el;
  }

  applyTheme(themeName) {
    const styles = this._shadow.querySelector('style');
    if (styles) {
      styles.textContent = this._getThemeCSS(themeName);
    }
  }

  _getThemeCSS(theme) {
    const themes = {
      light: `
        :host { --bg: #ffffff; --text: #333333; --border: #cccccc; }
      `,
      dark: `
        :host { --bg: #1a1a1a; --text: #ffffff; --border: #444444; }
      `,
      highContrast: `
        :host { --bg: #000000; --text: #ffff00; --border: #ffffff; }
      `
    };
    return themes[theme] || themes.light;
  }

  _render() {
    this._shadow.innerHTML = `
      <style>
        ${this._getThemeCSS(this.theme)}
        :host {
          display: block;
          font-family: system-ui, sans-serif;
          background: var(--bg);
          color: var(--text);
          border: 1px solid var(--border);
          padding: 16px;
        }
        .header { font-size: 1.5em; font-weight: bold; margin-bottom: 16px; }
        .content { line-height: 1.6; }
        .compact .content { margin: 0; padding: 8px; }
      </style>
      <div class="${this.compact ? 'compact' : ''}">
        <div class="header">${this.title} <small>v${this.version}</small></div>
        <div class="content">
          <slot></slot>
        </div>
      </div>
    `;
  }
}

customElements.define('api-reference', APIReference);

export { APIReference };
