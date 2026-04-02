# C++ Integration and Native Modules

## What You'll Learn

- How JavaScript calls C++ code in Node.js
- Creating native addons with N-API
- When to use native modules vs JavaScript
- Performance implications of JS-C++ boundary

## How JavaScript Calls C++

### The Binding Layer

```
Call Stack for fs.readFile():
─────────────────────────────────────────────
JavaScript Code
│  fs.readFile('data.txt', callback)
│
├─ JS Wrapper (lib/fs.js)
│    Validates arguments
│    Normalizes options
│
├─ C++ Binding (src/node_file.cc)
│    Converts JS types to C++ types
│    Sets up async request
│
├─ libuv (deps/uv/)
│    Submits I/O to OS kernel
│    Registers callback
│
├─ OS Kernel
│    Performs actual file read
│    Notifies completion
│
├─ libuv callback
│    Receives data from kernel
│    Queues JavaScript callback
│
└─ JavaScript callback
     Executes with (err, data)
```

### C++ Binding Example

```cpp
// hello.cc — Simple native addon using N-API
#include <node_api.h>
#include <string.h>

// Function that will be called from JavaScript
static napi_value Hello(napi_env env, napi_callback_info info) {
    napi_value result;
    
    // Create a JavaScript string
    const char* str = "Hello from C++!";
    napi_create_string_utf8(env, str, strlen(str), &result);
    
    return result;
}

// Function that adds two numbers
static napi_value Add(napi_env env, napi_callback_info info) {
    size_t argc = 2;
    napi_value args[2];
    napi_get_cb_info(env, info, &argc, args, NULL, NULL);
    
    double a, b;
    napi_get_value_double(env, args[0], &a);
    napi_get_value_double(env, args[1], &b);
    
    napi_value result;
    napi_create_double(env, a + b, &result);
    
    return result;
}

// Module initialization
static napi_value Init(napi_env env, napi_value exports) {
    napi_value hello_fn, add_fn;
    
    napi_create_function(env, "hello", NAPI_AUTO_LENGTH, Hello, NULL, &hello_fn);
    napi_create_function(env, "add", NAPI_AUTO_LENGTH, Add, NULL, &add_fn);
    
    napi_set_named_property(env, exports, "hello", hello_fn);
    napi_set_named_property(env, exports, "add", add_fn);
    
    return exports;
}

NAPI_MODULE(NODE_GYP_MODULE_NAME, Init)
```

### JavaScript Usage

```javascript
// Using the native addon
const addon = require('./build/Release/addon');

console.log(addon.hello());  // "Hello from C++!"
console.log(addon.add(5, 3)); // 8
```

## N-API: The Stable ABI

### Why N-API Matters

```
Without N-API (node-gyp):
─────────────────────────────────────────────
Your C++ Addon → Node.js internals → V8 internals
│                  │                    │
│  Changes with    │  Changes with      │
│  every Node.js   │  V8 updates        │
│  version         │                    │
│                  │                    │
└──────────────────┴────────────────────┘
Must recompile for every Node.js version
Breaking changes in V8 can break your addon

With N-API:
─────────────────────────────────────────────
Your C++ Addon → N-API (stable C interface)
│                  │
│  Same ABI across │
│  Node.js 12-22+  │
│                  │
└──────────────────┘
Compile once, run on multiple versions
```

### node-addon-api (C++ Wrapper)

```cpp
// hello-napi.cc — Using node-addon-api C++ wrapper
#include <napi.h>

// Simple function
Napi::String Hello(const Napi::CallbackInfo& info) {
    Napi::Env env = info.Env();
    return Napi::String::New(env, "Hello from C++!");
}

// Function with arguments
Napi::Number Add(const Napi::CallbackInfo& info) {
    Napi::Env env = info.Env();
    
    double a = info[0].As<Napi::Number>().DoubleValue();
    double b = info[1].As<Napi::Number>().DoubleValue();
    
    return Napi::Number::New(env, a + b);
}

// Async function
Napi::Value AsyncWork(const Napi::CallbackInfo& info) {
    Napi::Env env = info.Env();
    Napi::Function callback = info[0].As<Napi::Function>();
    
    // Execute in worker thread
    auto* worker = new Napi::AsyncWorker(callback);
    // ... configure worker ...
    worker->Queue();
    
    return env.Undefined();
}

// Module init
Napi::Object Init(Napi::Env env, Napi::Object exports) {
    exports.Set("hello", Napi::Function::New(env, Hello));
    exports.Set("add", Napi::Function::New(env, Add));
    return exports;
}

NODE_API_MODULE(addon, Init)
```

