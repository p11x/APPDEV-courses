# Compound Components Pattern

## Overview

The compound components pattern is an advanced React pattern that allows you to create expressive and flexible APIs for complex components. It involves a parent component that manages state and child components that render different parts of the UI while implicitly accessing that state. This pattern is commonly used in UI libraries (like Radix UI, Headless UI) and is excellent for building reusable, feature-rich components like tabs, selects, accordions, and modals.

## Prerequisites

- Understanding of React hooks (useState, useContext)
- Knowledge of component composition patterns
- Familiarity with context API
- Understanding of props and children

## Core Concepts

### What are Compound Components?

Compound components are a set of components that work together to form a complete UI element, where the parent manages state and children render different parts while sharing that state implicitly.

```jsx
// File: src/compound-basics.jsx

import React, { createContext, useContext, useState } from 'react';

// 1. Create context to share state between parent and children
const ToggleContext = createContext();

// 2. Parent component manages state and provides context
function Toggle({ children }) {
  const [isOn, setIsOn] = useState(false);
  
  const toggle = () => setIsOn(prev => !prev);
  
  // Provider contains both state and actions
  return (
    <ToggleContext.Provider value={{ isOn, toggle }}>
      {children}
    </ToggleContext.Provider>
  );
}

// 3. Child components access context to get state/actions
function ToggleOn({ children }) {
  const { isOn } = useContext(ToggleContext);
  return isOn ? <>{children}</> : null;
}

function ToggleOff({ children }) {
  const { isOn } = useContext(ToggleContext);
  return !isOn ? <>{children}</> : null;
}

function ToggleButton({ ...props }) {
  const { isOn, toggle } = useContext(ToggleContext);
  
  return (
    <button onClick={toggle} {...props}>
      {isOn ? 'ON' : 'OFF'}
    </button>
  );
}

// 4. Attach children to parent for clean API
Toggle.On = ToggleOn;
Toggle.Off = ToggleOff;
Toggle.Button = ToggleButton;

// 5. Usage - expressive API
function App() {
  return (
    <Toggle>
      <div>Light is</div>
      <Toggle.On>
        <span>ON 💡</span>
      </Toggle.On>
      <Toggle.Off>
        <span>OFF 🌙</span>
      </Toggle.Off>
      <Toggle.Button />
    </Toggle>
  );
}
```

### Building a Tabs Component

Let's build a complete tabs component using the compound pattern:

```jsx
// File: src/compound-tabs.jsx

import React, { createContext, useContext, useState } from 'react';

// Context for sharing tab state
const TabsContext = createContext(null);

// Parent component
function Tabs({ children, defaultTab = 0, onChange }) {
  const [activeIndex, setActiveIndex] = useState(defaultTab);
  
  const selectTab = (index) => {
    setActiveIndex(index);
    onChange?.(index);
  };
  
  // Provide state and actions to children
  const value = {
    activeIndex,
    selectTab
  };
  
  return (
    <TabsContext.Provider value={value}>
      <div className="tabs">{children}</div>
    </TabsContext.Provider>
  );
}

// TabList - container for tab buttons
function TabList({ children }) {
  return <div className="tab-list" role="tablist">{children}</div>;
}

// Tab - individual tab button
function Tab({ children, index }) {
  const { activeIndex, selectTab } = useContext(TabsContext);
  const isActive = activeIndex === index;
  
  return (
    <button
      role="tab"
      aria-selected={isActive}
      className={`tab ${isActive ? 'active' : ''}`}
      onClick={() => selectTab(index)}
    >
      {children}
    </button>
  );
}

// TabPanel - content panel for each tab
function TabPanel({ children, index }) {
  const { activeIndex } = useContext(TabsContext);
  
  if (activeIndex !== index) return null;
  
  return (
    <div role="tabpanel" className="tab-panel">
      {children}
    </div>
  );
}

// Attach children to parent
Tabs.List = TabList;
Tabs.Tab = Tab;
Tabs.Panel = TabPanel;

// Usage
function App() {
  return (
    <Tabs defaultTab={0} onChange={(index) => console.log('Tab changed:', index)}>
      <Tabs.List>
        <Tabs.Tab index={0}>Home</Tabs.Tab>
        <Tabs.Tab index={1}>About</Tabs.Tab>
        <Tabs.Tab index={2}>Contact</Tabs.Tab>
      </Tabs.List>
      
      <Tabs.Panel index={0}>
        <h3>Welcome Home!</h3>
        <p>This is the home page content.</p>
      </Tabs.Panel>
      
      <Tabs.Panel index={1}>
        <h3>About Us</h3>
        <p>Learn more about our company.</p>
      </Tabs.Panel>
      
      <Tabs.Panel index={2}>
        <h3>Contact Us</h3>
        <p>Get in touch with us.</p>
      </Tabs.Panel>
    </Tabs>
  );
}
```

