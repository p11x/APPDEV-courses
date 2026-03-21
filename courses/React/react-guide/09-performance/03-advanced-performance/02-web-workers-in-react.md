# Web Workers in React

## Overview

Web Workers run JavaScript in background threads, separate from the main thread. They're perfect for heavy computations that would otherwise block the UI. This guide covers creating and using Web Workers in React applications.

## Prerequisites

- Understanding of JavaScript asynchronous patterns
- Familiarity with React hooks

## Core Concepts

### Creating a Web Worker

```typescript
// File: src/workers/dataProcessor.worker.ts

// This runs in a separate thread
self.onmessage = (e: MessageEvent) => {
  const { data, type } = e.data;
  
  if (type === 'process') {
    // Heavy computation
    const result = heavyComputation(data);
    self.postMessage({ result });
  }
};

function heavyComputation(data: any[]) {
  // Simulate heavy work
  return data.map(item => ({
    ...item,
    processed: true
  }));
}
```

### Using Workers in React

```tsx
// File: src/hooks/useWorker.ts

import { useEffect, useRef, useState, useCallback } from 'react';

function useWorker<T, R>(workerFactory: () => Worker) {
  const workerRef = useRef<Worker | null>(null);
  const [result, setResult] = useState<R | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    workerRef.current = workerFactory();
    
    workerRef.current.onmessage = (e: MessageEvent) => {
      setResult(e.data.result);
      setLoading(false);
    };

    return () => {
      workerRef.current?.terminate();
    };
  }, [workerFactory]);

  const postMessage = useCallback((data: T) => {
    setLoading(true);
    workerRef.current?.postMessage({ data, type: 'process' });
  }, []);

  return { result, loading, postMessage };
}

// Usage
function DataProcessor() {
  const { result, loading, postMessage } = useWorker(
    () => new Worker(new URL('../workers/dataProcessor.worker.ts', import.meta.url))
  );

  const handleProcess = () => {
    postMessage(largeDataset);
  };

  return (
    <div>
      <button onClick={handleProcess} disabled={loading}>
        {loading ? 'Processing...' : 'Process Data'}
      </button>
      {result && <pre>{JSON.stringify(result, null, 2)}</pre>}
    </div>
  );
}
```

## Key Takeaways

- Web Workers run in background threads
- Use for heavy computations, data processing
- Communicate via postMessage
- Clean up workers when components unmount

## What's Next

Continue to [Performance Profiling with DevTools](/09-performance/03-advanced-performance/03-performance-profiling-devtools.md) to learn about identifying performance issues.