# 📚 JavaScript Patterns Library

## Design Patterns Implementation

---

## Table of Contents

1. [Creational Patterns](#creational-patterns)
2. [Structural Patterns](#structural-patterns)
3. [Behavioral Patterns](#behavioral-patterns)

---

## Creational Patterns

### Singleton

```javascript
class Singleton {
  static #instance = null;
  
  constructor() {
    if (Singleton.#instance) {
      return Singleton.#instance;
    }
    Singleton.#instance = this;
  }
  
  static getInstance() {
    if (!Singleton.#instance) {
      Singleton.#instance = new Singleton();
    }
    return Singleton.#instance;
  }
}
```

### Factory

```javascript
class ProductFactory {
  create(type) {
    switch (type) {
      case 'electronic':
        return new ElectronicProduct();
      case 'clothing':
        return new ClothingProduct();
      default:
        return new GenericProduct();
    }
  }
}
```

---

## Structural Patterns

### Adapter

```javascript
class OldAPI {
  getData() {
    return { data: 'old format' };
  }
}

class Adapter {
  constructor() {
    this.api = new OldAPI();
  }
  
  get() {
    const old = this.api.getData();
    return { ...old, formatted: true };
  }
}
```

### Decorator

```javascript
function logging(target, name, descriptor) {
  const original = descriptor.value;
  
  descriptor.value = function(...args) {
    console.log(`Calling ${name}`, args);
    const result = original.apply(this, args);
    console.log(`Result`, result);
    return result;
  };
  
  return descriptor;
}
```

---

## Behavioral Patterns

### Observer

```javascript
class Observer {
  constructor() {
    this.observers = [];
  }
  
  subscribe(fn) {
    this.observers.push(fn);
  }
  
  unsubscribe(fn) {
    this.observers = this.observers.filter(o => o !== fn);
  }
  
  notify(data) {
    this.observers.forEach(fn => fn(data));
  }
}
```

### Strategy

```javascript
class PaymentStrategy {
  constructor(strategy) {
    this.strategy = strategy;
  }
  
  pay(amount) {
    return this.strategy.pay(amount);
  }
}

class CreditCardPayment {
  pay(amount) {
    return `Paid ${amount} with credit card`;
  }
}

class PayPalPayment {
  pay(amount) {
    return `Paid ${amount} with PayPal`;
  }
}
```

---

## Summary

### Key Takeaways

1. **Creational**: Object creation
2. **Structural**: Object composition
3. **Behavioral**: Object communication

### Next Steps

- Continue with: [08_JAVASCRIPT_PERFORMANCE_OPTIMIZATION.md](08_JAVASCRIPT_PERFORMANCE_OPTIMIZATION.md)
- Study patterns in frameworks
- Implement own patterns

---

## Cross-References

- **Previous**: [../14_JAVA_SCRIPT_PROJECTS/54_PROJECT_23_MARKETPLACE.md](../14_JAVA_SCRIPT_PROJECTS/54_PROJECT_23_MARKETPLACE.md)
- **Next**: [08_JAVASCRIPT_PERFORMANCE_OPTIMIZATION.md](08_JAVASCRIPT_PERFORMANCE_OPTIMIZATION.md)

---

*Last updated: 2024*