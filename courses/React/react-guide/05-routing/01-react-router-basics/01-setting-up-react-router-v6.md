# Setting Up React Router v6

## Overview

React Router is the standard routing library for React. Version 6 brings significant improvements including the new hook-based API and better nested routing. This guide covers setting up React Router v6 for your application.

## Prerequisites

- Understanding of React components
- Knowledge of SPA concepts
- Node.js installed

## Core Concepts

```jsx
// File: src/App.jsx

import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <nav>
        <Link to="/">Home</Link>
        <Link to="/about">About</Link>
      </nav>
      
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
      </Routes>
    </BrowserRouter>
  );
}

function Home() { return <h1>Home</h1>; }
function About() { return <h1>About</h1>; }
```

## Key Takeaways

- BrowserRouter wraps the app
- Routes contain Route definitions
- Link provides client-side navigation
- Route component renders when path matches
