# 🚀 React JavaScript Master Guide

## Comprehensive React Development for JavaScript Professionals

---

## Table of Contents

1. [Introduction to React](#introduction-to-react)
2. [React Fundamentals](#react-fundamentals)
3. [JSX Deep Dive](#jsx-deep-dive)
4. [Component Architecture](#component-architecture)
5. [State Management](#state-management)
6. [Hooks System](#hooks-system)
7. [Event Handling](#event-handling)
8. [Conditional Rendering](#conditional-rendering)
9. [Lists and Keys](#lists-and-keys)
10. [Forms and Input](#forms-and-input)
11. [Data Fetching](#data-fetching)
12. [Performance Optimization](#performance-optimization)
13. [Best Practices](#best-practices)
14. [Real-World Examples](#real-world-examples)
15. [Practice Exercises](#practice-exercises)

---

## Introduction to React

### What is React?

React is a JavaScript library for building user interfaces, developed by Facebook (now Meta) in 2011. It has become the most popular front-end library for creating modern web applications. React uses a component-based architecture that allows developers to build reusable UI components.

```
┌─────────────────────────────────────────────────────────────┐
│                    REACT ECOSYSTEM                        │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐ │
│  │  React DOM  │  │  React     │  │   React Native      │ │
│  │  (Web)     │  │  Native    │  │   (Mobile)         │ │
│  └─────────────┘  └─────────────┘  └─────────────────────┘ │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐  │
│  │              React Core Library                     │  │
│  └─────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

### Why Learn React?

**Industry Statistics:**

- Over 10 million websites use React
- 80%+ of front-end developer positions require React
- Average React developer salary: $95,000-$150,000 annually

**Career Benefits:**

```javascript
// React skills that boost your career
const careerBenefits = {
  demand: "Very High",
  salary: "$95,000-$150,000",
  jobGrowth: "24% projected",
  topCompanies: ["Meta", "Netflix", " Airbnb", " Uber", "Instagram"],
  flexibility: ["Remote Work", "Freelancing", "Contract"]
};
```

### React vs Traditional JavaScript

**Traditional JavaScript Approach:**

```javascript
// Traditional DOM manipulation
document.addEventListener('DOMContentLoaded', function() {
  const button = document.getElementById('myButton');
  const display = document.getElementById('display');
  
  button.addEventListener('click', function() {
    const currentCount = parseInt(display.textContent);
    display.textContent = currentCount + 1;
  });
});
```

**React Approach:**

```jsx
// React component-based approach
function Counter() {
  const [count, setCount] = React.useState(0);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>
        Increment
      </button>
    </div>
  );
}
```

---

## React Fundamentals

### Setting Up React

**Using Create React App:**

```bash
# Create new React application
npx create-react-app my-app
cd my-app
npm start

# Using npm
npm create react-app my-app
```

**Using Vite (Faster Alternative):**

```bash
# Create React app with Vite
npm create vite@latest my-app -- --template react
cd my-app
npm install
npm run dev
```

**Project Structure:**

```
my-react-app/
├── public/
│   ├── index.html
│   ├── favicon.ico
│   └── manifest.json
├── src/
│   ├── App.js
│   ├── App.css
│   ├── index.js
│   └── index.css
├── package.json
└── README.md
```

### The Root File

**index.js - Entry Point:**

```javascript
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';

// Get root element
const root = ReactDOM.createRoot(
  document.getElementById('root')
);

// Render the App component
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
```

**index.html:**

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>React App</title>
</head>
<body>
  <div id="root"></div>
</body>
</html>
```

---

## JSX Deep Dive

### What is JSX?

JSX is a syntax extension for JavaScript that allows you to write HTML-like code in JavaScript. It looks like HTML but is actually JavaScript that gets compiled to React elements.

```jsx
// JSX code
const element = <h1>Hello, World!</h1>;

// Compiled to JavaScript
const element = React.createElement('h1', null, 'Hello, World!');
```

### JSX Rules

**Rule 1: Return Single Root Element**

```jsx
// ❌ Incorrect - Multiple root elements
function WrongComponent() {
  return (
    <h1>Title</h1>
    <p>Description</p>
  );
}

// ✅ Correct - Single root element with wrapper
function CorrectComponent() {
  return (
    <div>
      <h1>Title</h1>
      <p>Description</p>
    </div>
  );
}

// ✅ Correct - Using React Fragment
function FragmentComponent() {
  return (
    <>
      <h1>Title</h1>
      <p>Description</p>
    </>
  );
}
```

**Rule 2: Close All Tags**

```jsx
// ❌ Incorrect - Unclosed tag
const wrong = <img src="image.jpg">;

// ✅ Correct - Self-closing tag
const correct = <img src="image.jpg" />;

// ✅ Correct - Closing tag
const alsoCorrect = <input type="text"></input>;
```

**Rule 3: camelCase for Attributes**

```jsx
// ❌ Incorrect - kebab-case
const wrong = <div class-name="container">;

// ✅ Correct - camelCase
const correct = <div className="container">;

// ✅ Correct - inline styles use camelCase
const styleCorrect = (
  <div style={{backgroundColor: 'blue', fontSize: '16px'}}>
    Content
  </div>
);
```

### Embedding JavaScript in JSX

**Using Curly Braces:**

```jsx
function Greeting() {
  const name = 'John';
  const today = new Date();
  
  return (
    <div>
      <h1>Hello, {name}!</h1>
      <p>Today's date: {today.toDateString()}</p>
      <p>2 + 2 = {2 + 2}</p>
    </div>
  );
}
```

**Using JavaScript Expressions:**

```jsx
function MathDisplay() {
  const numbers = [1, 2, 3, 4, 5];
  
  return (
    <div>
      <p>Square of 5: {5 * 5}</p>
      <p>Maximum: {Math.max(...numbers)}</p>
      <p>Random: {Math.random()}</p>
      <p>Array length: {numbers.length}</p>
    </div>
  );
}
```

---

## Component Architecture

### Functional Components

**Basic Functional Component:**

```jsx
function Welcome() {
  return <h1>Welcome to React!</h1>;
}

// Arrow function version
const Welcome = () => <h1>Welcome to React!</h1>;

// With props
function Welcome(props) {
  return <h1>Welcome, {props.name}!</h1>;
}

// With destructuring
function Welcome({ name, age }) {
  return <h1>Welcome, {name}! You are {age} years old.</h1>;
}
```

**Component with State:**

```jsx
import { useState } from 'react';

function Counter() {
  const [count, setCount] = useState(0);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>
        Increment
      </button>
      <button onClick={() => setCount(count - 1)}>
        Decrement
      </button>
      <button onClick={() => setCount(0)}>
        Reset
      </button>
    </div>
  );
}
```

### Class Components

**Basic Class Component:**

```jsx
import React, { Component } from 'react';

class Welcome extends Component {
  render() {
    return <h1>Welcome to React!</h1>;
  }
}
```

**Class Component with State:**

```jsx
import React, { Component } from 'react';

class Counter extends Component {
  constructor(props) {
    super(props);
    this.state = {
      count: 0
    };
  }
  
  increment = () => {
    this.setState({ count: this.state.count + 1 });
  };
  
  decrement = () => {
    this.setState({ count: this.state.count - 1 });
  };
  
  render() {
    return (
      <div>
        <p>Count: {this.state.count}</p>
        <button onClick={this.increment}>Increment</button>
        <button onClick={this.decrement}>Decrement</button>
      </div>
    );
  }
}
```

**Lifecycle Methods:**

```jsx
import React, { Component } from 'react';

class LifecycleComponent extends Component {
  constructor(props) {
    super(props);
    this.state = { data: null };
    console.log('1. Constructor called');
  }
  
  static getDerivedStateFromProps(props, state) {
    console.log('2. getDerivedStateFromProps');
    return null;
  }
  
  componentDidMount() {
    console.log('3. componentDidMount');
    // Fetch data or set up subscriptions
    this.setState({ data: 'Loaded!' });
  }
  
  shouldComponentUpdate(nextProps, nextState) {
    console.log('4. shouldComponentUpdate');
    return true;
  }
  
  getSnapshotBeforeUpdate(prevProps, prevState) {
    console.log('5. getSnapshotBeforeUpdate');
    return null;
  }
  
  componentDidUpdate(prevProps, prevState, snapshot) {
    console.log('6. componentDidUpdate');
  }
  
  componentWillUnmount() {
    console.log('7. componentWillUnmount');
    // Clean up subscriptions
  }
  
  render() {
    console.log('Render method');
    return <div>State: {this.state.data}</div>;
  }
}
```

---

## State Management

### useState Hook

**Basic State:**

```jsx
import { useState } from 'react';

function BasicState() {
  const [message, setMessage] = useState('Hello');
  
  return (
    <div>
      <p>{message}</p>
      <button onClick={() => setMessage('Hi there!')}>
        Change Message
      </button>
    </div>
  );
}
```

**Multiple State Values:**

```jsx
import { useState } from 'react';

function MultipleState() {
  const [name, setName] = useState('');
  const [age, setAge] = useState(0);
  const [email, setEmail] = useState('');
  
  return (
    <form onSubmit={(e) => e.preventDefault()}>
      <input
        type="text"
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Enter name"
      />
      <input
        type="number"
        value={age}
        onChange={(e) => setAge(e.target.value)}
        placeholder="Enter age"
      />
      <input
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Enter email"
      />
      <p>Name: {name}</p>
      <p>Age: {age}</p>
      <p>Email: {email}</p>
    </form>
  );
}
```

**Object State:**

```jsx
import { useState } from 'react';

function ObjectState() {
  const [user, setUser] = useState({
    name: '',
    email: '',
    age: 0
  });
  
  const updateName = (e) => {
    setUser({ ...user, name: e.target.value });
  };
  
  const updateEmail = (e) => {
    setUser({ ...user, email: e.target.value });
  };
  
  const updateAge = (e) => {
    setUser({ ...user, age: e.target.value });
  };
  
  return (
    <div>
      <input value={user.name} onChange={updateName} />
      <input value={user.email} onChange={updateEmail} />
      <input value={user.age} onChange={updateAge} />
      <p>{JSON.stringify(user)}</p>
    </div>
  );
}
```

### useReducer Hook

**Basic Reducer:**

```jsx
import { useReducer } from 'react';

const initialState = { count: 0 };

function reducer(state, action) {
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

function CounterWithReducer() {
  const [state, dispatch] = useReducer(reducer, initialState);
  
  return (
    <div>
      <p>Count: {state.count}</p>
      <button onClick={() => dispatch({ type: 'increment' })}>
        +
      </button>
      <button onClick={() => dispatch({ type: 'decrement' })}>
        -
      </button>
      <button onClick={() => dispatch({ type: 'reset' })}>
        Reset
      </button>
    </div>
  );
}
```

**Complex Reducer:**

```jsx
import { useReducer } from 'react';

const initialState = {
  users: [],
  loading: false,
  error: null
};

function userReducer(state, action) {
  switch (action.type) {
    case 'FETCH_START':
      return { ...state, loading: true, error: null };
    case 'FETCH_SUCCESS':
      return { ...state, loading: false, users: action.payload };
    case 'FETCH_ERROR':
      return { ...state, loading: false, error: action.payload };
    case 'ADD_USER':
      return { ...state, users: [...state.users, action.payload] };
    case 'DELETE_USER':
      return {
        ...state,
        users: state.users.filter(user => user.id !== action.payload)
      };
    default:
      return state;
  }
}

function UserManager() {
  const [state, dispatch] = useReducer(userReducer, initialState);
  
  return (
    <div>
      {state.loading && <p>Loading...</p>}
      {state.error && <p>Error: {state.error}</p>}
      <ul>
        {state.users.map(user => (
          <li key={user.id}>{user.name}</li>
        ))}
      </ul>
    </div>
  );
}
```

---

## Hooks System

### useEffect Hook

**Basic useEffect:**

```jsx
import { useState, useEffect } from 'react';

function DataFetcher() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    // This runs after every render
    console.log('Effect ran!');
  });
  
  return <div>{loading ? 'Loading...' : data}</div>;
}
```

**useEffect with Cleanup:**

```jsx
import { useState, useEffect } from 'react';

function Timer() {
  const [seconds, setSeconds] = useState(0);
  
  useEffect(() => {
    const interval = setInterval(() => {
      setSeconds(s => s + 1);
    }, 1000);
    
    // Cleanup function
    return () => clearInterval(interval);
  }, []); // Empty array = run only once
  
  return <p>Seconds: {seconds}</p>;
}
```

**Fetching Data with useEffect:**

```jsx
import { useState, useEffect } from 'react';

function UserList() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    async function fetchUsers() {
      try {
        const response = await fetch('https://api.example.com/users');
        const data = await response.json();
        setUsers(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    
    fetchUsers();
  }, []); // Empty array = run once on mount
  
  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  
  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

### useContext Hook

**Creating Context:**

```jsx
import { createContext, useContext, useState } from 'react';

const ThemeContext = createContext();

function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };
  
  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

function ThemedButton() {
  const { theme, toggleTheme } = useContext(ThemeContext);
  
  const buttonStyle = {
    backgroundColor: theme === 'light' ? '#fff' : '#333',
    color: theme === 'light' ? '#333' : '#fff'
  };
  
  return (
    <button onClick={toggleTheme} style={buttonStyle}>
      Current Theme: {theme}
    </button>
  );
}

function App() {
  return (
    <ThemeProvider>
      <ThemedButton />
    </ThemeProvider>
  );
}
```

### Custom Hooks

**Creating Custom Hooks:**

```jsx
import { useState, useEffect } from 'react';

// Custom hook for local storage
function useLocalStorage(key, initialValue) {
  const [value, setValue] = useState(() => {
    const stored = localStorage.getItem(key);
    return stored ? JSON.parse(stored) : initialValue;
  });
  
  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(value));
  }, [key, value]);
  
  return [value, setValue];
}

// Using the custom hook
function StorageExample() {
  const [name, setName] = useLocalStorage('name', '');
  
  return (
    <input
      value={name}
      onChange={(e) => setName(e.target.value)}
      placeholder="Enter your name"
    />
  );
}
```

**Form Validation Hook:**

```jsx
import { useState } from 'react';

function useFormValidation(initialState, validate) {
  const [values, setValues] = useState(initialState);
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
    
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: null }));
    }
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    
    const validationErrors = validate(values);
    setErrors(validationErrors);
    
    if (Object.keys(validationErrors).length === 0) {
      // Form is valid - submit
      console.log('Form submitted:', values);
    }
    
    setIsSubmitting(false);
  };
  
  return { values, errors, isSubmitting, handleChange, handleSubmit };
}

// Using the hook
function LoginForm() {
  const validate = (values) => {
    const errors = {};
    if (!values.email) errors.email = 'Email is required';
    if (!values.password) errors.password = 'Password is required';
    if (values.password && values.password.length < 6) {
      errors.password = 'Password must be at least 6 characters';
    }
    return errors;
  };
  
  const { values, errors, handleChange, handleSubmit } = 
    useFormValidation({ email: '', password: '' }, validate);
  
  return (
    <form onSubmit={handleSubmit}>
      <input
        name="email"
        type="email"
        value={values.email}
        onChange={handleChange}
      />
      {errors.email && <span className="error">{errors.email}</span>}
      <input
        name="password"
        type="password"
        value={values.password}
        onChange={handleChange}
      />
      {errors.password && <span className="error">{errors.password}</span>}
      <button type="submit">Login</button>
    </form>
  );
}
```

---

## Event Handling

### Basic Event Handling

```jsx
function EventExample() {
  const handleClick = (event) => {
    console.log('Button clicked!');
    console.log('Event type:', event.type);
  };
  
  const handleMouseEnter = () => {
    console.log('Mouse entered!');
  };
  
  return (
    <button onClick={handleClick} onMouseEnter={handleMouseEnter}>
      Click Me
    </button>
  );
}
```

### Passing Arguments to Event Handlers

```jsx
function ArgumentExample() {
  const handleClick = (message, event) => {
    console.log(message);
    console.log('Event:', event);
  };
  
  return (
    <div>
      <button onClick={(e) => handleClick('Button 1 clicked', e)}>
        Button 1
      </button>
      <button onClick={(e) => handleClick('Button 2 clicked', e)}>
        Button 2
      </button>
    </div>
  );
}
```

### Handling Form Events

```jsx
import { useState } from 'react';

function FormEvents() {
  const [inputValue, setInputValue] = useState('');
  const [submitted, setSubmitted] = useState('');
  
  const handleChange = (e) => {
    setInputValue(e.target.value);
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(inputValue);
    setInputValue('');
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        value={inputValue}
        onChange={handleChange}
        placeholder="Enter text"
      />
      <button type="submit">Submit</button>
      {submitted && <p>Submitted: {submitted}</p>}
    </form>
  );
}
```

---

## Conditional Rendering

### Ternary Operator

```jsx
function TernaryExample({ isLoggedIn }) {
  return (
    <div>
      {isLoggedIn ? (
        <button>Logout</button>
      ) : (
        <button>Login</button>
      )}
    </div>
  );
}
```

### Logical && Operator

```jsx
function ConditionalList({ items }) {
  return (
    <div>
      {items.length === 0 && <p>No items found</p>}
      <ul>
        {items.map(item => (
          <li key={item.id}>{item.name}</li>
        ))}
      </ul>
    </div>
  );
}
```

### If-Else Statements

```jsx
function IfElseExample({ user }) {
  if (!user) {
    return <p>Please log in</p>;
  }
  
  if (user.isAdmin) {
    return <p>Welcome, Admin!</p>;
  }
  
  return <p>Welcome, User!</p>;
}
```

### Immediately Invoked Function

```jsx
function IIFEExample({ status }) {
  return (
    <div>
      {(() => {
        switch (status) {
          case 'loading':
            return <p>Loading...</p>;
          case 'success':
            return <p>Success!</p>;
          case 'error':
            return <p>Error occurred</p>;
          default:
            return <p>Unknown status</p>;
        }
      })()}
    </div>
  );
}
```

---

## Lists and Keys

### Mapping Arrays to Elements

```jsx
function ItemList({ items }) {
  return (
    <ul>
      {items.map(item => (
        <li key={item.id}>
          {item.name} - {item.price}
        </li>
      ))}
    </ul>
  );
}
```

### List with Index

```jsx
function NumberedList({ items }) {
  return (
    <ol>
      {items.map((item, index) => (
        <li key={index}>
          {index + 1}. {item}
        </li>
      ))}
    </ol>
  );
}
```

### Complex List Rendering

```jsx
function ProductGrid({ products }) {
  return (
    <div className="grid">
      {products.map(product => (
        <div key={product.id} className="card">
          <h3>{product.name}</h3>
          <p>{product.description}</p>
          <p>Price: ${product.price}</p>
          <button>Add to Cart</button>
        </div>
      ))}
    </div>
  );
}
```

---

## Forms and Input

### Controlled Components

```jsx
import { useState } from 'react';

function ControlledForm() {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: ''
  });
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Form data:', formData);
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <input
        name="username"
        value={formData.username}
        onChange={handleChange}
        placeholder="Username"
      />
      <input
        name="email"
        type="email"
        value={formData.email}
        onChange={handleChange}
        placeholder="Email"
      />
      <input
        name="password"
        type="password"
        value={formData.password}
        onChange={handleChange}
        placeholder="Password"
      />
      <button type="submit">Register</button>
    </form>
  );
}
```

### Select Elements

```jsx
function SelectExample() {
  const [selectedOption, setSelectedOption] = useState('');
  
  const options = [
    { value: 'apple', label: 'Apple' },
    { value: 'banana', label: 'Banana' },
    { value: 'orange', label: 'Orange' }
  ];
  
  return (
    <select 
      value={selectedOption}
      onChange={(e) => setSelectedOption(e.target.value)}
    >
      <option value="">Select a fruit</option>
      {options.map(option => (
        <option key={option.value} value={option.value}>
          {option.label}
        </option>
      ))}
    </select>
  );
}
```

### Textarea

```jsx
function TextareaExample() {
  const [message, setMessage] = useState('');
  
  return (
    <div>
      <textarea
        value={message}
        onChange={(e) => setMessage(e.target.value)}
        placeholder="Enter your message"
        rows={5}
        cols={30}
      />
      <p>Character count: {message.length}</p>
    </div>
  );
}
```

### Checkbox and Radio

```jsx
function CheckboxExample() {
  const [isChecked, setIsChecked] = useState(false);
  const [gender, setGender] = useState('male');
  
  return (
    <div>
      <label>
        <input
          type="checkbox"
          checked={isChecked}
          onChange={(e) => setIsChecked(e.target.checked)}
        />
        I agree to terms
      </label>
      
      <div>
        <label>
          <input
            type="radio"
            name="gender"
            value="male"
            checked={gender === 'male'}
            onChange={(e) => setGender(e.target.value)}
          />
          Male
        </label>
        <label>
          <input
            type="radio"
            name="gender"
            value="female"
            checked={gender === 'female'}
            onChange={(e) => setGender(e.target.value)}
          />
          Female
        </label>
      </div>
    </div>
  );
}
```

---

## Data Fetching

### Fetch API

```jsx
import { useState, useEffect } from 'react';

function FetchExample() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    fetch('https://api.example.com/data')
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        return response.json();
      })
      .then(data => {
        setData(data);
        setLoading(false);
      })
      .catch(error => {
        setError(error.message);
        setLoading(false);
      });
  }, []);
  
  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  return <div>{JSON.stringify(data)}</div>;
}
```

### Async/Await

```jsx
import { useState, useEffect } from 'react';

