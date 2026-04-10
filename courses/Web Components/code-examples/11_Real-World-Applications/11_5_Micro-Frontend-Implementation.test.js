/**
 * @group unit
 * @group real-world
 */
import { expect, fixture, html } from '@open-wc/testing';
import './11_5_Micro-Frontend-Implementation.js';

describe('MicroAppRegistry', () => {
  let registry;

  beforeEach(() => {
    registry = new MicroAppRegistry();
  });

  it('should register apps', () => {
    registry.register('dashboard', { entry: './dashboard.js', module: 'Dashboard' });
    const app = registry.get('dashboard');
    expect(app.name).to.equal('dashboard');
  });

  it('should unregister apps', () => {
    registry.register('app1', { entry: './app1.js' });
    registry.unregister('app1');
    expect(registry.get('app1')).to.be.undefined;
  });

  it('should get all apps', () => {
    registry.register('app1', { entry: './app1.js' });
    registry.register('app2', { entry: './app2.js' });
    const all = registry.getAll();
    expect(all.length).to.equal(2);
  });

  it('should set loading strategy', () => {
    registry.setLoadingStrategy('lazy');
    expect(registry.loadingStrategy).to.equal('lazy');
  });

  it('should get app by route', () => {
    registry.register('app', { entry: './app.js', activeWhen: '/app' });
    const app = registry.getAppByRoute('/app');
    expect(app).to.exist;
  });
});

describe('ComponentComposer', () => {
  let composer;

  beforeEach(() => {
    composer = new ComponentComposer();
  });

  it('should register components', () => {
    composer.registerComponent('header', {});
    expect(composer.getComponent('header')).to.exist;
  });

  it('should compose components', () => {
    composer.registerComponent('a', {});
    composer.registerComponent('b', {});
    const layout = composer.compose(['a', 'b']);
    expect(layout).to.be.an('array');
  });

  it('should get composed layout', () => {
    composer.compose(['a', 'b']);
    const layout = composer.getLayout();
    expect(layout).to.be.an('array');
  });
});

describe('SharedModuleLoader', () => {
  let loader;

  beforeEach(() => {
    loader = new SharedModuleLoader();
  });

  it('should register shared modules', () => {
    loader.registerShared('lodash', '4.17.21');
    const shared = loader.getShared('lodash');
    expect(shared.version).to.equal('4.17.21');
  });

  it('should load shared modules', async () => {
    loader.registerShared('lodash', '4.17.21');
    const loaded = await loader.loadShared('lodash');
    expect(loaded).to.exist;
  });

  it('should fallback to remote', async () => {
    loader.setRemote('https://cdn.example.com');
    const result = await loader.loadShared('lodash');
    expect(result).to.exist;
  });
});