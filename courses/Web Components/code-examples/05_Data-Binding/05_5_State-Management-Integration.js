/**
 * State Management Integration
 * Implements Redux/Vuex-style state management patterns for web components
 * @module data-binding/05_5_State-Management-Integration
 * @version 1.0.0
 * @example <state-container initial-state='{"count":0}'></state-container>
 */

const STATE_CONFIG = {
  middleware: ['logger', 'thunk', 'promise'],
  defaultState: { count: 0, data: null, loading: false, error: null },
  enableDevTools: true,
  enablePersistence: true,
  persistenceKey: 'web-component-state',
  maxHistorySize: 50
};

class StateError extends Error {
  constructor(message, code = 'STATE_ERROR') {
    super(message);
    this.name = 'StateError';
    this.code = code;
  }
}

const ActionTypes = {
  SET: 'STATE_SET',
  UPDATE: 'STATE_UPDATE',
  RESET: 'STATE_RESET',
  LOADING: 'STATE_LOADING',
  ERROR: 'STATE_ERROR',
  SUCCESS: 'STATE_SUCCESS'
};

class Action {
  constructor(type, payload = {}) {
    this.type = type;
    this.payload = payload;
    this.timestamp = Date.now();
  }

  static create(type, payload) {
    return new Action(type, payload);
  }
}

class ActionCreator {
  static set(key, value) {
    return Action.create(ActionTypes.SET, { key, value });
  }

  static update(changes) {
    return Action.create(ActionTypes.UPDATE, { changes });
  }

  static reset(initialState) {
    return Action.create(ActionTypes.RESET, { initialState });
  }

  static loading(loading = true) {
    return Action.create(ActionTypes.LOADING, { loading });
  }

  static error(error) {
    return Action.create(ActionTypes.ERROR, { error });
  }

  static success(data) {
    return Action.create(ActionTypes.SUCCESS, { data });
  }
}

class Reducer {
  static create(initialState, handlers) {
    return (state = initialState, action) => {
      const handler = handlers[action.type];
      if (handler) {
        return handler(state, action.payload);
      }
      return state;
    };
  }

  static combineReducers(reducers) {
    return (state = {}, action) => {
      const nextState = {};
      let hasChanged = false;

      for (const key in reducers) {
        const reducer = reducers[key];
        const previousState = state[key];
        const nextStateForKey = reducer(previousState, action);
        nextState[key] = nextStateForKey;
        hasChanged = hasChanged || nextStateForKey !== previousState;
      }

      return hasChanged ? nextState : state;
    };
  }
}

function loggerMiddleware(store) {
  return (next) => (action) => {
    console.log('[State] Dispatching:', action.type, action.payload);
    const result = next(action);
    console.log('[State] New state:', store.getState());
    return result;
  };
}

function thunkMiddleware(store) {
  return (next) => (action) => {
    if (typeof action === 'function') {
      return action(store.dispatch, store.getState);
    }
    return next(action);
  };
}

function promiseMiddleware(store) {
  return (next) => (action) => {
    if (action.payload instanceof Promise) {
      return action.payload
        .then(data => store.dispatch({ ...action, payload: { ...action.payload, data } }))
        .catch(error => store.dispatch(ActionCreator.error(error.message)));
    }
    return next(action);
  };
}

function persistenceMiddleware(store) {
  if (!STATE_CONFIG.enablePersistence) return (next) => next;

  const savedState = localStorage.getItem(STATE_CONFIG.persistenceKey);
  if (savedState) {
    try {
      const parsed = JSON.parse(savedState);
      store.dispatch(ActionCreator.update(parsed));
    } catch (e) {
      console.error('Failed to restore state:', e);
    }
  }

  return (next) => (action) => {
    const result = next(action);
    const state = store.getState();
    localStorage.setItem(STATE_CONFIG.persistenceKey, JSON.stringify(state));
    return result;
  };
}

class Store {
  constructor(reducer, preloadedState = {}, middleware = []) {
    this._reducer = reducer;
    this._state = preloadedState;
    this._listeners = [];
    this._middlewares = middleware;
    this._isDispatching = false;
  }

  getState() {
    return this._state;
  }

  dispatch(action) {
    if (this._isDispatching) {
      throw new StateError('Reducers may not dispatch', 'DISPATCH_IN_REDUCER');
    }

    this._isDispatching = true;
    try {
      this._state = this._reducer(this._state, action);
    } finally {
      this._isDispatching = false;
    }

    this._notifyListeners();
    return action;
  }

