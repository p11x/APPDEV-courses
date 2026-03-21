# ARIA Roles and Attributes

## Overview
ARIA (Accessible Rich Internet Applications) provides semantic information to assistive technologies when HTML alone isn't sufficient. While semantic HTML should always be the first choice, ARIA fills the gaps for complex interactive components. This guide covers the five rules of ARIA and how to use ARIA attributes correctly in React applications.

## Prerequisites
- Understanding of semantic HTML
- Familiarity with WCAG guidelines
- Knowledge of React components

## Core Concepts

### The Five Rules of ARIA

**Rule 1:** If a native HTML element has the semantics you need, use it instead of ARIA.

```tsx
// ❌ WRONG - Adding ARIA to native element
<button role="button">Submit</button>

// ✅ CORRECT - Use native element
<button>Submit</button>
```

**Rule 2:** Don't change native HTML semantics unless you have to.

```tsx
// ❌ WRONG - Changing semantics
<h1 role="button">Click me</h1>

// ✅ CORRECT - Use proper element
<button><h1>Click me</h1></button>
```

**Rule 3:** All interactive ARIA controls must be keyboard accessible.

```tsx
// ❌ WRONG - Non-keyboard accessible
<div role="button" tabIndex={0} onClick={handleClick}>Click</div>

// ✅ CORRECT - Native button is keyboard accessible
<button onClick={handleClick}>Click</button>
```

**Rule 4:** Don't use `role="presentation"` or `aria-hidden=""` on focusable elements.

```tsx
// ❌ WRONG - Hiding focusable elements
<button aria-hidden="true">Visible but hidden from AT</button>

// ✅ CORRECT - Don't hide visible focusable elements
<button>Visible button</button>
```

**Rule 5:** All interactive elements must have accessible names.

```tsx
// ❌ WRONG - No accessible name
<button aria-label=""></button>

// ✅ CORRECT - Has accessible name
<button aria-label="Close dialog">×</button>
```

### ARIA Roles

```tsx
// [File: app/components/RoleExamples.tsx]
// role="alert" - For important messages
function AlertMessage() {
  return (
    <div role="alert">
      Your session will expire in 5 minutes.
    </div>
  );
}

// role="dialog" - For modals
function Modal() {
  return (
    <div role="dialog" aria-modal="true" aria-labelledby="title">
      <h2 id="title">Confirm Action</h2>
      <p>Are you sure?</p>
    </div>
  );
}

// role="navigation" - For navigation regions
function Navigation() {
  return (
    <nav aria-label="Main">
      {/* Navigation items */}
    </nav>
  );
}

// role="search" - For search regions
function SearchForm() {
  return (
    <form role="search">
      <input type="search" aria-label="Search" />
    </form>
  );
}

// role="status" - For status messages
function StatusMessage() {
  return (
    <div role="status">
      Changes saved successfully.
    </div>
  );
}
```

### ARIA Attributes

```tsx
// [File: app/components/ARIAAttributes.tsx]
// aria-label - Provides accessible name
function LabeledButton() {
  return (
    <button aria-label="Close menu">
      ×
    </button>
  );
}

// aria-labelledby - References another element for label
function LabelledSection() {
  return (
    <section aria-labelledby="section-title">
      <h2 id="section-title">Section Title</h2>
      <p>Content...</p>
    </section>
  );
}

// aria-describedby - Provides additional description
function DescribedInput() {
  return (
    <div>
      <label htmlFor="password">Password</label>
      <input 
        id="password" 
        type="password"
        aria-describedby="password-hint"
      />
      <small id="password-hint">
        Must be at least 8 characters
      </small>
    </div>
  );
}

// aria-expanded - For collapsible elements
function Expandable() {
  const [isExpanded, setExpanded] = useState(false);
  
  return (
    <button
      aria-expanded={isExpanded}
      aria-controls="content"
      onClick={() => setExpanded(!isExpanded)}
    >
      Toggle
    </button>
    <div id="content" hidden={!isExpanded}>
      Collapsible content
    </div>
  );
}

// aria-hidden - Hide from assistive tech
function HiddenContent() {
  return (
    <div aria-hidden={true}>
      {/* Decorative elements only */}
    </div>
  );
}

// aria-live - For dynamic content updates
function LiveRegion() {
  const [message, setMessage] = useState('');
  
  return (
    <div aria-live="polite" aria-atomic="true">
      {message}
    </div>
  );
}
```

