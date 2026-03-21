# CSS-in-JS Limitations in App Router

## Key Limitations

1. **No SSR by default** - Most CSS-in-JS libraries need configuration
2. **Requires "use client"** - Can't use in Server Components
3. **Performance overhead** - Runtime style generation

## Recommended Alternatives

- **Tailwind CSS** - Built into Next.js
- **CSS Modules** - Zero runtime, works in Server Components
- **Global CSS** - For global styles

## Best Practice

Use CSS Modules or Tailwind for best performance:

```typescript
// Server Component - use CSS Modules
import styles from "./page.module.css";

export default function Page() {
  return <div className={styles.container}>Hello</div>;
}
```

```typescript
// Client Component - can use CSS-in-JS if needed
"use client";
import styled from "styled-components";
```
