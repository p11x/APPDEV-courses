# SOLID Principles in JavaScript

> Master the five fundamental principles of object-oriented design for maintainable JavaScript applications

## Table of Contents

1. [Introduction to SOLID](#introduction)
2. [Single Responsibility Principle (SRP)](#srp)
3. [Open/Closed Principle (OCP)](#ocp)
4. [Liskov Substitution Principle (LSP)](#lsp)
5. [Interface Segregation Principle (ISP)](#isp)
6. [Dependency Inversion Principle (DIP)](#dip)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## 1. Introduction to SOLID

[anchor](#1-introduction-to-solid)

SOLID is an acronym representing five fundamental principles of object-oriented design that, when applied together, make code more maintainable, flexible, and scalable. Originally formulated by Robert C. Martin (Uncle Bob), these principles have been adapted for JavaScript and TypeScript development.

### The Five Principles

| Principle | Description |
|-----------|-------------|
| **S**ingle Responsibility | A class should have only one reason to change |
| **O**pen/Closed | Software entities should be open for extension but closed for modification |
| **L**iskov Substitution | Objects should be replaceable with subtypes without breaking the application |
| **I**nterface Segregation | Prefer many small, specific interfaces over one large one |
| **D**ependency Inversion | Depend on abstractions, not on concrete implementations |

### Why SOLID Matters in JavaScript

JavaScript's dynamic nature and prototypal inheritance make SOLID principles particularly important:

- **TypeScript Integration**: SOLID code translates better to TypeScript
- **Framework Compatibility**: React, Angular, and Vue work better with SOLID code
- **Testing**: SOLID principles enable easier unit testing
- **Scaling**: Large applications require maintainable code

---

## 2. Single Responsibility Principle (SRP)

[anchor](#2-single-responsibility-principle-srp)

A class should have only one reason to change. This means each class should have a single, well-defined responsibility.

### Anti-Pattern: God Class

```javascript
// file: anti-patterns/GodClass.js
// ❌ VIOLATION: This class has too many responsibilities

class UserManager {
  constructor() {
    this.users = [];
  }

  // Responsibility 1: Data management
  createUser(data) {
    this.users.push(data);
    this.saveToDatabase(data);
    this.sendWelcomeEmail(data);
    this.logActivity(data, 'created');
  }

  // Responsibility 2: Database operations
  saveToDatabase(user) {
    console.log(`Saving ${user.name} to database`);
  }

  // Responsibility 3: Email notifications
  sendWelcomeEmail(user) {
    console.log(`Sending welcome email to ${user.email}`);
  }

  // Responsibility 4: Logging
  logActivity(user, action) {
    console.log(`[LOG] User ${user.name}: ${action}`);
  }

  // Responsibility 5: Validation
  validateUser(data) {
    if (!data.email.includes('@')) return false;
    if (data.name.length < 2) return false;
    return true;
  }
}
```

### Refactored: SRP Compliant

```javascript
// file: srp/UserValidator.js
// ✅ SRP: Single responsibility - validation

export class UserValidator {
  validate(userData) {
    const errors = [];

    if (!userData.name || userData.name.length < 2) {
      errors.push('Name must be at least 2 characters');
    }

    if (!userData.email || !this.#isValidEmail(userData.email)) {
      errors.push('Invalid email format');
    }

    if (!userData.password || userData.password.length < 8) {
      errors.push('Password must be at least 8 characters');
    }

    return { valid: errors.length === 0, errors };
  }

  #isValidEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }
}
```

```javascript
// file: srp/UserRepository.js
// ✅ SRP: Single responsibility - data persistence

export class UserRepository {
  #db;

  constructor(database) {
    this.#db = database;
  }

  async save(user) {
    return this.#db.insert('users', user);
  }

  async findById(id) {
    return this.#db.findOne('users', { id });
  }

  async findAll() {
    return this.#db.find('users');
  }

  async update(id, data) {
    return this.#db.update('users', { id }, data);
  }

  async delete(id) {
    return this.#db.delete('users', { id });
  }
}
```

```javascript
// file: srp/UserNotifier.js
// ✅ SRP: Single responsibility - notifications

export class UserNotifier {
  #emailService;
  #templates = {
    welcome: 'Welcome to our platform!',
    passwordReset: 'Click here to reset your password'
  };

  constructor(emailService) {
    this.#emailService = emailService;
  }

  async sendWelcome(user) {
    return this.#emailService.send({
      to: user.email,
      subject: 'Welcome!',
      body: this.#templates.welcome
    });
  }

  async sendPasswordReset(user, token) {
    return this.#emailService.send({
      to: user.email,
      subject: 'Password Reset',
      body: `${this.#templates.passwordReset}?token=${token}`
    });
  }
}
```

```javascript
// file: srp/ActivityLogger.js
// ✅ SRP: Single responsibility - logging

export class ActivityLogger {
  #logger;

  constructor(logger) {
    this.#logger = logger;
  }

  log(userId, action, metadata = {}) {
    this.#logger.info({
      type: 'user_activity',
      userId,
      action,
      timestamp: new Date().toISOString(),
      ...metadata
    });
  }

  logError(userId, error) {
    this.#logger.error({
      type: 'user_error',
      userId,
      error: error.message,
      stack: error.stack,
      timestamp: new Date().toISOString()
    });
  }
}
```

```javascript
// file: srp/UserService.js
// ✅ Coordinator class that orchestrates other SRP classes

import { UserValidator } from './UserValidator.js';
import { UserRepository } from './UserRepository.js';
import { UserNotifier } from './UserNotifier.js';
import { ActivityLogger } from './ActivityLogger.js';

export class UserService {
  #validator;
  #repository;
  #notifier;
  #logger;

  constructor(deps) {
    this.#validator = new UserValidator();
    this.#repository = deps.repository;
    this.#notifier = deps.notifier;
    this.#logger = deps.logger;
  }

  async createUser(userData) {
    const validation = this.#validator.validate(userData);
    if (!validation.valid) {
      throw new Error(validation.errors.join(', '));
    }

    const user = await this.#repository.save(userData);
    await this.#notifier.sendWelcome(user);
    this.#logger.log(user.id, 'user_created');

    return user;
  }
}
```

### Professional Use Case: Order Processing

```javascript
// file: srp/OrderProcessor.js
// SRP applied to order processing pipeline

export class OrderValidator {
  validate(order) {
    const errors = [];

    if (!order.items || order.items.length === 0) {
      errors.push('Order must have at least one item');
    }

    if (order.items.some(item => item.quantity <= 0)) {
      errors.push('Item quantity must be positive');
    }

    return { valid: errors.length === 0, errors };
  }
}

export class InventoryService {
  async reserve(items) {
    for (const item of items) {
      const available = await this.#checkInventory(item.productId);
      if (available < item.quantity) {
        throw new Error(`Insufficient inventory for ${item.productId}`);
      }
    }
    return true;
  }

  async #checkInventory(productId) {
    return 100;
  }
}

