/**
 * Performance Optimized Styling - Demonstrates CSS optimization techniques
 * including critical CSS inlining, lazy loading styles, style batching, and paint optimization
 * @module styling/06_5_Performance-Optimized-Styling
 * @version 1.0.0
 * @example <style-optimized-card></style-optimized-card>
 */

const CRITICAL_CSS_CACHE = new Map();
const STYLE_BATCH = [];
let batchRafael = null;

class StyleOptimizedCard extends HTMLElement {
  #shadowRoot;
  #criticalStyles;
  #lazyStyles;
  #styleObserver;
  #paintMetrics;
  #renderedAt;

  static get observedAttributes() {
    return ['optimize', 'viewport', 'priority'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#criticalStyles = '';
    this.#lazyStyles = '';
    this.#styleObserver = null;
    this.#paintMetrics = null;
    this.#renderedAt = 0;
  }

  #initializeCriticalStyles() {
    this.#criticalStyles = `
      :host {
        display: block;
        contain: content layout style;
        --card-bg: #ffffff;
        --card-color: #333333;
        --card-radius: 8px;
        --card-padding: 16px;
        --card-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      }
      .card {
        background: var(--card-bg);
        border-radius: var(--card-radius);
        box-shadow: var(--card-shadow);
        color: var(--card-color);
        padding: var(--card-padding);
        will-change: transform;
      }
      .title {
        font-size: 1.25rem;
        font-weight: 600;
        margin: 0 0 8px 0;
      }
      .content {
        font-size: 1rem;
        line-height: 1.5;
      }
    `;
  }

  #initializeLazyStyles() {
    this.#lazyStyles = `
      .card:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
      }
      .card.animating {
        transition: transform 0.3s ease, box-shadow 0.3s ease;
      }
      .highlight {
        background: linear-gradient(120deg, transparent 0%, transparent 100%);
        background-size: 200% 100%;
        background-position: 100% 0;
        transition: background-position 0.5s ease;
      }
      .highlight.visible {
        background-position: 0 0;
      }
      @media (prefers-reduced-motion: reduce) {
        .card, .highlight {
          transition: none;
        }
      }
    `;
  }

  connectedCallback() {
    this.#initializeCriticalStyles();
    this.#renderCriticalStyles();
    this.#setupLazyLoading();
    this.#setupIntersectionObserver();
    this.#setupPerformanceMetrics();

    this.#renderedAt = performance.now();
  }

  #renderCriticalStyles() {
    const styleElement = document.createElement('style');
    styleElement.textContent = this.#criticalStyles;
    this.#shadowRoot.appendChild(styleElement);

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

  #setupLazyLoading() {
    if (this.hasAttribute('optimize')) {
      const loadLazy = () => this.#loadLazyStyles();
      if (typeof requestIdleCallback === 'function') {
        requestIdleCallback(loadLazy, { timeout: 2000 });
      } else {
        setTimeout(loadLazy, 100);
      }
    }
  }

  #loadLazyStyles() {
    if (!this.#lazyStyles) return;

    const styleElement = document.createElement('style');
    styleElement.textContent = this.#lazyStyles;
    styleElement.media = 'screen';
    this.#shadowRoot.appendChild(styleElement);
  }

  #setupIntersectionObserver() {
    if (!('IntersectionObserver' in window)) return;

    const observer = new IntersectionObserver(
      (entries) => {
        for (const entry of entries) {
          if (entry.isIntersecting) {
            this.#onVisibilityChange(true);
            observer.disconnect();
          }
        }
      },
      { threshold: 0.1, rootMargin: '50px' }
    );

    observer.observe(this);
  }

  #onVisibilityChange(isVisible) {
    if (isVisible && this.hasAttribute('viewport')) {
      this.#loadLazyStyles();
    }
  }

  #setupPerformanceMetrics() {
    if (!this.hasAttribute('optimize')) return;

    this.#paintMetrics = {
      firstPaint: 0,
      firstContentfulPaint: 0,
      largestContentfulPaint: 0
    };

    if (performance.mark) {
      performance.mark('card-rendered');
    }
  }

  getPerformanceMetrics() {
    if (!this.#paintMetrics) return null;

    return {
      ...this.#paintMetrics,
      renderTime: performance.now() - this.#renderedAt,
      paintCount: this.#getPaintCount()
    };
  }

  #getPaintCount() {
    return this.#paintMetrics?.paintCount || 0;
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
  }
}

class StyleBatcher extends HTMLElement {
  #shadowRoot;
  #batchQueue;
  #batchTimeout;
  #batchSize;

  static get observedAttributes() {
    return ['batch-size', 'debounce'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#batchQueue = new Set();
    this.#batchTimeout = null;
    this.#batchSize = 10;
  }

  connectedCallback() {
    this.#batchSize = parseInt(this.getAttribute('batch-size')) || 10;
    this.#render();
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <div class="batcher">
        <slot></slot>
      </div>
    `;
  }

  batchStyleUpdate(element, property, value) {
    this.#batchQueue.add({ element, property, value });

    if (this.#batchQueue.size >= this.#batchSize) {
      this.#processBatch();
    } else if (!this.#batchTimeout) {
      const debounce = parseInt(this.getAttribute('debounce')) || 16;
      this.#batchTimeout = setTimeout(() => this.#processBatch(), debounce);
    }
  }

  #processBatch() {
    if (this.#batchTimeout) {
      clearTimeout(this.#batchTimeout);
      this.#batchTimeout = null;
    }

    const startTime = performance.now();

    this.#batchQueue.forEach(({ element, property, value }) => {
      element.style[property] = value;
    });

    this.#batchQueue.clear();

    const endTime = performance.now();
    console.log(`Batch processed in ${(endTime - startTime).toFixed(2)}ms`);
  }
}

