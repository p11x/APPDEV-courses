# Global Styles and Themes with Styled Components

## Overview

Styled-components provides powerful theming capabilities through its ThemeProvider component. Combined with createGlobalStyle, you can create consistent design systems with global styles, theme-aware components, and runtime theme switching. This guide covers setting up global styles, creating theme objects, and implementing theme switching.

## Prerequisites

- Completed styled-components basics guide
- Understanding of React Context
- Familiarity with CSS custom properties

## Core Concepts

### Creating Global Styles

Use createGlobalStyle to inject global CSS that applies to your entire application:

```tsx
// File: src/styles/GlobalStyles.ts

import { createGlobalStyle } from 'styled-components';

// Define theme type
export const theme = {
  colors: {
    primary: '#3b82f6',
    secondary: '#6b7280',
    success: '#10b981',
    warning: '#f59e0b',
    error: '#ef4444',
    background: '#ffffff',
    surface: '#f9fafb',
    text: '#111827',
    textSecondary: '#6b7280',
    border: '#e5e7eb'
  },
  spacing: {
    xs: '0.25rem',
    sm: '0.5rem',
    md: '1rem',
    lg: '1.5rem',
    xl: '2rem'
  },
  borderRadius: {
    sm: '0.25rem',
    md: '0.5rem',
    lg: '0.75rem',
    full: '9999px'
  },
  fonts: {
    body: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif',
    mono: '"SF Mono", Consolas, monospace'
  }
};

export type Theme = typeof theme;

// Create global styles
export const GlobalStyles = createGlobalStyle`
  /* Reset box sizing */
  *, *::before, *::after {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
  }
  
  /* Base styles */
  html {
    font-size: 16px;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
  }
  
  body {
    font-family: ${({ theme }) => theme.fonts.body};
    background-color: ${({ theme }) => theme.colors.background};
    color: ${({ theme }) => theme.colors.text};
    line-height: 1.5;
    min-height: 100vh;
  }
  
  /* Remove default button styles */
  button {
    font-family: inherit;
  }
  
  /* Focus styles */
  :focus-visible {
    outline: 2px solid ${({ theme }) => theme.colors.primary};
    outline-offset: 2px;
  }
  
  /* Smooth scrolling */
  html {
    scroll-behavior: smooth;
  }
  
  /* Custom scrollbar */
  ::-webkit-scrollbar {
    width: 8px;
    height: 8px;
  }
  
  ::-webkit-scrollbar-track {
    background: ${({ theme }) => theme.colors.surface};
  }
  
  ::-webkit-scrollbar-thumb {
    background: ${({ theme }) => theme.colors.border};
    border-radius: 4px;
  }
  
  ::-webkit-scrollbar-thumb:hover {
    background: ${({ theme }) => theme.colors.secondary};
  }
`;
```

### Setting Up ThemeProvider

Wrap your application with ThemeProvider:

```tsx
// File: src/App.tsx

import { ThemeProvider } from 'styled-components';
import { GlobalStyles, theme } from './styles/GlobalStyles';
import { MainPage } from './pages/MainPage';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <GlobalStyles />
      <MainPage />
    </ThemeProvider>
  );
}

export default App;
```

### Using Theme in Components

Access theme values using the theme prop:

```tsx
// File: src/components/Button.tsx

import styled, { css, useTheme } from 'styled-components';

interface ButtonProps {
  $variant?: 'primary' | 'secondary' | 'danger';
  $size?: 'sm' | 'md' | 'lg';
}

const Button = styled.button<ButtonProps>`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  border-radius: ${({ theme }) => theme.borderRadius.md};
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  
  /* Access theme values */
  font-size: ${({ $size, theme }) => 
    $size === 'sm' ? '0.875rem' : 
    $size === 'lg' ? '1.125rem' : '1rem'
  };
  
  padding: ${({ $size }) => 
    $size === 'sm' ? '0.375rem 0.75rem' :
    $size === 'lg' ? '0.75rem 1.5rem' :
    '0.5rem 1rem'
  };
  
  /* Variant styles using theme colors */
  ${({ $variant, theme }) => {
    switch ($variant) {
      case 'secondary':
        return css`
          background-color: ${theme.colors.secondary};
          color: white;
        `;
      case 'danger':
        return css`
          background-color: ${theme.colors.error};
          color: white;
        `;
      default:
        return css`
          background-color: ${theme.colors.primary};
          color: white;
        `;
    }
  }}
  
  &:hover {
    opacity: 0.9;
  }
  
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`;

function MyComponent() {
  // Access theme programmatically
  const theme = useTheme();
  
  return (
    <div>
      <Button>Primary</Button>
      <Button $variant="secondary">Secondary</Button>
      <Button $variant="danger">Danger</Button>
    </div>
  );
}
```

### Creating Dark/Light Themes

Define multiple themes and switch between them:

```tsx
// File: src/styles/themes.ts

import { DefaultTheme } from 'styled-components';

// Light theme
export const lightTheme: DefaultTheme = {
  colors: {
    primary: '#3b82f6',
    secondary: '#6b7280',
    success: '#10b981',
    warning: '#f59e0b',
    error: '#ef4444',
    background: '#ffffff',
    surface: '#f9fafb',
    text: '#111827',
    textSecondary: '#6b7280',
    border: '#e5e7eb'
  }
};

// Dark theme
export const darkTheme: DefaultTheme = {
  colors: {
    primary: '#60a5fa',
    secondary: '#9ca3af',
    success: '#34d399',
    warning: '#fbbf24',
    error: '#f87171',
    background: '#111827',
    surface: '#1f2937',
    text: '#f9fafb',
    textSecondary: '#9ca3af',
    border: '#374151'
  }
};
```

### Creating a Theme Toggle

Build theme switching functionality:

```tsx
// File: src/contexts/ThemeContext.tsx

import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { ThemeProvider as StyledThemeProvider, DefaultTheme } from 'styled-components';
import { lightTheme, darkTheme } from '../styles/themes';
import { GlobalStyles } from '../styles/GlobalStyles';

type ThemeMode = 'light' | 'dark';

interface ThemeContextType {
  mode: ThemeMode;
  toggleTheme: () => void;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export function useThemeMode() {
  const context = useContext(ThemeContext);
  if (!context) {
    throw new Error('useThemeMode must be used within ThemeProvider');
  }
  return context;
}

export function ThemeProvider({ children }: { children: ReactNode }) {
  const [mode, setMode] = useState<ThemeMode>(() => {
    const stored = localStorage.getItem('theme-mode') as ThemeMode;
    if (stored === 'light' || stored === 'dark') return stored;
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  });

  const toggleTheme = () => {
    setMode(prev => {
      const next = prev === 'light' ? 'dark' : 'light';
      localStorage.setItem('theme-mode', next);
      return next;
    });
  };

  const theme = mode === 'dark' ? darkTheme : lightTheme;

  return (
    <ThemeContext.Provider value={{ mode, toggleTheme }}>
      <StyledThemeProvider theme={theme}>
        <GlobalStyles />
        {children}
      </StyledThemeProvider>
    </ThemeContext.Provider>
  );
}
```

## Common Mistakes

### Not Typing the Theme

Always extend styled-components' DefaultTheme for TypeScript support:

```tsx
// ❌ WRONG - No theme typing
const Button = styled.button`
  color: ${props => props.theme.colors.primary};
`;

// ✅ CORRECT - Extend DefaultTheme
import { DefaultTheme } from 'styled-components';

declare module 'styled-components' {
  export interface DefaultTheme {
    colors: {
      primary: string;
      // ...other colors
    };
  }
}
```

## Real-World Example

A complete theming system:

```tsx
// File: src/styles/theme.ts

import { DefaultTheme } from 'styled-components';

export const lightTheme: DefaultTheme = {
  colors: {
    primary: {
      50: '#eff6ff',
      100: '#dbeafe',
      500: '#3b82f6',
      600: '#2563eb',
      700: '#1d4ed8'
    },
    gray: {
      50: '#f9fafb',
      100: '#f3f4f6',
      200: '#e5e7eb',
      300: '#d1d5db',
      400: '#9ca3af',
      500: '#6b7280',
      600: '#4b5563',
      700: '#374151',
      800: '#1f2937',
      900: '#111827'
    },
    success: '#10b981',
    warning: '#f59e0b',
    error: '#ef4444',
    background: '#ffffff',
    surface: '#f9fafb',
    text: '#111827',
    textSecondary: '#6b7280',
    border: '#e5e7eb'
  },
  spacing: {
    xs: '0.25rem',
    sm: '0.5rem',
    md: '1rem',
    lg: '1.5rem',
    xl: '2rem'
  },
  borderRadius: {
    sm: '0.25rem',
    md: '0.5rem',
    lg: '0.75rem',
    xl: '1rem',
    full: '9999px'
  },
  shadows: {
    sm: '0 1px 2px 0 rgba(0, 0, 0, 0.05)',
    md: '0 4px 6px -1px rgba(0, 0, 0, 0.1)',
    lg: '0 10px 15px -3px rgba(0, 0, 0, 0.1)'
  }
};

export const darkTheme: DefaultTheme = {
  ...lightTheme,
  colors: {
    ...lightTheme.colors,
    primary: {
      50: '#1e3a5f',
      100: '#1e40af',
      500: '#60a5fa',
      600: '#3b82f6',
      700: '#2563eb'
    },
    background: '#111827',
    surface: '#1f2937',
    text: '#f9fafb',
    textSecondary: '#d1d5db',
    border: '#374151'
  }
};
```

## Key Takeaways

- Use createGlobalStyle for global CSS rules
- Wrap application with ThemeProvider to provide theme to all components
- Access theme via props.theme or useTheme() hook
- Define multiple themes for light/dark mode switching
- Use localStorage to persist theme preferences
- Extend DefaultTheme for TypeScript support

## What's Next

This completes the Styling section. Continue to [Understanding React Renders](/09-performance/01-rendering-optimization/01-understanding-react-renders.md) to learn about React's rendering behavior and how to optimize performance.