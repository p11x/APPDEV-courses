# Charts with Recharts

## Overview

Recharts is a composable charting library built on React components. It provides simple, responsive charts with great defaults while allowing full customization.

## Prerequisites

- React basics
- Understanding of data visualization

## Core Concepts

### Installing Recharts

```bash
npm install recharts
```

### Line Chart Example

```tsx
// File: src/components/RevenueChart.tsx

import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

interface DataPoint {
  date: string;
  revenue: number;
  expenses: number;
}

export function RevenueChart({ data }: { data: DataPoint[] }) {
  return (
    <ResponsiveContainer width="100%" height={300}>
      <LineChart data={data}>
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="date" />
        <YAxis />
        <Tooltip />
        <Line type="monotone" dataKey="revenue" stroke="#3b82f6" />
        <Line type="monotone" dataKey="expenses" stroke="#ef4444" />
      </LineChart>
    </ResponsiveContainer>
  );
}
```

### Bar Chart Example

```tsx
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

export function SalesChart({ data }) {
  return (
    <ResponsiveContainer width="100%" height={300}>
      <BarChart data={data}>
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="month" />
        <YAxis />
        <Tooltip />
        <Bar dataKey="sales" fill="#3b82f6" />
      </BarChart>
    </ResponsiveContainer>
  );
}
```

## Key Takeaways

- Use ResponsiveContainer for fluid sizing
- Customize colors, labels, and tooltips
- Compose multiple chart types
- Handle loading and empty states

## What's Next

Continue to [Real-time Data with WebSockets](/11-real-world-projects/02-dashboard-app/03-real-time-data-with-websockets.md) to learn about live data updates.