# Expressions in JSX

## Overview

JavaScript expressions are the backbone of dynamic React UIs. By embedding expressions within JSX using curly braces, you can render dynamic data, implement conditional rendering, loop through arrays, and create highly interactive components. This guide covers all the ways you can use JavaScript expressions in JSX to build powerful React applications.

## Prerequisites

- Understanding of JSX basics (from previous lesson)
- Knowledge of JavaScript operators (ternary, logical, spread)
- Familiarity with array methods (map, filter, reduce)
- Understanding of React components and props

## Core Concepts

### Basic Expression Rendering

The most fundamental use of expressions in JSX is displaying dynamic data:

```jsx
// File: src/expressions/basic-rendering.jsx

import React from 'react';

function BasicRendering() {
  const name = 'Alice';
  const age = 30;
  const isMember = true;
  const items = ['Apple', 'Banana', 'Cherry'];
  
  return (
    <div>
      {/* Simple variable interpolation */}
      <h1>Hello, {name}!</h1>
      
      {/* Mathematical expressions */}
      <p>Age doubled: {age * 2}</p>
      
      {/* Function calls */}
      <p>Uppercase: {name.toUpperCase()}</p>
      <p>Length: {name.length}</p>
      
      {/* Boolean rendering (won't display anything) */}
      <p>Is member: {isMember}</p> {/* This shows 'true' or 'false' */}
      
      {/* Template literals */}
      <p>{`${name} is ${age} years old`}</p>
    </div>
  );
}
```

### Conditional Rendering

React offers several patterns for conditionally rendering content:

```jsx
// File: src/expressions/conditional-rendering.jsx

import React from 'react';

function ConditionalRendering({ user, isLoading, hasError }) {
  // Method 1: Ternary operator (like if-else)
  const greeting = user ? (
    <p>Welcome back, {user.name}!</p>
  ) : (
    <p>Please log in</p>
  );
  
  // Method 2: Logical AND operator (like if)
  const memberBadge = user?.isMember && (
    <span className="badge">Premium Member</span>
  );
  
  // Method 3: Ternary with null (hide element)
  const statusMessage = hasError ? (
    <p className="error">Something went wrong</p>
  ) : null;
  
  // Method 4: Immediately invoked function (complex conditions)
  const userLevel = (() => {
    if (!user) return 'Guest';
    if (user.posts > 100) return 'Expert';
    if (user.posts > 10) return 'Active';
    return 'Newcomer';
  })();
  
  // Method 5: Variable assignment with conditions
  let buttonText;
  if (isLoading) {
    buttonText = 'Loading...';
  } else if (hasError) {
    buttonText = 'Try Again';
  } else {
    buttonText = 'Submit';
  }
  
  return (
    <div>
      {greeting}
      {memberBadge}
      {statusMessage}
      <p>Your level: {userLevel}</p>
      <button disabled={isLoading}>{buttonText}</button>
    </div>
  );
}
```

### Array Rendering with Map

One of the most common patterns in React is rendering lists using `map()`:

```jsx
// File: src/expressions/array-rendering.jsx

import React from 'react';

function ArrayRendering() {
  const fruits = ['Apple', 'Banana', 'Cherry'];
  const users = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob', email: 'bob@example.com' },
    { id: 3, name: 'Charlie', email: 'charlie@example.com' },
  ];
  
  const products = [
    { id: 1, name: 'Laptop', price: 999, inStock: true },
    { id: 2, name: 'Phone', price: 699, inStock: false },
    { id: 3, name: 'Tablet', price: 449, inStock: true },
  ];
  
  return (
    <div>
      {/* Basic array rendering */}
      <h3>Fruits</h3>
      <ul>
        {fruits.map((fruit, index) => (
          <li key={index}>{fruit}</li>
        ))}
      </ul>
      
      {/* Object array with more details */}
      <h3>Users</h3>
      <ul>
        {users.map(user => (
          <li key={user.id}>
            <strong>{user.name}</strong> - {user.email}
          </li>
        ))}
      </ul>
      
      {/* Conditional rendering within list */}
      <h3>Products</h3>
      <ul>
        {products.map(product => (
          <li key={product.id}>
            {product.name} - ${product.price}
            {product.inStock ? (
              <span style={{ color: 'green' }}> (In Stock)</span>
            ) : (
              <span style={{ color: 'red' }}> (Out of Stock)</span>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
```

