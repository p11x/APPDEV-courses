/**
 * CSS Custom Properties in Components - Demonstrates CSS custom properties (CSS variables)
 * for theming and flexible component styling with reactive updates
 * @module styling/06_1_CSS-Custom-Properties-in-Components
 * @version 1.0.0
 * @example <themeable-card></themeable-card>
 */

const TEMPLATE = document.createElement('template');
TEMPLATE.innerHTML = `
  <style>
    :host {
      display: block;
      contain: content;
      --card-bg: #ffffff;
      --card-border: #e0e0e0;
      --card-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      --card-radius: 8px;
      --card-padding: 16px;
      --card-title-color: #333333;
      --card-text-color: #666666;
      --card-accent-color: #007bff;
      --card-transition: all 0.3s ease;
    }

    :host([theme="dark"]) {
      --card-bg: #1a1a1a;
      --card-border: #333333;
      --card-shadow: 0 2px 8px rgba(0, 0, 0, 0.4);
      --card-title-color: #ffffff;
      --card-text-color: #b0b0b0;
      --card-accent-color: #4dabf7;
    }

    :host([theme="ocean"]) {
      --card-bg: #e3f2fd;
      --card-border: #90caf9;
      --card-shadow: 0 2px 12px rgba(33, 150, 243, 0.2);
      --card-title-color: #1565c0;
      --card-text-color: #1976d2;
      --card-accent-color: #0288d1;
    }

    :host([theme="forest"]) {
      --card-bg: #e8f5e9;
      --card-border: #a5d6a7;
      --card-shadow: 0 2px 12px rgba(56, 142, 60, 0.2);
      --card-title-color: #2e7d32;
      --card-text-color: #388e3c;
      --card-accent-color: #43a047;
    }

    .card {
      background: var(--card-bg);
      border: 1px solid var(--card-border);
      border-radius: var(--card-radius);
      box-shadow: var(--card-shadow);
      padding: var(--card-padding);
      transition: var(--card-transition);
    }

    .card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
    }

    .card-title {
      color: var(--card-title-color);
      font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
      font-size: 1.25rem;
      font-weight: 600;
      margin: 0 0 12px 0;
      padding-bottom: 8px;
      border-bottom: 2px solid var(--card-accent-color);
    }

    .card-content {
      color: var(--card-text-color);
      font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
      font-size: 1rem;
      line-height: 1.5;
    }

    .card-actions {
      display: flex;
      gap: 8px;
      margin-top: 16px;
      padding-top: 12px;
      border-top: 1px solid var(--card-border);
    }

    button {
      background: var(--card-accent-color);
      border: none;
      border-radius: 4px;
      color: white;
      cursor: pointer;
      font-family: inherit;
      font-size: 0.875rem;
      padding: 8px 16px;
      transition: opacity 0.2s ease;
    }

    button:hover {
      opacity: 0.9;
    }

    button:active {
      transform: scale(0.98);
    }
  </style>
  <div class="card">
    <h2 class="card-title"><slot name="title">Default Title</slot></h2>
    <div class="card-content">
      <slot></slot>
    </div>
    <div class="card-actions">
      <button id="primary-action">Action</button>
    </div>
  </div>
`;

class ThemeableCard extends HTMLElement {
  #shadowRoot;
  #internals;
  #actionButton;
  #themeObserver;

  static get observedAttributes() {
    return ['theme', 'disabled', 'no-hover'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#internals = this.attachInternals();
    this.#shadowRoot.appendChild(TEMPLATE.content.cloneNode(true));

    this.#actionButton = this.#shadowRoot.getElementById('primary-action');
    this.#themeObserver = null;
    this.#boundHandlers = {
      handleAction: this.#handleAction.bind(this),
      observerCallback: this.#observerCallback.bind(this)
    };
  }

  static get formAssociated() {
    return true;
  }

  static get observedThemes() {
    return ['default', 'dark', 'ocean', 'forest'];
  }

  #handleAction(event) {
    if (this.disabled) return;

