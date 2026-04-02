---
title: "Bootstrap Plugin SDK"
difficulty: 3
category: "Advanced Development"
subcategory: "Custom Frameworks"
prerequisites:
  - Bootstrap 5 JavaScript API
  - ES6 Classes and Modules
  - Plugin Architecture Patterns
---

## Overview

The Bootstrap Plugin SDK provides a standardized framework for creating JavaScript plugins that integrate seamlessly with Bootstrap's existing plugin system. Bootstrap 5 uses a consistent pattern for its plugins (Modal, Dropdown, Tooltip, etc.) involving class-based components, data-api initialization, jQuery-free event handling, and a `getOrCreateInstance` pattern. The SDK formalizes these patterns so third-party plugins follow the same conventions.

A well-designed plugin SDK abstracts the boilerplate of plugin creation: default configuration merging, data attribute parsing, event dispatching, instance management, and disposal handling. Plugin authors focus on the component logic while the SDK handles integration with Bootstrap's ecosystem.

The SDK should support versioning alongside Bootstrap, provide TypeScript generics for configuration types, and include utilities for testing plugins against Bootstrap's compatibility requirements.

## Basic Implementation

The SDK provides a base class that all plugins extend, handling common functionality.

```js
// bootstrap-plugin-sdk.js
export class BootstrapPluginBase {
  static VERSION = '1.0.0';
  static DATA_KEY;
  static EVENT_KEY;
  static Default = {};

  constructor(element, config = {}) {
    this._element = typeof element === 'string'
      ? document.querySelector(element)
      : element;

    if (!this._element) {
      throw new Error(`Element not found`);
    }

    this._config = this._getConfig(config);
    this._isDisposed = false;
  }

  // Merge defaults, data attributes, and config
  _getConfig(config) {
    const dataAttrs = this._getDataAttributes();
    return { ...this.constructor.Default, ...dataAttrs, ...config };
  }

  _getDataAttributes() {
    const attrs = {};
    const prefix = `data-bs-`;

    Object.keys(this.constructor.Default).forEach(key => {
      const attr = this._element.getAttribute(`${prefix}${this._toKebab(key)}`);
      if (attr !== null) {
        attrs[key] = this._parseValue(attr);
      }
    });

    return attrs;
  }

  _toKebab(str) {
    return str.replace(/([A-Z])/g, '-$1').toLowerCase();
  }

  _parseValue(value) {
    if (value === 'true') return true;
    if (value === 'false') return false;
    if (value === 'null') return null;
    if (!isNaN(Number(value))) return Number(value);
    return value;
  }

  // Event system matching Bootstrap conventions
  _trigger(eventName, relatedTarget = null) {
    const event = new CustomEvent(
      `${this.constructor.EVENT_KEY}${eventName}`,
      {
        bubbles: true,
        cancelable: true,
        detail: { relatedTarget }
      }
    );

    const defaultPrevented = !this._element.dispatchEvent(event);
    return defaultPrevented;
  }

  // Instance management
  static getInstance(element) {
    return this.INSTANCES?.get(element) || null;
  }

  static getOrCreateInstance(element, config = {}) {
    return this.getInstance(element) || new this(element, config);
  }

  static _registerInstance(instance) {
    if (!this.INSTANCES) this.INSTANCES = new WeakMap();
    this.INSTANCES.set(instance._element, instance);
  }

  // Data API: auto-initialize from markup
  static _jQueryInterface(config) {
    return this.each(function () {
      const data = this.getInstance(this);
      if (typeof config === 'string') {
        data?.[config]();
      } else {
        new this(this, typeof config === 'object' ? config : {});
      }
    });
  }

  dispose() {
    this._isDisposed = true;
    this.constructor.INSTANCES?.delete(this._element);
    this._element = null;
  }
}
```

```html
<!-- Plugin usage with data attributes -->
<div data-bs-plugin="my-counter" data-bs-start="0" data-bs-step="1">
  <span class="counter-display">0</span>
  <button class="btn btn-primary" data-counter-action="increment">+1</button>
  <button class="btn btn-secondary" data-counter-action="decrement">-1</button>
  <button class="btn btn-outline-danger" data-counter-action="reset">Reset</button>
</div>
```

