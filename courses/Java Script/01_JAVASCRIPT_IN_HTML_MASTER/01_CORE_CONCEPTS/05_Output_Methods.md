# 📤 JavaScript Output Methods

## 📋 Overview

JavaScript provides multiple ways to output information. Understanding each method helps you choose the right tool for different situations.

---

## 🖥️ console.log()

### Basic Usage

```javascript
// Simple output
console.log("Hello, World!");

// Numbers
console.log(42);
console.log(3.14159);

// Variables
let name = "John";
console.log(name);

// Multiple values
console.log("Name:", name, "Age:", 30);
```

### Formatted Output

```javascript
// String formatting
console.log("User: %s, Age: %d", "John", 30);

// Number formatting
console.log("Price: $%.2f", 19.99);

// Object formatting
console.log("Object: %o", { name: "John" });

// Styled output (Chrome/Firefox)
console.log("%c styled text", "color: blue; font-size: 14px");
```

### Console Methods

```javascript
// Information
console.info("This is info");

// Warnings
console.warn("This is a warning");
console.warn("Warning:", "Be careful!");

// Errors
console.error("This is an error");

// Debug
console.debug("Debug message");

// Clear console
console.clear();
```

### Advanced Console

```javascript
// Grouping
console.group("User Details");
console.log("Name: John");
console.log("Age: 30");
console.groupEnd();

// Tables
console.table([
    { name: "John", age: 30 },
    { name: "Jane", age: 25 }
]);

// Timing
console.time("loop");
for (let i = 0; i < 10000; i++) {}
console.timeEnd("loop");

// Counting
console.count("myCounter");
console.count("myCounter");
console.countReset("myCounter");

// Trace
function one() { two(); }
function two() { console.trace("Stack trace"); }
one();
```

---

## 🔔 alert()

### Basic Usage

```javascript
// Simple alert
alert("Hello!");

// With variable
let message = "Welcome!";
alert(message);

// Alert with line breaks
alert("Line 1\nLine 2\nLine 3");
```

### Important Notes

```javascript
// ⚠️ Alert blocks execution
console.log("Before alert");
alert("Click OK to continue");
console.log("After alert"); // Won't run until alert is closed

// Use sparingly - interrupts user experience
function showWelcome() {
    alert("Welcome to our website!");
    // Better to use DOM elements instead
}
```

### Real-World Example

```javascript
// ❌ Not recommended for production
function greetUser(name) {
    alert(`Hello, ${name}!`);
    alert("Welcome to our app.");
    alert("Let's get started!");
}

// ✅ Better approach
function greetUser(name) {
    const greeting = document.getElementById('greeting');
    greeting.textContent = `Hello, ${name}!`;
    greeting.classList.add('show');
}
```

---

## 📝 document.write()

### Basic Usage

```javascript
// Write to document
document.write("Hello!");

// Write with HTML
document.write("<h1>Hello World</h1>");
document.write("<p>This is a paragraph</p>");
document.write("<strong>Bold text</strong>");
```

### Important Considerations

```javascript
// ⚠️ Overwrites page if called after load
document.write("Initial content"); // Works on load

// After page loads, document.write() clears everything
window.onload = function() {
    document.write("This replaces everything!");
};

// ✅ Better alternative
document.getElementById('output').innerHTML = "New content";
```

### Use Cases

```javascript
// Debugging - quick output
document.write("<pre>");
document.write(JSON.stringify(data, null, 2));
document.write("</pre>");

// Simple testing
document.write("<h2>Test Results</h2>");
document.write("<ul>");
results.forEach(r => document.write(`<li>${r}</li>`));
document.write("</ul>");
```

---

## 🖱️ DOM Output

### textContent

```javascript
// Select element
const output = document.getElementById('output');

// Set text content
output.textContent = "Hello, World!";

// With variable
let name = "John";
output.textContent = `Welcome, ${name}!`;
```

### innerHTML

