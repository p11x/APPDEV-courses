/**
 * Enterprise Architecture Patterns - Enterprise-grade patterns for Web Components including
 * feature flags, state management, micro-frontends, and scaling patterns
 * @module advanced-patterns/10_6_Enterprise-Architecture-Patterns
 * @version 1.0.0
 * @example <enterprise-component></enterprise-component>
 */

class FeatureFlagManager {
  constructor() {
    this.flags = new Map();
    this.overrides = new Map();
    this.listeners = new Map();
  }

  register(name, config = {}) {
    this.flags.set(name, {
      enabled: config.enabled ?? false,
      rollout: config.rollout ?? 0,
      targetGroups: config.targetGroups ?? [],
      conditions: config.conditions ?? [],
      metadata: config.metadata ?? {},
    });
  }

  enable(name) {
    const flag = this.flags.get(name);
    if (flag) {
      flag.enabled = true;
      this.notify(name, true);
    }
  }

  disable(name) {
    const flag = this.flags.get(name);
    if (flag) {
      flag.enabled = false;
      this.notify(name, false);
    }
  }

  isEnabled(name, context = {}) {
    const override = this.overrides.get(name);
    if (override !== undefined) return override;

    const flag = this.flags.get(name);
    if (!flag) return false;
    if (!flag.enabled) return false;

    if (flag.rollout > 0 && flag.rollout < 100) {
      const hash = this.hashContext(name, context);
      if ((hash % 100) >= flag.rollout) return false;
    }

    for (const group of flag.targetGroups) {
      if (context.groups?.includes(group)) return true;
    }

    for (const condition of flag.conditions) {
      if (!this.evaluateCondition(condition, context)) return false;
    }

    return true;
  }

  hashContext(name, context) {
    const str = `${name}:${JSON.stringify(context)}`;
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      const char = str.charCodeAt(i);
      hash = ((hash << 5) - hash) + char;
      hash = hash & hash;
    }
    return Math.abs(hash);
  }

  evaluateCondition(condition, context) {
    const { field, operator, value } = condition;
    const contextValue = this.getNestedValue(context, field);

    switch (operator) {
      case 'eq': return contextValue === value;
      case 'neq': return contextValue !== value;
      case 'gt': return contextValue > value;
      case 'gte': return contextValue >= value;
      case 'lt': return contextValue < value;
      case 'lte': return contextValue <= value;
      case 'in': return value.includes(contextValue);
      default: return false;
    }
  }

  getNestedValue(obj, path) {
    return path.split('.').reduce((o, k) => o?.[k], obj);
  }

  override(name, value) {
    this.overrides.set(name, value);
    this.notify(name, value);
  }

  clearOverride(name) {
    this.overrides.delete(name);
  }

  onChange(name, callback) {
    if (!this.listeners.has(name)) {
      this.listeners.set(name, new Set());
    }
    this.listeners.get(name).add(callback);
  }

  notify(name, value) {
    const callbacks = this.listeners.get(name);
    if (callbacks) {
      callbacks.forEach(cb => cb(value));
    }
  }

  getAll() {
    return Array.from(this.flags.entries()).map(([name, config]) => ({
      name,
      ...config,
      enabled: config.enabled && this.isEnabled(name),
    }));
  }
}

class StateStore {
  constructor() {
    this.state = new Map();
    this.reducers = new Map();
    this.listeners = new Set();
    this.middleware = [];
    this.history = [];
    this.maxHistory = 50;
  }

  getState(key) {
    return this.state.get(key);
  }

  setState(key, value) {
    const oldValue = this.state.get(key);
    this.state.set(key, value);
    this.notify({ type: 'SET', key, oldValue, newValue: value });
  }

  dispatch(action) {
    const processed = this.processMiddleware(action);
    const reducer = this.reducers.get(processed.type);

    if (reducer) {
      const newState = reducer(this.state, processed.payload);
      for (const [key, value] of Object.entries(newState)) {
        this.setState(key, value);
      }
    }

    this.history.push({ ...processed, timestamp: Date.now() });
    if (this.history.length > this.maxHistory) {
      this.history.shift();
    }

    return processed;
  }

  registerReducer(type, reducer) {
    this.reducers.set(type, reducer);
  }