### Filtering and Sorting in JSX

You can filter and sort arrays directly within JSX:

```jsx
// File: src/expressions/filter-sort.jsx

import React from 'react';

function FilterSortExample() {
  const allProducts = [
    { id: 1, name: 'Laptop', category: 'Electronics', price: 999 },
    { id: 2, name: 'Apple', category: 'Food', price: 2 },
    { id: 3, name: 'Shirt', category: 'Clothing', price: 29 },
    { id: 4, name: 'Phone', category: 'Electronics', price: 699 },
    { id: 5, name: 'Banana', category: 'Food', price: 1 },
    { id: 6, name: 'Pants', category: 'Clothing', price: 59 },
  ];
  
  return (
    <div>
      {/* Filter and map in one expression */}
      <h3>Electronics Only</h3>
      <ul>
        {allProducts
          .filter(product => product.category === 'Electronics')
          .map(product => (
            <li key={product.id}>{product.name} - ${product.price}</li>
          ))}
      </ul>
      
      {/* Sort and map */}
      <h3>Sorted by Price (Low to High)</h3>
      <ul>
        {[...allProducts]
          .sort((a, b) => a.price - b.price)
          .map(product => (
            <li key={product.id}>{product.name} - ${product.price}</li>
          ))}
      </ul>
      
      {/* Filter, sort, and limit */}
      <h3>Top 3 Cheapest Products</h3>
      <ul>
        {[...allProducts]
          .sort((a, b) => a.price - b.price)
          .slice(0, 3)
          .map(product => (
            <li key={product.id}>{product.name} - ${product.price}</li>
          ))}
      </ul>
    </div>
  );
}
```

### Object and Destructuring in JSX

```jsx
// File: src/expressions/objects.jsx

import React from 'react';

function ObjectExpressions() {
  const user = {
    name: 'Alice',
    age: 30,
    address: {
      city: 'New York',
      country: 'USA'
    },
    hobbies: ['Reading', 'Coding', 'Hiking']
  };
  
  // Nested property access
  const city = user.address?.city ?? 'Unknown';
  
  // Object methods
  const hobbyList = user.hobbies.join(', ');
  
  // Spread operator (Note: can't spread directly in JSX, need to use in assignment)
  const updatedUser = { ...user, age: 31 };
  
  return (
    <div>
      <h3>{user.name}'s Profile</h3>
      <p>Age: {user.age}</p>
      <p>City: {city}</p>
      <p>Hobbies: {hobbyList}</p>
      <p>New Age: {updatedUser.age}</p>
    </div>
  );
}

// Destructuring in component props
function UserCard({ name, age, address: { city } }) {
  return (
    <div>
      <h3>{name}</h3>
      <p>Age: {age}</p>
      <p>City: {city}</p>
    </div>
  );
}

// Usage
function DestructuringExample() {
  const user = { 
    name: 'Bob', 
    age: 25, 
    address: { city: 'Boston' } 
  };
  
  return <UserCard {...user} />;
}
```

## Common Mistakes

### Mistake 1: Using Statements Instead of Expressions

```jsx
// ❌ WRONG - if/else are statements, not expressions
function BadComponent() {
  const user = { name: 'Alice' };
  
  return (
    <div>
      {if (user) { <p>{user.name}</p> }} {/* This doesn't work! */}
    </div>
  );
}

// ✅ CORRECT - Use ternary operator for conditionals
function GoodComponent() {
  const user = { name: 'Alice' };
  
  return (
    <div>
      {user ? <p>{user.name}</p> : null}
    </div>
  );
}
```

### Mistake 2: Forgetting the Key Prop

```jsx
// ❌ WRONG - Missing key causes React warnings
function BadList() {
  const items = ['a', 'b', 'c'];
  return (
    <ul>
      {items.map(item => <li>{item}</li>)}
    </ul>
  );
}

// ✅ CORRECT - Always provide unique keys
function GoodList() {
  const items = ['a', 'b', 'c'];
  return (
    <ul>
      {items.map((item, index) => <li key={index}>{item}</li>)}
    </ul>
  );
}

// Even better - use unique IDs when available
function BetterList() {
  const items = [
    { id: 1, name: 'a' },
    { id: 2, name: 'b' },
    { id: 3, name: 'c' },
  ];
  return (
    <ul>
      {items.map(item => <li key={item.id}>{item.name}</li>)}
    </ul>
  );
}
```

