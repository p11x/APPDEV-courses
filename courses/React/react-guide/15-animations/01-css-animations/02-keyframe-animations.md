# CSS Keyframe Animations in React

## Overview
CSS keyframe animations provide granular control over animation sequences with multiple steps, perfect for complex animations that simple transitions cannot handle. Unlike transitions, keyframes allow you to define intermediate states at specific percentage points, enabling more sophisticated motion effects.

## Prerequisites
- CSS fundamentals
- React component basics
- Understanding of CSS Modules or styled-components

## Core Concepts

### @keyframes Syntax: From/To vs Percentage Stops

The @keyframes rule defines the sequence of animation frames. You can use either the `from`/`to` shorthand (0% and 100%) or explicit percentage stops for more complex sequences.

```css
/* [File: src/styles/animations.css] */

/* Using from/to — simplest case for two-state animations */
@keyframes slideInFromLeft {
  from {
    transform: translateX(-100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

/* Using percentage stops — for multi-step animations */
@keyframes complexEntrance {
  0% {
    opacity: 0;
    transform: translateY(20px) scale(0.95);
  }
  60% {
    opacity: 1;
    transform: translateY(-5px) scale(1.02);
  }
  100% {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

/* Animation with pause at 50% */
@keyframes colorShift {
  0%, 100% {
    background-color: #3498db;
    transform: rotate(0deg);
  }
  25%, 75% {
    background-color: #e74c3c;
    transform: rotate(180deg);
  }
  50% {
    background-color: #2ecc71;
    transform: rotate(360deg);
  }
}
```

### Animation Shorthand Properties

The `animation` CSS property is a shorthand for setting multiple animation properties in one declaration. Understanding each component helps you control timing precisely.

```css
/* [File: src/styles/animation-props.css] */

/* 
 * Full animation shorthand:
 * animation: name duration easing-function delay 
 *            iteration-count direction fill-mode play-state;
 */

/* Example: fade in with bounce, 0.5s duration, ease-out timing */
@keyframes bounceIn {
  0% { transform: scale(0.3); opacity: 0; }
  50% { transform: scale(1.05); }
  70% { transform: scale(0.9); }
  100% { transform: scale(1); opacity: 1; }
}

.bounce-in {
  /* name | duration | easing | delay | iterations | direction | fill-mode */
  animation: bounceIn 0.6s cubic-bezier(0.68, -0.55, 0.265, 1.55) 
             0s 1 normal both;
}

/* Different fill modes explained */
.fill-forwards {
  animation: fadeIn 0.5s ease forwards;
  /* forwards: keeps the final state (100%) after animation ends */
}

.fill-backwards {
  animation: fadeIn 0.5s ease backwards;
  /* backwards: applies the start state (0%) before animation begins */
}

.fill-both {
  animation: fadeIn 0.5s ease both;
  /* both: applies both 0% before and 100% after */
}
```

### Defining Keyframes in CSS Modules with :local Scope

CSS Modules automatically scopes styles, so keyframes need to be wrapped with `:global` or `:local` to ensure they're accessible. This prevents keyframe name collisions between components.

```css
/* [File: src/components/Loader/Loader.module.css] */

/* Using :local to scope the keyframe to this module */
:local {
  @keyframes spin {
    from { transform: rotate(0deg); }
    to { transform: rotate(360deg); }
  }
  
  @keyframes pulse {
    0%, 100% { opacity: 1; transform: scale(1); }
    50% { opacity: 0.5; transform: scale(0.95); }
  }
}

.loader {
  /* Referencing the local keyframe name */
  animation: spin 1s linear infinite;
}

.pulsing {
  animation: pulse 2s ease-in-out infinite;
}

/* Conditional animations based on props can be handled in React */
.active {
  animation: pulse 0.5s ease-in-out;
}
```

```tsx
// [File: src/components/Loader/Loader.tsx]
import React from 'react';
import styles from './Loader.module.css';

interface LoaderProps {
  type: 'spinner' | 'pulsing';
  size?: 'small' | 'medium' | 'large';
}

export function Loader({ type, size = 'medium' }: LoaderProps) {
  // Determine CSS class based on props — animation type changes the keyframe used
  const animationClass = type === 'spinner' ? styles.loader : styles.pulsing;
  
  return (
    <div 
      className={`${animationClass} ${styles[size]}`}
      role="status"
      aria-label={type === 'spinner' ? 'Loading' : 'Updating'}
    >
      <div className={styles.innerCircle} />
    </div>
  );
}
```

