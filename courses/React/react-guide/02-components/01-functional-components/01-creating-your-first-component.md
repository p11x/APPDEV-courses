# Creating Your First Component

## Overview

Components are the fundamental building blocks of any React application. They let you split the UI into independent, reusable pieces. In this guide, you'll learn how to create functional components using modern React practices. Functional components are the recommended way to write React components since the introduction of hooks in React 16.8.

## Prerequisites

- Basic understanding of JavaScript functions
- Knowledge of JSX syntax
- Familiarity with React basics (from previous section)
- Understanding of how to run a React application

## Core Concepts

### What is a Component?

A React component is a JavaScript function that returns JSX (JavaScript XML). Components let you split the UI into independent, reusable pieces. Think of components as custom, reusable HTML elements.

```jsx
// File: src/components/BasicComponent.jsx

import React from 'react';

// This is a functional component - a JavaScript function that returns JSX
function Welcome() {
  return <h1>Hello, World!</h1>;
}

// Components can also be arrow functions
const Greeting = () => {
  return <p>Welcome to our app!</p>;
};

// Components can accept data called "props"
function UserGreeting({ name }) {
  return <h1>Hello, {name}!</h1>;
}

// Export the component to make it available for import
export default Welcome;
```

### Creating Your First Component

Let's create a complete, working component step by step:

```jsx
// File: src/components/ProductCard.jsx

import React from 'react';

// Step 1: Define the component function
// Use PascalCase for component names (first letter capitalized)
function ProductCard() {
  // Step 2: Return JSX
  // The JSX describes what the UI should look like
  return (
    <div className="product-card">
      <img 
        src="https://via.placeholder.com/200" 
        alt="Product" 
      />
      <h2>Product Name</h2>
      <p className="price">$99.99</p>
      <button>Add to Cart</button>
    </div>
  );
}

// Step 3: Export the component
export default ProductCard;
```

Now let's use this component in our app:

```jsx
// File: src/App.jsx

import React from 'react';
import ProductCard from './components/ProductCard';

function App() {
  return (
    <div className="app">
      <h1>My Store</h1>
      <div className="products">
        {/* We can use our component multiple times */}
        <ProductCard />
        <ProductCard />
        <ProductCard />
      </div>
    </div>
  );
}

export default App;
```

### Components with Props

Props (short for properties) allow you to pass data to components, making them dynamic and reusable:

```jsx
// File: src/components/DynamicProductCard.jsx

import React from 'react';

// Components can accept props as parameters
function ProductCard({ name, price, image, description, onAddToCart }) {
  // Use the props in your JSX
  return (
    <div className="product-card">
      <img 
        src={image || 'https://via.placeholder.com/200'} 
        alt={name} 
      />
      <h2>{name}</h2>
      <p className="description">{description}</p>
      <p className="price">${price?.toFixed(2) ?? '0.00'}</p>
      <button onClick={onAddToCart}>
        Add to Cart
      </button>
    </div>
  );
}

// Default props provide fallback values
ProductCard.defaultProps = {
  name: 'Unknown Product',
  price: 0,
  description: 'No description available',
  onAddToCart: () => console.log('Added to cart!')
};

export default ProductCard;
```

### Using Props in the Parent Component

```jsx
// File: src/AppWithProps.jsx

import React from 'react';
import ProductCard from './components/DynamicProductCard';

function App() {
  // Sample data - in a real app, this might come from an API
  const products = [
    {
      id: 1,
      name: 'Laptop Pro',
      price: 1299.99,
      description: 'Powerful laptop for professionals',
      image: 'https://via.placeholder.com/200?text=Laptop'
    },
    {
      id: 2,
      name: 'Wireless Mouse',
      price: 49.99,
      description: 'Ergonomic wireless mouse',
      image: 'https://via.placeholder.com/200?text=Mouse'
    },
    {
      id: 3,
      name: 'Mechanical Keyboard',
      price: 149.99,
      description: 'RGB mechanical keyboard',
      image: 'https://via.placeholder.com/200?text=Keyboard'
    }
  ];
  
  // Handler function for add to cart
  const handleAddToCart = (productName) => {
    alert(`${productName} added to cart!`);
  };
  
  return (
    <div className="app">
      <h1>My Store</h1>
      <div className="products-grid">
        {products.map(product => (
          <ProductCard
            key={product.id}
            name={product.name}
            price={product.price}
            description={product.description}
            image={product.image}
            onAddToCart={() => handleAddToCart(product.name)}
          />
        ))}
      </div>
    </div>
  );
}

export default App;
```

### Component Composition

Components can contain other components - this is called composition:

```jsx
// File: src/components/ProductComponents.jsx

import React from 'react';

// Small, focused components
function ProductImage({ src, alt }) {
  return (
    <img 
      src={src} 
      alt={alt} 
      style={{ width: '100%', borderRadius: '8px' }}
    />
  );
}

function ProductTitle({ children }) {
  return (
    <h2 style={{ margin: '10px 0', color: '#333' }}>
      {children}
    </h2>
  );
}

function ProductPrice({ price }) {
  return (
    <p style={{ 
      fontSize: '20px', 
      fontWeight: 'bold', 
      color: '#4CAF50',
      margin: '10px 0'
    }}>
      ${price.toFixed(2)}
    </p>
  );
}

function ProductButton({ onClick, children }) {
  return (
    <button 
      onClick={onClick}
      style={{
        backgroundColor: '#2196F3',
        color: 'white',
        border: 'none',
        padding: '10px 20px',
        borderRadius: '4px',
        cursor: 'pointer',
        width: '100%'
      }}
    >
      {children}
    </button>
  );
}

// Compose them together
function ProductCard({ name, price, image, onAddToCart }) {
  return (
    <div style={{
      border: '1px solid #ddd',
      borderRadius: '8px',
      padding: '16px',
      maxWidth: '300px'
    }}>
      <ProductImage src={image} alt={name} />
      <ProductTitle>{name}</ProductTitle>
      <ProductPrice price={price} />
      <ProductButton onClick={onAddToCart}>
        Add to Cart
      </ProductButton>
    </div>
  );
}

export default ProductCard;
```

## Common Mistakes

### Mistake 1: Not Using PascalCase

```jsx
// ❌ WRONG - lowercase component name
function welcome() {
  return <h1>Hello</h1>;
}

// ✅ CORRECT - PascalCase (first letter capitalized)
function Welcome() {
  return <h1>Hello</h1>;
}
```

### Mistake 2: Forgetting to Return JSX

```jsx
// ❌ WRONG - No return statement
function BadComponent() {
  const message = 'Hello';
  // Missing return!
}

// ✅ CORRECT - Return JSX
function GoodComponent() {
  return <h1>Hello</h1>;
}

// Arrow functions need explicit return
const BadArrow = () => {
  <h1>Hello</h1>; // No return!
};

const GoodArrow = () => {
  return <h1>Hello</h1>;
};

// Or implicit return (no curly braces)
const ImplicitArrow = () => <h1>Hello</h1>;
```

### Mistake 3: Mutating Props

```jsx
// ❌ WRONG - Never mutate props!
function BadComponent({ name }) {
  name = name.toUpperCase(); // Don't do this!
  return <h1>{name}</h1>;
}

// ✅ CORRECT - Create a new variable
function GoodComponent({ name }) {
  const formattedName = name.toUpperCase();
  return <h1>{formattedName}</h1>;
}
```

### Mistake 4: Not Handling Missing Props

```jsx
// ❌ WRONG - No fallback for undefined props
function BadComponent({ name }) {
  return <h1>{name.toUpperCase()}</h1>; // Crashes if name is undefined!
}

// ✅ CORRECT - Use optional chaining and nullish coalescing
function GoodComponent({ name }) {
  return <h1>{name?.toUpperCase() ?? 'Guest'}</h1>;
}

// Or use default props
function BetterComponent({ name = 'Guest' }) {
  return <h1>{name.toUpperCase()}</h1>;
}
```

## Real-World Example

Let's build a complete dashboard with multiple components:

```jsx
// File: src/components/StatsCard.jsx

import React from 'react';

function StatsCard({ title, value, icon, trend, color = 'blue' }) {
  // Dynamic styles based on props
  const colorMap = {
    blue: { bg: '#E3F2FD', text: '#1976D2' },
    green: { bg: '#E8F5E9', text: '#388E3C' },
    orange: { bg: '#FFF3E0', text: '#F57C00' },
    red: { bg: '#FFEBEE', text: '#D32F2F' }
  };
  
  const colors = colorMap[color] || colorMap.blue;
  
  return (
    <div style={{
      backgroundColor: 'white',
      borderRadius: '8px',
      padding: '20px',
      boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
      display: 'flex',
      flexDirection: 'column',
      gap: '10px'
    }}>
      <div style={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center'
      }}>
        <span style={{ color: '#666', fontSize: '14px' }}>{title}</span>
        <span style={{ fontSize: '24px' }}>{icon}</span>
      </div>
      
      <div style={{ fontSize: '28px', fontWeight: 'bold', color: colors.text }}>
        {value}
      </div>
      
      {trend && (
        <div style={{
          fontSize: '12px',
          color: trend > 0 ? '#4CAF50' : '#f44336'
        }}>
          {trend > 0 ? '↑' : '↓'} {Math.abs(trend)}% from last week
        </div>
      )}
    </div>
  );
}

export default StatsCard;
```

