import { SafeHTMLRenderer, HTMLSanitizer, CSPHelper } from './03_3_Safe-HTML-Parsing-Methods.js';

describe('SafeHTMLRenderer', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('safe-html-renderer');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders basic HTML', () => {
      component.html = '<p>Hello</p>';
      expect(component.html).toContain('Hello');
    });

    test('renders with sanitization enabled by default', () => {
      component.html = '<p>Test</p>';
      expect(component._sanitize).toBe(true);
    });
  });

  describe('property changes', () => {
    test('html property triggers render', () => {
      component.html = '<b>Bold</b>';
      expect(component.html).toBe('<b>Bold</b>');
    });

    test('sanitize attribute toggles sanitization', () => {
      component.setAttribute('sanitize', 'false');
      expect(component._sanitize).toBe(false);
    });

    test('allowed-tags attribute updates allowed tags', () => {
      component.setAttribute('allowed-tags', 'p,span,div');
      expect(component._allowedTags.has('p')).toBe(true);
    });
  });

  describe('events', () => {
    test('dispatches render event', (done) => {
      component.addEventListener('render', () => done());
      component.render();
    });

    test('dispatches blocked event when content is blocked', (done) => {
      component.addEventListener('blocked', (e) => {
        expect(e.detail.blocked.length).toBeGreaterThan(0);
        done();
      });
      component.html = '<script>alert(1)</script>';
    });
  });

  describe('edge cases', () => {
    test('blocks dangerous tags', () => {
      component.html = '<script>alert(1)</script><p>Safe</p>';
      expect(component.shadowRoot.innerHTML).not.toContain('<script>');
    });

    test('blocks event handlers', () => {
      component.html = '<button onclick="alert(1)">Click</button>';
      expect(component.shadowRoot.innerHTML).not.toContain('onclick');
    });

    test('blocks unsafe href protocols', () => {
      component.html = '<a href="javascript:alert(1)">Link</a>';
      expect(component.shadowRoot.innerHTML).not.toContain('javascript:');
    });

    test('getBlockedContent returns blocked items', () => {
      component.html = '<script>x</script>';
      const blocked = component.getBlockedContent();
      expect(Array.isArray(blocked)).toBe(true);
    });
  });
});

describe('HTMLSanitizer', () => {
  describe('sanitize', () => {
    test('sanitizes HTML string', () => {
      const result = HTMLSanitizer.sanitize('<p>Hello</p>');
      expect(result.html).toContain('Hello');
    });

    test('removes dangerous tags', () => {
      const result = HTMLSanitizer.sanitize('<script>alert(1)</script>');
      expect(result.html).not.toContain('<script>');
      expect(result.blocked.length).toBeGreaterThan(0);
    });

    test('removes event handler attributes', () => {
      const result = HTMLSanitizer.sanitize('<button onclick="x">Click</button>');
      expect(result.html).not.toContain('onclick');
    });

    test('uses custom allowed tags', () => {
      const result = HTMLSanitizer.sanitize('<custom>Test</custom>', {
        allowedTags: ['custom']
      });
      expect(result.html).toContain('<custom>');
    });
  });

  describe('escape', () => {
    test('escapes HTML entities', () => {
      const escaped = HTMLSanitizer.escape('<>&"\'');
      expect(escaped).not.toContain('<');
      expect(escaped).not.toContain('>');
    });

    test('escapes ampersand', () => {
      expect(HTMLSanitizer.escape('A & B')).toContain('&amp;');
    });
  });

  describe('unescape', () => {
    test('unescapes HTML entities', () => {
      const unescaped = HTMLSanitizer.unescape('&lt;div&gt;');
      expect(unescaped).toBe('<div>');
    });
  });

  describe('isSafeURL', () => {
    test('rejects javascript: URLs', () => {
      expect(HTMLSanitizer.isSafeURL('javascript:alert(1)')).toBe(false);
    });

    test('rejects data: URLs', () => {
      expect(HTMLSanitizer.isSafeURL('data:text/html,<script>')).toBe(false);
    });

    test('accepts http: URLs', () => {
      expect(HTMLSanitizer.isSafeURL('http://example.com')).toBe(true);
    });

    test('accepts https: URLs', () => {
      expect(HTMLSanitizer.isSafeURL('https://example.com')).toBe(true);
    });
  });

  describe('stripTags', () => {
    test('strips all HTML tags', () => {
      const text = HTMLSanitizer.stripTags('<p>Hello <b>World</b></p>');
      expect(text).toBe('Hello World');
    });
  });
});

describe('CSPHelper', () => {
  describe('isEnforced', () => {
    test('checks if CSP is enforced', () => {
      const result = CSPHelper.isEnforced();
      expect(typeof result).toBe('boolean');
    });
  });
});
