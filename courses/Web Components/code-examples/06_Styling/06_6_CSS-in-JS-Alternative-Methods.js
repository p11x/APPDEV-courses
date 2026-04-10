/**
 * CSS-in-JS Alternative Methods - Demonstrates various CSS-in-JS approaches for Web Components
 * including tagged template literals, style objects, atomic CSS, and CSS modules pattern
 * @module styling/06_6_CSS-in-JS-Alternative-Methods
 * @version 1.0.0
 * @example <styled-card variant="primary"></styled-card>
 */

function css(strings, ...values) {
  let result = '';
  
  for (let i = 0; i < strings.length; i++) {
    result += strings[i];
    if (i < values.length) {
      const value = values[i];
      if (Array.isArray(value)) {
        result += value.join(' ');
      } else if (value !== undefined && value !== null) {
        result += String(value);
      }
    }
  }
  
  return result;
}

function keyframes(definitions) {
  let result = '@keyframes custom-keyframe { ';
  
  for (const [name, props] of Object.entries(definitions)) {
    result += `${name} { `;
    for (const [prop, value] of Object.entries(props)) {
      const cssProp = prop.replace(/([A-Z])/g, '-$1').toLowerCase();
      result += `${cssProp}: ${value}; `;
    }
    result += '} ';
  }
  
  result += '}';
  return result;
}

function global(definitions) {
  let result = '';
  
  for (const [selector, props] of Object.entries(definitions)) {
    result += `${selector} { `;
    for (const [prop, value] of Object.entries(props)) {
      const cssProp = prop.replace(/([A-Z])/g, '-$1').toLowerCase();
      result += `${cssProp}: ${value}; `;
    }
    result += '} ';
  }
  
  return result;
}

class AtomicCSSGenerator {
  #cache;
  #sheet;

  constructor() {
    this.#cache = new Map();
    this.#sheet = new CSSStyleSheet();
  }

  static #generateClassName(properties) {
    const hash = this.#hashProperties(properties);
    return `atomic-${hash}`;
  }

  static #hashProperties(properties) {
    const str = JSON.stringify(properties);
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      const char = str.charCodeAt(i);
      hash = ((hash << 5) - hash) + char;
      hash = hash & hash;
    }
    return Math.abs(hash).toString(36);
  }

  generate(properties) {
    const className = AtomicCSSGenerator.#generateClassName(properties);
    
    if (!this.#cache.has(className)) {
      const cssText = this.#propertiesToCSS(className, properties);
      this.#sheet.replaceSync(cssText);
      this.#cache.set(className, properties);
    }
    
    return className;
  }

  #propertiesToCSS(className, properties) {
    let cssText = `.${className} { `;
    
    for (const [prop, value] of Object.entries(properties)) {
      const cssProp = prop.replace(/([A-Z])/g, '-$1').toLowerCase();
      cssText += `${cssProp}: ${value}; `;
    }
    
    cssText += '} ';
    return cssText;
  }

  get sheet() {
    return this.#sheet;
  }
}

const atomicGenerator = new AtomicCSSGenerator();

class StyledCard extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #variant;

  static get observedAttributes() {
    return ['variant', 'size', 'elevated'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#variant = 'default';
  }

  #generateStyles() {
    const variantStyles = {
      default: {
        backgroundColor: '#ffffff',
        borderColor: '#e0e0e0',
        color: '#333333'
      },
      primary: {
        backgroundColor: '#007bff',
        borderColor: '#007bff',
        color: '#ffffff'
      },
      secondary: {
        backgroundColor: '#6c757d',
        borderColor: '#6c757d',
        color: '#ffffff'
      },
      success: {
        backgroundColor: '#28a745',
        borderColor: '#28a745',
        color: '#ffffff'
      },
      danger: {
        backgroundColor: '#dc3545',
        borderColor: '#dc3545',
        color: '#ffffff'
      },
      warning: {
        backgroundColor: '#ffc107',
        borderColor: '#ffc107',
        color: '#212529'
      }
    };

    const elevatedStyles = this.hasAttribute('elevated') ? {
      boxShadow: '0 4px 16px rgba(0, 0, 0, 0.15)',
      transform: 'translateY(-2px)'
    } : {
      boxShadow: '0 2px 8px rgba(0, 0, 0, 0.1)',
      transform: 'none'
    };

    const variantStyle = variantStyles[this.#variant] || variantStyles.default;
    
    return css`
      :host {
        display: block;
      }
      .card {
        ${css`
          background-color: ${variantStyle.backgroundColor};
          border-color: ${variantStyle.borderColor};
          color: ${variantStyle.color};
          border-radius: 8px;
          border-width: 1px;
          border-style: solid;
          padding: 16px;
          transition: all 0.3s ease;
          ${elevatedStyles}
        `}
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        font-size: 1rem;
        line-height: 1.5;
      }
      .title {
        font-size: 1.25rem;
        font-weight: 600;
        margin: 0 0 8px 0;
      }
    `;
  }

  connectedCallback() {
    this.#variant = this.getAttribute('variant') || 'default';
    this.#render();
  }

  #render() {
    const styles = this.#generateStyles();
    this.#styleSheet.replaceSync(styles);
    
    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    
    const template = document.createElement('template');
    template.innerHTML = `
      <article class="card">
        <h2 class="title"><slot name="title">Card Title</slot></h2>
        <div class="content">
          <slot></slot>
        </div>
      </article>
    `;
    
    this.#shadowRoot.appendChild(template.content.cloneNode(true));
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    if (name === 'variant') {
      this.#variant = newValue || 'default';
      this.#render();
    }
  }
}

