/**
 * Scoped Styling Techniques - Demonstrates style isolation methods for Web Components
 * including Shadow DOM, Constructable Stylesheets, and style encapsulation patterns
 * @module styling/06_2_Scoped-Styling-Techniques
 * @version 1.0.0
 * @example <scoped-panel></scoped-panel>
 */

const BASE_STYLES = new CSSStyleSheet();
BASE_STYLES.replaceSync(`
  *, *::before, *::after {
    box-sizing: border-box;
  }
`);

class ScopedPanel extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #internals;
  #headerElement;
  #contentElement;
  #template;

  static get observedAttributes() {
    return ['collapsed', 'variant', 'no-padding'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#internals = this.attachInternals();

    this.#styleSheet = new CSSStyleSheet();
    this.#template = this.#createTemplate();

    this.#headerElement = null;
    this.#contentElement = null;
  }

  static get formAssociated() {
    return true;
  }

  #createTemplate() {
    const template = document.createElement('template');
    template.innerHTML = `
      <div class="panel">
        <header class="panel-header">
          <slot name="header">
            <h2 class="panel-title">Panel Title</h2>
          </slot>
          <button type="button" class="panel-toggle" aria-label="Toggle panel">
            <span class="toggle-icon"></span>
          </button>
        </header>
        <div class="panel-content">
          <slot></slot>
        </div>
      </div>
    `;
    return template;
  }

  #createStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: block;
        contain: content;
        --panel-bg: #ffffff;
        --panel-border: #e0e0e0;
        --panel-radius: 8px;
        --panel-header-bg: #f5f5f5;
        --panel-header-padding: 12px 16px;
        --panel-content-padding: 16px;
        --panel-title-size: 1.125rem;
        --panel-title-weight: 600;
        --panel-title-color: #333333;
        --panel-transition: height 0.3s ease, opacity 0.2s ease;
        --panel-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      :host([variant="primary"]) {
        --panel-bg: #f8f9fa;
        --panel-border: #007bff;
        --panel-header-bg: #e7f1ff;
        --panel-title-color: #0056b3;
      }

      :host([variant="danger"]) {
        --panel-bg: #fff5f5;
        --panel-border: #dc3545;
        --panel-header-bg: #ffeaea;
        --panel-title-color: #c82333;
      }

      :host([variant="success"]) {
        --panel-bg: #f0fff4;
        --panel-border: #28a745;
        --panel-header-bg: #e8f5e9;
        --panel-title-color: #1e7e34;
      }

      :host([variant="outline"]) {
        --panel-bg: transparent;
        --panel-header-bg: transparent;
        --panel-shadow: none;
      }

      :host([collapsed]) {
        --panel-content-display: none;
      }

      :host([no-padding]) {
        --panel-header-padding: 0;
        --panel-content-padding: 0;
      }

      .panel {
        background: var(--panel-bg);
        border: 1px solid var(--panel-border);
        border-radius: var(--panel-radius);
        box-shadow: var(--panel-shadow);
        overflow: hidden;
      }

      .panel-header {
        align-items: center;
        background: var(--panel-header-bg);
        display: flex;
        justify-content: space-between;
        padding: var(--panel-header-padding);
      }

      .panel-title {
        color: var(--panel-title-color);
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        font-size: var(--panel-title-size);
        font-weight: var(--panel-title-weight);
        margin: 0;
      }

      .panel-toggle {
        align-items: center;
        background: transparent;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        display: flex;
        height: 24px;
        justify-content: center;
        padding: 0;
        width: 24px;
        transition: background-color 0.2s ease;
      }

      .panel-toggle:hover {
        background: rgba(0, 0, 0, 0.1);
      }

      .toggle-icon {
        border-color: var(--panel-title-color);
        border-style: solid;
        border-width: 0 2px 2px 0;
        display: block;
        height: 8px;
        transform: rotate(45deg);
        transition: transform 0.3s ease;
        width: 8px;
      }

      :host([collapsed]) .toggle-icon {
        transform: rotate(-45deg);
      }

      .panel-content {
        padding: var(--panel-content-padding);
        transition: var(--panel-transition);
      }

      .panel-content[hidden] {
        display: none;
      }
    `);
  }

  connectedCallback() {
    this.#createStyles();

    const adoptedStyles = new CSSStyleSheet();
    adoptedStyles.replaceSync(`
      @layer base {
        ${BASE_STYLES.cssRules[0].cssText}
      }
    `);

    this.#shadowRoot.adoptedStyleSheets = [adoptedStyles, this.#styleSheet];
    this.#shadowRoot.appendChild(this.#template.content.cloneNode(true));

    this.#headerElement = this.#shadowRoot.querySelector('.panel-header');
    this.#contentElement = this.#shadowRoot.querySelector('.panel-content');

    this.#setupEventListeners();
    this.#setupAccessibility();

    if (!this.hasAttribute('role')) {
      this.setAttribute('role', 'region');
    }
  }

  disconnectedCallback() {
    this.#cleanupEventListeners();
  }

  #setupEventListeners() {
    const toggle = this.#shadowRoot.querySelector('.panel-toggle');
    toggle?.addEventListener('click', this.#handleToggle.bind(this));

    this.#boundHandlers = {
      toggle: this.#handleToggle.bind(this),
      keydown: this.#handleKeydown.bind(this)
    };
  }

  #cleanupEventListeners() {
    const toggle = this.#shadowRoot.querySelector('.panel-toggle');
    toggle?.removeEventListener('click', this.#boundHandlers?.toggle);
    this.#headerElement?.removeEventListener('keydown', this.#boundHandlers?.keydown);
  }

  #setupAccessibility() {
    const toggle = this.#shadowRoot.querySelector('.panel-toggle');
    if (toggle) {
      toggle.setAttribute('aria-expanded', String(!this.hasAttribute('collapsed')));
      toggle.setAttribute('aria-controls', this.#contentElement?.id || 'panel-content');
    }
  }

  #handleToggle() {
    if (this.hasAttribute('collapsed')) {
      this.removeAttribute('collapsed');
    } else {
      this.setAttribute('collapsed', '');
    }
  }

  #handleKeydown(event) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.#handleToggle();
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'collapsed':
        this.#onCollapsedChange(newValue !== null);
        break;
      case 'variant':
        this.#onVariantChange(newValue);
        break;
      case 'no-padding':
        this.#onNoPaddingChange(newValue !== null);
        break;
    }
  }

  #onCollapsedChange(isCollapsed) {
    if (this.#contentElement) {
      this.#contentElement.hidden = isCollapsed;
    }

    const toggle = this.#shadowRoot.querySelector('.panel-toggle');
    if (toggle) {
      toggle.setAttribute('aria-expanded', String(!isCollapsed));
    }

    this.#internals?.dispatchEvent(
      new CustomEvent('panel-collapse-change', {
        bubbles: true,
        composed: true,
        detail: { collapsed: isCollapsed }
      })
    );
  }

  #onVariantChange(variant) {
    this.#internals?.dispatchEvent(
      new CustomEvent('panel-variant-change', {
        bubbles: true,
        composed: true,
        detail: { variant }
      })
    );
  }

  #onNoPaddingChange(noPadding) {
    if (this.#headerElement && this.#contentElement) {
      if (noPadding) {
        this.#headerElement.style.padding = '0';
        this.#contentElement.style.padding = '0';
      } else {
        this.#headerElement.style.padding = '';
        this.#contentElement.style.padding = '';
      }
    }
  }

  get collapsed() {
    return this.hasAttribute('collapsed');
  }

  set collapsed(value) {
    if (value) {
      this.setAttribute('collapsed', '');
    } else {
      this.removeAttribute('collapsed');
    }
  }

  get variant() {
    return this.getAttribute('variant') || 'default';
  }

  set variant(value) {
    this.setAttribute('variant', value || 'default');
  }

  toggle() {
    this.collapsed = !this.collapsed;
    return this.collapsed;
  }

  expand() {
    this.removeAttribute('collapsed');
  }

  collapse() {
    this.setAttribute('collapsed', '');
  }

  get form() {
    return this.#internals?.form;
  }

  getValidity() {
    return this.#internals?.validity;
  }

  #boundHandlers = null;
}

class ScopedButton extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #buttonElement;

  static get observedAttributes() {
    return ['disabled', 'variant', 'size', 'full-width'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#buttonElement = null;
  }

  #createStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: inline-block;
        contain: content;
        --btn-bg: #007bff;
        --btn-color: #ffffff;
        --btn-border: #007bff;
        --btn-radius: 4px;
        --btn-padding: 8px 16px;
        --btn-font-size: 1rem;
        --btn-font-weight: 500;
        --btn-shadow: none;
        --btn-transition: background-color 0.2s ease, transform 0.1s ease;
      }

      :host([variant="primary"]) {
        --btn-bg: #007bff;
        --btn-border: #007bff;
      }

      :host([variant="secondary"]) {
        --btn-bg: #6c757d;
        --btn-border: #6c757d;
      }

      :host([variant="success"]) {
        --btn-bg: #28a745;
        --btn-border: #28a745;
      }

      :host([variant="danger"]) {
        --btn-bg: #dc3545;
        --btn-border: #dc3545;
      }

      :host([variant="warning"]) {
        --btn-bg: #ffc107;
        --btn-border: #ffc107;
        --btn-color: #212529;
      }

      :host([variant="outline-primary"]) {
        --btn-bg: transparent;
        --btn-border: #007bff;
        --btn-color: #007bff;
      }

      :host([variant="outline-secondary"]) {
        --btn-bg: transparent;
        --btn-border: #6c757d;
        --btn-color: #6c757d;
      }

      :host([variant="ghost"]) {
        --btn-bg: transparent;
        --btn-border: transparent;
        --btn-color: #007bff;
      }

      :host([size="sm"]) {
        --btn-padding: 4px 12px;
        --btn-font-size: 0.875rem;
      }

      :host([size="lg"]) {
        --btn-padding: 12px 24px;
        --btn-font-size: 1.125rem;
      }

      :host([full-width]) {
        display: block;
        width: 100%;
      }

      :host([disabled]) {
        pointer-events: none;
      }

      .button {
        align-items: center;
        background: var(--btn-bg);
        border: 1px solid var(--btn-border);
        border-radius: var(--btn-radius);
        box-shadow: var(--btn-shadow);
        color: var(--btn-color);
        cursor: pointer;
        display: inline-flex;
        font-family: inherit;
        font-size: var(--btn-font-size);
        font-weight: var(--btn-font-weight);
        justify-content: center;
        padding: var(--btn-padding);
        text-decoration: none;
        transition: var(--btn-transition);
        user-select: none;
        width: auto;
      }

      :host([disabled]) .button {
        background: var(--btn-bg);
        opacity: 0.65;
        cursor: not-allowed;
      }

      .button:hover:not(:disabled) {
        filter: brightness(0.9);
      }

      .button:active:not(:disabled) {
        transform: scale(0.98);
      }

      .button:focus-visible {
        outline: 2px solid var(--btn-color);
        outline-offset: 2px;
      }
    `);
  }

  connectedCallback() {
    this.#createStyles();
    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];

    const template = document.createElement('template');
    template.innerHTML = `
      <button type="button" class="button" part="button">
        <slot></slot>
      </button>
    `;

    this.#shadowRoot.appendChild(template.content.cloneNode(true));
    this.#buttonElement = this.#shadowRoot.querySelector('.button');

    this.#setupEventListeners();

    if (!this.hasAttribute('role')) {
      this.setAttribute('role', 'button');
    }

    if (!this.hasAttribute('tabindex')) {
      this.setAttribute('tabindex', '0');
    }
  }

  disconnectedCallback() {
    this.#cleanupEventListeners();
  }

  #setupEventListeners() {
    this.#buttonElement?.addEventListener('click', this.#handleClick.bind(this));
    this.#buttonElement?.addEventListener('keydown', this.#handleKeydown.bind(this));
    this.#buttonElement?.addEventListener('focus', this.#handleFocus.bind(this));
    this.#buttonElement?.addEventListener('blur', this.#handleBlur.bind(this));
  }

  #cleanupEventListeners() {
    this.#buttonElement?.removeEventListener('click', this.#handleClick.bind(this));
  }

  #handleClick(event) {
    if (this.disabled) {
      event.preventDefault();
      event.stopPropagation();
      return;
    }

    this.dispatchEvent(new CustomEvent('button-click', {
      bubbles: true,
      composed: true,
      detail: { originalEvent: event }
    }));
  }

  #handleKeydown(event) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      if (!this.disabled) {
        this.#buttonElement?.click();
      }
    }
  }

  #handleFocus() {
    this.dispatchEvent(new CustomEvent('button-focus', {
      bubbles: true,
      composed: true
    }));
  }

  #handleBlur() {
    this.dispatchEvent(new CustomEvent('button-blur', {
      bubbles: true,
      composed: true
    }));
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'disabled':
        this.#updateDisabledState(newValue !== null);
        break;
      case 'variant':
      case 'size':
      case 'full-width':
        break;
    }
  }

  #updateDisabledState(isDisabled) {
    if (this.#buttonElement) {
      this.#buttonElement.disabled = isDisabled;
      this.#buttonElement.style.opacity = isDisabled ? '0.65' : '1';
      this.#buttonElement.style.cursor = isDisabled ? 'not-allowed' : 'pointer';
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

  get variant() {
    return this.getAttribute('variant') || 'primary';
  }

  set variant(value) {
    this.setAttribute('variant', value || 'primary');
  }

  get size() {
    return this.getAttribute('size') || 'md';
  }

  set size(value) {
    this.setAttribute('size', value || 'md');
  }

  focus() {
    this.#buttonElement?.focus();
  }

  blur() {
    this.#buttonElement?.blur();
  }

  click() {
    this.#buttonElement?.click();
  }
}

