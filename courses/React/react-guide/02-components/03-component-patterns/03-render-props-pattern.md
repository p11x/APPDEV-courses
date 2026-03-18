# Render Props Pattern

## Overview

The render props pattern is a technique for sharing code between React components using a prop whose value is a function. Instead of hardcoding what a component renders, you pass it a function (called the "render prop") that tells the component what to render. This pattern provides an alternative to HOCs for code reuse and has been largely replaced by custom hooks in modern React, but understanding it helps you work with older codebases.

## Prerequisites

- Understanding of React components and props
- Knowledge of React state and effects
- Familiarity with function props
- Understanding of JavaScript functions

## Core Concepts

### What is a Render Prop?

A render prop is a function prop that a component uses to know what to render:

```jsx
// File: src/render-props-basics.jsx

import React, { useState } from 'react';

// Component with render prop
function MouseTracker({ render }) {
  const [position, setPosition] = useState({ x: 0, y: 0 });
  
  const handleMouseMove = (event) => {
    setPosition({
      x: event.clientX,
      y: event.clientY
    });
  };
  
  // Call the render prop with current state
  return (
    <div style={{ height: '100vh' }} onMouseMove={handleMouseMove}>
      {render(position)}
    </div>
  );
}

// Usage with render prop
function App() {
  return (
    <MouseTracker
      render={({ x, y }) => (
        <h1>The mouse is at {x}, {y}</h1>
      )}
    />
  );
}
```

### Alternative Prop Names

The prop doesn't have to be called "render" - any prop that receives a function works:

```jsx
// File: src/render-prop-variants.jsx

import React, { useState } from 'react';

// Using 'children' as render prop (very common)
function Toggle({ children }) {
  const [on, setOn] = useState(false);
  
  return (
    <div>
      {children({ on, toggle: () => setOn(!on) })}
    </div>
  );
}

// Usage
function App() {
  return (
    <Toggle>
      {({ on, toggle }) => (
        <button onClick={toggle}>
          {on ? 'ON' : 'OFF'}
        </button>
      )}
    </Toggle>
  );
}

// Using 'component' prop
function List({ items, component: Component }) {
  return (
    <ul>
      {items.map((item, index) => (
        <Component key={index} item={item} />
      ))}
    </ul>
  );
}

// Using render prop
function ListWithRender({ items, renderItem }) {
  return (
    <ul>
      {items.map((item, index) => (
        <li key={index}>{renderItem(item)}</li>
      ))}
    </ul>
  );
}
```

### Building Reusable Hooks with Render Props

```jsx
// File: src/render-props-hooks.jsx

import React, { useState, useEffect } from 'react';

// Data fetching with render prop
function DataFetcher({ url, render }) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    async function fetch() {
      try {
        setLoading(true);
        const response = await fetch(url);
        const result = await response.json();
        setData(result);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    fetch();
  }, [url]);
  
  return render({ data, loading, error });
}

// Usage
function App() {
  return (
    <DataFetcher
      url="/api/users"
      render={({ data, loading, error }) => {
        if (loading) return <div>Loading...</div>;
        if (error) return <div>Error: {error}</div>;
        return (
          <ul>
            {data?.map(user => (
              <li key={user.id}>{user.name}</li>
            ))}
          </ul>
        );
      }}
    />
  );
}

// Window size with render prop
function WindowSize({ render }) {
  const [size, setSize] = useState({
    width: window.innerWidth,
    height: window.innerHeight
  });
  
  useEffect(() => {
    function handleResize() {
      setSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    }
    
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);
  
  return render(size);
}

// Usage
function App2() {
  return (
    <WindowSize
      render={({ width, height }) => (
        <p>Window is {width}x{height}</p>
      )}
    />
  );
}
```

### Implementing Common Patterns

