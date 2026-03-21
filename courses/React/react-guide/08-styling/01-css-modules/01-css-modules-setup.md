# CSS Modules Setup Guide

## Overview

CSS Modules are a CSS file format that solves one of the most frustrating problems in frontend development: global style conflicts. By scoping styles locally by default, CSS Modules prevent your styles from accidentally affecting other components. This guide covers what CSS Modules are, how to set them up with Vite, and how to use them effectively in your React applications.

## Prerequisites

- Basic understanding of CSS (selectors, properties, cascading)
- Familiarity with React component structure
- A React project set up with Vite (recommended) or Webpack

## Core Concepts

### Understanding CSS Modules

CSS Modules work by transforming your CSS class names at build time. Instead of using class names directly, each class gets a unique generated name, effectively scoping the styles to the component that imports them.

```css
/* File: src/components/Button.module.css */

/* This class is scoped to this module only */
.button {
  background-color: #3b82f6;
  color: white;
  padding: 0.75rem 1.5rem;
  border-radius: 0.375rem;
  border: none;
  cursor: pointer;
  font-weight: 600;
  transition: background-color 0.2s;
}

/* This class can be composed from other classes */
.primary {
  composes: button;
  background-color: #2563eb;
}

.secondary {
  composes: button;
  background-color: #6b7280;
}

/* The :hover pseudo-class works as expected */
.button:hover {
  background-color: #1d4ed8;
}

/* Modifier classes with BEM naming convention */
.button--large {
  padding: 1rem 2rem;
  font-size: 1.125rem;
}

.button--small {
  padding: 0.5rem 1rem;
  font-size: 0.875rem;
}
```

### Using CSS Modules in React Components

When you import a CSS Module, you get an object where keys are your class names and values are the generated unique class names.

```tsx
// File: src/components/Button.tsx

// Import the CSS Module - Vite/Webpack automatically handles .module.css files
// The import returns an object with your class names as keys
import styles from './Button.module.css';

// Define Button props using TypeScript interface
interface ButtonProps {
  // The button label/text
  children: React.ReactNode;
  // Button variant: primary (blue) or secondary (gray)
  variant?: 'primary' | 'secondary';
  // Size variant: small, medium, or large
  size?: 'small' | 'medium' | 'large';
  // Click handler
  onClick?: () => void;
  // Whether the button is disabled
  disabled?: boolean;
  // Optional additional CSS classes (merged with module classes)
  className?: string;
  // Button type attribute
  type?: 'button' | 'submit' | 'reset';
}

function Button({
  children,
  variant = 'primary',
  size = 'medium',
  onClick,
  disabled = false,
  className = '',
  type = 'button'
}: ButtonProps) {
  // Build class name by combining variant and size modifiers
  // The actual generated class names might look like:
  // "Button_primary__xK3p2 Button_button__abc123"
  const buttonClasses = [
    styles.button,
    styles[variant], // styles.primary or styles.secondary
    size !== 'medium' && styles[`button--${size}`], // conditional modifier
    className // additional classes passed from parent
  ]
    .filter(Boolean) // Remove falsy values
    .join(' '); // Join with spaces

  return (
    <button
      type={type}
      className={buttonClasses}
      onClick={onClick}
      disabled={disabled}
    >
      {children}
    </button>
  );
}

export default Button;
```

### Setting Up CSS Modules with Vite

Vite has built-in support for CSS Modules—no extra configuration is needed. The setup is automatic when you create a .module.css file.

```bash
# File: terminal

# Create a new Vite React TypeScript project
npm create vite@latest my-app -- --template react-ts

# Navigate to project directory
cd my-app

# Install dependencies
npm install

# Start development server
npm run dev

# Now you can create .module.css files directly!
```

Vite automatically detects `.module.css` files and processes them as CSS Modules. The generated class names use a format like `[filename]_[classname]__[hash]`, ensuring uniqueness.

### How CSS Modules Work Under the Hood

Understanding what happens during the build process helps debugging and reinforces why CSS Modules are effective.

```tsx
// File: src/components/Card.tsx

// When you import a CSS Module, you get an object like this:
// {
//   card: "Card_card__2k7t9",
//   title: "Card_title__3mNp5", 
//   content: "Card_content__7Qr1w",
//   footer: "Card_footer__8Kp2z",
//   highlighted: "Card_highlighted__9LmN4"
// }

import styles from './Card.module.css';

function Card({ title, children, highlighted = false }) {
  return (
    // The className combines base card styles with optional highlighted
    <div className={`${styles.card} ${highlighted ? styles.highlighted : ''}`}>
      <h2 className={styles.title}>{title}</h2>
      <div className={styles.content}>
        {children}
      </div>
    </div>
  );
}

export default Card;
```

