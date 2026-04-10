# ⚡ Event Handling Deep Dive

## 📋 Overview

Understanding event handling deeply is crucial for building responsive web applications. This guide covers advanced event patterns, custom events, and best practices.

---

## 🎯 Event Object Properties

### Key Properties

```javascript
element.addEventListener('click', function(event) {
    // Target - element that triggered event
    console.log(event.target);
    
    // CurrentTarget - element with listener
    console.log(event.currentTarget);
    
    // Type - event name
    console.log(event.type); // "click"
    
    // Timestamp
    console.log(event.timeStamp);
    
    // Default prevented
    console.log(event.defaultPrevented);
    
    // Stop propagation
    event.stopPropagation();
    event.stopImmediatePropagation();
});
```

---

## 🖱️ Mouse Events

### All Mouse Events

```javascript
element.addEventListener('mousedown', (e) => {
    console.log('Mouse down:', e.button); // 0=left, 1=middle, 2=right
});

element.addEventListener('mouseup', (e) => console.log('Mouse up'));
element.addEventListener('click', (e) => console.log('Click'));
element.addEventListener('dblclick', (e) => console.log('Double click'));
element.addEventListener('contextmenu', (e) => {
    e.preventDefault(); // Prevent context menu
    console.log('Right click');
});

element.addEventListener('mouseenter', (e) => console.log('Entered')); // No bubble
element.addEventListener('mouseleave', (e) => console.log('Left')); // No bubble
element.addEventListener('mouseover', (e) => console.log('Over')); // Bubbles
element.addEventListener('mouseout', (e) => console.log('Out')); // Bubbles
```

### Mouse Position

```javascript
element.addEventListener('mousemove', (e) => {
    // Relative to viewport
    console.log('Client:', e.clientX, e.clientY);
    
    // Relative to element
    console.log('Offset:', e.offsetX, e.offsetY);
    
    // Relative to document
    console.log('Page:', e.pageX, e.pageY);
});
```

---

## ⌨️ Keyboard Events

### Key Events

```javascript
document.addEventListener('keydown', (e) => {
    console.log('Key:', e.key);
    console.log('Code:', e.code); // Physical key
    console.log('KeyCode:', e.keyCode); // Deprecated but useful
    
    // Modifiers
    console.log('Shift:', e.shiftKey);
    console.log('Ctrl:', e.ctrlKey);
    console.log('Alt:', e.altKey);
    console.log('Meta:', e.metaKey); // Windows/Cmd
});

document.addEventListener('keyup', (e) => console.log('Key up'));
document.addEventListener('keypress', (e) => console.log('Key press'));
```

### Practical Example

```javascript
function handleKeyboardShortcut(e) {
    // Ctrl+S to save
    if (e.ctrlKey && e.key === 's') {
        e.preventDefault();
        saveDocument();
    }
    
    // Escape to close modal
    if (e.key === 'Escape') {
        closeModal();
    }
    
    // Arrow keys for navigation
    if (e.key === 'ArrowRight') {
        moveSelection('next');
    }
}
```

---

## 📝 Form Events

### Form Event Types

```javascript
const form = document.querySelector('form');
const input = document.querySelector('input');

form.addEventListener('submit', (e) => {
    e.preventDefault(); // Prevent submission
    console.log('Form submitted');
});

input.addEventListener('focus', () => console.log('Input focused'));
input.addEventListener('blur', () => console.log('Input blurred'));
input.addEventListener('input', (e) => {
    console.log('Input value:', e.target.value); // Fires on every change
});
input.addEventListener('change', (e) => {
    console.log('Value changed:', e.target.value); // Fires on blur
});
```

---

## 🎯 Custom Events

### Creating Custom Events

```javascript
// Basic custom event
const event = new CustomEvent('myEvent', {
    detail: { message: 'Hello!' }
});

element.dispatchEvent(event);

// Listen for custom event
element.addEventListener('myEvent', (e) => {
    console.log(e.detail.message);
});
```

### Practical Use Case

```javascript
class EventEmitter {
    constructor() {
        this.events = {};
    }
    
    on(event, callback) {
        if (!this.events[event]) {
            this.events[event] = [];
        }
        this.events[event].push(callback);
    }
    
    emit(event, data) {
        if (this.events[event]) {
            this.events[event].forEach(cb => cb(data));
        }
    }
}

// Usage
const emitter = new EventEmitter();
emitter.on('userLogin', (user) => {
    console.log('User logged in:', user.name);
});
emitter.emit('userLogin', { name: 'John' });
```

---

## 🔗 Related Topics

- [03_DOM_Event_Handling.md](./03_DOM_Event_Handling.md)
- [07_Event_Propagation_and_Bubbling.md](./07_Event_Propagation_and_Bubbling.md)

---

**Next: Learn about [Event Propagation and Bubbling](./07_Event_Propagation_and_Bubbling.md)**