```jsx
// File: src/render-props-patterns.jsx

import React, { useState, useEffect, useRef } from 'react';

// Toggle component using render prop
function Toggle({ children }) {
  const [on, setOn] = useState(false);
  
  return children({
    on,
    toggle: () => setOn(prev => !prev),
    setOn
  });
}

// Modal component using render prop
function Modal({ isOpen, render }) {
  if (!isOpen) return null;
  
  return (
    <div style={{
      position: 'fixed',
      top: 0,
      left: 0,
      right: 0,
      bottom: 0,
      backgroundColor: 'rgba(0,0,0,0.5)',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center'
    }}>
      <div style={{
        backgroundColor: 'white',
        padding: '20px',
        borderRadius: '8px',
        maxWidth: '400px'
      }}>
        {render({
          close: () => {} // Would be passed from parent
        })}
      </div>
    </div>
  );
}

// Accordion with render prop
function Accordion({ items, renderItem, renderContent }) {
  const [openIndex, setOpenIndex] = useState(-1);
  
  return (
    <div>
      {items.map((item, index) => (
        <div key={index}>
          {renderItem({
            item,
            isOpen: openIndex === index,
            onClick: () => setOpenIndex(openIndex === index ? -1 : index)
          })}
          {openIndex === index && renderContent({ item })}
        </div>
      ))}
    </div>
  );
}

// LocalStorage with render prop
function Storage({ name, initialValue, render }) {
  const [value, setValue] = useState(() => {
    const saved = localStorage.getItem(name);
    return saved ? JSON.parse(saved) : initialValue;
  });
  
  useEffect(() => {
    localStorage.setItem(name, JSON.stringify(value));
  }, [name, value]);
  
  return render({ value, setValue });
}

// Usage examples
function App() {
  return (
    <div>
      {/* Toggle */}
      <Toggle>
        {({ on, toggle }) => (
          <button onClick={toggle}>
            {on ? 'ON' : 'OFF'}
          </button>
        )}
      </Toggle>
      
      {/* Accordion */}
      <Accordion
        items={['Section 1', 'Section 2', 'Section 3']}
        renderItem={({ item, isOpen, onClick }) => (
          <button onClick={onClick}>
            {item} {isOpen ? '▲' : '▼'}
          </button>
        )}
        renderContent={({ item }) => (
          <p>Content for {item}</p>
        )}
      />
      
      {/* Storage */}
      <Storage
        name="theme"
        initialValue="light"
        render={({ value, setValue }) => (
          <button onClick={() => setValue(v => v === 'light' ? 'dark' : 'light')}>
            Theme: {value}
          </button>
        )}
      />
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Not Memoizing Render Functions

```jsx
// ❌ WRONG - New function created every render
function BadComponent() {
  return (
    <DataFetcher
      url="/api/data"
      render={({ data }) => <List data={data} />} // New function every render!
    />
  );
}

// ✅ CORRECT - Use useCallback or define outside
function GoodComponent() {
  const renderList = useCallback(({ data }) => (
    <List data={data} />
  ), []); // or dependencies
  
  return (
    <DataFetcher url="/api/data" render={renderList} />
  );
}
```

### Mistake 2: Confusing Render Props with Children

```jsx
// ❌ WRONG - Using children but expecting a function
function BadComponent({ children }) {
  return <div>{children}</div>; // children is treated as JSX, not function
}

// ✅ CORRECT - Use children as render prop when it's a function
function GoodComponent({ children }) {
  // If children is a function, call it
  return typeof children === 'function' 
    ? children({ someData }) 
    : <div>{children}</div>;
}
```

### Mistake 3: Not Using Hooks Instead

```jsx
// ❌ WRONG - Modern code should prefer hooks
function OldStyleFetcher({ url, render }) {
  const [data, setData] = useState(null);
  // ... fetch logic
  
  return render({ data });
}

// ✅ CORRECT - Use custom hooks in modern React
function useFetch(url) {
  const [data, setData] = useState(null);
  // ... fetch logic
  
  return data;
}

function ModernComponent() {
  const data = useFetch('/api/data');
  return <List data={data} />;
}
```

## Real-World Example

```jsx
// File: src/render-props/MouseTracker.jsx

import React, { useState } from 'react';

// Comprehensive mouse tracker with render prop
function MouseTracker({ render, showCursor = false }) {
  const [position, setPosition] = useState({ x: 0, y: 0 });
  const [isVisible, setIsVisible] = useState(true);
  
  const handleMouseMove = (event) => {
    setPosition({
      x: event.clientX,
      y: event.clientY
    });
  };
  
  const handleMouseEnter = () => setIsVisible(true);
  const handleMouseLeave = () => setIsVisible(false);
  
  // Provide comprehensive state to render prop
  const state = {
    position,
    isVisible,
    // Computed values
    relativeX: position.x / window.innerWidth,
    relativeY: position.y / window.innerHeight
  };
  
  return (
    <div 
      style={{ 
        width: '100%', 
        height: '100vh',
        cursor: showCursor ? 'crosshair' : 'default'
      }}
      onMouseMove={handleMouseMove}
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
    >
      {render(state)}
    </div>
  );
}

// Usage examples
function MouseFollower() {
  return (
    <MouseTracker
      render={({ position, isVisible }) => (
        isVisible && (
          <div style={{
            position: 'fixed',
            left: position.x + 10,
            top: position.y + 10,
            pointerEvents: 'none',
            backgroundColor: 'black',
            color: 'white',
            padding: '4px 8px',
            borderRadius: '4px',
            fontSize: '12px'
          }}>
            {position.x}, {position.y}
          </div>
        )
      )}
    />
  );
}

