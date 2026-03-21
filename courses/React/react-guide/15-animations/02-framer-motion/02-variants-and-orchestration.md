# Variants and Orchestration

## Overview
Framer Motion variants allow you to define animation states that can be orchestrated across multiple components. This guide covers creating complex animation sequences, staggering animations, and coordinating multiple animated elements.

## Prerequisites
- Framer Motion basics
- Understanding of React components

## Core Concepts

### Parent/Child Variants

```tsx
// [File: app/components/ProductGrid.tsx]
'use client';

import { motion } from 'framer-motion';

const containerVariants = {
  hidden: { opacity: 0 },
  show: {
    opacity: 1,
    transition: {
      staggerChildren: 0.1,
      delayChildren: 0.2
    }
  }
};

const itemVariants = {
  hidden: { opacity: 0, y: 20 },
  show: { 
    opacity: 1, 
    y: 0,
    transition: { duration: 0.4 }
  }
};

export default function ProductGrid({ products }: { products: any[] }) {
  return (
    <motion.div
      variants={containerVariants}
      initial="hidden"
      animate="show"
      className="product-grid"
    >
      {products.map(product => (
        <motion.div key={product.id} variants={itemVariants}>
          {product.name}
        </motion.div>
      ))}
    </motion.div>
  );
}
```

## Key Takeaways
- Use parent variants to orchestrate children
- Use staggerChildren for sequential animations
- Use delayChildren to offset child animations

## What's Next
Continue to [Layout Animations](03-layout-animations.md) to learn about automatic layout animations.