/**
 * Micro-Frontend Implementation - Module Federation and component composition for scalable micro-frontends
 * @module real-world/11_5_Micro-Frontend-Implementation
 * @version 1.0.0
 * @example <micro-app></micro-app>
 */

class MicroAppRegistry {
  constructor() {
    this.apps = new Map();
    this.activeApp = null;
    this.loadingStrategy = 'eager';
  }

  register(name, config) {
    this.apps.set(name, {
      name,
      entry: config.entry,
      module: config.module,
      routes: config.routes || [],
      props: config.props || {},
      lifecycle: config.lifecycle || {},
      activeWhen: config.activeWhen || '/',
    });
    return this;
  }

  unregister(name) {
    return this.apps.delete(name);
  }

  get(name) {
    return this.apps.get(name);
  }

  getAll() {
    return Array.from(this.apps.values());
  }

  async bootstrap(name) {
    const app = this.get(name);
    if (!app) {
      throw new Error(`App ${name} not registered`);
    }

    const module = await this.loadModule(app.entry);
    if (app.lifecycle.bootstrap) {
      await app.lifecycle.bootstrap(module);
    }
    return module;
  }

  async mount(name, container) {
    const app = this.get(name);
    if (!app) {
      throw new Error(`App ${name} not registered`);
    }

    const module = await this.loadModule(app.entry);
    if (module.mount) {
      await module.mount(container, app.props);
    }

    this.activeApp = name;
    return module;
  }

  async unmount(name) {
    const app = this.get(name);
    if (!app) return;

    const module = await this.loadModule(app.entry);
    if (module.unmount) {
      await module.unmount();
    }

    if (this.activeApp === name) {
      this.activeApp = null;
    }
  }

  async loadModule(entry) {
    return import(entry);
  }

  getActiveApp() {
    return this.activeApp;
  }
}

class MicroApp extends HTMLElement {
  constructor() {
    super();
    this.appName = '';
    this.appProps = {};
    this.loaded = false;
    this.error = null;
    this.registry = new MicroAppRegistry();
  }

  static get observedAttributes() {
    return ['name', 'src', 'props'];
  }

  static get styles() {
    return `
      :host {
        display: contents;
      }
      .micro-app-container {
        all: initial;
      }
      .loading {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 200px;
      }
      .spinner {
        width: 40px;
        height: 40px;
        border: 3px solid #f3f3f3;
        border-top: 3px solid #667eea;
        border-radius: 50%;
        animation: spin 1s linear infinite;
      }
      @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
      }
      .error {
        color: #dc3545;
        padding: 20px;
        text-align: center;
      }
      .app-root {
        display: block;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
    this.loadApp();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'props') {
        this.appProps = JSON.parse(newValue || '{}');
      } else {
        this[name] = newValue;
      }
    }
  }

  async loadApp() {
    if (!this.appName && !this.getAttribute('src')) {
      return;
    }

    this.renderLoading();

    try {
      const src = this.getAttribute('src');
      if (src) {
        await this.loadRemoteApp(src);
      } else {
        await this.loadLocalApp();
      }
      this.loaded = true;
      this.render();
    } catch (error) {
      this.error = error.message;
      this.renderError();
    }
  }

  async loadRemoteApp(src) {
    const response = await globalThis.fetch(src);
    const config = await response.json();
    
    this.registry.register(this.appName || config.name, {
      entry: config.entry,
      module: config.module,
      props: this.appProps,
    });
  }

  async loadLocalApp() {
    const app = this.registry.get(this.appName);
    if (!app) {
      throw new Error(`App ${this.appName} not found in registry`);
    }
  }

  render() {
    if (this.error) {
      this.renderError();
      return;
    }

    const name = this.getAttribute('name') || 'component';
    this.shadowRoot.innerHTML = `
      <style>${MicroApp.styles}</style>
      ${!this.loaded ? `
        <div class="loading">
          <div class="spinner"></div>
        </div>
      ` : `
        <div class="app-root">
          <slot></slot>
        </div>
      `}
    `;
  }

  renderLoading() {
    this.shadowRoot.innerHTML = `
      <style>${MicroApp.styles}</style>
      <div class="loading">
        <div class="spinner"></div>
      </div>
    `;
  }

  renderError() {
    this.shadowRoot.innerHTML = `
      <style>${MicroApp.styles}</style>
      <div class="error">
        <p>⚠️ Failed to load micro-app</p>
        <p>${this.error}</p>
        <button onclick="location.reload()">Retry</button>
      </div>
    `;
  }
}

class ComponentLoader extends HTMLElement {
  constructor() {
    super();
    this.components = new Map();
    this.loading = new Set();
  }

  static get styles() {
    return `
      :host {
        display: contents;
      }
      .component-wrapper {
        all: initial;
        font-family: inherit;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
  }

  register(name, loader) {
    this.components.set(name, loader);
    return this;
  }

  async load(componentName, props = {}) {
    if (this.loading.has(componentName)) {
      return this.loading.get(componentName);
    }

    const loader = this.components.get(componentName);
    if (!loader) {
      throw new Error(`Component ${componentName} not registered`);
    }

    const loadPromise = loader(props);
    this.loading.set(componentName, loadPromise);

    try {
      const Component = await loadPromise;
      this.renderComponent(Component, props);
    } finally {
      this.loading.delete(componentName);
    }
  }

  renderComponent(Component, props) {
    const wrapper = document.createElement('div');
    wrapper.className = 'component-wrapper';
    
    const instance = new Component(props);
    wrapper.appendChild(instance);
    
    this.shadowRoot.appendChild(wrapper);
  }

  async ssr(componentName, props = {}) {
    const loader = this.components.get(componentName);
    if (!loader) {
      throw new Error(`Component ${componentName} not registered`);
    }

    const Component = await loader(props);
    return Component.renderToString(props);
  }
}

class SharedState extends HTMLElement {
  constructor() {
    super();
    this.state = {};
    this.listeners = new Map();
  }

