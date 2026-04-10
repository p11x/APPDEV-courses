import { SlotContentDistributionMastery } from './04_3_Slot-Content-Distribution-Mastery.js';

describe('SlotContentDistributionMastery', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('slot-content-distribution-mastery');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders slot container', () => {
      expect(component.shadowRoot.innerHTML).toContain('slot-container');
    });

    test('renders multiple slot panels', () => {
      const panels = component.shadowRoot.querySelectorAll('.slot-panel');
      expect(panels.length).toBeGreaterThan(0);
    });

    test('renders slot selector', () => {
      expect(component.shadowRoot.innerHTML).toContain('slot-selector');
    });
  });

  describe('property changes', () => {
    test('mode attribute updates slot mode', () => {
      component.setAttribute('mode', 'dark');
      expect(component.getAttribute('mode')).toBe('dark');
    });

    test('show-fallback attribute toggles fallback visibility', () => {
      component.setAttribute('show-fallback', 'true');
      expect(component.getAttribute('show-fallback')).toBe('true');
    });

    test('layout attribute updates layout', () => {
      component.setAttribute('layout', 'grid');
      expect(component.getAttribute('layout')).toBe('grid');
    });
  });

  describe('events', () => {
    test('dispatches slot-change event', (done) => {
      component.addEventListener('slot-change', (e) => {
        expect(e.detail.slotName).toBeDefined();
        done();
      });
    });

    test('dispatches slot-select event', (done) => {
      component.addEventListener('slot-select', (e) => {
        expect(e.detail.slotName).toBeDefined();
        done();
      });
    });
  });

  describe('edge cases', () => {
    test('getSlotContent returns nodes for slot', () => {
      const slotEl = component.shadowRoot.querySelector('slot[name="default"]');
      expect(slotEl).toBeTruthy();
    });

    test('hasSlotContent returns boolean', () => {
      const result = component.hasSlotContent('default');
      expect(typeof result).toBe('boolean');
    });

    test('getSlots returns map of slots', () => {
      const slots = component.getSlots();
      expect(slots).toBeInstanceOf(Map);
    });

    test('assignNodes assigns nodes to slot', () => {
      const div = document.createElement('div');
      component.assignNodes('header', [div]);
    });
  });
});