### Building an Accordion Component

```jsx
// File: src/compound-accordion.jsx

import React, { createContext, useContext, useState } from 'react';

const AccordionContext = createContext(null);

function Accordion({ children, allowMultiple = false }) {
  const [openItems, setOpenItems] = useState(new Set());
  
  const toggleItem = (index) => {
    setOpenItems(prev => {
      const newSet = new Set(prev);
      if (newSet.has(index)) {
        newSet.delete(index);
      } else {
        if (!allowMultiple) {
          newSet.clear();
        }
        newSet.add(index);
      }
      return newSet;
    });
  };
  
  const isOpen = (index) => openItems.has(index);
  
  return (
    <AccordionContext.Provider value={{ isOpen, toggleItem, allowMultiple }}>
      <div className="accordion">{children}</div>
    </AccordionContext.Provider>
  );
}

function AccordionItem({ children, index }) {
  return <div className="accordion-item">{children}</div>;
}

function AccordionHeader({ children, index }) {
  const { isOpen, toggleItem } = useContext(AccordionContext);
  const isItemOpen = isOpen(index);
  
  return (
    <button
      className={`accordion-header ${isItemOpen ? 'open' : ''}`}
      onClick={() => toggleItem(index)}
      aria-expanded={isItemOpen}
    >
      <span>{children}</span>
      <span className="accordion-icon">{isItemOpen ? '−' : '+'}</span>
    </button>
  );
}

function AccordionPanel({ children, index }) {
  const { isOpen } = useContext(AccordionContext);
  
  if (!isOpen(index)) return null;
  
  return <div className="accordion-panel">{children}</div>;
}

// Attach children
Accordion.Item = AccordionItem;
Accordion.Header = AccordionHeader;
Accordion.Panel = AccordionPanel;

// Usage
function App() {
  return (
    <Accordion allowMultiple>
      <Accordion.Item>
        <AccordionHeader index={0}>What is React?</AccordionHeader>
        <AccordionPanel index={0}>
          React is a JavaScript library for building user interfaces.
        </AccordionPanel>
      </Accordion.Item>
      
      <Accordion.Item>
        <AccordionHeader index={1}>What are hooks?</AccordionHeader>
        <AccordionPanel index={1}>
          Hooks are functions that let you use state and other React features.
        </AccordionPanel>
      </Accordion.Item>
      
      <Accordion.Item>
        <AccordionHeader index={2}>Why use components?</AccordionHeader>
        <AccordionPanel index={2}>
          Components let you split the UI into independent, reusable pieces.
        </AccordionPanel>
      </Accordion.Item>
    </Accordion>
  );
}
```

### Advanced: Context with Additional Features

```jsx
// File: src/advanced-compound.jsx

import React, { createContext, useContext, useState, useCallback } from 'react';

// More complex context with multiple values
const SelectContext = createContext(null);

function Select({ 
  children, 
  value, 
  onChange, 
  placeholder = 'Select an option...' 
}) {
  const [isOpen, setIsOpen] = useState(false);
  const [highlightedIndex, setHighlightedIndex] = useState(-1);
  
  const open = useCallback(() => setIsOpen(true), []);
  const close = useCallback(() => {
    setIsOpen(false);
    setHighlightedIndex(-1);
  }, []);
  
  const select = useCallback((optionValue) => {
    onChange?.(optionValue);
    close();
  }, [onChange, close]);
  
  const valueContext = {
    value,
    isOpen,
    highlightedIndex,
    placeholder,
    open,
    close,
    select,
    setHighlightedIndex
  };
  
  return (
    <SelectContext.Provider value={valueContext}>
      <div className="select">
        {children}
      </div>
    </SelectContext.Provider>
  );
}

function SelectTrigger({ children }) {
  const { value, isOpen, open, placeholder } = useContext(SelectContext);
  
  const selectedLabel = children?.find?.(c => c.props.value === value)?.props?.children 
    || placeholder;
  
  return (
    <button 
      className={`select-trigger ${isOpen ? 'open' : ''}`}
      onClick={open}
      type="button"
    >
      {selectedLabel}
      <span className="select-arrow">▼</span>
    </button>
  );
}

function SelectOptions({ children }) {
  const { isOpen, highlightedIndex, close } = useContext(SelectContext);
  
  if (!isOpen) return null;
  
  const childrenArray = React.Children.toArray(children);
  
  return (
    <div className="select-options">
      {childrenArray.map((child, index) => (
        React.cloneElement(child, { 
          index,
          isHighlighted: index === highlightedIndex,
          onClose: close
        })
      ))}
    </div>
  );
}

function SelectOption({ children, value, index, isHighlighted, onClose, onSelect }) {
  const { select, value: selectedValue } = useContext(SelectContext);
  const isSelected = selectedValue === value;
  
  return (
    <div
      className={`select-option ${isSelected ? 'selected' : ''} ${isHighlighted ? 'highlighted' : ''}`}
      onClick={() => {
        select(value);
        onClose?.();
      }}
      onMouseEnter={() => {
        // Would need to set highlighted index via context
      }}
    >
      {children}
    </div>
  );
}

// Attach to parent
Select.Trigger = SelectTrigger;
Select.Options = SelectOptions;
Select.Option = SelectOption;

// Usage
function App() {
  const [selected, setSelected] = useState('');
  
  return (
    <Select value={selected} onChange={setSelected}>
      <Select.Trigger />
      <Select.Options>
        <Select.Option value="apple">Apple</Select.Option>
        <Select.Option value="banana">Banana</Select.Option>
        <Select.Option value="cherry">Cherry</Select.Option>
      </Select.Options>
    </Select>
  );
}
```

