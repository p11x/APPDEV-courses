# Framer Motion Setup

## Overview
Framer Motion is a powerful animation library for React that provides declarative, physics-based animations. It integrates seamlessly with React's component model and makes creating complex animations simple. This guide covers setting up Framer Motion and creating your first animations.

## Prerequisites
- React basics
- Understanding of React components
- Familiarity with CSS animations

## Core Concepts

### Installation

```bash
npm install framer-motion
# or
yarn add framer-motion
```

### Basic Motion Components

```tsx
// [File: app/components/MotionButton.tsx]
'use client';

import { motion } from 'framer-motion';

export default function MotionButton({ children, onClick }: { children: React.ReactNode, onClick?: () => void }) {
  return (
    <motion.button
      whileHover={{ scale: 1.05 }}
      whileTap={{ scale: 0.95 }}
      onClick={onClick}
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.3 }}
    >
      {children}
    </motion.button>
  );
}
```

### Motion Variants

```tsx
// [File: app/components/AnimatedCard.tsx]
'use client';

import { motion } from 'framer-motion';

const cardVariants = {
  hidden: { 
    opacity: 0, 
    y: 20 
  },
  visible: { 
    opacity: 1, 
    y: 0,
    transition: {
      duration: 0.5,
      ease: 'easeOut'
    }
  },
  hover: {
    scale: 1.02,
    boxShadow: '0 10px 30px rgba(0,0,0,0.15)'
  }
};

export default function AnimatedCard({ children }: { children: React.ReactNode }) {
  return (
    <motion.div
      variants={cardVariants}
      initial="hidden"
      animate="visible"
      whileHover="hover"
    >
      {children}
    </motion.div>
  );
}
```

### Staggered Animations

```tsx
// [File: app/components/StaggeredList.tsx]
'use client';

import { motion } from 'framer-motion';

const container = {
  hidden: { opacity: 0 },
  show: {
    opacity: 1,
    transition: {
      staggerChildren: 0.1
    }
  }
};

const item = {
  hidden: { opacity: 0, x: -20 },
  show: { opacity: 1, x: 0 }
};

export default function StaggeredList({ items }: { items: string[] }) {
  return (
    <motion.ul
      variants={container}
      initial="hidden"
      animate="show"
    >
      {items.map((item, index) => (
        <motion.li key={index} variants={item}>
          {item}
        </motion.li>
      ))}
    </motion.ul>
  );
}
```

### Gesture Animations

```tsx
// [File: app/components/DraggableCard.tsx]
'use client';

import { motion } from 'framer-motion';

export default function DraggableCard() {
  return (
    <motion.div
      drag
      dragConstraints={{ left: -100, right: 100, top: -100, bottom: 100 }}
      dragElastic={0.2}
      whileDrag={{ scale: 1.1 }}
    >
      Drag me!
    </motion.div>
  );
}
```

## Key Takeaways
- Use motion.div, motion.button, etc. instead of regular elements
- Use initial, animate, exit for animation states
- Use variants for reusable animation definitions
- Use whileHover, whileTap, whileFocus for gesture animations
- Use drag prop for draggable elements

## What's Next
Continue to [Variants and Orchestration](02-variants-and-orchestration.md) to learn about complex animation sequences.