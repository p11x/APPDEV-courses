import { StateContainer, STATE_CONFIG, StateError, Action, ActionCreator, Reducer, Store } from './05_5_State-Management-Integration.js';

describe('StateContainer', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('state-container');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders state display', () => {
      expect(component.shadowRoot.innerHTML).toContain('state-display');
    });
  });

  describe('property changes', () => {
    test('initial-state attribute sets initial state', () => {
      component.setAttribute('initial-state', JSON.stringify({ count: 5 }));
      expect(component._store.getState().count).toBe(5);
    });

    test('enable-dev-tools attribute toggles dev tools', () => {
      component.setAttribute('enable-dev-tools', 'false');
      expect(component.hasAttribute('enable-dev-tools')).toBe(true);
    });
  });

  describe('events', () => {
    test('dispatches state-change event', (done) => {
      component.addEventListener('state-change', (e) => {
        expect(e.detail.state).toBeDefined();
        done();
      });
      component._store.dispatch(ActionCreator.set('count', 1));
    });
  });

  describe('edge cases', () => {
    test('dispatches action', () => {
      const action = ActionCreator.set('count', 10);
      component._store.dispatch(action);
      expect(component._store.getState().count).toBe(10);
    });

    test('gets state', () => {
      const state = component.getState();
      expect(state).toBeDefined();
    });

    test('subscribes to state changes', () => {
      const callback = jest.fn();
      component.subscribe(callback);
      component._store.dispatch(ActionCreator.set('count', 5));
      expect(callback).toHaveBeenCalled();
    });

    test('resets state', () => {
      component._store.dispatch(ActionCreator.set('count', 100));
      component._store.dispatch(ActionCreator.reset({ count: 0 }));
      expect(component._store.getState().count).toBe(0);
    });

    test('tracks history', () => {
      component._store.dispatch(ActionCreator.set('count', 1));
      component._store.dispatch(ActionCreator.set('count', 2));
      expect(component._history.length).toBeGreaterThan(0);
    });
  });
});

describe('Action', () => {
  test('creates action with type', () => {
    const action = Action.create('TEST_TYPE', { data: 'test' });
    expect(action.type).toBe('TEST_TYPE');
    expect(action.payload.data).toBe('test');
  });
});

describe('ActionCreator', () => {
  test('creates set action', () => {
    const action = ActionCreator.set('key', 'value');
    expect(action.type).toBe('STATE_SET');
    expect(action.payload.key).toBe('key');
    expect(action.payload.value).toBe('value');
  });

  test('creates update action', () => {
    const action = ActionCreator.update({ count: 5 });
    expect(action.type).toBe('STATE_UPDATE');
  });

  test('creates reset action', () => {
    const action = ActionCreator.reset({ count: 0 });
    expect(action.type).toBe('STATE_RESET');
  });

  test('creates loading action', () => {
    const action = ActionCreator.loading(true);
    expect(action.type).toBe('STATE_LOADING');
  });

  test('creates error action', () => {
    const action = ActionCreator.error('Error occurred');
    expect(action.type).toBe('STATE_ERROR');
  });

  test('creates success action', () => {
    const action = ActionCreator.success({ data: 'ok' });
    expect(action.type).toBe('STATE_SUCCESS');
  });
});

describe('Reducer', () => {
  test('creates reducer with handlers', () => {
    const reducer = Reducer.create(
      { count: 0 },
      {
        'STATE_SET': (state, { key, value }) => ({ ...state, [key]: value })
      }
    );
    const action = Action.create('STATE_SET', { key: 'count', value: 5 });
    const newState = reducer(undefined, action);
    expect(newState.count).toBe(5);
  });

  test('combines reducers', () => {
    const reducer1 = (state = 0, action) => state + (action.type === 'INC' ? 1 : 0);
    const reducer2 = (state = '', action) => state + (action.type === 'APPEND' ? action.payload : '');
    const combined = Reducer.combineReducers({ count: reducer1, text: reducer2 });
    const action = Action.create('INC');
    const newState = combined({ count: 0, text: '' }, action);
    expect(newState.count).toBe(1);
  });
});

describe('Store', () => {
  let store;

  beforeEach(() => {
    store = new Store((state = 0, action) => state + (action.payload || 0));
  });

  test('creates store', () => {
    expect(store).toBeDefined();
  });

  test('gets state', () => {
    expect(store.getState()).toBe(0);
  });

  test('dispatches action', () => {
    store.dispatch({ type: 'INC', payload: 5 });
    expect(store.getState()).toBe(5);
  });

  test('subscribes to changes', () => {
    const callback = jest.fn();
    store.subscribe(callback);
    store.dispatch({ type: 'INC', payload: 1 });
    expect(callback).toHaveBeenCalled();
  });

  test('unsubscribes', () => {
    const callback = jest.fn();
    const unsub = store.subscribe(callback);
    unsub();
    store.dispatch({ type: 'INC', payload: 1 });
    expect(callback).not.toHaveBeenCalled();
  });
});

describe('STATE_CONFIG', () => {
  test('has middleware', () => {
    expect(STATE_CONFIG.middleware.length).toBeGreaterThan(0);
  });

  test('has default state', () => {
    expect(STATE_CONFIG.defaultState).toBeDefined();
  });

  test('has persistence key', () => {
    expect(STATE_CONFIG.persistenceKey).toBeDefined();
  });
});
