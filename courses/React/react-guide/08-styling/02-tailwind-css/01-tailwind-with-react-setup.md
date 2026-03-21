# Tailwind CSS Setup with React

## Overview

Tailwind CSS is a utility-first CSS framework that provides low-level utility classes to build custom designs without leaving your HTML. Unlike traditional CSS frameworks with pre-built components, Tailwind gives you building blocks to create unique designs. This guide covers setting up Tailwind with Vite, understanding the utility-first philosophy, and using the cn() helper pattern.

## Prerequisites

- A React project (Vite recommended)
- Basic understanding of CSS concepts
- Familiarity with terminal/command line

## Core Concepts

### Installing Tailwind CSS with Vite

Vite has excellent support for Tailwind CSS. The setup involves installing dependencies and initializing the config.

```bash
# File: terminal

# Create a new Vite React TypeScript project
npm create vite@latest my-tailwind-app -- --template react-ts

# Navigate to the project
cd my-tailwind-app

# Install Tailwind CSS and its dependencies
npm install -D tailwindcss postcss autoprefixer

# Initialize Tailwind config files
npx tailwindcss init -p
```

### Configuring Tailwind

After installation, configure Tailwind to scan your React files for classes.

```javascript
// File: tailwind.config.js

/** @type {import('tailwindcss').Config} */
export default {
  // Enable content scanning for class detection
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  
  // Enable dark mode via class strategy
  darkMode: 'class',
  
  // Extend the default theme with custom values
  theme: {
    // Custom color palette
    extend: {
      colors: {
        primary: {
          50: '#eff6ff',
          100: '#dbeafe',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
        },
        success: '#10b981',
        warning: '#f59e0b',
        error: '#ef4444',
      },
      
      // Custom spacing
      spacing: {
        '18': '4.5rem',
        '22': '5.5rem',
      },
      
      // Custom border radius
      borderRadius: {
        'xl': '1rem',
        '2xl': '1.5rem',
      },
      
      // Custom animations
      animation: {
        'fade-in': 'fadeIn 0.5s ease-out',
        'slide-up': 'slideUp 0.3s ease-out',
      },
      
      // Animation keyframes
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        slideUp: {
          '0%': { transform: 'translateY(10px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
      },
    },
  },
  
  // Enable JIT mode (default in Tailwind v3+)
  // Just-In-Time compiler generates only used styles
  
  // Add plugins
  plugins: [],
}
```

### Adding Tailwind Directives

Add the Tailwind directives to your main CSS file.

```css
/* File: src/index.css */

/* Tailwind base styles */
@tailwind base;

/* Tailwind components */
@tailwind components;

/* Tailwind utilities */
@tailwind utilities;

/* Custom base styles */
@layer base {
  body {
    @apply antialiased text-gray-900 bg-gray-50;
  }
  
  /* Dark mode styles */
  .dark body {
    @apply text-gray-100 bg-gray-900;
  }
}

/* Custom component classes */
@layer components {
  .btn-primary {
    @apply px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors;
  }
  
  .card {
    @apply bg-white rounded-xl shadow-md p-6;
  }
}

/* Custom utility classes */
@layer utilities {
  .text-balance {
    text-wrap: balance;
  }
}
```

### Using Tailwind in Components

Tailwind classes are applied directly to JSX elements—no separate CSS files needed.