class StyleEncapsulationHelper {
  static #createConstructableStyleSheet(cssText) {
    const sheet = new CSSStyleSheet();
    sheet.replaceSync(cssText);
    return sheet;
  }

  static createScopedStyles(cssText, hostSelector = ':host') {
    const prefixed = cssText.replace(/:host/g, hostSelector);
    return this.#createConstructableStyleSheet(prefixed);
  }

  static createLayerStyles(cssText, layerName = 'component') {
    return this.#createConstructableStyleSheet(`
      @layer ${layerName} {
        ${cssText}
      }
    `);
  }

  static createWithMediaQueries(styles) {
    let cssText = '';
    for (const [query, rules] of Object.entries(styles)) {
      cssText += `@media ${query} { ${rules} } `;
    }
    return this.#createConstructableStyleSheet(cssText);
  }

  static injectGlobalStyle(cssText, id) {
    if (!document.getElementById(id)) {
      const style = document.createElement('style');
      style.id = id;
      style.textContent = cssText;
      document.head.appendChild(style);
    }
  }

  static removeGlobalStyle(id) {
    const style = document.getElementById(id);
    if (style) {
      style.remove();
    }
  }
}

window.customElements.define('scoped-panel', ScopedPanel);
window.customElements.define('scoped-button', ScopedButton);

export { ScopedPanel, ScopedButton, StyleEncapsulationHelper };