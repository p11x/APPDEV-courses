# ⚠️ Understanding NaN

## 📋 Overview

`NaN` (Not a Number) is a special value in JavaScript that represents an invalid or undefined numerical result. Understanding NaN is crucial for proper number handling and avoiding bugs.

---

## 🔢 What is NaN?

### When NaN Occurs

```javascript
// Invalid mathematical operations
Math.sqrt(-1);          // NaN (square root of negative)
Math.log(-1);           // NaN (logarithm of negative)
0 / 0;                  // NaN
Infinity - Infinity;    // NaN
Infinity * 0;           // NaN

// Invalid number conversions
parseInt("hello");     // NaN
Number("abc");         // NaN
parseFloat("xyz");     // NaN

// Invalid arithmetic
"hello" * 5;           // NaN
undefined + 1;         // NaN
```

### NaN Properties

```javascript
// NaN is a primitive value
console.log(typeof NaN); // "number" (confusing!)

// NaN is not equal to itself
console.log(NaN === NaN); // false!
```

---

## 🔍 Checking for NaN

### isNaN() - Legacy Method

```javascript
// isNaN() attempts conversion before checking
isNaN(NaN);           // true
isNaN("hello");       // true (converts to NaN)
isNaN(100);           // false
isNaN("100");         // false (converts to 100)

// Problem: False positives
isNaN("hello");      // true (but "hello" isn't NaN!)
isNaN("100px");       // true
```

### Number.isNaN() - Modern Method (Recommended)

```javascript
// Does NOT convert - only true for actual NaN
Number.isNaN(NaN);           // true
Number.isNaN("hello");      // false (string!)
Number.isNaN(100);           // false
Number.isNaN("100");        // false

// Safe for all types
Number.isNaN(parseInt("abc")); // true
Number.isNaN(Math.sqrt(-1));   // true
```

---

## 🛠️ Handling NaN

### Before Math Operations

```javascript
function safeSqrt(value) {
    if (typeof value !== 'number') return NaN;
    if (value < 0) return NaN;
    return Math.sqrt(value);
}

console.log(safeSqrt(16));     // 4
console.log(safeSqrt(-4));     // NaN
console.log(safeSqrt("hello"));// NaN
```

### Checking Results

```javascript
// Validate result
let result = parseInt("hello");

if (Number.isNaN(result)) {
    console.log("Invalid number");
} else {
    console.log("Valid:", result);
}

// Alternative: isFinite (excludes NaN and Infinity)
let num = 0 / 0;
console.log(isFinite(num));   // false
console.log(Number.isFinite(num)); // false
```

### NaN in Calculations

```javascript
// NaN propagates through calculations
let a = NaN + 5;     // NaN
let b = NaN * 10;    // NaN
let c = NaN - 100;   // NaN

// Can detect with Number.isNaN()
if (Number.isNaN(a)) {
    console.log("Calculation resulted in NaN");
}
```

---

## 🎯 Best Practices

### Do

```javascript
// ✅ Use Number.isNaN()
if (Number.isNaN(value)) {
    // Handle NaN
}

// ✅ Check before parsing
function parseNumber(value) {
    const num = Number(value);
    return Number.isNaN(num) ? null : num;
}

// ✅ Use isFinite for valid numbers
if (isFinite(value)) {
    // Valid number (not NaN, Infinity, -Infinity)
}
```

### Don't

```javascript
// ❌ Don't use old isNaN (gives false positives)
isNaN("hello"); // true - but it's not NaN!

// ❌ Don't compare with NaN
value === NaN; // ALWAYS false!

// ❌ Don't assume NaN is falsy in a useful way
if (NaN) { } // This doesn't help detect NaN
```

---

## 🔗 Related Topics

- [09_Numbers_in_Depth.md](./09_Numbers_in_Depth.md)
- [07_Type_Coercion_and_Conversion.md](./07_Type_Coercion_and_Conversion.md)

---

**Next: Learn about [Conditional Statements](./03_CONTROL_FLOW/01_Conditional_Statements_Intro.md)**