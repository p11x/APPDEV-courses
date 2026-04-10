# ⚡ React Performance Optimization Complete Guide

## Building High-Performance React Applications

---

## Table of Contents

1. [Understanding React Performance](#understanding-react-performance)
2. [React.memo](#reactmemo)
3. [useMemo](#usememo)
4. [useCallback](#usecallback)
5. [Code Splitting](#code-splitting)
6. [Virtualization](#virtualization)
7. [Image Optimization](#image-optimization)
8. [Bundle Optimization](#bundle-optimization)
9. [Profiling and Debugging](#profiling-and-debugging)
10. [Real-World Examples](#real-world-examples)

---

## Understanding React Performance

### How React Renders

```
┌─────────────────────────────────────────────────────────────┐
│              REACT RENDER CYCLE                            │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│   State Change                                              │
│       │                                                    │
│       ▼                                                    │
│   ┌─────────────┐                                          │
│   │  Re-render │  Every component with state              │
│   └─────────────┘                                          │
│       │                                                    │
│       ▼                                                    │
│   ┌─────────────┐                                          │
│   │  Virtual   │  Creates new virtual DOM                 │
│   │    DOM     │  representation                          │
│   └─────────────┘                                          │
│       │                                                    │
│       ▼                                                    │
│   ┌─────────────┐                                          │
│   │   Diffing  │  Compare old vs new virtual DOM          │
│   └─────────────┘                                          │
│       │                                                    │
│       ▼                                                    │
│   ┌─────────────┐                                          │
│   │   Commit   │  Update only changed DOM nodes            │
│   └─────────────┘                                          │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Performance Problems

```jsx
// ❌ Unnecessary re-renders
function Parent() {
  const [count, setCount] = useState(0);
  
  // This function is recreated on every render
  const handleClick = () => console.log('click');
  
  return (
    <div>
      <p>Count: {count}</p>
      <Child onClick={handleClick} />
    </div>
  );
}

// ✅ Memoized to prevent unnecessary re-renders
function Parent() {
  const [count, setCount] = useState(0);
  
  const handleClick = useCallback(() => console.log('click'), []);
  
  return (
    <div>
      <p>Count: {count}</p>
      <MemoizedChild onClick={handleClick} />
    </div>
  );
}
```

---

## React.memo

### Basic Usage

```jsx
import { memo } from 'react';

// Memoized component - only re-renders when props change
const Button = memo(function Button({ onClick, children }) {
  console.log('Button rendered');
  return <button onClick={onClick}>{children}</button>;
});
```

### Custom Comparison

```jsx
const UserCard = memo(
  function UserCard({ user, onEdit }) {
    return (
      <div>
        <h3>{user.name}</h3>
        <button onClick={() => onEdit(user.id)}>Edit</button>
      </div>
    );
  },
  (prevProps, nextProps) => {
    // Custom comparison function
    // Return true if should NOT re-render
    return (
      prevProps.user.id === nextProps.user.id &&
      prevProps.user.name === nextProps.user.name
    );
  }
);
```

### Use Cases

```jsx
// ✅ Good use cases for memo:
// - Expensive components
// - Components with many children
// - Pure functional components
// - Large lists

// ❌ Don't use memo for:
// - Simple components
// - Frequently changing props
// - Context providers
```

---

## useMemo

### Basic Usage

```jsx
import { useMemo } from 'react';

function ExpensiveComponent({ items, filter }) {
  // Only recalculates when items or filter changes
  const filteredItems = useMemo(() => {
    console.log('Filtering...');
    return items.filter(item => 
      item.name.toLowerCase().includes(filter.toLowerCase())
    );
  }, [items, filter]);
  
  // Only recalculates when filteredItems changes
  const sortedItems = useMemo(() => {
    console.log('Sorting...');
    return [...filteredItems].sort((a, b) => a.name.localeCompare(b.name));
  }, [filteredItems]);
  
  return (
    <ul>
      {sortedItems.map(item => (
        <li key={item.id}>{item.name}</li>
      ))}
    </ul>
  );
}
```

### Memoizing Objects

```jsx
function Profile({ user, theme }) {
  // Memoize style object to prevent re-renders
  const styles = useMemo(() => ({
    container: {
      backgroundColor: theme === 'dark' ? '#333' : '#fff',
      color: theme === 'dark' ? '#fff' : '#333',
      padding: '20px'
    }
  }), [theme]);
  
  return <div style={styles}>{user.name}</div>;
}
```

### Expensive Calculations

```jsx
function DataAnalysis({ data, filters }) {
  const processedData = useMemo(() => {
    // Expensive calculations
    return data
      .filter(item => filters.includes(item.category))
      .map(item => ({
        ...item,
        score: calculateScore(item),
        rank: calculateRank(item)
      }))
      .sort((a, b) => b.score - a.score);
  }, [data, filters]);
  
  return <Chart data={processedData} />;
}
```

---

## useCallback

### Basic Usage

```jsx
import { useCallback } from 'react';

function Parent() {
  const [count, setCount] = useState(0);
  
  // Memoized callback
  const handleClick = useCallback(() => {
    console.log('Clicked');
  }, []); // Empty deps = never changes
  
  return <Child onClick={handleClick} />;
}

function ParentWithDeps() {
  const [count, setCount] = useState(0);
  
  // Callback with dependencies
  const handleClick = useCallback(() => {
    console.log('Count:', count);
  }, [count]); // Changes when count changes
  
  return <Child onClick={handleClick} />;
}
```

### Optimizing Child Components

```jsx
// Child won't re-render unless onClick changes
const Child = memo(function Child({ onClick }) {
  console.log('Child rendered');
  return <button onClick={onClick}>Click</button>;
});

function Parent() {
  const handleClick = useCallback(() => {
    console.log('Parent clicked');
  }, []);
  
  return <Child onClick={handleClick} />;
}
```

### Passing Callbacks to Dependencies

```jsx
function TodoList() {
  const [todos, setTodos] = useState([]);
  
  // Add todo
  const addTodo = useCallback((text) => {
    setTodos(prev => [...prev, { text, id: Date.now() }]);
  }, []);
  
  // Remove todo
  const removeTodo = useCallback((id) => {
    setTodos(prev => prev.filter(todo => todo.id !== id));
  }, []);
  
  // Toggle todo
  const toggleTodo = useCallback((id) => {
    setTodos(prev => prev.map(todo =>
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  }, []);
  
  return (
    <TodoForm onAdd={addTodo} />
    <TodoList todos={todos} onRemove={removeTodo} onToggle={toggleTodo} />
  );
}
```

---

## Code Splitting

### Lazy Loading

```jsx
import { lazy, Suspense } from 'react';
import { Routes, Route } from 'react-router-dom';

const Home = lazy(() => import('./pages/Home'));
const Dashboard = lazy(() => import('./pages/Dashboard'));
const Settings = lazy(() => import('./pages/Settings'));

function App() {
  return (
    <Suspense fallback={<Loading />}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/settings" element={<Settings />} />
      </Routes>
    </Suspense>
  );
}
```

### Component Lazy Loading

```jsx
import { lazy } from 'react';

const HeavyChart = lazy(() => import('./HeavyChart'));

function Analytics() {
  const [showChart, setShowChart] = useState(false);
  
  return (
    <div>
      <button onClick={() => setShowChart(true)}>Load Chart</button>
      {showChart && (
        <Suspense fallback={<div>Loading chart...</div>}>
          <HeavyChart />
        </Suspense>
      )}
    </div>
  );
}
```

### Route-Based Splitting

```jsx
import { lazy } from 'react';
import { Routes, Route } from 'react-router-dom';

const routes = [
  {
    path: '/',
    component: lazy(() => import('./pages/Home')),
    exact: true
  },
  {
    path: '/about',
    component: lazy(() => import('./pages/About'))
  },
  {
    path: '/dashboard',
    component: lazy(() => import('./pages/Dashboard'))
  }
];

function App() {
  return (
    <Suspense fallback={<PageLoader />}>
      <Routes>
        {routes.map(route => (
          <Route
            key={route.path}
            path={route.path}
            element={<route.component />}
          />
        ))}
      </Routes>
    </Suspense>
  );
}
```

---

## Virtualization

### Windowing Large Lists

```bash
npm install react-window
```

```jsx
import { FixedSizeList } from 'react-window';

function VirtualList({ items }) {
  const Row = ({ index, style }) => (
    <div style={style}>{items[index].name}</div>
  );
  
  return (
    <FixedSizeList
      height={400}
      itemCount={items.length}
      itemSize={50}
      width={300}
    >
      {Row}
    </FixedSizeList>
  );
}
```

### Variable Height Lists

```bash
npm install react-virtualized-auto-sizing
```

```jsx
import { useVirtualizer } from '@tanstack/react-virtual';

function VirtualAutoSizingList({ items }) {
  const parentRef = useRef();
  
  const virtualizer = useVirtualizer({
    count: items.length,
    getScrollElement: () => parentRef.current,
    estimateSize: () => 50
  });
  
  return (
    <div ref={parentRef} style={{ height: 400, overflow: 'auto' }}>
      <div style={{ height: virtualizer.getTotalSize() }}>
        {virtualizer.getVirtualItems().map(virtualItem => (
          <div
            key={virtualItem.key}
            style={{
              position: 'absolute',
              top: virtualItem.start,
              height: virtualItem.size
            }}
          >
            {items[virtualItem.index].name}
          </div>
        ))}
      </div>
    </div>
  );
}
```

---

## Image Optimization

### Lazy Loading Images

```jsx
function LazyImage({ src, alt }) {
  const [loaded, setLoaded] = useState(false);
  
  return (
    <img
      src={src}
      alt={alt}
      loading="lazy"
      onLoad={() => setLoaded(true)}
      style={{ opacity: loaded ? 1 : 0.5 }}
    />
  );
}
```

### Responsive Images

```jsx
function ResponsiveImage({ src, alt, sources }) {
  return (
    <picture>
      {sources.map(s => (
        <source key={s.src} srcSet={s.src} media={s.media} />
      ))}
      <img src={src} alt={alt} />
    </picture>
  );
}
```

### Image Component with Caching

```jsx
function CachedImage({ src, alt }) {
  const [image, setImage] = useState(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    let mounted = true;
    
    const loadImage = async () => {
      const cached = imageCache.get(src);
      if (cached) {
        setImage(cached);
        setLoading(false);
        return;
      }
      
      const img = new Image();
      img.src = src;
      img.onload = () => {
        if (!mounted) return;
        imageCache.set(src, src);
        setImage(src);
        setLoading(false);
      };
    };
    
    loadImage();
    return () => { mounted = false; };
  }, [src]);
  
  return (
    <img 
      src={image} 
      alt={alt} 
      style={{ opacity: loading ? 0 : 1 }} 
    />
  );
}
```

---

## Bundle Optimization

### Analyzing Bundle

```bash
# Install bundle analyzer
npm install --save-dev webpack-bundle-analyzer
```

```javascript
// webpack.config.js
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = {
  plugins: [
    new BundleAnalyzerPlugin()
  ]
};
```

### Tree Shaking

```javascript
// Only import what's needed
// ✅ Good
import { pick } from 'lodash';

// ❌ Bad - imports entire library
import _ from 'lodash';
```

### Code Splitting Strategies

```javascript
// webpack.config.js
module.exports = {
  optimization: {
    splitChunks: {
      chunks: 'all',
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendors',
          chunks: 'all'
        },
        common: {
          minChunks: 2,
          priority: -10,
          reuseExistingChunk: true
        }
      }
    }
  }
};
```

---

## Profiling and Debugging

### React DevTools Profiler

```jsx
// Wrap component with Profiler
function ProfileExample() {
  return (
    <Profiler id="MyComponent" onRender={onRenderCallback}>
      <MyComponent />
    </Profiler>
  );
}

function onRenderCallback(
  id,
  phase,
  actualDuration,
  baseDuration,
  startTime,
  commitTime
) {
  console.log(`${id} ${phase}:`, {
    actualDuration,
    baseDuration,
    startTime,
    commitTime
  });
}
```

### Performance Monitoring

```jsx
function usePerformanceMark(name, callback) {
  useEffect(() => {
    const start = performance.now();
    callback();
    const end = performance.now();
    console.log(`${name}: ${end - start}ms`);
  }, [callback]);
}

// Usage
function DataComponent() {
  const [data, setData] = useState([]);
  
  usePerformanceMark('Data fetch', () => {
    fetchData().then(setData);
  });
  
  return <List data={data} />;
}
```

### Measuring Render Performance

```jsx
function useRenderCount() {
  const count = useRef(0);
  useEffect(() => {
    count.current += 1;
    console.log('Render:', count.current);
  });
}

function WatchRender() {
  useRenderCount();
  return <div>Check console</div>;
}
```

---

## Real-World Examples

### Optimized Todo List

```jsx
import { memo, useMemo, useCallback, useState } from 'react';

const TodoItem = memo(function TodoItem({ todo, onToggle, onDelete }) {
  return (
    <div>
      <input
        type="checkbox"
        checked={todo.completed}
        onChange={() => onToggle(todo.id)}
      />
      <span>{todo.text}</span>
      <button onClick={() => onDelete(todo.id)}>Delete</button>
    </div>
  );
});

function TodoList() {
  const [todos, setTodos] = useState([]);
  const [filter, setFilter] = useState('all');
  
  const filteredTodos = useMemo(() => {
    switch (filter) {
      case 'completed':
        return todos.filter(t => t.completed);
      case 'active':
        return todos.filter(t => !t.completed);
      default:
        return todos;
    }
  }, [todos, filter]);
  
  const toggleTodo = useCallback((id) => {
    setTodos(prev => prev.map(todo =>
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  }, []);
  
  const deleteTodo = useCallback((id) => {
    setTodos(prev => prev.filter(todo => todo.id !== id));
  }, []);
  
  return (
    <div>
      <select value={filter} onChange={e => setFilter(e.target.value)}>
        <option value="all">All</option>
        <option value="active">Active</option>
        <option value="completed">Completed</option>
      </select>
      <ul>
        {filteredTodos.map(todo => (
          <TodoItem
            key={todo.id}
            todo={todo}
            onToggle={toggleTodo}
            onDelete={deleteTodo}
          />
        ))}
      </ul>
    </div>
  );
}
```

### Virtualized Table

```jsx
import { useVirtualizer } from '@tanstack/react-virtual';

function VirtualTable({ data, columns }) {
  const parentRef = useRef();
  
  const rowVirtualizer = useVirtualizer({
    count: data.length,
    getScrollElement: () => parentRef.current,
    estimateSize: () => 35
  });
  
  return (
    <div ref={parentRef} style={{ height: 500, overflow: 'auto' }}>
      <table>
        <thead>
          <tr>
            {columns.map(col => (
              <th key={col.key}>{col.title}</th>
            ))}
          </tr>
        </thead>
        <tbody style={{ height: rowVirtualizer.getTotalSize() }}>
          {rowVirtualizer.getVirtualItems().map(virtualRow => (
            <tr
              key={virtualRow.key}
              style={{
                position: 'absolute',
                top: virtualRow.start,
                height: virtualRow.size
              }}
            >
              {columns.map(col => (
                <td key={col.key}>{data[virtualRow.index][col.key]}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
```

---

## Summary

### Key Takeaways

1. **React.memo**: Prevents unnecessary re-renders
2. **useMemo**: Memoizes expensive calculations
3. **useCallback**: Memoizes callback functions
4. **Code Splitting**: Lazy load components
5. **Virtualization**: Render only visible items
6. **Bundle Optimization**: Reduce bundle size

### Next Steps

- Continue with: [08_REACT_TESTING_STRATEGIES.md](08_REACT_TESTING_STRATEGIES.md)
- Use React DevTools Profiler
- Implement performance budgets

---

## Cross-References

- **Previous**: [06_REACT_FORMS_MASTER.md](06_REACT_FORMS_MASTER.md)
- **Next**: [08_REACT_TESTING_STRATEGIES.md](08_REACT_TESTING_STRATEGIES.md)

---

*Last updated: 2024*