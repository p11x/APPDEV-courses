# useTransition and useDeferredValue

## Overview

useTransition and useDeferredValue are React 18 hooks designed to handle expensive UI updates while keeping the application responsive. useTransition lets you mark state updates as non-blocking transitions, while useDeferredValue lets you defer updating less critical parts of the UI. These hooks are essential for building smooth, performant user interfaces that don't freeze during heavy computations.

## Prerequisites

- Understanding of React hooks (useState, useEffect)
- Knowledge of React rendering behavior
- Familiarity with performance optimization concepts

## Core Concepts

### What is useTransition?

useTransition lets you mark a state update as a "transition", allowing React to prioritize other more urgent updates.

```jsx
// File: src/usetransition-basics.jsx

import React, { useState, useTransition } from 'react';

function SearchComponent() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  
  // isPending indicates if transition is happening
  const [isPending, startTransition] = useTransition();
  
  const handleChange = (e) => {
    const value = e.target.value;
    
    // Immediate update for input
    setQuery(value);
    
    // Mark expensive operation as transition
    startTransition(() => {
      // This can be interrupted if urgent updates come in
      const newResults = expensiveSearch(value);
      setResults(newResults);
    });
  };
  
  return (
    <div>
      <input value={query} onChange={handleChange} />
      {isPending && <p>Loading...</p>}
      <ResultsList results={results} />
    </div>
  );
}

function expensiveSearch(query) {
  // Expensive computation
  return Array.from({ length: 1000 }, (_, i) => `Result ${i}`);
}
```

### What is useDeferredValue?

useDeferredValue lets you defer updating a value, useful when you have expensive rendering that can wait.

```jsx
// File: src/usedeferredvalue.jsx

import React, { useState, useDeferredValue } from 'react';

function SearchComponent() {
  const [query, setQuery] = useState('');
  
  // Deferred version of query - updates later
  const deferredQuery = useDeferredValue(query);
  
  // Expensive rendering can use deferred value
  const results = deferredQuery ? expensiveRender(deferredQuery) : [];
  
  return (
    <div>
      <input value={query} onChange={e => setQuery(e.target.value)} />
      <ExpensiveList results={results} />
    </div>
  );
}

function expensiveRender(query) {
  // Expensive computation
  return Array.from({ length: 1000 }, (_, i) => `${query} - ${i}`);
}
```

## Common Mistakes

### Mistake 1: Using for Everything

```jsx
// ❌ WRONG - Overusing transitions
function BadComponent() {
  const [isPending, startTransition] = useTransition();
  
  const handleClick = () => {
    startTransition(() => {
      setCount(c => c + 1); // Not worth transitioning
    });
  };
}

// ✅ CORRECT - Only use for expensive operations
function GoodComponent() {
  const [isPending, startTransition] = useTransition();
  
  const handleSearch = (query) => {
    startTransition(() => {
      // Expensive search worth transitioning
      setResults(expensiveSearch(query));
    });
  };
}
```

## Real-World Example

```jsx
// File: src/components/SearchableList.jsx

import React, { useState, useTransition, useMemo, useDeferredValue } from 'react';

function SearchableList() {
  const [query, setQuery] = useState('');
  const [isPending, startTransition] = useTransition();
  
  // Generate large list
  const allItems = useMemo(() => {
    return Array.from({ length: 10000 }, (_, i) => ({
      id: i,
      name: `Item ${i}`,
      category: ['A', 'B', 'C', 'D'][i % 4]
    }));
  }, []);
  
  const handleChange = (e) => {
    const value = e.target.value;
    setQuery(value);
    
    startTransition(() => {
      // Filter happens in background
      setFilteredItems(
        value 
          ? allItems.filter(item => item.name.toLowerCase().includes(value.toLowerCase()))
          : allItems
      );
    });
  };
  
  const [filteredItems, setFilteredItems] = useState(allItems);
  
  return (
    <div style={{ padding: '20px' }}>
      <h2>Searchable List (10,000 items)</h2>
      
      <input
        type="text"
        value={query}
        onChange={handleChange}
        placeholder="Search..."
        style={{ 
          width: '100%', 
          padding: '10px', 
          fontSize: '16px',
          marginBottom: '10px'
        }}
      />
      
      {isPending && (
        <div style={{ color: '#666', marginBottom: '10px' }}>
          Filtering...
        </div>
      )}
      
      <div style={{ 
        height: '400px', 
        overflow: 'auto', 
        border: '1px solid #ddd',
        padding: '10px'
      }}>
        {filteredItems.slice(0, 100).map(item => (
          <div 
            key={item.id} 
            style={{ 
              padding: '8px', 
              borderBottom: '1px solid #eee',
              display: 'flex',
              justifyContent: 'space-between'
            }}
          >
            <span>{item.name}</span>
            <span style={{ color: '#666' }}>{item.category}</span>
          </div>
        ))}
        {filteredItems.length > 100 && (
          <p style={{ color: '#666', textAlign: 'center' }}>
            Showing 100 of {filteredItems.length} results
          </p>
        )}
      </div>
    </div>
  );
}

export default SearchableList;
```

## Key Takeaways

- useTransition marks state updates as non-blocking
- isPending indicates if transition is happening
- useDeferredValue defers a value for deferred rendering
- Both help keep UI responsive during expensive updates
- Only use for truly expensive operations

## What's Next

Now let's explore building custom hooks - reusable logic that extracts component code into separate functions.
