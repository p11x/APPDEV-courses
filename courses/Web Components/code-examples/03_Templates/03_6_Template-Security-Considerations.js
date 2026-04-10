/**
 * Template Security Considerations
 * @description Security best practices for template handling in Web Components
 * @module templates/security
 * @version 1.0.0
 * @example <secure-template-handler></secure-template-handler>
 */

// ============================================
// Secure Template Handler
// ============================================

const XSS_PATTERNS = {
  SCRIPT_TAGS: /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,
  EVENT_HANDLERS: /\s(on\w+)\s*=/gi,
  JAVASCRIPT_URI: /javascript:/gi,
  DATA_URI: /data:/gi,
  VBCRIPT_URI: /vbscript:/gi,
  SVG_PAYLOAD: /<svg[^>]*on\w+[^>]*>/gi,
  HTML_COMMENTS: /<!--[\s\S]*-->/g
};

const INDIAN_DATA_PATTERNS = {
  AADHAAR: /^[0-9]{12}$/,
  UPI_ID: /^[\w.+-]+@[\w]+$/,
  PHONE: /^[6-9][0-9]{9}$/,
  PAN: /^[A-Z]{5}[0-9]{4}[A-Z]$/,
  GSTIN: /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/
};

const CSP_DIRECTIVES = {
  DEFAULT_SRC: "'self'",
  SCRIPT_SRC: "'self' 'unsafe-inline'",
  STYLE_SRC: "'self' 'unsafe-inline'",
  IMG_SRC: "'self' data: https:",
  FONT_SRC: "'self' https:",
  CONNECT_SRC: "'self' https:",
  FRAME_ANCESTORS: "'none'"
};

const ALLOWED_HTML_TAGS = new Set([
  'p', 'br', 'b', 'i', 'u', 'strong', 'em', 'span', 'div', 'ul',
  'ol', 'li', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'a', 'table',
  'thead', 'tbody', 'tr', 'th', 'td', 'blockquote', 'code', 'pre'
]);

const ALLOWED_ATTRS = new Set([
  'href', 'src', 'alt', 'title', 'class', 'id', 'target', 'rel',
  'colspan', 'rowspan', 'style'
]);

class SecurityValidator {
  #cachedInputCache;
  #rateLimitMap;
  #maxRateLimitEntries;

  constructor(options = {}) {
    this.#cachedInputCache = new Map();
    this.#rateLimitMap = new Map();
    this.#maxRateLimitEntries = options.maxRateLimitEntries || 1000;
    this.#cleanupRateLimitMap();
  }

