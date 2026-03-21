# Typing Context API

## Overview
React Context API is used for sharing state across components without passing props through every level. Properly typing Context is essential for type safety throughout your application. The key challenge is handling the case where context might not have a value (before a provider is mounted), which requires careful type handling. This guide covers creating type-safe contexts, using generic context factories, and best practices for consuming context with hooks.

## Prerequisites
- Understanding of React hooks
- Familiarity with useState and useReducer
- Basic TypeScript knowledge
- Understanding of React Context API concept

## Core Concepts

### Basic Typed Context
The foundation of type-safe context involves properly typing the context value and handling the undefined case:

```typescript
// [File: src/context/BasicContext.tsx]
import React, { createContext, useContext, useState } from 'react';

// ======== Define the Context Value Type ========

interface User {
  id: string;
  name: string;
  email: string;
  role: 'admin' | 'user' | 'guest';
}

// The context value can be User or undefined (when not provided)
interface UserContextValue {
  user: User | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  isAuthenticated: boolean;
  isLoading: boolean;
}

// Create context with undefined as default - forces consumers to check
// This is the safest approach as it ensures Provider is used
const UserContext = createContext<UserContextValue | undefined>(undefined);

// ======== Provider Component ========

interface UserProviderProps {
  children: React.ReactNode;
}

export function UserProvider({ children }: UserProviderProps) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const login = async (email: string, password: string) => {
    setIsLoading(true);
    try {
      // Simulate API call
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
      });
      
      if (!response.ok) throw new Error('Login failed');
      
      const userData = await response.json();
      setUser(userData);
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    setUser(null);
  };

  const value: UserContextValue = {
    user,
    login,
    logout,
    isAuthenticated: !!user,
    isLoading,
  };

  return (
    <UserContext.Provider value={value}>
      {children}
    </UserContext.Provider>
  );
}

// ======== Custom Hook for Consuming Context ========

// This hook handles the undefined case and provides type safety
function useUser(): UserContextValue {
  const context = useContext(UserContext);
  
  // TypeScript knows context could be undefined
  if (context === undefined) {
    throw new Error('useUser must be used within a UserProvider');
  }
  
  return context;
}

// ======== Usage in Components ========

function UserProfile() {
  // TypeScript knows user is User | null
  // TypeScript knows login and logout are properly typed functions
  const { user, login, logout, isAuthenticated, isLoading } = useUser();
  
  if (isLoading) return <div>Loading...</div>;
  
  if (!isAuthenticated) {
    return (
      <button onClick={() => login('test@example.com', 'password')}>
        Login
      </button>
    );
  }
  
  return (
    <div>
      <h1>Welcome, {user?.name}!</h1>
      <p>Role: {user?.role}</p>
      <button onClick={logout}>Logout</button>
    </div>
  );
}

// Wrap components that need access to context
function App() {
  return (
    <UserProvider>
      <UserProfile />
    </UserProvider>
  );
}
```

### Generic Context Factory
For reusable context patterns, create a generic factory function:

```typescript
// [File: src/context/createContext.tsx]
import React, { createContext, useContext, useState, useCallback } from 'react';

// Generic context factory - creates a typed context with provider
function createTypedContext<T>() {
  // Create context with undefined default - forces provider usage
  const context = createContext<T | undefined>(undefined);
  
  // Custom hook with error boundary
  const useContextHook = (): T => {
    const value = useContext(context);
    if (value === undefined) {
      throw new Error('Context must be used within its Provider');
    }
    return value;
  };
  
  return [context, useContextHook, context.Provider] as const;
}

// ======== Example: Auth Context with Factory ========

// Define auth state type
interface AuthState {
  user: {
    id: string;
    name: string;
    email: string;
  } | null;
  isAuthenticated: boolean;
}

// Define actions type
interface AuthActions {
  login: (credentials: { email: string; password: string }) => Promise<void>;
  logout: () => void;
  updateProfile: (data: Partial<AuthState['user']>) => void;
}

// Combine state and actions
type AuthContextValue = AuthState & AuthActions;

// Create the typed context
const [AuthContext, useAuth, AuthProvider] = createTypedContext<AuthContextValue>();

// Create the actual provider component
function AuthProviderComponent({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<AuthState['user']>(null);
  
  const login = useCallback(async (credentials: { email: string; password: string }) => {
    // Login logic here
    setUser({ id: '1', name: 'John', email: credentials.email });
  }, []);
  
  const logout = useCallback(() => {
    setUser(null);
  }, []);
  
  const updateProfile = useCallback((data: Partial<AuthState['user']>) => {
    setUser(prev => prev ? { ...prev, ...data } : null);
  }, []);
  
  const value: AuthContextValue = {
    user,
    isAuthenticated: !!user,
    login,
    logout,
    updateProfile,
  };
  
  return (
    <AuthProvider value={value}>
      {children}
    </AuthProvider>
  );
}

// Now use it:
function Profile() {
  const { user, logout } = useAuth();
  return <div>Hello, {user?.name}</div>;
}
```

### Complex Context with Reducer
For complex state management in context, combine with useReducer:

```typescript
// [File: src/context/CartContext.tsx]
import React, { createContext, useContext, useReducer } from 'react';

// ======== Define Types ========

interface CartItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

interface CartState {
  items: CartItem[];
  totalItems: number;
  totalPrice: number;
  isOpen: boolean;
}

// Action types - discriminated union
type CartAction =
  | { type: 'ADD_ITEM'; payload: CartItem }
  | { type: 'REMOVE_ITEM'; payload: string }
  | { type: 'UPDATE_QUANTITY'; payload: { id: string; quantity: number } }
  | { type: 'CLEAR_CART' }
  | { type: 'TOGGLE_CART' };

// ======== Reducer ========

function cartReducer(state: CartState, action: CartAction): CartState {
  switch (action.type) {
    case 'ADD_ITEM': {
      const existingItem = state.items.find(
        item => item.id === action.payload.id
      );
      
      let newItems: CartItem[];
      
      if (existingItem) {
        // Update quantity of existing item
        newItems = state.items.map(item =>
          item.id === action.payload.id
            ? { ...item, quantity: item.quantity + action.payload.quantity }
            : item
        );
      } else {
        // Add new item
        newItems = [...state.items, action.payload];
      }
      
      return {
        ...state,
        items: newItems,
        totalItems: newItems.reduce((sum, item) => sum + item.quantity, 0),
        totalPrice: newItems.reduce(
          (sum, item) => sum + item.price * item.quantity, 
          0
        ),
      };
    }
    
    case 'REMOVE_ITEM': {
      const newItems = state.items.filter(
        item => item.id !== action.payload
      );
      
      return {
        ...state,
        items: newItems,
        totalItems: newItems.reduce((sum, item) => sum + item.quantity, 0),
        totalPrice: newItems.reduce(
          (sum, item) => sum + item.price * item.quantity, 
          0
        ),
      };
    }
    
    case 'UPDATE_QUANTITY': {
      if (action.payload.quantity <= 0) {
        return cartReducer(state, { type: 'REMOVE_ITEM', payload: action.payload.id });
      }
      
      const newItems = state.items.map(item =>
        item.id === action.payload.id
          ? { ...item, quantity: action.payload.quantity }
          : item
      );
      
      return {
        ...state,
        items: newItems,
        totalItems: newItems.reduce((sum, item) => sum + item.quantity, 0),
        totalPrice: newItems.reduce(
          (sum, item) => sum + item.price * item.quantity, 
          0
        ),
      };
    }
    
    case 'CLEAR_CART':
      return {
        items: [],
        totalItems: 0,
        totalPrice: 0,
        isOpen: state.isOpen,
      };
    
    case 'TOGGLE_CART':
      return {
        ...state,
        isOpen: !state.isOpen,
      };
      
    default:
      return state;
  }
}

// ======== Context ========

interface CartContextValue extends CartState {
  addItem: (item: Omit<CartItem, 'quantity'>, quantity?: number) => void;
  removeItem: (id: string) => void;
  updateQuantity: (id: string, quantity: number) => void;
  clearCart: () => void;
  toggleCart: () => void;
}

const CartContext = createContext<CartContextValue | undefined>(undefined);

// ======== Provider ========

interface CartProviderProps {
  children: React.ReactNode;
}

export function CartProvider({ children }: CartProviderProps) {
  const [state, dispatch] = useReducer(cartReducer, {
    items: [],
    totalItems: 0,
    totalPrice: 0,
    isOpen: false,
  });

  const addItem = (item: Omit<CartItem, 'quantity'>, quantity = 1) => {
    dispatch({ 
      type: 'ADD_ITEM', 
      payload: { ...item, quantity } 
    });
  };

  const removeItem = (id: string) => {
    dispatch({ type: 'REMOVE_ITEM', payload: id });
  };

  const updateQuantity = (id: string, quantity: number) => {
    dispatch({ type: 'UPDATE_QUANTITY', payload: { id, quantity } });
  };

  const clearCart = () => {
    dispatch({ type: 'CLEAR_CART' });
  };

  const toggleCart = () => {
    dispatch({ type: 'TOGGLE_CART' });
  };

  const value: CartContextValue = {
    ...state,
    addItem,
    removeItem,
    updateQuantity,
    clearCart,
    toggleCart,
  };

  return (
    <CartContext.Provider value={value}>
      {children}
    </CartContext.Provider>
  );
}

// ======== Custom Hook ========

export function useCart(): CartContextValue {
  const context = useContext(CartContext);
  
  if (!context) {
    throw new Error('useCart must be used within a CartProvider');
  }
  
  return context;
}

// ======== Usage ========

function CartButton() {
  const { totalItems, toggleCart } = useCart();
  
  return (
    <button onClick={toggleCart}>
      Cart ({totalItems})
    </button>
  );
}

function AddToCartButton({ product }: { product: { id: string; name: string; price: number } }) {
  const { addItem } = useCart();
  
  const handleAdd = () => {
    addItem({
      id: product.id,
      name: product.name,
      price: product.price,
      image: '/placeholder.jpg',
    });
  };
  
  return (
    <button onClick={handleAdd}>Add to Cart</button>
  );
}
```

