# Animating Lists and Reorder

## Overview
Framer Motion excels at animating lists with add, remove, and reorder operations. Using AnimatePresence and proper key management creates smooth, professional list animations.

## Prerequisites
- Framer Motion basics
- Understanding of variants

## Core Concepts

### Animated List

```tsx
// [File: app/components/AnimatedList.tsx]
'use client';

import { motion, AnimatePresence } from 'framer-motion';
import { useState } from 'react';

export default function AnimatedList() {
  const [items, setItems] = useState(['Item 1', 'Item 2', 'Item 3']);
  
  const add = () => {
    setItems([...items, `Item ${items.length + 1}`]);
  };
  
  const remove = (index: number) => {
    setItems(items.filter((_, i) => i !== index));
  };
  
  return (
    <motion.ul layout>
      <AnimatePresence>
        {items.map((item, index) => (
          <motion.li
            key={item}
            layout
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            exit={{ opacity: 0, x: 20 }}
          >
            {item}
            <button onClick={() => remove(index)}>×</button>
          </motion.li>
        ))}
      </AnimatePresence>
    </motion.ul>
  );
}
```

### Reorderable List

```tsx
// [File: app/components/ReorderableList.tsx]
'use client';

import { Reorder, motion } from 'framer-motion';
import { useState } from 'react';

export default function ReorderableList() {
  const [items, setItems] = useState(['A', 'B', 'C', 'D']);
  
  return (
    <Reorder.Group axis="y" values={items} onReorder={setItems}>
      {items.map((item) => (
        <Reorder.Item key={item} value={item}>
          {item}
        </Reorder.Item>
      ))}
    </Reorder.Group>
  );
}
```

## Key Takeaways
- Use AnimatePresence for enter/exit animations
- Use layout prop for smooth position changes
- Always use stable keys for proper animation

## What's Next
This completes the Animations module. Continue to [Feature-Based Folder Structure](16-architecture/01-project-architecture/01-feature-based-folder-structure.md) to learn about organizing React projects.