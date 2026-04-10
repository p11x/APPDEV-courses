# 🎯 DOM Manipulation Basics

## 📋 Overview

This guide covers the essential techniques for creating, modifying, and removing DOM elements.

---

## 🔨 Creating Elements

### createElement

```javascript
// Create new element
const newDiv = document.createElement('div');
const newButton = document.createElement('button');
const newLink = document.createElement('a');

// Set properties
newDiv.id = 'container';
newDiv.className = 'card';
newDiv.textContent = 'Hello World';
newButton.textContent = 'Click Me';
newLink.href = 'https://example.com';
```

### createTextNode

```javascript
// Create text node (safer than innerHTML for user input)
const text = document.createTextNode('This is safe text');

// Combine with elements
const p = document.createElement('p');
p.appendChild(text);
```

---

## ➕ Adding Elements

### appendChild

```javascript
const parent = document.getElementById('container');
const child = document.createElement('div');
child.textContent = 'New child';

// Add as last child
parent.appendChild(child);

// Can move existing elements
const another = document.querySelector('.item');
parent.appendChild(another); // Moves it!
```

### append (Modern)

```javascript
const container = document.getElementById('container');

// Append multiple items
container.append(
    document.createElement('div'),
    document.createElement('span'),
    'Just text'
);

// append is more flexible than appendChild
```

### prepend

```javascript
const list = document.getElementById('list');
const newItem = document.createElement('li');
newItem.textContent = 'First item';

// Add as first child
list.prepend(newItem);
```

### insertBefore / insertAfter

```javascript
const parent = document.getElementById('list');
const newItem = document.createElement('li');
newItem.textContent = 'New';

const existingItem = document.querySelector('li:nth-child(2)');

// Insert before
parent.insertBefore(newItem, existingItem);

// Insert after (using nextSibling)
parent.insertBefore(newItem, existingItem.nextSibling);
```

### insertAdjacentHTML

```javascript
const element = document.getElementById('container');

// Insert before the element
element.insertAdjacentHTML('beforebegin', '<p>Before</p>');

// Insert inside, at the beginning
element.insertAdjacentHTML('afterbegin', '<p>First</p>');

// Insert inside, at the end
element.insertAdjacentHTML('beforeend', '<p>Last</p>');

// Insert after the element
element.insertAdjacentHTML('afterend', '<p>After</p>');
```

---

## ✏️ Modifying Elements

### textContent vs innerHTML

```javascript
const el = document.getElementById('demo');

// textContent - sets text only (safer)
el.textContent = '<strong>Bold</strong>'; // Shows literal text

// innerHTML - parses HTML
el.innerHTML = '<strong>Bold</strong>'; // Shows bold text
```

### Modifying Attributes

```javascript
const link = document.querySelector('a');

// Set attribute
link.setAttribute('href', 'https://new-url.com');
link.setAttribute('target', '_blank');

// Get attribute
console.log(link.getAttribute('href'));

// Remove attribute
link.removeAttribute('disabled');

// Check attribute
if (link.hasAttribute('disabled')) { }

// Shorthand for common attributes
link.href = 'https://new-url.com';
link.id = 'new-id';
```

### Modifying Styles

```javascript
const box = document.querySelector('.box');

// Direct style property
box.style.color = 'red';
box.style.backgroundColor = 'blue';
box.style.fontSize = '16px';
box.style.display = 'none';

// CSS classes (recommended)
box.classList.add('active');
box.classList.remove('hidden');
box.classList.toggle('selected');
box.classList.contains('active'); // true/false
```

---

## 🗑️ Removing Elements

### remove (Modern)

```javascript
const element = document.querySelector('.old-item');
element.remove(); // Removes from DOM
```

### removeChild (Legacy)

```javascript
const parent = document.getElementById('list');
const child = document.querySelector('.item');

parent.removeChild(child); // Returns removed element
```

---

## 🎯 Real-World Example

### Dynamic List Manager

```javascript
class ListManager {
    constructor(containerId) {
        this.container = document.getElementById(containerId);
    }
    
    addItem(text) {
        const li = document.createElement('li');
        li.className = 'list-item';
        
        const span = document.createElement('span');
        span.textContent = text;
        
        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '×';
        deleteBtn.className = 'delete-btn';
        deleteBtn.onclick = () => li.remove();
        
        li.appendChild(span);
        li.appendChild(deleteBtn);
        
        this.container.appendChild(li);
    }
    
    clearAll() {
        this.container.innerHTML = '';
    }
}

// Usage
const list = new ListManager('my-list');
list.addItem('First item');
list.addItem('Second item');
```

---

## 📊 Summary Table

| Method | Purpose |
|--------|---------|
| `createElement` | Create new element |
| `appendChild` | Add as last child |
| `append` | Add multiple items |
| `prepend` | Add as first child |
| `insertBefore` | Insert before element |
| `remove` | Remove element |
| `removeChild` | Remove child |
| `textContent` | Set/get text |
| `innerHTML` | Set/get HTML |
| `classList` | Manage CSS classes |

---

## 🔗 Related Topics

- [01_DOM_Introduction.md](./01_DOM_Introduction.md)
- [02_DOM_Selectors_Complete_Reference.md](./02_DOM_Selectors_Complete_Reference.md)

---

**Next: Learn about [DOM Styling](./05_DOM_Styling.md)**