async function fetchData(url) {
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return response.json();
}

function AsyncFetchExample() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    async function loadData() {
      try {
        const result = await fetchData('https://api.example.com/data');
        setData(result);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    
    loadData();
  }, []);
  
  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  return <div>{JSON.stringify(data)}</div>;
}
```

### POST Request

```jsx
import { useState } from 'react';

function PostExample() {
  const [status, setStatus] = useState('');
  
  const handleSubmit = async (e) => {
    e.preventDefault();
    setStatus('submitting');
    
    try {
      const response = await fetch('https://api.example.com/users', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: 'John Doe',
          email: 'john@example.com'
        })
      });
      
      if (!response.ok) {
        throw new Error('Failed to create user');
      }
      
      const data = await response.json();
      setStatus('success: ' + JSON.stringify(data));
    } catch (error) {
      setStatus('error: ' + error.message);
    }
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <button type="submit">Create User</button>
      <p>{status}</p>
    </form>
  );
}
```

---

## Performance Optimization

### React.memo

```jsx
import { memo } from 'react';

const ExpensiveComponent = memo(function ExpensiveComponent({ data }) {
  // This component only re-renders when data changes
  console.log('Rendering ExpensiveComponent');
  return <div>{data.map(item => <p key={item.id}>{item.name}</p>)}</div>;
});
```

### useMemo

```jsx
import { useMemo } from 'react';

