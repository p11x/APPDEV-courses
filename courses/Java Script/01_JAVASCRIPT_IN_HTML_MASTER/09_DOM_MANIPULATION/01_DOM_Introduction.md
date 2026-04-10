# 🎯 DOM Introduction

## 📋 Overview

The **Document Object Model (DOM)** is a programming interface for web documents. It represents the page so that programs can change the document structure, style, and content. JavaScript can manipulate the DOM to create dynamic, interactive web pages.

---

## 🌳 The DOM Tree

```
Document
└── <html>
    └── <head>
    │   └── <title>
    └── <body>
        ├── <h1>
        ├── <p>
        └── <div>
            ├── <span>
            └── <button>
```

### Nodes and Elements

```javascript
// Everything is a node
document;           // Document node
document.head;      // Element node
document.body;      // Element node
"Hello".nodeType;   // Text node

// Element nodes have special properties
document.body.tagName;  // "BODY"
document.body.nodeType; // 1 (Element)
document.body.childNodes; // NodeList of children
```

---

## 🔍 Accessing the DOM

### The document Object

```javascript
// The document object is the entry point
document;                    // The entire document
document.documentElement;    // The <html> element
document.head;              // The <head> element
document.body;              // The <body> element
document.title;             // The page title
document.URL;              // The page URL
```

### DOM Properties vs Methods

```javascript
// Properties (getters)
document.body;              // Returns element
document.images;            // Returns collection
document.forms;             // Returns collection

// Methods
document.getElementById('id');    // Find element by ID
document.querySelector('selector'); // Find first match
document.createElement('tag');     // Create new element
```

---

## 📝 Basic DOM Operations

### Reading DOM

```javascript
// Get element by ID
const header = document.getElementById('main-header');

// Get elements by class (returns HTMLCollection)
const cards = document.getElementsByClassName('card');

// Get elements by tag (returns HTMLCollection)
const paragraphs = document.getElementsByTagName('p');

// Query selector (returns first match)
const firstButton = document.querySelector('button');

// Query selector all (returns NodeList)
const allButtons = document.querySelectorAll('button');
```

### Modifying DOM

```javascript
const element = document.getElementById('myElement');

// Change text content
element.textContent = 'New text';

// Change HTML content
element.innerHTML = '<strong>Bold text</strong>';

// Change attributes
element.setAttribute('class', 'highlight');
element.id = 'newId';

// Change styles
element.style.color = 'red';
element.style.backgroundColor = 'yellow';
```

---

## 🎯 Quick Examples

### Example 1: Hello World

```html
<!DOCTYPE html>
<html>
<head>
    <title>DOM Demo</title>
</head>
<body>
    <h1 id="title">Hello!</h1>
    <script>
        const title = document.getElementById('title');
        title.textContent = 'Hello, World!';
    </script>
</body>
</html>
```

### Example 2: Toggle Visibility

```html
<!DOCTYPE html>
<html>
<body>
    <button id="toggleBtn">Hide</button>
    <p id="content">This content can be hidden or shown.</p>
    
    <script>
        const btn = document.getElementById('toggleBtn');
        const content = document.getElementById('content');
        
        btn.addEventListener('click', () => {
            if (content.style.display === 'none') {
                content.style.display = 'block';
                btn.textContent = 'Hide';
            } else {
                content.style.display = 'none';
                btn.textContent = 'Show';
            }
        });
    </script>
</body>
</html>
```

### Example 3: List Manager

```html
<!DOCTYPE html>
<html>
<body>
    <input type="text" id="itemInput" placeholder="Enter item">
    <button id="addBtn">Add</button>
    <ul id="itemList"></ul>
    
    <script>
        const input = document.getElementById('itemInput');
        const btn = document.getElementById('addBtn');
        const list = document.getElementById('itemList');
        
        btn.addEventListener('click', () => {
            const text = input.value.trim();
            if (text) {
                const li = document.createElement('li');
                li.textContent = text;
                list.appendChild(li);
                input.value = '';
            }
        });
    </script>
</body>
</html>
```

---

## 📊 DOM Summary

| Method/Property | Returns | Use Case |
|-----------------|---------|----------|
| `getElementById()` | Single element | Get by ID |
| `getElementsByClassName()` | Collection | Get by class |
| `getElementsByTagName()` | Collection | Get by tag |
| `querySelector()` | First element | CSS selector |
| `querySelectorAll()` | NodeList | All matches |
| `textContent` | string | Get/set text |
| `innerHTML` | string | Get/set HTML |
| `style` | object | Get/set CSS |

---

## 🔗 Next Steps

- [02_DOM_Selectors_Complete_Reference.md](./02_DOM_Selectors_Complete_Reference.md)
- [03_Creating_and_Adding_Elements.md](./03_Creating_and_Adding_Elements.md)

---

**Next: Learn about [DOM Selectors](./02_DOM_Selectors_Complete_Reference.md)**