/**
 * Svelte Integration Methods - Web Components wrapped for Svelte applications
 * @module interoperability/08_5_Svelte-Integration-Methods
 * @version 1.0.0
 * @example <WebComponent tag="universal-card" {...props} on:cardAccept={handler} />
 */

function createSvelteWebComponentWrapper(options = {}) {
    const {
        tagName,
        props = [],
        events = {},
        methods = [],
        onMount,
        onDestroy
    } = options;

    return function WebComponentWrapper(props) {
        let element = null;
        let mounted = false;
        const eventListeners = new Map();

        const setupElement = () => {
            if (!mounted) return;
            element = document.createElement(tagName);
            bindProps(props);
            bindEvents();
            attachMethods();
            document.querySelector(`[data-wc-id="${props.wcId}"]`)?.appendChild(element);
        };

        const bindProps = (props) => {
            if (!element) return;
            props.forEach(prop => {
                if (props[prop] !== undefined) {
                    const value = props[prop];
                    if (typeof value === 'boolean') {
                        value ? element.setAttribute(prop, '') : element.removeAttribute(prop);
                    } else if (value !== null) {
                        element.setAttribute(prop, String(value));
                    }
                }
            });
        };

        const bindEvents = () => {
            Object.entries(events).forEach(([wcEvent, svelteEvent]) => {
                const handler = (e) => {
                    props.dispatch(svelteEvent, e.detail);
                };
                element.addEventListener(wcEvent, handler);
                eventListeners.set(wcEvent, handler);
            });
        };

        const attachMethods = () => {
            if (!element) return;
            methods.forEach(method => {
                // Methods are accessible via the returned interface
            });
        };

        const cleanup = () => {
            eventListeners.forEach((handler, event) => {
                element?.removeEventListener(event, handler);
            });
            eventListeners.clear();
            element?.remove();
            element = null;
        };

        const callMethod = (name, ...args) => {
            return element?.[name]?.(...args);
        };

        if (onMount) {
            onMount(() => {
                mounted = true;
                setupElement();
                return cleanup;
            });
        }

        return {
            element,
            callMethod,
            cleanup,
            getElement: () => element
        };
    };
}

function createSvelteAction(tagName, options = {}) {
    const {
        props: propOptions = {},
        events: eventOptions = {},
        shadowMode = 'open'
    } = options;

    return (node, config = {}) => {
        let element = null;
        const listeners = new Map();

        const init = async () => {
            await customElements.whenDefined(tagName);
            element = document.createElement(tagName);
            
            Object.entries(config).forEach(([key, value]) => {
                if (typeof value === 'boolean') {
                    value ? element.setAttribute(key, '') : element.removeAttribute(key);
                } else {
                    element.setAttribute(key, String(value));
                }
            });

            Object.entries(eventOptions).forEach(([wcEvent, handler]) => {
                const listener = (e) => handler(e.detail, e);
                element.addEventListener(wcEvent, listener);
                listeners.set(wcEvent, listener);
            });

            if (shadowMode) {
                node.attachShadow({ mode: shadowMode }).appendChild(element);
            } else {
                node.appendChild(element);
            }
        };

        const update = (config) => {
            Object.entries(config).forEach(([key, value]) => {
                if (typeof value === 'boolean') {
                    value ? element.setAttribute(key, '') : element.removeAttribute(key);
                } else {
                    element.setAttribute(key, String(value));
                }
            });
        };

        const destroy = () => {
            listeners.forEach((listener, event) => {
                element?.removeEventListener(event, listener);
            });
            listeners.clear();
            element?.remove();
            element = null;
        };

        init();

        return {
            update,
            destroy
        };
    };
}

const useWebComponent = (tagName, options = {}) => {
    const {
        props: propDefs = {},
        events: eventDefs = {},
        reactiveProps = []
    } = options;

    let element = null;
    let mounted = false;
    const listeners = new Map();
    const state = {};

    const init = async () => {
        await customElements.whenDefined(tagName);
        return document.createElement(tagName);
    };

    const mount = (el) => {
        mounted = true;
        element = el;
        setupEventListeners();
        return element;
    };

    const unmount = () => {
        cleanupEventListeners();
        mounted = false;
        element = null;
    };

    const setupEventListeners = () => {
        Object.entries(eventDefs).forEach(([wcEvent, handler]) => {
            const listener = (e) => handler(e.detail, e);
            element.addEventListener(wcEvent, listener);
            listeners.set(wcEvent, listener);
        });
    };

    const cleanupEventListeners = () => {
        listeners.forEach((listener, event) => {
            element?.removeEventListener(event, listener);
        });
        listeners.clear();
    };

    const setProp = (name, value) => {
        if (!element) return;
        if (typeof value === 'boolean') {
            value ? element.setAttribute(name, '') : element.removeAttribute(name);
        } else {
            element.setAttribute(name, String(value));
        }
    };

    const getProp = (name) => {
        return element?.getAttribute(name);
    };

    const callMethod = (name, ...args) => {
        return element?.[name]?.(...args);
    };

    return {
        init,
        mount,
        unmount,
        setProp,
        getProp,
        callMethod,
        get element() { return element; },
        get isMounted() { return mounted; }
    };
};

