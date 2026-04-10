/**
 * Micro-Frontend Architecture - Module Federation with Web Components
 * @module interoperability/08_6_Micro-Frontend-Architecture
 * @version 1.0.0
 * @example <mf-component remote="container" module="HeaderComponent"></mf-component>
 */

const MicroFrontendConfig = {
    remotes: new Map(),
    shared: new Map(),
    registeredComponents: new Map()
};

class MicroFrontendRegistry {
    constructor() {
        this._components = new Map();
        this._remoteConfigs = new Map();
    }

    register(name, config) {
        const componentConfig = {
            name,
            tagName: config.tagName || this._toTagName(name),
            module: config.module,
            remote: config.remote,
            props: config.props || {},
            styles: config.styles || null,
            dependencies: config.dependencies || [],
            lazy: config.lazy !== false,
            singleton: config.singleton || false,
            version: config.version || '1.0.0'
        };

        this._components.set(name, componentConfig);
        return this;
    }

    registerRemote(name, config) {
        const remoteConfig = {
            name,
            url: config.url,
            scope: config.scope || name,
            entry: config.entry,
            shared: config.shared || [],
            preload: config.preload || false,
            fallback: config.fallback || null
        };

        this._remoteConfigs.set(name, remoteConfig);
        MicroFrontendConfig.remotes.set(name, remoteConfig);
        return this;
    }

    async loadRemote(name) {
        const config = this._remoteConfigs.get(name);
        if (!config) {
            throw new Error(`Remote "${name}" not registered`);
        }

        if (config.fallback) {
            try {
                return await this._loadScript(config);
            } catch (error) {
                console.warn(`Failed to load remote ${name}, using fallback`);
                return config.fallback;
            }
        }

        return this._loadScript(config);
    }

    async _loadScript(config) {
        return new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.src = config.entry;
            script.async = true;

            script.onload = () => resolve(window[config.scope]);
            script.onerror = () => reject(new Error(`Failed to load remote: ${config.name}`));

            document.head.appendChild(script);
        });
    }

    get(name) {
        return this._components.get(name);
    }

    getRemote(name) {
        return this._remoteConfigs.get(name);
    }

    has(name) {
        return this._components.has(name);
    }

    list() {
        return Array.from(this._components.values());
    }

    listRemotes() {
        return Array.from(this._remoteConfigs.values());
    }

    _toTagName(name) {
        return name.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
    }
}

class MicroFrontendComponent extends HTMLElement {
    static get observedAttributes() {
        return ['remote', 'module', 'props', 'loading', 'error', 'fallback'];
    }

    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this._instance = null;
        this._loading = false;
        this._error = null;
        this._props = {};
    }

    connectedCallback() {
        this._render();
        this._loadComponent();
    }

    disconnectedCallback() {
        this._cleanup();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (oldValue === newValue) return;

        switch (name) {
            case 'props':
                this._props = this._parseProps(newValue);
                this._updateProps();
                break;
            case 'remote':
            case 'module':
                this._loadComponent();
                break;
            case 'loading':
                this._updateLoadingState();
                break;
            case 'error':
                this._updateErrorState();
                break;
            case 'fallback':
                this._render();
                break;
        }
    }

    async _loadComponent() {
        const remote = this.getAttribute('remote');
        const module = this.getAttribute('module');

        if (!remote || !module) return;

        this._setLoading(true);
        this._error = null;

        try {
            const registry = MicroFrontendRegistry;
            const remoteModule = await registry.loadRemote(remote);
            const Component = await remoteModule[module]();

            this._instance = this._createInstance(Component);
            this._applyProps();
            this._renderInstance();
            this._emit('loaded', { remote, module });
        } catch (error) {
            this._error = error;
            this._renderError();
            this._emit('error', { error, remote, module });
        } finally {
            this._setLoading(false);
        }
    }

    _createInstance(Component) {
        if (typeof Component === 'function') {
            if (customElements.get(Component.tagName)) {
                return document.createElement(Component.tagName);
            }
            return new Component();
        }

        if (Component.prototype instanceof HTMLElement) {
            return document.createElement(Component.tagName || Component.name);
        }

        return Component;
    }

    _applyProps() {
        if (!this._instance) return;

        Object.entries(this._props).forEach(([key, value]) => {
            if (typeof value === 'boolean') {
                value ? this._instance.setAttribute(key, '') : this._instance.removeAttribute(key);
            } else if (value !== null) {
                this._instance.setAttribute(key, String(value));
            }
        });
    }

    _updateProps() {
        this._applyProps();
    }

    _render() {
        this.shadowRoot.innerHTML = `
            <style>
                :host {
                    display: contents;
                }

                .container {
                    display: contents;
                }

                .loading {
                    display: none;
                    align-items: center;
                    justify-content: center;
                    min-height: 100px;
                }

                :host([loading]) .loading {
                    display: flex;
                }

                :host([loading]) .container {
                    display: none;
                }

                .spinner {
                    width: 32px;
                    height: 32px;
                    border: 3px solid #f0f0f0;
                    border-top-color: #007bff;
                    border-radius: 50%;
                    animation: spin 0.8s linear infinite;
                }

                @keyframes spin {
                    to { transform: rotate(360deg); }
                }

                .error {
                    display: none;
                    padding: 16px;
                    background: #fee;
                    border: 1px solid #fcc;
                    border-radius: 4px;
                    color: #c00;
                }

                :host([error]) .error {
                    display: block;
                }

                :host([error]) .container,
                :host([error]) .loading {
                    display: none;
                }

                .fallback {
                    display: none;
                }

                :host([error]) .fallback {
                    display: block;
                }
            </style>
            <div class="container"></div>
            <div class="loading">
                <div class="spinner"></div>
            </div>
            <div class="error"></div>
            <div class="fallback"><slot name="fallback"></slot></div>
        `;
    }

    _renderInstance() {
        if (!this._instance) return;
        const container = this.shadowRoot.querySelector('.container');
        container.innerHTML = '';
        container.appendChild(this._instance);
    }

    _renderError() {
        const errorEl = this.shadowRoot.querySelector('.error');
        if (errorEl && this._error) {
            errorEl.textContent = `Failed to load component: ${this._error.message}`;
        }
    }

    _updateLoadingState() {
        if (this.hasAttribute('loading')) {
            this.setAttribute('loading', '');
        } else {
            this.removeAttribute('loading');
        }
    }

    _updateErrorState() {
        // Handled by attribute
    }

    _setLoading(value) {
        if (value) {
            this.setAttribute('loading', '');
        } else {
            this.removeAttribute('loading');
        }
        this._loading = value;
    }

    _cleanup() {
        if (this._instance?.remove) {
            this._instance.remove();
        }
        this._instance = null;
    }

    _emit(name, detail) {
        this.dispatchEvent(new CustomEvent(name, {
            bubbles: true,
            composed: true,
            detail
        }));
    }

    get props() {
        return { ...this._props };
    }

    set props(value) {
        this._props = { ...value };
        this._applyProps();
    }

    setProp(name, value) {
        this._props[name] = value;
        this._applyProps();
    }

    reload() {
        this._cleanup();
        this._loadComponent();
    }
}

