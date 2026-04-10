# 🔄 Test Driven Development (TDD)

## 📋 Overview

Test Driven Development (TDD) is a development approach where tests are written before the code. The cycle is: Red → Green → Refactor.

---

## 🎯 The TDD Cycle

```
┌─────────────────────────────────────────┐
│           TDD CYCLE                      │
├─────────────────────────────────────────┤
│                                          │
│   1. RED   → Write failing test        │
│              ↓                           │
│   2. GREEN → Write minimum code         │
│              to pass test                │
│              ↓                           │
│   3. REFACTOR → Improve code           │
│              while keeping tests         │
│              passing                     │
│              ↓                           │
│         Repeat cycle                    │
│                                          │
└─────────────────────────────────────────┘
```

---

## 🎯 TDD in Practice

### Step 1: Write Failing Test (Red)

```javascript
// calculator.test.js
const Calculator = require('./calculator');

describe('Calculator', () => {
    describe('add', () => {
        it('should add two positive numbers', () => {
            const calc = new Calculator();
            expect(calc.add(2, 3)).toBe(5);
        });
    });
});

// Run test → FAILS (No Calculator class exists!)
```

### Step 2: Write Minimum Code (Green)

```javascript
// calculator.js
class Calculator {
    add(a, b) {
        return 5; // Hardcoded to pass test
    }
}

module.exports = Calculator;

// Run test → PASSES
```

### Step 3: Refactor

```javascript
// calculator.js - Proper implementation
class Calculator {
    add(a, b) {
        return a + b; // Real implementation
    }
    
    subtract(a, b) {
        return a - b;
    }
    
    multiply(a, b) {
        return a * b;
    }
}

module.exports = Calculator;

// Run tests → Still pass!
```

---

## 🎯 TDD Example: Todo List

### 1. Write Test First

```javascript
// todo.test.js
const TodoList = require('./todo');

describe('TodoList', () => {
    let todoList;
    
    beforeEach(() => {
        todoList = new TodoList();
    });
    
    describe('add', () => {
        it('should add a new todo', () => {
            const todo = todoList.add('Buy milk');
            expect(todo.text).toBe('Buy milk');
            expect(todoList.getAll()).toHaveLength(1);
        });
        
        it('should generate unique id', () => {
            const todo1 = todoList.add('Task 1');
            const todo2 = todoList.add('Task 2');
            expect(todo1.id).not.toBe(todo2.id);
        });
        
        it('should default status to pending', () => {
            const todo = todoList.add('New task');
            expect(todo.status).toBe('pending');
        });
    });
    
    describe('complete', () => {
        it('should mark todo as completed', () => {
            const todo = todoList.add('Task');
            todoList.complete(todo.id);
            expect(todoList.get(todo.id).status).toBe('completed');
        });
    });
});
```

### 2. Implement Minimum Code

```javascript
// todo.js
class TodoList {
    constructor() {
        this.todos = [];
    }
    
    add(text) {
        const todo = {
            id: Math.random().toString(36).substr(2, 9),
            text,
            status: 'pending'
        };
        this.todos.push(todo);
        return todo;
    }
    
    getAll() {
        return this.todos;
    }
    
    get(id) {
        return this.todos.find(t => t.id === id);
    }
    
    complete(id) {
        const todo = this.get(id);
        if (todo) {
            todo.status = 'completed';
        }
    }
}

module.exports = TodoList;
```

---

## 🎯 Benefits of TDD

| Benefit | Description |
|---------|-------------|
| **Better Design** | Forces thinking about API first |
| **Fewer Bugs** | Tests catch issues early |
| **Confidence** | Refactor without fear |
| **Documentation** | Tests document expected behavior |
| **Regression Prevention** | Catches breaking changes |

---

## 🎯 Common TDD Mistakes

1. **Writing too much test first** - Focus on one behavior at a time
2. **Skipping refactoring** - Keep code clean
3. **Not running tests frequently** - Run after each small change
4. **Testing implementation details** - Test behavior, not internals

---

## 🔗 Related Topics

- [02_Unit_Testing_Master_Class.md](./02_Unit_Testing_Master_Class.md)
- [18_Integration_Testing.md](./18_Integration_Testing.md)

---

**Next: [Mocking and Stubbing](./21_Mocking_and_Stubbing.md)**