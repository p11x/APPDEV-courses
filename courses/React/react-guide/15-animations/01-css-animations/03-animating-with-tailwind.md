# Animating with Tailwind CSS

## Overview
Tailwind CSS provides utility classes for common animations out of the box, plus the ability to define custom animations in your configuration. This guide covers both built-in animations and how to create custom ones while respecting accessibility preferences.

## Prerequisites
- Tailwind CSS fundamentals
- React component basics
- Understanding of CSS animations

## Core Concepts

### Built-in Tailwind Animation Utility Classes

Tailwind includes four built-in animation utilities that cover the most common use cases. Each is designed for specific scenarios in your application.

```tsx
// [File: src/components/LoadingExamples.tsx]
import React from 'react';

/**
 * Tailwind's built-in animations:
 * 
 * animate-spin   — Rotates element 360deg infinitely (loading spinners)
 * animate-bounce — Moves element up and down (scroll indicators)  
 * animate-pulse  — Fades element in and out (skeleton loaders)
 * animate-ping   — Scales and fades outward (notification dots)
 */

export function LoadingExamples() {
  return (
    <div className="flex gap-8 items-center p-8">
      {/* animate-spin: best for loading spinners and processing indicators */}
      <div className="flex flex-col items-center gap-2">
        <div className="w-8 h-8 border-4 border-blue-200 border-t-blue-600 rounded-full animate-spin" />
        <span className="text-sm text-gray-600">spin</span>
      </div>

      {/* animate-bounce: good for "scroll down" indicators */}
      <div className="flex flex-col items-center gap-2">
        <div className="w-8 h-8 bg-purple-500 rounded-full animate-bounce" />
        <span className="text-sm text-gray-600">bounce</span>
      </div>

      {/* animate-pulse: ideal for skeleton loaders and breathing effects */}
      <div className="flex flex-col items-center gap-2">
        <div className="w-8 h-8 bg-cyan-400 rounded animate-pulse" />
        <span className="text-sm text-gray-600">pulse</span>
      </div>

      {/* animate-ping: perfect for notification badges */}
      <div className="flex flex-col items-center gap-2 relative">
        <div className="w-8 h-8 bg-red-500 rounded-full animate-ping" />
        <span className="text-sm text-gray-600">ping</span>
      </div>
    </div>
  );
}
```

### Custom Keyframes in tailwind.config.js

For animations beyond the built-in ones, you can extend Tailwind's theme with custom keyframes and animations. This gives you full control while maintaining consistency.

```javascript
// [File: tailwind.config.js]
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      // Define custom keyframes — these are the individual animation steps
      keyframes: {
        // Slide up entrance animation
        slideUp: {
          '0%': { 
            transform: 'translateY(20px)', 
            opacity: '0' 
          },
          '100%': { 
            transform: 'translateY(0)', 
            opacity: '1' 
          },
        },
        
        // Slide down exit animation (for toasts, modals)
        slideDown: {
          '0%': { 
            transform: 'translateY(-20px)', 
            opacity: '0' 
          },
          '100%': { 
            transform: 'translateY(0)', 
            opacity: '1' 
          },
        },
        
        // Scale and fade in — good for modals and popovers
        scaleIn: {
          '0%': { 
            transform: 'scale(0.95)', 
            opacity: '0' 
          },
          '100%': { 
            transform: 'scale(1)', 
            opacity: '1' 
          },
        },
        
        // Horizontal shake — for form validation errors
        shake: {
          '0%, 100%': { transform: 'translateX(0)' },
          '10%, 30%, 50%, 70%, 90%': { transform: 'translateX(-4px)' },
          '20%, 40%, 60%, 80%': { transform: 'translateX(4px)' },
        },
        
        // Progress bar fill
        fillBar: {
          '0%': { width: '0%' },
          '100%': { width: 'var(--progress-width, 100%)' },
        },
      },
      
      // Define custom animations that reference the keyframes
      animation: {
        // Usage: animate-slide-up
        'slide-up': 'slideUp 0.5s ease-out',
        
        // Usage: animate-slide-down
        'slide-down': 'slideDown 0.5s ease-out',
        
        // Usage: animate-scale-in
        'scale-in': 'scaleIn 0.2s ease-out',
        
        // Usage: animate-shake
        'shake': 'shake 0.5s ease-in-out',
        
        // Usage: animate-fill-bar
        'fill-bar': 'fillBar 1s ease-out forwards',
        
        // Slower spin for larger loaders
        'spin-slow': 'spin 2s linear infinite',
      },
    },
  },
  plugins: [],
}
```

