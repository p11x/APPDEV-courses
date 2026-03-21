# Page and Layout Files in Next.js

## What You'll Learn
- How page.tsx creates route content
- How layout.tsx wraps pages with shared UI
- The relationship between pages and layouts

## Prerequisites
- Understanding of folder conventions
- Basic React component knowledge

## Concept Explained Simply

In the App Router, every route needs two main things: a **page** (the content) and often a **layout** (the wrapper around the content). Think of it like a picture frame: the layout is the frame that stays the same, while the page is the picture inside that changes.

When you navigate between pages in a Next.js app, the layout doesn't re-render — only the page content changes. This makes the app feel faster because the navigation elements don't blink or reload.

## The Layout File (layout.tsx)

A layout is a wrapper around one or more pages. It typically contains things that should persist across navigation: headers, sidebars, footers, navigation menus.

### Root Layout

Every Next.js app has a root layout at `src/app/layout.tsx`. This wraps ALL pages in your entire application:

```typescript
// src/app/layout.tsx
import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import Header from "@/components/Header";
import Footer from "@/components/Footer";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "My App",
  description: "A wonderful Next.js application",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <Header />
        <main style={{ minHeight: "calc(100vh - 200px)" }}>
          {children}
        </main>
        <Footer />
      </body>
    </html>
  );
}
```

### Nested Layouts

You can also create layouts for specific sections of your site:

```typescript
// src/app/dashboard/layout.tsx
// This layout only wraps pages in /dashboard
import DashboardNav from "@/components/DashboardNav";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div style={{ display: "flex" }}>
      <aside style={{ width: "250px", padding: "1rem" }}>
        <DashboardNav />
      </aside>
      <main style={{ flex: 1, padding: "1rem" }}>
        {children}
      </main>
    </div>
  );
}
```

## The Page File (page.tsx)

A page is the main content for a route. It can be a simple component or an async component that fetches data:

```typescript
// src/app/page.tsx (homepage)
export default function HomePage() {
  return (
    <main>
      <h1>Welcome to My App</h1>
      <p>This is the homepage content.</p>
    </main>
  );
}
```

```typescript
// src/app/dashboard/page.tsx
async function getData() {
  // This could fetch from a database or API
  return { userCount: 42, revenue: 1000 };
}

export default async function DashboardPage() {
  const data = await getData();

  return (
    <main>
      <h1>Dashboard</h1>
      <div className="stats">
        <div>Users: {data.userCount}</div>
        <div>Revenue: ${data.revenue}</div>
      </div>
    </main>
  );
}
```

## How Pages and Layouts Work Together

Here's the hierarchy:

```
src/app/
├── layout.tsx          ← Root layout (wraps everything)
│   ├── page.tsx         ← Homepage
│   ├── about/
│   │   ├── layout.tsx   ← About layout (wraps about pages)
│   │   └── page.tsx     ← About page
│   └── dashboard/
│       ├── layout.tsx   ← Dashboard layout
│       └── page.tsx     ← Dashboard page
```

When you visit `/dashboard`:
1. Root layout renders first
2. Dashboard layout renders inside root layout
3. Dashboard page renders inside dashboard layout

## Complete Code Example

Let's build a simple site with multiple pages and layouts:

```typescript
// src/app/layout.tsx (Root layout)
import Link from "next/link";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html>
      <body>
        <nav style={{ padding: "1rem", borderBottom: "1px solid #ccc" }}>
          <Link href="/" style={{ marginRight: "1rem" }}>Home</Link>
          <Link href="/about" style={{ marginRight: "1rem" }}>About</Link>
          <Link href="/dashboard">Dashboard</Link>
        </nav>
        {children}
      </body>
    </html>
  );
}
```

```typescript
// src/app/about/page.tsx
export default function AboutPage() {
  return (
    <main style={{ padding: "2rem" }}>
      <h1>About Us</h1>
      <p>We are awesome!</p>
    </main>
  );
}
```

```typescript
// src/app/dashboard/layout.tsx
export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div style={{ display: "flex", minHeight: "100vh" }}>
      <aside style={{ width: "200px", background: "#f5f5f5", padding: "1rem" }}>
        <h3>Dashboard</h3>
        <ul>
          <li>Overview</li>
          <li>Settings</li>
        </ul>
      </aside>
      <main style={{ flex: 1, padding: "2rem" }}>
        {children}
      </main>
    </div>
  );
}
```

```typescript
// src/app/dashboard/page.tsx
export default function DashboardPage() {
  return (
    <main>
      <h1>Dashboard Overview</h1>
      <p>Welcome to your dashboard!</p>
    </main>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `{children}` | Renders the nested content | Layouts wrap pages, children is the page |
| `<nav>`, `<aside>`, `<main>` | HTML semantic elements | Structure your layout semantically |
| `layout.tsx` | Special Next.js filename | Creates a layout wrapper |
| `page.tsx` | Special Next.js filename | Creates the page content |

## Common Mistakes

### Mistake #1: Forgetting Children Prop

```typescript
// ✗ Wrong: Layout must accept children
export default function Layout() {
  return <div>No children!</div>;
}

// ✓ Correct: Accept and render children
export default function Layout({ children }: { children: React.ReactNode }) {
  return <div>{children}</div>;
}
```

### Mistake #2: Putting Page Content in Layout

```typescript
// ✗ Wrong: Layout has hardcoded content instead of children
export default function Layout() {
  return (
    <div>
      <h1>Static Title</h1>
      <p>This is the page content</p>  // Should be {children}
    </div>
  );
}
```

### Mistake #3: Not Using Root Layout

Every app needs a root layout with `<html>` and `<body>`:

```typescript
// ✗ Wrong: Root layout missing html/body tags
export default function Layout({ children }: { children: React.ReactNode }) {
  return <div>{children}</div>;  // Invalid!
}

// ✓ Correct: Root layout has full HTML structure
export default function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html>
      <body>{children}</body>
    </html>
  );
}
```

## Summary

- Layouts wrap pages and persist across navigation
- Every route needs a `page.tsx` file for content
- The root layout (`app/layout.tsx`) wraps every page
- Nested layouts wrap pages in specific route groups
- Layouts receive `children` props that render the nested content

## Next Steps

Now let's learn about nested routes:

- [Nested Routes →](./nested-routes.md)
