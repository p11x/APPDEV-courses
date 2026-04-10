# Component Architecture Patterns

Comprehensive guide to component design patterns in modern JavaScript frameworks. Learn patterns for building scalable, maintainable, and reusable components.

## Table of Contents

1. [Introduction to Component Architecture](#introduction-to-component-architecture)
2. [Atomic Design Principles](#atomic-design-principles)
3. [Component Composition Patterns](#component-composition-patterns)
4. [State Management Patterns](#state-management-patterns)
5. [Props Drilling Solutions](#props-drilling-solutions)
6. [Lifecycle Patterns](#lifecycle-patterns)
7. [Higher-Order Components](#higher-order-components)
8. [Render Props Pattern](#render-props-pattern)
9. [Compound Components](#compound-components)
10. [Container/Presenter Pattern](#containerpresenter-pattern)
11. [Custom Hooks Pattern](#custom-hooks-pattern)
12. [Slots and Fallback Patterns](#slots-and-fallback-patterns)
13. [Key Takeaways](#key-takeaways)
14. [Common Pitfalls](#common-pitfalls)

---

## Introduction to Component Architecture

Component architecture is the foundation of modern UI frameworks. Good architecture enables reusability, maintainability, and testability. This guide covers proven patterns used in production applications.

### Core Principles

- **Single Responsibility**: Each component does one thing well
- **Encapsulation**: Internal details are hidden
- **Composition**: Complex UIs from simple pieces
- **Loose Coupling**: Components work independently
- **High Cohesion**: Related functionality together

---

## Atomic Design Principles

Atomic Design breaks UI into five hierarchical levels:

### Level Examples

```jsx
// file: atoms/Button.jsx
// Atoms: Basic building blocks
import React from 'react';
import './Button.css';

export const Button = ({ 
  children, 
  variant = 'primary', 
  size = 'medium',
  disabled = false,
  onClick,
}) => {
  return (
    <button
      className={`btn btn--${variant} btn--${size}`}
      disabled={disabled}
      onClick={onClick}
    >
      {children}
    </button>
  );
};

export default Button;
```

```jsx
// file: molecules/SearchInput.jsx
// Molecules: Simple groups of atoms
import React, { useState } from 'react';
import Button from '../atoms/Button';

export const SearchInput = ({ onSearch }) => {
  const [query, setQuery] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    onSearch(query);
  };

  return (
    <form onSubmit={handleSubmit} className="search-input">
      <input
        type="text"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        placeholder="Search..."
        className="search-input__field"
      />
      <Button type="submit" variant="primary">
        Search
      </Button>
    </form>
  );
};

export default SearchInput;
```

```jsx
// file: organisms/Header.jsx
// Organisms: Complex UI sections
import React from 'react';
import SearchInput from '../molecules/SearchInput';
import Navigation from '../molecules/Navigation';
import UserMenu from '../molecules/UserMenu';

export const Header = ({ 
  onSearch, 
  navigation, 
  user,
  onNavigate,
  onLogout,
}) => {
  return (
    <header className="header">
      <div className="header__logo">
        <img src="/logo.svg" alt="Logo" />
      </div>
      <SearchInput onSearch={onSearch} />
      <Navigation items={navigation} onNavigate={onNavigate} />
      <UserMenu user={user} onLogout={onLogout} />
    </header>
  );
};

export default Header;
```

```jsx
// file: templates/PageTemplate.jsx
// Templates: Page layouts
import React from 'react';
import Header from '../organisms/Header';
import Sidebar from '../organisms/Sidebar';
import Footer from '../organisms/Footer';

export const PageTemplate = ({
  children,
  headerProps,
  sidebarProps,
  footerProps,
}) => {
  return (
    <div className="page-template">
      <Header {...headerProps} />
      <div className="page-template__body">
        <Sidebar {...sidebarProps} />
        <main className="page-template__content">
          {children}
        </main>
      </div>
      <Footer {...footerProps} />
    </div>
  );
};

export default PageTemplate;
```

```jsx
// file: pages/UserDashboard.jsx
// Pages: Specific page instances
import React from 'react';
import PageTemplate from '../templates/PageTemplate';
import StatsGrid from '../organisms/StatsGrid';
import RecentActivity from '../organisms/RecentActivity';

export const UserDashboard = ({ user, stats, activity }) => {
  const headerProps = {
    navigation: [
      { label: 'Dashboard', path: '/dashboard' },
      { label: 'Profile', path: '/profile' },
    ],
    user: user,
  };

  const sidebarProps = {
    items: [
      { label: 'Overview', path: '/dashboard' },
      { label: 'Analytics', path: '/dashboard/analytics' },
      { label: 'Settings', path: '/dashboard/settings' },
    ],
  };

  return (
    <PageTemplate
      headerProps={headerProps}
      sidebarProps={sidebarProps}
    >
      <StatsGrid stats={stats} />
      <RecentActivity activity={activity} />
    </PageTemplate>
  );
};

export default UserDashboard;
```

---

## Component Composition Patterns

### The Composition API

```jsx
// file: components/CompositionExample.jsx
import React from 'react';

const Card = ({ children, title, actions }) => {
  return (
    <div className="card">
      {title && (
        <div className="card__header">
          <h3>{title}</h3>
        </div>
      )}
      <div className="card__body">
        {children}
      </div>
      {actions && (
        <div className="card__actions">
          {actions}
        </div>
      )}
    </div>
  );
};

const FlexLayout = ({ 
  children, 
  direction = 'row', 
  gap = '1rem',
  align = 'stretch',
  wrap = false,
}) => {
  const style = {
    display: 'flex',
    flexDirection: direction,
    gap,
    alignItems: align,
    flexWrap: wrap ? 'wrap' : 'nowrap',
  };
  
  return (
    <div style={style}>
      {children}
    </div>
  );
};

const Component = () => {
  return (
    <FlexLayout direction="column" gap="2rem">
      <Card
        title="Welcome"
        actions={<button>Learn More</button>}
      >
        <p>Welcome to our platform!</p>
      </Card>
      
      <FlexLayout wrap gap="1rem">
        <Card title="Feature 1">
          <p>Description 1</p>
        </Card>
        <Card title="Feature 2">
          <p>Description 2</p>
        </Card>
        <Card title="Feature 3">
          <p>Description 3</p>
        </Card>
      </FlexLayout>
    </FlexLayout>
  );
};

export default Component;
```

---

## Props Drilling Solutions

### Problem: Prop Tunneling

```jsx
// file: components/PropDrillingProblem.jsx
import React, { useState } from 'react';

// This creates deep prop drilling
const GrandChild = ({ user, theme, language, currency, onLogout }) => {
  return (
    <div className={`theme-${theme}`}>
      <p>User: {user.name}</p>
      <p>Language: {language}</p>
      <p>Currency: {currency}</p>
      <button onClick={onLogout}>Logout</button>
    </div>
  );
};

const Child = ({ user, theme, language, currency, onLogout }) => {
  return (
    <div>
      <h3>Child Component</h3>
      <GrandChild
        user={user}
        theme={theme}
        language={language}
        currency={currency}
        onLogout={onLogout}
      />
    </div>
  );
};

const Parent = ({ user, theme, language, currency, onLogout }) => {
  return (
    <div>
      <h2>Parent Component</h2>
      <Child
        user={user}
        theme={theme}
        language={language}
        currency={currency}
        onLogout={onLogout}
      />
    </div>
  );
};

// Anti-pattern: Too many props passed down
const App = () => {
  const [user] = useState({ name: 'John' });
  const [theme, setTheme] = useState('dark');
  const [language] = useState('en');
  const [currency] = useState('USD');
  const [onLogout] = useState(() => () => console.log('Logged out'));

  return (
    <Parent
      user={user}
      theme={theme}
      language={language}
      currency={currency}
      onLogout={onLogout}
    />
  );
};

export default App;
```

### Solution: Context API

```jsx
// file: context/AppContext.jsx
import React, { createContext, useContext, useState, useCallback } from 'react';

const AppContext = createContext(null);

export const AppProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [theme, setTheme] = useState('dark');
  const [language, setLanguage] = useState('en');
  const [currency, setCurrency] = useState('USD');

  const login = useCallback((userData) => {
    setUser(userData);
  }, []);

  const logout = useCallback(() => {
    setUser(null);
  }, []);

  const value = {
    user,
    theme,
    language,
    currency,
    login,
    logout,
    setTheme,
    setLanguage,
    setCurrency,
  };

  return (
    <AppContext.Provider value={value}>
      {children}
    </AppContext.Provider>
  );
};

export const useApp = () => {
  const context = useContext(AppContext);
  if (!context) {
    throw new Error('useApp must be used within AppProvider');
  }
  return context;
};
```

```jsx
// file: components/UsingContext.jsx
import React from 'react';
import { useApp } from '../context/AppContext';

const GrandChild = () => {
  const { user, theme, language, currency, logout } = useApp();

  return (
    <div className={`theme-${theme}`}>
      <p>User: {user?.name}</p>
      <p>Language: {language}</p>
      <p>Currency: {currency}</p>
      <button onClick={logout}>Logout</button>
    </div>
  );
};

const Child = () => (
  <div>
    <h3>Child Component</h3>
    <GrandChild />
  </div>
);

const Parent = () => (
  <div>
    <h2>Parent Component</h2>
    <Child />
  </div>
);

const App = () => (
  <Parent />
);

export default App;
```

---

## Lifecycle Patterns

### Class Component Lifecycle

```jsx
// file: components/ClassLifecycle.jsx
import React, { Component } from 'react';

class DataFetcher extends Component {
  constructor(props) {
    super(props);
    this.state = {
      data: null,
      loading: true,
      error: null,
    };
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (nextProps.url !== prevState.prevUrl) {
      return { prevUrl: nextProps.url, data: null, loading: true };
    }
    return null;
  }

  componentDidMount() {
    this.fetchData();
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.url !== this.props.url) {
      this.fetchData();
    }
  }

  componentWillUnmount() {
    if (this.abortController) {
      this.abortController.abort();
    }
  }

  fetchData = async () => {
    try {
      this.abortController = new AbortController();
      const response = await fetch(this.props.url, {
        signal: this.abortController.signal,
      });
      if (!response.ok) throw new Error('Failed to fetch');
      const data = await response.json();
      this.setState({ data, loading: false });
    } catch (error) {
      if (error.name !== 'AbortError') {
        this.setState({ error: error.message, loading: false });
      }
    }
  };

  getSnapshotBeforeUpdate(prevProps, prevState) {
    return prevState.data;
  }

  componentDidUpdate(prevProps, prevState, snapshot) {
    if (prevState.data !== this.state.data) {
      console.log('Data updated:', snapshot);
    }
  }

  render() {
    const { loading, data, error } = this.state;
    
    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;
    
    return this.props.render(data);
  }
}

export default DataFetcher;
```

### Functional Lifecycle with Hooks

```jsx
// file: components/HookLifecycle.jsx
import React, { useState, useEffect, useRef, useCallback } from 'react';

const useDataFetcher = (url) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const abortControllerRef = useRef(null);

  const fetchData = useCallback(async () => {
    abortControllerRef.current?.abort();
    abortControllerRef.current = new AbortController();

    try {
      setLoading(true);
      const response = await fetch(url, {
        signal: abortControllerRef.current.signal,
      });
      if (!response.ok) throw new Error('Failed to fetch');
      const result = await response.json();
      setData(result);
      setError(null);
    } catch (error) {
      if (error.name !== 'AbortError') {
        setError(error.message);
      }
    } finally {
      setLoading(false);
    }
  }, [url]);

  useEffect(() => {
    fetchData();
    return () => abortControllerRef.current?.abort();
  }, [fetchData]);

  return { data, loading, error, refetch: fetchData };
};

const DataDisplay = ({ url, render }) => {
  const { data, loading, error, refetch } = useDataFetcher(url);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return render(data, refetch);
};

export default DataDisplay;
```

---

## Higher-Order Components

### HOC Pattern

```jsx
// file: components/hoc/withLoading.jsx
import React from 'react';

const withLoading = (Component) => {
  return ({ isLoading, ...props }) => {
    if (isLoading) {
      return <div>Loading...</div>;
    }
    return <Component {...props} />;
  };
};

const UserList = ({ users }) => (
  <ul>
    {users.map((user) => (
      <li key={user.id}>{user.name}</li>
    ))}
  </ul>
);

const UserListWithLoading = withLoading(UserList);

const App = () => (
  <UserListWithLoading users={[]} isLoading={true} />
);

export default App;
```

```jsx
// file: components/hoc/withErrorBoundary.jsx
import React, { Component } from 'react';

const withErrorBoundary = (Component) => {
  return class ErrorBoundary extends Component {
    constructor(props) {
      super(props);
      this.state = { hasError: false, error: null };
    }

    static getDerivedStateFromError(error) {
      return { hasError: true, error };
    }

    componentDidCatch(error, errorInfo) {
      console.error('Error caught:', error, errorInfo);
    }

    render() {
      if (this.state.hasError) {
        return (
          <div>
            <h2>Something went wrong</h2>
            <p>{this.state.error?.message}</p>
            <button onClick={() => window.location.reload()}>
              Reload Page
            </button>
          </div>
        );
      }
      return <Component {...this.props} />;
    }
  };
};

const RiskyComponent = ({ data }) => {
  if (data缺失) {
    throw new Error('Missing data');
  }
  return <div>{data}</div>;
};

const SafeRiskyComponent = withErrorBoundary(RiskyComponent);

export default SafeRiskyComponent;
```

---

## Render Props Pattern

```jsx
// file: components/RenderPropsPattern.jsx
import React, { useState, useEffect } from 'react';

const MouseTracker = ({ render }) => {
  const [position, setPosition] = useState({ x: 0, y: 0 });

  useEffect(() => {
    const handleMouseMove = (event) => {
      setPosition({
        x: event.clientX,
        y: event.clientY,
      });
    };

    window.addEventListener('mousemove', handleMouseMove);
    return () => window.removeEventListener('mousemove', handleMouseMove);
  }, []);

  return render(position);
};

const App = () => (
  <MouseTracker
    render={({ x, y }) => (
      <div>
        Mouse position: {x}, {y}
      </div>
    )}
  />
);

export default App;
```

---

## Compound Components

```jsx
// file: components/CompoundComponents.jsx
import React, { createContext, useContext, useState } from 'react';

const AccordionContext = createContext(null);

const Accordion = ({ children, defaultOpen = null }) => {
  const [openItem, setOpenItem] = useState(defaultOpen);

  const value = {
    openItem,
    toggleItem: (item) => {
      setOpenItem((prev) => (prev === item ? null : item));
    },
  };

  return (
    <AccordionContext.Provider value={value}>
      <div className="accordion">{children}</div>
    </AccordionContext.Provider>
  );
};

const AccordionItem = ({ children, id }) => {
  const { openItem, toggleItem } = useContext(AccordionContext);
  const isOpen = openItem === id;

  return (
    <div className="accordion-item">
      <button
        className="accordion-header"
        onClick={() => toggleItem(id)}
        aria-expanded={isOpen}
      >
        {children.header}
      </button>
      {isOpen && <div className="accordion-content">{children.content}</div>}
    </div>
  );
};

Accordion.Item = AccordionItem;

const App = () => (
  <Accordion defaultOpen="item1">
    <Accordion.Item id="item1">
      {{
        header: 'Section 1',
        content: <p>Content for section 1</p>,
      }}
    </Accordion.Item>
    <Accordion.Item id="item2">
      {{
        header: 'Section 2',
        content: <p>Content for section 2</p>,
      }}
    </Accordion.Item>
  </Accordion>
);

export default App;
```

---

## Container/Presenter Pattern

```jsx
// file: components/ContainerPresenter.jsx
import React, { useState, useEffect } from 'react';

// Presenter: Pure UI logic
const UserListPresenter = ({ users, isLoading, error, onRefresh }) => {
  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <button onClick={onRefresh}>Refresh</button>
      <ul>
        {users.map((user) => (
          <li key={user.id}>
            {user.name} - {user.email}
          </li>
        ))}
      </ul>
    </div>
  );
};

// Container: Data and business logic
const UserListContainer = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const response = await fetch('/api/users');
      if (!response.ok) throw new Error('Failed to fetch');
      const data = await response.json();
      setUsers(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <UserListPresenter
      users={users}
      isLoading={loading}
      error={error}
      onRefresh={fetchUsers}
    />
  );
};

export default UserListContainer;
```

---

## Custom Hooks Pattern

```jsx
// file: hooks/useLocalStorage.js
import { useState, useEffect, useCallback } from 'react';

export const useLocalStorage = (key, initialValue) => {
  const [storedValue, setStoredValue] = useState(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      console.error('Error reading localStorage:', error);
      return initialValue;
    }
  });

  const setValue = useCallback(
    (value) => {
      try {
        const valueToStore = value instanceof Function
          ? value(storedValue)
          : value;
        setStoredValue(valueToStore);
        window.localStorage.setItem(key, JSON.stringify(valueToStore));
      } catch (error) {
        console.error('Error setting localStorage:', error);
      }
    },
    [key, storedValue]
  );

  useEffect(() => {
    const handleStorageChange = (e) => {
      if (e.key === key && e.newValue) {
        setStoredValue(JSON.parse(e.newValue));
      }
    };
    window.addEventListener('storage', handleStorageChange);
    return () => window.removeEventListener('storage', handleStorageChange);
  }, [key]);

  return [storedValue, setValue];
};

export default useLocalStorage;
```

```jsx
// file: hooks/useDebounce.js
import { useState, useEffect } from 'react';

export const useDebounce = (value, delay = 500) => {
  const [debouncedValue, setDebouncedValue] = useState(value);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => clearTimeout(timer);
  }, [value, delay]);

  return debouncedValue;
};

const SearchInput = () => {
  const [query, setQuery] = useState('');
  const debouncedQuery = useDebounce(query, 300);

  useEffect(() => {
    if (debouncedQuery) {
      console.log('Searching for:', debouncedQuery);
    }
  }, [debouncedQuery]);

  return (
    <input
      value={query}
      onChange={(e) => setQuery(e.target.value)}
      placeholder="Search..."
    />
  );
};

export default SearchInput;
```

---

## Slots and Fallback Patterns

```jsx
// file: components/SlotsAndFallbacks.jsx
import React from 'react';

const Card = ({ children, fallback = null }) => {
  const slots = React.Children.toArray(children).reduce(
    (acc, child) => {
      if (React.isValidElement(child)) {
        const slotName = child.props.slot || 'default';
        acc[slotName] = child;
      }
      return acc;
    },
    { default: null }
  );

  return (
    <div className="card">
      {slots.header && (
        <div className="card__header">{slots.header}</div>
      )}
      <div className="card__body">
        {slots.default || fallback}
      </div>
      {slots.footer && (
        <div className="card__footer">{slots.footer}</div>
      )}
    </div>
  );
};

Card.Header = ({ children }) => <div slot="header">{children}</div>;
Card.Footer = ({ children }) => <div slot="footer">{children}</div>;

const App = () => (
  <Card fallback={<p>No content available</p>}>
    <Card.Header>Card Title</Card.Header>
    <p>This is the main content</p>
    <Card.Footer>Footer content</Card.Footer>
  </Card>
);

export default App;
```

---

## Key Takeaways

1. **Atomic Design** provides hierarchical structure for UI components
2. **Composition over inheritance** enables flexible component building
3. **Context API** solves props drilling in moderate applications
4. **HOCs** enable cross-cutting concerns without inheritance
5. **Render props** provide flexible data access patterns
6. **Compound components** create intuitive APIs
7. **Container/Presenter** separates concerns effectively
8. **Custom hooks** abstract reusable logic elegantly

---

## Common Pitfalls

1. **Over-componentization**: Creating too many tiny components
2. **Ignoring Performance**: Not using React.memo for expensive components
3. **Bad Context Usage**: Context for frequently changing values
4. **Not Testing Components**: Missing unit tests for components
5. **Improper Prop Types**: Not defining prop types leads to runtime errors
6. **Tunneling Props**: Passing unnecessary props down the tree

---

## Related Files

- [01_FRAMEWORK_COMPARISON_MASTER](./01_FRAMEWORK_COMPARISON_MASTER.md)
- [03_VIRTUAL_DOM_EXPLANATION](./03_VIRTUAL_DOM_EXPLANATION.md)
- [04_STATE_MANAGEMENT_PATTERNS](./04_STATE_MANAGEMENT_PATTERNS.md)
- [06_FRAMEWORK_PERFORMANCE_OPTIMIZATION](./06_FRAMEWORK_PERFORMANCE_OPTIMIZATION.md)