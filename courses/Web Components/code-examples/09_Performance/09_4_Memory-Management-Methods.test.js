/**
 * @group unit
 * @group performance
 */
import { expect, fixture, html } from '@open-wc/testing';
import './09_4_Memory-Management-Methods.js';

describe('MemoryManager', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<memory-manager></memory-manager>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render memory manager UI', () => {
    const h3 = element.shadowRoot.querySelector('h3');
    expect(h3).to.exist;
    expect(h3.textContent).to.include('Memory Management');
  });

  it('should render memory bar', () => {
    const memoryBar = element.shadowRoot.querySelector('.memory-bar');
    expect(memoryBar).to.exist;
  });

  it('should render memory bar fill', () => {
    const fill = element.shadowRoot.getElementById('memory-fill');
    expect(fill).to.exist;
  });

  it('should render control buttons', () => {
    const buttons = element.shadowRoot.querySelectorAll('button');
    expect(buttons.length).to.equal(3);
  });

  it('should render stats grid', () => {
    const statsGrid = element.shadowRoot.querySelector('.stats-grid');
    expect(statsGrid).to.exist;
  });

  describe('Property Changes', () => {
    it('should cache values', () => {
      element.cache('testKey', { value: 'test' });
      const cached = element.getCached('testKey');
      expect(cached).to.exist;
      expect(cached.value).to.equal('test');
    });

    it('should return null for non-existent key', () => {
      const cached = element.getCached('nonExistent');
      expect(cached).to.be.null;
    });

    it('should return null for expired cache entry', () => {
      element.cache('expireKey', { value: 'test' }, -1000);
      const cached = element.getCached('expireKey');
      expect(cached).to.be.null;
    });

    it('should register event listener', () => {
      const handler = () => {};
      element.registerEventListener(window, 'click', handler);
      expect(element._eventListeners.length).to.be.greaterThan(0);
    });

    it('should register observer', () => {
      const observer = { disconnect: () => {} };
      element.registerObserver(observer);
      expect(element._observers.length).to.equal(1);
    });

    it('should allocate buffer', () => {
      const buffer = element.allocateBuffer(100);
      expect(buffer).to.exist;
      expect(buffer.length).to.equal(100);
    });

    it('should get memory info', () => {
      const info = element.getMemoryInfo();
      expect(info).to.have.property('cacheSize');
      expect(info).to.have.property('eventListeners');
      expect(info).to.have.property('observers');
      expect(info).to.have.property('buffers');
    });
  });

  describe('Events', () => {
    it('should setup control button handlers', () => {
      const forceGC = element.shadowRoot.getElementById('force-gc');
      const clearCache = element.shadowRoot.getElementById('clear-cache');
      const dumpMemory = element.shadowRoot.getElementById('dump-memory');
      expect(forceGC).to.exist;
      expect(clearCache).to.exist;
      expect(dumpMemory).to.exist;
    });
  });

  describe('Lifecycle', () => {
    it('should start memory monitoring on connectedCallback', () => {
      expect(element._memoryInterval).to.exist;
    });

    it('should cleanup on disconnectCallback', () => {
      element.disconnectCallback();
      expect(element._memoryInterval).to.be.undefined;
    });
  });
});

describe('LRUCache', () => {
  let cache;

  beforeEach(() => {
    cache = new LRUCache(3);
  });

  it('should store and retrieve values', () => {
    cache.set('key1', 'value1');
    expect(cache.get('key1')).to.equal('value1');
  });

  it('should evict oldest entry when capacity reached', () => {
    cache.set('key1', 'value1');
    cache.set('key2', 'value2');
    cache.set('key3', 'value3');
    cache.set('key4', 'value4');
    expect(cache.get('key1')).to.be.null;
    expect(cache.get('key4')).to.equal('value4');
  });

  it('should update existing key position', () => {
    cache.set('key1', 'value1');
    cache.set('key2', 'value2');
    cache.get('key1');
    cache.set('key3', 'value3');
    expect(cache.get('key2')).to.be.null;
  });

  it('should delete entries', () => {
    cache.set('key1', 'value1');
    cache.delete('key1');
    expect(cache.get('key1')).to.be.null;
  });

  it('should clear all entries', () => {
    cache.set('key1', 'value1');
    cache.set('key2', 'value2');
    cache.clear();
    expect(cache.size()).to.equal(0);
  });
});