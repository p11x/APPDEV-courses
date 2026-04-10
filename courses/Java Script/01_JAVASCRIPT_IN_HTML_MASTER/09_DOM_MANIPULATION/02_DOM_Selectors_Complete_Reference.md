# 🎯 DOM Selectors Complete Reference

## 📋 Overview

JavaScript provides multiple ways to select DOM elements. Understanding each selector method and when to use it is essential for efficient DOM manipulation.

---

## 🔍 Selector Methods Overview

| Method | Returns | Speed | CSS Selector |
|--------|---------|-------|--------------|
| `getElementById` | Element | Fastest | #id |
| `getElementsByClassName` | Collection | Fast | .class |
| `getElementsByTagName` | Collection | Fast | tag |
| `querySelector` | Element | Medium | Any CSS |
| `querySelectorAll` | NodeList | Medium | Any CSS |

---

## 🆔 getElementById

### Basic Usage

```javascript
// Single element by ID - FASTEST
const header = document.getElementById('header');
const nav = document.getElementById('main-nav');

// Returns null if not found
const missing = document.getElementById('not-exist'); // null
```

### Real-World Example

```html
<!DOCTYPE html>
<html>
<body>
    <div id="user-profile">
        <h1 id="user-name">John Doe</h1>
        <p id="user-email">john@example.com</p>
    </div>
    
    <script>
        const nameEl = document.getElementById('user-name');
        const emailEl = document.getElementById('user-email');
        
        // Modify content
        nameEl.textContent = 'Jane Smith';
        emailEl.textContent = 'jane@example.com';
        
        // Add styling
        nameEl.style.fontSize = '24px';
        nameEl.style.color = '#333';
    </script>
</body>
</html>
```

---

## 📂 getElementsByClassName

### Basic Usage

```javascript
// Returns HTMLCollection (live, array-like)
const cards = document.getElementsByClassName('card');
console.log(cards.length); // Number of elements

// Access by index
const firstCard = cards[0];
const secondCard = cards[1];
```

### Iterating Over Collection

```javascript
const items = document.getElementsByClassName('item');

// Using for loop
for (let i = 0; i < items.length; i++) {
    console.log(items[i].textContent);
}

// Using for...of (Array.from needed for some methods)
for (const item of items) {
    item.classList.add('active');
}
```

### Multiple Classes

```javascript
// Elements with ALL specified classes
const featuredCards = document.getElementsByClassName('card featured');

// Real-world: Apply to all elements with class
function hideAllWithClass(className) {
    const elements = document.getElementsByClassName(className);
    for (const el of elements) {
        el.style.display = 'none';
    }
}
```

---

## 🏷️ getElementsByTagName

### Basic Usage

```javascript
// Get all images
const images = document.getElementsByTagName('img');

// Get all links
const links = document.getElementsByTagName('a');

// Get all paragraphs
const paragraphs = document.getElementsByTagName('p');
```

### Common Use Cases

```javascript
// Count all links in page
const allLinks = document.getElementsByTagName('a');
console.log(`Found ${allLinks.length} links`);

// Add class to all images
const images = document.getElementsByTagName('img');
for (const img of images) {
    img.classList.add('img-responsive');
}

// Get all inputs
const inputs = document.getElementsByTagName('input');
```

---

## 🎨 querySelector

### Basic Usage

```javascript
// First matching element (or null)
const firstButton = document.querySelector('button');
const activeItem = document.querySelector('.active');
const navItem = document.querySelector('#main-nav');
```

### CSS Selector Examples

```javascript
// Descendant selector
const linkInNav = document.querySelector('nav a');

// Child selector
const firstListItem = document.querySelector('ul > li');

// Attribute selector
const emailInput = document.querySelector('input[type="email"]');

// Pseudo-class
const firstParagraph = document.querySelector('p:first-child');
const checkedBox = document.querySelector('input:checked');
```

### Real-World Example

