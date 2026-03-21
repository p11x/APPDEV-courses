# Shared Layouts in Next.js

## What You'll Learn
- How layouts wrap pages
- Creating layouts that share UI across routes
- Understanding the layout hierarchy

## Prerequisites
- Understanding of layout.tsx
- Basic React component knowledge

## Concept Explained Simply

A **shared layout** is a layout component that wraps multiple pages. When you create a layout in a folder, all pages inside that folder (and its subfolders) share that layout. The layout persists as you navigate between those pages — it doesn't re-render, only the page content changes.

Think of it like a picture frame: you can swap out different pictures (pages), but the frame (layout) stays the same. The frame provides the structure around all your pictures, just like a layout provides the structure around all your pages.

This is perfect for sidebars, navigation menus, headers, and footers that should stay visible while users move between pages.

## How Layouts Work

```
src/app/
├── layout.tsx              ← Root layout (wraps everything)
│   ├── page.tsx            ← Homepage
│   ├── about/
│   │   ├── layout.tsx      ← About layout (wraps about pages)
│   │   └── page.tsx        ← About page
│   └── dashboard/
│       ├── layout.tsx      ← Dashboard layout
│       └── page.tsx        ← Dashboard page
```

When you visit `/dashboard`:
1. Root layout renders (HTML, body, global elements)
2. Dashboard layout renders inside root layout
3. Dashboard page renders inside dashboard layout

## Complete Code Example

Let's build a dashboard with a shared layout:

```typescript
// src/app/layout.tsx - Root layout
import Link from "next/link";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html>
      <body>
        <div style={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}>
          <header style={{ backgroundColor: "#333", color: "white", padding: "1rem" }}>
            <nav style={{ display: "flex", gap: "1rem" }}>
              <Link href="/" style={{ color: "white" }}>Home</Link>
              <Link href="/dashboard" style={{ color: "white" }}>Dashboard</Link>
              <Link href="/settings" style={{ color: "white" }}>Settings</Link>
            </nav>
          </header>
          {children}
          <footer style={{ backgroundColor: "#eee", padding: "1rem", marginTop: "auto" }}>
            <p>© 2024 My App</p>
          </footer>
        </div>
      </body>
    </html>
  );
}
```

```typescript
// src/app/dashboard/layout.tsx - Dashboard-specific layout
import Link from "next/navigation";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div style={{ display: "flex", minHeight: "calc(100vh - 100px)" }}>
      {/* Sidebar */}
      <aside style={{ width: "250px", backgroundColor: "#f5f5f5", padding: "1rem" }}>
        <h3 style={{ marginBottom: "1rem" }}>Dashboard</h3>
        <nav style={{ display: "flex", flexDirection: "column", gap: "0.5rem" }}>
          <Link href="/dashboard" style={{ padding: "0.5rem", backgroundColor: "white", borderRadius: "4px", textDecoration: "none" }}>
            Overview
          </Link>
          <Link href="/dashboard/analytics" style={{ padding: "0.5rem", backgroundColor: "white", borderRadius: "4px", textDecoration: "none" }}>
            Analytics
          </Link>
          <Link href="/dashboard/reports" style={{ padding: "0.5rem", backgroundColor: "white", borderRadius: "4px", textDecoration: "none" }}>
            Reports
          </Link>
        </nav>
      </aside>
      
      {/* Main content */}
      <main style={{ flex: 1, padding: "2rem" }}>
        {children}
      </main>
    </div>
  );
}
```

```typescript
// src/app/dashboard/page.tsx - Dashboard homepage
export default function DashboardPage() {
  return (
    <section>
      <h1>Dashboard Overview</h1>
      <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: "1rem", marginTop: "2rem" }}>
        <div style={{ padding: "1.5rem", backgroundColor: "#e3f2fd", borderRadius: "8px" }}>
          <h3>Total Users</h3>
          <p style={{ fontSize: "2rem", margin: 0 }}>1,234</p>
        </div>
        <div style={{ padding: "1.5rem", backgroundColor: "#e8f5e9", borderRadius: "8px" }}>
          <h3>Revenue</h3>
          <p style={{ fontSize: "2rem", margin: 0 }}>$56,789</p>
        </div>
        <div style={{ padding: "1.5rem", backgroundColor: "#fff3e0", borderRadius: "8px" }}>
          <h3>Active Now</h3>
          <p style={{ fontSize: "2rem", margin: 0 }}>89</p>
        </div>
      </div>
    </section>
  );
}
```

```typescript
// src/app/dashboard/analytics/page.tsx - Analytics sub-page
export default function AnalyticsPage() {
  return (
    <section>
      <h1>Analytics</h1>
      <p>View your website analytics and performance metrics.</p>
      <div style={{ marginTop: "2rem", padding: "2rem", backgroundColor: "#f5f5f5", borderRadius: "8px" }}>
        <p>Analytics charts would go here...</p>
      </div>
    </section>
  );
}
```

## Layout Hierarchy Visualized

```
Root Layout (layout.tsx)
├── Global Header
├── ├── Dashboard Layout (dashboard/layout.tsx)
│   │   ├── Dashboard Sidebar
│   │   ├── ├── Dashboard Page (page.tsx)      ← /dashboard
│   │   │   └── Analytics Page                  ← /dashboard/analytics
│   │   └── Reports Page                        ← /dashboard/reports
│   └── Other pages...
└── Global Footer
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `layout.tsx` | Layout file | Wraps pages in its folder |
| `{children}` | Page content | Where the page renders |
| `<aside>` | Sidebar element | Dashboard navigation |
| `<main>` | Main content area | Where page content goes |

## Sharing State in Layouts

Layouts can share data using Server Components:

```typescript
// src/app/dashboard/layout.tsx
import { getUser } from "@/lib/auth";

export default async function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const user = await getUser(); // Fetch user on the server

  return (
    <div style={{ display: "flex" }}>
      <aside>
        <p>Welcome, {user.name}</p>
        {/* Sidebar content */}
      </aside>
      <main>{children}</main>
    </div>
  );
}
```

## Common Mistakes

### Mistake #1: Forgetting Children

```typescript
// ✗ Wrong: Layout without children
export default function Layout() {
  return <div>No content!</div>;
}

// ✓ Correct: Render children
export default function Layout({ children }: { children: React.ReactNode }) {
  return <div>{children}</div>;
}
```

### Mistake #2: Breaking the Layout Chain

Make sure every nested level has its layout if needed:

```typescript
// If you have nested routes, ensure layouts exist:
src/app/
├── layout.tsx          // Root - must exist
├── page.tsx
└── dashboard/
    ├── layout.tsx      // Dashboard section - must exist if you want it
    └── page.tsx
```

### Mistake #3: Client Components in Layouts

Layouts are Server Components by default. If you need interactivity:

```typescript
// Use "use client" for interactive layouts
"use client";

import { useState } from "react";

export default function InteractiveLayout({ children }: { children: React.ReactNode }) {
  const [sidebarOpen, setSidebarOpen] = useState(true);
  
  return (
    <div>
      <button onClick={() => setSidebarOpen(!sidebarOpen)}>Toggle</button>
      {children}
    </div>
  );
}
```

## Summary

- Layouts wrap pages and persist across navigation
- Create a `layout.tsx` file in any folder to add a layout
- Layouts nest: root → section → page
- Layouts can fetch data on the server and share it with pages
- Use `"use client"` if the layout needs interactivity

## Next Steps

Now let's learn about parallel routes:

- [Parallel Routes →](./parallel-routes.md)
