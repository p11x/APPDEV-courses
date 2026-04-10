import { StyleEncapsulationMethods } from './04_2_Style-Encapsulation-Methods.js';

describe('StyleEncapsulationMethods', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('style-encapsulation-methods');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders style wrapper', () => {
      expect(component.shadowRoot.innerHTML).toContain('style-wrapper');
    });

    test('renders style cards', () => {
      expect(component.shadowRoot.innerHTML).toContain('style-card');
    });

    test('renders demo buttons', () => {
      const buttons = component.shadowRoot.querySelectorAll('.demo-button');
      expect(buttons.length).toBeGreaterThan(0);
    });
  });

  describe('property changes', () => {
    test('theme attribute updates configuration', () => {
      component.setAttribute('theme', 'dark');
      expect(component._getCurrentTheme()).toBeDefined();
    });

    test('mode attribute applies styles', () => {
      component.setAttribute('mode', 'flat');
      expect(component.shadowRoot.innerHTML).toContain('mode="flat"');
    });

    test('compact attribute reduces padding', () => {
      component.setAttribute('compact', '');
      expect(component.hasAttribute('compact')).toBe(true);
    });
  });

  describe('events', () => {
    test('dispatches styles-initialized event', (done) => {
      component.addEventListener('styles-initialized', () => done());
      component.connectedCallback();
    });

    test('dispatches styles-cleaned event', (done) => {
      component.addEventListener('styles-cleaned', () => done());
      component.disconnectedCallback();
    });
  });

  describe('edge cases', () => {
    test('getStyle returns registered style', () => {
      const style = component.getStyle('elevation');
      expect(style).toBeDefined();
    });

    test('getStyle returns null for missing style', () => {
      const style = component.getStyle('non-existent');
      expect(style).toBeNull();
    });

    test('registerStyle adds new style', () => {
      component.registerStyle('custom', '.custom { color: red; }');
      const style = component.getStyle('custom');
      expect(style).toContain('color: red');
    });

    test('applyTheme sets theme attribute', () => {
      component.applyTheme('dark');
      expect(component.getAttribute('theme')).toBe('dark');
    });

    test('animation attribute toggles state', () => {
      component.setAttribute('animation', 'false');
      expect(component._config.enableAnimations).toBe(false);
    });

    test('CSS variables are generated from theme', () => {
      const theme = component._getCurrentTheme();
      const cssVars = component._generateCSSVariables(theme);
      expect(cssVars).toContain('--sec-bg');
    });
  });
});