    const canProceed = this.#internals?.dispatchEvent(
      new CustomEvent('card-action', {
        bubbles: true,
        composed: true,
        detail: { originalEvent: event }
      })
    );

    if (canProceed !== false) {
      this.#performAction();
    }
  }

  #performAction() {
    console.log('Card action performed');
  }

  #observerCallback(mutations) {
    for (const mutation of mutations) {
      if (mutation.type === 'attributes' && mutation.attributeName) {
        this.#handleAttributeChange(mutation.attributeName, mutation.oldValue, this.getAttribute(mutation.attributeName));
      }
    }
  }

  connectedCallback() {
    if (!this.hasAttribute('role')) {
      this.setAttribute('role', 'article');
    }

    this.#actionButton?.addEventListener('click', this.#boundHandlers.handleAction);

    if (this.#shadowRoot && MutationObserver) {
      this.#themeObserver = new MutationObserver(this.#boundHandlers.observerCallback);
      this.#themeObserver.observe(this, {
        attributes: true,
        attributeOldValue: true,
        attributeFilter: ['style', 'class']
      });
    }

    this.#applyDefaultTheme();
    this.#setupCustomPropertyAPI();
  }

  disconnectedCallback() {
    this.#actionButton?.removeEventListener('click', this.#boundHandlers.handleAction);
    this.#themeObserver?.disconnect();
  }

  #applyDefaultTheme() {
    const theme = this.getAttribute('theme') || 'default';
    if (!ThemeableCard.observedThemes.includes(theme)) {
      this.setAttribute('theme', 'default');
    }
  }

  #setupCustomPropertyAPI() {
    this.#defineCSSPropertyGetter('card-bg', 'background');
    this.#defineCSSPropertyGetter('card-border', 'border');
    this.#defineCSSPropertyGetter('card-shadow', 'box-shadow');
  }

  #defineCSSPropertyGetter(name, fallback) {
    const getterName = name.replace(/-([a-z])/g, (_, char) => char.toUpperCase());
    Object.defineProperty(this, getterName, {
      get: () => {
        return this.style.getPropertyValue(`--${name}`) ||
               getComputedStyle(this).getPropertyValue(`--${name}`).trim() ||
               '';
      },
      set: (value) => {
        if (value === null || value === undefined) {
          this.style.removeProperty(`--${name}`);
        } else {
          this.style.setProperty(`--${name}`, value);
        }
      }
    });
  }

  attributeChangedCallback(name, oldValue, newValue) {
    this.#handleAttributeChange(name, oldValue, newValue);
  }

  #handleAttributeChange(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'theme':
        this.#onThemeChange(oldValue, newValue);
        break;
      case 'disabled':
        this.#onDisabledChange(newValue !== null);
        break;
      case 'no-hover':
        this.#onNoHoverChange(newValue !== null);
        break;
    }
  }

  #onThemeChange(oldTheme, newTheme) {
    this.#internals?.dispatchEvent(
      new CustomEvent('theme-change', {
        bubbles: true,
        composed: true,
        detail: { oldTheme, newTheme }
      })
    );
  }

  #onDisabledChange(isDisabled) {
    if (this.#actionButton) {
      this.#actionButton.disabled = isDisabled;
      this.#actionButton.style.opacity = isDisabled ? '0.5' : '1';
      this.#actionButton.style.cursor = isDisabled ? 'not-allowed' : 'pointer';
    }

    this.#internals?.setFormValue(this.disabled ? 'disabled' : null);
  }

  #onNoHoverChange(noHover) {
    const card = this.#shadowRoot.querySelector('.card');
    if (card) {
      if (noHover) {
        card.style.transition = 'none';
      } else {
        card.style.transition = '';
      }
    }
  }

  get theme() {
    return this.getAttribute('theme') || 'default';
  }

  set theme(value) {
    if (value && ThemeableCard.observedThemes.includes(value)) {
      this.setAttribute('theme', value);
    } else if (value === null) {
      this.removeAttribute('theme');
    }
  }

  get disabled() {
    return this.hasAttribute('disabled');
  }

  set disabled(value) {
    if (value) {
      this.setAttribute('disabled', '');
    } else {
      this.removeAttribute('disabled');
    }
  }

  get cardBackground() {
    return this.style.getPropertyValue('--card-bg');
  }

  set cardBackground(value) {
    this.style.setProperty('--card-bg', value);
  }

  get cardAccent() {
    return this.style.getPropertyValue('--card-accent-color');
  }

  set cardAccent(value) {
    this.style.setProperty('--card-accent-color', value);
  }

  get form() {
    return this.#internals?.form;
  }

  get validity() {
    return this.#internals?.validity;
  }

  get willValidate() {
    return this.#internals?.willValidate;
  }

  checkValidity() {
    return this.#internals?.checkValidity() ?? true;
  }

  reportValidity() {
    return this.#internals?.reportValidity() ?? true;
  }

  getCSSCustomProperty(name) {
    return getComputedStyle(this).getPropertyValue(name).trim();
  }

  setCSSCustomProperty(name, value, priority = '') {
    if (name.startsWith('--')) {
      this.style.setProperty(name, value, priority);
    } else {
      this.style.setProperty(`--${name}`, value, priority);
    }
  }

  removeCSSCustomProperty(name) {
    const propName = name.startsWith('--') ? name : `--${name}`;
    this.style.removeProperty(propName);
  }

  getAllCSSCustomProperties() {
    const computedStyle = getComputedStyle(this);
    const properties = {};
    const len = computedStyle.length;

    for (let i = 0; i < len; i++) {
      const name = computedStyle[i];
      if (name.startsWith('--')) {
        properties[name] = computedStyle.getPropertyValue(name).trim();
      }
    }

    return properties;
  }

  animateCSSProperty(name, from, to, duration = 300, easing = 'ease') {
    const animation = this.animate(
      [
        { [name.startsWith('--') ? name : `--${name}`]: from },
        { [name.startsWith('--') ? name : `--${name}`]: to }
      ],
      { duration, easing }
    );

    return animation.finished;
  }

  #boundHandlers = null;
}

