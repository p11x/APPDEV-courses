# 🎣 React Hooks Complete Guide

## Mastering React Hooks for Professional Development

---

## Table of Contents

1. [Introduction to Hooks](#introduction-to-hooks)
2. [State Hooks (useState)](#state-hooks-usestate)
3. [Effect Hooks (useEffect)](#effect-hooks-useeffect)
4. [Context Hooks (useContext)](#context-hooks-usecontext)
5. [Ref Hooks (useRef)](#ref-hooks-useref)
6. [Callback Hooks (useCallback)](#callback-hooks-usecallback)
7. [Memo Hooks (useMemo)](#memo-hooks-usememo)
8. [Custom Hooks](#custom-hooks)
9. [Hook Rules and Best Practices](#hook-rules-and-best-practices)
10. [Real-World Examples](#real-world-examples)
11. [Practice Exercises](#practice-exercises)

---

## Introduction to Hooks

### What are Hooks?

Hooks are functions that let you "hook into" React state and lifecycle features from function components. They provide a more direct API to React concepts you already know.

```
┌─────────────────────────────────────────────────────────────┐
│                  REACT HOOKS ECOSYSTEM                   │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐  │
│  │   useState  │  │  useEffect  │  │   useContext    │  │
│  │   (State)   │  │  (Effects)  │  │   (Context)     │  │
│  └─────────────┘  └─────────────┘  └─────────────────┘  │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐  │
│  │   useRef   │  │ useCallback │  │    useMemo     │  │
│  │   (Refs)   │  │ (Callback)  │  │    (Memo)      │  │
│  └─────────────┘  └─────────────┘  └─────────────────┘  │
│                                                             │
│                    ▼                                        │
│         ┌─────────────────────┐                            │
│         │  CUSTOM HOOKS       │                            │
│         │  (Your own hooks!)  │                            │
│         └─────────────────────┘                            │
└─────────────────────────────────────────────────────────────┘
```

### Why Hooks?

**Before Hooks - Class Components:**

```jsx
class Counter extends React.Component {
  constructor(props) {
    super(props);
    this.state = { count: 0 };
  }
  
  componentDidMount() {
    console.log('Component mounted');
  }
  
  render() {
    return (
      <div>
        <p>Count: {this.state.count}</p>
        <button onClick={() => this.setState({ count: this.state.count + 1 })}>
          Increment
        </button>
      </div>
    );
  }
}
```

**After Hooks - Functional Components:**

```jsx
function Counter() {
  const [count, setCount] = React.useState(0);
  
  React.useEffect(() => {
    console.log('Component mounted');
  }, []);
  
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

## State Hooks (useState)

### Basic useState

```jsx
import { useState } from 'react';

function Counter() {
  const [count, setCount] = useState(0);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>+</button>
      <button onClick={() => setCount(count - 1)}>-</button>
      <button onClick={() => setCount(0)}>Reset</button>
    </div>
  );
}
```

### useState with Object

```jsx
import { useState } from 'react';

function UserForm() {
  const [user, setUser] = useState({
    name: '',
    email: '',
    age: 0
  });
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser(prev => ({ ...prev, [name]: value }));
  };
  
  return (
    <form>
      <input
        name="name"
        value={user.name}
        onChange={handleChange}
        placeholder="Name"
      />
      <input
        name="email"
        value={user.email}
        onChange={handleChange}
        placeholder="Email"
      />
      <input
        name="age"
        type="number"
        value={user.age}
        onChange={handleChange}
        placeholder="Age"
      />
    </form>
  );
}
```

### Multiple State Variables

```jsx
import { useState } from 'react';

function Form() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  const validate = () => {
    const newErrors = {};
    if (!name) newErrors.name = 'Name is required';
    if (!email) newErrors.email = 'Email is required';
    if (!password) newErrors.password = 'Password is required';
    if (password && password.length < 6) {
      newErrors.password = 'Password must be at least 6 characters';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    if (validate()) {
      setIsSubmitting(true);
      // Submit form
    }
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <input value={name} onChange={(e) => setName(e.target.value)} />
      {errors.name && <span>{errors.name}</span>}
      <input value={email} onChange={(e) => setEmail(e.target.value)} />
      {errors.email && <span>{errors.email}</span>}
      <input value={password} onChange={(e) => setPassword(e.target.value)} />
      {errors.password && <span>{errors.password}</span>}
      <button disabled={isSubmitting}>Submit</button>
    </form>
  );
}
```

### Functional State Updates

```jsx
import { useState } from 'react';

function Counter() {
  const [count, setCount] = useState(0);
  
  // Correct: Using functional update
  const increment = () => setCount(prev => prev + 1);
  const decrement = () => setCount(prev => prev - 1);
  const double = () => setCount(prev => prev * 2);
  const reset = () => setCount(0);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={increment}>+1</button>
      <button onClick={decrement}>-1</button>
      <button onClick={double}>Double</button>
      <button onClick={reset}>Reset</button>
    </div>
  );
}
```

### Lazy State Initialization

```jsx
import { useState } from 'react';

// Heavy computation function
function expensiveInitialState() {
  console.log('Expensive computation running...');
  return computeExpensiveValue(); // This runs once
}

function ExpensiveComponent() {
  // Only runs on initial render
  const [value, setValue] = useState(() => expensiveInitialState());
  
  // Can also use with a simple value
  const [items, setItems] = useState(() => {
    const stored = localStorage.getItem('items');
    return stored ? JSON.parse(stored) : [];
  });
  
  return <div>{value}</div>;
}
```

---

## Effect Hooks (useEffect)

### Basic useEffect

```jsx
import { useState, useEffect } from 'react';

function DataFetcher() {
  const [data, setData] = useState(null);
  
  // Runs after every render
  useEffect(() => {
    console.log('Effect ran!');
  });
  
  return <div>{data}</div>;
}
```

### useEffect with Cleanup

```jsx
import { useState, useEffect } from 'react';

function Timer() {
  const [seconds, setSeconds] = useState(0);
  
  useEffect(() => {
    const interval = setInterval(() => {
      setSeconds(s => s + 1);
    }, 1000);
    
    // Cleanup function - runs before unmount
    return () => clearInterval(interval);
  }, []); // Empty dependency array = run once
  
  return <p>Seconds: {seconds}</p>;
}
```

### Data Fetching with useEffect

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
        if (!response.ok) throw new Error('Failed to fetch');
        const data = await response.json();
        setUsers(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    
    fetchUsers();
  }, []); // Empty array = mount only
  
  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  
  return (
    <ul>
      {users.map(user => <li key={user.id}>{user.name}</li>)}
    </ul>
  );
}
```

### useEffect with Dependencies

```jsx
import { useState, useEffect } from 'react';

function SearchableList({ query }) {
  const [results, setResults] = useState([]);
  
  // Runs when query changes
  useEffect(() => {
    async function search() {
      const data = await performSearch(query);
      setResults(data);
    }
    
    if (query) {
      search();
    }
  }, [query]); // Re-run when query changes
  
  return (
    <ul>
      {results.map(result => <li key={result.id}>{result.name}</li>)}
    </ul>
  );
}
```

### Multiple useEffects

```jsx
import { useState, useEffect } from 'react';

function Dashboard({ userId }) {
  const [user, setUser] = useState(null);
  const [posts, setPosts] = useState([]);
  const [settings, setSettings] = useState(null);
  
  // Effect 1: Fetch user
  useEffect(() => {
    fetchUser(userId).then(setUser);
  }, [userId]);
  
  // Effect 2: Fetch posts
  useEffect(() => {
    if (userId) {
      fetchPosts(userId).then(setPosts);
    }
  }, [userId]);
  
  // Effect 3: Fetch settings
  useEffect(() => {
    fetchSettings().then(setSettings);
  }, []);
  
  // Effect 4: Update document title
  useEffect(() => {
    document.title = user ? `${user.name}'s Dashboard` : 'Dashboard';
  }, [user]);
  
  return <div>Dashboard content</div>;
}
```

---

## Context Hooks (useContext)

### Creating Context

```jsx
import { createContext, useContext, useState } from 'react';

const ThemeContext = createContext();

function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };
  
  return (
    <ThemeContext.Provider value={{ theme, setTheme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

function ThemedComponent() {
  const { theme, toggleTheme } = useContext(ThemeContext);
  
  return (
    <div className={theme}>
      <p>Current theme: {theme}</p>
      <button onClick={toggleTheme}>Toggle Theme</button>
    </div>
  );
}
```

### Multiple Contexts

```jsx
import { createContext, useContext, useState } from 'react';

const AuthContext = createContext();
const ThemeContext = createContext();

function AppProviders({ children }) {
  const [user, setUser] = useState(null);
  const [theme, setTheme] = useState('light');
  
  return (
    <AuthContext.Provider value={{ user, setUser }}>
      <ThemeContext.Provider value={{ theme, setTheme }}>
        {children}
      </ThemeContext.Provider>
    </AuthContext.Provider>
  );
}

function Dashboard() {
  const { user } = useContext(AuthContext);
  const { theme } = useContext(ThemeContext);
  
  return (
    <div className={theme}>
      <h1>Welcome, {user?.name}</h1>
    </div>
  );
}
```

---

## Ref Hooks (useRef)

### Basic useRef

```jsx
import { useRef } from 'react';

function FocusInput() {
  const inputRef = useRef(null);
  
  const handleFocus = () => {
    inputRef.current.focus();
  };
  
  return (
    <div>
      <input ref={inputRef} type="text" />
      <button onClick={handleFocus}>Focus Input</button>
    </div>
  );
}
```

### useRef for Keeping Values

```jsx
import { useRef, useState } from 'react';

function Counter() {
  const [count, setCount] = useState(0);
  const renderCount = useRef(0);
  
  renderCount.current++;
  
  return (
    <div>
      <p>Count: {count}</p>
      <p>Rendered: {renderCount.current} times</p>
      <button onClick={() => setCount(count + 1)}>Increment</button>
    </div>
  );
}
```

### useRef for Animations

```jsx
import { useRef, useEffect } from 'react';

function AnimatedBox() {
  const boxRef = useRef(null);
  
  useEffect(() => {
    const box = boxRef.current;
    let position = 0;
    let direction = 1;
    
    const animate = () => {
      position += direction * 2;
      if (position > 200 || position < 0) {
        direction *= -1;
      }
      box.style.transform = `translateX(${position}px)`;
      requestAnimationFrame(animate);
    };
    
    const frameId = requestAnimationFrame(animate);
    return () => cancelAnimationFrame(frameId);
  }, []);
  
  return (
    <div ref={boxRef} style={{
      width: 50,
      height: 50,
      background: 'blue'
    }} />
  );
}
```

---

## Callback Hooks (useCallback)

### Basic useCallback

```jsx
import { useState, useCallback } from 'react';

function Parent() {
  const [count, setCount] = useState(0);
  
  // Memoized callback - won't change unless count changes
  const handleClick = useCallback(() => {
    console.log('Count:', count);
  }, [count]);
  
  return <Child onClick={handleClick} />;
}

function Child({ onClick }) {
  return <button onClick={onClick}>Click me</button>;
}
```

### useCallback with Event Handlers

```jsx
import { useState, useCallback } from 'react';

function TodoList() {
  const [todos, setTodos] = useState([]);
  const [filter, setFilter] = useState('');
  
  const addTodo = useCallback((text) => {
    setTodos(prev => [...prev, { text, id: Date.now() }]);
  }, []);
  
  const removeTodo = useCallback((id) => {
    setTodos(prev => prev.filter(todo => todo.id !== id));
  }, []);
  
  const toggleTodo = useCallback((id) => {
    setTodos(prev => prev.map(todo =>
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  }, []);
  
  return (
    <div>
      <AddTodoForm onAdd={addTodo} />
      <FilterInput value={filter} onChange={setFilter} />
      <TodoItems todos={todos} onRemove={removeTodo} onToggle={toggleTodo} />
    </div>
  );
}
```

---

## Memo Hooks (useMemo)

### Basic useMemo

```jsx
import { useMemo } from 'react';

function ExpensiveComponent({ data, filter }) {
  // Only recalculates when data or filter changes
  const filteredData = useMemo(() => {
    console.log('Filtering...');
    return data.filter(item => 
      item.name.toLowerCase().includes(filter.toLowerCase())
    );
  }, [data, filter]);
  
  const sortedData = useMemo(() => {
    console.log('Sorting...');
    return [...filteredData].sort((a, b) => a.name.localeCompare(b.name));
  }, [filteredData]);
  
  return (
    <ul>
      {sortedData.map(item => <li key={item.id}>{item.name}</li>)}
    </ul>
  );
}
```

### useMemo for Objects

```jsx
import { useMemo } from 'react';

function UserProfile({ user, theme }) {
  // Memoize styles object to prevent unnecessary re-renders
  const styles = useMemo(() => ({
    container: {
      backgroundColor: theme === 'dark' ? '#333' : '#fff',
      color: theme === 'dark' ? '#fff' : '#333',
      padding: '20px'
    },
    name: {
      fontSize: '24px',
      fontWeight: 'bold'
    }
  }), [theme]);
  
  return (
    <div style={styles.container}>
      <p style={styles.name}>{user.name}</p>
      <p>{user.email}</p>
    </div>
  );
}
```

---

## Custom Hooks

### useLocalStorage Hook

```jsx
import { useState, useEffect } from 'react';

function useLocalStorage(key, initialValue) {
  // Get stored value or use initial
  const [value, setValue] = useState(() => {
    try {
      const item = localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      console.error(error);
      return initialValue;
    }
  });
  
  // Update localStorage when value changes
  useEffect(() => {
    try {
      localStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error(error);
    }
  }, [key, value]);
  
  return [value, setValue];
}

// Usage
function App() {
  const [name, setName] = useLocalStorage('name', '');
  const [theme, setTheme] = useLocalStorage('theme', 'light');
  
  return (
    <div>
      <input value={name} onChange={(e) => setName(e.target.value)} />
      <button onClick={() => setTheme(t => t === 'light' ? 'dark' : 'light')}>
        Toggle Theme
      </button>
    </div>
  );
}
```

### useDebounce Hook

```jsx
import { useState, useEffect } from 'react';

function useDebounce(value, delay = 500) {
  const [debouncedValue, setDebouncedValue] = useState(value);
  
  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);
    
    return () => clearTimeout(timer);
  }, [value, delay]);
  
  return debouncedValue;
}

// Usage
function SearchComponent() {
  const [query, setQuery] = useState('');
  const debouncedQuery = useDebounce(query, 300);
  
  useEffect(() => {
    if (debouncedQuery) {
      searchAPI(debouncedQuery);
    }
  }, [debouncedQuery]);
  
  return (
    <input
      value={query}
      onChange={(e) => setQuery(e.target.value)}
      placeholder="Search..."
    />
  );
}
```

### useFetch Hook

```jsx
import { useState, useEffect } from 'react';

function useFetch(url) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    const controller = new AbortController();
    
    async function fetchData() {
      try {
        setLoading(true);
        const response = await fetch(url, { signal: controller.signal });
        if (!response.ok) throw new Error('Failed to fetch');
        const json = await response.json();
        setData(json);
      } catch (err) {
        if (err.name !== 'AbortError') {
          setError(err.message);
        }
      } finally {
        setLoading(false);
      }
    }
    
    fetchData();
    
    return () => controller.abort();
  }, [url]);
  
  return { data, loading, error };
}

// Usage
function UserProfile({ userId }) {
  const { data, loading, error } = useFetch(`/api/users/${userId}`);
  
  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  
  return <div>{data?.name}</div>;
}
```

### useToggle Hook

```jsx
import { useState, useCallback } from 'react';

function useToggle(initialValue = false) {
  const [value, setValue] = useState(initialValue);
  
  const toggle = useCallback(() => {
    setValue(prev => !prev);
  }, []);
  
  const setTrue = useCallback(() => {
    setValue(true);
  }, []);
  
  const setFalse = useCallback(() => {
    setValue(false);
  }, []);
  
  return [value, { toggle, setTrue, setFalse, set: setValue }];
}

// Usage
function App() {
  const [isDarkMode, { toggle, setTrue, setFalse }] = useToggle(false);
  
  return (
    <div className={isDarkMode ? 'dark' : 'light'}>
      <button onClick={toggle}>Toggle Mode</button>
      <button onClick={setTrue}>Dark</button>
      <button onClick={setFalse}>Light</button>
    </div>
  );
}
```

---

## Hook Rules and Best Practices

### Rules of Hooks

```jsx
// Rule 1: Only call hooks at the top level
function Wrong() {
  // ❌ Don't call inside loops
  for (const item of items) {
    const [state, setState] = useState(item.id); // WRONG
  }
  
  // ❌ Don't call inside conditions
  if (condition) {
    useEffect(() => {}, []); // WRONG
  }
  
  // ✅ Correct - call at top level
  const [state1, setState1] = useState(1);
  const [state2, setState2] = useState(2);
}

// Rule 2: Only call hooks from React functions
function Wrong() {
  const handleClick = () => {
    const [state, setState] = useState(0); // WRONG
  };
  
  return <button onClick={handleClick}>Click</button>;
}

function Correct() {
  // ✅ Correct - call from component
  const [state, setState] = useState(0);
  
  return <button onClick={() => setState(state + 1)}>Click</button>;
}
```

### Best Practices

```jsx
// ✅ Use descriptive state names
function UserProfile() {
  const [userData, setUserData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState(null);
}

// ✅ Keep related state together
function Form() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: ''
  });
}

// ✅ Use reducer for complex state
function ComplexState() {
  const [state, dispatch] = useReducer(reducer, {
    users: [],
    selectedUser: null,
    loading: false
  });
}

// ✅ Memoize expensive calculations
function ExpensiveComponent({ items }) {
  const sortedItems = useMemo(
    () => [...items].sort((a, b) => b.value - a.value),
    [items]
  );
}
```

---

## Real-World Examples

### Infinite Scroll

```jsx
import { useState, useEffect, useRef } from 'react';

function InfiniteScroll({ fetchMore, hasMore }) {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const loaderRef = useRef(null);
  
  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        if (entries[0].isIntersecting && hasMore && !loading) {
          loadMore();
        }
      },
      { threshold: 1 }
    );
    
    if (loaderRef.current) {
      observer.observe(loaderRef.current);
    }
    
    return () => observer.disconnect();
  }, [loading, hasMore]);
  
  const loadMore = async () => {
    setLoading(true);
    const newItems = await fetchMore(items.length);
    setItems(prev => [...prev, ...newItems]);
    setLoading(false);
  };
  
  return (
    <div>
      {items.map(item => (
        <div key={item.id}>{item.content}</div>
      ))}
      <div ref={loaderRef}>
        {loading && <p>Loading more...</p>}
        {!hasMore && <p>No more items</p>}
      </div>
    </div>
  );
}
```

### Form with Validation

```jsx
import { useState, useCallback } from 'react';

