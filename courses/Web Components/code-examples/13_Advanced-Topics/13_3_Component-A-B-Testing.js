/**
 * Component A/B Testing - A/B and Multivariate Testing Components
 * @description Implement A/B testing, split testing, and multivariate testing for Web Components
 * @module advanced/ab-testing
 * @version 1.0.0
 */

(function() {
  'use strict';

  const STORAGE_KEY = 'ab_test_experiments';
  const AnalyticsEvents = {
    EXPOSURE: 'ab_test_exposure',
    CONVERSION: 'ab_test_conversion'
  };

  const DEFAULT_CONFIG = {
    autoTrack: true,
    persistSelection: true,
    storageType: 'localStorage',
    trackingEndpoint: null,
    defaultVariant: 'control'
  };

  class ABTestTracker {
    constructor(config = {}) {
      this.config = { ...DEFAULT_CONFIG, ...config };
      this.experiments = new Map();
      this.conversions = new Map();
      this.storage = this.createStorage();
      this.loadPersistedData();
    }

    createStorage() {
      if (this.config.storageType === 'sessionStorage') {
        return sessionStorage;
      }
      return localStorage;
    }

    loadPersistedData() {
      try {
        const data = this.storage.getItem(STORAGE_KEY);
        if (data) {
          const parsed = JSON.parse(data);
          this.experiments = new Map(parsed.experiments || []);
          this.conversions = new Map(parsed.conversions || []);
        }
      } catch (e) {
        console.warn('Failed to load A/B test data:', e);
      }
    }

    persistData() {
      try {
        const data = {
          experiments: Array.from(this.experiments.entries()),
          conversions: Array.from(this.conversions.entries())
        };
        this.storage.setItem(STORAGE_KEY, JSON.stringify(data));
      } catch (e) {
        console.warn('Failed to persist A/B test data:', e);
      }
    }

    createExperiment(name, variants, options = {}) {
      const experiment = {
        name,
        variants: this.normalizeVariants(variants),
        trafficAllocation: options.trafficAllocation || 100,
        trackingEndpoint: options.trackingEndpoint || this.config.trackingEndpoint,
        persistSelection: options.persistSelection !== undefined 
          ? options.persistSelection 
          : this.config.persistSelection,
        autoTrack: options.autoTrack !== undefined 
          ? options.autoTrack 
          : this.config.autoTrack,
        metadata: options.metadata || {},
        startDate: options.startDate || new Date(),
        endDate: options.endDate || null,
        status: options.status || 'running'
      };

      return experiment;
    }

    normalizeVariants(variants) {
      if (Array.isArray(variants)) {
        return variants.map((variant, index) => ({
          id: variant.id || `variant_${index}`,
          name: variant.name || variant.id || `Variant ${index + 1}`,
          weight: variant.weight || 1,
          config: variant.config || {}
        }));
      }

      if (typeof variants === 'object') {
        return Object.entries(variants).map(([id, config], index) => ({
          id,
          name: config.name || id,
          weight: config.weight || 1,
          config: config.config || {}
        }));
      }

      return [{ id: 'control', name: 'Control', weight: 1, config: {} }];
    }

    selectVariant(experiment) {
      if (!this.config.persistSelection) {
        return this.randomSelection(experiment);
      }

      const storageKey = `ab_test_${experiment.name}`;
      const storedVariant = this.storage.getItem(storageKey);

      if (storedVariant && this.validateVariant(experiment, storedVariant)) {
        return this.findVariant(experiment, storedVariant);
      }

      const selected = this.randomSelection(experiment);
      this.storage.setItem(storageKey, selected.id);

      return selected;
    }

    randomSelection(experiment) {
      const totalWeight = experiment.variants.reduce((sum, v) => sum + v.weight, 0);
      let random = Math.random() * totalWeight;

      for (const variant of experiment.variants) {
        random -= variant.weight;
        if (random <= 0) {
          return variant;
        }
      }

      return experiment.variants[0];
    }

    findVariant(experiment, variantId) {
      return experiment.variants.find(v => v.id === variantId);
    }

    validateVariant(experiment, variantId) {
      return experiment.variants.some(v => v.id === variantId);
    }

    enroll(experiment, options = {}) {
      if (experiment.endDate && new Date() > experiment.endDate) {
        experiment.status = 'ended';
        return this.findVariant(experiment, this.config.defaultVariant);
      }

      const variant = this.selectVariant(experiment);
      this.experiments.set(experiment.name, variant.id);

      if (experiment.autoTrack) {
        this.trackExposure(experiment.name, variant.id, experiment.metadata);
      }

      return variant;
    }

    trackExposure(experimentName, variantId, metadata = {}) {
      const event = {
        type: AnalyticsEvents.EXPOSURE,
        experimentName,
        variantId,
        timestamp: new Date().toISOString(),
        metadata
      };

      this.sendToAnalytics(event);

      const exposures = this.conversions.get(AnalyticsEvents.EXPOSURE) || [];
      exposures.push(event);
      this.conversions.set(AnalyticsEvents.EXPOSURE, exposures);
      this.persistData();
    }

    trackConversion(experimentName, conversionName, value = 1, metadata = {}) {
      const variantId = this.experiments.get(experimentName);
      if (!variantId) {
        console.warn(`No enrollment found for experiment: ${experimentName}`);
        return;
      }

      const event = {
        type: AnalyticsEvents.CONVERSION,
        experimentName,
        variantId,
        conversionName,
        value,
        timestamp: new Date().toISOString(),
        metadata
      };

      this.sendToAnalytics(event);

      const conversions = this.conversions.get(AnalyticsEvents.CONVERSION) || [];
      conversions.push(event);
      this.conversions.set(AnalyticsEvents.CONVERSION, conversions);
      this.persistData();

      this.dispatchEvent(new CustomEvent('ab-conversion', {
        detail: event,
        bubbles: true,
        composed: true
      }));
    }

    sendToAnalytics(event) {
      if (event.experimentName && event.trackingEndpoint) {
        fetch(event.trackingEndpoint, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(event)
        }).catch(console.error);
      }
    }

    getResults(experimentName) {
      const exposures = this.conversions.get(AnalyticsEvents.EXPOSURE) || [];
      const conversions = this.conversions.get(AnalyticsEvents.CONVERSION) || [];

      const expExposures = exposures.filter(e => e.experimentName === experimentName);
      const expConversions = conversions.filter(e => e.experimentName === experimentName);

      const results = {};
      const uniqueVariants = [...new Set(expExposures.map(e => e.variantId))];

      for (const variantId of uniqueVariants) {
        const variantExposures = expExposures.filter(e => e.variantId === variantId);
        const variantConversions = expConversions.filter(e => e.variantId === variantId);

        results[variantId] = {
          exposures: variantExposures.length,
          conversions: variantConversions.length,
          conversionRate: variantExposures.length > 0 
            ? variantConversions.length / variantExposures.length 
            : 0,
          totalValue: variantConversions.reduce((sum, c) => sum + (c.value || 0), 0)
        };
      }

      return results;
    }

    endExperiment(experimentName) {
      const variantId = this.experiments.get(experimentName);
      if (variantId) {
        this.experiments.delete(experimentName);
        this.persistData();
      }
    }

    resetExperiment(experimentName) {
      const storageKey = `ab_test_${experimentName}`;
      this.storage.removeItem(storageKey);
      this.experiments.delete(experimentName);
      this.conversions.set(AnalyticsEvents.EXPOSURE, 
        (this.conversions.get(AnalyticsEvents.EXPOSURE) || []).filter(e => e.experimentName !== experimentName));
      this.conversions.set(AnalyticsEvents.CONVERSION,
        (this.conversions.get(AnalyticsEvents.CONVERSION) || []).filter(e => e.experimentName !== experimentName));
      this.persistData();
    }
  }

  class ABTestComponent extends HTMLElement {
    static get observedAttributes() {
      return ['experiment', 'variant', 'track-conversion', 'default-variant'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._experimentName = '';
      this._variantId = '';
      this._conversionEvent = '';
      this._tracker = null;
      this._variant = null;
    }

    static get observedAttributes() {
      return ['experiment', 'variant', 'track-conversion', 'track-value', 'default-variant', 'persist'];
    }

    static get tracker() {
      if (!this._trackerInstance) {
        this._trackerInstance = new ABTestTracker();
      }
      return this._trackerInstance;
    }

    static set tracker(value) {
      this._trackerInstance = value;
    }

    connectedCallback() {
      this._tracker = this.constructor.tracker;
      this._experimentName = this.getAttribute('experiment') || '';
      this._conversionEvent = this.getAttribute('track-conversion') || '';
      this.initializeExperiment();
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'experiment') {
          this._experimentName = newValue;
          this.initializeExperiment();
        } else if (name === 'track-conversion') {
          this._conversionEvent = newValue;
        }
      }
    }

    initializeExperiment() {
      if (!this._experimentName || !this._tracker) return;

      const explicitVariant = this.getAttribute('variant');
      if (explicitVariant) {
        this._variantId = explicitVariant;
        return;
      }

      const defaultVariant = this.getAttribute('default-variant') || 'control';
      this._variant = this._tracker.selectVariant({
        name: this._experimentName,
        variants: [{ id: defaultVariant, name: defaultVariant, weight: 1, config: {} }],
        trafficAllocation: 100,
        autoTrack: true
      });

      this._variantId = this._variant?.id || defaultVariant;
    }

    get variant() {
      return this._variantId;
    }

    get variantConfig() {
      return this._variant?.config || {};
    }

    trackConversion(value = 1) {
      if (!this._experimentName || !this._conversionEvent) return;

      this._tracker.trackConversion(
        this._experimentName,
        this._conversionEvent,
        value,
        { component: this.tagName.toLowerCase() }
      );
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
        </style>
        <slot></slot>
      `;
    }
  }

  class MultivariateTestComponent extends HTMLElement {
    static get observedAttributes() {
      return ['test-name', 'variants', 'metrics', 'auto-optimize'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._testName = '';
      this._variants = [];
      this._metrics = new Map();
      this._autoOptimize = false;
      this._selectedVariantIndex = 0;
    }

    static get observedAttributes() {
      return ['test-name', 'variants', 'metrics', 'auto-optimize', 'optimization-goal'];
    }

    connectedCallback() {
      this._testName = this.getAttribute('test-name') || '';
      this._autoOptimize = this.hasAttribute('auto-optimize');
      this.parseVariants();
      this.selectVariant();
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'test-name') {
          this._testName = newValue;
        } else if (name === 'variants') {
          this.parseVariants();
        } else if (name === 'auto-optimize') {
          this._autoOptimize = this.hasAttribute('auto-optimize');
        }
      }
    }

    parseVariants() {
      const variantsAttr = this.getAttribute('variants');
      if (variantsAttr) {
        try {
          this._variants = JSON.parse(variantsAttr);
        } catch (e) {
          this._variants = [];
        }
      } else {
        this._variants = [];
      }
    }

    selectVariant() {
      if (this._variants.length === 0) {
        this._selectedVariantIndex = 0;
        return;
      }

      if (this._autoOptimize) {
        this._selectedVariantIndex = this.calculateOptimalVariant();
      } else {
        this._selectedVariantIndex = Math.floor(Math.random() * this._variants.length);
      }

      const variant = this._variants[this._selectedVariantIndex];
      this.trackExposure(variant);
    }

    calculateOptimalVariant() {
      let bestIndex = 0;
      let bestScore = -Infinity;

      this._variants.forEach((variant, index) => {
        const score = this.calculateVariantScore(variant);
        if (score > bestScore) {
          bestScore = score;
          bestIndex = index;
        }
      });

      return bestIndex;
    }

    calculateVariantScore(variant) {
      const metrics = this._metrics.get(variant.id) || {};
      const impressions = metrics.impressions || 0;
      const conversions = metrics.conversions || 0;

      if (impressions === 0) return 0;

      const baseRate = conversions / impressions;
      const sampleSize = Math.sqrt(impressions);

      return baseRate * 100 + (sampleSize / 100);
    }

    trackExposure(variant) {
      const metric = this._metrics.get(variant.id) || { impressions: 0, conversions: 0 };
      metric.impressions = (metric.impressions || 0) + 1;
      this._metrics.set(variant.id, metric);
    }

    recordConversion(variantId, value = 1) {
      this._variants.forEach((variant, index) => {
        if (variant.id === variantId || variant.id === undefined && index === this._selectedVariantIndex) {
          const metric = this._metrics.get(variant.id || index) || { impressions: 0, conversions: 0 };
          metric.conversions = (metric.conversions || 0) + value;
          this._metrics.set(variant.id || index, metric);
        }
      });
    }

    get selectedVariant() {
      return this._variants[this._selectedVariantIndex];
    }

    getVariantById(id) {
      return this._variants.find(v => v.id === id);
    }

    render() {
      const variant = this.selectedVariant || {};
      const content = variant.content || '';
      const template = variant.template || '';

      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
        </style>
        ${template ? `<template>${template}</template>` : ''}
        ${content ? `<div class="content">${content}</div>` : '<slot></slot>'}
      `;
    }

    getMetrics() {
      return {
        variants: Array.from(this._metrics.entries()).map(([id, data]) => ({
          id,
          ...data,
          rate: data.impressions > 0 ? data.conversions / data.impressions : 0
        }))
      };
    }
  }

  class TestProvider extends HTMLElement {
    static get observedAttributes() {
      return ['tracker'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._tracker = new ABTestTracker();
      this.setTrackerInstance();
    }

    static get observedAttributes() {
      return ['tracker', 'storage-type'];
    }

    setTrackerInstance() {
      ABTestComponent._trackerInstance = this._tracker;
    }

    connectedCallback() {
      const storageType = this.getAttribute('storage-type');
      if (storageType) {
        this._tracker = new ABTestTracker({ storageType });
        this.setTrackerInstance();
      }
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue && name === 'storage-type') {
        this._tracker = new ABTestTracker({ storageType: newValue });
        this.setTrackerInstance();
      }
    }

    get tracker() {
      return this._tracker;
    }

    createExperiment(name, variants, options) {
      return this._tracker.createExperiment(name, variants, options);
    }

    enroll(experiment, options) {
      return this._tracker.enroll(experiment, options);
    }

    trackConversion(experimentName, conversionName, value, metadata) {
      this._tracker.trackConversion(experimentName, conversionName, value, metadata);
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: contents;
          }
        </style>
        <slot></slot>
      `;
    }
  }

  customElements.define('ab-test-component', ABTestComponent);
  customElements.define('multivariate-test', MultivariateTestComponent);
  customElements.define('ab-test-provider', TestProvider);

  if (typeof window !== 'undefined') {
    window.ComponentABTesting = {
      ABTestTracker,
      ABTestComponent,
      MultivariateTestComponent,
      TestProvider,
      AnalyticsEvents
    };
  }

  export {
    ABTestTracker,
    ABTestComponent,
    MultivariateTestComponent,
    TestProvider,
    AnalyticsEvents
  };
})();