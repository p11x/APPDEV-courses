# Structuring Large Express Apps

## 📌 What You'll Learn
- How to organize large Express applications
- Patterns for scaling
- Best practices for enterprise apps

## 🧠 Concept Explained (Plain English)

As your Express application grows, the way you organize code becomes crucial. What works for a small app becomes unmanageable at scale. Large applications need clear separation of concerns, layered architecture, and patterns that make the codebase maintainable.

Think of a small startup versus a large corporation. A small team can communicate informally, but a large organization needs departments, hierarchies, and clear processes. Similarly, large Express apps need structure, layers, and organization.

## Layered Architecture

```
┌─────────────────────────────────────┐
│         Routes (API Layer)          │
│    Handles HTTP requests/responses  │
├─────────────────────────────────────┤
│       Controllers (Business)        │
│   Application logic and flow        │
├─────────────────────────────────────┤
│        Services (Domain)            │
│    Core business logic/rules        │
├─────────────────────────────────────┤
│      Repositories (Data)            │
│      Database operations            │
└─────────────────────────────────────┘
```

## Complete Project Structure

```
src/
├── config/                    # Configuration
│   ├── database.js          # DB connection
│   ├── auth.js              # Auth config
│   └── index.js             # Config loader
├── routes/                   # Route definitions
│   ├── userRoutes.js
│   ├── productRoutes.js
│   └── index.js            # Route aggregator
├── controllers/              # Request handlers
│   ├── userController.js
│   └── productController.js
├── services/                 # Business logic
│   ├── userService.js
│   └── productService.js
├── repositories/             # Data access
│   ├── userRepository.js
│   └── productRepository.js
├── models/                   # Database models
│   ├── User.js
│   └── Product.js
├── middleware/              # Express middleware
│   ├── auth.js
│   ├── validation.js
│   └── errorHandler.js
├── utils/                   # Helper functions
│   ├── validation.js
│   └── helpers.js
├── app.js                   # Express setup
└── server.js                # Entry point
```

## Example: Separating Concerns

### Controller (handles HTTP)

```javascript
// controllers/userController.js
import userService from '../services/userService.js';

export const getUser = async (req, res) => {
    const user = await userService.findById(req.params.id);
    res.json(user);
};
```

### Service (business logic)

```javascript
// services/userService.js
import userRepository from '../repositories/userRepository.js';

export const findById = async (id) => {
    const user = await userRepository.findById(id);
    if (!user) throw new Error('User not found');
    return user;
};
```

### Repository (data access)

```javascript
// repositories/userRepository.js
import User from '../models/User.js';

export const findById = async (id) => {
    return await User.findById(id);
};
```

## Key Principles

| Principle | Description |
|-----------|-------------|
| **Single Responsibility** | Each file does one thing |
|