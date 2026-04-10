/**
 * Component Communication Patterns - Cross-framework messaging system
 * @module interoperability/08_7_Component-Communication-Patterns
 * @version 1.0.0
 * @example <event-bus-channel name="app"></event-bus-channel>
 */

class EventBus extends EventTarget {
    constructor(options = {}) {
        super();
        this._channels = new Map();
        this._history = options.enableHistory !== false;
        this._maxHistory = options.maxHistory || 100;
        this._historyStore = [];
        this._wildcardHandlers = new Map();
    }

    subscribe(channel, handler, options = {}) {
        const { once = false, priority = 0 } = options;

        if (!this._channels.has(channel)) {
            this._channels.set(channel, new Set());
        }

        const subscription = {
            channel,
            handler,
            once,
            priority,
            id: Symbol()
        };

        this._channels.get(channel).add(subscription);

        if (this._history && channel !== '*') {
            const recentHistory = this._historyStore.filter(h => h.channel === channel);
            recentHistory.forEach(event => handler(event.data, event.meta));
        }

        return () => this.unsubscribe(channel, handler);
    }

    unsubscribe(channel, handler) {
        const subscriptions = this._channels.get(channel);
        if (subscriptions) {
            for (const sub of subscriptions) {
                if (sub.handler === handler) {
                    subscriptions.delete(sub);
                    break;
                }
            }
        }
    }

    publish(channel, data, meta = {}) {
        const event = {
            channel,
            data,
            meta: {
                ...meta,
                timestamp: Date.now(),
                source: meta.source || 'unknown'
            }
        };

        if (this._history) {
            this._historyStore.push(event);
            if (this._historyStore.length > this._maxHistory) {
                this._historyStore.shift();
            }
        }

        const subscriptions = this._channels.get(channel);
        if (subscriptions) {
            const sorted = Array.from(subscriptions).sort((a, b) => b.priority - a.priority);
            for (const sub of sorted) {
                try {
                    sub.handler(data, event.meta);
                    if (sub.once) {
                        subscriptions.delete(sub);
                    }
                } catch (error) {
                    console.error(`Error in event handler for ${channel}:`, error);
                }
            }
        }

        const wildcardSubs = this._wildcardHandlers.get('*');
        wildcardSubs?.forEach(sub => {
            try {
                sub.handler(channel, data, event.meta);
            } catch (error) {
                console.error('Error in wildcard event handler:', error);
            }
        });

        this.dispatchEvent(new CustomEvent(channel, { detail: event }));

        return event;
    }

    once(channel, handler, options = {}) {
        return this.subscribe(channel, handler, { ...options, once: true });
    }

    subscribeWildcard(handler) {
        if (!this._wildcardHandlers.has('*')) {
            this._wildcardHandlers.set('*', new Set());
        }
        this._wildcardHandlers.get('*').add({ handler, id: Symbol() });
    }

    getHistory(channel) {
        if (channel) {
            return this._historyStore.filter(e => e.channel === channel);
        }
        return [...this._historyStore];
    }

    clearHistory() {
        this._historyStore = [];
    }

    channels() {
        return Array.from(this._channels.keys());
    }

    subscriberCount(channel) {
        return this._channels.get(channel)?.size || 0;
    }
}

class MessageChannel {
    constructor(name, options = {}) {
        this.name = name;
        this.port1 = new MessagePort(name + ':1');
        this.port2 = new MessagePort(name + ':2');
        this._connected = false;
    }

    connect() {
        this._connected = true;
        this.port1._connected = true;
        this.port2._connected = true;
    }

    disconnect() {
        this._connected = false;
        this.port1._connected = false;
        this.port2._connected = false;
    }
}

class MessagePort {
    constructor(id) {
        this.id = id;
        this._handlers = new Map();
        this._connected = false;
    }

    postMessage(message, transfer) {
        if (!this._connected) {
            console.warn('Port not connected');
            return;
        }
        // Message dispatch logic
    }

    onmessage(handler) {
        this._handlers.set('message', handler);
    }

    close() {
        this._handlers.clear();
        this._connected = false;
    }
}

class ComponentBridge extends EventTarget {
    constructor(sourceId, targetId) {
        super();
        this.sourceId = sourceId;
        this.targetId = targetId;
        this._messageQueue = [];
        this._connected = false;
        this._middleware = [];
    }

    connect() {
        this._connected = true;
        this._flushQueue();
        return this;
    }

    disconnect() {
        this._connected = false;
        return this;
    }

    send(type, payload, options = {}) {
        const message = {
            id: Symbol(),
            type,
            payload,
            source: this.sourceId,
            target: this.targetId,
            timestamp: Date.now(),
            ...options
        };

        if (this._middleware.length > 0) {
            for (const mw of this._middleware) {
                const result = mw(message);
                if (result === false) return;
                if (result) message = result;
            }
        }

        if (!this._connected) {
            this._messageQueue.push(message);
            return;
        }

        this._dispatch(message);
    }

    use(middleware) {
        this._middleware.push(middleware);
        return this;
    }

    _dispatch(message) {
        this.dispatchEvent(new CustomEvent('message', { detail: message }));
        this.dispatchEvent(new CustomEvent(message.type, { detail: message }));

        window.postMessage({
            bridgeId: `${this.sourceId}:${this.targetId}`,
            ...message
        }, '*');
    }

