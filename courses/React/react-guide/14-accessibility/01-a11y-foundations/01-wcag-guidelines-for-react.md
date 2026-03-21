# WCAG Guidelines for React

## Overview
Web Content Accessibility Guidelines (WCAG) provide a comprehensive framework for making web content accessible to people with disabilities. For React developers, understanding and implementing WCAG is essential for creating inclusive applications. This guide covers the key WCAG principles—Perceivable, Operable, Understandable, and Robust (POUR)—and how to implement them in React applications.

## Prerequisites
- Basic understanding of HTML and CSS
- Familiarity with React components
- Knowledge of semantic HTML

## Core Concepts

### The Four POUR Principles

**1. Perceivable** - Information must be presentable to users in ways they can perceive

```tsx
// [File: app/components/ImageWithAlt.tsx]
// ❌ WRONG - Missing alt text
<img src="chart.png" />

// ✅ CORRECT - Descriptive alt text
<img 
  src="chart.png" 
  alt="Bar chart showing sales growth: Q1: $10k, Q2: $15k, Q3: $25k, Q4: $30k" 
/>

// For decorative images, use empty alt
<img src="decoration.png" alt="" />

// ❌ WRONG - Unhelpful alt text
<img src="photo.jpg" alt="Photo of product" />

// ✅ CORRECT - Descriptive alt text
<img src="photo.jpg" alt="Red Nike Air Max running shoes with white sole" />
```

**2. Operable** - User interface components must be operable

```tsx
// [File: app/components/AccessibleButton.tsx]
// ❌ WRONG - Non-interactive element
<div onClick={handleClick}>Click me</div>

// ✅ CORRECT - Use proper button element
<button onClick={handleClick}>Click me</button>

// ❌ WRONG - Keyboard inaccessible
<button onClick={handleClick} tabIndex={-1}>Click me</button>

// ✅ CORRECT - Keyboard accessible with visible focus
<button onClick={handleClick}>Click me</button>
```

**3. Understandable** - Information and UI operation must be understandable

```tsx
// [File: app/components/FormWithLabels.tsx]
// ❌ WRONG - Missing labels
<input type="text" placeholder="Enter email" />

// ✅ CORRECT - Proper labels
<label htmlFor="email">Email</label>
<input id="email" type="text" placeholder="Enter email" />

// ✅ ALSO CORRECT - Label wrapping
<label>
  Email
  <input type="text" placeholder="Enter email" />
</label>
```

**4. Robust** - Content must be robust enough for various user agents

```tsx
// [File: app/components/ValidMarkup.tsx]
// ❌ WRONG - Using div for everything (div soup)
<div className="nav">
  <div className="nav-item">Home</div>
  <div className="nav-item">About</div>
</div>

// ✅ CORRECT - Semantic HTML
<nav>
  <ul>
    <li><a href="/">Home</a></li>
    <li><a href="/about">About</a></li>
  </ul>
</nav>
```

### WCAG 2.2 AA Requirements

```tsx
// [File: app/components/AccessibleColor.tsx]
// Color contrast - minimum 4.5:1 for normal text
// ❌ WRONG - Poor contrast
<p style={{ color: '#999', backgroundColor: '#fff' }}>Low contrast text</p>

// ✅ CORRECT - Good contrast
<p style={{ color: '#333', backgroundColor: '#fff' }}>Good contrast text</p>

// Large text (18pt+) needs 3:1 contrast ratio
<p style={{ color: '#555', backgroundColor: '#fff', fontSize: '24px' }}>
  Large text with adequate contrast
</p>
```

```tsx
// [File: app/components/FocusIndicator.tsx]
// Focus indicators - must be visible
// ❌ WRONG - No focus styles
<button style={{ outline: 'none' }}>Submit</button>

// ✅ CORRECT - Visible focus indicator
<button style={{ outline: '2px solid blue', outlineOffset: '2px' }}>
  Submit
</button>

// ✅ ALSO CORRECT - Using CSS
<button className="focus-visible">Submit</button>
/* CSS: */
.focus-visible:focus-visible {
  outline: 2px solid blue;
  outline-offset: 2px;
}
```

```tsx
// [File: app/components/ErrorMessages.tsx]
// Error identification - clear error messages
// ❌ WRONG - Vague errors
<input aria-label="Email" />
<span>Invalid</span>

// ✅ CORRECT - Specific errors with aria-describedby
<input 
  id="email"
  aria-describedby="email-error"
  aria-invalid="true"
/>
<span id="email-error" role="alert">
  Please enter a valid email address (e.g., user@example.com)
</span>
```

### Accessible Forms