### Mistake 3: Not Handling Null/Undefined

```jsx
// ❌ WRONG - Trying to access properties on null/undefined
function BadComponent({ user }) {
  return <p>{user.name}</p>; // Crashes if user is undefined!
}

// ✅ CORRECT - Use optional chaining and nullish coalescing
function GoodComponent({ user }) {
  return <p>{user?.name ?? 'Unknown'}</p>;
}

// ✅ CORRECT - Conditional rendering
function BetterComponent({ user }) {
  return user ? <p>{user.name}</p> : <p>No user</p>;
}
```

### Mistake 4: Direct Object Spread in JSX

```jsx
// ❌ WRONG - Can't spread objects directly in JSX
function BadComponent() {
  const styles = { color: 'red', fontSize: '16px' };
  return <p {...styles}>Hello</p>; // This actually works for props!
}

// But this doesn't work:
function BadComponent2() {
  const user = { name: 'Alice', age: 30 };
  return <div>{...user}</div>; // ERROR!
}

// ✅ CORRECT - Use spread only on elements/attributes
function GoodComponent() {
  const user = { name: 'Alice', age: 30 };
  return <UserCard {...user} />;
}
```

## Real-World Example

Let's build a complete dashboard component demonstrating all expression patterns:

```jsx
// File: src/components/AdminDashboard.jsx

import React, { useState } from 'react';

function AdminDashboard() {
  // State for filtering
  const [filter, setFilter] = useState('all');
  const [sortBy, setSortBy] = useState('name');
  
  // Sample data
  const [orders] = useState([
    { id: 1, customer: 'Alice Smith', total: 150.00, status: 'completed', date: '2024-01-15' },
    { id: 2, customer: 'Bob Johnson', total: 89.99, status: 'pending', date: '2024-01-16' },
    { id: 3, customer: 'Charlie Brown', total: 250.50, status: 'completed', date: '2024-01-17' },
    { id: 4, customer: 'Diana Prince', total: 75.00, status: 'cancelled', date: '2024-01-18' },
    { id: 5, customer: 'Eve Wilson', total: 199.99, status: 'pending', date: '2024-01-19' },
  ]);
  
  // Derived data - filter and sort
  const filteredOrders = orders
    .filter(order => {
      if (filter === 'all') return true;
      return order.status === filter;
    })
    .sort((a, b) => {
      if (sortBy === 'name') return a.customer.localeCompare(b.customer);
      if (sortBy === 'total') return b.total - a.total; // descending
      if (sortBy === 'date') return new Date(b.date) - new Date(a.date);
      return 0;
    });
  
  // Statistics using reduce
  const stats = orders.reduce((acc, order) => ({
    totalRevenue: acc.totalRevenue + (order.status === 'completed' ? order.total : 0),
    totalOrders: acc.totalOrders + 1,
    pendingOrders: acc.pendingOrders + (order.status === 'pending' ? 1 : 0),
    completedOrders: acc.completedOrders + (order.status === 'completed' ? 1 : 0),
  }), { totalRevenue: 0, totalOrders: 0, pendingOrders: 0, completedOrders: 0 });
  
  // Status badge helper
  const getStatusBadge = (status) => {
    const badges = {
      completed: { color: '#4CAF50', text: '✓ Completed' },
      pending: { color: '#FF9800', text: '⏳ Pending' },
      cancelled: { color: '#f44336', text: '✗ Cancelled' },
    };
    const badge = badges[status];
    return <span style={{ 
      backgroundColor: badge.color, 
      color: 'white', 
      padding: '2px 8px', 
      borderRadius: '4px',
      fontSize: '12px'
    }}>{badge.text}</span>;
  };
  
  return (
    <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
      <h1>Order Dashboard</h1>
      
      {/* Statistics Cards */}
      <div style={{ 
        display: 'grid', 
        gridTemplateColumns: 'repeat(auto-fit, minmax(150px, 1fr))',
        gap: '15px',
        marginBottom: '30px'
      }}>
        <div style={{ border: '1px solid #ddd', padding: '15px', borderRadius: '8px' }}>
          <h4 style={{ margin: '0 0 10px 0', color: '#666' }}>Total Revenue</h4>
          <p style={{ margin: 0, fontSize: '24px', fontWeight: 'bold' }}>
            ${stats.totalRevenue.toFixed(2)}
          </p>
        </div>
        <div style={{ border: '1px solid #ddd', padding: '15px', borderRadius: '8px' }}>
          <h4 style={{ margin: '0 0 10px 0', color: '#666' }}>Total Orders</h4>
          <p style={{ margin: 0, fontSize: '24px', fontWeight: 'bold' }}>
            {stats.totalOrders}
          </p>
        </div>
        <div style={{ border: '1px solid #ddd', padding: '15px', borderRadius: '8px' }}>
          <h4 style={{ margin: '0 0 10px 0', color: '#666' }}>Pending</h4>
          <p style={{ margin: 0, fontSize: '24px', fontWeight: 'bold', color: '#FF9800' }}>
            {stats.pendingOrders}
          </p>
        </div>
        <div style={{ border: '1px solid #ddd', padding: '15px', borderRadius: '8px' }}>
          <h4 style={{ margin: '0 0 10px 0', color: '#666' }}>Completed</h4>
          <p style={{ margin: 0, fontSize: '24px', fontWeight: 'bold', color: '#4CAF50' }}>
            {stats.completedOrders}
          </p>
        </div>
      </div>
      
      {/* Filters */}
      <div style={{ marginBottom: '20px', display: 'flex', gap: '10px' }}>
        <label>
          Filter:
          <select 
            value={filter} 
            onChange={(e) => setFilter(e.target.value)}
            style={{ marginLeft: '10px', padding: '5px' }}
          >
            <option value="all">All Orders</option>
            <option value="completed">Completed</option>
            <option value="pending">Pending</option>
            <option value="cancelled">Cancelled</option>
          </select>
        </label>
        <label>
          Sort by:
          <select 
            value={sortBy} 
            onChange={(e) => setSortBy(e.target.value)}
            style={{ marginLeft: '10px', padding: '5px' }}
          >
            <option value="name">Customer Name</option>
            <option value="total">Total</option>
            <option value="date">Date</option>
          </select>
        </label>
      </div>
      
      {/* Orders List */}
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ backgroundColor: '#f5f5f5' }}>
            <th style={{ padding: '10px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Order ID</th>
            <th style={{ padding: '10px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Customer</th>
            <th style={{ padding: '10px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Date</th>
            <th style={{ padding: '10px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Total</th>
            <th style={{ padding: '10px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Status</th>
          </tr>
        </thead>
        <tbody>
          {filteredOrders.length > 0 ? (
            filteredOrders.map(order => (
              <tr key={order.id} style={{ borderBottom: '1px solid #eee' }}>
                <td style={{ padding: '10px' }}>#{order.id}</td>
                <td style={{ padding: '10px' }}>{order.customer}</td>
                <td style={{ padding: '10px' }}>{order.date}</td>
                <td style={{ padding: '10px' }}>${order.total.toFixed(2)}</td>
                <td style={{ padding: '10px' }}>{getStatusBadge(order.status)}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="5" style={{ padding: '20px', textAlign: 'center', color: '#666' }}>
                No orders found
              </td>
            </tr>
          )}
        </tbody>
      </table>
      
      <p style={{ marginTop: '20px', color: '#666' }}>
        Showing {filteredOrders.length} of {orders.length} orders
      </p>
    </div>
  );
}

export default AdminDashboard;
```

## Key Takeaways

- Use curly braces `{}` to embed JavaScript expressions in JSX
- Ternary operators (`condition ? true : false`) for conditional rendering
- Logical AND (`&&`) for showing/hiding elements based on condition
- Use `map()` to render lists, always provide unique `key` props
- Filter and sort arrays directly within JSX for derived data
- Use optional chaining (`?.`) and nullish coalescing (`??`) for safe property access
- Cannot use JavaScript statements (if/else, for, while) directly in JSX
- Use functions or ternary operators instead of statements for dynamic content

## What's Next

Now that you understand expressions in JSX, let's explore the key differences between JSX and HTML that often trip up newcomers.
