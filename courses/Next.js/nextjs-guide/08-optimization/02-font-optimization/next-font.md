# Next.js Font Optimization

## Using next/font

```typescript
// src/app/layout.tsx
import { Inter } from "next/font/google";

const inter = Inter({ subsets: ["latin"] });

export default function Layout({ children }) {
  return (
    <html lang="en">
      <body className={inter.className}>
        {children}
      </body>
    </html>
  );
}
```

## Summary

- Automatic font optimization
- No layout shift (CLS)
- Self-hosted automatically