### Defining Keyframes in styled-components with the keyframes Helper

styled-components provides a `keyframes` helper that generates a unique name for your animation, preventing conflicts and working seamlessly with SSR.

```typescript
// [File: src/components/Button/styled.tsx]
import styled, { keyframes } from 'styled-components';

// keyframes generates a unique, scoped animation name automatically
const shimmer = keyframes`
  0% {
    background-position: -200% 0;
  }
  100% {
    background-position: 200% 0;
  }
`;

const checkmarkDraw = keyframes`
  0% {
    stroke-dashoffset: 100;
  }
  100% {
    stroke-dashoffset: 0;
  }
`;

const pulseScale = keyframes`
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
`;

// Using the animations in styled components
export const ShimmerBox = styled.div`
  background: linear-gradient(
    90deg,
    #f0f0f0 25%,
    #e0e0e0 50%,
    #f0f0f0 75%
  );
  background-size: 200% 100%;
  animation: ${shimmer} 1.5s ease-in-out infinite;
  height: 20px;
  border-radius: 4px;
`;

export const SuccessIcon = styled.svg`
  path {
    stroke-dasharray: 100;
    stroke-dashoffset: 100;
    animation: ${checkmarkDraw} 0.5s ease forwards;
  }
`;

export const PulseButton = styled.button`
  animation: ${pulseScale} 2s ease-in-out infinite;
  padding: 12px 24px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  
  &:hover {
    animation-play-state: paused;
  }
`;
```

### Respecting prefers-reduced-motion: Always Wrap Animations

Accessibility is critical. The `prefers-reduced-motion` media query allows users to request minimal animation. Always wrap your animations with this check to respect user preferences.

```css
/* [File: src/styles/reduced-motion.css] */

/* Default animations for users who haven't expressed a preference */
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

.animate-fade {
  animation: fadeIn 0.3s ease-out;
}

.animate-slide {
  animation: slideUp 0.4s ease-out;
}

/* Disable animations for users who prefer reduced motion */
@media (prefers-reduced-motion: reduce) {
  .animate-fade,
  .animate-slide {
    animation: none;
    opacity: 1;
    transform: none;
  }
}
```

```tsx
// [File: src/hooks/usePrefersReducedMotion.ts]
import { useEffect, useState } from 'react';

/**
 * Hook to detect if the user prefers reduced motion.
 * Returns true if prefers-reduced-motion: reduce is set.
 * This is important for accessibility — always check this before
 * triggering programmatic animations.
 */
export function usePrefersReducedMotion(): boolean {
  const [prefersReducedMotion, setPrefersReducedMotion] = useState(false);

  useEffect(() => {
    // Check if the media query is supported and get the current value
    const mediaQuery = window.matchMedia('(prefers-reduced-motion: reduce)');
    setPrefersReducedMotion(mediaQuery.matches);

    // Listen for changes in the user's preference
    const handler = (event: MediaQueryListEvent) => {
      setPrefersReducedMotion(event.matches);
    };

    mediaQuery.addEventListener('change', handler);
    return () => mediaQuery.removeEventListener('change', handler);
  }, []);

  return prefersReducedMotion;
}
```

```tsx
// [File: src/components/AnimatedCard.tsx]
import React from 'react';
import { usePrefersReducedMotion } from '../hooks/usePrefersReducedMotion';
import styles from './AnimatedCard.module.css';

interface AnimatedCardProps {
  children: React.ReactNode;
  isVisible: boolean;
}

export function AnimatedCard({ children, isVisible }: AnimatedCardProps) {
  // Get the user's motion preference — critical for accessibility
  const prefersReducedMotion = usePrefersReducedMotion();
  
  // Determine which class to apply based on visibility and preference
  // If reduced motion is preferred, skip animation entirely
  const animationClass = prefersReducedMotion 
    ? '' 
    : isVisible 
      ? styles.visible 
      : styles.hidden;

  return (
    <div className={`${styles.card} ${animationClass}`}>
      {children}
    </div>
  );
}
```

## Common Mistakes

### ❌ Not Respecting prefers-reduced-motion / ✅ Fix

