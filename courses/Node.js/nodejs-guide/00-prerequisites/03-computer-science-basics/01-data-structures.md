# Data Structures for Node.js Development

## What You'll Learn

- Essential data structures in JavaScript
- When to use each data structure
- Performance characteristics and trade-offs
- Practical examples and use cases

## Arrays

Arrays are ordered collections that can hold any type of data.

### Basic Operations

```javascript
// Creating arrays
const arr1 = [1, 2, 3, 4, 5];
const arr2 = new Array(5).fill(0); // [0, 0, 0, 0, 0]
const arr3 = Array.from({ length: 5 }, (_, i) => i); // [0, 1, 2, 3, 4]

// Accessing elements
const first = arr1[0]; // 1
const last = arr1[arr1.length - 1]; // 5
const last2 = arr1.at(-1); // 5 (modern syntax)

// Adding/removing elements
arr1.push(6); // Add to end: [1, 2, 3, 4, 5, 6]
arr1.unshift(0); // Add to start: [0, 1, 2, 3, 4, 5, 6]
arr1.pop(); // Remove from end: [0, 1, 2, 3, 4, 5]
arr1.shift(); // Remove from start: [1, 2, 3, 4, 5]

// Splice (add/remove at any position)
arr1.splice(2, 0, 'a', 'b'); // Insert at index 2
arr1.splice(2, 2); // Remove 2 elements at index 2
```

### Performance Characteristics

| Operation | Time Complexity | Notes |
|-----------|-----------------|-------|
| Access | O(1) | Direct index access |
| Search | O(n) | Linear search |
| Insert at end | O(1) | Amortized |
| Insert at start | O(n) | Requires shifting |
| Delete at end | O(1) | |
| Delete at start | O(n) | Requires shifting |

### Dynamic Arrays

JavaScript arrays are dynamic - they resize automatically.

```javascript
// Arrays grow as needed
const arr = [];
for (let i = 0; i < 1000; i++) {
    arr.push(i); // Array grows automatically
}

// Pre-allocate for better performance
const preAllocated = new Array(1000);
for (let i = 0; i < 1000; i++) {
    preAllocated[i] = i;
}
```

## Objects

Objects are key-value stores with string keys.

### Basic Operations

```javascript
// Creating objects
const obj1 = { name: 'Alice', age: 30 };
const obj2 = new Object();
const obj3 = Object.create(null); // No prototype

// Accessing properties
const name = obj1.name; // 'Alice'
const age = obj1['age']; // 30
const key = 'email';
const email = obj1[key]; // undefined

// Adding/modifying properties
obj1.email = 'alice@example.com';
obj1['phone'] = '123-456-7890';

// Deleting properties
delete obj1.phone;

// Checking properties
const hasName = 'name' in obj1; // true
const hasOwnName = obj1.hasOwnProperty('name'); // true
```

### Object Methods

```javascript
const user = { name: 'Alice', age: 30, role: 'admin' };

// Get keys, values, entries
const keys = Object.keys(user); // ['name', 'age', 'role']
const values = Object.values(user); // ['Alice', 30, 'admin']
const entries = Object.entries(user); // [['name', 'Alice'], ['age', 30], ['role', 'admin']]

// Object.assign - merge objects
const defaults = { timeout: 5000, retries: 3 };
const userSettings = { timeout: 10000 };
const config = Object.assign({}, defaults, userSettings);

// Object.freeze - make immutable
const frozen = Object.freeze({ name: 'Alice' });
frozen.name = 'Bob'; // Silently fails

// Object.seal - prevent adding/deleting properties
const sealed = Object.seal({ name: 'Alice' });
sealed.email = 'alice@example.com'; // Fails
sealed.name = 'Bob'; // Works (modification allowed)
```

### Performance Characteristics

| Operation | Time Complexity | Notes |
|-----------|-----------------|-------|
| Access | O(1) | Hash table lookup |
| Search | O(1) | By key |
| Insert | O(1) | |
| Delete | O(1) | |

### Object vs Map