export class PaymentProcessor {
  #paymentGateway;

  constructor(paymentGateway) {
    this.#paymentGateway = paymentGateway;
  }

  async process(payment) {
    return this.#paymentGateway.charge(payment.amount, payment.method);
  }
}

export class ShippingService {
  async schedule(items, address) {
    return {
      trackingNumber: 'TRACK' + Date.now(),
      estimatedDelivery: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000)
    };
  }
}

export class OrderService {
  constructor(deps) {
    this.#validators = deps.validators;
    this.#inventory = deps.inventory;
    this.#payment = deps.payment;
    this.#shipping = deps.shipping;
  }

  #validators;
  #inventory;
  #payment;
  #shipping;

  async processOrder(order, payment, shippingAddress) {
    this.#validators.validate(order);
    await this.#inventory.reserve(order.items);
    await this.#payment.process(payment);
    return this.#shipping.schedule(order.items, shippingAddress);
  }
}
```

---

## 3. Open/Closed Principle (OCP)

[anchor](#3-openclosed-principle-ocp)

Software entities should be open for extension but closed for modification.

### Anti-Pattern

```javascript
// file: anti-patterns/SwitchStatement.js
// ❌ VIOLATION: Adding new payment types requires modifying this class

class PaymentProcessor {
  processPayment(amount, paymentType) {
    switch (paymentType) {
      case 'credit_card':
        return this.#processCreditCard(amount);
      case 'paypal':
        return this.#processPayPal(amount);
      case 'crypto':
        return this.#processCrypto(amount);
      default:
        throw new Error(`Unknown payment type: ${paymentType}`);
    }
  }

