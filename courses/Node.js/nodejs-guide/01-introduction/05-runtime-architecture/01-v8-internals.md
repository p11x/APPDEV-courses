# JavaScript Runtime Architecture

## What You'll Learn

- How the V8 engine works internally
- C++ integration and native module binding
- JavaScript-to-machine code compilation process
- Memory management and garbage collection strategies

## Architecture Overview

### The Three Pillars

```
┌─────────────────────────────────────────────────────────┐
│                    Your JavaScript Code                  │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   ┌─────────────┐   ┌─────────────┐   ┌─────────────┐  │
│   │    V8       │   │    libuv    │   │  C++ Bindings│  │
│   │   Engine    │   │  Event Loop │   │  & Native    │  │
│   │             │   │             │   │   Modules    │  │
│   │ - Parser    │   │ - I/O Ops   │   │             │  │
│   │ - Compiler  │   │ - Threading │   │ - fs        │  │
│   │ - Optimizer │   │ - Networking│   │ - crypto    │  │
│   │ - GC        │   │ - DNS       │   │ - http      │  │
│   └─────────────┘   └─────────────┘   └─────────────┘  │
│                                                         │
├─────────────────────────────────────────────────────────┤
│                   Operating System                       │
└─────────────────────────────────────────────────────────┘
```

## V8 Engine Internals

### What is V8?

V8 is Google's open-source JavaScript engine:
- Written in C++
- Used in Chrome, Node.js, Edge, Electron
- Compiles JavaScript directly to machine code
- Optimizes hot paths automatically

### V8 Compilation Pipeline

```
JavaScript Source Code
        │
        ▼
┌───────────────┐
│    Parser     │  Converts source to AST
└───────┬───────┘
        │
        ▼
┌───────────────┐
│   AST         │  Abstract Syntax Tree
└───────┬───────┘
        │
        ▼
┌───────────────┐
│  Interpreter  │  Generates bytecode (Ignition)
│  (Ignition)   │  Quick startup, baseline execution
└───────┬───────┘
        │
        ▼
┌───────────────┐
│   Profiler    │  Identifies hot functions
└───────┬───────┘
        │
        ▼
┌───────────────┐
│  Compiler     │  Optimizes hot code (TurboFan)
│  (TurboFan)   │  Generates optimized machine code
└───────┬───────┘
        │
        ▼
┌───────────────┐
│Machine Code   │  Direct CPU execution
└───────────────┘
```

### Ignition: The Interpreter

Ignition is V8's interpreter:
- Generates bytecode from AST
- Low memory footprint
- Fast startup time
- Baseline execution for all code

```javascript
// Example: How Ignition processes this
function add(a, b) {
    return a + b;
}

// Bytecode (simplified):
// Ldar a          ; Load accumulator with 'a'
// Add b           ; Add 'b' to accumulator
// Return          ; Return accumulator value
```

### TurboFan: The Optimizing Compiler

TurboFan optimizes frequently-executed code:
- Type speculation and optimization
- Inlining small functions
- Eliminating redundant checks
- Deoptimization when assumptions fail

```javascript
// V8 optimizes this pattern
function processArray(arr) {
    let sum = 0;
    for (let i = 0; i < arr.length; i++) {
        sum += arr[i];  // V8 assumes numbers after warmup
    }
    return sum;
}

// After optimization, V8 generates
// machine code that skips type checks
```

### Hidden Classes and Inline Caches

V8 uses hidden classes for fast property access:

```javascript
// Bad: Different initialization order
function Point(x, y) {
    this.x = x;
    this.y = y;
}

const p1 = new Point(1, 2);
const p2 = new Point(3, 4);
// Same hidden class - good!

// Worse: Different property order
function PointBad(x, y) {
    if (x > 0) {
        this.x = x;
        this.y = y;
    } else {
        this.y = y;
        this.x = x;
    }
}

const p3 = new PointBad(1, 2);
const p4 = new PointBad(-1, 3);
// Different hidden classes - bad for optimization
```

## libuv: The I/O Layer

### What is libuv?

libuv is a multi-platform C library:
- Provides asynchronous I/O
- Handles event loop
- Manages thread pool
- Cross-platform (Windows, macOS, Linux)

### libuv Architecture