  subscribe(listener) {
    this._listeners.push(listener);
    return () => {
      this._listeners = this._listeners.filter(l => l !== listener);
    };
  }

  replaceReducer(nextReducer) {
    this._reducer = nextReducer;
  }

  _notifyListeners() {
    this._listeners.forEach(listener => {
      listener(this._state);
    });
  }
}

function createStore(reducer, preloadedState, middleware) {
  return new Store(reducer, preloadedState, middleware);
}

const rootReducer = Reducer.create(STATE_CONFIG.defaultState, {
  [ActionTypes.SET]: (state, { key, value }) => ({
    ...state,
    [key]: value
  }),
  [ActionTypes.UPDATE]: (state, { changes }) => ({
    ...state,
    ...changes
  }),
  [ActionTypes.RESET]: (state, { initialState }) => ({
    ...initialState
  }),
  [ActionTypes.LOADING]: (state, { loading }) => ({
    ...state,
    loading
  }),
  [ActionTypes.ERROR]: (state, { error }) => ({
    ...state,
    error,
    loading: false
  }),
  [ActionTypes.SUCCESS]: (state, { data }) => ({
    ...state,
    data,
    loading: false,
    error: null
  })
});

const defaultMiddleware = [
  loggerMiddleware,
  thunkMiddleware,
  promiseMiddleware
];

if (STATE_CONFIG.enablePersistence) {
  defaultMiddleware.push(persistenceMiddleware);
}

const store = createStore(rootReducer, STATE_CONFIG.defaultState, defaultMiddleware);

class StateContainerElement extends HTMLElement {
  static get observedAttributes() {
    return ['initial-state', 'persistence', 'dev-tools'];
  }

  constructor() {
    super();
    this._localStore = null;
    this._initialState = { ...STATE_CONFIG.defaultState };
    this._enablePersistence = STATE_CONFIG.enablePersistence;
    this._enableDevTools = STATE_CONFIG.enableDevTools;
    this._unsubscribe = null;
    this._actionHistory = [];
    
    this._initStore();
    this._attachShadowDOM();
  }

  _initStore() {
    this._localStore = createStore(
      rootReducer,
      this._initialState,
      defaultMiddleware
    );
  }

  _attachShadowDOM() {
    const shadow = this.attachShadow({ mode: 'open' });
    
    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
        padding: 20px;
        border: 2px solid #ff5722;
        border-radius: 10px;
        background: linear-gradient(135deg, #fff3e0 0%, #fff 100%);
        font-family: 'Inter', system-ui, sans-serif;
      }

      :host([persistence="false"]) {
        border-color: #795548;
      }

      .container-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid rgba(255, 87, 34, 0.3);
      }

      .container-title {
        font-size: 16px;
        font-weight: 700;
        color: #bf360c;
      }

      .container-badge {
        font-size: 11px;
        padding: 4px 10px;
        background: #ff5722;
        color: white;
        border-radius: 12px;
        font-weight: 500;
      }

      .state-display {
        margin-bottom: 16px;
        padding: 16px;
        background: white;
        border-radius: 8px;
        border: 1px solid #ffe0b2;
      }

      .state-display-title {
        font-size: 12px;
        font-weight: 600;
        color: #e64a19;
        margin-bottom: 12px;
      }

      .state-item {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 8px 12px;
        margin: 4px 0;
        background: #fafafa;
        border-radius: 6px;
      }

      .state-key {
        font-size: 12px;
        color: #795548;
        font-weight: 500;
      }

      .state-value {
        font-family: 'Fira Code', monospace;
        font-size: 12px;
        color: #ff5722;
        max-width: 150px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }

      .state-value.loading {
        color: #2196f3;
      }

      .state-value.error {
        color: #f44336;
      }

      .state-value.data {
        color: #4caf50;
      }

      .controls {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 8px;
        margin-bottom: 16px;
      }

      .control-btn {
        padding: 10px 12px;
        border: 1px solid #ff8a65;
        border-radius: 6px;
        background: white;
        color: #d84315;
        font-size: 12px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
      }

      .control-btn:hover {
        background: #fbe9e7;
        border-color: #ff5722;
      }

      .control-btn:active {
        background: #ffccbc;
        transform: scale(0.98);
      }

      .action-history {
        background: #f5f5f5;
        border-radius: 6px;
        padding: 12px;
        max-height: 150px;
        overflow-y: auto;
      }

      .history-title {
        font-size: 12px;
        font-weight: 600;
        color: #616161;
        margin-bottom: 8px;
      }

      .history-item {
        padding: 4px 8px;
        margin: 2px 0;
        background: white;
        border-radius: 4px;
        font-size: 11px;
        font-family: monospace;
        display: flex;
        justify-content: space-between;
      }

