# 🚀 Throttling and Performance

## 📋 Overview

Throttling limits how often a function can be called over time. Unlike debouncing (which waits for inactivity), throttling ensures consistent execution at intervals.

---

## 🎯 Throttle Implementation

### Basic Throttle

```javascript
function throttle(func, limit) {
    let inThrottle;
    
    return function executedFunction(...args) {
        if (!inThrottle) {
            func.apply(this, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
}

// Usage - scroll handler
const handleScroll = throttle(() => {
    console.log('Scroll position:', window.scrollY);
}, 100);

window.addEventListener('scroll', handleScroll);
```

---

## 🔄 Throttle vs Debounce

| Feature | Throttle | Debounce |
|---------|----------|----------|
| First call | Immediate | Waits for silence |
| Subsequent calls | Rate-limited | Only after quiet period |
| Use case | Scrolling, dragging | Search, resize |
| Fires | At regular intervals | After inactivity |

---

## ⚡ Performance Optimization

### Scroll Optimization

```javascript
// Don't use scroll events for heavy operations
// ❌ Bad
window.addEventListener('scroll', () => {
    document.querySelectorAll('.item').forEach(item => {
        // Heavy DOM manipulation
    });
});

// ✅ Good - throttle and use requestAnimationFrame
const updateOnScroll = throttle(() => {
    requestAnimationFrame(() => {
        // Heavy DOM manipulation here
    });
}, 100);

window.addEventListener('scroll', updateOnScroll);
```

---

## 🔗 Related Topics

- [08_Timeout_and_Debouncing.md](./08_Timeout_and_Debouncing.md)
- [10_Event_Loop_Deep_Dive.md](./10_Event_Loop_Deep_Dive.md)

---

**Next: Learn about [Event Loop Deep Dive](./10_Event_Loop_Deep_Dive.md)**