import { expect, fixture, html } from '@open-wc/testing';

describe('08_8_State-Management-Cross-Framework', () => {
  describe('Store', () => {
    let store;

    beforeEach(() => {
      store = new Store({ count: 0, name: 'test' });
    });

    it('is a class extending EventTarget', () => {
      expect(Store).to.be.a('function');
    });

    it('can be instantiated with initial state', () => {
      expect(store.state).to.deep.equal({ count: 0, name: 'test' });
    });

    it('can be instantiated with options', () => {
      const storeWithOptions = new Store({}, { maxHistory: 100, immutable: false, namespace: 'my-store' });
      expect(storeWithOptions).to.exist;
    });

    it('returns frozen state when immutable', () => {
      const state = store.state;
      expect(Object.isFrozen(state)).to.be.true;
    });

    it('returns mutable state when not immutable', () => {
      const mutableStore = new Store({}, { immutable: false });
      const state = mutableStore.state;
      expect(Object.isFrozen(state)).to.be.false;
    });

    it('gets state by path', () => {
      expect(store.getState('count')).to.equal(0);
      expect(store.getState('name')).to.equal('test');
    });

    it('gets nested state by dot notation', () => {
      store.setState({ user: { name: 'John' } });
      expect(store.getState('user.name')).to.equal('John');
    });

    it('sets state with object', () => {
      store.setState({ count: 5 });
      expect(store.state.count).to.equal(5);
    });

    it('sets state with function', () => {
      store.setState((prevState) => ({ count: prevState.count + 1 }));
      expect(store.state.count).to.equal(1);
    });

    it('subscribes to state changes', () => {
      const listener = (newState, oldState) => {};
      const unsubscribe = store.subscribe(listener);
      expect(unsubscribe).to.be.a('function');
    });

    it('subscribes with selector', () => {
      const listener = (selectedState) => {};
      store.subscribe(listener, (state) => state.count);
    });

    it('unsubscribes from state changes', () => {
      const listener = () => {};
      const unsubscribe = store.subscribe(listener);
      unsubscribe();
    });

    it('adds middleware', () => {
      const middleware = (next) => (updates) => next(updates);
      store.addMiddleware(middleware);
    });

    it('removes middleware', () => {
      const middleware = () => {};
      store.addMiddleware(middleware);
      store.removeMiddleware(middleware);
    });

    it('registers computed values', () => {
      store.setState({ a: 1, b: 2 });
      store.registerComputed('sum', (state) => state.a + state.b);
      const sum = store.getComputed('sum');
      expect(sum).to.equal(3);
    });

    it('gets computed value', () => {
      store.setState({ x: 10 });
      store.registerComputed('doubled', (state) => state.x * 2);
      expect(store.getComputed('doubled')).to.equal(20);
    });

    it('undoes state', () => {
      store.setState({ value: 'initial' });
      store.setState({ value: 'changed' });
      store.undo();
      expect(store.state.value).to.equal('initial');
    });

    it('redoes state', () => {
      store.setState({ value: 'initial' });
      store.setState({ value: 'changed' });
      store.undo();
      store.redo();
      expect(store.state.value).to.equal('changed');
    });

    it('checks if can undo', () => {
      expect(store.canUndo()).to.be.false;
      store.setState({ value: 'test' });
      expect(store.canUndo()).to.be.true;
    });

    it('checks if can redo', () => {
      store.setState({ value: 'test' });
      store.undo();
      expect(store.canRedo()).to.be.true;
    });

    it('creates derived store', () => {
      const derived = store.derived((state) => ({ doubled: state.count * 2 }));
      expect(derived).to.be.instanceOf(Store);
    });

    it('resets to initial state', () => {
      store.setState({ value: 'changed' });
      store.reset();
      expect(store.state).to.deep.equal({ count: 0, name: 'test' });
    });

    it('dispatches state-change event', () => {
      store.setState({ count: 1 });
    });

    it('returns false when middleware blocks update', () => {
      const blockingMiddleware = () => false;
      store.addMiddleware(blockingMiddleware);
      const result = store.setState({ blocked: true });
      expect(result).to.be.false;
    });
  });

  describe('StoreProvider', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<store-provider name="app-store"></store-provider>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('applies name attribute', () => {
      expect(el.getAttribute('name')).to.equal('app-store');
    });

    it('applies store attribute', async () => {
      el.setAttribute('store', 'custom-store');
      await el.updateComplete;
      expect(el.getAttribute('store')).to.equal('custom-store');
    });

    it('gets store instance', () => {
      const storeInstance = el.getStore();
      expect(storeInstance).to.be.instanceOf(Store);
    });

    it('sets state on store', () => {
      el.setState({ newValue: 'test' });
    });
  });

  describe('UniversalStore', () => {
    it('is a class', () => {
      expect(UniversalStore).to.be.a('function');
    });

    it('creates store with default config', () => {
      const store = UniversalStore.create({ key: 'value' });
      expect(store.state).to.deep.equal({ key: 'value' });
    });

    it('creates store with custom namespace', () => {
      const store = UniversalStore.create({ data: 1 }, { namespace: 'custom' });
      expect(store).to.exist;
    });

    it('gets global store', () => {
      const globalStore = UniversalStore.getGlobal('global-ns');
      expect(globalStore).to.be.instanceOf(Store);
    });

    it('registers store globally', () => {
      const store = UniversalStore.create({ test: true });
      UniversalStore.register('test-register', store);
      const retrieved = UniversalStore.get('test-register');
      expect(retrieved).to.equal(store);
    });

    it('gets registered store', () => {
      const store = UniversalStore.create({ registered: true });
      UniversalStore.register('my-store', store);
      expect(UniversalStore.get('my-store')).to.equal(store);
    });

    it('checks if store exists', () => {
      const store = UniversalStore.create({ exists: true });
      UniversalStore.register('exists-store', store);
      expect(UniversalStore.has('exists-store')).to.be.true;
      expect(UniversalStore.has('nonexistent-store')).to.be.false;
    });

    it('creates slice of store', () => {
      const store = UniversalStore.create({ user: { name: 'John', age: 30 } });
      const slice = store.slice('user');
      expect(slice.state).to.deep.equal({ name: 'John', age: 30 });
    });
  });

  describe('StateObserver', () => {
    it('is a class', () => {
      expect(StateObserver).to.be.a('function');
    });

    it('can be instantiated with target', () => {
      const target = document.createElement('div');
      const observer = new StateObserver(target);
      expect(observer).to.exist;
    });

    it('observes state changes', () => {
      const target = document.createElement('div');
      const observer = new StateObserver(target);
      const handler = (newVal, oldVal) => {};
      observer.observe('value', handler);
    });

    it('unobserves state changes', () => {
      const target = document.createElement('div');
      const observer = new StateObserver(target);
      const handler = () => {};
      observer.observe('value', handler);
      observer.unobserve('value', handler);
    });

    it('disconnects observer', () => {
      const target = document.createElement('div');
      const observer = new StateObserver(target);
      observer.disconnect();
    });
  });

  describe('CrossFrameworkConnector', () => {
    it('is a class', () => {
      expect(CrossFrameworkConnector).to.be.a('function');
    });

    it('can be instantiated', () => {
      const connector = new CrossFrameworkConnector();
      expect(connector).to.exist;
    });

    it('connects framework store', () => {
      const connector = new CrossFrameworkConnector();
      const mockStore = { subscribe: () => {} };
      connector.connectFrameworkStore('react', mockStore);
    });

    it('connects WebComponent store', () => {
      const connector = new CrossFrameworkConnector();
      const wcStore = new Store({ wcValue: 'test' });
      connector.connectWebComponentStore('wc-id', wcStore);
    });

    it('syncs state between frameworks', () => {
      const connector = new CrossFrameworkConnector();
      const reactStore = { getState: () => ({ value: 1 }), subscribe: () => {} };
      const wcStore = new Store({});
      connector.connectFrameworkStore('react', reactStore);
      connector.connectWebComponentStore('wc', wcStore);
      connector.syncState('react', 'wc');
    });

    it('disconnects framework', () => {
      const connector = new CrossFrameworkConnector();
      connector.connectFrameworkStore('vue', {});
      connector.disconnectFramework('vue');
    });
  });

  describe('StateSync', () => {
    it('is a class', () => {
      expect(StateSync).to.be.a('function');
    });

    it('can be instantiated', () => {
      const sync = new StateSync('sync-channel');
      expect(sync).to.exist;
    });

    it('creates sync channel', () => {
      const sync = new StateSync('my-channel');
      expect(sync.channel).to.equal('my-channel');
    });

    it('adds participant', () => {
      const sync = new StateSync('test');
      const store = new Store({});
      sync.addParticipant('participant1', store);
    });

    it('removes participant', () => {
      const sync = new StateSync('test');
      const store = new Store({});
      sync.addParticipant('p1', store);
      sync.removeParticipant('p1');
    });

    it('broadcasts state change', () => {
      const sync = new StateSync('broadcast-test');
      const store = new Store({ value: 1 });
      sync.addParticipant('p1', store);
      sync.broadcast('p1', { value: 2 });
    });

    it('pauses sync', () => {
      const sync = new StateSync('test');
      sync.pause();
      expect(sync.isPaused()).to.be.true;
    });

    it('resumes sync', () => {
      const sync = new StateSync('test');
      sync.pause();
      sync.resume();
      expect(sync.isPaused()).to.be.false;
    });
  });

  describe('Edge Cases', () => {
    it('handles Store with no initial state', () => {
      const store = new Store();
      expect(store.state).to.deep.equal({});
    });

    it('handles Store with empty options', () => {
      const store = new Store({}, {});
      expect(store).to.exist;
    });

    it('handles StoreProvider without store attribute', () => {
      const el = document.createElement('store-provider');
      expect(el).to.exist;
    });
  });
});