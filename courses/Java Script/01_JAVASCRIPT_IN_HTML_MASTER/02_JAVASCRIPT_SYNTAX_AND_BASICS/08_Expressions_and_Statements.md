# 📝 Expressions and Statements

## 📋 Overview

Understanding the difference between expressions and statements is fundamental to JavaScript. This guide covers how JavaScript evaluates code and how to structure your programs.

---

## 🔤 What is an Expression?

An **expression** is any valid unit of code that produces a value.

### Types of Expressions

```javascript
// Arithmetic expressions
2 + 3                 // 5
10 * 5 - 2            // 48

// String expressions
"Hello" + " " + "World"  // "Hello World"

// Logical expressions
true && false         // false
5 > 3 || 2 < 1       // true

// Assignment expressions (value is the assigned value)
let x = 5            // 5 (the value 5)

// Function call expressions
Math.max(1, 2, 3)    // 3
"hello".toUpperCase() // "HELLO"

// Object/Array expressions
{ name: "John" }    // { name: "John" }
[1, 2, 3]           // [1, 2, 3]

// Ternary expression
let age = 20;
let status = age >= 18 ? "adult" : "minor"  // "adult"
```

### Expressions in Context

```javascript
// As function arguments
console.log(2 + 2);           // 4 is passed to console.log
alert("Hello, " + "World");  // "Hello, World" passed

// In array
let nums = [1 + 1, 2 + 2, 3 + 3]; // [2, 4, 6]

// In object
let config = {
    max: 10 + 5,
    enabled: true && false
};

// In function return
function add(a, b) {
    return a + b;  // Returns the expression result
}
```

---

## 📋 What is a Statement?

A **statement** is an instruction that performs an action.

### Types of Statements

```javascript
// Declaration statements
let name = "John";     // Variable declaration
const PI = 3.14;       // Constant declaration
function greet() {}    // Function declaration
class Person {}        // Class declaration

// Expression statements (expression followed by ;)
name = "Jane";         // Assignment statement
counter++;             // Increment statement
console.log("Hello");  // Function call statement

// Control flow statements
if (condition) { }    // If statement
for (let i = 0; i < 10; i++) { }  // For loop
while (condition) { } // While loop
switch (value) { }    // Switch statement

// Block statement (groups statements)
{
    let x = 1;
    let y = 2;
}

// Empty statement (do nothing)
;   // Useful in loops sometimes

// Labeled statements
loop: for(;;) { break loop; }
```

---

## 🔄 Statements vs Expressions

### Key Differences

```javascript
// Expression: Produces a value
let result = 5 + 3;  // 5 + 3 is expression, result gets value 8
console.log(result); // Can use expression anywhere value is needed

// Statement: Performs action, doesn't produce value
if (result > 5) {    // if is a statement
    console.log("Big");
}
// Cannot do: let x = if (...) - error!

// Most statements end with semicolon
let y = 5;   // Statement

// But block statements don't need semicolons
if (true) {
    let z = 10;  // No semicolon needed after }
```

### Expression as Statements

```javascript
// Any expression can be used as a statement
5 + 3;              // Does nothing useful, but valid
"Hello";            // Does nothing (string is created, then discarded)
myFunction();       // Call function (side effect)

// Useful expression statements
counter++;          // Increment
arr.push(item);     // Add to array
console.log("msg"); // Output
```

---

## 🎯 Operator Precedence

### Precedence Order (High to Low)

```javascript
// 1. Grouping
(2 + 3) * 4  // 20

// 2. Member access
obj.property
arr[0]

// 3. new (with arguments)
new Date()

// 4. Function call
func()

// 5. Logical NOT
!true

// 6. Unary operators
typeof x
+ x
- x

// 7. Exponentiation
2 ** 3

// 8. Multiplication/Division
* / %

// 9. Addition/Subtraction
+ -

// 10. Comparison
< <= > >=
== != ===

// 11. Bitwise AND
&

// 12. Bitwise OR
|

// 13. Logical AND
&&

// 14. Logical OR
||

// 15. Nullish coalescing
??

// 16. Conditional (ternary)
? :

// 17. Assignment
= += -= *= /= %= **=

// 18. Comma
a, b
```

### Examples

