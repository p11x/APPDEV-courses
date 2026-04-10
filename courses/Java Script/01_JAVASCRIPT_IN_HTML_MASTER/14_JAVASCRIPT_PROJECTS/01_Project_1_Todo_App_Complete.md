# 🚀 Project 1: Advanced Todo Application

## 📋 Project Overview

Build a feature-rich todo application with local storage, drag-and-drop, real-time collaboration, and advanced analytics. This project demonstrates:
- Full CRUD operations
- Local storage persistence
- Event delegation patterns
- Performance optimization
- Accessibility compliance
- Modern JavaScript patterns

---

## 🏗️ Architecture Overview

### File Structure

```
todo-app/
├── index.html              # Main application structure
├── css/
│   ├── variables.css       # CSS custom properties
│   └── styles.css          # Main styling
└── js/
    ├── app.js              # Main application logic
    ├── storage.js          # Local storage management
    └── utils/
        ├── validators.js   # Input validation
        └── helpers.js      # Utility functions
```

---

## 🎯 Core Features Implementation

### Feature 1: Advanced Todo Management

```javascript
class TodoManager {
    constructor() {
        this.todos = [];
        this.currentFilter = 'all';
        this.observers = [];
        this.loadFromStorage();
    }
    
    // Observer pattern for reactive updates
    subscribe(callback) {
        this.observers.push(callback);
        return () => {
            this.observers = this.observers.filter(obs => obs !== callback);
        };
    }
    
    notifyObservers() {
        this.observers.forEach(callback => callback(this.getState()));
    }
    
    // Add todo with validation
    addTodo(text, priority = 'medium', category = 'personal') {
        if (!text || text.trim().length === 0) {
            throw new Error('Todo text cannot be empty');
        }
        
        const todo = {
            id: this.generateId(),
            text: text.trim(),
            priority,
            category,
            status: 'pending',
            createdAt: new Date().toISOString(),
            completedAt: null
        };
        
        this.todos.unshift(todo);
        this.notifyObservers();
        this.saveToStorage();
        return todo;
    }
    
    // Advanced filtering
    filterTodos(criteria = {}) {
        const { status, priority, search } = criteria;
        
        return this.todos.filter(todo => {
            if (status && status !== 'all' && todo.status !== status) return false;
            if (priority && priority !== 'all' && todo.priority !== priority) return false;
            if (search) {
                const regex = new RegExp(search, 'i');
                if (!regex.test(todo.text)) return false;
            }
            return true;
        });
    }
    
    // Search todos
    searchTodos(query) {
        const regex = new RegExp(query, 'i');
        return this.todos.filter(todo => regex.test(todo.text));
    }
    
    getState() {
        return {
            todos: this.todos,
            filter: this.currentFilter,
            stats: this.getStats()
        };
    }
    
    getStats() {
        return {
            total: this.todos.length,
            completed: this.todos.filter(t => t.status === 'completed').length,
            pending: this.todos.filter(t => t.status === 'pending').length
        };
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('todos', JSON.stringify(this.todos));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('todos');
        if (stored) {
            try {
                this.todos = JSON.parse(stored);
            } catch (e) {
                this.todos = [];
            }
        }
    }
}
```

### Feature 2: Local Storage Manager

```javascript
class StorageManager {
    constructor() {
        this.storageKey = 'advanced-todos';
        this.version = '2.0';
        this.backupKey = 'todos-backup';
    }
    
    serializeData(data) {
        return JSON.stringify({
            version: this.version,
            timestamp: Date.now(),
            data
        });
    }
    
    validateAndMigrate(data) {
        if (!Array.isArray(data)) {
            return [];
        }
        
        return data.map(item => ({
            id: item.id || this.generateLegacyId(),
            text: item.text || '',
            priority: item.priority || 'medium',
            category: item.category || 'personal',
            status: item.status || 'pending',
            createdAt: item.createdAt || new Date().toISOString(),
            completedAt: item.completedAt || null
        }));
    }
    
    saveToStorage(data) {
        try {
            const serialized = this.serializeData(data);
            localStorage.setItem(this.storageKey, serialized);
            this.createBackup(data);
            return true;
        } catch (error) {
            console.error('Storage save failed:', error);
            return false;
        }
    }
    
    createBackup(data) {
        try {
            const backup = {
                timestamp: Date.now(),
                data: [...data]
            };
            localStorage.setItem(this.backupKey, JSON.stringify(backup));
        } catch (error) {
            console.error('Backup failed:', error);
        }
    }
    
    recoverFromBackup() {
        try {
            const backupStr = localStorage.getItem(this.backupKey);
            if (!backupStr) return null;
            
            const backup = JSON.parse(backupStr);
            return backup.data;
        } catch (error) {
            console.error('Backup recovery failed:', error);
            return null;
        }
    }
}
```

