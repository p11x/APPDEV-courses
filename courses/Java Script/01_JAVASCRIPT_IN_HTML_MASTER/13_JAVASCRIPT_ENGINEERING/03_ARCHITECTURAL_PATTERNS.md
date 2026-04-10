# Architectural Patterns in JavaScript

> Comprehensive guide to architectural patterns for building scalable JavaScript applications

## Table of Contents

1. [Introduction to Architectural Patterns](#introduction)
2. [MVC Pattern](#mvc)
3. [MVVM Pattern](#mvvm)
4. [Flux Pattern](#flux)
5. [Clean Architecture](#clean-architecture)
6. [Layered Architecture](#layered-architecture)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## 1. Introduction to Architectural Patterns

[anchor](#1-introduction-to-architectural-patterns)

Architectural patterns provide a high-level blueprint for organizing code in complex applications. Unlike design patterns which address specific implementation details, architectural patterns define the overall structure and organization of an application.

### Why Architecture Matters

- **Scalability**: Well-architected code scales with team size and codebase growth
- **Maintainability**: Clear separation of concerns makes code easier to maintain
- **Testability**: Isolated components can be tested independently
- **Flexibility**: Architecture enables technology substitution

### Key Principles

| Principle | Description |
|-----------|-------------|
| Separation of Concerns | Different concerns in different modules |
| Single Responsibility | Each component does one thing well |
| Dependency Direction | High-level policy doesn’t depend on low-level details |
| Interface Segregation | Use small, focused interfaces |

---

## 2. MVC Pattern

[anchor](#2-mvc-pattern)

Model-View-Controller (MVC) separates application logic into three interconnected components, allowing for efficient code organization and separation of concerns.

### Traditional MVC

```javascript
// file: mvc/Model.js
// MVC - Model component

class UserModel {
  #users = new Map();
  #listeners = new Set();

  subscribe(listener) {
    this.#listeners.add(listener);
    return () => this.#listeners.delete(listener);
  }

  #notify(event, data) {
    this.#listeners.forEach(listener => listener(event, data));
  }

  create(userData) {
    const user = {
      id: crypto.randomUUID(),
      ...userData,
      createdAt: new Date().toISOString()
    };
    this.#users.set(user.id, user);
    this.#notify('user:created', user);
    return user;
  }

  read(id) {
    return this.#users.get(id);
  }

  update(id, updates) {
    const user = this.#users.get(id);
    if (!user) throw new Error('User not found');
    
    const updatedUser = { ...user, ...updates, updatedAt: new Date().toISOString() };
    this.#users.set(id, updatedUser);
    this.#notify('user:updated', updatedUser);
    return updatedUser;
  }

  delete(id) {
    const deleted = this.#users.delete(id);
    if (deleted) this.#notify('user:deleted', { id });
    return deleted;
  }

  findAll() {
    return Array.from(this.#users.values());
  }

  findByEmail(email) {
    return Array.from(this.#users.values()).find(u => u.email === email);
  }
}

export const userModel = new UserModel();
```

```javascript
// file: mvc/View.js
// MVC - View component

class UserListView {
  #container;
  #onEdit;
  #onDelete;

  constructor(container, handlers) {
    this.#container = container;
    this.#onEdit = handlers.onEdit;
    this.#onDelete = handlers.onDelete;
  }

  render(users) {
    if (users.length === 0) {
      this.#container.innerHTML = '<p>No users found</p>';
      return;
    }

    this.#container.innerHTML = `
      <table class="user-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Created</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          ${users.map(user => `
            <tr data-id="${user.id}">
              <td>${user.name}</td>
              <td>${user.email}</td>
              <td>${new Date(user.createdAt).toLocaleDateString()}</td>
              <td>
                <button class="edit-btn" data-id="${user.id}">Edit</button>
                <button class="delete-btn" data-id="${user.id}">Delete</button>
              </td>
            </tr>
          `).join('')}
        </tbody>
      </table>
    `;

    this.#attachHandlers();
  }

  #attachHandlers() {
    this.#container.querySelectorAll('.edit-btn').forEach(btn => {
      btn.addEventListener('click', () => this.#onEdit(btn.dataset.id));
    });

    this.#container.querySelectorAll('.delete-btn').forEach(btn => {
      btn.addEventListener('click', () => this.#onDelete(btn.dataset.id));
    });
  }

  showError(message) {
    this.#container.innerHTML = `<div class="error">${message}</div>`;
  }

  showSuccess(message) {
    this.#container.innerHTML = `<div class="success">${message}</div>`;
  }
}

export default UserListView;
```

```javascript
// file: mvc/Controller.js
// MVC - Controller component

class UserController {
  #model;
  #view;

  constructor(model, view) {
    this.#model = model;
    this.#view = view;

    this.#model.subscribe((event, data) => {
      this.#handleModelChange(event, data);
    });
  }

  #handleModelChange(event, data) {
    switch (event) {
      case 'user:created':
        this.#view.showSuccess('User created successfully');
        this.listUsers();
        break;
      case 'user:updated':
        this.#view.showSuccess('User updated successfully');
        this.listUsers();
        break;
      case 'user:deleted':
        this.#view.showSuccess('User deleted successfully');
        this.listUsers();
        break;
    }
  }

  listUsers() {
    const users = this.#model.findAll();
    this.#view.render(users);
  }

  createUser(userData) {
    try {
      this.#model.create(userData);
    } catch (error) {
      this.#view.showError(error.message);
    }
  }

  updateUser(id, updates) {
    try {
      this.#model.update(id, updates);
    } catch (error) {
      this.#view.showError(error.message);
    }
  }

  deleteUser(id) {
    try {
      this.#model.delete(id);
    } catch (error) {
      this.#view.showError(error.message);
    }
  }

  getUser(id) {
    return this.#model.read(id);
  }
}

