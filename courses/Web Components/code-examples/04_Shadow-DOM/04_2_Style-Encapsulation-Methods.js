/**
 * Style-Encapsulation-Methods - Scoped styling techniques
 * @module 04_Shadow-DOM/Style-Encapsulation-Methods
 * @version 1.0.0
 * @example <style-encapsulation-methods></style-encapsulation-methods>
 */

class StyleEncapsulationMethods extends HTMLElement {
  /**
   * Creates an instance of StyleEncapsulationMethods.
   * @constructor
   */
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._config = {
      encapsulationMode: 'shadow',
      useCSSVariables: true,
      usePartAttribute: true,
      useThemeVars: true,
      prefix: 'sec',
      forceUniqueIds: true,
      enableAnimations: true,
    };
    this._theme = {
      light: {
        '--sec-bg': '#ffffff',
        '--sec-text': '#212121',
        '--sec-border': '#e0e0e0',
        '--sec-accent': '#6200ee',
        '--sec-secondary': '#03dac6',
        '--sec-error': '#b00020',
        '--sec-success': '#4caf50',
        '--sec-warning': '#ff9800',
        '--sec-info': '#2196f3',
        '--sec-surface': '#f5f5f5',
        '--sec-shadow': 'rgba(0, 0, 0, 0.15)',
      },
      dark: {
        '--sec-bg': '#121212',
        '--sec-text': '#ffffff',
        '--sec-border': '#333333',
        '--sec-accent': '#bb86fc',
        '--sec-secondary': '#03dac6',
        '--sec-error': '#cf6679',
        '--sec-success': '#81c784',
        '--sec-warning': '#ffb74d',
        '--sec-info': '#64b5f6',
        '--sec-surface': '#1e1e1e',
        '--sec-shadow': 'rgba(0, 0, 0, 0.5)',
      },
    };
    this._registeredStyles = new Map();
    this._activeStyleSheets = [];
    this._animationController = null;
    this._styleObserver = null;
  }

  /**
   * Lifecycle callback when the element is added to the DOM.
   * @method connectedCallback
   * @returns {void}
   */
  connectedCallback() {
    try {
      this._initializeStyles();
      this._registerBuiltInStyles();
      this._setupStyleObserver();
      this._injectDynamicStyles();
      this._render();
      this._dispatchEvent('styles-initialized');
    } catch (error) {
      this._handleError('connectedCallback', error);
    }
  }

  /**
   * Lifecycle callback when the element is removed from the DOM.
   * @method disconnectedCallback
   * @returns {void}
   */
  disconnectedCallback() {
    this._cleanupStyleObserver();
    this._cleanupRegisteredStyles();
    this._dispatchEvent('styles-cleaned');
  }

  /**
   * Lifecycle callback when an attribute changes.
   * @method observedAttributes
   * @returns {string[]} Array of observed attribute names.
   */
  static get observedAttributes() {
    return ['theme', 'mode', 'animation', 'compact'];
  }

  /**
   * Lifecycle callback when an attribute changes.
   * @method attributeChangedCallback
   * @param {string} name - The attribute name that changed.
   * @param {string} oldValue - The old value of the attribute.
   * @param {string} newValue - The new value of the attribute.
   * @returns {void}
   */
  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    this._handleAttributeChange(name, newValue);
  }

  /**
   * Initializes the base styling system.
   * @method _initializeStyles
   * @private
   * @returns {void}
   */
  _initializeStyles() {
    const baseStyles = this._createBaseStyles();
    this._injectStyleSheet(baseStyles, 'base');
  }

  /**
   * Creates the base styles for the component.
   * @method _createBaseStyles
   * @private
   * @returns {string} The base CSS.
   */
  _createBaseStyles() {
    const theme = this._getCurrentTheme();
    return `
      :host {
        display: block;
        ${this._generateCSSVariables(theme)}
      }

      :host([mode="flat"]) {
        --sec-radius: 0;
        --sec-shadow: none;
      }

      :host([mode="elevated"]) {
        --sec-radius: 12px;
        --sec-shadow: 0 8px 24px var(--sec-shadow);
      }

      :host([compact]) {
        --sec-padding: 8px;
        --sec-gap: 4px;
      }

      .style-wrapper {
        background: var(--sec-bg);
        color: var(--sec-text);
        border: 1px solid var(--sec-border);
        border-radius: var(--sec-radius, 8px);
        padding: var(--sec-padding, 16px);
        transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
      }

      .style-wrapper:hover {
        box-shadow: var(--sec-shadow);
        transform: translateY(-2px);
      }

      .section-title {
        font-size: 18px;
        font-weight: 600;
        margin: 0 0 12px 0;
        color: var(--sec-text);
      }

      .style-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
        gap: var(--sec-gap, 12px);
        margin: 16px 0;
      }

      .style-card {
        background: var(--sec-surface);
        border: 1px solid var(--sec-border);
        border-radius: calc(var(--sec-radius, 8px) - 4px);
        padding: 12px;
        transition: transform 0.2s ease, box-shadow 0.2s ease;
      }

      .style-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 4px 12px var(--sec-shadow);
      }

      .style-label {
        font-size: 12px;
        font-weight: 500;
        color: var(--sec-accent);
        text-transform: uppercase;
        letter-spacing: 0.5px;
        margin-bottom: 4px;
      }

      .style-value {
        font-size: 14px;
        color: var(--sec-text);
        font-family: 'Monaco', 'Consolas', monospace;
      }

      .demo-button {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        padding: 10px 20px;
        background: var(--sec-accent);
        color: white;
        border: none;
        border-radius: 4px;
        font-size: 14px;
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .demo-button:hover {
        filter: brightness(1.15);
        transform: translateY(-1px);
      }

      .demo-button:active {
        transform: translateY(0);
      }

      .demo-button.secondary {
        background: var(--sec-secondary);
        color: #000;
      }

      .demo-button.outline {
        background: transparent;
        border: 2px solid var(--sec-accent);
        color: var(--sec-accent);
      }

      .demo-button.danger {
        background: var(--sec-error);
      }

      .demo-button.success {
        background: var(--sec-success);
      }

      .demo-input {
        width: 100%;
        padding: 10px 14px;
        background: var(--sec-surface);
        border: 2px solid var(--sec-border);
        border-radius: 4px;
        color: var(--sec-text);
        font-size: 14px;
        transition: border-color 0.2s ease, box-shadow 0.2s ease;
        box-sizing: border-box;
      }

      .demo-input:focus {
        outline: none;
        border-color: var(--sec-accent);
        box-shadow: 0 0 0 3px rgba(98, 0, 238, 0.15);
      }

      .demo-input::placeholder {
        color: #999;
      }

      .chip-container {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
      }

      .chip {
        display: inline-flex;
        align-items: center;
        padding: 6px 12px;
        background: var(--sec-surface);
        border: 1px solid var(--sec-border);
        border-radius: 16px;
        font-size: 13px;
        color: var(--sec-text);
        transition: all 0.2s ease;
      }

      .chip:hover {
        background: var(--sec-accent);
        color: white;
        border-color: var(--sec-accent);
      }

      .chip.selected {
        background: var(--sec-accent);
        color: white;
        border-color: var(--sec-accent);
      }

      .divider {
        height: 1px;
        background: var(--sec-border);
        margin: 16px 0;
      }

      .progress-bar {
        width: 100%;
        height: 8px;
        background: var(--sec-surface);
        border-radius: 4px;
        overflow: hidden;
      }

      .progress-fill {
        height: 100%;
        background: linear-gradient(90deg, var(--sec-accent), var(--sec-secondary));
        border-radius: 4px;
        transition: width 0.3s ease;
      }

      .tooltip {
        position: relative;
        display: inline-block;
      }

      .tooltip-text {
        visibility: hidden;
        position: absolute;
        bottom: 125%;
        left: 50%;
        transform: translateX(-50%);
        padding: 8px 12px;
        background: var(--sec-text);
        color: var(--sec-bg);
        font-size: 12px;
        border-radius: 4px;
        white-space: nowrap;
        opacity: 0;
        transition: opacity 0.2s ease;
        z-index: 100;
      }

      .tooltip:hover .tooltip-text {
        visibility: visible;
        opacity: 1;
      }

      .badge {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-width: 20px;
        height: 20px;
        padding: 0 6px;
        background: var(--sec-error);
        color: white;
        font-size: 11px;
        font-weight: 600;
        border-radius: 10px;
      }

      .tab-container {
        display: flex;
        border-bottom: 2px solid var(--sec-border);
      }

      .tab {
        padding: 12px 20px;
        background: transparent;
        border: none;
        color: var(--sec-text);
        font-size: 14px;
        font-weight: 500;
        cursor: pointer;
        position: relative;
        transition: color 0.2s ease;
      }

      .tab::after {
        content: '';
        position: absolute;
        bottom: -2px;
        left: 0;
        width: 100%;
        height: 2px;
        background: var(--sec-accent);
        transform: scaleX(0);
        transition: transform 0.2s ease;
      }

      .tab:hover {
        color: var(--sec-accent);
      }

      .tab.active {
        color: var(--sec-accent);
      }

      .tab.active::after {
        transform: scaleX(1);
      }
    `;
  }

  /**
   * Registers built-in style components.
   * @method _registerBuiltInStyles
   * @private
   * @returns {void}
   */
  _registerBuiltInStyles() {
    const styleModules = [
      { name: 'elevation', styles: this._createElevationStyles() },
      { name: 'typography', styles: this._createTypographyStyles() },
      { name: 'layout', styles: this._createLayoutStyles() },
      { name: 'animation', styles: this._createAnimationStyles() },
      { name: 'responsive', styles: this._createResponsiveStyles() },
    ];

    styleModules.forEach(({ name, styles }) => {
      this._registeredStyles.set(name, styles);
    });
  }

  /**
   * Creates elevation shadow styles.
   * @method _createElevationStyles
   * @private
   * @returns {string} The elevation CSS.
   */
  _createElevationStyles() {
    return `
      .elevation-0 { box-shadow: none; }
      .elevation-1 { box-shadow: 0 1px 3px var(--sec-shadow); }
      .elevation-2 { box-shadow: 0 3px 6px var(--sec-shadow); }
      .elevation-3 { box-shadow: 0 6px 12px var(--sec-shadow); }
      .elevation-4 { box-shadow: 0 10px 20px var(--sec-shadow); }
      .elevation-5 { box-shadow: 0 15px 30px var(--sec-shadow); }
    `;
  }

  /**
   * Creates typography styles.
   * @method _createTypographyStyles
   * @private
   * @returns {string} The typography CSS.
   */
  _createTypographyStyles() {
    return `
      .text-h1 { font-size: 32px; font-weight: 700; line-height: 1.2; }
      .text-h2 { font-size: 24px; font-weight: 600; line-height: 1.3; }
      .text-h3 { font-size: 20px; font-weight: 600; line-height: 1.4; }
      .text-body { font-size: 16px; font-weight: 400; line-height: 1.5; }
      .text-caption { font-size: 12px; font-weight: 400; line-height: 1.4; }
      .text-button { font-size: 14px; font-weight: 500; letter-spacing: 0.5px; }
    `;
  }

  /**
   * Creates layout styles.
   * @method _createLayoutStyles
   * @private
   * @returns {string} The layout CSS.
   */
  _createLayoutStyles() {
    return `
      .flex { display: flex; }
      .flex-col { flex-direction: column; }
      .flex-wrap { flex-wrap: wrap; }
      .items-center { align-items: center; }
      .items-start { align-items: flex-start; }
      .items-end { align-items: flex-end; }
      .justify-center { justify-content: center; }
      .justify-between { justify-content: space-between; }
      .justify-around { justify-content: space-around; }
      .gap-1 { gap: 4px; }
      .gap-2 { gap: 8px; }
      .gap-3 { gap: 12px; }
      .gap-4 { gap: 16px; }
    `;
  }

  /**
   * Creates animation styles.
   * @method _createAnimationStyles
   * @private
   * @returns {string} The animation CSS.
   */
  _createAnimationStyles() {
    return `
      @keyframes fadeIn {
        from { opacity: 0; }
        to { opacity: 1; }
      }

      @keyframes slideUp {
        from { transform: translateY(20px); opacity: 0; }
        to { transform: translateY(0); opacity: 1; }
      }

      @keyframes slideDown {
        from { transform: translateY(-20px); opacity: 0; }
        to { transform: translateY(0); opacity: 1; }
      }

      @keyframes scaleIn {
        from { transform: scale(0.9); opacity: 0; }
        to { transform: scale(1); opacity: 1; }
      }

      @keyframes rotate {
        from { transform: rotate(0deg); }
        to { transform: rotate(360deg); }
      }

      .animate-fadeIn { animation: fadeIn 0.3s ease forwards; }
      .animate-slideUp { animation: slideUp 0.3s ease forwards; }
      .animate-slideDown { animation: slideDown 0.3s ease forwards; }
      .animate-scaleIn { animation: scaleIn 0.3s ease forwards; }
      .animate-spin { animation: rotate 1s linear infinite; }
    `;
  }

  /**
   * Creates responsive styles.
   * @method _createResponsiveStyles
   * @private
   * @returns {string} The responsive CSS.
   */
  _createResponsiveStyles() {
    return `
      @media (max-width: 600px) {
        .hide-mobile { display: none !important; }
        .full-width-mobile { width: 100% !important; }
      }

      @media (min-width: 601px) and (max-width: 960px) {
        .hide-tablet { display: none !important; }
      }

      @media (min-width: 961px) {
        .hide-desktop { display: none !important; }
      }
    `;
  }

  /**
   * Injects dynamic style generation.
   * @method _injectDynamicStyles
   * @private
   * @returns {void}
   */
  _injectDynamicStyles() {
    const dynamicStyles = this._createDynamicStyles();
    this._injectStyleSheet(dynamicStyles, 'dynamic');
  }

  /**
   * Creates dynamic styles based on configuration.
   * @method _createDynamicStyles
   * @private
   * @returns {string} The dynamic CSS.
   */
  _createDynamicStyles() {
    const prefix = this._config.prefix;
    return `
      [${prefix}-hidden] {
        display: none !important;
        visibility: hidden;
        opacity: 0;
      }

      [${prefix}-visible] {
        visibility: visible;
        opacity: 1;
      }

      [${prefix}-loading] {
        position: relative;
        pointer-events: none;
      }

      [${prefix}-loading]::after {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(255, 255, 255, 0.7);
        z-index: 10;
      }
    `;
  }

  /**
   * Generates CSS variables from theme.
   * @method _generateCSSVariables
   * @private
   * @param {Object} theme - The theme object.
   * @returns {string} CSS variables string.
   */
  _generateCSSVariables(theme) {
    if (!this._config.useCSSVariables) return '';
    
    return Object.entries(theme)
      .map(([key, value]) => `${key}: ${value};`)
      .join('\n');
  }

  /**
   * Gets the current theme configuration.
   * @method _getCurrentTheme
   * @private
   * @returns {Object} The current theme.
   */
  _getCurrentTheme() {
    const themeAttr = this.getAttribute('theme');
    return themeAttr === 'dark' ? this._theme.dark : this._theme.light;
  }

  /**
   * Injects a text/CSS as a style sheet.
   * @method _injectStyleSheet
   * @private
   * @param {string} css - The CSS text.
   * @param {string} id - The style sheet identifier.
   * @returns {CSSStyleSheet} The created style sheet.
   */
  _injectStyleSheet(css, id) {
    const style = document.createElement('style');
    style.setAttribute('data-style-id', id);
    style.textContent = css;
    this.shadowRoot.appendChild(style);
    
    if (style.sheet) {
      this._activeStyleSheets.push({ id, sheet: style.sheet });
    }
    
    return style.sheet;
  }

  /**
   * Sets up style observer for external style changes.
   * @method _setupStyleObserver
   * @private
   * @returns {void}
   */
  _setupStyleObserver() {
    if (!this._config.usePartAttribute) return;
    
    this._styleObserver = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.type === 'attributes' && mutation.attributeName === 'part') {
          this._handlePartChange();
        }
      });
    });

    this._styleObserver.observe(this, { attributes: true, attributeFilter: ['part'] });
  }

  /**
   * Handles part attribute changes.
   * @method _handlePartChange
   * @private
   * @returns {void}
   */
  _handlePartChange() {
    const part = this.getAttribute('part');
    if (part) {
      this._dispatchEvent('part-changed', { part });
    }
  }

  /**
   * Cleans up style observer.
   * @method _cleanupStyleObserver
   * @private
   * @returns {void}
   */
  _cleanupStyleObserver() {
    if (this._styleObserver) {
      this._styleObserver.disconnect();
      this._styleObserver = null;
    }
  }

  /**
   * Cleans up registered styles.
   * @method _cleanupRegisteredStyles
   * @private
   * @returns {void}
   */
  _cleanupRegisteredStyles() {
    this._registeredStyles.clear();
    this._activeStyleSheets = [];
  }

  /**
   * Handles attribute changes.
   * @method _handleAttributeChange
   * @private
   * @param {string} name - The attribute name.
   * @param {string} value - The new value.
   * @returns {void}
   */
  _handleAttributeChange(name, value) {
    switch (name) {
      case 'theme':
        this._updateTheme(value);
        break;
      case 'animation':
        this._updateAnimationState(value === 'true');
        break;
    }
  }

  /**
   * Updates the current theme.
   * @method _updateTheme
   * @private
   * @param {string} themeName - The theme name.
   * @returns {void}
   */
  _updateTheme(themeName) {
    const newTheme = this._theme[themeName] || this._theme.light;
    const cssVars = this._generateCSSVariables(newTheme);
    this.style.cssText += cssVars;
  }

  /**
   * Updates animation enabled state.
   * @method _updateAnimationState
   * @private
   * @param {boolean} enabled - Whether animations are enabled.
   * @returns {void}
   */
  _updateAnimationState(enabled) {
    this._config.enableAnimations = enabled;
  }

  /**
   * Renders the component.
   * @method _render
   * @private
   * @returns {void}
   */
  _render() {
    const template = document.createElement('template');
    template.innerHTML = `
      <div class="style-wrapper">
        <h2 class="section-title">Style Encapsulation Methods</h2>
        
        <div class="style-grid">
          <div class="style-card">
            <div class="style-label">Encapsulation</div>
            <div class="style-value">Shadow DOM</div>
          </div>
          <div class="style-card">
            <div class="style-label">CSS Variables</div>
            <div class="style-value">${this._config.useCSSVariables ? 'Enabled' : 'Disabled'}</div>
          </div>
          <div class="style-card">
            <div class="style-label">Part Attribute</div>
            <div class="style-value">${this._config.usePartAttribute ? 'Enabled' : 'Disabled'}</div>
          </div>
          <div class="style-card">
            <div class="style-label">Animations</div>
            <div class="style-value">${this._config.enableAnimations ? 'Enabled' : 'Disabled'}</div>
          </div>
        </div>

        <div class="divider"></div>

        <h3 style="font-size: 14px; margin: 12px 0;">Button Variants</h3>
        <div style="display: flex; gap: 8px; flex-wrap: wrap; margin-bottom: 16px;">
          <button class="demo-button">Primary</button>
          <button class="demo-button secondary">Secondary</button>
          <button class="demo-button outline">Outline</button>
          <button class="demo-button danger">Danger</button>
          <button class="demo-button success">Success</button>
        </div>

        <div class="divider"></div>

        <h3 style="font-size: 14px; margin: 12px 0;">Input Field</h3>
        <input type="text" class="demo-input" placeholder="Enter text..." />

        <div class="divider"></div>

        <h3 style="font-size: 14px; margin: 12px 0;">Chips</h3>
        <div class="chip-container">
          <span class="chip">JavaScript</span>
          <span class="chip selected">Shadow DOM</span>
          <span class="chip">Web Components</span>
          <span class="chip">Custom Elements</span>
        </div>

        <div class="divider"></div>

        <h3 style="font-size: 14px; margin: 12px 0;">Progress Bar</h3>
        <div class="progress-bar">
          <div class="progress-fill" style="width: 65%;"></div>
        </div>

        <div class="divider"></div>

        <h3 style="font-size: 14px; margin: 12px 0;">Tabs</h3>
        <div class="tab-container">
          <button class="tab active">Style 1</button>
          <button class="tab">Style 2</button>
          <button class="tab">Style 3</button>
        </div>
      </div>
    `;

    this.shadowRoot.appendChild(template.content.cloneNode(true));
  }

  /**
   * Dispatches a custom event.
   * @method _dispatchEvent
   * @private
   * @param {string} eventName - The event name.
   * @param {Object} detail - The event detail.
   * @returns {void}
   */
  _dispatchEvent(eventName, detail = {}) {
    this.dispatchEvent(
      new CustomEvent(eventName, {
        bubbles: true,
        composed: true,
        detail,
      })
    );
  }

  /**
   * Handles errors.
   * @method _handleError
   * @private
   * @param {string} source - The error source.
   * @param {Error} error - The error.
   * @returns {void}
   */
  _handleError(source, error) {
    console.error(`[StyleEncapsulationMethods] ${source}:`, error);
    this._dispatchEvent('error', { source, message: error.message });
  }

  /**
   * Gets a registered style by name.
   * @method getStyle
   * @public
   * @param {string} name - The style name.
   * @returns {string|null} The style CSS.
   */
  getStyle(name) {
    return this._registeredStyles.get(name) || null;
  }

  /**
   * Registers a new style.
   * @method registerStyle
   * @public
   * @param {string} name - The style name.
   * @param {string} css - The CSS.
   * @returns {void}
   */
  registerStyle(name, css) {
    this._registeredStyles.set(name, css);
    this._injectStyleSheet(css, name);
  }

  /**
   * Applies a theme.
   * @method applyTheme
   * @public
   * @param {string} themeName - The theme name.
   * @returns {void}
   */
  applyTheme(themeName) {
    this.setAttribute('theme', themeName);
  }
}

customElements.define('style-encapsulation-methods', StyleEncapsulationMethods);

export { StyleEncapsulationMethods };