      .history-item .type {
        color: #ff5722;
        font-weight: 600;
      }

      .history-item .payload {
        color: #795548;
      }

      .dispatch-panel {
        margin-top: 12px;
        padding: 12px;
        background: #e8f5e9;
        border-radius: 6px;
      }

      .dispatch-label {
        font-size: 11px;
        color: #2e7d32;
        margin-bottom: 6px;
      }

      .dispatch-input {
        width: 100%;
        padding: 8px 12px;
        border: 1px solid #a5d6a7;
        border-radius: 4px;
        font-size: 12px;
        font-family: monospace;
        box-sizing: border-box;
      }

      .dispatch-buttons {
        display: flex;
        gap: 8px;
        margin-top: 8px;
      }

      .dispatch-btn {
        flex: 1;
        padding: 8px;
        border: 1px solid #4caf50;
        border-radius: 4px;
        background: #4caf50;
        color: white;
        font-size: 11px;
        font-weight: 600;
        cursor: pointer;
      }

      .dispatch-btn:hover {
        background: #43a047;
      }

      .metrics {
        display: flex;
        gap: 12px;
        margin-top: 12px;
      }

      .metric {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 8px;
        background: #e3f2fd;
        border-radius: 6px;
      }

      .metric-label {
        font-size: 10px;
        color: #1565c0;
        text-transform: uppercase;
      }

      .metric-value {
        font-size: 18px;
        font-weight: 700;
        color: #1976d2;
      }

      @keyframes pulse {
        0%, 100% { transform: scale(1); }
        50% { transform: scale(1.02); }
      }

      .updating {
        animation: pulse 0.3s ease-in-out;
      }
    `;

    const container = document.createElement('div');
    container.innerHTML = `
      <div class="container-header">
        <span class="container-title">State Container</span>
        <span class="container-badge">REDUX</span>
      </div>
      
      <div class="state-display">
        <div class="state-display-title">Current State</div>
        <div id="state-items"></div>
      </div>
      
      <div class="controls">
        <button class="control-btn" id="btn-increment">+1</button>
        <button class="control-btn" id="btn-decrement">-1</button>
        <button class="control-btn" id="btn-loading">Load</button>
        <button class="control-btn" id="btn-reset">Reset</button>
      </div>
      
      <div class="dispatch-panel">
        <div class="dispatch-label">Custom Dispatch</div>
        <input class="dispatch-input" id="dispatch-input" placeholder='{"key":"value"}' />
        <div class="dispatch-buttons">
          <button class="dispatch-btn" id="btn-dispatch-set">SET</button>
          <button class="dispatch-btn" id="btn-dispatch-update">UPDATE</button>
          <button class="dispatch-btn" id="btn-dispatch-error">ERROR</button>
        </div>
      </div>
      
      <div class="action-history">
        <div class="history-title">Action History</div>
        <div id="action-history"></div>
      </div>
      
