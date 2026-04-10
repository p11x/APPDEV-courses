# 🔢 Numbers in JavaScript

## 📋 Overview

JavaScript has a single numeric type called **Number**, which represents both integers and floating-point numbers. Understanding how JavaScript handles numbers is crucial for mathematical operations.

---

## 🔢 Number Types

### Integer vs Float

```javascript
// Integers
let count = 42;
let year = 2024;

// Floating-point numbers
let price = 19.99;
let pi = 3.14159;

// Negative numbers
let temperature = -10.5;
let debt = -500;

// Exponential notation (very large or small numbers)
let billion = 1e9;    // 1000000000
let tiny = 1e-6;      // 0.000001

// Special numbers
Infinity;             // Positive infinity
-Infinity;            // Negative infinity
NaN;                  // Not a Number
```

### Safe Integer Range

```javascript
// JavaScript can safely represent integers between:
console.log(Number.MIN_SAFE_INTEGER); // -9007199254740991
console.log(Number.MAX_SAFE_INTEGER); // 9007199254740991

// Beyond this range, precision is lost!
console.log(9007199254740993); // 9007199254740992 (wrong!)

// For large integers, use BigInt
const bigNumber = 9007199254740993n;
```

---

## 🧮 Math Object

### Basic Math Operations

```javascript
// Rounding
Math.round(4.5);   // 5 (rounds to nearest)
Math.floor(4.9);    // 4 (rounds down)
Math.ceil(4.1);     // 5 (rounds up)
Math.trunc(4.9);    // 4 (removes decimal)

// Absolute value
Math.abs(-5);       // 5
Math.abs(5);       // 5

// Power and square root
Math.pow(2, 3);    // 8
Math.sqrt(16);     // 4
Math.cbrt(8);      // 2 (cube root)

// Min/Max
Math.min(1, 2, 3); // 1
Math.max(1, 2, 3); // 3

// Random number (0 to 1)
Math.random();     // 0.123456789
```

### Practical Examples

```javascript
// Random integer between min and max
function randomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
console.log(randomInt(1, 10)); // Random 1-10

// Round to specific decimal places
function roundTo(num, decimals) {
    return Number(Math.round(num + 'e' + decimals) + 'e-' + decimals);
}
console.log(roundTo(3.14159, 2)); // 3.14

// Calculate distance between two points
function distance(x1, y1, x2, y2) {
    return Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2));
}
```

---

## 🔢 Number Methods

### Converting Strings to Numbers

```javascript
// Number() - converts to number or NaN
Number("42");       // 42
Number("3.14");    // 3.14
Number("hello");   // NaN
Number("");        // 0
Number(true);      // 1

// parseInt() - integer from string
parseInt("42");    // 42
parseInt("3.14");  // 3
parseInt("100px"); // 100
parseInt("0x10");  // 16 (hex!)
parseInt("10", 10);// 10 (decimal)

// parseFloat() - decimal numbers
parseFloat("3.14");    // 3.14
parseFloat("3.14abc"); // 3.14
parseFloat("0.5");     // 0.5
```

### Checking Numbers

```javascript
// isNaN - is it not a number?
isNaN(NaN);         // true
isNaN("hello");    // true (converts first)
isNaN(42);         // false

// Number.isNaN - better (doesn't convert)
Number.isNaN(NaN);          // true
Number.isNaN("hello");      // false

// isFinite - is it a regular number?
isFinite(42);        // true
isFinite(Infinity);  // false
isFinite(NaN);       // false

// Number.isFinite - better
Number.isFinite(42);      // true
Number.isFinite(Infinity);// false

// isInteger
Number.isInteger(42);     // true
Number.isInteger(3.14);   // false
```

### Number Formatting

```javascript
// toFixed - decimal places
let num = 3.14159;
num.toFixed(2);     // "3.14"
num.toFixed(0);     // "3"
(1).toFixed(2);    // "1.00"

// toPrecision - significant digits
(123.456).toPrecision(3); // "123"
(0.00123).toPrecision(3); // "0.00123"

// toString with radix
(255).toString();      // "255"
(255).toString(16);   // "ff"
(255).toString(2);    // "11111111"
```

---

## ⚠️ Precision Problems

### Floating-Point Arithmetic