    _flushQueue() {
        while (this._messageQueue.length > 0) {
            const message = this._messageQueue.shift();
            this._dispatch(message);
        }
    }

    on(type, handler) {
        this.addEventListener(type, handler);
        return () => this.removeEventListener(type, handler);
    }

    once(type, handler) {
        const unsubscribe = this.on(type, (data) => {
            unsubscribe();
            handler(data);
        });
        return unsubscribe;
    }
}

class PubSubManager {
    constructor() {
        this._topics = new Map();
        this._subscribers = new Map();
        this._events = new EventTarget();
    }

    subscribe(topic, handler, options = {}) {
        const { id = Symbol(), priority = 0 } = options;

        if (!this._topics.has(topic)) {
            this._topics.set(topic, new Set());
        }

        const subscriber = { id, handler, priority };
        this._topics.get(topic).add(subscriber);

        this._events.dispatchEvent(new CustomEvent('subscribe', {
            detail: { topic, id }
        }));

        return () => this.unsubscribe(topic, id);
    }

    unsubscribe(topic, id) {
        const subscribers = this._topics.get(topic);
        if (subscribers) {
            for (const sub of subscribers) {
                if (sub.id === id) {
                    subscribers.delete(sub);
                    break;
                }
            }
        }

        this._events.dispatchEvent(new CustomEvent('unsubscribe', {
            detail: { topic, id }
        }));
    }

    publish(topic, data, meta = {}) {
        const subscribers = this._topics.get(topic);
        if (!subscribers || subscribers.size === 0) {
            return false;
        }

        const sorted = Array.from(subscribers).sort((a, b) => b.priority - a.priority);
        const results = [];

        for (const sub of sorted) {
            try {
                const result = sub.handler(data, { topic, ...meta });
                results.push({ id: sub.id, result });
            } catch (error) {
                results.push({ id: sub.id, error });
            }
        }

        this._events.dispatchEvent(new CustomEvent('publish', {
            detail: { topic, data, results }
        }));

        return results;
    }

    once(topic, handler) {
        return this.subscribe(topic, (data, meta) => {
            this.unsubscribe(topic, handler);
            handler(data, meta);
        });
    }

    topics() {
        return Array.from(this._topics.keys());
    }

    subscriberCount(topic) {
        return this._topics.get(topic)?.size || 0;
    }
}

class StateSyncChannel extends EventTarget {
    constructor(namespace) {
        super();
        this.namespace = namespace;
        this._state = {};
        this._version = 0;
        this._handlers = new Map();
        this._batching = false;
        this._pendingUpdates = new Map();
    }

    get state() {
        return { ...this._state };
    }

    setState(updates, meta = {}) {
        if (typeof updates === 'function') {
            updates = updates(this._state);
        }

        if (this._batching) {
            Object.assign(this._pendingUpdates, updates);
            return;
        }

        this._applyUpdates(updates, meta);
    }

    batch(fn) {
        this._batching = true;
        fn();
        this._batching = false;

        if (this._pendingUpdates.size > 0) {
            this._applyUpdates(Object.fromEntries(this._pendingUpdates), {});
            this._pendingUpdates.clear();
        }
    }

    _applyUpdates(updates, meta) {
        const prevState = { ...this._state };

        Object.entries(updates).forEach(([key, value]) => {
            if (value === undefined) {
                delete this._state[key];
            } else {
                this._state[key] = value;
            }
        });

        this._version++;

        this.dispatchEvent(new CustomEvent('change', {
            detail: {
                state: this.state,
                prevState,
                updates,
                version: this._version,
                namespace: this.namespace,
                ...meta
            }
        }));

        window.dispatchEvent(new CustomEvent(`state:${this.namespace}`, {
            detail: {
                state: this.state,
                updates,
                version: this._version
            }
        }));
    }

    subscribe(handler) {
        this.addEventListener('change', (e) => handler(e.detail));
        return () => this.removeEventListener('change', handler);
    }

    getVersion() {
        return this._version;
    }

    reset() {
        this._state = {};
        this._version++;
        this._applyUpdates({});
    }
}

const GlobalEventBus = new EventBus();

class CrossFrameworkBridge {
    static instances = new Map();

    static getInstance(id) {
        if (!this.instances.has(id)) {
            this.instances.set(id, new ComponentBridge('system', id));
        }
        return this.instances.get(id);
    }

    static broadcast(type, payload, meta = {}) {
        GlobalEventBus.publish('broadcast', { type, payload }, meta);
    }

    static subscribe(channel, handler) {
        return GlobalEventBus.subscribe(channel, handler);
    }

    static createChannel(name) {
        return new StateSyncChannel(name);
    }

    static createPubSub() {
        return new PubSubManager();
    }
}

customElements.define('event-bus-channel', class extends HTMLElement {
    connectedCallback() {
        const name = this.getAttribute('name') || 'default';
        this._channel = CrossFrameworkBridge.createChannel(name);
        this._channel.subscribe((detail) => {
            this.dispatchEvent(new CustomEvent('change', { detail }));
        });
    }

    get channel() {
        return this._channel;
    }
});

export {
    EventBus,
    MessageChannel,
    MessagePort,
    ComponentBridge,
    PubSubManager,
    StateSyncChannel,
    GlobalEventBus,
    CrossFrameworkBridge
};