### Conditional Animation Classes with cn() Helper

When you need to conditionally apply animations based on component state, a utility function helps keep JSX clean. This is common for entrance animations, loading states, and interactive feedback.

```typescript
// [File: src/lib/utils.ts]
import { clsx, type ClassValue } from 'clsx';

/**
 * Combines clsx and tailwind-merge for conditional class handling.
 * This is essential for building flexible component APIs.
 */
export function cn(...inputs: ClassValue[]) {
  return clsx(inputs);
}
```

```tsx
// [File: src/components/ConditionalAnimation.tsx]
import React, { useState } from 'react';
import { cn } from '../lib/utils';

interface AnimatedCardProps {
  isVisible: boolean;
  children: React.ReactNode;
}

export function AnimatedCard({ isVisible, children }: AnimatedCardProps) {
  return (
    <div
      className={cn(
        // Base card styles
        'bg-white rounded-lg shadow-md p-6 transition-all duration-300',
        
        // Conditional animation — only animate when visible
        // The 'animate-slide-up' class only applies when isVisible is true
        isVisible && 'animate-slide-up',
        
        // When not visible, ensure it's hidden (for conditional rendering)
        !isVisible && 'opacity-0'
      )}
    >
      {children}
    </div>
  );
}

interface LoadingButtonProps {
  isLoading: boolean;
  children: React.ReactNode;
  onClick: () => void;
}

/**
 * A button that shows a spinner animation when loading.
 * Demonstrates conditional animation based on state.
 */
export function LoadingButton({ 
  isLoading, 
  children, 
  onClick 
}: LoadingButtonProps) {
  return (
    <button
      onClick={onClick}
      disabled={isLoading}
      className={cn(
        // Base button styles
        'px-4 py-2 rounded-lg font-medium transition-colors',
        
        // Color variants
        'bg-blue-600 text-white hover:bg-blue-700',
        'disabled:bg-blue-400 disabled:cursor-not-allowed',
        
        // When loading, add the spin animation to an inner element
        // The button itself doesn't spin, but we show a spinner
        isLoading && 'relative'
      )}
    >
      {/* 
       * Using absolute positioning to overlay the spinner
       * on top of the button text when loading
       */}
      <span className={cn(
        isLoading && 'invisible'
      )}>
        {children}
      </span>
      
      {isLoading && (
        <span 
          className="absolute inset-0 flex items-center justify-center"
          aria-hidden="true"
        >
          {/* Built-in Tailwind spin animation */}
          <svg 
            className="animate-spin h-5 w-5" 
            xmlns="http://www.w3.org/2000/svg" 
            fill="none" 
            viewBox="0 0 24 24"
          >
            <circle 
              className="opacity-25" 
              cx="12" 
              cy="12" 
              r="10" 
              stroke="currentColor" 
              strokeWidth="4" 
            />
            <path 
              className="opacity-75" 
              fill="currentColor" 
              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" 
            />
          </svg>
        </span>
      )}
    </button>
  );
}
```

### Respecting prefers-reduced-motion in Tailwind

Tailwind provides the `motion-safe:` variant to conditionally apply animations only when the user hasn't requested reduced motion. This is essential for accessibility compliance.