```
┌─────────────────────────────────────────────────┐
│                  Node.js API                     │
│        (fs, http, net, dns, crypto)              │
└─────────────────────┬───────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────┐
│                    libuv                         │
│                                                 │
│  ┌─────────────┐  ┌─────────────────────────┐  │
│  │ Event Loop  │  │     Thread Pool         │  │
│  │             │  │                         │  │
│  │ - Timers    │  │  ┌───┐ ┌───┐ ┌───┐     │  │
│  │ - I/O Poll  │  │  │ T1│ │ T2│ │ T3│ ... │  │
│  │ - Callbacks │  │  └───┘ └───┘ └───┘     │  │
│  └─────────────┘  │  (Default: 4 threads)   │  │
│                   └─────────────────────────┘  │
└─────────────────────────────────────────────────┘
```

### Thread Pool Usage

Some operations use the thread pool automatically:

```javascript
const fs = require('fs');
const crypto = require('crypto');

// These use thread pool (not main thread)
fs.readFile('large-file.txt', (err, data) => {
    // Executed in thread pool
});

crypto.pbkdf2('password', 'salt', 100000, 64, 'sha512', (err, key) => {
    // CPU-intensive, runs in thread pool
});

// Control thread pool size
process.env.UV_THREADPOOL_SIZE = 8; // Default is 4
```

## C++ Bindings and Native Modules

### How JavaScript Calls C++

```
JavaScript Code
      │
      ▼
┌──────────────┐
│  JS Wrapper  │  (e.g., fs.readFile)
└──────┬───────┘
       │
       ▼
┌──────────────┐
│  C++ Binding │  (node_file.cc)
└──────┬───────┘
       │
       ▼
┌──────────────┐
│    libuv     │  (Actual I/O operation)
└──────┬───────┘
       │
       ▼
┌──────────────┐
│     OS       │  (System call)
└──────────────┘
```

### Native Addons

You can write C++ addons for Node.js:

```cpp
// hello.cc - Simple native addon
#include <node.h>

namespace demo {

using v8::FunctionCallbackInfo;
using v8::Isolate;
using v8::Local;
using v8::Object;
using v8::String;
using v8::Value;

void Hello(const FunctionCallbackInfo<Value>& args) {
    Isolate* isolate = args.GetIsolate();
    args.GetReturnValue().Set(
        String::NewFromUtf8(isolate, "world").ToLocalChecked()
    );
}

void Initialize(Local<Object> exports) {
    NODE_SET_METHOD(exports, "hello", Hello);
}

NODE_MODULE(NODE_GYP_MODULE_NAME, Initialize)

}  // namespace demo
```

```javascript
// Using the native addon
const addon = require('./build/Release/addon');
console.log(addon.hello()); // "world"
```

### N-API: Stable ABI

N-API provides stable C API:
- ABI-stable across Node.js versions
- Write once, run on multiple versions
- No recompilation needed

```javascript
// Using node-addon-api (C++ wrapper)
const nativeModule = require('./build/Release/native_module');
// Works on Node.js 12, 14, 16, 18, 20, 22+
```

## Memory Management

### V8 Memory Structure

```
V8 Memory Heap
┌─────────────────────────────────────────────────┐
│                                                 │
│  New Space (Semi-space)                        │
│  ┌─────────────────────────────────────────┐   │
│  │  Young Generation Objects               │   │
│  │  (Short-lived, frequent GC)             │   │
│  └─────────────────────────────────────────┘   │
│                                                 │
│  Old Space                                    │
│  ┌─────────────────────────────────────────┐   │
│  │  Old Generation Objects                 │   │
│  │  (Long-lived, less frequent GC)         │   │
│  └─────────────────────────────────────────┘   │
│                                                 │
│  Large Object Space                           │
│  ┌─────────────────────────────────────────┐   │
│  │  Objects larger than size threshold     │   │
│  └─────────────────────────────────────────┘   │
│                                                 │
│  Code Space                                   │
│  ┌─────────────────────────────────────────┐   │
│  │  JIT-compiled code                      │   │
│  └─────────────────────────────────────────┘   │
│                                                 │
│  Map Space                                    │
│  ┌─────────────────────────────────────────┐   │
│  │  Hidden classes (object shapes)         │   │
│  └─────────────────────────────────────────┘   │
│                                                 │
└─────────────────────────────────────────────────┘
```

### Garbage Collection

V8 uses generational garbage collection:

```javascript
// Young Generation (Scavenge)
// - Fast, frequent collection
// - Objects that survive move to Old Space

function createUser() {
    const user = {  // Allocated in New Space
        name: 'Alice',
        age: 30
    };
    return user;
}

// If user survives multiple GC cycles, moves to Old Space
```

### Mark-Sweep-Compact (Old Space)

