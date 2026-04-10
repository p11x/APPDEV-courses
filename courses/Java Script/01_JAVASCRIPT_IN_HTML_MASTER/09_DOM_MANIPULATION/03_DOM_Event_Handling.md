# 🎯 DOM Event Handling

## 📋 Overview

Event handling allows JavaScript to respond to user interactions. Events can be clicks, key presses, mouse movements, form submissions, and more.

---

## 🎪 Event Basics

### The Event Flow

```
┌─────────────────────────────────────┐
│           WINDOW                    │
│  ┌─────────────────────────────┐   │
│  │         DOCUMENT            │   │
│  │  ┌─────────────────────┐    │   │
│  │  │       <body>        │    │   │
│  │  │  ┌───────────────┐  │    │   │
│  │  │  │    <button>   │  │    │   │
│  │  │  └───────────────┘  │    │   │
│  │  └─────────────────────┘    │   │
│  └─────────────────────────────┘   │
└─────────────────────────────────────┘

Capturing Phase:  Window → Document → Body → Button
Target Phase:     Click happens on button
Bubbling Phase:   Button → Body → Document → Window
```

---

## 👂 Event Listeners

### addEventListener

```javascript
// Basic syntax
element.addEventListener(eventType, handlerFunction);

// Click event
const button = document.querySelector('#myButton');

button.addEventListener('click', function(event) {
    console.log('Button clicked!');
});

// Named function
function handleClick(event) {
    console.log('Clicked:', event.target);
}

button.addEventListener('click', handleClick);

// Remove listener
button.removeEventListener('click', handleClick);
```

### Event Object

```javascript
button.addEventListener('click', function(event) {
    // event type
    console.log(event.type); // "click"
    
    // target element
    console.log(event.target); // The element that was clicked
    
    // current target (where handler is attached)
    console.log(event.currentTarget); // button
    
    // prevent default behavior
    event.preventDefault();
    
    // stop propagation
    event.stopPropagation();
});
```

---

## ⌨️ Common Events

### Mouse Events

```javascript
const box = document.querySelector('.box');

box.addEventListener('click', () => console.log('click'));
box.addEventListener('dblclick', () => console.log('dblclick'));
box.addEventListener('mousedown', () => console.log('mousedown'));
box.addEventListener('mouseup', () => console.log('mouseup'));
box.addEventListener('mouseover', () => console.log('mouseover'));
box.addEventListener('mouseout', () => console.log('mouseout'));
box.addEventListener('mousemove', (e) => {
    console.log('X:', e.clientX, 'Y:', e.clientY);
});
```

### Keyboard Events

```javascript
document.addEventListener('keydown', (e) => {
    console.log('Key:', e.key);
    console.log('Code:', e.code);
    console.log('Shift:', e.shiftKey);
    console.log('Ctrl:', e.ctrlKey);
});

document.addEventListener('keyup', (e) => {
    console.log('Key released:', e.key);
});

document.addEventListener('keypress', (e) => {
    console.log('Character typed:', e.key);
});
```

### Form Events

```javascript
const form = document.querySelector('form');

form.addEventListener('submit', (e) => {
    e.preventDefault(); // Prevent page reload
    console.log('Form submitted!');
});

const input = document.querySelector('input');

input.addEventListener('focus', () => console.log('Focus'));
input.addEventListener('blur', () => console.log('Blur'));
input.addEventListener('input', (e) => console.log('Input:', e.target.value));
input.addEventListener('change', (e) => console.log('Changed:', e.target.value));
```

---

## 🎯 Event Delegation

### Concept

Instead of adding listeners to each child, add one to the parent.

```javascript
// ❌ Bad: Add listeners to each item
document.querySelectorAll('.item').forEach(item => {
    item.addEventListener('click', handleClick);
});

// ✅ Good: Delegate to parent
document.querySelector('.list').addEventListener('click', (e) => {
    // Check if clicked element is an item
    if (e.target.classList.contains('item')) {
        handleClick(e);
    }
});
```

### Real-World Example

```javascript
// Todo list with delegation
const todoList = document.getElementById('todo-list');

todoList.addEventListener('click', (e) => {
    const li = e.target.closest('li');
    
    if (e.target.classList.contains('delete-btn')) {
        li.remove();
    } else if (e.target.classList.contains('toggle-btn')) {
        li.classList.toggle('completed');
    }
});
```

---

## ⚠️ Event Best Practices

### Memory Leaks

```javascript
// ❌ Problem: Storing references prevents garbage collection
function setup() {
    const handler = () => console.log('click');
    element.addEventListener('click', handler);
    // No way to remove if element is removed from DOM
}

// ✅ Solution: Store and remove
function setup() {
    const handler = () => console.log('click');
    element.addEventListener('click', handler);
    
    // Remove when done
    element.removeEventListener('click', handler);
}
```

### Passive Listeners

```javascript
// ✅ For scroll events - improves performance
element.addEventListener('scroll', handler, { passive: true });

// ❌ Don't use if you need preventDefault
element.addEventListener('touchstart', handler, { passive: false });
```

---

## 🎯 Practice Exercise

### Interactive Form

```javascript
function setupFormValidation() {
    const form = document.getElementById('signup-form');
    const email = form.querySelector('input[type="email"]');
    const password = form.querySelector('input[type="password"]');
    
    const validateEmail = () => {
        const isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value);
        email.classList.toggle('error', !isValid);
    };
    
    const validatePassword = () => {
        const isValid = password.value.length >= 8;
        password.classList.toggle('error', !isValid);
    };
    
    email.addEventListener('blur', validateEmail);
    password.addEventListener('blur', validatePassword);
    
    form.addEventListener('submit', (e) => {
        validateEmail();
        validatePassword();
        
        if (form.querySelectorAll('.error').length > 0) {
            e.preventDefault();
        }
    });
}
```

---

## 🔗 Related Topics

- [01_DOM_Introduction.md](./01_DOM_Introduction.md)
- [02_DOM_Selectors_Complete_Reference.md](./02_DOM_Selectors_Complete_Reference.md)

---

**Next: Learn about [DOM Manipulation Basics](./04_DOM_Manipulation_Basics.md)**