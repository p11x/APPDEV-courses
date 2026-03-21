# Static Metadata Object

## Basic Usage

```typescript
// src/app/layout.tsx
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "My Website",
  description: "Welcome to my website",
};

export default function Layout({ children }) {
  return <>{children}</>;
}
```

## Page-Specific Metadata

```typescript
// src/app/about/page.tsx
export const metadata = {
  title: "About Us - My Website",
  description: "Learn more about our company",
};
```