```javascript
// Classic problem
0.1 + 0.2;          // 0.30000000000000004!
0.7 + 0.1;         // 0.7999999999999999

// Solutions
// 1. Use integers (multiply by 100 for cents)
const price1 = 0.10 * 100;  // 10
const price2 = 0.20 * 100;  // 20
const total = (price1 + price2) / 100; // 0.30

// 2. Use toFixed
(0.1 + 0.2).toFixed(2);  // "0.30"

// 3. Use Number.EPSILON for comparison
function areEqual(a, b) {
    return Math.abs(a - b) < Number.EPSILON;
}
areEqual(0.1 + 0.2, 0.3); // true
```

### Large Number Display

```javascript
// Very large numbers
let large = 1234567890;
large.toExponential();  // "1.23456789e+9"
large.toLocaleString(); // "1,234,567,890"

// Currency formatting
let price = 1234.56;
price.toLocaleString('en-US', { 
    style: 'currency', 
    currency: 'USD' 
}); // "$1,234.56"
```

---

## 🎯 Real-World Applications

### Currency Calculator

```javascript
class CurrencyCalculator {
    constructor(currency = 'USD') {
        this.currency = currency;
    }
    
    // Round to 2 decimal places for money
    roundToTwo(num) {
        return Math.round(num * 100) / 100;
    }
    
    // Add prices without precision errors
    addPrices(...prices) {
        // Convert to cents, add, convert back
        const cents = prices.map(p => Math.round(p * 100));
        const totalCents = cents.reduce((sum, c) => sum + c, 0);
        return totalCents / 100;
    }
    
    format(amount) {
        return amount.toLocaleString('en-US', {
            style: 'currency',
            currency: this.currency
        });
    }
}

const calc = new CurrencyCalculator('USD');
console.log(calc.addPrices(0.10, 0.20)); // 0.30 (correct!)
console.log(calc.format(1234.56)); // "$1,234.56"
```

### Statistics Calculator

```javascript
function calculateStats(numbers) {
    const n = numbers.length;
    if (n === 0) return null;
    
    // Sum
    const sum = numbers.reduce((a, b) => a + b, 0);
    
    // Mean
    const mean = sum / n;
    
    // Standard deviation
    const squaredDiffs = numbers.map(x => Math.pow(x - mean, 2));
    const variance = squaredDiffs.reduce((a, b) => a + b, 0) / n;
    const stdDev = Math.sqrt(variance);
    
    // Min/Max
    const min = Math.min(...numbers);
    const max = Math.max(...numbers);
    
    return { sum, mean, stdDev, min, max };
}

const stats = calculateStats([1, 2, 3, 4, 5]);
console.log(stats);
// { sum: 15, mean: 3, stdDev: 1.41..., min: 1, max: 5 }
```

---

## 🧪 Practice Exercises

### Exercise 1: Temperature Converter

```javascript
function celsiusToFahrenheit(celsius) {
    return (celsius * 9/5) + 32;
}

function fahrenheitToCelsius(fahrenheit) {
    return (fahrenheit - 32) * 5/9;
}

console.log(celsiusToFahrenheit(0));    // 32
console.log(celsiusToFahrenheit(100));  // 212
console.log(fahrenheitToCelsius(32));  // 0
```

### Exercise 2: Percentage Calculator

```javascript
function percentage(value, total) {
    return (value / total) * 100;
}

function calculatePortion(percentage, total) {
    return (percentage / 100) * total;
}

console.log(percentage(25, 100)); // 25
console.log(percentage(15, 200)); // 7.5
console.log(calculatePortion(25, 100)); // 25
```

---

## 📊 Quick Reference

| Method | Example | Result |
|--------|---------|--------|
| `Math.round()` | `Math.round(4.5)` | 5 |
| `Math.floor()` | `Math.floor(4.9)` | 4 |
| `Math.ceil()` | `Math.ceil(4.1)` | 5 |
| `Math.abs()` | `Math.abs(-5)` | 5 |
| `Math.pow()` | `Math.pow(2, 3)` | 8 |
| `Math.sqrt()` | `Math.sqrt(16)` | 4 |
| `Math.random()` | `Math.random()` | 0-1 |
| `Number()` | `Number("42")` | 42 |
| `parseInt()` | `parseInt("42")` | 42 |
| `toFixed()` | `(3.14).toFixed(1)` | "3.1" |

---

## 🔗 Related Topics

- [05_Data_Types_Complete.md](./05_Data_Types_Complete.md)
- [07_Type_Coercion_and_Conversion.md](./07_Type_Coercion_and_Conversion.md)

---

**Next: Learn about [Strings Fundamentals](./10_Strings_Fundamentals.md)**