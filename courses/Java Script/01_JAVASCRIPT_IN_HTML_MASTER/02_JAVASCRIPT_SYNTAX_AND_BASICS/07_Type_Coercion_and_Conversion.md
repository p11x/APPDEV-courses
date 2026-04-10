# 🔄 Type Coercion and Conversion

## 📋 Overview

Type coercion is JavaScript's automatic conversion of values from one type to another. Understanding this behavior is crucial for writing predictable code and avoiding bugs.

---

## 🔃 Implicit vs Explicit Conversion

### Implicit (Automatic) Conversion

```javascript
// String concatenation with +
console.log("5" + 3);     // "53" (number becomes string)
console.log("hello" + 1); // "hello1"

// Mathematical operations
console.log("5" - 3);     // 2 (string becomes number)
console.log("5" * "2");  // 10 (both become numbers)
console.log("10" / "2"); // 5

// Boolean in arithmetic
console.log(true + 1);   // 2 (true = 1)
console.log(false + 1);  // 1 (false = 0)
console.log(true + true); // 2
```

### Explicit (Manual) Conversion

```javascript
// To String
String(123);        // "123"
(123).toString();  // "123"
123 + "";          // "123"
JSON.stringify(123);// "123"

// To Number
Number("123");     // 123
parseInt("123");   // 123
parseFloat("123.45"); // 123.45
+"123";            // 123

// To Boolean
Boolean(1);        // true
!!1;               // true
Boolean("");       // false
!!0;               // false
```

---

## ⚠️ Falsy and Truthy Values

### Falsy Values (Become false)

```javascript
// These all evaluate to false in boolean context:
false
0
-0
0n (BigInt zero)
"" (empty string)
null
undefined
NaN

// Check falsy
if (!value) {
    console.log("Falsy value:", value);
}
```

### Truthy Values (Become true)

```javascript
// Everything else is truthy:
true
1
-1
"0"
"false"
[]
{}
function() {}
new Date()
Symbol("id")
Infinity
-Infinity

// Check truthy
if (value) {
    console.log("Truthy value:", value);
}
```

### Common Pitfalls

```javascript
// ❌ Pitfall: Empty array is truthy!
if ([]) { console.log("Empty array is truthy!"); } // Runs!

// ✅ Check array length
if (arr.length > 0) { console.log("Array has items"); }

// ❌ Pitfall: Object with all falsy values
const obj = { a: 0, b: "" };
if (obj) { console.log("Object is truthy!"); } // Runs!

// ❌ Pitfall: "0" string is truthy!
if ("0") { console.log("String '0' is truthy!"); } // Runs!
```

---

## 🔢 Number Conversion

### parseInt vs Number

```javascript
// parseInt - parses until invalid character
parseInt("123abc");   // 123
parseInt("10.5");    // 10 (stops at decimal)
parseInt("0x10");    // 16 (hexadecimal!)
parseInt("100px");   // 100

// Number - converts entire value
Number("123");       // 123
Number("123abc");    // NaN
Number("10.5");      // 10.5
Number("0x10");      // 16
Number("100px");     // NaN

// + unary operator
+"123";              // 123
+"10.5";            // 10.5
+"123abc";          // NaN
```

### Radix Parameter

```javascript
// Always specify radix with parseInt!
parseInt("11", 2);   // 3 (binary 11 = decimal 3)
parseInt("FF", 16); // 255 (hex FF = decimal 255)
parseInt("10", 8);   // 8 (octal 10 = decimal 8)

// Without radix - can be unpredictable
parseInt("010");     // 10 (or 8 in older browsers!)
parseInt("010", 10); // 10 (always decimal)
```

### NaN Handling

```javascript
// NaN (Not a Number) - special value
console.log(NaN);            // NaN
console.log(typeof NaN);    // "number"

// Check for NaN
isNaN(NaN);                 // true
isNaN("hello");            // true (attempts conversion)
Number.isNaN(NaN);          // true
Number.isNaN("hello");     // false (doesn't convert)

// Math operations that produce NaN
Math.sqrt(-1);              // NaN
parseInt("abc");           // NaN
undefined + 1;            // NaN
```

---

## 🧮 Boolean Conversion

### Truthy/Falsy in Conditions

