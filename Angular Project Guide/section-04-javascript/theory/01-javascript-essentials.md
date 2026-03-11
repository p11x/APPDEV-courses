# Section 4: JavaScript Essentials

## Introduction to JavaScript

JavaScript brings your web pages to life! While HTML provides structure and CSS provides styling, JavaScript adds interactivity and dynamic behavior.

### What You'll Learn

- JavaScript fundamentals
- Variables and data types
- Functions and scope
- DOM manipulation
- Event handling
- Asynchronous JavaScript (Promises, async/await)

---

## 4.1 Getting Started with JavaScript

### Where to Write JavaScript

**Option 1: In HTML file (inline):**
```html
<script>
    console.log("Hello, World!");
</script>
```

**Option 2: External file (recommended):**
```html
<script src="script.js"></script>
```

### Your First JavaScript Code
```javascript
// This is a comment
console.log("Hello, World!"); // Output to console
alert("Welcome!"); // Show popup
document.write("Hello from JavaScript!"); // Write to page
```

---

## 4.2 Variables and Data Types

### Variable Declaration

```javascript
// Modern way (const and let)
const name = "John";     // Cannot be reassigned
let age = 25;           // Can be reassigned

// Old way (avoid)
var oldStyle = "Don't use this";
```

### Data Types

**Primitive Types:**
```javascript
// String
const productName = "Laptop";

// Number
const price = 999.99;

// Boolean
const inStock = true;

// Undefined
let unknown; // undefined

// Null
const empty = null;
```

**Reference Types:**
```javascript
// Array
const products = ["Laptop", "Mouse", "Keyboard"];

// Object
const product = {
    name: "Laptop",
    price: 999.99,
    inStock: true
};
```

---

## 4.3 Operators

### Arithmetic Operators
```javascript
const sum = 10 + 5;      // 15
const diff = 10 - 5;     // 5
const product = 10 * 5;  // 50
const quotient = 10 / 5; // 2
const remainder = 10 % 3; // 1
```

### Comparison Operators
```javascript
5 === 5   // true (strict equality)
5 === "5" // false (different types)
5 == "5"  // true (loose equality)
5 !== 3   // true
5 > 3     // true
5 < 3     // false
```

### Logical Operators
```javascript
true && true   // true (AND)
true || false  // true (OR)
!true          // false (NOT)
```

---

## 4.4 Control Flow

### If-Else Statements
```javascript
const price = 100;

if (price > 100) {
    console.log("Expensive");
} else if (price > 50) {
    console.log("Moderate");
} else {
    console.log("Affordable");
}
```

### Switch Statement
```javascript
const category = "Electronics";

switch (category) {
    case "Electronics":
        console.log("Electronic products");
        break;
    case "Furniture":
        console.log("Furniture items");
        break;
    default:
        console.log("Other category");
}
```

### Loops

**For Loop:**
```javascript
for (let i = 0; i < 5; i++) {
    console.log(i); // 0, 1, 2, 3, 4
}
```

**While Loop:**
```javascript
let count = 0;
while (count < 5) {
    console.log(count);
    count++;
}
```

**For...of (Arrays):**
```javascript
const products = ["Laptop", "Mouse", "Keyboard"];

for (const product of products) {
    console.log(product);
}
```

---

## 4.5 Functions

### Function Declaration
```javascript
function greet(name) {
    return "Hello, " + name;
}

const message = greet("John");
console.log(message); // "Hello, John"
```

### Arrow Functions (ES6+)
```javascript
// Traditional
function add(a, b) {
    return a + b;
}

// Arrow function
const add = (a, b) => a + b;

// With body
const greet = (name) => {
    const message = "Hello, " + name;
    return message;
};
```

### Default Parameters
```javascript
function greet(name = "Guest") {
    return "Hello, " + name;
}

greet();        // "Hello, Guest"
greet("John");  // "Hello, John"
```

---

## 4.6 Arrays

### Creating Arrays
```javascript
const products = [];                    // Empty array
const numbers = [1, 2, 3, 4, 5];      // With values
const mixed = [1, "hello", true];     // Mixed types
```

### Array Methods
```javascript
const products = ["Laptop", "Mouse", "Keyboard"];

// Add to end
products.push("Monitor"); 

// Remove from end
products.pop();

// Add to beginning
products.unshift("Headphones");

// Remove from beginning
products.shift();

// Find index
products.indexOf("Mouse"); // 1

// Check if includes
products.includes("Mouse"); // true

// Slice (copy portion)
products.slice(0, 2); // ["Laptop", "Mouse"]

// Splice (remove/add)
products.splice(1, 1); // Remove "Mouse"
```

### Array Transformations
```javascript
const numbers = [1, 2, 3, 4, 5];

// map - transform each element
const doubled = numbers.map(n => n * 2); 
// [2, 4, 6, 8, 10]

// filter - keep matching elements
const evens = numbers.filter(n => n % 2 === 0);
// [2, 4]

// reduce - combine into single value
const sum = numbers.reduce((acc, n) => acc + n, 0);
// 15

// find - get first match
numbers.find(n => n > 3); // 4
```

---

## 4.7 Objects

### Creating Objects
```javascript
const product = {
    name: "Laptop",
    price: 999.99,
    inStock: true,
    
    // Method
    displayInfo: function() {
        return this.name + " - $" + this.price;
    }
};

// Or with arrow function method (note: 'this' won't work)
const product2 = {
    name: "Mouse",
    getName: () => this.name // Don't do this!
};
```

