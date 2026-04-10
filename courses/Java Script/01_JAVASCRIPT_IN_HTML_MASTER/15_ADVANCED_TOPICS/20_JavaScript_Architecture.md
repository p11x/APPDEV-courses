# 🏗️ JavaScript Architecture

## 📋 Overview

This guide covers architectural patterns and practices for building scalable JavaScript applications.

---

## 🎯 Application Architecture

### Layered Architecture

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│    (UI Components, Views)         │
├─────────────────────────────────────┤
│         Business Logic Layer        │
│    (Services, Use Cases)           │
├─────────────────────────────────────┤
│           Data Layer                │
│    (Repositories, API Clients)      │
├─────────────────────────────────────┤
│         Infrastructure              │
│    (Database, External Services)    │
└─────────────────────────────────────┘
```

### MVC Pattern

```javascript
// Model - Data and business logic
class UserModel {
    constructor() {
        this.users = [];
    }
    
    add(user) {
        this.users.push(user);
    }
    
    find(id) {
        return this.users.find(u => u.id === id);
    }
}

// View - UI rendering
class UserView {
    render(users) {
        return users.map(u => `<li>${u.name}</li>`).join('');
    }
}

// Controller - Coordination
class UserController {
    constructor(model, view) {
        this.model = model;
        this.view = view;
    }
    
    addUser(name) {
        const user = { id: Date.now(), name };
        this.model.add(user);
        return this.view.render(this.model.users);
    }
}
```

### Clean Architecture

```javascript
// Domain entities (core business logic)
class Order {
    constructor(items, customer) {
        this.items = items;
        this.customer = customer;
        this.status = 'pending';
    }
    
    calculateTotal() {
        return this.items.reduce((sum, item) => sum + item.price, 0);
    }
}

// Use cases (application business rules)
class CreateOrderUseCase {
    constructor(orderRepository, emailService) {
        this.orderRepository = orderRepository;
        this.emailService = emailService;
    }
    
    execute(orderData) {
        const order = new Order(orderData.items, orderData.customer);
        
        // Validate
        if (!orderData.items.length) {
            throw new Error('Order must have items');
        }
        
        // Save
        const savedOrder = this.orderRepository.save(order);
        
        // Notify
        this.emailService.sendConfirmation(orderData.customer.email);
        
        return savedOrder;
    }
}

// Interface adapters (controllers, presenters)
class OrderController {
    constructor(createOrderUseCase) {
        this.createOrderUseCase = createOrderUseCase;
    }
    
    handleRequest(req) {
        try {
            const order = this.createOrderUseCase.execute(req.body);
            return { status: 201, data: order };
        } catch (error) {
            return { status: 400, error: error.message };
        }
    }
}
```

---

## 🎯 Component Architecture

```javascript
// Base component
class Component {
    constructor(element) {
        this.element = element;
    }
    
    render() { }
    
    mount() { }
    
    unmount() { }
}

// Smart vs Dumb components
// Dumb - only render, no logic
const Button = ({ text, onClick }) => (
    `<button onclick="${onClick}">${text}</button>`
);

// Smart - has state and logic
class Counter extends Component {
    constructor(element) {
        super(element);
        this.state = { count: 0 };
    }
    
    increment() {
        this.setState({ count: this.state.count + 1 });
    }
    
    render() {
        return `<button>Count: ${this.state.count}</button>`;
    }
}
```

---

## 🔗 Related Topics

- [19_SOLID_Principles_JavaScript.md](./19_SOLID_Principles_JavaScript.md)
- [18_Design_Patterns_Complete.md](./18_Design_Patterns_Complete.md)

---

**Next: [Memory Management](./22_Memory_Management.md)**