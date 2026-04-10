# 🚀 JavaScript Advanced Techniques

## Professional Development Practices

---

## Table of Contents

1. [Modern JavaScript Features](#modern-javascript-features)
2. [Advanced Patterns](#advanced-patterns)
3. [Best Practices](#best-practices)

---

## Modern JavaScript Features

### Optional Chaining

```javascript
// Before
const city = user && user.profile && user.profile.address && user.profile.address.city;

// After (optional chaining)
const city = user?.profile?.address?.city;
```

### Nullish Coalescing

```javascript
// || returns first truthy value
const value = '' || 'default'; // 'default'

// ?? returns first non-nullish value
const value = '' ?? 'default'; // ''
```

### Logical Assignment

```javascript
// And assignment
user.name &&= 'John';

// Or assignment
user.name ||= 'Guest';

// Nullish assignment
user.name ??= 'Guest';
```

### Top-Level Await

```javascript
// ES2022 - Can use await at top level
const data = await fetch('https://api.example.com/data').then(r => r.json());
```

---

## Advanced Patterns

### Private Class Fields

```javascript
class Counter {
  #count = 0;
  
  increment() {
    this.#count++;
  }
  
  getCount() {
    return this.#count;
  }
}
```

### Record and Tuple

```javascript
// Record - object with specific key/value types
type User = Record<string, { name: string; age: number }>;

// Tuple - fixed-length array
type Point = [number, number];
const origin: Point = [0, 0];
```

### Private Methods

```javascript
class Processor {
  #process() {
    // Private
  }
  
  process() {
    return this.#process();
  }
}
```

---

## Best Practices

### Error Handling

```javascript
async function fetchData() {
  try {
    const response = await fetch(url);
    
    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }
    
    return await response.json();
  } catch (error) {
    if (error instanceof TypeError) {
      // Network error
    } else {
      // Other error
    }
    throw error;
  }
}
```

### Performance

```javascript
// Memoization
function memoize(fn) {
  const cache = new Map();
  
  return (...args) => {
    const key = JSON.stringify(args);
    
    if (cache.has(key)) {
      return cache.get(key);
    }
    
    const result = fn(...args);
    cache.set(key, result);
    return result;
  };
}
```

---

## Summary

### Key Takeaways

1. **Modern JS**: Optional chaining, nullish
2. **Private**: Class fields, methods
3. **Performance**: Memoization

### Final Tips

- Stay updated with ECMAScript proposals
- Use TypeScript for large projects
- Contribute to open source

---

## Cross-References

- **Previous**: [10_JAVASCRIPT_LIBRARIES_REFERENCE.md](10_JAVASCRIPT_LIBRARIES_REFERENCE.md)
- **Next**: [12_MODERN_JAVASCRIPT_ES2024.md](12_MODERN_JAVASCRIPT_ES2024.md)

---

*Last updated: 2024*