```css
/* File: src/components/Card.module.css */

/* These styles only apply to elements with the generated class names
   The actual output might look like: .Card_card__2k7t9 */
.card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: box-shadow 0.2s ease;
}

.card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.title {
  font-size: 1.25rem;
  font-weight: 600;
  margin: 0;
  padding: 1rem;
  border-bottom: 1px solid #e5e7eb;
}

.content {
  padding: 1rem;
}

/* This highlighted class won't conflict with other components */
.highlighted {
  border: 2px solid #3b82f6;
  box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.2);
}
```

### Composing Styles with the composes Keyword

CSS Modules support style composition, allowing you to build reusable style combinations.

```css
/* File: src/components/Text.module.css */

/* Base text styles */
.base {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  line-height: 1.5;
}

/* Heading variants compose from base */
.heading {
  composes: base;
  font-weight: 700;
  margin: 0;
}

/* Body text composes from base */
.body {
  composes: base;
  font-weight: 400;
}

/* Large heading */
.headingLarge {
  composes: heading;
  font-size: 2rem;
}

/* Small heading */
.headingSmall {
  composes: heading;
  font-size: 1.25rem;
}
```

```tsx
// File: src/components/Text.tsx

import styles from './Text.module.css';

interface TextProps {
  children: React.ReactNode;
  variant?: 'headingLarge' | 'headingSmall' | 'body';
  className?: string;
}

function Text({ children, variant = 'body', className = '' }: TextProps) {
  // Dynamic variant selection
  const Component = variant.startsWith('heading') ? 'h2' : 'p';
  
  return (
    <Component className={`${styles[variant]} ${className}`}>
      {children}
    </Component>
  );
}

export default Text;
```

## Common Mistakes

### Mistake 1: Using Global Class Names

Avoid using global class names that might conflict with other components or third-party libraries.

```tsx
// ❌ WRONG - Using common class names causes conflicts
<div className="container">
  <h2 className="title">Hello</h2>
</div>

/* In another component */
.container { ... } /* Could override other containers! */

// ✅ CORRECT - Use module-scoped classes
<div className={styles.container}>
  <h2 className={styles.title}>Hello</h2>
</div>
```

### Mistake 2: Forgetting to Import Styles

Always import the CSS Module in your component. Without the import, the styles won't be applied.

```tsx
// ❌ WRONG - Missing import, styles won't work
import React from 'react';

function Alert({ message }) {
  return <div className="alert">{message}</div>;
}

// ✅ CORRECT - Import the module
import styles from './Alert.module.css';

function Alert({ message }) {
  return <div className={styles.alert}>{message}</div>;
}
```

### Mistake 3: Using Hyphens in Class Names Incorrectly

CSS Modules convert dashes in class names to camelCase in JavaScript.

```css
/* File: src/components/Input.module.css */

/* CSS uses kebab-case */
.input-field {
  padding: 0.5rem;
}

.button-primary {
  background: blue;
}
```

```tsx
// ❌ WRONG - Trying to use kebab-case in JavaScript won't work
<input className={styles.input-field} /> // undefined!

// ✅ CORRECT - Use camelCase in JavaScript
<input className={styles.inputField} />

// ✅ ALSO CORRECT - Use bracket notation for dynamic names
const fieldName = 'inputField';
<input className={styles[fieldName]} />
```

## Real-World Example

Here's a complete Card component with multiple variants, using CSS Modules for scoped styling:

```tsx
// File: src/components/Card/Card.module.css

/* Base card styles */
.card {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: all 0.2s ease;
}

.card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

/* Card variants - outline */
.cardOutline {
  composes: card;
  box-shadow: none;
  border: 2px solid #e5e7eb;
}

.cardOutline:hover {
  border-color: #3b82f6;
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.15);
}

/* Card variants - elevated */
.cardElevated {
  composes: card;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 2px 4px rgba(0, 0, 0, 0.06);
}

.cardElevated:hover {
  box-shadow: 0 10px 15px rgba(0, 0, 0, 0.1), 0 4px 6px rgba(0, 0, 0, 0.05);
}

/* Card sections */
.header {
  padding: 1.25rem;
  border-bottom: 1px solid #e5e7eb;
}

.body {
  padding: 1.25rem;
}

.footer {
  padding: 1rem 1.25rem;
  background: #f9fafb;
  border-top: 1px solid #e5e7eb;
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
}

/* Typography */
.title {
  font-size: 1.125rem;
  font-weight: 600;
  color: #111827;
  margin: 0;
}

.subtitle {
  font-size: 0.875rem;
  color: #6b7280;
  margin: 0.25rem 0 0 0;
}

.description {
  font-size: 0.9375rem;
  color: #4b5563;
  line-height: 1.6;
  margin: 0;
}

/* Interactive states */
.clickable {
  cursor: pointer;
}

.clickable:active {
  transform: scale(0.98);
}

/* Padding sizes */
.paddingSmall {
  padding: 0.75rem;
}

.paddingMedium {
  padding: 1.25rem;
}

.paddingLarge {
  padding: 2rem;
}
```

