/**
 * State Management Cross-Framework - Universal state management for Web Components
 * @module interoperability/08_8_State-Management-Cross-Framework
 * @version 1.0.0
 * @example <universal-store name="app"></universal-store>
 */

class Store extends EventTarget {
    constructor(initialState = {}, options = {}) {
        super();
        this._state = { ...initialState };
        this._listeners = new Map();
        this._computed = new Map();
        this._middleware = [];
        this._history = [];
        this._maxHistory = options.maxHistory || 50;
        this._immutable = options.immutable !== false;
        this._namespace = options.namespace || 'store';
        this._devtools = options.devtools && window.__REDUX_DEVTOOLS_EXTENSION__;
    }

    get state() {
        return this._immutable ? Object.freeze({ ...this._state }) : { ...this._state };
    }

    getState(path) {
        if (!path) return this.state;

        return path.split('.').reduce((obj, key) => {
            return obj?.[key];
        }, this._state);
    }

    setState(updates, meta = {}) {
        const prevState = { ...this.state };

        if (typeof updates === 'function') {
            updates = updates(prevState);
        }

        const processedUpdates = this._applyMiddleware(updates, meta);
        if (processedUpdates === false) return false;

        Object.entries(processedUpdates).forEach(([key, value]) => {
            if (this._immutable) {
                this._state = this._setNestedValue(this._state, key, value);
            } else {
                this._state[key] = value;
            }
        });

        this._recordHistory(prevState, processedUpdates, meta);

        this._notify(processedUpdates, prevState, meta);

        if (this._devtools) {
            this._sendToDevtools(prevState, processedUpdates);
        }

        return true;
    }

    _setNestedValue(obj, path, value) {
        const keys = path.split('.');
        const result = { ...obj };
        let current = result;

        for (let i = 0; i < keys.length - 1; i++) {
            current[keys[i]] = { ...current[keys[i]] };
            current = current[keys[i]];
        }

        current[keys[keys.length - 1]] = value;
        return result;
    }

    subscribe(listener, selector = null) {
        const id = Symbol();
        this._listeners.set(id, { listener, selector });

        const unsubscribe = () => {
            this._listeners.delete(id);
        };

        return unsubscribe;
    }

    _notify(updates, prevState, meta) {
        const changedKeys = Object.keys(updates);

        this._listeners.forEach(({ listener, selector }) => {
            try {
                if (selector) {
                    const selected = selector(this.state);
                    const prevSelected = selector(prevState);
                    if (JSON.stringify(selected) !== JSON.stringify(prevSelected)) {
                        listener(selected, prevSelected);
                    }
                } else {
                    listener(this.state, prevState);
                }
            } catch (error) {
                console.error('Listener error:', error);
            }
        });

        changedKeys.forEach(key => {
            this.dispatchEvent(new CustomEvent(`change:${key}`, {
                detail: { key, value: this._state[key], prevValue: prevState[key] }
            }));
        });

        this.dispatchEvent(new CustomEvent('change', {
            detail: { updates, prevState, state: this.state, meta }
        }));
    }

    _applyMiddleware(updates, meta) {
        let result = updates;
        for (const mw of this._middleware) {
            result = mw(result, meta, this);
            if (result === false) return false;
        }
        return result;
    }

    use(middleware) {
        this._middleware.push(middleware);
        return this;
    }

    _recordHistory(prevState, updates, meta) {
        this._history.push({
            prevState,
            updates,
            meta,
            timestamp: Date.now()
        });

        if (this._history.length > this._maxHistory) {
            this._history.shift();
        }
    }

    undo() {
        if (this._history.length === 0) return false;

        const lastAction = this._history.pop();
        this._state = { ...lastAction.prevState };

        this._notify(lastAction.updates, lastAction.prevState, { type: 'UNDO' });

        return true;
    }

    history() {
        return [...this._history];
    }

    clearHistory() {
        this._history = [];
    }

    _sendToDevtools(prevState, updates) {
        window.__REDUX_DEVTOOLS_EXTENSION__.send({
            type: `${this._namespace}/UPDATE`,
            payload: updates
        }, this.state);
    }

    reset(initialState = {}) {
        this._state = { ...initialState };
        this._history = [];
        this._notify({}, this.state, { type: 'RESET' });
    }

    replaceState(newState) {
        const prevState = this.state;
        this._state = { ...newState };
        this._notify(newState, prevState, { type: 'REPLACE' });
    }
}

class ReactiveStore extends Store {
    constructor(initialState = {}, options = {}) {
        super(initialState, options);
        this._proxy = null;
        this._reactive = options.reactive !== false;

        if (this._reactive) {
            this._proxy = this._createProxy(this._state);
        }
    }

    get proxy() {
        return this._proxy || this._state;
    }

    _createProxy(target) {
        const self = this;

        return new Proxy(target, {
            get(obj, prop) {
                if (prop === '_isProxy') return true;
                if (prop === '_store') return self;
                return Reflect.get(obj, prop);
            },

            set(obj, prop, value) {
                const prev = obj[prop];
                if (prev === value) return true;

                const result = Reflect.set(obj, prop, value);
                if (result) {
                    self._notify({ [prop]: value }, { [prop]: prev }, { type: 'PROXY_SET' });
                }
                return result;
            },

            deleteProperty(obj, prop) {
                const prev = obj[prop];
                const result = Reflect.deleteProperty(obj, prop);
                if (result) {
                    self._notify({}, { [prop]: prev }, { type: 'PROXY_DELETE' });
                }
                return result;
            }
        });
    }
}

