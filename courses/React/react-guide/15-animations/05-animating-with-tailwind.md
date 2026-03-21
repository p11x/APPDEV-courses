# Animating with Tailwind

## Overview
Tailwind CSS provides utility classes for common animations and supports custom animations through configuration. Learn how to animate with Tailwind.

## Prerequisites
- Tailwind CSS knowledge
- Animation basics

## Core Concepts

### Built-in Animations

```tsx
// [File: src/components/TailwindAnimations.tsx]
import React, { useState } from 'react';

export function TailwindAnimations() {
  const [isLoading, setIsLoading] = useState(false);

  return (
    <div className="space-y-4">
      {/* Spin animation - built-in */}
      <button 
        disabled={isLoading}
        className={`px-4 py-2 bg-blue-500 text-white rounded ${isLoading ? 'animate-spin' : ''}`}
        onClick={() => setIsLoading(!isLoading)}
      >
        {isLoading ? 'Loading...' : 'Start Loading'}
      </button>

      {/* Ping animation - for notifications */}
      <div className="relative">
        <span className="animate-ping absolute inline-flex h-3 w-3 rounded-full bg-red-400 opacity-75"></span>
        <span className="relative inline-flex rounded-full h-3 w-3 bg-red-500"></span>
      </div>

      {/* Pulse animation */}
      <div className="animate-pulse bg-gray-200 h-4 w-32 rounded"></div>

      {/* Bounce animation */}
      <button className="animate-bounce bg-purple-500 text-white px-4 py-2 rounded">
        Scroll Down
      </button>
    </div>
  );
}
```

### Custom Animations in Tailwind Config

```javascript
// [File: tailwind.config.js]
module.exports = {
  theme: {
    extend: {
      animation: {
        'slide-up': 'slideUp 0.5s ease-out',
        'slide-down': 'slideDown 0.5s ease-out',
        'fade-in': 'fadeIn 0.3s ease-in-out',
        'scale-in': 'scaleIn 0.2s ease-out',
        'shake': 'shake 0.5s ease-in-out',
      },
      keyframes: {
        slideUp: {
          '0%': { transform: 'translateY(20px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
        slideDown: {
          '0%': { transform: 'translateY(-20px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        scaleIn: {
          '0%': { transform: 'scale(0.9)', opacity: '0' },
          '100%': { transform: 'scale(1)', opacity: '1' },
        },
        shake: {
          '0%, 100%': { transform: 'translateX(0)' },
          '25%': { transform: 'translateX(-5px)' },
          '75%': { transform: 'translateX(5px)' },
        },
      },
    },
  },
};
```

### Using Custom Animations

```tsx
// [File: src/components/AnimatedCard.tsx]
import React, { useState } from 'react';

export function AnimatedCard() {
  const [isVisible, setIsVisible] = useState(false);

  return (
    <div className="space-y-4">
      <button 
        onClick={() => setIsVisible(!isVisible)}
        className="px-4 py-2 bg-blue-500 text-white rounded"
      >
        Toggle Animation
      </button>

      <div 
        className={`p-4 bg-white rounded shadow ${
          isVisible 
            ? 'animate-scale-in' 
            : 'animate-fade-out'
        }`}
      >
        <h3>Animated Card</h3>
        <p>This card animates on toggle</p>
      </div>
    </div>
  );
}
```

## Key Takeaways
- Use built-in animate-spin, animate-pulse, animate-bounce
- Add custom animations in tailwind.config.js
- Use transition utilities for simple animations

## What's Next
This completes all modules in the React development guide!