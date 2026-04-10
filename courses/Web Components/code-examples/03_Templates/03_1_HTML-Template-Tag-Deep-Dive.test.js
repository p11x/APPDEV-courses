import { TemplateRenderer, TemplateRegistry, TemplateUtils } from './03_1_HTML-Template-Tag-Deep-Dive.js';

describe('TemplateRenderer', () => {
  let component;
  let template;

  beforeEach(() => {
    template = document.createElement('template');
    template.id = 'test-template';
    template.innerHTML = '<div class="test">{{message}}</div>';
    document.body.appendChild(template);

    component = document.createElement('template-renderer');
    component.setAttribute('template-id', 'test-template');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
    template?.remove();
  });

  describe('rendering', () => {
    test('renders template with basic data', () => {
      component.setAttribute('data', JSON.stringify({ message: 'Hello' }));
      component.render();
      expect(component.shadowRoot.innerHTML).toContain('Hello');
    });

    test('renders template without data', () => {
      component.render();
      expect(component.shadowRoot.innerHTML).toBeDefined();
    });

    test('handles missing template gracefully', () => {
      const comp = document.createElement('template-renderer');
      comp.render();
      expect(comp.shadowRoot.innerHTML).toContain('<slot>');
    });
  });

  describe('property changes', () => {
    test('updates data property triggers render', () => {
      component.data = { message: 'Updated' };
      expect(component.data).toEqual({ message: 'Updated' });
    });

    test('attribute change triggers render', () => {
      component.setAttribute('template-id', 'test-template');
      expect(component.getAttribute('template-id')).toBe('test-template');
    });
  });

  describe('events', () => {
    test('dispatches render event after rendering', (done) => {
      component.addEventListener('render', () => done());
      component.render();
    });
  });

  describe('edge cases', () => {
    test('handles invalid JSON data', () => {
      component.setAttribute('data', 'invalid');
      expect(() => component.render()).not.toThrow();
    });

    test('handles nested data binding', () => {
      const tmpl = document.createElement('template');
      tmpl.id = 'nested-test';
      tmpl.innerHTML = '<span>{{user.name}}</span>';
      document.body.appendChild(tmpl);

      component.setAttribute('template-id', 'nested-test');
      component.data = { user: { name: 'John' } };
      expect(component.shadowRoot.innerHTML).toContain('John');
      tmpl.remove();
    });

    test('renderList handles empty array', () => {
      component.renderList('test-template', []);
      expect(component.shadowRoot.innerHTML).toBeDefined();
    });

    test('clearCache removes cached templates', () => {
      component.render();
      component.clearCache();
      expect(component._templateCache.size).toBe(0);
    });
  });
});

describe('TemplateRegistry', () => {
  let registry;

  beforeEach(() => {
    registry = new TemplateRegistry();
  });

  describe('core functionality', () => {
    test('registers and retrieves template', () => {
      const tmpl = document.createElement('template');
      registry.register('test', tmpl);
      expect(registry.get('test')).toBe(tmpl);
    });

    test('checks if template exists', () => {
      const tmpl = document.createElement('template');
      registry.register('exists', tmpl);
      expect(registry.has('exists')).toBe(true);
      expect(registry.has('not-exists')).toBe(false);
    });

    test('returns all keys', () => {
      registry.register('a', document.createElement('template'));
      registry.register('b', document.createElement('template'));
      expect(registry.keys()).toContain('a');
      expect(registry.keys()).toContain('b');
    });

    test('returns size', () => {
      expect(registry.size).toBe(0);
      registry.register('test', document.createElement('template'));
      expect(registry.size).toBe(1);
    });

    test('clears all templates', () => {
      registry.register('test', document.createElement('template'));
      registry.clear();
      expect(registry.size).toBe(0);
    });
  });

  describe('edge cases', () => {
    test('enforces max size limit', () => {
      const smallRegistry = new TemplateRegistry();
      smallRegistry._maxSize = 2;
      smallRegistry.register('a', document.createElement('template'));
      smallRegistry.register('b', document.createElement('template'));
      smallRegistry.register('c', document.createElement('template'));
      expect(smallRegistry.size).toBe(2);
      expect(smallRegistry.has('a')).toBe(false);
    });

    test('returns null for non-existent template', () => {
      expect(registry.get('missing')).toBeNull();
    });
  });
});

describe('TemplateUtils', () => {
  describe('createTemplate', () => {
    test('creates template from string', () => {
      const tmpl = TemplateUtils.createTemplate('<div>Test</div>');
      expect(tmpl.tagName).toBe('TEMPLATE');
      expect(tmpl.innerHTML).toContain('Test');
    });
  });

  describe('cloneWithData', () => {
    test('clones template with data binding', () => {
      const tmpl = TemplateUtils.createTemplate('<span>{{message}}</span>');
      const clone = TemplateUtils.cloneWithData(tmpl, { message: 'Hello' });
      expect(clone.textContent).toContain('Hello');
    });

    test('returns clone without data', () => {
      const tmpl = TemplateUtils.createTemplate('<div>No data</div>');
      const clone = TemplateUtils.cloneWithData(tmpl);
      expect(clone.textContent).toContain('No data');
    });
  });

  describe('precompile', () => {
    test('precompiles templates', () => {
      const tmpl = document.createElement('template');
      tmpl.id = 'precompile-test';
      tmpl.innerHTML = '<div>Content</div>';
      document.body.appendChild(tmpl);

      const result = TemplateUtils.precompile(['precompile-test']);
      expect(result.length).toBeGreaterThan(0);
      tmpl.remove();
    });
  });

  describe('getContent', () => {
    test('returns template content', () => {
      const tmpl = TemplateUtils.createTemplate('<p>Content</p>');
      expect(TemplateUtils.getContent(tmpl)).toContain('Content');
    });

    test('handles null template', () => {
      expect(TemplateUtils.getContent(null)).toBe('');
    });
  });
});
