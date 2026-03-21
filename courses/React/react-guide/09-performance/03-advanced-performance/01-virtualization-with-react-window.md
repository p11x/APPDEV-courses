# Virtualization with react-window

## Overview

Rendering large lists can significantly impact performance. Virtualization (also called "windowing") solves this by only rendering items currently visible in the viewport. react-window is a popular library for virtualizing lists and grids.

## Prerequisites

- Understanding of React rendering
- Familiarity with list rendering

## Core Concepts

### Installing react-window

```bash
npm install react-window
```

### Basic Fixed Size List

```tsx
// File: src/components/VirtualList.tsx

import { FixedSizeList } from 'react-window';

interface Item {
  id: number;
  name: string;
}

function VirtualList({ items }: { items: Item[] }) {
  const Row = ({ index, style }: { index: number; style: React.CSSProperties }) => (
    <div style={style}>
      {items[index].name}
    </div>
  );

  return (
    <FixedSizeList
      height={400}
      itemCount={items.length}
      itemSize={50}
      width="100%"
    >
      {Row}
    </FixedSizeList>
  );
}
```

### Variable Size List

```tsx
// File: src/components/VariableList.tsx

import { VariableSizeList } from 'react-window';

function VariableList({ items }: { items: { id: number; name: string }[] }) {
  const getItemSize = (index: number) => {
    // Variable heights based on content
    return items[index].name.length > 20 ? 80 : 50;
  };

  const Row = ({ index, style }: { index: number; style: React.CSSProperties }) => (
    <div style={style}>
      {items[index].name}
    </div>
  );

  return (
    <VariableSizeList
      height={400}
      itemCount={items.length}
      itemSize={getItemSize}
      width="100%"
    >
      {Row}
    </VariableSizeList>
  );
}
```

## Key Takeaways

- Virtualization only renders visible items
- Use FixedSizeList for uniform rows
- Use VariableSizeList for dynamic heights
- Combine with infinite scroll for large datasets

## What's Next

Continue to [Web Workers in React](/09-performance/03-advanced-performance/02-web-workers-in-react.md) to learn about offloading heavy computations.