# useReducer for Complex State

## Overview

useReducer is a powerful React hook for managing complex state logic in your components. While useState is great for simple state, useReducer shines when you have complex state transitions, multiple sub-values, or when the next state depends on the previous one. It's particularly useful for forms, wizards, games, or any state machine-like behavior. This guide covers everything you need to master useReducer.

## Prerequisites

- Understanding of useState hook
- Knowledge of React state management concepts
- Familiarity with JavaScript reducers

## Core Concepts

### What is useReducer?

useReducer is similar to Redux but built into React. It takes a reducer function and an initial state, returning the current state and a dispatch function.

```jsx
// File: src/usereducer-basics.jsx

import React, { useReducer } from 'react';

// Define initial state
const initialState = { count: 0 };

// Define reducer function
function counterReducer(state, action) {
  switch (action.type) {
    case 'increment':
      return { count: state.count + 1 };
    case 'decrement':
      return { count: state.count - 1 };
    case 'reset':
      return { count: 0 };
    default:
      return state;
  }
}

function Counter() {
  const [state, dispatch] = useReducer(counterReducer, initialState);
  
  return (
    <div>
      <p>Count: {state.count}</p>
      <button onClick={() => dispatch({ type: 'increment' })}>+</button>
      <button onClick={() => dispatch({ type: 'decrement' })}>-</button>
      <button onClick={() => dispatch({ type: 'reset' })}>Reset</button>
    </div>
  );
}
```

### Reducer Pattern

```jsx
// File: src/usereducer-patterns.jsx

import React, { useReducer } from 'react';

// Initial state for a todo list
const initialState = {
  todos: [],
  filter: 'all',
  loading: false,
  error: null
};

// Reducer with multiple sub-values
function todoReducer(state, action) {
  switch (action.type) {
    case 'SET_TODOS':
      return { ...state, todos: action.payload, loading: false };
      
    case 'ADD_TODO':
      return {
        ...state,
        todos: [...state.todos, action.payload]
      };
      
    case 'TOGGLE_TODO':
      return {
        ...state,
        todos: state.todos.map(todo =>
          todo.id === action.payload
            ? { ...todo, completed: !todo.completed }
            : todo
        )
      };
      
    case 'DELETE_TODO':
      return {
        ...state,
        todos: state.todos.filter(todo => todo.id !== action.payload)
      };
      
    case 'SET_FILTER':
      return { ...state, filter: action.payload };
      
    case 'SET_LOADING':
      return { ...state, loading: action.payload };
      
    case 'SET_ERROR':
      return { ...state, error: action.payload, loading: false };
      
    default:
      return state;
  }
}

function TodoApp() {
  const [state, dispatch] = useReducer(todoReducer, initialState);
  
  const addTodo = (text) => {
    dispatch({
      type: 'ADD_TODO',
      payload: { id: Date.now(), text, completed: false }
    });
  };
  
  const toggleTodo = (id) => {
    dispatch({ type: 'TOGGLE_TODO', payload: id });
  };
  
  const deleteTodo = (id) => {
    dispatch({ type: 'DELETE_TODO', payload: id });
  };
  
  const setFilter = (filter) => {
    dispatch({ type: 'SET_FILTER', payload: filter });
  };
  
  // Derived state
  const filteredTodos = state.todos.filter(todo => {
    if (state.filter === 'active') return !todo.completed;
    if (state.filter === 'completed') return todo.completed;
    return true;
  });
  
  return (
    <div>
      <h1>Todos</h1>
      {/* ... render todos */}
    </div>
  );
}
```

### Complex State with useReducer

