# Keyframe Animations

## Overview
CSS keyframe animations provide granular control over animation sequences with multiple steps, perfect for complex animations that transitions can't handle.

## Prerequisites
- CSS fundamentals
- Animation basics

## Core Concepts

### Basic Keyframe Animation

```css
/* [File: src/styles/animations.css] */
@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(-100%);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: 0.5;
  }
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

/* Using the animations */
.slide-in {
  animation: slideIn 0.5s ease-out forwards;
}

.pulse {
  animation: pulse 2s ease-in-out infinite;
}

.spinner {
  animation: spin 1s linear infinite;
}
```

### Multi-Step Animation

```css
/* [File: src/styles/complex-animation.css] */
@keyframes complexEntrance {
  0% {
    opacity: 0;
    transform: scale(0.9) translateY(20px);
  }
  50% {
    opacity: 1;
    transform: scale(1.05) translateY(-5px);
  }
  100% {
    opacity: 1;
    transform: scale(1) translateY(0);
  }
}

@keyframes colorCycle {
  0% {
    background-color: #3498db;
  }
  33% {
    background-color: #e74c3c;
  }
  66% {
    background-color: #2ecc71;
  }
  100% {
    background-color: #3498db;
  }
}

.entrance-animation {
  animation: complexEntrance 0.8s ease-out forwards;
}

.color-cycle {
  animation: colorCycle 6s ease infinite;
}
```

### React Component with Keyframes

```tsx
// [File: src/components/LoadingSpinner.tsx]
import React from 'react';
import './spinner.css';

export function LoadingSpinner({ size = 'medium' }) {
  return (
    <div className={`spinner spinner-${size}`} role="status" aria-label="Loading">
      <div className="spinner-circle"></div>
    </div>
  );
}
```

```css
/* [File: src/components/spinner.css] */
@keyframes spinnerRotate {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.spinner-circle {
  border: 3px solid #f3f3f3;
  border-top: 3px solid #3498db;
  border-radius: 50%;
  animation: spinnerRotate 1s linear infinite;
}

.spinner-small .spinner-circle {
  width: 20px;
  height: 20px;
}

.spinner-medium .spinner-circle {
  width: 40px;
  height: 40px;
}

.spinner-large .spinner-circle {
  width: 60px;
  height: 60px;
}
```

## Key Takeaways
- Keyframes offer multi-step control
- Use animation-fill-mode: forwards to retain final state
- Control timing with keyframe percentages

## What's Next
Continue to [Animating with Tailwind](05-animating-with-tailwind.md) for Tailwind animations.