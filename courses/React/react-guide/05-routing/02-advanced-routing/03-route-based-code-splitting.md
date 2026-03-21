# Route-based Code Splitting

## Overview
Route-based code splitting separates your application into chunks that load on demand, reducing initial bundle size and improving performance.

## Key Concepts

### Vite Configuration for Code Splitting

```jsx
// vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom', 'react-router-dom'],
          ui: ['@mui/material', '@emotion/react'],
        },
      },
    },
  },
});
```

## Key Takeaways
- Configure Vite/Rollup for manual chunks
- Each route becomes a separate chunk
- Reduces initial JavaScript payload

## What's Next
Continue to [Breadcrumb Navigation](../03-router-patterns/01-breadcrumb-navigation.md)