```tsx
// [File: app/components/AccessibleForm.tsx]
export default function AccessibleForm() {
  return (
    <form>
      {/* Required fields indicated */}
      <label htmlFor="username">
        Username <span aria-label="required">*</span>
      </label>
      <input 
        id="username" 
        required 
        aria-required="true"
      />
      
      {/* Field with description */}
      <label htmlFor="password">Password</label>
      <input 
        id="password" 
        type="password"
        aria-describedby="password-hint"
      />
      <small id="password-hint">
        Must be at least 8 characters with one number
      </small>
      
      {/* Error states */}
      <label htmlFor="email">Email</label>
      <input 
        id="email"
        type="email"
        aria-invalid="true"
        aria-describedby="email-error"
      />
      <span id="email-error" role="alert" style={{ color: 'red' }}>
        Please enter a valid email address
      </span>
    </form>
  );
}
```

### Accessible Modals and Dialogs

```tsx
// [File: app/components/AccessibleModal.tsx]
import { useEffect, useRef } from 'react';

export default function AccessibleModal({ 
  isOpen, 
  onClose, 
  title, 
  children 
}) {
  const modalRef = useRef<HTMLDivElement>(null);
  const previousFocusRef = useRef<HTMLElement | null>(null);

  useEffect(() => {
    if (isOpen) {
      // Store previously focused element
      previousFocusRef.current = document.activeElement as HTMLElement;
      
      // Focus the modal
      modalRef.current?.focus();
      
      // Prevent body scroll
      document.body.style.overflow = 'hidden';
    } else {
      // Restore body scroll
      document.body.style.overflow = '';
      
      // Return focus to previous element
      previousFocusRef.current?.focus();
    }
  }, [isOpen]);

  // Handle escape key
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape' && isOpen) {
        onClose();
      }
    };

    document.addEventListener('keydown', handleKeyDown);
    return () => document.removeEventListener('keydown', handleKeyDown);
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  return (
    <div 
      role="dialog"
      aria-modal="true"
      aria-labelledby="modal-title"
      ref={modalRef}
      tabIndex={-1}
    >
      <h2 id="modal-title">{title}</h2>
      <button onClick={onClose} aria-label="Close">
        ×
      </button>
      {children}
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Using Placeholder as Label
```tsx
// ❌ WRONG - Placeholder is not a substitute for label
<input placeholder="Enter your email" />

// ✅ CORRECT - Always use labels
<label htmlFor="email">Email</label>
<input id="email" placeholder="e.g., john@example.com" />
```

### Mistake 2: Missing Focus Management
```tsx
// ❌ WRONG - Opening modal without managing focus
function Modal({ isOpen, onClose }) {
  if (!isOpen) return null;
  return <div role="dialog">{children}</div>;
}

// ✅ CORRECT - Manage focus properly
function Modal({ isOpen, onClose }) {
  // Use useEffect to manage focus as shown in the example above
}
```

### Mistake 3: Not Providing Skip Links
```tsx
// ❌ WRONG - No way to skip navigation
function Layout({ children }) {
  return (
    <nav>...</nav>
    <main>{children}</main>
  );
}

// ✅ CORRECT - Add skip link
function Layout({ children }) {
  return (
    <>
      <a href="#main-content" className="skip-link">
        Skip to main content
      </a>
      <nav>...</nav>
      <main id="main-content">{children}</main>
    </>
  );
}
```

## Real-World Example

Complete accessible React component:

```tsx
// [File: app/components/AccessibleProductCard.tsx]
interface ProductProps {
  name: string;
  description: string;
  price: number;
  image: string;
  onAddToCart: () => void;
}

export default function AccessibleProductCard({
  name,
  description,
  price,
  image,
  onAddToCart,
}: ProductProps) {
  return (
    <article className="product-card">
      {/* Decorative image - empty alt */}
      <img 
        src={image} 
        alt=""
        className="product-image"
      />
      
      <div className="product-content">
        {/* Semantic heading structure */}
        <h3 className="product-name">{name}</h3>
        
        <p className="product-description">{description}</p>
        
        <p className="product-price">
          {/* Screen reader announcement for price */}
          <span className="sr-only">Price:</span> 
          ${price.toFixed(2)}
        </p>
        
        {/* Accessible button with proper labeling */}
        <button 
          onClick={onAddToCart}
          className="add-to-cart-btn"
          aria-label={`Add ${name} to cart for $${price.toFixed(2)}`}
        >
          Add to Cart
        </button>
      </div>
    </article>
  );
}
```

## Key Takeaways
- Follow WCAG's four principles: Perceivable, Operable, Understandable, Robust
- Always provide alt text for images (descriptive or empty for decorative)
- Use semantic HTML elements (button, nav, main, article, etc.)
- Ensure keyboard accessibility - all interactive elements must be focusable
- Provide visible focus indicators
- Use proper form labels - never rely on placeholders
- Implement proper focus management for modals and dialogs
- Ensure sufficient color contrast (4.5:1 for text, 3:1 for large text)
- Use ARIA attributes only when HTML can't express the semantics

## What's Next
Continue to [Semantic HTML in React](02-semantic-html-in-react.md) to learn how to use semantic HTML elements effectively in React applications.