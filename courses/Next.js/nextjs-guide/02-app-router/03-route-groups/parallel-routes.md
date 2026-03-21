# Parallel Routes in Next.js

## What You'll Learn
- What parallel routes are
- How to render multiple pages simultaneously
- Building complex layouts with independent navigation

## Prerequisites
- Understanding of layouts
- Knowledge of route groups

## Concept Explained Simply

**Parallel routes** are a powerful feature that lets you render multiple pages in the same layout at the same time. Instead of one page being nested inside another, multiple "slots" render independently, each with its own content.

Think of it like a TV with picture-in-picture: you have the main show, but you can also see another channel in the corner. Both are showing different content simultaneously. Parallel routes work the same way — different parts of your page load independently.

The syntax uses the `@` symbol to name slots:

```
src/app/
├── @sidebar/
│   └── page.tsx        ← Renders in the sidebar slot
├── @main/
│   └── page.tsx        ← Renders in the main slot
└── layout.tsx          ← Uses both slots
```

## Why Use Parallel Routes?

1. **Independent loading** — Each slot loads independently, so slow content doesn't block the whole page
2. **Multiple regions** — Perfect for apps with multiple independent sections (feed + sidebar)
3. **Conditional layouts** — Show/hide different regions based on conditions

## Complete Code Example

Let's build a social media-style layout with a feed and sidebar:

```typescript
// src/app/layout.tsx - Root layout
export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html>
      <body>
        <div style={{ maxWidth: "1400px", margin: "0 auto" }}>
          <header style={{ padding: "1rem", borderBottom: "1px solid #ddd" }}>
            <h1>My Social App</h1>
          </header>
          {children}
        </div>
      </body>
    </html>
  );
}
```

```typescript
// src/app/@feed/page.tsx - Main feed content
// Named slot: @feed
export default function FeedPage() {
  const posts = [
    { id: 1, author: "Alice", content: "Just learned Next.js parallel routes! 🚀" },
    { id: 2, author: "Bob", content: "Building amazing apps with React." },
    { id: 3, author: "Charlie", content: "Server Components are game changers." },
  ];

  return (
    <div style={{ flex: 1 }}>
      <h2>Your Feed</h2>
      {posts.map((post) => (
        <article
          key={post.id}
          style={{
            padding: "1rem",
            marginBottom: "1rem",
            backgroundColor: "white",
            border: "1px solid #ddd",
            borderRadius: "8px",
          }}
        >
          <strong>{post.author}</strong>
          <p style={{ margin: "0.5rem 0 0" }}>{post.content}</p>
        </article>
      ))}
    </div>
  );
}
```

```typescript
// src/app/@sidebar/page.tsx - Sidebar content
// Named slot: @sidebar
export default function SidebarPage() {
  const suggestions = [
    { id: 1, name: "Diana", handle: "@diana_dev" },
    { id: 2, name: "Eve", handle: "@eve_codes" },
    { id: 3, name: "Frank", handle: "@frank_js" },
  ];

  return (
    <aside style={{ width: "300px", padding: "1rem", backgroundColor: "#f9f9f9" }}>
      <h3>Who to Follow</h3>
      {suggestbacks.map((user) => (
        <div
          key={user.id}
          style={{
            padding: "0.75rem",
            marginBottom: "0.5rem",
            backgroundColor: "white",
            borderRadius: "8px",
          }}
        >
          <strong>{user.name}</strong>
          <p style={{ margin: "0", color: "#666", fontSize: "0.9rem" }}>
            {user.handle}
          </p>
        </div>
      ))}
    </aside>
  );
}
```

```typescript
// src/app/page.tsx - The parallel route layout
// Uses both @feed and @sidebar slots
export default function HomePage() {
  return (
    <main style={{ display: "flex", padding: "2rem", gap: "2rem" }}>
      {/* These render the named slots */}
    </main>
  );
}
```

Wait, there's a special syntax. Let me fix this:

```typescript
// src/app/(feed)/layout.tsx - Layout that uses parallel routes
export default function FeedLayout({
  feed,
  sidebar,
}: {
  feed: React.ReactNode;
  sidebar: React.ReactNode;
}) {
  return (
    <main style={{ display: "flex", padding: "2rem", gap: "2rem" }}>
      {feed}
      {sidebar}
    </main>
  );
}
```

