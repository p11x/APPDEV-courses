/**
 * Dynamic Styling Methods - Demonstrates runtime styling techniques for Web Components
 * including dynamic style injection, runtime property updates, and interactive styling
 * @module styling/06_4_Dynamic-Styling-Methods
 * @version 1.0.0
 * @example <dynamic-stylist></dynamic-stylist>
 */

class DynamicStylist extends HTMLElement {
  #shadowRoot;
  #styleElement;
  #rules;
  #variables;
  #animationFrame;

  static get observedAttributes() {
    return ['mode', 'scale', 'debug'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleElement = null;
    this.#rules = new Map();
    this.#variables = new Map();
    this.#animationFrame = null;
  }

  #initialize() {
    this.#styleElement = document.createElement('style');
    this.#rules = new Map();
    this.#variables = new Map();

    this.#rules.set(':host', {
      display: 'block',
      '--stylist-bg': '#ffffff',
      '--stylist-color': '#333333',
      '--stylist-border': '#e0e0e0',
      '--stylist-radius': '8px',
      '--stylist-padding': '16px',
      '--stylist-shadow': '0 2px 8px rgba(0, 0, 0, 0.1)',
      '--stylist-transition': 'all 0.3s ease'
    });
  }

  #updateStyles() {
    let cssText = ':host { display: block; } ';

    for (const [selector, properties] of this.#rules) {
      cssText += `${selector} { `;
      for (const [property, value] of properties) {
        if (property.startsWith('--')) {
          cssText += `${property}: ${value}; `;
        } else {
          cssText += `${property}: ${value}; `;
        }
      }
      cssText += '} ';
    }

    if (this.#styleElement) {
      this.#styleElement.textContent = cssText;
    }
  }

  connectedCallback() {
    this.#initialize();

    this.#shadowRoot.appendChild(this.#styleElement);
    this.#updateStyles();
    this.#render();
    this.#setupEventListeners();
  }

  disconnectedCallback() {
    if (this.#animationFrame) {
      cancelAnimationFrame(this.#animationFrame);
    }
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <div class="stylist-container">
        <div class="stylist-header">
          <h3>Dynamic Stylist</h3>
        </div>
        <div class="stylist-content">
          <slot></slot>
        </div>
        <div class="stylist-controls">
          <button id="increase-size">Size +</button>
          <button id="decrease-size">Size -</button>
          <button id="toggle-mode">Mode</button>
          <button id="reset-styles">Reset</button>
        </div>
      </div>
    `;
  }

  #setupEventListeners() {
    const increaseBtn = this.#shadowRoot.getElementById('increase-size');
    const decreaseBtn = this.#shadowRoot.getElementById('decrease-size');
    const toggleBtn = this.#shadowRoot.getElementById('toggle-mode');
    const resetBtn = this.#shadowRoot.getElementById('reset-styles');

    increaseBtn?.addEventListener('click', () => this.#adjustScale(0.1));
    decreaseBtn?.addEventListener('click', () => this.#adjustScale(-0.1));
    toggleBtn?.addEventListener('click', () => this.#toggleMode());
    resetBtn?.addEventListener('click', () => this.#resetStyles());
  }

  #adjustScale(delta) {
    const current = this.#getCurrentScale();
    const newScale = Math.max(0.5, Math.min(2, current + delta));
    this.#setScale(newScale);
  }

  #getCurrentScale() {
    const transformRule = this.#rules.get('.stylist-container')?.transform;
    if (transformRule) {
      const match = transformRule.match(/scale\(([^)]+)\)/);
      return match ? parseFloat(match[1]) : 1;
    }
    return 1;
  }

  #setScale(scale) {
    this.#scheduleStyleUpdate('.stylist-container', 'transform', `scale(${scale})`);
  }

  #toggleMode() {
    const currentMode = this.getAttribute('mode') || 'light';
    const newMode = currentMode === 'light' ? 'dark' : 'light';
    this.setAttribute('mode', newMode);
  }

  #resetStyles() {
    this.#rules.clear();
    this.#rules.set(':host', {
      display: 'block',
      '--stylist-bg': '#ffffff',
      '--stylist-color': '#333333',
      '--stylist-border': '#e0e0e0',
      '--stylist-radius': '8px',
      '--stylist-padding': '16px'
    });
    this.#updateStyles();

    this.#scheduleStyleUpdate('.stylist-container', 'transform', '');
    this.setAttribute('mode', 'light');
    this.setAttribute('scale', '1');
  }

  #scheduleStyleUpdate(selector, property, value) {
    if (!this.#rules.has(selector)) {
      this.#rules.set(selector, new Map());
    }

    const rule = this.#rules.get(selector);
    if (value === '') {
      rule.delete(property);
    } else {
      rule.set(property, value);
    }

    if (this.#animationFrame) {
      cancelAnimationFrame(this.#animationFrame);
    }

    this.#animationFrame = requestAnimationFrame(() => {
      this.#updateStyles();
      this.#animationFrame = null;
    });
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'mode':
        this.#handleModeChange(newValue);
        break;
      case 'scale':
        this.#handleScaleChange(newValue);
        break;
      case 'debug':
        this.#handleDebugChange(newValue !== null);
        break;
    }
  }

  #handleModeChange(mode) {
    if (mode === 'dark') {
      this.#rules.set(':host', {
        '--stylist-bg': '#1a1a1a',
        '--stylist-color': '#e0e0e0',
        '--stylist-border': '#404040'
      });
    } else {
      this.#rules.set(':host', {
        '--stylist-bg': '#ffffff',
        '--stylist-color': '#333333',
        '--stylist-border': '#e0e0e0'
      });
    }
    this.#updateStyles();
  }

  #handleScaleChange(scale) {
    const numericScale = parseFloat(scale) || 1;
    this.#scheduleStyleUpdate('.stylist-container', 'transform', `scale(${numericScale})`);
  }

  #handleDebugChange(debug) {
    if (debug) {
      console.log('Current styles:', Object.fromEntries(this.#rules));
    }
  }

  setProperty(name, value) {
    this.#variables.set(name, value);

    if (name.startsWith('--')) {
      this.style.setProperty(name, value);
    } else {
      this.style.setProperty(`--${name}`, value);
    }

    this.#scheduleStyleUpdate(':host', name, value);
  }

  getProperty(name) {
    return this.#variables.get(name) || this.style.getPropertyValue(name.startsWith('--') ? name : `--${name}`);
  }

  removeProperty(name) {
    this.#variables.delete(name);
    this.style.removeProperty(name.startsWith('--') ? name : `--${name}`);
  }

  addRule(selector, properties) {
    const rule = new Map(Object.entries(properties));
    this.#rules.set(selector, rule);
    this.#updateStyles();
  }

  removeRule(selector) {
    this.#rules.delete(selector);
    this.#updateStyles();
  }

  updateRule(selector, property, value) {
    if (!this.#rules.has(selector)) {
      this.#rules.set(selector, new Map());
    }

    const rule = this.#rules.get(selector);
    if (value === undefined) {
      rule.delete(property);
    } else {
      rule.set(property, value);
    }

    this.#scheduleStyleUpdate(selector, property, value);
  }

  get allRules() {
    return Object.fromEntries(
      Array.from(this.#rules.entries(), ([selector, properties]) => [
        selector,
        Object.fromEntries(properties)
      ])
    );
  }
}

class RuntimeStyleInjector extends HTMLElement {
  #shadowRoot;
  #injectedStyles;
  #styleId;

  static get observedAttributes() {
    return ['styles', 'priority'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#injectedStyles = new Map();
    this.#styleId = `style-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
  }

  connectedCallback() {
    this.#render();
    this.#parseStyles();
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <style id="${this.#styleId}"></style>
      <slot></slot>
    `;
  }

  #parseStyles() {
    const styles = this.getAttribute('styles');
    if (styles) {
      this.#injectStyles(styles);
    }
  }

  #injectStyles(cssText, priority = '') {
    const styleElement = this.#shadowRoot.getElementById(this.#styleId);
    if (styleElement) {
      if (priority === 'important') {
        const importantStyles = cssText.replace(/;/g, ' !important;');
        styleElement.textContent += importantStyles;
      } else {
        styleElement.textContent += cssText;
      }
      this.#injectedStyles.set(this.#styleId, cssText);
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    if (name === 'styles') {
      this.#injectStyles(newValue, this.getAttribute('priority'));
    }
  }

  injectCSS(cssText, id) {
    const styleId = id || `dynamic-style-${Date.now()}`;
    const existing = this.#shadowRoot.getElementById(styleId);

    if (existing) {
      existing.textContent = cssText;
    } else {
      const style = document.createElement('style');
      style.id = styleId;
      style.textContent = cssText;
      this.#shadowRoot.appendChild(style);
    }

    this.#injectedStyles.set(styleId, cssText);
    return styleId;
  }

  removeStyle(id) {
    const style = this.#shadowRoot.getElementById(id);
    if (style) {
      style.remove();
      this.#injectedStyles.delete(id);
    }
  }

  getInjectedStyles() {
    return Object.fromEntries(this.#injectedStyles);
  }
}

class InteractiveStyleModifier extends HTMLElement {
  #shadowRoot;
  #transitionProps;
  #animationQueue;

  static get observedAttributes() {
    return ['animate', 'duration', 'easing'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#transitionProps = new Map();
    this.#animationQueue = [];
  }

  connectedCallback() {
    this.#render();
    this.#initializeDefaultTransitions();
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <div class="modifier">
        <slot></slot>
      </div>
    `;
  }

  #initializeDefaultTransitions() {
    this.#transitionProps.set('opacity', '0.3s ease');
    this.#transitionProps.set('transform', '0.3s ease');
    this.#transitionProps.set('background-color', '0.2s ease');
    this.#transitionProps.set('color', '0.2s ease');
    this.#transitionProps.set('border-color', '0.2s ease');
    this.#transitionProps.set('box-shadow', '0.3s ease');
  }

  transitionTo(styles, duration = 300) {
    return new Promise((resolve) => {
      const element = this.#shadowRoot.querySelector('.modifier');
      if (!element) {
        resolve();
        return;
      }

      const keys = Object.keys(styles);
      const transitionValue = keys
        .map(key => `${this.#mapProperty(key)} ${duration}ms`)
        .join(', ');

      element.style.transition = transitionValue;

      for (const [property, value] of Object.entries(styles)) {
        element.style[this.#mapProperty(property)] = value;
      }

      setTimeout(() => {
        resolve();
      }, duration);
    });
  }

  #mapProperty(name) {
    const camelCase = name.replace(/-([a-z])/g, (_, char) => char.toUpperCase());
    return camelCase;
  }

  animateKeyframes(keyframes, options = {}) {
    const element = this.#shadowRoot.querySelector('.modifier');
    if (!element) return Promise.resolve();

    const animation = element.animate(keyframes, {
      duration: options.duration || 300,
      easing: options.easing || 'ease',
      fill: options.fill || 'forwards',
      iterations: options.iterations || 1,
      direction: options.direction || 'normal'
    });

    return animation.finished;
  }

  setTransition(property, duration, easing = 'ease') {
    this.#transitionProps.set(property, `${duration}ms ${easing}`);
    this.#updateTransitionStyles();
  }

  #updateTransitionStyles() {
    const element = this.#shadowRoot.querySelector('.modifier');
    if (!element) return;

    const transitions = Array.from(this.#transitionProps.entries())
      .map(([prop, timing]) => `${prop} ${timing}`)
      .join(', ');

    element.style.transition = transitions;
  }

  clearTransitions() {
    this.#transitionProps.clear();
    const element = this.#shadowRoot.querySelector('.modifier');
    if (element) {
      element.style.transition = 'none';
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
  }
}