class CSSContainmentManager extends HTMLElement {
  #shadowRoot;
  #containmentLevel;

  static get observedAttributes() {
    return ['contain'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#containmentLevel = 'layout';
  }

  connectedCallback() {
    this.#containmentLevel = this.getAttribute('contain') || 'layout';
    this.#applyContainment();
    this.#render();
  }

  #applyContainment() {
    const validLevels = ['none', 'strict', 'layout', 'style', 'paint'];
    if (!validLevels.includes(this.#containmentLevel)) {
      this.#containmentLevel = 'layout';
    }
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <div class="contained" style="contain: ${this.#containmentLevel}">
        <slot></slot>
      </div>
    `;
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    if (name === 'contain') {
      this.#containmentLevel = newValue;
      this.#applyContainment();
    }
  }
}

class WillChangeOptimizer extends HTMLElement {
  #shadowRoot;
  #willChangeProps;

  static get observedAttributes() {
    return ['animate'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#willChangeProps = new Set();
  }

  connectedCallback() {
    this.#render();
    this.#setupWillChange();
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <div class="optimizer">
        <slot></slot>
      </div>
    `;
  }

  #setupWillChange() {
    const animate = this.getAttribute('animate');
    if (animate) {
      const props = animate.split(',').map(p => p.trim());
      props.forEach(prop => this.#willChangeProps.add(prop));
      this.#applyWillChange();
    }
  }

  #applyWillChange() {
    const element = this.#shadowRoot.querySelector('.optimizer');
    if (element) {
      element.style.willChange = Array.from(this.#willChangeProps).join(', ');
    }
  }

  optimizeProperty(prop) {
    this.#willChangeProps.add(prop);
    this.#applyWillChange();
  }

  clearOptimization() {
    this.#willChangeProps.clear();
    const element = this.#shadowRoot.querySelector('.optimizer');
    if (element) {
      element.style.willChange = 'auto';
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
  }
}

class PaintWorkletManager {
  #workletModule;
  #paintName;

  static async registerPaint(name, workletClass) {
    if (!('paintWorklet' in CSS)) {
      console.warn('Paint Worklet not supported');
      return false;
    }

    try {
      if (name.startsWith('paint-')) {
        name = name.substring(6);
      }

      CSS.paintWorklet.addModule(workletClass);
      return true;
    } catch (error) {
      console.error('Failed to register paint worklet:', error);
      return false;
    }
  }

  static registerGradientPaint(name) {
    const paintClass = class {
      static get inputProperties() {
        return ['--gradient-start', '--gradient-end', '--gradient-direction'];
      }

      static get outputProperties() {
        return ['background-image'];
      }

      paint(ctx, geom, props) {
        const start = props.get('--gradient-start')?.toString().trim() || '#007bff';
        const end = props.get('--gradient-end')?.toString().trim() || '#00d4ff';
        const direction = props.get('--gradient-direction')?.toString().trim() || '90deg';

        const angle = parseInt(direction) * (Math.PI / 180);
        const x1 = 0.5 + Math.cos(angle) * 0.5;
        const y1 = 0.5 + Math.sin(angle) * 0.5;

        const gradient = ctx.createLinearGradient(0, 0, geom.width, geom.height);
        gradient.addColorStop(0, start);
        gradient.addColorStop(1, end);

        ctx.fillStyle = gradient;
        ctx.fillRect(0, 0, geom.width, geom.height);
      }
    };

    return this.registerPaint(name, paintClass);
  }
}

class StyleMetricCollector extends HTMLElement {
  #shadowRoot;
  #metrics;

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#metrics = {
      styleRecalcs: 0,
      styleChanges: 0,
      paints: 0,
      layouts: 0
    };
  }

  connectedCallback() {
    this.#render();
    this.#setupMeasurement();
  }

  #render() {
    this.#shadowRoot.innerHTML = `
      <div class="metrics">
        <slot></slot>
      </div>
    `;
  }

  #setupMeasurement() {
    if (!this.hasAttribute('measure')) return;

    requestAnimationFrame(() => {
      this.#collectMetrics();
    });
  }

  #collectMetrics() {
    if (performance.getEntriesByType) {
      const paints = performance.getEntriesByType('paint');
      this.#metrics.paints = paints.length;
    }

    const element = this.#shadowRoot.querySelector('.metrics');
    if (element) {
      const computed = getComputedStyle(element);
      this.#metrics.styleRecalcs++;
    }
  }

  getMetrics() {
    return { ...this.#metrics };
  }

  resetMetrics() {
    this.#metrics = {
      styleRecalcs: 0,
      styleChanges: 0,
      paints: 0,
      layouts: 0
    };
  }
}

window.customElements.define('style-optimized-card', StyleOptimizedCard);
window.customElements.define('style-batcher', StyleBatcher);
window.customElements.define('css-containment-manager', CSSContainmentManager);
window.customElements.define('will-change-optimizer', WillChangeOptimizer);
window.customElements.define('style-metric-collector', StyleMetricCollector);

export { StyleOptimizedCard, StyleBatcher, CSSContainmentManager, WillChangeOptimizer, PaintWorkletManager, StyleMetricCollector };