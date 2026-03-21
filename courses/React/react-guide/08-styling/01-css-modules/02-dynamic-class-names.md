# Dynamic Class Names with CSS Modules

## Overview

In real-world React applications, you rarely apply static CSS classes. Instead, you need to conditionally apply classes based on props, state, or conditions. This guide covers using template literals, the clsx library, and handling complex class composition patterns with CSS Modules.

## Prerequisites

- Completed CSS Modules Setup guide
- Understanding of React props and state
- Familiarity with TypeScript generics (optional but recommended)

## Core Concepts

### Template Literals for Dynamic Classes

The simplest approach to combining CSS Module classes uses JavaScript template literals.

```tsx
// File: src/components/Badge/Badge.tsx

import styles from './Badge.module.css';

interface BadgeProps {
  /** Badge content */
  children: React.ReactNode;
  /** Visual style variant */
  variant?: 'default' | 'success' | 'warning' | 'error' | 'info';
  /** Size of the badge */
  size?: 'small' | 'medium' | 'large';
  /** Whether to show a dot indicator */
  dot?: boolean;
  /** Additional CSS classes */
  className?: string;
}

function Badge({
  children,
  variant = 'default',
  size = 'medium',
  dot = false,
  className = ''
}: BadgeProps) {
  // Combine multiple class names using template literals
  // Each condition adds its class if truthy
  const badgeClasses = `
    ${styles.badge}
    ${styles[`badge--${variant}`]}
    ${styles[`badge--${size}`]}
    ${dot ? styles['badge--with-dot'] : ''}
    ${className}
  `.trim().replace(/\s+/g, ' '); // Clean up extra spaces

  return (
    <span className={badgeClasses}>
      {dot && <span className={styles.dot} />}
      {children}
    </span>
  );
}

export default Badge;
```

```css
/* File: src/components/Badge/Badge.module.css */

/* Base badge styles */
.badge {
  display: inline-flex;
  align-items: center;
  gap: 0.375rem;
  font-weight: 500;
  border-radius: 9999px;
  white-space: nowrap;
}

/* Size variants */
.badge--small {
  padding: 0.125rem 0.5rem;
  font-size: 0.6875rem;
}

.badge--medium {
  padding: 0.25rem 0.75rem;
  font-size: 0.75rem;
}

.badge--large {
  padding: 0.375rem 1rem;
  font-size: 0.875rem;
}

/* Color variants */
.badge--default {
  background-color: #f3f4f6;
  color: #374151;
}

.badge--success {
  background-color: #d1fae5;
  color: #065f46;
}

.badge--warning {
  background-color: #fef3c7;
  color: #92400e;
}

.badge--error {
  background-color: #fee2e2;
  color: #991b1b;
}

.badge--info {
  background-color: #dbeafe;
  color: #1e40af;
}

/* Dot indicator */
.dot {
  width: 0.375rem;
  height: 0.375rem;
  border-radius: 50%;
  background-color: currentColor;
}

.badge--with-dot {
  padding-left: 0.5rem;
}
```

### Using the clsx Library

The clsx library provides a cleaner API for conditional class names, especially useful when dealing with multiple conditions.

```bash
# File: terminal

# Install clsx for conditional class names
npm install clsx

# Install type definitions (TypeScript)
npm install @types/clsx --save-dev
```

```tsx
// File: src/components/Button/Button.tsx

// clsx creates a conditional className string efficiently
import clsx from 'clsx';
import styles from './Button.module.css';

interface ButtonProps {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost';
  size?: 'sm' | 'md' | 'lg';
  fullWidth?: boolean;
  disabled?: boolean;
  loading?: boolean;
  onClick?: () => void;
  type?: 'button' | 'submit' | 'reset';
  className?: string;
}

function Button({
  children,
  variant = 'primary',
  size = 'md',
  fullWidth = false,
  disabled = false,
  loading = false,
  onClick,
  type = 'button',
  className = ''
}: ButtonProps) {
  // clsx handles conditional classes elegantly
  // It accepts strings, objects, and arrays
  const buttonClasses = clsx(
    // Base button class
    styles.button,
    // Variant class - maps directly to CSS class
    styles[`button--${variant}`],
    // Size class
    styles[`button--${size}`],
    // Conditional classes - truthy values are included, falsy are excluded
    {
      [styles['button--full-width']]: fullWidth,
      [styles['button--disabled']]: disabled,
      [styles['button--loading']]: loading
    },
    // Additional classes passed as props
    className
  );

  return (
    <button
      type={type}
      className={buttonClasses}
      onClick={onClick}
      disabled={disabled || loading}
    >
      {loading && <span className={styles.spinner} />}
      <span className={loading ? styles['button-text--hidden'] : ''}>
        {children}
      </span>
    </button>
  );
}

export default Button;
```

