# 💾 Memory Management in JavaScript

## 📋 Overview

JavaScript has automatic memory management (garbage collection), but understanding how it works helps write more efficient code and avoid memory leaks.

---

## 🎯 How Memory Works

### Memory Lifecycle

```
┌─────────────────────────────────────┐
│  1. ALLOCATION                     │
│     - Variables created            │
│     - Objects allocated            │
├─────────────────────────────────────┤
│  2. USAGE                          │
│     - Read/write variables         │
│     - Execute functions            │
├─────────────────────────────────────┤
│  3. RELEASE                        │
│     - Garbage collection           │
│     - Reclaim unused memory        │
└─────────────────────────────────────┘
```

### Memory Heap

```javascript
// Memory allocation
const obj = { name: 'John' };    // Heap: object
const str = 'Hello';            // Heap: string
const arr = [1, 2, 3];          // Heap: array
```

---

## 🎯 Garbage Collection

### Reference Counting (Legacy)

```javascript
// Problem: Circular references
const obj1 = { ref: obj2 };
const obj2 = { ref: obj1 };

// Neither can be collected - memory leak!
```

### Mark and Sweep (Modern)

```javascript
// GC marks all reachable objects
// Everything not marked is garbage

let user = { name: 'John' }; // reachable
user = null; // not reachable - will be collected
```

---

## 🎯 Memory Leaks

### Common Causes

```javascript
// 1. Global variables
function leak() {
    largeData = new Array(1000000); // Creates global!
}

// 2. Forgotten timers
setInterval(() => {
    // Never cleared - keeps running forever
}, 1000);

// 3. Closures holding references
function createCallback() {
    let largeData = new Array(1000000);
    return function() {
        console.log(largeData.length); // Keeps largeData in memory
    };
}

// 4. DOM references
const elements = [];
document.querySelectorAll('.item').forEach(el => {
    elements.push(el); // Keeps DOM elements in memory
});
```

### Detecting Leaks

```javascript
// Check memory usage
console.log(performance.memory);

// Monitor over time
function monitorMemory() {
    const used = performance.memory.usedJSHeapSize / 1024 / 1024;
    console.log(`Memory: ${used.toFixed(2)} MB`);
    
    setTimeout(monitorMemory, 5000);
}
```

---

## 🎯 Optimization Techniques

### Object Pooling

```javascript
// Reuse objects instead of creating new ones
class ObjectPool {
    constructor(factory, size = 10) {
        this.pool = [];
        this.factory = factory;
        
        for (let i = 0; i < size; i++) {
            this.pool.push(factory());
        }
    }
    
    acquire() {
        return this.pool.pop() || this.factory();
    }
    
    release(obj) {
        if (this.pool.length < 10) {
            this.pool.push(obj);
        }
    }
}

// Usage
const pool = new ObjectPool(() => ({ x: 0, y: 0 }));
const point = pool.acquire();
point.x = 10;
point.y = 20;
// Use point...
pool.release(point);
```

### Weak References

```javascript
// WeakMap - doesn't prevent garbage collection
const cache = new WeakMap();

function getData(key) {
    if (cache.has(key)) {
        return cache.get(key);
    }
    
    const data = expensiveOperation(key);
    cache.set(key, data);
    return data;
}
```

---

## 🔗 Related Topics

- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)

---

**Next: [Bundle Optimization](./23_Bundle_Optimization.md)**