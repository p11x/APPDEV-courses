# ⚡ React State Management Complete Guide

## Managing Application State in React

---

## Table of Contents

1. [Understanding State Management](#understanding-state-management)
2. [Local State with useState](#local-state-with-usestate)
3. [Global State Patterns](#global-state-patterns)
4. [Context API Deep Dive](#context-api-deep-dive)
5. [Redux Fundamentals](#redux-fundamentals)
6. [Zustand for Simple State](#zustand-for-simple-state)
7. [Server State with React Query](#server-state-with-react-query)
8. [State Machines with XState](#state-machines-with-xstate)
9. [Best Practices](#best-practices)
10. [Real-World Examples](#real-world-examples)

---

## Understanding State Management

```
┌─────────────────────────────────────────────────────────────┐
│              STATE MANAGEMENT OPTIONS                       │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  LOCAL STATE              GLOBAL STATE                       │
│  ─────────────          ───────────────                     │
│  ┌──────────┐          ┌────────────┐                   │
│  │useState  │          │  Context   │                   │
│  │useReducer│          │    API     │                   │
│  └──────────┘          └────────────┘                   │
│                              │                           │
│                              ▼                           │
│                    ┌────────────────────┐               │
│                    │  STATE LIBRARIES   │               │
│                    ├──────────────────┤                │
│                    │  Redux Toolkit   │               │
│                    │    Zustand     │               │
│                    │   Jotai      │               │
│                    │  Recoil      │               │
│                    └──────────────┘                   │
│                                                             │
│  SERVER STATE           COMPLEX STATE                        │
│  ───────────          ────────────                       │
│  ┌──────────┐          ┌────────────┐                   │
│  │ React   │          │  XState   │                   │
│  │ Query  │          │ (Machines)│                   │
│  └──────────┘          └────────────┘                   │
└─────────────────────────────────────────────────────────────┘
```

---

## Local State with useState

### Basic Pattern

```jsx
function Counter() {
  const [count, setCount] = useState(0);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>+</button>
    </div>
  );
}
```

### Complex State

```jsx
function Form() {
  const [form, setForm] = useState({
    name: '',
    email: '',
    password: ''
  });
  
  const updateField = (field, value) => {
    setForm(f => ({ ...f, [field]: value }));
  };
  
  return (
    <form>
      <input
        value={form.name}
        onChange={e => updateField('name', e.target.value)}
      />
      <input
        value={form.email}
        onChange={e => updateField('email', e.target.value)}
      />
      <input
        value={form.password}
        onChange={e => updateField('password', e.target.value)}
      />
    </form>
  );
}
```

### useReducer for Complex State

```jsx
const initialState = { count: 0, step: 1 };

function reducer(state, action) {
  switch (action.type) {
    case 'increment':
      return { ...state, count: state.count + state.step };
    case 'decrement':
      return { ...state, count: state.count - state.step };
    case 'setStep':
      return { ...state, step: action.payload };
    default:
      return state;
  }
}

function Counter() {
  const [state, dispatch] = useReducer(reducer, initialState);
  
  return (
    <div>
      <p>Count: {state.count}</p>
      <button onClick={() => dispatch({ type: 'increment' })}>+</button>
      <button onClick={() => dispatch({ type: 'decrement' })}>-</button>
      <input
        value={state.step}
        onChange={e => dispatch({ 
          type: 'setStep', 
          payload: Number(e.target.value) 
        })}
      />
    </div>
  );
}
```

---

## Global State Patterns

### Prop Drilling Problem

```jsx
// ❌ Prop drilling - difficult to maintain
function App() {
  const [user, setUser] = useState(null);
  
  return (
    <Header user={user} />
    <Main user={user} setUser={setUser} />
    <Footer user={user} />
  );
}

function Header({ user }) {
  return <nav>{user?.name}</nav>;
}
```

### Context Solution

```jsx
// ✅ Global state with Context
const UserContext = createContext();

function App() {
  const [user, setUser] = useState(null);
  
  return (
    <UserContext.Provider value={{ user, setUser }}>
      <Header />
      <Main />
      <Footer />
    </UserContext.Provider>
  );
}

function Header() {
  const { user } = useContext(UserContext);
  return <nav>{user?.name}</nav>;
}
```

---

## Context API Deep Dive

### Simple Context

```jsx
import { createContext, useContext, useState } from 'react';

const ThemeContext = createContext();

function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  return (
    <ThemeContext.Provider value={{ theme, setTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

function ThemedButton() {
  const { theme, setTheme } = useContext(ThemeContext);
  
  return (
    <button 
      onClick={() => setTheme(t => t === 'light' ? 'dark' : 'light')}
    >
      Theme: {theme}
    </button>
  );
}
```

### Context with Reducer

```jsx
import { createContext, useContext, useReducer } from 'react';

const StateContext = createContext();
const DispatchContext = createContext();

const initialState = { user: null, theme: 'light' };

function reducer(state, action) {
  switch (action.type) {
    case 'setUser':
      return { ...state, user: action.payload };
    case 'toggleTheme':
      return { ...state, theme: state.theme === 'light' ? 'dark' : 'light' };
    default:
      return state;
  }
}

function Provider({ children }) {
  const [state, dispatch] = useReducer(reducer, initialState);
  
  return (
    <StateContext.Provider value={state}>
      <DispatchContext.Provider value={dispatch}>
        {children}
      </DispatchContext.Provider>
    </StateContext.Provider>
  );
}

function UserDisplay() {
  const state = useContext(StateContext);
  return <p>{state.user?.name}</p>;
}
```

### Optimizing Context

```jsx
// Split contexts for different parts
const UserContext = createContext();
const ThemeContext = createContext();

// Or use memo to prevent unnecessary re-renders
const ThemeContext = createContext();

function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  
  const value = useMemo(() => ({
    theme,
    setTheme
  }), [theme]);
  
  return (
    <ThemeContext.Provider value={value}>
      {children}
    </ThemeContext.Provider>
  );
}
```

---

## Redux Fundamentals

### Setting Up Redux

```bash
npm install @reduxjs/toolkit react-redux
```

### Redux Store

```jsx
import { configureStore, createSlice } from '@reduxjs/toolkit';

const counterSlice = createSlice({
  name: 'counter',
  initialState: { value: 0 },
  reducers: {
    increment: (state) => {
      state.value += 1;
    },
    decrement: (state) => {
      state.value -= 1;
    },
    incrementBy: (state, action) => {
      state.value += action.payload;
    }
  }
});

export const { increment, decrement, incrementBy } = counterSlice.actions;

export const store = configureStore({
  reducer: {
    counter: counterSlice.reducer
  }
});
```

### Using Redux in Components

```jsx
import { useSelector, useDispatch } from 'react-redux';
import { increment, decrement } from './counterSlice';

function Counter() {
  const count = useSelector(state => state.counter.value);
  const dispatch = useDispatch();
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => dispatch(increment())}>+</button>
      <button onClick={() => dispatch(decrement())}>-</button>
    </div>
  );
}
```

### Redux with Async

```jsx
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';

export const fetchUser = createAsyncThunk(
  'user/fetchUser',
  async (userId) => {
    const response = await fetch(`/api/users/${userId}`);
    return response.json();
  }
);

const userSlice = createSlice({
  name: 'user',
  initialState: { 
    data: null, 
    status: 'idle',
    error: null 
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchUser.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchUser.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.data = action.payload;
      })
      .addCase(fetchUser.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      });
  }
});
```

---

## Zustand for Simple State

### Basic Zustand Store

```bash
npm install zustand
```

```jsx
import { create } from 'zustand';

const useStore = create(set => ({
  count: 0,
  increment: () => set(state => ({ count: state.count + 1 })),
  decrement: () => set(state => ({ count: state.count - 1 })),
  reset: () => set({ count: 0 })
}));

function Counter() {
  const { count, increment, decrement, reset } = useStore();
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={increment}>+</button>
      <button onClick={decrement}>-</button>
      <button onClick={reset}>Reset</button>
    </div>
  );
}
```

### Zustand with Persistence

```jsx
import { create } from 'zustand';
import { persist } from 'zustand/middleware';

const useStore = create(
  persist(
    (set) => ({
      user: null,
      setUser: (user) => set({ user }),
      clearUser: () => set({ user: null })
    }),
    {
      name: 'user-storage'
    }
  )
);
```

### Zustand with Async

```jsx
import { create } from 'zustand';

const useUserStore = create((set) => ({
  user: null,
  status: 'idle',
  fetchUser: async (userId) => {
    set({ status: 'loading' });
    try {
      const response = await fetch(`/api/users/${userId}`);
      const user = await response.json();
      set({ user, status: 'succeeded' });
    } catch (error) {
      set({ status: 'failed' });
    }
  }
}));

function UserProfile({ userId }) {
  const { user, status, fetchUser } = useUserStore();
  
  useEffect(() => {
    fetchUser(userId);
  }, [userId, fetchUser]);
  
  if (status === 'loading') return <p>Loading...</p>;
  
  return <div>{user?.name}</div>;
}
```

---

## Server State with React Query

### Setting Up

```bash
npm install @tanstack/react-query
```

```jsx
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <UserList />
    </QueryClientProvider>
  );
}
```

### Basic Queries

```jsx
import { useQuery } from '@tanstack/react-query';

function UserList() {
  const { data, isLoading, error } = useQuery({
    queryKey: ['users'],
    queryFn: () => fetch('/api/users').then(res => res.json())
  });
  
  if (isLoading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;
  
  return (
    <ul>
      {data.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

### Mutations

```jsx
import { useMutation, useQueryClient } from '@tanstack/react-query';

function AddUserForm() {
  const queryClient = useQueryClient();
  
  const mutation = useMutation({
    mutationFn: (newUser) => 
      fetch('/api/users', {
        method: 'POST',
        body: JSON.stringify(newUser)
      }).then(res => res.json()),
    onSuccess: () => {
      queryClient.invalidateQueries(['users']);
    }
  });
  
  const handleSubmit = (e) => {
    e.preventDefault();
    mutation.mutate({ name: 'John' });
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <button type="submit" disabled={mutation.isPending}>
        Add User
      </button>
    </form>
  );
}
```

---

## State Machines with XState

### Basic Machine

```bash
npm install xstate @xstate/react
```

```jsx
import { createMachine } from 'xstate';
import { useMachine } from '@xstate/react';

const toggleMachine = createMachine({
  id: 'toggle',
  initial: 'inactive',
  states: {
    inactive: {
      on: { TOGGLE: 'active' }
    },
    active: {
      on: { TOGGLE: 'inactive' }
    }
  }
});

function Toggle() {
  const [state, send] = useMachine(toggleMachine);
  
  return (
    <button onClick={() => send({ type: 'TOGGLE' })}>
      {state.value === 'active' ? 'ON' : 'OFF'}
    </button>
  );
}
```

### Complex Machine

```jsx
const fetchMachine = createMachine({
  id: 'fetch',
  initial: 'idle',
  states: {
    idle: {
      on: { FETCH: 'loading' }
    },
    loading: {
      invoke: {
        src: 'fetchData',
        onDone: { target: 'success', actions: 'assignData' },
        onError: { target: 'error', actions: 'assignError' }
      }
    },
    success: {
      on: { FETCH: 'loading' }
    },
    error: {
      on: { FETCH: 'loading' }
    }
  }
});

function DataFetcher() {
  const [state, send] = useMachine(fetchMachine, {
    services: {
      fetchData: () => fetch('/api/data').then(r => r.json())
    }
  });
  
  switch (state.value) {
    case 'idle':
      return <button onClick={() => send({ type: 'FETCH' })}>Load</button>;
    case 'loading':
      return <p>Loading...</p>;
    case 'success':
      return <p>{state.context.data}</p>;
    case 'error':
      return <p>Error: {state.context.error}</p>;
  }
}
```

---

## Best Practices

### Choosing State Solution

```
WHEN TO USE WHAT:
────────────────────────────────────────────

useState:
  - Component-local state
  - Simple UI state (modals, toggles)
  - Form inputs

useReducer:
  - Complex state logic
  - Multiple sub-values
  - Predictable transitions

Context:
  - Cross-cutting concerns
  - Theme, auth, locale
  - Low-frequency updates

Redux/Zustand:
  - Global app state
  - Many components need access
  - Complex updates

React Query:
  - Server data
  - Caching needed
  - Background updates

XState:
  - Complex flows
  - Finite states
  - Guided flows
```

### Performance Tips

```jsx
// ✅ Split large state
function GoodExample() {
  const [user, setUser] = useState(null);
  const [theme, setTheme] = useState('light');
  // Separate contexts prevent full re-renders
}

// ❌ Combined state causes re-renders
function BadExample() {
  const [state, setState] = useState({ user: null, theme: 'light' });
  // Any change triggers all component re-renders
}
```

---

## Real-World Examples

### Auth State

```jsx
import { create } from 'zustand';
import { persist } from 'zustand/middleware';

const useAuthStore = create(
  persist(
    (set, get) => ({
      user: null,
      token: null,
      
      login: async (email, password) => {
        const res = await fetch('/api/auth/login', {
          method: 'POST',
          body: JSON.stringify({ email, password })
        });
        const { user, token } = await res.json();
        set({ user, token });
      },
      
      logout: () => {
        set({ user: null, token: null });
      },
      
      isAuthenticated: () => !!get().token
    }),
    { name: 'auth-storage' }
  )
);
```

### Cart State

```jsx
import { create } from 'zustand';

const useCartStore = create((set, get) => ({
  items: [],
  
  addItem: (item) => {
    const { items } = get();
    const existing = items.find(i => i.id === item.id);
    
    if (existing) {
      set({
        items: items.map(i => 
          i.id === item.id 
            ? { ...i, quantity: i.quantity + 1 }
            : i
        )
      });
    } else {
      set({ items: [...items, { ...item, quantity: 1 }] });
    }
  },
  
  removeItem: (id) => {
    set({ items: get().items.filter(i => i.id !== id) });
  },
  
  clearCart: () => set({ items: [] }),
  
  total: () => {
    return get().items.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );
  }
}));
```

---

## Summary

### Key Takeaways

1. **useState**: Local component state
2. **useReducer**: Complex local state
3. **Context**: Cross-component state
4. **Redux**: Global state management
5. **Zustand**: Simple global state
6. **React Query**: Server state caching
7. **XState**: State machines

### Next Steps

- Continue with: [06_REACT_FORMS_MASTER.md](06_REACT_FORMS_MASTER.md)
- Practice with different state libraries
- Implement complex flows with XState

---

## Cross-References

- **Previous**: [04_REACT_ROUTING_MASTER.md](04_REACT_ROUTING_MASTER.md)
- **Next**: [06_REACT_FORMS_MASTER.md](06_REACT_FORMS_MASTER.md)

---

*Last updated: 2024*