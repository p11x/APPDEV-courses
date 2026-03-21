# Metadata Inheritance

Metadata automatically inherits from parent layouts.

```typescript
// src/app/layout.tsx - Root metadata
export const metadata = {
  title: "My Site",
  description: "My awesome site",
};

// src/app/about/page.tsx - Extends root
export const metadata = {
  title: "About - My Site", // Overrides title
  // description inherits from root
};
```

## Overriding

```typescript
export const metadata = {
  title: {
    default: "Site Title",
    template: "%s | My Site",
  },
};
```