## Common Mistakes

### Mistake 1: Not Providing Context Value Properly

```jsx
// ❌ WRONG - Creating new objects every render
function BadParent({ children }) {
  const [state, setState] = useState(false);
  
  return (
    <Context.Provider value={{ state, setState }}>
      {children}
    </Context.Provider>
  );
}

// ✅ CORRECT - Memoize context value to prevent unnecessary re-renders
function GoodParent({ children }) {
  const [state, setState] = useState(false);
  
  const value = useMemo(() => ({
    state,
    setState
  }), [state]); // Only recreate when state changes
  
  return (
    <Context.Provider value={value}>
      {children}
    </Context.Provider>
  );
}
```

### Mistake 2: Forgetting to Handle Missing Context

```jsx

// ❌ WRONG - No fallback when context is not provided
function BadChild() {
  const { value } = useContext(Context); // Could be undefined!
  return <div>{value}</div>;
}

// ✅ CORRECT - Provide default values in createContext
const Context = createContext({
  defaultValue: 'fallback'
});

function GoodChild() {
  const { value } = useContext(Context);
  return <div>{value}</div>;
}
```

### Mistake 3: Not Forwarding Refs

```jsx

// ❌ WRONG - Missing ref forwarding
function BadCompoundParent({ children }) {
  const [isOpen, setIsOpen] = useState(false);
  
  return (
    <Context.Provider value={{ isOpen, setIsOpen }}>
      <div>{children}</div>
    </Context.Provider>
  );
}

// ✅ CORRECT - Use forwardRef when needed
const CompoundParent = React.forwardRef(function CompoundParent({ children }, ref) {
  const [isOpen, setIsOpen] = useState(false);
  
  useImperativeHandle(ref, () => ({
    open: () => setIsOpen(true),
    close: () => setIsOpen(false),
    toggle: () => setIsOpen(prev => !prev)
  }));
  
  return (
    <Context.Provider value={{ isOpen, setIsOpen }}>
      <div>{children}</div>
    </Context.Provider>
  );
});
```

## Real-World Example

Let's build a complete Select dropdown using the compound pattern:

```jsx
// File: src/components/CompoundSelect.jsx

import React, { createContext, useContext, useState, useCallback, useMemo } from 'react';

// Create context with sensible defaults
const SelectContext = createContext({
  isOpen: false,
  selectedValue: '',
  options: [],
  onSelect: () => {},
  setOpen: () => {},
  registerOption: () => {},
  highlightedIndex: -1,
  setHighlightedIndex: () => {}
});

function Select({ 
  children, 
  value = '', 
  onChange, 
  placeholder = 'Select an option...' 
}) {
  const [isOpen, setIsOpen] = useState(false);
  const [highlightedIndex, setHighlightedIndex] = useState(-1);
  const [options, setOptions] = useState([]);
  
  // Register options from children
  const registerOption = useCallback((value, label) => {
    setOptions(prev => [...prev, { value, label }]);
  }, []);
  
  // Handle option selection
  const handleSelect = useCallback((optionValue) => {
    onChange?.(optionValue);
    setIsOpen(false);
    setHighlightedIndex(-1);
  }, [onChange]);
  
  // Toggle dropdown
  const toggleOpen = useCallback(() => {
    setIsOpen(prev => !prev);
  }, []);
  
  const valueContext = useMemo(() => ({
    isOpen,
    selectedValue: value,
    options,
    onSelect: handleSelect,
    setOpen: setIsOpen,
    registerOption,
    highlightedIndex,
    setHighlightedIndex
  }), [
    isOpen, 
    value, 
    options, 
    handleSelect, 
    registerOption,
    highlightedIndex
  ]);
  
  return (
    <SelectContext.Provider value={valueContext}>
      <div className="select-container">
        {children}
      </div>
    </SelectContext.Provider>
  );
}

function SelectTrigger({ children }) {
  const { isOpen, selectedValue, options, setOpen } = useContext(SelectContext);
  
  const selectedOption = options.find(opt => opt.value === selectedValue);
  const displayText = selectedOption?.label || placeholder;
  
  return (
    <button
      type="button"
      className={`select-trigger ${isOpen ? 'open' : ''}`}
      onClick={() => setOpen(prev => !prev)}
      aria-haspopup="listbox"
      aria-expanded={isOpen}
    >
      <span>{displayText}</span>
      <span className="select-arrow">{isOpen ? '▲' : '▼'}</span>
    </button>
  );
}

function SelectOptions({ children }) {
  const { isOpen, selectedValue, onSelect, highlightedIndex, setHighlightedIndex } = useContext(SelectContext);
  
  if (!isOpen) return null;
  
  const handleKeyDown = (e) => {
    const optionsCount = React.Children.count(children);
    
    if (e.key === 'ArrowDown') {
      e.preventDefault();
      setHighlightedIndex(prev => (prev + 1) % optionsCount);
    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      setHighlightedIndex(prev => (prev - 1 + optionsCount) % optionsCount);
    } else if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      if (highlightedIndex >= 0) {
        const option = React.Children.toArray(children)[highlightedIndex];
        onSelect?.(option.props.value);
      }
    } else if (e.key === 'Escape') {
      setHighlightedIndex(-1);
    }
  };
  
  return (
    <ul 
      className="select-options" 
      role="listbox"
      onKeyDown={handleKeyDown}
    >
      {React.Children.map(children, (child, index) => {
        return React.cloneElement(child, {
          isSelected: child.props.value === selectedValue,
          isHighlighted: index === highlightedIndex,
          index
        });
      })}
    </ul>
  );
}

function SelectOption({ children, value, isSelected, isHighlighted, index }) {
  const { onSelect, setOpen, setHighlightedIndex } = useContext(SelectContext);
  
  const handleClick = () => {
    onSelect?.(value);
  };
  
  const handleMouseEnter = () => {
    setHighlightedIndex(index);
  };
  
  return (
    <li
      className={`select-option ${isSelected ? 'selected' : ''} ${isHighlighted ? 'highlighted' : ''}`}
      role="option"
      aria-selected={isSelected}
      onClick={handleClick}
      onMouseEnter={handleMouseEnter}
    >
      {children}
      {isSelected && <span>✓</span>}
    </li>
  );
}

// Attach sub-components to main component
Select.Trigger = SelectTrigger;
Select.Options = SelectOptions;
Select.Option = SelectOption;

export default Select;
```

```jsx
// File: src/AppSelectDemo.jsx

import React, { useState } from 'react';
import Select from './components/CompoundSelect';

function App() {
  const [selectedCountry, setSelectedCountry] = useState('');
  const [selectedColor, setSelectedColor] = useState('');
  
  const styles = {
    container: {
      padding: '40px',
      fontFamily: 'Arial, sans-serif'
    },
    section: {
      marginBottom: '40px'
    }
  };
  
  return (
    <div style={styles.container}>
      <h1>Compound Select Demo</h1>
      
      <div style={styles.section}>
        <h3>Select Country</h3>
        <Select value={selectedCountry} onChange={setSelectedCountry}>
          <Select.Trigger />
          <Select.Options>
            <Select.Option value="us">United States</Select.Option>
            <Select.Option value="uk">United Kingdom</Select.Option>
            <Select.Option value="ca">Canada</Select.Option>
            <Select.Option value="au">Australia</Select.Option>
            <Select.Option value="de">Germany</Select.Option>
          </Select.Options>
        </Select>
        <p>Selected: {selectedCountry}</p>
      </div>
      
      <div style={styles.section}>
        <h3>Select Color</h3>
        <Select value={selectedColor} onChange={setSelectedColor}>
          <Select.Trigger />
          <Select.Options>
            <Select.Option value="red">🔴 Red</Select.Option>
            <Select.Option value="green">🟢 Green</Select.Option>
            <Select.Option value="blue">🔵 Blue</Select.Option>
            <Select.Option value="yellow">🟡 Yellow</Select.Option>
          </Select.Options>
        </Select>
        <p>Selected: {selectedColor}</p>
      </div>
    </div>
  );
}

export default App;
```

## Key Takeaways

- Compound components use React Context to share state between parent and children
- Parent component provides context with state and actions
- Child components consume context to access state and trigger actions
- This pattern creates expressive, declarative APIs
- Examples: Tabs, Accordion, Select, Modal, Dropdown
- Always memoize context values to prevent unnecessary re-renders
- Provide default values in createContext for better error messages

## What's Next

Now that you've learned compound components, let's explore different component patterns including presentational vs container components, higher-order components, and render props.