```tsx
// File: src/components/Card/Card.tsx

import React from 'react';
import styles from './Card.module.css';

// Define component props
interface CardProps {
  // Card content
  children?: React.ReactNode;
  // Card title (shown in header)
  title?: string;
  // Subtitle under the title
  subtitle?: string;
  // Main content (shown in body)
  description?: string;
  // Visual variant
  variant?: 'default' | 'outline' | 'elevated';
  // Padding size
  padding?: 'small' | 'medium' | 'large';
  // Make the card clickable
  onClick?: () => void;
  // Footer content (actions)
  footer?: React.ReactNode;
  // Additional CSS class
  className?: string;
}

function Card({
  children,
  title,
  subtitle,
  description,
  variant = 'default',
  padding = 'medium',
  onClick,
  footer,
  className = ''
}: CardProps) {
  // Build the class name based on variant
  // CSS Modules compose allows us to combine base styles with variants
  const cardClasses = [
    styles.card,
    variant !== 'default' && styles[`card${variant.charAt(0).toUpperCase() + variant.slice(1)}`],
    onClick && styles.clickable,
    className
  ]
    .filter(Boolean)
    .join(' ');

  // Build padding class
  const paddingClass = padding !== 'medium' 
    ? styles[`padding${padding.charAt(0).toUpperCase() + padding.slice(1)}`]
    : styles.paddingMedium;

  return (
    <div 
      className={`${cardClasses} ${paddingClass}`}
      onClick={onClick}
      role={onClick ? 'button' : undefined}
      tabIndex={onClick ? 0 : undefined}
      onKeyDown={onClick ? (e) => {
        if (e.key === 'Enter' || e.key === ' ') {
          e.preventDefault();
          onClick();
        }
      } : undefined}
    >
      {/* Header section with title and subtitle */}
      {(title || subtitle) && (
        <div className={styles.header}>
          {title && <h3 className={styles.title}>{title}</h3>}
          {subtitle && <p className={styles.subtitle}>{subtitle}</p>}
        </div>
      )}

      {/* Body section with description or children */}
      <div className={styles.body}>
        {description && <p className={styles.description}>{description}</p>}
        {children}
      </div>

      {/* Footer section for actions */}
      {footer && (
        <div className={styles.footer}>
          {footer}
        </div>
      )}
    </div>
  );
}

export default Card;
```

```tsx
// File: src/App.tsx

import Card from './components/Card/Card';

function App() {
  return (
    <div style={{ display: 'flex', gap: '1rem', padding: '2rem', flexWrap: 'wrap' }}>
      {/* Default card */}
      <Card
        title="Welcome"
        description="This is a default card with standard styling."
      />

      {/* Outline variant */}
      <Card
        variant="outline"
        title="Outline Card"
        description="This card has a visible border instead of shadow."
      />

      {/* Elevated variant */}
      <Card
        variant="elevated"
        title="Elevated Card"
        description="This card has a stronger shadow for emphasis."
      />

      {/* Interactive card with click handler */}
      <Card
        title="Click Me"
        description="Click this card to trigger an action."
        onClick={() => alert('Card clicked!')}
      >
        <p>Custom content inside the card body.</p>
      </Card>

      {/* Card with footer actions */}
      <Card
        title="With Actions"
        description="This card has action buttons in the footer."
        footer={
          <>
            <button onClick={() => console.log('Cancel')}>Cancel</button>
            <button onClick={() => console.log('Confirm')}>Confirm</button>
          </>
        }
      />
    </div>
  );
}

export default App;
```

## Key Takeaways

- CSS Modules automatically scope styles locally, preventing global conflicts
- Create `.module.css` files and import them as JavaScript objects
- Class names in JavaScript use camelCase (e.g., `.button-primary` becomes `styles.buttonPrimary`)
- Use the `composes` keyword to reuse and extend styles
- Vite has built-in CSS Modules support—no extra configuration needed
- Generated class names include a hash to ensure uniqueness
- Combine classes using array filtering and joining

## What's Next

Continue to [Dynamic Class Names](/08-styling/01-css-modules/02-dynamic-class-names.md) to learn how to conditionally apply CSS classes, use the clsx library for cleaner class name logic, and handle complex class composition patterns.