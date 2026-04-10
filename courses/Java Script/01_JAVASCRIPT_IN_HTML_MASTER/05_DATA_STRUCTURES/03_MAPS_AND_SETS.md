# JavaScript Maps and Sets: Complete Mastery Guide

JavaScript's Map and Set data structures provide powerful alternatives to traditional object and array approaches. Introduced in ES6, they offer better performance, cleaner APIs, and more predictable behavior. This comprehensive guide covers when and how to use each data structure.

---

## Table of Contents

1. [Introduction to Map and Set](#introduction-to-map-and-set)
2. [ES6 Map Deep Dive](#es6-map-deep-dive)
3. [Set Collections](#set-collections)
4. [WeakMap and WeakSet](#weakmap-and-weakset)
5. [Performance Comparison](#performance-comparison)
6. [Professional Use Cases](#professional-use-cases)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## Introduction to Map and Set

Understanding when to use Map vs plain objects, or Set vs arrays, is fundamental to writing efficient JavaScript.

### Plain Object Limitations

While objects serve many purposes, they have limitations that Maps address.

```javascript
// Object limitations
const obj = {};

// Only accepts string/Symbol keys
obj[{}] = 'value';  // Converts to "[object Object]" as string
console.log(obj['[object Object]']);  // 'value'

// Keys must be strings (before ES6)
obj[1] = 'number';  // Converts to "1"
console.log(obj['1']);  // 'number'

// Prototype properties can cause issues
const dict = { toString: 'custom', valueOf: 'custom' };
console.log(dict.toString);  // Function, not 'custom'!

// Automatic type conversion issues
obj['1'] = 'one';
obj[1] = 'one';  // Overwrites!
console.log(obj['1']);  // 'one'

// Cannot easily get size
console.log(Object.keys(obj).length);  // Need to compute
```

### Map Advantages

Maps solve many object limitations.

```javascript
// Map advantages
const map = new Map();

// Any type as key
map.set(1, 'number');
map.set(true, 'boolean');
map.set({ id: 1 }, 'object');
map.set(() => {}, 'function');

// Key equality is strict
map.set(1, 'one');
map.set('1', 'string one');  // Different key!
console.log(map.size);  // 5 - not 3

// Proper iteration
for (const [key, value] of map) {
    console.log(key, value);
}

// Direct size access
console.log(map.size);  // 5

// Methods that don't exist on objects
map.clear();
console.log(map.size);  // 0
```

### Set vs Array

Understanding when to use Set over Array.

```javascript
// Array allows duplicates
const array = [1, 2, 2, 3, 3, 3];
console.log(array.length);  // 6

// Set automatically removes duplicates
const set = new Set([1, 2, 2, 3, 3, 3]);
console.log(set.size);  // 3 -> {1, 2, 3}

// Array membership check is O(n)
const bigArray = Array.from({ length: 10000 }, (_, i) => i);
console.log(bigArray.includes(9999));  // O(n) - may be slow

// Set membership is O(1)
const bigSet = new Set(bigArray);
console.log(bigSet.has(9999));  // O(1) - fast

// Array preserves order with duplicates
const a = [1, 2, 1, 3];
console.log([...new Set(a)]);  // [1, 2, 3] - unique values
```

---

## ES6 Map Deep Dive

The Map object holds key-value pairs and remembers the original insertion order of keys.

### Map Creation and Basic Operations

```javascript
// Create empty Map
const map = new Map();

// Initialize with entries
const initialized = new Map([
    ['name', 'Alice'],
    ['age', 30]
]);

// set: Add or update entries
map.set('key', 'value');
map.set(1, 'numeric key');

// get: Retrieve values
console.log(map.get('key'));    // 'value'
console.log(map.get('missing')); // undefined

// has: Check key existence
console.log(map.has('key'));   // true

// delete: Remove entry
map.delete('key');
console.log(map.has('key'));    // false

// clear: Remove all entries
map.clear();
console.log(map.size);          // 0
```

### Map Iteration

Maps maintain insertion order and provide multiple iteration methods.

```javascript
const userRoles = new Map([
    ['alice', 'admin'],
    ['bob', 'user'],
    ['charlie', 'moderator']
]);

// Iterate over entries
for (const [user, role] of userRoles) {
    console.log(`${user}: ${role}`);
}

// keys()
for (const user of userRoles.keys()) {
    console.log(user);
}

// values()
for (const role of userRoles.values()) {
    console.log(role);
}

// entries()
for (const [user, role] of userRoles.entries()) {
    console.log(`${user}: ${role}`);
}

// Convert to array
const arrayFromMap = [...userRoles];
const keys = [...userRoles.keys()];
const values = [...userRoles.values()];

// forEach
userRoles.forEach((role, user) => {
    console.log(`${user}: ${role}`);
});
```

### Complex Keys

Maps accept any value as a key, enabling advanced patterns.

```javascript
// Object keys - useful for indexing
const dataByUser = new Map();

function storeUserData(userId, data) {
    const key = { type: 'user', id: userId };
    dataByUser.set(key, data);
}

function getUserData(userId) {
    return dataByUser.get({ type: 'user', id: userId });
}

// This won't work! - new object reference each time
storeUserData(1, { name: 'Alice' });
console.log(getUserData(1));  // undefined!

// Store reference
const userKey = { type: 'user', id: 1 };
dataByUser.set(userKey, { name: 'Alice' });
console.log(dataByUser.get(userKey));  // { name: 'Alice' }

// Function keys - memoization patterns
const calculationCache = new Map();

function memoize(fn) {
    return function(...args) {
        const key = fn.toString() + JSON.stringify(args);
        
        if (calculationCache.has(key)) {
            return calculationCache.get(key);
        }
        
        const result = fn.apply(null, args);
        calculationCache.set(key, result);
        return result;
    };
}
```

### Map Methods Reference

Complete Map API for reference.

```javascript
const map = new Map();

// Size
console.log(map.size);  // Number of entries

// clear() - remove all
map.clear();

// delete(key) - remove single
map.set('a', 1);
map.delete('a');

// entries() - iterator of [key, value]
for (const [k, v] of map.entries()) { }

// forEach(callback, thisArg)
map.forEach((value, key) => { }, thisArg);

// get(key)
map.get('key');

// has(key)
map.has('key');

// keys() - iterator of keys
for (const k of map.keys()) { }

// set(key, value)
map.set('key', 'value');

// values() - iterator of values
for (const v of map.values()) { }

// Iteration
for (const [k, v] of map) { }

// Spread
[...map];
[...map.keys()];
[...map.values()];
[...map.entries()];
```

---

## Set Collections

Set objects are collections of unique values in insertion order.

### Set Creation and Operations

```javascript
// Create empty Set
const set = new Set();

// Initialize with values
const initialized = new Set([1, 2, 3]);

// add: Add values
set.add(1);
set.add(2);
set.add(2);  // Ignored - already exists
console.log(set.size);  // 2

// has: Check membership
console.log(set.has(1));  // true
console.log(set.has(5)); // false

// delete: Remove value
set.delete(1);
console.log(set.has(1));  // false

// clear: Remove all
set.clear();
console.log(set.size);  // 0
```

### Set Iteration

Sets provide iteration similar to Maps, but only values.

```javascript
const colors = new Set(['red', 'green', 'blue']);

// Iterate values
for (const color of colors) {
    console.log(color);  // red, green, blue
}

// forEach
colors.forEach(color => {
    console.log(color);
});

// Convert to array
const array = [...colors];
const array2 = Array.from(colors);

// Spread
const combined = new Set([...colors, 'yellow', ...colors]);
console.log([...combined]);  // ['red', 'green', 'blue', 'yellow']
```

### Set Operations

Mathematical set operations can be implemented with JavaScript Sets.

```javascript
const setA = new Set([1, 2, 3, 4]);
const setB = new Set([3, 4, 5, 6]);

// Union - all elements from both
function union(a, b) {
    return new Set([...a, ...b]);
}
console.log([...union(setA, setB)]);  // [1, 2, 3, 4, 5, 6]

// Intersection - common elements
function intersection(a, b) {
    return new Set([...a].filter(x => b.has(x)));
}
console.log([...intersection(setA, setB)]);  // [3, 4]

// Difference - in A but not B
function difference(a, b) {
    return new Set([...a].filter(x => !b.has(x)));
}
console.log([...difference(setA, setB)]);  // [1, 2]

// Symmetric Difference - in either but not both
function symmetricDifference(a, b) {
    return new Set([
        ...[...a].filter(x => !b.has(x)),
        ...[...b].filter(x => !a.has(x))
    ]);
}
console.log([...symmetricDifference(setA, setB)]);  // [1, 2, 5, 6]

// Subset check
function isSubset(subset, superset) {
    return [...subset].every(x => superset.has(x));
}
console.log(isSubset(new Set([1, 2]), setA));  // true
```

### Weak Reference Values

Sets with object values use weak references for garbage collection.

```javascript
let object1 = { id: 1 };
let object2 = { id: 2 };

const weakSet = new WeakSet([object1, object2]);

console.log(weakSet.has(object1));  // true

// Object is removed when no other references exist
object1 = null;
// After garbage collection:
// weakSet no longer contains object1
```

---

## WeakMap and WeakSet

WeakMap and WeakSet hold references weakly, allowing garbage collection when keys/values are no longer referenced elsewhere.

### WeakMap Basics

WeakMaps allow garbage collection of their keys when no other references exist.

```javascript
// WeakMap - keys must be objects
const cache = new WeakMap();

// Set with object key
const userData = { userId: 1, name: 'Alice' };
cache.set(userData, { lastLogin: new Date() });

console.log(cache.get(userData));               // { lastLogin: ... }
console.log(cache.has(userData));              // true

// User data gets collected
userData = null;
// cache automatically cleans up when userData is garbage collected
```

### Use Cases for WeakMap

WeakMaps are perfect for storing private data associated with objects.

```javascript
// Private data storage
const privateData = new WeakMap();

class User {
    constructor(name, ssn) {
        // Store private data externally
        privateData.set(this, { name, ssn });
    }
    
    getName() {
        return privateData.get(this).name;
    }
    
    getSsn() {
        // Only last 4 digits shown
        const ssn = privateData.get(this).ssn;
        return `***-**-${ssn.slice(-4)}`;
    }
}

const user = new User('Alice', '123-45-6789');
console.log(user.getName());     // 'Alice'
console.log(user.getSsn());     // '***-**-6789'

// Cannot enumerate private data
// privateData is not iterable, maintaining encapsulation
```

### Caching with WeakMap

Memoization patterns benefit from WeakMap when keys are objects.

```javascript
const computedValues = new WeakMap();

function expensiveOperation(obj) {
    if (computedValues.has(obj)) {
        return computedValues.get(obj);
    }
    
    // Expensive computation
    const result = obj.data * 2;
    computedValues.set(obj, result);
    return result;
}

const object1 = { data: 10 };
console.log(expensiveOperation(object1));  // 20 - computed
console.log(expensiveOperation(object1));  // 20 - cached

// Clean up when object is no longer used
const leakPrevention = new WeakMap();
function withCleanup(fn) {
    return function(...args) {
        const cache = new Map();
        return function(...args) {
            const key = args[0];
            if (cache.has(key)) {
                return cache.get(key);
            }
            const result = fn.apply(this, args);
            cache.set(key, result);
            return result;
        };
    };
}
```

### WeakSet Use Cases

WeakSet is useful for tracking objects without preventing garbage collection.

```javascript
// Track marked elements
const markedElements = new WeakSet();

function markElement(element) {
    markedElements.add(element);
    element.classList.add('marked');
}

function isMarked(element) {
    return markedElements.has(element);
}

// When element is removed from DOM, WeakSet allows garbage collection
// markedElements automatically cleans up

// Event listener tracking
const listeners = new WeakSet();

function addOneTimeListener(element, event, handler) {
    const wrappedHandler = (e) => {
        handlers.delete(wrappedHandler);
        element.removeEventListener(event, wrappedHandler);
        handler(e);
    };
    listeners.add(wrappedHandler);
    element.addEventListener(event, wrappedHandler);
}

// Prevent duplicate listeners
const registeredElements = new WeakSet();

function registerOnce(element) {
    if (registeredElements.has(element)) {
        return false;
    }
    registeredElements.add(element);
    return true;
}
```

### Comparison with Regular Map/Set

```javascript
// Regular Map - strong references
const regularMap = new Map();
const obj = { id: 1 };
regularMap.set(obj, 'data');
obj = null;
// regularMap still has obj as key! No garbage collection

// WeakMap - allows collection when key is unreachable
const weakMap = new WeakMap();
const obj2 = { id: 2 };
weakMap.set(obj2, 'data');
obj2 = null;
// weakMap automatically removes entry when obj2 is garbage collected

// Regular Set - strong references
const regularSet = new Set();
const item = {};
regularSet.add(item);
item = null;
// regularSet still contains the reference!

// WeakSet - weak references
const weakSet = new WeakSet();
const item2 = {};
weakSet.add(item2);
item2 = null;
// weakSet allows garbage collection
```

---

## Performance Comparison

Different structures have different performance characteristics depending on the use case.

### Lookup Performance

```javascript
// Array - O(n) for searching
const array = Array.from({ length: 10000 }, (_, i) => i);
const start = performance.now();
for (let i = 0; i < 1000; i++) {
    array.includes(Math.floor(Math.random() * 10000));
}
console.log(`Array includes: ${performance.now() - start}ms`);

// Set - O(1) for membership
const set = new Set(array);
const start2 = performance.now();
for (let i = 0; i < 1000; i++) {
    set.has(Math.floor(Math.random() * 10000));
}
console.log(`Set has: ${performance.now() - start2}ms`);

// Object - O(1) for string keys
const obj = {};
for (let i = 0; i < 10000; i++) {
    obj[i] = i;
}
const start3 = performance.now();
for (let i = 0; i < 1000; i++) {
    obj[Math.floor(Math.random() * 10000)];
}
console.log(`Object access: ${performance.now() - start3}ms`);

// Map - O(1) for any key type
const map = new Map();
for (let i = 0; i < 10000; i++) {
    map.set(i, i);
}
const start4 = performance.now();
for (let i = 0; i < 1000; i++) {
    map.get(Math.floor(Math.random() * 10000));
}
console.log(`Map get: ${performance.now() - start4}ms`);
```

### When to Choose Each Structure

| Operation | Array | Set | Object | Map |
|-----------|-------|-----|--------|-----|
| Find by value | O(n) | O(1) | N/A | N/A |
| Find by key | O(n) | N/A | O(1) | O(1) |
| Insert | O(1)* | O(1) | O(1) | O(1) |
| Delete by value | O(n) | O(1) | N/A | O(1) |
| Delete by key | O(n) | N/A | O(1) | O(1) |
| Maintain order | Yes | Yes | No** | Yes |
| Any key type | No | No | No | Yes |
| Unique values | No | Yes | N/A | N/A |

\* Amortized
\** Modern engines maintain insertion order for string keys

### Memory Usage

```javascript
// Memory comparison
const millionStrings = Array.from({ length: 1000000 }, (_, i) => `item${i}`);

const array = millionStrings.map((s, i) => ({ key: s, value: i }));
const set = new Set(millionStrings);  // Set uses less memory than array for just values

// Map for key-value pairs of strings
const map = new Map(millionStrings.map((s, i) => [s, i]));

// Object - less memory but limited key types
const obj = Object.fromEntries(map);

// Array of objects - most memory
const objects = millionStrings.map((s, i) => ({ key: s, value: i }));
```

---

## Professional Use Cases

### LRU Cache Implementation with Map

```javascript
class LRUCache {
    constructor(capacity = 10) {
        this.capacity = capacity;
        this.cache = new Map();
    }
    
    get(key) {
        if (!this.cache.has(key)) return null;
        
        // Move to end (most recently used)
        const value = this.cache.get(key);
        this.cache.delete(key);
        this.cache.set(key, value);
        
        return value;
    }
    
    set(key, value) {
        // Delete if exists (will re-add at end)
        if (this.cache.has(key)) {
            this.cache.delete(key);
        }
        
        // At capacity - delete oldest (first)
        if (this.cache.size >= this.capacity) {
            const oldestKey = this.cache.keys().next().value;
            this.cache.delete(oldestKey);
        }
        
        this.cache.set(key, value);
    }
    
    delete(key) {
        return this.cache.delete(key);
    }
    
    clear() {
        this.cache.clear();
    }
    
    get size() {
        return this.cache.size;
    }
}

// Usage
const cache = new LRUCache(3);
cache.set('a', 1);
cache.set('b', 2);
cache.set('c', 3);
console.log(cache.get('a'));  // 1 - access moves to most recent
cache.set('d', 4);  // 'b' is removed (LRU)
console.log(cache.get('b')); // null
```

### MultiMap Structure

Handle multiple values per key.

```javascript
class MultiMap {
    constructor() {
        this.map = new Map();
    }
    
    add(key, value) {
        if (!this.map.has(key)) {
            this.map.set(key, new Set());
        }
        this.map.get(key).add(value);
        return this;
    }
    
    get(key) {
        return this.map.get(key) ?? new Set();
    }
    
    has(key, value) {
        if (!this.map.has(key)) return false;
        if (value === undefined) return true;
        return this.map.get(key).has(value);
    }
    
    delete(key, value) {
        if (!this.map.has(key)) return false;
        
        if (value === undefined) {
            // Delete all values for key
            return this.map.delete(key);
        }
        
        const removed = this.map.get(key).delete(value);
        if (this.map.get(key).size === 0) {
            this.map.delete(key);
        }
        return removed;
    }
    
    *entries() {
        for (const [key, values] of this.map) {
            for (const value of values) {
                yield [key, value];
            }
        }
    }
    
    get size() {
        return this.map.size;
    }
}

// Usage
const tags = new MultiMap();
tags.add('post', 1).add('post', 2).add('post', 3);
tags.add('draft', 1).add('post', 2);
console.log([...tags.get('post')]);  // [1, 2, 3]
console.log(tags.has('post', 2));    // true
```

### Unique ID Generator

```javascript
class UniqueIdGenerator {
    constructor() {
        this.ids = new Map();
        this.counter = 0;
    }
    
    generate(prefix = 'id') {
        return `${prefix}_${++this.counter}`;
    }
    
    reserve(id) {
        const currentId = `${id}_${++this.counter}`;
        if (!this.ids.has(currentId)) {
            this.ids.set(currentId, true);
            return currentId;
        }
        return this.reserve(prefix);  // Find unused
    }
}

// Usage
const generator = new UniqueIdGenerator();
console.log(generator.generate('user'));  // 'user_1'
console.log(generator.generate('post')); // 'post_2'
```

### GroupBy Classification

```javascript
function groupBy(array, keyFn) {
    return array.reduce((map, item) => {
        const key = keyFn(item);
        if (!map.has(key)) {
            map.set(key, []);
        }
        map.get(key).push(item);
        return map;
    }, new Map());
}

// Usage
const people = [
    { name: 'Alice', department: 'Engineering' },
    { name: 'Bob', department: 'Engineering' },
    { name: 'Charlie', department: 'Sales' },
    { name: 'Diana', department: 'Sales' }
];

const byDepartment = groupBy(people, p => p.department);
console.log([...byDepartment.entries()]);
// [
//   ['Engineering', [{ name: 'Alice', department: 'Engineering' }, ...]],
//   ['Sales', [{ name: 'Charlie', department: 'Sales' }, ...]]
// ]
```

### Event Emitter with WeakMap

```javascript
class WeakEventEmitter {
    constructor() {
        this.listeners = new WeakMap();
    }
    
    on(target, event, callback) {
        if (!this.listeners.has(target)) {
            this.listeners.set(target, new Map());
        }
        
        const targetListeners = this.listeners.get(target);
        if (!targetListeners.has(event)) {
            targetListeners.set(event, new Set());
        }
        
        const eventListeners = targetListeners.get(event);
        eventListeners.add(callback);
        
        return () => this.off(target, event, callback);
    }
    
    emit(target, event, ...args) {
        if (!this.listeners.has(target)) return;
        const targetListeners = this.listeners.get(target);
        if (!targetListeners.has(event)) return;
        
        targetListeners.get(event).forEach(cb => cb(...args));
    }
    
    off(target, event, callback) {
        if (!this.listeners.has(target)) return;
        const targetListeners = this.listeners.get(target);
        if (!targetListeners.has(event)) return;
        
        targetListeners.get(event).delete(callback);
    }
}

// Usage - listeners automatically cleaned up when target is garbage collected
const emitter = new WeakEventEmitter();
const button = { id: 'submit' };
emitter.on(button, 'click', () => console.log('Clicked'));
emitter.emit(button, 'click');  // 'Clicked'
button = null;  // Cleaned up
```

---

## Key Takeaways

1. **Use Map for key-value storage**: Better than objects for non-string keys, maintains order
2. **Use Set for unique collections**: Automatic deduplication, O(1) membership check
3. **Use WeakMap for private data**: Keys are objects that can be garbage collected
4. **Map preserves insertion order**: Iteration order matches insertion
5. **Set/Map have .size**: Better than Object.keys().length
6. **WeakMap/WeakSet only accept objects**: Primitives not allowed as keys
7. **Choose the right structure**: Consider key types, uniqueness, and operations needed

---

## Common Pitfalls

1. **Using objects when Map is better**: Maps handle any key type gracefully
2. **Forgetting unique value requirement**: Set removes duplicates silently
3. **Trying to iterate WeakMap/WeakSet**: Not iterable - loses entries to GC
4. **Using arrays for lookups**: O(n) vs O(1) performance difference
5. **Mutating objects as Map keys**: Loses access if key reference changes
6. **Comparing Maps/Sets**: Must compare contents, not references
7. **Not using forEach correctly**: First arg is value, second is key (opposite of Map)
8. **Confusing Object and Map**: Object keys are always strings after conversion

---

## Related Files

- **01_ARRAYS_MASTER.md**: Using Set to deduplicate arrays
- **02_OBJECTS_AND_PROPERTIES.md**: Object vs Map for key-value storage
- **04_DATA_STRUCTURES_ALGORITHMS.md**: Algorithm implementations using Map/Set
- **05_JAVASCRIPT_DATA_STRUCTURES_PATTERNS.md**: Professional implementation patterns
- **06_MEMORY_MANAGEMENT_DATA_STRUCTURES.md**: Memory considerations for collections