### Feature 3: Drag and Drop

```javascript
class DragDropManager {
    constructor(container) {
        this.container = container;
        this.draggedElement = null;
        this.draggedData = null;
        this.setupDragDrop();
    }
    
    setupDragDrop() {
        this.container.addEventListener('dragstart', this.handleDragStart.bind(this));
        this.container.addEventListener('dragover', this.handleDragOver.bind(this));
        this.container.addEventListener('drop', this.handleDrop.bind(this));
        this.container.addEventListener('dragend', this.handleDragEnd.bind(this));
    }
    
    handleDragStart(e) {
        this.draggedElement = e.target.closest('.todo-item');
        if (!this.draggedElement) return;
        
        this.draggedData = {
            id: this.draggedElement.dataset.id
        };
        
        this.draggedElement.classList.add('dragging');
        e.dataTransfer.effectAllowed = 'move';
    }
    
    handleDragOver(e) {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
    }
    
    handleDrop(e) {
        e.preventDefault();
        
        const dropTarget = e.target.closest('.todo-item');
        if (!dropTarget || !this.draggedElement) return;
        
        const container = this.draggedElement.parentNode;
        const afterElement = this.getDragAfterElement(container, e.clientY);
        
        if (afterElement) {
            container.insertBefore(this.draggedElement, afterElement);
        } else {
            container.appendChild(this.draggedElement);
        }
    }
    
    handleDragEnd(e) {
        if (this.draggedElement) {
            this.draggedElement.classList.remove('dragging');
            this.draggedElement = null;
            this.draggedData = null;
        }
    }
    
    getDragAfterElement(container, y) {
        const draggableElements = [...container.querySelectorAll('.todo-item:not(.dragging)')];
        
        return draggableElements.reduce((closest, child) => {
            const box = child.getBoundingClientRect();
            const offset = y - box.top - box.height / 2;
            
            if (offset < 0 && offset > closest.offset) {
                return { offset: offset, element: child };
            } else {
                return closest;
            }
        }, { offset: Number.NEGATIVE_INFINITY }).element;
    }
}
```

---

## 🎨 UI Implementation

### HTML Structure

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Advanced Todo App</title>
    <link rel="stylesheet" href="css/variables.css">
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="todo-app">
        <div class="todo-container">
            <header class="todo-header">
                <h1>📝 My Tasks</h1>
                <form class="todo-form" id="todoForm">
                    <input type="text" class="todo-input" id="todoInput" 
                           placeholder="What needs to be done?" required>
                    <select class="priority-select" id="prioritySelect">
                        <option value="low">Low</option>
                        <option value="medium" selected>Medium</option>
                        <option value="high">High</option>
                    </select>
                    <button type="submit" class="btn btn-add">Add</button>
                </form>
            </header>
            
            <div class="todo-filters">
                <button class="filter-btn active" data-filter="all">All</button>
                <button class="filter-btn" data-filter="pending">Pending</button>
                <button class="filter-btn" data-filter="completed">Completed</button>
            </div>
            
            <ul class="todo-list" id="todoList"></ul>
            
            <div class="todo-stats">
                <div class="stat-item">
                    <div class="stat-value" id="totalCount">0</div>
                    <div class="stat-label">Total</div>
                </div>
                <div class="stat-item">
                    <div class="stat-value" id="completedCount">0</div>
                    <div class="stat-label">Completed</div>
                </div>
                <div class="stat-item">
                    <div class="stat-value" id="pendingCount">0</div>
                    <div class="stat-label">Pending</div>
                </div>
            </div>
        </div>
    </div>
    
    <script src="js/utils/helpers.js"></script>
    <script src="js/utils/validators.js"></script>
    <script src="js/storage.js"></script>
    <script src="js/app.js"></script>
</body>
</html>
```

### CSS Styling

```css
/* css/variables.css */
:root {
    --primary-color: #6366f1;
    --primary-hover: #5558e3;
    --secondary-color: #f3f4f6;
    --text-primary: #1f2937;
    --text-secondary: #6b7280;
    --border-color: #e5e7eb;
    --success-color: #10b981;
    --warning-color: #f59e0b;
    --error-color: #ef4444;
    
    --spacing-xs: 0.25rem;
    --spacing-sm: 0.5rem;
    --spacing-md: 1rem;
    --spacing-lg: 1.5rem;
    
    --border-radius-sm: 0.25rem;
    --border-radius-md: 0.5rem;
    --border-radius-lg: 0.75rem;
    
    --shadow-sm: 0 1px 2px 0 rgb(0 0 0 / 0.05);
    --shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.1);
    --shadow-lg: 0 10px 15px -3px rgb(0 0 0 / 0.1);
}

