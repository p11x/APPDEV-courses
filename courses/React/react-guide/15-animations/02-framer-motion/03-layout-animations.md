# Layout Animations

## Overview
Framer Motion's layout animations automatically animate position and size changes when elements are added, removed, or reordered. The layout prop enables shared element transitions and automatic layout animations without complex configuration.

## Prerequisites
- Framer Motion basics
- Understanding of variants

## Core Concepts

### Automatic Layout Animations

```tsx
// [File: app/components/ReorderList.tsx]
'use client';

import { motion, AnimatePresence } from 'framer-motion';
import { useState } from 'react';

export default function ReorderList({ items }: { items: string[] }) {
  const [list, setList] = useState(items);
  
  const remove = (item: string) => {
    setList(list.filter(i => i !== item));
  };
  
  return (
    <motion.ul layout>
      <AnimatePresence>
        {list.map(item => (
          <motion.li
            key={item}
            layout
            initial={{ opacity: 0, scale: 0.8 }}
            animate={{ opacity: 1, scale: 1 }}
            exit={{ opacity: 0, scale: 0.8 }}
          >
            {item}
            <button onClick={() => remove(item)}>×</button>
          </motion.li>
        ))}
      </AnimatePresence>
    </motion.ul>
  );
}
```

### Shared Element Transitions

```tsx
// [File: app/components/SharedElement.tsx]
'use client';

import { motion } from 'framer-motion';

export default function SharedElementExample() {
  return (
    <motion.div layoutId="card">
      Shared element transitions automatically 
      animate between different positions!
    </motion.div>
  );
}
```

## Key Takeaways
- Use layout prop for automatic position/size animations
- Use layoutId for shared element transitions
- Use AnimatePresence for enter/exit animations

## What's Next
Continue to [Gesture Animations](04-gesture-animations.md) to learn about drag, pan, and other gesture-based animations.