```tsx
// ❌ WRONG — Always animating, ignoring user accessibility preferences
function BadAnimatedComponent() {
  const [isVisible, setIsVisible] = useState(false);
  
  return (
    <div 
      className={isVisible ? 'animate-slide-in' : ''}
      onClick={() => setIsVisible(true)}
    >
      Content
    </div>
  );
}

// ✅ CORRECT — Checking user preference before animating
function GoodAnimatedComponent() {
  const [isVisible, setIsVisible] = useState(false);
  const prefersReducedMotion = usePrefersReducedMotion();
  
  return (
    <div 
      className={
        isVisible 
          ? prefersReducedMotion 
            ? 'visible-instant'  // Skip animation
            : 'animate-slide-in'  // Animate
          : ''
      }
      onClick={() => setIsVisible(true)}
    >
      Content
    </div>
  );
}
```

### ❌ Forgetting Animation Fill Mode / ✅ Fix

```css
/* ❌ WRONG — Element disappears after animation because fill-mode isn't set */
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

.fade-in-broken {
  animation: fadeIn 0.5s ease;
  /* Element becomes invisible again after animation ends! */
}

/* ✅ CORRECT — Using fill-mode to retain the final state */
.fade-in-fixed {
  animation: fadeIn 0.5s ease forwards;
  /* forwards keeps the element visible after animation completes */
}

/* Also consider: animation-fill-mode: both; 
 * This applies the 0% state BEFORE the animation starts
 * and the 100% state AFTER it ends — often what you want
 */
```

### ❌ Using Incorrect Animation Timing / ✅ Fix

```tsx
// ❌ WRONG — Animation too fast, feels jarring to users
function BadLoadingSpinner() {
  return <div className="spinner" style={{ animationDuration: '0.1s' }} />;
}

// ✅ CORRECT — Slower, more comfortable animation duration
function GoodLoadingSpinner() {
  // 1s is a standard, comfortable spin duration
  return <div className="spinner" style={{ animationDuration: '1s' }} />;
}

/* General timing guidelines:
 * - Micro-interactions (hover): 150-200ms
 * - Standard transitions: 200-300ms
 * - Emphasis animations: 300-500ms
 * - Complex entrances: 400-800ms
 * - Loading spinners: 800ms-2s
 */
```

## Real-World Example: Skeleton Loader Shimmer + Success Checkmark

This example demonstrates two practical animations: a skeleton loader shimmer effect and a success checkmark draw-on animation using the stroke-dashoffset technique.

```tsx
// [File: src/components/SkeletonLoader/SkeletonLoader.tsx]
import React from 'react';
import styles from './SkeletonLoader.module.css';

interface SkeletonLoaderProps {
  lines?: number;
  width?: string;
}

/**
 * A skeleton loader displays placeholder content while data is loading.
 * The shimmer animation gives visual feedback that content is coming.
 */
export function SkeletonLoader({ lines = 3, width = '100%' }: SkeletonLoaderProps) {
  return (
    <div className={styles.container} role="status" aria-label="Loading content">
      {Array.from({ length: lines }).map((_, index) => (
        <div 
          key={index}
          className={styles.line}
          style={{ 
            width: index === lines - 1 ? '70%' : width 
            // Last line is shorter to simulate paragraph flow
          }}
        />
      ))}
    </div>
  );
}
```

```css
/* [File: src/components/SkeletonLoader/SkeletonLoader.module.css] */

/* Define the shimmer keyframe locally within this module */
:local {
  @keyframes shimmer {
    0% {
      /* Start with gradient positioned off to the left */
      background-position: -200% 0;
    }
    100% {
      /* End with gradient positioned off to the right */
      background-position: 200% 0;
    }
  }
}

.container {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.line {
  height: 16px;
  border-radius: 4px;
  
  /* The gradient creates the shimmer effect:
   * - Diagonal gradient from light to slightly darker gray
   * - Background size is 200% to allow movement across the element
   */
  background: linear-gradient(
    90deg,
    #f0f0f0 25%,
    #e8e8e8 50%,
    #f0f0f0 75%
  );
  background-size: 200% 100%;
  
  /* 
   * The animation moves the gradient across the element:
   * - name: shimmer (our keyframe)
   * - duration: 1.5s (smooth, not distracting)
   * - timing: ease-in-out (natural acceleration/deceleration)
   * - iteration: infinite (keeps running while loading)
   */
  animation: shimmer 1.5s ease-in-out infinite;
  
  /* Respect reduced motion preference */
  @media (prefers-reduced-motion: reduce) {
    animation: none;
    background: #f0f0f0;
  }
}
```

