# Page Transitions

## Overview
Creating smooth page transitions enhances user experience by providing visual continuity between route changes. Framer Motion works with React Router or Next.js App Router to create seamless transitions.

## Prerequisites
- Framer Motion basics
- React Router or Next.js knowledge

## Core Concepts

### AnimatePresence with Routes

```tsx
// [File: app/components/PageTransition.tsx]
'use client';

import { motion, AnimatePresence } from 'framer-motion';
import { useLocation } from 'react-router-dom';

export default function PageTransition({ children }: { children: React.ReactNode }) {
  const location = useLocation();
  
  return (
    <AnimatePresence mode="wait">
      <motion.div
        key={location.pathname}
        initial={{ opacity: 0, y: 10 }}
        animate={{ opacity: 1, y: 0 }}
        exit={{ opacity: 0, y: -10 }}
        transition={{ duration: 0.2 }}
      >
        {children}
      </motion.div>
    </AnimatePresence>
  );
}
```

## Key Takeaways
- Use AnimatePresence with mode="wait" for page transitions
- Use unique key for each route to trigger animations
- Consider scroll restoration with page transitions

## What's Next
Continue to [Scroll-Based Animations](02-scroll-based-animations.md) to learn about scroll-triggered animations.