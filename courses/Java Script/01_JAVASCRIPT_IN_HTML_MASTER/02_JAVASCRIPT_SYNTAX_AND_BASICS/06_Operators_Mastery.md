# ➕ Operators Mastery

## 📋 Overview

Operators are symbols that perform operations on values. JavaScript supports various types of operators for arithmetic, comparison, logical operations, and more.

---

## ➖ Arithmetic Operators

### Basic Operators

```javascript
// Addition
let sum = 5 + 3;        // 8
let str = "Hello" + " " + "World"; // "Hello World"

// Subtraction
let diff = 10 - 4;      // 6

// Multiplication
let product = 6 * 7;    // 42

// Division
let quotient = 20 / 4; // 5
let decimal = 10 / 3;  // 3.333...

// Modulo (remainder)
let remainder = 17 % 5; // 2

// Exponentiation (ES2016)
let power = 2 ** 3;    // 8
let square = 5 ** 2;   // 25

// Unary operators
let neg = -5;          // -5
let pos = +"10";       // 10 (converts string to number)
```

### Increment/Decrement

```javascript
let x = 5;

// Post-increment (returns then increments)
console.log(x++); // 5
console.log(x);   // 6

let y = 5;

// Pre-increment (increments then returns)
console.log(++y); // 6
console.log(y);   // 6

// Post-decrement
let a = 5;
console.log(a--); // 5
console.log(a);   // 4

// Pre-decrement
let b = 5;
console.log(--b); // 4
console.log(b);  // 4
```

### Practical Examples

```javascript
// Calculate area
const radius = 5;
const area = Math.PI * radius ** 2;
console.log(area); // 78.54...

// Loop with decrement
for (let i = 10; i > 0; i--) {
    console.log(i);
}

// Pagination calculation
const totalItems = 100;
const itemsPerPage = 10;
const totalPages = Math.ceil(totalItems / itemsPerPage);
console.log(totalPages); // 10
```

---

## 🔍 Comparison Operators

### Equality Operators

```javascript
// Loose equality (type coercion)
console.log(5 == "5");    // true
console.log(true == 1);   // true
console.log(null == undefined); // true

// Strict equality (no type coercion) - RECOMMENDED
console.log(5 === "5");   // false
console.log(true === 1);  // false

// Inequality
console.log(5 != "5");    // false (loose)
console.log(5 !== "5");   // true (strict)

// Objects comparison
const obj1 = { value: 5 };
const obj2 = { value: 5 };
console.log(obj1 === obj2); // false (different references)

const arr1 = [1, 2, 3];
const arr2 = [1, 2, 3];
console.log(arr1 === arr2); // false
```

### Relational Operators

```javascript
// Greater than
console.log(10 > 5);   // true

// Less than
console.log(5 < 10);   // true

// Greater than or equal
console.log(10 >= 10); // true

// Less than or equal
console.log(5 <= 10);  // true

// String comparison (alphabetical)
console.log("apple" < "banana"); // true
console.log("Apple" < "apple");  // false (uppercase < lowercase)

// Special values
console.log(NaN > 0);   // false
console.log(NaN === NaN); // false (NaN not equal to itself!)
console.log(Infinity > 1); // true
console.log(-Infinity < 1); // true
```

### Comparison Best Practices

```javascript
// ✅ Always use strict equality (===)
function validateAge(age) {
    if (age === undefined) {
        return "Age is required";
    }
    if (typeof age !== "number") {
        return "Age must be a number";
    }
    if (age < 0) {
        return "Age cannot be negative";
    }
    return "Valid";
}

// ❌ Avoid loose equality
// What does this evaluate to?
console.log("" == 0);   // true (empty string == 0)
console.log(" " == 0);  // true (whitespace == 0)
console.log(null == 0); // false
console.log(undefined == 0); // false
```

---

## 🔗 Logical Operators

### AND, OR, NOT

```javascript
// AND (&&) - both must be true
console.log(true && true);   // true
console.log(true && false);  // false

// OR (||) - at least one must be true
console.log(true || false);  // true
console.log(false || false); // false

// NOT (!) - inverts boolean
console.log(!true);  // false
console.log(!false); // true

// Combined
console.log(true && true || false); // true
console.log(false || true && false); // false
```

### Short-Circuit Evaluation

