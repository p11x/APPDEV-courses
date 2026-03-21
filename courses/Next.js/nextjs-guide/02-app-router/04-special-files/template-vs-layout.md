# Template vs Layout in Next.js

## What You'll Learn
- The difference between templates and layouts
- When to use each one
- Understanding the rendering behavior

## Prerequisites
- Understanding of layouts
- Basic React component knowledge

## Concept Explained Simply

Both `layout.tsx` and `template.tsx` wrap your pages, but they behave differently. The key difference is: **layouts don't re-render when navigating between pages, but templates do**.

Think of it like a movie theater:
- **Layout** = The theater building itself (stays the same for every show)
- **Template** = The screen and projector setup (gets reset for each movie)

When you navigate between pages, layouts stay mounted while pages change inside them. Templates get completely remounted — they unmount and mount again.

## When to Use Layouts

- Headers and footers that persist
- Sidebars that don't change
- Authentication state that stays
- Any UI that should remain stable during navigation

## When to Use Templates

- Page entrance animations (run on every visit)
- Forms that should reset when leaving and returning
- Any UI that needs to "start fresh" on each visit

## Complete Code Example

Let's build a page with both layout and template:

```typescript
// src/app/layout.tsx - Persistent across navigation
import Link from "next/link";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  console.log("Layout rendered!"); // Only once (mostly)
  
  return (
    <html>
      <body>
        <header style={{ padding: "1rem", borderBottom: "1px solid #ddd" }}>
          <nav style={{ display: "flex", gap: "1rem" }}>
            <Link href="/">Home</Link>
            <Link href="/page-a">Page A</Link>
            <Link href="/page-b">Page B</Link>
          </nav>
        </header>
        {children}
        <footer style={{ padding: "1rem", borderTop: "1px solid #ddd" }}>
          Footer - Same on every page!
        </footer>
      </body>
    </html>
  );
}
```

```typescript
// src/app/template.tsx - Remounts on navigation
"use client";

import { useState, useEffect } from "react";

export default function Template({
  children,
}: {
  children: React.ReactNode;
}) {
  const [mounted, setMounted] = useState(false);
  
  useEffect(() => {
    console.log("Template mounted!"); // Runs on EVERY navigation
    setMounted(true);
  }, []);

  return (
    <div style={{ padding: "2rem" }}>
      {mounted && (
        <div
          style={{
            animation: "fadeIn 0.5s ease-in",
          }}
        >
          {children}
        </div>
      )}
      <style>{`
        @keyframes fadeIn {
          from { opacity: 0; transform: translateY(10px); }
          to { opacity: 1; transform: translateY(0); }
        }
      `}</style>
    </div>
  );
}
```

```typescript
// src/app/page-a/page.tsx
export default function PageA() {
  console.log("Page A rendered!"); // Runs when visiting
  
  return (
    <main>
      <h1>Page A</h1>
      <p>Go to another page and notice how this unmounts!</p>
    </main>
  );
}
```

## Visual Comparison

```
User goes from /page-a to /page-b

Layout:
  ✓ Stays mounted (console.log runs once)
  ✓ Header/footer persist
  ✓ State is preserved

Template:
  ✓ Remounts (console.log runs again)
  ✓ Animation plays
  ✓ State resets
```

## Use Case: Form with Reset

Templates are perfect for forms that should reset:

```typescript
// Without template - layout keeps form state
// src/app/contact/layout.tsx
export default function ContactLayout({ children }: { children: React.ReactNode }) {
  const [formData, setFormData] = useState({}); // Stays between navigations!
  
  return (
    <form>
      {children}
    </form>
  );
}

// With template - form resets on each visit
// src/app/contact/template.tsx
"use client";

export default function ContactTemplate({ children }: { children: React.ReactNode }) {
  // This runs fresh every time the user visits /contact
  // Perfect for forms that should reset!
  
  return (
    <form>
      {children}
    </form>
  );
}
```

## Use Case: Entrance Animations

```typescript
// src/app/dashboard/template.tsx
"use client";

import { useEffect, useState } from "react";

export default function DashboardTemplate({
  children,
}: {
  children: React.ReactNode;
}) {
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    // Trigger animation on mount
    setIsVisible(true);
  }, []);

  return (
    <div
      style={{
        opacity: isVisible ? 1 : 0,
        transform: isVisible ? "translateY(0)" : "translateY(20px)",
        transition: "all 0.5s ease-out",
      }}
    >
      {children}
    </div>
  );
}
```

## Comparison Table

| Feature | Layout | Template |
|---------|--------|----------|
| Re-renders on navigation | ❌ No | ✅ Yes |
| Preserves state | ✅ Yes | ❌ No |
| Entrance animations | ❌ No | ✅ Yes |
| Persistent UI | ✅ Yes | ❌ No |
| Use for | Headers, sidebars, footers | Forms, animations, fresh starts |

## Common Mistakes

### Mistake #1: Using Template When Layout Is Better

```typescript
// ✗ Wrong: Template for persistent header
// src/app/template.tsx
export default function Template({ children }) {
  return (
    <header>My Header</header>  // Recreates on every navigation!
  );
}

// ✓ Correct: Use layout for persistent header
// src/app/layout.tsx
export default function Layout({ children }) {
  return (
    <header>My Header</header>
  );
}
```

### Mistake #2: Forgetting Template Is Client Component

```typescript
// ✗ Wrong: Template with server features won't work
// src/app/template.tsx
export default async function Template({ children }) {
  const data = await fetchData(); // Can't do this!
  return <div>{children}</div>;
}

// ✓ Correct: Template is client component
// src/app/template.tsx
"use client";

import { useState } from "react";

export default function Template({ children }) {
  return <div>{children}</div>;
}
```

### Mistake #3: Not Understanding State Behavior

```typescript
// In layout.tsx - state persists
export default function Layout({ children }) {
  const [count, setCount] = useState(0); // Stays when navigating!
  return (
    <button onClick={() => setCount(c => c + 1)}>
      Count: {count}
    </button>
  );
}

// In template.tsx - state resets
export default function Template({ children }) {
  const [count, setCount] = useState(0); // Resets on every visit!
  return (
    <button onClick={() => setCount(c => c + 1)}>
      Count: {count}
    </button>
  );
}
```

## Summary

- Layouts persist across navigation; templates remount
- Use layouts for headers, sidebars, footers
- Use templates for forms, animations, fresh state
- Templates are client components; layouts can be server components
- Understanding this difference helps you build better UX

## Next Steps

Now let's dive into React Server Components:

- [Server vs Client Components →](../../03-react-server-components/01-server-vs-client/mental-model.md)