function defineWebComponentAsSvelteComponent(tagName, svelteConfig = {}) {
    const {
        props: propDefs = [],
        events: eventDefs = [],
        methods: methodDefs = [],
        onMount: mountHook,
        onDestroy: destroyHook
    } = svelteConfig;

    return class SvelteWrappedComponent {
        static get observedAttributes() {
            return propDefs;
        }

        constructor() {
            this.attachShadow({ mode: 'open' });
            this._bound = false;
            this._listeners = new Map();
        }

        connectedCallback() {
            if (!this._bound) {
                this._bind();
                this._bound = true;
            }
        }

        disconnectedCallback() {
            this._cleanup();
            this._bound = false;
        }

        attributeChangedCallback(name, oldValue, newValue) {
            if (oldValue !== newValue) {
                this._notifyChange(name, newValue);
            }
        }

        _bind() {
            const slot = document.createElement('slot');
            this.shadowRoot.appendChild(slot);

            this._setupEventListeners();
            this._setupMethods();
        }

        _setupEventListeners() {
            eventDefs.forEach(eventName => {
                this.addEventListener(eventName, (e) => {
                    this.dispatchEvent(new CustomEvent(eventName, { detail: e.detail }));
                });
            });
        }

        _setupMethods() {
            methodDefs.forEach(method => {
                // Method setup
            });
        }

        _notifyChange(name, value) {
            this.dispatchEvent(new CustomEvent('propChange', {
                detail: { name, value }
            }));
        }

        _cleanup() {
            this._listeners.forEach((listener, event) => {
                this.removeEventListener(event, listener);
            });
            this._listeners.clear();
        }
    };
}

function svelteToWebComponent(SvelteComponent, options = {}) {
    const {
        tagName,
        props = [],
        events = [],
        shadow = true,
        scopedStyles = true
    } = options;

    class SvelteWrapped extends HTMLElement {
        static get observedAttributes() {
            return props;
        }

        constructor() {
            super();
            if (shadow) {
                this.attachShadow({ mode: 'open' });
            }
            this._svelteInstance = null;
        }

        connectedCallback() {
            this._render();
        }

        disconnectedCallback() {
            if (this._svelteInstance?.$destroy) {
                this._svelteInstance.$destroy();
            }
        }

        attributeChangedCallback(name, oldValue, newValue) {
            if (oldValue !== newValue && this._svelteInstance) {
                this._svelteInstance[name] = this._parseValue(newValue);
            }
        }

        _render() {
            // Svelte rendering logic
        }

        _parseValue(value) {
            if (value === 'true') return true;
            if (value === 'false') return false;
            if (!isNaN(value) && value !== '') return Number(value);
            try { return JSON.parse(value); } catch { return value; }
        }
    }

    customElements.define(tagName, SvelteWrapped);
    return SvelteWrapped;
}

const SvelteWebComponentStore = {
    registry: new Map(),

    register(tagName, Component) {
        this.registry.set(tagName, Component);
    },

    get(tagName) {
        return this.registry.get(tagName);
    },

    createStore() {
        return {
            subscribe: (callback) => {
                const handler = (e) => {
                    callback(e.detail);
                };
                document.addEventListener('wc:update', handler);
                return () => document.removeEventListener('wc:update', handler);
            },
            dispatch: (tagName, detail) => {
                document.dispatchEvent(new CustomEvent('wc:update', {
                    detail: { tagName, ...detail }
                }));
            }
        };
    }
};

function createWebComponentStore(tagName) {
    let state = {};
    const subscribers = new Set();

    return {
        get state() { return state; },
        
        setState(updater) {
            if (typeof updater === 'function') {
                state = updater(state);
            } else {
                state = { ...state, ...updater };
            }
            this._notify();
        },

        subscribe(callback) {
            subscribers.add(callback);
            return () => subscribers.delete(callback);
        },

        _notify() {
            subscribers.forEach(cb => cb(state));
            document.dispatchEvent(new CustomEvent(`wc:${tagName}:update`, {
                detail: state
            }));
        }
    };
}

export {
    createSvelteWebComponentWrapper,
    createSvelteAction,
    useWebComponent,
    defineWebComponentAsSvelteComponent,
    svelteToWebComponent,
    SvelteWebComponentStore,
    createWebComponentStore
};
