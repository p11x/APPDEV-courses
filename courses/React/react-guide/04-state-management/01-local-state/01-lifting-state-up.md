# Lifting State Up in React

## Overview
When multiple components need to share and synchronize the same state data, React's recommended pattern is to "lift state up" to their closest common ancestor. This fundamental concept allows sibling components to communicate through shared parent state, making data flow predictable and debugging easier.

## Prerequisites
- Basic understanding of React components
- Knowledge of props and component hierarchy
- Familiarity with useState and useEffect hooks

## Core Concepts

### When to Lift State
You should lift state up when:
1. Multiple components need access to the same data
2. Two sibling components need to share state
3. You want a single source of truth for certain data

```jsx
// File: src/components/TemperatureCalculator.jsx

import React, { useState } from 'react';

// ❌ BEFORE: Each component has its own state (problematic)
// If we wanted to show both Celsius AND Fahrenheit, we'd have no way to sync them

// ✅ AFTER: Lift state to common parent
function TemperatureCalculator() {
  // State lifted to parent - single source of truth
  const [temperature, setTemperature] = useState('');
  const [scale, setScale] = useState('celsius');

  // Convert temperature based on scale
  const convert = (temp, toScale) => {
    if (toScale === 'celsius') {
      return ((temp - 32) * 5) / 9;
    }
    return (temp * 9) / 5 + 32;
  };

  // Handler passed down to children
  const handleTemperatureChange = (value) => {
    setTemperature(value);
  };

  const handleScaleChange = (newScale) => {
    setScale(newScale);
    // Also convert the temperature when switching scales
    if (temperature) {
      const converted = convert(temperature, newScale);
      setTemperature(Math.round(converted * 10) / 10);
    }
  };

  return (
    <div>
      <h1>Temperature Converter</h1>
      
      {/* Both children receive the same state and handlers */}
      <TemperatureInput
        scale={scale}
        temperature={temperature}
        onTemperatureChange={handleTemperatureChange}
      />
      
      <BoilingVerdict celsius={scale === 'celsius' ? temperature : convert(temperature, 'celsius')} />
      
      <ScaleSelector scale={scale} onScaleChange={handleScaleChange} />
    </div>
  );
}

function TemperatureInput({ scale, temperature, onTemperatureChange }) {
  const scaleLabel = scale === 'celsius' ? 'Celsius' : 'Fahrenheit';
  
  return (
    <fieldset>
      <legend>Enter temperature in {scaleLabel}:</legend>
      <input
        type="number"
        value={temperature}
        onChange={(e) => onTemperatureChange(parseFloat(e.target.value) || '')}
      />
    </fieldset>
  );
}

function ScaleSelector({ scale, onScaleChange }) {
  return (
    <div>
      <label>
        <input
          type="radio"
          name="scale"
          value="celsius"
          checked={scale === 'celsius'}
          onChange={() => onScaleChange('celsius')}
        />
        Celsius
      </label>
      <label>
        <input
          type="radio"
          name="scale"
          value="fahrenheit"
          checked={scale === 'fahrenheit'}
          onChange={() => onScaleChange('fahrenheit')}
        />
        Fahrenheit
      </label>
    </div>
  );
}

function BoilingVerdict({ celsius }) {
  if (celsius >= 100) {
    return <p>The water would boil.</p>;
  }
  if (celsius <= 0) {
    return <p>The water would freeze.</p>;
  }
  return <p>The water would not boil or freeze.</p>;
}

export default TemperatureCalculator;
```

### Data Flow with Lifted State
When state is lifted, data flows down (via props) and actions flow up (via callback functions).

