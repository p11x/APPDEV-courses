import { DynamicTemplateBuilder, TemplateExpressionEngine, TemplateRegistry, TemplateListRenderer, escapeHtml, deepClone, deepMerge } from './03_4_Dynamic-Template-Generation.js';

describe('DynamicTemplateBuilder', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('dynamic-template-builder');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders template with basic data', () => {
      component.setTemplate('<p>Hello {{name}}</p>');
      component.setData({ name: 'World' });
      component.render();
      expect(component.getRenderedContent()).toContain('Hello World');
    });

    test('renders with attribute template', () => {
      component.setAttribute('template', '<span>{{value}}</span>');
      component.setData({ value: 'Test' });
      component.render();
      expect(component.getRenderedContent()).toContain('Test');
    });
  });

  describe('property changes', () => {
    test('setTemplate updates template', () => {
      component.setTemplate('<div>New</div>');
      expect(component.getTemplate()).toBe('<div>New</div>');
    });

    test('setData updates data context', () => {
      component.setData({ message: 'Hi' });
      expect(component.getData().message).toBe('Hi');
    });

    test('updateData partial update', () => {
      component.setData({ a: 1 });
      component.updateData({ b: 2 });
      expect(component.getData().a).toBe(1);
      expect(component.getData().b).toBe(2);
    });
  });

  describe('events', () => {
    test('dispatches template-rendered event', (done) => {
      component.addEventListener('template-rendered', (e) => {
        expect(e.detail.template).toBeDefined();
        done();
      });
      component.setTemplate('<p>Test</p>');
      component.render();
    });

    test('dispatches template-error event on error', (done) => {
      component.addEventListener('template-error', (e) => {
        expect(e.detail.error).toBeDefined();
        done();
      });
      component.setTemplate(null);
      component.render();
    });
  });

  describe('edge cases', () => {
    test('handles empty data', () => {
      component.setTemplate('<p>{{missing}}</p>');
      component.setData({});
      component.render();
      expect(component.getRenderedContent()).toBeDefined();
    });

    test('clear removes rendered content', () => {
      component.setTemplate('<p>Test</p>');
      component.render();
      component.clear();
      expect(component.getRenderedContent()).toBe('');
    });

    test('getRenderedElements returns HTMLCollection', () => {
      component.setTemplate('<div>1</div><div>2</div>');
      component.render();
      const elements = component.getRenderedElements();
      expect(elements.length).toBeGreaterThan(0);
    });

    test('registerHelper adds custom helper', () => {
      component.registerHelper('double', n => n * 2);
      const engine = component._engine;
      expect(engine._helpers.has('double')).toBe(true);
    });
  });
});

describe('TemplateExpressionEngine', () => {
  let engine;

  beforeEach(() => {
    engine = new TemplateExpressionEngine();
  });

  describe('render', () => {
    test('renders simple interpolation', () => {
      const result = engine.render('Hello {{name}}', { name: 'World' });
      expect(result).toBe('Hello World');
    });

    test('renders with helper functions', () => {
      const result = engine.render('{{name | uppercase}}', { name: 'hello' });
      expect(result).toBe('HELLO');
    });

    test('renders #if block', () => {
      const result = engine.render('{{#if shown}}Visible{{/if}}', { shown: true });
      expect(result).toBe('Visible');
    });

    test('renders #each block', () => {
      const result = engine.render('{{#each items}}{{this}} {{/each}}', { items: ['a', 'b'] });
      expect(result).toContain('a');
    });
  });

  describe('registerHelper', () => {
    test('registers custom helper', () => {
      engine.registerHelper('custom', val => `custom(${val})`);
      const result = engine.render('{{value | custom}}', { value: 'test' });
      expect(result).toContain('custom(test)');
    });

    test('throws on non-function helper', () => {
      expect(() => engine.registerHelper('bad', 'not a function')).toThrow();
    });
  });

  describe('edge cases', () => {
    test('handles missing variables', () => {
      const result = engine.render('{{missing}}', {});
      expect(result).toBeDefined();
    });

    test('handles nested object paths', () => {
      const result = engine.render('{{user.name}}', { user: { name: 'John' } });
      expect(result).toBe('John');
    });

    test('handles array access', () => {
      const result = engine.render('{{items[0]}}', { items: ['first'] });
      expect(result).toBe('first');
    });
  });
});

