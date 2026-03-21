# Dark Mode Implementation in Tailwind CSS

## Overview

Dark mode has become an essential feature for modern web applications. Tailwind CSS provides excellent dark mode support through its `dark:` variant. This guide covers implementing dark mode with the class strategy, creating a theme toggle, persisting user preferences, and handling system preferences.

## Prerequisites

- Completed Tailwind Setup guide
- Understanding of CSS custom properties (optional but helpful)
- Basic React state management knowledge

## Core Concepts

### Enabling Dark Mode

First, configure Tailwind to use the class-based dark mode strategy:

```javascript
// File: tailwind.config.js

/** @type {import('tailwindcss').Config} */
export default {
  // Use class strategy - dark mode controlled by adding 'dark' class to HTML
  darkMode: 'class',
  
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  
  theme: {
    extend: {},
  },
  
  plugins: [],
}
```

### Using the dark: Variant

Apply dark mode styles using the `dark:` prefix:

```tsx
// File: src/components/DarkModeExample.tsx

function DarkModeExample() {
  return (
    <div className="bg-white dark:bg-gray-900 text-gray-900 dark:text-gray-100 min-h-screen p-8">
      <h1 className="text-3xl font-bold mb-4">
        Dark Mode Example
      </h1>
      
      {/* Cards that adapt to dark mode */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div className="p-6 bg-gray-50 dark:bg-gray-800 rounded-xl">
          <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
            Card Title
          </h2>
          <p className="text-gray-600 dark:text-gray-300">
            This card adapts its background color based on the theme.
          </p>
        </div>
        
        <div className="p-6 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded-xl">
          <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
            Bordered Card
          </h2>
          <p className="text-gray-600 dark:text-gray-300">
            The border color also changes with the theme.
          </p>
        </div>
      </div>
      
      {/* Buttons with dark mode variants */}
      <div className="flex gap-4 mt-6">
        <button className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg dark:bg-blue-500 dark:hover:bg-blue-600">
          Primary Button
        </button>
        
        <button className="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-900 rounded-lg dark:bg-gray-700 dark:hover:bg-gray-600 dark:text-gray-100">
          Secondary Button
        </button>
        
        <button className="px-4 py-2 border-2 border-blue-600 text-blue-600 hover:bg-blue-50 rounded-lg dark:border-blue-400 dark:text-blue-400 dark:hover:bg-blue-900/30">
          Outline Button
        </button>
      </div>
    </div>
  );
}
```

### Creating a Theme Toggle Hook

Build a custom hook to manage dark mode state:

```tsx
// File: src/hooks/useDarkMode.ts

import { useState, useEffect, useCallback } from 'react';

type Theme = 'light' | 'dark';

export function useDarkMode() {
  const [theme, setTheme] = useState<Theme>(() => {
    // Check localStorage first
    const stored = localStorage.getItem('theme') as Theme;
    if (stored === 'light' || stored === 'dark') {
      return stored;
    }
    // Check system preference
    if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return 'dark';
    }
    return 'light';
  });

  // Update document class when theme changes
  useEffect(() => {
    const root = document.documentElement;
    
    if (theme === 'dark') {
      root.classList.add('dark');
    } else {
      root.classList.remove('dark');
    }
    
    // Persist to localStorage
    localStorage.setItem('theme', theme);
  }, [theme]);

  // Toggle theme
  const toggleTheme = useCallback(() => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  }, []);

  // Set specific theme
  const setTheme = useCallback((newTheme: Theme) => {
    setTheme(newTheme);
  }, []);

  return { theme, toggleTheme, setTheme, isDark: theme === 'dark' };
}
```

### Theme Toggle Button Component

Create a reusable toggle component:

```tsx
// File: src/components/ThemeToggle.tsx

import { useDarkMode } from '../hooks/useDarkMode';

function ThemeToggle() {
  const { isDark, toggleTheme } = useDarkMode();

  return (
    <button
      onClick={toggleTheme}
      className="p-2 rounded-lg bg-gray-100 hover:bg-gray-200 dark:bg-gray-800 dark:hover:bg-gray-700 transition-colors"
      aria-label={`Switch to ${isDark ? 'light' : 'dark'} mode`}
    >
      {/* Sun icon - shown in dark mode */}
      {isDark ? (
        <svg className="w-5 h-5 text-yellow-500" fill="currentColor" viewBox="0 0 20 20">
          <path
            fillRule="evenodd"
            d="M10 2a1 1 0 011 1v1a1 1 0 11-2 0V3a1 1 0 011-1zm4 8a4 4 0 11-8 0 4 4 0 018 0zm-.464 4.95l.707.707a1 1 0 001.414-1.414l-.707-.707a1 1 0 00-1.414 1.414zm2.12-10.607a1 1 0 010 1.414l-.706.707a1 1 0 11-1.414-1.414l.707-.707a1 1 0 011.414 0zM17 11a1 1 0 100-2h-1a1 1 0 100 2h1zm-7 4a1 1 0 011 1v1a1 1 0 11-2 0v-1a1 1 0 011-1zM5.05 6.464A1 1 0 106.465 5.05l-.708-.707a1 1 0 00-1.414 1.414l.707.707zm1.414 8.486l-.707.707a1 1 0 01-1.414-1.414l.707-.707a1 1 0 011.414 1.414zM4 11a1 1 0 100-2H3a1 1 0 000 2h1z"
            clipRule="evenodd"
          />
        </svg>
      ) : (
        /* Moon icon - shown in light mode */
        <svg className="w-5 h-5 text-gray-700" fill="currentColor" viewBox="0 0 20 20">
          <path d="M17.293 13.293A8 8 0 016.707 2.707a8.001 8.001 0 1010.586 10.586z" />
        </svg>
      )}
    </button>
  );
}
```