export default UserController;
```

### Professional Use Case: Express MVC

```javascript
// file: mvc/ExpressMVC.js
// MVC in Express - Model

import { pool } from '../database/pool.js';

class UserModel {
  async findAll() {
    const result = await pool.query('SELECT * FROM users ORDER BY created_at DESC');
    return result.rows;
  }

  async findById(id) {
    const result = await pool.query('SELECT * FROM users WHERE id = $1', [id]);
    return result.rows[0];
  }

  async create(userData) {
    const { name, email, password } = userData;
    const result = await pool.query(
      'INSERT INTO users (name, email, password) VALUES ($1, $2, $3) RETURNING *',
      [name, email, password]
    );
    return result.rows[0];
  }

  async update(id, userData) {
    const { name, email } = userData;
    const result = await pool.query(
      'UPDATE users SET name = $1, email = $2, updated_at = NOW() WHERE id = $3 RETURNING *',
      [name, email, id]
    );
    return result.rows[0];
  }

  async delete(id) {
    await pool.query('DELETE FROM users WHERE id = $1', [id]);
  }
}

export const userModel = new UserModel();
```

```javascript
// file: mvc/UserView.js
// MVC in Express - View (Template)

export function renderUserList(users, meta = {}) {
  return {
    data: {
      users: users.map(user => ({
        id: user.id,
        name: user.name,
        email: user.email,
        createdAt: user.created_at
      })),
      meta
    }
  };
}

export function renderUser(user) {
  return {
    data: {
      user: {
        id: user.id,
        name: user.name,
        email: user.email,
        createdAt: user.created_at
      }
    }
  };
}

export function renderError(error) {
  return {
    error: {
      message: error.message,
      code: error.code || 'INTERNAL_ERROR'
    }
  };
}

export function renderCreated(user) {
  return {
    data: { user },
    status: 201
  };
}

export function renderNoContent() {
  return { status: 204 };
}
```

```javascript
// file: mvc/UserController.js
// MVC in Express - Controller

import { userModel } from '../models/UserModel.js';
import * as UserView from '../views/UserView.js';

class UserController {
  async list(req, res) {
    try {
      const users = await userModel.findAll();
      res.json(renderUserList(users));
    } catch (error) {
      res.status(500).json(renderError(error));
    }
  }

  async get(req, res) {
    try {
      const user = await userModel.findById(req.params.id);
      if (!user) {
        return res.status(404).json(renderError(new Error('User not found')));
      }
      res.json(renderUser(user));
    } catch (error) {
      res.status(500).json(renderError(error));
    }
  }

  async create(req, res) {
    try {
      const user = await userModel.create(req.body);
      res.status(201).json(renderCreated(user));
    } catch (error) {
      res.status(400).json(renderError(error));
    }
  }