      <div class="metrics">
        <div class="metric">
          <span class="metric-label">Version</span>
          <span class="metric-value" id="state-version">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Actions</span>
          <span class="metric-value" id="action-count">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Listeners</span>
          <span class="metric-value" id="listener-count">0</span>
        </div>
      </div>
    `;
    
    shadow.appendChild(style);
    shadow.appendChild(container);
  }

  connectedCallback() {
    this._bindEvents();
    this._subscribe();
    this._render();
    this._logAction('INIT', { initialState: this._initialState });
  }

  disconnectedCallback() {
    this._unsubscribe?.();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'initial-state':
        try {
          this._initialState = JSON.parse(newValue);
          this._localStore?.dispatch(ActionCreator.reset(this._initialState));
        } catch (e) {
          console.error('Failed to parse initial state:', e);
        }
        break;
      case 'persistence':
        this._enablePersistence = newValue !== 'false';
        break;
      case 'dev-tools':
        this._enableDevTools = newValue !== 'false';
        break;
    }

    this._render();
  }

  _bindEvents() {
    const shadow = this.shadowRoot;
    
    const btnIncrement = shadow.getElementById('btn-increment');
    const btnDecrement = shadow.getElementById('btn-decrement');
    const btnLoading = shadow.getElementById('btn-loading');
    const btnReset = shadow.getElementById('btn-reset');
    const btnDispatchSet = shadow.getElementById('btn-dispatch-set');
    const btnDispatchUpdate = shadow.getElementById('btn-dispatch-update');
    const btnDispatchError = shadow.getElementById('btn-dispatch-error');
    const dispatchInput = shadow.getElementById('dispatch-input');

    btnIncrement?.addEventListener('click', () => {
      const current = this._localStore.getState();
      this.dispatch(ActionCreator.update({ count: current.count + 1 }));
      this._logAction('INCREMENT', { count: current.count + 1 });
    });

    btnDecrement?.addEventListener('click', () => {
      const current = this._localStore.getState();
      this.dispatch(ActionCreator.update({ count: current.count - 1 }));
      this._logAction('DECREMENT', { count: current.count - 1 });
    });

    btnLoading?.addEventListener('click', () => {
      this.dispatch(ActionCreator.loading(true));
      setTimeout(() => {
        this.dispatch(ActionCreator.success({ data: 'Loaded data' }));
        this._logAction('LOAD_COMPLETE', { data: 'Loaded data' });
      }, 500);
    });

    btnReset?.addEventListener('click', () => {
      this.dispatch(ActionCreator.reset(this._initialState));
      this._logAction('RESET', { initialState: this._initialState });
    });

    btnDispatchSet?.addEventListener('click', () => {
      try {
        const value = JSON.parse(dispatchInput.value);
        for (const [key, val] of Object.entries(value)) {
          this.dispatch(ActionCreator.set(key, val));
        }
        this._logAction('SET', value);
      } catch (e) {
        this.dispatch(ActionCreator.error('Invalid JSON'));
      }
    });

    btnDispatchUpdate?.addEventListener('click', () => {
      try {
        const changes = JSON.parse(dispatchInput.value);
        this.dispatch(ActionCreator.update(changes));
        this._logAction('UPDATE', changes);
      } catch (e) {
        this.dispatch(ActionCreator.error('Invalid JSON'));
      }
    });

    btnDispatchError?.addEventListener('click', () => {
      const errorMessage = dispatchInput.value || 'Test error';
      this.dispatch(ActionCreator.error(errorMessage));
      this._logAction('ERROR', { error: errorMessage });
    });
  }

  _subscribe() {
    this._unsubscribe = this._localStore.subscribe((state) => {
      this._render();
    });
  }

  dispatch(action) {
    return this._localStore.dispatch(action);
  }

  getState() {
    return this._localStore.getState();
  }

  _logAction(type, payload) {
    this._actionHistory.push({
      type,
      payload,
      timestamp: Date.now()
    });

    if (this._actionHistory.length > STATE_CONFIG.maxHistorySize) {
      this._actionHistory.shift();
    }

    this._updateHistoryDisplay();
  }

  _render() {
    const shadow = this.shadowRoot;
    const state = this._localStore.getState();
    
    const stateItems = shadow?.getElementById('state-items');
    if (stateItems) {
      stateItems.innerHTML = Object.entries(state)
        .map(([key, value]) => `
          <div class="state-item">
            <span class="state-key">${key}</span>
            <span class="state-value ${key === 'loading' && value ? 'loading' : ''} ${key === 'error' && value ? 'error' : ''} ${key === 'data' && value ? 'data' : ''}">
              ${typeof value === 'object' ? JSON.stringify(value) : String(value)}
            </span>
          </div>
        `).join('');
    }

    const version = shadow?.getElementById('state-version');
    if (version) {
      version.textContent = String(this._actionHistory.length);
    }

    const actionCount = shadow?.getElementById('action-count');
    if (actionCount) {
      actionCount.textContent = String(this._actionHistory.length);
    }

    const listenerCount = shadow?.getElementById('listener-count');
    if (listenerCount) {
      listenerCount.textContent = '1';
    }

    const stateItemsEl = shadow?.getElementById('state-items');
    if (stateItemsEl) {
      stateItemsEl.classList.add('updating');
      setTimeout(() => stateItemsEl.classList.remove('updating'), 300);
    }
  }

  _updateHistoryDisplay() {
    const shadow = this.shadowRoot;
    const historyEl = shadow?.getElementById('action-history');
    if (!historyEl) return;

    historyEl.innerHTML = this._actionHistory
      .slice(-10)
      .reverse()
      .map(entry => `
        <div class="history-item">
          <span class="type">${entry.type}</span>
          <span class="payload">${JSON.stringify(entry.payload).substring(0, 30)}</span>
        </div>
      `).join('');
  }
}

if (!customElements.get('state-container-element')) {
  customElements.define('state-container-element', StateContainerElement);
}

export { 
  StateContainerElement, 
  Store, 
  createStore, 
  Action, 
  ActionCreator, 
  ActionTypes,
  Reducer,
  STATE_CONFIG,
  StateError 
};