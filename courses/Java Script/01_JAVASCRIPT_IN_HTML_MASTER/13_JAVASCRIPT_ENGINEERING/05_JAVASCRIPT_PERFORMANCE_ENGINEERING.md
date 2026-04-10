# JavaScript Performance Engineering

> Master performance optimization techniques for building high-performance JavaScript applications

## Table of Contents

1. [Introduction to Performance Engineering](#introduction)
2. [Profiling and Diagnostics](#profiling)
3. [Optimization Techniques](#optimization)
4. [Performance Bottlenecks](#bottlenecks)
5. [Memory Leaks](#memory-leaks)
6. [Profiling Tools](#profiling-tools)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## 1. Introduction to Performance Engineering

[anchor](#1-introduction-to-performance-engineering)

Performance engineering is the systematic approach to optimizing JavaScript applications. It involves measuring, analyzing, and improving runtime performance, memory usage, and responsiveness.

### Why Performance Matters

- **User Experience**: Slow applications frustrate users
- **Conversion Rates**: Performance directly impacts business metrics
- **Search Rankings**: Core Web Vitals affect SEO
- **Resource Costs**: Efficient code reduces infrastructure costs

### Performance Metrics

| Metric | Target | Description |
|--------|--------|-------------|
| LCP | < 2.5s | Largest Contentful Paint |
| FID | < 100ms | First Input Delay |
| CLS | < 0.1 | Cumulative Layout Shift |
| TTI | < 3.8s | Time to Interactive |

---

## 2. Profiling and Diagnostics

[anchor](#2-profiling-and-diagnostics)

Profiling is the foundation of performance optimization. Before optimizing, measure.

### Performance Timing API

```javascript
// file: profiling/timing-api.js
// Using Performance API for precise measurements

function measureExecutionTime(fn, label = 'Execution') {
  const start = performance.now();
  const result = fn();
  const end = performance.now();
  
  console.log(`${label}: ${(end - start).toFixed(3)}ms`);
  return { result, duration: end - start };
}

function measureAsync(asyncFn, label = 'Async Execution') {
  return performance.mark(`${label}-start`), async () => {
    const start = performance.now();
    const result = await asyncFn();
    const end = performance.now();
    performance.mark(`${label}-end`);
    
    performance.measure(label, `${label}-start`, `${label}-end`);
    console.log(`${label}: ${(end - start).toFixed(3)}ms`);
    return result;
  };
}

// Detailed timing with PerformanceObserver
const observer = new PerformanceObserver((list) => {
  for (const entry of list.getEntries()) {
    console.log(`${entry.name}: ${entry.duration.toFixed(3)}ms`);
  }
});

observer.observe({ entryTypes: ['measure', 'navigation'] });

function fibonacci(n) {
  if (n <= 1) return n;
  return fibonacci(n - 1) + fibonacci(n - 2);
}

measureExecutionTime(() => fibonacci(30), 'Fibonacci(30)');
```

### Custom Profiler

```javascript
// file: profiling/custom-profiler.js
// Custom profiling utility

class Profiler {
  #marks = new Map();
  #measures = [];

  start(label) {
    this.#marks.set(label, performance.now());
  }

  end(label) {
    const start = this.#marks.get(label);
    if (!start) {
      console.warn(`No start mark found for: ${label}`);
      return;
    }

    const duration = performance.now() - start;
    this.#measures.push({ label, duration, timestamp: Date.now() });
    this.#marks.delete(label);

    return duration;
  }

  getReport() {
    return this.#measures
      .sort((a, b) => b.duration - a.duration)
      .map(m => ({
        label: m.label,
        duration: m.duration.toFixed(3) + 'ms',
        percentage: 0
      }));
  }

  clear() {
    this.#measures = [];
    this.#marks.clear();
  }
}

const profiler = new Profiler();

function processData(data) {
  profiler.start('processData');
  
  const result = data
    .filter(item => item.active)
    .map(item => ({
      ...item,
      computed: item.value * 2
    }))
    .sort((a, b) => b.computed - a.computed);

  profiler.end('processData');
  return result;
}

const data = Array.from({ length: 10000 }, (_, i) => ({
  id: i,
  value: Math.random() * 100,
  active: Math.random() > 0.5
}));

processData(data);
console.log('Performance Report:', profiler.getReport());
```

---

## 3. Optimization Techniques

[anchor](#3-optimization-techniques)

### Algorithm Optimization

```javascript
// file: optimization/algorithms.js
// Algorithm complexity optimization

// ❌ O(n²) - Quadratic
function findDuplicatesSlow(array) {
  const duplicates = [];
  for (let i = 0; i < array.length; i++) {
    for (let j = i + 1; j < array.length; j++) {
      if (array[i] === array[j] && !duplicates.includes(array[i])) {
        duplicates.push(array[i]);
      }
    }
  }
  return duplicates;
}

// ✅ O(n) - Linear
function findDuplicatesFast(array) {
  const seen = new Set();
  const duplicates = new Set();

  for (const item of array) {
    if (seen.has(item)) {
      duplicates.add(item);
    } else {
      seen.add(item);
    }
  }

  return Array.from(duplicates);
}

// ❌ O(2^n) - Exponential (recursive without memoization)
function fibonacciSlow(n) {
  if (n <= 1) return n;
  return fibonacciSlow(n - 1) + fibonacciSlow(n - 2);
}

// ✅ O(n) - Linear with memoization
function fibonacciFast(n, memo = {}) {
  if (n in memo) return memo[n];
  if (n <= 1) return n;
  
  memo[n] = fibonacciFast(n - 1, memo) + fibonacciFast(n - 2, memo);
  return memo[n];
}

// Benchmark
const array = Array.from({ length: 1000 }, () => Math.floor(Math.random() * 100));

console.time('Slow');
findDuplicatesSlow(array);
console.timeEnd('Slow');

console.time('Fast');
findDuplicatesFast(array);
console.timeEnd('Fast');
```

### Caching Strategies

```javascript
// file: optimization/caching.js
// Various caching implementations

// Memoization
function memoize(fn) {
  const cache = new Map();
  
  return function(...args) {
    const key = JSON.stringify(args);
    if (cache.has(key)) {
      return cache.get(key);
    }
    
    const result = fn.apply(this, args);
    cache.set(key, result);
    return result;
  };
}

// LRU Cache
class LRUCache {
  #capacity;
  #cache = new Map();

  constructor(capacity = 100) {
    this.#capacity = capacity;
  }

  get(key) {
    if (!this.#cache.has(key)) return null;
    
    const value = this.#cache.get(key);
    this.#cache.delete(key);
    this.#cache.set(key, value);
    
    return value;
  }

  put(key, value) {
    if (this.#cache.has(key)) {
      this.#cache.delete(key);
    } else if (this.#cache.size >= this.#capacity) {
      const firstKey = this.#cache.keys().next().value;
      this.#cache.delete(firstKey);
    }
    
    this.#cache.set(key, value);
  }

  clear() {
    this.#cache.clear();
  }
}

// TTL Cache
class TTLCache {
  #cache = new Map();
  #ttl;

  constructor(ttl = 60000) {
    this.#ttl = ttl;
    this.#cleanup();
  }

  set(key, value) {
    this.#cache.set(key, {
      value,
      expires: Date.now() + this.#ttl
    });
  }

  get(key) {
    const item = this.#cache.get(key);
    if (!item) return null;
    
    if (Date.now() > item.expires) {
      this.#cache.delete(key);
      return null;
    }
    
    return item.value;
  }

  #cleanup() {
    setInterval(() => {
      const now = Date.now();
      for (const [key, item] of this.#cache) {
        if (now > item.expires) {
          this.#cache.delete(key);
        }
      }
    }, this.#ttl);
  }
}

// Usage
const expensiveCalculation = memoize((n) => {
  console.log('Computing...');
  return n * n;
});

console.log(expensiveCalculation(5));
console.log(expensiveCalculation(5)); // Cached
```

### Loop Optimization

```javascript
// file: optimization/loops.js
// Loop performance patterns

// ❌ Unoptimized - creating arrays in loop
function unoptimizedFilter(data) {
  const results = [];
  for (let i = 0; i < data.length; i++) {
    if (data[i].active) {
      results.push(data[i]);
    }
  }
  return results;
}

// ✅ Optimized - pre-allocate when size is known
function optimizedFilterKnownSize(data) {
  const count = data.filter(d => d.active).length;
  const results = new Array(count);
  let index = 0;
  
  for (let i = 0; i < data.length; i++) {
    if (data[i].active) {
      results[index++] = data[i];
    }
  }
  return results;
}

// ✅ Modern - use built-in methods with hints
function modernFilter(data) {
  return data.filter(d => d.active);
}

// Use for...of for iterables
function iterateEfficiently(iterable) {
  for (const item of iterable) {
    processItem(item);
  }
}

// Break early when possible
function findFirstMatch(items, predicate) {
  for (const item of items) {
    if (predicate(item)) {
      return item;
    }
  }
  return null;
}
```

---

## 4. Performance Bottlenecks

[anchor](#4-performance-bottlenecks)

Identifying and addressing common bottlenecks.

### DOM Manipulation

```javascript
// file: bottlenecks/dom.js
// DOM performance optimization

// ❌ Bad - multiple reflows
function updateDOMBad(items) {
  const container = document.getElementById('container');
  container.innerHTML = '';
  
  items.forEach(item => {
    const div = document.createElement('div');
    div.textContent = item.name;
    container.appendChild(div); // Reflow on each append
  });
}

// ✅ Good - single reflow with DocumentFragment
function updateDOMGood(items) {
  const container = document.getElementById('container');
  const fragment = document.createDocumentFragment();
  
  items.forEach(item => {
    const div = document.createElement('div');
    div.textContent = item.name;
    fragment.appendChild(div);
  });
  
  container.appendChild(fragment); // Single reflow
}

// ✅ Better - batch updates
function updateDOMBatch(items) {
  const container = document.getElementById('container');
  container.innerHTML = items.map(item => 
    `<div>${item.name}</div>`
  ).join('');
}

// Virtual DOM pattern for frequent updates
class VirtualList {
  #container;
  #items;
  #itemHeight;
  #visibleCount;

  constructor(container, itemHeight, visibleCount) {
    this.#container = container;
    this.#itemHeight = itemHeight;
    this.#visibleCount = visibleCount;
    this.#items = [];
  }

  setItems(items) {
    this.#items = items;
    this.#render();
  }

  #render() {
    const totalHeight = this.#items.length * this.#itemHeight;
    this.#container.style.height = `${totalHeight}px`;
    this.#container.style.position = 'relative';
    
    this.#container.innerHTML = '';
    
    this.#items.slice(0, this.#visibleCount).forEach((item, index) => {
      const el = document.createElement('div');
      el.style.position = 'absolute';
      el.style.top = `${index * this.#itemHeight}px`;
      el.style.height = `${this.#itemHeight}px`;
      el.textContent = item.name;
      this.#container.appendChild(el);
    });
  }
}
```

### Event Handling

```javascript
// file: bottlenecks/events.js
// Event handler optimization

// ❌ Bad - multiple handlers
document.querySelectorAll('.btn').forEach(btn => {
  btn.addEventListener('click', handleClick);
});

// ✅ Good - event delegation
document.getElementById('container').addEventListener('click', (e) => {
  if (e.target.classList.contains('btn')) {
    handleClick(e);
  }
});

// ✅ Best - throttled handlers
function createThrottledHandler(handler, limit) {
  let inThrottle = false;
  
  return function(...args) {
    if (!inThrottle) {
      handler.apply(this, args);
      inThrottle = true;
      setTimeout(() => inThrottle = false, limit);
    }
  };
}

// Debounced search
function createDebouncedHandler(handler, delay) {
  let timeoutId;
  
  return function(...args) {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => handler.apply(this, args), delay);
  };
}

// Usage
const searchInput = document.getElementById('search');
searchInput.addEventListener('input', createDebouncedHandler((e) => {
  performSearch(e.target.value);
}, 300));
```

---

## 5. Memory Leaks

[anchor](#5-memory-leaks)

Identifying and fixing memory leaks.

### Common Leak Patterns

```javascript
// file: memory-leaks/patterns.js
// Common memory leak patterns and fixes

// ❌ Leak 1: Forgotten timers
function badPattern() {
  const data = [];
  
  setInterval(() => {
    data.push(new Date());
  }, 100);
  
  return () => data.length;
}

// ✅ Fix: Clear intervals
function goodPattern() {
  const data = [];
  const interval = setInterval(() => {
    data.push(new Date());
  }, 100);
  
  return () => {
    clearInterval(interval);
    return data.length;
  };
}

// ❌ Leak 2: Closures holding references
function createAccumulator() {
  const largeData = new Array(1000000).fill('x');
  
  return {
    add(item) {
      largeData.push(item);
      return largeData.length;
    }
  };
}

// ✅ Fix: Clean up references
function createCleanAccumulator() {
  let largeData = new Array(1000000).fill('x');
  
  return {
    add(item) {
      largeData.push(item);
      return largeData.length;
    },
    clear() {
      largeData = [];
    }
  };
}

// ❌ Leak 3: Event listeners not removed
class EventHandler {
  constructor(element) {
    this.element = element;
    this.data = new Array(10000).fill(0);
    
    this.element.addEventListener('click', this.handleClick);
  }

  handleClick = (e) => {
    console.log('Clicked', this.data.length);
  }

  destroy() {
    this.element.removeEventListener('click', this.handleClick);
    this.element = null;
    this.data = null;
  }
}

// ❌ Leak 4: DOM references
function badDOMReferences() {
  const elements = [];
  
  for (let i = 0; i < 1000; i++) {
    const div = document.createElement('div');
    elements.push(div);
    document.body.appendChild(div);
  }
  
  return () => elements;
}

// ✅ Fix: Clear DOM references
function cleanDOMReferences() {
  const elements = [];
  
  for (let i = 0; i < 1000; i++) {
    const div = document.createElement('div');
    elements.push(div);
    document.body.appendChild(div);
  }
  
  return () => {
    elements.forEach(el => el.remove());
    elements.length = 0;
  };
}
```

### Memory Leak Detection

```javascript
// file: memory-leaks/detection.js
// Memory leak detection utilities

class MemoryMonitor {
  #snapshots = [];
  #interval = null;

  start(intervalMs = 5000) {
    this.#interval = setInterval(() => {
      if (performance.memory) {
        const snapshot = {
          timestamp: Date.now(),
          usedJSHeapSize: performance.memory.usedJSHeapSize,
          totalJSHeapSize: performance.memory.totalJSHeapSize,
          jsHeapSizeLimit: performance.memory.jsHeapSizeLimit
        };
        this.#snapshots.push(snapshot);
      }
    }, intervalMs);
  }

  stop() {
    clearInterval(this.#interval);
    this.#interval = null;
  }

  report() {
    if (this.#snapshots.length < 2) {
      return 'Not enough data to analyze';
    }

    const first = this.#snapshots[0];
    const last = this.#snapshots[this.#snapshots.length - 1];
    const growth = last.usedJSHeapSize - first.usedJSHeapSize;
    const growthPercent = (growth / first.usedJSHeapSize) * 100;

    return {
      startSize: this.#formatSize(first.usedJSHeapSize),
      endSize: this.#formatSize(last.usedJSHeapSize),
      growth: this.#formatSize(growth),
      growthPercent: growthPercent.toFixed(2) + '%',
      snapshots: this.#snapshots.length
    };
  }

  #formatSize(bytes) {
    const mb = bytes / (1024 * 1024);
    return `${mb.toFixed(2)} MB`;
  }

  clear() {
    this.#snapshots = [];
  }
}

const monitor = new MemoryMonitor();
monitor.start();
```

---

## 6. Profiling Tools

[anchor](#6-profiling-tools)

### Chrome DevTools Integration

```javascript
// file: profiling/devtools.js
// Programmatic DevTools interactions

function profileFunction(fn, name = 'Function') {
  console.profile(name);
  try {
    return fn();
  } finally {
    console.profileEnd(name);
  }
}

function debugPerformanceMarks() {
  performance.mark('app-start');
  
  setTimeout(() => {
    performance.mark('async-complete');
    performance.measure('async-duration', 'app-start', 'async-complete');
  }, 100);
}

function consoleTiming(label) {
  console.time(label);
  return () => console.timeEnd(label);
}
```

### Lighthouse Integration

```javascript
// file: profiling/lighthouse.js
// Running Lighthouse programmatically

import lighthouse from 'lighthouse';
import chromeLauncher from 'chrome-launcher';

async function runAudit(url) {
  const chrome = await chromeLauncher.launch();
  const options = {
    port: chrome.port,
    output: 'json',
    onlyCategories: ['performance', 'accessibility', 'best-practices', 'seo'],
    throttling: {
      cpuSlowdownFactor: 4,
      downloadThroughput: 1.6 * 1024 * 1024 / 8,
      uploadThroughput: 1.5 * 1024 * 1024 / 8
    }
  };

  const result = await lighthouse(url, options);
  await chrome.kill();

  return {
    performance: result.lhr.categories.performance.score * 100,
    accessibility: result.lhr.categories.accessibility.score * 100,
    bestPractices: result.lhr.categories['best-practices'].score * 100,
    seo: result.lhr.categories.seo.score * 100,
    metrics: {
      firstContentfulPaint: result.lhr.audits['first-contentful-paint'].numericValue,
      largestContentfulPaint: result.lhr.audits['largest-contentful-paint'].numericValue,
      totalBlockingTime: result.lhr.audits['total-blocking-time'].numericValue,
      cumulativeLayoutShift: result.lhr.audits['cumulative-layout-shift'].numericValue
    }
  };
}
```

---

## Key Takeaways

- **Measure First**: Always profile before optimizing
- **Algorithm Complexity**: Choose O(n) over O(n²) when possible
- **Caching**: Use memoization for expensive computations
- **DOM**: Minimize reflows, use DocumentFragment
- **Events**: Use delegation and throttling
- **Memory**: Clear intervals, remove listeners, avoid closures

### Performance Optimization Priority

1. Algorithmic improvements
2. Caching and memoization
3. DOM optimization
4. Network optimization
5. Micro-optimizations (rarely needed)

### Security Considerations

- **Avoid eval()**: Security risk and performance penalty
- **Sanitize input**: Prevent injection attacks
- **Secure data handling**: Don't leak sensitive data to console

---

## Common Pitfalls

1. **Premature Optimization**: Measure before optimizing
2. **Micro-optimizations**: Focus on big wins first
3. **Ignoring Mobile**: Test on real devices
4. **Caching Everything**: Memory vs speed trade-off
5. **Forgetting Cleanup**: Always clear resources

---

## Related Files

- [Design Patterns in JavaScript](./01_DESIGN_PATTERNS_JAVASCRIPT.md) - Performance patterns
- [Code Organization and Structure](./04_CODE_ORGANIZATION_AND_STRUCTURE.md) - Module performance
- [JavaScript Debugging Bible](../44_JAVASCRIPT_DEBUGGING_BIBLE.md) - Performance debugging

---

## Practice Exercises

1. **Beginner**: Optimize a slow array filtering function
2. **Intermediate**: Implement LRU cache with size limits
3. **Advanced**: Build memory profiling tool with leak detection