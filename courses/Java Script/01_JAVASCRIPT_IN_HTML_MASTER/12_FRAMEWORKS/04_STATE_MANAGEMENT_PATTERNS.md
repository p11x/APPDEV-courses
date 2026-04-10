# State Management Patterns

Comprehensive guide to state management in JavaScript applications. Covers Redux, MobX, Vuex, Context API, and when to use each solution.

## Table of Contents

1. [Understanding State Management](#understanding-state-management)
2. [Local State with useState](#local-state-with-usestate)
3. [Context API Pattern](#context-api-pattern)
4. [Redux Pattern](#redux-pattern)
5. [MobX Pattern](#mobx-pattern)
6. [Vuex Pattern](#vuex-pattern)
7. [Zustand Pattern](#zustand-pattern)
8. [Jotai Pattern](#jotai-pattern)
9. [State Machine Pattern](#state-machine-pattern)
10. [Choosing the Right Solution](#choosing-the-right-solution)
11. [Key Takeaways](#key-takeaways)
12. [Common Pitfalls](#common-pitfalls)

---

## Understanding State Management

State management is critical for building maintainable applications. Understanding when and how to manage state prevents common bugs and performance issues.

### State Categories

| Type | Lifetime | Complexity |
|------|----------|-------------|
| Local/Component | Component | Low |
| Cross-component | Feature | Medium |
| Application | App-wide | High |
| Server/Cached | Session | High |

---

## Local State with useState

### Basic Patterns

```javascript
// file: state/useStateBasics.jsx
import React, { useState, useCallback } from 'react';

// Simple counter with local state
const Counter = () => {
  const [count, setCount] = useState(0);

  const increment = useCallback(() => {
    setCount((c) => c + 1);
  }, []);

  const decrement = useCallback(() => {
    setCount((c) => c - 1);
  }, []);

  const reset = useCallback(() => {
    setCount(0);
  }, []);

  return (
    <div>
      <h2>Count: {count}</h2>
      <button onClick={increment}>+</button>
      <button onClick={decrement}>-</button>
      <button onClick={reset}>Reset</button>
    </div>
  );
};

// Form handling with local state
const LoginForm = ({ onSubmit }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  const validate = useCallback(() => {
    const newErrors = {};
    
    if (!email) {
      newErrors.email = 'Email is required';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      newErrors.email = 'Invalid email format';
    }
    
    if (!password) {
      newErrors.password = 'Password is required';
    } else if (password.length < 8) {
      newErrors.password = 'Password must be at least 8 characters';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }, [email, password]);

  const handleSubmit = useCallback(async (e) => {
    e.preventDefault();
    
    if (!validate()) return;
    
    setIsSubmitting(true);
    try {
      await onSubmit({ email, password });
    } finally {
      setIsSubmitting(false);
    }
  }, [email, password, onSubmit, validate]);

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Email</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        {errors.email && <span>{errors.email}</span>}
      </div>
      
      <div>
        <label>Password</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        {errors.password && <span>{errors.password}</span>}
      </div>
      
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
};

export { Counter, LoginForm };
```

---

## Context API Pattern

### Simple Context Implementation

```javascript
// file: state/context/ThemeContext.jsx
import React, { createContext, useContext, useState, useCallback } from 'react';

const ThemeContext = createContext(null);

export const ThemeProvider = ({ children }) => {
  const [theme, setTheme] = useState('light');
  const [isDark, setIsDark] = useState(false);

  const toggleTheme = useCallback(() => {
    setTheme((prev) => (prev === 'light' ? 'dark' : 'light'));
    setIsDark((prev) => !prev);
  }, []);

  const value = {
    theme,
    isDark,
    toggleTheme,
    colors: isDark ? darkColors : lightColors,
  };

  return (
    <ThemeContext.Provider value={value}>
      {children}
    </ThemeContext.Provider>
  );
};

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (!context) {
    throw new Error('useTheme must be used within ThemeProvider');
  }
  return context;
};

const lightColors = {
  background: '#ffffff',
  text: '#000000',
  primary: '#0066cc',
};

const darkColors = {
  background: '#1a1a1a',
  text: '#ffffff',
  primary: '#4da6ff',
};
```

### Complex Context with Reducer

```javascript
// file: state/context/AuthContext.jsx
import React, { createContext, useContext, useReducer, useCallback } from 'react';

const AuthContext = createContext(null);

const initialState = {
  user: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,
  token: null,
};

const authReducer = (state, action) => {
  switch (action.type) {
    case 'LOGIN_START':
      return {
        ...state,
        isLoading: true,
        error: null,
      };
    case 'LOGIN_SUCCESS':
      return {
        ...state,
        isLoading: false,
        isAuthenticated: true,
        user: action.payload.user,
        token: action.payload.token,
      };
    case 'LOGIN_FAILURE':
      return {
        ...state,
        isLoading: false,
        error: action.payload,
      };
    case 'LOGOUT':
      return {
        ...initialState,
      };
    case 'UPDATE_PROFILE':
      return {
        ...state,
        user: { ...state.user, ...action.payload },
      };
    default:
      return state;
  }
};

export const AuthProvider = ({ children }) => {
  const [state, dispatch] = useReducer(authReducer, initialState);

  const login = useCallback(async (email, password) => {
    dispatch({ type: 'LOGIN_START' });
    
    try {
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
      });
      
      if (!response.ok) {
        throw new Error('Login failed');
      }
      
      const data = await response.json();
      dispatch({ type: 'LOGIN_SUCCESS', payload: data });
    } catch (error) {
      dispatch({ type: 'LOGIN_FAILURE', payload: error.message });
    }
  }, []);

  const logout = useCallback(() => {
    dispatch({ type: 'LOGOUT' });
  }, []);

  const updateProfile = useCallback((updates) => {
    dispatch({ type: 'UPDATE_PROFILE', payload: updates });
  }, []);

  return (
    <AuthContext.Provider value={{ ...state, login, logout, updateProfile }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};
```

---

## Redux Pattern

### Store Setup

```javascript
// file: state/redux/store.js
import { configureStore, createSlice, createAsyncThunk } from '@reduxjs/toolkit';

const initialState = {
  entities: [],
  ids: [],
  loading: false,
  error: null,
  selectedId: null,
};

const usersSlice = createSlice({
  name: 'users',
  initialState,
  reducers: {
    addUser: (state, action) => {
      const user = action.payload;
      state.entities.push(user);
      state.ids.push(user.id);
    },
    updateUser: (state, action) => {
      const { id, ...updates } = action.payload;
      const index = state.ids.indexOf(id);
      if (index !== -1) {
        state.entities[index] = { ...state.entities[index], ...updates };
      }
    },
    deleteUser: (state, action) => {
      const id = action.payload;
      const index = state.ids.indexOf(id);
      if (index !== -1) {
        state.entities.splice(index, 1);
        state.ids.splice(index, 1);
      }
    },
    setSelectedUser: (state, action) => {
      state.selectedId = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchUsers.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUsers.fulfilled, (state, action) => {
        state.loading = false;
        state.entities = action.payload;
        state.ids = action.payload.map((u) => u.id);
      })
      .addCase(fetchUsers.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      });
  },
});

export const fetchUsers = createAsyncThunk('users/fetchUsers', async () => {
  const response = await fetch('/api/users');
  return response.json();
});

export const { addUser, updateUser, deleteUser, setSelectedUser } = usersSlice.actions;

export const selectUsers = (state) => state.users.entities;
export const selectUserById = (state, id) =>
  state.users.entities.find((u) => u.id === id);
export const selectSelectedUser = (state) =>
  state.users.entities.find((u) => u.id === state.users.selectedId);

export const store = configureStore({
  reducer: {
    users: usersSlice.reducer,
  },
});

export default store;
```

### React Redux Integration

```javascript
// file: state/redux/UserList.jsx
import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { fetchUsers, deleteUser, selectUsers } from './store';

const UserList = () => {
  const users = useSelector(selectUsers);
  const loading = useSelector((state) => state.users.loading);
  const error = useSelector((state) => state.users.error);
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(fetchUsers());
  }, [dispatch]);

  const handleDelete = (id) => {
    if (confirm('Are you sure?')) {
      dispatch(deleteUser(id));
    }
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <ul>
      {users.map((user) => (
        <li key={user.id}>
          {user.name} - {user.email}
          <button onClick={() => handleDelete(user.id)}>Delete</button>
        </li>
      ))}
    </ul>
  );
};

export default UserList;
```

### Redux with Middleware

```javascript
// file: state/redux/middleware.js
import { createSlice, configureStore } from '@reduxjs/toolkit';

const loggingMiddleware = (store) => (next) => (action) => {
  console.log('Dispatching:', action);
  const result = next(action);
  console.log('New state:', store.getState());
  return result;
};

const analyticsMiddleware = (store) => (next) => (action) => {
  if (action.type.startsWith('users/')) {
    const state = store.getState();
    console.log('Analytics: tracked', action.type, state.users.ids.length);
  }
  return next(action);
};

const errorMiddleware = (store) => (next) => (action) => {
  try {
    return next(action);
  } catch (error) {
    console.error('Error:', error);
    return { type: 'ERROR', payload: error.message };
  }
};

const uiSlice = createSlice({
  name: 'ui',
  initialState: {
    notifications: [],
    modals: {},
    sidebarOpen: false,
  },
  reducers: {
    showNotification: (state, action) => {
      state.notifications.push({
        id: Date.now(),
        message: action.payload.message,
        type: action.payload.type || 'info',
        duration: action.payload.duration || 3000,
      });
    },
    dismissNotification: (state, action) => {
      state.notifications = state.notifications.filter(
        (n) => n.id !== action.payload
      );
    },
    openModal: (state, action) => {
      state.modals[action.payload] = true;
    },
    closeModal: (state, action) => {
      state.modals[action.payload] = false;
    },
    toggleSidebar: (state) => {
      state.sidebarOpen = !state.sidebarOpen;
    },
  },
});

export const {
  showNotification,
  dismissNotification,
  openModal,
  closeModal,
  toggleSidebar,
} = uiSlice.actions;

export const uiStore = configureStore({
  reducer: {
    ui: uiSlice.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(
      loggingMiddleware,
      analyticsMiddleware,
      errorMiddleware
    ),
});
```

---

## MobX Pattern

### Observable Store

```javascript
// file: state/mobx/UserStore.js
import { makeAutoObservable, runInAction } from 'mobx';

class UserStore {
  users = [];
  isLoading = false;
  error = null;
  selectedUser = null;

  constructor() {
    makeAutoObservable(this);
  }

  get userCount() {
    return this.users.length;
  }

  get activeUsers() {
    return this.users.filter((u) => u.isActive);
  }

  addUser(user) {
    this.users.push(user);
  }

  updateUser(id, updates) {
    const index = this.users.findIndex((u) => u.id === id);
    if (index !== -1) {
      this.users[index] = { ...this.users[index], ...updates };
    }
  }

  deleteUser(id) {
    this.users = this.users.filter((u) => u.id !== id);
    if (this.selectedUser?.id === id) {
      this.selectedUser = null;
    }
  }

  setSelectedUser(id) {
    this.selectedUser = this.users.find((u) => u.id === id) || null;
  }

  async fetchUsers() {
    this.isLoading = true;
    this.error = null;

    try {
      const response = await fetch('/api/users');
      if (!response.ok) throw new Error('Failed to fetch');
      
      const users = await response.json();
      runInAction(() => {
        this.users = users;
        this.isLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.error = error.message;
        this.isLoading = false;
      });
    }
  }
}

export const userStore = new UserStore();
```

### React MobX Integration

```javascript
// file: state/mobx/UserList.jsx
import React from 'react';
import { observer } from 'mobx-react-lite';
import { userStore } from './UserStore';

const UserList = observer(() => {
  const { users, isLoading, error } = userStore;

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <ul>
      {users.map((user) => (
        <li key={user.id}>
          {user.name} - {user.email}
        </li>
      ))}
    </ul>
  );
});

export default UserList;
```

---

## Vuex Pattern

### Vuex Modules

```javascript
// file: state/vuex/store.js
import { createStore } from 'vuex';

export const store = createStore({
  state: {
    users: [],
    loading: false,
    error: null,
    currentUser: null,
  },

  getters: {
    userCount: (state) => state.users.length,
    activeUsers: (state) => state.users.filter((u) => u.isActive),
    getUserById: (state) => (id) =>
      state.users.find((u) => u.id === id),
  },

  mutations: {
    SET_USERS(state, users) {
      state.users = users;
    },
    ADD_USER(state, user) {
      state.users.push(user);
    },
    UPDATE_USER(state, { id, updates }) {
      const index = state.users.findIndex((u) => u.id === id);
      if (index !== -1) {
        state.users[index] = { ...state.users[index], ...updates };
      }
    },
    DELETE_USER(state, id) {
      state.users = state.users.filter((u) => u.id !== id);
    },
    SET_LOADING(state, loading) {
      state.loading = loading;
    },
    SET_ERROR(state, error) {
      state.error = error;
    },
    SET_CURRENT_USER(state, user) {
      state.currentUser = user;
    },
  },

  actions: {
    async fetchUsers({ commit }) {
      commit('SET_LOADING', true);
      try {
        const response = await fetch('/api/users');
        if (!response.ok) throw new Error('Failed to fetch');
        const users = await response.json();
        commit('SET_USERS', users);
      } catch (error) {
        commit('SET_ERROR', error.message);
      } finally {
        commit('SET_LOADING', false);
      }
    },

    async createUser({ commit }, user) {
      commit('SET_LOADING', true);
      try {
        const response = await fetch('/api/users', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(user),
        });
        if (!response.ok) throw new Error('Failed to create');
        const newUser = await response.json();
        commit('ADD_USER', newUser);
      } catch (error) {
        commit('SET_ERROR', error.message);
      } finally {
        commit('SET_LOADING', false);
      }
    },

    deleteUser({ commit }, id) {
      commit('SET_LOADING', true);
      fetch(`/api/users/${id}`, { method: 'DELETE' })
        .then(() => commit('DELETE_USER', id))
        .catch((error) => commit('SET_ERROR', error.message))
        .finally(() => commit('SET_LOADING', false));
    },
  },

  modules: {
    auth: {
      state: {
        token: null,
        user: null,
      },
      getters: {
        isAuthenticated: (state) => !!state.token,
      },
      mutations: {
        SET_TOKEN(state, token) {
          state.token = token;
        },
        SET_USER(state, user) {
          state.user = user;
        },
        LOGOUT(state) {
          state.token = null;
          state.user = null;
        },
      },
    },
  },
});

export default store;
```

### Component Usage

```vue
<!-- file: state/vuex/UserList.vue -->
<template>
  <div>
    <div v-if="loading">Loading...</div>
    <div v-else-if="error">Error: {{ error }}</div>
    <ul v-else>
      <li v-for="user in users" :key="user.id">
        {{ user.name }} - {{ user.email }}
        <button @click="handleDelete(user.id)">Delete</button>
      </li>
    </ul>
  </div>
</template>

<script setup>
import { computed, onMounted } from 'vue';
import { useStore } from 'vuex';

const store = useStore();

const users = computed(() => store.state.users);
const loading = computed(() => store.state.loading);
const error = computed(() => store.state.error);

onMounted(() => {
  store.dispatch('fetchUsers');
});

const handleDelete = (id) => {
  if (confirm('Are you sure?')) {
    store.dispatch('deleteUser', id);
  }
};
</script>
```

---

## Zustand Pattern

### Simple Store

```javascript
// file: state/zustand/userStore.js
import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';

const useUserStore = create(
  devtools(
    persist(
      (set, get) => ({
        users: [],
        selectedUser: null,
        isLoading: false,
        error: null,

        addUser: (user) =>
          set((state) => ({
            users: [...state.users, user],
          })),

        updateUser: (id, updates) =>
          set((state) => ({
            users: state.users.map((u) =>
              u.id === id ? { ...u, ...updates } : u
            ),
          })),

        deleteUser: (id) =>
          set((state) => ({
            users: state.users.filter((u) => u.id !== id),
            selectedUser:
              state.selectedUser?.id === id
                ? null
                : state.selectedUser,
          })),

        setSelectedUser: (id) =>
          set((state) => ({
            selectedUser: state.users.find((u) => u.id === id) || null,
          })),

        fetchUsers: async () => {
          set({ isLoading: true, error: null });
          try {
            const response = await fetch('/api/users');
            if (!response.ok) throw new Error('Failed to fetch');
            const users = await response.json();
            set({ users, isLoading: false });
          } catch (error) {
            set({ error: error.message, isLoading: false });
          }
        },
      }),
      {
        name: 'user-storage',
        partialize: (state) => ({ users: state.users }),
      }
    ),
    { name: 'UserStore' }
  )
);

export default useUserStore;
```

---

## Jotai Pattern

### Atomic State

```javascript
// file: state/jotai/userAtoms.js
import { atom } from 'jotai';
import { atomWithStorage } from 'jotai/utils';

// Base atoms
export const userListAtom = atom([]);
export const selectedUserIdAtom = atom(null);
export const loadingAtom = atom(false);
export const errorAtom = atom(null);

// Derived atoms
export const userCountAtom = atom((get) => get(userListAtom).length);

export const selectedUserAtom = atom((get) => {
  const users = get(userListAtom);
  const id = get(selectedUserIdAtom);
  return users.find((u) => u.id === id) || null;
});

export const activeUsersAtom = atom((get) =>
  get(userListAtom).filter((u) => u.isActive)
);

// Computed atoms
export const userNamesAtom = atom((get) =>
  get(userListAtom).map((u) => u.name)
);

// Async atoms
export const fetchUsersAtom = atom(
  null,
  async (get, set) => {
    set(loadingAtom, true);
    set(errorAtom, null);
    
    try {
      const response = await fetch('/api/users');
      if (!response.ok) throw new Error('Failed to fetch');
      const users = await response.json();
      set(userListAtom, users);
    } catch (error) {
      set(errorAtom, error.message);
    } finally {
      set(loadingAtom, false);
    }
  }
);

// Persisted atoms
export const themeAtom = atomWithStorage('theme', 'light');
export const sidebarOpenAtom = atomWithStorage('sidebar', true);
```

### Component Usage

```javascript
// file: state/jotai/UserList.jsx
import React from 'react';
import { useAtom } from 'jotai';
import { userListAtom, selectedUserIdAtom, fetchUsersAtom } from './userAtoms';

const UserList = () => {
  const [users] = useAtom(userListAtom);
  const [selectedId, setSelectedId] = useAtom(selectedUserIdAtom);
  const [, fetchUsers] = useAtom(fetchUsersAtom);

  React.useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const handleSelect = (id) => {
    setSelectedId(id);
  };

  return (
    <ul>
      {users.map((user) => (
        <li
          key={user.id}
          onClick={() => handleSelect(user.id)}
          className={selectedId === user.id ? 'selected' : ''}
        >
          {user.name}
        </li>
      ))}
    </ul>
  );
};

export default UserList;
```

---

## State Machine Pattern

### XState Integration

```javascript
// file: state/machine/authMachine.js
import { createMachine, interpret } from 'xstate';

const authMachine = createMachine({
  id: 'auth',
  initial: 'idle',
  states: {
    idle: {
      on: { LOGIN: 'loading' },
    },
    loading: {
      invoke: {
        src: 'login',
        onDone: { target: 'success', actions: 'setUser' },
        onError: { target: 'failure', actions: 'setError' },
      },
    },
    success: {
      on: { LOGOUT: 'idle' },
    },
    failure: {
      on: { RETRY: 'loading', LOGIN: 'loading' },
    },
  },
  context: {
    user: null,
    error: null,
  },
  actions: {
    setUser: (context, event) => {
      context.user = event.data;
    },
    setError: (context, event) => {
      context.error = event.data;
    },
  },
});

const authService = interpret(authMachine)
  .onTransition((state) => {
    console.log('State:', state.value);
  })
  .start();

export default authMachine;
```

---

## Choosing the Right Solution

| Scenario | Solution |
|----------|----------|
| Small app, simple state | useState + Context |
| Medium app, shared state | Redux Toolkit |
| Large app, complex state | Redux + RTK Query |
| Vue app | Pinia/Vuex |
| MobX preference | MobX |
| Simple API | Zustand |
| Atomic state | Jotai |
| Complex flows | XState |

---

## Key Takeaways

1. **Start simple** with useState and Context
2. **Scale gradually** as needs grow
3. **Redux Toolkit** simplifies Redux significantly
4. **MobX** provides fine-grained reactivity
5. **Zustand** offers simplicity and performance
6. **Jotai** enables atomic state management
7. **XState** handles complex state machines

---

## Common Pitfalls

1. **Over-engineering** small apps with Redux
2. **Not normalizing** nested state
3. **Global state for everything** instead of local state
4. **Mutating state** directly
5. **Missing selectors** causing unnecessary re-renders
6. **Not handling async** state properly

---

## Related Files

- [02_COMPONENT_ARCHITECTURE_PATTERNS](./02_COMPONENT_ARCHITECTURE_PATTERNS.md)
- [05_FRAMEWORK_ROUTING_MASTER](./05_FRAMEWORK_ROUTING_MASTER.md)
- [06_FRAMEWORK_PERFORMANCE_OPTIMIZATION](./06_FRAMEWORK_PERFORMANCE_OPTIMIZATION.md)
- [07_FRAMEWORK_TESTING_STRATEGIES](./07_FRAMEWORK_TESTING_STRATEGIES.md)