```jsx
// File: src/components/ParentChildDataFlow.jsx

import React, { useState } from 'react';

// Parent component - owns the state
function ParentComponent() {
  // Single source of truth
  const [count, setCount] = useState(0);
  const [message, setMessage] = useState('');

  // Callback functions that children will call
  const handleIncrement = () => {
    setCount(prev => prev + 1);
  };

  const handleDecrement = () => {
    setCount(prev => prev - 1);
  };

  const handleMessageChange = (newMessage) => {
    setMessage(newMessage);
  };

  return (
    <div className="parent">
      <h2>Parent Component</h2>
      <p>Count: {count}</p>
      <p>Message: {message}</p>
      
      {/* Pass state and handlers down to children */}
      <CounterDisplay 
        count={count}
        onIncrement={handleIncrement}
        onDecrement={handleDecrement}
      />
      
      <MessageInput 
        message={message}
        onMessageChange={handleMessageChange}
      />
    </div>
  );
}

// Child component - receives state and handlers via props
function CounterDisplay({ count, onIncrement, onDecrement }) {
  return (
    <div className="child">
      <h3>Counter (Child)</h3>
      <p>Count from parent: {count}</p>
      <button onClick={onIncrement}>+</button>
      <button onClick={onDecrement}>-</button>
    </div>
  );
}

// Another child component - also receives state and handlers
function MessageInput({ message, onMessageChange }) {
  return (
    <div className="child">
      <h3>Message Input (Child)</h3>
      <input
        type="text"
        value={message}
        onChange={(e) => onMessageChange(e.target.value)}
        placeholder="Type a message..."
      />
    </div>
  );
}

export default ParentComponent;
```

### Alternative: Using Context for Deeply Nested Components
When components are deeply nested, passing props through many levels becomes cumbersome. Context provides an alternative.

```jsx
// File: src/context/ThemeContext.jsx

import React, { createContext, useContext, useState } from 'react';

// Create context
const ThemeContext = createContext();

// Provider component
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  const [fontSize, setFontSize] = useState(16);

  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };

  const value = {
    theme,
    fontSize,
    setFontSize,
    toggleTheme,
  };

  return (
    <ThemeContext.Provider value={value}>
      {children}
    </ThemeContext.Provider>
  );
}

// Custom hook for easy access
function useTheme() {
  const context = useContext(ThemeContext);
  if (!context) {
    throw new Error('useTheme must be used within ThemeProvider');
  }
  return context;
}

// Deeply nested component can access context directly
function DeeplyNestedButton() {
  const { theme, toggleTheme } = useTheme();

  return (
    <button 
      onClick={toggleTheme}
      style={{
        backgroundColor: theme === 'light' ? '#fff' : '#333',
        color: theme === 'light' ? '#333' : '#fff',
      }}
    >
      Toggle Theme
    </button>
  );
}

function App() {
  return (
    <ThemeProvider>
      <DeeplyNestedButton />
    </ThemeProvider>
  );
}

export { ThemeProvider, useTheme };
```

## Common Mistakes

### Mistake 1: Duplicating State in Multiple Components
Never maintain the same data in multiple components - leads to synchronization issues.

```jsx
// ❌ WRONG - Duplicate state causes bugs
function Parent() {
  const [name, setName] = useState('');
}

function Child1() {
  const [name, setName] = useState(''); // Duplicate!
  return <input value={name} onChange={e => setName(e.target.value)} />;
}

function Child2() {
  const [name, setName] = useState(''); // Duplicate!
  return <p>{name}</p>; // Won't update when Child1 changes!
}

// ✅ CORRECT - Lift state up
function Parent() {
  const [name, setName] = useState('');
  return (
    <>
      <Child1 name={name} onChange={setName} />
      <Child2 name={name} />
    </>
  );
}
```

### Mistake 2: Lifting State Too High
Don't lift state higher than necessary - keeps components tightly coupled.

```jsx
// ❌ WRONG - Lifting too high couples unrelated components
function App() {
  const [data, setData] = useState('');
  return <><Header data={data} /><Footer data={data} /></>;
}

// ✅ CORRECT - Lift to closest common ancestor
function Dashboard() {
  const [data, setData] = useState('');
  return <><Sidebar /><MainContent data={data} onChange={setData} /></>;
}
```

## Key Takeaways
- Lift state to the closest common ancestor of components that need to share data
- Pass state down as props, pass handlers up as callbacks
- Keep state as close to where it's used as possible
- Consider Context when props passing becomes excessive
- Single source of truth prevents synchronization bugs

## What's Next
Continue to [Controlled vs Uncontrolled Inputs](02-controlled-vs-uncontrolled-inputs.md) to understand the difference between these two approaches to form inputs.
