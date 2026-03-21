# Dashboard Layout Architecture

## Overview

Dashboard applications require thoughtful layout design with navigation, headers, and responsive content areas. This guide covers creating a modern dashboard layout using CSS Grid and managing sidebar state.

## Prerequisites

- CSS Grid knowledge
- React component composition

## Core Concepts

### Main Layout Component

```tsx
// File: src/components/DashboardLayout.tsx

import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Header from './Header';

export function DashboardLayout() {
  const [sidebarOpen, setSidebarOpen] = useState(true);

  return (
    <div className="dashboard">
      <Sidebar isOpen={sidebarOpen} onToggle={() => setSidebarOpen(!sidebarOpen)} />
      
      <div className={`main ${sidebarOpen ? 'sidebar-open' : 'sidebar-closed'}`}>
        <Header onMenuClick={() => setSidebarOpen(!sidebarOpen)} />
        
        <main className="content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
```

```css
/* File: src/components/DashboardLayout.css */

.dashboard {
  display: grid;
  grid-template-columns: auto 1fr;
  min-height: 100vh;
}

.main {
  display: flex;
  flex-direction: column;
  transition: margin-left 0.3s ease;
}

.main.sidebar-open {
  margin-left: 250px;
}

.main.sidebar-closed {
  margin-left: 64px;
}

.content {
  flex: 1;
  padding: 24px;
  overflow-y: auto;
}

@media (max-width: 768px) {
  .main.sidebar-open,
  .main.sidebar-closed {
    margin-left: 0;
  }
}
```

## Key Takeaways

- Use CSS Grid for layout
- Manage sidebar state with useState
- Make layout responsive
- Use Outlet for nested routes

## What's Next

Continue to [Charts with Recharts](/11-real-world-projects/02-dashboard-app/02-charts-with-recharts.md) to learn about data visualization.