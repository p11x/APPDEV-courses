# Theming with CSS Variables

## Overview

CSS Custom Properties (also known as CSS Variables) enable powerful theming capabilities in React applications. By defining design tokens as CSS variables, you can implement dark mode, theme switching, and dynamic styling changes without JavaScript-heavy solutions. This guide covers defining design tokens, implementing theme switching, and persisting user preferences.

## Prerequisites

- Completed CSS Modules Setup guide
- Understanding of CSS custom properties syntax
- Basic JavaScript for state management

## Core Concepts

### Defining Design Tokens

Design tokens are semantic values (colors, spacing, typography) that define your design system. Using CSS variables makes them centralized and easily themeable.

```css
/* File: src/styles/tokens.css */

/* CSS custom properties are defined on :root and inherited by all elements */

/* Color Palette - Primary Colors */
:root {
  /* Primary brand colors */
  --color-primary-50: #eff6ff;
  --color-primary-100: #dbeafe;
  --color-primary-500: #3b82f6;
  --color-primary-600: #2563eb;
  --color-primary-700: #1d4ed8;
  
  /* Neutral colors */
  --color-gray-50: #f9fafb;
  --color-gray-100: #f3f4f6;
  --color-gray-200: #e5e7eb;
  --color-gray-300: #d1d5db;
  --color-gray-400: #9ca3af;
  --color-gray-500: #6b7280;
  --color-gray-600: #4b5563;
  --color-gray-700: #374151;
  --color-gray-800: #1f2937;
  --color-gray-900: #111827;
  
  /* Semantic colors */
  --color-success: #10b981;
  --color-warning: #f59e0b;
  --color-error: #ef4444;
  --color-info: #3b82f6;
  
  /* Text colors */
  --color-text-primary: var(--color-gray-900);
  --color-text-secondary: var(--color-gray-600);
  --color-text-muted: var(--color-gray-400);
  
  /* Background colors */
  --color-bg-primary: #ffffff;
  --color-bg-secondary: var(--color-gray-50);
  --color-bg-tertiary: var(--color-gray-100);
  
  /* Border colors */
  --color-border: var(--color-gray-200);
  --color-border-hover: var(--color-gray-300);
  
  /* Spacing tokens */
  --spacing-xs: 0.25rem;   /* 4px */
  --spacing-sm: 0.5rem;    /* 8px */
  --spacing-md: 1rem;       /* 16px */
  --spacing-lg: 1.5rem;   /* 24px */
  --spacing-xl: 2rem;     /* 32px */
  --spacing-2xl: 3rem;    /* 48px */
  
  /* Typography */
  --font-family-base: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  --font-family-mono: 'SF Mono', Consolas, monospace;
  
  --font-size-xs: 0.75rem;    /* 12px */
  --font-size-sm: 0.875rem;   /* 14px */
  --font-size-base: 1rem;     /* 16px */
  --font-size-lg: 1.125rem;   /* 18px */
  --font-size-xl: 1.25rem;     /* 20px */
  --font-size-2xl: 1.5rem;     /* 24px */
  --font-size-3xl: 1.875rem;  /* 30px */
  --font-size-4xl: 2.25rem;   /* 36px */
  
  --font-weight-normal: 400;
  --font-weight-medium: 500;
  --font-weight-semibold: 600;
  --font-weight-bold: 700;
  
  /* Border radius */
  --radius-sm: 0.25rem;
  --radius-md: 0.5rem;
  --radius-lg: 0.75rem;
  --radius-xl: 1rem;
  --radius-full: 9999px;
  
  /* Shadows */
  --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
  --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
  --shadow-xl: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
  
  /* Transitions */
  --transition-fast: 150ms ease;
  --transition-base: 200ms ease;
  --transition-slow: 300ms ease;
}
```

### Implementing Dark Mode with CSS Variables

Dark mode is implemented by overriding CSS variables on a theme attribute or class.

```css
/* File: src/styles/dark-theme.css */

/* Dark theme overrides - applied via data-theme="dark" attribute */
[data-theme="dark"] {
  /* Invert text colors */
  --color-text-primary: #f9fafb;
  --color-text-secondary: #d1d5db;
  --color-text-muted: #6b7280;
  
  /* Dark backgrounds */
  --color-bg-primary: #111827;
  --color-bg-secondary: #1f2937;
  --color-bg-tertiary: #374151;
  
  /* Lighter borders for dark backgrounds */
  --color-border: #374151;
  --color-border-hover: #4b5563;
  
  /* Adjust shadows for dark mode */
  --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.3);
  --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.4);
  --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.4);
  --shadow-xl: 0 20px 25px -5px rgba(0, 0, 0, 0.4);
  
  /* Optional: adjust primary colors for dark mode */
  --color-primary-50: #1e3a5f;
  --color-primary-100: #1e40af;
  --color-primary-500: #60a5fa;
  --color-primary-600: #3b82f6;
}
```

