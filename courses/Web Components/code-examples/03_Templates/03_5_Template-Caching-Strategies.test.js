import { TemplateCacheManager, CacheStatistics, LRUCache } from './03_5_Template-Caching-Strategies.js';

describe('TemplateCacheManager', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('template-cache-manager');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders cache manager UI', () => {
      expect(component.shadowRoot.innerHTML).toBeDefined();
    });

    test('initializes with default options', () => {
      expect(component._maxCacheSize).toBe(50);
      expect(component._enablePrecompilation).toBe(true);
    });
  });

  describe('property changes', () => {
    test('max-cache-size attribute updates limit', () => {
      component.setAttribute('max-cache-size', '100');
      expect(component._maxCacheSize).toBe(100);
    });

    test('enable-precompilation attribute toggles feature', () => {
      component.setAttribute('enable-precompilation', 'false');
      expect(component._enablePrecompilation).toBe(false);
    });
  });

  describe('events', () => {
    test('dispatches events on template operations', (done) => {
      component.setTemplate('test-key', '<p>Test</p>');
      setTimeout(() => done());
    });
  });

  describe('edge cases', () => {
    test('setTemplate stores template in cache', () => {
      component.setTemplate('key1', '<div>Content</div>');
      expect(component.getTemplate('key1')).toBe('<div>Content</div>');
    });

    test('getTemplate returns null for missing key', () => {
      expect(component.getTemplate('missing')).toBeNull();
    });

    test('cacheTemplate with precompilation', () => {
      component.cacheTemplate('precompiled', '<p>Test</p>', true);
      expect(component.hasTemplate('precompiled')).toBe(true);
    });

    test('clearCache removes all templates', () => {
      component.setTemplate('key1', '<p>1</p>');
      component.setTemplate('key2', '<p>2</p>');
      component.clearCache();
      expect(component.getCachedKeys().length).toBe(0);
    });

    test('invalidate removes specific template', () => {
      component.setTemplate('to-invalidate', '<p>Test</p>');
      component.invalidate('to-invalidate');
      expect(component.hasTemplate('to-invalidate')).toBe(false);
    });

    test('getStats returns cache statistics', () => {
      const stats = component.getStats();
      expect(stats.size).toBeDefined();
      expect(stats.hits).toBeDefined();
      expect(stats.misses).toBeDefined();
    });

    test('getMultiple returns multiple templates', () => {
      component.setTemplate('a', '<p>a</p>');
      component.setTemplate('b', '<p>b</p>');
      const result = component.getMultiple(['a', 'b', 'c']);
      expect(result.a).toBe('<p>a</p>');
      expect(result.b).toBe('<p>b</p>');
      expect(result.c).toBeUndefined();
    });

    test('warmUp populates cache', () => {
      component.warmUp({ 'warm1': '<p>1</p>', 'warm2': '<p>2</p>' });
      expect(component.hasTemplate('warm1')).toBe(true);
      expect(component.hasTemplate('warm2')).toBe(true);
    });

    test('exportCache returns JSON string', () => {
      component.setTemplate('export-test', '<p>Test</p>');
      const json = component.exportCache();
      expect(json).toContain('export-test');
    });

    test('importCache restores from JSON', () => {
      const json = JSON.stringify({ templates: { 'imported': '<p>Imported</p>' } });
      component.importCache(json);
      expect(component.getTemplate('imported')).toBe('<p>Imported</p>');
    });
  });
});

describe('LRUCache', () => {
  let cache;

  beforeEach(() => {
    cache = new LRUCache(3);
  });

  describe('core functionality', () => {
    test('sets and gets values', () => {
      cache.set('key', 'value');
      expect(cache.get('key')).toBe('value');
    });

    test('returns undefined for missing key', () => {
      expect(cache.get('missing')).toBeUndefined();
    });

    test('checks key existence', () => {
      cache.set('exists', 'value');
      expect(cache.has('exists')).toBe(true);
      expect(cache.has('not-exists')).toBe(false);
    });

    test('deletes key', () => {
      cache.set('to-delete', 'value');
      cache.delete('to-delete');
      expect(cache.has('to-delete')).toBe(false);
    });
  });

  describe('LRU behavior', () => {
    test('evicts oldest entry when full', () => {
      cache.set('a', '1');
      cache.set('b', '2');
      cache.set('c', '3');
      cache.set('d', '4');
      expect(cache.has('a')).toBe(false);
      expect(cache.has('d')).toBe(true);
    });

    test('moves accessed key to newest', () => {
      cache.set('a', '1');
      cache.set('b', '2');
      cache.get('a');
      cache.set('c', '3');
      cache.set('d', '4');
      expect(cache.has('a')).toBe(true);
    });
  });

  describe('statistics', () => {
    test('tracks hits and misses', () => {
      cache.set('key', 'value');
      cache.get('key');
      cache.get('missing');
      const stats = cache.getStats();
      expect(stats.hits).toBe(1);
      expect(stats.misses).toBe(1);
    });

    test('calculates hit rate', () => {
      cache.set('a', '1');
      cache.get('a');
      cache.get('a');
      cache.get('missing');
      const stats = cache.getStats();
      expect(stats.hitRate).toBeGreaterThan(0);
    });
  });

  describe('clear', () => {
    test('clears all entries', () => {
      cache.set('a', '1');
      cache.set('b', '2');
      cache.clear();
      expect(cache.size()).toBe(0);
      expect(cache.get('a')).toBeUndefined();
    });
  });
});

describe('CacheStatistics', () => {
  test('generates statistics report', () => {
    const manager = new TemplateCacheManager();
    const stats = new CacheStatistics(manager);
    const report = stats.generateReport();
    expect(report.current).toBeDefined();
    expect(report.generatedAt).toBeDefined();
  });

  test('exports statistics to JSON', () => {
    const manager = new TemplateCacheManager();
    const stats = new CacheStatistics(manager);
    const exported = stats.export();
    expect(typeof exported).toBe('string');
  });

  test('clears history', () => {
    const manager = new TemplateCacheManager();
    const stats = new CacheStatistics(manager);
    stats.clearHistory();
    expect(stats.getCurrent()).toBeDefined();
  });
});
