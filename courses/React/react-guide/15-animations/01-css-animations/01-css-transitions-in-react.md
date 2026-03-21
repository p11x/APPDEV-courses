# CSS Transitions in React

## Overview
CSS transitions provide a simple way to animate property changes in React applications. Unlike JavaScript animations, CSS transitions are hardware-accelerated and run on the compositor thread, making them performant. This guide covers how to implement CSS transitions in React, from simple hover effects to complex state-driven animations.

## Prerequisites
- Basic CSS knowledge
- Understanding of React state
- Familiarity with className manipulation

## Core Concepts

### Basic Transitions

```css
/* [File: styles/button.css] */
.button {
  background-color: #007bff;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  
  /* Transition declaration */
  transition: background-color 0.3s ease;
}

.button:hover {
  background-color: #0056b3;
}

.button:active {
  transform: scale(0.98);
}
```

```tsx
// [File: app/components/TransitionButton.tsx]
export default function TransitionButton({ 
  children, 
  onClick 
}: { 
  children: React.ReactNode; 
  onClick?: () => void; 
}) {
  return (
    <button className="button" onClick={onClick}>
      {children}
    </button>
  );
}
```

### State-Driven Transitions

```tsx
// [File: app/components/TransitionSidebar.tsx]
'use client';

import { useState } from 'react';
import styles from './Sidebar.module.css';

export default function TransitionSidebar() {
  const [isOpen, setIsOpen] = useState(false);
  
  return (
    <>
      <button 
        onClick={() => setIsOpen(!isOpen)}
        className="toggle-button"
      >
        {isOpen ? 'Close' : 'Open'} Menu
      </button>
      
      <aside className={`${styles.sidebar} ${isOpen ? styles.open : ''}`}>
        <nav>
          <a href="/">Home</a>
          <a href="/about">About</a>
          <a href="/contact">Contact</a>
        </nav>
      </aside>
    </>
  );
}
```

```css
/* [File: app/components/Sidebar.module.css] */
.sidebar {
  position: fixed;
  left: 0;
  top: 0;
  bottom: 0;
  width: 250px;
  background: #333;
  color: white;
  
  /* Transform-based transition - more performant */
  transform: translateX(-100%);
  transition: transform 0.3s ease-in-out;
}

.sidebar.open {
  transform: translateX(0);
}
```

### Enter/Exit Transitions

```tsx
// [File: app/components/FadeTransition.tsx]
'use client';

import { useState, useEffect } from 'react';
import styles from './Fade.module.css';

export default function FadeTransition({ 
  isVisible, 
  children 
}: { 
  isVisible: boolean; 
  children: React.ReactNode; 
}) {
  const [show, setShow] = useState(isVisible);
  
  useEffect(() => {
    if (isVisible) {
      setShow(true);
    } else {
      // Wait for transition before unmounting
      const timer = setTimeout(() => setShow(false), 300);
      return () => clearTimeout(timer);
    }
  }, [isVisible]);
  
  if (!show) return null;
  
  return (
    <div className={`${styles.fade} ${isVisible ? styles.visible : styles.hidden}`}>
      {children}
    </div>
  );
}
```

```css
/* [File: app/components/Fade.module.css] */
.fade {
  transition: opacity 0.3s ease, visibility 0.3s ease;
}

.visible {
  opacity: 1;
  visibility: visible;
}

.hidden {
  opacity: 0;
  visibility: hidden;
}
```

### Transition Properties

```css
/* [File: styles/transitions.css] */
/* Basic transition shorthand */
.element {
  transition: property duration timing-function delay;
  
  /* Examples: */
  transition: all 0.3s ease;
  transition: background-color 0.2s ease-in-out;
  transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

/* Multiple properties */
.element {
  transition: 
    background-color 0.3s ease,
    transform 0.2s ease-out,
    opacity 0.15s linear;
}

/* Common timing functions */
.ease { transition-timing-function: ease; }
.ease-in { transition-timing-function: ease-in; }
.ease-out { transition-timing-function: ease-out; }
.ease-in-out { transition-timing-function: ease-in-out; }
.linear { transition-timing-function: linear; }
/* Custom cubic-bezier */
.custom { transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1); }
```

### Prefers Reduced Motion

```css
/* [File: styles/animations.css] */
.element {
  transition: transform 0.3s ease;
}

@media (prefers-reduced-motion: reduce) {
  .element {
    transition: none;
  }
}
```

## Common Mistakes

### Mistake 1: Animating layout properties
```css
/* ❌ WRONG - Animating layout properties causes reflow */
.element {
  transition: width 0.3s, height 0.3s; /* Causes reflow! */
}

/* ✅ CORRECT - Use transform for better performance */
.element {
  transition: transform 0.3s; /* Runs on compositor thread */
}
```

### Mistake 2: Not handling enter/exit
```tsx
// ❌ WRONG - Abrupt unmounting
{isVisible && <div className="modal">Content</div>}

// ✅ CORRECT - Smooth enter/exit
{isVisible && (
  <div className="modal fade-in">Content</div>
)}
```

## Real-World Example

Complete animated modal:

```tsx
// [File: app/components/AnimatedModal.tsx]
'use client';

import { useEffect, useState } from 'react';
import styles from './Modal.module.css';

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
}

export default function AnimatedModal({ 
  isOpen, 
  onClose, 
  title, 
  children 
}: ModalProps) {
  const [isVisible, setIsVisible] = useState(isOpen);
  
  useEffect(() => {
    if (isOpen) {
      setIsVisible(true);
    } else {
      const timer = setTimeout(() => setIsVisible(false), 300);
      return () => clearTimeout(timer);
    }
  }, [isOpen]);
  
  if (!isVisible) return null;
  
  return (
    <div 
      className={`${styles.overlay} ${isOpen ? styles.visible : ''}`}
      onClick={onClose}
    >
      <div 
        className={`${styles.modal} ${isOpen ? styles.open : ''}`}
        onClick={e => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
        aria-labelledby="modal-title"
      >
        <header className={styles.header}>
          <h2 id="modal-title">{title}</h2>
          <button 
            onClick={onClose}
            aria-label="Close modal"
            className={styles.closeButton}
          >
            ×
          </button>
        </header>
        <div className={styles.content}>
          {children}
        </div>
      </div>
    </div>
  );
}
```

```css
/* [File: app/components/Modal.module.css] */
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  
  opacity: 0;
  visibility: hidden;
  transition: opacity 0.3s ease, visibility 0.3s ease;
}

.overlay.visible {
  opacity: 1;
  visibility: visible;
}

.modal {
  background: white;
  border-radius: 8px;
  max-width: 500px;
  width: 90%;
  max-height: 80vh;
  overflow: auto;
  
  transform: scale(0.9) translateY(20px);
  opacity: 0;
  transition: transform 0.3s ease, opacity 0.3s ease;
}

.modal.open {
  transform: scale(1) translateY(0);
  opacity: 1;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 20px;
  border-bottom: 1px solid #eee;
}

.closeButton {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  padding: 4px 8px;
}

.content {
  padding: 20px;
}

@media (prefers-reduced-motion: reduce) {
  .overlay,
  .modal {
    transition: none;
  }
}
```

## Key Takeaways
- Use CSS transitions for simple property changes
- Prefer transform over layout properties for better performance
- Handle enter/exit transitions with state and timeouts
- Use prefers-reduced-motion for accessibility
- Combine multiple transitions with comma-separated values

## What's Next
Continue to [Framer Motion Setup](02-framer-motion/01-framer-motion-setup.md) to learn about the Framer Motion animation library for React.