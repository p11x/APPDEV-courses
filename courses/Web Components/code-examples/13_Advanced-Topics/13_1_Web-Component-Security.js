/**
 * Web Component Security - Security Implementation for Web Components
 * @description Security best practices including XSS prevention, CSP, attribute validation, and safe data handling
 * @module advanced/security
 * @version 1.0.0
 */

(function() {
  'use strict';

  const SecurityConfig = {
    allowedTags: new Set(['div', 'span', 'p', 'br', 'strong', 'em', 'ul', 'ol', 'li', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'a', 'button', 'input', 'label', 'select', 'option', 'textarea', 'table', 'thead', 'tbody', 'tr', 'th', 'td', 'img', 'svg', 'path', 'canvas', 'video', 'audio', 'source', 'iframe']),
    allowedAttrs: new Set(['id', 'class', 'style', 'title', 'alt', 'href', 'src', 'type', 'name', 'value', 'placeholder', 'disabled', 'readonly', 'required', 'checked', 'selected', 'min', 'max', 'step', 'pattern', 'width', 'height', 'target', 'rel', 'tabindex', 'aria-label', 'aria-describedby', 'role', 'data-*']),
    allowedSchemes: new Set(['https:', 'mailto:', 'tel:']),
    maxInputLength: 10000,
    sanitizationCache: new Map()
  };

  function isValidTagName(tagName) {
    const customElementPattern = /^[a-z]+-[a-z0-9-]*$/;
    return customElementPattern.test(tagName);
  }

  function isValidAttributeName(attrName) {
    const globalAttrPattern = /^(?!on|aria|data|aria-|xmlns|xlink)[a-z-]+$/;
    return globalAttrPattern.test(attrName) || attrName.startsWith('data-') && attrName.length > 5;
  }

  function escapeHtml(text) {
    if (typeof text !== 'string') return '';
    const map = {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#x27;',
      '/': '&#x2F;'
    };
    return text.replace(/[&<>"'/]/g, char => map[char]);
  }

  function sanitizeHTML(input, options = {}) {
    if (typeof input !== 'string') return '';
    if (input.length > SecurityConfig.maxInputLength) {
      input = input.substring(0, SecurityConfig.maxInputLength);
    }

    const parser = new DOMParser();
    const doc = parser.parseFromString(`<div>${input}</div>`, 'text/html');
    const element = doc.querySelector('div');

    if (!element) return '';

    const walker = document.createTreeWalker(element, NodeFilter.SHOW_ALL, null, false);
    const nodesToRemove = [];

    while (walker.nextNode()) {
      const node = walker.currentNode;
      if (node.nodeType === Node.ELEMENT_NODE) {
        const tagName = node.tagName.toLowerCase();
        if (!SecurityConfig.allowedTags.has(tagName)) {
          nodesToRemove.push(node);
          continue;
        }

        const attrs = Array.from(node.attributes);
        for (const attr of attrs) {
          const attrName = attr.name.toLowerCase();
          if (attrName.startsWith('on') || attrName === 'style' &&!/^[\w-]+\s*:[\w-]+(\s*;|$)/i.test(attr.value)) {
            node.removeAttribute(attr.name);
            continue;
          }

          if (attrName === 'href' || attrName === 'src') {
            try {
              const url = new URL(attr.value, options.baseUrl || window.location.href);
              if (!SecurityConfig.allowedSchemes.has(url.protocol)) {
                node.removeAttribute(attr.name);
              }
            } catch (e) {
              node.removeAttribute(attr.name);
            }
          }

          if (attrName === 'style') {
            node.style.cssText = '';
            node.removeAttribute('style');
          }
        }
      }

      if (node.nodeType === Node.TEXT_NODE) {
        const parent = node.parentNode;
        if (parent && (parent.tagName === 'SCRIPT' || parent.tagName === 'STYLE')) {
          continue;
        }
      }
    }

    nodesToRemove.forEach(node => node.remove());
    return element.innerHTML;
  }

  function createCSPPolicy() {
    return [
      "default-src 'self'",
      "script-src 'self' 'nonce-{nonce}' 'strict-dynamic'",
      "style-src 'self' 'unsafe-inline'",
      "img-src 'self' data: https:",
      "font-src 'self'",
      "connect-src 'self' https://api.example.com",
      "frame-ancestors 'none'",
      "base-uri 'self'",
      "form-action 'self'"
    ].join('; ');
  }

  function validateInput(value, type) {
    const validators = {
      email: /^[\w.-]+@[\w.-]+\.\w+$/,
      url: /^https?:\/\/.+/,
      number: /^-?\d+(\.\d+)?$/,
      integer: /^-?\d+$/,
      alphanumeric: /^[a-zA-Z0-9]+$/,
      alpha: /^[a-zA-Z]+$/,
      hex: /^[0-9a-fA-F]+$/
    };

    if (!validators[type]) return true;
    return validators[type].test(String(value).trim());
  }

  function sanitizeAttributeValue(value, attributeName) {
    if (typeof value !== 'string') return '';
    let sanitized = value.trim();

    if (attributeName === 'href' || attributeName === 'src') {
      try {
        const url = new URL(sanitized);
        if (!SecurityConfig.allowedSchemes.has(url.protocol)) {
          return '';
        }
      } catch (e) {
        return '';
      }
    }

    sanitized = escapeHtml(sanitized);
    return sanitized;
  }

  function createSecureShadowRoot(host, options = {}) {
    const shadowRoot = host.attachShadow({ mode: options.mode || 'open' });

    const nonce = options.nonce || btoa(Math.random()).substring(0, 16);
    const meta = document.createElement('meta');
    meta.httpEquiv = 'Content-Security-Policy';
    meta.content = createCSPPolicy().replace('{nonce}', nonce);

    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
      }
      [data-sanitized] {
        contain: content;
      }
    `;
    style.setAttribute('nonce', nonce);

    shadowRoot.appendChild(meta);
    shadowRoot.appendChild(style);

    return { shadowRoot, nonce };
  }

  function validateAndSanitizeProperties(properties, schema) {
    const errors = [];
    const sanitized = {};

    for (const [key, def] of Object.entries(schema)) {
      const value = properties[key];

      if (def.required && (value === undefined || value === null)) {
        errors.push({ field: key, message: `${key} is required` });
        continue;
      }

      if (value !== undefined && value !== null) {
        if (def.type && !validateInput(value, def.type)) {
          errors.push({ field: key, message: `${key} has invalid format` });
          continue;
        }

        if (def.maxLength && String(value).length > def.maxLength) {
          errors.push({ field: key, message: `${key} exceeds max length` });
          continue;
        }

        if (def.enum && !def.enum.includes(value)) {
          errors.push({ field: key, message: `${key} must be one of ${def.enum.join(', ')}` });
          continue;
        }

        if (def.sanitize) {
          sanitized[key] = escapeHtml(String(value));
        } else {
          sanitized[key] = value;
        }
      }
    }

    return { sanitized, errors };
  }

  class SecureComponent extends HTMLElement {
    static get observedAttributes() {
      return ['data-src', 'data-content'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._securityConfig = {
        nonce: btoa(Math.random()).substring(0, 16),
        sanitizeContent: true
      };
    }

    connectedCallback() {
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue && this.isConnected) {
        this.render();
      }
    }

    sanitize(content) {
      return sanitizeHTML(content, { baseUrl: this.baseURI });
    }

    render() {
      const content = this.getAttribute('data-content') || '';
      const sanitizedContent = this._securityConfig.sanitizeContent ? this.sanitize(content) : escapeHtml(content);

      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
            contain: content;
          }
          .container {
            padding: 16px;
            font-family: system-ui, -apple-system, sans-serif;
          }
        </style>
        <div class="container" data-sanitized>
          ${sanitizedContent}
        </div>
      `;
    }
  }

  class SecureInputComponent extends HTMLElement {
    static get observedAttributes() {
      return ['type', 'value', 'placeholder', 'disabled'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._value = '';
      this._type = 'text';
    }

    static get observedAttributes() {
      return ['type', 'placeholder', 'disabled'];
    }

    get value() {
      return this._value;
    }

    set value(val) {
      this._value = val;
      this.renderInput();
    }

    connectedCallback() {
      this._type = this.getAttribute('type') || 'text';
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'type') {
          this._type = newValue || 'text';
        }
        this.render();
      }
    }

    handleInput(event) {
      let value = event.target.value;

      if (this._type === 'number') {
        value = value.replace(/[^0-9.-]/g, '');
      } else if (this._type === 'email') {
        value = value.replace(/[^a-zA-Z0-9@._-]/g, '');
      }

      this._value = value;
      this.dispatchEvent(new CustomEvent('secure-change', {
        detail: { value: this._value },
        bubbles: true,
        composed: true
      }));
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          input {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 14px;
            box-sizing: border-box;
          }
          input:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 0 0 2px rgba(102, 126, 234, 0.2);
          }
          input:disabled {
            background: #f5f5f5;
            cursor: not-allowed;
          }
        </style>
        <input 
          type="${this._type}" 
          placeholder="${escapeHtml(this.getAttribute('placeholder') || '')}"
          ${this.hasAttribute('disabled') ? 'disabled' : ''}
        />
      `;

      const input = this.shadowRoot.querySelector('input');
      input.value = this._value;
      input.addEventListener('input', this.handleInput.bind(this));
    }

    renderInput() {
      const input = this.shadowRoot.querySelector('input');
      if (input) {
        input.value = this._value;
      }
    }
  }

  class AuthenticatedComponent extends HTMLElement {
    static get observedAttributes() {
      return [' token', 'api-endpoint'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._token = null;
      this._apiEndpoint = '';
      this._authorized = false;
    }

    static get observedAttributes() {
      return ['token', 'api-endpoint'];
    }

    connectedCallback() {
      this._token = this.getAttribute('token');
      this._apiEndpoint = this.getAttribute('api-endpoint') || '';
      this.checkAuth();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'token') {
          this._token = newValue;
        } else if (name === 'api-endpoint') {
          this._apiEndpoint = newValue;
        }
        this.checkAuth();
      }
    }

    async checkAuth() {
      if (!this._token) {
        this._authorized = false;
        this.render();
        return;
      }

      try {
        const response = await fetch(this._apiEndpoint, {
          headers: {
            'Authorization': `Bearer ${this._token}`,
            'Content-Type': 'application/json'
          }
        });

        this._authorized = response.ok;
      } catch (e) {
        this._authorized = false;
      }

      this.render();
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .content {
            padding: 20px;
          }
          .unauthorized {
            color: #dc3545;
            padding: 16px;
            background: #f8d7da;
            border-radius: 4px;
          }
        </style>
        ${this._authorized 
          ? `<div class="content"><slot></slot></div>` 
          : `<div class="unauthorized">Access Denied</div>`}
      `;
    }
  }

  customElements.define('secure-component', SecureComponent);
  customElements.define('secure-input', SecureInputComponent);
  customElements.define('authenticated-component', AuthenticatedComponent);

  if (typeof window !== 'undefined') {
    window.WebComponentSecurity = {
      SecurityConfig,
      isValidTagName,
      isValidAttributeName,
      escapeHtml,
      sanitizeHTML,
      validateInput,
      sanitizeAttributeValue,
      createSecureShadowRoot,
      validateAndSanitizeProperties,
      createCSPPolicy,
      SecureComponent,
      SecureInputComponent,
      AuthenticatedComponent
    };
  }

  export { 
    SecurityConfig,
    isValidTagName,
    isValidAttributeName,
    escapeHtml,
    sanitizeHTML,
    validateInput,
    sanitizeAttributeValue,
    createSecureShadowRoot,
    validateAndSanitizeProperties,
    createCSPPolicy,
    SecureComponent,
    SecureInputComponent,
    AuthenticatedComponent
  };
})();