```tsx
// [File: src/components/AccessibleAnimations.tsx]
import React from 'react';

/**
 * Demonstrates how to use Tailwind's motion-safe variant
 * to respect prefers-reduced-motion accessibility setting.
 */

export function AccessibleToast({ message, isVisible }: { 
  message: string; 
  isVisible: boolean;
}) {
  return (
    <div
      role="alert"
      className={`
        fixed bottom-4 right-4 p-4 rounded-lg shadow-lg
        bg-gray-900 text-white
        transform transition-all duration-300
        
        /* Animation that respects reduced motion preference */
        ${isVisible 
          ? 'translate-y-0 opacity-100' 
          : 'translate-y-4 opacity-0'
        }
        
        /* 
         * motion-safe: only applies animations when the user 
         * has NOT set prefers-reduced-motion: reduce
         * 
         * For users who prefer reduced motion:
         * - The transition still applies (smoothness)
         * - But the transform/opacity changes happen instantly
         */
        motion-safe:animate-slide-up
        motion-safe:transition-transform
      `}
    >
      {message}
    </div>
  );
}

export function AccessibleSkeleton() {
  return (
    <div className="space-y-3">
      {/* 
       * Using animate-pulse with motion-safe wrapper:
       * - Users with motion preferences: static gray background
       * - Others: pulsing animation
       */}
      <div className="h-4 bg-gray-200 rounded motion-safe:animate-pulse w-3/4" />
      <div className="h-4 bg-gray-200 rounded motion-safe:animate-pulse w-1/2" />
      <div className="h-4 bg-gray-200 rounded motion-safe:animate-pulse w-5/6" />
    </div>
  );
}

/**
 * For more complex scenarios, you might want to use CSS directly
 * with @media (prefers-reduced-motion: reduce)
 */
export function ComplexAccessibleAnimation() {
  return (
    <div className="
      /* Default animation for normal users */
      animate-spin
      
      /* 
       * Override to disable for reduced motion users
       * Using arbitrary value with @media inside style tag
       */
      [@media(prefers-reduced-motion:reduce)]:animate-none
      
      /* Alternatively, use Tailwind's motion-reduce variant */
      motion-reduce:animate-none
    ">
      {/* Content */}
    </div>
  );
}
```

## Common Mistakes

### ❌ Not Using motion-safe Variant / ✅ Fix

```tsx
// ❌ WRONG — Animation plays even for users who prefer reduced motion
function BadToast({ show }) {
  return (
    <div className={`
      fixed bottom-4 right-4
      animate-slide-up  /* Always animates! */
      ${show ? 'opacity-100' : 'opacity-0'}
    `}>
      Message
    </div>
  );
}

// ✅ CORRECT — Animation only runs when motion is acceptable
function GoodToast({ show }) {
  return (
    <div className={`
      fixed bottom-4 right-4
      motion-safe:animate-slide-up  /* Only animates if user OK with it */
      ${show ? 'opacity-100' : 'opacity-0'}
    `}>
      Message
    </div>
  );
}
```

### ❌ Overriding Built-in Animations Incorrectly / ✅ Fix

```tsx
// ❌ WRONG — Trying to override animation-duration inline doesn't work well
function BadSpinner() {
  // This actually works but is inconsistent with Tailwind's design
  return (
    <div className="animate-spin" style={{ animationDuration: '0.5s' }}>
      Spinner
    </div>
  );
}

// ✅ CORRECT — Define custom animation duration in config or use utility
function GoodSpinner() {
  // Create a faster spin in tailwind.config.js:
  // animation: { 'spin-fast': 'spin 0.5s linear infinite' }
  return (
    <div className="animate-spin-fast">
      Spinner
    </div>
  );
}

// OR use arbitrary values (Tailwind v3.4+):
function AlsoGoodSpinner() {
  return (
    <div className="animate-[spin_0.5s_linear_infinite]">
      Spinner
    </div>
  );
}
```

## Real-World Example: Complete Toast Notification System

This example demonstrates loading buttons, toast notifications, and progress bars—all using only Tailwind animation utilities.