  async update(req, res) {
    try {
      const user = await userModel.update(req.params.id, req.body);
      res.json(renderUser(user));
    } catch (error) {
      if (error.message === 'User not found') {
        return res.status(404).json(renderError(error));
      }
      res.status(400).json(renderError(error));
    }
  }

  async delete(req, res) {
    try {
      await userModel.delete(req.params.id);
      res.status(204).send();
    } catch (error) {
      res.status(500).json(renderError(error));
    }
  }
}

export const userController = new UserController();
```

---

## 3. MVVM Pattern

[anchor](#3-mvvm-pattern)

Model-View-ViewModel (MVVM) is a variation of MVC that includes a ViewModel layer, providing two-way data binding between View and ViewModel.

### Basic MVVM Implementation

```javascript
// file: mvvm/ViewModel.js
// MVVM - ViewModel component

class Observable {
  #value;
  #listeners = [];

  constructor(value) {
    this.#value = value;
  }

  get value() {
    return this.#value;
  }

  set value(newValue) {
    if (this.#value === newValue) return;
    this.#value = newValue;
    this.#notify();
  }

  subscribe(listener) {
    this.#listeners.push(listener);
    return () => {
      this.#listeners = this.#listeners.filter(l => l !== listener);
    };
  }

  #notify() {
    this.#listeners.forEach(listener => listener(this.#value));
  }
}

class UserFormViewModel {
  #name = new Observable('');
  #email = new Observable('');
  #errors = new Observable({});
  #isValid = new Observable(false);
  #isSubmitting = new Observable(false);

  constructor() {
    this.#name.subscribe(() => this.#validate());
    this.#email.subscribe(() => this.#validate());
  }

  get name() {
    return this.#name;
  }

  get email() {
    return this.#email;
  }

  get errors() {
    return this.#errors;
  }

  get isValid() {
    return this.#isValid;
  }

  get isSubmitting() {
    return this.#isSubmitting;
  }

  setName(value) {
    this.#name.value = value;
  }

  setEmail(value) {
    this.#email.value = value;
  }

  #validate() {
    const errors = {};

    if (this.#name.value.length < 2) {
      errors.name = 'Name must be at least 2 characters';
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(this.#email.value)) {
      errors.email = 'Invalid email format';
    }

    this.#errors.value = errors;
    this.#isValid.value = Object.keys(errors).length === 0;
  }

  async submit() {
    if (!this.#isValid.value) return;

    this.#isSubmitting.value = true;
    
    try {
      await this.#saveUser();
      this.reset();
    } finally {
      this.#isSubmitting.value = false;
    }
  }

  async #saveUser() {
    console.log('Saving user:', {
      name: this.#name.value,
      email: this.#email.value
    });
  }

  reset() {
    this.#name.value = '';
    this.#email.value = '';
  }
}

export default UserFormViewModel;
```

```javascript
// file: mvvm/UserFormView.js
// MVVM - View component with two-way binding

class UserFormView {
  #viewModel;
  #formElement;
  #submitCallback;

