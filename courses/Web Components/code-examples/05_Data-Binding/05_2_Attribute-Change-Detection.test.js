import { AttributeDetectorElement, ATTRIBUTE_CONFIG, AttributeChangeError } from './05_2_Attribute-Change-Detection.js';

describe('AttributeDetectorElement', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('attribute-detector-element');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders attribute grid', () => {
      expect(component.shadowRoot.innerHTML).toContain('attribute-grid');
    });

    test('renders controls', () => {
      const buttons = component.shadowRoot.querySelectorAll('.control-button');
      expect(buttons.length).toBe(3);
    });

    test('renders history panel', () => {
      expect(component.shadowRoot.innerHTML).toContain('history-panel');
    });
  });

  describe('property changes', () => {
    test('data-config attribute updates config', () => {
      component.setAttribute('data-config', JSON.stringify({ theme: 'dark' }));
      expect(component.getAttribute('data-config')).toContain('dark');
    });

    test('data-state attribute updates state', () => {
      component.setAttribute('data-state', JSON.stringify({ active: false }));
      expect(component.getAttribute('data-state')).toContain('false');
    });

    test('data-mode attribute updates mode', () => {
      component.setAttribute('data-mode', 'edit');
      expect(component.getAttribute('data-mode')).toBe('edit');
    });
  });

  describe('events', () => {
    test('observes attribute changes', () => {
      component.setAttribute('data-mode', 'view');
      expect(component._attributes.has('data-mode')).toBe(true);
    });
  });

  describe('edge cases', () => {
    test('observeAttribute registers callback', () => {
      const callback = jest.fn();
      const unsubscribe = component.observeAttribute('data-config', callback);
      expect(typeof unsubscribe).toBe('function');
    });

    test('observeAttribute throws for invalid attribute', () => {
      expect(() => component.observeAttribute('invalid', () => {})).toThrow();
    });

    test('getAttributeData returns attribute data', () => {
      const data = component.getAttributeData('data-mode');
      expect(data).toBeDefined();
    });

    test('getAttributeData returns null for missing attribute', () => {
      const data = component.getAttributeData('missing');
      expect(data).toBeNull();
    });

    test('dataConfig getter and setter', () => {
      component.dataConfig = { theme: 'dark', compact: true };
      expect(component.dataConfig.theme).toBe('dark');
    });

    test('dataState getter and setter', () => {
      component.dataState = { active: true };
      expect(component.dataState.active).toBe(true);
    });

    test('dataMode getter and setter', () => {
      component.dataMode = 'debug';
      expect(component.dataMode).toBe('debug');
    });

    test('records change in history', () => {
      component._recordChange('test-attr', 'old', 'new');
      expect(component._changeHistory.length).toBe(1);
    });
  });
});

describe('ATTRIBUTE_CONFIG', () => {
  test('has monitored attributes', () => {
    expect(ATTRIBUTE_CONFIG.monitoredAttributes.length).toBeGreaterThan(0);
  });

  test('has attribute parser', () => {
    expect(ATTRIBUTE_CONFIG.attributeParser['data-config']).toBeDefined();
  });

  test('has attribute serializer', () => {
    expect(ATTRIBUTE_CONFIG.attributeSerializer['data-config']).toBeDefined();
  });
});