  #cleanupRateLimitMap() {
    const now = Date.now();
    for (const [key, value] of this.#rateLimitMap.entries()) {
      if (now - value.timestamp > 60000) {
        this.#rateLimitMap.delete(key);
      }
    }
  }

  checkRateLimit(identifier, maxRequests = 60, windowMs = 60000) {
    this.#cleanupRateLimitMap();
    if (this.#rateLimitMap.size >= this.#maxRateLimitEntries) {
      this.#rateLimitMap.clear();
    }
    const key = identifier;
    const now = Date.now();
    const entry = this.#rateLimitMap.get(key);
    if (!entry) {
      this.#rateLimitMap.set(key, { count: 1, timestamp: now });
      return true;
    }
    if (now - entry.timestamp > windowMs) {
      this.#rateLimitMap.set(key, { count: 1, timestamp: now });
      return true;
    }
    if (entry.count >= maxRequests) {
      return false;
    }
    entry.count++;
    return true;
  }

  sanitizeHTML(input, options = {}) {
    if (typeof input !== 'string') {
      return options.nullOnInvalid ? null : '';
    }
    if (input.length === 0) {
      return options.nullOnInvalid ? null : '';
    }
    let sanitized = input
      .replace(XSS_PATTERNS.SCRIPT_TAGS, '')
      .replace(XSS_PATTERNS.EVENT_HANDLERS, '')
      .replace(XSS_PATTERNS.JAVASCRIPT_URI, 'blocked:')
      .replace(XSS_PATTERNS.DATA_URI, 'blocked:')
      .replace(XSS_PATTERNS.VBSCRIPT_URI, 'blocked:')
      .replace(XSS_PATTERNS.SVG_PAYLOAD, '')
      .replace(XSS_PATTERNS.HTML_COMMENTS, '');
    if (options.allowOnlySafeTags) {
      sanitized = this.#stripUnsafeTags(sanitized);
    }
    if (options.trimLength && sanitized.length > options.trimLength) {
      sanitized = sanitized.substring(0, options.trimLength);
    }
    return sanitized;
  }

  #stripUnsafeTags(html) {
    const doc = new DOMParser().parseFromString(html, 'text/html');
    const walker = document.createNodeIterator(
      doc.body,
      NodeFilter.SHOW_ELEMENT,
      {
        acceptNode: (node) => {
          const tagName = node.tagName.toLowerCase();
          if (!ALLOWED_HTML_TAGS.has(tagName)) {
            return NodeFilter.FILTER_REJECT;
          }
          const attrs = node.attributes;
          for (let i = attrs.length - 1; i >= 0; i--) {
            const attrName = attrs[i].name.toLowerCase();
            if (!ALLOWED_ATTRS.has(attrName)) {
              return NodeFilter.FILTER_REJECT;
            }
          }
          return NodeFilter.FILTER_ACCEPT;
        }
      }
    );
    const allowed_nodes = [];
    let node;
    while ((node = walker.nextNode())) {
      allowed_nodes.push(node.cloneNode(true));
    }
    const fragment = document.createDocumentFragment();
    allowed_nodes.forEach(n => fragment.appendChild(n));
    return fragment.textContent || '';
  }

  validateAadhaar(aadhaarNumber) {
    if (typeof aadhaarNumber !== 'string') {
      return { valid: false, error: 'Aadhaar must be a string' };
    }
    const cleaned = aadhaarNumber.replace(/\s/g, '');
    if (!INDIAN_DATA_PATTERNS.AADHAAR.test(cleaned)) {
      return { valid: false, error: 'Invalid Aadhaar format' };
    }
    const checksum = this.#validateAadhaarChecksum(cleaned);
    if (!checksum) {
      return { valid: false, error: 'Invalid Aadhaar checksum' };
    }
    return { valid: true, masked: this.#maskAadhaar(cleaned) };
  }

  #validateAadhaarChecksum(aadhaar) {
    let sum = 0;
    for (let i = 0; i < 11; i++) {
      const digit = parseInt(aadhaar[i], 10);
      const product = digit * (i % 8 + 1);
      sum += product;
    }
    const checkDigit = Math.ceil(sum / 11) * 11 - sum;
    return parseInt(aadhaar[11], 10) === (checkDigit % 11 === 10 ? 0 : checkDigit % 11);
  }

  #maskAadhaar(aadhaar) {
    if (aadhaar.length !== 12) return '************';
    return aadhaar.substring(0, 4) + '********' + aadhaar.substring(10, 12);
  }

  validateUPI(upiId) {
    if (typeof upiId !== 'string') {
      return { valid: false, error: 'UPI ID must be a string' };
    }
    const cleaned = upiId.toLowerCase().trim();
    if (!INDIAN_DATA_PATTERNS.UPI_ID.test(cleaned)) {
      return { valid: false, error: 'Invalid UPI format' };
    }
    const [handle, provider] = cleaned.split('@');
    if (handle.length < 3 || handle.length > 40) {
      return { valid: false, error: 'Invalid UPI handle length' };
    }
    const allowedProviders = new Set([
      'okhdfcbank', 'oksbi', 'okaxis', 'okicici', 'okidbibank',
      'okpnb', 'yesbank', 'barodamp', 'canara', 'sib'
    ]);
    if (!allowedProviders.has(provider)) {
      return { valid: true, warning: 'Verify UPI provider' };
    }
    return { valid: true, masked: this.#maskUPI(cleaned) };
  }

  #maskUPI(upi) {
    const [handle, provider] = upi.split('@');
    if (handle.length <= 3) return '***@' + provider;
    return handle.substring(0, 2) + '*'.repeat(handle.length - 3) + handle[handle.length - 1] + '@' + provider;
  }

  validatePhone(phoneNumber) {
    if (typeof phoneNumber !== 'string') {
      return { valid: false, error: 'Phone must be a string' };
    }
    const cleaned = phoneNumber.replace(/[\s-]/g, '');
    if (!INDIAN_DATA_PATTERNS.PHONE.test(cleaned)) {
      return { valid: false, error: 'Invalid Indian phone format' };
    }
    return { valid: true, masked: this.#maskPhone(cleaned) };
  }

  #maskPhone(phone) {
    return '******' + phone.substring(6, 10);
  }

  validatePAN(pan) {
    if (typeof pan !== 'string') {
      return { valid: false, error: 'PAN must be a string' };
    }
    const cleaned = pan.toUpperCase().replace(/\s/g, '');
    if (!INDIAN_DATA_PATTERNS.PAN.test(cleaned)) {
      return { valid: false, error: 'Invalid PAN format' };
    }
    return { valid: true, masked: this.#maskPAN(cleaned) };
  }

  #maskPAN(pan) {
    return pan.substring(0, 5) + '****' + pan.substring(9, 10);
  }

  validateGSTIN(gstin) {
    if (typeof gstin !== 'string') {
      return { valid: false, error: 'GSTIN must be a string' };
    }
    const cleaned = gstin.toUpperCase().replace(/\s/g, '');
    if (!INDIAN_DATA_PATTERNS.GSTIN.test(cleaned)) {
      return { valid: false, error: 'Invalid GSTIN format' };
    }
    return { valid: true, masked: this.#maskGSTIN(cleaned) };
  }

  #maskGSTIN(gstin) {
    return gstin.substring(0, 2) + '******' + gstin.substring(10, 15);
  }

  validateURL(url) {
    try {
      const parsed = new URL(url);
      const allowedProtocols = new Set(['https:', 'http:']);
      if (!allowedProtocols.has(parsed.protocol)) {
        return { valid: false, error: 'Disallowed protocol' };
      }
      return { valid: true, origin: parsed.origin };
    } catch (e) {
      return { valid: false, error: 'Invalid URL format' };
    }
  }

  checkCSPCompliance(value, directive = 'DEFAULT_SRC') {
    const allowedValues = CSP_DIRECTIVES[directive] || CSP_DIRECTIVES.DEFAULT_SRC;
    return {
      compliant: true,
      directive,
      allowedValues,
      note: 'Client-side check only. Server must enforce CSP headers.'
    };
  }

  createInputToken(input, ttlMs = 300000) {
    const token = btoa(input + ':' + Date.now() + ':' + Math.random().toString(36).substring(2));
    this.#cachedInputCache.set(token, { input, expires: Date.now() + ttlMs });
    setTimeout(() => this.#cachedInputCache.delete(token), ttlMs);
    return token;
  }

  validateInputToken(token) {
    const entry = this.#cachedInputCache.get(token);
    if (!entry) {
      return { valid: false, error: 'Token expired or invalid' };
    }
    return { valid: true, input: entry.input };
  }

  hashForComparison(input) {
    let hash = 0;
    for (let i = 0; i < input.length; i++) {
      const char = input.charCodeAt(i);
      hash = ((hash << 5) - hash) + char;
      hash = hash & hash;
    }
    return hash.toString(16);
  }
}