class ModuleFederationLoader {
    constructor(config) {
        this._config = config;
        this._modules = new Map();
        this._sharedScope = new Map();
    }

    async init() {
        const { name, remotes, shared } = this._config;

        for (const remote of remotes || []) {
            await this._registerRemote(remote);
        }

        for (const [name, module] of Object.entries(shared || {})) {
            this._sharedScope.set(name, module);
        }

        return this;
    }

    async _registerRemote(remote) {
        const { name, url, scope } = remote;

        if (!customElements.get(`mf-${name}`)) {
            customElements.define(`mf-${name}`, MicroFrontendComponent);
        }

        MicroFrontendRegistry.registerRemote(name, {
            name,
            url,
            scope: scope || `__remote_${name}`,
            entry: `${url}/remoteEntry.js`
        });
    }

    async loadModule(remoteName, moduleName) {
        const remote = MicroFrontendRegistry.getRemote(remoteName);
        if (!remote) {
            throw new Error(`Remote "${remoteName}" not found`);
        }

        const key = `${remoteName}:${moduleName}`;
        if (this._modules.has(key)) {
            return this._modules.get(key);
        }

        const module = await import(/* webpackIgnore: true */ `${remote.url}/${moduleName}`);
        this._modules.set(key, module);

        return module;
    }

    getShared(name) {
        return this._sharedScope.get(name);
    }

    setShared(name, module) {
        this._sharedScope.set(name, module);
    }
}

function createMicroFrontendHost(config) {
    return {
        name: config.name,
        loader: new ModuleFederationLoader(config),
        registry: MicroFrontendRegistry,

        async bootstrap() {
            await this.loader.init();
            return this;
        },

        registerComponent(name, config) {
            MicroFrontendRegistry.register(name, {
                ...config,
                remote: config.remote || this.name
            });
            return this;
        },

        mount(id, Component) {
            const container = document.getElementById(id);
            if (!container) {
                throw new Error(`Container #${id} not found`);
            }

            if (Component instanceof HTMLElement) {
                container.appendChild(Component);
            } else {
                const el = document.createElement(Component.tagName || 'div');
                container.appendChild(el);
            }

            return this;
        },

        unmount(id) {
            const container = document.getElementById(id);
            if (container) {
                container.innerHTML = '';
            }
            return this;
        }
    };
}

const MicroFrontendBus = {
    channels: new Map(),

    subscribe(channel, callback) {
        if (!this.channels.has(channel)) {
            this.channels.set(channel, new Set());
        }
        this.channels.get(channel).add(callback);

        return () => {
            this.channels.get(channel)?.delete(callback);
        };
    },

    publish(channel, message) {
        const subscribers = this.channels.get(channel);
        if (subscribers) {
            subscribers.forEach(callback => callback(message));
        }

        window.dispatchEvent(new CustomEvent(`mf:${channel}`, {
            detail: message
        }));
    },

    broadcast(message) {
        this.channels.forEach((subscribers) => {
            subscribers.forEach(callback => callback(message));
        });

        window.dispatchEvent(new CustomEvent('mf:broadcast', {
            detail: message
        }));
    }
};

customElements.define('mf-component', MicroFrontendComponent);

export {
    MicroFrontendConfig,
    MicroFrontendRegistry,
    MicroFrontendComponent,
    ModuleFederationLoader,
    createMicroFrontendHost,
    MicroFrontendBus
};