function QuadrantDetector() {
  return (
    <MouseTracker
      render={({ relativeX, relativeY }) => {
        const quadrant = 
          relativeX < 0.5 && relativeY < 0.5 ? 'Top Left' :
          relativeX >= 0.5 && relativeY < 0.5 ? 'Top Right' :
          relativeX < 0.5 && relativeY >= 0.5 ? 'Bottom Left' :
          'Bottom Right';
        
        return (
          <h1 style={{ 
            position: 'fixed',
            top: '50%',
            left: '50%',
            transform: 'translate(-50%, -50%)',
            fontSize: '48px'
          }}>
            {quadrant}
          </h1>
        );
      }}
    />
  );
}

function CoordinateGrid() {
  return (
    <MouseTracker
      showCursor
      render={({ position, relativeX, relativeY }) => (
        <svg style={{ width: '100%', height: '100%' }}>
          {/* Grid lines */}
          <line x1="50%" y1="0" x2="50%" y2="100%" stroke="#ddd" strokeWidth="2" />
          <line x1="0" y1="50%" x2="100%" y2="50%" stroke="#ddd" strokeWidth="2" />
          
          {/* Current position indicator */}
          <circle 
            cx={`${relativeX * 100}%`} 
            cy={`${relativeY * 100}%`} 
            r="10" 
            fill="red" 
          />
          
          {/* Coordinates text */}
          <text x="10" y="30" fill="#666">
            X: {position.x}, Y: {position.y}
          </text>
          <text x="10" y="50" fill="#666">
            Relative: {relativeX.toFixed(2)}, {relativeY.toFixed(2)}
          </text>
        </svg>
      )}
    />
  );
}

// Export all components
export { MouseTracker, MouseFollower, QuadrantDetector, CoordinateGrid };
export default MouseTracker;
```

```jsx
// File: src/render-props/UseCases.jsx

import React, { useState, useEffect } from 'react';

// Complete fetch data component
function FetchData({ url, render }) {
  const [state, setState] = useState({
    data: null,
    loading: true,
    error: null
  });
  
  useEffect(() => {
    let cancelled = false;
    
    async function fetch() {
      try {
        setState(s => ({ ...s, loading: true, error: null }));
        
        const response = await fetch(url);
        if (!response.ok) throw new Error(`HTTP ${response.status}`);
        
        const data = await response.json();
        
        if (!cancelled) {
          setState({ data, loading: false, error: null });
        }
      } catch (error) {
        if (!cancelled) {
          setState({ data: null, loading: false, error: error.message });
        }
      }
    }
    
    fetch();
    
    return () => { cancelled = true; };
  }, [url]);
  
  return render(state);
}

// Interval component
function Interval({ ms, render }) {
  const [elapsed, setElapsed] = useState(0);
  
  useEffect(() => {
    const id = setInterval(() => {
      setElapsed(e => e + 1);
    }, ms);
    
    return () => clearInterval(id);
  }, [ms]);
  
  return render(elapsed);
}

// Keyboard listener
function Keyboard({ render }) {
  const [keys, setKeys] = useState(new Set());
  
  useEffect(() => {
    function handleDown(e) {
      setKeys(prev => new Set([...prev, e.key]));
    }
    
    function handleUp(e) {
      setKeys(prev => {
        const next = new Set(prev);
        next.delete(e.key);
        return next;
      });
    }
    
    window.addEventListener('keydown', handleDown);
    window.addEventListener('keyup', handleUp);
    
    return () => {
      window.removeEventListener('keydown', handleDown);
      window.removeEventListener('keyup', handleUp);
    };
  }, []);
  
  return render({
    keys,
    hasKey: (key) => keys.has(key),
    isCtrlPressed: keys.has('Control'),
    isShiftPressed: keys.has('Shift'),
    isAltPressed: keys.has('Alt')
  });
}

// Example usage in App
function App() {
  return (
    <div>
      <h2>Data Fetching</h2>
      <FetchData
        url="https://jsonplaceholder.typicode.com/users/1"
        render={({ data, loading, error }) => (
          loading ? <p>Loading...</p> :
          error ? <p>Error: {error}</p> :
          <p>{data?.name}</p>
        )}
      />
      
      <h2>Timer</h2>
      <Interval ms={1000} render={(count) => (
        <p>Seconds: {count}</p>
      )} />
      
      <h2>Keyboard</h2>
      <Keyboard render={({ keys, isCtrlPressed }) => (
        <p>
          Keys pressed: {[...keys].join(', ')} <br />
          Ctrl pressed: {isCtrlPressed ? 'Yes' : 'No'}
        </p>
      )} />
    </div>
  );
}

export default App;
```

## Key Takeaways

- Render props use a function prop to determine what to render
- The prop doesn't have to be called "render" - children can also work
- Provides flexibility in how components render content
- Works well for cross-cutting concerns (data fetching, mouse tracking, etc.)
- Modern React prefers custom hooks over render props for shared logic
- Can cause performance issues if render function isn't memoized

## What's Next

Now that you've completed the components section, let's dive into React Hooks - the modern way to add state and other features to functional components. We'll start with the core hooks: useState, useEffect, and useRef.