  #processCreditCard(amount) {
    console.log(`Processing credit card: $${amount}`);
    return { success: true };
  }

  #processPayPal(amount) {
    console.log(`Processing PayPal: $${amount}`);
    return { success: true };
  }

  #processCrypto(amount) {
    console.log(`Processing crypto: $${amount}`);
    return { success: true };
  }
}
```

### Refactored: OCP Compliant

```javascript
// file: ocp/PaymentStrategy.js
// ✅ OCP: Add new payment methods without modifying existing code

class PaymentMethod {
  process(amount) {
    throw new Error('process() must be implemented');
  }

  validate() {
    return true;
  }
}

class CreditCardPayment extends PaymentMethod {
  #card;

  constructor(card) {
    super();
    this.#card = card;
  }

  process(amount) {
    console.log(`Processing credit card: $${amount}`);
    return { success: true, method: 'credit_card' };
  }
}

class PayPalPayment extends PaymentMethod {
  #email;

  constructor(email) {
    super();
    this.#email = email;
  }

  process(amount) {
    console.log(`Processing PayPal: $${amount}`);
    return { success: true, method: 'paypal' };
  }
}

class CryptoPayment extends PaymentMethod {
  #wallet;

  constructor(wallet) {
    super();
    this.#wallet = wallet;
  }

  process(amount) {
    console.log(`Processing crypto: $${amount}`);
    return { success: true, method: 'crypto' };
  }
}

class ApplePayPayment extends PaymentMethod {
  #deviceId;

  constructor(deviceId) {
    super();
    this.#deviceId = deviceId;
  }

  process(amount) {
    console.log(`Processing Apple Pay: $${amount}`);
    return { success: true, method: 'apple_pay' };
  }
}

class PaymentProcessor {
  #methods = new Map();

  register(type, PaymentClass) {
    this.#methods.set(type, PaymentClass);
  }

  process(amount, type, ...args) {
    const PaymentClass = this.#methods.get(type);
    if (!PaymentClass) {
      throw new Error(`Unknown payment type: ${type}`);
    }

    const method = new PaymentClass(...args);
    return method.process(amount);
  }
}

const processor = new PaymentProcessor();
processor.register('credit_card', CreditCardPayment);
processor.register('paypal', PayPalPayment);
processor.register('crypto', CryptoPayment);
processor.register('apple_pay', ApplePayPayment);

console.log(processor.process(100, 'credit_card', '4111111111111111'));
console.log(processor.process(50, 'paypal', 'user@example.com'));
```

### Professional Use Case: Discount System

```javascript
// file: ocp/DiscountCalculator.js
// OCP applied to a flexible discount system

class DiscountRule {
  apply(cart, context) {
    throw new Error('apply() must be implemented');
  }

  getPriority() {
    return 0;
  }
}

class PercentageDiscount extends DiscountRule {
  #percentage;
  #minAmount;

  constructor(percentage, minAmount = 0) {
    super();
    this.#percentage = percentage;
    this.#minAmount = minAmount;
  }

