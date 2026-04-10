import { expect, fixture, html, oneEvent } from '@open-wc/testing';

describe('08_7_Component-Communication-Patterns', () => {
  describe('EventBus', () => {
    let eventBus;

    beforeEach(() => {
      eventBus = new EventBus();
    });

    it('is a class extending EventTarget', () => {
      expect(EventBus).to.be.a('function');
    });

    it('can be instantiated', () => {
      expect(eventBus).to.exist;
    });

    it('subscribes to channel', () => {
      const handler = () => {};
      const unsubscribe = eventBus.subscribe('test-channel', handler);
      expect(unsubscribe).to.be.a('function');
    });

    it('subscribes with once option', () => {
      const handler = () => {};
      eventBus.subscribe('test-channel', handler, { once: true });
    });

    it('subscribes with priority option', () => {
      const handler = () => {};
      eventBus.subscribe('test-channel', handler, { priority: 10 });
    });

    it('unsubscribes from channel', () => {
      const handler = () => {};
      eventBus.subscribe('test-channel', handler);
      eventBus.unsubscribe('test-channel', handler);
    });

    it('publishes to channel', () => {
      const event = eventBus.publish('test-channel', { message: 'hello' });
      expect(event).to.exist;
      expect(event.channel).to.equal('test-channel');
      expect(event.data).to.deep.equal({ message: 'hello' });
    });

    it('publishes with metadata', () => {
      const event = eventBus.publish('test-channel', { data: 'value' }, { source: 'test' });
      expect(event.meta.source).to.equal('test');
    });

    it('adds timestamp to metadata', () => {
      const event = eventBus.publish('test-channel', {});
      expect(event.meta.timestamp).to.be.a('number');
    });

    it('creates channel with subscribe', () => {
      eventBus.subscribe('new-channel', () => {});
      expect(eventBus.hasChannel('new-channel')).to.be.true;
    });

    it('checks if channel exists', () => {
      eventBus.subscribe('existing-channel', () => {});
      expect(eventBus.hasChannel('existing-channel')).to.be.true;
    });

    it('gets channel subscriber count', () => {
      eventBus.subscribe('count-channel', () => {});
      eventBus.subscribe('count-channel', () => {});
      expect(eventBus.getSubscriberCount('count-channel')).to.equal(2);
    });

    it('clears channel', () => {
      eventBus.subscribe('clear-channel', () => {});
      eventBus.clearChannel('clear-channel');
      expect(eventBus.getSubscriberCount('clear-channel')).to.equal(0);
    });

    it('clears all channels', () => {
      eventBus.subscribe('channel1', () => {});
      eventBus.subscribe('channel2', () => {});
      eventBus.clearAll();
      expect(eventBus.getSubscriberCount('channel1')).to.equal(0);
      expect(eventBus.getSubscriberCount('channel2')).to.equal(0);
    });

    it('gets channel history', () => {
      eventBus.publish('history-channel', { value: 1 });
      eventBus.publish('history-channel', { value: 2 });
      const history = eventBus.getHistory('history-channel');
      expect(history.length).to.equal(2);
    });

    it('subscribes to wildcard', () => {
      const wildcardHandler = (channel, data) => {};
      eventBus.subscribe('*', wildcardHandler);
    });

    it('handles error in handler gracefully', () => {
      const badHandler = () => { throw new Error('Handler error'); };
      eventBus.subscribe('error-channel', badHandler);
      const event = eventBus.publish('error-channel', {});
      expect(event).to.exist;
    });
  });

  describe('ComponentBridge', () => {
    let bridge;

    beforeEach(() => {
      bridge = new ComponentBridge();
    });

    it('is a class', () => {
      expect(ComponentBridge).to.be.a('function');
    });

    it('can be instantiated', () => {
      expect(bridge).to.exist;
    });

    it('connects component', () => {
      const mockElement = document.createElement('div');
      bridge.connect('comp1', mockElement);
    });

    it('disconnects component', () => {
      const mockElement = document.createElement('div');
      bridge.connect('comp1', mockElement);
      bridge.disconnect('comp1');
    });

    it('sends message to component', () => {
      const mockElement = document.createElement('div');
      bridge.connect('comp1', mockElement);
      bridge.sendToComponent('comp1', 'test-message', { data: 'value' });
    });

    it('broadcasts message', () => {
      const mockElement1 = document.createElement('div');
      const mockElement2 = document.createElement('div');
      bridge.connect('comp1', mockElement1);
      bridge.connect('comp2', mockElement2);
      bridge.broadcast('broadcast-message', {});
    });

    it('registers message handler', () => {
      const handler = (componentId, message, data) => {};
      bridge.onMessage(handler);
    });

    it('gets connected component', () => {
      const mockElement = document.createElement('div');
      bridge.connect('comp1', mockElement);
      const element = bridge.getComponent('comp1');
      expect(element).to.equal(mockElement);
    });

    it('lists all connections', () => {
      bridge.connect('comp1', document.createElement('div'));
      bridge.connect('comp2', document.createElement('div'));
      const connections = bridge.getAllConnections();
      expect(Object.keys(connections).length).to.equal(2);
    });
  });

  describe('MessageChannel', () => {
    let channel;

    beforeEach(() => {
      channel = new MessageChannel('test-channel');
    });

    it('is a class', () => {
      expect(MessageChannel).to.be.a('function');
    });

    it('can be instantiated', () => {
      expect(channel).to.exist;
    });

    it('sends message', () => {
      channel.send({ type: 'test' });
    });

    it('receives message', () => {
      const handler = (message) => {};
      channel.onReceive(handler);
    });

    it('closes channel', () => {
      channel.close();
    });

    it('checks if channel is open', () => {
      expect(channel.isOpen()).to.be.true;
      channel.close();
      expect(channel.isOpen()).to.be.false;
    });
  });

  describe('StateBroadcaster', () => {
    it('is a class', () => {
      expect(StateBroadcaster).to.be.a('function');
    });

    it('can be instantiated', () => {
      const broadcaster = new StateBroadcaster('test-state');
      expect(broadcaster).to.exist;
    });

    it('broadcasts state', () => {
      const broadcaster = new StateBroadcaster('test');
      broadcaster.broadcast({ value: 'test' });
    });

    it('subscribes to state changes', () => {
      const broadcaster = new StateBroadcaster('test');
      const handler = (state) => {};
      broadcaster.subscribe(handler);
    });

    it('unsubscribes from state changes', () => {
      const broadcaster = new StateBroadcaster('test');
      const handler = () => {};
      broadcaster.subscribe(handler);
      broadcaster.unsubscribe(handler);
    });

    it('gets current state', () => {
      const broadcaster = new StateBroadcaster('test');
      broadcaster.broadcast({ value: 'test' });
      const state = broadcaster.getState();
      expect(state).to.deep.equal({ value: 'test' });
    });
  });

  describe('EventBusChannel', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<event-bus-channel name="test-bus"></event-bus-channel>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('applies name attribute', () => {
      expect(el.getAttribute('name')).to.equal('test-bus');
    });

    it('applies bus attribute', async () => {
      el.setAttribute('bus', 'custom-bus');
      await el.updateComplete;
      expect(el.getAttribute('bus')).to.equal('custom-bus');
    });

    it('sends message via send method', () => {
      el.send('test-message', { data: 'value' });
    });

    it('subscribes to messages via on method', () => {
      const handler = (message, data) => {};
      el.on('test-msg', handler);
    });

    it('unsubscribes via off method', () => {
      const handler = () => {};
      el.on('test-msg', handler);
      el.off('test-msg', handler);
    });
  });

  describe('Edge Cases', () => {
    it('handles EventBus with history disabled', () => {
      const bus = new EventBus({ enableHistory: false });
      bus.publish('no-history', {});
      const history = bus.getHistory('no-history');
      expect(history).to.be.undefined;
    });

    it('handles MessageChannel without messages', () => {
      const channel = new MessageChannel('empty');
      expect(channel.isOpen()).to.be.true;
    });

    it('handles StateBroadcaster without state', () => {
      const broadcaster = new StateBroadcaster('empty');
      const state = broadcaster.getState();
      expect(state).to.be.null;
    });
  });
});