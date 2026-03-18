# useMemo and useCallback

## Overview

useMemo and useCallback are React hooks for performance optimization. useMemo memoizes a computed value, while useCallback memoizes a function. Both help prevent unnecessary calculations and re-renders by only recalculating when their dependencies change. Understanding when and how to use these hooks is essential for building performant React applications.

## Prerequisites

- Understanding of React hooks (useState, useEffect)
- Knowledge of React re-rendering behavior
- Familiarity with JavaScript closures

## Core Concepts

### What is useMemo?

useMemo memoizes a computed value. It only recalculates the value when its dependencies change.

```jsx
// File: src/usememo-basics.jsx

import React, { useState, useMemo } from 'react';

function ExpensiveComponent() {
  const [count, setCount] = useState(0);
  const [items] = useState([1, 2, 3, 4, 5]);
  
  // This only recalculates when items changes
  const total = useMemo(() => {
    console.log('Calculating total...');
    return items.reduce((sum, item) => sum + item, 0);
  }, [items]);
  
  // This recalculates on EVERY render
  const badTotal = items.reduce((sum, item) => sum + item, 0);
  
  return (
    <div>
      <p>Total (memoized): {total}</p>
      <p>Total (not memoized): {badTotal}</p>
      <button onClick={() => setCount(count + 1)}>Count: {count}</button>
    </div>
  );
}
```

### When to Use useMemo

```jsx
// File: src/usememo-usecases.jsx

import React, { useState, useMemo } from 'react';

function UseMemoExamples() {
  const [users] = useState([
    { id: 1, name: 'Alice', age: 25 },
    { id: 2, name: 'Bob', age: 30 },
    { id: 3, name: 'Charlie', age: 25 }
  ]);
  
  const [filter, setFilter] = useState('');
  
  // 1. Expensive calculations
  const expensiveResult = useMemo(() => {
    return users
      .filter(u => u.name.toLowerCase().includes(filter.toLowerCase()))
      .sort((a, b) => a.name.localeCompare(b.name))
      .map(u => ({ ...u, name: u.name.toUpperCase() }));
  }, [users, filter]);
  
  // 2. Creating objects (to prevent new reference each render)
  const config = useMemo(() => ({
    apiUrl: 'https://api.example.com',
    timeout: 5000,
    retries: 3
  }), []);
  
  // 3. Filtering and sorting
  const sortedUsers = useMemo(() => {
    return [...users].sort((a, b) => a.age - b.age);
  }, [users]);
  
  // 4. Derived state
  const userCount = useMemo(() => users.length, [users]);
  const averageAge = useMemo(() => {
    if (users.length === 0) return 0;
    return users.reduce((sum, u) => sum + u.age, 0) / users.length;
  }, [users]);
  
  return (
    <div>
      <p>User count: {userCount}</p>
      <p>Average age: {averageAge.toFixed(1)}</p>
      <input value={filter} onChange={e => setFilter(e.target.value)} />
      {expensiveResult.map(u => <div key={u.id}>{u.name}</div>)}
    </div>
  );
}
```

### What is useCallback?

useCallback memoizes a function. It's useful when passing callbacks to child components that use React.memo.

```jsx
// File: src/usecallback-basics.jsx

import React, { useState, useCallback } from 'react';

// Child component that only re-renders when its props change
const MemoizedChild = React.memo(function MemoizedChild({ onClick }) {
  console.log('Child rendered');
  return <button onClick={onClick}>Click me</button>;
});

function Parent() {
  const [count, setCount] = useState(0);
  
  // Without useCallback, new function created each render
  const handleClick = () => {
    console.log('Clicked!');
  };
  
  // With useCallback, function is memoized
  const handleClickMemoized = useCallback(() => {
    console.log('Clicked!');
  }, []); // Empty deps = function never changes
  
  // Function with dependencies
  const handleClickWithCount = useCallback((name) => {
    console.log(`Clicked by ${name}! Count: ${count}`);
  }, [count]); // Recreates when count changes
  
  return (
    <div>
      <p>Count: {count}</p>
      <MemoizedChild onClick={handleClick} />
      <MemoizedChild onClick={handleClickMemoized} />
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Overusing useMemo/useCallback

```jsx
// ❌ WRONG - Premature optimization
function BadComponent() {
  const [count, setCount] = useState(0);
  
  // Unnecessary memoization for simple values
  const doubled = useMemo(() => count * 2, [count]);
  
  // Unnecessary memoization for simple functions
  const handleClick = useCallback(() => console.log('click'), []);
  
  return <button onClick={handleClick}>{doubled}</button>;
}

