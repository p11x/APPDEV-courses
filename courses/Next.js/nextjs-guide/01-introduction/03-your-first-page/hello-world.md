# Hello World: Your First Next.js Page

## What You'll Learn
- How to create a simple page in Next.js
- The difference between server and client components
- How to see your changes in real-time

## Prerequisites
- A Next.js project set up (from previous pages)
- Basic understanding of React components

## Concept Explained Simply

Creating a page in Next.js is incredibly simple — you just create a file. That's it! There's no router to configure, no complicated setup. Just create a file and it becomes a page.

The file you create must be named `page.tsx`. This is a special filename that Next.js recognizes. Any file named `page.tsx` inside the `app` folder automatically becomes a route.

Inside this file, you export a React component. This component is what gets rendered when someone visits that route. It's just like writing regular React — you can use JSX, props, and all the React patterns you know.

## Complete Code Example

Let's create a simple "Hello World" page. Open `src/app/page.tsx` and replace its contents with this:

```typescript
// src/app/page.tsx
export default function HomePage() {
  return (
    <div style={{ 
      display: 'flex', 
      flexDirection: 'column',
      alignItems: 'center', 
      justifyContent: 'center',
      minHeight: '100vh',
      fontFamily: 'system-ui, sans-serif'
    }}>
      <h1 style={{ fontSize: '3rem', marginBottom: '1rem' }}>
        Hello, World! 👋
      </h1>
      <p style={{ fontSize: '1.25rem', color: '#666' }}>
        Welcome to my Next.js app
      </p>
      <button 
        onClick={() => alert('You clicked me!')}
        style={{
          marginTop: '2rem',
          padding: '0.75rem 1.5rem',
          fontSize: '1rem',
          backgroundColor: '#0070f3',
          color: 'white',
          border: 'none',
          borderRadius: '5px',
          cursor: 'pointer'
        }}
      >
        Click Me
      </button>
    </div>
  );
}
```

Now let's create a new page at `/about`:

```typescript
// src/app/about/page.tsx
export default function AboutPage() {
  return (
    <main style={{ padding: '2rem' }}>
      <h1>About Me</h1>
      <p>
        Hi! I'm learning Next.js. This is my first website
        built with the App Router.
      </p>
      <p>
        Next.js makes it so easy to create pages. 
        I just create a file and it works!
      </p>
    </main>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `export default function HomePage()` | Exports the default component | Next.js uses this as the page content |
| `return (` | Returns JSX | The HTML-like syntax React uses |
| `<div style={{ ... }}>` | Inline styles | Quick styling without CSS files |
| `<h1>Hello, World!</h1>` | Heading element | The main title of the page |
| `<button onClick={...}>` | Button with click handler | Note: This makes it a Client Component |

## Server vs Client Components

Here's an important concept: by default, all components in the App Router are **Server Components**. They run only on the server and never send JavaScript to the browser.

However, when you use interactive features like `onClick`, `useState`, or `useEffect`, you need to mark the component as a **Client Component** using `"use client"`:

```typescript
// src/app/counter/page.tsx
"use client";  // ← This makes it a Client Component

import { useState } from "react";

export default function CounterPage() {
  const [count, setCount] = useState(0);

  return (
    <div style={{ padding: "2rem" }}>
      <h1>Counter: {count}</h1>
      <button onClick={() => setCount(count + 1)}>
        Increment
      </button>
    </div>
  );
}
```

## Common Mistakes

### Mistake #1: Forgetting to Export Default

```typescript
// ✗ Wrong: Missing export
function HomePage() {
  return <h1>Hello</h1>;
}

// ✓ Correct: Export default
export default function HomePage() {
  return <h1>Hello</h1>;
}
```

### Mistake #2: Using Client Features Without "use client"

```typescript
// ✗ Wrong: Using useState without "use client"
import { useState } from "react";

export default function Page() {
  const [count, setCount] = useState(0); // Error!
  return <button onClick={() => setCount(c => c + 1)}>Click</button>;
}

// ✓ Correct: Add "use client" at the top
"use client";

import { useState } from "react";

export default function Page() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(c => c + 1)}>Click</button>;
}
```

### Mistake #3: Wrong File Location

```typescript
// ✗ Wrong: Putting page.tsx in the wrong folder
src/pages/index.tsx  // This is Pages Router!

// ✓ Correct: App Router location
src/app/page.tsx
```

## Summary

- Create a file named `page.tsx` inside the `app` folder to make a page
- By default, pages are Server Components (no JavaScript sent to browser)
- Add `"use client"` when you need interactivity (onClick, useState, useEffect)
- Use inline styles for quick prototyping
- File location determines the URL path

## Next Steps

Let's learn how routing works in more detail:

- [Understanding File-Based Routing →](./understanding-file-based-routing.md)
