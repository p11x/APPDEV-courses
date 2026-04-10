# Code Organization and Structure

> Master the art of organizing JavaScript code for maintainability, scalability, and developer experience

## Table of Contents

1. [Introduction](#introduction)
2. [Module Patterns](#module-patterns)
3. [Project Structure](#project-structure)
4. [ES Modules](#es-modules)
5. [CommonJS](#commonjs)
6. [Best Practices](#best-practices)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## 1. Introduction

[anchor](#1-introduction)

Effective code organization is the foundation of maintainable software. As JavaScript applications grow in complexity, proper structure becomes critical for team collaboration, testing, and long-term maintenance.

### Why Code Organization Matters

- **Readability**: Well-organized code is easier to understand
- **Maintainability**: Clear structure simplifies bug fixes and updates
- **Testability**: Organized code is easier to test in isolation
- **Collaboration**: Teams can work efficiently with consistent structure

### Evolution of JavaScript Modules

| Era | Approach | Key Feature |
|-----|----------|-------------|
| Early | IIFE + Global | Namespacing |
| Common | CommonJS | Synchronous require |
| Modern | ES Modules | Native import/export |
| Hybrid | UMD | Universal compatibility |

---

## 2. Module Patterns

[anchor](#2-module-patterns)

JavaScript offers multiple module patterns, each with specific use cases and trade-offs.

### IIFE Pattern (Immediately Invoked Function Expression)

```javascript
// file: modules/iife-pattern.js
// IIFE pattern for creating private scope

const MathUtils = (function() {
  'use strict';

  function factorial(n) {
    if (n <= 1) return 1;
    return n * factorial(n - 1);
  }

  function fibonacci(n) {
    if (n <= 1) return n;
    let a = 0, b = 1;
    for (let i = 2; i <= n; i++) {
      [a, b] = [b, a + b];
    }
    return b;
  }

  function gcd(a, b) {
    a = Math.abs(a);
    b = Math.abs(b);
    while (b) {
      [a, b] = [b, a % b];
    }
    return a;
  }

  function lcm(a, b) {
    return Math.abs(a * b) / gcd(a, b);
  }

  return {
    factorial,
    fibonacci,
    gcd,
    lcm,
    version: '1.0.0'
  };
})();

console.log(MathUtils.factorial(5));
console.log(MathUtils.fibonacci(10));
console.log(MathUtils.gcd(48, 18));
```

### Revealing Module Pattern

```javascript
// file: modules/revealing-module.js
// Revealing Module pattern with public/private separation

const UserManager = (function() {
  const PRIVATE_STORAGE = new Map();
  let _idCounter = 0;

  function _generateId() {
    return `user_${++_idCounter}_${Date.now()}`;
  }

  function _validateEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }

  function _validateName(name) {
    return typeof name === 'string' && name.trim().length >= 2;
  }

  function _createUserObject(name, email) {
    return {
      id: _generateId(),
      name: name.trim(),
      email: email.toLowerCase().trim(),
      createdAt: new Date().toISOString(),
      metadata: {
        lastLogin: null,
        loginCount: 0
      }
    };
  }

  function _notify(type, user) {
    console.log(`[UserManager] ${type}: ${user.name} (${user.email})`);
  }

  function create(name, email) {
    if (!_validateName(name)) {
      throw new Error('Invalid name: must be at least 2 characters');
    }
    if (!_validateEmail(email)) {
      throw new Error('Invalid email format');
    }

    const user = _createUserObject(name, email);
    PRIVATE_STORAGE.set(user.id, user);
    _notify('created', user);

    return user;
  }

  function getById(id) {
    return PRIVATE_STORAGE.get(id) || null;
  }

  function getAll() {
    return Array.from(PRIVATE_STORAGE.values());
  }

  function remove(id) {
    const user = PRIVATE_STORAGE.get(id);
    if (!user) return false;

    PRIVATE_STORAGE.delete(id);
    _notify('deleted', user);
    return true;
  }

  function recordLogin(id) {
    const user = PRIVATE_STORAGE.get(id);
    if (!user) return null;

    user.metadata.lastLogin = new Date().toISOString();
    user.metadata.loginCount++;

    return user.metadata;
  }

  return {
    create,
    getById,
    getAll,
    remove,
    recordLogin,
    get count() {
      return PRIVATE_STORAGE.size;
    }
  };
})();

const user1 = UserManager.create('Alice Johnson', 'alice@example.com');
const user2 = UserManager.create('Bob Smith', 'bob@example.com');
console.log(UserManager.getAll());
console.log('Total users:', UserManager.count);
```

### Namespace Pattern

```javascript
// file: modules/namespace.js
// Namespace pattern for organizing related functionality

const App = {};

App.utils = (function() {
  const _cache = new Map();

  function debounce(fn, delay) {
    let timeoutId;
    return function(...args) {
      clearTimeout(timeoutId);
      timeoutId = setTimeout(() => fn.apply(this, args), delay);
    };
  }

  function throttle(fn, limit) {
    let inThrottle;
    return function(...args) {
      if (!inThrottle) {
        fn.apply(this, args);
        inThrottle = true;
        setTimeout(() => inThrottle = false, limit);
      }
    };
  }

  function memoize(fn) {
    return function(...args) {
      const key = JSON.stringify(args);
      if (_cache.has(key)) {
        return _cache.get(key);
      }
      const result = fn.apply(this, args);
      _cache.set(key, result);
      return result;
    };
  }

  return { debounce, throttle, memoize };
})();

App.validators = (function() {
  function isEmail(value) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
  }

  function isURL(value) {
    try {
      new URL(value);
      return true;
    } catch {
      return false;
    }
  }

  function isPhone(value) {
    return /^\+?[\d\s\-()]{10,}$/.test(value);
  }

  return { isEmail, isURL, isPhone };
})();

App.formatters = (function() {
  function currency(amount, currency = 'USD') {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency
    }).format(amount);
  }

  function relativeTime(date) {
    const now = new Date();
    const diff = now - new Date(date);
    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (days > 0) return `${days} day${days > 1 ? 's' : ''} ago`;
    if (hours > 0) return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    if (minutes > 0) return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    return 'just now';
  }

  return { currency, relativeTime };
})();
```

---

## 3. Project Structure

[anchor](#3-project-structure)

A well-organized project structure enables scalable development and easy navigation.

### Monorepo Structure

```
project/
├── package.json
├── .eslintrc.json
├── .prettierrc
├── README.md
├── src/
│   ├── index.js
│   ├── app.js
│   ├── config/
│   │   ├── index.js
│   │   ├── development.js
│   │   └── production.js
│   ├── core/
│   │   ├── errors/
│   │   │   ├── AppError.js
│   │   │   └── ErrorCodes.js
│   │   ├── events/
│   │   │   └── EventEmitter.js
│   │   └── utils/
│   │       ├── logger.js
│   │       └── helpers.js
│   ├── modules/
│   │   ├── auth/
│   │   │   ├── controllers/
│   │   │   ├── services/
│   │   │   ├── models/
│   │   │   ├── routes/
│   │   │   ├── middleware/
│   │   │   └── index.js
│   │   ├── users/
│   │   ├── products/
│   │   └── orders/
│   ├── services/
│   │   ├── payment/
│   │   ├── email/
│   │   └── storage/
│   ├── middleware/
│   │   ├── auth.js
│   │   ├── validation.js
│   │   └── errorHandler.js
│   ├── database/
│   │   ├── migrations/
│   │   ├── seeders/
│   │   └── connection.js
│   └── tests/
│       ├── unit/
│       ├── integration/
│       └── fixtures/
├── public/
│   ├── index.html
│   ├── css/
│   └── js/
└── dist/
```

### Feature-Based Structure

```javascript
// file: structure/feature-based.js
// Feature-based module organization

export const features = {
  auth: {
    controllers: {
      login: async (req, res) => { /* ... */ },
      logout: async (req, res) => { /* ... */ },
      register: async (req, res) => { /* ... */ }
    },
    services: {
      tokenService: () => { /* ... */ },
      oauthService: () => { /* ... */ }
    },
    middleware: {
      authenticate: (req, res, next) => { /* ... */ },
      authorize: (roles) => (req, res, next) => { /* ... */ }
    }
  },
  dashboard: {
    controllers: {},
    services: {
      analytics: () => { /* ... */ },
      notifications: () => { /* ... */ }
    }
  }
};

export function createFeatureRouter(feature) {
  const router = require('express').Router();
  
  Object.entries(feature.controllers).forEach(([name, handler]) => {
    router.post(`/${name}`, handler);
  });
  
  return router;
}
```

### Clean Architecture Structure

```
src/
├── domain/
│   ├── entities/
│   │   ├── User.js
│   │   ├── Order.js
│   │   └── Product.js
│   ├── value-objects/
│   │   ├── Email.js
│   │   └── Address.js
│   └── interfaces/
│       ├── IUserRepository.js
│       └── INotificationService.js
├── application/
│   ├── use-cases/
│   │   ├── CreateUser.js
│   │   ├── PlaceOrder.js
│   │   └── ProcessPayment.js
│   ├── dto/
│   │   ├── CreateUserDTO.js
│   │   └── OrderDTO.js
│   └── ports/
│       ├── UserRepositoryPort.js
│       └── PaymentGatewayPort.js
├── infrastructure/
│   ├── repositories/
│   │   ├── MongoUserRepository.js
│   │   └── SQLOrderRepository.js
│   ├── services/
│   │   ├── SendGridEmailService.js
│   │   └── StripePaymentService.js
│   └── database/
│       └── connection.js
└── presentation/
    ├── controllers/
    ├── routes/
    ├── middleware/
    └── views/
```

---

## 4. ES Modules

[anchor](#4-es-modules)

ES Modules (ESM) are the standard module system for modern JavaScript.

### Basic Exports

```javascript
// file: esm/utils.js
// Named exports

export function add(a, b) {
  return a + b;
}

export function subtract(a, b) {
  return a - b;
}

export function multiply(a, b) {
  return a * b;
}

export function divide(a, b) {
  if (b === 0) throw new Error('Division by zero');
  return a / b;
}

export const PI = 3.14159;

export class Calculator {
  constructor(initial = 0) {
    this.value = initial;
  }

  add(n) {
    this.value += n;
    return this;
  }

  subtract(n) {
    this.value -= n;
    return this;
  }

  getValue() {
    return this.value;
  }
}
```

```javascript
// file: esm/math.js
// Re-exporting and renaming

export { add, subtract, multiply, divide as div, PI, Calculator } from './utils.js';

export function square(n) {
  return n * n;
}

export function cube(n) {
  return n * n * n;
}

export * from './utils.js';
```

### Default Exports

```javascript
// file: esm/user-service.js
// Default export

export default class UserService {
  #repository;
  #notifier;

  constructor(repository, notifier) {
    this.#repository = repository;
    this.#notifier = notifier;
  }

  async createUser(userData) {
    const user = await this.#repository.save(userData);
    await this.#notifier.notify(user, 'welcome');
    return user;
  }

  async getUser(id) {
    return this.#repository.findById(id);
  }

  async getAllUsers() {
    return this.#repository.findAll();
  }

  async deleteUser(id) {
    return this.#repository.delete(id);
  }
}
```

### Import Syntax

```javascript
// file: esm/index.js
// Various import patterns

import { add, subtract, multiply, divide } from './utils.js';
import { add as sum, multiply as times } from './utils.js';
import * as MathUtils from './utils.js';
import Calculator, { PI } from './utils.js';
import('./utils.js').then(module => {
  console.log(module.add(1, 2));
});

console.log(add(2, 3));
console.log(MathUtils.PI);
const calc = new Calculator(10);
```

### Dynamic Imports

```javascript
// file: esm/dynamic-imports.js
// Dynamic imports for code splitting

class ModuleLoader {
  #cache = new Map();

  async loadModule(path) {
    if (this.#cache.has(path)) {
      return this.#cache.get(path);
    }

    const module = await import(path);
    this.#cache.set(path, module);
    return module;
  }

  async loadOnDemand(features) {
    const results = await Promise.all(
      features.map(f => this.loadModule(f))
    );
    return results;
  }
}

const loader = new ModuleLoader();

async function initializeApp(features) {
  if (features.includes('auth')) {
    const auth = await loader.loadModule('./features/auth.js');
    auth.initialize();
  }

  if (features.includes('analytics')) {
    const analytics = await loader.loadModule('./features/analytics.js');
    analytics.trackPageView();
  }
}
```

### Top-Level Await

```javascript
// file: esm/top-level-await.js
// Top-level await in modules

const config = await fetch('/config.json').then(r => r.json());

export const API_URL = config.apiUrl;
export const FEATURES = config.features;

export async function fetchData() {
  const response = await fetch(API_URL);
  return response.json();
}
```

---

## 5. CommonJS

[anchor](#5-commonjs)

CommonJS (CJS) remains important for Node.js and backward compatibility.

### Module Exports

```javascript
// file: cjs/math.js
// CommonJS exports

function add(a, b) {
  return a + b;
}

function subtract(a, b) {
  return a - b;
}

module.exports = {
  add,
  subtract,
  PI: 3.14159,
  Calculator: class {
    constructor(value = 0) {
      this.value = value;
    }
    add(n) {
      this.value += n;
      return this;
    }
    getValue() {
      return this.value;
    }
  }
};
```

### Export Patterns

```javascript
// file: cjs/exports-patterns.js
// Different export patterns in CommonJS

exports.add = function(a, b) { return a + b; };

module.exports.multiply = function(a, b) { return a * b; };

const divide = (a, b) => a / b;
module.exports.divide = divide;

module.exports = {
  add: require('./add'),
  subtract: require('./subtract')
};

module.exports = class MyClass {
  constructor() {
    this.value = 0;
  }
};
```

### Require Patterns

```javascript
// file: cjs/require-patterns.js
// Various require patterns

const fs = require('fs');

const utils = require('./utils');
const { add, subtract } = require('./utils');

const conditional = process.env.NODE_ENV === 'production'
  ? require('./prod-utils')
  : require('./dev-utils');

const asyncModule = require('./async-module');

async function load() {
  const module = await asyncModule;
  module.doSomething();
}
```

### Interop with ES Modules

```javascript
// file: package.json
// package.json for mixed module types

{
  "name": "my-package",
  "type": "module",
  "exports": {
    ".": {
      "import": "./dist/index.mjs",
      "require": "./dist/index.cjs"
    },
    "./utils": {
      "import": "./dist/utils.mjs",
      "require": "./dist/utils.cjs"
    }
  }
}
```

```javascript
// file: cjs/create-package.cjs
// Creating ESM-to-CJS bridge

const esmModule = require('./package.json');

module.exports = esmModule;
module.exports.__esModule = true;
```

---

## 6. Best Practices

[anchor](#6-best-practices)

Follow these practices for maintainable and scalable code.

### Naming Conventions

```javascript
// file: best-practices/naming.js
// Consistent naming patterns

const MAX_RETRIES = 3;
const DEFAULT_TIMEOUT = 5000;

function createUserAccount(userData) {}
function fetchUserProfile(userId) {}
function updateUserSettings(userId, settings) {}

class UserService {}
class PaymentProcessor {}
class EventEmitter {}

const userRepository = new UserRepository();
const notificationService = new NotificationService();

const isValid = true;
const hasPermission = false;
const userExists = true;

const users = [];
const activeUsers = users.filter(u => u.isActive);
```

### Barrel Files (Index Files)

```javascript
// file: best-practices/barrel-files.js
// Barrel file pattern for clean imports

// components/index.js
export { Button } from './Button.js';
export { Card } from './Card.js';
export { Modal } from './Modal.js';
export { Input } from './Input.js';

// services/index.js
export { UserService } from './UserService.js';
export { AuthService } from './AuthService.js';
export { PaymentService } from './PaymentService.js';

// Usage in other files
import { Button, Card, Modal } from './components/index.js';
import { UserService, AuthService } from './services/index.js';
```

### Dependency Injection

```javascript
// file: best-practices/dependency-injection.js
// Constructor injection

class UserService {
  #userRepository;
  #emailService;
  #logger;

  constructor(userRepository, emailService, logger) {
    this.#userRepository = userRepository;
    this.#emailService = emailService;
    this.#logger = logger;
  }

  async createUser(userData) {
    this.#logger.info('Creating user:', userData.email);
    const user = await this.#userRepository.save(userData);
    await this.#emailService.sendWelcome(user);
    return user;
  }
}

class Container {
  #services = new Map();

  register(name, factory) {
    this.#services.set(name, factory);
  }

  resolve(name) {
    const factory = this.#services.get(name);
    return factory(this);
  }
}

const container = new Container();
container.register('logger', () => console);
container.register('userRepository', () => new UserRepository());
container.register('emailService', () => new EmailService());
container.register('userService', (c) => new UserService(
  c.resolve('userRepository'),
  c.resolve('emailService'),
  c.resolve('logger')
));
```

### Error Handling

```javascript
// file: best-practices/error-handling.js
// Structured error handling

class AppError extends Error {
  constructor(message, code, statusCode = 500) {
    super(message);
    this.name = this.constructor.name;
    this.code = code;
    this.statusCode = statusCode;
    Error.captureStackTrace(this, this.constructor);
  }
}

class ValidationError extends AppError {
  constructor(message, errors = []) {
    super(message, 'VALIDATION_ERROR', 400);
    this.errors = errors;
  }
}

class NotFoundError extends AppError {
  constructor(resource) {
    super(`${resource} not found`, 'NOT_FOUND', 404);
  }
}

class UnauthorizedError extends AppError {
  constructor(message = 'Unauthorized') {
    super(message, 'UNAUTHORIZED', 401);
  }
}

function handleError(error) {
  if (error instanceof AppError) {
    return {
      error: {
        code: error.code,
        message: error.message,
        ...(error.errors && { errors: error.errors })
      }
    };
  }

  return {
    error: {
      code: 'INTERNAL_ERROR',
      message: 'An unexpected error occurred'
    }
  };
}
```

### Configuration Management

```javascript
// file: best-practices/configuration.js
// Environment-based configuration

const config = {
  development: {
    database: {
      host: 'localhost',
      port: 5432,
      name: 'dev_db'
    },
    api: {
      port: 3000,
      baseUrl: 'http://localhost:3000'
    },
    logging: {
      level: 'debug'
    }
  },
  production: {
    database: {
      host: process.env.DB_HOST,
      port: parseInt(process.env.DB_PORT),
      name: process.env.DB_NAME
    },
    api: {
      port: parseInt(process.env.PORT),
      baseUrl: process.env.BASE_URL
    },
    logging: {
      level: 'error'
    }
  }
};

function getConfig(env = process.env.NODE_ENV || 'development') {
  const envConfig = config[env] || config.development;
  
  return {
    ...envConfig,
    env,
    isDevelopment: env === 'development',
    isProduction: env === 'production'
  };
}
```

### Circular Dependency Handling

```javascript
// file: best-practices/circular-deps.js
// Avoiding and handling circular dependencies

// file: services/auth.js
// Forward declaration
let UserService;

export function setUserService(service) {
  UserService = service;
}

export function authenticate(credentials) {
  return UserService.validateCredentials(credentials);
}

// file: services/user.js
import { setUserService } from './auth.js';
import { AuthService } from './auth.js';

const authService = new AuthService();
setUserService(authService);

// OR use dependency injection
export class UserService {
  #authService;

  constructor(authService) {
    this.#authService = authService;
  }

  async login(credentials) {
    return this.#authService.authenticate(credentials);
  }
}
```

---

## Key Takeaways

- **Module Patterns**: Choose IIFE for legacy code, ES modules for new projects
- **Project Structure**: Feature-based or layered architecture scales well
- **ES Modules**: Use named exports for better tree-shaking
- **CommonJS**: Still needed for Node.js compatibility; use conditional requires
- **Barrel Files**: Simplify imports but be mindful of circular dependencies
- **Dependency Injection**: Makes testing and refactoring easier

### Performance Considerations

- **ES Modules**: Native support enables better optimization
- **Tree Shaking**: Named exports allow unused code elimination
- **Code Splitting**: Dynamic imports reduce initial bundle size
- **Lazy Loading**: Load features on-demand for large applications

### Security Considerations

- **Environment Variables**: Never commit secrets to version control
- **Module Isolation**: Each module should validate its inputs
- **Dependency Security**: Regularly audit dependencies for vulnerabilities

---

## Common Pitfalls

1. **Circular Dependencies**: Use forward declarations or restructure
2. **God Files**: Don't put everything in one file
3. **Inconsistent Structure**: Follow project conventions
4. **Deep Nesting**: Keep directory depth reasonable (max 4-5 levels)
5. **Mixing Patterns**: Don't combine CommonJS and ES modules inconsistently

---

## Related Files

- [Design Patterns in JavaScript](./01_DESIGN_PATTERNS_JAVASCRIPT.md) - Module patterns
- [SOLID Principles in JavaScript](./02_SOLID_PRINCIPLES_JAVASCRIPT.md) - Code organization principles
- [JavaScript Performance Engineering](./05_JAVASCRIPT_PERFORMANCE_ENGINEERING.md) - Module performance

---

## Practice Exercises

1. **Beginner**: Create a module system for a TODO application
2. **Intermediate**: Refactor a flat directory to feature-based structure
3. **Advanced**: Implement dependency injection container with circular resolution