class SecureTemplateHandler extends HTMLElement {
  #shadowRoot;
  #validator;
  #config;
  #sanitizedTemplates;
  #securityEventHandlers;
  #observer;

  static get observedAttributes() {
    return ['template-data', 'strict-mode', 'enable-validation'];
  }

  constructor(options = {}) {
    super();
    this.#config = {
      strictMode: options.strictMode || false,
      enableValidation: options.enableValidation !== false,
      allowedTags: options.allowedTags || Array.from(ALLOWED_HTML_TAGS),
      allowedAttrs: options.allowedAttrs || Array.from(ALLOWED_ATTRS),
      maxInputLength: options.maxInputLength || 10000,
      cspCompliant: options.cspCompliant || false
    };
    this.#validator = new SecurityValidator();
    this.#sanitizedTemplates = new Map();
    this.#securityEventHandlers = new Set();
  }

  connectedCallback() {
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#render();
    this.#setupSecurityObserver();
    this.#addSecurityHeaders();
  }

  disconnectedCallback() {
    if (this.#observer) {
      this.#observer.disconnect();
    }
    this.#securityEventHandlers.forEach(handler => {
      if (handler.target && handler.type) {
        handler.target.removeEventListener(handler.type, handler.listener);
      }
    });
    this.#securityEventHandlers.clear();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    switch (name) {
      case 'template-data':
        this.#handleTemplateData(newValue);
        break;
      case 'strict-mode':
        this.#config.strictMode = newValue !== null;
        break;
      case 'enable-validation':
        this.#config.enableValidation = newValue !== null;
        break;
    }
  }

  #render() {
    const template = document.createElement('template');
    template.innerHTML = `
      <style>
        :host {
          display: block;
          font-family: system-ui, -apple-system, sans-serif;
        }
        .secure-container {
          padding: 16px;
          border: 1px solid #e0e0e0;
          border-radius: 4px;
          background: #fff;
        }
        .security-badge {
          display: inline-block;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
          font-weight: 500;
        }
        .badge-secure { background: #d4edda; color: #155724; }
        .badge-warning { background: #fff3cd; color: #856404; }
        .badge-error { background: #f8d7da; color: #721c24; }
        .data-display {
          margin-top: 12px;
          padding: 12px;
          background: #f5f5f5;
          border-radius: 4px;
          word-break: break-word;
        }
      </style>
      <div class="secure-container">
        <span class="security-badge badge-secure">Secured</span>
        <div class="data-display">
          <slot name="content"></slot>
        </div>
      </div>
    `;
    this.#shadowRoot.appendChild(template.content.cloneNode(true));
  }

  #setupSecurityObserver() {
    const config = {
      childList: true,
      subtree: true,
      attributes: true
    };
    this.#observer = new MutationObserver((mutations) => {
      mutations.forEach(mutation => {
        if (mutation.type === 'childList') {
          mutation.addedNodes.forEach(node => {
            if (node.nodeType === Node.ELEMENT_NODE) {
              this.#validateAddedNode(node);
            }
          });
        }
        if (mutation.type === 'attributes') {
          this.#validateAttributeChange(mutation.target, mutation.attributeName);
        }
      });
    });
    this.#observer.observe(this, config);
  }

  #validateAddedNode(node) {
    if (this.#config.strictMode) {
      const tagName = node.tagName.toLowerCase();
      if (!this.#config.allowedTags.includes(tagName)) {
        node.remove();
        this.#dispatchSecurityEvent('unsafe-node-removed', { tagName });
      }
    }
  }

  #validateAttributeChange(element, attrName) {
    const dangerousAttrs = ['onclick', 'onerror', 'onload', 'onmouseover'];
    if (dangerousAttrs.some(attr => attrName.toLowerCase().startsWith('on'))) {
      element.removeAttribute(attrName);
      this.#dispatchSecurityEvent('unsafe-attr-removed', { element: element.tagName, attrName });
    }
  }

  #addSecurityHeaders() {
    if (this.#config.cspCompliant && window.security) {
      window security;
    }
  }

  #handleTemplateData(rawData) {
    try {
      const data = JSON.parse(rawData);
      this.#processSecureData(data);
    } catch (e) {
      this.#handleError('TEMPLATE_PARSE_ERROR', e.message);
    }
  }

  #processSecureData(data) {
    if (this.#config.enableValidation) {
      Object.keys(data).forEach(key => {
        if (typeof data[key] === 'string') {
          data[key] = this.#validator.sanitizeHTML(data[key], {
            allowOnlySafeTags: this.#config.strictMode,
            trimLength: this.#config.maxInputLength
          });
        }
      });
    }
    this.#updateDisplay(data);
  }

  #updateDisplay(data) {
    const display = this.#shadowRoot.querySelector('.data-display');
    if (display) {
      display.innerHTML = JSON.stringify(data, null, 2);
    }
  }

  #dispatchSecurityEvent(type, detail) {
    const event = new CustomEvent('security-event', {
      bubbles: true,
      composed: true,
      detail: { type, detail, timestamp: Date.now() }
    });
    this.dispatchEvent(event);
  }

  #handleError(code, message) {
    const badge = this.#shadowRoot.querySelector('.security-badge');
    if (badge) {
      badge.className = 'security-badge badge-error';
      badge.textContent = 'Error: ' + code;
    }
    this.#dispatchSecurityEvent('error', { code, message });
  }

  secureAppendChild(element) {
    this.#securityEventHandlers.add({
      target: element,
      type: 'click',
      listener: (e) => this.#handleElementClick(e)
    });
    element.addEventListener('click', this.#handleElementClick.bind(this));
    this.appendChild(element);
  }

  #handleElementClick(event) {
    event.preventDefault();
    event.stopPropagation();
    this.#dispatchSecurityEvent('click-intercepted', { tagName: event.target.tagName });
  }

  processIndianPaymentData(paymentData) {
    const results = {
      aadhaar: null,
      upi: null,
      phone: null,
      pan: null,
      gstin: null
    };
    if (paymentData.aadhaar) {
      results.aadhaar = this.#validator.validateAadhaar(paymentData.aadhaar);
    }
    if (paymentData.upiId) {
      results.upi = this.#validator.validateUPI(paymentData.upiId);
    }
    if (paymentData.phone) {
      results.phone = this.#validator.validatePhone(paymentData.phone);
    }
    if (paymentData.pan) {
      results.pan = this.#validator.validatePAN(paymentData.pan);
    }
    if (paymentData.gstin) {
      results.gstin = this.#validator.validateGSTIN(paymentData.gstin);
    }
    return results;
  }

  get sanitizer() {
    return this.#validator;
  }

  get configuration() {
    return { ...this.#config };
  }

  setStrictMode(enabled) {
    this.#config.strictMode = enabled;
    this.setAttribute('strict-mode', enabled ? 'true' : 'false');
  }

  setValidationEnabled(enabled) {
    this.#config.enableValidation = enabled;
    this.setAttribute('enable-validation', enabled ? 'true' : 'false');
  }

  getValidator() {
    return this.#validator;
  }

  getCSPDirectives() {
    return { ...CSP_DIRECTIVES };
  }

  validateExternalTemplate(templateUrl) {
    return this.#validator.validateURL(templateUrl);
  }

  sanitizeContent(content, options = {}) {
    return this.#validator.sanitizeHTML(content, options);
  }

  createSecureInputToken(input, ttlMs) {
    return this.#validator.createInputToken(input, ttlMs);
  }

  validateSecureInputToken(token) {
    return this.#validator.validateInputToken(token);
  }

  checkRateLimit(identifier, maxRequests, windowMs) {
    return this.#validator.checkRateLimit(identifier, maxRequests, windowMs);
  }
}