function useFormValidation(initialValues, validationRules) {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});
  
  const handleChange = useCallback((e) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
    
    // Validate on change if field was touched
    if (touched[name]) {
      const error = validationRules[name]?.(value, values);
      setErrors(prev => ({ ...prev, [name]: error }));
    }
  }, [touched, values, validationRules]);
  
  const handleBlur = useCallback((e) => {
    const { name } = e.target;
    setTouched(prev => ({ ...prev, [name]: true }));
    
    const error = validationRules[name]?.(values[name], values);
    setErrors(prev => ({ ...prev, [name]: error }));
  }, [values, validationRules]);
  
  const handleSubmit = useCallback((e, onSubmit) => {
    e.preventDefault();
    
    // Validate all fields
    const newErrors = {};
    Object.keys(validationRules).forEach(name => {
      const error = validationRules[name](values[name], values);
      if (error) newErrors[name] = error;
    });
    
    setErrors(newErrors);
    setTouched(
      Object.keys(validationRules).reduce((acc, key) => ({ ...acc, [key]: true }), {})
    );
    
    if (Object.keys(newErrors).length === 0) {
      onSubmit(values);
    }
  }, [values, validationRules]);
  
  return {
    values,
    errors,
    touched,
    handleChange,
    handleBlur,
    handleSubmit
  };
}
```

---

## Practice Exercises

### Exercise 1: useWindowSize Hook

```jsx
import { useState, useEffect } from 'react';