class StyleCalculator extends HTMLElement {
  #shadowRoot;
  #computedStyles;

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#computedStyles = null;
  }

  connectedCallback() {
    this.#render();
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <div class="calculator">
        <slot></slot>
      </div>
    `;
  }

  calculateStyleDifference(element1, element2) {
    const style1 = getComputedStyle(element1);
    const style2 = getComputedStyle(element2);

    const differences = {};
    const properties = ['backgroundColor', 'color', 'fontSize', 'fontWeight', 'padding', 'margin', 'border'];

    for (const prop of properties) {
      const val1 = style1[prop];
      const val2 = style2[prop];

      if (val1 !== val2) {
        differences[prop] = { before: val1, after: val2 };
      }
    }

    return differences;
  }

  resolveCSSValue(property, value) {
    const element = this.#shadowRoot.querySelector('.calculator');
    const original = element.style[property];

    element.style[property] = value;
    const computed = getComputedStyle(element)[property];
    element.style[property] = original;

    return computed;
  }

  calculateLayoutMetrics() {
    const element = this.#shadowRoot.querySelector('.calculator');
    const rect = element.getBoundingClientRect();

    return {
      width: rect.width,
      height: rect.height,
      top: rect.top,
      left: rect.left,
      right: rect.right,
      bottom: rect.bottom,
      offsetWidth: element.offsetWidth,
      offsetHeight: element.offsetHeight,
      clientWidth: element.clientWidth,
      clientHeight: element.clientHeight,
      scrollWidth: element.scrollWidth,
      scrollHeight: element.scrollHeight
    };
  }
}

window.customElements.define('dynamic-stylist', DynamicStylist);
window.customElements.define('runtime-style-injector', RuntimeStyleInjector);
window.customElements.define('interactive-style-modifier', InteractiveStyleModifier);
window.customElements.define('style-calculator', StyleCalculator);

export { DynamicStylist, RuntimeStyleInjector, InteractiveStyleModifier, StyleCalculator };