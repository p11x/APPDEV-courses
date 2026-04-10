# ⚙️ How JavaScript Works

## 🔍 Understanding the JavaScript Engine

When you write JavaScript code, it doesn't run directly on your computer's processor. Instead, it runs inside a **JavaScript Engine** that interprets and executes your code. Different browsers have different engines:

| Browser | Engine | Used In |
|---------|--------|---------|
| Chrome/Edge | V8 | Chrome, Edge, Node.js |
| Firefox | SpiderMonkey | Firefox |
| Safari | JavaScriptCore | Safari |
| Node.js | V8 | Server-side JavaScript |

---

## 🏗️ JavaScript Engine Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        JAVASCRIPT ENGINE                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   ┌─────────────┐     ┌─────────────┐     ┌─────────────┐    │
│   │   PARSER    │────►│  COMPILER   │────►│   EXECUTOR   │    │
│   │             │     │             │     │             │    │
│   │ • Converts  │     │ • JIT       │     │ • Executes  │    │
│   │   code to   │     │   Compiles  │     │   bytecode  │    │
│   │   AST       │     │ • Optimizes │     │ • Handles   │    │
│   └─────────────┘     └─────────────┘     └─────────────┘    │
│         │                   │                   │              │
│         ▼                   ▼                   ▼              │
│   ┌─────────────────────────────────────────────────────┐     │
│   │                    MEMORY HEAP                       │     │
│   │  • Stores variables, objects, functions              │     │
│   │  • Manages memory allocation                         │     │
│   └─────────────────────────────────────────────────────┘     │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Key Components:

1. **Parser** - Converts source code into Abstract Syntax Tree (AST)
2. **Compiler** - JIT (Just-In-Time) compilation, converts to bytecode
3. **Executor** - Runs the compiled bytecode
4. **Memory Heap** - Stores objects and variables

---

## 🎯 Execution Context

### What is Execution Context?

An **Execution Context** is an environment where JavaScript code runs. It contains:
- Variable environment
- Scope chain
- `this` binding

### Types of Execution Context:

```
┌────────────────────────────────────────────┐
│         GLOBAL EXECUTION CONTEXT           │
│  (Created when script first runs)          │
│                                            │
│  • Global variables                        │
│  • Functions                               │
│  • this (global object)                    │
└────────────────┬───────────────────────────┘
                 │
    ┌────────────┴────────────┐
    │                         │
    ▼                         ▼
┌─────────────┐      ┌─────────────┐
│ FUNCTION    │      │ FUNCTION    │
│ CONTEXT 1   │      │ CONTEXT 2   │
│             │      │             │
│ • Local vars│      │ • Local vars│
│ • Parameters│      │ • Parameters│
│ • this      │      │ • this      │
└─────────────┘      └─────────────┘
```

### Creating an Execution Context:

```javascript
// Global Context
let globalVar = "I'm global";

function outer() {
    // Function Context for outer()
    let outerVar = "I'm outer";
    
    function inner() {
        // Function Context for inner()
        let innerVar = "I'm inner";
        
        console.log(globalVar); // ✅ Accessible
        console.log(outerVar); // ✅ Accessible
        console.log(innerVar); // ✅ Accessible
    }
    
    inner();
}

outer();
```

---

## 🔄 The Event Loop

The **Event Loop** is what makes JavaScript asynchronous. It constantly checks the **Call Stack** and **Task Queue**.

```
┌─────────────────────────────────────────────────────────────────┐
│                         EVENT LOOP                             │
│                                                                 │
│    ┌──────────────┐     ┌──────────────┐                     │
│    │   CALL STACK │◄────│   TASK QUEUE  │                     │
│    │              │     │              │                     │
│    │  (LIFO)      │     │  (FIFO)      │                     │
│    │              │     │              │                     │
│    │  [func3]     │     │  [cb1]       │                     │
│    │  [func2]     │     │  [cb2]       │                     │
│    │  [func1]     │     │  [cb3]       │                     │
│    │  [global]    │     │              │                     │
│    └──────────────┘     └──────────────┘                     │
│          │                      ▲                              │
│          │                      │                              │
│          └──────────────────────┘                              │
│                        EVENT LOOP                              │
└─────────────────────────────────────────────────────────────────┘
```

### Step-by-Step Execution:

```javascript
console.log("1: Start");

setTimeout(() => {
    console.log("3: Timeout callback");
}, 0);

console.log("2: End");

// Output:
// 1: Start
// 2: End
// 3: Timeout callback
```

### Why this happens:

1. `console.log("1: Start")` runs immediately
2. `setTimeout` is passed to Web API (browser)
3. `console.log("2: End")` runs immediately
4. Event loop waits for Web API to complete
5. Callback is moved to Task Queue
6. Event loop pushes callback to Call Stack when empty
7. Callback executes: `console.log("3: Timeout callback")`

---

## 🧠 Memory Management

### How JavaScript Manages Memory:

JavaScript uses **automatic memory management** with **Garbage Collection**.

### Memory Lifecycle:

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  ALLOCATION │────►│    USAGE    │────►│  RELEASE    │
│             │     │             │     │             │
│ • Variables │     │ • Read/Write│     │ • GC runs   │
│ • Objects   │     │ • Functions │     │ • Mark-Sweep│
│ • Functions │     │ • Logic     │     │ • Reference │
└─────────────┘     └─────────────┘     └─────────────┘
```

### Garbage Collection Algorithms:

#### 1. Reference Counting (Old)
```javascript
// Problem: Circular references
let obj1 = { ref: obj2 };
let obj2 = { ref: obj1 };

// Neither can be cleaned - memory leak!
```

#### 2. Mark and Sweep (Modern)
```javascript
// GC marks all reachable objects
// Everything not marked is garbage

let user = { name: "John" }; // Reachable ✓
user = null; // Not reachable, will be collected ✓
```

### Memory Leak Examples:

```javascript
// ❌ Accidental global variables
function leak() {
    leaked = "I'm global!"; // No var/let/const
}

// ❌ Forgotten timers
setInterval(() => {
    // This never stops, holding memory
}, 1000);

// ❌ Closures holding references
function createCallback() {
    let largeData = new Array(1000000);
    return function() {
        console.log(largeData.length); // Keeps largeData alive
    };
}
```

---

## 📊 Call Stack

The **Call Stack** is a data structure that tracks function calls in JavaScript. It uses LIFO (Last In, First Out) ordering.

### How the Call Stack Works:

```javascript
function first() {
    console.log("First function");
}

function second() {
    console.log("Before calling first");
    first();
    console.log("After calling first");
}

function third() {
    console.log("Start");
    second();
    console.log("End");
}

third();

/*
Call Stack Animation:

1: third()          → third() pushed
2: console.log      → console.log pushed/popped
3: second()         → second() pushed
4: console.log      → console.log pushed/popped
5: first()          → first() pushed
6: console.log      → console.log pushed/popped
7: first() done     → first() popped
8: second() done   → second() popped
9: third() done    → third() popped
*/
```

### Stack Overflow:

```javascript
// ❌ Infinite recursion - causes stack overflow
function recursive() {
    recursive(); // Keeps calling itself forever
}

recursive(); // RangeError: Maximum call stack size exceeded
```

```javascript
// ✅ Proper recursion with base case
function factorial(n) {
    if (n <= 1) return 1; // Base case
    return n * factorial(n - 1); // Recursive call
}

console.log(factorial(5)); // 120
```

---

## 🔀 Synchronous vs Asynchronous

### Synchronous Code:

```javascript
// Synchronous - runs line by line
console.log("Step 1");
console.log("Step 2");
console.log("Step 3");

// Output:
// Step 1
// Step 2
// Step 3
```

### Asynchronous Code:

```javascript
// Asynchronous - doesn't wait
console.log("Step 1");

setTimeout(() => {
    console.log("Step 2 (delayed)");
}, 1000);

console.log("Step 3");

// Output:
// Step 1
// Step 3
// Step 2 (delayed)
```

### Common Async Operations:

```javascript
// 1. Timers
setTimeout(() => console.log("Delayed"), 1000);
setInterval(() => console.log("Repeats"), 1000);

// 2. DOM Events
document.addEventListener('click', () => console.log("Clicked!"));

// 3. Network Requests
fetch('https://api.example.com/data')
    .then(response => response.json())
    .then(data => console.log(data));

// 4. File Operations (Node.js)
fs.readFile('file.txt', (err, data) => {
    if (err) throw err;
    console.log(data);
});
```

---

## 🎯 Key Takeaways

1. **JavaScript Engine** parses, compiles, and executes your code
2. **Execution Context** provides the environment for code to run
3. **Event Loop** manages asynchronous operations
4. **Memory Heap** stores data, **Call Stack** tracks function calls
5. **Garbage Collection** automatically frees unused memory

---

## 🔗 Related Topics

- [01_Introduction_to_JavaScript.md](./01_Introduction_to_JavaScript.md)
- [04_Syntax_and_Style.md](./04_Syntax_and_Style.md)
- [08_Async_Javascript_Basics.md](../08_ASYNC_JAVASCRIPT/08_Async_Javascript_Basics.md)

---

**Next: Learn about [Setting Up Development Environment](./03_Setting_Up_Development_Environment.md)**