# Global Styles in CSS Modules

## Global vs Scoped

```css
/* src/app/globals.css - Global styles */
* {
  box-sizing: border-box;
}

body {
  margin: 0;
  font-family: system-ui, sans-serif;
}
```

```css
/* src/app/page.module.css - Scoped styles */
.container {
  padding: 2rem;
}
```

Import globals.css in layout:

```typescript
// src/app/layout.tsx
import "./globals.css";

export default function Layout({ children }) {
  return <body>{children}</body>;
}
```