describe('TemplateRegistry', () => {
  let registry;

  beforeEach(() => {
    registry = new TemplateRegistry();
  });

  describe('register', () => {
    test('registers template', () => {
      registry.register('test', '<p>Test</p>');
      expect(registry.has('test')).toBe(true);
    });

    test('throws on invalid name', () => {
      expect(() => registry.register('', '<p>x</p>')).toThrow();
    });

    test('throws on non-string template', () => {
      expect(() => registry.register('test', null)).toThrow();
    });
  });

  describe('get', () => {
    test('retrieves registered template', () => {
      registry.register('my-template', '<div>Content</div>');
      const tmpl = registry.get('my-template');
      expect(tmpl.template).toBe('<div>Content</div>');
    });

    test('returns null for missing template', () => {
      expect(registry.get('missing')).toBeNull();
    });
  });

  describe('render', () => {
    test('renders registered template with data', () => {
      registry.register('greet', 'Hello {{name}}');
      const result = registry.render('greet', { name: 'Alice' });
      expect(result).toBe('Hello Alice');
    });

    test('throws on missing template', () => {
      expect(() => registry.render('missing', {})).toThrow();
    });
  });

  describe('remove', () => {
    test('removes template', () => {
      registry.register('to-remove', '<p>x</p>');
      registry.remove('to-remove');
      expect(registry.has('to-remove')).toBe(false);
    });
  });

  describe('clear', () => {
    test('clears all templates', () => {
      registry.register('a', '<p>a</p>');
      registry.register('b', '<p>b</p>');
      registry.clear();
      expect(registry.getNames().length).toBe(0);
    });
  });
});

describe('TemplateListRenderer', () => {
  let renderer;

  beforeEach(() => {
    renderer = new TemplateListRenderer();
  });

  describe('render', () => {
    test('renders list of items', () => {
      const result = renderer.render('{{name}}', [{ name: 'Alice' }, { name: 'Bob' }]);
      expect(result).toContain('Alice');
      expect(result).toContain('Bob');
    });

    test('renders empty message for empty array', () => {
      const result = renderer.render('{{name}}', []);
      expect(result).toContain('No items');
    });

    test('throws on non-array items', () => {
      expect(() => renderer.render('{{x}}', 'not array')).toThrow();
    });
  });
});

describe('Utility functions', () => {
  describe('escapeHtml', () => {
    test('escapes special characters', () => {
      expect(escapeHtml('<script>')).toContain('&lt;');
    });

    test('handles null/undefined', () => {
      expect(escapeHtml(null)).toBe('');
      expect(escapeHtml(undefined)).toBe('');
    });
  });

  describe('deepClone', () => {
    test('clones simple object', () => {
      const obj = { a: 1 };
      const cloned = deepClone(obj);
      expect(cloned).toEqual(obj);
      expect(cloned).not.toBe(obj);
    });

    test('handles arrays', () => {
      const arr = [1, 2, 3];
      const cloned = deepClone(arr);
      expect(cloned).toEqual(arr);
    });
  });

  describe('deepMerge', () => {
    test('merges objects', () => {
      const result = deepMerge({ a: 1 }, { b: 2 });
      expect(result.a).toBe(1);
      expect(result.b).toBe(2);
    });

    test('deep merges nested objects', () => {
      const result = deepMerge({ a: { b: 1 } }, { a: { c: 2 } });
      expect(result.a.b).toBe(1);
      expect(result.a.c).toBe(2);
    });
  });
});
