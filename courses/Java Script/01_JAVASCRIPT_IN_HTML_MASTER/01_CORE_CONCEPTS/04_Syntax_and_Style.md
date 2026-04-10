# 📝 JavaScript Syntax and Style

## 📋 Overview

Proper syntax and coding style make your JavaScript code readable, maintainable, and professional. This guide covers essential syntax rules and best practices.

---

## 🔤 Basic Syntax Rules

### Statement Termination

```javascript
// Each statement ends with semicolon (recommended)
let name = "John";
let age = 30;

// Can also be written without semicolons ( ASI )
let city = "New York"
let country = "USA"

// Best practice: Always use semicolons
```

### Case Sensitivity

```javascript
// JavaScript is case-sensitive
let myVariable = "hello";
let MyVariable = "world";

console.log(myVariable); // hello
console.log(MyVariable); // world

// Keywords must be correct case
let IF = 1;  // ❌ Bad practice
let if = 1;  // ❌ Syntax error - 'if' is reserved
```

### Whitespace and Indentation

```javascript
// Good: Proper spacing and indentation
function calculateTotal(price, tax) {
    const subtotal = price * tax;
    const total = subtotal + price;
    return total;
}

// Bad: Hard to read
function calculateTotal(price,tax){
    const subtotal=price*tax;
    const total=subtotal+price;
    return total;
}
```

---

## 💬 Comments

### Single Line Comments

```javascript
// This is a single line comment
let x = 5; // Inline comment

// Calculate area
// Formula: π * r²
const radius = 10;
const area = Math.PI * radius * radius;
```

### Multi-line Comments

```javascript
/*
 * This is a multi-line comment
 * Used for longer explanations
 * or documenting functions
 */

function greet(name) {
    /*
     * Parameters:
     *   name - The user's name
     * Returns:
     *   A greeting string
     */
    return `Hello, ${name}!`;
}
```

### TODO Comments

```javascript
// TODO: Fix this bug
// FIXME: Optimize this function
// NOTE: This is important information
// WARNING: Be careful with this
```

---

## 📦 Code Blocks

```javascript
// Function block
function myFunction() {
    // Code inside braces
    let x = 1;
    console.log(x);
}

// Conditional block
if (condition) {
    // Execute this
}

// Loop block
for (let i = 0; i < 10; i++) {
    console.log(i);
}
```

---

## 🎨 Naming Conventions

### Variables and Functions

```javascript
// ✅ camelCase (recommended)
let userName = "John";
let totalPrice = 99.99;
function calculateTotal() {}

// ❌ Avoid: not consistent
let user_name = "John";
let UserName = "John";
let userName = "John";

// Constants (UPPER_SNAKE_CASE)
const MAX_RETRIES = 3;
const API_BASE_URL = "https://api.example.com";

// Classes (PascalCase)
class UserProfile {}
class ShoppingCart {}
class PaymentGateway {}
```

### Boolean Variables

```javascript
// ✅ Use is/has/can/should prefixes
let isActive = true;
let hasPermission = false;
let canEdit = true;
let shouldUpdate = false;

// ❌ Avoid unclear names
let active = true;
let flag = false;
let check = true;
```

---

## 📊 Code Organization

### Function Organization

```javascript
// ✅ Good: Logical ordering
// 1. Constants and variables
const API_URL = "https://api.example.com";
let cachedData = null;

// 2. Helper functions
function formatDate(date) {
    return new Date(date).toLocaleDateString();
}

// 3. Main functions
async function fetchData() {
    const response = await fetch(API_URL);
    return response.json();
}

// 4. Event handlers
function handleClick(event) {
    console.log("Clicked!", event.target);
}

// 5. Initialization
function init() {
    fetchData();
    setupEventListeners();
}
```

### File Organization

```javascript
// js/app.js - Example structure

// ============ CONSTANTS ============
const CONFIG = {
    apiUrl: "https://api.example.com",
    maxItems: 100,
    timeout: 5000
};

// ============ STATE ============
let state = {
    users: [],
    currentUser: null,
    loading: false
};

// ============ UTILITY FUNCTIONS ============
function formatUserName(name) {
    return name.trim().toLowerCase();
}

// ============ API FUNCTIONS ============
async function fetchUsers() { }

// ============ DOM FUNCTIONS ============
function renderUsers() { }

// ============ EVENT HANDLERS ============
function handleUserClick() { }

// ============ INITIALIZATION ============
function init() { }

// Run on load
document.addEventListener('DOMContentLoaded', init);
```

---

## 🔍 Linting and Formatting

### ESLint Configuration

```json
// .eslintrc.json
{
    "env": {
        "browser": true,
        "es2021": true
    },
    "extends": "eslint:recommended",
    "parserOptions": {
        "ecmaVersion": "latest",
        "sourceType": "module"
    },
    "rules": {
        "indent": ["error", 2],
        "linebreak-style": ["error", "unix"],
        "quotes": ["error", "single"],
        "semi": ["error", "always"],
        "no-unused-vars": "warn",
        "no-console": "off"
    }
}
```

### Prettier Configuration

```json
// .prettierrc
{
    "semi": true,
    "singleQuote": true,
    "tabWidth": 2,
    "trailingComma": "es5",
    "printWidth": 80,
    "arrowParens": "avoid"
}
```

---

## ⚠️ Common Mistakes

### 1. Forgetting Semicolons

```javascript
// ❌ Can cause issues
let a = 1
let b = 2

// ✅ Safe with semicolons
let a = 1;
let b = 2;
```

### 2. Mismatched Brackets

```javascript
// ❌ Error - missing closing brace
function test() {
    if (true) {
        console.log("Hello");
    }
// Missing }

// ✅ Correct
function test() {
    if (true) {
        console.log("Hello");
    }
}
```

### 3. Wrong Quotes

```javascript
// ❌ Mixed quotes cause confusion
let message = "Hello 'World'";
let bad = 'mixed"quotes";

// ✅ Consistent quotes
let message = 'Hello "World"';
let good = 'consistent quotes';
```

---

## 🛠️ VS Code Snippets

### Useful Snippets

```javascript
// log + Tab → console.log();
console.log($1);

// fun + Tab → function name() {}
function name($1) {
    $2
}

// arr + Tab → const arr = [];
const arr = [];

// for + Tab → for (let i = 0; i < length; i++) {}
for (let i = 0; i < ${1:array}.length; i++) {
    $2
}

// afe + Tab → async function name() {}
async function name($1) {
    $2
}
```

---

## 📋 Quick Reference

| Concept | Rule | Example |
|---------|------|---------|
| Variables | camelCase | `userName` |
| Constants | UPPER_CASE | `MAX_SIZE` |
| Classes | PascalCase | `UserProfile` |
| Functions | camelCase | `getUserData()` |
| Files | kebab-case | `user-profile.js` |
| Comments | Use liberally | `// explain why` |
| Indentation | 2 spaces | `  code` |
| Semicolons | Always use | `;` |

---

## 🎯 Best Practices Summary

1. ✅ Use semicolons at the end of statements
2. ✅ Use consistent indentation (2 or 4 spaces)
3. ✅ Use camelCase for variables and functions
4. ✅ Add comments for complex logic
5. ✅ Keep lines under 80 characters
6. ✅ Use meaningful variable names
7. ✅ Organize code logically
8. ✅ Use ESLint and Prettier

---

## 🔗 Related Topics

- [01_Introduction_to_JavaScript.md](./01_Introduction_to_JavaScript.md)
- [04_Variables_Deep_Dive.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/04_Variables_Deep_Dive.md)

---

**Next: Learn about [Variables](./04_Variables_Deep_Dive.md)**