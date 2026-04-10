# 📝 Strings Fundamentals

## 📋 Overview

Strings are sequences of characters used to represent text. JavaScript provides powerful methods for working with strings, from basic operations to complex pattern matching.

---

## 🔤 Creating Strings

### String Literals

```javascript
// Single quotes
let name = 'John';

// Double quotes
let greeting = "Hello World";

// Template literals (backticks) - ES6
let message = `Welcome, ${name}!`;

// Multi-line strings (only with backticks)
let multiline = `
    This is line 1
    This is line 2
    This is line 3
`;
```

### Special Characters

```javascript
// Escape sequences
let str = "He said \"Hello\"";  // He said "Hello"
let path = "C:\\Users\\John";   // C:\Users\John
let newLine = "Line1\nLine2";   // New line
let tab = "Col1\tCol2";         // Tab

// Common escapes
\'  // Single quote
\"  // Double quote
\\  // Backslash
\n  // New line
\r  // Carriage return
\t  // Tab
\b  // Backspace
```

---

## 📏 String Properties

### length Property

```javascript
let text = "Hello World";
console.log(text.length); // 11

// Empty string
console.log("".length); // 0

// Space counts as character
console.log(" ".length); // 1
```

### Accessing Characters

```javascript
let str = "Hello";

// Bracket notation (read only)
console.log(str[0]);  // H
console.log(str[4]);  // o
console.log(str[-1]); // undefined (no negative indexing)

// charAt method
console.log(str.charAt(0)); // H
console.log(str.charAt(100)); // "" (empty string)

// Modern method - at() (ES2022)
console.log(str.at(0));   // H
console.log(str.at(-1));  // o (negative counts from end!)
```

---

## ✂️ String Methods

### Case Conversion

```javascript
let text = "Hello World";

text.toUpperCase();  // "HELLO WORLD"
text.toLowerCase();  // "hello world"

// Capitalize first letter
function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}
capitalize("hello"); // "Hello"
```

### Trimming Whitespace

```javascript
let padded = "   Hello   ";

p.trim();       // "Hello" (removes both ends)
p.trimStart();  // "Hello   " (left side only)
p.trimEnd();    // "   Hello" (right side only)

// Use case: form input
let input = "  john@example.com  ";
let cleanEmail = input.trim().toLowerCase();
```

### Searching for Substrings

```javascript
let text = "Hello World, Welcome!";

// indexOf - first occurrence
text.indexOf("World");     // 6
text.indexOf("world");    // -1 (case sensitive)
text.indexOf("o");        // 4

// lastIndexOf - last occurrence  
text.lastIndexOf("o");    // 19

// includes (ES6) - returns boolean
text.includes("World");   // true
text.includes("world");  // false

// startsWith / endsWith
text.startsWith("Hello"); // true
text.endsWith("!");       // true
```

### Extracting Substrings

```javascript
let str = "Hello World";

// slice(start, end) - most flexible
str.slice(0, 5);    // "Hello"
str.slice(6);       // "World"
str.slice(-5);     // "World" (from end)
str.slice(-6, -1); // "World" (end not included)

// substring(start, length) - similar but no negative
str.substring(0, 5);  // "Hello"
str.substring(6);    // "World"

// substr(start, length) - deprecated, avoid
str.substr(0, 5);   // "Hello" (deprecated!)
```

### Replacing Text

```javascript
let text = "Hello World";

// replace - replaces first match
text.replace("World", "JavaScript"); // "Hello JavaScript"

// replace with regex
text.replace(/o/g, "X"); // "HellX WXrld"

// replaceAll (ES2021)
text.replaceAll("o", "X"); // "HellX WXrld"

// Multiple replacements
text.replace(/l/g, "L").replace(/o/g, "O"); // "HeLLO WOrLd"
```

### Splitting and Joining

```javascript
let csv = "apple,banana,cherry";

// split - string to array
csv.split(",");        // ["apple", "banana", "cherry"]
csv.split("");        // ["a","p","p","l","e"...]

// Split by regex
"Hello World".split(/o/); // ["Hell", " W", "rld"]

// join - array to string
["a", "b", "c"].join("-"); // "a-b-c"

// Practical: CSV handling
function parseCSV(csv) {
    return csv.split(",").map(s => s.trim());
}
```

---

## 🔗 Concatenation

### Methods

```javascript
// Using + operator
let first = "Hello";
let second = "World";
let combined = first + " " + second; // "Hello World"

// Using concat
first.concat(" ", second); // "Hello World"

// Template literals (recommended)
let message = `${first} ${second}`; // "Hello World"

// Array join
[first, second].join(" "); // "Hello World"

// Repeat string
"Ha".repeat(3); // "HaHaHa"
```

---

## 🔍 Character Operations

### Checking Character Types

```javascript
let char = "A";

// Unicode-based checks
"A".charCodeAt(0); // 65
"a".charCodeAt(0); // 97

// ES6 methods
"abc".includes("a"); // true
"abc".startsWith("a"); // true
"abc".endsWith("c"); // true

// Check if all letters
"abc".match(/[a-z]/i); // not ideal

// Use RegExp
function isLetter(char) {
    return /[a-zA-Z]/.test(char);
}
```

---

## 🎯 Real-World Examples

### Form Validation

```javascript
function validateEmail(email) {
    // Basic email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email.trim());
}

function validateUsername(username) {
    // 3-20 characters, alphanumeric and underscore
    const usernameRegex = /^[a-zA-Z0-9_]{3,20}$/;
    return usernameRegex.test(username);
}

function sanitizeInput(input) {
    // Remove HTML tags
    return input.replace(/<[^>]*>/g, "");
}

console.log(validateEmail("john@example.com")); // true
console.log(validateUsername("john_doe")); // true
```

### Text Processing

```javascript
function wordCount(text) {
    return text.trim().split(/\s+/).filter(w => w.length > 0).length;
}

function truncate(text, maxLength) {
    if (text.length <= maxLength) return text;
    return text.slice(0, maxLength - 3) + "...";
}

function slugify(text) {
    return text
        .toLowerCase()
        .trim()
        .replace(/[^\w\s-]/g, "")
        .replace(/[\s_-]+/g, "-")
        .replace(/^-+|-+$/g, "");
}

console.log(wordCount("Hello World")); // 2
console.log(truncate("Hello World", 8)); // "Hello..."
console.log(slugify("Hello World!")); // "hello-world"
```

---

## 📊 String Method Quick Reference

| Method | Description | Example |
|--------|-------------|---------|
| `length` | Character count | `"hello".length` → 5 |
| `charAt(n)` | Character at position | `"hello".charAt(1)` → "e" |
| `toUpperCase()` | Uppercase | `"hello".toUpperCase()` → "HELLO" |
| `toLowerCase()` | Lowercase | `"HELLO".toLowerCase()` → "hello" |
| `trim()` | Remove whitespace | `" hi ".trim()` → "hi" |
| `indexOf(str)` | Find position | `"hello".indexOf("l")` → 2 |
| `slice(start,end)` | Extract substring | `"hello".slice(1,3)` → "el" |
| `split(sep)` | To array | `"a,b".split(",")` → ["a","b"] |
| `replace(old,new)` | Replace text | `"hi".replace("i","o")` → "ho" |
| `concat(str)` | Combine strings | `"hi".concat(" there")` → "hi there" |

---

## 🔗 Related Topics

- [11_Template_Literals.md](./11_Template_Literals.md)
- [05_Data_Types_Complete.md](./05_Data_Types_Complete.md)

---

**Next: Learn about [Template Literals](./11_Template_Literals.md)**