```javascript
// Object - string keys only
const obj = {
    name: 'Alice',
    42: 'number key', // Converted to string '42'
    [Symbol('id')]: 123 // Symbol keys work
};

// Map - any key type
const map = new Map();
map.set('name', 'Alice');
map.set(42, 'number key');
map.set({ id: 1 }, 'object key');
map.set(function() {}, 'function key');

// Map advantages
// 1. Any key type
// 2. Maintains insertion order (guaranteed)
// 3. Size property
// 4. Iterable
// 5. Better performance for frequent additions/removals
```

## Maps

Maps are collections of key-value pairs with any key type.

### Basic Operations

```javascript
// Creating a Map
const map = new Map();

// Adding entries
map.set('name', 'Alice');
map.set(42, 'answer');
map.set(true, 'boolean');

// Getting entries
const name = map.get('name'); // 'Alice'
const missing = map.get('missing'); // undefined

// Checking existence
const hasName = map.has('name'); // true

// Size
console.log(map.size); // 3

// Deleting entries
map.delete(42); // true
map.clear(); // Remove all

// Iterating
for (const [key, value] of map) {
    console.log(`${key}: ${value}`);
}

// Converting to/from arrays
const arr = Array.from(map); // [[key, value], ...]
const obj = Object.fromEntries(map); // { key: value, ... }
```

### Use Cases

```javascript
// Use Map for non-string keys
const cache = new Map();
const key = { id: 1, type: 'user' };
cache.set(key, { name: 'Alice' });

// Use Map for frequent additions/deletions
const activeUsers = new Map();
activeUsers.set(userId, userData);
activeUsers.delete(userId); // O(1)

// Use Map when order matters
const ordered = new Map([
    ['first', 1],
    ['second', 2],
    ['third', 3]
]);
```

### Performance

| Operation | Time Complexity |
|-----------|-----------------|
| get | O(1) |
| set | O(1) |
| has | O(1) |
| delete | O(1) |

## Sets

Sets are collections of unique values.

### Basic Operations

```javascript
// Creating a Set
const set = new Set([1, 2, 3, 3, 4]); // {1, 2, 3, 4}

// Adding values
set.add(5);
set.add(3); // Duplicate, ignored

// Checking existence
const hasThree = set.has(3); // true

// Size
console.log(set.size); // 5

// Deleting values
set.delete(4); // true
set.clear(); // Remove all

// Iterating
for (const value of set) {
    console.log(value);
}

// Converting to array
const arr = Array.from(set); // [1, 2, 3, 4, 5]
const arr2 = [...set]; // [1, 2, 3, 4, 5]
```

### Set Operations

```javascript
// Union
const union = new Set([...setA, ...setB]);

// Intersection
const intersection = new Set([...setA].filter(x => setB.has(x)));

// Difference
const difference = new Set([...setA].filter(x => !setB.has(x)));

// Symmetric difference
const symDiff = new Set([
    ...[...setA].filter(x => !setB.has(x)),
    ...[...setB].filter(x => !setA.has(x))
]);

// Subset check
const isSubset = [...setA].every(x => setB.has(x));
```

### Use Cases

```javascript
// Remove duplicates from array
const unique = [...new Set(array)];

// Check membership efficiently
const allowedRoles = new Set(['admin', 'editor', 'viewer']);
if (allowedRoles.has(user.role)) {
    // User has valid role
}

// Track unique visitors
const visitors = new Set();
visitors.add(userId);
const uniqueCount = visitors.size;
```

### Performance

| Operation | Time Complexity |
|-----------|-----------------|
| add | O(1) |
| has | O(1) |
| delete | O(1) |

## WeakRef and FinalizationRegistry

For advanced memory management.

```javascript
// WeakRef - weak reference to object
let obj = { data: 'large data' };
const weakRef = new WeakRef(obj);

// Object can be garbage collected
obj = null;

// Try to dereference
const deref = weakRef.deref();
if (deref) {
    // Object still exists
    console.log(deref.data);
}

// FinalizationRegistry - cleanup when object is GC'd
const registry = new FinalizationRegistry((heldValue) => {
    console.log(`Object was garbage collected: ${heldValue}`);
});

let obj2 = { data: 'data' };
registry.register(obj2, 'some identifier');

obj2 = null; // Eventually triggers callback
```

