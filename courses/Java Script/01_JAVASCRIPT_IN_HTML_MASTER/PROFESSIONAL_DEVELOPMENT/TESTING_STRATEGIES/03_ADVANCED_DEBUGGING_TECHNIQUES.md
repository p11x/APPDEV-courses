# 🔍 JavaScript Advanced Debugging

## Deep Dive into Debugging Techniques

---

## Table of Contents

1. [Browser DevTools](#browser-devtools)
2. [Memory Debugging](#memory-debugging)
3. [Network Debugging](#network-debugging)
4. [Performance Profiling](#performance-profiling)

---

## Browser DevTools

### Console API

```javascript
// Basic logging
console.log('Debug info');
console.warn('Warning');
console.error('Error');

// Table for objects
console.table(users);

// Timing
console.time('operation');
// ... code ...
console.timeEnd('operation');

// Grouping
console.group('User');
console.log('Name:', user.name);
console.log('Email:', user.email);
console.groupEnd();

// Stack trace
console.trace();
```

### Elements Panel

- Inspect DOM elements
- View CSS computed values
- Edit in real-time
- Break on DOM changes

---

## Memory Debugging

### Finding Memory Leaks

```javascript
// Take heap snapshot before
console.memory;

// Make changes

// Take heap snapshot after
console.memory;

// Compare in DevTools Memory panel

// Check for detached DOM
function checkDetached() {
  const div = document.createElement('div');
  document.body.appendChild(div);
  document.body.removeChild(div);
  // div is now detached
}
```

### Common Leaks

```javascript
// 1. Global variables
window.globalVar = array; // Leak

// 2. Forgotten timers
setInterval(() => {}, 1000); // Never cleared

// 3. Event listeners
element.addEventListener('click', handler); // Never removed

// 4. Closures
function createLeak() {
  const large = new Array(1000000);
  return () => large; // Closure holding large array
}
```

---

## Network Debugging

### Interception

```javascript
// Intercept fetch
const originalFetch = window.fetch;
window.fetch = async (...args) => {
  console.log('Request:', args[0]);
  const start = Date.now();
  
  const response = await originalFetch(...args);
  
  console.log(`Response: ${response.status} (${Date.now() - start}ms)`);
  
  return response;
};

// Intercept XHR
const originalOpen = XMLHttpRequest.prototype.open;
XMLHttpRequest.prototype.open = function(method, url) {
  console.log(`${method} ${url}`);
  return originalOpen.apply(this, arguments);
};
```

### Request/Response Logging

```javascript
// Log all API requests
const apiLogger = {
  logRequest(request) {
    console.group(`📤 ${request.method} ${request.url}`);
    console.log('Headers:', request.headers);
    console.log('Body:', request.body);
    console.groupEnd();
  },
  
  logResponse(response) {
    console.group(`📥 ${response.status}`);
    console.log('Body:', response.body);
    console.groupEnd();
  }
};
```

---

## Performance Profiling

### Performance API

```javascript
// Mark
performance.mark('start-operation');
// ... code ...
performance.mark('end-operation');

// Measure
performance.measure(
  'operation duration',
  'start-operation',
  'end-operation'
);

// Get measures
const measures = performance.getEntriesByType('measure');
console.log(measures);
```

### FPS Monitoring

```javascript
function monitorFPS() {
  let lastTime = performance.now();
  let frames = 0;
  
  function loop() {
    frames++;
    const currentTime = performance.now();
    
    if (currentTime >= lastTime + 1000) {
      console.log(`FPS: ${frames}`);
      frames = 0;
      lastTime = currentTime;
    }
    
    requestAnimationFrame(loop);
  }
  
  loop();
}
```

---

## Summary

### Key Takeaways

1. **Console**: Advanced logging
2. **Memory**: Find leaks
3. **Network**: API debugging

### Debugging Tools

- Chrome DevTools
- Firefox Developer Tools
- VS Code Debugger

---

*Last updated: 2024*