class CSSPropertyWatcher extends HTMLElement {
  #shadowRoot;
  #observedProperties;
  #callback;
  #animationFrame;
  #lastValues;

  static get observedAttributes() {
    return ['property', 'callback'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#observedProperties = new Map();
    this.#callback = null;
    this.#animationFrame = null;
    this.#lastValues = new Map();
  }

  connectedCallback() {
    this.#render();
    this.#setupWatcher();
  }

  disconnectedCallback() {
    cancelAnimationFrame(this.#animationFrame);
  }

  #render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: none;
        }
      </style>
      <slot></slot>
    `;
  }

  #setupWatcher() {
    const properties = this.getAttribute('property')?.split(',').map(p => p.trim()) || [];
    const callbackName = this.getAttribute('callback');

    if (callbackName && window[callbackName]) {
      this.#callback = window[callbackName];
    }

    for (const prop of properties) {
      this.#observedProperties.set(prop, this.style.getPropertyValue(prop) || '');
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (name === 'callback' && newValue) {
      this.#callback = window[newValue] || null;
    }
  }
}

class CustomPropertyProvider extends HTMLElement {
  #shadowRoot;
  #properties;

  static get observedAttributes() {
    return ['properties', 'selector'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#properties = new Map();
  }

  connectedCallback() {
    this.#render();
    this.#parseProperties();

    const selector = this.getAttribute('selector') || ':root';
    this.#applyProperties(selector);
  }

  #render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: none;
        }
      </style>
    `;
  }

  #parseProperties() {
    const propString = this.getAttribute('properties') || '';
    const pairs = propString.split(';');

    for (const pair of pairs) {
      const [name, value] = pair.split(':').map(s => s.trim());
      if (name && value) {
        this.#properties.set(name, value);
      }
    }
  }

  #applyProperties(selector) {
    for (const [name, value] of this.#properties) {
      document.documentElement.style.setProperty(name, value);
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    if (name === 'properties') {
      this.#properties.clear();
      this.#parseProperties();

      const selector = this.getAttribute('selector') || ':root';
      this.#applyProperties(selector);
    }
  }

  setProperty(name, value, priority = '') {
    this.#properties.set(name, value);
    const selector = this.getAttribute('selector') || ':root';
    document.documentElement.style.setProperty(name, value, priority);
  }

  getProperty(name) {
    return this.#properties.get(name);
  }

  removeProperty(name) {
    this.#properties.delete(name);
    document.documentElement.style.removeProperty(name);
  }
}

window.customElements.define('themeable-card', ThemeableCard);
window.customElements.define('css-property-watcher', CSSPropertyWatcher);
window.customElements.define('custom-property-provider', CustomPropertyProvider);

export { ThemeableCard, CSSPropertyWatcher, CustomPropertyProvider };