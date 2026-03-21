# Gesture Animations

## Overview
Framer Motion provides powerful gesture-based animations that respond to user interactions like dragging, hovering, and tapping. This guide covers implementing drag, pan, and other gesture animations in React applications.

## Prerequisites
- Framer Motion basics
- Understanding of motion components

## Core Concepts

### Drag Animations

```tsx
// [File: app/components/DraggableItem.tsx]
'use client';

import { motion } from 'framer-motion';

export default function DraggableItem() {
  return (
    <motion.div
      drag
      dragConstraints={{ left: -100, right: 100, top: -100, bottom: 100 }}
      dragElastic={0.2}
      whileDrag={{ scale: 1.1, cursor: 'grabbing' }}
    >
      Drag me!
    </motion.div>
  );
}
```

### Pan Gestures

```tsx
// [File: app/components/PannableImage.tsx]
'use client';

import { motion, useMotionValue, useTransform } from 'framer-motion';
import { useRef } from 'react';

export default function PannableImage() {
  const ref = useRef(null);
  const x = useMotionValue(0);
  const rotate = useTransform(x, [-200, 200], [-30, 30]);
  
  return (
    <motion.div
      ref={ref}
      drag="x"
      dragConstraints={ref}
      style={{ x, rotate }}
      whileHover={{ scale: 1.05 }}
    >
      Pan image horizontally
    </motion.div>
  );
}
```

## Key Takeaways
- Use drag prop for draggable elements
- Use dragConstraints to limit drag range
- Use whileDrag for drag state styling

## What's Next
Continue to [Advanced Animations](03-advanced-animations/01-page-transitions.md) to learn about page transitions.