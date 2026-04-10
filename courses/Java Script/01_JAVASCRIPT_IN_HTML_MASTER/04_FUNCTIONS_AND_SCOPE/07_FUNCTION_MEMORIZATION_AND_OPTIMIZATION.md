# 📝 Function Memoization and Optimization

## 📋 Table of Contents

1. [Overview](#overview)
2. [Understanding Memoization](#understanding-memoization)
3. [Basic Memoization Patterns](#basic-memoization-patterns)
4. [Advanced Caching Strategies](#advanced-caching-strategies)
5. [Performance Considerations](#performance-considerations)
6. [Practical Use Cases](#practical-use-cases)
7. [Security Considerations](#security-considerations)
8. [Common Pitfalls](#common-pitfalls)
9. [Key Takeaways](#key-takeaways)

---

## Overview

Memoization is an optimization technique that caches the results of expensive function calls and returns cached results when the same inputs occur again. This can dramatically improve performance for recursive algorithms, computational functions, and frequently called operations. However, improper use can lead to memory leaks and unexpected behavior.

This comprehensive guide covers memoization fundamentals, various caching strategies, performance implications, and practical applications. You'll learn when to use memoization, how to implement it correctly, and common pitfalls to avoid. Modern JavaScript patterns and ES2024+ features are used throughout.

---

## Understanding Memoization

### What is Memoization?

```javascript
// students/01_whatIsMemoization.js

// Without memoization - exponential time
function fibonacciBad(n) {
    if (n <= 1) return n;
    return fibonacciBad(n - 1) + fibonacciBad(n - 2);
}

// With memoization - linear time
function fibonacciMemo(n, memo = {}) {
    if (n in memo) return memo[n];
    if (n <= 1) return n;
    
    memo[n] = fibonacciMemo(n - 1, memo) + fibonacciMemo(n - 2, memo);
    return memo[n];
}

// Performance comparison
console.time('Without memo');
console.log(fibonacciBad(30));  // 832040
console.timeEnd('Without memo');  // ~50-100ms

console.time('With memo');
console.log(fibonacciMemo(30));  // 832040
console.timeEnd('With memo');  // <1ms
```

### How Memoization Works

```javascript
// students/02_howItWorks.js

// Memoization pattern
function memoize(fn) {
    const cache = new Map();
    
    return function(...args) {
        const key = JSON.stringify(args);
        
        if (cache.has(key)) {
            console.log('Cache hit:', key);
            return cache.get(key);
        }
        
        console.log('Cache miss:', key);
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
}

// Usage
const expensiveFunction = (n) => {
    console.log('Computing for', n);
    return n * n;
};

const memoizedFn = memoize(expensiveFunction);

memoizedFn(5);  // Computes: "Computing for 5"
memoizedFn(5);  // Cache hit: "Cache hit: [5]"
memoizedFn(10); // Computes: "Computing for 10"
memoizedFn(5);  // Cache hit again
```

---

## Basic Memoization Patterns

### Generic Memoize Function

```javascript
// students/03_genericMemoize.js

// Simple memoization wrapper
function memoize(fn) {
    const cache = new Map();
    
    return function(...args) {
        const key = args.join('-');
        
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
}

// More robust version with options
function memoizeAdvanced(fn, { 
    keyGenerator = JSON.stringify,
    onCacheHit = null,
    onCacheMiss = null,
    maxSize = Infinity
} = {}) {
    const cache = new Map();
    
    return function(...args) {
        const key = keyGenerator(args);
        
        if (cache.has(key)) {
            onCacheHit?.(key, args);
            return cache.get(key);
        }
        
        onCacheMiss?.(key, args);
        const result = fn.apply(this, args);
        
        if (cache.size >= maxSize) {
            const firstKey = cache.keys().next().value;
            cache.delete(firstKey);
        }
        
        cache.set(key, result);
        return result;
    };
}

// Usage
const computeSquare = (n) => {
    console.log(`Computing ${n} squared`);
    return n * n;
};

const memoizedSquare = memoizeAdvanced(computeSquare, {
    onCacheHit: (k) => console.log(`Hit: ${k}`),
    onCacheMiss: (k) => console.log(`Miss: ${k}`)
});

memoizedSquare(5);  // Miss: [5], Computing 5 squared
memoizedSquare(5);  // Hit: [5]
memoizedSquare(10); // Miss: [10], Computing 10 squared
```

### Memoize with Context

```javascript
// students/04_memoizeContext.js

// Preserve 'this' context
function memoizeWithContext(fn) {
    const cache = new Map();
    
    return function(...args) {
        const key = args.join('-');
        
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
}

// Class method memoization
class Calculator {
    constructor() {
        this.calculate = memoizeWithContext(this.calculate);
    }
    
    calculate(a, b) {
        console.log(`Calculating ${a} + ${b}`);
        return a + b;
    }
}

const calc = new Calculator();
console.log(calc.calculate(2, 3));  // Computes
console.log(calc.calculate(2, 3));  // Cached
```

---

## Advanced Caching Strategies

### LRU Cache Implementation

```javascript
// students/05_lruCache.js

// Least Recently Used (LRU) Cache
class LRUCache {
    constructor(maxSize = 100) {
        this.maxSize = maxSize;
        this.cache = new Map();
    }
    
    get(key) {
        if (!this.cache.has(key)) return undefined;
        
        const value = this.cache.get(key);
        this.cache.delete(key);
        this.cache.set(key, value);  // Move to end (most recent)
        
        return value;
    }
    
    set(key, value) {
        if (this.cache.has(key)) {
            this.cache.delete(key);
        } else if (this.cache.size >= this.maxSize) {
            const firstKey = this.cache.keys().next().value;
            this.cache.delete(firstKey);
        }
        
        this.cache.set(key, value);
    }
    
    has(key) {
        return this.cache.has(key);
    }
    
    clear() {
        this.cache.clear();
    }
    
    get size() {
        return this.cache.size;
    }
}

// Memoize with LRU cache
function memoizeLRU(fn, maxSize = 100) {
    const cache = new LRUCache(maxSize);
    
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
const fibonacciLRU = memoizeLRU((n) => {
    if (n <= 1) return n;
    return fibonacciLRU(n - 1) + fibonacciLRU(n - 2);
});

console.log(fibonacciLRU(100));  // Fast with caching
```

### TTL Cache (Time-to-Live)

```javascript
// students/06_ttlCache.js

// Cache with expiration
class TTLCache {
    constructor(ttl = 60000) {  // Default 1 minute
        this.ttl = ttl;
        this.cache = new Map();
    }
    
    set(key, value) {
        this.cache.set(key, {
            value,
            expires: Date.now() + this.ttl
        });
    }
    
    get(key) {
        const item = this.cache.get(key);
        
        if (!item) return undefined;
        
        if (Date.now() > item.expires) {
            this.cache.delete(key);
            return undefined;
        }
        
        return item.value;
    }
    
    has(key) {
        return this.get(key) !== undefined;
    }
}

// Memoize with TTL
function memoizeTTL(fn, ttl = 60000) {
    const cache = new TTLCache(ttl);
    
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

// Usage - API response caching
const fetchWithCache = memoizeTTL(async (url) => {
    console.log('Fetching:', url);
    const response = await fetch(url);
    return response.json();
}, 30000);  // 30 second cache

fetchWithCache('https://api.example.com/data');  // Fetches
fetchWithCache('https://api.example.com/data');  // Cached
```

### WeakMap Cache

```javascript
// students/07_weakMap.js

// Use WeakMap for object-based caching (auto-cleanup when key is GC'd)
function memoizeWithWeakMap(fn) {
    const cache = new WeakMap();
    
    return function(obj) {
        if (!cache.has(fn)) {
            cache.set(fn, new Map());
        }
        
        const fnCache = cache.get(fn);
        const key = JSON.stringify(obj);
        
        if (fnCache.has(key)) {
            return fnCache.get(key);
        }
        
        const result = fn.call(this, obj);
        fnCache.set(key, result);
        return result;
    };
}

// For functions that take objects
const processUser = memoizeWithWeakMap((user) => {
    console.log('Processing user:', user.id);
    return {
        ...user,
        fullName: `${user.firstName} ${user.lastName}`,
        initials: `${user.firstName[0]}${user.lastName[0]}`
    };
});

const user1 = { id: 1, firstName: 'Alice', lastName: 'Smith' };
const user2 = { id: 1, firstName: 'Alice', lastName: 'Smith' };

console.log(processUser(user1));  // Computes
console.log(processUser(user2));  // Different object, recomputes (or use obj.id as key)
```

---

## Performance Considerations

### When to Use Memoization

```javascript
// students/08_whenToUse.js

// ✅ USE MEMOIZATION FOR:
// 1. Pure functions with expensive computation
const memoizedFibonacci = memoize((n) => {
    if (n <= 1) return n;
    return memoizedFibonacci(n - 1) + memoizedFibonacci(n - 2);
});

// 2. Recursive algorithms
const memoizedFactorial = memoize((n) => {
    if (n <= 1) return 1;
    return n * memoizedFactorial(n - 1);
});

// 3. API calls / data fetching
const fetchUser = memoize(async (id) => {
    const response = await fetch(`/api/users/${id}`);
    return response.json();
});

// 4. Complex data transformations
const parseConfig = memoize((configString) => {
    return JSON.parse(configString).validate().normalize();
});

// ❌ DON'T USE MEMOIZATION FOR:
// 1. Functions with side effects
const badMemoize = memoize((n) => {
    console.log('Side effect!');  // Side effect - may not run from cache
    return n * 2;
});

// 2. Functions with random values
const randomResult = memoize(() => Math.random());  // Always returns same cached value!

// 3. Functions with time-dependent results
const getCurrentTime = memoize(() => Date.now());  // Wrong! Time changes

// 4. Very fast functions (overhead > benefit)
const fastAdd = memoize((a, b) => a + b);  // Overhead not worth it
```

### Measuring Performance Impact

```javascript
// students/09_performanceMeasure.js

function benchmark(fn, args, iterations = 1000) {
    const times = [];
    
    for (let i = 0; i < iterations; i++) {
        const start = performance.now();
        fn(...args);
        const end = performance.now();
        times.push(end - start);
    }
    
    const avg = times.reduce((a, b) => a + b) / times.length;
    const min = Math.min(...times);
    const max = Math.max(...times);
    
    return { avg: avg.toFixed(3), min: min.toFixed(3), max: max.toFixed(3) };
}

// Compare memoized vs non-memoized
const complexCalc = (n) => {
    let result = 0;
    for (let i = 0; i < n * 1000; i++) {
        result += Math.sqrt(i);
    }
    return result;
};

const memoizedComplex = memoize(complexCalc);

// Warm up cache
memoizedComplex(100);
memoizedComplex(200);

// Benchmark
console.log('Non-memoized (cached):', benchmark(complexCalc, [100], 100));
console.log('Memoized (cached):', benchmark(memoizedComplex, [100], 100));
console.log('Non-memoized (fresh):', benchmark(complexCalc, [200], 100));
console.log('Memoized (fresh):', benchmark(memoizedComplex, [200], 100));
```

### Memory Considerations

```javascript
// students/10_memoryConsiderations.js

// Unlimited cache can cause memory issues
function memoizeUnbounded(fn) {
    const cache = new Map();
    
    return function(...args) {
        const key = JSON.stringify(args);
        
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const result = fn.apply(this, args);
        cache.set(key, result);  // Never clears!
        return result;
    };
}

// Better: bounded cache with manual cleanup
function memoizeBounded(fn, maxEntries = 100) {
    const cache = new Map();
    
    return function(...args) {
        const key = JSON.stringify(args);
        
        if (cache.has(key)) {
            const value = cache.get(key);
            cache.delete(key);
            cache.set(key, value);  // Move to most recent
            return value;
        }
        
        const result = fn.apply(this, args);
        
        if (cache.size >= maxEntries) {
            const firstKey = cache.keys().next().value;
            cache.delete(firstKey);
        }
        
        cache.set(key, result);
        return result;
    };
}

// Manual cache invalidation
const memoizedFn = (() => {
    const cache = new Map();
    
    const fn = (...args) => {
        const key = JSON.stringify(args);
        if (cache.has(key)) return cache.get(key);
        
        const result = args[0] * 2;  // Expensive computation
        cache.set(key, result);
        return result;
    };
    
    fn.clearCache = () => cache.clear();
    fn.getCacheSize = () => cache.size;
    
    return fn;
})();

console.log(memoizedFn(5));  // 10
console.log(memoizedFn.getCacheSize());  // 1
memoizedFn.clearCache();
console.log(memoizedFn.getCacheSize());  // 0
```

---

## Practical Use Cases

### 1. API Response Caching

```javascript
// students/11_apiCaching.js

// Create API client with caching
function createCachedApiClient(baseUrl, { ttl = 60000, maxSize = 50 } = {}) {
    const cache = new Map();
    
    async function fetchWithCache(endpoint, options = {}) {
        const key = `${baseUrl}${endpoint}`;
        
        if (cache.has(key)) {
            const { value, expires } = cache.get(key);
            if (Date.now() < expires) {
                console.log('Cache hit:', key);
                return value;
            }
            cache.delete(key);
        }
        
        console.log('Cache miss:', key);
        const response = await fetch(`${baseUrl}${endpoint}`, options);
        const data = await response.json();
        
        if (cache.size >= maxSize) {
            const firstKey = cache.keys().next().value;
            cache.delete(firstKey);
        }
        
        cache.set(key, {
            value: data,
            expires: Date.now() + ttl
        });
        
        return data;
    }
    
    return {
        get: (endpoint) => fetchWithCache(endpoint),
        post: (endpoint, body) => fetchWithCache(endpoint, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        }),
        clearCache: () => cache.clear()
    };
}

const api = createCachedApiClient('https://api.example.com', { ttl: 30000 });

// Usage
async function loadUsers() {
    const users = await api.get('/users');
    console.log('Users:', users.length);
    return users;
}

loadUsers();  // Fetches
loadUsers();  // Cached
```

### 2. Computed Property Derivation

```javascript
// students/12_computedProperties.js

// Derive computed values
function deriveComputed(target, derivations) {
    const cache = new Map();
    
    const proxy = new Proxy(target, {
        get(target, prop) {
            if (derivations.has(prop)) {
                const derivation = derivations.get(prop);
                const key = JSON.stringify(target);
                
                if (cache.has(key) && cache.get(key).has(prop)) {
                    return cache.get(key).get(prop);
                }
                
                const value = derivation(target);
                
                if (!cache.has(key)) {
                    cache.set(key, new Map());
                }
                cache.get(key).set(prop, value);
                
                return value;
            }
            
            return target[prop];
        }
    });
    
    return proxy;
}

// Usage
const user = deriveComputed(
    { firstName: 'John', lastName: 'Doe', birthYear: 1990 },
    new Map([
        ['fullName', u => `${u.firstName} ${u.lastName}`],
        ['age', u => new Date().getFullYear() - u.birthYear],
        ['initials', u => `${u.firstName[0]}${u.lastName[0]}`]
    ])
);

console.log(user.fullName);  // "John Doe"
console.log(user.age);        // Current year - 1990
console.log(user.initials);  // "JD"
```

### 3. Debounced Memoization

```javascript
// students/13_debouncedMemoize.js

// Combine memoization with debouncing for search
function createDebouncedSearch(fn, delay = 300) {
    let timeoutId;
    const cache = new Map();
    
    return function(query) {
        clearTimeout(timeoutId);
        
        if (cache.has(query)) {
            console.log('Using cached result for:', query);
            return Promise.resolve(cache.get(query));
        }
        
        return new Promise((resolve) => {
            timeoutId = setTimeout(async () => {
                console.log('Searching for:', query);
                const result = await fn(query);
                cache.set(query, result);
                resolve(result);
            }, delay);
        });
    };
}

// Usage
const search = createDebouncedSearch(async (query) => {
    // Simulate API call
    await new Promise(r => setTimeout(r, 100));
    return [`Result 1 for "${query}"`, `Result 2 for "${query}"`];
});

// Rapid calls with same query
search('javascript');  // Schedules search
search('javascript');  // Clears previous, schedules new
search('javascript');  // Clears previous, schedules new

setTimeout(() => {
    search('javascript');  // After 300ms, actually searches
}, 500);
```

---

## Security Considerations

### Cache Poisoning Prevention

```javascript
// students/14_security.js

// Validate cache keys to prevent injection
function secureMemoize(fn) {
    const cache = new Map();
    
    return function(...args) {
        // Validate all arguments
        for (const arg of args) {
            if (typeof arg === 'object' && arg !== null) {
                // Sanitize or reject objects as keys
                if (!arg._cacheKey) {
                    throw new Error('Invalid cache key: objects must have _cacheKey');
                }
            }
        }
        
        const key = args.map(arg => {
            if (typeof arg === 'object' && arg !== null) {
                return arg._cacheKey;
            }
            return String(arg);
        }).join('|');
        
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
}

// Use structuredClone for complex objects (no prototype pollution)
function safeMemoize(fn) {
    const cache = new Map();
    
    return function(...args) {
        const key = args.map(arg => {
            try {
                return JSON.stringify(arg);
            } catch {
                return String(arg);
            }
        }).join('::');
        
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const result = fn.apply(this, args);
        
        // Deep clone to prevent mutation of cached values
        const safeResult = structuredClone?.(result) || JSON.parse(JSON.stringify(result));
        
        cache.set(key, safeResult);
        return safeResult;
    };
}
```

---

## Common Pitfalls

### 1. Mutable Arguments

```javascript
// students/15_pitfallMutable.js

// ❌ WRONG: Mutating cached value
const memoizedBad = memoize((arr) => {
    const result = arr.slice();  // Copy
    result.push('modified');
    return result;
});

const original = [1, 2, 3];
const result1 = memoizedBad(original);
result1.push('hacked!');  // Also modifies cache!

const result2 = memoizedBad(original);  // Already modified!
console.log(result2);  // [1, 2, 3, 'modified', 'hacked!']

// ✅ CORRECT: Return immutable copy
const memoizedGood = memoize((arr) => {
    return [...arr, 'modified'];  // New array
});

const original2 = [1, 2, 3];
const result3 = memoizedGood(original2);
result3.push('hacked!');  // Doesn't affect cache

const result4 = memoizedGood(original2);  // Still [1, 2, 3, 'modified']
console.log(result4);
```

### 2. Key Collisions

```javascript
// students/16_pitfallKeys.js

// ❌ WRONG: JSON stringify order matters
function badKey(a, b) {
    return JSON.stringify({ a, b });
}

console.log(badKey({ a: 1, b: 2 }));  // {"a":1,"b":2}
console.log(badKey({ b: 2, a: 1 }));  // {"b":2,"a":1} - Different!

// ✅ CORRECT: Sort keys or use proper key generation
function goodKey(a, b) {
    const sortKeys = (obj) => Object.keys(obj).sort();
    return JSON.stringify({ 
        a: sortKeys(a), 
        b: sortKeys(b) 
    });
}

// Better: use custom key generator
function memoizeWithSort(fn) {
    const cache = new Map();
    
    return function(...args) {
        const key = args.map(arg => {
            if (typeof arg === 'object' && arg !== null) {
                return Object.entries(arg)
                    .sort(([k1], [k2]) => k1.localeCompare(k2))
                    .map(([k, v]) => `${k}:${v}`)
                    .join(';');
            }
            return String(arg);
        }).join('|');
        
        if (cache.has(key)) return cache.get(key);
        
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
}
```

### 3. Cache Not Invalidated

```javascript
// students/17_pitfallInvalidate.js

// ❌ WRONG: Cache never clears with changing data
const getUserData = memoize((userId) => {
    return fetch(`/api/users/${userId}`).then(r => r.json());
});

// After user updates profile, still returns old data!

// ✅ CORRECT: Add cache invalidation
function createInvalidatableMemoize(fn) {
    const cache = new Map();
    
    const memoized = function(...args) {
        const key = JSON.stringify(args);
        
        if (cache.has(key)) {
            return cache.get(key);
        }
        
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
    
    memoized.invalidate = (...args) => {
        const key = JSON.stringify(args);
        cache.delete(key);
    };
    
    memoized.invalidateAll = () => cache.clear();
    
    return memoized;
}

const getUser = createInvalidatableMemoize(async (id) => {
    const response = await fetch(`/api/users/${id}`);
    return response.json();
});

// After user updates
getUser(1);  // Gets new data
await getUser.invalidate(1);  // Clear cache
getUser(1);  // Fetches fresh data
```

---

## Key Takeaways

1. **Pure Functions Only**: Memoization only works for pure functions with same input → same output.

2. **Cache Key Generation**: Use reliable key generation - JSON.stringify can have issues with object key order.

3. **Memory Management**: Implement cache size limits and manual invalidation to prevent memory leaks.

4. **Return Copies**: Always return copies of cached data to prevent external mutation.

5. **Context Preservation**: Use `.apply()` or preserve `this` when memoizing methods.

6. **TTL for Dynamic Data**: Use TTL caches for time-sensitive data like API responses.

7. **Performance Tradeoffs**: Memoization has overhead - only use for expensive operations.

8. **Testing**: Test memoized functions with various input patterns to verify cache behavior.

---

## Related Files

- [03_SCOPE_CHAIN_AND_CLOSURES.md](./03_SCOPE_CHAIN_AND_CLOSURES.md) - Closures for memoization
- [06_HIGHER_ORDER_FUNCTIONS.md](./06_HIGHER_ORDER_FUNCTIONS.md) - Higher order function patterns
- [08_FUNCTION_TESTING_MASTER.md](./08_FUNCTION_TESTING_MASTER.md) - Testing memoized functions