# 🚀 Modern ES2024 Features

## Latest JavaScript Updates

---

## Table of Contents

1. [New Array Methods](#new-array-methods)
2. [Object Extensions](#object-extensions)
3. [String Improvements](#string-improvements)
4. [Async Features](#async-features)

---

## New Array Methods

### Array.findLast()

```javascript
const numbers = [1, 2, 3, 4, 5];

// Find last even number
const lastEven = numbers.findLast(n => n % 2 === 0);
// 4

// Find last greater than 3
const lastGreater = numbers.findLast(n => n > 3);
// 5
```

### Array.findLastIndex()

```javascript
const numbers = [1, 2, 3, 4, 5];

const lastEvenIndex = numbers.findLastIndex(n => n % 2 === 0);
// 3 (index of value 4)

const lastGreaterIndex = numbers.findLastIndex(n => n > 3);
// 4 (index of value 5)
```

### Array.toReversed()

```javascript
const numbers = [1, 2, 3, 4, 5];

// Returns new reversed array
const reversed = numbers.toReversed();
// [5, 4, 3, 2, 1]

// Original unchanged
console.log(numbers);
// [1, 2, 3, 4, 5]
```

### Array.toSorted()

```javascript
const unsorted = [3, 1, 4, 1, 5, 9, 2, 6];

const sorted = unsorted.toSorted();
// [1, 1, 2, 3, 4, 5, 6, 9]

// Original unchanged
console.log(unsorted);
// [3, 1, 4, 1, 5, 9, 2, 6]
```

---

## Object Extensions

### Object.groupBy()

```javascript
const users = [
  { name: 'John', age: 25 },
  { name: 'Jane', age: 30 },
  { name: 'Bob', age: 25 }
];

const grouped = Object.groupBy(users, user => user.age);
// {
//   25: [{ name: 'John', age: 25 }, { name: 'Bob', age: 25 }],
//   30: [{ name: 'Jane', age: 30 }]
// }
```

### Object.groupBy() with Map

```javascript
const data = [1, 2, 3, 4, 5];

const grouped = Object.groupBy(data, n => n % 2 === 0 ? 'even' : 'odd');
// { odd: [1, 3, 5], even: [2, 4] }
```

### Object.entries()

```javascript
const obj = { a: 1, b: 2 };

for (const [key, value] of Object.entries(obj)) {
  console.log(`${key}: ${value}`);
}
// a: 1
// b: 2
```

---

## String Improvements

### String.isWellFormed()

```javascript
const validString = 'Hello';
const invalidString = '\uD800';

// Check if well-formed
validString.isWellFormed();    // true
invalidString.isWellFormed(); // false
```

### String.toWellFormed()

```javascript
const invalid = 'test\uD800test';

const valid = invalid.toWellFormed();
// Returns safe version or empty string
```

---

## Async Features

### Promise.withResolvers()

```javascript
const { promise, resolve, reject } = Promise.withResolvers();

promise.then(value => console.log(value));
resolve('Success');
```

### Top-Level Await (ES2022)

```javascript
// Available in modules
const data = await fetch('https://api.example.com/data');
const json = await data.json();
```

### Array.toSpliced()

```javascript
const arr = [1, 2, 3, 4, 5];

// Remove 2 elements starting at index 1
const spliced = arr.toSpliced(1, 2, 6, 7);
// [1, 6, 7, 4, 5]

// Original unchanged
console.log(arr);
// [1, 2, 3, 4, 5]
```

---

## Summary

### Key Takeaways

1. **Array**: findLast, toSorted, toReversed
2. **Object**: groupBy
3. **String**: isWellFormed

### Browser Support

Check caniuse.com for current support
Use transpilers for older browsers

---

*Last updated: 2024*