### Creating a Theme Provider Component

A React component to manage theme state and persist preferences.

```tsx
// File: src/components/ThemeProvider/ThemeProvider.tsx

import { createContext, useContext, useState, useEffect, useCallback } from 'react';

// Define theme type
type Theme = 'light' | 'dark';

// Define context shape
interface ThemeContextType {
  theme: Theme;
  toggleTheme: () => void;
  setTheme: (theme: Theme) => void;
}

// Create context with undefined default
const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

// Storage key for persisting theme
const THEME_STORAGE_KEY = 'app-theme';

// Provider props
interface ThemeProviderProps {
  children: React.ReactNode;
  defaultTheme?: Theme;
}

export function ThemeProvider({ 
  children, 
  defaultTheme = 'light' 
}: ThemeProviderProps) {
  // Initialize theme from localStorage or default
  const [theme, setThemeState] = useState<Theme>(() => {
    // Check localStorage first
    const stored = localStorage.getItem(THEME_STORAGE_KEY) as Theme;
    if (stored === 'light' || stored === 'dark') {
      return stored;
    }
    
    // Check system preference
    if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return 'dark';
    }
    
    return defaultTheme;
  });

  // Apply theme to document
  useEffect(() => {
    // Set theme attribute on html element for CSS selectors
    document.documentElement.setAttribute('data-theme', theme);
    
    // Persist to localStorage
    localStorage.setItem(THEME_STORAGE_KEY, theme);
  }, [theme]);

  // Toggle between light and dark
  const toggleTheme = useCallback(() => {
    setThemeState(prev => prev === 'light' ? 'dark' : 'light');
  }, []);

  // Set specific theme
  const setTheme = useCallback((newTheme: Theme) => {
    setThemeState(newTheme);
  }, []);

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme, setTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

// Custom hook to use theme
export function useTheme() {
  const context = useContext(ThemeContext);
  
  if (context === undefined) {
    throw new Error('useTheme must be used within a ThemeProvider');
  }
  
  return context;
}
```

### Reading CSS Variables in JavaScript

Sometimes you need to read CSS variable values in JavaScript for dynamic calculations.

```tsx
// File: src/hooks/useCSSVariable.ts

import { useState, useEffect } from 'react';

/**
 * Hook to get the computed value of a CSS custom property
 * Useful for responsive calculations or dynamic sizing
 * 
 * @param variableName - The CSS variable name (e.g., '--spacing-md')
 * @param initialValue - Fallback value if variable isn't found
 * @returns The computed value of the CSS variable
 */
function useCSSVariable(variableName: string, initialValue: string = ''): string {
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    // Get the computed style from the root element
    const root = document.documentElement;
    const computedStyle = getComputedStyle(root);
    
    // Read the CSS variable
    const cssValue = computedStyle.getPropertyValue(variableName).trim();
    setValue(cssValue || initialValue);

    // Optional: Create a MutationObserver to watch for changes
    // This is useful if themes can change dynamically
    const observer = new MutationObserver(() => {
      const newValue = computedStyle.getPropertyValue(variableName).trim();
      setValue(newValue || initialValue);
    });

    observer.observe(root, {
      attributes: true,
      attributeFilter: ['data-theme', 'style']
    });

    return () => observer.disconnect();
  }, [variableName, initialValue]);

  return value;
}

export default useCSSVariable;
```

### Using CSS Variables in Components

```tsx
// File: src/components/Card/Card.tsx

import clsx from 'clsx';
import styles from './Card.module.css';
import { useTheme } from '../ThemeProvider/ThemeProvider';

interface CardProps {
  children: React.ReactNode;
  title?: string;
  variant?: 'default' | 'outlined' | 'elevated';
  className?: string;
}

function Card({ children, title, variant = 'default', className }: CardProps) {
  const { theme } = useTheme();

  return (
    <div
      className={clsx(
        styles.card,
        styles[`card--${variant}`],
        className
      )}
      // We can also set inline styles using CSS variables
      style={{
        // Using theme to set background based on CSS variable
        '--card-bg': 'var(--color-bg-primary)'
      } as React.CSSProperties}
    >
      {title && <h3 className={styles.title}>{title}</h3>}
      <div className={styles.content}>{children}</div>
    </div>
  );
}

export default Card;
```