```javascript
// Old Space uses Mark-Sweep-Compact
// Mark: Identify reachable objects
// Sweep: Free unreachable objects
// Compact: Reduce fragmentation

// Example of potential memory leak
const cache = new Map();

function processRequest(data) {
    const result = expensiveComputation(data);
    cache.set(data.id, result);  // Never removed!
    return result;
}

// Solution: Implement cache eviction
function processRequestSafe(data) {
    const result = expensiveComputation(data);
    cache.set(data.id, result);
    
    // Evict old entries
    if (cache.size > 1000) {
        const firstKey = cache.keys().next().value;
        cache.delete(firstKey);
    }
    
    return result;
}
```

### Memory Monitoring

```javascript
// Check memory usage
const used = process.memoryUsage();
console.log({
    rss: `${Math.round(used.rss / 1024 / 1024)} MB`,      // Resident Set Size
    heapTotal: `${Math.round(used.heapTotal / 1024 / 1024)} MB`, // Total heap
    heapUsed: `${Math.round(used.heapUsed / 1024 / 1024)} MB`,   // Used heap
    external: `${Math.round(used.external / 1024 / 1024)} MB`    // External memory
});

// Force garbage collection (with --expose-gc flag)
if (global.gc) {
    global.gc();
}
```

## JavaScript Compilation Details

### Type System in V8

V8 tracks types for optimization:

```javascript
// V8 tracks types in inline caches
function getProperty(obj) {
    return obj.value;  // V8 remembers obj's hidden class
}

// Consistent object shapes help V8
const obj1 = { value: 1, name: 'a' };
const obj2 = { value: 2, name: 'b' };
// Same hidden class - V8 optimizes

// Inconsistent shapes hurt performance
const obj3 = { name: 'c', value: 3 };
// Different hidden class - V8 must check
```

### Deoptimization

V8 may deoptimize when assumptions fail:

```javascript
// V8 optimizes for numbers
function add(a, b) {
    return a + b;
}

add(1, 2);      // V8: "always numbers!"
add(3, 4);      // V8: "confirmed numbers!"
// V8 generates optimized machine code

add("hello", "world");  // V8: "strings?! Deoptimize!"
// V8 falls back to interpreter, re-optimizes later
```

## Performance Implications

### Optimizing for V8

```javascript
// 1. Consistent object shapes
function createPoint(x, y) {
    return { x, y };  // Always same property order
}

// 2. Avoid polymorphic functions
function processValue(val) {
    // V8 optimizes better with monomorphic code
    return val * 2;
}

// 3. Use arrays efficiently
const arr = new Array(1000).fill(0);  // Pre-allocated
for (let i = 0; i < arr.length; i++) {
    arr[i] = i * 2;
}

// 4. Avoid hidden class changes
const obj = { a: 1 };
obj.b = 2;  // OK: adding property
delete obj.a;  // Bad: may create new hidden class
```

## Common Misconceptions

### Myth: JavaScript is always interpreted
**Reality**: V8 compiles JavaScript to machine code. The interpreter (Ignition) is just the first pass.

### Myth: Node.js is single-threaded
**Reality**: The main JavaScript execution is single-threaded, but libuv uses a thread pool for I/O operations.

### Myth: Garbage collection always pauses execution
**Reality**: V8 uses incremental and concurrent garbage collection to minimize pauses.

### Myth: Native addons are always faster
**Reality**: For simple operations, the overhead of crossing the JS-C++ boundary may exceed the benefit.

## Best Practices Checklist

- [ ] Understand V8's compilation pipeline
- [ ] Write V8-friendly code (consistent shapes)
- [ ] Monitor memory usage in production
- [ ] Use appropriate heap size limits
- [ ] Consider native addons for CPU-intensive tasks
- [ ] Profile before optimizing
- [ ] Keep up with V8 performance improvements

## Performance Optimization Tips

- Use monomorphic functions for better optimization
- Avoid creating objects in hot loops
- Use typed arrays for numeric data
- Profile with `--prof` flag
- Use `--max-old-space-size` for large heaps
- Consider worker threads for CPU-bound tasks

## Cross-References

- See [Event Loop Mechanics](./06-event-loop-mechanics.md) for async processing
- See [Performance Deep Dive](./09-performance-deep-dive.md) for optimization
- See [Runtime Comparison](./10-runtime-comparison.md) for alternative runtimes
- See [Real-world Cases](./11-real-world-cases.md) for architecture examples

## Next Steps

Now that you understand the runtime architecture, let's dive deeper into the event loop. Continue to [Event Loop Mechanics](./06-event-loop-mechanics.md).