/**
 * Reactive Programming Patterns
 * Implements RxJS-style observables, subscriptions, and reactive data streams
 * @module data-binding/05_4_Reactive-Programming-Patterns
 * @version 1.0.0
 * @example <reactive-element stream-name="events" auto-subscribe></reactive-element>
 */

const REACTIVE_CONFIG = {
  operators: ['map', 'filter', 'debounce', 'distinct', 'throttle', 'take', 'skip'],
  defaultDebounceTime: 300,
  defaultThrottleTime: 100,
  bufferSize: 100,
  enableAutoCleanup: true,
  subscriptionGracePeriod: 5000
};

class ReactiveError extends Error {
  constructor(message, code = 'REACTIVE_ERROR') {
    super(message);
    this.name = 'ReactiveError';
    this.code = code;
  }
}

class Subject {
  constructor() {
    this.observers = new Set();
    this.closed = false;
    this.value = undefined;
  }

  subscribe(observer) {
    if (this.closed) {
      throw new ReactiveError('Subject is closed', 'CLOSED');
    }

    if (typeof observer === 'function') {
      observer = { next: observer, error: () => {}, complete: () => {} };
    }

    this.observers.add(observer);

    if (this.value !== undefined) {
      observer.next(this.value);
    }

    return () => {
      this.observers.delete(observer);
    };
  }

  next(value) {
    if (this.closed) return;

    this.value = value;
    this.observers.forEach(observer => {
      try {
        observer.next(value);
      } catch (error) {
        observer.error(error);
      }
    });
  }

  error(error) {
    if (this.closed) return;

    this.observers.forEach(observer => {
      try {
        observer.error(error);
      } catch (e) {
        console.error('Observer error:', e);
      }
    });
    this.closed = true;
  }

  complete() {
    if (this.closed) return;

    this.observers.forEach(observer => {
      try {
        observer.complete();
      } catch (error) {
        console.error('Complete error:', error);
      }
    });
    this.closed = true;
  }

  get observed() {
    return Array.from(this.observers);
  }
}

class BehaviorSubject extends Subject {
  constructor(initialValue) {
    super();
    this.value = initialValue;
  }
}

class ReplaySubject extends Subject {
  constructor(bufferSize = 1) {
    super();
    this.bufferSize = bufferSize;
    this.buffer = [];
  }

  subscribe(observer) {
    const unsubscribe = super.subscribe(observer);

    for (const value of this.buffer) {
      observer.next(value);
    }

    return unsubscribe;
  }

  next(value) {
    if (this.closed) return;

    this.buffer.push(value);
    if (this.buffer.length > this.bufferSize) {
      this.buffer.shift();
    }

    super.next(value);
  }
}

class Operator {
  constructor(source) {
    this.source = source;
  }

  pipe(...operators) {
    return operators.reduce((acc, op) => op(acc), this.source);
  }

  map(transform) {
    const source = this.source;
    return new Observable(observer => {
      return source.subscribe({
        next: value => {
          try {
            const transformed = transform(value);
            observer.next(transformed);
          } catch (error) {
            observer.error(error);
          }
        },
        error: observer.error,
        complete: observer.complete
      });
    });
  }

  filter(predicate) {
    const source = this.source;
    return new Observable(observer => {
      return source.subscribe({
        next: value => {
          try {
            if (predicate(value)) {
              observer.next(value);
            }
          } catch (error) {
            observer.error(error);
          }
        },
        error: observer.error,
        complete: observer.complete
      });
    });
  }

  debounceTime(delay) {
    const source = this.source;
    let timeoutId = null;

    return new Observable(observer => {
      return source.subscribe({
        next: value => {
          clearTimeout(timeoutId);
          timeoutId = setTimeout(() => {
            observer.next(value);
          }, delay);
        },
        error: observer.error,
        complete: () => {
          clearTimeout(timeoutId);
          observer.complete();
        }
      });
    });
  }

  throttleTime(delay) {
    const source = this.source;
    let lastValue = null;
    let timeoutId = null;

    return new Observable(observer => {
      return source.subscribe({
        next: value => {
          lastValue = value;
          if (!timeoutId) {
            observer.next(value);
            timeoutId = setTimeout(() => {
              timeoutId = null;
              if (lastValue !== value) {
                observer.next(lastValue);
              }
            }, delay);
          }
        },
        error: observer.error,
        complete: () => {
          clearTimeout(timeoutId);
          observer.complete();
        }
      });
    });
  }

  distinct() {
    const source = this.source;
    const seen = new Set();

    return new Observable(observer => {
      return source.subscribe({
        next: value => {
          const key = JSON.stringify(value);
          if (!seen.has(key)) {
            seen.add(key);
            observer.next(value);
          }
        },
        error: observer.error,
        complete: observer.complete
      });
    });
  }