// ✅ CORRECT - Only memoize expensive operations
function GoodComponent() {
  const [count, setCount] = useState(0);
  
  // Simple calculation - don't memoize
  const doubled = count * 2;
  
  // Simple function - don't memoize
  const handleClick = () => console.log('click');
  
  return <button onClick={handleClick}>{doubled}</button>;
}
```

### Mistake 2: Missing Dependencies

```jsx
// ❌ WRONG - Missing dependencies
function BadComponent() {
  const [count, setCount] = useState(0);
  
  const handleClick = useCallback(() => {
    console.log(count); // Uses count but not in deps!
  }, []); // Wrong!
  
  return <button onClick={handleClick}>{count}</button>;
}

// ✅ CORRECT - Include all dependencies
function GoodComponent() {
  const [count, setCount] = useState(0);
  
  const handleClick = useCallback(() => {
    console.log(count);
  }, [count]); // Correct!
  
  return <button onClick={handleClick}>{count}</button>;
}
```

## Real-World Example

```jsx
// File: src/components/DataTable.jsx

import React, { useState, useMemo, useCallback } from 'react';

function DataTable({ data }) {
  const [sortConfig, setSortConfig] = useState({ key: null, direction: 'asc' });
  const [filterText, setFilterText] = useState('');
  
  // Memoized filtered data
  const filteredData = useMemo(() => {
    if (!filterText) return data;
    
    return data.filter(item =>
      Object.values(item).some(value =>
        String(value).toLowerCase().includes(filterText.toLowerCase())
      )
    );
  }, [data, filterText]);
  
  // Memoized sorted data
  const sortedData = useMemo(() => {
    if (!sortConfig.key) return filteredData;
    
    return [...filteredData].sort((a, b) => {
      const aVal = a[sortConfig.key];
      const bVal = b[sortConfig.key];
      
      if (aVal < bVal) return sortConfig.direction === 'asc' ? -1 : 1;
      if (aVal > bVal) return sortConfig.direction === 'asc' ? 1 : -1;
      return 0;
    });
  }, [filteredData, sortConfig]);
  
  // Memoized sort handler
  const handleSort = useCallback((key) => {
    setSortConfig(prev => ({
      key,
      direction: prev.key === key && prev.direction === 'asc' ? 'desc' : 'asc'
    }));
  }, []);
  
  // Memoized columns
  const columns = useMemo(() => {
    if (data.length === 0) return [];
    return Object.keys(data[0]);
  }, [data]);
  
  return (
    <div>
      <input
        value={filterText}
        onChange={e => setFilterText(e.target.value)}
        placeholder="Filter..."
      />
      
      <table>
        <thead>
          <tr>
            {columns.map(col => (
              <th key={col} onClick={() => handleSort(col)}>
                {col}
                {sortConfig.key === col && (
                  <span>{sortConfig.direction === 'asc' ? ' ▲' : ' ▼'}</span>
                )}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {sortedData.map((row, i) => (
            <tr key={i}>
              {columns.map(col => (
                <td key={col}>{row[col]}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default DataTable;
```

## Key Takeaways

- useMemo memoizes computed values
- useCallback memoizes functions
- Both accept a dependency array
- Only use when there's actual performance benefit
- Don't over-optimize - profile first
- Missing dependencies can cause bugs

## What's Next

Now let's explore useTransition and useDeferredValue - hooks for handling expensive updates while keeping the UI responsive.