  constructor(viewModel, container) {
    this.#viewModel = viewModel;
    this.#formElement = document.createElement('form');
    this.#formElement.className = 'user-form';
    container.appendChild(this.#formElement);

    this.#render();
    this.#bindEvents();
  }

  #render() {
    this.#formElement.innerHTML = `
      <div class="form-group">
        <label for="name">Name</label>
        <input type="text" id="name" name="name" />
        <span class="error" id="name-error"></span>
      </div>
      <div class="form-group">
        <label for="email">Email</label>
        <input type="email" id="email" name="email" />
        <span class="error" id="email-error"></span>
      </div>
      <button type="submit" id="submit-btn">Submit</button>
    `;
  }

  #bindEvents() {
    const nameInput = this.#formElement.querySelector('#name');
    const emailInput = this.#formElement.querySelector('#email');
    const submitBtn = this.#formElement.querySelector('#submit-btn');

    nameInput.addEventListener('input', (e) => {
      this.#viewModel.setName(e.target.value);
    });

    emailInput.addEventListener('input', (e) => {
      this.#viewModel.setEmail(e.target.value);
    });

    this.#viewModel.name.subscribe(value => {
      nameInput.value = value;
    });

    this.#viewModel.email.subscribe(value => {
      emailInput.value = value;
    });

    this.#viewModel.errors.subscribe(errors => {
      document.getElementById('name-error').textContent = errors.name || '';
      document.getElementById('email-error').textContent = errors.email || '';
    });

    this.#viewModel.isSubmitting.subscribe(isSubmitting => {
      submitBtn.disabled = isSubmitting;
      submitBtn.textContent = isSubmitting ? 'Submitting...' : 'Submit';
    });

    this.#formElement.addEventListener('submit', async (e) => {
      e.preventDefault();
      await this.#viewModel.submit();
    });
  }
}

export default UserFormView;
```

### Professional Use Case: Vue.js MVVM

```javascript
// file: mvvm/vue/UserProfile.js
// Vue.js component following MVVM

export default {
  name: 'UserProfile',
  
  data() {
    return {
      user: {
        name: '',
        email: '',
        bio: ''
      },
      isEditing: false,
      isSaving: false,
      errors: {}
    };
  },

  computed: {
    canSave() {
      return this.isEditing && !this.isSaving;
    }
  },

  methods: {
    async saveProfile() {
      this.isSaving = true;
      this.errors = {};

      try {
        await this.$api.users.update(this.user.id, this.user);
        this.isEditing = false;
      } catch (error) {
        this.errors = error.response?.data?.errors || {};
      } finally {
        this.isSaving = false;
      }
    },

    cancelEdit() {
      this.loadUser();
      this.isEditing = false;
    },

    async loadUser() {
      const user = await this.$api.users.get(this.$route.params.id);
      this.user = { ...user };
    }
  },

  mounted() {
    this.loadUser();
  }
};
```

---

## 4. Flux Pattern

[anchor](#4-flux-pattern)

Flux is an application architecture that enforces unidirectional data flow, making state changes predictable and easier to debug.

### Flux Implementation

```javascript
// file: flux/Dispatcher.js
// Flux - Dispatcher

class Dispatcher {
  #handlers = new Map();
  #pending = [];
  #isDispatching = false;

  register(actionType, handler) {
    if (!this.#handlers.has(actionType)) {
      this.#handlers.set(actionType, []);
    }
    this.#handlers.get(actionType).push(handler);

    return () => {
      const handlers = this.#handlers.get(actionType);
      const index = handlers.indexOf(handler);
      if (index > -1) handlers.splice(index, 1);
    };
  }

  dispatch(action) {
    if (this.#isDispatching) {
      throw new Error('Cannot dispatch while dispatching');
    }

    this.#isDispatching = true;
    this.#pending = [action];

    try {
      while (this.#pending.length) {
        const currentAction = this.#pending.shift();
        const handlers = this.#handlers.get(currentAction.type);

        if (handlers) {
          handlers.forEach(handler => {
            const newActions = handler(currentAction);
            if (newActions) {
              this.#pending.push(...newActions);
            }
          });
        }
      }
    } finally {
      this.#isDispatching = false;
    }
  }
}

export const dispatcher = new Dispatcher();
```

```javascript
// file: flux/ActionTypes.js
// Flux - Action Types

export const ActionTypes = {
  LOAD_USERS: 'LOAD_USERS',
  LOAD_USERS_SUCCESS: 'LOAD_USERS_SUCCESS',
  LOAD_USERS_ERROR: 'LOAD_USERS_ERROR',
  CREATE_USER: 'CREATE_USER',
  CREATE_USER_SUCCESS: 'CREATE_USER_SUCCESS',
  CREATE_USER_ERROR: 'CREATE_USER_ERROR',
  UPDATE_USER: 'UPDATE_USER',
  UPDATE_USER_SUCCESS: 'UPDATE_USER_SUCCESS',
  UPDATE_USER_ERROR: 'UPDATE_USER_ERROR',
  DELETE_USER: 'DELETE_USER',
  DELETE_USER_SUCCESS: 'DELETE_USER_SUCCESS',
  DELETE_USER_ERROR: 'DELETE_USER_ERROR'
};
```

```javascript
// file: flux/Actions.js
// Flux - Actions

import { dispatcher } from './Dispatcher.js';
import { ActionTypes } from './ActionTypes.js';

export const UserActions = {
  loadUsers() {
    dispatcher.dispatch({ type: ActionTypes.LOAD_USERS });
  },

  createUser(userData) {
    dispatcher.dispatch({ 
      type: ActionTypes.CREATE_USER, 
      payload: userData 
    });
  },

  updateUser(id, userData) {
    dispatcher.dispatch({ 
      type: ActionTypes.UPDATE_USER, 
      payload: { id, ...userData } 
    });
  },

  deleteUser(id) {
    dispatcher.dispatch({ 
      type: ActionTypes.DELETE_USER, 
      payload: { id } 
    });
  }
};
```

```javascript
// file: flux/Store.js
// Flux - Store

class Store {
  #state;
  #listeners = new Set();
  #reducer;

  constructor(initialState, reducer) {
    this.#state = initialState;
    this.#reducer = reducer;

    this.#registerDefaultHandlers();
  }

  getState() {
    return this.#state;
  }

  subscribe(listener) {
    this.#listeners.add(listener);
    return () => this.#listeners.delete(listener);
  }

  #notify() {
    this.#listeners.forEach(listener => listener(this.#state));
  }

  #registerDefaultHandlers() {}

  dispatch(action) {
    this.#state = this.#reducer(this.#state, action);
    this.#notify();
  }
}

class UserStore extends Store {
  constructor() {
    super(
      {
        users: [],
        loading: false,
        error: null
      },
      (state, action) => {
        switch (action.type) {
          case ActionTypes.LOAD_USERS:
            return { ...state, loading: true, error: null };

          case ActionTypes.LOAD_USERS_SUCCESS:
            return { 
              ...state, 
              users: action.payload, 
              loading: false 
            };

          case ActionTypes.LOAD_USERS_ERROR:
            return { 
              ...state, 
              loading: false, 
              error: action.payload 
            };

          case ActionTypes.CREATE_USER_SUCCESS:
            return {
              ...state,
              users: [...state.users, action.payload]
            };

          case ActionTypes.UPDATE_USER_SUCCESS:
            return {
              ...state,
              users: state.users.map(user =>
                user.id === action.payload.id ? action.payload : user
              )
            };

          case ActionTypes.DELETE_USER_SUCCESS:
            return {
              ...state,
              users: state.users.filter(user => user.id !== action.payload)
            };

          default:
            return state;
        }
      }
    );
  }
}

export const userStore = new UserStore();
```

### Professional Use Case: Redux-like Pattern

```javascript
// file: flux/createStore.js
// Flux - createStore utility

export function createStore(reducer, initialState) {
  let state = initialState;
  const listeners = new Set();

  return {
    getState() {
      return state;
    },

    dispatch(action) {
      state = reducer(state, action);
      listeners.forEach(listener => listener(state));
    },

    subscribe(listener) {
      listeners.add(listener);
      return () => listeners.delete(listener);
    }
  };
}

export function combineReducers(reducers) {
  return (state = {}, action) => {
    const nextState = {};
    
    Object.keys(reducers).forEach(key => {
      const reducer = reducers[key];
      const previousState = state[key];
      nextState[key] = reducer(previousState, action);
    });

    return nextState;
  };
}

export function bindActionCreators(actions, dispatch) {
  const bound = {};
  
  Object.keys(actions).forEach(key => {
    const action = actions[key];
    bound[key] = (...args) => dispatch(action(...args));
  });

  return bound;
}
```

---

## 5. Clean Architecture

[anchor](#5-clean-architecture)

Clean Architecture organizes code into layers with strict dependency rules, keeping business logic independent of frameworks and UI.

### Clean Architecture Implementation

```javascript
// file: clean/entities/User.js
// Clean Architecture - Entities (innermost layer)

export class User {
  #id;
  #name;
  #email;
  #createdAt;

  constructor({ id, name, email, createdAt }) {
    this.#id = id;
    this.#name = name;
    this.#email = email;
    this.#createdAt = createdAt || new Date();
  }

  get id() { return this.#id; }
  get name() { return this.#name; }
  get email() { return this.#email; }
  get createdAt() { return this.#createdAt; }
}
```

```javascript
// file: clean/usecases/CreateUser.js
// Clean Architecture - Use Cases

import { User } from '../entities/User.js';

export class CreateUserUseCase {
  #userRepository;
  #emailService;

  constructor(userRepository, emailService) {
    this.#userRepository = userRepository;
    this.#emailService = emailService;
  }

  async execute(userData) {
    const existingUser = await this.#userRepository.findByEmail(userData.email);
    if (existingUser) {
      throw new Error('User already exists');
    }

    if (userData.password.length < 8) {
      throw new Error('Password must be at least 8 characters');
    }

    const user = new User({
      id: crypto.randomUUID(),
      name: userData.name,
      email: userData.email,
      createdAt: new Date()
    });

    const createdUser = await this.#userRepository.save(user);

    await this.#emailService.send({
      to: user.email,
      subject: 'Welcome!',
      body: 'Welcome to our platform!'
    });

    return createdUser;
  }
}
```

```javascript
// file: clean/repositories/UserRepository.js
// Clean Architecture - Repository Interface

export class UserRepository {
  async save(user) {
    throw new Error('save() must be implemented');
  }

  async findById(id) {
    throw new Error('findById() must be implemented');
  }

  async findByEmail(email) {
    throw new Error('findByEmail() must be implemented');
  }

  async findAll() {
    throw new Error('findAll() must be implemented');
  }

  async delete(id) {
    throw new Error('delete() must be implemented');
  }
}
```

```javascript
// file: clean/repositories/MongoDBUserRepository.js
// Clean Architecture - Infrastructure

import { User } from '../entities/User.js';
import { UserRepository } from './UserRepository.js';

export class MongoDBUserRepository extends UserRepository {
  #collection;

  constructor(collection) {
    super();
    this.#collection = collection;
  }

  async save(user) {
    const result = await this.#collection.insertOne(user);
    return { ...user, id: result.insertedId };
  }

  async findById(id) {
    const user = await this.#collection.findOne({ id });
    return user ? new User(user) : null;
  }

  async findByEmail(email) {
    const user = await this.#collection.findOne({ email });
    return user ? new User(user) : null;
  }

  async findAll() {
    const users = await this.#collection.find().toArray();
    return users.map(u => new User(u));
  }

  async delete(id) {
    await this.#collection.deleteOne({ id });
  }
}
```

```javascript
// file: clean/dependencies.js
// Clean Architecture - Dependency Injection Container

class Container {
  #services = new Map();

  register(name, service) {
    this.#services.set(name, service);
  }

  resolve(name) {
    const service = this.#services.get(name);
    if (!service) {
      throw new Error(`Service not registered: ${name}`);
    }
    return service;
  }
}

const container = new Container();

export { container };
```

---

## 6. Layered Architecture

[anchor](#6-layered-architecture)

Layered Architecture organizes code into horizontal layers, with each layer depending only on the layer directly beneath it.

### Layered Architecture Implementation

```javascript
// file: layered/presentation/controllers/UserController.js
// Layered - Presentation Layer

export class UserPresentationController {
  #userService;

  constructor(userService) {
    this.#userService = userService;
  }

  async getUsers(req, res) {
    try {
      const users = await this.#userService.getAllUsers();
      res.json({ success: true, data: users });
    } catch (error) {
      res.status(500).json({ success: false, error: error.message });
    }
  }

  async createUser(req, res) {
    try {
      const user = await this.#userService.createUser(req.body);
      res.status(201).json({ success: true, data: user });
    } catch (error) {
      res.status(400).json({ success: false, error: error.message });
    }
  }
}
```

```javascript
// file: layered/application/services/UserService.js
// Layered - Application Layer

export class UserApplicationService {
  #userRepository;
  #userValidator;
  #notificationService;

  constructor(deps) {
    this.#userRepository = deps.userRepository;
    this.#userValidator = deps.userValidator;
    this.#notificationService = deps.notificationService;
  }

  async getAllUsers() {
    return this.#userRepository.findAll();
  }

  async getUserById(id) {
    const user = await this.#userRepository.findById(id);
    if (!user) {
      throw new Error('User not found');
    }
    return user;
  }

  async createUser(userData) {
    const validation = this.#userValidator.validate(userData);
    if (!validation.valid) {
      throw new Error(validation.errors.join(', '));
    }

    const existingUser = await this.#userRepository.findByEmail(userData.email);
    if (existingUser) {
      throw new Error('User with this email already exists');
    }

    const user = await this.#userRepository.create(userData);
    await this.#notificationService.sendWelcome(user);

    return user;
  }

  async updateUser(id, userData) {
    const existingUser = await this.#userRepository.findById(id);
    if (!existingUser) {
      throw new Error('User not found');
    }

    return this.#userRepository.update(id, userData);
  }

  async deleteUser(id) {
    const existingUser = await this.#userRepository.findById(id);
    if (!existingUser) {
      throw new Error('User not found');
    }

    return this.#userRepository.delete(id);
  }
}
```

```javascript
// file: layered/domain/repositories/UserRepository.js
// Layered - Domain Layer

export class UserDomainRepository {
  async create(user) {
    throw new Error('Not implemented');
  }

  async findById(id) {
    throw new Error('Not implemented');
  }

  async findByEmail(email) {
    throw new Error('Not implemented');
  }

  async findAll() {
    throw new Error('Not implemented');
  }

  async update(id, data) {
    throw new Error('Not implemented');
  }

  async delete(id) {
    throw new Error('Not implemented');
  }
}
```

```javascript
// file: layered/infrastructure/database/UserDatabase.js
// Layered - Infrastructure Layer

import { Pool } from 'pg';

export class PostgreSQLUserRepository {
  #pool;

  constructor(pool) {
    this.#pool = pool;
  }

  async create(userData) {
    const result = await this.#pool.query(
      'INSERT INTO users (name, email, password_hash) VALUES ($1, $2, $3) RETURNING *',
      [userData.name, userData.email, userData.passwordHash]
    );
    return this.#mapToUser(result.rows[0]);
  }

  async findById(id) {
    const result = await this.#pool.query(
      'SELECT * FROM users WHERE id = $1',
      [id]
    );
    return result.rows[0] ? this.#mapToUser(result.rows[0]) : null;
  }

  async findByEmail(email) {
    const result = await this.#pool.query(
      'SELECT * FROM users WHERE email = $1',
      [email]
    );
    return result.rows[0] ? this.#mapToUser(result.rows[0]) : null;
  }

  async findAll() {
    const result = await this.#pool.query('SELECT * FROM users ORDER BY created_at DESC');
    return result.rows.map(this.#mapToUser);
  }

  async update(id, data) {
    const result = await this.#pool.query(
      'UPDATE users SET name = $1, email = $2, updated_at = NOW() WHERE id = $3 RETURNING *',
      [data.name, data.email, id]
    );
    return result.rows[0] ? this.#mapToUser(result.rows[0]) : null;
  }

  async delete(id) {
    await this.#pool.query('DELETE FROM users WHERE id = $1', [id]);
  }

  #mapToUser(row) {
    return {
      id: row.id,
      name: row.name,
      email: row.email,
      createdAt: row.created_at,
      updatedAt: row.updated_at
    };
  }
}
```

---

## Key Takeaways

- **MVC**: Classic pattern, good for server-side frameworks like Express
- **MVVM**: Excellent for client-side frameworks with two-way binding (Vue, Angular)
- **Flux**: Unidirectional flow, great for complex React applications
- **Clean Architecture**: Maximum testability and framework independence
- **Layered**: Practical and familiar, works well for most web applications

### Performance Considerations

- **MVC**: Controller overhead can impact latency
- **MVVM**: Computed properties cache; avoid expensive operations
- **Flux**: Single store can become bottleneck; consider splitting
- **Clean Architecture**: Layer indirection adds overhead; profile carefully

### Security Considerations

- **Clean Architecture**: Domain isolation prevents SQL injection naturally
- **Flux**: Immutable state prevents mutations
- **Layered**: Each layer can add security checks

---

## Common Pitfalls

1. **Over-Engineering**: Don't use complex patterns for simple applications
2. **Mixing Patterns**: Don't combine incompatible patterns
3. **Ignoring Data Flow**: Ensure unidirectional flow is maintained
4. **Tight Coupling**: Keep layers independent
5. **Missing Boundaries**: Define clear interfaces between layers

---

## Related Files

- [Design Patterns in JavaScript](./01_DESIGN_PATTERNS_JAVASCRIPT.md) - Design patterns that complement architecture
- [SOLID Principles in JavaScript](./02_SOLID_PRINCIPLES_JAVASCRIPT.md) - Underlying principles
- [Code Organization and Structure](./04_CODE_ORGANIZATION_AND_STRUCTURE.md) - Project organization

---

## Practice Exercises

1. **Beginner**: Implement MVC for a blog comment system
2. **Intermediate**: Build MVVM for a dashboard with multiple components
3. **Advanced**: Create Clean Architecture with multiple use cases