```css
/* File: src/components/Button/Button.module.css */

.button {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  font-weight: 600;
  border-radius: 0.5rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  position: relative;
}

.button:focus-visible {
  outline: 2px solid #3b82f6;
  outline-offset: 2px;
}

/* Variants */
.button--primary {
  background-color: #3b82f6;
  color: white;
}

.button--primary:hover:not(:disabled) {
  background-color: #2563eb;
}

.button--secondary {
  background-color: #6b7280;
  color: white;
}

.button--secondary:hover:not(:disabled) {
  background-color: #4b5563;
}

.button--outline {
  background-color: transparent;
  border: 2px solid #3b82f6;
  color: #3b82f6;
}

.button--outline:hover:not(:disabled) {
  background-color: #eff6ff;
}

.button--ghost {
  background-color: transparent;
  color: #374151;
}

.button--ghost:hover:not(:disabled) {
  background-color: #f3f4f6;
}

/* Sizes */
.button--sm {
  padding: 0.375rem 0.75rem;
  font-size: 0.875rem;
}

.button--md {
  padding: 0.5rem 1rem;
  font-size: 1rem;
}

.button--lg {
  padding: 0.75rem 1.5rem;
  font-size: 1.125rem;
}

/* Modifiers */
.button--full-width {
  width: 100%;
}

.button--disabled,
.button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.button--loading {
  cursor: wait;
}

/* Loading spinner */
.spinner {
  width: 1rem;
  height: 1rem;
  border: 2px solid currentColor;
  border-top-color: transparent;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.button-text--hidden {
  opacity: 0;
}
```

### Combining Multiple Module Classes

Sometimes you need to combine classes from multiple CSS Modules in a single component.

```tsx
// File: src/components/Menu/MenuItem.tsx

import clsx from 'clsx';
import styles from './Menu.module.css';

interface MenuItemProps {
  children: React.ReactNode;
  icon?: React.ReactNode;
  active?: boolean;
  disabled?: boolean;
  onClick?: () => void;
  className?: string;
}

function MenuItem({
  children,
  icon,
  active = false,
  disabled = false,
  onClick,
  className = ''
}: MenuItemProps) {
  return (
    <button
      type="button"
      // Combine classes from styles and shared utility classes
      className={clsx(
        styles.menuItem,
        {
          [styles['menuItem--active']]: active,
          [styles['menuItem--disabled']]: disabled
        },
        className
      )}
      onClick={onClick}
      disabled={disabled}
      role="menuitem"
      aria-selected={active}
    >
      {icon && <span className={styles.icon}>{icon}</span>}
      <span className={styles.label}>{children}</span>
    </button>
  );
}

export default MenuItem;
```

### Creating a Class Name Utility

For larger applications, create a utility function to standardize class name handling.

```tsx
// File: src/utils/cn.ts

import clsx, { type ClassValue } from 'clsx';

/**
 * Combines CSS Module classes with regular classes
 * Use this instead of clsx directly for consistent behavior
 * 
 * @param inputs - Class names, objects, or arrays to combine
 * @returns A single className string
 */
export function cn(...inputs: ClassValue[]): string {
  return clsx(inputs);
}

// Usage in components:
// import { cn } from '../utils/cn';
// <div className={cn(styles.card, isActive && styles.active)} />
```

## Common Mistakes

### Mistake 1: Using Non-Existent Class Names

Always ensure the CSS class exists in your module file.

```tsx
// ❌ WRONG - Class doesn't exist, returns undefined
<div className={styles.nonexistent} />

// ✅ CORRECT - Reference existing classes
<div className={styles.card} />
```

### Mistake 2: Not Handling Undefined Values

Filter out undefined values to avoid "undefined" appearing in className.

```tsx
// ❌ WRONG - undefined appears in class list
<div className={`${styles.card} ${styles[variant]}`} />

// ✅ CORRECT - Filter or use clsx
<div className={clsx(styles.card, variant && styles[variant])} />
```

### Mistake 3: Hardcoding Class Names Instead of Using Modules

Keep styles scoped by always using the module object.

```tsx
// ❌ WRONG - Using plain string breaks scoping
<div className="card active">...</div>

// ✅ CORRECT - Always reference styles object
<div className={clsx(styles.card, styles.active)}>...</div>
```

## Real-World Example

Here's a complete input component with dynamic classes for validation states, focus, and disabled conditions:

```tsx
// File: src/components/Input/Input.module.css

.inputWrapper {
  display: flex;
  flex-direction: column;
  gap: 0.375rem;
}

.label {
  font-size: 0.875rem;
  font-weight: 500;
  color: #374151;
}

.required {
  color: #dc2626;
  margin-left: 0.25rem;
}

.inputContainer {
  position: relative;
  display: flex;
  align-items: center;
}

.input {
  width: 100%;
  padding: 0.625rem 0.875rem;
  font-size: 0.9375rem;
  border: 1px solid #d1d5db;
  border-radius: 0.5rem;
  background: white;
  transition: all 0.15s ease;
}

.input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

/* States */
.input--error {
  border-color: #dc2626;
}

.input--error:focus {
  box-shadow: 0 0 0 3px rgba(220, 38, 38, 0.1);
}

.input--disabled {
  background-color: #f9fafb;
  cursor: not-allowed;
  opacity: 0.6;
}

.input--withStartIcon {
  padding-left: 2.5rem;
}

.input--withEndIcon {
  padding-right: 2.5rem;
}

/* Icons */
.startIcon,
.endIcon {
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 1.25rem;
  height: 1.25rem;
  color: #6b7280;
}

.startIcon {
  left: 0.75rem;
}

.endIcon {
  right: 0.75rem;
}

/* Error message */
.errorMessage {
  font-size: 0.75rem;
  color: #dc2626;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

/* Help text */
.helpText {
  font-size: 0.75rem;
  color: #6b7280;
}
```

```tsx
// File: src/components/Input/Input.tsx

import { forwardRef } from 'react';
import clsx from 'clsx';
import styles from './Input.module.css';

interface InputProps extends Omit<React.InputHTMLAttributes<HTMLInputElement>, 'size'> {
  /** Label text */
  label?: string;
  /** Error message to display */
  error?: string;
  /** Help text shown below input */
  helpText?: string;
  /** Start icon (rendered before input) */
  startIcon?: React.ReactNode;
  /** End icon (rendered after input) */
  endIcon?: React.ReactNode;
  /** Size variant */
  size?: 'sm' | 'md' | 'lg';
  /** Whether to show the required asterisk */
  required?: boolean;
}

const Input = forwardRef<HTMLInputElement, InputProps>(({
  label,
  error,
  helpText,
  startIcon,
  endIcon,
  size = 'md',
  required = false,
  disabled = false,
  className,
  id,
  ...props
}, ref) => {
  // Generate ID if not provided
  const inputId = id || `input-${Math.random().toString(36).slice(2, 9)}`;
  
  // Determine if there's an error
  const hasError = Boolean(error);

  // Build input classes dynamically
  const inputClasses = clsx(
    styles.input,
    // Size modifier
    size === 'sm' && styles['input--small'],
    size === 'lg' && styles['input--large'],
    // State modifiers
    hasError && styles['input--error'],
    disabled && styles['input--disabled'],
    // Icon modifiers
    startIcon && styles['input--withStartIcon'],
    endIcon && styles['input--withEndIcon'],
    // Additional classes
    className
  );

  return (
    <div className={styles.inputWrapper}>
      {/* Label */}
      {label && (
        <label htmlFor={inputId} className={styles.label}>
          {label}
          {required && <span className={styles.required} aria-hidden="true">*</span>}
        </label>
      )}

      {/* Input container with icons */}
      <div className={styles.inputContainer}>
        {startIcon && (
          <span className={styles.startIcon} aria-hidden="true">
            {startIcon}
          </span>
        )}

        <input
          ref={ref}
          id={inputId}
          className={inputClasses}
          disabled={disabled}
          required={required}
          aria-invalid={hasError ? 'true' : 'false'}
          aria-describedby={
            [
              hasError && `${inputId}-error`,
              helpText && `${inputId}-help`
            ]
              .filter(Boolean)
              .join(' ') || undefined
          }
          {...props}
        />

        {endIcon && (
          <span className={styles.endIcon} aria-hidden="true">
            {endIcon}
          </span>
        )}
      </div>

      {/* Error message */}
      {error && (
        <span 
          id={`${inputId}-error`} 
          className={styles.errorMessage} 
          role="alert"
        >
          <span>⚠</span>
          {error}
        </span>
      )}

      {/* Help text - shown when there's no error */}
      {!error && helpText && (
        <span 
          id={`${inputId}-help`} 
          className={styles.helpText}
        >
          {helpText}
        </span>
      )}
    </div>
  );
});

Input.displayName = 'Input';

export default Input;
```

## Key Takeaways

- Use template literals or clsx library to build dynamic class strings
- Always verify CSS classes exist before referencing them
- Filter out falsy values to avoid "undefined" in className
- Combine classes from multiple CSS Modules when needed
- Create utility functions like `cn()` for consistent class handling
- Use conditional objects in clsx for clean if-else class logic
- Reference classes via bracket notation for dynamic names: `styles[classNameVar]`

## What's Next

Continue to [Theming with CSS Variables](/08-styling/01-css-modules/03-theming-with-css-variables.md) to learn how to implement theming, create CSS custom properties, and switch between light/dark modes.