```jsx
// File: src/usereducer-form.jsx

import React, { useReducer } from 'react';

// Initial state
const initialState = {
  values: {
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  },
  errors: {},
  touched: {},
  isSubmitting: false
};

// Action types as constants (prevents typos)
const ActionTypes = {
  SET_VALUE: 'SET_VALUE',
  SET_ERROR: 'SET_ERROR',
  SET_TOUCHED: 'SET_TOUCHED',
  RESET: 'RESET',
  SET_SUBMITTING: 'SET_SUBMITTING'
};

// Reducer
function formReducer(state, action) {
  switch (action.type) {
    case ActionTypes.SET_VALUE:
      return {
        ...state,
        values: {
          ...state.values,
          [action.field]: action.value
        }
      };
      
    case ActionTypes.SET_ERROR:
      return {
        ...state,
        errors: {
          ...state.errors,
          [action.field]: action.error
        }
      };
      
    case ActionTypes.SET_TOUCHED:
      return {
        ...state,
        touched: {
          ...state.touched,
          [action.field]: true
        }
      };
      
    case ActionTypes.SET_SUBMITTING:
      return {
        ...state,
        isSubmitting: action.value
      };
      
    case ActionTypes.RESET:
      return initialState;
      
    default:
      return state;
  }
}

function Form() {
  const [state, dispatch] = useReducer(formReducer, initialState);
  
  const handleChange = (field) => (e) => {
    dispatch({
      type: ActionTypes.SET_VALUE,
      field,
      value: e.target.value
    });
  };
  
  const handleBlur = (field) => () => {
    dispatch({ type: ActionTypes.SET_TOUCHED, field });
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Validate
    const errors = {};
    if (!state.values.name) errors.name = 'Name is required';
    if (!state.values.email) errors.email = 'Email is required';
    if (!state.values.password) errors.password = 'Password is required';
    
    if (Object.keys(errors).length > 0) {
      Object.keys(errors).forEach(field => {
        dispatch({ type: ActionTypes.SET_ERROR, field, error: errors[field] });
      });
      return;
    }
    
    // Submit
    dispatch({ type: ActionTypes.SET_SUBMITTING, value: true });
    
    // Simulate API call
    setTimeout(() => {
      console.log('Form submitted:', state.values);
      dispatch({ type: ActionTypes.SET_SUBMITTING, value: false });
    }, 1000);
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <div>
        <input
          value={state.values.name}
          onChange={handleChange('name')}
          onBlur={handleBlur('name')}
          placeholder="Name"
        />
        {state.errors.name && state.touched.name && (
          <span>{state.errors.name}</span>
        )}
      </div>
      {/* More fields... */}
    </form>
  );
}
```

## Common Mistakes

### Mistake 1: Not Using Immutability

```jsx
// ❌ WRONG - Mutating state directly
function badReducer(state, action) {
  if (action.type === 'increment') {
    state.count++; // Mutation!
    return state;
  }
  return state;
}

// ✅ CORRECT - Always return new state
function goodReducer(state, action) {
  if (action.type === 'increment') {
    return { ...state, count: state.count + 1 };
  }
  return state;
}
```

### Mistake 2: Missing Default Case

```jsx
// ❌ WRONG - No default case can cause bugs
function badReducer(state, action) {
  switch (action.type) {
    case 'increment':
      return { count: state.count + 1 };
  }
  // Forgot to return default!
}

// ✅ CORRECT - Always have default
function goodReducer(state, action) {
  switch (action.type) {
    case 'increment':
      return { count: state.count + 1 };
    default:
      return state;
  }
}
```

### Mistake 3: Using useState for Complex State

```jsx
// ❌ WRONG - useState gets messy with complex logic
function BadComponent() {
  const [todos, setTodos] = useState([]);
  const [filter, setFilter] = useState('all');
  const [loading, setLoading] = useState(false);
  // Multiple state updates that depend on each other
  
  const toggleTodo = (id) => {
    // This gets messy
    setTodos(prev => prev.map(...));
  };
}

// ✅ CORRECT - useReducer organizes complex state
function GoodComponent() {
  const [state, dispatch] = useReducer(reducer, initialState);
  // All related state in one place
}
```

## Real-World Example