  take(count) {
    const source = this.source;
    let taken = 0;

    return new Observable(observer => {
      if (count === 0) {
        observer.complete();
        return () => {};
      }

      return source.subscribe({
        next: value => {
          observer.next(value);
          taken++;
          if (taken >= count) {
            observer.complete();
          }
        },
        error: observer.error,
        complete: observer.complete
      });
    });
  }

  skip(count) {
    const source = this.source;
    let skipped = 0;

    return new Observable(observer => {
      return source.subscribe({
        next: value => {
          if (skipped < count) {
            skipped++;
          } else {
            observer.next(value);
          }
        },
        error: observer.error,
        complete: observer.complete
      });
    });
  }
}

class Observable extends Operator {
  constructor(subscribeFn) {
    super(null);
    this._subscribe = subscribeFn;
  }

  static from(values) {
    return new Observable(observer => {
      values.forEach(value => observer.next(value));
      observer.complete();
      return () => {};
    });
  }

  static fromEvent(element, eventName) {
    return new Observable(observer => {
      const handler = (event) => observer.next(event);
      element.addEventListener(eventName, handler);
      return () => element.removeEventListener(eventName, handler);
    });
  }

  static interval(intervalMs) {
    return new Observable(observer => {
      let count = 0;
      const id = setInterval(() => {
        observer.next(count++);
      }, intervalMs);
      return () => clearInterval(id);
    });
  }

  static timer(delayMs) {
    return new Observable(observer => {
      const id = setTimeout(() => {
        observer.next(0);
        observer.complete();
      }, delayMs);
      return () => clearTimeout(id);
    });
  }

  subscribe(observerOrNext, error, complete) {
    if (typeof observerOrNext === 'function') {
      observerOrNext = { next: observerOrNext, error, complete };
    }

    return this._subscribe(observerOrNext);
  }

  pipe(...operators) {
    return operators.reduce((acc, op) => op(acc), this);
  }
}

class ReactiveElement extends HTMLElement {
  static get observedAttributes() {
    return ['stream-name', 'auto-subscribe', 'debounce', 'filter-value'];
  }

  constructor() {
    super();
    this._stream = new BehaviorSubject(null);
    this._filteredStream = new Subject();
    this._subscription = null;
    this._streamName = 'default';
    this._autoSubscribe = false;
    this._debounceTime = REACTIVE_CONFIG.defaultDebounceTime;
    this._eventHistory = [];
    this._subscriptionCount = 0;
    
    this._initReactive();
    this._attachShadowDOM();
  }

  _initReactive() {
    this._setupFilterPipeline();
  }

  _setupFilterPipeline() {
    this._filteredStream = new Observable(observer => {
      return this._stream.subscribe({
        next: value => {
          if (value !== null && value !== undefined) {
            observer.next(value);
          }
        },
        error: observer.error,
        complete: observer.complete
      });
    }).pipe(
      this._filterOperator(),
      this._mapOperator()
    );
  }

  _filterOperator() {
    return {
      call: (source, predicate) => {
        return new Observable(observer => {
          return source.subscribe({
            next: value => {
              try {
                if (predicate(value)) {
                  observer.next(value);
                }
              } catch (error) {
                observer.error(error);
              }
            },
            error: observer.error,
            complete: observer.complete
          });
        });
      }
    };
  }

  _mapOperator() {
    return {
      call: (source, transform) => {
        return new Observable(observer => {
          return source.subscribe({
            next: value => {
              try {
                const result = transform(value);
                observer.next(result);
              } catch (error) {
                observer.error(error);
              }
            },
            error: observer.error,
            complete: observer.complete
          });
        });
      }
    };
  }