```js
// Example plugin using the SDK
import { BootstrapPluginBase } from './bootstrap-plugin-sdk.js';

export class CounterPlugin extends BootstrapPluginBase {
  static DATA_KEY = 'bs.counter';
  static EVENT_KEY = '.bs.counter';
  static Default = {
    start: 0,
    step: 1,
    min: null,
    max: null
  };

  constructor(element, config = {}) {
    super(element, config);
    this._value = this._config.start;
    this._display = this._element.querySelector('.counter-display');
    this._bindEvents();
    CounterPlugin._registerInstance(this);
  }

  _bindEvents() {
    this._element.addEventListener('click', (e) => {
      const action = e.target.dataset.counterAction;
      if (action === 'increment') this.increment();
      if (action === 'decrement') this.decrement();
      if (action === 'reset') this.reset();
    });
  }

  _update() {
    if (this._display) this._display.textContent = this._value;
    this._trigger('change', { value: this._value });
  }

  increment() {
    if (this._config.max !== null && this._value >= this._config.max) return;
    this._value += this._config.step;
    this._update();
  }

  decrement() {
    if (this._config.min !== null && this._value <= this._config.min) return;
    this._value -= this._config.step;
    this._update();
  }

  reset() {
    this._value = this._config.start;
    this._update();
  }

  get value() { return this._value; }
}

// Data API auto-init
document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('[data-bs-plugin="my-counter"]').forEach(el => {
    CounterPlugin.getOrCreateInstance(el);
  });
});
```

## Advanced Variations

```js
// Advanced: Plugin with lifecycle hooks and middleware
export class AdvancedPluginBase extends BootstrapPluginBase {
  constructor(element, config) {
    super(element, config);
    this._lifecycle = {
      beforeInit: [],
      afterInit: [],
      beforeUpdate: [],
      afterUpdate: [],
      beforeDestroy: []
    };
    this._middleware = [];
  }

  // Lifecycle hooks
  on(hook, callback) {
    if (this._lifecycle[hook]) {
      this._lifecycle[hook].push(callback);
    }
    return this;
  }

  async _runLifecycle(hook, context = {}) {
    for (const callback of this._lifecycle[hook]) {
      await callback({ ...context, plugin: this, element: this._element });
    }
  }

  // Middleware pattern
  use(middleware) {
    this._middleware.push(middleware);
    return this;
  }

  async _processMiddleware(action, payload) {
    let result = payload;
    for (const mw of this._middleware) {
      if (mw[action]) {
        result = await mw[action](result, this);
      }
    }
    return result;
  }

  async init() {
    await this._runLifecycle('beforeInit');
    await this._processMiddleware('init', this._config);
    this._initialized = true;
    await this._runLifecycle('afterInit');
  }

  async destroy() {
    await this._runLifecycle('beforeDestroy');
    super.dispose();
  }
}

// Logger middleware
const loggerMiddleware = {
  init: (config, plugin) => {
    console.log(`[${plugin.constructor.DATA_KEY}] Initializing with`, config);
    return config;
  },
  action: (payload, plugin) => {
    console.log(`[${plugin.constructor.DATA_KEY}] Action:`, payload);
    return payload;
  }
};
```

```js
// Plugin registry for discovering and managing plugins
class PluginRegistry {
  static plugins = new Map();

  static register(name, PluginClass) {
    if (this.plugins.has(name)) {
      console.warn(`Plugin "${name}" is already registered. Overwriting.`);
    }
    this.plugins.set(name, PluginClass);
    this._setupDataApi(name, PluginClass);
    return this;
  }

  static _setupDataApi(name, PluginClass) {
    const selector = `[data-bs-plugin="${name}"]`;
    const initHandler = () => {
      document.querySelectorAll(selector).forEach(el => {
        PluginClass.getOrCreateInstance(el);
      });
    };

    if (document.readyState === 'loading') {
      document.addEventListener('DOMContentLoaded', initHandler);
    } else {
      initHandler();
    }
  }

  static get(name) {
    return this.plugins.get(name);
  }

  static list() {
    return Array.from(this.plugins.keys());
  }

  static initAll(context = document) {
    this.plugins.forEach((PluginClass, name) => {
      context.querySelectorAll(`[data-bs-plugin="${name}"]`).forEach(el => {
        PluginClass.getOrCreateInstance(el);
      });
    });
  }
}
```