## Common Mistakes

### Mistake 1: Not Handling Undefined Context
```typescript
// ❌ WRONG - Context could be undefined, causing runtime errors
const UserContext = createContext<User>({} as User);
// Never do this! It hides potential bugs

// ✅ CORRECT - Handle undefined case properly
const UserContext = createContext<User | undefined>(undefined);

// Then check in hook:
function useUser() {
  const context = useContext(UserContext);
  if (!context) throw new Error('Must be used within Provider');
  return context;
}
```

### Mistake 2: Not Memoizing Context Value
```typescript
// ❌ WRONG - New object every render causes unnecessary re-renders
function UserProvider({ children }) {
  const [user, setUser] = useState(null);
  
  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
}

// ✅ CORRECT - Memoize the context value
function UserProvider({ children }) {
  const [user, setUser] = useState(null);
  
  const value = useMemo(() => ({ user, setUser }), [user]);
  
  return (
    <UserContext.Provider value={value}>
      {children}
    </UserContext.Provider>
  );
}
```

### Mistake 3: Creating Context Outside Provider
```typescript
// ❌ WRONG - Using context outside provider silently fails
// In another file:
const ThemeContext = createContext<string>('light');

// ✅ CORRECT - Always create context with undefined and require provider
const ThemeContext = createContext<string | undefined>(undefined);
```

## Real-World Example

Complete theme context with system preference detection:

```typescript
// [File: src/context/ThemeContext.tsx]
import React, { createContext, useContext, useState, useEffect, useMemo } from 'react';

// ======== Types ========

type Theme = 'light' | 'dark' | 'system';
type ResolvedTheme = 'light' | 'dark';

interface ThemeContextValue {
  theme: Theme;
  resolvedTheme: ResolvedTheme;
  setTheme: (theme: Theme) => void;
  isDark: boolean;
}

// ======== Context ========

const ThemeContext = createContext<ThemeContextValue | undefined>(undefined);

// ======== Provider ========

interface ThemeProviderProps {
  children: React.ReactNode;
  defaultTheme?: Theme;
}

export function ThemeProvider({ 
  children, 
  defaultTheme = 'system' 
}: ThemeProviderProps) {
  const [theme, setTheme] = useState<Theme>(defaultTheme);
  
  // Resolve system theme preference
  const [systemTheme, setSystemTheme] = useState<ResolvedTheme>('light');
  
  useEffect(() => {
    // Get initial system preference
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    setSystemTheme(mediaQuery.matches ? 'dark' : 'light');
    
    // Listen for changes
    const handler = (e: MediaQueryListEvent) => {
      setSystemTheme(e.matches ? 'dark' : 'light');
    };
    
    mediaQuery.addEventListener('change', handler);
    return () => mediaQuery.removeEventListener('change', handler);
  }, []);
  
  // Calculate resolved theme
  const resolvedTheme = useMemo<ResolvedTheme>(() => {
    if (theme === 'system') {
      return systemTheme;
    }
    return theme;
  }, [theme, systemTheme]);
  
  // Update document class when theme changes
  useEffect(() => {
    const root = document.documentElement;
    
    // Remove both classes first
    root.classList.remove('light', 'dark');
    
    // Add resolved theme class
    root.classList.add(resolvedTheme);
    
    // Set data attribute for CSS selectors
    root.setAttribute('data-theme', resolvedTheme);
  }, [resolvedTheme]);
  
  // Memoize context value to prevent unnecessary re-renders
  const value = useMemo<ThemeContextValue>(() => ({
    theme,
    resolvedTheme,
    setTheme,
    isDark: resolvedTheme === 'dark',
  }), [theme, resolvedTheme]);

  return (
    <ThemeContext.Provider value={value}>
      {children}
    </ThemeContext.Provider>
  );
}

// ======== Custom Hook ========

export function useTheme(): ThemeContextValue {
  const context = useContext(ThemeContext);
  
  if (!context) {
    throw new Error('useTheme must be used within a ThemeProvider');
  }
  
  return context;
}

// ======== Usage Examples ========

// Theme toggle component
function ThemeToggle() {
  const { theme, setTheme, isDark } = useTheme();
  
  const cycleTheme = () => {
    if (theme === 'light') setTheme('dark');
    else if (theme === 'dark') setTheme('system');
    else setTheme('light');
  };
  
  return (
    <button onClick={cycleTheme}>
      Current: {theme} ({isDark ? 'dark' : 'light'})
    </button>
  );
}

// Layout that applies theme class
function Layout({ children }: { children: React.ReactNode }) {
  const { isDark } = useTheme();
  
  return (
    <div className={isDark ? 'dark-theme' : 'light-theme'}>
      {children}
    </div>
  );
}

// ======== App Setup ========

function App() {
  return (
    <ThemeProvider defaultTheme="system">
      <Layout>
        <ThemeToggle />
        <main>Content goes here</main>
      </Layout>
    </ThemeProvider>
  );
}

export default App;
```

## Key Takeaways
- Always create context with `undefined` as default to force provider usage
- Create a custom hook that throws if context is undefined
- Always memoize context values with `useMemo` to prevent unnecessary re-renders
- Use discriminated unions for complex context state
- Combine `useReducer` with context for complex state management
- Use generic context factories for reusable context patterns
- Export both the Provider and custom hook for clean API

## What's Next
Continue to [Discriminated Unions for UI](03-advanced-ts-patterns/01-discriminated-unions-for-ui.md) to learn how to use discriminated unions for complex UI state management patterns.