```tsx
// [File: src/components/Toast/ToastContainer.tsx]
import React, { useState, useEffect } from 'react';
import { cn } from '../../lib/utils';

type ToastType = 'success' | 'error' | 'info' | 'warning';

interface Toast {
  id: string;
  message: string;
  type: ToastType;
}

/**
 * Toast notification system using Tailwind animations.
 * All animations respect prefers-reduced-motion.
 */
export function ToastContainer() {
  const [toasts, setToasts] = useState<Toast[]>([]);

  // Add a new toast
  const addToast = (message: string, type: ToastType) => {
    const id = Math.random().toString(36).substr(2, 9);
    setToasts(prev => [...prev, { id, message, type }]);
    
    // Auto-remove after 5 seconds
    setTimeout(() => {
      setToasts(prev => prev.filter(t => t.id !== id));
    }, 5000);
  };

  // Demo functions
  const showSuccess = () => addToast('Item saved successfully!', 'success');
  const showError = () => addToast('Failed to save changes', 'error');

  return (
    <div className="p-8 space-y-8">
      {/* Demo buttons */}
      <div className="flex gap-4">
        <DemoButton onClick={showSuccess} variant="success">
          Show Success Toast
        </DemoButton>
        <DemoButton onClick={showError} variant="error">
          Show Error Toast
        </DemoButton>
      </div>

      {/* Toast container — fixed position */}
      <div className="fixed bottom-4 right-4 space-y-2 z-50">
        {toasts.map(toast => (
          <ToastItem 
            key={toast.id} 
            toast={toast} 
            onDismiss={() => setToasts(prev => prev.filter(t => t.id !== toast.id))}
          />
        ))}
      </div>
    </div>
  );
}

function DemoButton({ 
  children, 
  onClick, 
  variant 
}: { 
  children: React.ReactNode; 
  onClick: () => void;
  variant: 'success' | 'error';
}) {
  return (
    <button
      onClick={onClick}
      className={cn(
        'px-4 py-2 rounded-lg font-medium transition-colors',
        variant === 'success' 
          ? 'bg-green-600 hover:bg-green-700 text-white' 
          : 'bg-red-600 hover:bg-red-700 text-white'
      )}
    >
      {children}
    </button>
  );
}

function ToastItem({ 
  toast, 
  onDismiss 
}: { 
  toast: Toast; 
  onDismiss: () => void;
}) {
  const colors = {
    success: 'bg-green-600',
    error: 'bg-red-600',
    info: 'bg-blue-600',
    warning: 'bg-yellow-600',
  };

  return (
    <div
      role="alert"
      className={cn(
        'flex items-center gap-3 px-4 py-3 rounded-lg shadow-lg text-white min-w-[300px]',
        colors[toast.type],
        
        /* Entrance animation using motion-safe */
        'motion-safe:animate-slide-up',
        
        /* Transition for smooth appearance */
        'transition-all duration-300'
      )}
    >
      {/* Icon based on type */}
      <ToastIcon type={toast.type} />
      
      {/* Message */}
      <span className="flex-1">{toast.message}</span>
      
      {/* Dismiss button */}
      <button
        onClick={onDismiss}
        className="p-1 hover:bg-white/20 rounded transition-colors"
        aria-label="Dismiss"
      >
        <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>
  );
}

function ToastIcon({ type }: { type: ToastType }) {
  const icons = {
    success: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
      </svg>
    ),
    error: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
      </svg>
    ),
    info: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
    ),
    warning: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
      </svg>
    ),
  };
  
  return icons[type];
}

// Progress bar example
export function ProgressBar({ progress }: { progress: number }) {
  return (
    <div className="w-full space-y-2">
      <div className="flex justify-between text-sm text-gray-600">
        <span>Progress</span>
        <span>{progress}%</span>
      </div>
      <div className="h-2 bg-gray-200 rounded-full overflow-hidden">
        <div 
          className={cn(
            'h-full bg-blue-600 rounded-full',
            'transition-all duration-500', // Smooth transition for progress changes
            
            // Animate width when progress changes
            // Uses CSS custom property for the width
            '[--progress-width:100%]',
            `w-[${progress}%]`
          )}
          style={{ width: `${progress}%` }}
        />
      </div>
    </div>
  );
}
```

## Key Takeaways

- Use built-in `animate-spin`, `animate-bounce`, `animate-pulse`, and `animate-ping` for common patterns
- Extend `tailwind.config.js` with custom `keyframes` and `animation` for more control
- Always use `motion-safe:` variant to respect `prefers-reduced-motion` accessibility preference
- Use the `cn()` helper from clsx/tailwind-merge for conditional animation classes
- Combine Tailwind's transition utilities with animations for the smoothest effects

## What's Next

Continue to [Server Actions](../13-nextjs/03-nextjs-features/04-server-actions.md) in the Next.js module to learn about Next.js 14's revolutionary server action feature for building full-stack React applications.