### Build Configuration

```json
// binding.gyp — Build configuration
{
  "targets": [
    {
      "target_name": "addon",
      "sources": ["hello-napi.cc"],
      "include_dirs": [
        "<!@(node -p \"require('node-addon-api').include\")"
      ],
      "dependencies": [
        "<!(node -p \"require('node-addon-api').gyp\")"
      ],
      "defines": ["NAPI_DISABLE_CPP_EXCEPTIONS"]
    }
  ]
}
```

```bash
# Build the addon
npm install node-addon-api
npx node-gyp configure build

# Use in JavaScript
node -e "console.log(require('./build/Release/addon').hello())"
```

## Performance: JS ↔ C++ Boundary

### Benchmarking the Boundary

```javascript
// benchmark-native.js — Compare JS vs native performance

const { performance } = require('node:perf_hooks');

// JavaScript implementation
function jsAdd(a, b) {
    return a + b;
}

function jsFibonacci(n) {
    if (n <= 1) return n;
    return jsFibonacci(n - 1) + jsFibonacci(n - 2);
}

function jsSumArray(arr) {
    let sum = 0;
    for (let i = 0; i < arr.length; i++) {
        sum += arr[i];
    }
    return sum;
}

// Native addon (if available)
let nativeAdd, nativeFibonacci;
try {
    const addon = require('./build/Release/addon');
    nativeAdd = addon.add;
    nativeFibonacci = addon.fibonacci;
} catch (e) {
    console.log('Native addon not built — skipping native benchmarks');
}

// Benchmark helper
function benchmark(name, fn, iterations) {
    // Warmup
    for (let i = 0; i < 100; i++) fn();
    
    const start = performance.now();
    for (let i = 0; i < iterations; i++) fn();
    const elapsed = performance.now() - start;
    
    console.log(`${name}: ${elapsed.toFixed(2)}ms (${iterations} iterations)`);
}

// Results show:
// Simple operations (add): JS is faster (no boundary crossing)
// Complex operations (fibonacci): Native can be faster
// Large data (array sum): Native is faster (no GC pressure)
```

### When Native Modules Help

```
Use native modules when:
─────────────────────────────────────────────
✓ CPU-intensive computation
  - Image/video processing
  - Cryptographic operations
  - Mathematical computations

✓ System-level access
  - File system monitoring (inotify)
  - Process management
  - Hardware interaction

✓ Performance-critical paths
  - Protocol parsing
  - Data serialization
  - Compression/decompression

Avoid native modules when:
─────────────────────────────────────────────
✗ Simple data manipulation
✗ I/O-bound operations
✗ Cross-platform compatibility critical
✗ Team lacks C++ expertise
✗ Rapid development needed
```

## Popular Native Modules

```bash
# Common native modules in the ecosystem:

# bcrypt — Password hashing
npm install bcrypt

# sharp — Image processing (libvips)
npm install sharp

# better-sqlite3 — SQLite driver
npm install better-sqlite3

# node-canvas — Canvas API implementation
npm install canvas

# node-sass — Sass compilation (deprecated, use dart-sass)
# npm install node-sass

# bcryptjs — Pure JS alternative
npm install bcryptjs
# No native compilation needed, slightly slower
```

## Best Practices Checklist

- [ ] Use N-API for new native addons (not V8 directly)
- [ ] Minimize JS-C++ boundary crossings
- [ ] Provide pure-JS fallbacks when possible
- [ ] Test across Node.js LTS versions
- [ ] Use node-addon-api for cleaner C++ code
- [ ] Benchmark: native isn't always faster
- [ ] Consider WASM as alternative to native addons

## Cross-References

- See [V8 Internals](./01-v8-internals.md) for engine details
- See [Memory Management](./03-memory-management.md) for GC interaction
- See [WebAssembly Integration](../18-wasm-integration/01-wasm-basics.md) as native alternative
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for optimization

## Next Steps

Continue to [Memory Management](./03-memory-management.md) for V8 memory internals.