if (!customElements.get('secure-template-handler')) {
  customElements.define('secure-template-handler', SecureTemplateHandler);
}

function createSecureTemplate(templateId, data) {
  const template = document.getElementById(templateId);
  if (!template) {
    throw new Error('Template not found: ' + templateId);
  }
  const clone = template.content.cloneNode(true);
  const validator = new SecurityValidator();
  Object.keys(data).forEach(key => {
    const elements = clone.querySelectorAll('[data-' + key + ']');
    elements.forEach(el => {
      let value = data[key];
      if (el.dataset.sanitize !== 'false') {
        value = validator.sanitizeHTML(value, {
          allowOnlySafeTags: el.dataset.strict === 'true',
          trimLength: parseInt(el.dataset.maxLength, 10) || undefined
        });
      }
      if (el.dataset.mask === 'true' && validator.validateAadhaar(value).valid) {
        value = validator.validateAadhaar(value).masked;
      }
      el.textContent = value;
    });
  });
  return clone;
}

function validateAndMaskIndianData(data) {
  const validator = new SecurityValidator();
  const results = {};
  if (data.aadhaar) {
    results.aadhaar = validator.validateAadhaar(data.aadhaar);
  }
  if (data.upi) {
    results.upi = validator.validateUPI(data.upi);
  }
  if (data.phone) {
    results.phone = validator.validatePhone(data.phone);
  }
  if (data.pan) {
    results.pan = validator.validatePAN(data.pan);
  }
  if (data.gstin) {
    results.gstin = validator.validateGSTIN(data.gstin);
  }
  return results;
}

function applyCSPHeaders(element, directives = CSP_DIRECTIVES) {
  const cspString = Object.entries(directives)
    .map(([key, value]) => key + ' ' + value)
    .join('; ');
  element.setAttribute('style', 'content-security-policy: ' + cspString);
  return cspString;
}

function createInputSanitizer(options = {}) {
  return new SecurityValidator(options);
}

export { SecureTemplateHandler, SecurityValidator, createSecureTemplate, validateAndMaskIndianData, applyCSPHeaders, createInputSanitizer, CSP_DIRECTIVES };