  _attachShadowDOM() {
    const shadow = this.attachShadow({ mode: 'open' });
    
    const style = document.createElement('style');
    style.textContent = `
      :host {
        display: block;
        padding: 20px;
        border: 2px solid #00bcd4;
        border-radius: 10px;
        background: linear-gradient(135deg, #e0f7fa 0%, #fff 100%);
        font-family: 'Roboto', system-ui, sans-serif;
      }

      :host([auto-subscribe]) {
        border-color: #4caf50;
        background: linear-gradient(135deg, #e8f5e9 0%, #fff 100%);
      }

      .reactive-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid rgba(0, 188, 212, 0.3);
      }

      .reactive-title {
        font-size: 16px;
        font-weight: 700;
        color: #006064;
      }

      .reactive-badge {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 11px;
        padding: 4px 10px;
        background: #00bcd4;
        color: white;
        border-radius: 12px;
      }

      .stream-visual {
        display: flex;
        flex-direction: column;
        gap: 12px;
        margin-bottom: 16px;
        padding: 16px;
        background: rgba(255, 255, 255, 0.7);
        border-radius: 8px;
      }

      .stream-item {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 10px 14px;
        background: white;
        border-radius: 6px;
        border: 1px solid #b2ebf2;
        transition: all 0.3s;
      }

      .stream-item.active {
        border-color: #00bcd4;
        box-shadow: 0 0 6px rgba(0, 188, 212, 0.3);
      }

      .stream-label {
        font-size: 12px;
        color: #00838f;
        font-weight: 500;
      }

      .stream-value {
        font-family: 'JetBrains Mono', monospace;
        font-size: 12px;
        color: #00acc1;
      }

      .main-stream {
        background: #e0f7fa;
        border-color: #00bcd4;
      }

      .main-stream .stream-label {
        color: #006064;
      }

      .controls {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 8px;
        margin-bottom: 16px;
      }

      .control-btn {
        padding: 10px 12px;
        border: 1px solid #0097a7;
        border-radius: 6px;
        background: white;
        color: #006064;
        font-size: 12px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
      }

      .control-btn:hover {
        background: #e0f7fa;
        border-color: #00bcd4;
      }

      .control-btn:active {
        background: #b2ebf2;
        transform: scale(0.98);
      }

      .event-log {
        background: #f5f5f5;
        border-radius: 6px;
        padding: 12px;
        max-height: 150px;
        overflow-y: auto;
      }

      .event-title {
        font-size: 12px;
        font-weight: 600;
        color: #616161;
        margin-bottom: 8px;
      }

      .event-item {
        padding: 4px 8px;
        margin: 2px 0;
        background: white;
        border-radius: 4px;
        font-size: 11px;
        font-family: monospace;
      }

      .event-item .time {
        color: #9e9e9e;
      }

      .event-item .value {
        color: #00acc1;
      }

      .metrics {
        display: flex;
        gap: 12px;
        margin-top: 12px;
        padding: 10px;
        background: #e8f5e9;
        border-radius: 6px;
      }

      .metric {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
      }

      .metric-label {
        font-size: 10px;
        color: #558b2f;
        text-transform: uppercase;
      }

      .metric-value {
        font-size: 18px;
        font-weight: 700;
        color: #2e7d32;
      }

      @keyframes emit {
        0% { transform: scale(1); }
        50% { transform: scale(1.05); }
        100% { transform: scale(1); }
      }

      .emitting {
        animation: emit 0.3s ease-in-out;
      }

      @keyframes fade {
        0% { opacity: 1; }
        100% { opacity: 0.3; }
      }

      .faded {
        animation: fade 2s ease-out forwards;
      }
    `;

    const container = document.createElement('div');
    container.innerHTML = `
      <div class="reactive-header">
        <span class="reactive-title">Reactive Stream</span>
        <span class="reactive-badge">
          <span id="active-indicator">●</span>
          <span id="stream-name-display">${this._streamName}</span>
        </span>
      </div>
      
      <div class="stream-visual">
        <div class="stream-item main-stream" id="main-stream">
          <span class="stream-label">Main Subject</span>
          <span class="stream-value" id="main-value">-</span>
        </div>
        <div class="stream-item" id="filtered-stream">
          <span class="stream-label">Filtered Stream</span>
          <span class="stream-value" id="filtered-value">-</span>
        </div>
        <div class="stream-item" id="behavior-stream">
          <span class="stream-label">Behavior Subject</span>
          <span class="stream-value" id="behavior-value">-</span>
        </div>
        <div class="stream-item" id="replay-stream">
          <span class="stream-label">Replay Subject</span>
          <span class="stream-value" id="replay-value">-</span>
        </div>
      </div>
      
      <div class="controls">
        <button class="control-btn" id="btn-emit">Emit</button>
        <button class="control-btn" id="btn-subscribe">Subscribe</button>
        <button class="control-btn" id="btn-unsubscribe">Unsubscribe</button>
      </div>
      
      <div class="event-log">
        <div class="event-title">Event Log</div>
        <div id="event-log"></div>
      </div>
      
      <div class="metrics">
        <div class="metric">
          <span class="metric-label">Events</span>
          <span class="metric-value" id="event-count">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Subscribers</span>
          <span class="metric-value" id="subscriber-count">0</span>
        </div>
        <div class="metric">
          <span class="metric-label">Buffer</span>
          <span class="metric-value" id="buffer-size">0</span>
        </div>
      </div>
    `;
    
    shadow.appendChild(style);
    shadow.appendChild(container);
  }

