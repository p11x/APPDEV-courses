# 📝 Template Literals

## 📋 Overview

Template literals are string literals that allow embedded expressions. They make string manipulation more readable and powerful compared to traditional string concatenation.

---

## 🎯 Basic Syntax

### Using Backticks

```javascript
// Old way - string concatenation
let name = "John";
let greeting = "Hello, " + name + "!";

// Template literal way
let greeting = `Hello, ${name}!`;

// Result: "Hello, John!"
```

### Multiline Strings

```javascript
// Old way - escape characters and concatenation
let oldWay = "Line 1\n" + "Line 2\n" + "Line 3";

// Template literal way
let newWay = `
    Line 1
    Line 2
    Line 3
`;

// Preserves formatting and newlines
```

---

## 🔧 Embedded Expressions

### Variables and Constants

```javascript
let firstName = "John";
let lastName = "Doe";

let fullName = `${firstName} ${lastName}`; // "John Doe"

// Can embed any JavaScript expression
let message = `${firstName.toUpperCase()} ${lastName.toUpperCase()}`;
// "JOHN DOE"
```

### Calculations

```javascript
let price = 100;
let tax = 0.08;

let total = `Total: $${(price * (1 + tax)).toFixed(2)}`;
// "Total: $108.00"

let numbers = [1, 2, 3];
let sum = `Sum: ${numbers.reduce((a, b) => a + b, 0)}`;
// "Sum: 6"
```

### Ternary Expressions

```javascript
let age = 20;
let status = `You are ${age >= 18 ? "an adult" : "a minor"}`;
// "You are an adult"

let isLoggedIn = false;
let message = `Status: ${isLoggedIn ? "Logged In" : "Guest"}`;
// "Status: Guest"
```

### Function Calls

```javascript
function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

let name = "john";
let formatted = `${capitalize(name)}`; // "John"

// Can call methods directly
let text = "hello";
let upper = `${text.toUpperCase()}`; // "HELLO"
```

---

## 🏗️ Practical Examples

### HTML Generation

```javascript
let user = { name: "John", email: "john@example.com" };

let html = `
    <div class="user-card">
        <h2>${user.name}</h2>
        <p>${user.email}</p>
    </div>
`;
```

### Dynamic CSS

```javascript
function createStyles(primaryColor, secondaryColor) {
    return `
        .btn-primary {
            background: ${primaryColor};
            color: white;
            padding: 10px 20px;
            border: none;
        }
        
        .btn-secondary {
            background: ${secondaryColor};
            color: white;
        }
    `;
}

let styles = createStyles("#3498db", "#2ecc71");
```

### SQL Queries

```javascript
let table = "users";
let conditions = ["name = 'John'", "age > 18"];

let query = `
    SELECT * 
    FROM ${table} 
    WHERE ${conditions.join(" AND ")}
`;
// SELECT * FROM users WHERE name = 'John' AND age > 18
```

---

## 🏷️ Tagged Templates

### Basic Tagged Template

```javascript
function highlight(strings, ...values) {
    // strings: array of literal string parts
    // values: array of interpolated values
    
    let result = "";
    strings.forEach((str, i) => {
        result += str;
        if (i < values.length) {
            result += `<strong>${values[i]}</strong>`;
        }
    });
    return result;
}

let name = "John";
let result = highlight`Hello ${name}!`;
// Result: "Hello <strong>John</strong>!"
```

### Practical Tagged Template

```javascript
// SQL query builder with escaping
function sql(strings, ...values) {
    let query = "";
    strings.forEach((str, i) => {
        query += str;
        if (i < values.length) {
            // Simple escaping (use library for production!)
            let value = String(values[i]).replace(/'/g, "''");
            query += `'${value}'`;
        }
    });
    return query;
}

let table = "users";
let name = "O'Brien";
let query = sql`SELECT * FROM ${table} WHERE name = ${name}`;
// "SELECT * FROM users WHERE name = 'O''Brien'"
```

---

## 📊 Comparison

| Feature | Old Way | Template Literal |
|---------|---------|------------------|
| Concatenation | `"a" + b + "c"` | `` `${a}b${c}` `` |
| Multiline | `"\nLine1\nLine2"` | `` `\nLine1\nLine2` `` |
| Expression | `calc(a, b)` | `` `${calc(a, b)}` `` |
| Readability | Poor | Excellent |

---

## 🎯 Best Practices

### When to Use Template Literals

```javascript
// ✅ Good uses
let message = `Hello, ${name}!`;
let html = `<div>${content}</div>`;
let query = `SELECT * FROM ${table}`;

// ❌ Overuse - simple string doesn't need it
let simple = `${"hello"}`; // Unnecessary
let plain = "just text";  // No variables needed
```

### Security Considerations

```javascript
// ❌ Dangerous - XSS vulnerability
let userInput = "<script>alert('xss')</script>";
let html = `<div>${userInput}</div>`;

// ✅ Safe - escape user input
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

let safeHtml = `<div>${escapeHtml(userInput)}</div>`;
```

---

## 🧪 Practice Exercises

### Exercise 1: Template String Builder

```javascript
function createUserCard(name, email, role) {
    // Use template literal to create HTML
    return `
        <div class="user-card">
            <h3>${name}</h3>
            <p>${email}</p>
            <span class="role">${role}</span>
        </div>
    `;
}

console.log(createUserCard("John", "john@email.com", "Admin"));
```

### Exercise 2: Format Currency

```javascript
function formatCurrency(amount, currency = "USD") {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

console.log(formatCurrency(1234.56)); // "$1,234.56"
console.log(formatCurrency(1234.56, 'EUR')); // "€1,234.56"
```

---

## 🔗 Related Topics

- [10_Strings_Fundamentals.md](./10_Strings_Fundamentals.md)
- [05_Data_Types_Complete.md](./05_Data_Types_Complete.md)

---

**Next: Learn about [Control Flow Introduction](./03_CONTROL_FLOW/01_Conditional_Statements_Intro.md)**