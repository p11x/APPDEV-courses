# 🚀 React Components Complete Guide

## Professional Component Architecture in React

---

## Table of Contents

1. [Component Fundamentals](#component-fundamentals)
2. [Component Lifecycle](#component-lifecycle)
3. [Props Deep Dive](#props-deep-dive)
4. [State Management Patterns](#state-management-patterns)
5. [Component Composition](#component-composition)
6. [Higher-Order Components](#higher-order-components)
7. [Render Props](#render-props)
8. [Compound Components](#compound-components)
9. [Controlled vs Uncontrolled](#controlled-vs-uncontrolled)
10. [Component Patterns](#component-patterns)
11. [Real-World Examples](#real-world-examples)
12. [Practice Exercises](#practice-exercises)

---

## Component Fundamentals

### What is a Component?

A component is a reusable, isolated piece of UI that encapsulates its own structure, style, and behavior. Components are the building blocks of any React application.

```
┌─────────────────────────────────────────────────────────────┐
│                  COMPONENT ANATOMY                        │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────────────────────────────────────────────┐     │
│  │                  COMPONENT                        │     │
│  │  ┌─────────────────────────────────────────┐   │     │
│  │  │         PROPS (Input)                   │   │     │
│  │  │    →  name, data, handlers, etc.       │   │     │
│  │  └─────────────────────────────────────────┘   │     │
│  │                    │                         │     │
│  │                    ▼                         │     │
│  │  ┌─────────────────────────────────────────┐   │     │
│  │  │         RENDER LOGIC                   │   │     │
│  │  │    →  JSX, conditions, loops         │   │     │
│  │  └─────────────────────────────────────────┘   │     │
│  │                    │                         │     │
│  │                    ▼                         │     │
│  │  ┌─────────────────────────────────────────┐   │     │
│  │  │         OUTPUT (JSX)                  │   │     │
│  │  │    →  DOM elements, styles           │   │     │
│  │  └─────────────────────────────────────────┘   │     │
│  │                    │                         │     │
│  │                    ▼                         │     │
│  │  ┌─────────────────────────────────────────┐   │     │
│  │  │         STATE (Internal)                │   │     │
│  │  │    →  useState, useReducer           │   │     │
│  │  └──────────���──────────────────────────────┘   │     │
│  └─────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
```

### Functional vs Class Components

**Functional Component:**

```jsx
// Modern functional component with hooks
function UserCard({ user, onEdit }) {
  const [isHovered, setIsHovered] = React.useState(false);
  
  return (
    <div 
      className="user-card"
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      <img src={user.avatar} alt={user.name} />
      <h3>{user.name}</h3>
      <p>{user.email}</p>
      {isHovered && (
        <button onClick={() => onEdit(user)}>Edit</button>
      )}
    </div>
  );
}
```

**Class Component:**

```jsx
// Traditional class component
class UserCard extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isHovered: false
    };
  }
  
  handleMouseEnter = () => {
    this.setState({ isHovered: true });
  };
  
  handleMouseLeave = () => {
    this.setState({ isHovered: false });
  };
  
  render() {
    const { user, onEdit } = this.props;
    const { isHovered } = this.state;
    
    return (
      <div 
        className="user-card"
        onMouseEnter={this.handleMouseEnter}
        onMouseLeave={this.handleMouseLeave}
      >
        <img src={user.avatar} alt={user.name} />
        <h3>{user.name}</h3>
        <p>{user.email}</p>
        {isHovered && (
          <button onClick={() => onEdit(user)}>Edit</button>
        )}
      </div>
    );
  }
}
```

### Component Types

```jsx
// Presentational (UI) Component
function Button({ children, variant = 'primary', ...props }) {
  return (
    <button className={`btn btn-${variant}`} {...props}>
      {children}
    </button>
  );
}

// Container (Logic) Component
function UserListContainer() {
  const [users, setUsers] = useState([]);
  
  useEffect(() => {
    fetchUsers().then(setUsers);
  }, []);
  
  return <UserList users={users} />;
}

// Page Component
function UsersPage() {
  return (
    <Layout>
      <UserListContainer />
    </Layout>
  );
}
```

---

## Component Lifecycle

### Understanding Component Lifecycle

```
┌─────────────────────────────────────────────────────────────┐
│              COMPONENT LIFECYCLE                            │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  Mounting          Updating          Unmounting              │
│  ────────         ────────         ──────────               │
│     │                │                  │                    │
│     ▼                ▼                  ▼                    │
│  ┌──────┐      ┌──────────┐       ┌────────────┐            │
│  │Constr│      │State/Props│       │component   │            │
│  │uctor│─────▶│  Change  │──────▶│WillUnmount │            │
│  └──────┘      └──────────┘       └────────────┘            │
│     │                │                  │                    │
│     ▼                ▼                  ▼                    │
│  ┌──────┐      ┌──────────┐                               │
│  │Mount │      │ should   │                               │
│  │phase │      │Component │                               │
│  └──────┘      │Update   │──────▶ Render                  │
│     │          └──────────┘                               │
│     ▼                                                       │
│  ┌──────┐      ┌──────────┐       ┌────────────┐            │
│  │component│  │getSnapshot│      │ Cleanup    │            │
│  │DidMount│◀──│BeforeUp   │◀────▶│Effects     │            │
│  └──────┘      └──────────┘       └────────────┘            │
│     │                │                  │                    │
│     ▼                ▼                  ▼                    │
│  ┌──────┐      ┌──────────┐       ┌────────────┐            │
│  │Render │      │ component │       │ component  │            │
│  └──────┘      │DidUpdate  │       │WillUnmount │            │
│                └──────────┘       └────────────┘            │
└─────────────────────────────────────────────────────────────┘
```

### Lifecycle Methods in Class Components

```jsx
class LifecycleDemo extends React.Component {
  constructor(props) {
    super(props);
    console.log('1. Constructor called');
    this.state = {
      data: null,
      counter: 0
    };
  }
  
  // Called before rendering, when receiving new props
  static getDerivedStateFromProps(props, state) {
    console.log('2. getDerivedStateFromProps');
    return null;
  }
  
  // Called after component is mounted to DOM
  componentDidMount() {
    console.log('3. componentDidMount');
    this.fetchData();
  }
  
  // Called before re-rendering, return false to prevent update
  shouldComponentUpdate(nextProps, nextState) {
    console.log('4. shouldComponentUpdate');
    return true;
  }
  
  // Called before DOM is updated
  getSnapshotBeforeUpdate(prevProps, prevState) {
    console.log('5. getSnapshotBeforeUpdate');
    return { scrollPosition: window.scrollY };
  }
  
  // Called after DOM is updated
  componentDidUpdate(prevProps, prevState, snapshot) {
    console.log('6. componentDidUpdate');
    if (snapshot) {
      console.log('Scroll position:', snapshot.scrollPosition);
    }
  }
  
  // Called before component is removed from DOM
  componentWillUnmount() {
    console.log('7. componentWillUnmount - Cleanup here');
    this.cancelFetch();
  }
  
  render() {
    console.log('Render method executed');
    return <div>Data: {this.state.data}</div>;
  }
}
```

### Functional Component Lifecycle with Hooks

```jsx
import { useState, useEffect, useRef } from 'react';

function LifecycleWithHooks() {
  const [data, setData] = useState(null);
  const [counter, setCounter] = useState(0);
  const mountedRef = useRef(false);
  
  // componentDidMount - runs once on mount
  useEffect(() => {
    console.log('componentDidMount');
    fetchData().then(result => setData(result));
  }, []);
  
  // componentDidUpdate - runs when data or counter changes
  useEffect(() => {
    console.log('componentDidUpdate for data');
  }, [data]);
  
  useEffect(() => {
    console.log('componentDidUpdate for counter');
  }, [counter]);
  
  // componentWillUnmount - cleanup function
  useEffect(() => {
    return () => {
      console.log('componentWillUnmount - cleanup');
    };
  }, []);
  
  // Conditional effect - runs when mountedRef changes
  useEffect(() => {
    if (!mountedRef.current) {
      mountedRef.current = true;
      return;
    }
    console.log('componentDidUpdate');
  });
  
  return (
    <div>
      <p>Data: {data}</p>
      <p>Counter: {counter}</p>
      <button onClick={() => setCounter(counter + 1)}>Increment</button>
    </div>
  );
}
```

---

## Props Deep Dive

### Understanding Props

Props (short for properties) are how components communicate with each other. They pass data from parent to child components.

```jsx
// Parent Component
function Parent() {
  const user = {
    id: 1,
    name: 'John Doe',
    email: 'john@example.com',
    role: 'admin'
  };
  
  return <UserCard user={user} onEdit={() => console.log('Edit clicked')} />;
}

// Child Component receiving props
function UserCard({ user, onEdit }) {
  return (
    <div className="user-card">
      <h3>{user.name}</h3>
      <p>{user.email}</p>
      <span className="role">{user.role}</span>
      <button onClick={onEdit}>Edit</button>
    </div>
  );
}
```

### Props Types and Validation

```jsx
import PropTypes from 'prop-types';

function ProductCard({ 
  product, 
  onAddToCart, 
  isFeatured = false,
  quantity = 1,
  tags = [],
  onSale 
}) {
  return (
    <div className={`product-card ${isFeatured ? 'featured' : ''}`}>
      <h3>{product.name}</h3>
      <p>${product.price}</p>
      <p>Quantity: {quantity}</p>
      <div className="tags">
        {tags.map(tag => <span key={tag}>{tag}</span>)}
      </div>
      {product.onSale && onSale && (
        <span className="sale-badge">On Sale!</span>
      )}
      <button onClick={() => onAddToCart(product, quantity)}>
        Add to Cart
      </button>
    </div>
  );
}

// PropTypes validation
ProductCard.propTypes = {
  product: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
    onSale: PropTypes.bool
  }).isRequired,
  onAddToCart: PropTypes.func.isRequired,
  isFeatured: PropTypes.bool,
  quantity: PropTypes.number,
  tags: PropTypes.arrayOf(PropTypes.string),
  onSale: PropTypes.bool
};

ProductCard.defaultProps = {
  isFeatured: false,
  quantity: 1,
  tags: [],
  onSale: false
};
```

### Children Props

```jsx
function Card({ title, children, footer }) {
  return (
    <div className="card">
      {title && <div className="card-header">{title}</div>}
      <div className="card-body">
        {children}
      </div>
      {footer && <div className="card-footer">{footer}</div>}
    </div>
  );
}

// Usage
function App() {
  return (
    <Card 
      title="Welcome" 
      footer={<button>Learn More</button>}
    >
      <p>This is the card content.</p>
      <p>You can put anything here!</p>
    </Card>
  );
}
```

### Render Props

```jsx
function MouseTracker({ render }) {
  const [position, setPosition] = useState({ x: 0, y: 0 });
  
  useEffect(() => {
    const handleMouseMove = (event) => {
      setPosition({
        x: event.clientX,
        y: event.clientY
      });
    };
    
    window.addEventListener('mousemove', handleMouseMove);
    return () => {
      window.removeEventListener('mousemove', handleMouseMove);
    };
  }, []);
  
  return render(position);
}

// Usage
function App() {
  return (
    <MouseTracker render={({ x, y }) => (
      <p>Mouse position: {x}, {y}</p>
    )} />
  );
}
```

### Function as Child

```jsx
function List({ items, renderItem }) {
  return (
    <ul>
      {items.map((item, index) => (
        <li key={item.id || index}>
          {renderItem(item)}
        </li>
      ))}
    </ul>
  );
}

// Usage
function App() {
  const users = [
    { id: 1, name: 'John', email: 'john@example.com' },
    { id: 2, name: 'Jane', email: 'jane@example.com' }
  ];
  
  return (
    <List 
      items={users}
      renderItem={(user) => (
        <>
          <strong>{user.name}</strong>
          <span> - {user.email}</span>
        </>
      )}
    />
  );
}
```

---

## State Management Patterns

### Lifting State Up

```jsx
function ParentComponent() {
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredItems, setFilteredItems] = useState(items);
  
  useEffect(() => {
    setFilteredItems(
      items.filter(item => 
        item.name.toLowerCase().includes(searchTerm.toLowerCase())
      )
    );
  }, [searchTerm]);
  
  return (
    <div>
      <SearchInput 
        value={searchTerm} 
        onChange={setSearchTerm} 
      />
      <ItemList items={filteredItems} />
    </div>
  );
}

function SearchInput({ value, onChange }) {
  return (
    <input
      type="text"
      value={value}
      onChange={(e) => onChange(e.target.value)}
      placeholder="Search..."
    />
  );
}

function ItemList({ items }) {
  return (
    <ul>
      {items.map(item => (
        <li key={item.id}>{item.name}</li>
      ))}
    </ul>
  );
}
```

### Context for Shared State

```jsx
const ThemeContext = React.createContext();

function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  const [fontSize, setFontSize] = useState(16);
  
  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };
  
  const increaseFontSize = () => setFontSize(prev => prev + 2);
  const decreaseFontSize = () => setFontSize(prev => Math.max(8, prev - 2));
  
  const value = {
    theme,
    fontSize,
    toggleTheme,
    increaseFontSize,
    decreaseFontSize
  };
  
  return (
    <ThemeContext.Provider value={value}>
      {children}
    </ThemeContext.Provider>
  );
}

// Using context in nested component
function SettingsPanel() {
  const { 
    theme, 
    fontSize, 
    toggleTheme,
    increaseFontSize,
    decreaseFontSize 
  } = React.useContext(ThemeContext);
  
  return (
    <div className={`settings-panel ${theme}`}>
      <h3>Settings</h3>
      <button onClick={toggleTheme}>Toggle Theme</button>
      <div>
        <button onClick={decreaseFontSize}>-</button>
        <span>Font Size: {fontSize}px</span>
        <button onClick={increaseFontSize}>+</button>
      </div>
    </div>
  );
}
```

### Reducer for Complex State

```jsx
const initialState = {
  users: [],
  selectedUser: null,
  loading: false,
  error: null,
  filter: ''
};

function userManagerReducer(state, action) {
  switch (action.type) {
    case 'SET_LOADING':
      return { ...state, loading: action.payload };
    
    case 'SET_USERS':
      return { 
        ...state, 
        users: action.payload, 
        loading: false 
      };
    
    case 'SELECT_USER':
      return { 
        ...state, 
        selectedUser: action.payload 
      };
    
    case 'ADD_USER':
      return { 
        ...state, 
        users: [...state.users, action.payload] 
      };
    
    case 'DELETE_USER':
      return {
        ...state,
        users: state.users.filter(u => u.id !== action.payload)
      };
    
    case 'SET_ERROR':
      return { 
        ...state, 
        error: action.payload, 
        loading: false 
      };
    
    case 'SET_FILTER':
      return { ...state, filter: action.payload };
    
    default:
      return state;
  }
}

// Using the reducer
function UserManager() {
  const [state, dispatch] = useReducer(userManagerReducer, initialState);
  
  const filteredUsers = state.users.filter(user =>
    user.name.toLowerCase().includes(state.filter.toLowerCase())
  );
  
  return (
    <div>
      <input
        value={state.filter}
        onChange={(e) => dispatch({ 
          type: 'SET_FILTER', 
          payload: e.target.value 
        })}
      />
      {state.loading && <p>Loading...</p>}
      <ul>
        {filteredUsers.map(user => (
          <li 
            key={user.id}
            onClick={() => dispatch({ 
              type: 'SELECT_USER', 
              payload: user 
            })}
          >
            {user.name}
          </li>
        ))}
      </ul>
    </div>
  );
}
```

---

## Component Composition

### Container/Presentational Pattern

```jsx
// Presentational Component - Only handles UI
function UserListView({ users, onSelectUser, selectedId }) {
  return (
    <ul className="user-list">
      {users.map(user => (
        <li 
          key={user.id}
          className={user.id === selectedId ? 'selected' : ''}
          onClick={() => onSelectUser(user)}
        >
          <img src={user.avatar} alt={user.name} />
          <div>
            <h4>{user.name}</h4>
            <p>{user.email}</p>
          </div>
        </li>
      ))}
    </ul>
  );
}

// Container Component - Handles logic and data
function UserListContainer() {
  const [users, setUsers] = useState([]);
  const [selectedId, setSelectedId] = useState(null);
  
  useEffect(() => {
    fetchUsers().then(setUsers);
  }, []);
  
  const handleSelectUser = (user) => {
    setSelectedId(user.id);
  };
  
  return (
    <UserListView 
      users={users} 
      onSelectUser={handleSelectUser}
      selectedId={selectedId}
    />
  );
}
```

### Slot Pattern

```jsx
function AppLayout({ header, sidebar, main, footer }) {
  return (
    <div className="app-layout">
      <header className="layout-header">{header}</header>
      <div className="layout-body">
        <aside className="layout-sidebar">{sidebar}</aside>
        <main className="layout-main">{main}</main>
      </div>
      <footer className="layout-footer">{footer}</footer>
    </div>
  );
}

// Usage
function App() {
  return (
    <AppLayout
      header={<h1>My App</h1>}
      sidebar={
        <nav>
          <a href="/">Home</a>
          <a href="/about">About</a>
        </nav>
      }
      main={<p>Welcome to my app!</p>}
      footer={<p>&copy; 2024</p>}
    />
  );
}
```

### Provider Pattern

```jsx
const ApiContext = React.createContext();

function ApiProvider({ baseUrl, children }) {
  const api = {
    get: (endpoint) => fetch(`${baseUrl}${endpoint}`).then(r => r.json()),
    post: (endpoint, data) => 
      fetch(`${baseUrl}${endpoint}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
      }).then(r => r.json()),
    put: (endpoint, data) => 
      fetch(`${baseUrl}${endpoint}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
      }).then(r => r.json()),
    delete: (endpoint) => 
      fetch(`${baseUrl}${endpoint}`, { method: 'DELETE' }).then(r => r.json())
  };
  
  return (
    <ApiContext.Provider value={api}>
      {children}
    </ApiContext.Provider>
  );
}

function UserService() {
  const api = React.useContext(ApiContext);
  
  const getUsers = () => api.get('/users');
  const createUser = (data) => api.post('/users', data);
  
  return { getUsers, createUser };
}
```

---

## Higher-Order Components

### What is a HOC?

A Higher-Order Component (HOC) is a function that takes a component and returns a new enhanced component.

```jsx
// Basic HOC
function withLogger(WrappedComponent) {
  return function WithLogger(props) {
    console.log('Rendering:', props.componentName);
    return <WrappedComponent {...props} />;
  };
}

// HOC with options
function withLoading(WrappedComponent, loadingIndicator) {
  return function WithLoading({ isLoading, ...props }) {
    if (isLoading) {
      return loadingIndicator || <p>Loading...</p>;
    }
    return <WrappedComponent {...props} />;
  };
}

// Usage
const EnhancedComponent = withLoading(
  MyComponent,
  <div>Custom loading...</div>
);

// <EnhancedComponent isLoading={true} data={...} />
```

### Common HOC Examples

```jsx
// Authentication HOC
function withAuth(WrappedComponent) {
  return function WithAuth(props) {
    const [user, setUser] = useState(null);
    
    useEffect(() => {
      checkAuth().then(setUser);
    }, []);
    
    if (!user) {
      return <p>Please log in</p>;
    }
    
    return <WrappedComponent {...props} user={user} />;
  };
}

// Analytics HOC
function withAnalytics(WrappedComponent) {
  return function WithAnalytics(props) {
    useEffect(() => {
      analytics.track('Component rendered', {
        component: WrappedComponent.name
      });
    }, []);
    
    return <WrappedComponent {...props} />;
  };
}

// Error Boundary HOC
function withErrorBoundary(WrappedComponent, fallback) {
  return function WithErrorBoundary(props) {
    const [error, setError] = useState(null);
    
    useEffect(() => {
      const handleError = (err) => {
        setError(err);
      };
      
      window.addEventListener('error', handleError);
      return () => window.removeEventListener('error', handleError);
    }, []);
    
    if (error) {
      return fallback || <p>Something went wrong</p>;
    }
    
    return <WrappedComponent {...props} />;
  };
}
```

---

## Compound Components

### Creating Compound Components

```jsx
function Menu({ children }) {
  const [activeIndex, setActiveIndex] = useState(0);
  
  // Create context for sharing state between compound parts
  const MenuContext = React.createContext();
  
  return (
    <MenuContext.Provider value={{ activeIndex, setActiveIndex }}>
      <div className="menu">{children}</div>
    </MenuContext.Provider>
  );
}

function MenuItem({ index, children }) {
  const { activeIndex, setActiveIndex } = React.useContext(MenuContext);
  const isActive = index === activeIndex;
  
  return (
    <button
      className={isActive ? 'active' : ''}
      onClick={() => setActiveIndex(index)}
    >
      {children}
    </button>
  );
}

function MenuContent({ children }) {
  const { activeIndex } = React.useContext(MenuContext);
  const contentChildren = React.Children.toArray(children);
  
  return <div>{contentChildren[activeIndex]}</div>;
}

// Attach sub-components
Menu.Item = MenuItem;
Menu.Content = MenuContent;

// Usage
function App() {
  return (
    <Menu>
      <Menu.Item index={0}>Home</Menu.Item>
      <Menu.Item index={1}>About</Menu.Item>
      <Menu.Item index={2}>Contact</Menu.Item>
      <Menu.Content>
        <div>Home content</div>
        <div>About content</div>
        <div>Contact content</div>
      </Menu.Content>
    </Menu>
  );
}
```

### Tabs Compound Component

```jsx
function Tabs({ children, defaultIndex = 0 }) {
  const [activeTab, setActiveTab] = useState(defaultIndex);
  const tabs = React.Children.toArray(children).filter(
    (child, index) => child.type === Tab
  );
  
  return (
    <div className="tabs">
      <div className="tab-list">
        {tabs.map((tab, index) => (
          <button
            key={index}
            className={index === activeTab ? 'active' : ''}
            onClick={() => setActiveTab(index)}
          >
            {tab.props.label}
          </button>
        ))}
      </div>
      <div className="tab-content">
        {children}
      </div>
    </div>
  );
}

function Tab({ label, children }) {
  return <div>{children}</div>;
}

// Usage
function App() {
  return (
    <Tabs defaultIndex={1}>
      <Tab label="Home">
        <p>Welcome to our website!</p>
      </Tab>
      <Tab label="About">
        <p>We are a company that makes great products.</p>
      </Tab>
      <Tab label="Contact">
        <p>Email us at contact@example.com</p>
      </Tab>
    </Tabs>
  );
}
```

---

## Controlled vs Uncontrolled

### Controlled Components

```jsx
// Controlled - state managed by React
function ControlledInput() {
  const [value, setValue] = useState('');
  
  return (
    <input
      value={value}
      onChange={(e) => setValue(e.target.value)}
    />
  );
}

// Controlled form
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
      />
      <input
        name="email"
        type="email"
        value={formData.email}
        onChange={handleChange}
      />
      <input
        name="password"
        type="password"
        value={formData.password}
        onChange={handleChange}
      />
      <button type="submit">Submit</button>
    </form>
  );
}
```

### Uncontrolled Components

```jsx
import { useRef } from 'react';

// Uncontrolled - state managed by DOM
function UncontrolledInput() {
  const inputRef = useRef(null);
  
  const handleSubmit = () => {
    console.log('Value:', inputRef.current.value);
  };
  
  return (
    <div>
      <input ref={inputRef} />
      <button onClick={handleSubmit}>Submit</button>
    </div>
  );
}

// File input (uncontrolled)
function FileUploader() {
  const fileInputRef = useRef(null);
  
  const handleUpload = () => {
    const files = fileInputRef.current.files;
    // Process files
  };
  
  return (
    <div>
      <input type="file" ref={fileInputRef} multiple />
      <button onClick={handleUpload}>Upload</button>
    </div>
  );
}
```

### When to Use Each

```jsx
// Use controlled when:
// - You need immediate validation
// - You want to disable the submit button based on input
// - You need to transform input on change

function ControlledValidation() {
  const [email, setEmail] = useState('');
  const [error, setError] = useState('');
  
  const validate = (value) => {
    if (!value.includes('@')) {
      setError('Invalid email');
    } else {
      setError('');
    }
  };
  
  return (
    <div>
      <input
        value={email}
        onChange={(e) => {
          setEmail(e.target.value);
          validate(e.target.value);
        }}
      />
      {error && <p>{error}</p>}
    </div>
  );
}

// Use uncontrolled when:
// - Working with third-party libraries
// - Simple forms
// - Performance optimization for large datasets
```

---

## Real-World Examples

### Modal Component

```jsx
function Modal({ isOpen, onClose, title, children }) {
  if (!isOpen) return null;
  
  return (
    <div className="modal-overlay" onClick={onClose}>
      <div 
        className="modal-content"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>{title}</h2>
          <button onClick={onClose}>&times;</button>
        </div>
        <div className="modal-body">
          {children}
        </div>
      </div>
    </div>
  );
}

// Usage with state
function App() {
  const [showModal, setShowModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  
  const handleEdit = (user) => {
    setSelectedUser(user);
    setShowModal(true);
  };
  
  return (
    <div>
      <UserList onEdit={handleEdit} />
      <Modal
        isOpen={showModal}
        onClose={() => setShowModal(false)}
        title="Edit User"
      >
        <UserForm 
          user={selectedUser}
          onSave={() => setShowModal(false)}
        />
      </Modal>
    </div>
  );
}
```

### Data Table Component

```jsx
function DataTable({ 
  data, 
  columns, 
  sortable = true,
  filterable = true 
}) {
  const [sortConfig, setSortConfig] = useState({ key: null, direction: 'asc' });
  const [filter, setFilter] = useState('');
  
  // Sort data
  const sortedData = useMemo(() => {
    if (!sortConfig.key) return data;
    
    return [...data].sort((a, b) => {
      if (a[sortConfig.key] < b[sortConfig.key]) {
        return sortConfig.direction === 'asc' ? -1 : 1;
      }
      if (a[sortConfig.key] > b[sortConfig.key]) {
        return sortConfig.direction === 'asc' ? 1 : -1;
      }
      return 0;
    });
  }, [data, sortConfig]);
  
  // Filter data
  const filteredData = useMemo(() => {
    if (!filter) return sortedData;
    
    return sortedData.filter(item =>
      Object.values(item).some(value =>
        String(value).toLowerCase().includes(filter.toLowerCase())
      )
    );
  }, [sortedData, filter]);
  
  const handleSort = (key) => {
    if (!sortable) return;
    
    setSortConfig(prev => ({
      key,
      direction: prev.key === key && prev.direction === 'asc' ? 'desc' : 'asc'
    }));
  };
  
  return (
    <div>
      {filterable && (
        <input
          value={filter}
          onChange={(e) => setFilter(e.target.value)}
          placeholder="Filter..."
        />
      )}
      <table>
        <thead>
          <tr>
            {columns.map(col => (
              <th 
                key={col.key}
                onClick={() => handleSort(col.key)}
              >
                {col.label}
                {sortConfig.key === col.key && (
                  <span>{sortConfig.direction === 'asc' ? ' ▲' : ' ▼'}</span>
                )}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {filteredData.map(row => (
            <tr key={row.id}>
              {columns.map(col => (
                <td key={col.key}>
                  {col.render ? col.render(row[col.key], row) : row[col.key]}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
```

---

## Practice Exercises

### Exercise 1: Accordion Component

```jsx
function Accordion({ items }) {
  const [openIndex, setOpenIndex] = useState(null);
  
  return (
    <div className="accordion">
      {items.map((item, index) => (
        <div key={index} className="accordion-item">
          <button
            className="accordion-header"
            onClick={() => setOpenIndex(
              openIndex === index ? null : index
            )}
          >
            {item.title}
            <span>{openIndex === index ? '−' : '+'}</span>
          </button>
          {openIndex === index && (
            <div className="accordion-content">
              {item.content}
            </div>
          )}
        </div>
      ))}
    </div>
  );
}

// Usage
function App() {
  const items = [
    { title: 'Section 1', content: 'Content 1' },
    { title: 'Section 2', content: 'Content 2' },
    { title: 'Section 3', content: 'Content 3' }
  ];
  
  return <Accordion items={items} />;
}
```

### Exercise 2: Dropdown Component

```jsx
function Dropdown({ options, onSelect, placeholder = 'Select...' }) {
  const [isOpen, setIsOpen] = useState(false);
  const [selected, setSelected] = useState(null);
  const dropdownRef = useRef(null);
  
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setIsOpen(false);
      }
    };
    
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);
  
  const handleSelect = (option) => {
    setSelected(option);
    onSelect(option);
    setIsOpen(false);
  };
  
  return (
    <div ref={dropdownRef} className="dropdown">
      <button 
        className="dropdown-toggle"
        onClick={() => setIsOpen(!isOpen)}
      >
        {selected ? selected.label : placeholder}
        <span>▼</span>
      </button>
      {isOpen && (
        <ul className="dropdown-menu">
          {options.map(option => (
            <li 
              key={option.value}
              onClick={() => handleSelect(option)}
            >
              {option.label}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
```

### Exercise 3: Pagination Component

```jsx
function Pagination({ totalItems, itemsPerPage, currentPage, onPageChange }) {
  const totalPages = Math.ceil(totalItems / itemsPerPage);
  
  const getPageNumbers = () => {
    const pages = [];
    const maxVisible = 5;
    
    let start = Math.max(1, currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(totalPages, start + maxVisible - 1);
    
    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    
    return pages;
  };
  
  return (
    <div className="pagination">
      <button
        disabled={currentPage === 1}
        onClick={() => onPageChange(currentPage - 1)}
      >
        Previous
      </button>
      {getPageNumbers().map(page => (
        <button
          key={page}
          className={page === currentPage ? 'active' : ''}
          onClick={() => onPageChange(page)}
        >
          {page}
        </button>
      ))}
      <button
        disabled={currentPage === totalPages}
        onClick={() => onPageChange(currentPage + 1)}
      >
        Next
      </button>
    </div>
  );
}
```

---

## Summary

### Key Takeaways

1. **Component Types**: Functional components with hooks are the modern standard
2. **Props**: Pass data and callbacks between components
3. **Lifecycle**: Understand mount, update, and unmount phases
4. **Patterns**: Use HOCs, render props, and compound components
5. **Controlled vs Uncontrolled**: Choose based on your needs

### Next Steps

- Continue with: [03_REACT_HOOKS_DEEP_DIVE.md](03_REACT_HOOKS_DEEP_DIVE.md)
- Learn about: React Router for navigation
- Study: State management with Redux/Zustand

### Related Resources

- [React Component Documentation](https://react.dev)
- [Thinking in React](https://react.dev/learn/thinking-in-react)

---

## Cross-References

- **Previous**: [01_REACT_FUNDAMENTALS.md](01_REACT_FUNDAMENTALS.md)
- **Next**: [03_REACT_HOOKS_DEEP_DIVE.md](03_REACT_HOOKS_DEEP_DIVE.md)

---

*Last updated: 2024*