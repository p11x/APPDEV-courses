# ⚡ JavaScript Performance Guide

## Optimization Techniques

---

## Table of Contents

1. [Runtime Performance](#runtime-performance)
2. [Memory Optimization](#memory-optimization)
3. [Rendering Performance](#rendering-performance)
4. [Network Optimization](#network-optimization)

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

// Usage
const debouncedSearch = debounce(search, 300);
input.addEventListener('input', debouncedSearch);
```

### Throttling

```javascript
function throttle(fn, delay = 300) {
  let lastCall = 0;
  
  return function(...args) {
    const now = Date.now();
    
    if (now - lastCall >= delay) {
      lastCall = now;
      fn.apply(this, args);
    }
  };
}
```

---

## Memory Optimization

### Object Pooling

```javascript
class ObjectPool {
  constructor(factory, size = 10) {
    this.factory = factory;
    this.pool = [];
    
    for (let i = 0; i < size; i++) {
      this.pool.push(factory());
    }
  }
  
  acquire() {
    return this.pool.pop() || this.factory();
  }
  
  release(obj) {
    if (this.pool.length < 10) {
      this.pool.push(obj);
    }
  }
}
```

### Event Listener Cleanup

```javascript
class Component {
  setup() {
    this.handleResize = this.onResize.bind(this);
    window.addEventListener('resize', this.handleResize);
  }
  
  cleanup() {
    window.removeEventListener('resize', this.handleResize);
  }
  
  onResize() {
    // Handle resize
  }
}
```

---

## Rendering Performance

### Virtualization

```javascript
function virtualList(container, items, itemHeight) {
  const visibleItems = Math.ceil(container.clientHeight / itemHeight);
  const scrollTop = container.scrollTop;
  const startIndex = Math.floor(scrollTop / itemHeight);
  const endIndex = startIndex + visibleItems + 1;
  
  container.innerHTML = '';
  
  for (let i = startIndex; i < endIndex && i < items.length; i++) {
    const el = document.createElement('div');
    el.style.position = 'absolute';
    el.style.top = `${i * itemHeight}px`;
    el.textContent = items[i];
    container.appendChild(el);
  }
}
```

---

## Network Optimization

### Request Batching

```javascript
class RequestBatcher {
  constructor(delay = 100) {
    this.delay = delay;
    this.queue = [];
    this.timeout = null;
  }
  
  add(request) {
    this.queue.push(request);
    
    if (!this.timeout) {
      this.timeout = setTimeout(() => this.flush(), this.delay);
    }
  }
  
  async flush() {
    const requests = [...this.queue];
    this.queue = [];
    this.timeout = null;
    
    // Batch requests
    const responses = await Promise.all(
      requests.map(r => r.execute())
    );
    
    responses.forEach((response, index) => {
      requests[index].resolve(response);
    });
  }
}
```

---

## Summary

### Key Takeaways

1. **Debouncing**: Rate limiting
2. **Pooling**: Reuse objects
3. **Virtualization**: Large lists

### Next Steps

- Continue with: [../CAREER_GUIDES/01_JAVASCRIPT_INTERVIEW_READY.md](../CAREER_GUIDES/01_JAVASCRIPT_INTERVIEW_READY.md)
- Measure performance
- Use DevTools

---

## Cross-References

- **Previous**: [07_JAVASCRIPT_DESIGN_PATTERNS.md](07_JAVASCRIPT_DESIGN_PATTERNS.md)
- **Next**: [../CAREER_GUIDES/01_JAVASCRIPT_INTERVIEW_READY.md](../CAREER_GUIDES/01_JAVASCRIPT_INTERVIEW_READY.md)

---

*Last updated: 2024*