## Choosing the Right Data Structure

### Decision Matrix

| Need | Use | Why |
|------|-----|-----|
| Ordered list | Array | Index access, iteration |
| Key-value pairs (string keys) | Object | Simple, fast |
| Key-value pairs (any keys) | Map | Flexible, performant |
| Unique values | Set | Automatic deduplication |
| LIFO stack | Array (push/pop) | Natural stack behavior |
| FIFO queue | Array (push/shift) | Simple queue |
| Priority queue | Array + sort | Manual implementation |
| Graph/Tree | Custom class | Complex relationships |

### Example: Choosing Between Object and Map

```javascript
// Use Object when
// 1. Keys are strings
// 2. Structure is fixed
// 3. Need JSON serialization
const config = {
    apiUrl: 'https://api.example.com',
    timeout: 5000,
    retries: 3
};

// Use Map when
// 1. Keys are not strings
// 2. Frequent additions/deletions
// 3. Need to iterate in insertion order
const cache = new Map();
cache.set(requestKey, response);
cache.delete(oldKey);
```

## Performance Considerations

### Array Performance

```javascript
// Slow: Searching in array
const users = [...]; // Large array
const found = users.find(u => u.id === targetId); // O(n)

// Fast: Use Map for lookups
const userMap = new Map(users.map(u => [u.id, u]));
const found = userMap.get(targetId); // O(1)
```

### Object Performance

```javascript
// Slow: Creating many objects
for (let i = 0; i < 1000000; i++) {
    const obj = { id: i, data: '...' }; // GC pressure
}

// Better: Reuse objects or use object pools
const pool = [];
function getObject() {
    return pool.pop() || { id: 0, data: '' };
}
function releaseObject(obj) {
    pool.push(obj);
}
```

## Troubleshooting Common Issues

### Array Reference Issues

```javascript
// Problem: Modifying original array
const original = [1, 2, 3];
const reference = original;
reference.push(4); // Original modified!

// Solution: Create copy
const copy = [...original];
copy.push(4); // Original unchanged
```

### Object Key Coercion

```javascript
// Problem: Numeric keys become strings
const obj = {};
obj[1] = 'one';
console.log(obj['1']); // 'one' (key was coerced)

// Solution: Use Map for non-string keys
const map = new Map();
map.set(1, 'one');
console.log(map.get(1)); // 'one'
```

### Set Equality

```javascript
// Problem: Objects in Sets are compared by reference
const set = new Set();
set.add({ id: 1 });
set.add({ id: 1 }); // Added (different reference)
console.log(set.size); // 2

// Solution: Use Map with string keys
const map = new Map();
const key = JSON.stringify({ id: 1 });
map.set(key, 'value');
```

## Best Practices Checklist

- [ ] Use arrays for ordered collections
- [ ] Use objects for simple key-value pairs with string keys
- [ ] Use Map for complex keys or frequent additions/deletions
- [ ] Use Set for unique value collections
- [ ] Pre-allocate arrays when size is known
- [ ] Use object spread for immutable updates
- [ ] Consider performance implications of data structure choice
- [ ] Use appropriate methods for each data structure
- [ ] Avoid modifying arrays/objects in place when possible
- [ ] Use WeakRef for memory-sensitive applications

## Performance Optimization Tips

- Use `Map` for frequent key-value operations
- Use `Set` for unique value operations
- Pre-allocate arrays with known size
- Use typed arrays for numeric data
- Consider object pooling for frequently created objects
- Use `Object.freeze` for immutable data
- Avoid nested loops when possible
- Use appropriate iteration methods

## Cross-References

- See [Algorithms and Complexity](./02-algorithms-complexity.md) for complexity analysis
- See [ES6+ Features](../02-javascript-fundamentals/01-es6-plus-features.md) for Map/Set syntax
- See [Performance Optimization](../06-performance-optimization/) for optimization techniques
- See [Node.js Installation](../05-nodejs-installation/) for environment setup

## Next Steps

Now that you understand data structures, let's explore algorithms and complexity analysis. Continue to [Algorithms and Complexity](./02-algorithms-complexity.md).