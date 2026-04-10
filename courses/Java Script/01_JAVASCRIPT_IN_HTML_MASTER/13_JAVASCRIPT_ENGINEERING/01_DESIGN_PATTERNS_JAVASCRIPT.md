# Design Patterns in JavaScript

> Master essential design patterns for building scalable and maintainable JavaScript applications

## Table of Contents

1. [Introduction to Design Patterns](#introduction)
2. [Singleton Pattern](#singleton)
3. [Factory Pattern](#factory)
4. [Observer Pattern](#observer)
5. [Strategy Pattern](#strategy)
6. [Decorator Pattern](#decorator)
7. [Module Pattern](#module)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)
10. [Related Files](#related-files)

---

## 1. Introduction to Design Patterns

[anchor](#1-introduction-to-design-patterns)

Design patterns are reusable solutions to commonly occurring problems in software design. They represent best practices evolved over time through trial and error by countless developers. In JavaScript, understanding these patterns is crucial for building robust applications that are maintainable, scalable, and easy to understand.

### Why Design Patterns Matter

- **Reusability**: Patterns provide proven solutions that can be applied across different projects
- **Communication**: They provide a common vocabulary for developers to discuss code
- **Best Practices**: Patterns embody lessons learned from thousands of real-world applications
- **Maintainability**: Well-structured code using patterns is easier to maintain and extend

### Categories of Design Patterns

Design patterns are typically divided into three categories:

1. **Creational Patterns**: Deal with object creation mechanisms
2. **Structural Patterns**: Deal with object composition
3. **Behavioral Patterns**: Deal with object communication

---

## 2. Singleton Pattern

[anchor](#2-singleton-pattern)

The Singleton pattern ensures a class has only one instance while providing a global access point to this instance. This is particularly useful when you need exactly one object to coordinate actions across the system.

### Basic Singleton Implementation

```javascript
// file: patterns/Singleton.js
// Singletons ensure a class has only one instance

class DatabaseConnection {
  constructor() {
    if (DatabaseConnection.instance) {
      throw new Error('Use DatabaseConnection.getInstance() to get the instance');
    }
    this.connected = false;
    this.connectionString = null;
    DatabaseConnection.instance = this;
  }

  connect(connectionString) {
    console.log(`Connecting to: ${connectionString}`);
    this.connectionString = connectionString;
    this.connected = true;
    console.log('Connection established');
  }

  disconnect() {
    console.log('Disconnecting from database');
    this.connected = false;
    this.connectionString = null;
  }

  static getInstance() {
    if (!DatabaseConnection.instance) {
      DatabaseConnection.instance = new DatabaseConnection();
    }
    return DatabaseConnection.instance;
  }
}

// Usage
const db1 = DatabaseConnection.getInstance();
const db2 = DatabaseConnection.getInstance();

console.log('Same instance?', db1 === db2); // true
db1.connect('postgresql://localhost:5432/mydb');
console.log('db2 connected:', db2.connected); // true
```

### Modern Singleton with Private Fields

```javascript
// file: patterns/ModernSingleton.js
// Using ES2022+ private fields for true encapsulation

class ConfigurationManager {
  #config;
  #initialized = false;

  constructor() {
    if (ConfigurationManager.instance) {
      return ConfigurationManager.instance;
    }
    ConfigurationManager.instance = this;
  }

  initialize(defaultConfig) {
    if (this.#initialized) {
      console.warn('Configuration already initialized');
      return this;
    }
    this.#config = { ...defaultConfig };
    this.#initialized = true;
    return this;
  }

  get(key) {
    return this.#config[key];
  }

  set(key, value) {
    this.#config[key] = value;
    return this;
  }

  getAll() {
    return Object.freeze({ ...this.#config });
  }

  static getInstance() {
    ConfigurationManager.instance ??= new ConfigurationManager();
    return ConfigurationManager.instance;
  }
}

// Usage
const config1 = ConfigurationManager.getInstance();
config1.initialize({ theme: 'dark', language: 'en', apiUrl: 'https://api.example.com' });

const config2 = ConfigurationManager.getInstance();
console.log(config2.get('theme')); // 'dark'
console.log(config1 === config2); // true
```

### Professional Use Case: CORS Management

```javascript
// file: services/CorsManager.js
// Managing Cross-Origin Resource Sharing in a web application

class CorsManager {
  #allowedOrigins;
  #allowedMethods;
  #allowedHeaders;
  #maxAge;

  static #instance;

  constructor() {
    if (CorsManager.#instance) {
      return CorsManager.#instance;
    }
    this.#allowedOrigins = new Set();
    this.#allowedMethods = new Set(['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS']);
    this.#allowedHeaders = new Set(['Content-Type', 'Authorization']);
    this.#maxAge = 3600;
    CorsManager.#instance = this;
  }

  addOrigin(origin) {
    try {
      const url = new URL(origin);
      this.#allowedOrigins.add(url.origin);
    } catch {
      throw new Error(`Invalid origin: ${origin}`);
    }
    return this;
  }

  addOrigins(origins) {
    origins.forEach(origin => this.addOrigin(origin));
    return this;
  }

  addMethod(method) {
    this.#allowedMethods.add(method.toUpperCase());
    return this;
  }

  addHeader(header) {
    this.#allowedHeaders.add(header.toLowerCase());
    return this;
  }

  getCorsHeaders(requestOrigin) {
    if (!this.#allowedOrigins.has(requestOrigin) && this.#allowedOrigins.size > 0) {
      return null;
    }

    return {
      'Access-Control-Allow-Origin': requestOrigin || '*',
      'Access-Control-Allow-Methods': [...this.#allowedMethods].join(', '),
      'Access-Control-Allow-Headers': [...this.#allowedHeaders].join(', '),
      'Access-Control-Max-Age': this.#maxAge
    };
  }

  static getInstance() {
    CorsManager.#instance ??= new CorsManager();
    return CorsManager.#instance;
  }
}

// Usage in Express middleware
import express from 'express';
const app = express();

const cors = CorsManager.getInstance()
  .addOrigins([
    'https://app.example.com',
    'https://admin.example.com'
  ])
  .addMethod('PATCH')
  .addHeader('X-Request-ID');

app.use((req, res, next) => {
  const headers = cors.getCorsHeaders(req.headers.origin);
  if (headers) {
    Object.entries(headers).forEach(([key, value]) => {
      res.setHeader(key, value);
    });
  }
  next();
});
```

### Security Considerations for Singletons

- **Thread Safety**: In server-side JavaScript (Node.js), consider race conditions when using singletons
- **Testing Challenges**: Singletons can make unit testing difficult; consider dependency injection
- **Memory Leaks**: Ensure singletons properly clean up resources when no longer needed

---

## 3. Factory Pattern

[anchor](#3-factory-pattern)

The Factory pattern provides an interface for creating objects without specifying their exact class. This is useful when you need to create different types of objects based on conditions or when object creation involves complex logic.

### Simple Factory

```javascript
// file: patterns/Factory.js
// Factory pattern for creating different notification types

class EmailNotification {
  constructor() {
    this.type = 'email';
  }

  send(message) {
    console.log(`[Email] Sending: ${message}`);
    return { success: true, channel: 'email' };
  }
}

class SMSNotification {
  constructor() {
    this.type = 'sms';
  }

  send(message) {
    console.log(`[SMS] Sending: ${message}`);
    return { success: true, channel: 'sms' };
  }
}

class PushNotification {
  constructor() {
    this.type = 'push';
  }

  send(message) {
    console.log(`[Push] Sending: ${message}`);
    return { success: true, channel: 'push' };
  }
}

class NotificationFactory {
  static create(type) {
    const factories = {
      email: EmailNotification,
      sms: SMSNotification,
      push: PushNotification
    };

    const NotificationClass = factories[type.toLowerCase()];
    if (!NotificationClass) {
      throw new Error(`Unknown notification type: ${type}`);
    }

    return new NotificationClass();
  }
}

// Usage
const notifier = NotificationFactory.create('email');
notifier.send('Your order has been shipped!');

const smsNotifier = NotificationFactory.create('sms');
smsNotifier.send('Your verification code is 123456');
```

### Advanced Factory with Configuration

```javascript
// file: patterns/AdvancedFactory.js
// Factory with configuration and validation

class PaymentProcessor {
  constructor(config) {
    this.config = config;
  }

  process(amount) {
    throw new Error('process() must be implemented');
  }
}

class CreditCardProcessor extends PaymentProcessor {
  process(amount) {
    console.log(`Processing credit card payment of $${amount}`);
    return { 
      success: true, 
      method: 'credit_card',
      amount,
      transactionId: `txn_${Date.now()}`
    };
  }
}

class PayPalProcessor extends PaymentProcessor {
  process(amount) {
    console.log(`Processing PayPal payment of $${amount}`);
    return { 
      success: true, 
      method: 'paypal',
      amount,
      transactionId: `pp_${Date.now()}`
    };
  }
}

class CryptoProcessor extends PaymentProcessor {
  #exchangeRate = 45000;

  process(amount) {
    const btcAmount = amount / this.#exchangeRate;
    console.log(`Processing crypto payment of ${btcAmount} BTC`);
    return { 
      success: true, 
      method: 'crypto',
      amount,
      btcAmount,
      transactionId: `crypto_${Date.now()}`
    };
  }
}

class PaymentProcessorFactory {
  #processors = new Map();

  constructor() {
    this.register('credit_card', CreditCardProcessor);
    this.register('paypal', PayPalProcessor);
    this.register('crypto', CryptoProcessor);
  }

  register(type, processorClass) {
    this.#processors.set(type, processorClass);
  }

  create(type, config = {}) {
    const ProcessorClass = this.#processors.get(type);
    if (!ProcessorClass) {
      const available = [...this.#processors.keys()].join(', ');
      throw new Error(`Unknown processor: ${type}. Available: ${available}`);
    }

    return new ProcessorClass(config);
  }

  getAvailableTypes() {
    return [...this.#processors.keys()];
  }
}

// Usage
const factory = new PaymentProcessorFactory();
const creditPayment = factory.create('credit_card');
const result = creditPayment.process(99.99);
console.log('Payment result:', result);
```

### Professional Use Case: Plugin System

```javascript
// file: plugins/PluginFactory.js
// Factory pattern for a plugin system in a text editor

class EditorPlugin {
  constructor(name) {
    this.name = name;
    this.enabled = true;
  }

  initialize(editor) {
    this.editor = editor;
  }

  execute(...args) {
    throw new Error('execute() must be implemented');
  }

  destroy() {
    this.enabled = false;
  }
}

class FormatPlugin extends EditorPlugin {
  constructor() {
    super('format');
  }

  execute(selection, format) {
    if (!this.enabled) return selection;
    return `<${format}>${selection}</${format}>`;
  }
}

class ValidatePlugin extends EditorPlugin {
  constructor() {
    super('validate');
  }

  execute(content) {
    const errors = [];
    if (content.length === 0) {
      errors.push('Content cannot be empty');
    }
    if (content.length > 10000) {
      errors.push('Content exceeds maximum length');
    }
    return { valid: errors.length === 0, errors };
  }
}

class SpellCheckPlugin extends EditorPlugin {
  #dictionary = new Set(['javascript', 'programming', 'software']);

  constructor() {
    super('spellcheck');
  }

  execute(content) {
    const words = content.toLowerCase().match(/\b\w+\b/g) || [];
    const misspellings = words.filter(word => !this.#dictionary.has(word));
    return { correct: misspellings.length === 0, misspellings };
  }

  addToDictionary(word) {
    this.#dictionary.add(word.toLowerCase());
  }
}

class PluginFactory {
  #plugins = new Map();

  register(name, PluginClass) {
    this.#plugins.set(name, PluginClass);
  }

  create(name, config = {}) {
    const PluginClass = this.#plugins.get(name);
    if (!PluginClass) {
      throw new Error(`Plugin not found: ${name}`);
    }
    return new PluginClass(config);
  }

  listAvailable() {
    return [...this.#plugins.keys()];
  }
}

// Usage
const pluginFactory = new PluginFactory();
pluginFactory.register('format', FormatPlugin);
pluginFactory.register('validate', ValidatePlugin);
pluginFactory.register('spellcheck', SpellCheckPlugin);

const formatPlugin = pluginFactory.create('format');
const result = formatPlugin.execute('hello world', 'strong');
console.log('Formatted:', result);
```

---

## 4. Observer Pattern

[anchor](#4-observer-pattern)

The Observer pattern defines a one-to-many dependency between objects. When one object changes state, all its dependents are notified automatically. This pattern is fundamental to event-driven architectures.

### Basic Observer Implementation

```javascript
// file: patterns/Observer.js
// Event emitter implementation following the Observer pattern

class EventEmitter {
  #events = new Map();

  on(event, listener) {
    if (!this.#events.has(event)) {
      this.#events.set(event, new Set());
    }
    this.#events.get(event).add(listener);
    
    // Return unsubscribe function
    return () => this.off(event, listener);
  }

  off(event, listener) {
    const listeners = this.#events.get(event);
    if (listeners) {
      listeners.delete(listener);
    }
  }

  emit(event, ...args) {
    const listeners = this.#events.get(event);
    if (listeners) {
      listeners.forEach(listener => {
        try {
          listener(...args);
        } catch (error) {
          console.error(`Error in listener for ${event}:`, error);
        }
      });
    }
    return this;
  }

  once(event, listener) {
    const wrapper = (...args) => {
      listener(...args);
      this.off(event, wrapper);
    };
    return this.on(event, wrapper);
  }

  listenerCount(event) {
    return this.#events.get(event)?.size || 0;
  }

  removeAllListeners(event) {
    if (event) {
      this.#events.delete(event);
    } else {
      this.#events.clear();
    }
    return this;
  }
}

// Usage
const emitter = new EventEmitter();

const logListener = (data) => console.log('Logged:', data);
const notifyListener = (data) => console.log('Notified:', data);

emitter.on('data', logListener);
emitter.on('data', notifyListener);
emitter.emit('data', { message: 'Hello!' });
// Output:
// Logged: { message: 'Hello!' }
// Notified: { message: 'Hello!' }

emitter.off('data', logListener);
emitter.emit('data', { message: 'Goodbye!' });
// Output:
// Notified: { message: 'Goodbye!' }
```

### Advanced Observer with Priority and Filtering

```javascript
// file: patterns/AdvancedObserver.js
// Enhanced event emitter with priority and filtering

class PriorityEventEmitter {
  #events = new Map();

  on(event, listener, { priority = 0 } = {}) {
    if (!this.#events.has(event)) {
      this.#events.set(event, []);
    }
    
    const handlers = this.#events.get(event);
    handlers.push({ listener, priority });
    handlers.sort((a, b) => b.priority - a.priority);
    
    const handlerIndex = handlers.length - 1;
    
    return () => {
      handlers.splice(handlerIndex, 1);
    };
  }

  onFiltered(event, filter, listener, options = {}) {
    const wrappedListener = (...args) => {
      if (filter(...args)) {
        listener(...args);
      }
    };
    return this.on(event, wrappedListener, options);
  }

  emit(event, ...args) {
    const handlers = this.#events.get(event);
    if (!handlers) return [];

    const results = [];
    for (const { listener } of handlers) {
      try {
        const result = listener(...args);
        if (result !== undefined) {
          results.push(result);
        }
      } catch (error) {
        console.error(`Error emitting ${event}:`, error);
      }
    }
    return results;
  }

  once(event, listener, options = {}) {
    let called = false;
    const wrapper = (...args) => {
      if (!called) {
        called = true;
        listener(...args);
      }
    };
    return this.on(event, wrapper, options);
  }
}

// Usage
const emitter = new PriorityEventEmitter();

// Priority: high priority listeners execute first
emitter.on('order', (order) => console.log('1. High priority:', order.status), { priority: 100 });
emitter.on('order', (order) => console.log('2. Normal priority:', order.id), { priority: 50 });
emitter.on('order', (order) => console.log('3. Low priority:', order.total), { priority: 10 });

emitter.emit('order', { id: 'ORD-001', status: 'processing', total: 99.99 });
// Output:
// 1. High priority: processing
// 2. Normal priority: ORD-001
// 3. Low priority: 99.99
```

### Professional Use Case: Real-time Data Binding

```javascript
// file: reactivity/ReactiveStore.js
// Reactive store using Observer pattern for state management

class ReactiveStore {
  #state = new Map();
  #emitter = new PriorityEventEmitter();
  #computed = new Map();
  #batch = false;
  #pendingUpdates = new Set();

  constructor(initialState = {}) {
    Object.entries(initialState).forEach(([key, value]) => {
      this.#state.set(key, value);
    });
  }

  get(key) {
    return this.#state.get(key);
  }

  set(key, value) {
    const oldValue = this.#state.get(key);
    if (oldValue === value) return;

    this.#state.set(key, value);
    
    if (this.#batch) {
      this.#pendingUpdates.add(key);
    } else {
      this.#notify(key, value, oldValue);
    }
    
    this.#recompute(key);
  }

  subscribe(key, listener, options = {}) {
    return this.#emitter.on(key, listener, options);
  }

  subscribeAny(listener) {
    return this.#emitter.on('*', listener);
  }

  batch Updates(callback) {
    this.#batch = true;
    this.#pendingUpdates.clear();
    
    callback();
    
    this.#pendingUpdates.forEach(key => {
      this.#notify(key, this.#state.get(key), undefined);
    });
    
    this.#batch = false;
    this.#pendingUpdates.clear();
  }

  compute(key, computeFn) {
    this.#computed.set(key, computeFn);
    this.#recompute(key);
  }

  #recompute(changedKey) {
    this.#computed.forEach((computeFn, key) => {
      try {
        const newValue = computeFn(this.#state);
        this.#state.set(key, newValue);
        this.#notify(key, newValue, undefined);
      } catch (error) {
        console.error(`Error computing ${key}:`, error);
      }
    });
  }

  #notify(key, newValue, oldValue) {
    this.#emitter.emit(key, newValue, oldValue);
    this.#emitter.emit('*', key, newValue, oldValue);
  }

  getState() {
    return Object.fromEntries(this.#state);
  }
}

// Usage
const store = new ReactiveStore({
  user: null,
  cart: [],
  total: 0
});

// Subscribe to specific changes
store.subscribe('cart', (cart) => {
  console.log('Cart updated:', cart.length, 'items');
});

// Subscribe with computed values
store.compute('total', (state) => {
  return state.cart.reduce((sum, item) => sum + item.price * item.qty, 0);
});

store.subscribe('total', (total) => {
  console.log('Total updated: $' + total.toFixed(2));
});

// Update state
store.set('cart', [{ id: 1, name: 'Widget', price: 9.99, qty: 2 }]);
// Output:
// Cart updated: 1 items
// Total updated: $19.98
```

---

## 5. Strategy Pattern

[anchor](#5-strategy-pattern)

The Strategy pattern defines a family of algorithms, encapsulates each one, and makes them interchangeable. It lets the algorithm vary independently from clients that use it.

### Basic Strategy Pattern

```javascript
// file: patterns/Strategy.js
// Strategy pattern for payment processing

class PaymentStrategy {
  pay(amount) {
    throw new Error('pay() must be implemented');
  }
}

class CreditCardStrategy extends PaymentStrategy {
  #cardNumber;
  #cvv;
  #expiry;

  constructor(cardNumber, cvv, expiry) {
    super();
    this.#cardNumber = cardNumber;
    this.#cvv = cvv;
    this.#expiry = expiry;
  }

  pay(amount) {
    // Simulate payment processing
    console.log(`Processing credit card payment: $${amount}`);
    console.log(`Card: ****${this.#cardNumber.slice(-4)}`);
    return { success: true, method: 'credit_card', amount };
  }
}

class PayPalStrategy extends PaymentStrategy {
  #email;

  constructor(email) {
    super();
    this.#email = email;
  }

  pay(amount) {
    console.log(`Processing PayPal payment: $${amount}`);
    console.log(`Account: ${this.#email}`);
    return { success: true, method: 'paypal', amount };
  }
}

class CryptoStrategy extends PaymentStrategy {
  #wallet;

  constructor(wallet) {
    super();
    this.#wallet = wallet;
  }

  pay(amount) {
    const btcAmount = (amount / 45000).toFixed(8);
    console.log(`Processing crypto payment: ${btcAmount} BTC`);
    console.log(`Wallet: ${this.#wallet.slice(0, 8)}...`);
    return { success: true, method: 'crypto', amount, btcAmount };
  }
}

class ShoppingCart {
  #items = [];
  #paymentStrategy;

  addItem(item) {
    this.#items.push(item);
  }

  removeItem(itemId) {
    const index = this.#items.findIndex(item => item.id === itemId);
    if (index > -1) {
      this.#items.splice(index, 1);
    }
  }

  setPaymentStrategy(strategy) {
    this.#paymentStrategy = strategy;
  }

  getTotal() {
    return this.#items.reduce((sum, item) => sum + item.price, 0);
  }

  checkout() {
    if (!this.#paymentStrategy) {
      throw new Error('No payment strategy set');
    }
    if (this.#items.length === 0) {
      throw new Error('Cart is empty');
    }

    const total = this.getTotal();
    return this.#paymentStrategy.pay(total);
  }
}

// Usage
const cart = new ShoppingCart();
cart.addItem({ id: 1, name: 'Book', price: 29.99 });
cart.addItem({ id: 2, name: 'Coffee', price: 4.99 });

// Pay with credit card
cart.setPaymentStrategy(new CreditCardStrategy('4111111111111111', '123', '12/25'));
console.log(cart.checkout());

// Switch to PayPal
cart.setPaymentStrategy(new PayPalStrategy('user@example.com'));
console.log(cart.checkout());
```

### Advanced Strategy with Validation

```javascript
// file: patterns/ValidationStrategy.js
// Strategy pattern for input validation

class ValidationStrategy {
  validate(value) {
    throw new Error('validate() must be implemented');
  }

  getErrorMessage() {
    return 'Validation failed';
  }
}

class EmailValidation extends ValidationStrategy {
  validate(value) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(value);
  }

  getErrorMessage() {
    return 'Invalid email format';
  }
}

class PasswordValidation extends ValidationStrategy {
  #minLength;
  #requireUppercase;
  #requireNumber;
  #requireSpecial;

  constructor(options = {}) {
    super();
    this.#minLength = options.minLength || 8;
    this.#requireUppercase = options.requireUppercase ?? true;
    this.#requireNumber = options.requireNumber ?? true;
    this.#requireSpecial = options.requireSpecial ?? true;
  }

  validate(value) {
    const errors = [];

    if (value.length < this.#minLength) {
      errors.push(`Password must be at least ${this.#minLength} characters`);
    }
    if (this.#requireUppercase && !/[A-Z]/.test(value)) {
      errors.push('Password must contain at least one uppercase letter');
    }
    if (this.#requireNumber && !/[0-9]/.test(value)) {
      errors.push('Password must contain at least one number');
    }
    if (this.#requireSpecial && !/[!@#$%^&*(),.?":{}|<>]/.test(value)) {
      errors.push('Password must contain at least one special character');
    }

    return { valid: errors.length === 0, errors };
  }
}

class PhoneValidation extends ValidationStrategy {
  #countryCode;

  constructor(countryCode = '+1') {
    super();
    this.#countryCode = countryCode;
  }

  validate(value) {
    const cleaned = value.replace(/\D/g, '');
    return cleaned.length >= 10 && cleaned.length <= 11;
  }

  getErrorMessage() {
    return 'Invalid phone number format';
  }
}

class FormValidator {
  #validations = new Map();

  addField(fieldName, strategy) {
    this.#validations.set(fieldName, strategy);
  }

  validate(data) {
    const results = {};
    let isValid = true;

    this.#validations.forEach((strategy, fieldName) => {
      const value = data[fieldName];
      const fieldValid = strategy.validate(value);
      results[fieldName] = {
        valid: typeof fieldValid === 'boolean' ? fieldValid : fieldValid.valid,
        error: typeof fieldValid === 'object' ? fieldValid.errors : null
      };
      if (!results[fieldName].valid) {
        isValid = false;
      }
    });

    return { valid: isValid, results };
  }
}

// Usage
const validator = new FormValidator();

validator.addField('email', new EmailValidation());
validator.addField('password', new PasswordValidation({
  minLength: 12,
  requireSpecial: true
}));
validator.addField('phone', new PhoneValidation('+1'));

const formData = {
  email: 'user@example.com',
  password: 'SecurePass123!',
  phone: '+1-555-123-4567'
};

const result = validator.validate(formData);
console.log('Validation result:', result);
```

### Professional Use Case: Sorting Algorithms

```javascript
// file: algorithms/SortStrategy.js
// Strategy pattern for swappable sorting algorithms

class SortStrategy {
  sort(array) {
    throw new Error('sort() must be implemented');
  }
}

class QuickSortStrategy extends SortStrategy {
  sort(array) {
    if (array.length <= 1) return [...array];
    
    const pivot = array[Math.floor(array.length / 2)];
    const left = array.filter(x => x < pivot);
    const middle = array.filter(x => x === pivot);
    const right = array.filter(x => x > pivot);
    
    return [
      ...this.sort(left),
      ...middle,
      ...this.sort(right)
    ];
  }
}

class MergeSortStrategy extends SortStrategy {
  sort(array) {
    if (array.length <= 1) return [...array];
    
    const mid = Math.floor(array.length / 2);
    const left = this.sort(array.slice(0, mid));
    const right = this.sort(array.slice(mid));
    
    return this.#merge(left, right);
  }

  #merge(left, right) {
    const result = [];
    let i = 0, j = 0;

    while (i < left.length && j < right.length) {
      if (left[i] <= right[j]) {
        result.push(left[i++]);
      } else {
        result.push(right[j++]);
      }
    }

    return [...result, ...left.slice(i), ...right.slice(j)];
  }
}

class BubbleSortStrategy extends SortStrategy {
  sort(array) {
    const result = [...array];
    const n = result.length;

    for (let i = 0; i < n - 1; i++) {
      for (let j = 0; j < n - i - 1; j++) {
        if (result[j] > result[j + 1]) {
          [result[j], result[j + 1]] = [result[j + 1], result[j]];
        }
      }
    }

    return result;
  }
}

class DataSorter {
  #data;
  #strategy;
  #metrics = { comparisons: 0, swaps: 0 };

  constructor(data, strategy) {
    this.#data = [...data];
    this.#strategy = strategy;
  }

  setStrategy(strategy) {
    this.#strategy = strategy;
  }

  sort() {
    this.#metrics = { comparisons: 0, swaps: 0 };
    const startTime = performance.now();
    const result = this.#strategy.sort(this.#data);
    const endTime = performance.now();

    return {
      data: result,
      time: endTime - startTime,
      metrics: this.#metrics
    };
  }
}

// Usage with performance comparison
const data = [64, 34, 25, 12, 22, 11, 90, 45, 33, 78];

console.log('QuickSort:');
const quickSorter = new DataSorter(data, new QuickSortStrategy());
console.log(quickSorter.sort());

console.log('\nMergeSort:');
const mergeSorter = new DataSorter(data, new MergeSortStrategy());
console.log(mergeSorter.sort());

console.log('\nBubbleSort:');
const bubbleSorter = new DataSorter(data, new BubbleSortStrategy());
console.log(bubbleSorter.sort());
```

---

## 6. Decorator Pattern

[anchor](#6-decorator-pattern)

The Decorator pattern attaches additional responsibilities to an object dynamically. Decorators provide a flexible alternative to subclassing for extending functionality.

### Basic Decorator

```javascript
// file: patterns/Decorator.js
// Decorator pattern for adding functionality to components

class Coffee {
  getCost() {
    return 5;
  }

  getDescription() {
    return 'Coffee';
  }
}

class CoffeeDecorator {
  #coffee;

  constructor(coffee) {
    this.#coffee = coffee;
  }

  getCost() {
    return this.#coffee.getCost();
  }

  getDescription() {
    return this.#coffee.getDescription();
  }
}

class MilkDecorator extends CoffeeDecorator {
  getCost() {
    return super.getCost() + 1.5;
  }

  getDescription() {
    return super.getDescription() + ', Milk';
  }
}

class SugarDecorator extends CoffeeDecorator {
  getCost() {
    return super.getCost() + 0.5;
  }

  getDescription() {
    return super.getDescription() + ', Sugar';
  }
}

class WhipDecorator extends CoffeeDecorator {
  getCost() {
    return super.getCost() + 2;
  }

  getDescription() {
    return super.getDescription() + ', Whipped Cream';
  }
}

class CaramelDecorator extends CoffeeDecorator {
  getCost() {
    return super.getCost() + 1;
  }

  getDescription() {
    return super.getDescription() + ', Caramel';
  }
}

// Usage
let myCoffee = new Coffee();
console.log(myCoffee.getDescription(), '-', '$' + myCoffee.getCost());

myCoffee = new MilkDecorator(myCoffee);
console.log(myCoffee.getDescription(), '-', '$' + myCoffee.getCost());

myCoffee = new SugarDecorator(myCoffee);
console.log(myCoffee.getDescription(), '-', '$' + myCoffee.getCost());

myCoffee = new WhipDecorator(myCoffee);
console.log(myCoffee.getDescription(), '-', '$' + myCoffee.getCost());
// Output:
// Coffee - $5
// Coffee, Milk - $6.5
// Coffee, Milk, Sugar - $7
// Coffee, Milk, Sugar, Whipped Cream - $9
```

### Function Decorators

```javascript
// file: patterns/FunctionDecorators.js
// Decorator pattern for functions using higher-order functions

function withLogging(fn) {
  return function(...args) {
    console.log(`Calling ${fn.name} with:`, args);
    const result = fn.apply(this, args);
    console.log(`${fn.name} returned:`, result);
    return result;
  };
}

function withTiming(fn) {
  return function(...args) {
    const start = performance.now();
    const result = fn.apply(this, args);
    const end = performance.now();
    console.log(`${fn.name} took ${(end - start).toFixed(2)}ms`);
    return result;
  };
}

function withCaching(fn) {
  const cache = new Map();
  
  return function(...args) {
    const key = JSON.stringify(args);
    
    if (cache.has(key)) {
      console.log(`Cache hit for ${fn.name}`);
      return cache.get(key);
    }
    
    const result = fn.apply(this, args);
    cache.set(key, result);
    return result;
  };
}

function withRetry(fn, { maxRetries = 3, delay = 1000 } = {}) {
  return async function(...args) {
    let lastError;
    
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
      try {
        return await fn.apply(this, args);
      } catch (error) {
        lastError = error;
        console.log(`Attempt ${attempt} failed, retrying...`);
        if (attempt < maxRetries) {
          await new Promise(resolve => setTimeout(resolve, delay));
        }
      }
    }
    
    throw lastError;
  };
}

function withValidation(validator, fn) {
  return function(...args) {
    const validation = validator(...args);
    if (!validation.valid) {
      throw new Error(`Validation failed: ${validation.errors.join(', ')}`);
    }
    return fn.apply(this, args);
  };
}

// Usage
const calculateFibonacci = (n) => {
  if (n <= 1) return n;
  return calculateFibonacci(n - 1) + calculateFibonacci(n - 2);
};

const fastFibonacci = withCaching(withTiming(calculateFibonacci));

console.log('Calculating Fibonacci(20):');
console.log('Result:', fastFibonacci(20));

console.log('\nCalculating again (should use cache):');
console.log('Result:', fastFibonacci(20));
```

### Professional Use Case: API Middleware

```javascript
// file: api/MiddlewareDecorators.js
// Decorator pattern for API request/response handling

class APIRequest {
  constructor(url, options = {}) {
    this.url = url;
    this.method = options.method || 'GET';
    this.headers = options.headers || {};
    this.body = options.body;
  }
}

class APIResponse {
  constructor(data, status, headers) {
    this.data = data;
    this.status = status;
    this.headers = headers;
    this.ok = status >= 200 && status < 300;
  }
}

function withAuth(apiCall) {
  return async function(token, ...args) {
    const request = args[0];
    request.headers['Authorization'] = `Bearer ${token}`;
    return apiCall(...args);
  };
}

function withRetry(apiCall, { maxRetries = 3, retryOn = [408, 429, 500, 502, 503, 504] } = {}) {
  return async function(...args) {
    let lastError;
    
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
      try {
        return await apiCall(...args);
      } catch (error) {
        if (!retryOn.includes(error.status) || attempt === maxRetries) {
          throw error;
        }
        console.log(`Request failed, retrying in ${attempt * 1000}ms...`);
        await new Promise(r => setTimeout(r, attempt * 1000));
      }
    }
  };
}

function withLogging(apiCall) {
  return async function(...args) {
    const request = args[0];
    console.log(`API Request: ${request.method} ${request.url}`);
    
    const response = await apiCall(...args);
    
    console.log(`API Response: ${response.status}`);
    return response;
  };
}

function withCache(apiCall, { ttl = 60000 } = {}) {
  const cache = new Map();
  
  return async function(...args) {
    const request = args[0];
    const cacheKey = `${request.method}:${request.url}`;
    
    const cached = cache.get(cacheKey);
    if (cached && Date.now() - cached.timestamp < ttl) {
      console.log('Cache hit');
      return cached.response;
    }
    
    const response = await apiCall(...args);
    cache.set(cacheKey, { response, timestamp: Date.now() });
    return response;
  };
}

function withErrorHandling(apiCall) {
  return async function(...args) {
    try {
      const response = await apiCall(...args);
      
      if (!response.ok) {
        throw new APIError(response.status, response.data);
      }
      
      return response;
    } catch (error) {
      if (error instanceof APIError) {
        console.error(`API Error ${error.status}:`, error.message);
      } else {
        console.error('Network Error:', error.message);
      }
      throw error;
    }
  };
}

class APIError extends Error {
  constructor(status, message) {
    super(message);
    this.status = status;
  }
}

// Usage
const fetchAPI = async (request) => {
  console.log('Making actual API call...');
  return new APIResponse({ data: 'success' }, 200, {});
};

const wrappedAPI = withLogging(withRetry(withCache(withErrorHandling(fetchAPI)));

// Simulate usage
(async () => {
  const request = new APIRequest('https://api.example.com/data');
  try {
    const response = await wrappedAPI(request);
    console.log('Final response:', response.data);
  } catch (error) {
    console.error('Failed:', error.message);
  }
})();
```

---

## 7. Module Pattern

[anchor](#7-module-pattern)

The Module pattern encapsulates code into self-contained units that expose a public API while keeping private state hidden. This is fundamental to creating maintainable JavaScript applications.

### Classic Module Pattern

```javascript
// file: patterns/Module.js
// Classic module pattern using closures

const UserManager = (function() {
  // Private state
  const users = new Map();
  let nextId = 1;

  // Private functions
  function generateId() {
    return `user_${nextId++}`;
  }

  function validateUser(user) {
    if (!user.name || typeof user.name !== 'string') {
      return { valid: false, error: 'Name is required' };
    }
    if (!user.email || !emailRegex.test(user.email)) {
      return { valid: false, error: 'Valid email is required' };
    }
    return { valid: true };
  }

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  // Public API
  return {
    create(userData) {
      const validation = validateUser(userData);
      if (!validation.valid) {
        throw new Error(validation.error);
      }

      const user = {
        id: generateId(),
        name: userData.name,
        email: userData.email,
        createdAt: new Date().toISOString()
      };

      users.set(user.id, user);
      return user;
    },

    get(userId) {
      return users.get(userId) || null;
    },

    getAll() {
      return Array.from(users.values());
    },

    update(userId, updates) {
      const user = users.get(userId);
      if (!user) {
        throw new Error('User not found');
      }

      const updatedUser = { ...user, ...updates, id: userId };
      const validation = validateUser(updatedUser);
      if (!validation.valid) {
        throw new Error(validation.error);
      }

      users.set(userId, updatedUser);
      return updatedUser;
    },

    delete(userId) {
      return users.delete(userId);
    },

    count() {
      return users.size;
    }
  };
})();

// Usage
const user1 = UserManager.create({ name: 'Alice', email: 'alice@example.com' });
const user2 = UserManager.create({ name: 'Bob', email: 'bob@example.com' });

console.log('All users:', UserManager.getAll());
console.log('Count:', UserManager.count());
console.log('Get Alice:', UserManager.get(user1.id));
```

### Modern ES Modules

```javascript
// file: modules/userRepository.js
// Modern ES Module pattern with exports

// Private implementation
class UserRepositoryImpl {
  #users = new Map();
  #eventEmitter = new PriorityEventEmitter();

  async create(userData) {
    const user = {
      id: crypto.randomUUID(),
      ...userData,
      createdAt: new Date().toISOString()
    };
    this.#users.set(user.id, user);
    this.#emit('created', user);
    return user;
  }

  async findById(id) {
    return this.#users.get(id) || null;
  }

  async findAll() {
    return Array.from(this.#users.values());
  }

  async update(id, updates) {
    const existing = this.#users.get(id);
    if (!existing) {
      throw new Error('User not found');
    }
    const updated = { ...existing, ...updates, updatedAt: new Date().toISOString() };
    this.#users.set(id, updated);
    this.#emit('updated', updated);
    return updated;
  }

  async delete(id) {
    const deleted = this.#users.delete(id);
    if (deleted) {
      this.#emit('deleted', { id });
    }
    return deleted;
  }

  subscribe(event, listener) {
    return this.#eventEmitter.on(event, listener);
  }

  #emit(event, data) {
    this.#eventEmitter.emit(event, data);
  }
}

// Singleton instance
let repositoryInstance = null;

export function getRepository() {
  repositoryInstance ??= new UserRepositoryImpl();
  return repositoryInstance;
}

export function createRepository() {
  return new UserRepositoryImpl();
}

export default { getRepository, createRepository };
```

```javascript
// file: modules/userService.js
// ES Module service layer

import { getRepository } from './userRepository.js';

class UserService {
  #repository;

  constructor() {
    this.#repository = getRepository();
  }

  async registerUser(userData) {
    if (!userData.email || !userData.password) {
      throw new Error('Email and password are required');
    }

    if (userData.password.length < 8) {
      throw new Error('Password must be at least 8 characters');
    }

    const hashedPassword = await this.#hashPassword(userData.password);
    
    return this.#repository.create({
      ...userData,
      password: hashedPassword
    });
  }

  async #hashPassword(password) {
    const encoder = new TextEncoder();
    const data = encoder.encode(password);
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    return hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
  }

  async getUser(id) {
    return this.#repository.findById(id);
  }

  async getAllUsers() {
    return this.#repository.findAll();
  }

  subscribeToUpdates(callback) {
    return this.#repository.subscribe('updated', callback);
  }
}

export { UserService };
export default UserService;
```

### Dynamic Module Loading

```javascript
// file: modules/dynamicLoader.js
// Dynamic module loading for large applications

class ModuleLoader {
  #modules = new Map();
  #loading = new Map();

  async load(modulePath) {
    if (this.#modules.has(modulePath)) {
      return this.#modules.get(modulePath);
    }

    if (this.#loading.has(modulePath)) {
      return this.#loading.get(modulePath);
    }

    const loadPromise = this.#doLoad(modulePath);
    this.#loading.set(modulePath, loadPromise);

    try {
      const module = await loadPromise;
      this.#modules.set(modulePath, module);
      return module;
    } finally {
      this.#loading.delete(modulePath);
    }
  }

  async #doLoad(modulePath) {
    const module = await import(modulePath);
    return module.default || module;
  }

  preload(modulePaths) {
    return Promise.all(modulePaths.map(path => this.load(path)));
  }

  getLoaded() {
    return [...this.#modules.keys()];
  }

  clearCache() {
    this.#modules.clear();
  }
}

// Usage
const loader = new ModuleLoader();

// Lazy load modules as needed
async function initializeApp() {
  const uiModule = await loader.load('./modules/ui.js');
  const apiModule = await loader.load('./modules/api.js');
  const storageModule = await loader.load('./modules/storage.js');

  uiModule.initialize();
  await apiModule.connect();
  storageModule.restore();

  console.log('Loaded modules:', loader.getLoaded());
}
```

---

## Key Takeaways

- **Singleton Pattern**: Use when you need exactly one instance with global access. Consider testing implications and potential memory leaks.
- **Factory Pattern**: Ideal for creating objects with complex initialization or when the exact type isn't known until runtime.
- **Observer Pattern**: Foundation of event-driven programming. Essential for building reactive systems and decoupled components.
- **Strategy Pattern**: Allows swapping algorithms at runtime. Useful for payment processing, validation, and sorting.
- **Decorator Pattern**: Add functionality without modifying original classes. Perfect for cross-cutting concerns like logging.
- **Module Pattern**: Encapsulates private state while exposing a clean public API. Fundamental to building maintainable applications.

### Performance Considerations

- **Singletons**: Can create bottlenecks in highly concurrent applications
- **Observers**: Memory leaks occur if listeners aren't properly removed
- **Decorators**: Chain length affects performance; use judiciously
- **Modules**: Dynamic imports improve initial load time but add complexity

### Security Best Practices

- Never store sensitive data in closure-based private fields (use encryption)
- Validate all inputs to factory and strategy classes
- Implement proper cleanup to prevent resource leaks
- Use secure random generation for IDs and tokens

---

## Common Pitfalls

1. **Overusing Singletons**: Makes testing difficult and creates tight coupling
2. **Memory Leaks in Observers**: Always return unsubscribe functions and call them
3. **Circular Dependencies**: Avoid in modules; use dependency injection instead
4. **Forgetting Error Handling**: Decorators and strategies should handle errors gracefully
5. **Excessive Decoration Chains**: Can make code harder to debug and understand

---

## Related Files

- [SOLID Principles in JavaScript](./02_SOLID_PRINCIPLES_JAVASCRIPT.md) - Learn how these patterns support SOLID principles
- [Code Organization and Structure](./04_CODE_ORGANIZATION_AND_STRUCTURE.md) - Advanced module patterns and project structure
- [JavaScript Performance Engineering](./05_JAVASCRIPT_PERFORMANCE_ENGINEERING.md) - Performance implications of patterns

---

## Practice Exercises

1. **Beginner**: Implement a Singleton logger with different log levels
2. **Intermediate**: Create a Strategy-based validation system for a registration form
3. **Advanced**: Build an Observer-based reactive data grid with computed columns