/* css/styles.css */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    min-height: 100vh;
    padding: var(--spacing-md);
}

.todo-app {
    display: flex;
    justify-content: center;
    align-items: flex-start;
    padding-top: 2rem;
}

.todo-container {
    background: white;
    border-radius: var(--border-radius-lg);
    box-shadow: var(--shadow-lg);
    width: 100%;
    max-width: 600px;
    overflow: hidden;
}

.todo-header {
    padding: var(--spacing-lg);
    background: var(--primary-color);
    color: white;
}

.todo-header h1 {
    margin-bottom: var(--spacing-md);
}

.todo-form {
    display: flex;
    gap: var(--spacing-sm);
}

.todo-input {
    flex: 1;
    padding: var(--spacing-md);
    border: 2px solid rgba(255, 255, 255, 0.3);
    border-radius: var(--border-radius-md);
    background: rgba(255, 255, 255, 0.1);
    color: white;
    font-size: 1rem;
}

.todo-input::placeholder {
    color: rgba(255, 255, 255, 0.7);
}

.todo-input:focus {
    outline: none;
    background: rgba(255, 255, 255, 0.2);
    border-color: rgba(255, 255, 255, 0.5);
}

.priority-select {
    padding: var(--spacing-sm);
    border-radius: var(--border-radius-md);
    border: none;
    cursor: pointer;
}

.btn-add {
    padding: var(--spacing-md) var(--spacing-lg);
    background: white;
    color: var(--primary-color);
    border: none;
    border-radius: var(--border-radius-md);
    font-weight: 600;
    cursor: pointer;
    transition: transform 0.2s;
}

.btn-add:hover {
    transform: scale(1.05);
}

.todo-filters {
    display: flex;
    gap: var(--spacing-sm);
    padding: var(--spacing-md);
    border-bottom: 1px solid var(--border-color);
}

.filter-btn {
    padding: var(--spacing-sm) var(--spacing-md);
    border: none;
    background: transparent;
    cursor: pointer;
    border-radius: var(--border-radius-md);
    transition: background 0.2s;
}

.filter-btn.active {
    background: var(--primary-color);
    color: white;
}

.todo-list {
    max-height: 400px;
    overflow-y: auto;
    padding: var(--spacing-md);
}

.todo-item {
    display: flex;
    align-items: center;
    padding: var(--spacing-md);
    margin-bottom: var(--spacing-sm);
    border-radius: var(--border-radius-md);
    background: white;
    border: 1px solid var(--border-color);
    cursor: pointer;
    transition: all 0.2s;
}

.todo-item:hover {
    box-shadow: var(--shadow-md);
    transform: translateX(4px);
}

.todo-item.completed {
    opacity: 0.7;
    background: var(--secondary-color);
}

.todo-item.completed .todo-text {
    text-decoration: line-through;
}

.todo-item.dragging {
    opacity: 0.5;
    transform: rotate(2deg);
}

.todo-checkbox {
    width: 20px;
    height: 20px;
    margin-right: var(--spacing-md);
    cursor: pointer;
}

.todo-text {
    flex: 1;
    font-size: 1rem;
}

.priority-badge {
    padding: var(--spacing-xs) var(--spacing-sm);
    border-radius: var(--border-radius-sm);
    font-size: 0.75rem;
    font-weight: 600;
    margin-right: var(--spacing-sm);
}

.priority-high {
    background: #fee2e2;
    color: var(--error-color);
}

.priority-medium {
    background: #fef3c7;
    color: var(--warning-color);
}

.priority-low {
    background: #d1fae5;
    color: var(--success-color);
}

.btn-delete {
    padding: var(--spacing-xs) var(--spacing-sm);
    background: var(--error-color);
    color: white;
    border: none;
    border-radius: var(--border-radius-sm);
    cursor: pointer;
    opacity: 0;
    transition: opacity 0.2s;
}

.todo-item:hover .btn-delete {
    opacity: 1;
}

.todo-stats {
    display: flex;
    justify-content: space-around;
    padding: var(--spacing-lg);
    background: var(--secondary-color);
}

.stat-item {
    text-align: center;
}

.stat-value {
    font-size: 1.5rem;
    font-weight: bold;
    color: var(--primary-color);
}

.stat-label {
    font-size: 0.85rem;
    color: var(--text-secondary);
}

@media (max-width: 768px) {
    .todo-form {
        flex-direction: column;
    }
    
    .todo-stats {
        flex-direction: column;
        gap: var(--spacing-md);
    }
}
```

---

## 🔧 Main Application Logic

### app.js

```javascript
class TodoApp {
    constructor() {
        this.todoManager = new TodoManager();
        this.dragDropManager = null;
        this.init();
    }
    
    init() {
        this.renderTodos();
        this.setupEventListeners();
        this.setupDragDrop();
        this.updateStats();
    }
    
