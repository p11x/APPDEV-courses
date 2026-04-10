# 🔬 Microtasks and Macrotasks

## 📋 Overview

JavaScript has two types of async tasks: microtasks and macrotasks. Understanding the difference is crucial for predicting code execution order.

---

## 🔍 Microtasks vs Macrotasks

### Microtasks (High Priority)

- Promise callbacks
- queueMicrotask()
- MutationObserver
- Process next tick (Node.js)

### Macrotasks (Lower Priority)

- setTimeout/setInterval
- I/O operations
- UI rendering

### Execution Order

```javascript
console.log('1. Sync');

setTimeout(() => console.log('2. Macrotask'), 0);

queueMicrotask(() => console.log('3. Microtask'));

Promise.resolve().then(() => console.log('4. Promise'));

console.log('5. Sync end');

// Order: 1, 5, 3, 4, 2
```

---

## 🔗 Related Topics

- [10_Event_Loop_Deep_Dive.md](./10_Event_Loop_Deep_Dive.md)
- [12_Async_Context_Management.md](./12_Async_Context_Management.md)

---

**Next: Learn about [Async Context Management](./12_Async_Context_Management.md)**