function useWindowSize() {
  const [size, setSize] = useState({
    width: window.innerWidth,
    height: window.innerHeight
  });
  
  useEffect(() => {
    const handleResize = () => {
      setSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    };
    
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);
  
  return size;
}

function ResponsiveComponent() {
  const { width, height } = useWindowSize();
  
  return (
    <p>
      Window size: {width} x {height}
    </p>
  );
}
```

### Exercise 2: usePrevious Hook

```jsx
import { useRef, useEffect } from 'react';

function usePrevious(value) {
  const ref = useRef();
  
  useEffect(() => {
    ref.current = value;
  }, [value]);
  
  return ref.current;
}

function Counter() {
  const [count, setCount] = useState(0);
  const previousCount = usePrevious(count);
  
  return (
    <div>
      <p>Current: {count}</p>
      <p>Previous: {previousCount}</p>
      <button onClick={() => setCount(count + 1)}>Increment</button>
    </div>
  );
}
```

### Exercise 3: useKeyPress Hook

```jsx
import { useState, useEffect } from 'react';

function useKeyPress(targetKey) {
  const [keyPressed, setKeyPressed] = useState(false);
  
  useEffect(() => {
    const handleKeyDown = (e) => {
      if (e.key === targetKey) {
        setKeyPressed(true);
      }
    };
    
    const handleKeyUp = (e) => {
      if (e.key === targetKey) {
        setKeyPressed(false);
      }
    };
    
    window.addEventListener('keydown', handleKeyDown);
    window.addEventListener('keyup', handleKeyUp);
    
    return () => {
      window.removeEventListener('keydown', handleKeyDown);
      window.removeEventListener('keyup', handleKeyUp);
    };
  }, [targetKey]);
  
  return keyPressed;
}

function App() {
  const spacePressed = useKeyPress(' ');
  
  return (
    <div>
      {spacePressed ? 'Space is pressed!' : 'Press space'}
    </div>
  );
}
```

---

## Summary

### Key Takeaways

1. **useState**: Manages component state
2. **useEffect**: Handles side effects and lifecycle
3. **useContext**: Accesses context values
4. **useRef**: Mutable ref object
5. **useCallback**: Memoizes callbacks
6. **useMemo**: Memoizes values
7. **Custom Hooks**: Reusable stateful logic

### Next Steps

- Continue with: [04_REACT_ROUTING_MASTER.md](04_REACT_ROUTING_MASTER.md)
- Learn Redux for complex state management
- Study React Server Components

---

## Cross-References

- **Previous**: [02_REACT_COMPONENTS_MASTER.md](02_REACT_COMPONENTS_MASTER.md)
- **Next**: [04_REACT_ROUTING_MASTER.md](04_REACT_ROUTING_MASTER.md)

---

*Last updated: 2024*