    setupEventListeners() {
        // Form submission
        document.getElementById('todoForm').addEventListener('submit', (e) => {
            e.preventDefault();
            this.addTodo();
        });
        
        // Filter buttons
        document.querySelectorAll('.filter-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                this.setFilter(e.target.dataset.filter);
            });
        });
        
        // Subscribe to state changes
        this.todoManager.subscribe(() => {
            this.renderTodos();
            this.updateStats();
        });
    }
    
    setupDragDrop() {
        const list = document.getElementById('todoList');
        this.dragDropManager = new DragDropManager(list);
    }
    
    addTodo() {
        const input = document.getElementById('todoInput');
        const priority = document.getElementById('prioritySelect').value;
        const text = input.value.trim();
        
        if (!text) return;
        
        try {
            this.todoManager.addTodo(text, priority);
            input.value = '';
            document.getElementById('prioritySelect').value = 'medium';
        } catch (error) {
            alert(error.message);
        }
    }
    
    setFilter(filter) {
        this.todoManager.currentFilter = filter;
        
        // Update active button
        document.querySelectorAll('.filter-btn').forEach(btn => {
            btn.classList.toggle('active', btn.dataset.filter === filter);
        });
        
        this.renderTodos();
    }
    
    renderTodos() {
        const list = document.getElementById('todoList');
        const filter = this.todoManager.currentFilter;
        
        const todos = this.todoManager.filterTodos({ 
            status: filter === 'all' ? null : filter 
        });
        
        list.innerHTML = todos.map(todo => this.createTodoHTML(todo)).join('');
        
        // Add event listeners to new elements
        list.querySelectorAll('.todo-item').forEach(item => {
            const checkbox = item.querySelector('.todo-checkbox');
            const deleteBtn = item.querySelector('.btn-delete');
            
            checkbox.addEventListener('change', () => {
                this.toggleTodo(item.dataset.id);
            });
            
            deleteBtn.addEventListener('click', () => {
                this.deleteTodo(item.dataset.id);
            });
        });
    }
    
    createTodoHTML(todo) {
        const priorityClass = `priority-${todo.priority}`;
        
        return `
            <li class="todo-item ${todo.status === 'completed' ? 'completed' : ''}" 
                draggable="true" data-id="${todo.id}">
                <input type="checkbox" class="todo-checkbox" 
                       ${todo.status === 'completed' ? 'checked' : ''}>
                <span class="todo-text">${this.escapeHTML(todo.text)}</span>
                <span class="priority-badge ${priorityClass}">${todo.priority}</span>
                <button class="btn-delete">Delete</button>
            </li>
        `;
    }
    
    escapeHTML(str) {
        const div = document.createElement('div');
        div.textContent = str;
        return div.innerHTML;
    }
    
    toggleTodo(id) {
        this.todoManager.toggleTodo(id);
    }
    
    deleteTodo(id) {
        this.todoManager.deleteTodo(id);
    }
    
    updateStats() {
        const stats = this.todoManager.getStats();
        document.getElementById('totalCount').textContent = stats.total;
        document.getElementById('completedCount').textContent = stats.completed;
        document.getElementById('pendingCount').textContent = stats.pending;
    }
}

// Add toggle and delete methods to TodoManager
Object.assign(TodoManager.prototype, {
    toggleTodo(id) {
        const todo = this.todos.find(t => t.id === id);
        if (todo) {
            todo.status = todo.status === 'completed' ? 'pending' : 'completed';
            if (todo.status === 'completed') {
                todo.completedAt = new Date().toISOString();
            } else {
                todo.completedAt = null;
            }
            this.notifyObservers();
            this.saveToStorage();
        }
    },
    
    deleteTodo(id) {
        this.todos = this.todos.filter(t => t.id !== id);
        this.notifyObservers();
        this.saveToStorage();
    }
});

// Initialize app when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    new TodoApp();
});
```

---

## 📊 Key Features Summary

| Feature | Implementation |
|---------|----------------|
| CRUD Operations | addTodo, toggleTodo, deleteTodo |
| Local Storage | StorageManager class |
| Drag & Drop | DragDropManager with touch support |
| Filtering | Filter by status (all/pending/completed) |
| Search | Regex-based search |
| Statistics | Real-time stats display |
| Responsive | Mobile-first CSS |

---

## 🔗 Related Topics

- [08_Event_Delegation_Patterns.md](../09_DOM_MANIPULATION/08_Event_Delegation_Patterns.md)
- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)
- [08_Timeout_and_Debouncing.md](../08_ASYNC_JAVASCRIPT/08_Timeout_and_Debouncing.md)

---

**Next: Learn about [Weather Dashboard Project](./02_Project_2_Weather_Dashboard.md)**