## Best Practices

1. **Follow Bootstrap's naming conventions** - Use `bs.` prefix for DATA_KEY, `.bs.` for EVENT_KEY, and `data-bs-` for data attributes.
2. **Implement getOrCreateInstance** - Every plugin must support singleton retrieval from DOM elements.
3. **Use WeakMap for instance storage** - Prevents memory leaks when elements are removed from the DOM.
4. **Dispatch cancelable events** - Allow consumers to call `preventDefault()` on lifecycle events to cancel actions.
5. **Parse data attributes correctly** - Handle boolean strings, numbers, JSON values, and null consistently.
6. **Support both programmatic and declarative APIs** - Plugins should work with both `new Plugin(el, config)` and `data-bs-plugin` attributes.
7. **Implement proper disposal** - Remove event listeners, clear timers, and nullify references in `dispose()`.
8. **Version your plugin** - Include a static `VERSION` property that matches your package version.
9. **Document configuration options** - Every default config key must be documented with type, default, and description.
10. **Provide TypeScript definitions** - Export interfaces for configuration objects and event detail types.

## Common Pitfalls

1. **Not cleaning up event listeners** - Failing to remove listeners in `dispose()` causes memory leaks and duplicate handlers.
2. **Overwriting Bootstrap's data-api** - Using the same `data-bs-*` attribute names as Bootstrap's built-in plugins causes conflicts.
3. **Missing null checks** - Not checking if element exists before accessing its properties throws errors with invalid selectors.
4. **Blocking disposal** - Holding references to disposed elements prevents garbage collection.
5. **Inconsistent event naming** - Using different event name patterns than Bootstrap makes integration difficult for consumers.

## Accessibility Considerations

Plugins must maintain keyboard accessibility and provide appropriate ARIA attributes for dynamic content.

```html
<!-- Accessible plugin: Star Rating -->
<div class="star-rating"
     data-bs-plugin="star-rating"
     data-bs-max="5"
     data-bs-value="0"
     role="radiogroup"
     aria-label="Rating">
  <button class="star-rating__star" role="radio" aria-checked="false" aria-label="1 star" tabindex="0">&#9734;</button>
  <button class="star-rating__star" role="radio" aria-checked="false" aria-label="2 stars" tabindex="-1">&#9734;</button>
  <button class="star-rating__star" role="radio" aria-checked="false" aria-label="3 stars" tabindex="-1">&#9734;</button>
  <button class="star-rating__star" role="radio" aria-checked="false" aria-label="4 stars" tabindex="-1">&#9734;</button>
  <button class="star-rating__star" role="radio" aria-checked="false" aria-label="5 stars" tabindex="-1">&#9734;</button>
  <span class="visually-hidden" aria-live="polite" id="rating-announce"></span>
</div>
```

## Responsive Behavior

Plugins should adapt their rendering and interaction patterns based on viewport size. Touch-friendly sizing, appropriate spacing, and mobile-optimized layouts should be configurable through plugin options.

```js
// Responsive-aware plugin
class ResponsiveTooltip extends BootstrapPluginBase {
  static Default = {
    placement: 'top',
    mobilePlacement: 'bottom',
    trigger: 'hover focus',
    mobileTrigger: 'click',
    breakpoint: 'md'
  };

  _getEffectiveConfig() {
    const isMobile = window.innerWidth < 768; // Bootstrap md breakpoint
    return {
      ...this._config,
      placement: isMobile ? this._config.mobilePlacement : this._config.placement,
      trigger: isMobile ? this._config.mobileTrigger : this._config.trigger
    };
  }
}
```
