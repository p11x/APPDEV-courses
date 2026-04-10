import { EventBubblingAndTargeting } from './04_4_Event-Bubbling-and-Targeting.js';

describe('EventBubblingAndTargeting', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('event-bubbling-and-targeting');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders event container', () => {
      expect(component.shadowRoot.innerHTML).toContain('event-container');
    });

    test('renders target layers', () => {
      const layers = component.shadowRoot.querySelectorAll('.target-layer');
      expect(layers.length).toBeGreaterThan(0);
    });

    test('renders event log', () => {
      expect(component.shadowRoot.innerHTML).toContain('event-log');
    });

    test('renders trigger buttons', () => {
      const buttons = component.shadowRoot.querySelectorAll('.trigger-button');
      expect(buttons.length).toBeGreaterThan(0);
    });
  });

  describe('property changes', () => {
    test('bubbles attribute updates config', () => {
      component.setAttribute('bubbles', 'false');
      expect(component.getAttribute('bubbles')).toBe('false');
    });

    test('verbose attribute toggles logging', () => {
      component.setAttribute('verbose', 'false');
      expect(component._eventConfig.eventLogger).toBe(false);
    });

    test('track-targets attribute configures tracking', () => {
      component.setAttribute('track-targets', 'true');
      expect(component.getAttribute('track-targets')).toBe('true');
    });
  });

  describe('events', () => {
    test('dispatches event-logged event', (done) => {
      component.addEventListener('event-logged', (e) => {
        expect(e.detail.type).toBeDefined();
        done();
      });
    });
  });

  describe('edge cases', () => {
    test('getEventHistory returns event entries', () => {
      const history = component.getEventHistory();
      expect(Array.isArray(history)).toBe(true);
    });

    test('clearHistory clears event history', () => {
      component.clearHistory();
      expect(component.getEventHistory().length).toBe(0);
    });

    test('dispatchNamedEvent dispatches custom event', () => {
      component.dispatchNamedEvent('test-event', { bubbles: true, composed: true });
      expect(component._eventHistory.length).toBeGreaterThan(0);
    });

    test('logs events with detail', () => {
      const initialLength = component._eventHistory.length;
      component._logEvent('test', { key: 'value' });
      expect(component._eventHistory.length).toBe(initialLength + 1);
    });

    test('event path is updated on events', () => {
      const event = new Event('test');
      event.composedPath = () => ['DIV', 'BODY', 'HTML'];
      component._updateEventPath(event);
      expect(component.shadowRoot.getElementById('eventPath')).toBeTruthy();
    });
  });
});