  static get instance() {
    if (!SharedState._instance) {
      SharedState._instance = new SharedState();
    }
    return SharedState._instance;
  }

  get(key) {
    return this.state[key];
  }

  set(key, value) {
    const oldValue = this.state[key];
    this.state[key] = value;
    this.notify(key, value, oldValue);
  }

  update(updates) {
    const oldState = { ...this.state };
    Object.assign(this.state, updates);
    
    for (const key in updates) {
      this.notify(key, updates[key], oldState[key]);
    }
  }

  subscribe(key, callback) {
    if (!this.listeners.has(key)) {
      this.listeners.set(key, new Set());
    }
    this.listeners.get(key).add(callback);
    
    return () => {
      this.listeners.get(key)?.delete(callback);
    };
  }

  notify(key, newValue, oldValue) {
    const listeners = this.listeners.get(key);
    if (listeners) {
      listeners.forEach(callback => {
        callback(newValue, oldValue);
      });
    }

    const globalListeners = this.listeners.get('*');
    if (globalListeners) {
      globalListeners.forEach(callback => {
        callback(key, newValue, oldValue);
      });
    }
  }

  getState() {
    return { ...this.state };
  }
}

class EventBus extends HTMLElement {
  constructor() {
    super();
    this.channels = new Map();
  }

  static get instance() {
    if (!EventBus._instance) {
      EventBus._instance = new EventBus();
    }
    return EventBus._instance;
  }

  subscribe(channel, callback) {
    if (!this.channels.has(channel)) {
      this.channels.set(channel, new Set());
    }
    
    const subscription = { channel, callback };
    this.channels.get(channel).add(callback);
    
    return () => {
      this.channels.get(channel)?.delete(callback);
    };
  }

  publish(channel, data) {
    const listeners = this.channels.get(channel);
    if (listeners) {
      listeners.forEach(callback => {
        try {
          callback(data);
        } catch (error) {
          console.error(`Error in channel ${channel}:`, error);
        }
      });
    }
  }

  once(channel, callback) {
    const wrapper = (data) => {
      callback(data);
      this.unsubscribe(channel, wrapper);
    };
    this.subscribe(channel, wrapper);
  }
}

class RouteManager extends HTMLElement {
  constructor() {
    super();
    this.routes = [];
    this.currentPath = window.location.pathname;
  }

  static get styles() {
    return `
      :host {
        display: none;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
    this.setupRouting();
  }

  addRoute(path, component, props = {}) {
    this.routes.push({ path, component, props });
    return this;
  }

  setupRouting() {
    window.addEventListener('popstate', () => {
      this.navigate(window.location.pathname, {}, false);
    });
  }

  navigate(path, state = {}, replace = false) {
    if (replace) {
      window.history.replaceState(state, '', path);
    } else {
      window.history.pushState(state, '', path);
    }
    this.currentPath = path;
    this.renderRoute();
  }

  renderRoute() {
    const route = this.routes.find(r => this.matchPath(r.path, this.currentPath));
    
    this.shadowRoot.innerHTML = `
      <style>${RouteManager.styles}</style>
    `;

    if (route) {
      const Component = route.component;
      const instance = new Component(route.props);
      this.shadowRoot.appendChild(instance);
    }
  }

  matchPath(pattern, path) {
    if (pattern === '*') return true;
    if (pattern === path) return true;
    
    const patternParts = pattern.split('/');
    const pathParts = path.split('/');
    
    if (patternParts.length !== pathParts.length) return false;
    
    return patternParts.every((part, i) => {
      if (part.startsWith(':')) return true;
      return part === pathParts[i];
    });
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${RouteManager.styles}</style>
    `;
    this.renderRoute();
  }

