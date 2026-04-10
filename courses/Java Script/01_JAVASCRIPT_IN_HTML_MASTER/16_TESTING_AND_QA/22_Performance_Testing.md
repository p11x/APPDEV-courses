# ⚡ Performance Testing

## 📋 Overview

Performance testing ensures JavaScript applications meet speed and resource usage requirements.

---

## 🎯 Performance Metrics

### Key Metrics to Measure

| Metric | Description | Target |
|--------|-------------|--------|
| **LCP** | Largest Contentful Paint | < 2.5s |
| **FCP** | First Contentful Paint | < 1.8s |
| **TTI** | Time to Interactive | < 3.8s |
| **TBT** | Total Blocking Time | < 200ms |
| **CLS** | Cumulative Layout Shift | < 0.1 |

---

## 🎯 Measuring Performance

### Using Performance API

```javascript
// Measure execution time
function measurePerformance(fn) {
    const start = performance.now();
    fn();
    const end = performance.now();
    return end - start;
}

// Measure async operations
async function measureAsyncPerformance(fn) {
    const start = performance.now();
    await fn();
    const end = performance.now();
    return end - start;
}

// Usage
const time = measurePerformance(() => {
    // Code to measure
    const arr = Array.from({ length: 10000 }, (_, i) => i);
    arr.sort((a, b) => b - a);
});

console.log(`Execution time: ${time.toFixed(2)}ms`);
```

### Using Performance Markers

```javascript
function measureWithMarks() {
    performance.mark('start-operation');
    
    // Operation
    const result = heavyComputation();
    
    performance.mark('end-operation');
    
    performance.measure(
        'operation-duration',
        'start-operation',
        'end-operation'
    );
    
    const measures = performance.getEntriesByName('operation-duration');
    console.log(measures[0].duration);
}
```

---

## 🎯 Load Testing

```javascript
// Simple load test
function loadTest(fn, iterations = 1000) {
    const times = [];
    
    for (let i = 0; i < iterations; i++) {
        const start = performance.now();
        fn();
        times.push(performance.now() - start);
    }
    
    const avg = times.reduce((a, b) => a + b, 0) / times.length;
    const min = Math.min(...times);
    const max = Math.max(...times);
    
    return { avg, min, max, times };
}

// Run load test
const results = loadTest(() => {
    document.querySelectorAll('.item');
});

console.log(`
    Average: ${results.avg.toFixed(2)}ms
    Min: ${results.min.toFixed(2)}ms
    Max: ${results.max.toFixed(2)}ms
`);
```

---

## 🎯 Memory Testing

```javascript
// Check for memory leaks
function checkMemoryLeaks() {
    const start = performance.memory.usedJSHeapSize;
    
    // Run operation multiple times
    for (let i = 0; i < 1000; i++) {
        createAndDestroyObjects();
    }
    
    const end = performance.memory.usedJSHeapSize;
    const increase = (end - start) / 1024 / 1024; // MB
    
    console.log(`Memory increase: ${increase.toFixed(2)}MB`);
    
    if (increase > 10) {
        console.warn('Potential memory leak detected!');
    }
}
```

---

## 🔗 Related Topics

- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)
- [01_Automated_Testing_Framework.md](./01_Automated_Testing_Framework.md)

---

**Next: [Accessibility Testing](./23_Accessibility_Testing.md)**