```tsx
// [File: src/components/SuccessCheckmark/SuccessCheckmark.tsx]
import React from 'react';
import styles from './SuccessCheckmark.module.css';

interface SuccessCheckmarkProps {
  show: boolean;
  size?: 'small' | 'medium' | 'large';
}

/**
 * A success checkmark that draws itself on using stroke-dasharray technique.
 * This creates a satisfying "hand-drawn" effect as the checkmark appears.
 */
export function SuccessCheckmark({ show, size = 'medium' }: SuccessCheckmarkProps) {
  if (!show) return null;
  
  // Map size prop to SVG viewBox dimensions
  const dimensions = {
    small: 24,
    medium: 48,
    large: 96
  };
  
  const dimension = dimensions[size];

  return (
    <div 
      className={`${styles.container} ${styles[size]}`}
      role="status"
      aria-label="Success"
    >
      <svg 
        viewBox={`0 0 ${dimension} ${dimension}`} 
        className={styles.svg}
      >
        {/* Circle outline */}
        <circle 
          className={styles.circle}
          cx={dimension / 2} 
          cy={dimension / 2} 
          r={dimension / 2 - 4}
        />
        {/* Checkmark path — uses stroke-dasharray for draw-on effect */}
        <path 
          className={styles.checkmark}
          d={getCheckmarkPath(size)}
          fill="none"
          stroke="currentColor"
          strokeWidth={size === 'small' ? 3 : 4}
          strokeLinecap="round"
          strokeLinejoin="round"
        />
      </svg>
    </div>
  );
}

// Helper to generate checkmark path based on size
function getCheckmarkPath(size: 'small' | 'medium' | 'large'): string {
  const d = {
    small: 'M5 12 l4 4 l8 -8',
    medium: 'M9 24 l8 8 l16 -16',
    large: 'M18 48 l16 16 l32 -32'
  };
  return d[size];
}
```

```css
/* [File: src/components/SuccessCheckmark/SuccessCheckmark.module.css] */

:local {
  @keyframes drawCircle {
    0% {
      stroke-dasharray: 100;
      stroke-dashoffset: 100;
    }
    100% {
      stroke-dasharray: 100;
      stroke-dashoffset: 0;
    }
  }
  
  @keyframes drawCheck {
    0% {
      stroke-dasharray: 50;
      stroke-dashoffset: 50;
    }
    100% {
      stroke-dasharray: 50;
      stroke-dashoffset: 0;
    }
  }
  
  @keyframes scaleIn {
    0% {
      transform: scale(0);
      opacity: 0;
    }
    50% {
      transform: scale(1.2);
    }
    100% {
      transform: scale(1);
      opacity: 1;
    }
  }
}

.container {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  animation: scaleIn 0.3s ease-out forwards;
}

.svg {
  color: #22c55e; /* Success green */
}

/* 
 * The stroke-dasharray technique works like this:
 * 1. Set stroke-dasharray equal to the path length (50 or 100)
 * 2. Initially offset by that same amount (making it invisible)
 * 3. Animate offset to 0 (making the line "draw" itself)
 * 
 * This creates a hand-drawn appearance that's visually appealing
 */

.circle {
  stroke: #22c55e;
  stroke-width: 3;
  stroke-dasharray: 100;
  stroke-dashoffset: 100;
  animation: drawCircle 0.4s ease forwards 0.1s;
  /* 0.1s delay starts the circle slightly before the checkmark */
  fill: none;
}

.checkmark {
  stroke-dasharray: 50;
  stroke-dashoffset: 50;
  animation: drawCheck 0.3s ease forwards 0.4s;
  /* 0.4s delay waits for the circle to complete first */
}

/* Size variants */
.small {
  width: 24px;
  height: 24px;
}

.medium {
  width: 48px;
  height: 48px;
}

.large {
  width: 96px;
  height: 96px;
}

@media (prefers-reduced-motion: reduce) {
  .container {
    animation: none;
  }
  
  .circle,
  .checkmark {
    animation: none;
    stroke-dashoffset: 0;
  }
}
```

## Key Takeaways

- Use `from`/`to` for simple two-state animations and percentages for complex multi-step sequences
- The animation shorthand follows: `name duration easing delay iteration direction fill-mode`
- Always wrap keyframes in `:local` for CSS Modules or use `keyframes` helper in styled-components
- Always check `prefers-reduced-motion` before running animations to respect accessibility preferences
- The stroke-dasharray/offset technique creates satisfying draw-on effects for checkmarks and icons

## What's Next

Continue to [Animating with Tailwind](03-animating-with-tailwind.md) to learn how to implement the same animations using Tailwind CSS utility classes.