```javascript
// Set HTML content
const container = document.getElementById('container');
container.innerHTML = "<h1>Title</h1><p>Paragraph</p>";

// Dynamic content
let items = ["Apple", "Banana", "Orange"];
let html = "<ul>";
items.forEach(item => html += `<li>${item}</li>`);
html += "</ul>";
container.innerHTML = html;

// ⚠️ Security concern - avoid with user input
// Use textContent for user input to prevent XSS
```

### createElement + appendChild

```javascript
// Create and append elements
const list = document.getElementById('list');

const item = document.createElement('li');
item.textContent = "New item";
list.appendChild(item);

// Multiple elements
const fruits = ['Apple', 'Banana', 'Orange'];
fruits.forEach(fruit => {
    const li = document.createElement('li');
    li.textContent = fruit;
    list.appendChild(li);
});
```

---

## 📊 Method Comparison

| Method | Best For | Blocking | Production Safe |
|--------|----------|----------|-----------------|
| `console.log()` | Debugging, development | No | Yes |
| `alert()` | Quick notifications | Yes | Avoid |
| `document.write()` | Quick testing | Yes | Avoid |
| `textContent` | Displaying text | No | Yes |
| `innerHTML` | Displaying HTML | No | With sanitization |
| `createElement` | Dynamic elements | No | Yes |

---

## 🎯 Practical Examples

### Example 1: Debug Information Panel

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        #debug-panel {
            background: #f5f5f5;
            padding: 10px;
            font-family: monospace;
            white-space: pre-wrap;
        }
    </style>
</head>
<body>
    <div id="debug-panel"></div>
    
    <script>
        function debug(message) {
            const panel = document.getElementById('debug-panel');
            const time = new Date().toLocaleTimeString();
            panel.textContent += `[${time}] ${message}\n`;
        }
        
        debug("Application started");
        debug("Loading user data...");
        debug("User data loaded successfully");
    </script>
</body>
</html>
```

### Example 2: Status Display

```html
<!DOCTYPE html>
<html>
<body>
    <h1 id="status">Loading...</h1>
    
    <script>
        // Simulate async operation
        setTimeout(() => {
            document.getElementById('status').textContent = "Ready!";
        }, 2000);
    </script>
</body>
</html>
```

### Example 3: Dynamic List

```html
<!DOCTYPE html>
<html>
<body>
    <ul id="todo-list"></ul>
    
    <script>
        const todos = [
            "Learn JavaScript",
            "Build a project",
            "Deploy to production"
        ];
        
        const list = document.getElementById('todo-list');
        
        todos.forEach(todo => {
            const li = document.createElement('li');
            li.textContent = todo;
            list.appendChild(li);
        });
    </script>
</body>
</html>
```

---

## 🛠️ Console Tricks

### Log Levels

```javascript
// Different log levels
console.log("   LOG: General information");
console.info("  INFO: Informational message");
console.warn(" WARN: Warning - something might be wrong");
console.error("ERROR: Error - something went wrong");
```

### Visual Styling

```javascript
// CSS styling in console
console.log(
    "%cBold Red Text",
    "font-weight: bold; color: red;"
);

console.log(
    "%cBlue Background",
    "background: blue; color: white; padding: 5px;"
);

console.log(
    "%cLarge Text %cWith Color",
    "font-size: 20px;",
    "color: green;"
);
```

### Interactive Objects

```javascript
// Inspect objects
const user = { name: "John", age: 30 };
console.log(user);
console.dir(user);
console.table(user);

// Group related output
console.group("User Profile");
console.log("Name: " + user.name);
console.log("Age: " + user.age);
console.groupEnd();
```

---

## ⚠️ Best Practices

1. ✅ Use `console.log` for development debugging
2. ✅ Use DOM methods for user-facing output
3. ❌ Avoid `alert()` in production
4. ❌ Avoid `document.write()` after page load
5. ✅ Use appropriate console methods (warn, error)
6. ✅ Clean up console statements before production

---

## 🔗 Related Topics

- [04_Syntax_and_Style.md](./04_Syntax_and_Style.md)
- [09_DOM_Introduction.md](../09_DOM_MANIPULATION/09_DOM_Introduction.md)

---

**Next: Learn about [Your First JavaScript Program](./06_Your_First_JavaScript_Program.md)**