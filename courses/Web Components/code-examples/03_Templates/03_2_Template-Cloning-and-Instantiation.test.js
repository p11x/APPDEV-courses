import { CloneInstantiator, InstancePool, CloneManager } from './03_2_Template-Cloning-and-Instantiation.js';

describe('CloneInstantiator', () => {
  let component;
  let template;

  beforeEach(() => {
    template = document.createElement('template');
    template.id = 'clone-test';
    template.innerHTML = '<div class="item">Test {{name}}</div>';
    document.body.appendChild(template);

    component = document.createElement('clone-instantiator');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
    template?.remove();
  });

  describe('rendering', () => {
    test('initializes with default options', () => {
      expect(CloneInstantiator.defaultOptions.deep).toBe(true);
      expect(CloneInstantiator.defaultOptions.includeEvents).toBe(false);
    });

    test('registers template from DOM', () => {
      component._initializeTemplates();
      expect(component.getTemplate('clone-test')).toBeTruthy();
    });
  });

  describe('property changes', () => {
    test('cloneTemplate with valid template', () => {
      const clone = component.cloneTemplate('clone-test');
      expect(clone).toBeTruthy();
    });

    test('cloneTemplate throws on missing template', () => {
      expect(() => component.cloneTemplate('non-existent')).toThrow();
    });

    test('cloneTemplate with custom options', () => {
      const clone = component.cloneTemplate('clone-test', { deep: true, namespace: 'ns' });
      expect(clone).toBeTruthy();
    });
  });

  describe('events', () => {
    test('dispatches ready event on connect', (done) => {
      component.addEventListener('clone-instantiator:ready', () => done());
      component.connectedCallback();
    });

    test('dispatches cloned event after cloning', (done) => {
      component.addEventListener('clone-instantiator:cloned', (e) => {
        expect(e.detail.cloneId).toBeDefined();
        done();
      });
      component.cloneTemplate('clone-test');
    });
  });

  describe('edge cases', () => {
    test('handles element template source', () => {
      const clone = component.cloneTemplate(template);
      expect(clone).toBeTruthy();
    });

    test('releaseClone removes clone from tracking', () => {
      const clone = component.cloneTemplate('clone-test', { id: 'test-clone' });
      expect(component.getClone('test-clone')).toBeTruthy();
      component.releaseClone('test-clone');
      expect(component.getClone('test-clone')).toBeNull();
    });

    test('clearAllClones removes all tracked clones', () => {
      component.cloneTemplate('clone-test', { id: 'clone-1' });
      component.cloneTemplate('clone-test', { id: 'clone-2' });
      expect(component.getAllClones().length).toBe(2);
      component.clearAllClones();
      expect(component.getAllClones().length).toBe(0);
    });

    test('setMaxClones validates input', () => {
      expect(() => component.setMaxClones(0)).toThrow();
      expect(() => component.setMaxClones(-1)).toThrow();
      expect(() => component.setMaxClones(1.5)).toThrow();
    });

    test('getAllClones returns clone info', () => {
      const clone = component.cloneTemplate('clone-test', { id: 'info-clone' });
      const clones = component.getAllClones();
      expect(clones.length).toBeGreaterThan(0);
      expect(clones[0].id).toBeDefined();
    });
  });
});

describe('InstancePool', () => {
  let pool;

  beforeEach(() => {
    pool = new InstancePool(() => Promise.resolve({ id: Math.random() }), {
      minSize: 0,
      maxSize: 5,
      initialSize: 0
    });
  });

  describe('core functionality', () => {
    test('creates instance pool with factory', () => {
      expect(pool).toBeTruthy();
    });

    test('acquires instance from pool', async () => {
      const instance = await pool.acquire();
      expect(instance).toBeTruthy();
      expect(instance.id).toBeDefined();
    });

    test('releases instance back to pool', async () => {
      const instance = await pool.acquire();
      expect(pool.getActiveInstances()).toContain(instance);
      pool.release(instance);
      expect(pool.getActiveInstances()).not.toContain(instance);
    });
  });

  describe('edge cases', () => {
    test('validates factory function', () => {
      expect(() => new InstancePool('not a function')).toThrow();
    });

    test('getStats returns pool statistics', async () => {
      await pool.acquire();
      const stats = pool.getStats();
      expect(stats.acquired).toBeGreaterThan(0);
    });

    test('has checks for instance', async () => {
      const instance = await pool.acquire();
      expect(pool.has(instance)).toBe(true);
      pool.release(instance);
      expect(pool.has(instance)).toBe(false);
    });

    test('drain disposes all instances', async () => {
      await pool.acquire();
      const count = pool.drain();
      expect(count).toBeGreaterThanOrEqual(0);
      expect(pool.getActiveInstances().length).toBe(0);
    });

    test('dispose cleans up resources', async () => {
      await pool.acquire();
      pool.dispose();
      expect(pool.getActiveInstances().length).toBe(0);
    });
  });
});

describe('CloneManager', () => {
  test('registers and retrieves instantiator', () => {
    const manager = new CloneManager();
    const instantiator = new CloneInstantiator();
    manager.registerInstantiator(instantiator);
    expect(manager.getInstantiator()).toBe(instantiator);
  });

  test('throws when no instantiator registered', () => {
    const manager = new CloneManager();
    expect(() => manager.create('test')).toThrow();
  });
});