  useMiddleware(fn) {
    this.middleware.push(fn);
  }

  processMiddleware(action) {
    let processed = action;
    for (const fn of this.middleware) {
      processed = fn(processed) || processed;
    }
    return processed;
  }

  subscribe(listener) {
    this.listeners.add(listener);
    return () => this.listeners.delete(listener);
  }

  notify(change) {
    this.listeners.forEach(l => l(change));
  }

  select(selector) {
    const state = Object.fromEntries(this.state);
    return selector(state);
  }

  reset() {
    this.state.clear();
    this.history = [];
  }
}

class MicroFrontendLoader {
  constructor() {
    this.apps = new Map();
    this.activeApp = null;
    this.shared = {};
  }

  register(config) {
    this.apps.set(config.name, {
      ...config,
      loaded: false,
      exports: null,
    });
  }

  async load(name, mountPoint) {
    const app = this.apps.get(name);
    if (!app) throw new Error(`App ${name} not found`);

    if (app.loaded && app.exports) {
      return app.exports;
    }

    if (app.bundle) {
      const module = await import(app.bundle);
      app.exports = app.mount ? module : module.default;
      app.loaded = true;
    }

    if (app.mount && mountPoint) {
      app.mount(mountPoint, this.shared);
    }

    this.activeApp = name;
    return app.exports;
  }

  async unload(name) {
    const app = this.apps.get(name);
    if (app && app.exports && app.unmount) {
      app.unmount();
    }
  }

  setShared(services) {
    this.shared = { ...this.shared, ...services };
  }

  getApp(name) {
    return this.apps.get(name);
  }

  getActiveApp() {
    return this.activeApp;
  }

  getApps() {
    return Array.from(this.apps.values());
  }
}

class ComponentFactory {
  constructor() {
    this.definitions = new Map();
    this.templates = new Map();
    this.mixins = [];
  }

  define(name, config) {
    this.definitions.set(name, config);
  }

  create(name, overrides = {}) {
    const def = this.definitions.get(name);
    if (!def) throw new Error(`Definition ${name} not found`);

    const config = { ...def, ...overrides };
    return this.assemble(config);
  }

  addMixin(mixin) {
    this.mixins.push(mixin);
  }

  assemble(config) {
    let ComponentClass = config.base || HTMLElement;

    for (const mixin of this.mixins) {
      ComponentClass = mixin(ComponentClass);
    }

    return class extends ComponentClass {
      constructor() {
        super();
        Object.assign(this, config.properties);
      }

      connectedCallback() {
        if (config.render) {
          this.render();
        }
      }
    };
  }

  register(name, tagName) {
    const ComponentClass = this.create(name);
    customElements.define(tagName, ComponentClass);
  }
}

class ComponentRegistryV2 {
  constructor() {
    this.components = new Map();
    this.categories = new Map();
    this.dependencies = new Map();
  }

  register(component, category = 'default') {
    this.components.set(component.tagName, {
      component,
      category,
      metadata: {},
    });

    if (!this.categories.has(category)) {
      this.categories.set(category, new Set());
    }
    this.categories.get(category).add(component.tagName);
  }

  addDependency(tagName, deps) {
    this.dependencies.set(tagName, deps);
  }

  resolveDependencies(tagName, resolved = new Set()) {
    if (resolved.has(tagName)) return resolved;
    const deps = this.dependencies.get(tagName) || [];

    for (const dep of deps) {
      this.resolveDependencies(dep, resolved);
    }

    resolved.add(tagName);
    return resolved;
  }

  getByCategory(category) {
    return Array.from(this.categories.get(category) || []);
  }

  async loadCategory(category) {
    const tags = this.getByCategory(category);
    for (const tag of tags) {
      await customElements.whenDefined(tag);
    }
  }
}

const featureFlags = new FeatureFlagManager();
const stateStore = new StateStore();
const microFrontendLoader = new MicroFrontendLoader();
const componentFactory = new ComponentFactory();
const registryV2 = new ComponentRegistryV2();

export { FeatureFlagManager, StateStore, MicroFrontendLoader, ComponentFactory, ComponentRegistryV2 };
export { featureFlags, stateStore, microFrontendLoader, componentFactory, registryV2 };

export default featureFlags;