```javascript
// if statement uses boolean coercion
if ("hello") { console.log("Runs!"); }   // Runs
if ("") { console.log("Won't run"); }   // Skipped

// Ternary operator
const value = null ?? "default"; // "default"

// Short-circuit evaluation
const name = user && user.name || "Anonymous";

// Nullish coalescing (ES2020)
const config = null;
const debug = config ?? false; // false (not "false"!)
```

### Conversion Functions

```javascript
// Boolean() explicit conversion
Boolean("");        // false
Boolean("hello");   // true
Boolean(0);         // false
Boolean(42);        // true
Boolean(null);      // false
Boolean({});        // true (empty object is truthy!)

// Double negation (often used)
!!"hello";          // true
!!0;                // false

// Alternative: ternary
const isValid = value ? true : false;
```

---

## 📝 String Conversion

### Automatic String Coercion

```javascript
// With + operator
"Result: " + 123;        // "Result: 123"
"Items: " + [1, 2, 3];   // "Items: 1,2,3"
"User: " + { name: "John" }; // "User: [object Object]"

// Template literals (recommended)
`Count: ${123}`;         // "Count: 123"
`Array: ${[1,2,3]}`;     // "Array: 1,2,3"

// Template literal with objects needs JSON.stringify
`User: ${JSON.stringify({name:"John"})}`; // '{"name":"John"}'
```

### Conversion Methods

```javascript
// toString() method
(123).toString();        // "123"
true.toString();        // "true"
[1,2,3].toString();     // "1,2,3"

// String() function
String(123);            // "123"
String(true);          // "true"
String([1,2,3]);        // "1,2,3"
String({});             // "[object Object]"

// JSON.stringify for objects
JSON.stringify({ a: 1 }); // '{"a":1}'
JSON.stringify([1,2]);   // '[1,2]'
JSON.stringify(null);    // 'null'
```

---

## 🎯 Best Practices

### Always Use Strict Equality

```javascript
// ❌ Pitfall: Loose equality with conversion
console.log(0 == "");   // true!
console.log(false == ""); // true!
console.log(null == undefined); // true!

// ✅ Safe: Strict equality
console.log(0 === "");  // false!
console.log(false === ""); // false!
```

### Handle NaN Properly

```javascript
// ❌ Wrong way
if (value + 5 === NaN) { } // Never matches!

// ✅ Correct ways
if (Number.isNaN(value)) { }
if (isNaN(value)) { } // Works but also converts

// Better: Check before operation
if (isFinite(value)) {
    const result = value + 5;
}
```

### Default Values Pattern

```javascript
// ❌ Problem: 0 or "" considered falsy
function setWidth(width) {
    return width || 100; // 0 becomes 100!
}

// ✅ Solution: Nullish coalescing
function setWidth(width) {
    return width ?? 100; // 0 stays 0!
}
```

---

## 🔄 Conversion Table

| Value | Number() | String() | Boolean() |
|-------|----------|----------|-----------|
| `1` | 1 | "1" | true |
| `0` | 0 | "0" | false |
| `"1"` | 1 | "1" | true |
| `""` | 0 | "" | false |
| `"text"` | NaN | "text" | true |
| `null` | 0 | "null" | false |
| `undefined` | NaN | "undefined" | false |
| `[]` | 0 | "" | true |
| `{}` | NaN | "[object Object]" | true |

---

## 🧪 Practice Exercises

### Exercise 1: Predict the Output

```javascript
// What does each evaluate to?
console.log("5" + 1);      // ?
console.log("5" - 1);      // ?
console.log(5 + "1");      // ?
console.log(5 - "1");      // ?
console.log(true + 1);     // ?
console.log(false + 1);    // ?
console.log("" + 5);       // ?
console.log("" - 5);       // ?
```

### Exercise 2: Fix the Bug

```javascript
// This function has bugs - fix them!
function calculateTotal(price, tax = 0.1) {
    // When price is "0", it concatenates!
    return price * (1 + tax);
}

// Fixed version:
function calculateTotalFixed(price, tax = 0.1) {
    const numPrice = Number(price);
    if (Number.isNaN(numPrice)) {
        throw new Error("Invalid price");
    }
    return numPrice * (1 + tax);
}
```

---

## 🔗 Related Topics

- [05_Data_Types_Complete.md](./05_Data_Types_Complete.md)
- [06_Operators_Mastery.md](./06_Operators_Mastery.md)

---

**Next: Learn about [Expressions and Statements](./07_Expressions_and_Statements.md)**