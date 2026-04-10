# 🔍 JavaScript Debugging Bible

## Advanced Debugging Techniques

---

## Table of Contents

1. [Browser DevTools](#browser-devtools)
2. [Console Methods](#console-methods)
3. [Breakpoints](#breakpoints)
4. [Network Debugging](#network-debugging)
5. [Memory Leaks](#memory-leaks)

---

## Browser DevTools

### Elements Panel

- Inspect HTML elements
- Edit DOM in real-time
- View computed styles

### Sources Panel

- Set breakpoints
- Step through code
- Watch variables

---

## Console Methods

### console.log

```javascript
console.log('Hello');
// Hello
```

### console.table

```javascript
const users = [{name: 'John'}, {name: 'Jane'}];
console.table(users);
```

### console.time/timeEnd

```javascript
console.time('loop');
for (let i = 0; i < 1000; i++) {}
console.timeEnd('loop');
```

### console.group

```javascript
console.group('User');
console.log('Name: John');
console.log('Age: 30');
console.groupEnd();
```

---

## Breakpoints

### Line Breakpoint

```javascript
function add(a, b) {
  // Set breakpoint here
  return a + b;
}
```

### Conditional Breakpoint

```javascript
function process(items) {
  // Right-click breakpoint, set condition: items.length > 10
}
```

---

## Network Debugging

### Intercept Requests

```javascript
const originalFetch = window.fetch;
window.fetch = async (...args) => {
  console.log('Request:', args[0]);
  const response = await originalFetch(...args);
  console.log('Response:', response.status);
  return response;
};
```

---

## Memory Leaks

### Finding Leaks

```javascript
// Take heap snapshot before
console.memory;

// Make changes

// Take heap snapshot after
console.memory;

// Compare snapshots
```

### Common Causes

- Global variables
- Closures
- Detached DOM nodes
- Event listeners

---

## Summary

### Key Takeaways

1. **Console**: Debug output
2. **Breakpoints**: Pause execution
3. **Network**: API debugging

### Next Steps

- Continue with: [03_JAVASCRIPT_SECURITY_GUIDE.md](03_JAVASCRIPT_SECURITY_GUIDE.md)
- Study memory profiling
- Learn breakpoints types

---

## Cross-References

- **Previous**: [01_JAVASCRIPT_ENCYCLOPEDIA.md](01_JAVASCRIPT_ENCYCLOPEDIA.md)
- **Next**: [03_JAVASCRIPT_SECURITY_GUIDE.md](03_JAVASCRIPT_SECURITY_GUIDE.md)

---

*Last updated: 2024*