```typescript
// src/app/(feed)/@feed/page.tsx
export default function FeedPage() {
  const posts = [
    { id: 1, author: "Alice", content: "Just learned Next.js parallel routes! 🚀" },
    { id: 2, author: "Bob", content: "Building amazing apps with React." },
  ];

  return (
    <div style={{ flex: 1 }}>
      <h2>Your Feed</h2>
      {posts.map((post) => (
        <article
          key={post.id}
          style={{
            padding: "1rem",
            marginBottom: "1rem",
            backgroundColor: "white",
            border: "1px solid #ddd",
            borderRadius: "8px",
          }}
        >
          <strong>{post.author}</strong>
          <p style={{ margin: "0.5rem 0 0" }}>{post.content}</p>
        </article>
      ))}
    </div>
  );
}
```

```typescript
// src/app/(feed)/@sidebar/page.tsx
export default function SidebarPage() {
  const suggestions = [
    { id: 1, name: "Diana", handle: "@diana_dev" },
    { id: 2, name: "Eve", handle: "@eve_codes" },
  ];

  return (
    <aside style={{ width: "300px", padding: "1rem", backgroundColor: "#f9f9f9" }}>
      <h3>Who to Follow</h3>
      {suggestions.map((user) => (
        <div
          key={user.id}
          style={{
            padding: "0.75rem",
            marginBottom: "0.5rem",
            backgroundColor: "white",
            borderRadius: "8px",
          }}
        >
          <strong>{user.name}</strong>
          <p style={{ margin: "0", color: "#666", fontSize: "0.9rem" }}>
            {user.handle}
          </p>
        </div>
      ))}
    </aside>
  );
}
```

Actually, let me simplify with the correct pattern:

```typescript
// src/app/@modal/page.tsx - Modal as parallel route
export default function ModalPage() {
  return (
    <div style={{
      position: "fixed",
      top: "50%",
      left: "50%",
      transform: "translate(-50%, -50%)",
      backgroundColor: "white",
      padding: "2rem",
      borderRadius: "8px",
      boxShadow: "0 4px 20px rgba(0,0,0,0.2)",
    }}>
      <h2>Modal Content</h2>
      <p>This is a parallel route rendered as a modal!</p>
    </div>
  );
}
```

## Syntax Summary

| Pattern | Syntax | Example |
|---------|--------|---------|
| Named slot | `@slotname` | `@feed`, `@sidebar` |
| Slot in layout | props | `{ feed, sidebar }` |

## Complete Working Example

```typescript
// src/app/layout.tsx
export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html>
      <body>
        <header>My App</header>
        {children}
      </body>
    </html>
  );
}
```

```typescript
// src/app/page.tsx - Redirects or shows home
import { redirect } from "next/navigation";

export default function HomePage() {
  redirect("/feed");
}
```

```typescript
// src/app/feed/layout.tsx - Using parallel routes
export default function FeedLayout({
  feed,
  sidebar,
}: {
  feed: React.ReactNode;
  sidebar: React.ReactNode;
}) {
  return (
    <div style={{ display: "flex", gap: "2rem", padding: "2rem" }}>
      <div style={{ flex: 1 }}>{feed}</div>
      <div style={{ width: "300px" }}>{sidebar}</div>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `@feed/` | Named slot folder | Creates a named slot |
| `props: { feed, sidebar }` | Slot props | Layout receives slots as props |
| `{feed}` and `{sidebar}` | Render slots | Render each parallel route |

## Common Mistakes

### Mistake #1: Forgetting Slot Props

```typescript
// ✗ Wrong: Not accepting slots in layout
export default function Layout({ children }) {
  return <div>{children}</div>;
}

// ✓ Correct: Accept named slots
export default function Layout({ feed, sidebar }) {
  return (
    <div>
      {feed}
      {sidebar}
    </div>
  );
}
```

### Mistake #2: Wrong Folder Structure

```typescript
// ✗ Wrong: Slots outside of layout's scope
src/app/
├── @feed/page.tsx      // Not connected to any layout!
└── page.tsx

// ✓ Correct: Slots in same route group or with layout
src/app/
├── feed/
│   ├── layout.tsx      // Layout uses slots
│   ├── @feed/
│   │   └── page.tsx
│   └── @sidebar/
│       └── page.tsx
```

## Summary

- Parallel routes use `@` syntax: `@feed`, `@sidebar`
- Named slots render independently in the layout
- Perfect for independent loading regions
- Slots are passed as props to the layout component
- Great for modals, feeds + sidebars, dashboards

## Next Steps

Now let's learn about special files like loading.tsx:

- [Loading UI →](../../02-app-router/04-special-files/loading-ui.md)