class Slice {
    constructor(name, initialState, reducers = {}) {
        this.name = name;
        this._initialState = initialState;
        this._reducers = reducers;
        this._handlers = new Map();
    }

    get initialState() {
        return this._initialState;
    }

    reducer(state = this._initialState, action) {
        const handler = this._handlers.get(action.type);
        if (!handler) return state;

        return handler(state, action.payload);
    }

    createReducer(initialState, caseReducers) {
        return (state = initialState, action) => {
            const caseReducer = caseReducers[action.type];
            return caseReducer ? caseReducer(state, action.payload) : state;
        };
    }
}

class StoreProvider extends EventTarget {
    constructor() {
        super();
        this._stores = new Map();
        this._context = new Map();
    }

    register(name, store) {
        this._stores.set(name, store);

        store.addEventListener?.('change', (e) => {
            this.dispatchEvent(new CustomEvent('storeChange', {
                detail: { name, ...e.detail }
            }));
        });

        return this;
    }

    get(name) {
        return this._stores.get(name);
    }

    has(name) {
        return this._stores.has(name);
    }

    remove(name) {
        const store = this._stores.get(name);
        if (store) {
            this._stores.delete(name);
            this.dispatchEvent(new CustomEvent('storeRemoved', {
                detail: { name }
            }));
        }
        return this;
    }

    list() {
        return Array.from(this._stores.keys());
    }

    getAll() {
        const result = {};
        this._stores.forEach((store, name) => {
            result[name] = store.state;
        });
        return result;
    }
}

class WebComponentStore extends HTMLElement {
    static get observedAttributes() {
        return ['name', 'initial-state'];
    }

    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this._store = null;
        this._unsubscribe = null;
        this._connected = false;
    }

    connectedCallback() {
        this._connected = true;
        this._initializeStore();
        this._render();
        this._subscribe();
    }

    disconnectedCallback() {
        this._connected = false;
        this._unsubscribe?.();
        Provider?.remove(this.getAttribute('name'));
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (oldValue === newValue) return;

        if (name === 'initial-state' && this._store) {
            try {
                const state = JSON.parse(newValue);
                this._store.reset(state);
            } catch (e) {
                console.error('Failed to parse initial state:', e);
            }
        }
    }

    _initializeStore() {
        const name = this.getAttribute('name') || 'default';
        let initialState = {};

        try {
            const stateAttr = this.getAttribute('initial-state');
            if (stateAttr) {
                initialState = JSON.parse(stateAttr);
            }
        } catch (e) {
            console.error('Failed to parse initial state:', e);
        }

        this._store = new ReactiveStore(initialState, {
            namespace: name,
            devtools: true
        });

        Provider.register(name, this._store);
    }

    _render() {
        this.shadowRoot.innerHTML = `
            <style>
                :host {
                    display: contents;
                }
            </style>
            <slot></slot>
        `;
    }

    _subscribe() {
        this._unsubscribe = this._store.subscribe((state, prevState) => {
            if (!this._connected) return;

            this.dispatchEvent(new CustomEvent('stateChange', {
                bubbles: true,
                composed: true,
                detail: { state, prevState }
            }));

            this._notifyChildren(state);
        });
    }

    _notifyChildren(state) {
        const slot = this.shadowRoot.querySelector('slot');
        if (!slot) return;

        const children = slot.assignedElements();
        children.forEach(child => {
            if (typeof child.setStoreState === 'function') {
                child.setStoreState(state);
            }
        });
    }

    get store() {
        return this._store;
    }

    dispatch(action) {
        return this._store?.setState(
            typeof action === 'function' ? action : { [action.type]: action.payload },
            { type: action.type }
        );
    }

    getState() {
        return this._store?.state;
    }
}

const Provider = new StoreProvider();

function createStore(name, initialState, options = {}) {
    const store = new ReactiveStore(initialState, {
        namespace: name,
        ...options
    });

    Provider.register(name, store);

    return store;
}

function useStore(name) {
    let store = Provider.get(name);

    if (!store) {
        store = createStore(name);
    }

    return {
        store,
        subscribe: (listener) => store.subscribe(listener),
        dispatch: (updates, meta) => store.setState(updates, meta),
        getState: () => store.state
    };
}

function createSlice(config) {
    return new Slice(
        config.name,
        config.initialState,
        config.reducers
    );
}

function combineReducers(slices) {
    return (state = {}, action) => {
        const result = {};
        let changed = false;

        Object.entries(slices).forEach(([name, slice]) => {
            const sliceState = state[name] || slice.initialState;
            const newSliceState = slice.reducer(sliceState, action);

            if (newSliceState !== sliceState) {
                result[name] = newSliceState;
                changed = true;
            } else {
                result[name] = sliceState;
            }
        });

        return changed ? result : state;
    };
}

customElements.define('universal-store', WebComponentStore);

export {
    Store,
    ReactiveStore,
    Slice,
    StoreProvider,
    WebComponentStore,
    Provider,
    createStore,
    useStore,
    createSlice,
    combineReducers
};