class StyledButton extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #size;

  static get observedAttributes() {
    return ['variant', 'size', 'fullwidth', 'disabled'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#size = 'medium';
  }

  #getSizeStyles() {
    const sizes = {
      small: css`
        font-size: 0.875rem;
        padding: 6px 12px;
      `,
      medium: css`
        font-size: 1rem;
        padding: 8px 16px;
      `,
      large: css`
        font-size: 1.125rem;
        padding: 12px 24px;
      `
    };

    return sizes[this.#size] || sizes.medium;
  }

  #getVariantStyles() {
    const variant = this.getAttribute('variant') || 'primary';
    
    const variants = {
      primary: css`
        background-color: #007bff;
        border-color: #007bff;
        color: #ffffff;
      `,
      secondary: css`
        background-color: #6c757d;
        border-color: #6c757d;
        color: #ffffff;
      `,
      outline: css`
        background-color: transparent;
        border-color: #007bff;
        color: #007bff;
      `
    };

    return variants[variant] || variants.primary;
  }

  #render() {
    const styles = css`
      :host {
        display: inline-block;
      }
      :host([disabled]) {
        pointer-events: none;
        opacity: 0.65;
      }
      :host([fullwidth]) {
        display: block;
        width: 100%;
      }
      .button {
        align-items: center;
        border-radius: 4px;
        border-style: solid;
        border-width: 1px;
        cursor: pointer;
        display: inline-flex;
        font-family: inherit;
        font-weight: 500;
        justify-content: center;
        text-decoration: none;
        transition: all 0.2s ease;
        user-select: none;
        width: auto;
      }
      :host([disabled]) .button {
        cursor: not-allowed;
        opacity: 0.65;
      }
      .button:hover:not(:disabled) {
        filter: brightness(0.9);
      }
      .button:active:not(:disabled) {
        transform: scale(0.98);
      }
      ${this.#getSizeStyles()}
      ${this.#getVariantStyles()}
    `;

    this.#styleSheet.replaceSync(styles);

    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];

    const template = document.createElement('template');
    template.innerHTML = `
      <button type="button" class="button" part="button">
        <slot></slot>
      </button>
    `;

    this.#shadowRoot.appendChild(template.content.cloneNode(true));

    this.#setupAccessibility();
  }

  #setupAccessibility() {
    const button = this.#shadowRoot.querySelector('.button');
    if (button) {
      if (!this.hasAttribute('role')) {
        this.setAttribute('role', 'button');
      }
      if (!this.hasAttribute('tabindex')) {
        this.setAttribute('tabindex', '0');
      }
      button.disabled = this.hasAttribute('disabled');
    }
  }

  connectedCallback() {
    this.#size = this.getAttribute('size') || 'medium';
    this.#render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    this.#render();
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
}

class StyleInjector {
  #sheets;

  constructor() {
    this.#sheets = new Map();
  }

  inject(id, cssText, scope = null) {
    const sheet = new CSSStyleSheet();
    sheet.replaceSync(cssText);

    if (scope) {
      scope.adoptedStyleSheets = [...scope.adoptedStyleSheets, sheet];
    }

    this.#sheets.set(id, sheet);
    return sheet;
  }

  remove(id) {
    const sheet = this.#sheets.get(id);
    if (sheet) {
      this.#sheets.delete(id);
      return true;
    }
    return false;
  }

  get(id) {
    return this.#sheets.get(id);
  }

  get all() {
    return Object.fromEntries(this.#sheets);
  }
}

const globalStyleInjector = new StyleInjector();

function createGlobalStyles(id, definitions) {
  const cssText = global(definitions);
  return globalStyleInjector.inject(id, cssText, document);
}

function createKeyframes(id, definitions) {
  const cssText = keyframes(definitions);
  return globalStyleInjector.inject(id, cssText, document);
}

function useAtomicClass(properties) {
  return atomicGenerator.generate(properties);
}

window.customElements.define('styled-card', StyledCard);
window.customElements.define('styled-button', StyledButton);

export { css, keyframes, global, StyledCard, StyledButton, StyleInjector, createGlobalStyles, createKeyframes, useAtomicClass };