  getCurrentRoute() {
    return this.routes.find(r => this.matchPath(r.path, this.currentPath));
  }
}

class LazyLoader extends HTMLElement {
  constructor() {
    super();
    this.loaded = false;
    this.observer = null;
    this.threshold = 0.1;
  }

  static get observedAttributes() {
    return ['src', 'threshold'];
  }

  static get styles() {
    return `
      :host {
        display: contents;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.setupIntersectionObserver();
    this.render();
  }

  disconnectedCallback() {
    this.observer?.disconnect();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'threshold') {
        this.threshold = parseFloat(newValue);
      }
      this.render();
    }
  }

  setupIntersectionObserver() {
    this.observer = new IntersectionObserver(
      (entries) => {
        entries.forEach(entry => {
          if (entry.isIntersecting && !this.loaded) {
            this.loadContent();
          }
        });
      },
      { threshold: this.threshold }
    );

    this.observer.observe(this);
  }

  async loadContent() {
    const src = this.getAttribute('src');
    if (!src) return;

    this.loaded = true;
    
    try {
      const content = await this.fetchContent(src);
      this.renderContent(content);
    } catch (error) {
      this.renderError(error.message);
    }
  }

  async fetchContent(src) {
    const response = await globalThis.fetch(src);
    return response.text();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${LazyLoader.styles}</style>
      <slot></slot>
    `;
  }

  renderContent(content) {
    this.shadowRoot.innerHTML = `
      <style>${LazyLoader.styles}</style>
      ${content}
    `;
  }

  renderError(message) {
    this.shadowRoot.innerHTML = `
      <style>${LazyLoader.styles}</style>
      <div class="error">Failed to load: ${message}</div>
    `;
  }
}

class AsyncBoundary extends HTMLElement {
  constructor() {
    super();
    this.fallback = null;
    this.error = null;
  }

  static get observedAttributes() {
    return ['fallback', 'error'];
  }

  static get styles() {
    return `
      :host {
        display: contents;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'error') {
        this.error = newValue;
        this.renderError();
      } else if (name === 'fallback') {
        this.fallback = newValue;
        this.render();
      }
    }
  }

  async wrap(promise) {
    try {
      const result = await promise;
      this.render();
      return result;
    } catch (error) {
      this.error = error.message;
      this.renderError();
      throw error;
    }
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${AsyncBoundary.styles}</style>
      <slot></slot>
    `;
  }

  renderError() {
    const fallback = this.getAttribute('fallback');
    this.shadowRoot.innerHTML = `
      <style>${AsyncBoundary.styles}</style>
      ${fallback || `<div class="error">Something went wrong: ${this.error}</div>`}
    `;
  }
}

class RemoteModuleLoader extends HTMLElement {
  constructor() {
    super();
    this.remote = '';
    this.exposes = new Map();
  }

  static get observedAttributes() {
    return ['remote'];
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue && name === 'remote') {
      this.remote = newValue;
    }
  }

  async expose(name, loader) {
    this.exposes.set(name, loader);
  }

  async load(remoteName) {
    const loader = this.exposes.get(remoteName);
    if (!loader) {
      throw new Error(`Module ${remoteName} not exposed`);
    }
    return loader();
  }

  async remotes() {
    const manifest = {
      name: this.remote,
      exposes: Object.fromEntries(this.exposes),
    };
    return manifest;
  }
}

class ModuleFederationPlugin extends HTMLElement {
  constructor() {
    super();
    this.name = '';
    this.remotes = {};
    this.shared = {};
    this.exposes = {};
  }

  static get styles() {
    return `
      :host {
        display: none;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'name') {
        this.name = newValue;
      }
    }
  }

  addRemote(name, url) {
    this.remotes[name] = url;
    return this;
  }

  addShared(name, version, shared) {
    this.shared[name] = { version, ...shared };
    return this;
  }

  expose(name, component) {
    this.exposes[name] = component;
    return this;
  }

  async bootstrap(container) {
    const entries = Object.entries(this.exposes);
    for (const [name, Component] of entries) {
      container.register(name, () => Promise.resolve(Component));
    }
    return container;
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${ModuleFederationPlugin.styles}</style>
    `;
  }
}

export { MicroAppRegistry, MicroApp, ComponentLoader, SharedState, EventBus, RouteManager, LazyLoader, AsyncBoundary, RemoteModuleLoader, ModuleFederationPlugin };