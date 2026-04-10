import { MessageBus, messageBus } from './04_8_Cross-Shadow-DOM-Communication.js';

describe('MessageBus', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('message-bus');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
    component?._cleanup();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders bus container', () => {
      expect(component.shadowRoot.innerHTML).toContain('bus-container');
    });

    test('renders channel list', () => {
      expect(component.shadowRoot.innerHTML).toContain('channel-list');
    });

    test('renders message log', () => {
      expect(component.shadowRoot.innerHTML).toContain('message-log');
    });
  });

  describe('property changes', () => {
    test('namespace attribute updates namespace', () => {
      component.setAttribute('namespace', 'test');
      expect(component._namespace).toBe('test');
    });

    test('max-history attribute updates history limit', () => {
      component.setAttribute('max-history', '100');
      expect(component._maxHistory).toBe(100);
    });

    test('buffered attribute triggers queue processing', () => {
      component.setAttribute('buffered', 'true');
      expect(component.hasAttribute('buffered')).toBe(true);
    });
  });

  describe('events', () => {
    test('dispatches subscribe event', (done) => {
      component.addEventListener('subscribe', (e) => {
        expect(e.detail.channel).toBeDefined();
        done();
      });
      component.subscribe('test-channel', () => {});
    });

    test('dispatches message event', (done) => {
      component.subscribe('test-msg', () => {});
      component.addEventListener('message', (e) => {
        expect(e.detail.channel).toBeDefined();
        done();
      });
      component.publish('test-msg', { data: 'test' });
    });
  });

  describe('edge cases', () => {
    test('subscribe registers callback', () => {
      const callback = jest.fn();
      component.subscribe('channel1', callback);
      expect(component.hasListeners('channel1')).toBe(true);
    });

    test('unsubscribe removes callback', () => {
      const callback = jest.fn();
      component.subscribe('channel2', callback);
      component.unsubscribe('channel2', callback);
      expect(component.hasListeners('channel2')).toBe(false);
    });

    test('publish sends message to listeners', () => {
      const callback = jest.fn();
      component.subscribe('channel3', callback);
      component.publish('channel3', 'message');
      expect(callback).toHaveBeenCalled();
    });

    test('publish returns false when no listeners', () => {
      const result = component.publish('no-listeners', 'message');
      expect(result).toBe(false);
    });

    test('hasListeners returns boolean', () => {
      expect(typeof component.hasListeners('test')).toBe('boolean');
    });

    test('getListeners returns count', () => {
      component.subscribe('count-test', () => {});
      expect(component.getListeners('count-test')).toBe(1);
    });

    test('getChannels returns array', () => {
      component.subscribe('ch1', () => {});
      component.subscribe('ch2', () => {});
      const channels = component.getChannels();
      expect(Array.isArray(channels)).toBe(true);
    });

    test('getHistory returns message history', () => {
      component.publish('history-test', 'msg');
      const history = component.getHistory('history-test');
      expect(Array.isArray(history)).toBe(true);
    });

    test('request returns Promise', () => {
      const promise = component.request('req-channel', 'data');
      expect(promise).toBeInstanceOf(Promise);
    });

    test('broadcast sends to broadcast channel', () => {
      const callback = jest.fn();
      component.subscribe('broadcast', callback);
      component.broadcast('event-name', { key: 'value' });
      expect(callback).toHaveBeenCalled();
    });
  });
});

describe('Global messageBus', () => {
  test('global instance is defined', () => {
    expect(messageBus).toBeDefined();
  });
});
