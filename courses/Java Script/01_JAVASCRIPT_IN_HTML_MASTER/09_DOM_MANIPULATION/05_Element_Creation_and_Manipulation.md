# 🔨 Element Creation and Manipulation

## 📋 Overview

Creating and manipulating DOM elements dynamically is essential for building interactive web applications. This guide covers techniques for efficiently creating, modifying, and managing DOM elements.

---

## 🏗️ Creating Elements

### createElement

```javascript
// Create new elements
const div = document.createElement('div');
const button = document.createElement('button');
const link = document.createElement('a');

// Set properties
div.id = 'container';
div.className = 'card active';
div.textContent = 'Hello World';
```

### createTextNode

```javascript
// Safer than innerHTML for user input
const text = document.createTextNode('User input text');
const p = document.createElement('p');
p.appendChild(text);
```

### cloneNode

```javascript
// Clone an element
const original = document.querySelector('.card');
const clone = original.cloneNode(true); // Deep clone (with children)
const shallow = original.cloneNode(false); // Shallow (no children)

// Use for templates
const template = document.querySelector('.template');
const copy = template.cloneNode(true);
copy.querySelector('.title').textContent = 'New Item';
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
```

### append (Modern)

```javascript
// Add multiple items at once
const container = document.querySelector('#list');
container.append(
    document.createElement('li'),
    document.createElement('li'),
    'Text node' // Can add text directly
);
```

### prepend

```javascript
// Add as first child
const list = document.querySelector('#list');
const newItem = document.createElement('li');
newItem.textContent = 'First!';
list.prepend(newItem);
```

### insertBefore / insertAfter

```javascript
const parent = document.querySelector('#list');
const newItem = document.createElement('li');
const existingItem = parent.querySelector('li:nth-child(2)');

// Insert before
parent.insertBefore(newItem, existingItem);

// Insert after
parent.insertBefore(newItem, existingItem.nextSibling);
```

### insertAdjacentHTML

```javascript
const element = document.querySelector('#container');

// Insert positions
element.insertAdjacentHTML('beforebegin', '<p>Before element</p>');
element.insertAdjacentHTML('afterbegin', '<p>First child</p>');
element.insertAdjacentHTML('beforeend', '<p>Last child</p>');
element.insertAdjacentHTML('afterend', '<p>After element</p>');
```

---

## ✏️ Modifying Elements

### textContent vs innerHTML

```javascript
const el = document.querySelector('#demo');

// textContent - safe, sets text only
el.textContent = '<strong>Bold</strong>'; // Shows as literal text

// innerHTML - parses HTML (be careful with user input!)
el.innerHTML = '<strong>Bold</strong>'; // Renders bold
```

### Dataset (data-* attributes)

```javascript
const element = document.createElement('div');
element.dataset.userId = '123';
element.dataset.role = 'admin';

// Access
console.log(element.dataset.userId); // "123"
console.log(element.dataset.role);   // "admin"
```

---

## 🗑️ Removing Elements

### remove (Modern)

```javascript
const element = document.querySelector('.old');
element.remove(); // Removes from DOM
```

### removeChild (Legacy)

```javascript
const parent = document.querySelector('#container');
const child = parent.querySelector('.item');
parent.removeChild(child);
```

---

## 🎯 Real-World Example

### Dynamic Todo List

```javascript
class TodoList {
    constructor(containerId) {
        this.container = document.getElementById(containerId);
    }
    
    addItem(text) {
        const li = document.createElement('li');
        li.className = 'todo-item';
        li.dataset.created = new Date().toISOString();
        
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.addEventListener('change', () => {
            li.classList.toggle('completed', checkbox.checked);
        });
        
        const span = document.createElement('span');
        span.textContent = text;
        
        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '×';
        deleteBtn.className = 'delete-btn';
        deleteBtn.addEventListener('click', () => li.remove());
        
        li.appendChild(checkbox);
        li.appendChild(span);
        li.appendChild(deleteBtn);
        
        this.container.appendChild(li);
    }
    
    clear() {
        this.container.innerHTML = '';
    }
}
```

---

## 🔗 Related Topics

- [01_DOM_Introduction.md](./01_DOM_Introduction.md)
- [02_DOM_Selectors_Complete_Reference.md](./02_DOM_Selectors_Complete_Reference.md)

---

**Next: Learn about [Event Handling Deep Dive](./06_Event_Handling_Deep_Dive.md)**