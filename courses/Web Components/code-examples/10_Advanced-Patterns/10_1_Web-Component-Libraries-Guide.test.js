/**
 * @group unit
 * @group advanced-patterns
 */
import { expect, fixture, html } from '@open-wc/testing';
import './10_1_Web-Component-Libraries-Guide.js';

describe('ComponentRegistry', () => {
  let registry;

  beforeEach(() => {
    registry = new ComponentRegistry();
  });

  it('should initialize with default config', () => {
    expect(registry.config.autoRegister).to.be.true;
    expect(registry.config.lazyLoad).to.be.false;
    expect(registry.config.debug).to.be.false;
  });

  it('should register components', () => {
    const definition = { tagName: 'test-component' };
    const result = registry.register('test', definition);
    expect(result).to.be.true;
    expect(registry.components.size).to.equal(1);
  });

  it('should not register duplicate components', () => {
    const definition = { tagName: 'test-component' };
    registry.register('test', definition);
    const result = registry.register('test', definition);
    expect(result).to.be.false;
  });

  it('should force register with option', () => {
    const definition = { tagName: 'test-component' };
    registry.register('test', definition);
    const result = registry.register('test', definition, { force: true });
    expect(result).to.be.true;
  });

  it('should unregister components', () => {
    const definition = { tagName: 'test-component' };
    registry.register('test', definition);
    const result = registry.unregister('test');
    expect(result).to.be.true;
    expect(registry.components.has('test')).to.be.false;
  });

  it('should not unregister if dependents exist', () => {
    const definition1 = { tagName: 'component-a' };
    const definition2 = { tagName: 'component-b', dependencies: ['component-a'] };
    registry.register('component-a', definition1);
    registry.register('component-b', definition2);
    const result = registry.unregister('component-a');
    expect(result).to.be.false;
  });

  it('should get component by name', () => {
    const definition = { tagName: 'test-component' };
    registry.register('test', definition);
    const comp = registry.get('test');
    expect(comp).to.exist;
    expect(comp.name).to.equal('test');
  });

  it('should get all components', () => {
    registry.register('test1', { tagName: 'test-1' });
    registry.register('test2', { tagName: 'test-2' });
    const all = registry.getAll();
    expect(all.length).to.equal(2);
  });

  it('should resolve dependencies', () => {
    registry.register('a', { tagName: 'a' });
    registry.register('b', { tagName: 'b', dependencies: ['a'] });
    const deps = registry.resolveDependencies('b');
    expect(deps).to.include('a');
    expect(deps).to.include('b');
  });

  it('should set config', () => {
    registry.setConfig({ debug: true });
    expect(registry.config.debug).to.be.true;
  });
});

describe('ComponentLoader', () => {
  let registry;
  let loader;

  beforeEach(() => {
    registry = new ComponentRegistry();
    loader = new ComponentLoader(registry);
  });

  it('should initialize with registry', () => {
    expect(loader.registry).to.equal(registry);
  });

  it('should resolve load order based on dependencies', () => {
    registry.register('a', { tagName: 'a' });
    registry.register('b', { tagName: 'b', dependencies: ['a'] });
    registry.register('c', { tagName: 'c', dependencies: ['b'] });
    const order = loader.resolveLoadOrder(['c']);
    expect(order[0]).to.equal('a');
    expect(order[1]).to.equal('b');
    expect(order[2]).to.equal('c');
  });

  it('should preload components', () => {
    const preloaded = loader.preload(['test']);
    expect(preloaded).to.be.an('array');
    expect(preloaded.length).to.equal(1);
  });
});

describe('LibraryBundler', () => {
  let bundler;

  beforeEach(() => {
    bundler = new LibraryBundler();
  });

  it('should add entries', () => {
    bundler.addEntry('test', './test.js');
    expect(bundler.entries.size).to.equal(1);
  });

  it('should add externals', () => {
    bundler.addExternal('lit');
    expect(bundler.externals.size).to.equal(1);
  });

  it('should use plugins', () => {
    const plugin = { transform: async () => {} };
    bundler.usePlugin(plugin);
    expect(bundler.plugins.length).to.equal(1);
  });

  it('should generate entry points', () => {
    bundler.addEntry('wc-button', './button.js');
    const entries = bundler.generateEntryPoints();
    expect(entries).to.have.property('button');
  });

  it('should get external imports', () => {
    bundler.addExternal('lit');
    const imports = bundler.getExternalImports();
    expect(imports[0].name).to.equal('lit');
  });
});

describe('ComponentPublisher', () => {
  let registry;
  let publisher;

  beforeEach(() => {
    registry = new ComponentRegistry();
    publisher = new ComponentPublisher(registry);
  });

  it('should configure distribution', () => {
    publisher.configure({ npm: true, cdn: true });
    expect(publisher.distribution.npm).to.be.true;
    expect(publisher.distribution.cdn).to.be.true;
  });

  it('should generate package.json', () => {
    registry.register('test', { tagName: 'test' });
    const pkg = publisher.generatePackageJson({ name: 'test-lib' });
    expect(pkg.name).to.equal('test-lib');
    expect(pkg.exports).to.have.property('./test');
  });

  it('should generate UMD exports', () => {
    registry.register('test', { tagName: 'test' });
    const umd = publisher.generateUMD();
    expect(umd.length).to.equal(1);
    expect(umd[0].umd).to.equal('test.umd.js');
  });
});

describe('Global Exports', () => {
  it('should export globalRegistry', () => {
    expect(globalRegistry).to.exist;
  });

  it('should export globalLoader', () => {
    expect(globalLoader).to.exist;
  });

  it('should export globalBundler', () => {
    expect(globalBundler).to.exist;
  });

  it('should export globalPublisher', () => {
    expect(globalPublisher).to.exist;
  });
});