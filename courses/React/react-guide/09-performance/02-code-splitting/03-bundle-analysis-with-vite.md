# Bundle Analysis with Vite

## Overview

Understanding your bundle composition is crucial for performance optimization. Vite provides built-in tools to analyze your bundle and identify large dependencies. This guide covers using rollup-plugin-visualizer and interpreting bundle output.

## Prerequisites

- Vite project setup
- Basic understanding of bundling

## Core Concepts

### Installing Bundle Analyzer

```bash
# File: terminal

npm install rollup-plugin-visualizer --save-dev
```

### Configuring Bundle Analysis

```javascript
// File: vite.config.ts

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { visualizer } from 'rollup-plugin-visualizer';

export default defineConfig({
  plugins: [
    react(),
    visualizer({
      filename: 'bundle-stats.html',
      open: true,
      gzip: true,
      brotliSize: true,
    }),
  ],
});
```

### Interpreting Results

The treemap view shows:
- Large files taking most space
- Which dependencies are largest
- Opportunities for code splitting

## Key Takeaways

- Use bundle analyzers to identify large dependencies
- Look for libraries that can be replaced with lighter alternatives
- Code split libraries that aren't needed immediately

## What's Next

Continue to [Virtualization with react-window](/09-performance/03-advanced-performance/01-virtualization-with-react-window.md) to learn about rendering large lists efficiently.