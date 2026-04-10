# 📈 JavaScript Performance Encyclopedia

## Complete Performance Guide

---

## Table of Contents

1. [Runtime Performance](#runtime-performance)
2. [Memory Performance](#memory-performance)
3. [Network Performance](#network-performance)
4. [Rendering Performance](#rendering-performance)

---

## Runtime Performance

### Debouncing

```javascript
function debounce(fn, delay = 300) {
  let timeoutId;
  
  return function(...args) {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => fn.apply(this, args), delay);
  };
}

// Usage - Search input
const debouncedSearch = debounce(search, 300);
input.addEventListener('input', debouncedSearch);
```

### Throttling

```javascript
function throttle(fn, limit = 300) {
  let inThrottle = false;
  
  return function(...args) {
    if (!inThrottle) {
      fn.apply(this, args);
      inThrottle = true;
      setTimeout(() => inThrottle = false, limit);
    }
  };
}

// Usage - Scroll events
const throttledScroll = throttle(handleScroll, 100);
window.addEventListener('scroll', throttledScroll);
```

### Memoization

```javascript
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

// Usage
const memoizedAdd = memoize((a, b) => {
  console.log('Computing...');
  return a + b;
});
```

---

## Memory Performance

### Object Pooling

```javascript
class ObjectPool {
  constructor(factory, initialSize = 10) {
    this.factory = factory;
    this.pool = [];
    
    for (let i = 0; i < initialSize; i++) {
      this.pool.push(factory());
    }
  }
  
  acquire() {
    if (this.pool.length > 0) {
      return this.pool.pop();
    }
    return this.factory();
  }
  
  release(obj) {
    if (this.pool.length < 100) { // Max pool size
      this.pool.push(obj);
    }
  }
}
```

### Event Listener Management

```javascript
class EventManager {
  constructor() {
    this.listeners = new Map();
  }
  
  add(element, event, handler) {
    const key = `${element}_${event}`;
    
    if (!this.listeners.has(key)) {
      this.listeners.set(key, handler);
      element.addEventListener(event, handler);
    }
  }
  
  remove(element, event) {
    const key = `${element}_${event}`;
    const handler = this.listeners.get(key);
    
    if (handler) {
      element.removeEventListener(event, handler);
      this.listeners.delete(key);
    }
  }
  
  clear() {
    this.listeners.forEach((handler, key) => {
      const [element, event] = key.split('_');
      element.removeEventListener(event, handler);
    });
    this.listeners.clear();
  }
}
```

---

## Network Performance

### Request Caching

```javascript
class CacheManager {
  constructor() {
    this.cache = new Map();
  }
  
  async get(url, options = {}) {
    const { ttl = 60000 } = options;
    const cached = this.cache.get(url);
    
    if (cached && Date.now() - cached.timestamp < ttl) {
      return cached.data;
    }
    
    const response = await fetch(url);
    const data = await response.json();
    
    this.cache.set(url, { data, timestamp: Date.now() });
    return data;
  }
  
  invalidate(url) {
    this.cache.delete(url);
  }
  
  clear() {
    this.cache.clear();
  }
}
```

### Request Batching

```javascript
class RequestBatcher {
  constructor(delay = 50) {
    this.delay = delay;
    this.queue = [];
    this.timer = null;
  }
  
  add(request) {
    this.queue.push(request);
    
    if (!this.timer) {
      this.timer = setTimeout(() => this.flush(), this.delay);
    }
  }
  
  async flush() {
    const batch = [...this.queue];
    this.queue = [];
    this.timer = null;
    
    const results = await Promise.allSettled(
      batch.map(req => req.execute())
    );
    
    batch.forEach((req, i) => {
      if (results[i].status === 'fulfilled') {
        req.resolve(results[i].value);
      } else {
        req.reject(results[i].reason);
      }
    });
  }
}
```

---

## Rendering Performance

### Virtual Scrolling

```javascript
class VirtualList {
  constructor(container, items, itemHeight) {
    this.container = container;
    this.items = items;
    this.itemHeight = itemHeight;
    this.render();
  }
  
  render() {
    const scrollTop = this.container.scrollTop;
    const viewportHeight = this.container.clientHeight;
    
    const startIndex = Math.floor(scrollTop / this.itemHeight);
    const endIndex = Math.min(
      startIndex + Math.ceil(viewportHeight / this.itemHeight),
      this.items.length
    );
    
    const visibleItems = this.items.slice(startIndex, endIndex);
    
    const offsetY = startIndex * this.itemHeight;
    
    this.container.innerHTML = '';
    
    visibleItems.forEach((item, index) => {
      const element = document.createElement('div');
      element.style.position = 'absolute';
      element.style.top = `${(startIndex + index) * this.itemHeight}px`;
      element.style.height = `${this.itemHeight}px`;
      element.textContent = item;
      this.container.appendChild(element);
    });
  }
}
```

---

## Summary

### Key Takeaways

1. **Debouncing**: User input optimization
2. **Pooling**: Object reuse
3. **Virtualization**: Large list rendering

### Performance Tools

- Chrome DevTools Performance
- LightHouse
- WebPageTest

---

*Last updated: 2024*