  apply(cart) {
    if (cart.subtotal < this.#minAmount) return 0;
    return cart.subtotal * (this.#percentage / 100);
  }

  getPriority() {
    return 10;
  }
}

class FixedDiscount extends DiscountRule {
  #amount;
  #minAmount;

  constructor(amount, minAmount = 0) {
    super();
    this.#amount = amount;
    this.#minAmount = minAmount;
  }

  apply(cart) {
    if (cart.subtotal < this.#minAmount) return 0;
    return this.#amount;
  }

  getPriority() {
    return 20;
  }
}

class BuyXGetYDiscount extends DiscountRule {
  #buyQuantity;
  #getQuantity;
  #productId;

  constructor(productId, buyQuantity, getQuantity) {
    super();
    this.#productId = productId;
    this.#buyQuantity = buyQuantity;
    this.#getQuantity = getQuantity;
  }

  apply(cart) {
    const item = cart.items.find(i => i.productId === this.#productId);
    if (!item || item.quantity < this.#buyQuantity) return 0;

    const freeItems = Math.floor(item.quantity / this.#buyQuantity) * this.#getQuantity;
    return freeItems * item.price;
  }

  getPriority() {
    return 30;
  }
}

class LoyaltyDiscount extends DiscountRule {
  #tierDiscounts = {
    bronze: 5,
    silver: 10,
    gold: 15,
    platinum: 20
  };

  constructor(tiers) {
    super();
    this.#tierDiscounts = { ...this.#tierDiscounts, ...tiers };
  }

  apply(cart, context) {
    const tier = context?.customerTier || 'bronze';
    const percentage = this.#tierDiscounts[tier] || 0;
    return cart.subtotal * (percentage / 100);
  }

  getPriority() {
    return 5;
  }
}

class DiscountCalculator {
  #rules = [];

  addRule(rule) {
    this.#rules.push(rule);
    this.#rules.sort((a, b) => b.getPriority() - a.getPriority());
  }

  calculate(cart, context = {}) {
    let totalDiscount = 0;

    for (const rule of this.#rules) {
      totalDiscount += rule.apply(cart, context);
    }

    return {
      subtotal: cart.subtotal,
      discount: totalDiscount,
      total: cart.subtotal - totalDiscount
    };
  }
}

const calculator = new DiscountCalculator();
calculator.addRule(new LoyaltyDiscount());
calculator.addRule(new PercentageDiscount(10, 100));
calculator.addRule(new FixedDiscount(50, 500));

const cart = {
  subtotal: 200,
  items: [{ productId: 'prod_1', quantity: 3, price: 50 }]
};

const context = { customerTier: 'gold' };
const result = calculator.calculate(cart, context);
console.log(result);
```

---

## 4. Liskov Substitution Principle (LSP)

[anchor](#4-liskov-substitution-principle-lsp)

Objects of a superclass should be replaceable with objects of a subclass without affecting correctness.

### Anti-Pattern: Violating LSP

```javascript
// file: anti-patterns/LiskovViolation.js
// ❌ VIOLATION: Subclass changes expected behavior

class Bird {
  fly() {
    return 'Flying high!';
  }

  eat() {
    return 'Eating seeds';
  }
}

class Penguin extends Bird {
  fly() {
    return null;
  }
}

function makeBirdFly(bird) {
  const flight = bird.fly();
  if (flight === null) {
    return 'Cannot fly';
  }
  return flight;
}

const penguin = new Penguin();
console.log(makeBirdFly(penguin));
```

### Refactored: LSP Compliant

```javascript
// file: lsp/BirdHierarchy.js
// ✅ LSP: Proper hierarchy with correct abstractions

class Bird {
  #species;

  constructor(species) {
    this.#species = species;
  }

  move() {
    throw new Error('move() must be implemented');
  }

  eat() {
    return 'eating';
  }

  getSpecies() {
    return this.#species;
  }
}

class FlyingBird extends Bird {
  move() {
    return 'flying';
  }

  fly() {
    return `${this.getSpecies()} is flying`;
  }
}

class WalkingBird extends Bird {
  move() {
    return 'walking';
  }

  walk() {
    return `${this.getSpecies()} is walking`;
  }
}

class Eagle extends FlyingBird {
  constructor() {
    super('Eagle');
  }
}

class Penguin extends WalkingBird {
  constructor() {
    super('Penguin');
  }

  swim() {
    return 'swimming';
  }
}

function demonstrateMovement(bird) {
  console.log(`${bird.getSpecies()}: ${bird.move()}`);
}

const eagle = new Eagle();
const penguin = new Penguin();

demonstrateMovement(eagle);
demonstrateMovement(penguin);
```

### Professional Use Case: Stream Processing

```javascript
// file: lsp/StreamPipeline.js
// LSP-compliant stream processing

class DataStream {
  async read() {
    throw new Error('read() must be implemented');
  }

  [Symbol.asyncIterator]() {
    throw new Error('[Symbol.asyncIterator] must be implemented');
  }
}

class ArrayStream extends DataStream {
  #data;
  #index = 0;

  constructor(data) {
    super();
    this.#data = data;
  }

  async read() {
    return this.#data[this.#index++] || null;
  }

  [Symbol.asyncIterator]() {
    return {
      next: async () => {
        if (this.#index >= this.#data.length) {
          return { done: true };
        }
        return { done: false, value: this.#data[this.#index++] };
      }
    };
  }
}

class FileStream extends DataStream {
  #content;
  #position = 0;

  constructor(content) {
    super();
    this.#content = content;
  }

  [Symbol.asyncIterator]() {
    return {
      next: async () => {
        const lines = this.#content.split('\n');
        if (this.#position >= lines.length) {
          return { done: true };
        }
        return { done: false, value: lines[this.#position++] };
      }
    };
  }
}

async function processStream(stream) {
  const results = [];
  
  for await (const chunk of stream) {
    results.push(chunk.toUpperCase());
  }
  
  return results;
}

const arrayStream = new ArrayStream(['a', 'b', 'c']);
console.log(await processStream(arrayStream));
```

---

## 5. Interface Segregation Principle (ISP)

[anchor](#5-interface-segregation-principle-isp)

Prefer many small, specific interfaces over one large, general interface.

### Anti-Pattern: Fat Interface

```javascript
// file: anti-patterns/FatInterface.js
// ❌ VIOLATION: Forcing classes to implement unused methods

class Graphic {
  draw() { throw new Error('Not implemented'); }
  rotate() { throw new Error('Not implemented'); }
  scale() { throw new Error('Not implemented'); }
  playAudio() { throw new Error('Not implemented'); }
  recordVideo() { throw new Error('Not implemented'); }
  compress() { throw new Error('Not implemented'); }
}

class Circle extends Graphic {
  draw() { /* draw circle */ }
  rotate() { /* don't need */ }
  scale() { /* don't need */ }
  playAudio() { /* don't need */ }
  recordVideo() { /* don't need */ }
  compress() { /* don't need */ }
}
```

### Refactored: ISP Compliant

```javascript
// file: isp/SmallInterfaces.js
// ✅ ISP: Small, specific interfaces

class Drawable {
  draw() {
    throw new Error('draw() must be implemented');
  }
}

class Rotatable {
  rotate(angle) {
    throw new Error('rotate() must be implemented');
  }
}

class Scalable {
  scale(factor) {
    throw new Error('scale() must be implemented');
  }
}

class Compressible {
  compress(algorithm) {
    throw new Error('compress() must be implemented');
  }
}

class Circle implements Drawable {
  draw() {
    console.log('Drawing circle');
  }
}

class Image implements Drawable, Scalable, Compressible {
  draw() {
    console.log('Drawing image');
  }

  scale(factor) {
    console.log(`Scaling image by ${factor}`);
  }

  compress(algorithm) {
    console.log(`Compressing with ${algorithm}`);
  }
}

class Button implements Drawable, Rotatable {
  draw() {
    console.log('Drawing button');
  }

  rotate(angle) {
    console.log(`Rotating button by ${angle} degrees`);
  }
}
```

### Professional Use Case: Plugin System

```javascript
// file: isp/PluginInterfaces.js
// Interface segregation for a plugin system

class IPlugin {
  getName() { throw new Error('Not implemented'); }
  getVersion() { throw new Error('Not implemented'); }
}

class IRenderable {
  render(ctx) { throw new Error('Not implemented'); }
}

class IClickable {
  onClick(handler) { throw new Error('Not implemented'); }
}

class IDraggable {
  onDragStart(handler) { throw new Error('Not implemented'); }
  onDragEnd(handler) { throw new Error('Not implemented'); }
}

class IEditable {
  edit() { throw new Error('Not implemented'); }
}

class MenuItem implements IPlugin, IRenderable, IClickable {
  #name;
  #handler;

  constructor(name) {
    this.#name = name;
  }

  getName() { return this.#name; }
  getVersion() { return '1.0.0'; }
  render(ctx) { console.log(`Rendering menu: ${this.#name}`); }
  onClick(handler) { this.#handler = handler; }
}

class DraggablePanel implements IPlugin, IRenderable, IDraggable, IEditable {
  getName() { return 'DraggablePanel'; }
  getVersion() { return '1.0.0'; }
  render(ctx) { console.log('Rendering panel'); }
  onDragStart(handler) { console.log('Drag started'); }
  onDragEnd(handler) { console.log('Drag ended'); }
  edit() { console.log('Editing panel'); }
}
```

---

## 6. Dependency Inversion Principle (DIP)

[anchor](#6-dependency-inversion-principle-dip)

High-level modules should not depend on low-level modules. Both should depend on abstractions.

### Anti-Pattern: Tight Coupling

```javascript
// file: anti-patterns/TightCoupling.js
// ❌ VIOLATION: High-level class depends on low-level implementation

class UserServiceLowLevel {
  createUser(user) {
    const db = new MySQLDatabase();
    db.insert('users', user);
  }
}

class MySQLDatabase {
  insert(table, data) { /* MySQL specific logic */ }
}
```

### Refactored: DIP Compliant

```javascript
// file: dip/DatabaseAbstraction.js
// ✅ DIP: Depend on abstractions

class Database {
  async insert(table, data) {
    throw new Error('insert() must be implemented');
  }

  async find(table, query) {
    throw new Error('find() must be implemented');
  }

  async update(table, query, data) {
    throw new Error('update() must be implemented');
  }
}

class MySQLDatabase extends Database {
  async insert(table, data) {
    console.log(`MySQL: Inserting into ${table}`);
    return { id: 1 };
  }

  async find(table, query) {
    console.log(`MySQL: Finding in ${table}`);
    return [];
  }
}

class PostgreSQLDatabase extends Database {
  async insert(table, data) {
    console.log(`PostgreSQL: Inserting into ${table}`);
    return { id: 1 };
  }

  async find(table, query) {
    console.log(`PostgreSQL: Finding in ${table}`);
    return [];
  }
}

class MongoDatabase extends Database {
  async insert(table, data) {
    console.log(`MongoDB: Inserting into ${table}`);
    return { id: 1 };
  }

  async find(table, query) {
    console.log(`MongoDB: Finding in ${table}`);
    return [];
  }
}
```

```javascript
// file: dip/EmailService.js
// ✅ DIP: Email service abstraction

class EmailService {
  async send(email) {
    throw new Error('send() must be implemented');
  }
}

class SendGridEmail extends EmailService {
  async send(email) {
    console.log(`SendGrid: Sending to ${email.to}`);
    return { success: true };
  }
}

class SESEmail extends EmailService {
  async send(email) {
    console.log(`SES: Sending to ${email.to}`);
    return { success: true };
  }
}
```

```javascript
// file: dip/RefactoredUserService.js
// ✅ High-level module depends on abstractions

export class UserService {
  #database;
  #emailService;

  constructor(database, emailService) {
    this.#database = database;
    this.#emailService = emailService;
  }

  async createUser(userData) {
    const user = {
      ...userData,
      createdAt: new Date().toISOString()
    };

    await this.#database.insert('users', user);
    
    await this.#emailService.send({
      to: userData.email,
      subject: 'Welcome!',
      body: 'Welcome to our platform!'
    });

    return user;
  }

  async getUser(id) {
    return this.#database.find('users', { id });
  }
}

const mysqlDb = new MySQLDatabase();
const sendGrid = new SendGridEmail();

const userService = new UserService(mysqlDb, sendGrid);
```

### Professional Use Case: Storage Abstraction

```javascript
// file: dip/StorageService.js
// DIP applied to storage abstraction

class StorageBackend {
  async read(key) { throw new Error('Not implemented'); }
  async write(key, value) { throw new Error('Not implemented'); }
  async delete(key) { throw new Error('Not implemented'); }
}

class LocalStorageBackend extends StorageBackend {
  #storage;

  constructor() {
    super();
    this.#storage = new Map();
  }

  async read(key) { return this.#storage.get(key); }
  async write(key, value) { this.#storage.set(key, value); }
  async delete(key) { this.#storage.delete(key); }
}

class RedisStorageBackend extends StorageBackend {
  constructor() {
    super();
  }

  async read(key) { return null; }
  async write(key, value) { console.log(`Redis: write ${key}`); }
  async delete(key) { console.log(`Redis: delete ${key}`); }
}

class CacheService {
  #storage;
  #ttl;

  constructor(storage, ttl = 3600) {
    this.#storage = storage;
    this.#ttl = ttl;
  }

  async get(key) {
    const cached = await this.#storage.read(`cache:${key}`);
    if (cached) {
      return JSON.parse(cached);
    }
    return null;
  }

  async set(key, value) {
    await this.#storage.write(`cache:${key}`, JSON.stringify(value));
  }

  async invalidate(key) {
    await this.#storage.delete(`cache:${key}`);
  }
}

const localStorage = new LocalStorageBackend();
const cacheService = new CacheService(localStorage, 1800);
```

---

## Key Takeaways

- **SRP**: Each class should have one reason to change. Split large classes into focused collaborators.
- **OCP**: Use polymorphism and composition to add features without modifying existing code.
- **LSP**: Subclasses must honor their parent contracts. Use proper abstractions.
- **ISP**: Small, focused interfaces prevent unnecessary dependencies.
- **DIP**: Depend on abstractions, not concrete implementations.

### Performance Considerations

- **SRP**: More classes mean more objects; balance with composition
- **OCP**: Strategy pattern adds indirection; measure in hot paths
- **ISP**: Interface calls have overhead; profile before optimizing
- **DIP**: Extra abstraction layers add call depth; consider inline caching

### Security Considerations

- **Dependency Injection**: Prevents tight coupling that hides security issues
- **Abstractions**: Easy to add security wrappers at boundaries
- **SRP**: Easier to audit focused classes for vulnerabilities

---

## Common Pitfalls

1. **Over-engineering**: Don't apply SOLID to trivial code
2. **Analysis Paralysis**: Perfect is the enemy of good
3. **Dogmatic Application**: Context matters; adapt principles
4. **Forgetting YAGNI**: You might not need that abstraction
5. **Ignoring Team Skills**: Match practices to team capability

---

## Related Files

- [Design Patterns in JavaScript](./01_DESIGN_PATTERNS_JAVASCRIPT.md) - Implementation of SOLID through patterns
- [Code Organization and Structure](./04_CODE_ORGANIZATION_AND_STRUCTURE.md) - Project structure for SOLID code
- [JavaScript Performance Engineering](./05_JAVASCRIPT_PERFORMANCE_ENGINEERING.md) - Performance implications

---

## Practice Exercises

1. **Beginner**: Refactor a God Controller class in Express using SRP
2. **Intermediate**: Create a notification system supporting email, SMS, push using OCP
3. **Advanced**: Build a plugin system with granular interfaces demonstrating ISP