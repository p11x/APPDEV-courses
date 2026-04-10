# 📚 String Templates and Interpolation

## 📋 Overview

Template literals (also known as template strings) are a powerful feature introduced in ES6 (ES2015) that fundamentally changed how we work with strings in JavaScript. They provide a more expressive and flexible way to create strings, supporting multi-line strings, embedded expressions, and custom processing through tagged templates.

This comprehensive guide covers everything from basic template literal syntax to advanced tagged template techniques, including security considerations and professional use cases. Understanding template literals is essential for modern JavaScript development, as they form the foundation for string manipulation in contemporary codebases.

---

## 🔤 Table of Contents

1. [Introduction to Template Literals](#introduction-to-template-literals)
2. [Expression Interpolation](#expression-interpolation)
3. [Multi-line Strings](#multi-line-strings)
4. [Tagged Templates](#tagged-templates)
5. [Raw Strings](#raw-strings)
6. [Security Considerations](#security-considerations)
7. [Advanced Techniques](#advanced-techniques)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)

---

## 🏁 Introduction to Template Literals

Template literals are string literals enclosed by backtick (`) characters. They allow for embedded expressions, called expressions interpolation, and support multi-line strings.

### Basic Syntax

```javascript
const greeting = `Hello, World!`;
```

This is functionally equivalent to:
```javascript
const greeting = "Hello, World!";
```

But template literals provide additional capabilities that make them far more powerful.

**Code Example 1: Basic Template Literals**

```javascript
// file: examples/basic-template.js
// Description: Understanding template literal basics

// Simple string with backticks
const simple = `Hello World`;
console.log(simple); // "Hello World"

// Equivalent to regular strings
const equivalent = "Hello World";
console.log(simple === equivalent); // true

// Works with quotes inside
const withQuotes = `He said "Hello"`;
console.log(withQuotes); // 'He said "Hello"'

// Single quotes inside
const withSingle = `It's a beautiful day`;
console.log(withSingle); // "It's a beautiful day"

// Template literals don't require escaping
const escaped = `No need to escape: " and ' and \`;
console.log(escaped); // 'No need to escape: " and ' and \'
```

---

## 🔢 Expression Interpolation

One of the most powerful features of template literals is the ability to embed expressions using `${expression}` syntax. Any JavaScript expression can be included within the curly braces.

### Simple Expressions

**Code Example 2: Expression Interpolation Basics**

```javascript
// file: examples/expression-interpolation.js
// Description: Basic expression interpolation

const name = "Alice";
const age = 30;

// Simple variable interpolation
const greeting = `Hello, ${name}!`;
console.log(greeting); // "Hello, Alice!"

// Expression evaluation
const message = `You will be ${age + 1} next year`;
console.log(message); // "You will be 31 next year"

// Function calls
const text = `Uppercase: ${"hello".toUpperCase()}`;
console.log(text); // "Uppercase: HELLO"

// Ternary operator
const status = `You are ${age >= 18 ? "an adult" : "a minor"}`;
console.log(status); // "You are an adult"

// Accessing object properties
const user = { name: "Bob", city: "New York" };
const info = `${user.name} lives in ${user.city}`;
console.log(info); // "Bob lives in New York"

// Array operations
const items = ["apple", "banana"];
const count = `You have ${items.length} items`;
console.log(count); // "You have 2 items"
```

### Complex Expressions

**Code Example 3: Complex Expression Interpolation**

```javascript
// file: examples/complex-expressions.js
// Description: Using complex expressions in templates

// Arrow functions
const add = (a, b) => a + b;
const result = `5 + 3 = ${add(5, 3)}`;
console.log(result); // "5 + 3 = 8"

// Function with template in return
function createMessage(user, points) {
    return `Welcome back, ${user.name}! 
    You have ${points} points remaining.`;
}

console.log(createMessage({ name: "John" }, 100));
// "Welcome back, John! \n    You have 100 points remaining."

// Multiple interpolations
const firstName = "John";
const lastName = "Doe";
const fullName = `${firstName} ${lastName}`;
console.log(fullName); // "John Doe"

// Calculations
const price = 99.99;
const tax = 0.1;
const total = `Total: $${(price * (1 + tax)).toFixed(2)}`;
console.log(total); // "Total: $109.99"

// Date formatting
const today = new Date();
const formatted = `Today is ${today.toLocaleDateString()}`;
console.log(formatted); // "Today is 4/3/2026"

// Condition in template
const user = { loggedIn: true, name: "Alice" };
const loginStatus = `${user.name} is ${user.loggedIn ? "online" : "offline"}`;
console.log(loginStatus); // "Alice is online"
```

---

## 📝 Multi-line Strings

Template literals preserve whitespace and newlines, making them perfect for multi-line strings without the need for escape sequences.

### Basic Multi-line

**Code Example 4: Multi-line Template Literals**

```javascript
// file: examples/multiline.js
// Description: Creating multi-line strings with templates

// Simple multi-line
const poem = `Roses are red,
Violets are blue,
Sugar is sweet,
And so are you.`;

console.log(poem);
// "Roses are red,\nViolets are blue,\nSugar is sweet,\nAnd so are you."

// Preserves indentation
const indented = `    This text is indented
        And so is this
    This too`;

console.log(indented);
// "    This text is indented\n        And so is this\n    This too"

// HTML with proper formatting
const html = `
<!DOCTYPE html>
<html>
<head>
    <title>My Page</title>
</head>
<body>
    <h1>Hello World</h1>
</body>
</html>
`.trim();

console.log(html);
// <!DOCTYPE html><html>...</html>

// CSS with proper formatting
const styles = `
.container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
}
`;
```

### Practical Multi-line Applications

**Code Example 5: Real-world Multi-line Use Cases**

```javascript
// file: examples/multiline-practical.js
// Description: Practical multi-line string applications

// Generating SQL queries
const tableName = "users";
const columns = ["id", "name", "email"];
const sql = `INSERT INTO ${tableName} (${columns.join(", ")})
VALUES (?, ?, ?)`;

console.log(sql);
// "INSERT INTO users (id, name, email)\nVALUES (?, ?, ?)"

// Creating email templates
const emailTemplate = `
Dear ${"Customer Name"},

Thank you for your order #${12345}.
Your order total is $${99.99}.

We will ship your items within 2-3 business days.

Best regards,
The Team
`.trim();

// Markdown formatting
const markdown = `
# Product Title

## Description
This is a **bold** description with *italic* text.

## Features
- Feature 1
- Feature 2
- Feature 3

## Price
$${29.99}
`;

// JSON string with formatting (using template)
const jsonString = JSON.stringify({
    name: "Product",
    price: 29.99,
    inStock: true
}, null, 2);

console.log(jsonString);
// {
//   "name": "Product",
//   "price": 29.99,
//   "inStock": true
// }
```

---

## 🏷️ Tagged Templates

Tagged templates are a powerful extension of template literals. They allow you to call a function to process the template literal, giving you full control over how the string is constructed.

### How Tagged Templates Work

When you use a tag function, JavaScript calls your function with:
1. An array of string segments (the literal parts)
2. The interpolated values

```javascript
function tagFunction(strings, ...values) {
    // strings: array of literal strings between expressions
    // values: the evaluated expressions
}
```

**Code Example 6: Tagged Template Basics**

```javascript
// file: examples/tagged-basics.js
// Description: Understanding tagged template functions

// Simple tagged template
function simpleTag(strings, ...values) {
    console.log("Strings:", strings);
    console.log("Values:", values);
    return "Result";
}

const result = simpleTag`Hello ${"World"}!`;
console.log("Result:", result);

// Strings: ["Hello ", "!"]
// Values: ["World"]
// Result: "Result"

// Creating a custom tag
function highlight(strings, ...values) {
    return strings.reduce((result, string, i) => {
        const value = values[i] ? `<mark>${values[i]}</mark>` : "";
        return result + string + value;
    }, "");
}

const name = "Alice";
const highlighted = highlight`Hello, ${name}!`;
console.log(highlighted); // "Hello, <mark>Alice</mark>!"
```

### Building Custom Tags

**Code Example 7: Custom Tag Functions**

```javascript
// file: examples/custom-tags.js
// Description: Building practical tagged template functions

// Tag that uppercases interpolated values
function upper(strings, ...values) {
    return strings.reduce((acc, str, i) => {
        const value = values[i] ? String(values[i]).toUpperCase() : "";
        return acc + str + value;
    }, "");
}

const name = "world";
console.log(upper`Hello, ${name}!`); // "Hello, WORLD!"

// Tag that joins arrays nicely
function joinArray(strings, ...values) {
    return strings.reduce((acc, str, i) => {
        const value = Array.isArray(values[i]) 
            ? values[i].join(", ") 
            : values[i];
        return acc + str + (value !== undefined ? value : "");
    }, "");
}

const fruits = ["apple", "banana", "orange"];
console.log(joinArray`I like ${fruits}!`); // "I like apple, banana, orange!"

// Tag that formats numbers as currency
function currency(strings, ...values) {
    return strings.reduce((acc, str, i) => {
        const value = typeof values[i] === "number" 
            ? `$${values[i].toFixed(2)}` 
            : values[i];
        return acc + str + (value !== undefined ? value : "");
    }, "");
}

const price = 99.99;
console.log(currency`Total: ${price}`); // "Total: $99.90"

// Tag that creates SQL-safe strings
function sql(strings, ...values) {
    return strings.reduce((acc, str, i) => {
        const value = typeof values[i] === "string" 
            ? `'${values[i].replace(/'/g, "''")}'` 
            : values[i];
        return acc + str + (value !== undefined ? value : "");
    }, "");
}

const table = "users";
const name = "O'Brien";
console.log(sql`SELECT * FROM ${table} WHERE name = ${name}`);
// "SELECT * FROM users WHERE name = 'O''Brien'"
```

### Advanced Tagged Templates

**Code Example 8: Advanced Tagged Template Patterns**

```javascript
// file: examples/advanced-tags.js
// Description: Advanced tagged template techniques

// Internationalization tag
const translations = {
    en: { greeting: "Hello", goodbye: "Goodbye" },
    es: { greeting: "Hola", goodbye: "Adiós" }
};

function i18n(strings, ...values) {
    const lang = values[values.length - 1] || "en"; // Last value is language
    return strings.reduce((acc, str, i) => {
        const value = values[i];
        if (typeof value === "string" && translations[lang][value]) {
            return acc + str + translations[lang][value];
        }
        return acc + str + (value !== undefined ? value : "");
    }, "");
}

console.log(i18n`${"greeting"}, World!${"en"}`);   // "Hello, World!"
console.log(i18n`${"greeting"}, World!${"es"}`);   // "Hola, World!"

// Translation tag with object lookup
function t(strings, ...values) {
    const dict = values[0]; // First value is dictionary
    return strings.reduce((acc, str, i) => {
        const key = values[i + 1];
        const translated = key && typeof dict === "object" ? dict[key] : key;
        return acc + str + (translated !== undefined ? translated : "");
    }, "");
}

const dict = { name: "Name", age: "Age" };
console.log(t`${dict} name: John, ${"age"}: 30`);
// "Name: John, Age: 30"

// GraphQL-style query builder
function gql(strings, ...values) {
    return strings.reduce((acc, str, i) => {
        let value = values[i];
        if (typeof value === "string") {
            value = `"${value}"`;
        } else if (Array.isArray(value)) {
            value = `[${value.map(v => typeof v === "string" ? `"${v}"` : v).join(", ")}]`;
        }
        return acc + str + (value !== undefined ? value : "");
    }, "");
}

const fields = ["id", "name"];
console.log(gql`
    query {
        user(id: ${123}) {
            ${fields}
        }
    }
`);
// "query {\n        user(id: 123) {\n            [\"id\", \"name\"]\n        }\n    }"
```

---

## 📄 Raw Strings

Template literals provide access to the "raw" string content through the `raw` property. This is particularly useful when you need the literal text without processing escape sequences.

### Using String.raw

**Code Example 9: Raw String Access**

```javascript
// file: examples/raw-strings.js
// Description: Working with raw strings

// String.raw function
const raw = String.raw`Hello\nWorld`;
console.log(raw); // "Hello\\nWorld" (backslash is literal)
console.log(`Hello\nWorld`); // "Hello\n" (newline is processed)

// Access raw in tagged template
function showRaw(strings, ...values) {
    console.log("Processed strings:", strings);
    console.log("Raw strings:", strings.raw);
    return strings.join("");
}

showRaw`Hello\nWorld`;
// Processed strings: ["Hello\nWorld"]
// Raw strings: ["Hello\\nWorld"]

// Useful for regex patterns
const pattern = String.raw`\.+`;  // Matches literal dots
console.log(pattern); // "\\.+"

// Creating Windows paths
const windowsPath = String.raw`C:\Users\John\Documents`;
console.log(windowsPath); // "C:\\Users\\John\\Documents"

// Escape sequences in raw
function escapeHtml(strings, ...values) {
    const rawStrings = strings.raw;
    // Work with unprocessed escape sequences
}

escapeHtml`Line1\nLine2`;
// Raw strings: ["Line1\\nLine2"]
```

---

## 🔒 Security Considerations

Template literals, like any string handling in JavaScript, require careful attention to security. The main concern is injection attacks.

### Understanding Injection Risks

**Code Example 10: Understanding Template Injection**

```javascript
// file: examples/security-basics.js
// Description: Understanding injection vulnerabilities

// ❌ Dangerous: Unvalidated user input in templates
function greetUnsafe(userName) {
    return `Hello, ${userName}!`;
}

// User could input malicious content
console.log(greetUnsafe("<script>alert('xss')</script>"));
// "Hello, <script>alert('xss')</script>!"

// In HTML context, this creates XSS vulnerability
const userInput = "${alert('XSS')}";
// In innerHTML: <div>${alert('XSS')}</div> - evaluates the expression!

// ❌ Dangerous: Template injection with eval-like behavior
const userTemplate = "${process.exit(1)}";
// This actually executes in template literal!
```

### Preventing Injection

**Code Example 11: Safe Template Practices**

```javascript
// file: examples/security-safe.js
// Description: Implementing safe template practices

// ✅ Safe: Sanitize user input before interpolation
function sanitize(str) {
    return str
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function greetSafe(userName) {
    const clean = sanitize(userName || "");
    return `Hello, ${clean}!`;
}

console.log(greetSafe("<script>alert(1)</script>"));
// "Hello, &lt;script&gt;alert(1)&lt;/script&gt;!"

// ✅ Safe: Use textContent instead of innerHTML
function setSafeText(element, userInput) {
    element.textContent = userInput;  // Automatically escaped
}

// ✅ Safe: Validate and constrain input
function validateName(name) {
    // Only allow alphanumeric and limited special chars
    return name.replace(/[^a-zA-Z0-9 _-]/g, "").slice(0, 50);
}

function greetValidated(name) {
    const safeName = validateName(name);
    return `Hello, ${safeName}!`;
}

console.log(greetValidated("John<script>"));  // "Hello, John!"
console.log(greetValidated("Robert'; DROP TABLE--")); // "Hello, Robert DROP TABLE--"

// ✅ Safe: Use specific escaping based on context
function escapeHtmlContext(str) {
    return str
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;");
}

function escapeAttributeContext(str) {
    return str
        .replace(/&/g, "&amp;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;");
}

function escapeUrlContext(str) {
    return encodeURIComponent(str);
}
```

### Professional Security Patterns

**Code Example 12: Production Security Implementation**

```javascript
// file: examples/security-production.js
// Description: Production-ready security patterns

// Input sanitization class
class InputSanitizer {
    static sanitizeHTML(input) {
        const div = document.createElement('div');
        div.textContent = input;
        return div.innerHTML;
    }

    static sanitizeAttribute(input) {
        return input.replace(/["'&<>]/g, (char) => {
            const entities = {
                '&': '&amp;',
                '"': '&quot;',
                "'": '&#39;',
                '<': '&lt;',
                '>': '&gt;'
            };
            return entities[char] || char;
        });
    }

    static sanitizeURL(input) {
        try {
            const url = new URL(input);
            return url.href;
        } catch {
            return '#';
        }
    }

    static sanitizeSQL(input) {
        if (typeof input !== 'string') return input;
        return input.replace(/['"]/g, '""');
    }
}

// Safe template for user-generated content
function createUserCard(user) {
    const name = InputSanitizer.sanitizeHTML(user.name);
    const bio = InputSanitizer.sanitizeHTML(user.bio || "");
    
    return `
        <div class="user-card">
            <h2>${name}</h2>
            <p>${bio}</p>
        </div>
    `;
}

// Safe attribute handling
function createButton(label, onClick) {
    const safeLabel = InputSanitizer.sanitizeAttribute(label);
    const safeHandler = InputSanitizer.sanitizeAttribute(onClick);
    
    return `<button onclick="${safeHandler}">${safeLabel}</button>`;
}

// Content Security Policy aware templates
function createCSPTemplate(cspnonce) {
    return (strings, ...values) => {
        // Add nonce to script tags
        return strings.reduce((result, str, i) => {
            const value = values[i] || "";
            let processed = str + value;
            
            if (str.endsWith('<script>')) {
                processed = str.replace('<script>', `<script nonce="${cspnonce}">`) + value;
            }
            
            return result + processed;
        }, "");
    };
}

// Usage
const nonce = "abc123xyz";
const safeTemplate = createCSPTemplate(nonce);
const scriptContent = safeTemplate`<script>alert('secure')</script>`;
```

---

## 🚀 Advanced Techniques

### Dynamic Template Building

**Code Example 13: Dynamic Template Generation**

```javascript
// file: examples/advanced-dynamic.js
// Description: Advanced dynamic template patterns

// Template from configuration
const createTemplate = (config) => {
    const parts = [];
    if (config.showTitle) parts.push("## ${title}");
    if (config.showDescription) parts.push("${description}");
    if (config.showDate) parts.push("Date: ${date}");
    
    const templateString = parts.join("\n");
    return (data) => templateString
        .replace(/\$\{title\}/g, data.title || "")
        .replace(/\$\{description\}/g, data.description || "")
        .replace(/\$\{date\}/g, data.date || "");
};

const renderTemplate = createTemplate({
    showTitle: true,
    showDescription: true,
    showDate: false
});

console.log(renderTemplate({ title: "My Post", description: "Content here" }));

// Conditional template sections
function template(strings, ...values) {
    return (data) => {
        return strings.reduce((result, str, i) => {
            let value = values[i];
            
            // Handle conditional sections
            if (typeof value === "function") {
                value = value(data);
            } else if (typeof value === "object" && value !== null) {
                value = value[data.condition] || "";
            }
            
            return result + str + (value !== undefined ? value : "");
        }, "");
    };
}

const section = template`${d => d.showHeader ? `<h1>${d.title}</h1>` : ""}
<p>${d.description}</p>`;

console.log(section({ showHeader: true, title: "Hello", description: "World" }));
```

### Performance Optimization

**Code Example 14: Template Performance Patterns**

```javascript
// file: examples/performance.js
// Description: Optimizing template performance

// Pre-compile static parts of templates
const templateCache = new Map();

function getCompiledTemplate(pattern) {
    if (!templateCache.has(pattern)) {
        const regex = new RegExp(pattern, 'g');
        templateCache.set(pattern, regex);
    }
    return templateCache.get(pattern);
}

// Lazy template evaluation
const createDeferredTemplate = (strings, ...values) => {
    let cachedResult = null;
    let evaluated = false;
    
    return () => {
        if (!evaluated) {
            cachedResult = strings.reduce((result, str, i) => {
                const value = typeof values[i] === "function" 
                    ? values[i]() 
                    : values[i];
                return result + str + (value !== undefined ? value : "");
            }, "");
            evaluated = true;
        }
        return cachedResult;
    };
};

const slowValue = () => {
    console.log("Computing...");
    return "expensive result";
};

const deferred = createDeferredTemplate`Value: ${slowValue}`;
console.log(deferred()); // "Computing..." + result
console.log(deferred()); // Returns cached result (no "Computing...")

// Batch string building for large templates
function buildLargeString(items) {
    const parts = [];
    
    for (const item of items) {
        parts.push(`<li>${item.name}: $${item.price}</li>`);
    }
    
    return `<ul>${parts.join("")}</ul>`;
}
```

---

## 📊 Key Takeaways

1. **Use Template Literals**: Prefer backticks over concatenation for readability.

2. **Expression Interpolation**: Any valid JavaScript expression can be interpolated using `${expression}`.

3. **Multi-line Support**: Template literals naturally support multi-line strings without escape sequences.

4. **Tagged Templates**: Use tag functions to process templates with custom logic.

5. **Security First**: Always sanitize user input before using in template literals, especially when outputting to HTML.

6. **Raw Strings**: Use `String.raw` or the `.raw` property when you need literal backslash characters.

7. **Performance**: Consider caching compiled templates for repeated use.

---

## ⚠️ Common Pitfalls

1. **Forgetting to Escape**: Template literals don't auto-escape; you must sanitize manually.

2. **Expression Errors**: Invalid expressions in `${}` will throw runtime errors.

3. **Accidental Interpolation**: Using `${}` in strings intended to be literal will cause interpolation.

4. **Not Closing Backticks**: Every backtick must have a matching pair.

5. **Confusing Tagged vs Untagged**: Tagged templates require a function name before the backtick.

6. **Nested Templates**: Can be confusing; consider breaking into separate variables.

7. **Undefined Values**: `${undefined}` produces the string "undefined".

---

## 🔗 Related Files

- **[String Methods Comprehensive](./01_STRING_METHODS_COMPREHENSIVE.md)** - String manipulation
- **[Regular Expressions Master](./02_REGULAR_EXPRESSIONS_MASTER.md)** - Pattern-based operations
- **[String Validation and Sanitization](./04_STRING_VALIDATION_AND_SANITIZATION.md)** - Input security
- **[JavaScript Syntax Basics](../02_JAVASCRIPT_SYNTAX_AND_BASICS/10_Strings_Fundamentals.md)** - Basic strings

---

## 📚 Further Reading

- [MDN: Template Literals](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals)
- [ES6 Specification](https://tc39.es/ecma262/#sec-template-literals)
- [OWASP XSS Prevention](https://cheatsheetseries.owasp.org/cheatsheets/Cross_Site_Scripting_Prevention_Cheat_Sheet.html)