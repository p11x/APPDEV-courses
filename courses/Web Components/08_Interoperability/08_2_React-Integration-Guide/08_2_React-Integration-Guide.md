# React Integration Guide

## OVERVIEW

Integrating Web Components with React requires specific patterns for props, events, and refs. This guide covers React wrapper components and direct usage patterns.

## IMPLEMENTATION DETAILS

### React Wrapper Pattern

```javascript
// Wrapper component for useElement
import React, { useRef, useEffect } from 'react';

export function MyElement({ value, onChange, ...props }, ref) {
  const elementRef = useRef(null);
  
  useEffect(() => {
    const el = elementRef.current;
    if (!el) return;
    
    const handler = (e) => onChange?.(e.detail);
    el.addEventListener('change', handler);
    
    return () => el.removeEventListener('change', handler);
  }, [onChange]);
  
  // Forward ref
  useEffect(() => {
    if (ref) {
      ref.current = elementRef.current;
    }
  }, [ref]);
  
  return (
    <my-element
      ref={elementRef}
      value={value}
      {...props}
    />
  );
}

export const MyElementWrapper = React.forwardRef(MyElement);
```

### Usage in React

```jsx
import { MyElementWrapper } from './MyElement';

function App() {
  const [value, setValue] = useState('');
  
  return (
    <MyElementWrapper
      value={value}
      onChange={(detail) => setValue(detail.value)}
      variant="primary"
    />
  );
}
```

### Custom Events in React

```javascript
// In Web Component
class EventElement extends HTMLElement {
  connectedCallback() {
    this.shadowRoot.innerHTML = '<button>Click</button>';
    this.shadowRoot.querySelector('button').addEventListener('click', () => {
      this.dispatchEvent(new CustomEvent('custom-change', {
        bubbles: true,
        composed: true,
        detail: { value: 'test' }
      }));
    });
  }
}

// In React - use onCamelCase
function EventComponent() {
  return (
    <event-element
      onCustomChange={(e) => console.log(e.detail)}
    />
  );
}
```

## NEXT STEPS

Proceed to **08_Interoperability/08_3_Vue-Integration-Strategies**.