### Handling System Preference

Detect and respond to system color scheme changes:

```tsx
// File: src/hooks/useSystemTheme.ts

import { useState, useEffect } from 'react';

type Theme = 'light' | 'dark';

export function useSystemTheme() {
  const [systemTheme, setSystemTheme] = useState<Theme>(() => {
    return window.matchMedia('(prefers-color-scheme: dark)').matches
      ? 'dark'
      : 'light';
  });

  useEffect(() => {
    // Create media query listener
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    
    const handleChange = (e: MediaQueryListEvent) => {
      setSystemTheme(e.matches ? 'dark' : 'light');
    };
    
    // Listen for changes
    mediaQuery.addEventListener('change', handleChange);
    
    // Cleanup
    return () => mediaQuery.removeEventListener('change', handleChange);
  }, []);

  return systemTheme;
}
```

## Common Mistakes

### Forgetting to Add dark Mode Config

Always enable dark mode in your Tailwind config:

```javascript
// ❌ WRONG - Missing darkMode config
export default {
  content: ['./src/**/*.{js,ts,jsx,tsx}'],
}

// ✅ CORRECT - Enable class-based dark mode
export default {
  darkMode: 'class',
  content: ['./src/**/*.{js,ts,jsx,tsx}'],
}
```

### Using Dark Colors Hardcoded

Always use Tailwind's dark: variant instead of hardcoding dark colors:

```tsx
// ❌ WRONG - Hardcoded dark colors
<div className="bg-gray-900 text-white">

// ✅ CORRECT - Use dark: variant
<div className="bg-white dark:bg-gray-900 dark:text-white">
```

## Real-World Example

A complete app layout with dark mode support:

```tsx
// File: src/App.tsx

import { useEffect, useState } from 'react';
import { useDarkMode } from './hooks/useDarkMode';
import ThemeToggle from './components/ThemeToggle';

function App() {
  const { theme, toggleTheme } = useDarkMode();

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
      {/* Header */}
      <header className="bg-white dark:bg-gray-800 border-b border-gray-200 dark:border-gray-700 sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            {/* Logo */}
            <div className="flex-shrink-0 flex items-center">
              <h1 className="text-2xl font-bold text-blue-600 dark:text-blue-400">
                MyApp
              </h1>
            </div>

            {/* Navigation */}
            <nav className="hidden md:flex items-center space-x-8">
              <a 
                href="#" 
                className="text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400"
              >
                Home
              </a>
              <a 
                href="#" 
                className="text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400"
              >
                Products
              </a>
              <a 
                href="#" 
                className="text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400"
              >
                About
              </a>
              <a 
                href="#" 
                className="text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400"
              >
                Contact
              </a>
            </nav>

            {/* Right side - theme toggle + CTA */}
            <div className="flex items-center gap-4">
              <ThemeToggle />
              <button className="hidden sm:block px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors">
                Get Started
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        {/* Hero Section */}
        <div className="text-center mb-16">
          <h2 className="text-4xl sm:text-5xl font-bold text-gray-900 dark:text-white mb-4">
            Build Something Amazing
          </h2>
          <p className="text-lg sm:text-xl text-gray-600 dark:text-gray-300 max-w-2xl mx-auto">
            Create beautiful, responsive applications with dark mode support out of the box.
          </p>
        </div>

        {/* Feature Cards */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          {[
            {
              title: 'Fast Performance',
              description: 'Built for speed with optimized rendering and minimal bundle size.',
              icon: '⚡'
            },
            {
              title: 'Dark Mode',
              description: 'Beautiful dark mode that adapts to your system preferences.',
              icon: '🌙'
            },
            {
              title: 'Responsive',
              description: 'Looks great on any device, from mobile to desktop.',
              icon: '📱'
            }
          ].map((feature, index) => (
            <div 
              key={index}
              className="bg-white dark:bg-gray-800 rounded-xl p-6 shadow-md hover:shadow-lg transition-shadow"
            >
              <div className="text-4xl mb-4">{feature.icon}</div>
              <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
                {feature.title}
              </h3>
              <p className="text-gray-600 dark:text-gray-300">
                {feature.description}
              </p>
            </div>
          ))}
        </div>

        {/* Toggle Demo */}
        <div className="mt-16 text-center">
          <p className="text-gray-600 dark:text-gray-400 mb-4">
            Current theme: <span className="font-semibold capitalize">{theme}</span>
          </p>
          <button
            onClick={toggleTheme}
            className="px-6 py-3 bg-gray-900 dark:bg-white text-white dark:text-gray-900 font-semibold rounded-lg hover:opacity-90 transition-opacity"
          >
            Toggle Theme
          </button>
        </div>
      </main>

      {/* Footer */}
      <footer className="bg-white dark:bg-gray-800 border-t border-gray-200 dark:border-gray-700 mt-16">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <p className="text-center text-gray-500 dark:text-gray-400">
            © 2024 MyApp. Built with React and Tailwind CSS.
          </p>
        </div>
      </footer>
    </div>
  );
}

export default App;
```

## Key Takeaways

- Enable dark mode in Tailwind with `darkMode: 'class'`
- Use `dark:` prefix for dark mode variants
- Build a theme toggle hook to manage state and persistence
- Use localStorage to persist user preferences
- Listen for system preference changes with matchMedia
- Test dark mode on actual devices
- Use semantic color names in your design system

## What's Next

Continue to [Styled Components Basics](/08-styling/03-styled-components/01-styled-components-basics.md) to learn another popular CSS-in-JS approach for styling React components.