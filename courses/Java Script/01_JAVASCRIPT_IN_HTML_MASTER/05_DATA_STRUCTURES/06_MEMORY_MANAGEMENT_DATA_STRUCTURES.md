# JavaScript Memory Management: Complete Mastery Guide

Understanding memory management is crucial for building performant JavaScript applications. This comprehensive guide covers garbage collection mechanisms, memory optimization techniques, preventing memory leaks, and best practices for handling large data structures in production applications.

---

## Table of Contents

1. [JavaScript Memory Model](#javascript-memory-model)
2. [Garbage Collection](#garbage-collection)
3. [Memory Optimization](#memory-optimization)
4. [Memory Leak Prevention](#memory-leak-prevention)
5. [Large Data Handling](#large-data-handling)
6. [Performance Monitoring](#performance-monitoring)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## JavaScript Memory Model

Understanding how JavaScript manages memory is fundamental to writing efficient code.

### Memory Allocation

JavaScript allocates memory automatically, but understanding the process helps identify issues.

```javascript
// Stack allocation - primitive values
function stackAllocation() {
    const num = 42;        // Stored on stack
    const str = 'hello';    // Reference stored, data elsewhere
    const bool = true;       // Stored on stack
    
    return { num, str, bool };
}

// Heap allocation - objects
function heapAllocation() {
    const arr = [1, 2, 3];     // Array stored on heap
    const obj = { a: 1 };      // Object stored on heap
    const nested = { data: [1, 2] };  // Both heap allocated
    
    return { arr, obj, nested };
}

// Memory ownership
function ownershipExample() {
    const original = { value: 1 };
    const reference = original;  // Both reference same object
    
    reference.value = 2;
    console.log(original.value);  // 2 - original modified!
    
    // Proper copy
    const copy = { ...original };
    copy.value = 3;
    console.log(original.value);  // 2 - original unchanged
}

// Shallow vs deep copy
const nested = { data: [1, 2, 3] };

// Shallow copy - nested reference preserved
const shallowCopy = { ...nested };
shallowCopy.data.push(4);
console.log(nested.data);  // [1, 2, 3, 4] - modified!

// Deep copy - completely independent
const deepCopy = JSON.parse(JSON.stringify(nested));
deepCopy.data.push(5);
console.log(nested.data);  // [1, 2, 3, 4] - unchanged

// Modern deep copy
const structured = structuredClone(nested);  // Native deep copy
```

### Reference Counting

How JavaScript tracks object references for garbage collection.

```javascript
// Reference counting example
function referenceCounting() {
    let obj = { data: 1 };  // Reference count: 1
    
    const ref1 = obj;  // Reference count: 2
    const ref2 = obj;  // Reference count: 3
    
    ref1 = null;  // Reference count: 2
    ref2 = null;  // Reference count: 0 - eligible for GC
    
    // Original reference also null
    obj = null;  // Reference count: 0 - eligible for GC
}

// Circular references
const obj1 = {};
const obj2 = {};

obj1.ref = obj2;
obj2.ref = obj1;

// Both eligible for GC when no external references
obj1 = null;
obj2 = null;
// Cycle is broken - can be collected
```

### Memory Lifecycle

```javascript
// Allocation
function allocate() {
    // New memory allocated
    const obj = { large: new Array(10000).fill('data') };
    return obj;
}

// Usage
const data = allocate();

// Usage ends - memory can be freed
// If no other references exist

// Explicit release
function explicitRelease() {
    const cache = new Map();
    
    function addToCache(key, value) {
        cache.set(key, value);
    }
    
    function clearCache() {
        cache.clear();
        // All entries now eligible for GC
    }
    
    return { addToCache, clearCache, cache };
}
```

---

## Garbage Collection

JavaScript uses automatic garbage collection, but understanding it helps write efficient code.

### Mark and Sweep

```javascript
// Mark and sweep algorithm concept
class SimpleGC {
    constructor() {
        this.heap = new Map();
        this.rootSet = new Set();
    }
    
    allocate(size) {
        const id = Math.random().toString(36).substr(2);
        this.heap.set(id, { size, data: new Array(size).fill(0) });
        return id;
    }
    
    mark() {
        // Mark all objects reachable from root
        for (const root of this.rootSet) {
            this.markRecursive(root);
        }
    }
    
    markRecursive(objId) {
        const obj = this.heap.get(objId);
        if (obj && !obj.marked) {
            obj.marked = true;
            
            for (const ref of obj.references || []) {
                this.markRecursive(ref);
            }
        }
    }
    
    sweep() {
        // Remove unmarked objects
        for (const [id, obj] of this.heap) {
            if (!obj.marked) {
                this.heap.delete(id);
            } else {
                obj.marked = false;
            }
        }
    }
}
```

### Generational GC

Modern engines use generational collection for efficiency.

```javascript
// Understanding generational behavior
function generationalBehavior() {
    // Short-lived objects in young generation
    function processRequest(data) {
        const temp = { ...data };  // Created, used, discarded
        return transform(temp);
    }
    
    // Persistent objects in old generation
    const cache = new Map();
    
    function getCached(key) {
        if (cache.has(key)) return cache.get(key);
        
        const value = createExpensive(key);
        cache.set(key, value);
        return value;
    }
    
    return { processRequest, getCached };
}

// Object lifecycle
function objectLifecycle() {
    // Young generation - frequent collection
    for (let i = 0; i < 100; i++) {
        const temporary = { index: i };  // Created/destroyed
        // Process quickly
    }
    
    // Old generation - collected less frequently
    const persistent = [];
    for (let i = 0; i < 1000; i++) {
        persistent.push({ index: i });
    }
    
    return persistent;
}
```

### GC-Friendly Patterns

```javascript
// Object pooling for frequent allocations
class ObjectPool {
    constructor(factory, reset, initialSize = 10) {
        this.factory = factory;
        this.reset = reset;
        this.pool = [];
        
        for (let i = 0; i < initialSize; i++) {
            this.pool.push(factory());
        }
    }
    
    acquire() {
        if (this.pool.length > 0) {
            const obj = this.pool.pop();
            this.reset(obj);
            return obj;
        }
        return this.factory();
    }
    
    release(obj) {
        this.pool.push(obj);
    }
}

// Pool usage for vectors
class Vector2D {
    constructor(x = 0, y = 0) {
        this.x = x;
        this.y = y;
    }
    
    reset(x = 0, y = 0) {
        this.x = x;
        this.y = y;
    }
}

const vectorPool = new ObjectPool(
    () => new Vector2D(),
    v => v.reset(0, 0),
    20
);

function calculatePoints(points) {
    const temp = vectorPool.acquire();
    
    for (let i = 0; i < points.length; i++) {
        temp.x = points[i].x;
        temp.y = points[i].y;
        // Use vector
    }
    
    vectorPool.release(temp);
}

// Avoid creating many short-lived objects
function processDataInefficient() {
    // Creates new object each iteration
    return data.map(item => ({
        value: item * 2,
        processed: true,
        timestamp: Date.now()
    }));
}

function processDataEfficient() {
    // Modify existing objects
    for (let i = 0; i < data.length; i++) {
        data[i].value = data[i].value * 2;
        data[i].processed = true;
        data[i].timestamp = Date.now();
    }
    return data;
}
```

---

## Memory Optimization

Techniques for managing memory in large applications.

### Typed Arrays

```javascript
// Using TypedArrays for numeric data
const intArray = new Int32Array(10000);
const floatArray = new Float64Array(10000);

// Equivalent regular arrays - much larger
const regularArray = new Array(10000);

// Memory comparison
const regular = Array.from({ length: 100000 }, (_, i) => i);
const typed = Int32Array.from({ length: 100000 }, (_, i) => i);

// TypedArray methods
const numbers = new Int32Array([3, 1, 4, 1, 5, 9, 2, 6]);

numbers.sort();  // In-place sort
console.log(numbers);  // Sorted

// Subarray - view into existing buffer
const buffer = new Int32Array([1, 2, 3, 4, 5]);
const subarray = buffer.subarray(1, 3);
console.log([...subarray]);  // [2, 3]

// DataView for mixed types
const buffer2 = new ArrayBuffer(8);
const view = new DataView(buffer2);

view.setInt8(0, 42);
view.setFloat64(1, 3.14);
console.log(view.getInt8(0));  // 42
console.log(view.getFloat64(1));  // 3.14
```

### ArrayBuffer and Shared Memory

```javascript
// ArrayBuffer for binary data
function arrayBufferOperations() {
    const size = 1024;
    const buffer = new ArrayBuffer(size);
    const view = new Uint8Array(buffer);
    
    // Fill
    for (let i = 0; i < size; i++) {
        view[i] = i % 256;
    }
    
    // Slice - creates new buffer
    const slice = buffer.slice(256, 512);
    console.log(slice.byteLength);  // 256
    
    return view;
}

// SharedArrayBuffer for multi-threaded access (Web Workers)
function sharedMemory() {
    const shared = new SharedArrayBuffer(1024);
    const view = new Int32Array(shared);
    
    // Atomics for thread-safe operations
    Atomics.add(view, 0, 1);
    Atomics.sub(view, 4, 1);
    Atomics.and(view, 8, 0xFF);
    Atomics.or(view, 12, 0xFF);
    
    return view;
}
```

### Data Streaming

```javascript
// Chunked processing for large data
function processLargeData(data, chunkSize = 1000) {
    const results = [];
    
    for (let i = 0; i < data.length; i += chunkSize) {
        const chunk = data.slice(i, i + chunkSize);
        
        // Process chunk
        const processed = processChunk(chunk);
        results.push(...processed);
        
        // Allow GC to run between chunks
        if (globalThis.gc) globalThis.gc();
    }
    
    return results;
}

// Generator-based processing
function* largeDataGenerator(size) {
    for (let i = 0; i < size; i++) {
        yield { index: i, data: generateData(i) };
    }
}

function processStream(generator) {
    const results = [];
    
    for (const item of generator) {
        results.push(transform(item));
        
        // Process in batches
        if (results.length >= 1000) {
            batchProcess(results);
            results = [];
        }
    }
    
    return results;
}
```

---

## Memory Leak Prevention

Identifying and fixing common memory leak patterns.

### Closures and Memory

```javascript
// Closure leak
function leakingClosure() {
    const largeData = new Array(1000000).fill('data');
    
    return function() {
        return largeData[0];  // Holds reference to largeData
    };
}

// Fix: Avoid holding unnecessary references
function nonLeakingClosure() {
    const largeData = new Array(1000000).fill('data');
    
    return function() {
        // Only reference what we need
        return largeData.length;  // No full array reference
    };
}

// Better: Explicit cleanup
function closureWithCleanup() {
    const largeData = new Array(1000000).fill('data');
    
    const fn = function() {
        return largeData[0];
    };
    
    fn.cleanup = function() {
        largeData.length = 0;
    };
    
    return fn;
}
```

### Event Listeners

```javascript
// Memory leak from event listeners
function leakingEventListeners() {
    class Component extends HTMLElement {
        constructor() {
            super();
            this.data = [];
        }
        
        connectedCallback() {
            // No cleanup when element is removed!
            window.addEventListener('resize', this.handleResize);
        }
    }
    
    return Component;
}

// Fixed with cleanup
function fixedEventListeners() {
    class Component extends HTMLElement {
        constructor() {
            super();
            this.data = [];
            this.boundResize = this.handleResize.bind(this);
        }
        
        connectedCallback() {
            window.addEventListener('resize', this.boundResize);
        }
        
        disconnectedCallback() {
            // Clean up when element removed
            window.removeEventListener('resize', this.boundResize);
            this.data = [];
        }
        
        handleResize() {
            // Handle resize
        }
    }
    
    return Component;
}

// Using AbortController
function abortControllerExample() {
    class DataFetcher {
        constructor() {
            this.abortController = new AbortController();
        }
        
        async fetch(url) {
            const response = await fetch(url, {
                signal: this.abortController.signal
            });
            return response.json();
        }
        
        abort() {
            this.abortController.abort();
        }
    }
    
    return DataFetcher;
}
```

### Timers and Intervals

```javascript
// Timer memory leak
function leakingTimer() {
    function startPeriodic() {
        const cache = [];
        
        setInterval(function() {
            // cache grows indefinitely
            cache.push(new Array(10000));
        }, 1000);
    }
    
    return startPeriodic;
}

// Fixed with Timer
function fixedTimer() {
    function startPeriodic() {
        let count = 0;
        const limit = 100;
        
        const intervalId = setInterval(function() {
            count++;
            
            if (count >= limit) {
                clearInterval(intervalId);  // Clean up
            }
        }, 1000);
        
        return intervalId;
    }
    
    return startPeriodic;
}

// RequestAnimationFrame cleanup
function animationCleanup() {
    class AnimatedComponent {
        constructor() {
            this.running = false;
            this.frameId = null;
        }
        
        start() {
            if (this.running) return;
            this.running = true;
            this.animate();
        }
        
        stop() {
            this.running = false;
            if (this.frameId) {
                cancelAnimationFrame(this.frameId);
                this.frameId = null;
            }
        }
        
        animate() {
            if (!this.running) return;
            
            // Update
            this.frameId = requestAnimationFrame(() => this.animate());
        }
    }
    
    return AnimatedComponent;
}
```

### Maps and Sets

```javascript
// Unbounded cache leak
function leakingCache() {
    const cache = new Map();
    
    function get(key, valueFn) {
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const value = valueFn();
        cache.set(key, value);
        return value;
    }
    
    return get;
}

// Fixed with size limit
function fixedCache(options = {}) {
    const maxSize = options.maxSize || 100;
    const cache = new Map();
    
    function get(key, valueFn) {
        if (cache.has(key)) {
            // Move to end (most recently used)
            const value = cache.get(key);
            cache.delete(key);
            cache.set(key, value);
            return value;
        }
        
        if (cache.size >= maxSize) {
            // Remove oldest (first item)
            const firstKey = cache.keys().next().value;
            cache.delete(firstKey);
        }
        
        const value = valueFn();
        cache.set(key, value);
        return value;
    }
    
    function clear() {
        cache.clear();
    }
    
    return { get, clear, size: () => cache.size };
}

// TTL cache
function TTLCache(maxAge = 60000) {
    const cache = new Map();
    
    function get(key) {
        const entry = cache.get(key);
        
        if (!entry) return null;
        
        if (Date.now() - entry.timestamp > maxAge) {
            cache.delete(key);
            return null;
        }
        
        return entry.value;
    }
    
    function set(key, value) {
        cache.set(key, { value, timestamp: Date.now() });
    }
    
    function cleanup() {
        const now = Date.now();
        
        for (const [key, entry] of cache) {
            if (now - entry.timestamp > maxAge) {
                cache.delete(key);
            }
        }
    }
    
    return { get, set, cleanup };
}
```

---

## Large Data Handling

Strategies for processing large datasets efficiently.

### Streaming Large Files

```javascript
// File streaming (Node.js)
async function streamLargeFile(filePath, processFn) {
    const fs = require('fs');
    const { createReadStream } = require('fs');
    
    return new Promise((resolve, reject) => {
        const stream = createReadStream(filePath, {
            highWaterMark: 1024 * 1024 // 1MB chunks
        });
        
        let totalProcessed = 0;
        
        stream.on('data', (chunk) => {
            const lines = chunk.toString().split('\n');
            totalProcessed += processFn(lines);
        });
        
        stream.on('end', () => resolve(totalProcessed));
        stream.on('error', reject);
    });
}

// Browser streaming with ReadableStream
async function streamUpload(data, onChunk) {
    const CHUNK_SIZE = 64 * 1024;
    let offset = 0;
    
    while (offset < data.length) {
        const chunk = data.slice(offset, offset + CHUNK_SIZE);
        await onChunk(chunk);
        offset += CHUNK_SIZE;
    }
}

// Virtual scrolling for large lists
class VirtualList {
    constructor() {
        this.itemHeight = 50;
        this.visibleItems = 10;
        this.renderedItems = new Map();
    }
    
    render(startIndex) {
        const container = document.getElementById('container');
        container.innerHTML = '';
        
        for (let i = startIndex; i < startIndex + this.visibleItems; i++) {
            const item = this.renderedItems.get(i);
            
            if (!item) {
                // Create virtual item
                const div = document.createElement('div');
                div.textContent = `Item ${i}`;
                container.appendChild(div);
            } else {
                container.appendChild(item);
            }
        }
    }
}
```

### Chunked Processing

```javascript
// Process data in chunks to avoid blocking
function chunkedProcess(data, processFn, chunkSize = 1000) {
    return new Promise((resolve) => {
        const results = [];
        let offset = 0;
        
        function processNext() {
            const chunk = data.slice(offset, offset + chunkSize);
            results.push(...processFn(chunk));
            offset += chunkSize;
            
            if (offset < data.length) {
                // Yield to main thread
                setTimeout(processNext, 0);
            } else {
                resolve(results);
            }
        }
        
        processNext();
    });
}

// Worker-based processing
function workerProcess(script, data) {
    return new Promise((resolve, reject) => {
        const worker = new Worker(script);
        
        worker.onmessage = (event) => {
            if (event.data.error) {
                reject(event.data.error);
            } else {
                resolve(event.data.result);
            }
        };
        
        worker.onerror = reject;
        worker.postMessage({ data });
    });
}

// Web Worker for heavy computation
const workerCode = `
    onmessage = function(e) {
        const result = heavyComputation(e.data);
        postMessage({ result });
    };
`;
```

### Pagination and Virtualization

```javascript
// Virtual list implementation
class VirtualListRenderer {
    constructor(container, itemRenderer, itemHeight = 50) {
        this.container = container;
        this.itemRenderer = itemRenderer;
        this.itemHeight = itemHeight;
        this.items = [];
        this.scrollTop = 0;
        this.visibleCount = 0;
        
        this.container.addEventListener('scroll', () => this.onScroll());
        window.addEventListener('resize', () => this.calculateVisible());
    }
    
    setItems(items) {
        this.items = items;
        this.container.style.height = `${items.length * this.itemHeight}px`;
        this.calculateVisible();
    }
    
    calculateVisible() {
        const rect = this.container.getBoundingClientRect();
        this.visibleCount = Math.ceil(rect.height / this.itemHeight);
        this.render();
    }
    
    render() {
        const start = Math.floor(this.container.scrollTop / this.itemHeight);
        const end = start + this.visibleCount + 1;
        
        // Only render visible items
        for (let i = start; i < Math.min(end, this.items.length); i++) {
            const item = this.items[i];
            const element = this.itemRenderer(item, i);
            element.style.position = 'absolute';
            element.style.top = `${i * this.itemHeight}px`;
            this.container.appendChild(element);
        }
    }
    
    onScroll() {
        this.render();
    }
}
```

---

## Performance Monitoring

Tools and techniques for monitoring memory usage.

```javascript
// Performance monitoring
function memoryMonitor() {
    if (performance.memory) {
        return {
            usedJSHeapSize: performance.memory.usedJSHeapSize,
            totalJSHeapSize: performance.memory.totalJSHeapSize,
            jsHeapSizeLimit: performance.memory.jsHeapSizeLimit,
            
            usagePercent() {
                return (this.usedJSHeapSize / this.jsHeapSizeLimit * 100).toFixed(2);
            }
        };
    }
    
    return null;
}

// Usage
const mem = memoryMonitor();
if (mem) {
    console.log(`Memory: ${mem.usagePercent()}%`);
}

// Tracking memory allocation
class MemoryTracker {
    constructor() {
        this.history = [];
        this.interval = null;
    }
    
    start(intervalMs = 1000) {
        this.interval = setInterval(() => {
            if (performance.memory) {
                this.history.push({
                    timestamp: Date.now(),
                    used: performance.memory.usedJSHeapSize
                });
            }
        }, intervalMs);
    }
    
    stop() {
        if (this.interval) {
            clearInterval(this.interval);
            this.interval = null;
        }
    }
    
    getTrend() {
        if (this.history.length < 2) return 'unknown';
        
        const recent = this.history.slice(-10);
        const first = recent[0].used;
        const last = recent[recent.length - 1].used;
        
        if (last > first * 1.1) return 'leaking';
        if (last < first * 0.9) return 'decreasing';
        return 'stable';
    }
}

// Console memory info
function logMemory() {
    const mem = performance.memory;
    if (!mem) {
        console.log('Memory API not available');
        return;
    }
    
    const formatSize = (bytes) => {
        return `${(bytes / 1024 / 1024).toFixed(2)} MB`;
    };
    
    console.log('=== Memory Report ===');
    console.log(`Used: ${formatSize(mem.usedJSHeapSize)}`);
    console.log(`Total: ${formatSize(mem.totalJSHeapSize)}`);
    console.log(`Limit: ${formatSize(mem.jsHeapSizeLimit)}`);
}
```

---

## Key Takeaways

1. **Understand the memory model**: Stack vs heap allocation
2. **Avoid circular references**: Breaking cycles for GC
3. **Use TypedArrays for numerics**: Significant memory savings
4. **Implement object pools**: Reuse frequently created objects
5. **Clean up event listeners**: Always remove when done
6. **Clear timers**: Cancel intervals/timeouts
7. **Use TTL caches**: Expire old data
8. **Monitor memory**: Track allocations in production

---

## Common Pitfalls

1. **Circular references**: Objects referencing each other
2. **Forgetting event listener cleanup**: Growing listener list
3. **Unbounded caches**: Never clearing old data
4. **Closures holding large data**: Unnecessary references
5. **DOM references**: Holding detached elements
6. **Timers not cleared**: Intervals running forever
7. **Large copied data**: Unnecessary deep copies
8. **Global variables**: Never garbage collected

---

## Related Files

- **01_ARRAYS_MASTER.md**: Array memory considerations
- **02_OBJECTS_AND_PROPERTIES.md**: Object memory model
- **03_MAPS_AND_SETS.md**: Map/Set memory patterns
- **04_DATA_STRUCTURES_ALGORITHMS.md**: Memory complexity
- **05_JAVASCRIPT_DATA_STRUCTURES_PATTERNS.md**: Memory-aware patterns