```css
/* File: src/components/Card/Card.module.css */

.card {
  /* Use CSS variables for theming */
  background-color: var(--color-bg-primary);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: var(--spacing-lg);
  transition: all var(--transition-base);
}

.card:hover {
  box-shadow: var(--shadow-md);
  border-color: var(--color-border-hover);
}

.title {
  color: var(--color-text-primary);
  font-size: var(--font-size-lg);
  font-weight: var(--font-weight-semibold);
  margin: 0 0 var(--spacing-md) 0;
}

.content {
  color: var(--color-text-secondary);
  font-size: var(--font-size-base);
  line-height: 1.6;
}
```

## Common Mistakes

### Mistake 1: Not Defining Fallback Values

CSS variables should have fallback values for older browsers or undefined states.

```css
/* ❌ WRONG - No fallback might cause issues */
background-color: var(--color-primary);

/* ✅ CORRECT - Provide fallback values */
background-color: var(--color-primary, #3b82f6);
```

### Mistake 2: Using JavaScript for All Theming

Don't use JavaScript objects to store all theme values—use CSS variables for better performance.

```tsx
// ❌ WRONG - Too much JavaScript overhead
const theme = {
  colors: { primary: '#3b82f6', ... },
  spacing: { ... }
};
<div style={{ backgroundColor: theme.colors.primary }} />

// ✅ CORRECT - Use CSS variables
<div style={{ backgroundColor: 'var(--color-primary)' }} />
```

## Real-World Example

Here's a complete theme toggle button component with smooth transitions:

```tsx
// File: src/components/ThemeToggle/ThemeToggle.tsx

import { useEffect } from 'react';
import { useTheme } from '../ThemeProvider/ThemeProvider';
import styles from './ThemeToggle.module.css';

/**
 * A toggle button that switches between light and dark themes
 * Shows animated icon based on current theme
 */
function ThemeToggle() {
  const { theme, toggleTheme } = useTheme();

  return (
    <button
      type="button"
      onClick={toggleTheme}
      className={styles.toggle}
      aria-label={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
      title={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
    >
      {/* Sun icon - shown in dark mode */}
      <span 
        className={`${styles.icon} ${theme === 'dark' ? styles.iconVisible : styles.iconHidden}`}
        aria-hidden="true"
      >
        ☀️
      </span>
      
      {/* Moon icon - shown in light mode */}
      <span 
        className={`${styles.icon} ${theme === 'light' ? styles.iconVisible : styles.iconHidden}`}
        aria-hidden="true"
      >
        🌙
      </span>
    </button>
  );
}

export default ThemeToggle;
```

```css
/* File: src/components/ThemeToggle/ThemeToggle.module.css */

.toggle {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 2.5rem;
  height: 2.5rem;
  border-radius: var(--radius-full);
  border: 1px solid var(--color-border);
  background-color: var(--color-bg-primary);
  cursor: pointer;
  transition: all var(--transition-base);
  overflow: hidden;
}

.toggle:hover {
  background-color: var(--color-bg-secondary);
  border-color: var(--color-border-hover);
}

.toggle:focus-visible {
  outline: 2px solid var(--color-primary-500);
  outline-offset: 2px;
}

.icon {
  font-size: 1.25rem;
  position: absolute;
  transition: all var(--transition-slow);
}

.iconVisible {
  opacity: 1;
  transform: scale(1) rotate(0deg);
}

.iconHidden {
  opacity: 0;
  transform: scale(0.5) rotate(-90deg);
}
```

```tsx
// File: src/App.tsx

import { ThemeProvider } from './components/ThemeProvider/ThemeProvider';
import ThemeToggle from './components/ThemeToggle/ThemeToggle';
import Card from './components/Card/Card';

function App() {
  return (
    <ThemeProvider>
      <div style={{ 
        minHeight: '100vh',
        backgroundColor: 'var(--color-bg-secondary)',
        color: 'var(--color-text-primary)',
        padding: 'var(--spacing-xl)'
      }}>
        <header style={{ 
          display: 'flex', 
          justifyContent: 'space-between',
          alignItems: 'center',
          marginBottom: 'var(--spacing-2xl)'
        }}>
          <h1>My App</h1>
          <ThemeToggle />
        </header>

        <Card title="Welcome">
          <p>
            This card automatically adapts to the current theme. 
            Try toggling between light and dark modes!
          </p>
        </Card>
      </div>
    </ThemeProvider>
  );
}

export default App;
```

## Key Takeaways

- Define design tokens as CSS custom properties in :root
- Override variables for dark mode using the data-theme attribute
- Create a ThemeProvider component to manage theme state
- Persist theme preferences in localStorage
- Use CSS variables in JavaScript via style props or getComputedStyle
- Provide fallback values for CSS variables for browser compatibility
- Theme transitions are smoother when using CSS variables

## What's Next

Continue to [Tailwind with React Setup](/08-styling/02-tailwind-css/01-tailwind-with-react-setup.md) to learn an alternative approach to styling using the utility-first CSS framework Tailwind CSS.