```javascript
// AND short-circuit - stops at first falsy
const andResult = false && console.log("won't run");
console.log(andResult); // false

// OR short-circuit - stops at first truthy
const orResult = true || console.log("won't run");
console.log(orResult); // true

// Practical use
function getUserName(user) {
    return user && user.name || "Anonymous";
}

console.log(getUserName({ name: "John" })); // "John"
console.log(getUserName(null)); // "Anonymous"

// Default values
const config = null;
const dbHost = config && config.database && config.database.host || "localhost";
console.log(dbHost); // "localhost"
```

### Nullish Coalescing (ES2020)

```javascript
// || vs ?? 
console.log(0 || "default");   // "default" (0 is falsy)
console.log(0 ?? "default");    // 0 (0 is not null/undefined)

console.log("" || "default");   // "default" ("" is falsy)
console.log("" ?? "default");   // "" ("" is not null/undefined)

console.log(null || "default"); // "default"
console.log(null ?? "default"); // "default"

console.log(undefined || "default"); // "default"
console.log(undefined ?? "default"); // "default"

// Practical use
let count = 0;
console.log(count || 1);  // 1 (0 is falsy)
console.log(count ?? 1);  // 0 (0 is valid!)
```

---

## 🧮 Assignment Operators

### Basic Assignment

```javascript
let x = 5;

// Compound assignment
x += 3;  // x = x + 3  -> 8
x -= 2;  // x = x - 2  -> 6
x *= 2;  // x = x * 2  -> 12
x /= 3;  // x = x / 3  -> 4
x %= 3;  // x = x % 3  -> 1
x **= 2; // x = x ** 2 -> 1

// String concatenation
let str = "Hello";
str += " World";
console.log(str); // "Hello World"
```

### Destructuring Assignment

```javascript
// Array destructuring
const [a, b, c] = [1, 2, 3];
console.log(a, b, c); // 1 2 3

// Skip values
const [first, , third] = [1, 2, 3];
console.log(first, third); // 1 3

// Rest pattern
const [head, ...tail] = [1, 2, 3, 4];
console.log(head); // 1
console.log(tail); // [2, 3, 4]

// Default values
const [x = 1, y = 2] = [undefined, 5];
console.log(x, y); // 1 5

// Object destructuring
const { name, age } = { name: "John", age: 30 };
console.log(name, age); // John 30

// Renamed properties
const { name: userName } = { name: "John" };
console.log(userName); // John

// Default values
const { role = "user" } = {};
console.log(role); // "user"
```

---

## ❓ Ternary Operator

### Syntax

```javascript
condition ? valueIfTrue : valueIfFalse
```

### Basic Usage

```javascript
const age = 20;
const status = age >= 18 ? "adult" : "minor";
console.log(status); // "adult"

// Nested ternary (avoid!)
const score = 85;
const grade = score >= 90 ? "A" : 
              score >= 80 ? "B" : 
              score >= 70 ? "C" : "F";
console.log(grade); // "B"
```

### Practical Examples

```javascript
// Conditional rendering
function getButtonClass(isPrimary) {
    return isPrimary ? "btn-primary" : "btn-secondary";
}

// Early return
function getShippingCost(order) {
    if (!order) return 0;
    return order.total > 100 ? 0 : 9.99;
}

// Value selection
const user = { name: "John", isAdmin: false };
const displayName = user.nickname ?? user.name ?? "Anonymous";
console.log(displayName); // "John"
```

---

## 🔢 Bitwise Operators (Advanced)

### Basic Bitwise

```javascript
// Bitwise AND
console.log(5 & 3); // 1 (0101 & 0011 = 0001)

// Bitwise OR
console.log(5 | 3); // 7 (0101 | 0011 = 0111)

// Bitwise XOR
console.log(5 ^ 3); // 6 (0101 ^ 0011 = 0110)

// Bitwise NOT
console.log(~5);   // -6

// Left shift
console.log(5 << 1); // 10

// Right shift
console.log(10 >> 1); // 5

// Unsigned right shift
console.log(-10 >>> 1); // Large number
```

### Use Cases

```javascript
// Check if odd/even
function isEven(n) {
    return (n & 1) === 0;
}
console.log(isEven(4)); // true
console.log(isEven(5)); // false

// Fast multiplication/division by 2
console.log(10 << 1); // 20 (multiply by 2)
console.log(20 >> 1); // 10 (divide by 2)

// Check bit
function hasBit(num, bitPosition) {
    return (num & (1 << bitPosition)) !== 0;
}
console.log(hasBit(5, 0)); // true (5 = 0101)
console.log(hasBit(5, 2)); // true (5 = 0101)
```