```jsx
// File: src/components/ShoppingCart.jsx

import React, { useReducer } from 'react';

// Action types
const ActionTypes = {
  ADD_ITEM: 'ADD_ITEM',
  REMOVE_ITEM: 'REMOVE_ITEM',
  UPDATE_QUANTITY: 'UPDATE_QUANTITY',
  CLEAR_CART: 'CLEAR_CART',
  APPLY_COUPON: 'APPLY_COUPON',
  SET_SHIPPING: 'SET_SHIPPING'
};

// Initial state
const initialState = {
  items: [],
  coupon: null,
  shipping: 0,
  discount: 0
};

// Reducer
function cartReducer(state, action) {
  switch (action.type) {
    case ActionTypes.ADD_ITEM: {
      const existingIndex = state.items.findIndex(
        item => item.id === action.payload.id
      );
      
      if (existingIndex >= 0) {
        // Item exists, update quantity
        const newItems = [...state.items];
        newItems[existingIndex] = {
          ...newItems[existingIndex],
          quantity: newItems[existingIndex].quantity + 1
        };
        return { ...state, items: newItems };
      }
      
      // New item
      return {
        ...state,
        items: [...state.items, { ...action.payload, quantity: 1 }]
      };
    }
    
    case ActionTypes.REMOVE_ITEM:
      return {
        ...state,
        items: state.items.filter(item => item.id !== action.payload)
      };
    
    case ActionTypes.UPDATE_QUANTITY: {
      const { id, quantity } = action.payload;
      if (quantity <= 0) {
        return {
          ...state,
          items: state.items.filter(item => item.id !== id)
        };
      }
      return {
        ...state,
        items: state.items.map(item =>
          item.id === id ? { ...item, quantity } : item
        )
      };
    }
    
    case ActionTypes.CLEAR_CART:
      return initialState;
    
    case ActionTypes.APPLY_COUPON:
      return {
        ...state,
        coupon: action.payload,
        discount: action.payload.discount || 0
      };
    
    case ActionTypes.SET_SHIPPING:
      return {
        ...state,
        shipping: action.payload
      };
    
    default:
      return state;
  }
}

// Calculate totals
function calculateTotals(state) {
  const subtotal = state.items.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );
  const discountAmount = subtotal * (state.discount / 100);
  const total = subtotal - discountAmount + state.shipping;
  
  return { subtotal, discountAmount, total };
}

// Cart Component
function ShoppingCart() {
  const [state, dispatch] = useReducer(cartReducer, initialState);
  const { subtotal, discountAmount, total } = calculateTotals(state);
  
  const addItem = (item) => {
    dispatch({ type: ActionTypes.ADD_ITEM, payload: item });
  };
  
  const removeItem = (id) => {
    dispatch({ type: ActionTypes.REMOVE_ITEM, payload: id });
  };
  
  const updateQuantity = (id, quantity) => {
    dispatch({ type: ActionTypes.UPDATE_QUANTITY, payload: { id, quantity } });
  };
  
  const clearCart = () => {
    dispatch({ type: ActionTypes.CLEAR_CART });
  };
  
  const applyCoupon = (code) => {
    const coupons = {
      'SAVE10': { code: 'SAVE10', discount: 10 },
      'SAVE20': { code: 'SAVE20', discount: 20 }
    };
    
    if (coupons[code]) {
      dispatch({ type: ActionTypes.APPLY_COUPON, payload: coupons[code] });
    }
  };
  
  return (
    <div style={{ maxWidth: '800px', margin: '0 auto', padding: '20px' }}>
      <h1>Shopping Cart</h1>
      
      {/* Cart Items */}
      {state.items.length === 0 ? (
        <p>Your cart is empty</p>
      ) : (
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {state.items.map(item => (
            <li key={item.id} style={{ 
              display: 'flex', 
              alignItems: 'center',
              padding: '15px',
              borderBottom: '1px solid #eee'
            }}>
              <div style={{ flex: 1 }}>
                <h3>{item.name}</h3>
                <p>${item.price.toFixed(2)}</p>
              </div>
              
              <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                <button onClick={() => updateQuantity(item.id, item.quantity - 1)}>-</button>
                <span>{item.quantity}</span>
                <button onClick={() => updateQuantity(item.id, item.quantity + 1)}>+</button>
                <button onClick={() => removeItem(item.id)}>Remove</button>
              </div>
              
              <div style={{ marginLeft: '20px', fontWeight: 'bold' }}>
                ${(item.price * item.quantity).toFixed(2)}
              </div>
            </li>
          ))}
        </ul>
      )}
      
      {/* Totals */}
      {state.items.length > 0 && (
        <div style={{ marginTop: '20px', textAlign: 'right' }}>
          <p>Subtotal: ${subtotal.toFixed(2)}</p>
          {state.discount > 0 && (
            <p>Discount ({state.discount}%): -${discountAmount.toFixed(2)}</p>
          )}
          <p>Shipping: ${state.shipping.toFixed(2)}</p>
          <h2>Total: ${total.toFixed(2)}</h2>
          
          <button onClick={clearCart} style={{ marginTop: '10px' }}>
            Clear Cart
          </button>
        </div>
      )}
    </div>
  );
}

export default ShoppingCart;
```

## Key Takeaways

- useReducer is ideal for complex state with multiple sub-values
- Always use immutability - never mutate state directly
- Use action type constants to prevent typos
- Reducers should be pure functions with no side effects
- useReducer provides more predictable state management than useState
- Use useState for simple state, useReducer for complex transitions

## What's Next

Now let's explore useContext - the hook for consuming context in functional components.
