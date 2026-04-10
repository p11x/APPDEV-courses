# 📚 JavaScript Quick Reference

## 📋 Overview

This is a quick reference guide for essential JavaScript concepts covered in this module. Keep it handy as you continue learning!

---

## 🎯 Core Concepts Summary

### What is JavaScript?
- **Language of the web** - Runs in browsers
- **Multi-paradigm** - Supports functional, OOP, procedural
- **Dynamic** - Loosely typed, interpreted
- **Event-driven** - Perfect for user interactions

### How JavaScript Works
1. **Parser** - Reads code, creates AST
2. **Compiler** - Converts to bytecode
3. **Executor** - Runs the code
4. **Event Loop** - Handles async operations
5. **Garbage Collector** - Manages memory

---

## 🛠️ Development Setup

### VS Code Essentials
| Action | Shortcut |
|--------|----------|
| Open folder | `Ctrl+K Ctrl+O` |
| New file | `Ctrl+N` |
| Save | `Ctrl+S` |
| Save all | `Ctrl+K S` |
| Find | `Ctrl+F` |
| Replace | `Ctrl+H` |
| Open terminal | `` Ctrl+` `` |

### Browser DevTools
| Action | Shortcut |
|--------|----------|
| Open DevTools | `F12` |
| Console | `Ctrl+Shift+J` |
| Inspect element | `Ctrl+Shift+C` |

---

## 📝 Syntax Basics

### Statements
```javascript
// End with semicolon
let x = 5;
let y = 10;

// Block structure
if (condition) {
    // code block
}
```

### Comments
```javascript
// Single line comment

/*
 * Multi-line
 * comment
 */
```

### Naming Conventions
```javascript
// Variables: camelCase
let userName = "John";

// Constants: UPPER_CASE
const MAX_SIZE = 100;

// Classes: PascalCase
class UserProfile {}
```

---

## 💬 Output Methods

```javascript
// Console (development)
console.log("Message");
console.info("Info");
console.warn("Warning");
console.error("Error");

// DOM
document.getElementById('output').textContent = "Hello";
element.innerHTML = "<strong>Bold</strong>";

// Alert (use sparingly)
alert("Message");

// Document write (avoid after load)
document.write("Content");
```

---

## 🔧 Variables

### Declaration Types

```javascript
// var - function scoped, can be reassigned (avoid)
var oldWay = "value";

// let - block scoped, can be reassigned
let count = 0;
count = 1; // OK

// const - block scoped, cannot be reassigned
const PI = 3.14;
// PI = 3.15; // Error!

// const with objects (properties can change)
const user = { name: "John" };
user.name = "Jane"; // OK
// user = {}; // Error!
```

---

## ➕ Operators

### Arithmetic
```javascript
+   // Add
-   // Subtract
*   // Multiply
/   // Divide
%   // Modulo (remainder)
**  // Exponent (ES2016)
```

### Comparison
```javascript
==  // Equal (loose)
=== // Equal (strict - recommended)
!=  // Not equal
!== // Strict not equal
>   // Greater than
<   // Less than
>=  // Greater or equal
<=  // Less or equal
```

### Logical
```javascript
&&  // AND
||  // OR
!   // NOT
```

### Assignment
```javascript
=   // Assign
+=  // Add and assign
-=  // Subtract and assign
*=  // Multiply and assign
/=  // Divide and assign
++  // Increment
--  // Decrement
```

---

## 🔄 Control Flow

### If Statement
```javascript
if (condition) {
    // runs if true
} else if (otherCondition) {
    // runs if first false, this true
} else {
    // runs if all false
}
```

### Ternary Operator
```javascript
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
// For
for (let i = 0; i < 10; i++) { }

// While
while (condition) { }

// Do-while
do { } while (condition);

// For...of (arrays)
for (let item of array) { }

// For...in (objects)
for (let key in object) { }
```

---

## 📦 Functions

### Declaration
```javascript
function greet(name) {
    return "Hello, " + name;
}
```

### Expression
```javascript
const greet = function(name) {
    return "Hello, " + name;
};
```

### Arrow Function
```javascript
const greet = (name) => "Hello, " + name;

// With body
const greet = (name) => {
    return "Hello, " + name;
};
```

### Parameters
```javascript
// Default parameters
function greet(name = "World") { }

// Rest parameters
function sum(...numbers) { }
```

---

## 📊 Data Types

### Primitive Types
```javascript
// String
"Hello" or 'Hello'

// Number
42, 3.14

// Boolean
true, false

// Undefined
let x; // undefined

// Null
let y = null;

// Symbol (ES6)
let sym = Symbol("description")

// BigInt (ES2020)
let big = 9007199254740991n
```

### Reference Types
```javascript
// Array
[1, 2, 3]

// Object
{ name: "John", age: 30 }

// Function
function() { }
```

### Type Checking
```javascript
typeof "hello"     // "string"
typeof 42         // "number"
typeof true       // "boolean"
typeof undefined  // "undefined"
typeof {}         // "object"
typeof []         // "object" (use Array.isArray())
```

---

## 🖥️ DOM Basics

### Selecting Elements
```javascript
document.getElementById('id')
document.querySelector('.class')
document.querySelectorAll('div')
```

### Modifying Elements
```javascript
element.textContent = "New text";
element.innerHTML = "<strong>Bold</strong>";
element.style.color = "blue";
element.classList.add('active');
element.setAttribute('data-id', '123');
```

### Events
```javascript
element.addEventListener('click', () => { });
element.addEventListener('mouseover', handler);
element.addEventListener('keypress', handler);
```

---

## ⚠️ Common Mistakes

| Mistake | Solution |
|---------|----------|
| Using `var` | Use `let` or `const` |
| `==` instead of `===` | Use strict equality |
| Forgetting semicolons | Always use semicolons |
| Not handling NaN | Use `isNaN()` or `Number.isNaN()` |
| Mutating const objects | Use spread operator to copy |
| Global variables | Use IIFE or modules |
| Memory leaks | Clear intervals, remove event listeners |

---

## 🧪 Quick Tests

### Test 1: Variable Declaration
```javascript
// What outputs?
let x = 5;
let y = x++;
console.log(x, y); // 6, 5
```

### Test 2: Type Coercion
```javascript
// What outputs?
console.log("5" + 3);  // "53"
console.log("5" - 3);  // 2
console.log(true + 1); // 2
```

### Test 3: Scope
```javascript
var x = 1;
if (true) {
    var x = 2;
    let y = 3;
}
console.log(x); // 2
// console.log(y); // ReferenceError
```

---

## 📚 Next Steps

Continue your learning journey:

1. **Variables Deep Dive** - Learn var, let, const differences
2. **Data Types** - Explore all JavaScript types
3. **Operators** - Master operators and expressions
4. **Control Flow** - Conditionals and loops
5. **Functions** - Create reusable code

---

## 🔗 Resources

| Resource | URL |
|----------|-----|
| MDN JavaScript | [Link](https://developer.mozilla.org/en-US/docs/Web/JavaScript) |
| JavaScript.info | [Link](https://javascript.info/) |
| Eloquent JavaScript | [Link](https://eloquentjavascript.net/) |

---

**Keep this reference handy! Happy coding! 🚀**