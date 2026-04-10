# 🎯 Event Delegation Patterns

## 📋 Overview

Event delegation is a powerful pattern that allows handling events on multiple child elements using a single parent event listener. This is essential for performance and managing dynamic content.

---

## 🏗️ What is Event Delegation?

Instead of adding event listeners to each child element, you add one listener to the parent and handle events as they bubble up.

```javascript
// ❌ Bad: Multiple listeners (Performance Killer)
document.querySelectorAll('.button').forEach(btn => {
    btn.addEventListener('click', handleClick);
});
// Creates 100 listeners if there are 100 buttons!

// ✅ Good: Single listener (Performance Optimized)
document.querySelector('.button-container').addEventListener('click', handleClick);

function handleClick(event) {
    if (event.target.classList.contains('button')) {
        const buttonId = event.target.dataset.id;
        handleButtonClick(buttonId);
    }
}
```

---

## 🌍 Real-World Scenarios

### Dynamic Todo List

```javascript
class TodoApp {
    constructor(containerId) {
        this.container = document.getElementById(containerId);
        this.setupEventListeners();
    }
    
    setupEventListeners() {
        // Single delegated listener handles all todo interactions
        this.container.addEventListener('click', (e) => {
            const todoItem = e.target.closest('.todo-item');
            if (!todoItem) return;
            
            // Determine what was clicked
            if (e.target.classList.contains('delete-btn')) {
                this.deleteTodo(todoItem);
            } else if (e.target.classList.contains('toggle-btn')) {
                this.toggleTodo(todoItem);
            } else if (e.target.classList.contains('edit-btn')) {
                this.editTodo(todoItem);
            }
        });
        
        // For checkbox changes
        this.container.addEventListener('change', (e) => {
            if (e.target.type === 'checkbox') {
                const todoItem = e.target.closest('.todo-item');
                todoItem.classList.toggle('completed', e.target.checked);
            }
        });
    }
    
    addTodo(text) {
        const li = document.createElement('li');
        li.className = 'todo-item';
        li.innerHTML = `
            <input type="checkbox" class="toggle-btn">
            <span>${text}</span>
            <button class="edit-btn">Edit</button>
            <button class="delete-btn">×</button>
        `;
        this.container.appendChild(li);
    }
    
    deleteTodo(element) {
        element.remove();
    }
    
    toggleTodo(element) {
        element.classList.toggle('completed');
    }
    
    editTodo(element) {
        const span = element.querySelector('span');
        const newText = prompt('Edit todo:', span.textContent);
        if (newText) span.textContent = newText;
    }
}
```

### Table with Actions

```javascript
class TableManager {
    constructor(tableId) {
        this.table = document.getElementById(tableId);
        this.setupDelegation();
    }
    
    setupDelegation() {
        this.table.addEventListener('click', (e) => {
            // Use closest() to handle clicks on child elements
            const row = e.target.closest('tr');
            if (!row || !row.dataset.id) return;
            
            if (e.target.classList.contains('edit-btn')) {
                this.editRow(row.dataset.id);
            } else if (e.target.classList.contains('delete-btn')) {
                this.deleteRow(row.dataset.id);
            } else if (e.target.classList.contains('view-btn')) {
                this.viewRow(row.dataset.id);
            }
        });
    }
    
    editRow(id) { console.log('Edit:', id); }
    deleteRow(id) { console.log('Delete:', id); }
    viewRow(id) { console.log('View:', id); }
}
```

---

## 🎯 Benefits of Event Delegation

| Benefit | Description |
|---------|-------------|
| **Memory** | Single listener vs hundreds |
| **Dynamic Content** | Works with elements added later |
| **Code Simplicity** | One place to manage events |
| **Performance** | Fewer event handlers |

---

## 🔧 Implementation Tips

### Using closest()

```javascript
// Always use closest() to ensure you're targeting the right element
container.addEventListener('click', (e) => {
    // Get the closest element with this class
    const button = e.target.closest('.button');
    
    if (button) {
        // Handle click
    }
});
```

### Filtering by Data Attributes

```javascript
element.addEventListener('click', (e) => {
    const action = e.target.dataset.action;
    
    switch(action) {
        case 'save':
            handleSave();
            break;
        case 'delete':
            handleDelete();
            break;
        case 'edit':
            handleEdit();
            break;
    }
});
```

---

## 🔗 Related Topics

- [03_DOM_Event_Handling.md](./03_DOM_Event_Handling.md)
- [06_Event_Handling_Deep_Dive.md](./06_Event_Handling_Deep_Dive.md)
- [07_Event_Propagation_and_Bubbling.md](./07_Event_Propagation_and_Bubbling.md)

---

**Next: Learn about [DOM Traversal Techniques](./09_DOM_Traversal_Techniques.md)**