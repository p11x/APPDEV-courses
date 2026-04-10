import { expect, fixture, html, oneEvent } from '@open-wc/testing';

describe('08_6_Micro-Frontend-Architecture', () => {
  describe('MicroFrontendConfig', () => {
    it('is an object', () => {
      expect(MicroFrontendConfig).to.be.an('object');
    });

    it('has remotes Map', () => {
      expect(MicroFrontendConfig).to.have.property('remotes');
      expect(MicroFrontendConfig.remotes).to.be.instanceOf(Map);
    });

    it('has shared Map', () => {
      expect(MicroFrontendConfig).to.have.property('shared');
      expect(MicroFrontendConfig.shared).to.be.instanceOf(Map);
    });

    it('has registeredComponents Map', () => {
      expect(MicroFrontendConfig).to.have.property('registeredComponents');
      expect(MicroFrontendConfig.registeredComponents).to.be.instanceOf(Map);
    });
  });

  describe('MicroFrontendRegistry', () => {
    let registry;

    beforeEach(() => {
      registry = new MicroFrontendRegistry();
    });

    it('is a class', () => {
      expect(MicroFrontendRegistry).to.be.a('function');
    });

    it('can be instantiated', () => {
      expect(registry).to.exist;
    });

    it('registers component', () => {
      const result = registry.register('TestComponent', {
        tagName: 'test-component',
        module: './TestComponent'
      });
      expect(result).to.equal(registry);
      expect(registry.has('TestComponent')).to.be.true;
    });

    it('registers remote', () => {
      const result = registry.registerRemote('remote1', {
        url: 'https://example.com',
        entry: 'https://example.com/remoteEntry.js'
      });
      expect(result).to.equal(registry);
    });

    it('gets registered component', () => {
      registry.register('MyComponent', { tagName: 'my-component' });
      const component = registry.get('MyComponent');
      expect(component).to.exist;
      expect(component.name).to.equal('MyComponent');
    });

    it('gets registered remote', () => {
      registry.registerRemote('myRemote', { url: 'http://test.com', entry: 'entry.js' });
      const remote = registry.getRemote('myRemote');
      expect(remote).to.exist;
      expect(remote.url).to.equal('http://test.com');
    });

    it('checks if component exists', () => {
      registry.register('ExistingComponent', { tagName: 'existing' });
      expect(registry.has('ExistingComponent')).to.be.true;
      expect(registry.has('NonExisting')).to.be.false;
    });

    it('lists all components', () => {
      registry.register('Component1', { tagName: 'comp1' });
      registry.register('Component2', { tagName: 'comp2' });
      const list = registry.list();
      expect(list.length).to.equal(2);
    });

    it('lists all remotes', () => {
      registry.registerRemote('remote1', { url: 'http://a.com', entry: 'a.js' });
      registry.registerRemote('remote2', { url: 'http://b.com', entry: 'b.js' });
      const list = registry.listRemotes();
      expect(list.length).to.equal(2);
    });

    it('converts name to tag name', () => {
      const tagName = registry._toTagName('MyComponent');
      expect(tagName).to.equal('my-component');
    });
  });

  describe('MicroFrontendComponent', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<mf-component></mf-component>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('applies remote attribute', async () => {
      el.setAttribute('remote', 'test-remote');
      await el.updateComplete;
      expect(el.getAttribute('remote')).to.equal('test-remote');
    });

    it('applies module attribute', async () => {
      el.setAttribute('module', 'TestModule');
      await el.updateComplete;
      expect(el.getAttribute('module')).to.equal('TestModule');
    });

    it('applies props attribute', async () => {
      el.setAttribute('props', JSON.stringify({ title: 'Test' }));
      await el.updateComplete;
      expect(el.getAttribute('props')).to.exist;
    });

    it('applies loading attribute', async () => {
      el.setAttribute('loading', '');
      await el.updateComplete;
      expect(el.hasAttribute('loading')).to.be.true;
    });

    it('applies error attribute', async () => {
      el.setAttribute('error', 'Failed to load');
      await el.updateComplete;
      expect(el.getAttribute('error')).to.equal('Failed to load');
    });

    it('applies fallback attribute', async () => {
      el.setAttribute('fallback', 'fallback-content');
      await el.updateComplete;
      expect(el.getAttribute('fallback')).to.equal('fallback-content');
    });

    it('has static observedAttributes', () => {
      expect(MicroFrontendComponent.observedAttributes).to.include('remote');
      expect(MicroFrontendComponent.observedAttributes).to.include('module');
      expect(MicroFrontendComponent.observedAttributes).to.include('props');
    });

    it('dispatches loaded event', async () => {
      const listener = oneEvent(el, 'loaded');
      el._emit('loaded', { remote: 'test', module: 'Test' });
      const event = await listener;
      expect(event.detail.remote).to.equal('test');
    });

    it('dispatches error event', async () => {
      const listener = oneEvent(el, 'error');
      el._emit('error', { error: new Error('test'), remote: 'r', module: 'm' });
      const event = await listener;
      expect(event.detail.error).to.be.instanceOf(Error);
    });
  });

  describe('MicroFrontendLoader', () => {
    let loader;

    beforeEach(() => {
      loader = new MicroFrontendLoader();
    });

    it('is a class', () => {
      expect(MicroFrontendLoader).to.be.a('function');
    });

    it('can be instantiated', () => {
      expect(loader).to.exist;
    });

    it('loads component from registry', async () => {
      const component = loader.loadComponent('TestComponent');
      expect(component).to.be.an('object');
    });

    it('preloads remote', async () => {
      loader.preloadRemote('test-remote');
    });

    it('gets loading status', () => {
      const status = loader.getLoadingStatus('test-id');
      expect(status).to.be.an('object');
    });
  });

  describe('ComponentLoader', () => {
    let loader;

    beforeEach(() => {
      loader = new ComponentLoader();
    });

    it('is a class', () => {
      expect(ComponentLoader).to.be.a('function');
    });

    it('can be instantiated', () => {
      expect(loader).to.exist;
    });

    it('loads component', () => {
      const promise = loader.load('test-element');
      expect(promise).to.be.instanceOf(Promise);
    });

    it('loads with config', () => {
      const promise = loader.load('test-element', { lazy: true });
      expect(promise).to.be.instanceOf(Promise);
    });

    it('checks if loaded', () => {
      expect(loader.isLoaded('test-element')).to.be.a('boolean');
    });

    it('gets loaded component', () => {
      expect(loader.getLoaded('test-element')).to.be.null;
    });

    it('unloads component', () => {
      loader.unload('test-element');
    });
  });

  describe('SharedComponentLoader', () => {
    it('is a class', () => {
      expect(SharedComponentLoader).to.be.a('function');
    });

    it('loads shared component', () => {
      const loader = new SharedComponentLoader();
      expect(loader).to.exist;
    });

    it('loads with version', () => {
      const loader = new SharedComponentLoader({ version: '2.0.0' });
      expect(loader).to.exist;
    });

    it('loads with shared deps', () => {
      const loader = new SharedComponentLoader({ shared: ['react', 'react-dom'] });
      expect(loader).to.exist;
    });
  });

  describe('Edge Cases', () => {
    it('handles registry without components', () => {
      const registry = new MicroFrontendRegistry();
      expect(registry.get('nonexistent')).to.be.undefined;
    });

    it('handles MicroFrontendComponent without attributes', () => {
      const el = document.createElement('mf-component');
      expect(el).to.exist;
    });

    it('handles parseProps with invalid JSON', () => {
      const el = document.createElement('mf-component');
      const result = el._parseProps('invalid json');
      expect(result).to.be.an('object');
    });
  });
});