```tsx
// File: src/components/Button.tsx

interface ButtonProps {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary' | 'outline';
  size?: 'sm' | 'md' | 'lg';
  disabled?: boolean;
  onClick?: () => void;
  className?: string;
}

function Button({
  children,
  variant = 'primary',
  size = 'md',
  disabled = false,
  onClick,
  className = ''
}: ButtonProps) {
  // Base classes + variant + size
  const baseClasses = 'inline-flex items-center justify-center font-medium rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2';
  
  const variantClasses = {
    primary: 'bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500',
    secondary: 'bg-gray-200 text-gray-900 hover:bg-gray-300 focus:ring-gray-500 dark:bg-gray-700 dark:text-gray-100',
    outline: 'border-2 border-blue-600 text-blue-600 hover:bg-blue-50 focus:ring-blue-500',
  };
  
  const sizeClasses = {
    sm: 'px-3 py-1.5 text-sm',
    md: 'px-4 py-2 text-base',
    lg: 'px-6 py-3 text-lg',
  };
  
  const disabledClasses = 'opacity-50 cursor-not-allowed';
  
  const classes = [
    baseClasses,
    variantClasses[variant],
    sizeClasses[size],
    disabled && disabledClasses,
    className,
  ].filter(Boolean).join(' ');
  
  return (
    <button
      className={classes}
      onClick={onClick}
      disabled={disabled}
    >
      {children}
    </button>
  );
}

export default Button;
```

### Creating a cn() Helper

For conditional classes in Tailwind, use a helper function to combine classes elegantly.

```tsx
// File: src/lib/utils.ts

import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

/**
 * Tailwind merge utility - combines tailwind classes intelligently
 * Handles conflicts where the last class wins
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

// Usage:
// import { cn } from '@/lib/utils';
//
// <div className={cn(
//   'base classes',
//   isActive && 'active classes',
//   variant === 'primary' ? 'primary classes' : 'secondary classes'
// )} />
```

```bash
# Install clsx and tailwind-merge
npm install clsx tailwind-merge
```

## Common Mistakes

### Mistake 1: Not Using @apply Correctly

Avoid overusing @apply—it defeats the purpose of utility classes.

```tsx
// ❌ WRONG - Creating components just to use @apply
/* In CSS */
.my-button {
  @apply px-4 py-2 bg-blue-500 text-white rounded;
}

// ✅ CORRECT - Use utilities directly in components
<button className="px-4 py-2 bg-blue-500 text-white rounded">
  Click me
</button>
```

### Mistake 2: Not Purging Unused Styles

Ensure your content paths are correct so unused styles aren't included.

```javascript
// ❌ WRONG - Missing file patterns
content: ["./index.html"],

// ✅ CORRECT - Include all relevant files
content: [
  "./index.html",
  "./src/**/*.{js,ts,jsx,tsx}",
],
```

## Real-World Example

Here's a responsive card component using Tailwind:

```tsx
// File: src/components/Card.tsx

import { cn } from '../lib/utils';

interface CardProps {
  children: React.ReactNode;
  title?: string;
  description?: string;
  image?: string;
  variant?: 'default' | 'outlined' | 'elevated';
  className?: string;
}

function Card({
  children,
  title,
  description,
  image,
  variant = 'default',
  className
}: CardProps) {
  const variantStyles = {
    default: 'bg-white shadow-md',
    outlined: 'bg-white border-2 border-gray-200',
    elevated: 'bg-white shadow-xl',
  };

  return (
    <div
      className={cn(
        'rounded-xl overflow-hidden transition-transform hover:scale-[1.02]',
        variantStyles[variant],
        className
      )}
    >
      {image && (
        <img
          src={image}
          alt={title}
          className="w-full h-48 object-cover"
        />
      )}
      
      <div className="p-6">
        {title && (
          <h3 className="text-xl font-semibold text-gray-900 mb-2">
            {title}
          </h3>
        )}
        
        {description && (
          <p className="text-gray-600 mb-4">
            {description}
          </p>
        )}
        
        {children}
      </div>
    </div>
  );
}

export default Card;
```

## Key Takeaways

- Install Tailwind with `npm install -D tailwindcss postcss autoprefixer`
- Initialize with `npx tailwindcss init -p`
- Configure content paths to scan your files
- Use utility classes directly in JSX
- Create a cn() helper using tailwind-merge for conditional classes
- Extend the theme in tailwind.config.js for custom values
- Use @layer for organizing custom component styles

## What's Next

Continue to [Responsive Design in Tailwind](/08-styling/02-tailwind-css/02-responsive-design-in-tailwind.md) to learn mobile-first responsive design patterns with Tailwind's breakpoint system.