function MemoizedList({ items, filter }) {
  const filteredItems = useMemo(() => {
    console.log('Filtering items...');
    return items.filter(item => item.name.includes(filter));
  }, [items, filter]);
  
  return (
    <ul>
      {filteredItems.map(item => (
        <li key={item.id}>{item.name}</li>
      ))}
    </ul>
  );
}
```

### useCallback

```jsx
import { useState, useCallback } from 'react';

function CallbackExample() {
  const [count, setCount] = useState(0);
  
  // This function is memoized and won't change on every render
  const handleClick = useCallback(() => {
    console.log('Button clicked!');
  }, []);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={handleClick}>Click</button>
      <button onClick={() => setCount(count + 1)}>Increment</button>
    </div>
  );
}
```

### Lazy Loading

```jsx
import { lazy, Suspense } from 'react';

const LazyComponent = lazy(() => import('./HeavyComponent'));

function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <LazyComponent />
    </Suspense>
  );
}
```

---

## Best Practices

### Component Structure

```jsx
// 1. Imports
import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// 2. Component definition
function UserCard({ user, onEdit, onDelete }) {
  // 3. State
  const [isEditing, setIsEditing] = useState(false);
  const [name, setName] = useState(user.name);
  
  // 4. Effects
  useEffect(() => {
    setName(user.name);
  }, [user.name]);
  
  // 5. Helper functions
  const handleSave = () => {
    onEdit({ ...user, name });
    setIsEditing(false);
  };
  
  // 6. Render
  return (
    <div className="user-card">
      {isEditing ? (
        <input value={name} onChange={(e) => setName(e.target.value)} />
      ) : (
        <p>{user.name}</p>
      )}
      <button onClick={isEditing ? handleSave : () => setIsEditing(true)}>
        {isEditing ? 'Save' : 'Edit'}
      </button>
      <button onClick={() => onDelete(user.id)}>Delete</button>
    </div>
  );
}

