# 🔄 Event Loop Deep Dive

## 📋 Overview

The event loop is the mechanism that allows JavaScript to perform non-blocking operations despite being single-threaded. Understanding it is crucial for writing efficient async code.

---

## 🎯 How the Event Loop Works

### Execution Model

```
┌─────────────────────────────────────────────────────────────┐
│                      EVENT LOOP                             │
│                                                             │
│   ┌─────────────┐     ┌─────────────┐     ┌─────────────┐  │
│   │   CALL      │◄────│   TASK      │◄────│    MACRO    │  │
│   │   STACK     │     │   QUEUE     │     │   TASK QUEUE│  │
│   │   (LIFO)    │     │   (FIFO)    │     │             │  │
│   └─────────────┘     └─────────────┘     └─────────────┘  │
│         │                   ▲                   │            │
│         │                   │                   │            │
│         └───────────────────┴───────────────────┘            │
│                        EVENT LOOP                           │
└─────────────────────────────────────────────────────────────┘

   ┌─────────────────────────────────────────────────────────┐
   │                  MICROTASK QUEUE                        │
   │  (Promises, MutationObserver, QueueMicrotask)          │
   └─────────────────────────────────────────────────────────┘
```

### Step-by-Step

```javascript
console.log('1. Start');          // Runs immediately

setTimeout(() => {
    console.log('2. Timeout');    // Runs after 0ms
}, 0);

Promise.resolve().then(() => {
    console.log('3. Promise');    // Runs after sync code
});

console.log('4. End');           // Runs immediately

// Output: 1, 4, 3, 2
```

---

## 🧪 Execution Order Demo

```javascript
// What order will this output?
console.log('1. Script start');

setTimeout(() => console.log('2. Timeout'), 0);

Promise.resolve().then(() => console.log('3. Promise 1'));

Promise.resolve().then(() => 
    setTimeout(() => console.log('4. Timeout in promise'), 0)
);

Promise.resolve().then(() => console.log('5. Promise 2'));

setTimeout(() => console.log('6. Timeout 2'), 0);

console.log('7. Script end');

// Output order:
// 1. Script start
// 7. Script end
// 3. Promise 1
// 5. Promise 2
// 2. Timeout
// 6. Timeout 2
// 4. Timeout in promise
```

---

## 🎯 Key Concepts

### 1. Call Stack

```javascript
function a() { console.log('a'); }
function b() { a(); }
function c() { b(); }

c(); // Executes: a, b, c (in order, then pops off)
```

### 2. Task Queue (Macrotasks)

```javascript
// These go to Task Queue (slower)
setTimeout(() => {}, 0);
setInterval(() => {}, 0);
I/O operations
UI rendering
```

### 3. Microtask Queue

```javascript
// These go to Microtask Queue (faster)
Promise.then()
Promise.catch()
Promise.finally()
queueMicrotask()
MutationObserver
```

---

## 🔗 Related Topics

- [09_Throttling_and_Performance.md](./09_Throttling_and_Performance.md)
- [11_Microtasks_and_Macrotasks.md](./11_Microtasks_and_Macrotasks.md)

---

**Next: Learn about [Microtasks and Macrotasks](./11_Microtasks_and_Macrotasks.md)**