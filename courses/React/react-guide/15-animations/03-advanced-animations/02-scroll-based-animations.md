# Scroll-Based Animations

## Overview
Scroll-based animations trigger based on scroll position, creating engaging experiences as users navigate through content. Framer Motion provides hooks for tracking scroll and creating parallax and reveal effects.

## Prerequisites
- Framer Motion basics
- Understanding of variants

## Core Concepts

### useScroll Hook

```tsx
// [File: app/components/ScrollAnimation.tsx]
'use client';

import { motion, useScroll, useTransform } from 'framer-motion';
import { useRef } from 'react';

export default function ScrollAnimation() {
  const ref = useRef(null);
  const { scrollYProgress } = useScroll({
    target: ref,
    offset: ['start end', 'end start']
  });
  
  const y = useTransform(scrollYProgress, [0, 1], [50, -50]);
  const opacity = useTransform(scrollYProgress, [0, 0.5, 1], [0, 1, 0]);
  
  return (
    <motion.div 
      ref={ref} 
      style={{ y, opacity }}
    >
      Scroll to see animation!
    </motion.div>
  );
}
```

### whileInView Animation

```tsx
// [File: app/components/RevealOnScroll.tsx]
'use client';

import { motion } from 'framer-motion';

export default function RevealOnScroll({ children }: { children: React.ReactNode }) {
  return (
    <motion.div
      initial={{ opacity: 0, y: 50 }}
      whileInView={{ opacity: 1, y: 0 }}
      viewport={{ once: true, margin: '-100px' }}
      transition={{ duration: 0.5 }}
    >
      {children}
    </motion.div>
  );
}
```

## Key Takeaways
- Use useScroll hook for scroll tracking
- Use useTransform to map scroll position to values
- Use whileInView for simple reveal animations

## What's Next
Continue to [Animating Lists](03-animating-lists-and-reorder.md) to learn about list animations.