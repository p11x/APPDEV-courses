# Reactive Programming Patterns

## OVERVIEW

Reactive programming patterns enable components to respond automatically to data changes. This guide covers observers, signals, and reactive state management.

## IMPLEMENTATION DETAILS

### Simple Reactive System

```javascript
class ReactiveElement extends HTMLElement {
  #state = {};
  #effects = [];
  
  setState(updates) {
    const oldState = this.#state;
    this.#state = { ...this.#state, ...updates };
    
    // Run effects that depend on changed properties
    this.#runEffects(this.#state, oldState);
    this.render();
  }
  
  #runEffects(newState, oldState) {
    this.#effects.forEach(effect => effect(newState, oldState));
  }
  
  effect(fn) {
    this.#effects.push(fn);
  }
}
```

### Signal-Based Reactivity

```javascript
class Signal {
  #value;
  #subscribers = new Set();
  
  constructor(value) {
    this.#value = value;
  }
  
  get() { return this.#value; }
  
  set(value) {
    if (this.#value === value) return;
    this.#value = value;
    this.#notify();
  }
  
  subscribe(fn) {
    this.#subscribers.add(fn);
    fn(this.#value);
    return () => this.#subscribers.delete(fn);
  }
  
  #notify() {
    this.#subscribers.forEach(fn => fn(this.#value));
  }
}

class SignalComponent extends HTMLElement {
  #count = new Signal(0);
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.#count.subscribe(value => this.render());
    this.render();
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <button onclick="this.getRootNode().host.increment()">
        Count: ${this.#count.get()}
      </button>
    `;
  }
  
  increment() {
    this.#count.set(this.#count.get() + 1);
  }
}
```

## NEXT STEPS

Proceed to **05_Data-Binding/05_5_State-Management-Integration**.