// 7. PropTypes
UserCard.propTypes = {
  user: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired
  }).isRequired,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired
};

export default UserCard;
```

### Folder Structure

```
src/
├── components/
│   ├── common/
│   │   ├── Button/
│   │   │   ├── Button.jsx
│   │   │   └── Button.css
│   │   └── Input/
│   ├── features/
│   │   ├── auth/
│   │   └── users/
│   └── layouts/
├── pages/
├── hooks/
├── context/
├── utils/
├── services/
└── App.jsx
```

---

## Real-World Examples

### Todo Application

```jsx
import { useState } from 'react';

function TodoApp() {
  const [todos, setTodos] = useState([]);
  const [inputValue, setInputValue] = useState('');
  
  const addTodo = () => {
    if (!inputValue.trim()) return;
    
    setTodos([
      ...todos,
      {
        id: Date.now(),
        text: inputValue,
        completed: false
      }
    ]);
    setInputValue('');
  };
  
  const toggleTodo = (id) => {
    setTodos(todos.map(todo =>
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  };
  
  const deleteTodo = (id) => {
    setTodos(todos.filter(todo => todo.id !== id));
  };
  
  return (
    <div>
      <h1>Todo App</h1>
      <input
        value={inputValue}
        onChange={(e) => setInputValue(e.target.value)}
        placeholder="Add a todo..."
      />
      <button onClick={addTodo}>Add</button>
      <ul>
        {todos.map(todo => (
          <li key={todo.id}>
            <span
              style={{
                textDecoration: todo.completed ? 'line-through' : 'none'
              }}
              onClick={() => toggleTodo(todo.id)}
            >
              {todo.text}
            </span>
            <button onClick={() => deleteTodo(todo.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
```

### Weather Dashboard

```jsx
import { useState, useEffect } from 'react';

function WeatherDashboard() {
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(true);
  const [city, setCity] = useState('New York');
  
  useEffect(() => {
    async function fetchWeather() {
      setLoading(true);
      try {
        // Simulated API call
        await new Promise(resolve => setTimeout(resolve, 1000));
        setWeather({
          temp: 72,
          condition: 'Sunny',
          humidity: 45,
          wind: 10
        });
      } finally {
        setLoading(false);
      }
    }
    fetchWeather();
  }, [city]);
  
  if (loading) return <div>Loading weather for {city}...</div>;
  
  return (
    <div>
      <h1>Weather in {city}</h1>
      {weather && (
        <div>
          <p>Temperature: {weather.temp}°F</p>
          <p>Condition: {weather.condition}</p>
          <p>Humidity: {weather.humidity}%</p>
          <p>Wind: {weather.wind} mph</p>
        </div>
      )}
      <input
        value={city}
        onChange={(e) => setCity(e.target.value)}
        placeholder="Enter city name..."
      />
    </div>
  );
}
```

---

## Practice Exercises

### Exercise 1: Counter with Limits

**Requirements:**

- Create a counter that can only go from 0 to 10
- Disable increment button when at 10
- Disable decrement button when at 0
- Show "Maximum/Minimum reached" message

```jsx
function LimitedCounter() {
  const [count, setCount] = useState(0);
  
  const increment = () => {
    if (count < 10) setCount(count + 1);
  };
  
  const decrement = () => {
    if (count > 0) setCount(count - 1);
  };
  
  return (
    <div>
      <p>Count: {count}</p>
      {count === 10 && <p>Maximum reached!</p>}
      {count === 0 && <p>Minimum reached!</p>}
      <button onClick={increment} disabled={count >= 10}>
        +
      </button>
      <button onClick={decrement} disabled={count <= 0}>
        -
      </button>
    </div>
  );
}
```

### Exercise 2: Shopping Cart

**Requirements:**

- Add items to cart
- Remove items from cart
- Calculate total price
- Show item count

```jsx
function ShoppingCart() {
  const [items, setItems] = useState([]);
  const [total, setTotal] = useState(0);
  
  const addItem = (item) => {
    setItems([...items, { ...item, id: Date.now() }]);
    setTotal(total + item.price);
  };
  
  const removeItem = (id, price) => {
    setItems(items.filter(item => item.id !== id));
    setTotal(total - price);
  };
  
  return (
    <div>
      <h2>Shopping Cart ({items.length} items)</h2>
      <p>Total: ${total.toFixed(2)}</p>
      <button onClick={() => addItem({ name: 'Apple', price: 1.5 })}>
        Add Apple
      </button>
      <button onClick={() => addItem({ name: 'Banana', price: 0.75 })}>
        Add Banana
      </button>
      <ul>
        {items.map(item => (
          <li key={item.id}>
            {item.name} - ${item.price.toFixed(2)}
            <button onClick={() => removeItem(item.id, item.price)}>
              Remove
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}
```

### Exercise 3: Toggle Tabs

**Requirements:**

- Create tabbed interface
- Only show active tab content
- Highlight active tab

```jsx
function TabInterface() {
  const [activeTab, setActiveTab] = useState('home');
  
  const tabs = [
    { id: 'home', label: 'Home', content: 'Welcome to Home!' },
    { id: 'about', label: 'About', content: 'About us page.' },
    { id: 'contact', label: 'Contact', content: 'Contact us at...' }
  ];
  
  return (
    <div>
      <div className="tabs">
        {tabs.map(tab => (
          <button
            key={tab.id}
            className={activeTab === tab.id ? 'active' : ''}
            onClick={() => setActiveTab(tab.id)}
          >
            {tab.label}
          </button>
        ))}
      </div>
      <div className="content">
        {tabs.find(tab => tab.id === activeTab)?.content}
      </div>
    </div>
  );
}
```

---

## Summary

### Key Takeaways

1. **React Fundamentals**: Components, JSX, and props are the building blocks
2. **State Management**: useState and useReducer manage application state
3. **Hooks System**: useEffect, useContext, and custom hooks provide powerful functionality
4. **Event Handling**: React events differ slightly from native DOM events
5. **Performance**: memo, useMemo, and useCallback optimize rendering

### Next Steps

- Continue with: [02_REACT_COMPONENTS_MASTER.md](02_REACT_COMPONENTS_MASTER.md)
- Practice building projects
- Learn React Router
- Study state management libraries (Redux, Zustand)

### Related Resources

- [React Official Documentation](https://react.dev)
- [Create React App](https://create-react-app.dev)
- [Vite React Template](https://vitejs.dev)

---

## Cross-References

- **Next**: [02_REACT_COMPONENTS_MASTER.md](02_REACT_COMPONENTS_MASTER.md)

---

*Last updated: 2024*