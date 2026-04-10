# Framework Performance Optimization

Comprehensive guide to optimizing JavaScript framework applications. Covers code splitting, lazy loading, memoization, bundle optimization, and profiling.

## Table of Contents

1. [Performance Fundamentals](#performance-fundamentals)
2. [Code Splitting](#code-splitting)
3. [Lazy Loading](#lazy-loading)
4. [Memoization Strategies](#memoization-strategies)
5. [Bundle Optimization](#bundle-optimization)
6. [Rendering Optimization](#rendering-optimization)
7. [Memory Management](#memory-management)
8. [Profiling Tools](#profiling-tools)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pitfalls)

---

## Performance Fundamentals

### Core Web Vitals

| Metric | Good | Needs Improvement | Poor |
|--------|------|--------------------|------|
| LCP | < 2.5s | 2.5s - 4s | > 4s |
| FID | < 100ms | 100ms - 300ms | > 300ms |
| CLS | < 0.1 | 0.1 - 0.25 | > 0.25 |

### Performance Budget

```javascript
// file: performance/budget.js
const performanceBudget = {
  javascript: {
    total: 170 * 1024,
    initial: 40 * 1024,
    lazyLoaded: 130 * 1024,
  },
  css: {
    total: 35 * 1024,
  },
  images: {
    total: 100 * 1024,
    hero: 50 * 1024,
    thumbnails: 50 * 1024,
  },
  fonts: {
    total: 25 * 1024,
  },
  total: 330 * 1024,
};
```

---

## Code Splitting

### Route-Based Splitting

```javascript
// file: splitting/RouteSplitting.jsx
import React, { Suspense, lazy } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

const Home = lazy(() => import('../pages/Home'));
const About = lazy(() => import('../pages/About'));
const Dashboard = lazy(() => import('../pages/Dashboard'));
const Settings = lazy(() => import('../pages/Settings'));
const Users = lazy(() => import('../pages/Users'));
const Products = lazy(() => import('../pages/Products'));
const Checkout = lazy(() => import('../pages/Checkout'));

const loadingFallback = (
  <div className="loading-spinner">
    <div className="spinner" />
    Loading...
  </div>
);

const App = () => (
  <BrowserRouter>
    <Suspense fallback={loadingFallback}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        
        <Route path="/dashboard/*" element={<Dashboard />} />
        
        <Route path="/settings/*" element={<Settings />} />
        
        <Route path="/users/*" element={<Users />} />
        
        <Route path="/products/*" element={<Products />} />
        
        <Route path="/checkout/*" element={<Checkout />} />
      </Routes>
    </Suspense>
  </BrowserRouter>
);
```

### Component-Level Splitting

```javascript
// file: splitting/ComponentSplitting.jsx
import React, { lazy, Suspense, useState, useCallback } from 'react';

const ChartComponent = lazy(() => import('./ChartComponent'));
const EditorComponent = lazy(() => import('./EditorComponent'));
const VideoPlayer = lazy(() => import('./VideoPlayer'));

const FeatureToggle = ({ feature, children }) => {
  const [isEnabled, setIsEnabled] = useState(false);

  const enableFeature = useCallback(() => {
    setIsEnabled(true);
  }, []);

  return (
    <div>
      {children({
        isEnabled,
        enableFeature,
      })}
    </div>
  );
};

const App = () => {
  const [showCharts, setShowCharts] = useState(false);
  const [showEditor, setShowEditor] = useState(false);
  const [showVideo, setShowVideo] = useState(false);

  return (
    <div>
      <button onClick={() => setShowCharts(true)}>Enable Charts</button>
      <button onClick={() => setShowEditor(true)}>Enable Editor</button>
      <button onClick={() => setShowVideo(true)}>Enable Video</button>

      <Suspense fallback={<div>Loading...</div>}>
        {showCharts && <ChartComponent />}
        {showEditor && <EditorComponent />}
        {showVideo && <VideoPlayer />}
      </Suspense>
    </div>
  );
};
```

---

## Lazy Loading

### Image Lazy Loading

```javascript
// file: lazy/ImageLazyLoading.jsx
import { useState, useEffect, useRef } from 'react';

const Image = ({ src, alt, placeholder, ...props }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [isInView, setIsInView] = useState(false);
  const imageRef = useRef(null);

  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsInView(true);
          observer.disconnect();
        }
      },
      {
        rootMargin: '50px',
        threshold: 0,
      }
    );

    if (imageRef.current) {
      observer.observe(imageRef.current);
    }

    return () => observer.disconnect();
  }, []);

  const handleLoad = () => {
    setIsLoaded(true);
  };

  return (
    <div ref={imageRef} className="image-container">
      {!isLoaded && placeholder}
      {isInView && (
        <img
          src={src}
          alt={alt}
          onLoad={handleLoad}
          style={{ opacity: isLoaded ? 1 : 0 }}
          {...props}
        />
      )}
    </div>
  );
};

export default Image;
```

### Data Lazy Loading

```javascript
// file: lazy/DataLazyLoading.jsx
import { useState, useEffect, useCallback, useRef } from 'react';

const useInfiniteScroll = (fetchFn, options = {}) => {
  const {
    threshold = 100,
    initialPage = 1,
  } = options;

  const [items, setItems] = useState([]);
  const [page, setPage] = useState(initialPage);
  const [loading, setLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const [error, setError] = useState(null);
  const observerRef = useRef(null);

  const loadMore = useCallback(async () => {
    if (loading || !hasMore) return;

    setLoading(true);
    setError(null);

    try {
      const newItems = await fetchFn(page);
      
      if (newItems.length === 0) {
        setHasMore(false);
      } else {
        setItems((prev) => [...prev, ...newItems]);
        setPage((prev) => prev + 1);
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, [fetchFn, page, loading, hasMore]);

  useEffect(() => {
    loadMore();
  }, []);

  const lastElementRef = useCallback(
    (node) => {
      if (loading) return;
      
      if (observerRef.current) {
        observerRef.current.disconnect();
      }

      observerRef.current = new IntersectionObserver(
        ([entry]) => {
          if (entry.isIntersecting && hasMore) {
            loadMore();
          }
        },
        { rootMargin: `${threshold}px` }
      );

      if (node) {
        observerRef.current.observe(node);
      }
    },
    [loading, hasMore, loadMore, threshold]
  );

  return {
    items,
    loading,
    hasMore,
    error,
    lastElementRef,
    loadMore,
  };
};

const UserList = () => {
  const fetchUsers = async (page) => {
    const response = await fetch(`/api/users?page=${page}`);
    return response.json();
  };

  const {
    items,
    loading,
    hasMore,
    lastElementRef,
  } = useInfiniteScroll(fetchUsers, { threshold: 200 });

  return (
    <div>
      {items.map((user, index) => (
        <div
          key={user.id}
          ref={index === items.length - 1 ? lastElementRef : null}
        >
          {user.name}
        </div>
      ))}
      {loading && <div>Loading more...</div>}
    </div>
  );
};
```

---

## Memoization Strategies

### React Memo

```javascript
// file: memo/ReactMemo.jsx
import React, { useMemo, useCallback, memo } from 'react';

const ExpensiveComponent = memo(({ data, onItemClick }) => {
  const processedData = useMemo(() => {
    return data.map((item) => ({
      ...item,
      score: calculateScore(item),
    }));
  }, [data]);

  const handleClick = useCallback((id) => {
    onItemClick(id);
  }, [onItemClick]);

  return (
    <ul>
      {processedData.map((item) => (
        <li key={item.id} onClick={() => handleClick(item.id)}>
          {item.score}
        </li>
      ))}
    </ul>
  );
});

const calculateScore = (item) => {
  return item.value * item.multiplier + item.bonus;
};

const ParentComponent = () => {
  const [data, setData] = useState(initialData);
  const [selectedId, setSelectedId] = useState(null);

  const handleItemClick = useCallback((id) => {
    setSelectedId(id);
  }, []);

  return (
    <ExpensiveComponent
      data={data}
      onItemClick={handleItemClick}
    />
  );
};
```

### Custom Memo Hook

```javascript
// file: memo/useMemoComparison.js
import { useRef, useEffect } from 'react';

const usePrevious = (value) => {
  const ref = useRef();
  useEffect(() => {
    ref.current = value;
  });
  return ref.current;
};

const useDeepCompare = (value) => {
  const previousValue = usePrevious(value);

  if (!isEqual(previousValue, value)) {
    return value;
  }

  return previousValue;
};

const isEqual = (a, b) => {
  if (a === b) return true;
  if (a == null || b == null) return false;
  if (typeof a !== typeof b) return false;

  if (typeof a === 'object') {
    const keysA = Object.keys(a);
    const keysB = Object.keys(b);

    if (keysA.length !== keysB.length) return false;

    return keysA.every((key) => isEqual(a[key], b[key]));
  }

  return false;
};

const Component = ({ config }) => {
  const stableConfig = useDeepCompare(config);

  useEffect(() => {
    console.log('Config changed:', stableConfig);
  }, [stableConfig]);
};
```

---

## Bundle Optimization

### Bundle Analysis

```javascript
// file: optimization/bundle-analysis.js
import { replaceLibImports } from './plugins/replace-lib-imports';

const webpackConfig = {
  mode: 'production',
  optimization: {
    minimize: true,
    usedExports: true,
    sideEffects: true,
    concatenateModules: true,
    splitChunks: {
      chunks: 'all',
      maxInitialRequests: 25,
      minSize: 20000,
      cacheGroups: {
        defaultVendors: {
          test: /[\\/]node_modules[\\/]/,
          priority: -10,
          reuseExistingChunk: true,
        },
        react: {
          test: /[\\/]node_modules[\\/](react|react-dom)[\\/]/,
          name: 'react-vendor',
          chunks: 'all',
          priority: 20,
        },
        common: {
          minChunks: 2,
          priority: -10,
          reuseExistingChunk: true,
        },
        styles: {
          name: 'styles',
          type: 'css/mini-extract',
          chunks: 'all',
        },
      },
    },
  },
  plugins: [
    new replaceLibImports({
      replace: {
        lodash: {
          replaceWith: 'lodash-es',
        },
      },
    }),
  ],
};

export default webpackConfig;
```

### Tree Shaking

```javascript
// file: optimization/tree-shaking.js
export const utils = {
  add: (a, b) => a + b,
  subtract: (a, b) => a - b,
};

export const unused = () => {
  console.log('This function is not exported');
};
```

```javascript
// file: optimization/pure-functions.js
import { add } from './utils';

const result = add(1, 2);
console.log(result);
```

---

## Rendering Optimization

### Virtual List

```javascript
// file: optimization/VirtualList.jsx
import { useState, useEffect, useRef } from 'react';

const VirtualList = ({
  items,
  itemHeight = 50,
  containerHeight = 600,
  overscan = 5,
  renderItem,
}) => {
  const [scrollTop, setScrollTop] = useState(0);
  const containerRef = useRef(null);

  useEffect(() => {
    const container = containerRef.current;
    if (!container) return;

    const handleScroll = (e) => {
      setScrollTop(e.target.scrollTop);
    };

    container.addEventListener('scroll', handleScroll);
    return () => container.removeEventListener('scroll', handleScroll);
  }, []);

  const getVisibleRange = () => {
    const startIndex = Math.max(
      0,
      Math.floor(scrollTop / itemHeight) - overscan
    );
    const visibleCount = Math.ceil(containerHeight / itemHeight) + overscan * 2;
    const endIndex = Math.min(items.length - 1, startIndex + visibleCount);

    return { startIndex, endIndex };
  };

  const { startIndex, endIndex } = getVisibleRange();
  const visibleItems = items.slice(startIndex, endIndex + 1);
  const totalHeight = items.length * itemHeight;
  const offsetY = startIndex * itemHeight;

  return (
    <div
      ref={containerRef}
      style={{
        height: containerHeight,
        overflow: 'auto',
      }}
    >
      <div style={{ height: totalHeight, position: 'relative' }}>
        <div
          style={{
            position: 'absolute',
            top: offsetY,
            left: 0,
            right: 0,
          }}
        >
          {visibleItems.map((item, index) => (
            <div
              key={item.id}
              style={{ height: itemHeight }}
            >
              {renderItem(item, startIndex + index)}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default VirtualList;
```

### Windowing Strategy

```javascript
// file: optimization/Windowing.jsx
import { useState, useEffect, useCallback } from 'react';

const useWindowedList = (items, options = {}) => {
  const {
    containerHeight = 600,
    itemHeight = 50,
    overscan = 5,
  } = options;

  const [scrollTop, setScrollTop] = useState(0);

  const handleScroll = useCallback((e) => {
    requestAnimationFrame(() => {
      setScrollTop(e.target.scrollTop);
    });
  }, []);

  const getItems = useCallback(() => {
    const startIndex = Math.max(
      0,
      Math.floor(scrollTop / itemHeight) - overscan
    );
    const visibleCount = Math.ceil(containerHeight / itemHeight);
    const endIndex = Math.min(
      items.length,
      startIndex + visibleCount + overscan * 2
    );

    return {
      items: items.slice(startIndex, endIndex),
      startIndex,
      totalHeight: items.length * itemHeight,
      offsetY: startIndex * itemHeight,
    };
  }, [items, scrollTop, containerHeight, itemHeight, overscan]);

  return {
    ...getItems(),
    handleScroll,
    scrollTop,
  };
};
```

---

## Memory Management

### Event Listener Cleanup

```javascript
// file: memory/EventCleanup.jsx
import { useEffect, useRef } from 'react';

const EventComponent = () => {
  const divRef = useRef(null);
  const handlersRef = useRef({});

  useEffect(() => {
    const div = divRef.current;
    if (!div) return;

    const handleMouseEnter = (e) => {
      console.log('Mouse entered:', e.target);
    };

    const handleMouseLeave = (e) => {
      console.log('Mouse left:', e.target);
    };

    handlersRef.current = {
      mouseenter: handleMouseEnter,
      mouseleave: handleMouseLeave,
    };

    Object.entries(handlersRef.current).forEach(([event, handler]) => {
      div.addEventListener(event, handler);
    });

    return () => {
      Object.entries(handlersRef.current).forEach(([event, handler]) => {
        div.removeEventListener(event, handler);
      });
    };
  }, []);

  return <div ref={divRef}>Hover me</div>;
};
```

### Subscription Cleanup

```javascript
// file: memory/SubscriptionCleanup.jsx
import { useEffect, useRef } from 'react';

const useSubscription = (subscribe, unsubscribe) => {
  const subscribedRef = useRef(false);

  useEffect(() => {
    if (subscribedRef.current) return;

    subscribe();
    subscribedRef.current = true;

    return () => {
      if (subscribedRef.current) {
        unsubscribe();
        subscribedRef.current = false;
      }
    };
  }, [subscribe, unsubscribe]);
};

const WebSocketComponent = () => {
  const wsRef = useRef(null);

  useEffect(() => {
    wsRef.current = new WebSocket('wss://example.com');

    wsRef.current.onopen = () => {
      console.log('Connected');
    };

    wsRef.current.onmessage = (event) => {
      console.log('Message:', event.data);
    };

    return () => {
      if (wsRef.current) {
        wsRef.current.close();
      }
    };
  }, []);

  return <div>WebSocket Connected</div>;
};
```

---

## Profiling Tools

### React DevTools Profiler

```javascript
// file: profiling/ReactProfiler.jsx
import React, { Profiler } from 'react';

const onRender = (id, phase, actualDuration, baseDuration, startTime, commitTime) => {
  console.log({
    id,
    phase,
    actualDuration,
    baseDuration,
    startTime,
    commitTime,
  });
};

const App = () => (
  <Profiler id="App" onRender={onRender}>
    <MainContent />
  </Profiler>
);
```

### Custom Performance Tracking

```javascript
// file: profiling/PerformanceTracking.jsx
import { useEffect, useRef } from 'react';

const usePerformanceMark = (markName) => {
  const startTime = useRef(0);

  useEffect(() => {
    if (performance.mark) {
      startTime.current = performance.now();
      performance.mark(`${markName}-start`);
    }

    return () => {
      if (performance.mark) {
        performance.mark(`${markName}-end`);
        performance.measure(
          markName,
          `${markName}-start`,
          `${markName}-end`
        );
      }
    };
  }, [markName]);

  return {
    mark: (name) => {
      if (performance.mark) {
        performance.mark(name);
      }
    },
    measure: (name, startMark, endMark) => {
      if (performance.measure) {
        performance.measure(name, startMark, endMark);
      }
    },
  };
};

const Component = () => {
  const perf = usePerformanceMark('Component-render');

  return <div>Component</div>;
};
```

---

## Key Takeaways

1. **Code splitting** reduces initial bundle size
2. **Lazy loading** defers expensive operations
3. **Memoization** prevents unnecessary re-renders
4. **Virtual lists** handle large data sets
5. **Memory management** prevents leaks
6. **Profiling** identifies bottlenecks

---

## Common Pitfalls

1. **Premature optimization** adds unnecessary complexity
2. **Over-memoization** wastes memory
3. **Not handling loading states** causes poor UX
4. **Memory leaks** from event listeners
5. **Not profiling** before optimizing

---

## Related Files

- [01_FRAMEWORK_COMPARISON_MASTER](./01_FRAMEWORK_COMPARISON_MASTER.md)
- [02_COMPONENT_ARCHITECTURE_PATTERNS](./02_COMPONENT_ARCHITECTURE_PATTERNS.md)
- [03_VIRTUAL_DOM_EXPLANATION](./03_VIRTUAL_DOM_EXPLANATION.md)
- [07_FRAMEWORK_TESTING_STRATEGIES](./07_FRAMEWORK_TESTING_STRATEGIES.md)