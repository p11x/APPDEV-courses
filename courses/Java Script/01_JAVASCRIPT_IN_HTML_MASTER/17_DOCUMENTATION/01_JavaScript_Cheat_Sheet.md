# 📚 JavaScript Quick Reference Guide

## 📋 Overview

This comprehensive cheat sheet covers essential JavaScript concepts, methods, and patterns for quick reference.

---

## 🔧 Variables & Types

### Declaration

```javascript
// const - cannot reassign (use by default)
const PI = 3.14159;
const user = { name: 'John' };
user.name = 'Jane'; // OK - modify property

// let - can reassign
let count = 0;
count = 1;

// var - function scoped (avoid in modern JS)
var oldWay = 'deprecated';
```

### Type Checking

```javascript
typeof "hello"     // "string"
typeof 42         // "number"
typeof true       // "boolean"
typeof {}         // "object"
typeof []         // "object" (use Array.isArray())
typeof null       // "object" (historical bug!)
typeof undefined  // "undefined"
typeof Symbol()  // "symbol"
typeof BigInt(1)  // "bigint"
```

### Type Conversion

```javascript
// To String
String(123)           // "123"
(123).toString()      // "123"
123 + ""              // "123"

// To Number
Number("123")        // 123
parseInt("123")       // 123
parseFloat("123.45") // 123.45
+"123"                // 123

// To Boolean
Boolean(1)            // true
!!value              // double negation
Boolean(0)           // false
Boolean("")          // false
```

---

## ➕ Operators

### Arithmetic

```javascript
+    // Add / concatenate
-    // Subtract
*    // Multiply
/    // Divide
%    // Modulo (remainder)
**   // Exponent (ES2016)
++   // Increment
--   // Decrement
```

### Comparison

```javascript
==   // Equal (loose)
===  // Equal (strict - use this!)
!=   // Not equal
!==  // Not equal (strict)
>    // Greater than
<    // Less than
>=   // Greater or equal
<=   // Less or equal
```

### Logical

```javascript
&&   // AND
||   // OR
!    // NOT
??   // Nullish coalescing (ES2020)

// Nullish coalescing examples
null ?? "default"    // "default"
0 ?? "default"       // 0 (not converted!)
"" ?? "default"      // "" (not converted!)
```

---

## 🔄 Control Flow

### If Statement

```javascript
if (condition) {
    // executes if true
} else if (otherCondition) {
    // executes if first false, this true
} else {
    // executes if all false
}

// Ternary operator
const status = age >= 18 ? "adult" : "minor";
```

### Switch

```javascript
switch (value) {
    case "a":
        // code
        break;
    case "b":
        // code
        break;
    default:
        // default code
}
```

### Loops

```javascript
// for loop
for (let i = 0; i < 10; i++) { }

// for...of (arrays)
for (const item of array) { }

// for...in (objects)
for (const key in obj) { }

// while loop
while (condition) { }

// forEach
array.forEach((item, index) => { });
```

---

## 📦 Functions

### Declaration

```javascript
function greet(name) {
    return `Hello, ${name}!`;
}

// Arrow function
const greet = (name) => `Hello, ${name}!`;

// Default parameters
function greet(name = "World") { }

// Rest parameters
function sum(...numbers) {
    return numbers.reduce((a, b) => a + b, 0);
}
```

---

## 📊 Arrays

### Methods

```javascript
// Adding/removing
push()      // Add to end
pop()       // Remove from end
unshift()   // Add to start
shift()     // Remove from start

// Transformation
map()       // Transform each element
filter()    // Filter elements
reduce()    // Reduce to single value
flatMap()   // map + flat

// Searching
find()      // First matching element
findIndex() // First matching index
includes()  // Contains value?
indexOf()   // Index of value
filter()    // All matching elements

// Sorting
sort()      // Sort in place
reverse()   // Reverse in place

// Other
join()      // Join to string
slice()     // Extract portion
splice()    // Add/remove elements
concat()    // Merge arrays
```

---

## 📝 Objects

### Methods

```javascript
// Keys/values/entries
Object.keys(obj)      // ["a", "b"]
Object.values(obj)    // [1, 2]
Object.entries(obj)   // [["a", 1], ["b", 2]]

// Destructuring
const { a, b } = obj;
const { a: alias } = obj;

// Spread
const newObj = { ...obj, newProp: value };
const newArr = [...arr, newItem];
```

---

## 🔍 DOM

### Selection

```javascript
document.getElementById('id')
document.querySelector('.class')
document.querySelectorAll('div')

element.textContent = "text"
element.innerHTML = "<b>HTML</b>"
element.classList.add('active')
element.classList.remove('active')
element.classList.toggle('active')
```

### Events

```javascript
element.addEventListener('click', handler)
element.removeEventListener('click', handler)

// Event object
event.target
event.currentTarget
event.preventDefault()
event.stopPropagation()
```

---

## ⏳ Async

### Promises

```javascript
// Create promise
new Promise((resolve, reject) => { 
    // resolve(value) or reject(error)
})

// Methods
promise.then(onResolve).catch(onReject)
promise.finally(onFinally)

Promise.all([p1, p2])     // Wait all
Promise.race([p1, p2])     // First to settle
Promise.allSettled([p1, p2]) // All settled
```

### Async/Await

```javascript
async function fetchData() {
    try {
        const response = await fetch(url);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(error);
    }
}
```

---

## 🎯 Common Patterns

### Debounce

```javascript
function debounce(func, wait) {
    let timeout;
    return function(...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), wait);
    };
}
```

### Throttle

```javascript
function throttle(func, limit) {
    let inThrottle;
    return function(...args) {
        if (!inThrottle) {
            func.apply(this, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
}
```

### Optional Chaining

```javascript
const value = obj?.nested?.property;
const method = obj?.method?.();
const arr = items?.[0];
```

### Nullish Coalescing

```javascript
const value = nullValue ?? "default";
```

---

## 🔗 Related Topics

- [04_Variables_Deep_Dive.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/04_Variables_Deep_Dive.md)
- [02_Promises_Complete_Guide.md](../08_ASYNC_JAVASCRIPT/02_Promises_Complete_Guide.md)

---

**Reference Complete!** Happy Coding! 🚀