```javascript
// Without parens - unexpected results?
let a = 2 + 3 * 4;  // 14 (not 20!)
let b = (2 + 3) * 4; // 20

// Chain of operations
let c = 10 > 5 && 3 < 1 || true;
// Evaluates: (10 > 5) && (3 < 1) || true
// true && false || true
// false || true
// true
```

---

## 🔗 Chaining and Grouping

### Using Parentheses

```javascript
// Grouping for clarity
const result = (a + b) * (c - d);

// Complex logical expression
const isValid = ((user.age >= 18) && 
                 (user.hasLicense)) || 
                 (user.isInstructor);

// Mathematical precedence
const formula = (a + b) * (c + d) / (e - f);
```

### Comma Operator

```javascript
// Execute multiple expressions, return last value
let x = (1, 2, 3);  // x = 3

// In loops - multiple variables
for (let i = 0, j = 10; i < j; i++, j--) {
    console.log(i, j);
}

// In function returns
function getCoords() {
    return (x = 1, y = 2); // Returns 2
}
```

---

## 📊 Statement Evaluation

### How JavaScript Executes

```javascript
// Step by step evaluation
let a = 5;
let b = 10;
let c = a + b;    // Evaluate a + b first (15), then assign

// Short-circuit evaluation
const config = null;
const setting = config && config.setting || "default";
// 1. config is null (falsy)
// 2. Short-circuit returns "default" without evaluating config.setting
```

### Function Expression vs Declaration

```javascript
// Function declaration (statement)
// Can be called before definition due to hoisting
sayHello();  // "Hello!"
function sayHello() {
    console.log("Hello!");
}

// Function expression (expression)
// Cannot be called before definition
sayHi();  // Error!
const sayHi = function() {
    console.log("Hi!");
};
```

---

## 🎯 Best Practices

### Clear Expression Writing

```javascript
// ❌ Hard to read
let result = a * b + c / d - e % f;

// ✅ Easy to read
let result = ((a * b) + (c / d)) - (e % f);

// ✅ Even better with variables
const totalItems = a * b;
const discount = c / d;
const remainder = e % f;
const finalResult = totalItems + discount - remainder;
```

### Statement Organization

```javascript
// Group related statements
function processOrder(order) {
    // 1. Validation (statements)
    if (!order) return null;
    if (!order.items || order.items.length === 0) return null;
    
    // 2. Calculation (expressions)
    const subtotal = calculateSubtotal(order.items);
    const tax = calculateTax(subtotal);
    const total = subtotal + tax;
    
    // 3. Return (expression)
    return { subtotal, tax, total };
}
```

---

## 🧪 Practice Exercises

### Exercise 1: Identify Expression vs Statement

```javascript
// Which are expressions? Which are statements?

5 + 5                      // Expression
let x = 10                 // Statement
x + 5                      // Expression
if (x > 5) { }            // Statement
x > 5                      // Expression
"hello".length             // Expression
console.log(x)             // Statement (function call as statement)
```

### Exercise 2: Write Equivalent Code

```javascript
// Rewrite using different operators
// Original:
if (age >= 18) {
    canVote = true;
} else {
    canVote = false;
}

// Using ternary (expression):
const canVote = age >= 18 ? true : false;

// Using AND/OR:
const canVote = age >= 18 || false;

// Best - use comparison directly:
const canVote = age >= 18;
```

---

## 📊 Quick Reference

| Category | Example | Produces Value? |
|----------|---------|-----------------|
| Arithmetic | `5 + 3` | ✅ Yes (8) |
| String | `"a" + "b"` | ✅ Yes ("ab") |
| Comparison | `5 > 3` | ✅ Yes (true) |
| Logical | `true && false` | ✅ Yes (false) |
| Assignment | `x = 5` | ✅ Yes (5) |
| Variable decl | `let x = 5` | ❌ No |
| If statement | `if (cond) {}` | ❌ No |
| Loop | `for (;;) {}` | ❌ No |
| Function decl | `function(){}` | ❌ No |

---

## 🔗 Related Topics

- [04_Variables_Deep_Dive.md](./04_Variables_Deep_Dive.md)
- [06_Operators_Mastery.md](./06_Operators_Mastery.md)

---

**Next: Learn about [Boolean Logic](./08_Boolean_Logic.md)**