---

## 📊 Operator Precedence

### Order (High to Low)

```javascript
// 1. Parentheses
console.log((2 + 3) * 4); // 20

// 2. Member access, call
console.log(obj.property);
console.log(arr[0]);

// 3. new (with arguments)
new Object()

// 4. Unary operators
++x, --x, !x, typeof

// 5. Arithmetic
**, *, /, %, +, -

// 6. Comparison
<, <=, >, >=, ==, !=, ===, !==

// 7. Bitwise
&, ^, |

// 8. Logical
&&, ??

// 9. Ternary
? :

// 10. Assignment
=, +=, -=, etc.

// 11. Comma
a, b
```

### Examples

```javascript
// What is the result?
console.log(2 + 3 * 4);  // 14 (not 20!)
console.log((2 + 3) * 4); // 20

console.log(true || false && false); // true (&& before ||)
console.log((true || false) && false); // false

console.log(1 || 2 && 3); // 1 (&& before ||)
```

---

## 🎯 Real-World Application

### Form Validation

```javascript
function validateForm(data) {
    const errors = [];
    
    // Required check
    if (!data.username || data.username.length < 3) {
        errors.push("Username must be at least 3 characters");
    }
    
    // Email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(data.email)) {
        errors.push("Invalid email format");
    }
    
    // Age validation
    if (typeof data.age !== "number" || data.age < 0 || data.age > 150) {
        errors.push("Age must be between 0 and 150");
    }
    
    // Password strength
    const hasUpper = /[A-Z]/.test(data.password);
    const hasLower = /[a-z]/.test(data.password);
    const hasNumber = /[0-9]/.test(data.password);
    const hasSpecial = /[!@#$%^&*]/.test(data.password);
    
    const strength = [hasUpper, hasLower, hasNumber, hasSpecial]
        .filter(Boolean).length;
    
    if (strength < 3 || data.password.length < 8) {
        errors.push("Password must be at least 8 characters with 3 of: uppercase, lowercase, number, special");
    }
    
    return {
        isValid: errors.length === 0,
        errors
    };
}

// Test
const result = validateForm({
    username: "jo",
    email: "invalid-email",
    age: -5,
    password: "weak"
});
console.log(result);
```

---

## 🎯 Practice Exercises

### Exercise 1: Grade Calculator

```javascript
function calculateGrade(score) {
    // Return letter grade based on score
    // A: 90+, B: 80-89, C: 70-79, D: 60-69, F: below 60
    
    if (typeof score !== "number" || score < 0 || score > 100) {
        return "Invalid score";
    }
    
    if (score >= 90) return "A";
    if (score >= 80) return "B";
    if (score >= 70) return "C";
    if (score >= 60) return "D";
    return "F";
}

console.log(calculateGrade(95)); // A
console.log(calculateGrade(82));  // B
```

### Exercise 2: Logical Operator Challenge

```javascript
// What will each log output?
console.log(true && 1);        // ?
console.log(false || 0);       // ?
console.log(null ?? "default"); // ?
console.log("" || "fallback"); // ?
```

---

## 📊 Quick Reference

| Operator | Type | Example | Result |
|----------|------|---------|--------|
| `+` | Arithmetic | 5 + 3 | 8 |
| `-` | Arithmetic | 5 - 3 | 2 |
| `*` | Arithmetic | 5 * 3 | 15 |
| `/` | Arithmetic | 15 / 3 | 5 |
| `%` | Arithmetic | 17 % 5 | 2 |
| `**` | Arithmetic | 2 ** 3 | 8 |
| `===` | Comparison | 5 === 5 | true |
| `!==` | Comparison | 5 !== 3 | true |
| `&&` | Logical | true && true | true |
| `\|\|` | Logical | false \|\| true | true |
| `??` | Nullish | null ?? "x" | "x" |
| `? :` | Ternary | true ? 1 : 0 | 1 |

---

## 🔗 Related Topics

- [04_Variables_Deep_Dive.md](./04_Variables_Deep_Dive.md)
- [06_Type_Coercion.md](./06_Type_Coercion.md)

---

**Next: Learn about [Type Coercion](./06_Type_Coercion.md)**