### Accessing Properties
```javascript
// Dot notation
product.name;        // "Laptop"
product.price;      // 999.99

// Bracket notation
product["name"];    // "Laptop"

// Adding new properties
product.category = "Electronics";

// Deleting properties
delete product.inStock;
```

### Object Methods
```javascript
const product = {
    name: "Laptop",
    price: 999.99
};

// Get all keys
Object.keys(product); // ["name", "price"]

// Get all values
Object.values(product); // ["Laptop", 999.99]

// Get all entries
Object.entries(product); 
// [["name", "Laptop"], ["price", 999.99]]
```

---

## 4.8 DOM Manipulation

### Selecting Elements
```javascript
// By ID
const element = document.getElementById("myId");

// By class (returns collection)
const elements = document.getElementsByClassName("myClass");

// By tag
const paragraphs = document.getElementsByTagName("p");

// Query selector (first match)
const element = document.querySelector(".myClass");

// Query selector all (all matches)
const elements = document.querySelectorAll(".myClass");
```

### Modifying Content
```javascript
const element = document.querySelector("#title");

// Text content
element.textContent = "New Title";

// HTML content
element.innerHTML = "<strong>Bold Title</strong>";
```

### Modifying Styles
```javascript
const element = document.querySelector(".card");

// Direct style
element.style.color = "blue";
element.style.backgroundColor = "#f0f0f0";

// Using class (recommended)
element.classList.add("active");
element.classList.remove("hidden");
element.classList.toggle("expanded");
element.classList.contains("active"); // true/false
```

### Creating Elements
```javascript
// Create new element
const newDiv = document.createElement("div");
newDiv.textContent = "Hello!";
newDiv.className = "card";

// Add to DOM
document.body.appendChild(newDiv);

// Or insert at specific location
const container = document.querySelector("#container");
container.insertBefore(newDiv, container.firstChild);
```

---

## 4.9 Event Handling

### Adding Event Listeners
```javascript
const button = document.querySelector("#myButton");

// Modern way (recommended)
button.addEventListener("click", function(event) {
    console.log("Button clicked!");
});

// Arrow function
button.addEventListener("click", (event) => {
    console.log(event.target);
});

// Inline (not recommended)
<button onclick="handleClick()">Click</button>
```

### Common Events

| Event | Description |
|-------|-------------|
| click | Mouse click |
| dblclick | Double click |
| mouseenter | Mouse enters element |
| mouseleave | Mouse leaves element |
| keydown | Key pressed |
| keyup | Key released |
| submit | Form submitted |
| change | Input value changed |
| input | Input value changed (immediate) |
| load | Page/element loaded |

### Event Object
```javascript
button.addEventListener("click", (event) => {
    event.target;        // Element that triggered event
    event.type;          // Event type ("click")
    event.preventDefault(); // Stop default behavior
    event.stopPropagation(); // Stop event bubbling
});
```

---

## 4.10 Asynchronous JavaScript

### Promises
```javascript
// Creating a promise
const fetchData = () => {
    return new Promise((resolve, reject) => {
        // Simulate API call
        setTimeout(() => {
            const data = { name: "Product" };
            resolve(data); // Success
            // reject("Error"); // Failure
        }, 1000);
    });
};

// Using promise
fetchData()
    .then(data => console.log(data))
    .catch(error => console.error(error))
    .finally(() => console.log("Done"));
```

### Async/Await (Modern Approach)
```javascript
// Async function
async function getProduct() {
    try {
        const response = await fetch("/api/products");
        const products = await response.json();
        return products;
    } catch (error) {
        console.error("Error:", error);
    }
}

// Calling async function
getProduct().then(products => console.log(products));
```

### Fetch API
```javascript
// GET request
fetch("https://api.example.com/products")
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error(error));

// POST request
fetch("https://api.example.com/products", {
    method: "POST",
    headers: {
        "Content-Type": "application/json"
    },
    body: JSON.stringify({
        name: "New Product",
        price: 99.99
    })
})
    .then(response => response.json())
    .then(data => console.log(data));
```

---

## 4.11 ES6+ Features

### Template Literals
```javascript
const name = "John";
const age = 25;

// Old way
const message = "My name is " + name + " and I am " + age + " years old.";

// New way (template literals)
const message = `My name is ${name} and I am ${age} years old.`;
```

### Destructuring
```javascript
// Array destructuring
const [first, second] = [1, 2, 3];
// first = 1, second = 2

// Object destructuring
const { name, price } = { name: "Laptop", price: 999 };
// name = "Laptop", price = 999
```

### Spread Operator
```javascript
// Array spread
const arr1 = [1, 2, 3];
const arr2 = [...arr1, 4, 5]; // [1, 2, 3, 4, 5]

// Object spread
const obj1 = { a: 1, b: 2 };
const obj2 = { ...obj1, c: 3 }; // { a: 1, b: 2, c: 3 }
```

---

## 4.12 Summary

### Key Takeaways

1. **Variables** - Use `const` and `let` (not `var`)
2. **Arrays** - Use methods like `map`, `filter`, `reduce`
3. **Objects** - Store related data together
4. **Functions** - Use arrow functions for concise syntax
5. **DOM** - Select, modify, and create elements
6. **Events** - Listen for user interactions
7. **Async** - Use `async/await` for asynchronous operations
8. **Fetch** - Use Fetch API for HTTP requests

### What's Next?

Now that you understand JavaScript, let's learn **Bootstrap** for rapid UI development, then move to **TypeScript** and finally **Angular**!