```jsx
// File: src/components/UserList.jsx

import React from 'react';

function UserAvatar({ src, name, size = 40 }) {
  return (
    <img
      src={src || `https://via.placeholder.com/${size}`}
      alt={name}
      style={{
        width: size,
        height: size,
        borderRadius: '50%',
        objectFit: 'cover'
      }}
    />
  );
}

function UserInfo({ name, email, role }) {
  return (
    <div>
      <div style={{ fontWeight: '500' }}>{name}</div>
      <div style={{ fontSize: '12px', color: '#666' }}>{email}</div>
    </div>
  );
}

function UserBadge({ role }) {
  const roleColors = {
    admin: '#9c27b0',
    moderator: '#2196F3',
    user: '#4CAF50'
  };
  
  return (
    <span style={{
      backgroundColor: roleColors[role] || roleColors.user,
      color: 'white',
      padding: '2px 8px',
      borderRadius: '4px',
      fontSize: '12px'
    }}>
      {role}
    </span>
  );
}

function UserList({ users }) {
  return (
    <div style={{
      backgroundColor: 'white',
      borderRadius: '8px',
      padding: '20px',
      boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
    }}>
      <h3 style={{ marginTop: 0 }}>Users</h3>
      
      {users.length === 0 ? (
        <p style={{ color: '#666' }}>No users found</p>
      ) : (
        <ul style={{ listStyle: 'none', padding: 0, margin: 0 }}>
          {users.map(user => (
            <li 
              key={user.id}
              style={{
                display: 'flex',
                alignItems: 'center',
                gap: '12px',
                padding: '12px 0',
                borderBottom: '1px solid #eee'
              }}
            >
              <UserAvatar src={user.avatar} name={user.name} />
              <UserInfo name={user.name} email={user.email} />
              <UserBadge role={user.role} />
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default UserList;
```

```jsx
// File: src/AppDashboard.jsx

import React from 'react';
import StatsCard from './components/StatsCard';
import UserList from './components/UserList';

function App() {
  // Sample data
  const stats = [
    { title: 'Total Users', value: '1,234', icon: '👥', trend: 12, color: 'blue' },
    { title: 'Revenue', value: '$45,678', icon: '💰', trend: 8, color: 'green' },
    { title: 'Orders', value: '345', icon: '📦', trend: -3, color: 'orange' },
    { title: 'Issues', value: '12', icon: '⚠️', trend: -15, color: 'red' }
  ];
  
  const users = [
    { id: 1, name: 'Alice Johnson', email: 'alice@example.com', role: 'admin', avatar: 'https://i.pravatar.cc/150?img=1' },
    { id: 2, name: 'Bob Smith', email: 'bob@example.com', role: 'user', avatar: 'https://i.pravatar.cc/150?img=2' },
    { id: 3, name: 'Charlie Brown', email: 'charlie@example.com', role: 'moderator', avatar: 'https://i.pravatar.cc/150?img=3' },
    { id: 4, name: 'Diana Prince', email: 'diana@example.com', role: 'user', avatar: 'https://i.pravatar.cc/150?img=4' }
  ];
  
  return (
    <div style={{ padding: '20px', backgroundColor: '#f5f5f5', minHeight: '100vh' }}>
      <h1 style={{ marginBottom: '20px' }}>Dashboard</h1>
      
      {/* Stats Grid */}
      <div style={{
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
        gap: '20px',
        marginBottom: '30px'
      }}>
        {stats.map((stat, index) => (
          <StatsCard
            key={index}
            title={stat.title}
            value={stat.value}
            icon={stat.icon}
            trend={stat.trend}
            color={stat.color}
          />
        ))}
      </div>
      
      {/* User List */}
      <UserList users={users} />
    </div>
  );
}

export default App;
```

## Key Takeaways

- Components are JavaScript functions that return JSX
- Always use PascalCase for component names (e.g., `MyComponent` not `myComponent`)
- Components can accept props to make them dynamic and reusable
- Use optional chaining (`?.`) and nullish coalescing (`??`) to handle missing props
- Component composition allows you to build complex UIs from simple pieces
- Never mutate props - they are read-only
- Use arrow function syntax or explicit returns, but be consistent
- Export components using `export default` for single exports or named exports for multiple

## What's Next

Now that you can create components, let's learn about props in detail - how to pass data to components and make them truly reusable.