### Accessible Interactive Components

```tsx
// [File: app/components/AccessibleAccordion.tsx]
'use client';

import { useState } from 'react';

interface AccordionItemProps {
  title: string;
  children: React.ReactNode;
}

function AccordionItem({ title, children }: AccordionItemProps) {
  const [isOpen, setIsOpen] = useState(false);
  
  return (
    <div>
      <button
        aria-expanded={isOpen}
        aria-controls={`panel-${title}`}
        onClick={() => setIsOpen(!isOpen)}
      >
        {title}
        <span aria-hidden={true}>{isOpen ? '−' : '+'}</span>
      </button>
      <div
        id={`panel-${title}`}
        role="region"
        aria-labelledby={`button-${title}`}
        hidden={!isOpen}
      >
        {children}
      </div>
    </div>
  );
}

export default function Accordion({ items }: { items: { title: string; content: string }[] }) {
  return (
    <div role="accordion">
      {items.map((item, index) => (
        <AccordionItem 
          key={index} 
          title={item.title}
        >
          {item.content}
        </AccordionItem>
      ))}
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Overusing ARIA
```tsx
// ❌ WRONG - ARIA everywhere
<div role="button" tabIndex={0} onClick={f}>Click</div>

// ✅ CORRECT - Native HTML
<button onClick={f}>Click</button>
```

### Mistake 2: Missing Labels
```tsx
// ❌ WRONG - No accessible name
<input type="text" />

// ✅ CORRECT - Has label
<input type="text" aria-label="Name" />
// OR
<label>Name <input type="text" /></label>
```

### Mistake 3: Incorrect aria-expanded
```tsx
// ❌ WRONG - Wrong value type
<button aria-expanded="true"> // string instead of boolean

// ✅ CORRECT - Boolean value
<button aria-expanded={true}>
```

## Real-World Example

Complete accessible tabs component:

```tsx
// [File: app/components/AccessibleTabs.tsx]
'use client';

import { useState } from 'react';

interface Tab {
  id: string;
  label: string;
  content: React.ReactNode;
}

export default function AccessibleTabs({ tabs }: { tabs: Tab[] }) {
  const [activeTab, setActiveTab] = useState(tabs[0]?.id);
  
  return (
    <div role="tablist" aria-label="Content tabs">
      {tabs.map((tab) => (
        <button
          key={tab.id}
          role="tab"
          aria-selected={activeTab === tab.id}
          aria-controls={`panel-${tab.id}`}
          id={`tab-${tab.id}`}
          onClick={() => setActiveTab(tab.id)}
        >
          {tab.label}
        </button>
      ))}
      
      {tabs.map((tab) => (
        <div
          key={`panel-${tab.id}`}
          role="tabpanel"
          id={`panel-${tab.id}`}
          aria-labelledby={`tab-${tab.id}`}
          hidden={activeTab !== tab.id}
        >
          {tab.content}
        </div>
      ))}
    </div>
  );
}
```

## Key Takeaways
- Use native HTML elements first before ARIA
- All interactive ARIA elements must be keyboard accessible
- Every interactive element needs an accessible name
- Use aria-label, aria-labelledby, aria-describedby appropriately
- Use aria-expanded for collapsible elements
- Use aria-live for dynamic content updates
- Don't use aria-hidden on focusable elements

## What's Next
This completes the Accessibility module. Continue to [CSS Transitions in React](15-animations/01-css-animations/01-css-transitions-in-react.md) to learn about creating smooth animations in React.