  connectedCallback() {
    this._bindEvents();
    
    if (this._autoSubscribe) {
      this._doSubscribe();
    }
    
    this._render();
    this._logEvent('system', 'initialized');
  }

  disconnectedCallback() {
    if (this._subscription) {
      this._subscription();
      this._subscription = null;
    }
    this._logEvent('system', 'destroyed');
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'stream-name':
        this._streamName = newValue;
        break;
      case 'auto-subscribe':
        this._autoSubscribe = newValue !== null;
        if (this._autoSubscribe && this.isConnected) {
          this._doSubscribe();
        }
        break;
      case 'debounce':
        this._debounceTime = parseInt(newValue, 10) || REACTIVE_CONFIG.defaultDebounceTime;
        this._setupFilterPipeline();
        break;
      case 'filter-value':
        this._setupFilterPipeline();
        break;
    }
    
    this._render();
  }

  _bindEvents() {
    const shadow = this.shadowRoot;
    
    const btnEmit = shadow.getElementById('btn-emit');
    const btnSubscribe = shadow.getElementById('btn-subscribe');
    const btnUnsubscribe = shadow.getElementById('btn-unsubscribe');

    btnEmit?.addEventListener('click', () => this.emit({ 
      data: Math.random() * 100,
      timestamp: Date.now()
    }));
    
    btnSubscribe?.addEventListener('click', () => this._doSubscribe());
    btnUnsubscribe?.addEventListener('click', () => this._doUnsubscribe());
  }

  emit(value) {
    this._stream.next(value);
    this._logEvent('emit', JSON.stringify(value));
    this._render();
  }

  _doSubscribe() {
    if (this._subscription) {
      return;
    }

    this._subscription = this._stream.subscribe({
      next: (value) => {
        this._updateStreamDisplay('main', value);
      },
      error: (error) => {
        this._logEvent('error', error.message);
      },
      complete: () => {
        this._logEvent('complete', 'Stream completed');
      }
    });

    this._subscriptionCount++;
    this._logEvent('subscribe', `Subscribed (${this._subscriptionCount})`);
    this._render();
  }

  _doUnsubscribe() {
    if (this._subscription) {
      this._subscription();
      this._subscription = null;
      this._logEvent('unsubscribe', 'Unsubscribed');
      this._render();
    }
  }

  subscribe(callback) {
    return this._stream.subscribe(callback);
  }

  _updateStreamDisplay(streamId, value) {
    const shadow = this.shadowRoot;
    const element = shadow?.querySelector(`[id="${streamId}-stream"]`);
    
    if (element) {
      element.classList.add('emitting');
      setTimeout(() => element.classList.remove('emitting'), 300);
    }

    const valueElement = shadow?.querySelector(`[id="${streamId}-value"]`);
    if (valueElement) {
      valueElement.textContent = typeof value === 'object' 
        ? JSON.stringify(value).substring(0, 30)
        : String(value);
    }
  }

  _logEvent(type, message) {
    this._eventHistory.push({
      type,
      message,
      timestamp: Date.now()
    });

    if (this._eventHistory.length > REACTIVE_CONFIG.bufferSize) {
      this._eventHistory.shift();
    }

    this._updateEventDisplay();
  }

  _render() {
    const shadow = this.shadowRoot;
    
    const streamNameDisplay = shadow?.getElementById('stream-name-display');
    const eventCount = shadow?.getElementById('event-count');
    const subscriberCount = shadow?.getElementById('subscriber-count');
    const bufferSize = shadow?.getElementById('buffer-size');

    if (streamNameDisplay) {
      streamNameDisplay.textContent = this._streamName;
    }

    if (eventCount) {
      eventCount.textContent = String(this._eventHistory.length);
    }

    if (subscriberCount) {
      subscriberCount.textContent = String(this._subscriptionCount);
    }

    if (bufferSize) {
      bufferSize.textContent = String(REACTIVE_CONFIG.bufferSize);
    }
  }

  _updateEventDisplay() {
    const shadow = this.shadowRoot;
    const eventLog = shadow?.getElementById('event-log');
    if (!eventLog) return;

    eventLog.innerHTML = this._eventHistory
      .slice(-10)
      .reverse()
      .map(entry => `
        <div class="event-item">
          <span class="time">[${new Date(entry.timestamp).toLocaleTimeString()}]</span>
          <span class="value">${entry.type}: ${entry.message}</span>
        </div>
      `).join('');
  }
}

if (!customElements.get('reactive-element')) {
  customElements.define('reactive-element', ReactiveElement);
}

export { 
  ReactiveElement, 
  Observable, 
  Subject, 
  BehaviorSubject, 
  ReplaySubject,
  Operator,
  REACTIVE_CONFIG,
  ReactiveError 
};