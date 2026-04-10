/**
 * Safe HTML Parsing Methods - Security-focused HTML Parsing
 * @description Security-focused HTML parsing to prevent XSS attacks in Web Components
 * @module templates/safe-parsing
 * @version 1.0.0
 * @example <safe-html-renderer></safe-html-renderer>
 */

// ============================================
// Safe HTML Renderer
// ============================================

/**
 * SafeHTMLRenderer - XSS-safe HTML rendering component
 * 
 * Features:
 * - DOMPurify-style sanitization
 * - Content Security Policy support
 * - Input validation
 * - Output encoding
 * - Script blocking
 * 
 * Props:
 * - html: string - HTML content to render (sanitized)
 * - sanitize: boolean - Enable sanitization (default: true)
 * - allowedTags: string - Comma-separated allowed tags
 * - allowedAttrs: string - Comma-separated allowed attributes
 * 
 * Events:
 * - render: Fired after safe rendering
 * - blocked: Fired when content is blocked
 */
class SafeHTMLRenderer extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._html = '';
    this._sanitize = true;
    this._allowedTags = new Set(['p', 'br', 'b', 'i', 'em', 'strong', 'a', 'ul', 'ol', 'li', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'span', 'div']);
    this._allowedAttrs = new Set(['href', 'class', 'id', 'title', 'alt']);
    this._blockedContent = [];
  }

  static get observedAttributes() {
    return ['html', 'sanitize', 'allowed-tags', 'allowed-attrs'];
  }

  connectedCallback() {
    this._html = this.getAttribute('html') || '';
    this._sanitize = this.hasAttribute('sanitize') ? this.getAttribute('sanitize') !== 'false' : true;
    
    if (this.hasAttribute('allowed-tags')) {
      this._allowedTags = new Set(this.getAttribute('allowed-tags').split(',').map(t => t.trim()));
    }
    if (this.hasAttribute('allowed-attrs')) {
      this._allowedAttrs = new Set(this.getAttribute('allowed-attrs').split(',').map(a => a.trim()));
    }
    
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    
    if (name === 'html') {
      this._html = newValue || '';
    } else if (name === 'sanitize') {
      this._sanitize = newValue !== 'false';
    } else if (name === 'allowed-tags') {
      this._allowedTags = new Set(newValue.split(',').map(t => t.trim()));
    } else if (name === 'allowed-attrs') {
      this._allowedAttrs = new Set(newValue.split(',').map(a => a.trim()));
    }
    
    this.render();
  }

  get html() {
    return this._html;
  }

  set html(value) {
    this._html = value;
    this.setAttribute('html', value);
    this.render();
  }

  /**
   * Sanitize HTML content
   * @private
   */
  _sanitizeHTML(html) {
    if (!this._sanitize) return html;
    
    const parser = new DOMParser();
    const doc = parser.parseFromString(`<div>${html}</div>`, 'text/html');
    const container = doc.body.firstChild;
    
    this._blockedContent = [];
    this._sanitizeNode(container);
    
    if (this._blockedContent.length > 0) {
      this.dispatchEvent(new CustomEvent('blocked', { 
        detail: { blocked: this._blockedContent },
        bubbles: true,
        composed: true
      }));
    }
    
    return container.innerHTML;
  }

  /**
   * Recursively sanitize nodes
   * @private
   */
  _sanitizeNode(node) {
    const dangerousTags = ['script', 'style', 'iframe', 'object', 'embed', 'link', 'base', 'meta', 'form'];
    const dangerousAttrs = ['onerror', 'onload', 'onclick', 'onmouseover', 'onfocus', 'onblur', 'onkeydown', 'onkeyup', 'onchange'];
    
    const elements = node.querySelectorAll('*');
    elements.forEach(el => {
      const tagName = el.tagName.toLowerCase();
      
      if (dangerousTags.includes(tagName)) {
        this._blockedContent.push({ type: 'tag', value: tagName });
        el.remove();
        return;
      }
      
      if (!this._allowedTags.has(tagName)) {
        this._blockedContent.push({ type: 'tag', value: tagName });
        el.remove();
        return;
      }
      
      Array.from(el.attributes).forEach(attr => {
        const attrName = attr.name.toLowerCase();
        
        if (dangerousAttrs.includes(attrName)) {
          this._blockedContent.push({ type: 'attribute', value: attrName, tag: tagName });
          el.removeAttribute(attr.name);
        }
        
        if (attrName.startsWith('on')) {
          this._blockedContent.push({ type: 'event-attr', value: attrName });
          el.removeAttribute(attr.name);
        }
        
        if (!this._allowedAttrs.has(attrName)) {
          this._blockedContent.push({ type: 'attribute', value: attrName, tag: tagName });
          el.removeAttribute(attr.name);
        }
      });
      
      if (tagName === 'a' && el.hasAttribute('href')) {
        const href = el.getAttribute('href').toLowerCase();
        if (href.startsWith('javascript:') || href.startsWith('data:') || href.startsWith('vbscript:')) {
          this._blockedContent.push({ type: 'unsafe-href', value: href });
          el.removeAttribute('href');
        }
      }
    });
    
    return node;
  }

  /**
   * Escape HTML special characters
   * @private
   */
  _escapeHTML(str) {
    const escapeMap = {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#39;'
    };
    return str.replace(/[&<>"']/g, char => escapeMap[char]);
  }

  render() {
    const sanitized = this._sanitizeHTML(this._html);
    
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
        }
        .content {
          font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
          line-height: 1.6;
        }
        .blocked-notice {
          padding: 8px 12px;
          background: #fff3cd;
          border: 1px solid #ffc107;
          border-radius: 4px;
          font-size: 12px;
          color: #856404;
          margin-top: 8px;
        }
      </style>
      <div class="content">${sanitized}</div>
    `;
    
    this.dispatchEvent(new CustomEvent('render', { bubbles: true, composed: true }));
  }

  /**
   * Get blocked content log
   * @returns {Array}
   */
  getBlockedContent() {
    return [...this._blockedContent];
  }
}

// ============================================
// HTML Sanitizer
// ============================================

/**
 * HTMLSanitizer - Static sanitization utilities
 */
class HTMLSanitizer {
  static get DEFAULT_ALLOWED_TAGS() {
    return ['p', 'br', 'b', 'i', 'em', 'strong', 'a', 'ul', 'ol', 'li', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'span', 'div', 'table', 'thead', 'tbody', 'tr', 'th', 'td', 'img', 'code', 'pre'];
  }

  static get DEFAULT_ALLOWED_ATTRS() {
    return ['href', 'src', 'class', 'id', 'title', 'alt', 'colspan', 'rowspan'];
  }

  static get DANGEROUS_TAGS() {
    return ['script', 'style', 'iframe', 'object', 'embed', 'link', 'base', 'meta', 'form', 'input', 'button', 'textarea', 'select', 'canvas', 'video', 'audio', 'source', 'track'];
  }

  static get DANGEROUS_ATTRS() {
    return ['onerror', 'onload', 'onclick', 'onmouseover', 'onmouseout', 'onmousedown', 'onmouseup', 'onfocus', 'onblur', 'onkeydown', 'onkeyup', 'onkeypress', 'onchange', 'onsubmit', 'onreset', 'onfocus', 'oncontextmenu'];
  }

  /**
   * Sanitize HTML string
   * @param {string} html - HTML to sanitize
   * @param {Object} options - Sanitization options
   * @returns {string}
   */
  static sanitize(html, options = {}) {
    const allowedTags = new Set(options.allowedTags || this.DEFAULT_ALLOWED_TAGS);
    const allowedAttrs = new Set(options.allowedAttrs || this.DEFAULT_ALLOWED_ATTRS);
    const blocked = [];
    
    const parser = new DOMParser();
    const doc = parser.parseFromString(`<div>${html}</div>`, 'text/html');
    const container = doc.body.firstChild;
    
    const elements = container.querySelectorAll('*');
    elements.forEach(el => {
      const tagName = el.tagName.toLowerCase();
      
      if (this.DANGEROUS_TAGS.includes(tagName) || !allowedTags.has(tagName)) {
        blocked.push({ type: 'tag', value: tagName });
        el.remove();
        return;
      }
      
      Array.from(el.attributes).forEach(attr => {
        const attrName = attr.name.toLowerCase();
        
        if (this.DANGEROUS_ATTRS.some(d => attrName.includes(d))) {
          blocked.push({ type: 'attribute', value: attrName });
          el.removeAttribute(attr.name);
        } else if (!allowedAttrs.has(attrName)) {
          blocked.push({ type: 'attribute', value: attrName });
          el.removeAttribute(attr.name);
        }
      });
      
      if (tagName === 'a' && el.hasAttribute('href')) {
        const href = el.getAttribute('href').toLowerCase();
        if (href.startsWith('javascript:') || href.startsWith('data:')) {
          blocked.push({ type: 'unsafe-href', value: href });
          el.removeAttribute('href');
        }
      }
    });
    
    return { html: container.innerHTML, blocked };
  }

  /**
   * Escape HTML entities
   * @param {string} str - String to escape
   * @returns {string}
   */
  static escape(str) {
    const escapeMap = {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#39;',
      '/': '&#x2F;'
    };
    return str.replace(/[&<>"'\/]/g, char => escapeMap[char]);
  }

  /**
   * Unescape HTML entities
   * @param {string} str - String to unescape
   * @returns {string}
   */
  static unescape(str) {
    const unescapeMap = {
      '&amp;': '&',
      '&lt;': '<',
      '&gt;': '>',
      '&quot;': '"',
      '&#39;': "'",
      '&#x2F;': '/'
    };
    return str.replace(/&amp;|&lt;|&gt;|&quot;|&#39;|&#x2F;/g, entity => unescapeMap[entity]);
  }

  /**
   * Validate URL for safety
   * @param {string} url - URL to validate
   * @returns {boolean}
   */
  static isSafeURL(url) {
    if (!url) return false;
    
    const normalized = url.toLowerCase().trim();
    const unsafeProtocols = ['javascript:', 'data:', 'vbscript:', 'mailto:'];
    
    if (unsafeProtocols.some(p => normalized.startsWith(p))) {
      return false;
    }
    
    try {
      const parsed = new URL(normalized, window.location.href);
      return ['http:', 'https:'].includes(parsed.protocol);
    } catch {
      return false;
    }
  }

  /**
   * Strip all tags, leaving only text
   * @param {string} html - HTML string
   * @returns {string}
   */
  static stripTags(html) {
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');
    return doc.body.textContent || '';
  }
}

// ============================================
// CSP Helper
// ============================================

/**
 * Content Security Policy utilities
 */
const CSPHelper = {
  /**
   * Apply CSP headers (simulation)
   */
  applyCSP() {
    const meta = document.createElement('meta');
    meta.httpEquiv = 'Content-Security-Policy';
    meta.content = "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'";
    document.head.appendChild(meta);
  },

  /**
   * Check if CSP is enforced
   * @returns {boolean}
   */
  isEnforced() {
    return document.querySelector('meta[http-equiv="Content-Security-Policy"]') !== null;
  }
};

// ============================================
// Export
// ============================================

export { SafeHTMLRenderer, HTMLSanitizer, CSPHelper };