/**
 * Security Best Practices - Security patterns for Web Components including
 * XSS prevention, CSP compliance, input validation, and secure data handling
 * @module advanced-patterns/10_4_Security-Best-Practices
 * @version 1.0.0
 * @example <secure-component></secure-component>
 */

class SecurityValidator {
  constructor() {
    this.patterns = {
      url: /^https?:\/\/[^\s/$.?#].[^\s]*$/i,
      email: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
      alphanumeric: /^[a-zA-Z0-9]+$/,
      numeric: /^[0-9]+$/,
    };
    this.blockedPatterns = [
      /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,
      /javascript:/gi,
      /on\w+\s*=/gi,
      /data:\s*text\/html/gi,
    ];
  }

  validate(value, type) {
    switch (type) {
      case 'url':
        return this.patterns.url.test(value) && !this.containsBlockedPatterns(value);
      case 'email':
        return this.patterns.email.test(value);
      case 'alphanumeric':
        return this.patterns.alphanumeric.test(value);
      case 'numeric':
        return this.patterns.numeric.test(value);
      default:
        return this.sanitize(value).length === value.length;
    }
  }

  sanitize(input) {
    if (typeof input !== 'string') return '';
    
    let sanitized = input
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#x27;')
      .replace(/\//g, '&#x2F;');

    for (const pattern of this.blockedPatterns) {
      sanitized = sanitized.replace(pattern, '');
    }

    return sanitized;
  }

  sanitizeHTML(input, allowed = []) {
    if (typeof input !== 'string') return '';

    const allowedTags = new Set(['p', 'br', 'b', 'i', 'em', 'strong', ...allowed]);
    const doc = new DOMParser().parseFromString(`<div>${input}</div>`, 'text/html');
    const walker = doc.createTreeWalker(doc.body, 1);

    const nodesToRemove = [];
    while (walker.nextNode()) {
      const node = walker.currentNode;
      if (!allowedTags.has(node.nodeName.toLowerCase())) {
        nodesToRemove.push(node);
      }
    }

    for (const node of nodesToRemove) {
      node.parentNode.replaceChild(
        doc.createTextNode(node.textContent),
        node
      );
    }

    return doc.body.innerHTML;
  }

  validateAttr(name, value) {
    const dangerous = ['javascript:', 'data:', 'vbscript:', 'on'];
    for (const d of dangerous) {
      if (value.toLowerCase().startsWith(d)) {
        return false;
      }
    }
    return true;
  }
}

class CSPManager {
  constructor() {
    this.policies = {
      default: "default-src 'self'",
      script: "script-src 'self' 'unsafe-inline' 'unsafe-eval'",
      style: "style-src 'self' 'unsafe-inline'",
      img: "img-src 'self' data: https:",
      connect: "connect-src 'self' https:",
      font: "font-src 'self' data:",
      object: "object-src 'none'",
      base: "base-uri 'self'",
      form: "form-action 'self'",
      frame: "frame-ancestors 'none'",
    };
    this nonce = this.generateNonce();
  }

  generateNonce() {
    const array = new Uint8Array(16);
    crypto.getRandomValues(array);
    return Array.from(array, b => b.toString(16).padStart(2, '0')).join('');
  }

  getPolicy(name = 'default') {
    return this.policies[name] || this.policies.default;
  }

  getFullPolicy() {
    return Object.values(this.policies).join('; ');
  }

  setPolicy(name, value) {
    this.policies[name] = value;
  }

  setNonce() {
    this.nonce = this.generateNonce();
  }

  getNonceAttribute() {
    return `nonce-${this.nonce}`;
  }

  applyToMeta() {
    let meta = document.querySelector('meta[http-equiv="Content-Security-Policy"]');
    if (!meta) {
      meta = document.createElement('meta');
      meta.httpEquiv = 'Content-Security-Policy';
      document.head.appendChild(meta);
    }
    meta.content = this.getFullPolicy();
  }
}

class InputSanitizer {
  constructor() {
    this.validators = new Map();
    this.sanitizers = new Map();
    this.registerDefaults();
  }

  registerDefaults() {
    this.registerValidator('string', v => typeof v === 'string');
    this.registerValidator('number', v => typeof v === 'number' && !isNaN(v));
    this.registerValidator('boolean', v => typeof v === 'boolean');
    this.registerValidator('array', v => Array.isArray(v));
    this.registerValidator('object', v => typeof v === 'object' && v !== null);

    this.registerSanitizer('string', v => String(v).slice(0, 10000));
    this.registerSanitizer('number', v => parseFloat(v) || 0);
    this.registerSanitizer('boolean', v => Boolean(v));
    this.registerSanitizer('html', new SecurityValidator().sanitizeHTML.bind(new SecurityValidator()));
  }

  registerValidator(type, fn) {
    this.validators.set(type, fn);
  }

  registerSanitizer(type, fn) {
    this.sanitizers.set(type, fn);
  }

  validate(value, type) {
    const validator = this.validators.get(type);
    if (!validator) return true;
    return validator(value);
  }

  sanitize(value, type) {
    const sanitizer = this.sanitizers.get(type);
    if (!sanitizer) return value;
    return sanitizer(value);
  }

  clean(value, type, options = {}) {
    if (options.validate !== false && !this.validate(value, type)) {
      if (options.strict) {
        throw new Error(`Invalid value for type: ${type}`);
      }
      return options.default ?? null;
    }

    return this.sanitize(value, type);
  }
}

class SecureEventManager {
  constructor() {
    this.listeners = new WeakMap();
    this.eventTypes = new Set();
    this.allowedOrigins = new Set();
    this.blocked = new Set();
  }

  addEventType(type) {
    this.eventTypes.add(type);
  }

  setAllowedOrigins(origins) {
    this.allowedOrigins.clear();
    origins.forEach(o => this.allowedOrigins.add(o));
  }

  isOriginAllowed(origin) {
    if (this.allowedOrigins.size === 0) return true;
    return this.allowedOrigins.has(origin);
  }

  validateEvent(event) {
    const origin = event.origin || event.getOrigin?.() || window.location.origin;
    
    if (!this.isOriginAllowed(origin)) {
      return false;
    }

    if (this.blocked.has(origin)) {
      return false;
    }

    return true;
  }

  safeDispatch(element, eventType, detail = {}) {
    const event = new CustomEvent(eventType, {
      bubbles: true,
      composed: true,
      cancelable: true,
      detail,
    });
    return element.dispatchEvent(event);
  }

  blockOrigin(origin) {
    this.blocked.add(origin);
  }

  unblockOrigin(origin) {
    this.blocked.delete(origin);
  }

  getBlockedOrigins() {
    return Array.from(this.blocked);
  }
}

class PermissionManager {
  constructor() {
    this.permissions = new Map();
    this.defaults = {
      observeAttributes: true,
      modifyDOM: true,
      accessShadow: true,
      dispatchEvents: true,
    };
  }

  grant(component, permission) {
    const perms = this.permissions.get(component) || { ...this.defaults };
    perms[permission] = true;
    this.permissions.set(component, perms);
  }

  revoke(component, permission) {
    const perms = this.permissions.get(component);
    if (perms) {
      perms[permission] = false;
    }
  }

  check(component, permission) {
    const perms = this.permissions.get(component);
    if (!perms) return this.defaults[permission] ?? false;
    return perms[permission] ?? this.defaults[permission] ?? false;
  }

  revokeAll(component) {
    this.permissions.delete(component);
  }
}

const securityValidator = new SecurityValidator();
const cspManager = new CSPManager();
const inputSanitizer = new InputSanitizer();
const secureEventManager = new SecureEventManager();
const permissionManager = new PermissionManager();

export { SecurityValidator, CSPManager, InputSanitizer, SecureEventManager, PermissionManager };
export { securityValidator, cspManager, inputSanitizer, secureEventManager, permissionManager };

export default securityValidator;