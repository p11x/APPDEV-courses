# Performance Profiling with DevTools

## Overview

React DevTools Profiler helps identify performance bottlenecks in your React application. Combined with Chrome's Performance tab, you can diagnose rendering issues, slow commits, and blocking operations.

## Prerequisites

- React DevTools installed
- Basic understanding of React rendering

## Core Concepts

### Using React DevTools Profiler

1. Install React DevTools browser extension
2. Open DevTools (F12) → Profiler tab
3. Click "Record" to start profiling
4. Interact with your app
5. Click "Stop" to see results

### Understanding the Flamegraph

The flamegraph shows:
- What rendered and why
- How long each commit took
- Which components are most expensive

### Using Chrome Performance Tab

1. Open DevTools → Performance tab
2. Click "Record"
3. Perform actions
4. Analyze:
   - Main thread activity
   - Long tasks (blocking UI)
   - Script evaluation
   - Layout and paint

## Key Takeaways

- Profile real user interactions
- Look for components that render frequently
- Check "Why did this render?" in profiler
- Monitor Long Tasks in Performance tab

## What's Next

This concludes the Performance section. Continue to [Vitest Setup with React](/10-testing/01-unit-testing/01-vitest-setup-with-react.md) to learn about testing your React applications.