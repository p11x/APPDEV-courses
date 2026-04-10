/**
 * Browser Compatibility Matrix - Polyfill Implementations
 * @description Feature detection and polyfill loading for cross-browser support
 * @module basics/browser-compatibility
 * @version 1.0.0
 */

// ============================================
// Feature Detection Utility
// ============================================

/**
 * WebComponentFeatures - Feature detection for Web Components
 */
class WebComponentFeatures {
  /**
   * Check if Custom Elements v1 are supported
   * @static
   * @returns {boolean}
   */
  static get customElements() {
    return (
      window.customElements &&
      typeof window.customElements.define === 'function' &&
      typeof window.customElements.get === 'function'
    );
  }

  /**
   * Check if Shadow DOM v1 is supported
   * @static
   * @returns {boolean}
   */
  static get shadowDOM() {
    const div = document.createElement('div');
    return (
      div.attachShadow &&
      typeof div.attachShadow === 'function'
    );
  }

  /**
   * Check if HTML Templates are supported
   * @static
   * @returns {boolean}
   */
  static get templates() {
    const template = document.createElement('template');
    return (
      template.content &&
      template.content.cloneNode &&
      typeof template.content.cloneNode === 'function'
    );
  }

  /**
   * Check if Constructable Stylesheets are supported
   * @static
   * @returns {boolean}
   */
  static get constructableSheets() {
    try {
      new CSSStyleSheet();
      return true;
    } catch (e) {
      return false;
    }
  }

  /**
   * Check if adoptedStyleSheets are supported
   * @static
   * @returns {boolean}
   */
  static get adoptedStyleSheets() {
    const div = document.createElement('div');
    return Array.isArray(div.shadowRoot?.adoptedStyleSheets);
  }

  /**
   * Check if formAssociated is supported
   * @static
   * @returns {boolean}
   */
  static get formAssociated() {
    return 'ElementInternals' in window;
  }

  /**
   * Get all feature support status
   * @static
   * @returns {Object}
   */
  static getAll() {
    return {
      customElements: this.customElements,
      shadowDOM: this.shadowDOM,
      templates: this.templates,
      constructableSheets: this.constructableSheets,
      adoptedStyleSheets: this.adoptedStyleSheets,
      formAssociated: this.formAssociated
    };
  }

  /**
   * Check if polyfills are needed
   * @static
   * @returns {boolean}
   */
  static needsPolyfills() {
    return !this.customElements || !this.shadowDOM || !this.templates;
  }
}

// ============================================
// Polyfill Loader
// ============================================

/**
 * PolyfillLoader - Dynamic polyfill loading
 */
class PolyfillLoader {
  constructor() {
    this.loaded = false;
    this.loading = false;
    this.loadPromise = null;
  }

  /**
   * Load required polyfills
   * @async
   * @returns {Promise<void>}
   */
  async load() {
    if (this.loaded) return;
    if (this.loading) return this.loadPromise;

    this.loading = true;

    try {
      const features = WebComponentFeatures.getAll();
      const loadPromises = [];

      if (!features.customElements) {
        console.log('[PolyfillLoader] Loading Custom Elements polyfill...');
        loadPromises.push(this._loadScript(
          'https://unpkg.com/@webcomponents/custom-elements@1.5.1/custom-elements.min.js'
        ));
      }

      if (!features.shadowDOM) {
        console.log('[PolyfillLoader] Loading Shadow DOM polyfill...');
        loadPromises.push(this._loadScript(
          'https://unpkg.com/@webcomponents/shadydom@1.11.0/shadydom.min.js'
        ));
      }

      if (!features.templates) {
        console.log('[PolyfillLoader] Loading Template polyfill...');
        loadPromises.push(this._loadScript(
          'https://unpkg.com/@webcomponents/template@1.2.0/template.min.js'
        ));
      }

      await Promise.all(loadPromises);
      this.loaded = true;
      console.log('[PolyfillLoader] All polyfills loaded successfully');
    } catch (error) {
      console.error('[PolyfillLoader] Failed to load polyfills:', error);
      throw error;
    } finally {
      this.loading = false;
    }
  }

  /**
   * Load a script dynamically
   * @private
   * @param {string} src - Script URL
   * @returns {Promise}
   */
  _loadScript(src) {
    return new Promise((resolve, reject) => {
      const script = document.createElement('script');
      script.src = src;
      script.onload = resolve;
      script.onerror = reject;
      document.head.appendChild(script);
    });
  }

  /**
   * Load polyfills only when needed
   * @static
   * @async
   * @returns {Promise<void>}
   */
  static async ensurePolyfills() {
    if (!WebComponentFeatures.needsPolyfills()) {
      console.log('[PolyfillLoader] No polyfills needed');
      return;
    }

    const loader = new PolyfillLoader();
    await loader.load();
  }
}

// ============================================
// Feature Detection Decorator
// ============================================

/**
 * Feature-aware component decorator
 * @param {HTMLElement} BaseClass - Base component class
 * @returns {HTMLElement} - Enhanced component with feature detection
 */
function FeatureAwareComponent(BaseClass) {
  return class extends BaseClass {
    constructor() {
      super();
      
      // Check feature support
      this._features = WebComponentFeatures.getAll();
      
      // Provide fallbacks for unsupported features
      if (!this._features.shadowDOM) {
        this._createFallbackShadow();
      }
    }

    _createFallbackShadow() {
      // Fallback for browsers without Shadow DOM
      Object.defineProperty(this, 'shadowRoot', {
        get: () => this._fallbackRoot || (this._fallbackRoot = this.createShadowRoot())
      });
    }

    hasFeature(feature) {
      return this._features[feature] || false;
    }
  };
}

// ============================================
// Export and Usage
// ============================================

export { WebComponentFeatures, PolyfillLoader, FeatureAwareComponent };

// Auto-load polyfills on import
if (typeof window !== 'undefined') {
  PolyfillLoader.ensurePolyfills().catch(console.error);
}

/*
  Usage:
  
  // Check feature support
  const features = WebComponentFeatures.getAll();
  console.log('Custom Elements:', features.customElements);
  console.log('Shadow DOM:', features.shadowDOM);
  
  // Manually load polyfills
  const loader = new PolyfillLoader();
  await loader.load();
  
  // Use feature-aware component
  @FeatureAwareComponent
  class MyComponent extends HTMLElement { }
*/