```javascript
// Form handling
const form = document.querySelector('#contact-form');
const submitBtn = form.querySelector('button[type="submit"]');
const emailInput = form.querySelector('input[name="email"]');

// Navigation
const activeLink = document.querySelector('.nav-link.active');
```

---

## 📋 querySelectorAll

### Basic Usage

```javascript
// Returns NodeList of ALL matches
const allButtons = document.querySelectorAll('button');
const allCards = document.querySelectorAll('.card');

// NodeList is NOT live (static snapshot)
const items = document.querySelectorAll('.item');
// Adding new .item won't appear in items
```

### Iterating

```javascript
const items = document.querySelectorAll('.item');

// forEach (NodeList has forEach)
items.forEach(item => {
    item.classList.add('processed');
});

// for...of loop
for (const item of items) {
    console.log(item.textContent);
}

// Convert to Array for more methods
const array = Array.from(items);
const filtered = array.filter(item => item.textContent.length > 10);
```

### Advanced Selectors

```javascript
// Multiple selectors
const inputsAndButtons = document.querySelectorAll('input, button');

// Complex selectors
const oddRows = document.querySelectorAll('tr:nth-child(odd)');
const dataAttrs = document.querySelectorAll('[data-value]');
const externalLinks = document.querySelectorAll('a[href^="http"]');
```

---

## ⚡ Performance Optimization

### Selector Speed Ranking

```javascript
// FASTEST to SLOWEST:
1. getElementById('#id')
2. getElementsByClassName('.class')
3. getElementsByTagName('tag')
4. querySelector('#id') - optimized for ID
5. querySelector('.class') - good
6. querySelector('tag') - good
7. querySelectorAll('complex selector') - slowest
```

### Optimization Tips

```javascript
// ❌ Bad: Multiple DOM queries
function badWay() {
    document.querySelectorAll('.item').forEach(el => el.classList.add('a'));
    document.querySelectorAll('.item').forEach(el => el.style.color = 'red');
    document.querySelectorAll('.item').forEach(el => el.textContent = 'x');
}

// ✅ Good: Cache the selector
function goodWay() {
    const items = document.querySelectorAll('.item');
    items.forEach(el => {
        el.classList.add('a');
        el.style.color = 'red';
        el.textContent = 'x';
    });
}

// ✅ Best: Use specific selectors
// Instead of: document.querySelectorAll('.item .sub-item')
// Use: document.querySelectorAll('.sub-item')
```

---

## 🎯 Real-World Application

### Todo List Manager

```javascript
class TodoList {
    constructor() {
        // Cache selectors for performance
        this.listContainer = document.querySelector('#todo-list');
        this.input = document.querySelector('#todo-input');
        this.addBtn = document.querySelector('#add-btn');
        
        this.setupEvents();
    }
    
    addTodo(text) {
        const li = document.createElement('li');
        li.className = 'todo-item';
        li.innerHTML = `
            <span>${text}</span>
            <button class="delete-btn">Delete</button>
        `;
        this.listContainer.appendChild(li);
    }
    
    setupEvents() {
        this.addBtn.addEventListener('click', () => {
            const text = this.input.value.trim();
            if (text) {
                this.addTodo(text);
                this.input.value = '';
            }
        });
    }
}
```

---

## 📊 Quick Reference

| Method | Returns | Live? | Speed |
|--------|---------|-------|-------|
| `getElementById` | Element | No | Fastest |
| `getElementsByClassName` | Collection | Yes | Fast |
| `getElementsByTagName` | Collection | Yes | Fast |
| `querySelector` | Element | No | Medium |
| `querySelectorAll` | NodeList | No | Medium |

---

## 🔗 Related Topics

- [01_DOM_Introduction.md](./01_DOM_Introduction.md)
- [03_Creating_and_Adding_Elements.md](./03_Creating_and_Adding_Elements.md)

---

**Next: Learn about [Creating and Adding Elements](./03_Creating_and_Adding_Elements.md)**